using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.currency
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertSame;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="Currency"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurrencyTest
	public class CurrencyTest
	{

	  //-----------------------------------------------------------------------
	  public virtual void test_constants()
	  {
		assertEquals(Currency.of("USD"), Currency.USD);
		assertEquals(Currency.of("EUR"), Currency.EUR);
		assertEquals(Currency.of("JPY"), Currency.JPY);
		assertEquals(Currency.of("GBP"), Currency.GBP);
		assertEquals(Currency.of("CHF"), Currency.CHF);
		assertEquals(Currency.of("AUD"), Currency.AUD);
		assertEquals(Currency.of("CAD"), Currency.CAD);
	  }

	  //-----------------------------------------------------------------------
	  public virtual void test_getAvailable()
	  {
		ISet<Currency> available = Currency.AvailableCurrencies;
		assertTrue(available.Contains(Currency.USD));
		assertTrue(available.Contains(Currency.EUR));
		assertTrue(available.Contains(Currency.JPY));
		assertTrue(available.Contains(Currency.GBP));
		assertTrue(available.Contains(Currency.CHF));
		assertTrue(available.Contains(Currency.AUD));
		assertTrue(available.Contains(Currency.CAD));
	  }

	  //-----------------------------------------------------------------------
	  public virtual void test_of_String()
	  {
		Currency test = Currency.of("SEK");
		assertEquals(test.Code, "SEK");
		assertSame(test, Currency.of("SEK"));
	  }

	  public virtual void test_of_String_historicCurrency()
	  {
		Currency test = Currency.of("BEF");
		assertEquals(test.Code, "BEF");
		assertEquals(test.MinorUnitDigits, 2);
		assertEquals(test.TriangulationCurrency, Currency.EUR);
		assertSame(test, Currency.of("BEF"));
	  }

	  public virtual void test_of_String_unknownCurrencyCreated()
	  {
		Currency test = Currency.of("AAA");
		assertEquals(test.Code, "AAA");
		assertEquals(test.MinorUnitDigits, 0);
		assertSame(test, Currency.of("AAA"));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_of_String_lowerCase()
	  public virtual void test_of_String_lowerCase()
	  {
		Currency.of("gbp");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "ofBad") public static Object[][] data_ofBad()
	  public static object[][] data_ofBad()
	  {
		return new object[][]
		{
			new object[] {""},
			new object[] {"AB"},
			new object[] {"gbp"},
			new object[] {"ABCD"},
			new object[] {"123"},
			new object[] {" GBP"},
			new object[] {null}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "ofBad", expectedExceptions = IllegalArgumentException.class) public void test_of_String_bad(String input)
	  public virtual void test_of_String_bad(string input)
	  {
		Currency.of(input);
	  }

	  //-----------------------------------------------------------------------
	  public virtual void test_parse_String()
	  {
		Currency test = Currency.parse("GBP");
		assertEquals(test.Code, "GBP");
		assertSame(test, Currency.GBP);
	  }

	  public virtual void test_parse_String_unknownCurrencyCreated()
	  {
		Currency test = Currency.parse("zyx");
		assertEquals(test.Code, "ZYX");
		assertEquals(test.MinorUnitDigits, 0);
		assertSame(test, Currency.of("ZYX"));
	  }

	  public virtual void test_parse_String_lowerCase()
	  {
		Currency test = Currency.parse("gbp");
		assertEquals(test.Code, "GBP");
		assertSame(test, Currency.GBP);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "parseBad") public static Object[][] data_parseBad()
	  public static object[][] data_parseBad()
	  {
		return new object[][]
		{
			new object[] {""},
			new object[] {"AB"},
			new object[] {"ABCD"},
			new object[] {"123"},
			new object[] {" GBP"},
			new object[] {null}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "parseBad", expectedExceptions = IllegalArgumentException.class) public void test_parse_String_bad(String input)
	  public virtual void test_parse_String_bad(string input)
	  {
		Currency.parse(input);
	  }

	  //-----------------------------------------------------------------------
	  public virtual void test_minorUnits()
	  {
		assertEquals(Currency.of("USD").MinorUnitDigits, 2);
		assertEquals(Currency.of("EUR").MinorUnitDigits, 2);
		assertEquals(Currency.of("JPY").MinorUnitDigits, 0);
		assertEquals(Currency.of("GBP").MinorUnitDigits, 2);
		assertEquals(Currency.of("CHF").MinorUnitDigits, 2);
		assertEquals(Currency.of("AUD").MinorUnitDigits, 2);
		assertEquals(Currency.of("CAD").MinorUnitDigits, 2);
	  }

	  public virtual void test_triangulatonCurrency()
	  {
		assertEquals(Currency.of("USD").TriangulationCurrency, Currency.USD);
		assertEquals(Currency.of("EUR").TriangulationCurrency, Currency.USD);
		assertEquals(Currency.of("JPY").TriangulationCurrency, Currency.USD);
		assertEquals(Currency.of("GBP").TriangulationCurrency, Currency.USD);
		assertEquals(Currency.of("CHF").TriangulationCurrency, Currency.USD);
		assertEquals(Currency.of("AUD").TriangulationCurrency, Currency.USD);
		assertEquals(Currency.of("CAD").TriangulationCurrency, Currency.USD);
	  }

	  //-----------------------------------------------------------------------
	  public virtual void test_roundMinorUnits_double()
	  {
		assertEquals(Currency.USD.roundMinorUnits(63.347d), 63.35d, 0d);
		assertEquals(Currency.USD.roundMinorUnits(63.34500001d), 63.35d, 0d);
		assertEquals(Currency.USD.roundMinorUnits(63.34499999d), 63.34d, 0d);
		assertEquals(Currency.JPY.roundMinorUnits(63.347d), 63d, 0d);
		assertEquals(Currency.JPY.roundMinorUnits(63.5347d), 64d, 0d);
	  }

	  public virtual void test_roundMinorUnits_BigDecimal()
	  {
		assertEquals(Currency.USD.roundMinorUnits(new decimal(63.347d)), new decimal("63.35"));
		assertEquals(Currency.USD.roundMinorUnits(new decimal(63.34500001d)), new decimal("63.35"));
		assertEquals(Currency.USD.roundMinorUnits(new decimal(63.34499999d)), new decimal("63.34"));
		assertEquals(Currency.JPY.roundMinorUnits(new decimal(63.347d)), new decimal("63"));
		assertEquals(Currency.JPY.roundMinorUnits(new decimal(63.5347d)), new decimal("64"));
	  }

	  //-----------------------------------------------------------------------
	  public virtual void test_compareTo()
	  {
		Currency a = Currency.EUR;
		Currency b = Currency.GBP;
		Currency c = Currency.JPY;
		assertEquals(0, a.CompareTo(a));
		assertEquals(0, b.CompareTo(b));
		assertEquals(0, c.CompareTo(c));

		assertTrue(a.CompareTo(b) < 0);
		assertTrue(b.CompareTo(a) > 0);

		assertTrue(a.CompareTo(c) < 0);
		assertTrue(c.CompareTo(a) > 0);

		assertTrue(b.CompareTo(c) < 0);
		assertTrue(c.CompareTo(b) > 0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = NullPointerException.class) public void test_compareTo_null()
	  public virtual void test_compareTo_null()
	  {
		Currency.EUR.CompareTo(null);
	  }

	  //-----------------------------------------------------------------------
	  public virtual void test_equals_hashCode()
	  {
		object a1 = Currency.GBP;
		object a2 = Currency.of("GBP");
		object b = Currency.EUR;
		assertEquals(a1.Equals(a1), true);
		assertEquals(a1.Equals(b), false);
		assertEquals(a1.Equals(a2), true);

		assertEquals(a2.Equals(a1), true);
		assertEquals(a2.Equals(a2), true);
		assertEquals(a2.Equals(b), false);

		assertEquals(b.Equals(a1), false);
		assertEquals(b.Equals(a2), false);
		assertEquals(b.Equals(b), true);

		assertEquals(a1.GetHashCode(), a2.GetHashCode());
	  }

	  public virtual void test_equals_bad()
	  {
		object a = Currency.GBP;
		assertEquals(a.Equals(null), false);
		assertEquals(a.Equals("String"), false);
		assertEquals(a.Equals(new object()), false);
	  }

	  //-----------------------------------------------------------------------
	  public virtual void test_toString()
	  {
		Currency test = Currency.GBP;
		assertEquals("GBP", test.ToString());
	  }

	  //-----------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(CurrencyDataLoader));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(Currency.GBP);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(Currency), Currency.GBP);
	  }

	}

}