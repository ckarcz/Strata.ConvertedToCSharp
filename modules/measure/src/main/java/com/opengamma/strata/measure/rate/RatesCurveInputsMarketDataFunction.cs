using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.rate
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableMap;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableSet;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.zip;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using MarketDataConfig = com.opengamma.strata.calc.marketdata.MarketDataConfig;
	using MarketDataFunction = com.opengamma.strata.calc.marketdata.MarketDataFunction;
	using MarketDataRequirements = com.opengamma.strata.calc.marketdata.MarketDataRequirements;
	using MapStream = com.opengamma.strata.collect.MapStream;
	using Messages = com.opengamma.strata.collect.Messages;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CurveDefinition = com.opengamma.strata.market.curve.CurveDefinition;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using RatesCurveInputs = com.opengamma.strata.market.curve.RatesCurveInputs;
	using RatesCurveInputsId = com.opengamma.strata.market.curve.RatesCurveInputsId;

	/// <summary>
	/// Market data function that builds the input data used when calibrating a curve.
	/// </summary>
	public sealed class RatesCurveInputsMarketDataFunction : MarketDataFunction<RatesCurveInputs, RatesCurveInputsId>
	{

	  public MarketDataRequirements requirements(RatesCurveInputsId id, MarketDataConfig marketDataConfig)
	  {
		RatesCurveGroupDefinition groupConfig = marketDataConfig.get(typeof(RatesCurveGroupDefinition), id.CurveGroupName);
		Optional<CurveDefinition> optionalDefinition = groupConfig.findCurveDefinition(id.CurveName);
		if (!optionalDefinition.Present)
		{
		  return MarketDataRequirements.empty();
		}
		CurveDefinition definition = optionalDefinition.get();
		return MarketDataRequirements.builder().addValues(nodeRequirements(ImmutableList.of(definition))).build();
	  }

	  public MarketDataBox<RatesCurveInputs> build(RatesCurveInputsId id, MarketDataConfig marketDataConfig, ScenarioMarketData marketData, ReferenceData refData)
	  {

		CurveGroupName groupName = id.CurveGroupName;
		CurveName curveName = id.CurveName;
		RatesCurveGroupDefinition groupDefn = marketDataConfig.get(typeof(RatesCurveGroupDefinition), groupName);
		Optional<CurveDefinition> optionalDefinition = groupDefn.findCurveDefinition(id.CurveName);

		if (!optionalDefinition.Present)
		{
		  throw new System.ArgumentException(Messages.format("No curve named '{}' found in group '{}'", curveName, groupName));
		}
		CurveDefinition configuredDefn = optionalDefinition.get();
		// determine market data needs
		MarketDataBox<LocalDate> valuationDates = marketData.ValuationDate;
		bool multipleValuationDates = valuationDates.ScenarioValue;
		// curve definition can vary for each valuation date
		if (multipleValuationDates)
		{
		  IList<CurveDefinition> curveDefns = IntStream.range(0, valuationDates.ScenarioCount).mapToObj(valuationDates.getValue).map((LocalDate valDate) => configuredDefn.filtered(valDate, refData)).collect(toImmutableList());

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Set<com.opengamma.strata.data.MarketDataId<?>> requirements = nodeRequirements(curveDefns);
		  ISet<MarketDataId<object>> requirements = nodeRequirements(curveDefns);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, com.opengamma.strata.data.scenario.MarketDataBox<?>> marketDataValues = requirements.stream().collect(toImmutableMap(k -> k, k -> marketData.getValue(k)));
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		  IDictionary<MarketDataId<object>, MarketDataBox<object>> marketDataValues = requirements.collect(toImmutableMap(k => k, k => marketData.getValue(k)));
		  return buildMultipleCurveInputs(MarketDataBox.ofScenarioValues(curveDefns), marketDataValues, valuationDates, refData);
		}
		// only one valuation date
		LocalDate valuationDate = valuationDates.getValue(0);
		CurveDefinition filteredDefn = configuredDefn.filtered(valuationDate, refData);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Set<com.opengamma.strata.data.MarketDataId<?>> requirements = nodeRequirements(com.google.common.collect.ImmutableList.of(filteredDefn));
		ISet<MarketDataId<object>> requirements = nodeRequirements(ImmutableList.of(filteredDefn));
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, com.opengamma.strata.data.scenario.MarketDataBox<?>> marketDataValues = requirements.stream().collect(toImmutableMap(k -> k, k -> marketData.getValue(k)));
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IDictionary<MarketDataId<object>, MarketDataBox<object>> marketDataValues = requirements.collect(toImmutableMap(k => k, k => marketData.getValue(k)));
		// Do any of the inputs contain values for multiple scenarios, or do they contain 1 value each?
		bool multipleInputValues = marketDataValues.Values.Any(MarketDataBox.isScenarioValue);

		return multipleInputValues || multipleValuationDates ? buildMultipleCurveInputs(MarketDataBox.ofSingleValue(filteredDefn), marketDataValues, valuationDates, refData) : buildSingleCurveInputs(filteredDefn, marketDataValues, valuationDate, refData);
	  }

	  // one valuation date, one set of market data
	  private MarketDataBox<RatesCurveInputs> buildSingleCurveInputs<T1>(CurveDefinition filteredDefn, IDictionary<T1> marketData, LocalDate valuationDate, ReferenceData refData) where T1 : com.opengamma.strata.data.MarketDataId<T1>
	  {

		// There is only a single map of values and single valuation date - create a single CurveInputs instance
		CurveMetadata curveMetadata = filteredDefn.metadata(valuationDate, refData);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<? extends com.opengamma.strata.data.MarketDataId<?>, ?> singleMarketDataValues = com.opengamma.strata.collect.MapStream.of(marketData).mapValues(box -> box.getSingleValue()).toMap();
		IDictionary<MarketDataId<object>, ?> singleMarketDataValues = MapStream.of(marketData).mapValues(box => box.SingleValue).toMap();

		RatesCurveInputs curveInputs = RatesCurveInputs.of(singleMarketDataValues, curveMetadata);
		return MarketDataBox.ofSingleValue(curveInputs);
	  }

	  // one valuation date, scenario market data
	  private MarketDataBox<RatesCurveInputs> buildMultipleCurveInputs<T1>(MarketDataBox<CurveDefinition> filteredDefns, IDictionary<T1> marketData, MarketDataBox<LocalDate> valuationDates, ReferenceData refData) where T1 : com.opengamma.strata.data.MarketDataId<T1>
	  {

		// If there are multiple values for any of the input data values or for the valuation
		// dates then we need to create multiple sets of inputs
		int scenarioCount = RatesCurveInputsMarketDataFunction.scenarioCount(valuationDates, marketData);

		ImmutableList.Builder<CurveMetadata> curveMetadataBuilder = ImmutableList.builder();
		for (int i = 0; i < scenarioCount; i++)
		{
		  LocalDate valDate = valuationDates.getValue(i);
		  CurveDefinition defn = filteredDefns.getValue(i);
		  curveMetadataBuilder.add(defn.metadata(valDate, refData));
		}
		IList<CurveMetadata> curveMetadata = curveMetadataBuilder.build();

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<java.util.Map<? extends com.opengamma.strata.data.MarketDataId<?>, ?>> scenarioValues = java.util.stream.IntStream.range(0, scenarioCount).mapToObj(i -> buildScenarioValues(marketData, i)).collect(toImmutableList());
		IList<IDictionary<MarketDataId<object>, ?>> scenarioValues = IntStream.range(0, scenarioCount).mapToObj(i => buildScenarioValues(marketData, i)).collect(toImmutableList());

		IList<RatesCurveInputs> curveInputs = zip(scenarioValues.stream(), curveMetadata.stream()).map(pair => RatesCurveInputs.of(pair.First, pair.Second)).collect(toImmutableList());

		return MarketDataBox.ofScenarioValues(curveInputs);
	  }

	  /// <summary>
	  /// Builds a map of market data identifier to market data value for a single scenario.
	  /// </summary>
	  /// <param name="values">  the market data values for all scenarios </param>
	  /// <param name="scenarioIndex">  the index of the scenario </param>
	  /// <returns> map of market data values for one scenario </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static java.util.Map<? extends com.opengamma.strata.data.MarketDataId<?>, ?> buildScenarioValues(java.util.Map<? extends com.opengamma.strata.data.MarketDataId<?>, com.opengamma.strata.data.scenario.MarketDataBox<?>> values, int scenarioIndex)
	  private static IDictionary<MarketDataId<object>, ?> buildScenarioValues<T1>(IDictionary<T1> values, int scenarioIndex) where T1 : com.opengamma.strata.data.MarketDataId<T1>
	  {

		return MapStream.of(values).mapValues(box => box.getValue(scenarioIndex)).toMap();
	  }

	  private static int scenarioCount<T1>(MarketDataBox<LocalDate> valuationDate, IDictionary<T1> marketData) where T1 : com.opengamma.strata.data.MarketDataId<T1>
	  {

		int scenarioCount = 0;

		if (valuationDate.ScenarioValue)
		{
		  scenarioCount = valuationDate.ScenarioCount;
		}
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: for (java.util.Map.Entry<? extends com.opengamma.strata.data.MarketDataId<?>, com.opengamma.strata.data.scenario.MarketDataBox<?>> entry : marketData.entrySet())
		foreach (KeyValuePair<MarketDataId<object>, MarketDataBox<object>> entry in marketData.SetOfKeyValuePairs())
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.scenario.MarketDataBox<?> box = entry.getValue();
		  MarketDataBox<object> box = entry.Value;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.MarketDataId<?> id = entry.getKey();
		  MarketDataId<object> id = entry.Key;

		  if (box.ScenarioValue)
		  {
			int boxScenarioCount = box.ScenarioCount;

			if (scenarioCount == 0)
			{
			  scenarioCount = boxScenarioCount;
			}
			else
			{
			  if (scenarioCount != boxScenarioCount)
			  {
				throw new System.ArgumentException(Messages.format("There are {} scenarios for ID {} which does not match the previous scenario count {}", boxScenarioCount, id, scenarioCount));
			  }
			}
		  }
		}
		if (scenarioCount != 0)
		{
		  return scenarioCount;
		}
		// This shouldn't happen, this method is only called after checking at least one of the values contains data
		// for multiple scenarios.
		throw new System.ArgumentException("Cannot count the scenarios, all data contained single values");
	  }

	  public Type<RatesCurveInputsId> MarketDataIdType
	  {
		  get
		  {
			return typeof(RatesCurveInputsId);
		  }
	  }

	  /// <summary>
	  /// Returns requirements for the market data needed by the curve nodes to build trades.
	  /// </summary>
	  /// <param name="curveDefns">  the curve definition containing the nodes </param>
	  /// <returns> requirements for the market data needed by the nodes to build trades </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static java.util.Set<com.opengamma.strata.data.MarketDataId<?>> nodeRequirements(java.util.List<com.opengamma.strata.market.curve.CurveDefinition> curveDefns)
	  private static ISet<MarketDataId<object>> nodeRequirements(IList<CurveDefinition> curveDefns)
	  {
		return curveDefns.stream().flatMap(defn => defn.Nodes.stream()).flatMap(node => node.requirements().stream()).collect(toImmutableSet());
	  }

	}

}