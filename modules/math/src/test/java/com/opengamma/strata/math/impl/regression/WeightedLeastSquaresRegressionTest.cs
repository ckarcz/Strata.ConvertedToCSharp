using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.regression
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Well44497b = org.apache.commons.math3.random.Well44497b;
	using Assert = org.testng.Assert;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class WeightedLeastSquaresRegressionTest
	public class WeightedLeastSquaresRegressionTest
	{

	  private static readonly Well44497b RANDOM = new Well44497b(0L);
	  private const double EPS = 1e-2;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		const double a0 = 2.3;
		const double a1 = -4.5;
		const double a2 = 0.76;
		const double a3 = 3.4;
		const int n = 30;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] x = new double[n][3];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] x = new double[n][3];
		double[][] x = RectangularArrays.ReturnRectangularDoubleArray(n, 3);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yIntercept = new double[n];
		double[] yIntercept = new double[n];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yNoIntercept = new double[n];
		double[] yNoIntercept = new double[n];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] w1 = new double[n][n];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] w1 = new double[n][n];
		double[][] w1 = RectangularArrays.ReturnRectangularDoubleArray(n, n);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] w2 = new double[n];
		double[] w2 = new double[n];
		double y, x1, x2, x3;
		for (int i = 0; i < n; i++)
		{
		  x1 = i;
		  x2 = x1 * x1;
		  x3 = Math.Sqrt(x1);
		  x[i] = new double[] {x1, x2, x3};
		  y = x1 * a1 + x2 * a2 + x3 * a3;
		  yNoIntercept[i] = y;
		  yIntercept[i] = y + a0;
		  for (int j = 0; j < n; j++)
		  {
			w1[i][j] = RANDOM.NextDouble();
		  }
		  w1[i][i] = 1.0;
		  w2[i] = 1.0;
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final WeightedLeastSquaresRegression wlsRegression = new WeightedLeastSquaresRegression();
		WeightedLeastSquaresRegression wlsRegression = new WeightedLeastSquaresRegression();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final OrdinaryLeastSquaresRegression olsRegression = new OrdinaryLeastSquaresRegression();
		OrdinaryLeastSquaresRegression olsRegression = new OrdinaryLeastSquaresRegression();
		try
		{
		  wlsRegression.regress(x, (double[]) null, yNoIntercept, false);
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
		  // Expected
		}
		LeastSquaresRegressionResult wls = wlsRegression.regress(x, w1, yIntercept, true);
		LeastSquaresRegressionResult ols = olsRegression.regress(x, yIntercept, true);
		assertRegressions(n, 4, wls, ols);
		wls = wlsRegression.regress(x, w1, yNoIntercept, false);
		ols = olsRegression.regress(x, yNoIntercept, false);
		assertRegressions(n, 3, wls, ols);
		wls = wlsRegression.regress(x, w2, yIntercept, true);
		ols = olsRegression.regress(x, yIntercept, true);
		assertRegressions(n, 4, wls, ols);
		wls = wlsRegression.regress(x, w2, yNoIntercept, false);
		ols = olsRegression.regress(x, yNoIntercept, false);
		assertRegressions(n, 3, wls, ols);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private void assertRegressions(final int n, final int k, final LeastSquaresRegressionResult regression1, final LeastSquaresRegressionResult regression2)
	  private void assertRegressions(int n, int k, LeastSquaresRegressionResult regression1, LeastSquaresRegressionResult regression2)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] r1 = regression1.getResiduals();
		double[] r1 = regression1.Residuals;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] r2 = regression2.getResiduals();
		double[] r2 = regression2.Residuals;
		for (int i = 0; i < n; i++)
		{
		  assertEquals(r1[i], r2[i], EPS);
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] b1 = regression1.getBetas();
		double[] b1 = regression1.Betas;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] t1 = regression1.getTStatistics();
		double[] t1 = regression1.TStatistics;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] p1 = regression1.getPValues();
		double[] p1 = regression1.PValues;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] s1 = regression1.getStandardErrorOfBetas();
		double[] s1 = regression1.StandardErrorOfBetas;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] b2 = regression2.getBetas();
		double[] b2 = regression2.Betas;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] t2 = regression2.getTStatistics();
		double[] t2 = regression2.TStatistics;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] p2 = regression2.getPValues();
		double[] p2 = regression2.PValues;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] s2 = regression2.getStandardErrorOfBetas();
		double[] s2 = regression2.StandardErrorOfBetas;
		for (int i = 0; i < k; i++)
		{
		  assertEquals(b1[i], b2[i], EPS);
		  assertEquals(t1[i], t2[i], EPS);
		  assertEquals(p1[i], p2[i], EPS);
		  assertEquals(s1[i], s2[i], EPS);
		}
		assertEquals(regression1.RSquared, regression2.RSquared, EPS);
		assertEquals(regression1.AdjustedRSquared, regression2.AdjustedRSquared, EPS);
		assertEquals(regression1.MeanSquareError, regression2.MeanSquareError, EPS);
	  }
	}

}