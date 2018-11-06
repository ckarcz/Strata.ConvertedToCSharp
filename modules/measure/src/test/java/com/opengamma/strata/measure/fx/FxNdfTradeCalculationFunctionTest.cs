using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fx
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.FxIndices.GBP_USD_WM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using Result = com.opengamma.strata.collect.result.Result;
	using FxRateId = com.opengamma.strata.data.FxRateId;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using TestMarketDataMap = com.opengamma.strata.measure.curve.TestMarketDataMap;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using DiscountingFxNdfTradePricer = com.opengamma.strata.pricer.fx.DiscountingFxNdfTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using FxNdf = com.opengamma.strata.product.fx.FxNdf;
	using FxNdfTrade = com.opengamma.strata.product.fx.FxNdfTrade;
	using ResolvedFxNdfTrade = com.opengamma.strata.product.fx.ResolvedFxNdfTrade;

	/// <summary>
	/// Test <seealso cref="FxNdfTradeCalculationFunction"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxNdfTradeCalculationFunctionTest
	public class FxNdfTradeCalculationFunctionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly FxRate FX_RATE = FxRate.of(GBP, USD, 1.5d);
	  private static readonly CurrencyAmount NOTIONAL = CurrencyAmount.of(GBP, (double) 100_000_000);
	  private static readonly FxNdf PRODUCT = FxNdf.builder().agreedFxRate(FX_RATE).settlementCurrencyNotional(NOTIONAL).index(GBP_USD_WM).paymentDate(date(2015, 3, 19)).build();
	  public static readonly FxNdfTrade TRADE = FxNdfTrade.builder().info(TradeInfo.builder().tradeDate(date(2015, 6, 1)).build()).product(PRODUCT).build();
	  public static readonly ResolvedFxNdfTrade RTRADE = TRADE.resolve(REF_DATA);

	  private static readonly CurveId DISCOUNT_CURVE_GBP_ID = CurveId.of("Default", "Discount-GBP");
	  private static readonly CurveId DISCOUNT_CURVE_USD_ID = CurveId.of("Default", "Discount-USD");
	  internal static readonly RatesMarketDataLookup RATES_LOOKUP = RatesMarketDataLookup.of(ImmutableMap.of(GBP, DISCOUNT_CURVE_GBP_ID, USD, DISCOUNT_CURVE_USD_ID), ImmutableMap.of());
	  private static readonly CalculationParameters PARAMS = CalculationParameters.of(RATES_LOOKUP);
	  private static readonly LocalDate VAL_DATE = TRADE.Product.PaymentDate.minusDays(7);

	  //-------------------------------------------------------------------------
	  public virtual void test_requirementsAndCurrency()
	  {
		FxNdfTradeCalculationFunction function = new FxNdfTradeCalculationFunction();
		ISet<Measure> measures = function.supportedMeasures();
		FunctionRequirements reqs = function.requirements(TRADE, measures, PARAMS, REF_DATA);
		assertThat(reqs.OutputCurrencies).containsExactly(GBP, USD);
		assertThat(reqs.ValueRequirements).isEqualTo(ImmutableSet.of(DISCOUNT_CURVE_GBP_ID, DISCOUNT_CURVE_USD_ID));
		assertThat(reqs.TimeSeriesRequirements).Empty;
		assertThat(function.naturalCurrency(TRADE, REF_DATA)).isEqualTo(GBP);
	  }

	  public virtual void test_simpleMeasures()
	  {
		FxNdfTradeCalculationFunction function = new FxNdfTradeCalculationFunction();
		ScenarioMarketData md = marketData();
		RatesProvider provider = RATES_LOOKUP.ratesProvider(md.scenario(0));
		DiscountingFxNdfTradePricer pricer = DiscountingFxNdfTradePricer.DEFAULT;
		CurrencyAmount expectedPv = pricer.presentValue(RTRADE, provider);
		MultiCurrencyAmount expectedCurrencyExp = pricer.currencyExposure(RTRADE, provider);
		CurrencyAmount expectedCash = pricer.currentCash(RTRADE, provider);
		FxRate expectedForwardFx = pricer.forwardFxRate(RTRADE, provider);

		ISet<Measure> measures = ImmutableSet.of(Measures.PRESENT_VALUE, Measures.CURRENCY_EXPOSURE, Measures.CURRENT_CASH, Measures.FORWARD_FX_RATE, Measures.RESOLVED_TARGET);
		assertThat(function.calculate(TRADE, measures, PARAMS, md, REF_DATA)).containsEntry(Measures.PRESENT_VALUE, Result.success(CurrencyScenarioArray.of(ImmutableList.of(expectedPv)))).containsEntry(Measures.CURRENCY_EXPOSURE, Result.success(MultiCurrencyScenarioArray.of(ImmutableList.of(expectedCurrencyExp)))).containsEntry(Measures.CURRENT_CASH, Result.success(CurrencyScenarioArray.of(ImmutableList.of(expectedCash)))).containsEntry(Measures.FORWARD_FX_RATE, Result.success(ScenarioArray.of(ImmutableList.of(expectedForwardFx)))).containsEntry(Measures.RESOLVED_TARGET, Result.success(RTRADE));
	  }

	  public virtual void test_pv01()
	  {
		FxNdfTradeCalculationFunction function = new FxNdfTradeCalculationFunction();
		ScenarioMarketData md = marketData();
		RatesProvider provider = RATES_LOOKUP.ratesProvider(md.scenario(0));
		DiscountingFxNdfTradePricer pricer = DiscountingFxNdfTradePricer.DEFAULT;
		PointSensitivities pvPointSens = pricer.presentValueSensitivity(RTRADE, provider);
		CurrencyParameterSensitivities pvParamSens = provider.parameterSensitivity(pvPointSens);
		MultiCurrencyAmount expectedPv01 = pvParamSens.total().multipliedBy(1e-4);
		CurrencyParameterSensitivities expectedBucketedPv01 = pvParamSens.multipliedBy(1e-4);

		ISet<Measure> measures = ImmutableSet.of(Measures.PV01_CALIBRATED_SUM, Measures.PV01_CALIBRATED_BUCKETED);
		assertThat(function.calculate(TRADE, measures, PARAMS, md, REF_DATA)).containsEntry(Measures.PV01_CALIBRATED_SUM, Result.success(MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv01)))).containsEntry(Measures.PV01_CALIBRATED_BUCKETED, Result.success(ScenarioArray.of(ImmutableList.of(expectedBucketedPv01))));
	  }

	  //-------------------------------------------------------------------------
	  internal static ScenarioMarketData marketData()
	  {
		Curve curve1 = ConstantCurve.of(Curves.discountFactors("Test", ACT_360), 0.992);
		Curve curve2 = ConstantCurve.of(Curves.discountFactors("Test", ACT_360), 0.991);
		TestMarketDataMap md = new TestMarketDataMap(VAL_DATE, ImmutableMap.of(DISCOUNT_CURVE_GBP_ID, curve1, DISCOUNT_CURVE_USD_ID, curve2, FxRateId.of(GBP, USD), FxRate.of(GBP, USD, 1.62)), ImmutableMap.of());
		return md;
	  }

	}

}