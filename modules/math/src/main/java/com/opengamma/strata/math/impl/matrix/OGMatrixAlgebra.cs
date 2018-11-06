using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.matrix
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Matrix = com.opengamma.strata.collect.array.Matrix;
	using TridiagonalMatrix = com.opengamma.strata.math.impl.linearalgebra.TridiagonalMatrix;

	/// <summary>
	/// A minimal implementation of matrix algebra.
	/// <para>
	/// This includes only some of the multiplications.
	/// For more advanced operations, such as calculating the inverse, use <seealso cref="CommonsMatrixAlgebra"/>.
	/// </para>
	/// </summary>
	// CSOFF: AbbreviationAsWordInName
	public class OGMatrixAlgebra : MatrixAlgebra
	{

	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <exception cref="UnsupportedOperationException"> always </exception>
	  public override double getCondition(Matrix m)
	  {
		throw new System.NotSupportedException();
	  }

	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <exception cref="UnsupportedOperationException"> always </exception>
	  public override double getDeterminant(Matrix m)
	  {
		throw new System.NotSupportedException();
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public override double getInnerProduct(Matrix m1, Matrix m2)
	  {
		ArgChecker.notNull(m1, "m1");
		ArgChecker.notNull(m2, "m2");
		if (m1 is DoubleArray && m2 is DoubleArray)
		{
		  DoubleArray array1 = (DoubleArray) m1;
		  DoubleArray array2 = (DoubleArray) m2;
		  return array1.combineReduce(array2, (r, a1, a2) => r + a1 * a2);
		}
		throw new System.ArgumentException("Can only find inner product of DoubleArray; have " + m1.GetType() + " and " + m2.GetType());
	  }

	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <exception cref="UnsupportedOperationException"> always </exception>
	  public override DoubleMatrix getInverse(Matrix m)
	  {
		throw new System.NotSupportedException();
	  }

	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <exception cref="UnsupportedOperationException"> always </exception>
	  public override double getNorm1(Matrix m)
	  {
		throw new System.NotSupportedException();
	  }

	  /// <summary>
	  /// {@inheritDoc} This is only implemented for <seealso cref="DoubleArray"/>. </summary>
	  /// <exception cref="IllegalArgumentException"> If the matrix is not a <seealso cref="DoubleArray"/> </exception>
	  public override double getNorm2(Matrix m)
	  {
		ArgChecker.notNull(m, "m");
		if (m is DoubleArray)
		{
		  DoubleArray array = (DoubleArray) m;
		  return Math.Sqrt(array.reduce(0d, (r, v) => r + v * v));

		}
		else if (m is DoubleMatrix)
		{
		  throw new System.NotSupportedException();
		}
		throw new System.ArgumentException("Can only find norm2 of a DoubleArray; have " + m.GetType());
	  }

	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <exception cref="UnsupportedOperationException"> always </exception>
	  public override double getNormInfinity(Matrix m)
	  {
		throw new System.NotSupportedException();
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public override DoubleMatrix getOuterProduct(Matrix m1, Matrix m2)
	  {
		ArgChecker.notNull(m1, "m1");
		ArgChecker.notNull(m2, "m2");
		if (m1 is DoubleArray && m2 is DoubleArray)
		{
		  DoubleArray array1 = (DoubleArray) m1;
		  DoubleArray array2 = (DoubleArray) m2;
		  return DoubleMatrix.of(array1.size(), array2.size(), (i, j) => array1.get(i) * array2.get(j));
		}
		throw new System.ArgumentException("Can only find outer product of DoubleArray; have " + m1.GetType() + " and " + m2.GetType());
	  }

	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <exception cref="UnsupportedOperationException"> always </exception>
	  public override DoubleMatrix getPower(Matrix m, int p)
	  {
		throw new System.NotSupportedException();
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public override double getTrace(Matrix m)
	  {
		ArgChecker.notNull(m, "m");
		if (m is DoubleMatrix)
		{
		  DoubleMatrix matrix = (DoubleMatrix) m;
		  ArgChecker.isTrue(matrix.Square, "Matrix not square");
		  double sum = 0d;
		  for (int i = 0; i < matrix.rowCount(); i++)
		  {
			sum += matrix.get(i, i);
		  }
		  return sum;
		}
		throw new System.ArgumentException("Can only take the trace of DoubleMatrix; have " + m.GetType());
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public override DoubleMatrix getTranspose(Matrix m)
	  {
		ArgChecker.notNull(m, "m");
		if (m is DoubleMatrix)
		{
		  DoubleMatrix matrix = (DoubleMatrix) m;
		  return DoubleMatrix.of(matrix.columnCount(), matrix.rowCount(), (i, j) => matrix.get(j, i));
		}
		throw new System.ArgumentException("Can only take transpose of DoubleMatrix; have " + m.GetType());
	  }

	  /// <summary>
	  /// {@inheritDoc} The following combinations of input matrices m1 and m2 are allowed:
	  /// <ul>
	  /// <li>m1 = 2-D matrix, m2 = 2-D matrix, returns $\mathbf{C} = \mathbf{AB}$
	  /// <li>m1 = 2-D matrix, m2 = 1-D matrix, returns $\mathbf{C} = \mathbf{A}b$
	  /// <li>m1 = 1-D matrix, m2 = 2-D matrix, returns $\mathbf{C} = a^T\mathbf{B}$
	  /// </ul>
	  /// </summary>
	  public override Matrix multiply(Matrix m1, Matrix m2)
	  {
		ArgChecker.notNull(m1, "m1");
		ArgChecker.notNull(m2, "m2");
		if (m1 is TridiagonalMatrix && m2 is DoubleArray)
		{
		  return multiply((TridiagonalMatrix) m1, (DoubleArray) m2);
		}
		else if (m1 is DoubleArray && m2 is TridiagonalMatrix)
		{
		  return multiply((DoubleArray) m1, (TridiagonalMatrix) m2);
		}
		else if (m1 is DoubleMatrix && m2 is DoubleMatrix)
		{
		  return multiply((DoubleMatrix) m1, (DoubleMatrix) m2);
		}
		else if (m1 is DoubleMatrix && m2 is DoubleArray)
		{
		  return multiply((DoubleMatrix) m1, (DoubleArray) m2);
		}
		else if (m1 is DoubleArray && m2 is DoubleMatrix)
		{
		  return multiply((DoubleArray) m1, (DoubleMatrix) m2);
		}
		throw new System.ArgumentException("Can only multiply two DoubleMatrix; a DoubleMatrix and a DoubleArray; " + "or a DoubleArray and a DoubleMatrix. have " + m1.GetType() + " and " + m2.GetType());
	  }

	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <exception cref="UnsupportedOperationException"> always </exception>
	  public override DoubleMatrix getPower(Matrix m, double p)
	  {
		throw new System.NotSupportedException();
	  }

	  private DoubleMatrix multiply(DoubleMatrix m1, DoubleMatrix m2)
	  {
		int p = m2.rowCount();
		ArgChecker.isTrue(m1.columnCount() == p, "Matrix size mismatch. m1 is " + m1.rowCount() + " by " + m1.columnCount() + ", but m2 is " + m2.rowCount() + " by " + m2.columnCount());
		return DoubleMatrix.of(m1.rowCount(), m2.columnCount(), (i, j) =>
		{
		double sum = 0d;
		for (int k = 0; k < p; k++)
		{
			sum += m1.get(i, k) * m2.get(k, j);
		}
		return sum;
		});
	  }

	  private DoubleArray multiply(DoubleMatrix matrix, DoubleArray vector)
	  {
		int n = vector.size();
		ArgChecker.isTrue(matrix.columnCount() == n, "Matrix/vector size mismatch");
		return DoubleArray.of(matrix.rowCount(), i =>
		{
		double sum = 0;
		for (int j = 0; j < n; j++)
		{
			sum += matrix.get(i, j) * vector.get(j);
		}
		return sum;
		});
	  }

	  private DoubleArray multiply(TridiagonalMatrix matrix, DoubleArray vector)
	  {
		double[] a = matrix.LowerSubDiagonalData;
		double[] b = matrix.DiagonalData;
		double[] c = matrix.UpperSubDiagonalData;
		double[] x = vector.toArrayUnsafe();
		int n = x.Length;
		ArgChecker.isTrue(b.Length == n, "Matrix/vector size mismatch");
		double[] res = new double[n];
		int i;
		res[0] = b[0] * x[0] + c[0] * x[1];
		res[n - 1] = b[n - 1] * x[n - 1] + a[n - 2] * x[n - 2];
		for (i = 1; i < n - 1; i++)
		{
		  res[i] = a[i - 1] * x[i - 1] + b[i] * x[i] + c[i] * x[i + 1];
		}
		return DoubleArray.ofUnsafe(res);
	  }

	  private DoubleArray multiply(DoubleArray vector, DoubleMatrix matrix)
	  {
		int n = vector.size();
		ArgChecker.isTrue(matrix.rowCount() == n, "Matrix/vector size mismatch");
		return DoubleArray.of(matrix.columnCount(), i =>
		{
		double sum = 0;
		for (int j = 0; j < n; j++)
		{
			sum += vector.get(j) * matrix.get(j, i);
		}
		return sum;
		});
	  }

	  private DoubleArray multiply(DoubleArray vector, TridiagonalMatrix matrix)
	  {
		double[] a = matrix.LowerSubDiagonalData;
		double[] b = matrix.DiagonalData;
		double[] c = matrix.UpperSubDiagonalData;
		double[] x = vector.toArrayUnsafe();
		int n = x.Length;
		ArgChecker.isTrue(b.Length == n, "Matrix/vector size mismatch");
		double[] res = new double[n];
		int i;
		res[0] = b[0] * x[0] + a[0] * x[1];
		res[n - 1] = b[n - 1] * x[n - 1] + c[n - 2] * x[n - 2];
		for (i = 1; i < n - 1; i++)
		{
		  res[i] = a[i] * x[i + 1] + b[i] * x[i] + c[i - 1] * x[i - 1];
		}
		return DoubleArray.ofUnsafe(res);
	  }

	}

}