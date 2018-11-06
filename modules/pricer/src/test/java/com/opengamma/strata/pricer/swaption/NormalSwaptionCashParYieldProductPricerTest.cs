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
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
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
	using RatesProviderDataSets = com.opengamma.strata.pricer.datasets.RatesProviderDataSets;
	using EuropeanVanillaOption = com.opengamma.strata.pricer.impl.option.EuropeanVanillaOption;
	using NormalFunctionData = com.opengamma.strata.pricer.impl.option.NormalFunctionData;
	using NormalPriceFunction = com.opengamma.strata.pricer.impl.option.NormalPriceFunction;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using LongShort = com.opengamma.strata.product.common.LongShort;
	using PutCall = com.opengamma.strata.product.common.PutCall;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapLegType = com.opengamma.strata.product.swap.SwapLegType;
	using CashSwaptionSettlement = com.opengamma.strata.product.swaption.CashSwaptionSettlement;
	using CashSwaptionSettlementMethod = com.opengamma.strata.product.swaption.CashSwaptionSettlementMethod;
	using PhysicalSwaptionSettlement = com.opengamma.strata.product.swaption.PhysicalSwaptionSettlement;
	using ResolvedSwaption = com.opengamma.strata.product.swaption.ResolvedSwaption;
	using Swaption = com.opengamma.strata.product.swaption.Swaption;

	/// <summary>
	/// Test <seealso cref="NormalSwaptionCashParYieldProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NormalSwaptionCashParYieldProductPricerTest
	public class NormalSwaptionCashParYieldProductPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = RatesProviderDataSets.VAL_DATE_2014_01_22;
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
	  private static readonly Swap SWAP_REC_PAST = USD_FIXED_6M_LIBOR_3M.toTrade(SWAPTION_PAST_EXERCISE_DATE, SWAPTION_PAST_EXERCISE_DATE, SWAPTION_PAST_EXERCISE_DATE.plusYears(10), SELL, NOTIONAL, STRIKE).Product;
	  private static readonly Swap SWAP_PAY_PAST = USD_FIXED_6M_LIBOR_3M.toTrade(SWAPTION_PAST_EXERCISE_DATE, SWAPTION_PAST_EXERCISE_DATE, SWAPTION_PAST_EXERCISE_DATE.plusYears(10), BUY, NOTIONAL, STRIKE).Product;
	  private static readonly LocalDate SETTLE_DATE = USD_LIBOR_3M.EffectiveDateOffset.adjust(SWAPTION_EXERCISE_DATE, REF_DATA);
	  private static readonly CashSwaptionSettlement PAR_YIELD = CashSwaptionSettlement.of(SETTLE_DATE, CashSwaptionSettlementMethod.PAR_YIELD);
	  private static readonly ResolvedSwaption SWAPTION_REC_LONG = Swaption.builder().swaptionSettlement(PAR_YIELD).expiryDate(AdjustableDate.of(SWAPTION_EXERCISE_DATE)).expiryTime(SWAPTION_EXPIRY_TIME).expiryZone(SWAPTION_EXPIRY_ZONE).longShort(LongShort.LONG).underlying(SWAP_REC).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_REC_SHORT = Swaption.builder().swaptionSettlement(PAR_YIELD).expiryDate(AdjustableDate.of(SWAPTION_EXERCISE_DATE)).expiryTime(SWAPTION_EXPIRY_TIME).expiryZone(SWAPTION_EXPIRY_ZONE).longShort(LongShort.SHORT).underlying(SWAP_REC).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_PAY_LONG = Swaption.builder().swaptionSettlement(PAR_YIELD).expiryDate(AdjustableDate.of(SWAPTION_EXERCISE_DATE)).expiryTime(SWAPTION_EXPIRY_TIME).expiryZone(SWAPTION_EXPIRY_ZONE).longShort(LongShort.LONG).underlying(SWAP_PAY).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_PAY_SHORT = Swaption.builder().swaptionSettlement(PAR_YIELD).expiryDate(AdjustableDate.of(SWAPTION_EXERCISE_DATE)).expiryTime(SWAPTION_EXPIRY_TIME).expiryZone(SWAPTION_EXPIRY_ZONE).longShort(LongShort.SHORT).underlying(SWAP_PAY).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_REC_LONG_AT_EXPIRY = Swaption.builder().swaptionSettlement(PAR_YIELD).expiryDate(AdjustableDate.of(VAL_DATE)).expiryTime(SWAPTION_EXPIRY_TIME).expiryZone(SWAPTION_EXPIRY_ZONE).longShort(LongShort.LONG).underlying(SWAP_REC).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_PAY_SHORT_AT_EXPIRY = Swaption.builder().swaptionSettlement(PAR_YIELD).expiryDate(AdjustableDate.of(VAL_DATE)).expiryTime(SWAPTION_EXPIRY_TIME).expiryZone(SWAPTION_EXPIRY_ZONE).longShort(LongShort.SHORT).underlying(SWAP_PAY).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_REC_LONG_PAST = Swaption.builder().swaptionSettlement(PAR_YIELD).expiryDate(AdjustableDate.of(SWAPTION_PAST_EXERCISE_DATE)).expiryTime(SWAPTION_EXPIRY_TIME).expiryZone(SWAPTION_EXPIRY_ZONE).longShort(LongShort.LONG).underlying(SWAP_REC_PAST).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_PAY_SHORT_PAST = Swaption.builder().swaptionSettlement(PAR_YIELD).expiryDate(AdjustableDate.of(SWAPTION_PAST_EXERCISE_DATE)).expiryTime(SWAPTION_EXPIRY_TIME).expiryZone(SWAPTION_EXPIRY_ZONE).longShort(LongShort.LONG).underlying(SWAP_PAY_PAST).build().resolve(REF_DATA);
	  // volatility and rate providers
	  private static readonly ImmutableRatesProvider RATE_PROVIDER = RatesProviderDataSets.multiUsd(VAL_DATE);
	  private static readonly NormalSwaptionExpiryTenorVolatilities VOLS = SwaptionNormalVolatilityDataSets.NORMAL_SWAPTION_VOLS_USD_STD;
	  private static readonly NormalSwaptionVolatilities VOLS_FLAT = SwaptionNormalVolatilityDataSets.NORMAL_SWAPTION_VOLS_USD_FLAT;
	  // test parameters
	  private const double FD_EPS = 1.0E-7;
	  private const double TOL = 1.0e-12;
	  // pricers
	  private static readonly NormalPriceFunction NORMAL = new NormalPriceFunction();
	  private static readonly NormalSwaptionCashParYieldProductPricer PRICER_SWAPTION = NormalSwaptionCashParYieldProductPricer.DEFAULT;
	  private static readonly DiscountingSwapProductPricer PRICER_SWAP = DiscountingSwapProductPricer.DEFAULT;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FINITE_DIFFERENCE_CALCULATOR = new RatesFiniteDifferenceSensitivityCalculator(FD_EPS);

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		CurrencyAmount pvRecComputed = PRICER_SWAPTION.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount pvPayComputed = PRICER_SWAPTION.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		double forward = PRICER_SWAP.parRate(RSWAP_REC, RATE_PROVIDER);
		double annuityCash = PRICER_SWAP.LegPricer.annuityCash(RSWAP_REC.getLegs(SwapLegType.FIXED).get(0), forward);
		double volatility = VOLS.volatility(SWAPTION_REC_LONG.Expiry, SWAP_TENOR_YEAR, STRIKE, forward);
		double discount = RATE_PROVIDER.discountFactor(USD, SETTLE_DATE);
		NormalFunctionData normalData = NormalFunctionData.of(forward, annuityCash * discount, volatility);
		double expiry = VOLS.relativeTime(SWAPTION_REC_LONG.Expiry);
		EuropeanVanillaOption optionRec = EuropeanVanillaOption.of(STRIKE, expiry, PutCall.PUT);
		EuropeanVanillaOption optionPay = EuropeanVanillaOption.of(STRIKE, expiry, PutCall.CALL);
		double pvRecExpected = NORMAL.getPriceFunction(optionRec).apply(normalData);
		double pvPayExpected = -NORMAL.getPriceFunction(optionPay).apply(normalData);
		assertEquals(pvRecComputed.Currency, USD);
		assertEquals(pvRecComputed.Amount, pvRecExpected, NOTIONAL * TOL);
		assertEquals(pvPayComputed.Currency, USD);
		assertEquals(pvPayComputed.Amount, pvPayExpected, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_at_expiry()
	  {
		CurrencyAmount pvRec = PRICER_SWAPTION.presentValue(SWAPTION_REC_LONG_AT_EXPIRY, RATE_PROVIDER, VOLS);
		CurrencyAmount pvPay = PRICER_SWAPTION.presentValue(SWAPTION_PAY_SHORT_AT_EXPIRY, RATE_PROVIDER, VOLS);
		double forward = PRICER_SWAP.parRate(RSWAP_REC, RATE_PROVIDER);
		double annuityCash = PRICER_SWAP.LegPricer.annuityCash(RSWAP_REC.getLegs(SwapLegType.FIXED).get(0), forward);
		double discount = RATE_PROVIDER.discountFactor(USD, SETTLE_DATE);
		assertEquals(pvRec.Amount, 0d, NOTIONAL * TOL);
		assertEquals(pvPay.Amount, discount * annuityCash * (STRIKE - forward), NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_after_expiry()
	  {
		CurrencyAmount pvRec = PRICER_SWAPTION.presentValue(SWAPTION_REC_LONG_PAST, RATE_PROVIDER, VOLS);
		CurrencyAmount pvPay = PRICER_SWAPTION.presentValue(SWAPTION_PAY_SHORT_PAST, RATE_PROVIDER, VOLS);
		assertEquals(pvRec.Amount, 0d, NOTIONAL * TOL);
		assertEquals(pvPay.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_parity()
	  {
		CurrencyAmount pvRecLong = PRICER_SWAPTION.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount pvRecShort = PRICER_SWAPTION.presentValue(SWAPTION_REC_SHORT, RATE_PROVIDER, VOLS);
		CurrencyAmount pvPayLong = PRICER_SWAPTION.presentValue(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount pvPayShort = PRICER_SWAPTION.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		assertEquals(pvRecLong.Amount, -pvRecShort.Amount, NOTIONAL * TOL);
		assertEquals(pvPayLong.Amount, -pvPayShort.Amount, NOTIONAL * TOL);
		double forward = PRICER_SWAP.parRate(RSWAP_REC, RATE_PROVIDER);
		double annuityCash = PRICER_SWAP.LegPricer.annuityCash(RSWAP_REC.getLegs(SwapLegType.FIXED).get(0), forward);
		double discount = RATE_PROVIDER.discountFactor(USD, SETTLE_DATE);
		double expected = discount * annuityCash * (forward - STRIKE);
		assertEquals(pvPayLong.Amount - pvRecLong.Amount, expected, NOTIONAL * TOL);
		assertEquals(pvPayShort.Amount - pvRecShort.Amount, -expected, NOTIONAL * TOL);
	  }

	  public virtual void test_physicalSettlement()
	  {
		Swaption swaption = Swaption.builder().swaptionSettlement(PhysicalSwaptionSettlement.DEFAULT).expiryDate(AdjustableDate.of(SWAPTION_EXERCISE_DATE)).expiryTime(SWAPTION_EXPIRY_TIME).expiryZone(SWAPTION_EXPIRY_ZONE).longShort(LongShort.LONG).underlying(SWAP_REC).build();
		assertThrowsIllegalArg(() => PRICER_SWAPTION.presentValue(swaption.resolve(REF_DATA), RATE_PROVIDER, VOLS));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueDelta()
	  {
		CurrencyAmount pvDeltaRecComputed = PRICER_SWAPTION.presentValueDelta(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount pvDeltaPayComputed = PRICER_SWAPTION.presentValueDelta(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		double forward = PRICER_SWAP.parRate(RSWAP_REC, RATE_PROVIDER);
		double annuityCash = PRICER_SWAP.LegPricer.annuityCash(RSWAP_REC.getLegs(SwapLegType.FIXED).get(0), forward);
		double volatility = VOLS.volatility(SWAPTION_REC_LONG.Expiry, SWAP_TENOR_YEAR, STRIKE, forward);
		double discount = RATE_PROVIDER.discountFactor(USD, SETTLE_DATE);
		NormalFunctionData normalData = NormalFunctionData.of(forward, annuityCash * discount, volatility);
		double expiry = VOLS.relativeTime(SWAPTION_REC_LONG.Expiry);
		EuropeanVanillaOption optionRec = EuropeanVanillaOption.of(STRIKE, expiry, PutCall.PUT);
		EuropeanVanillaOption optionPay = EuropeanVanillaOption.of(STRIKE, expiry, PutCall.CALL);
		double pvDeltaRecExpected = NORMAL.getDelta(optionRec, normalData);
		double pvDeltaPayExpected = -NORMAL.getDelta(optionPay, normalData);
		assertEquals(pvDeltaRecComputed.Currency, USD);
		assertEquals(pvDeltaRecComputed.Amount, pvDeltaRecExpected, NOTIONAL * TOL);
		assertEquals(pvDeltaPayComputed.Currency, USD);
		assertEquals(pvDeltaPayComputed.Amount, pvDeltaPayExpected, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueDelta_at_expiry()
	  {
		CurrencyAmount pvDeltaRec = PRICER_SWAPTION.presentValueDelta(SWAPTION_REC_LONG_AT_EXPIRY, RATE_PROVIDER, VOLS);
		CurrencyAmount pvDeltaPay = PRICER_SWAPTION.presentValueDelta(SWAPTION_PAY_SHORT_AT_EXPIRY, RATE_PROVIDER, VOLS);
		double forward = PRICER_SWAP.parRate(RSWAP_REC, RATE_PROVIDER);
		double annuityCash = PRICER_SWAP.LegPricer.annuityCash(RSWAP_REC.getLegs(SwapLegType.FIXED).get(0), forward);
		double discount = RATE_PROVIDER.discountFactor(USD, SETTLE_DATE);
		assertEquals(pvDeltaRec.Amount, 0d, NOTIONAL * TOL);
		assertEquals(pvDeltaPay.Amount, -discount * annuityCash, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueDelta_after_expiry()
	  {
		CurrencyAmount pvDeltaRec = PRICER_SWAPTION.presentValueDelta(SWAPTION_REC_LONG_PAST, RATE_PROVIDER, VOLS);
		CurrencyAmount pvDeltaPay = PRICER_SWAPTION.presentValueDelta(SWAPTION_PAY_SHORT_PAST, RATE_PROVIDER, VOLS);
		assertEquals(pvDeltaRec.Amount, 0d, NOTIONAL * TOL);
		assertEquals(pvDeltaPay.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueDelta_parity()
	  {
		CurrencyAmount pvDeltaRecLong = PRICER_SWAPTION.presentValueDelta(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount pvDeltaRecShort = PRICER_SWAPTION.presentValueDelta(SWAPTION_REC_SHORT, RATE_PROVIDER, VOLS);
		CurrencyAmount pvDeltaPayLong = PRICER_SWAPTION.presentValueDelta(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount pvDeltaPayShort = PRICER_SWAPTION.presentValueDelta(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		assertEquals(pvDeltaRecLong.Amount, -pvDeltaRecShort.Amount, NOTIONAL * TOL);
		assertEquals(pvDeltaPayLong.Amount, -pvDeltaPayShort.Amount, NOTIONAL * TOL);
		double forward = PRICER_SWAP.parRate(RSWAP_REC, RATE_PROVIDER);
		double annuityCash = PRICER_SWAP.LegPricer.annuityCash(RSWAP_REC.getLegs(SwapLegType.FIXED).get(0), forward);
		double discount = RATE_PROVIDER.discountFactor(USD, SETTLE_DATE);
		double expected = discount * annuityCash;
		assertEquals(pvDeltaPayLong.Amount - pvDeltaRecLong.Amount, expected, NOTIONAL * TOL);
		assertEquals(pvDeltaPayShort.Amount - pvDeltaRecShort.Amount, -expected, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueGamma()
	  {
		CurrencyAmount pvGammaRecComputed = PRICER_SWAPTION.presentValueGamma(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount pvGammaPayComputed = PRICER_SWAPTION.presentValueGamma(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		double forward = PRICER_SWAP.parRate(RSWAP_REC, RATE_PROVIDER);
		double annuityCash = PRICER_SWAP.LegPricer.annuityCash(RSWAP_REC.getLegs(SwapLegType.FIXED).get(0), forward);
		double volatility = VOLS.volatility(SWAPTION_REC_LONG.Expiry, SWAP_TENOR_YEAR, STRIKE, forward);
		double discount = RATE_PROVIDER.discountFactor(USD, SETTLE_DATE);
		NormalFunctionData normalData = NormalFunctionData.of(forward, annuityCash * discount, volatility);
		double expiry = VOLS.relativeTime(SWAPTION_REC_LONG.Expiry);
		EuropeanVanillaOption optionRec = EuropeanVanillaOption.of(STRIKE, expiry, PutCall.PUT);
		EuropeanVanillaOption optionPay = EuropeanVanillaOption.of(STRIKE, expiry, PutCall.CALL);
		double pvGammaRecExpected = NORMAL.getGamma(optionRec, normalData);
		double pvGammaPayExpected = -NORMAL.getGamma(optionPay, normalData);
		assertEquals(pvGammaRecComputed.Currency, USD);
		assertEquals(pvGammaRecComputed.Amount, pvGammaRecExpected, NOTIONAL * TOL);
		assertEquals(pvGammaPayComputed.Currency, USD);
		assertEquals(pvGammaPayComputed.Amount, pvGammaPayExpected, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueGamma_at_expiry()
	  {
		CurrencyAmount pvGammaRec = PRICER_SWAPTION.presentValueGamma(SWAPTION_REC_LONG_AT_EXPIRY, RATE_PROVIDER, VOLS);
		CurrencyAmount pvGammaPay = PRICER_SWAPTION.presentValueGamma(SWAPTION_PAY_SHORT_AT_EXPIRY, RATE_PROVIDER, VOLS);
		assertEquals(pvGammaRec.Amount, 0d, NOTIONAL * TOL);
		assertEquals(pvGammaPay.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueGamma_after_expiry()
	  {
		CurrencyAmount pvGammaRec = PRICER_SWAPTION.presentValueGamma(SWAPTION_REC_LONG_PAST, RATE_PROVIDER, VOLS);
		CurrencyAmount pvGammaPay = PRICER_SWAPTION.presentValueGamma(SWAPTION_PAY_SHORT_PAST, RATE_PROVIDER, VOLS);
		assertEquals(pvGammaRec.Amount, 0d, NOTIONAL * TOL);
		assertEquals(pvGammaPay.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueGamma_parity()
	  {
		CurrencyAmount pvGammaRecLong = PRICER_SWAPTION.presentValueGamma(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount pvGammaRecShort = PRICER_SWAPTION.presentValueGamma(SWAPTION_REC_SHORT, RATE_PROVIDER, VOLS);
		CurrencyAmount pvGammaPayLong = PRICER_SWAPTION.presentValueGamma(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount pvGammaPayShort = PRICER_SWAPTION.presentValueGamma(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		assertEquals(pvGammaRecLong.Amount, -pvGammaRecShort.Amount, NOTIONAL * TOL);
		assertEquals(pvGammaPayLong.Amount, -pvGammaPayShort.Amount, NOTIONAL * TOL);
		assertEquals(pvGammaPayLong.Amount, pvGammaRecLong.Amount, NOTIONAL * TOL);
		assertEquals(pvGammaPayShort.Amount, pvGammaRecShort.Amount, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueTheta()
	  {
		CurrencyAmount pvThetaRecComputed = PRICER_SWAPTION.presentValueTheta(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount pvThetaPayComputed = PRICER_SWAPTION.presentValueTheta(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		double forward = PRICER_SWAP.parRate(RSWAP_REC, RATE_PROVIDER);
		double annuityCash = PRICER_SWAP.LegPricer.annuityCash(RSWAP_REC.getLegs(SwapLegType.FIXED).get(0), forward);
		double volatility = VOLS.volatility(SWAPTION_REC_LONG.Expiry, SWAP_TENOR_YEAR, STRIKE, forward);
		double discount = RATE_PROVIDER.discountFactor(USD, SETTLE_DATE);
		NormalFunctionData normalData = NormalFunctionData.of(forward, annuityCash * discount, volatility);
		double expiry = VOLS.relativeTime(SWAPTION_REC_LONG.Expiry);
		EuropeanVanillaOption optionRec = EuropeanVanillaOption.of(STRIKE, expiry, PutCall.PUT);
		EuropeanVanillaOption optionPay = EuropeanVanillaOption.of(STRIKE, expiry, PutCall.CALL);
		double pvThetaRecExpected = NORMAL.getTheta(optionRec, normalData);
		double pvThetaPayExpected = -NORMAL.getTheta(optionPay, normalData);
		assertEquals(pvThetaRecComputed.Currency, USD);
		assertEquals(pvThetaRecComputed.Amount, pvThetaRecExpected, NOTIONAL * TOL);
		assertEquals(pvThetaPayComputed.Currency, USD);
		assertEquals(pvThetaPayComputed.Amount, pvThetaPayExpected, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueTheta_at_expiry()
	  {
		CurrencyAmount pvThetaRec = PRICER_SWAPTION.presentValueTheta(SWAPTION_REC_LONG_AT_EXPIRY, RATE_PROVIDER, VOLS);
		CurrencyAmount pvThetaPay = PRICER_SWAPTION.presentValueTheta(SWAPTION_PAY_SHORT_AT_EXPIRY, RATE_PROVIDER, VOLS);
		assertEquals(pvThetaRec.Amount, 0d, NOTIONAL * TOL);
		assertEquals(pvThetaPay.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueTheta_after_expiry()
	  {
		CurrencyAmount pvThetaRec = PRICER_SWAPTION.presentValueTheta(SWAPTION_REC_LONG_PAST, RATE_PROVIDER, VOLS);
		CurrencyAmount pvThetaPay = PRICER_SWAPTION.presentValueTheta(SWAPTION_PAY_SHORT_PAST, RATE_PROVIDER, VOLS);
		assertEquals(pvThetaRec.Amount, 0d, NOTIONAL * TOL);
		assertEquals(pvThetaPay.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueTheta_parity()
	  {
		CurrencyAmount pvThetaRecLong = PRICER_SWAPTION.presentValueTheta(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount pvThetaRecShort = PRICER_SWAPTION.presentValueTheta(SWAPTION_REC_SHORT, RATE_PROVIDER, VOLS);
		CurrencyAmount pvThetaPayLong = PRICER_SWAPTION.presentValueTheta(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount pvThetaPayShort = PRICER_SWAPTION.presentValueTheta(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		assertEquals(pvThetaRecLong.Amount, -pvThetaRecShort.Amount, NOTIONAL * TOL);
		assertEquals(pvThetaPayLong.Amount, -pvThetaPayShort.Amount, NOTIONAL * TOL);
		assertEquals(pvThetaPayLong.Amount, pvThetaRecLong.Amount, NOTIONAL * TOL);
		assertEquals(pvThetaPayShort.Amount, pvThetaRecShort.Amount, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------  
	  public virtual void test_currencyExposure()
	  {
		MultiCurrencyAmount computedRec = PRICER_SWAPTION.currencyExposure(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount computedPay = PRICER_SWAPTION.currencyExposure(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		PointSensitivityBuilder pointRec = PRICER_SWAPTION.presentValueSensitivityRatesStickyStrike(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount expectedRec = RATE_PROVIDER.currencyExposure(pointRec.build()).plus(PRICER_SWAPTION.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS));
		assertEquals(computedRec.size(), 1);
		assertEquals(computedRec.getAmount(USD).Amount, expectedRec.getAmount(USD).Amount, NOTIONAL * TOL);
		PointSensitivityBuilder pointPay = PRICER_SWAPTION.presentValueSensitivityRatesStickyStrike(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount expectedPay = RATE_PROVIDER.currencyExposure(pointPay.build()).plus(PRICER_SWAPTION.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS));
		assertEquals(computedPay.size(), 1);
		assertEquals(computedPay.getAmount(USD).Amount, expectedPay.getAmount(USD).Amount, NOTIONAL * TOL);
	  }

	  public virtual void test_currencyExposure_at_expiry()
	  {
		MultiCurrencyAmount computedRec = PRICER_SWAPTION.currencyExposure(SWAPTION_REC_LONG_AT_EXPIRY, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount computedPay = PRICER_SWAPTION.currencyExposure(SWAPTION_PAY_SHORT_AT_EXPIRY, RATE_PROVIDER, VOLS);
		PointSensitivityBuilder pointRec = PRICER_SWAPTION.presentValueSensitivityRatesStickyStrike(SWAPTION_REC_LONG_AT_EXPIRY, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount expectedRec = RATE_PROVIDER.currencyExposure(pointRec.build()).plus(PRICER_SWAPTION.presentValue(SWAPTION_REC_LONG_AT_EXPIRY, RATE_PROVIDER, VOLS));
		assertEquals(computedRec.size(), 1);
		assertEquals(computedRec.getAmount(USD).Amount, expectedRec.getAmount(USD).Amount, NOTIONAL * TOL);
		PointSensitivityBuilder pointPay = PRICER_SWAPTION.presentValueSensitivityRatesStickyStrike(SWAPTION_PAY_SHORT_AT_EXPIRY, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount expectedPay = RATE_PROVIDER.currencyExposure(pointPay.build()).plus(PRICER_SWAPTION.presentValue(SWAPTION_PAY_SHORT_AT_EXPIRY, RATE_PROVIDER, VOLS));
		assertEquals(computedPay.size(), 1);
		assertEquals(computedPay.getAmount(USD).Amount, expectedPay.getAmount(USD).Amount, NOTIONAL * TOL);
	  }

	  public virtual void test_currencyExposure_after_expiry()
	  {
		MultiCurrencyAmount computedRec = PRICER_SWAPTION.currencyExposure(SWAPTION_REC_LONG_PAST, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount computedPay = PRICER_SWAPTION.currencyExposure(SWAPTION_PAY_SHORT_PAST, RATE_PROVIDER, VOLS);
		assertEquals(computedRec.size(), 1);
		assertEquals(computedRec.getAmount(USD).Amount, 0d, NOTIONAL * TOL);
		assertEquals(computedPay.size(), 1);
		assertEquals(computedPay.getAmount(USD).Amount, 0d, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_impliedVolatility()
	  {
		double forward = PRICER_SWAP.parRate(RSWAP_REC, RATE_PROVIDER);
		double expected = VOLS.volatility(SWAPTION_REC_LONG.Expiry, SWAP_TENOR_YEAR, STRIKE, forward);
		double computedRec = PRICER_SWAPTION.impliedVolatility(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		double computedPay = PRICER_SWAPTION.impliedVolatility(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		assertEquals(computedRec, expected, TOL);
		assertEquals(computedPay, expected, TOL);
	  }

	  public virtual void test_impliedVolatility_at_expiry()
	  {
		double forward = PRICER_SWAP.parRate(RSWAP_REC, RATE_PROVIDER);
		double expected = VOLS.volatility(VAL_DATE.atTime(SWAPTION_EXPIRY_TIME).atZone(SWAPTION_EXPIRY_ZONE), SWAP_TENOR_YEAR, STRIKE, forward);
		double computedRec = PRICER_SWAPTION.impliedVolatility(SWAPTION_REC_LONG_AT_EXPIRY, RATE_PROVIDER, VOLS);
		double computedPay = PRICER_SWAPTION.impliedVolatility(SWAPTION_PAY_SHORT_AT_EXPIRY, RATE_PROVIDER, VOLS);
		assertEquals(computedRec, expected, TOL);
		assertEquals(computedPay, expected, TOL);
	  }

	  public virtual void test_impliedVolatility_after_expiry()
	  {
		assertThrowsIllegalArg(() => PRICER_SWAPTION.impliedVolatility(SWAPTION_REC_LONG_PAST, RATE_PROVIDER, VOLS));
		assertThrowsIllegalArg(() => PRICER_SWAPTION.impliedVolatility(SWAPTION_PAY_SHORT_PAST, RATE_PROVIDER, VOLS));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void implied_volatility_round_trip()
	  { // Compute pv and then implied vol from PV and compare with direct implied vol
		CurrencyAmount pvLongRec = PRICER_SWAPTION.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		double impliedLongRecComputed = PRICER_SWAPTION.impliedVolatilityFromPresentValue(SWAPTION_REC_LONG, RATE_PROVIDER, ACT_365F, pvLongRec.Amount);
		double impliedLongRecInterpolated = PRICER_SWAPTION.impliedVolatility(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		assertEquals(impliedLongRecComputed, impliedLongRecInterpolated, TOL);

		CurrencyAmount pvLongPay = PRICER_SWAPTION.presentValue(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS);
		double impliedLongPayComputed = PRICER_SWAPTION.impliedVolatilityFromPresentValue(SWAPTION_PAY_LONG, RATE_PROVIDER, ACT_365F, pvLongPay.Amount);
		double impliedLongPayInterpolated = PRICER_SWAPTION.impliedVolatility(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS);
		assertEquals(impliedLongPayComputed, impliedLongPayInterpolated, TOL);

		CurrencyAmount pvShortRec = PRICER_SWAPTION.presentValue(SWAPTION_REC_SHORT, RATE_PROVIDER, VOLS);
		double impliedShortRecComputed = PRICER_SWAPTION.impliedVolatilityFromPresentValue(SWAPTION_REC_SHORT, RATE_PROVIDER, ACT_365F, pvShortRec.Amount);
		double impliedShortRecInterpolated = PRICER_SWAPTION.impliedVolatility(SWAPTION_REC_SHORT, RATE_PROVIDER, VOLS);
		assertEquals(impliedShortRecComputed, impliedShortRecInterpolated, TOL);
	  }

	  public virtual void implied_volatility_wrong_sign()
	  {
		CurrencyAmount pvLongRec = PRICER_SWAPTION.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		assertThrowsIllegalArg(() => PRICER_SWAPTION.impliedVolatilityFromPresentValue(SWAPTION_REC_LONG, RATE_PROVIDER, ACT_365F, -pvLongRec.Amount));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivityRatesStickyStrike()
	  {
		PointSensitivities pointRec = PRICER_SWAPTION.presentValueSensitivityRatesStickyStrike(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS_FLAT).build();
		CurrencyParameterSensitivities computedRec = RATE_PROVIDER.parameterSensitivity(pointRec);
		CurrencyParameterSensitivities expectedRec = FINITE_DIFFERENCE_CALCULATOR.sensitivity(RATE_PROVIDER, (p) => PRICER_SWAPTION.presentValue(SWAPTION_REC_LONG, p, VOLS_FLAT));
		assertTrue(computedRec.equalWithTolerance(expectedRec, NOTIONAL * FD_EPS * 200d));
		PointSensitivities pointPay = PRICER_SWAPTION.presentValueSensitivityRatesStickyStrike(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS_FLAT).build();
		CurrencyParameterSensitivities computedPay = RATE_PROVIDER.parameterSensitivity(pointPay);
		CurrencyParameterSensitivities expectedPay = FINITE_DIFFERENCE_CALCULATOR.sensitivity(RATE_PROVIDER, (p) => PRICER_SWAPTION.presentValue(SWAPTION_PAY_SHORT, p, VOLS_FLAT));
		assertTrue(computedPay.equalWithTolerance(expectedPay, NOTIONAL * FD_EPS * 200d));
	  }

	  public virtual void test_presentValueSensitivityRatesStickyStrike_at_expiry()
	  {
		PointSensitivities pointRec = PRICER_SWAPTION.presentValueSensitivityRatesStickyStrike(SWAPTION_REC_LONG_AT_EXPIRY, RATE_PROVIDER, VOLS).build();
		foreach (PointSensitivity sensi in pointRec.Sensitivities)
		{
		  assertEquals(Math.Abs(sensi.Sensitivity), 0d);
		}
		PointSensitivities pointPay = PRICER_SWAPTION.presentValueSensitivityRatesStickyStrike(SWAPTION_PAY_SHORT_AT_EXPIRY, RATE_PROVIDER, VOLS).build();
		CurrencyParameterSensitivities computedPay = RATE_PROVIDER.parameterSensitivity(pointPay);
		CurrencyParameterSensitivities expectedPay = FINITE_DIFFERENCE_CALCULATOR.sensitivity(RATE_PROVIDER, (p) => PRICER_SWAPTION.presentValue(SWAPTION_PAY_SHORT_AT_EXPIRY, p, VOLS_FLAT));
		assertTrue(computedPay.equalWithTolerance(expectedPay, NOTIONAL * FD_EPS * 100d));
	  }

	  public virtual void test_presentValueSensitivityRatesStickyStrike_after_expiry()
	  {
		PointSensitivityBuilder pointRec = PRICER_SWAPTION.presentValueSensitivityRatesStickyStrike(SWAPTION_REC_LONG_PAST, RATE_PROVIDER, VOLS);
		PointSensitivityBuilder pointPay = PRICER_SWAPTION.presentValueSensitivityRatesStickyStrike(SWAPTION_PAY_SHORT_PAST, RATE_PROVIDER, VOLS);
		assertEquals(pointRec, PointSensitivityBuilder.none());
		assertEquals(pointPay, PointSensitivityBuilder.none());
	  }

	  public virtual void test_presentValueSensitivityRatesStickyStrike_parity()
	  {
		CurrencyParameterSensitivities pvSensiRecLong = RATE_PROVIDER.parameterSensitivity(PRICER_SWAPTION.presentValueSensitivityRatesStickyStrike(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS).build());
		CurrencyParameterSensitivities pvSensiRecShort = RATE_PROVIDER.parameterSensitivity(PRICER_SWAPTION.presentValueSensitivityRatesStickyStrike(SWAPTION_REC_SHORT, RATE_PROVIDER, VOLS).build());
		CurrencyParameterSensitivities pvSensiPayLong = RATE_PROVIDER.parameterSensitivity(PRICER_SWAPTION.presentValueSensitivityRatesStickyStrike(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS).build());
		CurrencyParameterSensitivities pvSensiPayShort = RATE_PROVIDER.parameterSensitivity(PRICER_SWAPTION.presentValueSensitivityRatesStickyStrike(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS).build());
		assertTrue(pvSensiRecLong.equalWithTolerance(pvSensiRecShort.multipliedBy(-1d), NOTIONAL * TOL));
		assertTrue(pvSensiPayLong.equalWithTolerance(pvSensiPayShort.multipliedBy(-1d), NOTIONAL * TOL));

		double forward = PRICER_SWAP.parRate(RSWAP_REC, RATE_PROVIDER);
		PointSensitivityBuilder forwardSensi = PRICER_SWAP.parRateSensitivity(RSWAP_REC, RATE_PROVIDER);
		double annuityCash = PRICER_SWAP.LegPricer.annuityCash(RSWAP_REC.getLegs(SwapLegType.FIXED).get(0), forward);
		double annuityCashDeriv = PRICER_SWAP.LegPricer.annuityCashDerivative(RSWAP_REC.getLegs(SwapLegType.FIXED).get(0), forward).getDerivative(0);
		double discount = RATE_PROVIDER.discountFactor(USD, SETTLE_DATE);
		PointSensitivityBuilder discountSensi = RATE_PROVIDER.discountFactors(USD).zeroRatePointSensitivity(SETTLE_DATE);
		PointSensitivities expecedPoint = discountSensi.multipliedBy(annuityCash * (forward - STRIKE)).combinedWith(forwardSensi.multipliedBy(discount * annuityCash + discount * annuityCashDeriv * (forward - STRIKE))).build();
		CurrencyParameterSensitivities expected = RATE_PROVIDER.parameterSensitivity(expecedPoint);
		assertTrue(expected.equalWithTolerance(pvSensiPayLong.combinedWith(pvSensiRecLong.multipliedBy(-1d)), NOTIONAL * TOL));
		assertTrue(expected.equalWithTolerance(pvSensiRecShort.combinedWith(pvSensiPayShort.multipliedBy(-1d)), NOTIONAL * TOL));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivityNormalVolatility()
	  {
		SwaptionSensitivity computedRec = PRICER_SWAPTION.presentValueSensitivityModelParamsVolatility(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount pvRecUp = PRICER_SWAPTION.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER, SwaptionNormalVolatilityDataSets.normalVolSwaptionProviderUsdStsShifted(FD_EPS));
		CurrencyAmount pvRecDw = PRICER_SWAPTION.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER, SwaptionNormalVolatilityDataSets.normalVolSwaptionProviderUsdStsShifted(-FD_EPS));
		double expectedRec = 0.5 * (pvRecUp.Amount - pvRecDw.Amount) / FD_EPS;
		assertEquals(computedRec.Currency, USD);
		assertEquals(computedRec.Sensitivity, expectedRec, FD_EPS * NOTIONAL);
		assertEquals(computedRec.VolatilitiesName, VOLS.Name);
		assertEquals(computedRec.Expiry, VOLS.relativeTime(SWAPTION_REC_LONG.Expiry));
		assertEquals(computedRec.Tenor, SWAP_TENOR_YEAR, TOL);
		assertEquals(computedRec.Strike, STRIKE, TOL);
		assertEquals(computedRec.Forward, PRICER_SWAP.parRate(RSWAP_REC, RATE_PROVIDER), TOL);
		SwaptionSensitivity computedPay = PRICER_SWAPTION.presentValueSensitivityModelParamsVolatility(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		CurrencyAmount pvUpPay = PRICER_SWAPTION.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER, SwaptionNormalVolatilityDataSets.normalVolSwaptionProviderUsdStsShifted(FD_EPS));
		CurrencyAmount pvDwPay = PRICER_SWAPTION.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER, SwaptionNormalVolatilityDataSets.normalVolSwaptionProviderUsdStsShifted(-FD_EPS));
		double expectedPay = 0.5 * (pvUpPay.Amount - pvDwPay.Amount) / FD_EPS;
		assertEquals(computedPay.Currency, USD);
		assertEquals(computedPay.Sensitivity, expectedPay, FD_EPS * NOTIONAL);
		assertEquals(computedPay.VolatilitiesName, VOLS.Name);
		assertEquals(computedPay.Expiry, VOLS.relativeTime(SWAPTION_PAY_SHORT.Expiry));
		assertEquals(computedPay.Tenor, SWAP_TENOR_YEAR, TOL);
		assertEquals(computedPay.Strike, STRIKE, TOL);
		assertEquals(computedPay.Forward, PRICER_SWAP.parRate(RSWAP_PAY, RATE_PROVIDER), TOL);
	  }

	  public virtual void test_presentValueSensitivityNormalVolatility_at_expiry()
	  {
		SwaptionSensitivity sensiRec = PRICER_SWAPTION.presentValueSensitivityModelParamsVolatility(SWAPTION_REC_LONG_AT_EXPIRY, RATE_PROVIDER, VOLS);
		assertEquals(sensiRec.Sensitivity, 0d, NOTIONAL * TOL);
		SwaptionSensitivity sensiPay = PRICER_SWAPTION.presentValueSensitivityModelParamsVolatility(SWAPTION_PAY_SHORT_AT_EXPIRY, RATE_PROVIDER, VOLS);
		assertEquals(sensiPay.Sensitivity, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueSensitivityNormalVolatility_after_expiry()
	  {
		SwaptionSensitivity sensiRec = PRICER_SWAPTION.presentValueSensitivityModelParamsVolatility(SWAPTION_REC_LONG_PAST, RATE_PROVIDER, VOLS);
		SwaptionSensitivity sensiPay = PRICER_SWAPTION.presentValueSensitivityModelParamsVolatility(SWAPTION_PAY_SHORT_PAST, RATE_PROVIDER, VOLS);
		assertEquals(sensiRec.Sensitivity, 0.0d, NOTIONAL * TOL);
		assertEquals(sensiPay.Sensitivity, 0.0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValueSensitivityNormalVolatility_parity()
	  {
		SwaptionSensitivity pvSensiRecLong = PRICER_SWAPTION.presentValueSensitivityModelParamsVolatility(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		SwaptionSensitivity pvSensiRecShort = PRICER_SWAPTION.presentValueSensitivityModelParamsVolatility(SWAPTION_REC_SHORT, RATE_PROVIDER, VOLS);
		SwaptionSensitivity pvSensiPayLong = PRICER_SWAPTION.presentValueSensitivityModelParamsVolatility(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS);
		SwaptionSensitivity pvSensiPayShort = PRICER_SWAPTION.presentValueSensitivityModelParamsVolatility(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		assertEquals(pvSensiRecLong.Sensitivity, -pvSensiRecShort.Sensitivity, NOTIONAL * TOL);
		assertEquals(pvSensiPayLong.Sensitivity, -pvSensiPayShort.Sensitivity, NOTIONAL * TOL);
		assertEquals(pvSensiRecLong.Sensitivity, pvSensiPayLong.Sensitivity, NOTIONAL * TOL);
		assertEquals(pvSensiPayShort.Sensitivity, pvSensiPayShort.Sensitivity, NOTIONAL * TOL);
	  }

	}

}