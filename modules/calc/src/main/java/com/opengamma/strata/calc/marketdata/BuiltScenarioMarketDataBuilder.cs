using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.marketdata
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using Failure = com.opengamma.strata.collect.result.Failure;
	using Result = com.opengamma.strata.collect.result.Result;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using ImmutableScenarioMarketData = com.opengamma.strata.data.scenario.ImmutableScenarioMarketData;
	using ImmutableScenarioMarketDataBuilder = com.opengamma.strata.data.scenario.ImmutableScenarioMarketDataBuilder;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;

	/// <summary>
	/// A mutable builder for building up <seealso cref="BuiltScenarioMarketData"/> instances.
	/// </summary>
	internal sealed class BuiltScenarioMarketDataBuilder
	{

	  /// <summary>
	  /// The valuation date associated with the market data. </summary>
	  private MarketDataBox<LocalDate> valuationDate = MarketDataBox.empty();

	  /// <summary>
	  /// The number of scenarios for which this builder contains market data. </summary>
	  private int scenarioCount;

	  /// <summary>
	  /// The single value market data items, keyed by ID. </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<com.opengamma.strata.data.MarketDataId<?>, com.opengamma.strata.data.scenario.MarketDataBox<?>> values = new java.util.HashMap<>();
	  private readonly IDictionary<MarketDataId<object>, MarketDataBox<object>> values = new Dictionary<MarketDataId<object>, MarketDataBox<object>>();

	  /// <summary>
	  /// Time series of observable market data values, keyed by ID. </summary>
	  private readonly IDictionary<ObservableId, LocalDateDoubleTimeSeries> timeSeries = new Dictionary<ObservableId, LocalDateDoubleTimeSeries>();

	  /// <summary>
	  /// Details of failures when building single market data values. </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<com.opengamma.strata.data.MarketDataId<?>, com.opengamma.strata.collect.result.Failure> valueFailures = new java.util.HashMap<>();
	  private readonly IDictionary<MarketDataId<object>, Failure> valueFailures = new Dictionary<MarketDataId<object>, Failure>();

	  /// <summary>
	  /// Details of failures when building time series of market data values. </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<com.opengamma.strata.data.MarketDataId<?>, com.opengamma.strata.collect.result.Failure> timeSeriesFailures = new java.util.HashMap<>();
	  private readonly IDictionary<MarketDataId<object>, Failure> timeSeriesFailures = new Dictionary<MarketDataId<object>, Failure>();

	  /// <summary>
	  /// Creates a builder pre-populated with the valuation date.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date associated with the market data </param>
	  internal BuiltScenarioMarketDataBuilder(LocalDate valuationDate)
	  {
		ArgChecker.notNull(valuationDate, "valuationDate");
		this.valuationDate = MarketDataBox.ofSingleValue(valuationDate);
		updateScenarioCount(this.valuationDate);
	  }

	  /// <summary>
	  /// Creates a builder pre-populated with the valuation date.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date associated with the market data </param>
	  internal BuiltScenarioMarketDataBuilder(MarketDataBox<LocalDate> valuationDate)
	  {
		ArgChecker.notNull(valuationDate, "valuationDate");

		if (valuationDate.ScenarioCount == 0)
		{
		  throw new System.ArgumentException("Valuation date must not be empty");
		}
		updateScenarioCount(valuationDate);
		this.valuationDate = valuationDate;
	  }

	  /// <summary>
	  /// Creates a builder pre-populated with data.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date associated with the market data </param>
	  /// <param name="scenarioCount">  the number of scenarios for which this builder contains market data </param>
	  /// <param name="values">  the single value market data items, keyed by ID </param>
	  /// <param name="timeSeries">  time series of observable market data values, keyed by ID </param>
	  /// <param name="valueFailures">  details of failures encountered when building market data values </param>
	  /// <param name="timeSeriesFailures">  details of failures encountered when building time series </param>
	  internal BuiltScenarioMarketDataBuilder<T1, T2, T3, T4>(MarketDataBox<LocalDate> valuationDate, int scenarioCount, IDictionary<T1> values, IDictionary<T2> timeSeries, IDictionary<T3> valueFailures, IDictionary<T4> timeSeriesFailures) where T1 : com.opengamma.strata.data.MarketDataId<T1> where T2 : com.opengamma.strata.data.ObservableId
	  {

		this.valuationDate = ArgChecker.notNull(valuationDate, "valuationDate");
		this.scenarioCount = scenarioCount;
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		this.values.putAll(values);
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		this.timeSeries.putAll(timeSeries);
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		this.valueFailures.putAll(valueFailures);
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		this.timeSeriesFailures.putAll(timeSeriesFailures);
	  }

	  /// <summary>
	  /// Adds a single item of market data, replacing any existing value with the same ID.
	  /// </summary>
	  /// <param name="id">  the ID of the market data </param>
	  /// <param name="value">  the market data value </param>
	  /// @param <T>  the type of the market data value </param>
	  /// <returns> this builder </returns>
	  internal BuiltScenarioMarketDataBuilder addValue<T>(MarketDataId<T> id, T value)
	  {
		ArgChecker.notNull(id, "id");
		ArgChecker.notNull(value, "value");
		values[id] = MarketDataBox.ofSingleValue(value);
		return this;
	  }

	  /// <summary>
	  /// Adds a single market data box, replacing any existing box with the same ID.
	  /// <para>
	  /// The type of the box is checked to ensure it is compatible with the ID.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="id">  the ID of the market data </param>
	  /// <param name="box">  the market data box </param>
	  /// <returns> this builder </returns>
	  internal BuiltScenarioMarketDataBuilder addBox<T1, T2>(MarketDataId<T1> id, MarketDataBox<T2> box)
	  {
		ArgChecker.notNull(id, "id");
		ArgChecker.notNull(box, "box");
		updateScenarioCount(box);
		checkBoxType(id, box);
		values[id] = box;
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds a result for a single item of market data, replacing any existing value with the same ID.
	  /// </summary>
	  /// <param name="id">  the ID of the market data </param>
	  /// <param name="result">  a result containing the market data value or details of why it could not be provided </param>
	  /// @param <T>  the type of the market data value </param>
	  /// <returns> this builder </returns>
	  internal BuiltScenarioMarketDataBuilder addResult<T, T1>(MarketDataId<T> id, Result<T1> result)
	  {
		ArgChecker.notNull(id, "id");
		ArgChecker.notNull(result, "result");

		if (result.Success)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.scenario.MarketDataBox<?> box = result.getValue();
		  MarketDataBox<object> box = result.Value;
		  checkBoxType(id, box);
		  updateScenarioCount(box);
		  values[id] = box;
		  valueFailures.Remove(id);
		}
		else
		{
		  valueFailures[id] = result.Failure;
		  values.Remove(id);
		}
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds a time series of observable market data values, replacing any existing time series with the same ID.
	  /// </summary>
	  /// <param name="id">  the ID of the values </param>
	  /// <param name="timeSeries">  a time series of observable market data values </param>
	  /// <returns> this builder </returns>
	  internal BuiltScenarioMarketDataBuilder addTimeSeries(ObservableId id, LocalDateDoubleTimeSeries timeSeries)
	  {
		ArgChecker.notNull(id, "id");
		ArgChecker.notNull(timeSeries, "timeSeries");
		this.timeSeries[id] = timeSeries;
		return this;
	  }

	  /// <summary>
	  /// Adds a time series of observable market data values, replacing any existing time series with the same ID.
	  /// </summary>
	  /// <param name="id">  the ID of the values </param>
	  /// <param name="result">  a time series of observable market data values </param>
	  /// <returns> this builder </returns>
	  internal BuiltScenarioMarketDataBuilder addTimeSeriesResult(ObservableId id, Result<LocalDateDoubleTimeSeries> result)
	  {
		ArgChecker.notNull(id, "id");
		ArgChecker.notNull(result, "result");

		if (result.Success)
		{
		  timeSeries[id] = result.Value;
		  timeSeriesFailures.Remove(id);
		}
		else
		{
		  timeSeriesFailures[id] = result.Failure;
		  timeSeries.Remove(id);
		}
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Builds a set of market data from the data in this builder.
	  /// <para>
	  /// It is possible to continue to add more data to a builder after calling {@code build()}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a set of market data from the data in this builder </returns>
	  internal BuiltScenarioMarketData build()
	  {
		if (valuationDate.ScenarioCount == 0)
		{
		  // This isn't checked in the main class otherwise it would be impossible to have an empty instance
		  throw new System.ArgumentException("Valuation date must be specified");
		}
		ImmutableScenarioMarketDataBuilder builder = ImmutableScenarioMarketData.builder(valuationDate).addBoxMap(values).addTimeSeriesMap(timeSeries);
		return new BuiltScenarioMarketData(builder.build(), valueFailures, timeSeriesFailures);
	  }

	  //-------------------------------------------------------------------------
	  private static void checkBoxType<T1, T2>(MarketDataId<T1> id, MarketDataBox<T2> box)
	  {
		if (!id.MarketDataType.IsAssignableFrom(box.MarketDataType))
		{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		  throw new System.ArgumentException(Messages.format("Market data type {} of value {} is not compatible with the market data type of the identifier {}", box.MarketDataType.FullName, box, id.MarketDataType.FullName));
		}
	  }

	  private void updateScenarioCount<T1>(MarketDataBox<T1> box)
	  {
		// If the box has a single value then it can be used with any number of scenarios - the same value is used
		// for all scenarios.
		if (box.SingleValue)
		{
		  if (scenarioCount == 0)
		  {
			scenarioCount = 1;
		  }
		  return;
		}
		int scenarioCount = box.ScenarioCount;

		if (this.scenarioCount == 0 || this.scenarioCount == 1)
		{
		  this.scenarioCount = scenarioCount;
		  return;
		}
		if (scenarioCount != this.scenarioCount)
		{
		  throw new System.ArgumentException(Messages.format("All values must have the same number of scenarios, expecting {} but received {}", this.scenarioCount, scenarioCount));
		}
	  }
	}

}