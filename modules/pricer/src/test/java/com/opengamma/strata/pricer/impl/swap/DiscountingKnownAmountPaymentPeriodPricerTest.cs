/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SimpleRatesProvider = com.opengamma.strata.pricer.rate.SimpleRatesProvider;
	using KnownAmountSwapPaymentPeriod = com.opengamma.strata.product.swap.KnownAmountSwapPaymentPeriod;

	/// <summary>
	/// Test <seealso cref="DiscountingKnownAmountPaymentPeriodPricer"/>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingKnownAmountPaymentPeriodPricerTest
	public class DiscountingKnownAmountPaymentPeriodPricerTest
	{

	  private static readonly DiscountingKnownAmountPaymentPeriodPricer PRICER = DiscountingKnownAmountPaymentPeriodPricer.DEFAULT;
	  private static readonly LocalDate VAL_DATE = LocalDate.of(2014, 1, 22);
	  private static readonly DayCount DAY_COUNT = DayCounts.ACT_360;
	  private static readonly LocalDate DATE_1 = LocalDate.of(2014, 1, 24);
	  private static readonly LocalDate DATE_2 = LocalDate.of(2014, 4, 25);
	  private static readonly LocalDate DATE_2U = LocalDate.of(2014, 4, 24);
	  private const double AMOUNT_1000 = 1000d;
	  private static readonly CurrencyAmount AMOUNT_GBP1000 = CurrencyAmount.of(GBP, 1000);
	  private static readonly LocalDate PAYMENT_DATE = LocalDate.of(2014, 4, 26);
	  private const double DISCOUNT_FACTOR = 0.976d;
	  private const double TOLERANCE_PV = 1E-7;

	  private static readonly Payment PAYMENT = Payment.of(AMOUNT_GBP1000, PAYMENT_DATE);
	  private static readonly Payment PAYMENT_PAST = Payment.of(AMOUNT_GBP1000, VAL_DATE.minusDays(1));

	  private static readonly KnownAmountSwapPaymentPeriod PERIOD = KnownAmountSwapPaymentPeriod.builder().payment(PAYMENT).startDate(DATE_1).endDate(DATE_2).unadjustedEndDate(DATE_2U).build();
	  private static readonly KnownAmountSwapPaymentPeriod PERIOD_PAST = KnownAmountSwapPaymentPeriod.builder().payment(PAYMENT_PAST).startDate(DATE_1).endDate(DATE_2).build();

	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.DOUBLE_QUADRATIC;
	  private static readonly Curve DISCOUNT_CURVE_GBP;
	  static DiscountingKnownAmountPaymentPeriodPricerTest()
	  {
		DoubleArray time_gbp = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 3.0, 4.0, 5.0, 10.0);
		DoubleArray rate_gbp = DoubleArray.of(0.0160, 0.0135, 0.0160, 0.0185, 0.0185, 0.0195, 0.0200, 0.0210);
		DISCOUNT_CURVE_GBP = InterpolatedNodalCurve.of(Curves.zeroRates("GBP-Discount", ACT_ACT_ISDA), time_gbp, rate_gbp, INTERPOLATOR);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		SimpleRatesProvider prov = createProvider(VAL_DATE);

		double pvExpected = AMOUNT_1000 * DISCOUNT_FACTOR;
		double pvComputed = PRICER.presentValue(PERIOD, prov);
		assertEquals(pvComputed, pvExpected, TOLERANCE_PV);
	  }

	  public virtual void test_presentValue_inPast()
	  {
		SimpleRatesProvider prov = createProvider(VAL_DATE);

		double pvComputed = PRICER.presentValue(PERIOD_PAST, prov);
		assertEquals(pvComputed, 0, TOLERANCE_PV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_forecastValue()
	  {
		SimpleRatesProvider prov = createProvider(VAL_DATE);

		double fvExpected = AMOUNT_1000;
		double fvComputed = PRICER.forecastValue(PERIOD, prov);
		assertEquals(fvComputed, fvExpected, TOLERANCE_PV);
	  }

	  public virtual void test_forecastValue_inPast()
	  {
		SimpleRatesProvider prov = createProvider(VAL_DATE);

		double fvComputed = PRICER.forecastValue(PERIOD_PAST, prov);
		assertEquals(fvComputed, 0, TOLERANCE_PV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity()
	  {
		SimpleRatesProvider prov = createProvider(VAL_DATE);

		PointSensitivities point = PRICER.presentValueSensitivity(PERIOD, prov).build();
		double relativeYearFraction = DAY_COUNT.relativeYearFraction(VAL_DATE, PAYMENT_DATE);
		double expected = -DISCOUNT_FACTOR * relativeYearFraction * AMOUNT_1000;
		ZeroRateSensitivity actual = (ZeroRateSensitivity) point.Sensitivities.get(0);
		assertEquals(actual.Currency, GBP);
		assertEquals(actual.CurveCurrency, GBP);
		assertEquals(actual.YearFraction, relativeYearFraction);
		assertEquals(actual.Sensitivity, expected, AMOUNT_1000 * TOLERANCE_PV);
	  }

	  public virtual void test_presentValueSensitivity_inPast()
	  {
		SimpleRatesProvider prov = createProvider(VAL_DATE);

		PointSensitivities computed = PRICER.presentValueSensitivity(PERIOD_PAST, prov).build();
		assertEquals(computed, PointSensitivities.empty());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_forecastValueSensitivity()
	  {
		SimpleRatesProvider prov = createProvider(VAL_DATE);

		assertEquals(PRICER.forecastValueSensitivity(PERIOD, prov), PointSensitivityBuilder.none());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_accruedInterest()
	  {
		LocalDate valDate = PERIOD.StartDate.plusDays(7);
		SimpleRatesProvider prov = createProvider(valDate);

		double expected = AMOUNT_1000 * (7d / (7 + 28 + 31 + 25));
		double computed = PRICER.accruedInterest(PERIOD, prov);
		assertEquals(computed, expected, TOLERANCE_PV);
	  }

	  public virtual void test_accruedInterest_valDateBeforePeriod()
	  {
		SimpleRatesProvider prov = createProvider(PERIOD.StartDate);

		double computed = PRICER.accruedInterest(PERIOD, prov);
		assertEquals(computed, 0, TOLERANCE_PV);
	  }

	  public virtual void test_accruedInterest_valDateAfterPeriod()
	  {
		SimpleRatesProvider prov = createProvider(PERIOD.EndDate.plusDays(1));

		double computed = PRICER.accruedInterest(PERIOD, prov);
		assertEquals(computed, 0, TOLERANCE_PV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_explainPresentValue()
	  {
		RatesProvider prov = createProvider(VAL_DATE);

		ExplainMapBuilder builder = ExplainMap.builder();
		PRICER.explainPresentValue(PERIOD, prov, builder);
		ExplainMap explain = builder.build();

		assertEquals(explain.get(ExplainKey.ENTRY_TYPE).get(), "KnownAmountPaymentPeriod");
		assertEquals(explain.get(ExplainKey.PAYMENT_DATE).get(), PERIOD.PaymentDate);
		assertEquals(explain.get(ExplainKey.PAYMENT_CURRENCY).get(), PERIOD.Currency);
		assertEquals(explain.get(ExplainKey.DISCOUNT_FACTOR).Value, DISCOUNT_FACTOR, TOLERANCE_PV);

		int daysBetween = (int) DAYS.between(DATE_1, DATE_2);
		assertEquals(explain.get(ExplainKey.START_DATE).get(), PERIOD.StartDate);
		assertEquals(explain.get(ExplainKey.UNADJUSTED_START_DATE).get(), PERIOD.UnadjustedStartDate);
		assertEquals(explain.get(ExplainKey.END_DATE).get(), PERIOD.EndDate);
		assertEquals(explain.get(ExplainKey.UNADJUSTED_END_DATE).get(), PERIOD.UnadjustedEndDate);
		assertEquals(explain.get(ExplainKey.DAYS).Value, (int?) daysBetween);

		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Currency, PERIOD.Currency);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Amount, AMOUNT_1000, TOLERANCE_PV);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Currency, PERIOD.Currency);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Amount, AMOUNT_1000 * DISCOUNT_FACTOR, TOLERANCE_PV);
	  }

	  public virtual void test_explainPresentValue_inPast()
	  {
		RatesProvider prov = createProvider(VAL_DATE);

		ExplainMapBuilder builder = ExplainMap.builder();
		PRICER.explainPresentValue(PERIOD_PAST, prov, builder);
		ExplainMap explain = builder.build();

		assertEquals(explain.get(ExplainKey.ENTRY_TYPE).get(), "KnownAmountPaymentPeriod");
		assertEquals(explain.get(ExplainKey.PAYMENT_DATE).get(), PERIOD_PAST.PaymentDate);
		assertEquals(explain.get(ExplainKey.PAYMENT_CURRENCY).get(), PERIOD_PAST.Currency);

		int daysBetween = (int) DAYS.between(DATE_1, DATE_2);
		assertEquals(explain.get(ExplainKey.START_DATE).get(), PERIOD_PAST.StartDate);
		assertEquals(explain.get(ExplainKey.UNADJUSTED_START_DATE).get(), PERIOD_PAST.UnadjustedStartDate);
		assertEquals(explain.get(ExplainKey.END_DATE).get(), PERIOD_PAST.EndDate);
		assertEquals(explain.get(ExplainKey.UNADJUSTED_END_DATE).get(), PERIOD_PAST.UnadjustedEndDate);
		assertEquals(explain.get(ExplainKey.DAYS).Value, (int?) daysBetween);

		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Currency, PERIOD_PAST.Currency);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Amount, 0, TOLERANCE_PV);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Currency, PERIOD_PAST.Currency);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Amount, 0 * DISCOUNT_FACTOR, TOLERANCE_PV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currencyExposure()
	  {
		ImmutableRatesProvider prov = ImmutableRatesProvider.builder(VAL_DATE).discountCurve(GBP, DISCOUNT_CURVE_GBP).build();
		MultiCurrencyAmount computed = PRICER.currencyExposure(PERIOD, prov);
		PointSensitivities point = PRICER.presentValueSensitivity(PERIOD, prov).build();
		MultiCurrencyAmount expected = prov.currencyExposure(point).plus(CurrencyAmount.of(GBP, PRICER.presentValue(PERIOD, prov)));
		assertEquals(computed, expected);
	  }

	  public virtual void test_currentCash_zero()
	  {
		ImmutableRatesProvider prov = ImmutableRatesProvider.builder(VAL_DATE).discountCurve(GBP, DISCOUNT_CURVE_GBP).build();
		double computed = PRICER.currentCash(PERIOD, prov);
		assertEquals(computed, 0d);
	  }

	  public virtual void test_currentCash_onPayment()
	  {
		ImmutableRatesProvider prov = ImmutableRatesProvider.builder(PERIOD.PaymentDate).discountCurve(GBP, DISCOUNT_CURVE_GBP).build();
		double computed = PRICER.currentCash(PERIOD, prov);
		assertEquals(computed, AMOUNT_1000);
	  }

	  //-------------------------------------------------------------------------
	  // creates a simple provider
	  private SimpleRatesProvider createProvider(LocalDate valDate)
	  {
		Curve curve = ConstantCurve.of(Curves.discountFactors("Test", DAY_COUNT), DISCOUNT_FACTOR);
		DiscountFactors df = SimpleDiscountFactors.of(GBP, valDate, curve);
		SimpleRatesProvider prov = new SimpleRatesProvider(valDate);
		prov.DayCount = DAY_COUNT;
		prov.DiscountFactors = df;
		return prov;
	  }

	}

}