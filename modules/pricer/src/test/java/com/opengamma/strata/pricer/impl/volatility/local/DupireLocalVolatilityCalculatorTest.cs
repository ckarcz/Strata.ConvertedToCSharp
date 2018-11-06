using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.volatility.local
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.INTERPOLATOR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.NATURAL_SPLINE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;
	using ConstantSurface = com.opengamma.strata.market.surface.ConstantSurface;
	using DefaultSurfaceMetadata = com.opengamma.strata.market.surface.DefaultSurfaceMetadata;
	using DeformedSurface = com.opengamma.strata.market.surface.DeformedSurface;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using NodalSurface = com.opengamma.strata.market.surface.NodalSurface;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;

	/// <summary>
	/// Test <seealso cref="DupireLocalVolatilityCalculator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DupireLocalVolatilityCalculatorTest
	public class DupireLocalVolatilityCalculatorTest
	{

	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(NATURAL_SPLINE, INTERPOLATOR, NATURAL_SPLINE, INTERPOLATOR);
	  private static readonly DoubleArray TIMES = DoubleArray.of(0.25, 0.25, 0.25, 0.5, 0.5, 0.5, 0.75, 0.75, 0.75, 1, 1, 1);
	  private static readonly DoubleArray STRIKES = DoubleArray.of(0.8, 1.4, 2, 0.8, 1.4, 2, 0.8, 1.4, 2, 0.8, 1.4, 2);
	  private static readonly DoubleArray VOLS = DoubleArray.of(0.21, 0.17, 0.185, 0.17, 0.15, 0.16, 0.15, 0.14, 0.14, 0.14, 0.13, 0.13);
	  private static readonly InterpolatedNodalSurface VOL_SURFACE = InterpolatedNodalSurface.of(DefaultSurfaceMetadata.of("Test"), TIMES, STRIKES, VOLS, INTERPOLATOR_2D);
	  private static readonly DoubleArray PRICES = DoubleArray.of(0.59600, 0.04868, 2.3012E-6, 0.59201, 0.06138, 4.7919E-5, 0.58812, 0.07063, 1.1365E-4, 0.58413, 0.07626, 2.4524E-4);
	  private static readonly InterpolatedNodalSurface PRICE_SURFACE = InterpolatedNodalSurface.of(DefaultSurfaceMetadata.of("Test"), TIMES, STRIKES, PRICES, INTERPOLATOR_2D);
	  private const double SPOT = 1.40;
	  private static readonly double[] TEST_STRIKES = new double[] {1.1, 1.4, 2.2};
	  private static readonly double[] TEST_TIMES = new double[] {0.1, 0.6, 1.1};
	  private const double FD_EPS = 1.0e-5;

	  private static readonly DupireLocalVolatilityCalculator CALC = new DupireLocalVolatilityCalculator();

	  public virtual void flatVolTest()
	  {
		double constantVol = 0.15;
		ConstantSurface impliedVolSurface = ConstantSurface.of("impliedVol", constantVol);
		System.Func<double, double> zeroRate = (double? x) =>
		{
	return 0.05d;
		};
		System.Func<double, double> zeroRate1 = (double? x) =>
		{
	return 0.02d;
		};
		double[] strikes = new double[] {90d, 100d, 115d};
		foreach (double strike in strikes)
		{
		  foreach (double time in TEST_TIMES)
		  {
			DeformedSurface localVolSurface = CALC.localVolatilityFromImpliedVolatility(impliedVolSurface, 100d, zeroRate, zeroRate1);
			assertEquals(localVolSurface.zValue(time, strike), constantVol);
		  }
		}
	  }

	  public virtual void test_localVolatilityFromImpliedVolatility()
	  {
		double r = 0.05;
		double q = 0.01;
		System.Func<double, double> interestRate = (double? x) =>
		{
	return r;
		};
		System.Func<double, double> dividendRate = (double? x) =>
		{
	return q;
		};
		foreach (double strike in TEST_STRIKES)
		{
		  foreach (double time in TEST_TIMES)
		  {
			double computedVol = CALC.localVolatilityFromImpliedVolatility(VOL_SURFACE, SPOT, interestRate, dividendRate).zValue(time, strike);
			double expectedVol = volFromFormula(r, q, time, strike, VOL_SURFACE);
			assertEquals(computedVol, expectedVol, FD_EPS);
			UnitParameterSensitivity computedSensi = CALC.localVolatilityFromImpliedVolatility(VOL_SURFACE, SPOT, interestRate, dividendRate).zValueParameterSensitivity(time, strike);
			for (int i = 0; i < VOLS.size(); ++i)
			{
			  InterpolatedNodalSurface surfaceUp = VOL_SURFACE.withZValues(VOLS.with(i, VOLS.get(i) + FD_EPS));
			  InterpolatedNodalSurface surfaceDw = VOL_SURFACE.withZValues(VOLS.with(i, VOLS.get(i) - FD_EPS));
			  double volUp = CALC.localVolatilityFromImpliedVolatility(surfaceUp, SPOT, interestRate, dividendRate).zValue(time, strike);
			  double volDw = CALC.localVolatilityFromImpliedVolatility(surfaceDw, SPOT, interestRate, dividendRate).zValue(time, strike);
			  double expectedSensi = 0.5 * (volUp - volDw) / FD_EPS;
			  assertEquals(computedSensi.Sensitivity.get(i), expectedSensi, FD_EPS * 10d);
			}
		  }
		}
	  }

	  public virtual void test_localVolatilityFromImpliedVolatility_smallStrike()
	  {
		double r = 0.05;
		double q = 0.01;
		System.Func<double, double> interestRate = (double? x) =>
		{
	return r;
		};
		System.Func<double, double> dividendRate = (double? x) =>
		{
	return q;
		};
		double strike = 1.0e-11;
		foreach (double time in TEST_TIMES)
		{
		  double computedVol = CALC.localVolatilityFromImpliedVolatility(VOL_SURFACE, SPOT, interestRate, dividendRate).zValue(time, strike);
		  double expectedVol = volFromFormula(r, q, time, strike, VOL_SURFACE);
		  assertEquals(computedVol, expectedVol, FD_EPS);
		  UnitParameterSensitivity computedSensi = CALC.localVolatilityFromImpliedVolatility(VOL_SURFACE, SPOT, interestRate, dividendRate).zValueParameterSensitivity(time, strike);
			for (int i = 0; i < VOLS.size(); ++i)
			{
			InterpolatedNodalSurface surfaceUp = VOL_SURFACE.withZValues(VOLS.with(i, VOLS.get(i) + FD_EPS));
			InterpolatedNodalSurface surfaceDw = VOL_SURFACE.withZValues(VOLS.with(i, VOLS.get(i) - FD_EPS));
			double volUp = CALC.localVolatilityFromImpliedVolatility(surfaceUp, SPOT, interestRate, dividendRate).zValue(time, strike);
			double volDw = CALC.localVolatilityFromImpliedVolatility(surfaceDw, SPOT, interestRate, dividendRate).zValue(time, strike);
			double expectedSensi = 0.5 * (volUp - volDw) / FD_EPS;
			assertEquals(computedSensi.Sensitivity.get(i), expectedSensi, FD_EPS * 10d);
			}
		}
	  }

	  public virtual void test_localVolatilityFromPrice()
	  {
		double r = 0.03;
		double q = 0.02;
		System.Func<double, double> interestRate = (double? x) =>
		{
	return r;
		};
		System.Func<double, double> dividendRate = (double? x) =>
		{
	return q;
		};
		foreach (double strike in TEST_STRIKES)
		{
		  foreach (double time in TEST_TIMES)
		  {
			double computedVol = CALC.localVolatilityFromPrice(PRICE_SURFACE, SPOT, interestRate, dividendRate).zValue(time, strike);
			double expectedVol = volFromFormulaPrice(r, q, time, strike, PRICE_SURFACE);
			assertEquals(computedVol, expectedVol, FD_EPS);
			UnitParameterSensitivity computedSensi = CALC.localVolatilityFromPrice(PRICE_SURFACE, SPOT, interestRate, dividendRate).zValueParameterSensitivity(time, strike);
			for (int i = 0; i < PRICES.size(); ++i)
			{
			  InterpolatedNodalSurface surfaceUp = PRICE_SURFACE.withZValues(PRICES.with(i, PRICES.get(i) + FD_EPS));
			  InterpolatedNodalSurface surfaceDw = PRICE_SURFACE.withZValues(PRICES.with(i, PRICES.get(i) - FD_EPS));
			  double priceUp = CALC.localVolatilityFromPrice(surfaceUp, SPOT, interestRate, dividendRate).zValue(time, strike);
			  double priceDw = CALC.localVolatilityFromPrice(surfaceDw, SPOT, interestRate, dividendRate).zValue(time, strike);
			  double expectedSensi = 0.5 * (priceUp - priceDw) / FD_EPS;
			  assertEquals(computedSensi.Sensitivity.get(i), expectedSensi, FD_EPS * 100d); // tiny call price
			}
		  }
		}
	  }

	  private double volFromFormula(double r, double q, double time, double strike, NodalSurface surface)
	  {
		double vol = surface.zValue(time, strike);
		double volT = 0.5 / FD_EPS * (surface.zValue(time + FD_EPS, strike) - surface.zValue(time - FD_EPS, strike));
		double volK = 0.5 / FD_EPS * (surface.zValue(time, strike + FD_EPS) - surface.zValue(time, strike - FD_EPS));
		double volKK = (surface.zValue(time, strike + FD_EPS) + surface.zValue(time, strike - FD_EPS) - 2d * vol) / FD_EPS / FD_EPS;
		double rootT = Math.Sqrt(time);
		double d1 = (Math.Log(SPOT / strike) + (r - q + 0.5 * vol * vol) * time) / vol / rootT;
		double d2 = (Math.Log(SPOT / strike) + (r - q - 0.5 * vol * vol) * time) / vol / rootT;
		double den = 1d + 2d * d1 * strike * rootT * volK + strike * strike * time * (d1 * d2 * volK * volK + vol * volKK);
		double var = (vol * vol + 2d * vol * time * (volT + (r - q) * strike * volK)) / den;
		return Math.Sqrt(var);
	  }

	  private double volFromFormulaPrice(double r, double q, double time, double strike, NodalSurface surface)
	  {
		double p = surface.zValue(time, strike);
		double pT = 0.5 / FD_EPS * (surface.zValue(time + FD_EPS, strike) - surface.zValue(time - FD_EPS, strike));
		double pK = 0.5 / FD_EPS * (surface.zValue(time, strike + FD_EPS) - surface.zValue(time, strike - FD_EPS));
		double pKK = (surface.zValue(time, strike + FD_EPS) + surface.zValue(time, strike - FD_EPS) - 2d * p) / FD_EPS / FD_EPS;
		double var = 2d * (pT + (r - q) * strike * pK + q * p) / (strike * strike * pKK);
		return Math.Sqrt(var);
	  }

	}

}