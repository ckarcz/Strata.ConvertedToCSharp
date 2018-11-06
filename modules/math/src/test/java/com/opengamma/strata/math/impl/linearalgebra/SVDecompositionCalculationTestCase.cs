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

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using Decomposition = com.opengamma.strata.math.linearalgebra.Decomposition;
	using DecompositionResult = com.opengamma.strata.math.linearalgebra.DecompositionResult;

	/// <summary>
	/// Abstract test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public abstract class SVDecompositionCalculationTestCase
	public abstract class SVDecompositionCalculationTestCase
	{
	  private const double EPS = 1e-10;
	  private static readonly DoubleMatrix A = DoubleMatrix.copyOf(new double[][]
	  {
		  new double[] {1, 2, 3},
		  new double[] {-3.4, -1, 4},
		  new double[] {1, 6, 1}
	  });

	  protected internal abstract Decomposition<SVDecompositionResult> SVD {get;}

	  protected internal abstract MatrixAlgebra Algebra {get;}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullObjectMatrix()
	  public virtual void testNullObjectMatrix()
	  {
		SVD.apply((DoubleMatrix) null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testRecoverOrginal()
	  public virtual void testRecoverOrginal()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.matrix.MatrixAlgebra algebra = getAlgebra();
		MatrixAlgebra algebra = Algebra;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.linearalgebra.DecompositionResult result = getSVD().apply(A);
		DecompositionResult result = SVD.apply(A);
		assertTrue(result is SVDecompositionResult);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final SVDecompositionResult svd_result = (SVDecompositionResult) result;
		SVDecompositionResult svd_result = (SVDecompositionResult) result;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix u = svd_result.getU();
		DoubleMatrix u = svd_result.U;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix w = com.opengamma.strata.collect.array.DoubleMatrix.diagonal(com.opengamma.strata.collect.array.DoubleArray.copyOf(svd_result.getSingularValues()));
		DoubleMatrix w = DoubleMatrix.diagonal(DoubleArray.copyOf(svd_result.SingularValues));
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix vt = svd_result.getVT();
		DoubleMatrix vt = svd_result.VT;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix a = (com.opengamma.strata.collect.array.DoubleMatrix) algebra.multiply(algebra.multiply(u, w), vt);
		DoubleMatrix a = (DoubleMatrix) algebra.multiply(algebra.multiply(u, w), vt);
		checkEquals(A, a);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testInvert()
	  public virtual void testInvert()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.matrix.MatrixAlgebra algebra = getAlgebra();
		MatrixAlgebra algebra = Algebra;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final SVDecompositionResult result = getSVD().apply(A);
		SVDecompositionResult result = SVD.apply(A);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix ut = result.getUT();
		DoubleMatrix ut = result.UT;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix v = result.getV();
		DoubleMatrix v = result.V;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] sv = result.getSingularValues();
		double[] sv = result.SingularValues;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n = sv.length;
		int n = sv.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] svinv = new double[n];
		double[] svinv = new double[n];
		for (int i = 0; i < n; i++)
		{
		  if (sv[i] == 0.0)
		  {
			svinv[i] = 0.0;
		  }
		  else
		  {
			svinv[i] = 1.0 / sv[i];
		  }
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix winv = com.opengamma.strata.collect.array.DoubleMatrix.diagonal(com.opengamma.strata.collect.array.DoubleArray.copyOf(svinv));
		DoubleMatrix winv = DoubleMatrix.diagonal(DoubleArray.copyOf(svinv));
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix ainv = (com.opengamma.strata.collect.array.DoubleMatrix) algebra.multiply(algebra.multiply(v, winv), ut);
		DoubleMatrix ainv = (DoubleMatrix) algebra.multiply(algebra.multiply(v, winv), ut);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix identity = (com.opengamma.strata.collect.array.DoubleMatrix) algebra.multiply(A, ainv);
		DoubleMatrix identity = (DoubleMatrix) algebra.multiply(A, ainv);
		checkIdentity(identity);

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

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private void checkIdentity(final com.opengamma.strata.collect.array.DoubleMatrix x)
	  private void checkIdentity(DoubleMatrix x)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n = x.rowCount();
		int n = x.rowCount();
		assertEquals(x.columnCount(), n);
		for (int i = 0; i < n; i++)
		{
		  for (int j = 0; j < n; j++)
		  {
			if (i == j)
			{
			  assertEquals(1.0, x.get(i, i), EPS);
			}
			else
			{
			  assertEquals(0.0, x.get(i, j), EPS);
			}
		  }
		}
	  }
	}

}