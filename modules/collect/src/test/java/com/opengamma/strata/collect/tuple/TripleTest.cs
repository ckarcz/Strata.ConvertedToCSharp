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
//ORIGINAL LINE: @Test public class TripleTest
	public class TripleTest
	{

	  private const object ANOTHER_TYPE = "";

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "factory") public static Object[][] data_factory()
	  public static object[][] data_factory()
	  {
		return new object[][]
		{
			new object[] {"A", "B", "C"},
			new object[] {"A", 200.2d, 6L}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_of_getters(Object first, Object second, Object third)
	  public virtual void test_of_getters(object first, object second, object third)
	  {
		Triple<object, object, object> test = Triple.of(first, second, third);
		assertEquals(test.First, first);
		assertEquals(test.Second, second);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_sizeElements(Object first, Object second, Object third)
	  public virtual void test_sizeElements(object first, object second, object third)
	  {
		Triple<object, object, object> test = Triple.of(first, second, third);
		assertEquals(test.size(), 3);
		assertEquals(test.elements(), ImmutableList.of(first, second, third));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factory") public void test_toString(Object first, Object second, Object third)
	  public virtual void test_toString(object first, object second, object third)
	  {
		Triple<object, object, object> test = Triple.of(first, second, third);
		string str = "[" + first + ", " + second + ", " + third + "]";
		assertEquals(test.ToString(), str);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "factoryNull") public static Object[][] data_factoryNull()
	  public static object[][] data_factoryNull()
	  {
		return new object[][]
		{
			new object[] {null, null, null},
			new object[] {null, "B", "C"},
			new object[] {"A", null, "C"},
			new object[] {"A", "B", null}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factoryNull", expectedExceptions = IllegalArgumentException.class) public void test_of_null(Object first, Object second, Object third)
	  public virtual void test_of_null(object first, object second, object third)
	  {
		Triple.of(first, second, third);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_compareTo()
	  {
		Triple<string, string, string> abc = Triple.of("A", "B", "C");
		Triple<string, string, string> adc = Triple.of("A", "D", "C");
		Triple<string, string, string> bac = Triple.of("B", "A", "C");
		Triple<string, string, string> bad = Triple.of("B", "A", "D");

		assertTrue(abc.CompareTo(abc) == 0);
		assertTrue(abc.CompareTo(adc) < 0);
		assertTrue(abc.CompareTo(bac) < 0);

		assertTrue(adc.CompareTo(abc) > 0);
		assertTrue(adc.CompareTo(adc) == 0);
		assertTrue(adc.CompareTo(bac) < 0);

		assertTrue(bac.CompareTo(abc) > 0);
		assertTrue(bac.CompareTo(adc) > 0);
		assertTrue(bac.CompareTo(bac) == 0);

		assertTrue(bad.CompareTo(abc) > 0);
		assertTrue(bad.CompareTo(adc) > 0);
		assertTrue(bad.CompareTo(bac) > 0);
		assertTrue(bad.CompareTo(bad) == 0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = ClassCastException.class) public void test_compareTo_notComparable()
	  public virtual void test_compareTo_notComparable()
	  {
		ThreadStart notComparable = () =>
		{
		};
		Triple<int, ThreadStart, string> test1 = Triple.of(1, notComparable, "A");
		Triple<int, ThreadStart, string> test2 = Triple.of(2, notComparable, "B");
		test1.CompareTo(test2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equals()
	  {
		Triple<int, string, string> a = Triple.of(1, "Hello", "Triple");
		Triple<int, string, string> a2 = Triple.of(1, "Hello", "Triple");
		Triple<int, string, string> b = Triple.of(1, "Goodbye", "Triple");
		Triple<int, string, string> c = Triple.of(2, "Hello", "Triple");
		Triple<int, string, string> d = Triple.of(2, "Goodbye", "Triple");
		Triple<int, string, string> e = Triple.of(2, "Goodbye", "Other");

		assertEquals(a.Equals(a), true);
		assertEquals(a.Equals(b), false);
		assertEquals(a.Equals(c), false);
		assertEquals(a.Equals(d), false);
		assertEquals(a.Equals(e), false);
		assertEquals(a.Equals(a2), true);

		assertEquals(b.Equals(a), false);
		assertEquals(b.Equals(b), true);
		assertEquals(b.Equals(c), false);
		assertEquals(b.Equals(d), false);
		assertEquals(b.Equals(e), false);

		assertEquals(c.Equals(a), false);
		assertEquals(c.Equals(b), false);
		assertEquals(c.Equals(c), true);
		assertEquals(c.Equals(d), false);
		assertEquals(c.Equals(e), false);

		assertEquals(d.Equals(a), false);
		assertEquals(d.Equals(b), false);
		assertEquals(d.Equals(c), false);
		assertEquals(d.Equals(d), true);
		assertEquals(d.Equals(e), false);

		assertEquals(e.Equals(a), false);
		assertEquals(e.Equals(b), false);
		assertEquals(e.Equals(c), false);
		assertEquals(e.Equals(d), false);
		assertEquals(e.Equals(e), true);
	  }

	  public virtual void test_equals_bad()
	  {
		Triple<int, string, string> a = Triple.of(1, "Hello", "Triple");
		assertEquals(a.Equals(null), false);
		assertEquals(a.Equals(ANOTHER_TYPE), false);
	  }

	  public virtual void test_hashCode()
	  {
		Triple<int, string, string> a = Triple.of(1, "Hello", "Triple");
		assertEquals(a.GetHashCode(), a.GetHashCode());
	  }

	  public virtual void test_toString()
	  {
		Triple<string, string, string> test = Triple.of("A", "B", "C");
		assertEquals("[A, B, C]", test.ToString());
	  }

	  public virtual void coverage()
	  {
		Triple<string, string, string> test = Triple.of("A", "B", "C");
		TestHelper.coverImmutableBean(test);
	  }

	}

}