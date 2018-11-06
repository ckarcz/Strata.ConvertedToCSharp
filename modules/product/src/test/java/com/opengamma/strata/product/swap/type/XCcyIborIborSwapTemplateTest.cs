/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_10Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_2Y;
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
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;

	/// <summary>
	/// Test <seealso cref="XCcyIborIborSwapTemplate"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class XCcyIborIborSwapTemplateTest
	public class XCcyIborIborSwapTemplateTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly HolidayCalendarId EUTA_USNY = EUTA.combinedWith(USNY);

	  private const double NOTIONAL_2M = 2_000_000d;
	  private static readonly CurrencyPair EUR_USD = CurrencyPair.of(Currency.EUR, Currency.USD);
	  private const double FX_EUR_USD = 1.15d;
	  private static readonly DaysAdjustment PLUS_TWO_DAY = DaysAdjustment.ofBusinessDays(2, EUTA_USNY);
	  private static readonly IborRateSwapLegConvention EUR3M = IborRateSwapLegConvention.builder().index(IborIndices.EUR_EURIBOR_3M).accrualBusinessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA_USNY)).build();
	  private static readonly IborRateSwapLegConvention USD3M = IborRateSwapLegConvention.builder().index(IborIndices.USD_LIBOR_3M).accrualBusinessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA_USNY)).build();
	  private static readonly XCcyIborIborSwapConvention CONV = ImmutableXCcyIborIborSwapConvention.of("EUR-EURIBOR-3M-USD-LIBOR-3M", EUR3M, USD3M, PLUS_TWO_DAY);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_spot()
	  {
		XCcyIborIborSwapTemplate test = XCcyIborIborSwapTemplate.of(TENOR_10Y, CONV);
		assertEquals(test.PeriodToStart, Period.ZERO);
		assertEquals(test.Tenor, TENOR_10Y);
		assertEquals(test.Convention, CONV);
		assertEquals(test.CurrencyPair, EUR_USD);
	  }

	  public virtual void test_of()
	  {
		XCcyIborIborSwapTemplate test = XCcyIborIborSwapTemplate.of(Period.ofMonths(3), TENOR_10Y, CONV);
		assertEquals(test.PeriodToStart, Period.ofMonths(3));
		assertEquals(test.Tenor, TENOR_10Y);
		assertEquals(test.Convention, CONV);
		assertEquals(test.CurrencyPair, EUR_USD);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_notEnoughData()
	  {
		assertThrowsIllegalArg(() => XCcyIborIborSwapTemplate.builder().tenor(TENOR_2Y).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createTrade()
	  {
		XCcyIborIborSwapTemplate @base = XCcyIborIborSwapTemplate.of(Period.ofMonths(3), TENOR_10Y, CONV);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate = date(2015, 8, 7);
		LocalDate endDate = date(2025, 8, 7);
		SwapTrade test = @base.createTrade(tradeDate, BUY, NOTIONAL_2M, NOTIONAL_2M * FX_EUR_USD, 0.25d, REF_DATA);
		Swap expected = Swap.of(EUR3M.toLeg(startDate, endDate, PAY, NOTIONAL_2M, 0.25d), USD3M.toLeg(startDate, endDate, RECEIVE, NOTIONAL_2M * FX_EUR_USD));
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		XCcyIborIborSwapTemplate test = XCcyIborIborSwapTemplate.of(Period.ofMonths(3), TENOR_10Y, CONV);
		coverImmutableBean(test);
		DaysAdjustment bda2 = DaysAdjustment.ofBusinessDays(1, EUTA);
		XCcyIborIborSwapConvention conv2 = ImmutableXCcyIborIborSwapConvention.of("XXX", USD3M, EUR3M, bda2);
		XCcyIborIborSwapTemplate test2 = XCcyIborIborSwapTemplate.of(Period.ofMonths(2), TENOR_2Y, conv2);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		XCcyIborIborSwapTemplate test = XCcyIborIborSwapTemplate.of(Period.ofMonths(3), TENOR_10Y, CONV);
		assertSerialization(test);
	  }

	}

}