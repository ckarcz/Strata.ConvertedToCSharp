using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using InflationRateCalculation = com.opengamma.strata.product.swap.InflationRateCalculation;

	/// <summary>
	/// A security representing a capital indexed bond.
	/// <para>
	/// A capital indexed bond is a financial instrument that represents a stream of inflation-adjusted payments.
	/// 
	/// <h4>Price</h4>
	/// Strata uses <i>decimal prices</i> for bonds in the trade model, pricers and market data.
	/// For example, a price of 99.32% is represented in Strata by 0.9932.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class CapitalIndexedBondSecurity implements LegalEntitySecurity, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CapitalIndexedBondSecurity : LegalEntitySecurity, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.product.SecurityInfo info;
		private readonly SecurityInfo info;
	  /// <summary>
	  /// The currency that the bond is traded in.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.currency.Currency currency;
	  private readonly Currency currency;
	  /// <summary>
	  /// The notional amount, must be positive.
	  /// <para>
	  /// The notional expressed here must be positive.
	  /// The currency of the notional is specified by {@code currency}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegativeOrZero") private final double notional;
	  private readonly double notional;
	  /// <summary>
	  /// The accrual schedule.
	  /// <para>
	  /// This is used to define the accrual periods.
	  /// These are used directly or indirectly to determine other dates in the product.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.schedule.PeriodicSchedule accrualSchedule;
	  private readonly PeriodicSchedule accrualSchedule;
	  /// <summary>
	  /// The inflation rate calculation.
	  /// <para>
	  /// The reference index is interpolated index or monthly index.
	  /// Real coupons are represented by {@code gearing} in the calculation.
	  /// The price index value at the start of the bond is represented by {@code firstIndexValue} in the calculation.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.swap.InflationRateCalculation rateCalculation;
	  private readonly InflationRateCalculation rateCalculation;
	  /// <summary>
	  /// The day count convention applicable.
	  /// <para>
	  /// The conversion from dates to a numerical value is made based on this day count.
	  /// For the inflation-indexed bond, the day count convention is used to compute accrued interest.
	  /// </para>
	  /// <para>
	  /// Note that the year fraction of a coupon payment is computed based on the unadjusted
	  /// dates in the schedule.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DayCount dayCount;
	  private readonly DayCount dayCount;
	  /// <summary>
	  /// Yield convention.
	  /// <para>
	  /// The convention defines how to convert from yield to price and inversely.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final CapitalIndexedBondYieldConvention yieldConvention;
	  private readonly CapitalIndexedBondYieldConvention yieldConvention;
	  /// <summary>
	  /// The legal entity identifier.
	  /// <para>
	  /// This identifier is used for the legal entity that issues the bond.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.product.LegalEntityId legalEntityId;
	  private readonly LegalEntityId legalEntityId;
	  /// <summary>
	  /// The number of days between valuation date and settlement date.
	  /// <para>
	  /// This is used to compute clean price.
	  /// The clean price is the relative price to be paid at the standard settlement date in exchange for the bond.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DaysAdjustment settlementDateOffset;
	  private readonly DaysAdjustment settlementDateOffset;
	  /// <summary>
	  /// Ex-coupon period.
	  /// <para>
	  /// Some bonds trade ex-coupons before the coupon payment. The coupon is paid not to the
	  /// owner of the bond on the payment date but to the owner of the bond on the detachment date.
	  /// The difference between the two is the ex-coupon period (measured in days).
	  /// </para>
	  /// <para>
	  /// Because the detachment date is not after the coupon date, the number of days
	  /// stored in this field should be zero or negative.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DaysAdjustment exCouponPeriod;
	  private readonly DaysAdjustment exCouponPeriod;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.exCouponPeriod_Renamed = DaysAdjustment.NONE;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.isTrue(settlementDateOffset.Days >= 0, "The settlement date offset must be non-negative");
		ArgChecker.isTrue(exCouponPeriod.Days <= 0, "The ex-coupon period is measured from the payment date, thus the days must be non-positive");
		ArgChecker.isTrue(rateCalculation.FirstIndexValue.HasValue, "Rate calculation must specify first index value");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the first index value
	  /// <para>
	  /// This is the price index value at the start of the bond.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the first index value </returns>
	  public double FirstIndexValue
	  {
		  get
		  {
			return rateCalculation.FirstIndexValue.Value; // validated in constructor
		  }
	  }

	  public ImmutableSet<SecurityId> UnderlyingIds
	  {
		  get
		  {
			return ImmutableSet.of();
		  }
	  }

	  public CapitalIndexedBondSecurity withInfo(SecurityInfo info)
	  {
		return toBuilder().info(info).build();
	  }

	  //-------------------------------------------------------------------------
	  public CapitalIndexedBond createProduct(ReferenceData refData)
	  {
		return new CapitalIndexedBond(SecurityId, currency, notional, accrualSchedule, rateCalculation, dayCount, yieldConvention, legalEntityId, settlementDateOffset, exCouponPeriod);
	  }

	  public CapitalIndexedBondTrade createTrade(TradeInfo info, double quantity, double tradePrice, ReferenceData refData)
	  {
		return new CapitalIndexedBondTrade(info, createProduct(refData), quantity, tradePrice);
	  }

	  public CapitalIndexedBondPosition createPosition(PositionInfo positionInfo, double quantity, ReferenceData refData)
	  {
		return CapitalIndexedBondPosition.ofNet(positionInfo, createProduct(refData), quantity);
	  }

	  public CapitalIndexedBondPosition createPosition(PositionInfo positionInfo, double longQuantity, double shortQuantity, ReferenceData refData)
	  {

		return CapitalIndexedBondPosition.ofLongShort(positionInfo, createProduct(refData), longQuantity, shortQuantity);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CapitalIndexedBondSecurity}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CapitalIndexedBondSecurity.Meta meta()
	  {
		return CapitalIndexedBondSecurity.Meta.INSTANCE;
	  }

	  static CapitalIndexedBondSecurity()
	  {
		MetaBean.register(CapitalIndexedBondSecurity.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static CapitalIndexedBondSecurity.Builder builder()
	  {
		return new CapitalIndexedBondSecurity.Builder();
	  }

	  private CapitalIndexedBondSecurity(SecurityInfo info, Currency currency, double notional, PeriodicSchedule accrualSchedule, InflationRateCalculation rateCalculation, DayCount dayCount, CapitalIndexedBondYieldConvention yieldConvention, LegalEntityId legalEntityId, DaysAdjustment settlementDateOffset, DaysAdjustment exCouponPeriod)
	  {
		JodaBeanUtils.notNull(info, "info");
		JodaBeanUtils.notNull(currency, "currency");
		ArgChecker.notNegativeOrZero(notional, "notional");
		JodaBeanUtils.notNull(accrualSchedule, "accrualSchedule");
		JodaBeanUtils.notNull(rateCalculation, "rateCalculation");
		JodaBeanUtils.notNull(dayCount, "dayCount");
		JodaBeanUtils.notNull(yieldConvention, "yieldConvention");
		JodaBeanUtils.notNull(legalEntityId, "legalEntityId");
		JodaBeanUtils.notNull(settlementDateOffset, "settlementDateOffset");
		JodaBeanUtils.notNull(exCouponPeriod, "exCouponPeriod");
		this.info = info;
		this.currency = currency;
		this.notional = notional;
		this.accrualSchedule = accrualSchedule;
		this.rateCalculation = rateCalculation;
		this.dayCount = dayCount;
		this.yieldConvention = yieldConvention;
		this.legalEntityId = legalEntityId;
		this.settlementDateOffset = settlementDateOffset;
		this.exCouponPeriod = exCouponPeriod;
		validate();
	  }

	  public override CapitalIndexedBondSecurity.Meta metaBean()
	  {
		return CapitalIndexedBondSecurity.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the standard security information.
	  /// <para>
	  /// This includes the security identifier.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SecurityInfo Info
	  {
		  get
		  {
			return info;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency that the bond is traded in. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public override Currency Currency
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
	  /// The notional expressed here must be positive.
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
	  /// Gets the accrual schedule.
	  /// <para>
	  /// This is used to define the accrual periods.
	  /// These are used directly or indirectly to determine other dates in the product.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PeriodicSchedule AccrualSchedule
	  {
		  get
		  {
			return accrualSchedule;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the inflation rate calculation.
	  /// <para>
	  /// The reference index is interpolated index or monthly index.
	  /// Real coupons are represented by {@code gearing} in the calculation.
	  /// The price index value at the start of the bond is represented by {@code firstIndexValue} in the calculation.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public InflationRateCalculation RateCalculation
	  {
		  get
		  {
			return rateCalculation;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the day count convention applicable.
	  /// <para>
	  /// The conversion from dates to a numerical value is made based on this day count.
	  /// For the inflation-indexed bond, the day count convention is used to compute accrued interest.
	  /// </para>
	  /// <para>
	  /// Note that the year fraction of a coupon payment is computed based on the unadjusted
	  /// dates in the schedule.
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
	  /// Gets yield convention.
	  /// <para>
	  /// The convention defines how to convert from yield to price and inversely.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CapitalIndexedBondYieldConvention YieldConvention
	  {
		  get
		  {
			return yieldConvention;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the legal entity identifier.
	  /// <para>
	  /// This identifier is used for the legal entity that issues the bond.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LegalEntityId LegalEntityId
	  {
		  get
		  {
			return legalEntityId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of days between valuation date and settlement date.
	  /// <para>
	  /// This is used to compute clean price.
	  /// The clean price is the relative price to be paid at the standard settlement date in exchange for the bond.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DaysAdjustment SettlementDateOffset
	  {
		  get
		  {
			return settlementDateOffset;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets ex-coupon period.
	  /// <para>
	  /// Some bonds trade ex-coupons before the coupon payment. The coupon is paid not to the
	  /// owner of the bond on the payment date but to the owner of the bond on the detachment date.
	  /// The difference between the two is the ex-coupon period (measured in days).
	  /// </para>
	  /// <para>
	  /// Because the detachment date is not after the coupon date, the number of days
	  /// stored in this field should be zero or negative.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DaysAdjustment ExCouponPeriod
	  {
		  get
		  {
			return exCouponPeriod;
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
		  CapitalIndexedBondSecurity other = (CapitalIndexedBondSecurity) obj;
		  return JodaBeanUtils.equal(info, other.info) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(notional, other.notional) && JodaBeanUtils.equal(accrualSchedule, other.accrualSchedule) && JodaBeanUtils.equal(rateCalculation, other.rateCalculation) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(yieldConvention, other.yieldConvention) && JodaBeanUtils.equal(legalEntityId, other.legalEntityId) && JodaBeanUtils.equal(settlementDateOffset, other.settlementDateOffset) && JodaBeanUtils.equal(exCouponPeriod, other.exCouponPeriod);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(info);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(notional);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(accrualSchedule);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rateCalculation);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(yieldConvention);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(legalEntityId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(settlementDateOffset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(exCouponPeriod);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(352);
		buf.Append("CapitalIndexedBondSecurity{");
		buf.Append("info").Append('=').Append(info).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("notional").Append('=').Append(notional).Append(',').Append(' ');
		buf.Append("accrualSchedule").Append('=').Append(accrualSchedule).Append(',').Append(' ');
		buf.Append("rateCalculation").Append('=').Append(rateCalculation).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("yieldConvention").Append('=').Append(yieldConvention).Append(',').Append(' ');
		buf.Append("legalEntityId").Append('=').Append(legalEntityId).Append(',').Append(' ');
		buf.Append("settlementDateOffset").Append('=').Append(settlementDateOffset).Append(',').Append(' ');
		buf.Append("exCouponPeriod").Append('=').Append(JodaBeanUtils.ToString(exCouponPeriod));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CapitalIndexedBondSecurity}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  info_Renamed = DirectMetaProperty.ofImmutable(this, "info", typeof(CapitalIndexedBondSecurity), typeof(SecurityInfo));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(CapitalIndexedBondSecurity), typeof(Currency));
			  notional_Renamed = DirectMetaProperty.ofImmutable(this, "notional", typeof(CapitalIndexedBondSecurity), Double.TYPE);
			  accrualSchedule_Renamed = DirectMetaProperty.ofImmutable(this, "accrualSchedule", typeof(CapitalIndexedBondSecurity), typeof(PeriodicSchedule));
			  rateCalculation_Renamed = DirectMetaProperty.ofImmutable(this, "rateCalculation", typeof(CapitalIndexedBondSecurity), typeof(InflationRateCalculation));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(CapitalIndexedBondSecurity), typeof(DayCount));
			  yieldConvention_Renamed = DirectMetaProperty.ofImmutable(this, "yieldConvention", typeof(CapitalIndexedBondSecurity), typeof(CapitalIndexedBondYieldConvention));
			  legalEntityId_Renamed = DirectMetaProperty.ofImmutable(this, "legalEntityId", typeof(CapitalIndexedBondSecurity), typeof(LegalEntityId));
			  settlementDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "settlementDateOffset", typeof(CapitalIndexedBondSecurity), typeof(DaysAdjustment));
			  exCouponPeriod_Renamed = DirectMetaProperty.ofImmutable(this, "exCouponPeriod", typeof(CapitalIndexedBondSecurity), typeof(DaysAdjustment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "info", "currency", "notional", "accrualSchedule", "rateCalculation", "dayCount", "yieldConvention", "legalEntityId", "settlementDateOffset", "exCouponPeriod");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code info} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SecurityInfo> info_Renamed;
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
		/// The meta-property for the {@code accrualSchedule} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PeriodicSchedule> accrualSchedule_Renamed;
		/// <summary>
		/// The meta-property for the {@code rateCalculation} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<InflationRateCalculation> rateCalculation_Renamed;
		/// <summary>
		/// The meta-property for the {@code dayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> dayCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code yieldConvention} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CapitalIndexedBondYieldConvention> yieldConvention_Renamed;
		/// <summary>
		/// The meta-property for the {@code legalEntityId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LegalEntityId> legalEntityId_Renamed;
		/// <summary>
		/// The meta-property for the {@code settlementDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> settlementDateOffset_Renamed;
		/// <summary>
		/// The meta-property for the {@code exCouponPeriod} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> exCouponPeriod_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "info", "currency", "notional", "accrualSchedule", "rateCalculation", "dayCount", "yieldConvention", "legalEntityId", "settlementDateOffset", "exCouponPeriod");
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
			case 3237038: // info
			  return info_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case 304659814: // accrualSchedule
			  return accrualSchedule_Renamed;
			case -521703991: // rateCalculation
			  return rateCalculation_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case -1895216418: // yieldConvention
			  return yieldConvention_Renamed;
			case 866287159: // legalEntityId
			  return legalEntityId_Renamed;
			case 135924714: // settlementDateOffset
			  return settlementDateOffset_Renamed;
			case 1408037338: // exCouponPeriod
			  return exCouponPeriod_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override CapitalIndexedBondSecurity.Builder builder()
		{
		  return new CapitalIndexedBondSecurity.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CapitalIndexedBondSecurity);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code info} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SecurityInfo> info()
		{
		  return info_Renamed;
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
		/// The meta-property for the {@code accrualSchedule} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PeriodicSchedule> accrualSchedule()
		{
		  return accrualSchedule_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code rateCalculation} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<InflationRateCalculation> rateCalculation()
		{
		  return rateCalculation_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dayCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DayCount> dayCount()
		{
		  return dayCount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code yieldConvention} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CapitalIndexedBondYieldConvention> yieldConvention()
		{
		  return yieldConvention_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code legalEntityId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LegalEntityId> legalEntityId()
		{
		  return legalEntityId_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code settlementDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> settlementDateOffset()
		{
		  return settlementDateOffset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code exCouponPeriod} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> exCouponPeriod()
		{
		  return exCouponPeriod_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  return ((CapitalIndexedBondSecurity) bean).Info;
			case 575402001: // currency
			  return ((CapitalIndexedBondSecurity) bean).Currency;
			case 1585636160: // notional
			  return ((CapitalIndexedBondSecurity) bean).Notional;
			case 304659814: // accrualSchedule
			  return ((CapitalIndexedBondSecurity) bean).AccrualSchedule;
			case -521703991: // rateCalculation
			  return ((CapitalIndexedBondSecurity) bean).RateCalculation;
			case 1905311443: // dayCount
			  return ((CapitalIndexedBondSecurity) bean).DayCount;
			case -1895216418: // yieldConvention
			  return ((CapitalIndexedBondSecurity) bean).YieldConvention;
			case 866287159: // legalEntityId
			  return ((CapitalIndexedBondSecurity) bean).LegalEntityId;
			case 135924714: // settlementDateOffset
			  return ((CapitalIndexedBondSecurity) bean).SettlementDateOffset;
			case 1408037338: // exCouponPeriod
			  return ((CapitalIndexedBondSecurity) bean).ExCouponPeriod;
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
	  /// The bean-builder for {@code CapitalIndexedBondSecurity}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<CapitalIndexedBondSecurity>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SecurityInfo info_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Currency currency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double notional_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PeriodicSchedule accrualSchedule_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal InflationRateCalculation rateCalculation_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount dayCount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CapitalIndexedBondYieldConvention yieldConvention_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LegalEntityId legalEntityId_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment settlementDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment exCouponPeriod_Renamed;

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
		internal Builder(CapitalIndexedBondSecurity beanToCopy)
		{
		  this.info_Renamed = beanToCopy.Info;
		  this.currency_Renamed = beanToCopy.Currency;
		  this.notional_Renamed = beanToCopy.Notional;
		  this.accrualSchedule_Renamed = beanToCopy.AccrualSchedule;
		  this.rateCalculation_Renamed = beanToCopy.RateCalculation;
		  this.dayCount_Renamed = beanToCopy.DayCount;
		  this.yieldConvention_Renamed = beanToCopy.YieldConvention;
		  this.legalEntityId_Renamed = beanToCopy.LegalEntityId;
		  this.settlementDateOffset_Renamed = beanToCopy.SettlementDateOffset;
		  this.exCouponPeriod_Renamed = beanToCopy.ExCouponPeriod;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  return info_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case 304659814: // accrualSchedule
			  return accrualSchedule_Renamed;
			case -521703991: // rateCalculation
			  return rateCalculation_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case -1895216418: // yieldConvention
			  return yieldConvention_Renamed;
			case 866287159: // legalEntityId
			  return legalEntityId_Renamed;
			case 135924714: // settlementDateOffset
			  return settlementDateOffset_Renamed;
			case 1408037338: // exCouponPeriod
			  return exCouponPeriod_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  this.info_Renamed = (SecurityInfo) newValue;
			  break;
			case 575402001: // currency
			  this.currency_Renamed = (Currency) newValue;
			  break;
			case 1585636160: // notional
			  this.notional_Renamed = (double?) newValue.Value;
			  break;
			case 304659814: // accrualSchedule
			  this.accrualSchedule_Renamed = (PeriodicSchedule) newValue;
			  break;
			case -521703991: // rateCalculation
			  this.rateCalculation_Renamed = (InflationRateCalculation) newValue;
			  break;
			case 1905311443: // dayCount
			  this.dayCount_Renamed = (DayCount) newValue;
			  break;
			case -1895216418: // yieldConvention
			  this.yieldConvention_Renamed = (CapitalIndexedBondYieldConvention) newValue;
			  break;
			case 866287159: // legalEntityId
			  this.legalEntityId_Renamed = (LegalEntityId) newValue;
			  break;
			case 135924714: // settlementDateOffset
			  this.settlementDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case 1408037338: // exCouponPeriod
			  this.exCouponPeriod_Renamed = (DaysAdjustment) newValue;
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

		public override CapitalIndexedBondSecurity build()
		{
		  return new CapitalIndexedBondSecurity(info_Renamed, currency_Renamed, notional_Renamed, accrualSchedule_Renamed, rateCalculation_Renamed, dayCount_Renamed, yieldConvention_Renamed, legalEntityId_Renamed, settlementDateOffset_Renamed, exCouponPeriod_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the standard security information.
		/// <para>
		/// This includes the security identifier.
		/// </para>
		/// </summary>
		/// <param name="info">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder info(SecurityInfo info)
		{
		  JodaBeanUtils.notNull(info, "info");
		  this.info_Renamed = info;
		  return this;
		}

		/// <summary>
		/// Sets the currency that the bond is traded in. </summary>
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
		/// The notional expressed here must be positive.
		/// The currency of the notional is specified by {@code currency}.
		/// </para>
		/// </summary>
		/// <param name="notional">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder notional(double notional)
		{
		  ArgChecker.notNegativeOrZero(notional, "notional");
		  this.notional_Renamed = notional;
		  return this;
		}

		/// <summary>
		/// Sets the accrual schedule.
		/// <para>
		/// This is used to define the accrual periods.
		/// These are used directly or indirectly to determine other dates in the product.
		/// </para>
		/// </summary>
		/// <param name="accrualSchedule">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder accrualSchedule(PeriodicSchedule accrualSchedule)
		{
		  JodaBeanUtils.notNull(accrualSchedule, "accrualSchedule");
		  this.accrualSchedule_Renamed = accrualSchedule;
		  return this;
		}

		/// <summary>
		/// Sets the inflation rate calculation.
		/// <para>
		/// The reference index is interpolated index or monthly index.
		/// Real coupons are represented by {@code gearing} in the calculation.
		/// The price index value at the start of the bond is represented by {@code firstIndexValue} in the calculation.
		/// </para>
		/// </summary>
		/// <param name="rateCalculation">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder rateCalculation(InflationRateCalculation rateCalculation)
		{
		  JodaBeanUtils.notNull(rateCalculation, "rateCalculation");
		  this.rateCalculation_Renamed = rateCalculation;
		  return this;
		}

		/// <summary>
		/// Sets the day count convention applicable.
		/// <para>
		/// The conversion from dates to a numerical value is made based on this day count.
		/// For the inflation-indexed bond, the day count convention is used to compute accrued interest.
		/// </para>
		/// <para>
		/// Note that the year fraction of a coupon payment is computed based on the unadjusted
		/// dates in the schedule.
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
		/// Sets yield convention.
		/// <para>
		/// The convention defines how to convert from yield to price and inversely.
		/// </para>
		/// </summary>
		/// <param name="yieldConvention">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder yieldConvention(CapitalIndexedBondYieldConvention yieldConvention)
		{
		  JodaBeanUtils.notNull(yieldConvention, "yieldConvention");
		  this.yieldConvention_Renamed = yieldConvention;
		  return this;
		}

		/// <summary>
		/// Sets the legal entity identifier.
		/// <para>
		/// This identifier is used for the legal entity that issues the bond.
		/// </para>
		/// </summary>
		/// <param name="legalEntityId">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder legalEntityId(LegalEntityId legalEntityId)
		{
		  JodaBeanUtils.notNull(legalEntityId, "legalEntityId");
		  this.legalEntityId_Renamed = legalEntityId;
		  return this;
		}

		/// <summary>
		/// Sets the number of days between valuation date and settlement date.
		/// <para>
		/// This is used to compute clean price.
		/// The clean price is the relative price to be paid at the standard settlement date in exchange for the bond.
		/// </para>
		/// </summary>
		/// <param name="settlementDateOffset">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder settlementDateOffset(DaysAdjustment settlementDateOffset)
		{
		  JodaBeanUtils.notNull(settlementDateOffset, "settlementDateOffset");
		  this.settlementDateOffset_Renamed = settlementDateOffset;
		  return this;
		}

		/// <summary>
		/// Sets ex-coupon period.
		/// <para>
		/// Some bonds trade ex-coupons before the coupon payment. The coupon is paid not to the
		/// owner of the bond on the payment date but to the owner of the bond on the detachment date.
		/// The difference between the two is the ex-coupon period (measured in days).
		/// </para>
		/// <para>
		/// Because the detachment date is not after the coupon date, the number of days
		/// stored in this field should be zero or negative.
		/// </para>
		/// </summary>
		/// <param name="exCouponPeriod">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder exCouponPeriod(DaysAdjustment exCouponPeriod)
		{
		  JodaBeanUtils.notNull(exCouponPeriod, "exCouponPeriod");
		  this.exCouponPeriod_Renamed = exCouponPeriod;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(352);
		  buf.Append("CapitalIndexedBondSecurity.Builder{");
		  buf.Append("info").Append('=').Append(JodaBeanUtils.ToString(info_Renamed)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("notional").Append('=').Append(JodaBeanUtils.ToString(notional_Renamed)).Append(',').Append(' ');
		  buf.Append("accrualSchedule").Append('=').Append(JodaBeanUtils.ToString(accrualSchedule_Renamed)).Append(',').Append(' ');
		  buf.Append("rateCalculation").Append('=').Append(JodaBeanUtils.ToString(rateCalculation_Renamed)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("yieldConvention").Append('=').Append(JodaBeanUtils.ToString(yieldConvention_Renamed)).Append(',').Append(' ');
		  buf.Append("legalEntityId").Append('=').Append(JodaBeanUtils.ToString(legalEntityId_Renamed)).Append(',').Append(' ');
		  buf.Append("settlementDateOffset").Append('=').Append(JodaBeanUtils.ToString(settlementDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("exCouponPeriod").Append('=').Append(JodaBeanUtils.ToString(exCouponPeriod_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}