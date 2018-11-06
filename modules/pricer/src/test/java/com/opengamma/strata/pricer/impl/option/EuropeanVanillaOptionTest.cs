/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.option
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PutCall.CALL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PutCall.PUT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="EuropeanVanillaOption"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class EuropeanVanillaOptionTest
	public class EuropeanVanillaOptionTest
	{

	  private const double STRIKE = 100;
	  private const double TIME = 0.5;

	  public virtual void testNegativeTime()
	  {
		assertThrowsIllegalArg(() => EuropeanVanillaOption.of(STRIKE, -TIME, CALL));
	  }

	  public virtual void test_of()
	  {
		EuropeanVanillaOption test = EuropeanVanillaOption.of(STRIKE, TIME, CALL);
		assertEquals(test.Strike, STRIKE, 0d);
		assertEquals(test.TimeToExpiry, TIME, 0d);
		assertEquals(test.PutCall, CALL);
		assertTrue(test.Call);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		EuropeanVanillaOption test = EuropeanVanillaOption.of(STRIKE, TIME, CALL);
		coverImmutableBean(test);
		EuropeanVanillaOption test2 = EuropeanVanillaOption.of(110, 0.6, PUT);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		EuropeanVanillaOption test = EuropeanVanillaOption.of(STRIKE, TIME, CALL);
		assertSerialization(test);
	  }

	}

}