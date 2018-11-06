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
//ORIGINAL LINE: @Test public class NegativeRateMethodTest
	public class NegativeRateMethodTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void adjust_allowNegative()
	  {
		assertEquals(NegativeRateMethod.ALLOW_NEGATIVE.adjust(1d), 1d, 0d);
		assertEquals(NegativeRateMethod.ALLOW_NEGATIVE.adjust(0d), 0d, 0d);
		assertEquals(NegativeRateMethod.ALLOW_NEGATIVE.adjust(-0d), -0d, 0d);
		assertEquals(NegativeRateMethod.ALLOW_NEGATIVE.adjust(-1d), -1d, 0d);
		assertEquals(NegativeRateMethod.ALLOW_NEGATIVE.adjust(double.MaxValue), double.MaxValue, 0d);
		assertEquals(NegativeRateMethod.ALLOW_NEGATIVE.adjust(double.Epsilon), double.Epsilon, 0d);
		assertEquals(NegativeRateMethod.ALLOW_NEGATIVE.adjust(double.PositiveInfinity), double.PositiveInfinity, 0d);
		assertEquals(NegativeRateMethod.ALLOW_NEGATIVE.adjust(double.NegativeInfinity), double.NegativeInfinity, 0d);
		assertEquals(NegativeRateMethod.ALLOW_NEGATIVE.adjust(Double.NaN), Double.NaN); // force to Double for comparison
	  }

	  public virtual void adjust_notNegative()
	  {
		assertEquals(NegativeRateMethod.NOT_NEGATIVE.adjust(1d), 1d, 0d);
		assertEquals(NegativeRateMethod.NOT_NEGATIVE.adjust(0d), 0d, 0d);
		assertEquals(NegativeRateMethod.NOT_NEGATIVE.adjust(-0d), 0d, 0d);
		assertEquals(NegativeRateMethod.NOT_NEGATIVE.adjust(-1d), 0d, 0d);
		assertEquals(NegativeRateMethod.NOT_NEGATIVE.adjust(double.MaxValue), double.MaxValue, 0d);
		assertEquals(NegativeRateMethod.NOT_NEGATIVE.adjust(double.Epsilon), double.Epsilon, 0d);
		assertEquals(NegativeRateMethod.NOT_NEGATIVE.adjust(double.PositiveInfinity), double.PositiveInfinity, 0d);
		assertEquals(NegativeRateMethod.NOT_NEGATIVE.adjust(double.NegativeInfinity), 0d, 0d);
		assertEquals(NegativeRateMethod.NOT_NEGATIVE.adjust(Double.NaN), Double.NaN); // force to Double for comparison
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {NegativeRateMethod.ALLOW_NEGATIVE, "AllowNegative"},
			new object[] {NegativeRateMethod.NOT_NEGATIVE, "NotNegative"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(NegativeRateMethod convention, String name)
	  public virtual void test_toString(NegativeRateMethod convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(NegativeRateMethod convention, String name)
	  public virtual void test_of_lookup(NegativeRateMethod convention, string name)
	  {
		assertEquals(NegativeRateMethod.of(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrows(() => NegativeRateMethod.of("Rubbish"), typeof(System.ArgumentException));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrows(() => NegativeRateMethod.of(null), typeof(System.ArgumentException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverEnum(typeof(NegativeRateMethod));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(NegativeRateMethod.ALLOW_NEGATIVE);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(NegativeRateMethod), NegativeRateMethod.ALLOW_NEGATIVE);
	  }

	}

}