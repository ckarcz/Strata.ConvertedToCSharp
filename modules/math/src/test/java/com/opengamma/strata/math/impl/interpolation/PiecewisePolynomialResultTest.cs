using System;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertTrue;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Test for <seealso cref="PiecewisePolynomialResult"/> and subclasses
	/// </summary>
	public class PiecewisePolynomialResultTest
	{

	  private const object ANOTHER_TYPE = "";

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void hashCodeEqualsTest()
	  public virtual void hashCodeEqualsTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] knots1 = new double[] {1.0, 2.0, 3.0, 4.0 };
		double[] knots1 = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] knots2 = new double[] {1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0 };
		double[] knots2 = new double[] {1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] matrix1 = new double[][] { {3.0, 3.0, 3.0 }, {1.0, 1.0, 1.0 }, {2.0, 2.0, 2.0 }, {3.0, 3.0, 3.0 }, {1.0, 1.0, 1.0 }, {2.0, 2.0, 2.0 } };
		double[][] matrix1 = new double[][]
		{
			new double[] {3.0, 3.0, 3.0},
			new double[] {1.0, 1.0, 1.0},
			new double[] {2.0, 2.0, 2.0},
			new double[] {3.0, 3.0, 3.0},
			new double[] {1.0, 1.0, 1.0},
			new double[] {2.0, 2.0, 2.0}
		};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] matrix2 = new double[][] { {3.0, 3.0, 3.0 }, {1.0, 1.0, 1.0 }, {2.0, 2.0, 2.0 } };
		double[][] matrix2 = new double[][]
		{
			new double[] {3.0, 3.0, 3.0},
			new double[] {1.0, 1.0, 1.0},
			new double[] {2.0, 2.0, 2.0}
		};
		const int order = 3;
		const int dim1 = 2;
		const int dim2 = 1;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PiecewisePolynomialResult res1 = new PiecewisePolynomialResult(com.opengamma.strata.collect.array.DoubleArray.copyOf(knots1), com.opengamma.strata.collect.array.DoubleMatrix.copyOf(matrix1), order, dim1);
		PiecewisePolynomialResult res1 = new PiecewisePolynomialResult(DoubleArray.copyOf(knots1), DoubleMatrix.copyOf(matrix1), order, dim1);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PiecewisePolynomialResult res2 = new PiecewisePolynomialResult(com.opengamma.strata.collect.array.DoubleArray.copyOf(knots1), com.opengamma.strata.collect.array.DoubleMatrix.copyOf(matrix1), order, dim1);
		PiecewisePolynomialResult res2 = new PiecewisePolynomialResult(DoubleArray.copyOf(knots1), DoubleMatrix.copyOf(matrix1), order, dim1);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PiecewisePolynomialResult res3 = new PiecewisePolynomialResult(com.opengamma.strata.collect.array.DoubleArray.copyOf(knots2), com.opengamma.strata.collect.array.DoubleMatrix.copyOf(matrix2), order, dim2);
		PiecewisePolynomialResult res3 = new PiecewisePolynomialResult(DoubleArray.copyOf(knots2), DoubleMatrix.copyOf(matrix2), order, dim2);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PiecewisePolynomialResult res4 = new PiecewisePolynomialResult(com.opengamma.strata.collect.array.DoubleArray.copyOf(knots1), com.opengamma.strata.collect.array.DoubleMatrix.copyOf(matrix1), 2, dim1);
		PiecewisePolynomialResult res4 = new PiecewisePolynomialResult(DoubleArray.copyOf(knots1), DoubleMatrix.copyOf(matrix1), 2, dim1);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PiecewisePolynomialResult res5 = new PiecewisePolynomialResult(com.opengamma.strata.collect.array.DoubleArray.copyOf(knots1), com.opengamma.strata.collect.array.DoubleMatrix.copyOf(matrix1), order, dim1 - 1);
		PiecewisePolynomialResult res5 = new PiecewisePolynomialResult(DoubleArray.copyOf(knots1), DoubleMatrix.copyOf(matrix1), order, dim1 - 1);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PiecewisePolynomialResult res6 = new PiecewisePolynomialResult(com.opengamma.strata.collect.array.DoubleArray.of(1.0, 2.0, 3.0, 5.0), com.opengamma.strata.collect.array.DoubleMatrix.copyOf(matrix1), order, dim1);
		PiecewisePolynomialResult res6 = new PiecewisePolynomialResult(DoubleArray.of(1.0, 2.0, 3.0, 5.0), DoubleMatrix.copyOf(matrix1), order, dim1);

		assertTrue(res1.Equals(res1));

		assertTrue(res1.Equals(res2));
		assertTrue(res2.Equals(res1));
		assertTrue(res2.GetHashCode() == res1.GetHashCode());

		assertTrue(!(res3.GetHashCode() == res1.GetHashCode()));
		assertTrue(!(res1.Equals(res3)));
		assertTrue(!(res3.Equals(res1)));

		assertTrue(!(res4.GetHashCode() == res1.GetHashCode()));
		assertTrue(!(res1.Equals(res4)));
		assertTrue(!(res4.Equals(res1)));

		assertTrue(!(res5.GetHashCode() == res1.GetHashCode()));
		assertTrue(!(res1.Equals(res5)));
		assertTrue(!(res5.Equals(res1)));

		assertTrue(!(res6.GetHashCode() == res1.GetHashCode()));
		assertTrue(!(res1.Equals(res6)));
		assertTrue(!(res6.Equals(res1)));

		assertTrue(!(res1.Equals(null)));
		assertTrue(!(res1.Equals(ANOTHER_TYPE)));

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix[] sense1 = new com.opengamma.strata.collect.array.DoubleMatrix[] {com.opengamma.strata.collect.array.DoubleMatrix.copyOf(matrix1), com.opengamma.strata.collect.array.DoubleMatrix.copyOf(matrix1)};
		DoubleMatrix[] sense1 = new DoubleMatrix[] {DoubleMatrix.copyOf(matrix1), DoubleMatrix.copyOf(matrix1)};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix[] sense2 = new com.opengamma.strata.collect.array.DoubleMatrix[] {com.opengamma.strata.collect.array.DoubleMatrix.copyOf(matrix1), com.opengamma.strata.collect.array.DoubleMatrix.copyOf(matrix1), com.opengamma.strata.collect.array.DoubleMatrix.copyOf(matrix1)};
		DoubleMatrix[] sense2 = new DoubleMatrix[] {DoubleMatrix.copyOf(matrix1), DoubleMatrix.copyOf(matrix1), DoubleMatrix.copyOf(matrix1)};

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PiecewisePolynomialResultsWithSensitivity resSen1 = new PiecewisePolynomialResultsWithSensitivity(com.opengamma.strata.collect.array.DoubleArray.copyOf(knots1), com.opengamma.strata.collect.array.DoubleMatrix.copyOf(matrix1), order, 1, sense1);
		PiecewisePolynomialResultsWithSensitivity resSen1 = new PiecewisePolynomialResultsWithSensitivity(DoubleArray.copyOf(knots1), DoubleMatrix.copyOf(matrix1), order, 1, sense1);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PiecewisePolynomialResultsWithSensitivity resSen2 = new PiecewisePolynomialResultsWithSensitivity(com.opengamma.strata.collect.array.DoubleArray.copyOf(knots1), com.opengamma.strata.collect.array.DoubleMatrix.copyOf(matrix1), order, 1, sense1);
		PiecewisePolynomialResultsWithSensitivity resSen2 = new PiecewisePolynomialResultsWithSensitivity(DoubleArray.copyOf(knots1), DoubleMatrix.copyOf(matrix1), order, 1, sense1);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PiecewisePolynomialResultsWithSensitivity resSen3 = new PiecewisePolynomialResultsWithSensitivity(com.opengamma.strata.collect.array.DoubleArray.copyOf(knots1), com.opengamma.strata.collect.array.DoubleMatrix.copyOf(matrix1), order, 1, sense2);
		PiecewisePolynomialResultsWithSensitivity resSen3 = new PiecewisePolynomialResultsWithSensitivity(DoubleArray.copyOf(knots1), DoubleMatrix.copyOf(matrix1), order, 1, sense2);
		assertTrue(resSen1.Equals(resSen1));

		assertTrue(!(resSen1.Equals(ANOTHER_TYPE)));

		assertTrue(!(resSen1.Equals(res5)));

		assertTrue(resSen1.Equals(resSen2));
		assertTrue(resSen2.Equals(resSen1));
		assertTrue(resSen1.GetHashCode() == resSen2.GetHashCode());

		assertTrue(!(resSen1.GetHashCode() == resSen3.GetHashCode()));
		assertTrue(!(resSen1.Equals(resSen3)));
		assertTrue(!(resSen3.Equals(resSen1)));

		try
		{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unused") final PiecewisePolynomialResultsWithSensitivity resSen0 = new PiecewisePolynomialResultsWithSensitivity(com.opengamma.strata.collect.array.DoubleArray.copyOf(knots1), com.opengamma.strata.collect.array.DoubleMatrix.copyOf(matrix1), order, 2, sense1);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
		  PiecewisePolynomialResultsWithSensitivity resSen0 = new PiecewisePolynomialResultsWithSensitivity(DoubleArray.copyOf(knots1), DoubleMatrix.copyOf(matrix1), order, 2, sense1);
		  throw new Exception();
		}
		catch (Exception e)
		{
		  assertTrue(e is System.NotSupportedException);
		}
	  }
	}

}