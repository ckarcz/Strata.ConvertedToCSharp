/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.marketdata
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CalculationTasks = com.opengamma.strata.calc.runner.CalculationTasks;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// Component that provides the ability to source and calibrate market data.
	/// <para>
	/// This component is used to create market data within Strata.
	/// Each method receives a set of requirements defining the market data that is required.
	/// This is typically obtained from <seealso cref="CalculationTasks#requirements(ReferenceData)"/>.
	/// </para>
	/// <para>
	/// Given the requirements, the factory will determine whether any raw market data is needed.
	/// This may use the <seealso cref="MarketDataConfig"/> to provide additional information.
	/// </para>
	/// <para>
	/// If raw market data is required, the <seealso cref="ObservableDataProvider"/> and <seealso cref="TimeSeriesProvider"/>
	/// will be invoked to supply it. Applications can implement these to supply data from an external source.
	/// Alternatively, the raw market data can be passed into each method using the {@code suppliedData} parameter.
	/// </para>
	/// <para>
	/// Once the raw data is obtained, the factory will determine whether it needs to be calibrated,
	/// which may also involve additional information from the <seealso cref="MarketDataConfig"/>.
	/// </para>
	/// <para>
	/// Two types of output can be built.
	/// The {@code create} method is used to obtain and calibrate a single set of market data.
	/// By contrast, the {@code createMultiScenario} methods are used to create data with multiple
	/// scenarios based on a <seealso cref="ScenarioDefinition"/>.
	/// </para>
	/// </summary>
	public interface MarketDataFactory
	{

	  /// <summary>
	  /// Obtains an instance of the factory based on providers of market data and time-series.
	  /// <para>
	  /// The market data functions are used to build the market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="observableDataProvider">  the provider of observable market data </param>
	  /// <param name="timeSeriesProvider">  the provider of time-series </param>
	  /// <param name="functions">  the functions that create the market data </param>
	  /// <returns> the market data factory </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static MarketDataFactory of(ObservableDataProvider observableDataProvider, TimeSeriesProvider timeSeriesProvider, MarketDataFunction<JavaToDotNetGenericWildcard, JavaToDotNetGenericWildcard>... functions)
	//  {
	//
	//	return new DefaultMarketDataFactory(observableDataProvider, timeSeriesProvider, ImmutableList.copyOf(functions));
	//  }

	  /// <summary>
	  /// Obtains an instance of the factory based on providers of market data and time-series.
	  /// <para>
	  /// The market data functions are used to build the market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="observableDataProvider">  the provider of observable market data </param>
	  /// <param name="timeSeriesProvider">  the provider of time-series </param>
	  /// <param name="functions">  the functions that create the market data </param>
	  /// <returns> the market data factory </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public static DefaultMarketDataFactory of(ObservableDataProvider observableDataProvider, TimeSeriesProvider timeSeriesProvider, java.util.List<MarketDataFunction<?, ?>> functions)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static DefaultMarketDataFactory of(ObservableDataProvider observableDataProvider, TimeSeriesProvider timeSeriesProvider, java.util.List<MarketDataFunction<JavaToDotNetGenericWildcard, JavaToDotNetGenericWildcard>> functions)
	//  {
	//
	//	return new DefaultMarketDataFactory(observableDataProvider, timeSeriesProvider, functions);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Builds a set of market data.
	  /// <para>
	  /// This builds market data based on the specified requirements and configuration.
	  /// If some market data is known, it can be supplied using the <seealso cref="MarketData"/> interface.
	  /// Only data not already present in the {@code suppliedData} will be built.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="requirements">  the market data required for the calculations </param>
	  /// <param name="marketDataConfig">  configuration needed to build non-observable market data, for example curves or surfaces </param>
	  /// <param name="suppliedData">  market data supplied by the user </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the market data required by the calculations plus details of any data that could not be built </returns>
	  BuiltMarketData create(MarketDataRequirements requirements, MarketDataConfig marketDataConfig, MarketData suppliedData, ReferenceData refData);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Builds the market data required for performing calculations for a set of scenarios.
	  /// <para>
	  /// This builds market data based on the specified requirements and configuration.
	  /// If some market data is known, it can be supplied using the <seealso cref="MarketData"/> interface.
	  /// Only data not already present in the {@code suppliedData} will be built.
	  /// The scenario definition will be applied, potentially generating multiple sets of market data.
	  /// </para>
	  /// <para>
	  /// If the scenario definition contains perturbations that apply to the inputs used to build market data,
	  /// the data must be built by this method, not provided in {@code suppliedData}.
	  /// </para>
	  /// <para>
	  /// For example, if a perturbation is defined that shocks the par rates used to build a curve, the curve
	  /// must not be provided in {@code suppliedData}. The factory will only build the curve using the par rates
	  /// if it is not found in {@code suppliedData}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="requirements">  the market data required for the calculations </param>
	  /// <param name="marketDataConfig">  configuration needed to build non-observable market data, for example curves or surfaces </param>
	  /// <param name="suppliedData">  the base market data used to derive the data for each scenario </param>
	  /// <param name="refData">  the reference data </param>
	  /// <param name="scenarioDefinition">  defines how the market data for each scenario is derived from the base data </param>
	  /// <returns> the market data required by the calculations </returns>
	  BuiltScenarioMarketData createMultiScenario(MarketDataRequirements requirements, MarketDataConfig marketDataConfig, MarketData suppliedData, ReferenceData refData, ScenarioDefinition scenarioDefinition);

	  /// <summary>
	  /// Builds the market data required for performing calculations for a set of scenarios.
	  /// <para>
	  /// This builds market data based on the specified requirements and configuration.
	  /// If some market data is known, it can be supplied using the <seealso cref="ScenarioMarketData"/> interface.
	  /// Only data not already present in the {@code suppliedData} will be built.
	  /// The scenario definition will be applied, potentially generating multiple sets of market data.
	  /// The number of scenarios in the supplied data must match that of the scenario definition.
	  /// </para>
	  /// <para>
	  /// If the scenario definition contains perturbations that apply to the inputs used to build market data,
	  /// the data must be built by this method, not provided in {@code suppliedData}.
	  /// </para>
	  /// <para>
	  /// For example, if a perturbation is defined that shocks the par rates used to build a curve, the curve
	  /// must not be provided in {@code suppliedData}. The factory will only build the curve using the par rates
	  /// if it is not found in {@code suppliedData}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="requirements">  the market data required for the calculations </param>
	  /// <param name="marketDataConfig">  configuration needed to build non-observable market data, for example curves or surfaces </param>
	  /// <param name="suppliedData">  the base market data used to derive the data for each scenario </param>
	  /// <param name="refData">  the reference data </param>
	  /// <param name="scenarioDefinition">  defines how the market data for each scenario is derived from the base data </param>
	  /// <returns> the market data required by the calculations </returns>
	  BuiltScenarioMarketData createMultiScenario(MarketDataRequirements requirements, MarketDataConfig marketDataConfig, ScenarioMarketData suppliedData, ReferenceData refData, ScenarioDefinition scenarioDefinition);

	}

}