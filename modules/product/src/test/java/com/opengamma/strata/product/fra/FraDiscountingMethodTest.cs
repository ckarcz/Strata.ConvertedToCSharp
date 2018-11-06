/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fra
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
//ORIGINAL LINE: @Test public class FraDiscountingMethodTest
	public class FraDiscountingMethodTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
		public static object[][] data_name()
		{
		return new object[][]
		{
			new object[] {FraDiscountingMethod.NONE, "None"},
			new object[] {FraDiscountingMethod.ISDA, "ISDA"},
			new object[] {FraDiscountingMethod.AFMA, "AFMA"}
		};
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(FraDiscountingMethod convention, String name)
	  public virtual void test_toString(FraDiscountingMethod convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(FraDiscountingMethod convention, String name)
	  public virtual void test_of_lookup(FraDiscountingMethod convention, string name)
	  {
		assertEquals(FraDiscountingMethod.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupUpperCase(FraDiscountingMethod convention, String name)
	  public virtual void test_of_lookupUpperCase(FraDiscountingMethod convention, string name)
	  {
		assertEquals(FraDiscountingMethod.of(name.ToUpper(Locale.ENGLISH)), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupLowerCase(FraDiscountingMethod convention, String name)
	  public virtual void test_of_lookupLowerCase(FraDiscountingMethod convention, string name)
	  {
		assertEquals(FraDiscountingMethod.of(name.ToLower(Locale.ENGLISH)), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrows(() => FraDiscountingMethod.of("Rubbish"), typeof(System.ArgumentException));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrows(() => FraDiscountingMethod.of(null), typeof(System.ArgumentException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverEnum(typeof(FraDiscountingMethod));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(FraDiscountingMethod.ISDA);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(FraDiscountingMethod), FraDiscountingMethod.ISDA);
	  }

	}

}