/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.payment
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
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
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using ResolvedBulletPayment = com.opengamma.strata.product.payment.ResolvedBulletPayment;
	using ResolvedBulletPaymentTrade = com.opengamma.strata.product.payment.ResolvedBulletPaymentTrade;

	/// <summary>
	/// Test <seealso cref="DiscountingBulletPaymentTradePricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingBulletPaymentTradePricerTest
	public class DiscountingBulletPaymentTradePricerTest
	{

	  private static readonly DiscountingBulletPaymentTradePricer PRICER = DiscountingBulletPaymentTradePricer.DEFAULT;
	  private const double DF = 0.96d;
	  private static readonly LocalDate VAL_DATE_2014_01_22 = RatesProviderFxDataSets.VAL_DATE_2014_01_22;
	  private static readonly LocalDate PAYMENT_DATE = VAL_DATE_2014_01_22.plusWeeks(8);
	  private static readonly LocalDate PAYMENT_DATE_PAST = VAL_DATE_2014_01_22.minusDays(1);
	  private const double NOTIONAL_USD = 100_000_000;
	  private static readonly CurrencyAmount AMOUNT = CurrencyAmount.of(USD, NOTIONAL_USD);
	  private static readonly ResolvedBulletPaymentTrade TRADE = ResolvedBulletPaymentTrade.of(TradeInfo.empty(), ResolvedBulletPayment.of(Payment.of(AMOUNT, PAYMENT_DATE)));
	  private static readonly ResolvedBulletPaymentTrade TRADE_PAST = ResolvedBulletPaymentTrade.of(TradeInfo.empty(), ResolvedBulletPayment.of(Payment.of(AMOUNT, PAYMENT_DATE_PAST)));

	  private static readonly ConstantCurve CURVE = ConstantCurve.of(Curves.discountFactors("Test", ACT_365F), DF);
	  private static readonly SimpleDiscountFactors DISCOUNT_FACTORS = SimpleDiscountFactors.of(USD, VAL_DATE_2014_01_22, CURVE);
	  private static readonly BaseProvider PROVIDER = new SimpleRatesProvider(VAL_DATE_2014_01_22, DISCOUNT_FACTORS);
	  private const double TOL = 1.0e-12;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue_provider()
	  {
		CurrencyAmount computed = PRICER.presentValue(TRADE, PROVIDER);
		double expected = NOTIONAL_USD * DF;
		assertEquals(computed.Amount, expected, NOTIONAL_USD * TOL);
	  }

	  public virtual void test_presentValue_provider_ended()
	  {
		CurrencyAmount computed = PRICER.presentValue(TRADE_PAST, PROVIDER);
		assertEquals(computed, CurrencyAmount.zero(USD));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_explainPresentValue_provider()
	  {
		CurrencyAmount fvExpected = AMOUNT;
		CurrencyAmount pvExpected = PRICER.presentValue(TRADE, PROVIDER);

		ExplainMap explain = PRICER.explainPresentValue(TRADE, PROVIDER);
		assertEquals(explain.get(ExplainKey.ENTRY_TYPE).get(), "Payment");
		assertEquals(explain.get(ExplainKey.PAYMENT_DATE).get(), PAYMENT_DATE);
		assertEquals(explain.get(ExplainKey.PAYMENT_CURRENCY).get(), USD);
		assertEquals(explain.get(ExplainKey.DISCOUNT_FACTOR).Value, DF, TOL);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Currency, USD);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Amount, fvExpected.Amount, TOL);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Currency, USD);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Amount, pvExpected.Amount, TOL);
	  }

	  public virtual void test_explainPresentValue_provider_ended()
	  {
		ExplainMap explain = PRICER.explainPresentValue(TRADE_PAST, PROVIDER);
		assertEquals(explain.get(ExplainKey.ENTRY_TYPE).get(), "Payment");
		assertEquals(explain.get(ExplainKey.PAYMENT_DATE).get(), PAYMENT_DATE_PAST);
		assertEquals(explain.get(ExplainKey.PAYMENT_CURRENCY).get(), USD);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Currency, USD);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Amount, 0, TOL);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Currency, USD);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Amount, 0, TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity_provider()
	  {
		PointSensitivities point = PRICER.presentValueSensitivity(TRADE, PROVIDER);
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
		PointSensitivities computed = PRICER.presentValueSensitivity(TRADE_PAST, PROVIDER);
		assertEquals(computed, PointSensitivities.empty());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_cashFlow_provider()
	  {
		CashFlow expected = CashFlow.ofForecastValue(PAYMENT_DATE, USD, NOTIONAL_USD, DF);
		assertEquals(PRICER.cashFlows(TRADE, PROVIDER), CashFlows.of(expected));
	  }

	  public virtual void test_cashFlow_provider_ended()
	  {
		assertEquals(PRICER.cashFlows(TRADE_PAST, PROVIDER), CashFlows.NONE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currencyExposure()
	  {
		assertEquals(PRICER.currencyExposure(TRADE, PROVIDER), PRICER.presentValue(TRADE, PROVIDER));
	  }

	  public virtual void test_currentCash_onDate()
	  {
		SimpleRatesProvider prov = new SimpleRatesProvider(PAYMENT_DATE, DISCOUNT_FACTORS);
		assertEquals(PRICER.currentCash(TRADE, prov), AMOUNT);
	  }

	  public virtual void test_currentCash_past()
	  {
		assertEquals(PRICER.currentCash(TRADE_PAST, PROVIDER), CurrencyAmount.zero(USD));
	  }

	}

}