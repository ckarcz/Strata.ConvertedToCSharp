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

	/// <summary>
	/// Parent class for matrix algebra operations. Basic operations (add, subtract, scale) are implemented in this class.
	/// </summary>
	public abstract class MatrixAlgebra
	{

	  /// <summary>
	  /// Adds two matrices. This operation can only be performed if the matrices are of the same type and dimensions. </summary>
	  /// <param name="m1"> The first matrix, not null </param>
	  /// <param name="m2"> The second matrix, not null </param>
	  /// <returns> The sum of the two matrices </returns>
	  /// <exception cref="IllegalArgumentException"> If the matrices are not of the same type, if the matrices are not the same shape. </exception>
	  public virtual Matrix add(Matrix m1, Matrix m2)
	  {
		ArgChecker.notNull(m1, "m1");
		ArgChecker.notNull(m2, "m2");
		if (m1 is DoubleArray)
		{
		  if (m2 is DoubleArray)
		  {
			DoubleArray array1 = (DoubleArray) m1;
			DoubleArray array2 = (DoubleArray) m2;
			return array1.plus(array2);
		  }
		  throw new System.ArgumentException("Tried to add a " + m1.GetType() + " and " + m2.GetType());

		}
		else if (m1 is DoubleMatrix)
		{
		  if (m2 is DoubleMatrix)
		  {
			DoubleMatrix matrix1 = (DoubleMatrix) m1;
			DoubleMatrix matrix2 = (DoubleMatrix) m2;
			return matrix1.plus(matrix2);
		  }
		  throw new System.ArgumentException("Tried to add a " + m1.GetType() + " and " + m2.GetType());
		}
		throw new System.NotSupportedException();
	  }

	  /// <summary>
	  /// Returns the quotient of two matrices $C = \frac{A}{B} = AB^{-1}$, where
	  /// $B^{-1}$ is the pseudo-inverse of $B$ i.e. $BB^{-1} = \mathbb{1}$. </summary>
	  /// <param name="m1"> The numerator matrix, not null. This matrix must be a <seealso cref="DoubleMatrix"/>. </param>
	  /// <param name="m2"> The denominator, not null. This matrix must be a <seealso cref="DoubleMatrix"/>. </param>
	  /// <returns> The result </returns>
	  public virtual Matrix divide(Matrix m1, Matrix m2)
	  {
		ArgChecker.notNull(m1, "m1");
		ArgChecker.notNull(m2, "m2");
		ArgChecker.isTrue(m1 is DoubleMatrix, "Can only divide a 2D matrix");
		ArgChecker.isTrue(m2 is DoubleMatrix, "Can only perform division with a 2D matrix");
		return multiply(m1, getInverse(m2));
	  }

	  /// <summary>
	  /// Returns the Kronecker product of two matrices. If $\mathbf{A}$ is an $m
	  /// \times n$ matrix and $\mathbf{B}$ is a $p \times q$ matrix, then the
	  /// Kronecker product $A \otimes B$ is an $mp \times nq$ matrix with elements
	  /// $$
	  /// \begin{align*}
	  /// A \otimes B &=
	  /// \begin{pmatrix}
	  /// a_{11}\mathbf{B} & \cdots & a_{1n}\mathbf{B} \\
	  /// \vdots & \ddots & \vdots \\
	  /// a_{m1}\mathbf{B} & \cdots & a_{mn}\mathbf{B}
	  /// \end{pmatrix}\\
	  /// &=
	  /// \begin{pmatrix}
	  /// a_{11}b_{11} & a_{11}b_{12} & \cdots & a_{11}b_{1q} & \cdots & a_{1n}b_{11} & a_{1n}b_{12} & \cdots & a_{1n}b_{1q}\\
	  /// a_{11}b_{21} & a_{11}b_{22} & \cdots & a_{11}b_{2q} & \cdots & a_{1n}b_{21} & a_{1n}b_{22} & \cdots & a_{1n}b_{2q} \\
	  /// \vdots & \vdots & \ddots & \vdots & \cdots & \vdots & \vdots & \ddots & \cdots \\
	  /// a_{11}b_{p1} & a_{11}b_{p2} & \cdots & a_{11}b_{pq} & \cdots & a_{1n}b_{p1} & a_{1n}b_{p2} & \cdots & a_{1n}b_{pq} \\
	  /// \vdots & \vdots & & \vdots & \ddots & \vdots & \vdots & & \cdots \\
	  /// a_{m1}b_{11} & a_{m1}b_{12} & \cdots & a_{m1}b_{1q} & \cdots & a_{mn}b_{11} & a_{mn}b_{12} & \cdots & a_{mn}b_{1q} \\
	  /// a_{m1}b_{21} & a_{m1}b_{22} & \cdots & a_{m1}b_{2q} & \cdots & a_{mn}b_{21} & a_{mn}b_{22} & \cdots & a_{mn}b_{2q} \\
	  /// \vdots & \vdots & \ddots & \vdots & \cdots & \vdots & \vdots & \ddots & \cdots \\
	  /// a_{m1}b_{p1} & a_{m1}b_{p2} & \cdots & a_{m1}b_{pq} & \cdots & a_{mn}b_{p1} & a_{mn}b_{p2} & \cdots & a_{mn}b_{pq}
	  /// \end{pmatrix}
	  /// \end{align*}
	  /// $$ </summary>
	  /// <param name="m1"> The first matrix, not null. This matrix must be a <seealso cref="DoubleMatrix"/>. </param>
	  /// <param name="m2"> The second matrix, not null. This matrix must be a <seealso cref="DoubleMatrix"/>. </param>
	  /// <returns> The Kronecker product </returns>
	  public virtual Matrix kroneckerProduct(Matrix m1, Matrix m2)
	  {
		ArgChecker.notNull(m1, "m1");
		ArgChecker.notNull(m2, "m2");
		if (m1 is DoubleMatrix && m2 is DoubleMatrix)
		{
		  DoubleMatrix matrix1 = (DoubleMatrix) m1;
		  DoubleMatrix matrix2 = (DoubleMatrix) m2;
		  int aRows = matrix1.rowCount();
		  int aCols = matrix1.columnCount();
		  int bRows = matrix2.rowCount();
		  int bCols = matrix2.columnCount();
		  int rRows = aRows * bRows;
		  int rCols = aCols * bCols;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[rRows][rCols];
		  double[][] res = RectangularArrays.ReturnRectangularDoubleArray(rRows, rCols);
		  for (int i = 0; i < aRows; i++)
		  {
			for (int j = 0; j < aCols; j++)
			{
			  double t = matrix1.get(i, j);
			  if (t != 0.0)
			  {
				for (int k = 0; k < bRows; k++)
				{
				  for (int l = 0; l < bCols; l++)
				  {
					res[i * bRows + k][j * bCols + l] = t * matrix2.get(k, l);
				  }
				}
			  }
			}
		  }
		  return DoubleMatrix.ofUnsafe(res);
		}
		throw new System.ArgumentException("Can only calculate the Kronecker product of two DoubleMatrix.");
	  }

	  /// <summary>
	  /// Multiplies two matrices. </summary>
	  /// <param name="m1"> The first matrix, not null. </param>
	  /// <param name="m2"> The second matrix, not null. </param>
	  /// <returns> The product of the two matrices. </returns>
	  public abstract Matrix multiply(Matrix m1, Matrix m2);

	  /// <summary>
	  /// Scale a vector or matrix by a given amount, i.e. each element is multiplied by the scale. </summary>
	  /// <param name="m"> A vector or matrix, not null </param>
	  /// <param name="scale"> The scale </param>
	  /// <returns> the scaled vector or matrix </returns>
	  public virtual Matrix scale(Matrix m, double scale)
	  {
		ArgChecker.notNull(m, "m");
		if (m is DoubleArray)
		{
		  return ((DoubleArray) m).multipliedBy(scale);

		}
		else if (m is DoubleMatrix)
		{
		  return ((DoubleMatrix) m).multipliedBy(scale);
		}
		throw new System.NotSupportedException();
	  }

	  /// <summary>
	  /// Subtracts two matrices. This operation can only be performed if the matrices are of the same type and dimensions. </summary>
	  /// <param name="m1"> The first matrix, not null </param>
	  /// <param name="m2"> The second matrix, not null </param>
	  /// <returns> The second matrix subtracted from the first </returns>
	  /// <exception cref="IllegalArgumentException"> If the matrices are not of the same type, if the matrices are not the same shape. </exception>
	  public virtual Matrix subtract(Matrix m1, Matrix m2)
	  {
		ArgChecker.notNull(m1, "m1");
		ArgChecker.notNull(m2, "m2");
		if (m1 is DoubleArray)
		{
		  if (m2 is DoubleArray)
		  {
			DoubleArray array1 = (DoubleArray) m1;
			DoubleArray array2 = (DoubleArray) m2;
			return array1.minus(array2);
		  }
		  throw new System.ArgumentException("Tried to subtract a " + m1.GetType() + " and " + m2.GetType());
		}
		else if (m1 is DoubleMatrix)
		{
		  if (m2 is DoubleMatrix)
		  {
			DoubleMatrix matrix1 = (DoubleMatrix) m1;
			DoubleMatrix matrix2 = (DoubleMatrix) m2;
			return matrix1.minus(matrix2);
		  }
		  throw new System.ArgumentException("Tried to subtract a " + m1.GetType() + " and " + m2.GetType());
		}
		throw new System.NotSupportedException();
	  }

	  /// <summary>
	  /// Returns the condition number of the matrix. </summary>
	  /// <param name="m"> A matrix, not null </param>
	  /// <returns> The condition number of the matrix </returns>
	  public abstract double getCondition(Matrix m);

	  /// <summary>
	  /// Returns the determinant of the matrix. </summary>
	  /// <param name="m"> A matrix, not null </param>
	  /// <returns> The determinant of the matrix </returns>
	  public abstract double getDeterminant(Matrix m);

	  /// <summary>
	  /// Returns the inverse (or pseudo-inverse) of the matrix. </summary>
	  /// <param name="m"> A matrix, not null </param>
	  /// <returns> The inverse matrix </returns>
	  public abstract DoubleMatrix getInverse(Matrix m);

	  /// <summary>
	  /// Returns the inner (or dot) product. </summary>
	  /// <param name="m1"> A vector, not null </param>
	  /// <param name="m2"> A vector, not null </param>
	  /// <returns> The scalar dot product </returns>
	  /// <exception cref="IllegalArgumentException"> If the vectors are not the same size </exception>
	  public abstract double getInnerProduct(Matrix m1, Matrix m2);

	  /// <summary>
	  /// Returns the outer product. </summary>
	  /// <param name="m1"> A vector, not null </param>
	  /// <param name="m2"> A vector, not null </param>
	  /// <returns> The outer product </returns>
	  /// <exception cref="IllegalArgumentException"> If the vectors are not the same size </exception>
	  public abstract DoubleMatrix getOuterProduct(Matrix m1, Matrix m2);

	  /// <summary>
	  /// For a vector, returns the <a href="http://mathworld.wolfram.com/L1-Norm.html">$L_1$ norm</a>
	  /// (also known as the Taxicab norm or Manhattan norm), i.e. $\Sigma |x_i|$.
	  /// <para>
	  /// For a matrix, returns the <a href="http://mathworld.wolfram.com/MaximumAbsoluteColumnSumNorm.html">maximum absolute column sum norm</a> of the matrix.
	  /// </para>
	  /// </summary>
	  /// <param name="m"> A vector or matrix, not null </param>
	  /// <returns> The $L_1$ norm </returns>
	  public abstract double getNorm1(Matrix m);

	  /// <summary>
	  /// For a vector, returns <a href="http://mathworld.wolfram.com/L2-Norm.html">$L_2$ norm</a> (also known as the
	  /// Euclidean norm).
	  /// <para>
	  /// For a matrix, returns the <a href="http://mathworld.wolfram.com/SpectralNorm.html">spectral norm</a>
	  /// </para>
	  /// </summary>
	  /// <param name="m"> A vector or matrix, not null </param>
	  /// <returns> the norm </returns>
	  public abstract double getNorm2(Matrix m);

	  /// <summary>
	  /// For a vector, returns the <a href="http://mathworld.wolfram.com/L-Infinity-Norm.html">$L_\infty$ norm</a>.
	  /// $L_\infty$ norm is the maximum of the absolute values of the elements.
	  /// <para>
	  /// For a matrix, returns the <a href="http://mathworld.wolfram.com/MaximumAbsoluteRowSumNorm.html">maximum absolute row sum norm</a>
	  /// </para>
	  /// </summary>
	  /// <param name="m"> a vector or a matrix, not null </param>
	  /// <returns> the norm </returns>
	  public abstract double getNormInfinity(Matrix m);

	  /// <summary>
	  /// Returns a matrix raised to an integer power, e.g. $\mathbf{A}^3 = \mathbf{A}\mathbf{A}\mathbf{A}$. </summary>
	  /// <param name="m"> A square matrix, not null </param>
	  /// <param name="p"> An integer power </param>
	  /// <returns> The result </returns>
	  public abstract DoubleMatrix getPower(Matrix m, int p);

	  /// <summary>
	  /// Returns a matrix raised to a power, $\mathbf{A}^3 = \mathbf{A}\mathbf{A}\mathbf{A}$. </summary>
	  /// <param name="m"> A square matrix, not null </param>
	  /// <param name="p"> The power </param>
	  /// <returns> The result </returns>
	  public abstract DoubleMatrix getPower(Matrix m, double p);

	  /// <summary>
	  /// Returns the trace (i.e. sum of diagonal elements) of a matrix. </summary>
	  /// <param name="m"> A matrix, not null. The matrix must be square. </param>
	  /// <returns> The trace </returns>
	  public abstract double getTrace(Matrix m);

	  /// <summary>
	  /// Returns the transpose of a matrix. </summary>
	  /// <param name="m"> A matrix, not null </param>
	  /// <returns> The transpose matrix </returns>
	  public abstract DoubleMatrix getTranspose(Matrix m);

	  /// <summary>
	  /// Compute $A^T A$, where A is a matrix. </summary>
	  /// <param name="a"> The matrix </param>
	  /// <returns> The result of $A^T A$ </returns>
	  public virtual DoubleMatrix matrixTransposeMultiplyMatrix(DoubleMatrix a)
	  {
		ArgChecker.notNull(a, "a");
		int n = a.rowCount();
		int m = a.columnCount();

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] data = new double[m][m];
		double[][] data = RectangularArrays.ReturnRectangularDoubleArray(m, m);
		for (int i = 0; i < m; i++)
		{
		  double sum = 0d;
		  for (int k = 0; k < n; k++)
		  {
			sum += a.get(k, i) * a.get(k, i);
		  }
		  data[i][i] = sum;

		  for (int j = i + 1; j < m; j++)
		  {
			sum = 0d;
			for (int k = 0; k < n; k++)
			{
			  sum += a.get(k, i) * a.get(k, j);
			}
			data[i][j] = sum;
			data[j][i] = sum;
		  }
		}
		return DoubleMatrix.ofUnsafe(data);
	  }

	}

}