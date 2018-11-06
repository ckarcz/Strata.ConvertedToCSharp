/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.linearalgebra
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertTrue;

	using Array2DRowRealMatrix = org.apache.commons.math3.linear.Array2DRowRealMatrix;
	using LUDecomposition = org.apache.commons.math3.linear.LUDecomposition;
	using RealMatrix = org.apache.commons.math3.linear.RealMatrix;
	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Tests the LUDecompositionCommonsResult class with well conditioned data and 
	/// poorly conditioned data.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LUDecompositionCommonsResultTest
	public class LUDecompositionCommonsResultTest
	{
		private bool InstanceFieldsInitialized = false;

		public LUDecompositionCommonsResultTest()
		{
			if (!InstanceFieldsInitialized)
			{
				InitializeInstanceFields();
				InstanceFieldsInitialized = true;
			}
		}

		private void InitializeInstanceFields()
		{
			decomp = new LUDecomposition(condok);
			result = new LUDecompositionCommonsResult(decomp);
		}


	  internal static double[][] rawAok = new double[][]
	  {
		  new double[] {100.0000000000000000, 9.0000000000000000, 10.0000000000000000, 1.0000000000000000},
		  new double[] {9.0000000000000000, 50.0000000000000000, 19.0000000000000000, 15.0000000000000000},
		  new double[] {10.0000000000000000, 11.0000000000000000, 29.0000000000000000, 21.0000000000000000},
		  new double[] {8.0000000000000000, 10.0000000000000000, 20.0000000000000000, 28.0000000000000000}
	  };
	  internal static double[][] rawAsingular = new double[][]
	  {
		  new double[] {1000000.0000000000000000, 2.0000000000000000, 3.0000000000000000},
		  new double[] {1000000.0000000000000000, 2.0000000000000000, 3.0000000000000000},
		  new double[] {4.0000000000000000, 5.0000000000000000, 6.0000000000000000}
	  };
	  internal static double[] rawRHSvect = new double[] {1, 2, 3, 4};
	  internal static double[][] rawRHSmat = new double[][]
	  {
		  new double[] {1, 2},
		  new double[] {3, 4},
		  new double[] {5, 6},
		  new double[] {7, 8}
	  };

	  internal RealMatrix condok = new Array2DRowRealMatrix(rawAok);
	  internal LUDecomposition decomp;
	  internal LUDecompositionCommonsResult result;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void checkThrowOnNull()
	  public virtual void checkThrowOnNull()
	  {
		new LUDecompositionCommonsResult(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void checkThrowOnSingular()
	  public virtual void checkThrowOnSingular()
	  {
		new LUDecompositionCommonsResult(new LUDecomposition(new Array2DRowRealMatrix(rawAsingular)));
	  }

	  public virtual void testGetL()
	  {
		double[][] expectedRaw = new double[][]
		{
			new double[] {1.0000000000000000, 0.0000000000000000, 0.0000000000000000, 0.0000000000000000},
			new double[] {0.0900000000000000, 1.0000000000000000, 0.0000000000000000, 0.0000000000000000},
			new double[] {0.1000000000000000, 0.2053262858304534, 1.0000000000000000, 0.0000000000000000},
			new double[] {0.0800000000000000, 0.1886562309412482, 0.6500406024227509, 1.0000000000000000}
		};
		assertTrue(FuzzyEquals.ArrayFuzzyEquals(result.L.toArray(), expectedRaw));
	  }

	  public virtual void testGetU()
	  {
		double[][] expectedRaw = new double[][]
		{
			new double[] {100.0000000000000000, 9.0000000000000000, 10.0000000000000000, 1.0000000000000000},
			new double[] {0.0000000000000000, 49.1899999999999977, 18.1000000000000014, 14.9100000000000001},
			new double[] {0.0000000000000000, 0.0000000000000000, 24.2835942264687930, 17.8385850782679398},
			new double[] {0.0000000000000000, 0.0000000000000000, 0.0000000000000000, 13.5113310060192049}
		};
		assertTrue(FuzzyEquals.ArrayFuzzyEquals(result.U.toArray(), expectedRaw));
	  }

	  public virtual void testGetDeterminant()
	  {
		assertTrue(FuzzyEquals.SingleValueFuzzyEquals(result.Determinant, 1613942.00000000));
	  }

	  public virtual void testGetP()
	  {
		double[][] expectedRaw = new double[][]
		{
			new double[] {1.0000000000000000, 0.0000000000000000, 0.0000000000000000, 0.0000000000000000},
			new double[] {0.0000000000000000, 1.0000000000000000, 0.0000000000000000, 0.0000000000000000},
			new double[] {0.0000000000000000, 0.0000000000000000, 1.0000000000000000, 0.0000000000000000},
			new double[] {0.0000000000000000, 0.0000000000000000, 0.0000000000000000, 1.0000000000000000}
		};
		assertTrue(FuzzyEquals.ArrayFuzzyEquals(result.P.toArray(), expectedRaw));
	  }

	  public virtual void testGetPivot()
	  {
		int[] expectedRaw = new int[] {0, 1, 2, 3};
		assertTrue(Arrays.Equals(result.Pivot, expectedRaw));
	  }

	  public virtual void testSolveForVector()
	  {
		double[] expectedRaw = new double[] {0.0090821107573878, -0.0038563963265099, -0.0016307897061976, 0.1428043882617839};
		assertTrue(FuzzyEquals.ArrayFuzzyEquals(result.solve(rawRHSvect), expectedRaw));

		assertTrue(FuzzyEquals.ArrayFuzzyEquals(result.solve(DoubleArray.copyOf(rawRHSvect)).toArray(), expectedRaw));
	  }

	  public virtual void testSolveForMatrix()
	  {
		double[][] expectedRaw = new double[][]
		{
			new double[] {0.0103938059732010, 0.0181642215147756},
			new double[] {-0.0147149030138629, -0.0077127926530197},
			new double[] {-0.0171480759531631, -0.0032615794123952},
			new double[] {0.2645342893362958, 0.2856087765235678}
		};

		assertTrue(FuzzyEquals.ArrayFuzzyEquals(result.solve(DoubleMatrix.copyOf(rawRHSmat)).toArray(), expectedRaw));
	  }

	}

}