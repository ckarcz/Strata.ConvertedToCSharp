/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="CurveName"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurveNameTest
	public class CurveNameTest
	{

	  public virtual void test_of()
	  {
		CurveName test = CurveName.of("Foo");
		assertEquals(test.Name, "Foo");
		assertEquals(test.MarketDataType, typeof(Curve));
		assertEquals(test.ToString(), "Foo");
		assertEquals(test.CompareTo(CurveName.of("Goo")) < 0, true);
	  }

	}

}