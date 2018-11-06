using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.NL_365;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.GB_RPI;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.US_CPI_U;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.bond.CapitalIndexedBondYieldConvention.GB_IL_FLOAT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.bond.CapitalIndexedBondYieldConvention.US_IL_REAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.PriceIndexCalculationMethod.INTERPOLATED;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueAdjustment = com.opengamma.strata.basics.value.ValueAdjustment;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using ValueStep = com.opengamma.strata.basics.value.ValueStep;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;
	using InflationRateCalculation = com.opengamma.strata.product.swap.InflationRateCalculation;

	/// <summary>
	/// Test <seealso cref="CapitalIndexedBond"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CapitalIndexedBondTest
	public class CapitalIndexedBondTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly SecurityId SECURITY_ID = SecurityId.of("OG-Test", "Bond");
	  private static readonly SecurityId SECURITY_ID2 = SecurityId.of("OG-Test", "Bond2");
	  private const double NOTIONAL = 10_000_000d;
	  private const double START_INDEX = 198.475;
	  private static readonly double[] COUPONS = new double[] {0.01, 0.015, 0.012, 0.09};
	  private static readonly ValueSchedule COUPON;
	  static CapitalIndexedBondTest()
	  {
		IList<ValueStep> steps = new List<ValueStep>();
		steps.Add(ValueStep.of(1, ValueAdjustment.ofReplace(COUPONS[1])));
		steps.Add(ValueStep.of(2, ValueAdjustment.ofReplace(COUPONS[2])));
		steps.Add(ValueStep.of(3, ValueAdjustment.ofReplace(COUPONS[3])));
		COUPON = ValueSchedule.of(COUPONS[0], steps);
	  }
	  private static readonly InflationRateCalculation RATE_CALC = InflationRateCalculation.builder().gearing(COUPON).index(US_CPI_U).lag(Period.ofMonths(3)).indexCalculationMethod(INTERPOLATED).firstIndexValue(START_INDEX).build();
	  private static readonly BusinessDayAdjustment EX_COUPON_ADJ = BusinessDayAdjustment.of(BusinessDayConventions.PRECEDING, USNY);
	  private static readonly DaysAdjustment EX_COUPON = DaysAdjustment.ofCalendarDays(-7, EX_COUPON_ADJ);
	  private static readonly DaysAdjustment SETTLE_OFFSET = DaysAdjustment.ofBusinessDays(2, USNY);
	  private static readonly LegalEntityId LEGAL_ENTITY = LegalEntityId.of("OG-Ticker", "US-Govt");
	  private static readonly LocalDate START = LocalDate.of(2008, 1, 13);
	  private static readonly LocalDate END = LocalDate.of(2010, 1, 13);
	  private static readonly Frequency FREQUENCY = Frequency.P6M;
	  private static readonly BusinessDayAdjustment SCHEDULE_ADJ = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, USNY);
	  private static readonly PeriodicSchedule SCHEDULE = PeriodicSchedule.of(START, END, FREQUENCY, SCHEDULE_ADJ, StubConvention.NONE, RollConventions.NONE);

	  public virtual void test_builder_full()
	  {
		CapitalIndexedBond test = sut();
		assertEquals(test.SecurityId, SECURITY_ID);
		assertEquals(test.Currency, USD);
		assertEquals(test.DayCount, ACT_ACT_ISDA);
		assertEquals(test.ExCouponPeriod, EX_COUPON);
		assertEquals(test.LegalEntityId, LEGAL_ENTITY);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.AccrualSchedule, SCHEDULE);
		assertEquals(test.RateCalculation, RATE_CALC);
		assertEquals(test.FirstIndexValue, RATE_CALC.FirstIndexValue.Value);
		assertEquals(test.SettlementDateOffset, SETTLE_OFFSET);
		assertEquals(test.YieldConvention, US_IL_REAL);
	  }

	  public virtual void test_builder_min()
	  {
		CapitalIndexedBond test = CapitalIndexedBond.builder().securityId(SECURITY_ID).notional(NOTIONAL).currency(USD).dayCount(ACT_ACT_ISDA).rateCalculation(RATE_CALC).legalEntityId(LEGAL_ENTITY).yieldConvention(US_IL_REAL).settlementDateOffset(SETTLE_OFFSET).accrualSchedule(SCHEDULE).build();
		assertEquals(test.SecurityId, SECURITY_ID);
		assertEquals(test.Currency, USD);
		assertEquals(test.DayCount, ACT_ACT_ISDA);
		assertEquals(test.ExCouponPeriod, DaysAdjustment.NONE);
		assertEquals(test.LegalEntityId, LEGAL_ENTITY);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.AccrualSchedule, SCHEDULE);
		assertEquals(test.RateCalculation, RATE_CALC);
		assertEquals(test.SettlementDateOffset, SETTLE_OFFSET);
		assertEquals(test.YieldConvention, US_IL_REAL);
	  }

	  public virtual void test_builder_fail()
	  {
		// negative settlement date offset
		assertThrowsIllegalArg(() => CapitalIndexedBond.builder().securityId(SECURITY_ID).notional(NOTIONAL).currency(USD).dayCount(ACT_ACT_ISDA).rateCalculation(RATE_CALC).exCouponPeriod(EX_COUPON).legalEntityId(LEGAL_ENTITY).yieldConvention(US_IL_REAL).settlementDateOffset(DaysAdjustment.ofBusinessDays(-2, USNY)).accrualSchedule(SCHEDULE).build());
		// positive ex-coupon days
		assertThrowsIllegalArg(() => CapitalIndexedBond.builder().securityId(SECURITY_ID).notional(NOTIONAL).currency(USD).dayCount(ACT_ACT_ISDA).rateCalculation(RATE_CALC).exCouponPeriod(DaysAdjustment.ofCalendarDays(7, BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, USNY))).legalEntityId(LEGAL_ENTITY).yieldConvention(US_IL_REAL).settlementDateOffset(SETTLE_OFFSET).accrualSchedule(SCHEDULE).build());
	  }

	  public virtual void test_resolve()
	  {
		CapitalIndexedBond @base = sut();
		LocalDate[] unAdjDates = new LocalDate[] {LocalDate.of(2008, 1, 13), LocalDate.of(2008, 7, 13), LocalDate.of(2009, 1, 13), LocalDate.of(2009, 7, 13), LocalDate.of(2010, 1, 13)};
		CapitalIndexedBondPaymentPeriod[] periodic = new CapitalIndexedBondPaymentPeriod[4];
		for (int i = 0; i < 4; ++i)
		{
		  LocalDate start = SCHEDULE_ADJ.adjust(unAdjDates[i], REF_DATA);
		  LocalDate end = SCHEDULE_ADJ.adjust(unAdjDates[i + 1], REF_DATA);
		  LocalDate detachment = EX_COUPON.adjust(end, REF_DATA);
		  RateComputation comp = RATE_CALC.createRateComputation(end);
		  periodic[i] = CapitalIndexedBondPaymentPeriod.builder().currency(USD).startDate(start).endDate(end).unadjustedStartDate(unAdjDates[i]).unadjustedEndDate(unAdjDates[i + 1]).detachmentDate(detachment).realCoupon(COUPONS[i]).rateComputation(comp).notional(NOTIONAL).build();
		}
		CapitalIndexedBondPaymentPeriod nominalExp = periodic[3].withUnitCoupon(periodic[0].StartDate, periodic[0].UnadjustedStartDate);
		ResolvedCapitalIndexedBond expected = ResolvedCapitalIndexedBond.builder().securityId(SECURITY_ID).dayCount(ACT_ACT_ISDA).legalEntityId(LEGAL_ENTITY).nominalPayment(nominalExp).periodicPayments(periodic).frequency(SCHEDULE.Frequency).rollConvention(SCHEDULE.calculatedRollConvention()).settlementDateOffset(SETTLE_OFFSET).yieldConvention(US_IL_REAL).rateCalculation(@base.RateCalculation).build();
		assertEquals(@base.resolve(REF_DATA), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(sut());
		coverBeanEquals(sut(), sut2());
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(sut());
	  }

	  //-------------------------------------------------------------------------
	  internal static CapitalIndexedBond sut()
	  {
		return CapitalIndexedBond.builder().securityId(SECURITY_ID).notional(NOTIONAL).currency(USD).dayCount(ACT_ACT_ISDA).rateCalculation(RATE_CALC).exCouponPeriod(EX_COUPON).legalEntityId(LEGAL_ENTITY).yieldConvention(US_IL_REAL).settlementDateOffset(SETTLE_OFFSET).accrualSchedule(SCHEDULE).build();
	  }

	  internal static CapitalIndexedBond sut1()
	  {
		return CapitalIndexedBond.builder().securityId(SECURITY_ID).notional(NOTIONAL).currency(USD).dayCount(ACT_ACT_ISDA).rateCalculation(RATE_CALC).exCouponPeriod(EX_COUPON).legalEntityId(LEGAL_ENTITY).yieldConvention(GB_IL_FLOAT).settlementDateOffset(SETTLE_OFFSET).accrualSchedule(SCHEDULE).build();
	  }

	  internal static CapitalIndexedBond sut2()
	  {
		return CapitalIndexedBond.builder().securityId(SECURITY_ID2).notional(5.0e7).currency(GBP).dayCount(NL_365).rateCalculation(InflationRateCalculation.builder().index(GB_RPI).lag(Period.ofMonths(2)).indexCalculationMethod(INTERPOLATED).firstIndexValue(124.556).build()).exCouponPeriod(EX_COUPON).legalEntityId(LegalEntityId.of("OG-Ticker", "US-Govt-1")).yieldConvention(GB_IL_FLOAT).settlementDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).accrualSchedule(PeriodicSchedule.of(START, END, FREQUENCY, BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, GBLO), StubConvention.NONE, RollConventions.NONE)).build();
	  }

	}

}