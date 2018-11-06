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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertTrue;

	using Well44497b = org.apache.commons.math3.random.Well44497b;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class OrdinaryLeastSquaresRegressionTest
	public class OrdinaryLeastSquaresRegressionTest
	{

	  private static readonly LeastSquaresRegression REGRESSION = new OrdinaryLeastSquaresRegression();
	  private static readonly Well44497b RANDOM = new Well44497b(0L);
	  private const double EPS = 1e-2;
	  private static readonly double FACTOR = 1.0 / EPS;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		const int n = 20;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] x = new double[n][5];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] x = new double[n][5];
		double[][] x = RectangularArrays.ReturnRectangularDoubleArray(n, 5);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] y1 = new double[n];
		double[] y1 = new double[n];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] y2 = new double[n];
		double[] y2 = new double[n];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] a1 = new double[] {3.4, 1.2, -0.62, -0.44, 0.65 };
		double[] a1 = new double[] {3.4, 1.2, -0.62, -0.44, 0.65};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] a2 = new double[] {0.98, 3.4, 1.2, -0.62, -0.44, 0.65 };
		double[] a2 = new double[] {0.98, 3.4, 1.2, -0.62, -0.44, 0.65};
		for (int i = 0; i < n; i++)
		{
		  for (int j = 0; j < 5; j++)
		  {
			x[i][j] = RANDOM.NextDouble() + (RANDOM.NextDouble() - 0.5) / FACTOR;
		  }
		  y1[i] = a1[0] * x[i][0] + a1[1] * x[i][1] + a1[2] * x[i][2] + a1[3] * x[i][3] + a1[4] * x[i][4] + RANDOM.NextDouble() / FACTOR;
		  y2[i] = a2[0] + a2[1] * x[i][0] + a2[2] * x[i][1] + a2[3] * x[i][2] + a2[4] * x[i][3] + a2[5] * x[i][4] + RANDOM.NextDouble() / FACTOR;
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final LeastSquaresRegressionResult result1 = REGRESSION.regress(x, null, y1, false);
		LeastSquaresRegressionResult result1 = REGRESSION.regress(x, null, y1, false);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final LeastSquaresRegressionResult result2 = REGRESSION.regress(x, null, y2, true);
		LeastSquaresRegressionResult result2 = REGRESSION.regress(x, null, y2, true);
		assertRegression(result1, a1);
		assertRegression(result2, a2);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] residuals1 = result1.getResiduals();
		double[] residuals1 = result1.Residuals;
		for (int i = 0; i < n; i++)
		{
		  assertEquals(y1[i], a1[0] * x[i][0] + a1[1] * x[i][1] + a1[2] * x[i][2] + a1[3] * x[i][3] + a1[4] * x[i][4] + residuals1[i], 10 * EPS);
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] residuals2 = result2.getResiduals();
		double[] residuals2 = result2.Residuals;
		for (int i = 0; i < n; i++)
		{
		  assertEquals(y2[i], a2[0] + a2[1] * x[i][0] + a2[2] * x[i][1] + a2[3] * x[i][2] + a2[4] * x[i][3] + a2[5] * x[i][4] + residuals2[i], 10 * EPS);
		}
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private void assertRegression(final LeastSquaresRegressionResult result, final double[] a)
	  private void assertRegression(LeastSquaresRegressionResult result, double[] a)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] beta = result.getBetas();
		double[] beta = result.Betas;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] tStat = result.getTStatistics();
		double[] tStat = result.TStatistics;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] pStat = result.getPValues();
		double[] pStat = result.PValues;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] stdErr = result.getStandardErrorOfBetas();
		double[] stdErr = result.StandardErrorOfBetas;
		for (int i = 0; i < 5; i++)
		{
		  assertEquals(beta[i], a[i], EPS);
		  assertTrue(Math.Abs(tStat[i]) > FACTOR);
		  assertTrue(pStat[i] < EPS);
		  assertTrue(stdErr[i] < EPS);
		}
		assertEquals(result.RSquared, 1, EPS);
		assertEquals(result.AdjustedRSquared, 1, EPS);
	  }
	}

}