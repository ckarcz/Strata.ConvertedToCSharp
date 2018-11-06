/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
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
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using DiscountingFixedCouponBondTradePricer = com.opengamma.strata.pricer.bond.DiscountingFixedCouponBondTradePricer;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using ResolvedFixedCouponBondTrade = com.opengamma.strata.product.bond.ResolvedFixedCouponBondTrade;

	/// <summary>
	/// Test <seealso cref="FixedCouponBondTradeCalculations"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FixedCouponBondTradeCalculationsTest
	public class FixedCouponBondTradeCalculationsTest
	{

	  private static readonly ResolvedFixedCouponBondTrade RTRADE = FixedCouponBondTradeCalculationFunctionTest.RTRADE;
	  private static readonly LegalEntityDiscountingMarketDataLookup LOOKUP = FixedCouponBondTradeCalculationFunctionTest.LOOKUP;
	  private static readonly MarketQuoteSensitivityCalculator MQ_CALC = MarketQuoteSensitivityCalculator.DEFAULT;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		ScenarioMarketData md = FixedCouponBondTradeCalculationFunctionTest.marketData();
		LegalEntityDiscountingProvider provider = LOOKUP.marketDataView(md.scenario(0)).discountingProvider();
		DiscountingFixedCouponBondTradePricer pricer = DiscountingFixedCouponBondTradePricer.DEFAULT;
		CurrencyAmount expectedPv = pricer.presentValue(RTRADE, provider);
		MultiCurrencyAmount expectedCurrencyExposure = pricer.currencyExposure(RTRADE, provider);
		CurrencyAmount expectedCurrentCash = pricer.currentCash(RTRADE, provider.ValuationDate);

		assertEquals(FixedCouponBondTradeCalculations.DEFAULT.presentValue(RTRADE, LOOKUP, md), CurrencyScenarioArray.of(ImmutableList.of(expectedPv)));
		assertEquals(FixedCouponBondTradeCalculations.DEFAULT.currencyExposure(RTRADE, LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedCurrencyExposure)));
		assertEquals(FixedCouponBondTradeCalculations.DEFAULT.currentCash(RTRADE, LOOKUP, md), CurrencyScenarioArray.of(ImmutableList.of(expectedCurrentCash)));
	  }

	  public virtual void test_pv01_calibrated()
	  {
		ScenarioMarketData md = FixedCouponBondTradeCalculationFunctionTest.marketData();
		LegalEntityDiscountingProvider provider = LOOKUP.marketDataView(md.scenario(0)).discountingProvider();
		DiscountingFixedCouponBondTradePricer pricer = DiscountingFixedCouponBondTradePricer.DEFAULT;
		PointSensitivities pvPointSens = pricer.presentValueSensitivity(RTRADE, provider);
		CurrencyParameterSensitivities pvParamSens = provider.parameterSensitivity(pvPointSens);
		MultiCurrencyAmount expectedPv01Cal = pvParamSens.total().multipliedBy(1e-4);
		CurrencyParameterSensitivities expectedPv01CalBucketed = pvParamSens.multipliedBy(1e-4);

		assertEquals(FixedCouponBondTradeCalculations.DEFAULT.pv01CalibratedSum(RTRADE, LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv01Cal)));
		assertEquals(FixedCouponBondTradeCalculations.DEFAULT.pv01CalibratedBucketed(RTRADE, LOOKUP, md), ScenarioArray.of(ImmutableList.of(expectedPv01CalBucketed)));
	  }

	  public virtual void test_pv01_quote()
	  {
		ScenarioMarketData md = FixedCouponBondTradeCalculationFunctionTest.marketData();
		LegalEntityDiscountingProvider provider = LOOKUP.marketDataView(md.scenario(0)).discountingProvider();
		DiscountingFixedCouponBondTradePricer pricer = DiscountingFixedCouponBondTradePricer.DEFAULT;
		PointSensitivities pvPointSens = pricer.presentValueSensitivity(RTRADE, provider);
		CurrencyParameterSensitivities pvParamSens = provider.parameterSensitivity(pvPointSens);
		CurrencyParameterSensitivities expectedPv01CalBucketed = MQ_CALC.sensitivity(pvParamSens, provider).multipliedBy(1e-4);
		MultiCurrencyAmount expectedPv01Cal = expectedPv01CalBucketed.total();

		MultiCurrencyScenarioArray sumComputed = FixedCouponBondTradeCalculations.DEFAULT.pv01MarketQuoteSum(RTRADE, LOOKUP, md);
		ScenarioArray<CurrencyParameterSensitivities> bucketedComputed = FixedCouponBondTradeCalculations.DEFAULT.pv01MarketQuoteBucketed(RTRADE, LOOKUP, md);
		assertEquals(sumComputed.ScenarioCount, 1);
		assertEquals(sumComputed.get(0).Currencies, ImmutableSet.of(GBP));
		assertTrue(DoubleMath.fuzzyEquals(sumComputed.get(0).getAmount(GBP).Amount, expectedPv01Cal.getAmount(GBP).Amount, 1.0e-10));
		assertEquals(bucketedComputed.ScenarioCount, 1);
		assertTrue(bucketedComputed.get(0).equalWithTolerance(expectedPv01CalBucketed, 1.0e-10));
	  }

	}

}