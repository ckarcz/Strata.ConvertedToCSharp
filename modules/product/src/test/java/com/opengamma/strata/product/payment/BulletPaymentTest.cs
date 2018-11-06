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
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;

	/// <summary>
	/// Test <seealso cref="BulletPayment"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BulletPaymentTest
	public class BulletPaymentTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly CurrencyAmount GBP_P1000 = CurrencyAmount.of(GBP, 1_000);
	  private static readonly CurrencyAmount GBP_M1000 = CurrencyAmount.of(GBP, -1_000);
	  private static readonly CurrencyAmount USD_P1600 = CurrencyAmount.of(USD, 1_600);
	  private static readonly LocalDate DATE_2015_06_29 = date(2015, 6, 29);
	  private static readonly LocalDate DATE_2015_06_30 = date(2015, 6, 30);

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		BulletPayment test = BulletPayment.builder().payReceive(PayReceive.PAY).value(GBP_P1000).date(AdjustableDate.of(DATE_2015_06_30)).build();
		assertEquals(test.PayReceive, PayReceive.PAY);
		assertEquals(test.Value, GBP_P1000);
		assertEquals(test.Date, AdjustableDate.of(DATE_2015_06_30));
		assertEquals(test.Currency, GBP);
		assertEquals(test.CrossCurrency, false);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(GBP));
		assertEquals(test.allCurrencies(), ImmutableSet.of(GBP));
	  }

	  public virtual void test_builder_notNegative()
	  {
		assertThrowsIllegalArg(() => BulletPayment.builder().payReceive(PayReceive.PAY).value(GBP_M1000).date(AdjustableDate.of(DATE_2015_06_30)).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve_pay()
	  {
		BulletPayment test = BulletPayment.builder().payReceive(PayReceive.PAY).value(GBP_P1000).date(AdjustableDate.of(DATE_2015_06_30)).build();
		ResolvedBulletPayment expected = ResolvedBulletPayment.of(Payment.of(GBP_M1000, DATE_2015_06_30));
		assertEquals(test.resolve(REF_DATA), expected);
	  }

	  public virtual void test_resolve_receive()
	  {
		BulletPayment test = BulletPayment.builder().payReceive(PayReceive.RECEIVE).value(GBP_P1000).date(AdjustableDate.of(DATE_2015_06_30)).build();
		ResolvedBulletPayment expected = ResolvedBulletPayment.of(Payment.of(GBP_P1000, DATE_2015_06_30));
		assertEquals(test.resolve(REF_DATA), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		BulletPayment test = BulletPayment.builder().payReceive(PayReceive.PAY).value(GBP_P1000).date(AdjustableDate.of(DATE_2015_06_30)).build();
		coverImmutableBean(test);
		BulletPayment test2 = BulletPayment.builder().payReceive(PayReceive.RECEIVE).value(USD_P1600).date(AdjustableDate.of(DATE_2015_06_29)).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		BulletPayment test = BulletPayment.builder().payReceive(PayReceive.PAY).value(GBP_P1000).date(AdjustableDate.of(DATE_2015_06_30)).build();
		assertSerialization(test);
	  }

	}

}