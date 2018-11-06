/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{

	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;

	/// <summary>
	/// Listener that is notified when calculations are performed by a <seealso cref="CalculationRunner"/>.
	/// <para>
	/// It is guaranteed that the methods of a listener will only be invoked by a single thread at any
	/// time. It is not guaranteed to be the same thread invoking a listener each time. The calling
	/// code is synchronized to ensure that any changes in the listener state will be
	/// visible to every thread used to invoke the listener. Therefore listener implementations
	/// are not required to be thread safe.
	/// </para>
	/// <para>
	/// A listener instance should not be used for multiple sets of calculations.
	/// </para>
	/// </summary>
	public interface CalculationListener
	{

	  /// <summary>
	  /// Invoked when the calculations start; guaranteed to be invoked
	  /// before <seealso cref="#resultReceived(CalculationTarget, CalculationResult)"/> and
	  /// <seealso cref="#calculationsComplete()"/>.
	  /// </summary>
	  /// <param name="targets"> the targets for which values are being calculated; these are often trades </param>
	  /// <param name="columns"> the columns for which values are being calculated </param>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default void calculationsStarted(java.util.List<com.opengamma.strata.basics.CalculationTarget> targets, java.util.List<com.opengamma.strata.calc.Column> columns)
	//  {
	//	// Default implementation does nothing, required for backwards compatibility
	//  }

	  /// <summary>
	  /// Invoked when a calculation completes.
	  /// <para>
	  /// It is guaranteed that <seealso cref="#calculationsStarted(List, List)"/> will be called before
	  /// this method and that this method will never be called after <seealso cref="#calculationsComplete()"/>.
	  /// </para>
	  /// <para>
	  /// It is possible that this method will never be called. This can happen if an empty list of targets
	  /// is passed to the calculation runner.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="target">  the calculation target, such as a trade </param>
	  /// <param name="result">  the result of the calculation </param>
	  void resultReceived(CalculationTarget target, CalculationResult result);

	  /// <summary>
	  /// Invoked when all calculations have completed.
	  /// <para>
	  /// This is guaranteed to be called after all results have been passed to <seealso cref="#resultReceived"/>.
	  /// </para>
	  /// <para>
	  /// This method will be called immediately after <seealso cref="#calculationsStarted(List, List)"/> and without any calls
	  /// to <seealso cref="#resultReceived(CalculationTarget, CalculationResult)"/> if there are no calculations to be performed.
	  /// This can happen if an empty list of targets is passed to the calculation runner.
	  /// </para>
	  /// </summary>
	  void calculationsComplete();

	}

}