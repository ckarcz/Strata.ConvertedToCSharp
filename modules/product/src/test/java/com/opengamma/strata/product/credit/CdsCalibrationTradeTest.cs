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
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.BUY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using StandardId = com.opengamma.strata.basics.StandardId;
	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using CdsQuoteConvention = com.opengamma.strata.product.credit.type.CdsQuoteConvention;

	/// <summary>
	/// Test <seealso cref="CdsCalibrationTrade"/> and <seealso cref="CdsQuote"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CdsCalibrationTradeTest
	public class CdsCalibrationTradeTest
	{
	  private static readonly HolidayCalendarId CALENDAR = HolidayCalendarIds.SAT_SUN;
	  private static readonly StandardId LEGAL_ENTITY = StandardId.of("OG", "ABC");
	  private const double COUPON = 0.05;
	  private const double NOTIONAL = 1.0e9;
	  private static readonly LocalDate START_DATE = LocalDate.of(2013, 12, 20);
	  private static readonly LocalDate END_DATE = LocalDate.of(2024, 9, 20);

	  private static readonly Cds PRODUCT = Cds.of(BUY, LEGAL_ENTITY, USD, NOTIONAL, START_DATE, END_DATE, P3M, CALENDAR, COUPON);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(LocalDate.of(2014, 1, 9));
	  private static readonly AdjustablePayment UPFRONT = AdjustablePayment.of(USD, NOTIONAL, LocalDate.of(2014, 1, 12));
	  private static readonly CdsTrade TRADE = CdsTrade.builder().product(PRODUCT).upfrontFee(UPFRONT).info(TRADE_INFO).build();
	  private static readonly CdsQuote QUOTE1 = CdsQuote.of(CdsQuoteConvention.POINTS_UPFRONT, 0.95);
	  private static readonly CdsQuote QUOTE2 = CdsQuote.of(CdsQuoteConvention.QUOTED_SPREAD, 0.0155);
	  private static readonly CdsQuote QUOTE3 = CdsQuote.of(CdsQuoteConvention.PAR_SPREAD, 0.012);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_quote()
	  {
		assertEquals(QUOTE3.QuoteConvention, CdsQuoteConvention.PAR_SPREAD);
		assertEquals(QUOTE3.QuotedValue, 0.012);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage_quote()
	  {
		coverImmutableBean(QUOTE2);
		coverBeanEquals(QUOTE2, QUOTE3);
	  }

	  public virtual void test_serialization_quote()
	  {
		assertSerialization(QUOTE1);
	  }

	  public virtual void test_of_trade()
	  {
		CdsCalibrationTrade test = CdsCalibrationTrade.of(TRADE, QUOTE1);
		assertEquals(test.UnderlyingTrade, TRADE);
		assertEquals(test.Quote, QUOTE1);
		assertEquals(test.Info, TRADE.Info);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage_trade()
	  {
		CdsCalibrationTrade test1 = CdsCalibrationTrade.of(TRADE, QUOTE1);
		coverImmutableBean(test1);
		CdsCalibrationTrade test2 = CdsCalibrationTrade.of(CdsTrade.builder().product(PRODUCT).info(TRADE_INFO).build(), QUOTE2);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization_trade()
	  {
		CdsCalibrationTrade test = CdsCalibrationTrade.of(TRADE, QUOTE1);
		assertSerialization(test);
	  }

	}

}