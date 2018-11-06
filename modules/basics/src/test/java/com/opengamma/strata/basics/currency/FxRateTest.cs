/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.currency
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="FxRate"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxRateTest
	public class FxRateTest
	{

	  private static readonly Currency AUD = Currency.AUD;
	  private static readonly Currency CAD = Currency.CAD;
	  private static readonly Currency EUR = Currency.EUR;
	  private static readonly Currency GBP = Currency.GBP;
	  private static readonly Currency USD = Currency.USD;
	  private const object ANOTHER_TYPE = "";

	  //-------------------------------------------------------------------------
	  public virtual void test_of_CurrencyCurrencyDouble()
	  {
		FxRate test = FxRate.of(GBP, USD, 1.5d);
		assertEquals(test.Pair, CurrencyPair.of(GBP, USD));
		assertEquals(test.fxRate(GBP, USD), 1.5d, 0);
		assertEquals(test.ToString(), "GBP/USD 1.5");
	  }

	  public virtual void test_of_CurrencyCurrencyDouble_reverseStandardOrder()
	  {
		FxRate test = FxRate.of(USD, GBP, 0.8d);
		assertEquals(test.Pair, CurrencyPair.of(USD, GBP));
		assertEquals(test.fxRate(USD, GBP), 0.8d, 0);
		assertEquals(test.ToString(), "USD/GBP 0.8");
	  }

	  public virtual void test_of_CurrencyCurrencyDouble_same()
	  {
		FxRate test = FxRate.of(USD, USD, 1d);
		assertEquals(test.Pair, CurrencyPair.of(USD, USD));
		assertEquals(test.fxRate(USD, USD), 1d, 0);
		assertEquals(test.ToString(), "USD/USD 1");
	  }

	  public virtual void test_of_CurrencyCurrencyDouble_invalid()
	  {
		assertThrowsIllegalArg(() => FxRate.of(GBP, USD, -1.5d));
		assertThrowsIllegalArg(() => FxRate.of(GBP, GBP, 2d));
	  }

	  public virtual void test_of_CurrencyCurrencyDouble_null()
	  {
		assertThrowsIllegalArg(() => FxRate.of(null, USD, 1.5d));
		assertThrowsIllegalArg(() => FxRate.of(USD, null, 1.5d));
		assertThrowsIllegalArg(() => FxRate.of(null, null, 1.5d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_CurrencyPairDouble()
	  {
		FxRate test = FxRate.of(CurrencyPair.of(GBP, USD), 1.5d);
		assertEquals(test.Pair, CurrencyPair.of(GBP, USD));
		assertEquals(test.fxRate(GBP, USD), 1.5d, 0);
		assertEquals(test.ToString(), "GBP/USD 1.5");
	  }

	  public virtual void test_of_CurrencyPairDouble_reverseStandardOrder()
	  {
		FxRate test = FxRate.of(CurrencyPair.of(USD, GBP), 0.8d);
		assertEquals(test.Pair, CurrencyPair.of(USD, GBP));
		assertEquals(test.fxRate(USD, GBP), 0.8d, 0);
		assertEquals(test.ToString(), "USD/GBP 0.8");
	  }

	  public virtual void test_of_CurrencyPairDouble_same()
	  {
		FxRate test = FxRate.of(CurrencyPair.of(USD, USD), 1d);
		assertEquals(test.Pair, CurrencyPair.of(USD, USD));
		assertEquals(test.fxRate(USD, USD), 1d, 0);
		assertEquals(test.ToString(), "USD/USD 1");
	  }

	  public virtual void test_of_CurrencyPairDouble_invalid()
	  {
		assertThrowsIllegalArg(() => FxRate.of(CurrencyPair.of(GBP, USD), -1.5d));
		assertThrowsIllegalArg(() => FxRate.of(CurrencyPair.of(USD, USD), 2d));
	  }

	  public virtual void test_of_CurrencyPairDouble_null()
	  {
		assertThrowsIllegalArg(() => FxRate.of(null, 1.5d));
	  }

	  public virtual void test_toConventional()
	  {
		assertEquals(FxRate.of(GBP, USD, 1.25), FxRate.of(USD, GBP, 0.8).toConventional());
		assertEquals(FxRate.of(GBP, USD, 1.25), FxRate.of(GBP, USD, 1.25).toConventional());
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "parseGood") public static Object[][] data_parseGood()
	  public static object[][] data_parseGood()
	  {
		return new object[][]
		{
			new object[] {"USD/EUR 205.123", USD, EUR, 205.123d},
			new object[] {"USD/EUR 3.00000000", USD, EUR, 3d},
			new object[] {"USD/EUR 2", USD, EUR, 2d},
			new object[] {"USD/EUR 0.1", USD, EUR, 0.1d},
			new object[] {"EUR/USD 0.001", EUR, USD, 0.001d},
			new object[] {"EUR/EUR 1", EUR, EUR, 1d},
			new object[] {"cAd/GbP 1.25", CAD, GBP, 1.25d}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "parseGood") public void test_parse_String_good(String input, Currency super, Currency counter, double rate)
	  public virtual void test_parse_String_good(string input, Currency @base, Currency counter, double rate)
	  {
		assertEquals(FxRate.parse(input), FxRate.of(@base, counter, rate));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "parseBad") public static Object[][] data_parseBad()
	  public static object[][] data_parseBad()
	  {
		return new object[][]
		{
			new object[] {"AUD 1.25"},
			new object[] {"AUD/GB 1.25"},
			new object[] {"AUD GBP 1.25"},
			new object[] {"AUD:GBP 1.25"},
			new object[] {"123/456"},
			new object[] {"EUR/GBP -1.25"},
			new object[] {"EUR/GBP 0"},
			new object[] {"EUR/EUR 1.25"},
			new object[] {""},
			new object[] {null}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "parseBad", expectedExceptions = IllegalArgumentException.class) public void test_parse_String_bad(String input)
	  public virtual void test_parse_String_bad(string input)
	  {
		FxRate.parse(input);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_inverse()
	  {
		FxRate test = FxRate.of(GBP, USD, 1.25d);
		assertEquals(test.inverse(), FxRate.of(USD, GBP, 0.8d));
	  }

	  public virtual void test_inverse_same()
	  {
		FxRate test = FxRate.of(GBP, GBP, 1d);
		assertEquals(test.inverse(), FxRate.of(GBP, GBP, 1d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_fxRate_forBase()
	  {
		FxRate test = FxRate.of(GBP, USD, 1.25d);
		assertEquals(test.fxRate(GBP, USD), 1.25d);
		assertEquals(test.fxRate(USD, GBP), 1d / 1.25d);
		assertThrowsIllegalArg(() => test.fxRate(GBP, AUD));
	  }

	  public virtual void test_fxRate_forPair()
	  {
		FxRate test = FxRate.of(GBP, USD, 1.25d);
		assertEquals(test.fxRate(GBP, USD), 1.25d);
		assertEquals(test.fxRate(USD, GBP), 1d / 1.25d);
		assertEquals(test.fxRate(GBP, GBP), 1d);
		assertEquals(test.fxRate(USD, USD), 1d);
		assertEquals(test.fxRate(AUD, AUD), 1d);
		assertThrowsIllegalArg(() => test.fxRate(AUD, GBP));
		assertThrowsIllegalArg(() => test.fxRate(GBP, AUD));
		assertThrowsIllegalArg(() => test.fxRate(AUD, USD));
		assertThrowsIllegalArg(() => test.fxRate(USD, AUD));
		assertThrowsIllegalArg(() => test.fxRate(EUR, AUD));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_convert()
	  {
		FxRate test = FxRate.of(GBP, USD, 1.25d);
		assertEquals(test.convert(100, GBP, USD), 125d);
		assertEquals(test.convert(100, USD, GBP), 100d / 1.25d);
		assertThrowsIllegalArg(() => test.convert(100, GBP, AUD));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_crossRate()
	  {
		FxRate gbpUsd = FxRate.of(GBP, USD, 5d / 4d);
		FxRate usdGbp = FxRate.of(USD, GBP, 4d / 5d);
		FxRate eurUsd = FxRate.of(EUR, USD, 8d / 7d);
		FxRate usdEur = FxRate.of(USD, EUR, 7d / 8d);
		FxRate eurGbp = FxRate.of(EUR, GBP, (8d / 7d) * (4d / 5d));
		FxRate gbpGbp = FxRate.of(GBP, GBP, 1d);
		FxRate usdUsd = FxRate.of(USD, USD, 1d);

		assertEquals(eurUsd.crossRate(usdGbp), eurGbp);
		assertEquals(eurUsd.crossRate(gbpUsd), eurGbp);
		assertEquals(usdEur.crossRate(usdGbp), eurGbp);
		assertEquals(usdEur.crossRate(gbpUsd), eurGbp);

		assertEquals(gbpUsd.crossRate(usdEur), eurGbp);
		assertEquals(gbpUsd.crossRate(eurUsd), eurGbp);
		assertEquals(usdGbp.crossRate(usdEur), eurGbp);
		assertEquals(usdGbp.crossRate(eurUsd), eurGbp);

		assertThrowsIllegalArg(() => gbpGbp.crossRate(gbpUsd)); // identity
		assertThrowsIllegalArg(() => usdUsd.crossRate(gbpUsd)); // identity
		assertThrowsIllegalArg(() => gbpUsd.crossRate(gbpUsd)); // same currencies
		assertThrowsIllegalArg(() => gbpUsd.crossRate(usdGbp)); // same currencies
		assertThrowsIllegalArg(() => gbpUsd.crossRate(FxRate.of(EUR, CAD, 12d / 5d))); // no common currency
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equals_hashCode()
	  {
		FxRate a1 = FxRate.of(AUD, GBP, 1.25d);
		FxRate a2 = FxRate.of(AUD, GBP, 1.25d);
		FxRate b = FxRate.of(USD, GBP, 1.25d);
		FxRate c = FxRate.of(USD, GBP, 1.35d);

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
		FxRate test = FxRate.of(AUD, GBP, 1.25d);
		assertFalse(test.Equals(ANOTHER_TYPE));
		assertFalse(test.Equals(null));
	  }

	  //-----------------------------------------------------------------------
	  public virtual void test_serialization()
	  {
		assertSerialization(FxRate.of(GBP, USD, 1.25d));
		assertSerialization(FxRate.of(GBP, GBP, 1d));
	  }

	  //-----------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(FxRate.of(GBP, USD, 1.25d));
	  }

	}

}