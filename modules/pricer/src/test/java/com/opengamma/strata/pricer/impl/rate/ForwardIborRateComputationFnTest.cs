/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
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

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using IborIndexRates = com.opengamma.strata.pricer.rate.IborIndexRates;
	using IborRateSensitivity = com.opengamma.strata.pricer.rate.IborRateSensitivity;
	using SimpleRatesProvider = com.opengamma.strata.pricer.rate.SimpleRatesProvider;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ForwardIborRateComputationFnTest
	public class ForwardIborRateComputationFnTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate FIXING_DATE = date(2014, 6, 30);
	  private static readonly LocalDate ACCRUAL_START_DATE = date(2014, 7, 2);
	  private static readonly LocalDate ACCRUAL_END_DATE = date(2014, 10, 2);
	  private static readonly LocalDate FORWARD_START_DATE = ACCRUAL_START_DATE.minusDays(2);
	  private static readonly LocalDate FORWARD_END_DATE = ACCRUAL_END_DATE.minusDays(2);
	  private const double RATE = 0.0123d;
	  private static readonly IborRateComputation GBP_LIBOR_3M_COMP = IborRateComputation.of(GBP_LIBOR_3M, FIXING_DATE, REF_DATA);
	  private static readonly IborRateSensitivity SENSITIVITY = IborRateSensitivity.of(GBP_LIBOR_3M_COMP.Observation, 1d);

	  public virtual void test_rate()
	  {
		SimpleRatesProvider prov = new SimpleRatesProvider();
		LocalDateDoubleTimeSeries timeSeries = LocalDateDoubleTimeSeries.of(FIXING_DATE, RATE);
		IborIndexRates mockIbor = new TestingIborIndexRates(GBP_LIBOR_3M, FIXING_DATE, LocalDateDoubleTimeSeries.empty(), timeSeries);
		prov.IborRates = mockIbor;

		ForwardIborRateComputationFn obsFn = ForwardIborRateComputationFn.DEFAULT;
		assertEquals(obsFn.rate(GBP_LIBOR_3M_COMP, ACCRUAL_START_DATE, ACCRUAL_END_DATE, prov), RATE);

		// explain
		ExplainMapBuilder builder = ExplainMap.builder();
		assertEquals(obsFn.explainRate(GBP_LIBOR_3M_COMP, ACCRUAL_START_DATE, ACCRUAL_END_DATE, prov, builder), RATE);

		ExplainMap built = builder.build();
		assertEquals(built.get(ExplainKey.OBSERVATIONS).Present, true);
		assertEquals(built.get(ExplainKey.OBSERVATIONS).get().size(), 1);
		assertEquals(built.get(ExplainKey.OBSERVATIONS).get().get(0).get(ExplainKey.FIXING_DATE), FIXING_DATE);
		assertEquals(built.get(ExplainKey.OBSERVATIONS).get().get(0).get(ExplainKey.INDEX), GBP_LIBOR_3M);
		assertEquals(built.get(ExplainKey.OBSERVATIONS).get().get(0).get(ExplainKey.INDEX_VALUE), RATE);
		assertEquals(built.get(ExplainKey.OBSERVATIONS).get().get(0).get(ExplainKey.FROM_FIXING_SERIES), true);
		assertEquals(built.get(ExplainKey.OBSERVATIONS).get().get(0).get(ExplainKey.FORWARD_RATE_START_DATE), FORWARD_START_DATE);
		assertEquals(built.get(ExplainKey.OBSERVATIONS).get().get(0).get(ExplainKey.FORWARD_RATE_END_DATE), FORWARD_END_DATE);
		assertEquals(built.get(ExplainKey.COMBINED_RATE), RATE);
	  }

	  public virtual void test_rateSensitivity()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider();
		prov.IborRates = mockIbor;

		when(mockIbor.ratePointSensitivity(GBP_LIBOR_3M_COMP.Observation)).thenReturn(SENSITIVITY);

		ForwardIborRateComputationFn obsFn = ForwardIborRateComputationFn.DEFAULT;
		assertEquals(obsFn.rateSensitivity(GBP_LIBOR_3M_COMP, ACCRUAL_START_DATE, ACCRUAL_END_DATE, prov), SENSITIVITY);
	  }

	}

}