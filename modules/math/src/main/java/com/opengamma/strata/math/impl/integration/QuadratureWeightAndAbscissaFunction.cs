/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.integration
{
	using OrthogonalPolynomialFunctionGenerator = com.opengamma.strata.math.impl.function.special.OrthogonalPolynomialFunctionGenerator;

	/// <summary>
	/// Interface for classes that generate weights and abscissas for use in Gaussian quadrature. The abscissas are the roots
	/// of an orthogonal polynomial <seealso cref="OrthogonalPolynomialFunctionGenerator"/>.
	/// </summary>
	public interface QuadratureWeightAndAbscissaFunction
	{

	  /// <param name="n"> The number of weights and abscissas to generate, not negative or zero </param>
	  /// <returns> An object containing the weights and abscissas </returns>
	  GaussianQuadratureData generate(int n);

	}

}