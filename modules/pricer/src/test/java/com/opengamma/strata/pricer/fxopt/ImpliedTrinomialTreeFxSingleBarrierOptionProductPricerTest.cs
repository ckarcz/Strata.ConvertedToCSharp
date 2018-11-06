using System;

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
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using RatesProviderFxDataSets = com.opengamma.strata.pricer.fx.RatesProviderFxDataSets;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using LongShort = com.opengamma.strata.product.common.LongShort;
	using ResolvedFxSingle = com.opengamma.strata.product.fx.ResolvedFxSingle;
	using ResolvedFxSingleBarrierOption = com.opengamma.strata.product.fxopt.ResolvedFxSingleBarrierOption;
	using ResolvedFxSingleBarrierOptionTrade = com.opengamma.strata.product.fxopt.ResolvedFxSingleBarrierOptionTrade;
	using ResolvedFxVanillaOption = com.opengamma.strata.product.fxopt.ResolvedFxVanillaOption;
	using BarrierType = com.opengamma.strata.product.option.BarrierType;
	using KnockType = com.opengamma.strata.product.option.KnockType;
	using SimpleConstantContinuousBarrier = com.opengamma.strata.product.option.SimpleConstantContinuousBarrier;

	/// <summary>
	/// Test <seealso cref="ImpliedTrinomialTreeFxSingleBarrierOptionProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ImpliedTrinomialTreeFxSingleBarrierOptionProductPricerTest
	public class ImpliedTrinomialTreeFxSingleBarrierOptionProductPricerTest
	{

	  private static readonly ZoneId ZONE = ZoneId.of("Z");
	  private static readonly LocalDate VAL_DATE = LocalDate.of(2011, 6, 13);
	  private static readonly ZonedDateTime VAL_DATETIME = VAL_DATE.atStartOfDay(ZONE);
	  private static readonly LocalDate PAY_DATE = LocalDate.of(2014, 9, 15);
	  private static readonly LocalDate EXPIRY_DATE = LocalDate.of(2014, 9, 15);
	  private static readonly ZonedDateTime EXPIRY_DATETIME = EXPIRY_DATE.atStartOfDay(ZONE);
	  // providers - flat
	  private static readonly ImmutableRatesProvider RATE_PROVIDER_FLAT = RatesProviderFxDataSets.createProviderEurUsdFlat(VAL_DATE);
	  private static readonly BlackFxOptionSmileVolatilities VOLS_FLAT = FxVolatilitySmileDataSet.createVolatilitySmileProvider5FlatFlat(VAL_DATETIME);
	  // providers
	  private static readonly ImmutableRatesProvider RATE_PROVIDER = RatesProviderFxDataSets.createProviderEURUSD(VAL_DATE);
	  private static readonly BlackFxOptionSmileVolatilities VOLS = FxVolatilitySmileDataSet.createVolatilitySmileProvider5(VAL_DATETIME);
	  // providers - after maturity
	  private static readonly ImmutableRatesProvider RATE_PROVIDER_AFTER = RatesProviderFxDataSets.createProviderEURUSD(EXPIRY_DATE.plusDays(1));
	  private static readonly BlackFxOptionSmileVolatilities VOLS_AFTER = FxVolatilitySmileDataSet.createVolatilitySmileProvider5(EXPIRY_DATETIME.plusDays(1));

	  private const double NOTIONAL = 100_000_000d;
	  private const double LEVEL_LOW = 1.25;
	  private const double LEVEL_HIGH = 1.6;
	  private static readonly SimpleConstantContinuousBarrier BARRIER_DKO = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_OUT, LEVEL_LOW);
	  private static readonly SimpleConstantContinuousBarrier BARRIER_UKI = SimpleConstantContinuousBarrier.of(BarrierType.UP, KnockType.KNOCK_IN, LEVEL_HIGH);
	  private const double REBATE_AMOUNT = 5_000_000d; // large rebate for testing
	  private static readonly CurrencyAmount REBATE = CurrencyAmount.of(USD, REBATE_AMOUNT);
	  private static readonly CurrencyAmount REBATE_BASE = CurrencyAmount.of(EUR, REBATE_AMOUNT);
	  private const double STRIKE_RATE_HIGH = 1.45;
	  private const double STRIKE_RATE_LOW = 1.35;
	  // call
	  private static readonly CurrencyAmount EUR_AMOUNT_REC = CurrencyAmount.of(EUR, NOTIONAL);
	  private static readonly CurrencyAmount USD_AMOUNT_PAY = CurrencyAmount.of(USD, -NOTIONAL * STRIKE_RATE_LOW);
	  private static readonly ResolvedFxSingle FX_PRODUCT = ResolvedFxSingle.of(EUR_AMOUNT_REC, USD_AMOUNT_PAY, PAY_DATE);
	  private static readonly ResolvedFxVanillaOption CALL = ResolvedFxVanillaOption.builder().longShort(LongShort.LONG).expiry(EXPIRY_DATETIME).underlying(FX_PRODUCT).build();
	  private static readonly CurrencyAmount EUR_AMOUNT_PAY = CurrencyAmount.of(EUR, -NOTIONAL);
	  private static readonly CurrencyAmount USD_AMOUNT_REC = CurrencyAmount.of(USD, NOTIONAL * STRIKE_RATE_HIGH);
	  private static readonly ResolvedFxSingle FX_PRODUCT_INV = ResolvedFxSingle.of(EUR_AMOUNT_PAY, USD_AMOUNT_REC, PAY_DATE);
	  private static readonly ResolvedFxVanillaOption PUT = ResolvedFxVanillaOption.builder().longShort(LongShort.SHORT).expiry(EXPIRY_DATETIME).underlying(FX_PRODUCT_INV).build();
	  private static readonly ResolvedFxSingleBarrierOption CALL_DKO = ResolvedFxSingleBarrierOption.of(CALL, BARRIER_DKO);
	  private static readonly ResolvedFxSingleBarrierOption CALL_UKI_C = ResolvedFxSingleBarrierOption.of(CALL, BARRIER_UKI, REBATE);
	  // pricers and pre-calibration
	  private static readonly ImpliedTrinomialTreeFxSingleBarrierOptionProductPricer PRICER_39 = new ImpliedTrinomialTreeFxSingleBarrierOptionProductPricer(39);
	  private static readonly RecombiningTrinomialTreeData DATA_39 = PRICER_39.Calibrator.calibrateTrinomialTree(CALL, RATE_PROVIDER, VOLS);
	  private static readonly ImpliedTrinomialTreeFxSingleBarrierOptionTradePricer TRADE_PRICER_39 = new ImpliedTrinomialTreeFxSingleBarrierOptionTradePricer(PRICER_39, DiscountingPaymentPricer.DEFAULT);

	  private static readonly ImpliedTrinomialTreeFxSingleBarrierOptionProductPricer PRICER_70 = new ImpliedTrinomialTreeFxSingleBarrierOptionProductPricer(70);
	  private static readonly RecombiningTrinomialTreeData DATA_70_FLAT = PRICER_70.Calibrator.calibrateTrinomialTree(CALL, RATE_PROVIDER_FLAT, VOLS_FLAT);
	  private static readonly BlackFxSingleBarrierOptionProductPricer BLACK_PRICER = BlackFxSingleBarrierOptionProductPricer.DEFAULT;
	  private static readonly BlackFxVanillaOptionProductPricer VANILLA_PRICER = BlackFxVanillaOptionProductPricer.DEFAULT;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test_black()
	  public virtual void test_black()
	  {
		double tol = 1.0e-2;
		for (int i = 0; i < 11; ++i)
		{
		  // up barrier
		  double lowerBarrier = 1.1 + 0.025 * i;
		  SimpleConstantContinuousBarrier dko = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_OUT, lowerBarrier);
		  ResolvedFxSingleBarrierOption optionDko = ResolvedFxSingleBarrierOption.of(CALL, dko);
		  double priceDkoBlack = BLACK_PRICER.price(optionDko, RATE_PROVIDER_FLAT, VOLS_FLAT);
		  double priceDko = PRICER_70.price(optionDko, RATE_PROVIDER_FLAT, VOLS_FLAT, DATA_70_FLAT);
		  assertEqualsRelative(priceDko, priceDkoBlack, tol);
		  SimpleConstantContinuousBarrier dki = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_IN, lowerBarrier);
		  ResolvedFxSingleBarrierOption optionDki = ResolvedFxSingleBarrierOption.of(CALL, dki);
		  double priceDkiBlack = BLACK_PRICER.price(optionDki, RATE_PROVIDER_FLAT, VOLS_FLAT);
		  double priceDki = PRICER_70.price(optionDki, RATE_PROVIDER_FLAT, VOLS_FLAT, DATA_70_FLAT);
		  assertEqualsRelative(priceDki, priceDkiBlack, tol);
		  // down barrier
		  double higherBarrier = 1.45 + 0.025 * i;
		  SimpleConstantContinuousBarrier uko = SimpleConstantContinuousBarrier.of(BarrierType.UP, KnockType.KNOCK_OUT, higherBarrier);
		  ResolvedFxSingleBarrierOption optionUko = ResolvedFxSingleBarrierOption.of(CALL, uko);
		  double priceUkoBlack = BLACK_PRICER.price(optionUko, RATE_PROVIDER_FLAT, VOLS_FLAT);
		  double priceUko = PRICER_70.price(optionUko, RATE_PROVIDER_FLAT, VOLS_FLAT, DATA_70_FLAT);
		  assertEqualsRelative(priceUko, priceUkoBlack, tol);
		  SimpleConstantContinuousBarrier uki = SimpleConstantContinuousBarrier.of(BarrierType.UP, KnockType.KNOCK_IN, higherBarrier);
		  ResolvedFxSingleBarrierOption optionUki = ResolvedFxSingleBarrierOption.of(CALL, uki);
		  double priceUkiBlack = BLACK_PRICER.price(optionUki, RATE_PROVIDER_FLAT, VOLS_FLAT);
		  double priceUki = PRICER_70.price(optionUki, RATE_PROVIDER_FLAT, VOLS_FLAT, DATA_70_FLAT);
		  assertEqualsRelative(priceUki, priceUkiBlack, tol);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test_black_currencyExposure()
	  public virtual void test_black_currencyExposure()
	  {
		double tol = 7.0e-2; // large tol due to approximated delta
		for (int i = 0; i < 8; ++i)
		{
		  // up barrier
		  double lowerBarrier = 1.1 + 0.025 * i;
		  SimpleConstantContinuousBarrier dko = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_OUT, lowerBarrier);
		  ResolvedFxSingleBarrierOption optionDko = ResolvedFxSingleBarrierOption.of(CALL, dko, REBATE_BASE);
		  MultiCurrencyAmount ceDkoBlack = BLACK_PRICER.currencyExposure(optionDko, RATE_PROVIDER_FLAT, VOLS_FLAT);
		  MultiCurrencyAmount ceDko = PRICER_70.currencyExposure(optionDko, RATE_PROVIDER_FLAT, VOLS_FLAT, DATA_70_FLAT);
		  assertEquals(ceDko.getAmount(EUR).Amount, ceDkoBlack.getAmount(EUR).Amount, NOTIONAL * tol);
		  assertEquals(ceDko.getAmount(USD).Amount, ceDkoBlack.getAmount(USD).Amount, NOTIONAL * tol);
		  SimpleConstantContinuousBarrier dki = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_IN, lowerBarrier);
		  ResolvedFxSingleBarrierOption optionDki = ResolvedFxSingleBarrierOption.of(CALL, dki, REBATE);
		  MultiCurrencyAmount ceDkiBlack = BLACK_PRICER.currencyExposure(optionDki, RATE_PROVIDER_FLAT, VOLS_FLAT);
		  MultiCurrencyAmount ceDki = PRICER_70.currencyExposure(optionDki, RATE_PROVIDER_FLAT, VOLS_FLAT, DATA_70_FLAT);
		  assertEquals(ceDki.getAmount(EUR).Amount, ceDkiBlack.getAmount(EUR).Amount, NOTIONAL * tol);
		  assertEquals(ceDki.getAmount(USD).Amount, ceDkiBlack.getAmount(USD).Amount, NOTIONAL * tol);
		  // down barrier
		  double higherBarrier = 1.45 + 0.025 * i;
		  SimpleConstantContinuousBarrier uko = SimpleConstantContinuousBarrier.of(BarrierType.UP, KnockType.KNOCK_OUT, higherBarrier);
		  ResolvedFxSingleBarrierOption optionUko = ResolvedFxSingleBarrierOption.of(CALL, uko, REBATE);
		  MultiCurrencyAmount ceUkoBlack = BLACK_PRICER.currencyExposure(optionUko, RATE_PROVIDER_FLAT, VOLS_FLAT);
		  MultiCurrencyAmount ceUko = PRICER_70.currencyExposure(optionUko, RATE_PROVIDER_FLAT, VOLS_FLAT, DATA_70_FLAT);
		  assertEquals(ceUko.getAmount(EUR).Amount, ceUkoBlack.getAmount(EUR).Amount, NOTIONAL * tol);
		  assertEquals(ceUko.getAmount(USD).Amount, ceUkoBlack.getAmount(USD).Amount, NOTIONAL * tol);
		  SimpleConstantContinuousBarrier uki = SimpleConstantContinuousBarrier.of(BarrierType.UP, KnockType.KNOCK_IN, higherBarrier);
		  ResolvedFxSingleBarrierOption optionUki = ResolvedFxSingleBarrierOption.of(CALL, uki, REBATE_BASE);
		  MultiCurrencyAmount ceUkiBlack = BLACK_PRICER.currencyExposure(optionUki, RATE_PROVIDER_FLAT, VOLS_FLAT);
		  MultiCurrencyAmount ceUki = PRICER_70.currencyExposure(optionUki, RATE_PROVIDER_FLAT, VOLS_FLAT, DATA_70_FLAT);
		  assertEquals(ceUki.getAmount(EUR).Amount, ceUkiBlack.getAmount(EUR).Amount, NOTIONAL * tol);
		  assertEquals(ceUki.getAmount(USD).Amount, ceUkiBlack.getAmount(USD).Amount, NOTIONAL * tol);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test_black_rebate()
	  public virtual void test_black_rebate()
	  {
		double tol = 1.5e-2;
		for (int i = 0; i < 11; ++i)
		{
		  // up barrier
		  double lowerBarrier = 1.1 + 0.025 * i;
		  SimpleConstantContinuousBarrier dko = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_OUT, lowerBarrier);
		  ResolvedFxSingleBarrierOption optionDko = ResolvedFxSingleBarrierOption.of(PUT, dko, REBATE_BASE);
		  double priceDkoBlack = BLACK_PRICER.price(optionDko, RATE_PROVIDER_FLAT, VOLS_FLAT);
		  double priceDko = PRICER_70.price(optionDko, RATE_PROVIDER_FLAT, VOLS_FLAT, DATA_70_FLAT);
		  assertEqualsRelative(priceDko, priceDkoBlack, tol);
		  SimpleConstantContinuousBarrier dki = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_IN, lowerBarrier);
		  ResolvedFxSingleBarrierOption optionDki = ResolvedFxSingleBarrierOption.of(PUT, dki, REBATE_BASE);
		  double priceDkiBlack = BLACK_PRICER.price(optionDki, RATE_PROVIDER_FLAT, VOLS_FLAT);
		  double priceDki = PRICER_70.price(optionDki, RATE_PROVIDER_FLAT, VOLS_FLAT, DATA_70_FLAT);
		  assertEqualsRelative(priceDki, priceDkiBlack, tol);
		  // down barrier
		  double higherBarrier = 1.45 + 0.025 * i;
		  SimpleConstantContinuousBarrier uko = SimpleConstantContinuousBarrier.of(BarrierType.UP, KnockType.KNOCK_OUT, higherBarrier);
		  ResolvedFxSingleBarrierOption optionUko = ResolvedFxSingleBarrierOption.of(PUT, uko, REBATE);
		  double priceUkoBlack = BLACK_PRICER.price(optionUko, RATE_PROVIDER_FLAT, VOLS_FLAT);
		  double priceUko = PRICER_70.price(optionUko, RATE_PROVIDER_FLAT, VOLS_FLAT, DATA_70_FLAT);
		  assertEqualsRelative(priceUko, priceUkoBlack, tol);
		  SimpleConstantContinuousBarrier uki = SimpleConstantContinuousBarrier.of(BarrierType.UP, KnockType.KNOCK_IN, higherBarrier);
		  ResolvedFxSingleBarrierOption optionUki = ResolvedFxSingleBarrierOption.of(PUT, uki, REBATE);
		  double priceUkiBlack = BLACK_PRICER.price(optionUki, RATE_PROVIDER_FLAT, VOLS_FLAT);
		  double priceUki = PRICER_70.price(optionUki, RATE_PROVIDER_FLAT, VOLS_FLAT, DATA_70_FLAT);
		  assertEqualsRelative(priceUki, priceUkiBlack, tol);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test_monotonicity()
	  public virtual void test_monotonicity()
	  {
		double priceDkoPrev = 100d;
		double priceDkiPrev = 0d;
		double priceUkoPrev = 0d;
		double priceUkiPrev = 100d;
		for (int i = 0; i < 50; ++i)
		{
		  // up barrier
		  double lowerBarrier = 1.1 + 0.006 * i;
		  SimpleConstantContinuousBarrier dko = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_OUT, lowerBarrier);
		  ResolvedFxSingleBarrierOption optionDko = ResolvedFxSingleBarrierOption.of(CALL, dko);
		  double priceDko = PRICER_39.price(optionDko, RATE_PROVIDER, VOLS, DATA_39);
		  SimpleConstantContinuousBarrier dki = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_IN, lowerBarrier);
		  ResolvedFxSingleBarrierOption optionDki = ResolvedFxSingleBarrierOption.of(CALL, dki);
		  double priceDki = PRICER_39.price(optionDki, RATE_PROVIDER, VOLS, DATA_39);
		  // down barrier
		  double higherBarrier = 1.4 + 0.006 * (i + 1);
		  SimpleConstantContinuousBarrier uko = SimpleConstantContinuousBarrier.of(BarrierType.UP, KnockType.KNOCK_OUT, higherBarrier);
		  ResolvedFxSingleBarrierOption optionUko = ResolvedFxSingleBarrierOption.of(CALL, uko);
		  double priceUko = PRICER_39.price(optionUko, RATE_PROVIDER, VOLS, DATA_39);
		  SimpleConstantContinuousBarrier uki = SimpleConstantContinuousBarrier.of(BarrierType.UP, KnockType.KNOCK_IN, higherBarrier);
		  ResolvedFxSingleBarrierOption optionUki = ResolvedFxSingleBarrierOption.of(CALL, uki);
		  double priceUki = PRICER_39.price(optionUki, RATE_PROVIDER, VOLS, DATA_39);
		  assertTrue(priceDkoPrev > priceDko);
		  assertTrue(priceDkiPrev < priceDki);
		  assertTrue(priceUkoPrev < priceUko);
		  assertTrue(priceUkiPrev > priceUki);
		  priceDkoPrev = priceDko;
		  priceDkiPrev = priceDki;
		  priceUkoPrev = priceUko;
		  priceUkiPrev = priceUki;
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test_inOutParity()
	  public virtual void test_inOutParity()
	  {
		double tol = 1.0e-2;
		double callPrice = VANILLA_PRICER.price(CALL, RATE_PROVIDER, VOLS);
		double putPrice = VANILLA_PRICER.price(PUT, RATE_PROVIDER, VOLS);
		for (int i = 0; i < 11; ++i)
		{
		  // up barrier
		  double lowerBarrier = 1.1 + 0.025 * i;
		  SimpleConstantContinuousBarrier dko = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_OUT, lowerBarrier);
		  ResolvedFxSingleBarrierOption callDko = ResolvedFxSingleBarrierOption.of(CALL, dko);
		  double priceCallDko = PRICER_39.price(callDko, RATE_PROVIDER, VOLS, DATA_39);
		  ResolvedFxSingleBarrierOption putDko = ResolvedFxSingleBarrierOption.of(PUT, dko);
		  double pricePutDko = PRICER_39.price(putDko, RATE_PROVIDER, VOLS, DATA_39);
		  SimpleConstantContinuousBarrier dki = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_IN, lowerBarrier);
		  ResolvedFxSingleBarrierOption callDki = ResolvedFxSingleBarrierOption.of(CALL, dki);
		  double priceCallDki = PRICER_39.price(callDki, RATE_PROVIDER, VOLS, DATA_39);
		  ResolvedFxSingleBarrierOption putDki = ResolvedFxSingleBarrierOption.of(PUT, dki);
		  double pricePutDki = PRICER_39.price(putDki, RATE_PROVIDER, VOLS, DATA_39);
		  assertEqualsRelative(priceCallDko + priceCallDki, callPrice, tol);
		  assertEqualsRelative(pricePutDko + pricePutDki, putPrice, tol);
		  // down barrier
		  double higherBarrier = 1.45 + 0.025 * i;
		  SimpleConstantContinuousBarrier uko = SimpleConstantContinuousBarrier.of(BarrierType.UP, KnockType.KNOCK_OUT, higherBarrier);
		  ResolvedFxSingleBarrierOption callUko = ResolvedFxSingleBarrierOption.of(CALL, uko);
		  double priceCallUko = PRICER_39.price(callUko, RATE_PROVIDER, VOLS, DATA_39);
		  ResolvedFxSingleBarrierOption putUko = ResolvedFxSingleBarrierOption.of(PUT, uko);
		  double pricePutUko = PRICER_39.price(putUko, RATE_PROVIDER, VOLS, DATA_39);
		  SimpleConstantContinuousBarrier uki = SimpleConstantContinuousBarrier.of(BarrierType.UP, KnockType.KNOCK_IN, higherBarrier);
		  ResolvedFxSingleBarrierOption callUki = ResolvedFxSingleBarrierOption.of(CALL, uki);
		  double priceCallUki = PRICER_39.price(callUki, RATE_PROVIDER, VOLS, DATA_39);
		  ResolvedFxSingleBarrierOption putUki = ResolvedFxSingleBarrierOption.of(PUT, uki);
		  double pricePutUki = PRICER_39.price(putUki, RATE_PROVIDER, VOLS, DATA_39);
		  assertEqualsRelative(priceCallUko + priceCallUki, callPrice, tol);
		  assertEqualsRelative(pricePutUko + pricePutUki, putPrice, tol);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivityRates()
	  {
		ImpliedTrinomialTreeFxSingleBarrierOptionProductPricer pricer = new ImpliedTrinomialTreeFxSingleBarrierOptionProductPricer(21);
		CurrencyParameterSensitivities computed = pricer.presentValueSensitivityRates(CALL_UKI_C, RATE_PROVIDER, VOLS);
		RatesFiniteDifferenceSensitivityCalculator calc = new RatesFiniteDifferenceSensitivityCalculator(1.0e-5);
		CurrencyParameterSensitivities expected = calc.sensitivity(RATE_PROVIDER, p => pricer.presentValue(CALL_UKI_C, p, VOLS));
		assertTrue(computed.equalWithTolerance(expected, 1.0e-13));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withData()
	  {
		ImpliedTrinomialTreeFxSingleBarrierOptionProductPricer pricer = new ImpliedTrinomialTreeFxSingleBarrierOptionProductPricer(5);
		RecombiningTrinomialTreeData data = pricer.Calibrator.calibrateTrinomialTree(CALL_DKO.UnderlyingOption, RATE_PROVIDER, VOLS);
		double price = pricer.price(CALL_UKI_C, RATE_PROVIDER, VOLS);
		double priceWithData = pricer.price(CALL_UKI_C, RATE_PROVIDER, VOLS, data);
		assertEquals(price, priceWithData);
		CurrencyAmount pv = pricer.presentValue(CALL_DKO, RATE_PROVIDER, VOLS);
		CurrencyAmount pvWithData = pricer.presentValue(CALL_DKO, RATE_PROVIDER, VOLS, data);
		assertEquals(pv, pvWithData);
		MultiCurrencyAmount ce = pricer.currencyExposure(CALL_UKI_C, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount ceWithData = pricer.currencyExposure(CALL_UKI_C, RATE_PROVIDER, VOLS, data);
		assertEquals(ce, ceWithData);
	  }

	  public virtual void test_expired_calibration()
	  {
		assertThrowsIllegalArg(() => PRICER_39.Calibrator.calibrateTrinomialTree(CALL_DKO.UnderlyingOption, RATE_PROVIDER_AFTER, VOLS_AFTER));
		// pricing also fails because trinomial data can not be obtained
		assertThrowsIllegalArg(() => PRICER_39.price(CALL_DKO, RATE_PROVIDER_AFTER, VOLS_AFTER));
		assertThrowsIllegalArg(() => PRICER_39.presentValue(CALL_DKO, RATE_PROVIDER_AFTER, VOLS_AFTER));
		assertThrowsIllegalArg(() => PRICER_39.currencyExposure(CALL_DKO, RATE_PROVIDER_AFTER, VOLS_AFTER));
	  }

	  public virtual void test_dataMismatch()
	  {
		assertThrowsIllegalArg(() => PRICER_70.presentValueSensitivityRates(CALL_DKO, RATE_PROVIDER, VOLS, DATA_39));
	  }

	  public virtual void test_tradePricer()
	  {
		for (int i = 0; i < 11; ++i)
		{
		  // up barrier
		  double lowerBarrier = 1.1 + 0.025 * i;
		  SimpleConstantContinuousBarrier dko = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_OUT, lowerBarrier);
		  ResolvedFxSingleBarrierOption callDko = ResolvedFxSingleBarrierOption.of(CALL, dko);
		  ResolvedFxSingleBarrierOptionTrade callTrade = ResolvedFxSingleBarrierOptionTrade.builder().product(callDko).premium(Payment.of(EUR, 0, VAL_DATE)).build();

		  CurrencyAmount pvProduct = PRICER_39.presentValue(callDko, RATE_PROVIDER, VOLS);
		  MultiCurrencyAmount pvTrade = TRADE_PRICER_39.presentValue(callTrade, RATE_PROVIDER, VOLS);
		  assertEquals(pvTrade.getAmount(USD), pvProduct);
		}
	  }

	  //-------------------------------------------------------------------------
	  private void assertEqualsRelative(double computed, double expected, double relTol)
	  {
		assertEquals(computed, expected, Math.Max(1d, Math.Abs(expected)) * relTol);
	  }

	}

}