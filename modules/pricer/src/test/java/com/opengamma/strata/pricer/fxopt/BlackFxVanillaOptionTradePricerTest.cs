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
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.LongShort.SHORT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxMatrix = com.opengamma.strata.basics.currency.FxMatrix;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesProviderDataSets = com.opengamma.strata.pricer.datasets.RatesProviderDataSets;
	using RatesProviderFxDataSets = com.opengamma.strata.pricer.fx.RatesProviderFxDataSets;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using ResolvedFxSingle = com.opengamma.strata.product.fx.ResolvedFxSingle;
	using ResolvedFxVanillaOption = com.opengamma.strata.product.fxopt.ResolvedFxVanillaOption;
	using ResolvedFxVanillaOptionTrade = com.opengamma.strata.product.fxopt.ResolvedFxVanillaOptionTrade;

	/// <summary>
	/// Test <seealso cref="BlackFxVanillaOptionTradePricer"/>. 
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BlackFxVanillaOptionTradePricerTest
	public class BlackFxVanillaOptionTradePricerTest
	{

	  private static readonly LocalDate VAL_DATE = RatesProviderDataSets.VAL_DATE_2014_01_22;
	  private static readonly LocalTime VAL_TIME = LocalTime.of(13, 45);
	  private static readonly ZoneId ZONE = ZoneId.of("Z");
	  private static readonly ZonedDateTime VAL_DATE_TIME = VAL_DATE.atTime(VAL_TIME).atZone(ZONE);
	  private static readonly ZonedDateTime EXPIRY = ZonedDateTime.of(2014, 5, 9, 13, 10, 0, 0, ZONE);

	  private static readonly FxMatrix FX_MATRIX = RatesProviderFxDataSets.fxMatrix();
	  private static readonly RatesProvider RATES_PROVIDER = RatesProviderFxDataSets.createProviderEURUSD(VAL_DATE);

	  private static readonly DoubleArray TIME_TO_EXPIRY = DoubleArray.of(0.01, 0.252, 0.501, 1.0, 2.0, 5.0);
	  private static readonly DoubleArray ATM = DoubleArray.of(0.175, 0.185, 0.18, 0.17, 0.16, 0.16);
	  private static readonly DoubleArray DELTA = DoubleArray.of(0.10, 0.25);
	  private static readonly DoubleMatrix RISK_REVERSAL = DoubleMatrix.ofUnsafe(new double[][]
	  {
		  new double[] {-0.010, -0.0050},
		  new double[] {-0.011, -0.0060},
		  new double[] {-0.012, -0.0070},
		  new double[] {-0.013, -0.0080},
		  new double[] {-0.014, -0.0090},
		  new double[] {-0.014, -0.0090}
	  });
	  private static readonly DoubleMatrix STRANGLE = DoubleMatrix.ofUnsafe(new double[][]
	  {
		  new double[] {0.0300, 0.0100},
		  new double[] {0.0310, 0.0110},
		  new double[] {0.0320, 0.0120},
		  new double[] {0.0330, 0.0130},
		  new double[] {0.0340, 0.0140},
		  new double[] {0.0340, 0.0140}
	  });
	  private static readonly InterpolatedStrikeSmileDeltaTermStructure SMILE_TERM = InterpolatedStrikeSmileDeltaTermStructure.of(TIME_TO_EXPIRY, DELTA, ATM, RISK_REVERSAL, STRANGLE, ACT_365F);
	  private static readonly CurrencyPair CURRENCY_PAIR = CurrencyPair.of(EUR, USD);
	  private static readonly BlackFxOptionSmileVolatilities VOLS = BlackFxOptionSmileVolatilities.of(FxOptionVolatilitiesName.of("Test"), CURRENCY_PAIR, VAL_DATE_TIME, SMILE_TERM);

	  private static readonly LocalDate PAYMENT_DATE = LocalDate.of(2014, 5, 13);
	  private const double NOTIONAL = 1.0e6;
	  private static readonly CurrencyAmount EUR_AMOUNT = CurrencyAmount.of(EUR, NOTIONAL);
	  private static readonly CurrencyAmount USD_AMOUNT = CurrencyAmount.of(USD, -NOTIONAL * FX_MATRIX.fxRate(EUR, USD));
	  private static readonly ResolvedFxSingle FX_PRODUCT = ResolvedFxSingle.of(EUR_AMOUNT, USD_AMOUNT, PAYMENT_DATE);
	  private static readonly ResolvedFxVanillaOption OPTION_PRODUCT = ResolvedFxVanillaOption.builder().longShort(SHORT).expiry(EXPIRY).underlying(FX_PRODUCT).build();
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.builder().tradeDate(VAL_DATE).build();
	  private static readonly LocalDate CASH_SETTLE_DATE = LocalDate.of(2014, 1, 25);
	  private static readonly Payment PREMIUM = Payment.of(EUR, NOTIONAL * 0.027, CASH_SETTLE_DATE);
	  private static readonly ResolvedFxVanillaOptionTrade OPTION_TRADE = ResolvedFxVanillaOptionTrade.builder().premium(PREMIUM).product(OPTION_PRODUCT).info(TRADE_INFO).build();

	  private static readonly BlackFxVanillaOptionProductPricer PRICER_PRODUCT = BlackFxVanillaOptionProductPricer.DEFAULT;
	  private static readonly BlackFxVanillaOptionTradePricer PRICER_TRADE = BlackFxVanillaOptionTradePricer.DEFAULT;
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
		PointSensitivities pvSensiProduct = PRICER_PRODUCT.presentValueSensitivityRatesStickyStrike(OPTION_PRODUCT, RATES_PROVIDER, VOLS);
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
		PointSensitivities point = PRICER_TRADE.presentValueSensitivityRatesStickyStrike(OPTION_TRADE, RATES_PROVIDER, VOLS);
		MultiCurrencyAmount pv = PRICER_TRADE.presentValue(OPTION_TRADE, RATES_PROVIDER, VOLS);
		MultiCurrencyAmount ceExpected = RATES_PROVIDER.currencyExposure(point).plus(pv);
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