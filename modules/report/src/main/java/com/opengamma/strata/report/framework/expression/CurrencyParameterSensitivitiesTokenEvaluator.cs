using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.expression
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableSet;


	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;

	/// <summary>
	/// Evaluates a token against currency parameter sensitivities.
	/// <para>
	/// Tokens are matched against the name and currency code of the sensitivities.
	/// All strings are converted to lower case before matching.
	/// </para>
	/// </summary>
	public class CurrencyParameterSensitivitiesTokenEvaluator : TokenEvaluator<CurrencyParameterSensitivities>
	{

	  public override Type TargetType
	  {
		  get
		  {
			return typeof(CurrencyParameterSensitivities);
		  }
	  }

	  public override ISet<string> tokens(CurrencyParameterSensitivities sensitivities)
	  {
		return sensitivities.Sensitivities.stream().flatMap(this.tokensForSensitivity).collect(toImmutableSet());
	  }

	  public override EvaluationResult evaluate(CurrencyParameterSensitivities sensitivities, CalculationFunctions functions, string firstToken, IList<string> remainingTokens)
	  {

//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<CurrencyParameterSensitivity> matchingSensitivities = sensitivities.Sensitivities.Where(sensitivity => matchesToken(sensitivity, firstToken)).collect(toImmutableList());

		switch (matchingSensitivities.Count)
		{
		  case 0:
			return invalidTokenFailure(sensitivities, firstToken);
		  case 1:
			return EvaluationResult.success(matchingSensitivities[0], remainingTokens);

		  default:
			return EvaluationResult.success(CurrencyParameterSensitivities.of(matchingSensitivities), remainingTokens);
		}
	  }

	  private Stream<string> tokensForSensitivity(CurrencyParameterSensitivity sensitivity)
	  {
		return ImmutableSet.of(sensitivity.Currency.Code, sensitivity.MarketDataName.Name).Select(v => v.ToLower(Locale.ENGLISH));
	  }

	  private bool matchesToken(CurrencyParameterSensitivity sensitivity, string token)
	  {
		return token.Equals(sensitivity.Currency.Code, StringComparison.OrdinalIgnoreCase) || token.Equals(sensitivity.MarketDataName.Name, StringComparison.OrdinalIgnoreCase);
	  }

	}

}