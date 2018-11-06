/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PricingExceptionTest
	public class PricingExceptionTest
	{

	  public virtual void test_constructor_message()
	  {
		PricingException test = new PricingException("Hello");
		assertEquals(test.Message, "Hello");
	  }

	  public virtual void test_constructor_messageCause()
	  {
		System.ArgumentException cause = new System.ArgumentException("Under");
		PricingException test = new PricingException("Hello", cause);
		assertEquals(test.Message, "Hello");
		assertEquals(test.InnerException, cause);
	  }

	}

}