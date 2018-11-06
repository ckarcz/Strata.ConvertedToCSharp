/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{
	/// <summary>
	/// The standard set of curve interpolators.
	/// <para>
	/// These are referenced from <seealso cref="CurveInterpolators"/> where their name is used to look up an
	/// instance of <seealso cref="CurveInterpolator"/>. This allows them to be referenced statically like a
	/// constant but also allows them to be redefined and new instances added.
	/// </para>
	/// </summary>
	internal sealed class StandardCurveInterpolators
	{

	  // Linear interpolator.
	  public static readonly CurveInterpolator LINEAR = LinearCurveInterpolator.INSTANCE;
	  // Log linear interpolator.
	  public static readonly CurveInterpolator LOG_LINEAR = LogLinearCurveInterpolator.INSTANCE;
	  // Square linear interpolator.
	  public static readonly CurveInterpolator SQUARE_LINEAR = SquareLinearCurveInterpolator.INSTANCE;
	  // Double quadratic interpolator.
	  public static readonly CurveInterpolator DOUBLE_QUADRATIC = DoubleQuadraticCurveInterpolator.INSTANCE;
	  // Time square interpolator.
	  public static readonly CurveInterpolator TIME_SQUARE = TimeSquareCurveInterpolator.INSTANCE;

	  //Log natural spline interpolation with monotonicity filter.
	  public static readonly CurveInterpolator LOG_NATURAL_SPLINE_MONOTONE_CUBIC = LogNaturalSplineMonotoneCubicInterpolator.INSTANCE;
	  // Log natural spline interpolation for discount factors
	  public static readonly CurveInterpolator LOG_NATURAL_SPLINE_DISCOUNT_FACTOR = LogNaturalSplineDiscountFactorCurveInterpolator.INSTANCE;
	  // Natural cubic spline interpolator.
	  public static readonly CurveInterpolator NATURAL_CUBIC_SPLINE = NaturalCubicSplineCurveInterpolator.INSTANCE;
	  // Natural spline interpolator.
	  public static readonly CurveInterpolator NATURAL_SPLINE = NaturalSplineCurveInterpolator.INSTANCE;
	  // Natural spline interpolator with non-negativity filter.
	  public static readonly CurveInterpolator NATURAL_SPLINE_NONNEGATIVITY_CUBIC = NaturalSplineNonnegativityCubicCurveInterpolator.INSTANCE;
	  // Product natural cubic spline interpolator.
	  public static readonly CurveInterpolator PRODUCT_NATURAL_SPLINE = ProductNaturalSplineCurveInterpolator.INSTANCE;
	  // Product natural cubic spline interpolator with monotonicity filter.
	  public static readonly CurveInterpolator PRODUCT_NATURAL_SPLINE_MONOTONE_CUBIC = ProductNaturalSplineMonotoneCubicInterpolator.INSTANCE;
	  // Product linear interpolator.
	  public static readonly CurveInterpolator PRODUCT_LINEAR = ProductLinearCurveInterpolator.INSTANCE;
	  // Step upper interpolator.
	  public static readonly CurveInterpolator STEP_UPPER = StepUpperCurveInterpolator.INSTANCE;
	  // Piecewise cubic Hermite interpolator with monotonicity.
	  public static readonly CurveInterpolator PCHIP = PiecewiseCubicHermiteMonotonicityCurveInterpolator.INSTANCE;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private StandardCurveInterpolators()
	  {
	  }

	}

}