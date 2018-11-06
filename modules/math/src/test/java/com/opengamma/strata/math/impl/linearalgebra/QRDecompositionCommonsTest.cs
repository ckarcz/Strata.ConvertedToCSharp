/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.linearalgebra
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertTrue;

	using Test = org.testng.annotations.Test;

	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using CommonsMatrixAlgebra = com.opengamma.strata.math.impl.matrix.CommonsMatrixAlgebra;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using Decomposition = com.opengamma.strata.math.linearalgebra.Decomposition;
	using DecompositionResult = com.opengamma.strata.math.linearalgebra.DecompositionResult;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class QRDecompositionCommonsTest
	public class QRDecompositionCommonsTest
	{
	  private static readonly MatrixAlgebra ALGEBRA = new CommonsMatrixAlgebra();
	  private static readonly Decomposition<QRDecompositionResult> QR = new QRDecompositionCommons();
	  private static readonly DoubleMatrix A = DoubleMatrix.copyOf(new double[][]
	  {
		  new double[] {1, 2, 3},
		  new double[] {4, 5, 6},
		  new double[] {7, 8, 9}
	  });
	  private const double EPS = 1e-9;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullObjectMatrix()
	  public virtual void testNullObjectMatrix()
	  {
		QR.apply((DoubleMatrix) null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testRecoverOrginal()
	  public virtual void testRecoverOrginal()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.linearalgebra.DecompositionResult result = QR.apply(A);
		DecompositionResult result = QR.apply(A);
		assertTrue(result is QRDecompositionResult);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final QRDecompositionResult qr = (QRDecompositionResult) result;
		QRDecompositionResult qr = (QRDecompositionResult) result;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix q = qr.getQ();
		DoubleMatrix q = qr.Q;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix r = qr.getR();
		DoubleMatrix r = qr.R;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix a = (com.opengamma.strata.collect.array.DoubleMatrix) ALGEBRA.multiply(q, r);
		DoubleMatrix a = (DoubleMatrix) ALGEBRA.multiply(q, r);
		checkEquals(A, a);
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