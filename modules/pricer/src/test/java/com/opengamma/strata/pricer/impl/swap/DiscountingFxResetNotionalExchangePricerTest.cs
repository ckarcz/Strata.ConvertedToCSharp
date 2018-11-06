using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.FxIndices.GBP_USD_WM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.FX_RESET_NOTIONAL_EXCHANGE_PAY_GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.FX_RESET_NOTIONAL_EXCHANGE_REC_USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.NOTIONAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.mock;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.when;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using DoubleMath = com.google.common.math.DoubleMath;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxMatrix = com.opengamma.strata.basics.currency.FxMatrix;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;
	using FxIndices = com.opengamma.strata.basics.index.FxIndices;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using FxIndexRates = com.opengamma.strata.pricer.fx.FxIndexRates;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using SimpleRatesProvider = com.opengamma.strata.pricer.rate.SimpleRatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using FxResetNotionalExchange = com.opengamma.strata.product.swap.FxResetNotionalExchange;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingFxResetNotionalExchangePricerTest
	public class DiscountingFxResetNotionalExchangePricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = LocalDate.of(2014, 6, 30);
	  private const double DISCOUNT_FACTOR = 0.98d;
	  private const double FX_RATE = 1.6d;
	  private const double TOLERANCE = 1.0e-10;
	  private static readonly FxMatrix FX_MATRIX = FxMatrix.of(GBP, USD, FX_RATE);

	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.DOUBLE_QUADRATIC;
	  private static readonly Curve DISCOUNT_CURVE_GBP;
	  private static readonly Curve DISCOUNT_CURVE_USD;
	  static DiscountingFxResetNotionalExchangePricerTest()
	  {
		DoubleArray time_gbp = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 3.0, 4.0, 5.0, 10.0);
		DoubleArray rate_gbp = DoubleArray.of(0.0160, 0.0135, 0.0160, 0.0185, 0.0185, 0.0195, 0.0200, 0.0210);
		DISCOUNT_CURVE_GBP = InterpolatedNodalCurve.of(Curves.zeroRates("GBP-Discount", ACT_ACT_ISDA), time_gbp, rate_gbp, INTERPOLATOR);
		DoubleArray time_usd = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 10.0);
		DoubleArray rate_usd = DoubleArray.of(0.0100, 0.0110, 0.0115, 0.0130, 0.0135, 0.0135);
		DISCOUNT_CURVE_USD = InterpolatedNodalCurve.of(Curves.zeroRates("USD-Discount", ACT_ACT_ISDA), time_usd, rate_usd, INTERPOLATOR);
	  }

	  private const double EPS_FD = 1.0e-7;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FD_CALCULATOR = new RatesFiniteDifferenceSensitivityCalculator(EPS_FD);

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		SimpleRatesProvider prov = createProvider(FX_RESET_NOTIONAL_EXCHANGE_REC_USD);

		DiscountingFxResetNotionalExchangePricer test = new DiscountingFxResetNotionalExchangePricer();
		double calculated = test.presentValue(FX_RESET_NOTIONAL_EXCHANGE_REC_USD, prov);
		assertEquals(calculated, FX_RESET_NOTIONAL_EXCHANGE_REC_USD.Notional * FX_RATE * DISCOUNT_FACTOR, 0d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity()
	  {
		ImmutableRatesProvider prov = ImmutableRatesProvider.builder(VAL_DATE).fxRateProvider(FX_MATRIX).discountCurve(GBP, DISCOUNT_CURVE_GBP).discountCurve(USD, DISCOUNT_CURVE_USD).build();
		FxResetNotionalExchange[] expanded = new FxResetNotionalExchange[] {FX_RESET_NOTIONAL_EXCHANGE_REC_USD, FX_RESET_NOTIONAL_EXCHANGE_PAY_GBP};
		for (int i = 0; i < 2; ++i)
		{
		  FxResetNotionalExchange fxReset = expanded[i];
		  DiscountingFxResetNotionalExchangePricer test = new DiscountingFxResetNotionalExchangePricer();

		  PointSensitivityBuilder pointSensitivityComputed = test.presentValueSensitivity(expanded[i], prov);
		  CurrencyParameterSensitivities parameterSensitivityComputed = prov.parameterSensitivity(pointSensitivityComputed.build());
		  CurrencyParameterSensitivities parameterSensitivityExpected = FD_CALCULATOR.sensitivity(prov, (p) => CurrencyAmount.of(fxReset.Currency, test.presentValue(fxReset, (p))));
		  assertTrue(parameterSensitivityComputed.equalWithTolerance(parameterSensitivityExpected, Math.Abs(expanded[i].Notional) * EPS_FD * 10.0));
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_forecastValue()
	  {
		SimpleRatesProvider prov = createProvider(FX_RESET_NOTIONAL_EXCHANGE_REC_USD);

		DiscountingFxResetNotionalExchangePricer test = new DiscountingFxResetNotionalExchangePricer();
		double calculated = test.forecastValue(FX_RESET_NOTIONAL_EXCHANGE_REC_USD, prov);
		assertEquals(calculated, FX_RESET_NOTIONAL_EXCHANGE_REC_USD.Notional * FX_RATE, 0d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_forecastValueSensitivity()
	  {
		ImmutableRatesProvider prov = ImmutableRatesProvider.builder(VAL_DATE).fxRateProvider(FX_MATRIX).discountCurve(GBP, DISCOUNT_CURVE_GBP).discountCurve(USD, DISCOUNT_CURVE_USD).build();
		FxResetNotionalExchange[] expanded = new FxResetNotionalExchange[] {FX_RESET_NOTIONAL_EXCHANGE_REC_USD, FX_RESET_NOTIONAL_EXCHANGE_PAY_GBP};
		for (int i = 0; i < 2; ++i)
		{
		  FxResetNotionalExchange fxReset = expanded[i];
		  DiscountingFxResetNotionalExchangePricer test = new DiscountingFxResetNotionalExchangePricer();

		  PointSensitivityBuilder pointSensitivityComputed = test.forecastValueSensitivity(expanded[i], prov);
		  CurrencyParameterSensitivities parameterSensitivityComputed = prov.parameterSensitivity(pointSensitivityComputed.build());
		  CurrencyParameterSensitivities parameterSensitivityExpected = FD_CALCULATOR.sensitivity(prov, (p) => CurrencyAmount.of(fxReset.Currency, test.forecastValue(fxReset, (p))));
		  assertTrue(parameterSensitivityComputed.equalWithTolerance(parameterSensitivityExpected, Math.Abs(expanded[i].Notional) * EPS_FD * 10.0));
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_explainPresentValue()
	  {
		SimpleRatesProvider prov = createProvider(FX_RESET_NOTIONAL_EXCHANGE_REC_USD);

		DiscountingFxResetNotionalExchangePricer test = new DiscountingFxResetNotionalExchangePricer();
		ExplainMapBuilder builder = ExplainMap.builder();
		test.explainPresentValue(FX_RESET_NOTIONAL_EXCHANGE_REC_USD, prov, builder);
		ExplainMap explain = builder.build();

		Currency paymentCurrency = FX_RESET_NOTIONAL_EXCHANGE_REC_USD.Currency;
		Currency notionalCurrency = FX_RESET_NOTIONAL_EXCHANGE_REC_USD.ReferenceCurrency;
		double notional = FX_RESET_NOTIONAL_EXCHANGE_REC_USD.Notional;
		double convertedNotional = notional * FX_RATE;
		assertEquals(explain.get(ExplainKey.ENTRY_TYPE).get(), "FxResetNotionalExchange");
		assertEquals(explain.get(ExplainKey.PAYMENT_DATE).get(), FX_RESET_NOTIONAL_EXCHANGE_REC_USD.PaymentDate);
		assertEquals(explain.get(ExplainKey.PAYMENT_CURRENCY).get(), paymentCurrency);
		assertEquals(explain.get(ExplainKey.TRADE_NOTIONAL).get().Currency, notionalCurrency);
		assertEquals(explain.get(ExplainKey.TRADE_NOTIONAL).get().Amount, notional, TOLERANCE);
		assertEquals(explain.get(ExplainKey.DISCOUNT_FACTOR).Value, DISCOUNT_FACTOR, TOLERANCE);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Currency, paymentCurrency);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Amount, convertedNotional, TOLERANCE);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Currency, paymentCurrency);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Amount, convertedNotional * DISCOUNT_FACTOR, TOLERANCE);
	  }

	  public virtual void test_explainPresentValue_paymentDateInPast()
	  {
		SimpleRatesProvider prov = createProvider(FX_RESET_NOTIONAL_EXCHANGE_REC_USD);
		prov.ValuationDate = VAL_DATE.plusYears(1);

		DiscountingFxResetNotionalExchangePricer test = new DiscountingFxResetNotionalExchangePricer();
		ExplainMapBuilder builder = ExplainMap.builder();
		test.explainPresentValue(FX_RESET_NOTIONAL_EXCHANGE_REC_USD, prov, builder);
		ExplainMap explain = builder.build();

		Currency paymentCurrency = FX_RESET_NOTIONAL_EXCHANGE_REC_USD.Currency;
		Currency notionalCurrency = FX_RESET_NOTIONAL_EXCHANGE_REC_USD.ReferenceCurrency;
		double notional = FX_RESET_NOTIONAL_EXCHANGE_REC_USD.Notional;
		assertEquals(explain.get(ExplainKey.ENTRY_TYPE).get(), "FxResetNotionalExchange");
		assertEquals(explain.get(ExplainKey.PAYMENT_DATE).get(), FX_RESET_NOTIONAL_EXCHANGE_REC_USD.PaymentDate);
		assertEquals(explain.get(ExplainKey.PAYMENT_CURRENCY).get(), paymentCurrency);
		assertEquals(explain.get(ExplainKey.TRADE_NOTIONAL).get().Currency, notionalCurrency);
		assertEquals(explain.get(ExplainKey.TRADE_NOTIONAL).get().Amount, notional, TOLERANCE);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Currency, paymentCurrency);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Amount, 0d, TOLERANCE);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Currency, paymentCurrency);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Amount, 0d * DISCOUNT_FACTOR, TOLERANCE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currencyExposure()
	  {
		double eps = 1.0e-14;
		ImmutableRatesProvider prov = ImmutableRatesProvider.builder(VAL_DATE).fxRateProvider(FX_MATRIX).discountCurve(GBP, DISCOUNT_CURVE_GBP).discountCurve(USD, DISCOUNT_CURVE_USD).build();
		DiscountingFxResetNotionalExchangePricer test = new DiscountingFxResetNotionalExchangePricer();
		// USD
		MultiCurrencyAmount computedUSD = test.currencyExposure(FX_RESET_NOTIONAL_EXCHANGE_REC_USD, prov);
		PointSensitivities pointUSD = test.presentValueSensitivity(FX_RESET_NOTIONAL_EXCHANGE_REC_USD, prov).build();
		MultiCurrencyAmount expectedUSD = prov.currencyExposure(pointUSD.convertedTo(USD, prov)).plus(CurrencyAmount.of(FX_RESET_NOTIONAL_EXCHANGE_REC_USD.Currency, test.presentValue(FX_RESET_NOTIONAL_EXCHANGE_REC_USD, prov)));
		assertFalse(computedUSD.contains(GBP)); // 0 GBP
		assertEquals(computedUSD.getAmount(USD).Amount, expectedUSD.getAmount(USD).Amount, eps * NOTIONAL);
		// GBP
		MultiCurrencyAmount computedGBP = test.currencyExposure(FX_RESET_NOTIONAL_EXCHANGE_PAY_GBP, prov);
		PointSensitivities pointGBP = test.presentValueSensitivity(FX_RESET_NOTIONAL_EXCHANGE_PAY_GBP, prov).build();
		MultiCurrencyAmount expectedGBP = prov.currencyExposure(pointGBP.convertedTo(GBP, prov)).plus(CurrencyAmount.of(FX_RESET_NOTIONAL_EXCHANGE_PAY_GBP.Currency, test.presentValue(FX_RESET_NOTIONAL_EXCHANGE_PAY_GBP, prov)));
		assertFalse(computedGBP.contains(USD)); // 0 USD
		assertEquals(computedGBP.getAmount(GBP).Amount, expectedGBP.getAmount(GBP).Amount, eps * NOTIONAL);
		// FD approximation
		FxMatrix fxMatrixUp = FxMatrix.of(GBP, USD, FX_RATE + EPS_FD);
		ImmutableRatesProvider provUp = ImmutableRatesProvider.builder(VAL_DATE).fxRateProvider(fxMatrixUp).discountCurve(GBP, DISCOUNT_CURVE_GBP).discountCurve(USD, DISCOUNT_CURVE_USD).build();
		double expectedFdUSD = -(test.presentValue(FX_RESET_NOTIONAL_EXCHANGE_REC_USD, provUp) - test.presentValue(FX_RESET_NOTIONAL_EXCHANGE_REC_USD, prov)) * FX_RATE * FX_RATE / EPS_FD;
		assertEquals(computedUSD.getAmount(USD).Amount, expectedFdUSD, EPS_FD * NOTIONAL);
		double expectedFdGBP = (test.presentValue(FX_RESET_NOTIONAL_EXCHANGE_PAY_GBP, provUp) - test.presentValue(FX_RESET_NOTIONAL_EXCHANGE_PAY_GBP, prov)) / EPS_FD;
		assertEquals(computedGBP.getAmount(GBP).Amount, expectedFdGBP, EPS_FD * NOTIONAL);
	  }

	  public virtual void test_currencyExposureBetweenFixingAndPayment()
	  {
		double eps = 1.0e-14;
		LocalDate valuationDate = date(2014, 6, 30);
		LocalDate paymentDate = date(2014, 7, 1);
		LocalDate fixingDate = date(2014, 6, 27);
		FxResetNotionalExchange resetNotionalUSD = FxResetNotionalExchange.of(CurrencyAmount.of(USD, NOTIONAL), paymentDate, FxIndexObservation.of(GBP_USD_WM, fixingDate, REF_DATA));
		FxResetNotionalExchange resetNotionalGBP = FxResetNotionalExchange.of(CurrencyAmount.of(GBP, -NOTIONAL), paymentDate, FxIndexObservation.of(GBP_USD_WM, fixingDate, REF_DATA));
		LocalDateDoubleTimeSeries ts = LocalDateDoubleTimeSeries.of(LocalDate.of(2014, 6, 27), 1.65);
		ImmutableRatesProvider prov = ImmutableRatesProvider.builder(valuationDate).fxRateProvider(FX_MATRIX).discountCurve(GBP, DISCOUNT_CURVE_GBP).discountCurve(USD, DISCOUNT_CURVE_USD).timeSeries(FxIndices.GBP_USD_WM, ts).build();
		DiscountingFxResetNotionalExchangePricer test = new DiscountingFxResetNotionalExchangePricer();
		// USD
		MultiCurrencyAmount computedUSD = test.currencyExposure(resetNotionalUSD, prov);
		PointSensitivities pointUSD = test.presentValueSensitivity(resetNotionalUSD, prov).build();
		MultiCurrencyAmount expectedUSD = prov.currencyExposure(pointUSD.convertedTo(USD, prov)).plus(CurrencyAmount.of(resetNotionalUSD.Currency, test.presentValue(resetNotionalUSD, prov)));
		assertFalse(computedUSD.contains(USD)); // 0 USD
		assertEquals(computedUSD.getAmount(GBP).Amount, expectedUSD.getAmount(GBP).Amount, eps * NOTIONAL);
		// GBP
		MultiCurrencyAmount computedGBP = test.currencyExposure(resetNotionalGBP, prov);
		PointSensitivities pointGBP = test.presentValueSensitivity(resetNotionalGBP, prov).build();
		MultiCurrencyAmount expectedGBP = prov.currencyExposure(pointGBP.convertedTo(GBP, prov)).plus(CurrencyAmount.of(resetNotionalGBP.Currency, test.presentValue(resetNotionalGBP, prov)));
		assertFalse(computedGBP.contains(GBP)); // 0 GBP
		assertEquals(computedGBP.getAmount(USD).Amount, expectedGBP.getAmount(USD).Amount, eps * NOTIONAL);
		// FD approximation
		FxMatrix fxMatrixUp = FxMatrix.of(GBP, USD, FX_RATE + EPS_FD);
		ImmutableRatesProvider provUp = ImmutableRatesProvider.builder(valuationDate).fxRateProvider(fxMatrixUp).discountCurve(GBP, DISCOUNT_CURVE_GBP).discountCurve(USD, DISCOUNT_CURVE_USD).timeSeries(FxIndices.GBP_USD_WM, ts).build();
		double expectedFdUSD = -(test.presentValue(resetNotionalUSD, provUp) - test.presentValue(resetNotionalUSD, prov)) * FX_RATE * FX_RATE / EPS_FD;
		assertTrue(!computedUSD.contains(USD) && DoubleMath.fuzzyEquals(expectedFdUSD, 0d, eps));
		double expectedFdGBP = (test.presentValue(resetNotionalGBP, provUp) - test.presentValue(resetNotionalGBP, prov)) / EPS_FD;
		assertTrue(!computedGBP.contains(GBP) && DoubleMath.fuzzyEquals(expectedFdGBP, 0d, eps));
	  }

	  public virtual void test_currencyExposureOnFixing_noTimeSeries()
	  {
		double eps = 1.0e-14;
		LocalDate valuationDate = date(2014, 6, 27);
		LocalDate paymentDate = date(2014, 7, 1);
		LocalDate fixingDate = date(2014, 6, 27);
		FxResetNotionalExchange resetNotionalUSD = FxResetNotionalExchange.of(CurrencyAmount.of(USD, NOTIONAL), paymentDate, FxIndexObservation.of(GBP_USD_WM, fixingDate, REF_DATA));
		FxResetNotionalExchange resetNotionalGBP = FxResetNotionalExchange.of(CurrencyAmount.of(GBP, -NOTIONAL), paymentDate, FxIndexObservation.of(GBP_USD_WM, fixingDate, REF_DATA));
		ImmutableRatesProvider prov = ImmutableRatesProvider.builder(valuationDate).fxRateProvider(FX_MATRIX).discountCurve(GBP, DISCOUNT_CURVE_GBP).discountCurve(USD, DISCOUNT_CURVE_USD).build();
		DiscountingFxResetNotionalExchangePricer test = new DiscountingFxResetNotionalExchangePricer();
		// USD
		MultiCurrencyAmount computedUSD = test.currencyExposure(resetNotionalUSD, prov);
		PointSensitivities pointUSD = test.presentValueSensitivity(resetNotionalUSD, prov).build();
		MultiCurrencyAmount expectedUSD = prov.currencyExposure(pointUSD.convertedTo(USD, prov)).plus(CurrencyAmount.of(resetNotionalUSD.Currency, test.presentValue(resetNotionalUSD, prov)));
		assertFalse(computedUSD.contains(GBP)); // 0 GBP
		assertEquals(computedUSD.getAmount(USD).Amount, expectedUSD.getAmount(USD).Amount, eps * NOTIONAL);
		// GBP
		MultiCurrencyAmount computedGBP = test.currencyExposure(resetNotionalGBP, prov);
		PointSensitivities pointGBP = test.presentValueSensitivity(resetNotionalGBP, prov).build();
		MultiCurrencyAmount expectedGBP = prov.currencyExposure(pointGBP.convertedTo(GBP, prov)).plus(CurrencyAmount.of(resetNotionalGBP.Currency, test.presentValue(resetNotionalGBP, prov)));
		assertFalse(computedGBP.contains(USD)); // 0 USD
		assertEquals(computedGBP.getAmount(GBP).Amount, expectedGBP.getAmount(GBP).Amount, eps * NOTIONAL);
		// FD approximation
		FxMatrix fxMatrixUp = FxMatrix.of(GBP, USD, FX_RATE + EPS_FD);
		ImmutableRatesProvider provUp = ImmutableRatesProvider.builder(valuationDate).fxRateProvider(fxMatrixUp).discountCurve(GBP, DISCOUNT_CURVE_GBP).discountCurve(USD, DISCOUNT_CURVE_USD).build();
		double expectedFdUSD = -(test.presentValue(resetNotionalUSD, provUp) - test.presentValue(resetNotionalUSD, prov)) * FX_RATE * FX_RATE / EPS_FD;
		assertEquals(computedUSD.getAmount(USD).Amount, expectedFdUSD, EPS_FD * NOTIONAL);
		double expectedFdGBP = (test.presentValue(resetNotionalGBP, provUp) - test.presentValue(resetNotionalGBP, prov)) / EPS_FD;
		assertEquals(computedGBP.getAmount(GBP).Amount, expectedFdGBP, EPS_FD * NOTIONAL);
	  }

	  public virtual void test_currentCash_zero()
	  {
		ImmutableRatesProvider prov = ImmutableRatesProvider.builder(VAL_DATE).fxRateProvider(FX_MATRIX).discountCurve(GBP, DISCOUNT_CURVE_GBP).discountCurve(USD, DISCOUNT_CURVE_USD).build();
		DiscountingFxResetNotionalExchangePricer test = new DiscountingFxResetNotionalExchangePricer();
		double cc = test.currentCash(FX_RESET_NOTIONAL_EXCHANGE_REC_USD, prov);
		assertEquals(cc, 0d);
	  }

	  public virtual void test_currentCash_onPayment()
	  {
		double eps = 1.0e-14;
		ImmutableRatesProvider prov = ImmutableRatesProvider.builder(FX_RESET_NOTIONAL_EXCHANGE_REC_USD.PaymentDate).fxRateProvider(FX_MATRIX).discountCurve(GBP, DISCOUNT_CURVE_GBP).discountCurve(USD, DISCOUNT_CURVE_USD).build();
		DiscountingFxResetNotionalExchangePricer test = new DiscountingFxResetNotionalExchangePricer();
		double rate = prov.fxIndexRates(FX_RESET_NOTIONAL_EXCHANGE_REC_USD.Observation.Index).rate(FX_RESET_NOTIONAL_EXCHANGE_REC_USD.Observation, FX_RESET_NOTIONAL_EXCHANGE_REC_USD.ReferenceCurrency);
		double ccUSD = test.currentCash(FX_RESET_NOTIONAL_EXCHANGE_REC_USD, prov);
		assertEquals(ccUSD, NOTIONAL * rate, eps);
		double ccGBP = test.currentCash(FX_RESET_NOTIONAL_EXCHANGE_PAY_GBP, prov);
		assertEquals(ccGBP, -NOTIONAL / rate, eps);
	  }

	  //-------------------------------------------------------------------------
	  // creates a simple provider
	  private SimpleRatesProvider createProvider(FxResetNotionalExchange ne)
	  {
		LocalDate paymentDate = ne.PaymentDate;
		double paymentTime = ACT_360.relativeYearFraction(VAL_DATE, paymentDate);
		Currency currency = ne.Currency;

		DiscountFactors mockDf = mock(typeof(DiscountFactors));
		when(mockDf.discountFactor(paymentDate)).thenReturn(DISCOUNT_FACTOR);
		ZeroRateSensitivity sens = ZeroRateSensitivity.of(currency, paymentTime, -DISCOUNT_FACTOR * paymentTime);
		when(mockDf.zeroRatePointSensitivity(paymentDate)).thenReturn(sens);
		FxIndexRates mockFxRates = mock(typeof(FxIndexRates));
		when(mockFxRates.rate(ne.Observation, ne.ReferenceCurrency)).thenReturn(FX_RATE);
		SimpleRatesProvider prov = new SimpleRatesProvider(VAL_DATE);
		prov.DiscountFactors = mockDf;
		prov.FxIndexRates = mockFxRates;
		prov.DayCount = ACT_360;
		return prov;
	  }

	}

}