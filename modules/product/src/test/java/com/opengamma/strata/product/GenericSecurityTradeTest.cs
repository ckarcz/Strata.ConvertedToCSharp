/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
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

	/// <summary>
	/// Test <seealso cref="GenericSecurityTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class GenericSecurityTradeTest
	public class GenericSecurityTradeTest
	{

	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2016, 6, 30));
	  private static readonly GenericSecurity SECURITY = GenericSecurityTest.sut();
	  private static readonly GenericSecurity SECURITY2 = GenericSecurityTest.sut2();
	  private const double QUANTITY = 100;
	  private const double QUANTITY2 = 200;
	  private const double PRICE = 123.50;
	  private const double PRICE2 = 120.50;

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		GenericSecurityTrade test = GenericSecurityTrade.of(TRADE_INFO, SECURITY, QUANTITY, PRICE);
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.Security, SECURITY);
		assertEquals(test.Quantity, QUANTITY);
		assertEquals(test.Price, PRICE);
		assertEquals(test.Product, SECURITY);
		assertEquals(test.Currency, SECURITY.Currency);
		assertEquals(test.SecurityId, SECURITY.SecurityId);
		assertEquals(test.withInfo(TRADE_INFO).Info, TRADE_INFO);
		assertEquals(test.withQuantity(129).Quantity, 129d, 0d);
		assertEquals(test.withPrice(129).Price, 129d, 0d);
	  }

	  public virtual void test_builder()
	  {
		GenericSecurityTrade test = sut();
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.Security, SECURITY);
		assertEquals(test.Quantity, QUANTITY);
		assertEquals(test.Price, PRICE);
		assertEquals(test.Currency, SECURITY.Currency);
		assertEquals(test.SecurityId, SECURITY.SecurityId);
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
	  public virtual void test_summarize()
	  {
		GenericSecurityTrade trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.SECURITY).currencies(SECURITY.Currency).description("1 x 100").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  internal static GenericSecurityTrade sut()
	  {
		return GenericSecurityTrade.builder().info(TRADE_INFO).security(SECURITY).quantity(QUANTITY).price(PRICE).build();
	  }

	  internal static GenericSecurityTrade sut2()
	  {
		return GenericSecurityTrade.builder().info(TradeInfo.empty()).security(SECURITY2).quantity(QUANTITY2).price(PRICE2).build();
	  }

	}

}