/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
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

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NotionalExchangeTest
	public class NotionalExchangeTest
	{

	  private static readonly LocalDate DATE_2014_06_30 = date(2014, 6, 30);
	  private static readonly CurrencyAmount GBP_1000 = CurrencyAmount.of(GBP, 1000d);

	  public virtual void test_of()
	  {
		NotionalExchange test = NotionalExchange.of(GBP_1000, DATE_2014_06_30);
		assertEquals(test.Payment, Payment.of(GBP_1000, DATE_2014_06_30));
		assertEquals(test.PaymentDate, DATE_2014_06_30);
		assertEquals(test.PaymentAmount, GBP_1000);
		assertEquals(test.Currency, GBP);
	  }

	  public virtual void test_of_Payment()
	  {
		NotionalExchange test = NotionalExchange.of(Payment.of(GBP_1000, DATE_2014_06_30));
		assertEquals(test.Payment, Payment.of(GBP_1000, DATE_2014_06_30));
		assertEquals(test.PaymentDate, DATE_2014_06_30);
		assertEquals(test.PaymentAmount, GBP_1000);
		assertEquals(test.Currency, GBP);
	  }

	  public virtual void test_of_null()
	  {
		assertThrowsIllegalArg(() => NotionalExchange.of(GBP_1000, null));
		assertThrowsIllegalArg(() => NotionalExchange.of(null, DATE_2014_06_30));
		assertThrowsIllegalArg(() => NotionalExchange.of(null, null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_adjustPaymentDate()
	  {
		NotionalExchange test = NotionalExchange.of(GBP_1000, DATE_2014_06_30);
		NotionalExchange expected = NotionalExchange.of(GBP_1000, DATE_2014_06_30.plusDays(2));
		assertEquals(test.adjustPaymentDate(TemporalAdjusters.ofDateAdjuster(d => d.plusDays(0))), test);
		assertEquals(test.adjustPaymentDate(TemporalAdjusters.ofDateAdjuster(d => d.plusDays(2))), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		NotionalExchange test = NotionalExchange.of(GBP_1000, DATE_2014_06_30);
		coverImmutableBean(test);
		NotionalExchange test2 = NotionalExchange.of(CurrencyAmount.of(GBP, 200d), date(2014, 1, 15));
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		NotionalExchange test = NotionalExchange.of(GBP_1000, DATE_2014_06_30);
		assertSerialization(test);
	  }

	}

}