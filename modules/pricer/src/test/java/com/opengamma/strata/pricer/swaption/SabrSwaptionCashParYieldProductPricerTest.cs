using System;
using System.Collections.Generic;

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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.fail;


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
	using DoublesPair = com.opengamma.strata.collect.tuple.DoublesPair;
	using SabrParameterType = com.opengamma.strata.market.model.SabrParameterType;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using ConstantSurface = com.opengamma.strata.market.surface.ConstantSurface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
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
	using SwapLegType = com.opengamma.strata.product.swap.SwapLegType;
	using CashSwaptionSettlement = com.opengamma.strata.product.swaption.CashSwaptionSettlement;
	using CashSwaptionSettlementMethod = com.opengamma.strata.product.swaption.CashSwaptionSettlementMethod;
	using PhysicalSwaptionSettlement = com.opengamma.strata.product.swaption.PhysicalSwaptionSettlement;
	using ResolvedSwaption = com.opengamma.strata.product.swaption.ResolvedSwaption;
	using Swaption = com.opengamma.strata.product.swaption.Swaption;

	/// <summary>
	/// Test <seealso cref="SabrSwaptionCashParYieldProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrSwaptionCashParYieldProductPricerTest
	public class SabrSwaptionCashParYieldProductPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly ZonedDateTime VAL_DATE_TIME = dateUtc(2008, 8, 18);

	  private static readonly ZonedDateTime MATURITY = dateUtc(2014, 3, 18);
	  private static readonly HolidayCalendarId CALENDAR = HolidayCalendarIds.SAT_SUN;
	  private static readonly BusinessDayAdjustment BDA_MF = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, CALENDAR);
	  private static readonly LocalDate SETTLE = BDA_MF.adjust(CALENDAR.resolve(REF_DATA).shift(MATURITY.toLocalDate(), 2), REF_DATA);
	  private const double NOTIONAL = 100000000; //100m
	  private const int TENOR_YEAR = 5;
	  private static readonly LocalDate END = SETTLE.plusYears(TENOR_YEAR);
	  private const double RATE = 0.0175;
	  private static readonly PeriodicSchedule PERIOD_FIXED = PeriodicSchedule.builder().startDate(SETTLE).endDate(END).frequency(P6M).businessDayAdjustment(BDA_MF).stubConvention(StubConvention.SHORT_FINAL).rollConvention(RollConventions.EOM).build();
	  private static readonly PaymentSchedule PAYMENT_FIXED = PaymentSchedule.builder().paymentFrequency(P6M).paymentDateOffset(DaysAdjustment.NONE).build();
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
	  private static readonly ResolvedSwapLeg RFIXED_LEG_REC = FIXED_LEG_REC.resolve(REF_DATA);
	  private static readonly CashSwaptionSettlement PAR_YIELD = CashSwaptionSettlement.of(SETTLE, CashSwaptionSettlementMethod.PAR_YIELD);
	  private static readonly ResolvedSwaption SWAPTION_REC_LONG = Swaption.builder().expiryDate(AdjustableDate.of(MATURITY.toLocalDate(), BDA_MF)).expiryTime(MATURITY.toLocalTime()).expiryZone(MATURITY.Zone).swaptionSettlement(PAR_YIELD).longShort(LONG).underlying(SWAP_REC).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_REC_SHORT = Swaption.builder().expiryDate(AdjustableDate.of(MATURITY.toLocalDate(), BDA_MF)).expiryTime(MATURITY.toLocalTime()).expiryZone(MATURITY.Zone).swaptionSettlement(PAR_YIELD).longShort(SHORT).underlying(SWAP_REC).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_PAY_LONG = Swaption.builder().expiryDate(AdjustableDate.of(MATURITY.toLocalDate(), BDA_MF)).expiryTime(MATURITY.toLocalTime()).expiryZone(MATURITY.Zone).swaptionSettlement(PAR_YIELD).longShort(LONG).underlying(SWAP_PAY).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_PAY_SHORT = Swaption.builder().expiryDate(AdjustableDate.of(MATURITY.toLocalDate(), BDA_MF)).expiryTime(MATURITY.toLocalTime()).expiryZone(MATURITY.Zone).swaptionSettlement(PAR_YIELD).longShort(SHORT).underlying(SWAP_PAY).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_PHYS = Swaption.builder().expiryDate(AdjustableDate.of(MATURITY.toLocalDate())).expiryTime(MATURITY.toLocalTime()).expiryZone(MATURITY.Zone).longShort(LongShort.LONG).swaptionSettlement(PhysicalSwaptionSettlement.DEFAULT).underlying(SWAP_REC).build().resolve(REF_DATA);

	  private static readonly SabrParametersSwaptionVolatilities VOLS_REG = SwaptionSabrRateVolatilityDataSet.getVolatilitiesEur(VAL_DATE_TIME.toLocalDate(), false);
	  private static readonly SabrParametersSwaptionVolatilities VOLS = SwaptionSabrRateVolatilityDataSet.getVolatilitiesEur(VAL_DATE_TIME.toLocalDate(), true);
	  private static readonly SabrParametersSwaptionVolatilities VOLS_AT_MATURITY = SwaptionSabrRateVolatilityDataSet.getVolatilitiesEur(MATURITY.toLocalDate(), true);
	  private static readonly SabrParametersSwaptionVolatilities VOLS_AFTER_MATURITY = SwaptionSabrRateVolatilityDataSet.getVolatilitiesEur(MATURITY.toLocalDate().plusDays(1), true);
	  private static readonly ImmutableRatesProvider RATE_PROVIDER = SwaptionSabrRateVolatilityDataSet.getRatesProviderEur(VAL_DATE_TIME.toLocalDate());
	  private static readonly ImmutableRatesProvider RATE_PROVIDER_AT_MATURITY = SwaptionSabrRateVolatilityDataSet.getRatesProviderEur(MATURITY.toLocalDate());
	  private static readonly ImmutableRatesProvider RATE_PROVIDER_AFTER_MATURITY = SwaptionSabrRateVolatilityDataSet.getRatesProviderEur(MATURITY.toLocalDate().plusDays(1));

	  private const double TOL = 1.0e-13;
	  private const double TOLERANCE_DELTA = 1.0E-2;
	  private const double FD_EPS = 1.0e-6;
	  private static readonly SabrSwaptionCashParYieldProductPricer PRICER = SabrSwaptionCashParYieldProductPricer.DEFAULT;
	  private static readonly DiscountingSwapProductPricer PRICER_SWAP = DiscountingSwapProductPricer.DEFAULT;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FD_CAL = new RatesFiniteDifferenceSensitivityCalculator(FD_EPS);

	  //-------------------------------------------------------------------------
	  public virtual void validate_cash_settlement()
	  {
		assertThrowsIllegalArg(() => PRICER.presentValue(SWAPTION_PHYS, RATE_PROVIDER, VOLS));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		CurrencyAmount computedRec = PRICER.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount computedPay = PRICER.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		double forward = PRICER_SWAP.parRate(RSWAP_REC, RATE_PROVIDER);
		double annuityCash = PRICER_SWAP.LegPricer.annuityCash(RFIXED_LEG_REC, forward);
		double expiry = VOLS.relativeTime(MATURITY);
		double volatility = VOLS.volatility(SWAPTION_REC_LONG.Expiry, TENOR_YEAR, RATE, forward);
		double df = RATE_PROVIDER.discountFactor(EUR, SETTLE);
		double expectedRec = df * annuityCash * BlackFormulaRepository.price(forward + SwaptionSabrRateVolatilityDataSet.SHIFT, RATE + SwaptionSabrRateVolatilityDataSet.SHIFT, expiry, volatility, false);
		double expectedPay = -df * annuityCash * BlackFormulaRepository.price(forward + SwaptionSabrRateVolatilityDataSet.SHIFT, RATE + SwaptionSabrRateVolatilityDataSet.SHIFT, expiry, volatility, true);
		assertEquals(computedRec.Currency, EUR);
		assertEquals(computedRec.Amount, expectedRec, NOTIONAL * TOL);
		assertEquals(computedPay.Currency, EUR);
		assertEquals(computedPay.Amount, expectedPay, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_atMaturity()
	  {
		CurrencyAmount computedRec = PRICER.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		CurrencyAmount computedPay = PRICER.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		double forward = PRICER_SWAP.parRate(RSWAP_REC, RATE_PROVIDER_AT_MATURITY);
		double annuityCash = PRICER_SWAP.LegPricer.annuityCash(RFIXED_LEG_REC, forward);
		double df = RATE_PROVIDER_AT_MATURITY.discountFactor(EUR, SETTLE);
		assertEquals(computedRec.Amount, df * annuityCash * (RATE - forward), NOTIONAL * TOL);
		assertEquals(computedPay.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_afterExpiry()
	  {
		CurrencyAmount computedRec = PRICER.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		CurrencyAmount computedPay = PRICER.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		assertEquals(computedRec.Amount, 0d, NOTIONAL * TOL);
		assertEquals(computedPay.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_parity()
	  {
		CurrencyAmount pvRecLong = PRICER.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount pvRecShort = PRICER.presentValue(SWAPTION_REC_SHORT, RATE_PROVIDER, VOLS);
		CurrencyAmount pvPayLong = PRICER.presentValue(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount pvPayShort = PRICER.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		assertEquals(pvRecLong.Amount, -pvRecShort.Amount, NOTIONAL * TOL);
		assertEquals(pvPayLong.Amount, -pvPayShort.Amount, NOTIONAL * TOL);
		double forward = PRICER_SWAP.parRate(RSWAP_REC, RATE_PROVIDER);
		double annuityCash = PRICER_SWAP.LegPricer.annuityCash(RFIXED_LEG_REC, forward);
		double df = RATE_PROVIDER.discountFactor(EUR, SETTLE);
		double expected = df * annuityCash * (forward - RATE);
		assertEquals(pvPayLong.Amount - pvRecLong.Amount, expected, NOTIONAL * TOL);
		assertEquals(pvPayShort.Amount - pvRecShort.Amount, -expected, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_parity_atMaturity()
	  {
		CurrencyAmount pvRecLong = PRICER.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		CurrencyAmount pvRecShort = PRICER.presentValue(SWAPTION_REC_SHORT, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		CurrencyAmount pvPayLong = PRICER.presentValue(SWAPTION_PAY_LONG, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		CurrencyAmount pvPayShort = PRICER.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		assertEquals(pvRecLong.Amount, -pvRecShort.Amount, NOTIONAL * TOL);
		assertEquals(pvPayLong.Amount, -pvPayShort.Amount, NOTIONAL * TOL);
		double forward = PRICER_SWAP.parRate(RSWAP_REC, RATE_PROVIDER_AT_MATURITY);
		double annuityCash = PRICER_SWAP.LegPricer.annuityCash(RFIXED_LEG_REC, forward);
		double df = RATE_PROVIDER_AT_MATURITY.discountFactor(EUR, SETTLE);
		double expected = df * annuityCash * (forward - RATE);
		assertEquals(pvPayLong.Amount - pvRecLong.Amount, expected, NOTIONAL * TOL);
		assertEquals(pvPayShort.Amount - pvRecShort.Amount, -expected, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currencyExposure()
	  {
		MultiCurrencyAmount computedRec = PRICER.currencyExposure(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount computedPay = PRICER.currencyExposure(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		PointSensitivityBuilder pointRec = PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount expectedRec = RATE_PROVIDER.currencyExposure(pointRec.build()).plus(PRICER.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS));
		assertEquals(computedRec.size(), 1);
		assertEquals(computedRec.getAmount(EUR).Amount, expectedRec.getAmount(EUR).Amount, NOTIONAL * TOL);
		PointSensitivityBuilder pointPay = PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount expectedPay = RATE_PROVIDER.currencyExposure(pointPay.build()).plus(PRICER.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS));
		assertEquals(computedPay.size(), 1);
		assertEquals(computedPay.getAmount(EUR).Amount, expectedPay.getAmount(EUR).Amount, NOTIONAL * TOL);
	  }

	  public virtual void test_currencyExposure_atMaturity()
	  {
		MultiCurrencyAmount computedRec = PRICER.currencyExposure(SWAPTION_REC_LONG, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		MultiCurrencyAmount computedPay = PRICER.currencyExposure(SWAPTION_PAY_SHORT, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		PointSensitivityBuilder pointRec = PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_REC_LONG, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		MultiCurrencyAmount expectedRec = RATE_PROVIDER.currencyExposure(pointRec.build()).plus(PRICER.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY));
		assertEquals(computedRec.size(), 1);
		assertEquals(computedRec.getAmount(EUR).Amount, expectedRec.getAmount(EUR).Amount, NOTIONAL * TOL);
		PointSensitivityBuilder pointPay = PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_PAY_SHORT, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		MultiCurrencyAmount expectedPay = RATE_PROVIDER.currencyExposure(pointPay.build()).plus(PRICER.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY));
		assertEquals(computedPay.size(), 1);
		assertEquals(computedPay.getAmount(EUR).Amount, expectedPay.getAmount(EUR).Amount, NOTIONAL * TOL);
	  }

	  public virtual void test_currencyExposure_afterMaturity()
	  {
		MultiCurrencyAmount computedRec = PRICER.currencyExposure(SWAPTION_REC_LONG, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		MultiCurrencyAmount computedPay = PRICER.currencyExposure(SWAPTION_PAY_SHORT, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		assertEquals(computedRec.size(), 1);
		assertEquals(computedRec.getAmount(EUR).Amount, 0d, NOTIONAL * TOL);
		assertEquals(computedPay.size(), 1);
		assertEquals(computedPay.getAmount(EUR).Amount, 0d, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_impliedVolatility()
	  {
		double computedRec = PRICER.impliedVolatility(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		double computedPay = PRICER.impliedVolatility(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		double forward = PRICER_SWAP.parRate(RSWAP_REC, RATE_PROVIDER);
		double expected = VOLS.volatility(MATURITY, TENOR_YEAR, RATE, forward);
		assertEquals(computedRec, expected, TOL);
		assertEquals(computedPay, expected, TOL);
	  }

	  public virtual void test_impliedVolatility_atMaturity()
	  {
		double computedRec = PRICER.impliedVolatility(SWAPTION_REC_LONG, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		double computedPay = PRICER.impliedVolatility(SWAPTION_PAY_SHORT, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		double forward = PRICER_SWAP.parRate(RSWAP_REC, RATE_PROVIDER_AT_MATURITY);
		double expected = VOLS_AT_MATURITY.volatility(MATURITY, TENOR_YEAR, RATE, forward);
		assertEquals(computedRec, expected, TOL);
		assertEquals(computedPay, expected, TOL);
	  }

	  public virtual void test_impliedVolatility_afterMaturity()
	  {
		assertThrowsIllegalArg(() => PRICER.impliedVolatility(SWAPTION_REC_LONG, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY));
		assertThrowsIllegalArg(() => PRICER.impliedVolatility(SWAPTION_PAY_SHORT, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueDelta_parity()
	  {
		double forward = PRICER_SWAP.parRate(RSWAP_REC, RATE_PROVIDER);
		ResolvedSwapLeg fixedLeg = SWAPTION_REC_LONG.Underlying.getLegs(SwapLegType.FIXED).get(0);
		double annuityCash = PRICER_SWAP.LegPricer.annuityCash(fixedLeg, forward);
		CashSwaptionSettlement cashSettlement = (CashSwaptionSettlement) SWAPTION_REC_LONG.SwaptionSettlement;
		double discountSettle = RATE_PROVIDER.discountFactor(fixedLeg.Currency, cashSettlement.SettlementDate);
		double pvbpCash = Math.Abs(annuityCash * discountSettle);
		CurrencyAmount deltaRec = PRICER.presentValueDelta(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount deltaPay = PRICER.presentValueDelta(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		assertEquals(deltaRec.Amount + deltaPay.Amount, -pvbpCash, TOLERANCE_DELTA);
	  }

	  public virtual void test_presentValueDelta_afterMaturity()
	  {
		CurrencyAmount deltaRec = PRICER.presentValueDelta(SWAPTION_REC_LONG, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		assertEquals(deltaRec.Amount, 0, TOLERANCE_DELTA);
		CurrencyAmount deltaPay = PRICER.presentValueDelta(SWAPTION_PAY_SHORT, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		assertEquals(deltaPay.Amount, 0, TOLERANCE_DELTA);
	  }

	  public virtual void test_presentValueDelta_atMaturity()
	  {
		double forward = PRICER_SWAP.parRate(RSWAP_REC, RATE_PROVIDER_AT_MATURITY);
		ResolvedSwapLeg fixedLeg = SWAPTION_REC_LONG.Underlying.getLegs(SwapLegType.FIXED).get(0);
		double annuityCash = PRICER_SWAP.LegPricer.annuityCash(fixedLeg, forward);
		CashSwaptionSettlement cashSettlement = (CashSwaptionSettlement) SWAPTION_REC_LONG.SwaptionSettlement;
		double discountSettle = RATE_PROVIDER_AT_MATURITY.discountFactor(fixedLeg.Currency, cashSettlement.SettlementDate);
		double pvbpCash = Math.Abs(annuityCash * discountSettle);
		CurrencyAmount deltaRec = PRICER.presentValueDelta(SWAPTION_REC_LONG, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		assertEquals(deltaRec.Amount, RATE > forward ? -pvbpCash : 0, TOLERANCE_DELTA);
		CurrencyAmount deltaPay = PRICER.presentValueDelta(SWAPTION_PAY_SHORT, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		assertEquals(deltaPay.Amount, RATE > forward ? 0 : pvbpCash, TOLERANCE_DELTA);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivityRatesStickyModel()
	  {
		PointSensitivityBuilder pointRec = PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyParameterSensitivities computedRec = RATE_PROVIDER.parameterSensitivity(pointRec.build());
		CurrencyParameterSensitivities expectedRec = FD_CAL.sensitivity(RATE_PROVIDER, (p) => PRICER.presentValue(SWAPTION_REC_LONG, (p), VOLS));
		assertTrue(computedRec.equalWithTolerance(expectedRec, NOTIONAL * FD_EPS * 200d));
		PointSensitivityBuilder pointPay = PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		CurrencyParameterSensitivities computedPay = RATE_PROVIDER.parameterSensitivity(pointPay.build());
		CurrencyParameterSensitivities expectedPay = FD_CAL.sensitivity(RATE_PROVIDER, (p) => PRICER.presentValue(SWAPTION_PAY_SHORT, (p), VOLS));
		assertTrue(computedPay.equalWithTolerance(expectedPay, NOTIONAL * FD_EPS * 200d));
	  }

	  public virtual void test_presentValueSensitivityRatesStickyStrike()
	  {
		SwaptionVolatilities volSabr = SwaptionSabrRateVolatilityDataSet.getVolatilitiesEur(VAL_DATE_TIME.toLocalDate(), false);
		double impliedVol = PRICER.impliedVolatility(SWAPTION_REC_LONG, RATE_PROVIDER, volSabr);
		SurfaceMetadata blackMeta = Surfaces.blackVolatilityByExpiryTenor("CST", VOLS.DayCount);
		SwaptionVolatilities volCst = BlackSwaptionExpiryTenorVolatilities.of(VOLS.Convention, VOLS.ValuationDateTime, ConstantSurface.of(blackMeta, impliedVol));
		// To obtain a constant volatility surface which create a sticky strike sensitivity
		PointSensitivityBuilder pointRec = PRICER.presentValueSensitivityRatesStickyStrike(SWAPTION_REC_LONG, RATE_PROVIDER, volSabr);
		CurrencyParameterSensitivities computedRec = RATE_PROVIDER.parameterSensitivity(pointRec.build());
		CurrencyParameterSensitivities expectedRec = FD_CAL.sensitivity(RATE_PROVIDER, (p) => PRICER.presentValue(SWAPTION_REC_LONG, (p), volCst));
		assertTrue(computedRec.equalWithTolerance(expectedRec, NOTIONAL * FD_EPS * 300d));

		PointSensitivityBuilder pointPay = PRICER.presentValueSensitivityRatesStickyStrike(SWAPTION_PAY_SHORT, RATE_PROVIDER, volSabr);
		CurrencyParameterSensitivities computedPay = RATE_PROVIDER.parameterSensitivity(pointPay.build());
		CurrencyParameterSensitivities expectedPay = FD_CAL.sensitivity(RATE_PROVIDER, (p) => PRICER.presentValue(SWAPTION_PAY_SHORT, (p), volCst));
		assertTrue(computedPay.equalWithTolerance(expectedPay, NOTIONAL * FD_EPS * 300d));
	  }

	  public virtual void test_presentValueSensitivityRatesStickyModel_atMaturity()
	  {
		PointSensitivityBuilder pointRec = PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_REC_LONG, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		CurrencyParameterSensitivities computedRec = RATE_PROVIDER_AT_MATURITY.parameterSensitivity(pointRec.build());
		CurrencyParameterSensitivities expectedRec = FD_CAL.sensitivity(RATE_PROVIDER_AT_MATURITY, (p) => PRICER.presentValue(SWAPTION_REC_LONG, (p), VOLS_AT_MATURITY));
		assertTrue(computedRec.equalWithTolerance(expectedRec, NOTIONAL * FD_EPS * 100d));
		PointSensitivities pointPay = PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_PAY_SHORT, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY).build();
		foreach (PointSensitivity sensi in pointPay.Sensitivities)
		{
		  assertEquals(Math.Abs(sensi.Sensitivity), 0d);
		}
	  }

	  public virtual void test_presentValueSensitivityRatesStickyModel_afterMaturity()
	  {
		PointSensitivities pointRec = PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_REC_LONG, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY).build();
		foreach (PointSensitivity sensi in pointRec.Sensitivities)
		{
		  assertEquals(Math.Abs(sensi.Sensitivity), 0d);
		}
		PointSensitivities pointPay = PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_PAY_SHORT, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY).build();
		foreach (PointSensitivity sensi in pointPay.Sensitivities)
		{
		  assertEquals(Math.Abs(sensi.Sensitivity), 0d);
		}
	  }

	  public virtual void test_presentValueSensitivityRatesStickyModel_parity()
	  {
		CurrencyParameterSensitivities pvSensiRecLong = RATE_PROVIDER.parameterSensitivity(PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS).build());
		CurrencyParameterSensitivities pvSensiRecShort = RATE_PROVIDER.parameterSensitivity(PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_REC_SHORT, RATE_PROVIDER, VOLS).build());
		CurrencyParameterSensitivities pvSensiPayLong = RATE_PROVIDER.parameterSensitivity(PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS).build());
		CurrencyParameterSensitivities pvSensiPayShort = RATE_PROVIDER.parameterSensitivity(PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS).build());
		assertTrue(pvSensiRecLong.equalWithTolerance(pvSensiRecShort.multipliedBy(-1d), NOTIONAL * TOL));
		assertTrue(pvSensiPayLong.equalWithTolerance(pvSensiPayShort.multipliedBy(-1d), NOTIONAL * TOL));

		double forward = PRICER_SWAP.parRate(RSWAP_REC, RATE_PROVIDER);
		PointSensitivityBuilder forwardSensi = PRICER_SWAP.parRateSensitivity(RSWAP_REC, RATE_PROVIDER);
		double annuityCash = PRICER_SWAP.LegPricer.annuityCash(RSWAP_REC.getLegs(SwapLegType.FIXED).get(0), forward);
		double annuityCashDeriv = PRICER_SWAP.LegPricer.annuityCashDerivative(RSWAP_REC.getLegs(SwapLegType.FIXED).get(0), forward).getDerivative(0);
		double discount = RATE_PROVIDER.discountFactor(EUR, SETTLE);
		PointSensitivityBuilder discountSensi = RATE_PROVIDER.discountFactors(EUR).zeroRatePointSensitivity(SETTLE);
		PointSensitivities expecedPoint = discountSensi.multipliedBy(annuityCash * (forward - RATE)).combinedWith(forwardSensi.multipliedBy(discount * annuityCash + discount * annuityCashDeriv * (forward - RATE))).build();
		CurrencyParameterSensitivities expected = RATE_PROVIDER.parameterSensitivity(expecedPoint);
		assertTrue(expected.equalWithTolerance(pvSensiPayLong.combinedWith(pvSensiRecLong.multipliedBy(-1d)), NOTIONAL * TOL));
		assertTrue(expected.equalWithTolerance(pvSensiRecShort.combinedWith(pvSensiPayShort.multipliedBy(-1d)), NOTIONAL * TOL));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueVega_parity()
	  {
		SwaptionSensitivity vegaRec = PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		SwaptionSensitivity vegaPay = PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		assertEquals(vegaRec.Sensitivity, -vegaPay.Sensitivity, TOLERANCE_DELTA);
	  }

	  public virtual void test_presentValueVega_atMaturity()
	  {
		SwaptionSensitivity vegaRec = PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_REC_LONG, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		assertEquals(vegaRec.Sensitivity, 0, TOLERANCE_DELTA);
		SwaptionSensitivity vegaPay = PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_PAY_SHORT, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		assertEquals(vegaPay.Sensitivity, 0, TOLERANCE_DELTA);
	  }

	  public virtual void test_presentValueVega_afterMaturity()
	  {
		SwaptionSensitivity vegaRec = PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_REC_LONG, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		assertEquals(vegaRec.Sensitivity, 0, TOLERANCE_DELTA);
		SwaptionSensitivity vegaPay = PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_PAY_SHORT, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		assertEquals(vegaPay.Sensitivity, 0, TOLERANCE_DELTA);
	  }

	  public virtual void test_presentValueVega_SwaptionSensitivity()
	  {
		SwaptionSensitivity vegaRec = PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		assertEquals(VOLS.parameterSensitivity(vegaRec), CurrencyParameterSensitivities.empty());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivityModelParamsSabr()
	  {
		PointSensitivities sensiRec = PRICER.presentValueSensitivityModelParamsSabr(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS).build();
		PointSensitivities sensiPay = PRICER.presentValueSensitivityModelParamsSabr(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS).build();
		double forward = PRICER_SWAP.parRate(RSWAP_REC, RATE_PROVIDER);
		double annuityCash = PRICER_SWAP.LegPricer.annuityCash(RFIXED_LEG_REC, forward);
		double expiry = VOLS.relativeTime(MATURITY);
		double volatility = VOLS.volatility(SWAPTION_REC_LONG.Expiry, TENOR_YEAR, RATE, forward);
		double df = RATE_PROVIDER.discountFactor(EUR, SETTLE);
		double[] volSensi = VOLS.Parameters.volatilityAdjoint(expiry, TENOR_YEAR, RATE, forward).Derivatives.toArray();
		double vegaRec = df * annuityCash * BlackFormulaRepository.vega(forward + SwaptionSabrRateVolatilityDataSet.SHIFT, RATE + SwaptionSabrRateVolatilityDataSet.SHIFT, expiry, volatility);
		double vegaPay = -df * annuityCash * BlackFormulaRepository.vega(forward + SwaptionSabrRateVolatilityDataSet.SHIFT, RATE + SwaptionSabrRateVolatilityDataSet.SHIFT, expiry, volatility);
		assertSensitivity(sensiRec, SabrParameterType.ALPHA, vegaRec * volSensi[2]);
		assertSensitivity(sensiRec, SabrParameterType.BETA, vegaRec * volSensi[3]);
		assertSensitivity(sensiRec, SabrParameterType.RHO, vegaRec * volSensi[4]);
		assertSensitivity(sensiRec, SabrParameterType.NU, vegaRec * volSensi[5]);
		assertSensitivity(sensiPay, SabrParameterType.ALPHA, vegaPay * volSensi[2]);
		assertSensitivity(sensiPay, SabrParameterType.BETA, vegaPay * volSensi[3]);
		assertSensitivity(sensiPay, SabrParameterType.RHO, vegaPay * volSensi[4]);
		assertSensitivity(sensiPay, SabrParameterType.NU, vegaPay * volSensi[5]);
	  }

	  private void assertSensitivity(PointSensitivities points, SabrParameterType type, double expected)
	  {
		foreach (PointSensitivity point in points.Sensitivities)
		{
		  SwaptionSabrSensitivity sens = (SwaptionSabrSensitivity) point;
		  assertEquals(sens.Currency, EUR);
		  assertEquals(sens.VolatilitiesName, VOLS.Name);
		  if (sens.SensitivityType == type)
		  {
			assertEquals(sens.Sensitivity, expected, NOTIONAL * TOL);
			return;
		  }
		}
		fail("Did not find sensitivity: " + type + " in " + points);
	  }

	  public virtual void test_presentValueSensitivityModelParamsSabr_atMaturity()
	  {
		PointSensitivities sensiRec = PRICER.presentValueSensitivityModelParamsSabr(SWAPTION_REC_LONG, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY).build();
		assertSensitivity(sensiRec, SabrParameterType.ALPHA, 0);
		assertSensitivity(sensiRec, SabrParameterType.BETA, 0);
		assertSensitivity(sensiRec, SabrParameterType.RHO, 0);
		assertSensitivity(sensiRec, SabrParameterType.NU, 0);
		PointSensitivities sensiPay = PRICER.presentValueSensitivityModelParamsSabr(SWAPTION_PAY_SHORT, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY).build();
		assertSensitivity(sensiPay, SabrParameterType.ALPHA, 0);
		assertSensitivity(sensiPay, SabrParameterType.BETA, 0);
		assertSensitivity(sensiPay, SabrParameterType.RHO, 0);
		assertSensitivity(sensiPay, SabrParameterType.NU, 0);
	  }

	  public virtual void test_presentValueSensitivityModelParamsSabr_afterMaturity()
	  {
		PointSensitivities sensiRec = PRICER.presentValueSensitivityModelParamsSabr(SWAPTION_REC_LONG, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY).build();
		assertEquals(sensiRec.Sensitivities.size(), 0);
		PointSensitivities sensiPay = PRICER.presentValueSensitivityModelParamsSabr(SWAPTION_PAY_SHORT, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY).build();
		assertEquals(sensiPay.Sensitivities.size(), 0);
	  }

	  public virtual void test_presentValueSensitivityModelParamsSabr_parity()
	  {
		PointSensitivities pvSensiRecLong = PRICER.presentValueSensitivityModelParamsSabr(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS).build();
		PointSensitivities pvSensiRecShort = PRICER.presentValueSensitivityModelParamsSabr(SWAPTION_REC_SHORT, RATE_PROVIDER, VOLS).build();
		PointSensitivities pvSensiPayLong = PRICER.presentValueSensitivityModelParamsSabr(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS).build();
		PointSensitivities pvSensiPayShort = PRICER.presentValueSensitivityModelParamsSabr(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS).build();

		assertSensitivity(pvSensiRecLong, pvSensiRecShort, SabrParameterType.ALPHA, -1);
		assertSensitivity(pvSensiPayLong, pvSensiPayShort, SabrParameterType.ALPHA, -1);
		assertSensitivity(pvSensiRecLong, pvSensiPayLong, SabrParameterType.ALPHA, 1);
		assertSensitivity(pvSensiPayShort, pvSensiPayShort, SabrParameterType.ALPHA, 1);

		assertSensitivity(pvSensiRecLong, pvSensiRecShort, SabrParameterType.BETA, -1);
		assertSensitivity(pvSensiPayLong, pvSensiPayShort, SabrParameterType.BETA, -1);
		assertSensitivity(pvSensiRecLong, pvSensiPayLong, SabrParameterType.BETA, 1);
		assertSensitivity(pvSensiPayShort, pvSensiPayShort, SabrParameterType.BETA, 1);

		assertSensitivity(pvSensiRecLong, pvSensiRecShort, SabrParameterType.RHO, -1);
		assertSensitivity(pvSensiPayLong, pvSensiPayShort, SabrParameterType.RHO, -1);
		assertSensitivity(pvSensiRecLong, pvSensiPayLong, SabrParameterType.RHO, 1);
		assertSensitivity(pvSensiPayShort, pvSensiPayShort, SabrParameterType.RHO, 1);

		assertSensitivity(pvSensiRecLong, pvSensiRecShort, SabrParameterType.NU, -1);
		assertSensitivity(pvSensiPayLong, pvSensiPayShort, SabrParameterType.NU, -1);
		assertSensitivity(pvSensiRecLong, pvSensiPayLong, SabrParameterType.NU, 1);
		assertSensitivity(pvSensiPayShort, pvSensiPayShort, SabrParameterType.NU, 1);
	  }

	  private void assertSensitivity(PointSensitivities points1, PointSensitivities points2, SabrParameterType type, int factor)
	  {

		// use ordinal() as a hack to find correct type
		assertEquals(points1.Sensitivities.get(type.ordinal()).Sensitivity, points2.Sensitivities.get(type.ordinal()).Sensitivity * factor, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void regressionPresentValue()
	  {
		CurrencyAmount pvLongPay = PRICER.presentValue(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS_REG);
		CurrencyAmount pvShortPay = PRICER.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS_REG);
		CurrencyAmount pvLongRec = PRICER.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS_REG);
		CurrencyAmount pvShortRec = PRICER.presentValue(SWAPTION_REC_SHORT, RATE_PROVIDER, VOLS_REG);
		assertEquals(pvLongPay.Amount, 2419978.690066857, NOTIONAL * TOL);
		assertEquals(pvShortPay.Amount, -2419978.690066857, NOTIONAL * TOL);
		assertEquals(pvLongRec.Amount, 3498144.2628540806, NOTIONAL * TOL);
		assertEquals(pvShortRec.Amount, -3498144.2628540806, NOTIONAL * TOL);
	  }

	  public virtual void regressionCurveSensitivity()
	  {
		double[] sensiDscExp = new double[] {0.0, 0.0, 0.0, 0.0, -1.1942174487944763E7, -1565567.6976298545};
		double[] sensiFwdExp = new double[] {0.0, 0.0, 0.0, 0.0, -2.3978768078237808E8, 4.8392987803482056E8};
		PointSensitivityBuilder point = PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS_REG);
		CurrencyParameterSensitivities sensi = RATE_PROVIDER.parameterSensitivity(point.build());
		double[] sensiDscCmp = sensi.getSensitivity(SwaptionSabrRateVolatilityDataSet.META_DSC_EUR.CurveName, EUR).Sensitivity.toArray();
		double[] sensiFwdCmp = sensi.getSensitivity(SwaptionSabrRateVolatilityDataSet.META_FWD_EUR.CurveName, EUR).Sensitivity.toArray();
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiDscCmp, sensiDscExp, TOL * NOTIONAL));
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiFwdCmp, sensiFwdExp, TOL * NOTIONAL));
	  }

	  public virtual void regressionSurfaceSensitivity()
	  {
		PointSensitivities pointComputed = PRICER.presentValueSensitivityModelParamsSabr(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS_REG).build();
		assertSensitivity(pointComputed, SabrParameterType.ALPHA, 4.862767907309804E7);
		assertSensitivity(pointComputed, SabrParameterType.BETA, -1.1095143998998241E7);
		assertSensitivity(pointComputed, SabrParameterType.RHO, 575158.6667143379);
		assertSensitivity(pointComputed, SabrParameterType.NU, 790627.3506603877);

		CurrencyParameterSensitivities sensiComputed = VOLS_REG.parameterSensitivity(pointComputed);
		double[][] alphaExp = new double[][]
		{
			new double[] {0.0, 0.0, 0.0},
			new double[] {0.5, 0.0, 0.0},
			new double[] {1.0, 0.0, 0.0},
			new double[] {2.0, 0.0, 0.0},
			new double[] {5.0, 0.0, 0.0},
			new double[] {10.0, 0.0, 0.0},
			new double[] {0.0, 1.0, 0.0},
			new double[] {0.5, 1.0, 0.0},
			new double[] {1.0, 1.0, 0.0},
			new double[] {2.0, 1.0, 0.0},
			new double[] {5.0, 1.0, 2.3882653164816026E7},
			new double[] {10.0, 1.0, 3132724.0980162215},
			new double[] {0.0, 10.0, 0.0},
			new double[] {0.5, 10.0, 0.0},
			new double[] {1.0, 10.0, 0.0},
			new double[] {2.0, 10.0, 0.0},
			new double[] {5.0, 10.0, 1.910612253185282E7},
			new double[] {10.0, 10.0, 2506179.2784129772},
			new double[] {0.0, 100.0, 0.0},
			new double[] {0.5, 100.0, 0.0},
			new double[] {1.0, 100.0, 0.0},
			new double[] {2.0, 100.0, 0.0},
			new double[] {5.0, 100.0, 0.0},
			new double[] {10.0, 100.0, 0.0}
		};
		double[][] betaExp = new double[][]
		{
			new double[] {0.0, 0.0, -0.0},
			new double[] {0.5, 0.0, -0.0},
			new double[] {1.0, 0.0, -0.0},
			new double[] {2.0, 0.0, -0.0},
			new double[] {5.0, 0.0, -0.0},
			new double[] {10.0, 0.0, -0.0},
			new double[] {100.0, 0.0, -0.0},
			new double[] {0.0, 1.0, -0.0},
			new double[] {0.5, 1.0, -0.0},
			new double[] {1.0, 1.0, -0.0},
			new double[] {2.0, 1.0, -0.0},
			new double[] {5.0, 1.0, -5449190.275839399},
			new double[] {10.0, 1.0, -714778.6124929579},
			new double[] {100.0, 1.0, -0.0},
			new double[] {0.0, 10.0, -0.0},
			new double[] {0.5, 10.0, -0.0},
			new double[] {1.0, 10.0, -0.0},
			new double[] {2.0, 10.0, -0.0},
			new double[] {5.0, 10.0, -4359352.220671519},
			new double[] {10.0, 10.0, -571822.8899943662},
			new double[] {100.0, 10.0, -0.0},
			new double[] {0.0, 100.0, -0.0},
			new double[] {0.5, 100.0, -0.0},
			new double[] {1.0, 100.0, -0.0},
			new double[] {2.0, 100.0, -0.0},
			new double[] {5.0, 100.0, -0.0},
			new double[] {10.0, 100.0, -0.0},
			new double[] {100.0, 100.0, -0.0}
		};
		double[][] rhoExp = new double[][]
		{
			new double[] {0.0, 0.0, 0.0},
			new double[] {0.5, 0.0, 0.0},
			new double[] {1.0, 0.0, 0.0},
			new double[] {2.0, 0.0, 0.0},
			new double[] {5.0, 0.0, 0.0},
			new double[] {10.0, 0.0, 0.0},
			new double[] {100.0, 0.0, 0.0},
			new double[] {0.0, 1.0, 0.0},
			new double[] {0.5, 1.0, 0.0},
			new double[] {1.0, 1.0, 0.0},
			new double[] {2.0, 1.0, 0.0},
			new double[] {5.0, 1.0, 282479.3453791586},
			new double[] {10.0, 1.0, 37053.24723991797},
			new double[] {100.0, 1.0, 0.0},
			new double[] {0.0, 10.0, 0.0},
			new double[] {1.0, 10.0, 0.0},
			new double[] {2.0, 10.0, 0.0},
			new double[] {0.5, 10.0, 0.0},
			new double[] {5.0, 10.0, 225983.4763033269},
			new double[] {10.0, 10.0, 29642.597791934375},
			new double[] {100.0, 10.0, 0.0},
			new double[] {0.0, 100.0, 0.0},
			new double[] {0.5, 100.0, 0.0},
			new double[] {1.0, 100.0, 0.0},
			new double[] {2.0, 100.0, 0.0},
			new double[] {5.0, 100.0, 0.0},
			new double[] {10.0, 100.0, 0.0},
			new double[] {100.0, 100.0, 0.0}
		};
		double[][] nuExp = new double[][]
		{
			new double[] {0.0, 0.0, 0.0},
			new double[] {0.5, 0.0, 0.0},
			new double[] {1.0, 0.0, 0.0},
			new double[] {2.0, 0.0, 0.0},
			new double[] {5.0, 0.0, 0.0},
			new double[] {10.0, 0.0, 0.0},
			new double[] {100.0, 0.0, 0.0},
			new double[] {0.0, 1.0, 0.0},
			new double[] {0.5, 1.0, 0.0},
			new double[] {1.0, 1.0, 0.0},
			new double[] {2.0, 1.0, 0.0},
			new double[] {5.0, 1.0, 388303.1055225815},
			new double[] {10.0, 1.0, 50934.31151096723},
			new double[] {100.0, 1.0, 0.0},
			new double[] {0.0, 10.0, 0.0},
			new double[] {0.5, 10.0, 0.0},
			new double[] {1.0, 10.0, 0.0},
			new double[] {2.0, 10.0, 0.0},
			new double[] {5.0, 10.0, 310642.48441806517},
			new double[] {10.0, 10.0, 40747.44920877378},
			new double[] {100.0, 10.0, 0.0},
			new double[] {0.0, 100.0, 0.0},
			new double[] {0.5, 100.0, 0.0},
			new double[] {1.0, 100.0, 0.0},
			new double[] {2.0, 100.0, 0.0},
			new double[] {5.0, 100.0, 0.0},
			new double[] {10.0, 100.0, 0.0},
			new double[] {100.0, 100.0, 0.0}
		};
		double[][][] exps = new double[][][] {alphaExp, betaExp, rhoExp, nuExp};
		SurfaceMetadata[] metadata = new SurfaceMetadata[] {SwaptionSabrRateVolatilityDataSet.META_ALPHA, SwaptionSabrRateVolatilityDataSet.META_BETA_EUR, SwaptionSabrRateVolatilityDataSet.META_RHO, SwaptionSabrRateVolatilityDataSet.META_NU};
		// x-y-value order does not match sorted order in surface, thus sort it
		CurrencyParameterSensitivities sensiExpected = CurrencyParameterSensitivities.empty();
		for (int i = 0; i < exps.Length; ++i)
		{
		  int size = exps[i].Length;
		  IDictionary<DoublesPair, double> sensiMap = new SortedDictionary<DoublesPair, double>();
		  for (int j = 0; j < size; ++j)
		  {
			sensiMap[DoublesPair.of(exps[i][j][0], exps[i][j][1])] = exps[i][j][2];
		  }
		  IList<ParameterMetadata> paramMetadata = new List<ParameterMetadata>(size);
		  IList<double> sensi = new List<double>();
		  foreach (KeyValuePair<DoublesPair, double> entry in sensiMap.SetOfKeyValuePairs())
		  {
			paramMetadata.Add(SwaptionSurfaceExpiryTenorParameterMetadata.of(entry.Key.First, entry.Key.Second));
			sensi.Add(entry.Value);
		  }
		  SurfaceMetadata surfaceMetadata = metadata[i].withParameterMetadata(paramMetadata);
		  sensiExpected = sensiExpected.combinedWith(CurrencyParameterSensitivity.of(surfaceMetadata.SurfaceName, surfaceMetadata.ParameterMetadata.get(), EUR, DoubleArray.copyOf(sensi)));
		}
		testSurfaceParameterSensitivities(sensiComputed, sensiExpected, TOL * NOTIONAL);
	  }

	  //-------------------------------------------------------------------------
	  private void testSurfaceParameterSensitivities(CurrencyParameterSensitivities computed, CurrencyParameterSensitivities expected, double tol)
	  {
		IList<CurrencyParameterSensitivity> listComputed = new List<CurrencyParameterSensitivity>(computed.Sensitivities);
		IList<CurrencyParameterSensitivity> listExpected = new List<CurrencyParameterSensitivity>(expected.Sensitivities);
		foreach (CurrencyParameterSensitivity sensExpected in listExpected)
		{
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		  int index = Math.Abs(Collections.binarySearch(listComputed, sensExpected, CurrencyParameterSensitivity::compareKey));
		  CurrencyParameterSensitivity sensComputed = listComputed[index];
		  int nSens = sensExpected.ParameterCount;
		  assertEquals(sensComputed.ParameterCount, nSens);
		  for (int i = 0; i < nSens; ++i)
		  {
			assertEquals(sensComputed.Sensitivity.get(i), sensExpected.Sensitivity.get(i), tol);
		  }
		  listComputed.RemoveAt(index);
		}
	  }

	}

}