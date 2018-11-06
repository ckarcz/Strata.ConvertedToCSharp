using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.marketdata
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.not;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableMap;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableSet;


	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using MapStream = com.opengamma.strata.collect.MapStream;
	using Result = com.opengamma.strata.collect.result.Result;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// The default market data factory.
	/// <para>
	/// This uses two providers, one for observable data and one for time-series.
	/// </para>
	/// </summary>
	internal sealed class DefaultMarketDataFactory : MarketDataFactory
	{

	  /// <summary>
	  /// Builds observable market data. </summary>
	  private readonly ObservableDataProvider observableDataProvider;

	  /// <summary>
	  /// Provides time-series of observable market data values. </summary>
	  private readonly TimeSeriesProvider timeSeriesProvider;

	  /// <summary>
	  /// Market data functions, keyed by the type of the market data ID they can handle. </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<Class, MarketDataFunction<?, ?>> functions;
	  private readonly IDictionary<Type, MarketDataFunction<object, ?>> functions;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance of the factory based on providers of market data and time-series.
	  /// <para>
	  /// The market data functions are used to build the market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="observableDataProvider">  the provider observable market data </param>
	  /// <param name="timeSeriesProvider">  the provider time-series </param>
	  /// <param name="functions">  the functions that create the market data </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") DefaultMarketDataFactory(ObservableDataProvider observableDataProvider, TimeSeriesProvider timeSeriesProvider, java.util.List<MarketDataFunction<?, ?>> functions)
	  internal DefaultMarketDataFactory<T1>(ObservableDataProvider observableDataProvider, TimeSeriesProvider timeSeriesProvider, IList<T1> functions)
	  {

		this.observableDataProvider = observableDataProvider;
		this.timeSeriesProvider = timeSeriesProvider;

		// Use a HashMap instead of an ImmutableMap.Builder so values can be overwritten.
		// If the functions argument includes a missing mapping builder it can overwrite the one inserted below
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<Class, MarketDataFunction<?, ?>> builderMap = new java.util.HashMap<>();
		IDictionary<Type, MarketDataFunction<object, ?>> builderMap = new Dictionary<Type, MarketDataFunction<object, ?>>();

		functions.ForEach(builder => builderMap.put(builder.MarketDataIdType, builder));
		this.functions = ImmutableMap.copyOf(builderMap);
	  }

	  //-------------------------------------------------------------------------
	  public BuiltMarketData create(MarketDataRequirements requirements, MarketDataConfig marketDataConfig, MarketData suppliedData, ReferenceData refData)
	  {

		ScenarioMarketData md = ScenarioMarketData.of(1, suppliedData);
		BuiltScenarioMarketData smd = createMultiScenario(requirements, marketDataConfig, md, refData, ScenarioDefinition.empty());
		return new BuiltMarketData(smd);
	  }

	  public BuiltScenarioMarketData createMultiScenario(MarketDataRequirements requirements, MarketDataConfig marketDataConfig, MarketData suppliedData, ReferenceData refData, ScenarioDefinition scenarioDefinition)
	  {

		ScenarioMarketData md = ScenarioMarketData.of(1, suppliedData);
		return createMultiScenario(requirements, marketDataConfig, md, refData, scenarioDefinition);
	  }

	  public BuiltScenarioMarketData createMultiScenario(MarketDataRequirements requirements, MarketDataConfig marketDataConfig, ScenarioMarketData suppliedData, ReferenceData refData, ScenarioDefinition scenarioDefinition)
	  {

		BuiltScenarioMarketDataBuilder dataBuilder = BuiltScenarioMarketData.builder(suppliedData.ValuationDate);
		BuiltScenarioMarketData builtData = dataBuilder.build();

		// Build a tree of the market data dependencies. The root of the tree represents the calculations.
		// The children of the root represent the market data directly used in the calculations. The children
		// of those nodes represent the market data required to build that data, and so on
		MarketDataNode root = MarketDataNode.buildDependencyTree(requirements, suppliedData, marketDataConfig, functions);

		// The leaf nodes of the dependency tree represent market data with no missing requirements for market data.
		// This includes:
		//   * Market data that is already available
		//   * Observable data whose value can be obtained from a market data provider
		//   * Market data that can be built from data that is already available
		//
		// Therefore the market data represented by the leaf nodes can be built immediately.
		//
		// Market data building proceeds in multiple steps. The operations in each step are:
		//   1) Build the market data represented by the leaf nodes of the dependency tree
		//   2) Create a copy of the dependency tree without the leaf nodes
		//   3) If the root of new dependency tree has children, go to step 1 with the new tree
		//
		// When the tree has no children it indicates all dependencies have been built and the market data
		// needed for the calculations is available.
		//
		// The result of this method also contains details of the problems for market data can't be built or found.

		while (!root.Leaf)
		{
		  // Effectively final reference to buildData which can be used in a lambda expression
		  BuiltScenarioMarketData marketData = builtData;

		  // The leaves of the dependency tree represent market data with no dependencies that can be built immediately
		  Pair<MarketDataNode, MarketDataRequirements> pair = root.withLeavesRemoved();

		  // The requirements contained in the leaf nodes
		  MarketDataRequirements leafRequirements = pair.Second;

		  // Time series of observable data ------------------------------------------------------------

		  // Build any time series that are required but not available
		  leafRequirements.TimeSeries.Where(id => marketData.getTimeSeries(id).Empty).Where(id => suppliedData.getTimeSeries(id).Empty).ForEach(id => dataBuilder.addTimeSeriesResult(id, timeSeriesProvider.provideTimeSeries(id)));

		  // Copy supplied time series to the scenario data
		  leafRequirements.TimeSeries.Where(id => !suppliedData.getTimeSeries(id).Empty).ForEach(id => dataBuilder.addTimeSeries(id, suppliedData.getTimeSeries(id)));

		  // Single values of observable data -----------------------------------------------------------

		  // Filter out IDs for the data that is already available
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		  ISet<ObservableId> observableIds = leafRequirements.Observables.Where(not(marketData.containsValue)).Where(not(suppliedData.containsValue)).collect(toImmutableSet());

		  // Observable data is built in bulk so it can be efficiently requested from data provider in one operation
		  if (observableIds.Count > 0)
		  {
			IDictionary<ObservableId, Result<double>> observableResults = observableDataProvider.provideObservableData(observableIds);
			MapStream.of(observableResults).forEach((id, res) => addObservableResult(id, res, refData, scenarioDefinition, dataBuilder));
		  }

		  // Copy observable data from the supplied data to the builder, applying any matching perturbations
		  leafRequirements.Observables.Where(suppliedData.containsValue).ForEach(id => addValue(id, suppliedData.getValue(id), refData, scenarioDefinition, dataBuilder));

		  // Non-observable data -----------------------------------------------------------------------

		  // Filter out IDs for the data that is already available and build the rest
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Set<com.opengamma.strata.data.MarketDataId<?>> nonObservableIds = leafRequirements.getNonObservables().stream().filter(not(marketData::containsValue)).filter(not(suppliedData::containsValue)).collect(toImmutableSet());
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		  ISet<MarketDataId<object>> nonObservableIds = leafRequirements.NonObservables.Where(not(marketData.containsValue)).Where(not(suppliedData.containsValue)).collect(toImmutableSet());

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, com.opengamma.strata.collect.result.Result<com.opengamma.strata.data.scenario.MarketDataBox<?>>> nonObservableResults = buildNonObservableData(nonObservableIds, marketDataConfig, marketData, refData);
		  IDictionary<MarketDataId<object>, Result<MarketDataBox<object>>> nonObservableResults = buildNonObservableData(nonObservableIds, marketDataConfig, marketData, refData);

		  MapStream.of(nonObservableResults).forEach((id, result) => addResult(id, result, refData, scenarioDefinition, dataBuilder));

		  // Copy supplied data to the scenario data after applying perturbations
		  leafRequirements.NonObservables.Where(suppliedData.containsValue).ForEach(id => addValue(id, suppliedData.getValue(id), refData, scenarioDefinition, dataBuilder));

		  // --------------------------------------------------------------------------------------------

		  // Put the data built so far into an object that will be used in the next phase of building data
		  builtData = dataBuilder.build();

		  // A copy of the dependency tree not including the leaf nodes
		  root = pair.First;
		}
		return builtData;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Builds items of non-observable market data using a market data function.
	  /// </summary>
	  /// <param name="id">  ID of the market data that should be built </param>
	  /// <param name="marketDataConfig">  configuration specifying how the market data should be built </param>
	  /// <param name="suppliedData">  existing set of market data that contains any data required to build the values </param>
	  /// <param name="refData">  the reference data, used to resolve trades </param>
	  /// <returns> a result containing the market data or details of why it wasn't built </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes"}) private com.opengamma.strata.collect.result.Result<com.opengamma.strata.data.scenario.MarketDataBox<?>> buildNonObservableData(com.opengamma.strata.data.MarketDataId id, MarketDataConfig marketDataConfig, BuiltScenarioMarketData suppliedData, com.opengamma.strata.basics.ReferenceData refData)
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  private Result<MarketDataBox<object>> buildNonObservableData(MarketDataId id, MarketDataConfig marketDataConfig, BuiltScenarioMarketData suppliedData, ReferenceData refData)
	  {

		// The raw types in this method are an unfortunate necessity. The type parameters on MarketDataBuilder
		// are mainly a useful guide for implementors as they constrain the method type signatures.
		// In this class a mixture of functions with different types are stored in a map. This loses the type
		// parameter information. When the functions are extracted from the map and used it's impossible to
		// convince the compiler the operations are safe, although the logic guarantees it.

		// This cast removes a spurious warning
		Type idClass = (Type) id.GetType();
		MarketDataFunction marketDataFunction = functions[idClass];

		if (marketDataFunction == null)
		{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		  throw new System.InvalidOperationException("No market data function available for market data ID of type " + idClass.FullName);
		}
		return Result.of(() => marketDataFunction.build(id, marketDataConfig, suppliedData, refData));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") private java.util.Map<com.opengamma.strata.data.MarketDataId<?>, com.opengamma.strata.collect.result.Result<com.opengamma.strata.data.scenario.MarketDataBox<?>>> buildNonObservableData(java.util.Set<? extends com.opengamma.strata.data.MarketDataId<?>> ids, MarketDataConfig marketDataConfig, BuiltScenarioMarketData marketData, com.opengamma.strata.basics.ReferenceData refData)
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  private IDictionary<MarketDataId<object>, Result<MarketDataBox<object>>> buildNonObservableData<T1>(ISet<T1> ids, MarketDataConfig marketDataConfig, BuiltScenarioMarketData marketData, ReferenceData refData)
	  {

//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return ids.collect(toImmutableMap(id => id, id => buildNonObservableData(id, marketDataConfig, marketData, refData)));
	  }

	  /// <summary>
	  /// Adds an item of market data to a builder.
	  /// <para>
	  /// If the result is a failure it is added to the list of failures.
	  /// </para>
	  /// <para>
	  /// If the result is a success it is passed to <seealso cref="#addValue"/> where the scenario definition is
	  /// applied and the data is added to the builder.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="id">  ID of the market data value </param>
	  /// <param name="valueResult">  a result containing the market data value or details of why it couldn't be built </param>
	  /// <param name="scenarioDefinition">  definition of a set of scenarios </param>
	  /// <param name="builder">  the value or failure details are added to this builder </param>
	  private void addResult<T1, T2>(MarketDataId<T1> id, Result<T2> valueResult, ReferenceData refData, ScenarioDefinition scenarioDefinition, BuiltScenarioMarketDataBuilder builder)
	  {

		if (valueResult.Failure)
		{
		  builder.addResult(id, valueResult);
		}
		else
		{
		  addValue(id, valueResult.Value, refData, scenarioDefinition, builder);
		}
	  }

	  /// <summary>
	  /// Adds an item of observable market data to a builder.
	  /// <para>
	  /// If the result is a failure it is added to the list of failures.
	  /// </para>
	  /// <para>
	  /// If the result is a success it is passed to <seealso cref="#addValue"/> where the scenario definition is
	  /// applied and the data is added to the builder.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="id">  ID of the market data value </param>
	  /// <param name="valueResult">  a result containing the market data value or details of why it couldn't be built </param>
	  /// <param name="scenarioDefinition">  definition of a set of scenarios </param>
	  /// <param name="builder">  the value or failure details are added to this builder </param>
	  private void addObservableResult(ObservableId id, Result<double> valueResult, ReferenceData refData, ScenarioDefinition scenarioDefinition, BuiltScenarioMarketDataBuilder builder)
	  {

		if (valueResult.Failure)
		{
		  builder.addResult(id, Result.failure(valueResult));
		}
		else
		{
		  addValue(id, MarketDataBox.ofSingleValue(valueResult.Value), refData, scenarioDefinition, builder);
		}
	  }

	  /// <summary>
	  /// Adds an item of market data to a builder.
	  /// <para>
	  /// The mappings from the scenario definition is applied to the value. If any of the mappings match the value
	  /// is perturbed and the perturbed values are added to the market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="id">  ID of the market data value </param>
	  /// <param name="value">  the market data value </param>
	  /// <param name="scenarioDefinition">  definition of a set of scenarios </param>
	  /// <param name="builder">  the market data is added to this builder </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") private void addValue(com.opengamma.strata.data.MarketDataId<?> id, com.opengamma.strata.data.scenario.MarketDataBox<?> value, com.opengamma.strata.basics.ReferenceData refData, ScenarioDefinition scenarioDefinition, BuiltScenarioMarketDataBuilder builder)
	  private void addValue<T1, T2>(MarketDataId<T1> id, MarketDataBox<T2> value, ReferenceData refData, ScenarioDefinition scenarioDefinition, BuiltScenarioMarketDataBuilder builder)
	  {

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Optional<PerturbationMapping<?>> optionalMapping = scenarioDefinition.getMappings().stream().filter(m -> m.matches(id, value, refData)).findFirst();
		Optional<PerturbationMapping<object>> optionalMapping = scenarioDefinition.Mappings.Where(m => m.matches(id, value, refData)).First();

		if (optionalMapping.Present)
		{
		  // This is definitely safe because the filter matched the value and the types of the filter and perturbation
		  // are compatible
		  PerturbationMapping<object> mapping = (PerturbationMapping<object>) optionalMapping.get();
		  MarketDataBox<object> objectValue = ((MarketDataBox<object>) value);
		  // Result.of() catches any exceptions thrown by the mapping and wraps them in a failure
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<com.opengamma.strata.data.scenario.MarketDataBox<?>> result = com.opengamma.strata.collect.result.Result.of(() -> mapping.applyPerturbation(objectValue, refData));
		  Result<MarketDataBox<object>> result = Result.of(() => mapping.applyPerturbation(objectValue, refData));
		  builder.addResult(id, result);
		}
		else
		{
		  builder.addBox(id, value);
		}
	  }

	}

}