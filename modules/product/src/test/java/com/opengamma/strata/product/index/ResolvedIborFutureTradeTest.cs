/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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


	/// <summary>
	/// Test <seealso cref="ResolvedIborFutureTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedIborFutureTradeTest
	public class ResolvedIborFutureTradeTest
	{

	  private static readonly ResolvedIborFuture PRODUCT = ResolvedIborFutureTest.sut();
	  private static readonly ResolvedIborFuture PRODUCT2 = ResolvedIborFutureTest.sut2();
	  private static readonly LocalDate TRADE_DATE = date(2014, 6, 30);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(TRADE_DATE);
	  private static readonly TradeInfo TRADE_INFO2 = TradeInfo.of(date(2014, 7, 1));
	  private const double QUANTITY = 100;
	  private const double QUANTITY2 = 200;
	  private const double PRICE = 0.99;
	  private const double PRICE2 = 0.98;

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		ResolvedIborFutureTrade test = sut();
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Quantity, QUANTITY);
		assertEquals(test.TradedPrice, TradedPrice.of(TRADE_DATE, PRICE));
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
	  internal static ResolvedIborFutureTrade sut()
	  {
		return ResolvedIborFutureTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(QUANTITY).tradedPrice(TradedPrice.of(TRADE_DATE, PRICE)).build();
	  }

	  internal static ResolvedIborFutureTrade sut2()
	  {
		return ResolvedIborFutureTrade.builder().info(TRADE_INFO2).product(PRODUCT2).quantity(QUANTITY2).tradedPrice(TradedPrice.of(TRADE_DATE, PRICE2)).build();
	  }

	}

}