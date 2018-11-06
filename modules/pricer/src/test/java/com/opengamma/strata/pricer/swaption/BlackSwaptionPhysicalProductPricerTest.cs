using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.BUY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.SELL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using ConstantSurface = com.opengamma.strata.market.surface.ConstantSurface;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using RatesProviderDataSets = com.opengamma.strata.pricer.datasets.RatesProviderDataSets;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using LongShort = com.opengamma.strata.product.common.LongShort;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapLegType = com.opengamma.strata.product.swap.SwapLegType;
	using CashSwaptionSettlement = com.opengamma.strata.product.swaption.CashSwaptionSettlement;
	using CashSwaptionSettlementMethod = com.opengamma.strata.product.swaption.CashSwaptionSettlementMethod;
	using PhysicalSwaptionSettlement = com.opengamma.strata.product.swaption.PhysicalSwaptionSettlement;
	using ResolvedSwaption = com.opengamma.strata.product.swaption.ResolvedSwaption;
	using Swaption = com.opengamma.strata.product.swaption.Swaption;
	using SwaptionSettlement = com.opengamma.strata.product.swaption.SwaptionSettlement;

	/// <summary>
	/// Tests <seealso cref="BlackSwaptionPhysicalProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BlackSwaptionPhysicalProductPricerTest
	public class BlackSwaptionPhysicalProductPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = date(2015, 8, 7);
	  private static readonly LocalDate SWAPTION_EXERCISE_DATE = VAL_DATE.plusYears(5);
	  private static readonly LocalDate SWAPTION_PAST_EXERCISE_DATE = VAL_DATE.minusYears(1);
	  private static readonly LocalTime SWAPTION_EXPIRY_TIME = LocalTime.of(11, 0);
	  private static readonly ZoneId SWAPTION_EXPIRY_ZONE = ZoneId.of("America/New_York");
	  private static readonly LocalDate SWAP_EFFECTIVE_DATE = USD_LIBOR_3M.calculateEffectiveFromFixing(SWAPTION_EXERCISE_DATE, REF_DATA);
	  private const int SWAP_TENOR_YEAR = 5;
	  private static readonly Period SWAP_TENOR = Period.ofYears(SWAP_TENOR_YEAR);
	  private static readonly LocalDate SWAP_MATURITY_DATE = SWAP_EFFECTIVE_DATE.plus(SWAP_TENOR);
	  private const double STRIKE = 0.01;
	  private const double NOTIONAL = 100_000_000;
	  private static readonly Swap SWAP_REC = USD_FIXED_6M_LIBOR_3M.toTrade(VAL_DATE, SWAP_EFFECTIVE_DATE, SWAP_MATURITY_DATE, SELL, NOTIONAL, STRIKE).Product;
	  private static readonly ResolvedSwap RSWAP_REC = SWAP_REC.resolve(REF_DATA);
	  private static readonly Swap SWAP_PAY = USD_FIXED_6M_LIBOR_3M.toTrade(VAL_DATE, SWAP_EFFECTIVE_DATE, SWAP_MATURITY_DATE, BUY, NOTIONAL, STRIKE).Product;
	  private static readonly ResolvedSwap RSWAP_PAY = SWAP_PAY.resolve(REF_DATA);
	  private static readonly Swap SWAP_PAST = USD_FIXED_6M_LIBOR_3M.toTrade(SWAPTION_PAST_EXERCISE_DATE, SWAPTION_PAST_EXERCISE_DATE, SWAPTION_PAST_EXERCISE_DATE.plusYears(10), BUY, NOTIONAL, STRIKE).Product;
	  private static readonly SwaptionSettlement PHYSICAL_SETTLE = PhysicalSwaptionSettlement.DEFAULT;
	  private static readonly SwaptionSettlement CASH_SETTLE = CashSwaptionSettlement.of(SWAP_REC.StartDate.Unadjusted, CashSwaptionSettlementMethod.PAR_YIELD);

	  private static readonly ResolvedSwaption SWAPTION_LONG_REC = Swaption.builder().swaptionSettlement(PHYSICAL_SETTLE).expiryDate(AdjustableDate.of(SWAPTION_EXERCISE_DATE)).expiryTime(SWAPTION_EXPIRY_TIME).expiryZone(SWAPTION_EXPIRY_ZONE).longShort(LongShort.LONG).underlying(SWAP_REC).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_SHORT_REC = Swaption.builder().swaptionSettlement(PHYSICAL_SETTLE).expiryDate(AdjustableDate.of(SWAPTION_EXERCISE_DATE)).expiryTime(SWAPTION_EXPIRY_TIME).expiryZone(SWAPTION_EXPIRY_ZONE).longShort(LongShort.SHORT).underlying(SWAP_REC).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_LONG_PAY = Swaption.builder().swaptionSettlement(PHYSICAL_SETTLE).expiryDate(AdjustableDate.of(SWAPTION_EXERCISE_DATE)).expiryTime(SWAPTION_EXPIRY_TIME).expiryZone(SWAPTION_EXPIRY_ZONE).longShort(LongShort.LONG).underlying(SWAP_PAY).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_LONG_REC_CASH = Swaption.builder().swaptionSettlement(CASH_SETTLE).expiryDate(AdjustableDate.of(SWAPTION_EXERCISE_DATE)).expiryTime(SWAPTION_EXPIRY_TIME).expiryZone(SWAPTION_EXPIRY_ZONE).longShort(LongShort.LONG).underlying(SWAP_REC).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_REC_AT_EXPIRY = Swaption.builder().swaptionSettlement(PHYSICAL_SETTLE).expiryDate(AdjustableDate.of(VAL_DATE)).expiryTime(SWAPTION_EXPIRY_TIME).expiryZone(SWAPTION_EXPIRY_ZONE).longShort(LongShort.LONG).underlying(SWAP_REC).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_PAY_AT_EXPIRY = Swaption.builder().swaptionSettlement(PHYSICAL_SETTLE).expiryDate(AdjustableDate.of(VAL_DATE)).expiryTime(SWAPTION_EXPIRY_TIME).expiryZone(SWAPTION_EXPIRY_ZONE).longShort(LongShort.LONG).underlying(SWAP_PAY).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_PAST = Swaption.builder().swaptionSettlement(PHYSICAL_SETTLE).expiryDate(AdjustableDate.of(SWAPTION_PAST_EXERCISE_DATE)).expiryTime(SWAPTION_EXPIRY_TIME).expiryZone(SWAPTION_EXPIRY_ZONE).longShort(LongShort.LONG).underlying(SWAP_PAST).build().resolve(REF_DATA);

	  private static readonly BlackSwaptionPhysicalProductPricer PRICER_SWAPTION_BLACK = BlackSwaptionPhysicalProductPricer.DEFAULT;
	  private static readonly DiscountingSwapProductPricer PRICER_SWAP = DiscountingSwapProductPricer.DEFAULT;
	  private const double FD_SHIFT = 0.5E-8;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FINITE_DIFFERENCE_CALCULATOR = new RatesFiniteDifferenceSensitivityCalculator(FD_SHIFT);

	  private static readonly ImmutableRatesProvider MULTI_USD = RatesProviderDataSets.multiUsd(VAL_DATE);
	  private static readonly BlackSwaptionExpiryTenorVolatilities BLACK_VOLS_CST_USD = SwaptionBlackVolatilityDataSets.BLACK_SWAPTION_VOLS_CST_USD;
	  private static readonly BlackSwaptionExpiryTenorVolatilities BLACK_VOLS_USD_STD = SwaptionBlackVolatilityDataSets.BLACK_SWAPTION_VOLS_USD_STD;

	  private const double TOLERANCE_PV = 1.0E-2;
	  private const double TOLERANCE_PV_DELTA = 1.0E+2;
	  private const double TOLERANCE_PV_VEGA = 1.0E+4;
	  private const double TOLERANCE_RATE = 1.0E-8;

	  //-------------------------------------------------------------------------
	  public virtual void validate_physical_settlement()
	  {
		assertThrowsIllegalArg(() => PRICER_SWAPTION_BLACK.presentValue(SWAPTION_LONG_REC_CASH, MULTI_USD, BLACK_VOLS_USD_STD));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_implied_volatility()
	  {
		double forward = PRICER_SWAP.parRate(RSWAP_REC, MULTI_USD);
		double volExpected = BLACK_VOLS_USD_STD.volatility(SWAPTION_LONG_REC.Expiry, SWAP_TENOR_YEAR, STRIKE, forward);
		double volComputed = PRICER_SWAPTION_BLACK.impliedVolatility(SWAPTION_LONG_PAY, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(volComputed, volExpected, TOLERANCE_RATE);
	  }

	  public virtual void test_implied_volatility_after_expiry()
	  {
		assertThrowsIllegalArg(() => PRICER_SWAPTION_BLACK.impliedVolatility(SWAPTION_PAST, MULTI_USD, BLACK_VOLS_USD_STD));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void present_value_formula()
	  {
		double forward = PRICER_SWAP.parRate(RSWAP_REC, MULTI_USD);
		double pvbp = PRICER_SWAP.LegPricer.pvbp(RSWAP_REC.getLegs(SwapLegType.FIXED).get(0), MULTI_USD);
		double volatility = BLACK_VOLS_USD_STD.volatility(SWAPTION_LONG_REC.Expiry, SWAP_TENOR_YEAR, STRIKE, forward);
		double expiry = BLACK_VOLS_USD_STD.relativeTime(SWAPTION_LONG_REC.Expiry);
		double pvExpected = Math.Abs(pvbp) * BlackFormulaRepository.price(forward, STRIKE, expiry, volatility, false);
		CurrencyAmount pvComputed = PRICER_SWAPTION_BLACK.presentValue(SWAPTION_LONG_REC, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pvComputed.Currency, USD);
		assertEquals(pvComputed.Amount, pvExpected, TOLERANCE_PV);
	  }

	  public virtual void present_value_long_short_parity()
	  {
		CurrencyAmount pvLong = PRICER_SWAPTION_BLACK.presentValue(SWAPTION_LONG_REC, MULTI_USD, BLACK_VOLS_USD_STD);
		CurrencyAmount pvShort = PRICER_SWAPTION_BLACK.presentValue(SWAPTION_SHORT_REC, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pvLong.Amount, -pvShort.Amount, TOLERANCE_PV);
	  }

	  public virtual void present_value_payer_receiver_parity()
	  {
		CurrencyAmount pvLongPay = PRICER_SWAPTION_BLACK.presentValue(SWAPTION_LONG_PAY, MULTI_USD, BLACK_VOLS_USD_STD);
		CurrencyAmount pvShortRec = PRICER_SWAPTION_BLACK.presentValue(SWAPTION_SHORT_REC, MULTI_USD, BLACK_VOLS_USD_STD);
		MultiCurrencyAmount pvSwapPay = PRICER_SWAP.presentValue(RSWAP_PAY, MULTI_USD);
		assertEquals(pvLongPay.Amount + pvShortRec.Amount, pvSwapPay.getAmount(USD).Amount, TOLERANCE_PV);
	  }

	  public virtual void present_value_at_expiry()
	  {
		CurrencyAmount pvRec = PRICER_SWAPTION_BLACK.presentValue(SWAPTION_REC_AT_EXPIRY, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pvRec.Amount, 0.0d, TOLERANCE_PV);
		CurrencyAmount pvPay = PRICER_SWAPTION_BLACK.presentValue(SWAPTION_PAY_AT_EXPIRY, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pvPay.Amount, PRICER_SWAP.presentValue(RSWAP_PAY, MULTI_USD).getAmount(USD).Amount, TOLERANCE_PV);
	  }

	  public virtual void present_value_after_expiry()
	  {
		CurrencyAmount pv = PRICER_SWAPTION_BLACK.presentValue(SWAPTION_PAST, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pv.Amount, 0.0d, TOLERANCE_PV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void present_value_delta_formula()
	  {
		double forward = PRICER_SWAP.parRate(RSWAP_REC, MULTI_USD);
		double pvbp = PRICER_SWAP.LegPricer.pvbp(RSWAP_REC.getLegs(SwapLegType.FIXED).get(0), MULTI_USD);
		double volatility = BLACK_VOLS_USD_STD.volatility(SWAPTION_LONG_REC.Expiry, SWAP_TENOR_YEAR, STRIKE, forward);
		double expiry = BLACK_VOLS_USD_STD.relativeTime(SWAPTION_LONG_REC.Expiry);
		double pvDeltaExpected = BlackFormulaRepository.delta(forward, STRIKE, expiry, volatility, false) * Math.Abs(pvbp);
		CurrencyAmount pvDeltaComputed = PRICER_SWAPTION_BLACK.presentValueDelta(SWAPTION_LONG_REC, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pvDeltaComputed.Currency, USD);
		assertEquals(pvDeltaComputed.Amount, pvDeltaExpected, TOLERANCE_PV);
	  }

	  public virtual void present_value_delta_long_short_parity()
	  {
		CurrencyAmount pvDeltaLong = PRICER_SWAPTION_BLACK.presentValueDelta(SWAPTION_LONG_REC, MULTI_USD, BLACK_VOLS_USD_STD);
		CurrencyAmount pvDeltaShort = PRICER_SWAPTION_BLACK.presentValueDelta(SWAPTION_SHORT_REC, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pvDeltaLong.Amount, -pvDeltaShort.Amount, TOLERANCE_PV);
	  }

	  public virtual void present_value_delta_payer_receiver_parity()
	  {
		CurrencyAmount pvDeltaLongPay = PRICER_SWAPTION_BLACK.presentValueDelta(SWAPTION_LONG_PAY, MULTI_USD, BLACK_VOLS_USD_STD);
		CurrencyAmount pvDeltaShortRec = PRICER_SWAPTION_BLACK.presentValueDelta(SWAPTION_SHORT_REC, MULTI_USD, BLACK_VOLS_USD_STD);
		double pvbp = PRICER_SWAP.LegPricer.pvbp(RSWAP_PAY.getLegs(SwapLegType.FIXED).get(0), MULTI_USD);
		assertEquals(pvDeltaLongPay.Amount + pvDeltaShortRec.Amount, Math.Abs(pvbp), TOLERANCE_PV);
	  }

	  public virtual void present_value_delta_at_expiry()
	  {
		CurrencyAmount pvDeltaRec = PRICER_SWAPTION_BLACK.presentValueDelta(SWAPTION_REC_AT_EXPIRY, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pvDeltaRec.Amount, 0d, TOLERANCE_PV);
		CurrencyAmount pvDeltaPay = PRICER_SWAPTION_BLACK.presentValueDelta(SWAPTION_PAY_AT_EXPIRY, MULTI_USD, BLACK_VOLS_USD_STD);
		double pvbp = PRICER_SWAP.LegPricer.pvbp(RSWAP_PAY.getLegs(SwapLegType.FIXED).get(0), MULTI_USD);
		assertEquals(pvDeltaPay.Amount, Math.Abs(pvbp), TOLERANCE_PV);
	  }

	  public virtual void present_value_delta_after_expiry()
	  {
		CurrencyAmount pvDelta = PRICER_SWAPTION_BLACK.presentValueDelta(SWAPTION_PAST, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pvDelta.Amount, 0d, TOLERANCE_PV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void present_value_gamma_formula()
	  {
		double forward = PRICER_SWAP.parRate(RSWAP_REC, MULTI_USD);
		double pvbp = PRICER_SWAP.LegPricer.pvbp(RSWAP_REC.getLegs(SwapLegType.FIXED).get(0), MULTI_USD);
		double volatility = BLACK_VOLS_USD_STD.volatility(SWAPTION_LONG_REC.Expiry, SWAP_TENOR_YEAR, STRIKE, forward);
		double expiry = BLACK_VOLS_USD_STD.relativeTime(SWAPTION_LONG_REC.Expiry);
		double pvGammaExpected = BlackFormulaRepository.gamma(forward, STRIKE, expiry, volatility) * Math.Abs(pvbp);
		CurrencyAmount pvGammaComputed = PRICER_SWAPTION_BLACK.presentValueGamma(SWAPTION_LONG_REC, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pvGammaComputed.Currency, USD);
		assertEquals(pvGammaComputed.Amount, pvGammaExpected, TOLERANCE_PV);
	  }

	  public virtual void present_value_gamma_long_short_parity()
	  {
		CurrencyAmount pvGammaLong = PRICER_SWAPTION_BLACK.presentValueGamma(SWAPTION_LONG_REC, MULTI_USD, BLACK_VOLS_USD_STD);
		CurrencyAmount pvGammaShort = PRICER_SWAPTION_BLACK.presentValueGamma(SWAPTION_SHORT_REC, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pvGammaLong.Amount, -pvGammaShort.Amount, TOLERANCE_PV);
	  }

	  public virtual void present_value_gamma_payer_receiver_parity()
	  {
		CurrencyAmount pvGammaLongPay = PRICER_SWAPTION_BLACK.presentValueGamma(SWAPTION_LONG_PAY, MULTI_USD, BLACK_VOLS_USD_STD);
		CurrencyAmount pvGammaShortRec = PRICER_SWAPTION_BLACK.presentValueGamma(SWAPTION_SHORT_REC, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pvGammaLongPay.Amount + pvGammaShortRec.Amount, 0d, TOLERANCE_PV);
	  }

	  public virtual void present_value_gamma_at_expiry()
	  {
		CurrencyAmount pvGammaRec = PRICER_SWAPTION_BLACK.presentValueGamma(SWAPTION_REC_AT_EXPIRY, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pvGammaRec.Amount, 0d, TOLERANCE_PV);
		CurrencyAmount pvGammaPay = PRICER_SWAPTION_BLACK.presentValueGamma(SWAPTION_PAY_AT_EXPIRY, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pvGammaPay.Amount, 0d, TOLERANCE_PV);
	  }

	  public virtual void present_value_gamma_after_expiry()
	  {
		CurrencyAmount pvGamma = PRICER_SWAPTION_BLACK.presentValueGamma(SWAPTION_PAST, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pvGamma.Amount, 0d, TOLERANCE_PV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void present_value_theta_formula()
	  {
		double forward = PRICER_SWAP.parRate(RSWAP_REC, MULTI_USD);
		double pvbp = PRICER_SWAP.LegPricer.pvbp(RSWAP_REC.getLegs(SwapLegType.FIXED).get(0), MULTI_USD);
		double volatility = BLACK_VOLS_USD_STD.volatility(SWAPTION_LONG_REC.Expiry, SWAP_TENOR_YEAR, STRIKE, forward);
		double expiry = BLACK_VOLS_USD_STD.relativeTime(SWAPTION_LONG_REC.Expiry);
		double pvThetaExpected = BlackFormulaRepository.driftlessTheta(forward, STRIKE, expiry, volatility) * Math.Abs(pvbp);
		CurrencyAmount pvThetaComputed = PRICER_SWAPTION_BLACK.presentValueTheta(SWAPTION_LONG_REC, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pvThetaComputed.Currency, USD);
		assertEquals(pvThetaComputed.Amount, pvThetaExpected, TOLERANCE_PV);
	  }

	  public virtual void present_value_theta_long_short_parity()
	  {
		CurrencyAmount pvThetaLong = PRICER_SWAPTION_BLACK.presentValueTheta(SWAPTION_LONG_REC, MULTI_USD, BLACK_VOLS_USD_STD);
		CurrencyAmount pvThetaShort = PRICER_SWAPTION_BLACK.presentValueTheta(SWAPTION_SHORT_REC, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pvThetaLong.Amount, -pvThetaShort.Amount, TOLERANCE_PV);
	  }

	  public virtual void present_value_theta_payer_receiver_parity()
	  {
		CurrencyAmount pvThetaLongPay = PRICER_SWAPTION_BLACK.presentValueTheta(SWAPTION_LONG_PAY, MULTI_USD, BLACK_VOLS_USD_STD);
		CurrencyAmount pvThetaShortRec = PRICER_SWAPTION_BLACK.presentValueTheta(SWAPTION_SHORT_REC, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pvThetaLongPay.Amount + pvThetaShortRec.Amount, 0d, TOLERANCE_PV);
	  }

	  public virtual void present_value_theta_at_expiry()
	  {
		CurrencyAmount pvThetaRec = PRICER_SWAPTION_BLACK.presentValueTheta(SWAPTION_REC_AT_EXPIRY, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pvThetaRec.Amount, 0d, TOLERANCE_PV);
		CurrencyAmount pvThetaPay = PRICER_SWAPTION_BLACK.presentValueTheta(SWAPTION_PAY_AT_EXPIRY, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pvThetaPay.Amount, 0d, TOLERANCE_PV);
	  }

	  public virtual void present_value_theta_after_expiry()
	  {
		CurrencyAmount pvTheta = PRICER_SWAPTION_BLACK.presentValueTheta(SWAPTION_PAST, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pvTheta.Amount, 0d, TOLERANCE_PV);
	  }

	  //-------------------------------------------------------------------------  
	  public virtual void currency_exposure()
	  {
		CurrencyAmount pv = PRICER_SWAPTION_BLACK.presentValue(SWAPTION_LONG_PAY, MULTI_USD, BLACK_VOLS_USD_STD);
		MultiCurrencyAmount ce = PRICER_SWAPTION_BLACK.currencyExposure(SWAPTION_LONG_PAY, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pv.Amount, ce.getAmount(USD).Amount, TOLERANCE_PV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void present_value_sensitivity_FD()
	  {
		PointSensitivities pvpt = PRICER_SWAPTION_BLACK.presentValueSensitivityRatesStickyStrike(SWAPTION_SHORT_REC, MULTI_USD, BLACK_VOLS_CST_USD).build();
		CurrencyParameterSensitivities pvpsAd = MULTI_USD.parameterSensitivity(pvpt);
		CurrencyParameterSensitivities pvpsFd = FINITE_DIFFERENCE_CALCULATOR.sensitivity(MULTI_USD, (p) => PRICER_SWAPTION_BLACK.presentValue(SWAPTION_SHORT_REC, p, BLACK_VOLS_CST_USD));
		assertTrue(pvpsAd.equalWithTolerance(pvpsFd, TOLERANCE_PV_DELTA));
	  }

	  public virtual void present_value_sensitivity_long_short_parity()
	  {
		PointSensitivities pvptLong = PRICER_SWAPTION_BLACK.presentValueSensitivityRatesStickyStrike(SWAPTION_LONG_REC, MULTI_USD, BLACK_VOLS_USD_STD).build();
		PointSensitivities pvptShort = PRICER_SWAPTION_BLACK.presentValueSensitivityRatesStickyStrike(SWAPTION_SHORT_REC, MULTI_USD, BLACK_VOLS_USD_STD).build();
		CurrencyParameterSensitivities pvpsLong = MULTI_USD.parameterSensitivity(pvptLong);
		CurrencyParameterSensitivities pvpsShort = MULTI_USD.parameterSensitivity(pvptShort);
		assertTrue(pvpsLong.equalWithTolerance(pvpsShort.multipliedBy(-1.0), TOLERANCE_PV_DELTA));
	  }

	  public virtual void present_value_sensitivity_payer_receiver_parity()
	  {
		PointSensitivities pvptLongPay = PRICER_SWAPTION_BLACK.presentValueSensitivityRatesStickyStrike(SWAPTION_LONG_PAY, MULTI_USD, BLACK_VOLS_USD_STD).build();
		PointSensitivities pvptShortRec = PRICER_SWAPTION_BLACK.presentValueSensitivityRatesStickyStrike(SWAPTION_SHORT_REC, MULTI_USD, BLACK_VOLS_USD_STD).build();
		PointSensitivities pvptSwapRec = PRICER_SWAP.presentValueSensitivity(RSWAP_PAY, MULTI_USD).build();
		CurrencyParameterSensitivities pvpsLongPay = MULTI_USD.parameterSensitivity(pvptLongPay);
		CurrencyParameterSensitivities pvpsShortRec = MULTI_USD.parameterSensitivity(pvptShortRec);
		CurrencyParameterSensitivities pvpsSwapRec = MULTI_USD.parameterSensitivity(pvptSwapRec);
		assertTrue(pvpsLongPay.combinedWith(pvpsShortRec).equalWithTolerance(pvpsSwapRec, TOLERANCE_PV_DELTA));
	  }

	  public virtual void present_value_sensitivity_at_expiry()
	  {
		PointSensitivities sensiRec = PRICER_SWAPTION_BLACK.presentValueSensitivityRatesStickyStrike(SWAPTION_REC_AT_EXPIRY, MULTI_USD, BLACK_VOLS_USD_STD).build();
		foreach (PointSensitivity sensi in sensiRec.Sensitivities)
		{
		  assertEquals(Math.Abs(sensi.Sensitivity), 0d);
		}
		PointSensitivities sensiPay = PRICER_SWAPTION_BLACK.presentValueSensitivityRatesStickyStrike(SWAPTION_PAY_AT_EXPIRY, MULTI_USD, BLACK_VOLS_USD_STD).build();
		PointSensitivities sensiPaySwap = PRICER_SWAP.presentValueSensitivity(RSWAP_PAY, MULTI_USD).build();
		assertTrue(MULTI_USD.parameterSensitivity(sensiPay).equalWithTolerance(MULTI_USD.parameterSensitivity(sensiPaySwap), TOLERANCE_PV));
	  }

	  public virtual void present_value_sensitivity_after_expiry()
	  {
		PointSensitivityBuilder pvpts = PRICER_SWAPTION_BLACK.presentValueSensitivityRatesStickyStrike(SWAPTION_PAST, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pvpts, PointSensitivityBuilder.none());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void present_value_sensitivityBlackVolatility_FD()
	  {
		double shiftVol = 1.0E-4;
		Surface surfaceUp = ConstantSurface.of(SwaptionBlackVolatilityDataSets.META_DATA, SwaptionBlackVolatilityDataSets.VOLATILITY + shiftVol);
		Surface surfaceDw = ConstantSurface.of(SwaptionBlackVolatilityDataSets.META_DATA, SwaptionBlackVolatilityDataSets.VOLATILITY - shiftVol);
		CurrencyAmount pvP = PRICER_SWAPTION_BLACK.presentValue(SWAPTION_LONG_PAY, MULTI_USD, BlackSwaptionExpiryTenorVolatilities.of(BLACK_VOLS_CST_USD.Convention, VAL_DATE.atStartOfDay(ZoneOffset.UTC), surfaceUp));
		CurrencyAmount pvM = PRICER_SWAPTION_BLACK.presentValue(SWAPTION_LONG_PAY, MULTI_USD, BlackSwaptionExpiryTenorVolatilities.of(BLACK_VOLS_CST_USD.Convention, VAL_DATE.atStartOfDay(ZoneOffset.UTC), surfaceDw));
		double pvnvsFd = (pvP.Amount - pvM.Amount) / (2 * shiftVol);
		SwaptionSensitivity pvnvsAd = PRICER_SWAPTION_BLACK.presentValueSensitivityModelParamsVolatility(SWAPTION_LONG_PAY, MULTI_USD, BLACK_VOLS_CST_USD);
		assertEquals(pvnvsAd.Currency, USD);
		assertEquals(pvnvsAd.Sensitivity, pvnvsFd, TOLERANCE_PV_VEGA);
		assertEquals(pvnvsAd.VolatilitiesName, BLACK_VOLS_CST_USD.Name);
		assertEquals(pvnvsAd.Expiry, BLACK_VOLS_CST_USD.relativeTime(SWAPTION_LONG_PAY.Expiry));
		assertEquals(pvnvsAd.Tenor, SWAP_TENOR_YEAR, TOLERANCE_RATE);
		assertEquals(pvnvsAd.Strike, STRIKE, TOLERANCE_RATE);
		double forward = PRICER_SWAP.parRate(RSWAP_REC, MULTI_USD);
		assertEquals(pvnvsAd.Forward, forward, TOLERANCE_RATE);
	  }

	  public virtual void present_value_sensitivityBlackVolatility_long_short_parity()
	  {
		SwaptionSensitivity pvptLongPay = PRICER_SWAPTION_BLACK.presentValueSensitivityModelParamsVolatility(SWAPTION_LONG_REC, MULTI_USD, BLACK_VOLS_USD_STD);
		SwaptionSensitivity pvptShortRec = PRICER_SWAPTION_BLACK.presentValueSensitivityModelParamsVolatility(SWAPTION_SHORT_REC, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pvptLongPay.Sensitivity, -pvptShortRec.Sensitivity, TOLERANCE_PV_VEGA);
	  }

	  public virtual void present_value_sensitivityBlackVolatility_payer_receiver_parity()
	  {
		SwaptionSensitivity pvptLongPay = PRICER_SWAPTION_BLACK.presentValueSensitivityModelParamsVolatility(SWAPTION_LONG_PAY, MULTI_USD, BLACK_VOLS_USD_STD);
		SwaptionSensitivity pvptShortRec = PRICER_SWAPTION_BLACK.presentValueSensitivityModelParamsVolatility(SWAPTION_SHORT_REC, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(pvptLongPay.Sensitivity + pvptShortRec.Sensitivity, 0, TOLERANCE_PV_VEGA);
	  }

	  public virtual void present_value_sensitivityBlackVolatility_at_expiry()
	  {
		SwaptionSensitivity sensiRec = PRICER_SWAPTION_BLACK.presentValueSensitivityModelParamsVolatility(SWAPTION_REC_AT_EXPIRY, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(sensiRec.Sensitivity, 0d, TOLERANCE_PV);
		SwaptionSensitivity sensiPay = PRICER_SWAPTION_BLACK.presentValueSensitivityModelParamsVolatility(SWAPTION_PAY_AT_EXPIRY, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(sensiPay.Sensitivity, 0d, TOLERANCE_PV);
	  }

	  public virtual void present_value_sensitivityBlackVolatility_after_expiry()
	  {
		SwaptionSensitivity v = PRICER_SWAPTION_BLACK.presentValueSensitivityModelParamsVolatility(SWAPTION_PAST, MULTI_USD, BLACK_VOLS_USD_STD);
		assertEquals(v.Sensitivity, 0.0d, TOLERANCE_PV_VEGA);
	  }

	}

}