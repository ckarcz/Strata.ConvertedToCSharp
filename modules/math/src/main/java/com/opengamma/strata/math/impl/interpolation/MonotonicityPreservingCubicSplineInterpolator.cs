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
	using PiecewisePolynomialWithSensitivityFunction1D = com.opengamma.strata.math.impl.function.PiecewisePolynomialWithSensitivityFunction1D;

	/// <summary>
	/// Filter for local monotonicity of cubic spline interpolation based on 
	/// R. L. Dougherty, A. Edelman, and J. M. Hyman, "Nonnegativity-, Monotonicity-, or Convexity-Preserving Cubic and Quintic Hermite Interpolation" 
	/// Mathematics Of Computation, v. 52, n. 186, April 1989, pp. 471-494. 
	/// 
	/// First, interpolant is computed by another cubic interpolation method. Then the first derivatives are modified such that local monotonicity conditions are satisfied. 
	/// </summary>
	public class MonotonicityPreservingCubicSplineInterpolator : PiecewisePolynomialInterpolator
	{
	  private readonly HermiteCoefficientsProvider _solver = new HermiteCoefficientsProvider();
	  private readonly PiecewisePolynomialWithSensitivityFunction1D _function = new PiecewisePolynomialWithSensitivityFunction1D();
	  private PiecewisePolynomialInterpolator _method;

	  private const double EPS = 1.e-7;
	  private const double SMALL = 1.e-14;

	  /// <summary>
	  /// Primary interpolation method should be passed. </summary>
	  /// <param name="method"> PiecewisePolynomialInterpolator </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public MonotonicityPreservingCubicSplineInterpolator(final PiecewisePolynomialInterpolator method)
	  public MonotonicityPreservingCubicSplineInterpolator(PiecewisePolynomialInterpolator method)
	  {
		_method = method;
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public PiecewisePolynomialResult interpolate(final double[] xValues, final double[] yValues)
	  public override PiecewisePolynomialResult interpolate(double[] xValues, double[] yValues)
	  {
		ArgChecker.notNull(xValues, "xValues");
		ArgChecker.notNull(yValues, "yValues");

		ArgChecker.isTrue(xValues.Length == yValues.Length | xValues.Length + 2 == yValues.Length, "(xValues length = yValues length) or (xValues length + 2 = yValues length)");
		ArgChecker.isTrue(xValues.Length > 4, "Data points should be more than 4");

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nDataPts = xValues.length;
		int nDataPts = xValues.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int yValuesLen = yValues.length;
		int yValuesLen = yValues.Length;

		for (int i = 0; i < nDataPts; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(xValues[i]), "xValues containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(xValues[i]), "xValues containing Infinity");
		}
		for (int i = 0; i < yValuesLen; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(yValues[i]), "yValues containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(yValues[i]), "yValues containing Infinity");
		}

		double[] xValuesSrt = Arrays.copyOf(xValues, nDataPts);
		double[] yValuesSrt = new double[nDataPts];
		if (nDataPts == yValuesLen)
		{
		  yValuesSrt = Arrays.copyOf(yValues, nDataPts);
		}
		else
		{
		  yValuesSrt = Arrays.copyOfRange(yValues, 1, nDataPts + 1);
		}
		DoubleArrayMath.sortPairs(xValuesSrt, yValuesSrt);

		for (int i = 1; i < nDataPts; ++i)
		{
		  ArgChecker.isFalse(xValuesSrt[i - 1] == xValuesSrt[i], "xValues should be distinct");
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] intervals = _solver.intervalsCalculator(xValuesSrt);
		double[] intervals = _solver.intervalsCalculator(xValuesSrt);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] slopes = _solver.slopesCalculator(yValuesSrt, intervals);
		double[] slopes = _solver.slopesCalculator(yValuesSrt, intervals);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PiecewisePolynomialResult result = _method.interpolate(xValues, yValues);
		PiecewisePolynomialResult result = _method.interpolate(xValues, yValues);

		ArgChecker.isTrue(result.Order == 4, "Primary interpolant is not cubic");

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] initialFirst = _function.differentiate(result, xValuesSrt).rowArray(0);
		double[] initialFirst = _function.differentiate(result, xValuesSrt).rowArray(0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] first = firstDerivativeCalculator(intervals, slopes, initialFirst);
		double[] first = firstDerivativeCalculator(intervals, slopes, initialFirst);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefs = _solver.solve(yValuesSrt, intervals, slopes, first);
		double[][] coefs = _solver.solve(yValuesSrt, intervals, slopes, first);

		for (int i = 0; i < nDataPts - 1; ++i)
		{
		  for (int j = 0; j < 4; ++j)
		  {
			ArgChecker.isFalse(double.IsNaN(coefs[i][j]), "Too large input");
			ArgChecker.isFalse(double.IsInfinity(coefs[i][j]), "Too large input");
		  }
		}

		return new PiecewisePolynomialResult(DoubleArray.copyOf(xValuesSrt), DoubleMatrix.copyOf(coefs), 4, 1);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public PiecewisePolynomialResult interpolate(final double[] xValues, final double[][] yValuesMatrix)
	  public override PiecewisePolynomialResult interpolate(double[] xValues, double[][] yValuesMatrix)
	  {
		ArgChecker.notNull(xValues, "xValues");
		ArgChecker.notNull(yValuesMatrix, "yValuesMatrix");

		ArgChecker.isTrue(xValues.Length == yValuesMatrix[0].Length | xValues.Length + 2 == yValuesMatrix[0].Length, "(xValues length = yValuesMatrix's row vector length) or (xValues length + 2 = yValuesMatrix's row vector length)");
		ArgChecker.isTrue(xValues.Length > 4, "Data points should be more than 4");

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
		  double[] yValuesSrt = new double[nDataPts];
		  if (nDataPts == yValuesLen)
		  {
			yValuesSrt = Arrays.copyOf(yValuesMatrix[i], nDataPts);
		  }
		  else
		  {
			yValuesSrt = Arrays.copyOfRange(yValuesMatrix[i], 1, nDataPts + 1);
		  }
		  DoubleArrayMath.sortPairs(xValuesSrt, yValuesSrt);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] intervals = _solver.intervalsCalculator(xValuesSrt);
		  double[] intervals = _solver.intervalsCalculator(xValuesSrt);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] slopes = _solver.slopesCalculator(yValuesSrt, intervals);
		  double[] slopes = _solver.slopesCalculator(yValuesSrt, intervals);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PiecewisePolynomialResult result = _method.interpolate(xValues, yValuesMatrix[i]);
		  PiecewisePolynomialResult result = _method.interpolate(xValues, yValuesMatrix[i]);

		  ArgChecker.isTrue(result.Order == 4, "Primary interpolant is not cubic");

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] initialFirst = _function.differentiate(result, xValuesSrt).rowArray(0);
		  double[] initialFirst = _function.differentiate(result, xValuesSrt).rowArray(0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] first = firstDerivativeCalculator(intervals, slopes, initialFirst);
		  double[] first = firstDerivativeCalculator(intervals, slopes, initialFirst);

		  coefMatrix[i] = DoubleMatrix.copyOf(_solver.solve(yValuesSrt, intervals, slopes, first));
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
		ArgChecker.notNull(xValues, "xValues");
		ArgChecker.notNull(yValues, "yValues");

		ArgChecker.isTrue(xValues.Length == yValues.Length | xValues.Length + 2 == yValues.Length, "(xValues length = yValues length) or (xValues length + 2 = yValues length)");
		ArgChecker.isTrue(xValues.Length > 4, "Data points should be more than 4");

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nDataPts = xValues.length;
		int nDataPts = xValues.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int yValuesLen = yValues.length;
		int yValuesLen = yValues.Length;

		for (int i = 0; i < nDataPts; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(xValues[i]), "xValues containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(xValues[i]), "xValues containing Infinity");
		}
		for (int i = 0; i < yValuesLen; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(yValues[i]), "yValues containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(yValues[i]), "yValues containing Infinity");
		}

		double[] yValuesSrt = new double[nDataPts];
		if (nDataPts == yValuesLen)
		{
		  yValuesSrt = Arrays.copyOf(yValues, nDataPts);
		}
		else
		{
		  yValuesSrt = Arrays.copyOfRange(yValues, 1, nDataPts + 1);
		}

		for (int i = 1; i < nDataPts; ++i)
		{
		  ArgChecker.isFalse(xValues[i - 1] == xValues[i], "xValues should be distinct");
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] intervals = _solver.intervalsCalculator(xValues);
		double[] intervals = _solver.intervalsCalculator(xValues);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] slopes = _solver.slopesCalculator(yValuesSrt, intervals);
		double[] slopes = _solver.slopesCalculator(yValuesSrt, intervals);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix[] slopesSensitivityWithAbs = slopesSensitivityWithAbsCalculator(intervals, slopes);
		DoubleMatrix[] slopesSensitivityWithAbs = slopesSensitivityWithAbsCalculator(intervals, slopes);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] slopesSensitivity = slopesSensitivityWithAbs[0].toArray();
		double[][] slopesSensitivity = slopesSensitivityWithAbs[0].toArray();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] slopesAbsSensitivity = slopesSensitivityWithAbs[1].toArray();
		double[][] slopesAbsSensitivity = slopesSensitivityWithAbs[1].toArray();
		DoubleArray[] firstWithSensitivity = new DoubleArray[nDataPts + 1];

		/*
		 * Mode sensitivity is not computed analytically for |s_i| = |s_{i+1}| or s_{i-1}*h_{i} + s_{i}*h_{i-1} = 0. 
		 * Centered finite difference approximation is used in such cases
		 */
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final boolean sym = checkSymm(slopes);
		bool sym = checkSymm(slopes);
		if (sym == true)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PiecewisePolynomialResult result = _method.interpolate(xValues, yValues);
		  PiecewisePolynomialResult result = _method.interpolate(xValues, yValues);
		  ArgChecker.isTrue(result.Order == 4, "Primary interpolant is not cubic");
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] initialFirst = _function.differentiate(result, xValues).rowArray(0);
		  double[] initialFirst = _function.differentiate(result, xValues).rowArray(0);
		  firstWithSensitivity[0] = DoubleArray.copyOf(firstDerivativeCalculator(intervals, slopes, initialFirst));

		  int nExtra = nDataPts == yValuesLen ? 0 : 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValuesUp = java.util.Arrays.copyOf(yValues, nDataPts + 2 * nExtra);
		  double[] yValuesUp = Arrays.copyOf(yValues, nDataPts + 2 * nExtra);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValuesDw = java.util.Arrays.copyOf(yValues, nDataPts + 2 * nExtra);
		  double[] yValuesDw = Arrays.copyOf(yValues, nDataPts + 2 * nExtra);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] tmp = new double[nDataPts][nDataPts];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] tmp = new double[nDataPts][nDataPts];
		  double[][] tmp = RectangularArrays.ReturnRectangularDoubleArray(nDataPts, nDataPts);
		  for (int i = nExtra; i < nDataPts + nExtra; ++i)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double den = Math.abs(yValues[i]) < SMALL ? EPS : yValues[i] * EPS;
			double den = Math.Abs(yValues[i]) < SMALL ? EPS : yValues[i] * EPS;
			yValuesUp[i] = Math.Abs(yValues[i]) < SMALL ? EPS : yValues[i] * (1.0 + EPS);
			yValuesDw[i] = Math.Abs(yValues[i]) < SMALL ? -EPS : yValues[i] * (1.0 - EPS);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValuesSrtUp = java.util.Arrays.copyOfRange(yValuesUp, nExtra, nDataPts + nExtra);
			double[] yValuesSrtUp = Arrays.copyOfRange(yValuesUp, nExtra, nDataPts + nExtra);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValuesSrtDw = java.util.Arrays.copyOfRange(yValuesDw, nExtra, nDataPts + nExtra);
			double[] yValuesSrtDw = Arrays.copyOfRange(yValuesDw, nExtra, nDataPts + nExtra);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] slopesUp = _solver.slopesCalculator(yValuesSrtUp, intervals);
			double[] slopesUp = _solver.slopesCalculator(yValuesSrtUp, intervals);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] slopesDw = _solver.slopesCalculator(yValuesSrtDw, intervals);
			double[] slopesDw = _solver.slopesCalculator(yValuesSrtDw, intervals);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] initialFirstUp = _function.differentiate(_method.interpolate(xValues, yValuesUp), xValues).rowArray(0);
			double[] initialFirstUp = _function.differentiate(_method.interpolate(xValues, yValuesUp), xValues).rowArray(0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] initialFirstDw = _function.differentiate(_method.interpolate(xValues, yValuesDw), xValues).rowArray(0);
			double[] initialFirstDw = _function.differentiate(_method.interpolate(xValues, yValuesDw), xValues).rowArray(0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] firstUp = firstDerivativeCalculator(intervals, slopesUp, initialFirstUp);
			double[] firstUp = firstDerivativeCalculator(intervals, slopesUp, initialFirstUp);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] firstDw = firstDerivativeCalculator(intervals, slopesDw, initialFirstDw);
			double[] firstDw = firstDerivativeCalculator(intervals, slopesDw, initialFirstDw);
			for (int j = 0; j < nDataPts; ++j)
			{
			  tmp[j][i - nExtra] = 0.5 * (firstUp[j] - firstDw[j]) / den;
			}
			yValuesUp[i] = yValues[i];
			yValuesDw[i] = yValues[i];
		  }
		  for (int i = 0; i < nDataPts; ++i)
		  {
			firstWithSensitivity[i + 1] = DoubleArray.copyOf(tmp[i]);
		  }
		}
		else
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PiecewisePolynomialResultsWithSensitivity resultWithSensitivity = _method.interpolateWithSensitivity(xValues, yValues);
		  PiecewisePolynomialResultsWithSensitivity resultWithSensitivity = _method.interpolateWithSensitivity(xValues, yValues);
		  ArgChecker.isTrue(resultWithSensitivity.Order == 4, "Primary interpolant is not cubic");

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] initialFirst = _function.differentiate(resultWithSensitivity, xValues).rowArray(0);
		  double[] initialFirst = _function.differentiate(resultWithSensitivity, xValues).rowArray(0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray[] initialFirstSense = _function.differentiateNodeSensitivity(resultWithSensitivity, xValues);
		  DoubleArray[] initialFirstSense = _function.differentiateNodeSensitivity(resultWithSensitivity, xValues);
		  firstWithSensitivity = firstDerivativeWithSensitivityCalculator(intervals, slopes, slopesSensitivity, slopesAbsSensitivity, initialFirst, initialFirstSense);
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix[] resMatrix = _solver.solveWithSensitivity(yValuesSrt, intervals, slopes, slopesSensitivity, firstWithSensitivity);
		DoubleMatrix[] resMatrix = _solver.solveWithSensitivity(yValuesSrt, intervals, slopes, slopesSensitivity, firstWithSensitivity);

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
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix[] coefSenseMatrix = new com.opengamma.strata.collect.array.DoubleMatrix[nDataPts - 1];
		DoubleMatrix[] coefSenseMatrix = new DoubleMatrix[nDataPts - 1];
		Array.Copy(resMatrix, 1, coefSenseMatrix, 0, nDataPts - 1);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nCoefs = coefMatrix.columnCount();
		int nCoefs = coefMatrix.columnCount();

		return new PiecewisePolynomialResultsWithSensitivity(DoubleArray.copyOf(xValues), coefMatrix, nCoefs, 1, coefSenseMatrix);
	  }

	  public override PiecewisePolynomialInterpolator PrimaryMethod
	  {
		  get
		  {
			return _method;
		  }
	  }

	  /// <summary>
	  /// First derivatives are modified such that cubic interpolant has the same sign as linear interpolator </summary>
	  /// <param name="intervals">  the intervals </param>
	  /// <param name="slopes">  the slopes </param>
	  /// <param name="initialFirst">  the initial first </param>
	  /// <returns> first derivative  </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private double[] firstDerivativeCalculator(final double[] intervals, final double[] slopes, final double[] initialFirst)
	  private double[] firstDerivativeCalculator(double[] intervals, double[] slopes, double[] initialFirst)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nDataPts = intervals.length + 1;
		int nDataPts = intervals.Length + 1;
		double[] res = new double[nDataPts];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] pSlopes = parabolaSlopesCalculator(intervals, slopes);
		double[][] pSlopes = parabolaSlopesCalculator(intervals, slopes);

		for (int i = 1; i < nDataPts - 1; ++i)
		{
		  double refValue = 3.0 * Math.Min(Math.Abs(slopes[i - 1]), Math.Min(Math.Abs(slopes[i]), Math.Abs(pSlopes[i - 1][1])));
		  if (i > 1)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sig1 = Math.signum(pSlopes[i - 1][1]);
			double sig1 = Math.Sign(pSlopes[i - 1][1]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sig2 = Math.signum(pSlopes[i - 1][0]);
			double sig2 = Math.Sign(pSlopes[i - 1][0]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sig3 = Math.signum(slopes[i - 1] - slopes[i - 2]);
			double sig3 = Math.Sign(slopes[i - 1] - slopes[i - 2]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sig4 = Math.signum(slopes[i] - slopes[i - 1]);
			double sig4 = Math.Sign(slopes[i] - slopes[i - 1]);
			if (Math.Abs(sig1 - sig2) <= 0.0 && Math.Abs(sig2 - sig3) <= 0.0 && Math.Abs(sig3 - sig4) <= 0.0)
			{
			  refValue = Math.Max(refValue, 1.5 * Math.Min(Math.Abs(pSlopes[i - 1][0]), Math.Abs(pSlopes[i - 1][1])));
			}
		  }
		  if (i < nDataPts - 2)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sig1 = Math.signum(-pSlopes[i - 1][1]);
			double sig1 = Math.Sign(-pSlopes[i - 1][1]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sig2 = Math.signum(-pSlopes[i - 1][2]);
			double sig2 = Math.Sign(-pSlopes[i - 1][2]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sig3 = Math.signum(slopes[i + 1] - slopes[i]);
			double sig3 = Math.Sign(slopes[i + 1] - slopes[i]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sig4 = Math.signum(slopes[i] - slopes[i - 1]);
			double sig4 = Math.Sign(slopes[i] - slopes[i - 1]);
			if (Math.Abs(sig1 - sig2) <= 0.0 && Math.Abs(sig2 - sig3) <= 0.0 && Math.Abs(sig3 - sig4) <= 0.0)
			{
			  refValue = Math.Max(refValue, 1.5 * Math.Min(Math.Abs(pSlopes[i - 1][2]), Math.Abs(pSlopes[i - 1][1])));
			}
		  }
		  res[i] = Math.Sign(initialFirst[i]) != Math.Sign(pSlopes[i - 1][1]) ? 0.0 : Math.Sign(initialFirst[i]) * Math.Min(Math.Abs(initialFirst[i]), refValue);
		}
		res[0] = Math.Sign(initialFirst[0]) != Math.Sign(slopes[0]) ? 0.0 : Math.Sign(initialFirst[0]) * Math.Min(Math.Abs(initialFirst[0]), 3.0 * Math.Abs(slopes[0]));
		res[nDataPts - 1] = Math.Sign(initialFirst[nDataPts - 1]) != Math.Sign(slopes[nDataPts - 2]) ? 0.0 : Math.Sign(initialFirst[nDataPts - 1]) * Math.Min(Math.Abs(initialFirst[nDataPts - 1]), 3.0 * Math.Abs(slopes[nDataPts - 2]));
		return res;
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.array.DoubleArray[] firstDerivativeWithSensitivityCalculator(final double[] intervals, final double[] slopes, final double[][] slopesSensitivity, final double[][] slopesAbsSensitivity, final double[] initialFirst, final com.opengamma.strata.collect.array.DoubleArray[] initialFirstSense)
	  private DoubleArray[] firstDerivativeWithSensitivityCalculator(double[] intervals, double[] slopes, double[][] slopesSensitivity, double[][] slopesAbsSensitivity, double[] initialFirst, DoubleArray[] initialFirstSense)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nDataPts = intervals.length + 1;
		int nDataPts = intervals.Length + 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray[] res = new com.opengamma.strata.collect.array.DoubleArray[nDataPts + 1];
		DoubleArray[] res = new DoubleArray[nDataPts + 1];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] first = new double[nDataPts];
		double[] first = new double[nDataPts];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] pSlopes = parabolaSlopesCalculator(intervals, slopes);
		double[][] pSlopes = parabolaSlopesCalculator(intervals, slopes);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix[] pSlopesAbsSensitivity = parabolaSlopesAbstSensitivityCalculator(intervals, slopesSensitivity, pSlopes);
		DoubleMatrix[] pSlopesAbsSensitivity = parabolaSlopesAbstSensitivityCalculator(intervals, slopesSensitivity, pSlopes);

		for (int i = 1; i < nDataPts - 1; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] tmpSense = new double[nDataPts];
		  double[] tmpSense = new double[nDataPts];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sigInitialFirst = Math.signum(initialFirst[i]);
		  double sigInitialFirst = Math.Sign(initialFirst[i]);
		  if (sigInitialFirst * Math.Sign(pSlopes[i - 1][1]) < 0.0)
		  {
			first[i] = 0.0;
			Arrays.fill(tmpSense, 0.0);
		  }
		  else
		  {
			double[] refValueWithSense = factoredMinWithSensitivityFinder(Math.Abs(slopes[i - 1]), slopesAbsSensitivity[i - 1], Math.Abs(slopes[i]), slopesAbsSensitivity[i], Math.Abs(pSlopes[i - 1][1]), pSlopesAbsSensitivity[1].rowArray(i - 1));
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] refSense = new double[nDataPts];
			double[] refSense = new double[nDataPts];
			Array.Copy(refValueWithSense, 1, refSense, 0, nDataPts);
			if (i > 1)
			{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sig1 = Math.signum(pSlopes[i - 1][1]);
			  double sig1 = Math.Sign(pSlopes[i - 1][1]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sig2 = Math.signum(pSlopes[i - 1][0]);
			  double sig2 = Math.Sign(pSlopes[i - 1][0]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sig3 = Math.signum(slopes[i - 1] - slopes[i - 2]);
			  double sig3 = Math.Sign(slopes[i - 1] - slopes[i - 2]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sig4 = Math.signum(slopes[i] - slopes[i - 1]);
			  double sig4 = Math.Sign(slopes[i] - slopes[i - 1]);
			  if (Math.Abs(sig1 - sig2) <= 0.0 && Math.Abs(sig2 - sig3) <= 0.0 && Math.Abs(sig3 - sig4) <= 0.0)
			  {
				refValueWithSense = modifyRefValueWithSensitivity(refValueWithSense[0], refSense, Math.Abs(pSlopes[i - 1][0]), pSlopesAbsSensitivity[0].rowArray(i - 2), Math.Abs(pSlopes[i - 1][1]), pSlopesAbsSensitivity[1].rowArray(i - 1));
			  }
			}
			if (i < nDataPts - 2)
			{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sig1 = Math.signum(-pSlopes[i - 1][1]);
			  double sig1 = Math.Sign(-pSlopes[i - 1][1]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sig2 = Math.signum(-pSlopes[i - 1][2]);
			  double sig2 = Math.Sign(-pSlopes[i - 1][2]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sig3 = Math.signum(slopes[i + 1] - slopes[i]);
			  double sig3 = Math.Sign(slopes[i + 1] - slopes[i]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sig4 = Math.signum(slopes[i] - slopes[i - 1]);
			  double sig4 = Math.Sign(slopes[i] - slopes[i - 1]);
			  if (Math.Abs(sig1 - sig2) <= 0.0 && Math.Abs(sig2 - sig3) <= 0.0 && Math.Abs(sig3 - sig4) <= 0.0)
			  {
				refValueWithSense = modifyRefValueWithSensitivity(refValueWithSense[0], refSense, Math.Abs(pSlopes[i - 1][2]), pSlopesAbsSensitivity[2].rowArray(i - 1), Math.Abs(pSlopes[i - 1][1]), pSlopesAbsSensitivity[1].rowArray(i - 1));
			  }
			}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double absFirst = Math.abs(initialFirst[i]);
			double absFirst = Math.Abs(initialFirst[i]);
			if (Math.Abs(absFirst - refValueWithSense[0]) < SMALL)
			{
			  first[i] = absFirst <= refValueWithSense[0] ? initialFirst[i] : sigInitialFirst * refValueWithSense[0];
			  for (int k = 0; k < nDataPts; ++k)
			  {
				tmpSense[k] = 0.5 * (initialFirstSense[i].get(k) + sigInitialFirst * refValueWithSense[k + 1]);
			  }
			}
			else
			{
			  if (absFirst < refValueWithSense[0])
			  {
				first[i] = initialFirst[i];
				Array.Copy(initialFirstSense[i].toArray(), 0, tmpSense, 0, nDataPts);
			  }
			  else
			  {
				first[i] = sigInitialFirst * refValueWithSense[0];
				for (int k = 0; k < nDataPts; ++k)
				{
				  tmpSense[k] = sigInitialFirst * refValueWithSense[k + 1];
				}
			  }
			}
		  }
		  res[i + 1] = DoubleArray.copyOf(tmpSense);
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] tmpSenseIni = new double[nDataPts];
		double[] tmpSenseIni = new double[nDataPts];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sigFirstIni = Math.signum(initialFirst[0]);
		double sigFirstIni = Math.Sign(initialFirst[0]);
		if (sigFirstIni * Math.Sign(slopes[0]) < 0.0)
		{
		  first[0] = 0.0;
		  Arrays.fill(tmpSenseIni, 0.0);
		}
		else
		{
		  if (Math.Abs(initialFirst[0]) > SMALL && Math.Abs(slopes[0]) < SMALL)
		  {
			first[0] = 0.0;
			Arrays.fill(tmpSenseIni, 0.0);
			tmpSenseIni[0] = -1.5 / intervals[0];
			tmpSenseIni[1] = 1.5 / intervals[0];
		  }
		  else
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double absFirst = Math.abs(initialFirst[0]);
			double absFirst = Math.Abs(initialFirst[0]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double modSlope = 3.0 * Math.abs(slopes[0]);
			double modSlope = 3.0 * Math.Abs(slopes[0]);
			if (Math.Abs(absFirst - modSlope) < SMALL)
			{
			  first[0] = absFirst <= modSlope ? initialFirst[0] : sigFirstIni * modSlope;
			  for (int k = 0; k < nDataPts; ++k)
			  {
				tmpSenseIni[k] = 0.5 * (initialFirstSense[0].get(k) + 3.0 * sigFirstIni * slopesAbsSensitivity[0][k]);
			  }
			}
			else
			{
			  if (absFirst < modSlope)
			  {
				first[0] = initialFirst[0];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double factor = Math.abs(initialFirst[0]) < SMALL ? 0.5 : 1.0;
				double factor = Math.Abs(initialFirst[0]) < SMALL ? 0.5 : 1.0;
				for (int k = 0; k < nDataPts; ++k)
				{
				  tmpSenseIni[k] = factor * initialFirstSense[0].get(k);
				}
			  }
			  else
			  {
				first[0] = sigFirstIni * modSlope;
				for (int k = 0; k < nDataPts; ++k)
				{
				  tmpSenseIni[k] = 3.0 * sigFirstIni * slopesAbsSensitivity[0][k];
				}
			  }
			}
		  }
		}
		res[1] = DoubleArray.copyOf(tmpSenseIni);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] tmpSenseFin = new double[nDataPts];
		double[] tmpSenseFin = new double[nDataPts];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sigFirstFin = Math.signum(initialFirst[nDataPts - 1]);
		double sigFirstFin = Math.Sign(initialFirst[nDataPts - 1]);
		if (sigFirstFin * Math.Sign(slopes[nDataPts - 2]) < 0.0)
		{
		  first[nDataPts - 1] = 0.0;
		  Arrays.fill(tmpSenseFin, 0.0);
		}
		else
		{
		  if (Math.Abs(initialFirst[nDataPts - 1]) > SMALL && Math.Abs(slopes[nDataPts - 2]) < SMALL)
		  {
			first[nDataPts - 1] = 0.0;
			Arrays.fill(tmpSenseFin, 0.0);
			tmpSenseFin[nDataPts - 2] = -1.5 / intervals[nDataPts - 2];
			tmpSenseFin[nDataPts - 1] = 1.5 / intervals[nDataPts - 2];
		  }
		  else
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double absFirst = Math.abs(initialFirst[nDataPts - 1]);
			double absFirst = Math.Abs(initialFirst[nDataPts - 1]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double modSlope = 3.0 * Math.abs(slopes[nDataPts - 2]);
			double modSlope = 3.0 * Math.Abs(slopes[nDataPts - 2]);
			if (Math.Abs(absFirst - modSlope) < SMALL)
			{
			  first[nDataPts - 1] = absFirst <= modSlope ? initialFirst[nDataPts - 1] : sigFirstFin * modSlope;
			  for (int k = 0; k < nDataPts; ++k)
			  {
				tmpSenseFin[k] = 0.5 * (initialFirstSense[nDataPts - 1].get(k) + 3.0 * sigFirstFin * slopesAbsSensitivity[nDataPts - 2][k]);
			  }
			}
			else
			{
			  if (absFirst < modSlope)
			  {
				first[nDataPts - 1] = initialFirst[nDataPts - 1];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double factor = Math.abs(initialFirst[nDataPts - 1]) < SMALL ? 0.5 : 1.0;
				double factor = Math.Abs(initialFirst[nDataPts - 1]) < SMALL ? 0.5 : 1.0;
				for (int k = 0; k < nDataPts; ++k)
				{
				  tmpSenseFin[k] = factor * initialFirstSense[nDataPts - 1].get(k);
				}
			  }
			  else
			  {
				first[nDataPts - 1] = sigFirstFin * modSlope;
				for (int k = 0; k < nDataPts; ++k)
				{
				  tmpSenseFin[k] = 3.0 * sigFirstFin * slopesAbsSensitivity[nDataPts - 2][k];
				}
			  }
			}
		  }
		}
		res[nDataPts] = DoubleArray.copyOf(tmpSenseFin);

		res[0] = DoubleArray.copyOf(first);
		return res;
	  }

	  /// <param name="intervals">  the intervals </param>
	  /// <param name="slopes">  the slopes </param>
	  /// <returns> Parabola slopes, each row vactor is (p^{-1}, p^{0}, p^{1}) for xValues_1,...,xValues_{nDataPts-2} </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private double[][] parabolaSlopesCalculator(final double[] intervals, final double[] slopes)
	  private double[][] parabolaSlopesCalculator(double[] intervals, double[] slopes)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData = intervals.length + 1;
		int nData = intervals.Length + 1;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[nData - 2][3];
		double[][] res = RectangularArrays.ReturnRectangularDoubleArray(nData - 2, 3);

		res[0][0] = double.PositiveInfinity;
		res[0][1] = (slopes[0] * intervals[1] + slopes[1] * intervals[0]) / (intervals[0] + intervals[1]);
		res[0][2] = (slopes[1] * (2.0 * intervals[1] + intervals[2]) - slopes[2] * intervals[1]) / (intervals[1] + intervals[2]);
		for (int i = 1; i < nData - 3; ++i)
		{
		  res[i][0] = (slopes[i] * (2.0 * intervals[i] + intervals[i - 1]) - slopes[i - 1] * intervals[i]) / (intervals[i - 1] + intervals[i]);
		  res[i][1] = (slopes[i] * intervals[i + 1] + slopes[i + 1] * intervals[i]) / (intervals[i] + intervals[i + 1]);
		  res[i][2] = (slopes[i + 1] * (2.0 * intervals[i + 1] + intervals[i + 2]) - slopes[i + 2] * intervals[i + 1]) / (intervals[i + 1] + intervals[i + 2]);
		}
		res[nData - 3][0] = (slopes[nData - 3] * (2.0 * intervals[nData - 3] + intervals[nData - 4]) - slopes[nData - 4] * intervals[nData - 3]) / (intervals[nData - 4] + intervals[nData - 3]);
		res[nData - 3][1] = (slopes[nData - 3] * intervals[nData - 2] + slopes[nData - 2] * intervals[nData - 3]) / (intervals[nData - 3] + intervals[nData - 2]);
		res[nData - 3][2] = double.PositiveInfinity;
		return res;
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.array.DoubleMatrix[] parabolaSlopesAbstSensitivityCalculator(final double[] intervals, final double[][] slopeSensitivity, final double[][] parabolaSlopes)
	  private DoubleMatrix[] parabolaSlopesAbstSensitivityCalculator(double[] intervals, double[][] slopeSensitivity, double[][] parabolaSlopes)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix[] res = new com.opengamma.strata.collect.array.DoubleMatrix[3];
		DoubleMatrix[] res = new DoubleMatrix[3];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData = intervals.length + 1;
		int nData = intervals.Length + 1;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] left = new double[nData - 3][nData];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] left = new double[nData - 3][nData];
		double[][] left = RectangularArrays.ReturnRectangularDoubleArray(nData - 3, nData);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] center = new double[nData - 2][nData];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] center = new double[nData - 2][nData];
		double[][] center = RectangularArrays.ReturnRectangularDoubleArray(nData - 2, nData);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] right = new double[nData - 3][nData];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] right = new double[nData - 3][nData];
		double[][] right = RectangularArrays.ReturnRectangularDoubleArray(nData - 3, nData);
		for (int i = 0; i < nData - 3; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sigLeft = Math.signum(parabolaSlopes[i + 1][0]);
		  double sigLeft = Math.Sign(parabolaSlopes[i + 1][0]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sigCenter = Math.signum(parabolaSlopes[i][1]);
		  double sigCenter = Math.Sign(parabolaSlopes[i][1]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sigRight = Math.signum(parabolaSlopes[i][2]);
		  double sigRight = Math.Sign(parabolaSlopes[i][2]);
		  if (sigLeft == 0.0)
		  {
			Arrays.fill(left[i], 0.0);
		  }
		  if (sigCenter == 0.0)
		  {
			Arrays.fill(center[i], 0.0);
		  }
		  if (sigRight == 0.0)
		  {
			Arrays.fill(right[i], 0.0);
		  }
		  for (int k = 0; k < nData; ++k)
		  {
			left[i][k] = sigLeft * (slopeSensitivity[i + 1][k] * (2.0 * intervals[i + 1] + intervals[i]) - slopeSensitivity[i][k] * intervals[i + 1]) / (intervals[i] + intervals[i + 1]);
			center[i][k] = sigCenter * (slopeSensitivity[i][k] * intervals[i + 1] + slopeSensitivity[i + 1][k] * intervals[i]) / (intervals[i] + intervals[i + 1]);
			right[i][k] = sigRight * (slopeSensitivity[i + 1][k] * (2.0 * intervals[i + 1] + intervals[i + 2]) - slopeSensitivity[i + 2][k] * intervals[i + 1]) / (intervals[i + 1] + intervals[i + 2]);
		  }
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sigCenterFin = Math.signum(parabolaSlopes[nData - 3][1]);
		double sigCenterFin = Math.Sign(parabolaSlopes[nData - 3][1]);
		if (sigCenterFin == 0.0)
		{
		  Arrays.fill(center[nData - 3], 0.0);
		}
		for (int k = 0; k < nData; ++k)
		{
		  center[nData - 3][k] = sigCenterFin * (slopeSensitivity[nData - 3][k] * intervals[nData - 2] + slopeSensitivity[nData - 2][k] * intervals[nData - 3]) / (intervals[nData - 3] + intervals[nData - 2]);
		}
		res[0] = DoubleMatrix.copyOf(left);
		res[1] = DoubleMatrix.copyOf(center);
		res[2] = DoubleMatrix.copyOf(right);

		return res;
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.array.DoubleMatrix[] slopesSensitivityWithAbsCalculator(final double[] intervals, final double[] slopes)
	  private DoubleMatrix[] slopesSensitivityWithAbsCalculator(double[] intervals, double[] slopes)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nDataPts = intervals.length + 1;
		int nDataPts = intervals.Length + 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix[] res = new com.opengamma.strata.collect.array.DoubleMatrix[2];
		DoubleMatrix[] res = new DoubleMatrix[2];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] slopesSensitivity = new double[nDataPts - 1][nDataPts];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] slopesSensitivity = new double[nDataPts - 1][nDataPts];
		double[][] slopesSensitivity = RectangularArrays.ReturnRectangularDoubleArray(nDataPts - 1, nDataPts);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] absSlopesSensitivity = new double[nDataPts - 1][nDataPts];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] absSlopesSensitivity = new double[nDataPts - 1][nDataPts];
		double[][] absSlopesSensitivity = RectangularArrays.ReturnRectangularDoubleArray(nDataPts - 1, nDataPts);

		for (int i = 0; i < nDataPts - 1; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sign = Math.signum(slopes[i]);
		  double sign = Math.Sign(slopes[i]);
		  Arrays.fill(slopesSensitivity[i], 0.0);
		  Arrays.fill(absSlopesSensitivity[i], 0.0);
		  slopesSensitivity[i][i] = -1.0 / intervals[i];
		  slopesSensitivity[i][i + 1] = 1.0 / intervals[i];
		  if (sign > 0.0)
		  {
			absSlopesSensitivity[i][i] = slopesSensitivity[i][i];
			absSlopesSensitivity[i][i + 1] = slopesSensitivity[i][i + 1];
		  }
		  if (sign < 0.0)
		  {
			absSlopesSensitivity[i][i] = -slopesSensitivity[i][i];
			absSlopesSensitivity[i][i + 1] = -slopesSensitivity[i][i + 1];
		  }
		}
		res[0] = DoubleMatrix.copyOf(slopesSensitivity);
		res[1] = DoubleMatrix.copyOf(absSlopesSensitivity);
		return res;
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private double[] factoredMinWithSensitivityFinder(final double val1, final double[] val1Sensitivity, final double val2, final double[] val2Sensitivity, final double val3, final double[] val3Sensitivity)
	  private double[] factoredMinWithSensitivityFinder(double val1, double[] val1Sensitivity, double val2, double[] val2Sensitivity, double val3, double[] val3Sensitivity)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData = val1Sensitivity.length;
		int nData = val1Sensitivity.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] res = new double[nData + 1];
		double[] res = new double[nData + 1];
		double tmpRef = 0.0;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] tmpSensitivity = new double[nData];
		double[] tmpSensitivity = new double[nData];

		if (val1 < val2)
		{
		  tmpRef = val1;
		  for (int i = 0; i < nData; ++i)
		  {
			tmpSensitivity[i] = val1Sensitivity[i];
		  }
		}
		else
		{
		  tmpRef = val2;
		  for (int i = 0; i < nData; ++i)
		  {
			tmpSensitivity[i] = val2Sensitivity[i];
		  }
		}

		if (val3 == tmpRef)
		{
		  res[0] = 3.0 * val3;
		  for (int i = 0; i < nData; ++i)
		  {
			res[i + 1] = 1.5 * (val3Sensitivity[i] + tmpSensitivity[i]);
		  }
		}
		else
		{
		  if (val3 < tmpRef)
		  {
			res[0] = 3.0 * val3;
			for (int i = 0; i < nData; ++i)
			{
			  res[i + 1] = 3.0 * val3Sensitivity[i];
			}
		  }
		  else
		  {
			res[0] = 3.0 * tmpRef;
			for (int i = 0; i < nData; ++i)
			{
			  res[i + 1] = 3.0 * tmpSensitivity[i];
			}
		  }
		}

		return res;
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private double[] modifyRefValueWithSensitivity(final double refVal, final double[] refValSensitivity, final double val1, final double[] val1Sensitivity, final double val2, final double[] val2Sensitivity)
	  private double[] modifyRefValueWithSensitivity(double refVal, double[] refValSensitivity, double val1, double[] val1Sensitivity, double val2, double[] val2Sensitivity)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData = refValSensitivity.length;
		int nData = refValSensitivity.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double absVal1 = Math.abs(val1);
		double absVal1 = Math.Abs(val1);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double absVal2 = Math.abs(val2);
		double absVal2 = Math.Abs(val2);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] res = new double[nData + 1];
		double[] res = new double[nData + 1];
		double tmpRef = 0.0;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] tmpSensitivity = new double[nData];
		double[] tmpSensitivity = new double[nData];

		if (absVal1 == absVal2)
		{
		  tmpRef = 1.5 * absVal1;
		  for (int i = 0; i < nData; ++i)
		  {
			tmpSensitivity[i] = 0.75 * (val1Sensitivity[i] + val2Sensitivity[i]);
		  }
		}
		else
		{
		  if (absVal1 < absVal2)
		  {
			tmpRef = 1.5 * absVal1;
			for (int i = 0; i < nData; ++i)
			{
			  tmpSensitivity[i] = 1.5 * (val1Sensitivity[i]);
			}
		  }
		  else
		  {
			tmpRef = 1.5 * absVal2;
			for (int i = 0; i < nData; ++i)
			{
			  tmpSensitivity[i] = 1.5 * (val2Sensitivity[i]);
			}
		  }
		}

		if (refVal == tmpRef)
		{
		  res[0] = refVal;
		  for (int i = 0; i < nData; ++i)
		  {
			res[i + 1] = 0.5 * (refValSensitivity[i] + tmpSensitivity[i]);
		  }
		}
		else
		{
		  if (refVal > tmpRef)
		  {
			res[0] = refVal;
			for (int i = 0; i < nData; ++i)
			{
			  res[i + 1] = refValSensitivity[i];
			}
		  }
		  else
		  {
			res[0] = tmpRef;
			for (int i = 0; i < nData; ++i)
			{
			  res[i + 1] = tmpSensitivity[i];
			}
		  }
		}

		return res;
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private boolean checkSymm(final double[] slopes)
	  private bool checkSymm(double[] slopes)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nDataM2 = slopes.length - 1;
		int nDataM2 = slopes.Length - 1;
		for (int i = 0; i < nDataM2; ++i)
		{
		  if (Math.Abs(Math.Abs(slopes[i]) - Math.Abs(slopes[i + 1])) < SMALL)
		  {
			return true;
		  }
		}
		return false;
	  }
	}

}