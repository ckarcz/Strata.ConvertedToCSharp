/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using LegalEntityGroup = com.opengamma.strata.market.curve.LegalEntityGroup;
	using RepoGroup = com.opengamma.strata.market.curve.RepoGroup;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using LegalEntityId = com.opengamma.strata.product.LegalEntityId;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using Bill = com.opengamma.strata.product.bond.Bill;
	using BillTrade = com.opengamma.strata.product.bond.BillTrade;
	using BillYieldConvention = com.opengamma.strata.product.bond.BillYieldConvention;
	using ResolvedBillTrade = com.opengamma.strata.product.bond.ResolvedBillTrade;

	/// <summary>
	/// Test <seealso cref="DiscountingBillTradePricer"/>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingBillTradePricerTest
	public class DiscountingBillTradePricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = date(2018, 6, 20);

	  // Bill
	  private static readonly SecurityId SECURITY_ID = SecurityId.of("OG-Ticker", "GOVT1-BOND1");
	  private static readonly LegalEntityId ISSUER_ID = LegalEntityId.of("OG-Ticker", "GOVT1");
	  private const BillYieldConvention YIELD_CONVENTION = BillYieldConvention.INTEREST_AT_MATURITY;

	  private static readonly HolidayCalendarId EUR_CALENDAR = HolidayCalendarIds.EUTA;
	  private static readonly BusinessDayAdjustment BUSINESS_ADJUST = BusinessDayAdjustment.of(BusinessDayConventions.MODIFIED_FOLLOWING, EUR_CALENDAR);
	  private const double NOTIONAL_AMOUNT = 1_000_000;
	  private static readonly LocalDate MATURITY_DATE = LocalDate.of(2018, 12, 12);
	  private static readonly AdjustableDate MATURITY_DATE_ADJ = AdjustableDate.of(MATURITY_DATE, BUSINESS_ADJUST);
	  private static readonly AdjustablePayment NOTIONAL = AdjustablePayment.of(CurrencyAmount.of(EUR, NOTIONAL_AMOUNT), MATURITY_DATE_ADJ);

	  private static readonly DaysAdjustment DATE_OFFSET = DaysAdjustment.ofBusinessDays(2, EUR_CALENDAR);
	  private static readonly DayCount DAY_COUNT = DayCounts.ACT_360;
	  private static readonly Bill BILL_PRODUCT = Bill.builder().dayCount(DAY_COUNT).legalEntityId(ISSUER_ID).notional(NOTIONAL).securityId(SECURITY_ID).settlementDateOffset(DATE_OFFSET).yieldConvention(YIELD_CONVENTION).build();
	  private static readonly LocalDate TRADE_DATE_BEFORE_VAL = date(2018, 6, 13);
	  private static readonly LocalDate SETTLEMENT_DATE_BEFORE_VAL = date(2018, 6, 14);
	  private static readonly LocalDate SETTLEMENT_DATE_ON_VAL = date(2018, 6, 20);
	  private static readonly LocalDate SETTLEMENT_DATE_AFTER_VAL = date(2018, 6, 22);
	  private static readonly TradeInfo TRADE_INFO_BEFORE_VAL = TradeInfo.builder().tradeDate(TRADE_DATE_BEFORE_VAL).settlementDate(SETTLEMENT_DATE_BEFORE_VAL).build();
	  private static readonly TradeInfo TRADE_INFO_ON_VAL = TradeInfo.builder().tradeDate(TRADE_DATE_BEFORE_VAL).settlementDate(SETTLEMENT_DATE_ON_VAL).build();
	  private static readonly TradeInfo TRADE_INFO_AFTER_VAL = TradeInfo.builder().tradeDate(TRADE_DATE_BEFORE_VAL).settlementDate(SETTLEMENT_DATE_AFTER_VAL).build();
	  private const double PRICE = 0.99;
	  private const double QUANTITY = 123;
	  private static readonly ResolvedBillTrade BILL_TRADE_SETTLE_BEFORE_VAL = BillTrade.builder().info(TRADE_INFO_BEFORE_VAL).product(BILL_PRODUCT).price(PRICE).quantity(QUANTITY).build().resolve(REF_DATA);
	  private static readonly ResolvedBillTrade BILL_TRADE_SETTLE_ON_VAL = BillTrade.builder().info(TRADE_INFO_ON_VAL).product(BILL_PRODUCT).price(PRICE).quantity(QUANTITY).build().resolve(REF_DATA);
	  private static readonly ResolvedBillTrade BILL_TRADE_SETTLE_AFTER_VAL = BillTrade.builder().info(TRADE_INFO_AFTER_VAL).product(BILL_PRODUCT).price(PRICE).quantity(QUANTITY).build().resolve(REF_DATA);

	  // rates provider
	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;
	  private static readonly CurveName NAME_REPO = CurveName.of("TestRepoCurve");
	  private static readonly CurveMetadata METADATA_REPO = Curves.zeroRates(NAME_REPO, ACT_365F);
	  private static readonly InterpolatedNodalCurve CURVE_REPO = InterpolatedNodalCurve.of(METADATA_REPO, DoubleArray.of(0.1, 2.0, 10.0), DoubleArray.of(0.05, 0.06, 0.09), INTERPOLATOR);
	  private static readonly DiscountFactors DSC_FACTORS_REPO = ZeroRateDiscountFactors.of(EUR, VAL_DATE, CURVE_REPO);
	  private static readonly RepoGroup GROUP_REPO = RepoGroup.of("GOVT1 BOND1");
	  private static readonly CurveName NAME_ISSUER = CurveName.of("TestIssuerCurve");
	  private static readonly CurveMetadata METADATA_ISSUER = Curves.zeroRates(NAME_ISSUER, ACT_365F);
	  private static readonly InterpolatedNodalCurve CURVE_ISSUER = InterpolatedNodalCurve.of(METADATA_ISSUER, DoubleArray.of(0.2, 9.0, 15.0), DoubleArray.of(0.03, 0.05, 0.13), INTERPOLATOR);
	  private static readonly DiscountFactors DSC_FACTORS_ISSUER = ZeroRateDiscountFactors.of(EUR, VAL_DATE, CURVE_ISSUER);
	  private static readonly LegalEntityGroup GROUP_ISSUER = LegalEntityGroup.of("GOVT1");
	  private static readonly LegalEntityDiscountingProvider PROVIDER = ImmutableLegalEntityDiscountingProvider.builder().issuerCurves(ImmutableMap.of(Pair.of(GROUP_ISSUER, EUR), DSC_FACTORS_ISSUER)).issuerCurveGroups(ImmutableMap.of(ISSUER_ID, GROUP_ISSUER)).repoCurves(ImmutableMap.of(Pair.of(GROUP_REPO, EUR), DSC_FACTORS_REPO)).repoCurveSecurityGroups(ImmutableMap.of(SECURITY_ID, GROUP_REPO)).valuationDate(VAL_DATE).build();

	  // pricers
	  private static readonly DiscountingBillProductPricer PRICER_PRODUCT = DiscountingBillProductPricer.DEFAULT;
	  private static readonly DiscountingBillTradePricer PRICER_TRADE = DiscountingBillTradePricer.DEFAULT;
	  private static readonly DiscountingPaymentPricer PRICER_PAYMENT = DiscountingPaymentPricer.DEFAULT;
	  private const double EPS = 1.0e-7;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FD_CALC = new RatesFiniteDifferenceSensitivityCalculator(EPS);

	  private const double Z_SPREAD = 0.035;
	  private const double TOLERANCE_PV = 1.0e-6;
	  private const double TOLERANCE_PVSENSI = 1.0e-2;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue_settle_before_val()
	  {
		CurrencyAmount pvComputed = PRICER_TRADE.presentValue(BILL_TRADE_SETTLE_BEFORE_VAL, PROVIDER);
		CurrencyAmount pvExpected = PRICER_PRODUCT.presentValue(BILL_PRODUCT.resolve(REF_DATA), PROVIDER).multipliedBy(QUANTITY);
		assertEquals(pvComputed.Currency, EUR);
		assertEquals(pvComputed.Amount, pvExpected.Amount, TOLERANCE_PV);
		MultiCurrencyAmount ceComputed = PRICER_TRADE.currencyExposure(BILL_TRADE_SETTLE_BEFORE_VAL, PROVIDER);
		assertEquals(ceComputed.Currencies.size(), 1);
		assertTrue(ceComputed.contains(EUR));
		assertEquals(ceComputed.getAmount(EUR).Amount, pvExpected.Amount, TOLERANCE_PV);
		CurrencyAmount cashComputed = PRICER_TRADE.currentCash(BILL_TRADE_SETTLE_BEFORE_VAL, VAL_DATE);
		assertEquals(cashComputed.Currency, EUR);
		assertEquals(cashComputed.Amount, 0, TOLERANCE_PV);
	  }

	  public virtual void test_presentValue_settle_on_val()
	  {
		CurrencyAmount pvComputed = PRICER_TRADE.presentValue(BILL_TRADE_SETTLE_ON_VAL, PROVIDER);
		CurrencyAmount pvExpected = PRICER_PRODUCT.presentValue(BILL_PRODUCT.resolve(REF_DATA), PROVIDER).plus(-PRICE * NOTIONAL_AMOUNT).multipliedBy(QUANTITY);
		assertEquals(pvComputed.Currency, EUR);
		assertEquals(pvComputed.Amount, pvExpected.Amount, TOLERANCE_PV);
		MultiCurrencyAmount ceComputed = PRICER_TRADE.currencyExposure(BILL_TRADE_SETTLE_ON_VAL, PROVIDER);
		assertEquals(ceComputed.Currencies.size(), 1);
		assertTrue(ceComputed.contains(EUR));
		assertEquals(ceComputed.getAmount(EUR).Amount, pvExpected.Amount, TOLERANCE_PV);
		CurrencyAmount cashComputed = PRICER_TRADE.currentCash(BILL_TRADE_SETTLE_ON_VAL, VAL_DATE);
		assertEquals(cashComputed.Currency, EUR);
		assertEquals(cashComputed.Amount, -PRICE * NOTIONAL_AMOUNT * QUANTITY, TOLERANCE_PV);
	  }

	  public virtual void test_presentValue_settle_after_val()
	  {
		CurrencyAmount pvComputed = PRICER_TRADE.presentValue(BILL_TRADE_SETTLE_AFTER_VAL, PROVIDER);
		CurrencyAmount pvExpected = PRICER_PRODUCT.presentValue(BILL_PRODUCT.resolve(REF_DATA), PROVIDER).multipliedBy(QUANTITY).plus(PRICER_PAYMENT.presentValue(BILL_TRADE_SETTLE_AFTER_VAL.Settlement.get(), PROVIDER.repoCurveDiscountFactors(BILL_PRODUCT.SecurityId, BILL_PRODUCT.LegalEntityId, BILL_PRODUCT.Currency).DiscountFactors));
		assertEquals(pvComputed.Currency, EUR);
		assertEquals(pvComputed.Amount, pvExpected.Amount, TOLERANCE_PV);
		MultiCurrencyAmount ceComputed = PRICER_TRADE.currencyExposure(BILL_TRADE_SETTLE_AFTER_VAL, PROVIDER);
		assertEquals(ceComputed.Currencies.size(), 1);
		assertTrue(ceComputed.contains(EUR));
		assertEquals(ceComputed.getAmount(EUR).Amount, pvExpected.Amount, TOLERANCE_PV);
		CurrencyAmount cashComputed = PRICER_TRADE.currentCash(BILL_TRADE_SETTLE_AFTER_VAL, VAL_DATE);
		assertEquals(cashComputed.Currency, EUR);
		assertEquals(cashComputed.Amount, 0, TOLERANCE_PV);
	  }

	  public virtual void test_currentcash_on_maturity()
	  {
		CurrencyAmount cashComputed = PRICER_TRADE.currentCash(BILL_TRADE_SETTLE_AFTER_VAL, MATURITY_DATE);
		assertEquals(cashComputed.Currency, EUR);
		assertEquals(cashComputed.Amount, NOTIONAL_AMOUNT * QUANTITY, TOLERANCE_PV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueZSpread_settle_before_val()
	  {
		CurrencyAmount pvComputed = PRICER_TRADE.presentValueWithZSpread(BILL_TRADE_SETTLE_BEFORE_VAL, PROVIDER, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0);
		CurrencyAmount pvExpected = PRICER_PRODUCT.presentValueWithZSpread(BILL_PRODUCT.resolve(REF_DATA), PROVIDER, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0).multipliedBy(QUANTITY);
		assertEquals(pvComputed.Currency, EUR);
		assertEquals(pvComputed.Amount, pvExpected.Amount, TOLERANCE_PV);
		MultiCurrencyAmount ceComputed = PRICER_TRADE.currencyExposureWithZSpread(BILL_TRADE_SETTLE_BEFORE_VAL, PROVIDER, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0);
		assertEquals(ceComputed.Currencies.size(), 1);
		assertTrue(ceComputed.contains(EUR));
		assertEquals(ceComputed.getAmount(EUR).Amount, pvExpected.Amount, TOLERANCE_PV);
	  }

	  public virtual void test_presentValueZSpread_settle_on_val()
	  {
		CurrencyAmount pvComputed = PRICER_TRADE.presentValueWithZSpread(BILL_TRADE_SETTLE_ON_VAL, PROVIDER, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0);
		CurrencyAmount pvExpected = PRICER_PRODUCT.presentValueWithZSpread(BILL_PRODUCT.resolve(REF_DATA), PROVIDER, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0).plus(-PRICE * NOTIONAL_AMOUNT).multipliedBy(QUANTITY);
		assertEquals(pvComputed.Currency, EUR);
		assertEquals(pvComputed.Amount, pvExpected.Amount, TOLERANCE_PV);
		MultiCurrencyAmount ceComputed = PRICER_TRADE.currencyExposureWithZSpread(BILL_TRADE_SETTLE_ON_VAL, PROVIDER, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0);
		assertEquals(ceComputed.Currencies.size(), 1);
		assertTrue(ceComputed.contains(EUR));
		assertEquals(ceComputed.getAmount(EUR).Amount, pvExpected.Amount, TOLERANCE_PV);
	  }

	  public virtual void test_presentValueZSpread_settle_after_val()
	  {
		CurrencyAmount pvComputed = PRICER_TRADE.presentValueWithZSpread(BILL_TRADE_SETTLE_AFTER_VAL, PROVIDER, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0);
		CurrencyAmount pvExpected = PRICER_PRODUCT.presentValueWithZSpread(BILL_PRODUCT.resolve(REF_DATA), PROVIDER, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0).multipliedBy(QUANTITY).plus(PRICER_PAYMENT.presentValue(BILL_TRADE_SETTLE_AFTER_VAL.Settlement.get(), PROVIDER.repoCurveDiscountFactors(BILL_PRODUCT.SecurityId, BILL_PRODUCT.LegalEntityId, BILL_PRODUCT.Currency).DiscountFactors));
		assertEquals(pvComputed.Currency, EUR);
		assertEquals(pvComputed.Amount, pvExpected.Amount, TOLERANCE_PV);
		MultiCurrencyAmount ceComputed = PRICER_TRADE.currencyExposureWithZSpread(BILL_TRADE_SETTLE_AFTER_VAL, PROVIDER, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0);
		assertEquals(ceComputed.Currencies.size(), 1);
		assertTrue(ceComputed.contains(EUR));
		assertEquals(ceComputed.getAmount(EUR).Amount, pvExpected.Amount, TOLERANCE_PV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_pvsensi_settle_before_val()
	  {
		PointSensitivities pvsensiComputed = PRICER_TRADE.presentValueSensitivity(BILL_TRADE_SETTLE_BEFORE_VAL, PROVIDER);
		PointSensitivities pvsensiExpected = PRICER_PRODUCT.presentValueSensitivity(BILL_PRODUCT.resolve(REF_DATA), PROVIDER).multipliedBy(QUANTITY);
		assertTrue(pvsensiComputed.equalWithTolerance(pvsensiExpected, TOLERANCE_PVSENSI));
		CurrencyParameterSensitivities paramSensiComputed = PROVIDER.parameterSensitivity(pvsensiComputed);
		CurrencyParameterSensitivities paramSensiExpected = FD_CALC.sensitivity(PROVIDER, p => PRICER_TRADE.presentValue(BILL_TRADE_SETTLE_BEFORE_VAL, p));
		assertTrue(paramSensiComputed.equalWithTolerance(paramSensiExpected, EPS * NOTIONAL_AMOUNT * QUANTITY));
	  }

	  public virtual void test_pvsensi_settle_on_val()
	  {
		PointSensitivities pvsensiComputed = PRICER_TRADE.presentValueSensitivity(BILL_TRADE_SETTLE_ON_VAL, PROVIDER);
		PointSensitivities pvsensiExpected = PRICER_PRODUCT.presentValueSensitivity(BILL_PRODUCT.resolve(REF_DATA), PROVIDER).multipliedBy(QUANTITY).combinedWith(RepoCurveZeroRateSensitivity.of((ZeroRateSensitivity) PRICER_PAYMENT.presentValueSensitivity(BILL_TRADE_SETTLE_ON_VAL.Settlement.get(), PROVIDER.repoCurveDiscountFactors(BILL_PRODUCT.SecurityId, BILL_PRODUCT.LegalEntityId, BILL_PRODUCT.Currency).DiscountFactors), GROUP_REPO).build());
		assertTrue(pvsensiComputed.equalWithTolerance(pvsensiExpected, TOLERANCE_PVSENSI));
		CurrencyParameterSensitivities paramSensiComputed = PROVIDER.parameterSensitivity(pvsensiComputed);
		CurrencyParameterSensitivities paramSensiExpected = FD_CALC.sensitivity(PROVIDER, p => PRICER_TRADE.presentValue(BILL_TRADE_SETTLE_ON_VAL, p));
		assertTrue(paramSensiComputed.equalWithTolerance(paramSensiExpected, EPS * NOTIONAL_AMOUNT * QUANTITY));
	  }

	  public virtual void test_pvsensi_settle_after_val()
	  {
		PointSensitivities pvsensiComputed = PRICER_TRADE.presentValueSensitivity(BILL_TRADE_SETTLE_AFTER_VAL, PROVIDER);
		PointSensitivities pvsensiExpected = PRICER_PRODUCT.presentValueSensitivity(BILL_PRODUCT.resolve(REF_DATA), PROVIDER).multipliedBy(QUANTITY).combinedWith(RepoCurveZeroRateSensitivity.of((ZeroRateSensitivity) PRICER_PAYMENT.presentValueSensitivity(BILL_TRADE_SETTLE_AFTER_VAL.Settlement.get(), PROVIDER.repoCurveDiscountFactors(BILL_PRODUCT.SecurityId, BILL_PRODUCT.LegalEntityId, BILL_PRODUCT.Currency).DiscountFactors), GROUP_REPO).build());
		assertTrue(pvsensiComputed.equalWithTolerance(pvsensiExpected, TOLERANCE_PVSENSI));
		CurrencyParameterSensitivities paramSensiComputed = PROVIDER.parameterSensitivity(pvsensiComputed);
		CurrencyParameterSensitivities paramSensiExpected = FD_CALC.sensitivity(PROVIDER, p => PRICER_TRADE.presentValue(BILL_TRADE_SETTLE_AFTER_VAL, p));
		assertTrue(paramSensiComputed.equalWithTolerance(paramSensiExpected, EPS * NOTIONAL_AMOUNT * QUANTITY));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_pvsensiZSpread_settle_before_val()
	  {
		PointSensitivities pvsensiComputed = PRICER_TRADE.presentValueSensitivityWithZSpread(BILL_TRADE_SETTLE_BEFORE_VAL, PROVIDER, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0);
		PointSensitivities pvsensiExpected = PRICER_PRODUCT.presentValueSensitivityWithZSpread(BILL_PRODUCT.resolve(REF_DATA), PROVIDER, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0).multipliedBy(QUANTITY);
		assertTrue(pvsensiComputed.equalWithTolerance(pvsensiExpected, TOLERANCE_PVSENSI));
		CurrencyParameterSensitivities paramSensiComputed = PROVIDER.parameterSensitivity(pvsensiComputed);
		CurrencyParameterSensitivities paramSensiExpected = FD_CALC.sensitivity(PROVIDER, p => PRICER_TRADE.presentValueWithZSpread(BILL_TRADE_SETTLE_BEFORE_VAL, p, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0));
		assertTrue(paramSensiComputed.equalWithTolerance(paramSensiExpected, EPS * NOTIONAL_AMOUNT * QUANTITY));
	  }

	  public virtual void test_pvsensiZSpread_settle_on_val()
	  {
		PointSensitivities pvsensiComputed = PRICER_TRADE.presentValueSensitivityWithZSpread(BILL_TRADE_SETTLE_ON_VAL, PROVIDER, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0);
		PointSensitivities pvsensiExpected = PRICER_PRODUCT.presentValueSensitivityWithZSpread(BILL_PRODUCT.resolve(REF_DATA), PROVIDER, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0).multipliedBy(QUANTITY).combinedWith(RepoCurveZeroRateSensitivity.of((ZeroRateSensitivity) PRICER_PAYMENT.presentValueSensitivity(BILL_TRADE_SETTLE_ON_VAL.Settlement.get(), PROVIDER.repoCurveDiscountFactors(BILL_PRODUCT.SecurityId, BILL_PRODUCT.LegalEntityId, BILL_PRODUCT.Currency).DiscountFactors), GROUP_REPO).build());
		assertTrue(pvsensiComputed.equalWithTolerance(pvsensiExpected, TOLERANCE_PVSENSI));
		CurrencyParameterSensitivities paramSensiComputed = PROVIDER.parameterSensitivity(pvsensiComputed);
		CurrencyParameterSensitivities paramSensiExpected = FD_CALC.sensitivity(PROVIDER, p => PRICER_TRADE.presentValueWithZSpread(BILL_TRADE_SETTLE_ON_VAL, p, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0));
		assertTrue(paramSensiComputed.equalWithTolerance(paramSensiExpected, EPS * NOTIONAL_AMOUNT * QUANTITY));
	  }

	  public virtual void test_pvsensiZSpread_settle_after_val()
	  {
		PointSensitivities pvsensiComputed = PRICER_TRADE.presentValueSensitivityWithZSpread(BILL_TRADE_SETTLE_AFTER_VAL, PROVIDER, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0);
		PointSensitivities pvsensiExpected = PRICER_PRODUCT.presentValueSensitivityWithZSpread(BILL_PRODUCT.resolve(REF_DATA), PROVIDER, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0).multipliedBy(QUANTITY).combinedWith(RepoCurveZeroRateSensitivity.of((ZeroRateSensitivity) PRICER_PAYMENT.presentValueSensitivity(BILL_TRADE_SETTLE_AFTER_VAL.Settlement.get(), PROVIDER.repoCurveDiscountFactors(BILL_PRODUCT.SecurityId, BILL_PRODUCT.LegalEntityId, BILL_PRODUCT.Currency).DiscountFactors), GROUP_REPO).build());
		assertTrue(pvsensiComputed.equalWithTolerance(pvsensiExpected, TOLERANCE_PVSENSI));
		CurrencyParameterSensitivities paramSensiComputed = PROVIDER.parameterSensitivity(pvsensiComputed);
		CurrencyParameterSensitivities paramSensiExpected = FD_CALC.sensitivity(PROVIDER, p => PRICER_TRADE.presentValueWithZSpread(BILL_TRADE_SETTLE_AFTER_VAL, p, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0));
		assertTrue(paramSensiComputed.equalWithTolerance(paramSensiExpected, EPS * NOTIONAL_AMOUNT * QUANTITY));
	  }

	}

}