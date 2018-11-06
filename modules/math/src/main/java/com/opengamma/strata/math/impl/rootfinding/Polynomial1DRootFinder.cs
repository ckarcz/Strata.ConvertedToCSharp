/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding
{
	using RealPolynomialFunction1D = com.opengamma.strata.math.impl.function.RealPolynomialFunction1D;

	/// <summary>
	/// Interface for classes that find the roots of a polynomial function <seealso cref="RealPolynomialFunction1D"/>.
	/// Although the coefficients of the polynomial function must be real, the roots can be real or complex. </summary>
	/// @param <T> Type of the roots. </param>
	public interface Polynomial1DRootFinder<T>
	{

	  /// <param name="function"> The function, not null </param>
	  /// <returns> The roots of the function </returns>
	  T[] getRoots(RealPolynomialFunction1D function);

	}

}