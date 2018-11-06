/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.format
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Test = org.testng.annotations.Test;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;

	/// <summary>
	/// Test <seealso cref="CurrencyParameterSensitivityValueFormatter"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurrencyParameterSensitivityValueFormatterTest
	public class CurrencyParameterSensitivityValueFormatterTest
	{

	  public virtual void formatForCsv()
	  {
		CurrencyParameterSensitivity sensitivity = CurrencyParameterSensitivity.of(CurveName.of("foo"), Currency.AED, DoubleArray.of(10, 20, 30));
		string str = CurrencyParameterSensitivityValueFormatter.INSTANCE.formatForCsv(sensitivity);
		assertThat(str).isEqualTo("1 = 10 | 2 = 20 | 3 = 30");
	  }

	  public virtual void formatForDisplay()
	  {
		CurrencyParameterSensitivity sensitivity = CurrencyParameterSensitivity.of(CurveName.of("foo"), Currency.AED, DoubleArray.of(1, 2, 3));
		string str = CurrencyParameterSensitivityValueFormatter.INSTANCE.formatForDisplay(sensitivity);
		assertThat(str).isEqualTo("1 = 1.00        | 2 = 2.00        | 3 = 3.00       ");
	  }

	}

}