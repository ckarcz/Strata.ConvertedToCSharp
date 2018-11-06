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
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
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
	/// Test <seealso cref="BillSecurity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BillSecurityTest
	public class BillSecurityTest
	{

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
	  private static readonly SecurityPriceInfo PRICE_INFO = SecurityPriceInfo.of(0.1, CurrencyAmount.of(CCY, 25));
	  private static readonly SecurityInfo INFO = SecurityInfo.of(SECURITY_ID, PRICE_INFO);

	  public virtual void test_builder()
	  {
		BillSecurity test = BillSecurity.builder().dayCount(DAY_COUNT).info(INFO).legalEntityId(LEGAL_ENTITY).notional(NOTIONAL).settlementDateOffset(SETTLE).yieldConvention(YIELD_CONVENTION).build();
		assertEquals(test.Currency, CCY);
		assertEquals(test.DayCount, DAY_COUNT);
		assertEquals(test.Info, INFO);
		assertEquals(test.LegalEntityId, LEGAL_ENTITY);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.SecurityId, SECURITY_ID);
		assertEquals(test.SettlementDateOffset, SETTLE);
		assertEquals(test.UnderlyingIds, ImmutableSet.of());
		assertEquals(test.YieldConvention, YIELD_CONVENTION);
	  }

	  public virtual void test_builder_fail()
	  {
		assertThrowsIllegalArg(() => BillSecurity.builder().dayCount(DAY_COUNT).info(INFO).legalEntityId(LEGAL_ENTITY).notional(NOTIONAL).settlementDateOffset(DaysAdjustment.ofBusinessDays(-1, USNY, BUSINESS_ADJUST)).yieldConvention(YIELD_CONVENTION).build());
		assertThrowsIllegalArg(() => BillSecurity.builder().dayCount(DAY_COUNT).info(INFO).legalEntityId(LEGAL_ENTITY).notional(AdjustablePayment.of(CurrencyAmount.of(CCY, -2_000_000), MATURITY_DATE_ADJ)).settlementDateOffset(SETTLE).yieldConvention(YIELD_CONVENTION).build());
	  }

	  public virtual void test_withInfo()
	  {
		BillSecurity @base = BillSecurity.builder().dayCount(DAY_COUNT).info(INFO).legalEntityId(LEGAL_ENTITY).notional(NOTIONAL).settlementDateOffset(SETTLE).yieldConvention(YIELD_CONVENTION).build();
		SecurityInfo info = SecurityInfo.of(SECURITY_ID, SecurityPriceInfo.ofCurrencyMinorUnit(CCY));
		BillSecurity expected = BillSecurity.builder().dayCount(DAY_COUNT).info(info).legalEntityId(LEGAL_ENTITY).notional(NOTIONAL).settlementDateOffset(SETTLE).yieldConvention(YIELD_CONVENTION).build();
		assertEquals(@base.withInfo(info), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createProduct()
	  {
		BillSecurity @base = BillSecurity.builder().dayCount(DAY_COUNT).info(INFO).legalEntityId(LEGAL_ENTITY).notional(NOTIONAL).settlementDateOffset(SETTLE).yieldConvention(YIELD_CONVENTION).build();
		Bill expectedProduct = Bill.builder().dayCount(DAY_COUNT).securityId(SECURITY_ID).dayCount(DAY_COUNT).legalEntityId(LEGAL_ENTITY).notional(NOTIONAL).settlementDateOffset(SETTLE).yieldConvention(YIELD_CONVENTION).build();
		assertEquals(@base.createProduct(ReferenceData.empty()), expectedProduct);
		TradeInfo tradeInfo = TradeInfo.of(date(2016, 6, 30));
		BillTrade expectedTrade = BillTrade.builder().info(tradeInfo).product(expectedProduct).quantity(100).price(1.235).build();
		assertEquals(@base.createTrade(tradeInfo, 100, 1.235, ReferenceData.empty()), expectedTrade);
	  }

	  public virtual void test_createPosition()
	  {
		BillSecurity test = BillSecurity.builder().dayCount(DAY_COUNT).info(INFO).legalEntityId(LEGAL_ENTITY).notional(NOTIONAL).settlementDateOffset(SETTLE).yieldConvention(YIELD_CONVENTION).build();
		Bill product = Bill.builder().dayCount(DAY_COUNT).securityId(SECURITY_ID).dayCount(DAY_COUNT).legalEntityId(LEGAL_ENTITY).notional(NOTIONAL).settlementDateOffset(SETTLE).yieldConvention(YIELD_CONVENTION).build();
		PositionInfo positionInfo = PositionInfo.empty();
		BillPosition expectedPosition1 = BillPosition.builder().info(positionInfo).product(product).longQuantity(100).build();
		assertEquals(test.createPosition(positionInfo, 100, ReferenceData.empty()), expectedPosition1);
		BillPosition expectedPosition2 = BillPosition.builder().info(positionInfo).product(product).longQuantity(100).shortQuantity(50).build();
		assertEquals(test.createPosition(positionInfo, 100, 50, ReferenceData.empty()), expectedPosition2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		BillSecurity test1 = BillSecurity.builder().dayCount(DAY_COUNT).info(INFO).legalEntityId(LEGAL_ENTITY).notional(NOTIONAL).settlementDateOffset(SETTLE).yieldConvention(YIELD_CONVENTION).build();
		coverImmutableBean(test1);
		BillSecurity test2 = BillSecurity.builder().dayCount(DayCounts.ACT_365F).info(SecurityInfo.of(SecurityId.of("OG-Test", "ID2"), PRICE_INFO)).legalEntityId(LegalEntityId.of("OG-Ticker", "LE2")).notional(AdjustablePayment.of(CurrencyAmount.of(CCY, 10), MATURITY_DATE_ADJ)).settlementDateOffset(DaysAdjustment.ofBusinessDays(2, EUTA, BUSINESS_ADJUST)).yieldConvention(BillYieldConvention.INTEREST_AT_MATURITY).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		BillSecurity test = BillSecurity.builder().dayCount(DAY_COUNT).info(INFO).legalEntityId(LEGAL_ENTITY).notional(NOTIONAL).settlementDateOffset(SETTLE).yieldConvention(YIELD_CONVENTION).build();
		assertSerialization(test);
	  }

	}

}