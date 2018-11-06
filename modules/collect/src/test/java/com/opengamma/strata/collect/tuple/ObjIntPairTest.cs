using System;
using System.Threading;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
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
//ORIGINAL LINE: @Test public class ObjIntPairTest
	public class ObjIntPairTest
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
			new object[] {"A", 2},
			new object[] {"B", 200},
			new object[] {"C", -2},
			new object[] {"D", 0}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_of_getters(String first, int second)
	  public virtual void test_of_getters(string first, int second)
	  {
		ObjIntPair<string> test = ObjIntPair.of(first, second);
		assertEquals(test.First, first);
		assertEquals(test.Second, second, TOLERANCE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_ofPair(String first, int second)
	  public virtual void test_ofPair(string first, int second)
	  {
		Pair<string, int> pair = Pair.of(first, second);
		ObjIntPair<string> test = ObjIntPair.ofPair(pair);
		assertEquals(test.First, first);
		assertEquals(test.Second, second, TOLERANCE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_sizeElements(String first, int second)
	  public virtual void test_sizeElements(string first, int second)
	  {
		ObjIntPair<string> test = ObjIntPair.of(first, second);
		assertEquals(test.size(), 2);
		assertEquals(test.elements(), ImmutableList.of(first, second));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_toString(String first, int second)
	  public virtual void test_toString(string first, int second)
	  {
		ObjIntPair<string> test = ObjIntPair.of(first, second);
		string str = "[" + first + ", " + second + "]";
		assertEquals(test.ToString(), str);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_toPair(String first, int second)
	  public virtual void test_toPair(string first, int second)
	  {
		ObjIntPair<string> test = ObjIntPair.of(first, second);
		assertEquals(test.toPair(), Pair.of(first, second));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_compareTo()
	  {
		ObjIntPair<string> p12 = ObjIntPair.of("1", 2);
		ObjIntPair<string> p13 = ObjIntPair.of("1", 3);
		ObjIntPair<string> p21 = ObjIntPair.of("2", 1);

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
		ObjIntPair<ThreadStart> test1 = ObjIntPair.of(notComparable, 2);
		ObjIntPair<ThreadStart> test2 = ObjIntPair.of(notComparable, 2);
		test1.CompareTo(test2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equals()
	  {
		ObjIntPair<string> a = ObjIntPair.of("1", 2);
		ObjIntPair<string> a2 = ObjIntPair.of("1", 2);
		ObjIntPair<string> b = ObjIntPair.of("1", 3);
		ObjIntPair<string> c = ObjIntPair.of("2", 2);
		ObjIntPair<string> d = ObjIntPair.of("2", 3);

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
		ObjIntPair<string> a = ObjIntPair.of("1", 1);
		assertEquals(a.Equals(null), false);
		assertEquals(a.Equals(ANOTHER_TYPE), false);
		object unrelatedType = Pair.of(Convert.ToInt32(1), Convert.ToInt32(1));
		assertEquals(a.Equals(unrelatedType), false);
	  }

	  public virtual void test_hashCode()
	  {
		ObjIntPair<string> a1 = ObjIntPair.of("1", 1);
		ObjIntPair<string> a2 = ObjIntPair.of("1", 1);
		assertEquals(a1.GetHashCode(), a2.GetHashCode());
	  }

	  public virtual void coverage()
	  {
		ObjIntPair<string> test = ObjIntPair.of("1", 1);
		TestHelper.coverImmutableBean(test);
	  }

	}

}