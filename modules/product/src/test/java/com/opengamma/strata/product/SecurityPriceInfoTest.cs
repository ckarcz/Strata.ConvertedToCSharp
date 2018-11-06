/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.JPY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;

	/// <summary>
	/// Test <seealso cref="SecurityPriceInfo"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SecurityPriceInfoTest
	public class SecurityPriceInfoTest
	{

	  public virtual void test_of()
	  {
		SecurityPriceInfo test = SecurityPriceInfo.of(0.01, CurrencyAmount.of(GBP, 0.01));
		assertEquals(test.TickSize, 0.01);
		assertEquals(test.TickValue, CurrencyAmount.of(GBP, 0.01));
		assertEquals(test.ContractSize, 1d);
		assertEquals(test.Currency, GBP);
	  }

	  public virtual void test_of_withContractSize()
	  {
		SecurityPriceInfo test = SecurityPriceInfo.of(0.01, CurrencyAmount.of(GBP, 0.01), 20);
		assertEquals(test.TickSize, 0.01);
		assertEquals(test.TickValue, CurrencyAmount.of(GBP, 0.01));
		assertEquals(test.ContractSize, 20d);
		assertEquals(test.Currency, GBP);
	  }

	  public virtual void test_ofCurrencyMinorUnit_GBP()
	  {
		SecurityPriceInfo test = SecurityPriceInfo.ofCurrencyMinorUnit(GBP);
		assertEquals(test.TickSize, 0.01);
		assertEquals(test.TickValue, CurrencyAmount.of(GBP, 0.01));
		assertEquals(test.ContractSize, 1d);
		assertEquals(test.Currency, GBP);
	  }

	  public virtual void test_ofCurrencyMinorUnit_JPY()
	  {
		SecurityPriceInfo test = SecurityPriceInfo.ofCurrencyMinorUnit(JPY);
		assertEquals(test.TickSize, 1d);
		assertEquals(test.TickValue, CurrencyAmount.of(JPY, 1));
		assertEquals(test.ContractSize, 1d);
		assertEquals(test.Currency, JPY);
	  }

	  public virtual void test_ofTradeUnitValue()
	  {
		SecurityPriceInfo priceInfo = SecurityPriceInfo.of(USD, 2000);
		double value = priceInfo.calculateMonetaryValue(3, 2);
		assertEquals(value, 12_000d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_calculateMonetaryAmount1()
	  {
		// CME-ED, 1bp = $25
		SecurityPriceInfo test = SecurityPriceInfo.of(0.005, CurrencyAmount.of(USD, 12.50), 1);
		assertEquals(test.calculateMonetaryAmount(1, 98), CurrencyAmount.of(USD, 245_000));
		assertEquals(test.calculateMonetaryAmount(1, 98.02), CurrencyAmount.of(USD, 245_050));
		// quantity is simple multiplier
		assertEquals(test.calculateMonetaryAmount(2, 98), CurrencyAmount.of(USD, 2 * 245_000));
		assertEquals(test.calculateMonetaryAmount(3, 98), CurrencyAmount.of(USD, 3 * 245_000));
		// contract size is simple multiplier
		SecurityPriceInfo test2 = SecurityPriceInfo.of(0.005, CurrencyAmount.of(USD, 12.50), 2);
		assertEquals(test2.calculateMonetaryAmount(1, 98), CurrencyAmount.of(USD, 2 * 245_000));
	  }

	  public virtual void test_calculateMonetaryValue()
	  {
		// CME-ED, 1bp = $25
		SecurityPriceInfo test = SecurityPriceInfo.of(0.005, CurrencyAmount.of(USD, 12.50), 1);
		assertEquals(test.calculateMonetaryValue(1, 98), 245_000d);
		assertEquals(test.calculateMonetaryValue(1, 98.02), 245_050d);
		// quantity is simple multiplier
		assertEquals(test.calculateMonetaryValue(2, 98), 2 * 245_000d);
		assertEquals(test.calculateMonetaryValue(3, 98), 3 * 245_000d);
		// contract size is simple multiplier
		SecurityPriceInfo test2 = SecurityPriceInfo.of(0.005, CurrencyAmount.of(USD, 12.50), 2);
		assertEquals(test2.calculateMonetaryValue(1, 98), 2 * 245_000d);
	  }

	  public virtual void test_getTradeUnitValue()
	  {
		SecurityPriceInfo test = SecurityPriceInfo.of(0.005, CurrencyAmount.of(USD, 12.50), 2);
		assertEquals(test.TradeUnitValue, 5000d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		SecurityPriceInfo test = SecurityPriceInfo.of(0.01, CurrencyAmount.of(GBP, 0.01));
		coverImmutableBean(test);
		SecurityPriceInfo test2 = SecurityPriceInfo.of(0.02, CurrencyAmount.of(GBP, 1), 20);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		SecurityPriceInfo test = SecurityPriceInfo.of(0.01, CurrencyAmount.of(GBP, 0.01));
		assertSerialization(test);
	  }

	}

}