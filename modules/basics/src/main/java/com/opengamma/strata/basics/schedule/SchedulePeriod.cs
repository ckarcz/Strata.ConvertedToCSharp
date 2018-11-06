using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.schedule
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ComparisonChain = com.google.common.collect.ComparisonChain;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DateAdjuster = com.opengamma.strata.basics.date.DateAdjuster;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// A period in a schedule.
	/// <para>
	/// This consists of a single period (date range) within a schedule.
	/// This is typically used as the basis for financial calculations, such as accrual of interest.
	/// </para>
	/// <para>
	/// Two pairs of dates are provided, start/end and unadjustedStart/unadjustedEnd.
	/// The period itself runs from {@code startDate} to {@code endDate}.
	/// The {@code unadjustedStartDate} and {@code unadjustedEndDate} are the dates used to
	/// calculate the {@code startDate} and {@code endDate} when applying business day adjustment.
	/// </para>
	/// <para>
	/// For example, consider a schedule that has periods every three months on the 10th of the month.
	/// From time to time, the scheduled date will be a weekend or holiday.
	/// In this case, a rule may apply moving the date to a valid business day.
	/// If this happens, then the "unadjusted" date is the original date in the periodic schedule
	/// and the "adjusted" date is the related valid business day.
	/// Note that not all schedules apply a business day adjustment.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class SchedulePeriod implements org.joda.beans.ImmutableBean, Comparable<SchedulePeriod>, java.io.Serializable
	[Serializable]
	public sealed class SchedulePeriod : ImmutableBean, IComparable<SchedulePeriod>
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate startDate;
		private readonly LocalDate startDate;
	  /// <summary>
	  /// The end date of this period, used for financial calculations such as interest accrual.
	  /// <para>
	  /// The last date in the schedule period, typically treated as exclusive.
	  /// If the schedule adjusts for business days, then this is the adjusted date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate endDate;
	  private readonly LocalDate endDate;
	  /// <summary>
	  /// The unadjusted start date.
	  /// <para>
	  /// The start date before any business day adjustment.
	  /// If the schedule adjusts for business days, then this is typically the regular periodic date.
	  /// If the schedule does not adjust for business days, then this is the same as the start date.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the start date if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate unadjustedStartDate;
	  private readonly LocalDate unadjustedStartDate;
	  /// <summary>
	  /// The unadjusted end date.
	  /// <para>
	  /// The end date before any business day adjustment.
	  /// If the schedule adjusts for business days, then this is typically the regular periodic date.
	  /// If the schedule does not adjust for business days, then this is the same as the end date.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the end date if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate unadjustedEndDate;
	  private readonly LocalDate unadjustedEndDate;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the adjusted and unadjusted dates.
	  /// </summary>
	  /// <param name="startDate">  the start date, used for financial calculations such as interest accrual </param>
	  /// <param name="endDate">  the end date, used for financial calculations such as interest accrual </param>
	  /// <param name="unadjustedStartDate">  the unadjusted start date </param>
	  /// <param name="unadjustedEndDate">  the adjusted end date </param>
	  /// <returns> the period </returns>
	  public static SchedulePeriod of(LocalDate startDate, LocalDate endDate, LocalDate unadjustedStartDate, LocalDate unadjustedEndDate)
	  {
		return new SchedulePeriod(startDate, endDate, unadjustedStartDate, unadjustedEndDate);
	  }

	  /// <summary>
	  /// Obtains an instance from two dates.
	  /// <para>
	  /// This factory is used when there is no business day adjustment of schedule dates.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="startDate">  the start date, used for financial calculations such as interest accrual </param>
	  /// <param name="endDate">  the end date, used for financial calculations such as interest accrual </param>
	  /// <returns> the period </returns>
	  public static SchedulePeriod of(LocalDate startDate, LocalDate endDate)
	  {
		return new SchedulePeriod(startDate, endDate, startDate, endDate);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.inOrderNotEqual(unadjustedStartDate, unadjustedEndDate, "unadjustedStartDate", "unadjustedEndDate");
		ArgChecker.inOrderNotEqual(startDate, endDate, "startDate", "endDate");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (builder.unadjustedStartDate_Renamed == null)
		{
		  builder.unadjustedStartDate_Renamed = builder.startDate_Renamed;
		}
		if (builder.unadjustedEndDate_Renamed == null)
		{
		  builder.unadjustedEndDate_Renamed = builder.endDate_Renamed;
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the length of the period.
	  /// <para>
	  /// This returns the length of the period, considering the adjusted start and end dates.
	  /// The calculation does not involve a day count or holiday calendar.
	  /// The period is calculated using <seealso cref="Period#between(LocalDate, LocalDate)"/> and as
	  /// such includes the start date and excludes the end date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the length of the period </returns>
	  public Period length()
	  {
		return Period.between(startDate, endDate);
	  }

	  /// <summary>
	  /// Calculates the number of days in the period.
	  /// <para>
	  /// This returns the actual number of days in the period, considering the adjusted start and end dates.
	  /// The calculation does not involve a day count or holiday calendar.
	  /// The length includes one date and excludes the other.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the actual number of days in the period </returns>
	  public int lengthInDays()
	  {
		return Math.toIntExact(endDate.toEpochDay() - startDate.toEpochDay());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the year fraction using the specified day count.
	  /// <para>
	  /// Additional information from the schedule is made available to the day count algorithm.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="dayCount">  the day count convention </param>
	  /// <param name="schedule">  the schedule that contains this period </param>
	  /// <returns> the year fraction, calculated via the day count </returns>
	  public double yearFraction(DayCount dayCount, Schedule schedule)
	  {
		ArgChecker.notNull(dayCount, "dayCount");
		ArgChecker.notNull(schedule, "schedule");
		return dayCount.yearFraction(startDate, endDate, schedule);
	  }

	  /// <summary>
	  /// Checks if this period is regular according to the specified frequency and roll convention.
	  /// <para>
	  /// A schedule period is normally created from a frequency and roll convention.
	  /// These can therefore be used to determine if the period is regular, which simply
	  /// means that the period end date can be generated from the start date and vice versa.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="frequency">  the frequency </param>
	  /// <param name="rollConvention">  the roll convention </param>
	  /// <returns> true if the period is regular </returns>
	  public bool isRegular(Frequency frequency, RollConvention rollConvention)
	  {
		ArgChecker.notNull(frequency, "frequency");
		ArgChecker.notNull(rollConvention, "rollConvention");
		return rollConvention.next(unadjustedStartDate, frequency).Equals(unadjustedEndDate) && rollConvention.previous(unadjustedEndDate, frequency).Equals(unadjustedStartDate);
	  }

	  /// <summary>
	  /// Checks if this period contains the specified date.
	  /// <para>
	  /// The adjusted start and end dates are used in the comparison.
	  /// The start date is included, the end date is excluded.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to check </param>
	  /// <returns> true if this period contains the date </returns>
	  public bool contains(LocalDate date)
	  {
		ArgChecker.notNull(date, "date");
		return !date.isBefore(startDate) && date.isBefore(endDate);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a sub-schedule within this period.
	  /// <para>
	  /// The sub-schedule will have the one or more periods.
	  /// The schedule is bounded by the unadjusted start and end date of this period.
	  /// The frequency and roll convention are used to build unadjusted schedule dates.
	  /// The stub convention is used to handle any remaining time when the new frequency
	  /// does not evenly divide into the period.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="frequency">  the frequency of the sub-schedule </param>
	  /// <param name="rollConvention">  the roll convention to use for rolling </param>
	  /// <param name="stubConvention">  the stub convention to use for any excess </param>
	  /// <param name="adjustment">  the business day adjustment to apply to the sub-schedule </param>
	  /// <returns> the sub-schedule </returns>
	  /// <exception cref="ScheduleException"> if the schedule cannot be created </exception>
	  public PeriodicSchedule subSchedule(Frequency frequency, RollConvention rollConvention, StubConvention stubConvention, BusinessDayAdjustment adjustment)
	  {

		return PeriodicSchedule.builder().startDate(unadjustedStartDate).endDate(unadjustedEndDate).frequency(frequency).businessDayAdjustment(adjustment).rollConvention(rollConvention).stubConvention(stubConvention).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts this period to one where the start and end dates are adjusted using the specified adjuster.
	  /// <para>
	  /// The start date of the result will be the start date of this period as altered by the specified adjuster.
	  /// The end date of the result will be the end date of this period as altered by the specified adjuster.
	  /// The unadjusted start date and unadjusted end date will be the same as in this period.
	  /// </para>
	  /// <para>
	  /// The adjuster will typically be obtained from <seealso cref="BusinessDayAdjustment#resolve(ReferenceData)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="adjuster">  the adjuster to use </param>
	  /// <returns> the adjusted schedule period </returns>
	  public SchedulePeriod toAdjusted(DateAdjuster adjuster)
	  {
		// implementation needs to return 'this' if unchanged to optimize downstream code
		LocalDate resultStart = adjuster.adjust(startDate);
		LocalDate resultEnd = adjuster.adjust(endDate);
		if (resultStart.Equals(startDate) && resultEnd.Equals(endDate))
		{
		  return this;
		}
		return of(resultStart, resultEnd, unadjustedStartDate, unadjustedEndDate);
	  }

	  /// <summary>
	  /// Converts this period to one where the start and end dates are set to the unadjusted dates.
	  /// <para>
	  /// The start date of the result will be the unadjusted start date of this period.
	  /// The end date of the result will be the unadjusted end date of this period.
	  /// The unadjusted start date and unadjusted end date will be the same as in this period.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the unadjusted schedule period </returns>
	  public SchedulePeriod toUnadjusted()
	  {
		if (unadjustedStartDate.Equals(startDate) && unadjustedEndDate.Equals(endDate))
		{
		  return this;
		}
		return of(unadjustedStartDate, unadjustedEndDate);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Compares this period to another by unadjusted start date, then unadjusted end date.
	  /// </summary>
	  /// <param name="other">  the other period </param>
	  /// <returns> the comparison value </returns>
	  public int CompareTo(SchedulePeriod other)
	  {
		return ComparisonChain.start().compare(unadjustedStartDate, other.unadjustedStartDate).compare(unadjustedEndDate, other.unadjustedEndDate).result();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code SchedulePeriod}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static SchedulePeriod.Meta meta()
	  {
		return SchedulePeriod.Meta.INSTANCE;
	  }

	  static SchedulePeriod()
	  {
		MetaBean.register(SchedulePeriod.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static SchedulePeriod.Builder builder()
	  {
		return new SchedulePeriod.Builder();
	  }

	  private SchedulePeriod(LocalDate startDate, LocalDate endDate, LocalDate unadjustedStartDate, LocalDate unadjustedEndDate)
	  {
		JodaBeanUtils.notNull(startDate, "startDate");
		JodaBeanUtils.notNull(endDate, "endDate");
		JodaBeanUtils.notNull(unadjustedStartDate, "unadjustedStartDate");
		JodaBeanUtils.notNull(unadjustedEndDate, "unadjustedEndDate");
		this.startDate = startDate;
		this.endDate = endDate;
		this.unadjustedStartDate = unadjustedStartDate;
		this.unadjustedEndDate = unadjustedEndDate;
		validate();
	  }

	  public override SchedulePeriod.Meta metaBean()
	  {
		return SchedulePeriod.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the start date of this period, used for financial calculations such as interest accrual.
	  /// <para>
	  /// The first date in the schedule period, typically treated as inclusive.
	  /// If the schedule adjusts for business days, then this is the adjusted date.
	  /// </para>
	  /// </summary>
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
	  /// Gets the end date of this period, used for financial calculations such as interest accrual.
	  /// <para>
	  /// The last date in the schedule period, typically treated as exclusive.
	  /// If the schedule adjusts for business days, then this is the adjusted date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate EndDate
	  {
		  get
		  {
			return endDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the unadjusted start date.
	  /// <para>
	  /// The start date before any business day adjustment.
	  /// If the schedule adjusts for business days, then this is typically the regular periodic date.
	  /// If the schedule does not adjust for business days, then this is the same as the start date.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the start date if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate UnadjustedStartDate
	  {
		  get
		  {
			return unadjustedStartDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the unadjusted end date.
	  /// <para>
	  /// The end date before any business day adjustment.
	  /// If the schedule adjusts for business days, then this is typically the regular periodic date.
	  /// If the schedule does not adjust for business days, then this is the same as the end date.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the end date if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate UnadjustedEndDate
	  {
		  get
		  {
			return unadjustedEndDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Returns a builder that allows this bean to be mutated. </summary>
	  /// <returns> the mutable builder, not null </returns>
	  public Builder toBuilder()
	  {
		return new Builder(this);
	  }

	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  SchedulePeriod other = (SchedulePeriod) obj;
		  return JodaBeanUtils.equal(startDate, other.startDate) && JodaBeanUtils.equal(endDate, other.endDate) && JodaBeanUtils.equal(unadjustedStartDate, other.unadjustedStartDate) && JodaBeanUtils.equal(unadjustedEndDate, other.unadjustedEndDate);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(startDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(unadjustedStartDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(unadjustedEndDate);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("SchedulePeriod{");
		buf.Append("startDate").Append('=').Append(startDate).Append(',').Append(' ');
		buf.Append("endDate").Append('=').Append(endDate).Append(',').Append(' ');
		buf.Append("unadjustedStartDate").Append('=').Append(unadjustedStartDate).Append(',').Append(' ');
		buf.Append("unadjustedEndDate").Append('=').Append(JodaBeanUtils.ToString(unadjustedEndDate));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code SchedulePeriod}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  startDate_Renamed = DirectMetaProperty.ofImmutable(this, "startDate", typeof(SchedulePeriod), typeof(LocalDate));
			  endDate_Renamed = DirectMetaProperty.ofImmutable(this, "endDate", typeof(SchedulePeriod), typeof(LocalDate));
			  unadjustedStartDate_Renamed = DirectMetaProperty.ofImmutable(this, "unadjustedStartDate", typeof(SchedulePeriod), typeof(LocalDate));
			  unadjustedEndDate_Renamed = DirectMetaProperty.ofImmutable(this, "unadjustedEndDate", typeof(SchedulePeriod), typeof(LocalDate));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "startDate", "endDate", "unadjustedStartDate", "unadjustedEndDate");
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
		/// The meta-property for the {@code endDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> endDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code unadjustedStartDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> unadjustedStartDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code unadjustedEndDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> unadjustedEndDate_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "startDate", "endDate", "unadjustedStartDate", "unadjustedEndDate");
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
			case -1607727319: // endDate
			  return endDate_Renamed;
			case 1457691881: // unadjustedStartDate
			  return unadjustedStartDate_Renamed;
			case 31758114: // unadjustedEndDate
			  return unadjustedEndDate_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override SchedulePeriod.Builder builder()
		{
		  return new SchedulePeriod.Builder();
		}

		public override Type beanType()
		{
		  return typeof(SchedulePeriod);
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
		/// The meta-property for the {@code endDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> endDate()
		{
		  return endDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code unadjustedStartDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> unadjustedStartDate()
		{
		  return unadjustedStartDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code unadjustedEndDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> unadjustedEndDate()
		{
		  return unadjustedEndDate_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -2129778896: // startDate
			  return ((SchedulePeriod) bean).StartDate;
			case -1607727319: // endDate
			  return ((SchedulePeriod) bean).EndDate;
			case 1457691881: // unadjustedStartDate
			  return ((SchedulePeriod) bean).UnadjustedStartDate;
			case 31758114: // unadjustedEndDate
			  return ((SchedulePeriod) bean).UnadjustedEndDate;
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
	  /// The bean-builder for {@code SchedulePeriod}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<SchedulePeriod>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate startDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate endDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate unadjustedStartDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate unadjustedEndDate_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(SchedulePeriod beanToCopy)
		{
		  this.startDate_Renamed = beanToCopy.StartDate;
		  this.endDate_Renamed = beanToCopy.EndDate;
		  this.unadjustedStartDate_Renamed = beanToCopy.UnadjustedStartDate;
		  this.unadjustedEndDate_Renamed = beanToCopy.UnadjustedEndDate;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case 1457691881: // unadjustedStartDate
			  return unadjustedStartDate_Renamed;
			case 31758114: // unadjustedEndDate
			  return unadjustedEndDate_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -2129778896: // startDate
			  this.startDate_Renamed = (LocalDate) newValue;
			  break;
			case -1607727319: // endDate
			  this.endDate_Renamed = (LocalDate) newValue;
			  break;
			case 1457691881: // unadjustedStartDate
			  this.unadjustedStartDate_Renamed = (LocalDate) newValue;
			  break;
			case 31758114: // unadjustedEndDate
			  this.unadjustedEndDate_Renamed = (LocalDate) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override Builder set<T1>(MetaProperty<T1> property, object value)
		{
		  base.set(property, value);
		  return this;
		}

		public override SchedulePeriod build()
		{
		  preBuild(this);
		  return new SchedulePeriod(startDate_Renamed, endDate_Renamed, unadjustedStartDate_Renamed, unadjustedEndDate_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the start date of this period, used for financial calculations such as interest accrual.
		/// <para>
		/// The first date in the schedule period, typically treated as inclusive.
		/// If the schedule adjusts for business days, then this is the adjusted date.
		/// </para>
		/// </summary>
		/// <param name="startDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder startDate(LocalDate startDate)
		{
		  JodaBeanUtils.notNull(startDate, "startDate");
		  this.startDate_Renamed = startDate;
		  return this;
		}

		/// <summary>
		/// Sets the end date of this period, used for financial calculations such as interest accrual.
		/// <para>
		/// The last date in the schedule period, typically treated as exclusive.
		/// If the schedule adjusts for business days, then this is the adjusted date.
		/// </para>
		/// </summary>
		/// <param name="endDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder endDate(LocalDate endDate)
		{
		  JodaBeanUtils.notNull(endDate, "endDate");
		  this.endDate_Renamed = endDate;
		  return this;
		}

		/// <summary>
		/// Sets the unadjusted start date.
		/// <para>
		/// The start date before any business day adjustment.
		/// If the schedule adjusts for business days, then this is typically the regular periodic date.
		/// If the schedule does not adjust for business days, then this is the same as the start date.
		/// </para>
		/// <para>
		/// When building, this will default to the start date if not specified.
		/// </para>
		/// </summary>
		/// <param name="unadjustedStartDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder unadjustedStartDate(LocalDate unadjustedStartDate)
		{
		  JodaBeanUtils.notNull(unadjustedStartDate, "unadjustedStartDate");
		  this.unadjustedStartDate_Renamed = unadjustedStartDate;
		  return this;
		}

		/// <summary>
		/// Sets the unadjusted end date.
		/// <para>
		/// The end date before any business day adjustment.
		/// If the schedule adjusts for business days, then this is typically the regular periodic date.
		/// If the schedule does not adjust for business days, then this is the same as the end date.
		/// </para>
		/// <para>
		/// When building, this will default to the end date if not specified.
		/// </para>
		/// </summary>
		/// <param name="unadjustedEndDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder unadjustedEndDate(LocalDate unadjustedEndDate)
		{
		  JodaBeanUtils.notNull(unadjustedEndDate, "unadjustedEndDate");
		  this.unadjustedEndDate_Renamed = unadjustedEndDate;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("SchedulePeriod.Builder{");
		  buf.Append("startDate").Append('=').Append(JodaBeanUtils.ToString(startDate_Renamed)).Append(',').Append(' ');
		  buf.Append("endDate").Append('=').Append(JodaBeanUtils.ToString(endDate_Renamed)).Append(',').Append(' ');
		  buf.Append("unadjustedStartDate").Append('=').Append(JodaBeanUtils.ToString(unadjustedStartDate_Renamed)).Append(',').Append(' ');
		  buf.Append("unadjustedEndDate").Append('=').Append(JodaBeanUtils.ToString(unadjustedEndDate_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}