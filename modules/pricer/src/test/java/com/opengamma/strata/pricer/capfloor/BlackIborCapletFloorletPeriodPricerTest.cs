/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.dateUtc;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;
	using DiscountingRatePaymentPeriodPricer = com.opengamma.strata.pricer.impl.swap.DiscountingRatePaymentPeriodPricer;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using IborCapletFloorletPeriod = com.opengamma.strata.product.capfloor.IborCapletFloorletPeriod;
	using FixedRateComputation = com.opengamma.strata.product.rate.FixedRateComputation;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;
	using RateAccrualPeriod = com.opengamma.strata.product.swap.RateAccrualPeriod;
	using RatePaymentPeriod = com.opengamma.strata.product.swap.RatePaymentPeriod;

	/// <summary>
	/// Test <seealso cref="BlackIborCapletFloorletPeriodPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BlackIborCapletFloorletPeriodPricerTest
	public class BlackIborCapletFloorletPeriodPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly ZonedDateTime VALUATION = dateUtc(2008, 8, 18);
	  private static readonly LocalDate FIXING = LocalDate.of(2011, 1, 3);
	  private const double NOTIONAL = 1000000; //1m
	  private const double STRIKE = 0.01;
	  private static readonly IborRateComputation RATE_COMP = IborRateComputation.of(EUR_EURIBOR_3M, FIXING, REF_DATA);
	  private static readonly IborCapletFloorletPeriod CAPLET_LONG = IborCapletFloorletPeriod.builder().caplet(STRIKE).startDate(RATE_COMP.EffectiveDate).endDate(RATE_COMP.MaturityDate).yearFraction(RATE_COMP.YearFraction).notional(NOTIONAL).iborRate(RATE_COMP).build();
	  private static readonly IborCapletFloorletPeriod CAPLET_SHORT = IborCapletFloorletPeriod.builder().caplet(STRIKE).startDate(RATE_COMP.EffectiveDate).endDate(RATE_COMP.MaturityDate).yearFraction(RATE_COMP.YearFraction).notional(-NOTIONAL).iborRate(RATE_COMP).build();
	  private static readonly IborCapletFloorletPeriod FLOORLET_LONG = IborCapletFloorletPeriod.builder().floorlet(STRIKE).startDate(RATE_COMP.EffectiveDate).endDate(RATE_COMP.MaturityDate).yearFraction(RATE_COMP.YearFraction).notional(NOTIONAL).iborRate(RATE_COMP).build();
	  private static readonly IborCapletFloorletPeriod FLOORLET_SHORT = IborCapletFloorletPeriod.builder().floorlet(STRIKE).startDate(RATE_COMP.EffectiveDate).endDate(RATE_COMP.MaturityDate).yearFraction(RATE_COMP.YearFraction).notional(-NOTIONAL).iborRate(RATE_COMP).build();
	  private static readonly RateAccrualPeriod IBOR_PERIOD = RateAccrualPeriod.builder().startDate(CAPLET_LONG.StartDate).endDate(CAPLET_LONG.EndDate).yearFraction(CAPLET_LONG.YearFraction).rateComputation(RATE_COMP).build();
	  private static readonly RatePaymentPeriod IBOR_COUPON = RatePaymentPeriod.builder().accrualPeriods(IBOR_PERIOD).paymentDate(CAPLET_LONG.PaymentDate).dayCount(EUR_EURIBOR_3M.DayCount).notional(NOTIONAL).currency(EUR).build();
	  private static readonly RateAccrualPeriod FIXED_PERIOD = RateAccrualPeriod.builder().startDate(CAPLET_LONG.StartDate).endDate(CAPLET_LONG.EndDate).rateComputation(FixedRateComputation.of(STRIKE)).yearFraction(CAPLET_LONG.YearFraction).build();
	  private static readonly RatePaymentPeriod FIXED_COUPON = RatePaymentPeriod.builder().accrualPeriods(FIXED_PERIOD).paymentDate(CAPLET_LONG.PaymentDate).dayCount(EUR_EURIBOR_3M.DayCount).notional(NOTIONAL).currency(EUR).build();
	  private static readonly RateAccrualPeriod FIXED_PERIOD_UNIT = RateAccrualPeriod.builder().startDate(CAPLET_LONG.StartDate).endDate(CAPLET_LONG.EndDate).rateComputation(FixedRateComputation.of(1d)).yearFraction(CAPLET_LONG.YearFraction).build();
	  private static readonly RatePaymentPeriod FIXED_COUPON_UNIT = RatePaymentPeriod.builder().accrualPeriods(FIXED_PERIOD_UNIT).paymentDate(CAPLET_LONG.PaymentDate).dayCount(EUR_EURIBOR_3M.DayCount).notional(NOTIONAL).currency(EUR).build();
	  // valuation date before fixing date
	  private static readonly ImmutableRatesProvider RATES = IborCapletFloorletDataSet.createRatesProvider(VALUATION.toLocalDate());
	  private static readonly BlackIborCapletFloorletExpiryStrikeVolatilities VOLS = IborCapletFloorletDataSet.createBlackVolatilities(VALUATION, EUR_EURIBOR_3M);
	  // valuation date equal to fixing date
	  private const double OBS_INDEX = 0.013;
	  private static readonly LocalDateDoubleTimeSeries TIME_SERIES = LocalDateDoubleTimeSeries.of(FIXING, OBS_INDEX);
	  private static readonly ImmutableRatesProvider RATES_ON_FIX = IborCapletFloorletDataSet.createRatesProvider(FIXING, EUR_EURIBOR_3M, TIME_SERIES);
	  private static readonly BlackIborCapletFloorletExpiryStrikeVolatilities VOLS_ON_FIX = IborCapletFloorletDataSet.createBlackVolatilities(FIXING.atStartOfDay(ZoneOffset.UTC), EUR_EURIBOR_3M);
	  // valuation date after fixing date
	  private static readonly ImmutableRatesProvider RATES_AFTER_FIX = IborCapletFloorletDataSet.createRatesProvider(FIXING.plusWeeks(1), EUR_EURIBOR_3M, TIME_SERIES);
	  private static readonly BlackIborCapletFloorletExpiryStrikeVolatilities VOLS_AFTER_FIX = IborCapletFloorletDataSet.createBlackVolatilities(FIXING.plusWeeks(1).atStartOfDay(ZoneOffset.UTC), EUR_EURIBOR_3M);
	  // valuation date after payment date
	  private static readonly LocalDate DATE_AFTER_PAY = LocalDate.of(2011, 5, 2);
	  private static readonly ImmutableRatesProvider RATES_AFTER_PAY = IborCapletFloorletDataSet.createRatesProvider(DATE_AFTER_PAY, EUR_EURIBOR_3M, TIME_SERIES);
	  private static readonly BlackIborCapletFloorletExpiryStrikeVolatilities VOLS_AFTER_PAY = IborCapletFloorletDataSet.createBlackVolatilities(DATE_AFTER_PAY.plusWeeks(1).atStartOfDay(ZoneOffset.UTC), EUR_EURIBOR_3M);
	  // normal vols
	  private static readonly NormalIborCapletFloorletExpiryStrikeVolatilities VOLS_NORMAL = IborCapletFloorletDataSet.createNormalVolatilities(VALUATION, EUR_EURIBOR_3M);
	  // shifted Black vols
	  private static readonly ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities SHIFTED_VOLS = IborCapletFloorletDataSet.createShiftedBlackVolatilities(VALUATION, EUR_EURIBOR_3M);

	  private const double TOL = 1.0e-14;
	  private const double EPS_FD = 1.0e-6;
	  private static readonly BlackIborCapletFloorletPeriodPricer PRICER = BlackIborCapletFloorletPeriodPricer.DEFAULT;
	  private static readonly VolatilityIborCapletFloorletPeriodPricer PRICER_BASE = VolatilityIborCapletFloorletPeriodPricer.DEFAULT;
	  private static readonly DiscountingRatePaymentPeriodPricer PRICER_COUPON = DiscountingRatePaymentPeriodPricer.DEFAULT;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FD_CAL = new RatesFiniteDifferenceSensitivityCalculator(EPS_FD);

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue_formula()
	  {
		CurrencyAmount computedCaplet = PRICER.presentValue(CAPLET_LONG, RATES, VOLS);
		CurrencyAmount computedFloorlet = PRICER.presentValue(FLOORLET_SHORT, RATES, VOLS);
		double forward = RATES.iborIndexRates(EUR_EURIBOR_3M).rate(RATE_COMP.Observation);
		double expiry = VOLS.relativeTime(CAPLET_LONG.FixingDateTime);
		double volatility = VOLS.volatility(expiry, STRIKE, forward);
		double df = RATES.discountFactor(EUR, CAPLET_LONG.PaymentDate);
		double expectedCaplet = NOTIONAL * df * CAPLET_LONG.YearFraction * BlackFormulaRepository.price(forward, STRIKE, expiry, volatility, true);
		double expectedFloorlet = -NOTIONAL * df * FLOORLET_SHORT.YearFraction * BlackFormulaRepository.price(forward, STRIKE, expiry, volatility, false);
		assertEquals(computedCaplet.Currency, EUR);
		assertEquals(computedCaplet.Amount, expectedCaplet, NOTIONAL * TOL);
		assertEquals(computedFloorlet.Currency, EUR);
		assertEquals(computedFloorlet.Amount, expectedFloorlet, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_parity()
	  {
		double capletLong = PRICER.presentValue(CAPLET_LONG, RATES, VOLS).Amount;
		double capletShort = PRICER.presentValue(CAPLET_SHORT, RATES, VOLS).Amount;
		double floorletLong = PRICER.presentValue(FLOORLET_LONG, RATES, VOLS).Amount;
		double floorletShort = PRICER.presentValue(FLOORLET_SHORT, RATES, VOLS).Amount;
		double iborCoupon = PRICER_COUPON.presentValue(IBOR_COUPON, RATES);
		double fixedCoupon = PRICER_COUPON.presentValue(FIXED_COUPON, RATES);
		assertEquals(capletLong, -capletShort, NOTIONAL * TOL);
		assertEquals(floorletLong, -floorletShort, NOTIONAL * TOL);
		assertEquals(capletLong - floorletLong, iborCoupon - fixedCoupon, NOTIONAL * TOL);
		assertEquals(capletShort - floorletShort, -iborCoupon + fixedCoupon, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_onFix()
	  {
		CurrencyAmount computedCaplet = PRICER.presentValue(CAPLET_LONG, RATES_ON_FIX, VOLS_ON_FIX);
		CurrencyAmount computedFloorlet = PRICER.presentValue(FLOORLET_SHORT, RATES_ON_FIX, VOLS_ON_FIX);
		double expectedCaplet = PRICER_COUPON.presentValue(FIXED_COUPON_UNIT, RATES_ON_FIX) * (OBS_INDEX - STRIKE);
		double expectedFloorlet = 0d;
		assertEquals(computedCaplet.Currency, EUR);
		assertEquals(computedCaplet.Amount, expectedCaplet, NOTIONAL * TOL);
		assertEquals(computedFloorlet.Currency, EUR);
		assertEquals(computedFloorlet.Amount, expectedFloorlet, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_afterFix()
	  {
		CurrencyAmount computedCaplet = PRICER.presentValue(CAPLET_LONG, RATES_AFTER_FIX, VOLS_AFTER_FIX);
		CurrencyAmount computedFloorlet = PRICER.presentValue(FLOORLET_SHORT, RATES_AFTER_FIX, VOLS_AFTER_FIX);
		double payoff = (OBS_INDEX - STRIKE) * PRICER_COUPON.presentValue(FIXED_COUPON_UNIT, RATES_AFTER_FIX);
		assertEquals(computedCaplet.Currency, EUR);
		assertEquals(computedCaplet.Amount, payoff, NOTIONAL * TOL);
		assertEquals(computedFloorlet.Currency, EUR);
		assertEquals(computedFloorlet.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_afterPay()
	  {
		CurrencyAmount computedCaplet = PRICER.presentValue(CAPLET_LONG, RATES_AFTER_PAY, VOLS_AFTER_PAY);
		CurrencyAmount computedFloorlet = PRICER.presentValue(FLOORLET_SHORT, RATES_AFTER_PAY, VOLS_AFTER_PAY);
		assertEquals(computedCaplet.Currency, EUR);
		assertEquals(computedCaplet.Amount, 0d, NOTIONAL * TOL);
		assertEquals(computedFloorlet.Currency, EUR);
		assertEquals(computedFloorlet.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_formula_shift()
	  {
		CurrencyAmount computedCaplet = PRICER.presentValue(CAPLET_LONG, RATES, SHIFTED_VOLS);
		CurrencyAmount computedFloorlet = PRICER.presentValue(FLOORLET_SHORT, RATES, SHIFTED_VOLS);
		double forward = RATES.iborIndexRates(EUR_EURIBOR_3M).rate(RATE_COMP.Observation);
		double expiry = SHIFTED_VOLS.relativeTime(CAPLET_LONG.FixingDateTime);
		double volatility = SHIFTED_VOLS.volatility(expiry, STRIKE, forward);
		double df = RATES.discountFactor(EUR, CAPLET_LONG.PaymentDate);
		double shift = IborCapletFloorletDataSet.SHIFT;
		double expectedCaplet = NOTIONAL * df * CAPLET_LONG.YearFraction * BlackFormulaRepository.price(forward + shift, STRIKE + shift, expiry, volatility, true);
		double expectedFloorlet = -NOTIONAL * df * FLOORLET_SHORT.YearFraction * BlackFormulaRepository.price(forward + shift, STRIKE + shift, expiry, volatility, false);
		assertEquals(computedCaplet.Currency, EUR);
		assertEquals(computedCaplet.Amount, expectedCaplet, NOTIONAL * TOL);
		assertEquals(computedFloorlet.Currency, EUR);
		assertEquals(computedFloorlet.Amount, expectedFloorlet, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_impliedVolatility()
	  {
		double computed = PRICER.impliedVolatility(CAPLET_LONG, RATES, VOLS);
		double expiry = VOLS.relativeTime(CAPLET_LONG.FixingDateTime);
		double expected = VOLS.Surface.zValue(expiry, STRIKE);
		assertEquals(computed, expected, TOL);
	  }

	  public virtual void test_impliedVolatility_onFix()
	  {
		double computed = PRICER.impliedVolatility(CAPLET_LONG, RATES_ON_FIX, VOLS_ON_FIX);
		double expected = VOLS_ON_FIX.Surface.zValue(0d, STRIKE);
		assertEquals(computed, expected, TOL);
	  }

	  public virtual void test_impliedVolatility_afterFix()
	  {
		assertThrowsIllegalArg(() => PRICER.impliedVolatility(CAPLET_LONG, RATES_AFTER_FIX, VOLS_AFTER_FIX));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueDelta_formula()
	  {
		CurrencyAmount computedCaplet = PRICER.presentValueDelta(CAPLET_LONG, RATES, VOLS);
		CurrencyAmount computedFloorlet = PRICER.presentValueDelta(FLOORLET_SHORT, RATES, VOLS);
		double forward = RATES.iborIndexRates(EUR_EURIBOR_3M).rate(RATE_COMP.Observation);
		double expiry = VOLS.relativeTime(CAPLET_LONG.FixingDateTime);
		double volatility = VOLS.volatility(expiry, STRIKE, forward);
		double df = RATES.discountFactor(EUR, CAPLET_LONG.PaymentDate);
		double expectedCaplet = NOTIONAL * df * CAPLET_LONG.YearFraction * BlackFormulaRepository.delta(forward, STRIKE, expiry, volatility, true);
		double expectedFloorlet = -NOTIONAL * df * FLOORLET_SHORT.YearFraction * BlackFormulaRepository.delta(forward, STRIKE, expiry, volatility, false);
		assertEquals(computedCaplet.Currency, EUR);
		assertEquals(computedCaplet.Amount, expectedCaplet, NOTIONAL * TOL);
		assertEquals(computedFloorlet.Currency, EUR);
		assertEquals(computedFloorlet.Amount, expectedFloorlet, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueDelta_parity()
	  {
		double capletLong = PRICER.presentValueDelta(CAPLET_LONG, RATES, VOLS).Amount;
		double capletShort = PRICER.presentValueDelta(CAPLET_SHORT, RATES, VOLS).Amount;
		double floorletLong = PRICER.presentValueDelta(FLOORLET_LONG, RATES, VOLS).Amount;
		double floorletShort = PRICER.presentValueDelta(FLOORLET_SHORT, RATES, VOLS).Amount;
		double unitCoupon = PRICER_COUPON.presentValue(FIXED_COUPON_UNIT, RATES);
		assertEquals(capletLong, -capletShort, NOTIONAL * TOL);
		assertEquals(floorletLong, -floorletShort, NOTIONAL * TOL);
		assertEquals(capletLong - floorletLong, unitCoupon, NOTIONAL * TOL);
		assertEquals(capletShort - floorletShort, -unitCoupon, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueDelta_onFix()
	  {
		CurrencyAmount computedCaplet = PRICER.presentValueDelta(CAPLET_LONG, RATES_ON_FIX, VOLS_ON_FIX);
		CurrencyAmount computedFloorlet = PRICER.presentValueDelta(FLOORLET_SHORT, RATES_ON_FIX, VOLS_ON_FIX);
		double expectedCaplet = PRICER_COUPON.presentValue(FIXED_COUPON_UNIT, RATES_ON_FIX);
		double expectedFloorlet = 0d;
		assertEquals(computedCaplet.Currency, EUR);
		assertEquals(computedCaplet.Amount, expectedCaplet, TOL);
		assertEquals(computedFloorlet.Currency, EUR);
		assertEquals(computedFloorlet.Amount, expectedFloorlet, TOL);
	  }

	  public virtual void test_presentValueDelta_afterFix()
	  {
		CurrencyAmount computedCaplet = PRICER.presentValueDelta(CAPLET_LONG, RATES_AFTER_FIX, VOLS_AFTER_FIX);
		CurrencyAmount computedFloorlet = PRICER.presentValueDelta(FLOORLET_SHORT, RATES_AFTER_FIX, VOLS_AFTER_FIX);
		assertEquals(computedCaplet.Currency, EUR);
		assertEquals(computedCaplet.Amount, 0d, TOL);
		assertEquals(computedFloorlet.Currency, EUR);
		assertEquals(computedFloorlet.Amount, 0d, TOL);
	  }

	  public virtual void test_presentValueDelta_formula_shift()
	  {
		CurrencyAmount computedCaplet = PRICER.presentValueDelta(CAPLET_LONG, RATES, SHIFTED_VOLS);
		CurrencyAmount computedFloorlet = PRICER.presentValueDelta(FLOORLET_SHORT, RATES, SHIFTED_VOLS);
		double forward = RATES.iborIndexRates(EUR_EURIBOR_3M).rate(RATE_COMP.Observation);
		double expiry = SHIFTED_VOLS.relativeTime(CAPLET_LONG.FixingDateTime);
		double volatility = SHIFTED_VOLS.volatility(expiry, STRIKE, forward);
		double df = RATES.discountFactor(EUR, CAPLET_LONG.PaymentDate);
		double shift = IborCapletFloorletDataSet.SHIFT;
		double expectedCaplet = NOTIONAL * df * CAPLET_LONG.YearFraction * BlackFormulaRepository.delta(forward + shift, STRIKE + shift, expiry, volatility, true);
		double expectedFloorlet = -NOTIONAL * df * FLOORLET_SHORT.YearFraction * BlackFormulaRepository.delta(forward + shift, STRIKE + shift, expiry, volatility, false);
		assertEquals(computedCaplet.Currency, EUR);
		assertEquals(computedCaplet.Amount, expectedCaplet, NOTIONAL * TOL);
		assertEquals(computedFloorlet.Currency, EUR);
		assertEquals(computedFloorlet.Amount, expectedFloorlet, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueGamma_formula()
	  {
		CurrencyAmount computedCaplet = PRICER.presentValueGamma(CAPLET_LONG, RATES, VOLS);
		CurrencyAmount computedFloorlet = PRICER.presentValueGamma(FLOORLET_SHORT, RATES, VOLS);
		double forward = RATES.iborIndexRates(EUR_EURIBOR_3M).rate(RATE_COMP.Observation);
		double expiry = VOLS.relativeTime(CAPLET_LONG.FixingDateTime);
		double volatility = VOLS.volatility(expiry, STRIKE, forward);
		double df = RATES.discountFactor(EUR, CAPLET_LONG.PaymentDate);
		double expectedCaplet = NOTIONAL * df * CAPLET_LONG.YearFraction * BlackFormulaRepository.gamma(forward, STRIKE, expiry, volatility);
		double expectedFloorlet = -NOTIONAL * df * FLOORLET_SHORT.YearFraction * BlackFormulaRepository.gamma(forward, STRIKE, expiry, volatility);
		assertEquals(computedCaplet.Currency, EUR);
		assertEquals(computedCaplet.Amount, expectedCaplet, NOTIONAL * TOL);
		assertEquals(computedFloorlet.Currency, EUR);
		assertEquals(computedFloorlet.Amount, expectedFloorlet, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueGamma_onFix()
	  {
		CurrencyAmount computedCaplet = PRICER.presentValueGamma(CAPLET_LONG, RATES_ON_FIX, VOLS_ON_FIX);
		CurrencyAmount computedFloorlet = PRICER.presentValueGamma(FLOORLET_SHORT, RATES_ON_FIX, VOLS_ON_FIX);
		double expectedCaplet = 0d;
		double expectedFloorlet = 0d;
		assertEquals(computedCaplet.Currency, EUR);
		assertEquals(computedCaplet.Amount, expectedCaplet, TOL);
		assertEquals(computedFloorlet.Currency, EUR);
		assertEquals(computedFloorlet.Amount, expectedFloorlet, TOL);
	  }

	  public virtual void test_presentValueGamma_afterFix()
	  {
		CurrencyAmount computedCaplet = PRICER.presentValueGamma(CAPLET_LONG, RATES_AFTER_FIX, VOLS_AFTER_FIX);
		CurrencyAmount computedFloorlet = PRICER.presentValueGamma(FLOORLET_SHORT, RATES_AFTER_FIX, VOLS_AFTER_FIX);
		assertEquals(computedCaplet.Currency, EUR);
		assertEquals(computedCaplet.Amount, 0d, TOL);
		assertEquals(computedFloorlet.Currency, EUR);
		assertEquals(computedFloorlet.Amount, 0d, TOL);
	  }

	  public virtual void test_presentValueGamma_formula_shift()
	  {
		CurrencyAmount computedCaplet = PRICER.presentValueGamma(CAPLET_LONG, RATES, SHIFTED_VOLS);
		CurrencyAmount computedFloorlet = PRICER.presentValueGamma(FLOORLET_SHORT, RATES, SHIFTED_VOLS);
		double forward = RATES.iborIndexRates(EUR_EURIBOR_3M).rate(RATE_COMP.Observation);
		double expiry = SHIFTED_VOLS.relativeTime(CAPLET_LONG.FixingDateTime);
		double volatility = SHIFTED_VOLS.volatility(expiry, STRIKE, forward);
		double df = RATES.discountFactor(EUR, CAPLET_LONG.PaymentDate);
		double shift = IborCapletFloorletDataSet.SHIFT;
		double expectedCaplet = NOTIONAL * df * CAPLET_LONG.YearFraction * BlackFormulaRepository.gamma(forward + shift, STRIKE + shift, expiry, volatility);
		double expectedFloorlet = -NOTIONAL * df * FLOORLET_SHORT.YearFraction * BlackFormulaRepository.gamma(forward + shift, STRIKE + shift, expiry, volatility);
		assertEquals(computedCaplet.Currency, EUR);
		assertEquals(computedCaplet.Amount, expectedCaplet, NOTIONAL * TOL);
		assertEquals(computedFloorlet.Currency, EUR);
		assertEquals(computedFloorlet.Amount, expectedFloorlet, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueTheta_formula()
	  {
		CurrencyAmount computedCaplet = PRICER.presentValueTheta(CAPLET_LONG, RATES, VOLS);
		CurrencyAmount computedFloorlet = PRICER.presentValueTheta(FLOORLET_SHORT, RATES, VOLS);
		double forward = RATES.iborIndexRates(EUR_EURIBOR_3M).rate(RATE_COMP.Observation);
		double expiry = VOLS.relativeTime(CAPLET_LONG.FixingDateTime);
		double volatility = VOLS.volatility(expiry, STRIKE, forward);
		double df = RATES.discountFactor(EUR, CAPLET_LONG.PaymentDate);
		double expectedCaplet = NOTIONAL * df * CAPLET_LONG.YearFraction * BlackFormulaRepository.driftlessTheta(forward, STRIKE, expiry, volatility);
		double expectedFloorlet = -NOTIONAL * df * FLOORLET_SHORT.YearFraction * BlackFormulaRepository.driftlessTheta(forward, STRIKE, expiry, volatility);
		assertEquals(computedCaplet.Currency, EUR);
		assertEquals(computedCaplet.Amount, expectedCaplet, NOTIONAL * TOL);
		assertEquals(computedFloorlet.Currency, EUR);
		assertEquals(computedFloorlet.Amount, expectedFloorlet, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueTheta_parity()
	  {
		double capletLong = PRICER.presentValueTheta(CAPLET_LONG, RATES, VOLS).Amount;
		double capletShort = PRICER.presentValueTheta(CAPLET_SHORT, RATES, VOLS).Amount;
		double floorletLong = PRICER.presentValueTheta(FLOORLET_LONG, RATES, VOLS).Amount;
		double floorletShort = PRICER.presentValueTheta(FLOORLET_SHORT, RATES, VOLS).Amount;
		assertEquals(capletLong, -capletShort, NOTIONAL * TOL);
		assertEquals(floorletLong, -floorletShort, NOTIONAL * TOL);
		assertEquals(capletLong, floorletLong, NOTIONAL * TOL);
		assertEquals(capletShort, floorletShort, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueTheta_onFix()
	  {
		CurrencyAmount computedCaplet = PRICER.presentValueTheta(CAPLET_LONG, RATES_ON_FIX, VOLS_ON_FIX);
		CurrencyAmount computedFloorlet = PRICER.presentValueTheta(FLOORLET_SHORT, RATES_ON_FIX, VOLS_ON_FIX);
		double expectedCaplet = 0d;
		double expectedFloorlet = 0d;
		assertEquals(computedCaplet.Currency, EUR);
		assertEquals(computedCaplet.Amount, expectedCaplet, TOL);
		assertEquals(computedFloorlet.Currency, EUR);
		assertEquals(computedFloorlet.Amount, expectedFloorlet, TOL);
	  }

	  public virtual void test_presentValueTheta_afterFix()
	  {
		CurrencyAmount computedCaplet = PRICER.presentValueTheta(CAPLET_LONG, RATES_AFTER_FIX, VOLS_AFTER_FIX);
		CurrencyAmount computedFloorlet = PRICER.presentValueTheta(FLOORLET_SHORT, RATES_AFTER_FIX, VOLS_AFTER_FIX);
		assertEquals(computedCaplet.Currency, EUR);
		assertEquals(computedCaplet.Amount, 0d, TOL);
		assertEquals(computedFloorlet.Currency, EUR);
		assertEquals(computedFloorlet.Amount, 0d, TOL);
	  }

	  public virtual void test_presentValueTheta_formula_shift()
	  {
		CurrencyAmount computedCaplet = PRICER.presentValueTheta(CAPLET_LONG, RATES, SHIFTED_VOLS);
		CurrencyAmount computedFloorlet = PRICER.presentValueTheta(FLOORLET_SHORT, RATES, SHIFTED_VOLS);
		double forward = RATES.iborIndexRates(EUR_EURIBOR_3M).rate(RATE_COMP.Observation);
		double expiry = SHIFTED_VOLS.relativeTime(CAPLET_LONG.FixingDateTime);
		double volatility = SHIFTED_VOLS.volatility(expiry, STRIKE, forward);
		double df = RATES.discountFactor(EUR, CAPLET_LONG.PaymentDate);
		double shift = IborCapletFloorletDataSet.SHIFT;
		double expectedCaplet = NOTIONAL * df * CAPLET_LONG.YearFraction * BlackFormulaRepository.driftlessTheta(forward + shift, STRIKE + shift, expiry, volatility);
		double expectedFloorlet = -NOTIONAL * df * FLOORLET_SHORT.YearFraction * BlackFormulaRepository.driftlessTheta(forward + shift, STRIKE + shift, expiry, volatility);
		assertEquals(computedCaplet.Currency, EUR);
		assertEquals(computedCaplet.Amount, expectedCaplet, NOTIONAL * TOL);
		assertEquals(computedFloorlet.Currency, EUR);
		assertEquals(computedFloorlet.Amount, expectedFloorlet, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity()
	  {
		PointSensitivityBuilder pointCaplet = PRICER.presentValueSensitivityRates(CAPLET_LONG, RATES, VOLS);
		CurrencyParameterSensitivities computedCaplet = RATES.parameterSensitivity(pointCaplet.build());
		PointSensitivityBuilder pointFloorlet = PRICER.presentValueSensitivityRates(FLOORLET_SHORT, RATES, VOLS);
		CurrencyParameterSensitivities computedFloorlet = RATES.parameterSensitivity(pointFloorlet.build());
		CurrencyParameterSensitivities expectedCaplet = FD_CAL.sensitivity(RATES, p => PRICER_BASE.presentValue(CAPLET_LONG, p, VOLS));
		CurrencyParameterSensitivities expectedFloorlet = FD_CAL.sensitivity(RATES, p => PRICER_BASE.presentValue(FLOORLET_SHORT, p, VOLS));
		assertTrue(computedCaplet.equalWithTolerance(expectedCaplet, EPS_FD * NOTIONAL * 50d));
		assertTrue(computedFloorlet.equalWithTolerance(expectedFloorlet, EPS_FD * NOTIONAL * 50d));
	  }

	  public virtual void test_presentValueSensitivity_onFix()
	  {
		PointSensitivityBuilder pointCaplet = PRICER.presentValueSensitivityRates(CAPLET_LONG, RATES_ON_FIX, VOLS_ON_FIX);
		CurrencyParameterSensitivities computedCaplet = RATES_ON_FIX.parameterSensitivity(pointCaplet.build());
		PointSensitivityBuilder pointFloorlet = PRICER.presentValueSensitivityRates(FLOORLET_SHORT, RATES_ON_FIX, VOLS_ON_FIX);
		CurrencyParameterSensitivities computedFloorlet = RATES_ON_FIX.parameterSensitivity(pointFloorlet.build());
		CurrencyParameterSensitivities expectedCaplet = FD_CAL.sensitivity(RATES_ON_FIX, p => PRICER_BASE.presentValue(CAPLET_LONG, p, VOLS_ON_FIX));
		CurrencyParameterSensitivities expectedFloorlet = FD_CAL.sensitivity(RATES_ON_FIX, p => PRICER_BASE.presentValue(FLOORLET_SHORT, p, VOLS_ON_FIX));
		assertTrue(computedCaplet.equalWithTolerance(expectedCaplet, EPS_FD * NOTIONAL));
		assertTrue(computedFloorlet.equalWithTolerance(expectedFloorlet, EPS_FD * NOTIONAL));
	  }

	  public virtual void test_presentValueSensitivity_afterFix()
	  {
		PointSensitivityBuilder pointCaplet = PRICER.presentValueSensitivityRates(CAPLET_LONG, RATES_AFTER_FIX, VOLS_AFTER_FIX);
		CurrencyParameterSensitivities computedCaplet = RATES_AFTER_FIX.parameterSensitivity(pointCaplet.build());
		PointSensitivityBuilder pointFloorlet = PRICER.presentValueSensitivityRates(FLOORLET_SHORT, RATES_AFTER_FIX, VOLS_AFTER_FIX);
		CurrencyParameterSensitivities computedFloorlet = RATES_AFTER_FIX.parameterSensitivity(pointFloorlet.build());
		CurrencyParameterSensitivities expectedCaplet = FD_CAL.sensitivity(RATES_AFTER_FIX, p => PRICER_BASE.presentValue(CAPLET_LONG, p, VOLS_AFTER_FIX));
		CurrencyParameterSensitivities expectedFloorlet = FD_CAL.sensitivity(RATES_AFTER_FIX, p => PRICER_BASE.presentValue(FLOORLET_SHORT, p, VOLS_AFTER_FIX));
		assertTrue(computedCaplet.equalWithTolerance(expectedCaplet, EPS_FD * NOTIONAL));
		assertTrue(computedFloorlet.equalWithTolerance(expectedFloorlet, EPS_FD * NOTIONAL));
	  }

	  public virtual void test_presentValueSensitivity_afterPay()
	  {
		PointSensitivityBuilder computedCaplet = PRICER.presentValueSensitivityRates(CAPLET_LONG, RATES_AFTER_PAY, VOLS_AFTER_PAY);
		PointSensitivityBuilder computedFloorlet = PRICER.presentValueSensitivityRates(FLOORLET_SHORT, RATES_AFTER_PAY, VOLS_AFTER_PAY);
		assertEquals(computedCaplet, PointSensitivityBuilder.none());
		assertEquals(computedFloorlet, PointSensitivityBuilder.none());
	  }

	  public virtual void test_presentValueSensitivity_shifted()
	  {
		PointSensitivityBuilder pointCaplet = PRICER.presentValueSensitivityRates(CAPLET_LONG, RATES, SHIFTED_VOLS);
		CurrencyParameterSensitivities computedCaplet = RATES.parameterSensitivity(pointCaplet.build());
		PointSensitivityBuilder pointFloorlet = PRICER.presentValueSensitivityRates(FLOORLET_SHORT, RATES, SHIFTED_VOLS);
		CurrencyParameterSensitivities computedFloorlet = RATES.parameterSensitivity(pointFloorlet.build());
		CurrencyParameterSensitivities expectedCaplet = FD_CAL.sensitivity(RATES, p => PRICER_BASE.presentValue(CAPLET_LONG, p, SHIFTED_VOLS));
		CurrencyParameterSensitivities expectedFloorlet = FD_CAL.sensitivity(RATES, p => PRICER_BASE.presentValue(FLOORLET_SHORT, p, SHIFTED_VOLS));
		assertTrue(computedCaplet.equalWithTolerance(expectedCaplet, EPS_FD * NOTIONAL * 50d));
		assertTrue(computedFloorlet.equalWithTolerance(expectedFloorlet, EPS_FD * NOTIONAL * 50d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivityVolatility()
	  {
		PointSensitivityBuilder pointCaplet = PRICER.presentValueSensitivityModelParamsVolatility(CAPLET_LONG, RATES, VOLS);
		CurrencyParameterSensitivity computedCaplet = VOLS.parameterSensitivity(pointCaplet.build()).Sensitivities.get(0);
		PointSensitivityBuilder pointFloorlet = PRICER.presentValueSensitivityModelParamsVolatility(FLOORLET_SHORT, RATES, VOLS);
		CurrencyParameterSensitivity computedFloorlet = VOLS.parameterSensitivity(pointFloorlet.build()).Sensitivities.get(0);
		testSurfaceSensitivity(computedCaplet, VOLS, v => PRICER.presentValue(CAPLET_LONG, RATES, v));
		testSurfaceSensitivity(computedFloorlet, VOLS, v => PRICER.presentValue(FLOORLET_SHORT, RATES, v));
	  }

	  private void testSurfaceSensitivity(CurrencyParameterSensitivity computed, BlackIborCapletFloorletExpiryStrikeVolatilities vols, System.Func<IborCapletFloorletVolatilities, CurrencyAmount> valueFn)
	  {
		double pvBase = valueFn(vols).Amount;
		InterpolatedNodalSurface surfaceBase = (InterpolatedNodalSurface) vols.Surface;
		int nParams = surfaceBase.ParameterCount;
		for (int i = 0; i < nParams; i++)
		{
		  DoubleArray zBumped = surfaceBase.ZValues.with(i, surfaceBase.ZValues.get(i) + EPS_FD);
		  InterpolatedNodalSurface surfaceBumped = surfaceBase.withZValues(zBumped);
		  BlackIborCapletFloorletExpiryStrikeVolatilities volsBumped = BlackIborCapletFloorletExpiryStrikeVolatilities.of(vols.Index, vols.ValuationDateTime, surfaceBumped);
		  double fd = (valueFn(volsBumped).Amount - pvBase) / EPS_FD;
		  assertEquals(computed.Sensitivity.get(i), fd, NOTIONAL * EPS_FD);
		}
	  }

	  public virtual void test_presentValueSensitivityVolatility_onFix()
	  {
		PointSensitivityBuilder computedCaplet = PRICER.presentValueSensitivityModelParamsVolatility(CAPLET_LONG, RATES_ON_FIX, VOLS_ON_FIX);
		PointSensitivityBuilder computedFloorlet = PRICER.presentValueSensitivityModelParamsVolatility(FLOORLET_SHORT, RATES_ON_FIX, VOLS_ON_FIX);
		assertEquals(computedCaplet, PointSensitivityBuilder.none());
		assertEquals(computedFloorlet, PointSensitivityBuilder.none());

	  }

	  public virtual void test_presentValueSensitivityVolatility_afterFix()
	  {
		PointSensitivityBuilder computedCaplet = PRICER.presentValueSensitivityModelParamsVolatility(CAPLET_LONG, RATES_AFTER_FIX, VOLS_AFTER_FIX);
		PointSensitivityBuilder computedFloorlet = PRICER.presentValueSensitivityModelParamsVolatility(FLOORLET_SHORT, RATES_AFTER_FIX, VOLS_AFTER_FIX);
		assertEquals(computedCaplet, PointSensitivityBuilder.none());
		assertEquals(computedFloorlet, PointSensitivityBuilder.none());
	  }

	  public virtual void test_presentValueSensitivityVolatility_shifted()
	  {
		PointSensitivityBuilder pointCaplet = PRICER.presentValueSensitivityModelParamsVolatility(CAPLET_LONG, RATES, SHIFTED_VOLS);
		CurrencyParameterSensitivity computedCaplet = SHIFTED_VOLS.parameterSensitivity(pointCaplet.build()).Sensitivities.get(0);
		PointSensitivityBuilder pointFloorlet = PRICER.presentValueSensitivityModelParamsVolatility(FLOORLET_SHORT, RATES, SHIFTED_VOLS);
		CurrencyParameterSensitivity computedFloorlet = SHIFTED_VOLS.parameterSensitivity(pointFloorlet.build()).Sensitivities.get(0);
		testSurfaceSensitivity(computedCaplet, SHIFTED_VOLS, v => PRICER.presentValue(CAPLET_LONG, RATES, v));
		testSurfaceSensitivity(computedFloorlet, SHIFTED_VOLS, v => PRICER.presentValue(FLOORLET_SHORT, RATES, v));
	  }

	  private void testSurfaceSensitivity(CurrencyParameterSensitivity computed, ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities vols, System.Func<IborCapletFloorletVolatilities, CurrencyAmount> valueFn)
	  {
		double pvBase = valueFn(vols).Amount;
		InterpolatedNodalSurface surfaceBase = (InterpolatedNodalSurface) vols.Surface;
		int nParams = surfaceBase.ParameterCount;
		for (int i = 0; i < nParams; i++)
		{
		  DoubleArray zBumped = surfaceBase.ZValues.with(i, surfaceBase.ZValues.get(i) + EPS_FD);
		  InterpolatedNodalSurface surfaceBumped = surfaceBase.withZValues(zBumped);
		  ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities volsBumped = ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities.of(vols.Index, vols.ValuationDateTime, surfaceBumped, vols.ShiftCurve);
		  double fd = (valueFn(volsBumped).Amount - pvBase) / EPS_FD;
		  assertEquals(computed.Sensitivity.get(i), fd, NOTIONAL * EPS_FD);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_fail_normal()
	  {
		assertThrowsIllegalArg(() => PRICER.presentValue(CAPLET_LONG, RATES, VOLS_NORMAL));
		assertThrowsIllegalArg(() => PRICER.impliedVolatility(CAPLET_LONG, RATES, VOLS_NORMAL));
		assertThrowsIllegalArg(() => PRICER.presentValueDelta(CAPLET_LONG, RATES, VOLS_NORMAL));
		assertThrowsIllegalArg(() => PRICER.presentValueGamma(CAPLET_LONG, RATES, VOLS_NORMAL));
		assertThrowsIllegalArg(() => PRICER.presentValueTheta(CAPLET_LONG, RATES, VOLS_NORMAL));
		assertThrowsIllegalArg(() => PRICER.presentValueSensitivityRates(CAPLET_LONG, RATES, VOLS_NORMAL));
		assertThrowsIllegalArg(() => PRICER.presentValueSensitivityModelParamsVolatility(CAPLET_LONG, RATES, VOLS_NORMAL));
	  }

	  //-------------------------------------------------------------------------
	  private static readonly IborCapletFloorletPeriod CAPLET_REG = IborCapletFloorletPeriod.builder().caplet(0.04).startDate(RATE_COMP.EffectiveDate).endDate(RATE_COMP.MaturityDate).yearFraction(RATE_COMP.YearFraction).notional(NOTIONAL).iborRate(RATE_COMP).build();

	  public virtual void regression_pv()
	  {
		CurrencyAmount pv = PRICER.presentValue(CAPLET_REG, RATES, VOLS);
		assertEquals(pv.Amount, 3.4403901240887094, TOL); // 2.x
	  }

	  public virtual void regression_pvSensi()
	  {
		PointSensitivityBuilder point = PRICER.presentValueSensitivityRates(CAPLET_REG, RATES, VOLS);
		CurrencyParameterSensitivities sensi = RATES.parameterSensitivity(point.build());
		double[] sensiDsc = new double[] {0.0, 0.0, 0.0, -7.148360371957523, -1.8968344850148018, 0.0}; // 2.x
		double[] sensiFwd = new double[] {0.0, 0.0, 0.0, -3999.714444844649, 5987.977558683395, 0.0, 0.0, 0.0}; // 2.x
		assertTrue(DoubleArrayMath.fuzzyEquals(sensi.getSensitivity(IborCapletFloorletDataSet.DSC_NAME, EUR).Sensitivity.toArray(), sensiDsc, NOTIONAL * TOL));
		assertTrue(DoubleArrayMath.fuzzyEquals(sensi.getSensitivity(IborCapletFloorletDataSet.FWD3_NAME, EUR).Sensitivity.toArray(), sensiFwd, NOTIONAL * TOL));
	  }

	}

}