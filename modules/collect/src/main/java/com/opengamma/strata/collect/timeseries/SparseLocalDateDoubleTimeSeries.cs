using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.timeseries
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using Doubles = com.google.common.primitives.Doubles;
	using ObjDoublePredicate = com.opengamma.strata.collect.function.ObjDoublePredicate;

	/// <summary>
	/// A immutable implementation of {@code LocalDateDoubleTimeSeries} where the
	/// data stored is expected to be relatively sparse.
	/// <para>
	/// A sparse time-series has a relatively low density of dates with values.
	/// For example, a few points spread throughout a year.
	/// If more or less continuous data is being used then <seealso cref="DenseLocalDateDoubleTimeSeries"/>
	/// is likely to be a better choice for the data.
	/// </para>
	/// <para>
	/// This implementation uses arrays internally.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private", metaScope = "package") final class SparseLocalDateDoubleTimeSeries implements org.joda.beans.ImmutableBean, java.io.Serializable, LocalDateDoubleTimeSeries
	[Serializable]
	internal sealed class SparseLocalDateDoubleTimeSeries : ImmutableBean, LocalDateDoubleTimeSeries
	{

	  /// <summary>
	  /// An empty time-series.
	  /// </summary>
	  internal static readonly LocalDateDoubleTimeSeries EMPTY = new SparseLocalDateDoubleTimeSeries(new LocalDate[0], new double[0]);

	  /// <summary>
	  /// The dates in the series.
	  /// The dates are ordered from earliest to latest.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "manual", validate = "notNull") private final java.time.LocalDate[] dates;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly LocalDate[] dates_Renamed;
	  /// <summary>
	  /// The values in the series.
	  /// The date for each value is at the matching array index.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "manual", validate = "notNull") private final double[] values;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly double[] values_Renamed;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a time-series from matching arrays of dates and values.
	  /// <para>
	  /// The two arrays must be the same size and must be sorted from earliest to latest.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="dates">  the date list </param>
	  /// <param name="values">  the value list </param>
	  /// <returns> the time-series </returns>
	  internal static SparseLocalDateDoubleTimeSeries of(ICollection<LocalDate> dates, ICollection<double> values)
	  {
		ArgChecker.noNulls(dates, "dates");
		ArgChecker.noNulls(values, "values");
		LocalDate[] datesArray = dates.toArray(new LocalDate[dates.Count]);
		double[] valuesArray = Doubles.toArray(values);
		validate(datesArray, valuesArray);
		return createUnsafe(datesArray, valuesArray);
	  }

	  // creates time-series by directly assigning the input arrays
	  // must only be called when safe to do so
	  private static SparseLocalDateDoubleTimeSeries createUnsafe(LocalDate[] dates, double[] values)
	  {
		return new SparseLocalDateDoubleTimeSeries(dates, values, true);
	  }

	  // validates the arrays are same length and in order
	  private static void validate(LocalDate[] dates, double[] values)
	  {
		ArgChecker.isTrue(dates.Length == values.Length, "Arrays are of different sizes - dates: {}, values: {}", dates.Length, values.Length);
		LocalDate maxDate = LocalDate.MIN;
		foreach (LocalDate date in dates)
		{
		  ArgChecker.isTrue(date.isAfter(maxDate), "Dates must be in ascending order but: {} is not after: {}", date, maxDate);
		  maxDate = date;
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance, validating the supplied arrays.
	  /// <para>
	  /// The arrays are cloned as this constructor is called from Joda-Beans.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="dates">  the dates </param>
	  /// <param name="values">  the values </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private SparseLocalDateDoubleTimeSeries(java.time.LocalDate[] dates, double[] values)
	  private SparseLocalDateDoubleTimeSeries(LocalDate[] dates, double[] values)
	  {
		ArgChecker.noNulls(dates, "dates");
		ArgChecker.notNull(values, "values");
		validate(dates, values);
		this.dates_Renamed = dates.Clone();
		this.values_Renamed = values.Clone();
	  }

	  /// <summary>
	  /// Creates an instance without validating the supplied arrays.
	  /// </summary>
	  /// <param name="dates">  the dates </param>
	  /// <param name="values">  the values </param>
	  /// <param name="trusted">  flag to distinguish constructor </param>
	  private SparseLocalDateDoubleTimeSeries(LocalDate[] dates, double[] values, bool trusted)
	  {
		// constructor exists to avoid clones where possible
		// because Joda-Beans owns the main constructor, this one has a weird flag
		// use createUnsafe() instead of calling this directly
		this.dates_Renamed = dates;
		this.values_Renamed = values;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the dates in the series.
	  /// The dates are ordered from earliest to latest. </summary>
	  /// <returns> the value of the property, not null </returns>
	  private LocalDate[] Dates
	  {
		  get
		  {
			return dates_Renamed.Clone();
		  }
	  }

	  /// <summary>
	  /// Gets the values in the series.
	  /// The date for each value is at the matching array index. </summary>
	  /// <returns> the value of the property, not null </returns>
	  private double[] Values
	  {
		  get
		  {
			return values_Renamed.Clone();
		  }
	  }

	  //-------------------------------------------------------------------------
	  public int size()
	  {
		return dates_Renamed.Length;
	  }

	  public bool Empty
	  {
		  get
		  {
			return dates_Renamed.Length == 0;
		  }
	  }

	  public bool containsDate(LocalDate date)
	  {
		return (findDatePosition(date) >= 0);
	  }

	  public double? get(LocalDate date)
	  {
		int position = findDatePosition(date);
		return (position >= 0 ? double?.of(values_Renamed[position]) : double?.empty());
	  }

	  private int findDatePosition(LocalDate date)
	  {
		return Arrays.binarySearch(dates_Renamed, date);
	  }

	  //-------------------------------------------------------------------------
	  public LocalDate LatestDate
	  {
		  get
		  {
			if (Empty)
			{
			  throw new NoSuchElementException("Unable to return latest, time-series is empty");
			}
			return dates_Renamed[dates_Renamed.Length - 1];
		  }
	  }

	  public double LatestValue
	  {
		  get
		  {
			if (Empty)
			{
			  throw new NoSuchElementException("Unable to return latest, time-series is empty");
			}
			return values_Renamed[values_Renamed.Length - 1];
		  }
	  }

	  //-------------------------------------------------------------------------
	  public LocalDateDoubleTimeSeries subSeries(LocalDate startInclusive, LocalDate endExclusive)
	  {
		ArgChecker.notNull(startInclusive, "startInclusive");
		ArgChecker.notNull(endExclusive, "endExclusive");
		if (endExclusive.isBefore(startInclusive))
		{
		  throw new System.ArgumentException("Invalid sub series, end before start: " + startInclusive + " to " + endExclusive);
		}
		// special case when this is empty or when the dates are the same
		if (Empty || startInclusive.Equals(endExclusive))
		{
		  return EMPTY;
		}
		// where in the array would start/end be (whether or not it's actually in the series)
		int startPos = Arrays.binarySearch(dates_Renamed, startInclusive);
		startPos = startPos >= 0 ? startPos : -startPos - 1;
		int endPos = Arrays.binarySearch(dates_Renamed, endExclusive);
		endPos = endPos >= 0 ? endPos : -endPos - 1;
		// create sub-series
		LocalDate[] timesArray = Arrays.copyOfRange(dates_Renamed, startPos, endPos);
		double[] valuesArray = Arrays.copyOfRange(values_Renamed, startPos, endPos);
		return createUnsafe(timesArray, valuesArray);
	  }

	  public LocalDateDoubleTimeSeries headSeries(int numPoints)
	  {
		ArgChecker.notNegative(numPoints, "numPoints");
		if (numPoints == 0)
		{
		  return EMPTY;
		}
		else if (numPoints >= size())
		{
		  return this;
		}
		LocalDate[] datesArray = Arrays.copyOfRange(dates_Renamed, 0, numPoints);
		double[] valuesArray = Arrays.copyOfRange(values_Renamed, 0, numPoints);
		return createUnsafe(datesArray, valuesArray);
	  }

	  public LocalDateDoubleTimeSeries tailSeries(int numPoints)
	  {
		ArgChecker.notNegative(numPoints, "numPoints");
		if (numPoints == 0)
		{
		  return EMPTY;
		}
		else if (numPoints >= size())
		{
		  return this;
		}
		LocalDate[] datesArray = Arrays.copyOfRange(dates_Renamed, size() - numPoints, size());
		double[] valuesArray = Arrays.copyOfRange(values_Renamed, size() - numPoints, size());
		return createUnsafe(datesArray, valuesArray);
	  }

	  //-------------------------------------------------------------------------
	  public Stream<LocalDateDoublePoint> stream()
	  {
		return IntStream.range(0, size()).mapToObj(i => LocalDateDoublePoint.of(dates_Renamed[i], values_Renamed[i]));
	  }

	  public Stream<LocalDate> dates()
	  {
		return Stream.of(dates_Renamed);
	  }

	  public DoubleStream values()
	  {
		return DoubleStream.of(values_Renamed);
	  }

	  //-------------------------------------------------------------------------
	  public void forEach(System.Action<LocalDate, double> action)
	  {
		ArgChecker.notNull(action, "action");
		for (int i = 0; i < size(); i++)
		{
		  action(dates_Renamed[i], values_Renamed[i]);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public LocalDateDoubleTimeSeries mapDates(System.Func<? super java.time.LocalDate, ? extends java.time.LocalDate> mapper)
	  public LocalDateDoubleTimeSeries mapDates<T1>(System.Func<T1> mapper) where T1 : java.time.LocalDate
	  {
		ArgChecker.notNull(mapper, "mapper");
		LocalDate[] dates = java.util.this.dates_Renamed.Select(mapper).ToArray(size => new LocalDate[size]);
		// Check the dates are still in ascending order after the mapping
		java.util.dates.Aggregate(this.checkAscending);
		return createUnsafe(dates, values_Renamed);
	  }

	  public LocalDateDoubleTimeSeries mapValues(System.Func<double, double> mapper)
	  {
		ArgChecker.notNull(mapper, "mapper");
		return createUnsafe(dates_Renamed, DoubleStream.of(values_Renamed).map(mapper).toArray());
	  }

	  public LocalDateDoubleTimeSeries filter(ObjDoublePredicate<LocalDate> predicate)
	  {
		ArgChecker.notNull(predicate, "predicate");
		// build up result in arrays keeping track of count of retained dates
		LocalDate[] resDates = new LocalDate[size()];
		double[] resValues = new double[size()];
		int resCount = 0;
		for (int i = 0; i < size(); i++)
		{
		  if (predicate.test(dates_Renamed[i], values_Renamed[i]))
		  {
			resDates[resCount] = dates_Renamed[i];
			resValues[resCount] = values_Renamed[i];
			resCount++;
		  }
		}
		return createUnsafe(Arrays.copyOf(resDates, resCount), Arrays.copyOf(resValues, resCount));
	  }

	  //-------------------------------------------------------------------------
	  public LocalDateDoubleTimeSeriesBuilder toBuilder()
	  {
		return new LocalDateDoubleTimeSeriesBuilder(dates_Renamed, values_Renamed);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this time-series is equal to another time-series.
	  /// <para>
	  /// Compares this {@code LocalDateDoubleTimeSeries} with another ensuring
	  /// that the dates and values are the same.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="obj">  the object to check, null returns false </param>
	  /// <returns> true if this is equal to the other date </returns>
	  public override bool Equals(object obj)
	  {
		if (this == obj)
		{
		  return true;
		}
		if (obj is SparseLocalDateDoubleTimeSeries)
		{
		  SparseLocalDateDoubleTimeSeries other = (SparseLocalDateDoubleTimeSeries) obj;
		  return Arrays.Equals(dates_Renamed, other.dates_Renamed) && Arrays.Equals(values_Renamed, other.values_Renamed);
		}
		return false;
	  }

	  /// <summary>
	  /// A hash code for this time-series.
	  /// </summary>
	  /// <returns> a suitable hash code </returns>
	  public override int GetHashCode()
	  {
		return 31 * Arrays.GetHashCode(dates_Renamed) + Arrays.GetHashCode(values_Renamed);
	  }

	  /// <summary>
	  /// Returns a string representation of the time-series.
	  /// </summary>
	  /// <returns> the string </returns>
	  public override string ToString()
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		return stream().map(LocalDateDoublePoint::toString).collect(Collectors.joining(", ", "[", "]"));
	  }

	  //--------------------------------------------------------------------------------------------------

	  /// <summary>
	  /// Checks the dates are in ascending order, throws an exception if not.
	  /// </summary>
	  /// <param name="earlier">  the date that should be earlier </param>
	  /// <param name="later">  the date that should be later </param>
	  /// <returns> the later date if it is after the earlier date, otherwise throw an exception </returns>
	  /// <exception cref="IllegalArgumentException"> if the dates are not in ascending order </exception>
	  private LocalDate checkAscending(LocalDate earlier, LocalDate later)
	  {
		if (earlier.isBefore(later))
		{
		  return later;
		}
		throw new System.ArgumentException(Messages.format("Dates must be in ascending order after calling mapDates but {} and {} are not", earlier, later));
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code SparseLocalDateDoubleTimeSeries}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static SparseLocalDateDoubleTimeSeries.Meta meta()
	  {
		return SparseLocalDateDoubleTimeSeries.Meta.INSTANCE;
	  }

	  static SparseLocalDateDoubleTimeSeries()
	  {
		MetaBean.register(SparseLocalDateDoubleTimeSeries.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override SparseLocalDateDoubleTimeSeries.Meta metaBean()
	  {
		return SparseLocalDateDoubleTimeSeries.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code SparseLocalDateDoubleTimeSeries}.
	  /// </summary>
	  internal sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  dates_Renamed = DirectMetaProperty.ofImmutable(this, "dates", typeof(SparseLocalDateDoubleTimeSeries), typeof(LocalDate[]));
			  values_Renamed = DirectMetaProperty.ofImmutable(this, "values", typeof(SparseLocalDateDoubleTimeSeries), typeof(double[]));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "dates", "values");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code dates} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate[]> dates_Renamed;
		/// <summary>
		/// The meta-property for the {@code values} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double[]> values_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "dates", "values");
		internal IDictionary<string, MetaProperty<object>> metaPropertyMap$;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Meta()
		{
			if (!InstanceFieldsInitialized)
			{
				InitializeInstanceFields();
				InstanceFieldsInitialized = true;
			}
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override protected org.joda.beans.MetaProperty<?> metaPropertyGet(String propertyName)
		protected internal override MetaProperty<object> metaPropertyGet(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 95356549: // dates
			  return dates_Renamed;
			case -823812830: // values
			  return values_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends SparseLocalDateDoubleTimeSeries> builder()
		public override BeanBuilder<SparseLocalDateDoubleTimeSeries> builder()
		{
		  return new SparseLocalDateDoubleTimeSeries.Builder();
		}

		public override Type beanType()
		{
		  return typeof(SparseLocalDateDoubleTimeSeries);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code dates} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate[]> dates()
		{
		  return dates_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code values} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double[]> values()
		{
		  return values_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 95356549: // dates
			  return ((SparseLocalDateDoubleTimeSeries) bean).Dates;
			case -823812830: // values
			  return ((SparseLocalDateDoubleTimeSeries) bean).Values;
		  }
		  return base.propertyGet(bean, propertyName, quiet);
		}

		protected internal override void propertySet(Bean bean, string propertyName, object newValue, bool quiet)
		{
		  metaProperty(propertyName);
		  if (quiet)
		  {
			return;
		  }
		  throw new System.NotSupportedException("Property cannot be written: " + propertyName);
		}

	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The bean-builder for {@code SparseLocalDateDoubleTimeSeries}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<SparseLocalDateDoubleTimeSeries>
	  {

		internal LocalDate[] dates;
		internal double[] values;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 95356549: // dates
			  return dates;
			case -823812830: // values
			  return values;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 95356549: // dates
			  this.dates = (LocalDate[]) newValue;
			  break;
			case -823812830: // values
			  this.values = (double[]) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override SparseLocalDateDoubleTimeSeries build()
		{
		  return new SparseLocalDateDoubleTimeSeries(dates, values);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("SparseLocalDateDoubleTimeSeries.Builder{");
		  buf.Append("dates").Append('=').Append(JodaBeanUtils.ToString(dates)).Append(',').Append(' ');
		  buf.Append("values").Append('=').Append(JodaBeanUtils.ToString(values));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}