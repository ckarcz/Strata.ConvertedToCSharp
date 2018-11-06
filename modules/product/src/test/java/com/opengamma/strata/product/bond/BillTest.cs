/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertThrows;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;

	/// <summary>
	/// Tests <seealso cref="Bill"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BillTest
	public class BillTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly BusinessDayAdjustment BUSINESS_ADJUST = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, USNY);
	  private const BillYieldConvention YIELD_CONVENTION = BillYieldConvention.DISCOUNT;
	  private static readonly LegalEntityId LEGAL_ENTITY = LegalEntityId.of("OG-Ticker", "US GOVT");
	  private static readonly Currency CCY = Currency.USD;
	  private const double NOTIONAL_AMOUNT = 1_000_000;
	  private static readonly LocalDate MATURITY_DATE = LocalDate.of(2019, 5, 23);
	  private static readonly AdjustableDate MATURITY_DATE_ADJ = AdjustableDate.of(MATURITY_DATE, BUSINESS_ADJUST);
	  private static readonly AdjustablePayment NOTIONAL = AdjustablePayment.of(CurrencyAmount.of(CCY, NOTIONAL_AMOUNT), MATURITY_DATE_ADJ);
	  private static readonly DayCount DAY_COUNT = DayCounts.ACT_360;
	  private static readonly SecurityId SECURITY_ID = SecurityId.of("OG-Test", "Bill2019-05-23");
	  private static readonly DaysAdjustment SETTLE = DaysAdjustment.ofBusinessDays(1, USNY, BUSINESS_ADJUST);
	  public static readonly Bill US_BILL = Bill.builder().dayCount(DAY_COUNT).legalEntityId(LEGAL_ENTITY).notional(NOTIONAL).securityId(SECURITY_ID).settlementDateOffset(SETTLE).yieldConvention(YIELD_CONVENTION).build();
	  public static readonly Bill BILL_2 = Bill.builder().dayCount(DayCounts.ACT_365F).legalEntityId(LegalEntityId.of("OG-Ticker", "LE2")).notional(AdjustablePayment.of(CurrencyAmount.of(CCY, 10), MATURITY_DATE_ADJ)).securityId(SecurityId.of("OG-Test", "ID2")).settlementDateOffset(DaysAdjustment.ofBusinessDays(2, EUTA, BUSINESS_ADJUST)).yieldConvention(BillYieldConvention.INTEREST_AT_MATURITY).build();
	  private const double TOLERANCE_PRICE = 1.0E-8;

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		assertEquals(US_BILL.Currency, CCY);
		assertEquals(US_BILL.DayCount, DAY_COUNT);
		assertEquals(US_BILL.LegalEntityId, LEGAL_ENTITY);
		assertEquals(US_BILL.Notional, NOTIONAL);
		assertEquals(US_BILL.SecurityId, SECURITY_ID);
		assertEquals(US_BILL.SettlementDateOffset, SETTLE);
		assertEquals(US_BILL.YieldConvention, YIELD_CONVENTION);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void price_from_yield_discount()
	  {
		double yield = 0.01;
		LocalDate settlementDate = LocalDate.of(2018, 8, 17);
		double af = US_BILL.DayCount.relativeYearFraction(settlementDate, MATURITY_DATE);
		double priceExpected = 1.0d - yield * af;
		double priceComputed = US_BILL.priceFromYield(yield, settlementDate);
		assertEquals(priceExpected, priceComputed, TOLERANCE_PRICE);
	  }

	  public virtual void yield_from_price_discount()
	  {
		double price = 0.99;
		LocalDate settlementDate = LocalDate.of(2018, 8, 17);
		double af = US_BILL.DayCount.relativeYearFraction(settlementDate, MATURITY_DATE);
		double yieldExpected = (1.0d - price) / af;
		double yieldComputed = US_BILL.yieldFromPrice(price, settlementDate);
		assertEquals(yieldExpected, yieldComputed, TOLERANCE_PRICE);
	  }

	  public virtual void price_from_yield_intatmat()
	  {
		Bill bill = US_BILL.toBuilder().yieldConvention(BillYieldConvention.INTEREST_AT_MATURITY).build();
		double yield = 0.01;
		LocalDate settlementDate = LocalDate.of(2018, 8, 17);
		double af = bill.DayCount.relativeYearFraction(settlementDate, MATURITY_DATE);
		double priceExpected = 1.0d / (1 + yield * af);
		double priceComputed = bill.priceFromYield(yield, settlementDate);
		assertEquals(priceExpected, priceComputed, TOLERANCE_PRICE);
	  }

	  public virtual void yield_from_price_intatmat()
	  {
		Bill bill = US_BILL.toBuilder().yieldConvention(BillYieldConvention.INTEREST_AT_MATURITY).build();
		double price = 0.99;
		LocalDate settlementDate = LocalDate.of(2018, 8, 17);
		double af = bill.DayCount.relativeYearFraction(settlementDate, MATURITY_DATE);
		double yieldExpected = (1.0d / price - 1.0d) / af;
		double yieldComputed = bill.yieldFromPrice(price, settlementDate);
		assertEquals(yieldExpected, yieldComputed, TOLERANCE_PRICE);
	  }

	  public virtual void test_positive_notional()
	  {
		assertThrows(() => Bill.builder().dayCount(DAY_COUNT).legalEntityId(LEGAL_ENTITY).notional(AdjustablePayment.of(CurrencyAmount.of(CCY, -10), MATURITY_DATE_ADJ)).securityId(SECURITY_ID).settlementDateOffset(SETTLE).yieldConvention(YIELD_CONVENTION).build());
	  }

	  public virtual void test_positive_offset()
	  {
		assertThrows(() => Bill.builder().dayCount(DAY_COUNT).legalEntityId(LEGAL_ENTITY).notional(NOTIONAL).securityId(SECURITY_ID).settlementDateOffset(DaysAdjustment.ofBusinessDays(-11, USNY, BUSINESS_ADJUST)).yieldConvention(YIELD_CONVENTION).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		ResolvedBill resolved = US_BILL.resolve(REF_DATA);
		assertEquals(resolved.DayCount, DAY_COUNT);
		assertEquals(resolved.LegalEntityId, LEGAL_ENTITY);
		assertEquals(resolved.Notional, NOTIONAL.resolve(REF_DATA));
		assertEquals(resolved.SecurityId, SECURITY_ID);
		assertEquals(resolved.SettlementDateOffset, SETTLE);
		assertEquals(resolved.YieldConvention, YIELD_CONVENTION);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(US_BILL);
		coverBeanEquals(US_BILL, BILL_2);
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(US_BILL);
	  }

	}

}