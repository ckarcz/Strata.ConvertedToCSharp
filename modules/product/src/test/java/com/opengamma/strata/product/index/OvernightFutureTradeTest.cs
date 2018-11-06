/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.GBP_SONIA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
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
	using Rounding = com.opengamma.strata.basics.value.Rounding;
	using OvernightAccrualMethod = com.opengamma.strata.product.swap.OvernightAccrualMethod;

	/// <summary>
	/// Test <seealso cref="OvernightFutureTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class OvernightFutureTradeTest
	public class OvernightFutureTradeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate TRADE_DATE = date(2018, 3, 18);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(TRADE_DATE);
	  private const double NOTIONAL = 5_000_000d;
	  private const double NOTIONAL2 = 10_000_000d;
	  private static readonly double ACCRUAL_FACTOR = TENOR_1M.Period.toTotalMonths() / 12.0;
	  private static readonly double ACCRUAL_FACTOR2 = TENOR_3M.Period.toTotalMonths() / 12.0;
	  private static readonly LocalDate LAST_TRADE_DATE = date(2018, 9, 28);
	  private static readonly LocalDate START_DATE = date(2018, 9, 1);
	  private static readonly LocalDate END_DATE = date(2018, 9, 30);
	  private static readonly LocalDate LAST_TRADE_DATE2 = date(2018, 6, 15);
	  private static readonly LocalDate START_DATE2 = date(2018, 3, 15);
	  private static readonly LocalDate END_DATE2 = date(2018, 6, 15);
	  private static readonly Rounding ROUNDING = Rounding.ofDecimalPlaces(5);
	  private static readonly SecurityId SECURITY_ID = SecurityId.of("OG-Test", "OnFuture");
	  private static readonly SecurityId SECURITY_ID2 = SecurityId.of("OG-Test", "OnFuture2");
	  private static readonly OvernightFuture PRODUCT = OvernightFuture.builder().securityId(SECURITY_ID).currency(USD).notional(NOTIONAL).accrualFactor(ACCRUAL_FACTOR).startDate(START_DATE).endDate(END_DATE).lastTradeDate(LAST_TRADE_DATE).index(USD_FED_FUND).accrualMethod(OvernightAccrualMethod.AVERAGED_DAILY).rounding(ROUNDING).build();
	  private static readonly OvernightFuture PRODUCT2 = OvernightFuture.builder().securityId(SECURITY_ID2).currency(GBP).notional(NOTIONAL2).accrualFactor(ACCRUAL_FACTOR2).startDate(START_DATE2).endDate(END_DATE2).lastTradeDate(LAST_TRADE_DATE2).index(GBP_SONIA).accrualMethod(OvernightAccrualMethod.COMPOUNDED).rounding(Rounding.none()).build();
	  private const double QUANTITY = 35;
	  private const double QUANTITY2 = 36;
	  private const double PRICE = 0.99;
	  private const double PRICE2 = 0.98;

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		OvernightFutureTrade test = OvernightFutureTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(QUANTITY).price(PRICE).build();
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
		assertThrowsIllegalArg(() => OvernightFutureTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(QUANTITY).price(2.1).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		OvernightFutureTrade trade = OvernightFutureTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(QUANTITY).price(PRICE).build();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(TRADE_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.OVERNIGHT_FUTURE).currencies(Currency.USD).description("OnFuture x 35").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		OvernightFutureTrade test = OvernightFutureTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(QUANTITY).price(PRICE).build();
		ResolvedOvernightFutureTrade resolved = test.resolve(REF_DATA);
		assertEquals(resolved.Info, TRADE_INFO);
		assertEquals(resolved.Product, PRODUCT.resolve(REF_DATA));
		assertEquals(resolved.Quantity, QUANTITY);
		assertEquals(resolved.TradedPrice, TradedPrice.of(TRADE_DATE, PRICE));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withQuantity()
	  {
		OvernightFutureTrade @base = OvernightFutureTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(QUANTITY).price(PRICE).build();
		double quantity = 65243;
		OvernightFutureTrade computed = @base.withQuantity(quantity);
		OvernightFutureTrade expected = OvernightFutureTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(quantity).price(PRICE).build();
		assertEquals(computed, expected);
	  }

	  public virtual void test_withPrice()
	  {
		OvernightFutureTrade @base = OvernightFutureTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(QUANTITY).price(PRICE).build();
		double price = 0.95;
		OvernightFutureTrade computed = @base.withPrice(price);
		OvernightFutureTrade expected = OvernightFutureTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(QUANTITY).price(price).build();
		assertEquals(computed, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		OvernightFutureTrade test1 = OvernightFutureTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(QUANTITY).price(PRICE).build();
		coverImmutableBean(test1);
		OvernightFutureTrade test2 = OvernightFutureTrade.builder().info(TradeInfo.empty()).product(PRODUCT2).quantity(QUANTITY2).price(PRICE2).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		OvernightFutureTrade test = OvernightFutureTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(QUANTITY).price(PRICE).build();
		assertSerialization(test);
	  }

	}

}