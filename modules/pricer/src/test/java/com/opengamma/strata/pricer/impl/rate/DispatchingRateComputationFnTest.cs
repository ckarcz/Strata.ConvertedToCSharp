using System.Collections.Generic;

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
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.US_CPI_U;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.ignoreThrows;
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
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using RateComputationFn = com.opengamma.strata.pricer.rate.RateComputationFn;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using FixedRateComputation = com.opengamma.strata.product.rate.FixedRateComputation;
	using IborAveragedFixing = com.opengamma.strata.product.rate.IborAveragedFixing;
	using IborAveragedRateComputation = com.opengamma.strata.product.rate.IborAveragedRateComputation;
	using IborInterpolatedRateComputation = com.opengamma.strata.product.rate.IborInterpolatedRateComputation;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;
	using InflationEndInterpolatedRateComputation = com.opengamma.strata.product.rate.InflationEndInterpolatedRateComputation;
	using InflationEndMonthRateComputation = com.opengamma.strata.product.rate.InflationEndMonthRateComputation;
	using InflationInterpolatedRateComputation = com.opengamma.strata.product.rate.InflationInterpolatedRateComputation;
	using InflationMonthlyRateComputation = com.opengamma.strata.product.rate.InflationMonthlyRateComputation;
	using OvernightAveragedDailyRateComputation = com.opengamma.strata.product.rate.OvernightAveragedDailyRateComputation;
	using OvernightAveragedRateComputation = com.opengamma.strata.product.rate.OvernightAveragedRateComputation;
	using OvernightCompoundedRateComputation = com.opengamma.strata.product.rate.OvernightCompoundedRateComputation;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Test public class DispatchingRateComputationFnTest
	public class DispatchingRateComputationFnTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate FIXING_DATE = date(2014, 6, 30);
	  private static readonly LocalDate ACCRUAL_START_DATE = date(2014, 7, 2);
	  private static readonly LocalDate ACCRUAL_END_DATE = date(2014, 10, 2);

	  private static readonly YearMonth ACCRUAL_START_MONTH = YearMonth.of(2014, 7);
	  private static readonly YearMonth ACCRUAL_END_MONTH = YearMonth.of(2015, 7);

	  private static readonly RatesProvider MOCK_PROV = new MockRatesProvider();
	  private static readonly RateComputationFn<IborRateComputation> MOCK_IBOR_EMPTY = mock(typeof(RateComputationFn));
	  private static readonly RateComputationFn<IborInterpolatedRateComputation> MOCK_IBOR_INT_EMPTY = mock(typeof(RateComputationFn));
	  private static readonly RateComputationFn<IborAveragedRateComputation> MOCK_IBOR_AVE_EMPTY = mock(typeof(RateComputationFn));
	  private static readonly RateComputationFn<OvernightCompoundedRateComputation> MOCK_ON_CPD_EMPTY = mock(typeof(RateComputationFn));
	  private static readonly RateComputationFn<OvernightAveragedRateComputation> MOCK_ON_AVE_EMPTY = mock(typeof(RateComputationFn));
	  private static readonly RateComputationFn<OvernightAveragedDailyRateComputation> MOCK_ON_AVE_DLY_EMPTY = mock(typeof(RateComputationFn));
	  private static readonly RateComputationFn<InflationMonthlyRateComputation> MOCK_INF_MON_EMPTY = mock(typeof(RateComputationFn));
	  private static readonly RateComputationFn<InflationInterpolatedRateComputation> MOCK_INF_INT_EMPTY = mock(typeof(RateComputationFn));
	  private static readonly RateComputationFn<InflationEndMonthRateComputation> MOCK_INF_BOND_MON_EMPTY = mock(typeof(RateComputationFn));
	  private static readonly RateComputationFn<InflationEndInterpolatedRateComputation> MOCK_INF_BOND_INT_EMPTY = mock(typeof(RateComputationFn));

	  private const double TOLERANCE_RATE = 1.0E-10;

	  public virtual void test_rate_FixedRateComputation()
	  {
		FixedRateComputation ro = FixedRateComputation.of(0.0123d);
		DispatchingRateComputationFn test = DispatchingRateComputationFn.DEFAULT;
		assertEquals(test.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV), 0.0123d, 0d);
	  }

	  public virtual void test_rate_IborRateComputation()
	  {
		RateComputationFn<IborRateComputation> mockIbor = mock(typeof(RateComputationFn));
		IborRateComputation ro = IborRateComputation.of(GBP_LIBOR_3M, FIXING_DATE, REF_DATA);
		when(mockIbor.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV)).thenReturn(0.0123d);
		DispatchingRateComputationFn test = new DispatchingRateComputationFn(mockIbor, MOCK_IBOR_INT_EMPTY, MOCK_IBOR_AVE_EMPTY, MOCK_ON_CPD_EMPTY, MOCK_ON_AVE_EMPTY, MOCK_ON_AVE_DLY_EMPTY, MOCK_INF_MON_EMPTY, MOCK_INF_INT_EMPTY, MOCK_INF_BOND_MON_EMPTY, MOCK_INF_BOND_INT_EMPTY);
		assertEquals(test.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV), 0.0123d, 0d);
	  }

	  public virtual void test_rate_IborInterpolatedRateComputation()
	  {
		double mockRate = 0.0123d;
		RateComputationFn<IborInterpolatedRateComputation> mockIborInt = mock(typeof(RateComputationFn));
		IborInterpolatedRateComputation ro = IborInterpolatedRateComputation.of(GBP_LIBOR_3M, GBP_LIBOR_6M, FIXING_DATE, REF_DATA);
		when(mockIborInt.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV)).thenReturn(mockRate);
		DispatchingRateComputationFn test = new DispatchingRateComputationFn(MOCK_IBOR_EMPTY, mockIborInt, MOCK_IBOR_AVE_EMPTY, MOCK_ON_CPD_EMPTY, MOCK_ON_AVE_EMPTY, MOCK_ON_AVE_DLY_EMPTY, MOCK_INF_MON_EMPTY, MOCK_INF_INT_EMPTY, MOCK_INF_BOND_MON_EMPTY, MOCK_INF_BOND_INT_EMPTY);
		assertEquals(test.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV), mockRate, 0d);
	  }

	  public virtual void test_rate_IborAverageRateComputation()
	  {
		double mockRate = 0.0123d;
		RateComputationFn<IborAveragedRateComputation> mockIborAve = mock(typeof(RateComputationFn));
		LocalDate[] fixingDates = new LocalDate[] {date(2014, 6, 30), date(2014, 7, 7), date(2014, 7, 14), date(2014, 7, 21)};
		double[] weights = new double[] {0.10d, 0.20d, 0.30d, 0.40d};
		IList<IborAveragedFixing> fixings = new List<IborAveragedFixing>();
		for (int i = 0; i < fixingDates.Length; i++)
		{
		  IborAveragedFixing fixing = IborAveragedFixing.builder().observation(IborIndexObservation.of(GBP_LIBOR_3M, fixingDates[i], REF_DATA)).weight(weights[i]).build();
		  fixings.Add(fixing);
		}
		IborAveragedRateComputation ro = IborAveragedRateComputation.of(fixings);
		when(mockIborAve.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV)).thenReturn(mockRate);
		DispatchingRateComputationFn test = new DispatchingRateComputationFn(MOCK_IBOR_EMPTY, MOCK_IBOR_INT_EMPTY, mockIborAve, MOCK_ON_CPD_EMPTY, MOCK_ON_AVE_EMPTY, MOCK_ON_AVE_DLY_EMPTY, MOCK_INF_MON_EMPTY, MOCK_INF_INT_EMPTY, MOCK_INF_BOND_MON_EMPTY, MOCK_INF_BOND_INT_EMPTY);
		assertEquals(test.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV), mockRate, 0d);
	  }

	  public virtual void test_rate_OvernightCompoundedRateComputation()
	  {
		double mockRate = 0.0123d;
		RateComputationFn<OvernightCompoundedRateComputation> mockOnCpd = mock(typeof(RateComputationFn));
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(USD_FED_FUND, ACCRUAL_START_DATE, ACCRUAL_END_DATE, 0, REF_DATA);
		when(mockOnCpd.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV)).thenReturn(mockRate);
		DispatchingRateComputationFn test = new DispatchingRateComputationFn(MOCK_IBOR_EMPTY, MOCK_IBOR_INT_EMPTY, MOCK_IBOR_AVE_EMPTY, mockOnCpd, MOCK_ON_AVE_EMPTY, MOCK_ON_AVE_DLY_EMPTY, MOCK_INF_MON_EMPTY, MOCK_INF_INT_EMPTY, MOCK_INF_BOND_MON_EMPTY, MOCK_INF_BOND_INT_EMPTY);
		assertEquals(test.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV), mockRate, TOLERANCE_RATE);
	  }

	  public virtual void test_rate_OvernightAveragedRateComputation()
	  {
		double mockRate = 0.0123d;
		RateComputationFn<OvernightAveragedRateComputation> mockOnAve = mock(typeof(RateComputationFn));
		OvernightAveragedRateComputation ro = OvernightAveragedRateComputation.of(USD_FED_FUND, ACCRUAL_START_DATE, ACCRUAL_END_DATE, 0, REF_DATA);
		when(mockOnAve.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV)).thenReturn(mockRate);
		DispatchingRateComputationFn test = new DispatchingRateComputationFn(MOCK_IBOR_EMPTY, MOCK_IBOR_INT_EMPTY, MOCK_IBOR_AVE_EMPTY, MOCK_ON_CPD_EMPTY, mockOnAve, MOCK_ON_AVE_DLY_EMPTY, MOCK_INF_MON_EMPTY, MOCK_INF_INT_EMPTY, MOCK_INF_BOND_MON_EMPTY, MOCK_INF_BOND_INT_EMPTY);
		assertEquals(test.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV), mockRate, TOLERANCE_RATE);
	  }

	  public virtual void test_rate_OvernightAveragedDailyRateComputation()
	  {
		double mockRate = 0.0123d;
		RateComputationFn<OvernightAveragedDailyRateComputation> mockOnAve = mock(typeof(RateComputationFn));
		OvernightAveragedDailyRateComputation ro = OvernightAveragedDailyRateComputation.of(USD_FED_FUND, ACCRUAL_START_DATE, ACCRUAL_END_DATE, REF_DATA);
		when(mockOnAve.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV)).thenReturn(mockRate);
		DispatchingRateComputationFn test = new DispatchingRateComputationFn(MOCK_IBOR_EMPTY, MOCK_IBOR_INT_EMPTY, MOCK_IBOR_AVE_EMPTY, MOCK_ON_CPD_EMPTY, MOCK_ON_AVE_EMPTY, mockOnAve, MOCK_INF_MON_EMPTY, MOCK_INF_INT_EMPTY, MOCK_INF_BOND_MON_EMPTY, MOCK_INF_BOND_INT_EMPTY);
		assertEquals(test.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV), mockRate, TOLERANCE_RATE);
	  }

	  public virtual void test_rate_InflationMonthlyRateComputation()
	  {
		double mockRate = 223.0d;
		RateComputationFn<InflationMonthlyRateComputation> mockInfMon = mock(typeof(RateComputationFn));
		InflationMonthlyRateComputation ro = InflationMonthlyRateComputation.of(US_CPI_U, ACCRUAL_START_MONTH, ACCRUAL_END_MONTH);
		when(mockInfMon.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV)).thenReturn(mockRate);
		DispatchingRateComputationFn test = new DispatchingRateComputationFn(MOCK_IBOR_EMPTY, MOCK_IBOR_INT_EMPTY, MOCK_IBOR_AVE_EMPTY, MOCK_ON_CPD_EMPTY, MOCK_ON_AVE_EMPTY, MOCK_ON_AVE_DLY_EMPTY, mockInfMon, MOCK_INF_INT_EMPTY, MOCK_INF_BOND_MON_EMPTY, MOCK_INF_BOND_INT_EMPTY);
		assertEquals(test.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV), mockRate, TOLERANCE_RATE);
	  }

	  public virtual void test_rate_InflationInterpolatedRateComputation()
	  {
		double mockRate = 223.0d;
		RateComputationFn<InflationInterpolatedRateComputation> mockInfInt = mock(typeof(RateComputationFn));
		InflationInterpolatedRateComputation ro = InflationInterpolatedRateComputation.of(US_CPI_U, ACCRUAL_START_MONTH, ACCRUAL_END_MONTH, 0.3);
		when(mockInfInt.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV)).thenReturn(mockRate);
		DispatchingRateComputationFn test = new DispatchingRateComputationFn(MOCK_IBOR_EMPTY, MOCK_IBOR_INT_EMPTY, MOCK_IBOR_AVE_EMPTY, MOCK_ON_CPD_EMPTY, MOCK_ON_AVE_EMPTY, MOCK_ON_AVE_DLY_EMPTY, MOCK_INF_MON_EMPTY, mockInfInt, MOCK_INF_BOND_MON_EMPTY, MOCK_INF_BOND_INT_EMPTY);
		assertEquals(test.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV), mockRate, TOLERANCE_RATE);
	  }

	  public virtual void test_rate_InflationEndMonthRateComputation()
	  {
		double mockRate = 223.0d;
		RateComputationFn<InflationEndMonthRateComputation> mockInfMon = mock(typeof(RateComputationFn));
		InflationEndMonthRateComputation ro = InflationEndMonthRateComputation.of(US_CPI_U, 123d, ACCRUAL_END_MONTH);
		when(mockInfMon.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV)).thenReturn(mockRate);
		DispatchingRateComputationFn test = new DispatchingRateComputationFn(MOCK_IBOR_EMPTY, MOCK_IBOR_INT_EMPTY, MOCK_IBOR_AVE_EMPTY, MOCK_ON_CPD_EMPTY, MOCK_ON_AVE_EMPTY, MOCK_ON_AVE_DLY_EMPTY, MOCK_INF_MON_EMPTY, MOCK_INF_INT_EMPTY, mockInfMon, MOCK_INF_BOND_INT_EMPTY);
		assertEquals(test.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV), mockRate, TOLERANCE_RATE);
	  }

	  public virtual void test_rate_InflationEndInterpolatedRateComputation()
	  {
		double mockRate = 223.0d;
		RateComputationFn<InflationEndInterpolatedRateComputation> mockInfInt = mock(typeof(RateComputationFn));
		InflationEndInterpolatedRateComputation ro = InflationEndInterpolatedRateComputation.of(US_CPI_U, 234d, ACCRUAL_END_MONTH, 0.3);
		when(mockInfInt.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV)).thenReturn(mockRate);
		DispatchingRateComputationFn test = new DispatchingRateComputationFn(MOCK_IBOR_EMPTY, MOCK_IBOR_INT_EMPTY, MOCK_IBOR_AVE_EMPTY, MOCK_ON_CPD_EMPTY, MOCK_ON_AVE_EMPTY, MOCK_ON_AVE_DLY_EMPTY, MOCK_INF_MON_EMPTY, MOCK_INF_INT_EMPTY, MOCK_INF_BOND_MON_EMPTY, mockInfInt);
		assertEquals(test.rate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV), mockRate, TOLERANCE_RATE);
	  }

	  public virtual void test_rate_unknownType()
	  {
		RateComputation mockComputation = mock(typeof(RateComputation));
		DispatchingRateComputationFn test = DispatchingRateComputationFn.DEFAULT;
		assertThrowsIllegalArg(() => test.rate(mockComputation, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_explainRate_FixedRateComputation()
	  {
		FixedRateComputation ro = FixedRateComputation.of(0.0123d);
		DispatchingRateComputationFn test = DispatchingRateComputationFn.DEFAULT;
		ExplainMapBuilder builder = ExplainMap.builder();
		assertEquals(test.explainRate(ro, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV, builder), 0.0123d, 0d);
		ExplainMap built = builder.build();
		assertEquals(built.get(ExplainKey.FIXED_RATE), 0.0123d);
		assertEquals(built.get(ExplainKey.COMBINED_RATE), 0.0123d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		DispatchingRateComputationFn test = new DispatchingRateComputationFn(MOCK_IBOR_EMPTY, MOCK_IBOR_INT_EMPTY, MOCK_IBOR_AVE_EMPTY, MOCK_ON_CPD_EMPTY, MOCK_ON_AVE_EMPTY, MOCK_ON_AVE_DLY_EMPTY, MOCK_INF_MON_EMPTY, MOCK_INF_INT_EMPTY, MOCK_INF_BOND_MON_EMPTY, MOCK_INF_BOND_INT_EMPTY);
		FixedRateComputation @fixed = FixedRateComputation.of(0.0123d);
		IborRateComputation ibor = IborRateComputation.of(GBP_LIBOR_3M, FIXING_DATE, REF_DATA);
		IborInterpolatedRateComputation iborInt = IborInterpolatedRateComputation.of(GBP_LIBOR_3M, GBP_LIBOR_6M, FIXING_DATE, REF_DATA);
		IborAveragedRateComputation iborAvg = IborAveragedRateComputation.of(ImmutableList.of(IborAveragedFixing.of(ibor.Observation)));
		OvernightCompoundedRateComputation onCpd = OvernightCompoundedRateComputation.of(USD_FED_FUND, ACCRUAL_START_DATE, ACCRUAL_END_DATE, 0, REF_DATA);
		OvernightAveragedRateComputation onAvg = OvernightAveragedRateComputation.of(USD_FED_FUND, ACCRUAL_START_DATE, ACCRUAL_END_DATE, 0, REF_DATA);
		OvernightAveragedDailyRateComputation onAvgDly = OvernightAveragedDailyRateComputation.of(USD_FED_FUND, ACCRUAL_START_DATE, ACCRUAL_END_DATE, REF_DATA);
		InflationMonthlyRateComputation inflationMonthly = InflationMonthlyRateComputation.of(US_CPI_U, ACCRUAL_START_MONTH, ACCRUAL_END_MONTH);
		InflationInterpolatedRateComputation inflationInterp = InflationInterpolatedRateComputation.of(US_CPI_U, ACCRUAL_START_MONTH, ACCRUAL_END_MONTH, 0.3);
		InflationEndMonthRateComputation inflationEndMonth = InflationEndMonthRateComputation.of(US_CPI_U, 234d, ACCRUAL_END_MONTH);
		InflationEndInterpolatedRateComputation inflationEndInterp = InflationEndInterpolatedRateComputation.of(US_CPI_U, 1234d, ACCRUAL_END_MONTH, 0.3);

		RateComputation mock = mock(typeof(RateComputation));
		ignoreThrows(() => test.rateSensitivity(@fixed, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV));
		ignoreThrows(() => test.rateSensitivity(ibor, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV));
		ignoreThrows(() => test.rateSensitivity(iborInt, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV));
		ignoreThrows(() => test.rateSensitivity(iborAvg, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV));
		ignoreThrows(() => test.rateSensitivity(onCpd, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV));
		ignoreThrows(() => test.rateSensitivity(onAvg, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV));
		ignoreThrows(() => test.rateSensitivity(onAvgDly, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV));
		ignoreThrows(() => test.rateSensitivity(inflationMonthly, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV));
		ignoreThrows(() => test.rateSensitivity(inflationInterp, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV));
		ignoreThrows(() => test.rateSensitivity(inflationEndMonth, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV));
		ignoreThrows(() => test.rateSensitivity(inflationEndInterp, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV));
		ignoreThrows(() => test.rateSensitivity(mock, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV));

		ExplainMapBuilder explain = ExplainMap.builder();
		ignoreThrows(() => test.explainRate(@fixed, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV, explain));
		ignoreThrows(() => test.explainRate(ibor, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV, explain));
		ignoreThrows(() => test.explainRate(iborInt, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV, explain));
		ignoreThrows(() => test.explainRate(iborAvg, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV, explain));
		ignoreThrows(() => test.explainRate(onCpd, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV, explain));
		ignoreThrows(() => test.explainRate(onAvg, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV, explain));
		ignoreThrows(() => test.explainRate(onAvgDly, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV, explain));
		ignoreThrows(() => test.explainRate(inflationMonthly, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV, explain));
		ignoreThrows(() => test.explainRate(inflationInterp, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV, explain));
		ignoreThrows(() => test.explainRate(inflationEndMonth, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV, explain));
		ignoreThrows(() => test.explainRate(inflationEndInterp, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV, explain));
		ignoreThrows(() => test.explainRate(mock, ACCRUAL_START_DATE, ACCRUAL_END_DATE, MOCK_PROV, explain));
	  }

	}

}