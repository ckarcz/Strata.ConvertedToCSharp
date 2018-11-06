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
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesProviderFxDataSets = com.opengamma.strata.pricer.fx.RatesProviderFxDataSets;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using ResolvedFxSingle = com.opengamma.strata.product.fx.ResolvedFxSingle;
	using ResolvedFxSingleBarrierOption = com.opengamma.strata.product.fxopt.ResolvedFxSingleBarrierOption;
	using ResolvedFxSingleBarrierOptionTrade = com.opengamma.strata.product.fxopt.ResolvedFxSingleBarrierOptionTrade;
	using ResolvedFxVanillaOption = com.opengamma.strata.product.fxopt.ResolvedFxVanillaOption;
	using BarrierType = com.opengamma.strata.product.option.BarrierType;
	using KnockType = com.opengamma.strata.product.option.KnockType;
	using SimpleConstantContinuousBarrier = com.opengamma.strata.product.option.SimpleConstantContinuousBarrier;

	/// <summary>
	/// Test <seealso cref="BlackFxSingleBarrierOptionTradePricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BlackFxSingleBarrierOptionTradePricerTest
	public class BlackFxSingleBarrierOptionTradePricerTest
	{

	  private static readonly ZoneId ZONE = ZoneId.of("Z");
	  private static readonly LocalDate VAL_DATE = LocalDate.of(2011, 6, 13);
	  private static readonly ZonedDateTime VAL_DATETIME = VAL_DATE.atStartOfDay(ZONE);
	  private static readonly LocalDate PAY_DATE = LocalDate.of(2014, 9, 15);
	  private static readonly LocalDate EXPIRY_DATE = LocalDate.of(2014, 9, 15);
	  private static readonly ZonedDateTime EXPIRY_DATETIME = EXPIRY_DATE.atStartOfDay(ZONE);
	  private static readonly BlackFxOptionSmileVolatilities VOLS = FxVolatilitySmileDataSet.createVolatilitySmileProvider5(VAL_DATETIME);
	  private static readonly ImmutableRatesProvider RATES_PROVIDER = RatesProviderFxDataSets.createProviderEurUsdActActIsda(VAL_DATE);

	  private const double NOTIONAL = 100_000_000d;
	  private const double LEVEL_LOW = 1.35;
	  private static readonly SimpleConstantContinuousBarrier BARRIER_DKI = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_IN, LEVEL_LOW);
	  private const double REBATE_AMOUNT = 50_000d;
	  private static readonly CurrencyAmount REBATE = CurrencyAmount.of(USD, REBATE_AMOUNT);
	  private const double STRIKE_RATE = 1.45;
	  private static readonly CurrencyAmount EUR_AMOUNT_REC = CurrencyAmount.of(EUR, NOTIONAL);
	  private static readonly CurrencyAmount USD_AMOUNT_PAY = CurrencyAmount.of(USD, -NOTIONAL * STRIKE_RATE);
	  private static readonly ResolvedFxSingle FX_PRODUCT = ResolvedFxSingle.of(EUR_AMOUNT_REC, USD_AMOUNT_PAY, PAY_DATE);
	  private static readonly ResolvedFxVanillaOption CALL = ResolvedFxVanillaOption.builder().longShort(LONG).expiry(EXPIRY_DATETIME).underlying(FX_PRODUCT).build();
	  private static readonly ResolvedFxSingleBarrierOption OPTION_PRODUCT = ResolvedFxSingleBarrierOption.of(CALL, BARRIER_DKI, REBATE);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.builder().tradeDate(VAL_DATE).build();
	  private static readonly LocalDate CASH_SETTLE_DATE = LocalDate.of(2011, 6, 16);
	  private static readonly Payment PREMIUM = Payment.of(EUR, NOTIONAL * 0.027, CASH_SETTLE_DATE);
	  private static readonly ResolvedFxSingleBarrierOptionTrade OPTION_TRADE = ResolvedFxSingleBarrierOptionTrade.builder().premium(PREMIUM).product(OPTION_PRODUCT).info(TRADE_INFO).build();

	  private static readonly BlackFxSingleBarrierOptionProductPricer PRICER_PRODUCT = BlackFxSingleBarrierOptionProductPricer.DEFAULT;
	  private static readonly BlackFxSingleBarrierOptionTradePricer PRICER_TRADE = BlackFxSingleBarrierOptionTradePricer.DEFAULT;
	  private static readonly DiscountingPaymentPricer PRICER_PAYMENT = DiscountingPaymentPricer.DEFAULT;
	  private const double TOL = 1.0e-13;

	  public virtual void test_presentValue()
	  {
		MultiCurrencyAmount pvSensiTrade = PRICER_TRADE.presentValue(OPTION_TRADE, RATES_PROVIDER, VOLS);
		CurrencyAmount pvSensiProduct = PRICER_PRODUCT.presentValue(OPTION_PRODUCT, RATES_PROVIDER, VOLS);
		CurrencyAmount pvSensiPremium = PRICER_PAYMENT.presentValue(PREMIUM, RATES_PROVIDER);
		assertEquals(pvSensiTrade, MultiCurrencyAmount.of(pvSensiProduct, pvSensiPremium));
	  }

	  public virtual void test_presentValueSensitivity()
	  {
		PointSensitivities pvSensiTrade = PRICER_TRADE.presentValueSensitivityRatesStickyStrike(OPTION_TRADE, RATES_PROVIDER, VOLS);
		PointSensitivities pvSensiProduct = PRICER_PRODUCT.presentValueSensitivityRatesStickyStrike(OPTION_PRODUCT, RATES_PROVIDER, VOLS).build();
		PointSensitivities pvSensiPremium = PRICER_PAYMENT.presentValueSensitivity(PREMIUM, RATES_PROVIDER).build();
		assertEquals(pvSensiTrade, pvSensiProduct.combinedWith(pvSensiPremium));
	  }

	  public virtual void test_presentValueSensitivityBlackVolatility()
	  {
		PointSensitivities pvSensiTrade = PRICER_TRADE.presentValueSensitivityModelParamsVolatility(OPTION_TRADE, RATES_PROVIDER, VOLS);
		PointSensitivities pvSensiProduct = PRICER_PRODUCT.presentValueSensitivityModelParamsVolatility(OPTION_PRODUCT, RATES_PROVIDER, VOLS).build();
		assertEquals(pvSensiTrade, pvSensiProduct);
	  }

	  public virtual void test_currencyExposure()
	  {
		MultiCurrencyAmount ceComputed = PRICER_TRADE.currencyExposure(OPTION_TRADE, RATES_PROVIDER, VOLS);
		MultiCurrencyAmount ceExpected = PRICER_PRODUCT.currencyExposure(OPTION_PRODUCT, RATES_PROVIDER, VOLS).plus(PRICER_PAYMENT.presentValue(PREMIUM, RATES_PROVIDER));
		assertEquals(ceComputed.size(), 2);
		assertEquals(ceComputed.getAmount(EUR).Amount, ceExpected.getAmount(EUR).Amount, TOL * NOTIONAL);
		assertEquals(ceComputed.getAmount(USD).Amount, ceExpected.getAmount(USD).Amount, TOL * NOTIONAL);
	  }

	  public virtual void test_currentCash_zero()
	  {
		assertEquals(PRICER_TRADE.currentCash(OPTION_TRADE, VAL_DATE), CurrencyAmount.zero(PREMIUM.Currency));
	  }

	  public virtual void test_currentCash_onSettle()
	  {
		assertEquals(PRICER_TRADE.currentCash(OPTION_TRADE, CASH_SETTLE_DATE), PREMIUM.Value);
	  }

	}

}