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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.ensureOnlyOne;


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

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DayCount_ScheduleInfo = com.opengamma.strata.basics.date.DayCount_ScheduleInfo;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using RollConvention = com.opengamma.strata.basics.schedule.RollConvention;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using InflationRateCalculation = com.opengamma.strata.product.swap.InflationRateCalculation;

	/// <summary>
	/// A capital indexed bond.
	/// <para>
	/// This is the resolved form of <seealso cref="CapitalIndexedBond"/> and is an input to the pricers.
	/// Applications will typically create a {@code ResolvedCapitalIndexedBond} from a {@code CapitalIndexedBond}
	/// using <seealso cref="CapitalIndexedBond#resolve(ReferenceData)"/>.
	/// </para>
	/// <para>
	/// The periodic coupon payments are defined in {@code periodicPayments},
	/// whereas {@code nominalPayment} separately represents the nominal payments.
	/// </para>
	/// <para>
	/// The legal entity of this bond is identified by {@code legalEntityId}.
	/// The enum, {@code yieldConvention}, specifies the yield computation convention.
	/// The accrued interest must be computed with {@code dayCount}.
	/// </para>
	/// <para>
	/// A {@code ResolvedCapitalIndexedBond} is bound to data that changes over time, such as holiday calendars.
	/// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// 
	/// <h4>Price</h4>
	/// Strata uses <i>decimal prices</i> for bonds in the trade model, pricers and market data.
	/// For example, a price of 99.32% is represented in Strata by 0.9932.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ResolvedCapitalIndexedBond implements com.opengamma.strata.product.ResolvedProduct, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ResolvedCapitalIndexedBond : ResolvedProduct, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.SecurityId securityId;
		private readonly SecurityId securityId;
	  /// <summary>
	  /// The nominal payment of the product.
	  /// <para>
	  /// The payment date of the nominal payment agrees with the final coupon payment date of the periodic payments.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final CapitalIndexedBondPaymentPeriod nominalPayment;
	  private readonly CapitalIndexedBondPaymentPeriod nominalPayment;
	  /// <summary>
	  /// The periodic payments of the product.
	  /// <para>
	  /// Each payment period represents part of the life-time of the product.
	  /// The start date and end date of the leg are determined from the first and last period.
	  /// As such, the periods should be sorted.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableList<CapitalIndexedBondPaymentPeriod> periodicPayments;
	  private readonly ImmutableList<CapitalIndexedBondPaymentPeriod> periodicPayments;
	  /// <summary>
	  /// The frequency of the bond payments.
	  /// <para>
	  /// This must match the frequency used to generate the payment schedule.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.schedule.Frequency frequency;
	  private readonly Frequency frequency;
	  /// <summary>
	  /// The roll convention of the bond payments.
	  /// <para>
	  /// This must match the convention used to generate the payment schedule.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.schedule.RollConvention rollConvention;
	  private readonly RollConvention rollConvention;
	  /// <summary>
	  /// The day count convention applicable.
	  /// <para>
	  /// The conversion from dates to a numerical value is made based on this day count.
	  /// For the inflation-indexed bond, the day count convention is used to compute accrued interest.
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
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.LegalEntityId legalEntityId;
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

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		Currency currencyNominal = nominalPayment.Currency;
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ISet<Currency> currencies = periodicPayments.Select(CapitalIndexedBondPaymentPeriod::getCurrency).collect(Collectors.toSet());
		currencies.Add(currencyNominal);
		ArgChecker.isTrue(currencies.Count == 1, "Product must have a single currency, found: " + currencies);
		ArgChecker.isTrue(rateCalculation.FirstIndexValue.HasValue, "Rate calculation must specify first index value");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the start date of the product.
	  /// <para>
	  /// This is the first coupon period date of the bond, often known as the effective date.
	  /// This date has been adjusted to be a valid business day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the start date </returns>
	  public LocalDate StartDate
	  {
		  get
		  {
			return periodicPayments.get(0).StartDate;
		  }
	  }

	  /// <summary>
	  /// Gets the end date of the product.
	  /// <para>
	  /// This is the last coupon period date of the bond, often known as the maturity date.
	  /// This date has been adjusted to be a valid business day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the end date </returns>
	  public LocalDate EndDate
	  {
		  get
		  {
			return periodicPayments.get(periodicPayments.size() - 1).EndDate;
		  }
	  }

	  /// <summary>
	  /// The unadjusted start date.
	  /// <para>
	  /// This is the unadjusted first coupon period date of the bond.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the unadjusted start date </returns>
	  public LocalDate UnadjustedStartDate
	  {
		  get
		  {
			return periodicPayments.get(0).UnadjustedStartDate;
		  }
	  }

	  /// <summary>
	  /// The unadjusted end date.
	  /// <para>
	  /// This is the unadjusted last coupon period date of the bond.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the unadjusted end date </returns>
	  public LocalDate UnadjustedEndDate
	  {
		  get
		  {
			return periodicPayments.get(periodicPayments.size() - 1).UnadjustedEndDate;
		  }
	  }

	  /// <summary>
	  /// Gets the currency of the product.
	  /// <para>
	  /// All payments in the bond will have this currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency </returns>
	  public Currency Currency
	  {
		  get
		  {
			return nominalPayment.Currency;
		  }
	  }

	  /// <summary>
	  /// Gets the notional amount, must be positive.
	  /// <para>
	  /// The notional expressed here must be positive.
	  /// The currency of the notional is specified by <seealso cref="#getCurrency()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the notional amount </returns>
	  public double Notional
	  {
		  get
		  {
			return periodicPayments.get(0).Notional;
		  }
	  }

	  /// <summary>
	  /// Checks if there is an ex-coupon period.
	  /// </summary>
	  /// <returns> true if has an ex-coupon period </returns>
	  public bool hasExCouponPeriod()
	  {
		return periodicPayments.get(0).hasExCouponPeriod();
	  }

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
	  public Optional<CapitalIndexedBondPaymentPeriod> findPeriod(LocalDate date)
	  {
		return periodicPayments.Where(p => p.contains(date)).Aggregate(ensureOnlyOne());
	  }

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
	  public int? findPeriodIndex(LocalDate date)
	  {
		for (int i = 0; i < periodicPayments.size(); i++)
		{
		  if (periodicPayments.get(i).contains(date))
		  {
			return int?.of(i);
		  }
		}
		return int?.empty();
	  }

	  /// <summary>
	  /// Calculates the year fraction within the specified period.
	  /// <para>
	  /// Year fractions on bonds are calculated on unadjusted dates.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <returns> the year fraction </returns>
	  /// <exception cref="IllegalArgumentException"> if the dates are outside the range of the bond or start is after end </exception>
	  public double yearFraction(LocalDate startDate, LocalDate endDate)
	  {
		return yearFraction(startDate, endDate, dayCount);
	  }

	  /// <summary>
	  /// Calculates the year fraction within the specified period and day count.
	  /// <para>
	  /// Year fractions on bonds are calculated on unadjusted dates.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="dayCount"> the day count </param>
	  /// <returns> the year fraction </returns>
	  /// <exception cref="IllegalArgumentException"> if the dates are outside the range of the bond or start is after end </exception>
	  public double yearFraction(LocalDate startDate, LocalDate endDate, DayCount dayCount)
	  {
		ArgChecker.inOrderOrEqual(UnadjustedStartDate, startDate, "bond.unadjustedStartDate", "startDate");
		ArgChecker.inOrderOrEqual(startDate, endDate, "startDate", "endDate");
		ArgChecker.inOrderOrEqual(endDate, UnadjustedEndDate, "endDate", "bond.unadjustedEndDate");
		// bond day counts often need ScheduleInfo
		DayCount_ScheduleInfo info = new DayCount_ScheduleInfoAnonymousInnerClass(this);
		return dayCount.yearFraction(startDate, endDate, info);
	  }

	  private class DayCount_ScheduleInfoAnonymousInnerClass : DayCount_ScheduleInfo
	  {
		  private readonly ResolvedCapitalIndexedBond outerInstance;

		  public DayCount_ScheduleInfoAnonymousInnerClass(ResolvedCapitalIndexedBond outerInstance)
		  {
			  this.outerInstance = outerInstance;
		  }

		  public LocalDate StartDate
		  {
			  get
			  {
				return outerInstance.UnadjustedStartDate;
			  }
		  }

		  public LocalDate EndDate
		  {
			  get
			  {
				return outerInstance.UnadjustedEndDate;
			  }
		  }

		  public Frequency Frequency
		  {
			  get
			  {
				return outerInstance.frequency;
			  }
		  }

		  public LocalDate getPeriodEndDate(LocalDate date)
		  {
			return outerInstance.periodicPayments.Where(p => p.contains(date)).Select(p => p.UnadjustedEndDate).First().orElseThrow(() => new System.ArgumentException("Date is not contained in any period"));
		  }

		  public bool EndOfMonthConvention
		  {
			  get
			  {
				return outerInstance.rollConvention == RollConventions.EOM;
			  }
		  }
	  }

	  //-------------------------------------------------------------------------
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
	  /// Calculates the accrued interest of the bond with the specified date.
	  /// </summary>
	  /// <param name="referenceDate">  the reference date </param>
	  /// <returns> the accrued interest of the product  </returns>
	  public double accruedInterest(LocalDate referenceDate)
	  {
		if (UnadjustedStartDate.isAfter(referenceDate))
		{
		  return 0d;
		}
		double notional = Notional;
		CapitalIndexedBondPaymentPeriod period = findPeriod(referenceDate).orElseThrow(() => new System.ArgumentException("Date outside range of bond"));
		LocalDate previousAccrualDate = period.UnadjustedStartDate;
		double realCoupon = period.RealCoupon;
		double couponPerYear = Frequency.eventsPerYear();
		double rate = realCoupon * couponPerYear;
		double accruedInterest = yieldConvention.Equals(CapitalIndexedBondYieldConvention.JP_IL_COMPOUND) || yieldConvention.Equals(CapitalIndexedBondYieldConvention.JP_IL_SIMPLE) ? yearFraction(previousAccrualDate, referenceDate, DayCounts.ACT_365F) * rate * notional : yearFraction(previousAccrualDate, referenceDate) * rate * notional;
		double result = 0d;
		if (hasExCouponPeriod() && !referenceDate.isBefore(period.DetachmentDate))
		{
		  result = accruedInterest - notional * rate * yearFraction(previousAccrualDate, period.UnadjustedEndDate);
		}
		else
		{
		  result = accruedInterest;
		}
		return result;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedCapitalIndexedBond}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ResolvedCapitalIndexedBond.Meta meta()
	  {
		return ResolvedCapitalIndexedBond.Meta.INSTANCE;
	  }

	  static ResolvedCapitalIndexedBond()
	  {
		MetaBean.register(ResolvedCapitalIndexedBond.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ResolvedCapitalIndexedBond.Builder builder()
	  {
		return new ResolvedCapitalIndexedBond.Builder();
	  }

	  private ResolvedCapitalIndexedBond(SecurityId securityId, CapitalIndexedBondPaymentPeriod nominalPayment, IList<CapitalIndexedBondPaymentPeriod> periodicPayments, Frequency frequency, RollConvention rollConvention, DayCount dayCount, CapitalIndexedBondYieldConvention yieldConvention, LegalEntityId legalEntityId, DaysAdjustment settlementDateOffset, InflationRateCalculation rateCalculation)
	  {
		JodaBeanUtils.notNull(securityId, "securityId");
		JodaBeanUtils.notNull(nominalPayment, "nominalPayment");
		JodaBeanUtils.notNull(periodicPayments, "periodicPayments");
		JodaBeanUtils.notNull(frequency, "frequency");
		JodaBeanUtils.notNull(rollConvention, "rollConvention");
		JodaBeanUtils.notNull(dayCount, "dayCount");
		JodaBeanUtils.notNull(yieldConvention, "yieldConvention");
		JodaBeanUtils.notNull(legalEntityId, "legalEntityId");
		JodaBeanUtils.notNull(settlementDateOffset, "settlementDateOffset");
		JodaBeanUtils.notNull(rateCalculation, "rateCalculation");
		this.securityId = securityId;
		this.nominalPayment = nominalPayment;
		this.periodicPayments = ImmutableList.copyOf(periodicPayments);
		this.frequency = frequency;
		this.rollConvention = rollConvention;
		this.dayCount = dayCount;
		this.yieldConvention = yieldConvention;
		this.legalEntityId = legalEntityId;
		this.settlementDateOffset = settlementDateOffset;
		this.rateCalculation = rateCalculation;
		validate();
	  }

	  public override ResolvedCapitalIndexedBond.Meta metaBean()
	  {
		return ResolvedCapitalIndexedBond.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the security identifier.
	  /// <para>
	  /// This identifier uniquely identifies the security within the system.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SecurityId SecurityId
	  {
		  get
		  {
			return securityId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the nominal payment of the product.
	  /// <para>
	  /// The payment date of the nominal payment agrees with the final coupon payment date of the periodic payments.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CapitalIndexedBondPaymentPeriod NominalPayment
	  {
		  get
		  {
			return nominalPayment;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the periodic payments of the product.
	  /// <para>
	  /// Each payment period represents part of the life-time of the product.
	  /// The start date and end date of the leg are determined from the first and last period.
	  /// As such, the periods should be sorted.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<CapitalIndexedBondPaymentPeriod> PeriodicPayments
	  {
		  get
		  {
			return periodicPayments;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the frequency of the bond payments.
	  /// <para>
	  /// This must match the frequency used to generate the payment schedule.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Frequency Frequency
	  {
		  get
		  {
			return frequency;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the roll convention of the bond payments.
	  /// <para>
	  /// This must match the convention used to generate the payment schedule.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public RollConvention RollConvention
	  {
		  get
		  {
			return rollConvention;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the day count convention applicable.
	  /// <para>
	  /// The conversion from dates to a numerical value is made based on this day count.
	  /// For the inflation-indexed bond, the day count convention is used to compute accrued interest.
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
		  ResolvedCapitalIndexedBond other = (ResolvedCapitalIndexedBond) obj;
		  return JodaBeanUtils.equal(securityId, other.securityId) && JodaBeanUtils.equal(nominalPayment, other.nominalPayment) && JodaBeanUtils.equal(periodicPayments, other.periodicPayments) && JodaBeanUtils.equal(frequency, other.frequency) && JodaBeanUtils.equal(rollConvention, other.rollConvention) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(yieldConvention, other.yieldConvention) && JodaBeanUtils.equal(legalEntityId, other.legalEntityId) && JodaBeanUtils.equal(settlementDateOffset, other.settlementDateOffset) && JodaBeanUtils.equal(rateCalculation, other.rateCalculation);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(securityId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(nominalPayment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(periodicPayments);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(frequency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rollConvention);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(yieldConvention);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(legalEntityId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(settlementDateOffset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rateCalculation);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(352);
		buf.Append("ResolvedCapitalIndexedBond{");
		buf.Append("securityId").Append('=').Append(securityId).Append(',').Append(' ');
		buf.Append("nominalPayment").Append('=').Append(nominalPayment).Append(',').Append(' ');
		buf.Append("periodicPayments").Append('=').Append(periodicPayments).Append(',').Append(' ');
		buf.Append("frequency").Append('=').Append(frequency).Append(',').Append(' ');
		buf.Append("rollConvention").Append('=').Append(rollConvention).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("yieldConvention").Append('=').Append(yieldConvention).Append(',').Append(' ');
		buf.Append("legalEntityId").Append('=').Append(legalEntityId).Append(',').Append(' ');
		buf.Append("settlementDateOffset").Append('=').Append(settlementDateOffset).Append(',').Append(' ');
		buf.Append("rateCalculation").Append('=').Append(JodaBeanUtils.ToString(rateCalculation));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedCapitalIndexedBond}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  securityId_Renamed = DirectMetaProperty.ofImmutable(this, "securityId", typeof(ResolvedCapitalIndexedBond), typeof(SecurityId));
			  nominalPayment_Renamed = DirectMetaProperty.ofImmutable(this, "nominalPayment", typeof(ResolvedCapitalIndexedBond), typeof(CapitalIndexedBondPaymentPeriod));
			  periodicPayments_Renamed = DirectMetaProperty.ofImmutable(this, "periodicPayments", typeof(ResolvedCapitalIndexedBond), (Type) typeof(ImmutableList));
			  frequency_Renamed = DirectMetaProperty.ofImmutable(this, "frequency", typeof(ResolvedCapitalIndexedBond), typeof(Frequency));
			  rollConvention_Renamed = DirectMetaProperty.ofImmutable(this, "rollConvention", typeof(ResolvedCapitalIndexedBond), typeof(RollConvention));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(ResolvedCapitalIndexedBond), typeof(DayCount));
			  yieldConvention_Renamed = DirectMetaProperty.ofImmutable(this, "yieldConvention", typeof(ResolvedCapitalIndexedBond), typeof(CapitalIndexedBondYieldConvention));
			  legalEntityId_Renamed = DirectMetaProperty.ofImmutable(this, "legalEntityId", typeof(ResolvedCapitalIndexedBond), typeof(LegalEntityId));
			  settlementDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "settlementDateOffset", typeof(ResolvedCapitalIndexedBond), typeof(DaysAdjustment));
			  rateCalculation_Renamed = DirectMetaProperty.ofImmutable(this, "rateCalculation", typeof(ResolvedCapitalIndexedBond), typeof(InflationRateCalculation));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "securityId", "nominalPayment", "periodicPayments", "frequency", "rollConvention", "dayCount", "yieldConvention", "legalEntityId", "settlementDateOffset", "rateCalculation");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code securityId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SecurityId> securityId_Renamed;
		/// <summary>
		/// The meta-property for the {@code nominalPayment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CapitalIndexedBondPaymentPeriod> nominalPayment_Renamed;
		/// <summary>
		/// The meta-property for the {@code periodicPayments} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<CapitalIndexedBondPaymentPeriod>> periodicPayments = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "periodicPayments", ResolvedCapitalIndexedBond.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<CapitalIndexedBondPaymentPeriod>> periodicPayments_Renamed;
		/// <summary>
		/// The meta-property for the {@code frequency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Frequency> frequency_Renamed;
		/// <summary>
		/// The meta-property for the {@code rollConvention} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<RollConvention> rollConvention_Renamed;
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
		/// The meta-property for the {@code rateCalculation} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<InflationRateCalculation> rateCalculation_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "securityId", "nominalPayment", "periodicPayments", "frequency", "rollConvention", "dayCount", "yieldConvention", "legalEntityId", "settlementDateOffset", "rateCalculation");
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
			case 1574023291: // securityId
			  return securityId_Renamed;
			case -44199542: // nominalPayment
			  return nominalPayment_Renamed;
			case -367345944: // periodicPayments
			  return periodicPayments_Renamed;
			case -70023844: // frequency
			  return frequency_Renamed;
			case -10223666: // rollConvention
			  return rollConvention_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case -1895216418: // yieldConvention
			  return yieldConvention_Renamed;
			case 866287159: // legalEntityId
			  return legalEntityId_Renamed;
			case 135924714: // settlementDateOffset
			  return settlementDateOffset_Renamed;
			case -521703991: // rateCalculation
			  return rateCalculation_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ResolvedCapitalIndexedBond.Builder builder()
		{
		  return new ResolvedCapitalIndexedBond.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ResolvedCapitalIndexedBond);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code securityId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SecurityId> securityId()
		{
		  return securityId_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code nominalPayment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CapitalIndexedBondPaymentPeriod> nominalPayment()
		{
		  return nominalPayment_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code periodicPayments} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<CapitalIndexedBondPaymentPeriod>> periodicPayments()
		{
		  return periodicPayments_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code frequency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Frequency> frequency()
		{
		  return frequency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code rollConvention} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<RollConvention> rollConvention()
		{
		  return rollConvention_Renamed;
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
		/// The meta-property for the {@code rateCalculation} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<InflationRateCalculation> rateCalculation()
		{
		  return rateCalculation_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1574023291: // securityId
			  return ((ResolvedCapitalIndexedBond) bean).SecurityId;
			case -44199542: // nominalPayment
			  return ((ResolvedCapitalIndexedBond) bean).NominalPayment;
			case -367345944: // periodicPayments
			  return ((ResolvedCapitalIndexedBond) bean).PeriodicPayments;
			case -70023844: // frequency
			  return ((ResolvedCapitalIndexedBond) bean).Frequency;
			case -10223666: // rollConvention
			  return ((ResolvedCapitalIndexedBond) bean).RollConvention;
			case 1905311443: // dayCount
			  return ((ResolvedCapitalIndexedBond) bean).DayCount;
			case -1895216418: // yieldConvention
			  return ((ResolvedCapitalIndexedBond) bean).YieldConvention;
			case 866287159: // legalEntityId
			  return ((ResolvedCapitalIndexedBond) bean).LegalEntityId;
			case 135924714: // settlementDateOffset
			  return ((ResolvedCapitalIndexedBond) bean).SettlementDateOffset;
			case -521703991: // rateCalculation
			  return ((ResolvedCapitalIndexedBond) bean).RateCalculation;
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
	  /// The bean-builder for {@code ResolvedCapitalIndexedBond}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ResolvedCapitalIndexedBond>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SecurityId securityId_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CapitalIndexedBondPaymentPeriod nominalPayment_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<CapitalIndexedBondPaymentPeriod> periodicPayments_Renamed = ImmutableList.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Frequency frequency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal RollConvention rollConvention_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount dayCount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CapitalIndexedBondYieldConvention yieldConvention_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LegalEntityId legalEntityId_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment settlementDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal InflationRateCalculation rateCalculation_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ResolvedCapitalIndexedBond beanToCopy)
		{
		  this.securityId_Renamed = beanToCopy.SecurityId;
		  this.nominalPayment_Renamed = beanToCopy.NominalPayment;
		  this.periodicPayments_Renamed = beanToCopy.PeriodicPayments;
		  this.frequency_Renamed = beanToCopy.Frequency;
		  this.rollConvention_Renamed = beanToCopy.RollConvention;
		  this.dayCount_Renamed = beanToCopy.DayCount;
		  this.yieldConvention_Renamed = beanToCopy.YieldConvention;
		  this.legalEntityId_Renamed = beanToCopy.LegalEntityId;
		  this.settlementDateOffset_Renamed = beanToCopy.SettlementDateOffset;
		  this.rateCalculation_Renamed = beanToCopy.RateCalculation;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1574023291: // securityId
			  return securityId_Renamed;
			case -44199542: // nominalPayment
			  return nominalPayment_Renamed;
			case -367345944: // periodicPayments
			  return periodicPayments_Renamed;
			case -70023844: // frequency
			  return frequency_Renamed;
			case -10223666: // rollConvention
			  return rollConvention_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case -1895216418: // yieldConvention
			  return yieldConvention_Renamed;
			case 866287159: // legalEntityId
			  return legalEntityId_Renamed;
			case 135924714: // settlementDateOffset
			  return settlementDateOffset_Renamed;
			case -521703991: // rateCalculation
			  return rateCalculation_Renamed;
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
			case 1574023291: // securityId
			  this.securityId_Renamed = (SecurityId) newValue;
			  break;
			case -44199542: // nominalPayment
			  this.nominalPayment_Renamed = (CapitalIndexedBondPaymentPeriod) newValue;
			  break;
			case -367345944: // periodicPayments
			  this.periodicPayments_Renamed = (IList<CapitalIndexedBondPaymentPeriod>) newValue;
			  break;
			case -70023844: // frequency
			  this.frequency_Renamed = (Frequency) newValue;
			  break;
			case -10223666: // rollConvention
			  this.rollConvention_Renamed = (RollConvention) newValue;
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
			case -521703991: // rateCalculation
			  this.rateCalculation_Renamed = (InflationRateCalculation) newValue;
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

		public override ResolvedCapitalIndexedBond build()
		{
		  return new ResolvedCapitalIndexedBond(securityId_Renamed, nominalPayment_Renamed, periodicPayments_Renamed, frequency_Renamed, rollConvention_Renamed, dayCount_Renamed, yieldConvention_Renamed, legalEntityId_Renamed, settlementDateOffset_Renamed, rateCalculation_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the security identifier.
		/// <para>
		/// This identifier uniquely identifies the security within the system.
		/// </para>
		/// </summary>
		/// <param name="securityId">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder securityId(SecurityId securityId)
		{
		  JodaBeanUtils.notNull(securityId, "securityId");
		  this.securityId_Renamed = securityId;
		  return this;
		}

		/// <summary>
		/// Sets the nominal payment of the product.
		/// <para>
		/// The payment date of the nominal payment agrees with the final coupon payment date of the periodic payments.
		/// </para>
		/// </summary>
		/// <param name="nominalPayment">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder nominalPayment(CapitalIndexedBondPaymentPeriod nominalPayment)
		{
		  JodaBeanUtils.notNull(nominalPayment, "nominalPayment");
		  this.nominalPayment_Renamed = nominalPayment;
		  return this;
		}

		/// <summary>
		/// Sets the periodic payments of the product.
		/// <para>
		/// Each payment period represents part of the life-time of the product.
		/// The start date and end date of the leg are determined from the first and last period.
		/// As such, the periods should be sorted.
		/// </para>
		/// </summary>
		/// <param name="periodicPayments">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder periodicPayments(IList<CapitalIndexedBondPaymentPeriod> periodicPayments)
		{
		  JodaBeanUtils.notNull(periodicPayments, "periodicPayments");
		  this.periodicPayments_Renamed = periodicPayments;
		  return this;
		}

		/// <summary>
		/// Sets the {@code periodicPayments} property in the builder
		/// from an array of objects. </summary>
		/// <param name="periodicPayments">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder periodicPayments(params CapitalIndexedBondPaymentPeriod[] periodicPayments)
		{
		  return this.periodicPayments(ImmutableList.copyOf(periodicPayments));
		}

		/// <summary>
		/// Sets the frequency of the bond payments.
		/// <para>
		/// This must match the frequency used to generate the payment schedule.
		/// </para>
		/// </summary>
		/// <param name="frequency">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder frequency(Frequency frequency)
		{
		  JodaBeanUtils.notNull(frequency, "frequency");
		  this.frequency_Renamed = frequency;
		  return this;
		}

		/// <summary>
		/// Sets the roll convention of the bond payments.
		/// <para>
		/// This must match the convention used to generate the payment schedule.
		/// </para>
		/// </summary>
		/// <param name="rollConvention">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder rollConvention(RollConvention rollConvention)
		{
		  JodaBeanUtils.notNull(rollConvention, "rollConvention");
		  this.rollConvention_Renamed = rollConvention;
		  return this;
		}

		/// <summary>
		/// Sets the day count convention applicable.
		/// <para>
		/// The conversion from dates to a numerical value is made based on this day count.
		/// For the inflation-indexed bond, the day count convention is used to compute accrued interest.
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

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(352);
		  buf.Append("ResolvedCapitalIndexedBond.Builder{");
		  buf.Append("securityId").Append('=').Append(JodaBeanUtils.ToString(securityId_Renamed)).Append(',').Append(' ');
		  buf.Append("nominalPayment").Append('=').Append(JodaBeanUtils.ToString(nominalPayment_Renamed)).Append(',').Append(' ');
		  buf.Append("periodicPayments").Append('=').Append(JodaBeanUtils.ToString(periodicPayments_Renamed)).Append(',').Append(' ');
		  buf.Append("frequency").Append('=').Append(JodaBeanUtils.ToString(frequency_Renamed)).Append(',').Append(' ');
		  buf.Append("rollConvention").Append('=').Append(JodaBeanUtils.ToString(rollConvention_Renamed)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("yieldConvention").Append('=').Append(JodaBeanUtils.ToString(yieldConvention_Renamed)).Append(',').Append(' ');
		  buf.Append("legalEntityId").Append('=').Append(JodaBeanUtils.ToString(legalEntityId_Renamed)).Append(',').Append(' ');
		  buf.Append("settlementDateOffset").Append('=').Append(JodaBeanUtils.ToString(settlementDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("rateCalculation").Append('=').Append(JodaBeanUtils.ToString(rateCalculation_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}