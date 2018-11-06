using System;

/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ConstrainedCubicSplineInterpolator = com.opengamma.strata.math.impl.interpolation.ConstrainedCubicSplineInterpolator;
	using CubicSplineInterpolator = com.opengamma.strata.math.impl.interpolation.CubicSplineInterpolator;
	using NaturalSplineInterpolator = com.opengamma.strata.math.impl.interpolation.NaturalSplineInterpolator;
	using PiecewiseCubicHermiteSplineInterpolator = com.opengamma.strata.math.impl.interpolation.PiecewiseCubicHermiteSplineInterpolator;
	using PiecewisePolynomialInterpolator = com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialInterpolator;
	using PiecewisePolynomialResultsWithSensitivity = com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResultsWithSensitivity;
	using SemiLocalCubicSplineInterpolator = com.opengamma.strata.math.impl.interpolation.SemiLocalCubicSplineInterpolator;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PiecewisePolynomialWithSensitivityFunction1DTest
	public class PiecewisePolynomialWithSensitivityFunction1DTest
	{
	  private const double EPS = 1.e-7;
	  private static readonly PiecewisePolynomialWithSensitivityFunction1D FUNCTION = new PiecewisePolynomialWithSensitivityFunction1D();

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void firstDerivativeFiniteDifferenceTest()
	  public virtual void firstDerivativeFiniteDifferenceTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialInterpolator[] interps = new com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialInterpolator[] {new com.opengamma.strata.math.impl.interpolation.NaturalSplineInterpolator(), new com.opengamma.strata.math.impl.interpolation.CubicSplineInterpolator(), new com.opengamma.strata.math.impl.interpolation.PiecewiseCubicHermiteSplineInterpolator(), new com.opengamma.strata.math.impl.interpolation.ConstrainedCubicSplineInterpolator(), new com.opengamma.strata.math.impl.interpolation.SemiLocalCubicSplineInterpolator() };
		PiecewisePolynomialInterpolator[] interps = new PiecewisePolynomialInterpolator[]
		{
			new NaturalSplineInterpolator(),
			new CubicSplineInterpolator(),
			new PiecewiseCubicHermiteSplineInterpolator(),
			new ConstrainedCubicSplineInterpolator(),
			new SemiLocalCubicSplineInterpolator()
		};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nInterps = interps.length;
		int nInterps = interps.Length;
		for (int k = 0; k < nInterps; ++k)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.8, 3.1, 5.9, 10.0, 16.0 };
		  double[] xValues = new double[] {1.0, 2.8, 3.1, 5.9, 10.0, 16.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1.0, 2.0, 3.0, -2.0, 5.0, -5.0 };
		  double[] yValues = new double[] {1.0, 2.0, 3.0, -2.0, 5.0, -5.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData = xValues.length;
		  int nData = xValues.Length;
		  double[] yValuesUp = Arrays.copyOf(yValues, nData);
		  double[] yValuesDw = Arrays.copyOf(yValues, nData);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xKeys = new double[10 * nData];
		  double[] xKeys = new double[10 * nData];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xMin = xValues[0];
		  double xMin = xValues[0];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xMax = xValues[nData - 1];
		  double xMax = xValues[nData - 1];
		  for (int i = 0; i < 10 * nData; ++i)
		  {
			xKeys[i] = xMin + (xMax - xMin) / (10 * nData - 1) * i;
		  }

		  PiecewisePolynomialResultsWithSensitivity result = interps[k].interpolateWithSensitivity(xValues, yValues);
		  for (int j = 0; j < nData; ++j)
		  {
			yValuesUp[j] = yValues[j] * (1.0 + EPS);
			yValuesDw[j] = yValues[j] * (1.0 - EPS);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResultsWithSensitivity resultUp = interps[k].interpolateWithSensitivity(xValues, yValuesUp);
			PiecewisePolynomialResultsWithSensitivity resultUp = interps[k].interpolateWithSensitivity(xValues, yValuesUp);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResultsWithSensitivity resultDw = interps[k].interpolateWithSensitivity(xValues, yValuesDw);
			PiecewisePolynomialResultsWithSensitivity resultDw = interps[k].interpolateWithSensitivity(xValues, yValuesDw);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] valuesUp = FUNCTION.evaluate(resultUp, xKeys).rowArray(0);
			double[] valuesUp = FUNCTION.evaluate(resultUp, xKeys).rowArray(0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] valuesDw = FUNCTION.evaluate(resultDw, xKeys).rowArray(0);
			double[] valuesDw = FUNCTION.evaluate(resultDw, xKeys).rowArray(0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] diffUp = FUNCTION.differentiate(resultUp, xKeys).rowArray(0);
			double[] diffUp = FUNCTION.differentiate(resultUp, xKeys).rowArray(0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] diffDw = FUNCTION.differentiate(resultDw, xKeys).rowArray(0);
			double[] diffDw = FUNCTION.differentiate(resultDw, xKeys).rowArray(0);
			for (int i = 0; i < 10 * nData; ++i)
			{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xKeyUp = xKeys[i] * (1.0 + EPS);
			  double xKeyUp = xKeys[i] * (1.0 + EPS);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xKeyDw = xKeys[i] * (1.0 - EPS);
			  double xKeyDw = xKeys[i] * (1.0 - EPS);
			  double valueFinite = 0.5 * (valuesUp[i] - valuesDw[i]) / EPS / yValues[j];
			  double senseFinite = 0.5 * (diffUp[i] - diffDw[i]) / EPS / yValues[j];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double resNodeSensitivity = FUNCTION.nodeSensitivity(result, xKeys[i]).get(j);
			  double resNodeSensitivity = FUNCTION.nodeSensitivity(result, xKeys[i]).get(j);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double resNodeSensitivityXkeyUp = FUNCTION.nodeSensitivity(result, xKeyUp).get(j);
			  double resNodeSensitivityXkeyUp = FUNCTION.nodeSensitivity(result, xKeyUp).get(j);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double resNodeSensitivityXkeyDw = FUNCTION.nodeSensitivity(result, xKeyDw).get(j);
			  double resNodeSensitivityXkeyDw = FUNCTION.nodeSensitivity(result, xKeyDw).get(j);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double senseFiniteXkey = 0.5 * (resNodeSensitivityXkeyUp - resNodeSensitivityXkeyDw) / EPS / xKeys[i];
			  double senseFiniteXkey = 0.5 * (resNodeSensitivityXkeyUp - resNodeSensitivityXkeyDw) / EPS / xKeys[i];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double resDiffNodeSensitivity = FUNCTION.differentiateNodeSensitivity(result, xKeys[i]).get(j);
			  double resDiffNodeSensitivity = FUNCTION.differentiateNodeSensitivity(result, xKeys[i]).get(j);
			  assertEquals(valueFinite, resNodeSensitivity, Math.Max(Math.Abs(yValues[j]) * EPS, EPS));
			  assertEquals(senseFinite, resDiffNodeSensitivity, Math.Max(Math.Abs(yValues[j]) * EPS, EPS));
			  assertEquals(senseFiniteXkey, resDiffNodeSensitivity, Math.Max(Math.Abs(xKeys[i]) * EPS, EPS));
			}
			yValuesUp[j] = yValues[j];
			yValuesDw[j] = yValues[j];
		  }
		}
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void secondDerivativeFiniteDifferenceTest()
	  public virtual void secondDerivativeFiniteDifferenceTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialInterpolator[] interps = new com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialInterpolator[] {new com.opengamma.strata.math.impl.interpolation.NaturalSplineInterpolator(), new com.opengamma.strata.math.impl.interpolation.CubicSplineInterpolator(), new com.opengamma.strata.math.impl.interpolation.PiecewiseCubicHermiteSplineInterpolator(), new com.opengamma.strata.math.impl.interpolation.ConstrainedCubicSplineInterpolator(), new com.opengamma.strata.math.impl.interpolation.SemiLocalCubicSplineInterpolator() };
		PiecewisePolynomialInterpolator[] interps = new PiecewisePolynomialInterpolator[]
		{
			new NaturalSplineInterpolator(),
			new CubicSplineInterpolator(),
			new PiecewiseCubicHermiteSplineInterpolator(),
			new ConstrainedCubicSplineInterpolator(),
			new SemiLocalCubicSplineInterpolator()
		};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nInterps = interps.length;
		int nInterps = interps.Length;
		for (int k = 0; k < nInterps; ++k)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.8, 3.1, 5.9, 10.0, 16.0 };
		  double[] xValues = new double[] {1.0, 2.8, 3.1, 5.9, 10.0, 16.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1.0, 2.0, 3.0, -2.0, 5.0, -5.0 };
		  double[] yValues = new double[] {1.0, 2.0, 3.0, -2.0, 5.0, -5.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData = xValues.length;
		  int nData = xValues.Length;
		  double[] yValuesUp = Arrays.copyOf(yValues, nData);
		  double[] yValuesDw = Arrays.copyOf(yValues, nData);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xKeys = new double[10 * nData];
		  double[] xKeys = new double[10 * nData];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xMin = xValues[0];
		  double xMin = xValues[0];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xMax = xValues[nData - 1];
		  double xMax = xValues[nData - 1];
		  for (int i = 0; i < 10 * nData; ++i)
		  {
			xKeys[i] = xMin + (xMax - xMin) / (10 * nData - 1) * i;
		  }

		  PiecewisePolynomialResultsWithSensitivity result = interps[k].interpolateWithSensitivity(xValues, yValues);
		  for (int j = 0; j < nData; ++j)
		  {
			yValuesUp[j] = yValues[j] * (1.0 + EPS);
			yValuesDw[j] = yValues[j] * (1.0 - EPS);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResultsWithSensitivity resultUp = interps[k].interpolateWithSensitivity(xValues, yValuesUp);
			PiecewisePolynomialResultsWithSensitivity resultUp = interps[k].interpolateWithSensitivity(xValues, yValuesUp);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResultsWithSensitivity resultDw = interps[k].interpolateWithSensitivity(xValues, yValuesDw);
			PiecewisePolynomialResultsWithSensitivity resultDw = interps[k].interpolateWithSensitivity(xValues, yValuesDw);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] diffUp = FUNCTION.differentiateTwice(resultUp, xKeys).toArray()[0];
			double[] diffUp = FUNCTION.differentiateTwice(resultUp, xKeys).toArray()[0];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] diffDw = FUNCTION.differentiateTwice(resultDw, xKeys).toArray()[0];
			double[] diffDw = FUNCTION.differentiateTwice(resultDw, xKeys).toArray()[0];
			for (int i = 0; i < 10 * nData; ++i)
			{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xKeyUp = xKeys[i] * (1.0 + EPS);
			  double xKeyUp = xKeys[i] * (1.0 + EPS);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xKeyDw = xKeys[i] * (1.0 - EPS);
			  double xKeyDw = xKeys[i] * (1.0 - EPS);

			  double senseFinite = 0.5 * (diffUp[i] - diffDw[i]) / EPS / yValues[j];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double resdiffNodeSensitivityXkeyUp = FUNCTION.differentiateNodeSensitivity(result, xKeyUp).get(j);
			  double resdiffNodeSensitivityXkeyUp = FUNCTION.differentiateNodeSensitivity(result, xKeyUp).get(j);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double resdiffNodeSensitivityXkeyDw = FUNCTION.differentiateNodeSensitivity(result, xKeyDw).get(j);
			  double resdiffNodeSensitivityXkeyDw = FUNCTION.differentiateNodeSensitivity(result, xKeyDw).get(j);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double senseFiniteXkey = 0.5 * (resdiffNodeSensitivityXkeyUp - resdiffNodeSensitivityXkeyDw) / EPS / xKeys[i];
			  double senseFiniteXkey = 0.5 * (resdiffNodeSensitivityXkeyUp - resdiffNodeSensitivityXkeyDw) / EPS / xKeys[i];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double resDiffTwiceNodeSensitivity = FUNCTION.differentiateTwiceNodeSensitivity(result, xKeys[i]).get(j);
			  double resDiffTwiceNodeSensitivity = FUNCTION.differentiateTwiceNodeSensitivity(result, xKeys[i]).get(j);

			  assertEquals(senseFinite, resDiffTwiceNodeSensitivity, Math.Max(Math.Abs(yValues[j]) * EPS, EPS));
			  assertEquals(senseFiniteXkey, resDiffTwiceNodeSensitivity, Math.Max(Math.Abs(xKeys[i]) * EPS, EPS));
			}
			yValuesUp[j] = yValues[j];
			yValuesDw[j] = yValues[j];
		  }
		}
	  }

	  /// <summary>
	  /// Interpolations with longer yValues
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void clampedFiniteDifferenceTest()
	  public virtual void clampedFiniteDifferenceTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialInterpolator[] interps = new com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialInterpolator[] {new com.opengamma.strata.math.impl.interpolation.CubicSplineInterpolator() };
		PiecewisePolynomialInterpolator[] interps = new PiecewisePolynomialInterpolator[] {new CubicSplineInterpolator()};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nInterps = interps.length;
		int nInterps = interps.Length;
		for (int k = 0; k < nInterps; ++k)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.8, 3.1, 5.9, 10.0, 16.0 };
		  double[] xValues = new double[] {1.0, 2.8, 3.1, 5.9, 10.0, 16.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] bcs = new double[] {-2.0, -1.5, 0.0, 1.0 / 3.0, 3.2 };
		  double[] bcs = new double[] {-2.0, -1.5, 0.0, 1.0 / 3.0, 3.2};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nBcs = bcs.length;
		  int nBcs = bcs.Length;
		  for (int l = 0; l < nBcs; ++l)
		  {
			for (int m = 0; m < nBcs; ++m)
			{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {bcs[l], 1.0, 2.0, 3.0, -2.0, 5.0, -5.0, bcs[m] };
			  double[] yValues = new double[] {bcs[l], 1.0, 2.0, 3.0, -2.0, 5.0, -5.0, bcs[m]};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData = xValues.length;
			  int nData = xValues.Length;
			  double[] yValuesUp = Arrays.copyOf(yValues, nData + 2);
			  double[] yValuesDw = Arrays.copyOf(yValues, nData + 2);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xKeys = new double[10 * nData];
			  double[] xKeys = new double[10 * nData];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xMin = xValues[0];
			  double xMin = xValues[0];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xMax = xValues[nData - 1];
			  double xMax = xValues[nData - 1];
			  for (int i = 0; i < 10 * nData; ++i)
			  {
				xKeys[i] = xMin + (xMax - xMin) / (10 * nData - 1) * i;
			  }

			  PiecewisePolynomialResultsWithSensitivity result = interps[k].interpolateWithSensitivity(xValues, yValues);
			  for (int j = 0; j < nData; ++j)
			  {
				yValuesUp[j + 1] = yValues[j + 1] * (1.0 + EPS);
				yValuesDw[j + 1] = yValues[j + 1] * (1.0 - EPS);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResultsWithSensitivity resultUp = interps[k].interpolateWithSensitivity(xValues, yValuesUp);
				PiecewisePolynomialResultsWithSensitivity resultUp = interps[k].interpolateWithSensitivity(xValues, yValuesUp);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResultsWithSensitivity resultDw = interps[k].interpolateWithSensitivity(xValues, yValuesDw);
				PiecewisePolynomialResultsWithSensitivity resultDw = interps[k].interpolateWithSensitivity(xValues, yValuesDw);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] valuesUp = FUNCTION.evaluate(resultUp, xKeys).toArray()[0];
				double[] valuesUp = FUNCTION.evaluate(resultUp, xKeys).toArray()[0];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] valuesDw = FUNCTION.evaluate(resultDw, xKeys).toArray()[0];
				double[] valuesDw = FUNCTION.evaluate(resultDw, xKeys).toArray()[0];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] diffUp = FUNCTION.differentiate(resultUp, xKeys).toArray()[0];
				double[] diffUp = FUNCTION.differentiate(resultUp, xKeys).toArray()[0];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] diffDw = FUNCTION.differentiate(resultDw, xKeys).toArray()[0];
				double[] diffDw = FUNCTION.differentiate(resultDw, xKeys).toArray()[0];
				for (int i = 0; i < 10 * nData; ++i)
				{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xKeyUp = xKeys[i] * (1.0 + EPS);
				  double xKeyUp = xKeys[i] * (1.0 + EPS);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xKeyDw = xKeys[i] * (1.0 - EPS);
				  double xKeyDw = xKeys[i] * (1.0 - EPS);
				  double valueFinite = 0.5 * (valuesUp[i] - valuesDw[i]) / EPS / yValues[j + 1];
				  double senseFinite = 0.5 * (diffUp[i] - diffDw[i]) / EPS / yValues[j + 1];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double resNodeSensitivity = FUNCTION.nodeSensitivity(result, xKeys[i]).get(j);
				  double resNodeSensitivity = FUNCTION.nodeSensitivity(result, xKeys[i]).get(j);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double resNodeSensitivityXkeyUp = FUNCTION.nodeSensitivity(result, xKeyUp).get(j);
				  double resNodeSensitivityXkeyUp = FUNCTION.nodeSensitivity(result, xKeyUp).get(j);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double resNodeSensitivityXkeyDw = FUNCTION.nodeSensitivity(result, xKeyDw).get(j);
				  double resNodeSensitivityXkeyDw = FUNCTION.nodeSensitivity(result, xKeyDw).get(j);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double senseFiniteXkey = 0.5 * (resNodeSensitivityXkeyUp - resNodeSensitivityXkeyDw) / EPS / xKeys[i];
				  double senseFiniteXkey = 0.5 * (resNodeSensitivityXkeyUp - resNodeSensitivityXkeyDw) / EPS / xKeys[i];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double resDiffNodeSensitivity = FUNCTION.differentiateNodeSensitivity(result, xKeys[i]).get(j);
				  double resDiffNodeSensitivity = FUNCTION.differentiateNodeSensitivity(result, xKeys[i]).get(j);
				  assertEquals(valueFinite, resNodeSensitivity, Math.Max(Math.Abs(yValues[j + 1]) * EPS, EPS));
				  assertEquals(senseFinite, resDiffNodeSensitivity, Math.Max(Math.Abs(yValues[j + 1]) * EPS, EPS));
				  assertEquals(senseFiniteXkey, resDiffNodeSensitivity, Math.Max(Math.Abs(xKeys[i]) * EPS, EPS));
				}
				yValuesUp[j + 1] = yValues[j + 1];
				yValuesDw[j + 1] = yValues[j + 1];
			  }
			}
		  }
		}
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void clampedSecondDerivativeFiniteDifferenceTest()
	  public virtual void clampedSecondDerivativeFiniteDifferenceTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialInterpolator[] interps = new com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialInterpolator[] {new com.opengamma.strata.math.impl.interpolation.CubicSplineInterpolator() };
		PiecewisePolynomialInterpolator[] interps = new PiecewisePolynomialInterpolator[] {new CubicSplineInterpolator()};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nInterps = interps.length;
		int nInterps = interps.Length;
		for (int k = 0; k < nInterps; ++k)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {1.0, 2.8, 3.1, 5.9, 10.0, 16.0 };
		  double[] xValues = new double[] {1.0, 2.8, 3.1, 5.9, 10.0, 16.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] bcs = new double[] {-2.0, -1.5, 0.0, 1.0 / 3.0, 3.2 };
		  double[] bcs = new double[] {-2.0, -1.5, 0.0, 1.0 / 3.0, 3.2};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nBcs = bcs.length;
		  int nBcs = bcs.Length;
		  for (int l = 0; l < nBcs; ++l)
		  {
			for (int m = 0; m < nBcs; ++m)
			{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {bcs[l], 1.0, 2.0, 3.0, -2.0, 5.0, -5.0, bcs[m] };
			  double[] yValues = new double[] {bcs[l], 1.0, 2.0, 3.0, -2.0, 5.0, -5.0, bcs[m]};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData = xValues.length;
			  int nData = xValues.Length;
			  double[] yValuesUp = Arrays.copyOf(yValues, nData + 2);
			  double[] yValuesDw = Arrays.copyOf(yValues, nData + 2);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xKeys = new double[10 * nData];
			  double[] xKeys = new double[10 * nData];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xMin = xValues[0];
			  double xMin = xValues[0];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xMax = xValues[nData - 1];
			  double xMax = xValues[nData - 1];
			  for (int i = 0; i < 10 * nData; ++i)
			  {
				xKeys[i] = xMin + (xMax - xMin) / (10 * nData - 1) * i;
			  }

			  PiecewisePolynomialResultsWithSensitivity result = interps[k].interpolateWithSensitivity(xValues, yValues);
			  for (int j = 0; j < nData; ++j)
			  {
				yValuesUp[j + 1] = yValues[j + 1] * (1.0 + EPS);
				yValuesDw[j + 1] = yValues[j + 1] * (1.0 - EPS);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResultsWithSensitivity resultUp = interps[k].interpolateWithSensitivity(xValues, yValuesUp);
				PiecewisePolynomialResultsWithSensitivity resultUp = interps[k].interpolateWithSensitivity(xValues, yValuesUp);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResultsWithSensitivity resultDw = interps[k].interpolateWithSensitivity(xValues, yValuesDw);
				PiecewisePolynomialResultsWithSensitivity resultDw = interps[k].interpolateWithSensitivity(xValues, yValuesDw);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] diffUp = FUNCTION.differentiateTwice(resultUp, xKeys).toArray()[0];
				double[] diffUp = FUNCTION.differentiateTwice(resultUp, xKeys).toArray()[0];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] diffDw = FUNCTION.differentiateTwice(resultDw, xKeys).toArray()[0];
				double[] diffDw = FUNCTION.differentiateTwice(resultDw, xKeys).toArray()[0];
				for (int i = 0; i < 10 * nData; ++i)
				{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xKeyUp = xKeys[i] * (1.0 + EPS);
				  double xKeyUp = xKeys[i] * (1.0 + EPS);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xKeyDw = xKeys[i] * (1.0 - EPS);
				  double xKeyDw = xKeys[i] * (1.0 - EPS);

				  double senseFinite = 0.5 * (diffUp[i] - diffDw[i]) / EPS / yValues[j + 1];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double resdiffNodeSensitivityXkeyUp = FUNCTION.differentiateNodeSensitivity(result, xKeyUp).get(j);
				  double resdiffNodeSensitivityXkeyUp = FUNCTION.differentiateNodeSensitivity(result, xKeyUp).get(j);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double resdiffNodeSensitivityXkeyDw = FUNCTION.differentiateNodeSensitivity(result, xKeyDw).get(j);
				  double resdiffNodeSensitivityXkeyDw = FUNCTION.differentiateNodeSensitivity(result, xKeyDw).get(j);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double senseFiniteXkey = 0.5 * (resdiffNodeSensitivityXkeyUp - resdiffNodeSensitivityXkeyDw) / EPS / xKeys[i];
				  double senseFiniteXkey = 0.5 * (resdiffNodeSensitivityXkeyUp - resdiffNodeSensitivityXkeyDw) / EPS / xKeys[i];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double resDiffTwiceNodeSensitivity = FUNCTION.differentiateTwiceNodeSensitivity(result, xKeys[i]).get(j);
				  double resDiffTwiceNodeSensitivity = FUNCTION.differentiateTwiceNodeSensitivity(result, xKeys[i]).get(j);

				  assertEquals(senseFinite, resDiffTwiceNodeSensitivity, Math.Max(Math.Abs(yValues[j + 1]) * EPS, EPS));
				  assertEquals(senseFiniteXkey, resDiffTwiceNodeSensitivity, Math.Max(Math.Abs(xKeys[i]) * EPS, EPS));
				}
				yValuesUp[j + 1] = yValues[j + 1];
				yValuesDw[j + 1] = yValues[j + 1];
			  }
			}
		  }
		}
	  }
	}

}