using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.expression
{

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;

	/// <summary>
	/// Evaluates a token against a currency amount.
	/// </summary>
	public class CurrencyAmountTokenEvaluator : TokenEvaluator<CurrencyAmount>
	{

	  private const string CURRENCY_FIELD = "currency";
	  private const string AMOUNT_FIELD = "amount";

	  public override Type<CurrencyAmount> TargetType
	  {
		  get
		  {
			return typeof(CurrencyAmount);
		  }
	  }

	  public override ImmutableSet<string> tokens(CurrencyAmount amount)
	  {
		return ImmutableSet.of(CURRENCY_FIELD, AMOUNT_FIELD);
	  }

	  public override EvaluationResult evaluate(CurrencyAmount amount, CalculationFunctions functions, string firstToken, IList<string> remainingTokens)
	  {

		if (firstToken.Equals(CURRENCY_FIELD, StringComparison.OrdinalIgnoreCase))
		{
		  return EvaluationResult.success(amount.Currency, remainingTokens);
		}
		if (firstToken.Equals(AMOUNT_FIELD, StringComparison.OrdinalIgnoreCase))
		{
		  // Can be rendered directly - retains the currency for formatting purposes
		  return EvaluationResult.success(amount, remainingTokens);
		}
		return invalidTokenFailure(amount, firstToken);
	  }

	}

}