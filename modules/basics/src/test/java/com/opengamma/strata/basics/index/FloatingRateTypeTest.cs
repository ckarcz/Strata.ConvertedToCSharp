/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverEnum;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="FloatingRateType"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FloatingRateTypeTest
	public class FloatingRateTypeTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_isIbor()
	  {
		assertEquals(FloatingRateType.IBOR.Ibor, true);
		assertEquals(FloatingRateType.OVERNIGHT_AVERAGED.Ibor, false);
		assertEquals(FloatingRateType.OVERNIGHT_COMPOUNDED.Ibor, false);
		assertEquals(FloatingRateType.PRICE.Ibor, false);
		assertEquals(FloatingRateType.OTHER.Ibor, false);
	  }

	  public virtual void test_isOvernight()
	  {
		assertEquals(FloatingRateType.IBOR.Overnight, false);
		assertEquals(FloatingRateType.OVERNIGHT_AVERAGED.Overnight, true);
		assertEquals(FloatingRateType.OVERNIGHT_COMPOUNDED.Overnight, true);
		assertEquals(FloatingRateType.PRICE.Overnight, false);
		assertEquals(FloatingRateType.OTHER.Overnight, false);
	  }

	  public virtual void test_isPrice()
	  {
		assertEquals(FloatingRateType.IBOR.Price, false);
		assertEquals(FloatingRateType.OVERNIGHT_AVERAGED.Price, false);
		assertEquals(FloatingRateType.OVERNIGHT_COMPOUNDED.Price, false);
		assertEquals(FloatingRateType.PRICE.Price, true);
		assertEquals(FloatingRateType.OTHER.Price, false);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {FloatingRateType.IBOR, "Ibor"},
			new object[] {FloatingRateType.OVERNIGHT_AVERAGED, "OvernightAveraged"},
			new object[] {FloatingRateType.OVERNIGHT_COMPOUNDED, "OvernightCompounded"},
			new object[] {FloatingRateType.PRICE, "Price"},
			new object[] {FloatingRateType.OTHER, "Other"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(FloatingRateType convention, String name)
	  public virtual void test_toString(FloatingRateType convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(FloatingRateType convention, String name)
	  public virtual void test_of_lookup(FloatingRateType convention, string name)
	  {
		assertEquals(FloatingRateType.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupUpperCase(FloatingRateType convention, String name)
	  public virtual void test_of_lookupUpperCase(FloatingRateType convention, string name)
	  {
		assertEquals(FloatingRateType.of(name.ToUpper(Locale.ENGLISH)), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupLowerCase(FloatingRateType convention, String name)
	  public virtual void test_of_lookupLowerCase(FloatingRateType convention, string name)
	  {
		assertEquals(FloatingRateType.of(name.ToLower(Locale.ENGLISH)), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => FloatingRateType.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => FloatingRateType.of(null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverEnum(typeof(FloatingRateType));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(FloatingRateType.IBOR);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(FloatingRateType), FloatingRateType.IBOR);
	  }

	}

}