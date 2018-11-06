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
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
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
	/// Test <seealso cref="FixedCouponBondTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FixedCouponBondTradeTest
	public class FixedCouponBondTradeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate TRADE_DATE = date(2015, 3, 25);
	  private static readonly LocalDate SETTLEMENT_DATE = date(2015, 3, 30);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.builder().tradeDate(TRADE_DATE).settlementDate(SETTLEMENT_DATE).build();
	  private static readonly TradeInfo TRADE_INFO2 = TradeInfo.builder().tradeDate(TRADE_DATE).build();
	  private const double QUANTITY = 10;
	  private const double PRICE = 123;
	  private const double PRICE2 = 200;
	  private static readonly FixedCouponBond PRODUCT = FixedCouponBondTest.sut();
	  private static readonly FixedCouponBond PRODUCT2 = FixedCouponBondTest.sut2();

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_resolved()
	  {
		FixedCouponBondTrade test = sut();
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.Quantity, QUANTITY);
		assertEquals(test.Price, PRICE);
		assertEquals(test.withInfo(TRADE_INFO).Info, TRADE_INFO);
		assertEquals(test.withQuantity(129).Quantity, 129d, 0d);
		assertEquals(test.withPrice(129).Price, 129d, 0d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		FixedCouponBondTrade trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(TRADE_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.BOND).currencies(Currency.EUR).description("Bond x 10").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		ResolvedFixedCouponBondTrade expected = ResolvedFixedCouponBondTrade.builder().info(TRADE_INFO).product(PRODUCT.resolve(REF_DATA)).quantity(QUANTITY).settlement(ResolvedFixedCouponBondSettlement.of(SETTLEMENT_DATE, PRICE)).build();
		assertEquals(sut().resolve(REF_DATA), expected);
	  }

	  public virtual void test_resolve_noTradeOrSettlementDate()
	  {
		FixedCouponBondTrade test = FixedCouponBondTrade.builder().info(TradeInfo.empty()).product(PRODUCT).quantity(QUANTITY).price(PRICE).build();
		assertThrows(() => test.resolve(REF_DATA), typeof(System.InvalidOperationException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withQuantity()
	  {
		FixedCouponBondTrade @base = sut();
		double quantity = 75343d;
		FixedCouponBondTrade computed = @base.withQuantity(quantity);
		FixedCouponBondTrade expected = FixedCouponBondTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(quantity).price(PRICE).build();
		assertEquals(computed, expected);
	  }

	  public virtual void test_withPrice()
	  {
		FixedCouponBondTrade @base = sut();
		double price = 135d;
		FixedCouponBondTrade computed = @base.withPrice(price);
		FixedCouponBondTrade expected = FixedCouponBondTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(QUANTITY).price(price).build();
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
	  internal static FixedCouponBondTrade sut()
	  {
		return FixedCouponBondTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(QUANTITY).price(PRICE).build();
	  }

	  internal static FixedCouponBondTrade sut2()
	  {
		return FixedCouponBondTrade.builder().info(TRADE_INFO2).product(PRODUCT2).quantity(100L).price(PRICE2).build();
	  }

	}

}