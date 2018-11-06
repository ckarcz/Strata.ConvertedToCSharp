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
//ORIGINAL LINE: @Test public class LongDoublePairTest
	public class LongDoublePairTest
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
			new object[] {1L, 2.5d},
			new object[] {-100L, 200.2d},
			new object[] {-1L, -2.5d},
			new object[] {0L, 0d}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_of_getters(long first, double second)
	  public virtual void test_of_getters(long first, double second)
	  {
		LongDoublePair test = LongDoublePair.of(first, second);
		assertEquals(test.First, first, TOLERANCE);
		assertEquals(test.Second, second, TOLERANCE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_ofPair(long first, double second)
	  public virtual void test_ofPair(long first, double second)
	  {
		Pair<long, double> pair = Pair.of(first, second);
		LongDoublePair test = LongDoublePair.ofPair(pair);
		assertEquals(test.First, first);
		assertEquals(test.Second, second, TOLERANCE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_sizeElements(long first, double second)
	  public virtual void test_sizeElements(long first, double second)
	  {
		LongDoublePair test = LongDoublePair.of(first, second);
		assertEquals(test.size(), 2);
		assertEquals(test.elements(), ImmutableList.of(first, second));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_toString(long first, double second)
	  public virtual void test_toString(long first, double second)
	  {
		LongDoublePair test = LongDoublePair.of(first, second);
		string str = "[" + first + ", " + second + "]";
		assertEquals(test.ToString(), str);
		assertEquals(LongDoublePair.parse(str), test);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_toPair(long first, double second)
	  public virtual void test_toPair(long first, double second)
	  {
		LongDoublePair test = LongDoublePair.of(first, second);
		assertEquals(test.toPair(), Pair.of(first, second));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "parseGood") public static Object[][] data_parseGood()
	  public static object[][] data_parseGood()
	  {
		return new object[][]
		{
			new object[] {"[1, 2.5]", 1L, 2.5d},
			new object[] {"[1,2.5]", 1L, 2.5d},
			new object[] {"[ 1, 2.5 ]", 1L, 2.5d},
			new object[] {"[-1, -2.5]", -1L, -2.5d},
			new object[] {"[0,4]", 0L, 4d},
			new object[] {"[1,201d]", 1L, 201d}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "parseGood") public void test_parse_good(String text, long first, double second)
	  public virtual void test_parse_good(string text, long first, double second)
	  {
		LongDoublePair test = LongDoublePair.parse(text);
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
		LongDoublePair.parse(text);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_compareTo()
	  {
		LongDoublePair p12 = LongDoublePair.of(1L, 2d);
		LongDoublePair p13 = LongDoublePair.of(1L, 3d);
		LongDoublePair p21 = LongDoublePair.of(2L, 1d);

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
		LongDoublePair a = LongDoublePair.of(1L, 2.0d);
		LongDoublePair a2 = LongDoublePair.of(1L, 2.0d);
		LongDoublePair b = LongDoublePair.of(1L, 3.0d);
		LongDoublePair c = LongDoublePair.of(2L, 2.0d);
		LongDoublePair d = LongDoublePair.of(2L, 3.0d);

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
		LongDoublePair a = LongDoublePair.of(1L, 1.7d);
		assertEquals(a.Equals(null), false);
		assertEquals(a.Equals(ANOTHER_TYPE), false);
		object unrelatedType = Pair.of(Convert.ToInt64(1L), Convert.ToDouble(1.7d));
		assertEquals(a.Equals(unrelatedType), false);
	  }

	  public virtual void test_hashCode()
	  {
		LongDoublePair a1 = LongDoublePair.of(1L, 1.7d);
		LongDoublePair a2 = LongDoublePair.of(1L, 1.7d);
		assertEquals(a1.GetHashCode(), a2.GetHashCode());
	  }

	  public virtual void coverage()
	  {
		LongDoublePair test = LongDoublePair.of(1L, 1.7d);
		TestHelper.coverImmutableBean(test);
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(LongDoublePair.of(1L, 1.7d));
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(LongDoublePair), LongDoublePair.of(1L, 1.7d));
	  }

	}

}