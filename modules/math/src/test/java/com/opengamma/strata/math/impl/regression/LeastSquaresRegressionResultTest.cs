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
//	import static org.testng.AssertJUnit.assertFalse;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.@internal.junit.ArrayAsserts.assertArrayEquals;

	using Well44497b = org.apache.commons.math3.random.Well44497b;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LeastSquaresRegressionResultTest
	public class LeastSquaresRegressionResultTest
	{

	  private static readonly LeastSquaresRegression REGRESSION = new OrdinaryLeastSquaresRegression();
	  private static readonly Well44497b RANDOM = new Well44497b(0L);
	  private static readonly LeastSquaresRegressionResult NO_INTERCEPT;
	  private static readonly LeastSquaresRegressionResult INTERCEPT;
	  private const double BETA_0 = 3.9;
	  private const double BETA_1 = -1.4;
	  private const double BETA_2 = 4.6;
	  private static readonly System.Func<double, double, double> F1 = (x1, x2) => x1 * BETA_1 + x2 * BETA_2;
	  private static readonly System.Func<double, double, double> F2 = (x1, x2) => BETA_0 + x1 * BETA_1 + x2 * BETA_2;
	  private const double EPS = 1e-9;

	  static LeastSquaresRegressionResultTest()
	  {
		const int n = 100;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] x = new double[n][2];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] x = new double[n][2];
		double[][] x = RectangularArrays.ReturnRectangularDoubleArray(n, 2);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] y1 = new double[n];
		double[] y1 = new double[n];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] y2 = new double[n];
		double[] y2 = new double[n];
		for (int i = 0; i < n; i++)
		{
		  x[i][0] = RANDOM.NextDouble();
		  x[i][1] = RANDOM.NextDouble();
		  y1[i] = F1.applyAsDouble(x[i][0], x[i][1]);
		  y2[i] = F2.applyAsDouble(x[i][0], x[i][1]);
		}
		NO_INTERCEPT = REGRESSION.regress(x, null, y1, false);
		INTERCEPT = REGRESSION.regress(x, null, y2, true);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testInputs()
	  public virtual void testInputs()
	  {
		new LeastSquaresRegressionResult(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullArray()
	  public virtual void testNullArray()
	  {
		NO_INTERCEPT.getPredictedValue(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testLongArray()
	  public virtual void testLongArray()
	  {
		NO_INTERCEPT.getPredictedValue(new double[] {2.4, 2.5, 3.4});
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testShortArray()
	  public virtual void testShortArray()
	  {
		NO_INTERCEPT.getPredictedValue(new double[] {2.4});
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testPredictedValue()
	  public virtual void testPredictedValue()
	  {
		double[] z;
		for (int i = 0; i < 10; i++)
		{
		  z = new double[] {RANDOM.NextDouble(), RANDOM.NextDouble()};
		  assertEquals(F1.applyAsDouble(z[0], z[1]), NO_INTERCEPT.getPredictedValue(z), EPS);
		  assertEquals(F2.applyAsDouble(z[0], z[1]), INTERCEPT.getPredictedValue(z), EPS);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testEqualsAndHashCode()
	  public virtual void testEqualsAndHashCode()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] residuals = new double[] {1, 2, 3 };
		double[] residuals = new double[] {1, 2, 3};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] betas = new double[] {1.1, 2.1, 3.1 };
		double[] betas = new double[] {1.1, 2.1, 3.1};
		const double meanSquareError = 0.78;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] standardErrorOfBeta = new double[] {1.2, 2.2, 3.2 };
		double[] standardErrorOfBeta = new double[] {1.2, 2.2, 3.2};
		const double rSquared = 0.98;
		const double rSquaredAdjusted = 0.96;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] tStats = new double[] {1.3, 2.3, 3.3 };
		double[] tStats = new double[] {1.3, 2.3, 3.3};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] pValues = new double[] {1.4, 2.4, 3.4 };
		double[] pValues = new double[] {1.4, 2.4, 3.4};
		const bool hasIntercept = false;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final LeastSquaresRegressionResult result = new LeastSquaresRegressionResult(betas, residuals, meanSquareError, standardErrorOfBeta, rSquared, rSquaredAdjusted, tStats, pValues, hasIntercept);
		LeastSquaresRegressionResult result = new LeastSquaresRegressionResult(betas, residuals, meanSquareError, standardErrorOfBeta, rSquared, rSquaredAdjusted, tStats, pValues, hasIntercept);
		LeastSquaresRegressionResult other = new LeastSquaresRegressionResult(result);
		assertEquals(result, other);
		assertEquals(result.GetHashCode(), other.GetHashCode());
		other = new LeastSquaresRegressionResult(betas, residuals, meanSquareError, standardErrorOfBeta, rSquared, rSquaredAdjusted, tStats, pValues, hasIntercept);
		assertEquals(result, other);
		assertEquals(result.GetHashCode(), other.GetHashCode());
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] x = new double[] {1.5, 2.5, 3.5 };
		double[] x = new double[] {1.5, 2.5, 3.5};
		other = new LeastSquaresRegressionResult(x, residuals, meanSquareError, standardErrorOfBeta, rSquared, rSquaredAdjusted, tStats, pValues, hasIntercept);
		assertFalse(result.Equals(other));
		other = new LeastSquaresRegressionResult(betas, x, meanSquareError, standardErrorOfBeta, rSquared, rSquaredAdjusted, tStats, pValues, hasIntercept);
		assertFalse(result.Equals(other));
		other = new LeastSquaresRegressionResult(betas, residuals, meanSquareError + 1, standardErrorOfBeta, rSquared, rSquaredAdjusted, tStats, pValues, hasIntercept);
		assertFalse(result.Equals(other));
		other = new LeastSquaresRegressionResult(betas, residuals, meanSquareError, x, rSquared, rSquaredAdjusted, tStats, pValues, hasIntercept);
		assertFalse(result.Equals(other));
		other = new LeastSquaresRegressionResult(betas, residuals, meanSquareError, standardErrorOfBeta, rSquared + 1, rSquaredAdjusted, tStats, pValues, hasIntercept);
		assertFalse(result.Equals(other));
		other = new LeastSquaresRegressionResult(betas, residuals, meanSquareError, standardErrorOfBeta, rSquared, rSquaredAdjusted + 1, tStats, pValues, hasIntercept);
		assertFalse(result.Equals(other));
		other = new LeastSquaresRegressionResult(betas, residuals, meanSquareError, standardErrorOfBeta, rSquared, rSquaredAdjusted, x, pValues, hasIntercept);
		assertFalse(result.Equals(other));
		other = new LeastSquaresRegressionResult(betas, residuals, meanSquareError, standardErrorOfBeta, rSquared, rSquaredAdjusted, tStats, x, hasIntercept);
		assertFalse(result.Equals(other));
		other = new LeastSquaresRegressionResult(betas, residuals, meanSquareError, standardErrorOfBeta, rSquared, rSquaredAdjusted, tStats, pValues, !hasIntercept);
		assertFalse(result.Equals(other));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testGetters()
	  public virtual void testGetters()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] residuals = new double[] {1, 2, 3 };
		double[] residuals = new double[] {1, 2, 3};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] betas = new double[] {1.1, 2.1, 3.1 };
		double[] betas = new double[] {1.1, 2.1, 3.1};
		const double meanSquareError = 0.78;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] standardErrorOfBeta = new double[] {1.2, 2.2, 3.2 };
		double[] standardErrorOfBeta = new double[] {1.2, 2.2, 3.2};
		const double rSquared = 0.98;
		const double rSquaredAdjusted = 0.96;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] tStats = new double[] {1.3, 2.3, 3.3 };
		double[] tStats = new double[] {1.3, 2.3, 3.3};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] pValues = new double[] {1.4, 2.4, 3.4 };
		double[] pValues = new double[] {1.4, 2.4, 3.4};
		const bool hasIntercept = false;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final LeastSquaresRegressionResult result = new LeastSquaresRegressionResult(betas, residuals, meanSquareError, standardErrorOfBeta, rSquared, rSquaredAdjusted, tStats, pValues, hasIntercept);
		LeastSquaresRegressionResult result = new LeastSquaresRegressionResult(betas, residuals, meanSquareError, standardErrorOfBeta, rSquared, rSquaredAdjusted, tStats, pValues, hasIntercept);
		assertEquals(result.AdjustedRSquared, rSquaredAdjusted, 0);
		assertArrayEquals(result.Betas, betas, 0);
		assertEquals(result.MeanSquareError, meanSquareError, 0);
		assertArrayEquals(result.PValues, pValues, 0);
		assertArrayEquals(result.Residuals, residuals, 0);
		assertEquals(result.RSquared, rSquared, 0);
		assertArrayEquals(result.StandardErrorOfBetas, standardErrorOfBeta, 0);
		assertArrayEquals(result.TStatistics, tStats, 0);
	  }
	}

}