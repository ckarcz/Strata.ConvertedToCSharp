/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// Component that provides the ability to run calculation tasks.
	/// <para>
	/// This interface is the lower-level counterpart to <seealso cref="CalculationRunner"/>.
	/// It provides the ability to calculate results based on <seealso cref="CalculationTasks"/>.
	/// Unless you need to optimize, the {@code CalculationRunner} is a simpler entry point.
	/// </para>
	/// <para>
	/// The purpose of the runner is to produce a grid of results, with a row for each target
	/// and a column for each measure. The targets and columns that define the grid of results
	/// are passed in using an instance of {@code CalculationTasks}.
	/// </para>
	/// <para>
	/// The {@code CalculationTasks} instance is obtained using 
	/// <seealso cref="CalculationTasks#of(CalculationRules, List, List, ReferenceData) static factory method"/>.
	/// It consists of a list of {@code CalculationTask} instances, where each task instance
	/// corresponds to a single cell in the grid of results. When the {@code CalculationTasks}
	/// instance is created for a set of trades and measures some one-off initialization is performed.
	/// Providing access to the instance allows the initialization to occur once, which could
	/// be a performance optimization if many different calculations are performed with the
	/// same set of trades and measures.
	/// </para>
	/// <para>
	/// Once obtained, the {@code CalculationTasks} instance may be used to calculate results.
	/// The four "calculate" methods handle the combination of single versus scenario market data,
	/// and synchronous versus asynchronous.
	/// </para>
	/// <para>
	/// A calculation runner is typically obtained using the static methods on this interface.
	/// The instance contains an executor thread-pool, thus care should be taken to ensure
	/// the thread-pool is correctly managed. For example, try-with-resources could be used:
	/// <pre>
	///  try (CalculationTaskRunner runner = CalculationTaskRunner.ofMultiThreaded()) {
	///    // use the runner
	///  }
	/// </pre>
	/// </para>
	/// </summary>
	public interface CalculationTaskRunner : AutoCloseable
	{

	  /// <summary>
	  /// Creates a standard multi-threaded calculation task runner capable of performing calculations.
	  /// <para>
	  /// This factory creates an executor basing the number of threads on the number of available processors.
	  /// It is recommended to use try-with-resources to manage the runner:
	  /// <pre>
	  ///  try (CalculationTaskRunner runner = CalculationTaskRunner.ofMultiThreaded()) {
	  ///    // use the runner
	  ///  }
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the calculation task runner </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static CalculationTaskRunner ofMultiThreaded()
	//  {
	//	return DefaultCalculationTaskRunner.ofMultiThreaded();
	//  }

	  /// <summary>
	  /// Creates a calculation task runner capable of performing calculations, specifying the executor.
	  /// <para>
	  /// It is the callers responsibility to manage the life-cycle of the executor.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="executor">  the executor to use </param>
	  /// <returns> the calculation task runner </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static CalculationTaskRunner of(java.util.concurrent.ExecutorService executor)
	//  {
	//	return DefaultCalculationTaskRunner.of(executor);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Performs calculations for a single set of market data.
	  /// <para>
	  /// This returns a grid of results based on the specified tasks and market data.
	  /// The grid will contain a row for each target and a column for each measure.
	  /// </para>
	  /// <para>
	  /// If the thread is interrupted while this method is blocked, calculations will stop
	  /// and a result returned indicating the failed tasks, with the interrupted flag set.
	  /// For additional control, use <seealso cref="#calculateAsync(CalculationTasks, MarketData, ReferenceData, CalculationListener)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tasks">  the calculation tasks to invoke </param>
	  /// <param name="marketData">  the market data to be used in the calculations </param>
	  /// <param name="refData">  the reference data to be used in the calculations </param>
	  /// <returns> the grid of calculation results, based on the tasks and market data </returns>
	  Results calculate(CalculationTasks tasks, MarketData marketData, ReferenceData refData);

	  /// <summary>
	  /// Performs calculations asynchronously for a single set of market data,
	  /// invoking a listener as each calculation completes.
	  /// <para>
	  /// This method requires the listener to assemble the results, but it can be much more memory efficient when
	  /// calculating aggregate results. If the individual results are discarded after they are incorporated into
	  /// the aggregate they can be garbage collected.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tasks">  the calculation tasks to invoke </param>
	  /// <param name="marketData">  the market data to be used in the calculations </param>
	  /// <param name="refData">  the reference data to be used in the calculations </param>
	  /// <param name="listener">  listener that is invoked when individual results are calculated </param>
	  void calculateAsync(CalculationTasks tasks, MarketData marketData, ReferenceData refData, CalculationListener listener);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Performs calculations for multiple scenarios, each with a different set of market data.
	  /// <para>
	  /// This returns a grid of results based on the specified tasks and market data.
	  /// The grid will contain a row for each target and a column for each measure.
	  /// Each cell will contain multiple results, one for each scenario.
	  /// </para>
	  /// <para>
	  /// If the thread is interrupted while this method is blocked, calculations will stop
	  /// and a result returned indicating the failed tasks, with the interrupted flag set.
	  /// For additional control, use
	  /// <seealso cref="#calculateMultiScenarioAsync(CalculationTasks, ScenarioMarketData, ReferenceData, CalculationListener)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tasks">  the calculation tasks to invoke </param>
	  /// <param name="marketData">  the market data to be used in the calculations </param>
	  /// <param name="refData">  the reference data to be used in the calculations </param>
	  /// <returns> the grid of calculation results, based on the tasks and market data </returns>
	  Results calculateMultiScenario(CalculationTasks tasks, ScenarioMarketData marketData, ReferenceData refData);

	  /// <summary>
	  /// Performs calculations asynchronously for multiple scenarios, each with a different set of market data,
	  /// invoking a listener as each calculation completes.
	  /// <para>
	  /// This method requires the listener to assemble the results, but it can be much more memory efficient when
	  /// calculating aggregate results. If the individual results are discarded after they are incorporated into
	  /// the aggregate they can be garbage collected.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tasks">  the calculation tasks to invoke </param>
	  /// <param name="marketData">  the market data to be used in the calculations </param>
	  /// <param name="refData">  the reference data to be used in the calculations </param>
	  /// <param name="listener">  listener that is invoked when individual results are calculated </param>
	  void calculateMultiScenarioAsync(CalculationTasks tasks, ScenarioMarketData marketData, ReferenceData refData, CalculationListener listener);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Closes any resources held by the component.
	  /// <para>
	  /// If the component holds an <seealso cref="ExecutorService"/>, this method will typically
	  /// call <seealso cref="ExecutorService#shutdown()"/>.
	  /// </para>
	  /// </summary>
	  void close();

	}

}