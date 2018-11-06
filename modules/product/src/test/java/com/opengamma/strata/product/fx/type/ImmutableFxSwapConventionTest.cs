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
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
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
	/// Tests <seealso cref="ImmutableFxSwapConvention"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ImmutableFxSwapConventionTest
	public class ImmutableFxSwapConventionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly CurrencyPair EUR_USD = CurrencyPair.of(Currency.EUR, Currency.USD);
	  private static readonly CurrencyPair GBP_USD = CurrencyPair.of(Currency.GBP, Currency.USD);
	  private static readonly HolidayCalendarId EUTA_USNY = EUTA.combinedWith(USNY);
	  private static readonly DaysAdjustment PLUS_TWO_DAYS = DaysAdjustment.ofBusinessDays(2, EUTA_USNY);
	  private static readonly DaysAdjustment PLUS_ONE_DAY = DaysAdjustment.ofBusinessDays(1, EUTA_USNY);
	  private static readonly BusinessDayAdjustment BDA_FOLLOW = BusinessDayAdjustment.of(FOLLOWING, GBLO);
	  private static readonly BusinessDayAdjustment BDA_MODFOLLOW = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO);

	  private const double NOTIONAL_EUR = 2_000_000d;
	  private const double FX_RATE_NEAR = 1.30d;
	  private const double FX_RATE_PTS = 0.0050d;

	  //-------------------------------------------------------------------------
	  public virtual void test_of_nobda()
	  {
		ImmutableFxSwapConvention test = ImmutableFxSwapConvention.of(EUR_USD, PLUS_TWO_DAYS);
		assertEquals(test.Name, EUR_USD.ToString());
		assertEquals(test.CurrencyPair, EUR_USD);
		assertEquals(test.SpotDateOffset, PLUS_TWO_DAYS);
		assertEquals(test.BusinessDayAdjustment, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA_USNY));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_bda()
	  {
		ImmutableFxSwapConvention test = ImmutableFxSwapConvention.of(EUR_USD, PLUS_TWO_DAYS, BDA_FOLLOW);
		assertEquals(test.Name, EUR_USD.ToString());
		assertEquals(test.CurrencyPair, EUR_USD);
		assertEquals(test.SpotDateOffset, PLUS_TWO_DAYS);
		assertEquals(test.BusinessDayAdjustment, BDA_FOLLOW);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		ImmutableFxSwapConvention test = ImmutableFxSwapConvention.builder().currencyPair(EUR_USD).name("EUR::USD").spotDateOffset(PLUS_TWO_DAYS).businessDayAdjustment(BDA_FOLLOW).build();
		assertEquals(test.Name, "EUR::USD");
		assertEquals(test.CurrencyPair, EUR_USD);
		assertEquals(test.SpotDateOffset, PLUS_TWO_DAYS);
		assertEquals(test.BusinessDayAdjustment, BDA_FOLLOW);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toTrade_periods()
	  {
		ImmutableFxSwapConvention @base = ImmutableFxSwapConvention.of(EUR_USD, PLUS_TWO_DAYS, BDA_FOLLOW);
		Period startPeriod = Period.ofMonths(3);
		Period endPeriod = Period.ofMonths(6);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate spotDate = PLUS_TWO_DAYS.adjust(tradeDate, REF_DATA);
		LocalDate nearDate = spotDate.plus(startPeriod);
		LocalDate farDate = spotDate.plus(endPeriod);
		FxSwapTrade test = @base.createTrade(tradeDate, startPeriod, endPeriod, BUY, NOTIONAL_EUR, FX_RATE_NEAR, FX_RATE_PTS, REF_DATA);
		FxSwap expected = FxSwap.ofForwardPoints(CurrencyAmount.of(EUR, NOTIONAL_EUR), FxRate.of(EUR, USD, FX_RATE_NEAR), FX_RATE_PTS, nearDate, farDate, BDA_FOLLOW);
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  public virtual void test_toTrade_dates()
	  {
		ImmutableFxSwapConvention @base = ImmutableFxSwapConvention.of(EUR_USD, PLUS_TWO_DAYS, BDA_FOLLOW);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate nearDate = LocalDate.of(2015, 7, 5);
		LocalDate nearDateAdj = LocalDate.of(2015, 7, 6); // Adjusted: 5 is Sunday
		LocalDate farDate = LocalDate.of(2015, 9, 5);
		LocalDate farDateAdj = LocalDate.of(2015, 9, 7); // Adjusted: 5 is Saturday
		FxSwapTrade test = @base.toTrade(tradeDate, nearDate, farDate, BUY, NOTIONAL_EUR, FX_RATE_NEAR, FX_RATE_PTS);
		FxSwap expected = FxSwap.ofForwardPoints(CurrencyAmount.of(EUR, NOTIONAL_EUR), FxRate.of(EUR, USD, FX_RATE_NEAR), FX_RATE_PTS, nearDate, farDate, BDA_FOLLOW);
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
		ResolvedFxSwap resolvedExpected = ResolvedFxSwap.ofForwardPoints(CurrencyAmount.of(EUR, NOTIONAL_EUR), USD, FX_RATE_NEAR, FX_RATE_PTS, nearDateAdj, farDateAdj);
		assertEquals(test.Product.resolve(REF_DATA), resolvedExpected);
	  }

	  public virtual void test_toTemplate_badDateOrder()
	  {
		ImmutableFxSwapConvention @base = ImmutableFxSwapConvention.of(EUR_USD, PLUS_TWO_DAYS, BDA_FOLLOW);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate nearDate = date(2015, 4, 5);
		LocalDate farDate = date(2015, 7, 5);
		assertThrowsIllegalArg(() => @base.toTrade(tradeDate, nearDate, farDate, BUY, NOTIONAL_EUR, FX_RATE_NEAR, FX_RATE_PTS));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ImmutableFxSwapConvention test = ImmutableFxSwapConvention.of(EUR_USD, PLUS_TWO_DAYS, BDA_FOLLOW);
		coverImmutableBean(test);
		ImmutableFxSwapConvention test2 = ImmutableFxSwapConvention.builder().name("GBP/USD").currencyPair(GBP_USD).spotDateOffset(PLUS_ONE_DAY).businessDayAdjustment(BDA_MODFOLLOW).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		ImmutableFxSwapConvention test = ImmutableFxSwapConvention.of(EUR_USD, PLUS_TWO_DAYS, BDA_FOLLOW);
		assertSerialization(test);
	  }

	}

}