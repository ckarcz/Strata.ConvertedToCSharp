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
	using PiecewiseCubicHermiteSplineInterpolatorWithSensitivity = com.opengamma.strata.math.impl.interpolation.PiecewiseCubicHermiteSplineInterpolatorWithSensitivity;
	using PiecewisePolynomialInterpolator = com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialInterpolator;
	using PiecewisePolynomialResult = com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResult;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using OGMatrixAlgebra = com.opengamma.strata.math.impl.matrix.OGMatrixAlgebra;

	/// <summary>
	/// Cubic Hermite interpolation preserving monotonicity.
	/// <para>
	/// The data points are interpolated by piecewise cubic Heremite polynomials. 
	/// The interpolation functions are monotonic in each interval.
	/// </para>
	/// </summary>
	[Serializable]
	internal sealed class PiecewiseCubicHermiteMonotonicityCurveInterpolator : CurveInterpolator
	{

	  /// <summary>
	  /// The interpolator name.
	  /// </summary>
	  public const string NAME = "PiecewiseCubicHermiteMonotonicity";
	  /// <summary>
	  /// The interpolator instance.
	  /// </summary>
	  public static readonly PiecewiseCubicHermiteMonotonicityCurveInterpolator INSTANCE = new PiecewiseCubicHermiteMonotonicityCurveInterpolator();

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
	  private PiecewiseCubicHermiteMonotonicityCurveInterpolator()
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
		internal readonly DoubleArray knots;
		internal readonly DoubleMatrix coefMatrix;
		internal readonly Supplier<DoubleMatrix[]> coefMatrixSensi;

		internal Bound(DoubleArray xValues, DoubleArray yValues) : base(xValues, yValues)
		{
		  this.xValues = xValues.toArrayUnsafe();
		  this.yValues = yValues.toArrayUnsafe();
		  PiecewisePolynomialInterpolator underlying = new PiecewiseCubicHermiteSplineInterpolatorWithSensitivity();
		  PiecewisePolynomialResult poly = underlying.interpolate(xValues.toArray(), yValues.toArray());
		  this.knots = poly.Knots;
		  this.coefMatrix = poly.CoefMatrix;
		  this.coefMatrixSensi = Suppliers.memoize(() => underlying.interpolateWithSensitivity(xValues.toArray(), yValues.toArray()).CoefficientSensitivityAll);
		}

		internal Bound(Bound @base, BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight) : base(@base, extrapolatorLeft, extrapolatorRight)
		{
		  this.xValues = @base.xValues;
		  this.yValues = @base.yValues;
		  this.knots = @base.knots;
		  this.coefMatrix = @base.coefMatrix;
		  this.coefMatrixSensi = @base.coefMatrixSensi;
		}

		//-------------------------------------------------------------------------
		internal static double evaluate(double xValue, DoubleArray knots, DoubleMatrix coefMatrix)
		{

		  int indicator = getIndicator(xValue, knots);
		  DoubleArray coefs = coefMatrix.row(indicator);
		  return getValue(coefs.toArrayUnsafe(), xValue, knots.get(indicator));
		}

		internal static double differentiate(double xValue, DoubleArray knots, DoubleMatrix coefMatrix)
		{

		  int indicator = getIndicator(xValue, knots);
		  DoubleArray coefs = coefMatrix.row(indicator);
		  return getDerivativeValue(coefs.toArrayUnsafe(), xValue, knots.get(indicator));
		}

		internal static int getIndicator(double xValue, DoubleArray knots)
		{
		  // check for 1 less interval than knots 
		  int lowerBound = FunctionUtils.getLowerBoundIndex(knots, xValue);
		  return lowerBound == knots.size() - 1 ? lowerBound - 1 : lowerBound;
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

		/// <param name="coefs">  {a_n,a_{n-1},...} of f(x) = a_n x^{n} + a_{n-1} x^{n-1} + .... </param>
		/// <param name="x">  the x </param>
		/// <param name="leftknot">  the knot specifying underlying interpolation function </param>
		/// <returns> the value of the underlying interpolation function at the value of x </returns>
		internal static double getDerivativeValue(double[] coefs, double x, double leftknot)
		{
		  int nCoefs = coefs.Length;
		  double s = x - leftknot;
		  double res = (nCoefs - 1) * coefs[0];
		  for (int i = 1; i < nCoefs - 1; i++)
		  { // nCoefs > 1
			res *= s;
			res += (nCoefs - i - 1d) * coefs[i];
		  }
		  return res;
		}

		//-------------------------------------------------------------------------
		protected internal override double doInterpolate(double xValue)
		{
		  return evaluate(xValue, knots, coefMatrix);
		}

		protected internal override double doFirstDerivative(double xValue)
		{
		  return differentiate(xValue, knots, coefMatrix);
		}

		protected internal override DoubleArray doParameterSensitivity(double xValue)
		{
		  int indicator = getIndicator(xValue, knots);
		  DoubleMatrix coefficientSensitivity = coefMatrixSensi.get()[indicator];
		  int nCoefs = coefficientSensitivity.rowCount();
		  double s = xValue - knots.get(indicator);
		  DoubleArray res = coefficientSensitivity.row(0);
		  for (int i = 1; i < nCoefs; i++)
		  {
			res = (DoubleArray) MA.scale(res, s);
			res = (DoubleArray) MA.add(res, coefficientSensitivity.row(i));
		  }
		  return res;
		}

		public override BoundCurveInterpolator bind(BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight)
		{

		  return new Bound(this, extrapolatorLeft, extrapolatorRight);
		}
	  }

	}

}