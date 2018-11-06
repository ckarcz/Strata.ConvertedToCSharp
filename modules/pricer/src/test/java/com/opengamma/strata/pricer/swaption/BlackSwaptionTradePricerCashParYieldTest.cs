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
//	import static com.opengamma.strata.collect.TestHelper.date;
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
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProviderDataSets = com.opengamma.strata.pricer.datasets.RatesProviderDataSets;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using LongShort = com.opengamma.strata.product.common.LongShort;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using CashSwaptionSettlement = com.opengamma.strata.product.swaption.CashSwaptionSettlement;
	using CashSwaptionSettlementMethod = com.opengamma.strata.product.swaption.CashSwaptionSettlementMethod;
	using ResolvedSwaption = com.opengamma.strata.product.swaption.ResolvedSwaption;
	using ResolvedSwaptionTrade = com.opengamma.strata.product.swaption.ResolvedSwaptionTrade;
	using Swaption = com.opengamma.strata.product.swaption.Swaption;

	/// <summary>
	/// Test <seealso cref="BlackSwaptionTradePricer"/> for cash par yield.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BlackSwaptionTradePricerCashParYieldTest
	public class BlackSwaptionTradePricerCashParYieldTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = date(2015, 8, 7);
	  private static readonly LocalDate SWAPTION_EXERCISE_DATE = VAL_DATE.plusYears(5);
	  private static readonly LocalTime SWAPTION_EXPIRY_TIME = LocalTime.of(11, 0);
	  private static readonly ZoneId SWAPTION_EXPIRY_ZONE = ZoneId.of("America/New_York");
	  private static readonly LocalDate SETTLE_DATE = USD_LIBOR_3M.calculateEffectiveFromFixing(SWAPTION_EXERCISE_DATE, REF_DATA);
	  private const int SWAP_TENOR_YEAR = 5;
	  private static readonly Period SWAP_TENOR = Period.ofYears(SWAP_TENOR_YEAR);
	  private static readonly LocalDate SWAP_MATURITY_DATE = SETTLE_DATE.plus(SWAP_TENOR);
	  private const double STRIKE = 0.01;
	  private const double NOTIONAL = 100_000_000;
	  private static readonly Swap SWAP_REC = USD_FIXED_6M_LIBOR_3M.toTrade(VAL_DATE, SETTLE_DATE, SWAP_MATURITY_DATE, SELL, NOTIONAL, STRIKE).Product;
	  private static readonly CashSwaptionSettlement PAR_YIELD = CashSwaptionSettlement.of(SETTLE_DATE, CashSwaptionSettlementMethod.PAR_YIELD);
	  private const double PREMIUM_AMOUNT = 100_000;
	  private static readonly ResolvedSwaption SWAPTION_LONG_REC = Swaption.builder().swaptionSettlement(PAR_YIELD).expiryDate(AdjustableDate.of(SWAPTION_EXERCISE_DATE)).expiryTime(SWAPTION_EXPIRY_TIME).expiryZone(SWAPTION_EXPIRY_ZONE).longShort(LongShort.LONG).underlying(SWAP_REC).build().resolve(REF_DATA);
	  private static readonly Payment PREMIUM_FWD_PAY = Payment.of(CurrencyAmount.of(USD, -PREMIUM_AMOUNT), SETTLE_DATE);
	  private static readonly ResolvedSwaptionTrade SWAPTION_PREFWD_LONG_REC = ResolvedSwaptionTrade.builder().product(SWAPTION_LONG_REC).premium(PREMIUM_FWD_PAY).build();
	  private static readonly Payment PREMIUM_TRA_PAY = Payment.of(CurrencyAmount.of(USD, -PREMIUM_AMOUNT), VAL_DATE);
	  private static readonly ResolvedSwaptionTrade SWAPTION_PRETOD_LONG_REC = ResolvedSwaptionTrade.builder().product(SWAPTION_LONG_REC).premium(PREMIUM_TRA_PAY).build();
	  private static readonly Payment PREMIUM_PAST_PAY = Payment.of(CurrencyAmount.of(USD, -PREMIUM_AMOUNT), VAL_DATE.minusDays(1));
	  private static readonly ResolvedSwaptionTrade SWAPTION_PREPAST_LONG_REC = ResolvedSwaptionTrade.builder().product(SWAPTION_LONG_REC).premium(PREMIUM_PAST_PAY).build();

	  private static readonly ImmutableRatesProvider RATE_PROVIDER = RatesProviderDataSets.multiUsd(VAL_DATE);
	  private static readonly BlackSwaptionExpiryTenorVolatilities VOLS = SwaptionBlackVolatilityDataSets.BLACK_SWAPTION_VOLS_CST_USD;

	  private static readonly VolatilitySwaptionTradePricer PRICER_COMMON = VolatilitySwaptionTradePricer.DEFAULT;
	  private static readonly BlackSwaptionTradePricer PRICER_TRADE = BlackSwaptionTradePricer.DEFAULT;
	  private static readonly BlackSwaptionCashParYieldProductPricer PRICER_PRODUCT = BlackSwaptionCashParYieldProductPricer.DEFAULT;
	  private static readonly DiscountingPaymentPricer PRICER_PAYMENT = DiscountingPaymentPricer.DEFAULT;

	  private const double TOL = 1.0e-12;

	  //-------------------------------------------------------------------------
	  public virtual void present_value_premium_forward()
	  {
		CurrencyAmount pvTrade = PRICER_TRADE.presentValue(SWAPTION_PREFWD_LONG_REC, RATE_PROVIDER, VOLS);
		CurrencyAmount pvProduct = PRICER_PRODUCT.presentValue(SWAPTION_LONG_REC, RATE_PROVIDER, VOLS);
		CurrencyAmount pvPremium = PRICER_PAYMENT.presentValue(PREMIUM_FWD_PAY, RATE_PROVIDER);
		assertEquals(pvTrade.Amount, pvProduct.Amount + pvPremium.Amount, NOTIONAL * TOL);
		// test via VolatilitySwaptionTradePricer
		CurrencyAmount pv = PRICER_COMMON.presentValue(SWAPTION_PREFWD_LONG_REC, RATE_PROVIDER, VOLS);
		assertEquals(pv, pvTrade);
	  }

	  public virtual void present_value_premium_valuedate()
	  {
		CurrencyAmount pvTrade = PRICER_TRADE.presentValue(SWAPTION_PRETOD_LONG_REC, RATE_PROVIDER, VOLS);
		CurrencyAmount pvProduct = PRICER_PRODUCT.presentValue(SWAPTION_LONG_REC, RATE_PROVIDER, VOLS);
		CurrencyAmount pvPremium = PRICER_PAYMENT.presentValue(PREMIUM_TRA_PAY, RATE_PROVIDER);
		assertEquals(pvTrade.Amount, pvProduct.Amount + pvPremium.Amount, NOTIONAL * TOL);
	  }

	  public virtual void present_value_premium_past()
	  {
		CurrencyAmount pvTrade = PRICER_TRADE.presentValue(SWAPTION_PREPAST_LONG_REC, RATE_PROVIDER, VOLS);
		CurrencyAmount pvProduct = PRICER_PRODUCT.presentValue(SWAPTION_LONG_REC, RATE_PROVIDER, VOLS);
		assertEquals(pvTrade.Amount, pvProduct.Amount, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void currency_exposure_premium_forward()
	  {
		CurrencyAmount pv = PRICER_TRADE.presentValue(SWAPTION_PREFWD_LONG_REC, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount ce = PRICER_TRADE.currencyExposure(SWAPTION_PREFWD_LONG_REC, RATE_PROVIDER, VOLS);
		assertEquals(pv.Amount, ce.getAmount(USD).Amount, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void current_cash_forward()
	  {
		CurrencyAmount ccTrade = PRICER_TRADE.currentCash(SWAPTION_PREFWD_LONG_REC, VAL_DATE);
		assertEquals(ccTrade.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void current_cash_vd()
	  {
		CurrencyAmount ccTrade = PRICER_TRADE.currentCash(SWAPTION_PRETOD_LONG_REC, VAL_DATE);
		assertEquals(ccTrade.Amount, -PREMIUM_AMOUNT, NOTIONAL * TOL);
	  }

	  public virtual void current_cash_past()
	  {
		CurrencyAmount ccTrade = PRICER_TRADE.currentCash(SWAPTION_PREPAST_LONG_REC, VAL_DATE);
		assertEquals(ccTrade.Amount, 0d, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void present_value_sensitivity_premium_forward()
	  {
		PointSensitivities pvcsTrade = PRICER_TRADE.presentValueSensitivityRatesStickyStrike(SWAPTION_PREFWD_LONG_REC, RATE_PROVIDER, VOLS);
		PointSensitivityBuilder pvcsProduct = PRICER_PRODUCT.presentValueSensitivityRatesStickyStrike(SWAPTION_LONG_REC, RATE_PROVIDER, VOLS);
		PointSensitivityBuilder pvcsPremium = PRICER_PAYMENT.presentValueSensitivity(PREMIUM_FWD_PAY, RATE_PROVIDER);
		CurrencyParameterSensitivities pvpsTrade = RATE_PROVIDER.parameterSensitivity(pvcsTrade);
		CurrencyParameterSensitivities pvpsProduct = RATE_PROVIDER.parameterSensitivity(pvcsProduct.combinedWith(pvcsPremium).build());
		assertTrue(pvpsTrade.equalWithTolerance(pvpsProduct, NOTIONAL * TOL));
	  }

	  public virtual void present_value_sensitivity_premium_valuedate()
	  {
		PointSensitivities pvcsTrade = PRICER_TRADE.presentValueSensitivityRatesStickyStrike(SWAPTION_PRETOD_LONG_REC, RATE_PROVIDER, VOLS);
		PointSensitivityBuilder pvcsProduct = PRICER_PRODUCT.presentValueSensitivityRatesStickyStrike(SWAPTION_LONG_REC, RATE_PROVIDER, VOLS);
		CurrencyParameterSensitivities pvpsTrade = RATE_PROVIDER.parameterSensitivity(pvcsTrade);
		CurrencyParameterSensitivities pvpsProduct = RATE_PROVIDER.parameterSensitivity(pvcsProduct.build());
		assertTrue(pvpsTrade.equalWithTolerance(pvpsProduct, NOTIONAL * TOL));
	  }

	  public virtual void present_value_sensitivity_premium_past()
	  {
		PointSensitivities pvcsTrade = PRICER_TRADE.presentValueSensitivityRatesStickyStrike(SWAPTION_PREPAST_LONG_REC, RATE_PROVIDER, VOLS);
		PointSensitivityBuilder pvcsProduct = PRICER_PRODUCT.presentValueSensitivityRatesStickyStrike(SWAPTION_LONG_REC, RATE_PROVIDER, VOLS);
		CurrencyParameterSensitivities pvpsTrade = RATE_PROVIDER.parameterSensitivity(pvcsTrade);
		CurrencyParameterSensitivities pvpsProduct = RATE_PROVIDER.parameterSensitivity(pvcsProduct.build());
		assertTrue(pvpsTrade.equalWithTolerance(pvpsProduct, NOTIONAL * TOL));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void present_value_black_vol_sensitivity_premium_forward()
	  {
		PointSensitivities vegaTrade = PRICER_TRADE.presentValueSensitivityModelParamsVolatility(SWAPTION_PREFWD_LONG_REC, RATE_PROVIDER, VOLS);
		SwaptionSensitivity vegaProduct = PRICER_PRODUCT.presentValueSensitivityModelParamsVolatility(SWAPTION_LONG_REC, RATE_PROVIDER, VOLS);
		assertEquals(vegaTrade.Sensitivities.get(0).Sensitivity, vegaProduct.Sensitivity, NOTIONAL * TOL);
	  }

	}

}