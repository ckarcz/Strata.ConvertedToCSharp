/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;

	/// <summary>
	/// Test <seealso cref="ResolvedBulletPaymentTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedBulletPaymentTradeTest
	public class ResolvedBulletPaymentTradeTest
	{

	  private static readonly CurrencyAmount GBP_P1000 = CurrencyAmount.of(GBP, 1_000);
	  private static readonly CurrencyAmount GBP_M1000 = CurrencyAmount.of(GBP, -1_000);
	  private static readonly LocalDate DATE_2015_06_30 = date(2015, 6, 30);
	  private static readonly ResolvedBulletPayment PRODUCT1 = ResolvedBulletPayment.of(Payment.of(GBP_P1000, DATE_2015_06_30));
	  private static readonly ResolvedBulletPayment PRODUCT2 = ResolvedBulletPayment.of(Payment.of(GBP_M1000, DATE_2015_06_30));
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2014, 6, 30));

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		ResolvedBulletPaymentTrade test = ResolvedBulletPaymentTrade.of(TRADE_INFO, PRODUCT1);
		assertEquals(test.Product, PRODUCT1);
		assertEquals(test.Info, TRADE_INFO);
	  }

	  public virtual void test_builder()
	  {
		ResolvedBulletPaymentTrade test = ResolvedBulletPaymentTrade.builder().product(PRODUCT1).build();
		assertEquals(test.Info, TradeInfo.empty());
		assertEquals(test.Product, PRODUCT1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ResolvedBulletPaymentTrade test = ResolvedBulletPaymentTrade.builder().info(TradeInfo.of(date(2014, 6, 30))).product(PRODUCT1).build();
		coverImmutableBean(test);
		ResolvedBulletPaymentTrade test2 = ResolvedBulletPaymentTrade.builder().product(PRODUCT2).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		ResolvedBulletPaymentTrade test = ResolvedBulletPaymentTrade.builder().info(TradeInfo.of(date(2014, 6, 30))).product(PRODUCT1).build();
		assertSerialization(test);
	  }

	}

}