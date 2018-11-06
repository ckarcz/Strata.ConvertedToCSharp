/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fx
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using ResolvedFxSingle = com.opengamma.strata.product.fx.ResolvedFxSingle;

	/// <summary>
	/// Test <seealso cref="DiscountingFxSingleProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingFxSingleProductPricerTest
	public class DiscountingFxSingleProductPricerTest
	{

	  private static readonly RatesProvider PROVIDER = RatesProviderFxDataSets.createProvider();
	  private static readonly Currency KRW = Currency.KRW;
	  private static readonly Currency USD = Currency.USD;
	  private static readonly LocalDate PAYMENT_DATE = RatesProviderFxDataSets.VAL_DATE_2014_01_22.plusWeeks(8);
	  private static readonly LocalDate PAYMENT_DATE_PAST = RatesProviderFxDataSets.VAL_DATE_2014_01_22.minusDays(1);
	  private const double NOMINAL_USD = 100_000_000;
	  private const double FX_RATE = 1123.45;
	  private static readonly ResolvedFxSingle FWD = ResolvedFxSingle.of(CurrencyAmount.of(USD, NOMINAL_USD), FxRate.of(USD, KRW, FX_RATE), PAYMENT_DATE);
	  private static readonly DiscountingFxSingleProductPricer PRICER = DiscountingFxSingleProductPricer.DEFAULT;
	  private const double TOL = 1.0e-12;
	  private const double EPS_FD = 1E-7;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator CAL_FD = new RatesFiniteDifferenceSensitivityCalculator(EPS_FD);

	  public virtual void test_presentValue()
	  {
		MultiCurrencyAmount computed = PRICER.presentValue(FWD, PROVIDER);
		double expected1 = NOMINAL_USD * PROVIDER.discountFactor(USD, PAYMENT_DATE);
		double expected2 = -NOMINAL_USD * FX_RATE * PROVIDER.discountFactor(KRW, PAYMENT_DATE);
		assertEquals(computed.getAmount(USD).Amount, expected1, NOMINAL_USD * TOL);
		assertEquals(computed.getAmount(KRW).Amount, expected2, NOMINAL_USD * TOL);
	  }

	  public virtual void test_presentValue_ended()
	  {
		ResolvedFxSingle fwd = ResolvedFxSingle.of(CurrencyAmount.of(USD, NOMINAL_USD), FxRate.of(USD, KRW, FX_RATE), PAYMENT_DATE_PAST);
		MultiCurrencyAmount computed = PRICER.presentValue(fwd, PROVIDER);
		assertEquals(computed, MultiCurrencyAmount.empty());
	  }

	  public virtual void test_parSpread()
	  {
		double spread = PRICER.parSpread(FWD, PROVIDER);
		ResolvedFxSingle fwdSp = ResolvedFxSingle.of(CurrencyAmount.of(USD, NOMINAL_USD), FxRate.of(USD, KRW, FX_RATE + spread), PAYMENT_DATE);
		MultiCurrencyAmount pv = PRICER.presentValue(fwdSp, PROVIDER);
		assertEquals(pv.convertedTo(USD, PROVIDER).Amount, 0d, NOMINAL_USD * TOL);
	  }

	  public virtual void test_parSpread_ended()
	  {
		ResolvedFxSingle fwd = ResolvedFxSingle.of(CurrencyAmount.of(USD, NOMINAL_USD), FxRate.of(USD, KRW, FX_RATE), PAYMENT_DATE_PAST);
		double spread = PRICER.parSpread(fwd, PROVIDER);
		assertEquals(spread, 0d, TOL);
	  }

	  public virtual void test_forwardFxRate()
	  {
		// forward rate is computed by discounting for any RatesProvider input.
		FxRate computed = PRICER.forwardFxRate(FWD, PROVIDER);
		double df1 = PROVIDER.discountFactor(USD, PAYMENT_DATE);
		double df2 = PROVIDER.discountFactor(KRW, PAYMENT_DATE);
		double spot = PROVIDER.fxRate(USD, KRW);
		FxRate expected = FxRate.of(USD, KRW, spot * df1 / df2);
		assertEquals(computed, expected);
	  }

	  public virtual void test_forwardFxRatePointSensitivity()
	  {
		PointSensitivityBuilder computed = PRICER.forwardFxRatePointSensitivity(FWD, PROVIDER);
		FxForwardSensitivity expected = FxForwardSensitivity.of(CurrencyPair.of(USD, KRW), USD, FWD.PaymentDate, 1d);
		assertEquals(computed, expected);
	  }

	  public virtual void test_forwardFxRateSpotSensitivity()
	  {
		double computed = PRICER.forwardFxRateSpotSensitivity(FWD, PROVIDER);
		double df1 = PROVIDER.discountFactor(USD, PAYMENT_DATE);
		double df2 = PROVIDER.discountFactor(KRW, PAYMENT_DATE);
		assertEquals(computed, df1 / df2);
	  }

	  public virtual void test_presentValueSensitivity()
	  {
		PointSensitivities point = PRICER.presentValueSensitivity(FWD, PROVIDER);
		CurrencyParameterSensitivities computed = PROVIDER.parameterSensitivity(point);
		CurrencyParameterSensitivities expectedUsd = CAL_FD.sensitivity(PROVIDER, (p) => PRICER.presentValue(FWD, (p)).getAmount(USD));
		CurrencyParameterSensitivities expectedKrw = CAL_FD.sensitivity(PROVIDER, (p) => PRICER.presentValue(FWD, (p)).getAmount(KRW));
		assertTrue(computed.equalWithTolerance(expectedUsd.combinedWith(expectedKrw), NOMINAL_USD * FX_RATE * EPS_FD));
	  }

	  public virtual void test_presentValueSensitivity_ended()
	  {
		ResolvedFxSingle fwd = ResolvedFxSingle.of(CurrencyAmount.of(USD, NOMINAL_USD), FxRate.of(USD, KRW, FX_RATE), PAYMENT_DATE_PAST);
		PointSensitivities computed = PRICER.presentValueSensitivity(fwd, PROVIDER);
		assertEquals(computed, PointSensitivities.empty());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currencyExposure()
	  {
		MultiCurrencyAmount computed = PRICER.currencyExposure(FWD, PROVIDER);
		MultiCurrencyAmount expected = PRICER.presentValue(FWD, PROVIDER);
		assertEquals(computed, expected);
	  }

	  public virtual void test_currentCash_zero()
	  {
		MultiCurrencyAmount computed = PRICER.currentCash(FWD, PROVIDER.ValuationDate);
		assertEquals(computed, MultiCurrencyAmount.empty());
	  }

	  public virtual void test_currentCash_onPayment()
	  {
		MultiCurrencyAmount computed = PRICER.currentCash(FWD, PAYMENT_DATE);
		assertEquals(computed, MultiCurrencyAmount.of(FWD.BaseCurrencyPayment.Value, FWD.CounterCurrencyPayment.Value));
	  }
	}

}