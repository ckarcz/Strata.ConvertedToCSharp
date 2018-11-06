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
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using LongShort = com.opengamma.strata.product.common.LongShort;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using CashSwaptionSettlement = com.opengamma.strata.product.swaption.CashSwaptionSettlement;
	using CashSwaptionSettlementMethod = com.opengamma.strata.product.swaption.CashSwaptionSettlementMethod;
	using ResolvedSwaption = com.opengamma.strata.product.swaption.ResolvedSwaption;
	using ResolvedSwaptionTrade = com.opengamma.strata.product.swaption.ResolvedSwaptionTrade;
	using Swaption = com.opengamma.strata.product.swaption.Swaption;

	/// <summary>
	/// Test <seealso cref="SabrSwaptionTradePricer"/> for cash par yield.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrSwaptionCashParYieldTradePricerTest
	public class SabrSwaptionCashParYieldTradePricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = LocalDate.of(2014, 1, 22);
	  // swaption trades
	  private const double NOTIONAL = 100000000; //100m
	  private const double RATE = 0.0350;
	  private const int TENOR_YEAR = 7;
	  private static readonly Tenor TENOR = Tenor.ofYears(TENOR_YEAR);
	  private static readonly ZonedDateTime MATURITY_DATE = LocalDate.of(2016, 1, 22).atStartOfDay(ZoneOffset.UTC); // 2Y
	  private static readonly Swap SWAP_REC = SwaptionSabrRateVolatilityDataSet.SWAP_CONVENTION_USD.createTrade(MATURITY_DATE.toLocalDate(), TENOR, BuySell.SELL, NOTIONAL, RATE, REF_DATA).Product;
	  private static readonly LocalDate SETTLE_DATE = SwaptionSabrRateVolatilityDataSet.SWAP_CONVENTION_USD.FloatingLeg.Index.calculateEffectiveFromFixing(MATURITY_DATE.toLocalDate(), REF_DATA);
	  private static readonly CashSwaptionSettlement PAR_YIELD = CashSwaptionSettlement.of(SETTLE_DATE, CashSwaptionSettlementMethod.PAR_YIELD);
	  private static readonly ResolvedSwaption SWAPTION_LONG_REC = Swaption.builder().expiryDate(AdjustableDate.of(MATURITY_DATE.toLocalDate())).expiryTime(MATURITY_DATE.toLocalTime()).expiryZone(MATURITY_DATE.Zone).longShort(LongShort.LONG).swaptionSettlement(PAR_YIELD).underlying(SWAP_REC).build().resolve(REF_DATA);
	  private const double PREMIUM_AMOUNT = 100_000;
	  private static readonly Payment PREMIUM_FWD_PAY = Payment.of(CurrencyAmount.of(USD, -PREMIUM_AMOUNT), MATURITY_DATE.toLocalDate());
	  private static readonly ResolvedSwaptionTrade SWAPTION_PREFWD_LONG_REC = ResolvedSwaptionTrade.builder().product(SWAPTION_LONG_REC).premium(PREMIUM_FWD_PAY).build();
	  private static readonly Payment PREMIUM_TRA_PAY = Payment.of(CurrencyAmount.of(USD, -PREMIUM_AMOUNT), VAL_DATE);
	  private static readonly ResolvedSwaptionTrade SWAPTION_PRETOD_LONG_REC = ResolvedSwaptionTrade.builder().product(SWAPTION_LONG_REC).premium(PREMIUM_TRA_PAY).build();
	  private static readonly Payment PREMIUM_PAST_PAY = Payment.of(CurrencyAmount.of(USD, -PREMIUM_AMOUNT), VAL_DATE.minusDays(1));
	  private static readonly ResolvedSwaptionTrade SWAPTION_PREPAST_LONG_REC = ResolvedSwaptionTrade.builder().product(SWAPTION_LONG_REC).premium(PREMIUM_PAST_PAY).build();
	  // providers
	  private static readonly ImmutableRatesProvider RATE_PROVIDER = SwaptionSabrRateVolatilityDataSet.getRatesProviderUsd(VAL_DATE);
	  private static readonly SabrSwaptionVolatilities VOLS = SwaptionSabrRateVolatilityDataSet.getVolatilitiesUsd(VAL_DATE, true);

	  private const double TOL = 1.0e-12;
	  private static readonly VolatilitySwaptionTradePricer PRICER_COMMON = VolatilitySwaptionTradePricer.DEFAULT;
	  private static readonly SabrSwaptionTradePricer PRICER_TRADE = SabrSwaptionTradePricer.DEFAULT;
	  private static readonly SabrSwaptionCashParYieldProductPricer PRICER_PRODUCT = SabrSwaptionCashParYieldProductPricer.DEFAULT;
	  private static readonly DiscountingPaymentPricer PRICER_PAYMENT = DiscountingPaymentPricer.DEFAULT;

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
		assertEquals(ccTrade.Amount, 0, NOTIONAL * TOL);
	  }

	  public virtual void current_cash_vd()
	  {
		CurrencyAmount ccTrade = PRICER_TRADE.currentCash(SWAPTION_PRETOD_LONG_REC, VAL_DATE);
		assertEquals(ccTrade.Amount, -PREMIUM_AMOUNT, NOTIONAL * TOL);
	  }

	  public virtual void current_cash_past()
	  {
		CurrencyAmount ccTrade = PRICER_TRADE.currentCash(SWAPTION_PREPAST_LONG_REC, VAL_DATE);
		assertEquals(ccTrade.Amount, 0, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void present_value_sensitivity_premium_forward()
	  {
		PointSensitivities pvcsTrade = PRICER_TRADE.presentValueSensitivityRatesStickyModel(SWAPTION_PREFWD_LONG_REC, RATE_PROVIDER, VOLS);
		PointSensitivityBuilder pvcsProduct = PRICER_PRODUCT.presentValueSensitivityRatesStickyModel(SWAPTION_LONG_REC, RATE_PROVIDER, VOLS);
		PointSensitivityBuilder pvcsPremium = PRICER_PAYMENT.presentValueSensitivity(PREMIUM_FWD_PAY, RATE_PROVIDER);
		CurrencyParameterSensitivities pvpsTrade = RATE_PROVIDER.parameterSensitivity(pvcsTrade);
		CurrencyParameterSensitivities pvpsProduct = RATE_PROVIDER.parameterSensitivity(pvcsProduct.combinedWith(pvcsPremium).build());
		assertTrue(pvpsTrade.equalWithTolerance(pvpsProduct, NOTIONAL * NOTIONAL * TOL));
	  }

	  public virtual void present_value_sensitivity_premium_valuedate()
	  {
		PointSensitivities pvcsTrade = PRICER_TRADE.presentValueSensitivityRatesStickyModel(SWAPTION_PRETOD_LONG_REC, RATE_PROVIDER, VOLS);
		PointSensitivityBuilder pvcsProduct = PRICER_PRODUCT.presentValueSensitivityRatesStickyModel(SWAPTION_LONG_REC, RATE_PROVIDER, VOLS);
		CurrencyParameterSensitivities pvpsTrade = RATE_PROVIDER.parameterSensitivity(pvcsTrade);
		CurrencyParameterSensitivities pvpsProduct = RATE_PROVIDER.parameterSensitivity(pvcsProduct.build());
		assertTrue(pvpsTrade.equalWithTolerance(pvpsProduct, NOTIONAL * NOTIONAL * TOL));
	  }

	  public virtual void present_value_sensitivity_premium_past()
	  {
		PointSensitivities pvcsTrade = PRICER_TRADE.presentValueSensitivityRatesStickyModel(SWAPTION_PREPAST_LONG_REC, RATE_PROVIDER, VOLS);
		PointSensitivityBuilder pvcsProduct = PRICER_PRODUCT.presentValueSensitivityRatesStickyModel(SWAPTION_LONG_REC, RATE_PROVIDER, VOLS);
		CurrencyParameterSensitivities pvpsTrade = RATE_PROVIDER.parameterSensitivity(pvcsTrade);
		CurrencyParameterSensitivities pvpsProduct = RATE_PROVIDER.parameterSensitivity(pvcsProduct.build());
		assertTrue(pvpsTrade.equalWithTolerance(pvpsProduct, NOTIONAL * NOTIONAL * TOL));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void present_value_vol_sensitivity_premium_forward()
	  {
		PointSensitivities vegaTrade = PRICER_TRADE.presentValueSensitivityModelParamsSabr(SWAPTION_PREFWD_LONG_REC, RATE_PROVIDER, VOLS);
		PointSensitivities vegaProduct = PRICER_PRODUCT.presentValueSensitivityModelParamsSabr(SWAPTION_LONG_REC, RATE_PROVIDER, VOLS).build();
		assertEquals(vegaTrade, vegaProduct);
	  }

	}

}