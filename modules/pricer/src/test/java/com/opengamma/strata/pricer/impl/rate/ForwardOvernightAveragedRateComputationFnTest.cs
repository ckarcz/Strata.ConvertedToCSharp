using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.rate
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.CHF_TOIS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.GBP_SONIA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.mock;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.when;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using OvernightIndexObservation = com.opengamma.strata.basics.index.OvernightIndexObservation;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using LocalDateDoubleTimeSeriesBuilder = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeriesBuilder;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using OvernightIndexRates = com.opengamma.strata.pricer.rate.OvernightIndexRates;
	using OvernightRateSensitivity = com.opengamma.strata.pricer.rate.OvernightRateSensitivity;
	using SimpleRatesProvider = com.opengamma.strata.pricer.rate.SimpleRatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using OvernightAveragedRateComputation = com.opengamma.strata.product.rate.OvernightAveragedRateComputation;

	/// <summary>
	/// Test <seealso cref="ForwardOvernightAveragedRateComputationFn"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ForwardOvernightAveragedRateComputationFnTest
	public class ForwardOvernightAveragedRateComputationFnTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate DUMMY_ACCRUAL_START_DATE = date(2015, 1, 1); // Accrual dates irrelevant for the rate
	  private static readonly LocalDate DUMMY_ACCRUAL_END_DATE = date(2015, 1, 1); // Accrual dates irrelevant for the rate
	  private static readonly LocalDate START_DATE = date(2015, 1, 8);
	  private static readonly LocalDate END_DATE = date(2015, 1, 15); // 1w only to decrease data
	  private static readonly LocalDate[] FIXING_DATES = new LocalDate[] {date(2015, 1, 7), date(2015, 1, 8), date(2015, 1, 9), date(2015, 1, 12), date(2015, 1, 13), date(2015, 1, 14), date(2015, 1, 15)};
	  private static readonly OvernightIndexObservation[] USD_OBS = new OvernightIndexObservation[] {OvernightIndexObservation.of(USD_FED_FUND, date(2015, 1, 7), REF_DATA), OvernightIndexObservation.of(USD_FED_FUND, date(2015, 1, 8), REF_DATA), OvernightIndexObservation.of(USD_FED_FUND, date(2015, 1, 9), REF_DATA), OvernightIndexObservation.of(USD_FED_FUND, date(2015, 1, 12), REF_DATA), OvernightIndexObservation.of(USD_FED_FUND, date(2015, 1, 13), REF_DATA), OvernightIndexObservation.of(USD_FED_FUND, date(2015, 1, 14), REF_DATA), OvernightIndexObservation.of(USD_FED_FUND, date(2015, 1, 15), REF_DATA)};
	  private static readonly OvernightIndexObservation[] GBP_OBS = new OvernightIndexObservation[] {OvernightIndexObservation.of(GBP_SONIA, date(2015, 1, 7), REF_DATA), OvernightIndexObservation.of(GBP_SONIA, date(2015, 1, 8), REF_DATA), OvernightIndexObservation.of(GBP_SONIA, date(2015, 1, 9), REF_DATA), OvernightIndexObservation.of(GBP_SONIA, date(2015, 1, 12), REF_DATA), OvernightIndexObservation.of(GBP_SONIA, date(2015, 1, 13), REF_DATA), OvernightIndexObservation.of(GBP_SONIA, date(2015, 1, 14), REF_DATA), OvernightIndexObservation.of(GBP_SONIA, date(2015, 1, 15), REF_DATA)};
	  private static readonly OvernightIndexObservation[] CHF_OBS = new OvernightIndexObservation[] {OvernightIndexObservation.of(CHF_TOIS, date(2015, 1, 7), REF_DATA), OvernightIndexObservation.of(CHF_TOIS, date(2015, 1, 8), REF_DATA), OvernightIndexObservation.of(CHF_TOIS, date(2015, 1, 9), REF_DATA), OvernightIndexObservation.of(CHF_TOIS, date(2015, 1, 12), REF_DATA), OvernightIndexObservation.of(CHF_TOIS, date(2015, 1, 13), REF_DATA), OvernightIndexObservation.of(CHF_TOIS, date(2015, 1, 14), REF_DATA), OvernightIndexObservation.of(CHF_TOIS, date(2015, 1, 15), REF_DATA)};
	  private static readonly double[] FIXING_RATES = new double[] {0.0012, 0.0023, 0.0034, 0.0045, 0.0056, 0.0067, 0.0078};
	  private const double TOLERANCE_RATE = 1.0E-10;
	  private const double EPS_FD = 1.0E-7;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Test for the case where publication lag=1, effective offset=0 (USD conventions) and no cutoff period. </summary>
	  public virtual void rateFedFundNoCutOff()
	  {
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(USD_FED_FUND);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		for (int i = 0; i < USD_OBS.Length; i++)
		{
		  when(mockRates.rate(USD_OBS[i])).thenReturn(FIXING_RATES[i]);
		}
		OvernightAveragedRateComputation ro = OvernightAveragedRateComputation.of(USD_FED_FUND, START_DATE, END_DATE, 0, REF_DATA);
		// Accrual dates = fixing dates
		ForwardOvernightAveragedRateComputationFn obsFn = ForwardOvernightAveragedRateComputationFn.DEFAULT;
		double accrualFactorTotal = 0.0d;
		double accruedRate = 0.0d;
		int indexLast = 5; // Fixing in the observation period are from 1 to 5 (inclusive)
		for (int i = 1; i <= indexLast; i++)
		{
		  LocalDate endDate = USD_OBS[i].MaturityDate;
		  double af = USD_FED_FUND.DayCount.yearFraction(FIXING_DATES[i], endDate);
		  accrualFactorTotal += af;
		  accruedRate += FIXING_RATES[i] * af;
		}
		double rateExpected = accruedRate / accrualFactorTotal;
		double rateComputed = obsFn.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		assertEquals(rateExpected, rateComputed, TOLERANCE_RATE);

		// explain
		ExplainMapBuilder builder = ExplainMap.builder();
		double explainedRate = obsFn.explainRate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv, builder);
		assertEquals(explainedRate, rateExpected, TOLERANCE_RATE);

		ExplainMap built = builder.build();
		assertEquals(built.get(ExplainKey.OBSERVATIONS).Present, false);
		assertEquals(built.get(ExplainKey.COMBINED_RATE).Value.doubleValue(), rateExpected, TOLERANCE_RATE);
	  }

	  /// <summary>
	  /// Test against FD approximation for the case where publication lag=1, effective offset=0 (USD conventions) and 
	  /// no cutoff period. Note that all the rates are bumped here, i.e., all the rates are treated as forward rates.
	  /// </summary>
	  public virtual void rateFedFundNoCutOffSensitivity()
	  {
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(USD_FED_FUND);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		for (int i = 0; i < USD_OBS.Length; i++)
		{
		  when(mockRates.rate(USD_OBS[i])).thenReturn(FIXING_RATES[i]);
		  LocalDate fixingEndDate = USD_OBS[i].MaturityDate;
		  OvernightRateSensitivity sensitivity = OvernightRateSensitivity.ofPeriod(USD_OBS[i], fixingEndDate, USD_FED_FUND.Currency, 1d);
		  when(mockRates.ratePointSensitivity(USD_OBS[i])).thenReturn(sensitivity);
		}
		OvernightAveragedRateComputation ro = OvernightAveragedRateComputation.of(USD_FED_FUND, START_DATE, END_DATE, 0, REF_DATA);
		ForwardOvernightAveragedRateComputationFn obsFn = ForwardOvernightAveragedRateComputationFn.DEFAULT;
		PointSensitivityBuilder sensitivityBuilderComputed = obsFn.rateSensitivity(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		PointSensitivities sensitivityComputed = sensitivityBuilderComputed.build().normalized();
		double?[] sensitivityExpected = computedSensitivityFD(ro, USD_FED_FUND, USD_OBS);
		assertEquals(sensitivityComputed.Sensitivities.size(), sensitivityExpected.Length);
		for (int i = 0; i < sensitivityExpected.Length; ++i)
		{
		  assertEquals(sensitivityComputed.Sensitivities.get(i).Sensitivity, sensitivityExpected[i], EPS_FD);
		}
	  }

	  /// <summary>
	  /// Test for the case where publication lag=1, effective offset=0 (USD conventions) and cutoff=2 (FedFund swaps). </summary>
	  public virtual void rateFedFund()
	  {
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(USD_FED_FUND);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		for (int i = 0; i < USD_OBS.Length; i++)
		{
		  when(mockRates.rate(USD_OBS[i])).thenReturn(FIXING_RATES[i]);
		}
		OvernightAveragedRateComputation ro = OvernightAveragedRateComputation.of(USD_FED_FUND, START_DATE, END_DATE, 2, REF_DATA);
		ForwardOvernightAveragedRateComputationFn obsFn = ForwardOvernightAveragedRateComputationFn.DEFAULT;
		double accrualFactorTotal = 0.0d;
		double accruedRate = 0.0d;
		int indexLast = 5; // Fixing in the observation period are from 1 to 5 (inclusive), but last is modified by cut-off
		for (int i = 1; i <= indexLast - 1; i++)
		{
		  LocalDate endDate = USD_OBS[i].MaturityDate;
		  double af = USD_FED_FUND.DayCount.yearFraction(FIXING_DATES[i], endDate);
		  accrualFactorTotal += af;
		  accruedRate += FIXING_RATES[i] * af;
		}
		// CutOff
		LocalDate endDate = USD_OBS[indexLast].MaturityDate;
		double af = USD_FED_FUND.DayCount.yearFraction(FIXING_DATES[indexLast], endDate);
		accrualFactorTotal += af;
		accruedRate += FIXING_RATES[indexLast - 1] * af;
		double rateExpected = accruedRate / accrualFactorTotal;
		double rateComputed = obsFn.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		assertEquals(rateExpected, rateComputed, TOLERANCE_RATE);
	  }

	  /// <summary>
	  /// Test against FD approximation for the case where publication lag=1, effective offset=0 (USD conventions) and 
	  /// cutoff=2 (FedFund swaps). 
	  /// Note that all the rates are bumped here, i.e., all the rates are treated as forward rates. 
	  /// </summary>
	  public virtual void rateFedFundSensitivity()
	  {
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(USD_FED_FUND);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		for (int i = 0; i < USD_OBS.Length; i++)
		{
		  when(mockRates.rate(USD_OBS[i])).thenReturn(FIXING_RATES[i]);
		  OvernightRateSensitivity sensitivity = OvernightRateSensitivity.of(USD_OBS[i], USD_FED_FUND.Currency, 1d);
		  when(mockRates.ratePointSensitivity(USD_OBS[i])).thenReturn(sensitivity);
		}
		OvernightAveragedRateComputation ro = OvernightAveragedRateComputation.of(USD_FED_FUND, START_DATE, END_DATE, 2, REF_DATA);
		ForwardOvernightAveragedRateComputationFn obsFn = ForwardOvernightAveragedRateComputationFn.DEFAULT;

		PointSensitivityBuilder sensitivityBuilderComputed = obsFn.rateSensitivity(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		PointSensitivities sensitivityComputed = sensitivityBuilderComputed.build().normalized();
		double?[] sensitivityExpected = computedSensitivityFD(ro, USD_FED_FUND, USD_OBS);
		assertEquals(sensitivityComputed.Sensitivities.size(), sensitivityExpected.Length);
		for (int i = 0; i < sensitivityExpected.Length; ++i)
		{
		  assertEquals(sensitivityComputed.Sensitivities.get(i).Sensitivity, sensitivityExpected[i], EPS_FD);
		}
	  }

	  /// <summary>
	  /// Test for the case where publication lag=0, effective offset=1 (CHF conventions) and no cutoff period.
	  /// The arithmetic average coupons are used mainly in USD. This test is more for completeness than a real case.
	  /// </summary>
	  public virtual void rateChfNoCutOff()
	  {
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(CHF_TOIS);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		for (int i = 0; i < CHF_OBS.Length; i++)
		{
		  when(mockRates.rate(CHF_OBS[i])).thenReturn(FIXING_RATES[i]);
		}
		OvernightAveragedRateComputation ro = OvernightAveragedRateComputation.of(CHF_TOIS, START_DATE, END_DATE, 0, REF_DATA);
		ForwardOvernightAveragedRateComputationFn obsFn = ForwardOvernightAveragedRateComputationFn.DEFAULT;
		double accrualFactorTotal = 0.0d;
		double accruedRate = 0.0d;
		int indexLast = 5; // Fixing in the observation period are from 0 to 4 (inclusive)
		for (int i = 0; i < indexLast; i++)
		{
		  LocalDate startDate = CHF_OBS[i].EffectiveDate;
		  LocalDate endDate = CHF_OBS[i].MaturityDate;
		  double af = CHF_TOIS.DayCount.yearFraction(startDate, endDate);
		  accrualFactorTotal += af;
		  accruedRate += FIXING_RATES[i] * af;
		}
		double rateExpected = accruedRate / accrualFactorTotal;
		double rateComputed = obsFn.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		assertEquals(rateExpected, rateComputed, TOLERANCE_RATE);
	  }

	  /// <summary>
	  /// Test for the case where publication lag=0, effective offset=1 (CHF conventions) and no cutoff period.
	  /// The arithmetic average coupons are used mainly in USD. This test is more for completeness than a real case.
	  /// </summary>
	  public virtual void rateChfNoCutOffSensitivity()
	  {
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(CHF_TOIS);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		for (int i = 0; i < CHF_OBS.Length; i++)
		{
		  when(mockRates.rate(CHF_OBS[i])).thenReturn(FIXING_RATES[i]);
		  OvernightRateSensitivity sensitivity = OvernightRateSensitivity.of(CHF_OBS[i], CHF_TOIS.Currency, 1d);
		  when(mockRates.ratePointSensitivity(CHF_OBS[i])).thenReturn(sensitivity);
		}
		OvernightAveragedRateComputation ro = OvernightAveragedRateComputation.of(CHF_TOIS, START_DATE, END_DATE, 0, REF_DATA);
		ForwardOvernightAveragedRateComputationFn obsFn = ForwardOvernightAveragedRateComputationFn.DEFAULT;
		PointSensitivityBuilder sensitivityBuilderComputed = obsFn.rateSensitivity(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		PointSensitivities sensitivityComputed = sensitivityBuilderComputed.build().normalized();
		double?[] sensitivityExpected = computedSensitivityFD(ro, CHF_TOIS, CHF_OBS);
		assertEquals(sensitivityComputed.Sensitivities.size(), sensitivityExpected.Length);
		for (int i = 0; i < sensitivityExpected.Length; ++i)
		{
		  assertEquals(sensitivityComputed.Sensitivities.get(i).Sensitivity, sensitivityExpected[i], EPS_FD);
		}
	  }

	  /// <summary>
	  /// Test for the case where publication lag=0, effective offset=0 (GBP conventions) and no cutoff period.
	  ///   The arithmetic average coupons are used mainly in USD. This test is more for completeness than a real case. 
	  /// </summary>
	  public virtual void rateGbpNoCutOff()
	  {
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(GBP_SONIA);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		for (int i = 0; i < GBP_OBS.Length; i++)
		{
		  when(mockRates.rate(GBP_OBS[i])).thenReturn(FIXING_RATES[i]);
		}
		OvernightAveragedRateComputation ro = OvernightAveragedRateComputation.of(GBP_SONIA, START_DATE, END_DATE, 0, REF_DATA);
		ForwardOvernightAveragedRateComputationFn obsFn = ForwardOvernightAveragedRateComputationFn.DEFAULT;
		double accrualFactorTotal = 0.0d;
		double accruedRate = 0.0d;
		int indexLast = 5; // Fixing in the observation period are from 1 to 5 (inclusive)
		for (int i = 1; i <= indexLast; i++)
		{
		  LocalDate startDate = GBP_OBS[i].EffectiveDate;
		  LocalDate endDate = GBP_OBS[i].MaturityDate;
		  double af = GBP_SONIA.DayCount.yearFraction(startDate, endDate);
		  accrualFactorTotal += af;
		  accruedRate += FIXING_RATES[i] * af;
		}
		double rateExpected = accruedRate / accrualFactorTotal;
		double rateComputed = obsFn.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		assertEquals(rateExpected, rateComputed, TOLERANCE_RATE);
	  }

	  /// <summary>
	  /// Test for the case where publication lag=0, effective offset=0 (GBP conventions) and no cutoff period.
	  ///   The arithmetic average coupons are used mainly in USD. This test is more for completeness than a real case. 
	  /// </summary>
	  public virtual void rateGbpNoCutOffSensitivity()
	  {
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(GBP_SONIA);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		for (int i = 0; i < GBP_OBS.Length; i++)
		{
		  when(mockRates.rate(GBP_OBS[i])).thenReturn(FIXING_RATES[i]);
		  OvernightRateSensitivity sensitivity = OvernightRateSensitivity.of(GBP_OBS[i], GBP_SONIA.Currency, 1d);
		  when(mockRates.ratePointSensitivity(GBP_OBS[i])).thenReturn(sensitivity);
		}
		OvernightAveragedRateComputation ro = OvernightAveragedRateComputation.of(GBP_SONIA, START_DATE, END_DATE, 0, REF_DATA);
		ForwardOvernightAveragedRateComputationFn obsFn = ForwardOvernightAveragedRateComputationFn.DEFAULT;
		PointSensitivityBuilder sensitivityBuilderComputed = obsFn.rateSensitivity(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		PointSensitivities sensitivityComputed = sensitivityBuilderComputed.build().normalized();
		double?[] sensitivityExpected = computedSensitivityFD(ro, GBP_SONIA, GBP_OBS);
		assertEquals(sensitivityComputed.Sensitivities.size(), sensitivityExpected.Length);
		for (int i = 0; i < sensitivityExpected.Length; ++i)
		{
		  assertEquals(sensitivityComputed.Sensitivities.get(i).Sensitivity, sensitivityExpected[i], EPS_FD);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.DOUBLE_QUADRATIC;
	  private static readonly LocalDateDoubleTimeSeries TIME_SERIES;
	  static ForwardOvernightAveragedRateComputationFnTest()
	  {
		LocalDateDoubleTimeSeriesBuilder builder = LocalDateDoubleTimeSeries.builder();
		for (int i = 0; i < FIXING_DATES.Length; i++)
		{
		  builder.put(FIXING_DATES[i], FIXING_RATES[i]);
		}
		TIME_SERIES = builder.build();
	  }
	  private static readonly RatesFiniteDifferenceSensitivityCalculator CAL_FD = new RatesFiniteDifferenceSensitivityCalculator(EPS_FD);

	  /// <summary>
	  /// Test parameter sensitivity with finite difference sensitivity calculator. Two days cutoff period. </summary>
	  public virtual void rateFedFundTwoDaysCutoffParameterSensitivity()
	  {
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 1), date(2015, 1, 8)};
		DoubleArray time = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 10.0);
		DoubleArray rate = DoubleArray.of(0.0100, 0.0110, 0.0115, 0.0130, 0.0135, 0.0135);

		for (int loopvaldate = 0; loopvaldate < 2; loopvaldate++)
		{
		  Curve onCurve = InterpolatedNodalCurve.of(Curves.zeroRates("ON", ACT_ACT_ISDA), time, rate, INTERPOLATOR);
		  ImmutableRatesProvider prov = ImmutableRatesProvider.builder(valuationDate[loopvaldate]).overnightIndexCurve(USD_FED_FUND, onCurve, TIME_SERIES).build();
		  OvernightAveragedRateComputation ro = OvernightAveragedRateComputation.of(USD_FED_FUND, START_DATE, END_DATE, 2, REF_DATA);
		  ForwardOvernightAveragedRateComputationFn obsFn = ForwardOvernightAveragedRateComputationFn.DEFAULT;

		  PointSensitivityBuilder sensitivityBuilderComputed = obsFn.rateSensitivity(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, prov);
		  CurrencyParameterSensitivities parameterSensitivityComputed = prov.parameterSensitivity(sensitivityBuilderComputed.build());

		  CurrencyParameterSensitivities parameterSensitivityExpected = CAL_FD.sensitivity(prov, (p) => CurrencyAmount.of(USD_FED_FUND.Currency, obsFn.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, (p))));
		  assertTrue(parameterSensitivityComputed.equalWithTolerance(parameterSensitivityExpected, EPS_FD * 10.0));
		}
	  }

	  /// <summary>
	  /// Test parameter sensitivity with finite difference sensitivity calculator. No cutoff period. </summary>
	  public virtual void rateChfNoCutOffParameterSensitivity()
	  {
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 1), date(2015, 1, 8)};
		DoubleArray time = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 10.0);
		DoubleArray rate = DoubleArray.of(0.0100, 0.0110, 0.0115, 0.0130, 0.0135, 0.0135);

		for (int loopvaldate = 0; loopvaldate < 2; loopvaldate++)
		{
		  Curve onCurve = InterpolatedNodalCurve.of(Curves.zeroRates("ON", ACT_ACT_ISDA), time, rate, INTERPOLATOR);
		  ImmutableRatesProvider prov = ImmutableRatesProvider.builder(valuationDate[loopvaldate]).overnightIndexCurve(CHF_TOIS, onCurve, TIME_SERIES).build();
		  OvernightAveragedRateComputation ro = OvernightAveragedRateComputation.of(CHF_TOIS, START_DATE, END_DATE, 0, REF_DATA);
		  ForwardOvernightAveragedRateComputationFn obsFn = ForwardOvernightAveragedRateComputationFn.DEFAULT;

		  PointSensitivityBuilder sensitivityBuilderComputed = obsFn.rateSensitivity(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, prov);
		  CurrencyParameterSensitivities parameterSensitivityComputed = prov.parameterSensitivity(sensitivityBuilderComputed.build());

		  CurrencyParameterSensitivities parameterSensitivityExpected = CAL_FD.sensitivity(prov, (p) => CurrencyAmount.of(CHF_TOIS.Currency, obsFn.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, (p))));
		  assertTrue(parameterSensitivityComputed.equalWithTolerance(parameterSensitivityExpected, EPS_FD * 10.0));
		}
	  }

	  private double?[] computedSensitivityFD(OvernightAveragedRateComputation ro, OvernightIndex index, OvernightIndexObservation[] indexObs)
	  {

		int nRates = FIXING_DATES.Length;
		OvernightIndexRates[] mockRatesUp = new OvernightIndexRates[nRates];
		SimpleRatesProvider[] simpleProvUp = new SimpleRatesProvider[nRates];
		OvernightIndexRates[] mockRatesDw = new OvernightIndexRates[nRates];
		SimpleRatesProvider[] simpleProvDw = new SimpleRatesProvider[nRates];
		double[][] ratesUp = new double[nRates][];
		double[][] ratesDw = new double[nRates][];
		for (int i = 0; i < nRates; ++i)
		{
		  mockRatesUp[i] = mock(typeof(OvernightIndexRates));
		  simpleProvUp[i] = new SimpleRatesProvider(mockRatesUp[i]);
		  mockRatesDw[i] = mock(typeof(OvernightIndexRates));
		  simpleProvDw[i] = new SimpleRatesProvider(mockRatesDw[i]);
		  ratesUp[i] = Arrays.copyOf(FIXING_RATES, nRates);
		  ratesDw[i] = Arrays.copyOf(FIXING_RATES, nRates);
		  ratesUp[i][i] += EPS_FD;
		  ratesDw[i][i] -= EPS_FD;
		}
		for (int i = 0; i < nRates; i++)
		{
		  for (int j = 0; j < nRates; ++j)
		  {
			when(mockRatesUp[j].rate(indexObs[i])).thenReturn(ratesUp[j][i]);
			when(mockRatesDw[j].rate(indexObs[i])).thenReturn(ratesDw[j][i]);
		  }
		}
		ForwardOvernightAveragedRateComputationFn obsFn = ForwardOvernightAveragedRateComputationFn.DEFAULT;
		IList<double> sensitivityExpected = new List<double>();
		for (int i = 0; i < nRates; ++i)
		{
		  double rateUp = obsFn.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProvUp[i]);
		  double rateDw = obsFn.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProvDw[i]);
		  double res = 0.5 * (rateUp - rateDw) / EPS_FD;
		  if (Math.Abs(res) > 1.0e-14)
		  {
			sensitivityExpected.Add(res);
		  }
		}
		int size = sensitivityExpected.Count;
		double?[] result = new double?[size];
		return sensitivityExpected.toArray(result);
	  }
	}

}