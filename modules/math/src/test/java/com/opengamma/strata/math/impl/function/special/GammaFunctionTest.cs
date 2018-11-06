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


	using Well44497b = org.apache.commons.math3.random.Well44497b;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class GammaFunctionTest
	public class GammaFunctionTest
	{

	  private static readonly Well44497b RANDOM = new Well44497b(0L);
	  private static readonly System.Func<double, double> GAMMA = new GammaFunction();
	  private static readonly System.Func<double, double> LN_GAMMA = new NaturalLogGammaFunction();
	  private const double EPS = 1e-9;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double x = RANDOM.nextDouble();
		double x = RANDOM.NextDouble();
		assertEquals(Math.Log(GAMMA.applyAsDouble(x)), LN_GAMMA.apply(x), EPS);
	  }
	}

}