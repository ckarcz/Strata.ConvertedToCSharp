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
//ORIGINAL LINE: @Test public class MatrixAlgebraImplementationTest
	public class MatrixAlgebraImplementationTest
	{

	  private static readonly MatrixAlgebra COMMONS = MatrixAlgebraFactory.COMMONS_ALGEBRA;
	  private static readonly MatrixAlgebra OG = MatrixAlgebraFactory.OG_ALGEBRA;
	  private static readonly DoubleArray M1 = DoubleArray.of(1, 2);
	  private static readonly DoubleArray M2 = DoubleArray.of(3, 4);
	  private static readonly DoubleMatrix M3 = DoubleMatrix.copyOf(new double[][]
	  {
		  new double[] {1, 2},
		  new double[] {2, 1}
	  });
	  private static readonly DoubleMatrix M4 = DoubleMatrix.copyOf(new double[][]
	  {
		  new double[] {5, 6},
		  new double[] {7, 8}
	  });
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
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testCommonsCondition()
	  public virtual void testCommonsCondition()
	  {
		COMMONS.getCondition(M1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = UnsupportedOperationException.class) public void testOGCondition()
	  public virtual void testOGCondition()
	  {
		OG.getCondition(M3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testCommonsDeterminant()
	  public virtual void testCommonsDeterminant()
	  {
		COMMONS.getCondition(M1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = UnsupportedOperationException.class) public void testOGDeterminant()
	  public virtual void testOGDeterminant()
	  {
		OG.getDeterminant(M3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testCommonsInnerProduct()
	  public virtual void testCommonsInnerProduct()
	  {
		COMMONS.getInnerProduct(M1, M3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testOGInnerProduct1()
	  public virtual void testOGInnerProduct1()
	  {
		OG.getInnerProduct(M1, DoubleArray.of(1, 2, 3));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testOGInnerProduct2()
	  public virtual void testOGInnerProduct2()
	  {
		OG.getInnerProduct(M1, M3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testCommonsInverse()
	  public virtual void testCommonsInverse()
	  {
		COMMONS.getInverse(M1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = UnsupportedOperationException.class) public void testOGInverse()
	  public virtual void testOGInverse()
	  {
		OG.getInverse(M1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testCommonsNorm1()
	  public virtual void testCommonsNorm1()
	  {
		COMMONS.getNorm1(M5);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = UnsupportedOperationException.class) public void testOGNorm1()
	  public virtual void testOGNorm1()
	  {
		OG.getNorm1(M1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testCommonsNorm2()
	  public virtual void testCommonsNorm2()
	  {
		COMMONS.getNorm2(M5);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = UnsupportedOperationException.class) public void testOGNorm2_1()
	  public virtual void testOGNorm2_1()
	  {
		OG.getNorm2(M3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testOGNorm2_2()
	  public virtual void testOGNorm2_2()
	  {
		OG.getNorm2(M5);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testCommonsNormInfinity()
	  public virtual void testCommonsNormInfinity()
	  {
		COMMONS.getNormInfinity(M5);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = UnsupportedOperationException.class) public void testOGNormInfinity()
	  public virtual void testOGNormInfinity()
	  {
		OG.getNormInfinity(M5);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testCommonsOuterProduct()
	  public virtual void testCommonsOuterProduct()
	  {
		COMMONS.getOuterProduct(M3, M4);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testOGOuterProduct()
	  public virtual void testOGOuterProduct()
	  {
		OG.getOuterProduct(M3, M4);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testCommonsPower()
	  public virtual void testCommonsPower()
	  {
		COMMONS.getPower(M1, 2);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = UnsupportedOperationException.class) public void testOGPower1()
	  public virtual void testOGPower1()
	  {
		OG.getPower(M2, 2);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = UnsupportedOperationException.class) public void testOGPower2()
	  public virtual void testOGPower2()
	  {
		OG.getPower(M2, 2.3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testCommonsTrace()
	  public virtual void testCommonsTrace()
	  {
		COMMONS.getTrace(M1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testOGTrace1()
	  public virtual void testOGTrace1()
	  {
		OG.getTrace(M1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testOGTrace2()
	  public virtual void testOGTrace2()
	  {
		OG.getTrace(DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1, 2, 3},
			new double[] {4, 5, 6}
		}));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testCommonsTranspose()
	  public virtual void testCommonsTranspose()
	  {
		COMMONS.getTranspose(M1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testOGTranspose()
	  public virtual void testOGTranspose()
	  {
		OG.getTranspose(M1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testCommonsMultiply1()
	  public virtual void testCommonsMultiply1()
	  {
		COMMONS.multiply(M1, M3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testCommonsMultiply2()
	  public virtual void testCommonsMultiply2()
	  {
		COMMONS.multiply(M3, M5);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testCommonsMultiply3()
	  public virtual void testCommonsMultiply3()
	  {
		COMMONS.multiply(M5, M3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testOGMultiply1()
	  public virtual void testOGMultiply1()
	  {
		OG.multiply(M5, M3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testOGMultiply2()
	  public virtual void testOGMultiply2()
	  {
		OG.multiply(DoubleArray.of(1, 2, 3), M3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testOGMultiply3()
	  public virtual void testOGMultiply3()
	  {
		OG.multiply(M3, DoubleArray.of(1, 2, 3));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testOGMultiply4()
	  public virtual void testOGMultiply4()
	  {
		OG.multiply(DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1, 2, 3},
			new double[] {4, 5, 6}
		}), M3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testCondition()
	  public virtual void testCondition()
	  {
		assertEquals(COMMONS.getCondition(M4), 86.9885042281285, EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testDeterminant()
	  public virtual void testDeterminant()
	  {
		assertEquals(COMMONS.getDeterminant(M4), -2.0, EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testNormL1()
	  public virtual void testNormL1()
	  {
		assertEquals(COMMONS.getNorm1(M1), 3, EPS);
		assertEquals(COMMONS.getNorm1(M4), 14, EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testNormL2()
	  public virtual void testNormL2()
	  {
		assertEquals(COMMONS.getNorm2(M1), 2.23606797749979, EPS);
		assertEquals(COMMONS.getNorm2(M4), 13.1900344372658, EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testNormLInf()
	  public virtual void testNormLInf()
	  {
		assertEquals(COMMONS.getNormInfinity(M1), 2, EPS);
		assertEquals(COMMONS.getNormInfinity(M4), 15, EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testTrace()
	  public virtual void testTrace()
	  {
		assertEquals(COMMONS.getTrace(M4), 13, EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testInnerProduct()
	  public virtual void testInnerProduct()
	  {
		assertEquals(COMMONS.getInnerProduct(M1, M2), 11, EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testInverse()
	  public virtual void testInverse()
	  {
		assertMatrixEquals(COMMONS.getInverse(M3), DoubleMatrix.copyOf(new double[][]
		{
			new double[] {-0.3333333333333333, 0.6666666666666666},
			new double[] {0.6666666666666666, -0.3333333333333333}
		}));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testMultiply()
	  public virtual void testMultiply()
	  {
		assertMatrixEquals(COMMONS.multiply(DoubleMatrix.identity(2), M3), M3);
		assertMatrixEquals(COMMONS.multiply(M3, M4), DoubleMatrix.copyOf(new double[][]
		{
			new double[] {19, 22},
			new double[] {17, 20}
		}));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testOuterProduct()
	  public virtual void testOuterProduct()
	  {
		assertMatrixEquals(COMMONS.getOuterProduct(M1, M2), DoubleMatrix.copyOf(new double[][]
		{
			new double[] {3, 4},
			new double[] {6, 8}
		}));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testPower()
	  public virtual void testPower()
	  {
		assertMatrixEquals(COMMONS.getPower(M3, 3), DoubleMatrix.copyOf(new double[][]
		{
			new double[] {13, 14},
			new double[] {14, 13}
		}));
		assertMatrixEquals(COMMONS.getPower(M3, 3), COMMONS.multiply(M3, COMMONS.multiply(M3, M3)));
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
	}

}