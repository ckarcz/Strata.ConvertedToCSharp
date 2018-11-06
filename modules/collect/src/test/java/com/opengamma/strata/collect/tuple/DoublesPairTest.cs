using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.tuple
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DoublesPairTest
	public class DoublesPairTest
	{

	  private const double TOLERANCE = 0.00001d;
	  private const object ANOTHER_TYPE = "";

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "factory") public static Object[][] data_factory()
	  public static object[][] data_factory()
	  {
		return new object[][]
		{
			new object[] {1.2d, 2.5d},
			new object[] {-100.1d, 200.2d},
			new object[] {-1.2d, -2.5d},
			new object[] {0d, 0d}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_of_getters(double first, double second)
	  public virtual void test_of_getters(double first, double second)
	  {
		DoublesPair test = DoublesPair.of(first, second);
		assertEquals(test.First, first, TOLERANCE);
		assertEquals(test.Second, second, TOLERANCE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_ofPair(double first, double second)
	  public virtual void test_ofPair(double first, double second)
	  {
		Pair<double, double> pair = Pair.of(first, second);
		DoublesPair test = DoublesPair.ofPair(pair);
		assertEquals(test.First, first, TOLERANCE);
		assertEquals(test.Second, second, TOLERANCE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_sizeElements(double first, double second)
	  public virtual void test_sizeElements(double first, double second)
	  {
		DoublesPair test = DoublesPair.of(first, second);
		assertEquals(test.size(), 2);
		assertEquals(test.elements(), ImmutableList.of(first, second));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_toString(double first, double second)
	  public virtual void test_toString(double first, double second)
	  {
		DoublesPair test = DoublesPair.of(first, second);
		string str = "[" + first + ", " + second + "]";
		assertEquals(test.ToString(), str);
		assertEquals(DoublesPair.parse(str), test);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_toPair(double first, double second)
	  public virtual void test_toPair(double first, double second)
	  {
		DoublesPair test = DoublesPair.of(first, second);
		assertEquals(test.toPair(), Pair.of(first, second));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "parseGood") public static Object[][] data_parseGood()
	  public static object[][] data_parseGood()
	  {
		return new object[][]
		{
			new object[] {"[1.2, 2.5]", 1.2d, 2.5d},
			new object[] {"[1.2,2.5]", 1.2d, 2.5d},
			new object[] {"[ 1.2, 2.5 ]", 1.2d, 2.5d},
			new object[] {"[-1.2, -2.5]", -1.2d, -2.5d},
			new object[] {"[0,4]", 0d, 4d},
			new object[] {"[1d,201d]", 1d, 201d}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "parseGood") public void test_parse_good(String text, double first, double second)
	  public virtual void test_parse_good(string text, double first, double second)
	  {
		DoublesPair test = DoublesPair.parse(text);
		assertEquals(test.First, first, TOLERANCE);
		assertEquals(test.Second, second, TOLERANCE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "parseBad") public static Object[][] data_parseBad()
	  public static object[][] data_parseBad()
	  {
		return new object[][]
		{
			new object[] {null},
			new object[] {""},
			new object[] {"[]"},
			new object[] {"[10]"},
			new object[] {"[10,20"},
			new object[] {"10,20]"},
			new object[] {"[10 20]"},
			new object[] {"[10,20,30]"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "parseBad", expectedExceptions = IllegalArgumentException.class) public void test_parse_bad(String text)
	  public virtual void test_parse_bad(string text)
	  {
		DoublesPair.parse(text);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_compareTo()
	  {
		DoublesPair p12 = DoublesPair.of(1d, 2d);
		DoublesPair p13 = DoublesPair.of(1d, 3d);
		DoublesPair p21 = DoublesPair.of(2d, 1d);

		assertTrue(p12.CompareTo(p12) == 0);
		assertTrue(p12.CompareTo(p13) < 0);
		assertTrue(p12.CompareTo(p21) < 0);

		assertTrue(p13.CompareTo(p12) > 0);
		assertTrue(p13.CompareTo(p13) == 0);
		assertTrue(p13.CompareTo(p21) < 0);

		assertTrue(p21.CompareTo(p12) > 0);
		assertTrue(p21.CompareTo(p13) > 0);
		assertTrue(p21.CompareTo(p21) == 0);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equals()
	  {
		DoublesPair a = DoublesPair.of(1d, 2.0d);
		DoublesPair a2 = DoublesPair.of(1d, 2.0d);
		DoublesPair b = DoublesPair.of(1d, 3.0d);
		DoublesPair c = DoublesPair.of(2d, 2.0d);
		DoublesPair d = DoublesPair.of(2d, 3.0d);

		assertEquals(a.Equals(a), true);
		assertEquals(a.Equals(b), false);
		assertEquals(a.Equals(c), false);
		assertEquals(a.Equals(d), false);
		assertEquals(a.Equals(a2), true);

		assertEquals(b.Equals(a), false);
		assertEquals(b.Equals(b), true);
		assertEquals(b.Equals(c), false);
		assertEquals(b.Equals(d), false);

		assertEquals(c.Equals(a), false);
		assertEquals(c.Equals(b), false);
		assertEquals(c.Equals(c), true);
		assertEquals(c.Equals(d), false);

		assertEquals(d.Equals(a), false);
		assertEquals(d.Equals(b), false);
		assertEquals(d.Equals(c), false);
		assertEquals(d.Equals(d), true);
	  }

	  public virtual void test_equals_bad()
	  {
		DoublesPair a = DoublesPair.of(1.1d, 1.7d);
		assertEquals(a.Equals(null), false);
		assertEquals(a.Equals(ANOTHER_TYPE), false);
		object unrelatedPair = Pair.of(Convert.ToDouble(1.1d), Convert.ToDouble(1.7d));
		assertEquals(a.Equals(unrelatedPair), false);
	  }

	  public virtual void test_hashCode()
	  {
		DoublesPair a1 = DoublesPair.of(1d, 2.0d);
		DoublesPair a2 = DoublesPair.of(1d, 2.0d);
		assertEquals(a1.GetHashCode(), a2.GetHashCode());
	  }

	  public virtual void coverage()
	  {
		DoublesPair test = DoublesPair.of(1d, 2.0d);
		TestHelper.coverImmutableBean(test);
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(DoublesPair.of(1d, 1.7d));
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(DoublesPair), DoublesPair.of(1d, 1.7d));
	  }

	}

}