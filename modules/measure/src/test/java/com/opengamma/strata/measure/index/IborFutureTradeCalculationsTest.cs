/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.index
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
	using DiscountingIborFutureTradePricer = com.opengamma.strata.pricer.index.DiscountingIborFutureTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedIborFutureTrade = com.opengamma.strata.product.index.ResolvedIborFutureTrade;

	/// <summary>
	/// Test <seealso cref="IborFutureTradeCalculations"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborFutureTradeCalculationsTest
	public class IborFutureTradeCalculationsTest
	{

	  private static readonly ResolvedIborFutureTrade RTRADE = IborFutureTradeCalculationFunctionTest.RTRADE;
	  private static readonly RatesMarketDataLookup RATES_LOOKUP = IborFutureTradeCalculationFunctionTest.RATES_LOOKUP;
	  private const double SETTLEMENT_PRICE = 0.9942;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		ScenarioMarketData md = IborFutureTradeCalculationFunctionTest.marketData();
		RatesProvider provider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		DiscountingIborFutureTradePricer pricer = DiscountingIborFutureTradePricer.DEFAULT;
		CurrencyAmount expectedPv = pricer.presentValue(RTRADE, provider, SETTLEMENT_PRICE);
		double expectedParSpread = pricer.parSpread(RTRADE, provider, SETTLEMENT_PRICE);

		assertEquals(IborFutureTradeCalculations.DEFAULT.presentValue(RTRADE, RATES_LOOKUP, md), CurrencyScenarioArray.of(ImmutableList.of(expectedPv)));
		assertEquals(IborFutureTradeCalculations.DEFAULT.parSpread(RTRADE, RATES_LOOKUP, md), DoubleScenarioArray.of(ImmutableList.of(expectedParSpread)));
	  }

	  public virtual void test_pv01()
	  {
		ScenarioMarketData md = IborFutureTradeCalculationFunctionTest.marketData();
		RatesProvider provider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		DiscountingIborFutureTradePricer pricer = DiscountingIborFutureTradePricer.DEFAULT;
		PointSensitivities pvPointSens = pricer.presentValueSensitivity(RTRADE, provider);
		CurrencyParameterSensitivities pvParamSens = provider.parameterSensitivity(pvPointSens);
		MultiCurrencyAmount expectedPv01Cal = pvParamSens.total().multipliedBy(1e-4);
		CurrencyParameterSensitivities expectedPv01CalBucketed = pvParamSens.multipliedBy(1e-4);

		assertEquals(IborFutureTradeCalculations.DEFAULT.pv01CalibratedSum(RTRADE, RATES_LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv01Cal)));
		assertEquals(IborFutureTradeCalculations.DEFAULT.pv01CalibratedBucketed(RTRADE, RATES_LOOKUP, md), ScenarioArray.of(ImmutableList.of(expectedPv01CalBucketed)));
	  }

	}

}