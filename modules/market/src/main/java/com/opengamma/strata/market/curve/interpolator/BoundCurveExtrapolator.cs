/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// A curve extrapolator that has been bound to a specific curve.
	/// <para>
	/// A bound extrapolator is created from a <seealso cref="CurveExtrapolator"/>.
	/// The bind process takes the definition of the extrapolator and combines it with the x-y values.
	/// This allows implementations to optimize extrapolation calculations.
	/// </para>
	/// <para>
	/// This interface is primarily used internally. Applications typically do not invoke these methods.
	/// </para>
	/// </summary>
	public interface BoundCurveExtrapolator
	{

	  /// <summary>
	  /// Left extrapolates the y-value from the specified x-value.
	  /// <para>
	  /// This method is only intended to be invoked when the x-value is less than the x-value of the first node.
	  /// The behavior is undefined if called with any other x-value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="xValue">  the x-value to find the y-value for </param>
	  /// <returns> the extrapolated y-value for the specified x-value </returns>
	  /// <exception cref="RuntimeException"> if the y-value cannot be calculated </exception>
	  double leftExtrapolate(double xValue);

	  /// <summary>
	  /// Calculates the first derivative of the left extrapolated y-value at the specified x-value.
	  /// <para>
	  /// This method is only intended to be invoked when the x-value is less than the x-value of the first node.
	  /// The behavior is undefined if called with any other x-value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="xValue">  the x-value to find the y-value for </param>
	  /// <returns> the first derivative of the extrapolated y-value for the specified x-value </returns>
	  /// <exception cref="RuntimeException"> if the derivative cannot be calculated </exception>
	  double leftExtrapolateFirstDerivative(double xValue);

	  /// <summary>
	  /// Calculates the parameter sensitivities of the left extrapolated y-value at the specified x-value.
	  /// <para>
	  /// This method is only intended to be invoked when the x-value is less than the x-value of the first node.
	  /// The behavior is undefined if called with any other x-value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="xValue">  the x-value to find the y-value for </param>
	  /// <returns> the parameter sensitivities of the extrapolated y-value for the specified x-value </returns>
	  /// <exception cref="RuntimeException"> if the sensitivity cannot be calculated </exception>
	  DoubleArray leftExtrapolateParameterSensitivity(double xValue);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Right extrapolates the y-value from the specified x-value.
	  /// <para>
	  /// This method is only intended to be invoked when the x-value is greater than the x-value of the last node.
	  /// The behavior is undefined if called with any other x-value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="xValue">  the x-value to find the y-value for </param>
	  /// <returns> the extrapolated y-value for the specified x-value </returns>
	  /// <exception cref="RuntimeException"> if the y-value cannot be calculated </exception>
	  double rightExtrapolate(double xValue);

	  /// <summary>
	  /// Calculates the first derivative of the right extrapolated y-value at the specified x-value.
	  /// <para>
	  /// This method is only intended to be invoked when the x-value is greater than the x-value of the last node.
	  /// The behavior is undefined if called with any other x-value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="xValue">  the x-value to find the y-value for </param>
	  /// <returns> the first derivative of the extrapolated y-value for the specified x-value </returns>
	  /// <exception cref="RuntimeException"> if the derivative cannot be calculated </exception>
	  double rightExtrapolateFirstDerivative(double xValue);

	  /// <summary>
	  /// Calculates the parameter sensitivities of the right extrapolated y-value at the specified x-value.
	  /// <para>
	  /// This method is only intended to be invoked when the x-value is greater than the x-value of the last node.
	  /// The behavior is undefined if called with any other x-value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="xValue">  the x-value to find the y-value for </param>
	  /// <returns> the parameter sensitivities of the extrapolated y-value for the specified x-value </returns>
	  /// <exception cref="RuntimeException"> if the sensitivity cannot be calculated </exception>
	  DoubleArray rightExtrapolateParameterSensitivity(double xValue);

	}

}