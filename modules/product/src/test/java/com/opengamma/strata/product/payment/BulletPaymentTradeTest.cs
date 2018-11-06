/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.payment
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
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
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;

	/// <summary>
	/// Test <seealso cref="BulletPaymentTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BulletPaymentTradeTest
	public class BulletPaymentTradeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly CurrencyAmount GBP_P1000 = CurrencyAmount.of(GBP, 1_000);
	  private static readonly LocalDate DATE_2015_06_30 = date(2015, 6, 30);
	  private static readonly BulletPayment PRODUCT1 = BulletPayment.builder().payReceive(PayReceive.PAY).value(GBP_P1000).date(AdjustableDate.of(DATE_2015_06_30)).build();
	  private static readonly BulletPayment PRODUCT2 = BulletPayment.builder().payReceive(PayReceive.RECEIVE).value(GBP_P1000).date(AdjustableDate.of(DATE_2015_06_30)).build();
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2014, 6, 30));

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		BulletPaymentTrade test = BulletPaymentTrade.of(TRADE_INFO, PRODUCT1);
		assertEquals(test.Product, PRODUCT1);
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.withInfo(TRADE_INFO).Info, TRADE_INFO);
	  }

	  public virtual void test_builder()
	  {
		BulletPaymentTrade test = BulletPaymentTrade.of(TRADE_INFO, PRODUCT1);
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.Product, PRODUCT1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		BulletPaymentTrade trade = BulletPaymentTrade.builder().info(TRADE_INFO).product(PRODUCT1).build();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(TRADE_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.BULLET_PAYMENT).currencies(Currency.GBP).description("Pay GBP 1k : 30Jun15").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		BulletPaymentTrade test = BulletPaymentTrade.of(TRADE_INFO, PRODUCT1);
		assertEquals(test.resolve(REF_DATA).Info, TRADE_INFO);
		assertEquals(test.resolve(REF_DATA).Product, PRODUCT1.resolve(REF_DATA));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		BulletPaymentTrade test = BulletPaymentTrade.builder().info(TRADE_INFO).product(PRODUCT1).build();
		coverImmutableBean(test);
		BulletPaymentTrade test2 = BulletPaymentTrade.builder().product(PRODUCT2).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		BulletPaymentTrade test = BulletPaymentTrade.builder().info(TRADE_INFO).product(PRODUCT1).build();
		assertSerialization(test);
	  }

	}

}