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
//ORIGINAL LINE: @Test public class PairTest
	public class PairTest
	{

	  private const object ANOTHER_TYPE = "";

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "factory") public static Object[][] data_factory()
	  public static object[][] data_factory()
	  {
		return new object[][]
		{
			new object[] {"A", "B"},
			new object[] {"A", 200.2d}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_of_getters(Object first, Object second)
	  public virtual void test_of_getters(object first, object second)
	  {
		Pair<object, object> test = Pair.of(first, second);
		assertEquals(test.First, first);
		assertEquals(test.Second, second);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_sizeElements(Object first, Object second)
	  public virtual void test_sizeElements(object first, object second)
	  {
		Pair<object, object> test = Pair.of(first, second);
		assertEquals(test.size(), 2);
		assertEquals(test.elements(), ImmutableList.of(first, second));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_toString(Object first, Object second)
	  public virtual void test_toString(object first, object second)
	  {
		Pair<object, object> test = Pair.of(first, second);
		string str = "[" + first + ", " + second + "]";
		assertEquals(test.ToString(), str);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "factoryNull") public static Object[][] data_factoryNull()
	  public static object[][] data_factoryNull()
	  {
		return new object[][]
		{
			new object[] {null, null},
			new object[] {null, "B"},
			new object[] {"A", null}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factoryNull", expectedExceptions = IllegalArgumentException.class) public void test_of_null(Object first, Object second)
	  public virtual void test_of_null(object first, object second)
	  {
		Pair.of(first, second);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_compareTo()
	  {
		Pair<string, string> ab = Pair.of("A", "B");
		Pair<string, string> ad = Pair.of("A", "D");
		Pair<string, string> ba = Pair.of("B", "A");

		assertTrue(ab.CompareTo(ab) == 0);
		assertTrue(ab.CompareTo(ad) < 0);
		assertTrue(ab.CompareTo(ba) < 0);

		assertTrue(ad.CompareTo(ab) > 0);
		assertTrue(ad.CompareTo(ad) == 0);
		assertTrue(ad.CompareTo(ba) < 0);

		assertTrue(ba.CompareTo(ab) > 0);
		assertTrue(ba.CompareTo(ad) > 0);
		assertTrue(ba.CompareTo(ba) == 0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = ClassCastException.class) public void test_compareTo_notComparable()
	  public virtual void test_compareTo_notComparable()
	  {
		ThreadStart notComparable = () =>
		{
		};
		Pair<ThreadStart, string> test1 = Pair.of(notComparable, "A");
		Pair<ThreadStart, string> test2 = Pair.of(notComparable, "B");
		test1.CompareTo(test2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equals()
	  {
		Pair<int, string> a = Pair.of(1, "Hello");
		Pair<int, string> a2 = Pair.of(1, "Hello");
		Pair<int, string> b = Pair.of(1, "Goodbye");
		Pair<int, string> c = Pair.of(2, "Hello");
		Pair<int, string> d = Pair.of(2, "Goodbye");

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
		Pair<int, string> a = Pair.of(1, "Hello");
		assertEquals(a.Equals(null), false);
		assertEquals(a.Equals(ANOTHER_TYPE), false);
	  }

	  public virtual void test_hashCode()
	  {
		Pair<int, string> a = Pair.of(1, "Hello");
		assertEquals(a.GetHashCode(), a.GetHashCode());
	  }

	  public virtual void test_toString()
	  {
		Pair<string, string> test = Pair.of("A", "B");
		assertEquals("[A, B]", test.ToString());
	  }

	  public virtual void coverage()
	  {
		Pair<string, string> test = Pair.of("A", "B");
		TestHelper.coverImmutableBean(test);
	  }

	}

}