/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
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
	/// Test <seealso cref="ResolvedSwapTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedSwapTradeTest
	public class ResolvedSwapTradeTest
	{

	  private static readonly ResolvedSwap SWAP1 = ResolvedSwap.of(ResolvedSwapTest.LEG1, ResolvedSwapTest.LEG2);
	  private static readonly ResolvedSwap SWAP2 = ResolvedSwap.of(ResolvedSwapTest.LEG1);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2014, 6, 30));

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		ResolvedSwapTrade test = ResolvedSwapTrade.of(TRADE_INFO, SWAP1);
		assertEquals(test.Product, SWAP1);
		assertEquals(test.Info, TRADE_INFO);
	  }

	  public virtual void test_builder()
	  {
		ResolvedSwapTrade test = ResolvedSwapTrade.builder().product(SWAP1).build();
		assertEquals(test.Info, TradeInfo.empty());
		assertEquals(test.Product, SWAP1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ResolvedSwapTrade test = ResolvedSwapTrade.builder().info(TradeInfo.of(date(2014, 6, 30))).product(SWAP1).build();
		coverImmutableBean(test);
		ResolvedSwapTrade test2 = ResolvedSwapTrade.builder().product(SWAP2).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		ResolvedSwapTrade test = ResolvedSwapTrade.builder().info(TradeInfo.of(date(2014, 6, 30))).product(SWAP1).build();
		assertSerialization(test);
	  }

	}

}