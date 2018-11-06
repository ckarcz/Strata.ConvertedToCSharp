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
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DateAdjuster = com.opengamma.strata.basics.date.DateAdjuster;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendar = com.opengamma.strata.basics.date.HolidayCalendar;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;

	/// <summary>
	/// A foreign exchange index implementation based on an immutable set of rules.
	/// <para>
	/// A standard immutable implementation of <seealso cref="FxIndex"/> that defines the currency pair
	/// and the rule for converting from fixing to maturity.
	/// </para>
	/// <para>
	/// In most cases, applications should refer to indices by name, using <seealso cref="FxIndex#of(String)"/>.
	/// The named index will typically be resolved to an instance of this class.
	/// As such, it is recommended to use the {@code FxIndex} interface in application
	/// code rather than directly referring to this class.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ImmutableFxIndex implements FxIndex, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ImmutableFxIndex : FxIndex, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final String name;
		private readonly string name;
	  /// <summary>
	  /// The currency pair.
	  /// <para>
	  /// An index defines an FX rate in a single direction, such as from EUR to USD.
	  /// This currency pair defines that direction.
	  /// </para>
	  /// <para>
	  /// In most cases, the same index can be used to convert in both directions
	  /// by taking the rate or the reciprocal as necessary.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.currency.CurrencyPair currencyPair;
	  private readonly CurrencyPair currencyPair;
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
	  /// The adjustment applied to the maturity date to obtain the fixing date.
	  /// <para>
	  /// The maturity date is the start date of the indexed deposit.
	  /// In most cases, the fixing date is 2 days before the maturity date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.date.DaysAdjustment fixingDateOffset;
	  private readonly DaysAdjustment fixingDateOffset;
	  /// <summary>
	  /// The adjustment applied to the fixing date to obtain the maturity date.
	  /// <para>
	  /// The maturity date is the start date of the indexed deposit.
	  /// In most cases, the maturity date is 2 days after the fixing date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.date.DaysAdjustment maturityDateOffset;
	  private readonly DaysAdjustment maturityDateOffset;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (builder.fixingDateOffset_Renamed == null && builder.fixingCalendar_Renamed != null && builder.maturityDateOffset_Renamed != null)
		{
		  int days = builder.maturityDateOffset_Renamed.Days;
		  HolidayCalendarId maturityCal = builder.maturityDateOffset_Renamed.Calendar;
		  if (maturityCal.combinedWith(builder.fixingCalendar_Renamed).Equals(maturityCal))
		  {
			builder.fixingDateOffset_Renamed = DaysAdjustment.ofBusinessDays(-days, maturityCal);
		  }
		  else
		  {
			builder.fixingDateOffset_Renamed = DaysAdjustment.ofBusinessDays(-days, maturityCal, BusinessDayAdjustment.of(BusinessDayConventions.PRECEDING, builder.fixingCalendar_Renamed));
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public LocalDate calculateMaturityFromFixing(LocalDate fixingDate, ReferenceData refData)
	  {
		// handle case where the input date is not a valid fixing date
		HolidayCalendar fixingCal = fixingCalendar.resolve(refData);
		LocalDate fixingBusinessDay = fixingCal.nextOrSame(fixingDate);
		// find the maturity date using the offset and calendar in DaysAdjustment
		return maturityDateOffset.adjust(fixingBusinessDay, refData);
	  }

	  public LocalDate calculateFixingFromMaturity(LocalDate maturityDate, ReferenceData refData)
	  {
		// handle case where the input date is not a valid maturity date
		LocalDate maturityBusinessDay = maturityDateCalendar().resolve(refData).nextOrSame(maturityDate);
		// find the fixing date iteratively
		HolidayCalendar fixingCal = fixingCalendar.resolve(refData);
		DateAdjuster maturityFromFixing = maturityDateOffset.resolve(refData);
		LocalDate fixingDate = maturityBusinessDay;
		while (fixingCal.isHoliday(fixingDate) || maturityFromFixing.adjust(fixingDate).isAfter(maturityBusinessDay))
		{
		  fixingDate = fixingDate.minusDays(1);
		}
		return fixingDate;
	  }

	  // finds the calendar of the maturity date
	  private HolidayCalendarId maturityDateCalendar()
	  {
		HolidayCalendarId cal = maturityDateOffset.ResultCalendar;
		return (cal == HolidayCalendarIds.NO_HOLIDAYS ? fixingCalendar : cal);
	  }

	  public System.Func<LocalDate, FxIndexObservation> resolve(ReferenceData refData)
	  {
		HolidayCalendar fixingCal = fixingCalendar.resolve(refData);
		DateAdjuster maturityAdj = maturityDateOffset.resolve(refData);
		return fixingDate => create(fixingDate, fixingCal, maturityAdj);
	  }

	  // creates an observation
	  private FxIndexObservation create(LocalDate fixingDate, HolidayCalendar fixingCal, DateAdjuster maturityAdjuster)
	  {

		LocalDate fixingBusinessDay = fixingCal.nextOrSame(fixingDate);
		LocalDate maturityDate = maturityAdjuster.adjust(fixingBusinessDay);
		return new FxIndexObservation(this, fixingDate, maturityDate);
	  }

	  //-------------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj is ImmutableFxIndex)
		{
		  return name.Equals(((ImmutableFxIndex) obj).name);
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
	  /// The meta-bean for {@code ImmutableFxIndex}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ImmutableFxIndex.Meta meta()
	  {
		return ImmutableFxIndex.Meta.INSTANCE;
	  }

	  static ImmutableFxIndex()
	  {
		MetaBean.register(ImmutableFxIndex.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ImmutableFxIndex.Builder builder()
	  {
		return new ImmutableFxIndex.Builder();
	  }

	  private ImmutableFxIndex(string name, CurrencyPair currencyPair, HolidayCalendarId fixingCalendar, DaysAdjustment fixingDateOffset, DaysAdjustment maturityDateOffset)
	  {
		JodaBeanUtils.notNull(name, "name");
		JodaBeanUtils.notNull(currencyPair, "currencyPair");
		JodaBeanUtils.notNull(fixingCalendar, "fixingCalendar");
		JodaBeanUtils.notNull(fixingDateOffset, "fixingDateOffset");
		JodaBeanUtils.notNull(maturityDateOffset, "maturityDateOffset");
		this.name = name;
		this.currencyPair = currencyPair;
		this.fixingCalendar = fixingCalendar;
		this.fixingDateOffset = fixingDateOffset;
		this.maturityDateOffset = maturityDateOffset;
	  }

	  public override ImmutableFxIndex.Meta metaBean()
	  {
		return ImmutableFxIndex.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the index name, such as 'EUR/GBP-ECB'. </summary>
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
	  /// Gets the currency pair.
	  /// <para>
	  /// An index defines an FX rate in a single direction, such as from EUR to USD.
	  /// This currency pair defines that direction.
	  /// </para>
	  /// <para>
	  /// In most cases, the same index can be used to convert in both directions
	  /// by taking the rate or the reciprocal as necessary.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurrencyPair CurrencyPair
	  {
		  get
		  {
			return currencyPair;
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
	  /// Gets the adjustment applied to the maturity date to obtain the fixing date.
	  /// <para>
	  /// The maturity date is the start date of the indexed deposit.
	  /// In most cases, the fixing date is 2 days before the maturity date.
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
	  /// Gets the adjustment applied to the fixing date to obtain the maturity date.
	  /// <para>
	  /// The maturity date is the start date of the indexed deposit.
	  /// In most cases, the maturity date is 2 days after the fixing date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DaysAdjustment MaturityDateOffset
	  {
		  get
		  {
			return maturityDateOffset;
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
	  /// The meta-bean for {@code ImmutableFxIndex}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(ImmutableFxIndex), typeof(string));
			  currencyPair_Renamed = DirectMetaProperty.ofImmutable(this, "currencyPair", typeof(ImmutableFxIndex), typeof(CurrencyPair));
			  fixingCalendar_Renamed = DirectMetaProperty.ofImmutable(this, "fixingCalendar", typeof(ImmutableFxIndex), typeof(HolidayCalendarId));
			  fixingDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "fixingDateOffset", typeof(ImmutableFxIndex), typeof(DaysAdjustment));
			  maturityDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "maturityDateOffset", typeof(ImmutableFxIndex), typeof(DaysAdjustment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "currencyPair", "fixingCalendar", "fixingDateOffset", "maturityDateOffset");
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
		/// The meta-property for the {@code currencyPair} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurrencyPair> currencyPair_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingCalendar} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<HolidayCalendarId> fixingCalendar_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> fixingDateOffset_Renamed;
		/// <summary>
		/// The meta-property for the {@code maturityDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> maturityDateOffset_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "currencyPair", "fixingCalendar", "fixingDateOffset", "maturityDateOffset");
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
			case 1005147787: // currencyPair
			  return currencyPair_Renamed;
			case 394230283: // fixingCalendar
			  return fixingCalendar_Renamed;
			case 873743726: // fixingDateOffset
			  return fixingDateOffset_Renamed;
			case 1574797394: // maturityDateOffset
			  return maturityDateOffset_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ImmutableFxIndex.Builder builder()
		{
		  return new ImmutableFxIndex.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ImmutableFxIndex);
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
		/// The meta-property for the {@code currencyPair} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurrencyPair> currencyPair()
		{
		  return currencyPair_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixingCalendar} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<HolidayCalendarId> fixingCalendar()
		{
		  return fixingCalendar_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixingDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> fixingDateOffset()
		{
		  return fixingDateOffset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code maturityDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> maturityDateOffset()
		{
		  return maturityDateOffset_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return ((ImmutableFxIndex) bean).Name;
			case 1005147787: // currencyPair
			  return ((ImmutableFxIndex) bean).CurrencyPair;
			case 394230283: // fixingCalendar
			  return ((ImmutableFxIndex) bean).FixingCalendar;
			case 873743726: // fixingDateOffset
			  return ((ImmutableFxIndex) bean).FixingDateOffset;
			case 1574797394: // maturityDateOffset
			  return ((ImmutableFxIndex) bean).MaturityDateOffset;
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
	  /// The bean-builder for {@code ImmutableFxIndex}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ImmutableFxIndex>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal string name_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurrencyPair currencyPair_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal HolidayCalendarId fixingCalendar_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment fixingDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment maturityDateOffset_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ImmutableFxIndex beanToCopy)
		{
		  this.name_Renamed = beanToCopy.Name;
		  this.currencyPair_Renamed = beanToCopy.CurrencyPair;
		  this.fixingCalendar_Renamed = beanToCopy.FixingCalendar;
		  this.fixingDateOffset_Renamed = beanToCopy.FixingDateOffset;
		  this.maturityDateOffset_Renamed = beanToCopy.MaturityDateOffset;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name_Renamed;
			case 1005147787: // currencyPair
			  return currencyPair_Renamed;
			case 394230283: // fixingCalendar
			  return fixingCalendar_Renamed;
			case 873743726: // fixingDateOffset
			  return fixingDateOffset_Renamed;
			case 1574797394: // maturityDateOffset
			  return maturityDateOffset_Renamed;
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
			case 1005147787: // currencyPair
			  this.currencyPair_Renamed = (CurrencyPair) newValue;
			  break;
			case 394230283: // fixingCalendar
			  this.fixingCalendar_Renamed = (HolidayCalendarId) newValue;
			  break;
			case 873743726: // fixingDateOffset
			  this.fixingDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case 1574797394: // maturityDateOffset
			  this.maturityDateOffset_Renamed = (DaysAdjustment) newValue;
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

		public override ImmutableFxIndex build()
		{
		  preBuild(this);
		  return new ImmutableFxIndex(name_Renamed, currencyPair_Renamed, fixingCalendar_Renamed, fixingDateOffset_Renamed, maturityDateOffset_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the index name, such as 'EUR/GBP-ECB'. </summary>
		/// <param name="name">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder name(string name)
		{
		  JodaBeanUtils.notNull(name, "name");
		  this.name_Renamed = name;
		  return this;
		}

		/// <summary>
		/// Sets the currency pair.
		/// <para>
		/// An index defines an FX rate in a single direction, such as from EUR to USD.
		/// This currency pair defines that direction.
		/// </para>
		/// <para>
		/// In most cases, the same index can be used to convert in both directions
		/// by taking the rate or the reciprocal as necessary.
		/// </para>
		/// </summary>
		/// <param name="currencyPair">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder currencyPair(CurrencyPair currencyPair)
		{
		  JodaBeanUtils.notNull(currencyPair, "currencyPair");
		  this.currencyPair_Renamed = currencyPair;
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
		/// Sets the adjustment applied to the maturity date to obtain the fixing date.
		/// <para>
		/// The maturity date is the start date of the indexed deposit.
		/// In most cases, the fixing date is 2 days before the maturity date.
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
		/// Sets the adjustment applied to the fixing date to obtain the maturity date.
		/// <para>
		/// The maturity date is the start date of the indexed deposit.
		/// In most cases, the maturity date is 2 days after the fixing date.
		/// </para>
		/// </summary>
		/// <param name="maturityDateOffset">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder maturityDateOffset(DaysAdjustment maturityDateOffset)
		{
		  JodaBeanUtils.notNull(maturityDateOffset, "maturityDateOffset");
		  this.maturityDateOffset_Renamed = maturityDateOffset;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(192);
		  buf.Append("ImmutableFxIndex.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("currencyPair").Append('=').Append(JodaBeanUtils.ToString(currencyPair_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingCalendar").Append('=').Append(JodaBeanUtils.ToString(fixingCalendar_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingDateOffset").Append('=').Append(JodaBeanUtils.ToString(fixingDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("maturityDateOffset").Append('=').Append(JodaBeanUtils.ToString(maturityDateOffset_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}