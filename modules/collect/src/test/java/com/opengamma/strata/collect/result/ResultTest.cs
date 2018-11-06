using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.result
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.CollectProjectAssertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.result.FailureReason.CALCULATION_FAILED;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.result.FailureReason.ERROR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.result.FailureReason.MISSING_DATA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertSame;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using Throwables = com.google.common.@base.Throwables;
	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResultTest
	public class ResultTest
	{

	  private static readonly System.Func<string, int> MAP_STRLEN = string.length;

	  private static readonly System.Func<string, Result<int>> FUNCTION_STRLEN = input => Result.success(input.length());
	  private static readonly System.Func<string, string, Result<string>> FUNCTION_MERGE = (t, u) => Result.success(t + " " + u);

	  //-------------------------------------------------------------------------
	  public virtual void success()
	  {
		Result<string> test = Result.success("success");
		assertEquals(test.Success, true);
		assertEquals(test.Failure, false);
		assertEquals(test.Value, "success");
		assertEquals(test.getValueOrElse("blue"), "success");
		assertThrowsIllegalArg(() => test.getValueOrElse(null));
		assertThrowsIllegalArg(() => test.getValueOrElseApply(null));
	  }

	  public virtual void success_getFailure()
	  {
		Result<string> test = Result.success("success");
		assertThrows(test.getFailure, typeof(System.InvalidOperationException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void success_map()
	  {
		Result<string> success = Result.success("success");
		Result<int> test = success.map(MAP_STRLEN);
		assertEquals(test.Success, true);
		assertEquals(test.Value, Convert.ToInt32(7));
	  }

	  public virtual void success_flatMap()
	  {
		Result<string> success = Result.success("success");
		Result<int> test = success.flatMap(FUNCTION_STRLEN);
		assertEquals(test.Success, true);
		assertEquals(test.Value, Convert.ToInt32(7));
	  }

	  public virtual void success_combineWith_success()
	  {
		Result<string> success1 = Result.success("Hello");
		Result<string> success2 = Result.success("World");
		Result<string> test = success1.combineWith(success2, FUNCTION_MERGE);
		assertEquals(test.Success, true);
		assertEquals(test.Value, "Hello World");
	  }

	  public virtual void success_combineWith_failure()
	  {
		Result<string> success = Result.success("Hello");
		Result<string> failure = Result.failure(new System.ArgumentException());
		Result<string> test = success.combineWith(failure, FUNCTION_MERGE);
		assertEquals(test.Success, false);
		assertEquals(test.Failure.Reason, ERROR);
		assertEquals(test.Failure.Items.size(), 1);
	  }

	  public virtual void success_combineWith_success_throws()
	  {
		Result<string> success1 = Result.success("Hello");
		Result<string> success2 = Result.success("World");
		Result<string> test = success1.combineWith(success2, (s1, s2) =>
		{
		throw new System.ArgumentException("Ooops");
		});
		assertThat(test).Failure.hasFailureMessageMatching("Ooops");
	  }

	  public virtual void success_stream()
	  {
		Result<string> success = Result.success("Hello");
		assertThat(success.ToArray()).containsExactly("Hello");
	  }

	  public virtual void success_map_throwing()
	  {
		Result<string> success = Result.success("success");
		Result<int> test = success.map(r =>
		{
		throw new System.ArgumentException("Big bad error");
		});
		assertEquals(test.Success, false);
		assertThat(test).isFailure(ERROR).hasFailureMessageMatching("Big bad error");
	  }

	  public virtual void success_flatMap_throwing()
	  {
		Result<string> success = Result.success("success");
		Result<int> test = success.flatMap(r =>
		{
		throw new System.ArgumentException("Big bad error");
		});
		assertEquals(test.Success, false);
		assertThat(test).isFailure(ERROR).hasFailureMessageMatching("Big bad error");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void failure()
	  {
		System.ArgumentException ex = new System.ArgumentException("failure");
		Result<string> test = Result.failure(ex);
		assertEquals(test.Success, false);
		assertEquals(test.Failure, true);
		assertEquals(test.getValueOrElse("blue"), "blue");
		assertEquals(test.getValueOrElseApply(f => "blue"), "blue");
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		assertEquals(test.getValueOrElseApply(Failure::getMessage), "failure");
		assertThrowsIllegalArg(() => test.getValueOrElse(null));
		assertThrowsIllegalArg(() => test.getValueOrElseApply(null));
		assertEquals(test.Failure.Reason, ERROR);
		assertEquals(test.Failure.Message, "failure");
		assertEquals(test.Failure.Items.size(), 1);
		FailureItem item = test.Failure.Items.GetEnumerator().next();
		assertEquals(item.Reason, ERROR);
		assertEquals(item.Message, "failure");
		assertEquals(item.CauseType.get(), ex.GetType());
		assertEquals(item.StackTrace, Throwables.getStackTraceAsString(ex).replace(Environment.NewLine, "\n"));
	  }

	  public virtual void failure_map_flatMap_ifSuccess()
	  {
		Result<string> test = Result.failure(new System.ArgumentException("failure"));
		Result<int> test1 = test.map(MAP_STRLEN);
		assertSame(test1, test);
		Result<int> test2 = test.flatMap(FUNCTION_STRLEN);
		assertSame(test2, test);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalStateException.class) public void failure_getValue()
	  public virtual void failure_getValue()
	  {
		Result<string> test = Result.failure(new System.ArgumentException());
		test.Value;
	  }

	  public virtual void failure_combineWith_success()
	  {
		Result<string> failure = Result.failure(new System.ArgumentException("failure"));
		Result<string> success = Result.success("World");
		Result<string> test = failure.combineWith(success, FUNCTION_MERGE);
		assertEquals(test.Failure.Reason, ERROR);
		assertEquals(test.Failure.Message, "failure");
	  }

	  public virtual void failure_combineWith_failure()
	  {
		Result<string> failure1 = Result.failure(new System.ArgumentException("failure"));
		Result<string> failure2 = Result.failure(new System.ArgumentException("fail"));
		Result<string> test = failure1.combineWith(failure2, FUNCTION_MERGE);
		assertEquals(test.Failure.Reason, ERROR);
		assertEquals(test.Failure.Message, "failure, fail");
	  }

	  public virtual void failure_stream()
	  {
		Result<string> failure = Result.failure(new System.ArgumentException("failure"));
		assertThat(failure.ToArray()).Empty;
	  }

	  public virtual void failure_map_throwing()
	  {
		Result<string> success = Result.failure(new System.ArgumentException("failure"));
		Result<int> test = success.map(r =>
		{
		throw new System.ArgumentException("Big bad error");
		});
		assertEquals(test.Success, false);
		assertEquals(test.Failure.Reason, ERROR);
		assertEquals(test.Failure.Message, "failure");
	  }

	  public virtual void failure_flatMap_throwing()
	  {
		Result<string> success = Result.failure(new System.ArgumentException("failure"));
		Result<int> test = success.flatMap(r =>
		{
		throw new System.ArgumentException("Big bad error");
		});
		assertEquals(test.Success, false);
		assertEquals(test.Failure.Reason, ERROR);
		assertEquals(test.Failure.Message, "failure");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void failure_fromStatusMessageArgs_placeholdersMatchArgs1()
	  {
		Result<string> failure = Result.failure(ERROR, "my {} failure", "blue");
		Result<int> test = Result.failure(failure);
		assertTrue(test.Failure);
		assertEquals(test.Failure.Message, "my blue failure");
	  }

	  public virtual void failure_fromStatusMessageArgs_placeholdersMatchArgs2()
	  {
		Result<string> failure = Result.failure(ERROR, "my {} {} failure", "blue", "rabbit");
		Result<int> test = Result.failure(failure);
		assertTrue(test.Failure);
		assertEquals(test.Failure.Message, "my blue rabbit failure");
	  }

	  public virtual void failure_fromStatusMessageArgs_placeholdersExceedArgs()
	  {
		Result<string> failure = Result.failure(ERROR, "my {} {} failure", "blue");
		Result<int> test = Result.failure(failure);
		assertTrue(test.Failure);
		assertEquals(test.Failure.Message, "my blue {} failure");
	  }

	  public virtual void failure_fromStatusMessageArgs_placeholdersLessThanArgs1()
	  {
		Result<string> failure = Result.failure(ERROR, "my {} failure", "blue", "rabbit");
		Result<int> test = Result.failure(failure);
		assertTrue(test.Failure);
		assertEquals(test.Failure.Message, "my blue failure - [rabbit]");
	  }

	  public virtual void failure_fromStatusMessageArgs_placeholdersLessThanArgs2()
	  {
		Result<string> failure = Result.failure(ERROR, "my {} failure", "blue", "rabbit", "carrot");
		Result<int> test = Result.failure(failure);
		assertTrue(test.Failure);
		assertEquals(test.Failure.Message, "my blue failure - [rabbit, carrot]");
	  }

	  public virtual void failure_fromStatusMessageArgs_placeholdersLessThanArgs3()
	  {
		Result<string> failure = Result.failure(ERROR, "my failure", "blue", "rabbit", "carrot");
		Result<int> test = Result.failure(failure);
		assertTrue(test.Failure);
		assertEquals(test.Failure.Message, "my failure - [blue, rabbit, carrot]");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void failure_fromResult_failure()
	  {
		Result<string> failure = Result.failure(ERROR, "my failure");
		Result<int> test = Result.failure(failure);
		assertTrue(test.Failure);
		assertEquals(test.Failure.Message, "my failure");
		assertEquals(test.Failure.Items.size(), 1);
		FailureItem item = test.Failure.Items.GetEnumerator().next();
		assertEquals(item.Reason, ERROR);
		assertEquals(item.Message, "my failure");
		assertEquals(item.CauseType.Present, false);
		assertEquals(item.StackTrace.Contains(".FailureItem.of("), false);
		assertEquals(item.StackTrace.Contains(".Failure.of("), false);
		assertEquals(item.StackTrace.Contains(".Result.failure("), false);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void failure_fromResult_success()
	  public virtual void failure_fromResult_success()
	  {
		Result<string> success = Result.success("Hello");
		Result.failure(success);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void failure_fromFailure()
	  {
		Failure failure = Failure.of(ERROR, "my failure");
		Result<int> test = Result.failure(failure);
		assertTrue(test.Failure);
		assertEquals(test.Failure.Message, "my failure");
		assertEquals(test.Failure.Items.size(), 1);
		FailureItem item = test.Failure.Items.GetEnumerator().next();
		assertEquals(item.Reason, ERROR);
		assertEquals(item.Message, "my failure");
		assertEquals(item.CauseType.Present, false);
		assertTrue(!string.ReferenceEquals(item.StackTrace, null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void ofNullable_nonNull()
	  {
		Result<int> test = Result.ofNullable(6);
		assertFalse(test.Failure);
		assertEquals(test.Value, 6);
	  }

	  public virtual void ofNullable_null()
	  {
		Result<int> test = Result.ofNullable(null);
		assertTrue(test.Failure);
		assertEquals(test.Failure.Message, "Found null where a value was expected");
		assertEquals(test.Failure.Items.size(), 1);
		FailureItem item = test.Failure.Items.GetEnumerator().next();
		assertEquals(item.Reason, MISSING_DATA);
		assertEquals(item.Message, "Found null where a value was expected");
		assertEquals(item.CauseType.Present, false);
		assertTrue(!string.ReferenceEquals(item.StackTrace, null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void of_with_success()
	  {

		Result<string> test = Result.of(() => "success");
		assertEquals(test.Success, true);
		assertEquals(test.Failure, false);
		assertEquals(test.Value, "success");
	  }

	  public virtual void of_with_exception()
	  {

		Result<string> test = Result.of(() =>
		{
		throw new System.ArgumentException("Big bad error");
		});
		assertEquals(test.Success, false);
		assertEquals(test.Failure, true);
		assertThrows(test.getValue, typeof(System.InvalidOperationException));
	  }

	  public virtual void wrap_with_success()
	  {
		Result<string> test = Result.wrap(() => Result.success("success"));
		assertEquals(test.Success, true);
		assertEquals(test.Failure, false);
		assertEquals(test.Value, "success");
	  }

	  public virtual void wrap_with_failure()
	  {

		Result<string> test = Result.wrap(() => Result.failure(ERROR, "Something failed"));
		assertEquals(test.Success, false);
		assertEquals(test.Failure, true);
		assertThrows(test.getValue, typeof(System.InvalidOperationException));
	  }

	  public virtual void wrap_with_exception()
	  {

		Result<string> test = Result.wrap(() =>
		{
		throw new System.ArgumentException("Big bad error");
		});
		assertEquals(test.Success, false);
		assertEquals(test.Failure, true);
		assertThrows(test.getValue, typeof(System.InvalidOperationException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void anyFailures_varargs()
	  {
		Result<string> success1 = Result.success("success 1");
		Result<string> success2 = Result.success("success 1");
		Result<object> failure1 = Result.failure(MISSING_DATA, "failure 1");
		Result<object> failure2 = Result.failure(ERROR, "failure 2");
		assertTrue(Result.anyFailures(failure1, failure2));
		assertTrue(Result.anyFailures(failure1, success1));
		assertFalse(Result.anyFailures(success1, success2));
	  }

	  public virtual void anyFailures_collection()
	  {
		Result<string> success1 = Result.success("success 1");
		Result<string> success2 = Result.success("success 1");
		Result<object> failure1 = Result.failure(MISSING_DATA, "failure 1");
		Result<object> failure2 = Result.failure(ERROR, "failure 2");
		assertTrue(Result.anyFailures(ImmutableList.of(failure1, failure2)));
		assertTrue(Result.anyFailures(ImmutableList.of(failure1, success1)));
		assertFalse(Result.anyFailures(ImmutableList.of(success1, success2)));
	  }

	  public virtual void countFailures_varargs()
	  {
		Result<string> success1 = Result.success("success 1");
		Result<string> success2 = Result.success("success 1");
		Result<object> failure1 = Result.failure(MISSING_DATA, "failure 1");
		Result<object> failure2 = Result.failure(ERROR, "failure 2");
		assertEquals(Result.countFailures(failure1, failure2), 2);
		assertEquals(Result.countFailures(failure1, success1), 1);
		assertEquals(Result.countFailures(success1, success2), 0);
	  }

	  public virtual void countFailures_collection()
	  {
		Result<string> success1 = Result.success("success 1");
		Result<string> success2 = Result.success("success 1");
		Result<object> failure1 = Result.failure(MISSING_DATA, "failure 1");
		Result<object> failure2 = Result.failure(ERROR, "failure 2");
		assertEquals(Result.countFailures(ImmutableList.of(failure1, failure2)), 2);
		assertEquals(Result.countFailures(ImmutableList.of(failure1, success1)), 1);
		assertEquals(Result.countFailures(ImmutableList.of(success1, success2)), 0);
	  }

	  public virtual void allSuccess_varargs()
	  {
		Result<string> success1 = Result.success("success 1");
		Result<string> success2 = Result.success("success 1");
		Result<object> failure1 = Result.failure(MISSING_DATA, "failure 1");
		Result<object> failure2 = Result.failure(ERROR, "failure 2");
		assertFalse(Result.allSuccessful(failure1, failure2));
		assertFalse(Result.allSuccessful(failure1, success1));
		assertTrue(Result.allSuccessful(success1, success2));
	  }

	  public virtual void allSuccess_collection()
	  {
		Result<string> success1 = Result.success("success 1");
		Result<string> success2 = Result.success("success 1");
		Result<object> failure1 = Result.failure(MISSING_DATA, "failure 1");
		Result<object> failure2 = Result.failure(ERROR, "failure 2");
		assertFalse(Result.allSuccessful(ImmutableList.of(failure1, failure2)));
		assertFalse(Result.allSuccessful(ImmutableList.of(failure1, success1)));
		assertTrue(Result.allSuccessful(ImmutableList.of(success1, success2)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void combine_iterableWithFailures()
	  {
		Result<string> success1 = Result.success("success 1");
		Result<string> success2 = Result.success("success 2");
		Result<string> failure1 = Result.failure(MISSING_DATA, "failure 1");
		Result<string> failure2 = Result.failure(ERROR, "failure 2");
		ISet<Result<string>> results = ImmutableSet.of(success1, success2, failure1, failure2);

		assertThat(Result.combine(results, s => s)).isFailure(FailureReason.MULTIPLE);
	  }

	  public virtual void combine_iterableWithSuccesses()
	  {
		Result<int> success1 = Result.success(1);
		Result<int> success2 = Result.success(2);
		Result<int> success3 = Result.success(3);
		Result<int> success4 = Result.success(4);
		ISet<Result<int>> results = ImmutableSet.of(success1, success2, success3, success4);

		Result<string> combined = Result.combine(results, s => "res" + s.reduce(1, (i1, i2) => i1 * i2));
		assertThat(combined).Success.hasValue("res24");
	  }

	  public virtual void combine_iterableWithSuccesses_throws()
	  {
		Result<int> success1 = Result.success(1);
		Result<int> success2 = Result.success(2);
		Result<int> success3 = Result.success(3);
		Result<int> success4 = Result.success(4);
		ISet<Result<int>> results = ImmutableSet.of(success1, success2, success3, success4);

		Result<string> combined = Result.combine(results, s =>
		{
		throw new System.ArgumentException("Ooops");
		});

		assertThat(combined).isFailure(ERROR).hasFailureMessageMatching("Ooops");
	  }

	  //-------------------------------------------------------------------------

	  public virtual void flatCombine_iterableWithFailures()
	  {
		Result<string> success1 = Result.success("success 1");
		Result<string> success2 = Result.success("success 2");
		Result<string> failure1 = Result.failure(MISSING_DATA, "failure 1");
		Result<string> failure2 = Result.failure(ERROR, "failure 2");
		ISet<Result<string>> results = ImmutableSet.of(success1, success2, failure1, failure2);

		assertThat(Result.flatCombine(results, Result.success)).isFailure(FailureReason.MULTIPLE);
	  }

	  public virtual void flatCombine_iterableWithSuccesses_combineFails()
	  {
		Result<int> success1 = Result.success(1);
		Result<int> success2 = Result.success(2);
		Result<int> success3 = Result.success(3);
		Result<int> success4 = Result.success(4);
		ISet<Result<int>> results = ImmutableSet.of(success1, success2, success3, success4);

		Result<string> combined = Result.flatCombine(results, s => Result.failure(CALCULATION_FAILED, "Could not do it"));

		assertThat(combined).isFailure(CALCULATION_FAILED);
	  }

	  public virtual void flatCombine_iterableWithSuccesses_combineSucceeds()
	  {
		Result<int> success1 = Result.success(1);
		Result<int> success2 = Result.success(2);
		Result<int> success3 = Result.success(3);
		Result<int> success4 = Result.success(4);
		ISet<Result<int>> results = ImmutableSet.of(success1, success2, success3, success4);

		Result<string> combined = Result.flatCombine(results, s => Result.success("res" + s.reduce(1, (i1, i2) => i1 * i2)));

		assertThat(combined).Success.hasValue("res24");
	  }

	  public virtual void flatCombine_iterableWithSuccesses_combineThrows()
	  {
		Result<int> success1 = Result.success(1);
		Result<int> success2 = Result.success(2);
		Result<int> success3 = Result.success(3);
		Result<int> success4 = Result.success(4);
		ISet<Result<int>> results = ImmutableSet.of(success1, success2, success3, success4);

		Result<string> combined = Result.flatCombine(results, s =>
		{
		throw new System.ArgumentException("Ooops");
		});

		assertThat(combined).isFailure(ERROR).hasFailureMessageMatching("Ooops");
	  }

	  //-------------------------------------------------------------------------

	  public virtual void failure_fromResults_varargs1()
	  {
		Result<string> success1 = Result.success("success 1");
		Result<string> success2 = Result.success("success 1");
		Result<object> failure1 = Result.failure(MISSING_DATA, "failure 1");
		Result<object> failure2 = Result.failure(ERROR, "failure 2");
		Result<object> test = Result.failure(success1, success2, failure1, failure2);
		ISet<FailureItem> expected = new HashSet<FailureItem>();
		expected.addAll(failure1.Failure.Items);
		expected.addAll(failure2.Failure.Items);
		assertEquals(test.Failure.Items, expected);
	  }

	  public virtual void failure_fromResults_varargs2()
	  {
		Result<string> success1 = Result.success("success 1");
		Result<string> success2 = Result.success("success 1");
		Result<object> failure1 = Result.failure(MISSING_DATA, "failure 1");
		Result<object> failure2 = Result.failure(ERROR, "failure 2");
		Result<object> test = Result.failure(success1, failure1, success2, failure2);
		ISet<FailureItem> expected = new HashSet<FailureItem>();
		expected.addAll(failure1.Failure.Items);
		expected.addAll(failure2.Failure.Items);
		assertEquals(test.Failure.Items, expected);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void failure_fromResults_varargs_allSuccess()
	  public virtual void failure_fromResults_varargs_allSuccess()
	  {
		Result<string> success1 = Result.success("success 1");
		Result<string> success2 = Result.success("success 1");
		Result.failure(success1, success2);
	  }

	  public virtual void failure_fromResults_collection()
	  {
		Result<string> success1 = Result.success("success 1");
		Result<string> success2 = Result.success("success 1");
		Result<string> failure1 = Result.failure(MISSING_DATA, "failure 1");
		Result<string> failure2 = Result.failure(ERROR, "failure 2");

		// Exposing collection explicitly shows why signature of failure is as it is
		IList<Result<string>> results = Arrays.asList(success1, success2, failure1, failure2);
		Result<string> test = Result.failure(results);
		ISet<FailureItem> expected = new HashSet<FailureItem>();
		expected.addAll(failure1.Failure.Items);
		expected.addAll(failure2.Failure.Items);
		assertEquals(test.Failure.Items, expected);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void failure_fromResults_collection_allSuccess()
	  public virtual void failure_fromResults_collection_allSuccess()
	  {
		Result<string> success1 = Result.success("success 1");
		Result<string> success2 = Result.success("success 1");
		Result.failure(Arrays.asList(success1, success2));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void generateFailureFromException()
	  {
		Exception exception = new Exception("something went wrong");
		Result<object> test = Result.failure(exception);
		assertEquals(test.Failure.Reason, ERROR);
		assertEquals(test.Failure.Message, "something went wrong");
	  }

	  public virtual void generateFailureFromExceptionWithMessage()
	  {
		Exception exception = new Exception("something went wrong");
		Result<object> test = Result.failure(exception, "my message");
		assertEquals(test.Failure.Reason, ERROR);
		assertEquals(test.Failure.Message, "my message");
	  }

	  public virtual void generateFailureFromExceptionWithCustomStatus()
	  {
		Exception exception = new Exception("something went wrong");
		Result<object> test = Result.failure(CALCULATION_FAILED, exception);
		assertEquals(test.Failure.Reason, CALCULATION_FAILED);
		assertEquals(test.Failure.Message, "something went wrong");
	  }

	  public virtual void generateFailureFromExceptionWithCustomStatusAndMessage()
	  {
		Exception exception = new Exception("something went wrong");
		Result<object> test = Result.failure(CALCULATION_FAILED, exception, "my message");
		assertEquals(test.Failure.Reason, CALCULATION_FAILED);
		assertEquals(test.Failure.Message, "my message");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void failureDeduplicateFailure()
	  {
		Result<object> result = Result.failure(MISSING_DATA, "failure");
		FailureItem failure = result.Failure.Items.GetEnumerator().next();

		Result<object> test = Result.failure(result, result);
		assertEquals(test.Failure.Items.size(), 1);
		assertEquals(test.Failure.Items, ImmutableSet.of(failure));
		assertEquals(test.Failure.Message, "failure");
	  }

	  public virtual void failureSameType()
	  {
		Result<object> failure1 = Result.failure(MISSING_DATA, "message 1");
		Result<object> failure2 = Result.failure(MISSING_DATA, "message 2");
		Result<object> failure3 = Result.failure(MISSING_DATA, "message 3");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: Result<?> composite = Result.failure(failure1, failure2, failure3);
		Result<object> composite = Result.failure(failure1, failure2, failure3);
		assertEquals(composite.Failure.Reason, MISSING_DATA);
		assertEquals(composite.Failure.Message, "message 1, message 2, message 3");
	  }

	  public virtual void failureDifferentTypes()
	  {
		Result<object> failure1 = Result.failure(MISSING_DATA, "message 1");
		Result<object> failure2 = Result.failure(CALCULATION_FAILED, "message 2");
		Result<object> failure3 = Result.failure(ERROR, "message 3");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: Result<?> composite = Result.failure(failure1, failure2, failure3);
		Result<object> composite = Result.failure(failure1, failure2, failure3);
		assertEquals(composite.Failure.Reason, FailureReason.MULTIPLE);
		assertEquals(composite.Failure.Message, "message 1, message 2, message 3");
	  }

	  //------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void createByBuilder_neitherValueNorFailure()
	  public virtual void createByBuilder_neitherValueNorFailure()
	  {
		Result.meta().builder().build();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void createByBuilder_bothValueAndFailure()
	  public virtual void createByBuilder_bothValueAndFailure()
	  {
		Result.meta().builder().set("value", "A").set("failure", Failure.of(CALCULATION_FAILED, "Fail")).build();
	  }

	  //-------------------------------------------------------------------------
	  public virtual void generatedStackTrace()
	  {
		Result<object> test = Result.failure(FailureReason.INVALID, "my {} {} failure", "big", "bad");
		assertEquals(test.Failure.Reason, FailureReason.INVALID);
		assertEquals(test.Failure.Message, "my big bad failure");
		assertEquals(test.Failure.Items.size(), 1);
		FailureItem item = test.Failure.Items.GetEnumerator().next();
		assertFalse(item.CauseType.Present);
		assertFalse(item.StackTrace.Contains(".FailureItem.of("));
		assertFalse(item.StackTrace.Contains(".Failure.of("));
		assertTrue(item.StackTrace.StartsWith("com.opengamma.strata.collect.result.FailureItem: my big bad failure", StringComparison.Ordinal));
		assertTrue(item.StackTrace.Contains(".generatedStackTrace("));
		assertEquals(item.ToString(), "INVALID: my big bad failure");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void generatedStackTrace_Failure()
	  {
		Failure test = Failure.of(FailureReason.INVALID, "my {} {} failure", "big", "bad");
		assertEquals(test.Reason, FailureReason.INVALID);
		assertEquals(test.Message, "my big bad failure");
		assertEquals(test.Items.size(), 1);
		FailureItem item = test.Items.GetEnumerator().next();
		assertFalse(item.CauseType.Present);
		assertFalse(item.StackTrace.Contains(".FailureItem.of("));
		assertFalse(item.StackTrace.Contains(".Failure.of("));
		assertTrue(item.StackTrace.StartsWith("com.opengamma.strata.collect.result.FailureItem: my big bad failure", StringComparison.Ordinal));
		assertTrue(item.StackTrace.Contains(".generatedStackTrace_Failure("));
		assertEquals(item.ToString(), "INVALID: my big bad failure");
	  }

	  //------------------------------------------------------------------------
	  public virtual void equalsHashCode()
	  {
		Exception ex = new Exception("Problem");
		Result<object> a1 = Result.failure(MISSING_DATA, ex);
		Result<object> a2 = Result.failure(MISSING_DATA, ex);
		Result<object> b = Result.failure(ERROR, "message 2");
		Result<object> c = Result.success("Foo");
		Result<object> d = Result.success("Bar");

		assertEquals(a1.Equals(a1), true);
		assertEquals(a1.Equals(a2), true);
		assertEquals(a1.Equals(b), false);
		assertEquals(a1.Equals(c), false);
		assertEquals(a1.Equals(d), false);

		assertEquals(b.Equals(a1), false);
		assertEquals(b.Equals(a2), false);
		assertEquals(b.Equals(b), true);
		assertEquals(b.Equals(c), false);
		assertEquals(b.Equals(d), false);

		assertEquals(c.Equals(a1), false);
		assertEquals(c.Equals(a2), false);
		assertEquals(c.Equals(b), false);
		assertEquals(c.Equals(c), true);
		assertEquals(c.Equals(d), false);

		assertEquals(d.Equals(a1), false);
		assertEquals(d.Equals(a2), false);
		assertEquals(d.Equals(b), false);
		assertEquals(d.Equals(c), false);
		assertEquals(d.Equals(d), true);
	  }

	  // Following are tests for Result using the AssertJ assertions
	  // Primarily a test of the assertions themselves

	  public virtual void assert_success()
	  {
		Result<string> test = Result.success("success");
		assertThat(test).Success.hasValue("success");
	  }

	  // We can't use assertThrows as that rethrows AssertionError
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = AssertionError.class) public void assert_success_getFailure()
	  public virtual void assert_success_getFailure()
	  {
		Result<string> test = Result.success("success");
		assertThat(test).Failure;
	  }

	  public virtual void assert_success_map()
	  {
		Result<string> success = Result.success("success");
		Result<int> test = success.map(MAP_STRLEN);
		assertThat(test).Success.hasValue(7);
	  }

	  public virtual void assert_success_flatMap()
	  {
		Result<string> success = Result.success("success");
		Result<int> test = success.flatMap(FUNCTION_STRLEN);
		assertThat(test).Success.hasValue(7);
	  }

	  public virtual void assert_success_combineWith_success()
	  {
		Result<string> success1 = Result.success("Hello");
		Result<string> success2 = Result.success("World");
		Result<string> test = success1.combineWith(success2, FUNCTION_MERGE);
		assertThat(test).Success.hasValue("Hello World");
	  }

	  public virtual void assert_success_combineWith_failure()
	  {
		Result<string> success = Result.success("Hello");
		Result<string> failure = Result.failure(new System.ArgumentException());
		Result<string> test = success.combineWith(failure, FUNCTION_MERGE);
		assertThat(test).isFailure(ERROR);
		assertThat(test.Failure.Items.size()).isEqualTo(1);
	  }

	  public virtual void assert_failure()
	  {
		Result<string> test = Result.failure(new System.ArgumentException("failure"));
		assertThat(test).isFailure(ERROR).hasFailureMessageMatching("failure");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		Result<object> failure = Result.failure(MISSING_DATA, "message 1");
		TestHelper.coverImmutableBean(failure);
		TestHelper.coverImmutableBean(failure.Failure);
		TestHelper.coverImmutableBean(failure.Failure.Items.GetEnumerator().next());

		Result<string> success = Result.success("Hello");
		TestHelper.coverImmutableBean(success);

		TestHelper.coverEnum(typeof(FailureReason));
	  }

	}

}