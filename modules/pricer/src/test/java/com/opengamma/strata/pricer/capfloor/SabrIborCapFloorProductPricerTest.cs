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
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using DiscountingSwapLegPricer = com.opengamma.strata.pricer.swap.DiscountingSwapLegPricer;
	using ResolvedIborCapFloor = com.opengamma.strata.product.capfloor.ResolvedIborCapFloor;
	using ResolvedIborCapFloorLeg = com.opengamma.strata.product.capfloor.ResolvedIborCapFloorLeg;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;

	/// <summary>
	/// Test <seealso cref="SabrIborCapFloorProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrIborCapFloorProductPricerTest
	public class SabrIborCapFloorProductPricerTest
	{

	  private const double NOTIONAL_VALUE = 1.0e6;
	  private static readonly ValueSchedule NOTIONAL = ValueSchedule.of(NOTIONAL_VALUE);
	  private static readonly LocalDate START = LocalDate.of(2015, 10, 21);
	  private static readonly LocalDate END = LocalDate.of(2020, 10, 21);
	  private const double STRIKE_VALUE = 0.0105;
	  private static readonly ValueSchedule STRIKE = ValueSchedule.of(STRIKE_VALUE);
	  private static readonly ResolvedIborCapFloorLeg CAP_LEG = IborCapFloorDataSet.createCapFloorLeg(EUR_EURIBOR_3M, START, END, STRIKE, NOTIONAL, CALL, RECEIVE);
	  private static readonly ResolvedSwapLeg PAY_LEG = IborCapFloorDataSet.createFixedPayLeg(EUR_EURIBOR_3M, START, END, 0.0015, NOTIONAL_VALUE, PAY);
	  private static readonly ResolvedIborCapFloor CAP_TWO_LEGS = ResolvedIborCapFloor.of(CAP_LEG, PAY_LEG);
	  private static readonly ResolvedIborCapFloor CAP_ONE_LEG = ResolvedIborCapFloor.of(CAP_LEG);

	  // valuation before start
	  private static readonly ZonedDateTime VALUATION = dateUtc(2015, 8, 20);
	  private static readonly ImmutableRatesProvider RATES = IborCapletFloorletSabrRateVolatilityDataSet.getRatesProvider(VALUATION.toLocalDate(), EUR_EURIBOR_3M, LocalDateDoubleTimeSeries.empty());
	  private static readonly SabrIborCapletFloorletVolatilities VOLS = IborCapletFloorletSabrRateVolatilityDataSet.getVolatilities(VALUATION, EUR_EURIBOR_3M);
	  // valuation at payment of 1st period
	  private const double OBS_INDEX_1 = 0.012;
	  private const double OBS_INDEX_2 = 0.0125;
	  private static readonly LocalDateDoubleTimeSeries TIME_SERIES = LocalDateDoubleTimeSeries.builder().put(date(2015, 10, 19), OBS_INDEX_1).put(date(2016, 1, 19), OBS_INDEX_2).build();
	  private static readonly ZonedDateTime VALUATION_PAY = dateUtc(2016, 1, 21);
	  private static readonly ImmutableRatesProvider RATES_PAY = IborCapletFloorletSabrRateVolatilityDataSet.getRatesProvider(VALUATION_PAY.toLocalDate(), EUR_EURIBOR_3M, TIME_SERIES);
	  private static readonly SabrIborCapletFloorletVolatilities VOLS_PAY = IborCapletFloorletSabrRateVolatilityDataSet.getVolatilities(VALUATION_PAY, EUR_EURIBOR_3M);

	  private const double TOL = 1.0e-13;
	  private static readonly SabrIborCapFloorProductPricer PRICER = SabrIborCapFloorProductPricer.DEFAULT;
	  private static readonly SabrIborCapFloorLegPricer PRICER_CAP_LEG = SabrIborCapFloorLegPricer.DEFAULT;
	  private static readonly DiscountingSwapLegPricer PRICER_PAY_LEG = DiscountingSwapLegPricer.DEFAULT;

	  public virtual void test_presentValue()
	  {
		MultiCurrencyAmount computed1 = PRICER.presentValue(CAP_ONE_LEG, RATES, VOLS);
		MultiCurrencyAmount computed2 = PRICER.presentValue(CAP_TWO_LEGS, RATES, VOLS);
		CurrencyAmount cap = PRICER_CAP_LEG.presentValue(CAP_LEG, RATES, VOLS);
		CurrencyAmount pay = PRICER_PAY_LEG.presentValue(PAY_LEG, RATES);
		assertEquals(computed1, MultiCurrencyAmount.of(cap));
		assertEquals(computed2, MultiCurrencyAmount.of(cap.plus(pay)));
	  }

	  public virtual void test_presentValueDelta()
	  {
		MultiCurrencyAmount computed1 = PRICER.presentValueDelta(CAP_ONE_LEG, RATES, VOLS);
		MultiCurrencyAmount computed2 = PRICER.presentValueDelta(CAP_TWO_LEGS, RATES, VOLS);
		CurrencyAmount cap = PRICER_CAP_LEG.presentValueDelta(CAP_LEG, RATES, VOLS);
		assertEquals(computed1, MultiCurrencyAmount.of(cap));
		assertEquals(computed2, MultiCurrencyAmount.of(cap));
	  }

	  public virtual void test_presentValueGamma()
	  {
		MultiCurrencyAmount computed1 = PRICER.presentValueGamma(CAP_ONE_LEG, RATES, VOLS);
		MultiCurrencyAmount computed2 = PRICER.presentValueGamma(CAP_TWO_LEGS, RATES, VOLS);
		CurrencyAmount cap = PRICER_CAP_LEG.presentValueGamma(CAP_LEG, RATES, VOLS);
		assertEquals(computed1, MultiCurrencyAmount.of(cap));
		assertEquals(computed2, MultiCurrencyAmount.of(cap));
	  }

	  public virtual void test_presentValueTheta()
	  {
		MultiCurrencyAmount computed1 = PRICER.presentValueTheta(CAP_ONE_LEG, RATES, VOLS);
		MultiCurrencyAmount computed2 = PRICER.presentValueTheta(CAP_TWO_LEGS, RATES, VOLS);
		CurrencyAmount cap = PRICER_CAP_LEG.presentValueTheta(CAP_LEG, RATES, VOLS);
		assertEquals(computed1, MultiCurrencyAmount.of(cap));
		assertEquals(computed2, MultiCurrencyAmount.of(cap));
	  }

	  public virtual void test_presentValueSensitivity()
	  {
		PointSensitivityBuilder computed1 = PRICER.presentValueSensitivityRates(CAP_ONE_LEG, RATES, VOLS);
		PointSensitivityBuilder computed2 = PRICER.presentValueSensitivityRates(CAP_TWO_LEGS, RATES, VOLS);
		PointSensitivityBuilder cap = PRICER_CAP_LEG.presentValueSensitivityRates(CAP_LEG, RATES, VOLS);
		PointSensitivityBuilder pay = PRICER_PAY_LEG.presentValueSensitivity(PAY_LEG, RATES);
		assertEquals(computed1, cap);
		assertEquals(computed2, cap.combinedWith(pay));
	  }

	  public virtual void test_presentValueSensitivityRatesStickyModel()
	  {
		PointSensitivityBuilder computed1 = PRICER.presentValueSensitivityRatesStickyModel(CAP_ONE_LEG, RATES, VOLS);
		PointSensitivityBuilder computed2 = PRICER.presentValueSensitivityRatesStickyModel(CAP_TWO_LEGS, RATES, VOLS);
		PointSensitivityBuilder cap = PRICER_CAP_LEG.presentValueSensitivityRatesStickyModel(CAP_LEG, RATES, VOLS);
		PointSensitivityBuilder pay = PRICER_PAY_LEG.presentValueSensitivity(PAY_LEG, RATES);
		assertEquals(computed1, cap);
		assertEquals(computed2, cap.combinedWith(pay));
	  }

	  public virtual void test_presentValueSensitivityVolatility()
	  {
		PointSensitivityBuilder computed1 = PRICER.presentValueSensitivityModelParamsVolatility(CAP_ONE_LEG, RATES, VOLS);
		PointSensitivityBuilder computed2 = PRICER.presentValueSensitivityModelParamsVolatility(CAP_TWO_LEGS, RATES, VOLS);
		PointSensitivityBuilder cap = PRICER_CAP_LEG.presentValueSensitivityModelParamsVolatility(CAP_LEG, RATES, VOLS);
		assertEquals(computed1, cap);
		assertEquals(computed2, cap);
	  }

	  public virtual void test_presentValueSensitivityModelParamsSabr()
	  {
		PointSensitivityBuilder computed1 = PRICER.presentValueSensitivityModelParamsSabr(CAP_ONE_LEG, RATES, VOLS);
		PointSensitivityBuilder computed2 = PRICER.presentValueSensitivityModelParamsSabr(CAP_TWO_LEGS, RATES, VOLS);
		PointSensitivityBuilder cap = PRICER_CAP_LEG.presentValueSensitivityModelParamsSabr(CAP_LEG, RATES, VOLS);
		assertEquals(computed1, cap);
		assertEquals(computed2, cap);
	  }

	  public virtual void test_currencyExposure()
	  {
		MultiCurrencyAmount computed1 = PRICER.currencyExposure(CAP_ONE_LEG, RATES, VOLS);
		MultiCurrencyAmount computed2 = PRICER.currencyExposure(CAP_TWO_LEGS, RATES, VOLS);
		MultiCurrencyAmount pv1 = PRICER.presentValue(CAP_ONE_LEG, RATES, VOLS);
		MultiCurrencyAmount pv2 = PRICER.presentValue(CAP_TWO_LEGS, RATES, VOLS);
		PointSensitivityBuilder point1 = PRICER.presentValueSensitivityRates(CAP_ONE_LEG, RATES, VOLS);
		PointSensitivityBuilder point2 = PRICER.presentValueSensitivityRates(CAP_TWO_LEGS, RATES, VOLS);
		MultiCurrencyAmount expected1 = RATES.currencyExposure(point1.build()).plus(pv1);
		MultiCurrencyAmount expected2 = RATES.currencyExposure(point2.build()).plus(pv2);
		assertEquals(computed1.getAmount(EUR).Amount, expected1.getAmount(EUR).Amount, NOTIONAL_VALUE * TOL);
		assertEquals(computed2.getAmount(EUR).Amount, expected2.getAmount(EUR).Amount, NOTIONAL_VALUE * TOL);
	  }

	  public virtual void test_currentCash()
	  {
		MultiCurrencyAmount cc1 = PRICER.currentCash(CAP_ONE_LEG, RATES, VOLS);
		MultiCurrencyAmount cc2 = PRICER.currentCash(CAP_TWO_LEGS, RATES, VOLS);
		assertEquals(cc1, MultiCurrencyAmount.of(CurrencyAmount.zero(EUR)));
		assertEquals(cc2, MultiCurrencyAmount.of(CurrencyAmount.zero(EUR)));
	  }

	  public virtual void test_currentCash_onPay()
	  {
		MultiCurrencyAmount cc1 = PRICER.currentCash(CAP_ONE_LEG, RATES_PAY, VOLS_PAY);
		MultiCurrencyAmount cc2 = PRICER.currentCash(CAP_TWO_LEGS, RATES_PAY, VOLS_PAY);
		CurrencyAmount ccCap = PRICER_CAP_LEG.currentCash(CAP_LEG, RATES_PAY, VOLS_PAY);
		CurrencyAmount ccPay = PRICER_PAY_LEG.currentCash(PAY_LEG, RATES_PAY);
		assertEquals(cc1, MultiCurrencyAmount.of(ccCap));
		assertEquals(cc2, MultiCurrencyAmount.of(ccCap).plus(ccPay));
	  }

	}

}