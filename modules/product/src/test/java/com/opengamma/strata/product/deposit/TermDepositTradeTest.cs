/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.deposit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// Test <seealso cref="TermDepositTrade"/>. 
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TermDepositTradeTest
	public class TermDepositTradeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private static readonly TermDeposit DEPOSIT = TermDeposit.builder().buySell(BuySell.BUY).currency(GBP).notional(100_000_000d).startDate(LocalDate.of(2015, 1, 19)).endDate(LocalDate.of(2015, 7, 19)).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO)).dayCount(ACT_365F).rate(0.0250).build();
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2014, 6, 30));

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		TermDepositTrade test = TermDepositTrade.of(TRADE_INFO, DEPOSIT);
		assertEquals(test.Product, DEPOSIT);
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.withInfo(TRADE_INFO).Info, TRADE_INFO);
	  }

	  public virtual void test_builder()
	  {
		TermDepositTrade test = TermDepositTrade.builder().product(DEPOSIT).info(TRADE_INFO).build();
		assertEquals(test.Product, DEPOSIT);
		assertEquals(test.Info, TRADE_INFO);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		TermDepositTrade trade = TermDepositTrade.builder().product(DEPOSIT).info(TRADE_INFO).build();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(TRADE_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.TERM_DEPOSIT).currencies(Currency.GBP).description("6M GBP 100mm Deposit 2.5% : 19Jan15-19Jul15").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		TermDepositTrade test = TermDepositTrade.of(TRADE_INFO, DEPOSIT);
		assertEquals(test.resolve(REF_DATA).Info, TRADE_INFO);
		assertEquals(test.resolve(REF_DATA).Product, DEPOSIT.resolve(REF_DATA));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		TermDepositTrade test1 = TermDepositTrade.builder().product(DEPOSIT).info(TRADE_INFO).build();
		coverImmutableBean(test1);
		TermDepositTrade test2 = TermDepositTrade.builder().product(DEPOSIT).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		TermDepositTrade test = TermDepositTrade.builder().product(DEPOSIT).info(TRADE_INFO).build();
		assertSerialization(test);
	  }

	}

}