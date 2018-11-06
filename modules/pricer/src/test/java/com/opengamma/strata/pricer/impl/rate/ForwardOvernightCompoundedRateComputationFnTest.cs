/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
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
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
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
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using OvernightIndexRates = com.opengamma.strata.pricer.rate.OvernightIndexRates;
	using OvernightRateSensitivity = com.opengamma.strata.pricer.rate.OvernightRateSensitivity;
	using SimpleRatesProvider = com.opengamma.strata.pricer.rate.SimpleRatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using OvernightCompoundedRateComputation = com.opengamma.strata.product.rate.OvernightCompoundedRateComputation;

	/// <summary>
	/// Test <seealso cref="ForwardOvernightCompoundedRateComputationFn"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ForwardOvernightCompoundedRateComputationFnTest
	public class ForwardOvernightCompoundedRateComputationFnTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate DUMMY_ACCRUAL_START_DATE = date(2015, 1, 1); // Accrual dates irrelevant for the rate
	  private static readonly LocalDate DUMMY_ACCRUAL_END_DATE = date(2015, 1, 31); // Accrual dates irrelevant for the rate
	  private static readonly LocalDate FIXING_START_DATE = date(2015, 1, 8);
	  private static readonly LocalDate FIXING_END_DATE = date(2015, 1, 15); // 1w only to decrease data
	  private static readonly LocalDate FIXING_FINAL_DATE = date(2015, 1, 14);
	  private static readonly LocalDate[] FIXING_DATES = new LocalDate[] {date(2015, 1, 7), date(2015, 1, 8), date(2015, 1, 9), date(2015, 1, 12), date(2015, 1, 13), date(2015, 1, 14), date(2015, 1, 15)};
	  private static readonly OvernightIndexObservation[] USD_OBS = new OvernightIndexObservation[] {OvernightIndexObservation.of(USD_FED_FUND, date(2015, 1, 7), REF_DATA), OvernightIndexObservation.of(USD_FED_FUND, date(2015, 1, 8), REF_DATA), OvernightIndexObservation.of(USD_FED_FUND, date(2015, 1, 9), REF_DATA), OvernightIndexObservation.of(USD_FED_FUND, date(2015, 1, 12), REF_DATA), OvernightIndexObservation.of(USD_FED_FUND, date(2015, 1, 13), REF_DATA), OvernightIndexObservation.of(USD_FED_FUND, date(2015, 1, 14), REF_DATA), OvernightIndexObservation.of(USD_FED_FUND, date(2015, 1, 15), REF_DATA)};
	  private static readonly OvernightIndexObservation[] GBP_OBS = new OvernightIndexObservation[] {OvernightIndexObservation.of(GBP_SONIA, date(2015, 1, 7), REF_DATA), OvernightIndexObservation.of(GBP_SONIA, date(2015, 1, 8), REF_DATA), OvernightIndexObservation.of(GBP_SONIA, date(2015, 1, 9), REF_DATA), OvernightIndexObservation.of(GBP_SONIA, date(2015, 1, 12), REF_DATA), OvernightIndexObservation.of(GBP_SONIA, date(2015, 1, 13), REF_DATA), OvernightIndexObservation.of(GBP_SONIA, date(2015, 1, 14), REF_DATA), OvernightIndexObservation.of(GBP_SONIA, date(2015, 1, 15), REF_DATA)};
	  private static readonly OvernightIndexObservation[] CHF_OBS = new OvernightIndexObservation[] {OvernightIndexObservation.of(CHF_TOIS, date(2015, 1, 7), REF_DATA), OvernightIndexObservation.of(CHF_TOIS, date(2015, 1, 8), REF_DATA), OvernightIndexObservation.of(CHF_TOIS, date(2015, 1, 9), REF_DATA), OvernightIndexObservation.of(CHF_TOIS, date(2015, 1, 12), REF_DATA), OvernightIndexObservation.of(CHF_TOIS, date(2015, 1, 13), REF_DATA), OvernightIndexObservation.of(CHF_TOIS, date(2015, 1, 14), REF_DATA), OvernightIndexObservation.of(CHF_TOIS, date(2015, 1, 15), REF_DATA)};
	  private static readonly double[] FIXING_RATES = new double[] {0.0012, 0.0023, 0.0034, 0.0045, 0.0056, 0.0067, 0.0078};
	  private static readonly double[] FORWARD_RATES = new double[] {0.0112, 0.0123, 0.0134, 0.0145, 0.0156, 0.0167, 0.0178};

	  private const double TOLERANCE_RATE = 1.0E-10;
	  private const double EPS_FD = 1.0E-7;

	  private static readonly ForwardOvernightCompoundedRateComputationFn OBS_FWD_ONCMP = ForwardOvernightCompoundedRateComputationFn.DEFAULT;

	  /// <summary>
	  /// No cutoff period and the period entirely forward. Test the forward part only. </summary>
	  public virtual void rateFedFundNoCutOffForward()
	  { // publication=1, cutoff=0, effective offset=0, Forward
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 1), date(2015, 1, 8)};
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(USD_FED_FUND, FIXING_START_DATE, FIXING_END_DATE, 0, REF_DATA);
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(USD_FED_FUND);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		double rateCmp = 0.0123;
		when(mockRates.periodRate(USD_OBS[1], FIXING_END_DATE)).thenReturn(rateCmp);
		double rateExpected = rateCmp;
		for (int loopvaldate = 0; loopvaldate < 2; loopvaldate++)
		{
		  when(mockRates.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  double rateComputed = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		  assertEquals(rateExpected, rateComputed, TOLERANCE_RATE);
		}

		// explain
		ExplainMapBuilder builder = ExplainMap.builder();
		double explainedRate = OBS_FWD_ONCMP.explainRate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv, builder);
		assertEquals(explainedRate, rateExpected, TOLERANCE_RATE);

		ExplainMap built = builder.build();
		assertEquals(built.get(ExplainKey.OBSERVATIONS).Present, false);
		assertEquals(built.get(ExplainKey.COMBINED_RATE).Value.doubleValue(), rateExpected, TOLERANCE_RATE);
	  }

	  /// <summary>
	  /// No cutoff period and the period entirely forward. Test the forward part only against FD approximation. </summary>
	  public virtual void rateFedFundNoCutOffForwardSensitivity()
	  { // publication=1, cutoff=0, effective offset=0, Forward
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 1), date(2015, 1, 8)};
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(USD_FED_FUND, FIXING_START_DATE, FIXING_END_DATE, 0, REF_DATA);
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(USD_FED_FUND);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		double rateCmp = 0.0123;
		when(mockRates.periodRate(USD_OBS[1], FIXING_END_DATE)).thenReturn(rateCmp);
		PointSensitivityBuilder rateSensitivity = OvernightRateSensitivity.ofPeriod(USD_OBS[1], FIXING_END_DATE, 1.0);
		when(mockRates.periodRatePointSensitivity(USD_OBS[1], FIXING_END_DATE)).thenReturn(rateSensitivity);
		OvernightIndexRates mockRatesUp = mock(typeof(OvernightIndexRates));
		SimpleRatesProvider simpleProvUp = new SimpleRatesProvider(mockRatesUp);
		when(mockRatesUp.periodRate(USD_OBS[1], FIXING_END_DATE)).thenReturn(rateCmp + EPS_FD);
		OvernightIndexRates mockRatesDw = mock(typeof(OvernightIndexRates));
		SimpleRatesProvider simpleProvDw = new SimpleRatesProvider(mockRatesDw);
		when(mockRatesDw.periodRate(USD_OBS[1], FIXING_END_DATE)).thenReturn(rateCmp - EPS_FD);

		for (int loopvaldate = 0; loopvaldate < 2; loopvaldate++)
		{
		  when(mockRates.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  when(mockRatesUp.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  when(mockRatesDw.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  double rateUp = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProvUp);
		  double rateDw = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProvDw);
		  double sensitivityExpected = 0.5 * (rateUp - rateDw) / EPS_FD;
		  PointSensitivityBuilder sensitivityBuilderExpected = OvernightRateSensitivity.ofPeriod(USD_OBS[1], FIXING_END_DATE, sensitivityExpected);
		  PointSensitivityBuilder sensitivityBuilderComputed = OBS_FWD_ONCMP.rateSensitivity(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		  assertTrue(sensitivityBuilderComputed.build().normalized().equalWithTolerance(sensitivityBuilderExpected.build().normalized(), EPS_FD));
		}
	  }

	  /// <summary>
	  /// No cutoff period and the period entirely forward. One day period. </summary>
	  public virtual void rateFedFundNoCutOffForward1d()
	  { // publication=1, cutoff=0, effective offset=0, Forward
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 1), date(2015, 1, 8)};
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(USD_FED_FUND, FIXING_END_DATE.minusDays(1), FIXING_END_DATE, 0, REF_DATA);
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(USD_FED_FUND);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		double rateCmp = 0.0123;
		when(mockRates.periodRate(USD_OBS[5], FIXING_END_DATE)).thenReturn(rateCmp);
		double rateExpected = rateCmp;
		for (int loopvaldate = 0; loopvaldate < 2; loopvaldate++)
		{
		  when(mockRates.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  double rateComputed = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		  assertEquals(rateExpected, rateComputed, TOLERANCE_RATE);
		}
	  }

	  /// <summary>
	  /// No cutoff period and the period entirely forward. Test the forward part only against FD approximation. </summary>
	  public virtual void rateFedFundNoCutOffForwardSensitivit1dy()
	  { // publication=1, cutoff=0, effective offset=0, Forward
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 1), date(2015, 1, 8)};
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(USD_FED_FUND, FIXING_END_DATE.minusDays(1), FIXING_END_DATE, 0, REF_DATA);
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(USD_FED_FUND);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		double rateCmp = 0.0123;
		when(mockRates.periodRate(USD_OBS[5], FIXING_END_DATE)).thenReturn(rateCmp);
		PointSensitivityBuilder rateSensitivity = OvernightRateSensitivity.ofPeriod(USD_OBS[5], FIXING_END_DATE, 1.0);
		when(mockRates.periodRatePointSensitivity(USD_OBS[5], FIXING_END_DATE)).thenReturn(rateSensitivity);
		OvernightIndexRates mockRatesUp = mock(typeof(OvernightIndexRates));
		SimpleRatesProvider simpleProvUp = new SimpleRatesProvider(mockRatesUp);
		when(mockRatesUp.periodRate(USD_OBS[5], FIXING_END_DATE)).thenReturn(rateCmp + EPS_FD);
		OvernightIndexRates mockRatesDw = mock(typeof(OvernightIndexRates));
		SimpleRatesProvider simpleProvDw = new SimpleRatesProvider(mockRatesDw);
		when(mockRatesDw.periodRate(USD_OBS[5], FIXING_END_DATE)).thenReturn(rateCmp - EPS_FD);

		for (int loopvaldate = 0; loopvaldate < 2; loopvaldate++)
		{
		  when(mockRates.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  when(mockRatesUp.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  when(mockRatesDw.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  double rateUp = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProvUp);
		  double rateDw = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProvDw);
		  double sensitivityExpected = 0.5 * (rateUp - rateDw) / EPS_FD;
		  PointSensitivityBuilder sensitivityBuilderExpected = OvernightRateSensitivity.ofPeriod(USD_OBS[5], FIXING_END_DATE, sensitivityExpected);
		  PointSensitivityBuilder sensitivityBuilderComputed = OBS_FWD_ONCMP.rateSensitivity(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		  assertTrue(sensitivityBuilderComputed.build().normalized().equalWithTolerance(sensitivityBuilderExpected.build().normalized(), 10 * EPS_FD));
		}
	  }

	  /// <summary>
	  /// Two days cutoff and the period is entirely forward. Test forward part plus cutoff specifics.
	  /// Almost all Overnight Compounding coupon (OIS) don't use cutoff period.
	  /// </summary>
	  public virtual void rateFedFund2CutOffForward()
	  { // publication=1, cutoff=2, effective offset=0, Forward
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 1), date(2015, 1, 8)};
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(USD_FED_FUND, FIXING_START_DATE, FIXING_END_DATE, 2, REF_DATA);
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(USD_FED_FUND);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		for (int i = 0; i < FIXING_DATES.Length; i++)
		{
		  when(mockRates.rate(USD_OBS[i])).thenReturn(FORWARD_RATES[i]);
		}
		double investmentFactor = 1.0;
		double afNonCutoff = 0.0;
		for (int i = 1; i < 5; i++)
		{
		  LocalDate endDate = USD_FED_FUND.calculateMaturityFromEffective(FIXING_DATES[i], REF_DATA);
		  double af = USD_FED_FUND.DayCount.yearFraction(FIXING_DATES[i], endDate);
		  afNonCutoff += af;
		  investmentFactor *= 1.0d + af * FORWARD_RATES[i];
		}
		double rateCmp = (investmentFactor - 1.0d) / afNonCutoff;
		when(mockRates.periodRate(USD_OBS[1], FIXING_FINAL_DATE)).thenReturn(rateCmp);
		LocalDate fixingCutOff = FIXING_DATES[5];
		LocalDate endDate = USD_FED_FUND.calculateMaturityFromEffective(fixingCutOff, REF_DATA);
		double afCutOff = USD_FED_FUND.DayCount.yearFraction(fixingCutOff, endDate);
		double rateExpected = ((1.0 + rateCmp * afNonCutoff) * (1.0d + FORWARD_RATES[4] * afCutOff) - 1.0d) / (afNonCutoff + afCutOff);
		for (int loopvaldate = 0; loopvaldate < 2; loopvaldate++)
		{
		  when(mockRates.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  double rateComputed = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		  assertEquals(rateExpected, rateComputed, TOLERANCE_RATE);
		}
	  }

	  /// <summary>
	  /// Two days cutoff and the period is entirely forward. Test forward part plus cutoff specifics against FD.
	  /// Almost all Overnight Compounding coupon (OIS) don't use cutoff period.
	  /// </summary>
	  public virtual void rateFedFund2CutOffForwardSensitivity()
	  { // publication=1, cutoff=2, effective offset=0, Forward
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 1), date(2015, 1, 8)};
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(USD_FED_FUND, FIXING_START_DATE, FIXING_END_DATE, 2, REF_DATA);
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(USD_FED_FUND);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		int nFixings = FIXING_DATES.Length;
		OvernightIndexRates[] mockRatesUp = new OvernightIndexRates[nFixings];
		SimpleRatesProvider[] simpleProvUp = new SimpleRatesProvider[nFixings];
		OvernightIndexRates[] mockRatesDw = new OvernightIndexRates[nFixings];
		SimpleRatesProvider[] simpleProvDw = new SimpleRatesProvider[nFixings];
		OvernightIndexRates mockRatesPeriodUp = mock(typeof(OvernightIndexRates));
		SimpleRatesProvider simpleProvPeriodUp = new SimpleRatesProvider(mockRatesPeriodUp);
		OvernightIndexRates mockRatesPeriodDw = mock(typeof(OvernightIndexRates));
		SimpleRatesProvider simpleProvPeriodDw = new SimpleRatesProvider(mockRatesPeriodDw);

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] forwardRatesUp = new double[nFixings][nFixings];
		double[][] forwardRatesUp = RectangularArrays.ReturnRectangularDoubleArray(nFixings, nFixings);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] forwardRatesDw = new double[nFixings][nFixings];
		double[][] forwardRatesDw = RectangularArrays.ReturnRectangularDoubleArray(nFixings, nFixings);
		for (int i = 0; i < nFixings; i++)
		{
		  mockRatesUp[i] = mock(typeof(OvernightIndexRates));
		  simpleProvUp[i] = new SimpleRatesProvider(mockRatesUp[i]);
		  mockRatesDw[i] = mock(typeof(OvernightIndexRates));
		  simpleProvDw[i] = new SimpleRatesProvider(mockRatesDw[i]);
		  for (int j = 0; j < nFixings; j++)
		  {
			double rateForUp = i == j ? FORWARD_RATES[j] + EPS_FD : FORWARD_RATES[j];
			double rateForDw = i == j ? FORWARD_RATES[j] - EPS_FD : FORWARD_RATES[j];
			forwardRatesUp[i][j] = rateForUp;
			forwardRatesDw[i][j] = rateForDw;
		  }
		}
		for (int i = 0; i < nFixings; i++)
		{
		  when(mockRates.rate(USD_OBS[i])).thenReturn(FORWARD_RATES[i]);
		  when(mockRatesPeriodUp.rate(USD_OBS[i])).thenReturn(FORWARD_RATES[i]);
		  when(mockRatesPeriodDw.rate(USD_OBS[i])).thenReturn(FORWARD_RATES[i]);
		  LocalDate fixingStartDate = USD_FED_FUND.calculateEffectiveFromFixing(FIXING_DATES[i], REF_DATA);
		  LocalDate fixingEndDate = USD_FED_FUND.calculateMaturityFromEffective(fixingStartDate, REF_DATA);
		  PointSensitivityBuilder rateSensitivity = OvernightRateSensitivity.ofPeriod(USD_OBS[i], fixingEndDate, 1.0);
		  when(mockRates.ratePointSensitivity(USD_OBS[i])).thenReturn(rateSensitivity);
		  for (int j = 0; j < nFixings; ++j)
		  {
			when(mockRatesUp[j].rate(USD_OBS[i])).thenReturn(forwardRatesUp[j][i]);
			when(mockRatesDw[j].rate(USD_OBS[i])).thenReturn(forwardRatesDw[j][i]);
		  }
		}
		double investmentFactor = 1.0;
		double afNonCutoff = 0.0;
		for (int i = 1; i < 5; i++)
		{
		  LocalDate endDate = USD_FED_FUND.calculateMaturityFromEffective(FIXING_DATES[i], REF_DATA);
		  double af = USD_FED_FUND.DayCount.yearFraction(FIXING_DATES[i], endDate);
		  afNonCutoff += af;
		  investmentFactor *= 1.0d + af * FORWARD_RATES[i];
		}
		double rateCmp = (investmentFactor - 1.0d) / afNonCutoff;
		when(mockRates.periodRate(USD_OBS[1], FIXING_FINAL_DATE)).thenReturn(rateCmp);
		when(mockRatesPeriodUp.periodRate(USD_OBS[1], FIXING_FINAL_DATE)).thenReturn(rateCmp + EPS_FD);
		when(mockRatesPeriodDw.periodRate(USD_OBS[1], FIXING_FINAL_DATE)).thenReturn(rateCmp - EPS_FD);
		PointSensitivityBuilder rateSensitivity = OvernightRateSensitivity.ofPeriod(USD_OBS[1], FIXING_FINAL_DATE, 1.0);
		when(mockRates.periodRatePointSensitivity(USD_OBS[1], FIXING_FINAL_DATE)).thenReturn(rateSensitivity);
		for (int i = 0; i < nFixings; ++i)
		{
		  when(mockRatesUp[i].periodRate(USD_OBS[1], FIXING_FINAL_DATE)).thenReturn(rateCmp);
		  when(mockRatesDw[i].periodRate(USD_OBS[1], FIXING_FINAL_DATE)).thenReturn(rateCmp);
		}

		for (int loopvaldate = 0; loopvaldate < 2; loopvaldate++)
		{
		  when(mockRates.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  when(mockRatesPeriodUp.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  when(mockRatesPeriodDw.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  PointSensitivityBuilder sensitivityBuilderExpected1 = PointSensitivityBuilder.none();
		  for (int i = 0; i < nFixings; ++i)
		  {
			when(mockRatesUp[i].ValuationDate).thenReturn(valuationDate[loopvaldate]);
			when(mockRatesDw[i].ValuationDate).thenReturn(valuationDate[loopvaldate]);
			double rateUp = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProvUp[i]);
			double rateDw = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProvDw[i]);
			double cutoffSensitivity = 0.5 * (rateUp - rateDw) / EPS_FD; // [4] is nonzero
			LocalDate fixingStartDate = USD_FED_FUND.calculateEffectiveFromFixing(FIXING_DATES[i], REF_DATA);
			LocalDate fixingEndDate = USD_FED_FUND.calculateMaturityFromEffective(fixingStartDate, REF_DATA);
			sensitivityBuilderExpected1 = cutoffSensitivity == 0.0 ? sensitivityBuilderExpected1 : sensitivityBuilderExpected1.combinedWith(OvernightRateSensitivity.ofPeriod(USD_OBS[i], fixingEndDate, cutoffSensitivity));
		  }
		  double ratePeriodUp = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProvPeriodUp);
		  double ratePeriodDw = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProvPeriodDw);
		  double periodSensitivity = 0.5 * (ratePeriodUp - ratePeriodDw) / EPS_FD;
		  PointSensitivityBuilder sensitivityBuilderExpected2 = OvernightRateSensitivity.ofPeriod(USD_OBS[1], FIXING_FINAL_DATE, periodSensitivity);
		  PointSensitivityBuilder sensitivityBuilderExpected = sensitivityBuilderExpected1.combinedWith(sensitivityBuilderExpected2);
		  PointSensitivityBuilder sensitivityBuilderComputed = OBS_FWD_ONCMP.rateSensitivity(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		  assertTrue(sensitivityBuilderComputed.build().normalized().equalWithTolerance(sensitivityBuilderExpected.build().normalized(), EPS_FD));
		}
	  }

	  /// <summary>
	  /// No cutoff and one already fixed ON rate. Test the already fixed portion with only one fixed ON rate. </summary>
	  public virtual void rateFedFund0CutOffValuation1()
	  {
		// publication=1, cutoff=0, effective offset=0, TS: Fixing 1
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 9), date(2015, 1, 12)};
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(USD_FED_FUND, FIXING_START_DATE, FIXING_END_DATE, 0, REF_DATA);
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(USD_FED_FUND);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		LocalDateDoubleTimeSeriesBuilder tsb = LocalDateDoubleTimeSeries.builder();
		for (int i = 0; i < 2; i++)
		{
		  tsb.put(FIXING_DATES[i], FIXING_RATES[i]);
		}
		when(mockRates.Fixings).thenReturn(tsb.build());
		for (int i = 0; i < 2; i++)
		{
		  when(mockRates.rate(USD_OBS[i])).thenReturn(FIXING_RATES[i]);
		}
		for (int i = 2; i < USD_OBS.Length; i++)
		{
		  when(mockRates.rate(USD_OBS[i])).thenReturn(FORWARD_RATES[i]);
		}
		LocalDate fixingknown = FIXING_DATES[1];
		LocalDate endDateKnown = USD_FED_FUND.calculateMaturityFromEffective(fixingknown, REF_DATA);
		double afKnown = USD_FED_FUND.DayCount.yearFraction(fixingknown, endDateKnown);
		double investmentFactor = 1.0;
		double afNoCutoff = 0.0;
		for (int i = 2; i < 6; i++)
		{
		  LocalDate endDate = USD_FED_FUND.calculateMaturityFromEffective(FIXING_DATES[i], REF_DATA);
		  double af = USD_FED_FUND.DayCount.yearFraction(FIXING_DATES[i], endDate);
		  afNoCutoff += af;
		  investmentFactor *= 1.0d + af * FORWARD_RATES[i];
		}
		double rateCmp = (investmentFactor - 1.0d) / afNoCutoff;
		when(mockRates.periodRate(USD_OBS[2], FIXING_END_DATE)).thenReturn(rateCmp);
		double rateExpected = ((1.0d + FIXING_RATES[1] * afKnown) * (1.0 + rateCmp * afNoCutoff) - 1.0d) / (afKnown + afNoCutoff);
		for (int loopvaldate = 0; loopvaldate < 2; loopvaldate++)
		{
		  when(mockRates.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  double rateComputed = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		  assertEquals(rateExpected, rateComputed, TOLERANCE_RATE);
		}
	  }

	  /// <summary>
	  /// No cutoff and one already fixed ON rate. Test the already fixed portion with only one fixed ON rate against FD. </summary>
	  public virtual void rateFedFund0CutOffValuation1Sensitivity()
	  {
		// publication=1, cutoff=0, effective offset=0, TS: Fixing 1
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 9), date(2015, 1, 12)};
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(USD_FED_FUND, FIXING_START_DATE, FIXING_END_DATE, 0, REF_DATA);
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(USD_FED_FUND);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		LocalDateDoubleTimeSeriesBuilder tsb = LocalDateDoubleTimeSeries.builder();
		for (int i = 0; i < 2; i++)
		{
		  tsb.put(FIXING_DATES[i], FIXING_RATES[i]);
		}
		when(mockRates.Fixings).thenReturn(tsb.build());

		OvernightIndexRates mockRatesUp = mock(typeof(OvernightIndexRates));
		SimpleRatesProvider simpleProvUp = new SimpleRatesProvider(mockRatesUp);
		OvernightIndexRates mockRatesDw = mock(typeof(OvernightIndexRates));
		SimpleRatesProvider simpleProvDw = new SimpleRatesProvider(mockRatesDw);
		when(mockRatesUp.Fixings).thenReturn(tsb.build());
		when(mockRatesDw.Fixings).thenReturn(tsb.build());
		double investmentFactor = 1.0;
		double afNoCutoff = 0.0;
		for (int i = 2; i < 6; i++)
		{
		  LocalDate endDate = USD_FED_FUND.calculateMaturityFromEffective(FIXING_DATES[i], REF_DATA);
		  double af = USD_FED_FUND.DayCount.yearFraction(FIXING_DATES[i], endDate);
		  afNoCutoff += af;
		  investmentFactor *= 1.0d + af * FORWARD_RATES[i];
		}
		double rateCmp = (investmentFactor - 1.0d) / afNoCutoff;
		when(mockRates.periodRate(USD_OBS[2], FIXING_END_DATE)).thenReturn(rateCmp);
		when(mockRatesUp.periodRate(USD_OBS[2], FIXING_END_DATE)).thenReturn(rateCmp + EPS_FD);
		when(mockRatesDw.periodRate(USD_OBS[2], FIXING_END_DATE)).thenReturn(rateCmp - EPS_FD);
		PointSensitivityBuilder periodSensitivity = OvernightRateSensitivity.ofPeriod(USD_OBS[2], FIXING_END_DATE, 1.0d);
		when(mockRates.periodRatePointSensitivity(USD_OBS[2], FIXING_END_DATE)).thenReturn(periodSensitivity);
		for (int loopvaldate = 0; loopvaldate < 2; loopvaldate++)
		{
		  when(mockRates.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  when(mockRatesUp.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  when(mockRatesDw.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  double rateUp = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProvUp);
		  double rateDw = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProvDw);
		  double sensitivityExpected = 0.5 * (rateUp - rateDw) / EPS_FD;
		  PointSensitivityBuilder sensitivityBuilderExpected = OvernightRateSensitivity.ofPeriod(USD_OBS[2], FIXING_END_DATE, sensitivityExpected);
		  PointSensitivityBuilder sensitivityBuilderComputed = OBS_FWD_ONCMP.rateSensitivity(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		  assertTrue(sensitivityBuilderComputed.build().normalized().equalWithTolerance(sensitivityBuilderExpected.build().normalized(), EPS_FD));
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// No cutoff period and two already fixed ON rate. ON index is SONIA. </summary>
	  public virtual void rateSonia0CutOffValuation2()
	  {
		// publication=0, cutoff=0, effective offset=0, TS: Fixing 2
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 9), date(2015, 1, 12)};
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(GBP_SONIA, FIXING_START_DATE, FIXING_END_DATE, 0, REF_DATA);
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(GBP_SONIA);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		LocalDateDoubleTimeSeriesBuilder tsb = LocalDateDoubleTimeSeries.builder();
		int lastFixing = 3;
		for (int i = 0; i < lastFixing; i++)
		{
		  tsb.put(FIXING_DATES[i], FIXING_RATES[i]);
		}
		when(mockRates.Fixings).thenReturn(tsb.build());
		for (int i = 0; i < lastFixing; i++)
		{
		  when(mockRates.rate(GBP_OBS[i])).thenReturn(FIXING_RATES[i]);
		}
		for (int i = lastFixing; i < GBP_OBS.Length; i++)
		{
		  when(mockRates.rate(GBP_OBS[i])).thenReturn(FORWARD_RATES[i]);
		}
		double afKnown = 0.0d;
		double investmentFactorKnown = 1.0d;
		for (int i = 0; i < lastFixing - 1; i++)
		{
		  LocalDate fixingknown = FIXING_DATES[i + 1];
		  LocalDate endDateKnown = GBP_SONIA.calculateMaturityFromEffective(fixingknown, REF_DATA);
		  double af = GBP_SONIA.DayCount.yearFraction(fixingknown, endDateKnown);
		  afKnown += af;
		  investmentFactorKnown *= 1.0d + FIXING_RATES[i + 1] * af;
		}
		double afNoCutoff = 0.0d;
		double investmentFactorNoCutoff = 1.0d;
		for (int i = lastFixing; i < 6; i++)
		{
		  LocalDate endDate = GBP_SONIA.calculateMaturityFromEffective(FIXING_DATES[i], REF_DATA);
		  double af = GBP_SONIA.DayCount.yearFraction(FIXING_DATES[i], endDate);
		  afNoCutoff += af;
		  investmentFactorNoCutoff *= 1.0d + af * FORWARD_RATES[i];
		}
		double rateCmp = (investmentFactorNoCutoff - 1.0d) / afNoCutoff;
		when(mockRates.periodRate(GBP_OBS[lastFixing], FIXING_DATES[6])).thenReturn(rateCmp);
		double rateExpected = (investmentFactorKnown * (1.0 + rateCmp * afNoCutoff) - 1.0d) / (afKnown + afNoCutoff);
		for (int loopvaldate = 0; loopvaldate < 2; loopvaldate++)
		{
		  when(mockRates.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  double rateComputed = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		  assertEquals(rateExpected, rateComputed, TOLERANCE_RATE);
		}
	  }

	  /// <summary>
	  /// Test rate sensitivity against FD approximation.
	  /// No cutoff period and two already fixed ON rate. ON index is SONIA. 
	  /// </summary>
	  public virtual void rateSonia0CutOffValuation2Sensitivity()
	  {
		// publication=0, cutoff=0, effective offset=0, TS: Fixing 2
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 9), date(2015, 1, 12)};
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(GBP_SONIA, FIXING_START_DATE, FIXING_END_DATE, 0, REF_DATA);
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(GBP_SONIA);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		LocalDateDoubleTimeSeriesBuilder tsb = LocalDateDoubleTimeSeries.builder();
		int lastFixing = 3;
		for (int i = 0; i < lastFixing; i++)
		{
		  tsb.put(FIXING_DATES[i], FIXING_RATES[i]);
		}
		when(mockRates.Fixings).thenReturn(tsb.build());

		OvernightIndexRates mockRatesUp = mock(typeof(OvernightIndexRates));
		SimpleRatesProvider simpleProvUp = new SimpleRatesProvider(mockRatesUp);
		OvernightIndexRates mockRatesDw = mock(typeof(OvernightIndexRates));
		SimpleRatesProvider simpleProvDw = new SimpleRatesProvider(mockRatesDw);
		when(mockRatesUp.Fixings).thenReturn(tsb.build());
		when(mockRatesDw.Fixings).thenReturn(tsb.build());
		double afNoCutoff = 0.0d;
		double investmentFactorNoCutoff = 1.0d;
		for (int i = lastFixing; i < 6; i++)
		{
		  LocalDate endDate = GBP_SONIA.calculateMaturityFromEffective(FIXING_DATES[i], REF_DATA);
		  double af = GBP_SONIA.DayCount.yearFraction(FIXING_DATES[i], endDate);
		  afNoCutoff += af;
		  investmentFactorNoCutoff *= 1.0d + af * FORWARD_RATES[i];
		}
		double rateCmp = (investmentFactorNoCutoff - 1.0d) / afNoCutoff;
		when(mockRates.periodRate(GBP_OBS[lastFixing], FIXING_DATES[6])).thenReturn(rateCmp);
		when(mockRatesUp.periodRate(GBP_OBS[lastFixing], FIXING_DATES[6])).thenReturn(rateCmp + EPS_FD);
		when(mockRatesDw.periodRate(GBP_OBS[lastFixing], FIXING_DATES[6])).thenReturn(rateCmp - EPS_FD);
		OvernightRateSensitivity periodSensitivity = OvernightRateSensitivity.ofPeriod(GBP_OBS[lastFixing], FIXING_DATES[6], 1.0d);
		when(mockRates.periodRatePointSensitivity(GBP_OBS[lastFixing], FIXING_DATES[6])).thenReturn(periodSensitivity);
		for (int loopvaldate = 0; loopvaldate < 2; loopvaldate++)
		{
		  when(mockRates.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  when(mockRatesUp.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  when(mockRatesDw.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  double rateUp = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProvUp);
		  double rateDw = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProvDw);
		  double sensitivityExpected = 0.5 * (rateUp - rateDw) / EPS_FD;
		  OvernightRateSensitivity sensitivityBuilderExpected = OvernightRateSensitivity.ofPeriod(GBP_OBS[lastFixing], FIXING_DATES[6], sensitivityExpected);
		  PointSensitivityBuilder sensitivityBuilderComputed = OBS_FWD_ONCMP.rateSensitivity(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		  assertTrue(sensitivityBuilderComputed.build().normalized().equalWithTolerance(sensitivityBuilderExpected.build().normalized(), EPS_FD));
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// No cutoff period and two already fixed ON rate. ON index is TOIS (with a effective offset of 1; TN rate). </summary>
	  public virtual void rateTois0CutOffValuation2()
	  {
		// publication=0, cutoff=0, effective offset=1, TS: Fixing 2
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 9), date(2015, 1, 12)};
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(CHF_TOIS, CHF_TOIS.calculateEffectiveFromFixing(FIXING_START_DATE, REF_DATA), CHF_TOIS.calculateEffectiveFromFixing(FIXING_END_DATE, REF_DATA), 0, REF_DATA);
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(CHF_TOIS);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		LocalDateDoubleTimeSeriesBuilder tsb = LocalDateDoubleTimeSeries.builder();
		int lastFixing = 3;
		for (int i = 0; i < lastFixing; i++)
		{
		  tsb.put(FIXING_DATES[i], FIXING_RATES[i]);
		}
		when(mockRates.Fixings).thenReturn(tsb.build());
		for (int i = 0; i < lastFixing; i++)
		{
		  when(mockRates.rate(CHF_OBS[i])).thenReturn(FIXING_RATES[i]);
		}
		for (int i = lastFixing; i < CHF_OBS.Length; i++)
		{
		  when(mockRates.rate(CHF_OBS[i])).thenReturn(FORWARD_RATES[i]);
		}
		double afKnown = 0.0d;
		double investmentFactorKnown = 1.0d;
		for (int i = 1; i < lastFixing; i++)
		{
		  LocalDate fixingknown = FIXING_DATES[i];
		  LocalDate startDateKnown = CHF_TOIS.calculateEffectiveFromFixing(fixingknown, REF_DATA);
		  LocalDate endDateKnown = CHF_TOIS.calculateMaturityFromEffective(startDateKnown, REF_DATA);
		  double af = CHF_TOIS.DayCount.yearFraction(startDateKnown, endDateKnown);
		  afKnown += af;
		  investmentFactorKnown *= 1.0d + af * FIXING_RATES[i];
		}
		double afNoCutoff = 0.0d;
		double investmentFactorNoCutoff = 1.0d;
		for (int i = lastFixing; i < 6; i++)
		{
		  LocalDate fixing = FIXING_DATES[i];
		  LocalDate startDate = CHF_TOIS.calculateEffectiveFromFixing(fixing, REF_DATA);
		  LocalDate endDate = CHF_TOIS.calculateMaturityFromEffective(startDate, REF_DATA);
		  double af = CHF_TOIS.DayCount.yearFraction(startDate, endDate);
		  afNoCutoff += af;
		  investmentFactorNoCutoff *= 1.0d + af * FORWARD_RATES[i];
		}
		double rateCmp = (investmentFactorNoCutoff - 1.0d) / afNoCutoff;
		LocalDate dateMat = CHF_TOIS.calculateMaturityFromFixing(FIXING_DATES[5], REF_DATA);
		OvernightIndexObservation obs = OvernightIndexObservation.of(CHF_TOIS, FIXING_DATES[lastFixing], REF_DATA);
		when(mockRates.periodRate(obs, dateMat)).thenReturn(rateCmp);
		double rateExpected = (investmentFactorKnown * (1.0 + rateCmp * afNoCutoff) - 1.0d) / (afKnown + afNoCutoff);
		for (int loopvaldate = 0; loopvaldate < 2; loopvaldate++)
		{
		  when(mockRates.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  double rateComputed = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		  assertEquals(rateExpected, rateComputed, TOLERANCE_RATE);
		}
	  }

	  /// <summary>
	  /// Test rate sensitivity against FD approximation.
	  /// No cutoff period and two already fixed ON rate. ON index is TOIS (with a effective offset of 1; TN rate). 
	  /// </summary>
	  public virtual void rateTois0CutOffValuation2Sensitivity()
	  {
		// publication=0, cutoff=0, effective offset=1, TS: Fixing 2
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 9), date(2015, 1, 12)};
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(CHF_TOIS, CHF_TOIS.calculateEffectiveFromFixing(FIXING_START_DATE, REF_DATA), CHF_TOIS.calculateEffectiveFromFixing(FIXING_END_DATE, REF_DATA), 0, REF_DATA);
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(CHF_TOIS);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		LocalDateDoubleTimeSeriesBuilder tsb = LocalDateDoubleTimeSeries.builder();
		int lastFixing = 3;
		for (int i = 0; i < lastFixing; i++)
		{
		  tsb.put(FIXING_DATES[i], FIXING_RATES[i]);
		}
		when(mockRates.Fixings).thenReturn(tsb.build());

		OvernightIndexRates mockRatesUp = mock(typeof(OvernightIndexRates));
		SimpleRatesProvider simpleProvUp = new SimpleRatesProvider(mockRatesUp);
		OvernightIndexRates mockRatesDw = mock(typeof(OvernightIndexRates));
		SimpleRatesProvider simpleProvDw = new SimpleRatesProvider(mockRatesDw);
		when(mockRatesUp.Fixings).thenReturn(tsb.build());
		when(mockRatesDw.Fixings).thenReturn(tsb.build());
		double afNoCutoff = 0.0d;
		double investmentFactorNoCutoff = 1.0d;
		for (int i = lastFixing; i < 6; i++)
		{
		  LocalDate fixing = FIXING_DATES[i];
		  LocalDate startDate = CHF_TOIS.calculateEffectiveFromFixing(fixing, REF_DATA);
		  LocalDate endDate = CHF_TOIS.calculateMaturityFromEffective(startDate, REF_DATA);
		  double af = CHF_TOIS.DayCount.yearFraction(startDate, endDate);
		  afNoCutoff += af;
		  investmentFactorNoCutoff *= 1.0d + af * FORWARD_RATES[i];
		}
		double rateCmp = (investmentFactorNoCutoff - 1.0d) / afNoCutoff;
		LocalDate dateMat = CHF_TOIS.calculateMaturityFromFixing(FIXING_DATES[5], REF_DATA);
		OvernightIndexObservation obs = OvernightIndexObservation.of(CHF_TOIS, FIXING_DATES[lastFixing], REF_DATA);
		when(mockRates.periodRate(obs, dateMat)).thenReturn(rateCmp);
		when(mockRatesUp.periodRate(obs, dateMat)).thenReturn(rateCmp + EPS_FD);
		when(mockRatesDw.periodRate(obs, dateMat)).thenReturn(rateCmp - EPS_FD);
		OvernightRateSensitivity periodSensitivity = OvernightRateSensitivity.ofPeriod(obs, dateMat, 1.0d);
		when(mockRates.periodRatePointSensitivity(obs, dateMat)).thenReturn(periodSensitivity);
		for (int loopvaldate = 0; loopvaldate < 2; loopvaldate++)
		{
		  when(mockRates.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  when(mockRatesUp.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  when(mockRatesDw.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  double rateUp = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProvUp);
		  double rateDw = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProvDw);
		  double sensitivityExpected = 0.5 * (rateUp - rateDw) / EPS_FD;
		  OvernightRateSensitivity sensitivityBuilderExpected = OvernightRateSensitivity.ofPeriod(obs, dateMat, sensitivityExpected);
		  PointSensitivityBuilder sensitivityBuilderComputed = OBS_FWD_ONCMP.rateSensitivity(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		  assertTrue(sensitivityBuilderComputed.build().normalized().equalWithTolerance(sensitivityBuilderExpected.build().normalized(), EPS_FD));
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// No cutoff and two already fixed ON rate. ON index is Fed Fund. </summary>
	  public virtual void rateFedFund0CutOffValuation2()
	  {
		// publication=1, cutoff=0, effective offset=0, TS: Fixing 2
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 12), date(2015, 1, 13)};
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(USD_FED_FUND, FIXING_START_DATE, FIXING_END_DATE, 0, REF_DATA);
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(USD_FED_FUND);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		LocalDateDoubleTimeSeriesBuilder tsb = LocalDateDoubleTimeSeries.builder();
		int lastFixing = 3;
		for (int i = 0; i < lastFixing; i++)
		{
		  tsb.put(FIXING_DATES[i], FIXING_RATES[i]);
		}
		when(mockRates.Fixings).thenReturn(tsb.build());
		for (int i = 0; i < lastFixing; i++)
		{
		  when(mockRates.rate(USD_OBS[i])).thenReturn(FIXING_RATES[i]);
		}
		for (int i = lastFixing; i < USD_OBS.Length; i++)
		{
		  when(mockRates.rate(USD_OBS[i])).thenReturn(FORWARD_RATES[i]);
		}
		double afKnown = 0.0d;
		double investmentFactorKnown = 1.0d;
		for (int i = 0; i < lastFixing - 1; i++)
		{
		  LocalDate fixingknown = FIXING_DATES[i + 1];
		  LocalDate endDateKnown = USD_FED_FUND.calculateMaturityFromEffective(fixingknown, REF_DATA);
		  double af = USD_FED_FUND.DayCount.yearFraction(fixingknown, endDateKnown);
		  afKnown += af;
		  investmentFactorKnown *= 1.0d + FIXING_RATES[i + 1] * af;
		}
		double investmentFactor = 1.0;
		double afNoCutoff = 0.0;
		for (int i = lastFixing; i < 6; i++)
		{
		  LocalDate endDate = USD_FED_FUND.calculateMaturityFromEffective(FIXING_DATES[i], REF_DATA);
		  double af = USD_FED_FUND.DayCount.yearFraction(FIXING_DATES[i], endDate);
		  afNoCutoff += af;
		  investmentFactor *= 1.0d + af * FORWARD_RATES[i];
		}
		double rateCmp = (investmentFactor - 1.0d) / afNoCutoff;
		when(mockRates.periodRate(USD_OBS[lastFixing], FIXING_DATES[6])).thenReturn(rateCmp);
		double rateExpected = (investmentFactorKnown * (1.0 + rateCmp * afNoCutoff) - 1.0d) / (afKnown + afNoCutoff);
		for (int loopvaldate = 0; loopvaldate < 2; loopvaldate++)
		{
		  when(mockRates.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  double rateComputed = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		  assertEquals(rateExpected, rateComputed, TOLERANCE_RATE);
		}
	  }

	  /// <summary>
	  /// Test rate sensitivity against FD approximation.
	  /// No cutoff and two already fixed ON rate. ON index is Fed Fund. 
	  /// </summary>
	  public virtual void rateFedFund0CutOffValuation2Sensitivity()
	  {
		// publication=1, cutoff=0, effective offset=0, TS: Fixing 2
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 12), date(2015, 1, 13)};
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(USD_FED_FUND, FIXING_START_DATE, FIXING_END_DATE, 0, REF_DATA);
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(USD_FED_FUND);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		LocalDateDoubleTimeSeriesBuilder tsb = LocalDateDoubleTimeSeries.builder();
		int lastFixing = 3;
		for (int i = 0; i < lastFixing; i++)
		{
		  tsb.put(FIXING_DATES[i], FIXING_RATES[i]);
		}
		when(mockRates.Fixings).thenReturn(tsb.build());

		OvernightIndexRates mockRatesUp = mock(typeof(OvernightIndexRates));
		SimpleRatesProvider simpleProvUp = new SimpleRatesProvider(mockRatesUp);
		OvernightIndexRates mockRatesDw = mock(typeof(OvernightIndexRates));
		SimpleRatesProvider simpleProvDw = new SimpleRatesProvider(mockRatesDw);
		when(mockRatesUp.Fixings).thenReturn(tsb.build());
		when(mockRatesDw.Fixings).thenReturn(tsb.build());
		double investmentFactor = 1.0;
		double afNoCutoff = 0.0;
		for (int i = lastFixing; i < 6; i++)
		{
		  LocalDate endDate = USD_FED_FUND.calculateMaturityFromEffective(FIXING_DATES[i], REF_DATA);
		  double af = USD_FED_FUND.DayCount.yearFraction(FIXING_DATES[i], endDate);
		  afNoCutoff += af;
		  investmentFactor *= 1.0d + af * FORWARD_RATES[i];
		}
		double rateCmp = (investmentFactor - 1.0d) / afNoCutoff;
		when(mockRates.periodRate(USD_OBS[lastFixing], FIXING_DATES[6])).thenReturn(rateCmp);
		when(mockRatesUp.periodRate(USD_OBS[lastFixing], FIXING_DATES[6])).thenReturn(rateCmp + EPS_FD);
		when(mockRatesDw.periodRate(USD_OBS[lastFixing], FIXING_DATES[6])).thenReturn(rateCmp - EPS_FD);
		OvernightRateSensitivity periodSensitivity = OvernightRateSensitivity.ofPeriod(USD_OBS[lastFixing], FIXING_DATES[6], 1.0d);
		when(mockRates.periodRatePointSensitivity(USD_OBS[lastFixing], FIXING_DATES[6])).thenReturn(periodSensitivity);
		for (int loopvaldate = 0; loopvaldate < 2; loopvaldate++)
		{
		  when(mockRates.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  when(mockRatesUp.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  when(mockRatesDw.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  double rateUp = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProvUp);
		  double rateDw = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProvDw);
		  double sensitivityExpected = 0.5 * (rateUp - rateDw) / EPS_FD;
		  OvernightRateSensitivity sensitivityBuilderExpected = OvernightRateSensitivity.ofPeriod(USD_OBS[lastFixing], FIXING_DATES[6], sensitivityExpected);
		  PointSensitivityBuilder sensitivityBuilderComputed = OBS_FWD_ONCMP.rateSensitivity(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);

		  assertTrue(sensitivityBuilderComputed.build().normalized().equalWithTolerance(sensitivityBuilderExpected.build().normalized(), EPS_FD));
		}
	  }

	  /// <summary>
	  /// No cutoff, all ON rates already fixed. Time series up to 14-Jan (last fixing date used). </summary>
	  public virtual void rateFedFund0CutOffValuationEndTs14()
	  {
		// publication=1, cutoff=0, effective offset=0, TS: Fixing all
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 15), date(2015, 1, 16), date(2015, 1, 17)};
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(USD_FED_FUND, FIXING_START_DATE, FIXING_END_DATE, 0, REF_DATA);
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(USD_FED_FUND);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		LocalDateDoubleTimeSeriesBuilder tsb = LocalDateDoubleTimeSeries.builder();
		int lastFixing = 6;
		for (int i = 0; i < lastFixing; i++)
		{
		  tsb.put(FIXING_DATES[i], FIXING_RATES[i]);
		}
		when(mockRates.Fixings).thenReturn(tsb.build());
		for (int i = 0; i < lastFixing; i++)
		{
		  when(mockRates.rate(USD_OBS[i])).thenReturn(FIXING_RATES[i]);
		}
		for (int i = lastFixing; i < USD_OBS.Length; i++)
		{
		  when(mockRates.rate(USD_OBS[i])).thenReturn(FORWARD_RATES[i]);
		}
		double afKnown = 0.0d;
		double investmentFactorKnown = 1.0d;
		for (int i = 0; i < 5; i++)
		{
		  LocalDate fixingknown = FIXING_DATES[i + 1];
		  LocalDate endDateKnown = USD_FED_FUND.calculateMaturityFromEffective(fixingknown, REF_DATA);
		  double af = USD_FED_FUND.DayCount.yearFraction(fixingknown, endDateKnown);
		  afKnown += af;
		  investmentFactorKnown *= 1.0d + FIXING_RATES[i + 1] * af;
		}
		double rateExpected = (investmentFactorKnown - 1.0d) / afKnown;
		for (int loopvaldate = 0; loopvaldate < valuationDate.Length; loopvaldate++)
		{
		  when(mockRates.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  double rateComputed = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		  assertEquals(rateExpected, rateComputed, TOLERANCE_RATE);
		}
	  }

	  /// <summary>
	  /// Test rate sensitivity. No cutoff, all ON rates already fixed. Thus expected sensitivity is none.
	  /// Time series up to 14-Jan (last fixing date used). 
	  /// </summary>
	  public virtual void rateFedFund0CutOffValuationEndTs14Sensitivity()
	  {
		// publication=1, cutoff=0, effective offset=0, TS: Fixing all
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 15), date(2015, 1, 16), date(2015, 1, 17)};
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(USD_FED_FUND, FIXING_START_DATE, FIXING_END_DATE, 0, REF_DATA);
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(USD_FED_FUND);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		LocalDateDoubleTimeSeriesBuilder tsb = LocalDateDoubleTimeSeries.builder();
		int lastFixing = 6;
		for (int i = 0; i < lastFixing; i++)
		{
		  tsb.put(FIXING_DATES[i], FIXING_RATES[i]);
		}
		when(mockRates.Fixings).thenReturn(tsb.build());
		for (int loopvaldate = 0; loopvaldate < valuationDate.Length; loopvaldate++)
		{
		  when(mockRates.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  PointSensitivityBuilder sensitivityComputed = OBS_FWD_ONCMP.rateSensitivity(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		  assertEquals(sensitivityComputed, PointSensitivityBuilder.none());
		}
	  }

	  /// <summary>
	  /// No cutoff, all ON rates already fixed. Time series up to 15-Jan (one day after the last fixing date). </summary>
	  public virtual void rateFedFund0CutOffValuationEndTs15()
	  {
		// publication=1, cutoff=0, effective offset=0, TS: Fixing all
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 16), date(2015, 1, 17)};
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(USD_FED_FUND, FIXING_START_DATE, FIXING_END_DATE, 0, REF_DATA);
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(USD_FED_FUND);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		LocalDateDoubleTimeSeriesBuilder tsb = LocalDateDoubleTimeSeries.builder();
		int lastFixing = 7;
		for (int i = 0; i < lastFixing; i++)
		{
		  tsb.put(FIXING_DATES[i], FIXING_RATES[i]);
		}
		when(mockRates.Fixings).thenReturn(tsb.build());
		for (int i = 0; i < lastFixing; i++)
		{
		  when(mockRates.rate(USD_OBS[i])).thenReturn(FIXING_RATES[i]);
		}
		for (int i = lastFixing; i < USD_OBS.Length; i++)
		{
		  when(mockRates.rate(USD_OBS[i])).thenReturn(FORWARD_RATES[i]);
		}
		double afKnown = 0.0d;
		double investmentFactorKnown = 1.0d;
		for (int i = 0; i < 5; i++)
		{
		  LocalDate fixingknown = FIXING_DATES[i + 1];
		  LocalDate endDateKnown = USD_FED_FUND.calculateMaturityFromEffective(fixingknown, REF_DATA);
		  double af = USD_FED_FUND.DayCount.yearFraction(fixingknown, endDateKnown);
		  afKnown += af;
		  investmentFactorKnown *= 1.0d + FIXING_RATES[i + 1] * af;
		}
		double rateExpected = (investmentFactorKnown - 1.0d) / afKnown;
		for (int loopvaldate = 0; loopvaldate < valuationDate.Length; loopvaldate++)
		{
		  when(mockRates.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  double rateComputed = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		  assertEquals(rateExpected, rateComputed, TOLERANCE_RATE);
		}
	  }

	  /// <summary>
	  /// Test rate sensitivity. No cutoff, all ON rates already fixed. Thus expected sensitivity is none.
	  /// Time series up to 15-Jan (one day after the last fixing date). 
	  /// </summary>
	  public virtual void rateFedFund0CutOffValuationEndTs15Sensitivity()
	  {
		// publication=1, cutoff=0, effective offset=0, TS: Fixing all
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 16), date(2015, 1, 17)};
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(USD_FED_FUND, FIXING_START_DATE, FIXING_END_DATE, 0, REF_DATA);
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(USD_FED_FUND);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		LocalDateDoubleTimeSeriesBuilder tsb = LocalDateDoubleTimeSeries.builder();
		int lastFixing = 7;
		for (int i = 0; i < lastFixing; i++)
		{
		  tsb.put(FIXING_DATES[i], FIXING_RATES[i]);
		}
		when(mockRates.Fixings).thenReturn(tsb.build());
		for (int loopvaldate = 0; loopvaldate < valuationDate.Length; loopvaldate++)
		{
		  when(mockRates.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  PointSensitivityBuilder sensitivityComputed = OBS_FWD_ONCMP.rateSensitivity(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		  assertEquals(sensitivityComputed, PointSensitivityBuilder.none());
		}
	  }

	  /// <summary>
	  /// Two days cutoff, all ON rates already fixed. </summary>
	  public virtual void rateFedFund2CutOffValuationEnd()
	  {
		// publication=1, cutoff=2, effective offset=0, TS: Fixing all
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 14), date(2015, 1, 15), date(2015, 1, 16)};
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(USD_FED_FUND, FIXING_START_DATE, FIXING_END_DATE, 2, REF_DATA);
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(USD_FED_FUND);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		LocalDateDoubleTimeSeriesBuilder tsb = LocalDateDoubleTimeSeries.builder();
		int lastFixing = 5;
		for (int i = 0; i < lastFixing; i++)
		{
		  tsb.put(FIXING_DATES[i], FIXING_RATES[i]);
		}
		when(mockRates.Fixings).thenReturn(tsb.build());
		for (int i = 0; i < lastFixing; i++)
		{
		  when(mockRates.rate(USD_OBS[i])).thenReturn(FIXING_RATES[i]);
		}
		for (int i = lastFixing; i < USD_OBS.Length; i++)
		{
		  when(mockRates.rate(USD_OBS[i])).thenReturn(FORWARD_RATES[i]);
		}
		double afKnown = 0.0d;
		double investmentFactorKnown = 1.0d;
		for (int i = 0; i < 4; i++)
		{
		  LocalDate fixingknown = FIXING_DATES[i + 1];
		  LocalDate endDateKnown = USD_FED_FUND.calculateMaturityFromEffective(fixingknown, REF_DATA);
		  double af = USD_FED_FUND.DayCount.yearFraction(fixingknown, endDateKnown);
		  afKnown += af;
		  investmentFactorKnown *= 1.0d + FIXING_RATES[i + 1] * af;
		}
		LocalDate fixingknown = FIXING_DATES[5];
		LocalDate endDateKnown = USD_FED_FUND.calculateMaturityFromEffective(fixingknown, REF_DATA);
		double af = USD_FED_FUND.DayCount.yearFraction(fixingknown, endDateKnown);
		afKnown += af;
		investmentFactorKnown *= 1.0d + FIXING_RATES[4] * af; //Cutoff
		double rateExpected = (investmentFactorKnown - 1.0d) / afKnown;
		for (int loopvaldate = 0; loopvaldate < 3; loopvaldate++)
		{
		  when(mockRates.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  double rateComputed = OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		  assertEquals(rateExpected, rateComputed, TOLERANCE_RATE);
		}
	  }

	  /// <summary>
	  /// Test rate sensitivity. Two days cutoff, all ON rates already fixed. Thus none is expected. </summary>
	  public virtual void rateFedFund2CutOffValuationEndSensitivity()
	  {
		// publication=1, cutoff=2, effective offset=0, TS: Fixing all
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 14), date(2015, 1, 15), date(2015, 1, 16)};
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(USD_FED_FUND, FIXING_START_DATE, FIXING_END_DATE, 2, REF_DATA);
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(USD_FED_FUND);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(mockRates);

		LocalDateDoubleTimeSeriesBuilder tsb = LocalDateDoubleTimeSeries.builder();
		int lastFixing = 5;
		for (int i = 0; i < lastFixing; i++)
		{
		  tsb.put(FIXING_DATES[i], FIXING_RATES[i]);
		}
		when(mockRates.Fixings).thenReturn(tsb.build());
		for (int loopvaldate = 0; loopvaldate < 3; loopvaldate++)
		{
		  when(mockRates.ValuationDate).thenReturn(valuationDate[loopvaldate]);
		  PointSensitivityBuilder sensitivityComputed = OBS_FWD_ONCMP.rateSensitivity(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv);
		  assertEquals(sensitivityComputed, PointSensitivityBuilder.none());
		}
	  }

	  /// <summary>
	  /// One past fixing missing. Checking the error thrown. </summary>
	  public virtual void rateFedFund0CutOffValuation2MissingFixing()
	  {
		// publication=1, cutoff=0, effective offset=0, TS: Fixing 2
		LocalDate valuationDate = date(2015, 1, 13);
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(USD_FED_FUND, FIXING_START_DATE, FIXING_END_DATE, 2, REF_DATA);
		OvernightIndexRates mockRates = mock(typeof(OvernightIndexRates));
		when(mockRates.Index).thenReturn(USD_FED_FUND);
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(valuationDate, mockRates);
		when(mockRates.ValuationDate).thenReturn(valuationDate);

		LocalDateDoubleTimeSeriesBuilder tsb = LocalDateDoubleTimeSeries.builder();
		int lastFixing = 2;
		for (int i = 0; i < lastFixing; i++)
		{
		  tsb.put(FIXING_DATES[i], FIXING_RATES[i]);
		}
		when(mockRates.Fixings).thenReturn(tsb.build());
		for (int i = 0; i < lastFixing; i++)
		{
		  when(mockRates.rate(USD_OBS[i])).thenReturn(FIXING_RATES[i]);
		}
		for (int i = lastFixing; i < USD_OBS.Length; i++)
		{
		  when(mockRates.rate(USD_OBS[i])).thenReturn(FORWARD_RATES[i]);
		}
		assertThrows(() => OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv), typeof(PricingException));
		assertThrows(() => OBS_FWD_ONCMP.rateSensitivity(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, simpleProv), typeof(PricingException));
	  }

	  //-------------------------------------------------------------------------
	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.DOUBLE_QUADRATIC;
	  private static readonly LocalDateDoubleTimeSeries TIME_SERIES;
	  static ForwardOvernightCompoundedRateComputationFnTest()
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
	  /// Test parameter sensitivity with fd calculator. No cutoff. </summary>
	  public virtual void rateNoCutOffForwardParameterSensitivity()
	  { // publication=1, cutoff=0, effective offset=0, Forward
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 1), date(2015, 1, 8)};
		DoubleArray time_usd = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 10.0);
		DoubleArray rate_usd = DoubleArray.of(0.0100, 0.0110, 0.0115, 0.0130, 0.0135, 0.0135);
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(USD_FED_FUND, FIXING_START_DATE, FIXING_END_DATE, 0, REF_DATA);

		for (int loopvaldate = 0; loopvaldate < 2; loopvaldate++)
		{
		  Curve fedFundCurve = InterpolatedNodalCurve.of(Curves.zeroRates("USD-Fed-Fund", ACT_ACT_ISDA), time_usd, rate_usd, INTERPOLATOR);
		  ImmutableRatesProvider prov = ImmutableRatesProvider.builder(valuationDate[loopvaldate]).overnightIndexCurve(USD_FED_FUND, fedFundCurve, TIME_SERIES).build();
		  PointSensitivityBuilder sensitivityBuilderComputed = OBS_FWD_ONCMP.rateSensitivity(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, prov);
		  CurrencyParameterSensitivities parameterSensitivityComputed = prov.parameterSensitivity(sensitivityBuilderComputed.build());
		  CurrencyParameterSensitivities parameterSensitivityExpected = CAL_FD.sensitivity(prov, (p) => CurrencyAmount.of(USD_FED_FUND.Currency, OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, (p))));
		  assertTrue(parameterSensitivityComputed.equalWithTolerance(parameterSensitivityExpected, EPS_FD * 10.0));
		}
	  }

	  /// <summary>
	  /// Test parameter sensitivity with fd calculator. Two days cutoff. </summary>
	  public virtual void rate2CutOffForwardParameterSensitivity()
	  { // publication=1, cutoff=2, effective offset=0, Forward
		LocalDate[] valuationDate = new LocalDate[] {date(2015, 1, 1), date(2015, 1, 8)};
		DoubleArray time_usd = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 10.0);
		DoubleArray rate_usd = DoubleArray.of(0.0100, 0.0110, 0.0115, 0.0130, 0.0135, 0.0135);
		OvernightCompoundedRateComputation ro = OvernightCompoundedRateComputation.of(USD_FED_FUND, FIXING_START_DATE, FIXING_END_DATE, 2, REF_DATA);

		for (int loopvaldate = 0; loopvaldate < 2; loopvaldate++)
		{
		  Curve fedFundCurve = InterpolatedNodalCurve.of(Curves.zeroRates("USD-Fed-Fund", ACT_ACT_ISDA), time_usd, rate_usd, INTERPOLATOR);
		  ImmutableRatesProvider prov = ImmutableRatesProvider.builder(valuationDate[loopvaldate]).overnightIndexCurve(USD_FED_FUND, fedFundCurve, TIME_SERIES).build();
		  PointSensitivityBuilder sensitivityBuilderComputed = OBS_FWD_ONCMP.rateSensitivity(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, prov);
		  CurrencyParameterSensitivities parameterSensitivityComputed = prov.parameterSensitivity(sensitivityBuilderComputed.build());
		  CurrencyParameterSensitivities parameterSensitivityExpected = CAL_FD.sensitivity(prov, (p) => CurrencyAmount.of(USD_FED_FUND.Currency, OBS_FWD_ONCMP.rate(ro, DUMMY_ACCRUAL_START_DATE, DUMMY_ACCRUAL_END_DATE, (p))));
		  assertTrue(parameterSensitivityComputed.equalWithTolerance(parameterSensitivityExpected, EPS_FD * 10.0));
		}
	  }

	}

}