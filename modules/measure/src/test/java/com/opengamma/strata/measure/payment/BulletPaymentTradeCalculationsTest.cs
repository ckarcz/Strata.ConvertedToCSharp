/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.payment
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CashFlows = com.opengamma.strata.market.amount.CashFlows;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using DiscountingPaymentPricer = com.opengamma.strata.pricer.DiscountingPaymentPricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedBulletPaymentTrade = com.opengamma.strata.product.payment.ResolvedBulletPaymentTrade;

	/// <summary>
	/// Test <seealso cref="BulletPaymentTradeCalculations"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BulletPaymentTradeCalculationsTest
	public class BulletPaymentTradeCalculationsTest
	{

	  private static readonly ResolvedBulletPaymentTrade RTRADE = BulletPaymentTradeCalculationFunctionTest.RTRADE;
	  private static readonly RatesMarketDataLookup RATES_LOOKUP = BulletPaymentTradeCalculationFunctionTest.RATES_LOOKUP;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		ScenarioMarketData md = BulletPaymentTradeCalculationFunctionTest.marketData();
		RatesProvider provider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		DiscountingPaymentPricer pricer = DiscountingPaymentPricer.DEFAULT;
		Payment payment = RTRADE.Product.Payment;
		CurrencyAmount expectedPv = pricer.presentValue(payment, provider);
		CashFlows expectedCashFlows = pricer.cashFlows(payment, provider);
		MultiCurrencyAmount expectedCurrencyExposure = pricer.currencyExposure(payment, provider);
		CurrencyAmount expectedCurrentCash = pricer.currentCash(payment, provider);

		assertEquals(BulletPaymentTradeCalculations.DEFAULT.presentValue(RTRADE, RATES_LOOKUP, md), CurrencyScenarioArray.of(ImmutableList.of(expectedPv)));
		assertEquals(BulletPaymentTradeCalculations.DEFAULT.cashFlows(RTRADE, RATES_LOOKUP, md), ScenarioArray.of(ImmutableList.of(expectedCashFlows)));
		assertEquals(BulletPaymentTradeCalculations.DEFAULT.currencyExposure(RTRADE, RATES_LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedCurrencyExposure)));
		assertEquals(BulletPaymentTradeCalculations.DEFAULT.currentCash(RTRADE, RATES_LOOKUP, md), CurrencyScenarioArray.of(ImmutableList.of(expectedCurrentCash)));
	  }

	  public virtual void test_pv01()
	  {
		ScenarioMarketData md = BulletPaymentTradeCalculationFunctionTest.marketData();
		RatesProvider provider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		DiscountingPaymentPricer pricer = DiscountingPaymentPricer.DEFAULT;
		Payment payment = RTRADE.Product.Payment;
		PointSensitivities pvPointSens = pricer.presentValueSensitivity(payment, provider).build();
		CurrencyParameterSensitivities pvParamSens = provider.parameterSensitivity(pvPointSens);
		MultiCurrencyAmount expectedPv01Cal = pvParamSens.total().multipliedBy(1e-4);
		CurrencyParameterSensitivities expectedPv01CalBucketed = pvParamSens.multipliedBy(1e-4);

		assertEquals(BulletPaymentTradeCalculations.DEFAULT.pv01CalibratedSum(RTRADE, RATES_LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv01Cal)));
		assertEquals(BulletPaymentTradeCalculations.DEFAULT.pv01CalibratedBucketed(RTRADE, RATES_LOOKUP, md), ScenarioArray.of(ImmutableList.of(expectedPv01CalBucketed)));
	  }

	}

}