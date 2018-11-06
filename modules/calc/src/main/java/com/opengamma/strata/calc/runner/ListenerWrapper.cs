using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{

	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Wrapper around a listener for thread-safety.
	/// <para>
	/// This is a wrapper around a <seealso cref="CalculationListener"/> that ensures the listener
	/// is only invoked by a single thread at a time. When the calculations are complete,
	/// it calls <seealso cref="CalculationListener#calculationsComplete() calculationsComplete"/>.
	/// </para>
	/// <para>
	/// Calculations may be performed in bulk for a given target.
	/// The logic in this class unwraps the <seealso cref="CalculationResults"/>, calling the
	/// listener with each individual <seealso cref="CalculationResult"/>.
	/// </para>
	/// </summary>
	internal sealed class ListenerWrapper : System.Action<CalculationResults>
	{

	  private static readonly Logger log = LoggerFactory.getLogger(typeof(ListenerWrapper));

	  /// <summary>
	  /// The wrapped listener. </summary>
	  private readonly CalculationListener listener;

	  /// <summary>
	  /// Queue of actions to perform on the delegate. </summary>
	  private readonly LinkedList<CalculationResults> queue = new LinkedList<CalculationResults>();

	  /// <summary>
	  /// Protects the queue and the executing flag. </summary>
	  private readonly Lock @lock = new ReentrantLock();

	  /// <summary>
	  /// This lock is never contended; it is used to guarantee the listener state is visible to all threads. </summary>
	  private readonly Lock listenerLock = new ReentrantLock();

	  /// <summary>
	  /// The total number of tasks to be executed. </summary>
	  private readonly int tasksExpected;

	  // Mutable state -----------------------------------------------------

	  /// <summary>
	  /// Flags whether a call to the underlying listener is executing.
	  /// If this flag is set when <seealso cref="#accept"/> is called, the result is added to
	  /// the queue and the calling thread returns. The executing thread will ensure
	  /// all queued results are delivered.
	  /// </summary>
	  private bool executing;

	  /// <summary>
	  /// The number of task results that have been received. </summary>
	  private int tasksReceived;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance wrapping the specified listener. </summary>
	  ///  <param name="listener">  the underlying listener wrapped by this object </param>
	  /// <param name="tasksExpected">  the number of tasks to be executed </param>
	  /// <param name="columns">  the columns for which values are being calculated </param>
	  internal ListenerWrapper(CalculationListener listener, int tasksExpected, IList<CalculationTarget> targets, IList<Column> columns)
	  {
		this.listener = ArgChecker.notNull(listener, "listener");
		this.tasksExpected = ArgChecker.notNegative(tasksExpected, "tasksExpected");

		listenerLock.@lock();
		try
		{
		  listener.calculationsStarted(targets, columns);

		  if (tasksExpected == 0)
		  {
			listener.calculationsComplete();
		  }
		}
		finally
		{
		  listenerLock.unlock();
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Accepts a calculation result and delivers it to the listener
	  /// <para>
	  /// This method can be invoked concurrently by multiple threads.
	  /// Only one of them will invoke the listener directly to ensure that
	  /// it is not accessed concurrently by multiple threads.
	  /// </para>
	  /// <para>
	  /// The other threads do not block while the listener is invoked. They
	  /// add their results to a queue and return quickly. Their results are
	  /// delivered by the thread invoking the listener.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="result"> the result of a calculation </param>
	  public override void accept(CalculationResults result)
	  {
		CalculationResults nextResult;

		// Multiple calculation threads can try to acquire this lock at the same time.
		// The thread which acquires the lock will set the executing flag and proceed into
		// the body of the method.
		// If another thread acquires the lock while the first thread is executing it will
		// add an item to the queue and return.
		// The lock also ensures the state of the executing flag and the queue are visible
		// to any thread acquiring the lock.
		@lock.@lock();
		try
		{
		  if (executing)
		  {
			// Another thread is already invoking the listener. Add the result to
			// the queue and return. The other thread will ensure the queued results
			// are delivered.
			queue.AddLast(result);
			return;
		  }
		  else
		  {
			// There is no thread invoking the listener. Set the executing flag to
			// ensure no other thread passes this point and invoke the listener.
			executing = true;
			nextResult = result;
		  }
		}
		finally
		{
		  @lock.unlock();
		}

		// The logic in the block above guarantees that there will never be more than one thread in the
		// rest of the method below this point.

		// Loop until the nextResult and all the results from the queue have been delivered
		for (;;)
		{
		  // The logic above means this lock is never contended; the executing flag means
		  // only one thread will ever be in this loop at any given time.
		  // This lock is required to ensure any state changes in the listener are visible to all threads
		  listenerLock.@lock();
		  try
		  {
			// Invoke the listener while not protected by lock. This allows other threads
			// to queue results while this thread is delivering them to the listener.
			foreach (CalculationResult cell in nextResult.Cells)
			{
			  listener.resultReceived(nextResult.Target, cell);
			}
		  }
		  catch (Exception e)
		  {
			log.warn("Exception invoking listener.resultReceived", e);
		  }
		  finally
		  {
			listenerLock.unlock();
		  }

		  // The following code must be executed whilst holding the lock to guarantee any changes
		  // to the executing flag and to the state of the queue are visible to all threads
		  @lock.@lock();
		  try
		  {
			if (++tasksReceived == tasksExpected)
			{
			  // The expected number of results have been received, inform the listener.
			  // The listener lock must be acquired to ensure any state changes in the listener are
			  // visible to all threads
			  listenerLock.@lock();
			  try
			  {
				listener.calculationsComplete();
			  }
			  catch (Exception e)
			  {
				log.warn("Exception invoking listener.calculationsComplete", e);
			  }
			  finally
			  {
				listenerLock.unlock();
			  }
			  return;
			}
			else if (queue.Count == 0)
			{
			  // There are no more results to deliver. Unset the executing flag and return.
			  // This allows the next calling thread to deliver results.
			  executing = false;
			  return;
			}
			else
			{
			  // There are results on the queue. This means another thread called accept(),
			  // added a result to the queue and returned while this thread was invoking the listener.
			  // This thread must deliver the results from the queue.
			  nextResult = queue.RemoveFirst();
			}
		  }
		  finally
		  {
			@lock.unlock();
		  }
		}
	  }
	}

}