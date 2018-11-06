/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.deposit
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
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using DiscountingTermDepositTradePricer = com.opengamma.strata.pricer.deposit.DiscountingTermDepositTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedTermDepositTrade = com.opengamma.strata.product.deposit.ResolvedTermDepositTrade;

	/// <summary>
	/// Test <seealso cref="TermDepositTradeCalculations"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TermDepositTradeCalculationsTest
	public class TermDepositTradeCalculationsTest
	{

	  private static readonly ResolvedTermDepositTrade RTRADE = TermDepositTradeCalculationFunctionTest.RTRADE;
	  private static readonly RatesMarketDataLookup RATES_LOOKUP = TermDepositTradeCalculationFunctionTest.RATES_LOOKUP;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		ScenarioMarketData md = TermDepositTradeCalculationFunctionTest.marketData();
		RatesProvider provider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		DiscountingTermDepositTradePricer pricer = DiscountingTermDepositTradePricer.DEFAULT;
		CurrencyAmount expectedPv = pricer.presentValue(RTRADE, provider);
		double expectedParRate = pricer.parRate(RTRADE, provider);
		double expectedParSpread = pricer.parSpread(RTRADE, provider);
		MultiCurrencyAmount expectedCurrencyExposure = pricer.currencyExposure(RTRADE, provider);
		CurrencyAmount expectedCurrentCash = pricer.currentCash(RTRADE, provider);

		assertEquals(TermDepositTradeCalculations.DEFAULT.presentValue(RTRADE, RATES_LOOKUP, md), CurrencyScenarioArray.of(ImmutableList.of(expectedPv)));
		assertEquals(TermDepositTradeCalculations.DEFAULT.parRate(RTRADE, RATES_LOOKUP, md), DoubleScenarioArray.of(ImmutableList.of(expectedParRate)));
		assertEquals(TermDepositTradeCalculations.DEFAULT.parSpread(RTRADE, RATES_LOOKUP, md), DoubleScenarioArray.of(ImmutableList.of(expectedParSpread)));
		assertEquals(TermDepositTradeCalculations.DEFAULT.currencyExposure(RTRADE, RATES_LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedCurrencyExposure)));
		assertEquals(TermDepositTradeCalculations.DEFAULT.currentCash(RTRADE, RATES_LOOKUP, md), CurrencyScenarioArray.of(ImmutableList.of(expectedCurrentCash)));
	  }

	  public virtual void test_pv01()
	  {
		ScenarioMarketData md = TermDepositTradeCalculationFunctionTest.marketData();
		RatesProvider provider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		DiscountingTermDepositTradePricer pricer = DiscountingTermDepositTradePricer.DEFAULT;
		PointSensitivities pvPointSens = pricer.presentValueSensitivity(RTRADE, provider);
		CurrencyParameterSensitivities pvParamSens = provider.parameterSensitivity(pvPointSens);
		MultiCurrencyAmount expectedPv01Cal = pvParamSens.total().multipliedBy(1e-4);
		CurrencyParameterSensitivities expectedPv01CalBucketed = pvParamSens.multipliedBy(1e-4);

		assertEquals(TermDepositTradeCalculations.DEFAULT.pv01CalibratedSum(RTRADE, RATES_LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv01Cal)));
		assertEquals(TermDepositTradeCalculations.DEFAULT.pv01CalibratedBucketed(RTRADE, RATES_LOOKUP, md), ScenarioArray.of(ImmutableList.of(expectedPv01CalBucketed)));
	  }

	}

}