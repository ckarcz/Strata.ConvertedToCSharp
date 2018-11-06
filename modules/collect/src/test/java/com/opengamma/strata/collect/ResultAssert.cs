/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect
{
	using AbstractAssert = org.assertj.core.api.AbstractAssert;

	using Failure = com.opengamma.strata.collect.result.Failure;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;

	/// <summary>
	/// An assert helper that provides useful AssertJ assertion
	/// methods for <seealso cref="Result"/> instances.
	/// <para>
	/// These allow {code Result} instances to be inspected in tests in the
	/// same fluent style as other basic classes.
	/// </para>
	/// <para>
	/// So the following:
	/// <pre>
	///   Result{@literal <SomeType>} result = someMethodCall();
	///   assertTrue(result.isSuccess());
	///   assertEquals(result.getValue(), SomeType.EXPECTED);
	/// </pre>
	/// can be replaced with:
	/// <pre>
	///   Result{@literal <SomeType>} result = someMethodCall();
	///   assertThat(result)
	///     .isSuccess()
	///     .hasValue(SomeType.EXPECTED);
	/// </pre>
	/// The advantage of the latter is that if the result was not
	/// a success, the error message produced will detail the
	/// failure result. In the former, the only information is that
	/// the result was not a success. Note that the <seealso cref="#isSuccess()"/>
	/// call in the latter is unnecessary as it will be checked by the
	/// <seealso cref="#hasValue(Object)"/> method as well.
	/// </para>
	/// <para>
	/// In order to be able to use a statically imported assertThat()
	/// method for both {@code Result} and other types, statically
	/// import <seealso cref="CollectProjectAssertions#assertThat(Result)"/>
	/// rather than this class.
	/// </para>
	/// </summary>
	public class ResultAssert : AbstractAssert<ResultAssert, Result<JavaToDotNetGenericWildcard>>
	{

	  /// <summary>
	  /// Create an {@code Assert} instance for the supplied {@code Result}.
	  /// </summary>
	  /// <param name="result">  the result instance to wrap </param>
	  /// <returns> an instance of {@code ResultAssert} </returns>
	  public static ResultAssert assertThat<T1>(Result<T1> result)
	  {
		return new ResultAssert(result);
	  }

	  /// <summary>
	  /// Private constructor, use <seealso cref="#assertThat(Result)"/> to construct an instance.
	  /// </summary>
	  /// <param name="actual">  the instance of {@code Result} to create an {@code Assert} for </param>
	  private ResultAssert<T1>(Result<T1> actual) : base(actual, typeof(ResultAssert))
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Assert that the {@code Result} is a Success.
	  /// </summary>
	  /// <returns> this, if the wrapped object is a success </returns>
	  /// <exception cref="AssertionError"> if the wrapped object is a failure </exception>
	  public virtual ResultAssert Success
	  {
		  get
		  {
			NotNull;
    
			if (!actual.Success)
			{
			  Failure failure = actual.Failure;
			  failWithMessage("Expected Success but was Failure with reason: <%s> and message: <%s>", failure.Reason, failure.Message);
			}
			return this;
		  }
	  }

	  /// <summary>
	  /// Assert that the {@code Result} is a success and contains the specified value.
	  /// </summary>
	  /// <param name="value">  the value the {@code Result} is expected to contain </param>
	  /// <returns> this, if the wrapped object is a success and has the specified value </returns>
	  /// <exception cref="AssertionError"> if the wrapped object is a failure, or does not have the specified value </exception>
	  public virtual ResultAssert hasValue(object value)
	  {
		Success;

		if (!actual.Value.Equals(value))
		{
		  failWithMessage("Expected Success with value: <%s> but was: <%s>", value, actual.Value);
		}
		return this;
	  }

	  /// <summary>
	  /// Assert that the {@code Result} is a Failure.
	  /// </summary>
	  /// <returns> this, if the wrapped object is a failure </returns>
	  /// <exception cref="AssertionError"> if the wrapped object is a success </exception>
	  public virtual ResultAssert Failure
	  {
		  get
		  {
			NotNull;
    
			if (!actual.Failure)
			{
			  failWithMessage("Expected Failure but was Success with value: <%s>", actual.Value);
			}
			return this;
		  }
	  }

	  /// <summary>
	  /// Assert that the {@code Result} is a failure with the specified reason.
	  /// </summary>
	  /// <param name="expected">  the expected failure reason </param>
	  /// <returns> this, if the wrapped object is a failure with the specified reason </returns>
	  /// <exception cref="AssertionError"> if the wrapped object is a success, or does not have the expected reason </exception>
	  public virtual ResultAssert isFailure(FailureReason expected)
	  {
		Failure;

		FailureReason actualReason = actual.Failure.Reason;
		if (actualReason != expected)
		{
		  failWithMessage("Expected Failure with reason: <%s> but was Failure with reason: <%s>", expected, actualReason);
		}
		return this;
	  }

	  /// <summary>
	  /// Assert that the {@code Result} is a failure with the specified message.
	  /// </summary>
	  /// <param name="regex">  the regex that the failure message is expected to match </param>
	  /// <returns> this, if the wrapped object is a failure with the specified message </returns>
	  /// <exception cref="AssertionError"> if the wrapped object is a success, or does not have the expected message </exception>
	  public virtual ResultAssert hasFailureMessageMatching(string regex)
	  {
		Failure;

		string message = actual.Failure.Message;
		if (!message.matches(regex))
		{
		  failWithMessage("Expected Failure with message matching: <%s> but was Failure with message: <%s>", regex, message);
		}
		return this;
	  }

	}

}