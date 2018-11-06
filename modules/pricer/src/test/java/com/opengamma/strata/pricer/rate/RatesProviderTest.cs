/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.rate
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.impl.MockRatesProvider.RATE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using MockRatesProvider = com.opengamma.strata.pricer.impl.MockRatesProvider;

	/// <summary>
	/// Test <seealso cref="RatesProvider"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class RatesProviderTest
	public class RatesProviderTest
	{

	  public virtual void test_fxRate_CurrencyPair()
	  {
		RatesProvider mockProv = new MockRatesProvider();
		assertEquals(mockProv.fxRate(CurrencyPair.of(GBP, USD)), RATE);
	  }

	}

}