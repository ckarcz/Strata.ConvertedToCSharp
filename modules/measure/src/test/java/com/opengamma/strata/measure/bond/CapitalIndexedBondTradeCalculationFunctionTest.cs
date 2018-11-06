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
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ICMA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.US_CPI_U;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.bond.CapitalIndexedBondYieldConvention.US_IL_REAL;
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
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using Result = com.opengamma.strata.collect.result.Result;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using LegalEntityGroup = com.opengamma.strata.market.curve.LegalEntityGroup;
	using RepoGroup = com.opengamma.strata.market.curve.RepoGroup;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using TestMarketDataMap = com.opengamma.strata.measure.curve.TestMarketDataMap;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using CapitalIndexedBondCurveDataSet = com.opengamma.strata.pricer.bond.CapitalIndexedBondCurveDataSet;
	using DiscountingCapitalIndexedBondTradePricer = com.opengamma.strata.pricer.bond.DiscountingCapitalIndexedBondTradePricer;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using LegalEntityId = com.opengamma.strata.product.LegalEntityId;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using CapitalIndexedBond = com.opengamma.strata.product.bond.CapitalIndexedBond;
	using CapitalIndexedBondTrade = com.opengamma.strata.product.bond.CapitalIndexedBondTrade;
	using ResolvedCapitalIndexedBondTrade = com.opengamma.strata.product.bond.ResolvedCapitalIndexedBondTrade;
	using InflationRateCalculation = com.opengamma.strata.product.swap.InflationRateCalculation;
	using PriceIndexCalculationMethod = com.opengamma.strata.product.swap.PriceIndexCalculationMethod;

	/// <summary>
	/// Test <seealso cref="CapitalIndexedBondTradeCalculationFunction"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CapitalIndexedBondTradeCalculationFunctionTest
	public class CapitalIndexedBondTradeCalculationFunctionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  // detachment date (for nonzero ex-coupon days) < valuation date < payment date
	  private static readonly LocalDate VAL_DATE = LocalDate.of(2014, 7, 13);

	  private static readonly DaysAdjustment SETTLE_OFFSET = DaysAdjustment.ofBusinessDays(3, USNY);
	  private static readonly Currency CURRENCY = USD;
	  private static readonly BusinessDayAdjustment BUSINESS_ADJUST = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, USNY);
	  private static readonly PeriodicSchedule SCHEDULE = PeriodicSchedule.of(date(2006, 1, 15), date(2016, 1, 15), P6M, BUSINESS_ADJUST, StubConvention.NONE, false);
	  private static readonly SecurityId SECURITY_ID = SecurityId.of("OG-Ticker", "BOND1");
	  private static readonly LegalEntityId ISSUER_ID = CapitalIndexedBondCurveDataSet.ISSUER_ID;
	  private static readonly CapitalIndexedBond PRODUCT = CapitalIndexedBond.builder().securityId(SECURITY_ID).notional(10_000_000d).currency(CURRENCY).dayCount(ACT_ACT_ICMA).rateCalculation(InflationRateCalculation.builder().gearing(ValueSchedule.of(0.01)).index(US_CPI_U).lag(Period.ofMonths(3)).indexCalculationMethod(PriceIndexCalculationMethod.INTERPOLATED).firstIndexValue(198.47742).build()).legalEntityId(ISSUER_ID).yieldConvention(US_IL_REAL).settlementDateOffset(SETTLE_OFFSET).accrualSchedule(SCHEDULE).build();

	  private const long QUANTITY = 100L;
	  private static readonly LocalDate SETTLEMENT_STANDARD = SETTLE_OFFSET.adjust(VAL_DATE, REF_DATA);
	  private static readonly TradeInfo TRADE_INFO_STANDARD = TradeInfo.builder().settlementDate(SETTLEMENT_STANDARD).build();
	  private const double TRADE_PRICE = 1.0203;
	  private static readonly CapitalIndexedBondTrade TRADE = CapitalIndexedBondTrade.builder().info(TRADE_INFO_STANDARD).product(PRODUCT).quantity(QUANTITY).price(TRADE_PRICE).build();
	  public static readonly ResolvedCapitalIndexedBondTrade RTRADE = TRADE.resolve(REF_DATA);

	  private static readonly RepoGroup REPO_GROUP = CapitalIndexedBondCurveDataSet.GROUP_REPO;
	  private static readonly LegalEntityGroup ISSUER_GROUP = CapitalIndexedBondCurveDataSet.GROUP_ISSUER;
	  private static readonly CurveId INF_CURVE_ID = CurveId.of("Default", "Inflation");
	  private static readonly CurveId REPO_CURVE_ID = CurveId.of("Default", "Repo");
	  private static readonly CurveId ISSUER_CURVE_ID = CurveId.of("Default", "Issuer");
	  public static readonly RatesMarketDataLookup RATES_LOOKUP = RatesMarketDataLookup.of(ImmutableMap.of(), ImmutableMap.of(US_CPI_U, INF_CURVE_ID));
	  public static readonly LegalEntityDiscountingMarketDataLookup LED_LOOKUP = LegalEntityDiscountingMarketDataLookup.of(ImmutableMap.of(ISSUER_ID, REPO_GROUP), ImmutableMap.of(Pair.of(REPO_GROUP, CURRENCY), REPO_CURVE_ID), ImmutableMap.of(ISSUER_ID, ISSUER_GROUP), ImmutableMap.of(Pair.of(ISSUER_GROUP, CURRENCY), ISSUER_CURVE_ID));
	  private static readonly CalculationParameters PARAMS = CalculationParameters.of(RATES_LOOKUP, LED_LOOKUP);

	  //-------------------------------------------------------------------------
	  public virtual void test_requirementsAndCurrency()
	  {
		CapitalIndexedBondTradeCalculationFunction<CapitalIndexedBondTrade> function = CapitalIndexedBondTradeCalculationFunction.TRADE;
		ISet<Measure> measures = function.supportedMeasures();
		FunctionRequirements reqs = function.requirements(TRADE, measures, PARAMS, REF_DATA);
		assertThat(reqs.OutputCurrencies).containsOnly(CURRENCY);
		assertThat(reqs.ValueRequirements).isEqualTo(ImmutableSet.of(INF_CURVE_ID, REPO_CURVE_ID, ISSUER_CURVE_ID));
		assertThat(reqs.TimeSeriesRequirements).isEqualTo(ImmutableSet.of(IndexQuoteId.of(US_CPI_U)));
		assertThat(function.naturalCurrency(TRADE, REF_DATA)).isEqualTo(CURRENCY);
	  }

	  public virtual void test_simpleMeasures()
	  {
		CapitalIndexedBondTradeCalculationFunction<CapitalIndexedBondTrade> function = CapitalIndexedBondTradeCalculationFunction.TRADE;
		ScenarioMarketData md = marketData();
		RatesProvider ratesProvider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		LegalEntityDiscountingProvider ledProvider = LED_LOOKUP.marketDataView(md.scenario(0)).discountingProvider();
		DiscountingCapitalIndexedBondTradePricer pricer = DiscountingCapitalIndexedBondTradePricer.DEFAULT;
		CurrencyAmount expectedPv = pricer.presentValue(RTRADE, ratesProvider, ledProvider);
		MultiCurrencyAmount expectedCurrencyExposure = pricer.currencyExposure(RTRADE, ratesProvider, ledProvider);
		CurrencyAmount expectedCurrentCash = pricer.currentCash(RTRADE, ratesProvider);

		ISet<Measure> measures = ImmutableSet.of(Measures.PRESENT_VALUE, Measures.CURRENCY_EXPOSURE, Measures.CURRENT_CASH, Measures.RESOLVED_TARGET);
		assertThat(function.calculate(TRADE, measures, PARAMS, md, REF_DATA)).containsEntry(Measures.PRESENT_VALUE, Result.success(CurrencyScenarioArray.of(ImmutableList.of(expectedPv)))).containsEntry(Measures.CURRENCY_EXPOSURE, Result.success(MultiCurrencyScenarioArray.of(ImmutableList.of(expectedCurrencyExposure)))).containsEntry(Measures.CURRENT_CASH, Result.success(CurrencyScenarioArray.of(ImmutableList.of(expectedCurrentCash)))).containsEntry(Measures.RESOLVED_TARGET, Result.success(RTRADE));
	  }

	  public virtual void test_pv01()
	  {
		CapitalIndexedBondTradeCalculationFunction<CapitalIndexedBondTrade> function = CapitalIndexedBondTradeCalculationFunction.TRADE;
		ScenarioMarketData md = marketData();
		RatesProvider ratesProvider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		LegalEntityDiscountingProvider ledProvider = LED_LOOKUP.marketDataView(md.scenario(0)).discountingProvider();
		DiscountingCapitalIndexedBondTradePricer pricer = DiscountingCapitalIndexedBondTradePricer.DEFAULT;
		PointSensitivities pvPointSens = pricer.presentValueSensitivity(RTRADE, ratesProvider, ledProvider);
		CurrencyParameterSensitivities pvParamSens = ledProvider.parameterSensitivity(pvPointSens);
		MultiCurrencyAmount expectedPv01Cal = pvParamSens.total().multipliedBy(1e-4);
		CurrencyParameterSensitivities expectedPv01CalBucketed = pvParamSens.multipliedBy(1e-4);

		ISet<Measure> measures = ImmutableSet.of(Measures.PV01_CALIBRATED_SUM, Measures.PV01_CALIBRATED_BUCKETED);
		assertThat(function.calculate(TRADE, measures, PARAMS, md, REF_DATA)).containsEntry(Measures.PV01_CALIBRATED_SUM, Result.success(MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv01Cal)))).containsEntry(Measures.PV01_CALIBRATED_BUCKETED, Result.success(ScenarioArray.of(ImmutableList.of(expectedPv01CalBucketed))));
	  }

	  //-------------------------------------------------------------------------
	  internal static ScenarioMarketData marketData()
	  {
		return new TestMarketDataMap(VAL_DATE, ImmutableMap.of(INF_CURVE_ID, CapitalIndexedBondCurveDataSet.CPI_CURVE, REPO_CURVE_ID, CapitalIndexedBondCurveDataSet.REPO_CURVE, ISSUER_CURVE_ID, CapitalIndexedBondCurveDataSet.ISSUER_CURVE), ImmutableMap.of(IndexQuoteId.of(US_CPI_U), CapitalIndexedBondCurveDataSet.getTimeSeries(VAL_DATE)));
	  }

	}

}