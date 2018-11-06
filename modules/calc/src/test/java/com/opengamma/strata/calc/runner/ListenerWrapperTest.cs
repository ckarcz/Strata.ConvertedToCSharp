using System.Collections.Generic;
using System.Threading;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.fail;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ListenerWrapperTest
	public class ListenerWrapperTest
	{

	  // Tests that a listener is only invoked by a single thread at any time even if multiple threads are
	  // invoking the wrapper concurrently.
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void concurrentExecution() throws InterruptedException
	  public virtual void concurrentExecution()
	  {
		int nThreads = Runtime.Runtime.availableProcessors();
		int resultsPerThread = 10;
		ConcurrentLinkedQueue<string> errors = new ConcurrentLinkedQueue<string>();
		System.Threading.CountdownEvent latch = new System.Threading.CountdownEvent(1);
		int expectedResultCount = nThreads * resultsPerThread;
		Listener listener = new Listener(errors, latch);
		System.Action<CalculationResults> wrapper = new ListenerWrapper(listener, expectedResultCount, ImmutableList.of(), ImmutableList.of());
		ExecutorService executor = Executors.newFixedThreadPool(nThreads);
		CalculationResult result = CalculationResult.of(0, 0, Result.failure(FailureReason.ERROR, "foo"));
		CalculationTarget target = new CalculationTargetAnonymousInnerClass(this);
		CalculationResults results = CalculationResults.of(target, ImmutableList.of(result));
		IntStream.range(0, expectedResultCount).forEach(i => executor.submit(() => wrapper(results)));

		latch.await();
		executor.shutdown();

		if (!errors.Empty)
		{
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		  string allErrors = errors.collect(joining("\n"));
		  fail(allErrors);
		}
	  }

	  private class CalculationTargetAnonymousInnerClass : CalculationTarget
	  {
		  private readonly ListenerWrapperTest outerInstance;

		  public CalculationTargetAnonymousInnerClass(ListenerWrapperTest outerInstance)
		  {
			  this.outerInstance = outerInstance;
		  }

	  }

	  public sealed class Listener : CalculationListener
	  {

		/// <summary>
		/// Calling fail() on a different thread from the one running the test won't cause the test to fail.
		/// If any failures occur in the listener the failure message is put on this queue.
		/// The test can check the queue at the end of the test and fail if it is non-empty.
		/// </summary>
		internal readonly LinkedList<string> errors;

		/// <summary>
		/// Latch that prevents the test method returning until all calculations have completed. </summary>
		internal readonly System.Threading.CountdownEvent latch;

		/// <summary>
		/// The name of the thread currently invoking this listener. </summary>
		internal volatile string threadName;

		public Listener(LinkedList<string> errors, System.Threading.CountdownEvent latch)
		{
		  this.errors = errors;
		  this.latch = latch;
		}

		public void resultReceived(CalculationTarget target, CalculationResult result)
		{
		  if (!string.ReferenceEquals(threadName, null))
		  {
			errors.AddLast("Expected threadName to be null but it was " + threadName);
		  }
		  threadName = Thread.CurrentThread.Name;

		  try
		  {
			// Give other threads a chance to get into this method
			Thread.Sleep(5);
		  }
		  catch (InterruptedException)
		  {
			// Won't ever happen
		  }
		  threadName = null;
		}

		public void calculationsComplete()
		{
		  if (!string.ReferenceEquals(threadName, null))
		  {
			errors.AddLast("Expected threadName to be null but it was " + threadName);
		  }
		  threadName = Thread.CurrentThread.Name;

		  try
		  {
			Thread.Sleep(5);
		  }
		  catch (InterruptedException)
		  {
			// Won't ever happen
		  }
		  threadName = null;
		  latch.Signal();
		}
	  }
	}

}