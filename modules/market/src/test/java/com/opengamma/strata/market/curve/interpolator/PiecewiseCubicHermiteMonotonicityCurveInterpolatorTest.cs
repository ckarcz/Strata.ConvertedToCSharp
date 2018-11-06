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
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.INTERPOLATOR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using PiecewisePolynomialWithSensitivityFunction1D = com.opengamma.strata.math.impl.function.PiecewisePolynomialWithSensitivityFunction1D;
	using PiecewiseCubicHermiteSplineInterpolatorWithSensitivity = com.opengamma.strata.math.impl.interpolation.PiecewiseCubicHermiteSplineInterpolatorWithSensitivity;
	using PiecewisePolynomialResult = com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResult;
	using PiecewisePolynomialResultsWithSensitivity = com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResultsWithSensitivity;

	/// <summary>
	/// Test <seealso cref="PiecewiseCubicHermiteMonotonicityCurveInterpolator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PiecewiseCubicHermiteMonotonicityCurveInterpolatorTest
	public class PiecewiseCubicHermiteMonotonicityCurveInterpolatorTest
	{

	  private static readonly PiecewiseCubicHermiteSplineInterpolatorWithSensitivity BASE = new PiecewiseCubicHermiteSplineInterpolatorWithSensitivity();
	  private static readonly PiecewiseCubicHermiteMonotonicityCurveInterpolator PCHIP = PiecewiseCubicHermiteMonotonicityCurveInterpolator.INSTANCE;
	  private static readonly PiecewisePolynomialWithSensitivityFunction1D PPVAL = new PiecewisePolynomialWithSensitivityFunction1D();
	  private static readonly double[] X = new double[] {0, 0.4000, 1.0000, 2.0000, 3.0000, 3.25, 5.0000};
	  private static readonly double[][] Y = new double[][]
	  {
		  new double[] {1.2200, 1.0, 0.9, 1.1, 1.2000, 1.3, 1.2000},
		  new double[] {0.2200, 1.12, 1.5, 1.5, 1.7000, 1.8, 1.9000},
		  new double[] {1.2200, 1.12, 1.5, 1.5, 1.5000, 1.8, 1.9000},
		  new double[] {1.0, 1.0, 0.9, 1.1, 1.2000, 1.3, 1.3000},
		  new double[] {1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0}
	  };

	  private static readonly double[] XX;
	  static PiecewiseCubicHermiteMonotonicityCurveInterpolatorTest()
	  {
		int nSamples = 66;
		XX = new double[nSamples];
		for (int i = 0; i < nSamples; i++)
		{
		  XX[i] = -0.5 + 0.1 * i;
		}
	  }
	  private const double TOL = 1.0e-14;

	  public virtual void test_basics()
	  {
		assertEquals(PCHIP.Name, PiecewiseCubicHermiteMonotonicityCurveInterpolator.NAME);
		assertEquals(PCHIP.ToString(), PiecewiseCubicHermiteMonotonicityCurveInterpolator.NAME);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void baseInterpolationTest()
	  {
		int nExamples = Y.Length;
		int n = XX.Length;
		for (int example = 0; example < nExamples; example++)
		{
		  PiecewisePolynomialResult pp = BASE.interpolate(X, Y[example]);
		  BoundCurveInterpolator bound = PCHIP.bind(DoubleArray.ofUnsafe(X), DoubleArray.ofUnsafe(Y[example]), INTERPOLATOR, INTERPOLATOR);
		  for (int i = 0; i < n; i++)
		  {
			double computedValue = bound.interpolate(XX[i]);
			double expectedValue = PPVAL.evaluate(pp, XX[i]).get(0);
			assertEquals(computedValue, expectedValue, 1e-14);
			double computedDerivative = bound.firstDerivative(XX[i]);
			double expectedDerivative = PPVAL.differentiate(pp, XX[i]).get(0);
			assertEquals(computedDerivative, expectedDerivative, 1e-14);
		  }
		}
	  }

	  public virtual void sensitivityTest()
	  {
		int nExamples = Y.Length;
		int n = XX.Length;
		int nData = X.Length;
		for (int example = 0; example < nExamples; example++)
		{
		  PiecewisePolynomialResultsWithSensitivity pp = BASE.interpolateWithSensitivity(X, Y[example]);
		  BoundCurveInterpolator bound = PCHIP.bind(DoubleArray.ofUnsafe(X), DoubleArray.ofUnsafe(Y[example]), INTERPOLATOR, INTERPOLATOR);
		  for (int i = 0; i < n; i++)
		  {
			DoubleArray computed = bound.parameterSensitivity(XX[i]);
			DoubleArray expected = PPVAL.nodeSensitivity(pp, XX[i]);
			for (int j = 0; j < nData; j++)
			{
			  assertTrue(DoubleArrayMath.fuzzyEquals(computed.toArray(), expected.toArray(), TOL));
			}
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_serialization()
	  {
		assertSerialization(PCHIP);
	  }

	}

}