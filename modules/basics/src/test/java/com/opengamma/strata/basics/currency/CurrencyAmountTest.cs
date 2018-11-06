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
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="CurrencyAmount"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurrencyAmountTest
	public class CurrencyAmountTest
	{

	  private static readonly Currency CCY1 = Currency.AUD;
	  private static readonly Currency CCY2 = Currency.CAD;
	  private const double AMT1 = 100;
	  private const double AMT2 = 200;
	  private static readonly CurrencyAmount CCY_AMOUNT = CurrencyAmount.of(CCY1, AMT1);
	  private static readonly CurrencyAmount CCY_AMOUNT_NEGATIVE = CurrencyAmount.of(CCY1, -AMT1);
	  private const object ANOTHER_TYPE = "";

	  public virtual void test_fixture()
	  {
		assertEquals(CCY_AMOUNT.Currency, CCY1);
		assertEquals(CCY_AMOUNT.Amount, AMT1, 0);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_zero_Currency()
	  {
		CurrencyAmount test = CurrencyAmount.zero(Currency.USD);
		assertEquals(test.Currency, Currency.USD);
		assertEquals(test.Amount, 0d, 0);
	  }

	  public virtual void test_zero_Currency_nullCurrency()
	  {
		assertThrowsIllegalArg(() => CurrencyAmount.zero(null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_Currency()
	  {
		CurrencyAmount test = CurrencyAmount.of(Currency.USD, AMT1);
		assertEquals(test.Currency, Currency.USD);
		assertEquals(test.Amount, AMT1, 0);
	  }

	  public virtual void test_of_Currency_nullCurrency()
	  {
		assertThrowsIllegalArg(() => CurrencyAmount.of((Currency) null, AMT1));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_String()
	  {
		CurrencyAmount test = CurrencyAmount.of("USD", AMT1);
		assertEquals(test.Currency, Currency.USD);
		assertEquals(test.Amount, AMT1, 0);
	  }

	  public virtual void test_of_String_nullCurrency()
	  {
		assertThrowsIllegalArg(() => CurrencyAmount.of((string) null, AMT1));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test_parse_String_roundTrip()
	  public virtual void test_parse_String_roundTrip()
	  {
		assertEquals(CurrencyAmount.parse(CCY_AMOUNT.ToString()), CCY_AMOUNT);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "parseGood") public static Object[][] data_parseGood()
	  public static object[][] data_parseGood()
	  {
		return new object[][]
		{
			new object[] {"AUD 100.001", Currency.AUD, 100.001d},
			new object[] {"AUD 321.123", Currency.AUD, 321.123d},
			new object[] {"AUD 123", Currency.AUD, 123d},
			new object[] {"GBP 0", Currency.GBP, 0d},
			new object[] {"USD -0", Currency.USD, -0d},
			new object[] {"EUR -0.01", Currency.EUR, -0.01d}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "parseGood") public void test_parse_String_good(String input, Currency currency, double amount)
	  public virtual void test_parse_String_good(string input, Currency currency, double amount)
	  {
		assertEquals(CurrencyAmount.parse(input), CurrencyAmount.of(currency, amount));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "parseBad") public static Object[][] data_parseBad()
	  public static object[][] data_parseBad()
	  {
		return new object[][]
		{
			new object[] {"AUD"},
			new object[] {"AUD aa"},
			new object[] {"AUD -.+-"},
			new object[] {"123"},
			new object[] {null}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "parseBad") public void test_parse_String_bad(String input)
	  public virtual void test_parse_String_bad(string input)
	  {
		assertThrowsIllegalArg(() => CurrencyAmount.parse(input));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_plus_CurrencyAmount()
	  {
		CurrencyAmount ccyAmount = CurrencyAmount.of(CCY1, AMT2);
		CurrencyAmount test = CCY_AMOUNT.plus(ccyAmount);
		assertEquals(test, CurrencyAmount.of(CCY1, AMT1 + AMT2));
	  }

	  public virtual void test_plus_CurrencyAmount_null()
	  {
		assertThrowsIllegalArg(() => CCY_AMOUNT.plus(null));
	  }

	  public virtual void test_plus_CurrencyAmount_wrongCurrency()
	  {
		assertThrowsIllegalArg(() => CCY_AMOUNT.plus(CurrencyAmount.of(CCY2, AMT2)));
	  }

	  public virtual void test_plus_double()
	  {
		CurrencyAmount test = CCY_AMOUNT.plus(AMT2);
		assertEquals(test, CurrencyAmount.of(CCY1, AMT1 + AMT2));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_minus_CurrencyAmount()
	  {
		CurrencyAmount ccyAmount = CurrencyAmount.of(CCY1, AMT2);
		CurrencyAmount test = CCY_AMOUNT.minus(ccyAmount);
		assertEquals(test, CurrencyAmount.of(CCY1, AMT1 - AMT2));
	  }

	  public virtual void test_minus_CurrencyAmount_null()
	  {
		assertThrowsIllegalArg(() => CCY_AMOUNT.minus(null));
	  }

	  public virtual void test_minus_CurrencyAmount_wrongCurrency()
	  {
		assertThrowsIllegalArg(() => CCY_AMOUNT.minus(CurrencyAmount.of(CCY2, AMT2)));
	  }

	  public virtual void test_minus_double()
	  {
		CurrencyAmount test = CCY_AMOUNT.minus(AMT2);
		assertEquals(test, CurrencyAmount.of(CCY1, AMT1 - AMT2));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_multipliedBy()
	  {
		CurrencyAmount test = CCY_AMOUNT.multipliedBy(3.5);
		assertEquals(test, CurrencyAmount.of(CCY1, AMT1 * 3.5));
	  }

	  public virtual void test_mapAmount()
	  {
		CurrencyAmount test = CCY_AMOUNT.mapAmount(v => v * 2 + 1);
		assertEquals(test, CurrencyAmount.of(CCY1, AMT1 * 2 + 1));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_negated()
	  {
		assertEquals(CCY_AMOUNT.negated(), CCY_AMOUNT_NEGATIVE);
		assertEquals(CCY_AMOUNT_NEGATIVE.negated(), CCY_AMOUNT);
		assertEquals(CurrencyAmount.zero(Currency.USD), CurrencyAmount.zero(Currency.USD).negated());
		assertEquals(CurrencyAmount.of(Currency.USD, -0d).negated(), CurrencyAmount.zero(Currency.USD));
	  }

	  public virtual void test_negative()
	  {
		assertEquals(CCY_AMOUNT.negative(), CCY_AMOUNT_NEGATIVE);
		assertEquals(CCY_AMOUNT_NEGATIVE.negative(), CCY_AMOUNT_NEGATIVE);
	  }

	  public virtual void test_positive()
	  {
		assertEquals(CCY_AMOUNT.positive(), CCY_AMOUNT);
		assertEquals(CCY_AMOUNT_NEGATIVE.positive(), CCY_AMOUNT);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_convertedTo_explicitRate()
	  {
		assertEquals(CCY_AMOUNT.convertedTo(CCY2, 2.5d), CurrencyAmount.of(CCY2, AMT1 * 2.5d));
		assertEquals(CCY_AMOUNT.convertedTo(CCY1, 1d), CCY_AMOUNT);
		assertThrowsIllegalArg(() => CCY_AMOUNT.convertedTo(CCY1, 1.5d));
	  }

	  public virtual void test_convertedTo_rateProvider()
	  {
		FxRateProvider provider = (ccy1, ccy2) => 2.5d;
		assertEquals(CCY_AMOUNT.convertedTo(CCY2, provider), CurrencyAmount.of(CCY2, AMT1 * 2.5d));
		assertEquals(CCY_AMOUNT.convertedTo(CCY1, provider), CCY_AMOUNT);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equals_hashCode()
	  {
		CurrencyAmount other = CurrencyAmount.of(CCY1, AMT1);
		assertTrue(CCY_AMOUNT.Equals(CCY_AMOUNT));
		assertTrue(CCY_AMOUNT.Equals(other));
		assertTrue(other.Equals(CCY_AMOUNT));
		assertEquals(CCY_AMOUNT.GetHashCode(), other.GetHashCode());
		other = CurrencyAmount.of(CCY1, AMT1);
		assertEquals(CCY_AMOUNT, other);
		assertEquals(CCY_AMOUNT.GetHashCode(), other.GetHashCode());
		other = CurrencyAmount.of(CCY2, AMT1);
		assertFalse(CCY_AMOUNT.Equals(other));
		other = CurrencyAmount.of(CCY1, AMT2);
		assertFalse(CCY_AMOUNT.Equals(other));
	  }

	  public virtual void test_equals_bad()
	  {
		assertFalse(CCY_AMOUNT.Equals(ANOTHER_TYPE));
		assertFalse(CCY_AMOUNT.Equals(null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toString()
	  {
		assertEquals(CurrencyAmount.of(Currency.AUD, 100d).ToString(), "AUD 100");
		assertEquals(CurrencyAmount.of(Currency.AUD, 100.123d).ToString(), "AUD 100.123");
	  }

	  //-----------------------------------------------------------------------
	  public virtual void test_serialization()
	  {
		assertSerialization(CCY_AMOUNT);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(CurrencyAmount), CCY_AMOUNT);
	  }

	}

}