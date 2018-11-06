using System;
using System.Collections.Generic;

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
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.fail;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoublesPair = com.opengamma.strata.collect.tuple.DoublesPair;
	using SabrParameterType = com.opengamma.strata.market.model.SabrParameterType;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using ConstantSurface = com.opengamma.strata.market.surface.ConstantSurface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using BuySell = com.opengamma.strata.product.common.BuySell;
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
	/// Test <seealso cref="SabrSwaptionPhysicalProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrSwaptionPhysicalProductPricerTest
	public class SabrSwaptionPhysicalProductPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = LocalDate.of(2014, 1, 22);
	  // swaptions
	  private const double NOTIONAL = 100000000; //100m
	  private const double RATE = 0.0350;
	  private const int TENOR_YEAR = 7;
	  private static readonly Tenor TENOR = Tenor.ofYears(TENOR_YEAR);
	  private static readonly ZonedDateTime MATURITY_DATE = LocalDate.of(2016, 1, 22).atStartOfDay(ZoneOffset.UTC); // 2Y
	  private static readonly Swap SWAP_PAY = SwaptionSabrRateVolatilityDataSet.SWAP_CONVENTION_USD.createTrade(MATURITY_DATE.toLocalDate(), TENOR, BuySell.BUY, NOTIONAL, RATE, REF_DATA).Product;
	  private static readonly ResolvedSwap RSWAP_PAY = SWAP_PAY.resolve(REF_DATA);
	  private static readonly Swap SWAP_REC = SwaptionSabrRateVolatilityDataSet.SWAP_CONVENTION_USD.createTrade(MATURITY_DATE.toLocalDate(), TENOR, BuySell.SELL, NOTIONAL, RATE, REF_DATA).Product;
	  private static readonly ResolvedSwap RSWAP_REC = SWAP_REC.resolve(REF_DATA);
	  private static readonly SwaptionSettlement PHYSICAL_SETTLE = PhysicalSwaptionSettlement.DEFAULT;
	  private static readonly SwaptionSettlement CASH_SETTLE = CashSwaptionSettlement.of(SWAP_REC.StartDate.Unadjusted, CashSwaptionSettlementMethod.PAR_YIELD);
	  private static readonly ResolvedSwaption SWAPTION_PAY_LONG = Swaption.builder().expiryDate(AdjustableDate.of(MATURITY_DATE.toLocalDate())).expiryTime(MATURITY_DATE.toLocalTime()).expiryZone(MATURITY_DATE.Zone).longShort(LongShort.LONG).swaptionSettlement(PHYSICAL_SETTLE).underlying(SWAP_PAY).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_PAY_SHORT = Swaption.builder().expiryDate(AdjustableDate.of(MATURITY_DATE.toLocalDate())).expiryTime(MATURITY_DATE.toLocalTime()).expiryZone(MATURITY_DATE.Zone).longShort(LongShort.SHORT).swaptionSettlement(PHYSICAL_SETTLE).underlying(SWAP_PAY).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_REC_LONG = Swaption.builder().expiryDate(AdjustableDate.of(MATURITY_DATE.toLocalDate())).expiryTime(MATURITY_DATE.toLocalTime()).expiryZone(MATURITY_DATE.Zone).longShort(LongShort.LONG).swaptionSettlement(PHYSICAL_SETTLE).underlying(SWAP_REC).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_REC_SHORT = Swaption.builder().expiryDate(AdjustableDate.of(MATURITY_DATE.toLocalDate())).expiryTime(MATURITY_DATE.toLocalTime()).expiryZone(MATURITY_DATE.Zone).longShort(LongShort.SHORT).swaptionSettlement(PHYSICAL_SETTLE).underlying(SWAP_REC).build().resolve(REF_DATA);
	  private static readonly ResolvedSwaption SWAPTION_CASH = Swaption.builder().expiryDate(AdjustableDate.of(MATURITY_DATE.toLocalDate())).expiryTime(MATURITY_DATE.toLocalTime()).expiryZone(MATURITY_DATE.Zone).longShort(LongShort.LONG).swaptionSettlement(CASH_SETTLE).underlying(SWAP_REC).build().resolve(REF_DATA);
	  // providers
	  private static readonly ImmutableRatesProvider RATE_PROVIDER = SwaptionSabrRateVolatilityDataSet.getRatesProviderUsd(VAL_DATE);
	  private static readonly ImmutableRatesProvider RATE_PROVIDER_AT_MATURITY = SwaptionSabrRateVolatilityDataSet.getRatesProviderUsd(MATURITY_DATE.toLocalDate());
	  private static readonly ImmutableRatesProvider RATE_PROVIDER_AFTER_MATURITY = SwaptionSabrRateVolatilityDataSet.getRatesProviderUsd(MATURITY_DATE.toLocalDate().plusDays(1));
	  private static readonly SabrParametersSwaptionVolatilities VOLS = SwaptionSabrRateVolatilityDataSet.getVolatilitiesUsd(VAL_DATE, true);
	  private static readonly SabrParametersSwaptionVolatilities VOLS_AT_MATURITY = SwaptionSabrRateVolatilityDataSet.getVolatilitiesUsd(MATURITY_DATE.toLocalDate(), true);
	  private static readonly SabrParametersSwaptionVolatilities VOLS_AFTER_MATURITY = SwaptionSabrRateVolatilityDataSet.getVolatilitiesUsd(MATURITY_DATE.toLocalDate().plusDays(1), true);
	  private static readonly SabrParametersSwaptionVolatilities VOLS_REGRESSION = SwaptionSabrRateVolatilityDataSet.getVolatilitiesUsd(VAL_DATE, false);
	  // test parameters and calculator
	  private const double TOL = 1.0e-13;
	  private const double TOLERANCE_DELTA = 1.0E-2;
	  private const double FD_EPS = 1.0e-6;
	  private const double REGRESSION_TOL = 1.0e-4; // due to tenor computation difference
	  private static readonly SabrSwaptionPhysicalProductPricer SWAPTION_PRICER = SabrSwaptionPhysicalProductPricer.DEFAULT;
	  private static readonly DiscountingSwapProductPricer SWAP_PRICER = DiscountingSwapProductPricer.DEFAULT;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FD_CAL = new RatesFiniteDifferenceSensitivityCalculator(FD_EPS);

	  //-------------------------------------------------------------------------
	  public virtual void validate_physical_settlement()
	  {
		assertThrowsIllegalArg(() => SWAPTION_PRICER.presentValue(SWAPTION_CASH, RATE_PROVIDER, VOLS));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		CurrencyAmount computedRec = SWAPTION_PRICER.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount computedPay = SWAPTION_PRICER.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		double forward = SWAP_PRICER.parRate(RSWAP_REC, RATE_PROVIDER);
		double pvbp = SWAP_PRICER.LegPricer.pvbp(RSWAP_REC.getLegs(SwapLegType.FIXED).get(0), RATE_PROVIDER);
		double volatility = VOLS.volatility(SWAPTION_REC_LONG.Expiry, TENOR_YEAR, RATE, forward);
		double maturity = VOLS.relativeTime(SWAPTION_REC_LONG.Expiry);
		double expectedRec = pvbp * BlackFormulaRepository.price(forward + SwaptionSabrRateVolatilityDataSet.SHIFT, RATE + SwaptionSabrRateVolatilityDataSet.SHIFT, maturity, volatility, false);
		double expectedPay = -pvbp * BlackFormulaRepository.price(forward + SwaptionSabrRateVolatilityDataSet.SHIFT, RATE + SwaptionSabrRateVolatilityDataSet.SHIFT, maturity, volatility, true);
		assertEquals(computedRec.Currency, USD);
		assertEquals(computedRec.Amount, expectedRec, NOTIONAL * TOL);
		assertEquals(computedPay.Currency, USD);
		assertEquals(computedPay.Amount, expectedPay, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_atMaturity()
	  {
		CurrencyAmount computedRec = SWAPTION_PRICER.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		CurrencyAmount computedPay = SWAPTION_PRICER.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		double swapPV = SWAP_PRICER.presentValue(RSWAP_REC, RATE_PROVIDER_AT_MATURITY).getAmount(USD).Amount;
		assertEquals(computedRec.Amount, swapPV, NOTIONAL * TOL);
		assertEquals(computedPay.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_afterExpiry()
	  {
		CurrencyAmount computedRec = SWAPTION_PRICER.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		CurrencyAmount computedPay = SWAPTION_PRICER.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		assertEquals(computedRec.Amount, 0d, NOTIONAL * TOL);
		assertEquals(computedPay.Amount, 0d, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_parity()
	  {
		CurrencyAmount pvRecLong = SWAPTION_PRICER.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount pvRecShort = SWAPTION_PRICER.presentValue(SWAPTION_REC_SHORT, RATE_PROVIDER, VOLS);
		CurrencyAmount pvPayLong = SWAPTION_PRICER.presentValue(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount pvPayShort = SWAPTION_PRICER.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		assertEquals(pvRecLong.Amount, -pvRecShort.Amount, NOTIONAL * TOL);
		assertEquals(pvPayLong.Amount, -pvPayShort.Amount, NOTIONAL * TOL);
		double swapPV = SWAP_PRICER.presentValue(RSWAP_PAY, RATE_PROVIDER).getAmount(USD).Amount;
		assertEquals(pvPayLong.Amount - pvRecLong.Amount, swapPV, NOTIONAL * TOL);
		assertEquals(pvPayShort.Amount - pvRecShort.Amount, -swapPV, NOTIONAL * TOL);
	  }

	  public virtual void test_presentValue_parity_atMaturity()
	  {
		CurrencyAmount pvRecLong = SWAPTION_PRICER.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		CurrencyAmount pvRecShort = SWAPTION_PRICER.presentValue(SWAPTION_REC_SHORT, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		CurrencyAmount pvPayLong = SWAPTION_PRICER.presentValue(SWAPTION_PAY_LONG, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		CurrencyAmount pvPayShort = SWAPTION_PRICER.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		assertEquals(pvRecLong.Amount, -pvRecShort.Amount, NOTIONAL * TOL);
		assertEquals(pvPayLong.Amount, -pvPayShort.Amount, NOTIONAL * TOL);
		double swapPV = SWAP_PRICER.presentValue(RSWAP_PAY, RATE_PROVIDER_AT_MATURITY).getAmount(USD).Amount;
		assertEquals(pvPayLong.Amount - pvRecLong.Amount, swapPV, NOTIONAL * TOL);
		assertEquals(pvPayShort.Amount - pvRecShort.Amount, -swapPV, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currencyExposure()
	  {
		MultiCurrencyAmount computedRec = SWAPTION_PRICER.currencyExposure(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount computedPay = SWAPTION_PRICER.currencyExposure(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		PointSensitivityBuilder pointRec = SWAPTION_PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount expectedRec = RATE_PROVIDER.currencyExposure(pointRec.build()).plus(SWAPTION_PRICER.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS));
		assertEquals(computedRec.size(), 1);
		assertEquals(computedRec.getAmount(USD).Amount, expectedRec.getAmount(USD).Amount, NOTIONAL * TOL);
		PointSensitivityBuilder pointPay = SWAPTION_PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		MultiCurrencyAmount expectedPay = RATE_PROVIDER.currencyExposure(pointPay.build()).plus(SWAPTION_PRICER.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS));
		assertEquals(computedPay.size(), 1);
		assertEquals(computedPay.getAmount(USD).Amount, expectedPay.getAmount(USD).Amount, NOTIONAL * TOL);
	  }

	  public virtual void test_currencyExposure_atMaturity()
	  {
		MultiCurrencyAmount computedRec = SWAPTION_PRICER.currencyExposure(SWAPTION_REC_LONG, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		MultiCurrencyAmount computedPay = SWAPTION_PRICER.currencyExposure(SWAPTION_PAY_SHORT, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		PointSensitivityBuilder pointRec = SWAPTION_PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_REC_LONG, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		MultiCurrencyAmount expectedRec = RATE_PROVIDER.currencyExposure(pointRec.build()).plus(SWAPTION_PRICER.presentValue(SWAPTION_REC_LONG, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY));
		assertEquals(computedRec.size(), 1);
		assertEquals(computedRec.getAmount(USD).Amount, expectedRec.getAmount(USD).Amount, NOTIONAL * TOL);
		PointSensitivityBuilder pointPay = SWAPTION_PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_PAY_SHORT, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		MultiCurrencyAmount expectedPay = RATE_PROVIDER.currencyExposure(pointPay.build()).plus(SWAPTION_PRICER.presentValue(SWAPTION_PAY_SHORT, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY));
		assertEquals(computedPay.size(), 1);
		assertEquals(computedPay.getAmount(USD).Amount, expectedPay.getAmount(USD).Amount, NOTIONAL * TOL);
	  }

	  public virtual void test_currencyExposure_afterMaturity()
	  {
		MultiCurrencyAmount computedRec = SWAPTION_PRICER.currencyExposure(SWAPTION_REC_LONG, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		MultiCurrencyAmount computedPay = SWAPTION_PRICER.currencyExposure(SWAPTION_PAY_SHORT, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		assertEquals(computedRec.size(), 1);
		assertEquals(computedRec.getAmount(USD).Amount, 0d, NOTIONAL * TOL);
		assertEquals(computedPay.size(), 1);
		assertEquals(computedPay.getAmount(USD).Amount, 0d, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_impliedVolatility()
	  {
		double computedRec = SWAPTION_PRICER.impliedVolatility(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		double computedPay = SWAPTION_PRICER.impliedVolatility(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		double forward = SWAP_PRICER.parRate(RSWAP_REC, RATE_PROVIDER);
		double expected = VOLS.volatility(MATURITY_DATE, TENOR_YEAR, RATE, forward);
		assertEquals(computedRec, expected, TOL);
		assertEquals(computedPay, expected, TOL);
	  }

	  public virtual void test_impliedVolatility_atMaturity()
	  {
		double computedRec = SWAPTION_PRICER.impliedVolatility(SWAPTION_REC_LONG, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		double computedPay = SWAPTION_PRICER.impliedVolatility(SWAPTION_PAY_SHORT, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		double forward = SWAP_PRICER.parRate(RSWAP_REC, RATE_PROVIDER_AT_MATURITY);
		double expected = VOLS_AT_MATURITY.volatility(MATURITY_DATE, TENOR_YEAR, RATE, forward);
		assertEquals(computedRec, expected, TOL);
		assertEquals(computedPay, expected, TOL);
	  }

	  public virtual void test_impliedVolatility_afterMaturity()
	  {
		assertThrowsIllegalArg(() => SWAPTION_PRICER.impliedVolatility(SWAPTION_REC_LONG, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY));
		assertThrowsIllegalArg(() => SWAPTION_PRICER.impliedVolatility(SWAPTION_PAY_SHORT, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueDelta_parity()
	  {
		double pvbp = SWAP_PRICER.LegPricer.pvbp(SWAPTION_REC_LONG.Underlying.getLegs(SwapLegType.FIXED).get(0), RATE_PROVIDER);
		CurrencyAmount deltaRec = SWAPTION_PRICER.presentValueDelta(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyAmount deltaPay = SWAPTION_PRICER.presentValueDelta(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		assertEquals(deltaRec.Amount + deltaPay.Amount, -pvbp, TOLERANCE_DELTA);
	  }

	  public virtual void test_presentValueDelta_afterMaturity()
	  {
		CurrencyAmount deltaRec = SWAPTION_PRICER.presentValueDelta(SWAPTION_REC_LONG, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		assertEquals(deltaRec.Amount, 0, TOLERANCE_DELTA);
		CurrencyAmount deltaPay = SWAPTION_PRICER.presentValueDelta(SWAPTION_PAY_SHORT, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		assertEquals(deltaPay.Amount, 0, TOLERANCE_DELTA);
	  }

	  public virtual void test_presentValueDelta_atMaturity()
	  {
		double forward = SWAP_PRICER.parRate(RSWAP_REC, RATE_PROVIDER_AT_MATURITY);
		double pvbp = SWAP_PRICER.LegPricer.pvbp(SWAPTION_REC_LONG.Underlying.getLegs(SwapLegType.FIXED).get(0), RATE_PROVIDER_AT_MATURITY);
		CurrencyAmount deltaRec = SWAPTION_PRICER.presentValueDelta(SWAPTION_REC_LONG, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		assertEquals(deltaRec.Amount, RATE > forward ? -pvbp : 0, TOLERANCE_DELTA);
		CurrencyAmount deltaPay = SWAPTION_PRICER.presentValueDelta(SWAPTION_PAY_SHORT, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		assertEquals(deltaPay.Amount, RATE > forward ? 0 : pvbp, TOLERANCE_DELTA);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivityRatesStickyModel()
	  {
		PointSensitivityBuilder pointRec = SWAPTION_PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		CurrencyParameterSensitivities computedRec = RATE_PROVIDER.parameterSensitivity(pointRec.build());
		CurrencyParameterSensitivities expectedRec = FD_CAL.sensitivity(RATE_PROVIDER, (p) => SWAPTION_PRICER.presentValue(SWAPTION_REC_LONG, (p), VOLS));
		assertTrue(computedRec.equalWithTolerance(expectedRec, NOTIONAL * FD_EPS * 100d));
		PointSensitivityBuilder pointPay = SWAPTION_PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		CurrencyParameterSensitivities computedPay = RATE_PROVIDER.parameterSensitivity(pointPay.build());
		CurrencyParameterSensitivities expectedPay = FD_CAL.sensitivity(RATE_PROVIDER, (p) => SWAPTION_PRICER.presentValue(SWAPTION_PAY_SHORT, (p), VOLS));
		assertTrue(computedPay.equalWithTolerance(expectedPay, NOTIONAL * FD_EPS * 100d));
	  }

	  public virtual void test_presentValueSensitivityRatesStickyModel_stickyStrike()
	  {
		SwaptionVolatilities volSabr = SwaptionSabrRateVolatilityDataSet.getVolatilitiesUsd(VAL_DATE, false);
		double impliedVol = SWAPTION_PRICER.impliedVolatility(SWAPTION_REC_LONG, RATE_PROVIDER, volSabr);
		SurfaceMetadata blackMeta = Surfaces.blackVolatilityByExpiryTenor("CST", VOLS.DayCount);
		SwaptionVolatilities volCst = BlackSwaptionExpiryTenorVolatilities.of(VOLS.Convention, VOLS.ValuationDateTime, ConstantSurface.of(blackMeta, impliedVol));
		// To obtain a constant volatility surface which create a sticky strike sensitivity
		PointSensitivityBuilder pointRec = SWAPTION_PRICER.presentValueSensitivityRatesStickyStrike(SWAPTION_REC_LONG, RATE_PROVIDER, volSabr);
		CurrencyParameterSensitivities computedRec = RATE_PROVIDER.parameterSensitivity(pointRec.build());
		CurrencyParameterSensitivities expectedRec = FD_CAL.sensitivity(RATE_PROVIDER, (p) => SWAPTION_PRICER.presentValue(SWAPTION_REC_LONG, (p), volCst));
		assertTrue(computedRec.equalWithTolerance(expectedRec, NOTIONAL * FD_EPS * 100d));

		PointSensitivityBuilder pointPay = SWAPTION_PRICER.presentValueSensitivityRatesStickyStrike(SWAPTION_PAY_SHORT, RATE_PROVIDER, volSabr);
		CurrencyParameterSensitivities computedPay = RATE_PROVIDER.parameterSensitivity(pointPay.build());
		CurrencyParameterSensitivities expectedPay = FD_CAL.sensitivity(RATE_PROVIDER, (p) => SWAPTION_PRICER.presentValue(SWAPTION_PAY_SHORT, (p), volCst));
		assertTrue(computedPay.equalWithTolerance(expectedPay, NOTIONAL * FD_EPS * 100d));
	  }

	  public virtual void test_presentValueSensitivityRatesStickyModel_atMaturity()
	  {
		PointSensitivityBuilder pointRec = SWAPTION_PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_REC_LONG, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		CurrencyParameterSensitivities computedRec = RATE_PROVIDER_AT_MATURITY.parameterSensitivity(pointRec.build());
		CurrencyParameterSensitivities expectedRec = FD_CAL.sensitivity(RATE_PROVIDER_AT_MATURITY, (p) => SWAPTION_PRICER.presentValue(SWAPTION_REC_LONG, (p), VOLS_AT_MATURITY));
		assertTrue(computedRec.equalWithTolerance(expectedRec, NOTIONAL * FD_EPS * 100d));
		PointSensitivities pointPay = SWAPTION_PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_PAY_SHORT, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY).build();
		foreach (PointSensitivity sensi in pointPay.Sensitivities)
		{
		  assertEquals(Math.Abs(sensi.Sensitivity), 0d);
		}
	  }

	  public virtual void test_presentValueSensitivityRatesStickyModel_afterMaturity()
	  {
		PointSensitivities pointRec = SWAPTION_PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_REC_LONG, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY).build();
		foreach (PointSensitivity sensi in pointRec.Sensitivities)
		{
		  assertEquals(Math.Abs(sensi.Sensitivity), 0d);
		}
		PointSensitivities pointPay = SWAPTION_PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_PAY_SHORT, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY).build();
		foreach (PointSensitivity sensi in pointPay.Sensitivities)
		{
		  assertEquals(Math.Abs(sensi.Sensitivity), 0d);
		}
	  }

	  public virtual void test_presentValueSensitivity_parity()
	  {
		CurrencyParameterSensitivities pvSensiRecLong = RATE_PROVIDER.parameterSensitivity(SWAPTION_PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS).build());
		CurrencyParameterSensitivities pvSensiRecShort = RATE_PROVIDER.parameterSensitivity(SWAPTION_PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_REC_SHORT, RATE_PROVIDER, VOLS).build());
		CurrencyParameterSensitivities pvSensiPayLong = RATE_PROVIDER.parameterSensitivity(SWAPTION_PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS).build());
		CurrencyParameterSensitivities pvSensiPayShort = RATE_PROVIDER.parameterSensitivity(SWAPTION_PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS).build());
		assertTrue(pvSensiRecLong.equalWithTolerance(pvSensiRecShort.multipliedBy(-1d), NOTIONAL * TOL));
		assertTrue(pvSensiPayLong.equalWithTolerance(pvSensiPayShort.multipliedBy(-1d), NOTIONAL * TOL));

		CurrencyParameterSensitivities pvSensiSwap = RATE_PROVIDER.parameterSensitivity(SWAP_PRICER.presentValueSensitivity(RSWAP_PAY, RATE_PROVIDER).build());
		assertTrue(pvSensiSwap.equalWithTolerance(pvSensiPayLong.combinedWith(pvSensiRecLong.multipliedBy(-1d)), NOTIONAL * TOL));
		assertTrue(pvSensiSwap.equalWithTolerance(pvSensiRecShort.combinedWith(pvSensiPayShort.multipliedBy(-1d)), NOTIONAL * TOL));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueVega_parity()
	  {
		SwaptionSensitivity vegaRec = SWAPTION_PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		SwaptionSensitivity vegaPay = SWAPTION_PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS);
		assertEquals(vegaRec.Sensitivity, -vegaPay.Sensitivity, TOLERANCE_DELTA);
	  }

	  public virtual void test_presentValueVega_atMaturity()
	  {
		SwaptionSensitivity vegaRec = SWAPTION_PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_REC_LONG, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		assertEquals(vegaRec.Sensitivity, 0, TOLERANCE_DELTA);
		SwaptionSensitivity vegaPay = SWAPTION_PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_PAY_SHORT, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY);
		assertEquals(vegaPay.Sensitivity, 0, TOLERANCE_DELTA);
	  }

	  public virtual void test_presentValueVega_afterMaturity()
	  {
		SwaptionSensitivity vegaRec = SWAPTION_PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_REC_LONG, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		assertEquals(vegaRec.Sensitivity, 0, TOLERANCE_DELTA);
		SwaptionSensitivity vegaPay = SWAPTION_PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_PAY_SHORT, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY);
		assertEquals(vegaPay.Sensitivity, 0, TOLERANCE_DELTA);
	  }

	  public virtual void test_presentValueVega_SwaptionSensitivity()
	  {
		SwaptionSensitivity vegaRec = SWAPTION_PRICER.presentValueSensitivityModelParamsVolatility(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS);
		assertEquals(VOLS.parameterSensitivity(vegaRec), CurrencyParameterSensitivities.empty());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivityModelParamsSabr()
	  {
		PointSensitivities sensiRec = SWAPTION_PRICER.presentValueSensitivityModelParamsSabr(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS).build();
		PointSensitivities sensiPay = SWAPTION_PRICER.presentValueSensitivityModelParamsSabr(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS).build();
		double forward = SWAP_PRICER.parRate(RSWAP_REC, RATE_PROVIDER);
		double pvbp = SWAP_PRICER.LegPricer.pvbp(RSWAP_REC.getLegs(SwapLegType.FIXED).get(0), RATE_PROVIDER);
		double volatility = VOLS.volatility(SWAPTION_REC_LONG.Expiry, TENOR_YEAR, RATE, forward);
		double maturity = VOLS.relativeTime(SWAPTION_REC_LONG.Expiry);
		double[] volSensi = VOLS.Parameters.volatilityAdjoint(maturity, TENOR_YEAR, RATE, forward).Derivatives.toArray();
		double vegaRec = pvbp * BlackFormulaRepository.vega(forward + SwaptionSabrRateVolatilityDataSet.SHIFT, RATE + SwaptionSabrRateVolatilityDataSet.SHIFT, maturity, volatility);
		double vegaPay = -pvbp * BlackFormulaRepository.vega(forward + SwaptionSabrRateVolatilityDataSet.SHIFT, RATE + SwaptionSabrRateVolatilityDataSet.SHIFT, maturity, volatility);
		assertSensitivity(sensiRec, SabrParameterType.ALPHA, vegaRec * volSensi[2], TOL);
		assertSensitivity(sensiRec, SabrParameterType.BETA, vegaRec * volSensi[3], TOL);
		assertSensitivity(sensiRec, SabrParameterType.RHO, vegaRec * volSensi[4], TOL);
		assertSensitivity(sensiRec, SabrParameterType.NU, vegaRec * volSensi[5], TOL);
		assertSensitivity(sensiPay, SabrParameterType.ALPHA, vegaPay * volSensi[2], TOL);
		assertSensitivity(sensiPay, SabrParameterType.BETA, vegaPay * volSensi[3], TOL);
		assertSensitivity(sensiPay, SabrParameterType.RHO, vegaPay * volSensi[4], TOL);
		assertSensitivity(sensiPay, SabrParameterType.NU, vegaPay * volSensi[5], TOL);
	  }

	  private void assertSensitivity(PointSensitivities points, SabrParameterType type, double expected, double tol)
	  {
		foreach (PointSensitivity point in points.Sensitivities)
		{
		  SwaptionSabrSensitivity sens = (SwaptionSabrSensitivity) point;
		  assertEquals(sens.Currency, USD);
		  assertEquals(sens.VolatilitiesName, VOLS.Name);
		  assertEquals(sens.Tenor, (double) TENOR_YEAR);
		  if (sens.SensitivityType == type)
		  {
			assertEquals(sens.Sensitivity, expected, NOTIONAL * tol);
			return;
		  }
		}
		fail("Did not find sensitivity: " + type + " in " + points);
	  }

	  public virtual void test_presentValueSensitivityModelParamsSabr_atMaturity()
	  {
		PointSensitivities sensiRec = SWAPTION_PRICER.presentValueSensitivityModelParamsSabr(SWAPTION_REC_LONG, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY).build();
		assertSensitivity(sensiRec, SabrParameterType.ALPHA, 0, TOL);
		assertSensitivity(sensiRec, SabrParameterType.BETA, 0, TOL);
		assertSensitivity(sensiRec, SabrParameterType.RHO, 0, TOL);
		assertSensitivity(sensiRec, SabrParameterType.NU, 0, TOL);
		PointSensitivities sensiPay = SWAPTION_PRICER.presentValueSensitivityModelParamsSabr(SWAPTION_PAY_SHORT, RATE_PROVIDER_AT_MATURITY, VOLS_AT_MATURITY).build();
		assertSensitivity(sensiPay, SabrParameterType.ALPHA, 0, TOL);
		assertSensitivity(sensiPay, SabrParameterType.BETA, 0, TOL);
		assertSensitivity(sensiPay, SabrParameterType.RHO, 0, TOL);
		assertSensitivity(sensiPay, SabrParameterType.NU, 0, TOL);
	  }

	  public virtual void test_presentValueSensitivityModelParamsSabr_afterMaturity()
	  {
		PointSensitivities sensiRec = SWAPTION_PRICER.presentValueSensitivityModelParamsSabr(SWAPTION_REC_LONG, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY).build();
		assertEquals(sensiRec.Sensitivities.size(), 0);
		PointSensitivities sensiPay = SWAPTION_PRICER.presentValueSensitivityModelParamsSabr(SWAPTION_PAY_SHORT, RATE_PROVIDER_AFTER_MATURITY, VOLS_AFTER_MATURITY).build();
		assertEquals(sensiPay.Sensitivities.size(), 0);
	  }

	  public virtual void test_presentValueSensitivityModelParamsSabr_parity()
	  {
		PointSensitivities pvSensiRecLong = SWAPTION_PRICER.presentValueSensitivityModelParamsSabr(SWAPTION_REC_LONG, RATE_PROVIDER, VOLS).build();
		PointSensitivities pvSensiRecShort = SWAPTION_PRICER.presentValueSensitivityModelParamsSabr(SWAPTION_REC_SHORT, RATE_PROVIDER, VOLS).build();
		PointSensitivities pvSensiPayLong = SWAPTION_PRICER.presentValueSensitivityModelParamsSabr(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS).build();
		PointSensitivities pvSensiPayShort = SWAPTION_PRICER.presentValueSensitivityModelParamsSabr(SWAPTION_PAY_SHORT, RATE_PROVIDER, VOLS).build();

		assertSensitivity(pvSensiRecLong, pvSensiRecShort, SabrParameterType.ALPHA, -1);
		assertSensitivity(pvSensiPayLong, pvSensiPayShort, SabrParameterType.ALPHA, -1);
		assertSensitivity(pvSensiRecLong, pvSensiPayLong, SabrParameterType.ALPHA, 1);
		assertSensitivity(pvSensiPayShort, pvSensiPayShort, SabrParameterType.ALPHA, 1);

		assertSensitivity(pvSensiRecLong, pvSensiRecShort, SabrParameterType.BETA, -1);
		assertSensitivity(pvSensiPayLong, pvSensiPayShort, SabrParameterType.BETA, -1);
		assertSensitivity(pvSensiRecLong, pvSensiPayLong, SabrParameterType.BETA, 1);
		assertSensitivity(pvSensiPayShort, pvSensiPayShort, SabrParameterType.BETA, 1);

		assertSensitivity(pvSensiRecLong, pvSensiRecShort, SabrParameterType.RHO, -1);
		assertSensitivity(pvSensiPayLong, pvSensiPayShort, SabrParameterType.RHO, -1);
		assertSensitivity(pvSensiRecLong, pvSensiPayLong, SabrParameterType.RHO, 1);
		assertSensitivity(pvSensiPayShort, pvSensiPayShort, SabrParameterType.RHO, 1);

		assertSensitivity(pvSensiRecLong, pvSensiRecShort, SabrParameterType.NU, -1);
		assertSensitivity(pvSensiPayLong, pvSensiPayShort, SabrParameterType.NU, -1);
		assertSensitivity(pvSensiRecLong, pvSensiPayLong, SabrParameterType.NU, 1);
		assertSensitivity(pvSensiPayShort, pvSensiPayShort, SabrParameterType.NU, 1);
	  }

	  private void assertSensitivity(PointSensitivities points1, PointSensitivities points2, SabrParameterType type, int factor)
	  {

		// use ordinal() as a hack to find correct type
		assertEquals(points1.Sensitivities.get(type.ordinal()).Sensitivity, points2.Sensitivities.get(type.ordinal()).Sensitivity * factor, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void regressionPv()
	  {
		CurrencyAmount pvComputed = SWAPTION_PRICER.presentValue(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS_REGRESSION);
		assertEquals(pvComputed.Amount, 3156216.489577751, REGRESSION_TOL * NOTIONAL);
	  }

	  public virtual void regressionPvCurveSensi()
	  {
		PointSensitivityBuilder point = SWAPTION_PRICER.presentValueSensitivityRatesStickyModel(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS_REGRESSION);
		CurrencyParameterSensitivities sensiComputed = RATE_PROVIDER.parameterSensitivity(point.build());
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] deltaDsc = {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 109037.92080563342, 637123.4570377409, -931862.187003511, -2556192.7520530378, -4233440.216336116, -5686205.439275854, -6160338.898970505, -3709275.494841247, 0.0};
		double[] deltaDsc = new double[] {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 109037.92080563342, 637123.4570377409, -931862.187003511, -2556192.7520530378, -4233440.216336116, -5686205.439275854, -6160338.898970505, -3709275.494841247, 0.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] deltaFwd = {0.0, 0.0, 0.0, 0.0, -1.0223186788452002E8, 2506923.9169937484, 4980364.73045286, 1.254633556119663E7, 1.528160539036628E8, 2.5824191204559547E8, 0.0, 0.0, 0.0, 0.0, 0.0};
		double[] deltaFwd = new double[] {0.0, 0.0, 0.0, 0.0, -1.0223186788452002E8, 2506923.9169937484, 4980364.73045286, 1.254633556119663E7, 1.528160539036628E8, 2.5824191204559547E8, 0.0, 0.0, 0.0, 0.0, 0.0};
		CurrencyParameterSensitivities sensiExpected = CurrencyParameterSensitivities.of(SwaptionSabrRateVolatilityDataSet.CURVE_DSC_USD.createParameterSensitivity(USD, DoubleArray.copyOf(deltaDsc)), SwaptionSabrRateVolatilityDataSet.CURVE_FWD_USD.createParameterSensitivity(USD, DoubleArray.copyOf(deltaFwd)));
		assertTrue(sensiComputed.equalWithTolerance(sensiExpected, NOTIONAL * REGRESSION_TOL));
	  }

	  public virtual void regressionPvSurfaceSensi()
	  {
		PointSensitivities pointComputed = SWAPTION_PRICER.presentValueSensitivityModelParamsSabr(SWAPTION_PAY_LONG, RATE_PROVIDER, VOLS_REGRESSION).build();
		assertSensitivity(pointComputed, SabrParameterType.ALPHA, 6.5786313367554754E7, REGRESSION_TOL);
		assertSensitivity(pointComputed, SabrParameterType.BETA, -1.2044275797229866E7, REGRESSION_TOL);
		assertSensitivity(pointComputed, SabrParameterType.RHO, 266223.51118849067, REGRESSION_TOL);
		assertSensitivity(pointComputed, SabrParameterType.NU, 400285.5505271345, REGRESSION_TOL);
		CurrencyParameterSensitivities sensiComputed = VOLS_REGRESSION.parameterSensitivity(pointComputed);
		double[][] alphaExp = new double[][]
		{
			new double[] {0.0, 1.0, 0.0},
			new double[] {0.5, 1.0, 0.0},
			new double[] {1.0, 1.0, 0.0},
			new double[] {2.0, 1.0, 0.0},
			new double[] {5.0, 1.0, 0.0},
			new double[] {10.0, 1.0, 0.0},
			new double[] {0.0, 5.0, 0.0},
			new double[] {0.5, 5.0, 0.0},
			new double[] {1.0, 5.0, 6204.475194599179},
			new double[] {2.0, 5.0, 3.94631212984123E7},
			new double[] {5.0, 5.0, 0.0},
			new double[] {10.0, 5.0, 0.0},
			new double[] {0.0, 10.0, 0.0},
			new double[] {0.5, 10.0, 0.0},
			new double[] {1.0, 10.0, 4136.961894403858},
			new double[] {2.0, 10.0, 2.631285063205345E7},
			new double[] {5.0, 10.0, 0.0},
			new double[] {10.0, 10.0, 0.0}
		};
		double[][] betaExp = new double[][]
		{
			new double[] {0.0, 1.0, -0.0},
			new double[] {0.5, 1.0, -0.0},
			new double[] {1.0, 1.0, -0.0},
			new double[] {2.0, 1.0, -0.0},
			new double[] {5.0, 1.0, -0.0},
			new double[] {10.0, 1.0, -0.0},
			new double[] {0.0, 5.0, -0.0},
			new double[] {0.5, 5.0, -0.0},
			new double[] {1.0, 5.0, -1135.926404680998},
			new double[] {2.0, 5.0, -7224978.759366533},
			new double[] {5.0, 5.0, -0.0},
			new double[] {10.0, 5.0, -0.0},
			new double[] {0.0, 10.0, -0.0},
			new double[] {0.5, 10.0, -0.0},
			new double[] {1.0, 10.0, -757.402375482629},
			new double[] {2.0, 10.0, -4817403.70908317},
			new double[] {5.0, 10.0, -0.0},
			new double[] {10.0, 10.0, -0.0}
		};
		double[][] rhoExp = new double[][]
		{
			new double[] {0.0, 1.0, 0.0},
			new double[] {0.5, 1.0, 0.0},
			new double[] {1.0, 1.0, 0.0},
			new double[] {2.0, 1.0, 0.0},
			new double[] {5.0, 1.0, 0.0},
			new double[] {10.0, 1.0, 0.0},
			new double[] {0.0, 5.0, 0.0},
			new double[] {0.5, 5.0, 0.0},
			new double[] {1.0, 5.0, 25.10821912392996},
			new double[] {2.0, 5.0, 159699.03429338703},
			new double[] {5.0, 5.0, 0.0},
			new double[] {10.0, 5.0, 0.0},
			new double[] {0.0, 10.0, 0.0},
			new double[] {0.5, 10.0, 0.0},
			new double[] {1.0, 10.0, 16.741423326578513},
			new double[] {2.0, 10.0, 106482.62725265314},
			new double[] {5.0, 10.0, 0.0},
			new double[] {10.0, 10.0, 0.0}
		};
		double[][] nuExp = new double[][]
		{
			new double[] {0.0, 1.0, 0.0},
			new double[] {0.5, 1.0, 0.0},
			new double[] {1.0, 1.0, 0.0},
			new double[] {2.0, 1.0, 0.0},
			new double[] {5.0, 1.0, 0.0},
			new double[] {10.0, 1.0, 0.0},
			new double[] {0.0, 5.0, 0.0},
			new double[] {0.5, 5.0, 0.0},
			new double[] {1.0, 5.0, 37.751952372314484},
			new double[] {2.0, 5.0, 240118.59649585965},
			new double[] {5.0, 5.0, 0.0},
			new double[] {10.0, 5.0, 0.0},
			new double[] {0.0, 10.0, 0.0},
			new double[] {0.5, 10.0, 0.0},
			new double[] {1.0, 10.0, 25.171893432592533},
			new double[] {2.0, 10.0, 160104.03018547},
			new double[] {5.0, 10.0, 0.0},
			new double[] {10.0, 10.0, 0.0}
		};
		double[][][] exps = new double[][][] {alphaExp, betaExp, rhoExp, nuExp};
		SurfaceMetadata[] metadata = new SurfaceMetadata[] {SwaptionSabrRateVolatilityDataSet.META_ALPHA, SwaptionSabrRateVolatilityDataSet.META_BETA_USD, SwaptionSabrRateVolatilityDataSet.META_RHO, SwaptionSabrRateVolatilityDataSet.META_NU};
		// x-y-value order does not match sorted order in surface, thus sort it
		CurrencyParameterSensitivities sensiExpected = CurrencyParameterSensitivities.empty();
		for (int i = 0; i < exps.Length; ++i)
		{
		  int size = exps[i].Length;
		  IDictionary<DoublesPair, double> sensiMap = new SortedDictionary<DoublesPair, double>();
		  for (int j = 0; j < size; ++j)
		  {
			sensiMap[DoublesPair.of(exps[i][j][0], exps[i][j][1])] = exps[i][j][2];
		  }
		  IList<ParameterMetadata> paramMetadata = new List<ParameterMetadata>(size);
		  IList<double> sensi = new List<double>();
		  foreach (KeyValuePair<DoublesPair, double> entry in sensiMap.SetOfKeyValuePairs())
		  {
			paramMetadata.Add(SwaptionSurfaceExpiryTenorParameterMetadata.of(entry.Key.First, entry.Key.Second));
			sensi.Add(entry.Value);
		  }
		  SurfaceMetadata surfaceMetadata = metadata[i].withParameterMetadata(paramMetadata);
		  sensiExpected = sensiExpected.combinedWith(CurrencyParameterSensitivity.of(surfaceMetadata.SurfaceName, surfaceMetadata.ParameterMetadata.get(), USD, DoubleArray.copyOf(sensi)));
		}
		testSurfaceParameterSensitivities(sensiComputed, sensiExpected, REGRESSION_TOL * NOTIONAL);
	  }

	  //-------------------------------------------------------------------------
	  private void testSurfaceParameterSensitivities(CurrencyParameterSensitivities computed, CurrencyParameterSensitivities expected, double tol)
	  {
		IList<CurrencyParameterSensitivity> listComputed = new List<CurrencyParameterSensitivity>(computed.Sensitivities);
		IList<CurrencyParameterSensitivity> listExpected = new List<CurrencyParameterSensitivity>(expected.Sensitivities);
		foreach (CurrencyParameterSensitivity sensExpected in listExpected)
		{
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		  int index = Math.Abs(Collections.binarySearch(listComputed, sensExpected, CurrencyParameterSensitivity::compareKey));
		  CurrencyParameterSensitivity sensComputed = listComputed[index];
		  int nSens = sensExpected.ParameterCount;
		  assertEquals(sensComputed.ParameterCount, nSens);
		  for (int i = 0; i < nSens; ++i)
		  {
			assertEquals(sensComputed.Sensitivity.get(i), sensExpected.Sensitivity.get(i), tol);
		  }
		  listComputed.RemoveAt(index);
		}
	  }

	}

}