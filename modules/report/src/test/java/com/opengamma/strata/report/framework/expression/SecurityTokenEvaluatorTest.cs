using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.expression
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.CollectProjectAssertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using StandardComponents = com.opengamma.strata.measure.StandardComponents;
	using GenericSecurity = com.opengamma.strata.product.GenericSecurity;
	using Security = com.opengamma.strata.product.Security;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using SecurityInfo = com.opengamma.strata.product.SecurityInfo;

	/// <summary>
	/// Test <seealso cref="SecurityTokenEvaluator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SecurityTokenEvaluatorTest
	public class SecurityTokenEvaluatorTest
	{

	  private static readonly CalculationFunctions FUNCTIONS = StandardComponents.calculationFunctions();

	  private static readonly SecurityId ID = SecurityId.of("OG-Test", "1");

	  public virtual void tokens()
	  {
		SecurityTokenEvaluator evaluator = new SecurityTokenEvaluator();
		ISet<string> tokens = evaluator.tokens(security());
		ISet<string> expected = ImmutableSet.of("id", "info", "currency", "priceInfo", "contractSize", "tickSize", "tickValue", "attributes");
		assertThat(tokens).isEqualTo(expected);
	  }

	  public virtual void evaluate()
	  {
		SecurityTokenEvaluator evaluator = new SecurityTokenEvaluator();
		Security sec = security();

		EvaluationResult quantity = evaluator.evaluate(sec, FUNCTIONS, "id", ImmutableList.of());
		assertThat(quantity.Result).hasValue(ID);

		EvaluationResult initialPrice = evaluator.evaluate(sec, FUNCTIONS, "currency", ImmutableList.of());
		assertThat(initialPrice.Result).hasValue(USD);

		// Check that property name isn't case sensitive
		EvaluationResult initialPrice2 = evaluator.evaluate(sec, FUNCTIONS, "Currency", ImmutableList.of());
		assertThat(initialPrice2.Result).hasValue(USD);

		// Unknown property
		EvaluationResult foo = evaluator.evaluate(sec, FUNCTIONS, "foo", ImmutableList.of());
		assertThat(foo.Result).Failure;
	  }

	  private static Security security()
	  {
		SecurityInfo info = SecurityInfo.of(ID, 20, CurrencyAmount.of(USD, 10));
		return GenericSecurity.of(info);
	  }

	}

}