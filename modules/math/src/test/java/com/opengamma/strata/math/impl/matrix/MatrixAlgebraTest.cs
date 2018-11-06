/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.matrix
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertTrue;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Matrix = com.opengamma.strata.collect.array.Matrix;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Test public class MatrixAlgebraTest
	public class MatrixAlgebraTest
	{
	  private static readonly MatrixAlgebra ALGEBRA = new MyMatrixAlgebra();

	  private static readonly DoubleArray M1 = DoubleArray.of(1, 2);
	  private static readonly DoubleArray M2 = DoubleArray.of(3, 4);
	  private static readonly DoubleMatrix M3 = DoubleMatrix.of(2, 2, 1d, 2d, 3d, 4d);
	  private static readonly DoubleMatrix M4 = DoubleMatrix.of(2, 2, 5d, 6d, 7d, 8d);
	  private static readonly Matrix M5 = new MatrixAnonymousInnerClass();

	  private class MatrixAnonymousInnerClass : Matrix
	  {
		  public MatrixAnonymousInnerClass()
		  {
		  }

		  public override int dimensions()
		  {
			return 1;
		  }

		  public override int size()
		  {
			return 0;
		  }

	  }
	  private const double EPS = 1e-10;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testAddWrongSize()
	  public virtual void testAddWrongSize()
	  {
		ALGEBRA.add(M1, M3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testAddDifferentRowNumber1D()
	  public virtual void testAddDifferentRowNumber1D()
	  {
		ALGEBRA.add(M1, DoubleArray.of(1, 2, 3));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testAddDifferentRowNumber2D()
	  public virtual void testAddDifferentRowNumber2D()
	  {
		ALGEBRA.add(M3, DoubleMatrix.of(3, 2, 1d, 2d, 3d, 4d, 5d, 6d));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testAddDifferentColumnNumber2D()
	  public virtual void testAddDifferentColumnNumber2D()
	  {
		ALGEBRA.add(M3, DoubleMatrix.of(2, 3, 1d, 2d, 3d, 4d, 5d, 6d));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testAddWrongType1()
	  public virtual void testAddWrongType1()
	  {
		ALGEBRA.add(M1, M5);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testAddWrongType2()
	  public virtual void testAddWrongType2()
	  {
		ALGEBRA.add(M3, M5);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = UnsupportedOperationException.class) public void testAddWrongType3()
	  public virtual void testAddWrongType3()
	  {
		ALGEBRA.add(M5, M5);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testAdd()
	  public virtual void testAdd()
	  {
		Matrix m = ALGEBRA.add(M1, M2);
		assertTrue(m is DoubleArray);
		assertMatrixEquals(m, DoubleArray.of(4, 6));
		m = ALGEBRA.add(M3, M4);
		assertTrue(m is DoubleMatrix);
		assertMatrixEquals(m, DoubleMatrix.of(2, 2, 6d, 8d, 10d, 12d));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testSubtractWrongSize()
	  public virtual void testSubtractWrongSize()
	  {
		ALGEBRA.subtract(M1, M3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testSubtractDifferentRowNumber1D()
	  public virtual void testSubtractDifferentRowNumber1D()
	  {
		ALGEBRA.subtract(M1, DoubleArray.of(1, 2, 3));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testSubtractDifferentRowNumber2D()
	  public virtual void testSubtractDifferentRowNumber2D()
	  {
		ALGEBRA.subtract(M3, DoubleMatrix.of(3, 2, 1d, 2d, 3d, 4d, 5d, 6d));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testSubtractDifferentColumnNumber2D()
	  public virtual void testSubtractDifferentColumnNumber2D()
	  {
		ALGEBRA.subtract(M3, DoubleMatrix.of(2, 3, 1d, 2d, 3d, 4d, 5d, 6d));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testSubtractWrongType1()
	  public virtual void testSubtractWrongType1()
	  {
		ALGEBRA.subtract(M1, M5);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testSubtractWrongType2()
	  public virtual void testSubtractWrongType2()
	  {
		ALGEBRA.subtract(M3, M5);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = UnsupportedOperationException.class) public void testSubtractWrongType3()
	  public virtual void testSubtractWrongType3()
	  {
		ALGEBRA.subtract(M5, M5);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testSubtract()
	  public virtual void testSubtract()
	  {
		Matrix m = ALGEBRA.subtract(M1, M2);
		assertTrue(m is DoubleArray);
		assertMatrixEquals(m, DoubleArray.of(-2, -2));
		m = ALGEBRA.subtract(M3, M4);
		assertTrue(m is DoubleMatrix);
		assertMatrixEquals(m, DoubleMatrix.of(2, 2, -4d, -4d, -4d, -4d));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = UnsupportedOperationException.class) public void testScaleWrongType()
	  public virtual void testScaleWrongType()
	  {
		ALGEBRA.scale(M5, 0.5);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testScale()
	  public virtual void testScale()
	  {
		Matrix m = ALGEBRA.scale(M1, 10);
		assertTrue(m is DoubleArray);
		assertMatrixEquals(m, DoubleArray.of(10, 20));
		m = ALGEBRA.scale(m, 0.1);
		assertMatrixEquals(m, M1);
		m = ALGEBRA.scale(M3, 10);
		assertTrue(m is DoubleMatrix);
		assertMatrixEquals(m, DoubleMatrix.of(2, 2, 10d, 20d, 30d, 40d));
		m = ALGEBRA.scale(m, 0.1);
		assertMatrixEquals(m, M3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testDivide1D()
	  public virtual void testDivide1D()
	  {
		ALGEBRA.divide(M1, M3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testDivide2D()
	  public virtual void testDivide2D()
	  {
		ALGEBRA.divide(M3, M1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testKroneckerProduct()
	  public virtual void testKroneckerProduct()
	  {
		Matrix m = ALGEBRA.kroneckerProduct(M3, M4);
		assertTrue(m is DoubleMatrix);
		assertMatrixEquals(m, DoubleMatrix.of(4, 4, 5, 6, 10, 12, 7, 8, 14, 16, 15, 18, 20, 24, 21, 24, 28, 32));

	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private void assertMatrixEquals(final com.opengamma.strata.collect.array.Matrix m1, final com.opengamma.strata.collect.array.Matrix m2)
	  private void assertMatrixEquals(Matrix m1, Matrix m2)
	  {
		if (m1 is DoubleArray)
		{
		  assertTrue(m2 is DoubleArray);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray m3 = (com.opengamma.strata.collect.array.DoubleArray) m1;
		  DoubleArray m3 = (DoubleArray) m1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray m4 = (com.opengamma.strata.collect.array.DoubleArray) m2;
		  DoubleArray m4 = (DoubleArray) m2;
		  assertEquals(m3.size(), m4.size());
		  for (int i = 0; i < m3.size(); i++)
		  {
			assertEquals(m3.get(i), m4.get(i), EPS);
		  }
		  return;
		}
		if (m2 is DoubleMatrix)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix m3 = (com.opengamma.strata.collect.array.DoubleMatrix) m1;
		  DoubleMatrix m3 = (DoubleMatrix) m1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix m4 = (com.opengamma.strata.collect.array.DoubleMatrix) m2;
		  DoubleMatrix m4 = (DoubleMatrix) m2;
		  assertEquals(m3.size(), m4.size());
		  assertEquals(m3.rowCount(), m4.rowCount());
		  assertEquals(m3.columnCount(), m4.columnCount());
		  for (int i = 0; i < m3.rowCount(); i++)
		  {
			for (int j = 0; j < m3.columnCount(); j++)
			{
			  assertEquals(m3.get(i, j), m4.get(i, j), EPS);
			}
		  }
		}
	  }

	  private class MyMatrixAlgebra : MatrixAlgebra
	  {

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public double getCondition(final com.opengamma.strata.collect.array.Matrix m)
		public override double getCondition(Matrix m)
		{
		  return 0;
		}

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public double getDeterminant(final com.opengamma.strata.collect.array.Matrix m)
		public override double getDeterminant(Matrix m)
		{
		  return 0;
		}

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public double getInnerProduct(final com.opengamma.strata.collect.array.Matrix m1, final com.opengamma.strata.collect.array.Matrix m2)
		public override double getInnerProduct(Matrix m1, Matrix m2)
		{
		  return 0;
		}

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public com.opengamma.strata.collect.array.DoubleMatrix getInverse(final com.opengamma.strata.collect.array.Matrix m)
		public override DoubleMatrix getInverse(Matrix m)
		{
		  return null;
		}

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public double getNorm1(final com.opengamma.strata.collect.array.Matrix m)
		public override double getNorm1(Matrix m)
		{
		  return 0;
		}

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public double getNorm2(final com.opengamma.strata.collect.array.Matrix m)
		public override double getNorm2(Matrix m)
		{
		  return 0;
		}

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public double getNormInfinity(final com.opengamma.strata.collect.array.Matrix m)
		public override double getNormInfinity(Matrix m)
		{
		  return 0;
		}

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public com.opengamma.strata.collect.array.DoubleMatrix getOuterProduct(final com.opengamma.strata.collect.array.Matrix m1, final com.opengamma.strata.collect.array.Matrix m2)
		public override DoubleMatrix getOuterProduct(Matrix m1, Matrix m2)
		{
		  return null;
		}

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public com.opengamma.strata.collect.array.DoubleMatrix getPower(final com.opengamma.strata.collect.array.Matrix m, final int p)
		public override DoubleMatrix getPower(Matrix m, int p)
		{
		  return null;
		}

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public double getTrace(final com.opengamma.strata.collect.array.Matrix m)
		public override double getTrace(Matrix m)
		{
		  return 0;
		}

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public com.opengamma.strata.collect.array.DoubleMatrix getTranspose(final com.opengamma.strata.collect.array.Matrix m)
		public override DoubleMatrix getTranspose(Matrix m)
		{
		  return null;
		}

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public com.opengamma.strata.collect.array.Matrix multiply(final com.opengamma.strata.collect.array.Matrix m1, final com.opengamma.strata.collect.array.Matrix m2)
		public override Matrix multiply(Matrix m1, Matrix m2)
		{
		  return null;
		}

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public com.opengamma.strata.collect.array.DoubleMatrix getPower(final com.opengamma.strata.collect.array.Matrix m, final double p)
		public override DoubleMatrix getPower(Matrix m, double p)
		{
		  return null;
		}

	  }
	}

}