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
	/// Contains the results of QR matrix decomposition.
	/// </summary>
	// CSOFF: AbbreviationAsWordInName
	public interface QRDecompositionResult : DecompositionResult
	{

	  /// <summary>
	  /// Returns the matrix $\mathbf{R}$ of the decomposition.
	  /// <para>
	  /// $\mathbf{R}$ is an upper-triangular matrix.
	  /// </para>
	  /// </summary>
	  /// <returns> the $\mathbf{R}$ matrix </returns>
	  DoubleMatrix R {get;}

	  /// <summary>
	  /// Returns the matrix $\mathbf{Q}$ of the decomposition.
	  /// <para>
	  /// $\mathbf{Q}$ is an orthogonal matrix.
	  /// </para>
	  /// </summary>
	  /// <returns> the $\mathbf{Q}$ matrix </returns>
	  DoubleMatrix Q {get;}

	  /// <summary>
	  /// Returns the transpose of the matrix $\mathbf{Q}$ of the decomposition.
	  /// <para>
	  /// $\mathbf{Q}$ is an orthogonal matrix.
	  /// </para>
	  /// </summary>
	  /// <returns> the $\mathbf{Q}$ matrix </returns>
	  DoubleMatrix QT {get;}

	}

}