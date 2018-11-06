/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fx.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
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
//	import static com.opengamma.strata.product.common.BuySell.BUY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;

	/// <summary>
	/// Test <seealso cref="FxSwapTemplate"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxSwapTemplateTest
	public class FxSwapTemplateTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly CurrencyPair EUR_USD = CurrencyPair.of(Currency.EUR, Currency.USD);
	  private static readonly HolidayCalendarId EUTA_USNY = EUTA.combinedWith(USNY);
	  private static readonly DaysAdjustment PLUS_TWO_DAYS = DaysAdjustment.ofBusinessDays(2, EUTA_USNY);
	  private static readonly DaysAdjustment PLUS_ONE_DAY = DaysAdjustment.ofBusinessDays(1, EUTA_USNY);
	  private static readonly ImmutableFxSwapConvention CONVENTION = ImmutableFxSwapConvention.of(EUR_USD, PLUS_TWO_DAYS);
	  private static readonly ImmutableFxSwapConvention CONVENTION2 = ImmutableFxSwapConvention.of(EUR_USD, PLUS_ONE_DAY);
	  private static readonly Period NEAR_PERIOD = Period.ofMonths(3);
	  private static readonly Period FAR_PERIOD = Period.ofMonths(6);

	  private const double NOTIONAL_EUR = 2_000_000d;
	  private const double FX_RATE_NEAR = 1.30d;
	  private const double FX_RATE_PTS = 0.0050d;

	  public virtual void test_of_far()
	  {
		FxSwapTemplate test = FxSwapTemplate.of(FAR_PERIOD, CONVENTION);
		assertEquals(test.PeriodToNear, Period.ZERO);
		assertEquals(test.PeriodToFar, FAR_PERIOD);
		assertEquals(test.Convention, CONVENTION);
		assertEquals(test.CurrencyPair, EUR_USD);
	  }

	  public virtual void test_of_near_far()
	  {
		FxSwapTemplate test = FxSwapTemplate.of(NEAR_PERIOD, FAR_PERIOD, CONVENTION);
		assertEquals(test.PeriodToNear, NEAR_PERIOD);
		assertEquals(test.PeriodToFar, FAR_PERIOD);
		assertEquals(test.Convention, CONVENTION);
		assertEquals(test.CurrencyPair, EUR_USD);
	  }

	  public virtual void test_builder_insufficientInfo()
	  {
		assertThrowsIllegalArg(() => FxSwapTemplate.builder().convention(CONVENTION).build());
		assertThrowsIllegalArg(() => FxSwapTemplate.builder().periodToNear(NEAR_PERIOD).build());
		assertThrowsIllegalArg(() => FxSwapTemplate.builder().periodToFar(FAR_PERIOD).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createTrade()
	  {
		FxSwapTemplate @base = FxSwapTemplate.of(NEAR_PERIOD, FAR_PERIOD, CONVENTION);
		LocalDate tradeDate = LocalDate.of(2015, 10, 29);
		FxSwapTrade test = @base.createTrade(tradeDate, BUY, NOTIONAL_EUR, FX_RATE_NEAR, FX_RATE_PTS, REF_DATA);
		LocalDate spotDate = PLUS_TWO_DAYS.adjust(tradeDate, REF_DATA);
		LocalDate nearDate = spotDate.plus(NEAR_PERIOD);
		LocalDate farDate = spotDate.plus(FAR_PERIOD);
		BusinessDayAdjustment bda = CONVENTION.BusinessDayAdjustment;
		FxSwap expected = FxSwap.ofForwardPoints(CurrencyAmount.of(EUR, NOTIONAL_EUR), FxRate.of(EUR, USD, FX_RATE_NEAR), FX_RATE_PTS, nearDate, farDate, bda);
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FxSwapTemplate test = FxSwapTemplate.of(NEAR_PERIOD, FAR_PERIOD, CONVENTION);
		coverImmutableBean(test);
		FxSwapTemplate test2 = FxSwapTemplate.of(Period.ofMonths(4), Period.ofMonths(7), CONVENTION2);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		FxSwapTemplate test = FxSwapTemplate.of(NEAR_PERIOD, FAR_PERIOD, CONVENTION);
		assertSerialization(test);
	  }

	}

}