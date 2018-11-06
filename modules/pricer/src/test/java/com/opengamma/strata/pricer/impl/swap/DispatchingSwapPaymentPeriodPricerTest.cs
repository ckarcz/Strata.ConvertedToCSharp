/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.ignoreThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.mock;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.when;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SwapDummyData = com.opengamma.strata.pricer.swap.SwapDummyData;
	using SwapPaymentPeriodPricer = com.opengamma.strata.pricer.swap.SwapPaymentPeriodPricer;
	using KnownAmountSwapPaymentPeriod = com.opengamma.strata.product.swap.KnownAmountSwapPaymentPeriod;
	using RatePaymentPeriod = com.opengamma.strata.product.swap.RatePaymentPeriod;
	using SwapPaymentPeriod = com.opengamma.strata.product.swap.SwapPaymentPeriod;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DispatchingSwapPaymentPeriodPricerTest
	public class DispatchingSwapPaymentPeriodPricerTest
	{

	  private static readonly RatesProvider MOCK_PROV = new MockRatesProvider();
	  private static readonly SwapPaymentPeriodPricer<RatePaymentPeriod> MOCK_RATE = mock(typeof(SwapPaymentPeriodPricer));
	  private static readonly SwapPaymentPeriodPricer<KnownAmountSwapPaymentPeriod> MOCK_KNOWN = mock(typeof(SwapPaymentPeriodPricer));

	  public virtual void test_presentValue_RatePaymentPeriod()
	  {
		double expected = 0.0123d;
		SwapPaymentPeriodPricer<RatePaymentPeriod> mockNotionalExchangeFn = mock(typeof(SwapPaymentPeriodPricer));
		when(mockNotionalExchangeFn.presentValue(SwapDummyData.FIXED_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV)).thenReturn(expected);
		DispatchingSwapPaymentPeriodPricer test = new DispatchingSwapPaymentPeriodPricer(mockNotionalExchangeFn, MOCK_KNOWN);
		assertEquals(test.presentValue(SwapDummyData.FIXED_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV), expected, 0d);
	  }

	  public virtual void test_presentValue_unknownType()
	  {
		SwapPaymentPeriod mockPaymentPeriod = mock(typeof(SwapPaymentPeriod));
		DispatchingSwapPaymentPeriodPricer test = DispatchingSwapPaymentPeriodPricer.DEFAULT;
		assertThrowsIllegalArg(() => test.presentValue(mockPaymentPeriod, MOCK_PROV));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_forecastValue_RatePaymentPeriod()
	  {
		double expected = 0.0123d;
		SwapPaymentPeriodPricer<RatePaymentPeriod> mockNotionalExchangeFn = mock(typeof(SwapPaymentPeriodPricer));
		when(mockNotionalExchangeFn.forecastValue(SwapDummyData.FIXED_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV)).thenReturn(expected);
		DispatchingSwapPaymentPeriodPricer test = new DispatchingSwapPaymentPeriodPricer(mockNotionalExchangeFn, MOCK_KNOWN);
		assertEquals(test.forecastValue(SwapDummyData.FIXED_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV), expected, 0d);
	  }

	  public virtual void test_forecastValue_unknownType()
	  {
		SwapPaymentPeriod mockPaymentPeriod = mock(typeof(SwapPaymentPeriod));
		DispatchingSwapPaymentPeriodPricer test = DispatchingSwapPaymentPeriodPricer.DEFAULT;
		assertThrowsIllegalArg(() => test.forecastValue(mockPaymentPeriod, MOCK_PROV));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity_unknownType()
	  {
		SwapPaymentPeriod mockPaymentPeriod = mock(typeof(SwapPaymentPeriod));
		DispatchingSwapPaymentPeriodPricer test = DispatchingSwapPaymentPeriodPricer.DEFAULT;
		assertThrowsIllegalArg(() => test.presentValueSensitivity(mockPaymentPeriod, MOCK_PROV));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_forecastValueSensitivity_unknownType()
	  {
		SwapPaymentPeriod mockPaymentPeriod = mock(typeof(SwapPaymentPeriod));
		DispatchingSwapPaymentPeriodPricer test = DispatchingSwapPaymentPeriodPricer.DEFAULT;
		assertThrowsIllegalArg(() => test.forecastValueSensitivity(mockPaymentPeriod, MOCK_PROV));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currencyExposure_RatePaymentPeriod()
	  {
		MultiCurrencyAmount expected = MultiCurrencyAmount.of(GBP, 0.0123d);
		SwapPaymentPeriodPricer<RatePaymentPeriod> mockNotionalExchangeFn = mock(typeof(SwapPaymentPeriodPricer));
		when(mockNotionalExchangeFn.currencyExposure(SwapDummyData.FIXED_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV)).thenReturn(expected);
		DispatchingSwapPaymentPeriodPricer test = new DispatchingSwapPaymentPeriodPricer(mockNotionalExchangeFn, MOCK_KNOWN);
		assertEquals(test.currencyExposure(SwapDummyData.FIXED_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV), expected);
	  }

	  public virtual void test_currencyExposure_unknownType()
	  {
		SwapPaymentPeriod mockPaymentPeriod = mock(typeof(SwapPaymentPeriod));
		DispatchingSwapPaymentPeriodPricer test = DispatchingSwapPaymentPeriodPricer.DEFAULT;
		assertThrowsIllegalArg(() => test.currencyExposure(mockPaymentPeriod, MOCK_PROV));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currentCash_RatePaymentPeriod()
	  {
		double expected = 0.0123d;
		SwapPaymentPeriodPricer<RatePaymentPeriod> mockNotionalExchangeFn = mock(typeof(SwapPaymentPeriodPricer));
		when(mockNotionalExchangeFn.currentCash(SwapDummyData.FIXED_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV)).thenReturn(expected);
		DispatchingSwapPaymentPeriodPricer test = new DispatchingSwapPaymentPeriodPricer(mockNotionalExchangeFn, MOCK_KNOWN);
		assertEquals(test.currentCash(SwapDummyData.FIXED_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV), expected, 0d);
	  }

	  public virtual void test_currentCash_unknownType()
	  {
		SwapPaymentPeriod mockPaymentPeriod = mock(typeof(SwapPaymentPeriod));
		DispatchingSwapPaymentPeriodPricer test = DispatchingSwapPaymentPeriodPricer.DEFAULT;
		assertThrowsIllegalArg(() => test.currentCash(mockPaymentPeriod, MOCK_PROV));
	  }

	  //------------------------------------------------------------------------- 
	  public virtual void coverage()
	  {
		DispatchingSwapPaymentPeriodPricer test = new DispatchingSwapPaymentPeriodPricer(MOCK_RATE, MOCK_KNOWN);

		SwapPaymentPeriod kapp = KnownAmountSwapPaymentPeriod.builder().payment(Payment.of(CurrencyAmount.of(GBP, 1000), date(2015, 8, 21))).startDate(date(2015, 5, 19)).endDate(date(2015, 8, 19)).build();
		SwapPaymentPeriod mockPaymentPeriod = mock(typeof(SwapPaymentPeriod));

		ignoreThrows(() => test.presentValue(SwapDummyData.FIXED_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV));
		ignoreThrows(() => test.presentValue(kapp, MOCK_PROV));
		ignoreThrows(() => test.presentValue(mockPaymentPeriod, MOCK_PROV));

		ignoreThrows(() => test.forecastValue(SwapDummyData.FIXED_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV));
		ignoreThrows(() => test.forecastValue(kapp, MOCK_PROV));
		ignoreThrows(() => test.forecastValue(mockPaymentPeriod, MOCK_PROV));

		ignoreThrows(() => test.pvbp(SwapDummyData.FIXED_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV));
		ignoreThrows(() => test.pvbp(kapp, MOCK_PROV));
		ignoreThrows(() => test.pvbp(mockPaymentPeriod, MOCK_PROV));

		ignoreThrows(() => test.presentValueSensitivity(SwapDummyData.FIXED_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV));
		ignoreThrows(() => test.presentValueSensitivity(kapp, MOCK_PROV));
		ignoreThrows(() => test.presentValueSensitivity(mockPaymentPeriod, MOCK_PROV));

		ignoreThrows(() => test.forecastValueSensitivity(SwapDummyData.FIXED_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV));
		ignoreThrows(() => test.forecastValueSensitivity(kapp, MOCK_PROV));
		ignoreThrows(() => test.forecastValueSensitivity(mockPaymentPeriod, MOCK_PROV));

		ignoreThrows(() => test.pvbpSensitivity(SwapDummyData.FIXED_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV));
		ignoreThrows(() => test.pvbpSensitivity(kapp, MOCK_PROV));
		ignoreThrows(() => test.pvbpSensitivity(mockPaymentPeriod, MOCK_PROV));

		ignoreThrows(() => test.accruedInterest(SwapDummyData.FIXED_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV));
		ignoreThrows(() => test.accruedInterest(kapp, MOCK_PROV));
		ignoreThrows(() => test.accruedInterest(mockPaymentPeriod, MOCK_PROV));

		ExplainMapBuilder explain = ExplainMap.builder();
		ignoreThrows(() => test.explainPresentValue(SwapDummyData.FIXED_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV, explain));
		ignoreThrows(() => test.explainPresentValue(kapp, MOCK_PROV, explain));
		ignoreThrows(() => test.explainPresentValue(mockPaymentPeriod, MOCK_PROV, explain));

		ignoreThrows(() => test.currencyExposure(SwapDummyData.FIXED_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV));
		ignoreThrows(() => test.currencyExposure(kapp, MOCK_PROV));
		ignoreThrows(() => test.currencyExposure(mockPaymentPeriod, MOCK_PROV));

		ignoreThrows(() => test.currentCash(SwapDummyData.FIXED_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV));
		ignoreThrows(() => test.currentCash(kapp, MOCK_PROV));
		ignoreThrows(() => test.currentCash(mockPaymentPeriod, MOCK_PROV));
	  }

	}

}