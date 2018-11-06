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

	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using PiecewisePolynomialFunction1D = com.opengamma.strata.math.impl.function.PiecewisePolynomialFunction1D;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NonnegativityPreservingCubicSplineInterpolatorTest
	public class NonnegativityPreservingCubicSplineInterpolatorTest
	{

	  private const double EPS = 1e-14;
	  private const double INF = 1.0 / 0.0;

	  /// 
	  public virtual void positivityClampedTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0, 5.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0, 5.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {0.0, 0.1, 1.0, 1.0, 20.0, 5.0, 0.0 };
		double[] yValues = new double[] {0.0, 0.1, 1.0, 1.0, 20.0, 5.0, 0.0};

		PiecewisePolynomialInterpolator interp = new CubicSplineInterpolator();
		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		PiecewisePolynomialResult resultPos = interpPos.interpolate(xValues, yValues);

		assertEquals(resultPos.Dimensions, result.Dimensions);
		assertEquals(resultPos.NumberOfIntervals, result.NumberOfIntervals);
		assertEquals(resultPos.Order, result.Order);

		const int nPts = 101;
		for (int i = 0; i < 101; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double key = 1.0 + 4.0 / (nPts - 1) * i;
		  double key = 1.0 + 4.0 / (nPts - 1) * i;
		  assertTrue(function.evaluate(resultPos, key).get(0) >= 0.0);
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData = xValues.length;
		int nData = xValues.Length;
		for (int i = 1; i < nData - 2; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double tau = Math.signum(resultPos.getCoefMatrix().get(i, 3));
		  double tau = Math.Sign(resultPos.CoefMatrix.get(i, 3));
		  assertTrue(resultPos.CoefMatrix.get(i, 2) * tau >= -3.0 * yValues[i + 1] * tau / (xValues[i + 1] - xValues[i]));
		  assertTrue(resultPos.CoefMatrix.get(i, 2) * tau <= 3.0 * yValues[i + 1] * tau / (xValues[i] - xValues[i - 1]));
		}
	  }

	  /// 
	  public virtual void positivityClampedMultiTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0, 5.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0, 5.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {0.0, 0.1, 1.0, 1.0, 20.0, 5.0, 0.0 }, {-10.0, 0.1, 1.0, 1.0, 20.0, 5.0, 0.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {0.0, 0.1, 1.0, 1.0, 20.0, 5.0, 0.0},
			new double[] {-10.0, 0.1, 1.0, 1.0, 20.0, 5.0, 0.0}
		};

		PiecewisePolynomialInterpolator interp = new CubicSplineInterpolator();
		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		PiecewisePolynomialResult resultPos = interpPos.interpolate(xValues, yValues);

		assertEquals(resultPos.Dimensions, result.Dimensions);
		assertEquals(resultPos.NumberOfIntervals, result.NumberOfIntervals);
		assertEquals(resultPos.Order, result.Order);

		const int nPts = 101;
		for (int i = 0; i < 101; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double key = 1.0 + 4.0 / (nPts - 1) * i;
		  double key = 1.0 + 4.0 / (nPts - 1) * i;
		  assertTrue(function.evaluate(resultPos, key).get(0) >= 0.0);
		}

		int dim = yValues.Length;
		int nData = xValues.Length;
		for (int j = 0; j < dim; ++j)
		{
		  for (int i = 1; i < nData - 2; ++i)
		  {
			DoubleMatrix coefMatrix = resultPos.CoefMatrix;
			double tau = Math.Sign(coefMatrix.get(dim * i + j, 3));
			assertTrue(coefMatrix.get(dim * i + j, 2) * tau >= -3.0 * yValues[j][i + 1] * tau / (xValues[i + 1] - xValues[i]));
			assertTrue(coefMatrix.get(dim * i + j, 2) * tau <= 3.0 * yValues[j][i + 1] * tau / (xValues[i] - xValues[i - 1]));
		  }
		}
	  }

	  /// 
	  public virtual void positivityNotAKnotTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0, 5.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0, 5.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {0.1, 1.0, 1.0, 20.0, 5.0 };
		double[] yValues = new double[] {0.1, 1.0, 1.0, 20.0, 5.0};

		PiecewisePolynomialInterpolator interp = new CubicSplineInterpolator();
		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		PiecewisePolynomialResult resultPos = interpPos.interpolate(xValues, yValues);

		assertEquals(resultPos.Dimensions, result.Dimensions);
		assertEquals(resultPos.NumberOfIntervals, result.NumberOfIntervals);
		assertEquals(resultPos.Order, result.Order);

		const int nPts = 101;
		for (int i = 0; i < 101; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double key = 1.0 + 4.0 / (nPts - 1) * i;
		  double key = 1.0 + 4.0 / (nPts - 1) * i;
		  assertTrue(function.evaluate(resultPos, key).get(0) >= 0.0);
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData = xValues.length;
		int nData = xValues.Length;
		for (int i = 1; i < nData - 2; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double tau = Math.signum(resultPos.getCoefMatrix().get(i, 3));
		  double tau = Math.Sign(resultPos.CoefMatrix.get(i, 3));
		  assertTrue(resultPos.CoefMatrix.get(i, 2) * tau >= -3.0 * yValues[i] * tau / (xValues[i + 1] - xValues[i]));
		  assertTrue(resultPos.CoefMatrix.get(i, 2) * tau <= 3.0 * yValues[i] * tau / (xValues[i] - xValues[i - 1]));
		}
	  }

	  /// 
	  public virtual void positivityEndIntervalsTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0, 5.0, 6.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0, 5.0, 6.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {0.01, 0.01, 0.01, 10.0, 20.0, 1.0 }, {0.01, 0.01, 10.0, 10.0, 0.01, 0.01 } };
		double[][] yValues = new double[][]
		{
			new double[] {0.01, 0.01, 0.01, 10.0, 20.0, 1.0},
			new double[] {0.01, 0.01, 10.0, 10.0, 0.01, 0.01}
		};

		PiecewisePolynomialInterpolator interp = new NaturalSplineInterpolator();
		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		PiecewisePolynomialResult resultPos = interpPos.interpolate(xValues, yValues);

		assertEquals(resultPos.Dimensions, result.Dimensions);
		assertEquals(resultPos.NumberOfIntervals, result.NumberOfIntervals);
		assertEquals(resultPos.Order, result.Order);

		const int nPts = 101;
		for (int i = 0; i < 101; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double key = 1.0 + 5.0 / (nPts - 1) * i;
		  double key = 1.0 + 5.0 / (nPts - 1) * i;
		  assertTrue(function.evaluate(resultPos, key).get(0) >= 0.0);
		}

		int dim = yValues.Length;
		int nData = xValues.Length;
		for (int j = 0; j < dim; ++j)
		{
		  for (int i = 1; i < nData - 2; ++i)
		  {
			DoubleMatrix coefMatrix = resultPos.CoefMatrix;
			double tau = Math.Sign(coefMatrix.get(dim * i + j, 3));
			assertTrue(coefMatrix.get(dim * i + j, 2) * tau >= -3.0 * yValues[j][i] * tau / (xValues[i + 1] - xValues[i]));
			assertTrue(coefMatrix.get(dim * i + j, 2) * tau <= 3.0 * yValues[j][i] * tau / (xValues[i] - xValues[i - 1]));
		  }
		}
	  }

	  /// <summary>
	  /// PiecewiseCubicHermiteSplineInterpolator is not modified for positive data
	  /// </summary>
	  public virtual void noModificationTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0, 5.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0, 5.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {0.1, 1.0, 1.0, 20.0, 5.0 }, {1.0, 2.0, 3.0, 0.0, 0.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {0.1, 1.0, 1.0, 20.0, 5.0},
			new double[] {1.0, 2.0, 3.0, 0.0, 0.0}
		};

		PiecewisePolynomialInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();
		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		PiecewisePolynomialResult resultPos = interpPos.interpolate(xValues, yValues);

		assertEquals(resultPos.Dimensions, result.Dimensions);
		assertEquals(resultPos.NumberOfIntervals, result.NumberOfIntervals);
		assertEquals(resultPos.Order, result.Order);

		for (int i = 1; i < xValues.Length - 1; ++i)
		{
		  for (int j = 0; j < 4; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = result.getCoefMatrix().get(i, j) == 0.0 ? 1.0 : Math.abs(result.getCoefMatrix().get(i, j));
			double @ref = result.CoefMatrix.get(i, j) == 0.0 ? 1.0 : Math.Abs(result.CoefMatrix.get(i, j));
			assertEquals(resultPos.CoefMatrix.get(i, j), result.CoefMatrix.get(i, j), @ref * EPS);
		  }
		}
	  }

	  /// 
	  public virtual void flipTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0, 5.0, 6.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0, 5.0, 6.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {3.0, 0.1, 0.01, 0.01, 0.1, 3.0 };
		double[] yValues = new double[] {3.0, 0.1, 0.01, 0.01, 0.1, 3.0};

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValuesFlip = new double[] {6.0, 2.0, 3.0, 5.0, 4.0, 1.0 };
		double[] xValuesFlip = new double[] {6.0, 2.0, 3.0, 5.0, 4.0, 1.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValuesFlip = new double[] {3.0, 0.1, 0.01, 0.1, 0.01, 3.0 };
		double[] yValuesFlip = new double[] {3.0, 0.1, 0.01, 0.1, 0.01, 3.0};

		PiecewisePolynomialInterpolator interp = new NaturalSplineInterpolator();

		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		PiecewisePolynomialResult resultPos = interpPos.interpolate(xValues, yValues);
		PiecewisePolynomialResult resultPosFlip = interpPos.interpolate(xValuesFlip, yValuesFlip);

		assertEquals(resultPos.Dimensions, resultPosFlip.Dimensions);
		assertEquals(resultPos.NumberOfIntervals, resultPosFlip.NumberOfIntervals);
		assertEquals(resultPos.Order, resultPosFlip.Order);

		const int nPts = 101;
		for (int i = 0; i < 101; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double key = 1.0 + 5.0 / (nPts - 1) * i;
		  double key = 1.0 + 5.0 / (nPts - 1) * i;
		  assertTrue(function.evaluate(resultPos, key).get(0) >= 0.0);
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData = xValues.length;
		int nData = xValues.Length;
		for (int i = 0; i < nData - 1; ++i)
		{
		  for (int k = 0; k < 4; ++k)
		  {
			assertEquals(resultPos.CoefMatrix.get(i, k), resultPosFlip.CoefMatrix.get(i, k));
		  }
		}
	  }

	  /// 
	  public virtual void flipMultiTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0, 5.0, 6.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0, 5.0, 6.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {3.0, 0.1, 0.01, 0.01, 0.1, 3.0 }, {3.0, 0.1, 0.01, 0.001, 2.0, 3.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {3.0, 0.1, 0.01, 0.01, 0.1, 3.0},
			new double[] {3.0, 0.1, 0.01, 0.001, 2.0, 3.0}
		};

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValuesFlip = new double[] {1.0, 2.0, 3.0, 5.0, 4.0, 6.0 };
		double[] xValuesFlip = new double[] {1.0, 2.0, 3.0, 5.0, 4.0, 6.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValuesFlip = new double[][] { {3.0, 0.1, 0.01, 0.1, 0.01, 3.0 }, {3.0, 0.1, 0.01, 2.0, 0.001, 3.0 } };
		double[][] yValuesFlip = new double[][]
		{
			new double[] {3.0, 0.1, 0.01, 0.1, 0.01, 3.0},
			new double[] {3.0, 0.1, 0.01, 2.0, 0.001, 3.0}
		};

		PiecewisePolynomialInterpolator interp = new NaturalSplineInterpolator();

		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		PiecewisePolynomialResult resultPos = interpPos.interpolate(xValues, yValues);
		PiecewisePolynomialResult resultPosFlip = interpPos.interpolate(xValuesFlip, yValuesFlip);

		assertEquals(resultPos.Dimensions, resultPosFlip.Dimensions);
		assertEquals(resultPos.NumberOfIntervals, resultPosFlip.NumberOfIntervals);
		assertEquals(resultPos.Order, resultPosFlip.Order);

		const int nPts = 101;
		for (int i = 0; i < 101; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double key = 1.0 + 5.0 / (nPts - 1) * i;
		  double key = 1.0 + 5.0 / (nPts - 1) * i;
		  assertTrue(function.evaluate(resultPos, key).get(0) >= 0.0);
		  assertTrue(function.evaluate(resultPos, key).get(1) >= 0.0);
		}

		int dim = yValues.Length;
		int nData = xValues.Length;
		for (int j = 0; j < dim; ++j)
		{
		  for (int i = 0; i < nData - 1; ++i)
		  {
			for (int k = 0; k < 4; ++k)
			{
			  assertEquals(resultPos.CoefMatrix.get(dim * i + j, k), resultPosFlip.CoefMatrix.get(dim * i + j, k));
			}
		  }
		}
	  }

	  /*
	   * Error tests
	   */
	  /// <summary>
	  /// Primary interpolation method should be cubic. 
	  /// Note that CubicSplineInterpolator returns a linear or quadratic function in certain situations 
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void lowDegreeTest()
	  public virtual void lowDegreeTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {0.0, 0.1, 0.05 };
		double[] yValues = new double[] {0.0, 0.1, 0.05};

		PiecewisePolynomialInterpolator interp = new CubicSplineInterpolator();
		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void lowDegreeMultiTest()
	  public virtual void lowDegreeMultiTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {0.0, 0.1, 0.05 }, {0.0, 0.1, 1.05 } };
		double[][] yValues = new double[][]
		{
			new double[] {0.0, 0.1, 0.05},
			new double[] {0.0, 0.1, 1.05}
		};

		PiecewisePolynomialInterpolator interp = new LinearInterpolator();
		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void dataShortTest()
	  public virtual void dataShortTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0 };
		double[] xValues = new double[] {1.0, 2.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {0.0, 0.1 };
		double[] yValues = new double[] {0.0, 0.1};

		PiecewisePolynomialInterpolator interp = new CubicSplineInterpolator();
		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void dataShortMultiTest()
	  public virtual void dataShortMultiTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0};
		double[] xValues = new double[] {1.0, 2.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {0.0, 0.1 }, {0.0, 0.1 } };
		double[][] yValues = new double[][]
		{
			new double[] {0.0, 0.1},
			new double[] {0.0, 0.1}
		};

		PiecewisePolynomialInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();
		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void coincideDataTest()
	  public virtual void coincideDataTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 1.0, 3.0 };
		double[] xValues = new double[] {1.0, 1.0, 3.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {0.0, 0.1, 0.05 };
		double[] yValues = new double[] {0.0, 0.1, 0.05};

		PiecewisePolynomialInterpolator interp = new CubicSplineInterpolator();
		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void coincideDataMultiTest()
	  public virtual void coincideDataMultiTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 2.0 };
		double[] xValues = new double[] {1.0, 2.0, 2.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {2.0, 0.0, 0.1, 0.05, 2.0 }, {1.0, 0.0, 0.1, 1.05, 2.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {2.0, 0.0, 0.1, 0.05, 2.0},
			new double[] {1.0, 0.0, 0.1, 1.05, 2.0}
		};

		PiecewisePolynomialInterpolator interp = new CubicSplineInterpolator();
		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void diffDataTest()
	  public virtual void diffDataTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {0.0, 0.1, 0.05 };
		double[] yValues = new double[] {0.0, 0.1, 0.05};

		PiecewisePolynomialInterpolator interp = new NaturalSplineInterpolator();
		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void diffDataMultiTest()
	  public virtual void diffDataMultiTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {2.0, 0.0, 0.1, 0.05, 2.0 }, {1.0, 0.0, 0.1, 1.05, 2.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {2.0, 0.0, 0.1, 0.05, 2.0},
			new double[] {1.0, 0.0, 0.1, 1.05, 2.0}
		};

		PiecewisePolynomialInterpolator interp = new NaturalSplineInterpolator();
		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullXdataTest()
	  public virtual void nullXdataTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
		double[] yValues = new double[] {0.0, 0.1, 0.05, 0.2};
		xValues = null;

		PiecewisePolynomialInterpolator interp = new CubicSplineInterpolator();
		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullYdataTest()
	  public virtual void nullYdataTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
		double[] yValues = new double[] {0.0, 0.1, 0.05, 0.2};
		yValues = null;

		PiecewisePolynomialInterpolator interp = new CubicSplineInterpolator();
		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullXdataMultiTest()
	  public virtual void nullXdataMultiTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
		double[][] yValues = new double[][]
		{
			new double[] {0.0, 0.1, 0.05, 0.2},
			new double[] {0.0, 0.1, 0.05, 0.2}
		};
		xValues = null;

		PiecewisePolynomialInterpolator interp = new CubicSplineInterpolator();
		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullYdataMultiTest()
	  public virtual void nullYdataMultiTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
		double[][] yValues = new double[][]
		{
			new double[] {0.0, 0.1, 0.05, 0.2},
			new double[] {0.0, 0.1, 0.05, 0.2}
		};
		yValues = null;

		PiecewisePolynomialInterpolator interp = new CubicSplineInterpolator();
		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void infXdataTest()
	  public virtual void infXdataTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, INF};
		double[] yValues = new double[] {0.0, 0.1, 0.05, 0.2};

		PiecewisePolynomialInterpolator interp = new CubicSplineInterpolator();
		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void infYdataTest()
	  public virtual void infYdataTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
		double[] yValues = new double[] {0.0, 0.0, 0.1, 0.05, 0.2, INF};

		PiecewisePolynomialInterpolator interp = new CubicSplineInterpolator();
		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nanXdataTest()
	  public virtual void nanXdataTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, Double.NaN};
		double[] yValues = new double[] {0.0, 0.1, 0.05, 0.2};

		PiecewisePolynomialInterpolator interp = new CubicSplineInterpolator();
		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nanYdataTest()
	  public virtual void nanYdataTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
		double[] yValues = new double[] {0.0, 0.0, 0.1, 0.05, 0.2, Double.NaN};

		PiecewisePolynomialInterpolator interp = new CubicSplineInterpolator();
		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void infXdataMultiTest()
	  public virtual void infXdataMultiTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, INF};
		double[][] yValues = new double[][]
		{
			new double[] {0.0, 0.1, 0.05, 0.2},
			new double[] {0.0, 0.1, 0.05, 0.2}
		};

		PiecewisePolynomialInterpolator interp = new CubicSplineInterpolator();
		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void infYdataMultiTest()
	  public virtual void infYdataMultiTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
		double[][] yValues = new double[][]
		{
			new double[] {0.0, 0.0, 0.1, 0.05, 0.2, 1.0},
			new double[] {0.0, 0.0, 0.1, 0.05, 0.2, INF}
		};

		PiecewisePolynomialInterpolator interp = new CubicSplineInterpolator();
		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nanXdataMultiTest()
	  public virtual void nanXdataMultiTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, Double.NaN};
		double[][] yValues = new double[][]
		{
			new double[] {0.0, 0.1, 0.05, 0.2},
			new double[] {0.0, 0.1, 0.05, 0.2}
		};

		PiecewisePolynomialInterpolator interp = new CubicSplineInterpolator();
		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nanYdataMultiTest()
	  public virtual void nanYdataMultiTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
		double[][] yValues = new double[][]
		{
			new double[] {0.0, 0.0, 0.1, 0.05, 0.2, 1.1},
			new double[] {0.0, 0.0, 0.1, 0.05, 0.2, Double.NaN}
		};

		PiecewisePolynomialInterpolator interp = new CubicSplineInterpolator();
		PiecewisePolynomialInterpolator interpPos = new NonnegativityPreservingCubicSplineInterpolator(interp);
		interpPos.interpolate(xValues, yValues);
	  }

	}

}