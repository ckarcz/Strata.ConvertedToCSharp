using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fra
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using Result = com.opengamma.strata.collect.result.Result;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using DoubleScenarioArray = com.opengamma.strata.data.scenario.DoubleScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CashFlows = com.opengamma.strata.market.amount.CashFlows;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using TestMarketDataMap = com.opengamma.strata.measure.curve.TestMarketDataMap;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using DiscountingFraProductPricer = com.opengamma.strata.pricer.fra.DiscountingFraProductPricer;
	using DiscountingFraTradePricer = com.opengamma.strata.pricer.fra.DiscountingFraTradePricer;
	using FraDummyData = com.opengamma.strata.pricer.fra.FraDummyData;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using FraTrade = com.opengamma.strata.product.fra.FraTrade;
	using ResolvedFra = com.opengamma.strata.product.fra.ResolvedFra;
	using ResolvedFraTrade = com.opengamma.strata.product.fra.ResolvedFraTrade;

	/// <summary>
	/// Test <seealso cref="FraTradeCalculationFunction"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FraTradeCalculationFunctionTest
	public class FraTradeCalculationFunctionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  public static readonly FraTrade TRADE = FraDummyData.FRA_TRADE;
	  public static readonly ResolvedFraTrade RTRADE = TRADE.resolve(REF_DATA);

	  private static readonly IborIndex INDEX = TRADE.Product.Index;
	  private static readonly Currency CURRENCY = TRADE.Product.Currency;
	  private static readonly CurveId DISCOUNT_CURVE_ID = CurveId.of("Default", "Discount");
	  private static readonly CurveId FORWARD_CURVE_ID = CurveId.of("Default", "Forward");
	  public static readonly RatesMarketDataLookup RATES_LOOKUP = RatesMarketDataLookup.of(ImmutableMap.of(CURRENCY, DISCOUNT_CURVE_ID), ImmutableMap.of(INDEX, FORWARD_CURVE_ID));
	  private static readonly CalculationParameters PARAMS = CalculationParameters.of(RATES_LOOKUP);
	  private static readonly LocalDate VAL_DATE = TRADE.Product.StartDate.minusDays(7);

	  //-------------------------------------------------------------------------
	  public virtual void test_requirementsAndCurrency()
	  {
		FraTradeCalculationFunction function = new FraTradeCalculationFunction();
		ISet<Measure> measures = function.supportedMeasures();
		FunctionRequirements reqs = function.requirements(TRADE, measures, PARAMS, REF_DATA);
		assertThat(reqs.OutputCurrencies).containsOnly(CURRENCY);
		assertThat(reqs.ValueRequirements).isEqualTo(ImmutableSet.of(DISCOUNT_CURVE_ID, FORWARD_CURVE_ID));
		assertThat(reqs.TimeSeriesRequirements).isEqualTo(ImmutableSet.of(IndexQuoteId.of(INDEX)));
		assertThat(function.naturalCurrency(TRADE, REF_DATA)).isEqualTo(CURRENCY);
	  }

	  public virtual void test_simpleMeasures()
	  {
		FraTradeCalculationFunction function = new FraTradeCalculationFunction();
		ScenarioMarketData md = marketData();
		RatesProvider provider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		DiscountingFraTradePricer pricer = DiscountingFraTradePricer.DEFAULT;
		CurrencyAmount expectedPv = pricer.presentValue(RTRADE, provider);
		ExplainMap expectedExplainPv = pricer.explainPresentValue(RTRADE, provider);
		double expectedParRate = pricer.parRate(RTRADE, provider);
		double expectedParSpread = pricer.parSpread(RTRADE, provider);
		CashFlows expectedCashFlows = pricer.cashFlows(RTRADE, provider);
		MultiCurrencyAmount expectedCurrencyExposure = pricer.currencyExposure(RTRADE, provider);
		CurrencyAmount expectedCurrentCash = pricer.currentCash(RTRADE, provider);

		ISet<Measure> measures = ImmutableSet.of(Measures.PRESENT_VALUE, Measures.EXPLAIN_PRESENT_VALUE, Measures.PAR_RATE, Measures.PAR_SPREAD, Measures.CASH_FLOWS, Measures.CURRENCY_EXPOSURE, Measures.CURRENT_CASH, Measures.RESOLVED_TARGET);
		assertThat(function.calculate(TRADE, measures, PARAMS, md, REF_DATA)).containsEntry(Measures.PRESENT_VALUE, Result.success(CurrencyScenarioArray.of(ImmutableList.of(expectedPv)))).containsEntry(Measures.EXPLAIN_PRESENT_VALUE, Result.success(ScenarioArray.of(ImmutableList.of(expectedExplainPv)))).containsEntry(Measures.PAR_RATE, Result.success(DoubleScenarioArray.of(ImmutableList.of(expectedParRate)))).containsEntry(Measures.PAR_SPREAD, Result.success(DoubleScenarioArray.of(ImmutableList.of(expectedParSpread)))).containsEntry(Measures.CASH_FLOWS, Result.success(ScenarioArray.of(ImmutableList.of(expectedCashFlows)))).containsEntry(Measures.CURRENCY_EXPOSURE, Result.success(MultiCurrencyScenarioArray.of(ImmutableList.of(expectedCurrencyExposure)))).containsEntry(Measures.CURRENT_CASH, Result.success(CurrencyScenarioArray.of(ImmutableList.of(expectedCurrentCash)))).containsEntry(Measures.RESOLVED_TARGET, Result.success(RTRADE));
	  }

	  public virtual void test_pv01()
	  {
		FraTradeCalculationFunction function = new FraTradeCalculationFunction();
		ScenarioMarketData md = marketData();
		RatesProvider provider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		DiscountingFraProductPricer pricer = DiscountingFraProductPricer.DEFAULT;
		ResolvedFra resolved = TRADE.Product.resolve(REF_DATA);
		PointSensitivities pvPointSens = pricer.presentValueSensitivity(resolved, provider);
		CurrencyParameterSensitivities pvParamSens = provider.parameterSensitivity(pvPointSens);
		MultiCurrencyAmount expectedPv01Cal = pvParamSens.total().multipliedBy(1e-4);
		CurrencyParameterSensitivities expectedPv01CalBucketed = pvParamSens.multipliedBy(1e-4);

		ISet<Measure> measures = ImmutableSet.of(Measures.PV01_CALIBRATED_SUM, Measures.PV01_CALIBRATED_BUCKETED);
		assertThat(function.calculate(TRADE, measures, PARAMS, md, REF_DATA)).containsEntry(Measures.PV01_CALIBRATED_SUM, Result.success(MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv01Cal)))).containsEntry(Measures.PV01_CALIBRATED_BUCKETED, Result.success(ScenarioArray.of(ImmutableList.of(expectedPv01CalBucketed))));
	  }

	  //-------------------------------------------------------------------------
	  internal static ScenarioMarketData marketData()
	  {
		Curve curve = ConstantCurve.of(Curves.discountFactors("Test", ACT_360), 0.99);
		return new TestMarketDataMap(VAL_DATE, ImmutableMap.of(DISCOUNT_CURVE_ID, curve, FORWARD_CURVE_ID, curve), ImmutableMap.of());
	  }

	}

}