using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Well44497b = org.apache.commons.math3.random.Well44497b;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class RealPolynomialFunction1DTest
	public class RealPolynomialFunction1DTest
	{

	  private static readonly Well44497b RANDOM = new Well44497b(0L);
	  private static readonly double[] C = new double[] {3.4, 5.6, 1.0, -4.0};
	  private static readonly DoubleFunction1D F = new RealPolynomialFunction1D(C);
	  private const double EPS = 1e-12;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullCoefficients()
	  public virtual void testNullCoefficients()
	  {
		new RealPolynomialFunction1D(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testEmptyCoefficients()
	  public virtual void testEmptyCoefficients()
	  {
		new RealPolynomialFunction1D(new double[0]);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testEvaluate()
	  public virtual void testEvaluate()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double x = RANDOM.nextDouble();
		double x = RANDOM.NextDouble();
		assertEquals(C[3] * Math.Pow(x, 3) + C[2] * Math.Pow(x, 2) + C[1] * x + C[0], F.applyAsDouble(x), EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testDerivative()
	  public virtual void testDerivative()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double x = RANDOM.nextDouble();
		double x = RANDOM.NextDouble();
		assertEquals(3 * C[3] * Math.Pow(x, 2) + 2 * C[2] * x + C[1], F.derivative().applyAsDouble(x), EPS);
	  }
	}

}