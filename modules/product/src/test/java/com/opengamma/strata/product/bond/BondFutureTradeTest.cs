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
	/// Test <seealso cref="BondFutureTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BondFutureTradeTest
	public class BondFutureTradeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  // future
	  private static readonly BondFuture FUTURE = BondFutureTest.sut();
	  private static readonly BondFuture FUTURE2 = BondFutureTest.sut2();
	  // trade
	  private static readonly LocalDate TRADE_DATE = date(2011, 6, 20);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(TRADE_DATE);
	  private static readonly TradeInfo TRADE_INFO2 = TradeInfo.of(date(2016, 7, 1));
	  private const double QUANTITY = 1234L;
	  private const double QUANTITY2 = 100L;
	  private const double PRICE = 1.2345;
	  private const double PRICE2 = 1.3;

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		BondFutureTrade test = sut();
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.Product, FUTURE);
		assertEquals(test.Quantity, QUANTITY);
		assertEquals(test.Price, PRICE);
		assertEquals(test.withInfo(TRADE_INFO).Info, TRADE_INFO);
		assertEquals(test.withQuantity(129).Quantity, 129d, 0d);
		assertEquals(test.withPrice(129).Price, 129d, 0d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		BondFutureTrade trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(TRADE_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.BOND_FUTURE).currencies(Currency.USD).description("BondFuture x 1234").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		ResolvedBondFutureTrade expected = ResolvedBondFutureTrade.builder().info(TRADE_INFO).product(FUTURE.resolve(REF_DATA)).quantity(QUANTITY).tradedPrice(TradedPrice.of(TRADE_INFO.TradeDate.get(), PRICE)).build();
		assertEquals(sut().resolve(REF_DATA), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withQuantity()
	  {
		BondFutureTrade @base = sut();
		double quantity = 366d;
		BondFutureTrade computed = @base.withQuantity(quantity);
		BondFutureTrade expected = BondFutureTrade.builder().info(TRADE_INFO).product(FUTURE).quantity(quantity).price(PRICE).build();
		assertEquals(computed, expected);
	  }

	  public virtual void test_withPrice()
	  {
		BondFutureTrade @base = sut();
		double price = 1.5d;
		BondFutureTrade computed = @base.withPrice(price);
		BondFutureTrade expected = BondFutureTrade.builder().info(TRADE_INFO).product(FUTURE).quantity(QUANTITY).price(price).build();
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

	  internal static BondFutureTrade sut()
	  {
		return BondFutureTrade.builder().info(TRADE_INFO).product(FUTURE).quantity(QUANTITY).price(PRICE).build();
	  }

	  internal static BondFutureTrade sut2()
	  {
		return BondFutureTrade.builder().info(TRADE_INFO2).product(FUTURE2).quantity(QUANTITY2).price(PRICE2).build();
	  }

	}

}