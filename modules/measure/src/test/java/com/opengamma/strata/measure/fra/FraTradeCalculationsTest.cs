/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fra
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using DoubleScenarioArray = com.opengamma.strata.data.scenario.DoubleScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CashFlows = com.opengamma.strata.market.amount.CashFlows;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using DiscountingFraTradePricer = com.opengamma.strata.pricer.fra.DiscountingFraTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedFraTrade = com.opengamma.strata.product.fra.ResolvedFraTrade;

	/// <summary>
	/// Test <seealso cref="FraTradeCalculations"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FraTradeCalculationsTest
	public class FraTradeCalculationsTest
	{

	  private static readonly ResolvedFraTrade RTRADE = FraTradeCalculationFunctionTest.RTRADE;
	  private static readonly RatesMarketDataLookup RATES_LOOKUP = FraTradeCalculationFunctionTest.RATES_LOOKUP;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		ScenarioMarketData md = FraTradeCalculationFunctionTest.marketData();
		RatesProvider provider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		DiscountingFraTradePricer pricer = DiscountingFraTradePricer.DEFAULT;
		CurrencyAmount expectedPv = pricer.presentValue(RTRADE, provider);
		ExplainMap expectedExplainPv = pricer.explainPresentValue(RTRADE, provider);
		double expectedParRate = pricer.parRate(RTRADE, provider);
		double expectedParSpread = pricer.parSpread(RTRADE, provider);
		CashFlows expectedCashFlows = pricer.cashFlows(RTRADE, provider);
		MultiCurrencyAmount expectedCurrencyExposure = pricer.currencyExposure(RTRADE, provider);
		CurrencyAmount expectedCurrentCash = pricer.currentCash(RTRADE, provider);

		assertEquals(FraTradeCalculations.DEFAULT.presentValue(RTRADE, RATES_LOOKUP, md), CurrencyScenarioArray.of(ImmutableList.of(expectedPv)));
		assertEquals(FraTradeCalculations.DEFAULT.explainPresentValue(RTRADE, RATES_LOOKUP, md), ScenarioArray.of(ImmutableList.of(expectedExplainPv)));
		assertEquals(FraTradeCalculations.DEFAULT.parRate(RTRADE, RATES_LOOKUP, md), DoubleScenarioArray.of(ImmutableList.of(expectedParRate)));
		assertEquals(FraTradeCalculations.DEFAULT.parSpread(RTRADE, RATES_LOOKUP, md), DoubleScenarioArray.of(ImmutableList.of(expectedParSpread)));
		assertEquals(FraTradeCalculations.DEFAULT.cashFlows(RTRADE, RATES_LOOKUP, md), ScenarioArray.of(ImmutableList.of(expectedCashFlows)));
		assertEquals(FraTradeCalculations.DEFAULT.currencyExposure(RTRADE, RATES_LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedCurrencyExposure)));
		assertEquals(FraTradeCalculations.DEFAULT.currentCash(RTRADE, RATES_LOOKUP, md), CurrencyScenarioArray.of(ImmutableList.of(expectedCurrentCash)));
	  }

	  public virtual void test_pv01()
	  {
		ScenarioMarketData md = FraTradeCalculationFunctionTest.marketData();
		RatesProvider provider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		DiscountingFraTradePricer pricer = DiscountingFraTradePricer.DEFAULT;
		PointSensitivities pvPointSens = pricer.presentValueSensitivity(RTRADE, provider);
		CurrencyParameterSensitivities pvParamSens = provider.parameterSensitivity(pvPointSens);
		MultiCurrencyAmount expectedPv01Cal = pvParamSens.total().multipliedBy(1e-4);
		CurrencyParameterSensitivities expectedPv01CalBucketed = pvParamSens.multipliedBy(1e-4);

		assertEquals(FraTradeCalculations.DEFAULT.pv01CalibratedSum(RTRADE, RATES_LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv01Cal)));
		assertEquals(FraTradeCalculations.DEFAULT.pv01CalibratedBucketed(RTRADE, RATES_LOOKUP, md), ScenarioArray.of(ImmutableList.of(expectedPv01CalBucketed)));
	  }

	}

}