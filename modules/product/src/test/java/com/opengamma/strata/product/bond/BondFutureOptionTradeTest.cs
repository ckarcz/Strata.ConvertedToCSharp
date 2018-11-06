/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
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
	/// Test <seealso cref="BondFutureOptionTrade"/>. 
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BondFutureOptionTradeTest
	public class BondFutureOptionTradeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private static readonly BondFutureOption OPTION_PRODUCT = BondFutureOptionTest.sut();
	  private static readonly BondFutureOption OPTION_PRODUCT2 = BondFutureOptionTest.sut2();
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2014, 3, 31));
	  private static readonly TradeInfo TRADE_INFO2 = TradeInfo.of(date(2014, 4, 1));
	  private const double QUANTITY = 1234;
	  private const double QUANTITY2 = 100;
	  private const double? PRICE = 0.01;
	  private const double? PRICE2 = 0.02;

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		BondFutureOptionTrade test = sut();
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.Product, OPTION_PRODUCT);
		assertEquals(test.Quantity, QUANTITY);
		assertEquals(test.Price, PRICE);
		assertEquals(test.withInfo(TRADE_INFO).Info, TRADE_INFO);
		assertEquals(test.withQuantity(129).Quantity, 129d, 0d);
		assertEquals(test.withPrice(129).Price, 129d, 0d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		BondFutureOptionTrade trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(TRADE_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.BOND_FUTURE_OPTION).currencies(Currency.USD).description("BondFutureOption x 1234").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		ResolvedBondFutureOptionTrade expected = ResolvedBondFutureOptionTrade.builder().info(TRADE_INFO).product(OPTION_PRODUCT.resolve(REF_DATA)).quantity(QUANTITY).tradedPrice(TradedPrice.of(TRADE_INFO.TradeDate.get(), PRICE.Value)).build();
		assertEquals(sut().resolve(REF_DATA), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withQuantity()
	  {
		BondFutureOptionTrade @base = sut();
		double quantity = 5432d;
		BondFutureOptionTrade computed = @base.withQuantity(quantity);
		BondFutureOptionTrade expected = BondFutureOptionTrade.builder().info(TRADE_INFO).product(OPTION_PRODUCT).quantity(quantity).price(PRICE.Value).build();
		assertEquals(computed, expected);
	  }

	  public virtual void test_withPrice()
	  {
		BondFutureOptionTrade @base = sut();
		double price = 0.05d;
		BondFutureOptionTrade computed = @base.withPrice(price);
		BondFutureOptionTrade expected = BondFutureOptionTrade.builder().info(TRADE_INFO).product(OPTION_PRODUCT).quantity(QUANTITY).price(price).build();
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
	  internal static BondFutureOptionTrade sut()
	  {
		return BondFutureOptionTrade.builder().info(TRADE_INFO).product(OPTION_PRODUCT).quantity(QUANTITY).price(PRICE.Value).build();
	  }

	  internal static BondFutureOptionTrade sut2()
	  {
		return BondFutureOptionTrade.builder().info(TRADE_INFO2).product(OPTION_PRODUCT2).quantity(QUANTITY2).price(PRICE2.Value).build();
	  }

	}

}