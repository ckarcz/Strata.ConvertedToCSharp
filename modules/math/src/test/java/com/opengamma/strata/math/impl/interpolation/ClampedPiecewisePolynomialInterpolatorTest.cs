using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using Function = com.google.common.@base.Function;
	using PiecewisePolynomialFunction1D = com.opengamma.strata.math.impl.function.PiecewisePolynomialFunction1D;

	/// <summary>
	/// Test <seealso cref="ClampedPiecewisePolynomialInterpolator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ClampedPiecewisePolynomialInterpolatorTest
	public class ClampedPiecewisePolynomialInterpolatorTest
	{
	  private static readonly double[] X_VALUES = new double[] {-1.0, -0.04, 0.1, 3.2, 15.0};
	  private static readonly double[] Y_VALUES = new double[] {12.4, -2.03, 11.41, 11.0, 0.2};
	  private static readonly double[] X_CLAMPED = new double[] {-1.5, 3.4, 22.0, 0.0};
	  private static readonly double[] Y_CLAMPED = new double[] {6.0, 2.2, 6.1, 3.2};
	  private static readonly double[] X_VALUES_TOTAL = new double[] {-1.5, -1.0, -0.04, 0.0, 0.1, 3.2, 3.4, 15.0, 22.0};
	  private static readonly double[] Y_VALUES_TOTAL = new double[] {6.0, 12.4, -2.03, 3.2, 11.41, 11.0, 2.2, 0.2, 6.1};
	  private static readonly PiecewisePolynomialInterpolator[] BASE_INTERP = new PiecewisePolynomialInterpolator[]
	  {
		  new NaturalSplineInterpolator(),
		  new PiecewiseCubicHermiteSplineInterpolatorWithSensitivity(),
		  new MonotonicityPreservingCubicSplineInterpolator(new CubicSplineInterpolator())
	  };
	  private const double TOL = 1.0e-14;

	  public virtual void testInterpolate()
	  {
		foreach (PiecewisePolynomialInterpolator baseInterp in BASE_INTERP)
		{
		  ClampedPiecewisePolynomialInterpolator interp = new ClampedPiecewisePolynomialInterpolator(baseInterp, X_CLAMPED, Y_CLAMPED);
		  PiecewisePolynomialResult computed = interp.interpolate(X_VALUES, Y_VALUES);
		  PiecewisePolynomialResult expected = baseInterp.interpolate(X_VALUES_TOTAL, Y_VALUES_TOTAL);
		  assertEquals(computed, expected);
		  assertEquals(interp.PrimaryMethod, baseInterp);
		}
	  }

	  public virtual void testInterpolateWithSensitivity()
	  {
		foreach (PiecewisePolynomialInterpolator baseInterp in BASE_INTERP)
		{
		  ClampedPiecewisePolynomialInterpolator interp = new ClampedPiecewisePolynomialInterpolator(baseInterp, X_CLAMPED, Y_CLAMPED);
		  PiecewisePolynomialResultsWithSensitivity computed = interp.interpolateWithSensitivity(X_VALUES, Y_VALUES);
		  PiecewisePolynomialResultsWithSensitivity expected = baseInterp.interpolateWithSensitivity(X_VALUES_TOTAL, Y_VALUES_TOTAL);
		  assertEquals(computed, expected);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = UnsupportedOperationException.class) public void testInterpolateMultiDim()
	  public virtual void testInterpolateMultiDim()
	  {
		ClampedPiecewisePolynomialInterpolator interp = new ClampedPiecewisePolynomialInterpolator(new NaturalSplineInterpolator(), new double[] {1d}, new double[] {2d});
		interp.interpolate(X_VALUES, new double[][] {Y_VALUES, Y_VALUES});
	  }

	  public virtual void testWrongClampedPoints()
	  {
		assertThrowsIllegalArg(() => new ClampedPiecewisePolynomialInterpolator(new NaturalSplineInterpolator(), new double[] {0d}, new double[] {0d, 1d}));
		assertThrowsIllegalArg(() => new ClampedPiecewisePolynomialInterpolator(new CubicSplineInterpolator(), new double[] {}, new double[] {}));
	  }

	  public virtual void testFunctionalForm()
	  {
		double[] xValues = new double[] {0.5, 1.0, 3.0, 5.0, 10.0, 30.0};
		double lambda0 = 0.14;
		double[] lambda = new double[] {0.25, 0.05, -0.12, 0.03, -0.15, 0.0};
		double pValueTmp = 0d;
		int nData = xValues.Length;
		for (int i = 0; i < nData - 1; ++i)
		{
		  lambda[nData - 1] += lambda[i] * xValues[i];
		  pValueTmp += lambda[i];
		}
		lambda[nData - 1] *= -1d / xValues[nData - 1];
		pValueTmp += lambda[nData - 1];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double pValue = pValueTmp;
		double pValue = pValueTmp;
		Function<double, double> func = new FunctionAnonymousInnerClass(this, xValues, lambda0, lambda, nData, pValue);
		double[] rt = new double[nData];
		for (int i = 0; i < nData; ++i)
		{
		  rt[i] = func.apply(xValues[i]);
		}
		ClampedPiecewisePolynomialInterpolator interp = new ClampedPiecewisePolynomialInterpolator(BASE_INTERP[0], new double[] {0d}, new double[] {0d});
		PiecewisePolynomialResult result = interp.interpolate(xValues, rt);
		PiecewisePolynomialFunction1D polyFunc = new PiecewisePolynomialFunction1D();
		for (int i = 0; i < 600; ++i)
		{
		  double tm = 0.05 * i;
		  double exp = func.apply(tm);
		  assertEquals(exp, polyFunc.evaluate(result, tm).get(0), Math.Abs(exp) * TOL);
		}
	  }

	  private class FunctionAnonymousInnerClass : Function<double, double>
	  {
		  private readonly ClampedPiecewisePolynomialInterpolatorTest outerInstance;

		  private double[] xValues;
		  private double lambda0;
		  private double[] lambda;
		  private int nData;
		  private double pValue;

		  public FunctionAnonymousInnerClass(ClampedPiecewisePolynomialInterpolatorTest outerInstance, double[] xValues, double lambda0, double[] lambda, int nData, double pValue)
		  {
			  this.outerInstance = outerInstance;
			  this.xValues = xValues;
			  this.lambda0 = lambda0;
			  this.lambda = lambda;
			  this.nData = nData;
			  this.pValue = pValue;
		  }

		  public override double? apply(double? t)
		  {
			int index = 0;
			double res = lambda0 * t - pValue * Math.Pow(t, 3) / 6.0;
			while (index < nData && t.Value > xValues[index])
			{
			  res += lambda[index] * Math.Pow(t - xValues[index], 3) / 6.0;
			  ++index;
			}
			return res;
		  }
	  }

	}

}