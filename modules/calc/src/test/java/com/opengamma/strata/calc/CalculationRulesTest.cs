/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using CalculationParameter = com.opengamma.strata.calc.runner.CalculationParameter;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using TestParameter = com.opengamma.strata.calc.runner.TestParameter;

	/// <summary>
	/// Test <seealso cref="CalculationRules"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CalculationRulesTest
	public class CalculationRulesTest
	{

	  private static readonly CalculationFunctions FUNCTIONS = CalculationFunctions.empty();
	  private static readonly CalculationParameter PARAM = new TestParameter();

	  //-------------------------------------------------------------------------
	  public virtual void test_of_FunctionsParametersArray()
	  {
		CalculationRules test = CalculationRules.of(FUNCTIONS, PARAM);
		assertEquals(test.Functions, FUNCTIONS);
		assertEquals(test.ReportingCurrency, ReportingCurrency.NATURAL);
		assertEquals(test.Parameters, CalculationParameters.of(PARAM));
	  }

	  public virtual void test_of_FunctionsParametersObject()
	  {
		CalculationRules test = CalculationRules.of(FUNCTIONS, CalculationParameters.of(PARAM));
		assertEquals(test.Functions, FUNCTIONS);
		assertEquals(test.ReportingCurrency, ReportingCurrency.NATURAL);
		assertEquals(test.Parameters, CalculationParameters.of(PARAM));
	  }

	  public virtual void test_of_FunctionsCurrencyParametersArray()
	  {
		CalculationRules test = CalculationRules.of(FUNCTIONS, USD, PARAM);
		assertEquals(test.Functions, FUNCTIONS);
		assertEquals(test.ReportingCurrency, ReportingCurrency.of(USD));
		assertEquals(test.Parameters, CalculationParameters.of(PARAM));
	  }

	  public virtual void test_of_All()
	  {
		CalculationRules test = CalculationRules.of(FUNCTIONS, ReportingCurrency.of(USD), CalculationParameters.of(PARAM));
		assertEquals(test.Functions, FUNCTIONS);
		assertEquals(test.ReportingCurrency, ReportingCurrency.of(USD));
		assertEquals(test.Parameters, CalculationParameters.of(PARAM));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CalculationRules test = CalculationRules.of(FUNCTIONS);
		coverImmutableBean(test);
		CalculationRules test2 = CalculationRules.of(FUNCTIONS, USD);
		coverBeanEquals(test, test2);
	  }

	}

}