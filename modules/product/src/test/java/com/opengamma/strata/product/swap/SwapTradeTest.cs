/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
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

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SwapTradeTest
	public class SwapTradeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2014, 6, 30));
	  private static readonly Swap SWAP1 = Swap.of(MockSwapLeg.MOCK_GBP1, MockSwapLeg.MOCK_USD1);
	  private static readonly Swap SWAP2 = Swap.of(MockSwapLeg.MOCK_GBP1);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		SwapTrade test = SwapTrade.of(TRADE_INFO, SWAP1);
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.Product, SWAP1);
		assertEquals(test.withInfo(TRADE_INFO).Info, TRADE_INFO);
	  }

	  public virtual void test_builder()
	  {
		SwapTrade test = SwapTrade.builder().product(SWAP1).build();
		assertEquals(test.Info, TradeInfo.empty());
		assertEquals(test.Product, SWAP1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		SwapTrade trade = SwapTrade.of(TRADE_INFO, SWAP1);
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(TRADE_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.SWAP).currencies(Currency.GBP, Currency.EUR, Currency.USD).description("7M Pay [GBP-LIBOR-3M, EUR/GBP-ECB, EUR-EONIA] / Rec [GBP-LIBOR-3M, EUR/GBP-ECB, EUR-EONIA] : 15Jan12-15Aug12").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		SwapTrade test = SwapTrade.of(TRADE_INFO, SWAP1);
		assertEquals(test.resolve(REF_DATA).Info, TRADE_INFO);
		assertEquals(test.resolve(REF_DATA).Product, SWAP1.resolve(REF_DATA));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		SwapTrade test = SwapTrade.builder().info(TRADE_INFO).product(SWAP1).build();
		coverImmutableBean(test);
		SwapTrade test2 = SwapTrade.builder().product(SWAP2).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		SwapTrade test = SwapTrade.builder().info(TRADE_INFO).product(SWAP1).build();
		assertSerialization(test);
	  }

	}

}