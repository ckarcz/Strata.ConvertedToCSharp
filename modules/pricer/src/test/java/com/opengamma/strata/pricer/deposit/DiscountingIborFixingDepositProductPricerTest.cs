/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.deposit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using ImmutableRatesProviderSimpleData = com.opengamma.strata.pricer.datasets.ImmutableRatesProviderSimpleData;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using IborFixingDeposit = com.opengamma.strata.product.deposit.IborFixingDeposit;
	using ResolvedIborFixingDeposit = com.opengamma.strata.product.deposit.ResolvedIborFixingDeposit;

	/// <summary>
	/// Test <seealso cref="DiscountingIborFixingDepositProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingIborFixingDepositProductPricerTest
	public class DiscountingIborFixingDepositProductPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = ImmutableRatesProviderSimpleData.VAL_DATE;
	  private static readonly LocalDate START_DATE = EUR_EURIBOR_6M.calculateEffectiveFromFixing(VAL_DATE, REF_DATA);
	  private static readonly LocalDate END_DATE = EUR_EURIBOR_6M.calculateMaturityFromEffective(START_DATE, REF_DATA);
	  private const double NOTIONAL = 100000000d;
	  private const double RATE = 0.0150;
	  private static readonly BusinessDayAdjustment BD_ADJ = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA);
	  private static readonly IborFixingDeposit DEPOSIT = IborFixingDeposit.builder().buySell(BuySell.BUY).notional(NOTIONAL).startDate(START_DATE).endDate(END_DATE).businessDayAdjustment(BD_ADJ).index(EUR_EURIBOR_6M).fixedRate(RATE).build();
	  private static readonly ResolvedIborFixingDeposit RDEPOSIT = DEPOSIT.resolve(REF_DATA);
	  private const double TOLERANCE_PV = 1E-2;
	  private const double TOLERANCE_PV_DELTA = 1E-2;
	  private const double TOLERANCE_RATE = 1E-8;
	  private const double TOLERANCE_RATE_DELTA = 1E-6;

	  private const double EPS_FD = 1E-7;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator CAL_FD = new RatesFiniteDifferenceSensitivityCalculator(EPS_FD);
	  private static readonly ImmutableRatesProvider IMM_PROV_NOFIX = ImmutableRatesProviderSimpleData.IMM_PROV_EUR_NOFIX;
	  private static readonly ImmutableRatesProvider IMM_PROV_FIX = ImmutableRatesProviderSimpleData.IMM_PROV_EUR_FIX;

	  private static readonly DiscountingIborFixingDepositProductPricer PRICER = DiscountingIborFixingDepositProductPricer.DEFAULT;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue_noFixing()
	  {
		double discountFactor = IMM_PROV_NOFIX.discountFactor(EUR, END_DATE);
		double forwardRate = IMM_PROV_NOFIX.iborIndexRates(EUR_EURIBOR_6M).rate(RDEPOSIT.FloatingRate.Observation);
		CurrencyAmount computed = PRICER.presentValue(RDEPOSIT, IMM_PROV_NOFIX);
		double expected = NOTIONAL * discountFactor * (RATE - forwardRate) * RDEPOSIT.YearFraction;
		assertEquals(computed.Currency, EUR);
		assertEquals(computed.Amount, expected, TOLERANCE_PV);
	  }

	  public virtual void test_presentValue_fixing()
	  {
		CurrencyAmount computedNoFix = PRICER.presentValue(RDEPOSIT, IMM_PROV_NOFIX);
		CurrencyAmount computedFix = PRICER.presentValue(RDEPOSIT, IMM_PROV_FIX); // Fixing should not be taken into account
		assertEquals(computedFix.Currency, EUR);
		assertEquals(computedFix.Amount, computedNoFix.Amount, TOLERANCE_PV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity_noFixing()
	  {
		PointSensitivities computed = PRICER.presentValueSensitivity(RDEPOSIT, IMM_PROV_NOFIX);
		CurrencyParameterSensitivities sensiComputed = IMM_PROV_NOFIX.parameterSensitivity(computed);
		CurrencyParameterSensitivities sensiExpected = CAL_FD.sensitivity(IMM_PROV_NOFIX, (p) => PRICER.presentValue(RDEPOSIT, (p)));
		assertTrue(sensiComputed.equalWithTolerance(sensiExpected, NOTIONAL * EPS_FD));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity_fixing()
	  {
		PointSensitivities computedNoFix = PRICER.presentValueSensitivity(RDEPOSIT, IMM_PROV_NOFIX);
		CurrencyParameterSensitivities sensiComputedNoFix = IMM_PROV_NOFIX.parameterSensitivity(computedNoFix);
		PointSensitivities computedFix = PRICER.presentValueSensitivity(RDEPOSIT, IMM_PROV_FIX);
		CurrencyParameterSensitivities sensiComputedFix = IMM_PROV_NOFIX.parameterSensitivity(computedFix);
		assertTrue(sensiComputedNoFix.equalWithTolerance(sensiComputedFix, TOLERANCE_PV_DELTA));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parRate()
	  {
		double parRate = PRICER.parRate(RDEPOSIT, IMM_PROV_NOFIX);
		IborFixingDeposit deposit0 = DEPOSIT.toBuilder().fixedRate(parRate).build();
		CurrencyAmount pv0 = PRICER.presentValue(deposit0.resolve(REF_DATA), IMM_PROV_NOFIX);
		assertEquals(pv0.Amount, 0, TOLERANCE_RATE);
		double parRate2 = PRICER.parRate(RDEPOSIT, IMM_PROV_NOFIX);
		assertEquals(parRate, parRate2, TOLERANCE_RATE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parSpread_noFixing()
	  {
		double parSpread = PRICER.parSpread(RDEPOSIT, IMM_PROV_NOFIX);
		IborFixingDeposit deposit0 = DEPOSIT.toBuilder().fixedRate(RATE + parSpread).build();
		CurrencyAmount pv0 = PRICER.presentValue(deposit0.resolve(REF_DATA), IMM_PROV_NOFIX);
		assertEquals(pv0.Amount, 0, TOLERANCE_RATE);
		double parSpread2 = PRICER.parSpread(RDEPOSIT, IMM_PROV_NOFIX);
		assertEquals(parSpread, parSpread2, TOLERANCE_RATE);
	  }

	  public virtual void test_parSpread_fixing()
	  {
		double parSpread1 = PRICER.parSpread(RDEPOSIT, IMM_PROV_FIX);
		double parSpread2 = PRICER.parSpread(RDEPOSIT, IMM_PROV_NOFIX);
		assertEquals(parSpread1, parSpread2, TOLERANCE_RATE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parSpreadSensitivity_noFixing()
	  {
		PointSensitivities computedNoFix = PRICER.parSpreadSensitivity(RDEPOSIT, IMM_PROV_NOFIX);
		CurrencyParameterSensitivities sensiComputedNoFix = IMM_PROV_NOFIX.parameterSensitivity(computedNoFix);
		CurrencyParameterSensitivities sensiExpected = CAL_FD.sensitivity(IMM_PROV_NOFIX, (p) => CurrencyAmount.of(EUR, PRICER.parSpread(RDEPOSIT, (p))));
		assertTrue(sensiComputedNoFix.equalWithTolerance(sensiExpected, TOLERANCE_RATE_DELTA));
		// Par rate and par spread sensitivities are equal
		PointSensitivities computedParRateNoFix = PRICER.parRateSensitivity(RDEPOSIT, IMM_PROV_NOFIX);
		CurrencyParameterSensitivities sensiComputedParRateNoFix = IMM_PROV_NOFIX.parameterSensitivity(computedParRateNoFix);
		assertTrue(sensiComputedNoFix.equalWithTolerance(sensiComputedParRateNoFix, TOLERANCE_RATE_DELTA));
		PointSensitivities computedFix = PRICER.parSpreadSensitivity(RDEPOSIT, IMM_PROV_FIX);
		CurrencyParameterSensitivities sensiComputedFix = IMM_PROV_NOFIX.parameterSensitivity(computedFix);
		assertTrue(sensiComputedFix.equalWithTolerance(sensiExpected, TOLERANCE_RATE_DELTA));
	  }

	  public virtual void test_parSpreadSensitivity_fixing()
	  {
		PointSensitivities computedNoFix = PRICER.parSpreadSensitivity(RDEPOSIT, IMM_PROV_NOFIX);
		PointSensitivities computedFix = PRICER.parSpreadSensitivity(RDEPOSIT, IMM_PROV_FIX);
		assertTrue(computedNoFix.equalWithTolerance(computedFix, TOLERANCE_PV_DELTA));
		PointSensitivities computedParRateFix = PRICER.parRateSensitivity(RDEPOSIT, IMM_PROV_FIX);
		assertTrue(computedParRateFix.equalWithTolerance(computedFix, TOLERANCE_PV_DELTA));
	  }

	}

}