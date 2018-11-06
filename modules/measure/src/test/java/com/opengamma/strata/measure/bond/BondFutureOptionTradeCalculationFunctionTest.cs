using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
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
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Result = com.opengamma.strata.collect.result.Result;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using FieldName = com.opengamma.strata.data.FieldName;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using ValueType = com.opengamma.strata.market.ValueType;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using LegalEntityGroup = com.opengamma.strata.market.curve.LegalEntityGroup;
	using RepoGroup = com.opengamma.strata.market.curve.RepoGroup;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using LogMoneynessStrike = com.opengamma.strata.market.option.LogMoneynessStrike;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using DefaultSurfaceMetadata = com.opengamma.strata.market.surface.DefaultSurfaceMetadata;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using SurfaceName = com.opengamma.strata.market.surface.SurfaceName;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;
	using TestMarketDataMap = com.opengamma.strata.measure.curve.TestMarketDataMap;
	using BlackBondFutureExpiryLogMoneynessVolatilities = com.opengamma.strata.pricer.bond.BlackBondFutureExpiryLogMoneynessVolatilities;
	using BlackBondFutureOptionMarginedTradePricer = com.opengamma.strata.pricer.bond.BlackBondFutureOptionMarginedTradePricer;
	using BondDataSets = com.opengamma.strata.pricer.bond.BondDataSets;
	using BondFutureVolatilitiesId = com.opengamma.strata.pricer.bond.BondFutureVolatilitiesId;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;
	using GenericVolatilitySurfaceYearFractionParameterMetadata = com.opengamma.strata.pricer.common.GenericVolatilitySurfaceYearFractionParameterMetadata;
	using LegalEntityDiscountingProviderDataSets = com.opengamma.strata.pricer.datasets.LegalEntityDiscountingProviderDataSets;
	using LegalEntityId = com.opengamma.strata.product.LegalEntityId;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using BondFutureOption = com.opengamma.strata.product.bond.BondFutureOption;
	using BondFutureOptionTrade = com.opengamma.strata.product.bond.BondFutureOptionTrade;
	using ResolvedBondFutureOptionTrade = com.opengamma.strata.product.bond.ResolvedBondFutureOptionTrade;

	/// <summary>
	/// Test <seealso cref="BondFutureTradeCalculationFunction"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BondFutureOptionTradeCalculationFunctionTest
	public class BondFutureOptionTradeCalculationFunctionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  // product and trade
	  private static readonly BondFutureOption PRODUCT = BondDataSets.FUTURE_OPTION_PRODUCT_EUR_115;
	  private static readonly BondFutureOptionTrade TRADE = BondDataSets.FUTURE_OPTION_TRADE_EUR;
	  public static readonly ResolvedBondFutureOptionTrade RTRADE = TRADE.resolve(REF_DATA);
	  // curves
	  private static readonly LegalEntityDiscountingProvider LED_PROVIDER = LegalEntityDiscountingProviderDataSets.ISSUER_REPO_ZERO_EUR;
	  // vol surface
	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly DoubleArray TIME = DoubleArray.of(0.20, 0.20, 0.20, 0.20, 0.20, 0.45, 0.45, 0.45, 0.45, 0.45);
	  private static readonly DoubleArray MONEYNESS = DoubleArray.of(-0.050, -0.005, 0.000, 0.005, 0.050, -0.050, -0.005, 0.000, 0.005, 0.050);
	  private static readonly DoubleArray VOL = DoubleArray.of(0.50, 0.49, 0.47, 0.48, 0.51, 0.45, 0.44, 0.42, 0.43, 0.46);
	  private static readonly SurfaceMetadata METADATA;
	  static BondFutureOptionTradeCalculationFunctionTest()
	  {
		IList<GenericVolatilitySurfaceYearFractionParameterMetadata> list = new List<GenericVolatilitySurfaceYearFractionParameterMetadata>();
		int nData = TIME.size();
		for (int i = 0; i < nData; ++i)
		{
		  GenericVolatilitySurfaceYearFractionParameterMetadata parameterMetadata = GenericVolatilitySurfaceYearFractionParameterMetadata.of(TIME.get(i), LogMoneynessStrike.of(MONEYNESS.get(i)));
		  list.Add(parameterMetadata);
		}
		METADATA = DefaultSurfaceMetadata.builder().surfaceName(SurfaceName.of("GOVT1-BOND-FUT-VOL")).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.LOG_MONEYNESS).zValueType(ValueType.BLACK_VOLATILITY).parameterMetadata(list).dayCount(ACT_365F).build();
	  }
	  private static readonly InterpolatedNodalSurface SURFACE = InterpolatedNodalSurface.of(METADATA, TIME, MONEYNESS, VOL, INTERPOLATOR_2D);
	  private static readonly LocalDate VAL_DATE = LED_PROVIDER.ValuationDate;
	  private static readonly LocalTime VAL_TIME = LocalTime.of(0, 0);
	  private static readonly ZoneId ZONE = PRODUCT.Expiry.Zone;
	  private static readonly ZonedDateTime VAL_DATE_TIME = VAL_DATE.atTime(VAL_TIME).atZone(ZONE);
	  private static readonly BlackBondFutureExpiryLogMoneynessVolatilities VOLS = BlackBondFutureExpiryLogMoneynessVolatilities.of(VAL_DATE_TIME, SURFACE);
	  private const double SETTLE_PRICE = 0.01;

	  private static readonly LegalEntityId ISSUER_ID = PRODUCT.UnderlyingFuture.DeliveryBasket.get(0).LegalEntityId;
	  private static readonly SecurityId FUTURE_SEC_ID = PRODUCT.UnderlyingFuture.SecurityId;
	  private static readonly RepoGroup REPO_GROUP = RepoGroup.of("Repo");
	  private static readonly LegalEntityGroup ISSUER_GROUP = LegalEntityGroup.of("Issuer");
	  private static readonly Currency CURRENCY = TRADE.Product.Currency;
	  private static readonly QuoteId QUOTE_ID = QuoteId.of(PRODUCT.SecurityId.StandardId, FieldName.SETTLEMENT_PRICE);
	  private static readonly CurveId REPO_CURVE_ID = CurveId.of("Default", "Repo");
	  private static readonly CurveId ISSUER_CURVE_ID = CurveId.of("Default", "Issuer");
	  private static readonly BondFutureVolatilitiesId VOLS_ID = BondFutureVolatilitiesId.of("Vols");
	  public static readonly LegalEntityDiscountingMarketDataLookup LED_LOOKUP = LegalEntityDiscountingMarketDataLookup.of(ImmutableMap.of(ISSUER_ID, REPO_GROUP), ImmutableMap.of(Pair.of(REPO_GROUP, CURRENCY), REPO_CURVE_ID), ImmutableMap.of(ISSUER_ID, ISSUER_GROUP), ImmutableMap.of(Pair.of(ISSUER_GROUP, CURRENCY), ISSUER_CURVE_ID));
	  public static readonly BondFutureOptionMarketDataLookup VOL_LOOKUP = BondFutureOptionMarketDataLookup.of(ImmutableMap.of(FUTURE_SEC_ID, VOLS_ID));
	  private static readonly CalculationParameters PARAMS = CalculationParameters.of(LED_LOOKUP, VOL_LOOKUP);

	  //-------------------------------------------------------------------------
	  public virtual void test_requirementsAndCurrency()
	  {
		BondFutureOptionTradeCalculationFunction<BondFutureOptionTrade> function = BondFutureOptionTradeCalculationFunction.TRADE;
		ISet<Measure> measures = function.supportedMeasures();
		FunctionRequirements reqs = function.requirements(TRADE, measures, PARAMS, REF_DATA);
		assertThat(reqs.OutputCurrencies).containsOnly(CURRENCY);
		assertThat(reqs.ValueRequirements).isEqualTo(ImmutableSet.of(QUOTE_ID, REPO_CURVE_ID, ISSUER_CURVE_ID, VOLS_ID));
		assertThat(reqs.TimeSeriesRequirements).isEqualTo(ImmutableSet.of());
		assertThat(function.naturalCurrency(TRADE, REF_DATA)).isEqualTo(CURRENCY);
	  }

	  public virtual void test_simpleMeasures()
	  {
		BondFutureOptionTradeCalculationFunction<BondFutureOptionTrade> function = BondFutureOptionTradeCalculationFunction.TRADE;
		ScenarioMarketData md = marketData();
		LegalEntityDiscountingProvider provider = LED_LOOKUP.marketDataView(md.scenario(0)).discountingProvider();
		BlackBondFutureOptionMarginedTradePricer pricer = BlackBondFutureOptionMarginedTradePricer.DEFAULT;
		CurrencyAmount expectedPv = pricer.presentValue(RTRADE, provider, VOLS, SETTLE_PRICE);
		MultiCurrencyAmount expectedCurrencyExposure = pricer.currencyExposure(RTRADE, provider, VOLS, SETTLE_PRICE);

		ISet<Measure> measures = ImmutableSet.of(Measures.PRESENT_VALUE, Measures.CURRENCY_EXPOSURE, Measures.RESOLVED_TARGET);
		assertThat(function.calculate(TRADE, measures, PARAMS, md, REF_DATA)).containsEntry(Measures.PRESENT_VALUE, Result.success(CurrencyScenarioArray.of(ImmutableList.of(expectedPv)))).containsEntry(Measures.CURRENCY_EXPOSURE, Result.success(MultiCurrencyScenarioArray.of(ImmutableList.of(expectedCurrencyExposure)))).containsEntry(Measures.RESOLVED_TARGET, Result.success(RTRADE));
	  }

	  public virtual void test_pv01()
	  {
		BondFutureOptionTradeCalculationFunction<BondFutureOptionTrade> function = BondFutureOptionTradeCalculationFunction.TRADE;
		ScenarioMarketData md = marketData();
		LegalEntityDiscountingProvider provider = LED_LOOKUP.marketDataView(md.scenario(0)).discountingProvider();
		BlackBondFutureOptionMarginedTradePricer pricer = BlackBondFutureOptionMarginedTradePricer.DEFAULT;
		PointSensitivities pvPointSens = pricer.presentValueSensitivityRates(RTRADE, provider, VOLS);
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
		return new TestMarketDataMap(VAL_DATE, ImmutableMap.of(REPO_CURVE_ID, curve, ISSUER_CURVE_ID, curve, QUOTE_ID, SETTLE_PRICE * 100, VOLS_ID, VOLS), ImmutableMap.of());
	  }

	}

}