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
	/// Test <seealso cref="ResolvedBulletPayment"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedBulletPaymentTest
	public class ResolvedBulletPaymentTest
	{

	  private static readonly CurrencyAmount GBP_P1000 = CurrencyAmount.of(GBP, 1_000);
	  private static readonly CurrencyAmount GBP_M1000 = CurrencyAmount.of(GBP, -1_000);
	  private static readonly LocalDate DATE_2015_06_30 = date(2015, 6, 30);
	  private static readonly Payment PAYMENT1 = Payment.of(GBP_P1000, DATE_2015_06_30);
	  private static readonly Payment PAYMENT2 = Payment.of(GBP_M1000, DATE_2015_06_30);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		ResolvedBulletPayment test = ResolvedBulletPayment.of(PAYMENT1);
		assertEquals(test.Payment, PAYMENT1);
		assertEquals(test.Currency, PAYMENT1.Currency);
	  }

	  public virtual void test_builder()
	  {
		ResolvedBulletPayment test = ResolvedBulletPayment.builder().payment(PAYMENT1).build();
		assertEquals(test.Payment, PAYMENT1);
		assertEquals(test.Currency, PAYMENT1.Currency);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ResolvedBulletPayment test = ResolvedBulletPayment.of(PAYMENT1);
		coverImmutableBean(test);
		ResolvedBulletPayment test2 = ResolvedBulletPayment.of(PAYMENT2);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		ResolvedBulletPayment test = ResolvedBulletPayment.of(PAYMENT1);
		assertSerialization(test);
	  }

	}

}