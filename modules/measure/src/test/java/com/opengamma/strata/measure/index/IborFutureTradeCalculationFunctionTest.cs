using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.index
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
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using Result = com.opengamma.strata.collect.result.Result;
	using FieldName = com.opengamma.strata.data.FieldName;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using DoubleScenarioArray = com.opengamma.strata.data.scenario.DoubleScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using TestMarketDataMap = com.opengamma.strata.measure.curve.TestMarketDataMap;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using DiscountingIborFutureTradePricer = com.opengamma.strata.pricer.index.DiscountingIborFutureTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using IborFutureTrade = com.opengamma.strata.product.index.IborFutureTrade;
	using ResolvedIborFutureTrade = com.opengamma.strata.product.index.ResolvedIborFutureTrade;
	using IborFutureConventions = com.opengamma.strata.product.index.type.IborFutureConventions;

	/// <summary>
	/// Test <seealso cref="IborFutureTradeCalculationFunction"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborFutureTradeCalculationFunctionTest
	public class IborFutureTradeCalculationFunctionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double MARKET_PRICE = 99.42;
	  public static readonly IborFutureTrade TRADE = IborFutureConventions.USD_LIBOR_3M_QUARTERLY_IMM.createTrade(LocalDate.of(2014, 9, 12), SecurityId.of("test", "test"), Period.ofMonths(1), 2, 5, 1_000_000, 0.9998, REF_DATA);
	  public static readonly ResolvedIborFutureTrade RTRADE = TRADE.resolve(REF_DATA);

	  private static readonly StandardId SEC_ID = TRADE.Product.SecurityId.StandardId;
	  private static readonly Currency CURRENCY = TRADE.Product.Currency;
	  private static readonly IborIndex INDEX = TRADE.Product.Index;
	  private static readonly CurveId DISCOUNT_CURVE_ID = CurveId.of("Default", "Discount");
	  private static readonly CurveId FORWARD_CURVE_ID = CurveId.of("Default", "Forward");
	  internal static readonly RatesMarketDataLookup RATES_LOOKUP = RatesMarketDataLookup.of(ImmutableMap.of(CURRENCY, DISCOUNT_CURVE_ID), ImmutableMap.of(INDEX, FORWARD_CURVE_ID));
	  private static readonly CalculationParameters PARAMS = CalculationParameters.of(RATES_LOOKUP);
	  private static readonly LocalDate VAL_DATE = TRADE.Product.LastTradeDate.minusDays(7);
	  private static readonly QuoteId QUOTE_KEY = QuoteId.of(SEC_ID, FieldName.SETTLEMENT_PRICE);

	  //-------------------------------------------------------------------------
	  public virtual void test_requirementsAndCurrency()
	  {
		IborFutureTradeCalculationFunction<IborFutureTrade> function = IborFutureTradeCalculationFunction.TRADE;
		ISet<Measure> measures = function.supportedMeasures();
		FunctionRequirements reqs = function.requirements(TRADE, measures, PARAMS, REF_DATA);
		assertThat(reqs.OutputCurrencies).containsOnly(CURRENCY);
		assertThat(reqs.ValueRequirements).isEqualTo(ImmutableSet.of(DISCOUNT_CURVE_ID, FORWARD_CURVE_ID, QUOTE_KEY));
		assertThat(reqs.TimeSeriesRequirements).isEqualTo(ImmutableSet.of(IndexQuoteId.of(INDEX)));
		assertThat(function.naturalCurrency(TRADE, REF_DATA)).isEqualTo(CURRENCY);
	  }

	  public virtual void test_simpleMeasures()
	  {
		IborFutureTradeCalculationFunction<IborFutureTrade> function = IborFutureTradeCalculationFunction.TRADE;
		ScenarioMarketData md = marketData();
		RatesProvider provider = RATES_LOOKUP.ratesProvider(md.scenario(0));
		double expectedPrice = DiscountingIborFutureTradePricer.DEFAULT.price(RTRADE, provider);
		CurrencyAmount expectedPv = DiscountingIborFutureTradePricer.DEFAULT.presentValue(RTRADE, provider, MARKET_PRICE / 100);
		double expectedParSpread = DiscountingIborFutureTradePricer.DEFAULT.parSpread(RTRADE, provider, MARKET_PRICE / 100);

		ISet<Measure> measures = ImmutableSet.of(Measures.UNIT_PRICE, Measures.PRESENT_VALUE, Measures.PAR_SPREAD, Measures.RESOLVED_TARGET);
		assertThat(function.calculate(TRADE, measures, PARAMS, md, REF_DATA)).containsEntry(Measures.UNIT_PRICE, Result.success(DoubleScenarioArray.of(ImmutableList.of(expectedPrice)))).containsEntry(Measures.PRESENT_VALUE, Result.success(CurrencyScenarioArray.of(ImmutableList.of(expectedPv)))).containsEntry(Measures.PAR_SPREAD, Result.success(DoubleScenarioArray.of(ImmutableList.of(expectedParSpread)))).containsEntry(Measures.RESOLVED_TARGET, Result.success(RTRADE));
	  }

	  //-------------------------------------------------------------------------
	  internal static ScenarioMarketData marketData()
	  {
		Curve curve = ConstantCurve.of(Curves.discountFactors("Test", ACT_360), 0.99);
		TestMarketDataMap md = new TestMarketDataMap(VAL_DATE, ImmutableMap.of(DISCOUNT_CURVE_ID, curve, FORWARD_CURVE_ID, curve, QUOTE_KEY, MARKET_PRICE), ImmutableMap.of());
		return md;
	  }

	}

}