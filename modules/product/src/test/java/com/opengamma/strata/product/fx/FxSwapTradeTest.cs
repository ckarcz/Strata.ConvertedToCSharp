/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fx
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
	/// Test <seealso cref="FxSwapTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxSwapTradeTest
	public class FxSwapTradeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly FxSwap PRODUCT = FxSwapTest.sut();
	  private static readonly FxSwap PRODUCT2 = FxSwapTest.sut2();
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2011, 11, 14));

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		FxSwapTrade test = FxSwapTrade.of(TRADE_INFO, PRODUCT);
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Product.CurrencyPair, PRODUCT.CurrencyPair);
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.withInfo(TRADE_INFO).Info, TRADE_INFO);
	  }

	  public virtual void test_builder()
	  {
		FxSwapTrade test = sut();
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.Product, PRODUCT);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		FxSwapTrade trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.FX_SWAP).currencies(Currency.GBP, Currency.USD).description("Rec GBP 1k @ GBP/USD 1.6 / Pay GBP 1k @ GBP/USD 1.55 : 21Nov11-21Dec11").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		FxSwapTrade test = sut();
		ResolvedFxSwapTrade expected = ResolvedFxSwapTrade.of(TRADE_INFO, PRODUCT.resolve(REF_DATA));
		assertEquals(test.resolve(REF_DATA), expected);
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
	  internal static FxSwapTrade sut()
	  {
		return FxSwapTrade.builder().product(PRODUCT).info(TRADE_INFO).build();
	  }

	  internal static FxSwapTrade sut2()
	  {
		return FxSwapTrade.builder().product(PRODUCT2).build();
	  }

	}

}