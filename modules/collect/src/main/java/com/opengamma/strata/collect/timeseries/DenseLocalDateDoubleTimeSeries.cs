using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.timeseries
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


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

	using Ordering = com.google.common.collect.Ordering;
	using Doubles = com.google.common.primitives.Doubles;
	using ObjDoublePredicate = com.opengamma.strata.collect.function.ObjDoublePredicate;

	/// <summary>
	/// An immutable implementation of {@code LocalDateDoubleTimeSeries} where the
	/// data stored is expected to be dense. For example, points for every
	/// working day in a month. If sparser data is being used then
	/// <seealso cref="SparseLocalDateDoubleTimeSeries"/> is likely to be a better
	/// choice for the data.
	/// <para>
	/// This implementation uses arrays internally.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private", metaScope = "package") final class DenseLocalDateDoubleTimeSeries implements org.joda.beans.ImmutableBean, LocalDateDoubleTimeSeries, java.io.Serializable
	[Serializable]
	internal sealed class DenseLocalDateDoubleTimeSeries : ImmutableBean, LocalDateDoubleTimeSeries
	{

	  /// <summary>
	  /// Enum indicating whether there are positions in the points
	  /// array for weekends and providing the different date
	  /// calculations for each case.
	  /// </summary>
	  internal abstract class DenseTimeSeriesCalculation
	  {
		/// <summary>
		/// Data is not held for weekends.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
		public static readonly DenseTimeSeriesCalculation SKIP_WEEKENDS = new DenseTimeSeriesCalculation()
		{
			int calculatePosition(java.time.LocalDate startDate, java.time.LocalDate date)
			{
				int unadjusted = (int) DAYS.between(startDate, date);
				int weekendAdjustment = startDate.getDayOfWeek().compareTo(date.getDayOfWeek()) > 0 ? 1 : 0;
				int numWeekends = (unadjusted / 7) + weekendAdjustment;
				return unadjusted - (2 * numWeekends);
			}
			java.time.LocalDate calculateDateFromPosition(java.time.LocalDate startDate, int position)
			{
				int numWeekends = position / 5;
				int remaining = position % 5;
				int endPointAdjustment = (remaining < (6 - startDate.get(DAY_OF_WEEK))) ? 0 : 2;
				return startDate.plusDays((7 * numWeekends) + remaining + endPointAdjustment);
			}
			boolean allowsDate(java.time.LocalDate date)
			{
				return !isWeekend(date);
			}
			public java.time.LocalDate adjustDate(java.time.LocalDate date)
			{
				return allowsDate(date) ? date : date.plusDays(8 - date.get(DAY_OF_WEEK));
			}
		},
		/// <summary>
		/// Data is held for weekends.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
		public static readonly DenseTimeSeriesCalculation INCLUDE_WEEKENDS = new DenseTimeSeriesCalculation()
		{
			int calculatePosition(java.time.LocalDate startDate, java.time.LocalDate date)
			{
				return (int) DAYS.between(startDate, date);
			}
			java.time.LocalDate calculateDateFromPosition(java.time.LocalDate startDate, int position)
			{
				return startDate.plusDays(position);
			}
			boolean allowsDate(java.time.LocalDate date)
			{
				return true;
			}
			public java.time.LocalDate adjustDate(java.time.LocalDate date)
			{
				return date;
			}
		};

		private static readonly IList<DenseTimeSeriesCalculation> valueList = new List<DenseTimeSeriesCalculation>();

		static DenseTimeSeriesCalculation()
		{
			valueList.Add(SKIP_WEEKENDS);
			valueList.Add(INCLUDE_WEEKENDS);
		}

		public enum InnerEnum
		{
			SKIP_WEEKENDS,
			INCLUDE_WEEKENDS
		}

		public readonly InnerEnum innerEnumValue;
		private readonly string nameValue;
		private readonly int ordinalValue;
		private static int nextOrdinal = 0;

		private DenseTimeSeriesCalculation(string name, InnerEnum innerEnum)
		{
			nameValue = name;
			ordinalValue = nextOrdinal++;
			innerEnumValue = innerEnum;
		}

		/// <summary>
		/// Calculates the position in the array where the supplied date should
		/// be located given a start date. As no information is held about the
		/// actual array, callers must check array bounds.
		/// </summary>
		/// <param name="startDate">  the start date for the series (the value for this
		///   entry will be stored at position 0 in the array) </param>
		/// <param name="date">  the date to calculate a position for </param>
		/// <returns> the position in the array where the date would be located </returns>
		internal abstract int calculatePosition(java.time.LocalDate startDate, java.time.LocalDate date);

		/// <summary>
		/// Given a start date and a position in an array, calculate what date
		/// the position holds data for.
		/// </summary>
		/// <param name="startDate">  the start date for the series (the value for this
		///   entry will be stored at position 0 in the array) </param>
		/// <param name="position">  the position in the array to calculate a date for </param>
		/// <returns> the date the position in the array holds data for </returns>
		internal abstract java.time.LocalDate calculateDateFromPosition(java.time.LocalDate startDate, int position);

		/// <summary>
		/// Indicates if the specified date would be a possible date
		/// for the calculation.
		/// </summary>
		/// <param name="date">  the date to check </param>
		/// <returns> true if the calculation would allow the date </returns>
		internal abstract bool allowsDate(java.time.LocalDate date);

		/// <summary>
		/// Adjusts the supplied data such that it is a valid
		/// date from the calculation's point of view.
		/// </summary>
		/// <param name="date">  the date to adjust </param>
		/// <returns> the adjusted date </returns>
		public abstract java.time.LocalDate adjustDate(java.time.LocalDate date);

		// Sufficient for the moment, in the future we may need to
		// vary depending on a non-Western weekend
		internal static bool isWeekend(java.time.LocalDate date)
		{
		  return date.get(DAY_OF_WEEK) > 5;
		}

		  public static IList<DenseTimeSeriesCalculation> values()
		  {
			  return valueList;
		  }

		  public int ordinal()
		  {
			  return ordinalValue;
		  }

		  public override string ToString()
		  {
			  return nameValue;
		  }

		  public static DenseTimeSeriesCalculation valueOf(string name)
		  {
			  foreach (DenseTimeSeriesCalculation enumInstance in DenseTimeSeriesCalculation.valueList)
			  {
				  if (enumInstance.nameValue == name)
				  {
					  return enumInstance;
				  }
			  }
			  throw new System.ArgumentException(name);
		  }
	  }

	  /// <summary>
	  /// Date corresponding to first element in the array. All other
	  /// values can be calculated using date arithmetic to find
	  /// correct point.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate startDate;
	  private readonly LocalDate startDate;

	  /// <summary>
	  /// The values in the series.
	  /// The date for each value is calculated using the position
	  /// in the array and the start date.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "private", validate = "notNull") private final double[] points;
	  private readonly double[] points;

	  /// <summary>
	  /// Whether we should store data for the weekends (NaN will be stored
	  /// if no data is available).
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "private", validate = "notNull") private final DenseTimeSeriesCalculation dateCalculation;
	  private readonly DenseTimeSeriesCalculation dateCalculation;

	  /// <summary>
	  /// Package protected factory method intended to be called
	  /// by the <seealso cref="LocalDateDoubleTimeSeriesBuilder"/>. As such
	  /// all the information passed is assumed to be consistent.
	  /// </summary>
	  /// <param name="startDate">  the earliest date included in the time-series </param>
	  /// <param name="endDate">  the latest date included in the time-series </param>
	  /// <param name="values">  stream holding the time-series points </param>
	  /// <param name="dateCalculation">  the date calculation method to be used </param>
	  /// <returns> a new time-series </returns>
	  internal static LocalDateDoubleTimeSeries of(LocalDate startDate, LocalDate endDate, Stream<LocalDateDoublePoint> values, DenseTimeSeriesCalculation dateCalculation)
	  {

		double[] points = new double[dateCalculation.calculatePosition(startDate, endDate) + 1];
		Arrays.fill(points, Double.NaN);
		values.forEach(pt => points[dateCalculation.calculatePosition(startDate, pt.Date)] = pt.Value);
		return new DenseLocalDateDoubleTimeSeries(startDate, points, dateCalculation, true);
	  }

	  // Private constructor, the trusted flag indicates whether the
	  // points array should be cloned. If trusted, it will not be cloned.
	  private DenseLocalDateDoubleTimeSeries(LocalDate startDate, double[] points, DenseTimeSeriesCalculation dateCalculation, bool trusted)
	  {

		ArgChecker.notNull(points, "points");
		this.startDate = ArgChecker.notNull(startDate, "startDate");
		this.points = trusted ? points : points.Clone();
		this.dateCalculation = ArgChecker.notNull(dateCalculation, "dateCalculation");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private DenseLocalDateDoubleTimeSeries(java.time.LocalDate startDate, double[] points, DenseTimeSeriesCalculation dateCalculation)
	  private DenseLocalDateDoubleTimeSeries(LocalDate startDate, double[] points, DenseTimeSeriesCalculation dateCalculation) : this(startDate, points, dateCalculation, false)
	  {
	  }

	  //-------------------------------------------------------------------------
	  public bool Empty
	  {
		  get
		  {
			return !validIndices().findFirst().Present;
		  }
	  }

	  public int size()
	  {
		return (int) validIndices().count();
	  }

	  public bool containsDate(LocalDate date)
	  {
		return get(date).HasValue;
	  }

	  public double? get(LocalDate date)
	  {
		if (!Empty && !date.isBefore(startDate) && dateCalculation.allowsDate(date))
		{
		  int position = dateCalculation.calculatePosition(startDate, date);
		  if (position < points.Length)
		  {
			double value = points[position];
			if (isValidPoint(value))
			{
			  return double?.of(value);
			}
		  }
		}
		return double?.empty();
	  }

	  //-------------------------------------------------------------------------
	  private IntStream reversedValidIndices()
	  {
		// As there is no way of constructing an IntStream from
		// n to m where n > m, we go from -n to m and then
		// take the additive inverse (sigh!)
		return IntStream.rangeClosed(1 - points.Length, 0).map(i => -i).filter(this.isValidIndex);
	  }

	  private LocalDate calculateDateFromPosition(int i)
	  {
		return dateCalculation.calculateDateFromPosition(startDate, i);
	  }

	  public LocalDate LatestDate
	  {
		  get
		  {
			return reversedValidIndices().mapToObj(this.calculateDateFromPosition).findFirst().orElseThrow(() => new NoSuchElementException("Unable to return latest date, time-series is empty"));
		  }
	  }

	  public double LatestValue
	  {
		  get
		  {
			return reversedValidIndices().mapToDouble(i => points[i]).findFirst().orElseThrow(() => new NoSuchElementException("Unable to return latest value, time-series is empty"));
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
		// or the series don't intersect
		if (Empty || startInclusive.Equals(endExclusive) || !startDate.isBefore(endExclusive) || startInclusive.isAfter(LatestDate))
		{
		  return LocalDateDoubleTimeSeries.empty();
		}

		LocalDate resolvedStart = dateCalculation.adjustDate(Ordering.natural().max(startInclusive, startDate));
		int startIndex = dateCalculation.calculatePosition(startDate, resolvedStart);
		int endIndex = dateCalculation.calculatePosition(startDate, endExclusive);
		return new DenseLocalDateDoubleTimeSeries(resolvedStart, Arrays.copyOfRange(points, Math.Max(0, startIndex), Math.Min(points.Length, endIndex)), dateCalculation, true);
	  }

	  public LocalDateDoubleTimeSeries headSeries(int numPoints)
	  {
		ArgChecker.notNegative(numPoints, "numPoints");

		if (numPoints == 0)
		{
		  return LocalDateDoubleTimeSeries.empty();
		}
		else if (numPoints > size())
		{
		  return this;
		}
		int endPosition = findHeadPoints(numPoints);
		return new DenseLocalDateDoubleTimeSeries(startDate, Arrays.copyOf(points, endPosition), dateCalculation);
	  }

	  private int findHeadPoints(int required)
	  {
		// Take enough points that aren't NaN
		// else we need the entire series
		return validIndices().skip(required).findFirst().orElse(points.Length);
	  }

	  public LocalDateDoubleTimeSeries tailSeries(int numPoints)
	  {
		ArgChecker.notNegative(numPoints, "numPoints");

		if (numPoints == 0)
		{
		  return LocalDateDoubleTimeSeries.empty();
		}
		else if (numPoints > size())
		{
		  return this;
		}

		int startPoint = findTailPoints(numPoints);

		return new DenseLocalDateDoubleTimeSeries(calculateDateFromPosition(startPoint), Arrays.copyOfRange(points, startPoint, points.Length), dateCalculation);
	  }

	  private int findTailPoints(int required)
	  {
		return reversedValidIndices().skip(required - 1).findFirst().orElse(0);
	  }

	  //-------------------------------------------------------------------------
	  public Stream<LocalDateDoublePoint> stream()
	  {
		return validIndices().mapToObj(i => LocalDateDoublePoint.of(calculateDateFromPosition(i), points[i]));
	  }

	  public DoubleStream values()
	  {
		return java.util.points.Where(this.isValidPoint);
	  }

	  public Stream<LocalDate> dates()
	  {
		return validIndices().mapToObj(this.calculateDateFromPosition);
	  }

	  private IntStream validIndices()
	  {
		return IntStream.range(0, points.Length).filter(this.isValidIndex);
	  }

	  private bool isValidIndex(int i)
	  {
		return isValidPoint(points[i]);
	  }

	  //-------------------------------------------------------------------------
	  public LocalDateDoubleTimeSeries filter(ObjDoublePredicate<LocalDate> predicate)
	  {
		Stream<LocalDateDoublePoint> filteredPoints = stream().filter(pt => predicate.test(pt.Date, pt.Value));

		// As we may have changed the density of the series by filtering
		// go via the builder to get the best implementation
		return (new LocalDateDoubleTimeSeriesBuilder(filteredPoints)).build();
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public LocalDateDoubleTimeSeries mapDates(System.Func<? super java.time.LocalDate, ? extends java.time.LocalDate> mapper)
	  public LocalDateDoubleTimeSeries mapDates<T1>(System.Func<T1> mapper) where T1 : java.time.LocalDate
	  {
		IList<LocalDate> dates = this.dates().map(mapper).collect(toImmutableList());
		dates.Aggregate(this.checkAscending);
		return LocalDateDoubleTimeSeries.builder().putAll(dates, Doubles.asList(points)).build();
	  }

	  public LocalDateDoubleTimeSeries mapValues(System.Func<double, double> mapper)
	  {
		DoubleStream values = DoubleStream.of(points).map(d => isValidPoint(d) ? applyMapper(mapper, d) : d);
		return new DenseLocalDateDoubleTimeSeries(startDate, values.toArray(), dateCalculation, true);
	  }

	  private double applyMapper(System.Func<double, double> mapper, double d)
	  {
		double value = mapper(d);
		if (!isValidPoint(value))
		{
		  throw new System.ArgumentException("Mapper must not map to NaN");
		}
		return value;
	  }

	  private bool isValidPoint(double d)
	  {
		return !double.IsNaN(d);
	  }

	  public void forEach(System.Action<LocalDate, double> action)
	  {
		validIndices().forEach(i => action(calculateDateFromPosition(i), points[i]));
	  }

	  public LocalDateDoubleTimeSeriesBuilder toBuilder()
	  {
		return new LocalDateDoubleTimeSeriesBuilder(stream());
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
	  /// The meta-bean for {@code DenseLocalDateDoubleTimeSeries}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static DenseLocalDateDoubleTimeSeries.Meta meta()
	  {
		return DenseLocalDateDoubleTimeSeries.Meta.INSTANCE;
	  }

	  static DenseLocalDateDoubleTimeSeries()
	  {
		MetaBean.register(DenseLocalDateDoubleTimeSeries.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override DenseLocalDateDoubleTimeSeries.Meta metaBean()
	  {
		return DenseLocalDateDoubleTimeSeries.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets date corresponding to first element in the array. All other
	  /// values can be calculated using date arithmetic to find
	  /// correct point. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate StartDate
	  {
		  get
		  {
			return startDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the values in the series.
	  /// The date for each value is calculated using the position
	  /// in the array and the start date. </summary>
	  /// <returns> the value of the property, not null </returns>
	  private double[] Points
	  {
		  get
		  {
			return points.Clone();
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets whether we should store data for the weekends (NaN will be stored
	  /// if no data is available). </summary>
	  /// <returns> the value of the property, not null </returns>
	  private DenseTimeSeriesCalculation DateCalculation
	  {
		  get
		  {
			return dateCalculation;
		  }
	  }

	  //-----------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  DenseLocalDateDoubleTimeSeries other = (DenseLocalDateDoubleTimeSeries) obj;
		  return JodaBeanUtils.equal(startDate, other.startDate) && JodaBeanUtils.equal(points, other.points) && JodaBeanUtils.equal(dateCalculation, other.dateCalculation);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(startDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(points);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dateCalculation);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("DenseLocalDateDoubleTimeSeries{");
		buf.Append("startDate").Append('=').Append(startDate).Append(',').Append(' ');
		buf.Append("points").Append('=').Append(points).Append(',').Append(' ');
		buf.Append("dateCalculation").Append('=').Append(JodaBeanUtils.ToString(dateCalculation));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code DenseLocalDateDoubleTimeSeries}.
	  /// </summary>
	  internal sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  startDate_Renamed = DirectMetaProperty.ofImmutable(this, "startDate", typeof(DenseLocalDateDoubleTimeSeries), typeof(LocalDate));
			  points_Renamed = DirectMetaProperty.ofImmutable(this, "points", typeof(DenseLocalDateDoubleTimeSeries), typeof(double[]));
			  dateCalculation_Renamed = DirectMetaProperty.ofImmutable(this, "dateCalculation", typeof(DenseLocalDateDoubleTimeSeries), typeof(DenseTimeSeriesCalculation));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "startDate", "points", "dateCalculation");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code startDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> startDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code points} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double[]> points_Renamed;
		/// <summary>
		/// The meta-property for the {@code dateCalculation} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DenseTimeSeriesCalculation> dateCalculation_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "startDate", "points", "dateCalculation");
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
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -982754077: // points
			  return points_Renamed;
			case -152592837: // dateCalculation
			  return dateCalculation_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends DenseLocalDateDoubleTimeSeries> builder()
		public override BeanBuilder<DenseLocalDateDoubleTimeSeries> builder()
		{
		  return new DenseLocalDateDoubleTimeSeries.Builder();
		}

		public override Type beanType()
		{
		  return typeof(DenseLocalDateDoubleTimeSeries);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code startDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> startDate()
		{
		  return startDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code points} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double[]> points()
		{
		  return points_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dateCalculation} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DenseTimeSeriesCalculation> dateCalculation()
		{
		  return dateCalculation_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -2129778896: // startDate
			  return ((DenseLocalDateDoubleTimeSeries) bean).StartDate;
			case -982754077: // points
			  return ((DenseLocalDateDoubleTimeSeries) bean).Points;
			case -152592837: // dateCalculation
			  return ((DenseLocalDateDoubleTimeSeries) bean).DateCalculation;
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
	  /// The bean-builder for {@code DenseLocalDateDoubleTimeSeries}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<DenseLocalDateDoubleTimeSeries>
	  {

		internal LocalDate startDate;
		internal double[] points;
		internal DenseTimeSeriesCalculation dateCalculation;

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
			case -2129778896: // startDate
			  return startDate;
			case -982754077: // points
			  return points;
			case -152592837: // dateCalculation
			  return dateCalculation;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -2129778896: // startDate
			  this.startDate = (LocalDate) newValue;
			  break;
			case -982754077: // points
			  this.points = (double[]) newValue;
			  break;
			case -152592837: // dateCalculation
			  this.dateCalculation = (DenseTimeSeriesCalculation) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override DenseLocalDateDoubleTimeSeries build()
		{
		  return new DenseLocalDateDoubleTimeSeries(startDate, points, dateCalculation);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("DenseLocalDateDoubleTimeSeries.Builder{");
		  buf.Append("startDate").Append('=').Append(JodaBeanUtils.ToString(startDate)).Append(',').Append(' ');
		  buf.Append("points").Append('=').Append(JodaBeanUtils.ToString(points)).Append(',').Append(' ');
		  buf.Append("dateCalculation").Append('=').Append(JodaBeanUtils.ToString(dateCalculation));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}