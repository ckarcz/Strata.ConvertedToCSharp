/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
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
//	import static com.opengamma.strata.product.common.LongShort.LONG;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.LongShort.SHORT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProviderDataSets = com.opengamma.strata.pricer.datasets.RatesProviderDataSets;
	using RatesProviderFxDataSets = com.opengamma.strata.pricer.fx.RatesProviderFxDataSets;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using ResolvedFxSingle = com.opengamma.strata.product.fx.ResolvedFxSingle;
	using ResolvedFxVanillaOption = com.opengamma.strata.product.fxopt.ResolvedFxVanillaOption;

	/// <summary>
	/// Test <seealso cref="BlackFxVanillaOptionProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BlackFxVanillaOptionProductPricerTest
	public class BlackFxVanillaOptionProductPricerTest
	{

	  private static readonly ZoneId ZONE = ZoneId.of("Z");
	  private static readonly ZonedDateTime EXPIRY = ZonedDateTime.of(2014, 5, 9, 13, 10, 0, 0, ZONE);
	  private static readonly LocalDate VAL_DATE = RatesProviderDataSets.VAL_DATE_2014_01_22;
	  private static readonly ZonedDateTime VAL_DATETIME_AFTER = EXPIRY.plusDays(1);
	  private static readonly LocalDate VAL_DATE_AFTER = VAL_DATETIME_AFTER.toLocalDate();
	  private static readonly LocalTime VAL_TIME = LocalTime.of(13, 45);
	  private static readonly ZonedDateTime VAL_DATETIME = VAL_DATE.atTime(VAL_TIME).atZone(ZONE);
	  private static readonly RatesProvider RATES_PROVIDER = RatesProviderFxDataSets.createProviderEURUSD(VAL_DATE);
	  private static readonly RatesProvider RATES_PROVIDER_EXPIRY = RatesProviderFxDataSets.createProviderEURUSD(EXPIRY.toLocalDate());
	  private static readonly RatesProvider RATES_PROVIDER_AFTER = RatesProviderFxDataSets.createProviderEURUSD(VAL_DATE_AFTER);
	  private static readonly BlackFxOptionSmileVolatilities VOLS = FxVolatilitySmileDataSet.createVolatilitySmileProvider6(VAL_DATETIME);
	  private static readonly BlackFxOptionSmileVolatilities VOLS_EXPIRY = FxVolatilitySmileDataSet.createVolatilitySmileProvider6(EXPIRY);
	  private static readonly BlackFxOptionSmileVolatilities VOLS_AFTER = FxVolatilitySmileDataSet.createVolatilitySmileProvider6(VAL_DATETIME_AFTER);
	  private static readonly InterpolatedStrikeSmileDeltaTermStructure SMILE_TERM = FxVolatilitySmileDataSet.SmileDeltaTermStructure6;

	  private static readonly CurrencyPair CURRENCY_PAIR = CurrencyPair.of(EUR, USD);
	  private const double NOTIONAL = 1.0e6;
	  private static readonly LocalDate PAYMENT_DATE = LocalDate.of(2014, 5, 13);
	  private const double STRIKE_RATE_HIGH = 1.44;
	  private const double STRIKE_RATE_LOW = 1.36;
	  private static readonly CurrencyAmount EUR_AMOUNT = CurrencyAmount.of(EUR, NOTIONAL);
	  private static readonly CurrencyAmount USD_AMOUNT_HIGH = CurrencyAmount.of(USD, -NOTIONAL * STRIKE_RATE_HIGH);
	  private static readonly CurrencyAmount USD_AMOUNT_LOW = CurrencyAmount.of(USD, -NOTIONAL * STRIKE_RATE_LOW);
	  private static readonly ResolvedFxSingle FX_PRODUCT_HIGH = ResolvedFxSingle.of(EUR_AMOUNT, USD_AMOUNT_HIGH, PAYMENT_DATE);
	  private static readonly ResolvedFxSingle FX_PRODUCT_LOW = ResolvedFxSingle.of(EUR_AMOUNT, USD_AMOUNT_LOW, PAYMENT_DATE);
	  private static readonly ResolvedFxVanillaOption CALL_OTM = ResolvedFxVanillaOption.builder().longShort(SHORT).expiry(EXPIRY).underlying(FX_PRODUCT_HIGH).build();
	  private static readonly ResolvedFxVanillaOption CALL_ITM = ResolvedFxVanillaOption.builder().longShort(LONG).expiry(EXPIRY).underlying(FX_PRODUCT_LOW).build();
	  private static readonly ResolvedFxVanillaOption PUT_OTM = ResolvedFxVanillaOption.builder().longShort(SHORT).expiry(EXPIRY).underlying(FX_PRODUCT_LOW.inverse()).build();
	  private static readonly ResolvedFxVanillaOption PUT_ITM = ResolvedFxVanillaOption.builder().longShort(LONG).expiry(EXPIRY).underlying(FX_PRODUCT_HIGH.inverse()).build();
	  private static readonly BlackFxVanillaOptionProductPricer PRICER = BlackFxVanillaOptionProductPricer.DEFAULT;
	  private const double TOL = 1.0e-13;
	  private const double FD_EPS = 1.0e-7;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FD_CAL = new RatesFiniteDifferenceSensitivityCalculator(FD_EPS);

	  //-------------------------------------------------------------------------
	  public virtual void test_price_presentValue()
	  {
		double priceCallOtm = PRICER.price(CALL_OTM, RATES_PROVIDER, VOLS);
		CurrencyAmount pvCallOtm = PRICER.presentValue(CALL_OTM, RATES_PROVIDER, VOLS);
		double pricePutOtm = PRICER.price(PUT_OTM, RATES_PROVIDER, VOLS);
		CurrencyAmount pvPutOtm = PRICER.presentValue(PUT_OTM, RATES_PROVIDER, VOLS);
		double timeToExpiry = VOLS.relativeTime(EXPIRY);
		double df = RATES_PROVIDER.discountFactor(USD, PAYMENT_DATE);
		double forward = PRICER.DiscountingFxSingleProductPricer.forwardFxRate(FX_PRODUCT_HIGH, RATES_PROVIDER).fxRate(CURRENCY_PAIR);
		double volHigh = SMILE_TERM.volatility(timeToExpiry, STRIKE_RATE_HIGH, forward);
		double volLow = SMILE_TERM.volatility(timeToExpiry, STRIKE_RATE_LOW, forward);
		double expectedPriceCallOtm = df * BlackFormulaRepository.price(forward, STRIKE_RATE_HIGH, timeToExpiry, volHigh, true);
		double expectedPricePutOtm = df * BlackFormulaRepository.price(forward, STRIKE_RATE_LOW, timeToExpiry, volLow, false);
		double expectedPvCallOtm = -NOTIONAL * df * BlackFormulaRepository.price(forward, STRIKE_RATE_HIGH, timeToExpiry, volHigh, true);
		double expectedPvPutOtm = -NOTIONAL * df * BlackFormulaRepository.price(forward, STRIKE_RATE_LOW, timeToExpiry, volLow, false);
		assertEquals(priceCallOtm, expectedPriceCallOtm, TOL);
		assertEquals(pvCallOtm.Currency, USD);
		assertEquals(pvCallOtm.Amount, expectedPvCallOtm, NOTIONAL * TOL);
		assertEquals(pricePutOtm, expectedPricePutOtm, TOL);
		assertEquals(pvPutOtm.Currency, USD);
		assertEquals(pvPutOtm.Amount, expectedPvPutOtm, NOTIONAL * TOL);
	  }

	  public virtual void test_price_presentValue_atExpiry()
	  {
		double df = RATES_PROVIDER_EXPIRY.discountFactor(USD, PAYMENT_DATE);
		double forward = PRICER.DiscountingFxSingleProductPricer.forwardFxRate(FX_PRODUCT_HIGH, RATES_PROVIDER_EXPIRY).fxRate(CURRENCY_PAIR);
		double priceCallOtm = PRICER.price(CALL_OTM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		CurrencyAmount pvCallOtm = PRICER.presentValue(CALL_OTM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		assertEquals(priceCallOtm, 0d, TOL);
		assertEquals(pvCallOtm.Amount, 0d, NOTIONAL * TOL);
		double priceCallItm = PRICER.price(CALL_ITM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		CurrencyAmount pvCallItm = PRICER.presentValue(CALL_ITM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		assertEquals(priceCallItm, df * (forward - STRIKE_RATE_LOW), TOL);
		assertEquals(pvCallItm.Amount, df * (forward - STRIKE_RATE_LOW) * NOTIONAL, NOTIONAL * TOL);
		double pricePutOtm = PRICER.price(PUT_OTM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		CurrencyAmount pvPutOtm = PRICER.presentValue(PUT_OTM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		assertEquals(pricePutOtm, 0d, TOL);
		assertEquals(pvPutOtm.Amount, 0d, NOTIONAL * TOL);
		double pricePutItm = PRICER.price(PUT_ITM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		CurrencyAmount pvPutItm = PRICER.presentValue(PUT_ITM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		assertEquals(pricePutItm, df * (STRIKE_RATE_HIGH - forward), TOL);
		assertEquals(pvPutItm.Amount, df * (STRIKE_RATE_HIGH - forward) * NOTIONAL, NOTIONAL * TOL);
	  }

	  public virtual void test_price_presentValue_afterExpiry()
	  {
		double price = PRICER.price(CALL_OTM, RATES_PROVIDER_AFTER, VOLS_AFTER);
		CurrencyAmount pv = PRICER.presentValue(CALL_OTM, RATES_PROVIDER_AFTER, VOLS_AFTER);
		assertEquals(price, 0d, NOTIONAL * TOL);
		assertEquals(pv.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_price_presentValue_parity()
	  {
		double df = RATES_PROVIDER.discountFactor(USD, PAYMENT_DATE);
		double forward = PRICER.DiscountingFxSingleProductPricer.forwardFxRate(FX_PRODUCT_HIGH, RATES_PROVIDER).fxRate(CURRENCY_PAIR);
		double priceCallOtm = PRICER.price(CALL_OTM, RATES_PROVIDER, VOLS);
		CurrencyAmount pvCallOtm = PRICER.presentValue(CALL_OTM, RATES_PROVIDER, VOLS);
		double pricePutItm = PRICER.price(PUT_ITM, RATES_PROVIDER, VOLS);
		CurrencyAmount pvPutItm = PRICER.presentValue(PUT_ITM, RATES_PROVIDER, VOLS);
		assertEquals(priceCallOtm - pricePutItm, df * (forward - STRIKE_RATE_HIGH), TOL);
		assertEquals(-pvCallOtm.Amount - pvPutItm.Amount, df * (forward - STRIKE_RATE_HIGH) * NOTIONAL, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_delta_presentValueDelta()
	  {
		double deltaCall = PRICER.delta(CALL_OTM, RATES_PROVIDER, VOLS);
		CurrencyAmount pvDeltaCall = PRICER.presentValueDelta(CALL_OTM, RATES_PROVIDER, VOLS);
		double deltaPut = PRICER.delta(PUT_ITM, RATES_PROVIDER, VOLS);
		CurrencyAmount pvDeltaPut = PRICER.presentValueDelta(PUT_ITM, RATES_PROVIDER, VOLS);
		double timeToExpiry = VOLS.relativeTime(EXPIRY);
		double dfFor = RATES_PROVIDER.discountFactor(EUR, PAYMENT_DATE);
		double forward = PRICER.DiscountingFxSingleProductPricer.forwardFxRate(FX_PRODUCT_HIGH, RATES_PROVIDER).fxRate(CURRENCY_PAIR);
		double vol = SMILE_TERM.volatility(timeToExpiry, STRIKE_RATE_HIGH, forward);
		double expectedDeltaCall = dfFor * BlackFormulaRepository.delta(forward, STRIKE_RATE_HIGH, timeToExpiry, vol, true);
		double expectedDeltaPut = dfFor * BlackFormulaRepository.delta(forward, STRIKE_RATE_HIGH, timeToExpiry, vol, false);
		double expectedPvDeltaCall = -NOTIONAL * dfFor * BlackFormulaRepository.delta(forward, STRIKE_RATE_HIGH, timeToExpiry, vol, true);
		double expectedPvDeltaPut = NOTIONAL * dfFor * BlackFormulaRepository.delta(forward, STRIKE_RATE_HIGH, timeToExpiry, vol, false);
		assertEquals(deltaCall, expectedDeltaCall, TOL);
		assertEquals(pvDeltaCall.Currency, USD);
		assertEquals(pvDeltaCall.Amount, expectedPvDeltaCall, NOTIONAL * TOL);
		assertEquals(deltaPut, expectedDeltaPut, TOL);
		assertEquals(pvDeltaPut.Currency, USD);
		assertEquals(pvDeltaPut.Amount, expectedPvDeltaPut, NOTIONAL * TOL);
	  }

	  public virtual void test_delta_presentValueDelta_atExpiry()
	  {
		double dfFor = RATES_PROVIDER_EXPIRY.discountFactor(EUR, PAYMENT_DATE);
		double deltaCallOtm = PRICER.delta(CALL_OTM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		CurrencyAmount pvDeltaCallOtm = PRICER.presentValueDelta(CALL_OTM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		assertEquals(deltaCallOtm, 0d, TOL);
		assertEquals(pvDeltaCallOtm.Amount, 0d, NOTIONAL * TOL);
		double deltaCallItm = PRICER.delta(CALL_ITM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		CurrencyAmount pvDeltaCallItm = PRICER.presentValueDelta(CALL_ITM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		assertEquals(deltaCallItm, dfFor, TOL);
		assertEquals(pvDeltaCallItm.Amount, NOTIONAL * dfFor, NOTIONAL * TOL);
		double deltaPutItm = PRICER.delta(PUT_ITM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		CurrencyAmount pvDeltaPutItm = PRICER.presentValueDelta(PUT_ITM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		assertEquals(deltaPutItm, -dfFor, TOL);
		assertEquals(pvDeltaPutItm.Amount, -NOTIONAL * dfFor, NOTIONAL * TOL);
		double deltaPutOtm = PRICER.delta(PUT_OTM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		CurrencyAmount pvDeltaPutOtm = PRICER.presentValueDelta(PUT_OTM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		assertEquals(deltaPutOtm, 0d, TOL);
		assertEquals(pvDeltaPutOtm.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_delta_presentValueDelta_afterExpiry()
	  {
		double delta = PRICER.delta(CALL_OTM, RATES_PROVIDER_AFTER, VOLS_AFTER);
		CurrencyAmount pvDelta = PRICER.presentValueDelta(CALL_OTM, RATES_PROVIDER_AFTER, VOLS_AFTER);
		assertEquals(delta, 0d, TOL);
		assertEquals(pvDelta.Amount, 0d, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity()
	  {
		// call
		PointSensitivities pointCall = PRICER.presentValueSensitivityRatesStickyStrike(CALL_OTM, RATES_PROVIDER, VOLS);
		CurrencyParameterSensitivities computedCall = RATES_PROVIDER.parameterSensitivity(pointCall);
		CurrencyParameterSensitivities expectedCall = FD_CAL.sensitivity(RATES_PROVIDER, (p) => PRICER.presentValue(CALL_OTM, (p), VOLS));
		// contribution via implied volatility, to be subtracted.
		CurrencyAmount pvVegaCall = PRICER.presentValueVega(CALL_OTM, RATES_PROVIDER, VOLS);
		CurrencyParameterSensitivities impliedVolSenseCall = FD_CAL.sensitivity(RATES_PROVIDER, (p) => CurrencyAmount.of(USD, PRICER.impliedVolatility(CALL_OTM, (p), VOLS))).multipliedBy(-pvVegaCall.Amount);
		assertTrue(computedCall.equalWithTolerance(expectedCall.combinedWith(impliedVolSenseCall), NOTIONAL * FD_EPS));
		// put
		PointSensitivities pointPut = PRICER.presentValueSensitivityRatesStickyStrike(PUT_OTM, RATES_PROVIDER, VOLS);
		CurrencyParameterSensitivities computedPut = RATES_PROVIDER.parameterSensitivity(pointPut);
		CurrencyParameterSensitivities expectedPut = FD_CAL.sensitivity(RATES_PROVIDER, (p) => PRICER.presentValue(PUT_OTM, (p), VOLS));
		// contribution via implied volatility, to be subtracted.
		CurrencyAmount pvVegaPut = PRICER.presentValueVega(PUT_OTM, RATES_PROVIDER, VOLS);
		CurrencyParameterSensitivities impliedVolSensePut = FD_CAL.sensitivity(RATES_PROVIDER, (p) => CurrencyAmount.of(USD, PRICER.impliedVolatility(PUT_OTM, (p), VOLS))).multipliedBy(-pvVegaPut.Amount);
		assertTrue(computedPut.equalWithTolerance(expectedPut.combinedWith(impliedVolSensePut), NOTIONAL * FD_EPS));
	  }

	  public virtual void test_presentValueSensitivity_atExpiry()
	  {
		// call
		PointSensitivities pointCall = PRICER.presentValueSensitivityRatesStickyStrike(CALL_OTM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		CurrencyParameterSensitivities computedCall = RATES_PROVIDER_EXPIRY.parameterSensitivity(pointCall);
		CurrencyParameterSensitivities expectedCall = FD_CAL.sensitivity(RATES_PROVIDER_EXPIRY, (p) => PRICER.presentValue(CALL_OTM, (p), VOLS_EXPIRY));
		assertTrue(computedCall.equalWithTolerance(expectedCall, NOTIONAL * FD_EPS));
		// put
		PointSensitivities pointPut = PRICER.presentValueSensitivityRatesStickyStrike(PUT_OTM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		CurrencyParameterSensitivities computedPut = RATES_PROVIDER_EXPIRY.parameterSensitivity(pointPut);
		CurrencyParameterSensitivities expectedPut = FD_CAL.sensitivity(RATES_PROVIDER_EXPIRY, (p) => PRICER.presentValue(PUT_OTM, (p), VOLS_EXPIRY));
		assertTrue(computedPut.equalWithTolerance(expectedPut, NOTIONAL * FD_EPS));
	  }

	  public virtual void test_presentValueSensitivity_afterExpiry()
	  {
		PointSensitivities point = PRICER.presentValueSensitivityRatesStickyStrike(CALL_ITM, RATES_PROVIDER_AFTER, VOLS_AFTER);
		assertEquals(point, PointSensitivities.empty());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_gamma_presentValueGamma()
	  {
		double gammaCall = PRICER.gamma(CALL_OTM, RATES_PROVIDER, VOLS);
		CurrencyAmount pvGammaCall = PRICER.presentValueGamma(CALL_OTM, RATES_PROVIDER, VOLS);
		double gammaPut = PRICER.gamma(PUT_ITM, RATES_PROVIDER, VOLS);
		CurrencyAmount pvGammaPut = PRICER.presentValueGamma(PUT_ITM, RATES_PROVIDER, VOLS);
		double timeToExpiry = VOLS.relativeTime(EXPIRY);
		double dfDom = RATES_PROVIDER.discountFactor(USD, PAYMENT_DATE);
		double dfFor = RATES_PROVIDER.discountFactor(EUR, PAYMENT_DATE);
		double forward = PRICER.DiscountingFxSingleProductPricer.forwardFxRate(FX_PRODUCT_HIGH, RATES_PROVIDER).fxRate(CURRENCY_PAIR);
		double vol = SMILE_TERM.volatility(timeToExpiry, STRIKE_RATE_HIGH, forward);
		double expectedGamma = dfFor * dfFor / dfDom * BlackFormulaRepository.gamma(forward, STRIKE_RATE_HIGH, timeToExpiry, vol);
		double expectedPvGamma = -NOTIONAL * dfFor * dfFor / dfDom * BlackFormulaRepository.gamma(forward, STRIKE_RATE_HIGH, timeToExpiry, vol);
		assertEquals(gammaCall, expectedGamma, TOL);
		assertEquals(pvGammaCall.Currency, USD);
		assertEquals(pvGammaCall.Amount, expectedPvGamma, NOTIONAL * TOL);
		assertEquals(gammaPut, expectedGamma, TOL);
		assertEquals(pvGammaPut.Currency, USD);
		assertEquals(pvGammaPut.Amount, -expectedPvGamma, NOTIONAL * TOL);
	  }

	  public virtual void test_gamma_presentValueGamma_atExpiry()
	  {
		double gamma = PRICER.gamma(PUT_ITM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		CurrencyAmount pvGamma = PRICER.presentValueGamma(PUT_ITM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		assertEquals(gamma, 0d, TOL);
		assertEquals(pvGamma.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_gamma_presentValueGamma_afterExpiry()
	  {
		double gamma = PRICER.gamma(CALL_ITM, RATES_PROVIDER_AFTER, VOLS_AFTER);
		CurrencyAmount pvGamma = PRICER.presentValueGamma(CALL_ITM, RATES_PROVIDER_AFTER, VOLS_AFTER);
		assertEquals(gamma, 0d, TOL);
		assertEquals(pvGamma.Amount, 0d, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_vega_presentValueVega()
	  {
		double vegaCall = PRICER.vega(CALL_OTM, RATES_PROVIDER, VOLS);
		CurrencyAmount pvVegaCall = PRICER.presentValueVega(CALL_OTM, RATES_PROVIDER, VOLS);
		double vegaPut = PRICER.vega(PUT_ITM, RATES_PROVIDER, VOLS);
		CurrencyAmount pvVegaPut = PRICER.presentValueVega(PUT_ITM, RATES_PROVIDER, VOLS);
		double timeToExpiry = VOLS.relativeTime(EXPIRY);
		double dfDom = RATES_PROVIDER.discountFactor(USD, PAYMENT_DATE);
		double forward = PRICER.DiscountingFxSingleProductPricer.forwardFxRate(FX_PRODUCT_HIGH, RATES_PROVIDER).fxRate(CURRENCY_PAIR);
		double vol = SMILE_TERM.volatility(timeToExpiry, STRIKE_RATE_HIGH, forward);
		double expectedVega = dfDom * BlackFormulaRepository.vega(forward, STRIKE_RATE_HIGH, timeToExpiry, vol);
		double expectedPvVega = -NOTIONAL * dfDom * BlackFormulaRepository.vega(forward, STRIKE_RATE_HIGH, timeToExpiry, vol);
		assertEquals(vegaCall, expectedVega, TOL);
		assertEquals(pvVegaCall.Currency, USD);
		assertEquals(pvVegaCall.Amount, expectedPvVega, NOTIONAL * TOL);
		assertEquals(vegaPut, expectedVega, TOL);
		assertEquals(pvVegaPut.Currency, USD);
		assertEquals(pvVegaPut.Amount, -expectedPvVega, NOTIONAL * TOL);
	  }

	  public virtual void test_vega_presentValueVega_atExpiry()
	  {
		double vega = PRICER.vega(PUT_ITM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		CurrencyAmount pvVega = PRICER.presentValueVega(PUT_ITM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		assertEquals(vega, 0d, TOL);
		assertEquals(pvVega.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_vega_presentValueVega_afterExpiry()
	  {
		double vega = PRICER.vega(CALL_ITM, RATES_PROVIDER_AFTER, VOLS_AFTER);
		CurrencyAmount pvVega = PRICER.presentValueVega(CALL_ITM, RATES_PROVIDER_AFTER, VOLS_AFTER);
		assertEquals(vega, 0d, TOL);
		assertEquals(pvVega.Amount, 0d, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivityBlackVolatility()
	  {
		FxOptionSensitivity computedCall = (FxOptionSensitivity) PRICER.presentValueSensitivityModelParamsVolatility(CALL_OTM, RATES_PROVIDER, VOLS);
		FxOptionSensitivity computedPut = (FxOptionSensitivity) PRICER.presentValueSensitivityModelParamsVolatility(PUT_ITM, RATES_PROVIDER, VOLS);
		double timeToExpiry = VOLS.relativeTime(EXPIRY);
		double df = RATES_PROVIDER.discountFactor(USD, PAYMENT_DATE);
		double forward = PRICER.DiscountingFxSingleProductPricer.forwardFxRate(FX_PRODUCT_HIGH, RATES_PROVIDER).fxRate(CURRENCY_PAIR);
		double vol = SMILE_TERM.volatility(timeToExpiry, STRIKE_RATE_HIGH, forward);
		FxOptionSensitivity expected = FxOptionSensitivity.of(VOLS.Name, CURRENCY_PAIR, timeToExpiry, STRIKE_RATE_HIGH, forward, USD, -NOTIONAL * df * BlackFormulaRepository.vega(forward, STRIKE_RATE_HIGH, timeToExpiry, vol));
		assertTrue(computedCall.build().equalWithTolerance(expected.build(), NOTIONAL * TOL));
		assertTrue(computedPut.build().equalWithTolerance(expected.build().multipliedBy(-1d), NOTIONAL * TOL));
	  }

	  public virtual void test_presentValueSensitivityBlackVolatility_atExpiry()
	  {
		PointSensitivityBuilder point = PRICER.presentValueSensitivityModelParamsVolatility(PUT_ITM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		assertEquals(point, PointSensitivityBuilder.none());
	  }

	  public virtual void test_presentValueSensitivityBlackVolatility_afterExpiry()
	  {
		PointSensitivityBuilder point = PRICER.presentValueSensitivityModelParamsVolatility(CALL_ITM, RATES_PROVIDER_AFTER, VOLS_AFTER);
		assertEquals(point, PointSensitivityBuilder.none());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_theta_presentValueTheta()
	  {
		double theta = PRICER.theta(CALL_OTM, RATES_PROVIDER, VOLS);
		CurrencyAmount pvTheta = PRICER.presentValueTheta(CALL_OTM, RATES_PROVIDER, VOLS);
		double timeToExpiry = VOLS.relativeTime(EXPIRY);
		double dfDom = RATES_PROVIDER.discountFactor(USD, PAYMENT_DATE);
		double forward = PRICER.DiscountingFxSingleProductPricer.forwardFxRate(FX_PRODUCT_HIGH, RATES_PROVIDER).fxRate(CURRENCY_PAIR);
		double vol = SMILE_TERM.volatility(timeToExpiry, STRIKE_RATE_HIGH, forward);
		double expectedTheta = dfDom * BlackFormulaRepository.driftlessTheta(forward, STRIKE_RATE_HIGH, timeToExpiry, vol);
		assertEquals(theta, expectedTheta, TOL);
		double expectedPvTheta = -NOTIONAL * dfDom * BlackFormulaRepository.driftlessTheta(forward, STRIKE_RATE_HIGH, timeToExpiry, vol);
		assertEquals(pvTheta.Currency, USD);
		assertEquals(pvTheta.Amount, expectedPvTheta, NOTIONAL * TOL);
	  }

	  public virtual void test_theta_presentValueTheta_atExpiry()
	  {
		double theta = PRICER.theta(PUT_ITM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		CurrencyAmount pvTheta = PRICER.presentValueTheta(PUT_ITM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY);
		assertEquals(theta, 0d, TOL);
		assertEquals(pvTheta.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_theta_presentValueTheta_afterExpiry()
	  {
		double theta = PRICER.theta(CALL_ITM, RATES_PROVIDER_AFTER, VOLS_AFTER);
		CurrencyAmount pvTheta = PRICER.presentValueTheta(CALL_ITM, RATES_PROVIDER_AFTER, VOLS_AFTER);
		assertEquals(theta, 0d, TOL);
		assertEquals(pvTheta.Amount, 0d, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_impliedVolatility()
	  {
		double computedCall = PRICER.impliedVolatility(CALL_OTM, RATES_PROVIDER, VOLS);
		double computedPut = PRICER.impliedVolatility(PUT_ITM, RATES_PROVIDER, VOLS);
		double timeToExpiry = VOLS.relativeTime(EXPIRY);
		double forward = PRICER.DiscountingFxSingleProductPricer.forwardFxRate(FX_PRODUCT_HIGH, RATES_PROVIDER).fxRate(CURRENCY_PAIR);
		double expected = SMILE_TERM.volatility(timeToExpiry, STRIKE_RATE_HIGH, forward);
		assertEquals(computedCall, expected);
		assertEquals(computedPut, expected);
	  }

	  public virtual void test_impliedVolatility_atExpiry()
	  {
		assertThrowsIllegalArg(() => PRICER.impliedVolatility(CALL_ITM, RATES_PROVIDER_EXPIRY, VOLS_EXPIRY));
	  }

	  public virtual void test_impliedVolatility_afterExpiry()
	  {
		assertThrowsIllegalArg(() => PRICER.impliedVolatility(CALL_ITM, RATES_PROVIDER_AFTER, VOLS_AFTER));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currencyExposure()
	  {
		MultiCurrencyAmount computedPricer = PRICER.currencyExposure(CALL_OTM, RATES_PROVIDER, VOLS);
		CurrencyAmount pv = PRICER.presentValue(CALL_OTM, RATES_PROVIDER, VOLS);
		PointSensitivities point = PRICER.presentValueSensitivityRatesStickyStrike(CALL_OTM, RATES_PROVIDER, VOLS);
		MultiCurrencyAmount computedPoint = RATES_PROVIDER.currencyExposure(point).plus(pv);
		assertEquals(computedPricer.getAmount(EUR).Amount, computedPoint.getAmount(EUR).Amount, NOTIONAL * TOL);
		assertEquals(computedPricer.getAmount(USD).Amount, computedPoint.getAmount(USD).Amount, NOTIONAL * TOL);
	  }

	}

}