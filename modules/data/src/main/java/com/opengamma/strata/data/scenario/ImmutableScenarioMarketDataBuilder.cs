using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;

	/// <summary>
	/// A mutable builder for market data.
	/// <para>
	/// This is used to create implementations of <seealso cref="ImmutableScenarioMarketData"/>.
	/// </para>
	/// </summary>
	public sealed class ImmutableScenarioMarketDataBuilder
	{

	  /// <summary>
	  /// The number of scenarios.
	  /// </summary>
	  private int scenarioCount;
	  /// <summary>
	  /// The valuation date associated with each scenario.
	  /// </summary>
	  private readonly MarketDataBox<LocalDate> valuationDate;
	  /// <summary>
	  /// The individual items of market data.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<com.opengamma.strata.data.MarketDataId<?>, MarketDataBox<?>> values = new java.util.HashMap<>();
	  private readonly IDictionary<MarketDataId<object>, MarketDataBox<object>> values_Renamed = new Dictionary<MarketDataId<object>, MarketDataBox<object>>();
	  /// <summary>
	  /// The time-series of market data values.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly IDictionary<ObservableId, LocalDateDoubleTimeSeries> timeSeries_Renamed = new Dictionary<ObservableId, LocalDateDoubleTimeSeries>();

	  //-------------------------------------------------------------------------
	  internal ImmutableScenarioMarketDataBuilder(LocalDate valuationDate)
	  {
		ArgChecker.notNull(valuationDate, "valuationDate");
		this.scenarioCount = -1;
		this.valuationDate = MarketDataBox.ofSingleValue(valuationDate);
	  }

	  internal ImmutableScenarioMarketDataBuilder(MarketDataBox<LocalDate> valuationDate)
	  {
		ArgChecker.notNull(valuationDate, "valuationDate");
		this.scenarioCount = -1;
		this.valuationDate = valuationDate;
	  }

	  internal ImmutableScenarioMarketDataBuilder<T1, T2>(int scenarioCount, MarketDataBox<LocalDate> valuationDate, IDictionary<T1> values, IDictionary<T2> timeSeries) where T1 : com.opengamma.strata.data.MarketDataId<T1> where T2 : com.opengamma.strata.data.ObservableId
	  {

		ArgChecker.notNegative(scenarioCount, "scenarioCount");
		ArgChecker.notNull(valuationDate, "valuationDate");
		ArgChecker.notNull(values, "values");
		ArgChecker.notNull(timeSeries, "timeSeries");
		this.scenarioCount = scenarioCount;
		this.valuationDate = valuationDate;
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		this.values_Renamed.putAll(values);
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		this.timeSeries_Renamed.putAll(timeSeries);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Sets the values in the builder, replacing any existing values.
	  /// </summary>
	  /// <param name="values">  the values </param>
	  /// <returns> this builder </returns>
	  public ImmutableScenarioMarketDataBuilder values<T1>(IDictionary<T1> values) where T1 : com.opengamma.strata.data.MarketDataId<T1>
	  {
		this.values_Renamed.Clear();
		return addValueMap(values);
	  }

	  /// <summary>
	  /// Sets the time-series in the builder, replacing any existing values.
	  /// </summary>
	  /// <param name="timeSeries">  the time-series </param>
	  /// <returns> this builder </returns>
	  public ImmutableScenarioMarketDataBuilder timeSeries<T1>(IDictionary<T1> timeSeries) where T1 : com.opengamma.strata.data.ObservableId
	  {
		this.timeSeries_Renamed.Clear();
		return addTimeSeriesMap(timeSeries);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds market data that is valid for all scenarios.
	  /// <para>
	  /// Any existing value with the same identifier will be replaced.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="id">  the identifier </param>
	  /// <param name="value">  the market data value </param>
	  /// @param <T>  the type of the market data value </param>
	  /// <returns> this builder </returns>
	  public ImmutableScenarioMarketDataBuilder addValue<T>(MarketDataId<T> id, T value)
	  {
		ArgChecker.notNull(id, "id");
		ArgChecker.notNull(value, "value");
		values_Renamed[id] = MarketDataBox.ofSingleValue(value);
		return this;
	  }

	  /// <summary>
	  /// Adds market data values that are valid for all scenarios.
	  /// <para>
	  /// Each value in the map is a single item of market data used in all scenarios.
	  /// Any existing value with the same identifier will be replaced.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="values">  the items of market data, keyed by identifier </param>
	  /// <returns> this builder </returns>
	  public ImmutableScenarioMarketDataBuilder addValueMap<T1>(IDictionary<T1> values) where T1 : com.opengamma.strata.data.MarketDataId<T1>
	  {
		ArgChecker.notNull(values, "values");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: for (java.util.Map.Entry<? extends com.opengamma.strata.data.MarketDataId<?>, ?> entry : values.entrySet())
		foreach (KeyValuePair<MarketDataId<object>, ?> entry in values.SetOfKeyValuePairs())
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.MarketDataId<?> id = entry.getKey();
		  MarketDataId<object> id = entry.Key;
		  object value = entry.Value;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: MarketDataBox<?> box = MarketDataBox.ofSingleValue(value);
		  MarketDataBox<object> box = MarketDataBox.ofSingleValue(value);
		  checkBoxType(id, box);
		  checkAndUpdateScenarioCount(box);
		  this.values_Renamed[id] = box;
		}
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds market data for each scenario.
	  /// <para>
	  /// Any existing value with the same identifier will be replaced.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="id">  the identifier </param>
	  /// <param name="values">  the market data values, one for each scenario </param>
	  /// @param <T>  the type of the market data values </param>
	  /// <returns> this builder </returns>
	  public ImmutableScenarioMarketDataBuilder addScenarioValue<T, T1>(MarketDataId<T> id, IList<T1> values) where T1 : T
	  {
		ArgChecker.notNull(id, "id");
		ArgChecker.notNull(values, "values");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: MarketDataBox<? extends T> box = MarketDataBox.ofScenarioValues(values);
		MarketDataBox<T> box = MarketDataBox.ofScenarioValues(values);
		checkAndUpdateScenarioCount(box);
		this.values_Renamed[id] = box;
		return this;
	  }

	  /// <summary>
	  /// Adds market data for each scenario.
	  /// <para>
	  /// Any existing value with the same identifier will be replaced.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="id">  the identifier </param>
	  /// <param name="value">  the market data values, one for each scenario </param>
	  /// @param <T>  the type of the market data values </param>
	  /// <returns> this builder </returns>
	  public ImmutableScenarioMarketDataBuilder addScenarioValue<T, T1>(MarketDataId<T> id, ScenarioArray<T1> value) where T1 : T
	  {
		ArgChecker.notNull(id, "id");
		ArgChecker.notNull(value, "values");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: MarketDataBox<? extends T> box = MarketDataBox.ofScenarioValue(value);
		MarketDataBox<T> box = MarketDataBox.ofScenarioValue(value);
		checkAndUpdateScenarioCount(box);
		this.values_Renamed[id] = box;
		return this;
	  }

	  /// <summary>
	  /// Adds market data values for each scenario.
	  /// <para>
	  /// Each value in the map contains multiple market data items, one for each scenario.
	  /// Any existing value with the same identifier will be replaced.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="values">  the items of market data, keyed by identifier </param>
	  /// <returns> this builder </returns>
	  public ImmutableScenarioMarketDataBuilder addScenarioValueMap<T1>(IDictionary<T1> values) where T1 : com.opengamma.strata.data.MarketDataId<T1>
	  {

		ArgChecker.notNull(values, "values");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: for (java.util.Map.Entry<? extends com.opengamma.strata.data.MarketDataId<?>, ? extends ScenarioArray<?>> entry : values.entrySet())
		foreach (KeyValuePair<MarketDataId<object>, ? extends ScenarioArray<object>> entry in values.SetOfKeyValuePairs())
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.MarketDataId<?> id = entry.getKey();
		  MarketDataId<object> id = entry.Key;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: ScenarioArray<?> value = entry.getValue();
		  ScenarioArray<object> value = entry.Value;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: MarketDataBox<?> box = MarketDataBox.ofScenarioValue(value);
		  MarketDataBox<object> box = MarketDataBox.ofScenarioValue(value);
		  checkBoxType(id, box);
		  checkAndUpdateScenarioCount(box);
		  this.values_Renamed[id] = box;
		}
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds market data wrapped in a box.
	  /// <para>
	  /// Any existing value with the same identifier will be replaced.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="id">  the identifier </param>
	  /// <param name="value">  the market data value </param>
	  /// @param <T>  the type of the market data value </param>
	  /// <returns> this builder </returns>
	  public ImmutableScenarioMarketDataBuilder addBox<T, T1>(MarketDataId<T> id, MarketDataBox<T1> value) where T1 : T
	  {
		ArgChecker.notNull(id, "id");
		ArgChecker.notNull(value, "value");
		checkAndUpdateScenarioCount(value);
		values_Renamed[id] = value;
		return this;
	  }

	  /// <summary>
	  /// Adds market data values for each scenario.
	  /// <para>
	  /// Each value in the map is a market data box.
	  /// Any existing value with the same identifier will be replaced.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="values">  the items of market data, keyed by identifier </param>
	  /// <returns> this builder </returns>
	  public ImmutableScenarioMarketDataBuilder addBoxMap<T1>(IDictionary<T1> values) where T1 : com.opengamma.strata.data.MarketDataId<T1>
	  {

		ArgChecker.notNull(values, "values");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: for (java.util.Map.Entry<? extends com.opengamma.strata.data.MarketDataId<?>, ? extends MarketDataBox<?>> entry : values.entrySet())
		foreach (KeyValuePair<MarketDataId<object>, ? extends MarketDataBox<object>> entry in values.SetOfKeyValuePairs())
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.MarketDataId<?> id = entry.getKey();
		  MarketDataId<object> id = entry.Key;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: MarketDataBox<?> box = entry.getValue();
		  MarketDataBox<object> box = entry.Value;
		  checkBoxType(id, box);
		  checkAndUpdateScenarioCount(box);
		  this.values_Renamed[id] = box;
		}
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds a time-series of observable market data values.
	  /// <para>
	  /// Any existing time-series with the same identifier will be replaced.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="id">  the identifier </param>
	  /// <param name="timeSeries">  a time-series of observable market data values </param>
	  /// <returns> this builder </returns>
	  public ImmutableScenarioMarketDataBuilder addTimeSeries(ObservableId id, LocalDateDoubleTimeSeries timeSeries)
	  {
		ArgChecker.notNull(id, "id");
		ArgChecker.notNull(timeSeries, "timeSeries");
		this.timeSeries_Renamed[id] = timeSeries;
		return this;
	  }

	  /// <summary>
	  /// Adds multiple time-series of observable market data values to the builder.
	  /// <para>
	  /// Any existing time-series with the same identifier will be replaced.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="timeSeriesMap">  the map of time-series </param>
	  /// <returns> this builder </returns>
	  public ImmutableScenarioMarketDataBuilder addTimeSeriesMap<T1>(IDictionary<T1> timeSeriesMap) where T1 : com.opengamma.strata.data.ObservableId
	  {

		ArgChecker.notNull(timeSeriesMap, "timeSeriesMap");
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		this.timeSeries_Renamed.putAll(timeSeriesMap);
		return this;
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

	  private void checkAndUpdateScenarioCount<T1>(MarketDataBox<T1> value)
	  {
		if (value.ScenarioValue)
		{
		  if (scenarioCount == -1)
		  {
			scenarioCount = value.ScenarioCount;
		  }
		  else if (value.ScenarioCount != scenarioCount)
		  {
			throw new System.ArgumentException(Messages.format("All values must have the same number of scenarios, expecting {} but received {}", scenarioCount, value.ScenarioCount));
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Builds the market data.
	  /// </summary>
	  /// <returns> the market data </returns>
	  public ImmutableScenarioMarketData build()
	  {
		if (scenarioCount == -1)
		{
		  scenarioCount = 1;
		}
		return new ImmutableScenarioMarketData(scenarioCount, valuationDate, values_Renamed, timeSeries_Renamed);
	  }

	}

}