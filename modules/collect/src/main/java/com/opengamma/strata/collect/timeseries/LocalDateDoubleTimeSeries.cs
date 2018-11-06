/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.timeseries
{


	using ObjDoublePredicate = com.opengamma.strata.collect.function.ObjDoublePredicate;
	using Pair = com.opengamma.strata.collect.tuple.Pair;

	/// <summary>
	/// Interface for all local date time-series types containing
	/// {@code double} values.
	/// <para>
	/// A time-series is similar to both a {@code SortedMap} of value keyed
	/// by date-time and a {@code List} of date-time to value pairs. As such,
	/// the date/times do not have to be evenly spread over time within the series.
	/// </para>
	/// <para>
	/// The distribution of the data will influence which implementation
	/// is most appropriate.
	/// </para>
	/// <para>
	/// Implementations must be immutable and thread-safe beans.
	/// </para>
	/// <para>
	/// Note that <seealso cref="Double#NaN"/> is used internally as a sentinel
	/// value and is therefore not allowed as a value.
	/// </para>
	/// </summary>
	public interface LocalDateDoubleTimeSeries
	{

	  /// <summary>
	  /// Returns an empty time-series. Generally a singleton instance
	  /// is returned.
	  /// </summary>
	  /// <returns> an empty time-series </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static LocalDateDoubleTimeSeries empty()
	//  {
	//	return SparseLocalDateDoubleTimeSeries.EMPTY;
	//  }

	  /// <summary>
	  /// Obtains a time-series containing a single date and value.
	  /// </summary>
	  /// <param name="date">  the singleton date </param>
	  /// <param name="value">  the singleton value </param>
	  /// <returns> the time-series </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static LocalDateDoubleTimeSeries of(java.time.LocalDate date, double value)
	//  {
	//	ArgChecker.notNull(date, "date");
	//	return builder().put(date, value).build();
	//  }

	  /// <summary>
	  /// Creates an empty builder, used to create time-series.
	  /// <para>
	  /// The builder has methods to create and modify a time-series.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the time-series builder </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static LocalDateDoubleTimeSeriesBuilder builder()
	//  {
	//	return new LocalDateDoubleTimeSeriesBuilder();
	//  }

	  /// <summary>
	  /// Returns a collector that can be used to create a time-series from a stream of points.
	  /// </summary>
	  /// <returns> the time-series collector </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static java.util.stream.Collector<LocalDateDoublePoint, LocalDateDoubleTimeSeriesBuilder, LocalDateDoubleTimeSeries> collector()
	//  {
	//
	//	return Collector.of(LocalDateDoubleTimeSeriesBuilder::new, LocalDateDoubleTimeSeriesBuilder::put, LocalDateDoubleTimeSeriesBuilder::putAll, LocalDateDoubleTimeSeriesBuilder::build);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Return the size of this time-series.
	  /// </summary>
	  /// <returns> the size of the time-series </returns>
	  int size();

	  /// <summary>
	  /// Indicates if this time-series is empty.
	  /// </summary>
	  /// <returns> true if the time-series contains no entries </returns>
	  bool Empty {get;}

	  /// <summary>
	  /// Checks if this time-series contains a value for the specified date.
	  /// </summary>
	  /// <param name="date">  the date to check for </param>
	  /// <returns> true if there is a value associated with the date </returns>
	  bool containsDate(LocalDate date);

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
	  double? get(LocalDate date);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Get the earliest date contained in this time-series.
	  /// <para>
	  /// If the time-series is empty then <seealso cref="NoSuchElementException"/> will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the earliest date contained in this time-series </returns>
	  /// <exception cref="NoSuchElementException"> if the time-series is empty </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.LocalDate getEarliestDate()
	//  {
	//	return dates().findFirst().orElseThrow(() -> new NoSuchElementException("Unable to return earliest date, time-series is empty"));
	//  }

	  /// <summary>
	  /// Get the value held for the earliest date contained in this time-series.
	  /// <para>
	  /// If the time-series is empty then <seealso cref="NoSuchElementException"/> will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the value held for the earliest date contained in this time-series </returns>
	  /// <exception cref="NoSuchElementException"> if the time-series is empty </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default double getEarliestValue()
	//  {
	//	return values().findFirst().orElseThrow(() -> new NoSuchElementException("Unable to return earliest value, time-series is empty"));
	//  }

	  /// <summary>
	  /// Get the latest date contained in this time-series.
	  /// <para>
	  /// If the time-series is empty then <seealso cref="NoSuchElementException"/> will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the latest date contained in this time-series </returns>
	  /// <exception cref="NoSuchElementException"> if the time-series is empty </exception>
	  LocalDate LatestDate {get;}

	  /// <summary>
	  /// Get the value held for the latest date contained in this time-series.
	  /// <para>
	  /// If the time-series is empty then <seealso cref="NoSuchElementException"/> will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the value held for the latest date contained in this time-series </returns>
	  /// <exception cref="NoSuchElementException"> if the time-series is empty </exception>
	  double LatestValue {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets part of this series as a sub-series between two dates.
	  /// <para>
	  /// The date-times do not have to match exactly.
	  /// The sub-series contains all entries between the two dates using a half-open interval.
	  /// The start date is included, the end date is excluded.
	  /// The dates may be before or after the end of the time-series.
	  /// </para>
	  /// <para>
	  /// To obtain the series before a specific date, used {@code LocalDate.MIN} as the first argument.
	  /// To obtain the series from a specific date, used {@code LocalDate.MAX} as the second argument.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="startInclusive">  the start date, inclusive </param>
	  /// <param name="endExclusive">  the end date, exclusive </param>
	  /// <returns> the sub-series between the dates </returns>
	  /// <exception cref="IllegalArgumentException"> if the end is before the start </exception>
	  LocalDateDoubleTimeSeries subSeries(LocalDate startInclusive, LocalDate endExclusive);

	  /// <summary>
	  /// Gets part of this series as a sub-series, choosing the earliest entries.
	  /// <para>
	  /// The sub-series contains the earliest part of the series up to the specified number of points.
	  /// If the series contains less points than the number requested, the whole time-series is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="numPoints">  the number of items to select, zero or greater </param>
	  /// <returns> the sub-series of the requested size starting with the earliest entry </returns>
	  /// <exception cref="IllegalArgumentException"> if the number of items is less than zero </exception>
	  LocalDateDoubleTimeSeries headSeries(int numPoints);

	  /// <summary>
	  /// Gets part of this series as a sub-series, choosing the latest entries.
	  /// <para>
	  /// The sub-series contains the latest part of the series up to the specified number of points.
	  /// If the series contains less points than the number requested, the whole time-series is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="numPoints">  the number of items to select, zero or greater </param>
	  /// <returns> the sub-series of the requested size ending with the latest entry </returns>
	  /// <exception cref="IllegalArgumentException"> if the number of items is less than zero </exception>
	  LocalDateDoubleTimeSeries tailSeries(int numPoints);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a stream over the points of this time-series.
	  /// <para>
	  /// This provides access to the entire time-series.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a stream over the points of this time-series </returns>
	  Stream<LocalDateDoublePoint> stream();

	  /// <summary>
	  /// Returns a stream over the dates of this time-series.
	  /// <para>
	  /// This is most useful to summarize the dates in the stream, such as calculating
	  /// the maximum or minimum date, or searching for a specific value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a stream over the dates of this time-series </returns>
	  Stream<LocalDate> dates();

	  /// <summary>
	  /// Returns a stream over the values of this time-series.
	  /// <para>
	  /// This is most useful to summarize the values in the stream, such as calculating
	  /// the maximum, minimum or average value, or searching for a specific value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a stream over the values of this time-series </returns>
	  DoubleStream values();

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Applies an action to each pair in the time series.
	  /// <para>
	  /// This is generally used to apply a mathematical operation to the values.
	  /// For example, the operator could multiply each value by a constant, or take the inverse.
	  /// <pre>
	  ///   base.forEach((date, value) -&gt; System.out.println(date + "=" + value));
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="action">  the action to be applied to each pair </param>
	  void forEach(System.Action<LocalDate, double> action);

	  /// <summary>
	  /// Applies an operation to each date in the time series which creates a new date, returning a new time series
	  /// with the new dates and the points from this time series.
	  /// <para>
	  /// This operation creates a new time series with the same data but the dates moved.
	  /// </para>
	  /// <para>
	  /// The operation must not change the dates in a way that reorders them. The mapped dates must be in ascending
	  /// order or an exception is thrown.
	  /// <pre>
	  ///   updatedSeries = timeSeries.mapDates(date -&gt; date.plusYears(1));
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="mapper">  the operation applied to each date in the time series </param>
	  /// <returns> a copy of this time series with new dates </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public abstract LocalDateDoubleTimeSeries mapDates(java.util.function.Function<? super java.time.LocalDate, ? extends java.time.LocalDate> mapper);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  LocalDateDoubleTimeSeries mapDates<T1>(System.Func<T1> mapper);

	  /// <summary>
	  /// Applies an operation to each value in the time series.
	  /// <para>
	  /// This is generally used to apply a mathematical operation to the values.
	  /// For example, the operator could multiply each value by a constant, or take the inverse.
	  /// <pre>
	  ///   multiplied = base.mapValues(value -&gt; value * 3);
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="mapper">  the operator to be applied to the values </param>
	  /// <returns> a copy of this series with the mapping applied to the original values </returns>
	  LocalDateDoubleTimeSeries mapValues(System.Func<double, double> mapper);

	  /// <summary>
	  /// Create a new time-series by filtering this one.
	  /// <para>
	  /// The time-series can be filtered by both date and value.
	  /// Note that if filtering by date range is required, it is likely to be more efficient to
	  /// use <seealso cref="#subSeries(LocalDate, LocalDate)"/> as that avoids traversal of the whole series.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="predicate">  the predicate to use to the filter the elements of the series </param>
	  /// <returns> a filtered version of the series </returns>
	  LocalDateDoubleTimeSeries filter(ObjDoublePredicate<LocalDate> predicate);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains the intersection of a pair of time series.
	  /// <para>
	  /// This returns a time-series with the intersection of the dates of the two inputs.
	  /// The operator is invoked to combine the values.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the time-series to combine with </param>
	  /// <param name="mapper">  the function to be used to combine the values </param>
	  /// <returns> a new time-series containing the dates in common between the
	  ///   input series with their values combined together using the function </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default LocalDateDoubleTimeSeries intersection(LocalDateDoubleTimeSeries other, System.Func<double, double, double> mapper)
	//  {
	//	ArgChecker.notNull(other, "other");
	//	ArgChecker.notNull(mapper, "mapper");
	//	return new LocalDateDoubleTimeSeriesBuilder().putAll(stream().filter(pt -> other.containsDate(pt.getDate())).map(pt -> LocalDateDoublePoint.of(pt.getDate(), mapper.applyAsDouble(pt.getValue(), other.get(pt.getDate()).getAsDouble())))).build();
	//  }

	  /// <summary>
	  /// Obtains the union of a pair of time series.
	  /// <para>
	  /// This returns a time-series with the union of the dates of the two inputs.
	  /// When the same date occurs in both time-series, the operator is invoked to combine the values.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the time-series to combine with </param>
	  /// <param name="mapper">  the function to be used to combine the values </param>
	  /// <returns> a new time-series containing the dates in common between the
	  ///   input series with their values combined together using the function </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default LocalDateDoubleTimeSeries union(LocalDateDoubleTimeSeries other, System.Func<double, double, double> mapper)
	//  {
	//	ArgChecker.notNull(other, "other");
	//	ArgChecker.notNull(mapper, "mapper");
	//	LocalDateDoubleTimeSeriesBuilder builder = new LocalDateDoubleTimeSeriesBuilder(stream());
	//	other.stream().forEach(pt -> builder.merge(pt, mapper));
	//	return builder.build();
	//  }

	  /// <summary>
	  /// Partition the time-series into a pair of distinct series using a predicate.
	  /// <para>
	  /// Points in the time-series which match the predicate will be put into the first series,
	  /// whilst those points which do not match will be put into the second.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="predicate">  predicate used to test the points in the time-series </param>
	  /// <returns> a {@code Pair} containing two time-series. The first is a series
	  ///   made of all the points in this series which match the predicate. The
	  ///   second is a series made of the points which do not match. </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.collect.tuple.Pair<LocalDateDoubleTimeSeries, LocalDateDoubleTimeSeries> partition(com.opengamma.strata.collect.function.ObjDoublePredicate<java.time.LocalDate> predicate)
	//  {
	//
	//	Map<bool, LocalDateDoubleTimeSeries> partitioned = stream().collect(partitioningBy(pt -> predicate.test(pt.getDate(), pt.getValue()), LocalDateDoubleTimeSeries.collector()));
	//
	//	return Pair.of(partitioned.get(true), partitioned.get(false));
	//  }

	  /// <summary>
	  /// Partition the time-series into a pair of distinct series using a predicate.
	  /// <para>
	  /// Points in the time-series whose values match the predicate will be put into the first series,
	  /// whilst those points whose values do not match will be put into the second.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="predicate">  predicate used to test the points in the time-series </param>
	  /// <returns> a {@code Pair} containing two time-series. The first is a series
	  ///   made of all the points in this series which match the predicate. The
	  ///   second is a series made of the points which do not match. </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.collect.tuple.Pair<LocalDateDoubleTimeSeries, LocalDateDoubleTimeSeries> partitionByValue(System.Func<double, boolean> predicate)
	//  {
	//	return partition((obj, value) -> predicate.test(value));
	//  }

	  /// <summary>
	  /// Return a builder populated with the values from this series.
	  /// <para>
	  /// This can be used to mutate the time-series.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a builder containing the point from this time-series </returns>
	  LocalDateDoubleTimeSeriesBuilder toBuilder();

	}

}