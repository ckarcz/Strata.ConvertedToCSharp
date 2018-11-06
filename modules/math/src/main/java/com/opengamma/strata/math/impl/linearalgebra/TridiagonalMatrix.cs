/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.linearalgebra
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Matrix = com.opengamma.strata.collect.array.Matrix;

	/// <summary>
	/// Class representing a tridiagonal matrix.
	/// $$
	/// \begin{align*}
	/// \begin{pmatrix}
	/// a_1     & b_1     & 0       & \cdots  & 0       & 0       & 0        \\
	/// c_1     & a_2     & b_2     & \cdots  & 0       & 0       & 0        \\
	/// 0       &         & \ddots  &         & \vdots  & \vdots  & \vdots   \\
	/// 0       & 0       & 0       &         & c_{n-2} & a_{n-1} & b_{n-1}  \\
	/// 0       & 0       & 0       & \cdots  & 0       & c_{n-1} & a_n     
	/// \end{pmatrix}
	/// \end{align*}
	/// $$
	/// </summary>
	public class TridiagonalMatrix : Matrix
	{

	  private readonly double[] _a;
	  private readonly double[] _b;
	  private readonly double[] _c;
	  private DoubleMatrix _matrix;

	  /// <param name="a"> An array containing the diagonal values of the matrix, not null </param>
	  /// <param name="b"> An array containing the upper sub-diagonal values of the matrix, not null.
	  ///   Its length must be one less than the length of the diagonal array </param>
	  /// <param name="c"> An array containing the lower sub-diagonal values of the matrix, not null.
	  ///   Its length must be one less than the length of the diagonal array </param>
	  public TridiagonalMatrix(double[] a, double[] b, double[] c)
	  {
		ArgChecker.notNull(a, "a");
		ArgChecker.notNull(b, "b");
		ArgChecker.notNull(c, "c");
		int n = a.Length;
		ArgChecker.isTrue(b.Length == n - 1, "Length of subdiagonal b is incorrect");
		ArgChecker.isTrue(c.Length == n - 1, "Length of subdiagonal c is incorrect");
		_a = a;
		_b = b;
		_c = c;
	  }

	  /// <summary>
	  /// Direct access to Diagonal Data. </summary>
	  /// <returns> An array of the values of the diagonal </returns>
	  public virtual double[] DiagonalData
	  {
		  get
		  {
			return _a;
		  }
	  }

	  /// <returns> An array of the values of the diagonal </returns>
	  public virtual double[] Diagonal
	  {
		  get
		  {
			return Arrays.copyOf(_a, _a.Length);
		  }
	  }

	  /// <summary>
	  /// Direct access to upper sub-Diagonal Data. </summary>
	  /// <returns> An array of the values of the upper sub-diagonal </returns>
	  public virtual double[] UpperSubDiagonalData
	  {
		  get
		  {
			return _b;
		  }
	  }

	  /// <returns> An array of the values of the upper sub-diagonal </returns>
	  public virtual double[] UpperSubDiagonal
	  {
		  get
		  {
			return Arrays.copyOf(_b, _b.Length);
		  }
	  }

	  /// <summary>
	  /// Direct access to lower sub-Diagonal Data. </summary>
	  /// <returns> An array of the values of the lower sub-diagonal </returns>
	  public virtual double[] LowerSubDiagonalData
	  {
		  get
		  {
			return _c;
		  }
	  }

	  /// <returns> An array of the values of the lower sub-diagonal </returns>
	  public virtual double[] LowerSubDiagonal
	  {
		  get
		  {
			return Arrays.copyOf(_c, _c.Length);
		  }
	  }

	  /// <returns> Returns the tridiagonal matrix as a <seealso cref="DoubleMatrix"/> </returns>
	  public virtual DoubleMatrix toDoubleMatrix()
	  {
		if (_matrix == null)
		{
		  calMatrix();
		}
		return _matrix;
	  }

	  private void calMatrix()
	  {
		int n = _a.Length;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] data = new double[n][n];
		double[][] data = RectangularArrays.ReturnRectangularDoubleArray(n, n);
		for (int i = 0; i < n; i++)
		{
		  data[i][i] = _a[i];
		}
		for (int i = 1; i < n; i++)
		{
		  data[i - 1][i] = _b[i - 1];
		}
		for (int i = 1; i < n; i++)
		{
		  data[i][i - 1] = _c[i - 1];
		}
		_matrix = DoubleMatrix.copyOf(data);
	  }

	  public override int GetHashCode()
	  {
		int prime = 31;
		int result = 1;
		result = prime * result + Arrays.GetHashCode(_a);
		result = prime * result + Arrays.GetHashCode(_b);
		result = prime * result + Arrays.GetHashCode(_c);
		return result;
	  }

	  public override bool Equals(object obj)
	  {
		if (this == obj)
		{
		  return true;
		}
		if (obj == null)
		{
		  return false;
		}
		if (this.GetType() != obj.GetType())
		{
		  return false;
		}
		TridiagonalMatrix other = (TridiagonalMatrix) obj;
		if (!Arrays.Equals(_a, other._a))
		{
		  return false;
		}
		if (!Arrays.Equals(_b, other._b))
		{
		  return false;
		}
		if (!Arrays.Equals(_c, other._c))
		{
		  return false;
		}
		return true;
	  }

	  public virtual int dimensions()
	  {
		return 2;
	  }

	  public virtual int size()
	  {
		return _a.Length;
	  }

	  /// <summary>
	  /// Gets the entry for the indices.
	  /// </summary>
	  /// <param name="index">  the indices </param>
	  /// <returns> the entry </returns>
	  public virtual double getEntry(params int[] index)
	  {
		ArgChecker.notNull(index, "indices");
		int n = _a.Length;
		int i = index[0];
		int j = index[1];
		ArgChecker.isTrue(i >= 0 && i < n, "x index {} out of range. Matrix has {} rows", index[0], n);
		ArgChecker.isTrue(j >= 0 && j < n, "y index {} out of range. Matrix has {} columns", index[1], n);
		if (i == j)
		{
		  return _a[i];
		}
		else if ((i - 1) == j)
		{
		  return _c[i - 1];
		}
		else if ((i + 1) == j)
		{
		  return _b[i];
		}

		return 0.0;
	  }

	}

}