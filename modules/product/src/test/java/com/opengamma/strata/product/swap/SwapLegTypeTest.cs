/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverEnum;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SwapLegTypeTest
	public class SwapLegTypeTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
		public static object[][] data_name()
		{
		return new object[][]
		{
			new object[] {SwapLegType.FIXED, "Fixed"},
			new object[] {SwapLegType.IBOR, "Ibor"},
			new object[] {SwapLegType.OVERNIGHT, "Overnight"},
			new object[] {SwapLegType.OTHER, "Other"}
		};
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(SwapLegType convention, String name)
	  public virtual void test_toString(SwapLegType convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(SwapLegType convention, String name)
	  public virtual void test_of_lookup(SwapLegType convention, string name)
	  {
		assertEquals(SwapLegType.of(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrows(() => SwapLegType.of("Rubbish"), typeof(System.ArgumentException));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrows(() => SwapLegType.of(null), typeof(System.ArgumentException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_isFixed()
	  {
		assertEquals(SwapLegType.FIXED.Fixed, true);
		assertEquals(SwapLegType.IBOR.Fixed, false);
		assertEquals(SwapLegType.OVERNIGHT.Fixed, false);
		assertEquals(SwapLegType.INFLATION.Fixed, false);
		assertEquals(SwapLegType.OTHER.Fixed, false);
	  }

	  public virtual void test_isFloat()
	  {
		assertEquals(SwapLegType.FIXED.Float, false);
		assertEquals(SwapLegType.IBOR.Float, true);
		assertEquals(SwapLegType.OVERNIGHT.Float, true);
		assertEquals(SwapLegType.INFLATION.Float, true);
		assertEquals(SwapLegType.OTHER.Float, false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverEnum(typeof(SwapLegType));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(SwapLegType.FIXED);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(SwapLegType), SwapLegType.IBOR);
	  }

	}

}