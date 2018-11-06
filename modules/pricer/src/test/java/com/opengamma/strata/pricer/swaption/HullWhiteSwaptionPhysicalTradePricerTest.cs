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
//	import static com.opengamma.strata.product.common.BuySell.SELL;
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
	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProviderDataSets = com.opengamma.strata.pricer.datasets.RatesProviderDataSets;
	using HullWhiteIborFutureDataSet = com.opengamma.strata.pricer.index.HullWhiteIborFutureDataSet;
	using HullWhiteOneFactorPiecewiseConstantParametersProvider = com.opengamma.strata.pricer.model.HullWhiteOneFactorPiecewiseConstantParametersProvider;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using LongShort = com.opengamma.strata.product.common.LongShort;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using FixedIborSwapConventions = com.opengamma.strata.product.swap.type.FixedIborSwapConventions;
	using PhysicalSwaptionSettlement = com.opengamma.strata.product.swaption.PhysicalSwaptionSettlement;
	using ResolvedSwaption = com.opengamma.strata.product.swaption.ResolvedSwaption;
	using ResolvedSwaptionTrade = com.opengamma.strata.product.swaption.ResolvedSwaptionTrade;
	using Swaption = com.opengamma.strata.product.swaption.Swaption;
	using SwaptionSettlement = com.opengamma.strata.product.swaption.SwaptionSettlement;

	/// <summary>
	/// Test <seealso cref="HullWhiteSwaptionPhysicalTradePricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class HullWhiteSwaptionPhysicalTradePricerTest
	public class HullWhiteSwaptionPhysicalTradePricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = RatesProviderDataSets.VAL_DATE_2014_01_22;
	  private static readonly LocalDate SWAPTION_EXERCISE_DATE = VAL_DATE.plusYears(5);
	  private static readonly LocalTime SWAPTION_EXPIRY_TIME = LocalTime.of(11, 0);
	  private static readonly ZoneId SWAPTION_EXPIRY_ZONE = ZoneId.of("America/New_York");
	  private static readonly LocalDate SWAP_EFFECTIVE_DATE = USD_LIBOR_3M.calculateEffectiveFromFixing(SWAPTION_EXERCISE_DATE, REF_DATA);
	  private const int SWAP_TENOR_YEAR = 5;
	  private static readonly Period SWAP_TENOR = Period.ofYears(SWAP_TENOR_YEAR);
	  private static readonly LocalDate SWAP_MATURITY_DATE = SWAP_EFFECTIVE_DATE.plus(SWAP_TENOR);
	  private const double STRIKE = 0.01;
	  private const double NOTIONAL = 100_000_000;
	  private static readonly Swap SWAP_REC = FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M.toTrade(VAL_DATE, SWAP_EFFECTIVE_DATE, SWAP_MATURITY_DATE, SELL, NOTIONAL, STRIKE).Product;
	  private static readonly SwaptionSettlement PHYSICAL_SETTLE = PhysicalSwaptionSettlement.DEFAULT;

	  private const double PREMIUM_AMOUNT = 100_000;
	  private static readonly ResolvedSwaption SWAPTION_LONG_REC = Swaption.builder().swaptionSettlement(PHYSICAL_SETTLE).expiryDate(AdjustableDate.of(SWAPTION_EXERCISE_DATE)).expiryTime(SWAPTION_EXPIRY_TIME).expiryZone(SWAPTION_EXPIRY_ZONE).longShort(LongShort.LONG).underlying(SWAP_REC).build().resolve(REF_DATA);
	  private static readonly Payment PREMIUM_FWD_PAY = Payment.of(CurrencyAmount.of(USD, -PREMIUM_AMOUNT), SWAP_EFFECTIVE_DATE);
	  private static readonly ResolvedSwaptionTrade SWAPTION_PREFWD_LONG_REC = ResolvedSwaptionTrade.builder().product(SWAPTION_LONG_REC).premium(PREMIUM_FWD_PAY).build();
	  private static readonly Payment PREMIUM_TRA_PAY = Payment.of(CurrencyAmount.of(USD, -PREMIUM_AMOUNT), VAL_DATE);
	  private static readonly ResolvedSwaptionTrade SWAPTION_PRETOD_LONG_REC = ResolvedSwaptionTrade.builder().product(SWAPTION_LONG_REC).premium(PREMIUM_TRA_PAY).build();
	  private static readonly Payment PREMIUM_PAST_PAY = Payment.of(CurrencyAmount.of(USD, -PREMIUM_AMOUNT), VAL_DATE.minusDays(1));
	  private static readonly ResolvedSwaptionTrade SWAPTION_PREPAST_LONG_REC = ResolvedSwaptionTrade.builder().product(SWAPTION_LONG_REC).premium(PREMIUM_PAST_PAY).build();

	  private static readonly HullWhiteSwaptionPhysicalProductPricer PRICER_PRODUCT = HullWhiteSwaptionPhysicalProductPricer.DEFAULT;
	  private static readonly HullWhiteSwaptionPhysicalTradePricer PRICER_TRADE = HullWhiteSwaptionPhysicalTradePricer.DEFAULT;
	  private static readonly DiscountingPaymentPricer PRICER_PAYMENT = DiscountingPaymentPricer.DEFAULT;

	  private static readonly ImmutableRatesProvider MULTI_USD = RatesProviderDataSets.multiUsd(VAL_DATE);
	  private static readonly HullWhiteOneFactorPiecewiseConstantParametersProvider HW_PROVIDER = HullWhiteIborFutureDataSet.createHullWhiteProvider(VAL_DATE);

	  private const double TOL = 1.0E-12;

	  //-------------------------------------------------------------------------
	  public virtual void present_value_premium_forward()
	  {
		CurrencyAmount pvTrade = PRICER_TRADE.presentValue(SWAPTION_PREFWD_LONG_REC, MULTI_USD, HW_PROVIDER);
		CurrencyAmount pvProduct = PRICER_PRODUCT.presentValue(SWAPTION_LONG_REC, MULTI_USD, HW_PROVIDER);
		CurrencyAmount pvPremium = PRICER_PAYMENT.presentValue(PREMIUM_FWD_PAY, MULTI_USD);
		assertEquals(pvTrade.Amount, pvProduct.Amount + pvPremium.Amount, TOL * NOTIONAL);
	  }

	  public virtual void present_value_premium_valuedate()
	  {
		CurrencyAmount pvTrade = PRICER_TRADE.presentValue(SWAPTION_PRETOD_LONG_REC, MULTI_USD, HW_PROVIDER);
		CurrencyAmount pvProduct = PRICER_PRODUCT.presentValue(SWAPTION_LONG_REC, MULTI_USD, HW_PROVIDER);
		CurrencyAmount pvPremium = PRICER_PAYMENT.presentValue(PREMIUM_TRA_PAY, MULTI_USD);
		assertEquals(pvTrade.Amount, pvProduct.Amount + pvPremium.Amount, TOL * NOTIONAL);
	  }

	  public virtual void present_value_premium_past()
	  {
		CurrencyAmount pvTrade = PRICER_TRADE.presentValue(SWAPTION_PREPAST_LONG_REC, MULTI_USD, HW_PROVIDER);
		CurrencyAmount pvProduct = PRICER_PRODUCT.presentValue(SWAPTION_LONG_REC, MULTI_USD, HW_PROVIDER);
		assertEquals(pvTrade.Amount, pvProduct.Amount, TOL * NOTIONAL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void currency_exposure_premium_forward()
	  {
		CurrencyAmount pv = PRICER_TRADE.presentValue(SWAPTION_PREFWD_LONG_REC, MULTI_USD, HW_PROVIDER);
		MultiCurrencyAmount ce = PRICER_TRADE.currencyExposure(SWAPTION_PREFWD_LONG_REC, MULTI_USD, HW_PROVIDER);
		assertEquals(pv.Amount, ce.getAmount(USD).Amount, TOL * NOTIONAL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void current_cash_forward()
	  {
		CurrencyAmount ccTrade = PRICER_TRADE.currentCash(SWAPTION_PREFWD_LONG_REC, VAL_DATE);
		assertEquals(ccTrade.Amount, 0, TOL * NOTIONAL);
	  }

	  public virtual void current_cash_vd()
	  {
		CurrencyAmount ccTrade = PRICER_TRADE.currentCash(SWAPTION_PRETOD_LONG_REC, VAL_DATE);
		assertEquals(ccTrade.Amount, -PREMIUM_AMOUNT, TOL * NOTIONAL);
	  }

	  public virtual void current_cash_past()
	  {
		CurrencyAmount ccTrade = PRICER_TRADE.currentCash(SWAPTION_PREPAST_LONG_REC, VAL_DATE);
		assertEquals(ccTrade.Amount, 0, TOL * NOTIONAL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void present_value_sensitivity_premium_forward()
	  {
		PointSensitivities pvcsTrade = PRICER_TRADE.presentValueSensitivityRates(SWAPTION_PREFWD_LONG_REC, MULTI_USD, HW_PROVIDER);
		PointSensitivityBuilder pvcsProduct = PRICER_PRODUCT.presentValueSensitivityRates(SWAPTION_LONG_REC, MULTI_USD, HW_PROVIDER);
		PointSensitivityBuilder pvcsPremium = PRICER_PAYMENT.presentValueSensitivity(PREMIUM_FWD_PAY, MULTI_USD);
		CurrencyParameterSensitivities pvpsTrade = MULTI_USD.parameterSensitivity(pvcsTrade);
		CurrencyParameterSensitivities pvpsProduct = MULTI_USD.parameterSensitivity(pvcsProduct.combinedWith(pvcsPremium).build());
		assertTrue(pvpsTrade.equalWithTolerance(pvpsProduct, TOL * NOTIONAL));
	  }

	  public virtual void present_value_sensitivity_premium_valuedate()
	  {
		PointSensitivities pvcsTrade = PRICER_TRADE.presentValueSensitivityRates(SWAPTION_PRETOD_LONG_REC, MULTI_USD, HW_PROVIDER);
		PointSensitivityBuilder pvcsProduct = PRICER_PRODUCT.presentValueSensitivityRates(SWAPTION_LONG_REC, MULTI_USD, HW_PROVIDER);
		CurrencyParameterSensitivities pvpsTrade = MULTI_USD.parameterSensitivity(pvcsTrade);
		CurrencyParameterSensitivities pvpsProduct = MULTI_USD.parameterSensitivity(pvcsProduct.build());
		assertTrue(pvpsTrade.equalWithTolerance(pvpsProduct, TOL * NOTIONAL));
	  }

	  public virtual void present_value_sensitivity_premium_past()
	  {
		PointSensitivities pvcsTrade = PRICER_TRADE.presentValueSensitivityRates(SWAPTION_PREPAST_LONG_REC, MULTI_USD, HW_PROVIDER);
		PointSensitivityBuilder pvcsProduct = PRICER_PRODUCT.presentValueSensitivityRates(SWAPTION_LONG_REC, MULTI_USD, HW_PROVIDER);
		CurrencyParameterSensitivities pvpsTrade = MULTI_USD.parameterSensitivity(pvcsTrade);
		CurrencyParameterSensitivities pvpsProduct = MULTI_USD.parameterSensitivity(pvcsProduct.build());
		assertTrue(pvpsTrade.equalWithTolerance(pvpsProduct, TOL * NOTIONAL));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void present_value_hw_param_sensitivity_premium_forward()
	  {
		DoubleArray hwTrade = PRICER_TRADE.presentValueSensitivityModelParamsHullWhite(SWAPTION_PREFWD_LONG_REC, MULTI_USD, HW_PROVIDER);
		DoubleArray hwProduct = PRICER_PRODUCT.presentValueSensitivityModelParamsHullWhite(SWAPTION_LONG_REC, MULTI_USD, HW_PROVIDER);
		assertTrue(DoubleArrayMath.fuzzyEquals(hwTrade.toArray(), hwProduct.toArray(), TOL * NOTIONAL));
	  }

	}

}