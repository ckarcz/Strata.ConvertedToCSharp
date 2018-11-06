using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.result
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;

	/// <summary>
	/// Test <seealso cref="FailureItem"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FailureItemTest
	public class FailureItemTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_of_reasonMessage()
	  {
		FailureItem test = FailureItem.of(FailureReason.INVALID, "my {} {} failure", "big", "bad");
		assertEquals(test.Reason, FailureReason.INVALID);
		assertEquals(test.Message, "my big bad failure");
		assertFalse(test.CauseType.Present);
		assertFalse(test.StackTrace.Contains(".FailureItem.of("));
		assertFalse(test.StackTrace.Contains(".Failure.of("));
		assertTrue(test.StackTrace.StartsWith("com.opengamma.strata.collect.result.FailureItem: my big bad failure", StringComparison.Ordinal));
		assertTrue(test.StackTrace.Contains(".test_of_reasonMessage("));
		assertEquals(test.ToString(), "INVALID: my big bad failure");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_reasonMessageShortStackTrace()
	  {
		FailureItem test = FailureItem.meta().builder().set("reason", FailureReason.INVALID).set("message", "my issue").set("stackTrace", "Short stack trace").set("causeType", typeof(System.ArgumentException)).build();
		assertEquals(test.ToString(), "INVALID: my issue: Short stack trace");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_reasonException()
	  {
		System.ArgumentException ex = new System.ArgumentException("exmsg");
		FailureItem test = FailureItem.of(FailureReason.INVALID, ex);
		assertEquals(test.Reason, FailureReason.INVALID);
		assertEquals(test.Message, "exmsg");
		assertTrue(test.CauseType.Present);
		assertEquals(test.CauseType.get(), typeof(System.ArgumentException));
		assertTrue(test.StackTrace.Contains(".test_of_reasonException("));
		assertEquals(test.ToString(), "INVALID: exmsg: java.lang.IllegalArgumentException");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_reasonMessageException()
	  {
		System.ArgumentException ex = new System.ArgumentException("exmsg");
		FailureItem test = FailureItem.of(FailureReason.INVALID, ex, "my failure");
		assertEquals(test.Reason, FailureReason.INVALID);
		assertEquals(test.Message, "my failure");
		assertTrue(test.CauseType.Present);
		assertEquals(test.CauseType.get(), typeof(System.ArgumentException));
		assertTrue(test.StackTrace.Contains(".test_of_reasonMessageException("));
		assertEquals(test.ToString(), "INVALID: my failure: java.lang.IllegalArgumentException: exmsg");
	  }

	  public virtual void test_of_reasonMessageExceptionNestedException()
	  {
		System.ArgumentException innerEx = new System.ArgumentException("inner");
		System.ArgumentException ex = new System.ArgumentException("exmsg", innerEx);
		FailureItem test = FailureItem.of(FailureReason.INVALID, ex, "my {} {} failure", "big", "bad");
		assertEquals(test.Reason, FailureReason.INVALID);
		assertEquals(test.Message, "my big bad failure");
		assertTrue(test.CauseType.Present);
		assertEquals(test.CauseType.get(), typeof(System.ArgumentException));
		assertTrue(test.StackTrace.Contains(".test_of_reasonMessageExceptionNestedException("));
		assertEquals(test.ToString(), "INVALID: my big bad failure: java.lang.IllegalArgumentException: exmsg");
	  }

	  public virtual void test_of_reasonMessageExceptionNestedExceptionWithAttributes()
	  {
		System.ArgumentException innerEx = new System.ArgumentException("inner");
		System.ArgumentException ex = new System.ArgumentException("exmsg", innerEx);
		FailureItem test = FailureItem.of(FailureReason.INVALID, ex, "a {foo} {bar} failure", "big", "bad");
		assertEquals(test.Attributes, ImmutableMap.of("foo", "big", "bar", "bad", FailureItem.EXCEPTION_MESSAGE_ATTRIBUTE, "exmsg"));
		assertEquals(test.Reason, FailureReason.INVALID);
		assertEquals(test.Message, "a big bad failure");
		assertTrue(test.CauseType.Present);
		assertEquals(test.CauseType.get(), typeof(System.ArgumentException));
		assertTrue(test.StackTrace.Contains(".test_of_reasonMessageExceptionNestedExceptionWithAttributes("));
		assertEquals(test.ToString(), "INVALID: a big bad failure: java.lang.IllegalArgumentException: exmsg");
	  }

	  public virtual void test_of_reasonMessageWithAttributes()
	  {
		System.ArgumentException innerEx = new System.ArgumentException("inner");
		System.ArgumentException ex = new System.ArgumentException("exmsg", innerEx);
		FailureItem test = FailureItem.of(FailureReason.INVALID, ex, "failure: {exceptionMessage}", "error");
		assertEquals(test.Attributes, ImmutableMap.of(FailureItem.EXCEPTION_MESSAGE_ATTRIBUTE, "error"));
		assertEquals(test.Reason, FailureReason.INVALID);
		assertEquals(test.Message, "failure: error");
		assertTrue(test.CauseType.Present);
		assertEquals(test.CauseType.get(), typeof(System.ArgumentException));
		assertTrue(test.StackTrace.Contains(".test_of_reasonMessageWithAttributes("));
		assertEquals(test.ToString(), "INVALID: failure: error: java.lang.IllegalArgumentException: exmsg");
	  }

	  public virtual void test_withAttribute()
	  {
		FailureItem test = FailureItem.of(FailureReason.INVALID, "my {one} {two} failure", "big", "bad");
		test = test.withAttribute("foo", "bar");
		assertEquals(test.Attributes, ImmutableMap.of("one", "big", "two", "bad", "foo", "bar"));
		assertEquals(test.Reason, FailureReason.INVALID);
		assertEquals(test.Message, "my big bad failure");
		assertFalse(test.CauseType.Present);
		assertFalse(test.StackTrace.Contains(".FailureItem.of("));
		assertFalse(test.StackTrace.Contains(".Failure.of("));
		assertTrue(test.StackTrace.StartsWith("com.opengamma.strata.collect.result.FailureItem: my big bad failure", StringComparison.Ordinal));
		assertTrue(test.StackTrace.Contains(".test_withAttribute("));
		assertEquals(test.ToString(), "INVALID: my big bad failure");
	  }

	  public virtual void test_withAttributes()
	  {
		FailureItem test = FailureItem.of(FailureReason.INVALID, "my {one} {two} failure", "big", "bad");
		test = test.withAttributes(ImmutableMap.of("foo", "bar", "two", "good"));
		assertEquals(test.Attributes, ImmutableMap.of("one", "big", "two", "good", "foo", "bar"));
		assertEquals(test.Reason, FailureReason.INVALID);
		assertEquals(test.Message, "my big bad failure");
		assertFalse(test.CauseType.Present);
		assertFalse(test.StackTrace.Contains(".FailureItem.of("));
		assertFalse(test.StackTrace.Contains(".Failure.of("));
		assertTrue(test.StackTrace.StartsWith("com.opengamma.strata.collect.result.FailureItem: my big bad failure", StringComparison.Ordinal));
		assertTrue(test.StackTrace.Contains(".test_withAttributes("));
		assertEquals(test.ToString(), "INVALID: my big bad failure");
	  }

	}

}