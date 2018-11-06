/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fx
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using DiscountingFxSingleTradePricer = com.opengamma.strata.pricer.fx.DiscountingFxSingleTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedFxSingleTrade = com.opengamma.strata.product.fx.ResolvedFxSingleTrade;

	/// <summary>
	/// Test <seealso cref="FxSingleTradeCalculations"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxSingleTradeCalculationsTest
	public class FxSingleTradeCalculationsTest
	{

	  private static readonly ResolvedFxSingleTrade RTRADE = FxSingleTradeCalculationFunctionTest.RTRADE;
	  private static readonly RatesMarketDataLookup RATES_LOOKUP = FxSingleTradeCalculationFunctionTest.RATES_LOOKUP;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		ScenarioMarketData md = FxSingleTradeCalculationFunctionTest.marketData();
		RatesProvider provider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		DiscountingFxSingleTradePricer pricer = DiscountingFxSingleTradePricer.DEFAULT;
		MultiCurrencyAmount expectedPv = pricer.presentValue(RTRADE, provider);
		MultiCurrencyAmount expectedCurrencyExposure = pricer.currencyExposure(RTRADE, provider);
		MultiCurrencyAmount expectedCurrentCash = pricer.currentCash(RTRADE, provider);
		FxRate expectedForwardFx = pricer.forwardFxRate(RTRADE, provider);

		assertEquals(FxSingleTradeCalculations.DEFAULT.presentValue(RTRADE, RATES_LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv)));
		assertEquals(FxSingleTradeCalculations.DEFAULT.currencyExposure(RTRADE, RATES_LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedCurrencyExposure)));
		assertEquals(FxSingleTradeCalculations.DEFAULT.currentCash(RTRADE, RATES_LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedCurrentCash)));
		assertEquals(FxSingleTradeCalculations.DEFAULT.forwardFxRate(RTRADE, RATES_LOOKUP, md), ScenarioArray.of(ImmutableList.of(expectedForwardFx)));
	  }

	  public virtual void test_pv01()
	  {
		ScenarioMarketData md = FxSingleTradeCalculationFunctionTest.marketData();
		RatesProvider provider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		DiscountingFxSingleTradePricer pricer = DiscountingFxSingleTradePricer.DEFAULT;
		PointSensitivities pvPointSens = pricer.presentValueSensitivity(RTRADE, provider);
		CurrencyParameterSensitivities pvParamSens = provider.parameterSensitivity(pvPointSens);
		MultiCurrencyAmount expectedPv01Cal = pvParamSens.total().multipliedBy(1e-4);
		CurrencyParameterSensitivities expectedPv01CalBucketed = pvParamSens.multipliedBy(1e-4);

		assertEquals(FxSingleTradeCalculations.DEFAULT.pv01CalibratedSum(RTRADE, RATES_LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv01Cal)));
		assertEquals(FxSingleTradeCalculations.DEFAULT.pv01CalibratedBucketed(RTRADE, RATES_LOOKUP, md), ScenarioArray.of(ImmutableList.of(expectedPv01CalBucketed)));
	  }

	}

}