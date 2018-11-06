using System;

/*
 * Copyright (C) 2011 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.linearalgebra
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using OGMatrixAlgebra = com.opengamma.strata.math.impl.matrix.OGMatrixAlgebra;

	/// <summary>
	/// Results of the OpenGamma implementation of Cholesky decomposition.
	/// </summary>
	public class CholeskyDecompositionOpenGammaResult : CholeskyDecompositionResult
	{

	  private static readonly MatrixAlgebra ALGEBRA = new OGMatrixAlgebra();
	  /// <summary>
	  /// The array that store the data.
	  /// </summary>
	  private readonly double[][] _lArray;
	  /// <summary>
	  /// The matrix L, result of the decomposition.
	  /// </summary>
	  private readonly DoubleMatrix _l;
	  /// <summary>
	  /// The matrix L^T, result of the decomposition.
	  /// </summary>
	  private readonly DoubleMatrix _lT;
	  /// <summary>
	  /// The determinant of the original matrix A = L L^T.
	  /// </summary>
	  private double _determinant;

	  /// <summary>
	  /// Constructor. </summary>
	  /// <param name="lArray"> The matrix L as an array of doubles. </param>
	  public CholeskyDecompositionOpenGammaResult(double[][] lArray)
	  {
		_lArray = lArray;
		_l = DoubleMatrix.copyOf(_lArray);
		_lT = ALGEBRA.getTranspose(_l);
		_determinant = 1.0;
		for (int loopdiag = 0; loopdiag < _lArray.Length; ++loopdiag)
		{
		  _determinant *= _lArray[loopdiag][loopdiag] * _lArray[loopdiag][loopdiag];
		}
	  }

	  public virtual DoubleArray solve(DoubleArray b)
	  {
		return b;
	  }

	  public virtual double[] solve(double[] b)
	  {
		int dim = b.Length;
		ArgChecker.isTrue(dim == _lArray.Length, "b array of incorrect size");
		double[] x = new double[dim];
		Array.Copy(b, 0, x, 0, dim);
		// L y = b (y stored in x array)
		for (int looprow = 0; looprow < dim; looprow++)
		{
		  x[looprow] /= _lArray[looprow][looprow];
		  for (int j = looprow + 1; j < dim; j++)
		  {
			x[j] -= x[looprow] * _lArray[j][looprow];
		  }
		}
		// L^T x = y
		for (int looprow = dim - 1; looprow >= -0; looprow--)
		{
		  x[looprow] /= _lArray[looprow][looprow];
		  for (int j = 0; j < looprow; j++)
		  {
			x[j] -= x[looprow] * _lArray[looprow][j];
		  }
		}
		return x;
	  }

	  public virtual DoubleMatrix solve(DoubleMatrix b)
	  {
		int nbRow = b.rowCount();
		int nbCol = b.columnCount();
		ArgChecker.isTrue(nbRow == _lArray.Length, "b array of incorrect size");
		double[][] x = b.toArray();
		// L Y = B (Y stored in x array)
		for (int loopcol = 0; loopcol < nbCol; loopcol++)
		{
		  for (int looprow = 0; looprow < nbRow; looprow++)
		  {
			x[looprow][loopcol] /= _lArray[looprow][looprow];
			for (int j = looprow + 1; j < nbRow; j++)
			{
			  x[j][loopcol] -= x[looprow][loopcol] * _lArray[j][looprow];
			}
		  }
		}
		// L^T X = Y
		for (int loopcol = 0; loopcol < nbCol; loopcol++)
		{
		  for (int looprow = nbRow - 1; looprow >= -0; looprow--)
		  {
			x[looprow][loopcol] /= _lArray[looprow][looprow];
			for (int j = 0; j < looprow; j++)
			{
			  x[j][loopcol] -= x[looprow][loopcol] * _lArray[looprow][j];
			}
		  }
		}
		return DoubleMatrix.copyOf(x);
	  }

	  public virtual DoubleMatrix L
	  {
		  get
		  {
			return _l;
		  }
	  }

	  public virtual DoubleMatrix LT
	  {
		  get
		  {
			return _lT;
		  }
	  }

	  public virtual double Determinant
	  {
		  get
		  {
			return _determinant;
		  }
	  }

	}

}