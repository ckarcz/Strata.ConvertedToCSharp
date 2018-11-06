using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertSame;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ValueType = com.opengamma.strata.market.ValueType;
	using LogMoneynessStrike = com.opengamma.strata.market.option.LogMoneynessStrike;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using DefaultSurfaceMetadata = com.opengamma.strata.market.surface.DefaultSurfaceMetadata;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using SurfaceName = com.opengamma.strata.market.surface.SurfaceName;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;
	using GenericVolatilitySurfaceYearFractionParameterMetadata = com.opengamma.strata.pricer.common.GenericVolatilitySurfaceYearFractionParameterMetadata;
	using LegalEntityDiscountingProviderDataSets = com.opengamma.strata.pricer.datasets.LegalEntityDiscountingProviderDataSets;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using ResolvedBondFutureOption = com.opengamma.strata.product.bond.ResolvedBondFutureOption;

	/// <summary>
	/// Test <seealso cref="BlackBondFutureOptionMarginedProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BlackBondFutureOptionMarginedProductPricerTest
	public class BlackBondFutureOptionMarginedProductPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  // product
	  private static readonly ResolvedBondFutureOption FUTURE_OPTION_PRODUCT = BondDataSets.FUTURE_OPTION_PRODUCT_EUR_116.resolve(REF_DATA);
	  // curves
	  private static readonly LegalEntityDiscountingProvider RATE_PROVIDER = LegalEntityDiscountingProviderDataSets.ISSUER_REPO_ZERO_EUR;
	  // vol surface
	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly DoubleArray TIME = DoubleArray.of(0.20, 0.20, 0.20, 0.20, 0.20, 0.45, 0.45, 0.45, 0.45, 0.45);
	  private static readonly DoubleArray MONEYNESS = DoubleArray.of(-0.050, -0.005, 0.000, 0.005, 0.050, -0.050, -0.005, 0.000, 0.005, 0.050);
	  private static readonly DoubleArray VOL = DoubleArray.of(0.50, 0.49, 0.47, 0.48, 0.51, 0.45, 0.44, 0.42, 0.43, 0.46);
	  private static readonly SurfaceMetadata METADATA;
	  static BlackBondFutureOptionMarginedProductPricerTest()
	  {
		IList<GenericVolatilitySurfaceYearFractionParameterMetadata> list = new List<GenericVolatilitySurfaceYearFractionParameterMetadata>();
		int nData = TIME.size();
		for (int i = 0; i < nData; ++i)
		{
		  GenericVolatilitySurfaceYearFractionParameterMetadata parameterMetadata = GenericVolatilitySurfaceYearFractionParameterMetadata.of(TIME.get(i), LogMoneynessStrike.of(MONEYNESS.get(i)));
		  list.Add(parameterMetadata);
		}
		METADATA = DefaultSurfaceMetadata.builder().surfaceName(SurfaceName.of("GOVT1-BOND-FUT-VOL")).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.LOG_MONEYNESS).zValueType(ValueType.BLACK_VOLATILITY).parameterMetadata(list).dayCount(ACT_365F).build();
	  }
	  private static readonly InterpolatedNodalSurface SURFACE = InterpolatedNodalSurface.of(METADATA, TIME, MONEYNESS, VOL, INTERPOLATOR_2D);
	  private static readonly LocalDate VAL_DATE = RATE_PROVIDER.ValuationDate;
	  private static readonly LocalTime VAL_TIME = LocalTime.of(0, 0);
	  private static readonly ZoneId ZONE = FUTURE_OPTION_PRODUCT.Expiry.Zone;
	  private static readonly ZonedDateTime VAL_DATE_TIME = VAL_DATE.atTime(VAL_TIME).atZone(ZONE);
	  private static readonly BlackBondFutureExpiryLogMoneynessVolatilities VOLS = BlackBondFutureExpiryLogMoneynessVolatilities.of(VAL_DATE_TIME, SURFACE);
	  private const double TOL = 1.0E-13;
	  private const double EPS = 1.0e-6;
	  // pricer
	  private static readonly DiscountingBondFutureProductPricer FUTURE_PRICER = DiscountingBondFutureProductPricer.DEFAULT;
	  private static readonly BlackBondFutureOptionMarginedProductPricer OPTION_PRICER = new BlackBondFutureOptionMarginedProductPricer(FUTURE_PRICER);
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FD_CAL = new RatesFiniteDifferenceSensitivityCalculator(EPS);

	  public virtual void test_getFuturePricer()
	  {
		assertSame(OPTION_PRICER.FuturePricer, FUTURE_PRICER);
	  }

	  public virtual void test_price()
	  {
		double computed = OPTION_PRICER.price(FUTURE_OPTION_PRODUCT, RATE_PROVIDER, VOLS);
		double futurePrice = FUTURE_PRICER.price(FUTURE_OPTION_PRODUCT.UnderlyingFuture, RATE_PROVIDER);
		double strike = FUTURE_OPTION_PRODUCT.StrikePrice;
		double expiryTime = ACT_365F.relativeYearFraction(VAL_DATE, FUTURE_OPTION_PRODUCT.ExpiryDate);
		double logMoneyness = Math.Log(strike / futurePrice);
		double vol = SURFACE.zValue(expiryTime, logMoneyness);
		double expected = BlackFormulaRepository.price(futurePrice, strike, expiryTime, vol, true);
		assertEquals(computed, expected, TOL);
	  }

	  public virtual void test_price_from_future_price()
	  {
		double futurePrice = 1.1d;
		double computed = OPTION_PRICER.price(FUTURE_OPTION_PRODUCT, RATE_PROVIDER, VOLS, futurePrice);
		double strike = FUTURE_OPTION_PRODUCT.StrikePrice;
		double expiryTime = ACT_365F.relativeYearFraction(VAL_DATE, FUTURE_OPTION_PRODUCT.ExpiryDate);
		double logMoneyness = Math.Log(strike / futurePrice);
		double vol = SURFACE.zValue(expiryTime, logMoneyness);
		double expected = BlackFormulaRepository.price(futurePrice, strike, expiryTime, vol, true);
		assertEquals(computed, expected, TOL);
	  }

	  public virtual void test_price_from_generic_provider()
	  {
		BondFutureVolatilities vols = BlackBondFutureExpiryLogMoneynessVolatilities.of(VAL_DATE_TIME, SURFACE);
		double computed = OPTION_PRICER.price(FUTURE_OPTION_PRODUCT, RATE_PROVIDER, vols);
		double expected = OPTION_PRICER.price(FUTURE_OPTION_PRODUCT, RATE_PROVIDER, VOLS);
		assertEquals(computed, expected, TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_delta()
	  {
		double computed = OPTION_PRICER.deltaStickyStrike(FUTURE_OPTION_PRODUCT, RATE_PROVIDER, VOLS);
		double futurePrice = FUTURE_PRICER.price(FUTURE_OPTION_PRODUCT.UnderlyingFuture, RATE_PROVIDER);
		double strike = FUTURE_OPTION_PRODUCT.StrikePrice;
		double expiryTime = ACT_365F.relativeYearFraction(VAL_DATE, FUTURE_OPTION_PRODUCT.ExpiryDate);
		double logMoneyness = Math.Log(strike / futurePrice);
		double vol = SURFACE.zValue(expiryTime, logMoneyness);
		double expected = BlackFormulaRepository.delta(futurePrice, strike, expiryTime, vol, true);
		assertEquals(computed, expected, TOL);
	  }

	  public virtual void test_delta_from_future_price()
	  {
		double futurePrice = 1.1d;
		double computed = OPTION_PRICER.deltaStickyStrike(FUTURE_OPTION_PRODUCT, RATE_PROVIDER, VOLS, futurePrice);
		double strike = FUTURE_OPTION_PRODUCT.StrikePrice;
		double expiryTime = ACT_365F.relativeYearFraction(VAL_DATE, FUTURE_OPTION_PRODUCT.ExpiryDate);
		double logMoneyness = Math.Log(strike / futurePrice);
		double vol = SURFACE.zValue(expiryTime, logMoneyness);
		double expected = BlackFormulaRepository.delta(futurePrice, strike, expiryTime, vol, true);
		assertEquals(computed, expected, TOL);
	  }

	  public virtual void test_gamma()
	  {
		double computed = OPTION_PRICER.gammaStickyStrike(FUTURE_OPTION_PRODUCT, RATE_PROVIDER, VOLS);
		double futurePrice = FUTURE_PRICER.price(FUTURE_OPTION_PRODUCT.UnderlyingFuture, RATE_PROVIDER);
		double strike = FUTURE_OPTION_PRODUCT.StrikePrice;
		double expiryTime = ACT_365F.relativeYearFraction(VAL_DATE, FUTURE_OPTION_PRODUCT.ExpiryDate);
		double logMoneyness = Math.Log(strike / futurePrice);
		double vol = SURFACE.zValue(expiryTime, logMoneyness);
		double expected = BlackFormulaRepository.gamma(futurePrice, strike, expiryTime, vol);
		assertEquals(computed, expected, TOL);
	  }

	  public virtual void test_gamma_from_future_price()
	  {
		double futurePrice = 1.1d;
		double computed = OPTION_PRICER.gammaStickyStrike(FUTURE_OPTION_PRODUCT, RATE_PROVIDER, VOLS, futurePrice);
		double strike = FUTURE_OPTION_PRODUCT.StrikePrice;
		double expiryTime = ACT_365F.relativeYearFraction(VAL_DATE, FUTURE_OPTION_PRODUCT.ExpiryDate);
		double logMoneyness = Math.Log(strike / futurePrice);
		double vol = SURFACE.zValue(expiryTime, logMoneyness);
		double expected = BlackFormulaRepository.gamma(futurePrice, strike, expiryTime, vol);
		assertEquals(computed, expected, TOL);
	  }

	  public virtual void test_theta()
	  {
		double computed = OPTION_PRICER.theta(FUTURE_OPTION_PRODUCT, RATE_PROVIDER, VOLS);
		double futurePrice = FUTURE_PRICER.price(FUTURE_OPTION_PRODUCT.UnderlyingFuture, RATE_PROVIDER);
		double strike = FUTURE_OPTION_PRODUCT.StrikePrice;
		double expiryTime = ACT_365F.relativeYearFraction(VAL_DATE, FUTURE_OPTION_PRODUCT.ExpiryDate);
		double logMoneyness = Math.Log(strike / futurePrice);
		double vol = SURFACE.zValue(expiryTime, logMoneyness);
		double expected = BlackFormulaRepository.driftlessTheta(futurePrice, strike, expiryTime, vol);
		assertEquals(computed, expected, TOL);
	  }

	  public virtual void test_theta_from_future_price()
	  {
		double futurePrice = 1.1d;
		double computed = OPTION_PRICER.theta(FUTURE_OPTION_PRODUCT, RATE_PROVIDER, VOLS, futurePrice);
		double strike = FUTURE_OPTION_PRODUCT.StrikePrice;
		double expiryTime = ACT_365F.relativeYearFraction(VAL_DATE, FUTURE_OPTION_PRODUCT.ExpiryDate);
		double logMoneyness = Math.Log(strike / futurePrice);
		double vol = SURFACE.zValue(expiryTime, logMoneyness);
		double expected = BlackFormulaRepository.driftlessTheta(futurePrice, strike, expiryTime, vol);
		assertEquals(computed, expected, TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_priceSensitivity()
	  {
		PointSensitivities point = OPTION_PRICER.priceSensitivityRatesStickyStrike(FUTURE_OPTION_PRODUCT, RATE_PROVIDER, VOLS);
		CurrencyParameterSensitivities computed = RATE_PROVIDER.parameterSensitivity(point);
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATE_PROVIDER, (p) => CurrencyAmount.of(EUR, OPTION_PRICER.price(FUTURE_OPTION_PRODUCT, (p), VOLS)));
		double futurePrice = FUTURE_PRICER.price(FUTURE_OPTION_PRODUCT.UnderlyingFuture, RATE_PROVIDER);
		double strike = FUTURE_OPTION_PRODUCT.StrikePrice;
		double expiryTime = ACT_365F.relativeYearFraction(VAL_DATE, FUTURE_OPTION_PRODUCT.ExpiryDate);
		double logMoneyness = Math.Log(strike / futurePrice);
		double logMoneynessUp = Math.Log(strike / (futurePrice + EPS));
		double logMoneynessDw = Math.Log(strike / (futurePrice - EPS));
		double vol = SURFACE.zValue(expiryTime, logMoneyness);
		double volUp = SURFACE.zValue(expiryTime, logMoneynessUp);
		double volDw = SURFACE.zValue(expiryTime, logMoneynessDw);
		double volSensi = 0.5 * (volUp - volDw) / EPS;
		double vega = BlackFormulaRepository.vega(futurePrice, strike, expiryTime, vol);
		CurrencyParameterSensitivities sensiVol = RATE_PROVIDER.parameterSensitivity(FUTURE_PRICER.priceSensitivity(FUTURE_OPTION_PRODUCT.UnderlyingFuture, RATE_PROVIDER)).multipliedBy(-vega * volSensi);
		expected = expected.combinedWith(sensiVol);
		assertTrue(computed.equalWithTolerance(expected, 30d * EPS));
	  }

	  public virtual void test_priceSensitivity_from_future_price()
	  {
		double futurePrice = 1.1d;
		PointSensitivities point = OPTION_PRICER.priceSensitivityRatesStickyStrike(FUTURE_OPTION_PRODUCT, RATE_PROVIDER, VOLS, futurePrice);
		CurrencyParameterSensitivities computed = RATE_PROVIDER.parameterSensitivity(point);
		double delta = OPTION_PRICER.deltaStickyStrike(FUTURE_OPTION_PRODUCT, RATE_PROVIDER, VOLS, futurePrice);
		CurrencyParameterSensitivities expected = RATE_PROVIDER.parameterSensitivity(FUTURE_PRICER.priceSensitivity(FUTURE_OPTION_PRODUCT.UnderlyingFuture, RATE_PROVIDER)).multipliedBy(delta);
		assertTrue(computed.equalWithTolerance(expected, TOL));
	  }

	  public virtual void test_priceSensitivity_from_generic_provider()
	  {
		BondFutureVolatilities volProvider = BlackBondFutureExpiryLogMoneynessVolatilities.of(VAL_DATE_TIME, SURFACE);
		PointSensitivities expected = OPTION_PRICER.priceSensitivityRatesStickyStrike(FUTURE_OPTION_PRODUCT, RATE_PROVIDER, VOLS);
		PointSensitivities computed = OPTION_PRICER.priceSensitivity(FUTURE_OPTION_PRODUCT, RATE_PROVIDER, volProvider);
		assertEquals(computed, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_priceSensitivityBlackVolatility()
	  {
		BondFutureOptionSensitivity sensi = OPTION_PRICER.priceSensitivityModelParamsVolatility(FUTURE_OPTION_PRODUCT, RATE_PROVIDER, VOLS);
		testPriceSensitivityBlackVolatility(VOLS.parameterSensitivity(sensi), (p) => OPTION_PRICER.price(FUTURE_OPTION_PRODUCT, RATE_PROVIDER, (p)));
	  }

	  public virtual void test_priceSensitivityBlackVolatility_from_future_price()
	  {
		double futurePrice = 1.1d;
		BondFutureOptionSensitivity sensi = OPTION_PRICER.priceSensitivityModelParamsVolatility(FUTURE_OPTION_PRODUCT, RATE_PROVIDER, VOLS, futurePrice);
		testPriceSensitivityBlackVolatility(VOLS.parameterSensitivity(sensi), (p) => OPTION_PRICER.price(FUTURE_OPTION_PRODUCT, RATE_PROVIDER, (p), futurePrice));
	  }

	  private void testPriceSensitivityBlackVolatility(CurrencyParameterSensitivities computed, System.Func<BlackBondFutureVolatilities, double> valueFn)
	  {
		IList<ParameterMetadata> list = computed.Sensitivities.get(0).ParameterMetadata;
		int nVol = VOL.size();
		assertEquals(list.Count, nVol);
		for (int i = 0; i < nVol; ++i)
		{
		  double[] volUp = Arrays.copyOf(VOL.toArray(), nVol);
		  double[] volDw = Arrays.copyOf(VOL.toArray(), nVol);
		  volUp[i] += EPS;
		  volDw[i] -= EPS;
		  InterpolatedNodalSurface sfUp = InterpolatedNodalSurface.of(METADATA, TIME, MONEYNESS, DoubleArray.copyOf(volUp), INTERPOLATOR_2D);
		  InterpolatedNodalSurface sfDw = InterpolatedNodalSurface.of(METADATA, TIME, MONEYNESS, DoubleArray.copyOf(volDw), INTERPOLATOR_2D);
		  BlackBondFutureExpiryLogMoneynessVolatilities provUp = BlackBondFutureExpiryLogMoneynessVolatilities.of(VAL_DATE_TIME, sfUp);
		  BlackBondFutureExpiryLogMoneynessVolatilities provDw = BlackBondFutureExpiryLogMoneynessVolatilities.of(VAL_DATE_TIME, sfDw);
		  double expected = 0.5 * (valueFn(provUp) - valueFn(provDw)) / EPS;
		  int index = -1;
		  for (int j = 0; j < nVol; ++j)
		  {
			GenericVolatilitySurfaceYearFractionParameterMetadata meta = (GenericVolatilitySurfaceYearFractionParameterMetadata) list[j];
			if (meta.YearFraction == TIME.get(i) && meta.Strike.Value == MONEYNESS.get(i))
			{
			  index = j;
			  continue;
			}
		  }
		  assertEquals(computed.Sensitivities.get(0).Sensitivity.get(index), expected, EPS);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_marginIndex()
	  {
		double price = 0.12d;
		double computed = OPTION_PRICER.marginIndex(FUTURE_OPTION_PRODUCT, price);
		assertEquals(computed, price * FUTURE_OPTION_PRODUCT.UnderlyingFuture.Notional);
	  }

	  public virtual void test_marginIndexSensitivity()
	  {
		PointSensitivities point = OPTION_PRICER.priceSensitivityRatesStickyStrike(FUTURE_OPTION_PRODUCT, RATE_PROVIDER, VOLS);
		PointSensitivities computed = OPTION_PRICER.marginIndexSensitivity(FUTURE_OPTION_PRODUCT, point);
		assertEquals(computed, point.multipliedBy(FUTURE_OPTION_PRODUCT.UnderlyingFuture.Notional));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void regression_price()
	  {
		double price = OPTION_PRICER.price(FUTURE_OPTION_PRODUCT, RATE_PROVIDER, VOLS);
		assertEquals(price, 0.08916005173932573, TOL); // 2.x
	  }
	}

}