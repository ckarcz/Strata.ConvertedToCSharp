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
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
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
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using SabrParametersSwaptionVolatilities = com.opengamma.strata.pricer.swaption.SabrParametersSwaptionVolatilities;
	using SwaptionSabrRateVolatilityDataSet = com.opengamma.strata.pricer.swaption.SwaptionSabrRateVolatilityDataSet;
	using CmsLeg = com.opengamma.strata.product.cms.CmsLeg;
	using CmsPeriod = com.opengamma.strata.product.cms.CmsPeriod;
	using ResolvedCmsLeg = com.opengamma.strata.product.cms.ResolvedCmsLeg;
	using SwapIndex = com.opengamma.strata.product.swap.SwapIndex;
	using SwapIndices = com.opengamma.strata.product.swap.SwapIndices;

	/// <summary>
	/// Test <seealso cref="SabrExtrapolationReplicationCmsLegPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrExtrapolationReplicationCmsLegPricerTest
	public class SabrExtrapolationReplicationCmsLegPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  // CMS legs
	  private static readonly SwapIndex INDEX = SwapIndices.EUR_EURIBOR_1100_5Y;
	  private static readonly LocalDate START = LocalDate.of(2015, 10, 21);
	  private static readonly LocalDate END = LocalDate.of(2020, 10, 21);
	  private static readonly Frequency FREQUENCY = Frequency.P12M;
	  private static readonly BusinessDayAdjustment BUSS_ADJ_EUR = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, EUTA);
	  private static readonly PeriodicSchedule SCHEDULE_EUR = PeriodicSchedule.of(START, END, FREQUENCY, BUSS_ADJ_EUR, StubConvention.NONE, RollConventions.NONE);
	  private const double CAP_VALUE = 0.0125;
	  private static readonly ValueSchedule CAP_STRIKE = ValueSchedule.of(CAP_VALUE);
	  private static readonly IList<ValueStep> FLOOR_STEPS = new List<ValueStep>();
	  private static readonly IList<ValueStep> NOTIONAL_STEPS = new List<ValueStep>();
	  private const double FLOOR_VALUE_0 = 0.014;
	  private const double FLOOR_VALUE_1 = 0.0135;
	  private const double FLOOR_VALUE_2 = 0.012;
	  private const double FLOOR_VALUE_3 = 0.013;
	  private const double NOTIONAL_VALUE_0 = 1.0e6;
	  private const double NOTIONAL_VALUE_1 = 1.1e6;
	  private const double NOTIONAL_VALUE_2 = 0.9e6;
	  private const double NOTIONAL_VALUE_3 = 1.2e6;
	  static SabrExtrapolationReplicationCmsLegPricerTest()
	  {
		FLOOR_STEPS.Add(ValueStep.of(1, ValueAdjustment.ofReplace(FLOOR_VALUE_1)));
		FLOOR_STEPS.Add(ValueStep.of(2, ValueAdjustment.ofReplace(FLOOR_VALUE_2)));
		FLOOR_STEPS.Add(ValueStep.of(3, ValueAdjustment.ofReplace(FLOOR_VALUE_3)));
		NOTIONAL_STEPS.Add(ValueStep.of(1, ValueAdjustment.ofReplace(NOTIONAL_VALUE_1)));
		NOTIONAL_STEPS.Add(ValueStep.of(2, ValueAdjustment.ofReplace(NOTIONAL_VALUE_2)));
		NOTIONAL_STEPS.Add(ValueStep.of(3, ValueAdjustment.ofReplace(NOTIONAL_VALUE_3)));
	  }
	  private static readonly ValueSchedule FLOOR_STRIKE = ValueSchedule.of(FLOOR_VALUE_0, FLOOR_STEPS);
	  private static readonly ValueSchedule NOTIONAL = ValueSchedule.of(NOTIONAL_VALUE_0, NOTIONAL_STEPS);
	  private static readonly ResolvedCmsLeg CAP_LEG = CmsLeg.builder().capSchedule(CAP_STRIKE).index(INDEX).notional(NOTIONAL).payReceive(RECEIVE).paymentSchedule(SCHEDULE_EUR).build().resolve(REF_DATA);
	  private static readonly ResolvedCmsLeg FLOOR_LEG = CmsLeg.builder().floorSchedule(FLOOR_STRIKE).index(INDEX).notional(NOTIONAL).payReceive(RECEIVE).paymentSchedule(SCHEDULE_EUR).build().resolve(REF_DATA);
	  private static readonly ResolvedCmsLeg COUPON_LEG = CmsLeg.builder().index(INDEX).notional(NOTIONAL).payReceive(PAY).paymentSchedule(SCHEDULE_EUR).build().resolve(REF_DATA);
	  // providers
	  private static readonly LocalDate VALUATION = LocalDate.of(2015, 8, 18);
	  private static readonly ImmutableRatesProvider RATES_PROVIDER = SwaptionSabrRateVolatilityDataSet.getRatesProviderEur(VALUATION);
	  private static readonly SabrParametersSwaptionVolatilities VOLATILITIES = SwaptionSabrRateVolatilityDataSet.getVolatilitiesEur(VALUATION, true);
	  // providers - valuation after the first payment
	  private static readonly LocalDate AFTER_PAYMENT = LocalDate.of(2016, 11, 25); // the first cms payment is 2016-10-21.
	  private static readonly LocalDate FIXING = LocalDate.of(2016, 10, 19); // fixing for the second period.
	  private const double OBS_INDEX = 0.013;
	  private static readonly LocalDateDoubleTimeSeries TIME_SERIES = LocalDateDoubleTimeSeries.of(FIXING, OBS_INDEX);
	  private static readonly ImmutableRatesProvider RATES_PROVIDER_AFTER_PERIOD = SwaptionSabrRateVolatilityDataSet.getRatesProviderEur(AFTER_PAYMENT, TIME_SERIES);
	  private static readonly SabrParametersSwaptionVolatilities VOLATILITIES_AFTER_PERIOD = SwaptionSabrRateVolatilityDataSet.getVolatilitiesEur(AFTER_PAYMENT, true);
	  // providers - valuation on the payment date
	  private static readonly LocalDate PAYMENT = LocalDate.of(2017, 10, 23); // payment date of the second payment
	  private static readonly ImmutableRatesProvider RATES_PROVIDER_ON_PAY = SwaptionSabrRateVolatilityDataSet.getRatesProviderEur(PAYMENT, TIME_SERIES);
	  private static readonly SabrParametersSwaptionVolatilities VOLATILITIES_ON_PAY = SwaptionSabrRateVolatilityDataSet.getVolatilitiesEur(PAYMENT, true);
	  // providers - valuation after maturity date
	  private static readonly LocalDate ENDED = END.plusDays(7);
	  private static readonly ImmutableRatesProvider RATES_PROVIDER_ENDED = SwaptionSabrRateVolatilityDataSet.getRatesProviderEur(ENDED);
	  private static readonly SabrParametersSwaptionVolatilities VOLATILITIES_ENDED = SwaptionSabrRateVolatilityDataSet.getVolatilitiesEur(ENDED, true);
	  // pricers
	  private const double CUT_OFF_STRIKE = 0.10;
	  private const double MU = 2.50;
	  private const double EPS = 1.0e-5;
	  private const double TOL = 1.0e-12;
	  private static readonly SabrExtrapolationReplicationCmsPeriodPricer PERIOD_PRICER = SabrExtrapolationReplicationCmsPeriodPricer.of(CUT_OFF_STRIKE, MU);
	  private static readonly SabrExtrapolationReplicationCmsLegPricer LEG_PRICER = new SabrExtrapolationReplicationCmsLegPricer(PERIOD_PRICER);
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FD_CAL = new RatesFiniteDifferenceSensitivityCalculator(EPS);
	  private static readonly DiscountingSwapProductPricer PRICER_SWAP = DiscountingSwapProductPricer.DEFAULT;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		CurrencyAmount computed = LEG_PRICER.presentValue(CAP_LEG, RATES_PROVIDER, VOLATILITIES);
		double expected = 0d;
		IList<CmsPeriod> cms = CAP_LEG.CmsPeriods;
		int size = cms.Count;
		for (int i = 0; i < size; ++i)
		{
		  expected += PERIOD_PRICER.presentValue(cms[i], RATES_PROVIDER, VOLATILITIES).Amount;
		}
		assertEquals(computed.Currency, EUR);
		assertEquals(computed.Amount, expected, NOTIONAL_VALUE_0 * TOL);
	  }

	  public virtual void test_presentValue_afterPay()
	  {
		CurrencyAmount computed = LEG_PRICER.presentValue(FLOOR_LEG, RATES_PROVIDER_AFTER_PERIOD, VOLATILITIES_AFTER_PERIOD);
		double expected = 0d;
		IList<CmsPeriod> cms = FLOOR_LEG.CmsPeriods;
		int size = cms.Count;
		for (int i = 1; i < size; ++i)
		{
		  expected += PERIOD_PRICER.presentValue(cms[i], RATES_PROVIDER_AFTER_PERIOD, VOLATILITIES_AFTER_PERIOD).Amount;
		}
		assertEquals(computed.Currency, EUR);
		assertEquals(computed.Amount, expected, NOTIONAL_VALUE_0 * TOL);
	  }

	  public virtual void test_presentValue_ended()
	  {
		CurrencyAmount computed = LEG_PRICER.presentValue(COUPON_LEG, RATES_PROVIDER_ENDED, VOLATILITIES_ENDED);
		assertEquals(computed, CurrencyAmount.zero(EUR));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity()
	  {
		PointSensitivityBuilder point = LEG_PRICER.presentValueSensitivityRates(FLOOR_LEG, RATES_PROVIDER, VOLATILITIES);
		CurrencyParameterSensitivities computed = RATES_PROVIDER.parameterSensitivity(point.build());
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATES_PROVIDER, p => LEG_PRICER.presentValue(FLOOR_LEG, p, VOLATILITIES));
		assertTrue(computed.equalWithTolerance(expected, EPS * NOTIONAL_VALUE_0 * 80d));
	  }

	  public virtual void test_presentValueSensitivity_afterPay()
	  {
		PointSensitivityBuilder point = LEG_PRICER.presentValueSensitivityRates(COUPON_LEG, RATES_PROVIDER_AFTER_PERIOD, VOLATILITIES_AFTER_PERIOD);
		CurrencyParameterSensitivities computed = RATES_PROVIDER_AFTER_PERIOD.parameterSensitivity(point.build());
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATES_PROVIDER_AFTER_PERIOD, p => LEG_PRICER.presentValue(COUPON_LEG, p, VOLATILITIES_AFTER_PERIOD));
		assertTrue(computed.equalWithTolerance(expected, EPS * NOTIONAL_VALUE_0 * 10d));
	  }

	  public virtual void test_presentValueSensitivity_ended()
	  {
		PointSensitivityBuilder computed = LEG_PRICER.presentValueSensitivityRates(CAP_LEG, RATES_PROVIDER_ENDED, VOLATILITIES_ENDED);
		assertEquals(computed, PointSensitivityBuilder.none());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivitySabrParameter()
	  {
		PointSensitivityBuilder computed = LEG_PRICER.presentValueSensitivityModelParamsSabr(FLOOR_LEG, RATES_PROVIDER, VOLATILITIES);
		PointSensitivityBuilder expected = PointSensitivityBuilder.none();
		IList<CmsPeriod> cms = FLOOR_LEG.CmsPeriods;
		int size = cms.Count;
		for (int i = 0; i < size; ++i)
		{
		  expected = expected.combinedWith(PERIOD_PRICER.presentValueSensitivityModelParamsSabr(cms[i], RATES_PROVIDER, VOLATILITIES));
		}
		assertEquals(computed, expected);
	  }

	  public virtual void test_presentValueSensitivitySabrParameter_afterPay()
	  {
		PointSensitivityBuilder computed = LEG_PRICER.presentValueSensitivityModelParamsSabr(FLOOR_LEG, RATES_PROVIDER_AFTER_PERIOD, VOLATILITIES_AFTER_PERIOD);
		PointSensitivityBuilder expected = PointSensitivityBuilder.none();
		IList<CmsPeriod> cms = FLOOR_LEG.CmsPeriods;
		int size = cms.Count;
		for (int i = 0; i < size; ++i)
		{
		  expected = expected.combinedWith(PERIOD_PRICER.presentValueSensitivityModelParamsSabr(cms[i], RATES_PROVIDER_AFTER_PERIOD, VOLATILITIES_AFTER_PERIOD));
		}
		assertEquals(computed, expected);
	  }

	  public virtual void test_presentValueSensitivitySabrParameter_ended()
	  {
		PointSensitivities computed = LEG_PRICER.presentValueSensitivityModelParamsSabr(CAP_LEG, RATES_PROVIDER_ENDED, VOLATILITIES_ENDED).build();
		assertEquals(computed, PointSensitivities.empty());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivityStrike()
	  {
		double computed = LEG_PRICER.presentValueSensitivityStrike(CAP_LEG, RATES_PROVIDER, VOLATILITIES);
		double expected = 0d;
		IList<CmsPeriod> cms = CAP_LEG.CmsPeriods;
		int size = cms.Count;
		for (int i = 0; i < size; ++i)
		{
		  expected += PERIOD_PRICER.presentValueSensitivityStrike(cms[i], RATES_PROVIDER, VOLATILITIES);
		}
		assertEquals(computed, expected, NOTIONAL_VALUE_0 * TOL);
	  }

	  public virtual void test_presentValueSensitivityStrike_afterPay()
	  {
		double computed = LEG_PRICER.presentValueSensitivityStrike(FLOOR_LEG, RATES_PROVIDER_AFTER_PERIOD, VOLATILITIES_AFTER_PERIOD);
		double expected = 0d;
		IList<CmsPeriod> cms = FLOOR_LEG.CmsPeriods;
		int size = cms.Count;
		for (int i = 1; i < size; ++i)
		{
		  expected += PERIOD_PRICER.presentValueSensitivityStrike(cms[i], RATES_PROVIDER_AFTER_PERIOD, VOLATILITIES_AFTER_PERIOD);
		}
		assertEquals(computed, expected, NOTIONAL_VALUE_0 * TOL);
	  }

	  public virtual void test_presentValueSensitivityStrike_ended()
	  {
		double computed = LEG_PRICER.presentValueSensitivityStrike(CAP_LEG, RATES_PROVIDER_ENDED, VOLATILITIES_ENDED);
		assertEquals(computed, 0d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currentCash()
	  {
		CurrencyAmount computed = LEG_PRICER.currentCash(FLOOR_LEG, RATES_PROVIDER, VOLATILITIES);
		assertEquals(computed, CurrencyAmount.zero(EUR));
	  }

	  public virtual void test_currentCash_onPay()
	  {
		CurrencyAmount computed = LEG_PRICER.currentCash(CAP_LEG, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY);
		assertEquals(computed.Amount, NOTIONAL_VALUE_1 * (OBS_INDEX - CAP_VALUE) * 367d / 360d, NOTIONAL_VALUE_0 * TOL);
	  }

	  public virtual void test_currentCash_twoPayments()
	  {
		ResolvedCmsLeg leg = ResolvedCmsLeg.builder().cmsPeriods(FLOOR_LEG.CmsPeriods.get(1), CAP_LEG.CmsPeriods.get(1)).payReceive(RECEIVE).build();
		CurrencyAmount computed = LEG_PRICER.currentCash(leg, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY);
		assertEquals(computed.Amount, NOTIONAL_VALUE_1 * (OBS_INDEX - CAP_VALUE + FLOOR_VALUE_1 - OBS_INDEX) * 367d / 360d, NOTIONAL_VALUE_0 * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_explainPresentValue()
	  {
		ExplainMap explain = LEG_PRICER.explainPresentValue(CAP_LEG, RATES_PROVIDER, VOLATILITIES);
		assertEquals(explain.get(ExplainKey.ENTRY_TYPE).get(), "CmsLeg");
		assertEquals(explain.get(ExplainKey.PAY_RECEIVE).get().ToString(), "Receive");
		assertEquals(explain.get(ExplainKey.PAYMENT_CURRENCY).get().Code, "EUR");
		assertEquals(explain.get(ExplainKey.START_DATE).get(), LocalDate.of(2015, 10, 21));
		assertEquals(explain.get(ExplainKey.END_DATE).get(), LocalDate.of(2020, 10, 21));
		assertEquals(explain.get(ExplainKey.INDEX).get().ToString(), "EUR-EURIBOR-1100-5Y");
		assertEquals(explain.get(ExplainKey.PRESENT_VALUE).get().Amount, 39728.51321029542);

		IList<ExplainMap> paymentPeriods = explain.get(ExplainKey.PAYMENT_PERIODS).get();
		assertEquals(paymentPeriods.Count, 5);
		//Test First Period
		ExplainMap cmsPeriod0 = paymentPeriods[0];
		assertEquals(cmsPeriod0.get(ExplainKey.ENTRY_TYPE).get(), "CmsCapletPeriod");
		assertEquals(cmsPeriod0.get(ExplainKey.STRIKE_VALUE).Value, 0.0125d);
		assertEquals(cmsPeriod0.get(ExplainKey.NOTIONAL).get().Amount, 1000000d);
		assertEquals(cmsPeriod0.get(ExplainKey.PAYMENT_DATE).get(), LocalDate.of(2016, 10, 21));
		assertEquals(cmsPeriod0.get(ExplainKey.DISCOUNT_FACTOR).Value, 0.9820085531995826d);
		assertEquals(cmsPeriod0.get(ExplainKey.START_DATE).get(), LocalDate.of(2015, 10, 21));
		assertEquals(cmsPeriod0.get(ExplainKey.END_DATE).get(), LocalDate.of(2016, 10, 21));
		assertEquals(cmsPeriod0.get(ExplainKey.FIXING_DATE).get(), LocalDate.of(2015, 10, 19));
		assertEquals(cmsPeriod0.get(ExplainKey.ACCRUAL_YEAR_FRACTION).Value, 1.0166666666666666d);
		double forwardSwapRate = PRICER_SWAP.parRate(CAP_LEG.CmsPeriods.get(0).UnderlyingSwap, RATES_PROVIDER);
		assertEquals(cmsPeriod0.get(ExplainKey.FORWARD_RATE).Value, forwardSwapRate);
		CurrencyAmount pv = PERIOD_PRICER.presentValue(CAP_LEG.CmsPeriods.get(0), RATES_PROVIDER, VOLATILITIES);
		assertEquals(cmsPeriod0.get(ExplainKey.PRESENT_VALUE).get(), pv);
	  }

	}

}