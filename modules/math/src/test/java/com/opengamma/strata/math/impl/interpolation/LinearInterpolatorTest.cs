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
	using PiecewisePolynomialWithSensitivityFunction1D = com.opengamma.strata.math.impl.function.PiecewisePolynomialWithSensitivityFunction1D;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LinearInterpolatorTest
	public class LinearInterpolatorTest
	{

	  private const double EPS = 1e-14;
	  private const double INF = 1.0 / 0.0;
	  private static readonly LinearInterpolator INTERP = new LinearInterpolator();

	  /// 
	  public virtual void recov2ptsTest()
	  {
		double[] xValues = new double[] {1.0, 2.0};
		double[] yValues = new double[] {6.0, 1.0};

		int nIntervalsExp = 1;
		int orderExp = 2;
		int dimExp = 1;
		double[][] coefsMatExp = new double[][]
		{
			new double[] {-5.0, 6.0}
		};
		PiecewisePolynomialResult result = INTERP.interpolate(xValues, yValues);
		assertEquals(result.Dimensions, dimExp);
		assertEquals(result.NumberOfIntervals, nIntervalsExp);
		assertEquals(result.Dimensions, dimExp);

		for (int i = 0; i < nIntervalsExp; ++i)
		{
		  for (int j = 0; j < orderExp; ++j)
		  {
			double @ref = coefsMatExp[i][j] == 0.0 ? 1.0 : Math.Abs(coefsMatExp[i][j]);
			assertEquals(result.CoefMatrix.get(i, j), coefsMatExp[i][j], @ref * EPS);
		  }
		}
		for (int j = 0; j < nIntervalsExp + 1; ++j)
		{
		  assertEquals(result.Knots.get(j), xValues[j]);
		}

		// sensitivity
		double delta = 1.0e-6;
		double[] keys = new double[] {-1.2, 1.63, 2.3};
		testSensitivity(xValues, yValues, keys, delta);
	  }

	  /// 
	  public virtual void recov4ptsTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 4.0, 7.0};
		double[] yValues = new double[] {6.0, 1.0, 8.0, -2.0};

		int nIntervalsExp = 3;
		int orderExp = 2;
		int dimExp = 1;
		double[][] coefsMatExp = new double[][]
		{
			new double[] {-5.0, 6.0},
			new double[] {7.0 / 2.0, 1.0},
			new double[] {-10.0 / 3.0, 8.0}
		};
		LinearInterpolator interpMatrix = new LinearInterpolator();
		PiecewisePolynomialResult result = interpMatrix.interpolate(xValues, yValues);
		assertEquals(result.Dimensions, dimExp);
		assertEquals(result.NumberOfIntervals, nIntervalsExp);
		assertEquals(result.Dimensions, dimExp);

		for (int i = 0; i < nIntervalsExp; ++i)
		{
		  for (int j = 0; j < orderExp; ++j)
		  {
			double @ref = coefsMatExp[i][j] == 0.0 ? 1.0 : Math.Abs(coefsMatExp[i][j]);
			assertEquals(result.CoefMatrix.get(i, j), coefsMatExp[i][j], @ref * EPS);
		  }
		}
		for (int j = 0; j < nIntervalsExp + 1; ++j)
		{
		  assertEquals(result.Knots.get(j), xValues[j]);
		}

		// sensitivity
		double delta = 1.0e-6;
		double[] keys = new double[] {-1.5, 2.43, 4.0, 7.0, 12.7};
		testSensitivity(xValues, yValues, keys, delta);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NullXvaluesTest()
	  public virtual void NullXvaluesTest()
	  {
		double[] xValues = new double[4];
		double[] yValues = new double[] {1.0, 2.0, 3.0, 4.0};
		xValues = null;
		INTERP.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NullYvaluesTest()
	  public virtual void NullYvaluesTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
		double[] yValues = new double[4];
		yValues = null;
		INTERP.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void wrongDatalengthTest()
	  public virtual void wrongDatalengthTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0};
		double[] yValues = new double[] {1.0, 2.0, 3.0, 4.0};
		INTERP.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void shortDataLengthTest()
	  public virtual void shortDataLengthTest()
	  {
		double[] xValues = new double[] {1.0};
		double[] yValues = new double[] {4.0};
		INTERP.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNxValuesTest()
	  public virtual void NaNxValuesTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, Double.NaN, 4.0};
		double[] yValues = new double[] {1.0, 2.0, 3.0, 4.0};
		INTERP.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNyValuesTest()
	  public virtual void NaNyValuesTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
		double[] yValues = new double[] {1.0, 2.0, Double.NaN, 4.0};
		INTERP.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfxValuesTest()
	  public virtual void InfxValuesTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, INF};
		double[] yValues = new double[] {1.0, 2.0, 3.0, 4.0};
		INTERP.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfyValuesTest()
	  public virtual void InfyValuesTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
		double[] yValues = new double[] {1.0, 2.0, 3.0, INF};
		INTERP.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void coincideXvaluesTest()
	  public virtual void coincideXvaluesTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 3.0};
		double[] yValues = new double[] {1.0, 2.0, 3.0, 4.0};
		INTERP.interpolate(xValues, yValues);
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
		const int orderExp = 2;
		const int dimExp = 2;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefsMatExp = new double[][] { {-5.0, 6.0 }, {3.0, 2.0 } };
		double[][] coefsMatExp = new double[][]
		{
			new double[] {-5.0, 6.0},
			new double[] {3.0, 2.0}
		};
		LinearInterpolator interpMatrix = new LinearInterpolator();
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
//ORIGINAL LINE: final double[][] yValues = new double[][] { {6.0, 1.0, 8.0, -2.0 }, {1.0, 1.0 / 3.0, 2.0 / 11.0, 1.0 / 7.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {6.0, 1.0, 8.0, -2.0},
			new double[] {1.0, 1.0 / 3.0, 2.0 / 11.0, 1.0 / 7.0}
		};

		const int nIntervalsExp = 3;
		const int orderExp = 2;
		const int dimExp = 2;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefsMatExp = new double[][] { {-5.0, 6.0 }, {-2.0 / 3.0, 1.0 }, {7.0, 1.0 }, {-5.0 / 33.0, 1.0 / 3.0 }, {-10.0, 8.0 }, {-3.0 / 77.0, 2.0 / 11.0 } };
		double[][] coefsMatExp = new double[][]
		{
			new double[] {-5.0, 6.0},
			new double[] {-2.0 / 3.0, 1.0},
			new double[] {7.0, 1.0},
			new double[] {-5.0 / 33.0, 1.0 / 3.0},
			new double[] {-10.0, 8.0},
			new double[] {-3.0 / 77.0, 2.0 / 11.0}
		};
		LinearInterpolator interpMatrix = new LinearInterpolator();
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
		INTERP.interpolate(xValues, yValues);
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
		INTERP.interpolate(xValues, yValues);
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
		INTERP.interpolate(xValues, yValues);
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
		INTERP.interpolate(xValues, yValues);
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
		INTERP.interpolate(xValues, yValues);
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
		INTERP.interpolate(xValues, yValues);
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
		INTERP.interpolate(xValues, yValues);
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
		INTERP.interpolate(xValues, yValues);
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
		INTERP.interpolate(xValues, yValues);
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
//ORIGINAL LINE: final double[][][] resultValuesExpected = new double[][][] { { {29.0 / 3.0, 15.0 / 4.0 }, {16.0, 1.0 / 2.0 } }, { {83.0 / 12.0, 11.0 / 3.0 }, {17.0 / 2.0, 0.0 } }, { {61.0 / 12.0, 13.0 / 3.0 }, {7.0 / 2.0, 0.0 } } };
		double[][][] resultValuesExpected = new double[][][]
		{
			new double[][]
			{
				new double[] {29.0 / 3.0, 15.0 / 4.0},
				new double[] {16.0, 1.0 / 2.0}
			},
			new double[][]
			{
				new double[] {83.0 / 12.0, 11.0 / 3.0},
				new double[] {17.0 / 2.0, 0.0}
			},
			new double[][]
			{
				new double[] {61.0 / 12.0, 13.0 / 3.0},
				new double[] {7.0 / 2.0, 0.0}
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

		LinearInterpolator interp = new LinearInterpolator();

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
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void LargeOutputTest()
	  public virtual void LargeOutputTest()
	  {
		double[] xValues = new double[] {1.0, 2.e-308, 3.e-308, 4.0};
		double[] yValues = new double[] {1.0, 2.0, 1.e308, 3.0};
		INTERP.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void LargeOutputMultiTest()
	  public virtual void LargeOutputMultiTest()
	  {
		double[] xValues = new double[] {1.0, 2.e-308, 3.e-308, 4.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.e307, 3.0, 4.0},
			new double[] {2.0, 2.0, 3.0, 4.0}
		};
		INTERP.interpolate(xValues, yValues);
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
		INTERP.interpolate(xValues, yValues[0], 1.e308);
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
		INTERP.interpolate(xValues, yValues, xKey);
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
		INTERP.interpolate(xValues, yValues, xKey);
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
		INTERP.interpolate(xValues, yValues, xKey);
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
		INTERP.interpolate(xValues, yValues, xKey);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void notReconnectedTest()
	  public virtual void notReconnectedTest()
	  {
		double[] xValues = new double[] {1.0, 2.000000000001, 2.000000000002, 4.0};
		double[] yValues = new double[] {2.0, 4.e10, 3.e-5, 5.e11};

		PiecewisePolynomialInterpolator interpPos = new LinearInterpolator();
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void notReconnectedMultiTest()
	  public virtual void notReconnectedMultiTest()
	  {
		double[] xValues = new double[] {1.0, 2.000000000001, 2.000000000002, 4.0};
		double[][] yValues = new double[][]
		{
			new double[] {2.0, 4.e10, 3.e-5, 5.e11}
		};

		PiecewisePolynomialInterpolator interpPos = new LinearInterpolator();
		interpPos.interpolate(xValues, yValues);
	  }

	  //-------------------------------------------------------------------------
	  private void testSensitivity(double[] xValues, double[] yValues, double[] keys, double delta)
	  {
		PiecewisePolynomialWithSensitivityFunction1D func = new PiecewisePolynomialWithSensitivityFunction1D();
		PiecewisePolynomialResultsWithSensitivity resultSensi = INTERP.interpolateWithSensitivity(xValues, yValues);
		DoubleArray[] computedArray = func.nodeSensitivity(resultSensi, keys);
		for (int i = 0; i < keys.Length; ++i)
		{
		  double @base = func.evaluate(resultSensi, keys[i]).get(0);
		  DoubleArray computed = func.nodeSensitivity(resultSensi, keys[i]);
		  assertEquals(computed, computedArray[i]);
		  for (int j = 0; j < yValues.Length; ++j)
		  {
			double[] yValuesBump = Arrays.copyOf(yValues, yValues.Length);
			yValuesBump[j] += delta;
			PiecewisePolynomialResult resultBump = INTERP.interpolate(xValues, yValuesBump);
			double expected = (func.evaluate(resultBump, keys[i]).get(0) - @base) / delta;
			assertEquals(computed.get(j), expected, delta);
		  }
		}
	  }
	}

}