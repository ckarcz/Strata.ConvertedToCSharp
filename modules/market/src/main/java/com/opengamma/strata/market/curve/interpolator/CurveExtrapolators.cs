/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// The standard set of curve extrapolators.
	/// </summary>
	public sealed class CurveExtrapolators
	{
	  // TODO: Check and add Javadoc for each constant

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<CurveExtrapolator> ENUM_LOOKUP = ExtendedEnum.of(typeof(CurveExtrapolator));

	  /// <summary>
	  /// Flat extrapolator.
	  /// <para>
	  /// The leftmost (rightmost) point of the data set is used for all extrapolated values.
	  /// </para>
	  /// </summary>
	  public static readonly CurveExtrapolator FLAT = CurveExtrapolator.of(StandardCurveExtrapolators.FLAT.Name);
	  /// <summary>
	  /// Linear extrapolator.
	  /// <para>
	  /// The extrapolation continues linearly from the leftmost (rightmost) point of the data set.
	  /// </para>
	  /// </summary>
	  public static readonly CurveExtrapolator LINEAR = CurveExtrapolator.of(StandardCurveExtrapolators.LINEAR.Name);
	  /// <summary>
	  /// Log linear extrapolator.
	  /// <para>
	  /// The extrapolant is {@code exp(f(x))} where {@code f(x)} is a linear function
	  /// which is smoothly connected with a log-interpolator {@code exp(F(x))}.
	  /// </para>
	  /// </summary>
	  public static readonly CurveExtrapolator LOG_LINEAR = CurveExtrapolator.of(StandardCurveExtrapolators.LOG_LINEAR.Name);
	  /// <summary>
	  /// Quadratic left extrapolator.
	  /// <para>
	  /// This left extrapolator is designed for extrapolating a discount factor where the
	  /// trivial point (0d,1d) is NOT involved in the data.
	  /// The extrapolation is completed by applying a quadratic extrapolant on the discount
	  /// factor (not log of the discount factor), where the point (0d,1d) is inserted and
	  /// the first derivative value is assumed to be continuous at the first key.
	  /// </para>
	  /// </summary>
	  public static readonly CurveExtrapolator QUADRATIC_LEFT = CurveExtrapolator.of(StandardCurveExtrapolators.QUADRATIC_LEFT.Name);
	  /// <summary>
	  /// Discount factor quadratic left extrapolator for zero rates.
	  /// <para>
	  /// This left extrapolator is designed for extrapolating a discount factor for zero rate inputs 
	  /// where the trivial point (0d,1d) is NOT involved in the data.
	  /// Use {@code QUADRATIC_LEFT} if the input data is discount factor values.
	  /// </para>
	  /// <para>
	  /// The extrapolation is completed by applying a quadratic extrapolant on the discount
	  /// factor (not log of the discount factor), where the point (0d,1d) is inserted and
	  /// the first derivative value is assumed to be continuous at the first key.
	  /// </para>
	  /// </summary>
	  public static readonly CurveExtrapolator DISCOUNT_FACTOR_QUADRATIC_LEFT_ZERO_RATE = CurveExtrapolator.of(StandardCurveExtrapolators.DISCOUNT_FACTOR_QUADRATIC_LEFT_ZERO_RATE.Name);
	  /// <summary>
	  /// Discount factor linear right extrapolator for zeor rates.
	  /// <para>
	  /// This right extrapolator is designed for extrapolating a discount factor for zero rate inputs. 
	  /// Use {@code LINEAR} if the input data is discount factor values.
	  /// </para>
	  /// <para>
	  /// The gradient of the extrapolation is determined so that the first derivative value of 
	  /// the discount factor is continuous at the last key.
	  /// </para>
	  /// </summary>
	  public static readonly CurveExtrapolator DISCOUNT_FACTOR_LINEAR_RIGHT_ZERO_RATE = CurveExtrapolator.of(StandardCurveExtrapolators.DISCOUNT_FACTOR_LINEAR_RIGHT_ZERO_RATE.Name);
	  /// <summary>
	  /// Product linear extrapolator.
	  /// <para>
	  /// Given a data set {@code (xValues[i], yValues[i])}, extrapolate {@code (x[i], x[i] * y[i])}
	  /// by a linear function.
	  /// </para>
	  /// <para>
	  /// The gradient of the extrapolation is obtained from the gradient of the interpolated
	  /// curve on {@code (x[i], x[i] * y[i])} at the first/last node.
	  /// </para>
	  /// <para>
	  /// The extrapolation is ambiguous at x=0. Thus the following rule applies: 
	  /// The x value of the first node must be strictly negative for the left extrapolation, whereas the x value of 
	  /// the last node must be strictly positive for the right extrapolation.
	  /// </para>
	  /// </summary>
	  public static readonly CurveExtrapolator PRODUCT_LINEAR = CurveExtrapolator.of(StandardCurveExtrapolators.PRODUCT_LINEAR.Name);
	  /// <summary>
	  /// Exponential extrapolator.
	  /// <para>
	  /// Outside the data range the function is an exponential exp(m*x) where m is such that
	  /// on the left {@code exp(m * firstXValue) = firstYValue} and on the right
	  /// {@code exp(m * lastXValue) = lastYValue}.
	  /// </para>
	  /// </summary>
	  public static readonly CurveExtrapolator EXPONENTIAL = CurveExtrapolator.of(StandardCurveExtrapolators.EXPONENTIAL.Name);
	  /// <summary>
	  /// Extrapolator that throws an exception if extrapolation is attempted.
	  /// </summary>
	  public static readonly CurveExtrapolator EXCEPTION = CurveExtrapolator.of(StandardCurveExtrapolators.EXCEPTION.Name);
	  /// <summary>
	  /// Interpolator extrapolator.
	  /// <para>
	  /// The extrapolator does no extrapolation itself and delegates to the interpolator for all operations.
	  /// </para>
	  /// </summary>
	  public static readonly CurveExtrapolator INTERPOLATOR = CurveExtrapolator.of(StandardCurveExtrapolators.INTERPOLATOR.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private CurveExtrapolators()
	  {
	  }

	}

}