/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.linearalgebra
{

	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Base interface for matrix decompositions, such as SVD and LU.
	/// </summary>
	/// @param <R> the type of the decomposition result </param>
	public interface Decomposition<R> : System.Func<DoubleMatrix, R> where R : DecompositionResult
	{

	  /// <summary>
	  /// Applies this function to the given argument.
	  /// </summary>
	  /// <param name="input">  the input matrix </param>
	  /// <returns> the resulting decomposition </returns>
	  R apply(DoubleMatrix input);

	}

}