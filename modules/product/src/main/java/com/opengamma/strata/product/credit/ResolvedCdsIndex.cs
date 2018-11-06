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
	/// A CDS (portfolio) index, resolved for pricing.
	/// <para>
	/// This is the resolved form of <seealso cref="CdsIndex"/> and is an input to the pricers.
	/// Applications will typically create a {@code ResolvedCdsIndex} from a {@code CdsIndex}
	/// using <seealso cref="CdsIndex#resolve(ReferenceData)"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ResolvedCdsIndex implements com.opengamma.strata.product.ResolvedProduct, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ResolvedCdsIndex : ResolvedProduct, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.common.BuySell buySell;
		private readonly BuySell buySell;
	  /// <summary>
	  /// The CDS index identifier.
	  /// <para>
	  /// This identifier is used to refer this CDS index product.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.StandardId cdsIndexId;
	  private readonly StandardId cdsIndexId;
	  /// <summary>
	  /// The legal entity identifiers.
	  /// <para>
	  /// These identifiers refer to the reference legal entities of the CDS index.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableList<com.opengamma.strata.basics.StandardId> legalEntityIds;
	  private readonly ImmutableList<StandardId> legalEntityIds;
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
	  /// It is usually 1 calendar day for standardized CDS index contracts. 
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DaysAdjustment stepinDateOffset;
	  private readonly DaysAdjustment stepinDateOffset;
	  /// <summary>
	  /// The number of days between valuation date and settlement date.
	  /// <para>
	  /// It is usually 3 business days for standardized CDS index contracts.
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

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Reduce this instance to {@code ResolvedCds}.
	  /// <para>
	  /// The resultant object is used for pricing CDS index products under the homogeneous pool assumption on constituent  
	  /// credit curves. 
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the CDS product </returns>
	  public ResolvedCds toSingleNameCds()
	  {
		return ResolvedCds.builder().buySell(BuySell).dayCount(DayCount).legalEntityId(CdsIndexId).paymentOnDefault(PaymentOnDefault).paymentPeriods(PaymentPeriods).protectionEndDate(ProtectionEndDate).protectionStart(ProtectionStart).stepinDateOffset(StepinDateOffset).settlementDateOffset(SettlementDateOffset).build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedCdsIndex}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ResolvedCdsIndex.Meta meta()
	  {
		return ResolvedCdsIndex.Meta.INSTANCE;
	  }

	  static ResolvedCdsIndex()
	  {
		MetaBean.register(ResolvedCdsIndex.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ResolvedCdsIndex.Builder builder()
	  {
		return new ResolvedCdsIndex.Builder();
	  }

	  private ResolvedCdsIndex(BuySell buySell, StandardId cdsIndexId, IList<StandardId> legalEntityIds, IList<CreditCouponPaymentPeriod> paymentPeriods, LocalDate protectionEndDate, DayCount dayCount, PaymentOnDefault paymentOnDefault, ProtectionStartOfDay protectionStart, DaysAdjustment stepinDateOffset, DaysAdjustment settlementDateOffset)
	  {
		JodaBeanUtils.notNull(buySell, "buySell");
		JodaBeanUtils.notNull(cdsIndexId, "cdsIndexId");
		JodaBeanUtils.notNull(legalEntityIds, "legalEntityIds");
		JodaBeanUtils.notEmpty(paymentPeriods, "paymentPeriods");
		JodaBeanUtils.notNull(protectionEndDate, "protectionEndDate");
		JodaBeanUtils.notNull(dayCount, "dayCount");
		JodaBeanUtils.notNull(paymentOnDefault, "paymentOnDefault");
		JodaBeanUtils.notNull(protectionStart, "protectionStart");
		JodaBeanUtils.notNull(stepinDateOffset, "stepinDateOffset");
		JodaBeanUtils.notNull(settlementDateOffset, "settlementDateOffset");
		this.buySell = buySell;
		this.cdsIndexId = cdsIndexId;
		this.legalEntityIds = ImmutableList.copyOf(legalEntityIds);
		this.paymentPeriods = ImmutableList.copyOf(paymentPeriods);
		this.protectionEndDate = protectionEndDate;
		this.dayCount = dayCount;
		this.paymentOnDefault = paymentOnDefault;
		this.protectionStart = protectionStart;
		this.stepinDateOffset = stepinDateOffset;
		this.settlementDateOffset = settlementDateOffset;
	  }

	  public override ResolvedCdsIndex.Meta metaBean()
	  {
		return ResolvedCdsIndex.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets whether the CDS index is buy or sell.
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
	  /// Gets the CDS index identifier.
	  /// <para>
	  /// This identifier is used to refer this CDS index product.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public StandardId CdsIndexId
	  {
		  get
		  {
			return cdsIndexId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the legal entity identifiers.
	  /// <para>
	  /// These identifiers refer to the reference legal entities of the CDS index.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<StandardId> LegalEntityIds
	  {
		  get
		  {
			return legalEntityIds;
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
	  /// It is usually 1 calendar day for standardized CDS index contracts.
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
	  /// It is usually 3 business days for standardized CDS index contracts.
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
		  ResolvedCdsIndex other = (ResolvedCdsIndex) obj;
		  return JodaBeanUtils.equal(buySell, other.buySell) && JodaBeanUtils.equal(cdsIndexId, other.cdsIndexId) && JodaBeanUtils.equal(legalEntityIds, other.legalEntityIds) && JodaBeanUtils.equal(paymentPeriods, other.paymentPeriods) && JodaBeanUtils.equal(protectionEndDate, other.protectionEndDate) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(paymentOnDefault, other.paymentOnDefault) && JodaBeanUtils.equal(protectionStart, other.protectionStart) && JodaBeanUtils.equal(stepinDateOffset, other.stepinDateOffset) && JodaBeanUtils.equal(settlementDateOffset, other.settlementDateOffset);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(buySell);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(cdsIndexId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(legalEntityIds);
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
		StringBuilder buf = new StringBuilder(352);
		buf.Append("ResolvedCdsIndex{");
		buf.Append("buySell").Append('=').Append(buySell).Append(',').Append(' ');
		buf.Append("cdsIndexId").Append('=').Append(cdsIndexId).Append(',').Append(' ');
		buf.Append("legalEntityIds").Append('=').Append(legalEntityIds).Append(',').Append(' ');
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
	  /// The meta-bean for {@code ResolvedCdsIndex}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  buySell_Renamed = DirectMetaProperty.ofImmutable(this, "buySell", typeof(ResolvedCdsIndex), typeof(BuySell));
			  cdsIndexId_Renamed = DirectMetaProperty.ofImmutable(this, "cdsIndexId", typeof(ResolvedCdsIndex), typeof(StandardId));
			  legalEntityIds_Renamed = DirectMetaProperty.ofImmutable(this, "legalEntityIds", typeof(ResolvedCdsIndex), (Type) typeof(ImmutableList));
			  paymentPeriods_Renamed = DirectMetaProperty.ofImmutable(this, "paymentPeriods", typeof(ResolvedCdsIndex), (Type) typeof(ImmutableList));
			  protectionEndDate_Renamed = DirectMetaProperty.ofImmutable(this, "protectionEndDate", typeof(ResolvedCdsIndex), typeof(LocalDate));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(ResolvedCdsIndex), typeof(DayCount));
			  paymentOnDefault_Renamed = DirectMetaProperty.ofImmutable(this, "paymentOnDefault", typeof(ResolvedCdsIndex), typeof(PaymentOnDefault));
			  protectionStart_Renamed = DirectMetaProperty.ofImmutable(this, "protectionStart", typeof(ResolvedCdsIndex), typeof(ProtectionStartOfDay));
			  stepinDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "stepinDateOffset", typeof(ResolvedCdsIndex), typeof(DaysAdjustment));
			  settlementDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "settlementDateOffset", typeof(ResolvedCdsIndex), typeof(DaysAdjustment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "buySell", "cdsIndexId", "legalEntityIds", "paymentPeriods", "protectionEndDate", "dayCount", "paymentOnDefault", "protectionStart", "stepinDateOffset", "settlementDateOffset");
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
		/// The meta-property for the {@code cdsIndexId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<StandardId> cdsIndexId_Renamed;
		/// <summary>
		/// The meta-property for the {@code legalEntityIds} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<com.opengamma.strata.basics.StandardId>> legalEntityIds = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "legalEntityIds", ResolvedCdsIndex.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<StandardId>> legalEntityIds_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentPeriods} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<CreditCouponPaymentPeriod>> paymentPeriods = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "paymentPeriods", ResolvedCdsIndex.class, (Class) com.google.common.collect.ImmutableList.class);
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
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "buySell", "cdsIndexId", "legalEntityIds", "paymentPeriods", "protectionEndDate", "dayCount", "paymentOnDefault", "protectionStart", "stepinDateOffset", "settlementDateOffset");
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
			case -464117509: // cdsIndexId
			  return cdsIndexId_Renamed;
			case 1085098268: // legalEntityIds
			  return legalEntityIds_Renamed;
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

		public override ResolvedCdsIndex.Builder builder()
		{
		  return new ResolvedCdsIndex.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ResolvedCdsIndex);
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
		/// The meta-property for the {@code cdsIndexId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<StandardId> cdsIndexId()
		{
		  return cdsIndexId_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code legalEntityIds} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<StandardId>> legalEntityIds()
		{
		  return legalEntityIds_Renamed;
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
			  return ((ResolvedCdsIndex) bean).BuySell;
			case -464117509: // cdsIndexId
			  return ((ResolvedCdsIndex) bean).CdsIndexId;
			case 1085098268: // legalEntityIds
			  return ((ResolvedCdsIndex) bean).LegalEntityIds;
			case -1674414612: // paymentPeriods
			  return ((ResolvedCdsIndex) bean).PaymentPeriods;
			case -1193325040: // protectionEndDate
			  return ((ResolvedCdsIndex) bean).ProtectionEndDate;
			case 1905311443: // dayCount
			  return ((ResolvedCdsIndex) bean).DayCount;
			case -480203780: // paymentOnDefault
			  return ((ResolvedCdsIndex) bean).PaymentOnDefault;
			case 2103482633: // protectionStart
			  return ((ResolvedCdsIndex) bean).ProtectionStart;
			case 852621746: // stepinDateOffset
			  return ((ResolvedCdsIndex) bean).StepinDateOffset;
			case 135924714: // settlementDateOffset
			  return ((ResolvedCdsIndex) bean).SettlementDateOffset;
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
	  /// The bean-builder for {@code ResolvedCdsIndex}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ResolvedCdsIndex>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BuySell buySell_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal StandardId cdsIndexId_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<StandardId> legalEntityIds_Renamed = ImmutableList.of();
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
		internal Builder(ResolvedCdsIndex beanToCopy)
		{
		  this.buySell_Renamed = beanToCopy.BuySell;
		  this.cdsIndexId_Renamed = beanToCopy.CdsIndexId;
		  this.legalEntityIds_Renamed = beanToCopy.LegalEntityIds;
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
			case -464117509: // cdsIndexId
			  return cdsIndexId_Renamed;
			case 1085098268: // legalEntityIds
			  return legalEntityIds_Renamed;
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
			case -464117509: // cdsIndexId
			  this.cdsIndexId_Renamed = (StandardId) newValue;
			  break;
			case 1085098268: // legalEntityIds
			  this.legalEntityIds_Renamed = (IList<StandardId>) newValue;
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

		public override ResolvedCdsIndex build()
		{
		  return new ResolvedCdsIndex(buySell_Renamed, cdsIndexId_Renamed, legalEntityIds_Renamed, paymentPeriods_Renamed, protectionEndDate_Renamed, dayCount_Renamed, paymentOnDefault_Renamed, protectionStart_Renamed, stepinDateOffset_Renamed, settlementDateOffset_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets whether the CDS index is buy or sell.
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
		/// Sets the CDS index identifier.
		/// <para>
		/// This identifier is used to refer this CDS index product.
		/// </para>
		/// </summary>
		/// <param name="cdsIndexId">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder cdsIndexId(StandardId cdsIndexId)
		{
		  JodaBeanUtils.notNull(cdsIndexId, "cdsIndexId");
		  this.cdsIndexId_Renamed = cdsIndexId;
		  return this;
		}

		/// <summary>
		/// Sets the legal entity identifiers.
		/// <para>
		/// These identifiers refer to the reference legal entities of the CDS index.
		/// </para>
		/// </summary>
		/// <param name="legalEntityIds">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder legalEntityIds(IList<StandardId> legalEntityIds)
		{
		  JodaBeanUtils.notNull(legalEntityIds, "legalEntityIds");
		  this.legalEntityIds_Renamed = legalEntityIds;
		  return this;
		}

		/// <summary>
		/// Sets the {@code legalEntityIds} property in the builder
		/// from an array of objects. </summary>
		/// <param name="legalEntityIds">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder legalEntityIds(params StandardId[] legalEntityIds)
		{
		  return this.legalEntityIds(ImmutableList.copyOf(legalEntityIds));
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
		/// It is usually 1 calendar day for standardized CDS index contracts.
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
		/// It is usually 3 business days for standardized CDS index contracts.
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
		  StringBuilder buf = new StringBuilder(352);
		  buf.Append("ResolvedCdsIndex.Builder{");
		  buf.Append("buySell").Append('=').Append(JodaBeanUtils.ToString(buySell_Renamed)).Append(',').Append(' ');
		  buf.Append("cdsIndexId").Append('=').Append(JodaBeanUtils.ToString(cdsIndexId_Renamed)).Append(',').Append(' ');
		  buf.Append("legalEntityIds").Append('=').Append(JodaBeanUtils.ToString(legalEntityIds_Renamed)).Append(',').Append(' ');
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