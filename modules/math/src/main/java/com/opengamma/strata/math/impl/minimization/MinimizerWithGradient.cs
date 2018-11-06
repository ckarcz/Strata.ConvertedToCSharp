/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{

	/// <summary>
	/// Interface for classes that extends the functionality of <seealso cref="Minimizer"/> by providing a method that takes a gradient function. </summary>
	/// @param <F> The type of the function to minimize </param>
	/// @param <G> The type of the gradient function </param>
	/// @param <S> The type of the start position of the minimization </param>
	public interface MinimizerWithGradient<F, G, S> : Minimizer<F, S>
	{

	  /// <param name="function"> The function to minimize, not null </param>
	  /// <param name="gradient"> The gradient function, not null </param>
	  /// <param name="startPosition"> The start position, not null </param>
	  /// <returns> The minimum </returns>
	  S minimize(F function, G gradient, S startPosition);

	}

}