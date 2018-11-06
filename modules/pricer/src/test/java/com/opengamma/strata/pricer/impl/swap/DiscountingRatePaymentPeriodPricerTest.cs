using System;
using System.Collections.Generic;

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
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.FxIndices.GBP_USD_WM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.datasets.RatesProviderDataSets.FX_MATRIX_GBP_USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.datasets.RatesProviderDataSets.MULTI_GBP_USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.datasets.RatesProviderDataSets.MULTI_GBP_USD_SIMPLE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.datasets.RatesProviderDataSets.VAL_DATE_2014_01_22;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.mock;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.when;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using DoubleMath = com.google.common.math.DoubleMath;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxMatrix = com.opengamma.strata.basics.currency.FxMatrix;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;
	using FxIndices = com.opengamma.strata.basics.index.FxIndices;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using IborIndexObservation = com.opengamma.strata.basics.index.IborIndexObservation;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProviderDataSets = com.opengamma.strata.pricer.datasets.RatesProviderDataSets;
	using FxIndexRates = com.opengamma.strata.pricer.fx.FxIndexRates;
	using IborRateSensitivity = com.opengamma.strata.pricer.rate.IborRateSensitivity;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RateComputationFn = com.opengamma.strata.pricer.rate.RateComputationFn;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SimpleRatesProvider = com.opengamma.strata.pricer.rate.SimpleRatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using FixedRateComputation = com.opengamma.strata.product.rate.FixedRateComputation;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;
	using CompoundingMethod = com.opengamma.strata.product.swap.CompoundingMethod;
	using FxReset = com.opengamma.strata.product.swap.FxReset;
	using NegativeRateMethod = com.opengamma.strata.product.swap.NegativeRateMethod;
	using RateAccrualPeriod = com.opengamma.strata.product.swap.RateAccrualPeriod;
	using RatePaymentPeriod = com.opengamma.strata.product.swap.RatePaymentPeriod;

	/// <summary>
	/// Test <seealso cref="DiscountingRatePaymentPeriodPricer"/>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingRatePaymentPeriodPricerTest
	public class DiscountingRatePaymentPeriodPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = VAL_DATE_2014_01_22;
	  private static readonly DayCount DAY_COUNT = DayCounts.ACT_360;
	  private static readonly LocalDate FX_DATE_1 = LocalDate.of(2014, 1, 22);
	  private static readonly LocalDate CPN_DATE_1 = LocalDate.of(2014, 1, 24);
	  private static readonly LocalDate CPN_DATE_2 = LocalDate.of(2014, 4, 24);
	  private static readonly LocalDate CPN_DATE_3 = LocalDate.of(2014, 7, 24);
	  private static readonly LocalDate CPN_DATE_4 = LocalDate.of(2014, 10, 24);
	  private const double ACCRUAL_FACTOR_1 = 0.245;
	  private const double ACCRUAL_FACTOR_2 = 0.255;
	  private const double ACCRUAL_FACTOR_3 = 0.25;
	  private const double GEARING = 2.0;
	  private const double SPREAD = -0.0025;
	  private const double NOTIONAL_100 = 1.0E8;
	  private static readonly LocalDate PAYMENT_DATE_1 = LocalDate.of(2014, 4, 26);
	  private static readonly LocalDate PAYMENT_DATE_3 = LocalDate.of(2014, 10, 26);
	  private const double RATE_1 = 0.0123d;
	  private const double RATE_2 = 0.0127d;
	  private const double RATE_3 = 0.0135d;
	  private const double RATE_FX = 1.6d;
	  private const double DISCOUNT_FACTOR = 0.976d;
	  private const double TOLERANCE_PV = 1E-7;

	  private const double EPS_FD = 1.0e-7;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator CAL_FD = new RatesFiniteDifferenceSensitivityCalculator(EPS_FD);

	  private static readonly double FX_RATE = FX_MATRIX_GBP_USD.fxRate(GBP, USD);
	  private static readonly FxMatrix FX_MATRIX_BUMP = FxMatrix.of(GBP, USD, FX_MATRIX_GBP_USD.fxRate(GBP, USD) + EPS_FD);

	  private static readonly RateAccrualPeriod ACCRUAL_PERIOD_1 = RateAccrualPeriod.builder().startDate(CPN_DATE_1).endDate(CPN_DATE_2).yearFraction(ACCRUAL_FACTOR_1).rateComputation(FixedRateComputation.of(RATE_1)).build();
	  private static readonly RateAccrualPeriod ACCRUAL_PERIOD_1_GS = RateAccrualPeriod.builder().startDate(CPN_DATE_1).endDate(CPN_DATE_2).yearFraction(ACCRUAL_FACTOR_1).rateComputation(FixedRateComputation.of(RATE_1)).gearing(GEARING).spread(SPREAD).build();
	  private static readonly RateAccrualPeriod ACCRUAL_PERIOD_1_NEG = RateAccrualPeriod.builder().startDate(CPN_DATE_1).endDate(CPN_DATE_2).yearFraction(ACCRUAL_FACTOR_1).rateComputation(FixedRateComputation.of(RATE_1)).gearing(-1d).negativeRateMethod(NegativeRateMethod.NOT_NEGATIVE).build();
	  private static readonly RateAccrualPeriod ACCRUAL_PERIOD_2_GS = RateAccrualPeriod.builder().startDate(CPN_DATE_2).endDate(CPN_DATE_3).yearFraction(ACCRUAL_FACTOR_2).rateComputation(FixedRateComputation.of(RATE_2)).gearing(GEARING).spread(SPREAD).build();
	  private static readonly RateAccrualPeriod ACCRUAL_PERIOD_3_GS = RateAccrualPeriod.builder().startDate(CPN_DATE_3).endDate(CPN_DATE_4).yearFraction(ACCRUAL_FACTOR_3).rateComputation(FixedRateComputation.of(RATE_3)).gearing(GEARING).spread(SPREAD).build();

	  private static readonly RatePaymentPeriod PAYMENT_PERIOD_1 = RatePaymentPeriod.builder().paymentDate(PAYMENT_DATE_1).accrualPeriods(ImmutableList.of(ACCRUAL_PERIOD_1)).dayCount(ACT_365F).currency(USD).notional(NOTIONAL_100).build();
	  private static readonly RatePaymentPeriod PAYMENT_PERIOD_1_FX = RatePaymentPeriod.builder().paymentDate(PAYMENT_DATE_1).accrualPeriods(ImmutableList.of(ACCRUAL_PERIOD_1)).dayCount(ACT_365F).currency(USD).notional(NOTIONAL_100).fxReset(FxReset.of(FxIndexObservation.of(GBP_USD_WM, FX_DATE_1, REF_DATA), GBP)).build();
	  private static readonly RatePaymentPeriod PAYMENT_PERIOD_1_GS = RatePaymentPeriod.builder().paymentDate(PAYMENT_DATE_1).accrualPeriods(ImmutableList.of(ACCRUAL_PERIOD_1_GS)).dayCount(ACT_365F).currency(USD).notional(NOTIONAL_100).build();
	  private static readonly RatePaymentPeriod PAYMENT_PERIOD_1_NEG = RatePaymentPeriod.builder().paymentDate(PAYMENT_DATE_1).accrualPeriods(ImmutableList.of(ACCRUAL_PERIOD_1_NEG)).dayCount(ACT_365F).currency(USD).notional(NOTIONAL_100).build();

	  private static readonly RatePaymentPeriod PAYMENT_PERIOD_FULL_GS = RatePaymentPeriod.builder().paymentDate(PAYMENT_DATE_3).accrualPeriods(ImmutableList.of(ACCRUAL_PERIOD_1_GS, ACCRUAL_PERIOD_2_GS, ACCRUAL_PERIOD_3_GS)).dayCount(ACT_365F).currency(USD).notional(NOTIONAL_100).build();
	  private static readonly RatePaymentPeriod PAYMENT_PERIOD_FULL_GS_FX_USD = RatePaymentPeriod.builder().paymentDate(PAYMENT_DATE_3).accrualPeriods(ImmutableList.of(ACCRUAL_PERIOD_1_GS, ACCRUAL_PERIOD_2_GS, ACCRUAL_PERIOD_3_GS)).dayCount(ACT_365F).currency(USD).notional(NOTIONAL_100).fxReset(FxReset.of(FxIndexObservation.of(GBP_USD_WM, FX_DATE_1, REF_DATA), GBP)).build();
	  private static readonly RatePaymentPeriod PAYMENT_PERIOD_FULL_GS_FX_GBP = RatePaymentPeriod.builder().paymentDate(PAYMENT_DATE_3).accrualPeriods(ImmutableList.of(ACCRUAL_PERIOD_1_GS, ACCRUAL_PERIOD_2_GS, ACCRUAL_PERIOD_3_GS)).dayCount(ACT_365F).currency(GBP).notional(NOTIONAL_100).fxReset(FxReset.of(FxIndexObservation.of(GBP_USD_WM, FX_DATE_1, REF_DATA), USD)).build();

	  // all tests use a fixed rate to avoid excessive use of mocks
	  // rate observation is separated from this class, so nothing is missed in unit test terms
	  // most testing on forecastValue as methods only differ in discountFactor
	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue_single()
	  {
		SimpleRatesProvider prov = createProvider(VAL_DATE);

		double pvExpected = RATE_1 * ACCRUAL_FACTOR_1 * NOTIONAL_100 * DISCOUNT_FACTOR;
		double pvComputed = DiscountingRatePaymentPeriodPricer.DEFAULT.presentValue(PAYMENT_PERIOD_1, prov);
		assertEquals(pvComputed, pvExpected, TOLERANCE_PV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_forecastValue_single()
	  {
		SimpleRatesProvider prov = createProvider(VAL_DATE);

		double fvExpected = RATE_1 * ACCRUAL_FACTOR_1 * NOTIONAL_100;
		double fvComputed = DiscountingRatePaymentPeriodPricer.DEFAULT.forecastValue(PAYMENT_PERIOD_1, prov);
		assertEquals(fvComputed, fvExpected, TOLERANCE_PV);
	  }

	  public virtual void test_forecastValue_single_fx()
	  {
		SimpleRatesProvider prov = createProvider(VAL_DATE);

		double fvExpected = RATE_1 * ACCRUAL_FACTOR_1 * NOTIONAL_100 * RATE_FX;
		double fvComputed = DiscountingRatePaymentPeriodPricer.DEFAULT.forecastValue(PAYMENT_PERIOD_1_FX, prov);
		assertEquals(fvComputed, fvExpected, TOLERANCE_PV);
	  }

	  public virtual void test_forecastValue_single_gearingSpread()
	  {
		SimpleRatesProvider prov = createProvider(VAL_DATE);

		double fvExpected = (RATE_1 * GEARING + SPREAD) * ACCRUAL_FACTOR_1 * NOTIONAL_100;
		double fvComputed = DiscountingRatePaymentPeriodPricer.DEFAULT.forecastValue(PAYMENT_PERIOD_1_GS, prov);
		assertEquals(fvComputed, fvExpected, TOLERANCE_PV);
	  }

	  public virtual void test_forecastValue_single_gearingNoNegative()
	  {
		SimpleRatesProvider prov = createProvider(VAL_DATE);

		double fvComputed = DiscountingRatePaymentPeriodPricer.DEFAULT.forecastValue(PAYMENT_PERIOD_1_NEG, prov);
		assertEquals(fvComputed, 0d, TOLERANCE_PV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_forecastValue_compoundNone()
	  {
		SimpleRatesProvider prov = createProvider(VAL_DATE);

		double fvExpected = ((RATE_1 * GEARING + SPREAD) * ACCRUAL_FACTOR_1 * NOTIONAL_100) + ((RATE_2 * GEARING + SPREAD) * ACCRUAL_FACTOR_2 * NOTIONAL_100) + ((RATE_3 * GEARING + SPREAD) * ACCRUAL_FACTOR_3 * NOTIONAL_100);
		double fvComputed = DiscountingRatePaymentPeriodPricer.DEFAULT.forecastValue(PAYMENT_PERIOD_FULL_GS, prov);
		assertEquals(fvComputed, fvExpected, TOLERANCE_PV);
	  }

	  public virtual void test_forecastValue_compoundNone_fx()
	  {
		SimpleRatesProvider prov = createProvider(VAL_DATE);

		double fvExpected = ((RATE_1 * GEARING + SPREAD) * ACCRUAL_FACTOR_1 * NOTIONAL_100 * RATE_FX) + ((RATE_2 * GEARING + SPREAD) * ACCRUAL_FACTOR_2 * NOTIONAL_100 * RATE_FX) + ((RATE_3 * GEARING + SPREAD) * ACCRUAL_FACTOR_3 * NOTIONAL_100 * RATE_FX);
		double fvComputed = DiscountingRatePaymentPeriodPricer.DEFAULT.forecastValue(PAYMENT_PERIOD_FULL_GS_FX_USD, prov);
		assertEquals(fvComputed, fvExpected, TOLERANCE_PV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_forecastValue_compoundStraight()
	  {
		SimpleRatesProvider prov = createProvider(VAL_DATE);

		RatePaymentPeriod period = PAYMENT_PERIOD_FULL_GS.toBuilder().compoundingMethod(CompoundingMethod.STRAIGHT).build();
		double invFactor1 = 1.0d + ACCRUAL_FACTOR_1 * (RATE_1 * GEARING + SPREAD);
		double invFactor2 = 1.0d + ACCRUAL_FACTOR_2 * (RATE_2 * GEARING + SPREAD);
		double invFactor3 = 1.0d + ACCRUAL_FACTOR_3 * (RATE_3 * GEARING + SPREAD);
		double fvExpected = NOTIONAL_100 * (invFactor1 * invFactor2 * invFactor3 - 1.0d);
		double fvComputed = DiscountingRatePaymentPeriodPricer.DEFAULT.forecastValue(period, prov);
		assertEquals(fvComputed, fvExpected, TOLERANCE_PV);
	  }

	  public virtual void test_forecastValue_compoundFlat()
	  {
		SimpleRatesProvider prov = createProvider(VAL_DATE);

		RatePaymentPeriod period = PAYMENT_PERIOD_FULL_GS.toBuilder().compoundingMethod(CompoundingMethod.FLAT).build();
		double cpa1 = NOTIONAL_100 * ACCRUAL_FACTOR_1 * (RATE_1 * GEARING + SPREAD);
		double cpa2 = NOTIONAL_100 * ACCRUAL_FACTOR_2 * (RATE_2 * GEARING + SPREAD) + cpa1 * ACCRUAL_FACTOR_2 * (RATE_2 * GEARING);
		double cpa3 = NOTIONAL_100 * ACCRUAL_FACTOR_3 * (RATE_3 * GEARING + SPREAD) + (cpa1 + cpa2) * ACCRUAL_FACTOR_3 * (RATE_3 * GEARING);
		double fvExpected = cpa1 + cpa2 + cpa3;
		double fvComputed = DiscountingRatePaymentPeriodPricer.DEFAULT.forecastValue(period, prov);
		assertEquals(fvComputed, fvExpected, TOLERANCE_PV);
	  }

	  public virtual void test_forecastValue_compoundFlat_notional()
	  {
		SimpleRatesProvider prov = createProvider(VAL_DATE);

		RatePaymentPeriod periodNot = PAYMENT_PERIOD_FULL_GS.toBuilder().compoundingMethod(CompoundingMethod.FLAT).build();
		RatePaymentPeriod period1 = PAYMENT_PERIOD_FULL_GS.toBuilder().compoundingMethod(CompoundingMethod.FLAT).notional(1.0d).build();
		double fvComputedNot = DiscountingRatePaymentPeriodPricer.DEFAULT.forecastValue(periodNot, prov);
		double fvComputed1 = DiscountingRatePaymentPeriodPricer.DEFAULT.forecastValue(period1, prov);
		assertEquals(fvComputedNot, fvComputed1 * NOTIONAL_100, TOLERANCE_PV);
	  }

	  public virtual void test_forecastValue_compoundSpreadExclusive()
	  {
		SimpleRatesProvider prov = createProvider(VAL_DATE);

		RatePaymentPeriod period = PAYMENT_PERIOD_FULL_GS.toBuilder().compoundingMethod(CompoundingMethod.SPREAD_EXCLUSIVE).build();
		double invFactor1 = 1.0d + ACCRUAL_FACTOR_1 * (RATE_1 * GEARING);
		double invFactor2 = 1.0d + ACCRUAL_FACTOR_2 * (RATE_2 * GEARING);
		double invFactor3 = 1.0d + ACCRUAL_FACTOR_3 * (RATE_3 * GEARING);
		double fvExpected = NOTIONAL_100 * (invFactor1 * invFactor2 * invFactor3 - 1.0d + (ACCRUAL_FACTOR_1 + ACCRUAL_FACTOR_2 + ACCRUAL_FACTOR_3) * SPREAD);
		double fvComputed = DiscountingRatePaymentPeriodPricer.DEFAULT.forecastValue(period, prov);
		assertEquals(fvComputed, fvExpected, TOLERANCE_PV);
	  }

	  public virtual void test_forecastValue_compoundSpreadExclusive_fx()
	  {
		SimpleRatesProvider prov = createProvider(VAL_DATE);

		RatePaymentPeriod period = PAYMENT_PERIOD_FULL_GS_FX_USD.toBuilder().compoundingMethod(CompoundingMethod.SPREAD_EXCLUSIVE).build();
		double invFactor1 = 1.0d + ACCRUAL_FACTOR_1 * (RATE_1 * GEARING);
		double invFactor2 = 1.0d + ACCRUAL_FACTOR_2 * (RATE_2 * GEARING);
		double invFactor3 = 1.0d + ACCRUAL_FACTOR_3 * (RATE_3 * GEARING);
		double fvExpected = NOTIONAL_100 * RATE_FX * (invFactor1 * invFactor2 * invFactor3 - 1.0d + (ACCRUAL_FACTOR_1 + ACCRUAL_FACTOR_2 + ACCRUAL_FACTOR_3) * SPREAD);
		double fvComputed = DiscountingRatePaymentPeriodPricer.DEFAULT.forecastValue(period, prov);
		assertEquals(fvComputed, fvExpected, TOLERANCE_PV);
	  }

	  //-------------------------------------------------------------------------
	  private static readonly RateAccrualPeriod ACCRUAL_PERIOD_1_FLOATING = RateAccrualPeriod.builder().startDate(CPN_DATE_1).endDate(CPN_DATE_2).yearFraction(ACCRUAL_FACTOR_1).rateComputation(IborRateComputation.of(GBP_LIBOR_3M, CPN_DATE_1, REF_DATA)).gearing(GEARING).spread(SPREAD).build();
	  private static readonly RateAccrualPeriod ACCRUAL_PERIOD_2_FLOATING = RateAccrualPeriod.builder().startDate(CPN_DATE_2).endDate(CPN_DATE_3).yearFraction(ACCRUAL_FACTOR_2).rateComputation(IborRateComputation.of(GBP_LIBOR_3M, CPN_DATE_2, REF_DATA)).gearing(GEARING).spread(SPREAD).build();
	  private static readonly RateAccrualPeriod ACCRUAL_PERIOD_3_FLOATING = RateAccrualPeriod.builder().startDate(CPN_DATE_3).endDate(CPN_DATE_4).yearFraction(ACCRUAL_FACTOR_3).rateComputation(IborRateComputation.of(GBP_LIBOR_3M, CPN_DATE_3, REF_DATA)).gearing(GEARING).spread(SPREAD).build();
	  private static readonly RatePaymentPeriod PAYMENT_PERIOD_FLOATING = RatePaymentPeriod.builder().paymentDate(PAYMENT_DATE_3).accrualPeriods(ImmutableList.of(ACCRUAL_PERIOD_1_FLOATING, ACCRUAL_PERIOD_2_FLOATING, ACCRUAL_PERIOD_3_FLOATING)).dayCount(ACT_365F).currency(USD).notional(NOTIONAL_100).build();
	  private static readonly RatePaymentPeriod PAYMENT_PERIOD_COMPOUNDING_STRAIGHT = RatePaymentPeriod.builder().paymentDate(PAYMENT_DATE_3).accrualPeriods(ImmutableList.of(ACCRUAL_PERIOD_1_FLOATING, ACCRUAL_PERIOD_2_FLOATING, ACCRUAL_PERIOD_3_FLOATING)).compoundingMethod(CompoundingMethod.STRAIGHT).dayCount(ACT_365F).currency(USD).notional(NOTIONAL_100).build();
	  private static readonly RatePaymentPeriod PAYMENT_PERIOD_COMPOUNDING_FLAT = RatePaymentPeriod.builder().paymentDate(PAYMENT_DATE_3).accrualPeriods(ImmutableList.of(ACCRUAL_PERIOD_1_FLOATING, ACCRUAL_PERIOD_2_FLOATING, ACCRUAL_PERIOD_3_FLOATING)).compoundingMethod(CompoundingMethod.FLAT).dayCount(ACT_365F).currency(USD).notional(NOTIONAL_100).build();
	  private static readonly RatePaymentPeriod PAYMENT_PERIOD_COMPOUNDING_EXCLUSIVE = RatePaymentPeriod.builder().paymentDate(PAYMENT_DATE_3).accrualPeriods(ImmutableList.of(ACCRUAL_PERIOD_1_FLOATING, ACCRUAL_PERIOD_2_FLOATING, ACCRUAL_PERIOD_3_FLOATING)).compoundingMethod(CompoundingMethod.SPREAD_EXCLUSIVE).dayCount(ACT_365F).currency(USD).notional(NOTIONAL_100).build();

	  /// <summary>
	  /// Test present value sensitivity for ibor, no compounding.
	  /// </summary>
	  public virtual void test_presentValueSensitivity_ibor_noCompounding()
	  {
		LocalDate valDate = PAYMENT_PERIOD_FLOATING.PaymentDate.minusDays(90);
		double paymentTime = DAY_COUNT.relativeYearFraction(valDate, PAYMENT_PERIOD_FLOATING.PaymentDate);
		DiscountFactors mockDf = mock(typeof(DiscountFactors));
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(valDate, mockDf);
		simpleProv.DayCount = DAY_COUNT;
		RateComputationFn<RateComputation> obsFunc = mock(typeof(RateComputationFn));

		when(mockDf.discountFactor(PAYMENT_PERIOD_FLOATING.PaymentDate)).thenReturn(DISCOUNT_FACTOR);
		ZeroRateSensitivity builder = ZeroRateSensitivity.of(PAYMENT_PERIOD_FLOATING.Currency, paymentTime, -DISCOUNT_FACTOR * paymentTime);
		when(mockDf.zeroRatePointSensitivity(PAYMENT_PERIOD_FLOATING.PaymentDate)).thenReturn(builder);

		DiscountingRatePaymentPeriodPricer pricer = new DiscountingRatePaymentPeriodPricer(obsFunc);
		LocalDate[] dates = new LocalDate[] {CPN_DATE_1, CPN_DATE_2, CPN_DATE_3, CPN_DATE_4};
		double[] rates = new double[] {RATE_1, RATE_2, RATE_3};
		for (int i = 0; i < 3; ++i)
		{
		  IborRateComputation rateComputation = (IborRateComputation) PAYMENT_PERIOD_FLOATING.AccrualPeriods.get(i).RateComputation;
		  IborRateSensitivity iborSense = IborRateSensitivity.of(rateComputation.Observation, 1d);
		  when(obsFunc.rateSensitivity(rateComputation, dates[i], dates[i + 1], simpleProv)).thenReturn(iborSense);
		  when(obsFunc.rate(rateComputation, dates[i], dates[i + 1], simpleProv)).thenReturn(rates[i]);
		}
		PointSensitivities senseComputed = pricer.presentValueSensitivity(PAYMENT_PERIOD_FLOATING, simpleProv).build();

		double eps = 1.e-7;
		IList<IborRateSensitivity> senseExpectedList = futureFwdSensitivityFD(simpleProv, PAYMENT_PERIOD_FLOATING, obsFunc, eps);
		PointSensitivities senseExpected = PointSensitivities.of(senseExpectedList).multipliedBy(DISCOUNT_FACTOR);
		IList<ZeroRateSensitivity> dscExpectedList = dscSensitivityFD(simpleProv, PAYMENT_PERIOD_FLOATING, obsFunc, eps);
		PointSensitivities senseExpectedDsc = PointSensitivities.of(dscExpectedList);

		assertTrue(senseComputed.equalWithTolerance(senseExpected.combinedWith(senseExpectedDsc), eps * PAYMENT_PERIOD_FLOATING.Notional));
	  }

	  /// <summary>
	  /// test forecast value sensitivity for ibor, no compounding.
	  /// </summary>
	  public virtual void test_forecastValueSensitivity_ibor_noCompounding()
	  {
		RatesProvider mockProv = mock(typeof(RatesProvider));
		RateComputationFn<RateComputation> obsFunc = mock(typeof(RateComputationFn));

		when(mockProv.ValuationDate).thenReturn(VAL_DATE);
		DiscountingRatePaymentPeriodPricer pricer = new DiscountingRatePaymentPeriodPricer(obsFunc);
		LocalDate[] dates = new LocalDate[] {CPN_DATE_1, CPN_DATE_2, CPN_DATE_3, CPN_DATE_4};
		double[] rates = new double[] {RATE_1, RATE_2, RATE_3};
		for (int i = 0; i < 3; ++i)
		{
		  IborRateComputation rateObs = (IborRateComputation) PAYMENT_PERIOD_FLOATING.AccrualPeriods.get(i).RateComputation;
		  IborRateSensitivity iborSense = IborRateSensitivity.of(rateObs.Observation, 1.0d);
		  when(obsFunc.rateSensitivity(rateObs, dates[i], dates[i + 1], mockProv)).thenReturn(iborSense);
		  when(obsFunc.rate(rateObs, dates[i], dates[i + 1], mockProv)).thenReturn(rates[i]);
		}
		PointSensitivities senseComputed = pricer.forecastValueSensitivity(PAYMENT_PERIOD_FLOATING, mockProv).build();

		double eps = 1.e-7;
		IList<IborRateSensitivity> senseExpectedList = futureFwdSensitivityFD(mockProv, PAYMENT_PERIOD_FLOATING, obsFunc, eps);
		PointSensitivities senseExpected = PointSensitivities.of(senseExpectedList);
		assertTrue(senseComputed.equalWithTolerance(senseExpected, eps * PAYMENT_PERIOD_FLOATING.Notional));
	  }

	  // test forecast value sensitivity for ibor, with straight, flat and exclusive compounding.
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "compoundingRatePaymentPeriod") public static Object[][] data_forecastValueSensitivity_ibor_compounding()
	  public static object[][] data_forecastValueSensitivity_ibor_compounding()
	  {
		return new object[][]
		{
			new object[] {PAYMENT_PERIOD_COMPOUNDING_STRAIGHT},
			new object[] {PAYMENT_PERIOD_COMPOUNDING_FLAT},
			new object[] {PAYMENT_PERIOD_COMPOUNDING_EXCLUSIVE}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "compoundingRatePaymentPeriod") public void test_forecastValueSensitivity_ibor_compounding(com.opengamma.strata.product.swap.RatePaymentPeriod period)
	  public virtual void test_forecastValueSensitivity_ibor_compounding(RatePaymentPeriod period)
	  {
		RatesProvider mockProv = mock(typeof(RatesProvider));
		RateComputationFn<RateComputation> obsFunc = mock(typeof(RateComputationFn));
		when(mockProv.ValuationDate).thenReturn(VAL_DATE);
		DiscountingRatePaymentPeriodPricer pricer = new DiscountingRatePaymentPeriodPricer(obsFunc);
		LocalDate[] dates = new LocalDate[] {CPN_DATE_1, CPN_DATE_2, CPN_DATE_3, CPN_DATE_4};
		double[] rates = new double[] {RATE_1, RATE_2, RATE_3};
		for (int i = 0; i < 3; ++i)
		{
		  IborRateComputation rateObs = (IborRateComputation) period.AccrualPeriods.get(i).RateComputation;
		  IborRateSensitivity iborSense = IborRateSensitivity.of(rateObs.Observation, 1.0d);
		  when(obsFunc.rateSensitivity(rateObs, dates[i], dates[i + 1], mockProv)).thenReturn(iborSense);
		  when(obsFunc.rate(rateObs, dates[i], dates[i + 1], mockProv)).thenReturn(rates[i]);
		}
		PointSensitivities senseComputed = pricer.forecastValueSensitivity(period, mockProv).build();
		IList<IborRateSensitivity> senseExpectedList = futureFwdSensitivityFD(mockProv, period, obsFunc, EPS_FD);
		PointSensitivities senseExpected = PointSensitivities.of(senseExpectedList);
		assertTrue(senseComputed.equalWithTolerance(senseExpected, EPS_FD * period.Notional));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_forecastValueSensitivity_compoundNone_fx()
	  {
		DiscountingRatePaymentPeriodPricer pricer = DiscountingRatePaymentPeriodPricer.DEFAULT;
		ImmutableRatesProvider provider = MULTI_GBP_USD;
		PointSensitivityBuilder pointSensiComputedUSD = pricer.forecastValueSensitivity(PAYMENT_PERIOD_FULL_GS_FX_USD, provider);
		CurrencyParameterSensitivities sensiComputedUSD = provider.parameterSensitivity(pointSensiComputedUSD.build().normalized());
		CurrencyParameterSensitivities sensiExpectedUSD = CAL_FD.sensitivity(provider, (p) => CurrencyAmount.of(USD, pricer.forecastValue(PAYMENT_PERIOD_FULL_GS_FX_USD, (p))));
		assertTrue(sensiComputedUSD.equalWithTolerance(sensiExpectedUSD, EPS_FD * PAYMENT_PERIOD_FULL_GS_FX_USD.Notional));

		PointSensitivityBuilder pointSensiComputedGBP = pricer.forecastValueSensitivity(PAYMENT_PERIOD_FULL_GS_FX_GBP, provider);
		CurrencyParameterSensitivities sensiComputedGBP = provider.parameterSensitivity(pointSensiComputedGBP.build().normalized());
		CurrencyParameterSensitivities sensiExpectedGBP = CAL_FD.sensitivity(provider, (p) => CurrencyAmount.of(GBP, pricer.forecastValue(PAYMENT_PERIOD_FULL_GS_FX_GBP, (p))));
		assertTrue(sensiComputedGBP.equalWithTolerance(sensiExpectedGBP, EPS_FD * PAYMENT_PERIOD_FULL_GS_FX_GBP.Notional));
	  }

	  public virtual void test_presentValueSensitivity_compoundNone_fx()
	  {
		DiscountingRatePaymentPeriodPricer pricer = DiscountingRatePaymentPeriodPricer.DEFAULT;
		ImmutableRatesProvider provider = MULTI_GBP_USD;
		PointSensitivityBuilder pointSensiComputedUSD = pricer.presentValueSensitivity(PAYMENT_PERIOD_FULL_GS_FX_USD, provider);
		CurrencyParameterSensitivities sensiComputedUSD = provider.parameterSensitivity(pointSensiComputedUSD.build().normalized());
		CurrencyParameterSensitivities sensiExpectedUSD = CAL_FD.sensitivity(provider, (p) => CurrencyAmount.of(USD, pricer.presentValue(PAYMENT_PERIOD_FULL_GS_FX_USD, (p))));
		assertTrue(sensiComputedUSD.equalWithTolerance(sensiExpectedUSD, EPS_FD * PAYMENT_PERIOD_FULL_GS_FX_USD.Notional));

		PointSensitivityBuilder pointSensiComputedGBP = pricer.presentValueSensitivity(PAYMENT_PERIOD_FULL_GS_FX_GBP, provider);
		CurrencyParameterSensitivities sensiComputedGBP = provider.parameterSensitivity(pointSensiComputedGBP.build().normalized());
		CurrencyParameterSensitivities sensiExpectedGBP = CAL_FD.sensitivity(provider, (p) => CurrencyAmount.of(GBP, pricer.presentValue(PAYMENT_PERIOD_FULL_GS_FX_GBP, (p))));
		assertTrue(sensiComputedGBP.equalWithTolerance(sensiExpectedGBP, EPS_FD * PAYMENT_PERIOD_FULL_GS_FX_GBP.Notional));
	  }

	  public virtual void test_forecastValueSensitivity_compoundNone_fx_dfCurve()
	  {
		DiscountingRatePaymentPeriodPricer pricer = DiscountingRatePaymentPeriodPricer.DEFAULT;
		ImmutableRatesProvider provider = MULTI_GBP_USD_SIMPLE;
		PointSensitivityBuilder pointSensiComputedUSD = pricer.forecastValueSensitivity(PAYMENT_PERIOD_FULL_GS_FX_USD, provider);
		CurrencyParameterSensitivities sensiComputedUSD = provider.parameterSensitivity(pointSensiComputedUSD.build().normalized());
		CurrencyParameterSensitivities sensiExpectedUSD = CAL_FD.sensitivity(provider, (p) => CurrencyAmount.of(USD, pricer.forecastValue(PAYMENT_PERIOD_FULL_GS_FX_USD, (p))));
		assertTrue(sensiComputedUSD.equalWithTolerance(sensiExpectedUSD, EPS_FD * PAYMENT_PERIOD_FULL_GS_FX_USD.Notional));

		PointSensitivityBuilder pointSensiComputedGBP = pricer.forecastValueSensitivity(PAYMENT_PERIOD_FULL_GS_FX_GBP, provider);
		CurrencyParameterSensitivities sensiComputedGBP = provider.parameterSensitivity(pointSensiComputedGBP.build().normalized());
		CurrencyParameterSensitivities sensiExpectedGBP = CAL_FD.sensitivity(provider, (p) => CurrencyAmount.of(GBP, pricer.forecastValue(PAYMENT_PERIOD_FULL_GS_FX_GBP, (p))));
		assertTrue(sensiComputedGBP.equalWithTolerance(sensiExpectedGBP, EPS_FD * PAYMENT_PERIOD_FULL_GS_FX_GBP.Notional));
	  }

	  public virtual void test_presentValueSensitivity_compoundNone_fx_dfCurve()
	  {
		DiscountingRatePaymentPeriodPricer pricer = DiscountingRatePaymentPeriodPricer.DEFAULT;
		ImmutableRatesProvider provider = MULTI_GBP_USD_SIMPLE;
		PointSensitivityBuilder pointSensiComputedUSD = pricer.presentValueSensitivity(PAYMENT_PERIOD_FULL_GS_FX_USD, provider);
		CurrencyParameterSensitivities sensiComputedUSD = provider.parameterSensitivity(pointSensiComputedUSD.build().normalized());
		CurrencyParameterSensitivities sensiExpectedUSD = CAL_FD.sensitivity(provider, (p) => CurrencyAmount.of(USD, pricer.presentValue(PAYMENT_PERIOD_FULL_GS_FX_USD, (p))));
		assertTrue(sensiComputedUSD.equalWithTolerance(sensiExpectedUSD, EPS_FD * PAYMENT_PERIOD_FULL_GS_FX_USD.Notional));

		PointSensitivityBuilder pointSensiComputedGBP = pricer.presentValueSensitivity(PAYMENT_PERIOD_FULL_GS_FX_GBP, provider);
		CurrencyParameterSensitivities sensiComputedGBP = provider.parameterSensitivity(pointSensiComputedGBP.build().normalized());
		CurrencyParameterSensitivities sensiExpectedGBP = CAL_FD.sensitivity(provider, (p) => CurrencyAmount.of(GBP, pricer.presentValue(PAYMENT_PERIOD_FULL_GS_FX_GBP, (p))));
		assertTrue(sensiComputedGBP.equalWithTolerance(sensiExpectedGBP, EPS_FD * PAYMENT_PERIOD_FULL_GS_FX_GBP.Notional));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("null") private java.util.List<com.opengamma.strata.pricer.rate.IborRateSensitivity> futureFwdSensitivityFD(com.opengamma.strata.pricer.rate.RatesProvider provider, com.opengamma.strata.product.swap.RatePaymentPeriod payment, com.opengamma.strata.pricer.rate.RateComputationFn<com.opengamma.strata.product.rate.RateComputation> obsFunc, double eps)
	  private IList<IborRateSensitivity> futureFwdSensitivityFD(RatesProvider provider, RatePaymentPeriod payment, RateComputationFn<RateComputation> obsFunc, double eps)
	  {
		LocalDate valuationDate = provider.ValuationDate;
		RatesProvider provNew = mock(typeof(RatesProvider));
		when(provNew.ValuationDate).thenReturn(valuationDate);

		ImmutableList<RateAccrualPeriod> periods = payment.AccrualPeriods;
		int nPeriods = periods.size();
		IList<IborRateSensitivity> forwardRateSensi = new List<IborRateSensitivity>();
		for (int j = 0; j < nPeriods; ++j)
		{
		  RateComputationFn<RateComputation> obsFuncUp = mock(typeof(RateComputationFn));
		  RateComputationFn<RateComputation> obsFuncDown = mock(typeof(RateComputationFn));
		  IborIndex index = null;
		  LocalDate fixingDate = null;
		  for (int i = 0; i < nPeriods; ++i)
		  {
			RateAccrualPeriod period = periods.get(i);
			IborRateComputation observation = (IborRateComputation) period.RateComputation;
			double rate = obsFunc.rate(observation, period.StartDate, period.EndDate, provider);
			if (i == j)
			{
			  fixingDate = observation.FixingDate;
			  index = observation.Index;
			  when(obsFuncUp.rate(observation, period.StartDate, period.EndDate, provNew)).thenReturn(rate + eps);
			  when(obsFuncDown.rate(observation, period.StartDate, period.EndDate, provNew)).thenReturn(rate - eps);
			}
			else
			{
			  when(obsFuncUp.rate(observation, period.StartDate, period.EndDate, provNew)).thenReturn(rate);
			  when(obsFuncDown.rate(observation, period.StartDate, period.EndDate, provNew)).thenReturn(rate);
			}
		  }
		  DiscountingRatePaymentPeriodPricer pricerUp = new DiscountingRatePaymentPeriodPricer(obsFuncUp);
		  DiscountingRatePaymentPeriodPricer pricerDown = new DiscountingRatePaymentPeriodPricer(obsFuncDown);
		  double up = pricerUp.forecastValue(payment, provNew);
		  double down = pricerDown.forecastValue(payment, provNew);
		  IborRateSensitivity fwdSense = IborRateSensitivity.of(IborIndexObservation.of(index, fixingDate, REF_DATA), 0.5 * (up - down) / eps);
		  forwardRateSensi.Add(fwdSense);
		}
		return forwardRateSensi;
	  }

	  private IList<ZeroRateSensitivity> dscSensitivityFD(RatesProvider provider, RatePaymentPeriod payment, RateComputationFn<RateComputation> obsFunc, double eps)
	  {
		LocalDate valuationDate = provider.ValuationDate;
		LocalDate paymentDate = payment.PaymentDate;
		double discountFactor = provider.discountFactor(payment.Currency, paymentDate);
		double paymentTime = DAY_COUNT.relativeYearFraction(valuationDate, paymentDate);
		Currency currency = payment.Currency;

		RatesProvider provUp = mock(typeof(RatesProvider));
		RatesProvider provDw = mock(typeof(RatesProvider));
		RateComputationFn<RateComputation> obsFuncNewUp = mock(typeof(RateComputationFn));
		RateComputationFn<RateComputation> obsFuncNewDw = mock(typeof(RateComputationFn));
		when(provUp.ValuationDate).thenReturn(valuationDate);
		when(provDw.ValuationDate).thenReturn(valuationDate);
		when(provUp.discountFactor(currency, paymentDate)).thenReturn(discountFactor * Math.Exp(-eps * paymentTime));
		when(provDw.discountFactor(currency, paymentDate)).thenReturn(discountFactor * Math.Exp(eps * paymentTime));

		ImmutableList<RateAccrualPeriod> periods = payment.AccrualPeriods;
		for (int i = 0; i < periods.size(); ++i)
		{
		  RateComputation observation = periods.get(i).RateComputation;
		  LocalDate startDate = periods.get(i).StartDate;
		  LocalDate endDate = periods.get(i).EndDate;
		  double rate = obsFunc.rate(observation, startDate, endDate, provider);
		  when(obsFuncNewUp.rate(observation, startDate, endDate, provUp)).thenReturn(rate);
		  when(obsFuncNewDw.rate(observation, startDate, endDate, provDw)).thenReturn(rate);
		}

		DiscountingRatePaymentPeriodPricer pricerUp = new DiscountingRatePaymentPeriodPricer(obsFuncNewUp);
		DiscountingRatePaymentPeriodPricer pricerDw = new DiscountingRatePaymentPeriodPricer(obsFuncNewDw);
		double pvUp = pricerUp.presentValue(payment, provUp);
		double pvDw = pricerDw.presentValue(payment, provDw);
		double res = 0.5 * (pvUp - pvDw) / eps;
		IList<ZeroRateSensitivity> zeroRateSensi = new List<ZeroRateSensitivity>();
		zeroRateSensi.Add(ZeroRateSensitivity.of(currency, paymentTime, res));
		return zeroRateSensi;
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_accruedInterest_firstAccrualPeriod()
	  {
		LocalDate valDate = PAYMENT_PERIOD_FULL_GS.StartDate.plusDays(7);
		SimpleRatesProvider prov = createProvider(valDate);

		double partial = PAYMENT_PERIOD_FULL_GS.DayCount.yearFraction(ACCRUAL_PERIOD_1_GS.StartDate, valDate);
		double fraction = partial / ACCRUAL_FACTOR_1;
		double expected = ((RATE_1 * GEARING + SPREAD) * ACCRUAL_FACTOR_1 * NOTIONAL_100) * fraction;

		double computed = DiscountingRatePaymentPeriodPricer.DEFAULT.accruedInterest(PAYMENT_PERIOD_FULL_GS, prov);
		assertEquals(computed, expected, TOLERANCE_PV);
	  }

	  public virtual void test_accruedInterest_lastAccrualPeriod()
	  {
		LocalDate valDate = PAYMENT_PERIOD_FULL_GS.EndDate.minusDays(7);
		SimpleRatesProvider prov = createProvider(valDate);

		double partial = PAYMENT_PERIOD_FULL_GS.DayCount.yearFraction(ACCRUAL_PERIOD_3_GS.StartDate, valDate);
		double fraction = partial / ACCRUAL_FACTOR_3;
		double expected = ((RATE_1 * GEARING + SPREAD) * ACCRUAL_FACTOR_1 * NOTIONAL_100) + ((RATE_2 * GEARING + SPREAD) * ACCRUAL_FACTOR_2 * NOTIONAL_100) + ((RATE_3 * GEARING + SPREAD) * ACCRUAL_FACTOR_3 * NOTIONAL_100 * fraction);

		double computed = DiscountingRatePaymentPeriodPricer.DEFAULT.accruedInterest(PAYMENT_PERIOD_FULL_GS, prov);
		assertEquals(computed, expected, TOLERANCE_PV);
	  }

	  public virtual void test_accruedInterest_valDateBeforePeriod()
	  {
		SimpleRatesProvider prov = createProvider(PAYMENT_PERIOD_FULL_GS.StartDate);

		double computed = DiscountingRatePaymentPeriodPricer.DEFAULT.accruedInterest(PAYMENT_PERIOD_FULL_GS, prov);
		assertEquals(computed, 0, TOLERANCE_PV);
	  }

	  public virtual void test_accruedInterest_valDateAfterPeriod()
	  {
		SimpleRatesProvider prov = createProvider(PAYMENT_PERIOD_FULL_GS.EndDate.plusDays(1));

		double computed = DiscountingRatePaymentPeriodPricer.DEFAULT.accruedInterest(PAYMENT_PERIOD_FULL_GS, prov);
		assertEquals(computed, 0, TOLERANCE_PV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_explainPresentValue_single()
	  {
		RatesProvider prov = createProvider(VAL_DATE);

		DiscountingRatePaymentPeriodPricer test = DiscountingRatePaymentPeriodPricer.DEFAULT;
		ExplainMapBuilder builder = ExplainMap.builder();
		test.explainPresentValue(PAYMENT_PERIOD_1, prov, builder);
		ExplainMap explain = builder.build();

		Currency currency = PAYMENT_PERIOD_1.Currency;
		double ua = RATE_1 * ACCRUAL_FACTOR_1;
		double fv = ua * NOTIONAL_100;
		assertEquals(explain.get(ExplainKey.ENTRY_TYPE).get(), "RatePaymentPeriod");
		assertEquals(explain.get(ExplainKey.PAYMENT_DATE).get(), PAYMENT_PERIOD_1.PaymentDate);
		assertEquals(explain.get(ExplainKey.PAYMENT_CURRENCY).get(), currency);
		assertEquals(explain.get(ExplainKey.NOTIONAL).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.NOTIONAL).get().Amount, NOTIONAL_100, TOLERANCE_PV);
		assertEquals(explain.get(ExplainKey.TRADE_NOTIONAL).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.TRADE_NOTIONAL).get().Amount, NOTIONAL_100, TOLERANCE_PV);
		assertEquals(explain.get(ExplainKey.COMPOUNDING).get(), PAYMENT_PERIOD_1.CompoundingMethod);
		assertEquals(explain.get(ExplainKey.DISCOUNT_FACTOR).Value, DISCOUNT_FACTOR, TOLERANCE_PV);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Amount, fv, TOLERANCE_PV);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Amount, fv * DISCOUNT_FACTOR, TOLERANCE_PV);

		assertEquals(explain.get(ExplainKey.ACCRUAL_PERIODS).get().size(), 1);
		ExplainMap explainAccrual = explain.get(ExplainKey.ACCRUAL_PERIODS).get().get(0);
		RateAccrualPeriod ap = PAYMENT_PERIOD_1.AccrualPeriods.get(0);
		int daysBetween = (int) DAYS.between(ap.StartDate, ap.EndDate);
		assertEquals(explainAccrual.get(ExplainKey.ENTRY_TYPE).get(), "AccrualPeriod");
		assertEquals(explainAccrual.get(ExplainKey.START_DATE).get(), ap.StartDate);
		assertEquals(explainAccrual.get(ExplainKey.UNADJUSTED_START_DATE).get(), ap.UnadjustedStartDate);
		assertEquals(explainAccrual.get(ExplainKey.END_DATE).get(), ap.EndDate);
		assertEquals(explainAccrual.get(ExplainKey.UNADJUSTED_END_DATE).get(), ap.UnadjustedEndDate);
		assertEquals(explainAccrual.get(ExplainKey.ACCRUAL_YEAR_FRACTION).Value, ap.YearFraction);
		assertEquals(explainAccrual.get(ExplainKey.ACCRUAL_DAYS).Value, (int?) daysBetween);
		assertEquals(explainAccrual.get(ExplainKey.GEARING).Value, ap.Gearing, TOLERANCE_PV);
		assertEquals(explainAccrual.get(ExplainKey.SPREAD).Value, ap.Spread, TOLERANCE_PV);
		assertEquals(explainAccrual.get(ExplainKey.FIXED_RATE).Value, RATE_1, TOLERANCE_PV);
		assertEquals(explainAccrual.get(ExplainKey.PAY_OFF_RATE).Value, RATE_1, TOLERANCE_PV);
		assertEquals(explainAccrual.get(ExplainKey.UNIT_AMOUNT).Value, ua, TOLERANCE_PV);
	  }

	  public virtual void test_explainPresentValue_single_paymentDateInPast()
	  {
		SimpleRatesProvider prov = createProvider(VAL_DATE);
		prov.ValuationDate = VAL_DATE.plusYears(1);

		DiscountingRatePaymentPeriodPricer test = DiscountingRatePaymentPeriodPricer.DEFAULT;
		ExplainMapBuilder builder = ExplainMap.builder();
		test.explainPresentValue(PAYMENT_PERIOD_1, prov, builder);
		ExplainMap explain = builder.build();

		Currency currency = PAYMENT_PERIOD_1.Currency;
		assertEquals(explain.get(ExplainKey.ENTRY_TYPE).get(), "RatePaymentPeriod");
		assertEquals(explain.get(ExplainKey.PAYMENT_DATE).get(), PAYMENT_PERIOD_1.PaymentDate);
		assertEquals(explain.get(ExplainKey.PAYMENT_CURRENCY).get(), currency);
		assertEquals(explain.get(ExplainKey.NOTIONAL).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.NOTIONAL).get().Amount, NOTIONAL_100, TOLERANCE_PV);
		assertEquals(explain.get(ExplainKey.TRADE_NOTIONAL).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.TRADE_NOTIONAL).get().Amount, NOTIONAL_100, TOLERANCE_PV);
		assertEquals(explain.get(ExplainKey.COMPLETED).Value, true);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Amount, 0d, TOLERANCE_PV);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Amount, 0d, TOLERANCE_PV);
	  }

	  public virtual void test_explainPresentValue_single_fx()
	  {
		RatesProvider prov = createProvider(VAL_DATE);

		DiscountingRatePaymentPeriodPricer test = DiscountingRatePaymentPeriodPricer.DEFAULT;
		ExplainMapBuilder builder = ExplainMap.builder();
		test.explainPresentValue(PAYMENT_PERIOD_1_FX, prov, builder);
		ExplainMap explain = builder.build();

		FxReset fxReset = PAYMENT_PERIOD_1_FX.FxReset.get();
		Currency currency = PAYMENT_PERIOD_1_FX.Currency;
		Currency referenceCurrency = fxReset.ReferenceCurrency;
		double ua = RATE_1 * ACCRUAL_FACTOR_1;
		double fv = ua * NOTIONAL_100 * RATE_FX;
		assertEquals(explain.get(ExplainKey.ENTRY_TYPE).get(), "RatePaymentPeriod");
		assertEquals(explain.get(ExplainKey.PAYMENT_DATE).get(), PAYMENT_PERIOD_1_FX.PaymentDate);
		assertEquals(explain.get(ExplainKey.PAYMENT_CURRENCY).get(), currency);
		assertEquals(explain.get(ExplainKey.NOTIONAL).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.NOTIONAL).get().Amount, NOTIONAL_100 * RATE_FX, TOLERANCE_PV);
		assertEquals(explain.get(ExplainKey.TRADE_NOTIONAL).get().Currency, referenceCurrency);
		assertEquals(explain.get(ExplainKey.TRADE_NOTIONAL).get().Amount, NOTIONAL_100, TOLERANCE_PV);
		assertEquals(explain.get(ExplainKey.COMPOUNDING).get(), PAYMENT_PERIOD_1_FX.CompoundingMethod);
		assertEquals(explain.get(ExplainKey.DISCOUNT_FACTOR).Value, DISCOUNT_FACTOR, TOLERANCE_PV);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Amount, fv, TOLERANCE_PV);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Amount, fv * DISCOUNT_FACTOR, TOLERANCE_PV);
		assertEquals(explain.get(ExplainKey.OBSERVATIONS).get().size(), 1);
		ExplainMap explainFxObs = explain.get(ExplainKey.OBSERVATIONS).get().get(0);
		assertEquals(explainFxObs.get(ExplainKey.ENTRY_TYPE).get(), "FxObservation");
		assertEquals(explainFxObs.get(ExplainKey.INDEX).get(), fxReset.Index);
		assertEquals(explainFxObs.get(ExplainKey.FIXING_DATE).get(), fxReset.Observation.FixingDate);
		assertEquals(explainFxObs.get(ExplainKey.INDEX_VALUE).Value, RATE_FX, TOLERANCE_PV);

		assertEquals(explain.get(ExplainKey.ACCRUAL_PERIODS).get().size(), 1);
		ExplainMap explainAccrual = explain.get(ExplainKey.ACCRUAL_PERIODS).get().get(0);
		RateAccrualPeriod ap = PAYMENT_PERIOD_1_FX.AccrualPeriods.get(0);
		int daysBetween = (int) DAYS.between(ap.StartDate, ap.EndDate);
		assertEquals(explainAccrual.get(ExplainKey.ENTRY_TYPE).get(), "AccrualPeriod");
		assertEquals(explainAccrual.get(ExplainKey.START_DATE).get(), ap.StartDate);
		assertEquals(explainAccrual.get(ExplainKey.UNADJUSTED_START_DATE).get(), ap.UnadjustedStartDate);
		assertEquals(explainAccrual.get(ExplainKey.END_DATE).get(), ap.EndDate);
		assertEquals(explainAccrual.get(ExplainKey.UNADJUSTED_END_DATE).get(), ap.UnadjustedEndDate);
		assertEquals(explainAccrual.get(ExplainKey.ACCRUAL_YEAR_FRACTION).Value, ap.YearFraction);
		assertEquals(explainAccrual.get(ExplainKey.ACCRUAL_DAYS).Value, (int?) daysBetween);
		assertEquals(explainAccrual.get(ExplainKey.GEARING).Value, ap.Gearing, TOLERANCE_PV);
		assertEquals(explainAccrual.get(ExplainKey.SPREAD).Value, ap.Spread, TOLERANCE_PV);
		assertEquals(explainAccrual.get(ExplainKey.FIXED_RATE).Value, RATE_1, TOLERANCE_PV);
		assertEquals(explainAccrual.get(ExplainKey.PAY_OFF_RATE).Value, RATE_1, TOLERANCE_PV);
		assertEquals(explainAccrual.get(ExplainKey.UNIT_AMOUNT).Value, ua, TOLERANCE_PV);
	  }

	  public virtual void test_explainPresentValue_single_gearingSpread()
	  {
		RatesProvider prov = createProvider(VAL_DATE);

		DiscountingRatePaymentPeriodPricer test = DiscountingRatePaymentPeriodPricer.DEFAULT;
		ExplainMapBuilder builder = ExplainMap.builder();
		test.explainPresentValue(PAYMENT_PERIOD_1_GS, prov, builder);
		ExplainMap explain = builder.build();

		Currency currency = PAYMENT_PERIOD_1_GS.Currency;
		double payOffRate = RATE_1 * GEARING + SPREAD;
		double ua = payOffRate * ACCRUAL_FACTOR_1;
		double fv = ua * NOTIONAL_100;
		assertEquals(explain.get(ExplainKey.ENTRY_TYPE).get(), "RatePaymentPeriod");
		assertEquals(explain.get(ExplainKey.PAYMENT_DATE).get(), PAYMENT_PERIOD_1_GS.PaymentDate);
		assertEquals(explain.get(ExplainKey.PAYMENT_CURRENCY).get(), currency);
		assertEquals(explain.get(ExplainKey.NOTIONAL).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.NOTIONAL).get().Amount, NOTIONAL_100, TOLERANCE_PV);
		assertEquals(explain.get(ExplainKey.TRADE_NOTIONAL).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.TRADE_NOTIONAL).get().Amount, NOTIONAL_100, TOLERANCE_PV);
		assertEquals(explain.get(ExplainKey.COMPOUNDING).get(), PAYMENT_PERIOD_1_GS.CompoundingMethod);
		assertEquals(explain.get(ExplainKey.DISCOUNT_FACTOR).Value, DISCOUNT_FACTOR, TOLERANCE_PV);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Amount, fv, TOLERANCE_PV);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Amount, fv * DISCOUNT_FACTOR, TOLERANCE_PV);

		assertEquals(explain.get(ExplainKey.ACCRUAL_PERIODS).get().size(), 1);
		ExplainMap explainAccrual = explain.get(ExplainKey.ACCRUAL_PERIODS).get().get(0);
		RateAccrualPeriod ap = PAYMENT_PERIOD_1_GS.AccrualPeriods.get(0);
		int daysBetween = (int) DAYS.between(ap.StartDate, ap.EndDate);
		assertEquals(explainAccrual.get(ExplainKey.ENTRY_TYPE).get(), "AccrualPeriod");
		assertEquals(explainAccrual.get(ExplainKey.START_DATE).get(), ap.StartDate);
		assertEquals(explainAccrual.get(ExplainKey.UNADJUSTED_START_DATE).get(), ap.UnadjustedStartDate);
		assertEquals(explainAccrual.get(ExplainKey.END_DATE).get(), ap.EndDate);
		assertEquals(explainAccrual.get(ExplainKey.UNADJUSTED_END_DATE).get(), ap.UnadjustedEndDate);
		assertEquals(explainAccrual.get(ExplainKey.ACCRUAL_YEAR_FRACTION).Value, ap.YearFraction);
		assertEquals(explainAccrual.get(ExplainKey.ACCRUAL_DAYS).Value, (int?) daysBetween);
		assertEquals(explainAccrual.get(ExplainKey.GEARING).Value, ap.Gearing, TOLERANCE_PV);
		assertEquals(explainAccrual.get(ExplainKey.SPREAD).Value, ap.Spread, TOLERANCE_PV);
		assertEquals(explainAccrual.get(ExplainKey.FIXED_RATE).Value, RATE_1, TOLERANCE_PV);
		assertEquals(explainAccrual.get(ExplainKey.PAY_OFF_RATE).Value, payOffRate, TOLERANCE_PV);
		assertEquals(explainAccrual.get(ExplainKey.UNIT_AMOUNT).Value, ua, TOLERANCE_PV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currencyExposure_fx()
	  {
		DiscountingRatePaymentPeriodPricer pricer = DiscountingRatePaymentPeriodPricer.DEFAULT;
		LocalDate valuationDate = VAL_DATE.minusWeeks(1);
		ImmutableRatesProvider provider = RatesProviderDataSets.multiGbpUsd(valuationDate);
		// USD
		MultiCurrencyAmount computedUSD = pricer.currencyExposure(PAYMENT_PERIOD_FULL_GS_FX_USD, provider);
		PointSensitivities pointUSD = pricer.presentValueSensitivity(PAYMENT_PERIOD_FULL_GS_FX_USD, provider).build();
		MultiCurrencyAmount expectedUSD = provider.currencyExposure(pointUSD.convertedTo(GBP, provider)).plus(CurrencyAmount.of(PAYMENT_PERIOD_FULL_GS_FX_USD.Currency, pricer.presentValue(PAYMENT_PERIOD_FULL_GS_FX_USD, provider)));
		assertEquals(computedUSD.getAmount(GBP).Amount, expectedUSD.getAmount(GBP).Amount, TOLERANCE_PV);
		assertFalse(computedUSD.contains(USD)); // 0 USD
		// GBP
		MultiCurrencyAmount computedGBP = pricer.currencyExposure(PAYMENT_PERIOD_FULL_GS_FX_GBP, provider);
		PointSensitivities pointGBP = pricer.presentValueSensitivity(PAYMENT_PERIOD_FULL_GS_FX_GBP, provider).build();
		MultiCurrencyAmount expectedGBP = provider.currencyExposure(pointGBP.convertedTo(USD, provider)).plus(CurrencyAmount.of(PAYMENT_PERIOD_FULL_GS_FX_GBP.Currency, pricer.presentValue(PAYMENT_PERIOD_FULL_GS_FX_GBP, provider)));
		assertEquals(computedGBP.getAmount(USD).Amount, expectedGBP.getAmount(USD).Amount, TOLERANCE_PV);
		assertFalse(computedGBP.contains(GBP)); // 0 GBP
		// FD approximation
		ImmutableRatesProvider provUp = RatesProviderDataSets.multiGbpUsd(valuationDate).toBuilder().fxRateProvider(FX_MATRIX_BUMP).build();
		double expectedFdUSD = (pricer.presentValue(PAYMENT_PERIOD_FULL_GS_FX_USD, provUp) - pricer.presentValue(PAYMENT_PERIOD_FULL_GS_FX_USD, provider)) / EPS_FD;
		assertEquals(computedUSD.getAmount(GBP).Amount, expectedFdUSD, EPS_FD * NOTIONAL_100);
		double expectedFdGBP = -(pricer.presentValue(PAYMENT_PERIOD_FULL_GS_FX_GBP, provUp) - pricer.presentValue(PAYMENT_PERIOD_FULL_GS_FX_GBP, provider)) * FX_RATE * FX_RATE / EPS_FD;
		assertEquals(computedGBP.getAmount(USD).Amount, expectedFdGBP, EPS_FD * NOTIONAL_100);
	  }

	  public virtual void test_currencyExposure_fx_betweenFixingAndPayment()
	  {
		DiscountingRatePaymentPeriodPricer pricer = DiscountingRatePaymentPeriodPricer.DEFAULT;
		LocalDate valuationDate = VAL_DATE.plusWeeks(1);
		LocalDateDoubleTimeSeries ts = LocalDateDoubleTimeSeries.of(LocalDate.of(2014, 1, 22), 1.55);
		ImmutableRatesProvider provider = RatesProviderDataSets.multiGbpUsd(valuationDate).toBuilder().timeSeries(FxIndices.GBP_USD_WM, ts).build();
		// USD
		MultiCurrencyAmount computedUSD = pricer.currencyExposure(PAYMENT_PERIOD_FULL_GS_FX_USD, provider);
		PointSensitivities pointUSD = pricer.presentValueSensitivity(PAYMENT_PERIOD_FULL_GS_FX_USD, provider).build();
		MultiCurrencyAmount expectedUSD = provider.currencyExposure(pointUSD.convertedTo(GBP, provider)).plus(CurrencyAmount.of(PAYMENT_PERIOD_FULL_GS_FX_USD.Currency, pricer.presentValue(PAYMENT_PERIOD_FULL_GS_FX_USD, provider)));
		assertEquals(computedUSD.getAmount(USD).Amount, expectedUSD.getAmount(USD).Amount, TOLERANCE_PV);
		assertFalse(computedUSD.contains(GBP)); // 0 GBP
		// GBP
		MultiCurrencyAmount computedGBP = pricer.currencyExposure(PAYMENT_PERIOD_FULL_GS_FX_GBP, provider);
		PointSensitivities pointGBP = pricer.presentValueSensitivity(PAYMENT_PERIOD_FULL_GS_FX_GBP, provider).build();
		MultiCurrencyAmount expectedGBP = provider.currencyExposure(pointGBP.convertedTo(USD, provider)).plus(CurrencyAmount.of(PAYMENT_PERIOD_FULL_GS_FX_GBP.Currency, pricer.presentValue(PAYMENT_PERIOD_FULL_GS_FX_GBP, provider)));
		assertEquals(computedGBP.getAmount(GBP).Amount, expectedGBP.getAmount(GBP).Amount, TOLERANCE_PV);
		assertFalse(computedGBP.contains(USD)); // 0 USD
		// FD approximation
		ImmutableRatesProvider provUp = RatesProviderDataSets.multiGbpUsd(valuationDate).toBuilder().fxRateProvider(FX_MATRIX_BUMP).timeSeries(FxIndices.GBP_USD_WM, ts).build();
		double expectedFdUSD = (pricer.presentValue(PAYMENT_PERIOD_FULL_GS_FX_USD, provUp) - pricer.presentValue(PAYMENT_PERIOD_FULL_GS_FX_USD, provider)) / EPS_FD;
		assertTrue(!computedUSD.contains(GBP) && DoubleMath.fuzzyEquals(expectedFdUSD, 0d, TOLERANCE_PV));
		double expectedFdGBP = -(pricer.presentValue(PAYMENT_PERIOD_FULL_GS_FX_GBP, provUp) - pricer.presentValue(PAYMENT_PERIOD_FULL_GS_FX_GBP, provider)) * FX_RATE * FX_RATE / EPS_FD;
		assertTrue(!computedGBP.contains(USD) && DoubleMath.fuzzyEquals(expectedFdGBP, 0d, TOLERANCE_PV));
	  }

	  public virtual void test_currencyExposure_fx_onFixing_noTimeSeries()
	  {
		DiscountingRatePaymentPeriodPricer pricer = DiscountingRatePaymentPeriodPricer.DEFAULT;
		ImmutableRatesProvider provider = MULTI_GBP_USD;
		// USD
		MultiCurrencyAmount computedUSD = pricer.currencyExposure(PAYMENT_PERIOD_FULL_GS_FX_USD, provider);
		PointSensitivities pointUSD = pricer.presentValueSensitivity(PAYMENT_PERIOD_FULL_GS_FX_USD, provider).build();
		MultiCurrencyAmount expectedUSD = provider.currencyExposure(pointUSD.convertedTo(GBP, provider)).plus(CurrencyAmount.of(PAYMENT_PERIOD_FULL_GS_FX_USD.Currency, pricer.presentValue(PAYMENT_PERIOD_FULL_GS_FX_USD, provider)));
		assertEquals(computedUSD.getAmount(GBP).Amount, expectedUSD.getAmount(GBP).Amount, TOLERANCE_PV);
		assertFalse(computedUSD.contains(USD)); // 0 GBP
		// GBP
		MultiCurrencyAmount computedGBP = pricer.currencyExposure(PAYMENT_PERIOD_FULL_GS_FX_GBP, provider);
		PointSensitivities pointGBP = pricer.presentValueSensitivity(PAYMENT_PERIOD_FULL_GS_FX_GBP, provider).build();
		MultiCurrencyAmount expectedGBP = provider.currencyExposure(pointGBP.convertedTo(USD, provider)).plus(CurrencyAmount.of(PAYMENT_PERIOD_FULL_GS_FX_GBP.Currency, pricer.presentValue(PAYMENT_PERIOD_FULL_GS_FX_GBP, provider)));
		assertEquals(computedGBP.getAmount(USD).Amount, expectedGBP.getAmount(USD).Amount, TOLERANCE_PV);
		assertFalse(computedGBP.contains(GBP)); // 0 GBP
		// FD approximation
		ImmutableRatesProvider provUp = RatesProviderDataSets.multiGbpUsd(VAL_DATE).toBuilder().fxRateProvider(FX_MATRIX_BUMP).build();
		double expectedFdUSD = (pricer.presentValue(PAYMENT_PERIOD_FULL_GS_FX_USD, provUp) - pricer.presentValue(PAYMENT_PERIOD_FULL_GS_FX_USD, provider)) / EPS_FD;
		assertEquals(computedUSD.getAmount(GBP).Amount, expectedFdUSD, EPS_FD * NOTIONAL_100);
		double expectedFdGBP = -(pricer.presentValue(PAYMENT_PERIOD_FULL_GS_FX_GBP, provUp) - pricer.presentValue(PAYMENT_PERIOD_FULL_GS_FX_GBP, provider)) * FX_RATE * FX_RATE / EPS_FD;
		assertEquals(computedGBP.getAmount(USD).Amount, expectedFdGBP, EPS_FD * NOTIONAL_100);
	  }

	  public virtual void test_currencyExposure_single()
	  {
		DiscountingRatePaymentPeriodPricer pricer = DiscountingRatePaymentPeriodPricer.DEFAULT;
		ImmutableRatesProvider provider = MULTI_GBP_USD;
		MultiCurrencyAmount computed = pricer.currencyExposure(PAYMENT_PERIOD_COMPOUNDING_STRAIGHT, provider);
		PointSensitivities point = pricer.presentValueSensitivity(PAYMENT_PERIOD_COMPOUNDING_STRAIGHT, provider).build();
		MultiCurrencyAmount expected = provider.currencyExposure(point).plus(CurrencyAmount.of(PAYMENT_PERIOD_COMPOUNDING_STRAIGHT.Currency, pricer.presentValue(PAYMENT_PERIOD_COMPOUNDING_STRAIGHT, provider)));
		assertEquals(computed.size(), expected.size());
		assertEquals(computed.getAmount(USD).Amount, expected.getAmount(USD).Amount, TOLERANCE_PV);
	  }

	  public virtual void test_currentCash_zero()
	  {
		DiscountingRatePaymentPeriodPricer pricer = DiscountingRatePaymentPeriodPricer.DEFAULT;
		ImmutableRatesProvider provider = MULTI_GBP_USD;
		double computed = pricer.currentCash(PAYMENT_PERIOD_COMPOUNDING_FLAT, provider);
		assertEquals(computed, 0d);
	  }

	  public virtual void test_currentCash_onPayment()
	  {
		DiscountingRatePaymentPeriodPricer pricer = DiscountingRatePaymentPeriodPricer.DEFAULT;
		RatesProvider provider = createProvider(PAYMENT_PERIOD_1.PaymentDate);
		double computed = pricer.currentCash(PAYMENT_PERIOD_1, provider);
		assertEquals(computed, RATE_1 * ACCRUAL_FACTOR_1 * NOTIONAL_100);
	  }

	  //-------------------------------------------------------------------------
	  // creates a simple provider
	  private SimpleRatesProvider createProvider(LocalDate valDate)
	  {
		DiscountFactors mockDf = mock(typeof(DiscountFactors));
		when(mockDf.discountFactor(PAYMENT_DATE_1)).thenReturn(DISCOUNT_FACTOR);
		FxIndexRates mockFxRates = mock(typeof(FxIndexRates));
		when(mockFxRates.rate(FxIndexObservation.of(GBP_USD_WM, FX_DATE_1, REF_DATA), GBP)).thenReturn(RATE_FX);
		SimpleRatesProvider prov = new SimpleRatesProvider(valDate);
		prov.DayCount = DAY_COUNT;
		prov.DiscountFactors = mockDf;
		prov.FxIndexRates = mockFxRates;
		return prov;
	  }

	}

}