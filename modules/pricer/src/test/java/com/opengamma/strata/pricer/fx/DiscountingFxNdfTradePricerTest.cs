/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fx
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using FxIndex = com.opengamma.strata.basics.index.FxIndex;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;
	using ImmutableFxIndex = com.opengamma.strata.basics.index.ImmutableFxIndex;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using ResolvedFxNdf = com.opengamma.strata.product.fx.ResolvedFxNdf;
	using ResolvedFxNdfTrade = com.opengamma.strata.product.fx.ResolvedFxNdfTrade;

	/// <summary>
	/// Test <seealso cref="DiscountingFxNdfProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingFxNdfTradePricerTest
	public class DiscountingFxNdfTradePricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly RatesProvider PROVIDER = RatesProviderFxDataSets.createProvider();
	  private static readonly Currency KRW = Currency.KRW;
	  private static readonly Currency USD = Currency.USD;
	  private static readonly LocalDate PAYMENT_DATE = RatesProviderFxDataSets.VAL_DATE_2014_01_22.plusWeeks(8);
	  private const double NOMINAL_USD = 100_000_000;
	  private static readonly CurrencyAmount CURRENCY_NOTIONAL = CurrencyAmount.of(USD, NOMINAL_USD);
	  private const double FX_RATE = 1123.45;
	  private static readonly FxIndex INDEX = ImmutableFxIndex.builder().name("USD/KRW").currencyPair(CurrencyPair.of(USD, KRW)).fixingCalendar(USNY).maturityDateOffset(DaysAdjustment.ofBusinessDays(2, USNY)).build();
	  private static readonly LocalDate FIXING_DATE = INDEX.calculateFixingFromMaturity(PAYMENT_DATE, REF_DATA);

	  private static readonly ResolvedFxNdf PRODUCT = ResolvedFxNdf.builder().settlementCurrencyNotional(CURRENCY_NOTIONAL).agreedFxRate(FxRate.of(USD, KRW, FX_RATE)).observation(FxIndexObservation.of(INDEX, FIXING_DATE, REF_DATA)).paymentDate(PAYMENT_DATE).build();
	  private static readonly ResolvedFxNdfTrade TRADE = ResolvedFxNdfTrade.of(TradeInfo.empty(), PRODUCT);

	  private static readonly DiscountingFxNdfProductPricer PRODUCT_PRICER = DiscountingFxNdfProductPricer.DEFAULT;
	  private static readonly DiscountingFxNdfTradePricer TRADE_PRICER = DiscountingFxNdfTradePricer.DEFAULT;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		assertEquals(TRADE_PRICER.presentValue(TRADE, PROVIDER), PRODUCT_PRICER.presentValue(PRODUCT, PROVIDER));
	  }

	  public virtual void test_presentValueSensitivity()
	  {
		assertEquals(TRADE_PRICER.presentValueSensitivity(TRADE, PROVIDER), PRODUCT_PRICER.presentValueSensitivity(PRODUCT, PROVIDER));
	  }

	  public virtual void test_currencyExposure()
	  {
		assertEquals(TRADE_PRICER.currencyExposure(TRADE, PROVIDER), PRODUCT_PRICER.currencyExposure(PRODUCT, PROVIDER));
	  }

	  public virtual void test_currentCash()
	  {
		assertEquals(TRADE_PRICER.currentCash(TRADE, PROVIDER), PRODUCT_PRICER.currentCash(PRODUCT, PROVIDER));
	  }

	  public virtual void test_forwardFxRate()
	  {
		assertEquals(TRADE_PRICER.forwardFxRate(TRADE, PROVIDER), PRODUCT_PRICER.forwardFxRate(PRODUCT, PROVIDER));
	  }

	}

}