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
	/// Test <seealso cref="ZeroRateDiscountFactors"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ZeroRateDiscountFactorsTest
	public class ZeroRateDiscountFactorsTest
	{

	  private static readonly LocalDate DATE_VAL = date(2015, 6, 4);
	  private static readonly LocalDate DATE_AFTER = date(2015, 7, 30);

	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;
	  private static readonly CurveName NAME = CurveName.of("TestCurve");
	  private static readonly CurveMetadata METADATA = Curves.zeroRates(NAME, ACT_365F);

	  private static readonly InterpolatedNodalCurve CURVE = InterpolatedNodalCurve.of(METADATA, DoubleArray.of(0, 10), DoubleArray.of(1, 2), INTERPOLATOR);
	  private static readonly InterpolatedNodalCurve CURVE2 = InterpolatedNodalCurve.of(METADATA, DoubleArray.of(0, 10), DoubleArray.of(2, 3), INTERPOLATOR);

	  private const double SPREAD = 0.05;
	  private const double TOL = 1.0e-12;
	  private const double TOL_FD = 1.0e-8;
	  private const double EPS = 1.0e-6;

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		ZeroRateDiscountFactors test = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE);
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
		InterpolatedNodalCurve notZeroRate = InterpolatedNodalCurve.of(Curves.discountFactors(NAME, ACT_365F), DoubleArray.of(0, 10), DoubleArray.of(1, 2), INTERPOLATOR);
		CurveMetadata noDayCountMetadata = DefaultCurveMetadata.builder().curveName(NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).build();
		InterpolatedNodalCurve notDayCount = InterpolatedNodalCurve.of(noDayCountMetadata, DoubleArray.of(0, 10), DoubleArray.of(1, 2), INTERPOLATOR);
		assertThrowsIllegalArg(() => ZeroRateDiscountFactors.of(GBP, DATE_VAL, notYearFraction));
		assertThrowsIllegalArg(() => ZeroRateDiscountFactors.of(GBP, DATE_VAL, notZeroRate));
		assertThrowsIllegalArg(() => ZeroRateDiscountFactors.of(GBP, DATE_VAL, notDayCount));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_discountFactor()
	  {
		ZeroRateDiscountFactors test = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double expected = Math.Exp(-relativeYearFraction * CURVE.yValue(relativeYearFraction));
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
		ZeroRateDiscountFactors test = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double discountFactor = test.discountFactor(DATE_AFTER);
		double zeroRate = test.zeroRate(DATE_AFTER);
		assertEquals(Math.Exp(-zeroRate * relativeYearFraction), discountFactor);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_discountFactorWithSpread_continuous()
	  {
		ZeroRateDiscountFactors test = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double expected = Math.Exp(-relativeYearFraction * (CURVE.yValue(relativeYearFraction) + SPREAD));
		assertEquals(test.discountFactorWithSpread(DATE_AFTER, SPREAD, CONTINUOUS, 0), expected, TOL);
	  }

	  public virtual void test_discountFactorWithSpread_periodic()
	  {
		int periodPerYear = 4;
		ZeroRateDiscountFactors test = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double discountFactorBase = test.discountFactor(DATE_AFTER);
		double rate = (Math.Pow(discountFactorBase, -1d / periodPerYear / relativeYearFraction) - 1d) * periodPerYear;
		double expected = discountFactorFromPeriodicallyCompoundedRate(rate + SPREAD, periodPerYear, relativeYearFraction);
		assertEquals(test.discountFactorWithSpread(DATE_AFTER, SPREAD, PERIODIC, periodPerYear), expected, TOL);
	  }

	  public virtual void test_discountFactorWithSpread_smallYearFraction()
	  {
		ZeroRateDiscountFactors test = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE);
		assertEquals(test.discountFactorWithSpread(DATE_VAL, SPREAD, PERIODIC, 1), 1d, TOL);
	  }

	  private double discountFactorFromPeriodicallyCompoundedRate(double rate, double periodPerYear, double time)
	  {
		return Math.Pow(1d + rate / periodPerYear, -periodPerYear * time);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_zeroRatePointSensitivity()
	  {
		ZeroRateDiscountFactors test = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double df = Math.Exp(-relativeYearFraction * CURVE.yValue(relativeYearFraction));
		ZeroRateSensitivity expected = ZeroRateSensitivity.of(GBP, relativeYearFraction, -df * relativeYearFraction);
		assertEquals(test.zeroRatePointSensitivity(DATE_AFTER), expected);
	  }

	  public virtual void test_zeroRatePointSensitivity_sensitivityCurrency()
	  {
		ZeroRateDiscountFactors test = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double df = Math.Exp(-relativeYearFraction * CURVE.yValue(relativeYearFraction));
		ZeroRateSensitivity expected = ZeroRateSensitivity.of(GBP, relativeYearFraction, USD, -df * relativeYearFraction);
		assertEquals(test.zeroRatePointSensitivity(DATE_AFTER, USD), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_zeroRatePointSensitivityWithSpread_continous()
	  {
		ZeroRateDiscountFactors test = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double df = Math.Exp(-relativeYearFraction * (CURVE.yValue(relativeYearFraction) + SPREAD));
		ZeroRateSensitivity expected = ZeroRateSensitivity.of(GBP, relativeYearFraction, -df * relativeYearFraction);
		assertEquals(test.zeroRatePointSensitivityWithSpread(DATE_AFTER, SPREAD, CONTINUOUS, 0), expected);
	  }

	  public virtual void test_zeroRatePointSensitivityWithSpread_sensitivityCurrency_continous()
	  {
		ZeroRateDiscountFactors test = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double df = Math.Exp(-relativeYearFraction * (CURVE.yValue(relativeYearFraction) + SPREAD));
		ZeroRateSensitivity expected = ZeroRateSensitivity.of(GBP, relativeYearFraction, USD, -df * relativeYearFraction);
		assertEquals(test.zeroRatePointSensitivityWithSpread(DATE_AFTER, USD, SPREAD, CONTINUOUS, 0), expected);
	  }

	  public virtual void test_zeroRatePointSensitivityWithSpread_periodic()
	  {
		int periodPerYear = 4;
		ZeroRateDiscountFactors test = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double discountFactorUp = Math.Exp(-(CURVE.yValue(relativeYearFraction) + EPS) * relativeYearFraction);
		double discountFactorDw = Math.Exp(-(CURVE.yValue(relativeYearFraction) - EPS) * relativeYearFraction);
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
		ZeroRateDiscountFactors test = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double discountFactorUp = Math.Exp(-(CURVE.yValue(relativeYearFraction) + EPS) * relativeYearFraction);
		double discountFactorDw = Math.Exp(-(CURVE.yValue(relativeYearFraction) - EPS) * relativeYearFraction);
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
		ZeroRateDiscountFactors test = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE);
		ZeroRateSensitivity expected = ZeroRateSensitivity.of(GBP, 0d, -0d);
		assertEquals(test.zeroRatePointSensitivityWithSpread(DATE_VAL, SPREAD, CONTINUOUS, 0), expected);
	  }

	  public virtual void test_zeroRatePointSensitivityWithSpread_sensitivityCurrency_smallYearFraction()
	  {
		ZeroRateDiscountFactors test = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE);
		ZeroRateSensitivity expected = ZeroRateSensitivity.of(GBP, 0d, USD, -0d);
		assertEquals(test.zeroRatePointSensitivityWithSpread(DATE_VAL, USD, SPREAD, PERIODIC, 2), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_unitParameterSensitivity()
	  {
		ZeroRateDiscountFactors test = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE);
		ZeroRateSensitivity sens = test.zeroRatePointSensitivity(DATE_AFTER);

		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		CurrencyParameterSensitivities expected = CurrencyParameterSensitivities.of(CURVE.yValueParameterSensitivity(relativeYearFraction).multipliedBy(sens.Currency, sens.Sensitivity));
		assertEquals(test.parameterSensitivity(sens), expected);
	  }

	  //-------------------------------------------------------------------------
	  // proper end-to-end FD tests are elsewhere
	  public virtual void test_parameterSensitivity()
	  {
		ZeroRateDiscountFactors test = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE);
		ZeroRateSensitivity point = ZeroRateSensitivity.of(GBP, 1d, 1d);
		assertEquals(test.parameterSensitivity(point).size(), 1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createParameterSensitivity()
	  {
		ZeroRateDiscountFactors test = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE);
		DoubleArray sensitivities = DoubleArray.of(0.12, 0.15);
		CurrencyParameterSensitivities sens = test.createParameterSensitivity(USD, sensitivities);
		assertEquals(sens.Sensitivities.get(0), CURVE.createParameterSensitivity(USD, sensitivities));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withCurve()
	  {
		ZeroRateDiscountFactors test = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE).withCurve(CURVE2);
		assertEquals(test.Curve, CURVE2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ZeroRateDiscountFactors test = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE);
		coverImmutableBean(test);
		ZeroRateDiscountFactors test2 = ZeroRateDiscountFactors.of(USD, DATE_VAL.plusDays(1), CURVE2);
		coverBeanEquals(test, test2);
	  }

	}

}