/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.rate
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.mock;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.when;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using IborIndexObservation = com.opengamma.strata.basics.index.IborIndexObservation;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using IborIndexRates = com.opengamma.strata.pricer.rate.IborIndexRates;
	using IborRateSensitivity = com.opengamma.strata.pricer.rate.IborRateSensitivity;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using IborInterpolatedRateComputation = com.opengamma.strata.product.rate.IborInterpolatedRateComputation;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ForwardIborInterpolatedRateComputationFnTest
	public class ForwardIborInterpolatedRateComputationFnTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate FIXING_DATE = date(2014, 6, 30);
	  private static readonly LocalDate ACCRUAL_START_DATE = date(2014, 7, 2);
	  private static readonly LocalDate ACCRUAL_END_DATE = date(2014, 11, 2);
	  private const double RATE3 = 0.0125d;
	  private const double RATE3TS = 0.0123d;
	  private const double RATE6 = 0.0234d;
	  private static readonly IborIndexObservation GBP_LIBOR_3M_OBS = IborIndexObservation.of(GBP_LIBOR_3M, FIXING_DATE, REF_DATA);
	  private static readonly IborIndexObservation GBP_LIBOR_6M_OBS = IborIndexObservation.of(GBP_LIBOR_6M, FIXING_DATE, REF_DATA);
	  private static readonly IborRateSensitivity SENSITIVITY3 = IborRateSensitivity.of(GBP_LIBOR_3M_OBS, 1d);
	  private static readonly IborRateSensitivity SENSITIVITY6 = IborRateSensitivity.of(GBP_LIBOR_6M_OBS, 1d);
	  private const double TOLERANCE_RATE = 1.0E-10;

	  public virtual void test_rate()
	  {
		RatesProvider mockProv = mock(typeof(RatesProvider));
		LocalDateDoubleTimeSeries timeSeries = LocalDateDoubleTimeSeries.of(FIXING_DATE, RATE3TS);
		IborIndexRates mockRates3M = new TestingIborIndexRates(GBP_LIBOR_3M, FIXING_DATE, LocalDateDoubleTimeSeries.empty(), timeSeries);
		IborIndexRates mockRates6M = new TestingIborIndexRates(GBP_LIBOR_6M, FIXING_DATE, LocalDateDoubleTimeSeries.of(FIXING_DATE, RATE6), LocalDateDoubleTimeSeries.empty());
		when(mockProv.iborIndexRates(GBP_LIBOR_3M)).thenReturn(mockRates3M);
		when(mockProv.iborIndexRates(GBP_LIBOR_6M)).thenReturn(mockRates6M);

		IborInterpolatedRateComputation ro = IborInterpolatedRateComputation.of(GBP_LIBOR_3M, GBP_LIBOR_6M, FIXING_DATE, REF_DATA);
		ForwardIborInterpolatedRateComputationFn obs = ForwardIborInterpolatedRateComputationFn.DEFAULT;
		LocalDate fixingEndDate3M = GBP_LIBOR_3M_OBS.MaturityDate;
		LocalDate fixingEndDate6M = GBP_LIBOR_6M_OBS.MaturityDate;
		double days3M = fixingEndDate3M.toEpochDay() - FIXING_DATE.toEpochDay(); //nb days in 3M fixing period
		double days6M = fixingEndDate6M.toEpochDay() - FIXING_DATE.toEpochDay(); //nb days in 6M fixing period
		double daysCpn = ACCRUAL_END_DATE.toEpochDay() - FIXING_DATE.toEpochDay();
		double weight3M = (days6M - daysCpn) / (days6M - days3M);
		double weight6M = (daysCpn - days3M) / (days6M - days3M);
		double rateExpected = (weight3M * RATE3TS + weight6M * RATE6);
		double rateComputed = obs.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, mockProv);
		assertEquals(rateComputed, rateExpected, TOLERANCE_RATE);

		// explain
		ExplainMapBuilder builder = ExplainMap.builder();
		assertEquals(obs.explainRate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, mockProv, builder), rateExpected, TOLERANCE_RATE);

		ExplainMap built = builder.build();
		assertEquals(built.get(ExplainKey.OBSERVATIONS).Present, true);
		assertEquals(built.get(ExplainKey.OBSERVATIONS).get().size(), 2);
		assertEquals(built.get(ExplainKey.OBSERVATIONS).get().get(0).get(ExplainKey.FIXING_DATE), FIXING_DATE);
		assertEquals(built.get(ExplainKey.OBSERVATIONS).get().get(0).get(ExplainKey.INDEX), GBP_LIBOR_3M);
		assertEquals(built.get(ExplainKey.OBSERVATIONS).get().get(0).get(ExplainKey.INDEX_VALUE), RATE3TS);
		assertEquals(built.get(ExplainKey.OBSERVATIONS).get().get(0).get(ExplainKey.WEIGHT), weight3M);
		assertEquals(built.get(ExplainKey.OBSERVATIONS).get().get(0).get(ExplainKey.FROM_FIXING_SERIES), true);
		assertEquals(built.get(ExplainKey.OBSERVATIONS).get().get(1).get(ExplainKey.FIXING_DATE), FIXING_DATE);
		assertEquals(built.get(ExplainKey.OBSERVATIONS).get().get(1).get(ExplainKey.INDEX), GBP_LIBOR_6M);
		assertEquals(built.get(ExplainKey.OBSERVATIONS).get().get(1).get(ExplainKey.INDEX_VALUE), RATE6);
		assertEquals(built.get(ExplainKey.OBSERVATIONS).get().get(1).get(ExplainKey.WEIGHT), weight6M);
		assertEquals(built.get(ExplainKey.OBSERVATIONS).get().get(1).get(ExplainKey.FROM_FIXING_SERIES), null);
		assertEquals(built.get(ExplainKey.COMBINED_RATE), rateExpected);
	  }

	  public virtual void test_rateSensitivity()
	  {
		RatesProvider mockProv = mock(typeof(RatesProvider));
		IborIndexRates mockRates3M = mock(typeof(IborIndexRates));
		IborIndexRates mockRates6M = mock(typeof(IborIndexRates));
		when(mockProv.iborIndexRates(GBP_LIBOR_3M)).thenReturn(mockRates3M);
		when(mockProv.iborIndexRates(GBP_LIBOR_6M)).thenReturn(mockRates6M);
		when(mockRates3M.ratePointSensitivity(GBP_LIBOR_3M_OBS)).thenReturn(SENSITIVITY3);
		when(mockRates6M.ratePointSensitivity(GBP_LIBOR_6M_OBS)).thenReturn(SENSITIVITY6);

		IborInterpolatedRateComputation ro = IborInterpolatedRateComputation.of(GBP_LIBOR_3M, GBP_LIBOR_6M, FIXING_DATE, REF_DATA);
		ForwardIborInterpolatedRateComputationFn obsFn = ForwardIborInterpolatedRateComputationFn.DEFAULT;
		LocalDate fixingEndDate3M = GBP_LIBOR_3M_OBS.MaturityDate;
		LocalDate fixingEndDate6M = GBP_LIBOR_6M_OBS.MaturityDate;
		double days3M = fixingEndDate3M.toEpochDay() - FIXING_DATE.toEpochDay(); //nb days in 3M fixing period
		double days6M = fixingEndDate6M.toEpochDay() - FIXING_DATE.toEpochDay(); //nb days in 6M fixing period
		double daysCpn = ACCRUAL_END_DATE.toEpochDay() - FIXING_DATE.toEpochDay();
		double weight3M = (days6M - daysCpn) / (days6M - days3M);
		double weight6M = (daysCpn - days3M) / (days6M - days3M);
		IborRateSensitivity sens3 = IborRateSensitivity.of(GBP_LIBOR_3M_OBS, weight3M);
		IborRateSensitivity sens6 = IborRateSensitivity.of(GBP_LIBOR_6M_OBS, weight6M);
		PointSensitivities expected = PointSensitivities.of(ImmutableList.of(sens3, sens6));
		PointSensitivityBuilder test = obsFn.rateSensitivity(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, mockProv);
		assertEquals(test.build(), expected);
	  }

	  public virtual void test_rateSensitivity_finiteDifference()
	  {
		double eps = 1.0e-7;
		RatesProvider mockProv = mock(typeof(RatesProvider));
		IborIndexRates mockRates3M = mock(typeof(IborIndexRates));
		IborIndexRates mockRates6M = mock(typeof(IborIndexRates));
		when(mockProv.iborIndexRates(GBP_LIBOR_3M)).thenReturn(mockRates3M);
		when(mockProv.iborIndexRates(GBP_LIBOR_6M)).thenReturn(mockRates6M);
		when(mockRates3M.rate(GBP_LIBOR_3M_OBS)).thenReturn(RATE3);
		when(mockRates6M.rate(GBP_LIBOR_6M_OBS)).thenReturn(RATE6);
		when(mockRates3M.ratePointSensitivity(GBP_LIBOR_3M_OBS)).thenReturn(SENSITIVITY3);
		when(mockRates6M.ratePointSensitivity(GBP_LIBOR_6M_OBS)).thenReturn(SENSITIVITY6);

		IborInterpolatedRateComputation ro = IborInterpolatedRateComputation.of(GBP_LIBOR_3M, GBP_LIBOR_6M, FIXING_DATE, REF_DATA);
		ForwardIborInterpolatedRateComputationFn obs = ForwardIborInterpolatedRateComputationFn.DEFAULT;
		PointSensitivityBuilder test = obs.rateSensitivity(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, mockProv);

		IborIndexRates mockRatesUp3M = mock(typeof(IborIndexRates));
		when(mockRatesUp3M.rate(GBP_LIBOR_3M_OBS)).thenReturn(RATE3 + eps);
		IborIndexRates mockRatesDw3M = mock(typeof(IborIndexRates));
		when(mockRatesDw3M.rate(GBP_LIBOR_3M_OBS)).thenReturn(RATE3 - eps);
		IborIndexRates mockRatesUp6M = mock(typeof(IborIndexRates));
		when(mockRatesUp6M.rate(GBP_LIBOR_6M_OBS)).thenReturn(RATE6 + eps);
		IborIndexRates mockRatesDw6M = mock(typeof(IborIndexRates));
		when(mockRatesDw6M.rate(GBP_LIBOR_6M_OBS)).thenReturn(RATE6 - eps);

		RatesProvider mockProvUp3M = mock(typeof(RatesProvider));
		when(mockProvUp3M.iborIndexRates(GBP_LIBOR_3M)).thenReturn(mockRatesUp3M);
		when(mockProvUp3M.iborIndexRates(GBP_LIBOR_6M)).thenReturn(mockRates6M);
		RatesProvider mockProvDw3M = mock(typeof(RatesProvider));
		when(mockProvDw3M.iborIndexRates(GBP_LIBOR_3M)).thenReturn(mockRatesDw3M);
		when(mockProvDw3M.iborIndexRates(GBP_LIBOR_6M)).thenReturn(mockRates6M);
		RatesProvider mockProvUp6M = mock(typeof(RatesProvider));
		when(mockProvUp6M.iborIndexRates(GBP_LIBOR_3M)).thenReturn(mockRates3M);
		when(mockProvUp6M.iborIndexRates(GBP_LIBOR_6M)).thenReturn(mockRatesUp6M);
		RatesProvider mockProvDw6M = mock(typeof(RatesProvider));
		when(mockProvDw6M.iborIndexRates(GBP_LIBOR_3M)).thenReturn(mockRates3M);
		when(mockProvDw6M.iborIndexRates(GBP_LIBOR_6M)).thenReturn(mockRatesDw6M);

		double rateUp3M = obs.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, mockProvUp3M);
		double rateDw3M = obs.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, mockProvDw3M);
		double senseExpected3M = 0.5 * (rateUp3M - rateDw3M) / eps;

		double rateUp6M = obs.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, mockProvUp6M);
		double rateDw6M = obs.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, mockProvDw6M);
		double senseExpected6M = 0.5 * (rateUp6M - rateDw6M) / eps;

		assertEquals(test.build().Sensitivities.get(0).Sensitivity, senseExpected3M, eps);
		assertEquals(test.build().Sensitivities.get(1).Sensitivity, senseExpected6M, eps);
	  }

	}

}