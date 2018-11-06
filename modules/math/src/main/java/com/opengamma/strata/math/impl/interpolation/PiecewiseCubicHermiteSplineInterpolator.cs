using System;

/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// C1 cubic interpolation preserving monotonicity based on 
	/// Fritsch, F. N.; Carlson, R. E. (1980) 
	/// "Monotone Piecewise Cubic Interpolation", SIAM Journal on Numerical Analysis 17 (2): 238–246. 
	/// Fritsch, F. N. and Butland, J. (1984)
	/// "A method for constructing local monotone piecewise cubic interpolants", SIAM Journal on Scientific and Statistical Computing 5 (2): 300-304.
	/// </summary>
	public class PiecewiseCubicHermiteSplineInterpolator : PiecewisePolynomialInterpolator
	{

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public PiecewisePolynomialResult interpolate(final double[] xValues, final double[] yValues)
	  public override PiecewisePolynomialResult interpolate(double[] xValues, double[] yValues)
	  {

		ArgChecker.notNull(xValues, "xValues");
		ArgChecker.notNull(yValues, "yValues");

		ArgChecker.isTrue(xValues.Length == yValues.Length, "xValues length = yValues length");
		ArgChecker.isTrue(xValues.Length > 1, "Data points should be more than 1");

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nDataPts = xValues.length;
		int nDataPts = xValues.Length;

		for (int i = 0; i < nDataPts; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(xValues[i]), "xData containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(xValues[i]), "xData containing Infinity");
		  ArgChecker.isFalse(double.IsNaN(yValues[i]), "yData containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(yValues[i]), "yData containing Infinity");
		}

		double[] xValuesSrt = Arrays.copyOf(xValues, nDataPts);
		double[] yValuesSrt = Arrays.copyOf(yValues, nDataPts);
		DoubleArrayMath.sortPairs(xValuesSrt, yValuesSrt);

		for (int i = 1; i < nDataPts; ++i)
		{
		  ArgChecker.isFalse(xValuesSrt[i - 1] == xValuesSrt[i], "xValues should be distinct");
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix coefMatrix = solve(xValuesSrt, yValuesSrt);
		DoubleMatrix coefMatrix = solve(xValuesSrt, yValuesSrt);

		for (int i = 0; i < coefMatrix.rowCount(); ++i)
		{
		  for (int j = 0; j < coefMatrix.columnCount(); ++j)
		  {
			ArgChecker.isFalse(double.IsNaN(coefMatrix.get(i, j)), "Too large input");
			ArgChecker.isFalse(double.IsInfinity(coefMatrix.get(i, j)), "Too large input");
		  }
		}

		return new PiecewisePolynomialResult(DoubleArray.copyOf(xValuesSrt), coefMatrix, coefMatrix.columnCount(), 1);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public PiecewisePolynomialResult interpolate(final double[] xValues, final double[][] yValuesMatrix)
	  public override PiecewisePolynomialResult interpolate(double[] xValues, double[][] yValuesMatrix)
	  {

		ArgChecker.notNull(xValues, "xValues");
		ArgChecker.notNull(yValuesMatrix, "yValuesMatrix");

		ArgChecker.isTrue(xValues.Length == yValuesMatrix[0].Length, "(xValues length = yValuesMatrix's row vector length)");
		ArgChecker.isTrue(xValues.Length > 1, "Data points should be more than 1");

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nDataPts = xValues.length;
		int nDataPts = xValues.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int dim = yValuesMatrix.length;
		int dim = yValuesMatrix.Length;

		for (int i = 0; i < nDataPts; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(xValues[i]), "xValues containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(xValues[i]), "xValues containing Infinity");
		  for (int j = 0; j < dim; ++j)
		  {
			ArgChecker.isFalse(double.IsNaN(yValuesMatrix[j][i]), "yValuesMatrix containing NaN");
			ArgChecker.isFalse(double.IsInfinity(yValuesMatrix[j][i]), "yValuesMatrix containing Infinity");
		  }
		}

		for (int i = 0; i < nDataPts; ++i)
		{
		  for (int j = i + 1; j < nDataPts; ++j)
		  {
			ArgChecker.isFalse(xValues[i] == xValues[j], "xValues should be distinct");
		  }
		}

		double[] xValuesSrt = new double[nDataPts];
		DoubleMatrix[] coefMatrix = new DoubleMatrix[dim];

		for (int i = 0; i < dim; ++i)
		{
		  xValuesSrt = Arrays.copyOf(xValues, nDataPts);
		  double[] yValuesSrt = Arrays.copyOf(yValuesMatrix[i], nDataPts);
		  DoubleArrayMath.sortPairs(xValuesSrt, yValuesSrt);

		  coefMatrix[i] = solve(xValuesSrt, yValuesSrt);
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nIntervals = coefMatrix[0].rowCount();
		int nIntervals = coefMatrix[0].rowCount();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nCoefs = coefMatrix[0].columnCount();
		int nCoefs = coefMatrix[0].columnCount();
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] resMatrix = new double[dim * nIntervals][nCoefs];
		double[][] resMatrix = RectangularArrays.ReturnRectangularDoubleArray(dim * nIntervals, nCoefs);

		for (int i = 0; i < nIntervals; ++i)
		{
		  for (int j = 0; j < dim; ++j)
		  {
			resMatrix[dim * i + j] = coefMatrix[j].row(i).toArray();
		  }
		}

		for (int i = 0; i < (nIntervals * dim); ++i)
		{
		  for (int j = 0; j < nCoefs; ++j)
		  {
			ArgChecker.isFalse(double.IsNaN(resMatrix[i][j]), "Too large input");
			ArgChecker.isFalse(double.IsInfinity(resMatrix[i][j]), "Too large input");
		  }
		}

		return new PiecewisePolynomialResult(DoubleArray.copyOf(xValuesSrt), DoubleMatrix.copyOf(resMatrix), nCoefs, dim);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public PiecewisePolynomialResultsWithSensitivity interpolateWithSensitivity(final double[] xValues, final double[] yValues)
	  public override PiecewisePolynomialResultsWithSensitivity interpolateWithSensitivity(double[] xValues, double[] yValues)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PiecewiseCubicHermiteSplineInterpolatorWithSensitivity interp = new PiecewiseCubicHermiteSplineInterpolatorWithSensitivity();
		PiecewiseCubicHermiteSplineInterpolatorWithSensitivity interp = new PiecewiseCubicHermiteSplineInterpolatorWithSensitivity();
		return interp.interpolateWithSensitivity(xValues, yValues);
	  }

	  /// <param name="xValues"> X values of data </param>
	  /// <param name="yValues"> Y values of data </param>
	  /// <returns> Coefficient matrix whose i-th row vector is {a3, a2, a1, a0} of f(x) = a3 * (x-x_i)^3 + a2 * (x-x_i)^2 +... for the i-th interval </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.array.DoubleMatrix solve(final double[] xValues, final double[] yValues)
	  private DoubleMatrix solve(double[] xValues, double[] yValues)
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nDataPts = xValues.length;
		int nDataPts = xValues.Length;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[nDataPts - 1][4];
		double[][] res = RectangularArrays.ReturnRectangularDoubleArray(nDataPts - 1, 4);
		double[] intervals = new double[nDataPts - 1];
		double[] grads = new double[nDataPts - 1];

		for (int i = 0; i < nDataPts - 1; ++i)
		{
		  intervals[i] = xValues[i + 1] - xValues[i];
		  grads[i] = (yValues[i + 1] - yValues[i]) / intervals[i];
		}

		if (nDataPts == 2)
		{
		  res[0][2] = grads[0];
		  res[0][3] = yValues[0];
		}
		else
		{
		  double[] derivatives = slopeFinder(intervals, grads);
		  for (int i = 0; i < nDataPts - 1; ++i)
		  {
			res[i][0] = (derivatives[i] - 2 * grads[i] + derivatives[i + 1]) / intervals[i] / intervals[i];
			res[i][1] = (3 * grads[i] - 2.0 * derivatives[i] - derivatives[i + 1]) / intervals[i];
			res[i][2] = derivatives[i];
			res[i][3] = yValues[i];
		  }
		}
		return DoubleMatrix.copyOf(res);
	  }

	  // calculates a set of the first derivatives at knots
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private double[] slopeFinder(final double[] intervals, final double[] grads)
	  private double[] slopeFinder(double[] intervals, double[] grads)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nInts = intervals.length;
		int nInts = intervals.Length;
		double[] res = new double[nInts + 1];

		res[0] = endpointSlope(intervals[0], intervals[1], grads[0], grads[1]);
		res[nInts] = endpointSlope(intervals[nInts - 1], intervals[nInts - 2], grads[nInts - 1], grads[nInts - 2]);

		for (int i = 1; i < nInts; ++i)
		{
		  if (grads[i] * grads[i - 1] <= 0)
		  {
			res[i] = 0.0;
		  }
		  else
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double den1 = 2.0 * intervals[i] + intervals[i - 1];
			double den1 = 2.0 * intervals[i] + intervals[i - 1];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double den2 = intervals[i] + 2.0 * intervals[i - 1];
			double den2 = intervals[i] + 2.0 * intervals[i - 1];
			res[i] = (den1 + den2) / (den1 / grads[i - 1] + den2 / grads[i]);
		  }
		}

		return res;
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private double endpointSlope(final double ints1, final double ints2, final double grads1, final double grads2)
	  private double endpointSlope(double ints1, double ints2, double grads1, double grads2)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double val = ((2.0 * ints1 + ints2) * grads1 - ints1 * grads2) / (ints1 + ints2);
		double val = ((2.0 * ints1 + ints2) * grads1 - ints1 * grads2) / (ints1 + ints2);

		if (Math.Sign(val) != Math.Sign(grads1))
		{
		  return 0.0;
		}
		else if (Math.Sign(grads1) != Math.Sign(grads2) && Math.Abs(val) > 3.0 * Math.Abs(grads1))
		{
		  return 3.0 * grads1;
		}
		return val;
	  }

	}

}