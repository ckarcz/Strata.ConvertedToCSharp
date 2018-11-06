using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Test <seealso cref="DoubleQuadraticCurveInterpolator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DoubleQuadraticCurveInterpolatorTest
	public class DoubleQuadraticCurveInterpolatorTest
	{

	  private static readonly Random RANDOM = new Random(0L);
	  private static readonly CurveInterpolator DQ_INTERPOLATOR = DoubleQuadraticCurveInterpolator.INSTANCE;
	  private static readonly CurveExtrapolator FLAT_EXTRAPOLATOR = CurveExtrapolators.FLAT;

	  private static readonly DoubleArray X_DATA = DoubleArray.of(0.0, 0.4, 1.0, 1.8, 2.8, 5.0);
	  private static readonly DoubleArray Y_DATA = DoubleArray.of(3.0, 4.0, 3.1, 2.0, 7.0, 2.0);
	  private static readonly double[] X_TEST = new double[] {0, 0.3, 1.0, 2.0, 4.5, 5.0};
	  private static readonly double[] Y_TEST = new double[] {3.0, 3.87, 3.1, 2.619393939, 5.068181818, 2.0};
	  private const double TOL = 1.e-12;
	  private const double EPS = 1e-7;
	  private static readonly DoubleArray X_SENS;
	  private static readonly DoubleArray Y_SENS;
	  static DoubleQuadraticCurveInterpolatorTest()
	  {
		double a = -0.045;
		double b = 0.03;
		double c = 0.3;
		double d = 0.05;
		double[] x = new double[] {0.0, 0.5, 1.0, 2.0, 3.0, 5.0, 7.0, 10.0, 15.0, 17.5, 20.0, 25.0, 30.0};
		double[] y = new double[x.Length];
		for (int i = 0; i < x.Length; i++)
		{
		  y[i] = (a + b * x[i]) * Math.Exp(-c * x[i]) + d;
		}
		X_SENS = DoubleArray.copyOf(x);
		Y_SENS = DoubleArray.copyOf(y);
	  }

	  public virtual void test_basics()
	  {
		assertEquals(DQ_INTERPOLATOR.Name, DoubleQuadraticCurveInterpolator.NAME);
		assertEquals(DQ_INTERPOLATOR.ToString(), DoubleQuadraticCurveInterpolator.NAME);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_interpolation()
	  {
		BoundCurveInterpolator bci = DQ_INTERPOLATOR.bind(X_DATA, Y_DATA, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		for (int i = 0; i < X_TEST.Length; i++)
		{
		  assertEquals(bci.interpolate(X_TEST[i]), Y_TEST[i], 1e-8);
		}
	  }

	  public virtual void test_oneInterval()
	  {
		DoubleArray x = DoubleArray.of(1.4, 1.8);
		DoubleArray y = DoubleArray.of(0.34, 0.56);
		BoundCurveInterpolator bci = DQ_INTERPOLATOR.bind(x, y, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		double value = bci.interpolate(1.6);
		assertEquals((y.get(0) + y.get(1)) / 2, value, 0.0);

		double m = (y.get(1) - y.get(0)) / (x.get(1) - x.get(0));
		assertEquals(bci.firstDerivative(1.5), m, 0.0);
		assertEquals(bci.firstDerivative(x.get(1)), m, 0.0);
	  }

	  public virtual void test_firstDerivative()
	  {
		BoundCurveInterpolator bci = DQ_INTERPOLATOR.bind(X_DATA, Y_DATA, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		double eps = 1e-8;
		double lo = bci.interpolate(0.2);
		double hi = bci.interpolate(0.2 + eps);
		double deriv = (hi - lo) / eps;
		assertEquals(bci.firstDerivative(0.2), deriv, 1e-6);
	  }

	  public virtual void test_firstDerivative2()
	  {
		double a = 1.34;
		double b = 7.0 / 3.0;
		double c = -0.52;
		double[] x = new double[] {-11.0 / 2.3, 0.0, 0.01, 2.71, 17.0 / 3.2};
		int n = x.Length;
		double[] y = new double[n];
		for (int i = 0; i < n; i++)
		{
		  y[i] = a + b * x[i] + c * x[i] * x[i];
		}
		BoundCurveInterpolator bci = DQ_INTERPOLATOR.bind(DoubleArray.copyOf(x), DoubleArray.copyOf(y), FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		double grad = bci.firstDerivative(x[n - 1]);
		assertEquals(b + 2 * c * x[n - 1], grad, 1e-15);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_sensitivities()
	  {
		BoundCurveInterpolator bci = DQ_INTERPOLATOR.bind(X_SENS, Y_SENS, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		double lastXValue = X_SENS.get(X_SENS.size() - 1);
		for (int i = 0; i < 100; i++)
		{
		  double t = lastXValue * RANDOM.NextDouble();
		  DoubleArray sensitivity = bci.parameterSensitivity(t);
		  assertEquals(sensitivity.sum(), 1d, TOL);
		}
	  }

	  public virtual void test_sensitivityEdgeCase()
	  {
		BoundCurveInterpolator bci = DQ_INTERPOLATOR.bind(X_SENS, Y_SENS, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		double lastXValue = X_SENS.get(X_SENS.size() - 1);
		DoubleArray sensitivity = bci.parameterSensitivity(lastXValue);
		for (int i = 0; i < sensitivity.size() - 1; i++)
		{
		  assertEquals(0, sensitivity.get(i), EPS);
		}
		assertEquals(1.0, sensitivity.get(sensitivity.size() - 1), EPS);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_firstNode()
	  {
		BoundCurveInterpolator bci = DQ_INTERPOLATOR.bind(X_DATA, Y_DATA, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		assertEquals(bci.interpolate(0.0), 3.0, TOL);
		assertEquals(bci.firstDerivative(0.0), bci.firstDerivative(0.00000001), 1e-6);
	  }

	  public virtual void test_allNodes()
	  {
		BoundCurveInterpolator bci = DQ_INTERPOLATOR.bind(X_DATA, Y_DATA, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		for (int i = 0; i < X_DATA.size(); i++)
		{
		  assertEquals(bci.interpolate(X_DATA.get(i)), Y_DATA.get(i), TOL);
		}
	  }

	  public virtual void test_lastNode()
	  {
		BoundCurveInterpolator bci = DQ_INTERPOLATOR.bind(X_DATA, Y_DATA, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		assertEquals(bci.interpolate(5.0), 2.0, TOL);
		assertEquals(bci.firstDerivative(5.0), bci.firstDerivative(4.99999999), 1e-6);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_serialization()
	  {
		assertSerialization(DQ_INTERPOLATOR);
	  }

	}

}