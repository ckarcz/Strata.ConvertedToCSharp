using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.LongShort.SHORT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
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
	using RatesProviderDataSets = com.opengamma.strata.pricer.datasets.RatesProviderDataSets;
	using BlackFxOptionSmileVolatilities = com.opengamma.strata.pricer.fxopt.BlackFxOptionSmileVolatilities;
	using BlackFxSingleBarrierOptionTradePricer = com.opengamma.strata.pricer.fxopt.BlackFxSingleBarrierOptionTradePricer;
	using FxOptionVolatilitiesId = com.opengamma.strata.pricer.fxopt.FxOptionVolatilitiesId;
	using FxOptionVolatilitiesName = com.opengamma.strata.pricer.fxopt.FxOptionVolatilitiesName;
	using InterpolatedStrikeSmileDeltaTermStructure = com.opengamma.strata.pricer.fxopt.InterpolatedStrikeSmileDeltaTermStructure;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using FxSingle = com.opengamma.strata.product.fx.FxSingle;
	using FxSingleBarrierOption = com.opengamma.strata.product.fxopt.FxSingleBarrierOption;
	using FxSingleBarrierOptionTrade = com.opengamma.strata.product.fxopt.FxSingleBarrierOptionTrade;
	using FxVanillaOption = com.opengamma.strata.product.fxopt.FxVanillaOption;
	using ResolvedFxSingleBarrierOptionTrade = com.opengamma.strata.product.fxopt.ResolvedFxSingleBarrierOptionTrade;
	using BarrierType = com.opengamma.strata.product.option.BarrierType;
	using KnockType = com.opengamma.strata.product.option.KnockType;
	using SimpleConstantContinuousBarrier = com.opengamma.strata.product.option.SimpleConstantContinuousBarrier;

	/// <summary>
	/// Test <seealso cref="FxSingleBarrierOptionTradeCalculationFunction"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxSingleBarrierOptionTradeCalculationFunctionTest
	public class FxSingleBarrierOptionTradeCalculationFunctionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private static readonly LocalDate VAL_DATE = RatesProviderDataSets.VAL_DATE_2014_01_22;
	  private static readonly ZoneId ZONE = ZoneId.of("Z");
	  private static readonly LocalDate PAYMENT_DATE = LocalDate.of(2014, 5, 13);
	  private const double NOTIONAL = 1.0e6;
	  private static readonly CurrencyAmount EUR_AMOUNT = CurrencyAmount.of(EUR, NOTIONAL);
	  private static readonly CurrencyAmount USD_AMOUNT = CurrencyAmount.of(USD, -NOTIONAL * 1.1d);
	  private static readonly FxSingle FX_PRODUCT = FxSingle.of(EUR_AMOUNT, USD_AMOUNT, PAYMENT_DATE);
	  private static readonly FxVanillaOption VANILLA = FxVanillaOption.builder().longShort(SHORT).expiryDate(LocalDate.of(2014, 5, 9)).expiryTime(LocalTime.of(13, 10)).expiryZone(ZONE).underlying(FX_PRODUCT).build();
	  private static readonly FxSingleBarrierOption OPTION_PRODUCT = FxSingleBarrierOption.builder().underlyingOption(VANILLA).barrier(SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_IN, 1.5)).build();
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.builder().tradeDate(VAL_DATE).build();
	  private static readonly LocalDate CASH_SETTLE_DATE = LocalDate.of(2014, 1, 25);
	  private static readonly AdjustablePayment PREMIUM = AdjustablePayment.of(EUR, NOTIONAL * 0.027, CASH_SETTLE_DATE);
	  public static readonly FxSingleBarrierOptionTrade TRADE = FxSingleBarrierOptionTrade.builder().premium(PREMIUM).product(OPTION_PRODUCT).info(TRADE_INFO).build();
	  public static readonly ResolvedFxSingleBarrierOptionTrade RTRADE = TRADE.resolve(REF_DATA);

	  private static readonly CurveId DISCOUNT_CURVE_EUR_ID = CurveId.of("Default", "Discount-EUR");
	  private static readonly CurveId DISCOUNT_CURVE_USD_ID = CurveId.of("Default", "Discount-USD");
	  internal static readonly RatesMarketDataLookup RATES_LOOKUP = RatesMarketDataLookup.of(ImmutableMap.of(EUR, DISCOUNT_CURVE_EUR_ID, USD, DISCOUNT_CURVE_USD_ID), ImmutableMap.of());

	  private static readonly DoubleArray TIME_TO_EXPIRY = DoubleArray.of(0.01, 0.252, 0.501, 1.0, 2.0, 5.0);
	  private static readonly DoubleArray ATM = DoubleArray.of(0.175, 0.185, 0.18, 0.17, 0.16, 0.16);
	  private static readonly DoubleArray DELTA = DoubleArray.of(0.10, 0.25);
	  private static readonly DoubleMatrix RISK_REVERSAL = DoubleMatrix.ofUnsafe(new double[][]
	  {
		  new double[] {-0.010, -0.0050},
		  new double[] {-0.011, -0.0060},
		  new double[] {-0.012, -0.0070},
		  new double[] {-0.013, -0.0080},
		  new double[] {-0.014, -0.0090},
		  new double[] {-0.014, -0.0090}
	  });
	  private static readonly DoubleMatrix STRANGLE = DoubleMatrix.ofUnsafe(new double[][]
	  {
		  new double[] {0.0300, 0.0100},
		  new double[] {0.0310, 0.0110},
		  new double[] {0.0320, 0.0120},
		  new double[] {0.0330, 0.0130},
		  new double[] {0.0340, 0.0140},
		  new double[] {0.0340, 0.0140}
	  });
	  private static readonly InterpolatedStrikeSmileDeltaTermStructure SMILE_TERM = InterpolatedStrikeSmileDeltaTermStructure.of(TIME_TO_EXPIRY, DELTA, ATM, RISK_REVERSAL, STRANGLE, ACT_365F);
	  private static readonly CurrencyPair CURRENCY_PAIR = CurrencyPair.of(EUR, USD);
	  public static readonly BlackFxOptionSmileVolatilities VOLS = BlackFxOptionSmileVolatilities.of(FxOptionVolatilitiesName.of("Test"), CURRENCY_PAIR, VAL_DATE.atStartOfDay(ZoneOffset.UTC), SMILE_TERM);

	  private static readonly FxOptionVolatilitiesId VOL_ID = FxOptionVolatilitiesId.of("EUR-USD");
	  public static readonly FxOptionMarketDataLookup FX_OPTION_LOOKUP = FxOptionMarketDataLookup.of(CURRENCY_PAIR, VOL_ID);
	  private static readonly CalculationParameters PARAMS = CalculationParameters.of(RATES_LOOKUP, FX_OPTION_LOOKUP);

	  //-------------------------------------------------------------------------
	  public virtual void test_requirementsAndCurrency()
	  {
		FxSingleBarrierOptionTradeCalculationFunction function = new FxSingleBarrierOptionTradeCalculationFunction();
		ISet<Measure> measures = function.supportedMeasures();
		FunctionRequirements reqs = function.requirements(TRADE, measures, PARAMS, REF_DATA);
		assertThat(reqs.OutputCurrencies).containsExactly(EUR, USD);
		assertThat(reqs.ValueRequirements).isEqualTo(ImmutableSet.of(DISCOUNT_CURVE_EUR_ID, DISCOUNT_CURVE_USD_ID, VOL_ID));
		assertThat(reqs.TimeSeriesRequirements).Empty;
		assertThat(function.naturalCurrency(TRADE, REF_DATA)).isEqualTo(EUR);
	  }

	  public virtual void test_simpleMeasures()
	  {
		FxSingleBarrierOptionTradeCalculationFunction function = new FxSingleBarrierOptionTradeCalculationFunction();
		ScenarioMarketData md = marketData();
		RatesProvider provider = RATES_LOOKUP.ratesProvider(md.scenario(0));
		BlackFxSingleBarrierOptionTradePricer pricer = BlackFxSingleBarrierOptionTradePricer.DEFAULT;
		MultiCurrencyAmount expectedPv = pricer.presentValue(RTRADE, provider, VOLS);
		MultiCurrencyAmount expectedCurrencyExp = pricer.currencyExposure(RTRADE, provider, VOLS);
		CurrencyAmount expectedCash = pricer.currentCash(RTRADE, VAL_DATE);

		ISet<Measure> measures = ImmutableSet.of(Measures.PRESENT_VALUE, Measures.PAR_SPREAD, Measures.CURRENCY_EXPOSURE, Measures.CURRENT_CASH, Measures.RESOLVED_TARGET);
		assertThat(function.calculate(TRADE, measures, PARAMS, md, REF_DATA)).containsEntry(Measures.PRESENT_VALUE, Result.success(MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv)))).containsEntry(Measures.CURRENCY_EXPOSURE, Result.success(MultiCurrencyScenarioArray.of(ImmutableList.of(expectedCurrencyExp)))).containsEntry(Measures.CURRENT_CASH, Result.success(CurrencyScenarioArray.of(ImmutableList.of(expectedCash)))).containsEntry(Measures.RESOLVED_TARGET, Result.success(RTRADE));
	  }

	  public virtual void test_pv01()
	  {
		FxSingleBarrierOptionTradeCalculationFunction function = new FxSingleBarrierOptionTradeCalculationFunction();
		ScenarioMarketData md = marketData();
		RatesProvider provider = RATES_LOOKUP.ratesProvider(md.scenario(0));
		BlackFxSingleBarrierOptionTradePricer pricer = BlackFxSingleBarrierOptionTradePricer.DEFAULT;
		PointSensitivities pvPointSens = pricer.presentValueSensitivityRatesStickyStrike(RTRADE, provider, VOLS);
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
		TestMarketDataMap md = new TestMarketDataMap(VAL_DATE, ImmutableMap.of(DISCOUNT_CURVE_EUR_ID, curve1, DISCOUNT_CURVE_USD_ID, curve2, VOL_ID, VOLS, FxRateId.of(EUR, USD), FxRate.of(EUR, USD, 1.62)), ImmutableMap.of());
		return md;
	  }

	}

}