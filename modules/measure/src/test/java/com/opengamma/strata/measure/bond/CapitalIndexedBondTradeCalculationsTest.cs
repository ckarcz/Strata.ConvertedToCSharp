/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using DiscountingCapitalIndexedBondTradePricer = com.opengamma.strata.pricer.bond.DiscountingCapitalIndexedBondTradePricer;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedCapitalIndexedBondTrade = com.opengamma.strata.product.bond.ResolvedCapitalIndexedBondTrade;

	/// <summary>
	/// Test <seealso cref="CapitalIndexedBondTradeCalculations"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CapitalIndexedBondTradeCalculationsTest
	public class CapitalIndexedBondTradeCalculationsTest
	{

	  private static readonly ResolvedCapitalIndexedBondTrade RTRADE = CapitalIndexedBondTradeCalculationFunctionTest.RTRADE;
	  private static readonly RatesMarketDataLookup RATES_LOOKUP = CapitalIndexedBondTradeCalculationFunctionTest.RATES_LOOKUP;
	  private static readonly LegalEntityDiscountingMarketDataLookup LED_LOOKUP = CapitalIndexedBondTradeCalculationFunctionTest.LED_LOOKUP;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		ScenarioMarketData md = CapitalIndexedBondTradeCalculationFunctionTest.marketData();
		RatesProvider ratesProvider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		LegalEntityDiscountingProvider ledProvider = LED_LOOKUP.marketDataView(md.scenario(0)).discountingProvider();
		DiscountingCapitalIndexedBondTradePricer pricer = DiscountingCapitalIndexedBondTradePricer.DEFAULT;
		CurrencyAmount expectedPv = pricer.presentValue(RTRADE, ratesProvider, ledProvider);
		MultiCurrencyAmount expectedCurrencyExposure = pricer.currencyExposure(RTRADE, ratesProvider, ledProvider);
		CurrencyAmount expectedCurrentCash = pricer.currentCash(RTRADE, ratesProvider);

		assertEquals(CapitalIndexedBondTradeCalculations.DEFAULT.presentValue(RTRADE, RATES_LOOKUP, LED_LOOKUP, md), CurrencyScenarioArray.of(ImmutableList.of(expectedPv)));
		assertEquals(CapitalIndexedBondTradeCalculations.DEFAULT.currencyExposure(RTRADE, RATES_LOOKUP, LED_LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedCurrencyExposure)));
		assertEquals(CapitalIndexedBondTradeCalculations.DEFAULT.currentCash(RTRADE, RATES_LOOKUP, LED_LOOKUP, md), CurrencyScenarioArray.of(ImmutableList.of(expectedCurrentCash)));
	  }

	  public virtual void test_pv01()
	  {
		ScenarioMarketData md = CapitalIndexedBondTradeCalculationFunctionTest.marketData();
		RatesProvider ratesProvider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		LegalEntityDiscountingProvider ledProvider = LED_LOOKUP.marketDataView(md.scenario(0)).discountingProvider();
		DiscountingCapitalIndexedBondTradePricer pricer = DiscountingCapitalIndexedBondTradePricer.DEFAULT;
		PointSensitivities pvPointSens = pricer.presentValueSensitivity(RTRADE, ratesProvider, ledProvider);
		CurrencyParameterSensitivities pvParamSens = ledProvider.parameterSensitivity(pvPointSens);
		MultiCurrencyAmount expectedPv01Cal = pvParamSens.total().multipliedBy(1e-4);
		CurrencyParameterSensitivities expectedPv01CalBucketed = pvParamSens.multipliedBy(1e-4);

		assertEquals(CapitalIndexedBondTradeCalculations.DEFAULT.pv01CalibratedSum(RTRADE, RATES_LOOKUP, LED_LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv01Cal)));
		assertEquals(CapitalIndexedBondTradeCalculations.DEFAULT.pv01CalibratedBucketed(RTRADE, RATES_LOOKUP, LED_LOOKUP, md), ScenarioArray.of(ImmutableList.of(expectedPv01CalBucketed)));
	  }

	}

}