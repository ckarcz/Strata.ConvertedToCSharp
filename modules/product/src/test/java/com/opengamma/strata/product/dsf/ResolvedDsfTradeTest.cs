/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;


	/// <summary>
	/// Test <seealso cref="ResolvedDsfTrade"/>. 
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedDsfTradeTest
	public class ResolvedDsfTradeTest
	{

	  private static readonly ResolvedDsf PRODUCT = ResolvedDsfTest.sut();
	  private static readonly ResolvedDsf PRODUCT2 = ResolvedDsfTest.sut2();
	  private const double QUANTITY = 100;
	  private const double QUANTITY2 = 200;
	  private const double PRICE = 0.99;
	  private const double PRICE2 = 0.98;
	  private static readonly LocalDate TRADE_DATE = date(2014, 6, 30);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(TRADE_DATE);

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		ResolvedDsfTrade test = sut();
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
	  internal static ResolvedDsfTrade sut()
	  {
		return ResolvedDsfTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(QUANTITY).tradedPrice(TradedPrice.of(TRADE_DATE, PRICE)).build();
	  }

	  internal static ResolvedDsfTrade sut2()
	  {
		return ResolvedDsfTrade.builder().product(PRODUCT2).quantity(QUANTITY2).tradedPrice(TradedPrice.of(TRADE_DATE, PRICE2)).build();
	  }

	}

}