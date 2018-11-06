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
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.NATURAL_SPLINE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.NATURAL_SPLINE_NONNEGATIVITY_CUBIC;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.TIME_SQUARE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleMath = com.google.common.math.DoubleMath;
	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoublesPair = com.opengamma.strata.collect.tuple.DoublesPair;
	using ConstantSurface = com.opengamma.strata.market.surface.ConstantSurface;
	using DefaultSurfaceMetadata = com.opengamma.strata.market.surface.DefaultSurfaceMetadata;
	using DeformedSurface = com.opengamma.strata.market.surface.DeformedSurface;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;

	/// <summary>
	/// Test <seealso cref="ImpliedTrinomialTreeLocalVolatilityCalculator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ImpliedTrinomialTreeLocalVolatilityCalculatorTest
	public class ImpliedTrinomialTreeLocalVolatilityCalculatorTest
	{

	  private static readonly GridSurfaceInterpolator INTERP_LINEAR = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly GridSurfaceInterpolator INTERP_TIMESQ_LINEAR = GridSurfaceInterpolator.of(TIME_SQUARE, LINEAR);
	  private static readonly GridSurfaceInterpolator INTERP_CUBIC = GridSurfaceInterpolator.of(NATURAL_SPLINE, INTERPOLATOR, NATURAL_SPLINE, INTERPOLATOR);
	  private static readonly GridSurfaceInterpolator INTERP_CUBIC_NN = GridSurfaceInterpolator.of(NATURAL_SPLINE_NONNEGATIVITY_CUBIC, INTERPOLATOR, NATURAL_SPLINE_NONNEGATIVITY_CUBIC, INTERPOLATOR);

	  private static readonly DoubleArray TIMES = DoubleArray.of(0.25, 0.25, 0.25, 0.5, 0.5, 0.5, 0.75, 0.75, 0.75, 1, 1, 1);
	  private static readonly DoubleArray STRIKES = DoubleArray.of(0.8, 1.4, 2, 0.8, 1.4, 2, 0.8, 1.4, 2, 0.8, 1.4, 2);
	  private static readonly DoubleArray VOLS = DoubleArray.of(0.21, 0.17, 0.185, 0.17, 0.15, 0.16, 0.15, 0.14, 0.14, 0.14, 0.13, 0.13);
	  private static readonly InterpolatedNodalSurface VOL_SURFACE = InterpolatedNodalSurface.ofUnsorted(DefaultSurfaceMetadata.of("Test"), TIMES, STRIKES, VOLS, INTERP_CUBIC);
	  private static readonly DoubleArray PRICES = DoubleArray.of(0.6024819282312833, 0.0507874597232295, 2.598419834431295E-6, 0.6049279456317715, 0.06581419934686354, 5.691088908182669E-5, 0.607338423139487, 0.07752243330525914, 1.4290312009415014E-4, 0.6097138918063894, 0.0856850744439275, 3.218460178780302E-4);
	  private static readonly InterpolatedNodalSurface PRICE_SURFACE = InterpolatedNodalSurface.ofUnsorted(DefaultSurfaceMetadata.of("Test"), TIMES, STRIKES, PRICES, INTERP_CUBIC_NN);
	  private const double SPOT = 1.40;

	  public virtual void flatVolTest()
	  {
		double tol = 2.0e-2;
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
		ImpliedTrinomialTreeLocalVolatilityCalculator calc = new ImpliedTrinomialTreeLocalVolatilityCalculator(45, 1d, INTERP_TIMESQ_LINEAR);
		InterpolatedNodalSurface localVolSurface = calc.localVolatilityFromImpliedVolatility(impliedVolSurface, 100d, zeroRate, zeroRate1);
		assertEquals(localVolSurface.ZValues.Where(d => !DoubleMath.fuzzyEquals(d, constantVol, tol)).Count(), 0);
	  }

	  public virtual void flatVolPriceTest()
	  {
		double tol = 2.0e-2;
		double constantVol = 0.15;
		double spot = 100d;
		double maxTime = 1d;
		int nSteps = 9;
		ConstantSurface impliedVolSurface = ConstantSurface.of("impliedVol", constantVol);
		System.Func<double, double> zeroRate = (double? x) =>
		{
	return 0d;
		};
		System.Func<DoublesPair, ValueDerivatives> func = (DoublesPair x) =>
		{
	double price = BlackFormulaRepository.price(spot, x.Second, x.First, constantVol, true);
	return ValueDerivatives.of(price, DoubleArray.EMPTY);
		};
		DeformedSurface priceSurface = DeformedSurface.of(DefaultSurfaceMetadata.of("price"), impliedVolSurface, func);
		ImpliedTrinomialTreeLocalVolatilityCalculator calc = new ImpliedTrinomialTreeLocalVolatilityCalculator(nSteps, maxTime, INTERP_TIMESQ_LINEAR);
		InterpolatedNodalSurface localVolSurface = calc.localVolatilityFromPrice(priceSurface, spot, zeroRate, zeroRate);
		assertEquals(localVolSurface.ZValues.Where(d => !DoubleMath.fuzzyEquals(d, constantVol, tol)).Count(), 0);
	  }

	  public virtual void comparisonDupireVolTest()
	  {
		double tol = 1.0e-2;
		ImpliedTrinomialTreeLocalVolatilityCalculator calc = new ImpliedTrinomialTreeLocalVolatilityCalculator(28, 1.45d, INTERP_LINEAR);
		System.Func<double, double> interestRate = (double? x) =>
		{
	return 0.03d;
		};
		System.Func<double, double> dividendRate = (double? x) =>
		{
	return 0.01d;
		};
		InterpolatedNodalSurface resTri = calc.localVolatilityFromImpliedVolatility(VOL_SURFACE, SPOT, interestRate, dividendRate);
		DeformedSurface resDup = (new DupireLocalVolatilityCalculator()).localVolatilityFromImpliedVolatility(VOL_SURFACE, SPOT, interestRate, dividendRate);
		double[][] sampleStrikes = new double[][]
		{
			new double[] {0.7 * SPOT, SPOT, 1.1 * SPOT, 1.4 * SPOT},
			new double[] {0.5 * SPOT, 0.9 * SPOT, SPOT, 1.3 * SPOT, 1.9 * SPOT}
		};
		double[] sampleTimes = new double[] {0.8, 1.1};
		for (int i = 0; i < sampleTimes.Length; ++i)
		{
		  double time = sampleTimes[i];
		  foreach (double strike in sampleStrikes[i])
		  {
			double volTri = resTri.zValue(time, strike);
			double volDup = resDup.zValue(time, strike);
			assertEquals(volTri, volDup, tol);
		  }
		}
	  }

	  public virtual void comparisonDupirePriceTest()
	  {
		double tol = 7.0e-2;
		ImpliedTrinomialTreeLocalVolatilityCalculator calc = new ImpliedTrinomialTreeLocalVolatilityCalculator(22, 1.1d, INTERP_LINEAR);
		System.Func<double, double> interestRate = (double? x) =>
		{
	return 0.003d;
		};
		System.Func<double, double> dividendRate = (double? x) =>
		{
	return 0.01d;
		};
		InterpolatedNodalSurface resTri = calc.localVolatilityFromPrice(PRICE_SURFACE, SPOT, interestRate, dividendRate);
		DeformedSurface resDup = (new DupireLocalVolatilityCalculator()).localVolatilityFromPrice(PRICE_SURFACE, SPOT, interestRate, dividendRate);
		// limited range due to interpolation/extrapolation of price surface -> negative call/put price reached
		double[][] sampleStrikes = new double[][]
		{
			new double[] {0.95 * SPOT, 1.05 * SPOT},
			new double[] {0.9 * SPOT, SPOT, 1.1 * SPOT}
		};
		double[] sampleTimes = new double[] {0.7, 1.05};
		for (int i = 0; i < sampleTimes.Length; ++i)
		{
		  double time = sampleTimes[i];
		  foreach (double strike in sampleStrikes[i])
		  {
			double volTri = resTri.zValue(time, strike);
			double volDup = resDup.zValue(time, strike);
			assertEquals(volTri, volDup, tol);
		  }
		}
	  }

	}

}