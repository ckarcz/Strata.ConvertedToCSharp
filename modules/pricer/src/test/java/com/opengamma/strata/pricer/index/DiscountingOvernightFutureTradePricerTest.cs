/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.index
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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using Rounding = com.opengamma.strata.basics.value.Rounding;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using OvernightFuture = com.opengamma.strata.product.index.OvernightFuture;
	using OvernightFutureTrade = com.opengamma.strata.product.index.OvernightFutureTrade;
	using ResolvedOvernightFutureTrade = com.opengamma.strata.product.index.ResolvedOvernightFutureTrade;
	using OvernightAccrualMethod = com.opengamma.strata.product.swap.OvernightAccrualMethod;

	/// <summary>
	/// Test <seealso cref="DiscountingOvernightFutureTradePricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingOvernightFutureTradePricerTest
	public class DiscountingOvernightFutureTradePricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VALUATION = LocalDate.of(2018, 7, 12);
	  private const double NOTIONAL = 5_000_000d;
	  private static readonly double ACCRUAL_FACTOR = TENOR_1M.Period.toTotalMonths() / 12.0;
	  private static readonly LocalDate LAST_TRADE_DATE = date(2018, 9, 28);
	  private static readonly LocalDate START_DATE = date(2018, 9, 1);
	  private static readonly LocalDate END_DATE = date(2018, 9, 30);
	  private static readonly Rounding ROUNDING = Rounding.ofDecimalPlaces(5);
	  private static readonly SecurityId SECURITY_ID = SecurityId.of("OG-Test", "OnFuture");
	  private static readonly OvernightFuture FUTURE = OvernightFuture.builder().securityId(SECURITY_ID).currency(USD).notional(NOTIONAL).accrualFactor(ACCRUAL_FACTOR).startDate(START_DATE).endDate(END_DATE).lastTradeDate(LAST_TRADE_DATE).index(USD_FED_FUND).accrualMethod(OvernightAccrualMethod.AVERAGED_DAILY).rounding(ROUNDING).build();
	  private static readonly LocalDate TRADE_DATE = date(2018, 2, 17);
	  private const long FUTURE_QUANTITY = 35;
	  private const double FUTURE_INITIAL_PRICE = 1.015;
	  private static readonly ResolvedOvernightFutureTrade RESOLVED_TRADE = OvernightFutureTrade.builder().info(TradeInfo.builder().tradeDate(TRADE_DATE).build()).product(FUTURE).quantity(FUTURE_QUANTITY).price(FUTURE_INITIAL_PRICE).build().resolve(REF_DATA);

	  private static readonly DoubleArray TIME = DoubleArray.of(0.02, 0.08, 0.25, 0.5);
	  private static readonly DoubleArray RATE = DoubleArray.of(0.01, 0.015, 0.008, 0.005);
	  private static readonly Curve CURVE = InterpolatedNodalCurve.of(Curves.zeroRates("FED-FUND", DayCounts.ACT_365F), TIME, RATE, CurveInterpolators.NATURAL_SPLINE);
	  private static readonly RatesProvider RATES_PROVIDER = getRatesProvider(VALUATION);
	  private static readonly RatesProvider RATES_PROVIDER_ON = getRatesProvider(TRADE_DATE);
	  private static readonly RatesProvider RATES_PROVIDER_AFTER = getRatesProvider(TRADE_DATE.plusWeeks(1));

	  private static RatesProvider getRatesProvider(LocalDate valuationDate)
	  {
		return ImmutableRatesProvider.builder(valuationDate).indexCurve(USD_FED_FUND, CURVE).build();
	  }

	  private const double TOL = 1.0e-14;
	  private static readonly DiscountingOvernightFutureProductPricer PRICER_PRODUCT = DiscountingOvernightFutureProductPricer.DEFAULT;
	  private static readonly DiscountingOvernightFutureTradePricer PRICER_TRADE = DiscountingOvernightFutureTradePricer.DEFAULT;
	  private const double TOLERANCE_PRICE = 1.0e-9;
	  private const double TOLERANCE_PV = 1.0e-4;

	  //------------------------------------------------------------------------- 
	  public virtual void test_price()
	  {
		double computed = PRICER_TRADE.price(RESOLVED_TRADE, RATES_PROVIDER);
		double expected = PRICER_PRODUCT.price(RESOLVED_TRADE.Product, RATES_PROVIDER);
		assertEquals(computed, expected, TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		double currentPrice = 0.995;
		double referencePrice = 0.9925;
		double currentPriceIndex = PRICER_PRODUCT.marginIndex(RESOLVED_TRADE.Product, currentPrice);
		double referencePriceIndex = PRICER_PRODUCT.marginIndex(RESOLVED_TRADE.Product, referencePrice);
		double presentValueExpected = (currentPriceIndex - referencePriceIndex) * RESOLVED_TRADE.Quantity;
		CurrencyAmount presentValueComputed = PRICER_TRADE.presentValue(RESOLVED_TRADE, currentPrice, referencePrice);
		assertEquals(presentValueComputed.Amount, presentValueExpected, TOLERANCE_PV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_reference_price_after_trade_date()
	  {
		LocalDate tradeDate = RESOLVED_TRADE.TradedPrice.get().TradeDate;
		LocalDate valuationDate = tradeDate.plusDays(1);
		double settlementPrice = 0.995;
		double referencePrice = PRICER_TRADE.referencePrice(RESOLVED_TRADE, valuationDate, settlementPrice);
		assertEquals(referencePrice, settlementPrice);
	  }

	  public virtual void test_reference_price_on_trade_date()
	  {
		LocalDate tradeDate = RESOLVED_TRADE.TradedPrice.get().TradeDate;
		LocalDate valuationDate = tradeDate;
		double settlementPrice = 0.995;
		double referencePrice = PRICER_TRADE.referencePrice(RESOLVED_TRADE, valuationDate, settlementPrice);
		assertEquals(referencePrice, RESOLVED_TRADE.TradedPrice.get().Price);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parSpread_after_trade_date()
	  {
		double lastClosingPrice = 0.99;
		double parSpreadExpected = PRICER_TRADE.price(RESOLVED_TRADE, RATES_PROVIDER_AFTER) - lastClosingPrice;
		double parSpreadComputed = PRICER_TRADE.parSpread(RESOLVED_TRADE, RATES_PROVIDER_AFTER, lastClosingPrice);
		assertEquals(parSpreadComputed, parSpreadExpected, TOLERANCE_PRICE);
	  }

	  public virtual void test_parSpread_on_trade_date()
	  {
		double lastClosingPrice = 0.99;
		double parSpreadExpected = PRICER_TRADE.price(RESOLVED_TRADE, RATES_PROVIDER_ON) - RESOLVED_TRADE.TradedPrice.get().Price;
		double parSpreadComputed = PRICER_TRADE.parSpread(RESOLVED_TRADE, RATES_PROVIDER_ON, lastClosingPrice);
		assertEquals(parSpreadComputed, parSpreadExpected, TOLERANCE_PRICE);
	  }

	  //------------------------------------------------------------------------- 
	  public virtual void test_presentValue_after_trade_date()
	  {
		double lastClosingPrice = 1.005;
		double expected = (PRICER_PRODUCT.price(RESOLVED_TRADE.Product, RATES_PROVIDER_AFTER) - lastClosingPrice) * FUTURE.AccrualFactor * FUTURE.Notional * RESOLVED_TRADE.Quantity;
		CurrencyAmount computed = PRICER_TRADE.presentValue(RESOLVED_TRADE, RATES_PROVIDER_AFTER, lastClosingPrice);
		assertEquals(computed.Amount, expected, NOTIONAL * TOL);
		assertEquals(computed.Currency, FUTURE.Currency);
	  }

	  public virtual void test_presentValue_on_trade_date()
	  {
		double lastClosingPrice = 1.005;
		double expected = (PRICER_PRODUCT.price(RESOLVED_TRADE.Product, RATES_PROVIDER_ON) - RESOLVED_TRADE.TradedPrice.get().Price) * FUTURE.AccrualFactor * FUTURE.Notional * RESOLVED_TRADE.Quantity;
		CurrencyAmount computed = PRICER_TRADE.presentValue(RESOLVED_TRADE, RATES_PROVIDER_ON, lastClosingPrice);
		assertEquals(computed.Amount, expected, TOLERANCE_PV);
		assertEquals(computed.Currency, FUTURE.Currency);
	  }

	  //-------------------------------------------------------------------------   
	  public virtual void test_presentValueSensitivity()
	  {
		PointSensitivities computed = PRICER_TRADE.presentValueSensitivity(RESOLVED_TRADE, RATES_PROVIDER);
		PointSensitivities expected = PRICER_PRODUCT.priceSensitivity(RESOLVED_TRADE.Product, RATES_PROVIDER).multipliedBy(FUTURE.Notional * FUTURE.AccrualFactor * RESOLVED_TRADE.Quantity);
		assertTrue(computed.equalWithTolerance(expected, NOTIONAL * TOL));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parSpreadSensitivity()
	  {
		PointSensitivities sensiExpected = PRICER_PRODUCT.priceSensitivity(RESOLVED_TRADE.Product, RATES_PROVIDER);
		PointSensitivities sensiComputed = PRICER_TRADE.parSpreadSensitivity(RESOLVED_TRADE, RATES_PROVIDER);
		assertTrue(sensiComputed.equalWithTolerance(sensiExpected, TOL));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_pricedSensitivity()
	  {
		PointSensitivities sensiExpected = PRICER_PRODUCT.priceSensitivity(RESOLVED_TRADE.Product, RATES_PROVIDER);
		PointSensitivities sensiComputed = PRICER_TRADE.priceSensitivity(RESOLVED_TRADE, RATES_PROVIDER);
		assertTrue(sensiComputed.equalWithTolerance(sensiExpected, TOL));
	  }

	}

}