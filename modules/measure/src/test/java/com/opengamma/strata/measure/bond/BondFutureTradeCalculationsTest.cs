/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using DoubleMath = com.google.common.math.DoubleMath;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using DoubleScenarioArray = com.opengamma.strata.data.scenario.DoubleScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using DiscountingBondFutureTradePricer = com.opengamma.strata.pricer.bond.DiscountingBondFutureTradePricer;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using ResolvedBondFutureTrade = com.opengamma.strata.product.bond.ResolvedBondFutureTrade;

	/// <summary>
	/// Test <seealso cref="BondFutureTradeCalculations"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BondFutureTradeCalculationsTest
	public class BondFutureTradeCalculationsTest
	{

	  private static readonly ResolvedBondFutureTrade RTRADE = BondFutureTradeCalculationFunctionTest.RTRADE;
	  private static readonly LegalEntityDiscountingMarketDataLookup LOOKUP = BondFutureTradeCalculationFunctionTest.LOOKUP;
	  private static readonly double SETTLE_PRICE = BondFutureTradeCalculationFunctionTest.SETTLE_PRICE;
	  private static readonly MarketQuoteSensitivityCalculator MQ_CALC = MarketQuoteSensitivityCalculator.DEFAULT;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		ScenarioMarketData md = BondFutureTradeCalculationFunctionTest.marketData();
		LegalEntityDiscountingProvider provider = LOOKUP.marketDataView(md.scenario(0)).discountingProvider();
		DiscountingBondFutureTradePricer pricer = DiscountingBondFutureTradePricer.DEFAULT;
		CurrencyAmount expectedPv = pricer.presentValue(RTRADE, provider, SETTLE_PRICE);
		double expectedParSpread = pricer.parSpread(RTRADE, provider, SETTLE_PRICE);

		assertEquals(BondFutureTradeCalculations.DEFAULT.presentValue(RTRADE, LOOKUP, md), CurrencyScenarioArray.of(ImmutableList.of(expectedPv)));
		assertEquals(BondFutureTradeCalculations.DEFAULT.parSpread(RTRADE, LOOKUP, md), DoubleScenarioArray.of(ImmutableList.of(expectedParSpread)));
	  }

	  public virtual void test_pv01_calibrated()
	  {
		ScenarioMarketData md = BondFutureTradeCalculationFunctionTest.marketData();
		LegalEntityDiscountingProvider provider = LOOKUP.marketDataView(md.scenario(0)).discountingProvider();
		DiscountingBondFutureTradePricer pricer = DiscountingBondFutureTradePricer.DEFAULT;
		PointSensitivities pvPointSens = pricer.presentValueSensitivity(RTRADE, provider);
		CurrencyParameterSensitivities pvParamSens = provider.parameterSensitivity(pvPointSens);
		MultiCurrencyAmount expectedPv01Cal = pvParamSens.total().multipliedBy(1e-4);
		CurrencyParameterSensitivities expectedPv01CalBucketed = pvParamSens.multipliedBy(1e-4);

		assertEquals(BondFutureTradeCalculations.DEFAULT.pv01CalibratedSum(RTRADE, LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv01Cal)));
		assertEquals(BondFutureTradeCalculations.DEFAULT.pv01CalibratedBucketed(RTRADE, LOOKUP, md), ScenarioArray.of(ImmutableList.of(expectedPv01CalBucketed)));
	  }

	  public virtual void test_pv01_quote()
	  {
		ScenarioMarketData md = BondFutureTradeCalculationFunctionTest.marketData();
		LegalEntityDiscountingProvider provider = LOOKUP.marketDataView(md.scenario(0)).discountingProvider();
		DiscountingBondFutureTradePricer pricer = DiscountingBondFutureTradePricer.DEFAULT;
		PointSensitivities pvPointSens = pricer.presentValueSensitivity(RTRADE, provider);
		CurrencyParameterSensitivities pvParamSens = provider.parameterSensitivity(pvPointSens);
		CurrencyParameterSensitivities expectedPv01CalBucketed = MQ_CALC.sensitivity(pvParamSens.multipliedBy(1e-4), provider);
		MultiCurrencyAmount expectedPv01Cal = expectedPv01CalBucketed.total();

		MultiCurrencyScenarioArray sumComputed = BondFutureTradeCalculations.DEFAULT.pv01MarketQuoteSum(RTRADE, LOOKUP, md);
		ScenarioArray<CurrencyParameterSensitivities> bucketedComputed = BondFutureTradeCalculations.DEFAULT.pv01MarketQuoteBucketed(RTRADE, LOOKUP, md);
		assertEquals(sumComputed.ScenarioCount, 1);
		assertEquals(sumComputed.get(0).Currencies, ImmutableSet.of(USD));
		assertTrue(DoubleMath.fuzzyEquals(sumComputed.get(0).getAmount(USD).Amount, expectedPv01Cal.getAmount(USD).Amount, 1.0e-10));
		assertEquals(bucketedComputed.ScenarioCount, 1);
		assertTrue(bucketedComputed.get(0).equalWithTolerance(expectedPv01CalBucketed, 1.0e-10));
	  }

	}

}