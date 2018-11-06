/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.mock;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.when;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using IborIndexRates = com.opengamma.strata.pricer.rate.IborIndexRates;
	using SimpleRatesProvider = com.opengamma.strata.pricer.rate.SimpleRatesProvider;
	using ResolvedIborFuture = com.opengamma.strata.product.index.ResolvedIborFuture;
	using ResolvedIborFutureTrade = com.opengamma.strata.product.index.ResolvedIborFutureTrade;

	/// <summary>
	/// Test <seealso cref="DiscountingIborFutureTradePricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingIborFutureTradePricerTest
	public class DiscountingIborFutureTradePricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly DiscountingIborFutureTradePricer PRICER_TRADE = DiscountingIborFutureTradePricer.DEFAULT;
	  private static readonly DiscountingIborFutureProductPricer PRICER_PRODUCT = DiscountingIborFutureProductPricer.DEFAULT;
	  private static readonly ResolvedIborFutureTrade FUTURE_TRADE = IborFutureDummyData.IBOR_FUTURE_TRADE.resolve(REF_DATA);
	  private static readonly ResolvedIborFuture FUTURE = FUTURE_TRADE.Product;

	  private const double RATE = 0.045;

	  private const double TOLERANCE_PRICE = 1.0e-9;
	  private const double TOLERANCE_PRICE_DELTA = 1.0e-9;
	  private const double TOLERANCE_PV = 1.0e-4;
	  private const double TOLERANCE_PV_DELTA = 1.0e-2;

	  //------------------------------------------------------------------------- 
	  public virtual void test_price()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider();
		prov.IborRates = mockIbor;
		when(mockIbor.rate(FUTURE.IborRate.Observation)).thenReturn(RATE);

		assertEquals(PRICER_TRADE.price(FUTURE_TRADE, prov), 1.0 - RATE, TOLERANCE_PRICE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		double currentPrice = 0.995;
		double referencePrice = 0.9925;
		double currentPriceIndex = PRICER_PRODUCT.marginIndex(FUTURE_TRADE.Product, currentPrice);
		double referencePriceIndex = PRICER_PRODUCT.marginIndex(FUTURE_TRADE.Product, referencePrice);
		double presentValueExpected = (currentPriceIndex - referencePriceIndex) * FUTURE_TRADE.Quantity;
		CurrencyAmount presentValueComputed = PRICER_TRADE.presentValue(FUTURE_TRADE, currentPrice, referencePrice);
		assertEquals(presentValueComputed.Amount, presentValueExpected, TOLERANCE_PV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_reference_price_after_trade_date()
	  {
		LocalDate tradeDate = FUTURE_TRADE.TradedPrice.get().TradeDate;
		LocalDate valuationDate = tradeDate.plusDays(1);
		double settlementPrice = 0.995;
		double referencePrice = PRICER_TRADE.referencePrice(FUTURE_TRADE, valuationDate, settlementPrice);
		assertEquals(referencePrice, settlementPrice);
	  }

	  public virtual void test_reference_price_on_trade_date()
	  {
		LocalDate tradeDate = FUTURE_TRADE.TradedPrice.get().TradeDate;
		LocalDate valuationDate = tradeDate;
		double settlementPrice = 0.995;
		double referencePrice = PRICER_TRADE.referencePrice(FUTURE_TRADE, valuationDate, settlementPrice);
		assertEquals(referencePrice, FUTURE_TRADE.TradedPrice.get().Price);
	  }

	  public virtual void test_reference_price_val_date_not_null()
	  {
		double settlementPrice = 0.995;
		assertThrowsIllegalArg(() => PRICER_TRADE.referencePrice(FUTURE_TRADE, null, settlementPrice));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parSpread_after_trade_date()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider();
		prov.IborRates = mockIbor;
		prov.ValuationDate = FUTURE_TRADE.TradedPrice.get().TradeDate.plusDays(1);
		when(mockIbor.rate(FUTURE.IborRate.Observation)).thenReturn(RATE);
		double lastClosingPrice = 0.99;
		double parSpreadExpected = PRICER_TRADE.price(FUTURE_TRADE, prov) - lastClosingPrice;
		double parSpreadComputed = PRICER_TRADE.parSpread(FUTURE_TRADE, prov, lastClosingPrice);
		assertEquals(parSpreadComputed, parSpreadExpected, TOLERANCE_PRICE);
	  }

	  public virtual void test_parSpread_on_trade_date()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider();
		prov.IborRates = mockIbor;
		prov.ValuationDate = FUTURE_TRADE.TradedPrice.get().TradeDate;
		when(mockIbor.rate(FUTURE.IborRate.Observation)).thenReturn(RATE);

		double lastClosingPrice = 0.99;
		double parSpreadExpected = PRICER_TRADE.price(FUTURE_TRADE, prov) - FUTURE_TRADE.TradedPrice.get().Price;
		double parSpreadComputed = PRICER_TRADE.parSpread(FUTURE_TRADE, prov, lastClosingPrice);
		assertEquals(parSpreadComputed, parSpreadExpected, TOLERANCE_PRICE);
	  }

	  //------------------------------------------------------------------------- 
	  public virtual void test_presentValue_after_trade_date()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider();
		prov.IborRates = mockIbor;
		prov.ValuationDate = FUTURE_TRADE.TradedPrice.get().TradeDate.plusDays(1);
		when(mockIbor.rate(FUTURE.IborRate.Observation)).thenReturn(RATE);

		double lastClosingPrice = 1.025;
		DiscountingIborFutureTradePricer pricerFn = DiscountingIborFutureTradePricer.DEFAULT;
		double expected = ((1.0 - RATE) - lastClosingPrice) * FUTURE.AccrualFactor * FUTURE.Notional * FUTURE_TRADE.Quantity;
		CurrencyAmount computed = pricerFn.presentValue(FUTURE_TRADE, prov, lastClosingPrice);
		assertEquals(computed.Amount, expected, TOLERANCE_PV);
		assertEquals(computed.Currency, FUTURE.Currency);
	  }

	  public virtual void test_presentValue_on_trade_date()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider();
		prov.IborRates = mockIbor;
		prov.ValuationDate = FUTURE_TRADE.TradedPrice.get().TradeDate;
		when(mockIbor.rate(FUTURE.IborRate.Observation)).thenReturn(RATE);

		double lastClosingPrice = 1.025;
		DiscountingIborFutureTradePricer pricerFn = DiscountingIborFutureTradePricer.DEFAULT;
		double expected = ((1.0 - RATE) - FUTURE_TRADE.TradedPrice.get().Price) * FUTURE.AccrualFactor * FUTURE.Notional * FUTURE_TRADE.Quantity;
		CurrencyAmount computed = pricerFn.presentValue(FUTURE_TRADE, prov, lastClosingPrice);
		assertEquals(computed.Amount, expected, TOLERANCE_PV);
		assertEquals(computed.Currency, FUTURE.Currency);
	  }

	  //-------------------------------------------------------------------------   
	  public virtual void test_presentValueSensitivity()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider();
		prov.IborRates = mockIbor;

		PointSensitivities sensiPrice = PRICER_PRODUCT.priceSensitivity(FUTURE, prov);
		PointSensitivities sensiPresentValueExpected = sensiPrice.multipliedBy(FUTURE.Notional * FUTURE.AccrualFactor * FUTURE_TRADE.Quantity);
		PointSensitivities sensiPresentValueComputed = PRICER_TRADE.presentValueSensitivity(FUTURE_TRADE, prov);
		assertTrue(sensiPresentValueComputed.equalWithTolerance(sensiPresentValueExpected, TOLERANCE_PV_DELTA));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parSpreadSensitivity()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider();
		prov.IborRates = mockIbor;

		PointSensitivities sensiExpected = PRICER_PRODUCT.priceSensitivity(FUTURE, prov);
		PointSensitivities sensiComputed = PRICER_TRADE.parSpreadSensitivity(FUTURE_TRADE, prov);
		assertTrue(sensiComputed.equalWithTolerance(sensiExpected, TOLERANCE_PRICE_DELTA));
	  }

	}

}