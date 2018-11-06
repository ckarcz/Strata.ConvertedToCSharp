using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;


	/// <summary>
	/// An adjustment that alters a date by adding a period of days.
	/// <para>
	/// When processing dates in finance, the rules for adjusting a date by a number of days can be complex.
	/// This class represents those rules, which operate in two steps - addition followed by adjustment.
	/// There are two main ways to perform the addition:
	/// 
	/// <h4>Approach 1 - calendar days addition</h4>
	/// This approach is triggered by using the {@code ofCalendarDays()} factory methods.
	/// When adding a number of days to a date the addition is simple, no holidays or weekends apply.
	/// For example, two days after Friday 15th August would be Sunday 17th, even though this is typically a weekend.
	/// There are two steps in the calculation:
	/// </para>
	/// <para>
	/// In step one, the number of days is added without skipping any dates.
	/// </para>
	/// <para>
	/// In step two, the result of step one is optionally adjusted to be a business day
	/// using a {@code BusinessDayAdjustment}.
	/// 
	/// <h4>Approach 2 - business days addition</h4>
	/// With this approach the days to be added are treated as business days.
	/// For example, two days after Friday 15th August would be Tuesday 19th, assuming a Saturday/Sunday
	/// weekend and no other applicable holidays.
	/// </para>
	/// <para>
	/// This approach is triggered by using the {@code ofBusinessDays()} factory methods.
	/// The distinction between business days, holidays and weekends is made using the specified holiday calendar.
	/// There are two steps in the calculation:
	/// </para>
	/// <para>
	/// In step one, the number of days is added using <seealso cref="HolidayCalendar#shift(LocalDate, int)"/>.
	/// </para>
	/// <para>
	/// In step two, the result of step one is optionally adjusted to be a business day
	/// using a {@code BusinessDayAdjustment}.
	/// </para>
	/// <para>
	/// At first glance, step two may seem pointless, as the result of step one will always be a valid business day.
	/// However, the step two adjustment allows the possibility of applying a different holiday calendar.
	/// </para>
	/// <para>
	/// For example, a rule might have two parts: "first add 2 London business days, and then adjust the
	/// result to be a valid New York business day using the 'ModifiedFollowing' convention".
	/// Note that the holiday calendar differs in the two parts of the rule.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class DaysAdjustment implements com.opengamma.strata.basics.Resolvable<DateAdjuster>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class DaysAdjustment : Resolvable<DateAdjuster>, ImmutableBean
	{

	  /// <summary>
	  /// An instance that performs no adjustment.
	  /// </summary>
	  public static readonly DaysAdjustment NONE = new DaysAdjustment(0, HolidayCalendarIds.NO_HOLIDAYS, BusinessDayAdjustment.NONE);

	  /// <summary>
	  /// The number of days to be added.
	  /// <para>
	  /// When the adjustment is performed, this amount will be added to the input date
	  /// using the calendar to determine the addition type.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final int days;
	  private readonly int days;
	  /// <summary>
	  /// The holiday calendar that defines the meaning of a day when performing the addition.
	  /// <para>
	  /// When the adjustment is performed, this calendar is used to determine which days are business days.
	  /// </para>
	  /// <para>
	  /// If the holiday calendar is 'None' then addition uses simple date addition arithmetic without
	  /// considering any days as holidays or weekends.
	  /// If the holiday calendar is anything other than 'None' then addition uses that calendar,
	  /// effectively repeatedly finding the next business day.
	  /// </para>
	  /// <para>
	  /// See the class-level documentation for more information.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final HolidayCalendarId calendar;
	  private readonly HolidayCalendarId calendar;
	  /// <summary>
	  /// The business day adjustment that is performed to the result of the addition.
	  /// <para>
	  /// This adjustment is applied to the result of the period addition calculation.
	  /// If the addition is performed using business days then any adjustment here is expected to
	  /// have a different holiday calendar to that used during addition.
	  /// </para>
	  /// <para>
	  /// If no adjustment is required, use the 'None' business day adjustment.
	  /// </para>
	  /// <para>
	  /// See the class-level documentation for more information.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final BusinessDayAdjustment adjustment;
	  private readonly BusinessDayAdjustment adjustment;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance that can adjust a date by a specific number of calendar days.
	  /// <para>
	  /// When adjusting a date, the specified number of calendar days is added.
	  /// Holidays and weekends are not taken into account in the calculation.
	  /// </para>
	  /// <para>
	  /// No business day adjustment is applied to the result of the addition.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="numberOfDays">  the number of days </param>
	  /// <returns> the days adjustment </returns>
	  public static DaysAdjustment ofCalendarDays(int numberOfDays)
	  {
		return new DaysAdjustment(numberOfDays, HolidayCalendarIds.NO_HOLIDAYS, BusinessDayAdjustment.NONE);
	  }

	  /// <summary>
	  /// Obtains an instance that can adjust a date by a specific number of calendar days.
	  /// <para>
	  /// When adjusting a date, the specified number of calendar days is added.
	  /// Holidays and weekends are not taken into account in the calculation.
	  /// </para>
	  /// <para>
	  /// The business day adjustment is applied to the result of the addition.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="numberOfDays">  the number of days </param>
	  /// <param name="adjustment">  the business day adjustment to apply to the result of the addition </param>
	  /// <returns> the days adjustment </returns>
	  public static DaysAdjustment ofCalendarDays(int numberOfDays, BusinessDayAdjustment adjustment)
	  {
		return new DaysAdjustment(numberOfDays, HolidayCalendarIds.NO_HOLIDAYS, adjustment);
	  }

	  /// <summary>
	  /// Obtains an instance that can adjust a date by a specific number of business days.
	  /// <para>
	  /// When adjusting a date, the specified number of business days is added.
	  /// This is equivalent to repeatedly finding the next business day.
	  /// </para>
	  /// <para>
	  /// No business day adjustment is applied to the result of the addition.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="numberOfDays">  the number of days </param>
	  /// <param name="holidayCalendar">  the calendar that defines holidays and business days </param>
	  /// <returns> the days adjustment </returns>
	  public static DaysAdjustment ofBusinessDays(int numberOfDays, HolidayCalendarId holidayCalendar)
	  {
		return new DaysAdjustment(numberOfDays, holidayCalendar, BusinessDayAdjustment.NONE);
	  }

	  /// <summary>
	  /// Obtains an instance that can adjust a date by a specific number of business days.
	  /// <para>
	  /// When adjusting a date, the specified number of business days is added.
	  /// This is equivalent to repeatedly finding the next business day.
	  /// </para>
	  /// <para>
	  /// The business day adjustment is applied to the result of the addition.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="numberOfDays">  the number of days </param>
	  /// <param name="holidayCalendar">  the calendar that defines holidays and business days </param>
	  /// <param name="adjustment">  the business day adjustment to apply to the result of the addition </param>
	  /// <returns> the days adjustment </returns>
	  public static DaysAdjustment ofBusinessDays(int numberOfDays, HolidayCalendarId holidayCalendar, BusinessDayAdjustment adjustment)
	  {
		return new DaysAdjustment(numberOfDays, holidayCalendar, adjustment);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adjusts the date, adding the period in days using the holiday calendar
	  /// and then applying the business day adjustment.
	  /// <para>
	  /// The calculation is performed in two steps.
	  /// </para>
	  /// <para>
	  /// Step one, use <seealso cref="HolidayCalendar#shift(LocalDate, int)"/> to add the number of days.
	  /// If the holiday calendar is 'None' this will effectively add calendar days.
	  /// </para>
	  /// <para>
	  /// Step two, use <seealso cref="BusinessDayAdjustment#adjust(LocalDate, ReferenceData)"/> to adjust the result of step one.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to adjust </param>
	  /// <param name="refData">  the reference data, used to find the holiday calendar </param>
	  /// <returns> the adjusted date </returns>
	  public LocalDate adjust(LocalDate date, ReferenceData refData)
	  {
		LocalDate added = calendar.resolve(refData).shift(date, days);
		return adjustment.adjust(added, refData);
	  }

	  /// <summary>
	  /// Resolves this adjustment using the specified reference data, returning an adjuster.
	  /// <para>
	  /// This returns a <seealso cref="DateAdjuster"/> that performs the same calculation as this adjustment.
	  /// It binds the holiday calendar, looked up from the reference data, into the result.
	  /// As such, there is no need to pass the reference data in again.
	  /// </para>
	  /// <para>
	  /// The resulting adjuster will be <seealso cref="#normalized() normalized"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="refData">  the reference data, used to find the holiday calendar </param>
	  /// <returns> the adjuster, bound to a specific holiday calendar </returns>
	  public DateAdjuster resolve(ReferenceData refData)
	  {
		HolidayCalendar holCalAdj = adjustment.Calendar.resolve(refData);
		if (calendar == HolidayCalendarIds.NO_HOLIDAYS)
		{
		  BusinessDayConvention adjustmentConvention = adjustment.Convention;
		  return date => adjustmentConvention.adjust(LocalDateUtils.plusDays(date, days), holCalAdj);
		}
		HolidayCalendar holCalAdd = calendar.resolve(refData);
		BusinessDayConvention adjustmentConvention = adjustment.Convention;
		return date => adjustmentConvention.adjust(holCalAdd.shift(date, days), holCalAdj);
	  }

	  /// <summary>
	  /// Gets the holiday calendar that will be applied to the result.
	  /// <para>
	  /// This adjustment may contain more than one holiday calendar.
	  /// This method returns the calendar used last.
	  /// As such, the adjusted date will always be valid according to this calendar.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the result holiday calendar </returns>
	  public HolidayCalendarId ResultCalendar
	  {
		  get
		  {
			HolidayCalendarId cal = adjustment.Calendar;
			if (cal == HolidayCalendarIds.NO_HOLIDAYS)
			{
			  cal = calendar;
			}
			return cal;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Normalizes the adjustment.
	  /// <para>
	  /// If the number of days is zero, the calendar is set no 'NoHolidays'.
	  /// If the number of days is non-zero and the calendar equals the adjustment calendar,
	  /// the adjustment is removed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the normalized adjustment </returns>
	  public DaysAdjustment normalized()
	  {
		if (days == 0)
		{
		  if (calendar != HolidayCalendarIds.NO_HOLIDAYS)
		  {
			return DaysAdjustment.ofCalendarDays(days, adjustment);
		  }
		  return this;
		}
		if (calendar.Equals(adjustment.Calendar))
		{
		  return DaysAdjustment.ofBusinessDays(days, calendar);
		}
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a string describing the adjustment.
	  /// </summary>
	  /// <returns> the descriptive string </returns>
	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(64);
		buf.Append(days);
		if (calendar == HolidayCalendarIds.NO_HOLIDAYS)
		{
		  buf.Append(" calendar day");
		  if (days != 1)
		  {
			buf.Append("s");
		  }
		}
		else
		{
		  buf.Append(" business day");
		  if (days != 1)
		  {
			buf.Append("s");
		  }
		  buf.Append(" using calendar ").Append(calendar.Name);
		}
		if (adjustment.Equals(BusinessDayAdjustment.NONE) == false)
		{
		  buf.Append(" then apply ").Append(adjustment);
		}
		return buf.ToString();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DaysAdjustment}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static DaysAdjustment.Meta meta()
	  {
		return DaysAdjustment.Meta.INSTANCE;
	  }

	  static DaysAdjustment()
	  {
		MetaBean.register(DaysAdjustment.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static DaysAdjustment.Builder builder()
	  {
		return new DaysAdjustment.Builder();
	  }

	  private DaysAdjustment(int days, HolidayCalendarId calendar, BusinessDayAdjustment adjustment)
	  {
		JodaBeanUtils.notNull(days, "days");
		JodaBeanUtils.notNull(calendar, "calendar");
		JodaBeanUtils.notNull(adjustment, "adjustment");
		this.days = days;
		this.calendar = calendar;
		this.adjustment = adjustment;
	  }

	  public override DaysAdjustment.Meta metaBean()
	  {
		return DaysAdjustment.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of days to be added.
	  /// <para>
	  /// When the adjustment is performed, this amount will be added to the input date
	  /// using the calendar to determine the addition type.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public int Days
	  {
		  get
		  {
			return days;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the holiday calendar that defines the meaning of a day when performing the addition.
	  /// <para>
	  /// When the adjustment is performed, this calendar is used to determine which days are business days.
	  /// </para>
	  /// <para>
	  /// If the holiday calendar is 'None' then addition uses simple date addition arithmetic without
	  /// considering any days as holidays or weekends.
	  /// If the holiday calendar is anything other than 'None' then addition uses that calendar,
	  /// effectively repeatedly finding the next business day.
	  /// </para>
	  /// <para>
	  /// See the class-level documentation for more information.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public HolidayCalendarId Calendar
	  {
		  get
		  {
			return calendar;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the business day adjustment that is performed to the result of the addition.
	  /// <para>
	  /// This adjustment is applied to the result of the period addition calculation.
	  /// If the addition is performed using business days then any adjustment here is expected to
	  /// have a different holiday calendar to that used during addition.
	  /// </para>
	  /// <para>
	  /// If no adjustment is required, use the 'None' business day adjustment.
	  /// </para>
	  /// <para>
	  /// See the class-level documentation for more information.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public BusinessDayAdjustment Adjustment
	  {
		  get
		  {
			return adjustment;
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
		  DaysAdjustment other = (DaysAdjustment) obj;
		  return (days == other.days) && JodaBeanUtils.equal(calendar, other.calendar) && JodaBeanUtils.equal(adjustment, other.adjustment);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(days);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(calendar);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(adjustment);
		return hash;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code DaysAdjustment}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  days_Renamed = DirectMetaProperty.ofImmutable(this, "days", typeof(DaysAdjustment), Integer.TYPE);
			  calendar_Renamed = DirectMetaProperty.ofImmutable(this, "calendar", typeof(DaysAdjustment), typeof(HolidayCalendarId));
			  adjustment_Renamed = DirectMetaProperty.ofImmutable(this, "adjustment", typeof(DaysAdjustment), typeof(BusinessDayAdjustment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "days", "calendar", "adjustment");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code days} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<int> days_Renamed;
		/// <summary>
		/// The meta-property for the {@code calendar} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<HolidayCalendarId> calendar_Renamed;
		/// <summary>
		/// The meta-property for the {@code adjustment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BusinessDayAdjustment> adjustment_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "days", "calendar", "adjustment");
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
			case 3076183: // days
			  return days_Renamed;
			case -178324674: // calendar
			  return calendar_Renamed;
			case 1977085293: // adjustment
			  return adjustment_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override DaysAdjustment.Builder builder()
		{
		  return new DaysAdjustment.Builder();
		}

		public override Type beanType()
		{
		  return typeof(DaysAdjustment);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code days} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<int> days()
		{
		  return days_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code calendar} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<HolidayCalendarId> calendar()
		{
		  return calendar_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code adjustment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BusinessDayAdjustment> adjustment()
		{
		  return adjustment_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3076183: // days
			  return ((DaysAdjustment) bean).Days;
			case -178324674: // calendar
			  return ((DaysAdjustment) bean).Calendar;
			case 1977085293: // adjustment
			  return ((DaysAdjustment) bean).Adjustment;
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
	  /// The bean-builder for {@code DaysAdjustment}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<DaysAdjustment>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal int days_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal HolidayCalendarId calendar_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BusinessDayAdjustment adjustment_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(DaysAdjustment beanToCopy)
		{
		  this.days_Renamed = beanToCopy.Days;
		  this.calendar_Renamed = beanToCopy.Calendar;
		  this.adjustment_Renamed = beanToCopy.Adjustment;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3076183: // days
			  return days_Renamed;
			case -178324674: // calendar
			  return calendar_Renamed;
			case 1977085293: // adjustment
			  return adjustment_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3076183: // days
			  this.days_Renamed = (int?) newValue.Value;
			  break;
			case -178324674: // calendar
			  this.calendar_Renamed = (HolidayCalendarId) newValue;
			  break;
			case 1977085293: // adjustment
			  this.adjustment_Renamed = (BusinessDayAdjustment) newValue;
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

		public override DaysAdjustment build()
		{
		  return new DaysAdjustment(days_Renamed, calendar_Renamed, adjustment_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the number of days to be added.
		/// <para>
		/// When the adjustment is performed, this amount will be added to the input date
		/// using the calendar to determine the addition type.
		/// </para>
		/// </summary>
		/// <param name="days">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder days(int days)
		{
		  JodaBeanUtils.notNull(days, "days");
		  this.days_Renamed = days;
		  return this;
		}

		/// <summary>
		/// Sets the holiday calendar that defines the meaning of a day when performing the addition.
		/// <para>
		/// When the adjustment is performed, this calendar is used to determine which days are business days.
		/// </para>
		/// <para>
		/// If the holiday calendar is 'None' then addition uses simple date addition arithmetic without
		/// considering any days as holidays or weekends.
		/// If the holiday calendar is anything other than 'None' then addition uses that calendar,
		/// effectively repeatedly finding the next business day.
		/// </para>
		/// <para>
		/// See the class-level documentation for more information.
		/// </para>
		/// </summary>
		/// <param name="calendar">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder calendar(HolidayCalendarId calendar)
		{
		  JodaBeanUtils.notNull(calendar, "calendar");
		  this.calendar_Renamed = calendar;
		  return this;
		}

		/// <summary>
		/// Sets the business day adjustment that is performed to the result of the addition.
		/// <para>
		/// This adjustment is applied to the result of the period addition calculation.
		/// If the addition is performed using business days then any adjustment here is expected to
		/// have a different holiday calendar to that used during addition.
		/// </para>
		/// <para>
		/// If no adjustment is required, use the 'None' business day adjustment.
		/// </para>
		/// <para>
		/// See the class-level documentation for more information.
		/// </para>
		/// </summary>
		/// <param name="adjustment">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder adjustment(BusinessDayAdjustment adjustment)
		{
		  JodaBeanUtils.notNull(adjustment, "adjustment");
		  this.adjustment_Renamed = adjustment;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("DaysAdjustment.Builder{");
		  buf.Append("days").Append('=').Append(JodaBeanUtils.ToString(days_Renamed)).Append(',').Append(' ');
		  buf.Append("calendar").Append('=').Append(JodaBeanUtils.ToString(calendar_Renamed)).Append(',').Append(' ');
		  buf.Append("adjustment").Append('=').Append(JodaBeanUtils.ToString(adjustment_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}