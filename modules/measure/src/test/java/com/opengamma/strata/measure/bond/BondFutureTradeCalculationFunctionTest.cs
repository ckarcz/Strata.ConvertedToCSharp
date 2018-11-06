using System.Collections.Generic;

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
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using DoubleMath = com.google.common.math.DoubleMath;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Result = com.opengamma.strata.collect.result.Result;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using FieldName = com.opengamma.strata.data.FieldName;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveParameterSize = com.opengamma.strata.market.curve.CurveParameterSize;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using JacobianCalibrationMatrix = com.opengamma.strata.market.curve.JacobianCalibrationMatrix;
	using LegalEntityGroup = com.opengamma.strata.market.curve.LegalEntityGroup;
	using RepoGroup = com.opengamma.strata.market.curve.RepoGroup;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using TestMarketDataMap = com.opengamma.strata.measure.curve.TestMarketDataMap;
	using BondDataSets = com.opengamma.strata.pricer.bond.BondDataSets;
	using DiscountingBondFutureTradePricer = com.opengamma.strata.pricer.bond.DiscountingBondFutureTradePricer;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using LegalEntityId = com.opengamma.strata.product.LegalEntityId;
	using BondFuture = com.opengamma.strata.product.bond.BondFuture;
	using BondFutureTrade = com.opengamma.strata.product.bond.BondFutureTrade;
	using ResolvedBondFutureTrade = com.opengamma.strata.product.bond.ResolvedBondFutureTrade;

	/// <summary>
	/// Test <seealso cref="BondFutureTradeCalculationFunction"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BondFutureTradeCalculationFunctionTest
	public class BondFutureTradeCalculationFunctionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private static readonly BondFuture PRODUCT = BondDataSets.FUTURE_PRODUCT_USD;
	  private static readonly BondFutureTrade TRADE = BondDataSets.FUTURE_TRADE_USD;
	  public static readonly ResolvedBondFutureTrade RTRADE = TRADE.resolve(REF_DATA);
	  public const double SETTLE_PRICE = BondDataSets.SETTLE_PRICE_USD;

	  private static readonly LegalEntityId ISSUER_ID = PRODUCT.DeliveryBasket.get(0).LegalEntityId;
	  private static readonly RepoGroup REPO_GROUP = RepoGroup.of("Repo");
	  private static readonly LegalEntityGroup ISSUER_GROUP = LegalEntityGroup.of("Issuer");
	  private static readonly Currency CURRENCY = TRADE.Product.Currency;
	  private static readonly QuoteId QUOTE_ID = QuoteId.of(PRODUCT.SecurityId.StandardId, FieldName.SETTLEMENT_PRICE);
	  private static readonly CurveId REPO_CURVE_ID = CurveId.of("Default", "Repo");
	  private static readonly CurveId ISSUER_CURVE_ID = CurveId.of("Default", "Issuer");
	  public static readonly LegalEntityDiscountingMarketDataLookup LOOKUP = LegalEntityDiscountingMarketDataLookup.of(ImmutableMap.of(ISSUER_ID, REPO_GROUP), ImmutableMap.of(Pair.of(REPO_GROUP, CURRENCY), REPO_CURVE_ID), ImmutableMap.of(ISSUER_ID, ISSUER_GROUP), ImmutableMap.of(Pair.of(ISSUER_GROUP, CURRENCY), ISSUER_CURVE_ID));
	  private static readonly CalculationParameters PARAMS = CalculationParameters.of(LOOKUP);
	  private static readonly LocalDate VAL_DATE = TRADE.Product.FirstNoticeDate.minusDays(7);
	  private static readonly MarketQuoteSensitivityCalculator MQ_CALC = MarketQuoteSensitivityCalculator.DEFAULT;

	  //-------------------------------------------------------------------------
	  public virtual void test_requirementsAndCurrency()
	  {
		BondFutureTradeCalculationFunction<BondFutureTrade> function = BondFutureTradeCalculationFunction.TRADE;
		ISet<Measure> measures = function.supportedMeasures();
		FunctionRequirements reqs = function.requirements(TRADE, measures, PARAMS, REF_DATA);
		assertThat(reqs.OutputCurrencies).containsOnly(CURRENCY);
		assertThat(reqs.ValueRequirements).isEqualTo(ImmutableSet.of(QUOTE_ID, REPO_CURVE_ID, ISSUER_CURVE_ID));
		assertThat(reqs.TimeSeriesRequirements).isEqualTo(ImmutableSet.of());
		assertThat(function.naturalCurrency(TRADE, REF_DATA)).isEqualTo(CURRENCY);
	  }

	  public virtual void test_simpleMeasures()
	  {
		BondFutureTradeCalculationFunction<BondFutureTrade> function = BondFutureTradeCalculationFunction.TRADE;
		ScenarioMarketData md = marketData();
		LegalEntityDiscountingProvider provider = LOOKUP.marketDataView(md.scenario(0)).discountingProvider();
		DiscountingBondFutureTradePricer pricer = DiscountingBondFutureTradePricer.DEFAULT;
		CurrencyAmount expectedPv = pricer.presentValue(RTRADE, provider, SETTLE_PRICE);
		MultiCurrencyAmount expectedCurrencyExposure = pricer.currencyExposure(RTRADE, provider, SETTLE_PRICE);

		ISet<Measure> measures = ImmutableSet.of(Measures.PRESENT_VALUE, Measures.CURRENCY_EXPOSURE, Measures.RESOLVED_TARGET);
		assertThat(function.calculate(TRADE, measures, PARAMS, md, REF_DATA)).containsEntry(Measures.PRESENT_VALUE, Result.success(CurrencyScenarioArray.of(ImmutableList.of(expectedPv)))).containsEntry(Measures.CURRENCY_EXPOSURE, Result.success(MultiCurrencyScenarioArray.of(ImmutableList.of(expectedCurrencyExposure)))).containsEntry(Measures.RESOLVED_TARGET, Result.success(RTRADE));
	  }

	  public virtual void test_pv01_calibrated()
	  {
		BondFutureTradeCalculationFunction<BondFutureTrade> function = BondFutureTradeCalculationFunction.TRADE;
		ScenarioMarketData md = marketData();
		LegalEntityDiscountingProvider provider = LOOKUP.marketDataView(md.scenario(0)).discountingProvider();
		DiscountingBondFutureTradePricer pricer = DiscountingBondFutureTradePricer.DEFAULT;
		PointSensitivities pvPointSens = pricer.presentValueSensitivity(RTRADE, provider);
		CurrencyParameterSensitivities pvParamSens = provider.parameterSensitivity(pvPointSens);
		MultiCurrencyAmount expectedPv01Cal = pvParamSens.total().multipliedBy(1e-4);
		CurrencyParameterSensitivities expectedPv01CalBucketed = pvParamSens.multipliedBy(1e-4);

		ISet<Measure> measures = ImmutableSet.of(Measures.PV01_CALIBRATED_SUM, Measures.PV01_CALIBRATED_BUCKETED);
		assertThat(function.calculate(TRADE, measures, PARAMS, md, REF_DATA)).containsEntry(Measures.PV01_CALIBRATED_SUM, Result.success(MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv01Cal)))).containsEntry(Measures.PV01_CALIBRATED_BUCKETED, Result.success(ScenarioArray.of(ImmutableList.of(expectedPv01CalBucketed))));
	  }

	  public virtual void test_pv01_quote()
	  {
		BondFutureTradeCalculationFunction<BondFutureTrade> function = BondFutureTradeCalculationFunction.TRADE;
		ScenarioMarketData md = marketData();
		LegalEntityDiscountingProvider provider = LOOKUP.marketDataView(md.scenario(0)).discountingProvider();
		DiscountingBondFutureTradePricer pricer = DiscountingBondFutureTradePricer.DEFAULT;
		PointSensitivities pvPointSens = pricer.presentValueSensitivity(RTRADE, provider);
		CurrencyParameterSensitivities pvParamSens = provider.parameterSensitivity(pvPointSens);
		CurrencyParameterSensitivities expectedPv01CalBucketed = MQ_CALC.sensitivity(pvParamSens.multipliedBy(1e-4), provider);
		MultiCurrencyAmount expectedPv01Cal = expectedPv01CalBucketed.total();

		ISet<Measure> measures = ImmutableSet.of(Measures.PV01_MARKET_QUOTE_SUM, Measures.PV01_MARKET_QUOTE_BUCKETED);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> computed = function.calculate(TRADE, measures, PARAMS, md, REF_DATA);
		IDictionary<Measure, Result<object>> computed = function.calculate(TRADE, measures, PARAMS, md, REF_DATA);
		MultiCurrencyScenarioArray sumComputed = (MultiCurrencyScenarioArray) computed[Measures.PV01_MARKET_QUOTE_SUM].Value;
		ScenarioArray<CurrencyParameterSensitivities> bucketedComputed = (ScenarioArray<CurrencyParameterSensitivities>) computed[Measures.PV01_MARKET_QUOTE_BUCKETED].Value;
		assertEquals(sumComputed.ScenarioCount, 1);
		assertEquals(sumComputed.get(0).Currencies, ImmutableSet.of(USD));
		assertTrue(DoubleMath.fuzzyEquals(sumComputed.get(0).getAmount(USD).Amount, expectedPv01Cal.getAmount(USD).Amount, 1.0e-10));
		assertEquals(bucketedComputed.ScenarioCount, 1);
		assertTrue(bucketedComputed.get(0).equalWithTolerance(expectedPv01CalBucketed, 1.0e-10));
	  }

	  //-------------------------------------------------------------------------
	  internal static ScenarioMarketData marketData()
	  {
		CurveParameterSize issuerSize = CurveParameterSize.of(ISSUER_CURVE_ID.CurveName, 3);
		CurveParameterSize repoSize = CurveParameterSize.of(REPO_CURVE_ID.CurveName, 2);
		JacobianCalibrationMatrix issuerMatrix = JacobianCalibrationMatrix.of(ImmutableList.of(issuerSize, repoSize), DoubleMatrix.copyOf(new double[][]
		{
			new double[] {0.95, 0.03, 0.01, 0.006, 0.004},
			new double[] {0.03, 0.95, 0.01, 0.005, 0.005},
			new double[] {0.03, 0.01, 0.95, 0.002, 0.008}
		}));
		JacobianCalibrationMatrix repoMatrix = JacobianCalibrationMatrix.of(ImmutableList.of(issuerSize, repoSize), DoubleMatrix.copyOf(new double[][]
		{
			new double[] {0.003, 0.003, 0.004, 0.97, 0.02},
			new double[] {0.003, 0.006, 0.001, 0.05, 0.94}
		}));
		CurveMetadata issuerMetadata = Curves.zeroRates(ISSUER_CURVE_ID.CurveName, ACT_360).withInfo(CurveInfoType.JACOBIAN, issuerMatrix);
		CurveMetadata repoMetadata = Curves.zeroRates(REPO_CURVE_ID.CurveName, ACT_360).withInfo(CurveInfoType.JACOBIAN, repoMatrix);
		Curve issuerCurve = InterpolatedNodalCurve.of(issuerMetadata, DoubleArray.of(1.0, 5.0, 10.0), DoubleArray.of(0.02, 0.04, 0.01), CurveInterpolators.LINEAR);
		Curve repoCurve = InterpolatedNodalCurve.of(repoMetadata, DoubleArray.of(0.5, 3.0), DoubleArray.of(0.005, 0.008), CurveInterpolators.LINEAR);
		return new TestMarketDataMap(VAL_DATE, ImmutableMap.of(REPO_CURVE_ID, repoCurve, ISSUER_CURVE_ID, issuerCurve, QUOTE_ID, SETTLE_PRICE * 100), ImmutableMap.of());
	  }

	}

}