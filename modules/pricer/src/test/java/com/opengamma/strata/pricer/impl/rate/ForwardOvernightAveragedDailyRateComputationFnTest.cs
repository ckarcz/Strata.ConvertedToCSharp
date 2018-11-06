/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.rate
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using OvernightIndexObservation = com.opengamma.strata.basics.index.OvernightIndexObservation;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using LocalDateDoubleTimeSeriesBuilder = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeriesBuilder;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using OvernightAveragedDailyRateComputation = com.opengamma.strata.product.rate.OvernightAveragedDailyRateComputation;

	/// <summary>
	/// Test <seealso cref="ForwardOvernightAveragedDailyRateComputationFn"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ForwardOvernightAveragedDailyRateComputationFnTest
	public class ForwardOvernightAveragedDailyRateComputationFnTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate DUMMY_ACCRUAL_START_DATE = date(2015, 1, 1); // Accrual dates irrelevant for the rate
	  private static readonly LocalDate DUMMY_ACCRUAL_END_DATE = date(2015, 1, 1); // Accrual dates irrelevant for the rate

	  private static readonly DateTimeFormatter FMT = DateTimeFormatter.ISO_DATE;
	  private static readonly LocalDate[] FIXING_DATES = new LocalDate[] {LocalDate.parse("2018-05-01", FMT), LocalDate.parse("2018-04-30", FMT), LocalDate.parse("2018-04-27", FMT), LocalDate.parse("2018-04-26", FMT), LocalDate.parse("2018-04-25", FMT), LocalDate.parse("2018-04-24", FMT), LocalDate.parse("2018-04-23", FMT), LocalDate.parse("2018-04-20", FMT), LocalDate.parse("2018-04-19", FMT), LocalDate.parse("2018-04-18", FMT), LocalDate.parse("2018-04-17", FMT), LocalDate.parse("2018-04-16", FMT), LocalDate.parse("2018-04-13", FMT), LocalDate.parse("2018-04-12", FMT), LocalDate.parse("2018-04-11", FMT), LocalDate.parse("2018-04-10", FMT), LocalDate.parse("2018-04-09", FMT), LocalDate.parse("2018-04-06", FMT), LocalDate.parse("2018-04-05", FMT), LocalDate.parse("2018-04-04", FMT), LocalDate.parse("2018-04-03", FMT), LocalDate.parse("2018-04-02", FMT), LocalDate.parse("2018-03-30", FMT), LocalDate.parse("2018-03-29", FMT), LocalDate.parse("2018-03-28", FMT), LocalDate.parse("2018-03-27", FMT), LocalDate.parse("2018-03-26", FMT), LocalDate.parse("2018-03-23", FMT), LocalDate.parse("2018-03-22", FMT), LocalDate.parse("2018-03-21", FMT), LocalDate.parse("2018-03-20", FMT), LocalDate.parse("2018-03-19", FMT), LocalDate.parse("2018-03-16", FMT), LocalDate.parse("2018-03-15", FMT), LocalDate.parse("2018-03-14", FMT), LocalDate.parse("2018-03-13", FMT), LocalDate.parse("2018-03-12", FMT), LocalDate.parse("2018-03-09", FMT), LocalDate.parse("2018-03-08", FMT), LocalDate.parse("2018-03-07", FMT), LocalDate.parse("2018-03-06", FMT), LocalDate.parse("2018-03-05", FMT), LocalDate.parse("2018-03-02", FMT), LocalDate.parse("2018-03-01", FMT), LocalDate.parse("2018-02-28", FMT), LocalDate.parse("2018-02-27", FMT), LocalDate.parse("2018-02-26", FMT), LocalDate.parse("2018-02-23", FMT)};
	  private static readonly double[] FIXING_RATES = new double[] {0.017, 0.0169, 0.017, 0.017, 0.017, 0.017, 0.017, 0.017, 0.0169, 0.0169, 0.0169, 0.0169, 0.0169, 0.0169, 0.0169, 0.0169, 0.0169, 0.0169, 0.0169, 0.0169, 0.0169, 0.0168, 0.0167, 0.0168, 0.0168, 0.0168, 0.0168, 0.0168, 0.0168, 0.0144, 0.0144, 0.0143, 0.0143, 0.0143, 0.0142, 0.0142, 0.0142, 0.0142, 0.0142, 0.0142, 0.0142, 0.0142, 0.0142, 0.0142, 0.0135, 0.0142, 0.0142, 0.0142};
	  private static readonly LocalDateDoubleTimeSeries TIME_SERIES;
	  static ForwardOvernightAveragedDailyRateComputationFnTest()
	  {
		LocalDateDoubleTimeSeriesBuilder builder = LocalDateDoubleTimeSeries.builder();
		for (int i = 0; i < FIXING_DATES.Length; ++i)
		{
		  builder.put(FIXING_DATES[i], FIXING_RATES[i]);
		}
		TIME_SERIES = builder.build();
	  }
	  private static readonly DoubleArray TIME = DoubleArray.of(0.02, 0.08, 0.25);
	  private static readonly DoubleArray RATE = DoubleArray.of(0.01, 0.015, 0.008);
	  private static readonly Curve CURVE = InterpolatedNodalCurve.of(Curves.zeroRates("FED-FUND", DayCounts.ACT_365F), TIME, RATE, CurveInterpolators.LINEAR);

	  private static readonly ForwardOvernightAveragedDailyRateComputationFn FUNCTION = ForwardOvernightAveragedDailyRateComputationFn.DEFAULT;

	  private const double TOL = 1.0e-14;

	  public virtual void test_before()
	  {
		LocalDate startDate = date(2018, 2, 1);
		LocalDate endDate = date(2018, 2, 28);
		OvernightAveragedDailyRateComputation cmp = OvernightAveragedDailyRateComputation.of(USD_FED_FUND, startDate, endDate, REF_DATA);
		ImmutableRatesProvider rates = getRatesProvider(date(2018, 1, 24));
		double computedRate = FUNCTION.rate(cmp, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, rates);
		PointSensitivityBuilder sensiComputed = FUNCTION.rateSensitivity(cmp, startDate, endDate, rates);
		ExplainMapBuilder builder = ExplainMap.builder();
		double explainRate = FUNCTION.explainRate(cmp, startDate, endDate, rates, builder);
		double expectedRate = 0d;
		PointSensitivityBuilder sensiExpected = PointSensitivityBuilder.none();
		LocalDate date = startDate;
		while (!date.isAfter(endDate))
		{
		  OvernightIndexObservation obs = OvernightIndexObservation.of(USD_FED_FUND, date, REF_DATA);
		  double rate = rates.overnightIndexRates(USD_FED_FUND).rate(obs);
		  PointSensitivityBuilder rateSensi = rates.overnightIndexRates(USD_FED_FUND).ratePointSensitivity(obs);
		  LocalDate nextDate = cmp.FixingCalendar.next(date);
		  long days = DAYS.between(date, nextDate);
		  expectedRate += rate * days;
		  sensiExpected = sensiComputed.combinedWith(rateSensi.multipliedBy(days));
		  date = nextDate;
		}
		double nDays = 28d;
		expectedRate /= nDays;
		sensiExpected = sensiExpected.multipliedBy(1d / nDays);
		assertEquals(computedRate, expectedRate, TOL);
		assertTrue(sensiComputed.build().equalWithTolerance(sensiExpected.build(), TOL));
		assertEquals(explainRate, computedRate, TOL);
		assertEquals(builder.build().get(ExplainKey.COMBINED_RATE).Value, expectedRate, TOL);
	  }

	  public virtual void test_between()
	  {
		LocalDate startDate = date(2018, 3, 1);
		LocalDate endDate = date(2018, 3, 31);
		OvernightAveragedDailyRateComputation cmp = OvernightAveragedDailyRateComputation.of(USD_FED_FUND, startDate, endDate, REF_DATA);
		ImmutableRatesProvider rates = getRatesProvider(date(2018, 3, 14));
		double computedRate = FUNCTION.rate(cmp, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, rates);
		PointSensitivityBuilder sensiComputed = FUNCTION.rateSensitivity(cmp, startDate, endDate, rates);
		ExplainMapBuilder builder = ExplainMap.builder();
		double explainRate = FUNCTION.explainRate(cmp, startDate, endDate, rates, builder);
		double expectedRate = 0d;
		PointSensitivityBuilder sensiExpected = PointSensitivityBuilder.none();
		LocalDate date = startDate;
		while (!date.isAfter(endDate))
		{
		  OvernightIndexObservation obs = OvernightIndexObservation.of(USD_FED_FUND, date, REF_DATA);
		  double rate = rates.overnightIndexRates(USD_FED_FUND).rate(obs);
		  PointSensitivityBuilder rateSensi = rates.overnightIndexRates(USD_FED_FUND).ratePointSensitivity(obs);
		  LocalDate nextDate = cmp.FixingCalendar.next(date);
		  long days = nextDate.MonthValue != 4 ? DAYS.between(date, nextDate) : DAYS.between(date, endDate.plusDays(1));
		  expectedRate += rate * days;
		  sensiExpected = sensiComputed.combinedWith(rateSensi.multipliedBy(days));
		  date = nextDate;
		}
		double nDays = 31d;
		expectedRate /= nDays;
		sensiExpected = sensiExpected.multipliedBy(1d / nDays);
		assertEquals(computedRate, expectedRate, TOL);
		assertTrue(sensiComputed.build().equalWithTolerance(sensiExpected.build(), TOL));
		assertEquals(explainRate, computedRate, TOL);
		assertEquals(builder.build().get(ExplainKey.COMBINED_RATE).Value, expectedRate, TOL);
	  }

	  public virtual void test_after_regression()
	  {
		LocalDate startDate = date(2018, 3, 1);
		LocalDate endDate = date(2018, 3, 31);
		OvernightAveragedDailyRateComputation cmp = OvernightAveragedDailyRateComputation.of(USD_FED_FUND, startDate, endDate, REF_DATA);
		ImmutableRatesProvider rates = getRatesProvider(date(2018, 4, 28));
		double computed = FUNCTION.rate(cmp, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, rates);
		double expected = 0.0150612903225806;
		assertEquals(computed, expected, TOL);
		assertEquals(FUNCTION.rateSensitivity(cmp, startDate, endDate, rates), PointSensitivityBuilder.none());
	  }

	  private static ImmutableRatesProvider getRatesProvider(LocalDate valuationDate)
	  {
		return ImmutableRatesProvider.builder(valuationDate).indexCurve(USD_FED_FUND, CURVE).timeSeries(USD_FED_FUND, TIME_SERIES).build();
	  }

	}

}