using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.credit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.SAT_SUN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.BUY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.credit.PaymentOnDefault.ACCRUED_PREMIUM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.credit.ProtectionStartOfDay.BEGINNING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// Test <seealso cref="ResolvedCds"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedCdsTest
	public class ResolvedCdsTest
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
	  private static readonly BusinessDayAdjustment BUSS_ADJ = BusinessDayAdjustment.of(FOLLOWING, SAT_SUN);

	  private static readonly IList<CreditCouponPaymentPeriod> PAYMENTS = new List<CreditCouponPaymentPeriod>();
	  static ResolvedCdsTest()
	  {
		int nDates = 44;
		LocalDate[] dates = new LocalDate[nDates];
		for (int i = 0; i < nDates; ++i)
		{
		  dates[i] = START_DATE.plusMonths(3 * i);
		}
		for (int i = 0; i < nDates - 2; ++i)
		{
		  LocalDate start = i == 0 ? dates[i] : BUSS_ADJ.adjust(dates[i], REF_DATA);
		  LocalDate end = BUSS_ADJ.adjust(dates[i + 1], REF_DATA);
		  PAYMENTS.Add(CreditCouponPaymentPeriod.builder().startDate(start).endDate(end).effectiveStartDate(start.minusDays(1)).effectiveEndDate(end.minusDays(1)).paymentDate(end).currency(USD).notional(NOTIONAL).fixedRate(COUPON).yearFraction(ACT_360.relativeYearFraction(start, end)).build());
		}
		LocalDate start = BUSS_ADJ.adjust(dates[nDates - 2], REF_DATA);
		LocalDate end = dates[nDates - 1];
		PAYMENTS.Add(CreditCouponPaymentPeriod.builder().startDate(start).endDate(end.plusDays(1)).effectiveStartDate(start.minusDays(1)).effectiveEndDate(end).paymentDate(BUSS_ADJ.adjust(end, REF_DATA)).currency(USD).notional(NOTIONAL).fixedRate(COUPON).yearFraction(ACT_360.relativeYearFraction(start, end.plusDays(1))).build());
	  }

	  public virtual void test_builder()
	  {
		ResolvedCds test = ResolvedCds.builder().buySell(BUY).dayCount(ACT_360).legalEntityId(LEGAL_ENTITY).paymentOnDefault(ACCRUED_PREMIUM).protectionStart(BEGINNING).paymentPeriods(PAYMENTS).protectionEndDate(PAYMENTS[PAYMENTS.Count - 1].EffectiveEndDate).settlementDateOffset(SETTLE_DAY_ADJ).stepinDateOffset(STEPIN_DAY_ADJ).build();
		assertEquals(test.BuySell, BUY);
		assertEquals(test.Currency, USD);
		assertEquals(test.AccrualStartDate, PAYMENTS[0].StartDate);
		assertEquals(test.AccrualEndDate, PAYMENTS[42].EndDate);
		assertEquals(test.DayCount, ACT_360);
		assertEquals(test.FixedRate, COUPON);
		assertEquals(test.LegalEntityId, LEGAL_ENTITY);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.PaymentOnDefault, ACCRUED_PREMIUM);
		assertEquals(test.PaymentPeriods, PAYMENTS);
		assertEquals(test.ProtectionEndDate, PAYMENTS[42].EffectiveEndDate);
		assertEquals(test.SettlementDateOffset, SETTLE_DAY_ADJ);
		assertEquals(test.ProtectionStart, BEGINNING);
		assertEquals(test.StepinDateOffset, STEPIN_DAY_ADJ);
	  }

	  public virtual void test_accruedYearFraction()
	  {
		double eps = 1.0e-15;
		ResolvedCds test = ResolvedCds.builder().buySell(BUY).dayCount(ACT_360).legalEntityId(LEGAL_ENTITY).paymentOnDefault(ACCRUED_PREMIUM).protectionStart(BEGINNING).paymentPeriods(PAYMENTS).protectionEndDate(PAYMENTS[PAYMENTS.Count - 1].EffectiveEndDate).settlementDateOffset(SETTLE_DAY_ADJ).stepinDateOffset(STEPIN_DAY_ADJ).build();
		double accStart = test.accruedYearFraction(START_DATE.minusDays(1));
		double accNextMinusOne = test.accruedYearFraction(START_DATE.plusMonths(3).minusDays(1));
		double accNext = test.accruedYearFraction(START_DATE.plusMonths(3));
		double accNextOne = test.accruedYearFraction(START_DATE.plusMonths(3).plusDays(1));
		double accMod = test.accruedYearFraction(START_DATE.plusYears(1));
		double accEnd = test.accruedYearFraction(END_DATE);
		double accEndOne = test.accruedYearFraction(END_DATE.plusDays(1));
		assertEquals(accStart, 0d);
		assertEquals(accNext, 0d);
		assertEquals(accNextMinusOne, ACT_360.relativeYearFraction(START_DATE, START_DATE.plusMonths(3).minusDays(1)), eps);
		assertEquals(accNextOne, 1d / 360d, eps);
		// 2.x
		assertEquals(accMod, 0.24722222222222223, eps);
		assertEquals(accEnd, 0.25555555555555554, eps);
		assertEquals(accEndOne, 0.25833333333333336, eps);
	  }

	  public virtual void test_effectiveStartDate()
	  {
		ResolvedCds test1 = ResolvedCds.builder().buySell(BUY).dayCount(ACT_360).legalEntityId(LEGAL_ENTITY).paymentOnDefault(ACCRUED_PREMIUM).protectionStart(BEGINNING).paymentPeriods(PAYMENTS).protectionEndDate(PAYMENTS[PAYMENTS.Count - 1].EffectiveEndDate).settlementDateOffset(SETTLE_DAY_ADJ).stepinDateOffset(STEPIN_DAY_ADJ).build();
		LocalDate date1 = LocalDate.of(2016, 3, 22);
		assertEquals(test1.calculateEffectiveStartDate(date1), date1.minusDays(1));
		LocalDate date2 = LocalDate.of(2013, 9, 22);
		assertEquals(test1.calculateEffectiveStartDate(date2), START_DATE.minusDays(1));
		ResolvedCds test2 = ResolvedCds.builder().buySell(BUY).dayCount(ACT_360).legalEntityId(LEGAL_ENTITY).paymentOnDefault(ACCRUED_PREMIUM).protectionStart(ProtectionStartOfDay.NONE).paymentPeriods(PAYMENTS).protectionEndDate(PAYMENTS[PAYMENTS.Count - 1].EffectiveEndDate).settlementDateOffset(SETTLE_DAY_ADJ).stepinDateOffset(STEPIN_DAY_ADJ).build();
		LocalDate date3 = LocalDate.of(2016, 3, 22);
		assertEquals(test2.calculateEffectiveStartDate(date3), date3);
		LocalDate date4 = LocalDate.of(2013, 9, 22);
		assertEquals(test2.calculateEffectiveStartDate(date4), START_DATE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ResolvedCds test1 = ResolvedCds.builder().buySell(BUY).dayCount(ACT_360).legalEntityId(LEGAL_ENTITY).paymentOnDefault(ACCRUED_PREMIUM).protectionStart(BEGINNING).paymentPeriods(PAYMENTS).protectionEndDate(PAYMENTS[PAYMENTS.Count - 1].EffectiveEndDate).settlementDateOffset(SETTLE_DAY_ADJ).stepinDateOffset(STEPIN_DAY_ADJ).build();
		coverImmutableBean(test1);
		ResolvedCds test2 = ResolvedCds.builder().buySell(BuySell.SELL).dayCount(DayCounts.ACT_365F).legalEntityId(StandardId.of("OG", "EFG")).paymentOnDefault(PaymentOnDefault.NONE).protectionStart(ProtectionStartOfDay.NONE).paymentPeriods(PAYMENTS[0]).protectionEndDate(PAYMENTS[0].EffectiveEndDate).settlementDateOffset(DaysAdjustment.NONE).stepinDateOffset(DaysAdjustment.NONE).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		ResolvedCds test = ResolvedCds.builder().buySell(BUY).dayCount(ACT_360).legalEntityId(LEGAL_ENTITY).paymentOnDefault(ACCRUED_PREMIUM).protectionStart(BEGINNING).paymentPeriods(PAYMENTS).protectionEndDate(PAYMENTS[PAYMENTS.Count - 1].EffectiveEndDate).settlementDateOffset(SETTLE_DAY_ADJ).stepinDateOffset(STEPIN_DAY_ADJ).build();
		assertSerialization(test);
	  }

	}

}