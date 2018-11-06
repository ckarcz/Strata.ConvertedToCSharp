/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.differentiation
{

	/// <summary>
	/// Given a one-dimensional function (see <seealso cref="Function"/>), returns a function that calculates the gradient.
	/// </summary>
	/// @param <S> the domain type of the function </param>
	/// @param <T> the range type of the function </param>
	/// @param <U> the range type of the differential </param>
	public interface Differentiator<S, T, U>
	{

	  /// <summary>
	  /// Provides a function that performs the differentiation.
	  /// </summary>
	  /// <param name="function">  a function for which to get the differential function </param>
	  /// <returns> a function that calculates the differential </returns>
	  System.Func<S, U> differentiate(System.Func<S, T> function);

	  /// <summary>
	  /// Provides a function that performs the differentiation.
	  /// </summary>
	  /// <param name="function">  a function for which to get the differential function </param>
	  /// <param name="domain">  a function that returns false if the requested value is not in  the domain, true otherwise </param>
	  /// <returns> a function that calculates the differential </returns>
	  System.Func<S, U> differentiate(System.Func<S, T> function, System.Func<S, bool> domain);

	}

}