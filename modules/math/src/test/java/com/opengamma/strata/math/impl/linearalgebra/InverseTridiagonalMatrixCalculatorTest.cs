/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.linearalgebra
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.math.impl.matrix.MatrixAlgebraFactory.OG_ALGEBRA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class InverseTridiagonalMatrixCalculatorTest
	public class InverseTridiagonalMatrixCalculatorTest
	{
	  private static readonly InverseTridiagonalMatrixCalculator CALCULATOR = new InverseTridiagonalMatrixCalculator();
	  private static readonly double[] A = new double[] {1.0, 2.4, -0.4, -0.8, 1.5, 7.8, -5.0, 1.0, 2.4, -0.4, 3.14};
	  private static readonly double[] B = new double[] {1.56, 0.33, 0.42, -0.23, 0.276, 4.76, 1.0, 2.4, -0.4, 0.2355};
	  private static readonly double[] C = new double[] {0.56, 0.63, -0.42, -0.23, 0.76, 1.76, 1.0, 2.4, -0.4, 2.4234};

	  private static readonly TridiagonalMatrix MATRIX = new TridiagonalMatrix(A, B, C);
	  private static readonly DoubleMatrix TRI = MATRIX.toDoubleMatrix();
	  private const double EPS = 1e-15;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullArray()
	  public virtual void testNullArray()
	  {
		CALCULATOR.apply((TridiagonalMatrix) null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testInvertIdentity()
	  public virtual void testInvertIdentity()
	  {
		const int n = 11;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] a = new double[n];
		double[] a = new double[n];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] b = new double[n - 1];
		double[] b = new double[n - 1];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] c = new double[n - 1];
		double[] c = new double[n - 1];
		int i, j;

		for (i = 0; i < n; i++)
		{
		  a[i] = 1.0;
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix res = CALCULATOR.apply(new TridiagonalMatrix(a, b, c));
		DoubleMatrix res = CALCULATOR.apply(new TridiagonalMatrix(a, b, c));
		for (i = 0; i < n; i++)
		{
		  for (j = 0; j < n; j++)
		  {
			assertEquals((i == j ? 1.0 : 0.0), res.get(i, j), EPS);
		  }
		}

	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testInvert()
	  public virtual void testInvert()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix res = CALCULATOR.apply(MATRIX);
		DoubleMatrix res = CALCULATOR.apply(MATRIX);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix idet = (com.opengamma.strata.collect.array.DoubleMatrix) OG_ALGEBRA.multiply(TRI, res);
		DoubleMatrix idet = (DoubleMatrix) OG_ALGEBRA.multiply(TRI, res);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n = idet.rowCount();
		int n = idet.rowCount();
		int i, j;
		for (i = 0; i < n; i++)
		{
		  for (j = 0; j < n; j++)
		  {
			assertEquals((i == j ? 1.0 : 0.0), idet.get(i, j), EPS);
		  }
		}

	  }

	}

}