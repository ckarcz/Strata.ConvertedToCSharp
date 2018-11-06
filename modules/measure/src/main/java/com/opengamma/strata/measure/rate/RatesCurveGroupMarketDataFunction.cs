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


	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using MarketDataConfig = com.opengamma.strata.calc.marketdata.MarketDataConfig;
	using MarketDataFunction = com.opengamma.strata.calc.marketdata.MarketDataFunction;
	using MarketDataRequirements = com.opengamma.strata.calc.marketdata.MarketDataRequirements;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CurveDefinition = com.opengamma.strata.market.curve.CurveDefinition;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using RatesCurveGroup = com.opengamma.strata.market.curve.RatesCurveGroup;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using RatesCurveGroupId = com.opengamma.strata.market.curve.RatesCurveGroupId;
	using RatesCurveInputs = com.opengamma.strata.market.curve.RatesCurveInputs;
	using RatesCurveInputsId = com.opengamma.strata.market.curve.RatesCurveInputsId;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;
	using RootFinderConfig = com.opengamma.strata.measure.curve.RootFinderConfig;
	using CalibrationMeasures = com.opengamma.strata.pricer.curve.CalibrationMeasures;
	using RatesCurveCalibrator = com.opengamma.strata.pricer.curve.RatesCurveCalibrator;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;

	/// <summary>
	/// Market data function that builds a curve group.
	/// <para>
	/// This function calibrates curves, turning a <seealso cref="RatesCurveGroupDefinition"/> into a <seealso cref="RatesCurveGroup"/>.
	/// </para>
	/// </summary>
	public class RatesCurveGroupMarketDataFunction : MarketDataFunction<RatesCurveGroup, RatesCurveGroupId>
	{

	  /// <summary>
	  /// The default analytics object that performs the curve calibration.
	  /// </summary>
	  private readonly CalibrationMeasures calibrationMeasures;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a new function for building curve groups using the standard measures.
	  /// <para>
	  /// This will use the standard <seealso cref="CalibrationMeasures#PAR_SPREAD par spread"/> measures
	  /// for calibration. The <seealso cref="MarketDataConfig"/> may contain a <seealso cref="RootFinderConfig"/>
	  /// to define the tolerances.
	  /// </para>
	  /// </summary>
	  public RatesCurveGroupMarketDataFunction() : this(CalibrationMeasures.PAR_SPREAD)
	  {
	  }

	  /// <summary>
	  /// Creates a new function for building curve groups.
	  /// <para>
	  /// The default calibrator is specified. The <seealso cref="MarketDataConfig"/> may contain a
	  /// <seealso cref="RootFinderConfig"/> that alters the tolerances used in calibration.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="calibrationMeasures">  the calibration measures to be used in the calibrator </param>
	  public RatesCurveGroupMarketDataFunction(CalibrationMeasures calibrationMeasures)
	  {
		this.calibrationMeasures = ArgChecker.notNull(calibrationMeasures, "calibrationMeasures");
	  }

	  //-------------------------------------------------------------------------
	  public virtual MarketDataRequirements requirements(RatesCurveGroupId id, MarketDataConfig marketDataConfig)
	  {
		RatesCurveGroupDefinition groupDefn = marketDataConfig.get(typeof(RatesCurveGroupDefinition), id.CurveGroupName);

		// request input data for any curves that need market data
		// no input data is requested if the curve definition contains all the market data needed to build the curve
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<RatesCurveInputsId> curveInputsIds = groupDefn.CurveDefinitions.Where(defn => requiresMarketData(defn)).Select(defn => defn.Name).Select(curveName => RatesCurveInputsId.of(groupDefn.Name, curveName, id.ObservableSource)).collect(toImmutableList());
		IList<ObservableId> timeSeriesIds = groupDefn.Entries.stream().flatMap(entry => entry.Indices.stream()).distinct().map(index => IndexQuoteId.of(index)).collect(toImmutableList());
		return MarketDataRequirements.builder().addValues(curveInputsIds).addTimeSeries(timeSeriesIds).build();
	  }

	  public virtual MarketDataBox<RatesCurveGroup> build(RatesCurveGroupId id, MarketDataConfig marketDataConfig, ScenarioMarketData marketData, ReferenceData refData)
	  {

		// create the calibrator, using the configured RootFinderConfig if found
		RootFinderConfig rfc = marketDataConfig.find(typeof(RootFinderConfig)).orElse(RootFinderConfig.standard());
		RatesCurveCalibrator calibrator = RatesCurveCalibrator.of(rfc.AbsoluteTolerance, rfc.RelativeTolerance, rfc.MaximumSteps, calibrationMeasures);

		// calibrate
		CurveGroupName groupName = id.CurveGroupName;
		RatesCurveGroupDefinition configuredDefn = marketDataConfig.get(typeof(RatesCurveGroupDefinition), groupName);
		return buildCurveGroup(configuredDefn, calibrator, marketData, refData, id.ObservableSource);
	  }

	  public virtual Type<RatesCurveGroupId> MarketDataIdType
	  {
		  get
		  {
			return typeof(RatesCurveGroupId);
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Builds a curve group given the configuration for the group and a set of market data.
	  /// </summary>
	  /// <param name="configuredGroup">  the definition of the curve group </param>
	  /// <param name="calibrator">  the calibrator </param>
	  /// <param name="marketData">  the market data containing any values required to build the curve group </param>
	  /// <param name="refData">  the reference data, used for resolving trades </param>
	  /// <param name="obsSource">  the source of observable market data </param>
	  /// <returns> a result containing the curve group or details of why it couldn't be built </returns>
	  internal virtual MarketDataBox<RatesCurveGroup> buildCurveGroup(RatesCurveGroupDefinition configuredGroup, RatesCurveCalibrator calibrator, ScenarioMarketData marketData, ReferenceData refData, ObservableSource obsSource)
	  {

		// find and combine all the input data
		CurveGroupName groupName = configuredGroup.Name;

//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<MarketDataBox<RatesCurveInputs>> inputBoxes = configuredGroup.CurveDefinitions.Select(curveDefn => curveInputs(curveDefn, marketData, groupName, obsSource)).collect(toImmutableList());
		MarketDataBox<LocalDate> valuationDates = marketData.ValuationDate;
		// If any of the inputs have values for multiple scenarios then we need to build a curve group for each scenario.
		// If all inputs contain a single value then we only need to build a single curve group.
		bool multipleValuationDates = valuationDates.ScenarioValue;
		bool multipleValues = inputBoxes.Any(MarketDataBox.isScenarioValue);
		IDictionary<ObservableId, LocalDateDoubleTimeSeries> fixings = extractFixings(marketData);

		return multipleValues || multipleValuationDates ? buildMultipleCurveGroups(configuredGroup, calibrator, valuationDates, inputBoxes, fixings, refData) : buildSingleCurveGroup(configuredGroup, calibrator, valuationDates.SingleValue, inputBoxes, fixings, refData);
	  }

	  // extract the fixings from the input data
	  private IDictionary<ObservableId, LocalDateDoubleTimeSeries> extractFixings(ScenarioMarketData marketData)
	  {
		IDictionary<ObservableId, LocalDateDoubleTimeSeries> fixings = new Dictionary<ObservableId, LocalDateDoubleTimeSeries>();
		foreach (ObservableId id in marketData.TimeSeriesIds)
		{
		  fixings[id] = marketData.getTimeSeries(id);
		}
		return fixings;
	  }

	  // calibrates when there are multiple groups
	  private MarketDataBox<RatesCurveGroup> buildMultipleCurveGroups(RatesCurveGroupDefinition configuredGroup, RatesCurveCalibrator calibrator, MarketDataBox<LocalDate> valuationDateBox, IList<MarketDataBox<RatesCurveInputs>> inputBoxes, IDictionary<ObservableId, LocalDateDoubleTimeSeries> fixings, ReferenceData refData)
	  {

		int scenarioCount = RatesCurveGroupMarketDataFunction.scenarioCount(valuationDateBox, inputBoxes);
		ImmutableList.Builder<RatesCurveGroup> builder = ImmutableList.builder();

		for (int i = 0; i < scenarioCount; i++)
		{
		  LocalDate valuationDate = valuationDateBox.getValue(i);
		  RatesCurveGroupDefinition filteredGroup = configuredGroup.filtered(valuationDate, refData);
		  IList<RatesCurveInputs> curveInputsList = inputsForScenario(inputBoxes, i);
		  MarketData inputs = inputsByKey(valuationDate, curveInputsList, fixings);
		  builder.add(buildGroup(filteredGroup, calibrator, inputs, refData));
		}
		ImmutableList<RatesCurveGroup> curveGroups = builder.build();
		return MarketDataBox.ofScenarioValues(curveGroups);
	  }

	  private static IList<RatesCurveInputs> inputsForScenario(IList<MarketDataBox<RatesCurveInputs>> boxes, int scenarioIndex)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return boxes.Select(box => box.getValue(scenarioIndex)).collect(toImmutableList());
	  }

	  // calibrates when there is a single group
	  private MarketDataBox<RatesCurveGroup> buildSingleCurveGroup(RatesCurveGroupDefinition configuredGroup, RatesCurveCalibrator calibrator, LocalDate valuationDate, IList<MarketDataBox<RatesCurveInputs>> inputBoxes, IDictionary<ObservableId, LocalDateDoubleTimeSeries> fixings, ReferenceData refData)
	  {

		RatesCurveGroupDefinition filteredGroup = configuredGroup.filtered(valuationDate, refData);
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<RatesCurveInputs> inputs = inputBoxes.Select(MarketDataBox::getSingleValue).collect(toImmutableList());
		MarketData inputValues = inputsByKey(valuationDate, inputs, fixings);
		RatesCurveGroup curveGroup = buildGroup(filteredGroup, calibrator, inputValues, refData);
		return MarketDataBox.ofSingleValue(curveGroup);
	  }

	  /// <summary>
	  /// Extracts the underlying quotes from the <seealso cref="RatesCurveInputs"/> instances and returns them in a map.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <param name="inputs">  input data for the curve </param>
	  /// <param name="fixings">  the fixings </param>
	  /// <returns> the underlying quotes from the input data </returns>
	  private static MarketData inputsByKey(LocalDate valuationDate, IList<RatesCurveInputs> inputs, IDictionary<ObservableId, LocalDateDoubleTimeSeries> fixings)
	  {

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, Object> marketDataMap = new java.util.HashMap<>();
		IDictionary<MarketDataId<object>, object> marketDataMap = new Dictionary<MarketDataId<object>, object>();

		foreach (RatesCurveInputs input in inputs)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<? extends com.opengamma.strata.data.MarketDataId<?>, ?> inputMarketData = input.getMarketData();
		  IDictionary<MarketDataId<object>, ?> inputMarketData = input.MarketData;

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: for (java.util.Map.Entry<? extends com.opengamma.strata.data.MarketDataId<?>, ?> entry : inputMarketData.entrySet())
		  foreach (KeyValuePair<MarketDataId<object>, ?> entry in inputMarketData.SetOfKeyValuePairs())
		  {
			object existingValue = marketDataMap[entry.Key];

			// If the same identifier is used by multiple different curves the corresponding market data value must be equal
			if (existingValue == null)
			{
			  marketDataMap[entry.Key] = entry.Value;
			}
			else if (!existingValue.Equals(entry.Value))
			{
			  throw new System.ArgumentException(Messages.format("Multiple unequal values found for identifier {}. Values: {} and {}", entry.Key, existingValue, entry.Value));
			}
		  }
		}
		return ImmutableMarketData.builder(valuationDate).values(marketDataMap).timeSeries(fixings).build();
	  }

	  private RatesCurveGroup buildGroup(RatesCurveGroupDefinition groupDefn, RatesCurveCalibrator calibrator, MarketData marketData, ReferenceData refData)
	  {

		// perform the calibration
		ImmutableRatesProvider calibratedProvider = calibrator.calibrate(groupDefn, marketData, refData);

		return RatesCurveGroup.of(groupDefn.Name, calibratedProvider.DiscountCurves, calibratedProvider.IndexCurves);
	  }

	  private static int scenarioCount(MarketDataBox<LocalDate> valuationDate, IList<MarketDataBox<RatesCurveInputs>> curveInputBoxes)
	  {

		int scenarioCount = 0;

		if (valuationDate.ScenarioValue)
		{
		  scenarioCount = valuationDate.ScenarioCount;
		}
		foreach (MarketDataBox<RatesCurveInputs> box in curveInputBoxes)
		{
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
				throw new System.ArgumentException(Messages.format("All boxes must have the same number of scenarios, current count = {}, box {} has {}", scenarioCount, box, box.ScenarioCount));
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

	  /// <summary>
	  /// Returns the inputs required for the curve if available.
	  /// <para>
	  /// If no market data is required to build the curve an empty set of inputs is returned.
	  /// If the curve requires inputs which are available in {@code marketData} they are returned.
	  /// If the curve requires inputs which are not available in {@code marketData} an exception is thrown
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveDefn">  the curve definition </param>
	  /// <param name="marketData">  the market data </param>
	  /// <param name="groupName">  the name of the curve group being built </param>
	  /// <param name="obsSource">  the source of the observable market data </param>
	  /// <returns> the input data required for the curve if available </returns>
	  private MarketDataBox<RatesCurveInputs> curveInputs(CurveDefinition curveDefn, ScenarioMarketData marketData, CurveGroupName groupName, ObservableSource obsSource)
	  {

		// only try to get inputs from the market data if the curve needs market data
		if (requiresMarketData(curveDefn))
		{
		  RatesCurveInputsId curveInputsId = RatesCurveInputsId.of(groupName, curveDefn.Name, obsSource);
		  return marketData.getValue(curveInputsId);
		}
		else
		{
		  return MarketDataBox.ofSingleValue(RatesCurveInputs.builder().build());
		}
	  }

	  /// <summary>
	  /// Checks if the curve configuration requires market data.
	  /// <para>
	  /// If the curve configuration contains all the data required to build the curve it is not necessary to
	  /// request input data for the curve points. However if market data is required for any point on the
	  /// curve this function must add <seealso cref="RatesCurveInputs"/> to its market data requirements.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveDefn">  the curve definition </param>
	  /// <returns> true if the curve requires market data for calibration </returns>
	  private bool requiresMarketData(CurveDefinition curveDefn)
	  {
		return curveDefn.Nodes.Any(node => !node.requirements().Empty);
	  }
	}

}