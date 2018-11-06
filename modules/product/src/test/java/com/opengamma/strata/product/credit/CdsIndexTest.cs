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

	using ImmutableList = com.google.common.collect.ImmutableList;
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
	/// Test <seealso cref="CdsIndex"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CdsIndexTest
	public class CdsIndexTest
	{
	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly HolidayCalendarId CALENDAR = HolidayCalendarIds.SAT_SUN;
	  private static readonly DaysAdjustment SETTLE_DAY_ADJ = DaysAdjustment.ofBusinessDays(3, CALENDAR);
	  private static readonly DaysAdjustment STEPIN_DAY_ADJ = DaysAdjustment.ofCalendarDays(1);
	  private static readonly StandardId INDEX_ID = StandardId.of("OG", "AA-INDEX");
	  private static readonly ImmutableList<StandardId> LEGAL_ENTITIES = ImmutableList.of(StandardId.of("OG", "ABC1"), StandardId.of("OG", "ABC2"), StandardId.of("OG", "ABC3"), StandardId.of("OG", "ABC4"));
	  private const double COUPON = 0.05;
	  private const double NOTIONAL = 1.0e9;
	  private static readonly LocalDate START_DATE = LocalDate.of(2013, 12, 20);
	  private static readonly LocalDate END_DATE = LocalDate.of(2024, 9, 20);
	  private static readonly CdsIndex PRODUCT = CdsIndex.of(BUY, INDEX_ID, LEGAL_ENTITIES, USD, NOTIONAL, START_DATE, END_DATE, P3M, SAT_SUN, COUPON);

	  public virtual void test_builder()
	  {
		LocalDate startDate = LocalDate.of(2014, 12, 20);
		LocalDate endDate = LocalDate.of(2020, 10, 20);
		PeriodicSchedule sch = PeriodicSchedule.of(startDate, endDate, P3M, BusinessDayAdjustment.NONE, SHORT_INITIAL, RollConventions.NONE);
		CdsIndex test = CdsIndex.builder().paymentSchedule(sch).buySell(SELL).currency(JPY).dayCount(ACT_365F).fixedRate(COUPON).cdsIndexId(INDEX_ID).legalEntityIds(LEGAL_ENTITIES).notional(NOTIONAL).paymentOnDefault(PaymentOnDefault.NONE).protectionStart(ProtectionStartOfDay.NONE).settlementDateOffset(SETTLE_DAY_ADJ).stepinDateOffset(STEPIN_DAY_ADJ).build();
		assertEquals(test.PaymentSchedule, sch);
		assertEquals(test.BuySell, SELL);
		assertEquals(test.Currency, JPY);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.FixedRate, COUPON);
		assertEquals(test.CdsIndexId, INDEX_ID);
		assertEquals(test.LegalEntityIds, LEGAL_ENTITIES);
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
		assertEquals(PRODUCT.PaymentSchedule, expected);
		assertEquals(PRODUCT.BuySell, BUY);
		assertEquals(PRODUCT.Currency, USD);
		assertEquals(PRODUCT.DayCount, ACT_360);
		assertEquals(PRODUCT.FixedRate, COUPON);
		assertEquals(PRODUCT.CdsIndexId, INDEX_ID);
		assertEquals(PRODUCT.LegalEntityIds, LEGAL_ENTITIES);
		assertEquals(PRODUCT.Notional, NOTIONAL);
		assertEquals(PRODUCT.PaymentOnDefault, ACCRUED_PREMIUM);
		assertEquals(PRODUCT.ProtectionStart, BEGINNING);
		assertEquals(PRODUCT.SettlementDateOffset, SETTLE_DAY_ADJ);
		assertEquals(PRODUCT.StepinDateOffset, STEPIN_DAY_ADJ);
		CdsIndex test = CdsIndex.of(BUY, INDEX_ID, LEGAL_ENTITIES, USD, NOTIONAL, START_DATE, END_DATE, P3M, SAT_SUN, COUPON);
		assertEquals(test, PRODUCT);
	  }

	  public virtual void test_resolve()
	  {
		BusinessDayAdjustment bussAdj = BusinessDayAdjustment.of(FOLLOWING, SAT_SUN);
		ResolvedCdsIndex test = PRODUCT.resolve(REF_DATA);
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
		ResolvedCdsIndex expected = ResolvedCdsIndex.builder().buySell(BUY).cdsIndexId(INDEX_ID).legalEntityIds(LEGAL_ENTITIES).dayCount(ACT_360).paymentOnDefault(ACCRUED_PREMIUM).paymentPeriods(payments).protectionStart(BEGINNING).protectionEndDate(END_DATE).settlementDateOffset(SETTLE_DAY_ADJ).stepinDateOffset(STEPIN_DAY_ADJ).build();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(PRODUCT);
		CdsIndex other = CdsIndex.builder().buySell(SELL).cdsIndexId(StandardId.of("OG", "AA-INDEX")).legalEntityIds(ImmutableList.of(StandardId.of("OG", "ABC1"), StandardId.of("OG", "ABC2"))).currency(JPY).notional(1d).paymentSchedule(PeriodicSchedule.of(LocalDate.of(2014, 1, 4), LocalDate.of(2020, 11, 20), P6M, BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, JPTO), StubConvention.SHORT_FINAL, RollConventions.NONE)).fixedRate(0.01).dayCount(ACT_365F).paymentOnDefault(PaymentOnDefault.NONE).protectionStart(ProtectionStartOfDay.NONE).settlementDateOffset(DaysAdjustment.NONE).stepinDateOffset(DaysAdjustment.NONE).build();
		coverBeanEquals(PRODUCT, other);
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(PRODUCT);
	  }

	}

}