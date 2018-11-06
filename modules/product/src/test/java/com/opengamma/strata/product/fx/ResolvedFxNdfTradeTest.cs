/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fx
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
	/// Test <seealso cref="ResolvedFxNdfTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedFxNdfTradeTest
	public class ResolvedFxNdfTradeTest
	{

	  private static readonly ResolvedFxNdf PRODUCT = ResolvedFxNdfTest.sut();
	  private static readonly ResolvedFxNdf PRODUCT2 = ResolvedFxNdfTest.sut2();
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2015, 1, 15));

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		ResolvedFxNdfTrade test = ResolvedFxNdfTrade.builder().info(TRADE_INFO).product(PRODUCT).build();
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.Product, PRODUCT);
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
	  internal static ResolvedFxNdfTrade sut()
	  {
		return ResolvedFxNdfTrade.builder().info(TRADE_INFO).product(PRODUCT).build();
	  }

	  internal static ResolvedFxNdfTrade sut2()
	  {
		return ResolvedFxNdfTrade.builder().product(PRODUCT2).build();
	  }

	}

}