/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="MarketDataName"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class MarketDataNameTest
	public class MarketDataNameTest
	{

	  private const object ANOTHER_TYPE = "";

	  public virtual void test_of()
	  {
		TestingName test = new TestingName("Foo");
		assertEquals(test.Name, "Foo");
		assertEquals(test.MarketDataType, typeof(string));
		assertEquals(test.ToString(), "Foo");
	  }

	  public virtual void test_comparison()
	  {
		TestingName test = new TestingName("Foo");
		assertEquals(test.Equals(test), true);
		assertEquals(test.GetHashCode(), test.GetHashCode());
		assertEquals(test.Equals(new TestingName("Eoo")), false);
		assertEquals(test.Equals(new TestingName("Foo")), true);
		assertEquals(test.Equals(ANOTHER_TYPE), false);
		assertEquals(test.Equals(null), false);
		assertEquals(test.CompareTo(new TestingName("Eoo")) > 0, true);
		assertEquals(test.CompareTo(new TestingName("Foo")) == 0, true);
		assertEquals(test.CompareTo(new TestingName("Goo")) < 0, true);
		assertEquals(test.CompareTo(new TestingName2("Foo")) < 0, true);
	  }

	}

}