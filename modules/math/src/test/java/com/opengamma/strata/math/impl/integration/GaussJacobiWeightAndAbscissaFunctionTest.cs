using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.integration
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertTrue;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class GaussJacobiWeightAndAbscissaFunctionTest extends WeightAndAbscissaFunctionTestCase
	public class GaussJacobiWeightAndAbscissaFunctionTest : WeightAndAbscissaFunctionTestCase
	{
	  private static readonly QuadratureWeightAndAbscissaFunction GAUSS_LEGENDRE = new GaussLegendreWeightAndAbscissaFunction();
	  private static readonly QuadratureWeightAndAbscissaFunction GAUSS_JACOBI_GL_EQUIV = new GaussJacobiWeightAndAbscissaFunction(0, 0);
	  private static readonly QuadratureWeightAndAbscissaFunction GAUSS_JACOBI_CHEBYSHEV_EQUIV = new GaussJacobiWeightAndAbscissaFunction(-0.5, -0.5);
	  private const double EPS = 1e-8;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		const int n = 12;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final GaussianQuadratureData f1 = GAUSS_LEGENDRE.generate(n);
		GaussianQuadratureData f1 = GAUSS_LEGENDRE.generate(n);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final GaussianQuadratureData f2 = GAUSS_JACOBI_GL_EQUIV.generate(n);
		GaussianQuadratureData f2 = GAUSS_JACOBI_GL_EQUIV.generate(n);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final GaussianQuadratureData f3 = GAUSS_JACOBI_CHEBYSHEV_EQUIV.generate(n);
		GaussianQuadratureData f3 = GAUSS_JACOBI_CHEBYSHEV_EQUIV.generate(n);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] w1 = f1.getWeights();
		double[] w1 = f1.Weights;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] w2 = f2.getWeights();
		double[] w2 = f2.Weights;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] x1 = f1.getAbscissas();
		double[] x1 = f1.Abscissas;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] x2 = f2.getAbscissas();
		double[] x2 = f2.Abscissas;
		assertTrue(w1.Length == w2.Length);
		assertTrue(x1.Length == x2.Length);
		for (int i = 0; i < n; i++)
		{
		  assertEquals(w1[i], w2[i], EPS);
		  assertEquals(x1[i], -x2[i], EPS);
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] w3 = f3.getWeights();
		double[] w3 = f3.Weights;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] x3 = f3.getAbscissas();
		double[] x3 = f3.Abscissas;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double chebyshevWeight = Math.PI / n;
		double chebyshevWeight = Math.PI / n;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<int, double> chebyshevAbscissa = new java.util.function.Function<int, double>()
		System.Func<int, double> chebyshevAbscissa = (final int? x) =>
		{

	return -Math.Cos(Math.PI * (x + 0.5) / n);

		};
		for (int i = 0; i < n; i++)
		{
		  assertEquals(chebyshevWeight, w3[i], EPS);
		  assertEquals(chebyshevAbscissa(i), -x3[i], EPS);
		}
	  }

	  protected internal override QuadratureWeightAndAbscissaFunction Function
	  {
		  get
		  {
			return GAUSS_JACOBI_GL_EQUIV;
		  }
	  }

	}

}