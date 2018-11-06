/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fra
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
	/// Test <seealso cref="ResolvedFraTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedFraTradeTest
	public class ResolvedFraTradeTest
	{

	  private static readonly ResolvedFra PRODUCT = ResolvedFraTest.sut();
	  private static readonly ResolvedFra PRODUCT2 = ResolvedFraTest.sut2();
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2014, 6, 30));

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		ResolvedFraTrade test = ResolvedFraTrade.of(TRADE_INFO, PRODUCT);
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Info, TRADE_INFO);
	  }

	  public virtual void test_builder()
	  {
		ResolvedFraTrade test = ResolvedFraTrade.builder().product(PRODUCT).build();
		assertEquals(test.Info, TradeInfo.empty());
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
	  internal static ResolvedFraTrade sut()
	  {
		return ResolvedFraTrade.builder().info(TRADE_INFO).product(PRODUCT).build();
	  }

	  internal static ResolvedFraTrade sut2()
	  {
		return ResolvedFraTrade.builder().product(PRODUCT2).build();
	  }

	}

}