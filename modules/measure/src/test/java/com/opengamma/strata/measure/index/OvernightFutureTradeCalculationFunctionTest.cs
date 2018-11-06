using System.Collections.Generic;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.NATURAL_SPLINE;
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
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using Rounding = com.opengamma.strata.basics.value.Rounding;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Result = com.opengamma.strata.collect.result.Result;
	using FieldName = com.opengamma.strata.data.FieldName;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using DoubleScenarioArray = com.opengamma.strata.data.scenario.DoubleScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurveParameterSize = com.opengamma.strata.market.curve.CurveParameterSize;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using JacobianCalibrationMatrix = com.opengamma.strata.market.curve.JacobianCalibrationMatrix;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using TestMarketDataMap = com.opengamma.strata.measure.curve.TestMarketDataMap;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using DiscountingOvernightFutureTradePricer = com.opengamma.strata.pricer.index.DiscountingOvernightFutureTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using OvernightFuture = com.opengamma.strata.product.index.OvernightFuture;
	using OvernightFutureTrade = com.opengamma.strata.product.index.OvernightFutureTrade;
	using ResolvedOvernightFutureTrade = com.opengamma.strata.product.index.ResolvedOvernightFutureTrade;
	using OvernightAccrualMethod = com.opengamma.strata.product.swap.OvernightAccrualMethod;

	/// <summary>
	/// Test <seealso cref="OvernightFutureTradeCalculationFunction"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class OvernightFutureTradeCalculationFunctionTest
	public class OvernightFutureTradeCalculationFunctionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = date(2018, 8, 18);
	  public const double MARKET_PRICE = 99.42;
	  private static readonly LocalDate TRADE_DATE = date(2018, 3, 18);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(TRADE_DATE);
	  private const double NOTIONAL = 5_000_000d;
	  private static readonly double ACCRUAL_FACTOR = TENOR_1M.Period.toTotalMonths() / 12.0;
	  private static readonly LocalDate LAST_TRADE_DATE = date(2018, 9, 28);
	  private static readonly LocalDate START_DATE = date(2018, 9, 1);
	  private static readonly LocalDate END_DATE = date(2018, 9, 30);
	  private static readonly Rounding ROUNDING = Rounding.ofDecimalPlaces(5);
	  private static readonly SecurityId SECURITY_ID = SecurityId.of("OG-Test", "OnFuture");
	  private static readonly OvernightFuture PRODUCT = OvernightFuture.builder().securityId(SECURITY_ID).currency(USD).notional(NOTIONAL).accrualFactor(ACCRUAL_FACTOR).startDate(START_DATE).endDate(END_DATE).lastTradeDate(LAST_TRADE_DATE).index(USD_FED_FUND).accrualMethod(OvernightAccrualMethod.AVERAGED_DAILY).rounding(ROUNDING).build();
	  private const double QUANTITY = 35;
	  private const double PRICE = 0.998;
	  private static readonly OvernightFutureTrade TRADE = OvernightFutureTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(QUANTITY).price(PRICE).build();
	  private static readonly ResolvedOvernightFutureTrade RESOLVED_TRADE = TRADE.resolve(REF_DATA);

	  private static readonly StandardId SEC_ID = TRADE.Product.SecurityId.StandardId;
	  private static readonly Currency CURRENCY = TRADE.Product.Currency;
	  private static readonly OvernightIndex INDEX = TRADE.Product.Index;
	  private static readonly CurveId FORWARD_CURVE_ID = CurveId.of("Default", "Forward");
	  private static readonly RatesMarketDataLookup RATES_LOOKUP = RatesMarketDataLookup.of(ImmutableMap.of(), ImmutableMap.of(INDEX, FORWARD_CURVE_ID));
	  private static readonly CalculationParameters PARAMS = CalculationParameters.of(RATES_LOOKUP);
	  private static readonly QuoteId QUOTE_KEY = QuoteId.of(SEC_ID, FieldName.SETTLEMENT_PRICE);
	  private static readonly DiscountingOvernightFutureTradePricer TRADE_PRICER = DiscountingOvernightFutureTradePricer.DEFAULT;

	  //-------------------------------------------------------------------------
	  public virtual void test_requirementsAndCurrency()
	  {
		OvernightFutureTradeCalculationFunction<OvernightFutureTrade> function = OvernightFutureTradeCalculationFunction.TRADE;
		ISet<Measure> measures = function.supportedMeasures();
		FunctionRequirements reqs = function.requirements(TRADE, measures, PARAMS, REF_DATA);
		assertThat(reqs.OutputCurrencies).Empty;
		assertThat(reqs.ValueRequirements).isEqualTo(ImmutableSet.of(FORWARD_CURVE_ID, QUOTE_KEY));
		assertThat(reqs.TimeSeriesRequirements).isEqualTo(ImmutableSet.of(IndexQuoteId.of(INDEX)));
		assertThat(function.naturalCurrency(TRADE, REF_DATA)).isEqualTo(CURRENCY);
	  }

	  public virtual void test_simpleMeasures()
	  {
		OvernightFutureTradeCalculationFunction<OvernightFutureTrade> function = OvernightFutureTradeCalculationFunction.TRADE;
		ScenarioMarketData md = marketData(FORWARD_CURVE_ID.CurveName);
		RatesProvider provider = RATES_LOOKUP.ratesProvider(md.scenario(0));
		double expectedPrice = TRADE_PRICER.price(RESOLVED_TRADE, provider);
		CurrencyAmount expectedPv = TRADE_PRICER.presentValue(RESOLVED_TRADE, provider, MARKET_PRICE / 100d);
		double expectedParSpread = TRADE_PRICER.parSpread(RESOLVED_TRADE, provider, MARKET_PRICE / 100d);

		ISet<Measure> measures = ImmutableSet.of(Measures.UNIT_PRICE, Measures.PRESENT_VALUE, Measures.PAR_SPREAD, Measures.RESOLVED_TARGET);
		assertThat(function.calculate(TRADE, measures, PARAMS, md, REF_DATA)).containsEntry(Measures.UNIT_PRICE, Result.success(DoubleScenarioArray.of(ImmutableList.of(expectedPrice)))).containsEntry(Measures.PRESENT_VALUE, Result.success(CurrencyScenarioArray.of(ImmutableList.of(expectedPv)))).containsEntry(Measures.PAR_SPREAD, Result.success(DoubleScenarioArray.of(ImmutableList.of(expectedParSpread)))).containsEntry(Measures.RESOLVED_TARGET, Result.success(RESOLVED_TRADE));
	  }

	  //-------------------------------------------------------------------------
	  internal static ScenarioMarketData marketData(CurveName curveName)
	  {
		DoubleMatrix jacobian = DoubleMatrix.ofUnsafe(new double[][]
		{
			new double[] {0.985d, 0.01d, 0d},
			new double[] {0.01d, 0.98d, 0.01d},
			new double[] {0.005d, 0.01d, 0.99d}
		});
		JacobianCalibrationMatrix jcm = JacobianCalibrationMatrix.of(ImmutableList.of(CurveParameterSize.of(curveName, 3)), jacobian);
		DoubleArray time = DoubleArray.of(0.1, 0.25, 0.5d);
		DoubleArray rate = DoubleArray.of(0.01, 0.015, 0.008d);
		Curve curve = InterpolatedNodalCurve.of(Curves.zeroRates(curveName, ACT_360).withInfo(CurveInfoType.JACOBIAN, jcm), time, rate, NATURAL_SPLINE);
		TestMarketDataMap md = new TestMarketDataMap(VAL_DATE, ImmutableMap.of(FORWARD_CURVE_ID, curve, QUOTE_KEY, MARKET_PRICE), ImmutableMap.of());
		return md;
	  }

	}

}