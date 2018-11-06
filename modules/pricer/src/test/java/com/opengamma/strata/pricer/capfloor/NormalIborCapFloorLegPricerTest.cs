/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.dateUtc;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PutCall.CALL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PutCall.PUT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using IborCapletFloorletPeriod = com.opengamma.strata.product.capfloor.IborCapletFloorletPeriod;
	using ResolvedIborCapFloorLeg = com.opengamma.strata.product.capfloor.ResolvedIborCapFloorLeg;

	/// <summary>
	/// Test <seealso cref="NormalIborCapFloorLegPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NormalIborCapFloorLegPricerTest
	public class NormalIborCapFloorLegPricerTest
	{

	  private const double STRIKE = 0.015;
	  private const double NOTIONAL_VALUE = 100_000_000;
	  private static readonly ValueSchedule STRIKE_SCHEDULE = ValueSchedule.of(STRIKE);
	  private static readonly ValueSchedule NOTIONAL = ValueSchedule.of(NOTIONAL_VALUE);
	  private static readonly LocalDate START = LocalDate.of(2011, 3, 17);
	  private static readonly LocalDate END = LocalDate.of(2016, 3, 17);
	  private static readonly ResolvedIborCapFloorLeg CAP = IborCapFloorDataSet.createCapFloorLeg(EUR_EURIBOR_3M, START, END, STRIKE_SCHEDULE, NOTIONAL, CALL, RECEIVE);
	  private static readonly ResolvedIborCapFloorLeg FLOOR = IborCapFloorDataSet.createCapFloorLeg(EUR_EURIBOR_3M, START, END, STRIKE_SCHEDULE, NOTIONAL, PUT, PAY);

	  // valuation before start
	  private static readonly ZonedDateTime VALUATION = dateUtc(2011, 1, 20);
	  private static readonly ImmutableRatesProvider RATES = IborCapletFloorletDataSet.createRatesProvider(VALUATION.toLocalDate());
	  private static readonly NormalIborCapletFloorletExpiryStrikeVolatilities VOLS = IborCapletFloorletDataSet.createNormalVolatilities(VALUATION, EUR_EURIBOR_3M);
	  // valuation between fixing of 3rd period and payment of 2nd period
	  private const double OBS_INDEX_2 = 0.012;
	  private const double OBS_INDEX_3 = 0.0125;
	  private static readonly LocalDateDoubleTimeSeries TIME_SERIES = LocalDateDoubleTimeSeries.builder().put(date(2011, 6, 15), OBS_INDEX_2).put(date(2011, 9, 15), OBS_INDEX_3).build();
	  private static readonly ZonedDateTime VALUATION_AFTER = dateUtc(2011, 9, 16);
	  private static readonly ImmutableRatesProvider RATES_AFTER = IborCapletFloorletDataSet.createRatesProvider(VALUATION_AFTER.toLocalDate(), EUR_EURIBOR_3M, TIME_SERIES);
	  private static readonly NormalIborCapletFloorletExpiryStrikeVolatilities VOLS_AFTER = IborCapletFloorletDataSet.createNormalVolatilities(VALUATION_AFTER, EUR_EURIBOR_3M);
	  // valuation at payment of 2nd period
	  private static readonly ZonedDateTime VALUATION_PAY = dateUtc(2011, 9, 19);
	  private static readonly ImmutableRatesProvider RATES_PAY = IborCapletFloorletDataSet.createRatesProvider(VALUATION_PAY.toLocalDate(), EUR_EURIBOR_3M, TIME_SERIES);
	  private static readonly NormalIborCapletFloorletExpiryStrikeVolatilities VOLS_PAY = IborCapletFloorletDataSet.createNormalVolatilities(VALUATION_PAY, EUR_EURIBOR_3M);

	  private const double TOL = 1.0e-14;
	  private static readonly NormalIborCapFloorLegPricer PRICER = NormalIborCapFloorLegPricer.DEFAULT;
	  private static readonly NormalIborCapletFloorletPeriodPricer PRICER_PERIOD = NormalIborCapletFloorletPeriodPricer.DEFAULT;

	  public virtual void test_presentValue()
	  {
		CurrencyAmount capComputed = PRICER.presentValue(CAP, RATES, VOLS);
		CurrencyAmount floorComputed = PRICER.presentValue(FLOOR, RATES, VOLS);
		double capExpected = 0d;
		double floorExpected = 0d;
		int nPeriods = CAP.CapletFloorletPeriods.size();
		for (int i = 0; i < nPeriods; ++i)
		{
		  capExpected += PRICER_PERIOD.presentValue(CAP.CapletFloorletPeriods.get(i), RATES, VOLS).Amount;
		  floorExpected += PRICER_PERIOD.presentValue(FLOOR.CapletFloorletPeriods.get(i), RATES, VOLS).Amount;
		}
		assertEquals(capComputed.Currency, EUR);
		assertEquals(capComputed.Amount, capExpected);
		assertEquals(floorComputed.Currency, EUR);
		assertEquals(floorComputed.Amount, floorExpected);
	  }

	  public virtual void test_presentValue_after()
	  {
		CurrencyAmount capComputed = PRICER.presentValue(CAP, RATES_AFTER, VOLS_AFTER);
		CurrencyAmount floorComputed = PRICER.presentValue(FLOOR, RATES_AFTER, VOLS_AFTER);
		double capExpected = 0d;
		IborCapletFloorletPeriod period = FLOOR.CapletFloorletPeriods.get(1);
		double floorExpected = -(STRIKE - OBS_INDEX_2) * NOTIONAL_VALUE * period.YearFraction * RATES_AFTER.discountFactor(EUR, period.PaymentDate);
		int nPeriods = CAP.CapletFloorletPeriods.size();
		for (int i = 2; i < nPeriods; ++i)
		{
		  capExpected += PRICER_PERIOD.presentValue(CAP.CapletFloorletPeriods.get(i), RATES_AFTER, VOLS_AFTER).Amount;
		  floorExpected += PRICER_PERIOD.presentValue(FLOOR.CapletFloorletPeriods.get(i), RATES_AFTER, VOLS_AFTER).Amount;
		}
		assertEquals(capComputed.Currency, EUR);
		assertEquals(capComputed.Amount, capExpected, TOL * NOTIONAL_VALUE);
		assertEquals(floorComputed.Currency, EUR);
		assertEquals(floorComputed.Amount, floorExpected, TOL * NOTIONAL_VALUE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueDelta()
	  {
		CurrencyAmount capComputed = PRICER.presentValueDelta(CAP, RATES, VOLS);
		CurrencyAmount floorComputed = PRICER.presentValueDelta(FLOOR, RATES, VOLS);
		double capExpected = 0d;
		double floorExpected = 0d;
		int nPeriods = CAP.CapletFloorletPeriods.size();
		for (int i = 0; i < nPeriods; ++i)
		{
		  capExpected += PRICER_PERIOD.presentValueDelta(CAP.CapletFloorletPeriods.get(i), RATES, VOLS).Amount;
		  floorExpected += PRICER_PERIOD.presentValueDelta(FLOOR.CapletFloorletPeriods.get(i), RATES, VOLS).Amount;
		}
		assertEquals(capComputed.Currency, EUR);
		assertEquals(capComputed.Amount, capExpected, TOL * NOTIONAL_VALUE);
		assertEquals(floorComputed.Currency, EUR);
		assertEquals(floorComputed.Amount, floorExpected, TOL * NOTIONAL_VALUE);
	  }

	  public virtual void test_presentValueDelta_after()
	  {
		CurrencyAmount capComputed = PRICER.presentValueDelta(CAP, RATES_AFTER, VOLS_AFTER);
		CurrencyAmount floorComputed = PRICER.presentValueDelta(FLOOR, RATES_AFTER, VOLS_AFTER);
		double capExpected = 0d;
		double floorExpected = 0d;
		int nPeriods = CAP.CapletFloorletPeriods.size();
		for (int i = 2; i < nPeriods; ++i)
		{
		  capExpected += PRICER_PERIOD.presentValueDelta(CAP.CapletFloorletPeriods.get(i), RATES_AFTER, VOLS_AFTER).Amount;
		  floorExpected += PRICER_PERIOD.presentValueDelta(FLOOR.CapletFloorletPeriods.get(i), RATES_AFTER, VOLS_AFTER).Amount;
		}
		assertEquals(capComputed.Currency, EUR);
		assertEquals(capComputed.Amount, capExpected, TOL * NOTIONAL_VALUE);
		assertEquals(floorComputed.Currency, EUR);
		assertEquals(floorComputed.Amount, floorExpected, TOL * NOTIONAL_VALUE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueGamma()
	  {
		CurrencyAmount capComputed = PRICER.presentValueGamma(CAP, RATES, VOLS);
		CurrencyAmount floorComputed = PRICER.presentValueGamma(FLOOR, RATES, VOLS);
		double capExpected = 0d;
		double floorExpected = 0d;
		int nPeriods = CAP.CapletFloorletPeriods.size();
		for (int i = 0; i < nPeriods; ++i)
		{
		  capExpected += PRICER_PERIOD.presentValueGamma(CAP.CapletFloorletPeriods.get(i), RATES, VOLS).Amount;
		  floorExpected += PRICER_PERIOD.presentValueGamma(FLOOR.CapletFloorletPeriods.get(i), RATES, VOLS).Amount;
		}
		assertEquals(capComputed.Currency, EUR);
		assertEquals(capComputed.Amount, capExpected, TOL * NOTIONAL_VALUE);
		assertEquals(floorComputed.Currency, EUR);
		assertEquals(floorComputed.Amount, floorExpected, TOL * NOTIONAL_VALUE);
	  }

	  public virtual void test_presentValueGamma_after()
	  {
		CurrencyAmount capComputed = PRICER.presentValueGamma(CAP, RATES_AFTER, VOLS_AFTER);
		CurrencyAmount floorComputed = PRICER.presentValueGamma(FLOOR, RATES_AFTER, VOLS_AFTER);
		double capExpected = 0d;
		double floorExpected = 0d;
		int nPeriods = CAP.CapletFloorletPeriods.size();
		for (int i = 2; i < nPeriods; ++i)
		{
		  capExpected += PRICER_PERIOD.presentValueGamma(CAP.CapletFloorletPeriods.get(i), RATES_AFTER, VOLS_AFTER).Amount;
		  floorExpected += PRICER_PERIOD.presentValueGamma(FLOOR.CapletFloorletPeriods.get(i), RATES_AFTER, VOLS_AFTER).Amount;
		}
		assertEquals(capComputed.Currency, EUR);
		assertEquals(capComputed.Amount, capExpected, TOL * NOTIONAL_VALUE);
		assertEquals(floorComputed.Currency, EUR);
		assertEquals(floorComputed.Amount, floorExpected, TOL * NOTIONAL_VALUE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueTheta()
	  {
		CurrencyAmount capComputed = PRICER.presentValueTheta(CAP, RATES, VOLS);
		CurrencyAmount floorComputed = PRICER.presentValueTheta(FLOOR, RATES, VOLS);
		double capExpected = 0d;
		double floorExpected = 0d;
		int nPeriods = CAP.CapletFloorletPeriods.size();
		for (int i = 0; i < nPeriods; ++i)
		{
		  capExpected += PRICER_PERIOD.presentValueTheta(CAP.CapletFloorletPeriods.get(i), RATES, VOLS).Amount;
		  floorExpected += PRICER_PERIOD.presentValueTheta(FLOOR.CapletFloorletPeriods.get(i), RATES, VOLS).Amount;
		}
		assertEquals(capComputed.Currency, EUR);
		assertEquals(capComputed.Amount, capExpected, TOL * NOTIONAL_VALUE);
		assertEquals(floorComputed.Currency, EUR);
		assertEquals(floorComputed.Amount, floorExpected, TOL * NOTIONAL_VALUE);
	  }

	  public virtual void test_presentValueTheta_after()
	  {
		CurrencyAmount capComputed = PRICER.presentValueTheta(CAP, RATES_AFTER, VOLS_AFTER);
		CurrencyAmount floorComputed = PRICER.presentValueTheta(FLOOR, RATES_AFTER, VOLS_AFTER);
		double capExpected = 0d;
		double floorExpected = 0d;
		int nPeriods = CAP.CapletFloorletPeriods.size();
		for (int i = 2; i < nPeriods; ++i)
		{
		  capExpected += PRICER_PERIOD.presentValueTheta(CAP.CapletFloorletPeriods.get(i), RATES_AFTER, VOLS_AFTER).Amount;
		  floorExpected += PRICER_PERIOD.presentValueTheta(FLOOR.CapletFloorletPeriods.get(i), RATES_AFTER, VOLS_AFTER).Amount;
		}
		assertEquals(capComputed.Currency, EUR);
		assertEquals(capComputed.Amount, capExpected, TOL * NOTIONAL_VALUE);
		assertEquals(floorComputed.Currency, EUR);
		assertEquals(floorComputed.Amount, floorExpected, TOL * NOTIONAL_VALUE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity()
	  {
		PointSensitivityBuilder capComputed = PRICER.presentValueSensitivityRates(CAP, RATES, VOLS);
		PointSensitivityBuilder floorComputed = PRICER.presentValueSensitivityRates(FLOOR, RATES, VOLS);
		PointSensitivityBuilder capExpected = PointSensitivityBuilder.none();
		PointSensitivityBuilder floorExpected = PointSensitivityBuilder.none();
		int nPeriods = CAP.CapletFloorletPeriods.size();
		for (int i = 0; i < nPeriods; ++i)
		{
		  capExpected = capExpected.combinedWith(PRICER_PERIOD.presentValueSensitivityRates(CAP.CapletFloorletPeriods.get(i), RATES, VOLS));
		  floorExpected = floorExpected.combinedWith(PRICER_PERIOD.presentValueSensitivityRates(FLOOR.CapletFloorletPeriods.get(i), RATES, VOLS));
		}
		CurrencyParameterSensitivities capSensiComputed = RATES_AFTER.parameterSensitivity(capComputed.build());
		CurrencyParameterSensitivities floorSensiComputed = RATES_AFTER.parameterSensitivity(floorComputed.build());
		CurrencyParameterSensitivities capSensiExpected = RATES_AFTER.parameterSensitivity(capExpected.build());
		CurrencyParameterSensitivities floorSensiExpected = RATES_AFTER.parameterSensitivity(floorExpected.build());
		assertTrue(capSensiComputed.equalWithTolerance(capSensiExpected, NOTIONAL_VALUE * TOL));
		assertTrue(floorSensiComputed.equalWithTolerance(floorSensiExpected, NOTIONAL_VALUE * TOL));
	  }

	  public virtual void test_presentValueSensitivity_after()
	  {
		PointSensitivityBuilder capComputed = PRICER.presentValueSensitivityRates(CAP, RATES_AFTER, VOLS_AFTER);
		PointSensitivityBuilder floorComputed = PRICER.presentValueSensitivityRates(FLOOR, RATES_AFTER, VOLS_AFTER);
		PointSensitivityBuilder capExpected = PointSensitivityBuilder.none();
		IborCapletFloorletPeriod period = FLOOR.CapletFloorletPeriods.get(1);
		PointSensitivityBuilder floorExpected = RATES_AFTER.discountFactors(EUR).zeroRatePointSensitivity(period.PaymentDate).multipliedBy(-(STRIKE - OBS_INDEX_2) * NOTIONAL_VALUE * period.YearFraction);
		int nPeriods = CAP.CapletFloorletPeriods.size();
		for (int i = 2; i < nPeriods; ++i)
		{
		  capExpected = capExpected.combinedWith(PRICER_PERIOD.presentValueSensitivityRates(CAP.CapletFloorletPeriods.get(i), RATES_AFTER, VOLS_AFTER));
		  floorExpected = floorExpected.combinedWith(PRICER_PERIOD.presentValueSensitivityRates(FLOOR.CapletFloorletPeriods.get(i), RATES_AFTER, VOLS_AFTER));
		}
		CurrencyParameterSensitivities capSensiComputed = RATES_AFTER.parameterSensitivity(capComputed.build());
		CurrencyParameterSensitivities floorSensiComputed = RATES_AFTER.parameterSensitivity(floorComputed.build());
		CurrencyParameterSensitivities capSensiExpected = RATES_AFTER.parameterSensitivity(capExpected.build());
		CurrencyParameterSensitivities floorSensiExpected = RATES_AFTER.parameterSensitivity(floorExpected.build());
		assertTrue(capSensiComputed.equalWithTolerance(capSensiExpected, NOTIONAL_VALUE * TOL));
		assertTrue(floorSensiComputed.equalWithTolerance(floorSensiExpected, NOTIONAL_VALUE * TOL));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivityVolatility()
	  {
		PointSensitivityBuilder capComputed = PRICER.presentValueSensitivityModelParamsVolatility(CAP, RATES, VOLS);
		PointSensitivityBuilder floorComputed = PRICER.presentValueSensitivityModelParamsVolatility(FLOOR, RATES, VOLS);
		CurrencyParameterSensitivities capExpected = CurrencyParameterSensitivities.empty();
		CurrencyParameterSensitivities floorExpected = CurrencyParameterSensitivities.empty();
		int nPeriods = CAP.CapletFloorletPeriods.size();
		for (int i = 0; i < nPeriods; ++i)
		{
		  capExpected = capExpected.combinedWith(VOLS.parameterSensitivity(PRICER_PERIOD.presentValueSensitivityModelParamsVolatility(CAP.CapletFloorletPeriods.get(i), RATES, VOLS).build()));
		  floorExpected = floorExpected.combinedWith(VOLS.parameterSensitivity(PRICER_PERIOD.presentValueSensitivityModelParamsVolatility(FLOOR.CapletFloorletPeriods.get(i), RATES, VOLS).build()));
		}
		CurrencyParameterSensitivities capSensiComputed = VOLS.parameterSensitivity(capComputed.build());
		CurrencyParameterSensitivities floorSensiComputed = VOLS.parameterSensitivity(floorComputed.build());
		CurrencyParameterSensitivity capSensiExpected = capExpected.Sensitivities.get(0);
		CurrencyParameterSensitivity floorSensiExpected = floorExpected.Sensitivities.get(0);
		assertTrue(DoubleArrayMath.fuzzyEquals(capSensiComputed.Sensitivities.get(0).Sensitivity.toArray(), capSensiExpected.Sensitivity.toArray(), TOL * NOTIONAL_VALUE));
		assertTrue(DoubleArrayMath.fuzzyEquals(floorSensiComputed.Sensitivities.get(0).Sensitivity.toArray(), floorSensiExpected.Sensitivity.toArray(), TOL * NOTIONAL_VALUE));
	  }

	  public virtual void test_presentValueSensitivityVolatility_after()
	  {
		PointSensitivityBuilder capComputed = PRICER.presentValueSensitivityModelParamsVolatility(CAP, RATES_AFTER, VOLS_AFTER);
		PointSensitivityBuilder floorComputed = PRICER.presentValueSensitivityModelParamsVolatility(FLOOR, RATES_AFTER, VOLS_AFTER);
		CurrencyParameterSensitivities capExpected = CurrencyParameterSensitivities.empty();
		CurrencyParameterSensitivities floorExpected = CurrencyParameterSensitivities.empty();
		int nPeriods = CAP.CapletFloorletPeriods.size();
		for (int i = 3; i < nPeriods; ++i)
		{
		  capExpected = capExpected.combinedWith(VOLS_AFTER.parameterSensitivity(PRICER_PERIOD.presentValueSensitivityModelParamsVolatility(CAP.CapletFloorletPeriods.get(i), RATES_AFTER, VOLS_AFTER).build()));
		  floorExpected = floorExpected.combinedWith(VOLS_AFTER.parameterSensitivity(PRICER_PERIOD.presentValueSensitivityModelParamsVolatility(FLOOR.CapletFloorletPeriods.get(i), RATES_AFTER, VOLS_AFTER).build()));
		}
		CurrencyParameterSensitivities capSensiComputed = VOLS_AFTER.parameterSensitivity(capComputed.build());
		CurrencyParameterSensitivities floorSensiComputed = VOLS_AFTER.parameterSensitivity(floorComputed.build());
		CurrencyParameterSensitivity capSensiExpected = capExpected.Sensitivities.get(0);
		CurrencyParameterSensitivity floorSensiExpected = floorExpected.Sensitivities.get(0);
		assertTrue(DoubleArrayMath.fuzzyEquals(capSensiComputed.Sensitivities.get(0).Sensitivity.toArray(), capSensiExpected.Sensitivity.toArray(), TOL * NOTIONAL_VALUE));
		assertTrue(DoubleArrayMath.fuzzyEquals(floorSensiComputed.Sensitivities.get(0).Sensitivity.toArray(), floorSensiExpected.Sensitivity.toArray(), TOL * NOTIONAL_VALUE));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currentCash()
	  {
		CurrencyAmount capComputed = PRICER.currentCash(CAP, RATES, VOLS);
		CurrencyAmount floorComputed = PRICER.currentCash(FLOOR, RATES, VOLS);
		assertEquals(capComputed.Currency, EUR);
		assertEquals(capComputed.Amount, 0d);
		assertEquals(floorComputed.Currency, EUR);
		assertEquals(floorComputed.Amount, 0d);
	  }

	  public virtual void test_currentCash_pay()
	  {
		CurrencyAmount capComputed = PRICER.currentCash(CAP, RATES_PAY, VOLS_PAY);
		CurrencyAmount floorComputed = PRICER.currentCash(FLOOR, RATES_PAY, VOLS_PAY);
		double capExpected = 0d;
		IborCapletFloorletPeriod period = FLOOR.CapletFloorletPeriods.get(1);
		double floorExpected = -(STRIKE - OBS_INDEX_2) * NOTIONAL_VALUE * period.YearFraction;
		assertEquals(capComputed.Currency, EUR);
		assertEquals(capComputed.Amount, capExpected);
		assertEquals(floorComputed.Currency, EUR);
		assertEquals(floorComputed.Amount, floorExpected, TOL * NOTIONAL_VALUE);
	  }

	}

}