/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.linearalgebra
{
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using DecompositionResult = com.opengamma.strata.math.linearalgebra.DecompositionResult;

	/// <summary>
	/// Contains the results of LU matrix decomposition.
	/// </summary>
	// CSOFF: AbbreviationAsWordInName
	public interface LUDecompositionResult : DecompositionResult
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
	  /// Returns the $\mathbf{U}$ matrix of the decomposition.
	  /// <para>
	  /// $\mathbf{U}$ is an upper-triangular matrix.
	  /// </para>
	  /// </summary>
	  /// <returns> the U matrix </returns>
	  DoubleMatrix U {get;}

	  /// <summary>
	  /// Returns the rows permutation matrix, $\mathbf{P}$.
	  /// <para>
	  /// P is a sparse matrix with exactly one element set to 1.0 in
	  /// each row and each column, all other elements being set to 0.0.
	  /// </para>
	  /// <para>
	  /// The positions of the 1 elements are given by the {@link #getPivot()
	  /// pivot permutation vector}.
	  /// </para>
	  /// </summary>
	  /// <returns> the $\mathbf{P}$ rows permutation matrix </returns>
	  /// <seealso cref= #getPivot() </seealso>
	  DoubleMatrix P {get;}

	  /// <summary>
	  /// Returns the pivot permutation vector. </summary>
	  /// <returns> the pivot permutation vector </returns>
	  int[] Pivot {get;}

	  /// <summary>
	  /// Return the determinant of the matrix. </summary>
	  /// <returns> determinant of the matrix </returns>
	  double Determinant {get;}

	}

}