/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// Test <seealso cref="DiscountIborIndexRates"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountIborIndexRatesTest
	public class DiscountIborIndexRatesTest
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
	  private static readonly CurveMetadata METADATA = Curves.zeroRates(NAME, CURVE_DAY_COUNT);
	  private static readonly InterpolatedNodalCurve CURVE = InterpolatedNodalCurve.of(METADATA, DoubleArray.of(0, 10), DoubleArray.of(0.01, 0.02), INTERPOLATOR);
	  private static readonly InterpolatedNodalCurve CURVE2 = InterpolatedNodalCurve.of(METADATA, DoubleArray.of(0, 10), DoubleArray.of(0.01, 0.03), INTERPOLATOR);
	  private static readonly ZeroRateDiscountFactors DFCURVE = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE);
	  private static readonly ZeroRateDiscountFactors DFCURVE2 = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE2);

	  private const double RATE_BEFORE = 0.013d;
	  private const double RATE_VAL = 0.014d;
	  private static readonly LocalDateDoubleTimeSeries SERIES = LocalDateDoubleTimeSeries.builder().put(DATE_BEFORE, RATE_BEFORE).put(DATE_VAL, RATE_VAL).build();
	  private static readonly LocalDateDoubleTimeSeries SERIES_MINIMAL = LocalDateDoubleTimeSeries.of(DATE_VAL, RATE_VAL);
	  private static readonly LocalDateDoubleTimeSeries SERIES_EMPTY = LocalDateDoubleTimeSeries.empty();

	  private const double TOLERANCE_RATE = 1.0E-8;

	  //-------------------------------------------------------------------------
	  public virtual void test_of_withoutFixings()
	  {
		DiscountIborIndexRates test = DiscountIborIndexRates.of(GBP_LIBOR_3M, DFCURVE);
		assertEquals(test.Index, GBP_LIBOR_3M);
		assertEquals(test.ValuationDate, DATE_VAL);
		assertEquals(test.Fixings, SERIES_EMPTY);
		assertEquals(test.DiscountFactors, DFCURVE);
		assertEquals(test.DiscountFactors, DFCURVE);
		assertEquals(test.ParameterCount, DFCURVE.ParameterCount);
		assertEquals(test.getParameter(0), DFCURVE.getParameter(0));
		assertEquals(test.getParameterMetadata(0), DFCURVE.getParameterMetadata(0));
		assertEquals(test.withParameter(0, 1d).DiscountFactors, DFCURVE.withParameter(0, 1d));
		assertEquals(test.withPerturbation((i, v, m) => v + 1d).DiscountFactors, DFCURVE.withPerturbation((i, v, m) => v + 1d));
		assertEquals(test.findData(CURVE.Name), CURVE);
		assertEquals(test.findData(CurveName.of("Rubbish")), null);
		// check IborIndexRates
		IborIndexRates test2 = IborIndexRates.of(GBP_LIBOR_3M, DATE_VAL, CURVE);
		assertEquals(test, test2);
	  }

	  public virtual void test_of_withFixings()
	  {
		DiscountIborIndexRates test = DiscountIborIndexRates.of(GBP_LIBOR_3M, DFCURVE, SERIES);
		assertEquals(test.Index, GBP_LIBOR_3M);
		assertEquals(test.ValuationDate, DATE_VAL);
		assertEquals(test.Fixings, SERIES);
		assertEquals(test.DiscountFactors, DFCURVE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withDiscountFactors()
	  {
		DiscountIborIndexRates test = DiscountIborIndexRates.of(GBP_LIBOR_3M, DFCURVE, SERIES);
		test = test.withDiscountFactors(DFCURVE2);
		assertEquals(test.Index, GBP_LIBOR_3M);
		assertEquals(test.ValuationDate, DATE_VAL);
		assertEquals(test.Fixings, SERIES);
		assertEquals(test.DiscountFactors, DFCURVE2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_rate_beforeValuation_fixing()
	  {
		DiscountIborIndexRates test = DiscountIborIndexRates.of(GBP_LIBOR_3M, DFCURVE, SERIES);
		assertEquals(test.rate(GBP_LIBOR_3M_BEFORE), RATE_BEFORE);
	  }

	  public virtual void test_rate_beforeValuation_noFixing_emptySeries()
	  {
		DiscountIborIndexRates test = DiscountIborIndexRates.of(GBP_LIBOR_3M, DFCURVE, SERIES_EMPTY);
		assertThrowsIllegalArg(() => test.rate(GBP_LIBOR_3M_BEFORE));
	  }

	  public virtual void test_rate_beforeValuation_noFixing_notEmptySeries()
	  {
		DiscountIborIndexRates test = DiscountIborIndexRates.of(GBP_LIBOR_3M, DFCURVE, SERIES_MINIMAL);
		assertThrowsIllegalArg(() => test.rate(GBP_LIBOR_3M_BEFORE));
	  }

	  public virtual void test_rate_onValuation_fixing()
	  {
		DiscountIborIndexRates test = DiscountIborIndexRates.of(GBP_LIBOR_3M, DFCURVE, SERIES);
		assertEquals(test.rate(GBP_LIBOR_3M_VAL), RATE_VAL);
	  }

	  public virtual void test_rateIgnoringFixings_onValuation_fixing()
	  {
		DiscountIborIndexRates test = DiscountIborIndexRates.of(GBP_LIBOR_3M, DFCURVE, SERIES);
		LocalDate startDate = GBP_LIBOR_3M_VAL.EffectiveDate;
		LocalDate endDate = GBP_LIBOR_3M_VAL.MaturityDate;
		double accrualFactor = GBP_LIBOR_3M_VAL.YearFraction;
		double expected = (DFCURVE.discountFactor(startDate) / DFCURVE.discountFactor(endDate) - 1) / accrualFactor;
		assertEquals(test.rateIgnoringFixings(GBP_LIBOR_3M_VAL), expected, TOLERANCE_RATE);
	  }

	  public virtual void test_rate_onValuation_noFixing()
	  {
		DiscountIborIndexRates test = DiscountIborIndexRates.of(GBP_LIBOR_3M, DFCURVE, SERIES_EMPTY);
		LocalDate startDate = GBP_LIBOR_3M_VAL.EffectiveDate;
		LocalDate endDate = GBP_LIBOR_3M_VAL.MaturityDate;
		double accrualFactor = GBP_LIBOR_3M_VAL.YearFraction;
		double expected = (DFCURVE.discountFactor(startDate) / DFCURVE.discountFactor(endDate) - 1) / accrualFactor;
		assertEquals(test.rate(GBP_LIBOR_3M_VAL), expected, TOLERANCE_RATE);
		assertEquals(test.rateIgnoringFixings(GBP_LIBOR_3M_VAL), expected, TOLERANCE_RATE);
	  }

	  public virtual void test_rate_afterValuation()
	  {
		DiscountIborIndexRates test = DiscountIborIndexRates.of(GBP_LIBOR_3M, DFCURVE, SERIES);
		LocalDate startDate = GBP_LIBOR_3M_AFTER.EffectiveDate;
		LocalDate endDate = GBP_LIBOR_3M_AFTER.MaturityDate;
		double accrualFactor = GBP_LIBOR_3M_AFTER.YearFraction;
		double expected = (DFCURVE.discountFactor(startDate) / DFCURVE.discountFactor(endDate) - 1) / accrualFactor;
		assertEquals(test.rate(GBP_LIBOR_3M_AFTER), expected, TOLERANCE_RATE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_ratePointSensitivity_fixing()
	  {
		DiscountIborIndexRates test = DiscountIborIndexRates.of(GBP_LIBOR_3M, DFCURVE, SERIES);
		assertEquals(test.ratePointSensitivity(GBP_LIBOR_3M_BEFORE), PointSensitivityBuilder.none());
		assertEquals(test.ratePointSensitivity(GBP_LIBOR_3M_VAL), PointSensitivityBuilder.none());
	  }

	  public virtual void test_rateIgnoringFixingsPointSensitivity_onValuation()
	  {
		DiscountIborIndexRates test = DiscountIborIndexRates.of(GBP_LIBOR_3M, DFCURVE, SERIES);
		IborRateSensitivity expected = IborRateSensitivity.of(GBP_LIBOR_3M_VAL, 1d);
		assertEquals(test.rateIgnoringFixingsPointSensitivity(GBP_LIBOR_3M_VAL), expected);
	  }

	  public virtual void test_ratePointSensitivity_onValuation_noFixing()
	  {
		DiscountIborIndexRates test = DiscountIborIndexRates.of(GBP_LIBOR_3M, DFCURVE, SERIES_EMPTY);
		IborRateSensitivity expected = IborRateSensitivity.of(GBP_LIBOR_3M_VAL, 1d);
		assertEquals(test.ratePointSensitivity(GBP_LIBOR_3M_VAL), expected);
		assertEquals(test.rateIgnoringFixingsPointSensitivity(GBP_LIBOR_3M_VAL), expected);
	  }

	  public virtual void test_ratePointSensitivity_afterValuation()
	  {
		DiscountIborIndexRates test = DiscountIborIndexRates.of(GBP_LIBOR_3M, DFCURVE, SERIES);
		IborRateSensitivity expected = IborRateSensitivity.of(GBP_LIBOR_3M_AFTER, 1d);
		assertEquals(test.ratePointSensitivity(GBP_LIBOR_3M_AFTER), expected);
	  }

	  //-------------------------------------------------------------------------
	  // proper end-to-end tests are elsewhere
	  public virtual void test_parameterSensitivity()
	  {
		DiscountIborIndexRates test = DiscountIborIndexRates.of(GBP_LIBOR_3M, DFCURVE, SERIES);
		IborRateSensitivity point = IborRateSensitivity.of(GBP_LIBOR_3M_AFTER, GBP, 1d);
		assertEquals(test.parameterSensitivity(point).size(), 1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createParameterSensitivity()
	  {
		DiscountIborIndexRates test = DiscountIborIndexRates.of(GBP_LIBOR_3M, DFCURVE, SERIES);
		DoubleArray sensitivities = DoubleArray.of(0.12, 0.15);
		CurrencyParameterSensitivities sens = test.createParameterSensitivity(USD, sensitivities);
		assertEquals(sens.Sensitivities.get(0), CURVE.createParameterSensitivity(USD, sensitivities));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		DiscountIborIndexRates test = DiscountIborIndexRates.of(GBP_LIBOR_3M, DFCURVE, SERIES);
		coverImmutableBean(test);
		DiscountIborIndexRates test2 = DiscountIborIndexRates.of(USD_LIBOR_3M, DFCURVE2, SERIES_EMPTY);
		coverBeanEquals(test, test2);
	  }

	}

}