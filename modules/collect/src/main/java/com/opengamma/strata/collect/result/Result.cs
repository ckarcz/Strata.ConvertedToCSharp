using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.result
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;

	/// <summary>
	/// The result of an operation, either success or failure.
	/// <para>
	/// This provides a functional approach to error handling, that can be used instead of exceptions.
	/// A success result contains a non-null result value.
	/// A failure result contains details of the <seealso cref="Failure failure"/> that occurred.
	/// </para>
	/// <para>
	/// Methods using this approach to error handling are expected to return {@code Result<T>}
	/// and not throw exceptions. The factory method <seealso cref="#of(Supplier)"/> and related methods
	/// can be used to capture exceptions and convert them to failure results.
	/// </para>
	/// <para>
	/// Application code using a result should also operate in a functional style.
	/// Use <seealso cref="#map(Function)"/> and <seealso cref="#flatMap(Function)"/> in preference to
	/// <seealso cref="#isSuccess()"/> and <seealso cref="#getValue()"/>.
	/// <pre>
	///  Result{@literal <Foo>} intermediateResult = calculateIntermediateResult();
	///  return intermediateResult.flatMap(foo -&gt; calculateFinalResult(foo, ...));
	/// </pre>
	/// </para>
	/// <para>
	/// Results can be generated using the factory methods on this class.
	/// 
	/// </para>
	/// </summary>
	/// @param <T> the type of the underlying result for a successful invocation </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class Result<T> implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class Result<T> : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final T value;
		private readonly T value;
	  /// <summary>
	  /// The failure.
	  /// This is only present if the result is a failure.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final Failure failure;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly Failure failure_Renamed;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a successful result wrapping a value.
	  /// <para>
	  /// This returns a successful result object for the non-null value.
	  /// </para>
	  /// <para>
	  /// Note that passing an instance of {@code Failure} to this method would
	  /// be a programming error.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R> the type of the value </param>
	  /// <param name="value">  the result value </param>
	  /// <returns> a successful result wrapping the value </returns>
	  public static Result<R> success<R>(R value)
	  {
		return new Result<R>(ArgChecker.notNull(value, "value"));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a failed result specifying the failure reason.
	  /// <para>
	  /// The message is produced using a template that contains zero to many "{}" placeholders.
	  /// Each placeholder is replaced by the next available argument.
	  /// If there are too few arguments, then the message will be left with placeholders.
	  /// If there are too many arguments, then the excess arguments are appended to the
	  /// end of the message. No attempt is made to format the arguments.
	  /// See <seealso cref="Messages#format(String, Object...)"/> for more details.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R> the expected type of the result </param>
	  /// <param name="reason">  the result reason </param>
	  /// <param name="message">  a message explaining the failure, uses "{}" for inserting {@code messageArgs} </param>
	  /// <param name="messageArgs">  the arguments for the message </param>
	  /// <returns> a failure result </returns>
	  public static Result<R> failure<R>(FailureReason reason, string message, params object[] messageArgs)
	  {
		string msg = Messages.format(message, messageArgs);
		return new Result<R>(Failure.of(FailureItem.of(reason, msg, 1)));
	  }

	  /// <summary>
	  /// Creates a success {@code Result} wrapping the value produced by the
	  /// supplier.
	  /// <para>
	  /// Note that if the supplier throws an exception, this will be caught
	  /// and converted to a failure {@code Result}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T> the type of the value </param>
	  /// <param name="supplier">  supplier of the result value </param>
	  /// <returns> a success {@code Result} wrapping the value produced by the supplier </returns>
	  public static Result<T> of<T>(System.Func<T> supplier)
	  {
		try
		{
		  return success(supplier());
		}
		catch (Exception e)
		{
		  return failure(e);
		}
	  }

	  /// <summary>
	  /// Creates a {@code Result} wrapping the result produced by the supplier.
	  /// <para>
	  /// Note that if the supplier throws an exception, this will be caught
	  /// and converted to a failure {@code Result}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T> the type of the result value </param>
	  /// <param name="supplier">  supplier of the result </param>
	  /// <returns> a {@code Result} produced by the supplier </returns>
	  public static Result<T> wrap<T>(System.Func<Result<T>> supplier)
	  {
		try
		{
		  return supplier();
		}
		catch (Exception e)
		{
		  return failure(e);
		}
	  }

	  /// <summary>
	  /// Creates a failed result caused by an exception.
	  /// <para>
	  /// The failure will have a reason of {@code ERROR}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R> the expected type of the result </param>
	  /// <param name="exception">  the cause of the failure </param>
	  /// <returns> a failure result </returns>
	  public static Result<R> failure<R>(Exception exception)
	  {
		return new Result<R>(Failure.of(FailureReason.ERROR, exception));
	  }

	  /// <summary>
	  /// Creates a failed result caused by an exception.
	  /// <para>
	  /// The failure will have a reason of {@code ERROR}.
	  /// </para>
	  /// <para>
	  /// The message is produced using a template that contains zero to many "{}" placeholders.
	  /// Each placeholder is replaced by the next available argument.
	  /// If there are too few arguments, then the message will be left with placeholders.
	  /// If there are too many arguments, then the excess arguments are appended to the
	  /// end of the message. No attempt is made to format the arguments.
	  /// See <seealso cref="Messages#format(String, Object...)"/> for more details.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R> the expected type of the result </param>
	  /// <param name="exception">  the cause of the failure </param>
	  /// <param name="message">  a message explaining the failure, uses "{}" for inserting {@code messageArgs} </param>
	  /// <param name="messageArgs">  the arguments for the message </param>
	  /// <returns> a failure result </returns>
	  public static Result<R> failure<R>(Exception exception, string message, params object[] messageArgs)
	  {
		return new Result<R>(Failure.of(FailureReason.ERROR, exception, message, messageArgs));
	  }

	  /// <summary>
	  /// Creates a failed result caused by an exception with a specified reason.
	  /// </summary>
	  /// @param <R> the expected type of the result </param>
	  /// <param name="reason">  the result reason </param>
	  /// <param name="exception">  the cause of the failure </param>
	  /// <returns> a failure result </returns>
	  public static Result<R> failure<R>(FailureReason reason, Exception exception)
	  {
		return new Result<R>(Failure.of(reason, exception));
	  }

	  /// <summary>
	  /// Creates a failed result caused by an exception with a specified reason and message.
	  /// <para>
	  /// The message is produced using a template that contains zero to many "{}" placeholders.
	  /// Each placeholder is replaced by the next available argument.
	  /// If there are too few arguments, then the message will be left with placeholders.
	  /// If there are too many arguments, then the excess arguments are appended to the
	  /// end of the message. No attempt is made to format the arguments.
	  /// See <seealso cref="Messages#format(String, Object...)"/> for more details.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R> the expected type of the result </param>
	  /// <param name="reason">  the result reason </param>
	  /// <param name="exception">  the cause of the failure </param>
	  /// <param name="message">  a message explaining the failure, uses "{}" for inserting {@code messageArgs} </param>
	  /// <param name="messageArgs">  the arguments for the message </param>
	  /// <returns> a failure result </returns>
	  public static Result<R> failure<R>(FailureReason reason, Exception exception, string message, params object[] messageArgs)
	  {

		return new Result<R>(Failure.of(reason, exception, message, messageArgs));
	  }

	  /// <summary>
	  /// Returns a failed result from another failed result.
	  /// <para>
	  /// This method ensures the result type matches the expected type.
	  /// If the specified result is a successful result then an exception is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R> the expected result type </param>
	  /// <param name="failureResult">  a failure result </param>
	  /// <returns> a failure result of the expected type </returns>
	  /// <exception cref="IllegalArgumentException"> if the result is a success </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public static <R> Result<R> failure(Result<?> failureResult)
	  public static Result<R> failure<R, T1>(Result<T1> failureResult)
	  {
		if (failureResult.Success)
		{
		  throw new System.ArgumentException("Result must be a failure");
		}
		return (Result<R>) failureResult;
	  }

	  /// <summary>
	  /// Creates a failed result combining multiple failed results.
	  /// <para>
	  /// The input results can be successes or failures, only the failures will be included in the created result.
	  /// Intended to be used with <seealso cref="#anyFailures(Result...)"/>.
	  /// <blockquote><pre>
	  ///   if (Result.anyFailures(result1, result2, result3) {
	  ///     return Result.failure(result1, result2, result3);
	  ///   }
	  /// </pre></blockquote>
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R> the expected type of the result </param>
	  /// <param name="result1">  the first result </param>
	  /// <param name="result2">  the second result </param>
	  /// <param name="results">  the rest of the results </param>
	  /// <returns> a failed result wrapping multiple other failed results </returns>
	  /// <exception cref="IllegalArgumentException"> if all of the results are successes </exception>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public static <R> Result<R> failure(Result<?> result1, Result<?> result2, Result<?>... results)
	  public static Result<R> failure<R, T1, T2>(Result<T1> result1, Result<T2> result2, params Result<object>[] results)
	  {
		ArgChecker.notNull(result1, "result1");
		ArgChecker.notNull(result2, "result2");
		ArgChecker.notNull(results, "results");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.google.common.collect.ImmutableList<Result<?>> list = com.google.common.collect.ImmutableList.builder<Result<?>>().add(result1).add(result2).addAll(java.util.Arrays.asList(results)).build();
		ImmutableList<Result<object>> list = ImmutableList.builder<Result<object>>().add(result1).add(result2).addAll(Arrays.asList(results)).build();
		return failure(list);
	  }

	  /// <summary>
	  /// Creates a failed result combining multiple failed results.
	  /// <para>
	  /// The input results can be successes or failures, only the failures will be included in the created result.
	  /// Intended to be used with <seealso cref="#anyFailures(Iterable)"/>.
	  /// <blockquote><pre>
	  ///   if (Result.anyFailures(results) {
	  ///     return Result.failure(results);
	  ///   }
	  /// </pre></blockquote>
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R> the expected type of the result </param>
	  /// <param name="results">  multiple results, of which at least one must be a failure, not empty </param>
	  /// <returns> a failed result wrapping multiple other failed results </returns>
	  /// <exception cref="IllegalArgumentException"> if results is empty or contains nothing but successes </exception>
	  public static Result<R> failure<R, T1>(IEnumerable<T1> results) where T1 : Result<T1>
	  {
		ArgChecker.notEmpty(results, "results");
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		ImmutableSet<FailureItem> items = Guavate.stream(results).filter(Result::isFailure).map(Result::getFailure).flatMap(f => f.Items.stream()).collect(Guavate.toImmutableSet());
		if (items.Empty)
		{
		  throw new System.ArgumentException("All results were successes");
		}
		return new Result<R>(Failure.of(items));
	  }

	  /// <summary>
	  /// Creates a failed result containing a failure.
	  /// <para>
	  /// This is useful for converting an existing {@code Failure} instance to a result.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R> the expected type of the result </param>
	  /// <param name="failure">  details of the failure </param>
	  /// <returns> a failed result containing the specified failure </returns>
	  public static Result<R> failure<R>(Failure failure)
	  {
		return new Result<R>(failure);
	  }

	  /// <summary>
	  /// Returns a success result containing the value if it is non-null, else returns a failure result
	  /// with the specified reason and message.
	  /// <para>
	  /// This is useful for interoperability with APIs that return {@code null}, for example {@code Map.get()}, where
	  /// a missing value should be treated as a failure.
	  /// </para>
	  /// <para>
	  /// The message is produced using a template that contains zero to many "{}" placeholders.
	  /// Each placeholder is replaced by the next available argument.
	  /// If there are too few arguments, then the message will be left with placeholders.
	  /// If there are too many arguments, then the excess arguments are appended to the
	  /// end of the message. No attempt is made to format the arguments.
	  /// See <seealso cref="Messages#format(String, Object...)"/> for more details.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R> the expected type of the result </param>
	  /// <param name="value">  the potentially null value </param>
	  /// <param name="reason">  the reason for the failure </param>
	  /// <param name="message">  a message explaining the failure, uses "{}" for inserting {@code messageArgs} </param>
	  /// <param name="messageArgs">  the arguments for the message </param>
	  /// <returns> a success result if the value is non-null, else a failure result </returns>
	  public static Result<R> ofNullable<R>(R value, FailureReason reason, string message, params object[] messageArgs)
	  {

		if (value != default(R))
		{
		  return success(value);
		}
		else
		{
		  return failure(reason, message, messageArgs);
		}
	  }

	  /// <summary>
	  /// Returns a success result containing the value if it is non-null, else returns a failure result
	  /// with a reason of <seealso cref="FailureReason#MISSING_DATA"/> and message to say an unexpected null was found.
	  /// <para>
	  /// This is useful for interoperability with APIs that can return {@code null} but where null is not expected.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R> the expected type of the result </param>
	  /// <param name="value">  the potentially null value </param>
	  /// <returns> a success result if the value is non-null, else a failure result </returns>
	  public static Result<R> ofNullable<R>(R value)
	  {
		return ofNullable(value, FailureReason.MISSING_DATA, "Found null where a value was expected");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if all the results are successful.
	  /// </summary>
	  /// <param name="results">  the results to check </param>
	  /// <returns> true if all of the results are successes </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public static boolean allSuccessful(Result<?>... results)
	  public static bool allSuccessful(params Result<object>[] results)
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		return Stream.of(results).allMatch(Result::isSuccess);
	  }

	  /// <summary>
	  /// Checks if all the results are successful.
	  /// </summary>
	  /// <param name="results">  the results to check </param>
	  /// <returns> true if all of the results are successes </returns>
	  public static bool allSuccessful<T1>(IEnumerable<T1> results) where T1 : Result<T1>
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		return Guavate.stream(results).allMatch(Result::isSuccess);
	  }

	  /// <summary>
	  /// Checks if any of the results are failures.
	  /// </summary>
	  /// <param name="results">  the results to check </param>
	  /// <returns> true if any of the results are failures </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public static boolean anyFailures(Result<?>... results)
	  public static bool anyFailures(params Result<object>[] results)
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		return Stream.of(results).anyMatch(Result::isFailure);
	  }

	  /// <summary>
	  /// Checks if any of the results are failures.
	  /// </summary>
	  /// <param name="results">  the results to check </param>
	  /// <returns> true if any of the results are failures </returns>
	  public static bool anyFailures<T1>(IEnumerable<T1> results) where T1 : Result<T1>
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		return Guavate.stream(results).anyMatch(Result::isFailure);
	  }

	  /// <summary>
	  /// Counts how many of the results are failures.
	  /// </summary>
	  /// <param name="results">  the results to check </param>
	  /// <returns> the number of results that are failures </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public static long countFailures(Result<?>... results)
	  public static long countFailures(params Result<object>[] results)
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		return Stream.of(results).filter(Result::isFailure).count();
	  }

	  /// <summary>
	  /// Counts how many of the results are failures.
	  /// </summary>
	  /// <param name="results">  the results to check </param>
	  /// <returns> the number of results that are failures </returns>
	  public static long countFailures<T1>(IEnumerable<T1> results) where T1 : Result<T1>
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		return Guavate.stream(results).filter(Result::isFailure).count();
	  }

	  /// <summary>
	  /// Takes a collection of results, checks if all of them are successes
	  /// and then applies the supplied function to the successes wrapping
	  /// the result in a success result. If any of the initial results was
	  /// a failure, then a failure result reflecting the failures in the
	  /// initial results is returned.
	  /// <para>
	  /// If an exception is thrown when the function is applied, this will
	  /// be caught and a failure {@code Result} returned.
	  /// </para>
	  /// <para>
	  /// The following code shows where this method can be used. The code:
	  /// <blockquote><pre>
	  ///   Set&lt;Result&lt;MyData&gt;&gt; results = goAndGatherData();
	  ///   if (Result.anyFailures(results)) {
	  ///     return Result.failure(results);
	  ///   } else {
	  ///     Set&lt;FooData&gt; combined =
	  ///         results.stream()
	  ///             .map(Result::getValue)
	  ///             .map(MyData::transformToFoo)
	  ///             .collect(toSet());
	  ///     return Result.success(combined);
	  ///   }
	  /// </pre></blockquote>
	  /// can be replaced with:
	  /// <blockquote><pre>
	  ///   Set&lt;Result&lt;MyData&gt;&gt; results = goAndGatherData();
	  ///   return Result.combine(results, myDataStream -&gt;
	  ///       myDataStream
	  ///           .map(MyData::transformToFoo)
	  ///           .collect(toSet())
	  ///   );
	  /// </pre></blockquote>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="results">  the results to be transformed if they are all successes </param>
	  /// <param name="function">  the function to apply to the stream of results if they were all successes </param>
	  /// @param <T>  the type of the values held in the input results </param>
	  /// @param <R>  the type of the values held in the transformed results </param>
	  /// <returns> a success result holding the result of applying the function to the
	  ///   input results if they were all successes, a failure otherwise </returns>
	  public static Result<R> combine<T, R, T1>(IEnumerable<T1> results, System.Func<Stream<T>, R> function) where T1 : Result<T>
	  {

		try
		{
		  return allSuccessful(results) ? success(function(extractSuccesses(results))) : failure(results);

		}
		catch (Exception e)
		{
		  return failure(e);
		}
	  }

	  /// <summary>
	  /// Takes a collection of results, checks if all of them are successes
	  /// and then applies the supplied function to the successes. If any of
	  /// the initial results was a failure, then a failure result reflecting
	  /// the failures in the initial results is returned.
	  /// <para>
	  /// If an exception is thrown when the function is applied, this will
	  /// be caught and a failure {@code Result} returned.
	  /// </para>
	  /// <para>
	  /// The following code shows where this method can be used. The code:
	  /// <blockquote><pre>
	  ///   Set&lt;Result&lt;MyData&gt;&gt; results = goAndGatherData();
	  ///   if (Result.anyFailures(results)) {
	  ///     return Result.failure(results);
	  ///   } else {
	  ///     Set&lt;FooData&gt; combined =
	  ///         results.stream()
	  ///             .map(Result::getValue)
	  ///             .map(MyData::transformToFoo)
	  ///             .collect(toSet());
	  ///     return doSomethingReturningResult(combined); // this could fail
	  ///   }
	  /// </pre></blockquote>
	  /// can be replaced with:
	  /// <blockquote><pre>
	  ///   Set&lt;Result&lt;MyData&gt;&gt; results = goAndGatherData();
	  ///   return Result.flatCombine(results, myDataStream -&gt; {
	  ///     Set&lt;CombinedData&gt; combined =
	  ///         myDataStream
	  ///             .map(MyData::transformToFoo)
	  ///             .collect(toSet());
	  ///     return doSomethingReturningResult(combined); // this could fail
	  ///   });
	  /// </pre></blockquote>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="results">  the results to be transformed if they are all successes </param>
	  /// <param name="function">  the function to apply to the stream of results if they were all successes </param>
	  /// @param <T>  the type of the values held in the input results </param>
	  /// @param <R>  the type of the values held in the transformed results </param>
	  /// <returns> a result holding the result of applying the function to the
	  ///   input results if they were all successes, a failure otherwise </returns>
	  public static Result<R> flatCombine<T, R, T1>(IEnumerable<T1> results, System.Func<Stream<T>, Result<R>> function) where T1 : Result<T>
	  {

		try
		{
		  return allSuccessful(results) ? function(extractSuccesses(results)) : failure(results);

		}
		catch (Exception e)
		{
		  return failure(e);
		}
	  }

	  private static Stream<T> extractSuccesses<T, T1>(IEnumerable<T1> results) where T1 : Result<T>
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		return Guavate.stream(results).map(Result::getValue);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="value">  the value to create from </param>
	  private Result(T value)
	  {
		this.value = value;
		this.failure_Renamed = null;
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="failure">  the failure to create from </param>
	  private Result(Failure failure)
	  {
		this.value = default(T);
		this.failure_Renamed = failure;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (value == default(T) && failure_Renamed == null)
		{
		  throw new System.ArgumentException("Both value and failure are null");
		}
		if (value != default(T) && failure_Renamed != null)
		{
		  throw new System.ArgumentException("Both value and failure are non-null");
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Indicates if this result represents a successful call and has a result available.
	  /// <para>
	  /// This is the opposite of <seealso cref="#isFailure()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if the result represents a success and a value is available </returns>
	  public bool Success
	  {
		  get
		  {
			return value != default(T);
		  }
	  }

	  /// <summary>
	  /// Indicates if this result represents a failure.
	  /// <para>
	  /// This is the opposite of <seealso cref="#isSuccess()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if the result represents a failure </returns>
	  public bool Failure
	  {
		  get
		  {
			return failure_Renamed != null;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the actual result value if calculated successfully, throwing an
	  /// exception if a failure occurred.
	  /// <para>
	  /// If this result is a failure then an {@code IllegalStateException} will be thrown.
	  /// To avoid this, call <seealso cref="#isSuccess()"/> or <seealso cref="#isFailure()"/> first.
	  /// </para>
	  /// <para>
	  /// Application code is recommended to use <seealso cref="#map(Function)"/> and
	  /// <seealso cref="#flatMap(Function)"/> in preference to this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the result value, only available if calculated successfully </returns>
	  /// <exception cref="IllegalStateException"> if called on a failure result </exception>
	  public T Value
	  {
		  get
		  {
			if (Failure)
			{
			  throw new System.InvalidOperationException("Unable to get a value from a failure result: " + Failure.Message);
			}
			return value;
		  }
	  }

	  /// <summary>
	  /// Returns the actual result value if calculated successfully, or the specified
	  /// default value if a failure occurred.
	  /// <para>
	  /// If this result is a success then the result value is returned.
	  /// If this result is a failure then the default value is returned.
	  /// The default value must not be null.
	  /// </para>
	  /// <para>
	  /// Application code is recommended to use <seealso cref="#map(Function)"/> and
	  /// <seealso cref="#flatMap(Function)"/> in preference to this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="defaultValue">  the default value to return if the result is a failure </param>
	  /// <returns> either the result value or the default value </returns>
	  public T getValueOrElse(T defaultValue)
	  {
		ArgChecker.notNull(defaultValue, "defaultValue");
		return (Success ? value : defaultValue);
	  }

	  /// <summary>
	  /// Returns the actual result value if calculated successfully, else the
	  /// specified function is applied to the {@code Failure} that occurred.
	  /// <para>
	  /// If this result is a success then the result value is returned.
	  /// If this result is a failure then the function is applied to the failure.
	  /// The function must not be null.
	  /// </para>
	  /// <para>
	  /// This method can be used in preference to <seealso cref="#getValueOrElse(Object)"/>
	  /// when the default value is expensive to create. In such cases, the
	  /// default value will get created on each call, even though it will be
	  /// immediately discarded if the result is a success.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="mapper">  function used to generate a default value. The function
	  ///   has no obligation to use the input {@code Failure} (in other words it can
	  ///   behave as a {@code Supplier<T>} if desired). </param>
	  /// <returns> either the result value or the result of the function </returns>
	  public T getValueOrElseApply(System.Func<Failure, T> mapper)
	  {
		ArgChecker.notNull(mapper, "mapper");
		return (Success ? value : mapper(failure_Renamed));
	  }

	  /// <summary>
	  /// Returns the failure instance indicating the reason why the calculation failed.
	  /// <para>
	  /// If this result is a success then an an IllegalStateException will be thrown.
	  /// To avoid this, call <seealso cref="#isSuccess()"/> or <seealso cref="#isFailure()"/> first.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the details of the failure, only available if calculation failed </returns>
	  /// <exception cref="IllegalStateException"> if called on a success result </exception>
	  public Failure Failure
	  {
		  get
		  {
			if (Success)
			{
			  throw new System.InvalidOperationException("Unable to get a failure from a success result");
			}
			return failure_Renamed;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Processes a successful result by applying a function that alters the value.
	  /// <para>
	  /// This operation allows post-processing of a result value.
	  /// The specified function represents a conversion to be performed on the value.
	  /// </para>
	  /// <para>
	  /// If this result is a success, then the specified function is invoked.
	  /// The return value of the specified function is returned to the caller
	  /// wrapped in a success result. If an exception is thrown when the function
	  /// is invoked, this will be caught and a failure {@code Result} returned.
	  /// </para>
	  /// <para>
	  /// If this result is a failure, then {@code this} is returned.
	  /// The specified function is not invoked.
	  /// </para>
	  /// <para>
	  /// For example, it allows a {@code double} to be converted to a string:
	  /// <blockquote><pre>
	  ///   result = ...
	  ///   return result.map(value -&gt; Double.toString(value));
	  /// </pre></blockquote>
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R>  the type of the value in the returned result </param>
	  /// <param name="function">  the function to transform the value with </param>
	  /// <returns> the new result </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public <R> Result<R> map(java.util.function.Function<? super T, ? extends R> function)
	  public Result<R> map<R, T1>(System.Func<T1> function) where T1 : R
	  {
		if (Success)
		{
		  try
		  {
			return Result.success(function(value));
		  }
		  catch (Exception e)
		  {
			return Result.failure(e);
		  }
		}
		else
		{
		  return Result.failure(this);
		}
	  }

	  /// <summary>
	  /// Processes a successful result by applying a function that returns another result.
	  /// <para>
	  /// This operation allows chaining of function calls that produce a result.
	  /// The specified function will typically call another method that returns a result.
	  /// </para>
	  /// <para>
	  /// If this result is a success, then the specified function is invoked.
	  /// The return value of the specified function is returned to the caller and may be
	  /// a success or failure. If an exception is thrown when the function
	  /// is invoked, this will be caught and a failure {@code Result} returned.
	  /// </para>
	  /// <para>
	  /// If this result is a failure, then an equivalent failure is returned.
	  /// The specified function is not invoked.
	  /// </para>
	  /// <para>
	  /// For example,
	  /// <blockquote><pre>
	  ///   result = ...
	  ///   return result.flatMap(value -&gt; doSomething(value));
	  /// </pre></blockquote>
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R>  the type of the value in the returned result </param>
	  /// <param name="function">  the function to transform the value with </param>
	  /// <returns> the new result </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public <R> Result<R> flatMap(java.util.function.Function<? super T, Result<R>> function)
	  public Result<R> flatMap<R, T1>(System.Func<T1> function)
	  {
		if (Success)
		{
		  try
		  {
			return Objects.requireNonNull(function(value));
		  }
		  catch (Exception e)
		  {
			return failure(e);
		  }
		}
		else
		{
		  return Result.failure(this);
		}
	  }

	  /// <summary>
	  /// Combines this result with another result.
	  /// <para>
	  /// This operation allows two results to be combined handling succeess and failure.
	  /// </para>
	  /// <para>
	  /// If both results are a success, then the specified function is invoked to combine them.
	  /// The return value of the specified function is returned to the caller and may be
	  /// a success or failure.
	  /// </para>
	  /// <para>
	  /// If either result is a failure, then a combination failure is returned.
	  /// The specified function is not invoked.
	  /// <blockquote><pre>
	  ///   result1 = ...
	  ///   result2 = ...
	  ///   return result1.combineWith(result2, (value1, value2) -&gt; doSomething(value1, value2));
	  /// </pre></blockquote>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  another result </param>
	  /// <param name="function">  a function for combining values from two results </param>
	  /// @param <U>  the type of the value in the other result </param>
	  /// @param <R>  the type of the value in the returned result </param>
	  /// <returns> a the result of combining the result values or a failure if either result is a failure </returns>
	  public Result<R> combineWith<U, R>(Result<U> other, System.Func<T, U, Result<R>> function)
	  {
		ArgChecker.notNull(other, "other");
		ArgChecker.notNull(function, "function");
		if (Success && other.Success)
		{
		  try
		  {
			return Objects.requireNonNull(function(value, other.value));
		  }
		  catch (Exception e)
		  {
			return failure(e);
		  }
		}
		else
		{
		  return failure(this, other);
		}
	  }

	  /// <summary>
	  /// Converts this result to a stream.
	  /// <para>
	  /// If this result is a success then a single element stream containing the result value is returned.
	  /// If this result is a failure then an empty stream is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a stream of size one or zero </returns>
	  public Stream<T> stream()
	  {
		return (Success ? Stream.of(value) : Stream.empty());
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code Result}. </summary>
	  /// <returns> the meta-bean, not null </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("rawtypes") public static Result.Meta meta()
	  public static Result.Meta meta()
	  {
		return Result.Meta.INSTANCE;
	  }

	  /// <summary>
	  /// The meta-bean for {@code Result}. </summary>
	  /// @param <R>  the bean's generic type </param>
	  /// <param name="cls">  the bean's generic type </param>
	  /// <returns> the meta-bean, not null </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public static <R> Result.Meta<R> metaResult(Class<R> cls)
	  public static Result.Meta<R> metaResult<R>(Type<R> cls)
	  {
		return Result.Meta.INSTANCE;
	  }

	  static Result()
	  {
		MetaBean.register(Result.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private Result(T value, Failure failure)
	  {
		this.value = value;
		this.failure_Renamed = failure;
		validate();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Result.Meta<T> metaBean()
	  public override Result.Meta<T> metaBean()
	  {
		return Result.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: Result<?> other = (Result<?>) obj;
		  Result<object> other = (Result<object>) obj;
		  return JodaBeanUtils.equal(value, other.value) && JodaBeanUtils.equal(failure_Renamed, other.failure_Renamed);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(value);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(failure_Renamed);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("Result{");
		buf.Append("value").Append('=').Append(value).Append(',').Append(' ');
		buf.Append("failure").Append('=').Append(JodaBeanUtils.ToString(failure_Renamed));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code Result}. </summary>
	  /// @param <T>  the type </param>
	  public sealed class Meta<T> : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  value_Renamed = (DirectMetaProperty) DirectMetaProperty.ofImmutable(this, "value", typeof(Result), typeof(object));
			  failure_Renamed = DirectMetaProperty.ofImmutable(this, "failure", typeof(Result), typeof(Failure));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "value", "failure");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("rawtypes") static final Meta INSTANCE = new Meta();
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code value} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<T> value = (org.joda.beans.impl.direct.DirectMetaProperty) org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "value", Result.class, Object.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<T> value_Renamed;
		/// <summary>
		/// The meta-property for the {@code failure} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Failure> failure_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "value", "failure");
		internal IDictionary<string, MetaProperty<object>> metaPropertyMap$;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Meta()
		{
			if (!InstanceFieldsInitialized)
			{
				InitializeInstanceFields();
				InstanceFieldsInitialized = true;
			}
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override protected org.joda.beans.MetaProperty<?> metaPropertyGet(String propertyName)
		protected internal override MetaProperty<object> metaPropertyGet(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 111972721: // value
			  return value_Renamed;
			case -1086574198: // failure
			  return failure_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends Result<T>> builder()
		public override BeanBuilder<Result<T>> builder()
		{
		  return new Result.Builder<>();
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) @Override public Class beanType()
		public override Type beanType()
		{
		  return (Type) typeof(Result);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code value} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<T> value()
		{
		  return value_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code failure} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Failure> failure()
		{
		  return failure_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 111972721: // value
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return ((Result<?>) bean).value;
			  return ((Result<object>) bean).value;
			case -1086574198: // failure
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return ((Result<?>) bean).failure;
			  return ((Result<object>) bean).failure;
		  }
		  return base.propertyGet(bean, propertyName, quiet);
		}

		protected internal override void propertySet(Bean bean, string propertyName, object newValue, bool quiet)
		{
		  metaProperty(propertyName);
		  if (quiet)
		  {
			return;
		  }
		  throw new System.NotSupportedException("Property cannot be written: " + propertyName);
		}

	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The bean-builder for {@code Result}. </summary>
	  /// @param <T>  the type </param>
	  private sealed class Builder<T> : DirectPrivateBeanBuilder<Result<T>>
	  {

		internal T value;
		internal Failure failure;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 111972721: // value
			  return value;
			case -1086574198: // failure
			  return failure;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Builder<T> set(String propertyName, Object newValue)
		public override Builder<T> set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 111972721: // value
			  this.value = (T) newValue;
			  break;
			case -1086574198: // failure
			  this.failure = (Failure) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override Result<T> build()
		{
		  return new Result<T>(value, failure);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("Result.Builder{");
		  buf.Append("value").Append('=').Append(JodaBeanUtils.ToString(value)).Append(',').Append(' ');
		  buf.Append("failure").Append('=').Append(JodaBeanUtils.ToString(failure));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}