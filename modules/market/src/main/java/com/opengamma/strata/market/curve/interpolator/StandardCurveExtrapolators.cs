/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{
	/// <summary>
	/// The standard set of curve extrapolators.
	/// <para>
	/// These are referenced from <seealso cref="CurveExtrapolators"/> where their name is used to look up an
	/// instance of <seealso cref="CurveExtrapolator"/>. This allows them to be referenced statically like a
	/// constant but also allows them to be redefined and new instances added.
	/// </para>
	/// </summary>
	internal sealed class StandardCurveExtrapolators
	{

	  // Flat extrapolator.
	  public static readonly CurveExtrapolator FLAT = FlatCurveExtrapolator.INSTANCE;
	  // Linear extrapolator.
	  public static readonly CurveExtrapolator LINEAR = LinearCurveExtrapolator.INSTANCE;
	  // Log linear extrapolator.
	  public static readonly CurveExtrapolator LOG_LINEAR = LogLinearCurveExtrapolator.INSTANCE;
	  // Quadratic left extrapolator.
	  public static readonly CurveExtrapolator QUADRATIC_LEFT = QuadraticLeftCurveExtrapolator.INSTANCE;
	  // Discount factor quadratic left zero rate extrapolator.
	  public static readonly CurveExtrapolator DISCOUNT_FACTOR_QUADRATIC_LEFT_ZERO_RATE = DiscountFactorQuadraticLeftZeroRateCurveExtrapolator.INSTANCE;
	  // Discount factor linear right zero rate extrapolator.
	  public static readonly CurveExtrapolator DISCOUNT_FACTOR_LINEAR_RIGHT_ZERO_RATE = DiscountFactorLinearRightZeroRateCurveExtrapolator.INSTANCE;
	  // Product linear extrapolator.
	  public static readonly CurveExtrapolator PRODUCT_LINEAR = ProductLinearCurveExtrapolator.INSTANCE;
	  // Exponential extrapolator.
	  public static readonly CurveExtrapolator EXPONENTIAL = ExponentialCurveExtrapolator.INSTANCE;
	  // Exception extrapolator.
	  public static readonly CurveExtrapolator EXCEPTION = ExceptionCurveExtrapolator.INSTANCE;
	  // Extrapolator that does no extrapolation and delegates to the interpolator.
	  public static readonly CurveExtrapolator INTERPOLATOR = InterpolatorCurveExtrapolator.INSTANCE;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private StandardCurveExtrapolators()
	  {
	  }

	}

}