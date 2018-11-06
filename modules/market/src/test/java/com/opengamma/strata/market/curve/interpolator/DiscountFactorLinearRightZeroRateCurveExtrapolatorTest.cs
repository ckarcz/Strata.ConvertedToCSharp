using System;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
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
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.DISCOUNT_FACTOR_LINEAR_RIGHT_ZERO_RATE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.DISCOUNT_FACTOR_QUADRATIC_LEFT_ZERO_RATE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.QUADRATIC_LEFT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LOG_NATURAL_SPLINE_MONOTONE_CUBIC;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.PRODUCT_LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.PRODUCT_NATURAL_SPLINE_MONOTONE_CUBIC;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Test <seealso cref="DiscountFactorLinearRightZeroRateCurveExtrapolator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountFactorLinearRightZeroRateCurveExtrapolatorTest
	public class DiscountFactorLinearRightZeroRateCurveExtrapolatorTest
	{

	  private static readonly DoubleArray X_VALUES = DoubleArray.of(1.0, 2.0, 3.0, 5.0, 7.0);
	  private static readonly DoubleArray Y_VALUES = DoubleArray.of(0.05, 0.01, 0.03, -0.01, 0.02);
	  private static readonly int NUM_DATA = X_VALUES.size();
	  private static readonly DoubleArray DSC_VALUES = DoubleArray.of(NUM_DATA, n => Math.Exp(-X_VALUES.get(n) * Y_VALUES.get(n)));
	  private const int NUM_KEYS = 20;
	  private static readonly DoubleArray X_KEYS = DoubleArray.of(NUM_KEYS, n => X_VALUES.get(NUM_DATA - 1) + 0.07d * n);
	  private const double EPS = 1.e-6;
	  private const double TOL = 1.e-11;
	  private const double TOL_E2E = 1.e-8;

	  public virtual void basicsTest()
	  {
		assertEquals(DISCOUNT_FACTOR_LINEAR_RIGHT_ZERO_RATE.Name, DiscountFactorLinearRightZeroRateCurveExtrapolator.NAME);
		assertEquals(DISCOUNT_FACTOR_LINEAR_RIGHT_ZERO_RATE.ToString(), DiscountFactorLinearRightZeroRateCurveExtrapolator.NAME);
	  }

	  public virtual void interpolateTest()
	  {
		BoundCurveInterpolator bci = PRODUCT_LINEAR.bind(X_VALUES, Y_VALUES, LINEAR, DISCOUNT_FACTOR_LINEAR_RIGHT_ZERO_RATE);
		double grad = -Y_VALUES.get(NUM_DATA - 1) * DSC_VALUES.get(NUM_DATA - 1) - X_VALUES.get(NUM_DATA - 1) * DSC_VALUES.get(NUM_DATA - 1) * bci.firstDerivative(X_VALUES.get(NUM_DATA - 1));
		for (int i = 0; i < NUM_KEYS; ++i)
		{
		  double key = X_KEYS.get(i);
		  double df = grad * (key - X_VALUES.get(NUM_DATA - 1)) + DSC_VALUES.get(NUM_DATA - 1);
		  assertEquals(bci.interpolate(key), -Math.Log(df) / key, TOL);
		}
	  }

	  public virtual void derivativeTest()
	  {
		BoundCurveInterpolator bci = PRODUCT_LINEAR.bind(X_VALUES, Y_VALUES, LINEAR, DISCOUNT_FACTOR_LINEAR_RIGHT_ZERO_RATE);
		for (int i = 0; i < NUM_KEYS; ++i)
		{
		  double key = X_KEYS.get(i);
		  double computed = bci.firstDerivative(key);
		  double expected = 0.5d * (bci.interpolate(key + EPS) - bci.interpolate(key - EPS)) / EPS;
		  assertEquals(computed, expected, EPS);
		}
	  }

	  public virtual void parameterSensitivityTest()
	  {
		BoundCurveInterpolator bci = PRODUCT_LINEAR.bind(X_VALUES, Y_VALUES, LINEAR, DISCOUNT_FACTOR_LINEAR_RIGHT_ZERO_RATE);
		for (int i = 0; i < NUM_KEYS; ++i)
		{
		  double key = X_KEYS.get(i);
		  DoubleArray computed = bci.parameterSensitivity(key);
		  for (int j = 0; j < NUM_DATA; ++j)
		  {
			double[] yValuesUp = Y_VALUES.toArray();
			double[] yValuesDw = Y_VALUES.toArray();
			yValuesUp[j] += EPS;
			yValuesDw[j] -= EPS;
			BoundCurveInterpolator bciUp = PRODUCT_LINEAR.bind(X_VALUES, DoubleArray.copyOf(yValuesUp), LINEAR, DISCOUNT_FACTOR_LINEAR_RIGHT_ZERO_RATE);
			BoundCurveInterpolator bciDw = PRODUCT_LINEAR.bind(X_VALUES, DoubleArray.copyOf(yValuesDw), LINEAR, DISCOUNT_FACTOR_LINEAR_RIGHT_ZERO_RATE);
			double expected = 0.5 * (bciUp.interpolate(key) - bciDw.interpolate(key)) / EPS;
			assertEquals(computed.get(j), expected, EPS * 10d);
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void e2eTest()
	  {
		BoundCurveInterpolator bciZero = PRODUCT_NATURAL_SPLINE_MONOTONE_CUBIC.bind(X_VALUES, Y_VALUES, DISCOUNT_FACTOR_QUADRATIC_LEFT_ZERO_RATE, DISCOUNT_FACTOR_LINEAR_RIGHT_ZERO_RATE);
		BoundCurveInterpolator bciDf = LOG_NATURAL_SPLINE_MONOTONE_CUBIC.bind(X_VALUES, DSC_VALUES, QUADRATIC_LEFT, LINEAR);
		int nKeys = 170;
		for (int i = 0; i < nKeys; ++i)
		{
		  double key = -0.1d + 0.05d * i;
		  double zero = bciZero.interpolate(key);
		  double df = bciDf.interpolate(key);
		  assertEquals(Math.Exp(-key * zero), df, TOL_E2E);
		  double zeroGrad = bciZero.firstDerivative(key);
		  double dfGrad = bciDf.firstDerivative(key);
		  assertEquals(-zero * df - key * df * zeroGrad, dfGrad, TOL_E2E);
		  DoubleArray zeroSensi = bciZero.parameterSensitivity(key);
		  DoubleArray dfSensi = bciDf.parameterSensitivity(key);
		  for (int j = 0; j < X_VALUES.size(); ++j)
		  {
			assertEquals(key * df * zeroSensi.get(j) / (X_VALUES.get(j) * DSC_VALUES.get(j)), dfSensi.get(j), TOL_E2E * 10d);
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void noLeftTest()
	  {
		BoundCurveInterpolator bci = PRODUCT_LINEAR.bind(X_VALUES, Y_VALUES, DISCOUNT_FACTOR_LINEAR_RIGHT_ZERO_RATE, DISCOUNT_FACTOR_LINEAR_RIGHT_ZERO_RATE);
		assertThrowsIllegalArg(() => bci.interpolate(0.2d));
		assertThrowsIllegalArg(() => bci.firstDerivative(0.3d));
		assertThrowsIllegalArg(() => bci.parameterSensitivity(0.6d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void serializationTest()
	  {
		assertSerialization(DISCOUNT_FACTOR_LINEAR_RIGHT_ZERO_RATE);
	  }

	}

}