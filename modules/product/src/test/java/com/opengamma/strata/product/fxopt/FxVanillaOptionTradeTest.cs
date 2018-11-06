/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
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

	/// <summary>
	/// Test <seealso cref="FxVanillaOptionTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxVanillaOptionTradeTest
	public class FxVanillaOptionTradeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double NOTIONAL = 1.0e6;
	  private static readonly FxVanillaOption PRODUCT = FxVanillaOptionTest.sut();
	  private static readonly FxVanillaOption PRODUCT2 = FxVanillaOptionTest.sut2();
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2014, 11, 12));
	  private static readonly AdjustablePayment PREMIUM = AdjustablePayment.of(CurrencyAmount.of(EUR, NOTIONAL * 0.05), date(2014, 11, 14));

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		FxVanillaOptionTrade test = sut();
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Product.CurrencyPair, PRODUCT.CurrencyPair);
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.Premium, PREMIUM);
		assertEquals(test.withInfo(TRADE_INFO).Info, TRADE_INFO);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		FxVanillaOptionTrade trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.FX_VANILLA_OPTION).currencies(Currency.USD, Currency.EUR).description("Long Rec EUR 1mm @ EUR/USD 1.35 Premium EUR 50k : 14Feb15").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		FxVanillaOptionTrade test = sut();
		ResolvedFxVanillaOptionTrade expected = ResolvedFxVanillaOptionTrade.builder().info(TRADE_INFO).product(PRODUCT.resolve(REF_DATA)).premium(PREMIUM.resolve(REF_DATA)).build();
		assertEquals(test.resolve(REF_DATA), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(sut());
		coverBeanEquals(sut(), sut2());
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(sut());
	  }

	  //-------------------------------------------------------------------------
	  internal static FxVanillaOptionTrade sut()
	  {
		return FxVanillaOptionTrade.builder().info(TRADE_INFO).product(PRODUCT).premium(PREMIUM).build();
	  }

	  internal static FxVanillaOptionTrade sut2()
	  {
		AdjustablePayment premium = AdjustablePayment.of(CurrencyAmount.of(EUR, NOTIONAL * 0.01), date(2014, 11, 13));
		return FxVanillaOptionTrade.builder().product(PRODUCT2).premium(premium).build();
	  }

	}

}