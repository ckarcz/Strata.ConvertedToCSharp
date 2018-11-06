using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.location
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertSame;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test Country.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CountryTest
	public class CountryTest
	{

	  private const object ANOTHER_TYPE = "";

	  //-----------------------------------------------------------------------
	  public virtual void test_constants()
	  {
		assertEquals(Country.of("EU"), Country.EU);
		assertEquals(Country.of("AT"), Country.AT);
		assertEquals(Country.of("BE"), Country.BE);
		assertEquals(Country.of("CH"), Country.CH);
		assertEquals(Country.of("CZ"), Country.CZ);
		assertEquals(Country.of("DE"), Country.DE);
		assertEquals(Country.of("DK"), Country.DK);
		assertEquals(Country.of("ES"), Country.ES);
		assertEquals(Country.of("FI"), Country.FI);
		assertEquals(Country.of("FR"), Country.FR);
		assertEquals(Country.of("GB"), Country.GB);
		assertEquals(Country.of("GR"), Country.GR);
		assertEquals(Country.of("HU"), Country.HU);
		assertEquals(Country.of("IE"), Country.IE);
		assertEquals(Country.of("IS"), Country.IS);
		assertEquals(Country.of("IT"), Country.IT);
		assertEquals(Country.of("LU"), Country.LU);
		assertEquals(Country.of("NL"), Country.NL);
		assertEquals(Country.of("NO"), Country.NO);
		assertEquals(Country.of("PL"), Country.PL);
		assertEquals(Country.of("PT"), Country.PT);
		assertEquals(Country.of("SE"), Country.SE);
		assertEquals(Country.of("SK"), Country.SK);
		assertEquals(Country.of("TR"), Country.TR);

		assertEquals(Country.of("AR"), Country.AR);
		assertEquals(Country.of("BR"), Country.BR);
		assertEquals(Country.of("CA"), Country.CA);
		assertEquals(Country.of("CL"), Country.CL);
		assertEquals(Country.of("MX"), Country.MX);
		assertEquals(Country.of("US"), Country.US);

		assertEquals(Country.of("AU"), Country.AU);
		assertEquals(Country.of("CN"), Country.CN);
		assertEquals(Country.of("EG"), Country.EG);
		assertEquals(Country.of("HK"), Country.HK);
		assertEquals(Country.of("ID"), Country.ID);
		assertEquals(Country.of("IL"), Country.IL);
		assertEquals(Country.of("IN"), Country.IN);
		assertEquals(Country.of("JP"), Country.JP);
		assertEquals(Country.of("KR"), Country.KR);
		assertEquals(Country.of("MY"), Country.MY);
		assertEquals(Country.of("NZ"), Country.NZ);
		assertEquals(Country.of("RU"), Country.RU);
		assertEquals(Country.of("SA"), Country.SA);
		assertEquals(Country.of("SG"), Country.SG);
		assertEquals(Country.of("TH"), Country.TH);
		assertEquals(Country.of("ZA"), Country.ZA);
	  }

	  //-----------------------------------------------------------------------
	  public virtual void test_getAvailable()
	  {
		ISet<Country> available = Country.AvailableCountries;
		assertTrue(available.Contains(Country.US));
		assertTrue(available.Contains(Country.EU));
		assertTrue(available.Contains(Country.JP));
		assertTrue(available.Contains(Country.GB));
		assertTrue(available.Contains(Country.CH));
		assertTrue(available.Contains(Country.AU));
		assertTrue(available.Contains(Country.CA));
	  }

	  //-----------------------------------------------------------------------
	  public virtual void test_of_String()
	  {
		Country test = Country.of("SE");
		assertEquals(test.Code, "SE");
		assertSame(test, Country.of("SE"));
	  }

	  public virtual void test_of_String_unknownCountryCreated()
	  {
		Country test = Country.of("AA");
		assertEquals(test.Code, "AA");
		assertSame(test, Country.of("AA"));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "ofBad") public static Object[][] data_ofBad()
	  public static object[][] data_ofBad()
	  {
		return new object[][]
		{
			new object[] {""},
			new object[] {"A"},
			new object[] {"gb"},
			new object[] {"ABC"},
			new object[] {"123"},
			new object[] {" GB"},
			new object[] {null}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "ofBad", expectedExceptions = IllegalArgumentException.class) public void test_of_String_bad(String input)
	  public virtual void test_of_String_bad(string input)
	  {
		Country.of(input);
	  }

	  //-----------------------------------------------------------------------
	  public virtual void test_parse_String()
	  {
		Country test = Country.parse("GB");
		assertEquals(test.Code, "GB");
		assertSame(test, Country.GB);
	  }

	  public virtual void test_parse_String_unknownCountryCreated()
	  {
		Country test = Country.parse("zy");
		assertEquals(test.Code, "ZY");
		assertSame(test, Country.of("ZY"));
	  }

	  public virtual void test_parse_String_lowerCase()
	  {
		Country test = Country.parse("gb");
		assertEquals(test.Code, "GB");
		assertSame(test, Country.GB);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "parseBad") public static Object[][] data_parseBad()
	  public static object[][] data_parseBad()
	  {
		return new object[][]
		{
			new object[] {""},
			new object[] {"A"},
			new object[] {"ABC"},
			new object[] {"123"},
			new object[] {" GB"},
			new object[] {null}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "parseBad", expectedExceptions = IllegalArgumentException.class) public void test_parse_String_bad(String input)
	  public virtual void test_parse_String_bad(string input)
	  {
		Country.parse(input);
	  }

	  //-----------------------------------------------------------------------
	  public virtual void test_compareTo()
	  {
		Country a = Country.EU;
		Country b = Country.GB;
		Country c = Country.JP;
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
		Country.EU.CompareTo(null);
	  }

	  //-----------------------------------------------------------------------
	  public virtual void test_equals_hashCode()
	  {
		Country a1 = Country.GB;
		Country a2 = Country.of("GB");
		Country b = Country.EU;
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
		Country a = Country.GB;
		assertEquals(a.Equals(null), false);
		assertEquals(a.Equals(ANOTHER_TYPE), false);
		assertEquals(a.Equals(new object()), false);
	  }

	  //-----------------------------------------------------------------------
	  public virtual void test_toString()
	  {
		Country test = Country.GB;
		assertEquals("GB", test.ToString());
	  }

	  //-----------------------------------------------------------------------
	  public virtual void test_serialization()
	  {
		assertSerialization(Country.GB);
		assertSerialization(Country.of("US"));
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(Country), Country.GB);
		assertJodaConvert(typeof(Country), Country.of("US"));
	  }

	}

}