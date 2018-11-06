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
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
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
	using ResolvedBondFutureOptionTrade = com.opengamma.strata.product.bond.ResolvedBondFutureOptionTrade;

	/// <summary>
	/// Test <seealso cref="BlackBondFutureOptionMarginedTradePricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BlackBondFutureOptionMarginedTradePricerTest
	public class BlackBondFutureOptionMarginedTradePricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  // product and trade
	  private static readonly ResolvedBondFutureOption OPTION_PRODUCT = BondDataSets.FUTURE_OPTION_PRODUCT_EUR_115.resolve(REF_DATA);
	  private static readonly ResolvedBondFutureOptionTrade OPTION_TRADE = BondDataSets.FUTURE_OPTION_TRADE_EUR.resolve(REF_DATA);
	  private const double NOTIONAL = BondDataSets.NOTIONAL_EUR;
	  private const long QUANTITY = BondDataSets.QUANTITY_EUR;
	  // curves
	  private static readonly LegalEntityDiscountingProvider RATE_PROVIDER = LegalEntityDiscountingProviderDataSets.ISSUER_REPO_ZERO_EUR;
	  // vol surface
	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly DoubleArray TIME = DoubleArray.of(0.20, 0.20, 0.20, 0.20, 0.20, 0.45, 0.45, 0.45, 0.45, 0.45);
	  private static readonly DoubleArray MONEYNESS = DoubleArray.of(-0.050, -0.005, 0.000, 0.005, 0.050, -0.050, -0.005, 0.000, 0.005, 0.050);
	  private static readonly DoubleArray VOL = DoubleArray.of(0.50, 0.49, 0.47, 0.48, 0.51, 0.45, 0.44, 0.42, 0.43, 0.46);
	  private static readonly SurfaceMetadata METADATA;
	  static BlackBondFutureOptionMarginedTradePricerTest()
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
	  private static readonly ZoneId ZONE = OPTION_PRODUCT.Expiry.Zone;
	  private static readonly ZonedDateTime VAL_DATE_TIME = VAL_DATE.atTime(VAL_TIME).atZone(ZONE);
	  private static readonly BlackBondFutureExpiryLogMoneynessVolatilities VOLS = BlackBondFutureExpiryLogMoneynessVolatilities.of(VAL_DATE_TIME, SURFACE);
	  private const double REFERENCE_PRICE = 0.01;

	  private const double TOL = 1.0E-13;
	  private const double EPS = 1.0e-6;
	  // pricer
	  private static readonly DiscountingBondFutureProductPricer FUTURE_PRICER = DiscountingBondFutureProductPricer.DEFAULT;
	  private static readonly BlackBondFutureOptionMarginedProductPricer OPTION_PRODUCT_PRICER = new BlackBondFutureOptionMarginedProductPricer(FUTURE_PRICER);
	  private static readonly BlackBondFutureOptionMarginedTradePricer OPTION_TRADE_PRICER = new BlackBondFutureOptionMarginedTradePricer(OPTION_PRODUCT_PRICER);
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FD_CAL = new RatesFiniteDifferenceSensitivityCalculator(EPS);

	  public virtual void test_presentValue()
	  {
		CurrencyAmount computed = OPTION_TRADE_PRICER.presentValue(OPTION_TRADE, RATE_PROVIDER, VOLS, REFERENCE_PRICE);
		double expected = (OPTION_PRODUCT_PRICER.price(OPTION_PRODUCT, RATE_PROVIDER, VOLS) - REFERENCE_PRICE) * NOTIONAL * QUANTITY;
		assertEquals(computed.Currency, Currency.EUR);
		assertEquals(computed.Amount, expected, TOL * NOTIONAL * QUANTITY);
	  }

	  public virtual void test_presentValue_from_future_price()
	  {
		double futurePrice = 0.975d;
		CurrencyAmount computed = OPTION_TRADE_PRICER.presentValue(OPTION_TRADE, RATE_PROVIDER, VOLS, futurePrice, REFERENCE_PRICE);
		double expected = NOTIONAL * QUANTITY * (OPTION_PRODUCT_PRICER.price(OPTION_PRODUCT, RATE_PROVIDER, VOLS, futurePrice) - REFERENCE_PRICE);
		assertEquals(computed.Currency, Currency.EUR);
		assertEquals(computed.Amount, expected, TOL * NOTIONAL * QUANTITY);
	  }

	  public virtual void test_presentValue_from_prices_date()
	  {
		double currentPrice = 0.0325;
		double lastClosingPrice = 0.03;
		LocalDate valuationDate1 = LocalDate.of(2014, 3, 30); // before trade date
		CurrencyAmount computed1 = OPTION_TRADE_PRICER.presentValue(OPTION_TRADE, valuationDate1, currentPrice, lastClosingPrice);
		double expected2 = NOTIONAL * QUANTITY * (currentPrice - lastClosingPrice);
		assertEquals(computed1.Currency, Currency.EUR);
		assertEquals(computed1.Amount, expected2, TOL * NOTIONAL * QUANTITY);
		LocalDate valuationDate2 = LocalDate.of(2014, 3, 31); // equal to trade date
		CurrencyAmount computed2 = OPTION_TRADE_PRICER.presentValue(OPTION_TRADE, valuationDate2, currentPrice, lastClosingPrice);
		double expected = NOTIONAL * QUANTITY * (currentPrice - OPTION_TRADE.TradedPrice.get().Price);
		assertEquals(computed2.Currency, Currency.EUR);
		assertEquals(computed2.Amount, expected, TOL * NOTIONAL * QUANTITY);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivityBlackVolatility()
	  {
		BondFutureOptionSensitivity sensi = OPTION_TRADE_PRICER.presentValueSensitivityModelParamsVolatility(OPTION_TRADE, RATE_PROVIDER, VOLS);
		testPriceSensitivityBlackVolatility(VOLS.parameterSensitivity(sensi), (p) => OPTION_TRADE_PRICER.presentValue(OPTION_TRADE, RATE_PROVIDER, (p), REFERENCE_PRICE).Amount);
	  }

	  public virtual void test_presentValueSensitivityBlackVolatility_from_future_price()
	  {
		double futurePrice = 0.975d;
		BondFutureOptionSensitivity sensi = OPTION_TRADE_PRICER.presentValueSensitivityModelParamsVolatility(OPTION_TRADE, RATE_PROVIDER, VOLS, futurePrice);
		testPriceSensitivityBlackVolatility(VOLS.parameterSensitivity(sensi), (p) => OPTION_TRADE_PRICER.presentValue(OPTION_TRADE, RATE_PROVIDER, (p), futurePrice, REFERENCE_PRICE).Amount);
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
		  assertEquals(computed.Sensitivities.get(0).Sensitivity.get(index), expected, EPS * NOTIONAL * QUANTITY);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity()
	  {
		PointSensitivities point = OPTION_TRADE_PRICER.presentValueSensitivityRates(OPTION_TRADE, RATE_PROVIDER, VOLS);
		CurrencyParameterSensitivities computed = RATE_PROVIDER.parameterSensitivity(point);
		double futurePrice = FUTURE_PRICER.price(OPTION_PRODUCT.UnderlyingFuture, RATE_PROVIDER);
		double strike = OPTION_PRODUCT.StrikePrice;
		double expiryTime = ACT_365F.relativeYearFraction(VAL_DATE, OPTION_PRODUCT.ExpiryDate);
		double logMoneyness = Math.Log(strike / futurePrice);
		double logMoneynessUp = Math.Log(strike / (futurePrice + EPS));
		double logMoneynessDw = Math.Log(strike / (futurePrice - EPS));
		double vol = SURFACE.zValue(expiryTime, logMoneyness);
		double volUp = SURFACE.zValue(expiryTime, logMoneynessUp);
		double volDw = SURFACE.zValue(expiryTime, logMoneynessDw);
		double volSensi = 0.5 * (volUp - volDw) / EPS;
		double vega = BlackFormulaRepository.vega(futurePrice, strike, expiryTime, vol);
		CurrencyParameterSensitivities sensiVol = RATE_PROVIDER.parameterSensitivity(FUTURE_PRICER.priceSensitivity(OPTION_PRODUCT.UnderlyingFuture, RATE_PROVIDER)).multipliedBy(-vega * volSensi * NOTIONAL * QUANTITY);
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATE_PROVIDER, (p) => OPTION_TRADE_PRICER.presentValue(OPTION_TRADE, (p), VOLS, REFERENCE_PRICE));
		assertTrue(computed.equalWithTolerance(expected.combinedWith(sensiVol), 30d * EPS * NOTIONAL * QUANTITY));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currencyExposure()
	  {
		MultiCurrencyAmount ceComputed = OPTION_TRADE_PRICER.currencyExposure(OPTION_TRADE, RATE_PROVIDER, VOLS, REFERENCE_PRICE);
		CurrencyAmount pv = OPTION_TRADE_PRICER.presentValue(OPTION_TRADE, RATE_PROVIDER, VOLS, REFERENCE_PRICE);
		assertEquals(ceComputed, MultiCurrencyAmount.of(pv));
	  }

	  //-------------------------------------------------------------------------
	  // regression to 2.x
	  public virtual void regression()
	  {
		CurrencyAmount pv = OPTION_TRADE_PRICER.presentValue(OPTION_TRADE, RATE_PROVIDER, VOLS, REFERENCE_PRICE);
		assertEquals(pv.Amount, 1.0044656145806769E7, TOL * NOTIONAL * QUANTITY);
		double[] sensiRepoExpected = new double[] {9266400.007519504, 6037835.299017232, 0.0, 0.0, 0.0, 0.0};
		double[] sensiIssuerExpected = new double[] {0.0, -961498.734103331, -2189527.424010516, -3.7783587809228E7, -3.025330833183195E8, 0.0};
		PointSensitivities point = OPTION_TRADE_PRICER.presentValueSensitivityRates(OPTION_TRADE, RATE_PROVIDER, VOLS);
		CurrencyParameterSensitivities pvSensi = RATE_PROVIDER.parameterSensitivity(point);
		double[] sensiIssuerComputed = pvSensi.Sensitivities.get(0).Sensitivity.toArray();
		double[] sensiRepoComputed = pvSensi.Sensitivities.get(1).Sensitivity.toArray();
		assertEquals(sensiRepoComputed.Length, sensiRepoExpected.Length);
		assertEquals(sensiIssuerComputed.Length, sensiIssuerExpected.Length);
		for (int i = 0; i < 6; ++i)
		{
		  assertEquals(sensiRepoComputed[i], sensiRepoExpected[i], TOL * NOTIONAL * QUANTITY);
		  assertEquals(sensiIssuerComputed[i], sensiIssuerExpected[i], TOL * NOTIONAL * QUANTITY);
		}
	  }
	}

}