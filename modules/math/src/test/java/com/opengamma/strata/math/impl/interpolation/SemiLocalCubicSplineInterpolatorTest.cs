﻿using System;

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

	using PiecewisePolynomialFunction1D = com.opengamma.strata.math.impl.function.PiecewisePolynomialFunction1D;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SemiLocalCubicSplineInterpolatorTest
	public class SemiLocalCubicSplineInterpolatorTest
	{

	  private const double EPS = 1e-13;
	  private const double INF = 1.0 / 0.0;

	  /// <summary>
	  /// Recovering linear test
	  /// </summary>
	  public virtual void linearTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0, 5.0, 6.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0, 5.0, 6.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData = xValues.length;
		int nData = xValues.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[nData];
		double[] yValues = new double[nData];
		for (int i = 0; i < nData; ++i)
		{
		  yValues[i] = xValues[i] / 7.0 + 1 / 11.0;
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefsMatExp = new double[][] { {0.0, 0.0, 1.0 / 7.0, yValues[0] }, {0.0, 0.0, 1.0 / 7.0, yValues[1] }, {0.0, 0.0, 1.0 / 7.0, yValues[2] }, {0.0, 0.0, 1.0 / 7.0, yValues[3] }, {0.0, 0.0, 1.0 / 7.0, yValues[4] } };
		double[][] coefsMatExp = new double[][]
		{
			new double[] {0.0, 0.0, 1.0 / 7.0, yValues[0]},
			new double[] {0.0, 0.0, 1.0 / 7.0, yValues[1]},
			new double[] {0.0, 0.0, 1.0 / 7.0, yValues[2]},
			new double[] {0.0, 0.0, 1.0 / 7.0, yValues[3]},
			new double[] {0.0, 0.0, 1.0 / 7.0, yValues[4]}
		};

		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		PiecewisePolynomialInterpolator interp = new SemiLocalCubicSplineInterpolator();
		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

		assertEquals(result.Dimensions, 1);
		assertEquals(result.NumberOfIntervals, 5);
		assertEquals(result.Order, 4);

		for (int i = 0; i < result.NumberOfIntervals; ++i)
		{
		  for (int j = 0; j < result.Order; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(coefsMatExp[i][j]) == 0.0 ? 1.0 : Math.abs(coefsMatExp[i][j]);
			double @ref = Math.Abs(coefsMatExp[i][j]) == 0.0 ? 1.0 : Math.Abs(coefsMatExp[i][j]);
			assertEquals(result.CoefMatrix.get(i, j), coefsMatExp[i][j], @ref * EPS);
		  }
		}

		const int nKeys = 101;
		for (int i = 0; i < nKeys; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double key = 1.0 + 5.0 / (nKeys - 1) * i;
		  double key = 1.0 + 5.0 / (nKeys - 1) * i;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = key / 7.0 + 1 / 11.0;
		  double @ref = key / 7.0 + 1 / 11.0;
		  assertEquals(function.evaluate(result, key).get(0), @ref, @ref * EPS);
		}
	  }

	  /// <summary>
	  /// Recovering quadratic function
	  /// </summary>
	  public virtual void quadraticTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0, 5.0, 6.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0, 5.0, 6.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData = xValues.length;
		int nData = xValues.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[nData];
		double[] yValues = new double[nData];
		for (int i = 0; i < nData; ++i)
		{
		  yValues[i] = xValues[i] * xValues[i] / 7.0 + xValues[i] / 13.0 + 1 / 11.0;
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefsMatExp = new double[][] { {0.0, 1.0 / 7.0, 2.0 / 7.0 * xValues[0] + 1.0 / 13.0, yValues[0] }, {0.0, 1.0 / 7.0, 2.0 / 7.0 * xValues[1] + 1.0 / 13.0, yValues[1] }, {0.0, 1.0 / 7.0, 2.0 / 7.0 * xValues[2] + 1.0 / 13.0, yValues[2] }, {0.0, 1.0 / 7.0, 2.0 / 7.0 * xValues[3] + 1.0 / 13.0, yValues[3] }, {0.0, 1.0 / 7.0, 2.0 / 7.0 * xValues[4] + 1.0 / 13.0, yValues[4] } };
		double[][] coefsMatExp = new double[][]
		{
			new double[] {0.0, 1.0 / 7.0, 2.0 / 7.0 * xValues[0] + 1.0 / 13.0, yValues[0]},
			new double[] {0.0, 1.0 / 7.0, 2.0 / 7.0 * xValues[1] + 1.0 / 13.0, yValues[1]},
			new double[] {0.0, 1.0 / 7.0, 2.0 / 7.0 * xValues[2] + 1.0 / 13.0, yValues[2]},
			new double[] {0.0, 1.0 / 7.0, 2.0 / 7.0 * xValues[3] + 1.0 / 13.0, yValues[3]},
			new double[] {0.0, 1.0 / 7.0, 2.0 / 7.0 * xValues[4] + 1.0 / 13.0, yValues[4]}
		};

		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		PiecewisePolynomialInterpolator interp = new SemiLocalCubicSplineInterpolator();
		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

		assertEquals(result.Dimensions, 1);
		assertEquals(result.NumberOfIntervals, 5);
		assertEquals(result.Order, 4);

		for (int i = 0; i < result.NumberOfIntervals; ++i)
		{
		  for (int j = 0; j < result.Order; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(coefsMatExp[i][j]) == 0.0 ? 1.0 : Math.abs(coefsMatExp[i][j]);
			double @ref = Math.Abs(coefsMatExp[i][j]) == 0.0 ? 1.0 : Math.Abs(coefsMatExp[i][j]);
			assertEquals(result.CoefMatrix.get(i, j), coefsMatExp[i][j], @ref * EPS);
		  }
		}

		const int nKeys = 101;
		for (int i = 0; i < nKeys; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double key = 1.0 + 5.0 / (nKeys - 1) * i;
		  double key = 1.0 + 5.0 / (nKeys - 1) * i;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = key * key / 7.0 + key / 13.0 + 1 / 11.0;
		  double @ref = key * key / 7.0 + key / 13.0 + 1 / 11.0;
		  assertEquals(function.evaluate(result, key).get(0), @ref, @ref * EPS);

		}
	  }

	  /// 
	  public virtual void quadraticMultiTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0, 5.0, 6.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0, 5.0, 6.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData = xValues.length;
		int nData = xValues.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[2][nData];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] yValues = new double[2][nData];
		double[][] yValues = RectangularArrays.ReturnRectangularDoubleArray(2, nData);
		for (int i = 0; i < nData; ++i)
		{
		  yValues[0][i] = xValues[i] * xValues[i] / 7.0 + xValues[i] / 13.0 + 1 / 11.0;
		}
		for (int i = 0; i < nData; ++i)
		{
		  yValues[1][i] = xValues[i] * xValues[i] / 3.0 + xValues[i] / 7.0 + 1 / 17.0;
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefsMatExp = new double[][] { {0.0, 1.0 / 7.0, 2.0 / 7.0 * xValues[0] + 1.0 / 13.0, yValues[0][0] }, {0.0, 1.0 / 3.0, 2.0 / 3.0 * xValues[0] + 1.0 / 7.0, yValues[1][0] }, {0.0, 1.0 / 7.0, 2.0 / 7.0 * xValues[1] + 1.0 / 13.0, yValues[0][1] }, {0.0, 1.0 / 3.0, 2.0 / 3.0 * xValues[1] + 1.0 / 7.0, yValues[1][1] }, {0.0, 1.0 / 7.0, 2.0 / 7.0 * xValues[2] + 1.0 / 13.0, yValues[0][2] }, {0.0, 1.0 / 3.0, 2.0 / 3.0 * xValues[2] + 1.0 / 7.0, yValues[1][2] }, {0.0, 1.0 / 7.0, 2.0 / 7.0 * xValues[3] + 1.0 / 13.0, yValues[0][3] }, {0.0, 1.0 / 3.0, 2.0 / 3.0 * xValues[3] + 1.0 / 7.0, yValues[1][3] }, {0.0, 1.0 / 7.0, 2.0 / 7.0 * xValues[4] + 1.0 / 13.0, yValues[0][4] }, {0.0, 1.0 / 3.0, 2.0 / 3.0 * xValues[4] + 1.0 / 7.0, yValues[1][4] } };
		double[][] coefsMatExp = new double[][]
		{
			new double[] {0.0, 1.0 / 7.0, 2.0 / 7.0 * xValues[0] + 1.0 / 13.0, yValues[0][0]},
			new double[] {0.0, 1.0 / 3.0, 2.0 / 3.0 * xValues[0] + 1.0 / 7.0, yValues[1][0]},
			new double[] {0.0, 1.0 / 7.0, 2.0 / 7.0 * xValues[1] + 1.0 / 13.0, yValues[0][1]},
			new double[] {0.0, 1.0 / 3.0, 2.0 / 3.0 * xValues[1] + 1.0 / 7.0, yValues[1][1]},
			new double[] {0.0, 1.0 / 7.0, 2.0 / 7.0 * xValues[2] + 1.0 / 13.0, yValues[0][2]},
			new double[] {0.0, 1.0 / 3.0, 2.0 / 3.0 * xValues[2] + 1.0 / 7.0, yValues[1][2]},
			new double[] {0.0, 1.0 / 7.0, 2.0 / 7.0 * xValues[3] + 1.0 / 13.0, yValues[0][3]},
			new double[] {0.0, 1.0 / 3.0, 2.0 / 3.0 * xValues[3] + 1.0 / 7.0, yValues[1][3]},
			new double[] {0.0, 1.0 / 7.0, 2.0 / 7.0 * xValues[4] + 1.0 / 13.0, yValues[0][4]},
			new double[] {0.0, 1.0 / 3.0, 2.0 / 3.0 * xValues[4] + 1.0 / 7.0, yValues[1][4]}
		};

		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		PiecewisePolynomialInterpolator interp = new SemiLocalCubicSplineInterpolator();
		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

		assertEquals(result.Dimensions, 2);
		assertEquals(result.NumberOfIntervals, 5);
		assertEquals(result.Order, 4);

		for (int i = 0; i < result.NumberOfIntervals * 2; ++i)
		{
		  for (int j = 0; j < result.Order; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(coefsMatExp[i][j]) == 0.0 ? 1.0 : Math.abs(coefsMatExp[i][j]);
			double @ref = Math.Abs(coefsMatExp[i][j]) == 0.0 ? 1.0 : Math.Abs(coefsMatExp[i][j]);
			assertEquals(result.CoefMatrix.get(i, j), coefsMatExp[i][j], @ref * EPS);
		  }
		}

		const int nKeys = 101;
		for (int i = 0; i < nKeys; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double key = 1.0 + 5.0 / (nKeys - 1) * i;
		  double key = 1.0 + 5.0 / (nKeys - 1) * i;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = key * key / 7.0 + key / 13.0 + 1 / 11.0;
		  double @ref = key * key / 7.0 + key / 13.0 + 1 / 11.0;
		  assertEquals(function.evaluate(result, key).get(0), @ref, @ref * EPS);

		}
	  }

	  /// <summary>
	  /// Sample data given in the original paper, consisting of constant part and monotonically increasing part
	  /// </summary>
	  public virtual void sampleDataTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {0.0, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0 };
		double[] xValues = new double[] {0.0, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.5, 15.0, 50.0, 60.0, 85.0 };
		double[] yValues = new double[] {10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.5, 15.0, 50.0, 60.0, 85.0};

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefsMatPartExp = new double[][] { {0.0, 0.0, 0.0, 10.0 }, {0.0, 0.0, 0.0, 10.0 }, {0.0, 0.0, 0.0, 10.0 }, {0.0, 0.0, 0.0, 10.0 }, {0.0, 0.0, 0.0, 10.0 } };
		double[][] coefsMatPartExp = new double[][]
		{
			new double[] {0.0, 0.0, 0.0, 10.0},
			new double[] {0.0, 0.0, 0.0, 10.0},
			new double[] {0.0, 0.0, 0.0, 10.0},
			new double[] {0.0, 0.0, 0.0, 10.0},
			new double[] {0.0, 0.0, 0.0, 10.0}
		};

		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		PiecewisePolynomialInterpolator interp = new SemiLocalCubicSplineInterpolator();
		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

		assertEquals(result.Dimensions, 1);
		assertEquals(result.NumberOfIntervals, 10);
		assertEquals(result.Order, 4);

		for (int i = 0; i < 5; ++i)
		{
		  for (int j = 0; j < 4; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(coefsMatPartExp[i][j]) == 0.0 ? 1.0 : Math.abs(coefsMatPartExp[i][j]);
			double @ref = Math.Abs(coefsMatPartExp[i][j]) == 0.0 ? 1.0 : Math.Abs(coefsMatPartExp[i][j]);
			assertEquals(result.CoefMatrix.get(i, j), coefsMatPartExp[i][j], @ref * EPS);
		  }
		}

		const int nKeys = 101;
		double key0 = 5.0;
		for (int i = 1; i < nKeys; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double key = 5.0 + 5.0 / (nKeys - 1) * i;
		  double key = 5.0 + 5.0 / (nKeys - 1) * i;
		  assertTrue(function.evaluate(result, key).get(0) - function.evaluate(result, key0).get(0) >= 0.0);
		  key0 = 5.0 + 5.0 / (nKeys - 1) * i;
		}

		key0 = 0.0;
		for (int i = 1; i < nKeys; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double key = 0.0 + 5.0 / (nKeys - 1) * i;
		  double key = 0.0 + 5.0 / (nKeys - 1) * i;
		  assertTrue(function.evaluate(result, key).get(0) - function.evaluate(result, key0).get(0) == 0.0);
		  key0 = 0.0 + 5.0 / (nKeys - 1) * i;
		}

	  }

	  /*
	   * Error tests
	   */

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

		PiecewisePolynomialInterpolator interpPos = new SemiLocalCubicSplineInterpolator();
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
//ORIGINAL LINE: final double[][] yValues = new double[][] { {0.0, 0.1}, {0.0, 0.1} };
		double[][] yValues = new double[][]
		{
			new double[] {0.0, 0.1},
			new double[] {0.0, 0.1}
		};

		PiecewisePolynomialInterpolator interpPos = new SemiLocalCubicSplineInterpolator();
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void dataDiffTest()
	  public virtual void dataDiffTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {0.0, 0.1, 3.0 };
		double[] yValues = new double[] {0.0, 0.1, 3.0};

		PiecewisePolynomialInterpolator interpPos = new SemiLocalCubicSplineInterpolator();
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void dataDiffMultiTest()
	  public virtual void dataDiffMultiTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {0.0, 0.1, 3.0 }, {0.0, 0.1, 3.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {0.0, 0.1, 3.0},
			new double[] {0.0, 0.1, 3.0}
		};

		PiecewisePolynomialInterpolator interpPos = new SemiLocalCubicSplineInterpolator();
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

		PiecewisePolynomialInterpolator interpPos = new SemiLocalCubicSplineInterpolator();
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
//ORIGINAL LINE: final double[][] yValues = new double[][] { {0.0, 0.1, 0.05 }, {0.0, 0.1, 1.05 } };
		double[][] yValues = new double[][]
		{
			new double[] {0.0, 0.1, 0.05},
			new double[] {0.0, 0.1, 1.05}
		};

		PiecewisePolynomialInterpolator interpPos = new SemiLocalCubicSplineInterpolator();
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

		PiecewisePolynomialInterpolator interpPos = new SemiLocalCubicSplineInterpolator();
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

		PiecewisePolynomialInterpolator interpPos = new SemiLocalCubicSplineInterpolator();
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

		PiecewisePolynomialInterpolator interpPos = new SemiLocalCubicSplineInterpolator();
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

		PiecewisePolynomialInterpolator interpPos = new SemiLocalCubicSplineInterpolator();
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void infXdataTest()
	  public virtual void infXdataTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, INF};
		double[] yValues = new double[] {0.0, 0.1, 0.05, 0.2};

		PiecewisePolynomialInterpolator interpPos = new SemiLocalCubicSplineInterpolator();
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void infYdataTest()
	  public virtual void infYdataTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
		double[] yValues = new double[] {0.1, 0.05, 0.2, INF};

		PiecewisePolynomialInterpolator interpPos = new SemiLocalCubicSplineInterpolator();
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nanXdataTest()
	  public virtual void nanXdataTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, Double.NaN};
		double[] yValues = new double[] {0.0, 0.1, 0.05, 0.2};

		PiecewisePolynomialInterpolator interpPos = new SemiLocalCubicSplineInterpolator();
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nanYdataTest()
	  public virtual void nanYdataTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0};
		double[] yValues = new double[] {0.1, 0.05, 0.2, Double.NaN};

		PiecewisePolynomialInterpolator interpPos = new SemiLocalCubicSplineInterpolator();
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

		PiecewisePolynomialInterpolator interpPos = new SemiLocalCubicSplineInterpolator();
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
			new double[] {0.1, 0.05, 0.2, 1.0},
			new double[] {0.1, 0.05, 0.2, INF}
		};

		PiecewisePolynomialInterpolator interpPos = new SemiLocalCubicSplineInterpolator();
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

		PiecewisePolynomialInterpolator interpPos = new SemiLocalCubicSplineInterpolator();
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
			new double[] {0.1, 0.05, 0.2, 1.1},
			new double[] {0.1, 0.05, 0.2, Double.NaN}
		};

		PiecewisePolynomialInterpolator interpPos = new SemiLocalCubicSplineInterpolator();
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void notReconnectedTest()
	  public virtual void notReconnectedTest()
	  {
		double[] xValues = new double[] {1.0, 2.0000000001, 2.0, 4.0};
		double[] yValues = new double[] {2.0, 400.0, 3.0, 500000000.0};
		PiecewisePolynomialInterpolator interpPos = new SemiLocalCubicSplineInterpolator();
		interpPos.interpolate(xValues, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void notReconnectedMultiTest()
	  public virtual void notReconnectedMultiTest()
	  {
		double[] xValues = new double[] {1.0, 2.0, 4.0, 2.0000000001};
		double[][] yValues = new double[][]
		{
			new double[] {2.0, 3.0, 500000000.0, 400.0}
		};
		PiecewisePolynomialInterpolator interpPos = new SemiLocalCubicSplineInterpolator();
		interpPos.interpolate(xValues, yValues);
	  }

	}

}