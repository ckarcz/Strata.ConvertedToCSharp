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
	/// Filter for nonnegativity of cubic spline interpolation based on 
	/// R. L. Dougherty, A. Edelman, and J. M. Hyman, "Nonnegativity-, Monotonicity-, or Convexity-Preserving Cubic and Quintic Hermite Interpolation" 
	/// Mathematics Of Computation, v. 52, n. 186, April 1989, pp. 471-494. 
	/// 
	/// First, interpolant is computed by another cubic interpolation method. Then the first derivatives are modified such that non-negativity conditions are satisfied. 
	/// Note that shape-preserving three-point formula is used at endpoints in order to ensure positivity of an interpolant in the first interval and the last interval 
	/// </summary>
	public class NonnegativityPreservingCubicSplineInterpolator : PiecewisePolynomialInterpolator
	{

	  private const double SMALL = 1.e-14;

	  private readonly HermiteCoefficientsProvider _solver = new HermiteCoefficientsProvider();
	  private readonly PiecewisePolynomialWithSensitivityFunction1D _function = new PiecewisePolynomialWithSensitivityFunction1D();
	  private PiecewisePolynomialInterpolator _method;

	  /// <summary>
	  /// Primary interpolation method should be passed. </summary>
	  /// <param name="method"> PiecewisePolynomialInterpolator </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public NonnegativityPreservingCubicSplineInterpolator(final PiecewisePolynomialInterpolator method)
	  public NonnegativityPreservingCubicSplineInterpolator(PiecewisePolynomialInterpolator method)
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
		ArgChecker.isTrue(xValues.Length > 2, "Data points should be more than 2");

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

		for (int i = 0; i < nDataPts - 1; ++i)
		{
		  for (int j = i + 1; j < nDataPts; ++j)
		  {
			ArgChecker.isFalse(xValues[i] == xValues[j], "xValues should be distinct");
		  }
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
//ORIGINAL LINE: final double[] first = firstDerivativeCalculator(yValuesSrt, intervals, slopes, initialFirst);
		double[] first = firstDerivativeCalculator(yValuesSrt, intervals, slopes, initialFirst);
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
		ArgChecker.isTrue(xValues.Length > 2, "Data points should be more than 2");

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
//ORIGINAL LINE: final double[] first = firstDerivativeCalculator(yValuesSrt, intervals, slopes, initialFirst);
		  double[] first = firstDerivativeCalculator(yValuesSrt, intervals, slopes, initialFirst);

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
		ArgChecker.isTrue(xValues.Length > 2, "Data points should be more than 2");

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

		for (int i = 0; i < nDataPts - 1; ++i)
		{
		  for (int j = i + 1; j < nDataPts; ++j)
		  {
			ArgChecker.isFalse(xValues[i] == xValues[j], "xValues should be distinct");
		  }
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

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] intervals = _solver.intervalsCalculator(xValues);
		double[] intervals = _solver.intervalsCalculator(xValues);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] slopes = _solver.slopesCalculator(yValuesSrt, intervals);
		double[] slopes = _solver.slopesCalculator(yValuesSrt, intervals);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PiecewisePolynomialResultsWithSensitivity resultWithSensitivity = _method.interpolateWithSensitivity(xValues, yValues);
		PiecewisePolynomialResultsWithSensitivity resultWithSensitivity = _method.interpolateWithSensitivity(xValues, yValues);

		ArgChecker.isTrue(resultWithSensitivity.Order == 4, "Primary interpolant is not cubic");

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] initialFirst = _function.differentiate(resultWithSensitivity, xValues).rowArray(0);
		double[] initialFirst = _function.differentiate(resultWithSensitivity, xValues).rowArray(0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] slopeSensitivity = _solver.slopeSensitivityCalculator(intervals);
		double[][] slopeSensitivity = _solver.slopeSensitivityCalculator(intervals);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray[] initialFirstSense = _function.differentiateNodeSensitivity(resultWithSensitivity, xValues);
		DoubleArray[] initialFirstSense = _function.differentiateNodeSensitivity(resultWithSensitivity, xValues);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray[] firstWithSensitivity = firstDerivativeWithSensitivityCalculator(yValuesSrt, intervals, initialFirst, initialFirstSense);
		DoubleArray[] firstWithSensitivity = firstDerivativeWithSensitivityCalculator(yValuesSrt, intervals, initialFirst, initialFirstSense);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix[] resMatrix = _solver.solveWithSensitivity(yValuesSrt, intervals, slopes, slopeSensitivity, firstWithSensitivity);
		DoubleMatrix[] resMatrix = _solver.solveWithSensitivity(yValuesSrt, intervals, slopes, slopeSensitivity, firstWithSensitivity);

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

	  // First derivatives are modified such that cubic interpolant has the same sign as linear interpolator 
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private double[] firstDerivativeCalculator(final double[] yValues, final double[] intervals, final double[] slopes, final double[] initialFirst)
	  private double[] firstDerivativeCalculator(double[] yValues, double[] intervals, double[] slopes, double[] initialFirst)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nDataPts = yValues.length;
		int nDataPts = yValues.Length;
		double[] res = new double[nDataPts];

		for (int i = 1; i < nDataPts - 1; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double tau = Math.signum(yValues[i]);
		  double tau = Math.Sign(yValues[i]);
		  res[i] = tau == 0.0 ? initialFirst[i] : Math.Min(3.0 * tau * yValues[i] / intervals[i - 1], Math.Max(-3.0 * tau * yValues[i] / intervals[i], tau * initialFirst[i])) / tau;
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double tauIni = Math.signum(yValues[0]);
		double tauIni = Math.Sign(yValues[0]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double tauFin = Math.signum(yValues[nDataPts - 1]);
		double tauFin = Math.Sign(yValues[nDataPts - 1]);
		res[0] = tauIni == 0.0 ? initialFirst[0] : Math.Min(3.0 * tauIni * yValues[0] / intervals[0], Math.Max(-3.0 * tauIni * yValues[0] / intervals[0], tauIni * initialFirst[0])) / tauIni;
		res[nDataPts - 1] = tauFin == 0.0 ? initialFirst[nDataPts - 1] : Math.Min(3.0 * tauFin * yValues[nDataPts - 1] / intervals[nDataPts - 2], Math.Max(-3.0 * tauFin * yValues[nDataPts - 1] / intervals[nDataPts - 2], tauFin * initialFirst[nDataPts - 1])) / tauFin;

		return res;
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.array.DoubleArray[] firstDerivativeWithSensitivityCalculator(final double[] yValues, final double[] intervals, final double[] initialFirst, final com.opengamma.strata.collect.array.DoubleArray[] initialFirstSense)
	  private DoubleArray[] firstDerivativeWithSensitivityCalculator(double[] yValues, double[] intervals, double[] initialFirst, DoubleArray[] initialFirstSense)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nDataPts = yValues.length;
		int nDataPts = yValues.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray[] res = new com.opengamma.strata.collect.array.DoubleArray[nDataPts + 1];
		DoubleArray[] res = new DoubleArray[nDataPts + 1];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] newFirst = new double[nDataPts];
		double[] newFirst = new double[nDataPts];

		for (int i = 1; i < nDataPts - 1; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double tau = Math.signum(yValues[i]);
		  double tau = Math.Sign(yValues[i]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double lower = -3.0 * tau * yValues[i] / intervals[i];
		  double lower = -3.0 * tau * yValues[i] / intervals[i];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double upper = 3.0 * tau * yValues[i] / intervals[i - 1];
		  double upper = 3.0 * tau * yValues[i] / intervals[i - 1];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = tau * initialFirst[i];
		  double @ref = tau * initialFirst[i];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] tmp = new double[nDataPts];
		  double[] tmp = new double[nDataPts];
		  Arrays.fill(tmp, 0.0);
		  if (Math.Abs(@ref - lower) < SMALL && tau != 0.0)
		  {
			newFirst[i] = @ref >= lower ? initialFirst[i] : lower / tau;
			for (int k = 0; k < nDataPts; ++k)
			{
			  tmp[k] = 0.5 * initialFirstSense[i].get(k);
			}
			tmp[i] -= 1.5 / intervals[i];
		  }
		  else
		  {
			if (@ref < lower)
			{
			  newFirst[i] = lower / tau;
			  tmp[i] = -3.0 / intervals[i];
			}
			else
			{
			  if (Math.Abs(@ref - upper) < SMALL && tau != 0.0)
			  {
				newFirst[i] = @ref <= upper ? initialFirst[i] : upper / tau;
				for (int k = 0; k < nDataPts; ++k)
				{
				  tmp[k] = 0.5 * initialFirstSense[i].get(k);
				}
				tmp[i] += 1.5 / intervals[i - 1];
			  }
			  else
			  {
				if (@ref > upper)
				{
				  newFirst[i] = upper / tau;
				  tmp[i] = 3.0 / intervals[i - 1];
				}
				else
				{
				  newFirst[i] = initialFirst[i];
				  Array.Copy(initialFirstSense[i].toArray(), 0, tmp, 0, nDataPts);
				}
			  }
			}
		  }
		  res[i + 1] = DoubleArray.copyOf(tmp);
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double tauIni = Math.signum(yValues[0]);
		double tauIni = Math.Sign(yValues[0]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double lowerIni = -3.0 * tauIni * yValues[0] / intervals[0];
		double lowerIni = -3.0 * tauIni * yValues[0] / intervals[0];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double upperIni = 3.0 * tauIni * yValues[0] / intervals[0];
		double upperIni = 3.0 * tauIni * yValues[0] / intervals[0];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double refIni = tauIni * initialFirst[0];
		double refIni = tauIni * initialFirst[0];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] tmpIni = new double[nDataPts];
		double[] tmpIni = new double[nDataPts];
		Arrays.fill(tmpIni, 0.0);
		if (Math.Abs(refIni - lowerIni) < SMALL && tauIni != 0.0)
		{
		  newFirst[0] = refIni >= lowerIni ? initialFirst[0] : lowerIni / tauIni;
		  for (int k = 0; k < nDataPts; ++k)
		  {
			tmpIni[k] = 0.5 * initialFirstSense[0].get(k);
		  }
		  tmpIni[0] -= 1.5 / intervals[0];
		}
		else
		{
		  if (refIni < lowerIni)
		  {
			newFirst[0] = lowerIni / tauIni;
			tmpIni[0] = -3.0 / intervals[0];
		  }
		  else
		  {
			if (Math.Abs(refIni - upperIni) < SMALL && tauIni != 0.0)
			{
			  newFirst[0] = refIni <= upperIni ? initialFirst[0] : upperIni / tauIni;
			  for (int k = 0; k < nDataPts; ++k)
			  {
				tmpIni[k] = 0.5 * initialFirstSense[0].get(k);
			  }
			  tmpIni[0] += 1.5 / intervals[0];
			}
			else
			{
			  if (refIni > upperIni)
			  {
				newFirst[0] = upperIni / tauIni;
				tmpIni[0] = 3.0 / intervals[0];
			  }
			  else
			  {
				newFirst[0] = initialFirst[0];
				Array.Copy(initialFirstSense[0].toArray(), 0, tmpIni, 0, nDataPts);
			  }
			}
		  }
		}
		res[1] = DoubleArray.copyOf(tmpIni);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double tauFin = Math.signum(yValues[nDataPts - 1]);
		double tauFin = Math.Sign(yValues[nDataPts - 1]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double lowerFin = -3.0 * tauFin * yValues[nDataPts - 1] / intervals[nDataPts - 2];
		double lowerFin = -3.0 * tauFin * yValues[nDataPts - 1] / intervals[nDataPts - 2];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double upperFin = 3.0 * tauFin * yValues[nDataPts - 1] / intervals[nDataPts - 2];
		double upperFin = 3.0 * tauFin * yValues[nDataPts - 1] / intervals[nDataPts - 2];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double refFin = tauFin * initialFirst[nDataPts - 1];
		double refFin = tauFin * initialFirst[nDataPts - 1];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] tmpFin = new double[nDataPts];
		double[] tmpFin = new double[nDataPts];
		Arrays.fill(tmpFin, 0.0);
		if (Math.Abs(refFin - lowerFin) < SMALL && tauFin != 0.0)
		{
		  newFirst[nDataPts - 1] = refFin >= lowerFin ? initialFirst[nDataPts - 1] : lowerFin / tauFin;
		  for (int k = 0; k < nDataPts; ++k)
		  {
			tmpFin[k] = 0.5 * initialFirstSense[nDataPts - 1].get(k);
		  }
		  tmpFin[nDataPts - 1] -= 1.5 / intervals[nDataPts - 2];
		}
		else
		{
		  if (refFin < lowerFin)
		  {
			newFirst[nDataPts - 1] = lowerFin / tauFin;
			tmpFin[nDataPts - 1] = -3.0 / intervals[nDataPts - 2];
		  }
		  else
		  {
			if (Math.Abs(refFin - upperFin) < SMALL && tauFin != 0.0)
			{
			  newFirst[nDataPts - 1] = refFin <= upperFin ? initialFirst[nDataPts - 1] : upperFin / tauFin;
			  for (int k = 0; k < nDataPts; ++k)
			  {
				tmpFin[k] = 0.5 * initialFirstSense[nDataPts - 1].get(k);
			  }
			  tmpFin[nDataPts - 1] += 1.5 / intervals[nDataPts - 2];
			}
			else
			{
			  if (refFin > upperFin)
			  {
				newFirst[nDataPts - 1] = upperFin / tauFin;
				tmpFin[nDataPts - 1] = 3.0 / intervals[nDataPts - 2];
			  }
			  else
			  {
				newFirst[nDataPts - 1] = initialFirst[nDataPts - 1];
				Array.Copy(initialFirstSense[nDataPts - 1].toArray(), 0, tmpFin, 0, nDataPts);
			  }
			}
		  }
		}
		res[nDataPts] = DoubleArray.copyOf(tmpFin);
		res[0] = DoubleArray.copyOf(newFirst);
		return res;
	  }

	}

}