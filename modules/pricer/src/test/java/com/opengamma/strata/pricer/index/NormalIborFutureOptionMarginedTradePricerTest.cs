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
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MoneynessType = com.opengamma.strata.market.model.MoneynessType;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;
	using IborIndexRates = com.opengamma.strata.pricer.rate.IborIndexRates;
	using SimpleRatesProvider = com.opengamma.strata.pricer.rate.SimpleRatesProvider;
	using TradedPrice = com.opengamma.strata.product.TradedPrice;
	using ResolvedIborFutureOption = com.opengamma.strata.product.index.ResolvedIborFutureOption;
	using ResolvedIborFutureOptionTrade = com.opengamma.strata.product.index.ResolvedIborFutureOptionTrade;

	/// <summary>
	/// Tests <seealso cref="NormalIborFutureOptionMarginedTradePricer"/>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NormalIborFutureOptionMarginedTradePricerTest
	public class NormalIborFutureOptionMarginedTradePricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly DoubleArray TIMES = DoubleArray.of(0.25, 0.25, 0.25, 0.25, 0.5, 0.5, 0.5, 0.5, 1.0, 1.0, 1.0, 1.0);
	  private static readonly DoubleArray MONEYNESS_PRICES = DoubleArray.of(-0.02, -0.01, 0.0, 0.01, -0.02, -0.01, 0.0, 0.01, -0.02, -0.01, 0.0, 0.01);
	  private static readonly DoubleArray NORMAL_VOL = DoubleArray.of(0.01, 0.011, 0.012, 0.010, 0.011, 0.012, 0.013, 0.012, 0.012, 0.013, 0.014, 0.014);
	  private static readonly InterpolatedNodalSurface PARAMETERS_PRICE = InterpolatedNodalSurface.of(Surfaces.normalVolatilityByExpirySimpleMoneyness("Test", ACT_365F, MoneynessType.PRICE), TIMES, MONEYNESS_PRICES, NORMAL_VOL, INTERPOLATOR_2D);

	  private static readonly LocalDate VAL_DATE = date(2015, 2, 17);
	  private static readonly LocalTime VAL_TIME = LocalTime.of(13, 45);
	  private static readonly ZoneId LONDON_ZONE = ZoneId.of("Europe/London");

	  private static readonly NormalIborFutureOptionVolatilities VOL_SIMPLE_MONEY_PRICE = NormalIborFutureOptionExpirySimpleMoneynessVolatilities.of(GBP_LIBOR_2M, VAL_DATE.atTime(VAL_TIME).atZone(LONDON_ZONE), PARAMETERS_PRICE);

	  private static readonly ResolvedIborFutureOption OPTION = IborFutureDummyData.IBOR_FUTURE_OPTION_2.resolve(REF_DATA);
	  private static readonly LocalDate TRADE_DATE = date(2015, 2, 16);
	  private const long OPTION_QUANTITY = 12345;
	  private const double TRADE_PRICE = 0.0100;
	  private static readonly ResolvedIborFutureOptionTrade FUTURE_OPTION_TRADE_TD = ResolvedIborFutureOptionTrade.builder().product(OPTION).quantity(OPTION_QUANTITY).tradedPrice(TradedPrice.of(VAL_DATE, TRADE_PRICE)).build();
	  private static readonly ResolvedIborFutureOptionTrade FUTURE_OPTION_TRADE = ResolvedIborFutureOptionTrade.builder().product(OPTION).quantity(OPTION_QUANTITY).tradedPrice(TradedPrice.of(TRADE_DATE, TRADE_PRICE)).build();

	  private const double RATE = 0.015;

	  private static readonly DiscountingIborFutureProductPricer FUTURE_PRICER = DiscountingIborFutureProductPricer.DEFAULT;
	  private static readonly NormalIborFutureOptionMarginedProductPricer OPTION_PRODUCT_PRICER = new NormalIborFutureOptionMarginedProductPricer(FUTURE_PRICER);
	  private static readonly NormalIborFutureOptionMarginedTradePricer OPTION_TRADE_PRICER = new NormalIborFutureOptionMarginedTradePricer(OPTION_PRODUCT_PRICER);

	  private const double TOLERANCE_PV = 1.0E-2;
	  private const double TOLERANCE_PV_DELTA = 1.0E-1;

	  // ----------     present value     ----------

	  public virtual void presentValue_from_option_price_trade_date()
	  {
		double optionPrice = 0.0125;
		double lastClosingPrice = 0.0150;
		CurrencyAmount pvComputed = OPTION_TRADE_PRICER.presentValue(FUTURE_OPTION_TRADE_TD, VAL_DATE, optionPrice, lastClosingPrice);
		double pvExpected = (OPTION_PRODUCT_PRICER.marginIndex(OPTION, optionPrice) - OPTION_PRODUCT_PRICER.marginIndex(OPTION, TRADE_PRICE)) * OPTION_QUANTITY;
		assertEquals(pvComputed.Amount, pvExpected, TOLERANCE_PV);
	  }

	  public virtual void presentValue_from_future_price()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider(VAL_DATE);
		prov.IborRates = mockIbor;
		when(mockIbor.rate(OPTION.UnderlyingFuture.IborRate.Observation)).thenReturn(RATE);

		double futurePrice = 0.9875;
		double lastClosingPrice = 0.0150;
		CurrencyAmount pvComputed = OPTION_TRADE_PRICER.presentValue(FUTURE_OPTION_TRADE, prov, VOL_SIMPLE_MONEY_PRICE, futurePrice, lastClosingPrice);
		double optionPrice = OPTION_PRODUCT_PRICER.price(OPTION, prov, VOL_SIMPLE_MONEY_PRICE, futurePrice);
		double pvExpected = (OPTION_PRODUCT_PRICER.marginIndex(OPTION, optionPrice) - OPTION_PRODUCT_PRICER.marginIndex(OPTION, lastClosingPrice)) * OPTION_QUANTITY;
		assertEquals(pvComputed.Amount, pvExpected, TOLERANCE_PV);
	  }

	  public virtual void presentValue_from_env()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider(VAL_DATE);
		prov.IborRates = mockIbor;
		when(mockIbor.rate(OPTION.UnderlyingFuture.IborRate.Observation)).thenReturn(RATE);

		double lastClosingPrice = 0.0150;
		CurrencyAmount pvComputed = OPTION_TRADE_PRICER.presentValue(FUTURE_OPTION_TRADE, prov, VOL_SIMPLE_MONEY_PRICE, lastClosingPrice);
		double optionPrice = OPTION_PRODUCT_PRICER.price(OPTION, prov, VOL_SIMPLE_MONEY_PRICE);
		double pvExpected = (OPTION_PRODUCT_PRICER.marginIndex(OPTION, optionPrice) - OPTION_PRODUCT_PRICER.marginIndex(OPTION, lastClosingPrice)) * OPTION_QUANTITY;
		assertEquals(pvComputed.Amount, pvExpected, TOLERANCE_PV);
	  }

	  // ----------     present value sensitivity     ----------

	  public virtual void presentValueSensitivity_from_env()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider();
		prov.IborRates = mockIbor;
		when(mockIbor.rate(OPTION.UnderlyingFuture.IborRate.Observation)).thenReturn(RATE);

		PointSensitivities psProduct = OPTION_PRODUCT_PRICER.priceSensitivityRatesStickyStrike(OPTION, prov, VOL_SIMPLE_MONEY_PRICE);
		PointSensitivities psExpected = psProduct.multipliedBy(OPTION_PRODUCT_PRICER.marginIndex(OPTION, 1) * OPTION_QUANTITY);
		PointSensitivities psComputed = OPTION_TRADE_PRICER.presentValueSensitivityRates(FUTURE_OPTION_TRADE, prov, VOL_SIMPLE_MONEY_PRICE);
		assertTrue(psComputed.equalWithTolerance(psExpected, TOLERANCE_PV_DELTA));
	  }

	  // ----------     present value normal vol sensitivity     ----------

	  public virtual void presentvalue_normalVolSensitivity_from_env()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider();
		prov.IborRates = mockIbor;
		when(mockIbor.rate(OPTION.UnderlyingFuture.IborRate.Observation)).thenReturn(RATE);

		IborFutureOptionSensitivity psProduct = OPTION_PRODUCT_PRICER.priceSensitivityModelParamsVolatility(OPTION, prov, VOL_SIMPLE_MONEY_PRICE);
		IborFutureOptionSensitivity psExpected = psProduct.withSensitivity(psProduct.Sensitivity * OPTION_PRODUCT_PRICER.marginIndex(OPTION, 1) * OPTION_QUANTITY);
		IborFutureOptionSensitivity psComputed = OPTION_TRADE_PRICER.presentValueSensitivityModelParamsVolatility(FUTURE_OPTION_TRADE, prov, VOL_SIMPLE_MONEY_PRICE);
		assertTrue(psExpected.compareKey(psComputed) == 0);
		assertEquals(psComputed.Sensitivity, psExpected.Sensitivity, TOLERANCE_PV_DELTA);
	  }

	}

}