using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.CompoundedRateType.CONTINUOUS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.CompoundedRateType.PERIODIC;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using DefaultCurveMetadata = com.opengamma.strata.market.curve.DefaultCurveMetadata;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;

	/// <summary>
	/// Test <seealso cref="SimpleDiscountFactors"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SimpleDiscountFactorsTest
	public class SimpleDiscountFactorsTest
	{

	  private static readonly LocalDate DATE_VAL = date(2015, 6, 4);
	  private static readonly LocalDate DATE_AFTER = date(2015, 7, 30);

	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;
	  private static readonly CurveName NAME = CurveName.of("TestCurve");
	  private static readonly CurveMetadata METADATA = Curves.discountFactors(NAME, ACT_365F);

	  private static readonly InterpolatedNodalCurve CURVE = InterpolatedNodalCurve.of(METADATA, DoubleArray.of(0, 10), DoubleArray.of(1, 2), INTERPOLATOR);
	  private static readonly InterpolatedNodalCurve CURVE2 = InterpolatedNodalCurve.of(METADATA, DoubleArray.of(0, 10), DoubleArray.of(2, 3), INTERPOLATOR);

	  private const double SPREAD = 0.05;
	  private const double TOL = 1.0e-12;
	  private const double TOL_FD = 1.0e-8;
	  private const double EPS = 1.0e-6;

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		SimpleDiscountFactors test = SimpleDiscountFactors.of(GBP, DATE_VAL, CURVE);
		assertEquals(test.Currency, GBP);
		assertEquals(test.ValuationDate, DATE_VAL);
		assertEquals(test.Curve, CURVE);
		assertEquals(test.ParameterCount, CURVE.ParameterCount);
		assertEquals(test.getParameter(0), CURVE.getParameter(0));
		assertEquals(test.getParameterMetadata(0), CURVE.getParameterMetadata(0));
		assertEquals(test.withParameter(0, 1d).Curve, CURVE.withParameter(0, 1d));
		assertEquals(test.withPerturbation((i, v, m) => v + 1d).Curve, CURVE.withPerturbation((i, v, m) => v + 1d));
		assertEquals(test.findData(CURVE.Name), CURVE);
		assertEquals(test.findData(CurveName.of("Rubbish")), null);
	  }

	  public virtual void test_of_badCurve()
	  {
		InterpolatedNodalCurve notYearFraction = InterpolatedNodalCurve.of(Curves.prices(NAME), DoubleArray.of(0, 10), DoubleArray.of(1, 2), INTERPOLATOR);
		InterpolatedNodalCurve notDiscountFactor = InterpolatedNodalCurve.of(Curves.zeroRates(NAME, ACT_365F), DoubleArray.of(0, 10), DoubleArray.of(1, 2), INTERPOLATOR);
		CurveMetadata noDayCountMetadata = DefaultCurveMetadata.builder().curveName(NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.DISCOUNT_FACTOR).build();
		InterpolatedNodalCurve notDayCount = InterpolatedNodalCurve.of(noDayCountMetadata, DoubleArray.of(0, 10), DoubleArray.of(1, 2), INTERPOLATOR);
		assertThrowsIllegalArg(() => SimpleDiscountFactors.of(GBP, DATE_VAL, notYearFraction));
		assertThrowsIllegalArg(() => SimpleDiscountFactors.of(GBP, DATE_VAL, notDiscountFactor));
		assertThrowsIllegalArg(() => SimpleDiscountFactors.of(GBP, DATE_VAL, notDayCount));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_discountFactor()
	  {
		SimpleDiscountFactors test = SimpleDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double expected = CURVE.yValue(relativeYearFraction);
		assertEquals(test.discountFactor(DATE_AFTER), expected);
	  }

	  public virtual void test_discountFactorTimeDerivative()
	  {
		DiscountFactors test = DiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double expectedP = test.discountFactor(relativeYearFraction + EPS);
		double expectedM = test.discountFactor(relativeYearFraction - EPS);
		assertEquals(test.discountFactorTimeDerivative(relativeYearFraction), (expectedP - expectedM) / (2 * EPS), TOL_FD);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_zeroRate()
	  {
		SimpleDiscountFactors test = SimpleDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double discountFactor = test.discountFactor(DATE_AFTER);
		double zeroRate = test.zeroRate(DATE_AFTER);
		assertEquals(Math.Exp(-zeroRate * relativeYearFraction), discountFactor);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_discountFactor_withSpread_continuous()
	  {
		SimpleDiscountFactors test = SimpleDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double expected = CURVE.yValue(relativeYearFraction) * Math.Exp(-SPREAD * relativeYearFraction);
		assertEquals(test.discountFactorWithSpread(DATE_AFTER, SPREAD, CONTINUOUS, 0), expected, TOL);
	  }

	  public virtual void test_discountFactor_withSpread_periodic()
	  {
		int periodPerYear = 4;
		SimpleDiscountFactors test = SimpleDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double discountFactorBase = test.discountFactor(DATE_AFTER);
		double rate = (Math.Pow(discountFactorBase, -1d / periodPerYear / relativeYearFraction) - 1d) * periodPerYear;
		double expected = discountFactorFromPeriodicallyCompoundedRate(rate + SPREAD, periodPerYear, relativeYearFraction);
		assertEquals(test.discountFactorWithSpread(DATE_AFTER, SPREAD, PERIODIC, periodPerYear), expected, TOL);
	  }

	  public virtual void test_discountFactor_withSpread_smallYearFraction()
	  {
		SimpleDiscountFactors test = SimpleDiscountFactors.of(GBP, DATE_VAL, CURVE);
		assertEquals(test.discountFactorWithSpread(DATE_VAL, SPREAD, PERIODIC, 2), 1d);
	  }

	  private double discountFactorFromPeriodicallyCompoundedRate(double rate, double periodPerYear, double time)
	  {
		return Math.Pow(1d + rate / periodPerYear, -periodPerYear * time);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_zeroRatePointSensitivity()
	  {
		SimpleDiscountFactors test = SimpleDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double df = CURVE.yValue(relativeYearFraction);
		ZeroRateSensitivity expected = ZeroRateSensitivity.of(GBP, relativeYearFraction, -df * relativeYearFraction);
		assertEquals(test.zeroRatePointSensitivity(DATE_AFTER), expected);
	  }

	  public virtual void test_zeroRatePointSensitivity_sensitivityCurrency()
	  {
		SimpleDiscountFactors test = SimpleDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double df = CURVE.yValue(relativeYearFraction);
		ZeroRateSensitivity expected = ZeroRateSensitivity.of(GBP, relativeYearFraction, USD, -df * relativeYearFraction);
		assertEquals(test.zeroRatePointSensitivity(DATE_AFTER, USD), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_zeroRatePointSensitivityWithSpread_continuous()
	  {
		SimpleDiscountFactors test = SimpleDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double df = CURVE.yValue(relativeYearFraction) * Math.Exp(-SPREAD * relativeYearFraction);
		ZeroRateSensitivity expected = ZeroRateSensitivity.of(GBP, relativeYearFraction, -df * relativeYearFraction);
		assertEquals(test.zeroRatePointSensitivityWithSpread(DATE_AFTER, SPREAD, CONTINUOUS, 0), expected);
	  }

	  public virtual void test_zeroRatePointSensitivityWithSpread_sensitivityCurrency_continuous()
	  {
		SimpleDiscountFactors test = SimpleDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double df = CURVE.yValue(relativeYearFraction) * Math.Exp(-SPREAD * relativeYearFraction);
		ZeroRateSensitivity expected = ZeroRateSensitivity.of(GBP, relativeYearFraction, USD, -df * relativeYearFraction);
		assertEquals(test.zeroRatePointSensitivityWithSpread(DATE_AFTER, USD, SPREAD, CONTINUOUS, 1), expected);
	  }

	  public virtual void test_zeroRatePointSensitivityWithSpread_periodic()
	  {
		int periodPerYear = 4;
		SimpleDiscountFactors test = SimpleDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double df = CURVE.yValue(relativeYearFraction);
		double discountFactorUp = df * Math.Exp(-EPS * relativeYearFraction);
		double discountFactorDw = df * Math.Exp(EPS * relativeYearFraction);
		double rateUp = (Math.Pow(discountFactorUp, -1d / periodPerYear / relativeYearFraction) - 1d) * periodPerYear;
		double rateDw = (Math.Pow(discountFactorDw, -1d / periodPerYear / relativeYearFraction) - 1d) * periodPerYear;
		double expectedValue = 0.5 / EPS * (discountFactorFromPeriodicallyCompoundedRate(rateUp + SPREAD, periodPerYear, relativeYearFraction) - discountFactorFromPeriodicallyCompoundedRate(rateDw + SPREAD, periodPerYear, relativeYearFraction));
		ZeroRateSensitivity computed = test.zeroRatePointSensitivityWithSpread(DATE_AFTER, SPREAD, PERIODIC, periodPerYear);
		assertEquals(computed.Sensitivity, expectedValue, EPS);
		assertEquals(computed.Currency, GBP);
		assertEquals(computed.CurveCurrency, GBP);
		assertEquals(computed.YearFraction, relativeYearFraction);
	  }

	  public virtual void test_zeroRatePointSensitivityWithSpread_sensitivityCurrency_periodic()
	  {
		int periodPerYear = 4;
		SimpleDiscountFactors test = SimpleDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double df = CURVE.yValue(relativeYearFraction);
		double discountFactorUp = df * Math.Exp(-EPS * relativeYearFraction);
		double discountFactorDw = df * Math.Exp(EPS * relativeYearFraction);
		double rateUp = (Math.Pow(discountFactorUp, -1d / periodPerYear / relativeYearFraction) - 1d) * periodPerYear;
		double rateDw = (Math.Pow(discountFactorDw, -1d / periodPerYear / relativeYearFraction) - 1d) * periodPerYear;
		double expectedValue = 0.5 / EPS * (discountFactorFromPeriodicallyCompoundedRate(rateUp + SPREAD, periodPerYear, relativeYearFraction) - discountFactorFromPeriodicallyCompoundedRate(rateDw + SPREAD, periodPerYear, relativeYearFraction));
		ZeroRateSensitivity computed = test.zeroRatePointSensitivityWithSpread(DATE_AFTER, USD, SPREAD, PERIODIC, periodPerYear);
		assertEquals(computed.Sensitivity, expectedValue, EPS);
		assertEquals(computed.Currency, USD);
		assertEquals(computed.CurveCurrency, GBP);
		assertEquals(computed.YearFraction, relativeYearFraction);
	  }

	  public virtual void test_zeroRatePointSensitivityWithSpread_smallYearFraction()
	  {
		SimpleDiscountFactors test = SimpleDiscountFactors.of(GBP, DATE_VAL, CURVE);
		ZeroRateSensitivity expected = ZeroRateSensitivity.of(GBP, 0d, -0d);
		assertEquals(test.zeroRatePointSensitivityWithSpread(DATE_VAL, SPREAD, CONTINUOUS, 0), expected);
	  }

	  public virtual void test_zeroRatePointSensitivityWithSpread_sensitivityCurrency_smallYearFraction()
	  {
		SimpleDiscountFactors test = SimpleDiscountFactors.of(GBP, DATE_VAL, CURVE);
		ZeroRateSensitivity expected = ZeroRateSensitivity.of(GBP, 0d, USD, -0d);
		assertEquals(test.zeroRatePointSensitivityWithSpread(DATE_VAL, USD, SPREAD, PERIODIC, 2), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currencyParameterSensitivity()
	  {
		SimpleDiscountFactors test = SimpleDiscountFactors.of(GBP, DATE_VAL, CURVE);
		ZeroRateSensitivity sens = test.zeroRatePointSensitivity(DATE_AFTER);

		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double discountFactor = CURVE.yValue(relativeYearFraction);
		CurrencyParameterSensitivities expected = CurrencyParameterSensitivities.of(CURVE.yValueParameterSensitivity(relativeYearFraction).multipliedBy(-1d / discountFactor / relativeYearFraction).multipliedBy(sens.Currency, sens.Sensitivity));
		assertEquals(test.parameterSensitivity(sens), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currencyParameterSensitivity_val_date()
	  {
		// Discount factor at valuation date is always 0, no sensitivity.
		SimpleDiscountFactors test = SimpleDiscountFactors.of(GBP, DATE_VAL, CURVE);
		ZeroRateSensitivity sens = test.zeroRatePointSensitivity(DATE_VAL);
		assertEquals(test.parameterSensitivity(sens), CurrencyParameterSensitivities.empty());
	  }

	  //-------------------------------------------------------------------------
	  // proper end-to-end FD tests are elsewhere
	  public virtual void test_parameterSensitivity()
	  {
		SimpleDiscountFactors test = SimpleDiscountFactors.of(GBP, DATE_VAL, CURVE);
		ZeroRateSensitivity point = ZeroRateSensitivity.of(GBP, 1d, 1d);
		assertEquals(test.parameterSensitivity(point).size(), 1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createParameterSensitivity()
	  {
		SimpleDiscountFactors test = SimpleDiscountFactors.of(GBP, DATE_VAL, CURVE);
		DoubleArray sensitivities = DoubleArray.of(0.12, 0.15);
		CurrencyParameterSensitivities sens = test.createParameterSensitivity(USD, sensitivities);
		assertEquals(sens.Sensitivities.get(0), CURVE.createParameterSensitivity(USD, sensitivities));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withCurve()
	  {
		SimpleDiscountFactors test = SimpleDiscountFactors.of(GBP, DATE_VAL, CURVE).withCurve(CURVE2);
		assertEquals(test.Curve, CURVE2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		SimpleDiscountFactors test = SimpleDiscountFactors.of(GBP, DATE_VAL, CURVE);
		coverImmutableBean(test);
		SimpleDiscountFactors test2 = SimpleDiscountFactors.of(USD, DATE_VAL.plusDays(1), CURVE2);
		coverBeanEquals(test, test2);
	  }

	}

}