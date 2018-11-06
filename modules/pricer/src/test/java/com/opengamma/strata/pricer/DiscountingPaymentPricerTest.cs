using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.CompoundedRateType.CONTINUOUS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.CompoundedRateType.PERIODIC;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using CashFlow = com.opengamma.strata.market.amount.CashFlow;
	using CashFlows = com.opengamma.strata.market.amount.CashFlows;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesProviderFxDataSets = com.opengamma.strata.pricer.fx.RatesProviderFxDataSets;
	using SimpleRatesProvider = com.opengamma.strata.pricer.rate.SimpleRatesProvider;

	/// <summary>
	/// Test <seealso cref="DiscountingPaymentPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingPaymentPricerTest
	public class DiscountingPaymentPricerTest
	{

	  private static readonly DiscountingPaymentPricer PRICER = DiscountingPaymentPricer.DEFAULT;
	  private const double DF = 0.96d;
	  private static readonly Currency USD = Currency.USD;
	  private static readonly LocalDate VAL_DATE_2014_01_22 = RatesProviderFxDataSets.VAL_DATE_2014_01_22;
	  private static readonly LocalDate PAYMENT_DATE = VAL_DATE_2014_01_22.plusWeeks(8);
	  private static readonly LocalDate PAYMENT_DATE_PAST = VAL_DATE_2014_01_22.minusDays(1);
	  private const double NOTIONAL_USD = 100_000_000;
	  private static readonly Payment PAYMENT = Payment.of(CurrencyAmount.of(USD, NOTIONAL_USD), PAYMENT_DATE);
	  private static readonly Payment PAYMENT_PAST = Payment.of(CurrencyAmount.of(USD, NOTIONAL_USD), PAYMENT_DATE_PAST);

	  private static readonly ConstantCurve CURVE = ConstantCurve.of(Curves.discountFactors("Test", ACT_365F), DF);
	  private static readonly SimpleDiscountFactors DISCOUNT_FACTORS = SimpleDiscountFactors.of(USD, VAL_DATE_2014_01_22, CURVE);
	  private static readonly BaseProvider PROVIDER = new SimpleRatesProvider(VAL_DATE_2014_01_22, DISCOUNT_FACTORS);
	  private const double Z_SPREAD = 0.02;
	  private const int PERIOD_PER_YEAR = 4;
	  private const double TOL = 1.0e-12;
	  private const double EPS = 1.0e-6;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue_provider()
	  {
		CurrencyAmount computed = PRICER.presentValue(PAYMENT, PROVIDER);
		double expected = NOTIONAL_USD * DF;
		assertEquals(computed.Amount, expected, NOTIONAL_USD * TOL);
	  }

	  public virtual void test_presentValue_provider_ended()
	  {
		CurrencyAmount computed = PRICER.presentValue(PAYMENT_PAST, PROVIDER);
		assertEquals(computed, CurrencyAmount.zero(USD));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue_df()
	  {
		CurrencyAmount computed = PRICER.presentValue(PAYMENT, DISCOUNT_FACTORS);
		double expected = NOTIONAL_USD * DF;
		assertEquals(computed.Amount, expected, NOTIONAL_USD * TOL);
	  }

	  public virtual void test_presentValue_df_ended()
	  {
		CurrencyAmount computed = PRICER.presentValue(PAYMENT_PAST, DISCOUNT_FACTORS);
		assertEquals(computed, CurrencyAmount.zero(USD));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueAmount_provider()
	  {
		double computed = PRICER.presentValueAmount(PAYMENT, PROVIDER);
		double expected = NOTIONAL_USD * DF;
		assertEquals(computed, expected, NOTIONAL_USD * TOL);
	  }

	  public virtual void test_presentValueAmount_provider_ended()
	  {
		double computed = PRICER.presentValueAmount(PAYMENT_PAST, PROVIDER);
		assertEquals(computed, 0d, 0d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueWithSpread_df_spread_continuous()
	  {
		CurrencyAmount computed = PRICER.presentValueWithSpread(PAYMENT, DISCOUNT_FACTORS, Z_SPREAD, CONTINUOUS, 0);
		double relativeYearFraction = ACT_365F.relativeYearFraction(VAL_DATE_2014_01_22, PAYMENT_DATE);
		double expected = NOTIONAL_USD * DF * Math.Exp(-Z_SPREAD * relativeYearFraction);
		assertEquals(computed.Amount, expected, NOTIONAL_USD * TOL);
	  }

	  public virtual void test_presentValueWithSpread_df_spread_periodic()
	  {
		CurrencyAmount computed = PRICER.presentValueWithSpread(PAYMENT, DISCOUNT_FACTORS, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		double relativeYearFraction = ACT_365F.relativeYearFraction(VAL_DATE_2014_01_22, PAYMENT_DATE);
		double rate = (Math.Pow(DF, -1d / PERIOD_PER_YEAR / relativeYearFraction) - 1d) * PERIOD_PER_YEAR;
		double expected = NOTIONAL_USD * discountFactorFromPeriodicallyCompoundedRate(rate + Z_SPREAD, PERIOD_PER_YEAR, relativeYearFraction);
		assertEquals(computed.Amount, expected, NOTIONAL_USD * TOL);
	  }

	  public virtual void test_presentValueWithSpread_df_ended_spread()
	  {
		CurrencyAmount computed = PRICER.presentValueWithSpread(PAYMENT_PAST, DISCOUNT_FACTORS, Z_SPREAD, PERIODIC, 3);
		assertEquals(computed, CurrencyAmount.zero(USD));
	  }

	  private double discountFactorFromPeriodicallyCompoundedRate(double rate, double periodPerYear, double time)
	  {
		return Math.Pow(1d + rate / periodPerYear, -periodPerYear * time);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_explainPresentValue_provider()
	  {
		CurrencyAmount fvExpected = PRICER.forecastValue(PAYMENT, PROVIDER);
		CurrencyAmount pvExpected = PRICER.presentValue(PAYMENT, PROVIDER);

		ExplainMap explain = PRICER.explainPresentValue(PAYMENT, PROVIDER);
		Currency currency = PAYMENT.Currency;
		assertEquals(explain.get(ExplainKey.ENTRY_TYPE).get(), "Payment");
		assertEquals(explain.get(ExplainKey.PAYMENT_DATE).get(), PAYMENT.Date);
		assertEquals(explain.get(ExplainKey.PAYMENT_CURRENCY).get(), currency);
		assertEquals(explain.get(ExplainKey.DISCOUNT_FACTOR).Value, DF, TOL);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Amount, fvExpected.Amount, TOL);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Amount, pvExpected.Amount, TOL);
	  }

	  public virtual void test_explainPresentValue_provider_ended()
	  {
		ExplainMap explain = PRICER.explainPresentValue(PAYMENT_PAST, PROVIDER);
		Currency currency = PAYMENT_PAST.Currency;
		assertEquals(explain.get(ExplainKey.ENTRY_TYPE).get(), "Payment");
		assertEquals(explain.get(ExplainKey.PAYMENT_DATE).get(), PAYMENT_PAST.Date);
		assertEquals(explain.get(ExplainKey.PAYMENT_CURRENCY).get(), currency);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Amount, 0, TOL);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Amount, 0, TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity_provider()
	  {
		PointSensitivities point = PRICER.presentValueSensitivity(PAYMENT, PROVIDER).build();
		double relativeYearFraction = ACT_365F.relativeYearFraction(VAL_DATE_2014_01_22, PAYMENT_DATE);
		double expected = -DF * relativeYearFraction * NOTIONAL_USD;
		ZeroRateSensitivity actual = (ZeroRateSensitivity) point.Sensitivities.get(0);
		assertEquals(actual.Currency, USD);
		assertEquals(actual.CurveCurrency, USD);
		assertEquals(actual.YearFraction, relativeYearFraction);
		assertEquals(actual.Sensitivity, expected, NOTIONAL_USD * TOL);
	  }

	  public virtual void test_presentValueSensitivity_provider_ended()
	  {
		PointSensitivities computed = PRICER.presentValueSensitivity(PAYMENT_PAST, PROVIDER).build();
		assertEquals(computed, PointSensitivities.empty());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity_df()
	  {
		PointSensitivities point = PRICER.presentValueSensitivity(PAYMENT, DISCOUNT_FACTORS).build();
		double relativeYearFraction = ACT_365F.relativeYearFraction(VAL_DATE_2014_01_22, PAYMENT_DATE);
		double expected = -DF * relativeYearFraction * NOTIONAL_USD;
		ZeroRateSensitivity actual = (ZeroRateSensitivity) point.Sensitivities.get(0);
		assertEquals(actual.Currency, USD);
		assertEquals(actual.CurveCurrency, USD);
		assertEquals(actual.YearFraction, relativeYearFraction);
		assertEquals(actual.Sensitivity, expected, NOTIONAL_USD * TOL);
	  }

	  public virtual void test_presentValueSensitivity_df_ended()
	  {
		PointSensitivities computed = PRICER.presentValueSensitivity(PAYMENT_PAST, DISCOUNT_FACTORS).build();
		assertEquals(computed, PointSensitivities.empty());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivityWithSpread_df_spread_continuous()
	  {
		PointSensitivities point = PRICER.presentValueSensitivityWithSpread(PAYMENT, DISCOUNT_FACTORS, Z_SPREAD, CONTINUOUS, 0).build();
		double relativeYearFraction = ACT_365F.relativeYearFraction(VAL_DATE_2014_01_22, PAYMENT_DATE);
		double expected = -DF * relativeYearFraction * NOTIONAL_USD * Math.Exp(-Z_SPREAD * relativeYearFraction);
		ZeroRateSensitivity actual = (ZeroRateSensitivity) point.Sensitivities.get(0);
		assertEquals(actual.Currency, USD);
		assertEquals(actual.CurveCurrency, USD);
		assertEquals(actual.YearFraction, relativeYearFraction);
		assertEquals(actual.Sensitivity, expected, NOTIONAL_USD * TOL);
	  }

	  public virtual void test_presentValueSensitivityWithSpread_df_spread_periodic()
	  {
		PointSensitivities point = PRICER.presentValueSensitivityWithSpread(PAYMENT, DISCOUNT_FACTORS, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR).build();
		double relativeYearFraction = ACT_365F.relativeYearFraction(VAL_DATE_2014_01_22, PAYMENT_DATE);
		double discountFactorUp = DF * Math.Exp(-EPS * relativeYearFraction);
		double discountFactorDw = DF * Math.Exp(EPS * relativeYearFraction);
		double rateUp = (Math.Pow(discountFactorUp, -1d / PERIOD_PER_YEAR / relativeYearFraction) - 1d) * PERIOD_PER_YEAR;
		double rateDw = (Math.Pow(discountFactorDw, -1d / PERIOD_PER_YEAR / relativeYearFraction) - 1d) * PERIOD_PER_YEAR;
		double expected = 0.5 * NOTIONAL_USD / EPS * (discountFactorFromPeriodicallyCompoundedRate(rateUp + Z_SPREAD, PERIOD_PER_YEAR, relativeYearFraction) - discountFactorFromPeriodicallyCompoundedRate(rateDw + Z_SPREAD, PERIOD_PER_YEAR, relativeYearFraction));
		ZeroRateSensitivity actual = (ZeroRateSensitivity) point.Sensitivities.get(0);
		assertEquals(actual.Currency, USD);
		assertEquals(actual.CurveCurrency, USD);
		assertEquals(actual.YearFraction, relativeYearFraction);
		assertEquals(actual.Sensitivity, expected, NOTIONAL_USD * EPS);
	  }

	  public virtual void test_presentValueSensitivityWithSpread_df_spread_ended()
	  {
		PointSensitivities computed = PRICER.presentValueSensitivityWithSpread(PAYMENT_PAST, DISCOUNT_FACTORS, Z_SPREAD, PERIODIC, 3).build();
		assertEquals(computed, PointSensitivities.empty());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_forecastValue_provider()
	  {
		assertEquals(PRICER.forecastValue(PAYMENT, PROVIDER).Amount, NOTIONAL_USD, 0d);
		assertEquals(PRICER.forecastValueAmount(PAYMENT, PROVIDER), NOTIONAL_USD, 0d);
	  }

	  public virtual void test_forecastValue_provider_ended()
	  {
		assertEquals(PRICER.forecastValue(PAYMENT_PAST, PROVIDER).Amount, 0d, 0d);
		assertEquals(PRICER.forecastValueAmount(PAYMENT_PAST, PROVIDER), 0d, 0d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_cashFlow_provider()
	  {
		CashFlow expected = CashFlow.ofForecastValue(PAYMENT_DATE, USD, NOTIONAL_USD, DF);
		assertEquals(PRICER.cashFlows(PAYMENT, PROVIDER), CashFlows.of(expected));
	  }

	  public virtual void test_cashFlow_provider_ended()
	  {
		assertEquals(PRICER.cashFlows(PAYMENT_PAST, PROVIDER), CashFlows.NONE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currencyExposure()
	  {
		assertEquals(PRICER.currencyExposure(PAYMENT, PROVIDER), MultiCurrencyAmount.of(PRICER.presentValue(PAYMENT, PROVIDER)));
	  }

	  public virtual void test_currentCash_onDate()
	  {
		SimpleRatesProvider prov = new SimpleRatesProvider(PAYMENT.Date, DISCOUNT_FACTORS);
		assertEquals(PRICER.currentCash(PAYMENT, prov), PAYMENT.Value);
	  }

	  public virtual void test_currentCash_past()
	  {
		assertEquals(PRICER.currentCash(PAYMENT_PAST, PROVIDER), CurrencyAmount.zero(USD));
	  }

	}

}