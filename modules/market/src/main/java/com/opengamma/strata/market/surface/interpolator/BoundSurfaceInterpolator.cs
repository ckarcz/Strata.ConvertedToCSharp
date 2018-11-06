/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.surface.interpolator
{
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// A surface interpolator that has been bound to a specific surface.
	/// <para>
	/// A bound interpolator is created from a <seealso cref="SurfaceInterpolator"/>.
	/// The bind process takes the definition of the interpolator and combines it with the x-y-z values.
	/// This allows implementations to optimize interpolation calculations.
	/// </para>
	/// </summary>
	public interface BoundSurfaceInterpolator
	{

	  /// <summary>
	  /// Computes the z-value for the specified x-y-value by interpolation.
	  /// </summary>
	  /// <param name="x">  the x-value to find the z-value for </param>
	  /// <param name="y">  the y-value to find the z-value for </param>
	  /// <returns> the value at the x-y-value </returns>
	  /// <exception cref="RuntimeException"> if the z-value cannot be calculated </exception>
	  double interpolate(double x, double y);

	  /// <summary>
	  /// Computes the sensitivity of the x-y-value with respect to the surface parameters.
	  /// <para>
	  /// This returns an array with one element for each parameter of the surface.
	  /// The array contains the sensitivity of the z-value at the specified x-y-value to each parameter.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="x">  the x-value at which the parameter sensitivity is computed </param>
	  /// <param name="y">  the y-value at which the parameter sensitivity is computed </param>
	  /// <returns> the sensitivity </returns>
	  /// <exception cref="RuntimeException"> if the sensitivity cannot be calculated </exception>
	  DoubleArray parameterSensitivity(double x, double y);

	}

}