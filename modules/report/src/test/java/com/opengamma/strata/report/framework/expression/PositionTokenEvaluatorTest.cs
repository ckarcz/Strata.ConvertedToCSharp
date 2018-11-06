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
	using StandardId = com.opengamma.strata.basics.StandardId;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using StandardComponents = com.opengamma.strata.measure.StandardComponents;
	using GenericSecurity = com.opengamma.strata.product.GenericSecurity;
	using GenericSecurityPosition = com.opengamma.strata.product.GenericSecurityPosition;
	using Position = com.opengamma.strata.product.Position;
	using PositionInfo = com.opengamma.strata.product.PositionInfo;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using SecurityInfo = com.opengamma.strata.product.SecurityInfo;

	/// <summary>
	/// Test <seealso cref="PositionTokenEvaluator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PositionTokenEvaluatorTest
	public class PositionTokenEvaluatorTest
	{

	  private static readonly CalculationFunctions FUNCTIONS = StandardComponents.calculationFunctions();

	  private static readonly GenericSecurity SECURITY = GenericSecurity.of(SecurityInfo.of(SecurityId.of("OG-Test", "1"), 20, CurrencyAmount.of(USD, 10)));

	  public virtual void tokens()
	  {
		PositionTokenEvaluator evaluator = new PositionTokenEvaluator();
		ISet<string> tokens = evaluator.tokens(trade());
		ISet<string> expected = ImmutableSet.of("longQuantity", "shortQuantity", "quantity", "security", "info", "id", "attributes");
		assertThat(tokens).isEqualTo(expected);
	  }

	  public virtual void evaluate()
	  {
		PositionTokenEvaluator evaluator = new PositionTokenEvaluator();
		Position pos = trade();

		EvaluationResult quantity = evaluator.evaluate(pos, FUNCTIONS, "quantity", ImmutableList.of());
		assertThat(quantity.Result).hasValue(6d);

		EvaluationResult initialPrice = evaluator.evaluate(pos, FUNCTIONS, "security", ImmutableList.of());
		assertThat(initialPrice.Result).hasValue(SECURITY);

		// Check that property name isn't case sensitive
		EvaluationResult initialPrice2 = evaluator.evaluate(pos, FUNCTIONS, "Security", ImmutableList.of());
		assertThat(initialPrice2.Result).hasValue(SECURITY);

		// Unknown property
		EvaluationResult foo = evaluator.evaluate(pos, FUNCTIONS, "foo", ImmutableList.of());
		assertThat(foo.Result).Failure;
	  }

	  private static Position trade()
	  {
		PositionInfo info = PositionInfo.of(StandardId.of("OG-Position", "1"));
		return GenericSecurityPosition.ofNet(info, SECURITY, 6);
	  }

	}

}