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
	/// Test <seealso cref="LogLinearCurveInterpolator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LogLinearCurveInterpolatorTest
	public class LogLinearCurveInterpolatorTest
	{

	  private static readonly CurveInterpolator LL_INTERPOLATOR = LogLinearCurveInterpolator.INSTANCE;
	  private static readonly CurveExtrapolator FLAT_EXTRAPOLATOR = CurveExtrapolators.FLAT;

	  private static readonly DoubleArray X_DATA = DoubleArray.of(0.0, 0.4, 1.0, 1.8, 2.8, 5.0);
	  private static readonly DoubleArray Y_DATA = DoubleArray.of(3.0, 4.0, 3.1, 2.0, 7.0, 2.0);
	  private static readonly DoubleArray Y_DATA_LOG = DoubleArray.of(Math.Log(3.0), Math.Log(4.0), Math.Log(3.1), Math.Log(2.0), Math.Log(7.0), Math.Log(2.0));
	  private const double TOL = 1.e-12;
	  private const double EPS = 1e-9;

	  public virtual void test_basics()
	  {
		assertEquals(LL_INTERPOLATOR.Name, LogLinearCurveInterpolator.NAME);
		assertEquals(LL_INTERPOLATOR.ToString(), LogLinearCurveInterpolator.NAME);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_interpolation()
	  {
		BoundCurveInterpolator bci = LL_INTERPOLATOR.bind(X_DATA, Y_DATA, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		for (int i = 0; i < X_DATA.size(); i++)
		{
		  assertEquals(bci.interpolate(X_DATA.get(i)), Y_DATA.get(i), TOL);
		}
		// log-linear same as linear where y-values have had log applied
		BoundCurveInterpolator bciLinear = CurveInterpolators.LINEAR.bind(X_DATA, Y_DATA_LOG, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		assertEquals(Math.Log(bci.interpolate(0.2)), bciLinear.interpolate(0.2), EPS);
		assertEquals(Math.Log(bci.interpolate(0.8)), bciLinear.interpolate(0.8), EPS);
		assertEquals(Math.Log(bci.interpolate(1.1)), bciLinear.interpolate(1.1), EPS);
		assertEquals(Math.Log(bci.interpolate(2.1)), bciLinear.interpolate(2.1), EPS);
		assertEquals(Math.Log(bci.interpolate(3.4)), bciLinear.interpolate(3.4), EPS);
	  }

	  public virtual void test_firstDerivative()
	  {
		BoundCurveInterpolator bci = LL_INTERPOLATOR.bind(X_DATA, Y_DATA, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		double eps = 1e-8;
		double lo = bci.interpolate(0.2);
		double hi = bci.interpolate(0.2 + eps);
		double deriv = (hi - lo) / eps;
		assertEquals(bci.firstDerivative(0.2), deriv, 1e-6);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_firstNode()
	  {
		BoundCurveInterpolator bci = LL_INTERPOLATOR.bind(X_DATA, Y_DATA, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		assertEquals(bci.interpolate(0.0), 3.0, TOL);
		assertEquals(bci.firstDerivative(0.0), bci.firstDerivative(0.00000001), 1e-6);
	  }

	  public virtual void test_allNodes()
	  {
		BoundCurveInterpolator bci = LL_INTERPOLATOR.bind(X_DATA, Y_DATA, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		for (int i = 0; i < X_DATA.size(); i++)
		{
		  assertEquals(bci.interpolate(X_DATA.get(i)), Y_DATA.get(i), TOL);
		}
	  }

	  public virtual void test_lastNode()
	  {
		BoundCurveInterpolator bci = LL_INTERPOLATOR.bind(X_DATA, Y_DATA, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		assertEquals(bci.interpolate(5.0), 2.0, TOL);
		assertEquals(bci.firstDerivative(5.0), bci.firstDerivative(4.99999999), 1e-6);
	  }

	  public virtual void test_interpolatorExtrapolator()
	  {
		DoubleArray xValues = DoubleArray.of(1, 2, 3);
		DoubleArray yValues = DoubleArray.of(2, 3, 5);
		DoubleArray yValuesLog = DoubleArray.of(Math.Log(2), Math.Log(3), Math.Log(5));
		CurveExtrapolator extrap = InterpolatorCurveExtrapolator.INSTANCE;
		// log-linear same as linear where y-values have had log applied
		BoundCurveInterpolator bciLinear = CurveInterpolators.LINEAR.bind(xValues, yValuesLog, extrap, extrap);
		BoundCurveInterpolator bci = LL_INTERPOLATOR.bind(xValues, yValues, extrap, extrap);
		assertEquals(Math.Log(bci.interpolate(0.5)), bciLinear.interpolate(0.5), EPS);
		assertEquals(Math.Log(bci.interpolate(1)), bciLinear.interpolate(1), EPS);
		assertEquals(Math.Log(bci.interpolate(1.5)), bciLinear.interpolate(1.5), EPS);
		assertEquals(Math.Log(bci.interpolate(2)), bciLinear.interpolate(2), EPS);
		assertEquals(Math.Log(bci.interpolate(2.5)), bciLinear.interpolate(2.5), EPS);
		assertEquals(Math.Log(bci.interpolate(3)), bciLinear.interpolate(3), EPS);
		assertEquals(Math.Log(bci.interpolate(3.5)), bciLinear.interpolate(3.5), EPS);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_serialization()
	  {
		assertSerialization(LL_INTERPOLATOR);
	  }

	}

}