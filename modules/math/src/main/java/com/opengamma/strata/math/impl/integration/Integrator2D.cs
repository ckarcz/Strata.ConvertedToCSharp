/*
 * Copyright (C) 2011 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.integration
{

	/// <summary>
	/// Class for defining the integration of 2-D functions.
	/// </summary>
	/// @param <T> the type of the function output and result </param>
	/// @param <U> the type of the function inputs and integration bounds </param>
	public abstract class Integrator2D<T, U> : Integrator<T, U, System.Func<U, U, T>>
	{
		public abstract T integrate(V f, U[] lower, U[] upper);

	}

}