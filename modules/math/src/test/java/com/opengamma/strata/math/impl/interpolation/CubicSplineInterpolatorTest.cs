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
//ORIGINAL LINE: @Test public class CubicSplineInterpolatorTest
	public class CubicSplineInterpolatorTest
	{
	  private const double EPS = 1e-14;

	  /// <summary>
	  /// All of the recovery tests for normal values with Clamped endpoint condition
	  /// </summary>
	  public virtual void ClampedRecoverTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1, 3, 2, 4 };
		double[] xValues = new double[] {1, 3, 2, 4};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {3, -1, 1, 0, 8, 12 };
		double[] yValues = new double[] {3, -1, 1, 0, 8, 12};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValuesMatrix = new double[][] { {3, -1, 1, 0, 8, 12 }, {-20, 20, 0, 5, 5, 10 } };
		double[][] yValuesMatrix = new double[][]
		{
			new double[] {3, -1, 1, 0, 8, 12},
			new double[] {-20, 20, 0, 5, 5, 10}
		};

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xSamples = new double[] {0, 5.0 / 2.0, 5.0 / 3.0, 29.0 / 7.0 };
		double[] xSamples = new double[] {0, 5.0 / 2.0, 5.0 / 3.0, 29.0 / 7.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] ySamples = new double[][] { {-8, 1.0 / 8.0, -1.0 / 27.0, Math.pow(15.0 / 7.0, 3.0) }, {45.0, 5.0 / 4.0, 80.0 / 9.0, 320.0 / 49.0 } };
		double[][] ySamples = new double[][]
		{
			new double[] {-8, 1.0 / 8.0, -1.0 / 27.0, Math.Pow(15.0 / 7.0, 3.0)},
			new double[] {45.0, 5.0 / 4.0, 80.0 / 9.0, 320.0 / 49.0}
		};

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] xSamplesMatrix = new double[][] { {-2, 1, 2, 3 }, {4, 5, 6, 7 } };
		double[][] xSamplesMatrix = new double[][]
		{
			new double[] {-2, 1, 2, 3},
			new double[] {4, 5, 6, 7}
		};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][][] ySamplesMatrix = new double[][][] { { {-64.0, -1.0, 0.0, 1.0 }, {8.0, 27.0, 64.0, 125.0 } }, { {125.0, 20.0, 5.0, 0 }, {5.0, 20.0, 45.0, 80.0 } } };
		double[][][] ySamplesMatrix = new double[][][]
		{
			new double[][]
			{
				new double[] {-64.0, -1.0, 0.0, 1.0},
				new double[] {8.0, 27.0, 64.0, 125.0}
			},
			new double[][]
			{
				new double[] {125.0, 20.0, 5.0, 0},
				new double[] {5.0, 20.0, 45.0, 80.0}
			}
		};
		const int xDim = 2;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix coefsExpectedMatrix = com.opengamma.strata.collect.array.DoubleMatrix.copyOf(new double[][] { {1.0, -3.0, 3.0, -1}, {0.0, 5.0, -20.0, 20}, {1.0, 0.0, 0.0, 0.0}, {0.0, 5.0, -10.0, 5}, {1.0, 3.0, 3.0, 1.0}, {0.0, 5.0, 0.0, 0.0}});
		DoubleMatrix coefsExpectedMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {0.0, 5.0, -20.0, 20},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 5.0, -10.0, 5},
			new double[] {1.0, 3.0, 3.0, 1.0},
			new double[] {0.0, 5.0, 0.0, 0.0}
		});
		const int dimMatrix = 2;
		const int orderMatrix = 4;
		const int nIntervalsMatrix = 3;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] knotsMatrix = new double[] {1, 2, 3, 4 };
		double[] knotsMatrix = new double[] {1, 2, 3, 4};

		CubicSplineInterpolator interpMatrix = new CubicSplineInterpolator();

		PiecewisePolynomialResult resultMatrix = interpMatrix.interpolate(xValues, yValuesMatrix);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nRows = coefsExpectedMatrix.rowCount();
		int nRows = coefsExpectedMatrix.rowCount();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nCols = coefsExpectedMatrix.columnCount();
		int nCols = coefsExpectedMatrix.columnCount();
		for (int i = 0; i < nRows; ++i)
		{
		  for (int j = 0; j < nCols; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = coefsExpectedMatrix.get(i, j) == 0.0 ? 1.0 : Math.abs(coefsExpectedMatrix.get(i, j));
			double @ref = coefsExpectedMatrix.get(i, j) == 0.0 ? 1.0 : Math.Abs(coefsExpectedMatrix.get(i, j));
			assertEquals(resultMatrix.CoefMatrix.get(i, j), coefsExpectedMatrix.get(i, j), @ref * EPS);
		  }
		}

		assertEquals(resultMatrix.NumberOfIntervals, nIntervalsMatrix);
		assertEquals(resultMatrix.Order, orderMatrix);
		assertEquals(resultMatrix.Dimensions, dimMatrix);
		assertEquals(resultMatrix.Knots.toArray(), knotsMatrix);

		DoubleMatrix resultValuesMatrix2D = interpMatrix.interpolate(xValues, yValuesMatrix, xSamples);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nSamples = xSamples.length;
		int nSamples = xSamples.Length;
		for (int i = 0; i < dimMatrix; ++i)
		{
		  for (int j = 0; j < nSamples; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = ySamples[i][j] == 0.0 ? 1.0 : Math.abs(ySamples[i][j]);
			double @ref = ySamples[i][j] == 0.0 ? 1.0 : Math.Abs(ySamples[i][j]);
			assertEquals(resultValuesMatrix2D.get(i, j), ySamples[i][j], @ref * EPS);
		  }
		}

		DoubleArray resultValuesMatrix1D = interpMatrix.interpolate(xValues, yValuesMatrix, xSamples[0]);
		for (int i = 0; i < dimMatrix; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = ySamples[i][0] == 0.0 ? 1.0 : Math.abs(ySamples[i][0]);
		  double @ref = ySamples[i][0] == 0.0 ? 1.0 : Math.Abs(ySamples[i][0]);
		  assertEquals(resultValuesMatrix1D.get(i), ySamples[i][0], @ref * EPS);
		}

		DoubleMatrix[] resultValuesMatrix2DVec = interpMatrix.interpolate(xValues, yValuesMatrix, xSamplesMatrix);
		for (int i = 0; i < nSamples; ++i)
		{
		  for (int j = 0; j < dimMatrix; ++j)
		  {
			for (int k = 0; k < xDim; ++k)
			{
			  double @ref = ySamplesMatrix[j][k][i] == 0.0 ? 1.0 : Math.Abs(ySamplesMatrix[j][k][i]);
			  assertEquals(resultValuesMatrix2DVec[i].get(j, k), ySamplesMatrix[j][k][i], @ref * EPS);
			}
		  }
		}

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

		for (int i = 0; i < nIntervalsMatrix; ++i)
		{
		  for (int j = 0; j < nCols; ++j)
		  {
			double @ref = coefsExpectedMatrix.get(2 * i, j) == 0.0 ? 1.0 : Math.Abs(coefsExpectedMatrix.get(2 * i, j));
			assertEquals(result.CoefMatrix.get(i, j), coefsExpectedMatrix.get(2 * i, j), @ref * EPS);
		  }
		}

		assertEquals(result.NumberOfIntervals, nIntervalsMatrix);
		assertEquals(result.Order, orderMatrix);
		assertEquals(result.Dimensions, 1);
		assertEquals(result.Knots.toArray(), knotsMatrix);

		DoubleArray resultValues1D = interp.interpolate(xValues, yValues, xSamples);
		for (int j = 0; j < nSamples; ++j)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = ySamples[0][j] == 0.0 ? 1.0 : Math.abs(ySamples[0][j]);
		  double @ref = ySamples[0][j] == 0.0 ? 1.0 : Math.Abs(ySamples[0][j]);
		  assertEquals(resultValues1D.get(j), ySamples[0][j], @ref * EPS);
		}

		double resultValue = interp.interpolate(xValues, yValues, xSamples[1]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double refb = ySamples[0][1] == 0.0 ? 1.0 : Math.abs(ySamples[0][1]);
		double refb = ySamples[0][1] == 0.0 ? 1.0 : Math.Abs(ySamples[0][1]);
		assertEquals(resultValue, ySamples[0][1], refb * EPS);

		DoubleMatrix resultValuesMatrix2DSingle = interp.interpolate(xValues, yValues, xSamplesMatrix);
		for (int i = 0; i < nSamples; ++i)
		{
		  for (int k = 0; k < xDim; ++k)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = ySamplesMatrix[0][k][i] == 0.0 ? 1.0 : Math.abs(ySamplesMatrix[0][k][i]);
			double @ref = ySamplesMatrix[0][k][i] == 0.0 ? 1.0 : Math.Abs(ySamplesMatrix[0][k][i]);
			assertEquals(resultValuesMatrix2DSingle.get(k, i), ySamplesMatrix[0][k][i], @ref * EPS);
		  }
		}

	  }

	  /// <summary>
	  /// All of the recovery tests for normal values with Not-A-Knot endpoint conditions
	  /// </summary>
	  public virtual void NotAKnotRecoverTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1, 3, 2, 4 };
		double[] xValues = new double[] {1, 3, 2, 4};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {-1, 1, 0, 8 };
		double[] yValues = new double[] {-1, 1, 0, 8};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValuesMatrix = new double[][] { {-1, 1, 0, 8 }, {20, 0, 5, 5 } };
		double[][] yValuesMatrix = new double[][]
		{
			new double[] {-1, 1, 0, 8},
			new double[] {20, 0, 5, 5}
		};

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xSamples = new double[] {0, 5.0 / 2.0, 7.0 / 3.0, 29.0 / 7.0 };
		double[] xSamples = new double[] {0, 5.0 / 2.0, 7.0 / 3.0, 29.0 / 7.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] ySamples = new double[][] { {-8, 1.0 / 8.0, 1.0 / 27.0, Math.pow(15.0 / 7.0, 3.0) }, {45.0, 5.0 / 4.0, 20.0 / 9.0, 320.0 / 49.0 } };
		double[][] ySamples = new double[][]
		{
			new double[] {-8, 1.0 / 8.0, 1.0 / 27.0, Math.Pow(15.0 / 7.0, 3.0)},
			new double[] {45.0, 5.0 / 4.0, 20.0 / 9.0, 320.0 / 49.0}
		};

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] xSamplesMatrix = new double[][] { {-2, 1, 2, 2.5 }, {4, 5, 6, 7 } };
		double[][] xSamplesMatrix = new double[][]
		{
			new double[] {-2, 1, 2, 2.5},
			new double[] {4, 5, 6, 7}
		};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][][] ySamplesMatrix = new double[][][] { { {-64.0, -1.0, 0.0, 1.0 / 8.0 }, {8.0, 27.0, 64.0, 125.0 } }, { {125.0, 20.0, 5.0, 5.0 / 4.0 }, {5.0, 20.0, 45.0, 80.0 } } };
		double[][][] ySamplesMatrix = new double[][][]
		{
			new double[][]
			{
				new double[] {-64.0, -1.0, 0.0, 1.0 / 8.0},
				new double[] {8.0, 27.0, 64.0, 125.0}
			},
			new double[][]
			{
				new double[] {125.0, 20.0, 5.0, 5.0 / 4.0},
				new double[] {5.0, 20.0, 45.0, 80.0}
			}
		};
		const int xDim = 2;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix coefsExpectedMatrix = com.opengamma.strata.collect.array.DoubleMatrix.copyOf(new double[][] { {1.0, -3.0, 3.0, -1}, {0.0, 5.0, -20.0, 20}, {1.0, 0.0, 0.0, 0.0}, {0.0, 5.0, -10.0, 5}, {1.0, 3.0, 3.0, 1.0}, {0.0, 5.0, 0.0, 0.0}});
		DoubleMatrix coefsExpectedMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {0.0, 5.0, -20.0, 20},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 5.0, -10.0, 5},
			new double[] {1.0, 3.0, 3.0, 1.0},
			new double[] {0.0, 5.0, 0.0, 0.0}
		});
		const int dimMatrix = 2;
		const int orderMatrix = 4;
		const int nIntervalsMatrix = 3;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] knotsMatrix = new double[] {1, 2, 3, 4 };
		double[] knotsMatrix = new double[] {1, 2, 3, 4};

		CubicSplineInterpolator interpMatrix = new CubicSplineInterpolator();

		PiecewisePolynomialResult resultMatrix = interpMatrix.interpolate(xValues, yValuesMatrix);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nRows = coefsExpectedMatrix.rowCount();
		int nRows = coefsExpectedMatrix.rowCount();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nCols = coefsExpectedMatrix.columnCount();
		int nCols = coefsExpectedMatrix.columnCount();
		for (int i = 0; i < nRows; ++i)
		{
		  for (int j = 0; j < nCols; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = coefsExpectedMatrix.get(i, j) == 0.0 ? 1.0 : Math.abs(coefsExpectedMatrix.get(i, j));
			double @ref = coefsExpectedMatrix.get(i, j) == 0.0 ? 1.0 : Math.Abs(coefsExpectedMatrix.get(i, j));
			assertEquals(resultMatrix.CoefMatrix.get(i, j), coefsExpectedMatrix.get(i, j), @ref * EPS);
		  }
		}

		assertEquals(resultMatrix.NumberOfIntervals, nIntervalsMatrix);
		assertEquals(resultMatrix.Order, orderMatrix);
		assertEquals(resultMatrix.Dimensions, dimMatrix);
		assertEquals(resultMatrix.Knots.toArray(), knotsMatrix);

		DoubleMatrix resultValuesMatrix2D = interpMatrix.interpolate(xValues, yValuesMatrix, xSamples);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nSamples = xSamples.length;
		int nSamples = xSamples.Length;
		for (int i = 0; i < dimMatrix; ++i)
		{
		  for (int j = 0; j < nSamples; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = ySamples[i][j] == 0.0 ? 1.0 : Math.abs(ySamples[i][j]);
			double @ref = ySamples[i][j] == 0.0 ? 1.0 : Math.Abs(ySamples[i][j]);
			assertEquals(resultValuesMatrix2D.get(i, j), ySamples[i][j], @ref * EPS);
		  }
		}

		DoubleArray resultValuesMatrix1D = interpMatrix.interpolate(xValues, yValuesMatrix, xSamples[0]);
		for (int i = 0; i < dimMatrix; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = ySamples[i][0] == 0.0 ? 1.0 : Math.abs(ySamples[i][0]);
		  double @ref = ySamples[i][0] == 0.0 ? 1.0 : Math.Abs(ySamples[i][0]);
		  assertEquals(resultValuesMatrix1D.get(i), ySamples[i][0], @ref * EPS);
		}

		DoubleMatrix[] resultValuesMatrix2DVec = interpMatrix.interpolate(xValues, yValuesMatrix, xSamplesMatrix);
		for (int i = 0; i < nSamples; ++i)
		{
		  for (int j = 0; j < dimMatrix; ++j)
		  {
			for (int k = 0; k < xDim; ++k)
			{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = ySamplesMatrix[j][k][i] == 0.0 ? 1.0 : Math.abs(ySamplesMatrix[j][k][i]);
			  double @ref = ySamplesMatrix[j][k][i] == 0.0 ? 1.0 : Math.Abs(ySamplesMatrix[j][k][i]);
			  assertEquals(resultValuesMatrix2DVec[i].get(j, k), ySamplesMatrix[j][k][i], @ref * EPS);
			}
		  }
		}

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

		for (int i = 0; i < nIntervalsMatrix; ++i)
		{
		  for (int j = 0; j < nCols; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = coefsExpectedMatrix.get(2 * i, j) == 0.0 ? 1.0 : Math.abs(coefsExpectedMatrix.get(2 * i, j));
			double @ref = coefsExpectedMatrix.get(2 * i, j) == 0.0 ? 1.0 : Math.Abs(coefsExpectedMatrix.get(2 * i, j));
			assertEquals(result.CoefMatrix.get(i, j), coefsExpectedMatrix.get(2 * i, j), @ref * EPS);
		  }
		}

		assertEquals(result.NumberOfIntervals, nIntervalsMatrix);
		assertEquals(result.Order, orderMatrix);
		assertEquals(result.Dimensions, 1);
		assertEquals(result.Knots.toArray(), knotsMatrix);

		DoubleArray resultValues1D = interp.interpolate(xValues, yValues, xSamples);
		for (int j = 0; j < nSamples; ++j)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = ySamples[0][j] == 0.0 ? 1.0 : Math.abs(ySamples[0][j]);
		  double @ref = ySamples[0][j] == 0.0 ? 1.0 : Math.Abs(ySamples[0][j]);
		  assertEquals(resultValues1D.get(j), ySamples[0][j], @ref * EPS);
		}

		double resultValue = interp.interpolate(xValues, yValues, xSamples[1]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double refb = ySamples[0][1] == 0.0 ? 1.0 : Math.abs(ySamples[0][1]);
		double refb = ySamples[0][1] == 0.0 ? 1.0 : Math.Abs(ySamples[0][1]);
		assertEquals(resultValue, ySamples[0][1], refb * EPS);

		DoubleMatrix resultValuesMatrix2DSingle = interp.interpolate(xValues, yValues, xSamplesMatrix);
		for (int i = 0; i < nSamples; ++i)
		{
		  for (int k = 0; k < xDim; ++k)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = ySamplesMatrix[0][k][i] == 0.0 ? 1.0 : Math.abs(ySamplesMatrix[0][k][i]);
			double @ref = ySamplesMatrix[0][k][i] == 0.0 ? 1.0 : Math.Abs(ySamplesMatrix[0][k][i]);
			assertEquals(resultValuesMatrix2DSingle.get(k, i), ySamplesMatrix[0][k][i], @ref * EPS);
		  }
		}

	  }

	  /// <summary>
	  /// For a small number of DataPoints with Not-A-Knot endpoint conditions, spline may reduce into linear or quadratic
	  /// Knots and coefficient Matrix are also reduced in these cases
	  /// </summary>
	  public virtual void LinearAndQuadraticNakTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValuesForLin = new double[] {1.0, 2.0 };
		double[] xValuesForLin = new double[] {1.0, 2.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValuesForLin = new double[][] { {3.0, 7.0 }, {2, -6 } };
		double[][] yValuesForLin = new double[][]
		{
			new double[] {3.0, 7.0},
			new double[] {2, -6}
		};

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValuesForQuad = new double[] {1.0, 2.0, 3.0 };
		double[] xValuesForQuad = new double[] {1.0, 2.0, 3.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValuesForQuad = new double[][] { {1.0, 6.0, 5.0 }, {2.0, -2.0, -3.0 } };
		double[][] yValuesForQuad = new double[][]
		{
			new double[] {1.0, 6.0, 5.0},
			new double[] {2.0, -2.0, -3.0}
		};

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefsExpectedForLin = new double[][] { {4.0, 3.0}, {-8.0, 2.0 } };
		double[][] coefsExpectedForLin = new double[][]
		{
			new double[] {4.0, 3.0},
			new double[] {-8.0, 2.0}
		};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefsExpectedForQuad = new double[][] { {-3.0, 8.0, 1.0 }, {3.0 / 2.0, -11.0 / 2.0, 2.0 } };
		double[][] coefsExpectedForQuad = new double[][]
		{
			new double[] {-3.0, 8.0, 1.0},
			new double[] {3.0 / 2.0, -11.0 / 2.0, 2.0}
		};

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] xKeys = new double[][] { {-0.5, 6.0 / 5.0, 2.38 }, {1.0, 2.0, 3.0 } };
		double[][] xKeys = new double[][]
		{
			new double[] {-0.5, 6.0 / 5.0, 2.38},
			new double[] {1.0, 2.0, 3.0}
		};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][][] yExpectedForLin = new double[][][] { { {-3.0, 3.8, 8.52 }, {3.0, 7.0, 11.0 } }, { {14.0, 0.4, -9.04 }, {2.0, -6.0, -14.0 } } };
		double[][][] yExpectedForLin = new double[][][]
		{
			new double[][]
			{
				new double[] {-3.0, 3.8, 8.52},
				new double[] {3.0, 7.0, 11.0}
			},
			new double[][]
			{
				new double[] {14.0, 0.4, -9.04},
				new double[] {2.0, -6.0, -14.0}
			}
		};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][][] yExpectedForQuad = new double[][][] { { {-17.75, 2.48, 6.3268 }, {1.0, 6.0, 5.0 } }, { {13.625, 0.96, -2.7334 }, {2.0, -2.0, -3.0 } } };
		double[][][] yExpectedForQuad = new double[][][]
		{
			new double[][]
			{
				new double[] {-17.75, 2.48, 6.3268},
				new double[] {1.0, 6.0, 5.0}
			},
			new double[][]
			{
				new double[] {13.625, 0.96, -2.7334},
				new double[] {2.0, -2.0, -3.0}
			}
		};

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int keyDim = xKeys.length;
		int keyDim = xKeys.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int keyLength = xKeys[0].length;
		int keyLength = xKeys[0].Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int yDim = yValuesForLin.length;
		int yDim = yValuesForLin.Length;

		/// <summary>
		/// Linear Interpolation
		/// </summary>

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		PiecewisePolynomialResult resultLin = interp.interpolate(xValuesForLin, yValuesForLin);

		int nRowsLin = coefsExpectedForLin.Length;
		int nColsLin = coefsExpectedForLin[0].Length;
		for (int i = 0; i < nRowsLin; ++i)
		{
		  for (int j = 0; j < nColsLin; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = coefsExpectedForLin[i][j] == 0.0 ? 1.0 : Math.abs(coefsExpectedForLin[i][j]);
			double @ref = coefsExpectedForLin[i][j] == 0.0 ? 1.0 : Math.Abs(coefsExpectedForLin[i][j]);
			assertEquals(resultLin.CoefMatrix.get(i, j), coefsExpectedForLin[i][j], @ref * EPS);
		  }
		}

		assertEquals(resultLin.Dimensions, yDim);
		assertEquals(resultLin.NumberOfIntervals, 1);
		assertEquals(resultLin.Knots.toArray(), xValuesForLin);
		assertEquals(resultLin.Order, 2);

		resultLin = interp.interpolate(xValuesForLin, yValuesForLin[0]);

		for (int i = 0; i < 2; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = coefsExpectedForLin[0][i] == 0.0 ? 1.0 : Math.abs(coefsExpectedForLin[0][i]);
		  double @ref = coefsExpectedForLin[0][i] == 0.0 ? 1.0 : Math.Abs(coefsExpectedForLin[0][i]);
		  assertEquals(resultLin.CoefMatrix.get(0, i), coefsExpectedForLin[0][i], @ref * EPS);
		}

		assertEquals(resultLin.Dimensions, 1);
		assertEquals(resultLin.NumberOfIntervals, 1);
		assertEquals(resultLin.Knots.toArray(), xValuesForLin);
		assertEquals(resultLin.Order, 2);

		DoubleMatrix resultMatrixLin2D = interp.interpolate(xValuesForLin, yValuesForLin[0], xKeys);
		for (int i = 0; i < keyDim; ++i)
		{
		  for (int j = 0; j < keyLength; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = yExpectedForLin[0][i][j] == 0.0 ? 1.0 : Math.abs(yExpectedForLin[0][i][j]);
			double @ref = yExpectedForLin[0][i][j] == 0.0 ? 1.0 : Math.Abs(yExpectedForLin[0][i][j]);
			assertEquals(resultMatrixLin2D.get(i, j), yExpectedForLin[0][i][j], @ref * EPS);
		  }
		}

		DoubleArray resultMatrixLin1D = interp.interpolate(xValuesForLin, yValuesForLin[0], xKeys[0]);
		for (int j = 0; j < keyLength; ++j)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = yExpectedForLin[0][0][j] == 0.0 ? 1.0 : Math.abs(yExpectedForLin[0][0][j]);
		  double @ref = yExpectedForLin[0][0][j] == 0.0 ? 1.0 : Math.Abs(yExpectedForLin[0][0][j]);
		  assertEquals(resultMatrixLin1D.get(j), yExpectedForLin[0][0][j], @ref * EPS);
		}

		double resultMatrixLinValue = interp.interpolate(xValuesForLin, yValuesForLin[0], xKeys[0][0]);
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = yExpectedForLin[0][0][0] == 0.0 ? 1.0 : Math.abs(yExpectedForLin[0][0][0]);
		  double @ref = yExpectedForLin[0][0][0] == 0.0 ? 1.0 : Math.Abs(yExpectedForLin[0][0][0]);
		  assertEquals(resultMatrixLinValue, yExpectedForLin[0][0][0], @ref * EPS);
		}

		DoubleArray resultMatrixLinValues1D = interp.interpolate(xValuesForLin, yValuesForLin, xKeys[0][0]);
		for (int i = 0; i < yDim; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = yExpectedForLin[i][0][0] == 0.0 ? 1.0 : Math.abs(yExpectedForLin[i][0][0]);
		  double @ref = yExpectedForLin[i][0][0] == 0.0 ? 1.0 : Math.Abs(yExpectedForLin[i][0][0]);
		  assertEquals(resultMatrixLinValues1D.get(i), yExpectedForLin[i][0][0], @ref * EPS);
		}

		DoubleMatrix[] resultMatrixLin2DVec = interp.interpolate(xValuesForLin, yValuesForLin, xKeys);
		for (int i = 0; i < yDim; ++i)
		{
		  for (int j = 0; j < keyDim; ++j)
		  {
			for (int k = 0; k < keyLength; ++k)
			{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = yExpectedForLin[i][j][k] == 0.0 ? 1.0 : Math.abs(yExpectedForLin[i][j][k]);
			  double @ref = yExpectedForLin[i][j][k] == 0.0 ? 1.0 : Math.Abs(yExpectedForLin[i][j][k]);
			  assertEquals(resultMatrixLin2DVec[k].get(i, j), yExpectedForLin[i][j][k], @ref * EPS);
			}
		  }
		}

		/// <summary>
		/// Quadratic Interpolation
		/// </summary>

		PiecewisePolynomialResult resultQuad = interp.interpolate(xValuesForQuad, yValuesForQuad);

		int nRowsQuad = coefsExpectedForQuad.Length;
		int nColsQuad = coefsExpectedForQuad[0].Length;
		for (int i = 0; i < nRowsQuad; ++i)
		{
		  for (int j = 0; j < nColsQuad; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = coefsExpectedForQuad[i][j] == 0.0 ? 1.0 : Math.abs(coefsExpectedForQuad[i][j]);
			double @ref = coefsExpectedForQuad[i][j] == 0.0 ? 1.0 : Math.Abs(coefsExpectedForQuad[i][j]);
			assertEquals(resultQuad.CoefMatrix.get(i, j), coefsExpectedForQuad[i][j], @ref * EPS);
		  }
		}

		assertEquals(resultQuad.Dimensions, yDim);
		assertEquals(resultQuad.NumberOfIntervals, 1);
		assertEquals(resultQuad.Knots.toArray(), new double[] {xValuesForQuad[0], xValuesForQuad[2]});
		assertEquals(resultQuad.Order, 3);

		resultQuad = interp.interpolate(xValuesForQuad, yValuesForQuad[0]);

		for (int i = 0; i < 3; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = coefsExpectedForQuad[0][i] == 0.0 ? 1.0 : Math.abs(coefsExpectedForQuad[0][i]);
		  double @ref = coefsExpectedForQuad[0][i] == 0.0 ? 1.0 : Math.Abs(coefsExpectedForQuad[0][i]);
		  assertEquals(resultQuad.CoefMatrix.get(0, i), coefsExpectedForQuad[0][i], @ref * EPS);
		}

		assertEquals(resultQuad.Dimensions, 1);
		assertEquals(resultQuad.NumberOfIntervals, 1);
		assertEquals(resultQuad.Knots.toArray(), new double[] {xValuesForQuad[0], xValuesForQuad[2]});
		assertEquals(resultQuad.Order, 3);

		DoubleMatrix resultMatrixQuad2D = interp.interpolate(xValuesForQuad, yValuesForQuad[0], xKeys);
		for (int i = 0; i < keyDim; ++i)
		{
		  for (int j = 0; j < keyLength; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = yExpectedForQuad[0][i][j] == 0.0 ? 1.0 : Math.abs(yExpectedForQuad[0][i][j]);
			double @ref = yExpectedForQuad[0][i][j] == 0.0 ? 1.0 : Math.Abs(yExpectedForQuad[0][i][j]);
			assertEquals(resultMatrixQuad2D.get(i, j), yExpectedForQuad[0][i][j], @ref * EPS);
		  }
		}

		DoubleArray resultMatrixQuad1D = interp.interpolate(xValuesForQuad, yValuesForQuad[0], xKeys[0]);
		for (int j = 0; j < keyLength; ++j)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = yExpectedForQuad[0][0][j] == 0.0 ? 1.0 : Math.abs(yExpectedForQuad[0][0][j]);
		  double @ref = yExpectedForQuad[0][0][j] == 0.0 ? 1.0 : Math.Abs(yExpectedForQuad[0][0][j]);
		  assertEquals(resultMatrixQuad1D.get(j), yExpectedForQuad[0][0][j], @ref * EPS);
		}

		double resultMatrixQuadValue = interp.interpolate(xValuesForQuad, yValuesForQuad[0], xKeys[0][0]);
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = yExpectedForQuad[0][0][0] == 0.0 ? 1.0 : Math.abs(yExpectedForQuad[0][0][0]);
		  double @ref = yExpectedForQuad[0][0][0] == 0.0 ? 1.0 : Math.Abs(yExpectedForQuad[0][0][0]);
		  assertEquals(resultMatrixQuadValue, yExpectedForQuad[0][0][0], @ref * EPS);
		}

		DoubleArray resultMatrixQuadValues1D = interp.interpolate(xValuesForQuad, yValuesForQuad, xKeys[0][0]);
		for (int i = 0; i < yDim; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = yExpectedForQuad[i][0][0] == 0.0 ? 1.0 : Math.abs(yExpectedForQuad[i][0][0]);
		  double @ref = yExpectedForQuad[i][0][0] == 0.0 ? 1.0 : Math.Abs(yExpectedForQuad[i][0][0]);
		  assertEquals(resultMatrixQuadValues1D.get(i), yExpectedForQuad[i][0][0], @ref * EPS);
		}

		DoubleMatrix[] resultMatrixQuad2DVec = interp.interpolate(xValuesForQuad, yValuesForQuad, xKeys);
		for (int i = 0; i < yDim; ++i)
		{
		  for (int j = 0; j < keyDim; ++j)
		  {
			for (int k = 0; k < keyLength; ++k)
			{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = yExpectedForQuad[i][j][k] == 0.0 ? 1.0 : Math.abs(yExpectedForQuad[i][j][k]);
			  double @ref = yExpectedForQuad[i][j][k] == 0.0 ? 1.0 : Math.Abs(yExpectedForQuad[i][j][k]);
			  assertEquals(resultMatrixQuad2DVec[k].get(i, j), yExpectedForQuad[i][j][k], @ref * EPS);
			}
		  }
		}

	  }

	  /// <summary>
	  /// Number of data should be larger than 1
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void DataShortNakTest()
	  public virtual void DataShortNakTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0 };
		double[] xValues = new double[] {1.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {4.0 };
		double[] yValues = new double[] {4.0};

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void DataShortNakMultiTest()
	  public virtual void DataShortNakMultiTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0 };
		double[] xValues = new double[] {1.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {4.0 }, {3.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {4.0},
			new double[] {3.0}
		};

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void DataShortClapmedTest()
	  public virtual void DataShortClapmedTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0 };
		double[] xValues = new double[] {1.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {0.0, 4.0, 3.0 };
		double[] yValues = new double[] {0.0, 4.0, 3.0};

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void DataShortClapmedMultiTest()
	  public virtual void DataShortClapmedMultiTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0 };
		double[] xValues = new double[] {1.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {0.0, 4.0, 3.0 }, {9.0, 4.0, 1.5 } };
		double[][] yValues = new double[][]
		{
			new double[] {0.0, 4.0, 3.0},
			new double[] {9.0, 4.0, 1.5}
		};

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);

	  }

	  /// <summary>
	  /// (yValues length) == (xValues length) + 2 or (yValues length) == (xValues length) should be satisfied
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void WrongDataLengthTest()
	  public virtual void WrongDataLengthTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1, 2, 3 };
		double[] xValues = new double[] {1, 2, 3};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {2, 3, 4, 5 };
		double[] yValues = new double[] {2, 3, 4, 5};

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void WrongDataLengthMultiTest()
	  public virtual void WrongDataLengthMultiTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1, 2, 3 };
		double[] xValues = new double[] {1, 2, 3};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {1, 3, 5, 2 }, {5, 3, 2, 7 }, {1, 8, -1, 0 } };
		double[][] yValues = new double[][]
		{
			new double[] {1, 3, 5, 2},
			new double[] {5, 3, 2, 7},
			new double[] {1, 8, -1, 0}
		};

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// <summary>
	  /// Repeated data are not allowed
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void RepeatDataTest()
	  public virtual void RepeatDataTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 0.5, 8.0, 1.0 / 2.0 };
		double[] xValues = new double[] {1.0, 2.0, 0.5, 8.0, 1.0 / 2.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {2.0, 3.0, 4.0, 5.0, 8.0 };
		double[] yValues = new double[] {2.0, 3.0, 4.0, 5.0, 8.0};

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void RepeatDataMultiTest()
	  public virtual void RepeatDataMultiTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 0.5, 8.0, 1.0 / 2.0 };
		double[] xValues = new double[] {1.0, 2.0, 0.5, 8.0, 1.0 / 2.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {2.0, 3.0, 4.0, 5.0, 8.0 }, {2.0, 1.0, 4.0, 2.0, 8.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {2.0, 3.0, 4.0, 5.0, 8.0},
			new double[] {2.0, 1.0, 4.0, 2.0, 8.0}
		};

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// <summary>
	  /// Data are null
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NullTest()
	  public virtual void NullTest()
	  {
		const double[] xValues = null;
		const double[] yValues = null;

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NullmultiTest()
	  public virtual void NullmultiTest()
	  {
		const double[] xValues = null;
		const double[][] yValues = null;

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// <summary>
	  /// Data are infinite-valued
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfinityXTest()
	  public virtual void InfinityXTest()
	  {

		const int nPts = 5;
		double[] xValues = new double[nPts];
		double[] yValues = new double[nPts];

		const double zero = 0.0;

		for (int i = 0; i < nPts; ++i)
		{
		  xValues[i] = 1.0 / zero;
		  yValues[i] = i;
		}

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfinityYTest()
	  public virtual void InfinityYTest()
	  {

		const int nPts = 5;
		double[] xValues = new double[nPts];
		double[] yValues = new double[nPts];

		const double zero = 0.0;

		for (int i = 0; i < nPts; ++i)
		{
		  xValues[i] = i;
		  yValues[i] = 1.0 / zero;
		}

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfinityXMultiTest()
	  public virtual void InfinityXMultiTest()
	  {

		const int nPts = 5;
		const int nDim = 3;
		double[] xValues = new double[nPts];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] yValues = new double[nDim][nPts];
		double[][] yValues = RectangularArrays.ReturnRectangularDoubleArray(nDim, nPts);

		const double zero = 0.0;

		for (int i = 0; i < nPts; ++i)
		{
		  xValues[i] = i;
		  for (int j = 0; j < nDim; ++j)
		  {
			yValues[j][i] = i;
		  }
		}
		xValues[1] = 1.0 / zero;

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfinityYMultiTest()
	  public virtual void InfinityYMultiTest()
	  {

		const int nPts = 5;
		const int nDim = 3;
		double[] xValues = new double[nPts];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] yValues = new double[nDim][nPts];
		double[][] yValues = RectangularArrays.ReturnRectangularDoubleArray(nDim, nPts);

		const double zero = 0.0;

		for (int i = 0; i < nPts; ++i)
		{
		  xValues[i] = i;
		  for (int j = 0; j < nDim; ++j)
		  {
			yValues[j][i] = i;
		  }
		}
		yValues[1][2] = 1.0 / zero;

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// <summary>
	  /// Data are NaN
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNXTest()
	  public virtual void NaNXTest()
	  {

		const int nPts = 5;
		double[] xValues = new double[nPts];
		double[] yValues = new double[nPts];

		for (int i = 0; i < nPts; ++i)
		{
		  xValues[i] = i;
		  yValues[i] = i;
		}

		xValues[1] = Double.NaN;

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNYTest()
	  public virtual void NaNYTest()
	  {

		const int nPts = 5;
		double[] xValues = new double[nPts];
		double[] yValues = new double[nPts];

		for (int i = 0; i < nPts; ++i)
		{
		  xValues[i] = i;
		  yValues[i] = i;
		}

		yValues[1] = Double.NaN;

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNXMultiTest()
	  public virtual void NaNXMultiTest()
	  {

		const int nPts = 5;
		const int nDim = 3;
		double[] xValues = new double[nPts];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] yValues = new double[nDim][nPts];
		double[][] yValues = RectangularArrays.ReturnRectangularDoubleArray(nDim, nPts);

		for (int i = 0; i < nPts; ++i)
		{
		  xValues[i] = i;
		  for (int j = 0; j < nDim; ++j)
		  {
			yValues[j][i] = i;
		  }
		}

		xValues[1] = Double.NaN;

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNYMultiTest()
	  public virtual void NaNYMultiTest()
	  {

		const int nPts = 5;
		const int nDim = 3;
		double[] xValues = new double[nPts];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] yValues = new double[nDim][nPts];
		double[][] yValues = RectangularArrays.ReturnRectangularDoubleArray(nDim, nPts);

		for (int i = 0; i < nPts; ++i)
		{
		  xValues[i] = i;
		  for (int j = 0; j < nDim; ++j)
		  {
			yValues[j][i] = i;
		  }
		}

		yValues[1][0] = Double.NaN;

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNOutputNakTest()
	  public virtual void NaNOutputNakTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1.0, 6.e307, -2.e306, 3.0 };
		double[] yValues = new double[] {1.0, 6.e307, -2.e306, 3.0};

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNOutputClampedTest()
	  public virtual void NaNOutputClampedTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {2.0, 1.0, 6.e307, -2.e306, 3.0, 6.0 };
		double[] yValues = new double[] {2.0, 1.0, 6.e307, -2.e306, 3.0, 6.0};

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNOutputNakMultiTest()
	  public virtual void NaNOutputNakMultiTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {1.0, 2.0, 3.0, 4.0 }, {1.0, 6.e307, -2.e306, 3.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 3.0, 4.0},
			new double[] {1.0, 6.e307, -2.e306, 3.0}
		};

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNOutputClampedMultiTest()
	  public virtual void NaNOutputClampedMultiTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {3.0, 1.0, 2.0, 3.0, 4.0, 1.0 }, {100.0, 1.0, 6.e307, -2.e306, 3.0, 2 } };
		double[][] yValues = new double[][]
		{
			new double[] {3.0, 1.0, 2.0, 3.0, 4.0, 1.0},
			new double[] {100.0, 1.0, 6.e307, -2.e306, 3.0, 2}
		};

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);

	  }

	  /// <summary>
	  /// Infinite output due to large data
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfOutputNakTest()
	  public virtual void InfOutputNakTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 1.000001 };
		double[] xValues = new double[] {1.0, 1.000001};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1.0, 3.e307 };
		double[] yValues = new double[] {1.0, 3.e307};

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfOutputClampedTest()
	  public virtual void InfOutputClampedTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 1.000001 };
		double[] xValues = new double[] {1.0, 1.000001};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {0.0, 1.0, 3.e307, 0.0 };
		double[] yValues = new double[] {0.0, 1.0, 3.e307, 0.0};

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfOutputNakMultiTest()
	  public virtual void InfOutputNakMultiTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 1.000001 };
		double[] xValues = new double[] {1.0, 1.000001};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {1.0, 2.0 }, {1.0, 3.e307 } };
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0},
			new double[] {1.0, 3.e307}
		};

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfOutputClampedMultiTest()
	  public virtual void InfOutputClampedMultiTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 1.000001 };
		double[] xValues = new double[] {1.0, 1.000001};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {3.0, 1.0, 2.0, 1.0 }, {0.0, 1.0, 3.e307, 0.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {3.0, 1.0, 2.0, 1.0},
			new double[] {0.0, 1.0, 3.e307, 0.0}
		};

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfOutputNakQuadTest()
	  public virtual void InfOutputNakQuadTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 1.000001, 1.000002 };
		double[] xValues = new double[] {1.0, 1.000001, 1.000002};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1.0, 3.e307, 3.e-307 };
		double[] yValues = new double[] {1.0, 3.e307, 3.e-307};

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfOutputNakQuadMultiTest()
	  public virtual void InfOutputNakQuadMultiTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 1.000001, 1.000002 };
		double[] xValues = new double[] {1.0, 1.000001, 1.000002};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {2.0, 3.0, 4.0 }, {1.0, 3.e307, 3.e-307 } };
		double[][] yValues = new double[][]
		{
			new double[] {2.0, 3.0, 4.0},
			new double[] {1.0, 3.e307, 3.e-307}
		};

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues);

	  }

	  /// <summary>
	  /// Infinite output due to large key
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void LargeKeyTest()
	  public virtual void LargeKeyTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {8.0, 6.0, 7.0, 8.0 };
		double[] yValues = new double[] {8.0, 6.0, 7.0, 8.0};
		const double key = 3.e103;

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues, key);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void LargeMultiKeyTest()
	  public virtual void LargeMultiKeyTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {8.0, 6.0, 7.0, 8.0 };
		double[] yValues = new double[] {8.0, 6.0, 7.0, 8.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] key = new double[] {1.0, 3.0, 3.e103 };
		double[] key = new double[] {1.0, 3.0, 3.e103};

		CubicSplineInterpolator interp = new CubicSplineInterpolator();
		interp.interpolate(xValues, yValues, key);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void LargeKeyMultiTest()
	  public virtual void LargeKeyMultiTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {8.0, 6.0, 7.0, 8.0 }, {3.0, 12.0, 1.0, 8.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {8.0, 6.0, 7.0, 8.0},
			new double[] {3.0, 12.0, 1.0, 8.0}
		};
		const double key = 3.e103;
		CubicSplineInterpolator interp = new CubicSplineInterpolator();
		interp.interpolate(xValues, yValues, key);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void LargeMultiKeyMultiTest()
	  public virtual void LargeMultiKeyMultiTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {8.0, 6.0, 7.0, 8.0 }, {3.0, 12.0, 1.0, 8.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {8.0, 6.0, 7.0, 8.0},
			new double[] {3.0, 12.0, 1.0, 8.0}
		};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] key = new double[] {1.0, 3.0, 3.e103 };
		double[] key = new double[] {1.0, 3.0, 3.e103};
		CubicSplineInterpolator interp = new CubicSplineInterpolator();
		interp.interpolate(xValues, yValues, key);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void LargeMatrixKeyTest()
	  public virtual void LargeMatrixKeyTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {8.0, 6.0, 7.0, 8.0 };
		double[] yValues = new double[] {8.0, 6.0, 7.0, 8.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] key = new double[][] { {1.0, 3.0, 3.e103 }, {0.1, 2.0, 5.0 } };
		double[][] key = new double[][]
		{
			new double[] {1.0, 3.0, 3.e103},
			new double[] {0.1, 2.0, 5.0}
		};

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues, key);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void LargeMatrixKeyMultiTest()
	  public virtual void LargeMatrixKeyMultiTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {8.0, 6.0, 7.0, 8.0 }, {3.0, 12.0, 1.0, 8.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {8.0, 6.0, 7.0, 8.0},
			new double[] {3.0, 12.0, 1.0, 8.0}
		};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] key = new double[][] { {1.0, 3.0, 3.e103 }, {0.1, 2.0, 5.0 } };
		double[][] key = new double[][]
		{
			new double[] {1.0, 3.0, 3.e103},
			new double[] {0.1, 2.0, 5.0}
		};

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues, key);

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

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

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

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

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

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

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

		CubicSplineInterpolator interp = new CubicSplineInterpolator();

		interp.interpolate(xValues, yValues, xKey);

	  }

	}

}