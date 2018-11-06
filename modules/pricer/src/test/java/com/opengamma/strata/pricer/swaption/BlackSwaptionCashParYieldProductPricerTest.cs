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
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
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
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
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

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
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
	using FixedIborSwapConvention = com.opengamma.strata.product.swap.type.FixedIborSwapConvention;
	using FixedIborSwapConventions = com.opengamma.strata.product.swap.type.FixedIborSwapConventions;
	using CashSwaptionSettlement = com.opengamma.strata.product.swaption.CashSwaptionSettlement;
	using CashSwaptionSettlementMethod = com.opengamma.strata.product.swaption.CashSwaptionSettlementMethod;
	using PhysicalSwaptionSettlement = com.opengamma.strata.product.swaption.PhysicalSwaptionSettlement;
	using ResolvedSwaption = com.opengamma.strata.product.swaption.ResolvedSwaption;
	using Swaption = com.opengamma.strata.product.swaption.Swaption;

	/// <summary>
	/// Test <seealso cref="BlackSwaptionCashParYieldProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BlackSwaptionCashParYieldProductPricerTest
	public class BlackSwaptionCashParYieldProductPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = LocalDate.of(2012, 1, 10);
	  // curve
	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;
	  private static readonly DoubleArray DSC_TIME = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 10.0);
	  private static readonly DoubleArray DSC_RATE = DoubleArray.of(0.0150, 0.0125, 0.0150, 0.0175, 0.0150, 0.0150);
	  private static readonly CurveName DSC_NAME = CurveName.of("EUR Dsc");
	  private static readonly CurveMetadata META_DSC = Curves.zeroRates(DSC_NAME, ACT_ACT_ISDA);
	  private static readonly InterpolatedNodalCurve DSC_CURVE = InterpolatedNodalCurve.of(META_DSC, DSC_TIME, DSC_RATE, INTERPOLATOR);
	  private static readonly DoubleArray FWD6_TIME = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 10.0);
	  private static readonly DoubleArray FWD6_RATE = DoubleArray.of(0.0150, 0.0125, 0.0150, 0.0175, 0.0150, 0.0150);
	  private static readonly CurveName FWD6_NAME = CurveName.of("EUR EURIBOR 6M");
	  private static readonly CurveMetadata META_FWD6 = Curves.zeroRates(FWD6_NAME, ACT_ACT_ISDA);
	  private static readonly InterpolatedNodalCurve FWD6_CURVE = InterpolatedNodalCurve.of(META_FWD6, FWD6_TIME, FWD6_RATE, INTERPOLATOR);
	  private static readonly ImmutableRatesProvider RATE_PROVIDER = ImmutableRatesProvider.builder(VAL_DATE).discountCurve(EUR, DSC_CURVE).iborIndexCurve(EUR_EURIBOR_6M, FWD6_CURVE).build();
	  // surface
	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly DoubleArray EXPIRY = DoubleArray.of(0.5, 0.5, 1.0, 1.0, 5.0, 5.0);
	  private static readonly DoubleArray TENOR = DoubleArray.of(2, 10, 2, 10, 2, 10);
	  private static readonly DoubleArray VOL = DoubleArray.of(0.35, 0.30, 0.34, 0.25, 0.25, 0.20);
	  private static readonly FixedIborSwapConvention SWAP_CONVENTION = FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_6M;
	  private static readonly SurfaceMetadata METADATA = Surfaces.blackVolatilityByExpiryTenor("Black Vol", ACT_ACT_ISDA);
	  private static readonly Surface SURFACE = InterpolatedNodalSurface.of(METADATA, EXPIRY, TENOR, VOL, INTERPOLATOR_2D);
	  private static readonly BlackSwaptionExpiryTenorVolatilities VOLS = BlackSwaptionExpiryTenorVolatilities.of(SWAP_CONVENTION, VAL_DATE.atStartOfDay(ZoneOffset.UTC), SURFACE);
	  // underlying swap and swaption
	  private static readonly HolidayCalendarId CALENDAR = HolidayCalendarIds.SAT_SUN;
	  private static readonly BusinessDayAdjustment BDA_MF = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, CALENDAR);
	  private static readonly LocalDate MATURITY = BDA_MF.adjust(VAL_DATE.plusMonths(26), REF_DATA);
	  private static readonly LocalDate SETTLE = BDA_MF.adjust(CALENDAR.resolve(REF_DATA).shift(MATURITY, 2), REF_DATA);
	  private const double NOTIONAL = 123456789.0;
	  private static readonly LocalDate END = SETTLE.plusYears(5);
	  private const double RATE = 0.02;
	  private static readonly PeriodicSchedule PERIOD_FIXED = PeriodicSchedule.builder().startDate(SETTLE).endDate(END).frequency(P12M).businessDayAdjustment(BDA_MF).stubConvention(StubConvention.SHORT_FINAL).build();
	  private static readonly PaymentSchedule PAYMENT_FIXED = PaymentSchedule.builder().paymentFrequency(P12M).paymentDateOffset(DaysAdjustment.NONE).build();
	  private static readonly FixedRateCalculation RATE_FIXED = FixedRateCalculation.builder().dayCount(THIRTY_U_360).rate(ValueSchedule.of(RATE)).build();
	  private static readonly PeriodicSchedule PERIOD_IBOR = PeriodicSchedule.builder().startDate(SETTLE).endDate(END).frequency(P6M).businessDayAdjustment(BDA_MF).stubConvention(StubConvention.SHORT_FINAL).build();
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
	  private static readonly ResolvedSwaption SWAPTION_REC_LONG = Swaption.builder().expiryDate(AdjustableDate.of(MATURITY, BDA_MF)).expiryTime(LocalTime.NOON).expiryZone(ZoneOffset.UTC).swaptionSettlement(PAR_YIELD).longShort(LONG).underlying(SWAP_REC).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_REC_SHORT = Swaption.builder().expiryDate(AdjustableDate.of(MATURITY, BDA_MF)).expiryTime(LocalTime.NOON).expiryZone(ZoneOffset.UTC).swaptionSettlement(PAR_YIELD).longShort(SHORT).underlying(SWAP_REC).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_PAY_LONG = Swaption.builder().expiryDate(AdjustableDate.of(MATURITY, BDA_MF)).expiryTime(LocalTime.NOON).expiryZone(ZoneOffset.UTC).swaptionSettlement(PAR_YIELD).longShort(LONG).underlying(SWAP_PAY).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_PAY_SHORT = Swaption.builder().expiryDate(AdjustableDate.of(MATURITY, BDA_MF)).expiryTime(LocalTime.NOON).expiryZone(ZoneOffset.UTC).swaptionSettlement(PAR_YIELD).longShort(SHORT).underlying(SWAP_PAY).build().resolve(REF_DATA);
	  // providers used for specific tests
	  private static readonly ImmutableRatesProvider RATES_PROVIDER_AT_MATURITY = ImmutableRatesProvider.builder(MATURITY).discountCurve(EUR, DSC_CURVE).iborIndexCurve(EUR_EURIBOR_6M, FWD6_CURVE).build();
	  private static readonly ImmutableRatesProvider RATES_PROVIDER_AFTER_MATURITY = ImmutableRatesProvider.builder(MATURITY.plusDays(1)).discountCurve(EUR, DSC_CURVE).iborIndexCurve(EUR_EURIBOR_6M, FWD6_CURVE).build();
	  private static readonly BlackSwaptionExpiryTenorVolatilities VOLS_AT_MATURITY = BlackSwaptionExpiryTenorVolatilities.of(SWAP_CONVENTION, MATURITY.atStartOfDay(ZoneOffset.UTC), SURFACE);
	  private static readonly BlackSwaptionExpiryTenorVolatilities VOLS_AFTER_MATURITY = BlackSwaptionExpiryTenorVolatilities.of(SWAP_CONVENTION, MATURITY.plusDays(1).atStartOfDay(ZoneOffset.UTC), SURFACE);
	  // test parameters
	  private const double TOL = 1.0e-12;
	  private const double FD_EPS = 1.0e-7;
	  // pricer
	  private static readonly BlackSwaptionCashParYieldProductPricer PRICER = BlackSwaptionCashParYieldProductPricer.DEFAULT;
	  private static readonly DiscountingSwapProductPricer SWAP_PRICER = DiscountingSwapProductPricer.DEFAULT;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FD_CAL = new RatesFiniteDifferenceSensitivityCalculator(FD_EPS);

	  public virtual void test_presentValue()
	  {
		CurrencyAmount computedRec = PRICER.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount computedPay = PRICER.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		double forward = SWAP_PRICER.parRate(RSWAP_REC, RATE_PROVIDER);
		double annuityCash = SWAP_PRICER.LegPricer.annuityCash(RFIXED_LEG_REC, forward);
		double expiry = VOLS.relativeTime(SWAPTION_REC_LONG.Expiry);
		double tenor = VOLS.tenor(SETTLE, END);
		double volatility = SURFACE.zValue(expiry, tenor);
		double settle = ACT_ACT_ISDA.relativeYearFraction(VAL_DATE, SETTLE);
		double df = Math.Exp(-DSC_CURVE.yValue(settle) * settle);
		double expectedRec = df * annuityCash * BlackFormulaRepository.price(forward, RATE, expiry, volatility, false);
		double expectedPay = -df * annuityCash * BlackFormulaRepository.price(forward, RATE, expiry, volatility, true);
		assertEquals(computedRec.Currency, EUR);
		assertEquals(computedRec.Amount, expectedRec, NOTIONAL * TOL);
		assertEquals(computedPay.Currency, EUR);
		assertEquals(computedPay.Amount, expectedPay, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_atMaturity()
	  {
		CurrencyAmount computedRec = PRICER.presentValue(SWAPTION_REC_LONG, RATES_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		CurrencyAmount computedPay = PRICER.presentValue(SWAPTION_PAY_SHORT, RATES_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		double forward = SWAP_PRICER.parRate(RSWAP_REC, RATES_PROVIDER_AT_MATURITY);
		double annuityCash = SWAP_PRICER.LegPricer.annuityCash(RFIXED_LEG_REC, forward);
		double settle = ACT_ACT_ISDA.relativeYearFraction(MATURITY, SETTLE);
		double df = Math.Exp(-DSC_CURVE.yValue(settle) * settle);
		assertEquals(computedRec.Amount, df * annuityCash * (RATE - forward), NOTIONAL * TOL);
		assertEquals(computedPay.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_afterMaturity()
	  {
		CurrencyAmount computedRec = PRICER.presentValue(SWAPTION_REC_LONG, RATES_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		CurrencyAmount computedPay = PRICER.presentValue(SWAPTION_PAY_SHORT, RATES_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
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
		double forward = SWAP_PRICER.parRate(RSWAP_REC, RATE_PROVIDER);
		double annuityCash = SWAP_PRICER.LegPricer.annuityCash(RSWAP_REC.getLegs(SwapLegType.FIXED).get(0), forward);
		double discount = RATE_PROVIDER.discountFactor(EUR, SETTLE);
		double expected = discount * annuityCash * (forward - RATE);
		assertEquals(pvPayLong.Amount - pvRecLong.Amount, expected, NOTIONAL * TOL);
		assertEquals(pvPayShort.Amount - pvRecShort.Amount, -expected, NOTIONAL * TOL);
	  }

	  public virtual void test_physicalSettlement()
	  {
		Swaption swaption = Swaption.builder().expiryDate(AdjustableDate.of(MATURITY, BDA_MF)).expiryTime(LocalTime.NOON).expiryZone(ZoneOffset.UTC).swaptionSettlement(PhysicalSwaptionSettlement.DEFAULT).longShort(LONG).underlying(SWAP_PAY).build();
		assertThrowsIllegalArg(() => PRICER.impliedVolatility(swaption.resolve(REF_DATA), RATE_PROVIDER, VOLS));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueDelta()
	  {
		CurrencyAmount computedRec = PRICER.presentValueDelta(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount computedPay = PRICER.presentValueDelta(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		double forward = SWAP_PRICER.parRate(RSWAP_REC, RATE_PROVIDER);
		double annuityCash = SWAP_PRICER.LegPricer.annuityCash(RFIXED_LEG_REC, forward);
		double expiry = VOLS.relativeTime(SWAPTION_REC_LONG.Expiry);
		double tenor = VOLS.tenor(SETTLE, END);
		double volatility = SURFACE.zValue(expiry, tenor);
		double settle = ACT_ACT_ISDA.relativeYearFraction(VAL_DATE, SETTLE);
		double df = Math.Exp(-DSC_CURVE.yValue(settle) * settle);
		double expectedRec = df * annuityCash * BlackFormulaRepository.delta(forward, RATE, expiry, volatility, false);
		double expectedPay = -df * annuityCash * BlackFormulaRepository.delta(forward, RATE, expiry, volatility, true);
		assertEquals(computedRec.Currency, EUR);
		assertEquals(computedRec.Amount, expectedRec, NOTIONAL * TOL);
		assertEquals(computedPay.Currency, EUR);
		assertEquals(computedPay.Amount, expectedPay, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueDelta_atMaturity()
	  {
		CurrencyAmount computedRec = PRICER.presentValueDelta(SWAPTION_REC_LONG, RATES_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		CurrencyAmount computedPay = PRICER.presentValueDelta(SWAPTION_PAY_SHORT, RATES_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		double forward = SWAP_PRICER.parRate(RSWAP_REC, RATES_PROVIDER_AT_MATURITY);
		double annuityCash = SWAP_PRICER.LegPricer.annuityCash(RFIXED_LEG_REC, forward);
		double settle = ACT_ACT_ISDA.relativeYearFraction(MATURITY, SETTLE);
		double df = Math.Exp(-DSC_CURVE.yValue(settle) * settle);
		assertEquals(computedRec.Amount, -df * annuityCash, NOTIONAL * TOL);
		assertEquals(computedPay.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueDelta_afterMaturity()
	  {
		CurrencyAmount computedRec = PRICER.presentValueDelta(SWAPTION_REC_LONG, RATES_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		CurrencyAmount computedPay = PRICER.presentValueDelta(SWAPTION_PAY_SHORT, RATES_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		assertEquals(computedRec.Amount, 0d, NOTIONAL * TOL);
		assertEquals(computedPay.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueDelta_parity()
	  {
		CurrencyAmount pvDeltaRecLong = PRICER.presentValueDelta(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount pvDeltaRecShort = PRICER.presentValueDelta(SWAPTION_REC_SHORT, RATE_PROVIDER, VOLS);
		CurrencyAmount pvDeltaPayLong = PRICER.presentValueDelta(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount pvDeltaPayShort = PRICER.presentValueDelta(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		assertEquals(pvDeltaRecLong.Amount, -pvDeltaRecShort.Amount, NOTIONAL * TOL);
		assertEquals(pvDeltaPayLong.Amount, -pvDeltaPayShort.Amount, NOTIONAL * TOL);
		double forward = SWAP_PRICER.parRate(RSWAP_REC, RATE_PROVIDER);
		double annuityCash = SWAP_PRICER.LegPricer.annuityCash(RSWAP_REC.getLegs(SwapLegType.FIXED).get(0), forward);
		double discount = RATE_PROVIDER.discountFactor(EUR, SETTLE);
		double expected = discount * annuityCash;
		assertEquals(pvDeltaPayLong.Amount - pvDeltaRecLong.Amount, expected, NOTIONAL * TOL);
		assertEquals(pvDeltaPayShort.Amount - pvDeltaRecShort.Amount, -expected, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueGamma()
	  {
		CurrencyAmount computedRec = PRICER.presentValueGamma(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount computedPay = PRICER.presentValueGamma(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		double forward = SWAP_PRICER.parRate(RSWAP_REC, RATE_PROVIDER);
		double annuityCash = SWAP_PRICER.LegPricer.annuityCash(RFIXED_LEG_REC, forward);
		double expiry = VOLS.relativeTime(SWAPTION_REC_LONG.Expiry);
		double tenor = VOLS.tenor(SETTLE, END);
		double volatility = SURFACE.zValue(expiry, tenor);
		double settle = ACT_ACT_ISDA.relativeYearFraction(VAL_DATE, SETTLE);
		double df = Math.Exp(-DSC_CURVE.yValue(settle) * settle);
		double expectedRec = df * annuityCash * BlackFormulaRepository.gamma(forward, RATE, expiry, volatility);
		double expectedPay = -df * annuityCash * BlackFormulaRepository.gamma(forward, RATE, expiry, volatility);
		assertEquals(computedRec.Currency, EUR);
		assertEquals(computedRec.Amount, expectedRec, NOTIONAL * TOL);
		assertEquals(computedPay.Currency, EUR);
		assertEquals(computedPay.Amount, expectedPay, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueGamma_atMaturity()
	  {
		CurrencyAmount computedRec = PRICER.presentValueGamma(SWAPTION_REC_LONG, RATES_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		CurrencyAmount computedPay = PRICER.presentValueGamma(SWAPTION_PAY_SHORT, RATES_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		assertEquals(computedRec.Amount, 0d, NOTIONAL * TOL);
		assertEquals(computedPay.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueGamma_afterMaturity()
	  {
		CurrencyAmount computedRec = PRICER.presentValueGamma(SWAPTION_REC_LONG, RATES_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		CurrencyAmount computedPay = PRICER.presentValueGamma(SWAPTION_PAY_SHORT, RATES_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		assertEquals(computedRec.Amount, 0d, NOTIONAL * TOL);
		assertEquals(computedPay.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueGamma_parity()
	  {
		CurrencyAmount pvGammaRecLong = PRICER.presentValueGamma(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount pvGammaRecShort = PRICER.presentValueGamma(SWAPTION_REC_SHORT, RATE_PROVIDER, VOLS);
		CurrencyAmount pvGammaPayLong = PRICER.presentValueGamma(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount pvGammaPayShort = PRICER.presentValueGamma(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		assertEquals(pvGammaRecLong.Amount, -pvGammaRecShort.Amount, NOTIONAL * TOL);
		assertEquals(pvGammaPayLong.Amount, -pvGammaPayShort.Amount, NOTIONAL * TOL);
		assertEquals(pvGammaPayLong.Amount, pvGammaRecLong.Amount, NOTIONAL * TOL);
		assertEquals(pvGammaPayShort.Amount, pvGammaRecShort.Amount, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueTheta()
	  {
		CurrencyAmount computedRec = PRICER.presentValueTheta(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount computedPay = PRICER.presentValueTheta(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		double forward = SWAP_PRICER.parRate(RSWAP_REC, RATE_PROVIDER);
		double annuityCash = SWAP_PRICER.LegPricer.annuityCash(RFIXED_LEG_REC, forward);
		double expiry = VOLS.relativeTime(SWAPTION_REC_LONG.Expiry);
		double tenor = VOLS.tenor(SETTLE, END);
		double volatility = SURFACE.zValue(expiry, tenor);
		double settle = ACT_ACT_ISDA.relativeYearFraction(VAL_DATE, SETTLE);
		double df = Math.Exp(-DSC_CURVE.yValue(settle) * settle);
		double expectedRec = df * annuityCash * BlackFormulaRepository.driftlessTheta(forward, RATE, expiry, volatility);
		double expectedPay = -df * annuityCash * BlackFormulaRepository.driftlessTheta(forward, RATE, expiry, volatility);
		assertEquals(computedRec.Currency, EUR);
		assertEquals(computedRec.Amount, expectedRec, NOTIONAL * TOL);
		assertEquals(computedPay.Currency, EUR);
		assertEquals(computedPay.Amount, expectedPay, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueTheta_atMaturity()
	  {
		CurrencyAmount computedRec = PRICER.presentValueTheta(SWAPTION_REC_LONG, RATES_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		CurrencyAmount computedPay = PRICER.presentValueTheta(SWAPTION_PAY_SHORT, RATES_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		assertEquals(computedRec.Amount, 0d, NOTIONAL * TOL);
		assertEquals(computedPay.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueTheta_afterMaturity()
	  {
		CurrencyAmount computedRec = PRICER.presentValueTheta(SWAPTION_REC_LONG, RATES_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		CurrencyAmount computedPay = PRICER.presentValueTheta(SWAPTION_PAY_SHORT, RATES_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		assertEquals(computedRec.Amount, 0d, NOTIONAL * TOL);
		assertEquals(computedPay.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueTheta_parity()
	  {
		CurrencyAmount pvThetaRecLong = PRICER.presentValueTheta(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount pvThetaRecShort = PRICER.presentValueTheta(SWAPTION_REC_SHORT, RATE_PROVIDER, VOLS);
		CurrencyAmount pvThetaPayLong = PRICER.presentValueTheta(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount pvThetaPayShort = PRICER.presentValueTheta(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		assertEquals(pvThetaRecLong.Amount, -pvThetaRecShort.Amount, NOTIONAL * TOL);
		assertEquals(pvThetaPayLong.Amount, -pvThetaPayShort.Amount, NOTIONAL * TOL);
		assertEquals(pvThetaPayLong.Amount, pvThetaRecLong.Amount, NOTIONAL * TOL);
		assertEquals(pvThetaPayShort.Amount, pvThetaRecShort.Amount, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currencyExposure()
	  {
		MultiCurrencyAmount computedRec = PRICER.currencyExposure(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount computedPay = PRICER.currencyExposure(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		PointSensitivityBuilder pointRec = PRICER.presentValueSensitivityRatesStickyStrike(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount expectedRec = RATE_PROVIDER.currencyExposure(pointRec.build()).plus(PRICER.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS));
		assertEquals(computedRec.size(), 1);
		assertEquals(computedRec.getAmount(EUR).Amount, expectedRec.getAmount(EUR).Amount, NOTIONAL * TOL);
		PointSensitivityBuilder pointPay = PRICER.presentValueSensitivityRatesStickyStrike(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount expectedPay = RATE_PROVIDER.currencyExposure(pointPay.build()).plus(PRICER.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS));
		assertEquals(computedPay.size(), 1);
		assertEquals(computedPay.getAmount(EUR).Amount, expectedPay.getAmount(EUR).Amount, NOTIONAL * TOL);
	  }

	  public virtual void test_currencyExposure_atMaturity()
	  {
		MultiCurrencyAmount computedRec = PRICER.currencyExposure(SWAPTION_REC_LONG, RATES_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		MultiCurrencyAmount computedPay = PRICER.currencyExposure(SWAPTION_PAY_SHORT, RATES_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		PointSensitivityBuilder pointRec = PRICER.presentValueSensitivityRatesStickyStrike(SWAPTION_REC_LONG, RATES_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		MultiCurrencyAmount expectedRec = RATE_PROVIDER.currencyExposure(pointRec.build()).plus(PRICER.presentValue(SWAPTION_REC_LONG, RATES_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY));
		assertEquals(computedRec.size(), 1);
		assertEquals(computedRec.getAmount(EUR).Amount, expectedRec.getAmount(EUR).Amount, NOTIONAL * TOL);
		PointSensitivityBuilder pointPay = PRICER.presentValueSensitivityRatesStickyStrike(SWAPTION_PAY_SHORT, RATES_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		MultiCurrencyAmount expectedPay = RATE_PROVIDER.currencyExposure(pointPay.build()).plus(PRICER.presentValue(SWAPTION_PAY_SHORT, RATES_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY));
		assertEquals(computedPay.size(), 1);
		assertEquals(computedPay.getAmount(EUR).Amount, expectedPay.getAmount(EUR).Amount, NOTIONAL * TOL);
	  }

	  public virtual void test_currencyExposure_afterMaturity()
	  {
		MultiCurrencyAmount computedRec = PRICER.currencyExposure(SWAPTION_REC_LONG, RATES_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		MultiCurrencyAmount computedPay = PRICER.currencyExposure(SWAPTION_PAY_SHORT, RATES_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
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
		double expiry = VOLS.relativeTime(SWAPTION_REC_LONG.Expiry);
		double tenor = VOLS.tenor(SETTLE, END);
		double expected = SURFACE.zValue(expiry, tenor);
		assertEquals(computedRec, expected);
		assertEquals(computedPay, expected);
	  }

	  public virtual void test_impliedVolatility_atMaturity()
	  {
		double computedRec = PRICER.impliedVolatility(SWAPTION_REC_LONG, RATES_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		double computedPay = PRICER.impliedVolatility(SWAPTION_PAY_SHORT, RATES_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		double expiry = 0d;
		double tenor = VOLS.tenor(SETTLE, END);
		double expected = SURFACE.zValue(expiry, tenor);
		assertEquals(computedRec, expected);
		assertEquals(computedPay, expected);
	  }

	  public virtual void test_impliedVolatility_afterMaturity()
	  {
		assertThrowsIllegalArg(() => PRICER.impliedVolatility(SWAPTION_REC_LONG, RATES_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY));
		assertThrowsIllegalArg(() => PRICER.impliedVolatility(SWAPTION_PAY_SHORT, RATES_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivityRatesStickyStrike()
	  {
		PointSensitivityBuilder pointRec = PRICER.presentValueSensitivityRatesStickyStrike(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyParameterSensitivities computedRec = RATE_PROVIDER.parameterSensitivity(pointRec.build());
		CurrencyParameterSensitivities expectedRec = FD_CAL.sensitivity(RATE_PROVIDER, (p) => PRICER.presentValue(SWAPTION_REC_LONG, (p), VOLS));
		assertTrue(computedRec.equalWithTolerance(expectedRec, NOTIONAL * FD_EPS * 100d));
		PointSensitivityBuilder pointPay = PRICER.presentValueSensitivityRatesStickyStrike(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		CurrencyParameterSensitivities computedPay = RATE_PROVIDER.parameterSensitivity(pointPay.build());
		CurrencyParameterSensitivities expectedPay = FD_CAL.sensitivity(RATE_PROVIDER, (p) => PRICER.presentValue(SWAPTION_PAY_SHORT, (p), VOLS));
		assertTrue(computedPay.equalWithTolerance(expectedPay, NOTIONAL * FD_EPS * 100d));
	  }

	  public virtual void test_presentValueSensitivityRatesStickyStrike_atMaturity()
	  {
		PointSensitivityBuilder pointRec = PRICER.presentValueSensitivityRatesStickyStrike(SWAPTION_REC_LONG, RATES_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		CurrencyParameterSensitivities computedRec = RATES_PROVIDER_AT_MATURITY.parameterSensitivity(pointRec.build());
		CurrencyParameterSensitivities expectedRec = FD_CAL.sensitivity(RATES_PROVIDER_AT_MATURITY, (p) => PRICER.presentValue(SWAPTION_REC_LONG, (p), VOLS_AT_MATURITY));
		assertTrue(computedRec.equalWithTolerance(expectedRec, NOTIONAL * FD_EPS * 100d));
		PointSensitivities pointPay = PRICER.presentValueSensitivityRatesStickyStrike(SWAPTION_PAY_SHORT, RATES_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY).build();
		foreach (PointSensitivity sensi in pointPay.Sensitivities)
		{
		  assertEquals(Math.Abs(sensi.Sensitivity), 0d);
		}
	  }

	  public virtual void test_presentValueSensitivityRatesStickyStrike_afterMaturity()
	  {
		PointSensitivities pointRec = PRICER.presentValueSensitivityRatesStickyStrike(SWAPTION_REC_LONG, RATES_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY).build();
		foreach (PointSensitivity sensi in pointRec.Sensitivities)
		{
		  assertEquals(Math.Abs(sensi.Sensitivity), 0d);
		}
		PointSensitivities pointPay = PRICER.presentValueSensitivityRatesStickyStrike(SWAPTION_PAY_SHORT, RATES_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY).build();
		foreach (PointSensitivity sensi in pointPay.Sensitivities)
		{
		  assertEquals(Math.Abs(sensi.Sensitivity), 0d);
		}
	  }

	  public virtual void test_presentValueSensitivityRatesStickyStrike_parity()
	  {
		CurrencyParameterSensitivities pvSensiRecLong = RATE_PROVIDER.parameterSensitivity(PRICER.presentValueSensitivityRatesStickyStrike(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS).build());
		CurrencyParameterSensitivities pvSensiRecShort = RATE_PROVIDER.parameterSensitivity(PRICER.presentValueSensitivityRatesStickyStrike(SWAPTION_REC_SHORT, RATE_PROVIDER, VOLS).build());
		CurrencyParameterSensitivities pvSensiPayLong = RATE_PROVIDER.parameterSensitivity(PRICER.presentValueSensitivityRatesStickyStrike(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS).build());
		CurrencyParameterSensitivities pvSensiPayShort = RATE_PROVIDER.parameterSensitivity(PRICER.presentValueSensitivityRatesStickyStrike(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS).build());
		assertTrue(pvSensiRecLong.equalWithTolerance(pvSensiRecShort.multipliedBy(-1d), NOTIONAL * TOL));
		assertTrue(pvSensiPayLong.equalWithTolerance(pvSensiPayShort.multipliedBy(-1d), NOTIONAL * TOL));

		double forward = SWAP_PRICER.parRate(RSWAP_REC, RATE_PROVIDER);
		PointSensitivityBuilder forwardSensi = SWAP_PRICER.parRateSensitivity(RSWAP_REC, RATE_PROVIDER);
		double annuityCash = SWAP_PRICER.LegPricer.annuityCash(RSWAP_REC.getLegs(SwapLegType.FIXED).get(0), forward);
		double annuityCashDeriv = SWAP_PRICER.LegPricer.annuityCashDerivative(RSWAP_REC.getLegs(SwapLegType.FIXED).get(0), forward).getDerivative(0);
		double discount = RATE_PROVIDER.discountFactor(EUR, SETTLE);
		PointSensitivityBuilder discountSensi = RATE_PROVIDER.discountFactors(EUR).zeroRatePointSensitivity(SETTLE);
		PointSensitivities expecedPoint = discountSensi.multipliedBy(annuityCash * (forward - RATE)).combinedWith(forwardSensi.multipliedBy(discount * annuityCash + discount * annuityCashDeriv * (forward - RATE))).build();
		CurrencyParameterSensitivities expected = RATE_PROVIDER.parameterSensitivity(expecedPoint);
		assertTrue(expected.equalWithTolerance(pvSensiPayLong.combinedWith(pvSensiRecLong.multipliedBy(-1d)), NOTIONAL * TOL));
		assertTrue(expected.equalWithTolerance(pvSensiRecShort.combinedWith(pvSensiPayShort.multipliedBy(-1d)), NOTIONAL * TOL));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivityBlackVolatility()
	  {
		SwaptionSensitivity sensiRec = PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		SwaptionSensitivity sensiPay = PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		double forward = SWAP_PRICER.parRate(RSWAP_REC, RATE_PROVIDER);
		double annuityCash = SWAP_PRICER.LegPricer.annuityCash(RFIXED_LEG_REC, forward);
		double expiry = VOLS.relativeTime(SWAPTION_REC_LONG.Expiry);
		double tenor = VOLS.tenor(SETTLE, END);
		double volatility = SURFACE.zValue(expiry, tenor);
		double settle = ACT_ACT_ISDA.relativeYearFraction(VAL_DATE, SETTLE);
		double df = Math.Exp(-DSC_CURVE.yValue(settle) * settle);
		double expectedRec = df * annuityCash * BlackFormulaRepository.vega(forward, RATE, expiry, volatility);
		double expectedPay = -df * annuityCash * BlackFormulaRepository.vega(forward, RATE, expiry, volatility);
		assertEquals(sensiRec.Currency, EUR);
		assertEquals(sensiRec.Sensitivity, expectedRec, NOTIONAL * TOL);
		assertEquals(sensiRec.VolatilitiesName, VOLS.Name);
		assertEquals(sensiRec.Expiry, expiry);
		assertEquals(sensiRec.Tenor, 5.0);
		assertEquals(sensiRec.Strike, RATE);
		assertEquals(sensiRec.Forward, forward, TOL);
		assertEquals(sensiPay.Currency, EUR);
		assertEquals(sensiPay.Sensitivity, expectedPay, NOTIONAL * TOL);
		assertEquals(sensiRec.VolatilitiesName, VOLS.Name);
		assertEquals(sensiPay.Expiry, expiry);
		assertEquals(sensiPay.Tenor, 5.0);
		assertEquals(sensiPay.Strike, RATE);
		assertEquals(sensiPay.Forward, forward, TOL);
	  }

	  public virtual void test_presentValueSensitivityBlackVolatility_atMaturity()
	  {
		SwaptionSensitivity sensiRec = PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_REC_LONG, RATES_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		assertEquals(sensiRec.Sensitivity, 0d, NOTIONAL * TOL);
		SwaptionSensitivity sensiPay = PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_PAY_SHORT, RATES_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		assertEquals(sensiPay.Sensitivity, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueSensitivityBlackVolatility_afterMaturity()
	  {
		SwaptionSensitivity sensiRec = PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_REC_LONG, RATES_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		assertEquals(sensiRec.Sensitivity, 0d, NOTIONAL * TOL);
		SwaptionSensitivity sensiPay = PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_PAY_SHORT, RATES_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		assertEquals(sensiPay.Sensitivity, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueSensitivityBlackVolatility_parity()
	  {
		SwaptionSensitivity pvSensiRecLong = PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		SwaptionSensitivity pvSensiRecShort = PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_REC_SHORT, RATE_PROVIDER, VOLS);
		SwaptionSensitivity pvSensiPayLong = PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS);
		SwaptionSensitivity pvSensiPayShort = PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		assertEquals(pvSensiRecLong.Sensitivity, -pvSensiRecShort.Sensitivity, NOTIONAL * TOL);
		assertEquals(pvSensiPayLong.Sensitivity, -pvSensiPayShort.Sensitivity, NOTIONAL * TOL);
		assertEquals(pvSensiRecLong.Sensitivity, pvSensiPayLong.Sensitivity, NOTIONAL * TOL);
		assertEquals(pvSensiPayShort.Sensitivity, pvSensiPayShort.Sensitivity, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void pvRegression()
	  {
		CurrencyAmount pv = PRICER.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		assertEquals(pv.Amount, 3823688.253812721, NOTIONAL * TOL); // 2.x
	  }

	  public virtual void pvCurveSensiRegression()
	  {
		PointSensitivityBuilder point = PRICER.presentValueSensitivityRatesStickyStrike(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyParameterSensitivities computed = RATE_PROVIDER.parameterSensitivity(point.build());
		computed.getSensitivity(DSC_NAME, EUR).Sensitivity;
		DoubleArray dscSensi = DoubleArray.of(0.0, 0.0, 0.0, -7143525.908886078, -1749520.4110068753, -719115.4683096837); // 2.x
		DoubleArray fwdSensi = DoubleArray.of(0d, 0d, 0d, 1.7943318714062232E8, -3.4987983718159467E8, -2.6516758066404995E8); // 2.x
		CurrencyParameterSensitivity dsc = DSC_CURVE.createParameterSensitivity(EUR, dscSensi);
		CurrencyParameterSensitivity fwd = FWD6_CURVE.createParameterSensitivity(EUR, fwdSensi);
		CurrencyParameterSensitivities expected = CurrencyParameterSensitivities.of(ImmutableList.of(dsc, fwd));
		assertTrue(computed.equalWithTolerance(expected, NOTIONAL * TOL));
	  }

	}

}