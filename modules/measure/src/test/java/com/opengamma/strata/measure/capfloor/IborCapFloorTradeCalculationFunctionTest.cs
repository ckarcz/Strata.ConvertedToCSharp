using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.capfloor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.dateUtc;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PutCall.CALL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using Result = com.opengamma.strata.collect.result.Result;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;
	using TestMarketDataMap = com.opengamma.strata.measure.curve.TestMarketDataMap;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using IborCapFloorDataSet = com.opengamma.strata.pricer.capfloor.IborCapFloorDataSet;
	using IborCapletFloorletDataSet = com.opengamma.strata.pricer.capfloor.IborCapletFloorletDataSet;
	using IborCapletFloorletVolatilitiesId = com.opengamma.strata.pricer.capfloor.IborCapletFloorletVolatilitiesId;
	using NormalIborCapFloorTradePricer = com.opengamma.strata.pricer.capfloor.NormalIborCapFloorTradePricer;
	using NormalIborCapletFloorletExpiryStrikeVolatilities = com.opengamma.strata.pricer.capfloor.NormalIborCapletFloorletExpiryStrikeVolatilities;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using IborCapFloor = com.opengamma.strata.product.capfloor.IborCapFloor;
	using IborCapFloorLeg = com.opengamma.strata.product.capfloor.IborCapFloorLeg;
	using IborCapFloorTrade = com.opengamma.strata.product.capfloor.IborCapFloorTrade;
	using ResolvedIborCapFloorTrade = com.opengamma.strata.product.capfloor.ResolvedIborCapFloorTrade;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;

	/// <summary>
	/// Test <seealso cref="IborCapFloorTradeCalculationFunction"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborCapFloorTradeCalculationFunctionTest
	public class IborCapFloorTradeCalculationFunctionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double NOTIONAL_VALUE = 1.0e6;
	  private static readonly ValueSchedule NOTIONAL = ValueSchedule.of(NOTIONAL_VALUE);
	  private static readonly LocalDate START = LocalDate.of(2015, 10, 21);
	  private static readonly LocalDate END = LocalDate.of(2020, 10, 21);
	  private const double STRIKE_VALUE = 0.0105;
	  private static readonly ValueSchedule STRIKE = ValueSchedule.of(STRIKE_VALUE);
	  private static readonly IborCapFloorLeg CAP_LEG = IborCapFloorDataSet.createCapFloorLegUnresolved(EUR_EURIBOR_6M, START, END, STRIKE, NOTIONAL, CALL, RECEIVE);
	  private static readonly SwapLeg PAY_LEG = IborCapFloorDataSet.createFixedPayLegUnresolved(EUR_EURIBOR_6M, START, END, 0.0395, NOTIONAL_VALUE, PAY);
	  private static readonly IborCapFloor CAP_TWO_LEGS = IborCapFloor.of(CAP_LEG, PAY_LEG);
	  private static readonly ZonedDateTime VALUATION = dateUtc(2015, 8, 20);
	  internal static readonly NormalIborCapletFloorletExpiryStrikeVolatilities VOLS = IborCapletFloorletDataSet.createNormalVolatilities(VALUATION, EUR_EURIBOR_6M);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.builder().tradeDate(VALUATION.toLocalDate()).build();
	  private static readonly IborCapFloorTrade TRADE = IborCapFloorTrade.builder().product(CAP_TWO_LEGS).info(TRADE_INFO).build();
	  internal static readonly ResolvedIborCapFloorTrade RTRADE = TRADE.resolve(REF_DATA);

	  private static readonly Currency CURRENCY = RTRADE.Product.CapFloorLeg.Currency;
	  private static readonly IborIndex INDEX = RTRADE.Product.CapFloorLeg.Index;
	  private static readonly LocalDate VAL_DATE = VOLS.ValuationDate;
	  private static readonly CurveId DISCOUNT_CURVE_ID = CurveId.of("Default", "Discount");
	  private static readonly CurveId FORWARD_CURVE_ID = CurveId.of("Default", "Forward");
	  private static readonly IborCapletFloorletVolatilitiesId VOL_ID = IborCapletFloorletVolatilitiesId.of("IborCapFloorVols.Normal.USD");
	  internal static readonly RatesMarketDataLookup RATES_LOOKUP = RatesMarketDataLookup.of(ImmutableMap.of(CURRENCY, DISCOUNT_CURVE_ID), ImmutableMap.of(INDEX, FORWARD_CURVE_ID));
	  internal static readonly IborCapFloorMarketDataLookup SWAPTION_LOOKUP = IborCapFloorMarketDataLookup.of(INDEX, VOL_ID);
	  private static readonly CalculationParameters PARAMS = CalculationParameters.of(RATES_LOOKUP, SWAPTION_LOOKUP);

	  //-------------------------------------------------------------------------
	  public virtual void test_requirementsAndCurrency()
	  {
		IborCapFloorTradeCalculationFunction function = new IborCapFloorTradeCalculationFunction();
		ISet<Measure> measures = function.supportedMeasures();
		FunctionRequirements reqs = function.requirements(TRADE, measures, PARAMS, REF_DATA);
		assertThat(reqs.OutputCurrencies).containsOnly(CURRENCY);
		assertThat(reqs.ValueRequirements).isEqualTo(ImmutableSet.of(DISCOUNT_CURVE_ID, FORWARD_CURVE_ID, VOL_ID));
		assertThat(reqs.TimeSeriesRequirements).isEqualTo(ImmutableSet.of(IndexQuoteId.of(INDEX)));
		assertThat(function.naturalCurrency(TRADE, REF_DATA)).isEqualTo(CURRENCY);
	  }

	  public virtual void test_simpleMeasures()
	  {
		IborCapFloorTradeCalculationFunction function = new IborCapFloorTradeCalculationFunction();
		ScenarioMarketData md = marketData();
		RatesProvider provider = RATES_LOOKUP.ratesProvider(md.scenario(0));
		NormalIborCapFloorTradePricer pricer = NormalIborCapFloorTradePricer.DEFAULT;
		MultiCurrencyAmount expectedPv = pricer.presentValue(RTRADE, provider, VOLS);
		MultiCurrencyAmount expectedCurrencyExposure = pricer.currencyExposure(RTRADE, provider, VOLS);
		MultiCurrencyAmount expectedCurrentCash = pricer.currentCash(RTRADE, provider, VOLS);

		ISet<Measure> measures = ImmutableSet.of(Measures.PRESENT_VALUE, Measures.CURRENCY_EXPOSURE, Measures.CURRENT_CASH);
		assertThat(function.calculate(TRADE, measures, PARAMS, md, REF_DATA)).containsEntry(Measures.PRESENT_VALUE, Result.success(MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv)))).containsEntry(Measures.CURRENCY_EXPOSURE, Result.success(MultiCurrencyScenarioArray.of(ImmutableList.of(expectedCurrencyExposure)))).containsEntry(Measures.CURRENT_CASH, Result.success(MultiCurrencyScenarioArray.of(ImmutableList.of(expectedCurrentCash))));
	  }

	  //-------------------------------------------------------------------------
	  internal static ScenarioMarketData marketData()
	  {
		Curve curve = ConstantCurve.of(Curves.discountFactors("Test", ACT_360), 0.99);
		TestMarketDataMap md = new TestMarketDataMap(VAL_DATE, ImmutableMap.of(DISCOUNT_CURVE_ID, curve, FORWARD_CURVE_ID, curve, VOL_ID, VOLS), ImmutableMap.of());
		return md;
	  }

	}

}