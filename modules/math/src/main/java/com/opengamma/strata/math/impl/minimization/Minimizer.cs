/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{

	/// <summary>
	/// Interface that finds the minimum value of a function. The function must be one-dimensional but the input type is not constrained </summary>
	/// @param <F> The type of the function </param>
	/// @param <S> The type of the start position for the minimization </param>
	public interface Minimizer<F, S>
	{

	  /// <param name="function"> The function to be minimized, not null </param>
	  /// <param name="startPosition"> The start position </param>
	  /// <returns> The minimum </returns>
	  S minimize(F function, S startPosition);

	}

}