using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using MoreObjects = com.google.common.@base.MoreObjects;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DateAdjuster = com.opengamma.strata.basics.date.DateAdjuster;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendar = com.opengamma.strata.basics.date.HolidayCalendar;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using TenorAdjustment = com.opengamma.strata.basics.date.TenorAdjustment;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// An Ibor index implementation based on an immutable set of rules.
	/// <para>
	/// A standard immutable implementation of <seealso cref="IborIndex"/> that defines the currency
	/// and the rules for converting from fixing to effective and maturity.
	/// </para>
	/// <para>
	/// In most cases, applications should refer to indices by name, using <seealso cref="IborIndex#of(String)"/>.
	/// The named index will typically be resolved to an instance of this class.
	/// As such, it is recommended to use the {@code IborIndex} interface in application
	/// code rather than directly referring to this class.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ImmutableIborIndex implements IborIndex, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ImmutableIborIndex : IborIndex, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final String name;
		private readonly string name;
	  /// <summary>
	  /// The currency of the index.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.currency.Currency currency;
	  private readonly Currency currency;
	  /// <summary>
	  /// Whether the index is active, defaulted to true.
	  /// <para>
	  /// Over time some indices become inactive and are no longer produced.
	  /// If this occurs, this flag will be set to false.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(overrideGet = true) private final boolean active;
	  private readonly bool active;
	  /// <summary>
	  /// The calendar that determines which dates are fixing dates.
	  /// <para>
	  /// The fixing date is when the rate is determined.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.date.HolidayCalendarId fixingCalendar;
	  private readonly HolidayCalendarId fixingCalendar;
	  /// <summary>
	  /// The fixing time.
	  /// <para>
	  /// The rate is fixed at the fixing time of the fixing date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalTime fixingTime;
	  private readonly LocalTime fixingTime;
	  /// <summary>
	  /// The fixing time-zone.
	  /// <para>
	  /// The time-zone of the fixing time.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.ZoneId fixingZone;
	  private readonly ZoneId fixingZone;
	  /// <summary>
	  /// The adjustment applied to the effective date to obtain the fixing date.
	  /// <para>
	  /// The fixing date is the date on which the index is to be observed.
	  /// In most cases, the fixing date is 0 or 2 days before the effective date.
	  /// This data structure allows the complex rules of some indices to be represented.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.date.DaysAdjustment fixingDateOffset;
	  private readonly DaysAdjustment fixingDateOffset;
	  /// <summary>
	  /// The adjustment applied to the fixing date to obtain the effective date.
	  /// <para>
	  /// The effective date is the start date of the indexed deposit.
	  /// In most cases, the effective date is 0 or 2 days after the fixing date.
	  /// This data structure allows the complex rules of some indices to be represented.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.date.DaysAdjustment effectiveDateOffset;
	  private readonly DaysAdjustment effectiveDateOffset;
	  /// <summary>
	  /// The adjustment applied to the effective date to obtain the maturity date.
	  /// <para>
	  /// The maturity date is the end date of the indexed deposit and is relative to the effective date.
	  /// This data structure allows the complex rules of some indices to be represented.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.date.TenorAdjustment maturityDateOffset;
	  private readonly TenorAdjustment maturityDateOffset;
	  /// <summary>
	  /// The day count convention.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.date.DayCount dayCount;
	  private readonly DayCount dayCount;
	  /// <summary>
	  /// The default day count convention for the associated fixed leg.
	  /// <para>
	  /// A rate index is often paid against a fixed leg, such as in a vanilla Swap.
	  /// The day count convention of the fixed leg often differs from that of the index,
	  /// and the default is value is available here.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.date.DayCount defaultFixedLegDayCount;
	  private readonly DayCount defaultFixedLegDayCount;
	  /// <summary>
	  /// The floating rate name, such as 'GBP-LIBOR'.
	  /// </summary>
	  [NonSerialized]
	  private readonly string floatingRateName; // derived

	  //-------------------------------------------------------------------------
	  // creates an instance
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private ImmutableIborIndex(String name, com.opengamma.strata.basics.currency.Currency currency, boolean active, com.opengamma.strata.basics.date.HolidayCalendarId fixingCalendar, java.time.LocalTime fixingTime, java.time.ZoneId fixingZone, com.opengamma.strata.basics.date.DaysAdjustment fixingDateOffset, com.opengamma.strata.basics.date.DaysAdjustment effectiveDateOffset, com.opengamma.strata.basics.date.TenorAdjustment maturityDateOffset, com.opengamma.strata.basics.date.DayCount dayCount, com.opengamma.strata.basics.date.DayCount defaultFixedLegDayCount)
	  private ImmutableIborIndex(string name, Currency currency, bool active, HolidayCalendarId fixingCalendar, LocalTime fixingTime, ZoneId fixingZone, DaysAdjustment fixingDateOffset, DaysAdjustment effectiveDateOffset, TenorAdjustment maturityDateOffset, DayCount dayCount, DayCount defaultFixedLegDayCount)
	  {

		this.name = ArgChecker.notNull(name, "name");
		this.currency = ArgChecker.notNull(currency, "currency");
		this.active = active;
		this.fixingCalendar = ArgChecker.notNull(fixingCalendar, "fixingCalendar");
		this.fixingTime = ArgChecker.notNull(fixingTime, "fixingTime");
		this.fixingZone = ArgChecker.notNull(fixingZone, "fixingZone");
		this.fixingDateOffset = ArgChecker.notNull(fixingDateOffset, "fixingDateOffset");
		this.effectiveDateOffset = ArgChecker.notNull(effectiveDateOffset, "effectiveDateOffset");
		this.maturityDateOffset = ArgChecker.notNull(maturityDateOffset, "maturityDateOffset");
		this.dayCount = ArgChecker.notNull(dayCount, "dayCount");
		this.defaultFixedLegDayCount = MoreObjects.firstNonNull(defaultFixedLegDayCount, dayCount);
		// derive from name, but don't store FloatingRateName, to avoid directly linking data at this point
		string suffix = "-" + maturityDateOffset.Tenor.ToString();
		if (!name.EndsWith(suffix, StringComparison.Ordinal))
		{
		  throw new System.ArgumentException(Messages.format("IborIndex name '{}' must end with tenor '{}'", name, maturityDateOffset.Tenor.ToString()));
		}
		this.floatingRateName = name.Substring(0, name.Length - suffix.Length);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.active_Renamed = true;
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new ImmutableIborIndex(name, currency, active, fixingCalendar, fixingTime, fixingZone, fixingDateOffset, effectiveDateOffset, maturityDateOffset, dayCount, defaultFixedLegDayCount);
	  }

	  //-------------------------------------------------------------------------
	  public Tenor Tenor
	  {
		  get
		  {
			return maturityDateOffset.Tenor;
		  }
	  }

	  public FloatingRateName FloatingRateName
	  {
		  get
		  {
			return FloatingRateName.of(floatingRateName);
		  }
	  }

	  //-------------------------------------------------------------------------
	  public ZonedDateTime calculateFixingDateTime(LocalDate fixingDate)
	  {
		return fixingDate.atTime(fixingTime).atZone(fixingZone);
	  }

	  public LocalDate calculateEffectiveFromFixing(LocalDate fixingDate, ReferenceData refData)
	  {
		LocalDate fixingBusinessDay = fixingCalendar.resolve(refData).nextOrSame(fixingDate);
		return effectiveDateOffset.adjust(fixingBusinessDay, refData);
	  }

	  public LocalDate calculateMaturityFromFixing(LocalDate fixingDate, ReferenceData refData)
	  {
		LocalDate fixingBusinessDay = fixingCalendar.resolve(refData).nextOrSame(fixingDate);
		return maturityDateOffset.adjust(effectiveDateOffset.adjust(fixingBusinessDay, refData), refData);
	  }

	  public LocalDate calculateFixingFromEffective(LocalDate effectiveDate, ReferenceData refData)
	  {
		LocalDate effectiveBusinessDay = effectiveDateCalendar(refData).nextOrSame(effectiveDate);
		return fixingDateOffset.adjust(effectiveBusinessDay, refData);
	  }

	  public LocalDate calculateMaturityFromEffective(LocalDate effectiveDate, ReferenceData refData)
	  {
		LocalDate effectiveBusinessDay = effectiveDateCalendar(refData).nextOrSame(effectiveDate);
		return maturityDateOffset.adjust(effectiveBusinessDay, refData);
	  }

	  // finds the calendar of the effective date
	  private HolidayCalendar effectiveDateCalendar(ReferenceData refData)
	  {
		HolidayCalendarId cal = effectiveDateOffset.ResultCalendar;
		if (cal == HolidayCalendarIds.NO_HOLIDAYS)
		{
		  cal = fixingCalendar;
		}
		return cal.resolve(refData);
	  }

	  public System.Func<LocalDate, IborIndexObservation> resolve(ReferenceData refData)
	  {
		HolidayCalendar fixingCal = fixingCalendar.resolve(refData);
		DateAdjuster effectiveAdjuster = effectiveDateOffset.resolve(refData);
		DateAdjuster maturityAdjuster = maturityDateOffset.resolve(refData);
		return fixingDate => create(fixingDate, fixingCal, effectiveAdjuster, maturityAdjuster);
	  }

	  // creates an observation
	  private IborIndexObservation create(LocalDate fixingDate, HolidayCalendar fixingCal, DateAdjuster effectiveAdjuster, DateAdjuster maturityAdjuster)
	  {

		LocalDate fixingBusinessDay = fixingCal.nextOrSame(fixingDate);
		LocalDate effectiveDate = effectiveAdjuster.adjust(fixingBusinessDay);
		LocalDate maturityDate = maturityAdjuster.adjust(effectiveDate);
		double yearFraction = dayCount.yearFraction(effectiveDate, maturityDate);
		return new IborIndexObservation(this, fixingDate, effectiveDate, maturityDate, yearFraction);
	  }

	  //-------------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj is ImmutableIborIndex)
		{
		  return name.Equals(((ImmutableIborIndex) obj).name);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		return name.GetHashCode();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the name of the index.
	  /// </summary>
	  /// <returns> the name of the index </returns>
	  public override string ToString()
	  {
		return Name;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableIborIndex}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ImmutableIborIndex.Meta meta()
	  {
		return ImmutableIborIndex.Meta.INSTANCE;
	  }

	  static ImmutableIborIndex()
	  {
		MetaBean.register(ImmutableIborIndex.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ImmutableIborIndex.Builder builder()
	  {
		return new ImmutableIborIndex.Builder();
	  }

	  public override ImmutableIborIndex.Meta metaBean()
	  {
		return ImmutableIborIndex.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the index name, such as 'GBP-LIBOR-3M'. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public string Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency of the index. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Currency Currency
	  {
		  get
		  {
			return currency;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets whether the index is active, defaulted to true.
	  /// <para>
	  /// Over time some indices become inactive and are no longer produced.
	  /// If this occurs, this flag will be set to false.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public bool Active
	  {
		  get
		  {
			return active;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the calendar that determines which dates are fixing dates.
	  /// <para>
	  /// The fixing date is when the rate is determined.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public HolidayCalendarId FixingCalendar
	  {
		  get
		  {
			return fixingCalendar;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the fixing time.
	  /// <para>
	  /// The rate is fixed at the fixing time of the fixing date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalTime FixingTime
	  {
		  get
		  {
			return fixingTime;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the fixing time-zone.
	  /// <para>
	  /// The time-zone of the fixing time.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ZoneId FixingZone
	  {
		  get
		  {
			return fixingZone;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the adjustment applied to the effective date to obtain the fixing date.
	  /// <para>
	  /// The fixing date is the date on which the index is to be observed.
	  /// In most cases, the fixing date is 0 or 2 days before the effective date.
	  /// This data structure allows the complex rules of some indices to be represented.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DaysAdjustment FixingDateOffset
	  {
		  get
		  {
			return fixingDateOffset;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the adjustment applied to the fixing date to obtain the effective date.
	  /// <para>
	  /// The effective date is the start date of the indexed deposit.
	  /// In most cases, the effective date is 0 or 2 days after the fixing date.
	  /// This data structure allows the complex rules of some indices to be represented.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DaysAdjustment EffectiveDateOffset
	  {
		  get
		  {
			return effectiveDateOffset;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the adjustment applied to the effective date to obtain the maturity date.
	  /// <para>
	  /// The maturity date is the end date of the indexed deposit and is relative to the effective date.
	  /// This data structure allows the complex rules of some indices to be represented.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public TenorAdjustment MaturityDateOffset
	  {
		  get
		  {
			return maturityDateOffset;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the day count convention. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DayCount DayCount
	  {
		  get
		  {
			return dayCount;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the default day count convention for the associated fixed leg.
	  /// <para>
	  /// A rate index is often paid against a fixed leg, such as in a vanilla Swap.
	  /// The day count convention of the fixed leg often differs from that of the index,
	  /// and the default is value is available here.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public override DayCount DefaultFixedLegDayCount
	  {
		  get
		  {
			return defaultFixedLegDayCount;
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

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableIborIndex}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(ImmutableIborIndex), typeof(string));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(ImmutableIborIndex), typeof(Currency));
			  active_Renamed = DirectMetaProperty.ofImmutable(this, "active", typeof(ImmutableIborIndex), Boolean.TYPE);
			  fixingCalendar_Renamed = DirectMetaProperty.ofImmutable(this, "fixingCalendar", typeof(ImmutableIborIndex), typeof(HolidayCalendarId));
			  fixingTime_Renamed = DirectMetaProperty.ofImmutable(this, "fixingTime", typeof(ImmutableIborIndex), typeof(LocalTime));
			  fixingZone_Renamed = DirectMetaProperty.ofImmutable(this, "fixingZone", typeof(ImmutableIborIndex), typeof(ZoneId));
			  fixingDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "fixingDateOffset", typeof(ImmutableIborIndex), typeof(DaysAdjustment));
			  effectiveDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "effectiveDateOffset", typeof(ImmutableIborIndex), typeof(DaysAdjustment));
			  maturityDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "maturityDateOffset", typeof(ImmutableIborIndex), typeof(TenorAdjustment));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(ImmutableIborIndex), typeof(DayCount));
			  defaultFixedLegDayCount_Renamed = DirectMetaProperty.ofImmutable(this, "defaultFixedLegDayCount", typeof(ImmutableIborIndex), typeof(DayCount));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "currency", "active", "fixingCalendar", "fixingTime", "fixingZone", "fixingDateOffset", "effectiveDateOffset", "maturityDateOffset", "dayCount", "defaultFixedLegDayCount");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code name} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<string> name_Renamed;
		/// <summary>
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-property for the {@code active} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<bool> active_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingCalendar} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<HolidayCalendarId> fixingCalendar_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingTime} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalTime> fixingTime_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingZone} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ZoneId> fixingZone_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> fixingDateOffset_Renamed;
		/// <summary>
		/// The meta-property for the {@code effectiveDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> effectiveDateOffset_Renamed;
		/// <summary>
		/// The meta-property for the {@code maturityDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<TenorAdjustment> maturityDateOffset_Renamed;
		/// <summary>
		/// The meta-property for the {@code dayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> dayCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code defaultFixedLegDayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> defaultFixedLegDayCount_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "currency", "active", "fixingCalendar", "fixingTime", "fixingZone", "fixingDateOffset", "effectiveDateOffset", "maturityDateOffset", "dayCount", "defaultFixedLegDayCount");
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
			case 3373707: // name
			  return name_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case -1422950650: // active
			  return active_Renamed;
			case 394230283: // fixingCalendar
			  return fixingCalendar_Renamed;
			case 1255686170: // fixingTime
			  return fixingTime_Renamed;
			case 1255870713: // fixingZone
			  return fixingZone_Renamed;
			case 873743726: // fixingDateOffset
			  return fixingDateOffset_Renamed;
			case 1571923688: // effectiveDateOffset
			  return effectiveDateOffset_Renamed;
			case 1574797394: // maturityDateOffset
			  return maturityDateOffset_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case -2037801138: // defaultFixedLegDayCount
			  return defaultFixedLegDayCount_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ImmutableIborIndex.Builder builder()
		{
		  return new ImmutableIborIndex.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ImmutableIborIndex);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code name} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<string> name()
		{
		  return name_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code currency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> currency()
		{
		  return currency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code active} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<bool> active()
		{
		  return active_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixingCalendar} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<HolidayCalendarId> fixingCalendar()
		{
		  return fixingCalendar_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixingTime} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalTime> fixingTime()
		{
		  return fixingTime_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixingZone} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ZoneId> fixingZone()
		{
		  return fixingZone_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixingDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> fixingDateOffset()
		{
		  return fixingDateOffset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code effectiveDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> effectiveDateOffset()
		{
		  return effectiveDateOffset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code maturityDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<TenorAdjustment> maturityDateOffset()
		{
		  return maturityDateOffset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dayCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DayCount> dayCount()
		{
		  return dayCount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code defaultFixedLegDayCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DayCount> defaultFixedLegDayCount()
		{
		  return defaultFixedLegDayCount_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return ((ImmutableIborIndex) bean).Name;
			case 575402001: // currency
			  return ((ImmutableIborIndex) bean).Currency;
			case -1422950650: // active
			  return ((ImmutableIborIndex) bean).Active;
			case 394230283: // fixingCalendar
			  return ((ImmutableIborIndex) bean).FixingCalendar;
			case 1255686170: // fixingTime
			  return ((ImmutableIborIndex) bean).FixingTime;
			case 1255870713: // fixingZone
			  return ((ImmutableIborIndex) bean).FixingZone;
			case 873743726: // fixingDateOffset
			  return ((ImmutableIborIndex) bean).FixingDateOffset;
			case 1571923688: // effectiveDateOffset
			  return ((ImmutableIborIndex) bean).EffectiveDateOffset;
			case 1574797394: // maturityDateOffset
			  return ((ImmutableIborIndex) bean).MaturityDateOffset;
			case 1905311443: // dayCount
			  return ((ImmutableIborIndex) bean).DayCount;
			case -2037801138: // defaultFixedLegDayCount
			  return ((ImmutableIborIndex) bean).DefaultFixedLegDayCount;
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
	  /// The bean-builder for {@code ImmutableIborIndex}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ImmutableIborIndex>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal string name_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Currency currency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal bool active_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal HolidayCalendarId fixingCalendar_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalTime fixingTime_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ZoneId fixingZone_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment fixingDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment effectiveDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal TenorAdjustment maturityDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount dayCount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount defaultFixedLegDayCount_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		  applyDefaults(this);
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ImmutableIborIndex beanToCopy)
		{
		  this.name_Renamed = beanToCopy.Name;
		  this.currency_Renamed = beanToCopy.Currency;
		  this.active_Renamed = beanToCopy.Active;
		  this.fixingCalendar_Renamed = beanToCopy.FixingCalendar;
		  this.fixingTime_Renamed = beanToCopy.FixingTime;
		  this.fixingZone_Renamed = beanToCopy.FixingZone;
		  this.fixingDateOffset_Renamed = beanToCopy.FixingDateOffset;
		  this.effectiveDateOffset_Renamed = beanToCopy.EffectiveDateOffset;
		  this.maturityDateOffset_Renamed = beanToCopy.MaturityDateOffset;
		  this.dayCount_Renamed = beanToCopy.DayCount;
		  this.defaultFixedLegDayCount_Renamed = beanToCopy.DefaultFixedLegDayCount;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case -1422950650: // active
			  return active_Renamed;
			case 394230283: // fixingCalendar
			  return fixingCalendar_Renamed;
			case 1255686170: // fixingTime
			  return fixingTime_Renamed;
			case 1255870713: // fixingZone
			  return fixingZone_Renamed;
			case 873743726: // fixingDateOffset
			  return fixingDateOffset_Renamed;
			case 1571923688: // effectiveDateOffset
			  return effectiveDateOffset_Renamed;
			case 1574797394: // maturityDateOffset
			  return maturityDateOffset_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case -2037801138: // defaultFixedLegDayCount
			  return defaultFixedLegDayCount_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  this.name_Renamed = (string) newValue;
			  break;
			case 575402001: // currency
			  this.currency_Renamed = (Currency) newValue;
			  break;
			case -1422950650: // active
			  this.active_Renamed = (bool?) newValue.Value;
			  break;
			case 394230283: // fixingCalendar
			  this.fixingCalendar_Renamed = (HolidayCalendarId) newValue;
			  break;
			case 1255686170: // fixingTime
			  this.fixingTime_Renamed = (LocalTime) newValue;
			  break;
			case 1255870713: // fixingZone
			  this.fixingZone_Renamed = (ZoneId) newValue;
			  break;
			case 873743726: // fixingDateOffset
			  this.fixingDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case 1571923688: // effectiveDateOffset
			  this.effectiveDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case 1574797394: // maturityDateOffset
			  this.maturityDateOffset_Renamed = (TenorAdjustment) newValue;
			  break;
			case 1905311443: // dayCount
			  this.dayCount_Renamed = (DayCount) newValue;
			  break;
			case -2037801138: // defaultFixedLegDayCount
			  this.defaultFixedLegDayCount_Renamed = (DayCount) newValue;
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

		public override ImmutableIborIndex build()
		{
		  return new ImmutableIborIndex(name_Renamed, currency_Renamed, active_Renamed, fixingCalendar_Renamed, fixingTime_Renamed, fixingZone_Renamed, fixingDateOffset_Renamed, effectiveDateOffset_Renamed, maturityDateOffset_Renamed, dayCount_Renamed, defaultFixedLegDayCount_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the index name, such as 'GBP-LIBOR-3M'. </summary>
		/// <param name="name">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder name(string name)
		{
		  JodaBeanUtils.notNull(name, "name");
		  this.name_Renamed = name;
		  return this;
		}

		/// <summary>
		/// Sets the currency of the index. </summary>
		/// <param name="currency">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder currency(Currency currency)
		{
		  JodaBeanUtils.notNull(currency, "currency");
		  this.currency_Renamed = currency;
		  return this;
		}

		/// <summary>
		/// Sets whether the index is active, defaulted to true.
		/// <para>
		/// Over time some indices become inactive and are no longer produced.
		/// If this occurs, this flag will be set to false.
		/// </para>
		/// </summary>
		/// <param name="active">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder active(bool active)
		{
		  this.active_Renamed = active;
		  return this;
		}

		/// <summary>
		/// Sets the calendar that determines which dates are fixing dates.
		/// <para>
		/// The fixing date is when the rate is determined.
		/// </para>
		/// </summary>
		/// <param name="fixingCalendar">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixingCalendar(HolidayCalendarId fixingCalendar)
		{
		  JodaBeanUtils.notNull(fixingCalendar, "fixingCalendar");
		  this.fixingCalendar_Renamed = fixingCalendar;
		  return this;
		}

		/// <summary>
		/// Sets the fixing time.
		/// <para>
		/// The rate is fixed at the fixing time of the fixing date.
		/// </para>
		/// </summary>
		/// <param name="fixingTime">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixingTime(LocalTime fixingTime)
		{
		  JodaBeanUtils.notNull(fixingTime, "fixingTime");
		  this.fixingTime_Renamed = fixingTime;
		  return this;
		}

		/// <summary>
		/// Sets the fixing time-zone.
		/// <para>
		/// The time-zone of the fixing time.
		/// </para>
		/// </summary>
		/// <param name="fixingZone">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixingZone(ZoneId fixingZone)
		{
		  JodaBeanUtils.notNull(fixingZone, "fixingZone");
		  this.fixingZone_Renamed = fixingZone;
		  return this;
		}

		/// <summary>
		/// Sets the adjustment applied to the effective date to obtain the fixing date.
		/// <para>
		/// The fixing date is the date on which the index is to be observed.
		/// In most cases, the fixing date is 0 or 2 days before the effective date.
		/// This data structure allows the complex rules of some indices to be represented.
		/// </para>
		/// </summary>
		/// <param name="fixingDateOffset">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixingDateOffset(DaysAdjustment fixingDateOffset)
		{
		  JodaBeanUtils.notNull(fixingDateOffset, "fixingDateOffset");
		  this.fixingDateOffset_Renamed = fixingDateOffset;
		  return this;
		}

		/// <summary>
		/// Sets the adjustment applied to the fixing date to obtain the effective date.
		/// <para>
		/// The effective date is the start date of the indexed deposit.
		/// In most cases, the effective date is 0 or 2 days after the fixing date.
		/// This data structure allows the complex rules of some indices to be represented.
		/// </para>
		/// </summary>
		/// <param name="effectiveDateOffset">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder effectiveDateOffset(DaysAdjustment effectiveDateOffset)
		{
		  JodaBeanUtils.notNull(effectiveDateOffset, "effectiveDateOffset");
		  this.effectiveDateOffset_Renamed = effectiveDateOffset;
		  return this;
		}

		/// <summary>
		/// Sets the adjustment applied to the effective date to obtain the maturity date.
		/// <para>
		/// The maturity date is the end date of the indexed deposit and is relative to the effective date.
		/// This data structure allows the complex rules of some indices to be represented.
		/// </para>
		/// </summary>
		/// <param name="maturityDateOffset">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder maturityDateOffset(TenorAdjustment maturityDateOffset)
		{
		  JodaBeanUtils.notNull(maturityDateOffset, "maturityDateOffset");
		  this.maturityDateOffset_Renamed = maturityDateOffset;
		  return this;
		}

		/// <summary>
		/// Sets the day count convention. </summary>
		/// <param name="dayCount">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dayCount(DayCount dayCount)
		{
		  JodaBeanUtils.notNull(dayCount, "dayCount");
		  this.dayCount_Renamed = dayCount;
		  return this;
		}

		/// <summary>
		/// Sets the default day count convention for the associated fixed leg.
		/// <para>
		/// A rate index is often paid against a fixed leg, such as in a vanilla Swap.
		/// The day count convention of the fixed leg often differs from that of the index,
		/// and the default is value is available here.
		/// </para>
		/// </summary>
		/// <param name="defaultFixedLegDayCount">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder defaultFixedLegDayCount(DayCount defaultFixedLegDayCount)
		{
		  JodaBeanUtils.notNull(defaultFixedLegDayCount, "defaultFixedLegDayCount");
		  this.defaultFixedLegDayCount_Renamed = defaultFixedLegDayCount;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(384);
		  buf.Append("ImmutableIborIndex.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("active").Append('=').Append(JodaBeanUtils.ToString(active_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingCalendar").Append('=').Append(JodaBeanUtils.ToString(fixingCalendar_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingTime").Append('=').Append(JodaBeanUtils.ToString(fixingTime_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingZone").Append('=').Append(JodaBeanUtils.ToString(fixingZone_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingDateOffset").Append('=').Append(JodaBeanUtils.ToString(fixingDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("effectiveDateOffset").Append('=').Append(JodaBeanUtils.ToString(effectiveDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("maturityDateOffset").Append('=').Append(JodaBeanUtils.ToString(maturityDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("defaultFixedLegDayCount").Append('=').Append(JodaBeanUtils.ToString(defaultFixedLegDayCount_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}