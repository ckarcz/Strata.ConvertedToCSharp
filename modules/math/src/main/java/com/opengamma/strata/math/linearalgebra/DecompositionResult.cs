/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.linearalgebra
{
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Contains the results of matrix decomposition.
	/// <para>
	/// The decomposed matrices (such as the L and U matrices for LU decomposition) are stored in this class.
	/// There are methods that allow calculations to be performed using these matrices.
	/// </para>
	/// </summary>
	public interface DecompositionResult
	{

	  /// <summary>
	  /// Solves $\mathbf{A}x = b$ where $\mathbf{A}$ is a (decomposed) matrix and $b$ is a vector.
	  /// </summary>
	  /// <param name="input">  the vector to calculate with </param>
	  /// <returns> the vector x </returns>
	  DoubleArray solve(DoubleArray input);

	  /// <summary>
	  /// Solves $\mathbf{A}x = b$ where $\mathbf{A}$ is a (decomposed) matrix and $b$ is a vector.
	  /// </summary>
	  /// <param name="input">  the vector to calculate with </param>
	  /// <returns> the vector x  </returns>
	  double[] solve(double[] input);

	  /// <summary>
	  /// Solves $\mathbf{A}x = \mathbf{B}$ where $\mathbf{A}$ is a (decomposed) matrix and $\mathbf{B}$ is a matrix.
	  /// </summary>
	  /// <param name="input">  the matrix to calculate with </param>
	  /// <returns> the matrix x </returns>
	  DoubleMatrix solve(DoubleMatrix input);

	}

}