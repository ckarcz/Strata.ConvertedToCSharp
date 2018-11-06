using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.cms
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueAdjustment = com.opengamma.strata.basics.value.ValueAdjustment;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using ValueStep = com.opengamma.strata.basics.value.ValueStep;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using DiscountingCmsPeriodPricer = com.opengamma.strata.pricer.impl.cms.DiscountingCmsPeriodPricer;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using SwaptionSabrRateVolatilityDataSet = com.opengamma.strata.pricer.swaption.SwaptionSabrRateVolatilityDataSet;
	using CmsLeg = com.opengamma.strata.product.cms.CmsLeg;
	using CmsPeriod = com.opengamma.strata.product.cms.CmsPeriod;
	using ResolvedCmsLeg = com.opengamma.strata.product.cms.ResolvedCmsLeg;
	using SwapIndex = com.opengamma.strata.product.swap.SwapIndex;
	using SwapIndices = com.opengamma.strata.product.swap.SwapIndices;

	/// <summary>
	/// Test <seealso cref="DiscountingCmsLegPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingCmsLegPricerTest
	public class DiscountingCmsLegPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  // CMS legs
	  private static readonly SwapIndex INDEX = SwapIndices.EUR_EURIBOR_1100_5Y;
	  private static readonly LocalDate START = LocalDate.of(2015, 10, 21);
	  private static readonly LocalDate END = LocalDate.of(2020, 10, 21);
	  private static readonly Frequency FREQUENCY = Frequency.P12M;
	  private static readonly BusinessDayAdjustment BUSS_ADJ_EUR = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, EUTA);
	  private static readonly PeriodicSchedule SCHEDULE_EUR = PeriodicSchedule.of(START, END, FREQUENCY, BUSS_ADJ_EUR, StubConvention.NONE, RollConventions.NONE);
	  private static readonly IList<ValueStep> NOTIONAL_STEPS = new List<ValueStep>();
	  private const double NOTIONAL_VALUE_0 = 100_000_000;
	  private const double NOTIONAL_VALUE_1 = 1.1e6;
	  private const double NOTIONAL_VALUE_2 = 0.9e6;
	  private const double NOTIONAL_VALUE_3 = 1.2e6;
	  static DiscountingCmsLegPricerTest()
	  {
		NOTIONAL_STEPS.Add(ValueStep.of(1, ValueAdjustment.ofReplace(NOTIONAL_VALUE_1)));
		NOTIONAL_STEPS.Add(ValueStep.of(2, ValueAdjustment.ofReplace(NOTIONAL_VALUE_2)));
		NOTIONAL_STEPS.Add(ValueStep.of(3, ValueAdjustment.ofReplace(NOTIONAL_VALUE_3)));
	  }
	  private static readonly ValueSchedule NOTIONAL = ValueSchedule.of(NOTIONAL_VALUE_0, NOTIONAL_STEPS);
	  private static readonly ResolvedCmsLeg COUPON_LEG = CmsLeg.builder().index(INDEX).notional(NOTIONAL).payReceive(PAY).paymentSchedule(SCHEDULE_EUR).build().resolve(REF_DATA);
	  // providers
	  private static readonly LocalDate VALUATION = LocalDate.of(2015, 8, 18);
	  private static readonly ImmutableRatesProvider RATES_PROVIDER = SwaptionSabrRateVolatilityDataSet.getRatesProviderEur(VALUATION);
	  // providers - valuation after the first payment
	  private static readonly LocalDate AFTER_PAYMENT = LocalDate.of(2016, 11, 25); // the first cms payment is 2016-10-21.
	  private static readonly LocalDate FIXING = LocalDate.of(2016, 10, 19); // fixing for the second period.
	  private const double OBS_INDEX = 0.013;
	  private static readonly LocalDateDoubleTimeSeries TIME_SERIES = LocalDateDoubleTimeSeries.of(FIXING, OBS_INDEX);
	  private static readonly ImmutableRatesProvider RATES_PROVIDER_AFTER_PERIOD = SwaptionSabrRateVolatilityDataSet.getRatesProviderEur(AFTER_PAYMENT, TIME_SERIES);
	  // providers - valuation on the payment date
	  private static readonly LocalDate PAYMENT = LocalDate.of(2017, 10, 23); // payment date of the second payment
	  private static readonly ImmutableRatesProvider RATES_PROVIDER_ON_PAY = SwaptionSabrRateVolatilityDataSet.getRatesProviderEur(PAYMENT, TIME_SERIES);
	  // providers - valuation after maturity date
	  private static readonly LocalDate ENDED = END.plusDays(7);
	  private static readonly ImmutableRatesProvider RATES_PROVIDER_ENDED = SwaptionSabrRateVolatilityDataSet.getRatesProviderEur(ENDED);
	  // pricers
	  private const double EPS = 1.0e-5;
	  private const double TOLERANCE_PV = 1.0E-2;
	  private const double TOLERANCE_DELTA = 1.0E+3;
	  private static readonly DiscountingCmsPeriodPricer PERIOD_PRICER = DiscountingCmsPeriodPricer.DEFAULT;
	  private static readonly DiscountingCmsLegPricer LEG_PRICER = new DiscountingCmsLegPricer(PERIOD_PRICER);
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FD_CAL = new RatesFiniteDifferenceSensitivityCalculator(EPS);

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		CurrencyAmount computed = LEG_PRICER.presentValue(COUPON_LEG, RATES_PROVIDER);
		double expected = 0d;
		IList<CmsPeriod> cms = COUPON_LEG.CmsPeriods;
		int size = cms.Count;
		for (int i = 0; i < size; ++i)
		{
		  expected += PERIOD_PRICER.presentValue(cms[i], RATES_PROVIDER).Amount;
		}
		assertEquals(computed.Currency, EUR);
		assertEquals(computed.Amount, expected, TOLERANCE_PV);
	  }

	  public virtual void test_presentValue_afterPay()
	  {
		CurrencyAmount computed = LEG_PRICER.presentValue(COUPON_LEG, RATES_PROVIDER_AFTER_PERIOD);
		double expected = 0d;
		IList<CmsPeriod> cms = COUPON_LEG.CmsPeriods;
		int size = cms.Count;
		for (int i = 1; i < size; ++i)
		{
		  expected += PERIOD_PRICER.presentValue(cms[i], RATES_PROVIDER_AFTER_PERIOD).Amount;
		}
		assertEquals(computed.Currency, EUR);
		assertEquals(computed.Amount, expected, TOLERANCE_PV);
	  }

	  public virtual void test_presentValue_ended()
	  {
		CurrencyAmount computed = LEG_PRICER.presentValue(COUPON_LEG, RATES_PROVIDER_ENDED);
		assertEquals(computed, CurrencyAmount.zero(EUR));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity()
	  {
		PointSensitivityBuilder point = LEG_PRICER.presentValueSensitivity(COUPON_LEG, RATES_PROVIDER);
		CurrencyParameterSensitivities computed = RATES_PROVIDER.parameterSensitivity(point.build());
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATES_PROVIDER, p => LEG_PRICER.presentValue(COUPON_LEG, p));
		assertTrue(computed.equalWithTolerance(expected, TOLERANCE_DELTA));
	  }

	  public virtual void test_presentValueSensitivity_afterPay()
	  {
		PointSensitivityBuilder point = LEG_PRICER.presentValueSensitivity(COUPON_LEG, RATES_PROVIDER_AFTER_PERIOD);
		CurrencyParameterSensitivities computed = RATES_PROVIDER_AFTER_PERIOD.parameterSensitivity(point.build());
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATES_PROVIDER_AFTER_PERIOD, p => LEG_PRICER.presentValue(COUPON_LEG, p));
		assertTrue(computed.equalWithTolerance(expected, TOLERANCE_DELTA));
	  }

	  public virtual void test_presentValueSensitivity_ended()
	  {
		PointSensitivityBuilder computed = LEG_PRICER.presentValueSensitivity(COUPON_LEG, RATES_PROVIDER_ENDED);
		assertEquals(computed, PointSensitivityBuilder.none());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currentCash()
	  {
		CurrencyAmount computed = LEG_PRICER.currentCash(COUPON_LEG, RATES_PROVIDER);
		assertEquals(computed, CurrencyAmount.zero(EUR));
	  }

	  public virtual void test_currentCash_onPay()
	  {
		CurrencyAmount computed = LEG_PRICER.currentCash(COUPON_LEG, RATES_PROVIDER_ON_PAY);
		assertEquals(computed.Amount, -NOTIONAL_VALUE_1 * OBS_INDEX * 367d / 360d, TOLERANCE_PV);
	  }

	}

}