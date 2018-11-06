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
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using Rounding = com.opengamma.strata.basics.value.Rounding;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using DiscountingOvernightFutureTradePricer = com.opengamma.strata.pricer.index.DiscountingOvernightFutureTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using OvernightFuture = com.opengamma.strata.product.index.OvernightFuture;
	using OvernightFutureTrade = com.opengamma.strata.product.index.OvernightFutureTrade;
	using ResolvedOvernightFutureTrade = com.opengamma.strata.product.index.ResolvedOvernightFutureTrade;
	using OvernightAccrualMethod = com.opengamma.strata.product.swap.OvernightAccrualMethod;

	/// <summary>
	/// Test <seealso cref="OvernightFutureTradeCalculations"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class OvernightFutureTradeCalculationsTest
	public class OvernightFutureTradeCalculationsTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
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
	  private const double ONE_BP = 1.0e-4;
	  private const double ONE_PC = 1.0e-2;
	  private static readonly OvernightIndex INDEX = TRADE.Product.Index;
	  private static readonly CurveId FORWARD_CURVE_ID = CurveId.of("Default", "Forward");
	  private static readonly RatesMarketDataLookup RATES_LOOKUP = RatesMarketDataLookup.of(ImmutableMap.of(), ImmutableMap.of(INDEX, FORWARD_CURVE_ID));
	  private static readonly double SETTLEMENT_PRICE = OvernightFutureTradeCalculationFunctionTest.MARKET_PRICE * ONE_PC;
	  private static readonly ScenarioMarketData MARKET_DATA = OvernightFutureTradeCalculationFunctionTest.marketData(FORWARD_CURVE_ID.CurveName);
	  private static readonly RatesProvider RATES_PROVIDER = RATES_LOOKUP.marketDataView(MARKET_DATA.scenario(0)).ratesProvider();
	  private static readonly DiscountingOvernightFutureTradePricer TRADE_PRICER = DiscountingOvernightFutureTradePricer.DEFAULT;
	  private static readonly MarketQuoteSensitivityCalculator MQ_CALC = MarketQuoteSensitivityCalculator.DEFAULT;
	  private static readonly OvernightFutureTradeCalculations CALC = OvernightFutureTradeCalculations.DEFAULT;
	  private const double TOL = 1.0e-14;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		CurrencyAmount expected = TRADE_PRICER.presentValue(RESOLVED_TRADE, RATES_PROVIDER, SETTLEMENT_PRICE);
		assertEquals(CALC.presentValue(RESOLVED_TRADE, RATES_LOOKUP, MARKET_DATA), CurrencyScenarioArray.of(ImmutableList.of(expected)));
		assertEquals(CALC.presentValue(RESOLVED_TRADE, RATES_PROVIDER), expected);
	  }

	  public virtual void test_parSpread()
	  {
		double expected = TRADE_PRICER.parSpread(RESOLVED_TRADE, RATES_PROVIDER, SETTLEMENT_PRICE);
		assertEquals(CALC.parSpread(RESOLVED_TRADE, RATES_LOOKUP, MARKET_DATA).get(0).doubleValue(), expected, TOL);
		assertEquals(CALC.parSpread(RESOLVED_TRADE, RATES_PROVIDER), expected, TOL);
	  }

	  public virtual void test_unitPrice()
	  {
		double expected = TRADE_PRICER.price(RESOLVED_TRADE, RATES_PROVIDER);
		assertEquals(CALC.unitPrice(RESOLVED_TRADE, RATES_LOOKUP, MARKET_DATA).get(0).doubleValue(), expected, TOL);
		assertEquals(CALC.unitPrice(RESOLVED_TRADE, RATES_PROVIDER), expected, TOL);
	  }

	  public virtual void test_pv01_calibrated()
	  {
		PointSensitivities pvPointSens = TRADE_PRICER.presentValueSensitivity(RESOLVED_TRADE, RATES_PROVIDER);
		CurrencyParameterSensitivities pvParamSens = RATES_PROVIDER.parameterSensitivity(pvPointSens);
		MultiCurrencyAmount expectedPv01Cal = pvParamSens.total().multipliedBy(ONE_BP);
		CurrencyParameterSensitivities expectedPv01CalBucketed = pvParamSens.multipliedBy(ONE_BP);
		assertEquals(CALC.pv01CalibratedSum(RESOLVED_TRADE, RATES_LOOKUP, MARKET_DATA), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv01Cal)));
		assertEquals(CALC.pv01CalibratedSum(RESOLVED_TRADE, RATES_PROVIDER), expectedPv01Cal);
		assertEquals(CALC.pv01CalibratedBucketed(RESOLVED_TRADE, RATES_LOOKUP, MARKET_DATA), ScenarioArray.of(ImmutableList.of(expectedPv01CalBucketed)));
		assertEquals(CALC.pv01CalibratedBucketed(RESOLVED_TRADE, RATES_PROVIDER), expectedPv01CalBucketed);
	  }

	  public virtual void test_pv01_quote()
	  {
		PointSensitivities pvPointSens = TRADE_PRICER.presentValueSensitivity(RESOLVED_TRADE, RATES_PROVIDER);
		CurrencyParameterSensitivities pvParamSens = RATES_PROVIDER.parameterSensitivity(pvPointSens);
		CurrencyParameterSensitivities expectedPv01Bucketed = MQ_CALC.sensitivity(pvParamSens, RATES_PROVIDER).multipliedBy(ONE_BP);
		MultiCurrencyAmount expectedPv01Sum = expectedPv01Bucketed.total();
		assertEquals(CALC.pv01MarketQuoteSum(RESOLVED_TRADE, RATES_LOOKUP, MARKET_DATA), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv01Sum)));
		assertEquals(CALC.pv01MarketQuoteSum(RESOLVED_TRADE, RATES_PROVIDER), expectedPv01Sum);
		assertEquals(CALC.pv01MarketQuoteBucketed(RESOLVED_TRADE, RATES_LOOKUP, MARKET_DATA), ScenarioArray.of(ImmutableList.of(expectedPv01Bucketed)));
		assertEquals(CALC.pv01MarketQuoteBucketed(RESOLVED_TRADE, RATES_PROVIDER), expectedPv01Bucketed);
	  }

	}

}