using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.THIRTY_U_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P12M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.dateUtc;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.LongShort.LONG;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.LongShort.SHORT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using NormalDistribution = com.opengamma.strata.math.impl.statistics.distribution.NormalDistribution;
	using ProbabilityDistribution = com.opengamma.strata.math.impl.statistics.distribution.ProbabilityDistribution;
	using CashFlowEquivalentCalculator = com.opengamma.strata.pricer.impl.rate.swap.CashFlowEquivalentCalculator;
	using HullWhiteIborFutureDataSet = com.opengamma.strata.pricer.index.HullWhiteIborFutureDataSet;
	using HullWhiteOneFactorPiecewiseConstantParameters = com.opengamma.strata.pricer.model.HullWhiteOneFactorPiecewiseConstantParameters;
	using HullWhiteOneFactorPiecewiseConstantParametersProvider = com.opengamma.strata.pricer.model.HullWhiteOneFactorPiecewiseConstantParametersProvider;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using SwapPaymentEventPricer = com.opengamma.strata.pricer.swap.SwapPaymentEventPricer;
	using LongShort = com.opengamma.strata.product.common.LongShort;
	using FixedRateCalculation = com.opengamma.strata.product.swap.FixedRateCalculation;
	using IborRateCalculation = com.opengamma.strata.product.swap.IborRateCalculation;
	using NotionalSchedule = com.opengamma.strata.product.swap.NotionalSchedule;
	using PaymentSchedule = com.opengamma.strata.product.swap.PaymentSchedule;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;
	using SwapPaymentEvent = com.opengamma.strata.product.swap.SwapPaymentEvent;
	using CashSwaptionSettlement = com.opengamma.strata.product.swaption.CashSwaptionSettlement;
	using CashSwaptionSettlementMethod = com.opengamma.strata.product.swaption.CashSwaptionSettlementMethod;
	using PhysicalSwaptionSettlement = com.opengamma.strata.product.swaption.PhysicalSwaptionSettlement;
	using ResolvedSwaption = com.opengamma.strata.product.swaption.ResolvedSwaption;
	using Swaption = com.opengamma.strata.product.swaption.Swaption;

	/// <summary>
	/// Test <seealso cref="HullWhiteSwaptionPhysicalProductPricer"/>. 
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class HullWhiteSwaptionPhysicalProductPricerTest
	public class HullWhiteSwaptionPhysicalProductPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly ZonedDateTime MATURITY = dateUtc(2016, 7, 7);
	  private static readonly HolidayCalendarId CALENDAR = HolidayCalendarIds.SAT_SUN;
	  private static readonly BusinessDayAdjustment BDA_MF = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, CALENDAR);
	  private static readonly LocalDate SETTLE = BDA_MF.adjust(CALENDAR.resolve(REF_DATA).shift(MATURITY.toLocalDate(), 2), REF_DATA);
	  private const double NOTIONAL = 100000000; //100m
	  private const int TENOR_YEAR = 5;
	  private static readonly LocalDate END = SETTLE.plusYears(TENOR_YEAR);
	  private const double RATE = 0.0175;
	  private static readonly PeriodicSchedule PERIOD_FIXED = PeriodicSchedule.builder().startDate(SETTLE).endDate(END).frequency(P12M).businessDayAdjustment(BDA_MF).stubConvention(StubConvention.SHORT_FINAL).rollConvention(RollConventions.EOM).build();
	  private static readonly PaymentSchedule PAYMENT_FIXED = PaymentSchedule.builder().paymentFrequency(P12M).paymentDateOffset(DaysAdjustment.NONE).build();
	  private static readonly FixedRateCalculation RATE_FIXED = FixedRateCalculation.builder().dayCount(THIRTY_U_360).rate(ValueSchedule.of(RATE)).build();
	  private static readonly PeriodicSchedule PERIOD_IBOR = PeriodicSchedule.builder().startDate(SETTLE).endDate(END).frequency(P6M).businessDayAdjustment(BDA_MF).stubConvention(StubConvention.SHORT_FINAL).rollConvention(RollConventions.EOM).build();
	  private static readonly PaymentSchedule PAYMENT_IBOR = PaymentSchedule.builder().paymentFrequency(P6M).paymentDateOffset(DaysAdjustment.NONE).build();
	  private static readonly IborRateCalculation RATE_IBOR = IborRateCalculation.builder().index(EUR_EURIBOR_6M).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, CALENDAR, BDA_MF)).build();
	  private static readonly SwapLeg FIXED_LEG_REC = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PERIOD_FIXED).paymentSchedule(PAYMENT_FIXED).notionalSchedule(NotionalSchedule.of(EUR, NOTIONAL)).calculation(RATE_FIXED).build();
	  private static readonly SwapLeg FIXED_LEG_PAY = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PERIOD_FIXED).paymentSchedule(PAYMENT_FIXED).notionalSchedule(NotionalSchedule.of(EUR, NOTIONAL)).calculation(RATE_FIXED).build();
	  private static readonly SwapLeg IBOR_LEG_REC = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PERIOD_IBOR).paymentSchedule(PAYMENT_IBOR).notionalSchedule(NotionalSchedule.of(EUR, NOTIONAL)).calculation(RATE_IBOR).build();
	  private static readonly SwapLeg IBOR_LEG_PAY = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PERIOD_IBOR).paymentSchedule(PAYMENT_IBOR).notionalSchedule(NotionalSchedule.of(EUR, NOTIONAL)).calculation(RATE_IBOR).build();
	  private static readonly Swap SWAP_REC = Swap.of(FIXED_LEG_REC, IBOR_LEG_PAY);
	  private static readonly ResolvedSwap RSWAP_REC = SWAP_REC.resolve(REF_DATA);
	  private static readonly Swap SWAP_PAY = Swap.of(FIXED_LEG_PAY, IBOR_LEG_REC);
	  private static readonly ResolvedSwap RSWAP_PAY = SWAP_PAY.resolve(REF_DATA);
	  private static readonly CashSwaptionSettlement PAR_YIELD = CashSwaptionSettlement.of(SETTLE, CashSwaptionSettlementMethod.PAR_YIELD);
	  private static readonly ResolvedSwaption SWAPTION_REC_LONG = Swaption.builder().expiryDate(AdjustableDate.of(MATURITY.toLocalDate(), BDA_MF)).expiryTime(MATURITY.toLocalTime()).expiryZone(MATURITY.Zone).swaptionSettlement(PhysicalSwaptionSettlement.DEFAULT).longShort(LONG).underlying(SWAP_REC).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_REC_SHORT = Swaption.builder().expiryDate(AdjustableDate.of(MATURITY.toLocalDate(), BDA_MF)).expiryTime(MATURITY.toLocalTime()).expiryZone(MATURITY.Zone).swaptionSettlement(PhysicalSwaptionSettlement.DEFAULT).longShort(SHORT).underlying(SWAP_REC).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_PAY_LONG = Swaption.builder().expiryDate(AdjustableDate.of(MATURITY.toLocalDate(), BDA_MF)).expiryTime(MATURITY.toLocalTime()).expiryZone(MATURITY.Zone).swaptionSettlement(PhysicalSwaptionSettlement.DEFAULT).longShort(LONG).underlying(SWAP_PAY).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_PAY_SHORT = Swaption.builder().expiryDate(AdjustableDate.of(MATURITY.toLocalDate(), BDA_MF)).expiryTime(MATURITY.toLocalTime()).expiryZone(MATURITY.Zone).swaptionSettlement(PhysicalSwaptionSettlement.DEFAULT).longShort(SHORT).underlying(SWAP_PAY).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_CASH = Swaption.builder().expiryDate(AdjustableDate.of(MATURITY.toLocalDate())).expiryTime(MATURITY.toLocalTime()).expiryZone(MATURITY.Zone).longShort(LongShort.LONG).swaptionSettlement(PAR_YIELD).underlying(SWAP_REC).build().resolve(REF_DATA);

	  private static readonly LocalDate VALUATION = LocalDate.of(2011, 7, 7);
	  private static readonly HullWhiteOneFactorPiecewiseConstantParametersProvider HW_PROVIDER = HullWhiteIborFutureDataSet.createHullWhiteProvider(VALUATION);
	  private static readonly HullWhiteOneFactorPiecewiseConstantParametersProvider HW_PROVIDER_AT_MATURITY = HullWhiteIborFutureDataSet.createHullWhiteProvider(MATURITY.toLocalDate());
	  private static readonly HullWhiteOneFactorPiecewiseConstantParametersProvider HW_PROVIDER_AFTER_MATURITY = HullWhiteIborFutureDataSet.createHullWhiteProvider(MATURITY.toLocalDate().plusDays(1));
	  private static readonly ImmutableRatesProvider RATE_PROVIDER = HullWhiteIborFutureDataSet.createRatesProvider(VALUATION);
	  private static readonly ImmutableRatesProvider RATES_PROVIDER_AT_MATURITY = HullWhiteIborFutureDataSet.createRatesProvider(MATURITY.toLocalDate());
	  private static readonly ImmutableRatesProvider RATES_PROVIDER_AFTER_MATURITY = HullWhiteIborFutureDataSet.createRatesProvider(MATURITY.toLocalDate().plusDays(1));

	  private const double TOL = 1.0e-12;
	  private const double FD_TOL = 1.0e-7;
	  private static readonly HullWhiteSwaptionPhysicalProductPricer PRICER = HullWhiteSwaptionPhysicalProductPricer.DEFAULT;
	  private static readonly DiscountingSwapProductPricer SWAP_PRICER = DiscountingSwapProductPricer.DEFAULT;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FD_CAL = new RatesFiniteDifferenceSensitivityCalculator(FD_TOL);
	  private static readonly ProbabilityDistribution<double> NORMAL = new NormalDistribution(0, 1);

	  //-------------------------------------------------------------------------
	  public virtual void validate_physical_settlement()
	  {
		assertThrowsIllegalArg(() => PRICER.presentValue(SWAPTION_CASH, RATE_PROVIDER, HW_PROVIDER));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		CurrencyAmount computedRec = PRICER.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER, HW_PROVIDER);
		CurrencyAmount computedPay = PRICER.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER, HW_PROVIDER);
		SwapPaymentEventPricer<SwapPaymentEvent> paymentEventPricer = SwapPaymentEventPricer.standard();
		ResolvedSwapLeg cashFlowEquiv = CashFlowEquivalentCalculator.cashFlowEquivalentSwap(RSWAP_REC, RATE_PROVIDER);
		LocalDate expiryDate = MATURITY.toLocalDate();
		int nPayments = cashFlowEquiv.PaymentEvents.size();
		double[] alpha = new double[nPayments];
		double[] discountedCashFlow = new double[nPayments];
		for (int loopcf = 0; loopcf < nPayments; loopcf++)
		{
		  SwapPaymentEvent payment = cashFlowEquiv.PaymentEvents.get(loopcf);
		  alpha[loopcf] = HW_PROVIDER.alpha(RATE_PROVIDER.ValuationDate, expiryDate, expiryDate, payment.PaymentDate);
		  discountedCashFlow[loopcf] = paymentEventPricer.presentValue(payment, RATE_PROVIDER);
		}
		double omegaPay = -1d;
		double kappa = HW_PROVIDER.Model.kappa(DoubleArray.copyOf(discountedCashFlow), DoubleArray.copyOf(alpha));
		double expectedRec = 0.0;
		double expectedPay = 0.0;
		for (int loopcf = 0; loopcf < nPayments; loopcf++)
		{
		  expectedRec += discountedCashFlow[loopcf] * NORMAL.getCDF((kappa + alpha[loopcf]));
		  expectedPay += discountedCashFlow[loopcf] * NORMAL.getCDF(omegaPay * (kappa + alpha[loopcf]));
		}
		assertEquals(computedRec.Currency, EUR);
		assertEquals(computedRec.Amount, expectedRec, NOTIONAL * TOL);
		assertEquals(computedPay.Currency, EUR);
		assertEquals(computedPay.Amount, expectedPay, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_atMaturity()
	  {
		CurrencyAmount computedRec = PRICER.presentValue(SWAPTION_REC_LONG, RATES_PROVIDER_AT_MATURITY, HW_PROVIDER_AT_MATURITY);
		CurrencyAmount computedPay = PRICER.presentValue(SWAPTION_PAY_SHORT, RATES_PROVIDER_AT_MATURITY, HW_PROVIDER_AT_MATURITY);
		double swapPv = SWAP_PRICER.presentValue(RSWAP_REC, RATES_PROVIDER_AT_MATURITY).getAmount(EUR).Amount;
		assertEquals(computedRec.Amount, swapPv, NOTIONAL * TOL);
		assertEquals(computedPay.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_afterExpiry()
	  {
		CurrencyAmount computedRec = PRICER.presentValue(SWAPTION_REC_LONG, RATES_PROVIDER_AFTER_MATURITY, HW_PROVIDER_AFTER_MATURITY);
		CurrencyAmount computedPay = PRICER.presentValue(SWAPTION_PAY_SHORT, RATES_PROVIDER_AFTER_MATURITY, HW_PROVIDER_AFTER_MATURITY);
		assertEquals(computedRec.Amount, 0d, NOTIONAL * TOL);
		assertEquals(computedPay.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_parity()
	  {
		CurrencyAmount pvRecLong = PRICER.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER, HW_PROVIDER);
		CurrencyAmount pvRecShort = PRICER.presentValue(SWAPTION_REC_SHORT, RATE_PROVIDER, HW_PROVIDER);
		CurrencyAmount pvPayLong = PRICER.presentValue(SWAPTION_PAY_LONG, RATE_PROVIDER, HW_PROVIDER);
		CurrencyAmount pvPayShort = PRICER.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER, HW_PROVIDER);
		assertEquals(pvRecLong.Amount, -pvRecShort.Amount, NOTIONAL * TOL);
		assertEquals(pvPayLong.Amount, -pvPayShort.Amount, NOTIONAL * TOL);
		double swapPv = SWAP_PRICER.presentValue(RSWAP_PAY, RATE_PROVIDER).getAmount(EUR).Amount;
		assertEquals(pvPayLong.Amount - pvRecLong.Amount, swapPv, NOTIONAL * TOL);
		assertEquals(pvPayShort.Amount - pvRecShort.Amount, -swapPv, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currencyExposure()
	  {
		MultiCurrencyAmount computedRec = PRICER.currencyExposure(SWAPTION_REC_LONG, RATE_PROVIDER, HW_PROVIDER);
		MultiCurrencyAmount computedPay = PRICER.currencyExposure(SWAPTION_PAY_SHORT, RATE_PROVIDER, HW_PROVIDER);
		PointSensitivityBuilder pointRec = PRICER.presentValueSensitivityRates(SWAPTION_REC_LONG, RATE_PROVIDER, HW_PROVIDER);
		MultiCurrencyAmount expectedRec = RATE_PROVIDER.currencyExposure(pointRec.build()).plus(PRICER.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER, HW_PROVIDER));
		assertEquals(computedRec.size(), 1);
		assertEquals(computedRec.getAmount(EUR).Amount, expectedRec.getAmount(EUR).Amount, NOTIONAL * TOL);
		PointSensitivityBuilder pointPay = PRICER.presentValueSensitivityRates(SWAPTION_PAY_SHORT, RATE_PROVIDER, HW_PROVIDER);
		MultiCurrencyAmount expectedPay = RATE_PROVIDER.currencyExposure(pointPay.build()).plus(PRICER.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER, HW_PROVIDER));
		assertEquals(computedPay.size(), 1);
		assertEquals(computedPay.getAmount(EUR).Amount, expectedPay.getAmount(EUR).Amount, NOTIONAL * TOL);
	  }

	  public virtual void test_currencyExposure_atMaturity()
	  {
		MultiCurrencyAmount computedRec = PRICER.currencyExposure(SWAPTION_REC_LONG, RATES_PROVIDER_AT_MATURITY, HW_PROVIDER_AT_MATURITY);
		MultiCurrencyAmount computedPay = PRICER.currencyExposure(SWAPTION_PAY_SHORT, RATES_PROVIDER_AT_MATURITY, HW_PROVIDER_AT_MATURITY);
		PointSensitivityBuilder pointRec = PRICER.presentValueSensitivityRates(SWAPTION_REC_LONG, RATES_PROVIDER_AT_MATURITY, HW_PROVIDER_AT_MATURITY);
		MultiCurrencyAmount expectedRec = RATE_PROVIDER.currencyExposure(pointRec.build()).plus(PRICER.presentValue(SWAPTION_REC_LONG, RATES_PROVIDER_AT_MATURITY, HW_PROVIDER_AT_MATURITY));
		assertEquals(computedRec.size(), 1);
		assertEquals(computedRec.getAmount(EUR).Amount, expectedRec.getAmount(EUR).Amount, NOTIONAL * TOL);
		PointSensitivityBuilder pointPay = PRICER.presentValueSensitivityRates(SWAPTION_PAY_SHORT, RATES_PROVIDER_AT_MATURITY, HW_PROVIDER_AT_MATURITY);
		MultiCurrencyAmount expectedPay = RATE_PROVIDER.currencyExposure(pointPay.build()).plus(PRICER.presentValue(SWAPTION_PAY_SHORT, RATES_PROVIDER_AT_MATURITY, HW_PROVIDER_AT_MATURITY));
		assertEquals(computedPay.size(), 1);
		assertEquals(computedPay.getAmount(EUR).Amount, expectedPay.getAmount(EUR).Amount, NOTIONAL * TOL);
	  }

	  public virtual void test_currencyExposure_afterMaturity()
	  {
		MultiCurrencyAmount computedRec = PRICER.currencyExposure(SWAPTION_REC_LONG, RATES_PROVIDER_AFTER_MATURITY, HW_PROVIDER_AFTER_MATURITY);
		MultiCurrencyAmount computedPay = PRICER.currencyExposure(SWAPTION_PAY_SHORT, RATES_PROVIDER_AFTER_MATURITY, HW_PROVIDER_AFTER_MATURITY);
		assertEquals(computedRec.size(), 1);
		assertEquals(computedRec.getAmount(EUR).Amount, 0d, NOTIONAL * TOL);
		assertEquals(computedPay.size(), 1);
		assertEquals(computedPay.getAmount(EUR).Amount, 0d, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity()
	  {
		PointSensitivityBuilder pointRec = PRICER.presentValueSensitivityRates(SWAPTION_REC_LONG, RATE_PROVIDER, HW_PROVIDER);
		CurrencyParameterSensitivities computedRec = RATE_PROVIDER.parameterSensitivity(pointRec.build());
		CurrencyParameterSensitivities expectedRec = FD_CAL.sensitivity(RATE_PROVIDER, (p) => PRICER.presentValue(SWAPTION_REC_LONG, (p), HW_PROVIDER));
		assertTrue(computedRec.equalWithTolerance(expectedRec, NOTIONAL * FD_TOL * 1000d));
		PointSensitivityBuilder pointPay = PRICER.presentValueSensitivityRates(SWAPTION_PAY_SHORT, RATE_PROVIDER, HW_PROVIDER);
		CurrencyParameterSensitivities computedPay = RATE_PROVIDER.parameterSensitivity(pointPay.build());
		CurrencyParameterSensitivities expectedPay = FD_CAL.sensitivity(RATE_PROVIDER, (p) => PRICER.presentValue(SWAPTION_PAY_SHORT, (p), HW_PROVIDER));
		assertTrue(computedPay.equalWithTolerance(expectedPay, NOTIONAL * FD_TOL * 1000d));
	  }

	  public virtual void test_presentValueSensitivity_atMaturity()
	  {
		PointSensitivityBuilder pointRec = PRICER.presentValueSensitivityRates(SWAPTION_REC_LONG, RATES_PROVIDER_AT_MATURITY, HW_PROVIDER_AT_MATURITY);
		CurrencyParameterSensitivities computedRec = RATES_PROVIDER_AT_MATURITY.parameterSensitivity(pointRec.build());
		CurrencyParameterSensitivities expectedRec = FD_CAL.sensitivity(RATES_PROVIDER_AT_MATURITY, (p) => PRICER.presentValue(SWAPTION_REC_LONG, (p), HW_PROVIDER_AT_MATURITY));
		assertTrue(computedRec.equalWithTolerance(expectedRec, NOTIONAL * FD_TOL * 1000d));
		PointSensitivities pointPay = PRICER.presentValueSensitivityRates(SWAPTION_PAY_SHORT, RATES_PROVIDER_AT_MATURITY, HW_PROVIDER_AT_MATURITY).build();
		foreach (PointSensitivity sensi in pointPay.Sensitivities)
		{
		  assertEquals(Math.Abs(sensi.Sensitivity), 0d);
		}
	  }

	  public virtual void test_presentValueSensitivity_afterMaturity()
	  {
		PointSensitivities pointRec = PRICER.presentValueSensitivityRates(SWAPTION_REC_LONG, RATES_PROVIDER_AFTER_MATURITY, HW_PROVIDER_AFTER_MATURITY).build();
		foreach (PointSensitivity sensi in pointRec.Sensitivities)
		{
		  assertEquals(Math.Abs(sensi.Sensitivity), 0d);
		}
		PointSensitivities pointPay = PRICER.presentValueSensitivityRates(SWAPTION_PAY_SHORT, RATES_PROVIDER_AFTER_MATURITY, HW_PROVIDER_AFTER_MATURITY).build();
		foreach (PointSensitivity sensi in pointPay.Sensitivities)
		{
		  assertEquals(Math.Abs(sensi.Sensitivity), 0d);
		}
	  }

	  public virtual void test_presentValueSensitivity_parity()
	  {
		CurrencyParameterSensitivities pvSensiRecLong = RATE_PROVIDER.parameterSensitivity(PRICER.presentValueSensitivityRates(SWAPTION_REC_LONG, RATE_PROVIDER, HW_PROVIDER).build());
		CurrencyParameterSensitivities pvSensiRecShort = RATE_PROVIDER.parameterSensitivity(PRICER.presentValueSensitivityRates(SWAPTION_REC_SHORT, RATE_PROVIDER, HW_PROVIDER).build());
		CurrencyParameterSensitivities pvSensiPayLong = RATE_PROVIDER.parameterSensitivity(PRICER.presentValueSensitivityRates(SWAPTION_PAY_LONG, RATE_PROVIDER, HW_PROVIDER).build());
		CurrencyParameterSensitivities pvSensiPayShort = RATE_PROVIDER.parameterSensitivity(PRICER.presentValueSensitivityRates(SWAPTION_PAY_SHORT, RATE_PROVIDER, HW_PROVIDER).build());
		assertTrue(pvSensiRecLong.equalWithTolerance(pvSensiRecShort.multipliedBy(-1d), NOTIONAL * TOL));
		assertTrue(pvSensiPayLong.equalWithTolerance(pvSensiPayShort.multipliedBy(-1d), NOTIONAL * TOL));
		PointSensitivities expectedPoint = SWAP_PRICER.presentValueSensitivity(RSWAP_PAY, RATE_PROVIDER).build();
		CurrencyParameterSensitivities expected = RATE_PROVIDER.parameterSensitivity(expectedPoint);
		assertTrue(expected.equalWithTolerance(pvSensiPayLong.combinedWith(pvSensiRecLong.multipliedBy(-1d)), NOTIONAL * TOL));
		assertTrue(expected.equalWithTolerance(pvSensiRecShort.combinedWith(pvSensiPayShort.multipliedBy(-1d)), NOTIONAL * TOL));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivityHullWhiteParameter()
	  {
		DoubleArray computedRec = PRICER.presentValueSensitivityModelParamsHullWhite(SWAPTION_REC_LONG, RATE_PROVIDER, HW_PROVIDER);
		DoubleArray computedPay = PRICER.presentValueSensitivityModelParamsHullWhite(SWAPTION_PAY_SHORT, RATE_PROVIDER, HW_PROVIDER);
		DoubleArray vols = HW_PROVIDER.Parameters.Volatility;
		int size = vols.size();
		double[] expectedRec = new double[size];
		double[] expectedPay = new double[size];
		for (int i = 0; i < size; ++i)
		{
		  double[] volsUp = vols.toArray();
		  double[] volsDw = vols.toArray();
		  volsUp[i] += FD_TOL;
		  volsDw[i] -= FD_TOL;
		  HullWhiteOneFactorPiecewiseConstantParameters paramsUp = HullWhiteOneFactorPiecewiseConstantParameters.of(HW_PROVIDER.Parameters.MeanReversion, DoubleArray.copyOf(volsUp), HW_PROVIDER.Parameters.VolatilityTime.subArray(1, size));
		  HullWhiteOneFactorPiecewiseConstantParameters paramsDw = HullWhiteOneFactorPiecewiseConstantParameters.of(HW_PROVIDER.Parameters.MeanReversion, DoubleArray.copyOf(volsDw), HW_PROVIDER.Parameters.VolatilityTime.subArray(1, size));
		  HullWhiteOneFactorPiecewiseConstantParametersProvider provUp = HullWhiteOneFactorPiecewiseConstantParametersProvider.of(paramsUp, HW_PROVIDER.DayCount, HW_PROVIDER.ValuationDateTime);
		  HullWhiteOneFactorPiecewiseConstantParametersProvider provDw = HullWhiteOneFactorPiecewiseConstantParametersProvider.of(paramsDw, HW_PROVIDER.DayCount, HW_PROVIDER.ValuationDateTime);
		  expectedRec[i] = 0.5 * (PRICER.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER, provUp).Amount - PRICER.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER, provDw).Amount) / FD_TOL;
		  expectedPay[i] = 0.5 * (PRICER.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER, provUp).Amount - PRICER.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER, provDw).Amount) / FD_TOL;
		}
		assertTrue(DoubleArrayMath.fuzzyEquals(computedRec.toArray(), expectedRec, NOTIONAL * FD_TOL));
		assertTrue(DoubleArrayMath.fuzzyEquals(computedPay.toArray(), expectedPay, NOTIONAL * FD_TOL));
	  }

	  public virtual void test_presentValueSensitivityHullWhiteParameter_atMaturity()
	  {
		DoubleArray pvSensiRec = PRICER.presentValueSensitivityModelParamsHullWhite(SWAPTION_REC_LONG, RATES_PROVIDER_AT_MATURITY, HW_PROVIDER_AT_MATURITY);
		assertTrue(pvSensiRec.equalZeroWithTolerance(NOTIONAL * TOL));
		DoubleArray pvSensiPay = PRICER.presentValueSensitivityModelParamsHullWhite(SWAPTION_PAY_SHORT, RATES_PROVIDER_AT_MATURITY, HW_PROVIDER_AT_MATURITY);
		assertTrue(pvSensiPay.equalZeroWithTolerance(NOTIONAL * TOL));
	  }

	  public virtual void test_presentValueSensitivityHullWhiteParameter_afterMaturity()
	  {
		DoubleArray pvSensiRec = PRICER.presentValueSensitivityModelParamsHullWhite(SWAPTION_REC_LONG, RATES_PROVIDER_AFTER_MATURITY, HW_PROVIDER_AFTER_MATURITY);
		assertTrue(pvSensiRec.equalZeroWithTolerance(NOTIONAL * TOL));
		DoubleArray pvSensiPay = PRICER.presentValueSensitivityModelParamsHullWhite(SWAPTION_PAY_SHORT, RATES_PROVIDER_AFTER_MATURITY, HW_PROVIDER_AFTER_MATURITY);
		assertTrue(pvSensiPay.equalZeroWithTolerance(NOTIONAL * TOL));
	  }

	  public virtual void test_presentValueSensitivityHullWhiteParameter_parity()
	  {
		DoubleArray pvSensiRecLong = PRICER.presentValueSensitivityModelParamsHullWhite(SWAPTION_REC_LONG, RATE_PROVIDER, HW_PROVIDER);
		DoubleArray pvSensiRecShort = PRICER.presentValueSensitivityModelParamsHullWhite(SWAPTION_REC_SHORT, RATE_PROVIDER, HW_PROVIDER);
		DoubleArray pvSensiPayLong = PRICER.presentValueSensitivityModelParamsHullWhite(SWAPTION_PAY_LONG, RATE_PROVIDER, HW_PROVIDER);
		DoubleArray pvSensiPayShort = PRICER.presentValueSensitivityModelParamsHullWhite(SWAPTION_PAY_SHORT, RATE_PROVIDER, HW_PROVIDER);
		assertTrue(pvSensiRecLong.equalWithTolerance(pvSensiRecShort.multipliedBy(-1d), NOTIONAL * TOL));
		assertTrue(pvSensiPayLong.equalWithTolerance(pvSensiPayShort.multipliedBy(-1d), NOTIONAL * TOL));
		assertTrue(pvSensiPayLong.equalWithTolerance(pvSensiRecLong, NOTIONAL * TOL));
		assertTrue(pvSensiRecShort.equalWithTolerance(pvSensiPayShort, NOTIONAL * TOL));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void regression_pv()
	  {
		CurrencyAmount pv = PRICER.presentValue(SWAPTION_PAY_LONG, RATE_PROVIDER, HW_PROVIDER);
		assertEquals(pv.Amount, 4213670.335092038, NOTIONAL * TOL);
	  }

	  public virtual void regression_curveSensitivity()
	  {
		PointSensitivities point = PRICER.presentValueSensitivityRates(SWAPTION_PAY_LONG, RATE_PROVIDER, HW_PROVIDER).build();
		CurrencyParameterSensitivities computed = RATE_PROVIDER.parameterSensitivity(point);
		double[] dscExp = new double[] {0.0, 0.0, 0.0, 0.0, -1.4127023229222856E7, -1.744958350376594E7};
		double[] fwdExp = new double[] {0.0, 0.0, 0.0, 0.0, -2.0295973516660026E8, 4.12336887967829E8};
		assertTrue(DoubleArrayMath.fuzzyEquals(computed.getSensitivity(HullWhiteIborFutureDataSet.DSC_NAME, EUR).Sensitivity.toArray(), dscExp, NOTIONAL * TOL));
		assertTrue(DoubleArrayMath.fuzzyEquals(computed.getSensitivity(HullWhiteIborFutureDataSet.FWD6_NAME, EUR).Sensitivity.toArray(), fwdExp, NOTIONAL * TOL));
	  }

	  public virtual void regression_hullWhiteSensitivity()
	  {
		DoubleArray computed = PRICER.presentValueSensitivityModelParamsHullWhite(SWAPTION_PAY_LONG, RATE_PROVIDER, HW_PROVIDER);
		double[] expected = new double[] {2.9365484063149095E7, 3.262667329294093E7, 7.226220286364576E7, 2.4446925038968167E8, 120476.73820821749};
		assertTrue(DoubleArrayMath.fuzzyEquals(computed.toArray(), expected, NOTIONAL * TOL));
	  }
	}

}