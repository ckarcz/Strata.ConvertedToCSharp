/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;

	/// <summary>
	/// Test <seealso cref="ResolvedCapitalIndexedBondTrade"/>. 
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedCapitalIndexedBondTradeTest
	public class ResolvedCapitalIndexedBondTradeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		ResolvedCapitalIndexedBondTrade test = sut();
		assertEquals(test.Settlement.Present, true);
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
	  internal static ResolvedCapitalIndexedBondTrade sut()
	  {
		return CapitalIndexedBondTradeTest.sut().resolve(REF_DATA);
	  }

	  internal static ResolvedCapitalIndexedBondTrade sut2()
	  {
		return CapitalIndexedBondTradeTest.sut2().resolve(REF_DATA);
	  }

	}

}