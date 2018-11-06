using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.NOTIONAL_EXCHANGE_REC_GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.mock;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.when;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SimpleRatesProvider = com.opengamma.strata.pricer.rate.SimpleRatesProvider;
	using NotionalExchange = com.opengamma.strata.product.swap.NotionalExchange;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingNotionalExchangePricerTest
	public class DiscountingNotionalExchangePricerTest
	{

	  private static readonly LocalDate VAL_DATE = NOTIONAL_EXCHANGE_REC_GBP.PaymentDate.minusDays(90);
	  private static readonly DayCount DAY_COUNT = DayCounts.ACT_360;
	  private const double DISCOUNT_FACTOR = 0.98d;
	  private const double TOLERANCE = 1.0e-10;

	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.DOUBLE_QUADRATIC;
	  private static readonly Curve DISCOUNT_CURVE_GBP;
	  static DiscountingNotionalExchangePricerTest()
	  {
		DoubleArray time_gbp = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 3.0, 4.0, 5.0, 10.0);
		DoubleArray rate_gbp = DoubleArray.of(0.0160, 0.0135, 0.0160, 0.0185, 0.0185, 0.0195, 0.0200, 0.0210);
		DISCOUNT_CURVE_GBP = InterpolatedNodalCurve.of(Curves.zeroRates("GBP-Discount", ACT_ACT_ISDA), time_gbp, rate_gbp, INTERPOLATOR);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		SimpleRatesProvider prov = createProvider(NOTIONAL_EXCHANGE_REC_GBP);

		DiscountingNotionalExchangePricer test = DiscountingNotionalExchangePricer.DEFAULT;
		double calculated = test.presentValue(NOTIONAL_EXCHANGE_REC_GBP, prov);
		assertEquals(calculated, NOTIONAL_EXCHANGE_REC_GBP.PaymentAmount.Amount * DISCOUNT_FACTOR, 0d);
	  }

	  public virtual void test_forecastValue()
	  {
		SimpleRatesProvider prov = createProvider(NOTIONAL_EXCHANGE_REC_GBP);

		DiscountingNotionalExchangePricer test = DiscountingNotionalExchangePricer.DEFAULT;
		double calculated = test.forecastValue(NOTIONAL_EXCHANGE_REC_GBP, prov);
		assertEquals(calculated, NOTIONAL_EXCHANGE_REC_GBP.PaymentAmount.Amount, 0d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity()
	  {
		SimpleRatesProvider prov = createProvider(NOTIONAL_EXCHANGE_REC_GBP);

		DiscountingNotionalExchangePricer test = DiscountingNotionalExchangePricer.DEFAULT;
		PointSensitivities senseComputed = test.presentValueSensitivity(NOTIONAL_EXCHANGE_REC_GBP, prov).build();

		double eps = 1.0e-7;
		PointSensitivities senseExpected = PointSensitivities.of(dscSensitivityFD(prov, NOTIONAL_EXCHANGE_REC_GBP, eps));
		assertTrue(senseComputed.equalWithTolerance(senseExpected, NOTIONAL_EXCHANGE_REC_GBP.PaymentAmount.Amount * eps));
	  }

	  public virtual void test_forecastValueSensitivity()
	  {
		SimpleRatesProvider prov = createProvider(NOTIONAL_EXCHANGE_REC_GBP);

		DiscountingNotionalExchangePricer test = DiscountingNotionalExchangePricer.DEFAULT;
		PointSensitivities senseComputed = test.forecastValueSensitivity(NOTIONAL_EXCHANGE_REC_GBP, prov).build();

		double eps = 1.0e-12;
		PointSensitivities senseExpected = PointSensitivities.empty();
		assertTrue(senseComputed.equalWithTolerance(senseExpected, NOTIONAL_EXCHANGE_REC_GBP.PaymentAmount.Amount * eps));
	  }

	  private IList<ZeroRateSensitivity> dscSensitivityFD(RatesProvider provider, NotionalExchange @event, double eps)
	  {
		Currency currency = @event.Currency;
		LocalDate paymentDate = @event.PaymentDate;
		double discountFactor = provider.discountFactor(currency, paymentDate);
		double paymentTime = DAY_COUNT.relativeYearFraction(VAL_DATE, paymentDate);
		RatesProvider provUp = mock(typeof(RatesProvider));
		RatesProvider provDw = mock(typeof(RatesProvider));
		when(provUp.ValuationDate).thenReturn(VAL_DATE);
		when(provUp.discountFactor(currency, paymentDate)).thenReturn(discountFactor * Math.Exp(-eps * paymentTime));
		when(provDw.ValuationDate).thenReturn(VAL_DATE);
		when(provDw.discountFactor(currency, paymentDate)).thenReturn(discountFactor * Math.Exp(eps * paymentTime));
		DiscountingNotionalExchangePricer pricer = DiscountingNotionalExchangePricer.DEFAULT;
		double pvUp = pricer.presentValue(@event, provUp);
		double pvDw = pricer.presentValue(@event, provDw);
		double res = 0.5 * (pvUp - pvDw) / eps;
		IList<ZeroRateSensitivity> zeroRateSensi = new List<ZeroRateSensitivity>();
		zeroRateSensi.Add(ZeroRateSensitivity.of(currency, paymentTime, res));
		return zeroRateSensi;
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_explainPresentValue()
	  {
		SimpleRatesProvider prov = createProvider(NOTIONAL_EXCHANGE_REC_GBP);

		DiscountingNotionalExchangePricer test = DiscountingNotionalExchangePricer.DEFAULT;
		ExplainMapBuilder builder = ExplainMap.builder();
		test.explainPresentValue(NOTIONAL_EXCHANGE_REC_GBP, prov, builder);
		ExplainMap explain = builder.build();

		Currency currency = NOTIONAL_EXCHANGE_REC_GBP.Currency;
		CurrencyAmount notional = NOTIONAL_EXCHANGE_REC_GBP.PaymentAmount;
		assertEquals(explain.get(ExplainKey.ENTRY_TYPE).get(), "NotionalExchange");
		assertEquals(explain.get(ExplainKey.PAYMENT_DATE).get(), NOTIONAL_EXCHANGE_REC_GBP.PaymentDate);
		assertEquals(explain.get(ExplainKey.PAYMENT_CURRENCY).get(), currency);
		assertEquals(explain.get(ExplainKey.TRADE_NOTIONAL).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.TRADE_NOTIONAL).get().Amount, notional.Amount, TOLERANCE);
		assertEquals(explain.get(ExplainKey.DISCOUNT_FACTOR).Value, DISCOUNT_FACTOR, TOLERANCE);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Amount, notional.Amount, TOLERANCE);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Amount, notional.Amount * DISCOUNT_FACTOR, TOLERANCE);
	  }

	  public virtual void test_explainPresentValue_paymentDateInPast()
	  {
		SimpleRatesProvider prov = createProvider(NOTIONAL_EXCHANGE_REC_GBP);
		prov.ValuationDate = VAL_DATE.plusYears(1);

		DiscountingNotionalExchangePricer test = DiscountingNotionalExchangePricer.DEFAULT;
		ExplainMapBuilder builder = ExplainMap.builder();
		test.explainPresentValue(NOTIONAL_EXCHANGE_REC_GBP, prov, builder);
		ExplainMap explain = builder.build();

		Currency currency = NOTIONAL_EXCHANGE_REC_GBP.Currency;
		CurrencyAmount notional = NOTIONAL_EXCHANGE_REC_GBP.PaymentAmount;
		assertEquals(explain.get(ExplainKey.ENTRY_TYPE).get(), "NotionalExchange");
		assertEquals(explain.get(ExplainKey.PAYMENT_DATE).get(), NOTIONAL_EXCHANGE_REC_GBP.PaymentDate);
		assertEquals(explain.get(ExplainKey.PAYMENT_CURRENCY).get(), currency);
		assertEquals(explain.get(ExplainKey.TRADE_NOTIONAL).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.TRADE_NOTIONAL).get().Amount, notional.Amount, TOLERANCE);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Amount, 0d, TOLERANCE);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Amount, 0d * DISCOUNT_FACTOR, TOLERANCE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currencyExposure()
	  {
		ImmutableRatesProvider prov = ImmutableRatesProvider.builder(VAL_DATE).discountCurve(GBP, DISCOUNT_CURVE_GBP).build();
		DiscountingNotionalExchangePricer test = DiscountingNotionalExchangePricer.DEFAULT;
		MultiCurrencyAmount computed = test.currencyExposure(NOTIONAL_EXCHANGE_REC_GBP, prov);
		PointSensitivities point = test.presentValueSensitivity(NOTIONAL_EXCHANGE_REC_GBP, prov).build();
		MultiCurrencyAmount expected = prov.currencyExposure(point).plus(CurrencyAmount.of(NOTIONAL_EXCHANGE_REC_GBP.Currency, test.presentValue(NOTIONAL_EXCHANGE_REC_GBP, prov)));
		assertEquals(computed, expected);
	  }

	  public virtual void test_currentCash_zero()
	  {
		ImmutableRatesProvider prov = ImmutableRatesProvider.builder(VAL_DATE).discountCurve(GBP, DISCOUNT_CURVE_GBP).build();
		DiscountingNotionalExchangePricer test = DiscountingNotionalExchangePricer.DEFAULT;
		double computed = test.currentCash(NOTIONAL_EXCHANGE_REC_GBP, prov);
		assertEquals(computed, 0d);
	  }

	  public virtual void test_currentCash_onPayment()
	  {
		ImmutableRatesProvider prov = ImmutableRatesProvider.builder(NOTIONAL_EXCHANGE_REC_GBP.PaymentDate).discountCurve(GBP, DISCOUNT_CURVE_GBP).build();
		DiscountingNotionalExchangePricer test = DiscountingNotionalExchangePricer.DEFAULT;
		double notional = NOTIONAL_EXCHANGE_REC_GBP.PaymentAmount.Amount;
		double computed = test.currentCash(NOTIONAL_EXCHANGE_REC_GBP, prov);
		assertEquals(computed, notional);
	  }

	  //-------------------------------------------------------------------------
	  // creates a simple provider
	  private SimpleRatesProvider createProvider(NotionalExchange ne)
	  {
		LocalDate paymentDate = ne.PaymentDate;
		double paymentTime = DAY_COUNT.relativeYearFraction(VAL_DATE, paymentDate);
		Currency currency = ne.Currency;

		DiscountFactors mockDf = mock(typeof(DiscountFactors));
		when(mockDf.discountFactor(paymentDate)).thenReturn(DISCOUNT_FACTOR);
		ZeroRateSensitivity sens = ZeroRateSensitivity.of(currency, paymentTime, -DISCOUNT_FACTOR * paymentTime);
		when(mockDf.zeroRatePointSensitivity(paymentDate)).thenReturn(sens);
		SimpleRatesProvider prov = new SimpleRatesProvider(VAL_DATE, mockDf);
		prov.DayCount = DAY_COUNT;
		return prov;
	  }

	}

}