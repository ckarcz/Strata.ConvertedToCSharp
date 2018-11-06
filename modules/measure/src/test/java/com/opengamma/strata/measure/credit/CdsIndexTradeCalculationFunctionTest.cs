using System.Collections.Generic;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.credit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Measure = com.opengamma.strata.calc.Measure;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using Result = com.opengamma.strata.collect.result.Result;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using DoubleScenarioArray = com.opengamma.strata.data.scenario.DoubleScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PriceType = com.opengamma.strata.pricer.common.PriceType;
	using AccrualOnDefaultFormula = com.opengamma.strata.pricer.credit.AccrualOnDefaultFormula;
	using AnalyticSpreadSensitivityCalculator = com.opengamma.strata.pricer.credit.AnalyticSpreadSensitivityCalculator;
	using CreditRatesProvider = com.opengamma.strata.pricer.credit.CreditRatesProvider;
	using IsdaHomogenousCdsIndexTradePricer = com.opengamma.strata.pricer.credit.IsdaHomogenousCdsIndexTradePricer;
	using JumpToDefault = com.opengamma.strata.pricer.credit.JumpToDefault;
	using SpreadSensitivityCalculator = com.opengamma.strata.pricer.credit.SpreadSensitivityCalculator;
	using CdsIndexTrade = com.opengamma.strata.product.credit.CdsIndexTrade;
	using ResolvedCdsIndexTrade = com.opengamma.strata.product.credit.ResolvedCdsIndexTrade;

	/// <summary>
	/// Test <seealso cref="CdsIndexTradeCalculationFunction"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CdsIndexTradeCalculationFunctionTest
	public class CdsIndexTradeCalculationFunctionTest
	{

	  private static readonly CdsIndexTrade TRADE = CreditDataSet.INDEX_TRADE;
	  private static readonly ResolvedCdsIndexTrade RTRADE = CreditDataSet.RESOLVED_INDEX_TRADE;
	  private static readonly CdsIndexTradeCalculationFunction FUNCTION = new CdsIndexTradeCalculationFunction();
	  private static readonly CreditRatesProvider RATES_PROVIDER = CreditDataSet.INDEX_LOOKUP.marketDataView(CreditDataSet.MARKET_DATA.scenario(0)).creditRatesProvider();
	  private static readonly IsdaHomogenousCdsIndexTradePricer PRICER = new IsdaHomogenousCdsIndexTradePricer(AccrualOnDefaultFormula.CORRECT);
	  private static readonly SpreadSensitivityCalculator CS01_CALC = new AnalyticSpreadSensitivityCalculator(AccrualOnDefaultFormula.CORRECT);

	  //-------------------------------------------------------------------------
	  public virtual void test_requirementsAndCurrency()
	  {
		ISet<Measure> measures = FUNCTION.supportedMeasures();
		FunctionRequirements reqs = FUNCTION.requirements(TRADE, measures, CreditDataSet.INDEX_PARAMS, CreditDataSet.REF_DATA);
		assertThat(reqs.OutputCurrencies).containsOnly(USD);
		assertThat(reqs.ValueRequirements).isEqualTo(ImmutableSet.of(CreditDataSet.INDEX_CREDIT_CURVE_ID, CreditDataSet.USD_DSC_CURVE_ID, CreditDataSet.INDEX_RECOVERY_CURVE_ID));
		assertThat(reqs.TimeSeriesRequirements).isEqualTo(ImmutableSet.of());
		assertThat(FUNCTION.naturalCurrency(TRADE, CreditDataSet.REF_DATA)).isEqualTo(USD);
	  }

	  public virtual void test_simpleMeasures()
	  {
		CurrencyAmount expectedPv = PRICER.presentValue(RTRADE, RATES_PROVIDER, PriceType.DIRTY, CreditDataSet.REF_DATA);
		CurrencyAmount expectedPr = PRICER.presentValueOnSettle(RTRADE, RATES_PROVIDER, PriceType.CLEAN, CreditDataSet.REF_DATA);
		double expectedCp = 1d - PRICER.price(RTRADE, RATES_PROVIDER, PriceType.CLEAN, CreditDataSet.REF_DATA);
		JumpToDefault expectedJtd = PRICER.jumpToDefault(RTRADE, RATES_PROVIDER, CreditDataSet.REF_DATA);
		CurrencyAmount expectedEl = PRICER.expectedLoss(RTRADE, RATES_PROVIDER);
		CurrencyAmount expectedR01 = PRICER.recovery01OnSettle(RTRADE, RATES_PROVIDER, CreditDataSet.REF_DATA);

		ISet<Measure> measures = ImmutableSet.of(Measures.PRESENT_VALUE, CreditMeasures.PRINCIPAL, Measures.UNIT_PRICE, CreditMeasures.JUMP_TO_DEFAULT, CreditMeasures.EXPECTED_LOSS, CreditMeasures.RECOVERY01);
		assertThat(FUNCTION.calculate(TRADE, measures, CreditDataSet.INDEX_PARAMS, CreditDataSet.MARKET_DATA, CreditDataSet.REF_DATA)).containsEntry(Measures.PRESENT_VALUE, Result.success(CurrencyScenarioArray.of(ImmutableList.of(expectedPv)))).containsEntry(CreditMeasures.PRINCIPAL, Result.success(CurrencyScenarioArray.of(ImmutableList.of(expectedPr)))).containsEntry(Measures.UNIT_PRICE, Result.success(DoubleScenarioArray.of(ImmutableList.of(expectedCp)))).containsEntry(CreditMeasures.JUMP_TO_DEFAULT, Result.success(ScenarioArray.of(ImmutableList.of(expectedJtd)))).containsEntry(CreditMeasures.EXPECTED_LOSS, Result.success(CurrencyScenarioArray.of(ImmutableList.of(expectedEl)))).containsEntry(CreditMeasures.RECOVERY01, Result.success(CurrencyScenarioArray.of(ImmutableList.of(expectedR01))));
	  }

	  public virtual void test_curveSensitivityMeasures()
	  {
		double oneBp = 1e-4;
		PointSensitivities pvPointSens = PRICER.presentValueSensitivity(RTRADE, RATES_PROVIDER, CreditDataSet.REF_DATA);
		CurrencyParameterSensitivities pvParamSens = RATES_PROVIDER.parameterSensitivity(pvPointSens);
		MultiCurrencyAmount expectedPv01Cal = pvParamSens.total().multipliedBy(oneBp);
		CurrencyParameterSensitivities expectedPv01CalBucketed = pvParamSens.multipliedBy(oneBp);
		CurrencyParameterSensitivity expectedCs01Bucketed = CS01_CALC.bucketedCs01(RTRADE, RATES_PROVIDER, CreditDataSet.REF_DATA);
		CurrencyAmount expectedCs01Parallel = CS01_CALC.parallelCs01(RTRADE, RATES_PROVIDER, CreditDataSet.REF_DATA);
		PointSensitivities pvPointSensOnSettle = PRICER.presentValueOnSettleSensitivity(RTRADE, RATES_PROVIDER, CreditDataSet.REF_DATA);
		CurrencyParameterSensitivity ir01 = RATES_PROVIDER.singleDiscountCurveParameterSensitivity(pvPointSensOnSettle, USD);
		CurrencyAmount expectedIr01Cal = ir01.total().multipliedBy(oneBp);
		CurrencyParameterSensitivity expectedIr01CalBucketed = ir01.multipliedBy(oneBp);

		ISet<Measure> measures = ImmutableSet.of(Measures.PV01_CALIBRATED_SUM, Measures.PV01_CALIBRATED_BUCKETED, CreditMeasures.CS01_PARALLEL, CreditMeasures.CS01_BUCKETED, CreditMeasures.IR01_CALIBRATED_PARALLEL, CreditMeasures.IR01_CALIBRATED_BUCKETED);
		assertThat(FUNCTION.calculate(TRADE, measures, CreditDataSet.INDEX_PARAMS, CreditDataSet.MARKET_DATA, CreditDataSet.REF_DATA)).containsEntry(Measures.PV01_CALIBRATED_SUM, Result.success(MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv01Cal)))).containsEntry(Measures.PV01_CALIBRATED_BUCKETED, Result.success(ScenarioArray.of(ImmutableList.of(expectedPv01CalBucketed)))).containsEntry(CreditMeasures.CS01_PARALLEL, Result.success(CurrencyScenarioArray.of(ImmutableList.of(expectedCs01Parallel)))).containsEntry(CreditMeasures.CS01_BUCKETED, Result.success(ScenarioArray.of(ImmutableList.of(expectedCs01Bucketed)))).containsEntry(CreditMeasures.IR01_CALIBRATED_PARALLEL, Result.success(CurrencyScenarioArray.of(ImmutableList.of(expectedIr01Cal)))).containsEntry(CreditMeasures.IR01_CALIBRATED_BUCKETED, Result.success(ScenarioArray.of(ImmutableList.of(expectedIr01CalBucketed))));
	  }

	}

}