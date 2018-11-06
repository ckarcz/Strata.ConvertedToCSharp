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
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.GBP_SONIA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
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
	using OvernightIndexObservation = com.opengamma.strata.basics.index.OvernightIndexObservation;
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
	/// Test <seealso cref="DiscountOvernightIndexRates"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountOvernightIndexRatesTest
	public class DiscountOvernightIndexRatesTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate DATE_VAL = date(2015, 6, 3);
	  private static readonly LocalDate DATE_BEFORE = date(2015, 6, 2);
	  private static readonly LocalDate DATE_AFTER = date(2015, 7, 30);
	  private static readonly LocalDate DATE_AFTER_END = date(2015, 7, 31);

	  private static readonly OvernightIndexObservation GBP_SONIA_VAL = OvernightIndexObservation.of(GBP_SONIA, DATE_VAL, REF_DATA);
	  private static readonly OvernightIndexObservation GBP_SONIA_BEFORE = OvernightIndexObservation.of(GBP_SONIA, DATE_BEFORE, REF_DATA);
	  private static readonly OvernightIndexObservation USD_FEDFUND_BEFORE = OvernightIndexObservation.of(USD_FED_FUND, DATE_BEFORE, REF_DATA);
	  private static readonly OvernightIndexObservation GBP_SONIA_AFTER = OvernightIndexObservation.of(GBP_SONIA, DATE_AFTER, REF_DATA);
	  private static readonly OvernightIndexObservation GBP_SONIA_AFTER_END = OvernightIndexObservation.of(GBP_SONIA, DATE_AFTER_END, REF_DATA);

	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;
	  private static readonly CurveName NAME = CurveName.of("TestCurve");
	  private static readonly CurveMetadata METADATA = Curves.zeroRates(NAME, ACT_365F);
	  private static readonly InterpolatedNodalCurve CURVE = InterpolatedNodalCurve.of(METADATA, DoubleArray.of(0, 10), DoubleArray.of(0.01, 0.02), INTERPOLATOR);
	  private static readonly InterpolatedNodalCurve CURVE2 = InterpolatedNodalCurve.of(METADATA, DoubleArray.of(0, 10), DoubleArray.of(0.01, 0.03), INTERPOLATOR);
	  private static readonly ZeroRateDiscountFactors DFCURVE = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE);
	  private static readonly ZeroRateDiscountFactors DFCURVE2 = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE2);

	  private const double RATE_BEFORE = 0.013d;
	  private const double RATE_VAL = 0.014d;
	  private static readonly LocalDateDoubleTimeSeries SERIES = LocalDateDoubleTimeSeries.builder().put(DATE_BEFORE, RATE_BEFORE).put(DATE_VAL, RATE_VAL).build();
	  private static readonly LocalDateDoubleTimeSeries SERIES_MINIMAL = LocalDateDoubleTimeSeries.of(DATE_VAL, RATE_VAL);
	  private static readonly LocalDateDoubleTimeSeries SERIES_EMPTY = LocalDateDoubleTimeSeries.empty();

	  //-------------------------------------------------------------------------
	  public virtual void test_of_withoutFixings()
	  {
		DiscountOvernightIndexRates test = DiscountOvernightIndexRates.of(GBP_SONIA, DFCURVE);
		assertEquals(test.Index, GBP_SONIA);
		assertEquals(test.ValuationDate, DATE_VAL);
		assertEquals(test.Fixings, SERIES_EMPTY);
		assertEquals(test.DiscountFactors, DFCURVE);
		assertEquals(test.ParameterCount, DFCURVE.ParameterCount);
		assertEquals(test.getParameter(0), DFCURVE.getParameter(0));
		assertEquals(test.getParameterMetadata(0), DFCURVE.getParameterMetadata(0));
		assertEquals(test.withParameter(0, 1d).DiscountFactors, DFCURVE.withParameter(0, 1d));
		assertEquals(test.withPerturbation((i, v, m) => v + 1d).DiscountFactors, DFCURVE.withPerturbation((i, v, m) => v + 1d));
		assertEquals(test.findData(CURVE.Name), CURVE);
		assertEquals(test.findData(CurveName.of("Rubbish")), null);
		// check IborIndexRates
		OvernightIndexRates test2 = OvernightIndexRates.of(GBP_SONIA, DATE_VAL, CURVE);
		assertEquals(test, test2);
	  }

	  public virtual void test_of_withFixings()
	  {
		DiscountOvernightIndexRates test = DiscountOvernightIndexRates.of(GBP_SONIA, DFCURVE, SERIES);
		assertEquals(test.Index, GBP_SONIA);
		assertEquals(test.ValuationDate, DATE_VAL);
		assertEquals(test.Fixings, SERIES);
		assertEquals(test.DiscountFactors, DFCURVE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withDiscountFactors()
	  {
		DiscountOvernightIndexRates test = DiscountOvernightIndexRates.of(GBP_SONIA, DFCURVE, SERIES);
		test = test.withDiscountFactors(DFCURVE2);
		assertEquals(test.Index, GBP_SONIA);
		assertEquals(test.ValuationDate, DATE_VAL);
		assertEquals(test.Fixings, SERIES);
		assertEquals(test.DiscountFactors, DFCURVE2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_rate_beforeValuation_fixing()
	  {
		DiscountOvernightIndexRates test = DiscountOvernightIndexRates.of(GBP_SONIA, DFCURVE, SERIES);
		assertEquals(test.rate(GBP_SONIA_BEFORE), RATE_BEFORE);
	  }

	  public virtual void test_rate_beforeValuation_noFixing_emptySeries()
	  {
		DiscountOvernightIndexRates test = DiscountOvernightIndexRates.of(GBP_SONIA, DFCURVE, SERIES_EMPTY);
		assertThrowsIllegalArg(() => test.rate(GBP_SONIA_BEFORE));
	  }

	  public virtual void test_rate_beforeValuation_noFixing_notEmptySeries()
	  {
		DiscountOvernightIndexRates test = DiscountOvernightIndexRates.of(GBP_SONIA, DFCURVE, SERIES_MINIMAL);
		assertThrowsIllegalArg(() => test.rate(GBP_SONIA_BEFORE));
	  }

	  public virtual void test_rate_onValuation_fixing()
	  {
		DiscountOvernightIndexRates test = DiscountOvernightIndexRates.of(GBP_SONIA, DFCURVE, SERIES);
		assertEquals(test.rate(GBP_SONIA_VAL), RATE_VAL);
	  }

	  public virtual void test_rateIgnoringFixings_onValuation_fixing()
	  {
		DiscountOvernightIndexRates test = DiscountOvernightIndexRates.of(GBP_SONIA, DFCURVE, SERIES);
		LocalDate startDate = GBP_SONIA_VAL.EffectiveDate;
		LocalDate endDate = GBP_SONIA_VAL.MaturityDate;
		double accrualFactor = GBP_SONIA_VAL.YearFraction;
		double expected = (DFCURVE.discountFactor(startDate) / DFCURVE.discountFactor(endDate) - 1) / accrualFactor;
		assertEquals(test.rateIgnoringFixings(GBP_SONIA_VAL), expected, 1e-8);
	  }

	  public virtual void test_rate_onPublication_noFixing()
	  {
		DiscountOvernightIndexRates test = DiscountOvernightIndexRates.of(GBP_SONIA, DFCURVE, SERIES_EMPTY);
		LocalDate startDate = GBP_SONIA_VAL.EffectiveDate;
		LocalDate endDate = GBP_SONIA_VAL.MaturityDate;
		double accrualFactor = GBP_SONIA.DayCount.yearFraction(startDate, endDate);
		double expected = (DFCURVE.discountFactor(startDate) / DFCURVE.discountFactor(endDate) - 1) / accrualFactor;
		assertEquals(test.rate(GBP_SONIA_VAL), expected, 1e-4);
	  }

	  public virtual void test_rate_afterPublication()
	  {
		DiscountOvernightIndexRates test = DiscountOvernightIndexRates.of(GBP_SONIA, DFCURVE, SERIES);
		LocalDate startDate = GBP_SONIA_AFTER.EffectiveDate;
		LocalDate endDate = GBP_SONIA_AFTER.MaturityDate;
		double accrualFactor = GBP_SONIA.DayCount.yearFraction(startDate, endDate);
		double expected = (DFCURVE.discountFactor(startDate) / DFCURVE.discountFactor(endDate) - 1) / accrualFactor;
		assertEquals(test.rate(GBP_SONIA_AFTER), expected, 1e-8);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_ratePointSensitivity_fixing()
	  {
		DiscountOvernightIndexRates test = DiscountOvernightIndexRates.of(GBP_SONIA, DFCURVE, SERIES);
		assertEquals(test.ratePointSensitivity(GBP_SONIA_BEFORE), PointSensitivityBuilder.none());
		assertEquals(test.ratePointSensitivity(GBP_SONIA_VAL), PointSensitivityBuilder.none());
	  }

	  public virtual void test_rateIgnoringFixingsPointSensitivity_onValuation()
	  {
		DiscountOvernightIndexRates test = DiscountOvernightIndexRates.of(GBP_SONIA, DFCURVE, SERIES);
		OvernightRateSensitivity expected = OvernightRateSensitivity.of(GBP_SONIA_VAL, 1d);
		assertEquals(test.rateIgnoringFixingsPointSensitivity(GBP_SONIA_VAL), expected);
	  }

	  public virtual void test_ratePointSensitivity_onPublication_noFixing()
	  {
		DiscountOvernightIndexRates test = DiscountOvernightIndexRates.of(GBP_SONIA, DFCURVE, SERIES_EMPTY);
		OvernightRateSensitivity expected = OvernightRateSensitivity.of(GBP_SONIA_VAL, 1d);
		assertEquals(test.ratePointSensitivity(GBP_SONIA_VAL), expected);
	  }

	  public virtual void test_ratePointSensitivity_afterPublication()
	  {
		DiscountOvernightIndexRates test = DiscountOvernightIndexRates.of(GBP_SONIA, DFCURVE, SERIES);
		OvernightRateSensitivity expected = OvernightRateSensitivity.of(GBP_SONIA_AFTER, 1d);
		assertEquals(test.ratePointSensitivity(GBP_SONIA_AFTER), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_periodRate()
	  {
		DiscountOvernightIndexRates test = DiscountOvernightIndexRates.of(GBP_SONIA, DFCURVE, SERIES);
		double accrualFactor = GBP_SONIA.DayCount.yearFraction(DATE_AFTER, DATE_AFTER_END);
		double expected = (DFCURVE.discountFactor(DATE_AFTER) / DFCURVE.discountFactor(DATE_AFTER_END) - 1) / accrualFactor;
		assertEquals(test.periodRate(GBP_SONIA_AFTER, DATE_AFTER_END), expected, 1e-8);
	  }

	  // This type of "forward" for the day before is required when the publication offset is 1.
	  // The fixing for the previous day will still be unknown at the beginning of the day and need to be computed from the curve.
	  public virtual void test_periodRate_publication_1()
	  {
		DiscountOvernightIndexRates test = DiscountOvernightIndexRates.of(USD_FED_FUND, DFCURVE, SERIES);
		double accrualFactor = USD_FED_FUND.DayCount.yearFraction(DATE_BEFORE, DATE_VAL);
		double expected = (DFCURVE.discountFactor(DATE_BEFORE) / DFCURVE.discountFactor(DATE_VAL) - 1) / accrualFactor;
		assertEquals(test.periodRate(USD_FEDFUND_BEFORE, DATE_VAL), expected, 1e-8);
	  }

	  public virtual void test_periodRate_badDates()
	  {
		DiscountOvernightIndexRates test = DiscountOvernightIndexRates.of(GBP_SONIA, DFCURVE, SERIES);
		assertThrowsIllegalArg(() => test.periodRate(GBP_SONIA_AFTER_END, DATE_AFTER));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_periodRatePointSensitivity()
	  {
		DiscountOvernightIndexRates test = DiscountOvernightIndexRates.of(GBP_SONIA, DFCURVE, SERIES);
		OvernightRateSensitivity expected = OvernightRateSensitivity.ofPeriod(GBP_SONIA_AFTER, DATE_AFTER_END, GBP, 1d);
		assertEquals(test.periodRatePointSensitivity(GBP_SONIA_AFTER, DATE_AFTER_END), expected);
	  }

	  public virtual void test_periodRatePointSensitivity_onholidaybeforepublication()
	  {
		LocalDate lastFixingDate = LocalDate.of(2017, 6, 30);
		LocalDate gbdBeforeValDate = LocalDate.of(2017, 7, 3);
		LocalDate gbdAfterValDate = LocalDate.of(2017, 7, 5);
		double fixingValue = 0.0010;
		InterpolatedNodalCurve curve = InterpolatedNodalCurve.of(METADATA, DoubleArray.of(-1.0d, 10.0d), DoubleArray.of(0.01, 0.02), INTERPOLATOR);
		ZeroRateDiscountFactors df = ZeroRateDiscountFactors.of(USD, LocalDate.of(2017, 7, 4), curve);
		LocalDateDoubleTimeSeries series = LocalDateDoubleTimeSeries.builder().put(lastFixingDate, fixingValue).build();
		DiscountOvernightIndexRates test = DiscountOvernightIndexRates.of(USD_FED_FUND, df, series);
		OvernightIndexObservation obs = OvernightIndexObservation.of(USD_FED_FUND, gbdBeforeValDate, REF_DATA);
		OvernightRateSensitivity expected = OvernightRateSensitivity.ofPeriod(obs, gbdAfterValDate, USD, 1d);
		assertEquals(test.periodRatePointSensitivity(obs, gbdAfterValDate), expected);
	  }

	  public virtual void test_periodRatePointSensitivity_badDates()
	  {
		DiscountOvernightIndexRates test = DiscountOvernightIndexRates.of(GBP_SONIA, DFCURVE, SERIES);
		assertThrowsIllegalArg(() => test.periodRatePointSensitivity(GBP_SONIA_AFTER_END, DATE_AFTER));
	  }

	  //-------------------------------------------------------------------------
	  // proper end-to-end tests are elsewhere
	  public virtual void test_parameterSensitivity()
	  {
		DiscountOvernightIndexRates test = DiscountOvernightIndexRates.of(GBP_SONIA, DFCURVE, SERIES);
		OvernightRateSensitivity point = OvernightRateSensitivity.ofPeriod(GBP_SONIA_AFTER, DATE_AFTER_END, GBP, 1d);
		assertEquals(test.parameterSensitivity(point).size(), 1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createParameterSensitivity()
	  {
		DiscountOvernightIndexRates test = DiscountOvernightIndexRates.of(GBP_SONIA, DFCURVE, SERIES);
		DoubleArray sensitivities = DoubleArray.of(0.12, 0.15);
		CurrencyParameterSensitivities sens = test.createParameterSensitivity(USD, sensitivities);
		assertEquals(sens.Sensitivities.get(0), CURVE.createParameterSensitivity(USD, sensitivities));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		DiscountOvernightIndexRates test = DiscountOvernightIndexRates.of(GBP_SONIA, DFCURVE, SERIES);
		coverImmutableBean(test);
		DiscountOvernightIndexRates test2 = DiscountOvernightIndexRates.of(USD_FED_FUND, DFCURVE2, SERIES_EMPTY);
		coverBeanEquals(test, test2);
	  }

	}

}