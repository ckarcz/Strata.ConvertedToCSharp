/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.index
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
	/// Test <seealso cref="IborFutureOptionTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborFutureOptionTradeTest
	public class IborFutureOptionTradeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate TRADE_DATE = date(2015, 2, 17);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(TRADE_DATE);
	  private static readonly IborFutureOption PRODUCT = IborFutureOptionTest.sut();
	  private static readonly IborFutureOption PRODUCT2 = IborFutureOptionTest.sut2();
	  private const double QUANTITY = 35;
	  private const double QUANTITY2 = 36;
	  private const double PRICE = 0.99;
	  private const double PRICE2 = 0.98;

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		IborFutureOptionTrade test = sut();
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Quantity, QUANTITY);
		assertEquals(test.Price, PRICE);
		assertEquals(test.Currency, PRODUCT.Currency);
		assertEquals(test.withInfo(TRADE_INFO).Info, TRADE_INFO);
		assertEquals(test.withQuantity(129).Quantity, 129d, 0d);
		assertEquals(test.withPrice(129).Price, 129d, 0d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		IborFutureOptionTrade trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(TRADE_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.IBOR_FUTURE_OPTION).currencies(Currency.USD).description("IborFutureOption x 35").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		IborFutureOptionTrade test = sut();
		ResolvedIborFutureOptionTrade resolved = test.resolve(REF_DATA);
		assertEquals(resolved.Info, TRADE_INFO);
		assertEquals(resolved.Product, PRODUCT.resolve(REF_DATA));
		assertEquals(resolved.Quantity, QUANTITY);
		assertEquals(resolved.TradedPrice, TradedPrice.of(TRADE_DATE, PRICE));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withQuantity()
	  {
		IborFutureOptionTrade @base = sut();
		double quantity = 65243;
		IborFutureOptionTrade computed = @base.withQuantity(quantity);
		IborFutureOptionTrade expected = IborFutureOptionTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(quantity).price(PRICE).build();
		assertEquals(computed, expected);
	  }

	  public virtual void test_withPrice()
	  {
		IborFutureOptionTrade @base = sut();
		double price = 0.95;
		IborFutureOptionTrade computed = @base.withPrice(price);
		IborFutureOptionTrade expected = IborFutureOptionTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(QUANTITY).price(price).build();
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
	  internal static IborFutureOptionTrade sut()
	  {
		return IborFutureOptionTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(QUANTITY).price(PRICE).build();
	  }

	  internal virtual IborFutureOptionTrade sut2()
	  {
		return IborFutureOptionTrade.builder().product(PRODUCT2).quantity(QUANTITY2).price(PRICE2).build();
	  }

	}

}