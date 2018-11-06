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
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.SwapLegType.FIXED;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Index = com.opengamma.strata.basics.index.Index;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using ValueAdjustment = com.opengamma.strata.basics.value.ValueAdjustment;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using ValueStep = com.opengamma.strata.basics.value.ValueStep;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class KnownAmountSwapLegTest
	public class KnownAmountSwapLegTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate DATE_01_05 = date(2014, 1, 5);
	  private static readonly LocalDate DATE_01_06 = date(2014, 1, 6);
	  private static readonly LocalDate DATE_02_05 = date(2014, 2, 5);
	  private static readonly LocalDate DATE_02_07 = date(2014, 2, 7);
	  private static readonly LocalDate DATE_03_05 = date(2014, 3, 5);
	  private static readonly LocalDate DATE_03_07 = date(2014, 3, 7);
	  private static readonly LocalDate DATE_04_05 = date(2014, 4, 5);
	  private static readonly LocalDate DATE_04_07 = date(2014, 4, 7);
	  private static readonly LocalDate DATE_04_09 = date(2014, 4, 9);
	  private static readonly DaysAdjustment PLUS_THREE_DAYS = DaysAdjustment.ofBusinessDays(3, GBLO);
	  private static readonly DaysAdjustment PLUS_TWO_DAYS = DaysAdjustment.ofBusinessDays(2, GBLO);

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		BusinessDayAdjustment bda = BusinessDayAdjustment.of(FOLLOWING, GBLO);
		PeriodicSchedule accrualSchedule = PeriodicSchedule.builder().startDate(DATE_01_05).endDate(DATE_04_05).frequency(P1M).businessDayAdjustment(bda).build();
		PaymentSchedule paymentSchedule = PaymentSchedule.builder().paymentFrequency(P1M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).build();
		ValueSchedule amountSchedule = ValueSchedule.of(123d);
		KnownAmountSwapLeg test = KnownAmountSwapLeg.builder().payReceive(PAY).accrualSchedule(accrualSchedule).paymentSchedule(paymentSchedule).amount(amountSchedule).currency(GBP).build();
		assertEquals(test.PayReceive, PAY);
		assertEquals(test.StartDate, AdjustableDate.of(DATE_01_05, bda));
		assertEquals(test.EndDate, AdjustableDate.of(DATE_04_05, bda));
		assertEquals(test.AccrualSchedule, accrualSchedule);
		assertEquals(test.PaymentSchedule, paymentSchedule);
		assertEquals(test.Amount, amountSchedule);
		assertEquals(test.Currency, GBP);
		assertEquals(test.allCurrencies(), ImmutableSet.of(GBP));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_collectIndices()
	  {
		KnownAmountSwapLeg test = KnownAmountSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(DATE_01_05).endDate(DATE_04_05).frequency(P1M).businessDayAdjustment(BusinessDayAdjustment.of(FOLLOWING, GBLO)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(P1M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).build()).amount(ValueSchedule.of(123d)).currency(GBP).build();
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of());
		assertEquals(test.allIndices(), ImmutableSet.of());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		// test case
		KnownAmountSwapLeg test = KnownAmountSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(DATE_01_05).endDate(DATE_04_05).frequency(P1M).businessDayAdjustment(BusinessDayAdjustment.of(FOLLOWING, GBLO)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(P1M).paymentDateOffset(PLUS_TWO_DAYS).build()).amount(ValueSchedule.builder().initialValue(123d).steps(ValueStep.of(1, ValueAdjustment.ofReplace(234d))).build()).currency(GBP).build();
		// expected
		KnownAmountSwapPaymentPeriod rpp1 = KnownAmountSwapPaymentPeriod.builder().payment(Payment.ofPay(CurrencyAmount.of(GBP, 123d), DATE_02_07)).startDate(DATE_01_06).endDate(DATE_02_05).unadjustedStartDate(DATE_01_05).build();
		KnownAmountSwapPaymentPeriod rpp2 = KnownAmountSwapPaymentPeriod.builder().payment(Payment.ofPay(CurrencyAmount.of(GBP, 234d), DATE_03_07)).startDate(DATE_02_05).endDate(DATE_03_05).build();
		KnownAmountSwapPaymentPeriod rpp3 = KnownAmountSwapPaymentPeriod.builder().payment(Payment.ofPay(CurrencyAmount.of(GBP, 234d), DATE_04_09)).startDate(DATE_03_05).endDate(DATE_04_07).unadjustedEndDate(DATE_04_05).build();
		// assertion
		assertEquals(test.resolve(REF_DATA), ResolvedSwapLeg.builder().type(FIXED).payReceive(PAY).paymentPeriods(rpp1, rpp2, rpp3).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		KnownAmountSwapLeg test = KnownAmountSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(DATE_01_05).endDate(DATE_04_05).frequency(P1M).businessDayAdjustment(BusinessDayAdjustment.of(FOLLOWING, GBLO)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(P1M).paymentDateOffset(PLUS_TWO_DAYS).build()).amount(ValueSchedule.of(123d)).currency(GBP).build();
		coverImmutableBean(test);
		KnownAmountSwapLeg test2 = KnownAmountSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(DATE_02_05).endDate(DATE_03_05).frequency(P1M).businessDayAdjustment(BusinessDayAdjustment.of(FOLLOWING, GBLO)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(P1M).paymentDateOffset(PLUS_THREE_DAYS).build()).amount(ValueSchedule.of(2000d)).currency(EUR).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		KnownAmountSwapLeg test = KnownAmountSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(DATE_01_05).endDate(DATE_04_05).frequency(P1M).businessDayAdjustment(BusinessDayAdjustment.of(FOLLOWING, GBLO)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(P1M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).build()).amount(ValueSchedule.of(123d)).currency(GBP).build();
		assertSerialization(test);
	  }

	}

}