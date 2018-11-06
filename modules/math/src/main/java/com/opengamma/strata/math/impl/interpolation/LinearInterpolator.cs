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
	/// Interpolate consecutive two points by a straight line. 
	/// </summary>
	public class LinearInterpolator : PiecewisePolynomialInterpolator
	{

	  private const double ERROR = 1.e-13;

	  public override PiecewisePolynomialResult interpolate(double[] xValues, double[] yValues)
	  {
		ArgChecker.notEmpty(xValues, "xValues");
		ArgChecker.notEmpty(yValues, "yValues");
		int nDataPts = xValues.Length;
		ArgChecker.isTrue(nDataPts > 1, "at least two data points required");
		ArgChecker.isTrue(nDataPts == yValues.Length, "xValues length = yValues length");
		for (int i = 0; i < nDataPts; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(xValues[i]), "xData containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(xValues[i]), "xData containing Infinity");
		  ArgChecker.isFalse(double.IsNaN(yValues[i]), "yData containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(yValues[i]), "yData containing Infinity");
		}
		if (nDataPts == 1)
		{
		  return new PiecewisePolynomialResult(DoubleArray.copyOf(xValues), DoubleMatrix.filled(1, 1, yValues[0]), 1, 1);
		}

		for (int i = 0; i < nDataPts; ++i)
		{
		  for (int j = i + 1; j < nDataPts; ++j)
		  {
			ArgChecker.isFalse(xValues[i] == xValues[j], "xValues should be distinct");
		  }
		}

		double[] xValuesSrt = Arrays.copyOf(xValues, nDataPts);
		double[] yValuesSrt = Arrays.copyOf(yValues, nDataPts);
		DoubleArrayMath.sortPairs(xValuesSrt, yValuesSrt);

		DoubleMatrix coefMatrix = solve(xValuesSrt, yValuesSrt);

		for (int i = 0; i < coefMatrix.rowCount(); ++i)
		{
		  for (int j = 0; j < coefMatrix.columnCount(); ++j)
		  {
			ArgChecker.isFalse(double.IsNaN(coefMatrix.get(i, j)), "Too large input");
			ArgChecker.isFalse(double.IsInfinity(coefMatrix.get(i, j)), "Too large input");
		  }
		  double @ref = 0.0;
		  double interval = xValuesSrt[i + 1] - xValuesSrt[i];
		  for (int j = 0; j < 2; ++j)
		  {
			@ref += coefMatrix.get(i, j) * Math.Pow(interval, 1 - j);
			ArgChecker.isFalse(double.IsNaN(coefMatrix.get(i, j)), "Too large input");
			ArgChecker.isFalse(double.IsInfinity(coefMatrix.get(i, j)), "Too large input");
		  }
		  double bound = Math.Max(Math.Abs(@ref) + Math.Abs(yValuesSrt[i + 1]), 1.e-1);
		  ArgChecker.isTrue(Math.Abs(@ref - yValuesSrt[i + 1]) < ERROR * bound, "Input is too large/small or data are not distinct enough");
		}

		return new PiecewisePolynomialResult(DoubleArray.copyOf(xValuesSrt), coefMatrix, coefMatrix.columnCount(), 1);
	  }

	  public override PiecewisePolynomialResult interpolate(double[] xValues, double[][] yValuesMatrix)
	  {

		ArgChecker.notEmpty(xValues, "xValues");
		ArgChecker.notEmpty(yValuesMatrix, "yValuesMatrix");

		int nDataPts = xValues.Length;
		ArgChecker.isTrue(nDataPts > 1, "at least two data points required");
		ArgChecker.isTrue(nDataPts == yValuesMatrix[0].Length, "(xValues length = yValuesMatrix's row vector length)");
		int dim = yValuesMatrix.Length;
		for (int i = 0; i < nDataPts; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(xValues[i]), "xData containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(xValues[i]), "xData containing Infinity");
		  for (int j = 0; j < dim; ++j)
		  {
			ArgChecker.isFalse(double.IsNaN(yValuesMatrix[j][i]), "yValuesMatrix containing NaN");
			ArgChecker.isFalse(double.IsInfinity(yValuesMatrix[j][i]), "yValuesMatrix containing Infinity");
		  }
		}
		for (int k = 0; k < dim; ++k)
		{
		  for (int i = 0; i < nDataPts; ++i)
		  {
			for (int j = i + 1; j < nDataPts; ++j)
			{
			  ArgChecker.isFalse(xValues[i] == xValues[j], "xValues should be distinct");
			}
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

		  for (int k = 0; k < xValuesSrt.Length - 1; ++k)
		  {
			double @ref = 0.0;
			double interval = xValuesSrt[k + 1] - xValuesSrt[k];
			for (int j = 0; j < 2; ++j)
			{
			  @ref += coefMatrix[i].get(k, j) * Math.Pow(interval, 1 - j);
			  ArgChecker.isFalse(double.IsNaN(coefMatrix[i].get(k, j)), "Too large input");
			  ArgChecker.isFalse(double.IsInfinity(coefMatrix[i].get(k, j)), "Too large input");
			}
			double bound = Math.Max(Math.Abs(@ref) + Math.Abs(yValuesSrt[k + 1]), 1.e-1);
			ArgChecker.isTrue(Math.Abs(@ref - yValuesSrt[k + 1]) < ERROR * bound, "Input is too large/small or data points are too close");
		  }
		}

		int nIntervals = coefMatrix[0].rowCount();
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

		return new PiecewisePolynomialResult(DoubleArray.copyOf(xValuesSrt), DoubleMatrix.copyOf(resMatrix), nCoefs, dim);
	  }

	  public override PiecewisePolynomialResultsWithSensitivity interpolateWithSensitivity(double[] xValues, double[] yValues)
	  {
		ArgChecker.notEmpty(xValues, "xValues");
		ArgChecker.notEmpty(yValues, "yValues");
		int nDataPts = xValues.Length;
		ArgChecker.isTrue(nDataPts > 1, "at least two data points required");
		ArgChecker.isTrue(nDataPts == yValues.Length, "xValues length = yValues length");
		for (int i = 0; i < nDataPts; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(xValues[i]), "xData containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(xValues[i]), "xData containing Infinity");
		  ArgChecker.isFalse(double.IsNaN(yValues[i]), "yData containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(yValues[i]), "yData containing Infinity");
		}
		for (int i = 0; i < nDataPts; ++i)
		{
		  for (int j = i + 1; j < nDataPts; ++j)
		  {
			ArgChecker.isFalse(xValues[i] == xValues[j], "xValues should be distinct");
		  }
		}

		double[] xValuesSrt = Arrays.copyOf(xValues, nDataPts);
		double[] yValuesSrt = Arrays.copyOf(yValues, nDataPts);
		DoubleArrayMath.sortPairs(xValuesSrt, yValuesSrt);

		DoubleMatrix[] res = solveSensitivity(xValuesSrt, yValuesSrt);
		DoubleMatrix coefMatrix = res[nDataPts - 1];
		DoubleMatrix[] coefSenseMatrix = Arrays.copyOf(res, nDataPts - 1);

		for (int i = 0; i < coefMatrix.rowCount(); ++i)
		{
		  for (int j = 0; j < coefMatrix.columnCount(); ++j)
		  {
			ArgChecker.isFalse(double.IsNaN(coefMatrix.get(i, j)), "Too large input");
			ArgChecker.isFalse(double.IsInfinity(coefMatrix.get(i, j)), "Too large input");
		  }
		  double @ref = 0.0;
		  double interval = xValuesSrt[i + 1] - xValuesSrt[i];
		  for (int j = 0; j < 2; ++j)
		  {
			@ref += coefMatrix.get(i, j) * Math.Pow(interval, 1 - j);
			ArgChecker.isFalse(double.IsNaN(coefMatrix.get(i, j)), "Too large input");
			ArgChecker.isFalse(double.IsInfinity(coefMatrix.get(i, j)), "Too large input");
		  }
		  double bound = Math.Max(Math.Abs(@ref) + Math.Abs(yValuesSrt[i + 1]), 1.e-1);
		  ArgChecker.isTrue(Math.Abs(@ref - yValuesSrt[i + 1]) < ERROR * bound, "Input is too large/small or data are not distinct enough");
		}

		return new PiecewisePolynomialResultsWithSensitivity(DoubleArray.ofUnsafe(xValuesSrt), coefMatrix, coefMatrix.columnCount(), 1, coefSenseMatrix);
	  }

	  /// <param name="xValues"> X values of data </param>
	  /// <param name="yValues"> Y values of data </param>
	  /// <returns> Coefficient matrix whose i-th row vector is {a1, a0} of f(x) = a1 * (x-x_i) + a0 for the i-th interval </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.array.DoubleMatrix solve(final double[] xValues, final double[] yValues)
	  private DoubleMatrix solve(double[] xValues, double[] yValues)
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nDataPts = xValues.length;
		int nDataPts = xValues.Length;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[nDataPts - 1][2];
		double[][] res = RectangularArrays.ReturnRectangularDoubleArray(nDataPts - 1, 2);

		for (int i = 0; i < nDataPts - 1; ++i)
		{
		  res[i][1] = yValues[i];
		  res[i][0] = (yValues[i + 1] - yValues[i]) / (xValues[i + 1] - xValues[i]);
		}

		return DoubleMatrix.copyOf(res);
	  }

	  /// <param name="xValues"> X values of data </param>
	  /// <param name="yValues"> Y values of data </param>
	  /// <returns> Coefficient matrix and coefficient sensitivity matrices </returns>
	  private DoubleMatrix[] solveSensitivity(double[] xValues, double[] yValues)
	  {

		int nDataPts = xValues.Length;
		DoubleMatrix[] res = new DoubleMatrix[nDataPts];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] coef = new double[nDataPts - 1][2];
		double[][] coef = RectangularArrays.ReturnRectangularDoubleArray(nDataPts - 1, 2);

		for (int i = 0; i < nDataPts - 1; ++i)
		{
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] coefSensi = new double[2][nDataPts];
		  double[][] coefSensi = RectangularArrays.ReturnRectangularDoubleArray(2, nDataPts);
		  double intervalInv = 1d / (xValues[i + 1] - xValues[i]);
		  coef[i][1] = yValues[i];
		  coef[i][0] = (yValues[i + 1] - yValues[i]) * intervalInv;
		  coefSensi[1][i] = 1d;
		  coefSensi[0][i] = -intervalInv;
		  coefSensi[0][i + 1] = intervalInv;
		  res[i] = DoubleMatrix.ofUnsafe(coefSensi);
		}
		res[nDataPts - 1] = DoubleMatrix.ofUnsafe(coef);

		return res;
	  }

	}

}