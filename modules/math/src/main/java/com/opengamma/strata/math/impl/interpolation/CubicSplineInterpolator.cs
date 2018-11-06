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
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// C2 cubic spline interpolator with Clamped/Not-A-Knot endpoint conditions.
	/// </summary>
	public class CubicSplineInterpolator : PiecewisePolynomialInterpolator
	{

	  private CubicSplineSolver _solver;

	  /// <summary>
	  /// If (xValues length) = (yValues length), Not-A-Knot endpoint conditions are used.
	  /// If (xValues length) + 2 = (yValues length), Clamped endpoint conditions are used. </summary>
	  /// <param name="xValues"> X values of data </param>
	  /// <param name="yValues"> Y values of data </param>
	  /// <returns> <seealso cref="PiecewisePolynomialResult"/> containing knots, coefficients of piecewise polynomials, number of intervals, degree of polynomials, dimension of spline </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public PiecewisePolynomialResult interpolate(final double[] xValues, final double[] yValues)
	  public override PiecewisePolynomialResult interpolate(double[] xValues, double[] yValues)
	  {

		ArgChecker.notNull(xValues, "xValues");
		ArgChecker.notNull(yValues, "yValues");

		ArgChecker.isTrue(xValues.Length == yValues.Length | xValues.Length + 2 == yValues.Length, "(xValues length = yValues length) or (xValues length + 2 = yValues length)");
		ArgChecker.isTrue(xValues.Length > 1, "Data points should be more than 1");

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nDataPts = xValues.length;
		int nDataPts = xValues.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nYdata = yValues.length;
		int nYdata = yValues.Length;

		for (int i = 0; i < nDataPts; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(xValues[i]), "xData containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(xValues[i]), "xData containing Infinity");
		}
		for (int i = 0; i < nYdata; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(yValues[i]), "yData containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(yValues[i]), "yData containing Infinity");
		}

		for (int i = 0; i < nDataPts; ++i)
		{
		  for (int j = i + 1; j < nDataPts; ++j)
		  {
			ArgChecker.isFalse(xValues[i] == xValues[j], "Data should be distinct");
		  }
		}

		double[] xValuesSrt = new double[nDataPts];
		double[] yValuesSrt = new double[nDataPts];

		xValuesSrt = Arrays.copyOf(xValues, nDataPts);

		if (xValues.Length + 2 == yValues.Length)
		{
		  _solver = new CubicSplineClampedSolver(yValues[0], yValues[nDataPts + 1]);
		  yValuesSrt = Arrays.copyOfRange(yValues, 1, nDataPts + 1);
		}
		else
		{
		  _solver = new CubicSplineNakSolver();
		  yValuesSrt = Arrays.copyOf(yValues, nDataPts);
		}
		DoubleArrayMath.sortPairs(xValuesSrt, yValuesSrt);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix coefMatrix = _solver.solve(xValuesSrt, yValuesSrt);
		DoubleMatrix coefMatrix = _solver.solve(xValuesSrt, yValuesSrt);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nCoefs = coefMatrix.columnCount();
		int nCoefs = coefMatrix.columnCount();

		for (int i = 0; i < _solver.getKnotsMat1D(xValuesSrt).size() - 1; ++i)
		{
		  for (int j = 0; j < nCoefs; ++j)
		  {
			ArgChecker.isFalse(double.IsNaN(coefMatrix.get(i, j)), "Too large input");
			ArgChecker.isFalse(double.IsInfinity(coefMatrix.get(i, j)), "Too large input");
		  }
		}

		return new PiecewisePolynomialResult(_solver.getKnotsMat1D(xValuesSrt), coefMatrix, nCoefs, 1);

	  }

	  /// <summary>
	  /// If (xValues length) = (yValuesMatrix NumberOfColumn), Not-A-Knot endpoint conditions are used.
	  /// If (xValues length) + 2 = (yValuesMatrix NumberOfColumn), Clamped endpoint conditions are used. </summary>
	  /// <param name="xValues"> X values of data </param>
	  /// <param name="yValuesMatrix"> Y values of data, where NumberOfRow defines dimension of the spline </param>
	  /// <returns> <seealso cref="PiecewisePolynomialResult"/> containing knots, coefficients of piecewise polynomials, number of intervals, degree of polynomials, dimension of spline </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public PiecewisePolynomialResult interpolate(final double[] xValues, final double[][] yValuesMatrix)
	  public override PiecewisePolynomialResult interpolate(double[] xValues, double[][] yValuesMatrix)
	  {

		ArgChecker.notNull(xValues, "xValues");
		ArgChecker.notNull(yValuesMatrix, "yValuesMatrix");

		ArgChecker.isTrue(xValues.Length == yValuesMatrix[0].Length | xValues.Length + 2 == yValuesMatrix[0].Length, "(xValues length = yValuesMatrix's row vector length) or (xValues length + 2 = yValuesMatrix's row vector length)");
		ArgChecker.isTrue(xValues.Length > 1, "Data points should be more than 1");

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nDataPts = xValues.length;
		int nDataPts = xValues.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nYdata = yValuesMatrix[0].length;
		int nYdata = yValuesMatrix[0].Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int dim = yValuesMatrix.length;
		int dim = yValuesMatrix.Length;

		for (int i = 0; i < nDataPts; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(xValues[i]), "xData containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(xValues[i]), "xData containing Infinity");
		}
		for (int i = 0; i < nYdata; ++i)
		{
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
			  ArgChecker.isFalse(xValues[i] == xValues[j], "Data should be distinct");
			}
		  }
		}

		double[] xValuesSrt = new double[nDataPts];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] yValuesMatrixSrt = new double[dim][nDataPts];
		double[][] yValuesMatrixSrt = RectangularArrays.ReturnRectangularDoubleArray(dim, nDataPts);

		if (xValues.Length + 2 == yValuesMatrix[0].Length)
		{
		  double[] iniConds = new double[dim];
		  double[] finConds = new double[dim];
		  for (int i = 0; i < dim; ++i)
		  {
			iniConds[i] = yValuesMatrix[i][0];
			finConds[i] = yValuesMatrix[i][nDataPts + 1];
		  }
		  _solver = new CubicSplineClampedSolver(iniConds, finConds);

		  for (int i = 0; i < dim; ++i)
		  {
			xValuesSrt = Arrays.copyOf(xValues, nDataPts);
			double[] yValuesSrt = Arrays.copyOfRange(yValuesMatrix[i], 1, nDataPts + 1);
			DoubleArrayMath.sortPairs(xValuesSrt, yValuesSrt);

			yValuesMatrixSrt[i] = Arrays.copyOf(yValuesSrt, nDataPts);
		  }
		}
		else
		{
		  _solver = new CubicSplineNakSolver();
		  for (int i = 0; i < dim; ++i)
		  {
			xValuesSrt = Arrays.copyOf(xValues, nDataPts);
			double[] yValuesSrt = Arrays.copyOf(yValuesMatrix[i], nDataPts);
			DoubleArrayMath.sortPairs(xValuesSrt, yValuesSrt);

			yValuesMatrixSrt[i] = Arrays.copyOf(yValuesSrt, nDataPts);
		  }
		}

		DoubleMatrix[] coefMatrix = _solver.solveMultiDim(xValuesSrt, DoubleMatrix.copyOf(yValuesMatrixSrt));

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

		for (int i = 0; i < dim * nIntervals; ++i)
		{
		  for (int j = 0; j < nCoefs; ++j)
		  {
			ArgChecker.isFalse(double.IsNaN(resMatrix[i][j]), "Too large input");
			ArgChecker.isFalse(double.IsInfinity(resMatrix[i][j]), "Too large input");
		  }
		}

		return new PiecewisePolynomialResult(_solver.getKnotsMat1D(xValuesSrt), DoubleMatrix.copyOf(resMatrix), nCoefs, dim);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public PiecewisePolynomialResultsWithSensitivity interpolateWithSensitivity(final double[] xValues, final double[] yValues)
	  public override PiecewisePolynomialResultsWithSensitivity interpolateWithSensitivity(double[] xValues, double[] yValues)
	  {
		ArgChecker.notNull(xValues, "xValues");
		ArgChecker.notNull(yValues, "yValues");

		ArgChecker.isTrue(xValues.Length == yValues.Length | xValues.Length + 2 == yValues.Length, "(xValues length = yValues length) or (xValues length + 2 = yValues length)");
		ArgChecker.isTrue(xValues.Length > 1, "Data points should be more than 1");

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nDataPts = xValues.length;
		int nDataPts = xValues.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nYdata = yValues.length;
		int nYdata = yValues.Length;

		for (int i = 0; i < nDataPts; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(xValues[i]), "xData containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(xValues[i]), "xData containing Infinity");
		}
		for (int i = 0; i < nYdata; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(yValues[i]), "yData containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(yValues[i]), "yData containing Infinity");
		}

		for (int i = 0; i < nDataPts; ++i)
		{
		  for (int j = i + 1; j < nDataPts; ++j)
		  {
			ArgChecker.isFalse(xValues[i] == xValues[j], "Data should be distinct");
		  }
		}

		double[] yValuesSrt = new double[nDataPts];

		if (xValues.Length + 2 == yValues.Length)
		{
		  _solver = new CubicSplineClampedSolver(yValues[0], yValues[nDataPts + 1]);
		  yValuesSrt = Arrays.copyOfRange(yValues, 1, nDataPts + 1);
		}
		else
		{
		  _solver = new CubicSplineNakSolver();
		  yValuesSrt = Arrays.copyOf(yValues, nDataPts);
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix[] resMatrix = _solver.solveWithSensitivity(xValues, yValuesSrt);
		DoubleMatrix[] resMatrix = _solver.solveWithSensitivity(xValues, yValuesSrt);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int len = resMatrix.length;
		int len = resMatrix.Length;
		for (int k = 0; k < len; k++)
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
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix[] coefSenseMatrix = new com.opengamma.strata.collect.array.DoubleMatrix[len - 1];
		DoubleMatrix[] coefSenseMatrix = new DoubleMatrix[len - 1];
		Array.Copy(resMatrix, 1, coefSenseMatrix, 0, len - 1);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nCoefs = coefMatrix.columnCount();
		int nCoefs = coefMatrix.columnCount();

		return new PiecewisePolynomialResultsWithSensitivity(_solver.getKnotsMat1D(xValues), coefMatrix, nCoefs, 1, coefSenseMatrix);
	  }
	}

}