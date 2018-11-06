/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.FxIndices.GBP_USD_WM;
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
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;
	using Index = com.opengamma.strata.basics.index.Index;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;

	/// <summary>
	/// Test <seealso cref="KnownAmountNotionalSwapPaymentPeriod"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class KnownAmountNotionalSwapPaymentPeriodTest
	public class KnownAmountNotionalSwapPaymentPeriodTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly CurrencyAmount GBP_P50000 = CurrencyAmount.of(GBP, 50000);
	  private static readonly CurrencyAmount GBP_P1000 = CurrencyAmount.of(GBP, 1000);
	  private static readonly CurrencyAmount USD_P50000 = CurrencyAmount.of(USD, 50000);
	  private static readonly LocalDate DATE_2014_03_30 = date(2014, 3, 30);
	  private static readonly LocalDate DATE_2014_06_30 = date(2014, 6, 30);
	  private static readonly LocalDate DATE_2014_09_30 = date(2014, 9, 30);
	  private static readonly LocalDate DATE_2014_10_01 = date(2014, 10, 1);
	  private static readonly LocalDate DATE_2014_10_03 = date(2014, 10, 3);
	  private static readonly Payment PAYMENT_2014_10_01 = Payment.of(GBP_P1000, DATE_2014_10_01);
	  private static readonly Payment PAYMENT_2014_10_03 = Payment.of(GBP_P1000, DATE_2014_10_03);
	  private static readonly FxIndexObservation FX_RESET = FxIndexObservation.of(GBP_USD_WM, date(2014, 3, 28), REF_DATA);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		SchedulePeriod sched = SchedulePeriod.of(DATE_2014_03_30, DATE_2014_09_30);
		KnownAmountNotionalSwapPaymentPeriod test = KnownAmountNotionalSwapPaymentPeriod.of(PAYMENT_2014_10_03, sched, GBP_P50000);
		assertEquals(test.Payment, PAYMENT_2014_10_03);
		assertEquals(test.StartDate, DATE_2014_03_30);
		assertEquals(test.UnadjustedStartDate, DATE_2014_03_30);
		assertEquals(test.EndDate, DATE_2014_09_30);
		assertEquals(test.UnadjustedEndDate, DATE_2014_09_30);
		assertEquals(test.PaymentDate, DATE_2014_10_03);
		assertEquals(test.Currency, GBP);
		assertEquals(test.NotionalAmount, GBP_P50000);
		assertEquals(test.FxResetObservation, null);
	  }

	  public virtual void test_of_fxReset()
	  {
		SchedulePeriod sched = SchedulePeriod.of(DATE_2014_03_30, DATE_2014_09_30);
		KnownAmountNotionalSwapPaymentPeriod test = KnownAmountNotionalSwapPaymentPeriod.of(PAYMENT_2014_10_03, sched, USD_P50000, FX_RESET);
		assertEquals(test.Payment, PAYMENT_2014_10_03);
		assertEquals(test.StartDate, DATE_2014_03_30);
		assertEquals(test.UnadjustedStartDate, DATE_2014_03_30);
		assertEquals(test.EndDate, DATE_2014_09_30);
		assertEquals(test.UnadjustedEndDate, DATE_2014_09_30);
		assertEquals(test.PaymentDate, DATE_2014_10_03);
		assertEquals(test.Currency, GBP);
		assertEquals(test.NotionalAmount, USD_P50000);
		assertEquals(test.FxResetObservation, FX_RESET);
	  }

	  public virtual void test_builder_defaultDates()
	  {
		KnownAmountNotionalSwapPaymentPeriod test = KnownAmountNotionalSwapPaymentPeriod.builder().payment(PAYMENT_2014_10_03).startDate(DATE_2014_03_30).endDate(DATE_2014_10_01).notionalAmount(USD_P50000).fxResetObservation(FX_RESET).build();
		assertEquals(test.Payment, PAYMENT_2014_10_03);
		assertEquals(test.StartDate, DATE_2014_03_30);
		assertEquals(test.UnadjustedStartDate, DATE_2014_03_30);
		assertEquals(test.EndDate, DATE_2014_10_01);
		assertEquals(test.UnadjustedEndDate, DATE_2014_10_01);
		assertEquals(test.PaymentDate, DATE_2014_10_03);
		assertEquals(test.Currency, GBP);
		assertEquals(test.NotionalAmount, USD_P50000);
		assertEquals(test.FxResetObservation, FX_RESET);
	  }

	  public virtual void test_builder_invalid()
	  {
		assertThrowsIllegalArg(() => KnownAmountNotionalSwapPaymentPeriod.builder().payment(PAYMENT_2014_10_03).endDate(DATE_2014_10_01).notionalAmount(GBP_P50000).build());
		assertThrowsIllegalArg(() => KnownAmountNotionalSwapPaymentPeriod.builder().payment(PAYMENT_2014_10_03).startDate(DATE_2014_10_01).notionalAmount(GBP_P50000).build());
		assertThrowsIllegalArg(() => KnownAmountNotionalSwapPaymentPeriod.builder().payment(PAYMENT_2014_10_03).startDate(DATE_2014_10_01).endDate(DATE_2014_10_01).notionalAmount(GBP_P50000).build());
		assertThrowsIllegalArg(() => KnownAmountNotionalSwapPaymentPeriod.builder().payment(PAYMENT_2014_10_03).startDate(DATE_2014_03_30).endDate(DATE_2014_10_01).notionalAmount(CurrencyAmount.of(USD, 1000d)).build());
		assertThrowsIllegalArg(() => KnownAmountNotionalSwapPaymentPeriod.builder().payment(PAYMENT_2014_10_03).startDate(DATE_2014_03_30).endDate(DATE_2014_10_01).notionalAmount(CurrencyAmount.of(GBP, 1000d)).fxResetObservation(FX_RESET).build());
		assertThrowsIllegalArg(() => KnownAmountNotionalSwapPaymentPeriod.builder().payment(PAYMENT_2014_10_03).startDate(DATE_2014_03_30).endDate(DATE_2014_10_01).notionalAmount(CurrencyAmount.of(EUR, 1000d)).fxResetObservation(FX_RESET).build());
		assertThrowsIllegalArg(() => KnownAmountNotionalSwapPaymentPeriod.builder().payment(Payment.of(CurrencyAmount.of(EUR, 1000d), DATE_2014_10_03)).startDate(DATE_2014_03_30).endDate(DATE_2014_10_01).notionalAmount(CurrencyAmount.of(USD, 1000d)).fxResetObservation(FX_RESET).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_adjustPaymentDate()
	  {
		KnownAmountNotionalSwapPaymentPeriod test = KnownAmountNotionalSwapPaymentPeriod.builder().payment(PAYMENT_2014_10_01).startDate(DATE_2014_03_30).unadjustedStartDate(DATE_2014_03_30).endDate(DATE_2014_10_01).unadjustedEndDate(DATE_2014_09_30).notionalAmount(GBP_P50000).build();
		KnownAmountNotionalSwapPaymentPeriod expected = KnownAmountNotionalSwapPaymentPeriod.builder().payment(PAYMENT_2014_10_03).startDate(DATE_2014_03_30).unadjustedStartDate(DATE_2014_03_30).endDate(DATE_2014_10_01).unadjustedEndDate(DATE_2014_09_30).notionalAmount(GBP_P50000).build();
		assertEquals(test.adjustPaymentDate(TemporalAdjusters.ofDateAdjuster(d => d.plusDays(0))), test);
		assertEquals(test.adjustPaymentDate(TemporalAdjusters.ofDateAdjuster(d => d.plusDays(2))), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_collectIndices_simple()
	  {
		KnownAmountNotionalSwapPaymentPeriod test = KnownAmountNotionalSwapPaymentPeriod.builder().payment(PAYMENT_2014_10_03).startDate(DATE_2014_03_30).unadjustedStartDate(DATE_2014_03_30).endDate(DATE_2014_10_01).unadjustedEndDate(DATE_2014_09_30).notionalAmount(GBP_P50000).build();
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of());
	  }

	  public virtual void test_collectIndices_fxReset()
	  {
		SchedulePeriod sched = SchedulePeriod.of(DATE_2014_03_30, DATE_2014_09_30);
		KnownAmountNotionalSwapPaymentPeriod test = KnownAmountNotionalSwapPaymentPeriod.of(PAYMENT_2014_10_03, sched, USD_P50000, FX_RESET);
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of(FX_RESET.Index));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		KnownAmountNotionalSwapPaymentPeriod test = KnownAmountNotionalSwapPaymentPeriod.builder().payment(PAYMENT_2014_10_03).startDate(DATE_2014_03_30).unadjustedStartDate(DATE_2014_03_30).endDate(DATE_2014_10_01).unadjustedEndDate(DATE_2014_09_30).notionalAmount(GBP_P50000).build();
		coverImmutableBean(test);
		KnownAmountNotionalSwapPaymentPeriod test2 = KnownAmountNotionalSwapPaymentPeriod.builder().payment(PAYMENT_2014_10_03.negated()).startDate(DATE_2014_06_30).endDate(DATE_2014_09_30).notionalAmount(GBP_P1000).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		KnownAmountNotionalSwapPaymentPeriod test = KnownAmountNotionalSwapPaymentPeriod.builder().payment(PAYMENT_2014_10_03).startDate(DATE_2014_03_30).unadjustedStartDate(DATE_2014_03_30).endDate(DATE_2014_10_01).unadjustedEndDate(DATE_2014_09_30).notionalAmount(GBP_P50000).build();
		assertSerialization(test);
	  }

	}

}