using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using FiniteDifferenceType = com.opengamma.strata.math.impl.differentiation.FiniteDifferenceType;
	using ScalarFieldFirstOrderDifferentiator = com.opengamma.strata.math.impl.differentiation.ScalarFieldFirstOrderDifferentiator;
	using ScalarFirstOrderDifferentiator = com.opengamma.strata.math.impl.differentiation.ScalarFirstOrderDifferentiator;

	/// <summary>
	/// Test <seealso cref="ProductLinearCurveExtrapolator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ProductLinearCurveExtrapolatorTest
	public class ProductLinearCurveExtrapolatorTest
	{

	  private static readonly CurveExtrapolator EXTRAP = ProductLinearCurveExtrapolator.INSTANCE;
	  private static readonly DoubleArray X_DATA = DoubleArray.of(-0.5, 0.4, 1.0, 1.8, 2.8, 5.0);
	  private static readonly DoubleArray Y_DATA = DoubleArray.of(3.0, 4.0, 3.1, 2.0, 7.0, 2.0);
	  private static readonly int SIZE = X_DATA.size();
	  private static readonly DoubleArray X_LEFT_TEST = DoubleArray.of(-3.5, -1.1);
	  private static readonly DoubleArray X_RIGHT_TEST = DoubleArray.of(8.5, 11.1);
	  private const double EPS = 1.e-7;
	  private static readonly ScalarFirstOrderDifferentiator DIFF_CALC = new ScalarFirstOrderDifferentiator(FiniteDifferenceType.CENTRAL, EPS);
	  private static readonly ScalarFieldFirstOrderDifferentiator SENS_CALC = new ScalarFieldFirstOrderDifferentiator(FiniteDifferenceType.CENTRAL, EPS);

	  public virtual void test_basics()
	  {
		assertEquals(EXTRAP.Name, ProductLinearCurveExtrapolator.NAME);
		assertEquals(EXTRAP.ToString(), ProductLinearCurveExtrapolator.NAME);
	  }

	  public virtual void test_extrapolation()
	  {
		BoundCurveInterpolator bind = CurveInterpolators.DOUBLE_QUADRATIC.bind(X_DATA, Y_DATA, EXTRAP, EXTRAP);
		double gradLeft = (bind.interpolate(X_DATA.get(0) + EPS) * (X_DATA.get(0) + EPS) - Y_DATA.get(0) * X_DATA.get(0)) / EPS;
		for (int i = 0; i < X_LEFT_TEST.size(); ++i)
		{
		  double xyLeft = gradLeft * (X_LEFT_TEST.get(i) - X_DATA.get(0)) + Y_DATA.get(0) * X_DATA.get(0);
		  double expected = xyLeft / X_LEFT_TEST.get(i);
		  assertEquals(bind.interpolate(X_LEFT_TEST.get(i)), expected, 10d * Math.Abs(expected) * EPS);
		}
		double gradRight = (Y_DATA.get(SIZE - 1) * X_DATA.get(SIZE - 1) - bind.interpolate(X_DATA.get(SIZE - 1) - EPS) * (X_DATA.get(SIZE - 1) - EPS)) / EPS;
		for (int i = 0; i < X_RIGHT_TEST.size(); ++i)
		{
		  double xyRight = gradRight * (X_RIGHT_TEST.get(i) - X_DATA.get(SIZE - 1)) + Y_DATA.get(SIZE - 1) * X_DATA.get(SIZE - 1);
		  double expected = xyRight / X_RIGHT_TEST.get(i);
		  assertEquals(bind.interpolate(X_RIGHT_TEST.get(i)), expected, 10d * Math.Abs(expected) * EPS);
		}
	  }

	  public virtual void test_derivative_sensitivity()
	  {
		BoundCurveInterpolator bind = CurveInterpolators.DOUBLE_QUADRATIC.bind(X_DATA, Y_DATA, EXTRAP, EXTRAP);
		System.Func<double, double> derivFunc = x => bind.interpolate(x);

		for (int i = 0; i < X_LEFT_TEST.size(); ++i)
		{
		  assertEquals(bind.firstDerivative(X_LEFT_TEST.get(i)), DIFF_CALC.differentiate(derivFunc).apply(X_LEFT_TEST.get(i)), EPS);
		  int index = i;
		  System.Func<DoubleArray, double> sensFunc = y => CurveInterpolators.DOUBLE_QUADRATIC.bind(X_DATA, y, EXTRAP, EXTRAP).interpolate(X_LEFT_TEST.get(index));
		  assertTrue(DoubleArrayMath.fuzzyEquals(bind.parameterSensitivity(X_LEFT_TEST.get(index)).toArray(), SENS_CALC.differentiate(sensFunc).apply(Y_DATA).toArray(), EPS));
		}
		for (int i = 0; i < X_RIGHT_TEST.size(); ++i)
		{
		  assertEquals(bind.firstDerivative(X_RIGHT_TEST.get(i)), DIFF_CALC.differentiate(derivFunc).apply(X_RIGHT_TEST.get(i)), EPS);
		  int index = i;
		  System.Func<DoubleArray, double> sensFunc = y => CurveInterpolators.DOUBLE_QUADRATIC.bind(X_DATA, y, EXTRAP, EXTRAP).interpolate(X_RIGHT_TEST.get(index));
		  assertTrue(DoubleArrayMath.fuzzyEquals(bind.parameterSensitivity(X_RIGHT_TEST.get(index)).toArray(), SENS_CALC.differentiate(sensFunc).apply(Y_DATA).toArray(), EPS));
		}
	  }

	  public virtual void errorTest()
	  {
		DoubleArray xValues1 = DoubleArray.of(1, 2, 3);
		DoubleArray xValues2 = DoubleArray.of(-3, -2, -1);
		DoubleArray yValues = DoubleArray.of(1, 2, 3);
		BoundCurveInterpolator bind1 = CurveInterpolators.DOUBLE_QUADRATIC.bind(xValues1, yValues, EXTRAP, EXTRAP);
		BoundCurveInterpolator bind2 = CurveInterpolators.DOUBLE_QUADRATIC.bind(xValues2, yValues, EXTRAP, EXTRAP);
		assertThrowsIllegalArg(() => bind1.interpolate(-1));
		assertThrowsIllegalArg(() => bind1.firstDerivative(-1));
		assertThrowsIllegalArg(() => bind1.parameterSensitivity(-1));
		assertThrowsIllegalArg(() => bind2.interpolate(1));
		assertThrowsIllegalArg(() => bind2.firstDerivative(1));
		assertThrowsIllegalArg(() => bind2.parameterSensitivity(1));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(EXTRAP);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void sampleDataTest()
	  {
		DoubleArray xValues = DoubleArray.of(0.5, 1.0, 5.0, 10.0);
		DoubleArray yValues = DoubleArray.of(0.02, 0.05, 0.015, 0.01);
		DoubleArray rightKeys = DoubleArray.of(10.0, 12.0, 25.0, 35.0);
		DoubleArray leftKeys = DoubleArray.of(0.5, 0.25, 0.12, 0.005);
		BoundCurveInterpolator bind = CurveInterpolators.PRODUCT_NATURAL_SPLINE.bind(xValues, yValues, CurveExtrapolators.FLAT, CurveExtrapolators.PRODUCT_LINEAR);
		System.Func<double, double> fwdFunc = (double? x) =>
		{
	return 0.5 * (bind.interpolate(x + EPS) * (x + EPS) - bind.interpolate(x - EPS) * (x - EPS)) / EPS;
		};
		for (int i = 1; i < 3; ++i)
		{
		  // constant forward
		  assertEquals(fwdFunc(rightKeys.get(0)), fwdFunc(rightKeys.get(i)), EPS);
		  // constant zero
		  assertEquals(bind.interpolate(leftKeys.get(0)), bind.interpolate(leftKeys.get(i)), EPS);
		}
	  }

	}

}