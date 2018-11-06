/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ICMA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.US_CPI_U;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.CompoundedRateType.CONTINUOUS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.CompoundedRateType.PERIODIC;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.bond.CapitalIndexedBondYieldConvention.GB_IL_FLOAT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.bond.CapitalIndexedBondYieldConvention.US_IL_REAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.PriceIndexCalculationMethod.INTERPOLATED;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


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
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using LegalEntityId = com.opengamma.strata.product.LegalEntityId;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using CapitalIndexedBond = com.opengamma.strata.product.bond.CapitalIndexedBond;
	using CapitalIndexedBondPaymentPeriod = com.opengamma.strata.product.bond.CapitalIndexedBondPaymentPeriod;
	using CapitalIndexedBondPosition = com.opengamma.strata.product.bond.CapitalIndexedBondPosition;
	using CapitalIndexedBondTrade = com.opengamma.strata.product.bond.CapitalIndexedBondTrade;
	using KnownAmountBondPaymentPeriod = com.opengamma.strata.product.bond.KnownAmountBondPaymentPeriod;
	using ResolvedCapitalIndexedBond = com.opengamma.strata.product.bond.ResolvedCapitalIndexedBond;
	using ResolvedCapitalIndexedBondTrade = com.opengamma.strata.product.bond.ResolvedCapitalIndexedBondTrade;
	using InflationRateCalculation = com.opengamma.strata.product.swap.InflationRateCalculation;

	/// <summary>
	/// Test <seealso cref="DiscountingCapitalIndexedBondTradePricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingCapitalIndexedBondTradePricerTest
	public class DiscountingCapitalIndexedBondTradePricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  // detachment date (for nonzero ex-coupon days) < valuation date < payment date
	  private static readonly LocalDate VALUATION = LocalDate.of(2014, 7, 13);
	  private static readonly LocalDate PAYMENT = LocalDate.of(2014, 7, 15);
	  private static readonly LocalDateDoubleTimeSeries TS = CapitalIndexedBondCurveDataSet.getTimeSeries(VALUATION);
	  private static readonly ImmutableRatesProvider RATES_PROVIDER = CapitalIndexedBondCurveDataSet.getRatesProvider(VALUATION, TS);
	  private static readonly ImmutableRatesProvider RATES_PROVIDER_ON_PAY = CapitalIndexedBondCurveDataSet.getRatesProvider(PAYMENT, TS);
	  private static readonly LegalEntityDiscountingProvider ISSUER_RATES_PROVIDER = CapitalIndexedBondCurveDataSet.getLegalEntityDiscountingProvider(VALUATION);
	  private static readonly IssuerCurveDiscountFactors ISSUER_DISCOUNT_FACTORS = CapitalIndexedBondCurveDataSet.getIssuerCurveDiscountFactors(VALUATION);
	  private const double Z_SPREAD = 0.015;
	  private const int PERIOD_PER_YEAR = 4;

	  private const double NOTIONAL = 10_000_000d;
	  private const double START_INDEX = 198.47742;
	  private const double REAL_COUPON_VALUE = 0.01;
	  private static readonly ValueSchedule REAL_COUPON = ValueSchedule.of(REAL_COUPON_VALUE);
	  private static readonly InflationRateCalculation RATE_CALC = InflationRateCalculation.builder().gearing(REAL_COUPON).index(US_CPI_U).lag(Period.ofMonths(3)).indexCalculationMethod(INTERPOLATED).firstIndexValue(START_INDEX).build();
	  private static readonly BusinessDayAdjustment EX_COUPON_ADJ = BusinessDayAdjustment.of(BusinessDayConventions.PRECEDING, USNY);
	  private static readonly DaysAdjustment SETTLE_OFFSET = DaysAdjustment.ofBusinessDays(3, USNY);
	  private static readonly LegalEntityId LEGAL_ENTITY = CapitalIndexedBondCurveDataSet.IssuerId;
	  private static readonly LocalDate START = LocalDate.of(2006, 1, 15);
	  private static readonly LocalDate END = LocalDate.of(2016, 1, 15);
	  private static readonly Frequency FREQUENCY = Frequency.P6M;
	  private static readonly BusinessDayAdjustment BUSINESS_ADJUST = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, USNY);
	  private static readonly PeriodicSchedule SCHEDULE = PeriodicSchedule.of(START, END, FREQUENCY, BUSINESS_ADJUST, StubConvention.NONE, RollConventions.NONE);
	  private static readonly SecurityId SECURITY_ID = SecurityId.of("OG-Ticker", "BOND1");
	  private static readonly CapitalIndexedBond PRODUCT = CapitalIndexedBond.builder().securityId(SECURITY_ID).notional(NOTIONAL).currency(USD).dayCount(ACT_ACT_ICMA).rateCalculation(RATE_CALC).legalEntityId(LEGAL_ENTITY).yieldConvention(US_IL_REAL).settlementDateOffset(SETTLE_OFFSET).accrualSchedule(SCHEDULE).build();
	  private static readonly ResolvedCapitalIndexedBond RPRODUCT = PRODUCT.resolve(REF_DATA);
	  private static readonly DaysAdjustment EX_COUPON = DaysAdjustment.ofCalendarDays(-5, EX_COUPON_ADJ);
	  private static readonly CapitalIndexedBond PRODUCT_EX_COUPON = CapitalIndexedBond.builder().securityId(SECURITY_ID).notional(NOTIONAL).currency(USD).dayCount(ACT_ACT_ICMA).rateCalculation(RATE_CALC).legalEntityId(LEGAL_ENTITY).yieldConvention(US_IL_REAL).settlementDateOffset(SETTLE_OFFSET).accrualSchedule(SCHEDULE).exCouponPeriod(EX_COUPON).build();
	  private static readonly ResolvedCapitalIndexedBond RPRODUCT_EX_COUPON = PRODUCT_EX_COUPON.resolve(REF_DATA);
	  private static readonly CapitalIndexedBond PRODUCT_ILF = CapitalIndexedBond.builder().securityId(SECURITY_ID).notional(NOTIONAL).currency(USD).dayCount(ACT_ACT_ICMA).rateCalculation(RATE_CALC).legalEntityId(LEGAL_ENTITY).yieldConvention(GB_IL_FLOAT).settlementDateOffset(SETTLE_OFFSET).accrualSchedule(SCHEDULE).build();
	  private static readonly ResolvedCapitalIndexedBond RPRODUCT_ILF = PRODUCT_ILF.resolve(REF_DATA);

	  private const long QUANTITY = 100L;
	  private static readonly LocalDate SETTLEMENT_BEFORE = VALUATION.minusWeeks(1);
	  private static readonly LocalDate SETTLEMENT_EARLY = VALUATION;
	  private static readonly LocalDate SETTLEMENT_LATE = LocalDate.of(2015, 2, 19);
	  private static readonly LocalDate SETTLEMENT_STANDARD = SETTLE_OFFSET.adjust(VALUATION, REF_DATA);
	  private static readonly TradeInfo TRADE_INFO_SETTLED = TradeInfo.builder().settlementDate(SETTLEMENT_BEFORE).build();
	  private static readonly TradeInfo TRADE_INFO_EARLY = TradeInfo.builder().settlementDate(SETTLEMENT_EARLY).build();
	  private static readonly TradeInfo TRADE_INFO_LATE = TradeInfo.builder().settlementDate(SETTLEMENT_LATE).build();
	  private static readonly TradeInfo TRADE_INFO_STANDARD = TradeInfo.builder().settlementDate(SETTLEMENT_STANDARD).build();
	  private const double TRADE_PRICE = 1.0203;
	  private static readonly ResolvedCapitalIndexedBondTrade TRADE_SETTLED = CapitalIndexedBondTrade.builder().info(TRADE_INFO_SETTLED).product(PRODUCT).quantity(QUANTITY).price(TRADE_PRICE).build().resolve(REF_DATA);
	  private static readonly ResolvedCapitalIndexedBondTrade TRADE_EARLY = CapitalIndexedBondTrade.builder().info(TRADE_INFO_EARLY).product(PRODUCT).quantity(QUANTITY).price(TRADE_PRICE).build().resolve(REF_DATA);
	  private static readonly ResolvedCapitalIndexedBondTrade TRADE_EX_COUPON_EARLY = CapitalIndexedBondTrade.builder().info(TRADE_INFO_EARLY).product(PRODUCT_EX_COUPON).quantity(QUANTITY).price(TRADE_PRICE).build().resolve(REF_DATA);
	  private static readonly ResolvedCapitalIndexedBondTrade TRADE_LATE = CapitalIndexedBondTrade.builder().info(TRADE_INFO_LATE).product(PRODUCT).quantity(QUANTITY).price(TRADE_PRICE).build().resolve(REF_DATA);
	  private static readonly ResolvedCapitalIndexedBondTrade TRADE_STANDARD = CapitalIndexedBondTrade.builder().info(TRADE_INFO_STANDARD).product(PRODUCT).quantity(QUANTITY).price(TRADE_PRICE).build().resolve(REF_DATA);
	  private static readonly ResolvedCapitalIndexedBondTrade TRADE_ILF_STANDARD = CapitalIndexedBondTrade.builder().info(TRADE_INFO_STANDARD).product(PRODUCT_ILF).quantity(QUANTITY).price(TRADE_PRICE).build().resolve(REF_DATA);
	  private static readonly ResolvedCapitalIndexedBondTrade TRADE_EX_COUPON_STANDARD = CapitalIndexedBondTrade.builder().info(TRADE_INFO_STANDARD).product(PRODUCT_EX_COUPON).quantity(QUANTITY).price(TRADE_PRICE).build().resolve(REF_DATA);
	  private static readonly ResolvedCapitalIndexedBondTrade POSITION = CapitalIndexedBondPosition.builder().product(PRODUCT).longQuantity(QUANTITY).build().resolve(REF_DATA);
	  private static readonly CapitalIndexedBondPaymentPeriod SETTLE_PERIOD_LATE = (CapitalIndexedBondPaymentPeriod) TRADE_LATE.Settlement.get().Payment;
	  private static readonly CapitalIndexedBondPaymentPeriod SETTLE_PERIOD_STANDARD = (CapitalIndexedBondPaymentPeriod) TRADE_STANDARD.Settlement.get().Payment;
	  private static readonly KnownAmountBondPaymentPeriod SETTLE_PERIOD_ILF = (KnownAmountBondPaymentPeriod) TRADE_ILF_STANDARD.Settlement.get().Payment;

	  private const double TOL = 1.0e-12;
	  private const double EPS = 1.0e-6;
	  private static readonly DiscountingCapitalIndexedBondTradePricer PRICER = DiscountingCapitalIndexedBondTradePricer.DEFAULT;
	  private static readonly DiscountingCapitalIndexedBondProductPricer PRODUCT_PRICER = DiscountingCapitalIndexedBondProductPricer.DEFAULT;
	  private static readonly DiscountingCapitalIndexedBondPaymentPeriodPricer PERIOD_PRICER = DiscountingCapitalIndexedBondPaymentPeriodPricer.DEFAULT;
	  private static readonly DiscountingPaymentPricer PAYMENT_PRICER = DiscountingPaymentPricer.DEFAULT;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FD_CAL = new RatesFiniteDifferenceSensitivityCalculator(EPS);

	  //-------------------------------------------------------------------------
	  public virtual void test_netAmount_standard()
	  {
		CurrencyAmount computed = PRICER.netAmount(TRADE_STANDARD, RATES_PROVIDER);
		double expected = PERIOD_PRICER.forecastValue(SETTLE_PERIOD_STANDARD, RATES_PROVIDER);
		assertEquals(computed.Amount, expected, QUANTITY * NOTIONAL * TOL);
	  }

	  public virtual void test_netAmount_late()
	  {
		CurrencyAmount computed = PRICER.netAmount(TRADE_LATE, RATES_PROVIDER);
		double expected = PERIOD_PRICER.forecastValue(SETTLE_PERIOD_LATE, RATES_PROVIDER);
		assertEquals(computed.Amount, expected, QUANTITY * NOTIONAL * TOL);
	  }

	  public virtual void test_netAmountfixed()
	  {
		CurrencyAmount computed = PRICER.netAmount(TRADE_ILF_STANDARD, RATES_PROVIDER);
		double expected = PAYMENT_PRICER.forecastValueAmount(SETTLE_PERIOD_ILF.Payment, RATES_PROVIDER);
		assertEquals(computed.Amount, expected, QUANTITY * NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueFromCleanPrice_standard()
	  {
		CurrencyAmount computed = PRICER.presentValueFromCleanPrice(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE);
		assertEquals(computed.Amount, 0d, NOTIONAL * QUANTITY * TOL);
	  }

	  public virtual void test_presentValueFromCleanPrice_early()
	  {
		CurrencyAmount computed = PRICER.presentValueFromCleanPrice(TRADE_EARLY, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE);
		CurrencyAmount netAmount = PRICER.netAmount(TRADE_EARLY, RATES_PROVIDER);
		CapitalIndexedBondPaymentPeriod period = PRODUCT.resolve(REF_DATA).PeriodicPayments.get(16);
		double pvDiff = PERIOD_PRICER.presentValue(period, RATES_PROVIDER, ISSUER_DISCOUNT_FACTORS) * QUANTITY;
		double df1 = ISSUER_RATES_PROVIDER.repoCurveDiscountFactors(SECURITY_ID, LEGAL_ENTITY, USD).discountFactor(SETTLEMENT_EARLY);
		double df2 = ISSUER_RATES_PROVIDER.repoCurveDiscountFactors(SECURITY_ID, LEGAL_ENTITY, USD).discountFactor(SETTLEMENT_STANDARD);
		double expected1 = netAmount.Amount * df1;
		double expected2 = -pvDiff + QUANTITY * df2 * PRICER.forecastValueStandardFromCleanPrice(RPRODUCT, RATES_PROVIDER, SETTLEMENT_STANDARD, TRADE_PRICE).Amount;
		assertEquals(computed.Amount, expected1 + expected2, NOTIONAL * QUANTITY * TOL);
	  }

	  public virtual void test_presentValueFromCleanPrice_early_exCoupon()
	  {
		CurrencyAmount computed = PRICER.presentValueFromCleanPrice(TRADE_EX_COUPON_EARLY, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE);
		CurrencyAmount netAmount = PRICER.netAmount(TRADE_EX_COUPON_EARLY, RATES_PROVIDER);
		double df1 = ISSUER_RATES_PROVIDER.repoCurveDiscountFactors(SECURITY_ID, LEGAL_ENTITY, USD).discountFactor(SETTLEMENT_EARLY);
		double df2 = ISSUER_RATES_PROVIDER.repoCurveDiscountFactors(SECURITY_ID, LEGAL_ENTITY, USD).discountFactor(SETTLEMENT_STANDARD);
		double expected1 = netAmount.Amount * df1;
		double expected2 = QUANTITY * df2 * PRICER.forecastValueStandardFromCleanPrice(RPRODUCT_EX_COUPON, RATES_PROVIDER, SETTLEMENT_STANDARD, TRADE_PRICE).Amount;
		assertEquals(computed.Amount, expected1 + expected2, NOTIONAL * QUANTITY * TOL);
	  }

	  public virtual void test_presentValueFromCleanPrice_late()
	  {
		CurrencyAmount computed = PRICER.presentValueFromCleanPrice(TRADE_LATE, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE);
		CurrencyAmount netAmount = PRICER.netAmount(TRADE_LATE, RATES_PROVIDER);
		CapitalIndexedBondPaymentPeriod period = PRODUCT.resolve(REF_DATA).PeriodicPayments.get(17);
		double pvDiff = PERIOD_PRICER.presentValue(period, RATES_PROVIDER, ISSUER_DISCOUNT_FACTORS) * QUANTITY;
		double df1 = ISSUER_RATES_PROVIDER.repoCurveDiscountFactors(SECURITY_ID, LEGAL_ENTITY, USD).discountFactor(SETTLEMENT_LATE);
		double df2 = ISSUER_RATES_PROVIDER.repoCurveDiscountFactors(SECURITY_ID, LEGAL_ENTITY, USD).discountFactor(SETTLEMENT_STANDARD);
		double expected1 = netAmount.Amount * df1;
		double expected2 = pvDiff + QUANTITY * df2 * PRICER.forecastValueStandardFromCleanPrice(RPRODUCT, RATES_PROVIDER, SETTLEMENT_STANDARD, TRADE_PRICE).Amount;
		assertEquals(computed.Amount, expected1 + expected2, NOTIONAL * QUANTITY * TOL);
	  }

	  public virtual void test_presentValueFromCleanPriceWithZSpread_standard()
	  {
		CurrencyAmount computed = PRICER.presentValueFromCleanPriceWithZSpread(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		CurrencyAmount netAmount = PRICER.netAmount(TRADE_STANDARD, RATES_PROVIDER);
		double df1 = ISSUER_RATES_PROVIDER.repoCurveDiscountFactors(SECURITY_ID, LEGAL_ENTITY, USD).discountFactor(SETTLEMENT_STANDARD);
		double df2 = ISSUER_RATES_PROVIDER.repoCurveDiscountFactors(SECURITY_ID, LEGAL_ENTITY, USD).discountFactor(SETTLEMENT_STANDARD);
		double expected1 = netAmount.Amount * df1;
		double expected2 = QUANTITY * df2 * PRICER.forecastValueStandardFromCleanPrice(RPRODUCT, RATES_PROVIDER, SETTLEMENT_STANDARD, TRADE_PRICE).Amount;
		assertEquals(computed.Amount, expected1 + expected2, NOTIONAL * QUANTITY * TOL);
	  }

	  public virtual void test_presentValueFromCleanPriceWithZSpread_early()
	  {
		CurrencyAmount computed = PRICER.presentValueFromCleanPriceWithZSpread(TRADE_EARLY, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE, Z_SPREAD, CONTINUOUS, 0);
		CurrencyAmount netAmount = PRICER.netAmount(TRADE_EARLY, RATES_PROVIDER);
		CapitalIndexedBondPaymentPeriod period = PRODUCT.resolve(REF_DATA).PeriodicPayments.get(16);
		double pvDiff = PERIOD_PRICER.presentValueWithZSpread(period, RATES_PROVIDER, ISSUER_DISCOUNT_FACTORS, Z_SPREAD, CONTINUOUS, 0) * QUANTITY;
		double df1 = ISSUER_RATES_PROVIDER.repoCurveDiscountFactors(SECURITY_ID, LEGAL_ENTITY, USD).discountFactor(SETTLEMENT_EARLY);
		double df2 = ISSUER_RATES_PROVIDER.repoCurveDiscountFactors(SECURITY_ID, LEGAL_ENTITY, USD).discountFactor(SETTLEMENT_STANDARD);
		double expected1 = netAmount.Amount * df1;
		double expected2 = -pvDiff + QUANTITY * df2 * PRICER.forecastValueStandardFromCleanPrice(RPRODUCT, RATES_PROVIDER, SETTLEMENT_STANDARD, TRADE_PRICE).Amount;
		assertEquals(computed.Amount, expected1 + expected2, NOTIONAL * QUANTITY * TOL);
	  }

	  public virtual void test_presentValueFromCleanPriceWithZSpread_early_exCoupon()
	  {
		CurrencyAmount computed = PRICER.presentValueFromCleanPriceWithZSpread(TRADE_EX_COUPON_EARLY, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE, Z_SPREAD, CONTINUOUS, 0);
		CurrencyAmount netAmount = PRICER.netAmount(TRADE_EX_COUPON_EARLY, RATES_PROVIDER);
		double df1 = ISSUER_RATES_PROVIDER.repoCurveDiscountFactors(SECURITY_ID, LEGAL_ENTITY, USD).discountFactor(SETTLEMENT_EARLY);
		double df2 = ISSUER_RATES_PROVIDER.repoCurveDiscountFactors(SECURITY_ID, LEGAL_ENTITY, USD).discountFactor(SETTLEMENT_STANDARD);
		double expected1 = netAmount.Amount * df1;
		double expected2 = QUANTITY * df2 * PRICER.forecastValueStandardFromCleanPrice(RPRODUCT_EX_COUPON, RATES_PROVIDER, SETTLEMENT_STANDARD, TRADE_PRICE).Amount;
		assertEquals(computed.Amount, expected1 + expected2, NOTIONAL * QUANTITY * TOL);
	  }

	  public virtual void test_presentValueFromCleanPriceWithZSpread_late()
	  {
		CurrencyAmount computed = PRICER.presentValueFromCleanPriceWithZSpread(TRADE_LATE, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE, Z_SPREAD, CONTINUOUS, 0);
		CurrencyAmount netAmount = PRICER.netAmount(TRADE_LATE, RATES_PROVIDER);
		CapitalIndexedBondPaymentPeriod period = PRODUCT.resolve(REF_DATA).PeriodicPayments.get(17);
		double pvDiff = PERIOD_PRICER.presentValueWithZSpread(period, RATES_PROVIDER, ISSUER_DISCOUNT_FACTORS, Z_SPREAD, CONTINUOUS, 0) * QUANTITY;
		double df1 = ISSUER_RATES_PROVIDER.repoCurveDiscountFactors(SECURITY_ID, LEGAL_ENTITY, USD).discountFactor(SETTLEMENT_LATE);
		double df2 = ISSUER_RATES_PROVIDER.repoCurveDiscountFactors(SECURITY_ID, LEGAL_ENTITY, USD).discountFactor(SETTLEMENT_STANDARD);
		double expected1 = netAmount.Amount * df1;
		double expected2 = pvDiff + QUANTITY * df2 * PRICER.forecastValueStandardFromCleanPrice(RPRODUCT, RATES_PROVIDER, SETTLEMENT_STANDARD, TRADE_PRICE).Amount;
		assertEquals(computed.Amount, expected1 + expected2, NOTIONAL * QUANTITY * TOL);
	  }

	  public virtual void test_presentValueFromCleanPrice_fixed()
	  {
		CurrencyAmount computed = PRICER.presentValueFromCleanPrice(TRADE_ILF_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE);
		assertEquals(computed.Amount, 0d, NOTIONAL * QUANTITY * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivityFromCleanPrice_standard()
	  {
		PointSensitivities point = PRICER.presentValueSensitivityFromCleanPrice(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE);
		CurrencyParameterSensitivities computed = ISSUER_RATES_PROVIDER.parameterSensitivity(point).combinedWith(RATES_PROVIDER.parameterSensitivity(point));
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATES_PROVIDER, p => PRICER.presentValueFromCleanPrice(TRADE_STANDARD, p, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE)).combinedWith(FD_CAL.sensitivity(ISSUER_RATES_PROVIDER, p => PRICER.presentValueFromCleanPrice(TRADE_STANDARD, RATES_PROVIDER, p, REF_DATA, TRADE_PRICE)));
		assertTrue(computed.equalWithTolerance(expected, NOTIONAL * QUANTITY * EPS));
	  }

	  public virtual void test_presentValueSensitivityFromCleanPrice_early_exCoupon()
	  {
		PointSensitivities point = PRICER.presentValueSensitivityFromCleanPrice(TRADE_EARLY, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE);
		CurrencyParameterSensitivities computed = ISSUER_RATES_PROVIDER.parameterSensitivity(point).combinedWith(RATES_PROVIDER.parameterSensitivity(point));
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATES_PROVIDER, p => PRICER.presentValueFromCleanPrice(TRADE_EARLY, p, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE)).combinedWith(FD_CAL.sensitivity(ISSUER_RATES_PROVIDER, p => PRICER.presentValueFromCleanPrice(TRADE_EARLY, RATES_PROVIDER, p, REF_DATA, TRADE_PRICE)));
		assertTrue(computed.equalWithTolerance(expected, NOTIONAL * QUANTITY * EPS));
	  }

	  public virtual void test_presentValueSensitivityFromCleanPrice_early()
	  {
		PointSensitivities point = PRICER.presentValueSensitivityFromCleanPrice(TRADE_EX_COUPON_EARLY, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE);
		CurrencyParameterSensitivities computed = ISSUER_RATES_PROVIDER.parameterSensitivity(point).combinedWith(RATES_PROVIDER.parameterSensitivity(point));
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATES_PROVIDER, p => PRICER.presentValueFromCleanPrice(TRADE_EX_COUPON_EARLY, p, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE)).combinedWith(FD_CAL.sensitivity(ISSUER_RATES_PROVIDER, p => PRICER.presentValueFromCleanPrice(TRADE_EX_COUPON_EARLY, RATES_PROVIDER, p, REF_DATA, TRADE_PRICE)));
		assertTrue(computed.equalWithTolerance(expected, NOTIONAL * QUANTITY * EPS));
	  }

	  public virtual void test_presentValueSensitivityFromCleanPrice_late()
	  {
		PointSensitivities point = PRICER.presentValueSensitivityFromCleanPrice(TRADE_LATE, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE);
		CurrencyParameterSensitivities computed = ISSUER_RATES_PROVIDER.parameterSensitivity(point).combinedWith(RATES_PROVIDER.parameterSensitivity(point));
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATES_PROVIDER, p => PRICER.presentValueFromCleanPrice(TRADE_LATE, p, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE)).combinedWith(FD_CAL.sensitivity(ISSUER_RATES_PROVIDER, p => PRICER.presentValueFromCleanPrice(TRADE_LATE, RATES_PROVIDER, p, REF_DATA, TRADE_PRICE)));
		assertTrue(computed.equalWithTolerance(expected, NOTIONAL * QUANTITY * EPS));
	  }

	  public virtual void test_presentValueSensitivityFromCleanPriceWithZSpread_standard()
	  {
		PointSensitivities point = PRICER.presentValueSensitivityFromCleanPriceWithZSpread(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE, Z_SPREAD, CONTINUOUS, 0);
		CurrencyParameterSensitivities computed = ISSUER_RATES_PROVIDER.parameterSensitivity(point).combinedWith(RATES_PROVIDER.parameterSensitivity(point));
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATES_PROVIDER, p => PRICER.presentValueFromCleanPriceWithZSpread(TRADE_STANDARD, p, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE, Z_SPREAD, CONTINUOUS, 0)).combinedWith(FD_CAL.sensitivity(ISSUER_RATES_PROVIDER, p => PRICER.presentValueFromCleanPriceWithZSpread(TRADE_STANDARD, RATES_PROVIDER, p, REF_DATA, TRADE_PRICE, Z_SPREAD, CONTINUOUS, 0)));
		assertTrue(computed.equalWithTolerance(expected, NOTIONAL * QUANTITY * EPS));
	  }

	  public virtual void test_presentValueSensitivityFromCleanPriceWithZSpread_early_exCoupon()
	  {
		PointSensitivities point = PRICER.presentValueSensitivityFromCleanPriceWithZSpread(TRADE_EX_COUPON_EARLY, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		CurrencyParameterSensitivities computed = ISSUER_RATES_PROVIDER.parameterSensitivity(point).combinedWith(RATES_PROVIDER.parameterSensitivity(point));
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATES_PROVIDER, p => PRICER.presentValueFromCleanPriceWithZSpread(TRADE_EX_COUPON_EARLY, p, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR)).combinedWith(FD_CAL.sensitivity(ISSUER_RATES_PROVIDER, p => PRICER.presentValueFromCleanPriceWithZSpread(TRADE_EX_COUPON_EARLY, RATES_PROVIDER, p, REF_DATA, TRADE_PRICE, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR)));
		assertTrue(computed.equalWithTolerance(expected, NOTIONAL * QUANTITY * EPS));
	  }

	  public virtual void test_presentValueSensitivityFromCleanPriceWithZSpread_early()
	  {
		PointSensitivities point = PRICER.presentValueSensitivityFromCleanPriceWithZSpread(TRADE_EARLY, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		CurrencyParameterSensitivities computed = ISSUER_RATES_PROVIDER.parameterSensitivity(point).combinedWith(RATES_PROVIDER.parameterSensitivity(point));
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATES_PROVIDER, p => PRICER.presentValueFromCleanPriceWithZSpread(TRADE_EARLY, p, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR)).combinedWith(FD_CAL.sensitivity(ISSUER_RATES_PROVIDER, p => PRICER.presentValueFromCleanPriceWithZSpread(TRADE_EARLY, RATES_PROVIDER, p, REF_DATA, TRADE_PRICE, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR)));
		assertTrue(computed.equalWithTolerance(expected, NOTIONAL * QUANTITY * EPS));
	  }

	  public virtual void test_presentValueSensitivityFromCleanPriceWithZSpread_late()
	  {
		PointSensitivities point = PRICER.presentValueSensitivityFromCleanPriceWithZSpread(TRADE_LATE, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		CurrencyParameterSensitivities computed = ISSUER_RATES_PROVIDER.parameterSensitivity(point).combinedWith(RATES_PROVIDER.parameterSensitivity(point));
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATES_PROVIDER, p => PRICER.presentValueFromCleanPriceWithZSpread(TRADE_LATE, p, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR)).combinedWith(FD_CAL.sensitivity(ISSUER_RATES_PROVIDER, p => PRICER.presentValueFromCleanPriceWithZSpread(TRADE_LATE, RATES_PROVIDER, p, REF_DATA, TRADE_PRICE, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR)));
		assertTrue(computed.equalWithTolerance(expected, NOTIONAL * QUANTITY * EPS));
	  }

	  public virtual void test_presentValueSensitivityFromCleanPrice_fixed()
	  {
		PointSensitivities point = PRICER.presentValueSensitivityFromCleanPrice(TRADE_ILF_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE);
		CurrencyParameterSensitivities computed = ISSUER_RATES_PROVIDER.parameterSensitivity(point).combinedWith(RATES_PROVIDER.parameterSensitivity(point));
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATES_PROVIDER, p => PRICER.presentValueFromCleanPrice(TRADE_ILF_STANDARD, p, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE)).combinedWith(FD_CAL.sensitivity(ISSUER_RATES_PROVIDER, p => PRICER.presentValueFromCleanPrice(TRADE_ILF_STANDARD, RATES_PROVIDER, p, REF_DATA, TRADE_PRICE)));
		assertTrue(computed.equalWithTolerance(expected, NOTIONAL * QUANTITY * EPS));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue_standard()
	  {
		CurrencyAmount computed = PRICER.presentValue(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER);
		double expected1 = PRODUCT_PRICER.presentValue(RPRODUCT, RATES_PROVIDER, ISSUER_RATES_PROVIDER, SETTLEMENT_STANDARD).Amount * QUANTITY;
		double df = ISSUER_RATES_PROVIDER.repoCurveDiscountFactors(SECURITY_ID, LEGAL_ENTITY, USD).discountFactor(SETTLEMENT_STANDARD);
		double expected2 = df * PERIOD_PRICER.forecastValue(SETTLE_PERIOD_STANDARD, RATES_PROVIDER);
		assertEquals(computed.Amount, expected1 + expected2, NOTIONAL * QUANTITY * TOL);
	  }

	  public virtual void test_presentValue_late()
	  {
		CurrencyAmount computed = PRICER.presentValue(TRADE_LATE, RATES_PROVIDER, ISSUER_RATES_PROVIDER);
		double expected1 = PRODUCT_PRICER.presentValue(RPRODUCT, RATES_PROVIDER, ISSUER_RATES_PROVIDER, SETTLEMENT_LATE).Amount * QUANTITY;
		double df = ISSUER_RATES_PROVIDER.repoCurveDiscountFactors(SECURITY_ID, LEGAL_ENTITY, USD).discountFactor(SETTLEMENT_LATE);
		double expected2 = df * PERIOD_PRICER.forecastValue(SETTLE_PERIOD_LATE, RATES_PROVIDER);
		assertEquals(computed.Amount, expected1 + expected2, NOTIONAL * QUANTITY * TOL);
	  }

	  public virtual void test_presentValueWithZSpread_standard()
	  {
		CurrencyAmount computed = PRICER.presentValueWithZSpread(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER, Z_SPREAD, CONTINUOUS, 0);
		double expected1 = QUANTITY * PRODUCT_PRICER.presentValueWithZSpread(RPRODUCT, RATES_PROVIDER, ISSUER_RATES_PROVIDER, SETTLEMENT_STANDARD, Z_SPREAD, CONTINUOUS, 0).Amount;
		double df = ISSUER_RATES_PROVIDER.repoCurveDiscountFactors(SECURITY_ID, LEGAL_ENTITY, USD).discountFactor(SETTLEMENT_STANDARD);
		double expected2 = df * PERIOD_PRICER.forecastValue(SETTLE_PERIOD_STANDARD, RATES_PROVIDER);
		assertEquals(computed.Amount, expected1 + expected2, NOTIONAL * QUANTITY * TOL);
	  }

	  public virtual void test_presentValueWithZSpread_late()
	  {
		CurrencyAmount computed = PRICER.presentValueWithZSpread(TRADE_LATE, RATES_PROVIDER, ISSUER_RATES_PROVIDER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		double expected1 = QUANTITY * PRODUCT_PRICER.presentValueWithZSpread(RPRODUCT, RATES_PROVIDER, ISSUER_RATES_PROVIDER, SETTLEMENT_LATE, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR).Amount;
		double df = ISSUER_RATES_PROVIDER.repoCurveDiscountFactors(SECURITY_ID, LEGAL_ENTITY, USD).discountFactor(SETTLEMENT_LATE);
		double expected2 = df * PERIOD_PRICER.forecastValue(SETTLE_PERIOD_LATE, RATES_PROVIDER);
		assertEquals(computed.Amount, expected1 + expected2, NOTIONAL * QUANTITY * TOL);
	  }

	  public virtual void test_presentValue_fixed()
	  {
		CurrencyAmount computed = PRICER.presentValue(TRADE_ILF_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER);
		double expected1 = PRODUCT_PRICER.presentValue(RPRODUCT_ILF, RATES_PROVIDER, ISSUER_RATES_PROVIDER, SETTLEMENT_STANDARD).Amount * QUANTITY;
		double df = ISSUER_RATES_PROVIDER.repoCurveDiscountFactors(SECURITY_ID, LEGAL_ENTITY, USD).discountFactor(SETTLEMENT_STANDARD);
		double expected2 = df * PAYMENT_PRICER.forecastValueAmount(SETTLE_PERIOD_ILF.Payment, RATES_PROVIDER);
		assertEquals(computed.Amount, expected1 + expected2, NOTIONAL * QUANTITY * TOL);
	  }

	  public virtual void test_presentValue_position()
	  {
		CurrencyAmount computed = PRICER.presentValue(POSITION, RATES_PROVIDER, ISSUER_RATES_PROVIDER);
		double expected1 = PRODUCT_PRICER.presentValue(RPRODUCT, RATES_PROVIDER, ISSUER_RATES_PROVIDER, VALUATION).Amount * QUANTITY;
		assertEquals(computed.Amount, expected1, NOTIONAL * QUANTITY * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity_standard()
	  {
		PointSensitivities point = PRICER.presentValueSensitivity(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER);
		CurrencyParameterSensitivities computed = ISSUER_RATES_PROVIDER.parameterSensitivity(point).combinedWith(RATES_PROVIDER.parameterSensitivity(point));
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATES_PROVIDER, p => PRICER.presentValue(TRADE_STANDARD, p, ISSUER_RATES_PROVIDER)).combinedWith(FD_CAL.sensitivity(ISSUER_RATES_PROVIDER, p => PRICER.presentValue(TRADE_STANDARD, RATES_PROVIDER, p)));
		assertTrue(computed.equalWithTolerance(expected, NOTIONAL * QUANTITY * EPS));
	  }

	  public virtual void test_presentValueSensitivity_late()
	  {
		PointSensitivities point = PRICER.presentValueSensitivity(TRADE_LATE, RATES_PROVIDER, ISSUER_RATES_PROVIDER);
		CurrencyParameterSensitivities computed = ISSUER_RATES_PROVIDER.parameterSensitivity(point).combinedWith(RATES_PROVIDER.parameterSensitivity(point));
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATES_PROVIDER, p => PRICER.presentValue(TRADE_LATE, p, ISSUER_RATES_PROVIDER)).combinedWith(FD_CAL.sensitivity(ISSUER_RATES_PROVIDER, p => PRICER.presentValue(TRADE_LATE, RATES_PROVIDER, p)));
		assertTrue(computed.equalWithTolerance(expected, NOTIONAL * QUANTITY * EPS));
	  }

	  public virtual void test_presentValueSensitivityWithZSpread_standard()
	  {
		PointSensitivities point = PRICER.presentValueSensitivityWithZSpread(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		CurrencyParameterSensitivities computed = ISSUER_RATES_PROVIDER.parameterSensitivity(point).combinedWith(RATES_PROVIDER.parameterSensitivity(point));
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATES_PROVIDER, p => PRICER.presentValueWithZSpread(TRADE_STANDARD, p, ISSUER_RATES_PROVIDER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR)).combinedWith(FD_CAL.sensitivity(ISSUER_RATES_PROVIDER, p => PRICER.presentValueWithZSpread(TRADE_STANDARD, RATES_PROVIDER, p, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR)));
		assertTrue(computed.equalWithTolerance(expected, NOTIONAL * QUANTITY * EPS));
	  }

	  public virtual void test_presentValueSensitivityWithZSpread_late()
	  {
		PointSensitivities point = PRICER.presentValueSensitivityWithZSpread(TRADE_LATE, RATES_PROVIDER, ISSUER_RATES_PROVIDER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		CurrencyParameterSensitivities computed = ISSUER_RATES_PROVIDER.parameterSensitivity(point).combinedWith(RATES_PROVIDER.parameterSensitivity(point));
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATES_PROVIDER, p => PRICER.presentValueWithZSpread(TRADE_LATE, p, ISSUER_RATES_PROVIDER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR)).combinedWith(FD_CAL.sensitivity(ISSUER_RATES_PROVIDER, p => PRICER.presentValueWithZSpread(TRADE_LATE, RATES_PROVIDER, p, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR)));
		assertTrue(computed.equalWithTolerance(expected, NOTIONAL * QUANTITY * EPS));
	  }

	  public virtual void test_presentValueSensitivity_fixed()
	  {
		PointSensitivities point = PRICER.presentValueSensitivity(TRADE_ILF_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER);
		CurrencyParameterSensitivities computed = ISSUER_RATES_PROVIDER.parameterSensitivity(point).combinedWith(RATES_PROVIDER.parameterSensitivity(point));
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATES_PROVIDER, p => PRICER.presentValue(TRADE_ILF_STANDARD, p, ISSUER_RATES_PROVIDER)).combinedWith(FD_CAL.sensitivity(ISSUER_RATES_PROVIDER, p => PRICER.presentValue(TRADE_ILF_STANDARD, RATES_PROVIDER, p)));
		assertTrue(computed.equalWithTolerance(expected, NOTIONAL * QUANTITY * EPS));
	  }

	  public virtual void test_presentValueSensitivity_position()
	  {
		PointSensitivities point = PRICER.presentValueSensitivity(POSITION, RATES_PROVIDER, ISSUER_RATES_PROVIDER);
		CurrencyParameterSensitivities computed = ISSUER_RATES_PROVIDER.parameterSensitivity(point).combinedWith(RATES_PROVIDER.parameterSensitivity(point));
		CurrencyParameterSensitivities fdRates = FD_CAL.sensitivity(RATES_PROVIDER, p => PRICER.presentValue(POSITION, p, ISSUER_RATES_PROVIDER));
		CurrencyParameterSensitivities fdPrice = FD_CAL.sensitivity(ISSUER_RATES_PROVIDER, p => PRICER.presentValue(POSITION, RATES_PROVIDER, p));
		CurrencyParameterSensitivities expected = fdRates.combinedWith(fdPrice);
		assertTrue(computed.equalWithTolerance(expected, NOTIONAL * QUANTITY * EPS));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currencyExposureFromCleanPrice()
	  {
		MultiCurrencyAmount computed = PRICER.currencyExposureFromCleanPrice(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE);
		PointSensitivities point = PRICER.presentValueSensitivityFromCleanPrice(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE);
		MultiCurrencyAmount expected = RATES_PROVIDER.currencyExposure(point).plus(PRICER.presentValueFromCleanPrice(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE));
		assertEquals(computed.Amounts.size(), 1);
		assertEquals(computed.getAmount(USD).Amount, expected.getAmount(USD).Amount, NOTIONAL * QUANTITY * TOL);
	  }

	  public virtual void test_currencyExposureFromCleanPriceWithZSpread()
	  {
		MultiCurrencyAmount computed = PRICER.currencyExposureFromCleanPriceWithZSpread(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		PointSensitivities point = PRICER.presentValueSensitivityFromCleanPriceWithZSpread(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		MultiCurrencyAmount expected = RATES_PROVIDER.currencyExposure(point).plus(PRICER.presentValueFromCleanPriceWithZSpread(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, TRADE_PRICE, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR));
		assertEquals(computed.Amounts.size(), 1);
		assertEquals(computed.getAmount(USD).Amount, expected.getAmount(USD).Amount, NOTIONAL * QUANTITY * TOL);
	  }

	  public virtual void test_currencyExposure()
	  {
		MultiCurrencyAmount computed = PRICER.currencyExposure(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER);
		PointSensitivities point = PRICER.presentValueSensitivity(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER);
		MultiCurrencyAmount expected = RATES_PROVIDER.currencyExposure(point).plus(PRICER.presentValue(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER));
		assertEquals(computed.Amounts.size(), 1);
		assertEquals(computed.getAmount(USD).Amount, expected.getAmount(USD).Amount, NOTIONAL * QUANTITY * TOL);
	  }

	  public virtual void test_currencyExposureWithZSpread()
	  {
		MultiCurrencyAmount computed = PRICER.currencyExposureWithZSpread(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		PointSensitivities point = PRICER.presentValueSensitivityWithZSpread(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		MultiCurrencyAmount expected = RATES_PROVIDER.currencyExposure(point).plus(PRICER.presentValueWithZSpread(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR));
		assertEquals(computed.Amounts.size(), 1);
		assertEquals(computed.getAmount(USD).Amount, expected.getAmount(USD).Amount, NOTIONAL * QUANTITY * TOL);
	  }

	  public virtual void test_currentCash()
	  {
		CurrencyAmount computed = PRICER.currentCash(TRADE_SETTLED, RATES_PROVIDER_ON_PAY);
		CurrencyAmount expected = PRODUCT_PRICER.currentCash(RPRODUCT, RATES_PROVIDER_ON_PAY, SETTLEMENT_BEFORE);
		assertEquals(computed.Amount, expected.Amount, NOTIONAL * QUANTITY * TOL);
	  }

	  public virtual void test_currentCash_position()
	  {
		CurrencyAmount computed = PRICER.currentCash(POSITION, RATES_PROVIDER_ON_PAY);
		CurrencyAmount expected = PRODUCT_PRICER.currentCash(RPRODUCT, RATES_PROVIDER_ON_PAY, PAYMENT);
		assertEquals(computed.Amount, expected.Amount, NOTIONAL * QUANTITY * TOL);
	  }

	  public virtual void test_currentCash_early()
	  {
		CurrencyAmount computed = PRICER.currentCash(TRADE_EARLY, RATES_PROVIDER);
		CurrencyAmount expected = PRICER.netAmount(TRADE_EARLY, RATES_PROVIDER);
		assertEquals(computed.Amount, expected.Amount, NOTIONAL * QUANTITY * TOL);
	  }

	  //-------------------------------------------------------------------------
	  private static readonly double CLEAN_REAL_FROM_CURVES;
	  private static readonly double CLEAN_REAL_FROM_CURVES_ZSPREAD;
	  static DiscountingCapitalIndexedBondTradePricerTest()
	  {
		double dirtyNominal = PRODUCT_PRICER.dirtyNominalPriceFromCurves(RPRODUCT, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA);
		double cleanNominal = PRODUCT_PRICER.cleanNominalPriceFromDirtyNominalPrice(RPRODUCT, RATES_PROVIDER, SETTLEMENT_STANDARD, dirtyNominal);
		CLEAN_REAL_FROM_CURVES = PRODUCT_PRICER.realPriceFromNominalPrice(RPRODUCT, RATES_PROVIDER, SETTLEMENT_STANDARD, cleanNominal);
		double dirtyNominalZSpread = PRODUCT_PRICER.dirtyNominalPriceFromCurvesWithZSpread(RPRODUCT, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		double cleanNominalZSpread = PRODUCT_PRICER.cleanNominalPriceFromDirtyNominalPrice(RPRODUCT, RATES_PROVIDER, SETTLEMENT_STANDARD, dirtyNominalZSpread);
		CLEAN_REAL_FROM_CURVES_ZSPREAD = PRODUCT_PRICER.realPriceFromNominalPrice(RPRODUCT, RATES_PROVIDER, SETTLEMENT_STANDARD, cleanNominalZSpread);
	  }

	  public virtual void test_presentValue_coherency_standard()
	  {
		CurrencyAmount pvFromCleanPrice = PRICER.presentValueFromCleanPrice(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, CLEAN_REAL_FROM_CURVES);
		CurrencyAmount pvFromCurves = PRICER.presentValue(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER);
		assertEquals(pvFromCleanPrice.Amount, pvFromCurves.Amount, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_coherency_exCoupon()
	  {
		CurrencyAmount pvFromCleanPrice = PRICER.presentValueFromCleanPrice(TRADE_EX_COUPON_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, CLEAN_REAL_FROM_CURVES);
		CurrencyAmount pvFromCurves = PRICER.presentValue(TRADE_EX_COUPON_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER);
		assertEquals(pvFromCleanPrice.Amount, pvFromCurves.Amount, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueWithZSpread_coherency_standard()
	  {
		CurrencyAmount pvFromCleanPrice = PRICER.presentValueFromCleanPriceWithZSpread(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, CLEAN_REAL_FROM_CURVES_ZSPREAD, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		CurrencyAmount pvFromCurves = PRICER.presentValueWithZSpread(TRADE_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		assertEquals(pvFromCleanPrice.Amount, pvFromCurves.Amount, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueWithZSpread_coherency_exCoupon()
	  {
		CurrencyAmount pvFromCleanPrice = PRICER.presentValueFromCleanPriceWithZSpread(TRADE_EX_COUPON_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER, REF_DATA, CLEAN_REAL_FROM_CURVES_ZSPREAD, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		CurrencyAmount pvFromCurves = PRICER.presentValueWithZSpread(TRADE_EX_COUPON_STANDARD, RATES_PROVIDER, ISSUER_RATES_PROVIDER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		assertEquals(pvFromCleanPrice.Amount, pvFromCurves.Amount, NOTIONAL * TOL);
	  }

	}

}