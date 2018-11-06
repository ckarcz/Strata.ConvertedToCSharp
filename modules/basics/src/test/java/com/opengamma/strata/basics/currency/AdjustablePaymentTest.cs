/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
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

	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;

	/// <summary>
	/// Test <seealso cref="AdjustablePayment"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class AdjustablePaymentTest
	public class AdjustablePaymentTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly CurrencyAmount GBP_P1000 = CurrencyAmount.of(GBP, 1_000);
	  private static readonly CurrencyAmount GBP_M1000 = CurrencyAmount.of(GBP, -1_000);
	  private static readonly CurrencyAmount EUR_P1600 = CurrencyAmount.of(EUR, 1_600);
	  private static readonly LocalDate DATE_2015_06_29 = date(2015, 6, 29);
	  private static readonly AdjustableDate DATE_2015_06_28_ADJ = AdjustableDate.of(date(2015, 6, 28), BusinessDayAdjustment.of(FOLLOWING, GBLO));
	  private static readonly LocalDate DATE_2015_06_30 = date(2015, 6, 30);
	  private static readonly AdjustableDate DATE_2015_06_30_FIX = AdjustableDate.of(date(2015, 6, 30));

	  //-------------------------------------------------------------------------
	  public virtual void test_of_3argsFixed()
	  {
		AdjustablePayment test = AdjustablePayment.of(GBP, 1000, DATE_2015_06_30);
		assertEquals(test.Value, GBP_P1000);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Amount, 1_000, 0d);
		assertEquals(test.Date, DATE_2015_06_30_FIX);
	  }

	  public virtual void test_of_3argsAdjustable()
	  {
		AdjustablePayment test = AdjustablePayment.of(GBP, 1000, DATE_2015_06_28_ADJ);
		assertEquals(test.Value, GBP_P1000);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Amount, 1_000, 0d);
		assertEquals(test.Date, DATE_2015_06_28_ADJ);
	  }

	  public virtual void test_of_2argsFixed()
	  {
		AdjustablePayment test = AdjustablePayment.of(GBP_P1000, DATE_2015_06_30);
		assertEquals(test.Value, GBP_P1000);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Amount, 1_000, 0d);
		assertEquals(test.Date, DATE_2015_06_30_FIX);
	  }

	  public virtual void test_of_2argsAdjustable()
	  {
		AdjustablePayment test = AdjustablePayment.of(GBP_P1000, DATE_2015_06_28_ADJ);
		assertEquals(test.Value, GBP_P1000);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Amount, 1_000, 0d);
		assertEquals(test.Date, DATE_2015_06_28_ADJ);
	  }

	  public virtual void test_ofPayFixed()
	  {
		AdjustablePayment test = AdjustablePayment.ofPay(GBP_P1000, DATE_2015_06_30);
		assertEquals(test.Value, GBP_M1000);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Amount, -1_000, 0d);
		assertEquals(test.Date, DATE_2015_06_30_FIX);
	  }

	  public virtual void test_ofPayAdjustable()
	  {
		AdjustablePayment test = AdjustablePayment.ofPay(GBP_P1000, DATE_2015_06_28_ADJ);
		assertEquals(test.Value, GBP_M1000);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Amount, -1_000, 0d);
		assertEquals(test.Date, DATE_2015_06_28_ADJ);
	  }

	  public virtual void test_ofReceiveFixed()
	  {
		AdjustablePayment test = AdjustablePayment.ofReceive(GBP_P1000, DATE_2015_06_30);
		assertEquals(test.Value, GBP_P1000);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Amount, 1_000, 0d);
		assertEquals(test.Date, DATE_2015_06_30_FIX);
	  }

	  public virtual void test_ofReceiveAdjustable()
	  {
		AdjustablePayment test = AdjustablePayment.ofReceive(GBP_P1000, DATE_2015_06_28_ADJ);
		assertEquals(test.Value, GBP_P1000);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Amount, 1_000, 0d);
		assertEquals(test.Date, DATE_2015_06_28_ADJ);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		AdjustablePayment test = AdjustablePayment.ofReceive(GBP_P1000, DATE_2015_06_28_ADJ);
		assertEquals(test.resolve(REF_DATA), Payment.of(GBP_P1000, DATE_2015_06_29));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_negated()
	  {
		AdjustablePayment test = AdjustablePayment.ofReceive(GBP_P1000, DATE_2015_06_30);
		assertEquals(test.negated(), AdjustablePayment.of(GBP_M1000, DATE_2015_06_30));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		AdjustablePayment test = AdjustablePayment.of(GBP_P1000, DATE_2015_06_30);
		coverImmutableBean(test);
		AdjustablePayment test2 = AdjustablePayment.of(EUR_P1600, DATE_2015_06_28_ADJ);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		AdjustablePayment test = AdjustablePayment.of(GBP_P1000, DATE_2015_06_30);
		assertSerialization(test);
	  }

	}

}