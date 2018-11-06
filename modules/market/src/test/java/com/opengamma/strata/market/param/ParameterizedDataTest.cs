/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertSame;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="ParameterizedData"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ParameterizedDataTest
	public class ParameterizedDataTest
	{

	  private static readonly ParameterizedData CURVE = new TestingParameterizedData(1d);

	  public virtual void test_withPerturbation()
	  {
		assertSame(CURVE.withPerturbation((i, v, m) => v), CURVE);
		assertEquals(CURVE.withPerturbation((i, v, m) => v + 2d).getParameter(0), 3d);
	  }

	}

}