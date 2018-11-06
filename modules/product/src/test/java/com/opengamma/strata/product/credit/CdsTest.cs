using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.credit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.JPY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.JPTO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.SAT_SUN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.StubConvention.SHORT_INITIAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.StubConvention.SMART_INITIAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.BUY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.SELL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.credit.PaymentOnDefault.ACCRUED_PREMIUM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.credit.ProtectionStartOfDay.BEGINNING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;

	/// <summary>
	/// Test <seealso cref="Cds"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CdsTest
	public class CdsTest
	{
	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly HolidayCalendarId CALENDAR = HolidayCalendarIds.SAT_SUN;
	  private static readonly DaysAdjustment SETTLE_DAY_ADJ = DaysAdjustment.ofBusinessDays(3, CALENDAR);
	  private static readonly DaysAdjustment STEPIN_DAY_ADJ = DaysAdjustment.ofCalendarDays(1);
	  private static readonly StandardId LEGAL_ENTITY = StandardId.of("OG", "ABC");
	  private const double COUPON = 0.05;
	  private const double NOTIONAL = 1.0e9;
	  private static readonly LocalDate START_DATE = LocalDate.of(2013, 12, 20);
	  private static readonly LocalDate END_DATE = LocalDate.of(2024, 9, 20);
	  private static readonly Cds PRODUCT_STD = Cds.of(BUY, LEGAL_ENTITY, USD, NOTIONAL, START_DATE, END_DATE, P3M, SAT_SUN, COUPON);

	  public virtual void test_builder()
	  {
		LocalDate startDate = LocalDate.of(2014, 12, 20);
		LocalDate endDate = LocalDate.of(2020, 10, 20);
		PeriodicSchedule sch = PeriodicSchedule.of(startDate, endDate, P3M, BusinessDayAdjustment.NONE, SHORT_INITIAL, RollConventions.NONE);
		Cds test = Cds.builder().paymentSchedule(sch).buySell(SELL).currency(JPY).dayCount(ACT_365F).fixedRate(COUPON).legalEntityId(LEGAL_ENTITY).notional(NOTIONAL).paymentOnDefault(PaymentOnDefault.NONE).protectionStart(ProtectionStartOfDay.NONE).settlementDateOffset(SETTLE_DAY_ADJ).stepinDateOffset(STEPIN_DAY_ADJ).build();
		assertEquals(test.PaymentSchedule, sch);
		assertEquals(test.BuySell, SELL);
		assertEquals(test.Currency, JPY);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.FixedRate, COUPON);
		assertEquals(test.LegalEntityId, LEGAL_ENTITY);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.PaymentOnDefault, PaymentOnDefault.NONE);
		assertEquals(test.ProtectionStart, ProtectionStartOfDay.NONE);
		assertEquals(test.SettlementDateOffset, SETTLE_DAY_ADJ);
		assertEquals(test.StepinDateOffset, STEPIN_DAY_ADJ);
		assertEquals(test.CrossCurrency, false);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(JPY));
		assertEquals(test.allCurrencies(), ImmutableSet.of(JPY));
	  }

	  public virtual void test_of()
	  {
		BusinessDayAdjustment bussAdj = BusinessDayAdjustment.of(FOLLOWING, SAT_SUN);
		PeriodicSchedule expected = PeriodicSchedule.builder().startDate(START_DATE).endDate(END_DATE).businessDayAdjustment(bussAdj).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).endDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).frequency(P3M).rollConvention(RollConventions.NONE).stubConvention(SMART_INITIAL).build();
		assertEquals(PRODUCT_STD.PaymentSchedule, expected);
		assertEquals(PRODUCT_STD.BuySell, BUY);
		assertEquals(PRODUCT_STD.Currency, USD);
		assertEquals(PRODUCT_STD.DayCount, ACT_360);
		assertEquals(PRODUCT_STD.FixedRate, COUPON);
		assertEquals(PRODUCT_STD.LegalEntityId, LEGAL_ENTITY);
		assertEquals(PRODUCT_STD.Notional, NOTIONAL);
		assertEquals(PRODUCT_STD.PaymentOnDefault, ACCRUED_PREMIUM);
		assertEquals(PRODUCT_STD.ProtectionStart, BEGINNING);
		assertEquals(PRODUCT_STD.SettlementDateOffset, SETTLE_DAY_ADJ);
		assertEquals(PRODUCT_STD.StepinDateOffset, STEPIN_DAY_ADJ);
		Cds test = Cds.of(BUY, LEGAL_ENTITY, USD, NOTIONAL, START_DATE, END_DATE, P3M, SAT_SUN, COUPON);
		assertEquals(test, PRODUCT_STD);
	  }

	  public virtual void test_resolve()
	  {
		BusinessDayAdjustment bussAdj = BusinessDayAdjustment.of(FOLLOWING, SAT_SUN);
		ResolvedCds test = PRODUCT_STD.resolve(REF_DATA);
		int nDates = 44;
		LocalDate[] dates = new LocalDate[nDates];
		for (int i = 0; i < nDates; ++i)
		{
		  dates[i] = START_DATE.plusMonths(3 * i);
		}
		IList<CreditCouponPaymentPeriod> payments = new List<CreditCouponPaymentPeriod>(nDates - 1);
		for (int i = 0; i < nDates - 2; ++i)
		{
		  LocalDate start = i == 0 ? dates[i] : bussAdj.adjust(dates[i], REF_DATA);
		  LocalDate end = bussAdj.adjust(dates[i + 1], REF_DATA);
		  payments.Add(CreditCouponPaymentPeriod.builder().startDate(start).endDate(end).unadjustedStartDate(dates[i]).unadjustedEndDate(dates[i + 1]).effectiveStartDate(start.minusDays(1)).effectiveEndDate(end.minusDays(1)).paymentDate(end).currency(USD).notional(NOTIONAL).fixedRate(COUPON).yearFraction(ACT_360.relativeYearFraction(start, end)).build());
		}
		LocalDate start = bussAdj.adjust(dates[nDates - 2], REF_DATA);
		LocalDate end = dates[nDates - 1];
		payments.Add(CreditCouponPaymentPeriod.builder().startDate(start).endDate(end.plusDays(1)).unadjustedStartDate(dates[nDates - 2]).unadjustedEndDate(end).effectiveStartDate(start.minusDays(1)).effectiveEndDate(end).paymentDate(bussAdj.adjust(end, REF_DATA)).currency(USD).notional(NOTIONAL).fixedRate(COUPON).yearFraction(ACT_360.relativeYearFraction(start, end.plusDays(1))).build());
		ResolvedCds expected = ResolvedCds.builder().buySell(BUY).legalEntityId(LEGAL_ENTITY).dayCount(ACT_360).paymentOnDefault(ACCRUED_PREMIUM).paymentPeriods(payments).protectionStart(BEGINNING).protectionEndDate(END_DATE).settlementDateOffset(SETTLE_DAY_ADJ).stepinDateOffset(STEPIN_DAY_ADJ).build();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(PRODUCT_STD);
		Cds other = Cds.builder().buySell(SELL).legalEntityId(StandardId.of("OG", "EFG")).currency(JPY).notional(1d).fixedRate(0.01).dayCount(ACT_365F).paymentSchedule(PeriodicSchedule.builder().businessDayAdjustment(BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, JPTO)).startDate(LocalDate.of(2014, 1, 4)).endDate(LocalDate.of(2020, 11, 20)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).endDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).frequency(P6M).rollConvention(RollConventions.NONE).stubConvention(StubConvention.SHORT_FINAL).build()).paymentOnDefault(PaymentOnDefault.NONE).protectionStart(ProtectionStartOfDay.NONE).stepinDateOffset(DaysAdjustment.NONE).settlementDateOffset(DaysAdjustment.NONE).build();
		coverBeanEquals(PRODUCT_STD, other);
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(PRODUCT_STD);
	  }

	}

}