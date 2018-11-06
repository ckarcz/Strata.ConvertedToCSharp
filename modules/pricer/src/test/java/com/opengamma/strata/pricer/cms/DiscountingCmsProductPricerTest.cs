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
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using DiscountingCmsPeriodPricer = com.opengamma.strata.pricer.impl.cms.DiscountingCmsPeriodPricer;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using DiscountingSwapLegPricer = com.opengamma.strata.pricer.swap.DiscountingSwapLegPricer;
	using SwaptionSabrRateVolatilityDataSet = com.opengamma.strata.pricer.swaption.SwaptionSabrRateVolatilityDataSet;
	using CmsLeg = com.opengamma.strata.product.cms.CmsLeg;
	using ResolvedCms = com.opengamma.strata.product.cms.ResolvedCms;
	using ResolvedCmsLeg = com.opengamma.strata.product.cms.ResolvedCmsLeg;
	using FixedRateCalculation = com.opengamma.strata.product.swap.FixedRateCalculation;
	using NotionalSchedule = com.opengamma.strata.product.swap.NotionalSchedule;
	using PaymentSchedule = com.opengamma.strata.product.swap.PaymentSchedule;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;
	using SwapIndex = com.opengamma.strata.product.swap.SwapIndex;
	using SwapIndices = com.opengamma.strata.product.swap.SwapIndices;

	/// <summary>
	/// Test <seealso cref="SabrExtrapolationReplicationCmsProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingCmsProductPricerTest
	public class DiscountingCmsProductPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  // CMS products
	  private static readonly SwapIndex INDEX = SwapIndices.EUR_EURIBOR_1100_5Y;
	  private static readonly LocalDate START = LocalDate.of(2015, 10, 21);
	  private static readonly LocalDate END = LocalDate.of(2020, 10, 21);
	  private static readonly Frequency FREQUENCY = Frequency.P12M;
	  private static readonly BusinessDayAdjustment BUSS_ADJ_EUR = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, EUTA);
	  private static readonly PeriodicSchedule SCHEDULE_EUR = PeriodicSchedule.of(START, END, FREQUENCY, BUSS_ADJ_EUR, StubConvention.NONE, RollConventions.NONE);
	  private const double NOTIONAL_VALUE = 100_000_000;
	  private static readonly ValueSchedule NOTIONAL = ValueSchedule.of(NOTIONAL_VALUE);
	  private static readonly ResolvedCmsLeg CMS_LEG = CmsLeg.builder().index(INDEX).notional(NOTIONAL).payReceive(RECEIVE).paymentSchedule(SCHEDULE_EUR).build().resolve(REF_DATA);
	  private static readonly ResolvedSwapLeg PAY_LEG = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(SCHEDULE_EUR).calculation(FixedRateCalculation.of(0.0035, ACT_360)).paymentSchedule(PaymentSchedule.builder().paymentFrequency(FREQUENCY).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.of(CurrencyAmount.of(EUR, NOTIONAL_VALUE))).build().resolve(REF_DATA);
	  private static readonly ResolvedCms CMS_TWO_LEGS = ResolvedCms.of(CMS_LEG, PAY_LEG);
	  private static readonly ResolvedCms CMS_ONE_LEG = ResolvedCms.of(CMS_LEG);
	  // providers
	  private static readonly LocalDate VALUATION = LocalDate.of(2015, 8, 18);
	  private static readonly ImmutableRatesProvider RATES_PROVIDER = SwaptionSabrRateVolatilityDataSet.getRatesProviderEur(VALUATION);
	  // providers - valuation on payment date
	  private static readonly LocalDate FIXING = LocalDate.of(2016, 10, 19); // fixing for the second period.
	  private const double OBS_INDEX = 0.013;
	  private static readonly LocalDateDoubleTimeSeries TIME_SERIES = LocalDateDoubleTimeSeries.of(FIXING, OBS_INDEX);
	  private static readonly LocalDate PAYMENT = LocalDate.of(2017, 10, 23); // payment date of the second payment
	  private static readonly ImmutableRatesProvider RATES_PROVIDER_ON_PAY = SwaptionSabrRateVolatilityDataSet.getRatesProviderEur(PAYMENT, TIME_SERIES);
	  // pricers
	  private static readonly DiscountingCmsPeriodPricer PERIOD_PRICER = DiscountingCmsPeriodPricer.DEFAULT;
	  private static readonly DiscountingCmsLegPricer CMS_LEG_PRICER = new DiscountingCmsLegPricer(PERIOD_PRICER);
	  private static readonly DiscountingSwapLegPricer SWAP_LEG_PRICER = DiscountingSwapLegPricer.DEFAULT;
	  private static readonly DiscountingCmsProductPricer PRODUCT_PRICER = DiscountingCmsProductPricer.DEFAULT;
	  private const double TOL = 1.0e-13;

	  public virtual void test_presentValue()
	  {
		MultiCurrencyAmount pv1 = PRODUCT_PRICER.presentValue(CMS_ONE_LEG, RATES_PROVIDER);
		MultiCurrencyAmount pv2 = PRODUCT_PRICER.presentValue(CMS_TWO_LEGS, RATES_PROVIDER);
		CurrencyAmount pvCms = CMS_LEG_PRICER.presentValue(CMS_LEG, RATES_PROVIDER);
		CurrencyAmount pvPay = SWAP_LEG_PRICER.presentValue(PAY_LEG, RATES_PROVIDER);
		assertEquals(pv1, MultiCurrencyAmount.of(pvCms));
		assertEquals(pv2, MultiCurrencyAmount.of(pvCms).plus(pvPay));
	  }

	  public virtual void test_presentValueSensitivity()
	  {
		PointSensitivityBuilder pt1 = PRODUCT_PRICER.presentValueSensitivity(CMS_ONE_LEG, RATES_PROVIDER);
		PointSensitivityBuilder pt2 = PRODUCT_PRICER.presentValueSensitivity(CMS_TWO_LEGS, RATES_PROVIDER);
		PointSensitivityBuilder ptCms = CMS_LEG_PRICER.presentValueSensitivity(CMS_LEG, RATES_PROVIDER);
		PointSensitivityBuilder ptPay = SWAP_LEG_PRICER.presentValueSensitivity(PAY_LEG, RATES_PROVIDER);
		assertEquals(pt1, ptCms);
		assertEquals(pt2, ptCms.combinedWith(ptPay));
	  }

	  public virtual void test_currencyExposure()
	  {
		MultiCurrencyAmount computed1 = PRODUCT_PRICER.currencyExposure(CMS_ONE_LEG, RATES_PROVIDER);
		MultiCurrencyAmount computed2 = PRODUCT_PRICER.currencyExposure(CMS_TWO_LEGS, RATES_PROVIDER);
		MultiCurrencyAmount pv1 = PRODUCT_PRICER.presentValue(CMS_ONE_LEG, RATES_PROVIDER);
		PointSensitivityBuilder pt1 = PRODUCT_PRICER.presentValueSensitivity(CMS_ONE_LEG, RATES_PROVIDER);
		MultiCurrencyAmount expected1 = RATES_PROVIDER.currencyExposure(pt1.build()).plus(pv1);
		MultiCurrencyAmount pv2 = PRODUCT_PRICER.presentValue(CMS_TWO_LEGS, RATES_PROVIDER);
		PointSensitivityBuilder pt2 = PRODUCT_PRICER.presentValueSensitivity(CMS_TWO_LEGS, RATES_PROVIDER);
		MultiCurrencyAmount expected2 = RATES_PROVIDER.currencyExposure(pt2.build()).plus(pv2);
		assertEquals(computed1.getAmount(EUR).Amount, expected1.getAmount(EUR).Amount, NOTIONAL_VALUE * TOL);
		assertEquals(computed2.getAmount(EUR).Amount, expected2.getAmount(EUR).Amount, NOTIONAL_VALUE * TOL);
	  }

	  public virtual void test_currentCash()
	  {
		MultiCurrencyAmount cc1 = PRODUCT_PRICER.currentCash(CMS_ONE_LEG, RATES_PROVIDER);
		MultiCurrencyAmount cc2 = PRODUCT_PRICER.currentCash(CMS_TWO_LEGS, RATES_PROVIDER);
		assertEquals(cc1, MultiCurrencyAmount.of(CurrencyAmount.zero(EUR)));
		assertEquals(cc2, MultiCurrencyAmount.of(CurrencyAmount.zero(EUR)));
	  }

	  public virtual void test_currentCash_onPay()
	  {
		MultiCurrencyAmount cc1 = PRODUCT_PRICER.currentCash(CMS_ONE_LEG, RATES_PROVIDER_ON_PAY);
		MultiCurrencyAmount cc2 = PRODUCT_PRICER.currentCash(CMS_TWO_LEGS, RATES_PROVIDER_ON_PAY);
		CurrencyAmount ccCms = CMS_LEG_PRICER.currentCash(CMS_LEG, RATES_PROVIDER_ON_PAY);
		CurrencyAmount ccPay = SWAP_LEG_PRICER.currentCash(PAY_LEG, RATES_PROVIDER_ON_PAY);
		assertEquals(cc1, MultiCurrencyAmount.of(ccCms));
		assertEquals(cc2, MultiCurrencyAmount.of(ccCms).plus(ccPay));
	  }

	}

}