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
	using QRDecomposition = org.apache.commons.math3.linear.QRDecomposition;
	using RealMatrix = org.apache.commons.math3.linear.RealMatrix;
	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Tests the QR decomposition result
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class QRDecompositionCommonsResultTest
	public class QRDecompositionCommonsResultTest
	{
		private bool InstanceFieldsInitialized = false;

		public QRDecompositionCommonsResultTest()
		{
			if (!InstanceFieldsInitialized)
			{
				InitializeInstanceFields();
				InstanceFieldsInitialized = true;
			}
		}

		private void InitializeInstanceFields()
		{
			qr = new QRDecomposition(condok);
			result = new QRDecompositionCommonsResult(qr);
		}

	  internal static double[][] rawAok = new double[][]
	  {
		  new double[] {100.0000000000000000, 9.0000000000000000, 10.0000000000000000, 1.0000000000000000},
		  new double[] {9.0000000000000000, 50.0000000000000000, 19.0000000000000000, 15.0000000000000000},
		  new double[] {10.0000000000000000, 11.0000000000000000, 29.0000000000000000, 21.0000000000000000},
		  new double[] {8.0000000000000000, 10.0000000000000000, 20.0000000000000000, 28.0000000000000000}
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
	  internal QRDecomposition qr;
	  internal QRDecompositionCommonsResult result;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testThrowOnNull()
	  public virtual void testThrowOnNull()
	  {
		new QRDecompositionCommonsResult(null);
	  }

	  public virtual void testGetQ()
	  {
		double[][] expectedRaw = new double[][]
		{
			new double[] {-0.9879705944808324, 0.1189683945357854, 0.0984381737009301, 0.0083994941034602},
			new double[] {-0.0889173535032749, -0.9595057553635415, 0.2632608647546777, 0.0462182513606091},
			new double[] {-0.0987970594480833, -0.1873133740004731, -0.8116464267094188, 0.5444106161481449},
			new double[] {-0.0790376475584666, -0.1735192394127411, -0.5120876107238650, -0.8375024792591188}
		};
		assertTrue(FuzzyEquals.ArrayFuzzyEquals(result.Q.toArray(), expectedRaw));
	  }

	  public virtual void testGetR()
	  {
		double[][] expectedRaw = new double[][]
		{
			new double[] {-101.2175874045612574, -15.2147471550048152, -16.0150033365342956, -6.6095232770767698},
			new double[] {0.0000000000000000, -50.7002117254876197, -25.9433980408179785, -23.0657374934840220},
			new double[] {0.0000000000000000, 0.0000000000000000, -27.7931604217022681, -27.3356769161449193},
			new double[] {0.0000000000000000, 0.0000000000000000, 0.0000000000000000, -11.3157732156316868}
		};
		assertTrue(FuzzyEquals.ArrayFuzzyEquals(result.R.toArray(), expectedRaw));
	  }

	  public virtual void testGetQT()
	  {
		double[][] expectedRaw = new double[][]
		{
			new double[] {-0.9879705944808324, -0.0889173535032749, -0.0987970594480833, -0.0790376475584666},
			new double[] {0.1189683945357854, -0.9595057553635415, -0.1873133740004731, -0.1735192394127411},
			new double[] {0.0984381737009301, 0.2632608647546777, -0.8116464267094188, -0.5120876107238650},
			new double[] {0.0083994941034602, 0.0462182513606091, 0.5444106161481449, -0.8375024792591188}
		};
		assertTrue(FuzzyEquals.ArrayFuzzyEquals(result.QT.toArray(), expectedRaw));
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