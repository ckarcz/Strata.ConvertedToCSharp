/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.integration
{

	/// <summary>
	/// Interface for integration. The function to be integrated can be multi-dimensional. The result
	/// of the integration does not have to be the same type as the integration bounds.
	/// </summary>
	/// @param <T> Type of the function output and result </param>
	/// @param <U> Type of the integration bounds </param>
	/// @param <V> Type of the function to be integrated (e.g. <seealso cref="Function"/>, </param>
	public interface Integrator<T, U, V>
	{

	  /// <param name="f"> The function to be integrated, not null </param>
	  /// <param name="lower"> The array of lower bounds of integration, not null or empty </param>
	  /// <param name="upper"> The array of upper bounds of integration, not null or empty </param>
	  /// <returns> The result of the integral </returns>
	  T integrate(V f, U[] lower, U[] upper);

	}

}