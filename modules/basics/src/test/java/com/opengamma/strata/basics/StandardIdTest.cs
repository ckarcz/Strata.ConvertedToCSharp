/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="StandardId"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class StandardIdTest
	public class StandardIdTest
	{

	  private const string SCHEME = "Scheme";
	  private const string OTHER_SCHEME = "Other";
	  private const object ANOTHER_TYPE = "";

	  //-------------------------------------------------------------------------
	  public virtual void test_factory_String_String()
	  {
		StandardId test = StandardId.of("scheme:/+foo", "value");
		assertEquals(test.Scheme, "scheme:/+foo");
		assertEquals(test.Value, "value");
		assertEquals(test.ToString(), "scheme:/+foo~value");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_factory_String_String_nullScheme()
	  public virtual void test_factory_String_String_nullScheme()
	  {
		StandardId.of(null, "value");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_factory_String_String_nullValue()
	  public virtual void test_factory_String_String_nullValue()
	  {
		StandardId.of("Scheme", null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_factory_String_String_emptyValue()
	  public virtual void test_factory_String_String_emptyValue()
	  {
		StandardId.of("Scheme", "");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "factoryValid") public static Object[][] data_factoryValid()
	  public static object[][] data_factoryValid()
	  {
		return new object[][]
		{
			new object[] {"ABCDEFGHIJKLMNOPQRSTUVWXYZ", "123"},
			new object[] {"abcdefghijklmnopqrstuvwxyz", "123"},
			new object[] {"0123456789:/+.=_-", "123"},
			new object[] {"ABC", "! !\"$%%^&*()123abcxyzABCXYZ"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factoryValid") public void test_factory_String_String_valid(String scheme, String value)
	  public virtual void test_factory_String_String_valid(string scheme, string value)
	  {
		StandardId.of(scheme, value);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "factoryInvalid") public static Object[][] data_factoryInvalid()
	  public static object[][] data_factoryInvalid()
	  {
		return new object[][]
		{
			new object[] {"", ""},
			new object[] {" ", "123"},
			new object[] {"{", "123"},
			new object[] {"ABC", " 123"},
			new object[] {"ABC", "12}3"},
			new object[] {"ABC", "12\u00003"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "factoryInvalid", expectedExceptions = IllegalArgumentException.class) public void test_factory_String_String_invalid(String scheme, String value)
	  public virtual void test_factory_String_String_invalid(string scheme, string value)
	  {
		StandardId.of(scheme, value);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_encodeScheme()
	  {
		string test = StandardId.encodeScheme("https://opengamma.com/foo/../~bar#test");
		assertEquals(test, "https://opengamma.com/foo/../%7Ebar%23test");
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "formats") public static Object[][] data_formats()
	  public static object[][] data_formats()
	  {
		return new object[][]
		{
			new object[] {"Value", "A~Value"},
			new object[] {"a+b", "A~a+b"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "formats") public void test_formats_toString(String value, String expected)
	  public virtual void test_formats_toString(string value, string expected)
	  {
		StandardId test = StandardId.of("A", value);
		assertEquals(test.ToString(), expected);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "formats") public void test_formats_parse(String value, String text)
	  public virtual void test_formats_parse(string value, string text)
	  {
		StandardId test = StandardId.parse(text);
		assertEquals(test.Scheme, "A");
		assertEquals(test.Value, value);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parse()
	  {
		StandardId test = StandardId.parse("Scheme~value");
		assertEquals(test.Scheme, SCHEME);
		assertEquals(test.Value, "value");
		assertEquals(test.ToString(), "Scheme~value");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "parseInvalidFormat") public static Object[][] data_parseInvalidFormat()
	  public static object[][] data_parseInvalidFormat()
	  {
		return new object[][]
		{
			new object[] {"Scheme"},
			new object[] {"Scheme~"},
			new object[] {"~value"},
			new object[] {"Scheme:value"},
			new object[] {"a~b~c"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "parseInvalidFormat", expectedExceptions = IllegalArgumentException.class) public void test_parse_invalidFormat(String text)
	  public virtual void test_parse_invalidFormat(string text)
	  {
		StandardId.parse(text);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equals()
	  {
		StandardId d1a = StandardId.of(SCHEME, "d1");
		StandardId d1b = StandardId.of(SCHEME, "d1");
		StandardId d2 = StandardId.of(SCHEME, "d2");
		StandardId d3 = StandardId.of("Different", "d1");
		assertEquals((object) d1a.Equals(d1a), true);
		assertEquals((object) d1a.Equals(d1b), true);
		assertEquals((object) d1a.Equals(d2), false);
		assertEquals((object) d1b.Equals(d1a), true);
		assertEquals((object) d1b.Equals(d1b), true);
		assertEquals((object) d1b.Equals(d2), false);
		assertEquals((object) d2.Equals(d1a), false);
		assertEquals((object) d2.Equals(d1b), false);
		assertEquals((object) d2.Equals(d2), true);
		assertEquals((object) d3.Equals(d1a), false);
		assertEquals((object) d3.Equals(d2), false);
		assertEquals((object) d3.Equals(d3), true);
		assertEquals((object) d1b.Equals(ANOTHER_TYPE), false);
		assertEquals((object) d1b.Equals(null), false);
	  }

	  public virtual void test_hashCode()
	  {
		StandardId d1a = StandardId.of(SCHEME, "d1");
		StandardId d1b = StandardId.of(SCHEME, "d1");
		assertEquals((object) d1b.GetHashCode(), d1a.GetHashCode());
	  }

	  public virtual void test_comparisonByScheme()
	  {
		StandardId id1 = StandardId.of(SCHEME, "123");
		StandardId id2 = StandardId.of(OTHER_SCHEME, "234");
		// as schemes are different, will compare by scheme
		assertThat(id1).isGreaterThan(id2);
	  }

	  public virtual void test_comparisonWithSchemeSame()
	  {
		StandardId id1 = StandardId.of(SCHEME, "123");
		StandardId id2 = StandardId.of(SCHEME, "234");
		// as schemes are same, will compare by id
		assertThat(id1).isLessThan(id2);
	  }

	  public virtual void coverage()
	  {
		coverImmutableBean(StandardId.of(SCHEME, "123"));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(StandardId.of(SCHEME, "123"));
	  }

	}

}