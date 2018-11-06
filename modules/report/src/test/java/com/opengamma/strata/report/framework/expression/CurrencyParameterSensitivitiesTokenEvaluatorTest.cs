using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.expression
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.CollectProjectAssertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using StandardComponents = com.opengamma.strata.measure.StandardComponents;

	/// <summary>
	/// Test <seealso cref="CurrencyParameterSensitivitiesTokenEvaluator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurrencyParameterSensitivitiesTokenEvaluatorTest
	public class CurrencyParameterSensitivitiesTokenEvaluatorTest
	{

	  private static readonly CalculationFunctions FUNCTIONS = StandardComponents.calculationFunctions();

	  public virtual void tokens()
	  {
		CurrencyParameterSensitivity sensitivity1 = CurrencyParameterSensitivity.of(CurveName.of("curve1"), Currency.AUD, DoubleArray.EMPTY);

		CurrencyParameterSensitivity sensitivity2 = CurrencyParameterSensitivity.of(CurveName.of("curve2"), Currency.CHF, DoubleArray.EMPTY);

		CurrencyParameterSensitivities sensitivities = CurrencyParameterSensitivities.of(sensitivity1, sensitivity2);

		ISet<string> expected = ImmutableSet.of("curve1", "curve2", "aud", "chf");
		CurrencyParameterSensitivitiesTokenEvaluator evaluator = new CurrencyParameterSensitivitiesTokenEvaluator();
		assertThat(evaluator.tokens(sensitivities)).isEqualTo(expected);
	  }

	  public virtual void evaluate()
	  {
		CurrencyParameterSensitivity sensitivity1 = CurrencyParameterSensitivity.of(CurveName.of("curve1"), Currency.AUD, DoubleArray.EMPTY);

		CurrencyParameterSensitivity sensitivity2 = CurrencyParameterSensitivity.of(CurveName.of("curve2"), Currency.CHF, DoubleArray.EMPTY);

		CurrencyParameterSensitivity sensitivity3 = CurrencyParameterSensitivity.of(CurveName.of("curve2"), Currency.AUD, DoubleArray.EMPTY);

		CurrencyParameterSensitivities sensitivities = CurrencyParameterSensitivities.of(sensitivity1, sensitivity2, sensitivity3);

		CurrencyParameterSensitivitiesTokenEvaluator evaluator = new CurrencyParameterSensitivitiesTokenEvaluator();

		CurrencyParameterSensitivities expected1 = CurrencyParameterSensitivities.of(sensitivity1, sensitivity3);
		EvaluationResult result1 = evaluator.evaluate(sensitivities, FUNCTIONS, "aud", ImmutableList.of());
		assertThat(result1.Result).Success;
		CurrencyParameterSensitivities result1Value = (CurrencyParameterSensitivities) result1.Result.Value;
		assertThat(result1Value.Sensitivities).containsAll(expected1.Sensitivities);

		CurrencyParameterSensitivities expected2 = CurrencyParameterSensitivities.of(sensitivity2, sensitivity3);
		EvaluationResult result2 = evaluator.evaluate(sensitivities, FUNCTIONS, "curve2", ImmutableList.of());
		assertThat(result2.Result).Success;
		CurrencyParameterSensitivities result2Value = (CurrencyParameterSensitivities) result2.Result.Value;
		assertThat(result2Value.Sensitivities).containsAll(expected2.Sensitivities);

		EvaluationResult result3 = evaluator.evaluate(sensitivities, FUNCTIONS, "chf", ImmutableList.of());
		assertThat(result3.Result).hasValue(sensitivity2);

		EvaluationResult result4 = evaluator.evaluate(sensitivities, FUNCTIONS, "usd", ImmutableList.of());
		assertThat(result4.Result).Failure;
	  }
	}

}