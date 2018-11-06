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

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class GeneralizedLeastSquaresRegressionTest
	public class GeneralizedLeastSquaresRegressionTest
	{
	  private const double EPS = 1e-9;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		const double a0 = 2.3;
		const double a1 = 4.7;
		const double a2 = -0.99;
		const double a3 = -5.1;
		const double a4 = 0.27;
		const int n = 30;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] x = new double[n][4];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] x = new double[n][4];
		double[][] x = RectangularArrays.ReturnRectangularDoubleArray(n, 4);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yIntercept = new double[n];
		double[] yIntercept = new double[n];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yNoIntercept = new double[n];
		double[] yNoIntercept = new double[n];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] w = new double[n][n];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] w = new double[n][n];
		double[][] w = RectangularArrays.ReturnRectangularDoubleArray(n, n);
		double y, x1, x2, x3, x4;
		for (int i = 0; i < n; i++)
		{
		  x1 = i;
		  x2 = x1 * x1;
		  x3 = Math.Sqrt(x1);
		  x4 = x1 * x2;
		  x[i] = new double[] {x1, x2, x3, x4};
		  y = x1 * a1 + x2 * a2 + x3 * a3 + x4 * a4;
		  yNoIntercept[i] = y;
		  yIntercept[i] = y + a0;
		  for (int j = 0; j < n; j++)
		  {
			w[i][j] = 0.0;
		  }
		  w[i][i] = 1.0;
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final GeneralizedLeastSquaresRegression regression = new GeneralizedLeastSquaresRegression();
		GeneralizedLeastSquaresRegression regression = new GeneralizedLeastSquaresRegression();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final OrdinaryLeastSquaresRegression olsRegression = new OrdinaryLeastSquaresRegression();
		OrdinaryLeastSquaresRegression olsRegression = new OrdinaryLeastSquaresRegression();
		LeastSquaresRegressionResult gls = regression.regress(x, w, yIntercept, true);
		LeastSquaresRegressionResult ols = olsRegression.regress(x, yIntercept, true);
		assertRegressions(n, 5, gls, ols);
		gls = regression.regress(x, w, yNoIntercept, false);
		ols = olsRegression.regress(x, yNoIntercept, false);
		assertRegressions(n, 4, gls, ols);
		gls = regression.regress(x, w, yIntercept, true);
		ols = olsRegression.regress(x, yIntercept, true);
		assertRegressions(n, 5, gls, ols);
		gls = regression.regress(x, w, yNoIntercept, false);
		ols = olsRegression.regress(x, yNoIntercept, false);
		assertRegressions(n, 4, gls, ols);
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
//ORIGINAL LINE: final double[] b2 = regression2.getBetas();
		double[] b2 = regression2.Betas;
		for (int i = 0; i < k; i++)
		{
		  assertEquals(b1[i], b2[i], EPS);
		}
	  }
	}

}