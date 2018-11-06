/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.currency
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertSame;


	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="Payment"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PaymentTest
	public class PaymentTest
	{

	  private static readonly CurrencyAmount GBP_P1000 = CurrencyAmount.of(GBP, 1_000);
	  private static readonly CurrencyAmount GBP_M1000 = CurrencyAmount.of(GBP, -1_000);
	  private static readonly CurrencyAmount EUR_P1600 = CurrencyAmount.of(EUR, 1_600);
	  private static readonly LocalDate DATE_2015_06_29 = date(2015, 6, 29);
	  private static readonly LocalDate DATE_2015_06_30 = date(2015, 6, 30);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_3args()
	  {
		Payment test = Payment.of(GBP, 1000, DATE_2015_06_30);
		assertEquals(test.Value, GBP_P1000);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Amount, 1_000, 0d);
		assertEquals(test.Date, DATE_2015_06_30);
	  }

	  public virtual void test_of_2args()
	  {
		Payment test = Payment.of(GBP_P1000, DATE_2015_06_30);
		assertEquals(test.Value, GBP_P1000);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Amount, 1_000, 0d);
		assertEquals(test.Date, DATE_2015_06_30);
	  }

	  public virtual void test_ofPay()
	  {
		Payment test = Payment.ofPay(GBP_P1000, DATE_2015_06_30);
		assertEquals(test.Value, GBP_M1000);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Amount, -1_000, 0d);
		assertEquals(test.Date, DATE_2015_06_30);
	  }

	  public virtual void test_ofReceive()
	  {
		Payment test = Payment.ofReceive(GBP_P1000, DATE_2015_06_30);
		assertEquals(test.Value, GBP_P1000);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Amount, 1_000, 0d);
		assertEquals(test.Date, DATE_2015_06_30);
	  }

	  public virtual void test_builder()
	  {
		Payment test = Payment.builder().value(GBP_P1000).date(DATE_2015_06_30).build();
		assertEquals(test.Value, GBP_P1000);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Amount, 1_000, 0d);
		assertEquals(test.Date, DATE_2015_06_30);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_adjustDate()
	  {
		Payment test = Payment.ofReceive(GBP_P1000, DATE_2015_06_29);
		Payment expected = Payment.of(GBP_P1000, DATE_2015_06_29.plusDays(1));
		assertEquals(test.adjustDate(TemporalAdjusters.ofDateAdjuster(d => d.plusDays(1))), expected);
	  }

	  public virtual void test_adjustDate_noChange()
	  {
		Payment test = Payment.ofReceive(GBP_P1000, DATE_2015_06_29);
		assertSame(test.adjustDate(TemporalAdjusters.ofDateAdjuster(d => d.plusDays(1).minusDays(1))), test);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_negated()
	  {
		Payment test = Payment.ofReceive(GBP_P1000, DATE_2015_06_30);
		assertEquals(test.negated(), Payment.of(GBP_M1000, DATE_2015_06_30));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_convertedTo_rateProvider()
	  {
		Payment test = Payment.ofReceive(GBP_P1000, DATE_2015_06_30);
		FxRateProvider provider = (ccy1, ccy2) => 1.6d;
		assertEquals(test.convertedTo(EUR, provider), Payment.ofReceive(EUR_P1600, DATE_2015_06_30));
		assertEquals(test.convertedTo(GBP, provider), test);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		Payment test = Payment.of(GBP_P1000, DATE_2015_06_30);
		coverImmutableBean(test);
		Payment test2 = Payment.of(EUR_P1600, DATE_2015_06_29);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		Payment test = Payment.of(GBP_P1000, DATE_2015_06_30);
		assertSerialization(test);
	  }

	}

}