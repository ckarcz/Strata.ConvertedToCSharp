/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.currency
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	public class MoneyTest
	{

	  private static readonly Currency CCY_AUD = Currency.AUD;
	  private const double AMT_100_12 = 100.12;
	  private const double AMT_100_MORE_DECIMALS = 100.1249;
	  private static readonly Currency CCY_RON = Currency.RON;
	  private const double AMT_200_23 = 200.23;
	  private static readonly Currency CCY_BHD = Currency.BHD; //3 decimals
	  private static readonly CurrencyAmount CCYAMT = CurrencyAmount.of(CCY_RON, AMT_200_23);
	  //Not the Money instances
	  private static readonly Money MONEY_200_RON = Money.of(CCYAMT);
	  private static readonly Money MONEY_100_AUD = Money.of(CCY_AUD, AMT_100_12);
	  private static readonly Money MONEY_100_13_AUD = Money.of(CCY_AUD, AMT_100_MORE_DECIMALS);
	  private static readonly Money MONEY_200_RON_ALTERNATIVE = Money.of(CCY_RON, AMT_200_23);
	  private static readonly Money MONEY_100_12_BHD = Money.of(CCY_BHD, AMT_100_12);
	  private static readonly Money MONEY_100_125_BHD = Money.of(CCY_BHD, AMT_100_MORE_DECIMALS);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testOfCurrencyAndAmount() throws Exception
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
	  public virtual void testOfCurrencyAndAmount()
	  {
		assertEquals(MONEY_100_AUD.Currency, CCY_AUD);
		assertEquals(MONEY_100_AUD.Amount, (new decimal(AMT_100_12)).setScale(2, RoundingMode.HALF_UP));
		assertEquals(MONEY_100_13_AUD.Currency, CCY_AUD);
		assertEquals(MONEY_100_13_AUD.Amount, decimal.valueOf(AMT_100_12).setScale(2, RoundingMode.HALF_UP)); //Testing the rounding from 3 to 2 decimals
		assertEquals(MONEY_100_12_BHD.Currency, CCY_BHD);
		assertEquals(MONEY_100_12_BHD.Amount, decimal.valueOf(AMT_100_12).setScale(3, RoundingMode.HALF_UP));
		assertEquals(MONEY_100_125_BHD.Currency, CCY_BHD);
		assertEquals(MONEY_100_125_BHD.Amount, decimal.valueOf(100.125)); //Testing the rounding from 4 to 3 decimals

	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testOfCurrencyAmount() throws Exception
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
	  public virtual void testOfCurrencyAmount()
	  {
		assertEquals(MONEY_200_RON.Currency, CCY_RON);
		assertEquals(MONEY_200_RON.Amount, (new decimal(AMT_200_23)).setScale(2, RoundingMode.HALF_EVEN));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testConvertedToWithExplicitRate() throws Exception
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
	  public virtual void testConvertedToWithExplicitRate()
	  {
		assertEquals(Money.of(Currency.RON, 200.23), MONEY_200_RON.convertedTo(CCY_RON, decimal.valueOf(1)));
		assertEquals(Money.of(Currency.RON, 260.31), MONEY_100_AUD.convertedTo(CCY_RON, decimal.valueOf(2.6d)));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = "FX rate must be 1 when no conversion required") public void testConvertedToWithExplicitRateForSameCurrency() throws Exception
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
	  public virtual void testConvertedToWithExplicitRateForSameCurrency()
	  {
		assertEquals(Money.of(Currency.RON, 200.23), MONEY_200_RON.convertedTo(CCY_RON, decimal.valueOf(1.1)));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testConvertedToWithRateProvider() throws Exception
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
	  public virtual void testConvertedToWithRateProvider()
	  {
		FxRateProvider provider = (ccy1, ccy2) => 2.5d;
		assertEquals(Money.of(Currency.RON, 250.30), MONEY_100_AUD.convertedTo(CCY_RON, provider));
		assertEquals(Money.of(Currency.RON, 200.23), MONEY_200_RON.convertedTo(CCY_RON, provider));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testCompareTo() throws Exception
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
	  public virtual void testCompareTo()
	  {
		assertEquals(-1, MONEY_100_AUD.CompareTo(MONEY_200_RON));
		assertEquals(0, MONEY_200_RON.CompareTo(MONEY_200_RON_ALTERNATIVE));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testEquals() throws Exception
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
	  public virtual void testEquals()
	  {
		assertTrue(MONEY_200_RON.Equals(MONEY_200_RON));
		assertFalse(MONEY_200_RON.Equals(null));
		assertTrue(MONEY_200_RON.Equals(MONEY_200_RON_ALTERNATIVE));
		assertFalse(MONEY_100_AUD.Equals(MONEY_200_RON));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testHashCode() throws Exception
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
	  public virtual void testHashCode()
	  {
		assertTrue(MONEY_200_RON.GetHashCode() == MONEY_200_RON_ALTERNATIVE.GetHashCode());
		assertFalse(MONEY_200_RON.GetHashCode() == MONEY_100_AUD.GetHashCode());
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testToString() throws Exception
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
	  public virtual void testToString()
	  {
		assertEquals("RON 200.23", MONEY_200_RON.ToString());
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testParse() throws Exception
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
	  public virtual void testParse()
	  {
		assertEquals(Money.parse("RON 200.23"), MONEY_200_RON);
		assertEquals(Money.parse("RON 200.2345"), MONEY_200_RON);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = "Unable to parse amount: 200.23 RON") public void testParseWrongFormat() throws Exception
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
	  public virtual void testParseWrongFormat()
	  {
		Money.parse("200.23 RON");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = "Unable to parse amount, invalid format: [$]100") public void testParseWrongElementsNumber() throws Exception
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
	  public virtual void testParseWrongElementsNumber()
	  {
		Money.parse("$100");
	  }

	}

}