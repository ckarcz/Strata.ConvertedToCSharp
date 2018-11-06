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
	/// Test <seealso cref="LinearCurveInterpolator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LinearCurveInterpolatorTest
	public class LinearCurveInterpolatorTest
	{

	  private static readonly CurveInterpolator LINEAR_INTERPOLATOR = LinearCurveInterpolator.INSTANCE;
	  private static readonly CurveExtrapolator FLAT_EXTRAPOLATOR = CurveExtrapolators.FLAT;

	  private static readonly DoubleArray X_DATA = DoubleArray.of(0.0, 0.4, 1.0, 1.8, 2.8, 5.0);
	  private static readonly DoubleArray Y_DATA = DoubleArray.of(3.0, 4.0, 3.1, 2.0, 7.0, 2.0);
	  private static readonly DoubleArray X_TEST = DoubleArray.of(0.2, 1.1, 2.3);
	  private static readonly DoubleArray Y_TEST = DoubleArray.of(3.5, 3.1 - (1.1 / 8), 4.5);
	  private const double TOL = 1.e-12;

	  public virtual void test_basics()
	  {
		assertEquals(LINEAR_INTERPOLATOR.Name, LinearCurveInterpolator.NAME);
		assertEquals(LINEAR_INTERPOLATOR.ToString(), LinearCurveInterpolator.NAME);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_interpolation()
	  {
		BoundCurveInterpolator bci = LINEAR_INTERPOLATOR.bind(X_DATA, Y_DATA, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		for (int i = 0; i < X_DATA.size(); i++)
		{
		  assertEquals(bci.interpolate(X_DATA.get(i)), Y_DATA.get(i), TOL);
		}
		for (int i = 0; i < X_TEST.size(); i++)
		{
		  assertEquals(bci.interpolate(X_TEST.get(i)), Y_TEST.get(i), TOL);
		}
	  }

	  public virtual void test_firstDerivative()
	  {
		BoundCurveInterpolator bci = LINEAR_INTERPOLATOR.bind(X_DATA, Y_DATA, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		double eps = 1e-8;
		double lo = bci.interpolate(0.2);
		double hi = bci.interpolate(0.2 + eps);
		double deriv = (hi - lo) / eps;
		assertEquals(bci.firstDerivative(0.2), deriv, 1e-6);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_firstNode()
	  {
		BoundCurveInterpolator bci = LINEAR_INTERPOLATOR.bind(X_DATA, Y_DATA, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		assertEquals(bci.interpolate(0.0), 3.0, TOL);
		assertEquals(bci.firstDerivative(0.0), bci.firstDerivative(0.01), TOL);
		assertEquals(bci.parameterSensitivity(0.0).get(0), 1d, TOL);
		assertEquals(bci.parameterSensitivity(0.0).get(1), 0d, TOL);
	  }

	  public virtual void test_allNodes()
	  {
		BoundCurveInterpolator bci = LINEAR_INTERPOLATOR.bind(X_DATA, Y_DATA, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		for (int i = 0; i < X_DATA.size(); i++)
		{
		  assertEquals(bci.interpolate(X_DATA.get(i)), Y_DATA.get(i), TOL);
		}
	  }

	  public virtual void test_lastNode()
	  {
		BoundCurveInterpolator bci = LINEAR_INTERPOLATOR.bind(X_DATA, Y_DATA, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		assertEquals(bci.interpolate(5.0), 2.0, TOL);
		assertEquals(bci.firstDerivative(5.0), bci.firstDerivative(4.99), TOL);
		assertEquals(bci.parameterSensitivity(5.0).get(X_DATA.size() - 2), 0d, TOL);
		assertEquals(bci.parameterSensitivity(5.0).get(X_DATA.size() - 1), 1d, TOL);
	  }

	  public virtual void test_interpolatorExtrapolator()
	  {
		DoubleArray xValues = DoubleArray.of(1, 2, 3);
		DoubleArray yValues = DoubleArray.of(2, 3, 5);
		CurveExtrapolator extrap = InterpolatorCurveExtrapolator.INSTANCE;
		BoundCurveInterpolator boundInterp = LINEAR_INTERPOLATOR.bind(xValues, yValues, extrap, extrap);
		assertEquals(boundInterp.interpolate(0.5), 1.5, TOL);
		assertEquals(boundInterp.interpolate(1), 2, TOL);
		assertEquals(boundInterp.interpolate(1.5), 2.5, TOL);
		assertEquals(boundInterp.interpolate(2), 3, TOL);
		assertEquals(boundInterp.interpolate(2.5), 4, TOL);
		assertEquals(boundInterp.interpolate(3), 5, TOL);
		assertEquals(boundInterp.interpolate(3.5), 6, TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_serialization()
	  {
		assertSerialization(LINEAR_INTERPOLATOR);
	  }

	}

}