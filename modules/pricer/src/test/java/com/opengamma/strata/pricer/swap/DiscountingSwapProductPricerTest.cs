using System;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_5Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.GB_RPI;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.datasets.RatesProviderDataSets.MULTI_EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.datasets.RatesProviderDataSets.MULTI_USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.FIXED_RATE_PAYMENT_PERIOD_PAY_GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.FIXED_RATE_PAYMENT_PERIOD_PAY_USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.FIXED_SWAP_LEG_PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.FIXED_SWAP_LEG_PAY_USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.IBOR_RATE_COMP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.IBOR_RATE_PAYMENT_PERIOD_REC_GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.IBOR_SWAP_LEG_REC_GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.INFLATION_FIXED_SWAP_LEG_PAY_GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.INFLATION_FIXED_SWAP_LEG_PAY_GBP_FIXED_RATE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.INFLATION_MONTHLY_SWAP_LEG_REC_GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.NOTIONAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.NOTIONAL_EXCHANGE_PAY_GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.NOTIONAL_EXCHANGE_PAY_USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.NOTIONAL_EXCHANGE_REC_GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.SWAP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.SWAP_CROSS_CURRENCY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.SWAP_INFLATION;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.SWAP_TRADE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.SWAP_TRADE_CROSS_CURRENCY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.BUY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedIborSwapConventions.GBP_FIXED_1Y_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.IborIborSwapConventions.USD_LIBOR_3M_LIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.mock;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.when;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using CashFlow = com.opengamma.strata.market.amount.CashFlow;
	using CashFlows = com.opengamma.strata.market.amount.CashFlows;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProviderDataSets = com.opengamma.strata.pricer.datasets.RatesProviderDataSets;
	using MockRatesProvider = com.opengamma.strata.pricer.impl.MockRatesProvider;
	using IborRateSensitivity = com.opengamma.strata.pricer.rate.IborRateSensitivity;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using CompoundingMethod = com.opengamma.strata.product.swap.CompoundingMethod;
	using FixedRateCalculation = com.opengamma.strata.product.swap.FixedRateCalculation;
	using NotionalSchedule = com.opengamma.strata.product.swap.NotionalSchedule;
	using PaymentSchedule = com.opengamma.strata.product.swap.PaymentSchedule;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;
	using SwapPaymentEvent = com.opengamma.strata.product.swap.SwapPaymentEvent;
	using SwapPaymentPeriod = com.opengamma.strata.product.swap.SwapPaymentPeriod;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;
	using FixedIborSwapTemplate = com.opengamma.strata.product.swap.type.FixedIborSwapTemplate;
	using FixedInflationSwapConventions = com.opengamma.strata.product.swap.type.FixedInflationSwapConventions;
	using IborIborSwapConvention = com.opengamma.strata.product.swap.type.IborIborSwapConvention;
	using IborIborSwapTemplate = com.opengamma.strata.product.swap.type.IborIborSwapTemplate;
	using IborRateSwapLegConvention = com.opengamma.strata.product.swap.type.IborRateSwapLegConvention;
	using ImmutableIborIborSwapConvention = com.opengamma.strata.product.swap.type.ImmutableIborIborSwapConvention;
	using ThreeLegBasisSwapConvention = com.opengamma.strata.product.swap.type.ThreeLegBasisSwapConvention;
	using ThreeLegBasisSwapConventions = com.opengamma.strata.product.swap.type.ThreeLegBasisSwapConventions;

	/// <summary>
	/// Tests <seealso cref="DiscountingSwapProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingSwapProductPricerTest
	public class DiscountingSwapProductPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly RatesProvider MOCK_PROV = new MockRatesProvider(RatesProviderDataSets.VAL_DATE_2014_01_22);
	  private static readonly LocalDate VAL_DATE_INFLATION = date(2014, 7, 8);

	  private static readonly ImmutableRatesProvider RATES_GBP = RatesProviderDataSets.MULTI_GBP;

	  private static readonly ImmutableRatesProvider RATES_GBP_USD = RatesProviderDataSets.MULTI_GBP_USD;
	  private const double FD_SHIFT = 1.0E-7;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FINITE_DIFFERENCE_CALCULATOR = new RatesFiniteDifferenceSensitivityCalculator(FD_SHIFT);

	  private const double TOLERANCE_RATE = 1.0e-12;
	  private const double TOLERANCE_RATE_DELTA = 1.0E-6;
	  private const double TOLERANCE_PV = 1.0e-2;

	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;
	  private static readonly double[] INDEX_VALUES = new double[] {242d, 242d, 242d, 242d, 242d, 242d};
	  private const double START_INDEX = 218d;
	  private static readonly LocalDateDoubleTimeSeries TS_INFLATION = LocalDateDoubleTimeSeries.of(date(2014, 3, 31), START_INDEX);
	  private static readonly Curve PRICE_CURVE = InterpolatedNodalCurve.of(Curves.prices("GB_RPI_CURVE_FLAT"), DoubleArray.of(1, 2, 3, 4, 5, 6), DoubleArray.ofUnsafe(INDEX_VALUES), INTERPOLATOR);
	  private static readonly ImmutableRatesProvider RATES_GBP_INFLATION = ImmutableRatesProvider.builder(VAL_DATE_INFLATION).discountCurves(RatesProviderDataSets.multiGbp(VAL_DATE_INFLATION).DiscountCurves).priceIndexCurve(GB_RPI, PRICE_CURVE).timeSeries(GB_RPI, TS_INFLATION).build();

	  // non compounding
	  private static readonly IborIborSwapConvention CONV_USD_LIBOR3M_LIBOR6M = ImmutableIborIborSwapConvention.of("USD-Swap", IborRateSwapLegConvention.of(USD_LIBOR_3M), IborRateSwapLegConvention.of(USD_LIBOR_6M));
	  private const double FIXED_RATE = 0.01;
	  private const double SPREAD = 0.0015;
	  private const double NOTIONAL_SWAP = 100_000_000;
	  private static readonly SwapTrade SWAP_USD_FIXED_6M_LIBOR_3M_5Y = FixedIborSwapTemplate.of(Period.ZERO, TENOR_5Y, USD_FIXED_6M_LIBOR_3M).createTrade(MULTI_USD.ValuationDate, BUY, NOTIONAL_SWAP, FIXED_RATE, REF_DATA);
	  private static readonly SwapTrade SWAP_USD_LIBOR_3M_LIBOR_6M_5Y = IborIborSwapTemplate.of(Period.ZERO, TENOR_5Y, CONV_USD_LIBOR3M_LIBOR6M).createTrade(MULTI_USD.ValuationDate, BUY, NOTIONAL_SWAP, SPREAD, REF_DATA);
	  private static readonly SwapTrade SWAP_GBP_ZC_INFLATION_5Y = FixedInflationSwapConventions.GBP_FIXED_ZC_GB_RPI.createTrade(VAL_DATE_INFLATION, TENOR_5Y, BUY, NOTIONAL_SWAP, FIXED_RATE, REF_DATA);

	  private static readonly DiscountingSwapProductPricer SWAP_PRODUCT_PRICER = DiscountingSwapProductPricer.DEFAULT;
	  private static readonly DiscountingSwapTradePricer SWAP_TRADE_PRICER = DiscountingSwapTradePricer.DEFAULT;

	  //-------------------------------------------------------------------------
	  public virtual void test_getters()
	  {
		assertEquals(DiscountingSwapProductPricer.DEFAULT.LegPricer, DiscountingSwapLegPricer.DEFAULT);
		assertEquals(DiscountingSwapTradePricer.DEFAULT.ProductPricer, DiscountingSwapProductPricer.DEFAULT);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public void test_legPricer()
	  public virtual void test_legPricer()
	  {
		SwapPaymentPeriodPricer<SwapPaymentPeriod> mockPeriod = mock(typeof(SwapPaymentPeriodPricer));
		SwapPaymentEventPricer<SwapPaymentEvent> mockEvent = mock(typeof(SwapPaymentEventPricer));
		DiscountingSwapLegPricer pricerLeg = new DiscountingSwapLegPricer(mockPeriod, mockEvent);
		DiscountingSwapProductPricer pricerSwap = new DiscountingSwapProductPricer(pricerLeg);
		assertEquals(pricerSwap.LegPricer, pricerLeg);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parRate_singleCurrency()
	  {
		RatesProvider mockProv = mock(typeof(RatesProvider));
		when(mockProv.discountFactor(GBP, FIXED_RATE_PAYMENT_PERIOD_PAY_GBP.PaymentDate)).thenReturn(0.99d);
		when(mockProv.ValuationDate).thenReturn(RatesProviderDataSets.VAL_DATE_2014_01_22);
		when(mockProv.fxRate(GBP, GBP)).thenReturn(1.0);
		SwapPaymentPeriodPricer<SwapPaymentPeriod> mockPeriod = mock(typeof(SwapPaymentPeriodPricer));
		double fwdRate = 0.01;
		double pvCpnIbor = 0.99 * fwdRate * 0.25 * 1_000_000;
		when(mockPeriod.presentValue(IBOR_RATE_PAYMENT_PERIOD_REC_GBP, mockProv)).thenReturn(pvCpnIbor);
		double pvbpCpnFixed = -0.99 * 0.25 * 1_000_000;
		double pvCpnFixed = 0.0123d * pvbpCpnFixed;
		when(mockPeriod.presentValue(FIXED_RATE_PAYMENT_PERIOD_PAY_GBP, mockProv)).thenReturn(pvCpnFixed);
		when(mockPeriod.pvbp(FIXED_RATE_PAYMENT_PERIOD_PAY_GBP, mockProv)).thenReturn(pvbpCpnFixed);
		SwapPaymentEventPricer<SwapPaymentEvent> mockEvent = mock(typeof(SwapPaymentEventPricer));
		double pvNotional = 980_000d;
		when(mockEvent.presentValue(NOTIONAL_EXCHANGE_REC_GBP, mockProv)).thenReturn(pvNotional);
		when(mockEvent.presentValue(NOTIONAL_EXCHANGE_PAY_GBP, mockProv)).thenReturn(-pvNotional);
		DiscountingSwapLegPricer pricerLeg = new DiscountingSwapLegPricer(mockPeriod, mockEvent);
		DiscountingSwapProductPricer pricerSwap = new DiscountingSwapProductPricer(pricerLeg);
		double pvbp = pricerLeg.pvbp(FIXED_SWAP_LEG_PAY, mockProv);
		double parRateExpected1 = -(pvCpnIbor + -pvNotional + pvNotional) / pvbp;
		double parRateExpected2 = fwdRate;
		double parRateComputed = pricerSwap.parRate(SWAP, mockProv);
		assertEquals(parRateComputed, parRateExpected1, TOLERANCE_RATE);
		assertEquals(parRateComputed, parRateExpected2, TOLERANCE_RATE);

		// test via SwapTrade
		DiscountingSwapTradePricer pricerTrade = new DiscountingSwapTradePricer(pricerSwap);
		assertEquals(pricerTrade.parRate(SWAP_TRADE, MOCK_PROV), pricerSwap.parRate(SWAP, MOCK_PROV));
	  }

	  public virtual void test_parRate_crossCurrency()
	  {
		RatesProvider mockProv = mock(typeof(RatesProvider));
		when(mockProv.discountFactor(USD, FIXED_RATE_PAYMENT_PERIOD_PAY_USD.PaymentDate)).thenReturn(0.99d);
		when(mockProv.ValuationDate).thenReturn(RatesProviderDataSets.VAL_DATE_2014_01_22);
		when(mockProv.fxRate(GBP, GBP)).thenReturn(1.0);
		when(mockProv.fxRate(USD, USD)).thenReturn(1.0);
		double fxGbpUsd = 1.51d;
		when(mockProv.fxRate(GBP, USD)).thenReturn(fxGbpUsd);
		SwapPaymentPeriodPricer<SwapPaymentPeriod> mockPeriod = mock(typeof(SwapPaymentPeriodPricer));
		double fwdRate = 0.01;
		double pvCpnIborGbp = 0.99 * fwdRate * 0.25 * 1_000_000;
		when(mockPeriod.presentValue(IBOR_RATE_PAYMENT_PERIOD_REC_GBP, mockProv)).thenReturn(pvCpnIborGbp);
		SwapPaymentEventPricer<SwapPaymentEvent> mockEvent = mock(typeof(SwapPaymentEventPricer));
		double pvNotionalGbp = 980_000d;
		when(mockEvent.presentValue(NOTIONAL_EXCHANGE_REC_GBP, mockProv)).thenReturn(pvNotionalGbp);
		double pvNotionalUsd = -fxGbpUsd * 981_000d;
		when(mockEvent.presentValue(NOTIONAL_EXCHANGE_PAY_USD, mockProv)).thenReturn(pvNotionalUsd);
		DiscountingSwapLegPricer pricerLeg = new DiscountingSwapLegPricer(mockPeriod, mockEvent);
		DiscountingSwapProductPricer pricerSwap = new DiscountingSwapProductPricer(pricerLeg);
		double pvbp = pricerLeg.pvbp(FIXED_SWAP_LEG_PAY_USD, mockProv);
		double parRateExpected = -((pvCpnIborGbp + pvNotionalGbp) * fxGbpUsd + pvNotionalUsd) / pvbp;
		double parRateComputed = pricerSwap.parRate(SWAP, mockProv);
		assertEquals(parRateComputed, parRateExpected, TOLERANCE_RATE);
	  }

	  public virtual void test_parRate_bothLegFloating()
	  {
		ResolvedSwap swap = ResolvedSwap.of(IBOR_SWAP_LEG_REC_GBP, IBOR_SWAP_LEG_REC_GBP);
		SwapPaymentPeriodPricer<SwapPaymentPeriod> mockPeriod = mock(typeof(SwapPaymentPeriodPricer));
		SwapPaymentEventPricer<SwapPaymentEvent> mockEvent = mock(typeof(SwapPaymentEventPricer));
		DiscountingSwapLegPricer pricerLeg = new DiscountingSwapLegPricer(mockPeriod, mockEvent);
		DiscountingSwapProductPricer pricerSwap = new DiscountingSwapProductPricer(pricerLeg);
		assertThrowsIllegalArg(() => pricerSwap.parRate(swap, MOCK_PROV));
	  }

	  public virtual void test_parRate_inflation()
	  {
		DiscountingSwapLegPricer pricerLeg = DiscountingSwapLegPricer.DEFAULT;
		DiscountingSwapProductPricer pricerSwap = new DiscountingSwapProductPricer(pricerLeg);
		ImmutableRatesProvider prov = ImmutableRatesProvider.builder(VAL_DATE_INFLATION).discountCurves(RATES_GBP_INFLATION.DiscountCurves).priceIndexCurve(GB_RPI, PRICE_CURVE).timeSeries(GB_RPI, TS_INFLATION).build();
		double parRateComputed = pricerSwap.parRate(SWAP_INFLATION, prov);
		ResolvedSwapLeg fixedLeg = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(date(2014, 6, 9)).endDate(date(2019, 6, 9)).frequency(Frequency.P12M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.ofYears(5)).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).compoundingMethod(CompoundingMethod.STRAIGHT).build()).notionalSchedule(NotionalSchedule.of(GBP, NOTIONAL)).calculation(FixedRateCalculation.builder().rate(ValueSchedule.of(parRateComputed)).dayCount(DayCounts.ONE_ONE).build()).build().resolve(REF_DATA);
		ResolvedSwap swapWithParRate = ResolvedSwap.of(INFLATION_MONTHLY_SWAP_LEG_REC_GBP, fixedLeg);
		double pvWithParRate = pricerSwap.presentValue(swapWithParRate, prov).getAmount(GBP).Amount;
		assertEquals(pvWithParRate, 0.0d, NOTIONAL * TOLERANCE_RATE);
	  }

	  public virtual void test_parRate_inflation_periodic()
	  {
		ResolvedSwapLeg fixedLeg = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(date(2014, 6, 9)).endDate(date(2019, 6, 9)).frequency(Frequency.P6M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).build()).notionalSchedule(NotionalSchedule.of(GBP, NOTIONAL)).calculation(FixedRateCalculation.builder().rate(ValueSchedule.of(0.04)).dayCount(DayCounts.ACT_365F).build()).build().resolve(REF_DATA);
		ResolvedSwap swap = ResolvedSwap.of(INFLATION_MONTHLY_SWAP_LEG_REC_GBP, fixedLeg);
		DiscountingSwapLegPricer pricerLeg = DiscountingSwapLegPricer.DEFAULT;
		DiscountingSwapProductPricer pricerSwap = new DiscountingSwapProductPricer(pricerLeg);
		ImmutableRatesProvider prov = ImmutableRatesProvider.builder(VAL_DATE_INFLATION).discountCurves(RATES_GBP_INFLATION.DiscountCurves).priceIndexCurve(GB_RPI, PRICE_CURVE).timeSeries(GB_RPI, TS_INFLATION).build();
		double parRateComputed = pricerSwap.parRate(swap, prov);
		ResolvedSwapLeg fixedLegWithParRate = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(date(2014, 6, 9)).endDate(date(2019, 6, 9)).frequency(Frequency.P6M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).build()).notionalSchedule(NotionalSchedule.of(GBP, NOTIONAL)).calculation(FixedRateCalculation.builder().rate(ValueSchedule.of(parRateComputed)).dayCount(DayCounts.ACT_365F).build()).build().resolve(REF_DATA);
		ResolvedSwap swapWithParRate = ResolvedSwap.of(INFLATION_MONTHLY_SWAP_LEG_REC_GBP, fixedLegWithParRate);
		double pvWithParRate = pricerSwap.presentValue(swapWithParRate, prov).getAmount(GBP).Amount;
		assertEquals(pvWithParRate, 0.0d, NOTIONAL * TOLERANCE_RATE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue_singleCurrency()
	  {
		SwapPaymentPeriodPricer<SwapPaymentPeriod> mockPeriod = mock(typeof(SwapPaymentPeriodPricer));
		when(mockPeriod.presentValue(IBOR_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV)).thenReturn(1000d);
		when(mockPeriod.presentValue(FIXED_RATE_PAYMENT_PERIOD_PAY_GBP, MOCK_PROV)).thenReturn(-500d);
		SwapPaymentEventPricer<SwapPaymentEvent> mockEvent = mock(typeof(SwapPaymentEventPricer));
		when(mockEvent.presentValue(NOTIONAL_EXCHANGE_REC_GBP, MOCK_PROV)).thenReturn(35d);
		when(mockEvent.presentValue(NOTIONAL_EXCHANGE_PAY_GBP, MOCK_PROV)).thenReturn(-30d);
		DiscountingSwapLegPricer pricerLeg = new DiscountingSwapLegPricer(mockPeriod, mockEvent);
		DiscountingSwapProductPricer pricerSwap = new DiscountingSwapProductPricer(pricerLeg);
		assertEquals(pricerSwap.presentValue(SWAP, MOCK_PROV), MultiCurrencyAmount.of(GBP, 505d));

		// test via SwapTrade
		DiscountingSwapTradePricer pricerTrade = new DiscountingSwapTradePricer(pricerSwap);
		assertEquals(pricerTrade.presentValue(SWAP_TRADE, MOCK_PROV), pricerSwap.presentValue(SWAP, MOCK_PROV));
	  }

	  public virtual void test_presentValue_crossCurrency()
	  {
		SwapPaymentPeriodPricer<SwapPaymentPeriod> mockPeriod = mock(typeof(SwapPaymentPeriodPricer));
		when(mockPeriod.presentValue(IBOR_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV)).thenReturn(1000d);
		when(mockPeriod.presentValue(FIXED_RATE_PAYMENT_PERIOD_PAY_USD, MOCK_PROV)).thenReturn(-500d);
		SwapPaymentEventPricer<SwapPaymentEvent> mockEvent = mock(typeof(SwapPaymentEventPricer));
		DiscountingSwapLegPricer pricerLeg = new DiscountingSwapLegPricer(mockPeriod, mockEvent);
		DiscountingSwapProductPricer pricerSwap = new DiscountingSwapProductPricer(pricerLeg);
		MultiCurrencyAmount expected = MultiCurrencyAmount.of(CurrencyAmount.of(GBP, 1000d), CurrencyAmount.of(USD, -500d));
		assertEquals(pricerSwap.presentValue(SWAP_CROSS_CURRENCY, MOCK_PROV), expected);

		// test via SwapTrade
		DiscountingSwapTradePricer pricerTrade = new DiscountingSwapTradePricer(pricerSwap);
		assertEquals(pricerTrade.presentValue(SWAP_TRADE_CROSS_CURRENCY, MOCK_PROV), pricerSwap.presentValue(SWAP_CROSS_CURRENCY, MOCK_PROV));
	  }

	  public virtual void test_presentValue_withCurrency_crossCurrency()
	  {
		SwapPaymentPeriodPricer<SwapPaymentPeriod> mockPeriod = mock(typeof(SwapPaymentPeriodPricer));
		when(mockPeriod.presentValue(IBOR_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV)).thenReturn(1000d);
		when(mockPeriod.presentValue(FIXED_RATE_PAYMENT_PERIOD_PAY_USD, MOCK_PROV)).thenReturn(-500d);
		SwapPaymentEventPricer<SwapPaymentEvent> mockEvent = mock(typeof(SwapPaymentEventPricer));
		DiscountingSwapLegPricer pricerLeg = new DiscountingSwapLegPricer(mockPeriod, mockEvent);
		DiscountingSwapProductPricer pricerSwap = new DiscountingSwapProductPricer(pricerLeg);
		CurrencyAmount expected = CurrencyAmount.of(USD, 1000d * MockRatesProvider.RATE - 500d);
		assertEquals(pricerSwap.presentValue(SWAP_CROSS_CURRENCY, USD, MOCK_PROV), expected);

		// test via SwapTrade
		DiscountingSwapTradePricer pricerTrade = new DiscountingSwapTradePricer(pricerSwap);
		assertEquals(pricerTrade.presentValue(SWAP_TRADE_CROSS_CURRENCY, USD, MOCK_PROV), pricerSwap.presentValue(SWAP_CROSS_CURRENCY, USD, MOCK_PROV));
	  }

	  public virtual void test_presentValue_inflation()
	  {
		DiscountingSwapLegPricer pricerLeg = DiscountingSwapLegPricer.DEFAULT;
		DiscountingSwapProductPricer pricerSwap = new DiscountingSwapProductPricer(pricerLeg);
		LocalDate paymentDate = SWAP_INFLATION.Legs.get(0).PaymentPeriods.get(0).PaymentDate;
		double fixedRate = INFLATION_FIXED_SWAP_LEG_PAY_GBP_FIXED_RATE;
		MultiCurrencyAmount pvComputed = pricerSwap.presentValue(SWAP_INFLATION, RATES_GBP_INFLATION);
		double pvExpected = (-(INDEX_VALUES[0] / START_INDEX - 1.0) + Math.Pow(1 + fixedRate, 5) - 1.0) * NOTIONAL * RATES_GBP_INFLATION.discountFactor(GBP, paymentDate);
		assertTrue(pvComputed.Currencies.size() == 1);
		assertEquals(pvComputed.getAmount(GBP).Amount, pvExpected, NOTIONAL * TOLERANCE_RATE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_forecastValue_singleCurrency()
	  {
		SwapPaymentPeriodPricer<SwapPaymentPeriod> mockPeriod = mock(typeof(SwapPaymentPeriodPricer));
		when(mockPeriod.forecastValue(IBOR_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV)).thenReturn(1000d);
		when(mockPeriod.forecastValue(FIXED_RATE_PAYMENT_PERIOD_PAY_GBP, MOCK_PROV)).thenReturn(-500d);
		SwapPaymentEventPricer<SwapPaymentEvent> mockEvent = mock(typeof(SwapPaymentEventPricer));
		DiscountingSwapLegPricer pricerLeg = new DiscountingSwapLegPricer(mockPeriod, mockEvent);
		DiscountingSwapProductPricer pricerSwap = new DiscountingSwapProductPricer(pricerLeg);
		assertEquals(pricerSwap.forecastValue(SWAP, MOCK_PROV), MultiCurrencyAmount.of(GBP, 500d));

		// test via SwapTrade
		DiscountingSwapTradePricer pricerTrade = new DiscountingSwapTradePricer(pricerSwap);
		assertEquals(pricerTrade.forecastValue(SWAP_TRADE, MOCK_PROV), pricerSwap.forecastValue(SWAP, MOCK_PROV));
	  }

	  public virtual void test_forecastValue_crossCurrency()
	  {
		SwapPaymentPeriodPricer<SwapPaymentPeriod> mockPeriod = mock(typeof(SwapPaymentPeriodPricer));
		when(mockPeriod.forecastValue(IBOR_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV)).thenReturn(1000d);
		when(mockPeriod.forecastValue(FIXED_RATE_PAYMENT_PERIOD_PAY_USD, MOCK_PROV)).thenReturn(-500d);
		SwapPaymentEventPricer<SwapPaymentEvent> mockEvent = mock(typeof(SwapPaymentEventPricer));
		DiscountingSwapLegPricer pricerLeg = new DiscountingSwapLegPricer(mockPeriod, mockEvent);
		DiscountingSwapProductPricer pricerSwap = new DiscountingSwapProductPricer(pricerLeg);
		MultiCurrencyAmount expected = MultiCurrencyAmount.of(CurrencyAmount.of(GBP, 1000d), CurrencyAmount.of(USD, -500d));
		assertEquals(pricerSwap.forecastValue(SWAP_CROSS_CURRENCY, MOCK_PROV), expected);

		// test via SwapTrade
		DiscountingSwapTradePricer pricerTrade = new DiscountingSwapTradePricer(pricerSwap);
		assertEquals(pricerTrade.forecastValue(SWAP_TRADE_CROSS_CURRENCY, MOCK_PROV), pricerSwap.forecastValue(SWAP_CROSS_CURRENCY, MOCK_PROV));
	  }

	  public virtual void test_forecastValue_inflation()
	  {
		DiscountingSwapLegPricer pricerLeg = DiscountingSwapLegPricer.DEFAULT;
		DiscountingSwapProductPricer pricerSwap = new DiscountingSwapProductPricer(pricerLeg);
		ImmutableRatesProvider prov = ImmutableRatesProvider.builder(VAL_DATE_INFLATION).discountCurves(RATES_GBP_INFLATION.DiscountCurves).priceIndexCurve(GB_RPI, PRICE_CURVE).timeSeries(GB_RPI, TS_INFLATION).build();
		MultiCurrencyAmount fvComputed = pricerSwap.forecastValue(SWAP_INFLATION, prov);
		double fixedRate = INFLATION_FIXED_SWAP_LEG_PAY_GBP_FIXED_RATE;
		double fvExpected = (-(INDEX_VALUES[0] / START_INDEX - 1.0) + Math.Pow(1.0 + fixedRate, 5) - 1.0) * NOTIONAL;
		assertTrue(fvComputed.Currencies.size() == 1);
		assertEquals(fvComputed.getAmount(GBP).Amount, fvExpected, NOTIONAL * TOLERANCE_RATE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_accruedInterest_firstAccrualPeriod()
	  {
		RatesProvider prov = new MockRatesProvider(IBOR_RATE_PAYMENT_PERIOD_REC_GBP.StartDate.plusDays(7));
		SwapPaymentPeriodPricer<SwapPaymentPeriod> mockPeriod = mock(typeof(SwapPaymentPeriodPricer));
		when(mockPeriod.accruedInterest(IBOR_RATE_PAYMENT_PERIOD_REC_GBP, prov)).thenReturn(1000d);
		when(mockPeriod.accruedInterest(FIXED_RATE_PAYMENT_PERIOD_PAY_GBP, prov)).thenReturn(-500d);
		SwapPaymentEventPricer<SwapPaymentEvent> mockEvent = mock(typeof(SwapPaymentEventPricer));
		DiscountingSwapLegPricer pricerLeg = new DiscountingSwapLegPricer(mockPeriod, mockEvent);
		DiscountingSwapProductPricer pricerSwap = new DiscountingSwapProductPricer(pricerLeg);
		assertEquals(pricerSwap.accruedInterest(SWAP, prov), MultiCurrencyAmount.of(GBP, 500d));

		// test via SwapTrade
		DiscountingSwapTradePricer pricerTrade = new DiscountingSwapTradePricer(pricerSwap);
		assertEquals(pricerTrade.accruedInterest(SWAP_TRADE, MOCK_PROV), pricerSwap.accruedInterest(SWAP, MOCK_PROV));
	  }

	  public virtual void test_accruedInterest_valDateBeforePeriod()
	  {
		RatesProvider prov = new MockRatesProvider(IBOR_RATE_PAYMENT_PERIOD_REC_GBP.StartDate);
		SwapPaymentPeriodPricer<SwapPaymentPeriod> mockPeriod = mock(typeof(SwapPaymentPeriodPricer));
		SwapPaymentEventPricer<SwapPaymentEvent> mockEvent = mock(typeof(SwapPaymentEventPricer));
		DiscountingSwapLegPricer pricerLeg = new DiscountingSwapLegPricer(mockPeriod, mockEvent);
		DiscountingSwapProductPricer pricerSwap = new DiscountingSwapProductPricer(pricerLeg);
		assertEquals(pricerSwap.accruedInterest(SWAP, prov), MultiCurrencyAmount.of(GBP, 0d));
	  }

	  public virtual void test_accruedInterest_valDateAfterPeriod()
	  {
		RatesProvider prov = new MockRatesProvider(IBOR_RATE_PAYMENT_PERIOD_REC_GBP.EndDate.plusDays(1));
		SwapPaymentPeriodPricer<SwapPaymentPeriod> mockPeriod = mock(typeof(SwapPaymentPeriodPricer));
		SwapPaymentEventPricer<SwapPaymentEvent> mockEvent = mock(typeof(SwapPaymentEventPricer));
		DiscountingSwapLegPricer pricerLeg = new DiscountingSwapLegPricer(mockPeriod, mockEvent);
		DiscountingSwapProductPricer pricerSwap = new DiscountingSwapProductPricer(pricerLeg);
		assertEquals(pricerSwap.accruedInterest(SWAP, prov), MultiCurrencyAmount.of(GBP, 0d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parRateSensitivity_singleCurrency()
	  {
		PointSensitivities point = SWAP_PRODUCT_PRICER.parRateSensitivity(SWAP, RATES_GBP).build();
		CurrencyParameterSensitivities prAd = RATES_GBP.parameterSensitivity(point);
		CurrencyParameterSensitivities prFd = FINITE_DIFFERENCE_CALCULATOR.sensitivity(RATES_GBP, p => CurrencyAmount.of(GBP, SWAP_PRODUCT_PRICER.parRate(SWAP, p)));
		assertTrue(prAd.equalWithTolerance(prFd, TOLERANCE_RATE_DELTA));

		// test via SwapTrade
		DiscountingSwapTradePricer pricerTrade = DiscountingSwapTradePricer.DEFAULT;
		DiscountingSwapProductPricer pricerSwap = DiscountingSwapProductPricer.DEFAULT;
		assertEquals(pricerTrade.parRateSensitivity(SWAP_TRADE, RATES_GBP), pricerSwap.parRateSensitivity(SWAP, RATES_GBP).build());
	  }

	  public virtual void test_parRateSensitivity_crossCurrency()
	  {
		PointSensitivities point = SWAP_PRODUCT_PRICER.parRateSensitivity(SWAP_CROSS_CURRENCY, RATES_GBP_USD).build();
		CurrencyParameterSensitivities prAd = RATES_GBP_USD.parameterSensitivity(point);
		CurrencyParameterSensitivities prFd = FINITE_DIFFERENCE_CALCULATOR.sensitivity(RATES_GBP_USD, p => CurrencyAmount.of(USD, SWAP_PRODUCT_PRICER.parRate(SWAP_CROSS_CURRENCY, p)));
		assertTrue(prAd.equalWithTolerance(prFd, TOLERANCE_RATE_DELTA));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity()
	  {
		// ibor leg
		IborRateSensitivity fwdSense = IborRateSensitivity.of(IBOR_RATE_COMP.Observation, GBP, 140.0);
		ZeroRateSensitivity dscSense = ZeroRateSensitivity.of(GBP, 3d, -162.0);
		PointSensitivityBuilder sensiFloating = fwdSense.combinedWith(dscSense);
		// fixed leg
		PointSensitivityBuilder sensiFixed = ZeroRateSensitivity.of(GBP, 3d, 152.0);
		// events
		Currency ccy = IBOR_SWAP_LEG_REC_GBP.Currency;
		PointSensitivityBuilder sensiEvent = ZeroRateSensitivity.of(ccy, 4d, -134.0);
		PointSensitivities expected = sensiFloating.build().combinedWith(sensiEvent.build()).combinedWith(sensiFixed.build()).combinedWith(sensiEvent.build());

		SwapPaymentPeriodPricer<SwapPaymentPeriod> mockPeriod = mock(typeof(SwapPaymentPeriodPricer));
		SwapPaymentEventPricer<SwapPaymentEvent> mockEvent = mock(typeof(SwapPaymentEventPricer));
		when(mockPeriod.presentValueSensitivity(IBOR_SWAP_LEG_REC_GBP.PaymentPeriods.get(0), MOCK_PROV)).thenAnswer(t => sensiFloating.build().toMutable());
		when(mockPeriod.presentValueSensitivity(FIXED_SWAP_LEG_PAY.PaymentPeriods.get(0), MOCK_PROV)).thenAnswer(t => sensiFixed.build().toMutable());
		when(mockEvent.presentValueSensitivity(IBOR_SWAP_LEG_REC_GBP.PaymentEvents.get(0), MOCK_PROV)).thenAnswer(t => sensiEvent.build().toMutable());
		when(mockEvent.presentValueSensitivity(FIXED_SWAP_LEG_PAY.PaymentEvents.get(0), MOCK_PROV)).thenAnswer(t => sensiEvent.build().toMutable());
		DiscountingSwapLegPricer pricerLeg = new DiscountingSwapLegPricer(mockPeriod, mockEvent);
		DiscountingSwapProductPricer pricerSwap = new DiscountingSwapProductPricer(pricerLeg);
		PointSensitivities res = pricerSwap.presentValueSensitivity(SWAP, MOCK_PROV).build();

		assertTrue(res.equalWithTolerance(expected, TOLERANCE_RATE));

		// test via SwapTrade
		DiscountingSwapTradePricer pricerTrade = new DiscountingSwapTradePricer(pricerSwap);
		assertEquals(pricerTrade.presentValueSensitivity(SWAP_TRADE, MOCK_PROV), pricerSwap.presentValueSensitivity(SWAP, MOCK_PROV).build());
	  }

	  public virtual void test_presentValueSensitivity_inflation()
	  {
		DiscountingSwapLegPricer pricerLeg = DiscountingSwapLegPricer.DEFAULT;
		DiscountingSwapProductPricer pricerSwap = new DiscountingSwapProductPricer(pricerLeg);
		ImmutableRatesProvider prov = ImmutableRatesProvider.builder(VAL_DATE_INFLATION).discountCurves(RATES_GBP_INFLATION.DiscountCurves).priceIndexCurve(GB_RPI, PRICE_CURVE).timeSeries(GB_RPI, TS_INFLATION).build();
		PointSensitivityBuilder pvSensiComputed = pricerSwap.presentValueSensitivity(SWAP_INFLATION, prov);
		PointSensitivityBuilder pvSensiInflationLeg = pricerLeg.presentValueSensitivity(INFLATION_MONTHLY_SWAP_LEG_REC_GBP, prov);
		PointSensitivityBuilder pvSensiFixedLeg = pricerLeg.presentValueSensitivity(INFLATION_FIXED_SWAP_LEG_PAY_GBP, prov);
		PointSensitivityBuilder pvSensiExpected = pvSensiFixedLeg.combinedWith(pvSensiInflationLeg);
		assertTrue(pvSensiComputed.build().normalized().equalWithTolerance(pvSensiExpected.build().normalized(), TOLERANCE_RATE * NOTIONAL));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_forecastValueSensitivity()
	  {
		// ibor leg
		PointSensitivityBuilder sensiFloating = IborRateSensitivity.of(IBOR_RATE_COMP.Observation, GBP, 140.0);
		// fixed leg
		PointSensitivityBuilder sensiFixed = PointSensitivityBuilder.none();
		// events
		PointSensitivityBuilder sensiEvent = PointSensitivityBuilder.none();
		PointSensitivities expected = sensiFloating.build();

		SwapPaymentPeriodPricer<SwapPaymentPeriod> mockPeriod = mock(typeof(SwapPaymentPeriodPricer));
		SwapPaymentEventPricer<SwapPaymentEvent> mockEvent = mock(typeof(SwapPaymentEventPricer));
		when(mockPeriod.forecastValueSensitivity(IBOR_SWAP_LEG_REC_GBP.PaymentPeriods.get(0), MOCK_PROV)).thenAnswer(t => sensiFloating.build().toMutable());
		when(mockPeriod.forecastValueSensitivity(FIXED_SWAP_LEG_PAY.PaymentPeriods.get(0), MOCK_PROV)).thenAnswer(t => sensiFixed.build().toMutable());
		when(mockEvent.forecastValueSensitivity(IBOR_SWAP_LEG_REC_GBP.PaymentEvents.get(0), MOCK_PROV)).thenAnswer(t => sensiEvent.build().toMutable());
		when(mockEvent.forecastValueSensitivity(FIXED_SWAP_LEG_PAY.PaymentEvents.get(0), MOCK_PROV)).thenAnswer(t => sensiEvent.build().toMutable());
		DiscountingSwapLegPricer pricerLeg = new DiscountingSwapLegPricer(mockPeriod, mockEvent);
		DiscountingSwapProductPricer pricerSwap = new DiscountingSwapProductPricer(pricerLeg);
		PointSensitivities res = pricerSwap.forecastValueSensitivity(SWAP, MOCK_PROV).build();

		assertTrue(res.equalWithTolerance(expected, TOLERANCE_RATE));

		// test via SwapTrade
		DiscountingSwapTradePricer pricerTrade = new DiscountingSwapTradePricer(pricerSwap);
		assertEquals(pricerTrade.forecastValueSensitivity(SWAP_TRADE, MOCK_PROV), pricerSwap.forecastValueSensitivity(SWAP, MOCK_PROV).build());
	  }

	  public virtual void test_forecastValueSensitivity_inflation()
	  {
		DiscountingSwapLegPricer pricerLeg = DiscountingSwapLegPricer.DEFAULT;
		DiscountingSwapProductPricer pricerSwap = new DiscountingSwapProductPricer(pricerLeg);
		ImmutableRatesProvider prov = ImmutableRatesProvider.builder(VAL_DATE_INFLATION).discountCurves(RATES_GBP_INFLATION.DiscountCurves).priceIndexCurve(GB_RPI, PRICE_CURVE).timeSeries(GB_RPI, TS_INFLATION).build();
		PointSensitivityBuilder fvSensiComputed = pricerSwap.forecastValueSensitivity(SWAP_INFLATION, prov);
		PointSensitivityBuilder fvSensiInflationLeg = pricerLeg.forecastValueSensitivity(INFLATION_MONTHLY_SWAP_LEG_REC_GBP, prov);
		PointSensitivityBuilder fvSensiFixedLeg = pricerLeg.forecastValueSensitivity(INFLATION_FIXED_SWAP_LEG_PAY_GBP, prov);
		PointSensitivityBuilder fvSensiExpected = fvSensiFixedLeg.combinedWith(fvSensiInflationLeg);
		assertTrue(fvSensiComputed.build().normalized().equalWithTolerance(fvSensiExpected.build().normalized(), TOLERANCE_RATE * NOTIONAL));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_cashFlows()
	  {
		RatesProvider mockProv = mock(typeof(RatesProvider));
		SwapPaymentPeriodPricer<SwapPaymentPeriod> mockPeriod = mock(typeof(SwapPaymentPeriodPricer));
		double df1 = 0.98;
		double df2 = 0.93;
		double fvGBP = 1000d;
		double fvUSD = -500d;
		when(mockPeriod.forecastValue(IBOR_RATE_PAYMENT_PERIOD_REC_GBP, mockProv)).thenReturn(fvGBP);
		when(mockPeriod.forecastValue(FIXED_RATE_PAYMENT_PERIOD_PAY_USD, mockProv)).thenReturn(fvUSD);
		when(mockProv.ValuationDate).thenReturn(LocalDate.of(2014, 7, 1));
		when(mockProv.discountFactor(IBOR_RATE_PAYMENT_PERIOD_REC_GBP.Currency, IBOR_RATE_PAYMENT_PERIOD_REC_GBP.PaymentDate)).thenReturn(df1);
		when(mockProv.discountFactor(FIXED_RATE_PAYMENT_PERIOD_PAY_USD.Currency, FIXED_RATE_PAYMENT_PERIOD_PAY_USD.PaymentDate)).thenReturn(df2);
		SwapPaymentEventPricer<SwapPaymentEvent> mockEvent = mock(typeof(SwapPaymentEventPricer));
		DiscountingSwapLegPricer pricerLeg = new DiscountingSwapLegPricer(mockPeriod, mockEvent);
		DiscountingSwapProductPricer pricerSwap = new DiscountingSwapProductPricer(pricerLeg);

		CashFlows computed = pricerSwap.cashFlows(SWAP_CROSS_CURRENCY, mockProv);
		CashFlow flowGBP = CashFlow.ofForecastValue(IBOR_RATE_PAYMENT_PERIOD_REC_GBP.PaymentDate, GBP, fvGBP, df1);
		CashFlow flowUSD = CashFlow.ofForecastValue(FIXED_RATE_PAYMENT_PERIOD_PAY_USD.PaymentDate, USD, fvUSD, df2);
		CashFlows expected = CashFlows.of(ImmutableList.of(flowGBP, flowUSD));
		assertEquals(computed, expected);

		// test via SwapTrade
		DiscountingSwapTradePricer pricerTrade = new DiscountingSwapTradePricer(pricerSwap);
		assertEquals(pricerTrade.cashFlows(SWAP_TRADE, MOCK_PROV), pricerSwap.cashFlows(SWAP, MOCK_PROV));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_explainPresentValue_singleCurrency()
	  {
		SwapPaymentPeriodPricer<SwapPaymentPeriod> mockPeriod = mock(typeof(SwapPaymentPeriodPricer));
		when(mockPeriod.presentValue(IBOR_RATE_PAYMENT_PERIOD_REC_GBP, MOCK_PROV)).thenReturn(1000d);
		when(mockPeriod.presentValue(FIXED_RATE_PAYMENT_PERIOD_PAY_GBP, MOCK_PROV)).thenReturn(-500d);
		SwapPaymentEventPricer<SwapPaymentEvent> mockEvent = mock(typeof(SwapPaymentEventPricer));
		when(mockEvent.presentValue(NOTIONAL_EXCHANGE_REC_GBP, MOCK_PROV)).thenReturn(35d);
		when(mockEvent.presentValue(NOTIONAL_EXCHANGE_PAY_GBP, MOCK_PROV)).thenReturn(-30d);

		DiscountingSwapLegPricer pricerLeg = new DiscountingSwapLegPricer(mockPeriod, mockEvent);
		DiscountingSwapProductPricer pricerSwap = new DiscountingSwapProductPricer(pricerLeg);
		assertEquals(pricerSwap.presentValue(SWAP, MOCK_PROV), MultiCurrencyAmount.of(GBP, 505d));

		ExplainMap explain = pricerSwap.explainPresentValue(SWAP, MOCK_PROV);
		assertEquals(explain.get(ExplainKey.ENTRY_TYPE).get(), "Swap");

		assertEquals(explain.get(ExplainKey.LEGS).get().size(), 2);
		ExplainMap explainLeg0 = explain.get(ExplainKey.LEGS).get().get(0);
		ResolvedSwapLeg leg0 = SWAP.Legs.get(0);
		double fv0 = pricerLeg.forecastValue(leg0, MOCK_PROV).Amount;
		assertEquals(explainLeg0.get(ExplainKey.ENTRY_TYPE).get(), "Leg");
		assertEquals(explainLeg0.get(ExplainKey.ENTRY_INDEX).Value.intValue(), 0);
		assertEquals(explainLeg0.get(ExplainKey.PAY_RECEIVE).get(), leg0.PayReceive);
		assertEquals(explainLeg0.get(ExplainKey.LEG_TYPE).get(), leg0.Type.ToString());
		assertEquals(explainLeg0.get(ExplainKey.PAYMENT_PERIODS).get().size(), 1);
		assertEquals(explainLeg0.get(ExplainKey.PAYMENT_EVENTS).get().size(), 1);
		assertEquals(explainLeg0.get(ExplainKey.FORECAST_VALUE).get().Currency, leg0.Currency);
		assertEquals(explainLeg0.get(ExplainKey.FORECAST_VALUE).get().Amount, fv0, TOLERANCE_RATE);
		ExplainMap explainLeg1 = explain.get(ExplainKey.LEGS).get().get(1);
		ResolvedSwapLeg leg1 = SWAP.Legs.get(0);
		double fv1 = pricerLeg.forecastValue(leg1, MOCK_PROV).Amount;
		assertEquals(explainLeg1.get(ExplainKey.ENTRY_TYPE).get(), "Leg");
		assertEquals(explainLeg1.get(ExplainKey.ENTRY_INDEX).Value.intValue(), 1);
		assertEquals(explainLeg1.get(ExplainKey.PAYMENT_PERIODS).get().size(), 1);
		assertEquals(explainLeg1.get(ExplainKey.PAYMENT_EVENTS).get().size(), 1);
		assertEquals(explainLeg1.get(ExplainKey.FORECAST_VALUE).get().Currency, leg1.Currency);
		assertEquals(explainLeg1.get(ExplainKey.FORECAST_VALUE).get().Amount, fv1, TOLERANCE_RATE);

		// test via SwapTrade
		DiscountingSwapTradePricer pricerTrade = new DiscountingSwapTradePricer(pricerSwap);
		assertEquals(pricerTrade.explainPresentValue(SWAP_TRADE, MOCK_PROV), pricerSwap.explainPresentValue(SWAP, MOCK_PROV));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parSpread_fixedIbor()
	  {
		ResolvedSwapTrade swapTrade = SWAP_USD_FIXED_6M_LIBOR_3M_5Y.resolve(REF_DATA);
		double ps = SWAP_PRODUCT_PRICER.parSpread(swapTrade.Product, MULTI_USD);
		SwapTrade swap0 = FixedIborSwapTemplate.of(Period.ZERO, TENOR_5Y, USD_FIXED_6M_LIBOR_3M).createTrade(MULTI_USD.ValuationDate, BUY, NOTIONAL_SWAP, FIXED_RATE + ps, REF_DATA);
		CurrencyAmount pv0 = SWAP_PRODUCT_PRICER.presentValue(swap0.Product.resolve(REF_DATA), USD, MULTI_USD);
		assertEquals(pv0.Amount, 0, TOLERANCE_PV);

		// test via SwapTrade
		DiscountingSwapProductPricer pricerSwap = DiscountingSwapProductPricer.DEFAULT;
		DiscountingSwapTradePricer pricerTrade = DiscountingSwapTradePricer.DEFAULT;
		assertEquals(pricerTrade.parSpread(swapTrade, MULTI_USD), pricerSwap.parSpread(swapTrade.Product, MULTI_USD));
	  }

	  public virtual void test_parSpread_fixedInflation()
	  {
		ResolvedSwapTrade tradeZc = SWAP_GBP_ZC_INFLATION_5Y.resolve(REF_DATA);
		double ps = SWAP_PRODUCT_PRICER.parSpread(tradeZc.Product, RATES_GBP_INFLATION);
		SwapTrade swap0 = FixedInflationSwapConventions.GBP_FIXED_ZC_GB_RPI.createTrade(VAL_DATE_INFLATION, TENOR_5Y, BUY, NOTIONAL_SWAP, FIXED_RATE + ps, REF_DATA);
		CurrencyAmount pv0 = SWAP_PRODUCT_PRICER.presentValue(swap0.Product.resolve(REF_DATA), GBP, RATES_GBP_INFLATION);
		assertEquals(pv0.Amount, 0, TOLERANCE_PV);
	  }

	  public virtual void test_parSpread_iborIbor()
	  {
		double ps = SWAP_PRODUCT_PRICER.parSpread(SWAP_USD_LIBOR_3M_LIBOR_6M_5Y.Product.resolve(REF_DATA), MULTI_USD);
		SwapTrade swap0 = IborIborSwapTemplate.of(Period.ZERO, TENOR_5Y, CONV_USD_LIBOR3M_LIBOR6M).createTrade(MULTI_USD.ValuationDate, BUY, NOTIONAL_SWAP, SPREAD + ps, REF_DATA);
		CurrencyAmount pv0 = SWAP_PRODUCT_PRICER.presentValue(swap0.Product.resolve(REF_DATA), USD, MULTI_USD);
		assertEquals(pv0.Amount, 0, TOLERANCE_PV);
	  }

	  public virtual void test_parSpread_iborCmpIbor()
	  {
		SwapTrade trade = USD_LIBOR_3M_LIBOR_6M.createTrade(MULTI_USD.ValuationDate, TENOR_5Y, BUY, NOTIONAL_SWAP, SPREAD, REF_DATA);
		double ps = SWAP_PRODUCT_PRICER.parSpread(trade.Product.resolve(REF_DATA), MULTI_USD);
		SwapTrade swap0 = USD_LIBOR_3M_LIBOR_6M.createTrade(MULTI_USD.ValuationDate, TENOR_5Y, BUY, NOTIONAL_SWAP, SPREAD + ps, REF_DATA);
		CurrencyAmount pv0 = SWAP_PRODUCT_PRICER.presentValue(swap0.Product.resolve(REF_DATA), USD, MULTI_USD);
		assertEquals(pv0.Amount, 0, TOLERANCE_PV);
	  }

	  /* Test par spread for IBOR swaps with compounding and a unique payment period. */
	  public virtual void test_parSpread_iborCmpIbor_1period()
	  {
		SwapTrade trade = USD_LIBOR_3M_LIBOR_6M.createTrade(MULTI_USD.ValuationDate, Tenor.TENOR_6M, BUY, NOTIONAL_SWAP, SPREAD, REF_DATA);
		double ps = SWAP_PRODUCT_PRICER.parSpread(trade.Product.resolve(REF_DATA), MULTI_USD);
		SwapTrade swap0 = USD_LIBOR_3M_LIBOR_6M.createTrade(MULTI_USD.ValuationDate, Tenor.TENOR_6M, BUY, NOTIONAL_SWAP, SPREAD + ps, REF_DATA);
		CurrencyAmount pv0 = SWAP_PRODUCT_PRICER.presentValue(swap0.Product.resolve(REF_DATA), USD, MULTI_USD);
		assertEquals(pv0.Amount, 0, TOLERANCE_PV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parSpreadSensitivity_fixedIbor()
	  {
		ResolvedSwapTrade trade = SWAP_USD_FIXED_6M_LIBOR_3M_5Y.resolve(REF_DATA);
		PointSensitivities point = SWAP_PRODUCT_PRICER.parSpreadSensitivity(trade.Product, MULTI_USD).build();
		CurrencyParameterSensitivities prAd = MULTI_USD.parameterSensitivity(point);
		CurrencyParameterSensitivities prFd = FINITE_DIFFERENCE_CALCULATOR.sensitivity(MULTI_USD, p => CurrencyAmount.of(USD, SWAP_PRODUCT_PRICER.parSpread(trade.Product, p)));
		assertTrue(prAd.equalWithTolerance(prFd, TOLERANCE_RATE_DELTA));

		// test via SwapTrade
		DiscountingSwapTradePricer pricerTrade = DiscountingSwapTradePricer.DEFAULT;
		DiscountingSwapProductPricer pricerSwap = DiscountingSwapProductPricer.DEFAULT;
		assertEquals(pricerTrade.parSpreadSensitivity(trade, MULTI_USD), pricerSwap.parSpreadSensitivity(trade.Product, MULTI_USD).build());
	  }

	  public virtual void test_parSpreadSensitivity_fixedInflation()
	  {
		ResolvedSwapTrade tradeZc = SWAP_GBP_ZC_INFLATION_5Y.resolve(REF_DATA);
		PointSensitivities point = SWAP_PRODUCT_PRICER.parSpreadSensitivity(tradeZc.Product, RATES_GBP_INFLATION).build();
		CurrencyParameterSensitivities prAd = RATES_GBP_INFLATION.parameterSensitivity(point);
		CurrencyParameterSensitivities prFd = FINITE_DIFFERENCE_CALCULATOR.sensitivity(RATES_GBP_INFLATION, p => CurrencyAmount.of(GBP, SWAP_PRODUCT_PRICER.parSpread(tradeZc.Product, p)));
		assertTrue(prAd.equalWithTolerance(prFd, TOLERANCE_RATE_DELTA));

		// trade v product
		DiscountingSwapTradePricer pricerTrade = DiscountingSwapTradePricer.DEFAULT;
		DiscountingSwapProductPricer pricerSwap = DiscountingSwapProductPricer.DEFAULT;
		assertEquals(pricerTrade.parSpreadSensitivity(tradeZc, RATES_GBP_INFLATION), pricerSwap.parSpreadSensitivity(tradeZc.Product, RATES_GBP_INFLATION).build());
	  }

	  public virtual void test_parSpreadSensitivity_iborIbor()
	  {
		ResolvedSwap expanded = SWAP_USD_LIBOR_3M_LIBOR_6M_5Y.Product.resolve(REF_DATA);
		PointSensitivities point = SWAP_PRODUCT_PRICER.parSpreadSensitivity(expanded, MULTI_USD).build();
		CurrencyParameterSensitivities prAd = MULTI_USD.parameterSensitivity(point);
		CurrencyParameterSensitivities prFd = FINITE_DIFFERENCE_CALCULATOR.sensitivity(MULTI_USD, p => CurrencyAmount.of(USD, SWAP_PRODUCT_PRICER.parSpread(expanded, p)));
		assertTrue(prAd.equalWithTolerance(prFd, TOLERANCE_RATE_DELTA));
	  }

	  public virtual void test_parSpreadSensitivity_iborCmpIbor()
	  {
		ResolvedSwap swap = USD_LIBOR_3M_LIBOR_6M.createTrade(MULTI_USD.ValuationDate, TENOR_5Y, BUY, NOTIONAL_SWAP, SPREAD, REF_DATA).resolve(REF_DATA).Product;
		PointSensitivities point = SWAP_PRODUCT_PRICER.parSpreadSensitivity(swap, MULTI_USD).build();
		CurrencyParameterSensitivities prAd = MULTI_USD.parameterSensitivity(point);
		CurrencyParameterSensitivities prFd = FINITE_DIFFERENCE_CALCULATOR.sensitivity(MULTI_USD, p => CurrencyAmount.of(USD, SWAP_PRODUCT_PRICER.parSpread(swap, p)));
		assertTrue(prAd.equalWithTolerance(prFd, TOLERANCE_RATE_DELTA));
	  }

	  public virtual void test_parSpreadSensitivity_iborCmpIbor_1Period()
	  {
		ResolvedSwap swap = USD_LIBOR_3M_LIBOR_6M.createTrade(MULTI_USD.ValuationDate, Tenor.TENOR_6M, BUY, NOTIONAL_SWAP, SPREAD, REF_DATA).resolve(REF_DATA).Product;
		PointSensitivities point = SWAP_PRODUCT_PRICER.parSpreadSensitivity(swap, MULTI_USD).build();
		CurrencyParameterSensitivities prAd = MULTI_USD.parameterSensitivity(point);
		CurrencyParameterSensitivities prFd = FINITE_DIFFERENCE_CALCULATOR.sensitivity(MULTI_USD, p => CurrencyAmount.of(USD, SWAP_PRODUCT_PRICER.parSpread(swap, p)));
		assertTrue(prAd.equalWithTolerance(prFd, TOLERANCE_RATE_DELTA));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currencyExposure_singleCurrency()
	  {
		PointSensitivities point = SWAP_PRODUCT_PRICER.parRateSensitivity(SWAP, RATES_GBP).build();
		MultiCurrencyAmount expected = RATES_GBP.currencyExposure(point).plus(SWAP_PRODUCT_PRICER.presentValue(SWAP, RATES_GBP));
		MultiCurrencyAmount computed = SWAP_PRODUCT_PRICER.currencyExposure(SWAP, RATES_GBP);
		assertEquals(computed, expected);
		MultiCurrencyAmount fromTrade = SWAP_TRADE_PRICER.currencyExposure(SWAP_TRADE, RATES_GBP);
		assertEquals(fromTrade, computed);
	  }

	  public virtual void test_currencyExposure_crossCurrency()
	  {
		PointSensitivities point = SWAP_PRODUCT_PRICER.parRateSensitivity(SWAP_CROSS_CURRENCY, RATES_GBP_USD).build();
		MultiCurrencyAmount expected = RATES_GBP_USD.currencyExposure(point).plus(SWAP_PRODUCT_PRICER.presentValue(SWAP_CROSS_CURRENCY, RATES_GBP_USD));
		MultiCurrencyAmount computed = SWAP_PRODUCT_PRICER.currencyExposure(SWAP_CROSS_CURRENCY, RATES_GBP_USD);
		assertEquals(computed, expected);
		MultiCurrencyAmount fromTrade = SWAP_TRADE_PRICER.currencyExposure(SWAP_TRADE_CROSS_CURRENCY, RATES_GBP_USD);
		assertEquals(fromTrade, computed);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currentCash_zero()
	  {
		MultiCurrencyAmount computed = SWAP_PRODUCT_PRICER.currentCash(SWAP_CROSS_CURRENCY, RATES_GBP_USD);
		assertEquals(computed, MultiCurrencyAmount.of(CurrencyAmount.zero(GBP), CurrencyAmount.zero(USD)));
		MultiCurrencyAmount fromTrade = SWAP_TRADE_PRICER.currentCash(SWAP_TRADE_CROSS_CURRENCY, RATES_GBP_USD);
		assertEquals(fromTrade, computed);
	  }

	  public virtual void test_currentCash_onPayment()
	  {
		ResolvedSwapTrade trade = GBP_FIXED_1Y_LIBOR_3M.createTrade(MULTI_USD.ValuationDate, TENOR_5Y, BUY, NOTIONAL_SWAP, SPREAD, REF_DATA).resolve(REF_DATA);
		ResolvedSwap expanded = trade.Product;
		LocalDate payDate = expanded.Legs.get(0).PaymentPeriods.get(2).PaymentDate;
		ImmutableRatesProvider prov = RatesProviderDataSets.multiGbp(payDate).toBuilder().timeSeries(GBP_LIBOR_3M, LocalDateDoubleTimeSeries.of(LocalDate.of(2016, 10, 24), 0.003)).build();
		MultiCurrencyAmount computed = SWAP_PRODUCT_PRICER.currentCash(expanded, prov);
		MultiCurrencyAmount expected = MultiCurrencyAmount.of(SWAP_PRODUCT_PRICER.LegPricer.currentCash(expanded.Legs.get(0), prov).plus(SWAP_PRODUCT_PRICER.LegPricer.currentCash(expanded.Legs.get(1), prov)));
		assertEquals(computed, expected);
		MultiCurrencyAmount fromTrade = SWAP_TRADE_PRICER.currentCash(trade, prov);
		assertEquals(fromTrade, computed);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void three_leg_swap()
	  {
		ThreeLegBasisSwapConvention conv = ThreeLegBasisSwapConventions.EUR_FIXED_1Y_EURIBOR_3M_EURIBOR_6M;
		LocalDate tradeDate = LocalDate.of(2014, 1, 22);
		ResolvedSwap swap = conv.createTrade(tradeDate, Period.ofMonths(1), TENOR_5Y, BUY, NOTIONAL_SWAP, SPREAD, REF_DATA).Product.resolve(REF_DATA);
		// pv
		MultiCurrencyAmount pvComputed = SWAP_PRODUCT_PRICER.presentValue(swap, MULTI_EUR);
		DiscountingSwapLegPricer legPricer = SWAP_PRODUCT_PRICER.LegPricer;
		CurrencyAmount pvExpected = legPricer.presentValue(swap.Legs.get(0), MULTI_EUR).plus(legPricer.presentValue(swap.Legs.get(1), MULTI_EUR)).plus(legPricer.presentValue(swap.Legs.get(2), MULTI_EUR));
		assertEquals(pvComputed.getAmount(EUR), pvExpected);
		// pv sensitivity
		PointSensitivityBuilder pvPointComputed = SWAP_PRODUCT_PRICER.presentValueSensitivity(swap, MULTI_EUR);
		PointSensitivityBuilder pvPointExpected = legPricer.presentValueSensitivity(swap.Legs.get(0), MULTI_EUR).combinedWith(legPricer.presentValueSensitivity(swap.Legs.get(1), MULTI_EUR)).combinedWith(legPricer.presentValueSensitivity(swap.Legs.get(2), MULTI_EUR));
		assertEquals(pvPointComputed, pvPointExpected);
		// par rate
		double parRate = SWAP_PRODUCT_PRICER.parRate(swap, MULTI_EUR);
		ResolvedSwap swapParRate = conv.createTrade(tradeDate, Period.ofMonths(1), TENOR_5Y, BUY, NOTIONAL_SWAP, parRate, REF_DATA).Product.resolve(REF_DATA);
		MultiCurrencyAmount pvParRate = SWAP_PRODUCT_PRICER.presentValue(swapParRate, MULTI_EUR);
		assertEquals(pvParRate, MultiCurrencyAmount.of(EUR, 0d));
		// par rate sensitivity
		PointSensitivities parRatePoint = SWAP_PRODUCT_PRICER.parRateSensitivity(swap, MULTI_EUR).build();
		CurrencyParameterSensitivities parRateSensiComputed = MULTI_EUR.parameterSensitivity(parRatePoint);
		CurrencyParameterSensitivities parRateSensiExpected = FINITE_DIFFERENCE_CALCULATOR.sensitivity(MULTI_EUR, p => CurrencyAmount.of(EUR, SWAP_PRODUCT_PRICER.parRate(swap, p)));
		assertTrue(parRateSensiComputed.equalWithTolerance(parRateSensiExpected, TOLERANCE_RATE_DELTA));
		// par spread
		double parSpread = SWAP_PRODUCT_PRICER.parSpread(swap, MULTI_EUR);
		ResolvedSwap swapParSpread = conv.createTrade(tradeDate, Period.ofMonths(1), TENOR_5Y, BUY, NOTIONAL_SWAP, SPREAD + parSpread, REF_DATA).Product.resolve(REF_DATA);
		MultiCurrencyAmount pvParSpread = SWAP_PRODUCT_PRICER.presentValue(swapParSpread, MULTI_EUR);
		assertEquals(pvParSpread, MultiCurrencyAmount.of(EUR, 0d));
		// par spread sensitivity
		PointSensitivities parSpreadPoint = SWAP_PRODUCT_PRICER.parSpreadSensitivity(swap, MULTI_EUR).build();
		CurrencyParameterSensitivities parSpreadSensiComputed = MULTI_EUR.parameterSensitivity(parSpreadPoint);
		CurrencyParameterSensitivities parSpreadSensiExpected = FINITE_DIFFERENCE_CALCULATOR.sensitivity(MULTI_EUR, p => CurrencyAmount.of(EUR, SWAP_PRODUCT_PRICER.parSpread(swap, p)));
		assertTrue(parSpreadSensiComputed.equalWithTolerance(parSpreadSensiExpected, TOLERANCE_RATE_DELTA));
	  }

	}

}