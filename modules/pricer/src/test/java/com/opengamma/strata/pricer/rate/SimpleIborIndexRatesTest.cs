/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.rate
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using IborIndexObservation = com.opengamma.strata.basics.index.IborIndexObservation;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using DefaultCurveMetadata = com.opengamma.strata.market.curve.DefaultCurveMetadata;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// Tests <seealso cref="SimpleIborIndexRates"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SimpleIborIndexRatesTest
	public class SimpleIborIndexRatesTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate DATE_VAL = date(2015, 6, 4);
	  private static readonly LocalDate DATE_BEFORE = date(2015, 6, 3);
	  private static readonly LocalDate DATE_AFTER = date(2015, 7, 30);

	  private static readonly IborIndexObservation GBP_LIBOR_3M_VAL = IborIndexObservation.of(GBP_LIBOR_3M, DATE_VAL, REF_DATA);
	  private static readonly IborIndexObservation GBP_LIBOR_3M_BEFORE = IborIndexObservation.of(GBP_LIBOR_3M, DATE_BEFORE, REF_DATA);
	  private static readonly IborIndexObservation GBP_LIBOR_3M_AFTER = IborIndexObservation.of(GBP_LIBOR_3M, DATE_AFTER, REF_DATA);

	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;
	  private static readonly DayCount CURVE_DAY_COUNT = ACT_ACT_ISDA;
	  private static readonly CurveName NAME = CurveName.of("TestCurve");
	  private static readonly CurveMetadata METADATA = DefaultCurveMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.FORWARD_RATE).curveName(NAME).dayCount(CURVE_DAY_COUNT).build();

	  private static readonly InterpolatedNodalCurve CURVE = InterpolatedNodalCurve.of(METADATA, DoubleArray.of(0, 10), DoubleArray.of(1, 2), INTERPOLATOR);
	  private static readonly InterpolatedNodalCurve CURVE2 = InterpolatedNodalCurve.of(METADATA, DoubleArray.of(0, 10), DoubleArray.of(2, 3), INTERPOLATOR);

	  private const double RATE_BEFORE = 0.013d;
	  private const double RATE_VAL = 0.014d;
	  private static readonly LocalDateDoubleTimeSeries SERIES = LocalDateDoubleTimeSeries.builder().put(DATE_BEFORE, RATE_BEFORE).put(DATE_VAL, RATE_VAL).build();
	  private static readonly LocalDateDoubleTimeSeries SERIES_MINIMAL = LocalDateDoubleTimeSeries.of(DATE_VAL, RATE_VAL);
	  private static readonly LocalDateDoubleTimeSeries SERIES_EMPTY = LocalDateDoubleTimeSeries.empty();

	  private const double TOLERANCE_RATE = 1.0E-8;

	  //-------------------------------------------------------------------------
	  public virtual void test_of_withoutFixings()
	  {
		SimpleIborIndexRates test = SimpleIborIndexRates.of(GBP_LIBOR_3M, DATE_VAL, CURVE);
		assertEquals(test.Index, GBP_LIBOR_3M);
		assertEquals(test.ValuationDate, DATE_VAL);
		assertEquals(test.Fixings, SERIES_EMPTY);
		assertEquals(test.Curve, CURVE);
		assertEquals(test.ParameterCount, CURVE.ParameterCount);
		assertEquals(test.getParameter(0), CURVE.getParameter(0));
		assertEquals(test.getParameterMetadata(0), CURVE.getParameterMetadata(0));
		assertEquals(test.withParameter(0, 1d).Curve, CURVE.withParameter(0, 1d));
		assertEquals(test.withPerturbation((i, v, m) => v + 1d).Curve, CURVE.withPerturbation((i, v, m) => v + 1d));
		assertEquals(test.findData(CURVE.Name), CURVE);
		assertEquals(test.findData(CurveName.of("Rubbish")), null);
		// check IborIndexRates
		IborIndexRates test2 = IborIndexRates.of(GBP_LIBOR_3M, DATE_VAL, CURVE);
		assertEquals(test, test2);
	  }

	  public virtual void test_of_withFixings()
	  {
		SimpleIborIndexRates test = SimpleIborIndexRates.of(GBP_LIBOR_3M, DATE_VAL, CURVE, SERIES);
		assertEquals(test.Index, GBP_LIBOR_3M);
		assertEquals(test.ValuationDate, DATE_VAL);
		assertEquals(test.Fixings, SERIES);
		assertEquals(test.Curve, CURVE);
	  }

	  public virtual void test_of_badCurve()
	  {
		CurveMetadata noDayCountMetadata = DefaultCurveMetadata.builder().curveName(NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.FORWARD_RATE).build();
		InterpolatedNodalCurve notDayCount = InterpolatedNodalCurve.of(noDayCountMetadata, DoubleArray.of(0, 10), DoubleArray.of(1, 2), INTERPOLATOR);
		assertThrowsIllegalArg(() => SimpleIborIndexRates.of(GBP_LIBOR_3M, DATE_VAL, notDayCount));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withDiscountFactors()
	  {
		SimpleIborIndexRates test = SimpleIborIndexRates.of(GBP_LIBOR_3M, DATE_VAL, CURVE, SERIES);
		test = test.withCurve(CURVE2);
		assertEquals(test.Index, GBP_LIBOR_3M);
		assertEquals(test.ValuationDate, DATE_VAL);
		assertEquals(test.Fixings, SERIES);
		assertEquals(test.Curve, CURVE2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_rate_beforeValuation_fixing()
	  {
		SimpleIborIndexRates test = SimpleIborIndexRates.of(GBP_LIBOR_3M, DATE_VAL, CURVE, SERIES);
		assertEquals(test.rate(GBP_LIBOR_3M_BEFORE), RATE_BEFORE);
	  }

	  public virtual void test_rate_beforeValuation_noFixing_emptySeries()
	  {
		SimpleIborIndexRates test = SimpleIborIndexRates.of(GBP_LIBOR_3M, DATE_VAL, CURVE, SERIES_EMPTY);
		assertThrowsIllegalArg(() => test.rate(GBP_LIBOR_3M_BEFORE));
	  }

	  public virtual void test_rate_beforeValuation_noFixing_notEmptySeries()
	  {
		SimpleIborIndexRates test = SimpleIborIndexRates.of(GBP_LIBOR_3M, DATE_VAL, CURVE, SERIES_MINIMAL);
		assertThrowsIllegalArg(() => test.rate(GBP_LIBOR_3M_BEFORE));
	  }

	  public virtual void test_rate_onValuation_fixing()
	  {
		SimpleIborIndexRates test = SimpleIborIndexRates.of(GBP_LIBOR_3M, DATE_VAL, CURVE, SERIES);
		assertEquals(test.rate(GBP_LIBOR_3M_VAL), RATE_VAL);
	  }

	  public virtual void test_rateIgnoringFixings_onValuation_fixing()
	  {
		SimpleIborIndexRates test = SimpleIborIndexRates.of(GBP_LIBOR_3M, DATE_VAL, CURVE, SERIES);
		double time = CURVE_DAY_COUNT.yearFraction(DATE_VAL, GBP_LIBOR_3M_VAL.MaturityDate);
		double expected = CURVE.yValue(time);
		assertEquals(test.rateIgnoringFixings(GBP_LIBOR_3M_VAL), expected, TOLERANCE_RATE);
	  }

	  public virtual void test_rate_onValuation_noFixing()
	  {
		SimpleIborIndexRates test = SimpleIborIndexRates.of(GBP_LIBOR_3M, DATE_VAL, CURVE, SERIES_EMPTY);
		double time = CURVE_DAY_COUNT.yearFraction(DATE_VAL, GBP_LIBOR_3M_VAL.MaturityDate);
		double expected = CURVE.yValue(time);
		assertEquals(test.rate(GBP_LIBOR_3M_VAL), expected, TOLERANCE_RATE);
		assertEquals(test.rateIgnoringFixings(GBP_LIBOR_3M_VAL), expected, TOLERANCE_RATE);
	  }

	  public virtual void test_rate_afterValuation()
	  {
		SimpleIborIndexRates test = SimpleIborIndexRates.of(GBP_LIBOR_3M, DATE_VAL, CURVE, SERIES);
		double time = CURVE_DAY_COUNT.yearFraction(DATE_VAL, GBP_LIBOR_3M_AFTER.MaturityDate);
		double expected = CURVE.yValue(time);
		assertEquals(test.rate(GBP_LIBOR_3M_AFTER), expected, TOLERANCE_RATE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_ratePointSensitivity_fixing()
	  {
		SimpleIborIndexRates test = SimpleIborIndexRates.of(GBP_LIBOR_3M, DATE_VAL, CURVE, SERIES);
		assertEquals(test.ratePointSensitivity(GBP_LIBOR_3M_BEFORE), PointSensitivityBuilder.none());
		assertEquals(test.ratePointSensitivity(GBP_LIBOR_3M_VAL), PointSensitivityBuilder.none());
	  }

	  public virtual void test_rateIgnoringFixingsPointSensitivity_onValuation()
	  {
		SimpleIborIndexRates test = SimpleIborIndexRates.of(GBP_LIBOR_3M, DATE_VAL, CURVE, SERIES);
		IborRateSensitivity expected = IborRateSensitivity.of(GBP_LIBOR_3M_VAL, 1d);
		assertEquals(test.rateIgnoringFixingsPointSensitivity(GBP_LIBOR_3M_VAL), expected);
	  }

	  public virtual void test_ratePointSensitivity_onValuation_noFixing()
	  {
		SimpleIborIndexRates test = SimpleIborIndexRates.of(GBP_LIBOR_3M, DATE_VAL, CURVE, SERIES_EMPTY);
		IborRateSensitivity expected = IborRateSensitivity.of(GBP_LIBOR_3M_VAL, 1d);
		assertEquals(test.ratePointSensitivity(GBP_LIBOR_3M_VAL), expected);
		assertEquals(test.rateIgnoringFixingsPointSensitivity(GBP_LIBOR_3M_VAL), expected);
	  }

	  public virtual void test_ratePointSensitivity_afterValuation()
	  {
		SimpleIborIndexRates test = SimpleIborIndexRates.of(GBP_LIBOR_3M, DATE_VAL, CURVE, SERIES);
		IborRateSensitivity expected = IborRateSensitivity.of(GBP_LIBOR_3M_AFTER, 1d);
		assertEquals(test.ratePointSensitivity(GBP_LIBOR_3M_AFTER), expected);
	  }

	  //-------------------------------------------------------------------------
	  // proper end-to-end tests are elsewhere
	  public virtual void test_parameterSensitivity()
	  {
		SimpleIborIndexRates test = SimpleIborIndexRates.of(GBP_LIBOR_3M, DATE_VAL, CURVE, SERIES);
		IborRateSensitivity point = IborRateSensitivity.of(GBP_LIBOR_3M_AFTER, GBP, 1d);
		assertEquals(test.parameterSensitivity(point).size(), 1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createParameterSensitivity()
	  {
		SimpleIborIndexRates test = SimpleIborIndexRates.of(GBP_LIBOR_3M, DATE_VAL, CURVE, SERIES);
		DoubleArray sensitivities = DoubleArray.of(0.12, 0.15);
		CurrencyParameterSensitivities sens = test.createParameterSensitivity(USD, sensitivities);
		assertEquals(sens.Sensitivities.get(0), CURVE.createParameterSensitivity(USD, sensitivities));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		SimpleIborIndexRates test = SimpleIborIndexRates.of(GBP_LIBOR_3M, DATE_VAL, CURVE, SERIES);
		coverImmutableBean(test);
		SimpleIborIndexRates test2 = SimpleIborIndexRates.of(USD_LIBOR_3M, DATE_AFTER, CURVE2, SERIES_EMPTY);
		coverBeanEquals(test, test2);
	  }

	}

}