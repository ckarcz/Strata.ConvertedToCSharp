using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.expression
{

	using Strings = com.google.common.@base.Strings;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;

	/// <summary>
	/// Evaluator that evaluates the first token in the expression.
	/// <para>
	/// The supported values for the first token are enumerated in <seealso cref="ValueRootType"/>.
	/// </para>
	/// </summary>
	internal class RootEvaluator : TokenEvaluator<ResultsRow>
	{

	  /// <summary>
	  /// The single shared instance of this class. </summary>
	  internal static readonly RootEvaluator INSTANCE = new RootEvaluator();

	  private static readonly ImmutableSet<string> TOKENS = ImmutableSet.of(ValueRootType.MEASURES.token(), ValueRootType.PRODUCT.token(), ValueRootType.SECURITY.token(), ValueRootType.TRADE.token(), ValueRootType.POSITION.token(), ValueRootType.TARGET.token());

	  public override Type TargetType
	  {
		  get
		  {
			// This isn't used because the root parser has special treatment
			return typeof(ResultsRow);
		  }
	  }

	  public override ISet<string> tokens(ResultsRow target)
	  {
		return TOKENS;
	  }

	  public override EvaluationResult evaluate(ResultsRow resultsRow, CalculationFunctions functions, string firstToken, IList<string> remainingTokens)
	  {

		ValueRootType rootType = ValueRootType.parseToken(firstToken);
		switch (rootType.innerEnumValue)
		{
		  case com.opengamma.strata.report.framework.expression.ValueRootType.InnerEnum.MEASURES:
			return evaluateMeasures(resultsRow, functions, remainingTokens);
		  case com.opengamma.strata.report.framework.expression.ValueRootType.InnerEnum.PRODUCT:
			return EvaluationResult.of(resultsRow.Product, remainingTokens);
		  case com.opengamma.strata.report.framework.expression.ValueRootType.InnerEnum.SECURITY:
			return EvaluationResult.of(resultsRow.Security, remainingTokens);
		  case com.opengamma.strata.report.framework.expression.ValueRootType.InnerEnum.TRADE:
			return EvaluationResult.of(resultsRow.Trade, remainingTokens);
		  case com.opengamma.strata.report.framework.expression.ValueRootType.InnerEnum.POSITION:
			return EvaluationResult.of(resultsRow.Position, remainingTokens);
		  case com.opengamma.strata.report.framework.expression.ValueRootType.InnerEnum.TARGET:
			return EvaluationResult.success(resultsRow.Target, remainingTokens);
		  default:
			throw new System.ArgumentException("Unknown root token '" + rootType.token() + "'");
		}
	  }

	  // find the result starting from a measure
	  private EvaluationResult evaluateMeasures(ResultsRow resultsRow, CalculationFunctions functions, IList<string> remainingTokens)
	  {

		// if no measures, return list of valid measures
		if (remainingTokens.Count == 0 || Strings.nullToEmpty(remainingTokens[0]).Trim().Empty)
		{
		  IList<string> measureNames = ResultsRow.measureNames(resultsRow.Target, functions);
		  return EvaluationResult.failure("No measure specified. Use one of: {}", measureNames);
		}
		// evaluate the measure name
		string measureToken = remainingTokens[0];
		return EvaluationResult.of(resultsRow.getResult(measureToken), remainingTokens.subList(1, remainingTokens.Count));
	  }

	}

}