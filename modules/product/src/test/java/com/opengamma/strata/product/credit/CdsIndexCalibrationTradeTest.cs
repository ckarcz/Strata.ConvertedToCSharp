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
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.SAT_SUN;
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

	using ImmutableList = com.google.common.collect.ImmutableList;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using CdsQuoteConvention = com.opengamma.strata.product.credit.type.CdsQuoteConvention;

	/// <summary>
	/// Test <seealso cref="CdsIndexCalibrationTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CdsIndexCalibrationTradeTest
	public class CdsIndexCalibrationTradeTest
	{
	  private static readonly StandardId INDEX_ID = StandardId.of("OG", "ABCXX");
	  private static readonly ImmutableList<StandardId> LEGAL_ENTITIES = ImmutableList.of(StandardId.of("OG", "ABC1"), StandardId.of("OG", "ABC2"), StandardId.of("OG", "ABC3"));
	  private const double COUPON = 0.05;
	  private const double NOTIONAL = 1.0e9;
	  private static readonly LocalDate START_DATE = LocalDate.of(2013, 12, 20);
	  private static readonly LocalDate END_DATE = LocalDate.of(2024, 9, 20);

	  private static readonly CdsIndex PRODUCT = CdsIndex.of(BUY, INDEX_ID, LEGAL_ENTITIES, USD, NOTIONAL, START_DATE, END_DATE, P3M, SAT_SUN, COUPON);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(LocalDate.of(2014, 1, 9));
	  private static readonly AdjustablePayment UPFRONT = AdjustablePayment.of(USD, NOTIONAL, LocalDate.of(2014, 1, 12));
	  private static readonly CdsIndexTrade TRADE = CdsIndexTrade.builder().product(PRODUCT).upfrontFee(UPFRONT).info(TRADE_INFO).build();
	  private static readonly CdsQuote QUOTE1 = CdsQuote.of(CdsQuoteConvention.POINTS_UPFRONT, 0.95);
	  private static readonly CdsQuote QUOTE2 = CdsQuote.of(CdsQuoteConvention.QUOTED_SPREAD, 0.0155);
	  private static readonly CdsQuote QUOTE3 = CdsQuote.of(CdsQuoteConvention.PAR_SPREAD, 0.012);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_trade()
	  {
		CdsIndexCalibrationTrade test = CdsIndexCalibrationTrade.of(TRADE, QUOTE1);
		assertEquals(test.UnderlyingTrade, TRADE);
		assertEquals(test.Quote, QUOTE1);
		assertEquals(test.Info, TRADE.Info);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage_trade()
	  {
		CdsIndexCalibrationTrade test1 = CdsIndexCalibrationTrade.of(TRADE, QUOTE1);
		coverImmutableBean(test1);
		CdsIndexCalibrationTrade test2 = CdsIndexCalibrationTrade.of(CdsIndexTrade.builder().product(PRODUCT).info(TRADE_INFO).build(), QUOTE2);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization_trade()
	  {
		CdsIndexCalibrationTrade test = CdsIndexCalibrationTrade.of(TRADE, QUOTE3);
		assertSerialization(test);
	  }

	}

}