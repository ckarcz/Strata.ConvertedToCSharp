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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.ensureOnlyOne;


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

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// A single-name credit default swap (CDS), resolved for pricing.
	/// <para>
	/// This is the resolved form of <seealso cref="Cds"/> and is an input to the pricers.
	/// Applications will typically create a {@code ResolvedCds} from a {@code Cds}
	/// using <seealso cref="Cds#resolve(ReferenceData)"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ResolvedCds implements com.opengamma.strata.product.ResolvedProduct, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ResolvedCds : ResolvedProduct, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.common.BuySell buySell;
		private readonly BuySell buySell;
	  /// <summary>
	  /// The legal entity identifier.
	  /// <para>
	  /// This identifier is used for the reference legal entity of the CDS.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.StandardId legalEntityId;
	  private readonly StandardId legalEntityId;
	  /// <summary>
	  /// The periodic payments based on the fixed rate.
	  /// <para>
	  /// Each payment period represents part of the life-time of the leg.
	  /// In most cases, the periods do not overlap. However, since each payment period
	  /// is essentially independent the data model allows overlapping periods.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty") private final com.google.common.collect.ImmutableList<CreditCouponPaymentPeriod> paymentPeriods;
	  private readonly ImmutableList<CreditCouponPaymentPeriod> paymentPeriods;
	  /// <summary>
	  /// The protection end date.
	  /// <para>
	  /// This may be different from the accrual end date of the last payment period in {@code periodicPayments}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate protectionEndDate;
	  private readonly LocalDate protectionEndDate;
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
	  /// The payment on default.
	  /// <para>
	  /// Whether the accrued premium is paid in the event of a default.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final PaymentOnDefault paymentOnDefault;
	  private readonly PaymentOnDefault paymentOnDefault;
	  /// <summary>
	  /// The protection start of the day.
	  /// <para>
	  /// When the protection starts on the start date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final ProtectionStartOfDay protectionStart;
	  private readonly ProtectionStartOfDay protectionStart;
	  /// <summary>
	  /// The number of days between valuation date and step-in date.
	  /// <para>
	  /// The step-in date is also called protection effective date. 
	  /// It is usually 1 calendar day for standardized CDS contracts. 
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DaysAdjustment stepinDateOffset;
	  private readonly DaysAdjustment stepinDateOffset;
	  /// <summary>
	  /// The number of days between valuation date and settlement date.
	  /// <para>
	  /// It is usually 3 business days for standardized CDS contracts.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DaysAdjustment settlementDateOffset;
	  private readonly DaysAdjustment settlementDateOffset;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains the accrual start date.
	  /// <para>
	  /// In general this is different from the protection start date. 
	  /// Use {@code stepinDateOffset} to compute the protection start date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the accrual start date </returns>
	  public LocalDate AccrualStartDate
	  {
		  get
		  {
			return paymentPeriods.get(0).StartDate;
		  }
	  }

	  /// <summary>
	  /// Obtains the accrual end date.
	  /// </summary>
	  /// <returns> the accrual end date </returns>
	  public LocalDate AccrualEndDate
	  {
		  get
		  {
			return paymentPeriods.get(paymentPeriods.size() - 1).EndDate;
		  }
	  }

	  /// <summary>
	  /// Obtains the notional.
	  /// </summary>
	  /// <returns> the notional </returns>
	  public double Notional
	  {
		  get
		  {
			return paymentPeriods.get(0).Notional;
		  }
	  }

	  /// <summary>
	  /// Obtains the currency.
	  /// </summary>
	  /// <returns> the currency </returns>
	  public Currency Currency
	  {
		  get
		  {
			return paymentPeriods.get(0).Currency;
		  }
	  }

	  /// <summary>
	  /// Obtains the fixed coupon rate.
	  /// </summary>
	  /// <returns> the fixed rate </returns>
	  public double FixedRate
	  {
		  get
		  {
			return paymentPeriods.get(0).FixedRate;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains the effective start date from the step-in date. 
	  /// </summary>
	  /// <param name="stepinDate">  the step-in date </param>
	  /// <returns> the effective start date </returns>
	  public LocalDate calculateEffectiveStartDate(LocalDate stepinDate)
	  {
		LocalDate startDate = stepinDate.isAfter(AccrualStartDate) ? stepinDate : AccrualStartDate;
		return protectionStart.Beginning ? startDate.minusDays(1) : startDate;
	  }

	  /// <summary>
	  /// Calculates the settlement date from the valuation date.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <param name="refData">  the reference data to use </param>
	  /// <returns> the settlement date </returns>
	  public LocalDate calculateSettlementDateFromValuation(LocalDate valuationDate, ReferenceData refData)
	  {
		return settlementDateOffset.adjust(valuationDate, refData);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the period that contains the specified date.
	  /// <para>
	  /// The search is performed using unadjusted dates.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to find the period for </param>
	  /// <returns> the period, empty if not found </returns>
	  /// <exception cref="IllegalArgumentException"> if more than one period matches </exception>
	  public Optional<CreditCouponPaymentPeriod> findPeriod(LocalDate date)
	  {
		return paymentPeriods.Where(p => p.contains(date)).Aggregate(ensureOnlyOne());
	  }

	  /// <summary>
	  /// Calculates the accrued premium per fractional spread for unit notional.
	  /// </summary>
	  /// <param name="stepinDate">  the step-in date </param>
	  /// <returns> the accrued year fraction </returns>
	  public double accruedYearFraction(LocalDate stepinDate)
	  {
		if (stepinDate.isBefore(AccrualStartDate))
		{
		  return 0d;
		}
		if (stepinDate.isEqual(AccrualEndDate))
		{
		  return paymentPeriods.get(paymentPeriods.size() - 1).YearFraction;
		}
		CreditCouponPaymentPeriod period = findPeriod(stepinDate).orElseThrow(() => new System.ArgumentException("Date outside range"));
		return dayCount.relativeYearFraction(period.StartDate, stepinDate);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedCds}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ResolvedCds.Meta meta()
	  {
		return ResolvedCds.Meta.INSTANCE;
	  }

	  static ResolvedCds()
	  {
		MetaBean.register(ResolvedCds.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ResolvedCds.Builder builder()
	  {
		return new ResolvedCds.Builder();
	  }

	  private ResolvedCds(BuySell buySell, StandardId legalEntityId, IList<CreditCouponPaymentPeriod> paymentPeriods, LocalDate protectionEndDate, DayCount dayCount, PaymentOnDefault paymentOnDefault, ProtectionStartOfDay protectionStart, DaysAdjustment stepinDateOffset, DaysAdjustment settlementDateOffset)
	  {
		JodaBeanUtils.notNull(buySell, "buySell");
		JodaBeanUtils.notNull(legalEntityId, "legalEntityId");
		JodaBeanUtils.notEmpty(paymentPeriods, "paymentPeriods");
		JodaBeanUtils.notNull(protectionEndDate, "protectionEndDate");
		JodaBeanUtils.notNull(dayCount, "dayCount");
		JodaBeanUtils.notNull(paymentOnDefault, "paymentOnDefault");
		JodaBeanUtils.notNull(protectionStart, "protectionStart");
		JodaBeanUtils.notNull(stepinDateOffset, "stepinDateOffset");
		JodaBeanUtils.notNull(settlementDateOffset, "settlementDateOffset");
		this.buySell = buySell;
		this.legalEntityId = legalEntityId;
		this.paymentPeriods = ImmutableList.copyOf(paymentPeriods);
		this.protectionEndDate = protectionEndDate;
		this.dayCount = dayCount;
		this.paymentOnDefault = paymentOnDefault;
		this.protectionStart = protectionStart;
		this.stepinDateOffset = stepinDateOffset;
		this.settlementDateOffset = settlementDateOffset;
	  }

	  public override ResolvedCds.Meta metaBean()
	  {
		return ResolvedCds.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets whether the CDS is buy or sell.
	  /// <para>
	  /// A value of 'Buy' implies buying protection, where the fixed coupon is paid
	  /// and the protection is received  in the event of default.
	  /// A value of 'Sell' implies selling protection, where the fixed coupon is received
	  /// and the protection is paid in the event of default.
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
	  /// Gets the legal entity identifier.
	  /// <para>
	  /// This identifier is used for the reference legal entity of the CDS.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public StandardId LegalEntityId
	  {
		  get
		  {
			return legalEntityId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the periodic payments based on the fixed rate.
	  /// <para>
	  /// Each payment period represents part of the life-time of the leg.
	  /// In most cases, the periods do not overlap. However, since each payment period
	  /// is essentially independent the data model allows overlapping periods.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public ImmutableList<CreditCouponPaymentPeriod> PaymentPeriods
	  {
		  get
		  {
			return paymentPeriods;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the protection end date.
	  /// <para>
	  /// This may be different from the accrual end date of the last payment period in {@code periodicPayments}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate ProtectionEndDate
	  {
		  get
		  {
			return protectionEndDate;
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
	  /// Gets the payment on default.
	  /// <para>
	  /// Whether the accrued premium is paid in the event of a default.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PaymentOnDefault PaymentOnDefault
	  {
		  get
		  {
			return paymentOnDefault;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the protection start of the day.
	  /// <para>
	  /// When the protection starts on the start date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ProtectionStartOfDay ProtectionStart
	  {
		  get
		  {
			return protectionStart;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of days between valuation date and step-in date.
	  /// <para>
	  /// The step-in date is also called protection effective date.
	  /// It is usually 1 calendar day for standardized CDS contracts.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DaysAdjustment StepinDateOffset
	  {
		  get
		  {
			return stepinDateOffset;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of days between valuation date and settlement date.
	  /// <para>
	  /// It is usually 3 business days for standardized CDS contracts.
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
		  ResolvedCds other = (ResolvedCds) obj;
		  return JodaBeanUtils.equal(buySell, other.buySell) && JodaBeanUtils.equal(legalEntityId, other.legalEntityId) && JodaBeanUtils.equal(paymentPeriods, other.paymentPeriods) && JodaBeanUtils.equal(protectionEndDate, other.protectionEndDate) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(paymentOnDefault, other.paymentOnDefault) && JodaBeanUtils.equal(protectionStart, other.protectionStart) && JodaBeanUtils.equal(stepinDateOffset, other.stepinDateOffset) && JodaBeanUtils.equal(settlementDateOffset, other.settlementDateOffset);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(buySell);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(legalEntityId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentPeriods);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(protectionEndDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentOnDefault);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(protectionStart);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(stepinDateOffset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(settlementDateOffset);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(320);
		buf.Append("ResolvedCds{");
		buf.Append("buySell").Append('=').Append(buySell).Append(',').Append(' ');
		buf.Append("legalEntityId").Append('=').Append(legalEntityId).Append(',').Append(' ');
		buf.Append("paymentPeriods").Append('=').Append(paymentPeriods).Append(',').Append(' ');
		buf.Append("protectionEndDate").Append('=').Append(protectionEndDate).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("paymentOnDefault").Append('=').Append(paymentOnDefault).Append(',').Append(' ');
		buf.Append("protectionStart").Append('=').Append(protectionStart).Append(',').Append(' ');
		buf.Append("stepinDateOffset").Append('=').Append(stepinDateOffset).Append(',').Append(' ');
		buf.Append("settlementDateOffset").Append('=').Append(JodaBeanUtils.ToString(settlementDateOffset));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedCds}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  buySell_Renamed = DirectMetaProperty.ofImmutable(this, "buySell", typeof(ResolvedCds), typeof(BuySell));
			  legalEntityId_Renamed = DirectMetaProperty.ofImmutable(this, "legalEntityId", typeof(ResolvedCds), typeof(StandardId));
			  paymentPeriods_Renamed = DirectMetaProperty.ofImmutable(this, "paymentPeriods", typeof(ResolvedCds), (Type) typeof(ImmutableList));
			  protectionEndDate_Renamed = DirectMetaProperty.ofImmutable(this, "protectionEndDate", typeof(ResolvedCds), typeof(LocalDate));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(ResolvedCds), typeof(DayCount));
			  paymentOnDefault_Renamed = DirectMetaProperty.ofImmutable(this, "paymentOnDefault", typeof(ResolvedCds), typeof(PaymentOnDefault));
			  protectionStart_Renamed = DirectMetaProperty.ofImmutable(this, "protectionStart", typeof(ResolvedCds), typeof(ProtectionStartOfDay));
			  stepinDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "stepinDateOffset", typeof(ResolvedCds), typeof(DaysAdjustment));
			  settlementDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "settlementDateOffset", typeof(ResolvedCds), typeof(DaysAdjustment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "buySell", "legalEntityId", "paymentPeriods", "protectionEndDate", "dayCount", "paymentOnDefault", "protectionStart", "stepinDateOffset", "settlementDateOffset");
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
		/// The meta-property for the {@code legalEntityId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<StandardId> legalEntityId_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentPeriods} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<CreditCouponPaymentPeriod>> paymentPeriods = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "paymentPeriods", ResolvedCds.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<CreditCouponPaymentPeriod>> paymentPeriods_Renamed;
		/// <summary>
		/// The meta-property for the {@code protectionEndDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> protectionEndDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code dayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> dayCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentOnDefault} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PaymentOnDefault> paymentOnDefault_Renamed;
		/// <summary>
		/// The meta-property for the {@code protectionStart} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ProtectionStartOfDay> protectionStart_Renamed;
		/// <summary>
		/// The meta-property for the {@code stepinDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> stepinDateOffset_Renamed;
		/// <summary>
		/// The meta-property for the {@code settlementDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> settlementDateOffset_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "buySell", "legalEntityId", "paymentPeriods", "protectionEndDate", "dayCount", "paymentOnDefault", "protectionStart", "stepinDateOffset", "settlementDateOffset");
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
			case 866287159: // legalEntityId
			  return legalEntityId_Renamed;
			case -1674414612: // paymentPeriods
			  return paymentPeriods_Renamed;
			case -1193325040: // protectionEndDate
			  return protectionEndDate_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case -480203780: // paymentOnDefault
			  return paymentOnDefault_Renamed;
			case 2103482633: // protectionStart
			  return protectionStart_Renamed;
			case 852621746: // stepinDateOffset
			  return stepinDateOffset_Renamed;
			case 135924714: // settlementDateOffset
			  return settlementDateOffset_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ResolvedCds.Builder builder()
		{
		  return new ResolvedCds.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ResolvedCds);
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
		/// The meta-property for the {@code legalEntityId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<StandardId> legalEntityId()
		{
		  return legalEntityId_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code paymentPeriods} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<CreditCouponPaymentPeriod>> paymentPeriods()
		{
		  return paymentPeriods_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code protectionEndDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> protectionEndDate()
		{
		  return protectionEndDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dayCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DayCount> dayCount()
		{
		  return dayCount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code paymentOnDefault} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PaymentOnDefault> paymentOnDefault()
		{
		  return paymentOnDefault_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code protectionStart} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ProtectionStartOfDay> protectionStart()
		{
		  return protectionStart_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code stepinDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> stepinDateOffset()
		{
		  return stepinDateOffset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code settlementDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> settlementDateOffset()
		{
		  return settlementDateOffset_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 244977400: // buySell
			  return ((ResolvedCds) bean).BuySell;
			case 866287159: // legalEntityId
			  return ((ResolvedCds) bean).LegalEntityId;
			case -1674414612: // paymentPeriods
			  return ((ResolvedCds) bean).PaymentPeriods;
			case -1193325040: // protectionEndDate
			  return ((ResolvedCds) bean).ProtectionEndDate;
			case 1905311443: // dayCount
			  return ((ResolvedCds) bean).DayCount;
			case -480203780: // paymentOnDefault
			  return ((ResolvedCds) bean).PaymentOnDefault;
			case 2103482633: // protectionStart
			  return ((ResolvedCds) bean).ProtectionStart;
			case 852621746: // stepinDateOffset
			  return ((ResolvedCds) bean).StepinDateOffset;
			case 135924714: // settlementDateOffset
			  return ((ResolvedCds) bean).SettlementDateOffset;
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
	  /// The bean-builder for {@code ResolvedCds}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ResolvedCds>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BuySell buySell_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal StandardId legalEntityId_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<CreditCouponPaymentPeriod> paymentPeriods_Renamed = ImmutableList.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate protectionEndDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount dayCount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PaymentOnDefault paymentOnDefault_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ProtectionStartOfDay protectionStart_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment stepinDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment settlementDateOffset_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ResolvedCds beanToCopy)
		{
		  this.buySell_Renamed = beanToCopy.BuySell;
		  this.legalEntityId_Renamed = beanToCopy.LegalEntityId;
		  this.paymentPeriods_Renamed = beanToCopy.PaymentPeriods;
		  this.protectionEndDate_Renamed = beanToCopy.ProtectionEndDate;
		  this.dayCount_Renamed = beanToCopy.DayCount;
		  this.paymentOnDefault_Renamed = beanToCopy.PaymentOnDefault;
		  this.protectionStart_Renamed = beanToCopy.ProtectionStart;
		  this.stepinDateOffset_Renamed = beanToCopy.StepinDateOffset;
		  this.settlementDateOffset_Renamed = beanToCopy.SettlementDateOffset;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 244977400: // buySell
			  return buySell_Renamed;
			case 866287159: // legalEntityId
			  return legalEntityId_Renamed;
			case -1674414612: // paymentPeriods
			  return paymentPeriods_Renamed;
			case -1193325040: // protectionEndDate
			  return protectionEndDate_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case -480203780: // paymentOnDefault
			  return paymentOnDefault_Renamed;
			case 2103482633: // protectionStart
			  return protectionStart_Renamed;
			case 852621746: // stepinDateOffset
			  return stepinDateOffset_Renamed;
			case 135924714: // settlementDateOffset
			  return settlementDateOffset_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Builder set(String propertyName, Object newValue)
		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 244977400: // buySell
			  this.buySell_Renamed = (BuySell) newValue;
			  break;
			case 866287159: // legalEntityId
			  this.legalEntityId_Renamed = (StandardId) newValue;
			  break;
			case -1674414612: // paymentPeriods
			  this.paymentPeriods_Renamed = (IList<CreditCouponPaymentPeriod>) newValue;
			  break;
			case -1193325040: // protectionEndDate
			  this.protectionEndDate_Renamed = (LocalDate) newValue;
			  break;
			case 1905311443: // dayCount
			  this.dayCount_Renamed = (DayCount) newValue;
			  break;
			case -480203780: // paymentOnDefault
			  this.paymentOnDefault_Renamed = (PaymentOnDefault) newValue;
			  break;
			case 2103482633: // protectionStart
			  this.protectionStart_Renamed = (ProtectionStartOfDay) newValue;
			  break;
			case 852621746: // stepinDateOffset
			  this.stepinDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case 135924714: // settlementDateOffset
			  this.settlementDateOffset_Renamed = (DaysAdjustment) newValue;
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

		public override ResolvedCds build()
		{
		  return new ResolvedCds(buySell_Renamed, legalEntityId_Renamed, paymentPeriods_Renamed, protectionEndDate_Renamed, dayCount_Renamed, paymentOnDefault_Renamed, protectionStart_Renamed, stepinDateOffset_Renamed, settlementDateOffset_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets whether the CDS is buy or sell.
		/// <para>
		/// A value of 'Buy' implies buying protection, where the fixed coupon is paid
		/// and the protection is received  in the event of default.
		/// A value of 'Sell' implies selling protection, where the fixed coupon is received
		/// and the protection is paid in the event of default.
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
		/// Sets the legal entity identifier.
		/// <para>
		/// This identifier is used for the reference legal entity of the CDS.
		/// </para>
		/// </summary>
		/// <param name="legalEntityId">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder legalEntityId(StandardId legalEntityId)
		{
		  JodaBeanUtils.notNull(legalEntityId, "legalEntityId");
		  this.legalEntityId_Renamed = legalEntityId;
		  return this;
		}

		/// <summary>
		/// Sets the periodic payments based on the fixed rate.
		/// <para>
		/// Each payment period represents part of the life-time of the leg.
		/// In most cases, the periods do not overlap. However, since each payment period
		/// is essentially independent the data model allows overlapping periods.
		/// </para>
		/// </summary>
		/// <param name="paymentPeriods">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentPeriods(IList<CreditCouponPaymentPeriod> paymentPeriods)
		{
		  JodaBeanUtils.notEmpty(paymentPeriods, "paymentPeriods");
		  this.paymentPeriods_Renamed = paymentPeriods;
		  return this;
		}

		/// <summary>
		/// Sets the {@code paymentPeriods} property in the builder
		/// from an array of objects. </summary>
		/// <param name="paymentPeriods">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentPeriods(params CreditCouponPaymentPeriod[] paymentPeriods)
		{
		  return this.paymentPeriods(ImmutableList.copyOf(paymentPeriods));
		}

		/// <summary>
		/// Sets the protection end date.
		/// <para>
		/// This may be different from the accrual end date of the last payment period in {@code periodicPayments}.
		/// </para>
		/// </summary>
		/// <param name="protectionEndDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder protectionEndDate(LocalDate protectionEndDate)
		{
		  JodaBeanUtils.notNull(protectionEndDate, "protectionEndDate");
		  this.protectionEndDate_Renamed = protectionEndDate;
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
		/// Sets the payment on default.
		/// <para>
		/// Whether the accrued premium is paid in the event of a default.
		/// </para>
		/// </summary>
		/// <param name="paymentOnDefault">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentOnDefault(PaymentOnDefault paymentOnDefault)
		{
		  JodaBeanUtils.notNull(paymentOnDefault, "paymentOnDefault");
		  this.paymentOnDefault_Renamed = paymentOnDefault;
		  return this;
		}

		/// <summary>
		/// Sets the protection start of the day.
		/// <para>
		/// When the protection starts on the start date.
		/// </para>
		/// </summary>
		/// <param name="protectionStart">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder protectionStart(ProtectionStartOfDay protectionStart)
		{
		  JodaBeanUtils.notNull(protectionStart, "protectionStart");
		  this.protectionStart_Renamed = protectionStart;
		  return this;
		}

		/// <summary>
		/// Sets the number of days between valuation date and step-in date.
		/// <para>
		/// The step-in date is also called protection effective date.
		/// It is usually 1 calendar day for standardized CDS contracts.
		/// </para>
		/// </summary>
		/// <param name="stepinDateOffset">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder stepinDateOffset(DaysAdjustment stepinDateOffset)
		{
		  JodaBeanUtils.notNull(stepinDateOffset, "stepinDateOffset");
		  this.stepinDateOffset_Renamed = stepinDateOffset;
		  return this;
		}

		/// <summary>
		/// Sets the number of days between valuation date and settlement date.
		/// <para>
		/// It is usually 3 business days for standardized CDS contracts.
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

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(320);
		  buf.Append("ResolvedCds.Builder{");
		  buf.Append("buySell").Append('=').Append(JodaBeanUtils.ToString(buySell_Renamed)).Append(',').Append(' ');
		  buf.Append("legalEntityId").Append('=').Append(JodaBeanUtils.ToString(legalEntityId_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentPeriods").Append('=').Append(JodaBeanUtils.ToString(paymentPeriods_Renamed)).Append(',').Append(' ');
		  buf.Append("protectionEndDate").Append('=').Append(JodaBeanUtils.ToString(protectionEndDate_Renamed)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentOnDefault").Append('=').Append(JodaBeanUtils.ToString(paymentOnDefault_Renamed)).Append(',').Append(' ');
		  buf.Append("protectionStart").Append('=').Append(JodaBeanUtils.ToString(protectionStart_Renamed)).Append(',').Append(' ');
		  buf.Append("stepinDateOffset").Append('=').Append(JodaBeanUtils.ToString(stepinDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("settlementDateOffset").Append('=').Append(JodaBeanUtils.ToString(settlementDateOffset_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}