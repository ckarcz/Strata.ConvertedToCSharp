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
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;

	/// <summary>
	/// Token evaluator for currency parameter sensitivity.
	/// <para>
	/// Although there is a formatter for this type, users will traverse to a single sensitivity from
	/// a list of sensitivities. This traversal may include redundant tokens, so the purpose of this
	/// evaluator is to continue returning the same sensitivity object as long as the tokens are
	/// consistent with the fields on this object.
	/// </para>
	/// </summary>
	public class CurrencyParameterSensitivityTokenEvaluator : TokenEvaluator<CurrencyParameterSensitivity>
	{

	  public override Type TargetType
	  {
		  get
		  {
			return typeof(CurrencyParameterSensitivity);
		  }
	  }

	  public override ISet<string> tokens(CurrencyParameterSensitivity sensitivity)
	  {
		return ImmutableSet.of(sensitivity.Currency.Code.ToLower(Locale.ENGLISH), sensitivity.MarketDataName.Name.ToLower(Locale.ENGLISH));
	  }

	  public override EvaluationResult evaluate(CurrencyParameterSensitivity sensitivity, CalculationFunctions functions, string firstToken, IList<string> remainingTokens)
	  {

		if (firstToken.Equals(sensitivity.Currency.Code, StringComparison.OrdinalIgnoreCase) || firstToken.Equals(sensitivity.MarketDataName.Name, StringComparison.OrdinalIgnoreCase))
		{
		  return EvaluationResult.success(sensitivity, remainingTokens);
		}
		else
		{
		  return invalidTokenFailure(sensitivity, firstToken);
		}
	  }

	}

}