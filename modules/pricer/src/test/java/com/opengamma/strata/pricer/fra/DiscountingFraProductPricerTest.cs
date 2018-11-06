using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fra
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.fra.FraDummyData.FRA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.fra.FraDummyData.FRA_AFMA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.fra.FraDummyData.FRA_NONE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.fra.FraDummyData.FRA_TRADE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.mock;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.when;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using IborIndexObservation = com.opengamma.strata.basics.index.IborIndexObservation;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using CashFlows = com.opengamma.strata.market.amount.CashFlows;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProviderDataSets = com.opengamma.strata.pricer.datasets.RatesProviderDataSets;
	using IborIndexRates = com.opengamma.strata.pricer.rate.IborIndexRates;
	using IborRateSensitivity = com.opengamma.strata.pricer.rate.IborRateSensitivity;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RateComputationFn = com.opengamma.strata.pricer.rate.RateComputationFn;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SimpleIborIndexRates = com.opengamma.strata.pricer.rate.SimpleIborIndexRates;
	using SimpleRatesProvider = com.opengamma.strata.pricer.rate.SimpleRatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using Fra = com.opengamma.strata.product.fra.Fra;
	using ResolvedFra = com.opengamma.strata.product.fra.ResolvedFra;
	using ResolvedFraTrade = com.opengamma.strata.product.fra.ResolvedFraTrade;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;

	/// <summary>
	/// Tests <seealso cref="DiscountingFraProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingFraProductPricerTest
	public class DiscountingFraProductPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = LocalDate.of(2014, 1, 22);
	  private static readonly DayCount DAY_COUNT = DayCounts.ACT_ACT_ISDA;
	  private const double TOLERANCE = 1E-12;
	  private const double DISCOUNT_FACTOR = 0.98d;
	  private const double FORWARD_RATE = 0.02;

	  private static readonly ResolvedFraTrade RFRA_TRADE = FRA_TRADE.resolve(REF_DATA);
	  private static readonly ResolvedFra RFRA = FRA.resolve(REF_DATA);
	  private static readonly ResolvedFra RFRA_NONE = FRA_NONE.resolve(REF_DATA);
	  private static readonly ResolvedFra RFRA_AFMA = FRA_AFMA.resolve(REF_DATA);

	  /// <summary>
	  /// Test forecast value for ISDA FRA Discounting method.
	  /// </summary>
	  public virtual void test_forecastValue_ISDA()
	  {
		SimpleRatesProvider prov = createProvider(RFRA);

		double fixedRate = FRA.FixedRate;
		double yearFraction = RFRA.YearFraction;
		double notional = RFRA.Notional;
		double expected = notional * yearFraction * (FORWARD_RATE - fixedRate) / (1.0 + yearFraction * FORWARD_RATE);

		DiscountingFraProductPricer test = DiscountingFraProductPricer.DEFAULT;
		CurrencyAmount computed = test.forecastValue(RFRA, prov);
		assertEquals(computed.Amount, expected, TOLERANCE);

		// test via FraTrade
		DiscountingFraTradePricer testTrade = new DiscountingFraTradePricer(test);
		assertEquals(testTrade.forecastValue(RFRA_TRADE, prov), test.forecastValue(RFRA, prov));
	  }

	  /// <summary>
	  /// Test forecast value for NONE FRA Discounting method.
	  /// </summary>
	  public virtual void test_forecastValue_NONE()
	  {
		SimpleRatesProvider prov = createProvider(RFRA_NONE);

		double fixedRate = FRA_NONE.FixedRate;
		double yearFraction = RFRA_NONE.YearFraction;
		double notional = RFRA_NONE.Notional;
		double expected = notional * yearFraction * (FORWARD_RATE - fixedRate);

		DiscountingFraProductPricer test = DiscountingFraProductPricer.DEFAULT;
		CurrencyAmount computed = test.forecastValue(RFRA_NONE, prov);
		assertEquals(computed.Amount, expected, TOLERANCE);
	  }

	  /// <summary>
	  /// Test forecast value for AFMA FRA Discounting method.
	  /// </summary>
	  public virtual void test_forecastValue_AFMA()
	  {
		SimpleRatesProvider prov = createProvider(RFRA_AFMA);

		double fixedRate = FRA_AFMA.FixedRate;
		double yearFraction = RFRA_AFMA.YearFraction;
		double notional = RFRA_AFMA.Notional;
		double expected = -notional * (1.0 / (1.0 + yearFraction * FORWARD_RATE) - 1.0 / (1.0 + yearFraction * fixedRate));

		DiscountingFraProductPricer test = DiscountingFraProductPricer.DEFAULT;
		CurrencyAmount computed = test.forecastValue(RFRA_AFMA, prov);
		assertEquals(computed.Amount, expected, TOLERANCE);
	  }

	  /// <summary>
	  /// Test FRA paying in the past.
	  /// </summary>
	  public virtual void test_forecastValue_inPast()
	  {
		SimpleRatesProvider prov = createProvider(RFRA.toBuilder().paymentDate(VAL_DATE.minusDays(1)).build());

		DiscountingFraProductPricer test = DiscountingFraProductPricer.DEFAULT;
		CurrencyAmount computed = test.forecastValue(RFRA.toBuilder().paymentDate(VAL_DATE.minusDays(1)).build(), prov);
		assertEquals(computed.Amount, 0d, TOLERANCE);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Test present value for ISDA FRA Discounting method.
	  /// </summary>
	  public virtual void test_presentValue_ISDA()
	  {
		SimpleRatesProvider prov = createProvider(RFRA);

		DiscountingFraProductPricer test = DiscountingFraProductPricer.DEFAULT;
		CurrencyAmount pvComputed = test.presentValue(RFRA, prov);
		CurrencyAmount pvExpected = test.forecastValue(RFRA, prov).multipliedBy(DISCOUNT_FACTOR);
		assertEquals(pvComputed.Amount, pvExpected.Amount, TOLERANCE);

		// test via FraTrade
		DiscountingFraTradePricer testTrade = new DiscountingFraTradePricer(test);
		assertEquals(testTrade.presentValue(RFRA_TRADE, prov), test.presentValue(RFRA, prov));
	  }

	  /// <summary>
	  /// Test present value for NONE FRA Discounting method.
	  /// </summary>
	  public virtual void test_presentValue_NONE()
	  {
		SimpleRatesProvider prov = createProvider(RFRA_NONE);

		DiscountingFraProductPricer test = DiscountingFraProductPricer.DEFAULT;
		CurrencyAmount pvComputed = test.presentValue(RFRA_NONE, prov);
		CurrencyAmount pvExpected = test.forecastValue(RFRA_NONE, prov).multipliedBy(DISCOUNT_FACTOR);
		assertEquals(pvComputed.Amount, pvExpected.Amount, TOLERANCE);
	  }

	  /// <summary>
	  /// Test present value for ISDA FRA Discounting method.
	  /// </summary>
	  public virtual void test_presentValue_AFMA()
	  {
		SimpleRatesProvider prov = createProvider(RFRA_AFMA);

		DiscountingFraProductPricer test = DiscountingFraProductPricer.DEFAULT;
		CurrencyAmount pvComputed = test.presentValue(RFRA_AFMA, prov);
		CurrencyAmount pvExpected = test.forecastValue(RFRA_AFMA, prov).multipliedBy(DISCOUNT_FACTOR);
		assertEquals(pvComputed.Amount, pvExpected.Amount, TOLERANCE);
	  }

	  /// <summary>
	  /// Test FRA paying in the past.
	  /// </summary>
	  public virtual void test_presentValue_inPast()
	  {
		ResolvedFra fra = RFRA.toBuilder().paymentDate(VAL_DATE.minusDays(1)).build();
		SimpleRatesProvider prov = createProvider(fra);

		DiscountingFraProductPricer test = DiscountingFraProductPricer.DEFAULT;
		CurrencyAmount computed = test.presentValue(fra, prov);
		assertEquals(computed.Amount, 0d, TOLERANCE);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Test forecast value sensitivity for ISDA FRA discounting method.
	  /// </summary>
	  public virtual void test_forecastValueSensitivity_ISDA()
	  {
		SimpleRatesProvider prov = createProvider(RFRA);

		DiscountingFraProductPricer test = DiscountingFraProductPricer.DEFAULT;
		PointSensitivities sensitivity = test.forecastValueSensitivity(RFRA, prov);
		double eps = 1.e-7;
		double fdSense = forecastValueFwdSensitivity(RFRA, FORWARD_RATE, eps);

		ImmutableList<PointSensitivity> sensitivities = sensitivity.Sensitivities;
		assertEquals(sensitivities.size(), 1);
		IborRateSensitivity sensitivity0 = (IborRateSensitivity) sensitivities.get(0);
		assertEquals(sensitivity0.Index, FRA.Index);
		assertEquals(sensitivity0.Observation.FixingDate, FRA.StartDate);
		assertEquals(sensitivity0.Sensitivity, fdSense, FRA.Notional * eps);

		// test via FraTrade
		DiscountingFraTradePricer testTrade = new DiscountingFraTradePricer(test);
		assertEquals(testTrade.forecastValueSensitivity(RFRA_TRADE, prov), test.forecastValueSensitivity(RFRA, prov));
	  }

	  /// <summary>
	  /// Test forecast value sensitivity for NONE FRA discounting method.
	  /// </summary>
	  public virtual void test_forecastValueSensitivity_NONE()
	  {
		SimpleRatesProvider prov = createProvider(RFRA_NONE);

		DiscountingFraProductPricer test = DiscountingFraProductPricer.DEFAULT;
		PointSensitivities sensitivity = test.forecastValueSensitivity(RFRA_NONE, prov);
		double eps = 1.e-7;
		double fdSense = forecastValueFwdSensitivity(RFRA_NONE, FORWARD_RATE, eps);

		ImmutableList<PointSensitivity> sensitivities = sensitivity.Sensitivities;
		assertEquals(sensitivities.size(), 1);
		IborRateSensitivity sensitivity0 = (IborRateSensitivity) sensitivities.get(0);
		assertEquals(sensitivity0.Index, FRA_NONE.Index);
		assertEquals(sensitivity0.Observation.FixingDate, FRA_NONE.StartDate);
		assertEquals(sensitivity0.Sensitivity, fdSense, FRA_NONE.Notional * eps);
	  }

	  /// <summary>
	  /// Test forecast value sensitivity for AFMA FRA discounting method.
	  /// </summary>
	  public virtual void test_forecastValueSensitivity_AFMA()
	  {
		SimpleRatesProvider prov = createProvider(RFRA_AFMA);

		DiscountingFraProductPricer test = DiscountingFraProductPricer.DEFAULT;
		PointSensitivities sensitivity = test.forecastValueSensitivity(RFRA_AFMA, prov);
		double eps = 1.e-7;
		double fdSense = forecastValueFwdSensitivity(RFRA_AFMA, FORWARD_RATE, eps);

		ImmutableList<PointSensitivity> sensitivities = sensitivity.Sensitivities;
		assertEquals(sensitivities.size(), 1);
		IborRateSensitivity sensitivity0 = (IborRateSensitivity) sensitivities.get(0);
		assertEquals(sensitivity0.Index, FRA_AFMA.Index);
		assertEquals(sensitivity0.Observation.FixingDate, FRA_AFMA.StartDate);
		assertEquals(sensitivity0.Sensitivity, fdSense, FRA_AFMA.Notional * eps);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Test present value sensitivity for ISDA  
	  /// </summary>
	  public virtual void test_presentValueSensitivity_ISDA()
	  {
		RateComputationFn<RateComputation> mockObs = mock(typeof(RateComputationFn));
		DiscountFactors mockDf = mock(typeof(DiscountFactors));
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(VAL_DATE, mockDf);

		ResolvedFra fraExp = RFRA;
		double forwardRate = 0.05;
		double discountRate = 0.015;
		double paymentTime = 0.3;
		double discountFactor = Math.Exp(-discountRate * paymentTime);
		LocalDate fixingDate = FRA.StartDate;
		IborIndexObservation obs = IborIndexObservation.of(FRA.Index, fixingDate, REF_DATA);
		PointSensitivityBuilder sens = IborRateSensitivity.of(obs, 1d);
		when(mockDf.discountFactor(fraExp.PaymentDate)).thenReturn(discountFactor);
		when(mockDf.zeroRatePointSensitivity(fraExp.PaymentDate)).thenReturn(ZeroRateSensitivity.of(fraExp.Currency, paymentTime, -discountFactor * paymentTime));
		when(mockObs.rateSensitivity(fraExp.FloatingRate, fraExp.StartDate, fraExp.EndDate, simpleProv)).thenReturn(sens);
		when(mockObs.rate(fraExp.FloatingRate, FRA.StartDate, FRA.EndDate, simpleProv)).thenReturn(forwardRate);
		DiscountingFraProductPricer test = new DiscountingFraProductPricer(mockObs);
		PointSensitivities sensitivity = test.presentValueSensitivity(fraExp, simpleProv);
		double eps = 1.e-7;
		double fdDscSense = dscSensitivity(RFRA, forwardRate, discountFactor, paymentTime, eps);
		double fdSense = presentValueFwdSensitivity(RFRA, forwardRate, discountFactor, eps);

		ImmutableList<PointSensitivity> sensitivities = sensitivity.Sensitivities;
		assertEquals(sensitivities.size(), 2);
		IborRateSensitivity sensitivity0 = (IborRateSensitivity) sensitivities.get(0);
		assertEquals(sensitivity0.Index, FRA.Index);
		assertEquals(sensitivity0.Observation.FixingDate, fixingDate);
		assertEquals(sensitivity0.Sensitivity, fdSense, FRA.Notional * eps);
		ZeroRateSensitivity sensitivity1 = (ZeroRateSensitivity) sensitivities.get(1);
		assertEquals(sensitivity1.Currency, FRA.Currency);
		assertEquals(sensitivity1.YearFraction, paymentTime);
		assertEquals(sensitivity1.Sensitivity, fdDscSense, FRA.Notional * eps);

		// test via FraTrade
		DiscountingFraTradePricer testTrade = new DiscountingFraTradePricer(test);
		assertEquals(testTrade.presentValueSensitivity(RFRA_TRADE, simpleProv), test.presentValueSensitivity(fraExp, simpleProv));
	  }

	  /// <summary>
	  /// Test present value sensitivity for NONE FRA discounting method.
	  /// </summary>
	  public virtual void test_presentValueSensitivity_NONE()
	  {
		RateComputationFn<RateComputation> mockObs = mock(typeof(RateComputationFn));
		DiscountFactors mockDf = mock(typeof(DiscountFactors));
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(VAL_DATE, mockDf);

		ResolvedFra fraExp = RFRA_NONE;
		double forwardRate = 0.025;
		double discountRate = 0.01;
		double paymentTime = 0.3;
		double discountFactor = Math.Exp(-discountRate * paymentTime);
		LocalDate fixingDate = FRA_NONE.StartDate;
		IborIndexObservation obs = IborIndexObservation.of(FRA.Index, fixingDate, REF_DATA);
		PointSensitivityBuilder sens = IborRateSensitivity.of(obs, 1d);
		when(mockDf.discountFactor(fraExp.PaymentDate)).thenReturn(discountFactor);
		when(mockDf.zeroRatePointSensitivity(fraExp.PaymentDate)).thenReturn(ZeroRateSensitivity.of(fraExp.Currency, paymentTime, -discountFactor * paymentTime));
		when(mockObs.rateSensitivity(fraExp.FloatingRate, fraExp.StartDate, fraExp.EndDate, simpleProv)).thenReturn(sens);
		when(mockObs.rate(fraExp.FloatingRate, FRA_NONE.StartDate, FRA_NONE.EndDate, simpleProv)).thenReturn(forwardRate);
		DiscountingFraProductPricer test = new DiscountingFraProductPricer(mockObs);
		PointSensitivities sensitivity = test.presentValueSensitivity(fraExp, simpleProv);
		double eps = 1.e-7;
		double fdDscSense = dscSensitivity(RFRA_NONE, forwardRate, discountFactor, paymentTime, eps);
		double fdSense = presentValueFwdSensitivity(RFRA_NONE, forwardRate, discountFactor, eps);

		ImmutableList<PointSensitivity> sensitivities = sensitivity.Sensitivities;
		assertEquals(sensitivities.size(), 2);
		IborRateSensitivity sensitivity0 = (IborRateSensitivity) sensitivities.get(0);
		assertEquals(sensitivity0.Index, FRA_NONE.Index);
		assertEquals(sensitivity0.Observation.FixingDate, fixingDate);
		assertEquals(sensitivity0.Sensitivity, fdSense, FRA_NONE.Notional * eps);
		ZeroRateSensitivity sensitivity1 = (ZeroRateSensitivity) sensitivities.get(1);
		assertEquals(sensitivity1.Currency, FRA_NONE.Currency);
		assertEquals(sensitivity1.YearFraction, paymentTime);
		assertEquals(sensitivity1.Sensitivity, fdDscSense, FRA_NONE.Notional * eps);
	  }

	  /// <summary>
	  /// Test present value sensitivity for AFMA FRA discounting method.
	  /// </summary>
	  public virtual void test_presentValueSensitivity_AFMA()
	  {
		RateComputationFn<RateComputation> mockObs = mock(typeof(RateComputationFn));
		DiscountFactors mockDf = mock(typeof(DiscountFactors));
		SimpleRatesProvider simpleProv = new SimpleRatesProvider(VAL_DATE, mockDf);

		ResolvedFra fraExp = RFRA_AFMA;
		double forwardRate = 0.05;
		double discountRate = 0.025;
		double paymentTime = 0.3;
		double discountFactor = Math.Exp(-discountRate * paymentTime);
		LocalDate fixingDate = FRA_AFMA.StartDate;
		IborIndexObservation obs = IborIndexObservation.of(FRA.Index, fixingDate, REF_DATA);
		PointSensitivityBuilder sens = IborRateSensitivity.of(obs, 1d);
		when(mockDf.discountFactor(fraExp.PaymentDate)).thenReturn(discountFactor);
		when(mockDf.zeroRatePointSensitivity(fraExp.PaymentDate)).thenReturn(ZeroRateSensitivity.of(fraExp.Currency, paymentTime, -discountFactor * paymentTime));
		when(mockObs.rateSensitivity(fraExp.FloatingRate, fraExp.StartDate, fraExp.EndDate, simpleProv)).thenReturn(sens);
		when(mockObs.rate(fraExp.FloatingRate, FRA_AFMA.StartDate, FRA_AFMA.EndDate, simpleProv)).thenReturn(forwardRate);
		DiscountingFraProductPricer test = new DiscountingFraProductPricer(mockObs);
		PointSensitivities sensitivity = test.presentValueSensitivity(fraExp, simpleProv);
		double eps = 1.e-7;
		double fdDscSense = dscSensitivity(RFRA_AFMA, forwardRate, discountFactor, paymentTime, eps);
		double fdSense = presentValueFwdSensitivity(RFRA_AFMA, forwardRate, discountFactor, eps);

		ImmutableList<PointSensitivity> sensitivities = sensitivity.Sensitivities;
		assertEquals(sensitivities.size(), 2);
		IborRateSensitivity sensitivity0 = (IborRateSensitivity) sensitivities.get(0);
		assertEquals(sensitivity0.Index, FRA_AFMA.Index);
		assertEquals(sensitivity0.Observation.FixingDate, fixingDate);
		assertEquals(sensitivity0.Sensitivity, fdSense, FRA_AFMA.Notional * eps);
		ZeroRateSensitivity sensitivity1 = (ZeroRateSensitivity) sensitivities.get(1);
		assertEquals(sensitivity1.Currency, FRA_AFMA.Currency);
		assertEquals(sensitivity1.YearFraction, paymentTime);
		assertEquals(sensitivity1.Sensitivity, fdDscSense, FRA_AFMA.Notional * eps);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Test par rate for ISDA FRA Discounting method.
	  /// </summary>
	  public virtual void test_parRate_ISDA()
	  {
		ResolvedFra fraExp = RFRA;
		SimpleRatesProvider prov = createProvider(fraExp);

		DiscountingFraProductPricer test = DiscountingFraProductPricer.DEFAULT;
		double parRate = test.parRate(fraExp, prov);
		assertEquals(parRate, FORWARD_RATE);
		ResolvedFra fra = createNewFra(FRA, parRate);
		CurrencyAmount pv = test.presentValue(fra, prov);
		assertEquals(pv.Amount, 0.0, TOLERANCE);

		// test via FraTrade
		DiscountingFraTradePricer testTrade = new DiscountingFraTradePricer(test);
		assertEquals(testTrade.parRate(RFRA_TRADE, prov), test.parRate(RFRA, prov));
	  }

	  /// <summary>
	  /// Test par rate for NONE FRA Discounting method.
	  /// </summary>
	  public virtual void test_parRate_NONE()
	  {
		ResolvedFra fraExp = RFRA_NONE;
		SimpleRatesProvider prov = createProvider(fraExp);

		DiscountingFraProductPricer test = DiscountingFraProductPricer.DEFAULT;
		double parRate = test.parRate(fraExp, prov);
		assertEquals(parRate, FORWARD_RATE);
		ResolvedFra fra = createNewFra(FRA_NONE, parRate);
		CurrencyAmount pv = test.presentValue(fra, prov);
		assertEquals(pv.Amount, 0.0, TOLERANCE);
	  }

	  /// <summary>
	  /// Test par rate for AFMA FRA Discounting method.
	  /// </summary>
	  public virtual void test_parRate_AFMA()
	  {
		ResolvedFra fraExp = RFRA_AFMA;
		SimpleRatesProvider prov = createProvider(fraExp);

		DiscountingFraProductPricer test = DiscountingFraProductPricer.DEFAULT;
		double parRate = test.parRate(fraExp, prov);
		assertEquals(parRate, FORWARD_RATE);
		ResolvedFra fra = createNewFra(FRA_AFMA, parRate);
		CurrencyAmount pv = test.presentValue(fra, prov);
		assertEquals(pv.Amount, 0.0, TOLERANCE);
	  }

	  /// <summary>
	  /// Test par spread for ISDA FRA Discounting method.
	  /// </summary>
	  public virtual void test_parSpread_ISDA()
	  {
		ResolvedFra fraExp = RFRA;
		SimpleRatesProvider prov = createProvider(fraExp);

		DiscountingFraProductPricer test = DiscountingFraProductPricer.DEFAULT;
		double parSpread = test.parSpread(fraExp, prov);
		ResolvedFra fra = createNewFra(FRA, FRA.FixedRate + parSpread);
		CurrencyAmount pv = test.presentValue(fra, prov);
		assertEquals(pv.Amount, 0.0, TOLERANCE);

		// test via FraTrade
		DiscountingFraTradePricer testTrade = new DiscountingFraTradePricer(test);
		assertEquals(testTrade.parSpread(RFRA_TRADE, prov), test.parSpread(RFRA, prov));
	  }

	  /// <summary>
	  /// Test par spread for NONE FRA Discounting method.
	  /// </summary>
	  public virtual void test_parSpread_NONE()
	  {
		ResolvedFra fraExp = RFRA_NONE;
		SimpleRatesProvider prov = createProvider(fraExp);

		DiscountingFraProductPricer test = DiscountingFraProductPricer.DEFAULT;
		double parSpread = test.parSpread(fraExp, prov);
		ResolvedFra fra = createNewFra(FRA_NONE, FRA_NONE.FixedRate + parSpread);
		CurrencyAmount pv = test.presentValue(fra, prov);
		assertEquals(pv.Amount, 0.0, TOLERANCE);
	  }

	  /// <summary>
	  /// Test par spread for AFMA FRA Discounting method.
	  /// </summary>
	  public virtual void test_parSpread_AFMA()
	  {
		ResolvedFra fraExp = RFRA_AFMA;
		SimpleRatesProvider prov = createProvider(fraExp);

		DiscountingFraProductPricer test = DiscountingFraProductPricer.DEFAULT;
		double parSpread = test.parSpread(fraExp, prov);
		ResolvedFra fra = createNewFra(FRA_AFMA, FRA_AFMA.FixedRate + parSpread);
		CurrencyAmount pv = test.presentValue(fra, prov);
		assertEquals(pv.Amount, 0.0, TOLERANCE);
	  }

	  private const double EPS_FD = 1E-7;
	  private static readonly DiscountingFraProductPricer DEFAULT_PRICER = DiscountingFraProductPricer.DEFAULT;
	  private static readonly DiscountingFraTradePricer DEFAULT_TRADE_PRICER = DiscountingFraTradePricer.DEFAULT;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator CAL_FD = new RatesFiniteDifferenceSensitivityCalculator(EPS_FD);
	  private static readonly ImmutableRatesProvider IMM_PROV;
	  static DiscountingFraProductPricerTest()
	  {
		CurveInterpolator interp = CurveInterpolators.DOUBLE_QUADRATIC;
		DoubleArray time_gbp = DoubleArray.of(0.0, 0.1, 0.25, 0.5, 0.75, 1.0, 2.0);
		DoubleArray rate_gbp = DoubleArray.of(0.0160, 0.0165, 0.0155, 0.0155, 0.0155, 0.0150, 0.014);
		InterpolatedNodalCurve dscCurve = InterpolatedNodalCurve.of(Curves.zeroRates("GBP-Discount", DAY_COUNT), time_gbp, rate_gbp, interp);
		DoubleArray time_index = DoubleArray.of(0.0, 0.25, 0.5, 1.0);
		DoubleArray rate_index = DoubleArray.of(0.0180, 0.0180, 0.0175, 0.0165);
		InterpolatedNodalCurve indexCurve = InterpolatedNodalCurve.of(Curves.zeroRates("GBP-GBPIBOR3M", DAY_COUNT), time_index, rate_index, interp);
		IMM_PROV = ImmutableRatesProvider.builder(VAL_DATE).discountCurve(GBP, dscCurve).iborIndexCurve(GBP_LIBOR_3M, indexCurve).build();
	  }

	  /// <summary>
	  /// Test par spread sensitivity for ISDA FRA Discounting method.
	  /// </summary>
	  public virtual void test_parSpreadSensitivity_ISDA()
	  {
		PointSensitivities sensiSpread = DEFAULT_PRICER.parSpreadSensitivity(RFRA, IMM_PROV);
		CurrencyParameterSensitivities sensiComputed = IMM_PROV.parameterSensitivity(sensiSpread);
		CurrencyParameterSensitivities sensiExpected = CAL_FD.sensitivity(IMM_PROV, (p) => CurrencyAmount.of(FRA.Currency, DEFAULT_PRICER.parSpread(RFRA, (p))));
		assertTrue(sensiComputed.equalWithTolerance(sensiExpected, EPS_FD));
		PointSensitivities sensiRate = DEFAULT_PRICER.parRateSensitivity(RFRA, IMM_PROV);
		assertTrue(sensiSpread.equalWithTolerance(sensiRate, EPS_FD));

		// test via FraTrade
		assertEquals(DEFAULT_TRADE_PRICER.parRateSensitivity(RFRA_TRADE, IMM_PROV), DEFAULT_PRICER.parRateSensitivity(RFRA, IMM_PROV));
		assertEquals(DEFAULT_TRADE_PRICER.parSpreadSensitivity(RFRA_TRADE, IMM_PROV), DEFAULT_PRICER.parSpreadSensitivity(RFRA, IMM_PROV));
	  }

	  /// <summary>
	  /// Test par spread sensitivity for NONE FRA Discounting method.
	  /// </summary>
	  public virtual void test_parSpreadSensitivity_NONE()
	  {
		PointSensitivities sensiSpread = DEFAULT_PRICER.parSpreadSensitivity(RFRA_NONE, IMM_PROV);
		CurrencyParameterSensitivities sensiComputed = IMM_PROV.parameterSensitivity(sensiSpread);
		CurrencyParameterSensitivities sensiExpected = CAL_FD.sensitivity(IMM_PROV, (p) => CurrencyAmount.of(FRA_NONE.Currency, DEFAULT_PRICER.parSpread(RFRA_NONE, (p))));
		assertTrue(sensiComputed.equalWithTolerance(sensiExpected, EPS_FD));
		PointSensitivities sensiRate = DEFAULT_PRICER.parRateSensitivity(RFRA_NONE, IMM_PROV);
		assertTrue(sensiSpread.equalWithTolerance(sensiRate, EPS_FD));
	  }

	  /// <summary>
	  /// Test par spread sensitivity for AFMA FRA Discounting method.
	  /// </summary>
	  public virtual void test_parSpreadSensitivity_AFMA()
	  {
		PointSensitivities sensiSpread = DEFAULT_PRICER.parSpreadSensitivity(RFRA_AFMA, IMM_PROV);
		CurrencyParameterSensitivities sensiComputed = IMM_PROV.parameterSensitivity(sensiSpread);
		CurrencyParameterSensitivities sensiExpected = CAL_FD.sensitivity(IMM_PROV, (p) => CurrencyAmount.of(FRA_AFMA.Currency, DEFAULT_PRICER.parSpread(RFRA_AFMA, (p))));
		assertTrue(sensiComputed.equalWithTolerance(sensiExpected, EPS_FD));
		PointSensitivities sensiRate = DEFAULT_PRICER.parRateSensitivity(RFRA_AFMA, IMM_PROV);
		assertTrue(sensiSpread.equalWithTolerance(sensiRate, EPS_FD));
	  }

	  private ResolvedFra createNewFra(Fra product, double newFixedRate)
	  {
		return Fra.builder().buySell(product.BuySell).notional(product.Notional).startDate(product.StartDate).endDate(product.EndDate).index(product.Index).fixedRate(newFixedRate).currency(product.Currency).build().resolve(REF_DATA);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Test cash flow for ISDA FRA Discounting method.
	  /// </summary>
	  public virtual void test_cashFlows_ISDA()
	  {
		ResolvedFra fraExp = RFRA;
		SimpleRatesProvider prov = createProvider(fraExp);

		double fixedRate = FRA.FixedRate;
		double yearFraction = fraExp.YearFraction;
		double notional = fraExp.Notional;
		double expected = notional * yearFraction * (FORWARD_RATE - fixedRate) / (1.0 + yearFraction * FORWARD_RATE);

		DiscountingFraProductPricer test = DiscountingFraProductPricer.DEFAULT;
		CashFlows computed = test.cashFlows(fraExp, prov);
		assertEquals(computed.getCashFlows().size(), 1);
		assertEquals(computed.getCashFlows().size(), 1);
		assertEquals(computed.getCashFlows().get(0).PaymentDate, fraExp.PaymentDate);
		assertEquals(computed.getCashFlows().get(0).ForecastValue.Currency, fraExp.Currency);
		assertEquals(computed.getCashFlows().get(0).ForecastValue.Amount, expected, TOLERANCE);

		// test via FraTrade
		DiscountingFraTradePricer testTrade = new DiscountingFraTradePricer(test);
		assertEquals(testTrade.cashFlows(RFRA_TRADE, prov), test.cashFlows(fraExp, prov));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Test explain.
	  /// </summary>
	  public virtual void test_explainPresentValue_ISDA()
	  {
		ResolvedFra fraExp = RFRA;
		SimpleRatesProvider prov = createProvider(fraExp);

		DiscountingFraProductPricer test = DiscountingFraProductPricer.DEFAULT;
		CurrencyAmount fvExpected = test.forecastValue(fraExp, prov);
		CurrencyAmount pvExpected = test.presentValue(fraExp, prov);

		ExplainMap explain = test.explainPresentValue(fraExp, prov);
		Currency currency = fraExp.Currency;
		int daysBetween = (int) DAYS.between(fraExp.StartDate, fraExp.EndDate);
		assertEquals(explain.get(ExplainKey.ENTRY_TYPE).get(), "FRA");
		assertEquals(explain.get(ExplainKey.PAYMENT_DATE).get(), fraExp.PaymentDate);
		assertEquals(explain.get(ExplainKey.START_DATE).get(), fraExp.StartDate);
		assertEquals(explain.get(ExplainKey.END_DATE).get(), fraExp.EndDate);
		assertEquals(explain.get(ExplainKey.ACCRUAL_YEAR_FRACTION).Value, fraExp.YearFraction);
		assertEquals(explain.get(ExplainKey.DAYS).Value, (int?)(int) daysBetween);
		assertEquals(explain.get(ExplainKey.PAYMENT_CURRENCY).get(), currency);
		assertEquals(explain.get(ExplainKey.NOTIONAL).get().Amount, fraExp.Notional, TOLERANCE);
		assertEquals(explain.get(ExplainKey.TRADE_NOTIONAL).get().Amount, fraExp.Notional, TOLERANCE);

		assertEquals(explain.get(ExplainKey.OBSERVATIONS).get().size(), 1);
		ExplainMap explainObs = explain.get(ExplainKey.OBSERVATIONS).get().get(0);
		IborRateComputation floatingRate = (IborRateComputation) fraExp.FloatingRate;
		assertEquals(explainObs.get(ExplainKey.INDEX).get(), floatingRate.Index);
		assertEquals(explainObs.get(ExplainKey.FIXING_DATE).get(), floatingRate.FixingDate);
		assertEquals(explainObs.get(ExplainKey.INDEX_VALUE).Value, FORWARD_RATE, TOLERANCE);
		assertEquals(explainObs.get(ExplainKey.FROM_FIXING_SERIES).HasValue, false);
		assertEquals(explain.get(ExplainKey.DISCOUNT_FACTOR).Value, DISCOUNT_FACTOR, TOLERANCE);
		assertEquals(explain.get(ExplainKey.FIXED_RATE).Value, fraExp.FixedRate, TOLERANCE);
		assertEquals(explain.get(ExplainKey.PAY_OFF_RATE).Value, FORWARD_RATE, TOLERANCE);
		assertEquals(explain.get(ExplainKey.COMBINED_RATE).Value, FORWARD_RATE, TOLERANCE);
		assertEquals(explain.get(ExplainKey.UNIT_AMOUNT).Value, fvExpected.Amount / fraExp.Notional, TOLERANCE);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.FORECAST_VALUE).get().Amount, fvExpected.Amount, TOLERANCE);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Currency, currency);
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Amount, pvExpected.Amount, TOLERANCE);

		// test via FraTrade
		DiscountingFraTradePricer testTrade = new DiscountingFraTradePricer(test);
		assertEquals(testTrade.explainPresentValue(RFRA_TRADE, prov), test.explainPresentValue(RFRA, prov));
	  }

	  //-------------------------------------------------------------------------
	  // creates a simple provider
	  private SimpleRatesProvider createProvider(ResolvedFra fraExp)
	  {
		DiscountFactors mockDf = SimpleDiscountFactors.of(GBP, VAL_DATE, ConstantCurve.of(Curves.discountFactors("DSC", DAY_COUNT), DISCOUNT_FACTOR));
		LocalDateDoubleTimeSeries timeSeries = LocalDateDoubleTimeSeries.of(VAL_DATE, FORWARD_RATE);
		IborIndexRates mockIbor = SimpleIborIndexRates.of(GBP_LIBOR_3M, VAL_DATE, ConstantCurve.of(Curves.forwardRates("L3M", DAY_COUNT), FORWARD_RATE), timeSeries);
		SimpleRatesProvider prov = new SimpleRatesProvider(VAL_DATE, mockDf);
		prov.IborRates = mockIbor;
		return prov;
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity_zeroCurve_FD()
	  {
		double eps = 1.0e-6;
		ImmutableRatesProvider prov = RatesProviderDataSets.MULTI_GBP_USD;
		RatesFiniteDifferenceSensitivityCalculator cal = new RatesFiniteDifferenceSensitivityCalculator(eps);
		DiscountingFraProductPricer pricer = DiscountingFraProductPricer.DEFAULT;
		ResolvedFra fraExp = RFRA;
		PointSensitivities point = pricer.presentValueSensitivity(fraExp, prov);
		CurrencyParameterSensitivities computed = prov.parameterSensitivity(point);
		CurrencyParameterSensitivities expected = cal.sensitivity(prov, p => pricer.presentValue(fraExp, p));
		assertTrue(computed.equalWithTolerance(expected, eps * FRA.Notional));
	  }

	  public virtual void test_presentValueSensitivity_dfCurve_FD()
	  {
		double eps = 1.0e-6;
		ImmutableRatesProvider prov = RatesProviderDataSets.MULTI_GBP_USD_SIMPLE;
		RatesFiniteDifferenceSensitivityCalculator cal = new RatesFiniteDifferenceSensitivityCalculator(eps);
		DiscountingFraProductPricer pricer = DiscountingFraProductPricer.DEFAULT;
		ResolvedFra fraExp = RFRA;
		PointSensitivities point = pricer.presentValueSensitivity(fraExp, prov);
		CurrencyParameterSensitivities computed = prov.parameterSensitivity(point);
		CurrencyParameterSensitivities expected = cal.sensitivity(prov, p => pricer.presentValue(fraExp, p));
		assertTrue(computed.equalWithTolerance(expected, eps * FRA.Notional));
	  }

	  //-------------------------------------------------------------------------
	  private double forecastValueFwdSensitivity(ResolvedFra fra, double forwardRate, double eps)
	  {

		RateComputationFn<RateComputation> obsFuncNew = mock(typeof(RateComputationFn));
		RatesProvider provNew = mock(typeof(RatesProvider));
		when(provNew.ValuationDate).thenReturn(VAL_DATE);
		when(obsFuncNew.rate(fra.FloatingRate, fra.StartDate, fra.EndDate, provNew)).thenReturn(forwardRate + eps);
		CurrencyAmount upValue = (new DiscountingFraProductPricer(obsFuncNew)).forecastValue(fra, provNew);
		when(obsFuncNew.rate(fra.FloatingRate, fra.StartDate, fra.EndDate, provNew)).thenReturn(forwardRate - eps);
		CurrencyAmount downValue = (new DiscountingFraProductPricer(obsFuncNew)).forecastValue(fra, provNew);
		return upValue.minus(downValue).multipliedBy(0.5 / eps).Amount;
	  }

	  private double presentValueFwdSensitivity(ResolvedFra fra, double forwardRate, double discountFactor, double eps)
	  {

		RateComputationFn<RateComputation> obsFuncNew = mock(typeof(RateComputationFn));
		RatesProvider provNew = mock(typeof(RatesProvider));
		when(provNew.ValuationDate).thenReturn(VAL_DATE);
		when(provNew.discountFactor(fra.Currency, fra.PaymentDate)).thenReturn(discountFactor);
		when(obsFuncNew.rate(fra.FloatingRate, fra.StartDate, fra.EndDate, provNew)).thenReturn(forwardRate + eps);
		CurrencyAmount upValue = (new DiscountingFraProductPricer(obsFuncNew)).presentValue(fra, provNew);
		when(obsFuncNew.rate(fra.FloatingRate, fra.StartDate, fra.EndDate, provNew)).thenReturn(forwardRate - eps);
		CurrencyAmount downValue = (new DiscountingFraProductPricer(obsFuncNew)).presentValue(fra, provNew);
		return upValue.minus(downValue).multipliedBy(0.5 / eps).Amount;
	  }

	  private double dscSensitivity(ResolvedFra fra, double forwardRate, double discountFactor, double paymentTime, double eps)
	  {

		RatesProvider provNew = mock(typeof(RatesProvider));
		when(provNew.ValuationDate).thenReturn(VAL_DATE);
		RateComputationFn<RateComputation> obsFuncNew = mock(typeof(RateComputationFn));
		when(obsFuncNew.rate(fra.FloatingRate, fra.StartDate, fra.EndDate, provNew)).thenReturn(forwardRate);
		when(provNew.discountFactor(fra.Currency, fra.PaymentDate)).thenReturn(discountFactor * Math.Exp(-eps * paymentTime));
		CurrencyAmount upDscValue = (new DiscountingFraProductPricer(obsFuncNew)).presentValue(fra, provNew);
		when(provNew.discountFactor(fra.Currency, fra.PaymentDate)).thenReturn(discountFactor * Math.Exp(eps * paymentTime));
		CurrencyAmount downDscValue = (new DiscountingFraProductPricer(obsFuncNew)).presentValue(fra, provNew);
		return upDscValue.minus(downDscValue).multipliedBy(0.5 / eps).Amount;
	  }

	}

}