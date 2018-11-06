using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{
	/// <summary>
	/// Product linear interpolation. 
	/// <para>
	/// Given a data set {@code (x[i], y[i])}, interpolate {@code (x[i], x[i] * y[i])} by linear functions. 
	/// </para>
	/// <para>
	/// As a curve for the product {@code x * y} is not well-defined at {@code x = 0}, we impose
	/// the condition that all of the x data to be the same sign, such that the origin is not within data range.
	/// The x key value must not be close to zero.
	/// </para>
	/// <para>
	/// See <seealso cref="LinearInterpolator"/> for the detail on the underlying interpolator. 
	/// </para>
	/// </summary>

	using Supplier = com.google.common.@base.Supplier;
	using Suppliers = com.google.common.@base.Suppliers;
	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using PiecewisePolynomialWithSensitivityFunction1D = com.opengamma.strata.math.impl.function.PiecewisePolynomialWithSensitivityFunction1D;
	using LinearInterpolator = com.opengamma.strata.math.impl.interpolation.LinearInterpolator;
	using PiecewisePolynomialResult = com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResult;
	using PiecewisePolynomialResultsWithSensitivity = com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResultsWithSensitivity;

	[Serializable]
	internal sealed class ProductLinearCurveInterpolator : CurveInterpolator
	{

	  /// <summary>
	  /// The interpolator name.
	  /// </summary>
	  public const string NAME = "ProductLinear";
	  /// <summary>
	  /// The interpolator instance.
	  /// </summary>
	  public static readonly CurveInterpolator INSTANCE = new ProductLinearCurveInterpolator();

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;
	  /// <summary>
	  /// The small parameter. 
	  /// </summary>
	  private const double SMALL = 1e-10;
	  /// <summary>
	  /// The polynomial function.
	  /// </summary>
	  private static readonly PiecewisePolynomialWithSensitivityFunction1D FUNCTION = new PiecewisePolynomialWithSensitivityFunction1D();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private ProductLinearCurveInterpolator()
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

	  //-------------------------------------------------------------------------
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

		internal Bound(DoubleArray xValues, DoubleArray yValues) : base(xValues, yValues)
		{
		  ArgChecker.isTrue(xValues.get(0) > 0d || xValues.get(xValues.size() - 1) < 0d, "xValues must have the same sign");
		  this.xValues = xValues.toArrayUnsafe();
		  this.yValues = yValues.toArrayUnsafe();
		  LinearInterpolator underlying = new LinearInterpolator();
		  this.poly = underlying.interpolate(xValues.toArray(), getProduct(this.xValues, this.yValues));
		  this.polySens = Suppliers.memoize(() => underlying.interpolateWithSensitivity(xValues.toArray(), getProduct(this.xValues, this.yValues)));
		}

		internal Bound(Bound @base, BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight) : base(@base, extrapolatorLeft, extrapolatorRight)
		{
		  this.xValues = @base.xValues;
		  this.yValues = @base.yValues;
		  this.poly = @base.poly;
		  this.polySens = @base.polySens;
		}

		//-------------------------------------------------------------------------
		internal static double[] getProduct(double[] xValues, double[] yValues)
		{
		  int nData = yValues.Length;
		  double[] xyValues = new double[nData];
		  for (int i = 0; i < nData; ++i)
		  {
			xyValues[i] = xValues[i] * yValues[i];
		  }
		  return xyValues;
		}

		//-------------------------------------------------------------------------
		protected internal override double doInterpolate(double xValue)
		{
		  ArgChecker.isTrue(Math.Abs(xValue) > SMALL, "magnitude of xValue must not be small");
		  double resValue = FUNCTION.evaluate(poly, xValue).get(0);
		  return resValue / xValue;
		}

		protected internal override double doFirstDerivative(double xValue)
		{
		  ArgChecker.isTrue(Math.Abs(xValue) > SMALL, "magnitude of xValue must not be small");
		  ValueDerivatives resValue = FUNCTION.evaluateAndDifferentiate(poly, xValue);
		  return -resValue.Value / (xValue * xValue) + resValue.getDerivative(0) / xValue;
		}

		protected internal override DoubleArray doParameterSensitivity(double xValue)
		{
		  ArgChecker.isTrue(Math.Abs(xValue) > SMALL, "magnitude of xValue must not be small");
		  DoubleArray resSense = FUNCTION.nodeSensitivity(polySens.get(), xValue);
		  return resSense.multipliedBy(DoubleArray.of(resSense.size(), i => xValues[i] / xValue));
		}

		public override BoundCurveInterpolator bind(BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight)
		{

		  return new Bound(this, extrapolatorLeft, extrapolatorRight);
		}
	  }

	}

}