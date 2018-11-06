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
//	import static org.testng.AssertJUnit.assertFalse;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertTrue;

	using Test = org.testng.annotations.Test;

	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TridiagonalMatrixTest
	public class TridiagonalMatrixTest
	{
	  private static readonly double[] A = new double[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
	  private static readonly double[] B = new double[] {1, 2, 3, 4, 5, 6, 7, 8, 9};
	  private static readonly double[] C = new double[] {2, 3, 4, 5, 6, 7, 8, 9, 10};
	  private static readonly TridiagonalMatrix M = new TridiagonalMatrix(A, B, C);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullA()
	  public virtual void testNullA()
	  {
		new TridiagonalMatrix(null, B, C);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullB()
	  public virtual void testNullB()
	  {
		new TridiagonalMatrix(A, null, C);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullC()
	  public virtual void testNullC()
	  {
		new TridiagonalMatrix(A, B, null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testWrongB()
	  public virtual void testWrongB()
	  {
		new TridiagonalMatrix(A, new double[] {1, 2, 3, 4, 5, 6, 7, 8}, C);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testWrongC()
	  public virtual void testWrongC()
	  {
		new TridiagonalMatrix(A, B, new double[] {1, 2, 3, 4, 5, 6, 7});
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testGetters()
	  public virtual void testGetters()
	  {
		assertTrue(Arrays.Equals(A, M.DiagonalData));
		assertTrue(Arrays.Equals(B, M.UpperSubDiagonalData));
		assertTrue(Arrays.Equals(C, M.LowerSubDiagonalData));
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n = A.length;
		int n = A.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix matrix = M.toDoubleMatrix();
		DoubleMatrix matrix = M.toDoubleMatrix();
		for (int i = 0; i < n; i++)
		{
		  for (int j = 0; j < n; j++)
		  {
			if (i == j)
			{
			  assertEquals(matrix.get(i, j), A[i], 0);
			}
			else if (j == i + 1)
			{
			  assertEquals(matrix.get(i, j), B[j - 1], 0);
			}
			else if (j == i - 1)
			{
			  assertEquals(matrix.get(i, j), C[j], 0);
			}
			else
			{
			  assertEquals(matrix.get(i, j), 0, 0);
			}
		  }
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testHashCodeAndEquals()
	  public virtual void testHashCodeAndEquals()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] a = java.util.Arrays.copyOf(A, A.length);
		double[] a = Arrays.copyOf(A, A.Length);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] b = java.util.Arrays.copyOf(B, B.length);
		double[] b = Arrays.copyOf(B, B.Length);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] c = java.util.Arrays.copyOf(C, C.length);
		double[] c = Arrays.copyOf(C, C.Length);
		TridiagonalMatrix other = new TridiagonalMatrix(a, b, c);
		assertEquals(other, M);
		assertEquals(other.GetHashCode(), M.GetHashCode());
		a[1] = 1000;
		other = new TridiagonalMatrix(a, B, C);
		assertFalse(other.Equals(M));
		b[1] = 1000;
		other = new TridiagonalMatrix(A, b, C);
		assertFalse(other.Equals(M));
		c[1] = 1000;
		other = new TridiagonalMatrix(A, B, c);
		assertFalse(other.Equals(M));
	  }
	}

}