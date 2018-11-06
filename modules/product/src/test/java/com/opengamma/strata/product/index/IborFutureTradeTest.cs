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
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
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
	/// Test <seealso cref="IborFutureTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborFutureTradeTest
	public class IborFutureTradeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate TRADE_DATE = date(2015, 3, 18);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(TRADE_DATE);
	  private static readonly IborFuture PRODUCT = IborFutureTest.sut();
	  private static readonly IborFuture PRODUCT2 = IborFutureTest.sut2();
	  private const double QUANTITY = 35;
	  private const double QUANTITY2 = 36;
	  private const double PRICE = 0.99;
	  private const double PRICE2 = 0.98;

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		IborFutureTrade test = sut();
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Price, PRICE);
		assertEquals(test.Quantity, QUANTITY);
		assertEquals(test.withInfo(TRADE_INFO).Info, TRADE_INFO);
		assertEquals(test.withQuantity(0.9129).Quantity, 0.9129d, 1e-10);
		assertEquals(test.withPrice(0.9129).Price, 0.9129d, 1e-10);
	  }

	  public virtual void test_builder_badPrice()
	  {
		assertThrowsIllegalArg(() => sut().toBuilder().price(2.1).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		IborFutureTrade trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(TRADE_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.IBOR_FUTURE).currencies(Currency.USD).description("IborFuture x 35").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		IborFutureTrade test = sut();
		ResolvedIborFutureTrade resolved = test.resolve(REF_DATA);
		assertEquals(resolved.Info, TRADE_INFO);
		assertEquals(resolved.Product, PRODUCT.resolve(REF_DATA));
		assertEquals(resolved.Quantity, QUANTITY);
		assertEquals(resolved.TradedPrice, TradedPrice.of(TRADE_DATE, PRICE));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withQuantity()
	  {
		IborFutureTrade @base = sut();
		double quantity = 65243;
		IborFutureTrade computed = @base.withQuantity(quantity);
		IborFutureTrade expected = IborFutureTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(quantity).price(PRICE).build();
		assertEquals(computed, expected);
	  }

	  public virtual void test_withPrice()
	  {
		IborFutureTrade @base = sut();
		double price = 0.95;
		IborFutureTrade computed = @base.withPrice(price);
		IborFutureTrade expected = IborFutureTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(QUANTITY).price(price).build();
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
	  internal static IborFutureTrade sut()
	  {
		return IborFutureTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(QUANTITY).price(PRICE).build();
	  }

	  internal static IborFutureTrade sut2()
	  {
		return IborFutureTrade.builder().product(PRODUCT2).quantity(QUANTITY2).price(PRICE2).build();
	  }

	}

}