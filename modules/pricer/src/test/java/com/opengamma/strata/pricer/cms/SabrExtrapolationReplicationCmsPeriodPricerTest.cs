using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.cms
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.SwapIndices.EUR_EURIBOR_1100_5Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using RungeKuttaIntegrator1D = com.opengamma.strata.math.impl.integration.RungeKuttaIntegrator1D;
	using SabrExtrapolationRightFunction = com.opengamma.strata.pricer.impl.option.SabrExtrapolationRightFunction;
	using SabrFormulaData = com.opengamma.strata.pricer.impl.volatility.smile.SabrFormulaData;
	using SabrInterestRateParameters = com.opengamma.strata.pricer.model.SabrInterestRateParameters;
	using SabrVolatilityFormula = com.opengamma.strata.pricer.model.SabrVolatilityFormula;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using SabrParametersSwaptionVolatilities = com.opengamma.strata.pricer.swaption.SabrParametersSwaptionVolatilities;
	using SwaptionSabrRateVolatilityDataSet = com.opengamma.strata.pricer.swaption.SwaptionSabrRateVolatilityDataSet;
	using SwaptionVolatilitiesName = com.opengamma.strata.pricer.swaption.SwaptionVolatilitiesName;
	using CmsPeriod = com.opengamma.strata.product.cms.CmsPeriod;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using PutCall = com.opengamma.strata.product.common.PutCall;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapIndex = com.opengamma.strata.product.swap.SwapIndex;
	using SwapLegType = com.opengamma.strata.product.swap.SwapLegType;
	using FixedIborSwapConvention = com.opengamma.strata.product.swap.type.FixedIborSwapConvention;

	/// <summary>
	/// Test <seealso cref="SabrExtrapolationReplicationCmsPeriodPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrExtrapolationReplicationCmsPeriodPricerTest
	public class SabrExtrapolationReplicationCmsPeriodPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VALUATION = LocalDate.of(2010, 8, 18);
	  private static readonly LocalDate FIXING = LocalDate.of(2020, 4, 24);
	  private static readonly LocalDate START = LocalDate.of(2020, 4, 28);
	  private static readonly LocalDate END = LocalDate.of(2021, 4, 28);
	  private static readonly LocalDate AFTER_FIXING = LocalDate.of(2020, 8, 11);
	  private static readonly LocalDate PAYMENT = LocalDate.of(2021, 4, 28);
	  private static readonly LocalDate AFTER_PAYMENT = LocalDate.of(2021, 4, 29);

	  // providers
	  private static readonly ImmutableRatesProvider RATES_PROVIDER = SwaptionSabrRateVolatilityDataSet.getRatesProviderEur(VALUATION);
	  private static readonly SabrParametersSwaptionVolatilities VOLATILITIES = SwaptionSabrRateVolatilityDataSet.getVolatilitiesEur(VALUATION, false);
	  private static readonly SabrParametersSwaptionVolatilities VOLATILITIES_SHIFT = SwaptionSabrRateVolatilityDataSet.getVolatilitiesEur(VALUATION, true);
	  private static readonly double SHIFT = VOLATILITIES_SHIFT.Parameters.ShiftSurface.getParameter(0); // constant surface
	  private const double OBS_INDEX = 0.0135;
	  private static readonly LocalDateDoubleTimeSeries TIME_SERIES = LocalDateDoubleTimeSeries.of(FIXING, OBS_INDEX);
	  // providers - on fixing date, no time series
	  private static readonly ImmutableRatesProvider RATES_PROVIDER_ON_FIX = SwaptionSabrRateVolatilityDataSet.getRatesProviderEur(FIXING);
	  private static readonly SabrParametersSwaptionVolatilities VOLATILITIES_ON_FIX = SwaptionSabrRateVolatilityDataSet.getVolatilitiesEur(FIXING, true);
	  // providers - after fixing date, no time series
	  private static readonly ImmutableRatesProvider RATES_PROVIDER_NO_TS = SwaptionSabrRateVolatilityDataSet.getRatesProviderEur(AFTER_FIXING);
	  private static readonly SabrParametersSwaptionVolatilities VOLATILITIES_NO_TS = SwaptionSabrRateVolatilityDataSet.getVolatilitiesEur(AFTER_FIXING, true);
	  // providers - between fixing date and payment date
	  private static readonly ImmutableRatesProvider RATES_PROVIDER_AFTER_FIX = SwaptionSabrRateVolatilityDataSet.getRatesProviderEur(AFTER_FIXING, TIME_SERIES);
	  private static readonly SabrParametersSwaptionVolatilities VOLATILITIES_AFTER_FIX = SwaptionSabrRateVolatilityDataSet.getVolatilitiesEur(AFTER_FIXING, true);
	  // providers - on payment date
	  private static readonly ImmutableRatesProvider RATES_PROVIDER_ON_PAY = SwaptionSabrRateVolatilityDataSet.getRatesProviderEur(PAYMENT, TIME_SERIES);
	  private static readonly SabrParametersSwaptionVolatilities VOLATILITIES_ON_PAY = SwaptionSabrRateVolatilityDataSet.getVolatilitiesEur(PAYMENT, true);
	  // providers - ended
	  private static readonly ImmutableRatesProvider RATES_PROVIDER_AFTER_PAY = SwaptionSabrRateVolatilityDataSet.getRatesProviderEur(AFTER_PAYMENT, TIME_SERIES);
	  private static readonly SabrParametersSwaptionVolatilities VOLATILITIES_AFTER_PAY = SwaptionSabrRateVolatilityDataSet.getVolatilitiesEur(AFTER_PAYMENT, true);

	  private static readonly double ACC_FACTOR = ACT_360.relativeYearFraction(START, END);
	  private const double NOTIONAL = 10000000; // 10m
	  private const double STRIKE = 0.04;
	  private const double STRIKE_NEGATIVE = -0.01;
	  // CMS - buy
	  private static readonly CmsPeriod COUPON = createCmsCoupon(true);
	  private static readonly CmsPeriod CAPLET = createCmsCaplet(true, STRIKE);
	  private static readonly CmsPeriod FLOORLET = createCmsFloorlet(true, STRIKE);
	  // CMS - sell
	  private static readonly CmsPeriod COUPON_SELL = createCmsCoupon(false);
	  private static readonly CmsPeriod CAPLET_SELL = createCmsCaplet(false, STRIKE);
	  private static readonly CmsPeriod FLOORLET_SELL = createCmsFloorlet(false, STRIKE);
	  // CMS - zero strikes
	  private static readonly CmsPeriod CAPLET_ZERO = createCmsCaplet(true, 0d);
	  private static readonly CmsPeriod FLOORLET_ZERO = createCmsFloorlet(true, 0d);
	  // CMS - negative strikes, to become positive after shift
	  private static readonly CmsPeriod CAPLET_NEGATIVE = createCmsCaplet(true, STRIKE_NEGATIVE);
	  private static readonly CmsPeriod FLOORLET_NEGATIVE = createCmsFloorlet(true, STRIKE_NEGATIVE);
	  // CMS - negative strikes, to become zero after shift
	  private static readonly CmsPeriod CAPLET_SHIFT = createCmsCaplet(true, -SHIFT);
	  private static readonly CmsPeriod FLOORLET_SHIFT = createCmsFloorlet(true, -SHIFT);

	  private const double CUT_OFF_STRIKE = 0.10;
	  private const double MU = 2.50;
	  private const double EPS = 1.0e-5;
	  private const double TOL = 1.0e-12;
	  private static readonly SabrExtrapolationReplicationCmsPeriodPricer PRICER = SabrExtrapolationReplicationCmsPeriodPricer.of(CUT_OFF_STRIKE, MU);
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FD_CAL = new RatesFiniteDifferenceSensitivityCalculator(EPS);
	  private static readonly DiscountingSwapProductPricer PRICER_SWAP = DiscountingSwapProductPricer.DEFAULT;

	  public virtual void test_presentValue_zero()
	  {
		CurrencyAmount pv = PRICER.presentValue(COUPON, RATES_PROVIDER, VOLATILITIES);
		CurrencyAmount pvCaplet = PRICER.presentValue(CAPLET_ZERO, RATES_PROVIDER, VOLATILITIES);
		CurrencyAmount pvFloorlet = PRICER.presentValue(FLOORLET_ZERO, RATES_PROVIDER, VOLATILITIES);
		assertEquals(pv.Amount, pvCaplet.Amount, NOTIONAL * TOL);
		assertEquals(pvFloorlet.Amount, 0d, 2.0d * NOTIONAL * TOL);
		CurrencyAmount pvShift = PRICER.presentValue(COUPON, RATES_PROVIDER, VOLATILITIES_SHIFT);
		CurrencyAmount pvCapletShift = PRICER.presentValue(CAPLET_SHIFT, RATES_PROVIDER, VOLATILITIES_SHIFT);
		CurrencyAmount pvFloorletShift = PRICER.presentValue(FLOORLET_SHIFT, RATES_PROVIDER, VOLATILITIES_SHIFT);
		double dfPayment = RATES_PROVIDER.discountFactor(EUR, PAYMENT);
		assertEquals(pvShift.Amount, pvCapletShift.Amount - SHIFT * dfPayment * NOTIONAL * ACC_FACTOR, NOTIONAL * TOL);
		assertEquals(pvFloorletShift.Amount, 0d, 2.0d * NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_buySell()
	  {
		CurrencyAmount pvBuy = PRICER.presentValue(COUPON, RATES_PROVIDER, VOLATILITIES);
		CurrencyAmount pvCapletBuy = PRICER.presentValue(CAPLET, RATES_PROVIDER, VOLATILITIES);
		CurrencyAmount pvFloorletBuy = PRICER.presentValue(FLOORLET, RATES_PROVIDER, VOLATILITIES);
		CurrencyAmount pvSell = PRICER.presentValue(COUPON_SELL, RATES_PROVIDER, VOLATILITIES);
		CurrencyAmount pvCapletSell = PRICER.presentValue(CAPLET_SELL, RATES_PROVIDER, VOLATILITIES);
		CurrencyAmount pvFloorletSell = PRICER.presentValue(FLOORLET_SELL, RATES_PROVIDER, VOLATILITIES);
		assertEquals(pvBuy.Amount, -pvSell.Amount, NOTIONAL * TOL);
		assertEquals(pvCapletBuy.Amount, -pvCapletSell.Amount, NOTIONAL * TOL);
		assertEquals(pvFloorletBuy.Amount, -pvFloorletSell.Amount, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_afterFix()
	  {
		CurrencyAmount pv = PRICER.presentValue(COUPON, RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX);
		CurrencyAmount pvCapletOtm = PRICER.presentValue(CAPLET, RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX);
		CurrencyAmount pvCapletItm = PRICER.presentValue(CAPLET_NEGATIVE, RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX);
		CurrencyAmount pvFloorletItm = PRICER.presentValue(FLOORLET, RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX);
		CurrencyAmount pvFloorletOtm = PRICER.presentValue(FLOORLET_NEGATIVE, RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX);
		double factor = RATES_PROVIDER_AFTER_FIX.discountFactor(EUR, PAYMENT) * NOTIONAL * COUPON.YearFraction;
		assertEquals(pv.Amount, OBS_INDEX * factor, NOTIONAL * TOL);
		assertEquals(pvCapletOtm.Amount, 0d, NOTIONAL * TOL);
		assertEquals(pvCapletItm.Amount, (OBS_INDEX - STRIKE_NEGATIVE) * factor, NOTIONAL * TOL);
		assertEquals(pvFloorletItm.Amount, (STRIKE - OBS_INDEX) * factor, NOTIONAL * TOL);
		assertEquals(pvFloorletOtm.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_onPayment()
	  {
		CurrencyAmount pv = PRICER.presentValue(COUPON, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY);
		CurrencyAmount pvCapletOtm = PRICER.presentValue(CAPLET, RATES_PROVIDER_ON_PAY, VOLATILITIES_AFTER_FIX);
		CurrencyAmount pvCapletItm = PRICER.presentValue(CAPLET_NEGATIVE, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY);
		CurrencyAmount pvFloorletItm = PRICER.presentValue(FLOORLET, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY);
		CurrencyAmount pvFloorletOtm = PRICER.presentValue(FLOORLET_NEGATIVE, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY);
		double factor = NOTIONAL * COUPON.YearFraction;
		assertEquals(pv.Amount, OBS_INDEX * factor, NOTIONAL * TOL);
		assertEquals(pvCapletOtm.Amount, 0d, NOTIONAL * TOL);
		assertEquals(pvCapletItm.Amount, (OBS_INDEX - STRIKE_NEGATIVE) * factor, NOTIONAL * TOL);
		assertEquals(pvFloorletItm.Amount, (STRIKE - OBS_INDEX) * factor, NOTIONAL * TOL);
		assertEquals(pvFloorletOtm.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_afterPayment()
	  {
		CurrencyAmount pv = PRICER.presentValue(COUPON, RATES_PROVIDER_AFTER_PAY, VOLATILITIES_AFTER_PAY);
		CurrencyAmount pvCaplet = PRICER.presentValue(CAPLET, RATES_PROVIDER_AFTER_PAY, VOLATILITIES_AFTER_PAY);
		CurrencyAmount pvFloorlet = PRICER.presentValue(FLOORLET, RATES_PROVIDER_AFTER_PAY, VOLATILITIES_AFTER_PAY);
		assertEquals(pv, CurrencyAmount.zero(EUR));
		assertEquals(pvCaplet, CurrencyAmount.zero(EUR));
		assertEquals(pvFloorlet, CurrencyAmount.zero(EUR));
	  }

	  public virtual void test_presentValue_afterFix_noTimeSeries()
	  {
		assertThrowsIllegalArg(() => PRICER.presentValue(COUPON, RATES_PROVIDER_NO_TS, VOLATILITIES_NO_TS));
		assertThrowsIllegalArg(() => PRICER.presentValue(CAPLET, RATES_PROVIDER_NO_TS, VOLATILITIES_NO_TS));
		assertThrowsIllegalArg(() => PRICER.presentValue(FLOORLET, RATES_PROVIDER_NO_TS, VOLATILITIES_NO_TS));
	  }

	  public virtual void test_presentValue_cap_floor_parity()
	  {
		// Cap/Floor parity is not perfect as the cash swaption standard formula is not arbitrage free.
		CurrencyAmount pvCap = PRICER.presentValue(CAPLET, RATES_PROVIDER, VOLATILITIES_SHIFT);
		CurrencyAmount pvFloor = PRICER.presentValue(FLOORLET, RATES_PROVIDER, VOLATILITIES_SHIFT);
		CurrencyAmount pvCpn = PRICER.presentValue(COUPON, RATES_PROVIDER, VOLATILITIES_SHIFT);
		double pvStrike = STRIKE * NOTIONAL * ACC_FACTOR * RATES_PROVIDER.discountFactor(EUR, PAYMENT);
		assertEquals(pvCap.Amount - pvFloor.Amount, pvCpn.Amount - pvStrike, 1.0E+3);
		CurrencyAmount pvCap1 = PRICER.presentValue(CAPLET_NEGATIVE, RATES_PROVIDER, VOLATILITIES_SHIFT);
		CurrencyAmount pvFloor1 = PRICER.presentValue(FLOORLET_NEGATIVE, RATES_PROVIDER, VOLATILITIES_SHIFT);
		CurrencyAmount pvCpn1 = PRICER.presentValue(COUPON, RATES_PROVIDER, VOLATILITIES_SHIFT);
		double pvStrike1 = STRIKE_NEGATIVE * NOTIONAL * ACC_FACTOR * RATES_PROVIDER.discountFactor(EUR, PAYMENT);
		assertEquals(pvCap1.Amount - pvFloor1.Amount, pvCpn1.Amount - pvStrike1, 1.0E+3);
		CurrencyAmount pvCap2 = PRICER.presentValue(CAPLET, RATES_PROVIDER, VOLATILITIES);
		CurrencyAmount pvFloor2 = PRICER.presentValue(FLOORLET, RATES_PROVIDER, VOLATILITIES);
		CurrencyAmount pvCpn2 = PRICER.presentValue(COUPON, RATES_PROVIDER, VOLATILITIES);
		double pvStrike2 = STRIKE * NOTIONAL * ACC_FACTOR * RATES_PROVIDER.discountFactor(EUR, PAYMENT);
		assertEquals(pvCap2.Amount - pvFloor2.Amount, pvCpn2.Amount - pvStrike2, 1.0E+3);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity()
	  {
		PointSensitivityBuilder pvPointCoupon = PRICER.presentValueSensitivityRates(COUPON_SELL, RATES_PROVIDER, VOLATILITIES);
		CurrencyParameterSensitivities computedCoupon = RATES_PROVIDER.parameterSensitivity(pvPointCoupon.build());
		CurrencyParameterSensitivities expectedCoupon = FD_CAL.sensitivity(RATES_PROVIDER, p => PRICER.presentValue(COUPON_SELL, p, VOLATILITIES));
		assertTrue(computedCoupon.equalWithTolerance(expectedCoupon, EPS * NOTIONAL * 50d));
		PointSensitivityBuilder pvCapPoint = PRICER.presentValueSensitivityRates(CAPLET_SELL, RATES_PROVIDER, VOLATILITIES);
		CurrencyParameterSensitivities computedCap = RATES_PROVIDER.parameterSensitivity(pvCapPoint.build());
		CurrencyParameterSensitivities expectedCap = FD_CAL.sensitivity(RATES_PROVIDER, p => PRICER.presentValue(CAPLET_SELL, p, VOLATILITIES));
		assertTrue(computedCap.equalWithTolerance(expectedCap, EPS * NOTIONAL * 50d));
		PointSensitivityBuilder pvFloorPoint = PRICER.presentValueSensitivityRates(FLOORLET_SELL, RATES_PROVIDER, VOLATILITIES);
		CurrencyParameterSensitivities computedFloor = RATES_PROVIDER.parameterSensitivity(pvFloorPoint.build());
		CurrencyParameterSensitivities expectedFloor = FD_CAL.sensitivity(RATES_PROVIDER, p => PRICER.presentValue(FLOORLET_SELL, p, VOLATILITIES));
		assertTrue(computedFloor.equalWithTolerance(expectedFloor, EPS * NOTIONAL * 10d));
	  }

	  public virtual void test_presentValueSensitivity_shift()
	  {
	//    CurrencyAmount tmp = PRICER.presentValue(COUPON, RATES_PROVIDER, VOLATILITIES_SHIFT);
		PointSensitivityBuilder pvPointCoupon = PRICER.presentValueSensitivityRates(COUPON, RATES_PROVIDER, VOLATILITIES_SHIFT);
		CurrencyParameterSensitivities computedCoupon = RATES_PROVIDER.parameterSensitivity(pvPointCoupon.build());
		CurrencyParameterSensitivities expectedCoupon = FD_CAL.sensitivity(RATES_PROVIDER, p => PRICER.presentValue(COUPON, p, VOLATILITIES_SHIFT));
		assertTrue(computedCoupon.equalWithTolerance(expectedCoupon, EPS * NOTIONAL * 50d));
		PointSensitivityBuilder pvCapPoint = PRICER.presentValueSensitivityRates(CAPLET_NEGATIVE, RATES_PROVIDER, VOLATILITIES_SHIFT);
		CurrencyParameterSensitivities computedCap = RATES_PROVIDER.parameterSensitivity(pvCapPoint.build());
		CurrencyParameterSensitivities expectedCap = FD_CAL.sensitivity(RATES_PROVIDER, p => PRICER.presentValue(CAPLET_NEGATIVE, p, VOLATILITIES_SHIFT));
		assertTrue(computedCap.equalWithTolerance(expectedCap, EPS * NOTIONAL * 50d));
		PointSensitivityBuilder pvFloorPoint = PRICER.presentValueSensitivityRates(FLOORLET_NEGATIVE, RATES_PROVIDER, VOLATILITIES_SHIFT);
		CurrencyParameterSensitivities computedFloor = RATES_PROVIDER.parameterSensitivity(pvFloorPoint.build());
		CurrencyParameterSensitivities expectedFloor = FD_CAL.sensitivity(RATES_PROVIDER, p => PRICER.presentValue(FLOORLET_NEGATIVE, p, VOLATILITIES_SHIFT));
		assertTrue(computedFloor.equalWithTolerance(expectedFloor, EPS * NOTIONAL * 10d));
	  }

	  public virtual void test_presentValueSensitivity_onFix()
	  {
		PointSensitivityBuilder pvPointCoupon = PRICER.presentValueSensitivityRates(COUPON_SELL, RATES_PROVIDER_ON_FIX, VOLATILITIES_ON_FIX);
		CurrencyParameterSensitivities computedCoupon = RATES_PROVIDER_ON_FIX.parameterSensitivity(pvPointCoupon.build());
		CurrencyParameterSensitivities expectedCoupon = FD_CAL.sensitivity(RATES_PROVIDER_ON_FIX, p => PRICER.presentValue(COUPON_SELL, p, VOLATILITIES_ON_FIX));
		assertTrue(computedCoupon.equalWithTolerance(expectedCoupon, EPS * NOTIONAL * 50d));
		PointSensitivityBuilder pvCapPoint = PRICER.presentValueSensitivityRates(CAPLET_SELL, RATES_PROVIDER_ON_FIX, VOLATILITIES_ON_FIX);
		CurrencyParameterSensitivities computedCap = RATES_PROVIDER_ON_FIX.parameterSensitivity(pvCapPoint.build());
		CurrencyParameterSensitivities expectedCap = FD_CAL.sensitivity(RATES_PROVIDER_ON_FIX, p => PRICER.presentValue(CAPLET_SELL, p, VOLATILITIES_ON_FIX));
		assertTrue(computedCap.equalWithTolerance(expectedCap, EPS * NOTIONAL * 80d));
		PointSensitivityBuilder pvFloorPoint = PRICER.presentValueSensitivityRates(FLOORLET_SELL, RATES_PROVIDER_ON_FIX, VOLATILITIES_ON_FIX);
		CurrencyParameterSensitivities computedFloor = RATES_PROVIDER_ON_FIX.parameterSensitivity(pvFloorPoint.build());
		CurrencyParameterSensitivities expectedFloor = FD_CAL.sensitivity(RATES_PROVIDER_ON_FIX, p => PRICER.presentValue(FLOORLET_SELL, p, VOLATILITIES_ON_FIX));
		assertTrue(computedFloor.equalWithTolerance(expectedFloor, EPS * NOTIONAL * 50d));
	  }

	  public virtual void test_presentValueSensitivity_afterFix()
	  {
		PointSensitivityBuilder pvPointCoupon = PRICER.presentValueSensitivityRates(COUPON_SELL, RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX);
		CurrencyParameterSensitivities computedCoupon = RATES_PROVIDER_AFTER_FIX.parameterSensitivity(pvPointCoupon.build());
		CurrencyParameterSensitivities expectedCoupon = FD_CAL.sensitivity(RATES_PROVIDER_AFTER_FIX, p => PRICER.presentValue(COUPON_SELL, p, VOLATILITIES_AFTER_FIX));
		assertTrue(computedCoupon.equalWithTolerance(expectedCoupon, EPS * NOTIONAL));
		PointSensitivityBuilder pvCapPoint = PRICER.presentValueSensitivityRates(CAPLET_SELL, RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX);
		CurrencyParameterSensitivities computedCap = RATES_PROVIDER_AFTER_FIX.parameterSensitivity(pvCapPoint.build());
		CurrencyParameterSensitivities expectedCap = FD_CAL.sensitivity(RATES_PROVIDER_AFTER_FIX, p => PRICER.presentValue(CAPLET_SELL, p, VOLATILITIES_AFTER_FIX));
		assertTrue(computedCap.equalWithTolerance(expectedCap, EPS * NOTIONAL));
		PointSensitivityBuilder pvFloorPoint = PRICER.presentValueSensitivityRates(FLOORLET_SELL, RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX);
		CurrencyParameterSensitivities computedFloor = RATES_PROVIDER_AFTER_FIX.parameterSensitivity(pvFloorPoint.build());
		CurrencyParameterSensitivities expectedFloor = FD_CAL.sensitivity(RATES_PROVIDER_AFTER_FIX, p => PRICER.presentValue(FLOORLET_SELL, p, VOLATILITIES_AFTER_FIX));
		assertTrue(computedFloor.equalWithTolerance(expectedFloor, EPS * NOTIONAL));
	  }

	  public virtual void test_presentValueSensitivity_onPayment()
	  {
		PointSensitivityBuilder pvSensi = PRICER.presentValueSensitivityRates(COUPON, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY);
		PointSensitivityBuilder pvSensiCapletOtm = PRICER.presentValueSensitivityRates(CAPLET, RATES_PROVIDER_ON_PAY, VOLATILITIES_AFTER_FIX);
		PointSensitivityBuilder pvSensiCapletItm = PRICER.presentValueSensitivityRates(CAPLET_NEGATIVE, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY);
		PointSensitivityBuilder pvSensiFloorletItm = PRICER.presentValueSensitivityRates(FLOORLET, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY);
		PointSensitivityBuilder pvSensiFloorletOtm = PRICER.presentValueSensitivityRates(FLOORLET_NEGATIVE, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY);
		double paymentTime = RATES_PROVIDER_ON_PAY.discountFactors(EUR).relativeYearFraction(PAYMENT);
		PointSensitivityBuilder expected = ZeroRateSensitivity.of(EUR, paymentTime, -0d);
		assertEquals(pvSensi, expected);
		assertEquals(pvSensiCapletOtm, expected);
		assertEquals(pvSensiCapletItm, expected);
		assertEquals(pvSensiFloorletItm, expected);
		assertEquals(pvSensiFloorletOtm, expected);
	  }

	  public virtual void test_presentValueSensitivity_afterFix_noTimeSeries()
	  {
		assertThrowsIllegalArg(() => PRICER.presentValueSensitivityRates(COUPON, RATES_PROVIDER_NO_TS, VOLATILITIES_NO_TS));
		assertThrowsIllegalArg(() => PRICER.presentValueSensitivityRates(CAPLET, RATES_PROVIDER_NO_TS, VOLATILITIES_NO_TS));
		assertThrowsIllegalArg(() => PRICER.presentValueSensitivityRates(FLOORLET, RATES_PROVIDER_NO_TS, VOLATILITIES_NO_TS));
	  }

	  public virtual void test_presentValueSensitivity_afterPayment()
	  {
		PointSensitivityBuilder pt = PRICER.presentValueSensitivityRates(COUPON, RATES_PROVIDER_AFTER_PAY, VOLATILITIES_AFTER_PAY);
		PointSensitivityBuilder ptCap = PRICER.presentValueSensitivityRates(CAPLET, RATES_PROVIDER_AFTER_PAY, VOLATILITIES_AFTER_PAY);
		PointSensitivityBuilder ptFloor = PRICER.presentValueSensitivityRates(FLOORLET, RATES_PROVIDER_AFTER_PAY, VOLATILITIES_AFTER_PAY);
		assertEquals(pt, PointSensitivityBuilder.none());
		assertEquals(ptCap, PointSensitivityBuilder.none());
		assertEquals(ptFloor, PointSensitivityBuilder.none());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivitySabrParameter()
	  {
		testPresentValueSensitivitySabrParameter(COUPON_SELL, CAPLET_SELL, FLOORLET_SELL, RATES_PROVIDER, VOLATILITIES);
		testPresentValueSensitivitySabrParameter(COUPON, CAPLET_NEGATIVE, FLOORLET_NEGATIVE, RATES_PROVIDER, VOLATILITIES_SHIFT);
		testPresentValueSensitivitySabrParameter(COUPON_SELL, CAPLET_SELL, FLOORLET_SELL, RATES_PROVIDER_ON_FIX, VOLATILITIES_ON_FIX);
	  }

	  public virtual void test_presentValueSensitivitySabrParameter_afterFix()
	  {
		PointSensitivityBuilder pvCouponPoint = PRICER.presentValueSensitivityModelParamsSabr(COUPON_SELL, RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX);
		PointSensitivityBuilder pvCapPoint = PRICER.presentValueSensitivityModelParamsSabr(CAPLET_SELL, RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX);
		PointSensitivityBuilder pvFloorPoint = PRICER.presentValueSensitivityModelParamsSabr(FLOORLET_SELL, RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX);
		assertEquals(pvCouponPoint, PointSensitivityBuilder.none());
		assertEquals(pvCapPoint, PointSensitivityBuilder.none());
		assertEquals(pvFloorPoint, PointSensitivityBuilder.none());
	  }

	  public virtual void test_presentValueSensitivitySabrParameter_onPayment()
	  {
		PointSensitivityBuilder pvSensi = PRICER.presentValueSensitivityModelParamsSabr(COUPON, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY);
		PointSensitivityBuilder pvSensiCapletOtm = PRICER.presentValueSensitivityModelParamsSabr(CAPLET, RATES_PROVIDER_ON_PAY, VOLATILITIES_AFTER_FIX);
		PointSensitivityBuilder pvSensiCapletItm = PRICER.presentValueSensitivityModelParamsSabr(CAPLET_NEGATIVE, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY);
		PointSensitivityBuilder pvSensiFloorletItm = PRICER.presentValueSensitivityModelParamsSabr(FLOORLET, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY);
		PointSensitivityBuilder pvSensiFloorletOtm = PRICER.presentValueSensitivityModelParamsSabr(FLOORLET_NEGATIVE, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY);
		assertEquals(pvSensi, PointSensitivityBuilder.none());
		assertEquals(pvSensiCapletOtm, PointSensitivityBuilder.none());
		assertEquals(pvSensiCapletItm, PointSensitivityBuilder.none());
		assertEquals(pvSensiFloorletItm, PointSensitivityBuilder.none());
		assertEquals(pvSensiFloorletOtm, PointSensitivityBuilder.none());
	  }

	  public virtual void test_presentValueSensitivitySabrParameter_afterFix_noTimeSeries()
	  {
		assertThrowsIllegalArg(() => PRICER.presentValueSensitivityModelParamsSabr(COUPON, RATES_PROVIDER_NO_TS, VOLATILITIES_NO_TS));
		assertThrowsIllegalArg(() => PRICER.presentValueSensitivityModelParamsSabr(CAPLET, RATES_PROVIDER_NO_TS, VOLATILITIES_NO_TS));
		assertThrowsIllegalArg(() => PRICER.presentValueSensitivityModelParamsSabr(FLOORLET, RATES_PROVIDER_NO_TS, VOLATILITIES_NO_TS));
	  }

	  public virtual void test_presentValueSensitivitySabrParameter_afterPayment()
	  {
		PointSensitivityBuilder sensi = PRICER.presentValueSensitivityModelParamsSabr(COUPON, RATES_PROVIDER_AFTER_PAY, VOLATILITIES_AFTER_PAY);
		PointSensitivityBuilder sensiCap = PRICER.presentValueSensitivityModelParamsSabr(CAPLET, RATES_PROVIDER_AFTER_PAY, VOLATILITIES_AFTER_PAY);
		PointSensitivityBuilder sensiFloor = PRICER.presentValueSensitivityModelParamsSabr(FLOORLET, RATES_PROVIDER_AFTER_PAY, VOLATILITIES_AFTER_PAY);
		assertEquals(sensi, PointSensitivityBuilder.none());
		assertEquals(sensiCap, PointSensitivityBuilder.none());
		assertEquals(sensiFloor, PointSensitivityBuilder.none());
	  }

	  public virtual void test_adjusted_forward_rate()
	  {
		CmsPeriod coupon1 = COUPON.toBuilder().notional(1.0).yearFraction(1.0).build();
		CurrencyAmount pvBuy = PRICER.presentValue(coupon1, RATES_PROVIDER, VOLATILITIES);
		double df = RATES_PROVIDER.discountFactor(EUR, PAYMENT);
		double adjustedForwardRateExpected = pvBuy.Amount / df;
		double adjustedForwardRateComputed = PRICER.adjustedForwardRate(COUPON, RATES_PROVIDER, VOLATILITIES);
		assertEquals(adjustedForwardRateComputed, adjustedForwardRateExpected, TOL);
	  }

	  public virtual void test_adjustment_forward_rate()
	  {
		double adjustedForwardRateComputed = PRICER.adjustedForwardRate(COUPON, RATES_PROVIDER, VOLATILITIES);
		double forward = PRICER_SWAP.parRate(COUPON.UnderlyingSwap, RATES_PROVIDER);
		double adjustmentComputed = PRICER.adjustmentToForwardRate(COUPON, RATES_PROVIDER, VOLATILITIES);
		assertEquals(adjustmentComputed, adjustedForwardRateComputed - forward, TOL);
	  }


	  public virtual void test_adjusted_forward_rate_cap_floor()
	  {
		double adjustedForwardRateCoupon = PRICER.adjustedForwardRate(COUPON, RATES_PROVIDER, VOLATILITIES);
		double adjustedForwardRateFloor = PRICER.adjustedForwardRate(FLOORLET, RATES_PROVIDER, VOLATILITIES);
		assertEquals(adjustedForwardRateCoupon, adjustedForwardRateFloor, TOL);
		double adjustedForwardRateCap = PRICER.adjustedForwardRate(CAPLET, RATES_PROVIDER, VOLATILITIES);
		assertEquals(adjustedForwardRateCoupon, adjustedForwardRateCap, TOL);
	  }

	  public virtual void test_adjusted_forward_rate_afterFix()
	  {
		double adjustedForward = PRICER.adjustedForwardRate(COUPON, RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX);
		assertEquals(adjustedForward, OBS_INDEX, TOL);
	  }

	  public virtual void test_adjusted_rate_error()
	  {
		assertThrowsIllegalArg(() => PRICER.adjustmentToForwardRate(COUPON, RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX));
	  }

	  //-------------------------------------------------------------------------
	  private static readonly CmsPeriod CAPLET_UP = createCmsCaplet(true, STRIKE + EPS);
	  private static readonly CmsPeriod CAPLET_DW = createCmsCaplet(true, STRIKE - EPS);
	  private static readonly CmsPeriod FLOORLET_UP = createCmsFloorlet(true, STRIKE + EPS);
	  private static readonly CmsPeriod FLOORLET_DW = createCmsFloorlet(true, STRIKE - EPS);

	  public virtual void test_presentValueSensitivityStrike()
	  {
		double computedCaplet = PRICER.presentValueSensitivityStrike(CAPLET, RATES_PROVIDER, VOLATILITIES);
		double expectedCaplet = 0.5 * (PRICER.presentValue(CAPLET_UP, RATES_PROVIDER, VOLATILITIES).Amount - PRICER.presentValue(CAPLET_DW, RATES_PROVIDER, VOLATILITIES).Amount) / EPS;
		assertEquals(computedCaplet, expectedCaplet, NOTIONAL * EPS);
		double computedFloorlet = PRICER.presentValueSensitivityStrike(FLOORLET, RATES_PROVIDER, VOLATILITIES);
		double expectedFloorlet = 0.5 * (PRICER.presentValue(FLOORLET_UP, RATES_PROVIDER, VOLATILITIES).Amount - PRICER.presentValue(FLOORLET_DW, RATES_PROVIDER, VOLATILITIES).Amount) / EPS;
		assertEquals(computedFloorlet, expectedFloorlet, NOTIONAL * EPS);
	  }

	  public virtual void test_presentValueSensitivityStrike_shift()
	  {
		double computedCaplet = PRICER.presentValueSensitivityStrike(CAPLET_NEGATIVE, RATES_PROVIDER, VOLATILITIES_SHIFT);
		CmsPeriod capletUp = createCmsCaplet(true, STRIKE_NEGATIVE + EPS);
		CmsPeriod capletDw = createCmsCaplet(true, STRIKE_NEGATIVE - EPS);
		double expectedCaplet = 0.5 * (PRICER.presentValue(capletUp, RATES_PROVIDER, VOLATILITIES_SHIFT).Amount - PRICER.presentValue(capletDw, RATES_PROVIDER, VOLATILITIES_SHIFT).Amount) / EPS;
		assertEquals(computedCaplet, expectedCaplet, NOTIONAL * EPS);
		double computedFloorlet = PRICER.presentValueSensitivityStrike(FLOORLET_NEGATIVE, RATES_PROVIDER, VOLATILITIES_SHIFT);
		CmsPeriod floorletUp = createCmsFloorlet(true, STRIKE_NEGATIVE + EPS);
		CmsPeriod floorletDw = createCmsFloorlet(true, STRIKE_NEGATIVE - EPS);
		double expectedFloorlet = 0.5 * (PRICER.presentValue(floorletUp, RATES_PROVIDER, VOLATILITIES_SHIFT).Amount - PRICER.presentValue(floorletDw, RATES_PROVIDER, VOLATILITIES_SHIFT).Amount) / EPS;
		assertEquals(computedFloorlet, expectedFloorlet, NOTIONAL * EPS);
	  }

	  public virtual void test_presentValueSensitivityStrike_onFix()
	  {
		double computedCaplet = PRICER.presentValueSensitivityStrike(CAPLET, RATES_PROVIDER_ON_FIX, VOLATILITIES_ON_FIX);
		double expectedCaplet = 0.5 * (PRICER.presentValue(CAPLET_UP, RATES_PROVIDER_ON_FIX, VOLATILITIES_ON_FIX).Amount - PRICER.presentValue(CAPLET_DW, RATES_PROVIDER_ON_FIX, VOLATILITIES_ON_FIX).Amount) / EPS;
		assertEquals(computedCaplet, expectedCaplet, NOTIONAL * EPS);
		double computedFloorlet = PRICER.presentValueSensitivityStrike(FLOORLET, RATES_PROVIDER_ON_FIX, VOLATILITIES_ON_FIX);
		double expectedFloorlet = 0.5 * (PRICER.presentValue(FLOORLET_UP, RATES_PROVIDER_ON_FIX, VOLATILITIES_ON_FIX).Amount - PRICER.presentValue(FLOORLET_DW, RATES_PROVIDER_ON_FIX, VOLATILITIES_ON_FIX).Amount) / EPS;
		assertEquals(computedFloorlet, expectedFloorlet, NOTIONAL * EPS * 10d);
	  }

	  public virtual void test_presentValueSensitivityStrike_afterFix()
	  {
		double cmpCapletOtm = PRICER.presentValueSensitivityStrike(CAPLET, RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX);
		double cmpCapletItm = PRICER.presentValueSensitivityStrike(CAPLET_NEGATIVE, RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX);
		double cmpFloorletItm = PRICER.presentValueSensitivityStrike(FLOORLET, RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX);
		double cmpFloorletOtm = PRICER.presentValueSensitivityStrike(FLOORLET_NEGATIVE, RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX);
		double expCapletOtm = (PRICER.presentValue(CAPLET_UP, RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX).Amount - PRICER.presentValue(CAPLET_DW, RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX).Amount) * 0.5 / EPS;
		double expCapletItm = (PRICER.presentValue(createCmsCaplet(true, STRIKE_NEGATIVE + EPS), RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX).Amount - PRICER.presentValue(createCmsCaplet(true, STRIKE_NEGATIVE - EPS), RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX).Amount) * 0.5 / EPS;
		double expFloorletItm = (PRICER.presentValue(FLOORLET_UP, RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX).Amount - PRICER.presentValue(FLOORLET_DW, RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX).Amount) * 0.5 / EPS;
		double expFloorletOtm = (PRICER.presentValue(createCmsFloorlet(true, STRIKE_NEGATIVE + EPS), RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX).Amount - PRICER.presentValue(createCmsFloorlet(true, STRIKE_NEGATIVE - EPS), RATES_PROVIDER_AFTER_FIX, VOLATILITIES_AFTER_FIX).Amount) * 0.5 / EPS;
		assertEquals(cmpCapletOtm, expCapletOtm, NOTIONAL * EPS);
		assertEquals(cmpCapletItm, expCapletItm, NOTIONAL * EPS);
		assertEquals(cmpFloorletOtm, expFloorletOtm, NOTIONAL * EPS);
		assertEquals(cmpFloorletItm, expFloorletItm, NOTIONAL * EPS);
	  }

	  public virtual void test_presentValueSensitivityStrike_onPayment()
	  {
		double computedCaplet = PRICER.presentValueSensitivityStrike(CAPLET, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY);
		double expectedCaplet = (PRICER.presentValue(CAPLET_UP, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY).Amount - PRICER.presentValue(CAPLET_DW, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY).Amount) * 0.5 / EPS;
		assertEquals(computedCaplet, expectedCaplet, NOTIONAL * EPS);
		double computedFloorlet = PRICER.presentValueSensitivityStrike(FLOORLET, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY);
		double expectedFloorlet = (PRICER.presentValue(FLOORLET_UP, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY).Amount - PRICER.presentValue(FLOORLET_DW, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY).Amount) * 0.5 / EPS;
		assertEquals(computedFloorlet, expectedFloorlet, NOTIONAL * EPS);
	  }

	  public virtual void test_presentValueSensitivityStrike_afterFix_noTimeSeries()
	  {
		assertThrowsIllegalArg(() => PRICER.presentValueSensitivityStrike(COUPON, RATES_PROVIDER_NO_TS, VOLATILITIES_NO_TS));
		assertThrowsIllegalArg(() => PRICER.presentValueSensitivityStrike(CAPLET, RATES_PROVIDER_NO_TS, VOLATILITIES_NO_TS));
		assertThrowsIllegalArg(() => PRICER.presentValueSensitivityStrike(FLOORLET, RATES_PROVIDER_NO_TS, VOLATILITIES_NO_TS));
	  }

	  public virtual void test_presentValueSensitivityStrike_afterPayment()
	  {
		double sensiCap = PRICER.presentValueSensitivityStrike(CAPLET, RATES_PROVIDER_AFTER_PAY, VOLATILITIES_AFTER_PAY);
		double sensiFloor = PRICER.presentValueSensitivityStrike(FLOORLET, RATES_PROVIDER_AFTER_PAY, VOLATILITIES_AFTER_PAY);
		assertEquals(sensiCap, 0d);
		assertEquals(sensiFloor, 0d);
	  }

	  public virtual void test_presentValueSensitivityStrike_coupon()
	  {
		assertThrowsIllegalArg(() => PRICER.presentValueSensitivityStrike(COUPON, RATES_PROVIDER, VOLATILITIES));
	  }

	  //-------------------------------------------------------------------------
	  private void testPresentValueSensitivitySabrParameter(CmsPeriod coupon, CmsPeriod caplet, CmsPeriod foorlet, RatesProvider ratesProvider, SabrParametersSwaptionVolatilities volatilities)
	  {
		PointSensitivities pvPointCoupon = PRICER.presentValueSensitivityModelParamsSabr(coupon, ratesProvider, volatilities).build();
		CurrencyParameterSensitivities computedCoupon = volatilities.parameterSensitivity(pvPointCoupon);
		PointSensitivities pvCapPoint = PRICER.presentValueSensitivityModelParamsSabr(caplet, ratesProvider, volatilities).build();
		CurrencyParameterSensitivities computedCap = volatilities.parameterSensitivity(pvCapPoint);
		PointSensitivities pvFloorPoint = PRICER.presentValueSensitivityModelParamsSabr(foorlet, ratesProvider, volatilities).build();
		CurrencyParameterSensitivities computedFloor = volatilities.parameterSensitivity(pvFloorPoint);

		SabrInterestRateParameters sabr = volatilities.Parameters;
		// alpha surface
		InterpolatedNodalSurface surfaceAlpha = (InterpolatedNodalSurface) sabr.AlphaSurface;
		CurrencyParameterSensitivity sensiCouponAlpha = computedCoupon.getSensitivity(surfaceAlpha.Name, EUR);
		int nParamsAlpha = surfaceAlpha.ParameterCount;
		for (int i = 0; i < nParamsAlpha; ++i)
		{
		  InterpolatedNodalSurface[] bumpedSurfaces = bumpSurface(surfaceAlpha, i);
		  SabrInterestRateParameters sabrUp = SabrInterestRateParameters.of(bumpedSurfaces[0], sabr.BetaSurface, sabr.RhoSurface, sabr.NuSurface, sabr.ShiftSurface, SabrVolatilityFormula.hagan());
		  SabrInterestRateParameters sabrDw = SabrInterestRateParameters.of(bumpedSurfaces[1], sabr.BetaSurface, sabr.RhoSurface, sabr.NuSurface, sabr.ShiftSurface, SabrVolatilityFormula.hagan());
		  testSensitivityValue(coupon, caplet, foorlet, ratesProvider, i, sensiCouponAlpha.Sensitivity, computedCap.getSensitivity(surfaceAlpha.Name, EUR).Sensitivity, computedFloor.getSensitivity(surfaceAlpha.Name, EUR).Sensitivity, replaceSabrParameters(sabrUp, volatilities), replaceSabrParameters(sabrDw, volatilities));
		}
		// beta surface
		InterpolatedNodalSurface surfaceBeta = (InterpolatedNodalSurface) sabr.BetaSurface;
		CurrencyParameterSensitivity sensiCouponBeta = computedCoupon.getSensitivity(surfaceBeta.Name, EUR);
		int nParamsBeta = surfaceBeta.ParameterCount;
		for (int i = 0; i < nParamsBeta; ++i)
		{
		  InterpolatedNodalSurface[] bumpedSurfaces = bumpSurface(surfaceBeta, i);
		  SabrInterestRateParameters sabrUp = SabrInterestRateParameters.of(sabr.AlphaSurface, bumpedSurfaces[0], sabr.RhoSurface, sabr.NuSurface, sabr.ShiftSurface, SabrVolatilityFormula.hagan());
		  SabrInterestRateParameters sabrDw = SabrInterestRateParameters.of(sabr.AlphaSurface, bumpedSurfaces[1], sabr.RhoSurface, sabr.NuSurface, sabr.ShiftSurface, SabrVolatilityFormula.hagan());
		  testSensitivityValue(coupon, caplet, foorlet, ratesProvider, i, sensiCouponBeta.Sensitivity, computedCap.getSensitivity(surfaceBeta.Name, EUR).Sensitivity, computedFloor.getSensitivity(surfaceBeta.Name, EUR).Sensitivity, replaceSabrParameters(sabrUp, volatilities), replaceSabrParameters(sabrDw, volatilities));
		}
		// rho surface
		InterpolatedNodalSurface surfaceRho = (InterpolatedNodalSurface) sabr.RhoSurface;
		CurrencyParameterSensitivity sensiCouponRho = computedCoupon.getSensitivity(surfaceRho.Name, EUR);
		int nParamsRho = surfaceRho.ParameterCount;
		for (int i = 0; i < nParamsRho; ++i)
		{
		  InterpolatedNodalSurface[] bumpedSurfaces = bumpSurface(surfaceRho, i);
		  SabrInterestRateParameters sabrUp = SabrInterestRateParameters.of(sabr.AlphaSurface, sabr.BetaSurface, bumpedSurfaces[0], sabr.NuSurface, sabr.ShiftSurface, SabrVolatilityFormula.hagan());
		  SabrInterestRateParameters sabrDw = SabrInterestRateParameters.of(sabr.AlphaSurface, sabr.BetaSurface, bumpedSurfaces[1], sabr.NuSurface, sabr.ShiftSurface, SabrVolatilityFormula.hagan());
		  testSensitivityValue(coupon, caplet, foorlet, ratesProvider, i, sensiCouponRho.Sensitivity, computedCap.getSensitivity(surfaceRho.Name, EUR).Sensitivity, computedFloor.getSensitivity(surfaceRho.Name, EUR).Sensitivity, replaceSabrParameters(sabrUp, volatilities), replaceSabrParameters(sabrDw, volatilities));
		}
		// nu surface
		InterpolatedNodalSurface surfaceNu = (InterpolatedNodalSurface) sabr.NuSurface;
		CurrencyParameterSensitivity sensiCouponNu = computedCoupon.getSensitivity(surfaceNu.Name, EUR);
		int nParamsNu = surfaceNu.ParameterCount;
		for (int i = 0; i < nParamsNu; ++i)
		{
		  InterpolatedNodalSurface[] bumpedSurfaces = bumpSurface(surfaceNu, i);
		  SabrInterestRateParameters sabrUp = SabrInterestRateParameters.of(sabr.AlphaSurface, sabr.BetaSurface, sabr.RhoSurface, bumpedSurfaces[0], sabr.ShiftSurface, SabrVolatilityFormula.hagan());
		  SabrInterestRateParameters sabrDw = SabrInterestRateParameters.of(sabr.AlphaSurface, sabr.BetaSurface, sabr.RhoSurface, bumpedSurfaces[1], sabr.ShiftSurface, SabrVolatilityFormula.hagan());
		  testSensitivityValue(coupon, caplet, foorlet, ratesProvider, i, sensiCouponNu.Sensitivity, computedCap.getSensitivity(surfaceNu.Name, EUR).Sensitivity, computedFloor.getSensitivity(surfaceNu.Name, EUR).Sensitivity, replaceSabrParameters(sabrUp, volatilities), replaceSabrParameters(sabrDw, volatilities));
		}
	  }

	  private InterpolatedNodalSurface[] bumpSurface(InterpolatedNodalSurface surface, int position)
	  {
		DoubleArray zValues = surface.ZValues;
		InterpolatedNodalSurface surfaceUp = surface.withZValues(zValues.with(position, zValues.get(position) + EPS));
		InterpolatedNodalSurface surfaceDw = surface.withZValues(zValues.with(position, zValues.get(position) - EPS));
		return new InterpolatedNodalSurface[] {surfaceUp, surfaceDw};
	  }

	  private SabrParametersSwaptionVolatilities replaceSabrParameters(SabrInterestRateParameters sabrParams, SabrParametersSwaptionVolatilities orgVols)
	  {
		return SabrParametersSwaptionVolatilities.of(SwaptionVolatilitiesName.of("Test-SABR"), orgVols.Convention, orgVols.ValuationDateTime, sabrParams);
	  }

	  private void testSensitivityValue(CmsPeriod coupon, CmsPeriod caplet, CmsPeriod floorlet, RatesProvider ratesProvider, int index, DoubleArray computedCouponSensi, DoubleArray computedCapSensi, DoubleArray computedFloorSensi, SabrParametersSwaptionVolatilities volsUp, SabrParametersSwaptionVolatilities volsDw)
	  {
		double expectedCoupon = 0.5 * (PRICER.presentValue(coupon, ratesProvider, volsUp).Amount - PRICER.presentValue(coupon, ratesProvider, volsDw).Amount) / EPS;
		double expectedCap = 0.5 * (PRICER.presentValue(caplet, ratesProvider, volsUp).Amount - PRICER.presentValue(caplet, ratesProvider, volsDw).Amount) / EPS;
		double expectedFloor = 0.5 * (PRICER.presentValue(floorlet, ratesProvider, volsUp).Amount - PRICER.presentValue(floorlet, ratesProvider, volsDw).Amount) / EPS;
		assertEquals(computedCouponSensi.get(index), expectedCoupon, EPS * NOTIONAL * 10d);
		assertEquals(computedCapSensi.get(index), expectedCap, EPS * NOTIONAL * 10d);
		assertEquals(computedFloorSensi.get(index), expectedFloor, EPS * NOTIONAL * 10d);
	  }

	  private static CmsPeriod createCmsCoupon(bool isBuy)
	  {
		double notional = isBuy ? NOTIONAL : -NOTIONAL;
		return CmsPeriod.builder().dayCount(ACT_360).currency(EUR).index(EUR_EURIBOR_1100_5Y).startDate(START).endDate(END).fixingDate(FIXING).notional(notional).paymentDate(PAYMENT).yearFraction(ACC_FACTOR).underlyingSwap(createUnderlyingSwap(FIXING)).build();
	  }

	  private static CmsPeriod createCmsCaplet(bool isBuy, double strike)
	  {
		double notional = isBuy ? NOTIONAL : -NOTIONAL;
		return CmsPeriod.builder().dayCount(ACT_360).currency(EUR).index(EUR_EURIBOR_1100_5Y).startDate(START).endDate(END).fixingDate(FIXING).notional(notional).paymentDate(PAYMENT).yearFraction(ACC_FACTOR).caplet(strike).underlyingSwap(createUnderlyingSwap(FIXING)).build();
	  }

	  private static CmsPeriod createCmsFloorlet(bool isBuy, double strike)
	  {
		double notional = isBuy ? NOTIONAL : -NOTIONAL;
		return CmsPeriod.builder().dayCount(ACT_360).currency(EUR).index(EUR_EURIBOR_1100_5Y).startDate(START).endDate(END).fixingDate(FIXING).notional(notional).paymentDate(PAYMENT).yearFraction(ACC_FACTOR).floorlet(strike).underlyingSwap(createUnderlyingSwap(FIXING)).build();
	  }

	  // creates and resolves the underlying swap
	  private static ResolvedSwap createUnderlyingSwap(LocalDate fixingDate)
	  {
		FixedIborSwapConvention conv = EUR_EURIBOR_1100_5Y.Template.Convention;
		LocalDate effectiveDate = conv.calculateSpotDateFromTradeDate(fixingDate, REF_DATA);
		LocalDate maturityDate = effectiveDate.plus(EUR_EURIBOR_1100_5Y.Template.Tenor);
		Swap swap = conv.toTrade(fixingDate, effectiveDate, maturityDate, BuySell.BUY, 1d, 1d).Product;
		return swap.resolve(REF_DATA);
	  }

	  //-------------------------------------------------------------------------
	  private const double TOLERANCE_K_P = 1.0E-8;
	  private const double TOLERANCE_K_PP = 1.0E-4;
	  private const double TOLERANCE_PV = 1.0E+0;

	  /* Check that the internal function used in the integrant (h, G and k in the documentation) are correctly implemented. */
	  public virtual void integrant_internal()
	  {
		SwapIndex index = CAPLET.Index;
		LocalDate effectiveDate = CAPLET.UnderlyingSwap.StartDate;
		ResolvedSwap expanded = CAPLET.UnderlyingSwap;
		double tenor = VOLATILITIES_SHIFT.tenor(effectiveDate, CAPLET.UnderlyingSwap.EndDate);
		double theta = VOLATILITIES_SHIFT.relativeTime(CAPLET.FixingDate.atTime(index.FixingTime).atZone(index.FixingZone));
		double delta = index.Template.Convention.FixedLeg.DayCount.relativeYearFraction(effectiveDate, PAYMENT);
		double S0 = PRICER_SWAP.parRate(COUPON.UnderlyingSwap, RATES_PROVIDER);
		CmsIntegrantProvider integrant = new CmsIntegrantProvider(this, CAPLET, expanded, STRIKE, tenor, theta, S0, -delta, VOLATILITIES_SHIFT, CUT_OFF_STRIKE, MU);
		// Integrant internal
		double h = integrant.h(STRIKE);
		double hExpected = Math.Pow(1 + STRIKE, -delta);
		assertEquals(h, hExpected, TOLERANCE_K_P);
		double g = integrant.g(STRIKE);
		double gExpected = (1.0 - 1.0 / Math.Pow(1 + STRIKE, tenor)) / STRIKE;
		assertEquals(g, gExpected, TOLERANCE_K_P);
		double kExpected = integrant.h(STRIKE) / integrant.g(STRIKE);
		double k = integrant.k(STRIKE);
		assertEquals(k, kExpected, TOLERANCE_K_P);
		double shiftFd = 1.0E-5;
		double kP = integrant.h(STRIKE + shiftFd) / integrant.g(STRIKE + shiftFd);
		double kM = integrant.h(STRIKE - shiftFd) / integrant.g(STRIKE - shiftFd);
		double[] kpkpp = integrant.kpkpp(STRIKE);
		assertEquals(kpkpp[0], (kP - kM) / (2 * shiftFd), TOLERANCE_K_P);
		assertEquals(kpkpp[1], (kP + kM - 2 * k) / (shiftFd * shiftFd), TOLERANCE_K_PP);
	  }

	  /* Check the present value v.  */
	  public virtual void test_presentValue_replication_cap()
	  {
		SwapIndex index = CAPLET.Index;
		LocalDate effectiveDate = CAPLET.UnderlyingSwap.StartDate;
		ResolvedSwap expanded = CAPLET.UnderlyingSwap;
		double tenor = VOLATILITIES.tenor(effectiveDate, CAPLET.UnderlyingSwap.EndDate);
		double theta = VOLATILITIES.relativeTime(CAPLET.FixingDate.atTime(index.FixingTime).atZone(index.FixingZone));
		double delta = index.Template.Convention.FixedLeg.DayCount.relativeYearFraction(effectiveDate, PAYMENT);
		double ptp = RATES_PROVIDER.discountFactor(EUR, PAYMENT);
		double S0 = PRICER_SWAP.parRate(COUPON.UnderlyingSwap, RATES_PROVIDER);
		CmsIntegrantProvider integrant = new CmsIntegrantProvider(this, CAPLET, expanded, STRIKE, tenor, theta, S0, -delta, VOLATILITIES_SHIFT, CUT_OFF_STRIKE, MU);
		// Strike part
		double h_1S0 = 1.0 / integrant.h(S0);
		double gS0 = integrant.g(S0);
		double kK = integrant.k(STRIKE);
		double bsS0 = integrant.bs(STRIKE);
		double strikePart = ptp * h_1S0 * gS0 * kK * bsS0;
		// Integral part
		RungeKuttaIntegrator1D integrator = new RungeKuttaIntegrator1D(1.0E-7, 1.0E-10, 10);
		double integralPart = ptp * integrator.integrate(integrant.integrant(), STRIKE, 100.0);
		double pvExpected = (strikePart + integralPart) * NOTIONAL * ACC_FACTOR;
		CurrencyAmount pvComputed = PRICER.presentValue(CAPLET, RATES_PROVIDER, VOLATILITIES_SHIFT);
		assertEquals(pvComputed.Amount, pvExpected, TOLERANCE_PV);
	  }

	  //---------------------------------------------------------------------
	  public virtual void test_explainPresentValue()
	  {
		ExplainMapBuilder builder = ExplainMap.builder();
		PRICER.explainPresentValue(FLOORLET, RATES_PROVIDER, VOLATILITIES, builder);
		ExplainMap explain = builder.build();
		//Test a CMS Floorlet Period.
		assertEquals(explain.get(ExplainKey.ENTRY_TYPE).get(), "CmsFloorletPeriod");
		assertEquals(explain.get(ExplainKey.STRIKE_VALUE).Value, 0.04d);
		assertEquals(explain.get(ExplainKey.NOTIONAL).get().Amount, 10000000d);
		assertEquals(explain.get(ExplainKey.PAYMENT_DATE).get(), LocalDate.of(2021, 0x4, 28));
		assertEquals(explain.get(ExplainKey.DISCOUNT_FACTOR).Value, 0.8518053333230845d);
		assertEquals(explain.get(ExplainKey.START_DATE).get(), LocalDate.of(2020, 0x4, 28));
		assertEquals(explain.get(ExplainKey.END_DATE).get(), LocalDate.of(2021, 0x4, 28));
		assertEquals(explain.get(ExplainKey.FIXING_DATE).get(), LocalDate.of(2020, 0x4, 24));
		assertEquals(explain.get(ExplainKey.ACCRUAL_YEAR_FRACTION).Value, 1.0138888888888888d);
		double forwardSwapRate = PRICER_SWAP.parRate(FLOORLET.UnderlyingSwap, RATES_PROVIDER);
		assertEquals(explain.get(ExplainKey.FORWARD_RATE).Value, forwardSwapRate);
		CurrencyAmount pv = PRICER.presentValue(FLOORLET, RATES_PROVIDER, VOLATILITIES);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get(), pv);
		double adjustedForwardRate = PRICER.adjustedForwardRate(FLOORLET, RATES_PROVIDER, VOLATILITIES);
		assertEquals(explain.get(ExplainKey.CONVEXITY_ADJUSTED_RATE).Value, adjustedForwardRate);

	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Simplified integrant for testing; only cap; underlying with annual payments </summary>
	  private class CmsIntegrantProvider
	  {
		  private readonly SabrExtrapolationReplicationCmsPeriodPricerTest outerInstance;

		internal readonly int nbFixedPeriod;
		internal readonly double eta;
		internal readonly double strike;
		internal readonly double shift;
		internal readonly double factor;
		internal readonly SabrExtrapolationRightFunction sabrExtrapolation;

		public CmsIntegrantProvider(SabrExtrapolationReplicationCmsPeriodPricerTest outerInstance, CmsPeriod cmsPeriod, ResolvedSwap swap, double strike, double tenor, double timeToExpiry, double forward, double eta, SabrParametersSwaptionVolatilities swaptionVolatilities, double cutOffStrike, double mu)
		{
			this.outerInstance = outerInstance;

		  ResolvedSwapLeg fixedLeg = swap.getLegs(SwapLegType.FIXED).get(0);
		  this.nbFixedPeriod = fixedLeg.PaymentPeriods.size();
		  this.eta = eta;
		  SabrInterestRateParameters @params = swaptionVolatilities.Parameters;
		  SabrFormulaData sabrPoint = SabrFormulaData.of(@params.alpha(timeToExpiry, tenor), @params.beta(timeToExpiry, tenor), @params.rho(timeToExpiry, tenor), @params.nu(timeToExpiry, tenor));
		  this.shift = @params.shift(timeToExpiry, tenor);
		  this.sabrExtrapolation = SabrExtrapolationRightFunction.of(forward + shift, timeToExpiry, sabrPoint, cutOffStrike + shift, mu);
		  this.strike = strike;
		  this.factor = g(forward) / h(forward);
		}

		/// <summary>
		/// Obtains the integrant used in price replication.
		/// </summary>
		/// <returns> the integrant </returns>
		internal virtual System.Func<double, double> integrant()
		{
		  return (double? x) =>
		  {
	  double[] kD = kpkpp(x.Value);
	  // Implementation note: kD[0] contains the first derivative of k; kD[1] the second derivative of k.
	  return factor * (kD[1] * (x - strike) + 2d * kD[0]) * bs(x.Value);
		  };
		}

		/// <summary>
		/// The approximation of the discount factor as function of the swap rate.
		/// </summary>
		/// <param name="x">  the swap rate. </param>
		/// <returns> the discount factor. </returns>
		internal virtual double h(double x)
		{
		  return Math.Pow(1d + x, eta);
		}

		/// <summary>
		/// The cash annuity.
		/// </summary>
		/// <param name="x">  the swap rate. </param>
		/// <returns> the annuity. </returns>
		internal virtual double g(double x)
		{
			double periodFactor = 1d + x;
			double nPeriodDiscount = Math.Pow(periodFactor, -nbFixedPeriod);
			return (1d - nPeriodDiscount) / x;
		}

		/// <summary>
		/// The factor used in the strike part and in the integration of the replication.
		/// </summary>
		/// <param name="x">  the swap rate. </param>
		/// <returns> the factor. </returns>
		internal virtual double k(double x)
		{
		  double g;
		  double h;
			double periodFactor = 1d + x;
			double nPeriodDiscount = Math.Pow(periodFactor, -nbFixedPeriod);
			g = (1d - nPeriodDiscount) / x;
			h = Math.Pow(1.0 + x, eta);
		  return h / g;
		}

		/// <summary>
		/// The first and second derivative of the function k.
		/// <para>
		/// The first element is the first derivative and the second element is second derivative.
		/// 
		/// </para>
		/// </summary>
		/// <param name="x">  the swap rate. </param>
		/// <returns> the derivatives </returns>
		protected internal virtual double[] kpkpp(double x)
		{
		  double periodFactor = 1d + x;
		  double nPeriodDiscount = Math.Pow(periodFactor, -nbFixedPeriod);
		  /*The value of the annuity and its first and second derivative. */
		  double g, gp, gpp;
			g = (1d - nPeriodDiscount) / x;
			gp = -g / x + nbFixedPeriod * nPeriodDiscount / (x * periodFactor);
			gpp = 2d / (x * x) * g - 2d * nbFixedPeriod * nPeriodDiscount / (x * x * periodFactor) - (nbFixedPeriod + 1d) * nbFixedPeriod * nPeriodDiscount / (x * periodFactor * periodFactor);
		  double h = Math.Pow(1d + x, eta);
		  double hp = eta * h / periodFactor;
		  double hpp = (eta - 1d) * hp / periodFactor;
		  double kp = hp / g - h * gp / (g * g);
		  double kpp = hpp / g - 2d * hp * gp / (g * g) - h * (gpp / (g * g) - 2d * (gp * gp) / (g * g * g));
		  return new double[] {kp, kpp};
		}

		/// <summary>
		/// The Black price with numeraire 1 as function of the strike.
		/// </summary>
		/// <param name="strike">  the strike. </param>
		/// <returns> the Black price. </returns>
		internal virtual double bs(double strike)
		{
		  return sabrExtrapolation.price(strike + shift, PutCall.CALL);
		}
	  }

	}

}