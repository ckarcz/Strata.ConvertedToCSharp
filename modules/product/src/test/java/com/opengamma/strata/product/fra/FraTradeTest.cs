/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fra
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
	using Currency = com.opengamma.strata.basics.currency.Currency;

	/// <summary>
	/// Test <seealso cref="FraTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FraTradeTest
	public class FraTradeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly Fra PRODUCT = FraTest.sut();
	  private static readonly Fra PRODUCT2 = FraTest.sut2();
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2015, 3, 15));

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		FraTrade test = FraTrade.of(TRADE_INFO, PRODUCT);
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.withInfo(TRADE_INFO).Info, TRADE_INFO);
	  }

	  public virtual void test_builder()
	  {
		FraTrade test = FraTrade.builder().product(PRODUCT).build();
		assertEquals(test.Info, TradeInfo.empty());
		assertEquals(test.Product, PRODUCT);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		FraTrade test = FraTrade.of(TRADE_INFO, PRODUCT);
		assertEquals(test.resolve(REF_DATA).Info, TRADE_INFO);
		assertEquals(test.resolve(REF_DATA).Product, PRODUCT.resolve(REF_DATA));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		FraTrade trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(TRADE_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.FRA).currencies(Currency.GBP).description("3x6 GBP 1mm Rec GBP-LIBOR / Pay 2.5% : 15Jun15-15Sep15").build();
		assertEquals(trade.summarize(), expected);
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
	  internal static FraTrade sut()
	  {
		return FraTrade.builder().info(TRADE_INFO).product(PRODUCT).build();
	  }

	  internal static FraTrade sut2()
	  {
		return FraTrade.builder().product(PRODUCT2).build();
	  }

	}

}