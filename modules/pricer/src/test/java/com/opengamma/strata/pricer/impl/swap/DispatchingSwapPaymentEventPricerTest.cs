/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.mock;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.when;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SwapDummyData = com.opengamma.strata.pricer.swap.SwapDummyData;
	using SwapPaymentEventPricer = com.opengamma.strata.pricer.swap.SwapPaymentEventPricer;
	using FxResetNotionalExchange = com.opengamma.strata.product.swap.FxResetNotionalExchange;
	using NotionalExchange = com.opengamma.strata.product.swap.NotionalExchange;
	using SwapPaymentEvent = com.opengamma.strata.product.swap.SwapPaymentEvent;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DispatchingSwapPaymentEventPricerTest
	public class DispatchingSwapPaymentEventPricerTest
	{

	  private static readonly RatesProvider MOCK_PROV = new MockRatesProvider();
	  private static readonly SwapPaymentEventPricer<NotionalExchange> MOCK_NOTIONAL_EXG = mock(typeof(SwapPaymentEventPricer));
	  private static readonly SwapPaymentEventPricer<FxResetNotionalExchange> MOCK_FX_NOTIONAL_EXG = mock(typeof(SwapPaymentEventPricer));

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue_NotionalExchange()
	  {
		double expected = 0.0123d;
		SwapPaymentEventPricer<NotionalExchange> mockCalledFn = mock(typeof(SwapPaymentEventPricer));
		when(mockCalledFn.presentValue(SwapDummyData.NOTIONAL_EXCHANGE_REC_GBP, MOCK_PROV)).thenReturn(expected);
		DispatchingSwapPaymentEventPricer test = new DispatchingSwapPaymentEventPricer(mockCalledFn, MOCK_FX_NOTIONAL_EXG);
		assertEquals(test.presentValue(SwapDummyData.NOTIONAL_EXCHANGE_REC_GBP, MOCK_PROV), expected, 0d);
	  }

	  public virtual void test_presentValue_FxResetNotionalExchange()
	  {
		double expected = 0.0123d;
		SwapPaymentEventPricer<FxResetNotionalExchange> mockCalledFn = mock(typeof(SwapPaymentEventPricer));
		when(mockCalledFn.presentValue(SwapDummyData.FX_RESET_NOTIONAL_EXCHANGE_REC_USD, MOCK_PROV)).thenReturn(expected);
		DispatchingSwapPaymentEventPricer test = new DispatchingSwapPaymentEventPricer(MOCK_NOTIONAL_EXG, mockCalledFn);
		assertEquals(test.presentValue(SwapDummyData.FX_RESET_NOTIONAL_EXCHANGE_REC_USD, MOCK_PROV), expected, 0d);
	  }

	  public virtual void test_presentValue_unknownType()
	  {
		SwapPaymentEvent mockPaymentEvent = mock(typeof(SwapPaymentEvent));
		DispatchingSwapPaymentEventPricer test = DispatchingSwapPaymentEventPricer.DEFAULT;
		assertThrowsIllegalArg(() => test.presentValue(mockPaymentEvent, MOCK_PROV));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_forecastValue_NotionalExchange()
	  {
		double expected = 0.0123d;
		SwapPaymentEventPricer<NotionalExchange> mockCalledFn = mock(typeof(SwapPaymentEventPricer));
		when(mockCalledFn.forecastValue(SwapDummyData.NOTIONAL_EXCHANGE_REC_GBP, MOCK_PROV)).thenReturn(expected);
		DispatchingSwapPaymentEventPricer test = new DispatchingSwapPaymentEventPricer(mockCalledFn, MOCK_FX_NOTIONAL_EXG);
		assertEquals(test.forecastValue(SwapDummyData.NOTIONAL_EXCHANGE_REC_GBP, MOCK_PROV), expected, 0d);
	  }

	  public virtual void test_forecastValue_FxResetNotionalExchange()
	  {
		double expected = 0.0123d;
		SwapPaymentEventPricer<FxResetNotionalExchange> mockCalledFn = mock(typeof(SwapPaymentEventPricer));
		when(mockCalledFn.forecastValue(SwapDummyData.FX_RESET_NOTIONAL_EXCHANGE_REC_USD, MOCK_PROV)).thenReturn(expected);
		DispatchingSwapPaymentEventPricer test = new DispatchingSwapPaymentEventPricer(MOCK_NOTIONAL_EXG, mockCalledFn);
		assertEquals(test.forecastValue(SwapDummyData.FX_RESET_NOTIONAL_EXCHANGE_REC_USD, MOCK_PROV), expected, 0d);
	  }

	  public virtual void test_forecastValue_unknownType()
	  {
		SwapPaymentEvent mockPaymentEvent = mock(typeof(SwapPaymentEvent));
		DispatchingSwapPaymentEventPricer test = DispatchingSwapPaymentEventPricer.DEFAULT;
		assertThrowsIllegalArg(() => test.forecastValue(mockPaymentEvent, MOCK_PROV));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity_unknownType()
	  {
		SwapPaymentEvent mockPaymentEvent = mock(typeof(SwapPaymentEvent));
		DispatchingSwapPaymentEventPricer test = DispatchingSwapPaymentEventPricer.DEFAULT;
		assertThrowsIllegalArg(() => test.presentValueSensitivity(mockPaymentEvent, MOCK_PROV));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_forecastValueSensitivity_unknownType()
	  {
		SwapPaymentEvent mockPaymentEvent = mock(typeof(SwapPaymentEvent));
		DispatchingSwapPaymentEventPricer test = DispatchingSwapPaymentEventPricer.DEFAULT;
		assertThrowsIllegalArg(() => test.forecastValueSensitivity(mockPaymentEvent, MOCK_PROV));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currencyExposure_NotionalExchange()
	  {
		MultiCurrencyAmount expected = MultiCurrencyAmount.of(Currency.GBP, 0.0123d);
		SwapPaymentEventPricer<NotionalExchange> mockCalledFn = mock(typeof(SwapPaymentEventPricer));
		when(mockCalledFn.currencyExposure(SwapDummyData.NOTIONAL_EXCHANGE_REC_GBP, MOCK_PROV)).thenReturn(expected);
		DispatchingSwapPaymentEventPricer test = new DispatchingSwapPaymentEventPricer(mockCalledFn, MOCK_FX_NOTIONAL_EXG);
		assertEquals(test.currencyExposure(SwapDummyData.NOTIONAL_EXCHANGE_REC_GBP, MOCK_PROV), expected);
	  }

	  public virtual void test_currencyExposure_FxResetNotionalExchange()
	  {
		MultiCurrencyAmount expected = MultiCurrencyAmount.of(Currency.GBP, 0.0123d);
		SwapPaymentEventPricer<FxResetNotionalExchange> mockCalledFn = mock(typeof(SwapPaymentEventPricer));
		when(mockCalledFn.currencyExposure(SwapDummyData.FX_RESET_NOTIONAL_EXCHANGE_REC_USD, MOCK_PROV)).thenReturn(expected);
		DispatchingSwapPaymentEventPricer test = new DispatchingSwapPaymentEventPricer(MOCK_NOTIONAL_EXG, mockCalledFn);
		assertEquals(test.currencyExposure(SwapDummyData.FX_RESET_NOTIONAL_EXCHANGE_REC_USD, MOCK_PROV), expected);
	  }

	  public virtual void test_currencyExposure_unknownType()
	  {
		SwapPaymentEvent mockPaymentEvent = mock(typeof(SwapPaymentEvent));
		DispatchingSwapPaymentEventPricer test = DispatchingSwapPaymentEventPricer.DEFAULT;
		assertThrowsIllegalArg(() => test.currencyExposure(mockPaymentEvent, MOCK_PROV));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currentCash_NotionalExchange()
	  {
		double expected = 0.0123d;
		SwapPaymentEventPricer<NotionalExchange> mockCalledFn = mock(typeof(SwapPaymentEventPricer));
		when(mockCalledFn.currentCash(SwapDummyData.NOTIONAL_EXCHANGE_REC_GBP, MOCK_PROV)).thenReturn(expected);
		DispatchingSwapPaymentEventPricer test = new DispatchingSwapPaymentEventPricer(mockCalledFn, MOCK_FX_NOTIONAL_EXG);
		assertEquals(test.currentCash(SwapDummyData.NOTIONAL_EXCHANGE_REC_GBP, MOCK_PROV), expected, 0d);
	  }

	  public virtual void test_currentCash_FxResetNotionalExchange()
	  {
		double expected = 0.0123d;
		SwapPaymentEventPricer<FxResetNotionalExchange> mockCalledFn = mock(typeof(SwapPaymentEventPricer));
		when(mockCalledFn.currentCash(SwapDummyData.FX_RESET_NOTIONAL_EXCHANGE_REC_USD, MOCK_PROV)).thenReturn(expected);
		DispatchingSwapPaymentEventPricer test = new DispatchingSwapPaymentEventPricer(MOCK_NOTIONAL_EXG, mockCalledFn);
		assertEquals(test.currentCash(SwapDummyData.FX_RESET_NOTIONAL_EXCHANGE_REC_USD, MOCK_PROV), expected, 0d);
	  }

	  public virtual void test_currentCash_unknownType()
	  {
		SwapPaymentEvent mockPaymentEvent = mock(typeof(SwapPaymentEvent));
		DispatchingSwapPaymentEventPricer test = DispatchingSwapPaymentEventPricer.DEFAULT;
		assertThrowsIllegalArg(() => test.currentCash(mockPaymentEvent, MOCK_PROV));
	  }
	}

}