/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.dsf
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;

	/// <summary>
	/// Test <seealso cref="DsfTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DsfTradeTest
	public class DsfTradeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  internal static readonly Dsf PRODUCT = DsfTest.sut();
	  internal static readonly Dsf PRODUCT2 = DsfTest.sut2();
	  private static readonly LocalDate TRADE_DATE = LocalDate.of(2014, 6, 12);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.builder().tradeDate(TRADE_DATE).settlementDate(LocalDate.of(2014, 6, 14)).build();
	  private const double QUANTITY = 100L;
	  private const double QUANTITY2 = 200L;
	  private const double PRICE = 0.99;
	  private const double PRICE2 = 0.98;

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		DsfTrade test = sut();
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Quantity, QUANTITY);
		assertEquals(test.Price, PRICE);
		assertEquals(test.SecurityId, PRODUCT.SecurityId);
		assertEquals(test.Currency, PRODUCT.Currency);
		assertEquals(test.withInfo(TRADE_INFO).Info, TRADE_INFO);
		assertEquals(test.withQuantity(129).Quantity, 129d, 0d);
		assertEquals(test.withPrice(129).Price, 129d, 0d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		DsfTrade trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(TRADE_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.DSF).currencies(Currency.USD).description("DSF x 100").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		DsfTrade test = sut();
		ResolvedDsfTrade expected = ResolvedDsfTrade.builder().info(TRADE_INFO).product(PRODUCT.resolve(REF_DATA)).quantity(QUANTITY).tradedPrice(TradedPrice.of(TRADE_DATE, PRICE)).build();
		assertEquals(test.resolve(REF_DATA), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withQuantity()
	  {
		DsfTrade @base = sut();
		double quantity = 6423d;
		DsfTrade computed = @base.withQuantity(quantity);
		DsfTrade expected = DsfTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(quantity).price(PRICE).build();
		assertEquals(computed, expected);
	  }

	  public virtual void test_withPrice()
	  {
		DsfTrade @base = sut();
		double price = 6423d;
		DsfTrade computed = @base.withPrice(price);
		DsfTrade expected = DsfTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(QUANTITY).price(price).build();
		assertEquals(computed, expected);
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
	  internal static DsfTrade sut()
	  {
		return DsfTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(QUANTITY).price(PRICE).build();
	  }

	  internal static DsfTrade sut2()
	  {
		return DsfTrade.builder().product(PRODUCT2).quantity(QUANTITY2).price(PRICE2).build();
	  }

	}

}