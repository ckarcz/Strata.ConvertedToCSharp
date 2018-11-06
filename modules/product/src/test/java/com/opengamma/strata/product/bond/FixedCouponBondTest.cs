/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.SAT_SUN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using Schedule = com.opengamma.strata.basics.schedule.Schedule;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;

	/// <summary>
	/// Test <seealso cref="FixedCouponBond"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FixedCouponBondTest
	public class FixedCouponBondTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const FixedCouponBondYieldConvention YIELD_CONVENTION = FixedCouponBondYieldConvention.DE_BONDS;
	  private static readonly LegalEntityId LEGAL_ENTITY = LegalEntityId.of("OG-Ticker", "BUN EUR");
	  private const double NOTIONAL = 1.0e7;
	  private const double FIXED_RATE = 0.015;
	  private static readonly DaysAdjustment DATE_OFFSET = DaysAdjustment.ofBusinessDays(3, EUTA);
	  private static readonly DayCount DAY_COUNT = DayCounts.ACT_365F;
	  private static readonly LocalDate START_DATE = LocalDate.of(2015, 4, 12);
	  private static readonly LocalDate END_DATE = LocalDate.of(2025, 4, 12);
	  private static readonly SecurityId SECURITY_ID = SecurityId.of("OG-Test", "Bond");
	  private static readonly SecurityId SECURITY_ID2 = SecurityId.of("OG-Test", "Bond2");
	  private static readonly BusinessDayAdjustment BUSINESS_ADJUST = BusinessDayAdjustment.of(BusinessDayConventions.MODIFIED_FOLLOWING, EUTA);
	  private static readonly PeriodicSchedule PERIOD_SCHEDULE = PeriodicSchedule.of(START_DATE, END_DATE, Frequency.P6M, BUSINESS_ADJUST, StubConvention.SHORT_INITIAL, false);
	  private const int EX_COUPON_DAYS = 5;
	  private static readonly DaysAdjustment EX_COUPON = DaysAdjustment.ofBusinessDays(-EX_COUPON_DAYS, EUTA, BUSINESS_ADJUST);

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		FixedCouponBond test = sut();
		assertEquals(test.SecurityId, SECURITY_ID);
		assertEquals(test.DayCount, DAY_COUNT);
		assertEquals(test.FixedRate, FIXED_RATE);
		assertEquals(test.LegalEntityId, LEGAL_ENTITY);
		assertEquals(test.Currency, EUR);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.AccrualSchedule, PERIOD_SCHEDULE);
		assertEquals(test.SettlementDateOffset, DATE_OFFSET);
		assertEquals(test.YieldConvention, YIELD_CONVENTION);
		assertEquals(test.ExCouponPeriod, EX_COUPON);
	  }

	  public virtual void test_builder_fail()
	  {
		assertThrowsIllegalArg(() => FixedCouponBond.builder().securityId(SECURITY_ID).dayCount(DAY_COUNT).fixedRate(FIXED_RATE).legalEntityId(LEGAL_ENTITY).currency(EUR).notional(NOTIONAL).accrualSchedule(PERIOD_SCHEDULE).settlementDateOffset(DATE_OFFSET).yieldConvention(YIELD_CONVENTION).exCouponPeriod(DaysAdjustment.ofBusinessDays(EX_COUPON_DAYS, EUTA, BUSINESS_ADJUST)).build());
		assertThrowsIllegalArg(() => FixedCouponBond.builder().securityId(SECURITY_ID).dayCount(DAY_COUNT).fixedRate(FIXED_RATE).legalEntityId(LEGAL_ENTITY).currency(EUR).notional(NOTIONAL).accrualSchedule(PERIOD_SCHEDULE).settlementDateOffset(DaysAdjustment.ofBusinessDays(-3, EUTA)).yieldConvention(YIELD_CONVENTION).build());
	  }

	  public virtual void test_resolve()
	  {
		FixedCouponBond @base = sut();
		ResolvedFixedCouponBond resolved = @base.resolve(REF_DATA);
		assertEquals(resolved.LegalEntityId, LEGAL_ENTITY);
		assertEquals(resolved.SettlementDateOffset, DATE_OFFSET);
		assertEquals(resolved.YieldConvention, YIELD_CONVENTION);
		ImmutableList<FixedCouponBondPaymentPeriod> periodicPayments = resolved.PeriodicPayments;
		int expNum = 20;
		assertEquals(periodicPayments.size(), expNum);
		LocalDate unadjustedEnd = END_DATE;
		Schedule unadjusted = PERIOD_SCHEDULE.createSchedule(REF_DATA).toUnadjusted();
		for (int i = 0; i < expNum; ++i)
		{
		  FixedCouponBondPaymentPeriod payment = periodicPayments.get(expNum - 1 - i);
		  assertEquals(payment.Currency, EUR);
		  assertEquals(payment.Notional, NOTIONAL);
		  assertEquals(payment.FixedRate, FIXED_RATE);
		  assertEquals(payment.UnadjustedEndDate, unadjustedEnd);
		  assertEquals(payment.EndDate, BUSINESS_ADJUST.adjust(unadjustedEnd, REF_DATA));
		  assertEquals(payment.PaymentDate, payment.EndDate);
		  LocalDate unadjustedStart = unadjustedEnd.minusMonths(6);
		  assertEquals(payment.UnadjustedStartDate, unadjustedStart);
		  assertEquals(payment.StartDate, BUSINESS_ADJUST.adjust(unadjustedStart, REF_DATA));
		  assertEquals(payment.YearFraction, unadjusted.getPeriod(expNum - 1 - i).yearFraction(DAY_COUNT, unadjusted));
		  assertEquals(payment.DetachmentDate, EX_COUPON.adjust(payment.PaymentDate, REF_DATA));
		  unadjustedEnd = unadjustedStart;
		}
		Payment expectedPayment = Payment.of(CurrencyAmount.of(EUR, NOTIONAL), BUSINESS_ADJUST.adjust(END_DATE, REF_DATA));
		assertEquals(resolved.NominalPayment, expectedPayment);
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
	  internal static FixedCouponBond sut()
	  {
		return FixedCouponBond.builder().securityId(SECURITY_ID).dayCount(DAY_COUNT).fixedRate(FIXED_RATE).legalEntityId(LEGAL_ENTITY).currency(EUR).notional(NOTIONAL).accrualSchedule(PERIOD_SCHEDULE).settlementDateOffset(DATE_OFFSET).yieldConvention(YIELD_CONVENTION).exCouponPeriod(EX_COUPON).build();
	  }

	  internal static FixedCouponBond sut2()
	  {
		BusinessDayAdjustment adj = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, SAT_SUN);
		PeriodicSchedule sche = PeriodicSchedule.of(START_DATE, END_DATE, Frequency.P12M, adj, StubConvention.SHORT_INITIAL, true);
		return FixedCouponBond.builder().securityId(SECURITY_ID2).dayCount(DayCounts.ACT_360).fixedRate(0.005).legalEntityId(LegalEntityId.of("OG-Ticker", "BUN EUR 2")).currency(GBP).notional(1.0e6).accrualSchedule(sche).settlementDateOffset(DaysAdjustment.ofBusinessDays(2, SAT_SUN)).yieldConvention(FixedCouponBondYieldConvention.GB_BUMP_DMO).build();
	  }

	}

}