/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using DiscountingBillTradePricer = com.opengamma.strata.pricer.bond.DiscountingBillTradePricer;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using ResolvedBillTrade = com.opengamma.strata.product.bond.ResolvedBillTrade;

	/// <summary>
	/// Test <seealso cref="BillTradeCalculations"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BillTradeCalculationsTest
	public class BillTradeCalculationsTest
	{

	  private static readonly ResolvedBillTrade RTRADE = BillTradeCalculationFunctionTest.RTRADE;
	  private static readonly LegalEntityDiscountingMarketDataLookup LOOKUP = BillTradeCalculationFunctionTest.LOOKUP;
	  private static readonly BillTradeCalculations CALC = BillTradeCalculations.DEFAULT;
	  private static readonly DiscountingBillTradePricer PRICER = DiscountingBillTradePricer.DEFAULT;
	  private static readonly MarketQuoteSensitivityCalculator MQ_CALC = MarketQuoteSensitivityCalculator.DEFAULT;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		ScenarioMarketData md = BillTradeCalculationFunctionTest.marketData();
		LegalEntityDiscountingProvider provider = LOOKUP.marketDataView(md.scenario(0)).discountingProvider();
		CurrencyAmount expectedPv = PRICER.presentValue(RTRADE, provider);
		MultiCurrencyAmount expectedCurrencyExposure = PRICER.currencyExposure(RTRADE, provider);
		CurrencyAmount expectedCurrentCash = PRICER.currentCash(RTRADE, provider.ValuationDate);

		assertEquals(CALC.presentValue(RTRADE, LOOKUP, md), CurrencyScenarioArray.of(ImmutableList.of(expectedPv)));
		assertEquals(CALC.currencyExposure(RTRADE, LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedCurrencyExposure)));
		assertEquals(CALC.currentCash(RTRADE, LOOKUP, md), CurrencyScenarioArray.of(ImmutableList.of(expectedCurrentCash)));
		assertEquals(CALC.presentValue(RTRADE, provider), expectedPv);
		assertEquals(CALC.currencyExposure(RTRADE, provider), expectedCurrencyExposure);
		assertEquals(CALC.currentCash(RTRADE, provider), expectedCurrentCash);
	  }

	  public virtual void test_pv01_calibrated()
	  {
		ScenarioMarketData md = BillTradeCalculationFunctionTest.marketData();
		LegalEntityDiscountingProvider provider = LOOKUP.marketDataView(md.scenario(0)).discountingProvider();
		PointSensitivities pvPointSens = PRICER.presentValueSensitivity(RTRADE, provider);
		CurrencyParameterSensitivities pvParamSens = provider.parameterSensitivity(pvPointSens);
		MultiCurrencyAmount expectedPv01Cal = pvParamSens.total().multipliedBy(1e-4);
		CurrencyParameterSensitivities expectedPv01CalBucketed = pvParamSens.multipliedBy(1e-4);

		assertEquals(BillTradeCalculations.DEFAULT.pv01CalibratedSum(RTRADE, LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv01Cal)));
		assertEquals(BillTradeCalculations.DEFAULT.pv01CalibratedBucketed(RTRADE, LOOKUP, md), ScenarioArray.of(ImmutableList.of(expectedPv01CalBucketed)));
		assertEquals(BillTradeCalculations.DEFAULT.pv01CalibratedSum(RTRADE, provider), expectedPv01Cal);
		assertEquals(BillTradeCalculations.DEFAULT.pv01CalibratedBucketed(RTRADE, provider), expectedPv01CalBucketed);
	  }

	  public virtual void test_pv01_quote()
	  {
		ScenarioMarketData md = BillTradeCalculationFunctionTest.marketData();
		LegalEntityDiscountingProvider provider = LOOKUP.marketDataView(md.scenario(0)).discountingProvider();
		PointSensitivities pvPointSens = PRICER.presentValueSensitivity(RTRADE, provider);
		CurrencyParameterSensitivities pvParamSens = provider.parameterSensitivity(pvPointSens);
		CurrencyParameterSensitivities expectedPv01CalBucketed = MQ_CALC.sensitivity(pvParamSens, provider).multipliedBy(1e-4);
		MultiCurrencyAmount expectedPv01Cal = expectedPv01CalBucketed.total();

		assertEquals(BillTradeCalculations.DEFAULT.pv01MarketQuoteSum(RTRADE, LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv01Cal)));
		assertEquals(BillTradeCalculations.DEFAULT.pv01MarketQuoteBucketed(RTRADE, LOOKUP, md), ScenarioArray.of(ImmutableList.of(expectedPv01CalBucketed)));
		assertEquals(BillTradeCalculations.DEFAULT.pv01MarketQuoteSum(RTRADE, provider), expectedPv01Cal);
		assertEquals(BillTradeCalculations.DEFAULT.pv01MarketQuoteBucketed(RTRADE, provider), expectedPv01CalBucketed);
	  }

	}

}