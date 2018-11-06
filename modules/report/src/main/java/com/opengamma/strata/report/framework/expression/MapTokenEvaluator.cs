using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.expression
{

	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using MapStream = com.opengamma.strata.collect.MapStream;

	/// <summary>
	/// Evaluates a token against a map.
	/// </summary>
	public class MapTokenEvaluator : TokenEvaluator<IDictionary<JavaToDotNetGenericWildcard, JavaToDotNetGenericWildcard>>
	{

	  public override Type TargetType
	  {
		  get
		  {
			return typeof(System.Collections.IDictionary);
		  }
	  }

	  public override ISet<string> tokens<T1>(IDictionary<T1> map)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return map.Keys.Select(k => k.ToString().ToLower(Locale.ENGLISH)).collect(Collectors.toSet());
	  }

	  public override EvaluationResult evaluate<T1>(IDictionary<T1> map, CalculationFunctions functions, string firstToken, IList<string> remainingTokens)
	  {

		return MapStream.of(map).filterKeys(key => firstToken.Equals(key.ToString(), StringComparison.OrdinalIgnoreCase)).findFirst().map(e => EvaluationResult.success(e.Value, remainingTokens)).orElse(invalidTokenFailure(map, firstToken));
	  }
	}

}