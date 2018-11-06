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
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using HolidayCalendar = com.opengamma.strata.basics.date.HolidayCalendar;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using Tenor = com.opengamma.strata.basics.date.Tenor;

	/// <summary>
	/// An overnight index, such as Sonia or Eonia.
	/// <para>
	/// An index represented by this class relates to lending over one night.
	/// The rate typically refers to "Today/Tomorrow" but might refer to "Tomorrow/Next".
	/// </para>
	/// <para>
	/// The index is defined by four dates.
	/// The fixing date is the date on which the index is to be observed.
	/// The publication date is the date on which the fixed rate is actually published.
	/// The effective date is the date on which the implied deposit starts.
	/// The maturity date is the date on which the implied deposit ends.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ImmutableOvernightIndex implements OvernightIndex, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ImmutableOvernightIndex : OvernightIndex, ImmutableBean
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
	  /// The calendar that the index uses.
	  /// <para>
	  /// All dates are calculated with reference to the same calendar.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.date.HolidayCalendarId fixingCalendar;
	  private readonly HolidayCalendarId fixingCalendar;
	  /// <summary>
	  /// The number of days to add to the fixing date to obtain the publication date.
	  /// <para>
	  /// In most cases, the fixing rate is available on the fixing date.
	  /// In a few cases, publication of the fixing rate is delayed until the following business day.
	  /// This property is zero if publication is on the fixing date, or one if it is the next day.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final int publicationDateOffset;
	  private readonly int publicationDateOffset;
	  /// <summary>
	  /// The number of days to add to the fixing date to obtain the effective date.
	  /// <para>
	  /// In most cases, the settlement date and start of the implied deposit is on the fixing date.
	  /// In a few cases, the settlement date is the following business day.
	  /// This property is zero if settlement is on the fixing date, or one if it is the next day.
	  /// Maturity is always one business day after the settlement date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final int effectiveDateOffset;
	  private readonly int effectiveDateOffset;
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

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.active_Renamed = true;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (builder.defaultFixedLegDayCount_Renamed == null)
		{
		  builder.defaultFixedLegDayCount_Renamed = builder.dayCount_Renamed;
		}
	  }

	  public Tenor Tenor
	  {
		  get
		  {
			return Tenor.TENOR_1D;
		  }
	  }

	  public FloatingRateName FloatingRateName
	  {
		  get
		  {
			return FloatingRateName.of(name);
		  }
	  }

	  //-------------------------------------------------------------------------
	  public LocalDate calculatePublicationFromFixing(LocalDate fixingDate, ReferenceData refData)
	  {
		HolidayCalendar fixingCal = fixingCalendar.resolve(refData);
		return fixingCal.shift(fixingCal.nextOrSame(fixingDate), publicationDateOffset);
	  }

	  public LocalDate calculateEffectiveFromFixing(LocalDate fixingDate, ReferenceData refData)
	  {
		HolidayCalendar fixingCal = fixingCalendar.resolve(refData);
		return fixingCal.shift(fixingCal.nextOrSame(fixingDate), effectiveDateOffset);
	  }

	  public LocalDate calculateMaturityFromFixing(LocalDate fixingDate, ReferenceData refData)
	  {
		HolidayCalendar fixingCal = fixingCalendar.resolve(refData);
		return fixingCal.shift(fixingCal.nextOrSame(fixingDate), effectiveDateOffset + 1);
	  }

	  public LocalDate calculateFixingFromEffective(LocalDate effectiveDate, ReferenceData refData)
	  {
		HolidayCalendar fixingCal = fixingCalendar.resolve(refData);
		return fixingCal.shift(fixingCal.nextOrSame(effectiveDate), -effectiveDateOffset);
	  }

	  public LocalDate calculateMaturityFromEffective(LocalDate effectiveDate, ReferenceData refData)
	  {
		HolidayCalendar fixingCal = fixingCalendar.resolve(refData);
		return fixingCal.shift(fixingCal.nextOrSame(effectiveDate), 1);
	  }

	  //-------------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj is ImmutableOvernightIndex)
		{
		  return name.Equals(((ImmutableOvernightIndex) obj).name);
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
	  /// The meta-bean for {@code ImmutableOvernightIndex}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ImmutableOvernightIndex.Meta meta()
	  {
		return ImmutableOvernightIndex.Meta.INSTANCE;
	  }

	  static ImmutableOvernightIndex()
	  {
		MetaBean.register(ImmutableOvernightIndex.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ImmutableOvernightIndex.Builder builder()
	  {
		return new ImmutableOvernightIndex.Builder();
	  }

	  private ImmutableOvernightIndex(string name, Currency currency, bool active, HolidayCalendarId fixingCalendar, int publicationDateOffset, int effectiveDateOffset, DayCount dayCount, DayCount defaultFixedLegDayCount)
	  {
		JodaBeanUtils.notNull(name, "name");
		JodaBeanUtils.notNull(currency, "currency");
		JodaBeanUtils.notNull(fixingCalendar, "fixingCalendar");
		JodaBeanUtils.notNull(publicationDateOffset, "publicationDateOffset");
		JodaBeanUtils.notNull(effectiveDateOffset, "effectiveDateOffset");
		JodaBeanUtils.notNull(dayCount, "dayCount");
		JodaBeanUtils.notNull(defaultFixedLegDayCount, "defaultFixedLegDayCount");
		this.name = name;
		this.currency = currency;
		this.active = active;
		this.fixingCalendar = fixingCalendar;
		this.publicationDateOffset = publicationDateOffset;
		this.effectiveDateOffset = effectiveDateOffset;
		this.dayCount = dayCount;
		this.defaultFixedLegDayCount = defaultFixedLegDayCount;
	  }

	  public override ImmutableOvernightIndex.Meta metaBean()
	  {
		return ImmutableOvernightIndex.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the index name, such as 'GBP-SONIA'. </summary>
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
	  /// Gets the calendar that the index uses.
	  /// <para>
	  /// All dates are calculated with reference to the same calendar.
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
	  /// Gets the number of days to add to the fixing date to obtain the publication date.
	  /// <para>
	  /// In most cases, the fixing rate is available on the fixing date.
	  /// In a few cases, publication of the fixing rate is delayed until the following business day.
	  /// This property is zero if publication is on the fixing date, or one if it is the next day.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public int PublicationDateOffset
	  {
		  get
		  {
			return publicationDateOffset;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of days to add to the fixing date to obtain the effective date.
	  /// <para>
	  /// In most cases, the settlement date and start of the implied deposit is on the fixing date.
	  /// In a few cases, the settlement date is the following business day.
	  /// This property is zero if settlement is on the fixing date, or one if it is the next day.
	  /// Maturity is always one business day after the settlement date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public int EffectiveDateOffset
	  {
		  get
		  {
			return effectiveDateOffset;
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
	  /// The meta-bean for {@code ImmutableOvernightIndex}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(ImmutableOvernightIndex), typeof(string));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(ImmutableOvernightIndex), typeof(Currency));
			  active_Renamed = DirectMetaProperty.ofImmutable(this, "active", typeof(ImmutableOvernightIndex), Boolean.TYPE);
			  fixingCalendar_Renamed = DirectMetaProperty.ofImmutable(this, "fixingCalendar", typeof(ImmutableOvernightIndex), typeof(HolidayCalendarId));
			  publicationDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "publicationDateOffset", typeof(ImmutableOvernightIndex), Integer.TYPE);
			  effectiveDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "effectiveDateOffset", typeof(ImmutableOvernightIndex), Integer.TYPE);
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(ImmutableOvernightIndex), typeof(DayCount));
			  defaultFixedLegDayCount_Renamed = DirectMetaProperty.ofImmutable(this, "defaultFixedLegDayCount", typeof(ImmutableOvernightIndex), typeof(DayCount));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "currency", "active", "fixingCalendar", "publicationDateOffset", "effectiveDateOffset", "dayCount", "defaultFixedLegDayCount");
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
		/// The meta-property for the {@code publicationDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<int> publicationDateOffset_Renamed;
		/// <summary>
		/// The meta-property for the {@code effectiveDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<int> effectiveDateOffset_Renamed;
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
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "currency", "active", "fixingCalendar", "publicationDateOffset", "effectiveDateOffset", "dayCount", "defaultFixedLegDayCount");
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
			case 1901198637: // publicationDateOffset
			  return publicationDateOffset_Renamed;
			case 1571923688: // effectiveDateOffset
			  return effectiveDateOffset_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case -2037801138: // defaultFixedLegDayCount
			  return defaultFixedLegDayCount_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ImmutableOvernightIndex.Builder builder()
		{
		  return new ImmutableOvernightIndex.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ImmutableOvernightIndex);
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
		/// The meta-property for the {@code publicationDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<int> publicationDateOffset()
		{
		  return publicationDateOffset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code effectiveDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<int> effectiveDateOffset()
		{
		  return effectiveDateOffset_Renamed;
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
			  return ((ImmutableOvernightIndex) bean).Name;
			case 575402001: // currency
			  return ((ImmutableOvernightIndex) bean).Currency;
			case -1422950650: // active
			  return ((ImmutableOvernightIndex) bean).Active;
			case 394230283: // fixingCalendar
			  return ((ImmutableOvernightIndex) bean).FixingCalendar;
			case 1901198637: // publicationDateOffset
			  return ((ImmutableOvernightIndex) bean).PublicationDateOffset;
			case 1571923688: // effectiveDateOffset
			  return ((ImmutableOvernightIndex) bean).EffectiveDateOffset;
			case 1905311443: // dayCount
			  return ((ImmutableOvernightIndex) bean).DayCount;
			case -2037801138: // defaultFixedLegDayCount
			  return ((ImmutableOvernightIndex) bean).DefaultFixedLegDayCount;
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
	  /// The bean-builder for {@code ImmutableOvernightIndex}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ImmutableOvernightIndex>
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
		internal int publicationDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal int effectiveDateOffset_Renamed;
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
		internal Builder(ImmutableOvernightIndex beanToCopy)
		{
		  this.name_Renamed = beanToCopy.Name;
		  this.currency_Renamed = beanToCopy.Currency;
		  this.active_Renamed = beanToCopy.Active;
		  this.fixingCalendar_Renamed = beanToCopy.FixingCalendar;
		  this.publicationDateOffset_Renamed = beanToCopy.PublicationDateOffset;
		  this.effectiveDateOffset_Renamed = beanToCopy.EffectiveDateOffset;
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
			case 1901198637: // publicationDateOffset
			  return publicationDateOffset_Renamed;
			case 1571923688: // effectiveDateOffset
			  return effectiveDateOffset_Renamed;
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
			case 1901198637: // publicationDateOffset
			  this.publicationDateOffset_Renamed = (int?) newValue.Value;
			  break;
			case 1571923688: // effectiveDateOffset
			  this.effectiveDateOffset_Renamed = (int?) newValue.Value;
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

		public override ImmutableOvernightIndex build()
		{
		  preBuild(this);
		  return new ImmutableOvernightIndex(name_Renamed, currency_Renamed, active_Renamed, fixingCalendar_Renamed, publicationDateOffset_Renamed, effectiveDateOffset_Renamed, dayCount_Renamed, defaultFixedLegDayCount_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the index name, such as 'GBP-SONIA'. </summary>
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
		/// Sets the calendar that the index uses.
		/// <para>
		/// All dates are calculated with reference to the same calendar.
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
		/// Sets the number of days to add to the fixing date to obtain the publication date.
		/// <para>
		/// In most cases, the fixing rate is available on the fixing date.
		/// In a few cases, publication of the fixing rate is delayed until the following business day.
		/// This property is zero if publication is on the fixing date, or one if it is the next day.
		/// </para>
		/// </summary>
		/// <param name="publicationDateOffset">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder publicationDateOffset(int publicationDateOffset)
		{
		  JodaBeanUtils.notNull(publicationDateOffset, "publicationDateOffset");
		  this.publicationDateOffset_Renamed = publicationDateOffset;
		  return this;
		}

		/// <summary>
		/// Sets the number of days to add to the fixing date to obtain the effective date.
		/// <para>
		/// In most cases, the settlement date and start of the implied deposit is on the fixing date.
		/// In a few cases, the settlement date is the following business day.
		/// This property is zero if settlement is on the fixing date, or one if it is the next day.
		/// Maturity is always one business day after the settlement date.
		/// </para>
		/// </summary>
		/// <param name="effectiveDateOffset">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder effectiveDateOffset(int effectiveDateOffset)
		{
		  JodaBeanUtils.notNull(effectiveDateOffset, "effectiveDateOffset");
		  this.effectiveDateOffset_Renamed = effectiveDateOffset;
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
		  StringBuilder buf = new StringBuilder(288);
		  buf.Append("ImmutableOvernightIndex.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("active").Append('=').Append(JodaBeanUtils.ToString(active_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingCalendar").Append('=').Append(JodaBeanUtils.ToString(fixingCalendar_Renamed)).Append(',').Append(' ');
		  buf.Append("publicationDateOffset").Append('=').Append(JodaBeanUtils.ToString(publicationDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("effectiveDateOffset").Append('=').Append(JodaBeanUtils.ToString(effectiveDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("defaultFixedLegDayCount").Append('=').Append(JodaBeanUtils.ToString(defaultFixedLegDayCount_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}