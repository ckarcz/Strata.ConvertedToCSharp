using System;

/*
 * Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.differentiation
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class MaxtrixFieldFirstOrderDifferentiatorTest
	public class MaxtrixFieldFirstOrderDifferentiatorTest
	{
	  private static readonly MatrixFieldFirstOrderDifferentiator DIFF = new MatrixFieldFirstOrderDifferentiator();

	  private static readonly System.Func<DoubleArray, DoubleMatrix> F = (final DoubleArray x) =>
	  {

  double x1 = x.get(0);
  double x2 = x.get(1);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] y = new double[3][2];
  double[][] y = RectangularArrays.ReturnRectangularDoubleArray(3, 2);
  y[0][0] = x1 * x1 + 2 * x2 * x2 - x1 * x2 + x1 * Math.Cos(x2) - x2 * Math.Sin(x1);
  y[1][0] = 2 * x1 * x2 * Math.Cos(x1 * x2) - x1 * Math.Sin(x1) - x2 * Math.Cos(x2);
  y[2][0] = x1 - x2;
  y[0][1] = 7.0;
  y[1][1] = Math.Sin(x1);
  y[2][1] = Math.Cos(x2);
  return DoubleMatrix.copyOf(y);
	  };

	  private static readonly System.Func<DoubleArray, DoubleMatrix[]> G = (final DoubleArray x) =>
	  {

  double x1 = x.get(0);
  double x2 = x.get(1);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] y = new double[3][2];
  double[][] y = RectangularArrays.ReturnRectangularDoubleArray(3, 2);
  y[0][0] = 2 * x1 - x2 + Math.Cos(x2) - x2 * Math.Cos(x1);
  y[1][0] = 2 * x2 * Math.Cos(x1 * x2) - 2 * x1 * x2 * x2 * Math.Sin(x1 * x2) - Math.Sin(x1) - x1 * Math.Cos(x1);
  y[2][0] = 1.0;
  y[0][1] = 0.0;
  y[1][1] = Math.Cos(x1);
  y[2][1] = 0.0;
  DoubleMatrix m1 = DoubleMatrix.copyOf(y);
  y[0][0] = 4 * x2 - x1 - x1 * Math.Sin(x2) - Math.Sin(x1);
  y[1][0] = 2 * x1 * Math.Cos(x1 * x2) - 2 * x1 * x1 * x2 * Math.Sin(x1 * x2) - Math.Cos(x2) + x2 * Math.Sin(x2);
  y[2][0] = -1.0;
  y[0][1] = 0.0;
  y[1][1] = 0.0;
  y[2][1] = -Math.Sin(x2);
  DoubleMatrix m2 = DoubleMatrix.copyOf(y);
  return new DoubleMatrix[] {m1, m2};
	  };

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		System.Func<DoubleArray, DoubleMatrix[]> analDiffFunc = DIFF.differentiate(F);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray x = com.opengamma.strata.collect.array.DoubleArray.of(1.3423, 0.235);
		DoubleArray x = DoubleArray.of(1.3423, 0.235);

		DoubleMatrix[] alRes = analDiffFunc(x);
		DoubleMatrix[] fdRes = G.apply(x);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int p = fdRes.length;
		int p = fdRes.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n = fdRes[0].rowCount();
		int n = fdRes[0].rowCount();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int m = fdRes[0].columnCount();
		int m = fdRes[0].columnCount();
		assertEquals(p, alRes.Length);
		assertEquals(n, alRes[0].rowCount());
		assertEquals(m, alRes[0].columnCount());

		for (int k = 0; k < p; k++)
		{
		  for (int i = 0; i < n; i++)
		  {
			for (int j = 0; j < m; j++)
			{
			  assertEquals(fdRes[k].get(i, j), alRes[k].get(i, j), 1e-8);
			}
		  }
		}
	  }

	}

}