/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.etd
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableReferenceData = com.opengamma.strata.basics.ImmutableReferenceData;

	/// <summary>
	/// Test <seealso cref="EtdOptionTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class EtdOptionTradeTest
	public class EtdOptionTradeTest
	{

	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(LocalDate.of(2017, 1, 1));
	  private static readonly EtdOptionSecurity SECURITY = EtdOptionSecurityTest.sut();

	  public virtual void test_of()
	  {
		EtdOptionTrade test = EtdOptionTrade.of(TRADE_INFO, SECURITY, 1000, 20);
		assertEquals(test.Security, SECURITY);
		assertEquals(test.Quantity, 1000d, 0d);
		assertEquals(test.Price, 20d, 0d);
		assertEquals(test.SecurityId, SECURITY.SecurityId);
		assertEquals(test.Currency, SECURITY.Currency);
		assertEquals(test.withInfo(TRADE_INFO).Info, TRADE_INFO);
		assertEquals(test.withQuantity(129).Quantity, 129d, 0d);
		assertEquals(test.withPrice(129).Price, 129d, 0d);
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
		EtdOptionTrade trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.ETD_OPTION).currencies(SECURITY.Currency).description(SECURITY.SecurityId.StandardId.Value + " x 3000, Jun17 P2").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolveTarget()
	  {
		GenericSecurity security = GenericSecurity.of(SECURITY.Info);
		Trade test = sut().resolveTarget(ImmutableReferenceData.of(SECURITY.SecurityId, security));
		GenericSecurityTrade expected = GenericSecurityTrade.of(TRADE_INFO, security, 3000, 20);
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  internal static EtdOptionTrade sut()
	  {
		return EtdOptionTrade.builder().info(TRADE_INFO).security(SECURITY).quantity(3000).price(20).build();
	  }

	  internal static EtdOptionTrade sut2()
	  {
		return EtdOptionTrade.builder().security(EtdOptionSecurityTest.sut2()).quantity(4000).price(30).build();
	  }

	}

}