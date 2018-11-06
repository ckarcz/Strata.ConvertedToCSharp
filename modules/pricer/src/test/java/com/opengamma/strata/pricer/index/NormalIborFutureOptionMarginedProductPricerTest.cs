/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_2M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.mock;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.when;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MoneynessType = com.opengamma.strata.market.model.MoneynessType;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;
	using EuropeanVanillaOption = com.opengamma.strata.pricer.impl.option.EuropeanVanillaOption;
	using NormalFunctionData = com.opengamma.strata.pricer.impl.option.NormalFunctionData;
	using NormalPriceFunction = com.opengamma.strata.pricer.impl.option.NormalPriceFunction;
	using IborIndexRates = com.opengamma.strata.pricer.rate.IborIndexRates;
	using SimpleRatesProvider = com.opengamma.strata.pricer.rate.SimpleRatesProvider;
	using ResolvedIborFutureOption = com.opengamma.strata.product.index.ResolvedIborFutureOption;

	/// <summary>
	/// Tests <seealso cref="NormalIborFutureOptionMarginedProductPricer"/>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NormalIborFutureOptionMarginedProductPricerTest
	public class NormalIborFutureOptionMarginedProductPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly DoubleArray TIMES = DoubleArray.of(0.25, 0.25, 0.25, 0.25, 0.50, 0.50, 0.50, 0.50, 1.00, 1.00, 1.00, 1.00);
	  private static readonly DoubleArray MONEYNESS_PRICES = DoubleArray.of(-0.02, -0.01, 0.00, 0.01, -0.02, -0.01, 0.00, 0.01, -0.02, -0.01, 0.00, 0.01);
	  private static readonly DoubleArray NORMAL_VOL = DoubleArray.of(0.01, 0.011, 0.012, 0.010, 0.011, 0.012, 0.013, 0.012, 0.012, 0.013, 0.014, 0.014);
	  private static readonly InterpolatedNodalSurface PARAMETERS_PRICE = InterpolatedNodalSurface.of(Surfaces.normalVolatilityByExpirySimpleMoneyness("Test", ACT_365F, MoneynessType.PRICE), TIMES, MONEYNESS_PRICES, NORMAL_VOL, INTERPOLATOR_2D);

	  private static readonly LocalDate VAL_DATE = date(2015, 2, 17);
	  private static readonly LocalTime VAL_TIME = LocalTime.of(13, 45);
	  private static readonly ZoneId LONDON_ZONE = ZoneId.of("Europe/London");

	  private static readonly NormalIborFutureOptionVolatilities VOL_SIMPLE_MONEY_PRICE = NormalIborFutureOptionExpirySimpleMoneynessVolatilities.of(GBP_LIBOR_2M, VAL_DATE.atTime(VAL_TIME).atZone(LONDON_ZONE), PARAMETERS_PRICE);

	  private static readonly ResolvedIborFutureOption OPTION = IborFutureDummyData.IBOR_FUTURE_OPTION_2.resolve(REF_DATA);

	  private const double RATE = 0.015;

	  private static readonly NormalPriceFunction NORMAL_FUNCTION = new NormalPriceFunction();
	  private static readonly DiscountingIborFutureProductPricer FUTURE_PRICER = DiscountingIborFutureProductPricer.DEFAULT;
	  private static readonly NormalIborFutureOptionMarginedProductPricer OPTION_PRICER = new NormalIborFutureOptionMarginedProductPricer(FUTURE_PRICER);

	  private const double TOLERANCE_PRICE = 1.0E-10;
	  private const double TOLERANCE_PRICE_DELTA = 1.0E-8;

	  // ----------     price     ----------
	  public virtual void price_from_future_price()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider();
		prov.IborRates = mockIbor;
		when(mockIbor.rate(OPTION.UnderlyingFuture.IborRate.Observation)).thenReturn(RATE);

		double futurePrice = 0.9875;
		double strike = OPTION.StrikePrice;
		double timeToExpiry = ACT_365F.relativeYearFraction(VAL_DATE, OPTION.ExpiryDate);
		double priceSimpleMoneyness = strike - futurePrice;
		double normalVol = PARAMETERS_PRICE.zValue(timeToExpiry, priceSimpleMoneyness);
		EuropeanVanillaOption option = EuropeanVanillaOption.of(strike, timeToExpiry, OPTION.PutCall);
		NormalFunctionData normalPoint = NormalFunctionData.of(futurePrice, 1.0, normalVol);
		double optionPriceExpected = NORMAL_FUNCTION.getPriceFunction(option).apply(normalPoint);
		double optionPriceComputed = OPTION_PRICER.price(OPTION, prov, VOL_SIMPLE_MONEY_PRICE, futurePrice);
		assertEquals(optionPriceComputed, optionPriceExpected, TOLERANCE_PRICE);
	  }

	  public virtual void price_from_env()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider();
		prov.IborRates = mockIbor;
		when(mockIbor.rate(OPTION.UnderlyingFuture.IborRate.Observation)).thenReturn(RATE);

		double futurePrice = 1.0 - RATE;
		double optionPriceExpected = OPTION_PRICER.price(OPTION, prov, VOL_SIMPLE_MONEY_PRICE, futurePrice);
		double optionPriceComputed = OPTION_PRICER.price(OPTION, prov, VOL_SIMPLE_MONEY_PRICE);
		assertEquals(optionPriceComputed, optionPriceExpected, TOLERANCE_PRICE);
	  }

	  // ----------     delta     ----------
	  public virtual void delta_from_future_price()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider();
		prov.IborRates = mockIbor;
		when(mockIbor.rate(OPTION.UnderlyingFuture.IborRate.Observation)).thenReturn(RATE);

		double futurePrice = 0.9875;
		double strike = OPTION.StrikePrice;
		double timeToExpiry = ACT_365F.relativeYearFraction(VAL_DATE, OPTION.ExpiryDate);
		double priceSimpleMoneyness = strike - futurePrice;
		double normalVol = PARAMETERS_PRICE.zValue(timeToExpiry, priceSimpleMoneyness);
		EuropeanVanillaOption option = EuropeanVanillaOption.of(strike, timeToExpiry, OPTION.PutCall);
		NormalFunctionData normalPoint = NormalFunctionData.of(futurePrice, 1.0, normalVol);
		double optionDeltaExpected = NORMAL_FUNCTION.getDelta(option, normalPoint);
		double optionDeltaComputed = OPTION_PRICER.deltaStickyStrike(OPTION, prov, VOL_SIMPLE_MONEY_PRICE, futurePrice);
		assertEquals(optionDeltaComputed, optionDeltaExpected, TOLERANCE_PRICE);
	  }

	  public virtual void delta_from_env()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider();
		prov.IborRates = mockIbor;
		when(mockIbor.rate(OPTION.UnderlyingFuture.IborRate.Observation)).thenReturn(RATE);

		double futurePrice = 1.0 - RATE;
		double optionDeltaExpected = OPTION_PRICER.deltaStickyStrike(OPTION, prov, VOL_SIMPLE_MONEY_PRICE, futurePrice);
		double optionDeltaComputed = OPTION_PRICER.deltaStickyStrike(OPTION, prov, VOL_SIMPLE_MONEY_PRICE);
		assertEquals(optionDeltaComputed, optionDeltaExpected, TOLERANCE_PRICE);
	  }

	  // ----------     priceSensitivity     ----------
	  public virtual void priceSensitivityStickyStrike_from_future_price()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider();
		prov.IborRates = mockIbor;
		when(mockIbor.rate(OPTION.UnderlyingFuture.IborRate.Observation)).thenReturn(RATE);

		double futurePrice = 0.9875;
		PointSensitivities futurePriceSensitivity = FUTURE_PRICER.priceSensitivity(OPTION.UnderlyingFuture, prov);
		double delta = OPTION_PRICER.deltaStickyStrike(OPTION, prov, VOL_SIMPLE_MONEY_PRICE, futurePrice);
		PointSensitivities optionPriceSensitivityExpected = futurePriceSensitivity.multipliedBy(delta);
		PointSensitivities optionPriceSensitivityComputed = OPTION_PRICER.priceSensitivityRatesStickyStrike(OPTION, prov, VOL_SIMPLE_MONEY_PRICE, futurePrice);
		assertTrue(optionPriceSensitivityExpected.equalWithTolerance(optionPriceSensitivityComputed, TOLERANCE_PRICE_DELTA));
	  }

	  public virtual void priceSensitivityStickyStrike_from_env()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider();
		prov.IborRates = mockIbor;
		when(mockIbor.rate(OPTION.UnderlyingFuture.IborRate.Observation)).thenReturn(RATE);

		PointSensitivities futurePriceSensitivity = OPTION_PRICER.FuturePricer.priceSensitivity(OPTION.UnderlyingFuture, prov);
		double delta = OPTION_PRICER.deltaStickyStrike(OPTION, prov, VOL_SIMPLE_MONEY_PRICE);
		PointSensitivities optionPriceSensitivityExpected = futurePriceSensitivity.multipliedBy(delta);
		PointSensitivities optionPriceSensitivityComputed = OPTION_PRICER.priceSensitivityRatesStickyStrike(OPTION, prov, VOL_SIMPLE_MONEY_PRICE);
		assertTrue(optionPriceSensitivityExpected.equalWithTolerance(optionPriceSensitivityComputed, TOLERANCE_PRICE_DELTA));
	  }

	  // ----------     priceSensitivityNormalVolatility     ----------
	  public virtual void priceSensitivityNormalVolatility_from_future_price()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider();
		prov.IborRates = mockIbor;
		when(mockIbor.rate(OPTION.UnderlyingFuture.IborRate.Observation)).thenReturn(RATE);

		double futurePrice = 0.9875;
		double strike = OPTION.StrikePrice;
		double timeToExpiry = ACT_365F.relativeYearFraction(VAL_DATE, OPTION.ExpiryDate);
		double priceSimpleMoneyness = strike - futurePrice;
		double normalVol = PARAMETERS_PRICE.zValue(timeToExpiry, priceSimpleMoneyness);
		EuropeanVanillaOption option = EuropeanVanillaOption.of(strike, timeToExpiry, OPTION.PutCall);
		NormalFunctionData normalPoint = NormalFunctionData.of(futurePrice, 1.0, normalVol);
		double optionVegaExpected = NORMAL_FUNCTION.getVega(option, normalPoint);
		IborFutureOptionSensitivity optionVegaComputed = OPTION_PRICER.priceSensitivityModelParamsVolatility(OPTION, prov, VOL_SIMPLE_MONEY_PRICE, futurePrice);
		assertEquals(optionVegaComputed.Sensitivity, optionVegaExpected, TOLERANCE_PRICE);
		assertEquals(optionVegaComputed.Expiry, timeToExpiry);
		assertEquals(optionVegaComputed.FixingDate, OPTION.UnderlyingFuture.IborRate.Observation.FixingDate);
		assertEquals(optionVegaComputed.StrikePrice, OPTION.StrikePrice);
		assertEquals(optionVegaComputed.FuturePrice, futurePrice);
	  }

	  public virtual void priceSensitivityNormalVolatility_from_env()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider();
		prov.IborRates = mockIbor;
		when(mockIbor.rate(OPTION.UnderlyingFuture.IborRate.Observation)).thenReturn(RATE);

		double futurePrice = 1.0 - RATE;
		IborFutureOptionSensitivity optionVegaExpected = OPTION_PRICER.priceSensitivityModelParamsVolatility(OPTION, prov, VOL_SIMPLE_MONEY_PRICE, futurePrice);
		IborFutureOptionSensitivity optionVegaComputed = OPTION_PRICER.priceSensitivityModelParamsVolatility(OPTION, prov, VOL_SIMPLE_MONEY_PRICE);
		assertTrue(optionVegaExpected.compareKey(optionVegaComputed) == 0);
		assertEquals(optionVegaComputed.Sensitivity, optionVegaExpected.Sensitivity, TOLERANCE_PRICE_DELTA);
	  }

	}

}