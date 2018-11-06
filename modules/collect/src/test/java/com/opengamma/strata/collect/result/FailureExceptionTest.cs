/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.result
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="FailureException"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FailureExceptionTest
	public class FailureExceptionTest
	{

	  public virtual void test_constructor_failure()
	  {
		Failure failure = Failure.of(FailureReason.UNSUPPORTED, "Test");
		FailureException test = new FailureException(failure);
		assertEquals(test.Failure, failure);
	  }

	}

}