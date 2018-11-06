/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.LongShort.LONG;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.LongShort.SHORT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using DoubleMath = com.google.common.math.DoubleMath;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxMatrix = com.opengamma.strata.basics.currency.FxMatrix;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProviderFxDataSets = com.opengamma.strata.pricer.fx.RatesProviderFxDataSets;
	using BlackBarrierPriceFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackBarrierPriceFormulaRepository;
	using BlackOneTouchAssetPriceFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackOneTouchAssetPriceFormulaRepository;
	using BlackOneTouchCashPriceFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackOneTouchCashPriceFormulaRepository;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using ResolvedFxSingle = com.opengamma.strata.product.fx.ResolvedFxSingle;
	using ResolvedFxSingleBarrierOption = com.opengamma.strata.product.fxopt.ResolvedFxSingleBarrierOption;
	using ResolvedFxVanillaOption = com.opengamma.strata.product.fxopt.ResolvedFxVanillaOption;
	using BarrierType = com.opengamma.strata.product.option.BarrierType;
	using KnockType = com.opengamma.strata.product.option.KnockType;
	using SimpleConstantContinuousBarrier = com.opengamma.strata.product.option.SimpleConstantContinuousBarrier;

	/// <summary>
	/// Test <seealso cref="BlackFxVanillaOptionProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BlackFxSingleBarrierOptionProductPricerTest
	public class BlackFxSingleBarrierOptionProductPricerTest
	{

	  private static readonly ZoneId ZONE = ZoneId.of("Z");
	  private static readonly LocalDate VAL_DATE = LocalDate.of(2011, 6, 13);
	  private static readonly ZonedDateTime VAL_DATETIME = VAL_DATE.atStartOfDay(ZONE);
	  private static readonly LocalDate PAY_DATE = LocalDate.of(2014, 9, 15);
	  private static readonly LocalDate EXPIRY_DATE = LocalDate.of(2014, 9, 15);
	  private static readonly ZonedDateTime EXPIRY_DATETIME = EXPIRY_DATE.atStartOfDay(ZONE);
	  // providers
	  private static readonly BlackFxOptionSmileVolatilities VOLS = FxVolatilitySmileDataSet.createVolatilitySmileProvider5(VAL_DATETIME);
	  private static readonly ImmutableRatesProvider RATE_PROVIDER = RatesProviderFxDataSets.createProviderEurUsdActActIsda(VAL_DATE);
	  private static readonly BlackFxOptionSmileVolatilities VOLS_FLAT = FxVolatilitySmileDataSet.createVolatilitySmileProvider5Flat(VAL_DATETIME);
	  // providers - valuation at expiry
	  private static readonly BlackFxOptionSmileVolatilities VOLS_EXPIRY = FxVolatilitySmileDataSet.createVolatilitySmileProvider5(EXPIRY_DATETIME);
	  private static readonly ImmutableRatesProvider RATE_PROVIDER_EXPIRY = RatesProviderFxDataSets.createProviderEurUsdActActIsda(EXPIRY_DATE);
	  // provider - valuation after expiry
	  private static readonly BlackFxOptionSmileVolatilities VOLS_AFTER = FxVolatilitySmileDataSet.createVolatilitySmileProvider5(EXPIRY_DATETIME.plusDays(1));
	  private static readonly ImmutableRatesProvider RATE_PROVIDER_AFTER = RatesProviderFxDataSets.createProviderEurUsdActActIsda(EXPIRY_DATE.plusDays(1));

	  private static readonly CurrencyPair CURRENCY_PAIR = CurrencyPair.of(EUR, USD);
	  private static readonly double SPOT = RATE_PROVIDER.fxRate(CURRENCY_PAIR);
	  private const double NOTIONAL = 100_000_000d;
	  private const double LEVEL_LOW = 1.35;
	  private const double LEVEL_HIGH = 1.5;
	  private static readonly SimpleConstantContinuousBarrier BARRIER_DKI = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_IN, LEVEL_LOW);
	  private static readonly SimpleConstantContinuousBarrier BARRIER_DKO = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_OUT, LEVEL_LOW);
	  private static readonly SimpleConstantContinuousBarrier BARRIER_UKI = SimpleConstantContinuousBarrier.of(BarrierType.UP, KnockType.KNOCK_IN, LEVEL_HIGH);
	  private static readonly SimpleConstantContinuousBarrier BARRIER_UKO = SimpleConstantContinuousBarrier.of(BarrierType.UP, KnockType.KNOCK_OUT, LEVEL_HIGH);
	  private const double REBATE_AMOUNT = 50_000d;
	  private static readonly CurrencyAmount REBATE = CurrencyAmount.of(USD, REBATE_AMOUNT);
	  private static readonly CurrencyAmount REBATE_BASE = CurrencyAmount.of(EUR, REBATE_AMOUNT);
	  private const double STRIKE_RATE = 1.45;
	  // call
	  private static readonly CurrencyAmount EUR_AMOUNT_REC = CurrencyAmount.of(EUR, NOTIONAL);
	  private static readonly CurrencyAmount USD_AMOUNT_PAY = CurrencyAmount.of(USD, -NOTIONAL * STRIKE_RATE);
	  private static readonly ResolvedFxSingle FX_PRODUCT = ResolvedFxSingle.of(EUR_AMOUNT_REC, USD_AMOUNT_PAY, PAY_DATE);
	  private static readonly ResolvedFxVanillaOption CALL = ResolvedFxVanillaOption.builder().longShort(LONG).expiry(EXPIRY_DATETIME).underlying(FX_PRODUCT).build();
	  private static readonly ResolvedFxSingleBarrierOption CALL_DKI = ResolvedFxSingleBarrierOption.of(CALL, BARRIER_DKI, REBATE);
	  private static readonly ResolvedFxSingleBarrierOption CALL_DKI_BASE = ResolvedFxSingleBarrierOption.of(CALL, BARRIER_DKI, REBATE_BASE);
	  private static readonly ResolvedFxSingleBarrierOption CALL_DKO = ResolvedFxSingleBarrierOption.of(CALL, BARRIER_DKO, REBATE);
	  private static readonly ResolvedFxSingleBarrierOption CALL_DKO_BASE = ResolvedFxSingleBarrierOption.of(CALL, BARRIER_DKO, REBATE_BASE);
	  private static readonly ResolvedFxSingleBarrierOption CALL_UKI = ResolvedFxSingleBarrierOption.of(CALL, BARRIER_UKI, REBATE);
	  private static readonly ResolvedFxSingleBarrierOption CALL_UKI_BASE = ResolvedFxSingleBarrierOption.of(CALL, BARRIER_UKI, REBATE_BASE);
	  private static readonly ResolvedFxSingleBarrierOption CALL_UKO = ResolvedFxSingleBarrierOption.of(CALL, BARRIER_UKO, REBATE);
	  private static readonly ResolvedFxSingleBarrierOption CALL_UKO_BASE = ResolvedFxSingleBarrierOption.of(CALL, BARRIER_UKO, REBATE_BASE);
	  // put
	  private static readonly CurrencyAmount EUR_AMOUNT_PAY = CurrencyAmount.of(EUR, -NOTIONAL);
	  private static readonly CurrencyAmount USD_AMOUNT_REC = CurrencyAmount.of(USD, NOTIONAL * STRIKE_RATE);
	  private static readonly ResolvedFxSingle FX_PRODUCT_INV = ResolvedFxSingle.of(EUR_AMOUNT_PAY, USD_AMOUNT_REC, PAY_DATE);
	  private static readonly ResolvedFxVanillaOption PUT = ResolvedFxVanillaOption.builder().longShort(SHORT).expiry(EXPIRY_DATETIME).underlying(FX_PRODUCT_INV).build();
	  private static readonly ResolvedFxSingleBarrierOption PUT_DKI = ResolvedFxSingleBarrierOption.of(PUT, BARRIER_DKI, REBATE);
	  private static readonly ResolvedFxSingleBarrierOption PUT_DKI_BASE = ResolvedFxSingleBarrierOption.of(PUT, BARRIER_DKI, REBATE_BASE);
	  private static readonly ResolvedFxSingleBarrierOption PUT_DKO = ResolvedFxSingleBarrierOption.of(PUT, BARRIER_DKO, REBATE);
	  private static readonly ResolvedFxSingleBarrierOption PUT_DKO_BASE = ResolvedFxSingleBarrierOption.of(PUT, BARRIER_DKO, REBATE_BASE);
	  private static readonly ResolvedFxSingleBarrierOption PUT_UKI = ResolvedFxSingleBarrierOption.of(PUT, BARRIER_UKI, REBATE);
	  private static readonly ResolvedFxSingleBarrierOption PUT_UKI_BASE = ResolvedFxSingleBarrierOption.of(PUT, BARRIER_UKI, REBATE_BASE);
	  private static readonly ResolvedFxSingleBarrierOption PUT_UKO = ResolvedFxSingleBarrierOption.of(PUT, BARRIER_UKO, REBATE);
	  private static readonly ResolvedFxSingleBarrierOption PUT_UKO_BASE = ResolvedFxSingleBarrierOption.of(PUT, BARRIER_UKO, REBATE_BASE);
	  private static readonly ResolvedFxSingleBarrierOption[] OPTION_ALL = new ResolvedFxSingleBarrierOption[] {CALL_DKI, CALL_DKI_BASE, CALL_DKO, CALL_DKO_BASE, CALL_UKI, CALL_UKI_BASE, CALL_UKO, CALL_UKO_BASE, PUT_DKI, PUT_DKI_BASE, PUT_DKO, PUT_DKO_BASE, PUT_UKI, PUT_UKI_BASE, PUT_UKO, PUT_UKO_BASE};

	  private static readonly BlackFxSingleBarrierOptionProductPricer PRICER = BlackFxSingleBarrierOptionProductPricer.DEFAULT;
	  private static readonly BlackFxVanillaOptionProductPricer VANILLA_PRICER = BlackFxVanillaOptionProductPricer.DEFAULT;
	  private static readonly BlackBarrierPriceFormulaRepository BARRIER_PRICER = new BlackBarrierPriceFormulaRepository();
	  private static readonly BlackOneTouchAssetPriceFormulaRepository ASSET_REBATE_PRICER = new BlackOneTouchAssetPriceFormulaRepository();
	  private static readonly BlackOneTouchCashPriceFormulaRepository CASH_REBATE_PRICER = new BlackOneTouchCashPriceFormulaRepository();
	  private const double TOL = 1.0e-12;
	  private const double FD_EPS = 1.0e-6;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FD_CAL = new RatesFiniteDifferenceSensitivityCalculator(FD_EPS);

	  //-------------------------------------------------------------------------
	  public virtual void test_price_presentValue()
	  {
		double computedPriceCall = PRICER.price(CALL_UKI, RATE_PROVIDER, VOLS);
		double computedPricePut = PRICER.price(PUT_UKO_BASE, RATE_PROVIDER, VOLS);
		CurrencyAmount computedPvCall = PRICER.presentValue(CALL_UKI, RATE_PROVIDER, VOLS);
		CurrencyAmount computedPvPut = PRICER.presentValue(PUT_UKO_BASE, RATE_PROVIDER, VOLS);
		double rateBase = RATE_PROVIDER.discountFactors(EUR).zeroRate(PAY_DATE);
		double rateCounter = RATE_PROVIDER.discountFactors(USD).zeroRate(PAY_DATE);
		double costOfCarry = rateCounter - rateBase;
		double forward = RATE_PROVIDER.fxForwardRates(CURRENCY_PAIR).rate(EUR, PAY_DATE);
		double volatility = VOLS.volatility(CURRENCY_PAIR, EXPIRY_DATETIME, STRIKE_RATE, forward);
		double timeToExpiry = VOLS.relativeTime(EXPIRY_DATETIME);
		double rebateRate = REBATE_AMOUNT / NOTIONAL;
		double expectedCash = CASH_REBATE_PRICER.price(SPOT, timeToExpiry, costOfCarry, rateCounter, volatility, BARRIER_UKO);
		double expectedAsset = ASSET_REBATE_PRICER.price(SPOT, timeToExpiry, costOfCarry, rateCounter, volatility, BARRIER_UKI);
		double expectedPriceCall = BARRIER_PRICER.price(SPOT, STRIKE_RATE, timeToExpiry, costOfCarry, rateCounter, volatility, true, BARRIER_UKI) + rebateRate * expectedCash;
		double expectedPricePut = BARRIER_PRICER.price(SPOT, STRIKE_RATE, timeToExpiry, costOfCarry, rateCounter, volatility, false, BARRIER_UKO) + rebateRate * expectedAsset;
		assertEquals(computedPriceCall, expectedPriceCall, TOL);
		assertEquals(computedPricePut, expectedPricePut, TOL);
		assertEquals(computedPvCall.Currency, USD);
		assertEquals(computedPvPut.Currency, USD);
		assertEquals(computedPvCall.Amount, expectedPriceCall * NOTIONAL, TOL);
		assertEquals(computedPvPut.Amount, -expectedPricePut * NOTIONAL, TOL);
	  }

	  public virtual void test_price_presentValue_atExpiry()
	  {
		double computedPriceCall = PRICER.price(CALL_UKI, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		double computedPriceCallZero = PRICER.price(CALL_UKO, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		double computedPricePut = PRICER.price(PUT_UKO_BASE, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		CurrencyAmount computedPvCall = PRICER.presentValue(CALL_UKI, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		CurrencyAmount computedPvCallZero = PRICER.presentValue(CALL_UKO, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		CurrencyAmount computedPvPut = PRICER.presentValue(PUT_UKO_BASE, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		double expectedPriceCall = REBATE_AMOUNT / NOTIONAL;
		double expectedPricePut = STRIKE_RATE - SPOT;
		assertEquals(computedPriceCall, expectedPriceCall, TOL);
		assertEquals(computedPriceCallZero, 0d, TOL);
		assertEquals(computedPricePut, expectedPricePut, TOL);
		assertEquals(computedPvCall.Amount, expectedPriceCall * NOTIONAL, TOL);
		assertEquals(computedPvCallZero.Amount, 0d * NOTIONAL, TOL);
		assertEquals(computedPvPut.Amount, -expectedPricePut * NOTIONAL, TOL);
	  }

	  public virtual void test_price_presentValue_afterExpiry()
	  {
		double computedPriceCall = PRICER.price(CALL_UKI, RATE_PROVIDER_AFTER, VOLS_AFTER);
		double computedPricePut = PRICER.price(PUT_UKO_BASE, RATE_PROVIDER_AFTER, VOLS_AFTER);
		CurrencyAmount computedPvCall = PRICER.presentValue(CALL_UKI, RATE_PROVIDER_AFTER, VOLS_AFTER);
		CurrencyAmount computedPvPut = PRICER.presentValue(PUT_UKO_BASE, RATE_PROVIDER_AFTER, VOLS_AFTER);
		assertEquals(computedPriceCall, 0d, TOL);
		assertEquals(computedPricePut, 0d, TOL);
		assertEquals(computedPvCall.Amount, 0d, TOL);
		assertEquals(computedPvPut.Amount, 0d, TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_inOutParity()
	  {
		ResolvedFxSingleBarrierOption callDKI = ResolvedFxSingleBarrierOption.of(CALL, BARRIER_DKI);
		ResolvedFxSingleBarrierOption callDKO = ResolvedFxSingleBarrierOption.of(CALL, BARRIER_DKO);
		ResolvedFxSingleBarrierOption callUKI = ResolvedFxSingleBarrierOption.of(CALL, BARRIER_UKI);
		ResolvedFxSingleBarrierOption callUKO = ResolvedFxSingleBarrierOption.of(CALL, BARRIER_UKO);
		ResolvedFxSingleBarrierOption putDKI = ResolvedFxSingleBarrierOption.of(PUT, BARRIER_DKI);
		ResolvedFxSingleBarrierOption putDKO = ResolvedFxSingleBarrierOption.of(PUT, BARRIER_DKO);
		ResolvedFxSingleBarrierOption putUKI = ResolvedFxSingleBarrierOption.of(PUT, BARRIER_UKI);
		ResolvedFxSingleBarrierOption putUKO = ResolvedFxSingleBarrierOption.of(PUT, BARRIER_UKO);
		//pv
		CurrencyAmount pvCall = VANILLA_PRICER.presentValue(CALL, RATE_PROVIDER, VOLS);
		CurrencyAmount computedPvCallUp = PRICER.presentValue(callUKO, RATE_PROVIDER, VOLS).plus(PRICER.presentValue(callUKI, RATE_PROVIDER, VOLS));
		CurrencyAmount computedPvCallDw = PRICER.presentValue(callDKO, RATE_PROVIDER, VOLS).plus(PRICER.presentValue(callDKI, RATE_PROVIDER, VOLS));
		assertEquals(computedPvCallUp.Amount, pvCall.Amount, NOTIONAL * TOL);
		assertEquals(computedPvCallDw.Amount, pvCall.Amount, NOTIONAL * TOL);
		CurrencyAmount pvPut = VANILLA_PRICER.presentValue(PUT, RATE_PROVIDER, VOLS);
		CurrencyAmount computedPvPutUp = PRICER.presentValue(putUKO, RATE_PROVIDER, VOLS).plus(PRICER.presentValue(putUKI, RATE_PROVIDER, VOLS));
		CurrencyAmount computedPvPutDw = PRICER.presentValue(putDKO, RATE_PROVIDER, VOLS).plus(PRICER.presentValue(putDKI, RATE_PROVIDER, VOLS));
		assertEquals(computedPvPutUp.Amount, pvPut.Amount, NOTIONAL * TOL);
		assertEquals(computedPvPutDw.Amount, pvPut.Amount, NOTIONAL * TOL);
		// curve sensitivity
		PointSensitivities pvSensiCall = VANILLA_PRICER.presentValueSensitivityRatesStickyStrike(CALL, RATE_PROVIDER, VOLS);
		PointSensitivities computedPvSensiCallUp = PRICER.presentValueSensitivityRatesStickyStrike(callUKO, RATE_PROVIDER, VOLS).combinedWith(PRICER.presentValueSensitivityRatesStickyStrike(callUKI, RATE_PROVIDER, VOLS)).build();
		PointSensitivities computedPvSensiCallDw = PRICER.presentValueSensitivityRatesStickyStrike(callDKO, RATE_PROVIDER, VOLS).combinedWith(PRICER.presentValueSensitivityRatesStickyStrike(callDKI, RATE_PROVIDER, VOLS)).build();
		assertTrue(RATE_PROVIDER.parameterSensitivity(pvSensiCall).equalWithTolerance(RATE_PROVIDER.parameterSensitivity(computedPvSensiCallUp), TOL * NOTIONAL));
		assertTrue(RATE_PROVIDER.parameterSensitivity(pvSensiCall).equalWithTolerance(RATE_PROVIDER.parameterSensitivity(computedPvSensiCallDw), TOL * NOTIONAL));
		PointSensitivities pvSensiPut = VANILLA_PRICER.presentValueSensitivityRatesStickyStrike(PUT, RATE_PROVIDER, VOLS);
		PointSensitivities computedPvSensiPutUp = PRICER.presentValueSensitivityRatesStickyStrike(putUKO, RATE_PROVIDER, VOLS).combinedWith(PRICER.presentValueSensitivityRatesStickyStrike(putUKI, RATE_PROVIDER, VOLS)).build();
		PointSensitivities computedPvSensiPutDw = PRICER.presentValueSensitivityRatesStickyStrike(putDKO, RATE_PROVIDER, VOLS).combinedWith(PRICER.presentValueSensitivityRatesStickyStrike(putDKI, RATE_PROVIDER, VOLS)).build();
		assertTrue(RATE_PROVIDER.parameterSensitivity(pvSensiPut).equalWithTolerance(RATE_PROVIDER.parameterSensitivity(computedPvSensiPutUp), TOL * NOTIONAL));
		assertTrue(RATE_PROVIDER.parameterSensitivity(pvSensiPut).equalWithTolerance(RATE_PROVIDER.parameterSensitivity(computedPvSensiPutDw), TOL * NOTIONAL));
	  }

	  public virtual void farBarrierOutTest()
	  {
		double smallBarrier = 1.0e-6;
		double largeBarrier = 1.0e6;
		SimpleConstantContinuousBarrier dkoSmall = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_OUT, smallBarrier);
		SimpleConstantContinuousBarrier uKoLarge = SimpleConstantContinuousBarrier.of(BarrierType.UP, KnockType.KNOCK_OUT, largeBarrier);
		ResolvedFxSingleBarrierOption callDko = ResolvedFxSingleBarrierOption.of(CALL, dkoSmall, REBATE);
		ResolvedFxSingleBarrierOption callUko = ResolvedFxSingleBarrierOption.of(CALL, uKoLarge, REBATE_BASE);
		ResolvedFxSingleBarrierOption putDko = ResolvedFxSingleBarrierOption.of(PUT, dkoSmall, REBATE);
		ResolvedFxSingleBarrierOption putUko = ResolvedFxSingleBarrierOption.of(PUT, uKoLarge, REBATE_BASE);
		// pv
		CurrencyAmount pvCallDko = PRICER.presentValue(callDko, RATE_PROVIDER, VOLS);
		CurrencyAmount pvCallUko = PRICER.presentValue(callUko, RATE_PROVIDER, VOLS);
		CurrencyAmount pvCall = VANILLA_PRICER.presentValue(CALL, RATE_PROVIDER, VOLS);
		CurrencyAmount pvPutDko = PRICER.presentValue(putDko, RATE_PROVIDER, VOLS);
		CurrencyAmount pvPutUko = PRICER.presentValue(putUko, RATE_PROVIDER, VOLS);
		CurrencyAmount pvPut = VANILLA_PRICER.presentValue(PUT, RATE_PROVIDER, VOLS);
		assertEquals(pvCallDko.Amount, pvCall.Amount, NOTIONAL * TOL);
		assertEquals(pvCallUko.Amount, pvCall.Amount, NOTIONAL * TOL);
		assertEquals(pvPutDko.Amount, pvPut.Amount, NOTIONAL * TOL);
		assertEquals(pvPutUko.Amount, pvPut.Amount, NOTIONAL * TOL);
		// currency exposure
		MultiCurrencyAmount ceCallDko = PRICER.currencyExposure(callDko, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount ceCallUko = PRICER.currencyExposure(callUko, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount ceCall = VANILLA_PRICER.currencyExposure(CALL, RATE_PROVIDER, VOLS);
		assertEquals(ceCallDko.getAmount(EUR).Amount, ceCall.getAmount(EUR).Amount, NOTIONAL * TOL);
		assertEquals(ceCallDko.getAmount(USD).Amount, ceCall.getAmount(USD).Amount, NOTIONAL * TOL);
		assertEquals(ceCallUko.getAmount(EUR).Amount, ceCall.getAmount(EUR).Amount, NOTIONAL * TOL);
		assertEquals(ceCallUko.getAmount(USD).Amount, ceCall.getAmount(USD).Amount, NOTIONAL * TOL);
		MultiCurrencyAmount cePutDko = PRICER.currencyExposure(putDko, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount cePutUko = PRICER.currencyExposure(putUko, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount cePut = VANILLA_PRICER.currencyExposure(PUT, RATE_PROVIDER, VOLS);
		assertEquals(cePutDko.getAmount(EUR).Amount, cePut.getAmount(EUR).Amount, NOTIONAL * TOL);
		assertEquals(cePutDko.getAmount(USD).Amount, cePut.getAmount(USD).Amount, NOTIONAL * TOL);
		assertEquals(cePutUko.getAmount(EUR).Amount, cePut.getAmount(EUR).Amount, NOTIONAL * TOL);
		assertEquals(cePutUko.getAmount(USD).Amount, cePut.getAmount(USD).Amount, NOTIONAL * TOL);
	  }

	  public virtual void farBarrierInTest()
	  {
		double smallBarrier = 1.0e-6;
		double largeBarrier = 1.0e6;
		SimpleConstantContinuousBarrier dkiSmall = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_IN, smallBarrier);
		SimpleConstantContinuousBarrier uKiLarge = SimpleConstantContinuousBarrier.of(BarrierType.UP, KnockType.KNOCK_IN, largeBarrier);
		ResolvedFxSingleBarrierOption callDki = ResolvedFxSingleBarrierOption.of(CALL, dkiSmall, REBATE);
		ResolvedFxSingleBarrierOption callUki = ResolvedFxSingleBarrierOption.of(CALL, uKiLarge, REBATE_BASE);
		ResolvedFxSingleBarrierOption putDki = ResolvedFxSingleBarrierOption.of(PUT, dkiSmall, REBATE);
		ResolvedFxSingleBarrierOption putUki = ResolvedFxSingleBarrierOption.of(PUT, uKiLarge, REBATE_BASE);
		// pv
		CurrencyAmount pvCallDki = PRICER.presentValue(callDki, RATE_PROVIDER, VOLS);
		CurrencyAmount pvCallUki = PRICER.presentValue(callUki, RATE_PROVIDER, VOLS);
		CurrencyAmount pvPutDki = PRICER.presentValue(putDki, RATE_PROVIDER, VOLS);
		CurrencyAmount pvPutUki = PRICER.presentValue(putUki, RATE_PROVIDER, VOLS);
		double dfUsd = RATE_PROVIDER.discountFactor(USD, PAY_DATE);
		double dfEur = RATE_PROVIDER.discountFactor(EUR, PAY_DATE);
		assertEquals(pvCallDki.Amount, REBATE_AMOUNT * dfUsd, NOTIONAL * TOL);
		assertEquals(pvCallUki.Amount, REBATE_AMOUNT * SPOT * dfEur, NOTIONAL * TOL);
		assertEquals(pvPutDki.Amount, -REBATE_AMOUNT * dfUsd, NOTIONAL * TOL);
		assertEquals(pvPutUki.Amount, -REBATE_AMOUNT * SPOT * dfEur, NOTIONAL * TOL);
		// currency exposure
		MultiCurrencyAmount ceCallDki = PRICER.currencyExposure(callDki, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount ceCallUki = PRICER.currencyExposure(callUki, RATE_PROVIDER, VOLS);
		assertEquals(ceCallDki.getAmount(EUR).Amount, 0d, NOTIONAL * TOL);
		assertEquals(ceCallDki.getAmount(USD).Amount, REBATE_AMOUNT * dfUsd, NOTIONAL * TOL);
		assertEquals(ceCallUki.getAmount(EUR).Amount, REBATE_AMOUNT * dfEur, NOTIONAL * TOL);
		assertEquals(ceCallUki.getAmount(USD).Amount, 0d, NOTIONAL * TOL);
		MultiCurrencyAmount cePutDki = PRICER.currencyExposure(putDki, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount cePutUki = PRICER.currencyExposure(putUki, RATE_PROVIDER, VOLS);
		assertEquals(cePutDki.getAmount(EUR).Amount, 0d, NOTIONAL * TOL);
		assertEquals(cePutDki.getAmount(USD).Amount, -REBATE_AMOUNT * dfUsd, NOTIONAL * TOL);
		assertEquals(cePutUki.getAmount(EUR).Amount, -REBATE_AMOUNT * dfEur, NOTIONAL * TOL);
		assertEquals(cePutUki.getAmount(USD).Amount, 0d, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity()
	  {
		foreach (ResolvedFxSingleBarrierOption option in OPTION_ALL)
		{
		  PointSensitivityBuilder point = PRICER.presentValueSensitivityRatesStickyStrike(option, RATE_PROVIDER, VOLS);
		  CurrencyParameterSensitivities computed = RATE_PROVIDER.parameterSensitivity(point.build());
		  CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATE_PROVIDER, p => PRICER.presentValue(option, p, VOLS));
		  double pvVega = ((FxOptionSensitivity) PRICER.presentValueSensitivityModelParamsVolatility(option, RATE_PROVIDER, VOLS)).Sensitivity;
		  CurrencyParameterSensitivities sensiViaFwd = FD_CAL.sensitivity(RATE_PROVIDER, p => CurrencyAmount.of(USD, VANILLA_PRICER.impliedVolatility(CALL, p, VOLS))).multipliedBy(-pvVega);
		  expected = expected.combinedWith(sensiViaFwd);
		  assertTrue(computed.equalWithTolerance(expected, FD_EPS * NOTIONAL * 10d));
		}
	  }

	  public virtual void test_presentValueSensitivity_atExpiry()
	  {
		foreach (ResolvedFxSingleBarrierOption option in OPTION_ALL)
		{
		  PointSensitivityBuilder point = PRICER.presentValueSensitivityRatesStickyStrike(option, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		  CurrencyParameterSensitivities computed = RATE_PROVIDER_EXPIRY.parameterSensitivity(point.build());
		  CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATE_PROVIDER_EXPIRY, p => PRICER.presentValue(option, p, VOLS_EXPIRY));
		  assertTrue(computed.equalWithTolerance(expected, FD_EPS * NOTIONAL * 10d));
		}
	  }

	  public virtual void test_presentValueSensitivity_afterExpiry()
	  {
		foreach (ResolvedFxSingleBarrierOption option in OPTION_ALL)
		{
		  PointSensitivityBuilder point = PRICER.presentValueSensitivityRatesStickyStrike(option, RATE_PROVIDER_AFTER, VOLS_AFTER);
		  assertEquals(point, PointSensitivityBuilder.none());
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_vega_presentValueSensitivityVolatility()
	  {
		double computedVegaCall = PRICER.vega(CALL_UKI, RATE_PROVIDER, VOLS);
		FxOptionSensitivity computedCall = (FxOptionSensitivity) PRICER.presentValueSensitivityModelParamsVolatility(CALL_UKI, RATE_PROVIDER, VOLS);
		double computedVegaPut = PRICER.vega(PUT_UKO_BASE, RATE_PROVIDER, VOLS);
		FxOptionSensitivity computedPut = (FxOptionSensitivity) PRICER.presentValueSensitivityModelParamsVolatility(PUT_UKO_BASE, RATE_PROVIDER, VOLS);
		double rateBase = RATE_PROVIDER.discountFactors(EUR).zeroRate(PAY_DATE);
		double rateCounter = RATE_PROVIDER.discountFactors(USD).zeroRate(PAY_DATE);
		double costOfCarry = rateCounter - rateBase;
		double forward = RATE_PROVIDER.fxForwardRates(CURRENCY_PAIR).rate(EUR, PAY_DATE);
		double volatility = VOLS.volatility(CURRENCY_PAIR, EXPIRY_DATETIME, STRIKE_RATE, forward);
		double timeToExpiry = VOLS.relativeTime(EXPIRY_DATETIME);
		double rebateRate = REBATE_AMOUNT / NOTIONAL;
		double expectedCash = CASH_REBATE_PRICER.priceAdjoint(SPOT, timeToExpiry, costOfCarry, rateCounter, volatility, BARRIER_UKO).getDerivative(3);
		double expectedAsset = ASSET_REBATE_PRICER.priceAdjoint(SPOT, timeToExpiry, costOfCarry, rateCounter, volatility, BARRIER_UKI).getDerivative(3);
		double expectedCall = BARRIER_PRICER.priceAdjoint(SPOT, STRIKE_RATE, timeToExpiry, costOfCarry, rateCounter, volatility, true, BARRIER_UKI).getDerivative(4) + rebateRate * expectedCash;
		double expectedPut = BARRIER_PRICER.priceAdjoint(SPOT, STRIKE_RATE, timeToExpiry, costOfCarry, rateCounter, volatility, false, BARRIER_UKO).getDerivative(4) + rebateRate * expectedAsset;
		assertEquals(computedVegaCall, expectedCall, TOL);
		assertEquals(computedCall.Sensitivity, expectedCall * NOTIONAL, TOL * NOTIONAL);
		assertEquals(computedCall.Currency, USD);
		assertEquals(computedCall.CurrencyPair, CURRENCY_PAIR);
		assertEquals(computedCall.Strike, STRIKE_RATE);
		assertEquals(computedCall.Forward, forward, TOL);
		assertEquals(computedCall.Expiry, timeToExpiry);
		assertEquals(computedVegaPut, expectedPut, TOL);
		assertEquals(computedPut.Sensitivity, -expectedPut * NOTIONAL, TOL * NOTIONAL);
		assertEquals(computedPut.Currency, USD);
		assertEquals(computedPut.CurrencyPair, CURRENCY_PAIR);
		assertEquals(computedPut.Strike, STRIKE_RATE);
		assertEquals(computedPut.Forward, forward, TOL);
		assertEquals(computedPut.Expiry, timeToExpiry);
	  }

	  public virtual void test_vega_presentValueSensitivityVolatility_atExpiry()
	  {
		double computedVegaCall = PRICER.vega(CALL_UKI, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		PointSensitivityBuilder computedCall = PRICER.presentValueSensitivityModelParamsVolatility(CALL_UKI, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		double computedVegaPut = PRICER.vega(PUT_UKO_BASE, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		PointSensitivityBuilder computedPut = PRICER.presentValueSensitivityModelParamsVolatility(PUT_UKO_BASE, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		assertEquals(computedVegaCall, 0d);
		assertEquals(computedCall, PointSensitivityBuilder.none());
		assertEquals(computedVegaPut, 0d);
		assertEquals(computedPut, PointSensitivityBuilder.none());
	  }

	  public virtual void test_vega_presentValueSensitivityVolatility_afterExpiry()
	  {
		double computedVegaCall = PRICER.vega(CALL_UKI, RATE_PROVIDER_AFTER, VOLS_AFTER);
		PointSensitivityBuilder computedCall = PRICER.presentValueSensitivityModelParamsVolatility(CALL_UKI, RATE_PROVIDER_AFTER, VOLS_AFTER);
		double computedVegaPut = PRICER.vega(PUT_UKO_BASE, RATE_PROVIDER_AFTER, VOLS_AFTER);
		PointSensitivityBuilder computedPut = PRICER.presentValueSensitivityModelParamsVolatility(PUT_UKO_BASE, RATE_PROVIDER_AFTER, VOLS_AFTER);
		assertEquals(computedVegaCall, 0d);
		assertEquals(computedCall, PointSensitivityBuilder.none());
		assertEquals(computedVegaPut, 0d);
		assertEquals(computedPut, PointSensitivityBuilder.none());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currencyExposure()
	  {
		foreach (ResolvedFxSingleBarrierOption option in OPTION_ALL)
		{
		  CurrencyAmount pv = PRICER.presentValue(option, RATE_PROVIDER, VOLS_FLAT);
		  MultiCurrencyAmount computed = PRICER.currencyExposure(option, RATE_PROVIDER, VOLS_FLAT);
		  FxMatrix fxMatrix = FxMatrix.builder().addRate(EUR, USD, SPOT + FD_EPS).build();
		  ImmutableRatesProvider provBumped = RATE_PROVIDER.toBuilder().fxRateProvider(fxMatrix).build();
		  CurrencyAmount pvBumped = PRICER.presentValue(option, provBumped, VOLS_FLAT);
		  double ceCounterFD = pvBumped.Amount - pv.Amount;
		  double ceBaseFD = pvBumped.Amount / (SPOT + FD_EPS) - pv.Amount / SPOT;
		  assertEquals(computed.getAmount(EUR).Amount * FD_EPS, ceCounterFD, NOTIONAL * TOL);
		  assertEquals(computed.getAmount(USD).Amount * (1.0d / (SPOT + FD_EPS) - 1.0d / SPOT), ceBaseFD, NOTIONAL * TOL);
		}
	  }

	  public virtual void test_currencyExposure_atExpiry()
	  {
		foreach (ResolvedFxSingleBarrierOption option in OPTION_ALL)
		{
		  CurrencyAmount pv = PRICER.presentValue(option, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		  MultiCurrencyAmount computed = PRICER.currencyExposure(option, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		  FxMatrix fxMatrix = FxMatrix.builder().addRate(EUR, USD, SPOT + FD_EPS).build();
		  ImmutableRatesProvider provBumped = RATE_PROVIDER_EXPIRY.toBuilder().fxRateProvider(fxMatrix).build();
		  CurrencyAmount pvBumped = PRICER.presentValue(option, provBumped, VOLS_EXPIRY);
		  double ceCounterFD = pvBumped.Amount - pv.Amount;
		  double ceBaseFD = pvBumped.Amount / (SPOT + FD_EPS) - pv.Amount / SPOT;
		  assertEquals(computed.getAmount(EUR).Amount * FD_EPS, ceCounterFD, NOTIONAL * TOL);
		  assertEquals(computed.getAmount(USD).Amount * (1.0d / (SPOT + FD_EPS) - 1.0d / SPOT), ceBaseFD, NOTIONAL * TOL);
		}
	  }

	  public virtual void test_currencyExposure_afterExpiry()
	  {
		foreach (ResolvedFxSingleBarrierOption option in OPTION_ALL)
		{
		  MultiCurrencyAmount computed = PRICER.currencyExposure(option, RATE_PROVIDER_AFTER, VOLS_AFTER);
		  assertEquals(computed, MultiCurrencyAmount.empty());
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_delta_presentValueDelta()
	  {
		double computedDeltaCall = PRICER.delta(CALL_UKI, RATE_PROVIDER, VOLS);
		double computedDeltaPut = PRICER.delta(PUT_UKO_BASE, RATE_PROVIDER, VOLS);
		CurrencyAmount computedPvDeltaCall = PRICER.presentValueDelta(CALL_UKI, RATE_PROVIDER, VOLS);
		CurrencyAmount computedPvDeltaPut = PRICER.presentValueDelta(PUT_UKO_BASE, RATE_PROVIDER, VOLS);
		double rateBase = RATE_PROVIDER.discountFactors(EUR).zeroRate(PAY_DATE);
		double rateCounter = RATE_PROVIDER.discountFactors(USD).zeroRate(PAY_DATE);
		double costOfCarry = rateCounter - rateBase;
		double forward = RATE_PROVIDER.fxForwardRates(CURRENCY_PAIR).rate(EUR, PAY_DATE);
		double volatility = VOLS.volatility(CURRENCY_PAIR, EXPIRY_DATETIME, STRIKE_RATE, forward);
		double timeToExpiry = VOLS.relativeTime(EXPIRY_DATETIME);
		double rebateRate = REBATE_AMOUNT / NOTIONAL;
		double expectedCash = CASH_REBATE_PRICER.priceAdjoint(SPOT, timeToExpiry, costOfCarry, rateCounter, volatility, BARRIER_UKO).getDerivative(0);
		double expectedAsset = ASSET_REBATE_PRICER.priceAdjoint(SPOT, timeToExpiry, costOfCarry, rateCounter, volatility, BARRIER_UKI).getDerivative(0);
		double expectedDeltaCall = BARRIER_PRICER.priceAdjoint(SPOT, STRIKE_RATE, timeToExpiry, costOfCarry, rateCounter, volatility, true, BARRIER_UKI).getDerivative(0) + rebateRate * expectedCash;
		double expectedDeltaPut = BARRIER_PRICER.priceAdjoint(SPOT, STRIKE_RATE, timeToExpiry, costOfCarry, rateCounter, volatility, false, BARRIER_UKO).getDerivative(0) + rebateRate * expectedAsset;
		assertEquals(computedDeltaCall, expectedDeltaCall, TOL);
		assertEquals(computedDeltaPut, expectedDeltaPut, TOL);
		assertEquals(computedPvDeltaCall.Currency, USD);
		assertEquals(computedPvDeltaPut.Currency, USD);
		assertEquals(computedPvDeltaCall.Amount, expectedDeltaCall * NOTIONAL, TOL);
		assertEquals(computedPvDeltaPut.Amount, -expectedDeltaPut * NOTIONAL, TOL);
	  }

	  public virtual void test_delta_presentValueDelta_atExpiry()
	  {
		double computedDeltaCall = PRICER.delta(CALL_UKI, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		double computedDeltaPut = PRICER.delta(PUT_UKO_BASE, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		CurrencyAmount computedPvDeltaCall = PRICER.presentValueDelta(CALL_UKI, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		CurrencyAmount computedPvDeltaPut = PRICER.presentValueDelta(PUT_UKO_BASE, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		double expectedDeltaPut = -1d;
		assertEquals(computedDeltaCall, 0d, TOL);
		assertEquals(computedDeltaPut, expectedDeltaPut, TOL);
		assertEquals(computedPvDeltaCall.Amount, 0d, TOL);
		assertEquals(computedPvDeltaPut.Amount, -expectedDeltaPut * NOTIONAL, TOL);
	  }

	  public virtual void test_delta_presentValueDelta_afterExpiry()
	  {
		double computedDeltaCall = PRICER.delta(CALL_UKI, RATE_PROVIDER_AFTER, VOLS_AFTER);
		double computedDeltaPut = PRICER.delta(PUT_UKO_BASE, RATE_PROVIDER_AFTER, VOLS_AFTER);
		CurrencyAmount computedPvDeltaCall = PRICER.presentValueDelta(CALL_UKI, RATE_PROVIDER_AFTER, VOLS_AFTER);
		CurrencyAmount computedPvDeltaPut = PRICER.presentValueDelta(PUT_UKO_BASE, RATE_PROVIDER_AFTER, VOLS_AFTER);
		assertEquals(computedDeltaCall, 0d, TOL);
		assertEquals(computedDeltaPut, 0d, TOL);
		assertEquals(computedPvDeltaCall.Amount, 0d, TOL);
		assertEquals(computedPvDeltaPut.Amount, 0d, TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_gamma_presentValueGamma()
	  {
		double computedGammaCall = PRICER.gamma(CALL_UKI, RATE_PROVIDER, VOLS);
		double computedGammaPut = PRICER.gamma(PUT_UKO_BASE, RATE_PROVIDER, VOLS);
		CurrencyAmount computedPvGammaCall = PRICER.presentValueGamma(CALL_UKI, RATE_PROVIDER, VOLS);
		CurrencyAmount computedPvGammaPut = PRICER.presentValueGamma(PUT_UKO_BASE, RATE_PROVIDER, VOLS);
		double rateBase = RATE_PROVIDER.discountFactors(EUR).zeroRate(PAY_DATE);
		double rateCounter = RATE_PROVIDER.discountFactors(USD).zeroRate(PAY_DATE);
		double costOfCarry = rateCounter - rateBase;
		double forward = RATE_PROVIDER.fxForwardRates(CURRENCY_PAIR).rate(EUR, PAY_DATE);
		double volatility = VOLS.volatility(CURRENCY_PAIR, EXPIRY_DATETIME, STRIKE_RATE, forward);
		double timeToExpiry = VOLS.relativeTime(EXPIRY_DATETIME);
		double rebateRate = REBATE_AMOUNT / NOTIONAL;
		double expectedCash = CASH_REBATE_PRICER.priceAdjoint(SPOT, timeToExpiry, costOfCarry, rateCounter, volatility, BARRIER_UKO).getDerivative(5);
		double expectedAsset = ASSET_REBATE_PRICER.priceAdjoint(SPOT, timeToExpiry, costOfCarry, rateCounter, volatility, BARRIER_UKI).getDerivative(5);
		double expectedGammaCall = BARRIER_PRICER.priceAdjoint(SPOT, STRIKE_RATE, timeToExpiry, costOfCarry, rateCounter, volatility, true, BARRIER_UKI).getDerivative(6) + rebateRate * expectedCash;
		double expectedGammaPut = BARRIER_PRICER.priceAdjoint(SPOT, STRIKE_RATE, timeToExpiry, costOfCarry, rateCounter, volatility, false, BARRIER_UKO).getDerivative(6) + rebateRate * expectedAsset;
		assertEquals(computedGammaCall, expectedGammaCall, TOL);
		assertEquals(computedGammaPut, expectedGammaPut, TOL);
		assertEquals(computedPvGammaCall.Currency, USD);
		assertEquals(computedPvGammaPut.Currency, USD);
		assertEquals(computedPvGammaCall.Amount, expectedGammaCall * NOTIONAL, TOL);
		assertEquals(computedPvGammaPut.Amount, -expectedGammaPut * NOTIONAL, TOL);
	  }

	  public virtual void test_gamma_presentValueGamma_atExpiry()
	  {
		double computedGammaCall = PRICER.gamma(CALL_UKI, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		double computedGammaPut = PRICER.gamma(PUT_UKO_BASE, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		CurrencyAmount computedPvGammaCall = PRICER.presentValueGamma(CALL_UKI, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		CurrencyAmount computedPvGammaPut = PRICER.presentValueGamma(PUT_UKO_BASE, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		assertEquals(computedGammaCall, 0d, TOL);
		assertEquals(computedGammaPut, 0d, TOL);
		assertEquals(computedPvGammaCall.Amount, 0d, TOL);
		assertEquals(computedPvGammaPut.Amount, 0d, TOL);
	  }

	  public virtual void test_gamma_presentValueGamma_afterExpiry()
	  {
		double computedGammaCall = PRICER.gamma(CALL_UKI, RATE_PROVIDER_AFTER, VOLS_AFTER);
		double computedGammaPut = PRICER.gamma(PUT_UKO_BASE, RATE_PROVIDER_AFTER, VOLS_AFTER);
		CurrencyAmount computedPvGammaCall = PRICER.presentValueGamma(CALL_UKI, RATE_PROVIDER_AFTER, VOLS_AFTER);
		CurrencyAmount computedPvGammaPut = PRICER.presentValueGamma(PUT_UKO_BASE, RATE_PROVIDER_AFTER, VOLS_AFTER);
		assertEquals(computedGammaCall, 0d, TOL);
		assertEquals(computedGammaPut, 0d, TOL);
		assertEquals(computedPvGammaCall.Amount, 0d, TOL);
		assertEquals(computedPvGammaPut.Amount, 0d, TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_theta_presentValueTheta()
	  {
		double computedThetaCall = PRICER.theta(CALL_UKI, RATE_PROVIDER, VOLS);
		double computedThetaPut = PRICER.theta(PUT_UKO_BASE, RATE_PROVIDER, VOLS);
		CurrencyAmount computedPvThetaCall = PRICER.presentValueTheta(CALL_UKI, RATE_PROVIDER, VOLS);
		CurrencyAmount computedPvThetaPut = PRICER.presentValueTheta(PUT_UKO_BASE, RATE_PROVIDER, VOLS);
		double rateBase = RATE_PROVIDER.discountFactors(EUR).zeroRate(PAY_DATE);
		double rateCounter = RATE_PROVIDER.discountFactors(USD).zeroRate(PAY_DATE);
		double costOfCarry = rateCounter - rateBase;
		double forward = RATE_PROVIDER.fxForwardRates(CURRENCY_PAIR).rate(EUR, PAY_DATE);
		double volatility = VOLS.volatility(CURRENCY_PAIR, EXPIRY_DATETIME, STRIKE_RATE, forward);
		double timeToExpiry = VOLS.relativeTime(EXPIRY_DATETIME);
		double rebateRate = REBATE_AMOUNT / NOTIONAL;
		double expectedCash = CASH_REBATE_PRICER.priceAdjoint(SPOT, timeToExpiry, costOfCarry, rateCounter, volatility, BARRIER_UKO).getDerivative(4);
		double expectedAsset = ASSET_REBATE_PRICER.priceAdjoint(SPOT, timeToExpiry, costOfCarry, rateCounter, volatility, BARRIER_UKI).getDerivative(4);
		double expectedThetaCall = BARRIER_PRICER.priceAdjoint(SPOT, STRIKE_RATE, timeToExpiry, costOfCarry, rateCounter, volatility, true, BARRIER_UKI).getDerivative(5) + rebateRate * expectedCash;
		double expectedThetaPut = BARRIER_PRICER.priceAdjoint(SPOT, STRIKE_RATE, timeToExpiry, costOfCarry, rateCounter, volatility, false, BARRIER_UKO).getDerivative(5) + rebateRate * expectedAsset;
		expectedThetaCall *= -1d;
		expectedThetaPut *= -1d;
		assertEquals(computedThetaCall, expectedThetaCall, TOL);
		assertEquals(computedThetaPut, expectedThetaPut, TOL);
		assertEquals(computedPvThetaCall.Currency, USD);
		assertEquals(computedPvThetaPut.Currency, USD);
		assertEquals(computedPvThetaCall.Amount, expectedThetaCall * NOTIONAL, TOL);
		assertEquals(computedPvThetaPut.Amount, -expectedThetaPut * NOTIONAL, TOL);
	  }

	  public virtual void test_theta_presentValueTheta_atExpiry()
	  {
		double computedThetaCall = PRICER.theta(CALL_UKI, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		double computedThetaPut = PRICER.theta(PUT_UKO_BASE, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		CurrencyAmount computedPvThetaCall = PRICER.presentValueTheta(CALL_UKI, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		CurrencyAmount computedPvThetaPut = PRICER.presentValueTheta(PUT_UKO_BASE, RATE_PROVIDER_EXPIRY, VOLS_EXPIRY);
		double rateBase = RATE_PROVIDER_EXPIRY.discountFactors(EUR).zeroRate(PAY_DATE);
		double rateCounter = RATE_PROVIDER_EXPIRY.discountFactors(USD).zeroRate(PAY_DATE);
		double expectedThetaCall = -(REBATE_AMOUNT / NOTIONAL) * rateCounter;
		double expectedThetaPut = -rateCounter * STRIKE_RATE + rateBase * SPOT;
		expectedThetaCall *= -1d;
		expectedThetaPut *= -1d;
		assertEquals(computedThetaCall, expectedThetaCall, TOL);
		assertEquals(computedThetaPut, expectedThetaPut, TOL);
		assertEquals(computedPvThetaCall.Amount, expectedThetaCall * NOTIONAL, TOL * NOTIONAL);
		assertEquals(computedPvThetaPut.Amount, -expectedThetaPut * NOTIONAL, TOL);
	  }

	  public virtual void test_theta_presentValueTheta_afterExpiry()
	  {
		double computedThetaCall = PRICER.theta(CALL_UKI, RATE_PROVIDER_AFTER, VOLS_AFTER);
		double computedThetaPut = PRICER.theta(PUT_UKO_BASE, RATE_PROVIDER_AFTER, VOLS_AFTER);
		CurrencyAmount computedPvThetaCall = PRICER.presentValueTheta(CALL_UKI, RATE_PROVIDER_AFTER, VOLS_AFTER);
		CurrencyAmount computedPvThetaPut = PRICER.presentValueTheta(PUT_UKO_BASE, RATE_PROVIDER_AFTER, VOLS_AFTER);
		assertEquals(computedThetaCall, 0d, TOL);
		assertEquals(computedThetaPut, 0d, TOL);
		assertEquals(computedPvThetaCall.Amount, 0d, TOL);
		assertEquals(computedPvThetaPut.Amount, 0d, TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void regression_pv()
	  {
		CurrencyAmount pv = PRICER.presentValue(CALL_DKI, RATE_PROVIDER, VOLS);
		assertEquals(pv.Amount, 9035006.129433425, NOTIONAL * TOL);
		CurrencyAmount pvBase = PRICER.presentValue(CALL_DKI_BASE, RATE_PROVIDER, VOLS);
		assertEquals(pvBase.Amount, 9038656.396419544, NOTIONAL * TOL); // UI put on USD/EUR rate with FX conversion in 2.x
		CurrencyAmount pvPut = PRICER.presentValue(PUT_DKO, RATE_PROVIDER, VOLS);
		assertEquals(pvPut.Amount, -55369.48871310125, NOTIONAL * TOL);
		CurrencyAmount pvPutBase = PRICER.presentValue(PUT_DKO_BASE, RATE_PROVIDER, VOLS);
		assertEquals(pvPutBase.Amount, -71369.96172030675, NOTIONAL * TOL); // UI call on USD/EUR rate with FX conversion in 2.x
	  }

	  public virtual void regression_curveSensitivity()
	  {
		PointSensitivityBuilder point = PRICER.presentValueSensitivityRatesStickyStrike(CALL_DKI, RATE_PROVIDER, VOLS);
		CurrencyParameterSensitivities pvSensi = RATE_PROVIDER.parameterSensitivity(point.build());
		double[] eurSensi = new double[] {0.0, 0.0, 0.0, -8.23599758653779E7, -5.943903918586236E7};
		double[] usdSensi = new double[] {0.0, 0.0, 0.0, 6.526531701730868E7, 4.710185614928411E7};
		assertTrue(DoubleArrayMath.fuzzyEquals(eurSensi, pvSensi.getSensitivity(RatesProviderFxDataSets.getCurveName(EUR), USD).Sensitivity.toArray(), NOTIONAL * TOL));
		assertTrue(DoubleArrayMath.fuzzyEquals(usdSensi, pvSensi.getSensitivity(RatesProviderFxDataSets.getCurveName(USD), USD).Sensitivity.toArray(), NOTIONAL * TOL));
		PointSensitivityBuilder pointBase = PRICER.presentValueSensitivityRatesStickyStrike(CALL_DKI_BASE, RATE_PROVIDER, VOLS);
		CurrencyParameterSensitivities pvSensiBase = RATE_PROVIDER.parameterSensitivity(pointBase.build()).convertedTo(EUR, RATE_PROVIDER);
		double[] eurSensiBase = new double[] {0.0, 0.0, 0.0, -5.885393657463378E7, -4.247477498074986E7};
		double[] usdSensiBase = new double[] {0.0, 0.0, 0.0, 4.663853277047497E7, 3.365894110322015E7};
		assertTrue(DoubleArrayMath.fuzzyEquals(eurSensiBase, pvSensiBase.getSensitivity(RatesProviderFxDataSets.getCurveName(EUR), EUR).Sensitivity.toArray(), NOTIONAL * TOL));
		assertTrue(DoubleArrayMath.fuzzyEquals(usdSensiBase, pvSensiBase.getSensitivity(RatesProviderFxDataSets.getCurveName(USD), EUR).Sensitivity.toArray(), NOTIONAL * TOL));
		PointSensitivityBuilder pointPut = PRICER.presentValueSensitivityRatesStickyStrike(PUT_DKO, RATE_PROVIDER, VOLS).multipliedBy(-1d);
		CurrencyParameterSensitivities pvSensiPut = RATE_PROVIDER.parameterSensitivity(pointPut.build());
		double[] eurSensiPut = new double[] {0.0, 0.0, 0.0, 22176.623866383557, 16004.827601682477};
		double[] usdSensiPut = new double[] {0.0, 0.0, 0.0, -48509.60688347871, -35009.29176024644};
		assertTrue(DoubleArrayMath.fuzzyEquals(eurSensiPut, pvSensiPut.getSensitivity(RatesProviderFxDataSets.getCurveName(EUR), USD).Sensitivity.toArray(), NOTIONAL * TOL));
		assertTrue(DoubleArrayMath.fuzzyEquals(usdSensiPut, pvSensiPut.getSensitivity(RatesProviderFxDataSets.getCurveName(USD), USD).Sensitivity.toArray(), NOTIONAL * TOL));
		PointSensitivityBuilder pointPutBase = PRICER.presentValueSensitivityRatesStickyStrike(PUT_DKO_BASE, RATE_PROVIDER, VOLS).multipliedBy(-1d);
		CurrencyParameterSensitivities pvSensiPutBase = RATE_PROVIDER.parameterSensitivity(pointPutBase.build()).convertedTo(EUR, RATE_PROVIDER);
		double[] eurSensiPutBase = new double[] {0.0, 0.0, 0.0, 24062.637495868825, 17365.96007956571};
		double[] usdSensiPutBase = new double[] {0.0, 0.0, 0.0, -44888.77092190999, -32396.141278548253};
		assertTrue(DoubleArrayMath.fuzzyEquals(eurSensiPutBase, pvSensiPutBase.getSensitivity(RatesProviderFxDataSets.getCurveName(EUR), EUR).Sensitivity.toArray(), NOTIONAL * TOL));
		assertTrue(DoubleArrayMath.fuzzyEquals(usdSensiPutBase, pvSensiPutBase.getSensitivity(RatesProviderFxDataSets.getCurveName(USD), EUR).Sensitivity.toArray(), NOTIONAL * TOL));
	  }

	  public virtual void regression_volSensitivity()
	  {
		PointSensitivityBuilder point = PRICER.presentValueSensitivityModelParamsVolatility(CALL_DKI, RATE_PROVIDER, VOLS);
		CurrencyParameterSensitivity pvSensi = VOLS.parameterSensitivity((FxOptionSensitivity) point).Sensitivities.get(0);
		PointSensitivityBuilder pointBase = PRICER.presentValueSensitivityModelParamsVolatility(CALL_DKI_BASE, RATE_PROVIDER, VOLS);
		CurrencyParameterSensitivity pvSensiBase = VOLS.parameterSensitivity((FxOptionSensitivity) pointBase).convertedTo(EUR, RATE_PROVIDER).Sensitivities.get(0);
		PointSensitivityBuilder pointPut = PRICER.presentValueSensitivityModelParamsVolatility(PUT_DKO, RATE_PROVIDER, VOLS).multipliedBy(-1d);
		CurrencyParameterSensitivity pvSensiPut = VOLS.parameterSensitivity((FxOptionSensitivity) pointPut).Sensitivities.get(0);
		PointSensitivityBuilder pointPutBase = PRICER.presentValueSensitivityModelParamsVolatility(PUT_DKO_BASE, RATE_PROVIDER, VOLS).multipliedBy(-1d);
		CurrencyParameterSensitivity pvSensiPutBase = VOLS.parameterSensitivity((FxOptionSensitivity) pointPutBase).convertedTo(EUR, RATE_PROVIDER).Sensitivities.get(0);
		double[] computed = pvSensi.Sensitivity.toArray();
		double[] computedBase = pvSensiBase.Sensitivity.toArray();
		double[] computedPut = pvSensiPut.Sensitivity.toArray();
		double[] computedPutBase = pvSensiPutBase.Sensitivity.toArray();
		double[][] expected = new double[][]
		{
			new double[] {0.0, 0.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 0.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 0.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 0.0, 3.154862889936005E7, 186467.57005640838, 0.0},
			new double[] {0.0, 0.0, 5.688931113627187E7, 336243.18963600876, 0.0}
		};
		double[][] expectedBase = new double[][]
		{
			new double[] {0.0, 0.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 0.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 0.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 0.0, 2.2532363577178854E7, 133177.10564432456, 0.0},
			new double[] {0.0, 0.0, 4.063094615828866E7, 240148.4331822043, 0.0}
		};
		double[][] expectedPut = new double[][]
		{
			new double[] {-0.0, -0.0, -0.0, -0.0, -0.0},
			new double[] {-0.0, -0.0, -0.0, -0.0, -0.0},
			new double[] {-0.0, -0.0, -0.0, -0.0, -0.0},
			new double[] {-0.0, -0.0, -53011.143048566446, -313.32135103910525, -0.0},
			new double[] {-0.0, -0.0, -95591.07688006328, -564.989238732409, -0.0}
		};
		double[][] expectedPutBase = new double[][]
		{
			new double[] {-0.0, -0.0, -0.0, -0.0, -0.0},
			new double[] {-0.0, -0.0, -0.0, -0.0, -0.0},
			new double[] {-0.0, -0.0, -0.0, -0.0, -0.0},
			new double[] {-0.0, -0.0, -35148.33541137355, -207.743566815316, -0.0},
			new double[] {-0.0, -0.0, -63380.39588085656, -374.6086223530026, -0.0}
		};
		for (int i = 0; i < computed.Length; ++i)
		{
		  int row = i / 5;
		  int col = i % 5;
		  assertTrue(DoubleMath.fuzzyEquals(computed[i], expected[row][col], NOTIONAL * TOL));
		  assertTrue(DoubleMath.fuzzyEquals(computedBase[i], expectedBase[row][col], NOTIONAL * TOL));
		  assertTrue(DoubleMath.fuzzyEquals(computedPut[i], expectedPut[row][col], NOTIONAL * TOL));
		  assertTrue(DoubleMath.fuzzyEquals(computedPutBase[i], expectedPutBase[row][col], NOTIONAL * TOL));
		}
	  }

	  public virtual void regression_currencyExposure()
	  {
		MultiCurrencyAmount pv = PRICER.currencyExposure(CALL_DKI, RATE_PROVIDER, VOLS);
		assertEquals(pv.getAmount(EUR).Amount, -2.8939530642669797E7, NOTIONAL * TOL);
		assertEquals(pv.getAmount(USD).Amount, 4.955034902917114E7, NOTIONAL * TOL);
		MultiCurrencyAmount pvBase = PRICER.currencyExposure(CALL_DKI_BASE, RATE_PROVIDER, VOLS);
		assertEquals(pvBase.getAmount(EUR).Amount, -2.8866459583853487E7, NOTIONAL * TOL);
		assertEquals(pvBase.getAmount(USD).Amount, 4.9451699813814424E7, NOTIONAL * TOL);
		MultiCurrencyAmount pvPut = PRICER.currencyExposure(PUT_DKO, RATE_PROVIDER, VOLS);
		assertEquals(pvPut.getAmount(EUR).Amount, -105918.46956467835, NOTIONAL * TOL);
		assertEquals(pvPut.getAmount(USD).Amount, 92916.36867744842, NOTIONAL * TOL);
		MultiCurrencyAmount pvPutBase = PRICER.currencyExposure(PUT_DKO_BASE, RATE_PROVIDER, VOLS);
		assertEquals(pvPutBase.getAmount(EUR).Amount, -76234.66256109312, NOTIONAL * TOL);
		assertEquals(pvPutBase.getAmount(USD).Amount, 35358.56586522361, NOTIONAL * TOL);
	  }

	}

}