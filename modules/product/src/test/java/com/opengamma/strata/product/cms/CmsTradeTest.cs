/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.cms
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
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;

	/// <summary>
	/// Test <seealso cref="CmsTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CmsTradeTest
	public class CmsTradeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate TRADE_DATE = LocalDate.of(2015, 9, 21);
	  private static readonly LocalDate SETTLE_DATE = LocalDate.of(2015, 9, 23);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.builder().tradeDate(TRADE_DATE).settlementDate(SETTLE_DATE).build();
	  private static readonly AdjustablePayment PREMIUM = AdjustablePayment.of(CurrencyAmount.of(EUR, -0.001 * 1.0e6), SETTLE_DATE);

	  private static readonly Cms PRODUCT_CAP = Cms.of(CmsTest.sutCap().CmsLeg);
	  private static readonly Cms PRODUCT_CAP2 = CmsTest.sutCap();
	  private static readonly Cms PRODUCT_FLOOR = CmsTest.sutFloor();

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		CmsTrade test = sut();
		assertEquals(test.Premium.get(), PREMIUM);
		assertEquals(test.Product, PRODUCT_CAP);
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.withInfo(TRADE_INFO).Info, TRADE_INFO);
	  }

	  public virtual void test_builder_noPrem()
	  {
		CmsTrade test = CmsTrade.builder().info(TRADE_INFO).product(PRODUCT_CAP2).build();
		assertFalse(test.Premium.Present);
		assertEquals(test.Product, PRODUCT_CAP2);
		assertEquals(test.Info, TRADE_INFO);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		CmsTrade trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(TRADE_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.CMS).currencies(Currency.EUR).description("2Y EUR 1mm Rec EUR-EURIBOR-1100-10Y Cap 1.25% / Pay Premium : 21Oct15-21Oct17").build();
		assertEquals(trade.summarize(), expected);
	  }

	  public virtual void test_summarize_floor()
	  {
		CmsTrade trade = CmsTrade.builder().info(TRADE_INFO).product(PRODUCT_FLOOR).premium(PREMIUM).build();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(TRADE_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.CMS).currencies(Currency.EUR).description("2Y EUR 1mm Rec EUR-EURIBOR-1100-10Y Floor 1.25% / Pay Premium : 21Oct15-21Oct17").build();
		assertEquals(trade.summarize(), expected);
	  }

	  public virtual void test_summarize_singleLeg()
	  {
		CmsTrade trade = CmsTrade.builder().product(Cms.of(CmsLegTest.sutCap())).build();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(TRADE_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.CMS).currencies(Currency.EUR).description("2Y EUR 1mm Rec EUR-EURIBOR-1100-10Y Cap 1.25% : 21Oct15-21Oct17").build();

		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		ResolvedCmsTrade expected = ResolvedCmsTrade.builder().info(TRADE_INFO).product(PRODUCT_CAP.resolve(REF_DATA)).premium(PREMIUM.resolve(REF_DATA)).build();
		assertEquals(sut().resolve(REF_DATA), expected);
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
	  internal static CmsTrade sut()
	  {
		return CmsTrade.builder().info(TRADE_INFO).product(PRODUCT_CAP).premium(PREMIUM).build();
	  }

	  internal static CmsTrade sut2()
	  {
		return CmsTrade.builder().product(PRODUCT_CAP2).build();
	  }

	}

}