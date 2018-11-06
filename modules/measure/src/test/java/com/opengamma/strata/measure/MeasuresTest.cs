/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="Measures"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class MeasuresTest
	public class MeasuresTest
	{

	  public virtual void test_standard()
	  {
		assertEquals(Measures.PRESENT_VALUE.CurrencyConvertible, true);
		assertEquals(Measures.EXPLAIN_PRESENT_VALUE.CurrencyConvertible, false);
		assertEquals(Measures.PV01_CALIBRATED_SUM.CurrencyConvertible, true);
		assertEquals(Measures.PV01_CALIBRATED_BUCKETED.CurrencyConvertible, true);
		assertEquals(Measures.PV01_MARKET_QUOTE_SUM.CurrencyConvertible, true);
		assertEquals(Measures.PV01_MARKET_QUOTE_BUCKETED.CurrencyConvertible, true);
		assertEquals(Measures.PAR_RATE.CurrencyConvertible, false);
		assertEquals(Measures.PAR_SPREAD.CurrencyConvertible, false);
		assertEquals(Measures.CURRENCY_EXPOSURE.CurrencyConvertible, false);
		assertEquals(Measures.CURRENT_CASH.CurrencyConvertible, true);
	  }

	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(Measures));
	  }

	}

}