using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.matrix
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using TridiagonalMatrix = com.opengamma.strata.math.impl.linearalgebra.TridiagonalMatrix;
	using NormalDistribution = com.opengamma.strata.math.impl.statistics.distribution.NormalDistribution;
	using ProbabilityDistribution = com.opengamma.strata.math.impl.statistics.distribution.ProbabilityDistribution;
	using AssertMatrix = com.opengamma.strata.math.impl.util.AssertMatrix;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class OGMatrixAlgebraTest
	public class OGMatrixAlgebraTest
	{
	  private static ProbabilityDistribution<double> RANDOM = new NormalDistribution(0, 1);
	  private static readonly MatrixAlgebra ALGEBRA = MatrixAlgebraFactory.getMatrixAlgebra("OG");
	  private static readonly DoubleMatrix A = DoubleMatrix.copyOf(new double[][]
	  {
		  new double[] {1.0, 2.0, 3.0},
		  new double[] {-1.0, 1.0, 0.0},
		  new double[] {-2.0, 1.0, -2.0}
	  });
	  private static readonly DoubleMatrix B = DoubleMatrix.copyOf(new double[][]
	  {
		  new double[] {1, 1},
		  new double[] {2, -2},
		  new double[] {3, 1}
	  });
	  private static readonly DoubleMatrix C = DoubleMatrix.copyOf(new double[][]
	  {
		  new double[] {14, 0},
		  new double[] {1, -3},
		  new double[] {-6, -6}
	  });
	  private static readonly DoubleArray D = DoubleArray.of(1, 1, 1);
	  private static readonly DoubleArray E = DoubleArray.of(-1, 2, 3);
	  private static readonly DoubleArray F = DoubleArray.of(2, -2, 1);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testMatrixSizeMismatch()
	  public virtual void testMatrixSizeMismatch()
	  {
		ALGEBRA.multiply(B, A);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testDotProduct()
	  public virtual void testDotProduct()
	  {
		double res = ALGEBRA.getInnerProduct(E, F);
		assertEquals(-3.0, res, 1e-15);
		res = ALGEBRA.getNorm2(E);
		assertEquals(Math.Sqrt(14.0), res, 1e-15);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testOuterProduct()
	  public virtual void testOuterProduct()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix res = ALGEBRA.getOuterProduct(E, F);
		DoubleMatrix res = ALGEBRA.getOuterProduct(E, F);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int rows = res.rowCount();
		int rows = res.rowCount();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int cols = res.columnCount();
		int cols = res.columnCount();
		int i, j;
		for (i = 0; i < rows; i++)
		{
		  for (j = 0; j < cols; j++)
		  {
			assertEquals(res.get(i, j), E.get(i) * F.get(j), 1e-15);
		  }
		}

	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testMultiply()
	  public virtual void testMultiply()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix c = (com.opengamma.strata.collect.array.DoubleMatrix) ALGEBRA.multiply(A, B);
		DoubleMatrix c = (DoubleMatrix) ALGEBRA.multiply(A, B);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int rows = c.rowCount();
		int rows = c.rowCount();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int cols = c.columnCount();
		int cols = c.columnCount();
		int i, j;
		for (i = 0; i < rows; i++)
		{
		  for (j = 0; j < cols; j++)
		  {
			assertEquals(c.get(i, j), C.get(i, j), 1e-15);
		  }
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray d = (com.opengamma.strata.collect.array.DoubleArray) ALGEBRA.multiply(A, D);
		DoubleArray d = (DoubleArray) ALGEBRA.multiply(A, D);
		assertEquals(6, d.get(0), 1e-15);
		assertEquals(0, d.get(1), 1e-15);
		assertEquals(-3, d.get(2), 1e-15);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testTridiagonalMultiply()
	  public virtual void testTridiagonalMultiply()
	  {
		const int n = 37;
		double[] l = new double[n - 1];
		double[] c = new double[n];
		double[] u = new double[n - 1];
		double[] x = new double[n];

		for (int ii = 0; ii < n; ii++)
		{
		  c[ii] = RANDOM.nextRandom();
		  x[ii] = RANDOM.nextRandom();
		  if (ii < n - 1)
		  {
			l[ii] = RANDOM.nextRandom();
			u[ii] = RANDOM.nextRandom();
		  }
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.linearalgebra.TridiagonalMatrix m = new com.opengamma.strata.math.impl.linearalgebra.TridiagonalMatrix(c, u, l);
		TridiagonalMatrix m = new TridiagonalMatrix(c, u, l);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray xVec = com.opengamma.strata.collect.array.DoubleArray.copyOf(x);
		DoubleArray xVec = DoubleArray.copyOf(x);
		DoubleArray y1 = (DoubleArray) ALGEBRA.multiply(m, xVec);
		DoubleMatrix full = m.toDoubleMatrix();
		DoubleArray y2 = (DoubleArray) ALGEBRA.multiply(full, xVec);

		for (int i = 0; i < n; i++)
		{
		  assertEquals(y1.get(i), y2.get(i), 1e-12);
		}

	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testTranspose()
	  public virtual void testTranspose()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix a = com.opengamma.strata.collect.array.DoubleMatrix.copyOf(new double[][] { {1, 2}, {3, 4}, {5, 6}});
		DoubleMatrix a = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1, 2},
			new double[] {3, 4},
			new double[] {5, 6}
		});
		assertEquals(3, a.rowCount());
		assertEquals(2, a.columnCount());
		DoubleMatrix aT = ALGEBRA.getTranspose(a);
		assertEquals(2, aT.rowCount());
		assertEquals(3, aT.columnCount());

	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void matrixTransposeMultipleMatrixTest()
	  public virtual void matrixTransposeMultipleMatrixTest()
	  {
		DoubleMatrix a = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, 2.0, 3.0},
			new double[] {-3.0, 1.3, 7.0}
		});
		DoubleMatrix aTa = ALGEBRA.matrixTransposeMultiplyMatrix(a);
		DoubleMatrix aTaRef = (DoubleMatrix) ALGEBRA.multiply(ALGEBRA.getTranspose(a), a);
		AssertMatrix.assertEqualsMatrix(aTaRef, aTa, 1e-15);
	  }

	}

}