using System;
using System.Threading;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{

	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;

	/// <summary>
	/// Superclass for mutable calculation listeners that collect the results of individual calculations and
	/// create a single aggregate result when the calculations are complete.
	/// </summary>
	/// @param <T>  the type of the aggregate result </param>
	public abstract class AggregatingCalculationListener<T> : CalculationListener
	{

	  /// <summary>
	  /// A future representing the aggregate result. </summary>
	  private readonly CompletableFuture<T> future = new CompletableFuture<T>();

	  public void calculationsComplete()
	  {
		future.complete(createAggregateResult());
	  }

	  /// <summary>
	  /// Returns the aggregate result of the calculations, blocking until it is available.
	  /// <para>
	  /// If the thread is interrupted while this method is blocked, then a runtime exception
	  /// is thrown, but with the interrupt flag set.
	  /// For additional control, use <seealso cref="#getFuture()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the aggregate result of the calculations, blocking until it is available </returns>
	  public virtual T result()
	  {
		try
		{
		  return future.get();
		}
		catch (InterruptedException ex)
		{
		  Thread.CurrentThread.Interrupt();
		  throw new Exception(ex);
		}
		catch (ExecutionException ex)
		{
		  throw new Exception("Exception getting result", ex);
		}
	  }

	  /// <summary>
	  /// A future providing asynchronous notification when the results are available.
	  /// </summary>
	  /// <returns> a future providing asynchronous notification when the results are available </returns>
	  public virtual CompletableFuture<T> Future
	  {
		  get
		  {
			return future;
		  }
	  }

	  public override abstract void resultReceived(CalculationTarget target, CalculationResult result);

	  /// <summary>
	  /// Invoked to create the aggregate result when the individual calculations are complete.
	  /// <para>
	  /// This is guaranteed to be invoked after all results have been passed to <seealso cref="#resultReceived"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the aggregate result of all the calculations </returns>
	  protected internal abstract T createAggregateResult();
	}

}