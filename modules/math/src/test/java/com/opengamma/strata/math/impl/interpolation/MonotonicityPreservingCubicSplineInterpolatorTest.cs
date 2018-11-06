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
//ORIGINAL LINE: @Test public class MonotonicityPreservingCubicSplineInterpolatorTest
	public class MonotonicityPreservingCubicSplineInterpolatorTest
	{

	  private const double EPS = 1e-13;
	  private const double INF = 1.0 / 0.0;

	  /// 
	  public virtual void localMonotonicityIncTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {2.0, 3.0, 5.0, 8.0, 9.0, 13.0 };
		double[] xValues = new double[] {2.0, 3.0, 5.0, 8.0, 9.0, 13.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1.0, 1.01, 2.0, 2.1, 2.2, 2.201 };
		double[] yValues = new double[] {1.0, 1.01, 2.0, 2.1, 2.2, 2.201};

		PiecewisePolynomialInterpolator interp = new NaturalSplineInterpolator();
		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
		PiecewisePolynomialResult resultPos = interpPos.interpolate(xValues, yValues);

		assertEquals(resultPos.Dimensions, result.Dimensions);
		assertEquals(resultPos.NumberOfIntervals, result.NumberOfIntervals);
		assertEquals(resultPos.Order, result.Order);

		const int nKeys = 111;
		double key0 = 2.0;
		for (int i = 1; i < nKeys; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double key = 2.0 + 11.0 / (nKeys - 1) * i;
		  double key = 2.0 + 11.0 / (nKeys - 1) * i;
		  assertTrue(function.evaluate(resultPos, key).get(0) - function.evaluate(resultPos, key0).get(0) >= 0.0);

		  key0 = 2.0 + 11.0 / (nKeys - 1) * i;
		}
	  }

	  /// 
	  public virtual void localMonotonicityClampedTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {-2.0, 3.0, 4.0, 8.0, 9.1, 10.0 };
		double[] xValues = new double[] {-2.0, 3.0, 4.0, 8.0, 9.1, 10.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {0.0, 10.0, 9.5, 2.0, 1.1, -2.2, -2.6, 0.0 };
		double[] yValues = new double[] {0.0, 10.0, 9.5, 2.0, 1.1, -2.2, -2.6, 0.0};

		PiecewisePolynomialInterpolator interp = new CubicSplineInterpolator();
		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
		PiecewisePolynomialResult resultPos = interpPos.interpolate(xValues, yValues);

		assertEquals(resultPos.Dimensions, result.Dimensions);
		assertEquals(resultPos.NumberOfIntervals, result.NumberOfIntervals);
		assertEquals(resultPos.Order, result.Order);

		const int nKeys = 121;
		double key0 = -2.0;
		for (int i = 1; i < nKeys; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double key = -2.0 + 12.0 / (nKeys - 1) * i;
		  double key = -2.0 + 12.0 / (nKeys - 1) * i;
		  assertTrue(function.evaluate(resultPos, key).get(0) - function.evaluate(resultPos, key0).get(0) <= 0.0);

		  key0 = -2.0 + 11.0 / (nKeys - 1) * i;
		}
	  }

	  /// 
	  public virtual void localMonotonicityClampedMultiTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {-2.0, 3.0, 4.0, 8.0, 9.1, 10.0 };
		double[] xValues = new double[] {-2.0, 3.0, 4.0, 8.0, 9.1, 10.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {0.0, 10.0, 9.5, 2.0, 1.1, -2.2, -2.6, 0.0 }, {10.0, 10.0, 9.5, 2.0, 1.1, -2.2, -2.6, 10.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {0.0, 10.0, 9.5, 2.0, 1.1, -2.2, -2.6, 0.0},
			new double[] {10.0, 10.0, 9.5, 2.0, 1.1, -2.2, -2.6, 10.0}
		};

		PiecewisePolynomialInterpolator interp = new CubicSplineInterpolator();
		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
		PiecewisePolynomialResult resultPos = interpPos.interpolate(xValues, yValues);

		assertEquals(resultPos.Dimensions, result.Dimensions);
		assertEquals(resultPos.NumberOfIntervals, result.NumberOfIntervals);
		assertEquals(resultPos.Order, result.Order);

		const int nKeys = 121;
		double key0 = -2.0;
		for (int i = 1; i < nKeys; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double key = -2.0 + 12.0 / (nKeys - 1) * i;
		  double key = -2.0 + 12.0 / (nKeys - 1) * i;
		  assertTrue(function.evaluate(resultPos, key).get(0) - function.evaluate(resultPos, key0).get(0) <= 0.0);

		  key0 = -2.0 + 11.0 / (nKeys - 1) * i;
		}
		key0 = -2.0;
		for (int i = 1; i < nKeys; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double key = -2.0 + 12.0 / (nKeys - 1) * i;
		  double key = -2.0 + 12.0 / (nKeys - 1) * i;
		  assertTrue(function.evaluate(resultPos, key).get(1) - function.evaluate(resultPos, key0).get(1) <= 0.0);

		  key0 = -2.0 + 11.0 / (nKeys - 1) * i;
		}
	  }

	  /// 
	  public virtual void localMonotonicityDecTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {-2.0, 3.0, 4.0, 8.0, 9.1, 10.0 };
		double[] xValues = new double[] {-2.0, 3.0, 4.0, 8.0, 9.1, 10.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {10.0, 9.5, 2.0, 1.1, -2.2, -2.6 };
		double[] yValues = new double[] {10.0, 9.5, 2.0, 1.1, -2.2, -2.6};

		PiecewisePolynomialInterpolator interp = new CubicSplineInterpolator();
		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
		PiecewisePolynomialResult resultPos = interpPos.interpolate(xValues, yValues);

		assertEquals(resultPos.Dimensions, result.Dimensions);
		assertEquals(resultPos.NumberOfIntervals, result.NumberOfIntervals);
		assertEquals(resultPos.Order, result.Order);

		const int nKeys = 121;
		double key0 = -2.0;
		for (int i = 1; i < nKeys; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double key = -2.0 + 12.0 / (nKeys - 1) * i;
		  double key = -2.0 + 12.0 / (nKeys - 1) * i;
		  assertTrue(function.evaluate(resultPos, key).get(0) - function.evaluate(resultPos, key0).get(0) <= 0.0);

		  key0 = -2.0 + 11.0 / (nKeys - 1) * i;
		}
	  }

	  /// <summary>
	  /// local extrema are not necessarily at data-points
	  /// </summary>
	  public virtual void extremumTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8 };
		double[] xValues = new double[] {1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] yValues = new double[][] { {1.0, 1.0, 2.0, 4.0, 4.0, 2.0, 1.0, 1.0 }, {10.0, 10.0, 6.0, 4.0, 4.0, 6.0, 10.0, 10.0 } };
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 1.0, 2.0, 4.0, 4.0, 2.0, 1.0, 1.0},
			new double[] {10.0, 10.0, 6.0, 4.0, 4.0, 6.0, 10.0, 10.0}
		};

		PiecewisePolynomialInterpolator interp = new CubicSplineInterpolator();
		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
		PiecewisePolynomialResult resultPos = interpPos.interpolate(xValues, yValues);

		assertEquals(resultPos.Dimensions, result.Dimensions);
		assertEquals(resultPos.NumberOfIntervals, result.NumberOfIntervals);
		assertEquals(resultPos.Order, result.Order);

		assertTrue(function.evaluate(resultPos, 4.5).get(0) - function.evaluate(resultPos, 4).get(0) >= 0.0);
		assertTrue(function.evaluate(resultPos, 4.5).get(0) - function.evaluate(resultPos, 5).get(0) >= 0.0);
		assertTrue(function.evaluate(resultPos, 4.5).get(1) - function.evaluate(resultPos, 4).get(1) <= 0.0);
		assertTrue(function.evaluate(resultPos, 4.5).get(1) - function.evaluate(resultPos, 5).get(1) <= 0.0);

		const int nKeys = 41;
		double key0 = 1.0;
		for (int i = 1; i < nKeys; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double key = 1.0 + 3.0 / (nKeys - 1) * i;
		  double key = 1.0 + 3.0 / (nKeys - 1) * i;
		  assertTrue(function.evaluate(resultPos, key).get(0) - function.evaluate(resultPos, key0).get(0) >= 0.0);

		  key0 = 1.0 + 3.0 / (nKeys - 1) * i;
		}
		key0 = 1.0;
		for (int i = 1; i < nKeys; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double key = 1.0 + 3.0 / (nKeys - 1) * i;
		  double key = 1.0 + 3.0 / (nKeys - 1) * i;
		  assertTrue(function.evaluate(resultPos, key).get(1) - function.evaluate(resultPos, key0).get(1) <= 0.0);

		  key0 = 1.0 + 3.0 / (nKeys - 1) * i;
		}
		key0 = 5.0;
		for (int i = 1; i < nKeys; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double key = 5.0 + 3.0 / (nKeys - 1) * i;
		  double key = 5.0 + 3.0 / (nKeys - 1) * i;
		  assertTrue(function.evaluate(resultPos, key).get(0) - function.evaluate(resultPos, key0).get(0) <= 0.0);

		  key0 = 5.0 + 3.0 / (nKeys - 1) * i;
		}
		key0 = 5.0;
		for (int i = 1; i < nKeys; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double key = 5.0 + 3.0 / (nKeys - 1) * i;
		  double key = 5.0 + 3.0 / (nKeys - 1) * i;
		  assertTrue(function.evaluate(resultPos, key).get(1) - function.evaluate(resultPos, key0).get(1) >= 0.0);

		  key0 = 5.0 + 3.0 / (nKeys - 1) * i;
		}
	  }

	  /// <summary>
	  /// PiecewiseCubicHermiteSplineInterpolator is not modified except the first 2 and last 2 intervals
	  /// </summary>
	  public virtual void localMonotonicityDec2Test()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {-2.0, 3.0, 4.0, 8.0, 9.1, 10.0, 12.0, 14.0 };
		double[] xValues = new double[] {-2.0, 3.0, 4.0, 8.0, 9.1, 10.0, 12.0, 14.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {11.0, 9.5, 2.0, 1.1, -2.2, -2.6, 2.0, 2.0 };
		double[] yValues = new double[] {11.0, 9.5, 2.0, 1.1, -2.2, -2.6, 2.0, 2.0};

		PiecewisePolynomialInterpolator interp = new PiecewiseCubicHermiteSplineInterpolator();
		PiecewisePolynomialResult result = interp.interpolate(xValues, yValues);

		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
		PiecewisePolynomialResult resultPos = interpPos.interpolate(xValues, yValues);

		assertEquals(resultPos.Dimensions, result.Dimensions);
		assertEquals(resultPos.NumberOfIntervals, result.NumberOfIntervals);
		assertEquals(resultPos.Order, result.Order);

		for (int i = 2; i < resultPos.NumberOfIntervals - 2; ++i)
		{
		  for (int j = 0; j < 4; ++j)
		  {
			assertEquals(resultPos.CoefMatrix.get(i, j), result.CoefMatrix.get(i, j), EPS);
		  }
		}

		const int nKeys = 121;
		double key0 = -2.0;
		for (int i = 1; i < nKeys; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double key = -2.0 + 12.0 / (nKeys - 1) * i;
		  double key = -2.0 + 12.0 / (nKeys - 1) * i;
		  assertTrue(function.evaluate(resultPos, key).get(0) - function.evaluate(resultPos, key0).get(0) <= 0.0);

		  key0 = -2.0 + 11.0 / (nKeys - 1) * i;
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
		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
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
		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
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
		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
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
		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
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
		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
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
		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
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
		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
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
		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
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
		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
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
		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
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
		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
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
		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
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
		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
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
		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
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
		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
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
		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
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
		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
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
		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
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
		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
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
		PiecewisePolynomialInterpolator interpPos = new MonotonicityPreservingCubicSplineInterpolator(interp);
		interpPos.interpolate(xValues, yValues);
	  }
	}

}