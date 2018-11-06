/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
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

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using Index = com.opengamma.strata.basics.index.Index;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;

	/// <summary>
	/// Test <seealso cref="KnownAmountBondPaymentPeriod"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class KnownAmountBondPaymentPeriodTest
	public class KnownAmountBondPaymentPeriodTest
	{

	  private static readonly CurrencyAmount GBP_P1000 = CurrencyAmount.of(GBP, 1000);
	  private static readonly LocalDate DATE_2014_03_30 = date(2014, 3, 30);
	  private static readonly LocalDate DATE_2014_06_30 = date(2014, 6, 30);
	  private static readonly LocalDate DATE_2014_09_30 = date(2014, 9, 30);
	  private static readonly LocalDate DATE_2014_10_01 = date(2014, 10, 1);
	  private static readonly LocalDate DATE_2014_10_03 = date(2014, 10, 3);
	  private static readonly Payment PAYMENT_2014_10_01 = Payment.of(GBP_P1000, DATE_2014_10_01);
	  private static readonly Payment PAYMENT_2014_10_03 = Payment.of(GBP_P1000, DATE_2014_10_03);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		SchedulePeriod sched = SchedulePeriod.of(DATE_2014_03_30, DATE_2014_09_30);
		KnownAmountBondPaymentPeriod test = KnownAmountBondPaymentPeriod.of(PAYMENT_2014_10_03, sched);
		assertEquals(test.Payment, PAYMENT_2014_10_03);
		assertEquals(test.StartDate, DATE_2014_03_30);
		assertEquals(test.UnadjustedStartDate, DATE_2014_03_30);
		assertEquals(test.EndDate, DATE_2014_09_30);
		assertEquals(test.UnadjustedEndDate, DATE_2014_09_30);
		assertEquals(test.PaymentDate, DATE_2014_10_03);
		assertEquals(test.Currency, GBP);
	  }

	  public virtual void test_builder_defaultDates()
	  {
		KnownAmountBondPaymentPeriod test = KnownAmountBondPaymentPeriod.builder().payment(PAYMENT_2014_10_03).startDate(DATE_2014_03_30).endDate(DATE_2014_10_01).build();
		assertEquals(test.Payment, PAYMENT_2014_10_03);
		assertEquals(test.StartDate, DATE_2014_03_30);
		assertEquals(test.UnadjustedStartDate, DATE_2014_03_30);
		assertEquals(test.EndDate, DATE_2014_10_01);
		assertEquals(test.UnadjustedEndDate, DATE_2014_10_01);
		assertEquals(test.PaymentDate, DATE_2014_10_03);
		assertEquals(test.Currency, GBP);
	  }

	  public virtual void test_builder_invalid()
	  {
		assertThrowsIllegalArg(() => KnownAmountBondPaymentPeriod.builder().payment(PAYMENT_2014_10_03).endDate(DATE_2014_10_01).build());
		assertThrowsIllegalArg(() => KnownAmountBondPaymentPeriod.builder().payment(PAYMENT_2014_10_03).startDate(DATE_2014_10_01).build());
		assertThrowsIllegalArg(() => KnownAmountBondPaymentPeriod.builder().payment(PAYMENT_2014_10_03).startDate(DATE_2014_10_01).endDate(DATE_2014_10_01).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_adjustPaymentDate()
	  {
		KnownAmountBondPaymentPeriod test = KnownAmountBondPaymentPeriod.builder().payment(PAYMENT_2014_10_01).startDate(DATE_2014_03_30).unadjustedStartDate(DATE_2014_03_30).endDate(DATE_2014_10_01).unadjustedEndDate(DATE_2014_09_30).build();
		KnownAmountBondPaymentPeriod expected = KnownAmountBondPaymentPeriod.builder().payment(PAYMENT_2014_10_03).startDate(DATE_2014_03_30).unadjustedStartDate(DATE_2014_03_30).endDate(DATE_2014_10_01).unadjustedEndDate(DATE_2014_09_30).build();
		assertEquals(test.adjustPaymentDate(TemporalAdjusters.ofDateAdjuster(d => d.plusDays(0))), test);
		assertEquals(test.adjustPaymentDate(TemporalAdjusters.ofDateAdjuster(d => d.plusDays(2))), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_collectIndices_simple()
	  {
		KnownAmountBondPaymentPeriod test = KnownAmountBondPaymentPeriod.builder().payment(PAYMENT_2014_10_03).startDate(DATE_2014_03_30).unadjustedStartDate(DATE_2014_03_30).endDate(DATE_2014_10_01).unadjustedEndDate(DATE_2014_09_30).build();
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		KnownAmountBondPaymentPeriod test = KnownAmountBondPaymentPeriod.builder().payment(PAYMENT_2014_10_03).startDate(DATE_2014_03_30).unadjustedStartDate(DATE_2014_03_30).endDate(DATE_2014_10_01).unadjustedEndDate(DATE_2014_09_30).build();
		coverImmutableBean(test);
		KnownAmountBondPaymentPeriod test2 = KnownAmountBondPaymentPeriod.builder().payment(PAYMENT_2014_10_03.negated()).startDate(DATE_2014_06_30).endDate(DATE_2014_09_30).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		KnownAmountBondPaymentPeriod test = KnownAmountBondPaymentPeriod.builder().payment(PAYMENT_2014_10_03).startDate(DATE_2014_03_30).unadjustedStartDate(DATE_2014_03_30).endDate(DATE_2014_10_01).unadjustedEndDate(DATE_2014_09_30).build();
		assertSerialization(test);
	  }

	}

}