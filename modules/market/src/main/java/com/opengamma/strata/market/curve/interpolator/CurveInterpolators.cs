/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// The standard set of curve interpolators.
	/// </summary>
	public sealed class CurveInterpolators
	{
	  // TODO: Check and add Javadoc for each constant

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<CurveInterpolator> ENUM_LOOKUP = ExtendedEnum.of(typeof(CurveInterpolator));

	  /// <summary>
	  /// Linear interpolator.
	  /// <para>
	  /// The interpolated value of the function <i>y</i> at <i>x</i> between two data points
	  /// <i>(x<sub>1</sub>, y<sub>1</sub>)</i> and <i>(x<sub>2</sub>, y<sub>2</sub>)</i> is given by:<br>
	  /// <i>y = y<sub>1</sub> + (x - x<sub>1</sub>) * (y<sub>2</sub> - y<sub>1</sub>)
	  /// / (x<sub>2</sub> - x<sub>1</sub>)</i>.
	  /// </para>
	  /// </summary>
	  public static readonly CurveInterpolator LINEAR = CurveInterpolator.of(StandardCurveInterpolators.LINEAR.Name);
	  /// <summary>
	  /// Log linear interpolator.
	  /// <para>
	  /// The interpolated value of the function <i>y</i> at <i>x</i> between two data points
	  /// <i>(x<sub>1</sub>, y<sub>1</sub>)</i> and <i>(x<sub>2</sub>, y<sub>2</sub>)</i> is given by:<br>
	  /// <i>y = y<sub>1</sub> (y<sub>2</sub> / y<sub>1</sub>) ^ ((x - x<sub>1</sub>) /
	  /// (x<sub>2</sub> - x<sub>1</sub>))</i><br>
	  /// It is the equivalent of performing a linear interpolation on a data set after
	  /// taking the logarithm of the y-values.
	  /// </para>
	  /// </summary>
	  public static readonly CurveInterpolator LOG_LINEAR = CurveInterpolator.of(StandardCurveInterpolators.LOG_LINEAR.Name);
	  /// <summary>
	  /// Square linear interpolator.
	  /// <para>
	  /// The interpolator is used for interpolation on variance for options.
	  /// Interpolation is linear on y^2. All values of y must be positive.
	  /// </para>
	  /// </summary>
	  public static readonly CurveInterpolator SQUARE_LINEAR = CurveInterpolator.of(StandardCurveInterpolators.SQUARE_LINEAR.Name);
	  /// <summary>
	  /// Double quadratic interpolator.
	  /// </summary>
	  public static readonly CurveInterpolator DOUBLE_QUADRATIC = CurveInterpolator.of(StandardCurveInterpolators.DOUBLE_QUADRATIC.Name);
	  /// <summary>
	  /// Time square interpolator.
	  /// <para>
	  /// The interpolation is linear on {@code x y^2}. The interpolator is used for interpolation on
	  /// integrated variance for options. All values of y must be positive.
	  /// </para>
	  /// </summary>
	  public static readonly CurveInterpolator TIME_SQUARE = CurveInterpolator.of(StandardCurveInterpolators.TIME_SQUARE.Name);

	  /// <summary>
	  /// Log natural spline interpolation with monotonicity filter.
	  /// <para>
	  /// Finds an interpolant {@code F(x) = exp( f(x) )} where {@code f(x)} is a Natural cubic
	  /// spline with Monotonicity cubic filter.
	  /// </para>
	  /// </summary>
	  public static readonly CurveInterpolator LOG_NATURAL_SPLINE_MONOTONE_CUBIC = CurveInterpolator.of(StandardCurveInterpolators.LOG_NATURAL_SPLINE_MONOTONE_CUBIC.Name);
	  /// <summary>
	  /// Log natural spline interpolator for discount factors.
	  /// <para>
	  /// Finds an interpolant {@code F(x) = exp( f(x) )} where {@code f(x)} is a natural cubic spline going through
	  /// the point (0,1).  
	  /// </para>
	  /// </summary>
	  public static readonly CurveInterpolator LOG_NATURAL_SPLINE_DISCOUNT_FACTOR = CurveInterpolator.of(StandardCurveInterpolators.LOG_NATURAL_SPLINE_DISCOUNT_FACTOR.Name);
	  /// <summary>
	  /// Natural cubic spline interpolator.
	  /// </summary>
	  public static readonly CurveInterpolator NATURAL_CUBIC_SPLINE = CurveInterpolator.of(StandardCurveInterpolators.NATURAL_CUBIC_SPLINE.Name);
	  /// <summary>
	  /// Natural spline interpolator.
	  /// </summary>
	  public static readonly CurveInterpolator NATURAL_SPLINE = CurveInterpolator.of(StandardCurveInterpolators.NATURAL_SPLINE.Name);
	  /// <summary>
	  /// Natural spline interpolator with non-negativity filter.
	  /// </summary>
	  public static readonly CurveInterpolator NATURAL_SPLINE_NONNEGATIVITY_CUBIC = CurveInterpolator.of(StandardCurveInterpolators.NATURAL_SPLINE_NONNEGATIVITY_CUBIC.Name);
	  /// <summary>
	  /// Product natural spline interpolator.
	  /// <para>
	  /// Given a data set {@code (x[i], y[i])}, interpolate {@code (x[i], x[i] * y[i])} by natural cubic spline.
	  /// </para>
	  /// <para>
	  /// As a curve for the product {@code x * y} is not well-defined at {@code x = 0}, we impose
	  /// the condition that all of the x data to be the same sign, such that the origin is not within data range.
	  /// The x key value must not be close to zero.
	  /// </para>
	  /// </summary>
	  public static readonly CurveInterpolator PRODUCT_NATURAL_SPLINE = CurveInterpolator.of(StandardCurveInterpolators.PRODUCT_NATURAL_SPLINE.Name);
	  /// <summary>
	  /// Product natural spline interpolator with monotonicity filter.
	  /// <para>
	  /// Given a data set {@code (x[i], y[i])}, interpolate {@code (x[i], x[i] * y[i])} by natural
	  /// cubic spline with monotonicity filter.
	  /// </para>
	  /// </summary>
	  public static readonly CurveInterpolator PRODUCT_NATURAL_SPLINE_MONOTONE_CUBIC = CurveInterpolator.of(StandardCurveInterpolators.PRODUCT_NATURAL_SPLINE_MONOTONE_CUBIC.Name);
	  /// <summary>
	  /// Product linear interpolator.
	  /// <para>
	  /// Given a data set {@code (x[i], y[i])}, interpolate {@code (x[i], x[i] * y[i])} by linear functions. 
	  /// </para>
	  /// <para>
	  /// As a curve for the product {@code x * y} is not well-defined at {@code x = 0}, we impose
	  /// the condition that all of the x data to be the same sign, such that the origin is not within data range.
	  /// The x key value must not be close to zero.
	  /// </para>
	  /// </summary>
	  public static readonly CurveInterpolator PRODUCT_LINEAR = CurveInterpolator.of(StandardCurveInterpolators.PRODUCT_LINEAR.Name);
	  /// <summary>
	  /// Step upper interpolator.
	  /// <para>
	  /// The interpolated value at <i>x</i> s.t. <i>x<sub>1</sub> < x =< x<sub>2</sub></i> is the value at <i>x<sub>2</sub></i>. 
	  /// </para>
	  /// </summary>
	  public static readonly CurveInterpolator STEP_UPPER = CurveInterpolator.of(StandardCurveInterpolators.STEP_UPPER.Name);
	  /// <summary>
	  /// Piecewise cubic Hermite interpolator with monotonicity.
	  /// </summary>
	  public static readonly CurveInterpolator PCHIP = CurveInterpolator.of(StandardCurveInterpolators.PCHIP.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private CurveInterpolators()
	  {
	  }

	}

}