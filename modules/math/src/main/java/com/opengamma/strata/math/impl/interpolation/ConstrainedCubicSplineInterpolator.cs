using System;

/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{

	using Doubles = com.google.common.primitives.Doubles;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Cubic spline interpolation based on
	/// C.J.C. Kruger, "Constrained Cubic Spline Interpolation for Chemical Engineering Applications," 2002
	/// </summary>
	public class ConstrainedCubicSplineInterpolator : PiecewisePolynomialInterpolator
	{
	  private const double ERROR = 1.e-13;
	  private readonly HermiteCoefficientsProvider _solver = new HermiteCoefficientsProvider();

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public PiecewisePolynomialResult interpolate(final double[] xValues, final double[] yValues)
	  public override PiecewisePolynomialResult interpolate(double[] xValues, double[] yValues)
	  {

		ArgChecker.notNull(xValues, "xValues");
		ArgChecker.notNull(yValues, "yValues");

		ArgChecker.isTrue(xValues.Length == yValues.Length, "(xValues length = yValues length) should be true");
		ArgChecker.isTrue(xValues.Length > 1, "Data points should be >= 2");

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nDataPts = xValues.length;
		int nDataPts = xValues.Length;

		for (int i = 0; i < nDataPts; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(xValues[i]), "xValues containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(xValues[i]), "xValues containing Infinity");
		  ArgChecker.isFalse(double.IsNaN(yValues[i]), "yValues containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(yValues[i]), "yValues containing Infinity");
		}

		for (int i = 0; i < nDataPts - 1; ++i)
		{
		  for (int j = i + 1; j < nDataPts; ++j)
		  {
			ArgChecker.isFalse(xValues[i] == xValues[j], "xValues should be distinct");
		  }
		}

		double[] xValuesSrt = Arrays.copyOf(xValues, nDataPts);
		double[] yValuesSrt = Arrays.copyOf(yValues, nDataPts);
		DoubleArrayMath.sortPairs(xValuesSrt, yValuesSrt);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] intervals = _solver.intervalsCalculator(xValuesSrt);
		double[] intervals = _solver.intervalsCalculator(xValuesSrt);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] slopes = _solver.slopesCalculator(yValuesSrt, intervals);
		double[] slopes = _solver.slopesCalculator(yValuesSrt, intervals);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] first = firstDerivativeCalculator(slopes);
		double[] first = firstDerivativeCalculator(slopes);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefs = _solver.solve(yValuesSrt, intervals, slopes, first);
		double[][] coefs = _solver.solve(yValuesSrt, intervals, slopes, first);

		for (int i = 0; i < nDataPts - 1; ++i)
		{
		  double @ref = 0.0;
		  for (int j = 0; j < 4; ++j)
		  {
			@ref += coefs[i][j] * Math.Pow(intervals[i], 3 - j);
			ArgChecker.isFalse(double.IsNaN(coefs[i][j]), "Too large input");
			ArgChecker.isFalse(double.IsInfinity(coefs[i][j]), "Too large input");
		  }
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double bound = Math.max(Math.abs(ref) + Math.abs(yValuesSrt[i + 1]), 1.e-1);
		  double bound = Math.Max(Math.Abs(@ref) + Math.Abs(yValuesSrt[i + 1]), 1.e-1);
		  ArgChecker.isTrue(Math.Abs(@ref - yValuesSrt[i + 1]) < ERROR * bound, "Input is too large/small or data points are too close");
		}

		return new PiecewisePolynomialResult(DoubleArray.copyOf(xValuesSrt), DoubleMatrix.copyOf(coefs), 4, 1);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public PiecewisePolynomialResult interpolate(final double[] xValues, final double[][] yValuesMatrix)
	  public override PiecewisePolynomialResult interpolate(double[] xValues, double[][] yValuesMatrix)
	  {
		ArgChecker.notNull(xValues, "xValues");
		ArgChecker.notNull(yValuesMatrix, "yValuesMatrix");

		ArgChecker.isTrue(xValues.Length == yValuesMatrix[0].Length, "(xValues length = yValuesMatrix's row vector length) should be true");
		ArgChecker.isTrue(xValues.Length > 1, "Data points should be >= 2");

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nDataPts = xValues.length;
		int nDataPts = xValues.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int yValuesLen = yValuesMatrix[0].length;
		int yValuesLen = yValuesMatrix[0].Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int dim = yValuesMatrix.length;
		int dim = yValuesMatrix.Length;

		for (int i = 0; i < nDataPts; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(xValues[i]), "xValues containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(xValues[i]), "xValues containing Infinity");
		}
		for (int i = 0; i < yValuesLen; ++i)
		{
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

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] intervals = _solver.intervalsCalculator(xValuesSrt);
		  double[] intervals = _solver.intervalsCalculator(xValuesSrt);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] slopes = _solver.slopesCalculator(yValuesSrt, intervals);
		  double[] slopes = _solver.slopesCalculator(yValuesSrt, intervals);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] first = firstDerivativeCalculator(slopes);
		  double[] first = firstDerivativeCalculator(slopes);

		  coefMatrix[i] = DoubleMatrix.copyOf(_solver.solve(yValuesSrt, intervals, slopes, first));

		  for (int k = 0; k < intervals.Length; ++k)
		  {
			double @ref = 0.0;
			for (int j = 0; j < 4; ++j)
			{
			  @ref += coefMatrix[i].get(k, j) * Math.Pow(intervals[k], 3 - j);
			  ArgChecker.isFalse(double.IsNaN(coefMatrix[i].get(k, j)), "Too large input");
			  ArgChecker.isFalse(double.IsInfinity(coefMatrix[i].get(k, j)), "Too large input");
			}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double bound = Math.max(Math.abs(ref) + Math.abs(yValuesSrt[k + 1]), 1.e-1);
			double bound = Math.Max(Math.Abs(@ref) + Math.Abs(yValuesSrt[k + 1]), 1.e-1);
			ArgChecker.isTrue(Math.Abs(@ref - yValuesSrt[k + 1]) < ERROR * bound, "Input is too large/small or data points are too close");
		  }
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

		return new PiecewisePolynomialResult(DoubleArray.copyOf(xValuesSrt), DoubleMatrix.copyOf(resMatrix), nCoefs, dim);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public PiecewisePolynomialResultsWithSensitivity interpolateWithSensitivity(final double[] xValues, final double[] yValues)
	  public override PiecewisePolynomialResultsWithSensitivity interpolateWithSensitivity(double[] xValues, double[] yValues)
	  {
		ArgChecker.notNull(xValues, "xValues");
		ArgChecker.notNull(yValues, "yValues");

		ArgChecker.isTrue(xValues.Length == yValues.Length, "(xValues length = yValues length) should be true");
		ArgChecker.isTrue(xValues.Length > 1, "Data points should be >= 2");

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nDataPts = xValues.length;
		int nDataPts = xValues.Length;

		for (int i = 0; i < nDataPts; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(xValues[i]), "xValues containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(xValues[i]), "xValues containing Infinity");
		  ArgChecker.isFalse(double.IsNaN(yValues[i]), "yValues containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(yValues[i]), "yValues containing Infinity");
		}

		for (int i = 0; i < nDataPts - 1; ++i)
		{
		  for (int j = i + 1; j < nDataPts; ++j)
		  {
			ArgChecker.isFalse(xValues[i] == xValues[j], "xValues should be distinct");
		  }
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] intervals = _solver.intervalsCalculator(xValues);
		double[] intervals = _solver.intervalsCalculator(xValues);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] slopes = _solver.slopesCalculator(yValues, intervals);
		double[] slopes = _solver.slopesCalculator(yValues, intervals);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] slopeSensitivity = _solver.slopeSensitivityCalculator(intervals);
		double[][] slopeSensitivity = _solver.slopeSensitivityCalculator(intervals);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray[] firstWithSensitivity = firstDerivativeWithSensitivityCalculator(slopes, slopeSensitivity);
		DoubleArray[] firstWithSensitivity = firstDerivativeWithSensitivityCalculator(slopes, slopeSensitivity);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix[] resMatrix = _solver.solveWithSensitivity(yValues, intervals, slopes, slopeSensitivity, firstWithSensitivity);
		DoubleMatrix[] resMatrix = _solver.solveWithSensitivity(yValues, intervals, slopes, slopeSensitivity, firstWithSensitivity);

		for (int k = 0; k < nDataPts; k++)
		{
		  DoubleMatrix m = resMatrix[k];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int rows = m.rowCount();
		  int rows = m.rowCount();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int cols = m.columnCount();
		  int cols = m.columnCount();
		  for (int i = 0; i < rows; ++i)
		  {
			for (int j = 0; j < cols; ++j)
			{
			  ArgChecker.isTrue(Doubles.isFinite(m.get(i, j)), "Matrix contains a NaN or infinite");
			}
		  }
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix coefMatrix = resMatrix[0];
		DoubleMatrix coefMatrix = resMatrix[0];
		for (int i = 0; i < nDataPts - 1; ++i)
		{
		  double @ref = 0.0;
		  for (int j = 0; j < 4; ++j)
		  {
			@ref += coefMatrix.get(i, j) * Math.Pow(intervals[i], 3 - j);
		  }
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double bound = Math.max(Math.abs(ref) + Math.abs(yValues[i + 1]), 1.e-1);
		  double bound = Math.Max(Math.Abs(@ref) + Math.Abs(yValues[i + 1]), 1.e-1);
		  ArgChecker.isTrue(Math.Abs(@ref - yValues[i + 1]) < ERROR * bound, "Input is too large/small or data points are too close");
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix[] coefSenseMatrix = new com.opengamma.strata.collect.array.DoubleMatrix[nDataPts - 1];
		DoubleMatrix[] coefSenseMatrix = new DoubleMatrix[nDataPts - 1];
		Array.Copy(resMatrix, 1, coefSenseMatrix, 0, nDataPts - 1);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nCoefs = coefMatrix.columnCount();
		int nCoefs = coefMatrix.columnCount();

		return new PiecewisePolynomialResultsWithSensitivity(DoubleArray.copyOf(xValues), coefMatrix, nCoefs, 1, coefSenseMatrix);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private double[] firstDerivativeCalculator(final double[] slopes)
	  private double[] firstDerivativeCalculator(double[] slopes)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData = slopes.length + 1;
		int nData = slopes.Length + 1;
		double[] res = new double[nData];

		for (int i = 1; i < nData - 1; ++i)
		{
		  res[i] = Math.Sign(slopes[i - 1]) * Math.Sign(slopes[i]) <= 0.0 ? 0.0 : 2.0 * slopes[i] * slopes[i - 1] / (slopes[i] + slopes[i - 1]);
		}
		res[0] = 1.5 * slopes[0] - 0.5 * res[1];
		res[nData - 1] = 1.5 * slopes[nData - 2] - 0.5 * res[nData - 2];

		return res;
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.array.DoubleArray[] firstDerivativeWithSensitivityCalculator(final double[] slopes, final double[][] slopeSensitivity)
	  private DoubleArray[] firstDerivativeWithSensitivityCalculator(double[] slopes, double[][] slopeSensitivity)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData = slopes.length + 1;
		int nData = slopes.Length + 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] first = new double[nData];
		double[] first = new double[nData];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] sense = new double[nData][nData];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] sense = new double[nData][nData];
		double[][] sense = RectangularArrays.ReturnRectangularDoubleArray(nData, nData);
		DoubleArray[] res = new DoubleArray[nData + 1];

		for (int i = 1; i < nData - 1; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sign = Math.signum(slopes[i - 1]) * Math.signum(slopes[i]);
		  double sign = Math.Sign(slopes[i - 1]) * Math.Sign(slopes[i]);
		  if (sign <= 0.0)
		  {
			first[i] = 0.0;
		  }
		  else
		  {
			first[i] = 2.0 * slopes[i] * slopes[i - 1] / (slopes[i] + slopes[i - 1]);
		  }
		  if (sign < 0.0)
		  {
			Arrays.fill(sense[i], 0.0);
		  }
		  else
		  {
			for (int k = 0; k < nData; ++k)
			{
			  if (Math.Abs(slopes[i] + slopes[i - 1]) == 0.0)
			  {
				Arrays.fill(sense[i], 0.0);
			  }
			  else
			  {
				if (sign == 0.0)
				{
				  sense[i][k] = (slopes[i] * slopes[i] * slopeSensitivity[i - 1][k] + slopes[i - 1] * slopes[i - 1] * slopeSensitivity[i][k]) / (slopes[i] + slopes[i - 1]) / (slopes[i] + slopes[i - 1]);
				}
				else
				{
				  sense[i][k] = 2.0 * (slopes[i] * slopes[i] * slopeSensitivity[i - 1][k] + slopes[i - 1] * slopes[i - 1] * slopeSensitivity[i][k]) / (slopes[i] + slopes[i - 1]) / (slopes[i] + slopes[i - 1]);
				}
			  }
			}
		  }
		  res[i + 1] = DoubleArray.copyOf(sense[i]);
		}
		first[0] = 1.5 * slopes[0] - 0.5 * first[1];
		first[nData - 1] = 1.5 * slopes[nData - 2] - 0.5 * first[nData - 2];
		res[0] = DoubleArray.copyOf(first);
		for (int k = 0; k < nData; ++k)
		{
		  sense[0][k] = 1.5 * slopeSensitivity[0][k] - 0.5 * sense[1][k];
		  sense[nData - 1][k] = 1.5 * slopeSensitivity[nData - 2][k] - 0.5 * sense[nData - 2][k];
		}
		res[1] = DoubleArray.copyOf(sense[0]);
		res[nData] = DoubleArray.copyOf(sense[nData - 1]);

		return res;
	  }
	}

}