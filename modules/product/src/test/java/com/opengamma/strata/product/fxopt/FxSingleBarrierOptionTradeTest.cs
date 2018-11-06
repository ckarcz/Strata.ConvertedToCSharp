/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using LongShort = com.opengamma.strata.product.common.LongShort;
	using FxSingle = com.opengamma.strata.product.fx.FxSingle;
	using BarrierType = com.opengamma.strata.product.option.BarrierType;
	using KnockType = com.opengamma.strata.product.option.KnockType;
	using SimpleConstantContinuousBarrier = com.opengamma.strata.product.option.SimpleConstantContinuousBarrier;

	/// <summary>
	/// Test <seealso cref="FxSingleBarrierOptionTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxSingleBarrierOptionTradeTest
	public class FxSingleBarrierOptionTradeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate EXPIRY_DATE = LocalDate.of(2015, 2, 14);
	  private static readonly LocalTime EXPIRY_TIME = LocalTime.of(12, 15);
	  private static readonly ZoneId EXPIRY_ZONE = ZoneId.of("Z");
	  private const LongShort LONG = LongShort.LONG;
	  private static readonly LocalDate PAYMENT_DATE = LocalDate.of(2015, 2, 16);
	  private const double NOTIONAL = 1.0e6;
	  private static readonly CurrencyAmount EUR_AMOUNT = CurrencyAmount.of(EUR, NOTIONAL);
	  private static readonly CurrencyAmount USD_AMOUNT = CurrencyAmount.of(USD, -NOTIONAL * 1.35);
	  private static readonly FxSingle FX = FxSingle.of(EUR_AMOUNT, USD_AMOUNT, PAYMENT_DATE);
	  private static readonly FxVanillaOption VANILLA_OPTION = FxVanillaOption.builder().longShort(LONG).expiryDate(EXPIRY_DATE).expiryTime(EXPIRY_TIME).expiryZone(EXPIRY_ZONE).underlying(FX).build();
	  private static readonly SimpleConstantContinuousBarrier BARRIER = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_IN, 1.2);
	  private static readonly CurrencyAmount REBATE = CurrencyAmount.of(USD, 5.0e4);
	  private static readonly FxSingleBarrierOption PRODUCT = FxSingleBarrierOption.of(VANILLA_OPTION, BARRIER, REBATE);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2014, 11, 12));
	  private static readonly AdjustablePayment PREMIUM = AdjustablePayment.of(CurrencyAmount.of(EUR, NOTIONAL * 0.05), date(2014, 11, 14));

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		FxSingleBarrierOptionTrade test = sut();
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Product.CurrencyPair, PRODUCT.CurrencyPair);
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.Premium, PREMIUM);
		assertEquals(test.withInfo(TRADE_INFO).Info, TRADE_INFO);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		FxSingleBarrierOptionTrade trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.FX_SINGLE_BARRIER_OPTION).currencies(Currency.USD, Currency.EUR).description("Long Barrier Rec EUR 1mm @ EUR/USD 1.35 Premium EUR 50k : 14Feb15").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		FxSingleBarrierOptionTrade @base = sut();
		ResolvedFxSingleBarrierOptionTrade expected = ResolvedFxSingleBarrierOptionTrade.builder().info(TRADE_INFO).product(PRODUCT.resolve(REF_DATA)).premium(PREMIUM.resolve(REF_DATA)).build();
		assertEquals(@base.resolve(REF_DATA), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FxSingleBarrierOptionTrade test1 = sut();
		FxSingleBarrierOptionTrade test2 = FxSingleBarrierOptionTrade.builder().product(FxSingleBarrierOption.of(VANILLA_OPTION, BARRIER)).premium(AdjustablePayment.of(CurrencyAmount.of(EUR, NOTIONAL * 0.01), date(2014, 11, 13))).build();
		coverImmutableBean(test1);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		FxSingleBarrierOptionTrade test = sut();
		assertSerialization(test);
	  }

	  //-------------------------------------------------------------------------
	  internal static FxSingleBarrierOptionTrade sut()
	  {
		return FxSingleBarrierOptionTrade.builder().info(TRADE_INFO).product(PRODUCT).premium(PREMIUM).build();
	  }

	}

}