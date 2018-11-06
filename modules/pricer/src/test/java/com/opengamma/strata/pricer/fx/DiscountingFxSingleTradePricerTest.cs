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
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using ResolvedFxSingle = com.opengamma.strata.product.fx.ResolvedFxSingle;
	using ResolvedFxSingleTrade = com.opengamma.strata.product.fx.ResolvedFxSingleTrade;

	/// <summary>
	/// Test <seealso cref="DiscountingFxSingleProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingFxSingleTradePricerTest
	public class DiscountingFxSingleTradePricerTest
	{

	  private static readonly RatesProvider PROVIDER = RatesProviderFxDataSets.createProvider();
	  private static readonly Currency KRW = Currency.KRW;
	  private static readonly Currency USD = Currency.USD;
	  private static readonly LocalDate PAYMENT_DATE = RatesProviderFxDataSets.VAL_DATE_2014_01_22.plusWeeks(8);
	  private const double NOMINAL_USD = 100_000_000;
	  private const double FX_RATE = 1123.45;

	  private static readonly ResolvedFxSingle PRODUCT = ResolvedFxSingle.of(CurrencyAmount.of(USD, NOMINAL_USD), FxRate.of(USD, KRW, FX_RATE), PAYMENT_DATE);
	  private static readonly ResolvedFxSingleTrade TRADE = ResolvedFxSingleTrade.of(TradeInfo.empty(), PRODUCT);

	  private static readonly DiscountingFxSingleProductPricer PRODUCT_PRICER = DiscountingFxSingleProductPricer.DEFAULT;
	  private static readonly DiscountingFxSingleTradePricer TRADE_PRICER = DiscountingFxSingleTradePricer.DEFAULT;

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

	  public virtual void test_currencyExposure()
	  {
		assertEquals(TRADE_PRICER.currencyExposure(TRADE, PROVIDER), PRODUCT_PRICER.currencyExposure(PRODUCT, PROVIDER));
	  }

	  public virtual void test_currentCash()
	  {
		assertEquals(TRADE_PRICER.currentCash(TRADE, PROVIDER), PRODUCT_PRICER.currentCash(PRODUCT, PROVIDER.ValuationDate));
	  }

	  public virtual void test_forwardFxRate()
	  {
		assertEquals(TRADE_PRICER.forwardFxRate(TRADE, PROVIDER), PRODUCT_PRICER.forwardFxRate(PRODUCT, PROVIDER));
	  }

	  public virtual void test_forwardFxRatePointSensitivity()
	  {
		assertEquals(TRADE_PRICER.forwardFxRatePointSensitivity(TRADE, PROVIDER), PRODUCT_PRICER.forwardFxRatePointSensitivity(PRODUCT, PROVIDER).build());
	  }

	  public virtual void test_forwardFxRateSpotSensitivity()
	  {
		assertEquals(TRADE_PRICER.forwardFxRateSpotSensitivity(TRADE, PROVIDER), PRODUCT_PRICER.forwardFxRateSpotSensitivity(PRODUCT, PROVIDER));
	  }

	}

}