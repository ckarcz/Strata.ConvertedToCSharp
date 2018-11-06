using System;

/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NaturalSplineInterpolatorTest
	public class NaturalSplineInterpolatorTest
	{
	  private const double EPS = 1e-14;
	  private const double INF = 1.0 / 0.0;

	  /// 
	  public virtual void recov2ptsTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0 };
		double[] xValues = new double[] {1.0, 2.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {6.0, 1.0 };
		double[] yValues = new double[] {6.0, 1.0};

		const int nIntervalsExp = 1;
		const int orderExp = 4;
		const int dimExp = 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefsMatExp = new double[][] {{0.0, 0.0, -5.0, 6.0 } };
		double[][] coefsMatExp = new double[][]
		{
			new double[] {0.0, 0.0, -5.0, 6.0}
		};

		NaturalSplineInterpolator interpMatrix = new NaturalSplineInterpolator();

		PiecewisePolynomialResult result = interpMatrix.interpolate(xValues, yValues);

		assertEquals(result.Dimensions, dimExp);
		assertEquals(result.NumberOfIntervals, nIntervalsExp);
		assertEquals(result.Dimensions, dimExp);

		for (int i = 0; i < nIntervalsExp; ++i)
		{
		  for (int j = 0; j < orderExp; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = coefsMatExp[i][j] == 0.0 ? 1.0 : Math.abs(coefsMatExp[i][j]);
			double @ref = coefsMatExp[i][j] == 0.0 ? 1.0 : Math.Abs(coefsMatExp[i][j]);
			assertEquals(result.CoefMatrix.get(i, j), coefsMatExp[i][j], @ref * EPS);
		  }
		}

		for (int j = 0; j < nIntervalsExp + 1; ++j)
		{
		  assertEquals(result.Knots.get(j), xValues[j]);
		}
	  }

	  /// 
	  public virtual void recov4ptsTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {6.0, 25.0 / 6.0, 10.0 / 3.0, 4.0 };
		double[] yValues = new double[] {6.0, 25.0 / 6.0, 10.0 / 3.0, 4.0};

		const int nIntervalsExp = 3;
		const int orderExp = 4;
		const int dimExp = 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefsMatExp = new double[][] { {1.0 / 6.0, 0.0, -2.0, 6.0 }, {1.0 / 6.0, 1.0 / 2.0, -3.0 / 2.0, 25.0 / 6.0 }, {-1.0 / 3.0, 1.0, 0.0, 10.0 / 3.0 } };
		double[][] coefsMatExp = new double[][]
		{
			new double[] {1.0 / 6.0, 0.0, -2.0, 6.0},
			new double[] {1.0 / 6.0, 1.0 / 2.0, -3.0 / 2.0, 25.0 / 6.0},
			new double[] {-1.0 / 3.0, 1.0, 0.0, 10.0 / 3.0}
		};

		PiecewisePolynomialInterpolator interpMatrix = new NaturalSplineInterpolator();

		PiecewisePolynomialResult result = interpMatrix.interpolate(xValues, yValues);

		assertEquals(result.Dimensions, dimExp);
		assertEquals(result.NumberOfIntervals, nIntervalsExp);
		assertEquals(result.Dimensions, dimExp);

		for (int i = 0; i < nIntervalsExp; ++i)
		{
		  for (int j = 0; j < orderExp; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = coefsMatExp[i][j] == 0.0 ? 1.0 : Math.abs(coefsMatExp[i][j]);
			double @ref = coefsMatExp[i][j] == 0.0 ? 1.0 : Math.Abs(coefsMatExp[i][j]);
			assertEquals(result.CoefMatrix.get(i, j), coefsMatExp[i][j], @ref * EPS);
		  }
		}

		for (int j = 0; j < nIntervalsExp + 1; ++j)
		{
		  assertEquals(result.Knots.get(j), xValues[j]);
		}
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NullXvaluesTest()
	  public virtual void NullXvaluesTest()
	  {
		double[] xValues = new double[4];
		double[] yValues = new double[] {1.0, 2.0, 3.0, 4.0};

		xValues = null;

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NullYvaluesTest()
	  public virtual void NullYvaluesTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
		double[] yValues = new double[4];

		yValues = null;

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void wrongDatalengthTest()
	  public virtual void wrongDatalengthTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0};
		double[] yValues = new double[] {1.0, 2.0, 3.0, 4.0};

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void shortDataLengthTest()
	  public virtual void shortDataLengthTest()
	  {
		double[] xValues = new double[] {1.0};
		double[] yValues = new double[] {4.0};

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNxValuesTest()
	  public virtual void NaNxValuesTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, Double.NaN, 4.0};
		double[] yValues = new double[] {1.0, 2.0, 3.0, 4.0};

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNyValuesTest()
	  public virtual void NaNyValuesTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
		double[] yValues = new double[] {1.0, 2.0, Double.NaN, 4.0};

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfxValuesTest()
	  public virtual void InfxValuesTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, INF};
		double[] yValues = new double[] {1.0, 2.0, 3.0, 4.0};

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfyValuesTest()
	  public virtual void InfyValuesTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
		double[] yValues = new double[] {1.0, 2.0, 3.0, INF};

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void coincideXvaluesTest()
	  public virtual void coincideXvaluesTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 3.0};
		double[] yValues = new double[] {1.0, 2.0, 3.0, 4.0};

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
	  public virtual void recov2ptsMultiTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0 };
		double[] xValues = new double[] {1.0, 2.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {6.0, 1.0 }, {2.0, 5.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {6.0, 1.0},
			new double[] {2.0, 5.0}
		};

		const int nIntervalsExp = 1;
		const int orderExp = 4;
		const int dimExp = 2;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefsMatExp = new double[][] { {0.0, 0.0, -5.0, 6.0 }, {0.0, 0.0, 3.0, 2.0 } };
		double[][] coefsMatExp = new double[][]
		{
			new double[] {0.0, 0.0, -5.0, 6.0},
			new double[] {0.0, 0.0, 3.0, 2.0}
		};

		NaturalSplineInterpolator interpMatrix = new NaturalSplineInterpolator();

		PiecewisePolynomialResult result = interpMatrix.interpolate(xValues, yValues);

		assertEquals(result.Dimensions, dimExp);
		assertEquals(result.NumberOfIntervals, nIntervalsExp);
		assertEquals(result.Dimensions, dimExp);

		for (int i = 0; i < nIntervalsExp * dimExp; ++i)
		{
		  for (int j = 0; j < orderExp; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = coefsMatExp[i][j] == 0.0 ? 1.0 : Math.abs(coefsMatExp[i][j]);
			double @ref = coefsMatExp[i][j] == 0.0 ? 1.0 : Math.Abs(coefsMatExp[i][j]);
			assertEquals(result.CoefMatrix.get(i, j), coefsMatExp[i][j], @ref * EPS);
		  }
		}

		for (int j = 0; j < nIntervalsExp + 1; ++j)
		{
		  assertEquals(result.Knots.get(j), xValues[j]);
		}
	  }

	  /// 
	  public virtual void recov4ptsMultiTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {6.0, 25.0 / 6.0, 10.0 / 3.0, 4.0 }, {6.0, 1.0, 0.0, 0.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {6.0, 25.0 / 6.0, 10.0 / 3.0, 4.0},
			new double[] {6.0, 1.0, 0.0, 0.0}
		};

		const int nIntervalsExp = 3;
		const int orderExp = 4;
		const int dimExp = 2;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefsMatExp = new double[][] { {1.0 / 6.0, 0.0, -2.0, 6.0 }, {1.0, 0.0, -6.0, 6.0 }, {1.0 / 6.0, 1.0 / 2.0, -3.0 / 2.0, 25.0 / 6.0 }, {-1.0, 3.0, -3.0, 1.0 }, {-1.0 / 3.0, 1.0, 0.0, 10.0 / 3.0 }, {0.0, 0.0, 0.0, 0 } };
		double[][] coefsMatExp = new double[][]
		{
			new double[] {1.0 / 6.0, 0.0, -2.0, 6.0},
			new double[] {1.0, 0.0, -6.0, 6.0},
			new double[] {1.0 / 6.0, 1.0 / 2.0, -3.0 / 2.0, 25.0 / 6.0},
			new double[] {-1.0, 3.0, -3.0, 1.0},
			new double[] {-1.0 / 3.0, 1.0, 0.0, 10.0 / 3.0},
			new double[] {0.0, 0.0, 0.0, 0}
		};

		NaturalSplineInterpolator interpMatrix = new NaturalSplineInterpolator();

		PiecewisePolynomialResult result = interpMatrix.interpolate(xValues, yValues);

		assertEquals(result.Dimensions, dimExp);
		assertEquals(result.NumberOfIntervals, nIntervalsExp);
		assertEquals(result.Dimensions, dimExp);

		for (int i = 0; i < nIntervalsExp * dimExp; ++i)
		{
		  for (int j = 0; j < orderExp; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = coefsMatExp[i][j] == 0.0 ? 1.0 : Math.abs(coefsMatExp[i][j]);
			double @ref = coefsMatExp[i][j] == 0.0 ? 1.0 : Math.Abs(coefsMatExp[i][j]);
			assertEquals(result.CoefMatrix.get(i, j), coefsMatExp[i][j], @ref * EPS);
		  }
		}

		for (int j = 0; j < nIntervalsExp + 1; ++j)
		{
		  assertEquals(result.Knots.get(j), xValues[j]);
		}
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NullXvaluesMultiTest()
	  public virtual void NullXvaluesMultiTest()
	  {
		double[] xValues = new double[4];
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 3.0, 4.0},
			new double[] {1.0, 5.0, 3.0, 4.0}
		};

		xValues = null;

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NullYvaluesMultiTest()
	  public virtual void NullYvaluesMultiTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] yValues = new double[2][4];
		double[][] yValues = RectangularArrays.ReturnRectangularDoubleArray(2, 4);

		yValues = null;

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void wrongDatalengthMultiTest()
	  public virtual void wrongDatalengthMultiTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 3.0, 4.0},
			new double[] {2.0, 2.0, 3.0, 4.0}
		};

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void shortDataLengthMultiTest()
	  public virtual void shortDataLengthMultiTest()
	  {
		double[] xValues = new double[] {1.0};
		double[][] yValues = new double[][]
		{
			new double[] {4.0},
			new double[] {1.0}
		};

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNxValuesMultiTest()
	  public virtual void NaNxValuesMultiTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, Double.NaN, 4.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 3.0, 4.0},
			new double[] {2.0, 2.0, 3.0, 4.0}
		};

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNyValuesMultiTest()
	  public virtual void NaNyValuesMultiTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 3.0, 4.0},
			new double[] {1.0, 2.0, Double.NaN, 4.0}
		};

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfxValuesMultiTest()
	  public virtual void InfxValuesMultiTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, INF};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 3.0, 4.0},
			new double[] {2.0, 2.0, 3.0, 4.0}
		};

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfyValuesMultiTest()
	  public virtual void InfyValuesMultiTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 3.0, 4.0},
			new double[] {1.0, 2.0, 3.0, INF}
		};

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void coincideXvaluesMultiTest()
	  public virtual void coincideXvaluesMultiTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 3.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 3.0, 4.0},
			new double[] {2.0, 2.0, 3.0, 4.0}
		};

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// <summary>
	  /// Derive value of the underlying cubic spline function at the value of xKey
	  /// </summary>
	  public virtual void InterpolantsTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {6.0, 25.0 / 6.0, 10.0 / 3.0, 4.0 }, {6.0, 1.0, 0.0, 0.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {6.0, 25.0 / 6.0, 10.0 / 3.0, 4.0},
			new double[] {6.0, 1.0, 0.0, 0.0}
		};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] xKey = new double[][] { {-1.0, 0.5, 1.5 }, {2.5, 3.5, 4.5 } };
		double[][] xKey = new double[][]
		{
			new double[] {-1.0, 0.5, 1.5},
			new double[] {2.5, 3.5, 4.5}
		};

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][][] resultValuesExpected = new double[][][] { { {26.0 / 3.0, 57.0 / 16.0 }, {10.0, 1.0 / 8.0 } }, { {335.0 / 48.0, 85.0 / 24.0 }, {71.0 / 8.0, 0.0 } }, { {241.0 / 48.0, 107.0 / 24.0 }, {25.0 / 8.0, 0.0 } } };
		double[][][] resultValuesExpected = new double[][][]
		{
			new double[][]
			{
				new double[] {26.0 / 3.0, 57.0 / 16.0},
				new double[] {10.0, 1.0 / 8.0}
			},
			new double[][]
			{
				new double[] {335.0 / 48.0, 85.0 / 24.0},
				new double[] {71.0 / 8.0, 0.0}
			},
			new double[][]
			{
				new double[] {241.0 / 48.0, 107.0 / 24.0},
				new double[] {25.0 / 8.0, 0.0}
			}
		};

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int yDim = yValues.length;
		int yDim = yValues.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int keyLength = xKey[0].length;
		int keyLength = xKey[0].Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int keyDim = xKey.length;
		int keyDim = xKey.Length;

		PiecewisePolynomialInterpolator interp = new NaturalSplineInterpolator();

		double value = interp.interpolate(xValues, yValues[0], xKey[0][0]);
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = resultValuesExpected[0][0][0] == 0.0 ? 1.0 : Math.abs(resultValuesExpected[0][0][0]);
		  double @ref = resultValuesExpected[0][0][0] == 0.0 ? 1.0 : Math.Abs(resultValuesExpected[0][0][0]);
		  assertEquals(value, resultValuesExpected[0][0][0], @ref * EPS);
		}

		DoubleArray valuesVec1 = interp.interpolate(xValues, yValues, xKey[0][0]);
		for (int i = 0; i < yDim; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = resultValuesExpected[0][i][0] == 0.0 ? 1.0 : Math.abs(resultValuesExpected[0][i][0]);
		  double @ref = resultValuesExpected[0][i][0] == 0.0 ? 1.0 : Math.Abs(resultValuesExpected[0][i][0]);
		  assertEquals(valuesVec1.get(i), resultValuesExpected[0][i][0], @ref * EPS);
		}

		DoubleArray valuesVec2 = interp.interpolate(xValues, yValues[0], xKey[0]);
		for (int k = 0; k < keyLength; ++k)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = resultValuesExpected[k][0][0] == 0.0 ? 1.0 : Math.abs(resultValuesExpected[k][0][0]);
		  double @ref = resultValuesExpected[k][0][0] == 0.0 ? 1.0 : Math.Abs(resultValuesExpected[k][0][0]);
		  assertEquals(valuesVec2.get(k), resultValuesExpected[k][0][0], @ref * EPS);
		}

		DoubleMatrix valuesMat1 = interp.interpolate(xValues, yValues[0], xKey);
		for (int j = 0; j < keyDim; ++j)
		{
		  for (int k = 0; k < keyLength; ++k)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = resultValuesExpected[k][0][j] == 0.0 ? 1.0 : Math.abs(resultValuesExpected[k][0][j]);
			double @ref = resultValuesExpected[k][0][j] == 0.0 ? 1.0 : Math.Abs(resultValuesExpected[k][0][j]);
			assertEquals(valuesMat1.get(j, k), resultValuesExpected[k][0][j], @ref * EPS);
		  }
		}

		DoubleMatrix valuesMat2 = interp.interpolate(xValues, yValues, xKey[0]);
		for (int i = 0; i < yDim; ++i)
		{
		  for (int k = 0; k < keyLength; ++k)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = resultValuesExpected[k][i][0] == 0.0 ? 1.0 : Math.abs(resultValuesExpected[k][i][0]);
			double @ref = resultValuesExpected[k][i][0] == 0.0 ? 1.0 : Math.Abs(resultValuesExpected[k][i][0]);
			assertEquals(valuesMat2.get(i, k), resultValuesExpected[k][i][0], @ref * EPS);
		  }
		}

		DoubleMatrix[] valuesMat3 = interp.interpolate(xValues, yValues, xKey);
		for (int i = 0; i < yDim; ++i)
		{
		  for (int j = 0; j < keyDim; ++j)
		  {
			for (int k = 0; k < keyLength; ++k)
			{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = resultValuesExpected[k][i][j] == 0.0 ? 1.0 : Math.abs(resultValuesExpected[k][i][j]);
			  double @ref = resultValuesExpected[k][i][j] == 0.0 ? 1.0 : Math.Abs(resultValuesExpected[k][i][j]);
			  assertEquals(valuesMat3[k].get(i, j), resultValuesExpected[k][i][j], @ref * EPS);
			}
		  }
		}
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfiniteOutputTest()
	  public virtual void InfiniteOutputTest()
	  {
		double[] xValues = new double[] {1.e-308, 2.e-308};
		double[] yValues = new double[] {1.0, 1.e308};

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfiniteOutputMultiTest()
	  public virtual void InfiniteOutputMultiTest()
	  {
		double[] xValues = new double[] {1.e-308, 2.e-308};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 1.e308},
			new double[] {2.0, 1.0}
		};

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NanOutputTest()
	  public virtual void NanOutputTest()
	  {
		double[] xValues = new double[] {1.0, 2.e-308, 3.e-308, 4.0};
		double[] yValues = new double[] {1.0, 2.0, 1.e308, 3.0};

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NanOutputMultiTest()
	  public virtual void NanOutputMultiTest()
	  {
		double[] xValues = new double[] {1.0, 2.e-308, 3.e-308, 4.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 3.0, 4.0},
			new double[] {2.0, 2.0, 3.0, 4.0}
		};

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void LargeInterpolantsTest()
	  public virtual void LargeInterpolantsTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {2.0, 10.0, 2.0, 5.0 }, {1.0, 2.0, 10.0, 11.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {2.0, 10.0, 2.0, 5.0},
			new double[] {1.0, 2.0, 10.0, 11.0}
		};

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues[0], 1.e308);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NullKeyTest()
	  public virtual void NullKeyTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0};
		double[] yValues = new double[] {1.0, 3.0, 4.0};
		double[] xKey = new double[3];

		xKey = null;

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues, xKey);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NullKeyMultiTest()
	  public virtual void NullKeyMultiTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 3.0, 4.0},
			new double[] {2.0, 3.0, 1.0}
		};
		double[] xKey = new double[3];

		xKey = null;

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues, xKey);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NullKeyMatrixTest()
	  public virtual void NullKeyMatrixTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0};
		double[] yValues = new double[] {1.0, 3.0, 4.0};
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] xKey = new double[3][3];
		double[][] xKey = RectangularArrays.ReturnRectangularDoubleArray(3, 3);

		xKey = null;

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues, xKey);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NullKeyMatrixMultiTest()
	  public virtual void NullKeyMatrixMultiTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 3.0, 4.0},
			new double[] {2.0, 3.0, 1.0}
		};
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] xKey = new double[3][4];
		double[][] xKey = RectangularArrays.ReturnRectangularDoubleArray(3, 4);

		xKey = null;

		NaturalSplineInterpolator interp = new NaturalSplineInterpolator();

		interp.interpolate(xValues, yValues, xKey);

	  }
	}

}