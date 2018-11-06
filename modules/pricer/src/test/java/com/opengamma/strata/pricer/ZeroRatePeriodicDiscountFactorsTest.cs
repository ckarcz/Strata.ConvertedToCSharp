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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using DefaultCurveMetadata = com.opengamma.strata.market.curve.DefaultCurveMetadata;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;

	/// <summary>
	/// Test <seealso cref="ZeroRatePeriodicDiscountFactors"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ZeroRatePeriodicDiscountFactorsTest
	public class ZeroRatePeriodicDiscountFactorsTest
	{

	  private static readonly LocalDate DATE_VAL = date(2015, 6, 4);
	  private static readonly LocalDate DATE_AFTER = date(2016, 7, 21);

	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;
	  private static readonly CurveName NAME = CurveName.of("TestCurve");
	  private const int CMP_PERIOD = 2;
	  private static readonly CurveMetadata META_ZERO_PERIODIC = DefaultCurveMetadata.builder().curveName(NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).addInfo(CurveInfoType.COMPOUNDING_PER_YEAR, CMP_PERIOD).build();

	  private static readonly DoubleArray X = DoubleArray.of(0, 5, 10);
	  private static readonly DoubleArray Y = DoubleArray.of(0.0100, 0.0200, 0.0150);
	  private static readonly InterpolatedNodalCurve CURVE = InterpolatedNodalCurve.of(META_ZERO_PERIODIC, X, Y, INTERPOLATOR);
	  private static readonly InterpolatedNodalCurve CURVE2 = InterpolatedNodalCurve.of(META_ZERO_PERIODIC, DoubleArray.of(0, 10), DoubleArray.of(2, 3), INTERPOLATOR);

	  private const double SPREAD = 0.05;
	  private const double TOLERANCE_DF = 1.0e-12;
	  private const double TOLERANCE_DELTA = 1.0e-10;
	  private const double TOLERANCE_DELTA_FD = 1.0e-8;
	  private const double EPS = 1.0e-6;

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		ZeroRatePeriodicDiscountFactors test = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, CURVE);
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
		CurveMetadata noDayCountMetadata = DefaultCurveMetadata.builder().curveName(NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).addInfo(CurveInfoType.COMPOUNDING_PER_YEAR, 4).build();
		InterpolatedNodalCurve notDayCount = InterpolatedNodalCurve.of(noDayCountMetadata, DoubleArray.of(0, 10), DoubleArray.of(1, 2), INTERPOLATOR);
		CurveMetadata metaNoCompoundPerYear = DefaultCurveMetadata.builder().curveName(NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).build();
		InterpolatedNodalCurve notCompoundPerYear = InterpolatedNodalCurve.of(metaNoCompoundPerYear, DoubleArray.of(0, 10), DoubleArray.of(1, 2), INTERPOLATOR);
		CurveMetadata metaNegativeNb = DefaultCurveMetadata.builder().curveName(NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).addInfo(CurveInfoType.COMPOUNDING_PER_YEAR, -1).build();
		InterpolatedNodalCurve curveNegativeNb = InterpolatedNodalCurve.of(metaNegativeNb, DoubleArray.of(0, 10), DoubleArray.of(1, 2), INTERPOLATOR);
		assertThrowsIllegalArg(() => ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, notYearFraction));
		assertThrowsIllegalArg(() => ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, notZeroRate));
		assertThrowsIllegalArg(() => ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, notDayCount));
		assertThrowsIllegalArg(() => ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, notCompoundPerYear));
		assertThrowsIllegalArg(() => ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, curveNegativeNb));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_discountFactor()
	  {
		ZeroRatePeriodicDiscountFactors test = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double expected = Math.Pow(1.0d + CURVE.yValue(relativeYearFraction) / CMP_PERIOD, -CMP_PERIOD * relativeYearFraction);
		assertEquals(test.discountFactor(DATE_AFTER), expected);
	  }

	  public virtual void test_discountFactorTimeDerivative()
	  {
		DiscountFactors test = DiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double expectedP = test.discountFactor(relativeYearFraction + EPS);
		double expectedM = test.discountFactor(relativeYearFraction - EPS);
		assertEquals(test.discountFactorTimeDerivative(relativeYearFraction), (expectedP - expectedM) / (2 * EPS), TOLERANCE_DELTA_FD);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_zeroRate()
	  {
		ZeroRatePeriodicDiscountFactors test = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double discountFactor = test.discountFactor(DATE_AFTER);
		double zeroRate = test.zeroRate(DATE_AFTER);
		assertEquals(Math.Exp(-zeroRate * relativeYearFraction), discountFactor);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_discountFactorWithSpread_continuous()
	  {
		ZeroRatePeriodicDiscountFactors test = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double df = test.discountFactor(DATE_AFTER);
		double expected = df * Math.Exp(-SPREAD * relativeYearFraction);
		assertEquals(test.discountFactorWithSpread(DATE_AFTER, SPREAD, CONTINUOUS, 0), expected, TOLERANCE_DF);
	  }

	  public virtual void test_discountFactorWithSpread_periodic()
	  {
		int periodPerYear = 4;
		ZeroRatePeriodicDiscountFactors test = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double discountFactorBase = test.discountFactor(DATE_AFTER);
		double onePlus = Math.Pow(discountFactorBase, -1.0d / (periodPerYear * relativeYearFraction));
		double expected = Math.Pow(onePlus + SPREAD / periodPerYear, -periodPerYear * relativeYearFraction);
		assertEquals(test.discountFactorWithSpread(DATE_AFTER, SPREAD, PERIODIC, periodPerYear), expected, TOLERANCE_DF);
	  }

	  public virtual void test_discountFactorWithSpread_smallYearFraction()
	  {
		ZeroRatePeriodicDiscountFactors test = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, CURVE);
		assertEquals(test.discountFactorWithSpread(DATE_VAL, SPREAD, PERIODIC, 1), 1d, TOLERANCE_DF);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_zeroRatePointSensitivity()
	  {
		ZeroRatePeriodicDiscountFactors test = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double df = test.discountFactor(DATE_AFTER);
		ZeroRateSensitivity expected = ZeroRateSensitivity.of(GBP, relativeYearFraction, -df * relativeYearFraction);
		assertEquals(test.zeroRatePointSensitivity(DATE_AFTER), expected);
	  }

	  public virtual void test_zeroRatePointSensitivity_sensitivityCurrency()
	  {
		ZeroRatePeriodicDiscountFactors test = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double df = test.discountFactor(DATE_AFTER);
		ZeroRateSensitivity expected = ZeroRateSensitivity.of(GBP, relativeYearFraction, USD, -df * relativeYearFraction);
		assertEquals(test.zeroRatePointSensitivity(DATE_AFTER, USD), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_zeroRatePointSensitivityWithSpread_continous()
	  {
		ZeroRatePeriodicDiscountFactors test = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double df = test.discountFactorWithSpread(DATE_AFTER, SPREAD, CONTINUOUS, 0);
		ZeroRateSensitivity expected = ZeroRateSensitivity.of(GBP, relativeYearFraction, -df * relativeYearFraction);
		ZeroRateSensitivity computed = test.zeroRatePointSensitivityWithSpread(DATE_AFTER, SPREAD, CONTINUOUS, 0);
		assertTrue(computed.compareKey(expected) == 0);
		assertEquals(computed.Sensitivity, expected.Sensitivity, TOLERANCE_DELTA);
	  }

	  public virtual void test_zeroRatePointSensitivityWithSpread_sensitivityCurrency_continous()
	  {
		ZeroRatePeriodicDiscountFactors test = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double df = test.discountFactorWithSpread(DATE_AFTER, SPREAD, CONTINUOUS, 0);
		ZeroRateSensitivity expected = ZeroRateSensitivity.of(GBP, relativeYearFraction, USD, -df * relativeYearFraction);
		ZeroRateSensitivity computed = test.zeroRatePointSensitivityWithSpread(DATE_AFTER, USD, SPREAD, CONTINUOUS, 0);
		assertTrue(computed.compareKey(expected) == 0);
		assertEquals(computed.Sensitivity, expected.Sensitivity, TOLERANCE_DELTA);
	  }

	  public virtual void test_zeroRatePointSensitivityWithSpread_periodic()
	  {
		int periodPerYear = 4;
		ZeroRatePeriodicDiscountFactors test = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double df = test.discountFactor(DATE_AFTER);
		double z = -1.0 / relativeYearFraction * Math.Log(df);
		double shift = 1.0E-6;
		double zP = z + shift;
		double zM = z - shift;
		double dfSP = Math.Pow(Math.Pow(Math.Exp(-zP * relativeYearFraction), -1.0 / (relativeYearFraction * periodPerYear)) + SPREAD / periodPerYear, -relativeYearFraction * periodPerYear);
		double dfSM = Math.Pow(Math.Pow(Math.Exp(-zM * relativeYearFraction), -1.0 / (relativeYearFraction * periodPerYear)) + SPREAD / periodPerYear, -relativeYearFraction * periodPerYear);
		double ddfSdz = (dfSP - dfSM) / (2 * shift);
		ZeroRateSensitivity expected = ZeroRateSensitivity.of(GBP, relativeYearFraction, ddfSdz);
		ZeroRateSensitivity computed = test.zeroRatePointSensitivityWithSpread(DATE_AFTER, SPREAD, PERIODIC, periodPerYear);
		assertTrue(computed.compareKey(expected) == 0);
		assertEquals(computed.Sensitivity, expected.Sensitivity, TOLERANCE_DELTA_FD);
	  }

	  public virtual void test_zeroRatePointSensitivityWithSpread_sensitivityCurrency_periodic()
	  {
		int periodPerYear = 4;
		ZeroRatePeriodicDiscountFactors test = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double relativeYearFraction = ACT_365F.relativeYearFraction(DATE_VAL, DATE_AFTER);
		double df = test.discountFactor(DATE_AFTER);
		double z = -1.0 / relativeYearFraction * Math.Log(df);
		double shift = 1.0E-6;
		double zP = z + shift;
		double zM = z - shift;
		double dfSP = Math.Pow(Math.Pow(Math.Exp(-zP * relativeYearFraction), -1.0 / (relativeYearFraction * periodPerYear)) + SPREAD / periodPerYear, -relativeYearFraction * periodPerYear);
		double dfSM = Math.Pow(Math.Pow(Math.Exp(-zM * relativeYearFraction), -1.0 / (relativeYearFraction * periodPerYear)) + SPREAD / periodPerYear, -relativeYearFraction * periodPerYear);
		double ddfSdz = (dfSP - dfSM) / (2 * shift);
		ZeroRateSensitivity expected = ZeroRateSensitivity.of(GBP, relativeYearFraction, USD, ddfSdz);
		ZeroRateSensitivity computed = test.zeroRatePointSensitivityWithSpread(DATE_AFTER, USD, SPREAD, PERIODIC, periodPerYear);
		assertTrue(computed.compareKey(expected) == 0);
		assertEquals(computed.Sensitivity, expected.Sensitivity, TOLERANCE_DELTA_FD);
	  }

	  public virtual void test_zeroRatePointSensitivityWithSpread_smallYearFraction()
	  {
		ZeroRatePeriodicDiscountFactors test = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, CURVE);
		ZeroRateSensitivity expected = ZeroRateSensitivity.of(GBP, 0d, 0.0d);
		ZeroRateSensitivity computed = test.zeroRatePointSensitivityWithSpread(DATE_VAL, SPREAD, CONTINUOUS, 0);
		assertTrue(computed.compareKey(expected) == 0);
		assertEquals(computed.Sensitivity, expected.Sensitivity, TOLERANCE_DELTA_FD);
	  }

	  public virtual void test_zeroRatePointSensitivityWithSpread_sensitivityCurrency_smallYearFraction()
	  {
		ZeroRatePeriodicDiscountFactors test = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, CURVE);
		ZeroRateSensitivity expected = ZeroRateSensitivity.of(GBP, 0d, USD, 0.0d);
		ZeroRateSensitivity computed = test.zeroRatePointSensitivityWithSpread(DATE_VAL, USD, SPREAD, CONTINUOUS, 0);
		assertTrue(computed.compareKey(expected) == 0);
		assertEquals(computed.Sensitivity, expected.Sensitivity, TOLERANCE_DELTA_FD);
	  }

	  //-------------------------------------------------------------------------
	  //-------------------------------------------------------------------------
	  public virtual void test_parameterSensitivity()
	  {
		ZeroRatePeriodicDiscountFactors test = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double sensiValue = 25d;
		ZeroRateSensitivity point = test.zeroRatePointSensitivity(DATE_AFTER);
		point = point.multipliedBy(sensiValue);
		CurrencyParameterSensitivities sensiObject = test.parameterSensitivity(point);
		assertEquals(sensiObject.size(), 1);
		CurrencyParameterSensitivity sensi1 = sensiObject.Sensitivities.get(0);
		assertEquals(sensi1.Currency, GBP);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parameterSensitivity_full()
	  {
		ZeroRatePeriodicDiscountFactors test = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double sensiValue = 25d;
		ZeroRateSensitivity point = test.zeroRatePointSensitivity(DATE_AFTER);
		point = point.multipliedBy(sensiValue);
		CurrencyParameterSensitivities sensiObject = test.parameterSensitivity(point);
		assertEquals(sensiObject.Sensitivities.size(), 1);
		DoubleArray sensi0 = sensiObject.Sensitivities.get(0).Sensitivity;
		double shift = 1.0E-6;
		for (int i = 0; i < X.size(); i++)
		{
		  DoubleArray yP = Y.with(i, Y.get(i) + shift);
		  InterpolatedNodalCurve curveP = InterpolatedNodalCurve.of(META_ZERO_PERIODIC, X, yP, INTERPOLATOR);
		  double dfP = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, curveP).discountFactor(DATE_AFTER);
		  DoubleArray yM = Y.with(i, Y.get(i) - shift);
		  InterpolatedNodalCurve curveM = InterpolatedNodalCurve.of(META_ZERO_PERIODIC, X, yM, INTERPOLATOR);
		  double dfM = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, curveM).discountFactor(DATE_AFTER);
		  assertEquals(sensi0.get(i), sensiValue * (dfP - dfM) / (2 * shift), TOLERANCE_DELTA_FD);
		}
	  }

	  public virtual void test_parameterSensitivity_withSpread_full()
	  {
		int periodPerYear = 2;
		double spread = 0.0011; // 11 bp
		ZeroRatePeriodicDiscountFactors test = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, CURVE);
		double sensiValue = 25d;
		ZeroRateSensitivity point = test.zeroRatePointSensitivityWithSpread(DATE_AFTER, spread, PERIODIC, periodPerYear);
		point = point.multipliedBy(sensiValue);
		CurrencyParameterSensitivities sensiObject = test.parameterSensitivity(point);
		assertEquals(sensiObject.Sensitivities.size(), 1);
		DoubleArray sensi0 = sensiObject.Sensitivities.get(0).Sensitivity;
		double shift = 1.0E-6;
		for (int i = 0; i < X.size(); i++)
		{
		  DoubleArray yP = Y.with(i, Y.get(i) + shift);
		  InterpolatedNodalCurve curveP = InterpolatedNodalCurve.of(META_ZERO_PERIODIC, X, yP, INTERPOLATOR);
		  double dfP = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, curveP).discountFactorWithSpread(DATE_AFTER, spread, PERIODIC, periodPerYear);
		  DoubleArray yM = Y.with(i, Y.get(i) - shift);
		  InterpolatedNodalCurve curveM = InterpolatedNodalCurve.of(META_ZERO_PERIODIC, X, yM, INTERPOLATOR);
		  double dfM = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, curveM).discountFactorWithSpread(DATE_AFTER, spread, PERIODIC, periodPerYear);
		  assertEquals(sensi0.get(i), sensiValue * (dfP - dfM) / (2 * shift), TOLERANCE_DELTA_FD, "With spread - " + i);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createParameterSensitivity()
	  {
		ZeroRatePeriodicDiscountFactors test = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, CURVE);
		DoubleArray sensitivities = DoubleArray.of(0.12, 0.15, 0.16);
		CurrencyParameterSensitivities sens = test.createParameterSensitivity(USD, sensitivities);
		assertEquals(sens.Sensitivities.get(0), CURVE.createParameterSensitivity(USD, sensitivities));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withCurve()
	  {
		ZeroRatePeriodicDiscountFactors test = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, CURVE).withCurve(CURVE2);
		assertEquals(test.Curve, CURVE2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ZeroRatePeriodicDiscountFactors test = ZeroRatePeriodicDiscountFactors.of(GBP, DATE_VAL, CURVE);
		coverImmutableBean(test);
		ZeroRatePeriodicDiscountFactors test2 = ZeroRatePeriodicDiscountFactors.of(USD, DATE_VAL.plusDays(1), CURVE2);
		coverBeanEquals(test, test2);
	  }

	}

}