using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.expression
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.CollectProjectAssertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.BUY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Bean = org.joda.beans.Bean;
	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using LegAmounts = com.opengamma.strata.market.amount.LegAmounts;
	using SwapLegAmount = com.opengamma.strata.market.amount.SwapLegAmount;
	using StandardComponents = com.opengamma.strata.measure.StandardComponents;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using Fra = com.opengamma.strata.product.fra.Fra;
	using SwapLegType = com.opengamma.strata.product.swap.SwapLegType;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BeanTokenEvaluatorTest
	public class BeanTokenEvaluatorTest
	{

	  private static readonly CalculationFunctions FUNCTIONS = StandardComponents.calculationFunctions();

	  public virtual void evaluate()
	  {
		Bean bean = BeanTokenEvaluatorTest.bean();
		BeanTokenEvaluator evaluator = new BeanTokenEvaluator();

		EvaluationResult notional1 = evaluator.evaluate(bean, FUNCTIONS, "notional", ImmutableList.of());
		assertThat(notional1.Result).hasValue(1_000_000d);

		EvaluationResult notional2 = evaluator.evaluate(bean, FUNCTIONS, "Notional", ImmutableList.of());
		assertThat(notional2.Result).hasValue(1_000_000d);
	  }

	  public virtual void tokens()
	  {
		Bean bean = BeanTokenEvaluatorTest.bean();
		BeanTokenEvaluator evaluator = new BeanTokenEvaluator();

		ISet<string> tokens = evaluator.tokens(bean);
		ImmutableSet<string> expectedTokens = ImmutableSet.of("buySell", "currency", "notional", "startDate", "endDate", "businessDayAdjustment", "paymentDate", "fixedRate", "index", "indexInterpolated", "fixingDateOffset", "dayCount", "discounting");

		assertThat(tokens).isEqualTo(expectedTokens);
	  }

	  /// <summary>
	  /// Tests evaluating a bean with a single property. There are 2 different expected behaviours:
	  /// 
	  /// 1) If the token matches the property, the property value is returned and the token is consumed. This is the same
	  ///    as the normal bean behaviour.
	  /// 2) If the token doesn't match the property it is assumed to match something on the property's value. In this
	  ///    case the property value is returned and no tokens are consumed.
	  /// </summary>
	  public virtual void evaluateSingleProperty()
	  {
		SwapLegAmount amount = SwapLegAmount.builder().amount(CurrencyAmount.of(Currency.AUD, 7)).payReceive(PayReceive.PAY).type(SwapLegType.FIXED).currency(Currency.AUD).build();
		LegAmounts amounts = LegAmounts.of(amount);
		BeanTokenEvaluator evaluator = new BeanTokenEvaluator();

		EvaluationResult result1 = evaluator.evaluate(amounts, FUNCTIONS, "amounts", ImmutableList.of("foo", "bar"));
		assertThat(result1.Result).hasValue(ImmutableList.of(amount));
		assertThat(result1.RemainingTokens).isEqualTo(ImmutableList.of("foo", "bar"));

		EvaluationResult result2 = evaluator.evaluate(amounts, FUNCTIONS, "baz", ImmutableList.of("foo", "bar"));
		assertThat(result2.Result).hasValue(ImmutableList.of(amount));
		assertThat(result2.RemainingTokens).isEqualTo(ImmutableList.of("baz", "foo", "bar"));
	  }

	  /// <summary>
	  /// Tests the tokens() method when the bean has a single property. The tokens should include the single property
	  /// name plus the tokens of the property value.
	  /// </summary>
	  public virtual void tokensSingleProperty()
	  {
		SwapLegAmount amount = SwapLegAmount.builder().amount(CurrencyAmount.of(Currency.AUD, 7)).payReceive(PayReceive.PAY).type(SwapLegType.FIXED).currency(Currency.AUD).build();
		LegAmounts amounts = LegAmounts.of(amount);
		BeanTokenEvaluator evaluator = new BeanTokenEvaluator();

		ISet<string> tokens = evaluator.tokens(amounts);
		assertThat(tokens).isEqualTo(ImmutableSet.of("amounts", "0", "aud", "pay", "fixed"));
	  }

	  private static Bean bean()
	  {
		return Fra.builder().buySell(BUY).notional(1_000_000).startDate(date(2015, 8, 5)).endDate(date(2015, 11, 5)).paymentDate(AdjustableDate.of(date(2015, 8, 7))).fixedRate(0.25d).index(GBP_LIBOR_3M).build();
	  }
	}

}