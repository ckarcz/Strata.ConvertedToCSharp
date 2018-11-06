/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swaption
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using SettlementType = com.opengamma.strata.product.common.SettlementType;

	/// <summary>
	/// Test <seealso cref="PhysicalSwaptionSettlement"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PhysicalSwaptionSettlementTest
	public class PhysicalSwaptionSettlementTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_DEFAULT()
	  {
		PhysicalSwaptionSettlement test = PhysicalSwaptionSettlement.DEFAULT;
		assertEquals(test.SettlementType, SettlementType.PHYSICAL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		PhysicalSwaptionSettlement test = PhysicalSwaptionSettlement.DEFAULT;
		coverImmutableBean(test);
	  }

	  public virtual void test_serialization()
	  {
		PhysicalSwaptionSettlement test = PhysicalSwaptionSettlement.DEFAULT;
		assertSerialization(test);
	  }

	}

}