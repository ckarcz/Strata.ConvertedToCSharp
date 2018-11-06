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
//ORIGINAL LINE: @Test public class IntDoublePairTest
	public class IntDoublePairTest
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
			new object[] {1, 2.5d},
			new object[] {-100, 200.2d},
			new object[] {-1, -2.5d},
			new object[] {0, 0d}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_of_getters(int first, double second)
	  public virtual void test_of_getters(int first, double second)
	  {
		IntDoublePair test = IntDoublePair.of(first, second);
		assertEquals(test.First, first);
		assertEquals(test.Second, second, TOLERANCE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_ofPair(int first, double second)
	  public virtual void test_ofPair(int first, double second)
	  {
		Pair<int, double> pair = Pair.of(first, second);
		IntDoublePair test = IntDoublePair.ofPair(pair);
		assertEquals(test.First, first);
		assertEquals(test.Second, second, TOLERANCE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_sizeElements(int first, double second)
	  public virtual void test_sizeElements(int first, double second)
	  {
		IntDoublePair test = IntDoublePair.of(first, second);
		assertEquals(test.size(), 2);
		assertEquals(test.elements(), ImmutableList.of(first, second));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_toString(int first, double second)
	  public virtual void test_toString(int first, double second)
	  {
		IntDoublePair test = IntDoublePair.of(first, second);
		string str = "[" + first + ", " + second + "]";
		assertEquals(test.ToString(), str);
		assertEquals(IntDoublePair.parse(str), test);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_toPair(int first, double second)
	  public virtual void test_toPair(int first, double second)
	  {
		IntDoublePair test = IntDoublePair.of(first, second);
		assertEquals(test.toPair(), Pair.of(first, second));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "parseGood") public static Object[][] data_parseGood()
	  public static object[][] data_parseGood()
	  {
		return new object[][]
		{
			new object[] {"[1, 2.5]", 1, 2.5d},
			new object[] {"[1,2.5]", 1, 2.5d},
			new object[] {"[ 1, 2.5 ]", 1, 2.5d},
			new object[] {"[-1, -2.5]", -1, -2.5d},
			new object[] {"[0,4]", 0, 4d},
			new object[] {"[1,201d]", 1, 201d}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "parseGood") public void test_parse_good(String text, int first, double second)
	  public virtual void test_parse_good(string text, int first, double second)
	  {
		IntDoublePair test = IntDoublePair.parse(text);
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
		IntDoublePair.parse(text);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_compareTo()
	  {
		IntDoublePair p12 = IntDoublePair.of(1, 2d);
		IntDoublePair p13 = IntDoublePair.of(1, 3d);
		IntDoublePair p21 = IntDoublePair.of(2, 1d);

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
		IntDoublePair a = IntDoublePair.of(1, 2.0d);
		IntDoublePair a2 = IntDoublePair.of(1, 2.0d);
		IntDoublePair b = IntDoublePair.of(1, 3.0d);
		IntDoublePair c = IntDoublePair.of(2, 2.0d);
		IntDoublePair d = IntDoublePair.of(2, 3.0d);

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
		IntDoublePair a = IntDoublePair.of(1, 1.7d);
		assertEquals(a.Equals(null), false);
		assertEquals(a.Equals(ANOTHER_TYPE), false);
		object unrelatdPair = Pair.of(Convert.ToInt32(1), Convert.ToDouble(1.7d));
		assertEquals(a.Equals(unrelatdPair), false);
	  }

	  public virtual void test_hashCode()
	  {
		IntDoublePair a1 = IntDoublePair.of(1, 1.7d);
		IntDoublePair a2 = IntDoublePair.of(1, 1.7d);
		assertEquals(a1.GetHashCode(), a2.GetHashCode());
	  }

	  public virtual void coverage()
	  {
		IntDoublePair test = IntDoublePair.of(1, 1.7d);
		TestHelper.coverImmutableBean(test);
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(IntDoublePair.of(1, 1.7d));
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(IntDoublePair), IntDoublePair.of(1, 1.7d));
	  }

	}

}