/*
 * Copyright (C) 2011 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.linearalgebra
{
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using DecompositionResult = com.opengamma.strata.math.linearalgebra.DecompositionResult;

	/// <summary>
	/// Contains the results of Cholesky matrix decomposition.
	/// </summary>
	// CSOFF: AbbreviationAsWordInName
	public interface CholeskyDecompositionResult : DecompositionResult
	{

	  /// <summary>
	  /// Returns the $\mathbf{L}$ matrix of the decomposition.
	  /// <para>
	  /// $\mathbf{L}$ is a lower-triangular matrix.
	  /// </para>
	  /// </summary>
	  /// <returns> the $\mathbf{L}$ matrix </returns>
	  DoubleMatrix L {get;}

	  /// <summary>
	  /// Returns the transpose of the matrix $\mathbf{L}$ of the decomposition.
	  /// <para>
	  /// $\mathbf{L}^T$ is a upper-triangular matrix.
	  /// </para>
	  /// </summary>
	  /// <returns> the $\mathbf{L}^T$ matrix </returns>
	  DoubleMatrix LT {get;}

	  /// <summary>
	  /// Return the determinant of the matrix. </summary>
	  /// <returns> determinant of the matrix </returns>
	  double Determinant {get;}

	}

}