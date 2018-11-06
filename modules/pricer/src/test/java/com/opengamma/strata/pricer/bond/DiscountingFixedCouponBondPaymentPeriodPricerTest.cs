/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.CompoundedRateType.PERIODIC;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using LegalEntityGroup = com.opengamma.strata.market.curve.LegalEntityGroup;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using FixedCouponBondPaymentPeriod = com.opengamma.strata.product.bond.FixedCouponBondPaymentPeriod;

	/// <summary>
	/// Test <seealso cref="DiscountingFixedCouponBondPaymentPeriodPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingFixedCouponBondPaymentPeriodPricerTest
	public class DiscountingFixedCouponBondPaymentPeriodPricerTest
	{

	  // issuer curves
	  private static readonly LocalDate VAL_DATE = date(2015, 1, 28);
	  private static readonly LocalDate VAL_DATE_AFTER = date(2015, 8, 28);
	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;
	  private static readonly CurveName NAME = CurveName.of("TestCurve");
	  private static readonly CurveMetadata METADATA = Curves.zeroRates(NAME, ACT_365F);
	  private static readonly InterpolatedNodalCurve CURVE = InterpolatedNodalCurve.of(METADATA, DoubleArray.of(0, 10), DoubleArray.of(0.1, 0.18), INTERPOLATOR);
	  private static readonly DiscountFactors DSC_FACTORS = ZeroRateDiscountFactors.of(GBP, VAL_DATE, CURVE);
	  private static readonly DiscountFactors DSC_FACTORS_AFTER = ZeroRateDiscountFactors.of(GBP, VAL_DATE_AFTER, CURVE);
	  private static readonly LegalEntityGroup GROUP = LegalEntityGroup.of("ISSUER1");
	  private static readonly IssuerCurveDiscountFactors ISSUER_CURVE = IssuerCurveDiscountFactors.of(DSC_FACTORS, GROUP);
	  private static readonly IssuerCurveDiscountFactors ISSUER_CURVE_AFTER = IssuerCurveDiscountFactors.of(DSC_FACTORS_AFTER, GROUP);
	  // coupon payment
	  private static readonly LocalDate START = LocalDate.of(2015, 2, 2);
	  private static readonly LocalDate END = LocalDate.of(2015, 8, 2);
	  private static readonly LocalDate START_ADJUSTED = LocalDate.of(2015, 2, 2);
	  private static readonly LocalDate END_ADJUSTED = LocalDate.of(2015, 8, 3);
	  private const double FIXED_RATE = 0.025;
	  private const double NOTIONAL = 1.0e7;
	  private const double YEAR_FRACTION = 0.51;
	  private static readonly FixedCouponBondPaymentPeriod PAYMENT_PERIOD = FixedCouponBondPaymentPeriod.builder().currency(USD).startDate(START_ADJUSTED).unadjustedStartDate(START).endDate(END_ADJUSTED).unadjustedEndDate(END).notional(NOTIONAL).fixedRate(FIXED_RATE).yearFraction(YEAR_FRACTION).build();
	  /// z-spread
	  private const double Z_SPREAD = 0.02;
	  private const int PERIOD_PER_YEAR = 4;

	  private static readonly DiscountingFixedCouponBondPaymentPeriodPricer PRICER = DiscountingFixedCouponBondPaymentPeriodPricer.DEFAULT;
	  private const double TOL = 1.0e-12;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		double computed = PRICER.presentValue(PAYMENT_PERIOD, ISSUER_CURVE);
		double expected = FIXED_RATE * NOTIONAL * YEAR_FRACTION * DSC_FACTORS.discountFactor(END_ADJUSTED);
		assertEquals(computed, expected);
	  }

	  public virtual void test_presentValueWithSpread()
	  {
		double computed = PRICER.presentValueWithSpread(PAYMENT_PERIOD, ISSUER_CURVE, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		double expected = FIXED_RATE * NOTIONAL * YEAR_FRACTION * DSC_FACTORS.discountFactorWithSpread(END_ADJUSTED, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		assertEquals(computed, expected);
	  }

	  public virtual void test_forecastValue()
	  {
		double computed = PRICER.forecastValue(PAYMENT_PERIOD, ISSUER_CURVE);
		double expected = FIXED_RATE * NOTIONAL * YEAR_FRACTION;
		assertEquals(computed, expected);
	  }

	  public virtual void test_presentValue_past()
	  {
		double computed = PRICER.presentValue(PAYMENT_PERIOD, ISSUER_CURVE_AFTER);
		assertEquals(computed, 0d);
	  }

	  public virtual void test_presentValueWithSpread_past()
	  {
		double computed = PRICER.presentValueWithSpread(PAYMENT_PERIOD, ISSUER_CURVE_AFTER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		assertEquals(computed, 0d);
	  }

	  public virtual void test_forecastValue_past()
	  {
		double computed = PRICER.forecastValue(PAYMENT_PERIOD, ISSUER_CURVE_AFTER);
		assertEquals(computed, 0d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity()
	  {
		PointSensitivityBuilder computed = PRICER.presentValueSensitivity(PAYMENT_PERIOD, ISSUER_CURVE);
		PointSensitivityBuilder expected = IssuerCurveZeroRateSensitivity.of(DSC_FACTORS.zeroRatePointSensitivity(END_ADJUSTED).multipliedBy(FIXED_RATE * NOTIONAL * YEAR_FRACTION), GROUP);
		assertEquals(computed, expected);
	  }

	  public virtual void test_presentValueSensitivityWithSpread()
	  {
		PointSensitivityBuilder computed = PRICER.presentValueSensitivityWithSpread(PAYMENT_PERIOD, ISSUER_CURVE, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		PointSensitivityBuilder expected = IssuerCurveZeroRateSensitivity.of(DSC_FACTORS.zeroRatePointSensitivityWithSpread(END_ADJUSTED, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR).multipliedBy(FIXED_RATE * NOTIONAL * YEAR_FRACTION), GROUP);
		assertEquals(computed, expected);
	  }

	  public virtual void test_forecastValueSensitivity()
	  {
		PointSensitivityBuilder computed = PRICER.forecastValueSensitivity(PAYMENT_PERIOD, ISSUER_CURVE);
		assertEquals(computed, PointSensitivityBuilder.none());
	  }

	  public virtual void test_presentValueSensitivity_past()
	  {
		PointSensitivityBuilder computed = PRICER.presentValueSensitivity(PAYMENT_PERIOD, ISSUER_CURVE_AFTER);
		assertEquals(computed, PointSensitivityBuilder.none());
	  }

	  public virtual void test_presentValueSensitivityWithSpread_past()
	  {
		PointSensitivityBuilder computed = PRICER.presentValueSensitivityWithSpread(PAYMENT_PERIOD, ISSUER_CURVE_AFTER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		assertEquals(computed, PointSensitivityBuilder.none());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_explainPresentValue()
	  {
		ExplainMapBuilder builder = ExplainMap.builder();
		PRICER.explainPresentValue(PAYMENT_PERIOD, ISSUER_CURVE, builder);
		ExplainMap explain = builder.build();
		assertEquals(explain.get(ExplainKey.ENTRY_TYPE).get(), "FixedCouponBondPaymentPeriod");
		assertEquals(explain.get(ExplainKey.PAYMENT_DATE).get(), PAYMENT_PERIOD.PaymentDate);
		assertEquals(explain.get(ExplainKey.PAYMENT_CURRENCY).get(), PAYMENT_PERIOD.Currency);
		assertEquals(explain.get(ExplainKey.START_DATE).get(), START_ADJUSTED);
		assertEquals(explain.get(ExplainKey.UNADJUSTED_START_DATE).get(), START);
		assertEquals(explain.get(ExplainKey.END_DATE).get(), END_ADJUSTED);
		assertEquals(explain.get(ExplainKey.UNADJUSTED_END_DATE).get(), END);
		assertEquals(explain.get(ExplainKey.DAYS).Value.intValue(), (int) DAYS.between(START_ADJUSTED, END_ADJUSTED));
		assertEquals(explain.get(ExplainKey.DISCOUNT_FACTOR).Value, DSC_FACTORS.discountFactor(END_ADJUSTED));
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Amount, FIXED_RATE * NOTIONAL * YEAR_FRACTION, NOTIONAL * TOL);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Amount, FIXED_RATE * NOTIONAL * YEAR_FRACTION * DSC_FACTORS.discountFactor(END_ADJUSTED), NOTIONAL * TOL);
	  }

	  public virtual void test_explainPresentValue_past()
	  {
		ExplainMapBuilder builder = ExplainMap.builder();
		PRICER.explainPresentValue(PAYMENT_PERIOD, ISSUER_CURVE_AFTER, builder);
		ExplainMap explain = builder.build();
		assertEquals(explain.get(ExplainKey.ENTRY_TYPE).get(), "FixedCouponBondPaymentPeriod");
		assertEquals(explain.get(ExplainKey.PAYMENT_DATE).get(), PAYMENT_PERIOD.PaymentDate);
		assertEquals(explain.get(ExplainKey.PAYMENT_CURRENCY).get(), PAYMENT_PERIOD.Currency);
		assertEquals(explain.get(ExplainKey.START_DATE).get(), START_ADJUSTED);
		assertEquals(explain.get(ExplainKey.UNADJUSTED_START_DATE).get(), START);
		assertEquals(explain.get(ExplainKey.END_DATE).get(), END_ADJUSTED);
		assertEquals(explain.get(ExplainKey.UNADJUSTED_END_DATE).get(), END);
		assertEquals(explain.get(ExplainKey.DAYS).Value.intValue(), (int) DAYS.between(START_ADJUSTED, END_ADJUSTED));
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Amount, 0d, NOTIONAL * TOL);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_explainPresentValueWithSpread()
	  {
		ExplainMapBuilder builder = ExplainMap.builder();
		PRICER.explainPresentValueWithSpread(PAYMENT_PERIOD, ISSUER_CURVE, builder, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		ExplainMap explain = builder.build();
		assertEquals(explain.get(ExplainKey.ENTRY_TYPE).get(), "FixedCouponBondPaymentPeriod");
		assertEquals(explain.get(ExplainKey.PAYMENT_DATE).get(), PAYMENT_PERIOD.PaymentDate);
		assertEquals(explain.get(ExplainKey.PAYMENT_CURRENCY).get(), PAYMENT_PERIOD.Currency);
		assertEquals(explain.get(ExplainKey.START_DATE).get(), START_ADJUSTED);
		assertEquals(explain.get(ExplainKey.UNADJUSTED_START_DATE).get(), START);
		assertEquals(explain.get(ExplainKey.END_DATE).get(), END_ADJUSTED);
		assertEquals(explain.get(ExplainKey.UNADJUSTED_END_DATE).get(), END);
		assertEquals(explain.get(ExplainKey.DAYS).Value.intValue(), (int) DAYS.between(START_ADJUSTED, END_ADJUSTED));
		assertEquals(explain.get(ExplainKey.DISCOUNT_FACTOR).Value, DSC_FACTORS.discountFactorWithSpread(END_ADJUSTED, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR));
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Amount, FIXED_RATE * NOTIONAL * YEAR_FRACTION, NOTIONAL * TOL);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Amount, FIXED_RATE * NOTIONAL * YEAR_FRACTION * DSC_FACTORS.discountFactorWithSpread(END_ADJUSTED, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR), NOTIONAL * TOL);
	  }

	  public virtual void test_explainPresentValueWithSpread_past()
	  {
		ExplainMapBuilder builder = ExplainMap.builder();
		PRICER.explainPresentValueWithSpread(PAYMENT_PERIOD, ISSUER_CURVE_AFTER, builder, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		ExplainMap explain = builder.build();
		assertEquals(explain.get(ExplainKey.ENTRY_TYPE).get(), "FixedCouponBondPaymentPeriod");
		assertEquals(explain.get(ExplainKey.PAYMENT_DATE).get(), PAYMENT_PERIOD.PaymentDate);
		assertEquals(explain.get(ExplainKey.PAYMENT_CURRENCY).get(), PAYMENT_PERIOD.Currency);
		assertEquals(explain.get(ExplainKey.START_DATE).get(), START_ADJUSTED);
		assertEquals(explain.get(ExplainKey.UNADJUSTED_START_DATE).get(), START);
		assertEquals(explain.get(ExplainKey.END_DATE).get(), END_ADJUSTED);
		assertEquals(explain.get(ExplainKey.UNADJUSTED_END_DATE).get(), END);
		assertEquals(explain.get(ExplainKey.DAYS).Value.intValue(), (int) DAYS.between(START_ADJUSTED, END_ADJUSTED));
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Amount, 0d, NOTIONAL * TOL);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Amount, 0d, NOTIONAL * TOL);
	  }

	}

}