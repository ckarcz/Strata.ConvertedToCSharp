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
//	import static com.opengamma.strata.basics.index.PriceIndices.GB_HICP;
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
//	import static com.opengamma.strata.product.bond.CapitalIndexedBondYieldConvention.GB_IL_FLOAT;
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
	using InflationRateCalculation = com.opengamma.strata.product.swap.InflationRateCalculation;
	using PriceIndexCalculationMethod = com.opengamma.strata.product.swap.PriceIndexCalculationMethod;

	/// <summary>
	/// Test <seealso cref="CapitalIndexedBondSecurity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CapitalIndexedBondSecurityTest
	public class CapitalIndexedBondSecurityTest
	{

	  private static readonly CapitalIndexedBond PRODUCT = CapitalIndexedBondTest.sut();
	  private static readonly CapitalIndexedBond PRODUCT2 = CapitalIndexedBondTest.sut2();
	  private static readonly SecurityPriceInfo PRICE_INFO = SecurityPriceInfo.of(0.1, CurrencyAmount.of(GBP, 25));
	  private static readonly SecurityInfo INFO = SecurityInfo.of(PRODUCT.SecurityId, PRICE_INFO);
	  private static readonly CapitalIndexedBondYieldConvention YIELD_CONVENTION = GB_IL_FLOAT;
	  private static readonly LegalEntityId LEGAL_ENTITY = LegalEntityId.of("OG-Ticker", "BUN EUR");
	  private const double NOTIONAL = 1.0e7;
	  private static readonly InflationRateCalculation RATE = InflationRateCalculation.of(GB_HICP, 3, PriceIndexCalculationMethod.MONTHLY, 120d);
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
		CapitalIndexedBondSecurity test = sut();
		assertEquals(test.Info, INFO);
		assertEquals(test.SecurityId, PRODUCT.SecurityId);
		assertEquals(test.Currency, PRODUCT.Currency);
		assertEquals(test.UnderlyingIds, ImmutableSet.of());
		assertEquals(test.FirstIndexValue, PRODUCT.FirstIndexValue);
	  }

	  public virtual void test_builder_fail()
	  {
		assertThrowsIllegalArg(() => CapitalIndexedBondSecurity.builder().info(INFO).dayCount(DAY_COUNT).rateCalculation(RATE).legalEntityId(LEGAL_ENTITY).currency(EUR).notional(NOTIONAL).accrualSchedule(PERIOD_SCHEDULE).settlementDateOffset(DATE_OFFSET).yieldConvention(YIELD_CONVENTION).exCouponPeriod(DaysAdjustment.ofBusinessDays(EX_COUPON_DAYS, EUTA, BUSINESS_ADJUST)).build());
		assertThrowsIllegalArg(() => CapitalIndexedBondSecurity.builder().info(INFO).dayCount(DAY_COUNT).rateCalculation(RATE).legalEntityId(LEGAL_ENTITY).currency(EUR).notional(NOTIONAL).accrualSchedule(PERIOD_SCHEDULE).settlementDateOffset(DaysAdjustment.ofBusinessDays(-3, EUTA)).yieldConvention(YIELD_CONVENTION).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createProduct()
	  {
		CapitalIndexedBondSecurity test = sut();
		assertEquals(test.createProduct(ReferenceData.empty()), PRODUCT);
		TradeInfo tradeInfo = TradeInfo.builder().tradeDate(date(2016, 6, 30)).settlementDate(date(2016, 7, 1)).build();
		CapitalIndexedBondTrade expectedTrade = CapitalIndexedBondTrade.builder().info(tradeInfo).product(PRODUCT).quantity(100).price(123.50).build();
		assertEquals(test.createTrade(tradeInfo, 100, 123.50, ReferenceData.empty()), expectedTrade);
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
	  internal static CapitalIndexedBondSecurity sut()
	  {
		return createSecurity(PRODUCT);
	  }

	  internal static CapitalIndexedBondSecurity sut2()
	  {
		return createSecurity(PRODUCT2);
	  }

	  internal static CapitalIndexedBondSecurity createSecurity(CapitalIndexedBond product)
	  {
		return CapitalIndexedBondSecurity.builder().info(SecurityInfo.of(product.SecurityId, INFO.PriceInfo)).currency(product.Currency).notional(product.Notional).accrualSchedule(product.AccrualSchedule).rateCalculation(product.RateCalculation).dayCount(product.DayCount).yieldConvention(product.YieldConvention).legalEntityId(product.LegalEntityId).settlementDateOffset(product.SettlementDateOffset).exCouponPeriod(product.ExCouponPeriod).build();
	  }

	}

}