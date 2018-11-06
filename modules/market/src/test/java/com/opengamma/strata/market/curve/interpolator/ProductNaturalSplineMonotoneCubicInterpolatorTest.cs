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
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using FiniteDifferenceType = com.opengamma.strata.math.impl.differentiation.FiniteDifferenceType;
	using ScalarFieldFirstOrderDifferentiator = com.opengamma.strata.math.impl.differentiation.ScalarFieldFirstOrderDifferentiator;
	using ScalarFirstOrderDifferentiator = com.opengamma.strata.math.impl.differentiation.ScalarFirstOrderDifferentiator;
	using PiecewisePolynomialWithSensitivityFunction1D = com.opengamma.strata.math.impl.function.PiecewisePolynomialWithSensitivityFunction1D;
	using LogNaturalSplineHelper = com.opengamma.strata.math.impl.interpolation.LogNaturalSplineHelper;
	using MonotonicityPreservingCubicSplineInterpolator = com.opengamma.strata.math.impl.interpolation.MonotonicityPreservingCubicSplineInterpolator;
	using PiecewisePolynomialInterpolator = com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialInterpolator;
	using PiecewisePolynomialResultsWithSensitivity = com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResultsWithSensitivity;

	/// <summary>
	/// Test <seealso cref="ProductNaturalSplineMonotoneCubicInterpolator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ProductNaturalSplineMonotoneCubicInterpolatorTest
	public class ProductNaturalSplineMonotoneCubicInterpolatorTest
	{

	  private static readonly CurveInterpolator INTERP = ProductNaturalSplineMonotoneCubicInterpolator.INSTANCE;
	  private static readonly PiecewisePolynomialInterpolator BASE_INTERP = new MonotonicityPreservingCubicSplineInterpolator(new LogNaturalSplineHelper());
	  private static readonly PiecewisePolynomialWithSensitivityFunction1D FUNCTION = new PiecewisePolynomialWithSensitivityFunction1D();
	  private const double TOL = 1.0e-12;
	  private const double EPS = 1.0e-6;
	  private static readonly ScalarFirstOrderDifferentiator DIFF_CALC = new ScalarFirstOrderDifferentiator(FiniteDifferenceType.CENTRAL, EPS);
	  private static readonly ScalarFieldFirstOrderDifferentiator SENS_CALC = new ScalarFieldFirstOrderDifferentiator(FiniteDifferenceType.CENTRAL, EPS);

	  public virtual void sampleDataTest()
	  {
		DoubleArray xValues = DoubleArray.of(0.5, 1.0, 2.5, 4.2, 10.0, 15.0, 30.0);
		DoubleArray yValues = DoubleArray.of(4.0, 2.0, 1.0, 5.0, 10.0, 3.5, -2.0);
		int nData = yValues.size();
		DoubleArray pValues = DoubleArray.of(nData, i => xValues.get(i) * yValues.get(i));
		System.Func<double, bool> domain = (double? x) =>
		{
	return x.Value >= xValues.get(0) && x.Value <= xValues.get(nData - 1);
		};
		DoubleArray keys = DoubleArray.of(xValues.get(0), 0.7, 1.2, 7.8, 10.0, 17.52, 25.0, xValues.get(nData - 1));
		int nKeys = keys.size();
		BoundCurveInterpolator bound = INTERP.bind(xValues, yValues);
		PiecewisePolynomialResultsWithSensitivity ppRes = BASE_INTERP.interpolateWithSensitivity(xValues.toArray(), pValues.toArray());
		System.Func<double, double> funcDeriv = x => bound.interpolate(x.Value);
		for (int i = 0; i < nKeys; ++i)
		{
		  // interpolate
		  assertEquals(bound.interpolate(keys.get(i)), FUNCTION.evaluate(ppRes, keys.get(i)).get(0) / keys.get(i), TOL);
		  // first derivative
		  double firstExp = DIFF_CALC.differentiate(funcDeriv, domain).apply(keys.get(i));
		  assertEquals(bound.firstDerivative(keys.get(i)), firstExp, EPS);
		  // parameter sensitivity
		  int index = i;
		  System.Func<DoubleArray, double> funcSensi = x => INTERP.bind(xValues, x).interpolate(keys.get(index));
		  DoubleArray sensExp = SENS_CALC.differentiate(funcSensi).apply(yValues);
		  assertTrue(DoubleArrayMath.fuzzyEquals(bound.parameterSensitivity(keys.get(i)).toArray(), sensExp.toArray(), EPS));
		}
	  }

	  public virtual void negativeDataTest()
	  {
		DoubleArray xValues = DoubleArray.of(-34.5, -27.0, -22.5, -14.2, -10.0, -5.0, -0.3);
		DoubleArray yValues = DoubleArray.of(4.0, 2.0, 1.0, 5.0, 10.0, 3.5, -2.0);
		int nData = yValues.size();
		DoubleArray pValues = DoubleArray.of(nData, i => xValues.get(i) * yValues.get(i));
		System.Func<double, bool> domain = (double? x) =>
		{
	return x.Value >= xValues.get(0) && x.Value <= xValues.get(nData - 1);
		};
		DoubleArray keys = DoubleArray.of(xValues.get(0), -27.7, -21.2, -17.8, -10.0, -1.52, -0.35, xValues.get(nData - 1));
		int nKeys = keys.size();
		BoundCurveInterpolator bound = INTERP.bind(xValues, yValues);
		PiecewisePolynomialResultsWithSensitivity ppRes = BASE_INTERP.interpolateWithSensitivity(xValues.toArray(), pValues.toArray());
		System.Func<double, double> funcDeriv = x => bound.interpolate(x.Value);
		for (int i = 0; i < nKeys; ++i)
		{
		  // interpolate
		  assertEquals(bound.interpolate(keys.get(i)), FUNCTION.evaluate(ppRes, keys.get(i)).get(0) / keys.get(i), TOL);
		  // first derivative
		  double firstExp = DIFF_CALC.differentiate(funcDeriv, domain).apply(keys.get(i));
		  assertEquals(bound.firstDerivative(keys.get(i)), firstExp, EPS);
		  // parameter sensitivity
		  int index = i;
		  System.Func<DoubleArray, double> funcSensi = x => INTERP.bind(xValues, x).interpolate(keys.get(index));
		  DoubleArray sensExp = SENS_CALC.differentiate(funcSensi).apply(yValues);
		  assertTrue(DoubleArrayMath.fuzzyEquals(bound.parameterSensitivity(keys.get(i)).toArray(), sensExp.toArray(), EPS));
		}
	  }

	  public virtual void linearDataTest()
	  {
		DoubleArray xValues = DoubleArray.of(0.5, 2.0, 3.0, 4.0, 5.0);
		DoubleArray yValues = DoubleArray.of(1.0, 4.0, 6.0, 8.0, 10.0);
		int nData = yValues.size();
		DoubleArray pValues = DoubleArray.of(nData, i => xValues.get(i) * yValues.get(i));
		System.Func<double, bool> domain = (double? x) =>
		{
	return x.Value >= xValues.get(0) && x.Value <= xValues.get(nData - 1);
		};
		DoubleArray keys = DoubleArray.of(xValues.get(0), 1.1, 2.0, 4.7, xValues.get(nData - 1));
		int nKeys = keys.size();
		BoundCurveInterpolator bound = INTERP.bind(xValues, yValues);
		PiecewisePolynomialResultsWithSensitivity ppRes = BASE_INTERP.interpolateWithSensitivity(xValues.toArray(), pValues.toArray());
		System.Func<double, double> funcDeriv = x => bound.interpolate(x.Value);
		for (int i = 0; i < nKeys; ++i)
		{
		  // interpolate
		  assertEquals(bound.interpolate(keys.get(i)), FUNCTION.evaluate(ppRes, keys.get(i)).get(0) / keys.get(i), TOL);
		  // first derivative
		  double firstExp = DIFF_CALC.differentiate(funcDeriv, domain).apply(keys.get(i));
		  assertEquals(bound.firstDerivative(keys.get(i)), firstExp, EPS);
		  // parameter sensitivity
		  int index = i;
		  System.Func<DoubleArray, double> funcSensi = x => INTERP.bind(xValues, x).interpolate(keys.get(index));
		  DoubleArray sensExp = SENS_CALC.differentiate(funcSensi).apply(yValues);
		  assertTrue(DoubleArrayMath.fuzzyEquals(bound.parameterSensitivity(keys.get(i)).toArray(), sensExp.toArray(), EPS));
		}
	  }

	  public virtual void smallKeyTest()
	  {
		DoubleArray xValues = DoubleArray.of(1e-13, 3e-10, 3e-8, 2e-5, 3e-5);
		DoubleArray yValues = DoubleArray.of(1.0, -12.5, 13.2, 1.5, -1.5);
		double keyDw = 1.0e-12;
		BoundCurveInterpolator bound = INTERP.bind(xValues, yValues);
		assertThrowsIllegalArg(() => bound.interpolate(keyDw));
		assertThrowsIllegalArg(() => bound.firstDerivative(keyDw));
		assertThrowsIllegalArg(() => bound.parameterSensitivity(keyDw));
	  }

	  public virtual void getterTest()
	  {
		assertEquals(INTERP.Name, ProductNaturalSplineMonotoneCubicInterpolator.NAME);
		assertEquals(INTERP.ToString(), ProductNaturalSplineMonotoneCubicInterpolator.NAME);
	  }

	  public virtual void serializationTest()
	  {
		assertSerialization(INTERP);
	  }

	}

}