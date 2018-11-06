using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function.special
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NaturalLogGammaFunctionTest
	public class NaturalLogGammaFunctionTest
	{
	  private static readonly System.Func<double, double> LN_GAMMA = new NaturalLogGammaFunction();
	  private const double EPS = 1e-9;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeNumber()
	  public virtual void testNegativeNumber()
	  {
		LN_GAMMA.apply(-0.1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testRecurrence()
	  public virtual void testRecurrence()
	  {
		double z = 12;
		double gamma = getGammaFunction(LN_GAMMA.apply(z));
		assertEquals(getGammaFunction(LN_GAMMA.apply(z + 1)), z * gamma, gamma * EPS);
		z = 11.34;
		gamma = getGammaFunction(LN_GAMMA.apply(z));
		assertEquals(getGammaFunction(LN_GAMMA.apply(z + 1)), z * gamma, gamma * EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testIntegerArgument()
	  public virtual void testIntegerArgument()
	  {
		const int x = 5;
		const double factorial = 24;
		assertEquals(getGammaFunction(LN_GAMMA.apply(Convert.ToDouble(x))), factorial, EPS);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private double getGammaFunction(final double x)
	  private double getGammaFunction(double x)
	  {
		return Math.Exp(x);
	  }
	}

}