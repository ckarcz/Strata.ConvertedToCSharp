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

	using ImmutableReferenceData = com.opengamma.strata.basics.ImmutableReferenceData;

	/// <summary>
	/// Test <seealso cref="SecurityTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SecurityTradeTest
	public class SecurityTradeTest
	{

	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2016, 6, 30));
	  private static readonly SecurityId SECURITY_ID = SecurityId.of("OG-Test", "Id");
	  private static readonly SecurityId SECURITY_ID2 = SecurityId.of("OG-Test", "Id2");
	  private const double QUANTITY = 100;
	  private const double QUANTITY2 = 200;
	  private const double PRICE = 123.50;
	  private const double PRICE2 = 120.50;

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		SecurityTrade test = SecurityTrade.of(TRADE_INFO, SECURITY_ID, QUANTITY, PRICE);
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.SecurityId, SECURITY_ID);
		assertEquals(test.Quantity, QUANTITY);
		assertEquals(test.Price, PRICE);
		assertEquals(test.withInfo(TRADE_INFO).Info, TRADE_INFO);
		assertEquals(test.withQuantity(129).Quantity, 129d, 0d);
		assertEquals(test.withPrice(129).Price, 129d, 0d);
	  }

	  public virtual void test_builder()
	  {
		SecurityTrade test = sut();
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.SecurityId, SECURITY_ID);
		assertEquals(test.Quantity, QUANTITY);
		assertEquals(test.Price, PRICE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		SecurityTrade trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.SECURITY).description("Id x 100").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolveTarget()
	  {
		GenericSecurity security = GenericSecurityTest.sut();
		Trade test = sut().resolveTarget(ImmutableReferenceData.of(SECURITY_ID, security));
		GenericSecurityTrade expected = GenericSecurityTrade.of(TRADE_INFO, security, QUANTITY, PRICE);
		assertEquals(test, expected);
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
	  internal static SecurityTrade sut()
	  {
		return SecurityTrade.builder().info(TRADE_INFO).securityId(SECURITY_ID).quantity(QUANTITY).price(PRICE).build();
	  }

	  internal static SecurityTrade sut2()
	  {
		return SecurityTrade.builder().info(TradeInfo.empty()).securityId(SECURITY_ID2).quantity(QUANTITY2).price(PRICE2).build();
	  }

	}

}