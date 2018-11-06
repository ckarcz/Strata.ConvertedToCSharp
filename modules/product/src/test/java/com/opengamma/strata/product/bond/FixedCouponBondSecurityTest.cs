/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
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
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;

	/// <summary>
	/// Test <seealso cref="FixedCouponBondSecurity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FixedCouponBondSecurityTest
	public class FixedCouponBondSecurityTest
	{

	  private static readonly FixedCouponBond PRODUCT = FixedCouponBondTest.sut();
	  private static readonly FixedCouponBond PRODUCT2 = FixedCouponBondTest.sut2();
	  private static readonly SecurityPriceInfo PRICE_INFO = SecurityPriceInfo.of(0.1, CurrencyAmount.of(GBP, 25));
	  private static readonly SecurityInfo INFO = SecurityInfo.of(PRODUCT.SecurityId, PRICE_INFO);
	  private const FixedCouponBondYieldConvention YIELD_CONVENTION = FixedCouponBondYieldConvention.DE_BONDS;
	  private static readonly LegalEntityId LEGAL_ENTITY = LegalEntityId.of("OG-Ticker", "BUN EUR");
	  private const double NOTIONAL = 1.0e7;
	  private const double FIXED_RATE = 0.015;
	  private static readonly DaysAdjustment DATE_OFFSET = DaysAdjustment.ofBusinessDays(3, EUTA);
	  private static readonly DayCount DAY_COUNT = DayCounts.ACT_365F;
	  private static readonly LocalDate START_DATE = LocalDate.of(2015, 4, 12);
	  private static readonly LocalDate END_DATE = LocalDate.of(2025, 4, 12);
	  private static readonly BusinessDayAdjustment BUSINESS_ADJUST = BusinessDayAdjustment.of(BusinessDayConventions.MODIFIED_FOLLOWING, EUTA);
	  private static readonly PeriodicSchedule PERIOD_SCHEDULE = PeriodicSchedule.of(START_DATE, END_DATE, Frequency.P6M, BUSINESS_ADJUST, StubConvention.SHORT_INITIAL, false);
	  private const int EX_COUPON_DAYS = 5;

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		FixedCouponBondSecurity test = sut();
		assertEquals(test.Info, INFO);
		assertEquals(test.SecurityId, PRODUCT.SecurityId);
		assertEquals(test.Currency, PRODUCT.Currency);
		assertEquals(test.UnderlyingIds, ImmutableSet.of());
	  }

	  public virtual void test_builder_fail()
	  {
		assertThrowsIllegalArg(() => FixedCouponBondSecurity.builder().info(INFO).dayCount(DAY_COUNT).fixedRate(FIXED_RATE).legalEntityId(LEGAL_ENTITY).currency(EUR).notional(NOTIONAL).accrualSchedule(PERIOD_SCHEDULE).settlementDateOffset(DATE_OFFSET).yieldConvention(YIELD_CONVENTION).exCouponPeriod(DaysAdjustment.ofBusinessDays(EX_COUPON_DAYS, EUTA, BUSINESS_ADJUST)).build());
		assertThrowsIllegalArg(() => FixedCouponBondSecurity.builder().info(INFO).dayCount(DAY_COUNT).fixedRate(FIXED_RATE).legalEntityId(LEGAL_ENTITY).currency(EUR).notional(NOTIONAL).accrualSchedule(PERIOD_SCHEDULE).settlementDateOffset(DaysAdjustment.ofBusinessDays(-3, EUTA)).yieldConvention(YIELD_CONVENTION).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createProduct()
	  {
		FixedCouponBondSecurity test = sut();
		assertEquals(test.createProduct(ReferenceData.empty()), PRODUCT);
		TradeInfo tradeInfo = TradeInfo.of(date(2016, 6, 30));
		FixedCouponBondTrade expectedTrade = FixedCouponBondTrade.builder().info(tradeInfo).product(PRODUCT).quantity(100).price(123.50).build();
		assertEquals(test.createTrade(tradeInfo, 100, 123.50, ReferenceData.empty()), expectedTrade);
	  }

	  public virtual void test_createPosition()
	  {
		FixedCouponBondSecurity test = sut();
		PositionInfo positionInfo = PositionInfo.empty();
		FixedCouponBondPosition expectedPosition1 = FixedCouponBondPosition.builder().info(positionInfo).product(PRODUCT).longQuantity(100).build();
		assertEquals(test.createPosition(positionInfo, 100, ReferenceData.empty()), expectedPosition1);
		FixedCouponBondPosition expectedPosition2 = FixedCouponBondPosition.builder().info(positionInfo).product(PRODUCT).longQuantity(100).shortQuantity(50).build();
		assertEquals(test.createPosition(positionInfo, 100, 50, ReferenceData.empty()), expectedPosition2);
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
	  internal static FixedCouponBondSecurity sut()
	  {
		return createSecurity(PRODUCT);
	  }

	  internal static FixedCouponBondSecurity sut2()
	  {
		return createSecurity(PRODUCT2);
	  }

	  internal static FixedCouponBondSecurity createSecurity(FixedCouponBond product)
	  {
		return FixedCouponBondSecurity.builder().info(SecurityInfo.of(product.SecurityId, INFO.PriceInfo)).currency(product.Currency).notional(product.Notional).accrualSchedule(product.AccrualSchedule).fixedRate(product.FixedRate).dayCount(product.DayCount).yieldConvention(product.YieldConvention).legalEntityId(product.LegalEntityId).settlementDateOffset(product.SettlementDateOffset).exCouponPeriod(product.ExCouponPeriod).build();
	  }

	}

}