using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc
{

	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CalculationListener = com.opengamma.strata.calc.runner.CalculationListener;
	using CalculationTaskRunner = com.opengamma.strata.calc.runner.CalculationTaskRunner;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// Component that provides the ability to perform calculations on multiple targets, measures and scenarios.
	/// <para>
	/// The strata-pricer module provides the ability to calculate results for a single trade,
	/// single measure and single set of market data. {@code CalculationRunner} provides the ability
	/// to calculate results for many trades, many measures and many sets of market data.
	/// </para>
	/// <para>
	/// Once obtained, the {@code CalculationRunner} instance may be used to calculate results.
	/// The four "calculate" methods handle the combination of single versus scenario market data,
	/// and synchronous versus asynchronous.
	/// </para>
	/// <para>
	/// A calculation runner is typically obtained using the static methods on this interface.
	/// The instance contains an executor thread-pool, thus care should be taken to ensure
	/// the thread-pool is correctly managed. For example, try-with-resources could be used:
	/// <pre>
	///  try (CalculationRunner runner = CalculationRunner.ofMultiThreaded()) {
	///    // use the runner
	///  }
	/// </pre>
	/// </para>
	/// </summary>
	public interface CalculationRunner : AutoCloseable
	{

	  /// <summary>
	  /// Creates a standard multi-threaded calculation runner capable of performing calculations.
	  /// <para>
	  /// This factory creates an executor basing the number of threads on the number of available processors.
	  /// It is recommended to use try-with-resources to manage the runner:
	  /// <pre>
	  ///  try (CalculationRunner runner = CalculationRunner.ofMultiThreaded()) {
	  ///    // use the runner
	  ///  }
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the calculation runner </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static CalculationRunner ofMultiThreaded()
	//  {
	//	return DefaultCalculationRunner.ofMultiThreaded();
	//  }

	  /// <summary>
	  /// Creates a calculation runner capable of performing calculations, specifying the executor.
	  /// <para>
	  /// It is the callers responsibility to manage the life-cycle of the executor.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="executor">  the executor to use </param>
	  /// <returns> the calculation runner </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static CalculationRunner of(java.util.concurrent.ExecutorService executor)
	//  {
	//	return DefaultCalculationRunner.of(executor);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Performs calculations for a single set of market data.
	  /// <para>
	  /// This returns a grid of results based on the specified targets, columns, rules and market data.
	  /// The grid will contain a row for each target and a column for each measure.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="calculationRules">  the rules defining how the calculation is performed </param>
	  /// <param name="targets">  the targets for which values of the measures will be calculated </param>
	  /// <param name="columns">  the configuration for the columns that will be calculated,
	  ///   including the measure and any column-specific overrides </param>
	  /// <param name="marketData">  the market data to be used in the calculations </param>
	  /// <param name="refData">  the reference data to be used in the calculations </param>
	  /// <returns> the grid of calculation results, based on the targets and columns </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public abstract Results calculate(CalculationRules calculationRules, java.util.List<? extends com.opengamma.strata.basics.CalculationTarget> targets, java.util.List<Column> columns, com.opengamma.strata.data.MarketData marketData, com.opengamma.strata.basics.ReferenceData refData);
	  Results calculate<T1>(CalculationRules calculationRules, IList<T1> targets, IList<Column> columns, MarketData marketData, ReferenceData refData);

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
	  /// <param name="calculationRules">  the rules defining how the calculation is performed </param>
	  /// <param name="targets">  the targets for which values of the measures will be calculated </param>
	  /// <param name="columns">  the configuration for the columns that will be calculated,
	  ///   including the measure and any column-specific overrides </param>
	  /// <param name="marketData">  the market data to be used in the calculations </param>
	  /// <param name="refData">  the reference data to be used in the calculations </param>
	  /// <param name="listener">  listener that is invoked when individual results are calculated </param>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public abstract void calculateAsync(CalculationRules calculationRules, java.util.List<? extends com.opengamma.strata.basics.CalculationTarget> targets, java.util.List<Column> columns, com.opengamma.strata.data.MarketData marketData, com.opengamma.strata.basics.ReferenceData refData, com.opengamma.strata.calc.runner.CalculationListener listener);
	  void calculateAsync<T1>(CalculationRules calculationRules, IList<T1> targets, IList<Column> columns, MarketData marketData, ReferenceData refData, CalculationListener listener);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Performs calculations for multiple scenarios, each with a different set of market data.
	  /// <para>
	  /// This returns a grid of results based on the specified targets, columns, rules and market data.
	  /// The grid will contain a row for each target and a column for each measure.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="calculationRules">  the rules defining how the calculation is performed </param>
	  /// <param name="targets">  the targets for which values of the measures will be calculated </param>
	  /// <param name="columns">  the configuration for the columns that will be calculated,
	  ///   including the measure and any column-specific overrides </param>
	  /// <param name="marketData">  the market data to be used in the calculations </param>
	  /// <param name="refData">  the reference data to be used in the calculations </param>
	  /// <returns> the grid of calculation results, based on the targets and columns </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public abstract Results calculateMultiScenario(CalculationRules calculationRules, java.util.List<? extends com.opengamma.strata.basics.CalculationTarget> targets, java.util.List<Column> columns, com.opengamma.strata.data.scenario.ScenarioMarketData marketData, com.opengamma.strata.basics.ReferenceData refData);
	  Results calculateMultiScenario<T1>(CalculationRules calculationRules, IList<T1> targets, IList<Column> columns, ScenarioMarketData marketData, ReferenceData refData);

	  /// <summary>
	  /// Performs calculations asynchronously for a multiple scenarios, each with a different set of market data,
	  /// invoking a listener as each calculation completes.
	  /// <para>
	  /// This method requires the listener to assemble the results, but it can be much more memory efficient when
	  /// calculating aggregate results. If the individual results are discarded after they are incorporated into
	  /// the aggregate they can be garbage collected.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="calculationRules">  the rules defining how the calculation is performed </param>
	  /// <param name="targets">  the targets for which values of the measures will be calculated </param>
	  /// <param name="columns">  the configuration for the columns that will be calculated,
	  ///   including the measure and any column-specific overrides </param>
	  /// <param name="marketData">  the market data to be used in the calculations </param>
	  /// <param name="refData">  the reference data to be used in the calculations </param>
	  /// <param name="listener">  listener that is invoked when individual results are calculated </param>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public abstract void calculateMultiScenarioAsync(CalculationRules calculationRules, java.util.List<? extends com.opengamma.strata.basics.CalculationTarget> targets, java.util.List<Column> columns, com.opengamma.strata.data.scenario.ScenarioMarketData marketData, com.opengamma.strata.basics.ReferenceData refData, com.opengamma.strata.calc.runner.CalculationListener listener);
	  void calculateMultiScenarioAsync<T1>(CalculationRules calculationRules, IList<T1> targets, IList<Column> columns, ScenarioMarketData marketData, ReferenceData refData, CalculationListener listener);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying task runner.
	  /// <para>
	  /// In most cases, this runner will be implemented using an instance of <seealso cref="CalculationTaskRunner"/>.
	  /// That interface provides a lower-level API, with the ability optimize if similar calculations
	  /// are being made repeatedly.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the underlying task runner </returns>
	  /// <exception cref="UnsupportedOperationException"> if access to the task runner is not provided </exception>
	  CalculationTaskRunner TaskRunner {get;}

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