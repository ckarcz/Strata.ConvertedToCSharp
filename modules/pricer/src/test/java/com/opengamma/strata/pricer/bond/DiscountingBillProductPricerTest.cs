using System;

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
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
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
	using Bill = com.opengamma.strata.product.bond.Bill;
	using BillYieldConvention = com.opengamma.strata.product.bond.BillYieldConvention;
	using ResolvedBill = com.opengamma.strata.product.bond.ResolvedBill;

	/// <summary>
	/// Test <seealso cref="DiscountingBillProductPricer"/>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingBillProductPricerTest
	public class DiscountingBillProductPricerTest
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
	  private static readonly ResolvedBill BILL = Bill.builder().dayCount(DAY_COUNT).legalEntityId(ISSUER_ID).notional(NOTIONAL).securityId(SECURITY_ID).settlementDateOffset(DATE_OFFSET).yieldConvention(YIELD_CONVENTION).build().resolve(REF_DATA);
	  private static readonly ResolvedBill BILL_PAST = BILL.toBuilder().notional(BILL.Notional.toBuilder().date(VAL_DATE.minusDays(1)).build()).build();

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
	  private static readonly DiscountingBillProductPricer PRICER = DiscountingBillProductPricer.DEFAULT;
	  private const double EPS = 1.0e-7;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FD_CALC = new RatesFiniteDifferenceSensitivityCalculator(EPS);

	  private const double Z_SPREAD = 0.035;
	  private const double TOLERANCE_PV = 1.0e-6;
	  private const double TOLERANCE_PRICE = 1.0e-10;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		CurrencyAmount pvComputed = PRICER.presentValue(BILL, PROVIDER);
		double pvExpected = DSC_FACTORS_ISSUER.discountFactor(MATURITY_DATE) * NOTIONAL.Amount;
		assertEquals(pvComputed.Currency, EUR);
		assertEquals(pvComputed.Amount, pvExpected, TOLERANCE_PV);
	  }

	  public virtual void test_presentValue_aftermaturity()
	  {
		CurrencyAmount pvComputed = PRICER.presentValue(BILL_PAST, PROVIDER);
		assertEquals(pvComputed.Currency, EUR);
		assertEquals(pvComputed.Amount, 0.0d, TOLERANCE_PV);
	  }
	  public virtual void test_presentValue_zspread()
	  {
		CurrencyAmount pvComputed = PRICER.presentValueWithZSpread(BILL, PROVIDER, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0);
		double pvExpected = DSC_FACTORS_ISSUER.discountFactor(MATURITY_DATE) * NOTIONAL.Amount * Math.Exp(-Z_SPREAD * DSC_FACTORS_ISSUER.relativeYearFraction(MATURITY_DATE));
		assertEquals(pvComputed.Currency, EUR);
		assertEquals(pvComputed.Amount, pvExpected, TOLERANCE_PV);
	  }

	  public virtual void test_presentValue_zspread_aftermaturity()
	  {
		CurrencyAmount pvComputed = PRICER.presentValueWithZSpread(BILL_PAST, PROVIDER, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0);
		assertEquals(pvComputed.Currency, EUR);
		assertEquals(pvComputed.Amount, 0.0d, TOLERANCE_PV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void presentValueSensitivity()
	  {
		PointSensitivities sensiComputed = PRICER.presentValueSensitivity(BILL, PROVIDER);
		PointSensitivities sensiExpected = IssuerCurveDiscountFactors.of(DSC_FACTORS_ISSUER, GROUP_ISSUER).zeroRatePointSensitivity(MATURITY_DATE).multipliedBy(NOTIONAL.Amount).build();
		assertTrue(sensiComputed.equalWithTolerance(sensiExpected, TOLERANCE_PV));
		CurrencyParameterSensitivities paramSensiComputed = PROVIDER.parameterSensitivity(sensiComputed);
		CurrencyParameterSensitivities paramSensiExpected = FD_CALC.sensitivity(PROVIDER, p => PRICER.presentValue(BILL, p));
		assertTrue(paramSensiComputed.equalWithTolerance(paramSensiExpected, EPS * NOTIONAL_AMOUNT));
	  }

	  public virtual void presentValueSensitivity_aftermaturity()
	  {
		PointSensitivities sensiComputed = PRICER.presentValueSensitivity(BILL_PAST, PROVIDER);
		PointSensitivities sensiExpected = PointSensitivities.empty();
		assertTrue(sensiComputed.equalWithTolerance(sensiExpected, TOLERANCE_PV));
	  }

	  public virtual void presentValueSensitivity_zspread()
	  {
		PointSensitivities sensiComputed = PRICER.presentValueSensitivityWithZSpread(BILL, PROVIDER, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0);
		PointSensitivities sensiExpected = IssuerCurveZeroRateSensitivity.of(DSC_FACTORS_ISSUER.zeroRatePointSensitivityWithSpread(MATURITY_DATE, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0), GROUP_ISSUER).multipliedBy(NOTIONAL.Amount).build();
		assertTrue(sensiComputed.equalWithTolerance(sensiExpected, TOLERANCE_PV));
		CurrencyParameterSensitivities paramSensiComputed = PROVIDER.parameterSensitivity(sensiComputed);
		CurrencyParameterSensitivities paramSensiExpected = FD_CALC.sensitivity(PROVIDER, p => PRICER.presentValueWithZSpread(BILL, p, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0));
		assertTrue(paramSensiComputed.equalWithTolerance(paramSensiExpected, EPS * NOTIONAL_AMOUNT));
	  }

	  public virtual void presentValueSensitivity_zspread_aftermaturity()
	  {
		PointSensitivities sensiComputed = PRICER.presentValueSensitivityWithZSpread(BILL_PAST, PROVIDER, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0);
		PointSensitivities sensiExpected = PointSensitivities.empty();
		assertTrue(sensiComputed.equalWithTolerance(sensiExpected, TOLERANCE_PV));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void priceFromCurves()
	  {
		LocalDate settlementDate = VAL_DATE.plusDays(1);
		double priceComputed = PRICER.priceFromCurves(BILL, PROVIDER, settlementDate);
		double dfMaturity = DSC_FACTORS_ISSUER.discountFactor(MATURITY_DATE);
		double dfSettle = DSC_FACTORS_REPO.discountFactor(settlementDate);
		double priceExpected = dfMaturity / dfSettle;
		assertEquals(priceComputed, priceExpected, TOLERANCE_PRICE);
	  }

	  public virtual void price_settle_date_after_maturity_error()
	  {
		assertThrows(() => PRICER.priceFromCurves(BILL, PROVIDER, MATURITY_DATE), typeof(System.ArgumentException));
	  }

	  public virtual void price_settle_date_before_valuation_error()
	  {
		assertThrows(() => PRICER.priceFromCurves(BILL, PROVIDER, VAL_DATE.minusDays(1)), typeof(System.ArgumentException));
	  }

	  public virtual void priceFromCurves_zspread()
	  {
		LocalDate settlementDate = VAL_DATE.plusDays(1);
		double priceComputed = PRICER.priceFromCurvesWithZSpread(BILL, PROVIDER, settlementDate, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0);
		double dfMaturity = DSC_FACTORS_ISSUER.discountFactor(MATURITY_DATE);
		double dfSettle = DSC_FACTORS_REPO.discountFactor(settlementDate);
		double priceExpected = dfMaturity * Math.Exp(-Z_SPREAD * DSC_FACTORS_ISSUER.relativeYearFraction(MATURITY_DATE)) / dfSettle;
		assertEquals(priceComputed, priceExpected, TOLERANCE_PRICE);
	  }

	  public virtual void price_zspread_settle_date_after_maturity_error()
	  {
		assertThrows(() => PRICER.priceFromCurvesWithZSpread(BILL, PROVIDER, MATURITY_DATE, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0), typeof(System.ArgumentException));
	  }

	  public virtual void price_zspread_settle_date_before_valuation_error()
	  {
		assertThrows(() => PRICER.priceFromCurvesWithZSpread(BILL, PROVIDER, VAL_DATE.minusDays(1), Z_SPREAD, CompoundedRateType.CONTINUOUS, 0), typeof(System.ArgumentException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void yieldFromCurves()
	  {
		LocalDate settlementDate = VAL_DATE.plusDays(1);
		double yieldComputed = PRICER.yieldFromCurves(BILL, PROVIDER, settlementDate);
		double dfMaturity = DSC_FACTORS_ISSUER.discountFactor(MATURITY_DATE);
		double dfSettle = DSC_FACTORS_REPO.discountFactor(settlementDate);
		double priceExpected = dfMaturity / dfSettle;
		double yieldExpected = BILL.yieldFromPrice(priceExpected, settlementDate);
		assertEquals(yieldComputed, yieldExpected, TOLERANCE_PRICE);
	  }

	  public virtual void yield_settle_date_after_maturity_error()
	  {
		assertThrows(() => PRICER.yieldFromCurves(BILL, PROVIDER, MATURITY_DATE), typeof(System.ArgumentException));
	  }

	  public virtual void yield_settle_date_before_valuation_error()
	  {
		assertThrows(() => PRICER.yieldFromCurves(BILL, PROVIDER, VAL_DATE.minusDays(1)), typeof(System.ArgumentException));
	  }

	  public virtual void yieldFromCurves_zspread()
	  {
		LocalDate settlementDate = VAL_DATE.plusDays(1);
		double yieldComputed = PRICER.yieldFromCurvesWithZSpread(BILL, PROVIDER, settlementDate, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0);
		double priceExpected = PRICER.priceFromCurvesWithZSpread(BILL, PROVIDER, settlementDate, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0);
		double yieldExpected = BILL.yieldFromPrice(priceExpected, settlementDate);
		assertEquals(yieldComputed, yieldExpected, TOLERANCE_PRICE);
	  }

	  public virtual void yield_zspread_settle_date_after_maturity_error()
	  {
		assertThrows(() => PRICER.yieldFromCurvesWithZSpread(BILL, PROVIDER, MATURITY_DATE, Z_SPREAD, CompoundedRateType.CONTINUOUS, 0), typeof(System.ArgumentException));
	  }

	  public virtual void yield_zspread_settle_date_before_valuation_error()
	  {
		assertThrows(() => PRICER.yieldFromCurvesWithZSpread(BILL, PROVIDER, VAL_DATE.minusDays(1), Z_SPREAD, CompoundedRateType.CONTINUOUS, 0), typeof(System.ArgumentException));
	  }

	}

}