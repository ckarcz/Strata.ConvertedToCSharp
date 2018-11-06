using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.timeseries
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.timeseries.DenseLocalDateDoubleTimeSeries.DenseTimeSeriesCalculation.INCLUDE_WEEKENDS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.timeseries.DenseLocalDateDoubleTimeSeries.DenseTimeSeriesCalculation.SKIP_WEEKENDS;


	/// <summary>
	/// Builder to create the immutable {@code LocalDateDoubleTimeSeries}.
	/// <para>
	/// This builder allows a time-series to be created.
	/// Entries can be added to the builder in any order.
	/// If a date is duplicated it will overwrite an earlier entry.
	/// </para>
	/// <para>
	/// Use <seealso cref="LocalDateDoubleTimeSeries#builder()"/> to create an instance.
	/// </para>
	/// </summary>
	public sealed class LocalDateDoubleTimeSeriesBuilder
	{

	  /// <summary>
	  /// Threshold for deciding whether we use the dense or sparse time-series implementation.
	  /// </summary>
	  private const double DENSITY_THRESHOLD = 0.7;

	  /// <summary>
	  /// The entries for the time-series.
	  /// </summary>
	  private readonly SortedDictionary<LocalDate, double> entries = new SortedDictionary<LocalDate, double>();

	  /// <summary>
	  /// Keep track of whether we have weekends in the data.
	  /// </summary>
	  private bool containsWeekends;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance.
	  /// <para>
	  /// Use <seealso cref="LocalDateDoubleTimeSeries#builder()"/>.
	  /// </para>
	  /// </summary>
	  internal LocalDateDoubleTimeSeriesBuilder()
	  {
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// <para>
	  /// Use <seealso cref="LocalDateDoubleTimeSeries#toBuilder()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="dates">  the dates to initialize with </param>
	  /// <param name="values">  the values to initialize with </param>
	  internal LocalDateDoubleTimeSeriesBuilder(LocalDate[] dates, double[] values)
	  {
		for (int i = 0; i < dates.Length; i++)
		{
		  put(dates[i], values[i]);
		}
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// <para>
	  /// Use <seealso cref="DenseLocalDateDoubleTimeSeries#toBuilder()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="points">  the stream of points to initialize with </param>
	  internal LocalDateDoubleTimeSeriesBuilder(Stream<LocalDateDoublePoint> points)
	  {
		points.forEach(pt => put(pt.Date, pt.Value));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the value associated with the specified date.
	  /// <para>
	  /// The result is an <seealso cref="OptionalDouble"/> which avoids the need to handle null
	  /// or exceptions. Use {@code isPresent()} to check whether the value is present.
	  /// Use {@code orElse(double)} to default a missing value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to get the value for </param>
	  /// <returns> the value associated with the date, optional empty if the date is not present </returns>
	  public double? get(LocalDate date)
	  {
		double? value = entries[date];
		return (value != null ? double?.of(value) : double?.empty());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Puts the specified date/value point into this builder.
	  /// </summary>
	  /// <param name="date">  the date to be added </param>
	  /// <param name="value">  the value associated with the date </param>
	  /// <returns> this builder </returns>
	  public LocalDateDoubleTimeSeriesBuilder put(LocalDate date, double value)
	  {
		ArgChecker.notNull(date, "date");
		ArgChecker.isFalse(double.IsNaN(value), "NaN is not allowed as a value");
		entries[date] = value;
		if (!containsWeekends && date.get(ChronoField.DAY_OF_WEEK) > 5)
		{
		  containsWeekends = true;
		}
		return this;
	  }

	  /// <summary>
	  /// Puts the specified date/value point into this builder.
	  /// </summary>
	  /// <param name="point">  the point to be added </param>
	  /// <returns> this builder </returns>
	  public LocalDateDoubleTimeSeriesBuilder put(LocalDateDoublePoint point)
	  {
		ArgChecker.notNull(point, "point");
		put(point.Date, point.Value);
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Merges the specified date/value point into this builder.
	  /// <para>
	  /// The operator is invoked if the date already exists.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to be added </param>
	  /// <param name="value">  the value associated with the date </param>
	  /// <param name="operator">  the operator to use for merging </param>
	  /// <returns> this builder </returns>
	  public LocalDateDoubleTimeSeriesBuilder merge(LocalDate date, double value, System.Func<double, double, double> @operator)
	  {
		ArgChecker.notNull(date, "date");
		ArgChecker.notNull(@operator, "operator");
		entries.merge(date, value, (a, b) => @operator(a, b));
		return this;
	  }

	  /// <summary>
	  /// Merges the specified date/value point into this builder.
	  /// <para>
	  /// The operator is invoked if the date already exists.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="point">  the point to be added </param>
	  /// <param name="operator">  the operator to use for merging </param>
	  /// <returns> this builder </returns>
	  public LocalDateDoubleTimeSeriesBuilder merge(LocalDateDoublePoint point, System.Func<double, double, double> @operator)
	  {
		ArgChecker.notNull(point, "point");
		entries.merge(point.Date, point.Value, (a, b) => @operator(a, b));
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Puts all the specified dates and values into this builder.
	  /// <para>
	  /// The date and value collections must be the same size.
	  /// </para>
	  /// <para>
	  /// The date-value pairs are added one by one.
	  /// If a date is duplicated it will overwrite an earlier entry.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="dates">  the dates to be added </param>
	  /// <param name="values">  the values to be added </param>
	  /// <returns> this builder </returns>
	  public LocalDateDoubleTimeSeriesBuilder putAll(ICollection<LocalDate> dates, ICollection<double> values)
	  {
		ArgChecker.noNulls(dates, "dates");
		ArgChecker.noNulls(values, "values");
		ArgChecker.isTrue(dates.Count == values.Count, "Arrays are of different sizes - dates: {}, values: {}", dates.Count, values.Count);
		IEnumerator<LocalDate> itDate = dates.GetEnumerator();
		IEnumerator<double> itValue = values.GetEnumerator();
		for (int i = 0; i < dates.Count; i++)
		{
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		  put(itDate.next(), itValue.next());
		}
		return this;
	  }

	  /// <summary>
	  /// Puts all the specified dates and values into this builder.
	  /// <para>
	  /// The date collection and value array must be the same size.
	  /// </para>
	  /// <para>
	  /// The date-value pairs are added one by one.
	  /// If a date is duplicated it will overwrite an earlier entry.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="dates">  the dates to be added </param>
	  /// <param name="values">  the values to be added </param>
	  /// <returns> this builder </returns>
	  public LocalDateDoubleTimeSeriesBuilder putAll(ICollection<LocalDate> dates, double[] values)
	  {
		ArgChecker.noNulls(dates, "dates");
		ArgChecker.notNull(values, "values");
		ArgChecker.isTrue(dates.Count == values.Length, "Arrays are of different sizes - dates: {}, values: {}", dates.Count, values.Length);
		IEnumerator<LocalDate> itDate = dates.GetEnumerator();
		for (int i = 0; i < dates.Count; i++)
		{
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		  put(itDate.next(), values[i]);
		}
		return this;
	  }

	  /// <summary>
	  /// Puts all the specified points into this builder.
	  /// <para>
	  /// The points are added one by one.
	  /// If a date is duplicated it will overwrite an earlier entry.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="points">  the points to be added </param>
	  /// <returns> this builder </returns>
	  public LocalDateDoubleTimeSeriesBuilder putAll(Stream<LocalDateDoublePoint> points)
	  {
		ArgChecker.notNull(points, "points");
		points.forEach(this.put);
		return this;
	  }

	  /// <summary>
	  /// Puts all the specified points into this builder.
	  /// <para>
	  /// The points are added one by one.
	  /// If a date is duplicated it will overwrite an earlier entry.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="points">  the points to be added </param>
	  /// <returns> this builder </returns>
	  public LocalDateDoubleTimeSeriesBuilder putAll(IList<LocalDateDoublePoint> points)
	  {
		ArgChecker.notNull(points, "points");
		return putAll(points.stream());
	  }

	  /// <summary>
	  /// Puts the contents of the specified builder into this builder.
	  /// <para>
	  /// The points are added one by one.
	  /// If a date is duplicated it will overwrite an earlier entry.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other builder </param>
	  /// <returns> this builder </returns>
	  public LocalDateDoubleTimeSeriesBuilder putAll(LocalDateDoubleTimeSeriesBuilder other)
	  {
		ArgChecker.notNull(other, "other");
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		entries.putAll(other.entries);
		containsWeekends = containsWeekends || other.containsWeekends;
		return this;
	  }

	  /// <summary>
	  /// Puts all the entries from the supplied map into this builder.
	  /// <para>
	  /// If a date is duplicated it will overwrite an earlier entry.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="map">  the map of points to be added </param>
	  /// <returns> this builder </returns>
	  public LocalDateDoubleTimeSeriesBuilder putAll(IDictionary<LocalDate, double> map)
	  {
		ArgChecker.noNulls(map, "map");
		map.SetOfKeyValuePairs().forEach(e => put(e.Key, e.Value));
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Build the time-series from the builder.
	  /// </summary>
	  /// <returns> a time-series containing the entries from the builder </returns>
	  public LocalDateDoubleTimeSeries build()
	  {

		if (entries.Count == 0)
		{
		  return LocalDateDoubleTimeSeries.empty();
		}

		// Depending on how dense the data is, judge which type of time series
		// is the best fit
		return density() > DENSITY_THRESHOLD ? createDenseSeries() : createSparseSeries();
	  }

	  private LocalDateDoubleTimeSeries createDenseSeries()
	  {
		return DenseLocalDateDoubleTimeSeries.of(entries.firstKey(), entries.lastKey(), streamEntries(), determineCalculation());
	  }

	  private SparseLocalDateDoubleTimeSeries createSparseSeries()
	  {
		return SparseLocalDateDoubleTimeSeries.of(entries.Keys, entries.Values);
	  }

	  private Stream<LocalDateDoublePoint> streamEntries()
	  {
		return entries.SetOfKeyValuePairs().Select(e => LocalDateDoublePoint.of(e.Key, e.Value));
	  }

	  private DenseLocalDateDoubleTimeSeries.DenseTimeSeriesCalculation determineCalculation()
	  {
		return containsWeekends ? INCLUDE_WEEKENDS : SKIP_WEEKENDS;
	  }

	  private double density()
	  {
		// We can use the calculators to work out range size
		double rangeSize = determineCalculation().calculatePosition(entries.firstKey(), entries.lastKey()) + 1;
		return entries.Count / rangeSize;
	  }

	}

}