/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fx
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using ResolvedFxSwap = com.opengamma.strata.product.fx.ResolvedFxSwap;
	using ResolvedFxSwapTrade = com.opengamma.strata.product.fx.ResolvedFxSwapTrade;

	/// <summary>
	/// Test <seealso cref="DiscountingFxSwapProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingFxSwapTradePricerTest
	public class DiscountingFxSwapTradePricerTest
	{

	  private static readonly RatesProvider PROVIDER = RatesProviderFxDataSets.createProvider();
	  private static readonly Currency KRW = Currency.KRW;
	  private static readonly Currency USD = Currency.USD;
	  private static readonly LocalDate PAYMENT_DATE_NEAR = RatesProviderFxDataSets.VAL_DATE_2014_01_22.plusWeeks(1);
	  private static readonly LocalDate PAYMENT_DATE_FAR = PAYMENT_DATE_NEAR.plusMonths(1);
	  private const double NOMINAL_USD = 100_000_000;
	  private const double FX_RATE = 1109.5;
	  private const double FX_FWD_POINTS = 4.45;

	  private static readonly ResolvedFxSwap PRODUCT = ResolvedFxSwap.ofForwardPoints(CurrencyAmount.of(USD, NOMINAL_USD), KRW, FX_RATE, FX_FWD_POINTS, PAYMENT_DATE_NEAR, PAYMENT_DATE_FAR);
	  private static readonly ResolvedFxSwapTrade TRADE = ResolvedFxSwapTrade.of(TradeInfo.empty(), PRODUCT);

	  private static readonly DiscountingFxSwapProductPricer PRODUCT_PRICER = DiscountingFxSwapProductPricer.DEFAULT;
	  private static readonly DiscountingFxSwapTradePricer TRADE_PRICER = DiscountingFxSwapTradePricer.DEFAULT;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		assertEquals(TRADE_PRICER.presentValue(TRADE, PROVIDER), PRODUCT_PRICER.presentValue(PRODUCT, PROVIDER));
	  }

	  public virtual void test_presentValueSensitivity()
	  {
		assertEquals(TRADE_PRICER.presentValueSensitivity(TRADE, PROVIDER), PRODUCT_PRICER.presentValueSensitivity(PRODUCT, PROVIDER));
	  }

	  public virtual void test_parSpread()
	  {
		assertEquals(TRADE_PRICER.parSpread(TRADE, PROVIDER), PRODUCT_PRICER.parSpread(PRODUCT, PROVIDER));
	  }

	  public virtual void test_parSpreadSensitivity()
	  {
		assertEquals(TRADE_PRICER.parSpreadSensitivity(TRADE, PROVIDER), PRODUCT_PRICER.parSpreadSensitivity(PRODUCT, PROVIDER));
	  }

	  public virtual void test_currencyExposure()
	  {
		assertEquals(TRADE_PRICER.currencyExposure(TRADE, PROVIDER), PRODUCT_PRICER.currencyExposure(PRODUCT, PROVIDER));
	  }

	  public virtual void test_currentCash()
	  {
		assertEquals(TRADE_PRICER.currentCash(TRADE, PROVIDER), PRODUCT_PRICER.currentCash(PRODUCT, PROVIDER.ValuationDate));
	  }

	}

}