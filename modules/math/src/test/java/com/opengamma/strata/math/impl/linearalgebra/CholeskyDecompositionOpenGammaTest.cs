/*
 * Copyright (C) 2011 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.linearalgebra
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;
	using ArrayAsserts = org.testng.@internal.junit.ArrayAsserts;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using OGMatrixAlgebra = com.opengamma.strata.math.impl.matrix.OGMatrixAlgebra;
	using Decomposition = com.opengamma.strata.math.linearalgebra.Decomposition;

	/// <summary>
	/// Tests the Cholesky decomposition OpenGamma implementation.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CholeskyDecompositionOpenGammaTest
	public class CholeskyDecompositionOpenGammaTest
	{

	  private static readonly MatrixAlgebra ALGEBRA = new OGMatrixAlgebra();
	  private static readonly CholeskyDecompositionOpenGamma CDOG = new CholeskyDecompositionOpenGamma();
	  private static readonly Decomposition<CholeskyDecompositionResult> CDC = new CholeskyDecompositionCommons();
	  private static readonly DoubleMatrix A3 = DoubleMatrix.copyOf(new double[][]
	  {
		  new double[] {10.0, 2.0, -1.0},
		  new double[] {2.0, 5.0, -2.0},
		  new double[] {-1.0, -2.0, 15.0}
	  });
	  private static readonly DoubleMatrix A5 = DoubleMatrix.copyOf(new double[][]
	  {
		  new double[] {10.0, 2.0, -1.0, 1.0, 1.0},
		  new double[] {2.0, 5.0, -2.0, 0.5, 0.5},
		  new double[] {-1.0, -2.0, 15.0, 1.0, 0.5},
		  new double[] {1.0, 0.5, 1.0, 10.0, -1.0},
		  new double[] {1.0, 0.5, 0.5, -1.0, 25.0}
	  });
	  private const double EPS = 1e-9;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullObjectMatrix()
	  public virtual void testNullObjectMatrix()
	  {
		CDOG.apply((DoubleMatrix) null);
	  }

	  /// <summary>
	  /// Tests A = L L^T.
	  /// </summary>
	  public virtual void recoverOrginal()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final CholeskyDecompositionResult result = CDOG.apply(A3);
		CholeskyDecompositionResult result = CDOG.apply(A3);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix a = (com.opengamma.strata.collect.array.DoubleMatrix) ALGEBRA.multiply(result.getL(), result.getLT());
		DoubleMatrix a = (DoubleMatrix) ALGEBRA.multiply(result.L, result.LT);
		checkEquals(A3, a);
	  }

	  /// <summary>
	  /// Tests solve Ax = b from A and b.
	  /// </summary>
	  public virtual void solveVector()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final CholeskyDecompositionResult result = CDOG.apply(A5);
		CholeskyDecompositionResult result = CDOG.apply(A5);
		double[] b = new double[] {1.0, 2.0, 3.0, 4.0, -1.0};
		double[] x = result.solve(b);
		DoubleArray ax = (DoubleArray) ALGEBRA.multiply(A5, DoubleArray.copyOf(x));
		ArrayAsserts.assertArrayEquals("Cholesky decomposition OpenGamma - solve", b, ax.toArray(), 1.0E-10);
	  }

	  /// <summary>
	  /// Tests solve AX = B from A and B.
	  /// </summary>
	  public virtual void solveMatrix()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final CholeskyDecompositionResult result = CDOG.apply(A5);
		CholeskyDecompositionResult result = CDOG.apply(A5);
		double[][] b = new double[][]
		{
			new double[] {1.0, 2.0},
			new double[] {2.0, 3.0},
			new double[] {3.0, 4.0},
			new double[] {4.0, -2.0},
			new double[] {-1.0, -1.0}
		};
		DoubleMatrix x = result.solve(DoubleMatrix.copyOf(b));
		DoubleMatrix ax = (DoubleMatrix) ALGEBRA.multiply(A5, x);
		ArrayAsserts.assertArrayEquals("Cholesky decomposition OpenGamma - solve", b[0], ax.rowArray(0), 1.0E-10);
		ArrayAsserts.assertArrayEquals("Cholesky decomposition OpenGamma - solve", b[1], ax.rowArray(1), 1.0E-10);
	  }

	  /// <summary>
	  /// Compare results with Common decomposition
	  /// </summary>
	  public virtual void compareCommon()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final CholeskyDecompositionResult resultOG = CDOG.apply(A3);
		CholeskyDecompositionResult resultOG = CDOG.apply(A3);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final CholeskyDecompositionResult resultC = CDC.apply(A3);
		CholeskyDecompositionResult resultC = CDC.apply(A3);
		checkEquals(resultC.L, resultOG.L);
		checkEquals(ALGEBRA.getTranspose(resultC.L), resultOG.LT);
		assertEquals("Determinant", resultC.Determinant, resultOG.Determinant, 1.0E-10);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private void checkEquals(final com.opengamma.strata.collect.array.DoubleMatrix x, final com.opengamma.strata.collect.array.DoubleMatrix y)
	  private void checkEquals(DoubleMatrix x, DoubleMatrix y)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n = x.rowCount();
		int n = x.rowCount();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int m = x.columnCount();
		int m = x.columnCount();
		assertEquals(n, y.rowCount());
		assertEquals(m, y.columnCount());
		for (int i = 0; i < n; i++)
		{
		  for (int j = 0; j < m; j++)
		  {
			assertEquals(x.get(i, j), y.get(i, j), EPS);
		  }
		}
	  }

	}

}