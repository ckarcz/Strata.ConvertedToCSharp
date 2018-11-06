/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.capfloor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
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

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;

	/// <summary>
	/// Test <seealso cref="ResolvedIborCapFloorTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedIborCapFloorTradeTest
	public class ResolvedIborCapFloorTradeTest
	{

	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2016, 6, 30));
	  private static readonly ResolvedIborCapFloor PRODUCT = ResolvedIborCapFloor.of(ResolvedIborCapFloorTest.CAPFLOOR_LEG, ResolvedIborCapFloorTest.PAY_LEG);
	  private static readonly Payment PREMIUM = Payment.of(CurrencyAmount.of(EUR, -0.001 * 1.0e6), date(2016, 7, 2));

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		ResolvedIborCapFloorTrade test = ResolvedIborCapFloorTrade.builder().product(PRODUCT).build();
		assertEquals(test.Info, TradeInfo.empty());
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Premium, null);
	  }

	  public virtual void test_builder_full()
	  {
		ResolvedIborCapFloorTrade test = ResolvedIborCapFloorTrade.builder().info(TRADE_INFO).product(PRODUCT).premium(PREMIUM).build();
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Premium, PREMIUM);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ResolvedIborCapFloorTrade test = ResolvedIborCapFloorTrade.builder().info(TRADE_INFO).product(PRODUCT).premium(PREMIUM).build();
		coverImmutableBean(test);
		ResolvedIborCapFloorTrade test2 = ResolvedIborCapFloorTrade.builder().product(PRODUCT).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		ResolvedIborCapFloorTrade test = ResolvedIborCapFloorTrade.builder().product(PRODUCT).build();
		assertSerialization(test);
	  }

	}

}