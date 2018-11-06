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


	using Joiner = com.google.common.@base.Joiner;
	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;
	using Fra = com.opengamma.strata.product.fra.Fra;
	using FraTrade = com.opengamma.strata.product.fra.FraTrade;

	/// <summary>
	/// Evaluates a path describing a value to be shown in a trade report.
	/// <para>
	/// For example, if the expression is '{@code Product.index.name}' and the results contain <seealso cref="FraTrade"/> instances
	/// the following calls will be made for each trade in the results:
	/// <ul>
	///   <li>{@code FraTrade.getProduct()} returning a <seealso cref="Fra"/></li>
	///   <li>{@code Fra.getIndex()} returning an <seealso cref="IborIndex"/></li>
	///   <li>{@code IborIndex.getName()} returning the index name</li>
	/// </ul>
	/// The result of evaluating the expression is the index name.
	/// </para>
	/// </summary>
	public sealed class ValuePathEvaluator
	{

	  /// <summary>
	  /// The separator used in the value path. </summary>
	  private const string PATH_SEPARATOR = "\\.";

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.google.common.collect.ImmutableList<TokenEvaluator<?>> EVALUATORS = com.google.common.collect.ImmutableList.of(new CurrencyAmountTokenEvaluator(), new MapTokenEvaluator(), new CurrencyParameterSensitivitiesTokenEvaluator(), new CurrencyParameterSensitivityTokenEvaluator(), new PositionTokenEvaluator(), new TradeTokenEvaluator(), new SecurityTokenEvaluator(), new BeanTokenEvaluator(), new IterableTokenEvaluator());
	  private static readonly ImmutableList<TokenEvaluator<object>> EVALUATORS = ImmutableList.of(new CurrencyAmountTokenEvaluator(), new MapTokenEvaluator(), new CurrencyParameterSensitivitiesTokenEvaluator(), new CurrencyParameterSensitivityTokenEvaluator(), new PositionTokenEvaluator(), new TradeTokenEvaluator(), new SecurityTokenEvaluator(), new BeanTokenEvaluator(), new IterableTokenEvaluator());

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the measure encoded in a value path, if present.
	  /// </summary>
	  /// <param name="valuePath">  the value path </param>
	  /// <returns> the measure, if present </returns>
	  public static Optional<Measure> measure(string valuePath)
	  {
		try
		{
		  IList<string> tokens = tokenize(valuePath);
		  ValueRootType rootType = ValueRootType.parseToken(tokens[0]);

		  if (rootType != ValueRootType.MEASURES || tokens.Count < 2)
		  {
			return null;
		  }
		  Measure measure = Measure.of(tokens[1]);
		  return measure;
		}
		catch (Exception)
		{
		  return null;
		}
	  }

	  /// <summary>
	  /// Evaluates a value path against a set of results, returning the resolved result for each trade.
	  /// </summary>
	  /// <param name="valuePath">  the value path </param>
	  /// <param name="results">  the calculation results </param>
	  /// <returns> the list of resolved results for each trade </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public static java.util.List<com.opengamma.strata.collect.result.Result<?>> evaluate(String valuePath, com.opengamma.strata.report.ReportCalculationResults results)
	  public static IList<Result<object>> evaluate(string valuePath, ReportCalculationResults results)
	  {
		IList<string> tokens = tokenize(valuePath);

		if (tokens.Count < 1)
		{
		  return Collections.nCopies(results.Targets.Count, Result.failure(FailureReason.INVALID, "Column expressions must not be empty"));
		}
		CalculationFunctions functions = results.CalculationFunctions;
		int rowCount = results.CalculationResults.RowCount;
		return IntStream.range(0, rowCount).mapToObj(rowIndex => evaluate(functions, tokens, RootEvaluator.INSTANCE, new ResultsRow(results, rowIndex))).collect(toImmutableList());
	  }

	  // Tokens always has at least one token
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static <T> com.opengamma.strata.collect.result.Result<?> evaluate(com.opengamma.strata.calc.runner.CalculationFunctions functions, java.util.List<String> tokens, TokenEvaluator<T> evaluator, T target)
	  private static Result<object> evaluate<T>(CalculationFunctions functions, IList<string> tokens, TokenEvaluator<T> evaluator, T target)
	  {

		IList<string> remaining = tokens.subList(1, tokens.Count);
		EvaluationResult evaluationResult = evaluator.evaluate(target, functions, tokens[0], remaining);

		if (evaluationResult.Complete)
		{
		  return evaluationResult.Result;
		}
		object value = evaluationResult.Result.Value;
		Optional<TokenEvaluator<object>> nextEvaluator = getEvaluator(value.GetType());

		return nextEvaluator.Present ? evaluate(functions, evaluationResult.RemainingTokens, nextEvaluator.get(), value) : noEvaluatorResult(remaining, value);
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static com.opengamma.strata.collect.result.Result<?> noEvaluatorResult(java.util.List<String> remaining, Object value)
	  private static Result<object> noEvaluatorResult(IList<string> remaining, object value)
	  {
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		return Result.failure(FailureReason.INVALID, "Expression '{}' cannot be invoked on type {}", Joiner.on('.').join(remaining), value.GetType().FullName);
	  }

	  /// <summary>
	  /// Gets the supported tokens on the given object.
	  /// </summary>
	  /// <param name="object">  the object for which to return the valid tokens </param>
	  /// <returns> the tokens </returns>
	  public static ISet<string> tokens(object @object)
	  {
		return getEvaluator(@object.GetType()).map(evaluator => evaluator.tokens(@object)).orElse(ImmutableSet.of());
	  }

	  //-------------------------------------------------------------------------
	  // splits a value path into tokens for processing
	  private static IList<string> tokenize(string valuePath)
	  {
		string[] tokens = valuePath.Split(PATH_SEPARATOR, true);
		return ImmutableList.copyOf(tokens);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") private static java.util.Optional<TokenEvaluator<Object>> getEvaluator(Class targetClass)
	  private static Optional<TokenEvaluator<object>> getEvaluator(Type targetClass)
	  {
		return EVALUATORS.Where(e => targetClass.IsAssignableFrom(e.TargetType)).Select(e => (TokenEvaluator<object>) e).First();
	  }

	  //-------------------------------------------------------------------------
	  private ValuePathEvaluator()
	  {
	  }

	}

}