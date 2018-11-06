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
	/// An adjustment that alters a date if it falls on a day other than a business day.
	/// <para>
	/// When processing dates in finance, it is typically intended that non-business days,
	/// such as weekends and holidays, are converted to a nearby valid business day.
	/// This class represents the necessary adjustment.
	/// </para>
	/// <para>
	/// This class combines a <seealso cref="BusinessDayConvention business day convention"/>
	/// with a <seealso cref="HolidayCalendarId holiday calendar"/>. To adjust a date,
	/// <seealso cref="ReferenceData"/> must be provided to resolve the holiday calendar.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class BusinessDayAdjustment implements com.opengamma.strata.basics.Resolvable<DateAdjuster>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class BusinessDayAdjustment : Resolvable<DateAdjuster>, ImmutableBean
	{

	  /// <summary>
	  /// An instance that performs no adjustment.
	  /// </summary>
	  public static readonly BusinessDayAdjustment NONE = new BusinessDayAdjustment(BusinessDayConventions.NO_ADJUST, HolidayCalendarIds.NO_HOLIDAYS);

	  /// <summary>
	  /// The convention used to the adjust the date if it does not fall on a business day.
	  /// <para>
	  /// The convention determines whether to move forwards or backwards when it is a holiday.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final BusinessDayConvention convention;
	  private readonly BusinessDayConvention convention;
	  /// <summary>
	  /// The calendar that defines holidays and business days.
	  /// <para>
	  /// When the adjustment is made, this calendar is used to skip holidays.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final HolidayCalendarId calendar;
	  private readonly HolidayCalendarId calendar;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance using the specified convention and calendar.
	  /// <para>
	  /// When adjusting a date, the convention rule will be applied using the specified calendar.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="convention">  the convention used to the adjust the date if it does not fall on a business day </param>
	  /// <param name="calendar">  the calendar that defines holidays and business days </param>
	  /// <returns> the adjuster </returns>
	  public static BusinessDayAdjustment of(BusinessDayConvention convention, HolidayCalendarId calendar)
	  {
		return new BusinessDayAdjustment(convention, calendar);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adjusts the date as necessary if it is not a business day.
	  /// <para>
	  /// If the date is a business day it will be returned unaltered.
	  /// If the date is not a business day, the convention will be applied.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to adjust </param>
	  /// <param name="refData">  the reference data, used to find the holiday calendar </param>
	  /// <returns> the adjusted date </returns>
	  public LocalDate adjust(LocalDate date, ReferenceData refData)
	  {
		HolidayCalendar holCal = calendar.resolve(refData);
		return convention.adjust(date, holCal);
	  }

	  /// <summary>
	  /// Resolves this adjustment using the specified reference data, returning an adjuster.
	  /// <para>
	  /// This returns a <seealso cref="DateAdjuster"/> that performs the same calculation as this adjustment.
	  /// It binds the holiday calendar, looked up from the reference data, into the result.
	  /// As such, there is no need to pass the reference data in again.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="refData">  the reference data, used to find the holiday calendar </param>
	  /// <returns> the adjuster, bound to a specific holiday calendar </returns>
	  public DateAdjuster resolve(ReferenceData refData)
	  {
		HolidayCalendar holCal = calendar.resolve(refData);
		return date => convention.adjust(date, holCal);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a string describing the adjustment.
	  /// </summary>
	  /// <returns> the descriptive string </returns>
	  public override string ToString()
	  {
		if (this.Equals(NONE))
		{
		  return convention.ToString();
		}
		return convention + " using calendar " + calendar.Name;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code BusinessDayAdjustment}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static BusinessDayAdjustment.Meta meta()
	  {
		return BusinessDayAdjustment.Meta.INSTANCE;
	  }

	  static BusinessDayAdjustment()
	  {
		MetaBean.register(BusinessDayAdjustment.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static BusinessDayAdjustment.Builder builder()
	  {
		return new BusinessDayAdjustment.Builder();
	  }

	  private BusinessDayAdjustment(BusinessDayConvention convention, HolidayCalendarId calendar)
	  {
		JodaBeanUtils.notNull(convention, "convention");
		JodaBeanUtils.notNull(calendar, "calendar");
		this.convention = convention;
		this.calendar = calendar;
	  }

	  public override BusinessDayAdjustment.Meta metaBean()
	  {
		return BusinessDayAdjustment.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the convention used to the adjust the date if it does not fall on a business day.
	  /// <para>
	  /// The convention determines whether to move forwards or backwards when it is a holiday.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public BusinessDayConvention Convention
	  {
		  get
		  {
			return convention;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the calendar that defines holidays and business days.
	  /// <para>
	  /// When the adjustment is made, this calendar is used to skip holidays.
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
		  BusinessDayAdjustment other = (BusinessDayAdjustment) obj;
		  return JodaBeanUtils.equal(convention, other.convention) && JodaBeanUtils.equal(calendar, other.calendar);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(convention);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(calendar);
		return hash;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code BusinessDayAdjustment}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  convention_Renamed = DirectMetaProperty.ofImmutable(this, "convention", typeof(BusinessDayAdjustment), typeof(BusinessDayConvention));
			  calendar_Renamed = DirectMetaProperty.ofImmutable(this, "calendar", typeof(BusinessDayAdjustment), typeof(HolidayCalendarId));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "convention", "calendar");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code convention} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BusinessDayConvention> convention_Renamed;
		/// <summary>
		/// The meta-property for the {@code calendar} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<HolidayCalendarId> calendar_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "convention", "calendar");
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
			case 2039569265: // convention
			  return convention_Renamed;
			case -178324674: // calendar
			  return calendar_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override BusinessDayAdjustment.Builder builder()
		{
		  return new BusinessDayAdjustment.Builder();
		}

		public override Type beanType()
		{
		  return typeof(BusinessDayAdjustment);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code convention} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BusinessDayConvention> convention()
		{
		  return convention_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code calendar} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<HolidayCalendarId> calendar()
		{
		  return calendar_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 2039569265: // convention
			  return ((BusinessDayAdjustment) bean).Convention;
			case -178324674: // calendar
			  return ((BusinessDayAdjustment) bean).Calendar;
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
	  /// The bean-builder for {@code BusinessDayAdjustment}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<BusinessDayAdjustment>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BusinessDayConvention convention_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal HolidayCalendarId calendar_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(BusinessDayAdjustment beanToCopy)
		{
		  this.convention_Renamed = beanToCopy.Convention;
		  this.calendar_Renamed = beanToCopy.Calendar;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 2039569265: // convention
			  return convention_Renamed;
			case -178324674: // calendar
			  return calendar_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 2039569265: // convention
			  this.convention_Renamed = (BusinessDayConvention) newValue;
			  break;
			case -178324674: // calendar
			  this.calendar_Renamed = (HolidayCalendarId) newValue;
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

		public override BusinessDayAdjustment build()
		{
		  return new BusinessDayAdjustment(convention_Renamed, calendar_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the convention used to the adjust the date if it does not fall on a business day.
		/// <para>
		/// The convention determines whether to move forwards or backwards when it is a holiday.
		/// </para>
		/// </summary>
		/// <param name="convention">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder convention(BusinessDayConvention convention)
		{
		  JodaBeanUtils.notNull(convention, "convention");
		  this.convention_Renamed = convention;
		  return this;
		}

		/// <summary>
		/// Sets the calendar that defines holidays and business days.
		/// <para>
		/// When the adjustment is made, this calendar is used to skip holidays.
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

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("BusinessDayAdjustment.Builder{");
		  buf.Append("convention").Append('=').Append(JodaBeanUtils.ToString(convention_Renamed)).Append(',').Append(' ');
		  buf.Append("calendar").Append('=').Append(JodaBeanUtils.ToString(calendar_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}