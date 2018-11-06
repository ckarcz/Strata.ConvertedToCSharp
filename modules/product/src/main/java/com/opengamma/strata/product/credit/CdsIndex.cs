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
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Resolvable = com.opengamma.strata.basics.Resolvable;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using Schedule = com.opengamma.strata.basics.schedule.Schedule;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// A CDS (portfolio) index product. 
	/// <para>
	/// A CDS index is a portofolio of single name credit default swaps. 
	/// The protection buyer periodically pays fixed coupons to the protection seller until the expiry, 
	/// and in return, the protection buyer receives the bond of a defaulted constituent legal entity for par.  
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class CdsIndex implements com.opengamma.strata.product.Product, com.opengamma.strata.basics.Resolvable<ResolvedCdsIndex>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CdsIndex : Product, Resolvable<ResolvedCdsIndex>, ImmutableBean
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
	  /// The currency of the CDS index.
	  /// <para>
	  /// The amounts of the notional are expressed in terms of this currency.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Currency currency;
	  private readonly Currency currency;
	  /// <summary>
	  /// The notional amount, must be non-negative.
	  /// <para>
	  /// The fixed notional amount applicable during the lifetime of the CDS.
	  /// The currency of the notional is specified by {@code currency}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegativeOrZero") private final double notional;
	  private readonly double notional;
	  /// <summary>
	  /// The payment schedule.
	  /// <para>
	  /// This is used to define the payment periods.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.schedule.PeriodicSchedule paymentSchedule;
	  private readonly PeriodicSchedule paymentSchedule;
	  /// <summary>
	  /// The fixed coupon rate.
	  /// <para>
	  /// This must be represented in decimal form.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative") private final double fixedRate;
	  private readonly double fixedRate;
	  /// <summary>
	  /// The day count convention.
	  /// <para>
	  /// This is used to convert dates to a numerical value.
	  /// </para>
	  /// <para>
	  /// When building, this will default to 'Act/360'.
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
	  /// <para>
	  /// When building, this will default to 'AccruedPremium'.
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
	  /// <para>
	  /// When building, this will default to 'Beginning'.
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
	  /// <para>
	  /// When building, this will default to 1 calendar day.
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
	  /// <para>
	  /// When building, this will default to 3 business days in the calendar of the payment schedule.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DaysAdjustment settlementDateOffset;
	  private readonly DaysAdjustment settlementDateOffset;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance of a standardized CDS index.
	  /// </summary>
	  /// <param name="buySell">  buy or sell </param>
	  /// <param name="cdsIndexId">  the CDS index ID </param>
	  /// <param name="legalEntityIds">  the legal entity IDs </param>
	  /// <param name="currency">  the currency </param>
	  /// <param name="notional">  the notional </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="calendar">  the calendar </param>
	  /// <param name="paymentFrequency">  the payment frequency </param>
	  /// <param name="fixedRate">  the fixed coupon rate </param>
	  /// <returns> the instance </returns>
	  public static CdsIndex of(BuySell buySell, StandardId cdsIndexId, IList<StandardId> legalEntityIds, Currency currency, double notional, LocalDate startDate, LocalDate endDate, Frequency paymentFrequency, HolidayCalendarId calendar, double fixedRate)
	  {

		PeriodicSchedule accrualSchedule = PeriodicSchedule.builder().businessDayAdjustment(BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, calendar)).startDate(startDate).endDate(endDate).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).endDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).frequency(paymentFrequency).rollConvention(RollConventions.NONE).stubConvention(StubConvention.SMART_INITIAL).build();
		return new CdsIndex(buySell, cdsIndexId, legalEntityIds, currency, notional, accrualSchedule, fixedRate, DayCounts.ACT_360, PaymentOnDefault.ACCRUED_PREMIUM, ProtectionStartOfDay.BEGINNING, DaysAdjustment.ofCalendarDays(1), DaysAdjustment.ofBusinessDays(3, calendar));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.dayCount_Renamed = DayCounts.ACT_360;
		builder.paymentOnDefault_Renamed = PaymentOnDefault.ACCRUED_PREMIUM;
		builder.protectionStart_Renamed = ProtectionStartOfDay.BEGINNING;
		builder.stepinDateOffset_Renamed = DaysAdjustment.ofCalendarDays(1);
	  }

	  //-------------------------------------------------------------------------
	  public ImmutableSet<Currency> allCurrencies()
	  {
		return ImmutableSet.of(currency);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (builder.settlementDateOffset_Renamed == null && builder.paymentSchedule_Renamed != null)
		{
		  builder.settlementDateOffset_Renamed = DaysAdjustment.ofBusinessDays(3, builder.paymentSchedule_Renamed.BusinessDayAdjustment.Calendar);
		}
	  }

	  //-------------------------------------------------------------------------
	  public ResolvedCdsIndex resolve(ReferenceData refData)
	  {
		Schedule adjustedSchedule = paymentSchedule.createSchedule(refData);
		ImmutableList.Builder<CreditCouponPaymentPeriod> accrualPeriods = ImmutableList.builder();
		int nPeriods = adjustedSchedule.size();
		for (int i = 0; i < nPeriods - 1; i++)
		{
		  SchedulePeriod period = adjustedSchedule.getPeriod(i);
		  accrualPeriods.add(CreditCouponPaymentPeriod.builder().startDate(period.StartDate).endDate(period.EndDate).unadjustedStartDate(period.UnadjustedStartDate).unadjustedEndDate(period.UnadjustedEndDate).effectiveStartDate(protectionStart.Beginning ? period.StartDate.minusDays(1) : period.StartDate).effectiveEndDate(protectionStart.Beginning ? period.EndDate.minusDays(1) : period.EndDate).paymentDate(period.EndDate).notional(notional).currency(currency).fixedRate(fixedRate).yearFraction(period.yearFraction(dayCount, adjustedSchedule)).build());
		}
		// last period - accrual end is modified
		SchedulePeriod lastPeriod = adjustedSchedule.getPeriod(nPeriods - 1);
		LocalDate accEndDate = protectionStart.Beginning ? lastPeriod.EndDate.plusDays(1) : lastPeriod.EndDate;
		SchedulePeriod modifiedPeriod = lastPeriod.toBuilder().endDate(accEndDate).build();
		accrualPeriods.add(CreditCouponPaymentPeriod.builder().startDate(modifiedPeriod.StartDate).endDate(modifiedPeriod.EndDate).unadjustedStartDate(modifiedPeriod.UnadjustedStartDate).unadjustedEndDate(modifiedPeriod.UnadjustedEndDate).effectiveStartDate(protectionStart.Beginning ? lastPeriod.StartDate.minusDays(1) : lastPeriod.StartDate).effectiveEndDate(lastPeriod.EndDate).paymentDate(paymentSchedule.BusinessDayAdjustment.adjust(lastPeriod.EndDate, refData)).notional(notional).currency(currency).fixedRate(fixedRate).yearFraction(modifiedPeriod.yearFraction(dayCount, adjustedSchedule)).build());
		ImmutableList<CreditCouponPaymentPeriod> paymentPeriods = accrualPeriods.build();

		return ResolvedCdsIndex.builder().buySell(buySell).cdsIndexId(cdsIndexId).legalEntityIds(legalEntityIds).protectionStart(protectionStart).paymentOnDefault(paymentOnDefault).paymentPeriods(paymentPeriods).protectionEndDate(lastPeriod.EndDate).settlementDateOffset(settlementDateOffset).stepinDateOffset(stepinDateOffset).dayCount(dayCount).build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CdsIndex}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CdsIndex.Meta meta()
	  {
		return CdsIndex.Meta.INSTANCE;
	  }

	  static CdsIndex()
	  {
		MetaBean.register(CdsIndex.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static CdsIndex.Builder builder()
	  {
		return new CdsIndex.Builder();
	  }

	  private CdsIndex(BuySell buySell, StandardId cdsIndexId, IList<StandardId> legalEntityIds, Currency currency, double notional, PeriodicSchedule paymentSchedule, double fixedRate, DayCount dayCount, PaymentOnDefault paymentOnDefault, ProtectionStartOfDay protectionStart, DaysAdjustment stepinDateOffset, DaysAdjustment settlementDateOffset)
	  {
		JodaBeanUtils.notNull(buySell, "buySell");
		JodaBeanUtils.notNull(cdsIndexId, "cdsIndexId");
		JodaBeanUtils.notNull(legalEntityIds, "legalEntityIds");
		JodaBeanUtils.notNull(currency, "currency");
		ArgChecker.notNegativeOrZero(notional, "notional");
		JodaBeanUtils.notNull(paymentSchedule, "paymentSchedule");
		ArgChecker.notNegative(fixedRate, "fixedRate");
		JodaBeanUtils.notNull(dayCount, "dayCount");
		JodaBeanUtils.notNull(paymentOnDefault, "paymentOnDefault");
		JodaBeanUtils.notNull(protectionStart, "protectionStart");
		JodaBeanUtils.notNull(stepinDateOffset, "stepinDateOffset");
		JodaBeanUtils.notNull(settlementDateOffset, "settlementDateOffset");
		this.buySell = buySell;
		this.cdsIndexId = cdsIndexId;
		this.legalEntityIds = ImmutableList.copyOf(legalEntityIds);
		this.currency = currency;
		this.notional = notional;
		this.paymentSchedule = paymentSchedule;
		this.fixedRate = fixedRate;
		this.dayCount = dayCount;
		this.paymentOnDefault = paymentOnDefault;
		this.protectionStart = protectionStart;
		this.stepinDateOffset = stepinDateOffset;
		this.settlementDateOffset = settlementDateOffset;
	  }

	  public override CdsIndex.Meta metaBean()
	  {
		return CdsIndex.Meta.INSTANCE;
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
	  /// Gets the currency of the CDS index.
	  /// <para>
	  /// The amounts of the notional are expressed in terms of this currency.
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
	  /// Gets the notional amount, must be non-negative.
	  /// <para>
	  /// The fixed notional amount applicable during the lifetime of the CDS.
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
	  /// Gets the payment schedule.
	  /// <para>
	  /// This is used to define the payment periods.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PeriodicSchedule PaymentSchedule
	  {
		  get
		  {
			return paymentSchedule;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the fixed coupon rate.
	  /// <para>
	  /// This must be represented in decimal form.
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
	  /// Gets the day count convention.
	  /// <para>
	  /// This is used to convert dates to a numerical value.
	  /// </para>
	  /// <para>
	  /// When building, this will default to 'Act/360'.
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
	  /// <para>
	  /// When building, this will default to 'AccruedPremium'.
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
	  /// <para>
	  /// When building, this will default to 'Beginning'.
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
	  /// <para>
	  /// When building, this will default to 1 calendar day.
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
	  /// <para>
	  /// When building, this will default to 3 business days in the calendar of the payment schedule.
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
		  CdsIndex other = (CdsIndex) obj;
		  return JodaBeanUtils.equal(buySell, other.buySell) && JodaBeanUtils.equal(cdsIndexId, other.cdsIndexId) && JodaBeanUtils.equal(legalEntityIds, other.legalEntityIds) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(notional, other.notional) && JodaBeanUtils.equal(paymentSchedule, other.paymentSchedule) && JodaBeanUtils.equal(fixedRate, other.fixedRate) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(paymentOnDefault, other.paymentOnDefault) && JodaBeanUtils.equal(protectionStart, other.protectionStart) && JodaBeanUtils.equal(stepinDateOffset, other.stepinDateOffset) && JodaBeanUtils.equal(settlementDateOffset, other.settlementDateOffset);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(buySell);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(cdsIndexId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(legalEntityIds);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(notional);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentSchedule);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixedRate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentOnDefault);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(protectionStart);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(stepinDateOffset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(settlementDateOffset);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(416);
		buf.Append("CdsIndex{");
		buf.Append("buySell").Append('=').Append(buySell).Append(',').Append(' ');
		buf.Append("cdsIndexId").Append('=').Append(cdsIndexId).Append(',').Append(' ');
		buf.Append("legalEntityIds").Append('=').Append(legalEntityIds).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("notional").Append('=').Append(notional).Append(',').Append(' ');
		buf.Append("paymentSchedule").Append('=').Append(paymentSchedule).Append(',').Append(' ');
		buf.Append("fixedRate").Append('=').Append(fixedRate).Append(',').Append(' ');
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
	  /// The meta-bean for {@code CdsIndex}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  buySell_Renamed = DirectMetaProperty.ofImmutable(this, "buySell", typeof(CdsIndex), typeof(BuySell));
			  cdsIndexId_Renamed = DirectMetaProperty.ofImmutable(this, "cdsIndexId", typeof(CdsIndex), typeof(StandardId));
			  legalEntityIds_Renamed = DirectMetaProperty.ofImmutable(this, "legalEntityIds", typeof(CdsIndex), (Type) typeof(ImmutableList));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(CdsIndex), typeof(Currency));
			  notional_Renamed = DirectMetaProperty.ofImmutable(this, "notional", typeof(CdsIndex), Double.TYPE);
			  paymentSchedule_Renamed = DirectMetaProperty.ofImmutable(this, "paymentSchedule", typeof(CdsIndex), typeof(PeriodicSchedule));
			  fixedRate_Renamed = DirectMetaProperty.ofImmutable(this, "fixedRate", typeof(CdsIndex), Double.TYPE);
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(CdsIndex), typeof(DayCount));
			  paymentOnDefault_Renamed = DirectMetaProperty.ofImmutable(this, "paymentOnDefault", typeof(CdsIndex), typeof(PaymentOnDefault));
			  protectionStart_Renamed = DirectMetaProperty.ofImmutable(this, "protectionStart", typeof(CdsIndex), typeof(ProtectionStartOfDay));
			  stepinDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "stepinDateOffset", typeof(CdsIndex), typeof(DaysAdjustment));
			  settlementDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "settlementDateOffset", typeof(CdsIndex), typeof(DaysAdjustment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "buySell", "cdsIndexId", "legalEntityIds", "currency", "notional", "paymentSchedule", "fixedRate", "dayCount", "paymentOnDefault", "protectionStart", "stepinDateOffset", "settlementDateOffset");
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
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<com.opengamma.strata.basics.StandardId>> legalEntityIds = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "legalEntityIds", CdsIndex.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<StandardId>> legalEntityIds_Renamed;
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
		/// The meta-property for the {@code paymentSchedule} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PeriodicSchedule> paymentSchedule_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixedRate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> fixedRate_Renamed;
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
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "buySell", "cdsIndexId", "legalEntityIds", "currency", "notional", "paymentSchedule", "fixedRate", "dayCount", "paymentOnDefault", "protectionStart", "stepinDateOffset", "settlementDateOffset");
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
			case 575402001: // currency
			  return currency_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case -1499086147: // paymentSchedule
			  return paymentSchedule_Renamed;
			case 747425396: // fixedRate
			  return fixedRate_Renamed;
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

		public override CdsIndex.Builder builder()
		{
		  return new CdsIndex.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CdsIndex);
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
		/// The meta-property for the {@code paymentSchedule} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PeriodicSchedule> paymentSchedule()
		{
		  return paymentSchedule_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixedRate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> fixedRate()
		{
		  return fixedRate_Renamed;
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
			  return ((CdsIndex) bean).BuySell;
			case -464117509: // cdsIndexId
			  return ((CdsIndex) bean).CdsIndexId;
			case 1085098268: // legalEntityIds
			  return ((CdsIndex) bean).LegalEntityIds;
			case 575402001: // currency
			  return ((CdsIndex) bean).Currency;
			case 1585636160: // notional
			  return ((CdsIndex) bean).Notional;
			case -1499086147: // paymentSchedule
			  return ((CdsIndex) bean).PaymentSchedule;
			case 747425396: // fixedRate
			  return ((CdsIndex) bean).FixedRate;
			case 1905311443: // dayCount
			  return ((CdsIndex) bean).DayCount;
			case -480203780: // paymentOnDefault
			  return ((CdsIndex) bean).PaymentOnDefault;
			case 2103482633: // protectionStart
			  return ((CdsIndex) bean).ProtectionStart;
			case 852621746: // stepinDateOffset
			  return ((CdsIndex) bean).StepinDateOffset;
			case 135924714: // settlementDateOffset
			  return ((CdsIndex) bean).SettlementDateOffset;
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
	  /// The bean-builder for {@code CdsIndex}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<CdsIndex>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BuySell buySell_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal StandardId cdsIndexId_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<StandardId> legalEntityIds_Renamed = ImmutableList.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Currency currency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double notional_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PeriodicSchedule paymentSchedule_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double fixedRate_Renamed;
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
		  applyDefaults(this);
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(CdsIndex beanToCopy)
		{
		  this.buySell_Renamed = beanToCopy.BuySell;
		  this.cdsIndexId_Renamed = beanToCopy.CdsIndexId;
		  this.legalEntityIds_Renamed = beanToCopy.LegalEntityIds;
		  this.currency_Renamed = beanToCopy.Currency;
		  this.notional_Renamed = beanToCopy.Notional;
		  this.paymentSchedule_Renamed = beanToCopy.PaymentSchedule;
		  this.fixedRate_Renamed = beanToCopy.FixedRate;
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
			case 575402001: // currency
			  return currency_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case -1499086147: // paymentSchedule
			  return paymentSchedule_Renamed;
			case 747425396: // fixedRate
			  return fixedRate_Renamed;
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
			case 575402001: // currency
			  this.currency_Renamed = (Currency) newValue;
			  break;
			case 1585636160: // notional
			  this.notional_Renamed = (double?) newValue.Value;
			  break;
			case -1499086147: // paymentSchedule
			  this.paymentSchedule_Renamed = (PeriodicSchedule) newValue;
			  break;
			case 747425396: // fixedRate
			  this.fixedRate_Renamed = (double?) newValue.Value;
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

		public override CdsIndex build()
		{
		  preBuild(this);
		  return new CdsIndex(buySell_Renamed, cdsIndexId_Renamed, legalEntityIds_Renamed, currency_Renamed, notional_Renamed, paymentSchedule_Renamed, fixedRate_Renamed, dayCount_Renamed, paymentOnDefault_Renamed, protectionStart_Renamed, stepinDateOffset_Renamed, settlementDateOffset_Renamed);
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
		/// Sets the currency of the CDS index.
		/// <para>
		/// The amounts of the notional are expressed in terms of this currency.
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
		/// Sets the notional amount, must be non-negative.
		/// <para>
		/// The fixed notional amount applicable during the lifetime of the CDS.
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
		/// Sets the payment schedule.
		/// <para>
		/// This is used to define the payment periods.
		/// </para>
		/// </summary>
		/// <param name="paymentSchedule">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentSchedule(PeriodicSchedule paymentSchedule)
		{
		  JodaBeanUtils.notNull(paymentSchedule, "paymentSchedule");
		  this.paymentSchedule_Renamed = paymentSchedule;
		  return this;
		}

		/// <summary>
		/// Sets the fixed coupon rate.
		/// <para>
		/// This must be represented in decimal form.
		/// </para>
		/// </summary>
		/// <param name="fixedRate">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixedRate(double fixedRate)
		{
		  ArgChecker.notNegative(fixedRate, "fixedRate");
		  this.fixedRate_Renamed = fixedRate;
		  return this;
		}

		/// <summary>
		/// Sets the day count convention.
		/// <para>
		/// This is used to convert dates to a numerical value.
		/// </para>
		/// <para>
		/// When building, this will default to 'Act/360'.
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
		/// <para>
		/// When building, this will default to 'AccruedPremium'.
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
		/// <para>
		/// When building, this will default to 'Beginning'.
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
		/// <para>
		/// When building, this will default to 1 calendar day.
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
		/// <para>
		/// When building, this will default to 3 business days in the calendar of the payment schedule.
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
		  StringBuilder buf = new StringBuilder(416);
		  buf.Append("CdsIndex.Builder{");
		  buf.Append("buySell").Append('=').Append(JodaBeanUtils.ToString(buySell_Renamed)).Append(',').Append(' ');
		  buf.Append("cdsIndexId").Append('=').Append(JodaBeanUtils.ToString(cdsIndexId_Renamed)).Append(',').Append(' ');
		  buf.Append("legalEntityIds").Append('=').Append(JodaBeanUtils.ToString(legalEntityIds_Renamed)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("notional").Append('=').Append(JodaBeanUtils.ToString(notional_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentSchedule").Append('=').Append(JodaBeanUtils.ToString(paymentSchedule_Renamed)).Append(',').Append(' ');
		  buf.Append("fixedRate").Append('=').Append(JodaBeanUtils.ToString(fixedRate_Renamed)).Append(',').Append(' ');
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