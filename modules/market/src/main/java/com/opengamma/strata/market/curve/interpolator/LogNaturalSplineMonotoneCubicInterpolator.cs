using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{

	using Supplier = com.google.common.@base.Supplier;
	using Suppliers = com.google.common.@base.Suppliers;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using FunctionUtils = com.opengamma.strata.math.impl.FunctionUtils;
	using LogNaturalSplineHelper = com.opengamma.strata.math.impl.interpolation.LogNaturalSplineHelper;
	using MonotonicityPreservingCubicSplineInterpolator = com.opengamma.strata.math.impl.interpolation.MonotonicityPreservingCubicSplineInterpolator;
	using PiecewisePolynomialInterpolator = com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialInterpolator;
	using PiecewisePolynomialResult = com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResult;
	using PiecewisePolynomialResultsWithSensitivity = com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResultsWithSensitivity;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using OGMatrixAlgebra = com.opengamma.strata.math.impl.matrix.OGMatrixAlgebra;

	/// <summary>
	/// Log natural cubic interpolation with monotonicity filter.
	/// <para>
	/// Finds an interpolant {@code F(x) = exp( f(x) )} where {@code f(x)} is a Natural cubic
	/// spline with Monotonicity cubic filter.
	/// </para>
	/// </summary>
	[Serializable]
	internal sealed class LogNaturalSplineMonotoneCubicInterpolator : CurveInterpolator
	{

	  /// <summary>
	  /// The interpolator name.
	  /// </summary>
	  public const string NAME = "LogNaturalSplineMonotoneCubic";
	  /// <summary>
	  /// The interpolator instance.
	  /// </summary>
	  public static readonly CurveInterpolator INSTANCE = new LogNaturalSplineMonotoneCubicInterpolator();

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;
	  /// <summary>
	  /// Underlying matrix algebra.
	  /// </summary>
	  private static readonly MatrixAlgebra MA = new OGMatrixAlgebra();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private LogNaturalSplineMonotoneCubicInterpolator()
	  {
	  }

	  // resolve instance
	  private object readResolve()
	  {
		return INSTANCE;
	  }

	  //-------------------------------------------------------------------------
	  public string Name
	  {
		  get
		  {
			return NAME;
		  }
	  }

	  public BoundCurveInterpolator bind(DoubleArray xValues, DoubleArray yValues)
	  {
		return new Bound(xValues, yValues);
	  }

	  //-----------------------------------------------------------------------
	  public override string ToString()
	  {
		return NAME;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Bound interpolator.
	  /// </summary>
	  internal class Bound : AbstractBoundCurveInterpolator
	  {
		internal readonly double[] xValues;
		internal readonly double[] yValues;
		internal readonly PiecewisePolynomialResult poly;
		internal readonly Supplier<PiecewisePolynomialResultsWithSensitivity> polySens;
		internal double[] logYValues;

		internal Bound(DoubleArray xValues, DoubleArray yValues) : base(xValues, yValues)
		{
		  this.xValues = xValues.toArrayUnsafe();
		  this.yValues = yValues.toArrayUnsafe();
		  this.logYValues = getYLogValues(this.yValues);
		  PiecewisePolynomialInterpolator underlying = new MonotonicityPreservingCubicSplineInterpolator(new LogNaturalSplineHelper());
		  this.poly = underlying.interpolate(xValues.toArray(), logYValues);
		  this.polySens = Suppliers.memoize(() => underlying.interpolateWithSensitivity(xValues.toArray(), logYValues));
		}

		internal Bound(Bound @base, BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight) : base(@base, extrapolatorLeft, extrapolatorRight)
		{
		  this.xValues = @base.xValues;
		  this.yValues = @base.yValues;
		  this.logYValues = @base.logYValues;
		  this.poly = @base.poly;
		  this.polySens = @base.polySens;
		}

		//-------------------------------------------------------------------------
		internal static double evaluate(double xValue, DoubleArray knots, DoubleMatrix coefMatrix, int dimensions)
		{

		  // check for 1 less interval than knots 
		  int lowerBound = FunctionUtils.getLowerBoundIndex(knots, xValue);
		  int indicator = lowerBound == knots.size() - 1 ? lowerBound - 1 : lowerBound;
		  DoubleArray coefs = coefMatrix.row(dimensions * indicator);
		  return getValue(coefs.toArrayUnsafe(), xValue, knots.get(indicator));
		}

		internal static double differentiate(double xValue, DoubleArray knots, DoubleMatrix coefMatrix, int dimensions, int nCoefs, int numberOfIntervals)
		{

		  int rowCount = dimensions * numberOfIntervals;
		  int colCount = nCoefs - 1;
		  DoubleMatrix coef = DoubleMatrix.of(rowCount, colCount, (i, j) => coefMatrix.get(i, j) * (nCoefs - j - 1));
		  return evaluate(xValue, knots, coef, dimensions);
		}

		internal static DoubleArray nodeSensitivity(double xValue, DoubleArray knots, DoubleMatrix coefMatrix, int dimensions, int interval, DoubleMatrix coefficientSensitivity)
		{

		  double s = xValue - knots.get(interval);
		  int nCoefs = coefficientSensitivity.rowCount();

		  DoubleArray res = coefficientSensitivity.row(0);
		  for (int i = 1; i < nCoefs; i++)
		  {
			res = (DoubleArray) MA.scale(res, s);
			res = (DoubleArray) MA.add(res, coefficientSensitivity.row(i));
		  }
		  return res;
		}

		internal static double[] getValues(double[] bareValues)
		{
		  int nValues = bareValues.Length;
		  double[] res = new double[nValues];

		  for (int i = 0; i < nValues; ++i)
		  {
			res[i] = Math.Exp(bareValues[i]);
		  }
		  return res;
		}

		/// <param name="coefs">  {a_n,a_{n-1},...} of f(x) = a_n x^{n} + a_{n-1} x^{n-1} + .... </param>
		/// <param name="x">  the x </param>
		/// <param name="leftknot">  the knot specifying underlying interpolation function </param>
		/// <returns> the value of the underlying interpolation function at the value of x </returns>
		internal static double getValue(double[] coefs, double x, double leftknot)
		{
		  int nCoefs = coefs.Length;
		  double s = x - leftknot;
		  double res = coefs[0];
		  for (int i = 1; i < nCoefs; i++)
		  {
			res *= s;
			res += coefs[i];
		  }
		  return res;
		}

		internal static double[] getYLogValues(double[] yValues)
		{
		  int nData = yValues.Length;
		  double[] logYValues = new double[nData];
		  for (int i = 0; i < nData; ++i)
		  {
			logYValues[i] = Math.Log(yValues[i]);
		  }
		  return logYValues;
		}

		//-------------------------------------------------------------------------
		protected internal override double doInterpolate(double xValue)
		{
		  double resValue = evaluate(xValue, poly.Knots, poly.CoefMatrix, poly.Dimensions);
		  return Math.Exp(resValue);
		}

		protected internal override double doFirstDerivative(double xValue)
		{
		  double resValue = evaluate(xValue, poly.Knots, poly.CoefMatrix, poly.Dimensions);
		  int nCoefs = poly.Order;
		  int numberOfIntervals = poly.NumberOfIntervals;
		  double resDerivative = differentiate(xValue, poly.Knots, poly.CoefMatrix, poly.Dimensions, nCoefs, numberOfIntervals);

		  return Math.Exp(resValue) * resDerivative;
		}

		protected internal override DoubleArray doParameterSensitivity(double xValue)
		{
		  int interval = FunctionUtils.getLowerBoundIndex(poly.Knots, xValue);
		  if (interval == poly.Knots.size() - 1)
		  {
			interval--; // there is 1 less interval that knots
		  }

		  DoubleMatrix coefficientSensitivity = polySens.get().getCoefficientSensitivity(interval);
		  double[] resSense = nodeSensitivity(xValue, poly.Knots, poly.CoefMatrix, poly.Dimensions, interval, coefficientSensitivity).toArray();
		  double resValue = Math.Exp(evaluate(xValue, poly.Knots, poly.CoefMatrix, poly.Dimensions));
		  double[] knotValues = getValues(logYValues);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int knotValuesLength = knotValues.length;
		  int knotValuesLength = knotValues.Length;
		  double[] res = new double[knotValuesLength];
		  for (int i = 0; i < knotValuesLength; ++i)
		  {
			res[i] = resSense[i] * resValue / knotValues[i];
		  }
		  return DoubleArray.ofUnsafe(res);
		}

		public override BoundCurveInterpolator bind(BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight)
		{

		  return new Bound(this, extrapolatorLeft, extrapolatorRight);
		}
	  }

	}

}