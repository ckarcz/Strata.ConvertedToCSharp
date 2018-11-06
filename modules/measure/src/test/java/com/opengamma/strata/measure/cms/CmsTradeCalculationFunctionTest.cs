using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.cms
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueAdjustment = com.opengamma.strata.basics.value.ValueAdjustment;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using ValueStep = com.opengamma.strata.basics.value.ValueStep;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using Result = com.opengamma.strata.collect.result.Result;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;
	using TestMarketDataMap = com.opengamma.strata.measure.curve.TestMarketDataMap;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using SwaptionMarketDataLookup = com.opengamma.strata.measure.swaption.SwaptionMarketDataLookup;
	using SabrExtrapolationReplicationCmsLegPricer = com.opengamma.strata.pricer.cms.SabrExtrapolationReplicationCmsLegPricer;
	using SabrExtrapolationReplicationCmsPeriodPricer = com.opengamma.strata.pricer.cms.SabrExtrapolationReplicationCmsPeriodPricer;
	using SabrExtrapolationReplicationCmsProductPricer = com.opengamma.strata.pricer.cms.SabrExtrapolationReplicationCmsProductPricer;
	using SabrExtrapolationReplicationCmsTradePricer = com.opengamma.strata.pricer.cms.SabrExtrapolationReplicationCmsTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SabrSwaptionVolatilities = com.opengamma.strata.pricer.swaption.SabrSwaptionVolatilities;
	using SwaptionSabrRateVolatilityDataSet = com.opengamma.strata.pricer.swaption.SwaptionSabrRateVolatilityDataSet;
	using SwaptionVolatilitiesId = com.opengamma.strata.pricer.swaption.SwaptionVolatilitiesId;
	using Cms = com.opengamma.strata.product.cms.Cms;
	using CmsLeg = com.opengamma.strata.product.cms.CmsLeg;
	using CmsTrade = com.opengamma.strata.product.cms.CmsTrade;
	using ResolvedCmsTrade = com.opengamma.strata.product.cms.ResolvedCmsTrade;
	using SwapIndex = com.opengamma.strata.product.swap.SwapIndex;
	using SwapIndices = com.opengamma.strata.product.swap.SwapIndices;

	/// <summary>
	/// Test <seealso cref="CmsTradeCalculationFunction"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CmsTradeCalculationFunctionTest
	public class CmsTradeCalculationFunctionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly SwapIndex SWAP_INDEX = SwapIndices.EUR_EURIBOR_1100_5Y;
	  private static readonly LocalDate START = LocalDate.of(2015, 10, 21);
	  private static readonly LocalDate END = LocalDate.of(2020, 10, 21);
	  private static readonly Frequency FREQUENCY = Frequency.P12M;
	  private static readonly BusinessDayAdjustment BUSS_ADJ_EUR = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, EUTA);
	  private static readonly PeriodicSchedule SCHEDULE_EUR = PeriodicSchedule.of(START, END, FREQUENCY, BUSS_ADJ_EUR, StubConvention.NONE, RollConventions.NONE);
	  private static readonly IList<ValueStep> NOTIONAL_STEPS = new List<ValueStep>();
	  private const double NOTIONAL_VALUE_0 = 100_000_000;
	  private const double NOTIONAL_VALUE_1 = 1.1e6;
	  private const double NOTIONAL_VALUE_2 = 0.9e6;
	  private const double NOTIONAL_VALUE_3 = 1.2e6;
	  static CmsTradeCalculationFunctionTest()
	  {
		NOTIONAL_STEPS.Add(ValueStep.of(1, ValueAdjustment.ofReplace(NOTIONAL_VALUE_1)));
		NOTIONAL_STEPS.Add(ValueStep.of(2, ValueAdjustment.ofReplace(NOTIONAL_VALUE_2)));
		NOTIONAL_STEPS.Add(ValueStep.of(3, ValueAdjustment.ofReplace(NOTIONAL_VALUE_3)));
	  }
	  private static readonly ValueSchedule NOTIONAL = ValueSchedule.of(NOTIONAL_VALUE_0, NOTIONAL_STEPS);
	  private static readonly Cms PRODUCT = Cms.of(CmsLeg.builder().index(SWAP_INDEX).notional(NOTIONAL).payReceive(PAY).paymentSchedule(SCHEDULE_EUR).build());
	  public static readonly CmsTrade TRADE = CmsTrade.builder().product(PRODUCT).build();
	  public static readonly ResolvedCmsTrade RTRADE = TRADE.resolve(REF_DATA);

	  private static readonly Currency CURRENCY = PRODUCT.CmsLeg.Currency;
	  public static readonly IborIndex INDEX = (IborIndex) PRODUCT.allRateIndices().GetEnumerator().next();
	  private static readonly CurveId DISCOUNT_CURVE_ID = CurveId.of("Default", "Discount");
	  private static readonly CurveId FORWARD_CURVE_ID = CurveId.of("Default", "Forward");
	  public static readonly RatesMarketDataLookup RATES_LOOKUP = RatesMarketDataLookup.of(ImmutableMap.of(CURRENCY, DISCOUNT_CURVE_ID), ImmutableMap.of(INDEX, FORWARD_CURVE_ID));
	  private static readonly SwaptionVolatilitiesId SWAPTION_ID = SwaptionVolatilitiesId.of("SABRVols");
	  public static readonly SwaptionMarketDataLookup SWAPTION_LOOKUP = SwaptionMarketDataLookup.of(INDEX, SWAPTION_ID);
	  private const double CUT_OFF_STRIKE = 0.10;
	  private const double MU = 2.50;
	  public static readonly CmsSabrExtrapolationParams CMS_MODEL = CmsSabrExtrapolationParams.of(CUT_OFF_STRIKE, MU);
	  private static readonly CalculationParameters PARAMS = CalculationParameters.of(RATES_LOOKUP, SWAPTION_LOOKUP, CMS_MODEL);
	  private static readonly LocalDate VAL_DATE = START.plusMonths(1);
	  public static readonly SabrSwaptionVolatilities VOLS = SwaptionSabrRateVolatilityDataSet.getVolatilitiesEur(VAL_DATE, false);

	  //-------------------------------------------------------------------------
	  public virtual void test_requirementsAndCurrency()
	  {
		CmsTradeCalculationFunction function = new CmsTradeCalculationFunction();
		ISet<Measure> measures = function.supportedMeasures();
		FunctionRequirements reqs = function.requirements(TRADE, measures, PARAMS, REF_DATA);
		assertThat(reqs.OutputCurrencies).containsOnly(CURRENCY);
		assertThat(reqs.ValueRequirements).isEqualTo(ImmutableSet.of(DISCOUNT_CURVE_ID, FORWARD_CURVE_ID, SWAPTION_ID));
		assertThat(reqs.TimeSeriesRequirements).isEqualTo(ImmutableSet.of(IndexQuoteId.of(INDEX)));
		assertThat(function.naturalCurrency(TRADE, REF_DATA)).isEqualTo(CURRENCY);
	  }

	  public virtual void test_simpleMeasures()
	  {
		CmsTradeCalculationFunction function = new CmsTradeCalculationFunction();
		ScenarioMarketData md = marketData();
		RatesProvider provider = RATES_LOOKUP.ratesProvider(md.scenario(0));
		SabrExtrapolationReplicationCmsTradePricer pricer = new SabrExtrapolationReplicationCmsTradePricer(new SabrExtrapolationReplicationCmsProductPricer(new SabrExtrapolationReplicationCmsLegPricer(SabrExtrapolationReplicationCmsPeriodPricer.of(CUT_OFF_STRIKE, MU))));
		ResolvedCmsTrade resolved = TRADE.resolve(REF_DATA);
		MultiCurrencyAmount expectedPv = pricer.presentValue(resolved, provider, VOLS);

		ISet<Measure> measures = ImmutableSet.of(Measures.PRESENT_VALUE, Measures.RESOLVED_TARGET);
		assertThat(function.calculate(TRADE, measures, PARAMS, md, REF_DATA)).containsEntry(Measures.PRESENT_VALUE, Result.success(MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv)))).containsEntry(Measures.RESOLVED_TARGET, Result.success(TRADE.resolve(REF_DATA)));
	  }

	  //-------------------------------------------------------------------------
	  internal static ScenarioMarketData marketData()
	  {
		LocalDateDoubleTimeSeries ts = LocalDateDoubleTimeSeries.of(date(2015, 10, 19), 0.013);
		TestMarketDataMap md = new TestMarketDataMap(VAL_DATE, ImmutableMap.of(DISCOUNT_CURVE_ID, SwaptionSabrRateVolatilityDataSet.CURVE_DSC_EUR, FORWARD_CURVE_ID, SwaptionSabrRateVolatilityDataSet.CURVE_FWD_EUR, SWAPTION_ID, VOLS), ImmutableMap.of(IndexQuoteId.of(SWAP_INDEX), ts));
		return md;
	  }

	}

}