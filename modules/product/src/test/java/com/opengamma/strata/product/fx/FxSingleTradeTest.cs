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
	/// Test <seealso cref="FxSingleTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxSingleTradeTest
	public class FxSingleTradeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly FxSingle PRODUCT = FxSingleTest.sut();
	  private static readonly FxSingle PRODUCT2 = FxSingleTest.sut2();
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2015, 1, 15));

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		FxSingleTrade test = FxSingleTrade.of(TRADE_INFO, PRODUCT);
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Product.CurrencyPair, PRODUCT.CurrencyPair);
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.withInfo(TRADE_INFO).Info, TRADE_INFO);
	  }

	  public virtual void test_builder()
	  {
		FxSingleTrade test = FxSingleTrade.builder().product(PRODUCT).build();
		assertEquals(test.Info, TradeInfo.empty());
		assertEquals(test.Product, PRODUCT);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		FxSingleTrade trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(TRADE_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.FX_SINGLE).currencies(Currency.GBP, Currency.USD).description("Rec GBP 1k @ GBP/USD 1.6 : 30Jun15").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		FxSingleTrade test = FxSingleTrade.builder().product(PRODUCT).info(TRADE_INFO).build();
		ResolvedFxSingleTrade expected = ResolvedFxSingleTrade.of(TRADE_INFO, PRODUCT.resolve(REF_DATA));
		assertEquals(test.resolve(REF_DATA), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FxSingleTrade test = sut();
		coverImmutableBean(test);
		FxSingleTrade test2 = sut2();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(sut());
	  }

	  //-------------------------------------------------------------------------
	  internal static FxSingleTrade sut()
	  {
		return FxSingleTrade.builder().info(TradeInfo.of(date(2014, 6, 30))).product(PRODUCT).build();
	  }

	  internal static FxSingleTrade sut2()
	  {
		return FxSingleTrade.builder().product(PRODUCT2).build();
	  }

	}

}