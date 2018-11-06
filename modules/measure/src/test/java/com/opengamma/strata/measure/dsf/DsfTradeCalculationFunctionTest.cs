using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.dsf
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.SAT_SUN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using Result = com.opengamma.strata.collect.result.Result;
	using FieldName = com.opengamma.strata.data.FieldName;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using DoubleScenarioArray = com.opengamma.strata.data.scenario.DoubleScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using TestMarketDataMap = com.opengamma.strata.measure.curve.TestMarketDataMap;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using DiscountingDsfTradePricer = com.opengamma.strata.pricer.dsf.DiscountingDsfTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using Dsf = com.opengamma.strata.product.dsf.Dsf;
	using DsfTrade = com.opengamma.strata.product.dsf.DsfTrade;
	using ResolvedDsfTrade = com.opengamma.strata.product.dsf.ResolvedDsfTrade;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;
	using FixedRateSwapLegConvention = com.opengamma.strata.product.swap.type.FixedRateSwapLegConvention;
	using IborRateSwapLegConvention = com.opengamma.strata.product.swap.type.IborRateSwapLegConvention;

	/// <summary>
	/// Test <seealso cref="DsfTradeCalculationFunction"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DsfTradeCalculationFunctionTest
	public class DsfTradeCalculationFunctionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly BusinessDayAdjustment BDA_MF = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, SAT_SUN);
	  private static readonly SwapLeg FIXED_LEG = FixedRateSwapLegConvention.of(Currency.GBP, DayCounts.ACT_360, Frequency.P6M, BDA_MF).toLeg(LocalDate.of(2013, 6, 30), LocalDate.of(2016, 6, 30), PayReceive.RECEIVE, 1, 0.001);
	  private static readonly SwapLeg IBOR_LEG = IborRateSwapLegConvention.of(IborIndices.GBP_LIBOR_6M).toLeg(LocalDate.of(2013, 6, 30), LocalDate.of(2016, 6, 30), PayReceive.PAY, 1);
	  private static readonly Swap SWAP = Swap.of(FIXED_LEG, IBOR_LEG);
	  private static readonly LocalDate LAST_TRADE = LocalDate.of(2013, 6, 17);
	  private static readonly LocalDate DELIVERY = LocalDate.of(2013, 6, 19);
	  private const double NOTIONAL = 100000;
	  private static readonly StandardId DSF_ID = StandardId.of("OG-Ticker", "DSF1");
	  private static readonly Dsf FUTURE = Dsf.builder().securityId(SecurityId.of(DSF_ID)).deliveryDate(DELIVERY).lastTradeDate(LAST_TRADE).notional(NOTIONAL).underlyingSwap(SWAP).build();
	  private const double TRADE_PRICE = 0.98 + 31.0 / 32.0 / 100.0; // price quoted in 32nd of 1%
	  public const double REF_PRICE = 0.98 + 30.0 / 32.0 / 100.0; // price quoted in 32nd of 1%
	  private const long QUANTITY = 1234L;
	  public static readonly DsfTrade TRADE = DsfTrade.builder().info(TradeInfo.of(LocalDate.of(2013, 6, 15))).product(FUTURE).quantity(QUANTITY).price(TRADE_PRICE).build();
	  public static readonly ResolvedDsfTrade RTRADE = TRADE.resolve(REF_DATA);
	  private static readonly Currency CURRENCY = SWAP.PayLeg.get().Currency;
	  public static readonly IborIndex INDEX = (IborIndex) SWAP.allIndices().GetEnumerator().next();
	  private static readonly CurveId DISCOUNT_CURVE_ID = CurveId.of("Default", "Discount");
	  private static readonly CurveId FORWARD_CURVE_ID = CurveId.of("Default", "Forward");
	  public static readonly RatesMarketDataLookup RATES_LOOKUP = RatesMarketDataLookup.of(ImmutableMap.of(CURRENCY, DISCOUNT_CURVE_ID), ImmutableMap.of(INDEX, FORWARD_CURVE_ID));
	  private static readonly CalculationParameters PARAMS = CalculationParameters.of(RATES_LOOKUP);
	  private static readonly LocalDate VAL_DATE = LAST_TRADE.minusDays(7);
	  private static readonly QuoteId QUOTE_KEY = QuoteId.of(DSF_ID, FieldName.SETTLEMENT_PRICE);

	  //-------------------------------------------------------------------------
	  public virtual void test_requirementsAndCurrency()
	  {
		DsfTradeCalculationFunction<DsfTrade> function = DsfTradeCalculationFunction.TRADE;
		ISet<Measure> measures = function.supportedMeasures();
		FunctionRequirements reqs = function.requirements(TRADE, measures, PARAMS, REF_DATA);
		assertThat(reqs.OutputCurrencies).containsOnly(CURRENCY);
		assertThat(reqs.ValueRequirements).isEqualTo(ImmutableSet.of(QUOTE_KEY, DISCOUNT_CURVE_ID, FORWARD_CURVE_ID));
		assertThat(reqs.TimeSeriesRequirements).isEqualTo(ImmutableSet.of(IndexQuoteId.of(INDEX)));
		assertThat(function.naturalCurrency(TRADE, REF_DATA)).isEqualTo(CURRENCY);
	  }

	  public virtual void test_simpleMeasures()
	  {
		DsfTradeCalculationFunction<DsfTrade> function = DsfTradeCalculationFunction.TRADE;
		ScenarioMarketData md = marketData();
		RatesProvider provider = RATES_LOOKUP.ratesProvider(md.scenario(0));
		DiscountingDsfTradePricer pricer = DiscountingDsfTradePricer.DEFAULT;
		double expectedPrice = pricer.price(RTRADE, provider);
		CurrencyAmount expectedPv = pricer.presentValue(RTRADE, provider, REF_PRICE);
		MultiCurrencyAmount expectedCurrencyExposure = pricer.currencyExposure(RTRADE, provider, REF_PRICE);

		ISet<Measure> measures = ImmutableSet.of(Measures.UNIT_PRICE, Measures.PRESENT_VALUE, Measures.CURRENCY_EXPOSURE, Measures.RESOLVED_TARGET);
		assertThat(function.calculate(TRADE, measures, PARAMS, md, REF_DATA)).containsEntry(Measures.UNIT_PRICE, Result.success(DoubleScenarioArray.of(ImmutableList.of(expectedPrice)))).containsEntry(Measures.PRESENT_VALUE, Result.success(CurrencyScenarioArray.of(ImmutableList.of(expectedPv)))).containsEntry(Measures.CURRENCY_EXPOSURE, Result.success(MultiCurrencyScenarioArray.of(ImmutableList.of(expectedCurrencyExposure)))).containsEntry(Measures.RESOLVED_TARGET, Result.success(RTRADE));
	  }

	  public virtual void test_pv01()
	  {
		DsfTradeCalculationFunction<DsfTrade> function = DsfTradeCalculationFunction.TRADE;
		ScenarioMarketData md = marketData();
		RatesProvider provider = RATES_LOOKUP.ratesProvider(md.scenario(0));
		DiscountingDsfTradePricer pricer = DiscountingDsfTradePricer.DEFAULT;
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
		Curve curve = ConstantCurve.of(Curves.discountFactors("Test", ACT_360), 0.99);
		TestMarketDataMap md = new TestMarketDataMap(VAL_DATE, ImmutableMap.of(DISCOUNT_CURVE_ID, curve, FORWARD_CURVE_ID, curve, QUOTE_KEY, REF_PRICE), ImmutableMap.of());
		return md;
	  }

	}

}