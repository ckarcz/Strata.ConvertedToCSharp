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


	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using Fra = com.opengamma.strata.product.fra.Fra;

	/// <summary>
	/// Evaluates a token against an object to produce another object.
	/// <para>
	/// Tokens are taken from expressions in a report template. These expressions tell the reporting framework
	/// how to navigate a tree of data to find values to include in the report.
	/// </para>
	/// <para>
	/// For example, if the token is '{@code index}' and the object is a <seealso cref="Fra"/>the method {@code Fra.getIndex()}
	/// will be invoked and the result will contain an <seealso cref="IborIndex"/>.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of the target </param>
	public abstract class TokenEvaluator<T>
	{

	  /// <summary>
	  /// Gets the type against which tokens can be evaluated in this implementation.
	  /// </summary>
	  /// <returns> the evaluation type </returns>
	  public abstract Type TargetType {get;}

	  /// <summary>
	  /// Gets the set of supported token for the given object.
	  /// </summary>
	  /// <param name="object">  the object against which tokens may be evaluated </param>
	  /// <returns>  the set of supported tokens </returns>
	  public abstract ISet<string> tokens(T @object);

	  /// <summary>
	  /// Evaluates a token against a given object.
	  /// </summary>
	  /// <param name="target">  the object against which to evaluate the token </param>
	  /// <param name="functions">  the calculation functions </param>
	  /// <param name="firstToken">  the first token of the expression </param>
	  /// <param name="remainingTokens">  the remaining tokens in the expression, possibly empty </param>
	  /// <returns> the result of the evaluation </returns>
	  public abstract EvaluationResult evaluate(T target, CalculationFunctions functions, string firstToken, IList<string> remainingTokens);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Generates a failure result for an invalid token.
	  /// </summary>
	  /// <param name="object">  the object against which the failure occurred </param>
	  /// <param name="token">  the invalid token </param>
	  /// <returns> the failure result </returns>
	  protected internal virtual EvaluationResult invalidTokenFailure(T @object, string token)
	  {
		return tokenFailure("Invalid", @object, token);
	  }

	  /// <summary>
	  /// Generates a failure result for an ambiguous token.
	  /// </summary>
	  /// <param name="object">  the object against which the failure occurred. </param>
	  /// <param name="token">  the ambiguous token </param>
	  /// <returns> the failure result </returns>
	  protected internal virtual EvaluationResult ambiguousTokenFailure(T @object, string token)
	  {
		return tokenFailure("Ambiguous", @object, token);
	  }

	  // produces a failure result
	  private EvaluationResult tokenFailure(string reason, T @object, string token)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<string> orderedValidTokens = tokens(@object).OrderBy(c => c).collect(toImmutableList());
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		return EvaluationResult.failure("{} field '{}' in type {}. Use one of: {}", reason, token, @object.GetType().FullName, orderedValidTokens);
	  }

	}

}