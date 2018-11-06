using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.currency
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.AUD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.BHD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.BRL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.CAD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;

	/// <summary>
	/// Test <seealso cref="CurrencyPair"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurrencyPairTest
	public class CurrencyPairTest
	{

	  private const object ANOTHER_TYPE = "";

	  //-----------------------------------------------------------------------
	  public virtual void test_getAvailable()
	  {
		ISet<CurrencyPair> available = CurrencyPair.AvailablePairs;
		assertTrue(available.Contains(CurrencyPair.of(EUR, USD)));
		assertTrue(available.Contains(CurrencyPair.of(EUR, GBP)));
		assertTrue(available.Contains(CurrencyPair.of(GBP, USD)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_CurrencyCurrency()
	  {
		CurrencyPair test = CurrencyPair.of(GBP, USD);
		assertEquals(test.Base, GBP);
		assertEquals(test.Counter, USD);
		assertEquals(test.Identity, false);
		assertEquals(test.toSet(), ImmutableSet.of(GBP, USD));
		assertEquals(test.ToString(), "GBP/USD");
	  }

	  public virtual void test_of_CurrencyCurrency_reverseStandardOrder()
	  {
		CurrencyPair test = CurrencyPair.of(USD, GBP);
		assertEquals(test.Base, USD);
		assertEquals(test.Counter, GBP);
		assertEquals(test.Identity, false);
		assertEquals(test.toSet(), ImmutableSet.of(GBP, USD));
		assertEquals(test.ToString(), "USD/GBP");
	  }

	  public virtual void test_of_CurrencyCurrency_same()
	  {
		CurrencyPair test = CurrencyPair.of(USD, USD);
		assertEquals(test.Base, USD);
		assertEquals(test.Counter, USD);
		assertEquals(test.Identity, true);
		assertEquals(test.ToString(), "USD/USD");
	  }

	  public virtual void test_of_CurrencyCurrency_null()
	  {
		assertThrowsIllegalArg(() => CurrencyPair.of(null, USD));
		assertThrowsIllegalArg(() => CurrencyPair.of(USD, null));
		assertThrowsIllegalArg(() => CurrencyPair.of(null, null));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "parseGood") public static Object[][] data_parseGood()
	  public static object[][] data_parseGood()
	  {
		return new object[][]
		{
			new object[] {"USD/EUR", USD, EUR},
			new object[] {"EUR/USD", EUR, USD},
			new object[] {"EUR/EUR", EUR, EUR},
			new object[] {"cAd/GbP", CAD, GBP}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "parseGood") public void test_parse_String_good(String input, Currency super, Currency counter)
	  public virtual void test_parse_String_good(string input, Currency @base, Currency counter)
	  {
		assertEquals(CurrencyPair.parse(input), CurrencyPair.of(@base, counter));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "parseBad") public static Object[][] data_parseBad()
	  public static object[][] data_parseBad()
	  {
		return new object[][]
		{
			new object[] {"AUD"},
			new object[] {"AUD/GB"},
			new object[] {"AUD GBP"},
			new object[] {"AUD:GBP"},
			new object[] {"123/456"},
			new object[] {""},
			new object[] {null}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "parseBad", expectedExceptions = IllegalArgumentException.class) public void test_parse_String_bad(String input)
	  public virtual void test_parse_String_bad(string input)
	  {
		CurrencyPair.parse(input);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_inverse()
	  {
		CurrencyPair test = CurrencyPair.of(GBP, USD);
		assertEquals(test.inverse(), CurrencyPair.of(USD, GBP));
	  }

	  public virtual void test_inverse_same()
	  {
		CurrencyPair test = CurrencyPair.of(GBP, GBP);
		assertEquals(test.inverse(), CurrencyPair.of(GBP, GBP));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_contains_Currency()
	  {
		CurrencyPair test = CurrencyPair.of(GBP, USD);
		assertEquals(test.contains(GBP), true);
		assertEquals(test.contains(USD), true);
		assertEquals(test.contains(EUR), false);
	  }

	  public virtual void test_contains_Currency_same()
	  {
		CurrencyPair test = CurrencyPair.of(GBP, GBP);
		assertEquals(test.contains(GBP), true);
		assertEquals(test.contains(USD), false);
		assertEquals(test.contains(EUR), false);
	  }

	  public virtual void test_contains_Currency_null()
	  {
		CurrencyPair test = CurrencyPair.of(GBP, USD);
		assertThrowsIllegalArg(() => test.contains(null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_other_Currency()
	  {
		CurrencyPair test = CurrencyPair.of(GBP, USD);
		assertEquals(test.other(GBP), USD);
		assertEquals(test.other(USD), GBP);
		assertThrows(typeof(System.ArgumentException), () => test.other(EUR));
	  }

	  public virtual void test_other_Currency_same()
	  {
		CurrencyPair test = CurrencyPair.of(GBP, GBP);
		assertEquals(test.other(GBP), GBP);
		assertThrows(typeof(System.ArgumentException), () => test.other(EUR));
	  }

	  public virtual void test_other_Currency_null()
	  {
		CurrencyPair test = CurrencyPair.of(GBP, USD);
		assertThrowsIllegalArg(() => test.other(null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_isInverse_CurrencyPair()
	  {
		CurrencyPair test = CurrencyPair.of(GBP, USD);
		assertEquals(test.isInverse(test), false);
		assertEquals(test.isInverse(CurrencyPair.of(GBP, USD)), false);
		assertEquals(test.isInverse(CurrencyPair.of(USD, GBP)), true);
		assertEquals(test.isInverse(CurrencyPair.of(GBP, EUR)), false);
		assertEquals(test.isInverse(CurrencyPair.of(EUR, GBP)), false);
		assertEquals(test.isInverse(CurrencyPair.of(USD, EUR)), false);
		assertEquals(test.isInverse(CurrencyPair.of(EUR, USD)), false);
	  }

	  public virtual void test_isInverse_CurrencyPair_null()
	  {
		CurrencyPair test = CurrencyPair.of(GBP, USD);
		assertThrowsIllegalArg(() => test.isInverse(null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_cross_CurrencyPair()
	  {
		CurrencyPair gbpGbp = CurrencyPair.of(GBP, GBP);
		CurrencyPair gbpUsd = CurrencyPair.of(GBP, USD);
		CurrencyPair usdGbp = CurrencyPair.of(USD, GBP);
		CurrencyPair eurGbp = CurrencyPair.of(EUR, GBP);
		CurrencyPair eurUsd = CurrencyPair.of(EUR, USD);
		CurrencyPair usdEur = CurrencyPair.of(USD, EUR);

		assertEquals(gbpUsd.cross(gbpUsd), null);
		assertEquals(gbpUsd.cross(usdGbp), null);
		assertEquals(gbpGbp.cross(gbpUsd), null);
		assertEquals(gbpUsd.cross(gbpGbp), null);

		assertEquals(gbpUsd.cross(usdEur), eurGbp);
		assertEquals(gbpUsd.cross(eurUsd), eurGbp);
		assertEquals(usdGbp.cross(usdEur), eurGbp);
		assertEquals(usdGbp.cross(eurUsd), eurGbp);

		assertEquals(usdEur.cross(gbpUsd), eurGbp);
		assertEquals(usdEur.cross(usdGbp), eurGbp);
		assertEquals(eurUsd.cross(gbpUsd), eurGbp);
		assertEquals(eurUsd.cross(usdGbp), eurGbp);
	  }

	  public virtual void test_cross_CurrencyPair_null()
	  {
		CurrencyPair test = CurrencyPair.of(GBP, USD);
		assertThrowsIllegalArg(() => test.cross(null));
	  }

	  //-----------------------------------------------------------------------
	  public virtual void test_isConventional()
	  {
		assertEquals(CurrencyPair.of(GBP, USD).Conventional, true);
		assertEquals(CurrencyPair.of(USD, GBP).Conventional, false);
		// There is no configuration for GBP/BRL or BRL/GBP so the ordering list is used to choose a convention pair
		// GBP is in the currency order list and BRL isn't so GBP is the base
		assertEquals(CurrencyPair.of(GBP, BRL).Conventional, true);
		assertEquals(CurrencyPair.of(BRL, GBP).Conventional, false);
		// There is no configuration for BHD/BRL or BRL/BHD and neither are in the list specifying currency priority order.
		// Lexicographical ordering is used
		assertEquals(CurrencyPair.of(BHD, BRL).Conventional, true);
		assertEquals(CurrencyPair.of(BRL, BHD).Conventional, false);
		assertEquals(CurrencyPair.of(GBP, GBP).Conventional, true);
	  }

	  public virtual void test_toConventional()
	  {
		assertEquals(CurrencyPair.of(GBP, USD).toConventional(), CurrencyPair.of(GBP, USD));
		assertEquals(CurrencyPair.of(USD, GBP).toConventional(), CurrencyPair.of(GBP, USD));

		assertEquals(CurrencyPair.of(GBP, BRL).toConventional(), CurrencyPair.of(GBP, BRL));
		assertEquals(CurrencyPair.of(BRL, GBP).toConventional(), CurrencyPair.of(GBP, BRL));

		assertEquals(CurrencyPair.of(BHD, BRL).toConventional(), CurrencyPair.of(BHD, BRL));
		assertEquals(CurrencyPair.of(BRL, BHD).toConventional(), CurrencyPair.of(BHD, BRL));
	  }

	  public virtual void test_rateDigits()
	  {
		assertEquals(CurrencyPair.of(GBP, USD).RateDigits, 4);
		assertEquals(CurrencyPair.of(USD, GBP).RateDigits, 4);
		assertEquals(CurrencyPair.of(BRL, GBP).RateDigits, 4);
		assertEquals(CurrencyPair.of(GBP, BRL).RateDigits, 4);
		assertEquals(CurrencyPair.of(BRL, BHD).RateDigits, 5);
		assertEquals(CurrencyPair.of(BHD, BRL).RateDigits, 5);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equals_hashCode()
	  {
		CurrencyPair a1 = CurrencyPair.of(AUD, GBP);
		CurrencyPair a2 = CurrencyPair.of(AUD, GBP);
		CurrencyPair b = CurrencyPair.of(USD, GBP);
		CurrencyPair c = CurrencyPair.of(USD, EUR);

		assertEquals(a1.Equals(a1), true);
		assertEquals(a1.Equals(a2), true);
		assertEquals(a1.Equals(b), false);
		assertEquals(a1.Equals(c), false);

		assertEquals(b.Equals(a1), false);
		assertEquals(b.Equals(a2), false);
		assertEquals(b.Equals(b), true);
		assertEquals(b.Equals(c), false);

		assertEquals(c.Equals(a1), false);
		assertEquals(c.Equals(a2), false);
		assertEquals(c.Equals(b), false);
		assertEquals(c.Equals(c), true);

		assertEquals(a1.GetHashCode(), a2.GetHashCode());
	  }

	  public virtual void test_equals_bad()
	  {
		CurrencyPair test = CurrencyPair.of(AUD, GBP);
		assertFalse(test.Equals(ANOTHER_TYPE));
		assertFalse(test.Equals(null));
	  }

	  //-----------------------------------------------------------------------
	  public virtual void test_serialization()
	  {
		assertSerialization(CurrencyPair.of(GBP, USD));
		assertSerialization(CurrencyPair.of(GBP, GBP));
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(CurrencyPair), CurrencyPair.of(GBP, USD));
		assertJodaConvert(typeof(CurrencyPair), CurrencyPair.of(GBP, GBP));
	  }

	}

}