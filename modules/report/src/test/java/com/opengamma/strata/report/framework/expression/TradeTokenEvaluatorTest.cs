using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using GenericSecurityTrade = com.opengamma.strata.product.GenericSecurityTrade;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using SecurityInfo = com.opengamma.strata.product.SecurityInfo;
	using Trade = com.opengamma.strata.product.Trade;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;

	/// <summary>
	/// Test <seealso cref="TradeTokenEvaluator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TradeTokenEvaluatorTest
	public class TradeTokenEvaluatorTest
	{

	  private static readonly CalculationFunctions FUNCTIONS = StandardComponents.calculationFunctions();

	  public virtual void tokens()
	  {
		TradeTokenEvaluator evaluator = new TradeTokenEvaluator();
		ISet<string> tokens = evaluator.tokens(trade());
		ISet<string> expected = ImmutableSet.of("quantity", "price", "security", "id", "counterparty", "tradeDate", "tradeTime", "zone", "settlementDate", "attributes", "info");
		assertThat(tokens).isEqualTo(expected);
	  }

	  public virtual void evaluate()
	  {
		TradeTokenEvaluator evaluator = new TradeTokenEvaluator();
		Trade trade = TradeTokenEvaluatorTest.trade();

		EvaluationResult quantity = evaluator.evaluate(trade, FUNCTIONS, "quantity", ImmutableList.of());
		assertThat(quantity.Result).hasValue(123d);

		EvaluationResult initialPrice = evaluator.evaluate(trade, FUNCTIONS, "price", ImmutableList.of());
		assertThat(initialPrice.Result).hasValue(456d);

		// Check that property name isn't case sensitive
		EvaluationResult initialPrice2 = evaluator.evaluate(trade, FUNCTIONS, "price", ImmutableList.of());
		assertThat(initialPrice2.Result).hasValue(456d);

		EvaluationResult counterparty = evaluator.evaluate(trade, FUNCTIONS, "counterparty", ImmutableList.of());
		assertThat(counterparty.Result).hasValue(StandardId.of("cpty", "a"));

		// Optional property with no value
		EvaluationResult tradeTime = evaluator.evaluate(trade, FUNCTIONS, "tradeTime", ImmutableList.of());
		assertThat(tradeTime.Result).Failure;

		// Unknown property
		EvaluationResult foo = evaluator.evaluate(trade, FUNCTIONS, "foo", ImmutableList.of());
		assertThat(foo.Result).Failure;
	  }

	  private static Trade trade()
	  {
		SecurityInfo info = SecurityInfo.of(SecurityId.of("OG-Test", "1"), 20, CurrencyAmount.of(USD, 10));
		GenericSecurity security = GenericSecurity.of(info);
		TradeInfo tradeInfo = TradeInfo.builder().counterparty(StandardId.of("cpty", "a")).build();
		return GenericSecurityTrade.builder().info(tradeInfo).security(security).quantity(123).price(456).build();
	  }

	}

}