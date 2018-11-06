using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.deposit
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Resolvable = com.opengamma.strata.basics.Resolvable;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DateAdjuster = com.opengamma.strata.basics.date.DateAdjuster;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// A term deposit.
	/// <para>
	/// A term deposit is a financial instrument that provides a fixed rate of interest on
	/// an amount for a specific term.
	/// For example, investing GBP 1,000 for 3 months at a 1% interest rate.
	/// </para>
	/// <para>
	/// The instrument has two payments, one at the start date and one at the end date.
	/// For example, investing  GBP 1,000 for 3 months implies an initial payment to the counterparty
	/// of GBP 1,000 and a final payment from the counterparty of GBP 1,000 plus interest.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class TermDeposit implements com.opengamma.strata.product.Product, com.opengamma.strata.basics.Resolvable<ResolvedTermDeposit>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class TermDeposit : Product, Resolvable<ResolvedTermDeposit>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.common.BuySell buySell;
		private readonly BuySell buySell;
	  /// <summary>
	  /// The primary currency.
	  /// <para>
	  /// This is the currency of the term deposit and the currency that payment is made in.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Currency currency;
	  private readonly Currency currency;
	  /// <summary>
	  /// The notional amount.
	  /// <para>
	  /// The notional represents the principal amount, and must be non-negative.
	  /// The currency of the notional is specified by {@code currency}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative") private final double notional;
	  private readonly double notional;
	  /// <summary>
	  /// The start date of the deposit.
	  /// <para>
	  /// Interest accrues from this date.
	  /// This date is typically set to be a valid business day.
	  /// Optionally, the {@code businessDayAdjustment} property may be set to provide a rule for adjustment.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate startDate;
	  private readonly LocalDate startDate;
	  /// <summary>
	  /// The end date of the deposit.
	  /// <para>
	  /// Interest accrues until this date.
	  /// This date is typically set to be a valid business day.
	  /// Optionally, the {@code businessDayAdjustment} property may be set to provide a rule for adjustment.
	  /// This date must be after the start date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate endDate;
	  private readonly LocalDate endDate;
	  /// <summary>
	  /// The business day adjustment to apply to the start and end date, optional.
	  /// <para>
	  /// The start and end date are typically defined as valid business days and thus
	  /// do not need to be adjusted. If this optional property is present, then the
	  /// start and end date will be adjusted as defined here.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.basics.date.BusinessDayAdjustment businessDayAdjustment;
	  private readonly BusinessDayAdjustment businessDayAdjustment;
	  /// <summary>
	  /// The day count convention.
	  /// <para>
	  /// This is used to convert dates to a numerical value.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DayCount dayCount;
	  private readonly DayCount dayCount;
	  /// <summary>
	  /// The fixed interest rate to be paid.
	  /// A 5% rate will be expressed as 0.05.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double rate;
	  private readonly double rate;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.inOrderNotEqual(startDate, endDate, "startDate", "endDate");
	  }

	  //-------------------------------------------------------------------------
	  public ImmutableSet<Currency> allCurrencies()
	  {
		return ImmutableSet.of(currency);
	  }

	  //-------------------------------------------------------------------------
	  public ResolvedTermDeposit resolve(ReferenceData refData)
	  {
		DateAdjuster bda = BusinessDayAdjustment.orElse(BusinessDayAdjustment.NONE).resolve(refData);
		LocalDate start = bda.adjust(startDate);
		LocalDate end = bda.adjust(endDate);
		double yearFraction = dayCount.yearFraction(start, end);
		return ResolvedTermDeposit.builder().startDate(start).endDate(end).yearFraction(yearFraction).currency(Currency).notional(buySell.normalize(notional)).rate(rate).build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code TermDeposit}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TermDeposit.Meta meta()
	  {
		return TermDeposit.Meta.INSTANCE;
	  }

	  static TermDeposit()
	  {
		MetaBean.register(TermDeposit.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static TermDeposit.Builder builder()
	  {
		return new TermDeposit.Builder();
	  }

	  private TermDeposit(BuySell buySell, Currency currency, double notional, LocalDate startDate, LocalDate endDate, BusinessDayAdjustment businessDayAdjustment, DayCount dayCount, double rate)
	  {
		JodaBeanUtils.notNull(buySell, "buySell");
		JodaBeanUtils.notNull(currency, "currency");
		ArgChecker.notNegative(notional, "notional");
		JodaBeanUtils.notNull(startDate, "startDate");
		JodaBeanUtils.notNull(endDate, "endDate");
		JodaBeanUtils.notNull(dayCount, "dayCount");
		this.buySell = buySell;
		this.currency = currency;
		this.notional = notional;
		this.startDate = startDate;
		this.endDate = endDate;
		this.businessDayAdjustment = businessDayAdjustment;
		this.dayCount = dayCount;
		this.rate = rate;
		validate();
	  }

	  public override TermDeposit.Meta metaBean()
	  {
		return TermDeposit.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets whether the term deposit is 'Buy' or 'Sell'.
	  /// <para>
	  /// A value of 'Buy' implies payment of the principal at the start date and receipt of the
	  /// principal plus interest at the end date. A value of 'Sell' implies the opposite.
	  /// In other words, 'Buy' refers to buying the service of "money storage", with the benefit
	  /// of receiving interest.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public BuySell BuySell
	  {
		  get
		  {
			return buySell;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the primary currency.
	  /// <para>
	  /// This is the currency of the term deposit and the currency that payment is made in.
	  /// </para>
	  /// </summary>
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
	  /// Gets the notional amount.
	  /// <para>
	  /// The notional represents the principal amount, and must be non-negative.
	  /// The currency of the notional is specified by {@code currency}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double Notional
	  {
		  get
		  {
			return notional;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the start date of the deposit.
	  /// <para>
	  /// Interest accrues from this date.
	  /// This date is typically set to be a valid business day.
	  /// Optionally, the {@code businessDayAdjustment} property may be set to provide a rule for adjustment.
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
	  /// Gets the end date of the deposit.
	  /// <para>
	  /// Interest accrues until this date.
	  /// This date is typically set to be a valid business day.
	  /// Optionally, the {@code businessDayAdjustment} property may be set to provide a rule for adjustment.
	  /// This date must be after the start date.
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
	  /// Gets the business day adjustment to apply to the start and end date, optional.
	  /// <para>
	  /// The start and end date are typically defined as valid business days and thus
	  /// do not need to be adjusted. If this optional property is present, then the
	  /// start and end date will be adjusted as defined here.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<BusinessDayAdjustment> BusinessDayAdjustment
	  {
		  get
		  {
			return Optional.ofNullable(businessDayAdjustment);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the day count convention.
	  /// <para>
	  /// This is used to convert dates to a numerical value.
	  /// </para>
	  /// </summary>
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
	  /// Gets the fixed interest rate to be paid.
	  /// A 5% rate will be expressed as 0.05. </summary>
	  /// <returns> the value of the property </returns>
	  public double Rate
	  {
		  get
		  {
			return rate;
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
		  TermDeposit other = (TermDeposit) obj;
		  return JodaBeanUtils.equal(buySell, other.buySell) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(notional, other.notional) && JodaBeanUtils.equal(startDate, other.startDate) && JodaBeanUtils.equal(endDate, other.endDate) && JodaBeanUtils.equal(businessDayAdjustment, other.businessDayAdjustment) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(rate, other.rate);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(buySell);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(notional);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(startDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(businessDayAdjustment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rate);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(288);
		buf.Append("TermDeposit{");
		buf.Append("buySell").Append('=').Append(buySell).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("notional").Append('=').Append(notional).Append(',').Append(' ');
		buf.Append("startDate").Append('=').Append(startDate).Append(',').Append(' ');
		buf.Append("endDate").Append('=').Append(endDate).Append(',').Append(' ');
		buf.Append("businessDayAdjustment").Append('=').Append(businessDayAdjustment).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("rate").Append('=').Append(JodaBeanUtils.ToString(rate));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code TermDeposit}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  buySell_Renamed = DirectMetaProperty.ofImmutable(this, "buySell", typeof(TermDeposit), typeof(BuySell));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(TermDeposit), typeof(Currency));
			  notional_Renamed = DirectMetaProperty.ofImmutable(this, "notional", typeof(TermDeposit), Double.TYPE);
			  startDate_Renamed = DirectMetaProperty.ofImmutable(this, "startDate", typeof(TermDeposit), typeof(LocalDate));
			  endDate_Renamed = DirectMetaProperty.ofImmutable(this, "endDate", typeof(TermDeposit), typeof(LocalDate));
			  businessDayAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "businessDayAdjustment", typeof(TermDeposit), typeof(BusinessDayAdjustment));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(TermDeposit), typeof(DayCount));
			  rate_Renamed = DirectMetaProperty.ofImmutable(this, "rate", typeof(TermDeposit), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "buySell", "currency", "notional", "startDate", "endDate", "businessDayAdjustment", "dayCount", "rate");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code buySell} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BuySell> buySell_Renamed;
		/// <summary>
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-property for the {@code notional} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> notional_Renamed;
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
		/// The meta-property for the {@code businessDayAdjustment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BusinessDayAdjustment> businessDayAdjustment_Renamed;
		/// <summary>
		/// The meta-property for the {@code dayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> dayCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code rate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> rate_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "buySell", "currency", "notional", "startDate", "endDate", "businessDayAdjustment", "dayCount", "rate");
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
			case 244977400: // buySell
			  return buySell_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case -1065319863: // businessDayAdjustment
			  return businessDayAdjustment_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 3493088: // rate
			  return rate_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override TermDeposit.Builder builder()
		{
		  return new TermDeposit.Builder();
		}

		public override Type beanType()
		{
		  return typeof(TermDeposit);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code buySell} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BuySell> buySell()
		{
		  return buySell_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code currency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> currency()
		{
		  return currency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code notional} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> notional()
		{
		  return notional_Renamed;
		}

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
		/// The meta-property for the {@code businessDayAdjustment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BusinessDayAdjustment> businessDayAdjustment()
		{
		  return businessDayAdjustment_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dayCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DayCount> dayCount()
		{
		  return dayCount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code rate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> rate()
		{
		  return rate_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 244977400: // buySell
			  return ((TermDeposit) bean).BuySell;
			case 575402001: // currency
			  return ((TermDeposit) bean).Currency;
			case 1585636160: // notional
			  return ((TermDeposit) bean).Notional;
			case -2129778896: // startDate
			  return ((TermDeposit) bean).StartDate;
			case -1607727319: // endDate
			  return ((TermDeposit) bean).EndDate;
			case -1065319863: // businessDayAdjustment
			  return ((TermDeposit) bean).businessDayAdjustment;
			case 1905311443: // dayCount
			  return ((TermDeposit) bean).DayCount;
			case 3493088: // rate
			  return ((TermDeposit) bean).Rate;
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
	  /// The bean-builder for {@code TermDeposit}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<TermDeposit>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BuySell buySell_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Currency currency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double notional_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate startDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate endDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BusinessDayAdjustment businessDayAdjustment_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount dayCount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double rate_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(TermDeposit beanToCopy)
		{
		  this.buySell_Renamed = beanToCopy.BuySell;
		  this.currency_Renamed = beanToCopy.Currency;
		  this.notional_Renamed = beanToCopy.Notional;
		  this.startDate_Renamed = beanToCopy.StartDate;
		  this.endDate_Renamed = beanToCopy.EndDate;
		  this.businessDayAdjustment_Renamed = beanToCopy.businessDayAdjustment;
		  this.dayCount_Renamed = beanToCopy.DayCount;
		  this.rate_Renamed = beanToCopy.Rate;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 244977400: // buySell
			  return buySell_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case -1065319863: // businessDayAdjustment
			  return businessDayAdjustment_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 3493088: // rate
			  return rate_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 244977400: // buySell
			  this.buySell_Renamed = (BuySell) newValue;
			  break;
			case 575402001: // currency
			  this.currency_Renamed = (Currency) newValue;
			  break;
			case 1585636160: // notional
			  this.notional_Renamed = (double?) newValue.Value;
			  break;
			case -2129778896: // startDate
			  this.startDate_Renamed = (LocalDate) newValue;
			  break;
			case -1607727319: // endDate
			  this.endDate_Renamed = (LocalDate) newValue;
			  break;
			case -1065319863: // businessDayAdjustment
			  this.businessDayAdjustment_Renamed = (BusinessDayAdjustment) newValue;
			  break;
			case 1905311443: // dayCount
			  this.dayCount_Renamed = (DayCount) newValue;
			  break;
			case 3493088: // rate
			  this.rate_Renamed = (double?) newValue.Value;
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

		public override TermDeposit build()
		{
		  return new TermDeposit(buySell_Renamed, currency_Renamed, notional_Renamed, startDate_Renamed, endDate_Renamed, businessDayAdjustment_Renamed, dayCount_Renamed, rate_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets whether the term deposit is 'Buy' or 'Sell'.
		/// <para>
		/// A value of 'Buy' implies payment of the principal at the start date and receipt of the
		/// principal plus interest at the end date. A value of 'Sell' implies the opposite.
		/// In other words, 'Buy' refers to buying the service of "money storage", with the benefit
		/// of receiving interest.
		/// </para>
		/// </summary>
		/// <param name="buySell">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder buySell(BuySell buySell)
		{
		  JodaBeanUtils.notNull(buySell, "buySell");
		  this.buySell_Renamed = buySell;
		  return this;
		}

		/// <summary>
		/// Sets the primary currency.
		/// <para>
		/// This is the currency of the term deposit and the currency that payment is made in.
		/// </para>
		/// </summary>
		/// <param name="currency">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder currency(Currency currency)
		{
		  JodaBeanUtils.notNull(currency, "currency");
		  this.currency_Renamed = currency;
		  return this;
		}

		/// <summary>
		/// Sets the notional amount.
		/// <para>
		/// The notional represents the principal amount, and must be non-negative.
		/// The currency of the notional is specified by {@code currency}.
		/// </para>
		/// </summary>
		/// <param name="notional">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder notional(double notional)
		{
		  ArgChecker.notNegative(notional, "notional");
		  this.notional_Renamed = notional;
		  return this;
		}

		/// <summary>
		/// Sets the start date of the deposit.
		/// <para>
		/// Interest accrues from this date.
		/// This date is typically set to be a valid business day.
		/// Optionally, the {@code businessDayAdjustment} property may be set to provide a rule for adjustment.
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
		/// Sets the end date of the deposit.
		/// <para>
		/// Interest accrues until this date.
		/// This date is typically set to be a valid business day.
		/// Optionally, the {@code businessDayAdjustment} property may be set to provide a rule for adjustment.
		/// This date must be after the start date.
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
		/// Sets the business day adjustment to apply to the start and end date, optional.
		/// <para>
		/// The start and end date are typically defined as valid business days and thus
		/// do not need to be adjusted. If this optional property is present, then the
		/// start and end date will be adjusted as defined here.
		/// </para>
		/// </summary>
		/// <param name="businessDayAdjustment">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder businessDayAdjustment(BusinessDayAdjustment businessDayAdjustment)
		{
		  this.businessDayAdjustment_Renamed = businessDayAdjustment;
		  return this;
		}

		/// <summary>
		/// Sets the day count convention.
		/// <para>
		/// This is used to convert dates to a numerical value.
		/// </para>
		/// </summary>
		/// <param name="dayCount">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dayCount(DayCount dayCount)
		{
		  JodaBeanUtils.notNull(dayCount, "dayCount");
		  this.dayCount_Renamed = dayCount;
		  return this;
		}

		/// <summary>
		/// Sets the fixed interest rate to be paid.
		/// A 5% rate will be expressed as 0.05. </summary>
		/// <param name="rate">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder rate(double rate)
		{
		  this.rate_Renamed = rate;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(288);
		  buf.Append("TermDeposit.Builder{");
		  buf.Append("buySell").Append('=').Append(JodaBeanUtils.ToString(buySell_Renamed)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("notional").Append('=').Append(JodaBeanUtils.ToString(notional_Renamed)).Append(',').Append(' ');
		  buf.Append("startDate").Append('=').Append(JodaBeanUtils.ToString(startDate_Renamed)).Append(',').Append(' ');
		  buf.Append("endDate").Append('=').Append(JodaBeanUtils.ToString(endDate_Renamed)).Append(',').Append(' ');
		  buf.Append("businessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(businessDayAdjustment_Renamed)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("rate").Append('=').Append(JodaBeanUtils.ToString(rate_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}