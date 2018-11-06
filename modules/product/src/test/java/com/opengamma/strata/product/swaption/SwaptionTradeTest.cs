/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swaption
{
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
	/// Test {@code SwaptionTrade}.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SwaptionTradeTest
	public class SwaptionTradeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly Swaption SWAPTION = SwaptionTest.sut();
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2014, 3, 14));
	  private static readonly AdjustablePayment PREMIUM = AdjustablePayment.of(CurrencyAmount.of(Currency.USD, -3150000d), date(2014, 3, 17));

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		SwaptionTrade test = SwaptionTrade.of(TRADE_INFO, SWAPTION, PREMIUM);
		assertEquals(test.Premium, PREMIUM);
		assertEquals(test.Product, SWAPTION);
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.withInfo(TRADE_INFO).Info, TRADE_INFO);
	  }

	  public virtual void test_builder()
	  {
		SwaptionTrade test = sut();
		assertEquals(test.Premium, PREMIUM);
		assertEquals(test.Product, SWAPTION);
		assertEquals(test.Info, TRADE_INFO);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		SwaptionTrade trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(TRADE_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.SWAPTION).currencies(Currency.USD).description("Long 10Y USD 100mm Rec USD-LIBOR-3M / Pay 1.5% : 14Jun14").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		SwaptionTrade test = SwaptionTrade.of(TRADE_INFO, SWAPTION, PREMIUM);
		assertEquals(test.resolve(REF_DATA).Premium, PREMIUM.resolve(REF_DATA));
		assertEquals(test.resolve(REF_DATA).Product, SWAPTION.resolve(REF_DATA));
		assertEquals(test.resolve(REF_DATA).Info, TRADE_INFO);
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
	  internal static SwaptionTrade sut()
	  {
		return SwaptionTrade.builder().premium(PREMIUM).product(SWAPTION).info(TRADE_INFO).build();
	  }

	  internal static SwaptionTrade sut2()
	  {
		return SwaptionTrade.builder().premium(AdjustablePayment.of(CurrencyAmount.of(Currency.USD, -3050000d), LocalDate.of(2014, 3, 17))).product(SwaptionTest.sut2()).build();
	  }

	}

}