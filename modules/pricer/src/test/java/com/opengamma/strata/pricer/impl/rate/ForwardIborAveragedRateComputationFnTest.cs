using System.Collections.Generic;

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
	using SimpleRatesProvider = com.opengamma.strata.pricer.rate.SimpleRatesProvider;
	using IborAveragedFixing = com.opengamma.strata.product.rate.IborAveragedFixing;
	using IborAveragedRateComputation = com.opengamma.strata.product.rate.IborAveragedRateComputation;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ForwardIborAveragedRateComputationFnTest
	public class ForwardIborAveragedRateComputationFnTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly IborIndexObservation[] OBSERVATIONS = new IborIndexObservation[] {IborIndexObservation.of(GBP_LIBOR_3M, date(2014, 6, 30), REF_DATA), IborIndexObservation.of(GBP_LIBOR_3M, date(2014, 7, 7), REF_DATA), IborIndexObservation.of(GBP_LIBOR_3M, date(2014, 7, 14), REF_DATA), IborIndexObservation.of(GBP_LIBOR_3M, date(2014, 7, 21), REF_DATA)};
	  private static readonly double[] FIXING_VALUES = new double[] {0.0123d, 0.0234d, 0.0345d, 0.0456d};
	  private static readonly double[] WEIGHTS = new double[] {0.10d, 0.20d, 0.30d, 0.40d};
	  private static readonly IborRateSensitivity[] SENSITIVITIES = new IborRateSensitivity[] {IborRateSensitivity.of(OBSERVATIONS[0], 1d), IborRateSensitivity.of(OBSERVATIONS[1], 1d), IborRateSensitivity.of(OBSERVATIONS[2], 1d), IborRateSensitivity.of(OBSERVATIONS[3], 1d)};

	  private static readonly LocalDate ACCRUAL_START_DATE = date(2014, 7, 2);
	  private static readonly LocalDate ACCRUAL_END_DATE = date(2014, 11, 2);
	  private const double TOLERANCE_RATE = 1.0E-10;

	  public virtual void test_rate()
	  {
		LocalDate fixingDate = OBSERVATIONS[0].FixingDate;
		LocalDateDoubleTimeSeries timeSeries = LocalDateDoubleTimeSeries.of(fixingDate, FIXING_VALUES[0]);
		LocalDateDoubleTimeSeries rates = LocalDateDoubleTimeSeries.builder().put(OBSERVATIONS[1].FixingDate, FIXING_VALUES[1]).put(OBSERVATIONS[2].FixingDate, FIXING_VALUES[2]).put(OBSERVATIONS[3].FixingDate, FIXING_VALUES[3]).build();
		IborIndexRates mockIbor = new TestingIborIndexRates(GBP_LIBOR_3M, fixingDate, rates, timeSeries);
		SimpleRatesProvider prov = new SimpleRatesProvider(fixingDate);
		prov.IborRates = mockIbor;

		IList<IborAveragedFixing> fixings = new List<IborAveragedFixing>();
		double totalWeightedRate = 0.0d;
		double totalWeight = 0.0d;
		for (int i = 0; i < OBSERVATIONS.Length; i++)
		{
		  IborIndexObservation obs = OBSERVATIONS[i];
		  IborAveragedFixing fixing = IborAveragedFixing.builder().observation(obs).weight(WEIGHTS[i]).build();
		  fixings.Add(fixing);
		  totalWeightedRate += FIXING_VALUES[i] * WEIGHTS[i];
		  totalWeight += WEIGHTS[i];
		}

		double rateExpected = totalWeightedRate / totalWeight;
		IborAveragedRateComputation ro = IborAveragedRateComputation.of(fixings);
		ForwardIborAveragedRateComputationFn obsFn = ForwardIborAveragedRateComputationFn.DEFAULT;
		double rateComputed = obsFn.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, prov);
		assertEquals(rateComputed, rateExpected, TOLERANCE_RATE);

		// explain
		ExplainMapBuilder builder = ExplainMap.builder();
		assertEquals(obsFn.explainRate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, prov, builder), rateExpected, TOLERANCE_RATE);

		ExplainMap built = builder.build();
		assertEquals(built.get(ExplainKey.OBSERVATIONS).Present, true);
		assertEquals(built.get(ExplainKey.OBSERVATIONS).get().size(), OBSERVATIONS.Length);
		for (int i = 0; i < 4; i++)
		{
		  ExplainMap childMap = built.get(ExplainKey.OBSERVATIONS).get().get(i);
		  assertEquals(childMap.get(ExplainKey.FIXING_DATE), (OBSERVATIONS[i].FixingDate));
		  assertEquals(childMap.get(ExplainKey.INDEX), GBP_LIBOR_3M);
		  assertEquals(childMap.get(ExplainKey.INDEX_VALUE), FIXING_VALUES[i]);
		  assertEquals(childMap.get(ExplainKey.WEIGHT), WEIGHTS[i]);
		  assertEquals(childMap.get(ExplainKey.FROM_FIXING_SERIES), i == 0 ? true : null);
		}
		assertEquals(built.get(ExplainKey.COMBINED_RATE), rateExpected);
	  }

	  public virtual void test_rateSensitivity()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider();
		prov.IborRates = mockIbor;

		IList<IborAveragedFixing> fixings = new List<IborAveragedFixing>();
		double totalWeight = 0.0d;
		for (int i = 0; i < OBSERVATIONS.Length; i++)
		{
		  IborIndexObservation obs = OBSERVATIONS[i];
		  IborAveragedFixing fixing = IborAveragedFixing.builder().observation(obs).weight(WEIGHTS[i]).build();
		  fixings.Add(fixing);
		  totalWeight += WEIGHTS[i];
		  when(mockIbor.ratePointSensitivity(obs)).thenReturn(SENSITIVITIES[i]);
		}

		PointSensitivities expected = PointSensitivities.of(ImmutableList.of(IborRateSensitivity.of(OBSERVATIONS[0], WEIGHTS[0] / totalWeight), IborRateSensitivity.of(OBSERVATIONS[1], WEIGHTS[1] / totalWeight), IborRateSensitivity.of(OBSERVATIONS[2], WEIGHTS[2] / totalWeight), IborRateSensitivity.of(OBSERVATIONS[3], WEIGHTS[3] / totalWeight)));
		IborAveragedRateComputation ro = IborAveragedRateComputation.of(fixings);
		ForwardIborAveragedRateComputationFn obsFn = ForwardIborAveragedRateComputationFn.DEFAULT;
		PointSensitivityBuilder test = obsFn.rateSensitivity(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, prov);
		assertEquals(test.build(), expected);
	  }

	  public virtual void test_rateSensitivity_finiteDifference()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider();
		prov.IborRates = mockIbor;

		double eps = 1.0e-7;
		int nDates = OBSERVATIONS.Length;
		IList<IborAveragedFixing> fixings = new List<IborAveragedFixing>();
		for (int i = 0; i < nDates; i++)
		{
		  IborIndexObservation obs = OBSERVATIONS[i];
		  IborAveragedFixing fixing = IborAveragedFixing.builder().observation(obs).weight(WEIGHTS[i]).build();
		  fixings.Add(fixing);
		  when(mockIbor.ratePointSensitivity(obs)).thenReturn(SENSITIVITIES[i]);
		}

		IborAveragedRateComputation ro = IborAveragedRateComputation.of(fixings);
		ForwardIborAveragedRateComputationFn obsFn = ForwardIborAveragedRateComputationFn.DEFAULT;
		PointSensitivityBuilder test = obsFn.rateSensitivity(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, prov);
		for (int i = 0; i < nDates; ++i)
		{
		  IborIndexRates mockIborUp = mock(typeof(IborIndexRates));
		  SimpleRatesProvider provUp = new SimpleRatesProvider();
		  provUp.IborRates = mockIborUp;
		  IborIndexRates mockIborDw = mock(typeof(IborIndexRates));
		  SimpleRatesProvider provDw = new SimpleRatesProvider();
		  provDw.IborRates = mockIborDw;

		  for (int j = 0; j < nDates; ++j)
		  {
			if (i == j)
			{
			  when(mockIborUp.rate(OBSERVATIONS[j])).thenReturn(FIXING_VALUES[j] + eps);
			  when(mockIborDw.rate(OBSERVATIONS[j])).thenReturn(FIXING_VALUES[j] - eps);
			}
			else
			{
			  when(mockIborUp.rate(OBSERVATIONS[j])).thenReturn(FIXING_VALUES[j]);
			  when(mockIborDw.rate(OBSERVATIONS[j])).thenReturn(FIXING_VALUES[j]);
			}
		  }
		  double rateUp = obsFn.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, provUp);
		  double rateDw = obsFn.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, provDw);
		  double resExpected = 0.5 * (rateUp - rateDw) / eps;
		  assertEquals(test.build().Sensitivities.get(i).Sensitivity, resExpected, eps);
		}
	  }

	}

}