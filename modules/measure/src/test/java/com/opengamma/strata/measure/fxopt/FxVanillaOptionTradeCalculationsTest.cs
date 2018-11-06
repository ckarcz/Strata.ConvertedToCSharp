/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.measure.fxopt.FxVanillaOptionMethod.BLACK;
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
	using BlackFxVanillaOptionTradePricer = com.opengamma.strata.pricer.fxopt.BlackFxVanillaOptionTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedFxVanillaOptionTrade = com.opengamma.strata.product.fxopt.ResolvedFxVanillaOptionTrade;

	/// <summary>
	/// Test <seealso cref="FxVanillaOptionTradeCalculations"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxVanillaOptionTradeCalculationsTest
	public class FxVanillaOptionTradeCalculationsTest
	{

	  private static readonly ResolvedFxVanillaOptionTrade RTRADE = FxVanillaOptionTradeCalculationFunctionTest.RTRADE;
	  private static readonly RatesMarketDataLookup RATES_LOOKUP = FxVanillaOptionTradeCalculationFunctionTest.RATES_LOOKUP;
	  private static readonly FxOptionMarketDataLookup FX_OPTION_LOOKUP = FxVanillaOptionTradeCalculationFunctionTest.FX_OPTION_LOOKUP;
	  private static readonly BlackFxOptionVolatilities VOLS = FxVanillaOptionTradeCalculationFunctionTest.VOLS;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		ScenarioMarketData md = FxVanillaOptionTradeCalculationFunctionTest.marketData();
		RatesProvider provider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		BlackFxVanillaOptionTradePricer pricer = BlackFxVanillaOptionTradePricer.DEFAULT;
		MultiCurrencyAmount expectedPv = pricer.presentValue(RTRADE, provider, VOLS);
		MultiCurrencyAmount expectedCurrencyExposure = pricer.currencyExposure(RTRADE, provider, VOLS);
		CurrencyAmount expectedCurrentCash = pricer.currentCash(RTRADE, provider.ValuationDate);

		assertEquals(FxVanillaOptionTradeCalculations.DEFAULT.presentValue(RTRADE, RATES_LOOKUP, FX_OPTION_LOOKUP, md, BLACK), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv)));
		assertEquals(FxVanillaOptionTradeCalculations.DEFAULT.currencyExposure(RTRADE, RATES_LOOKUP, FX_OPTION_LOOKUP, md, BLACK), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedCurrencyExposure)));
		assertEquals(FxVanillaOptionTradeCalculations.DEFAULT.currentCash(RTRADE, RATES_LOOKUP, FX_OPTION_LOOKUP, md, BLACK), CurrencyScenarioArray.of(ImmutableList.of(expectedCurrentCash)));
	  }

	  public virtual void test_pv01()
	  {
		ScenarioMarketData md = FxVanillaOptionTradeCalculationFunctionTest.marketData();
		RatesProvider provider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		BlackFxVanillaOptionTradePricer pricer = BlackFxVanillaOptionTradePricer.DEFAULT;
		PointSensitivities pvPointSens = pricer.presentValueSensitivityRatesStickyStrike(RTRADE, provider, VOLS);
		CurrencyParameterSensitivities pvParamSens = provider.parameterSensitivity(pvPointSens);
		MultiCurrencyAmount expectedPv01Cal = pvParamSens.total().multipliedBy(1e-4);
		CurrencyParameterSensitivities expectedPv01CalBucketed = pvParamSens.multipliedBy(1e-4);

		assertEquals(FxVanillaOptionTradeCalculations.DEFAULT.pv01RatesCalibratedSum(RTRADE, RATES_LOOKUP, FX_OPTION_LOOKUP, md, BLACK), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv01Cal)));
		assertEquals(FxVanillaOptionTradeCalculations.DEFAULT.pv01RatesCalibratedBucketed(RTRADE, RATES_LOOKUP, FX_OPTION_LOOKUP, md, BLACK), ScenarioArray.of(ImmutableList.of(expectedPv01CalBucketed)));
	  }

	}

}