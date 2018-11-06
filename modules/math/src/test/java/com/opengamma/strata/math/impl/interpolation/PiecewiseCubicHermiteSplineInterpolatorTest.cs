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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Here we use the following notation
	/// h_i = xValues[i+1] - xValues[i]
	/// delta_i = (yValues[i+1] - yValues[i])/h_i
	/// d_i = dF(x)/dx |x=xValues[i] where F(x) is the piecewise interpolation function
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PiecewiseCubicHermiteSplineInterpolatorTest
	public class PiecewiseCubicHermiteSplineInterpolatorTest
	{

	  private const double EPS = 1e-14;
	  private const double INF = 1.0 / 0.0;

	  /// <summary>
	  /// Test for the case with boundary value d_0 = 0
	  /// </summary>
	  public virtual void BvCase1Test()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1.0, 2.0, 10.0, 11.0 };
		double[] yValues = new double[] {1.0, 2.0, 10.0, 11.0};

		const int nIntervalsExp = 3;
		const int orderExp = 4;
		const int dimExp = 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefsMatExp = new double[][] { {-2.0 / 9.0, 11.0 / 9.0, 0.0, 1.0 }, {-112.0 / 9.0, 56.0 / 3.0, 16.0 / 9.0, 2.0 }, {-2.0 / 9.0, -80.0 / 144.0, 16.0 / 9.0, 10.0 } };
		double[][] coefsMatExp = new double[][]
		{
			new double[] {-2.0 / 9.0, 11.0 / 9.0, 0.0, 1.0},
			new double[] {-112.0 / 9.0, 56.0 / 3.0, 16.0 / 9.0, 2.0},
			new double[] {-2.0 / 9.0, -80.0 / 144.0, 16.0 / 9.0, 10.0}
		};

		PiecewisePolynomialInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

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

	  /// <summary>
	  /// Test for the case with boundary value d_0 = 3 * delta_0
	  /// </summary>
	  public virtual void BvCase2Test()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {9.0, 10.0, 1.0, 3.0 };
		double[] yValues = new double[] {9.0, 10.0, 1.0, 3.0};

		const int nIntervalsExp = 3;
		const int orderExp = 4;
		const int dimExp = 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefsMatExp = new double[][] { {1.0, -3.0, 3.0, 9.0 }, {18.0, -27.0, 0.0, 10.0 }, {2.0, 0.0, 0.0, 1.0 } };
		double[][] coefsMatExp = new double[][]
		{
			new double[] {1.0, -3.0, 3.0, 9.0},
			new double[] {18.0, -27.0, 0.0, 10.0},
			new double[] {2.0, 0.0, 0.0, 1.0}
		};

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

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

	  /// <summary>
	  /// Test for the case with boundary value d_0 = ((2 * h_0 + h_1) * delta_0 - h_0 * delta_1)/(h_0 + h_1)
	  /// </summary>
	  public virtual void BvCase3Test()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {2.0, 3.0, 2.0, 3.0 };
		double[] yValues = new double[] {2.0, 3.0, 2.0, 3.0};

		const int nIntervalsExp = 3;
		const int orderExp = 4;
		const int dimExp = 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefsMatExp = new double[][] { {0.0, -1.0, 2.0, 2.0 }, {2.0, -3.0, 0.0, 3.0 }, {0.0, 1.0, 0.0, 2.0 } };
		double[][] coefsMatExp = new double[][]
		{
			new double[] {0.0, -1.0, 2.0, 2.0},
			new double[] {2.0, -3.0, 0.0, 3.0},
			new double[] {0.0, 1.0, 0.0, 2.0}
		};

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

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

	  /// <summary>
	  /// Test for the case with boundary value d_0 = ((2 * h_0 + h_1) * delta_0 - h_0 * delta_1)/(h_0 + h_1) corresponding to the other branch
	  /// </summary>
	  public virtual void BvCase3AnotherTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {2.0, 3.0, 2.0, 1.0 };
		double[] yValues = new double[] {2.0, 3.0, 2.0, 1.0};

		const int nIntervalsExp = 3;
		const int orderExp = 4;
		const int dimExp = 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefsMatExp = new double[][] { {0.0, -1.0, 2.0, 2.0 }, {1.0, -2.0, 0.0, 3.0 }, {0.0, 0.0, -1.0, 2.0 } };
		double[][] coefsMatExp = new double[][]
		{
			new double[] {0.0, -1.0, 2.0, 2.0},
			new double[] {1.0, -2.0, 0.0, 3.0},
			new double[] {0.0, 0.0, -1.0, 2.0}
		};

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

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

	  /// <summary>
	  /// d_i =0 if delta_i = 0 or delta_{i-1} = 0
	  /// </summary>
	  public virtual void CoincideYvaluesTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1.0, 2.0, 2.0, 3.0 };
		double[] yValues = new double[] {1.0, 2.0, 2.0, 3.0};

		const int nIntervalsExp = 3;
		const int orderExp = 4;
		const int dimExp = 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefsMatExp = new double[][] { {-1.0 / 2.0, 0.0, 1.5, 1.0 }, {0.0, 0.0, 0.0, 2.0 }, {-0.5, 1.5, 0.0, 2.0 } };
		double[][] coefsMatExp = new double[][]
		{
			new double[] {-1.0 / 2.0, 0.0, 1.5, 1.0},
			new double[] {0.0, 0.0, 0.0, 2.0},
			new double[] {-0.5, 1.5, 0.0, 2.0}
		};

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

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

	  /// <summary>
	  /// Intervals have different length
	  /// </summary>
	  public virtual void diffIntervalsTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 5.0, 8.0 };
		double[] xValues = new double[] {1.0, 2.0, 5.0, 8.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {2.0, 3.0, 2.0, 1.0 };
		double[] yValues = new double[] {2.0, 3.0, 2.0, 1.0};

		const int nIntervalsExp = 3;
		const int orderExp = 4;
		const int dimExp = 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefsMatExp = new double[][] { {-2.0 / 3.0, 1.0 / 3.0, 4.0 / 3.0, 2.0 }, {1.0 / 27.0, -2.0 / 9.0, 0.0, 3.0 }, {0.0, 0.0, -1.0 / 3.0, 2.0 } };
		double[][] coefsMatExp = new double[][]
		{
			new double[] {-2.0 / 3.0, 1.0 / 3.0, 4.0 / 3.0, 2.0},
			new double[] {1.0 / 27.0, -2.0 / 9.0, 0.0, 3.0},
			new double[] {0.0, 0.0, -1.0 / 3.0, 2.0}
		};

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

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

	  /// <summary>
	  /// Linear interpolation for 2 data points
	  /// </summary>
	  public virtual void LinearTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0 };
		double[] xValues = new double[] {1.0, 2.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1.0, 4.0 };
		double[] yValues = new double[] {1.0, 4.0};

		const int nIntervalsExp = 1;
		const int orderExp = 4;
		const int dimExp = 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefsMatExp = new double[][] {{0.0, 0.0, 3.0, 1.0 } };
		double[][] coefsMatExp = new double[][]
		{
			new double[] {0.0, 0.0, 3.0, 1.0}
		};

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

		assertEquals(result.Dimensions, dimExp);
		assertEquals(result.NumberOfIntervals, nIntervalsExp);
		assertEquals(result.Dimensions, dimExp);

		for (int i = 0; i < nIntervalsExp; ++i)
		{
		  for (int j = 0; j < orderExp; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = result.getCoefMatrix().get(i, j) == 0.0 ? 1.0 : Math.abs(result.getCoefMatrix().get(i, j));
			double @ref = result.CoefMatrix.get(i, j) == 0.0 ? 1.0 : Math.Abs(result.CoefMatrix.get(i, j));
			assertEquals(result.CoefMatrix.get(i, j), coefsMatExp[i][j], @ref * EPS);
		  }
		}

		for (int j = 0; j < nIntervalsExp + 1; ++j)
		{
		  assertEquals(result.Knots.get(j), xValues[j]);
		}
	  }

	  //(enabled=false)
	  public virtual void monotonicTest()
	  {
		const bool print = false; //turn to false before pushing
		if (print)
		{

		}

		PiecewiseCubicHermiteSplineInterpolator interpolator = new PiecewiseCubicHermiteSplineInterpolator();

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {0.0, 0.3, 0.6, 1.5, 2.7, 3.4, 4.8, 5.9 };
		double[] xValues = new double[] {0.0, 0.3, 0.6, 1.5, 2.7, 3.4, 4.8, 5.9};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1.0, 1.2, 1.5, 2.0, 2.1, 3.0, 3.1, 3.3 };
		double[] yValues = new double[] {1.0, 1.2, 1.5, 2.0, 2.1, 3.0, 3.1, 3.3};
		const int nPts = 300;
		double old = yValues[0] * xValues[0];
		for (int i = 0; i < nPts; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double key = 0.0 + i * 5.9 / (nPts - 1);
		  double key = 0.0 + i * 5.9 / (nPts - 1);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double value = interpolator.interpolate(xValues, yValues, key);
		  double value = interpolator.interpolate(xValues, yValues, key);
		  if (print)
		  {

		  }
		  assertTrue(value >= old);
		  old = value;
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

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

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

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void wrongDatalengthTest()
	  public virtual void wrongDatalengthTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0};
		double[] yValues = new double[] {1.0, 2.0, 3.0, 4.0};

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void shortDataLengthTest()
	  public virtual void shortDataLengthTest()
	  {
		double[] xValues = new double[] {1.0};
		double[] yValues = new double[] {4.0};

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNxValuesTest()
	  public virtual void NaNxValuesTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, Double.NaN, 4.0};
		double[] yValues = new double[] {1.0, 2.0, 3.0, 4.0};

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNyValuesTest()
	  public virtual void NaNyValuesTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
		double[] yValues = new double[] {1.0, 2.0, Double.NaN, 4.0};

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfxValuesTest()
	  public virtual void InfxValuesTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, INF};
		double[] yValues = new double[] {1.0, 2.0, 3.0, 4.0};

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfyValuesTest()
	  public virtual void InfyValuesTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
		double[] yValues = new double[] {1.0, 2.0, 3.0, INF};

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void coincideXvaluesTest()
	  public virtual void coincideXvaluesTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 3.0};
		double[] yValues = new double[] {1.0, 2.0, 3.0, 4.0};

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// <summary>
	  ///  Tests for multi-dimensions with all of the endpoint conditions
	  /// </summary>
	  public virtual void AllBvsMultiTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {1.0, 2.0, 10.0, 11.0 }, {9.0, 10.0, 1.0, 3.0 }, {2.0, 3.0, 2.0, 3.0 }, {1.0, 2.0, 2.0, 3.0 }, {2.0, 3.0, 2.0, 1.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 10.0, 11.0},
			new double[] {9.0, 10.0, 1.0, 3.0},
			new double[] {2.0, 3.0, 2.0, 3.0},
			new double[] {1.0, 2.0, 2.0, 3.0},
			new double[] {2.0, 3.0, 2.0, 1.0}
		};

		const int nIntervalsExp = 3;
		const int orderExp = 4;
		const int dimExp = 5;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefsMatExp = new double[][] { {-2.0 / 9.0, 11.0 / 9.0, 0.0, 1.0 }, {1.0, -3.0, 3.0, 9.0 }, {0.0, -1.0, 2.0, 2.0 }, {-1.0 / 2.0, 0.0, 1.5, 1.0 }, {0.0, -1.0, 2.0, 2.0 }, {-112.0 / 9.0, 56.0 / 3.0, 16.0 / 9.0, 2.0 }, {18.0, -27.0, 0.0, 10.0 }, {2.0, -3.0, 0.0, 3.0 }, {0.0, 0.0, 0.0, 2.0 }, {1.0, -2.0, 0.0, 3.0 }, {-2.0 / 9.0, -80.0 / 144.0, 16.0 / 9.0, 10.0 }, {2.0, 0.0, 0.0, 1.0 }, {0.0, 1.0, 0.0, 2.0 }, {-0.5, 1.5, 0.0, 2.0 }, {0.0, 0.0, -1.0, 2.0 } };
		double[][] coefsMatExp = new double[][]
		{
			new double[] {-2.0 / 9.0, 11.0 / 9.0, 0.0, 1.0},
			new double[] {1.0, -3.0, 3.0, 9.0},
			new double[] {0.0, -1.0, 2.0, 2.0},
			new double[] {-1.0 / 2.0, 0.0, 1.5, 1.0},
			new double[] {0.0, -1.0, 2.0, 2.0},
			new double[] {-112.0 / 9.0, 56.0 / 3.0, 16.0 / 9.0, 2.0},
			new double[] {18.0, -27.0, 0.0, 10.0},
			new double[] {2.0, -3.0, 0.0, 3.0},
			new double[] {0.0, 0.0, 0.0, 2.0},
			new double[] {1.0, -2.0, 0.0, 3.0},
			new double[] {-2.0 / 9.0, -80.0 / 144.0, 16.0 / 9.0, 10.0},
			new double[] {2.0, 0.0, 0.0, 1.0},
			new double[] {0.0, 1.0, 0.0, 2.0},
			new double[] {-0.5, 1.5, 0.0, 2.0},
			new double[] {0.0, 0.0, -1.0, 2.0}
		};

		PiecewisePolynomialInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

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

	  /// <summary>
	  /// Linear interpolation for 2 data points
	  /// </summary>
	  public virtual void LinearMultiTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0 };
		double[] xValues = new double[] {1.0, 2.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {1.0, 4.0 }, {1.0, 1.0 / 3.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 4.0},
			new double[] {1.0, 1.0 / 3.0}
		};

		const int nIntervalsExp = 1;
		const int orderExp = 4;
		const int dimExp = 2;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefsMatExp = new double[][] { {0.0, 0.0, 3.0, 1.0 }, {0.0, 0.0, -2.0 / 3.0, 1.0 } };
		double[][] coefsMatExp = new double[][]
		{
			new double[] {0.0, 0.0, 3.0, 1.0},
			new double[] {0.0, 0.0, -2.0 / 3.0, 1.0}
		};

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

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

	  /// <summary>
	  /// Intervals have different length
	  /// </summary>
	  public virtual void diffIntervalsMultiTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 5.0, 8.0 };
		double[] xValues = new double[] {1.0, 2.0, 5.0, 8.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {2.0, 3.0, 2.0, 1.0 }, {-1.0, 3.0, 6.0, 7.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {2.0, 3.0, 2.0, 1.0},
			new double[] {-1.0, 3.0, 6.0, 7.0}
		};

		const int nIntervalsExp = 3;
		const int orderExp = 4;
		const int dimExp = 2;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefsMatExp = new double[][] { {-2.0 / 3.0, 1.0 / 3.0, 4.0 / 3.0, 2.0 }, {-53.0 / 36, 13.0 / 18.0, 19.0 / 4.0, -1.0 }, {1.0 / 27.0, -2.0 / 9.0, 0.0, 3.0 }, {5.0 / 162.0, -19.0 / 54.0, 16.0 / 9.0, 3.0 }, {0.0, 0.0, -1.0 / 3.0, 2.0 }, {-1.0 / 54.0, 0.0, 1.0 / 2.0, 6.0 } };
		double[][] coefsMatExp = new double[][]
		{
			new double[] {-2.0 / 3.0, 1.0 / 3.0, 4.0 / 3.0, 2.0},
			new double[] {-53.0 / 36, 13.0 / 18.0, 19.0 / 4.0, -1.0},
			new double[] {1.0 / 27.0, -2.0 / 9.0, 0.0, 3.0},
			new double[] {5.0 / 162.0, -19.0 / 54.0, 16.0 / 9.0, 3.0},
			new double[] {0.0, 0.0, -1.0 / 3.0, 2.0},
			new double[] {-1.0 / 54.0, 0.0, 1.0 / 2.0, 6.0}
		};

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

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

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

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

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

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

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

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

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

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

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

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

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

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

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

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

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

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

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

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
//ORIGINAL LINE: final double[][] yValues = new double[][] { {2.0, 3.0, 2.0, 1.0 }, {1.0, 2.0, 10.0, 11.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {2.0, 3.0, 2.0, 1.0},
			new double[] {1.0, 2.0, 10.0, 11.0}
		};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] xKey = new double[][] { {-1.0, 0.5, 1.5 }, {2.5, 3.5, 4.5 } };
		double[][] xKey = new double[][]
		{
			new double[] {-1.0, 0.5, 1.5},
			new double[] {2.5, 3.5, 4.5}
		};

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][][] resultValuesExpected = new double[][][] { { {-6.0, 21.0 / 8.0 }, {23.0 / 3.0, 6.0 } }, { {3.0 / 4.0, 3.0 / 2.0 }, {4.0 / 3.0, 193.0 / 18.0 } }, { {11.0 / 4.0, 1.0 / 2.0 }, {23.0 / 18.0, 32.0 / 3.0 } } };
		double[][][] resultValuesExpected = new double[][][]
		{
			new double[][]
			{
				new double[] {-6.0, 21.0 / 8.0},
				new double[] {23.0 / 3.0, 6.0}
			},
			new double[][]
			{
				new double[] {3.0 / 4.0, 3.0 / 2.0},
				new double[] {4.0 / 3.0, 193.0 / 18.0}
			},
			new double[][]
			{
				new double[] {11.0 / 4.0, 1.0 / 2.0},
				new double[] {23.0 / 18.0, 32.0 / 3.0}
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

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

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
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NullKeyTest()
	  public virtual void NullKeyTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0};
		double[] yValues = new double[] {1.0, 3.0, 4.0};
		double[] xKey = new double[3];

		xKey = null;

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

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

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

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

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

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

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

		interp.interpolate(xValues, yValues, xKey);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfiniteOutputTest()
	  public virtual void InfiniteOutputTest()
	  {
		double[] xValues = new double[] {1.e-308, 2.e-308};
		double[] yValues = new double[] {1.0, 1.e308};

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

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

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

		interp.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NanOutputTest()
	  public virtual void NanOutputTest()
	  {
		double[] xValues = new double[] {1.0, 2.e-308, 3.e-308, 4.0};
		double[] yValues = new double[] {1.0, 2.0, 1.e308, 3.0};

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

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

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

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

		PiecewiseCubicHermiteSplineInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();

		interp.interpolate(xValues, yValues[0], 1.e308);
	  }

	}

}