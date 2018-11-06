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
	/// Test <seealso cref="FxNdfTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxNdfTradeTest
	public class FxNdfTradeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly FxNdf PRODUCT = FxNdfTest.sut();
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2015, 1, 15));

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		FxNdfTrade test = FxNdfTrade.of(TRADE_INFO, PRODUCT);
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Product.CurrencyPair, PRODUCT.CurrencyPair);
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.withInfo(TRADE_INFO).Info, TRADE_INFO);
	  }

	  public virtual void test_builder()
	  {
		FxNdfTrade test = sut();
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Info, TRADE_INFO);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		FxNdfTrade trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.FX_NDF).currencies(Currency.GBP, Currency.USD).description("Rec GBP 100mm @ GBP/USD 1.5 NDF : 19Mar15").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		FxNdfTrade test = sut();
		ResolvedFxNdfTrade expected = ResolvedFxNdfTrade.of(TRADE_INFO, PRODUCT.resolve(REF_DATA));
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
	  internal static FxNdfTrade sut()
	  {
		return FxNdfTrade.builder().product(PRODUCT).info(TRADE_INFO).build();
	  }

	  internal static FxNdfTrade sut2()
	  {
		return FxNdfTrade.builder().product(PRODUCT).build();
	  }

	}

}