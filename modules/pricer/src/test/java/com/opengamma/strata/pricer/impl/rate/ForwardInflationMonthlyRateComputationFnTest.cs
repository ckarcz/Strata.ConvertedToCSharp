/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.rate
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.GB_RPIX;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using PriceIndexObservation = com.opengamma.strata.basics.index.PriceIndexObservation;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using InflationRateSensitivity = com.opengamma.strata.pricer.rate.InflationRateSensitivity;
	using InflationMonthlyRateComputation = com.opengamma.strata.product.rate.InflationMonthlyRateComputation;

	/// <summary>
	/// Test <seealso cref="ForwardInflationMonthlyRateComputationFn"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ForwardInflationMonthlyRateComputationFnTest
	public class ForwardInflationMonthlyRateComputationFnTest
	{

	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;
	  private static readonly LocalDate VAL_DATE = LocalDate.of(2014, 6, 10);

	  private static readonly LocalDate DUMMY_ACCRUAL_START_DATE = date(2015, 1, 4); // Accrual dates irrelevant for the rate
	  private static readonly LocalDate DUMMY_ACCRUAL_END_DATE = date(2016, 1, 5); // Accrual dates irrelevant for the rate
	  private static readonly YearMonth REFERENCE_START_MONTH = YearMonth.of(2014, 10);
	  private static readonly YearMonth REFERENCE_END_MONTH = YearMonth.of(2015, 10);
	  private const double RATE_START = 317.0;
	  private const double RATE_END = 344.0;

	  private const double EPS = 1.0e-12;
	  private const double EPS_FD = 1.0e-4;

	  //-------------------------------------------------------------------------
	  public virtual void test_rate()
	  {
		ImmutableRatesProvider prov = createProvider(RATE_START, RATE_END);

		InflationMonthlyRateComputation ro = InflationMonthlyRateComputation.of(GB_RPIX, REFERENCE_START_MONTH, REFERENCE_END_MONTH);
		ForwardInflationMonthlyRateComputationFn obsFn = ForwardInflationMonthlyRateComputationFn.DEFAULT;

		double rateExpected = RATE_END / RATE_START - 1.0;
		assertEquals(obsFn.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, prov), rateExpected, EPS);

		// explain
		ExplainMapBuilder builder = ExplainMap.builder();
		assertEquals(obsFn.explainRate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, prov, builder), rateExpected, EPS);

		ExplainMap built = builder.build();
		assertEquals(built.get(ExplainKey.OBSERVATIONS).Present, true);
		assertEquals(built.get(ExplainKey.OBSERVATIONS).get().size(), 2);
		ExplainMap explain0 = built.get(ExplainKey.OBSERVATIONS).get().get(0);
		assertEquals(explain0.get(ExplainKey.FIXING_DATE), REFERENCE_START_MONTH.atEndOfMonth());
		assertEquals(explain0.get(ExplainKey.INDEX), GB_RPIX);
		assertEquals(explain0.get(ExplainKey.INDEX_VALUE), RATE_START);
		ExplainMap explain1 = built.get(ExplainKey.OBSERVATIONS).get().get(1);
		assertEquals(explain1.get(ExplainKey.FIXING_DATE), REFERENCE_END_MONTH.atEndOfMonth());
		assertEquals(explain1.get(ExplainKey.INDEX), GB_RPIX);
		assertEquals(explain1.get(ExplainKey.INDEX_VALUE), RATE_END);
		assertEquals(built.get(ExplainKey.COMBINED_RATE).Value.doubleValue(), rateExpected, EPS);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_rateSensitivity()
	  {
		ImmutableRatesProvider prov = createProvider(RATE_START, RATE_END);
		ImmutableRatesProvider provStartUp = createProvider(RATE_START + EPS_FD, RATE_END);
		ImmutableRatesProvider provStartDw = createProvider(RATE_START - EPS_FD, RATE_END);
		ImmutableRatesProvider provEndUp = createProvider(RATE_START, RATE_END + EPS_FD);
		ImmutableRatesProvider provEndDw = createProvider(RATE_START, RATE_END - EPS_FD);

		InflationMonthlyRateComputation ro = InflationMonthlyRateComputation.of(GB_RPIX, REFERENCE_START_MONTH, REFERENCE_END_MONTH);
		ForwardInflationMonthlyRateComputationFn obsFn = ForwardInflationMonthlyRateComputationFn.DEFAULT;

		double rateSrtUp = obsFn.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, provStartUp);
		double rateSrtDw = obsFn.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, provStartDw);
		double rateEndUp = obsFn.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, provEndUp);
		double rateEndDw = obsFn.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, provEndDw);

		PointSensitivityBuilder sensiStr = InflationRateSensitivity.of(PriceIndexObservation.of(GB_RPIX, REFERENCE_START_MONTH), 0.5 * (rateSrtUp - rateSrtDw) / EPS_FD);
		PointSensitivityBuilder sensiEnd = InflationRateSensitivity.of(PriceIndexObservation.of(GB_RPIX, REFERENCE_END_MONTH), 0.5 * (rateEndUp - rateEndDw) / EPS_FD);
		PointSensitivityBuilder sensiExpected = sensiStr.combinedWith(sensiEnd);

		PointSensitivityBuilder sensiComputed = obsFn.rateSensitivity(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, prov);
		assertTrue(sensiComputed.build().normalized().equalWithTolerance(sensiExpected.build().normalized(), EPS_FD));
	  }

	  private ImmutableRatesProvider createProvider(double rateStart, double rateEnd)
	  {

		LocalDateDoubleTimeSeries timeSeries = LocalDateDoubleTimeSeries.of(VAL_DATE.with(lastDayOfMonth()), 300);
		InterpolatedNodalCurve curve = InterpolatedNodalCurve.of(Curves.prices("GB-RPIX"), DoubleArray.of(4, 16), DoubleArray.of(rateStart, rateEnd), INTERPOLATOR);
		return ImmutableRatesProvider.builder(VAL_DATE).priceIndexCurve(GB_RPIX, curve).timeSeries(GB_RPIX, timeSeries).build();
	  }

	}

}