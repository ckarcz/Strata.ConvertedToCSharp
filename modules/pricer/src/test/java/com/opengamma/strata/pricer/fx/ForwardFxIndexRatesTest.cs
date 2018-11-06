/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fx
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.FxIndices.EUR_GBP_ECB;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.FxIndices.GBP_USD_WM;
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
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// Test <seealso cref="ForwardFxIndexRates"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ForwardFxIndexRatesTest
	public class ForwardFxIndexRatesTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate DATE_VAL = date(2015, 6, 4);
	  private static readonly LocalDate DATE_BEFORE = date(2015, 6, 3);
	  private static readonly LocalDate DATE_AFTER = date(2015, 7, 30);

	  private static readonly FxIndexObservation OBS_VAL = FxIndexObservation.of(GBP_USD_WM, DATE_VAL, REF_DATA);
	  private static readonly FxIndexObservation OBS_BEFORE = FxIndexObservation.of(GBP_USD_WM, DATE_BEFORE, REF_DATA);
	  private static readonly FxIndexObservation OBS_AFTER = FxIndexObservation.of(GBP_USD_WM, DATE_AFTER, REF_DATA);
	  private static readonly FxIndexObservation OBS_EUR_VAL = FxIndexObservation.of(EUR_GBP_ECB, DATE_VAL, REF_DATA);

	  private static readonly CurrencyPair PAIR_GBP_USD = CurrencyPair.of(GBP, USD);
	  private static readonly CurrencyPair PAIR_USD_GBP = CurrencyPair.of(USD, GBP);
	  private static readonly CurrencyPair PAIR_EUR_GBP = CurrencyPair.of(EUR, GBP);
	  private static readonly FxRate FX_RATE = FxRate.of(GBP, USD, 1.5d);
	  private static readonly FxRate FX_RATE_EUR_GBP = FxRate.of(EUR, GBP, 0.7d);

	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;
	  private static readonly CurveMetadata METADATA1 = Curves.zeroRates("TestCurve", ACT_365F);
	  private static readonly CurveMetadata METADATA2 = Curves.zeroRates("TestCurveUSD", ACT_365F);
	  private static readonly InterpolatedNodalCurve CURVE1 = InterpolatedNodalCurve.of(METADATA1, DoubleArray.of(0, 10), DoubleArray.of(0.01, 0.02), INTERPOLATOR);
	  private static readonly InterpolatedNodalCurve CURVE2 = InterpolatedNodalCurve.of(METADATA2, DoubleArray.of(0, 10), DoubleArray.of(0.015, 0.025), INTERPOLATOR);
	  private static readonly ZeroRateDiscountFactors DFCURVE_GBP = ZeroRateDiscountFactors.of(GBP, DATE_VAL, CURVE1);
	  private static readonly ZeroRateDiscountFactors DFCURVE_USD = ZeroRateDiscountFactors.of(USD, DATE_VAL, CURVE2);
	  private static readonly ZeroRateDiscountFactors DFCURVE_EUR = ZeroRateDiscountFactors.of(EUR, DATE_VAL, CURVE2);

	  private const double RATE_BEFORE = 0.013d;
	  private const double RATE_VAL = 0.014d;
	  private static readonly LocalDateDoubleTimeSeries SERIES = LocalDateDoubleTimeSeries.builder().put(DATE_BEFORE, RATE_BEFORE).put(DATE_VAL, RATE_VAL).build();
	  private static readonly LocalDateDoubleTimeSeries SERIES_MINIMAL = LocalDateDoubleTimeSeries.of(DATE_VAL, RATE_VAL);
	  private static readonly LocalDateDoubleTimeSeries SERIES_EMPTY = LocalDateDoubleTimeSeries.empty();

	  private static readonly FxForwardRates FWD_RATES = DiscountFxForwardRates.of(PAIR_GBP_USD, FX_RATE, DFCURVE_GBP, DFCURVE_USD);
	  private static readonly FxForwardRates FWD_RATES_USD_GBP = DiscountFxForwardRates.of(PAIR_USD_GBP, FX_RATE.inverse(), DFCURVE_USD, DFCURVE_GBP);
	  private static readonly FxForwardRates FWD_RATES_EUR_GBP = DiscountFxForwardRates.of(PAIR_EUR_GBP, FX_RATE_EUR_GBP, DFCURVE_EUR, DFCURVE_GBP);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_withoutFixings()
	  {
		ForwardFxIndexRates test = ForwardFxIndexRates.of(GBP_USD_WM, FWD_RATES);
		assertEquals(test.Index, GBP_USD_WM);
		assertEquals(test.ValuationDate, DATE_VAL);
		assertEquals(test.Fixings, SERIES_EMPTY);
		assertEquals(test.FxForwardRates, FWD_RATES);
		assertEquals(test.findData(CURVE1.Name), CURVE1);
		assertEquals(test.findData(CURVE2.Name), CURVE2);
		assertEquals(test.findData(CurveName.of("Rubbish")), null);
		assertEquals(test.ParameterCount, FWD_RATES.ParameterCount);
		assertEquals(test.getParameter(0), FWD_RATES.getParameter(0));
		assertEquals(test.getParameterMetadata(0), FWD_RATES.getParameterMetadata(0));
		assertEquals(test.withParameter(0, 1d).FxForwardRates, FWD_RATES.withParameter(0, 1d));
		assertEquals(test.withPerturbation((i, v, m) => v + 1d).FxForwardRates, FWD_RATES.withPerturbation((i, v, m) => v + 1d));
	  }

	  public virtual void test_of_withFixings()
	  {
		ForwardFxIndexRates test = ForwardFxIndexRates.of(GBP_USD_WM, FWD_RATES, SERIES);
		assertEquals(test.Index, GBP_USD_WM);
		assertEquals(test.ValuationDate, DATE_VAL);
		assertEquals(test.Fixings, SERIES);
		assertEquals(test.FxForwardRates, FWD_RATES);
	  }

	  public virtual void test_of_nonMatchingCurrency()
	  {
		assertThrowsIllegalArg(() => ForwardFxIndexRates.of(GBP_USD_WM, FWD_RATES_USD_GBP, SERIES));
		assertThrowsIllegalArg(() => ForwardFxIndexRates.of(GBP_USD_WM, FWD_RATES_EUR_GBP, SERIES));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_rate_beforeValuation_fixing()
	  {
		ForwardFxIndexRates test = ForwardFxIndexRates.of(GBP_USD_WM, FWD_RATES, SERIES);
		assertEquals(test.rate(OBS_BEFORE, GBP), RATE_BEFORE);
		assertEquals(test.rate(OBS_BEFORE, USD), 1d / RATE_BEFORE);
	  }

	  public virtual void test_rate_beforeValuation_noFixing_emptySeries()
	  {
		ForwardFxIndexRates test = ForwardFxIndexRates.of(GBP_USD_WM, FWD_RATES, SERIES_EMPTY);
		assertThrowsIllegalArg(() => test.rate(OBS_BEFORE, GBP));
	  }

	  public virtual void test_rate_beforeValuation_noFixing_notEmptySeries()
	  {
		ForwardFxIndexRates test = ForwardFxIndexRates.of(GBP_USD_WM, FWD_RATES, SERIES_MINIMAL);
		assertThrowsIllegalArg(() => test.rate(OBS_BEFORE, GBP));
	  }

	  public virtual void test_rate_onValuation_fixing()
	  {
		ForwardFxIndexRates test = ForwardFxIndexRates.of(GBP_USD_WM, FWD_RATES, SERIES);
		assertEquals(test.rate(OBS_VAL, GBP), RATE_VAL);
		assertEquals(test.rate(OBS_VAL, USD), 1d / RATE_VAL);
	  }

	  public virtual void test_rate_onValuation_noFixing()
	  {
		ForwardFxIndexRates test = ForwardFxIndexRates.of(GBP_USD_WM, FWD_RATES, SERIES_EMPTY);
		LocalDate maturityDate = GBP_USD_WM.calculateMaturityFromFixing(DATE_VAL, REF_DATA);
		double dfCcyBaseAtMaturity = DFCURVE_GBP.discountFactor(maturityDate);
		double dfCcyCounterAtMaturity = DFCURVE_USD.discountFactor(maturityDate);
		double expected = FX_RATE.fxRate(GBP, USD) * (dfCcyBaseAtMaturity / dfCcyCounterAtMaturity);
		assertEquals(test.rate(OBS_VAL, GBP), expected, 1e-8);
		assertEquals(test.rate(OBS_VAL, USD), 1d / expected, 1e-8);
	  }

	  public virtual void test_rate_afterValuation()
	  {
		ForwardFxIndexRates test = ForwardFxIndexRates.of(GBP_USD_WM, FWD_RATES, SERIES);
		LocalDate maturityDate = GBP_USD_WM.calculateMaturityFromFixing(DATE_AFTER, REF_DATA);
		double dfCcyBaseAtMaturity = DFCURVE_GBP.discountFactor(maturityDate);
		double dfCcyCounterAtMaturity = DFCURVE_USD.discountFactor(maturityDate);
		double expected = FX_RATE.fxRate(GBP, USD) * (dfCcyBaseAtMaturity / dfCcyCounterAtMaturity);
		assertEquals(test.rate(OBS_AFTER, GBP), expected, 1e-8);
		assertEquals(test.rate(OBS_AFTER, USD), 1d / expected, 1e-8);
	  }

	  public virtual void test_rate_nonMatchingCurrency()
	  {
		ForwardFxIndexRates test = ForwardFxIndexRates.of(GBP_USD_WM, FWD_RATES, SERIES);
		assertThrowsIllegalArg(() => test.rate(OBS_EUR_VAL, EUR));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_ratePointSensitivity_fixing()
	  {
		ForwardFxIndexRates test = ForwardFxIndexRates.of(GBP_USD_WM, FWD_RATES, SERIES);
		assertEquals(test.ratePointSensitivity(OBS_BEFORE, GBP), PointSensitivityBuilder.none());
		assertEquals(test.ratePointSensitivity(OBS_VAL, GBP), PointSensitivityBuilder.none());
	  }

	  public virtual void test_ratePointSensitivity_onValuation_noFixing()
	  {
		ForwardFxIndexRates test = ForwardFxIndexRates.of(GBP_USD_WM, FWD_RATES, SERIES_EMPTY);
		assertEquals(test.ratePointSensitivity(OBS_VAL, GBP), FxIndexSensitivity.of(OBS_VAL, GBP, 1d));
		assertEquals(test.ratePointSensitivity(OBS_VAL, USD), FxIndexSensitivity.of(OBS_VAL, USD, 1d));
	  }

	  public virtual void test_ratePointSensitivity_afterValuation()
	  {
		ForwardFxIndexRates test = ForwardFxIndexRates.of(GBP_USD_WM, FWD_RATES, SERIES);
		assertEquals(test.ratePointSensitivity(OBS_AFTER, GBP), FxIndexSensitivity.of(OBS_AFTER, GBP, 1d));
		assertEquals(test.ratePointSensitivity(OBS_AFTER, USD), FxIndexSensitivity.of(OBS_AFTER, USD, 1d));
	  }

	  public virtual void test_ratePointSensitivity_nonMatchingCurrency()
	  {
		ForwardFxIndexRates test = ForwardFxIndexRates.of(GBP_USD_WM, FWD_RATES, SERIES);
		assertThrowsIllegalArg(() => test.ratePointSensitivity(OBS_EUR_VAL, EUR));
	  }

	  //-------------------------------------------------------------------------
	  //proper end-to-end tests are elsewhere
	  public virtual void test_parameterSensitivity()
	  {
		ForwardFxIndexRates test = ForwardFxIndexRates.of(GBP_USD_WM, FWD_RATES, SERIES);
		FxIndexSensitivity point = FxIndexSensitivity.of(OBS_VAL, GBP, 1d);
		assertEquals(test.parameterSensitivity(point).size(), 2);
		FxIndexSensitivity point2 = FxIndexSensitivity.of(OBS_VAL, USD, 1d);
		assertEquals(test.parameterSensitivity(point2).size(), 2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ForwardFxIndexRates test = ForwardFxIndexRates.of(GBP_USD_WM, FWD_RATES, SERIES);
		coverImmutableBean(test);
		ForwardFxIndexRates test2 = ForwardFxIndexRates.of(EUR_GBP_ECB, FWD_RATES_EUR_GBP, SERIES_MINIMAL);
		coverBeanEquals(test, test2);
	  }

	}

}