/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.measure.fxopt.FxSingleBarrierOptionMethod.BLACK;
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
	using BlackFxOptionVolatilities = com.opengamma.strata.pricer.fxopt.BlackFxOptionVolatilities;
	using BlackFxSingleBarrierOptionTradePricer = com.opengamma.strata.pricer.fxopt.BlackFxSingleBarrierOptionTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedFxSingleBarrierOptionTrade = com.opengamma.strata.product.fxopt.ResolvedFxSingleBarrierOptionTrade;

	/// <summary>
	/// Test <seealso cref="FxSingleBarrierOptionTradeCalculations"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxSingleBarrierOptionTradeCalculationsTest
	public class FxSingleBarrierOptionTradeCalculationsTest
	{

	  private static readonly ResolvedFxSingleBarrierOptionTrade RTRADE = FxSingleBarrierOptionTradeCalculationFunctionTest.RTRADE;
	  private static readonly RatesMarketDataLookup RATES_LOOKUP = FxSingleBarrierOptionTradeCalculationFunctionTest.RATES_LOOKUP;
	  private static readonly FxOptionMarketDataLookup FX_OPTION_LOOKUP = FxSingleBarrierOptionTradeCalculationFunctionTest.FX_OPTION_LOOKUP;
	  private static readonly BlackFxOptionVolatilities VOLS = FxSingleBarrierOptionTradeCalculationFunctionTest.VOLS;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		ScenarioMarketData md = FxSingleBarrierOptionTradeCalculationFunctionTest.marketData();
		RatesProvider provider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		BlackFxSingleBarrierOptionTradePricer pricer = BlackFxSingleBarrierOptionTradePricer.DEFAULT;
		MultiCurrencyAmount expectedPv = pricer.presentValue(RTRADE, provider, VOLS);
		MultiCurrencyAmount expectedCurrencyExposure = pricer.currencyExposure(RTRADE, provider, VOLS);
		CurrencyAmount expectedCurrentCash = pricer.currentCash(RTRADE, provider.ValuationDate);

		assertEquals(FxSingleBarrierOptionTradeCalculations.DEFAULT.presentValue(RTRADE, RATES_LOOKUP, FX_OPTION_LOOKUP, md, BLACK), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv)));
		assertEquals(FxSingleBarrierOptionTradeCalculations.DEFAULT.currencyExposure(RTRADE, RATES_LOOKUP, FX_OPTION_LOOKUP, md, BLACK), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedCurrencyExposure)));
		assertEquals(FxSingleBarrierOptionTradeCalculations.DEFAULT.currentCash(RTRADE, RATES_LOOKUP, FX_OPTION_LOOKUP, md, BLACK), CurrencyScenarioArray.of(ImmutableList.of(expectedCurrentCash)));
	  }

	  public virtual void test_pv01()
	  {
		ScenarioMarketData md = FxSingleBarrierOptionTradeCalculationFunctionTest.marketData();
		RatesProvider provider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		BlackFxSingleBarrierOptionTradePricer pricer = BlackFxSingleBarrierOptionTradePricer.DEFAULT;
		PointSensitivities pvPointSens = pricer.presentValueSensitivityRatesStickyStrike(RTRADE, provider, VOLS);
		CurrencyParameterSensitivities pvParamSens = provider.parameterSensitivity(pvPointSens);
		MultiCurrencyAmount expectedPv01Cal = pvParamSens.total().multipliedBy(1e-4);
		CurrencyParameterSensitivities expectedPv01CalBucketed = pvParamSens.multipliedBy(1e-4);

		assertEquals(FxSingleBarrierOptionTradeCalculations.DEFAULT.pv01RatesCalibratedSum(RTRADE, RATES_LOOKUP, FX_OPTION_LOOKUP, md, BLACK), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv01Cal)));
		assertEquals(FxSingleBarrierOptionTradeCalculations.DEFAULT.pv01RatesCalibratedBucketed(RTRADE, RATES_LOOKUP, FX_OPTION_LOOKUP, md, BLACK), ScenarioArray.of(ImmutableList.of(expectedPv01CalBucketed)));
	  }

	}

}