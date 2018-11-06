/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.surface.interpolator
{
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Interface for interpolators that interpolate a surface.
	/// </summary>
	public interface SurfaceInterpolator
	{

	  /// <summary>
	  /// Binds this interpolator to a surface.
	  /// <para>
	  /// The bind process takes the definition of the interpolator and combines it with the x-y-z values.
	  /// This allows implementations to optimize interpolation calculations.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="xValues">  the x-values of the surface, must be sorted from low to high </param>
	  /// <param name="yValues">  the y-values of the surface, must be sorted from low to high within x </param>
	  /// <param name="zValues">  the z-values of the surface </param>
	  /// <returns> the bound interpolator </returns>
	  BoundSurfaceInterpolator bind(DoubleArray xValues, DoubleArray yValues, DoubleArray zValues);

	}

}