using System;
using System.Threading;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.tuple
{
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
//ORIGINAL LINE: @Test public class ObjDoublePairTest
	public class ObjDoublePairTest
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
			new object[] {"A", 2.5d},
			new object[] {"B", 200.2d},
			new object[] {"C", -2.5d},
			new object[] {"D", 0d}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_of_getters(String first, double second)
	  public virtual void test_of_getters(string first, double second)
	  {
		ObjDoublePair<string> test = ObjDoublePair.of(first, second);
		assertEquals(test.First, first);
		assertEquals(test.Second, second, TOLERANCE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_ofPair(String first, double second)
	  public virtual void test_ofPair(string first, double second)
	  {
		Pair<string, double> pair = Pair.of(first, second);
		ObjDoublePair<string> test = ObjDoublePair.ofPair(pair);
		assertEquals(test.First, first);
		assertEquals(test.Second, second, TOLERANCE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_sizeElements(String first, double second)
	  public virtual void test_sizeElements(string first, double second)
	  {
		ObjDoublePair<string> test = ObjDoublePair.of(first, second);
		assertEquals(test.size(), 2);
		assertEquals(test.elements(), ImmutableList.of(first, second));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_toString(String first, double second)
	  public virtual void test_toString(string first, double second)
	  {
		ObjDoublePair<string> test = ObjDoublePair.of(first, second);
		string str = "[" + first + ", " + second + "]";
		assertEquals(test.ToString(), str);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_toPair(String first, double second)
	  public virtual void test_toPair(string first, double second)
	  {
		ObjDoublePair<string> test = ObjDoublePair.of(first, second);
		assertEquals(test.toPair(), Pair.of(first, second));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_compareTo()
	  {
		ObjDoublePair<string> p12 = ObjDoublePair.of("1", 2d);
		ObjDoublePair<string> p13 = ObjDoublePair.of("1", 3d);
		ObjDoublePair<string> p21 = ObjDoublePair.of("2", 1d);

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

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = ClassCastException.class) public void test_compareTo_notComparable()
	  public virtual void test_compareTo_notComparable()
	  {
		ThreadStart notComparable = () =>
		{
		};
		ObjDoublePair<ThreadStart> test1 = ObjDoublePair.of(notComparable, 2d);
		ObjDoublePair<ThreadStart> test2 = ObjDoublePair.of(notComparable, 2d);
		test1.CompareTo(test2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equals()
	  {
		ObjDoublePair<string> a = ObjDoublePair.of("1", 2.0d);
		ObjDoublePair<string> a2 = ObjDoublePair.of("1", 2.0d);
		ObjDoublePair<string> b = ObjDoublePair.of("1", 3.0d);
		ObjDoublePair<string> c = ObjDoublePair.of("2", 2.0d);
		ObjDoublePair<string> d = ObjDoublePair.of("2", 3.0d);

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
		ObjDoublePair<string> a = ObjDoublePair.of("1", 1.7d);
		assertEquals(a.Equals(null), false);
		assertEquals(a.Equals(ANOTHER_TYPE), false);
		object unrelatedType = Pair.of(Convert.ToInt32(1), Convert.ToDouble(1.7d));
		assertEquals(a.Equals(unrelatedType), false);
	  }

	  public virtual void test_hashCode()
	  {
		ObjDoublePair<string> a1 = ObjDoublePair.of("1", 1.7d);
		ObjDoublePair<string> a2 = ObjDoublePair.of("1", 1.7d);
		assertEquals(a1.GetHashCode(), a2.GetHashCode());
	  }

	  public virtual void coverage()
	  {
		ObjDoublePair<string> test = ObjDoublePair.of("1", 1.7d);
		TestHelper.coverImmutableBean(test);
	  }

	}

}