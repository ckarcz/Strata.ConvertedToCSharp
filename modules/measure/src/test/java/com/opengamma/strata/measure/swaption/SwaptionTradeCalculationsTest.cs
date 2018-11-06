/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.swaption
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
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SwaptionVolatilities = com.opengamma.strata.pricer.swaption.SwaptionVolatilities;
	using VolatilitySwaptionTradePricer = com.opengamma.strata.pricer.swaption.VolatilitySwaptionTradePricer;
	using ResolvedSwaptionTrade = com.opengamma.strata.product.swaption.ResolvedSwaptionTrade;

	/// <summary>
	/// Test <seealso cref="SwaptionTradeCalculations"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SwaptionTradeCalculationsTest
	public class SwaptionTradeCalculationsTest
	{

	  private static readonly ResolvedSwaptionTrade RTRADE = SwaptionTradeCalculationFunctionTest.RTRADE;
	  private static readonly RatesMarketDataLookup RATES_LOOKUP = SwaptionTradeCalculationFunctionTest.RATES_LOOKUP;
	  private static readonly SwaptionMarketDataLookup SWAPTION_LOOKUP = SwaptionTradeCalculationFunctionTest.SWAPTION_LOOKUP;
	  private static readonly SwaptionVolatilities VOLS = SwaptionTradeCalculationFunctionTest.NORMAL_VOL_SWAPTION_PROVIDER_USD;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		ScenarioMarketData md = SwaptionTradeCalculationFunctionTest.marketData();
		RatesProvider provider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		VolatilitySwaptionTradePricer pricer = VolatilitySwaptionTradePricer.DEFAULT;
		CurrencyAmount expectedPv = pricer.presentValue(RTRADE, provider, VOLS);
		MultiCurrencyAmount expectedCurrencyExposure = pricer.currencyExposure(RTRADE, provider, VOLS);
		CurrencyAmount expectedCurrentCash = pricer.currentCash(RTRADE, provider.ValuationDate);

		assertEquals(SwaptionTradeCalculations.DEFAULT.presentValue(RTRADE, RATES_LOOKUP, SWAPTION_LOOKUP, md), CurrencyScenarioArray.of(ImmutableList.of(expectedPv)));
		assertEquals(SwaptionTradeCalculations.DEFAULT.currencyExposure(RTRADE, RATES_LOOKUP, SWAPTION_LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedCurrencyExposure)));
		assertEquals(SwaptionTradeCalculations.DEFAULT.currentCash(RTRADE, RATES_LOOKUP, SWAPTION_LOOKUP, md), CurrencyScenarioArray.of(ImmutableList.of(expectedCurrentCash)));
	  }

	  public virtual void test_pv01()
	  {
		ScenarioMarketData md = SwaptionTradeCalculationFunctionTest.marketData();
		RatesProvider provider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		VolatilitySwaptionTradePricer pricer = VolatilitySwaptionTradePricer.DEFAULT;
		PointSensitivities pvPointSens = pricer.presentValueSensitivityRatesStickyStrike(RTRADE, provider, VOLS);
		CurrencyParameterSensitivities pvParamSens = provider.parameterSensitivity(pvPointSens);
		MultiCurrencyAmount expectedPv01Cal = pvParamSens.total().multipliedBy(1e-4);
		CurrencyParameterSensitivities expectedPv01CalBucketed = pvParamSens.multipliedBy(1e-4);

		assertEquals(SwaptionTradeCalculations.DEFAULT.pv01RatesCalibratedSum(RTRADE, RATES_LOOKUP, SWAPTION_LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv01Cal)));
		assertEquals(SwaptionTradeCalculations.DEFAULT.pv01RatesCalibratedBucketed(RTRADE, RATES_LOOKUP, SWAPTION_LOOKUP, md), ScenarioArray.of(ImmutableList.of(expectedPv01CalBucketed)));
	  }

	}

}