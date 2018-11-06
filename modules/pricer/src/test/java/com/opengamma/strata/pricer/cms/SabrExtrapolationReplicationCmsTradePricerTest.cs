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
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using DiscountingSwapLegPricer = com.opengamma.strata.pricer.swap.DiscountingSwapLegPricer;
	using SabrParametersSwaptionVolatilities = com.opengamma.strata.pricer.swaption.SabrParametersSwaptionVolatilities;
	using SwaptionSabrRateVolatilityDataSet = com.opengamma.strata.pricer.swaption.SwaptionSabrRateVolatilityDataSet;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using CmsLeg = com.opengamma.strata.product.cms.CmsLeg;
	using ResolvedCms = com.opengamma.strata.product.cms.ResolvedCms;
	using ResolvedCmsLeg = com.opengamma.strata.product.cms.ResolvedCmsLeg;
	using ResolvedCmsTrade = com.opengamma.strata.product.cms.ResolvedCmsTrade;
	using FixedRateCalculation = com.opengamma.strata.product.swap.FixedRateCalculation;
	using NotionalSchedule = com.opengamma.strata.product.swap.NotionalSchedule;
	using PaymentSchedule = com.opengamma.strata.product.swap.PaymentSchedule;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;
	using SwapIndex = com.opengamma.strata.product.swap.SwapIndex;
	using SwapIndices = com.opengamma.strata.product.swap.SwapIndices;

	/// <summary>
	/// Test <seealso cref="SabrExtrapolationReplicationCmsTradePricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrExtrapolationReplicationCmsTradePricerTest
	public class SabrExtrapolationReplicationCmsTradePricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  // trades
	  private static readonly LocalDate VALUATION = LocalDate.of(2015, 8, 18);
	  private static readonly SwapIndex INDEX = SwapIndices.EUR_EURIBOR_1100_5Y;
	  private static readonly LocalDate START = LocalDate.of(2015, 10, 21);
	  private static readonly LocalDate END = LocalDate.of(2020, 10, 21);
	  private static readonly Frequency FREQUENCY = Frequency.P12M;
	  private static readonly BusinessDayAdjustment BUSS_ADJ_EUR = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, EUTA);
	  private static readonly PeriodicSchedule SCHEDULE_EUR = PeriodicSchedule.of(START, END, FREQUENCY, BUSS_ADJ_EUR, StubConvention.NONE, RollConventions.NONE);
	  private const double NOTIONAL_VALUE = 1.0e6;
	  private static readonly ValueSchedule NOTIONAL = ValueSchedule.of(NOTIONAL_VALUE);
	  private static readonly ValueSchedule CAP = ValueSchedule.of(0.0125);
	  private static readonly ResolvedCmsLeg CMS_LEG = CmsLeg.builder().index(INDEX).notional(NOTIONAL).payReceive(RECEIVE).paymentSchedule(SCHEDULE_EUR).capSchedule(CAP).build().resolve(REF_DATA);
	  private static readonly ResolvedSwapLeg PAY_LEG = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(SCHEDULE_EUR).calculation(FixedRateCalculation.of(0.01, ACT_360)).paymentSchedule(PaymentSchedule.builder().paymentFrequency(FREQUENCY).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.of(CurrencyAmount.of(EUR, NOTIONAL_VALUE))).build().resolve(REF_DATA);
	  private static readonly ResolvedCms CMS_TWO_LEGS = ResolvedCms.of(CMS_LEG, PAY_LEG);
	  private static readonly ResolvedCms CMS_ONE_LEG = ResolvedCms.of(CMS_LEG);
	  private static readonly Payment PREMIUM = Payment.of(CurrencyAmount.of(EUR, -0.03 * NOTIONAL_VALUE), VALUATION);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.builder().tradeDate(VALUATION).build();
	  private static readonly ResolvedCmsTrade CMS_TRADE = ResolvedCmsTrade.builder().product(CMS_TWO_LEGS).info(TRADE_INFO).build();
	  private static readonly ResolvedCmsTrade CMS_TRADE_PREMIUM = ResolvedCmsTrade.builder().product(CMS_ONE_LEG).premium(PREMIUM).build();
	  // providers
	  private static readonly ImmutableRatesProvider RATES_PROVIDER = SwaptionSabrRateVolatilityDataSet.getRatesProviderEur(VALUATION);
	  private static readonly SabrParametersSwaptionVolatilities VOLATILITIES = SwaptionSabrRateVolatilityDataSet.getVolatilitiesEur(VALUATION, true);
	  // providers - valuation on payment date
	  private static readonly LocalDate FIXING = LocalDate.of(2016, 10, 19); // fixing for the second period.
	  private const double OBS_INDEX = 0.013;
	  private static readonly LocalDateDoubleTimeSeries TIME_SERIES = LocalDateDoubleTimeSeries.of(FIXING, OBS_INDEX);
	  private static readonly LocalDate PAYMENT = LocalDate.of(2017, 10, 23); // payment date of the second payment
	  private static readonly ImmutableRatesProvider RATES_PROVIDER_ON_PAY = SwaptionSabrRateVolatilityDataSet.getRatesProviderEur(PAYMENT, TIME_SERIES);
	  private static readonly SabrParametersSwaptionVolatilities VOLATILITIES_ON_PAY = SwaptionSabrRateVolatilityDataSet.getVolatilitiesEur(PAYMENT, true);
	  // pricers
	  private const double CUT_OFF_STRIKE = 0.10;
	  private const double MU = 2.50;
	  private static readonly SabrExtrapolationReplicationCmsPeriodPricer PERIOD_PRICER = SabrExtrapolationReplicationCmsPeriodPricer.of(CUT_OFF_STRIKE, MU);
	  private static readonly SabrExtrapolationReplicationCmsLegPricer CMS_LEG_PRICER = new SabrExtrapolationReplicationCmsLegPricer(PERIOD_PRICER);
	  private static readonly SabrExtrapolationReplicationCmsProductPricer PRODUCT_PRICER = new SabrExtrapolationReplicationCmsProductPricer(CMS_LEG_PRICER, DiscountingSwapLegPricer.DEFAULT);
	  private static readonly DiscountingPaymentPricer PREMIUM_PRICER = DiscountingPaymentPricer.DEFAULT;
	  private static readonly SabrExtrapolationReplicationCmsTradePricer TRADE_PRICER = new SabrExtrapolationReplicationCmsTradePricer(PRODUCT_PRICER, PREMIUM_PRICER);
	  private const double TOL = 1.0e-13;

	  public virtual void test_presentValue()
	  {
		MultiCurrencyAmount pv1 = TRADE_PRICER.presentValue(CMS_TRADE_PREMIUM, RATES_PROVIDER, VOLATILITIES);
		MultiCurrencyAmount pv2 = TRADE_PRICER.presentValue(CMS_TRADE, RATES_PROVIDER, VOLATILITIES);
		MultiCurrencyAmount pvProd1 = PRODUCT_PRICER.presentValue(CMS_ONE_LEG, RATES_PROVIDER, VOLATILITIES);
		MultiCurrencyAmount pvProd2 = PRODUCT_PRICER.presentValue(CMS_TWO_LEGS, RATES_PROVIDER, VOLATILITIES);
		CurrencyAmount pvPrem = PREMIUM_PRICER.presentValue(PREMIUM, RATES_PROVIDER);
		assertEquals(pv1, pvProd1.plus(pvPrem));
		assertEquals(pv2, pvProd2);
	  }

	  public virtual void test_presentValueSensitivity()
	  {
		PointSensitivities pt1 = TRADE_PRICER.presentValueSensitivityRates(CMS_TRADE_PREMIUM, RATES_PROVIDER, VOLATILITIES);
		PointSensitivities pt2 = TRADE_PRICER.presentValueSensitivityRates(CMS_TRADE, RATES_PROVIDER, VOLATILITIES);
		PointSensitivityBuilder ptProd1 = PRODUCT_PRICER.presentValueSensitivityRates(CMS_ONE_LEG, RATES_PROVIDER, VOLATILITIES);
		PointSensitivityBuilder ptProd2 = PRODUCT_PRICER.presentValueSensitivityRates(CMS_TWO_LEGS, RATES_PROVIDER, VOLATILITIES);
		PointSensitivityBuilder ptPrem = PREMIUM_PRICER.presentValueSensitivity(PREMIUM, RATES_PROVIDER);
		assertEquals(pt1, ptProd1.combinedWith(ptPrem).build());
		assertEquals(pt2, ptProd2.build());
	  }

	  public virtual void test_presentValueSensitivitySabrParameter()
	  {
		PointSensitivities pt1 = TRADE_PRICER.presentValueSensitivityModelParamsSabr(CMS_TRADE_PREMIUM, RATES_PROVIDER, VOLATILITIES);
		PointSensitivities pt2 = TRADE_PRICER.presentValueSensitivityModelParamsSabr(CMS_TRADE, RATES_PROVIDER, VOLATILITIES);
		PointSensitivities ptProd1 = PRODUCT_PRICER.presentValueSensitivityModelParamsSabr(CMS_ONE_LEG, RATES_PROVIDER, VOLATILITIES).build();
		PointSensitivities ptProd2 = PRODUCT_PRICER.presentValueSensitivityModelParamsSabr(CMS_TWO_LEGS, RATES_PROVIDER, VOLATILITIES).build();
		assertEquals(pt1, ptProd1);
		assertEquals(pt2, ptProd2);
	  }

	  public virtual void test_presentValueSensitivityStrike()
	  {
		double sensi1 = TRADE_PRICER.presentValueSensitivityStrike(CMS_TRADE_PREMIUM, RATES_PROVIDER, VOLATILITIES);
		double sensi2 = TRADE_PRICER.presentValueSensitivityStrike(CMS_TRADE, RATES_PROVIDER, VOLATILITIES);
		double sensiProd1 = PRODUCT_PRICER.presentValueSensitivityStrike(CMS_ONE_LEG, RATES_PROVIDER, VOLATILITIES);
		double sensiProd2 = PRODUCT_PRICER.presentValueSensitivityStrike(CMS_TWO_LEGS, RATES_PROVIDER, VOLATILITIES);
		assertEquals(sensi1, sensiProd1);
		assertEquals(sensi2, sensiProd2);
	  }

	  public virtual void test_currencyExposure()
	  {
		MultiCurrencyAmount computed1 = TRADE_PRICER.currencyExposure(CMS_TRADE_PREMIUM, RATES_PROVIDER, VOLATILITIES);
		MultiCurrencyAmount computed2 = TRADE_PRICER.currencyExposure(CMS_TRADE, RATES_PROVIDER, VOLATILITIES);
		MultiCurrencyAmount pv1 = TRADE_PRICER.presentValue(CMS_TRADE_PREMIUM, RATES_PROVIDER, VOLATILITIES);
		PointSensitivities pt1 = TRADE_PRICER.presentValueSensitivityRates(CMS_TRADE_PREMIUM, RATES_PROVIDER, VOLATILITIES);
		MultiCurrencyAmount expected1 = RATES_PROVIDER.currencyExposure(pt1).plus(pv1);
		MultiCurrencyAmount pv2 = TRADE_PRICER.presentValue(CMS_TRADE, RATES_PROVIDER, VOLATILITIES);
		PointSensitivities pt2 = TRADE_PRICER.presentValueSensitivityRates(CMS_TRADE, RATES_PROVIDER, VOLATILITIES);
		MultiCurrencyAmount expected2 = RATES_PROVIDER.currencyExposure(pt2).plus(pv2);
		assertEquals(computed1.getAmount(EUR).Amount, expected1.getAmount(EUR).Amount, NOTIONAL_VALUE * TOL);
		assertEquals(computed2.getAmount(EUR).Amount, expected2.getAmount(EUR).Amount, NOTIONAL_VALUE * TOL);
	  }

	  public virtual void test_currentCash()
	  {
		MultiCurrencyAmount cc1 = TRADE_PRICER.currentCash(CMS_TRADE_PREMIUM, RATES_PROVIDER, VOLATILITIES);
		MultiCurrencyAmount cc2 = TRADE_PRICER.currentCash(CMS_TRADE, RATES_PROVIDER, VOLATILITIES);
		assertEquals(cc1, MultiCurrencyAmount.of(PREMIUM.Value));
		assertEquals(cc2, MultiCurrencyAmount.of(CurrencyAmount.zero(EUR)));
	  }

	  public virtual void test_currentCash_onPay()
	  {
		MultiCurrencyAmount cc1 = TRADE_PRICER.currentCash(CMS_TRADE_PREMIUM, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY);
		MultiCurrencyAmount cc2 = TRADE_PRICER.currentCash(CMS_TRADE, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY);
		MultiCurrencyAmount ccProd1 = PRODUCT_PRICER.currentCash(CMS_ONE_LEG, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY);
		MultiCurrencyAmount ccProd2 = PRODUCT_PRICER.currentCash(CMS_TWO_LEGS, RATES_PROVIDER_ON_PAY, VOLATILITIES_ON_PAY);
		assertEquals(cc1, ccProd1);
		assertEquals(cc2, ccProd2);
	  }

	}

}