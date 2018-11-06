using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.credit
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

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// A period over which a fixed coupon is paid.
	/// <para>
	/// A single payment period within a CDS, <seealso cref="ResolvedCds"/>.
	/// The payments of the CDS consist of periodic coupon payments and protection payment on default.
	/// This class represents a single payment of the periodic payments.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class CreditCouponPaymentPeriod implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CreditCouponPaymentPeriod : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Currency currency;
		private readonly Currency currency;
	  /// <summary>
	  /// The notional amount, must be positive.
	  /// <para>
	  /// The notional amount applicable during the period.
	  /// The currency of the notional is specified by {@code currency}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative") private final double notional;
	  private readonly double notional;
	  /// <summary>
	  /// The start date of the accrual period.
	  /// <para>
	  /// This is the first accrual date in the period.
	  /// If the schedule adjusts for business days, then this is the adjusted date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate startDate;
	  private readonly LocalDate startDate;
	  /// <summary>
	  /// The end date of the accrual period.
	  /// <para>
	  /// This is the last accrual date in the period.
	  /// If the schedule adjusts for business days, then this is the adjusted date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate endDate;
	  private readonly LocalDate endDate;
	  /// <summary>
	  /// The unadjusted start date.
	  /// <para>
	  /// The start date before any business day adjustment is applied.
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
	  /// The end date before any business day adjustment is applied.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the end date if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate unadjustedEndDate;
	  private readonly LocalDate unadjustedEndDate;
	  /// <summary>
	  /// The effective protection start date of the period.
	  /// <para>
	  /// This is the first date in the protection period associated with the payment period.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate effectiveStartDate;
	  private readonly LocalDate effectiveStartDate;
	  /// <summary>
	  /// The effective protection end date of the period.
	  /// <para>
	  /// This is the last date in the protection period associated with the payment period.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate effectiveEndDate;
	  private readonly LocalDate effectiveEndDate;
	  /// <summary>
	  /// The payment date.
	  /// <para>
	  /// The fixed rate is paid on this date.
	  /// This is not necessarily the same as {@code endDate}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate paymentDate;
	  private readonly LocalDate paymentDate;
	  /// <summary>
	  /// The fixed coupon rate.
	  /// <para>
	  /// The single payment is based on this fixed coupon rate.
	  /// The coupon must be represented in fraction.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double fixedRate;
	  private readonly double fixedRate;
	  /// <summary>
	  /// The year fraction that the accrual period represents.
	  /// <para>
	  /// The year fraction of a period is based on {@code startDate} and {@code endDate}.
	  /// The value is usually calculated using a specific <seealso cref="DayCount"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative") private final double yearFraction;
	  private readonly double yearFraction;

	  //-------------------------------------------------------------------------
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
	  // does this period contain the date
	  internal bool contains(LocalDate date)
	  {
		return !date.isBefore(startDate) && date.isBefore(endDate);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CreditCouponPaymentPeriod}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CreditCouponPaymentPeriod.Meta meta()
	  {
		return CreditCouponPaymentPeriod.Meta.INSTANCE;
	  }

	  static CreditCouponPaymentPeriod()
	  {
		MetaBean.register(CreditCouponPaymentPeriod.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static CreditCouponPaymentPeriod.Builder builder()
	  {
		return new CreditCouponPaymentPeriod.Builder();
	  }

	  private CreditCouponPaymentPeriod(Currency currency, double notional, LocalDate startDate, LocalDate endDate, LocalDate unadjustedStartDate, LocalDate unadjustedEndDate, LocalDate effectiveStartDate, LocalDate effectiveEndDate, LocalDate paymentDate, double fixedRate, double yearFraction)
	  {
		JodaBeanUtils.notNull(currency, "currency");
		ArgChecker.notNegative(notional, "notional");
		JodaBeanUtils.notNull(startDate, "startDate");
		JodaBeanUtils.notNull(endDate, "endDate");
		JodaBeanUtils.notNull(unadjustedStartDate, "unadjustedStartDate");
		JodaBeanUtils.notNull(unadjustedEndDate, "unadjustedEndDate");
		JodaBeanUtils.notNull(effectiveStartDate, "effectiveStartDate");
		JodaBeanUtils.notNull(effectiveEndDate, "effectiveEndDate");
		JodaBeanUtils.notNull(paymentDate, "paymentDate");
		ArgChecker.notNegative(yearFraction, "yearFraction");
		this.currency = currency;
		this.notional = notional;
		this.startDate = startDate;
		this.endDate = endDate;
		this.unadjustedStartDate = unadjustedStartDate;
		this.unadjustedEndDate = unadjustedEndDate;
		this.effectiveStartDate = effectiveStartDate;
		this.effectiveEndDate = effectiveEndDate;
		this.paymentDate = paymentDate;
		this.fixedRate = fixedRate;
		this.yearFraction = yearFraction;
	  }

	  public override CreditCouponPaymentPeriod.Meta metaBean()
	  {
		return CreditCouponPaymentPeriod.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the primary currency of the payment period.
	  /// <para>
	  /// The amounts of the notional are usually expressed in terms of this currency,
	  /// however they can be converted from amounts in a different currency.
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
	  /// Gets the notional amount, must be positive.
	  /// <para>
	  /// The notional amount applicable during the period.
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
	  /// Gets the start date of the accrual period.
	  /// <para>
	  /// This is the first accrual date in the period.
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
	  /// Gets the end date of the accrual period.
	  /// <para>
	  /// This is the last accrual date in the period.
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
	  /// The start date before any business day adjustment is applied.
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
	  /// The end date before any business day adjustment is applied.
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
	  /// Gets the effective protection start date of the period.
	  /// <para>
	  /// This is the first date in the protection period associated with the payment period.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate EffectiveStartDate
	  {
		  get
		  {
			return effectiveStartDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the effective protection end date of the period.
	  /// <para>
	  /// This is the last date in the protection period associated with the payment period.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate EffectiveEndDate
	  {
		  get
		  {
			return effectiveEndDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the payment date.
	  /// <para>
	  /// The fixed rate is paid on this date.
	  /// This is not necessarily the same as {@code endDate}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate PaymentDate
	  {
		  get
		  {
			return paymentDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the fixed coupon rate.
	  /// <para>
	  /// The single payment is based on this fixed coupon rate.
	  /// The coupon must be represented in fraction.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double FixedRate
	  {
		  get
		  {
			return fixedRate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the year fraction that the accrual period represents.
	  /// <para>
	  /// The year fraction of a period is based on {@code startDate} and {@code endDate}.
	  /// The value is usually calculated using a specific <seealso cref="DayCount"/>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double YearFraction
	  {
		  get
		  {
			return yearFraction;
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
		  CreditCouponPaymentPeriod other = (CreditCouponPaymentPeriod) obj;
		  return JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(notional, other.notional) && JodaBeanUtils.equal(startDate, other.startDate) && JodaBeanUtils.equal(endDate, other.endDate) && JodaBeanUtils.equal(unadjustedStartDate, other.unadjustedStartDate) && JodaBeanUtils.equal(unadjustedEndDate, other.unadjustedEndDate) && JodaBeanUtils.equal(effectiveStartDate, other.effectiveStartDate) && JodaBeanUtils.equal(effectiveEndDate, other.effectiveEndDate) && JodaBeanUtils.equal(paymentDate, other.paymentDate) && JodaBeanUtils.equal(fixedRate, other.fixedRate) && JodaBeanUtils.equal(yearFraction, other.yearFraction);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(notional);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(startDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(unadjustedStartDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(unadjustedEndDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(effectiveStartDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(effectiveEndDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixedRate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(yearFraction);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(384);
		buf.Append("CreditCouponPaymentPeriod{");
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("notional").Append('=').Append(notional).Append(',').Append(' ');
		buf.Append("startDate").Append('=').Append(startDate).Append(',').Append(' ');
		buf.Append("endDate").Append('=').Append(endDate).Append(',').Append(' ');
		buf.Append("unadjustedStartDate").Append('=').Append(unadjustedStartDate).Append(',').Append(' ');
		buf.Append("unadjustedEndDate").Append('=').Append(unadjustedEndDate).Append(',').Append(' ');
		buf.Append("effectiveStartDate").Append('=').Append(effectiveStartDate).Append(',').Append(' ');
		buf.Append("effectiveEndDate").Append('=').Append(effectiveEndDate).Append(',').Append(' ');
		buf.Append("paymentDate").Append('=').Append(paymentDate).Append(',').Append(' ');
		buf.Append("fixedRate").Append('=').Append(fixedRate).Append(',').Append(' ');
		buf.Append("yearFraction").Append('=').Append(JodaBeanUtils.ToString(yearFraction));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CreditCouponPaymentPeriod}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(CreditCouponPaymentPeriod), typeof(Currency));
			  notional_Renamed = DirectMetaProperty.ofImmutable(this, "notional", typeof(CreditCouponPaymentPeriod), Double.TYPE);
			  startDate_Renamed = DirectMetaProperty.ofImmutable(this, "startDate", typeof(CreditCouponPaymentPeriod), typeof(LocalDate));
			  endDate_Renamed = DirectMetaProperty.ofImmutable(this, "endDate", typeof(CreditCouponPaymentPeriod), typeof(LocalDate));
			  unadjustedStartDate_Renamed = DirectMetaProperty.ofImmutable(this, "unadjustedStartDate", typeof(CreditCouponPaymentPeriod), typeof(LocalDate));
			  unadjustedEndDate_Renamed = DirectMetaProperty.ofImmutable(this, "unadjustedEndDate", typeof(CreditCouponPaymentPeriod), typeof(LocalDate));
			  effectiveStartDate_Renamed = DirectMetaProperty.ofImmutable(this, "effectiveStartDate", typeof(CreditCouponPaymentPeriod), typeof(LocalDate));
			  effectiveEndDate_Renamed = DirectMetaProperty.ofImmutable(this, "effectiveEndDate", typeof(CreditCouponPaymentPeriod), typeof(LocalDate));
			  paymentDate_Renamed = DirectMetaProperty.ofImmutable(this, "paymentDate", typeof(CreditCouponPaymentPeriod), typeof(LocalDate));
			  fixedRate_Renamed = DirectMetaProperty.ofImmutable(this, "fixedRate", typeof(CreditCouponPaymentPeriod), Double.TYPE);
			  yearFraction_Renamed = DirectMetaProperty.ofImmutable(this, "yearFraction", typeof(CreditCouponPaymentPeriod), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "currency", "notional", "startDate", "endDate", "unadjustedStartDate", "unadjustedEndDate", "effectiveStartDate", "effectiveEndDate", "paymentDate", "fixedRate", "yearFraction");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

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
		/// The meta-property for the {@code effectiveStartDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> effectiveStartDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code effectiveEndDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> effectiveEndDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> paymentDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixedRate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> fixedRate_Renamed;
		/// <summary>
		/// The meta-property for the {@code yearFraction} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> yearFraction_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "currency", "notional", "startDate", "endDate", "unadjustedStartDate", "unadjustedEndDate", "effectiveStartDate", "effectiveEndDate", "paymentDate", "fixedRate", "yearFraction");
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
			case 575402001: // currency
			  return currency_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case 1457691881: // unadjustedStartDate
			  return unadjustedStartDate_Renamed;
			case 31758114: // unadjustedEndDate
			  return unadjustedEndDate_Renamed;
			case -1815017143: // effectiveStartDate
			  return effectiveStartDate_Renamed;
			case -566060158: // effectiveEndDate
			  return effectiveEndDate_Renamed;
			case -1540873516: // paymentDate
			  return paymentDate_Renamed;
			case 747425396: // fixedRate
			  return fixedRate_Renamed;
			case -1731780257: // yearFraction
			  return yearFraction_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override CreditCouponPaymentPeriod.Builder builder()
		{
		  return new CreditCouponPaymentPeriod.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CreditCouponPaymentPeriod);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
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

		/// <summary>
		/// The meta-property for the {@code effectiveStartDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> effectiveStartDate()
		{
		  return effectiveStartDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code effectiveEndDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> effectiveEndDate()
		{
		  return effectiveEndDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code paymentDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> paymentDate()
		{
		  return paymentDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixedRate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> fixedRate()
		{
		  return fixedRate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code yearFraction} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> yearFraction()
		{
		  return yearFraction_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 575402001: // currency
			  return ((CreditCouponPaymentPeriod) bean).Currency;
			case 1585636160: // notional
			  return ((CreditCouponPaymentPeriod) bean).Notional;
			case -2129778896: // startDate
			  return ((CreditCouponPaymentPeriod) bean).StartDate;
			case -1607727319: // endDate
			  return ((CreditCouponPaymentPeriod) bean).EndDate;
			case 1457691881: // unadjustedStartDate
			  return ((CreditCouponPaymentPeriod) bean).UnadjustedStartDate;
			case 31758114: // unadjustedEndDate
			  return ((CreditCouponPaymentPeriod) bean).UnadjustedEndDate;
			case -1815017143: // effectiveStartDate
			  return ((CreditCouponPaymentPeriod) bean).EffectiveStartDate;
			case -566060158: // effectiveEndDate
			  return ((CreditCouponPaymentPeriod) bean).EffectiveEndDate;
			case -1540873516: // paymentDate
			  return ((CreditCouponPaymentPeriod) bean).PaymentDate;
			case 747425396: // fixedRate
			  return ((CreditCouponPaymentPeriod) bean).FixedRate;
			case -1731780257: // yearFraction
			  return ((CreditCouponPaymentPeriod) bean).YearFraction;
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
	  /// The bean-builder for {@code CreditCouponPaymentPeriod}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<CreditCouponPaymentPeriod>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Currency currency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double notional_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate startDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate endDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate unadjustedStartDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate unadjustedEndDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate effectiveStartDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate effectiveEndDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate paymentDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double fixedRate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double yearFraction_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(CreditCouponPaymentPeriod beanToCopy)
		{
		  this.currency_Renamed = beanToCopy.Currency;
		  this.notional_Renamed = beanToCopy.Notional;
		  this.startDate_Renamed = beanToCopy.StartDate;
		  this.endDate_Renamed = beanToCopy.EndDate;
		  this.unadjustedStartDate_Renamed = beanToCopy.UnadjustedStartDate;
		  this.unadjustedEndDate_Renamed = beanToCopy.UnadjustedEndDate;
		  this.effectiveStartDate_Renamed = beanToCopy.EffectiveStartDate;
		  this.effectiveEndDate_Renamed = beanToCopy.EffectiveEndDate;
		  this.paymentDate_Renamed = beanToCopy.PaymentDate;
		  this.fixedRate_Renamed = beanToCopy.FixedRate;
		  this.yearFraction_Renamed = beanToCopy.YearFraction;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 575402001: // currency
			  return currency_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case 1457691881: // unadjustedStartDate
			  return unadjustedStartDate_Renamed;
			case 31758114: // unadjustedEndDate
			  return unadjustedEndDate_Renamed;
			case -1815017143: // effectiveStartDate
			  return effectiveStartDate_Renamed;
			case -566060158: // effectiveEndDate
			  return effectiveEndDate_Renamed;
			case -1540873516: // paymentDate
			  return paymentDate_Renamed;
			case 747425396: // fixedRate
			  return fixedRate_Renamed;
			case -1731780257: // yearFraction
			  return yearFraction_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
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
			case 1457691881: // unadjustedStartDate
			  this.unadjustedStartDate_Renamed = (LocalDate) newValue;
			  break;
			case 31758114: // unadjustedEndDate
			  this.unadjustedEndDate_Renamed = (LocalDate) newValue;
			  break;
			case -1815017143: // effectiveStartDate
			  this.effectiveStartDate_Renamed = (LocalDate) newValue;
			  break;
			case -566060158: // effectiveEndDate
			  this.effectiveEndDate_Renamed = (LocalDate) newValue;
			  break;
			case -1540873516: // paymentDate
			  this.paymentDate_Renamed = (LocalDate) newValue;
			  break;
			case 747425396: // fixedRate
			  this.fixedRate_Renamed = (double?) newValue.Value;
			  break;
			case -1731780257: // yearFraction
			  this.yearFraction_Renamed = (double?) newValue.Value;
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

		public override CreditCouponPaymentPeriod build()
		{
		  preBuild(this);
		  return new CreditCouponPaymentPeriod(currency_Renamed, notional_Renamed, startDate_Renamed, endDate_Renamed, unadjustedStartDate_Renamed, unadjustedEndDate_Renamed, effectiveStartDate_Renamed, effectiveEndDate_Renamed, paymentDate_Renamed, fixedRate_Renamed, yearFraction_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the primary currency of the payment period.
		/// <para>
		/// The amounts of the notional are usually expressed in terms of this currency,
		/// however they can be converted from amounts in a different currency.
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
		/// Sets the notional amount, must be positive.
		/// <para>
		/// The notional amount applicable during the period.
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
		/// Sets the start date of the accrual period.
		/// <para>
		/// This is the first accrual date in the period.
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
		/// Sets the end date of the accrual period.
		/// <para>
		/// This is the last accrual date in the period.
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
		/// The start date before any business day adjustment is applied.
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
		/// The end date before any business day adjustment is applied.
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

		/// <summary>
		/// Sets the effective protection start date of the period.
		/// <para>
		/// This is the first date in the protection period associated with the payment period.
		/// </para>
		/// </summary>
		/// <param name="effectiveStartDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder effectiveStartDate(LocalDate effectiveStartDate)
		{
		  JodaBeanUtils.notNull(effectiveStartDate, "effectiveStartDate");
		  this.effectiveStartDate_Renamed = effectiveStartDate;
		  return this;
		}

		/// <summary>
		/// Sets the effective protection end date of the period.
		/// <para>
		/// This is the last date in the protection period associated with the payment period.
		/// </para>
		/// </summary>
		/// <param name="effectiveEndDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder effectiveEndDate(LocalDate effectiveEndDate)
		{
		  JodaBeanUtils.notNull(effectiveEndDate, "effectiveEndDate");
		  this.effectiveEndDate_Renamed = effectiveEndDate;
		  return this;
		}

		/// <summary>
		/// Sets the payment date.
		/// <para>
		/// The fixed rate is paid on this date.
		/// This is not necessarily the same as {@code endDate}.
		/// </para>
		/// </summary>
		/// <param name="paymentDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentDate(LocalDate paymentDate)
		{
		  JodaBeanUtils.notNull(paymentDate, "paymentDate");
		  this.paymentDate_Renamed = paymentDate;
		  return this;
		}

		/// <summary>
		/// Sets the fixed coupon rate.
		/// <para>
		/// The single payment is based on this fixed coupon rate.
		/// The coupon must be represented in fraction.
		/// </para>
		/// </summary>
		/// <param name="fixedRate">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixedRate(double fixedRate)
		{
		  this.fixedRate_Renamed = fixedRate;
		  return this;
		}

		/// <summary>
		/// Sets the year fraction that the accrual period represents.
		/// <para>
		/// The year fraction of a period is based on {@code startDate} and {@code endDate}.
		/// The value is usually calculated using a specific <seealso cref="DayCount"/>.
		/// </para>
		/// </summary>
		/// <param name="yearFraction">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder yearFraction(double yearFraction)
		{
		  ArgChecker.notNegative(yearFraction, "yearFraction");
		  this.yearFraction_Renamed = yearFraction;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(384);
		  buf.Append("CreditCouponPaymentPeriod.Builder{");
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("notional").Append('=').Append(JodaBeanUtils.ToString(notional_Renamed)).Append(',').Append(' ');
		  buf.Append("startDate").Append('=').Append(JodaBeanUtils.ToString(startDate_Renamed)).Append(',').Append(' ');
		  buf.Append("endDate").Append('=').Append(JodaBeanUtils.ToString(endDate_Renamed)).Append(',').Append(' ');
		  buf.Append("unadjustedStartDate").Append('=').Append(JodaBeanUtils.ToString(unadjustedStartDate_Renamed)).Append(',').Append(' ');
		  buf.Append("unadjustedEndDate").Append('=').Append(JodaBeanUtils.ToString(unadjustedEndDate_Renamed)).Append(',').Append(' ');
		  buf.Append("effectiveStartDate").Append('=').Append(JodaBeanUtils.ToString(effectiveStartDate_Renamed)).Append(',').Append(' ');
		  buf.Append("effectiveEndDate").Append('=').Append(JodaBeanUtils.ToString(effectiveEndDate_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentDate").Append('=').Append(JodaBeanUtils.ToString(paymentDate_Renamed)).Append(',').Append(' ');
		  buf.Append("fixedRate").Append('=').Append(JodaBeanUtils.ToString(fixedRate_Renamed)).Append(',').Append(' ');
		  buf.Append("yearFraction").Append('=').Append(JodaBeanUtils.ToString(yearFraction_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}