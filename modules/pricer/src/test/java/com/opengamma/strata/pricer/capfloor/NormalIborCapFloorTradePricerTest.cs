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
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_6M;
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
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using ResolvedIborCapFloor = com.opengamma.strata.product.capfloor.ResolvedIborCapFloor;
	using ResolvedIborCapFloorLeg = com.opengamma.strata.product.capfloor.ResolvedIborCapFloorLeg;
	using ResolvedIborCapFloorTrade = com.opengamma.strata.product.capfloor.ResolvedIborCapFloorTrade;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;

	/// <summary>
	/// Test <seealso cref="NormalIborCapFloorTradePricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NormalIborCapFloorTradePricerTest
	public class NormalIborCapFloorTradePricerTest
	{

	  private const double NOTIONAL_VALUE = 1.0e6;
	  private static readonly ValueSchedule NOTIONAL = ValueSchedule.of(NOTIONAL_VALUE);
	  private static readonly LocalDate START = LocalDate.of(2015, 10, 21);
	  private static readonly LocalDate END = LocalDate.of(2020, 10, 21);
	  private const double STRIKE_VALUE = 0.0105;
	  private static readonly ValueSchedule STRIKE = ValueSchedule.of(STRIKE_VALUE);
	  private static readonly ResolvedIborCapFloorLeg CAP_LEG = IborCapFloorDataSet.createCapFloorLeg(EUR_EURIBOR_6M, START, END, STRIKE, NOTIONAL, CALL, RECEIVE);
	  private static readonly ResolvedSwapLeg PAY_LEG = IborCapFloorDataSet.createFixedPayLeg(EUR_EURIBOR_6M, START, END, 0.0395, NOTIONAL_VALUE, PAY);
	  private static readonly ResolvedIborCapFloor CAP_TWO_LEGS = ResolvedIborCapFloor.of(CAP_LEG, PAY_LEG);
	  private static readonly ResolvedIborCapFloor CAP_ONE_LEG = ResolvedIborCapFloor.of(CAP_LEG);

	  // valuation before start
	  private static readonly ZonedDateTime VALUATION = dateUtc(2015, 8, 20);
	  private static readonly ImmutableRatesProvider RATES = IborCapletFloorletDataSet.createRatesProvider(VALUATION.toLocalDate());
	  private static readonly NormalIborCapletFloorletExpiryStrikeVolatilities VOLS = IborCapletFloorletDataSet.createNormalVolatilities(VALUATION, EUR_EURIBOR_6M);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.builder().tradeDate(VALUATION.toLocalDate()).build();
	  private static readonly Payment PREMIUM = Payment.of(EUR, -NOTIONAL_VALUE * 0.19, VALUATION.toLocalDate());
	  private static readonly ResolvedIborCapFloorTrade TRADE_PAYLEG = ResolvedIborCapFloorTrade.builder().product(CAP_TWO_LEGS).info(TRADE_INFO).build();
	  private static readonly ResolvedIborCapFloorTrade TRADE_PREMIUM = ResolvedIborCapFloorTrade.builder().product(CAP_ONE_LEG).premium(PREMIUM).info(TradeInfo.empty()).build();

	  //   valuation at payment of 1st period
	  private const double OBS_INDEX_1 = 0.013;
	  private const double OBS_INDEX_2 = 0.0135;
	  private static readonly LocalDateDoubleTimeSeries TIME_SERIES = LocalDateDoubleTimeSeries.builder().put(date(2015, 10, 19), OBS_INDEX_1).put(date(2016, 4, 19), OBS_INDEX_2).build();
	  private static readonly ZonedDateTime VALUATION_PAY = dateUtc(2016, 4, 21);
	  private static readonly ImmutableRatesProvider RATES_PAY = IborCapletFloorletDataSet.createRatesProvider(VALUATION_PAY.toLocalDate(), EUR_EURIBOR_6M, TIME_SERIES);
	  private static readonly NormalIborCapletFloorletExpiryStrikeVolatilities VOLS_PAY = IborCapletFloorletDataSet.createNormalVolatilities(VALUATION_PAY, EUR_EURIBOR_6M);

	  private const double TOL = 1.0e-13;
	  private static readonly NormalIborCapFloorTradePricer PRICER = NormalIborCapFloorTradePricer.DEFAULT;
	  private static readonly NormalIborCapFloorProductPricer PRICER_PRODUCT = NormalIborCapFloorProductPricer.DEFAULT;
	  private static readonly DiscountingPaymentPricer PRICER_PREMIUM = DiscountingPaymentPricer.DEFAULT;

	  public virtual void test_presentValue()
	  {
		MultiCurrencyAmount computedWithPayLeg = PRICER.presentValue(TRADE_PAYLEG, RATES, VOLS);
		MultiCurrencyAmount computedWithPremium = PRICER.presentValue(TRADE_PREMIUM, RATES, VOLS);
		MultiCurrencyAmount pvOneLeg = PRICER_PRODUCT.presentValue(CAP_ONE_LEG, RATES, VOLS);
		MultiCurrencyAmount pvTwoLegs = PRICER_PRODUCT.presentValue(CAP_TWO_LEGS, RATES, VOLS);
		CurrencyAmount pvPrem = PRICER_PREMIUM.presentValue(PREMIUM, RATES);
		assertEquals(computedWithPayLeg, pvTwoLegs);
		assertEquals(computedWithPremium, pvOneLeg.plus(pvPrem));
	  }

	  public virtual void test_presentValueSensitivity()
	  {
		PointSensitivities computedWithPayLeg = PRICER.presentValueSensitivityRates(TRADE_PAYLEG, RATES, VOLS);
		PointSensitivities computedWithPremium = PRICER.presentValueSensitivityRates(TRADE_PREMIUM, RATES, VOLS);
		PointSensitivities pvOneLeg = PRICER_PRODUCT.presentValueSensitivityRates(CAP_ONE_LEG, RATES, VOLS).build();
		PointSensitivities pvTwoLegs = PRICER_PRODUCT.presentValueSensitivityRates(CAP_TWO_LEGS, RATES, VOLS).build();
		PointSensitivities pvPrem = PRICER_PREMIUM.presentValueSensitivity(PREMIUM, RATES).build();
		assertEquals(computedWithPayLeg, pvTwoLegs);
		assertEquals(computedWithPremium, pvOneLeg.combinedWith(pvPrem));
	  }

	  public virtual void test_currencyExposure()
	  {
		MultiCurrencyAmount computedWithPayLeg = PRICER.currencyExposure(TRADE_PAYLEG, RATES, VOLS);
		MultiCurrencyAmount computedWithPremium = PRICER.currencyExposure(TRADE_PREMIUM, RATES, VOLS);
		MultiCurrencyAmount pvWithPayLeg = PRICER.presentValue(TRADE_PAYLEG, RATES, VOLS);
		MultiCurrencyAmount pvWithPremium = PRICER.presentValue(TRADE_PREMIUM, RATES, VOLS);
		PointSensitivities pointWithPayLeg = PRICER.presentValueSensitivityRates(TRADE_PAYLEG, RATES, VOLS);
		PointSensitivities pointWithPremium = PRICER.presentValueSensitivityRates(TRADE_PREMIUM, RATES, VOLS);
		MultiCurrencyAmount expectedWithPayLeg = RATES.currencyExposure(pointWithPayLeg).plus(pvWithPayLeg);
		MultiCurrencyAmount expectedWithPremium = RATES.currencyExposure(pointWithPremium).plus(pvWithPremium);
		assertEquals(computedWithPayLeg.getAmount(EUR).Amount, expectedWithPayLeg.getAmount(EUR).Amount, NOTIONAL_VALUE * TOL);
		assertEquals(computedWithPremium.getAmount(EUR).Amount, expectedWithPremium.getAmount(EUR).Amount, NOTIONAL_VALUE * TOL);
	  }

	  public virtual void test_currentCash()
	  {
		MultiCurrencyAmount computedWithPayLeg = PRICER.currentCash(TRADE_PAYLEG, RATES, VOLS);
		MultiCurrencyAmount computedWithPremium = PRICER.currentCash(TRADE_PREMIUM, RATES, VOLS);
		assertEquals(computedWithPayLeg, MultiCurrencyAmount.of(CurrencyAmount.zero(EUR)));
		assertEquals(computedWithPremium, MultiCurrencyAmount.of(PREMIUM.Value));
	  }

	  public virtual void test_currentCash_onPay()
	  {
		MultiCurrencyAmount computedWithPayLeg = PRICER.currentCash(TRADE_PAYLEG, RATES_PAY, VOLS_PAY);
		MultiCurrencyAmount computedWithPremium = PRICER.currentCash(TRADE_PREMIUM, RATES_PAY, VOLS_PAY);
		MultiCurrencyAmount expectedWithPayLeg = PRICER_PRODUCT.currentCash(CAP_TWO_LEGS, RATES_PAY, VOLS_PAY);
		MultiCurrencyAmount expectedWithPremium = PRICER_PRODUCT.currentCash(CAP_ONE_LEG, RATES_PAY, VOLS_PAY);
		assertEquals(computedWithPayLeg, expectedWithPayLeg);
		assertEquals(computedWithPremium, expectedWithPremium);
	  }

	}

}