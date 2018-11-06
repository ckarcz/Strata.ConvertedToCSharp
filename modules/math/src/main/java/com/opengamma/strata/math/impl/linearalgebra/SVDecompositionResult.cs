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
	/// Contains the results of SV matrix decomposition.
	/// </summary>
	// CSOFF: AbbreviationAsWordInName
	public interface SVDecompositionResult : DecompositionResult
	{

	  /// <summary>
	  /// Returns the matrix $\mathbf{U}$ of the decomposition.
	  /// <para>
	  /// $\mathbf{U}$ is an orthogonal matrix, i.e. its transpose is also its inverse.
	  /// </para>
	  /// </summary>
	  /// <returns> the $\mathbf{U}$ matrix </returns>
	  DoubleMatrix U {get;}

	  /// <summary>
	  /// Returns the transpose of the matrix $\mathbf{U}$ of the decomposition.
	  /// <para>
	  /// $\mathbf{U}$ is an orthogonal matrix, i.e. its transpose is also its inverse.
	  /// </para>
	  /// </summary>
	  /// <returns> the U matrix (or null if decomposed matrix is singular) </returns>
	  DoubleMatrix UT {get;}

	  /// <summary>
	  /// Returns the diagonal matrix $\mathbf{\Sigma}$ of the decomposition.
	  /// <para>
	  /// $\mathbf{\Sigma}$ is a diagonal matrix. The singular values are provided in
	  /// non-increasing order.
	  /// </para>
	  /// </summary>
	  /// <returns> the $\mathbf{\Sigma}$ matrix </returns>
	  DoubleMatrix S {get;}

	  /// <summary>
	  /// Returns the diagonal elements of the matrix $\mathbf{\Sigma}$ of the decomposition.
	  /// <para>
	  /// The singular values are provided in non-increasing order.
	  /// </para>
	  /// </summary>
	  /// <returns> the diagonal elements of the $\mathbf{\Sigma}$ matrix </returns>
	  double[] SingularValues {get;}

	  /// <summary>
	  /// Returns the matrix $\mathbf{V}$ of the decomposition.
	  /// <para>
	  /// $\mathbf{V}$ is an orthogonal matrix, i.e. its transpose is also its inverse.
	  /// </para>
	  /// </summary>
	  /// <returns> the $\mathbf{V}$ matrix </returns>
	  DoubleMatrix V {get;}

	  /// <summary>
	  /// Returns the transpose of the matrix $\mathbf{V}$ of the decomposition.
	  /// <para>
	  /// $\mathbf{V}$ is an orthogonal matrix, i.e. its transpose is also its inverse.
	  /// </para>
	  /// </summary>
	  /// <returns> the $\mathbf{V}$ matrix </returns>
	  DoubleMatrix VT {get;}

	  /// <summary>
	  /// Returns the $L_2$ norm of the matrix.
	  /// <para>
	  /// The $L_2$ norm is $\max\left(\frac{|\mathbf{A} \times U|_2}{|U|_2}\right)$, where $|.|_2$ denotes the vectorial 2-norm
	  /// (i.e. the traditional Euclidian norm).
	  /// </para>
	  /// </summary>
	  /// <returns> norm </returns>
	  double Norm {get;}

	  /// <summary>
	  /// Returns the condition number of the matrix. </summary>
	  /// <returns> condition number of the matrix </returns>
	  double ConditionNumber {get;}

	  /// <summary>
	  /// Returns the effective numerical matrix rank.
	  /// <para>The effective numerical rank is the number of non-negligible
	  /// singular values. The threshold used to identify non-negligible
	  /// terms is $\max(m, n) \times \mathrm{ulp}(S_1)$, where $\mathrm{ulp}(S_1)$  
	  /// is the least significant bit of the largest singular value.
	  /// </para>
	  /// </summary>
	  /// <returns> effective numerical matrix rank </returns>
	  int Rank {get;}

	}

}