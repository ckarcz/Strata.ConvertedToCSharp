using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;

	/// <summary>
	/// A mutable builder for instances of <seealso cref="ImmutableMarketData"/>.
	/// </summary>
	public class ImmutableMarketDataBuilder
	{

	  /// <summary>
	  /// The valuation date associated with the market data.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private LocalDate valuationDate_Renamed;
	  /// <summary>
	  /// The market data values.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<MarketDataId<?>, Object> values = new java.util.HashMap<>();
	  private readonly IDictionary<MarketDataId<object>, object> values_Renamed = new Dictionary<MarketDataId<object>, object>();
	  /// <summary>
	  /// The time-series of historic market data values.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly IDictionary<ObservableId, LocalDateDoubleTimeSeries> timeSeries_Renamed = new Dictionary<ObservableId, LocalDateDoubleTimeSeries>();

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an empty builder.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  internal ImmutableMarketDataBuilder(LocalDate valuationDate)
	  {
		this.valuationDate_Renamed = ArgChecker.notNull(valuationDate, "valuationDate");
	  }

	  /// <summary>
	  /// Creates a builder pre-populated with data.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date associated with the market data </param>
	  /// <param name="values">  the single value market data items, keyed by identifier </param>
	  /// <param name="timeSeries">  time-series of observable market data values, keyed by identifier </param>
	  internal ImmutableMarketDataBuilder<T1>(LocalDate valuationDate, IDictionary<T1> values, IDictionary<ObservableId, LocalDateDoubleTimeSeries> timeSeries)
	  {

		this.valuationDate_Renamed = ArgChecker.notNull(valuationDate, "valuationDate");
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		this.values_Renamed.putAll(ArgChecker.notNull(values, "values"));
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		this.timeSeries_Renamed.putAll(ArgChecker.notNull(timeSeries, "timeSeries"));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Sets the valuation date.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date to set </param>
	  /// <returns> this builder </returns>
	  public virtual ImmutableMarketDataBuilder valuationDate(LocalDate valuationDate)
	  {
		ArgChecker.notNull(valuationDate, "valuationDate");
		this.valuationDate_Renamed = valuationDate;
		return this;
	  }

	  /// <summary>
	  /// Sets the values in the builder, replacing any existing values.
	  /// </summary>
	  /// <param name="values">  the values </param>
	  /// <returns> this builder </returns>
	  public virtual ImmutableMarketDataBuilder values<T1>(IDictionary<T1> values) where T1 : MarketDataId<T1>
	  {
		this.values_Renamed.Clear();
		return addValueMap(values);
	  }

	  /// <summary>
	  /// Sets the time-series in the builder, replacing any existing values.
	  /// </summary>
	  /// <param name="timeSeries">  the time-series </param>
	  /// <returns> this builder </returns>
	  public virtual ImmutableMarketDataBuilder timeSeries<T1>(IDictionary<T1> timeSeries) where T1 : ObservableId
	  {
		this.timeSeries_Renamed.Clear();
		return addTimeSeriesMap(timeSeries);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds a value to the builder.
	  /// </summary>
	  /// <param name="id">  the identifier </param>
	  /// <param name="value">  the market data value </param>
	  /// @param <T>  the type of the market data value </param>
	  /// <returns> this builder </returns>
	  public virtual ImmutableMarketDataBuilder addValue<T>(MarketDataId<T> id, T value)
	  {
		ArgChecker.notNull(id, "id");
		ArgChecker.notNull(value, "value");
		values_Renamed[id] = value;
		return this;
	  }

	  /// <summary>
	  /// Adds a value to the builder when the types are not known at compile time.
	  /// </summary>
	  /// <param name="id">  the identifier </param>
	  /// <param name="value">  the market data value </param>
	  /// @param <T>  the type of the market data value </param>
	  /// <returns> this builder </returns>
	  public virtual ImmutableMarketDataBuilder addValueUnsafe<T, T1>(MarketDataId<T1> id, object value)
	  {
		ArgChecker.notNull(id, "id");
		ArgChecker.notNull(value, "value");
		ImmutableMarketData.checkType(id, value);
		values_Renamed[id] = value;
		return this;
	  }

	  /// <summary>
	  /// Adds multiple values to the builder.
	  /// </summary>
	  /// <param name="values">  the values </param>
	  /// <returns> this builder </returns>
	  public virtual ImmutableMarketDataBuilder addValueMap<T1>(IDictionary<T1> values) where T1 : MarketDataId<T1>
	  {
		ArgChecker.notNull(values, "values");
		values.SetOfKeyValuePairs().forEach(e => addValueUnsafe(e.Key, e.Value));
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
	  public virtual ImmutableMarketDataBuilder addTimeSeries(ObservableId id, LocalDateDoubleTimeSeries timeSeries)
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
	  public virtual ImmutableMarketDataBuilder addTimeSeriesMap<T1>(IDictionary<T1> timeSeriesMap) where T1 : ObservableId
	  {

		ArgChecker.notNull(timeSeriesMap, "timeSeriesMap");
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		this.timeSeries_Renamed.putAll(timeSeriesMap);
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a set of market data built from the data in this builder.
	  /// </summary>
	  /// <returns> a set of market data built from the data in this builder </returns>
	  public virtual ImmutableMarketData build()
	  {
		return new ImmutableMarketData(valuationDate_Renamed, values_Renamed, timeSeries_Renamed);
	  }

	}

}