/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// A curve interpolator that has been bound to a specific curve.
	/// <para>
	/// A bound interpolator is created from a <seealso cref="CurveInterpolator"/>.
	/// The bind process takes the definition of the interpolator and combines it with the x-y values.
	/// This allows implementations to optimize interpolation calculations.
	/// </para>
	/// <para>
	/// A bound interpolator is typically linked to two <seealso cref="BoundCurveExtrapolator extrapolators"/>.
	/// If an attempt is made to interpolate an x-value outside the range defined by
	/// the first and last nodes, the appropriate extrapolator will be used.
	/// </para>
	/// </summary>
	public interface BoundCurveInterpolator
	{

	  /// <summary>
	  /// Computes the y-value for the specified x-value by interpolation.
	  /// </summary>
	  /// <param name="x">  the x-value to find the y-value for </param>
	  /// <returns> the value at the x-value </returns>
	  /// <exception cref="RuntimeException"> if the y-value cannot be calculated </exception>
	  double interpolate(double x);

	  /// <summary>
	  /// Computes the first derivative of the y-value for the specified x-value.
	  /// <para>
	  /// The first derivative is {@code dy/dx}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="x">  the x-value at which the derivative is taken </param>
	  /// <returns> the first derivative </returns>
	  /// <exception cref="RuntimeException"> if the derivative cannot be calculated </exception>
	  double firstDerivative(double x);

	  /// <summary>
	  /// Computes the sensitivity of the y-value with respect to the curve parameters.
	  /// <para>
	  /// This returns an array with one element for each parameter of the curve.
	  /// The array contains the sensitivity of the y-value at the specified x-value to each parameter.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="x">  the x-value at which the parameter sensitivity is computed </param>
	  /// <returns> the sensitivity </returns>
	  /// <exception cref="RuntimeException"> if the sensitivity cannot be calculated </exception>
	  DoubleArray parameterSensitivity(double x);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Binds this interpolator to the specified extrapolators.
	  /// <para>
	  /// The bound interpolator provides methods to interpolate the y-value for a x-value.
	  /// If an attempt is made to interpolate an x-value outside the range defined by
	  /// the first and last nodes, the appropriate extrapolator will be used.
	  /// </para>
	  /// <para>
	  /// This method is intended to be called from within
	  /// <seealso cref="CurveInterpolator#bind(DoubleArray, DoubleArray, CurveExtrapolator, CurveExtrapolator)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="extrapolatorLeft">  the extrapolator for x-values on the left </param>
	  /// <param name="extrapolatorRight">  the extrapolator for x-values on the right </param>
	  /// <returns> the bound interpolator </returns>
	  BoundCurveInterpolator bind(BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight);

	}

}