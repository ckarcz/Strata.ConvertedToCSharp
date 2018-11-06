using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.expression
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Messages = com.opengamma.strata.collect.Messages;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;

	/// <summary>
	/// The result of a <seealso cref="TokenEvaluator"/> evaluating an expression against an object.
	/// <para>
	/// The result contains the result of the evaluation and the remaining tokens in the expression.
	/// </para>
	/// </summary>
	public sealed class EvaluationResult
	{

	  /// <summary>
	  /// The result of evaluating the expression against the object.
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final com.opengamma.strata.collect.result.Result<?> result;
	  private readonly Result<object> result;
	  /// <summary>
	  /// The tokens remaining in the expression after evaluation.
	  /// </summary>
	  private readonly IList<string> remainingTokens;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates the result of successfully evaluating a token against an object.
	  /// </summary>
	  /// <param name="value">  the result of evaluating the expression against the object </param>
	  /// <param name="remainingTokens">  the tokens remaining in the expression after evaluation </param>
	  /// <returns> the result of successfully evaluating a token against an object </returns>
	  public static EvaluationResult success(object value, IList<string> remainingTokens)
	  {
		return new EvaluationResult(Result.success(value), remainingTokens);
	  }

	  /// <summary>
	  /// Creates a result for an unsuccessful evaluation of an expression.
	  /// </summary>
	  /// <param name="message">  the error message </param>
	  /// <param name="messageValues">  values substituted into the error message. See <seealso cref="Messages#format(String, Object...)"/>
	  ///   for details </param>
	  /// <returns> the result of an unsuccessful evaluation of an expression </returns>
	  public static EvaluationResult failure(string message, params object[] messageValues)
	  {
		string msg = Messages.format(message, messageValues);
		return new EvaluationResult(Result.failure(FailureReason.INVALID, msg), ImmutableList.of());
	  }

	  /// <summary>
	  /// Creates the result of evaluating a token against an object.
	  /// </summary>
	  /// <param name="result">  the result of evaluating the expression against the object </param>
	  /// <param name="remainingTokens">  the tokens remaining in the expression after evaluation </param>
	  /// <returns> the result of evaluating a token against an object </returns>
	  public static EvaluationResult of<T1>(Result<T1> result, IList<string> remainingTokens)
	  {
		return new EvaluationResult(result, remainingTokens);
	  }

	  // restricted constructor
	  private EvaluationResult<T1>(Result<T1> result, IList<string> remainingTokens)
	  {
		this.result = result;
		this.remainingTokens = remainingTokens;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the result of evaluating the expression against the object.
	  /// </summary>
	  /// <returns> the result of evaluating the expression against the object </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.opengamma.strata.collect.result.Result<?> getResult()
	  public Result<object> Result
	  {
		  get
		  {
			return result;
		  }
	  }

	  /// <summary>
	  /// Returns the tokens remaining in the expression after evaluation.
	  /// </summary>
	  /// <returns> the tokens remaining in the expression after evaluation </returns>
	  public IList<string> RemainingTokens
	  {
		  get
		  {
			return remainingTokens;
		  }
	  }

	  /// <summary>
	  /// Returns true if evaluation of the whole expression is complete.
	  /// <para>
	  /// This occurs if the evaluation failed or all tokens in the expression have been consumed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> returns true if evaluation of the whole expression is complete </returns>
	  public bool Complete
	  {
		  get
		  {
			return Result.Failure || RemainingTokens.Count == 0;
		  }
	  }

	}

}