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
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
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
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.SwapLegType.FIXED;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.SwapLegType.IBOR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using Index = com.opengamma.strata.basics.index.Index;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedSwapLegTest
	public class ResolvedSwapLegTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate DATE_2014_06_30 = date(2014, 6, 30);
	  private static readonly LocalDate DATE_2014_09_30 = date(2014, 9, 30);
	  private static readonly LocalDate DATE_2014_12_30 = date(2014, 12, 30);
	  private static readonly LocalDate DATE_2014_10_01 = date(2014, 10, 1);
	  private static readonly LocalDate DATE_2015_01_01 = date(2015, 1, 1);
	  private static readonly IborRateComputation GBP_LIBOR_3M_2014_06_28 = IborRateComputation.of(GBP_LIBOR_3M, date(2014, 6, 28), REF_DATA);
	  private static readonly IborRateComputation GBP_LIBOR_3M_2014_09_28 = IborRateComputation.of(GBP_LIBOR_3M, date(2014, 9, 28), REF_DATA);
	  private static readonly NotionalExchange NOTIONAL_EXCHANGE = NotionalExchange.of(CurrencyAmount.of(GBP, 2000d), DATE_2014_10_01);
	  private static readonly RateAccrualPeriod RAP1 = RateAccrualPeriod.builder().startDate(DATE_2014_06_30).endDate(DATE_2014_09_30).yearFraction(0.25d).rateComputation(GBP_LIBOR_3M_2014_06_28).build();
	  private static readonly RateAccrualPeriod RAP2 = RateAccrualPeriod.builder().startDate(DATE_2014_09_30).endDate(DATE_2014_12_30).yearFraction(0.25d).rateComputation(GBP_LIBOR_3M_2014_09_28).build();
	  private static readonly RatePaymentPeriod RPP1 = RatePaymentPeriod.builder().paymentDate(DATE_2014_10_01).accrualPeriods(RAP1).dayCount(ACT_365F).currency(GBP).notional(5000d).build();
	  private static readonly RatePaymentPeriod RPP2 = RatePaymentPeriod.builder().paymentDate(DATE_2015_01_01).accrualPeriods(RAP2).dayCount(ACT_365F).currency(GBP).notional(6000d).build();
	  private static readonly RatePaymentPeriod RPP3 = RatePaymentPeriod.builder().paymentDate(DATE_2014_10_01).accrualPeriods(RAP1).dayCount(ACT_365F).currency(USD).notional(6000d).build();

	  public virtual void test_builder()
	  {
		ResolvedSwapLeg test = ResolvedSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1).paymentEvents(NOTIONAL_EXCHANGE).build();
		assertEquals(test.Type, IBOR);
		assertEquals(test.PayReceive, RECEIVE);
		assertEquals(test.StartDate, DATE_2014_06_30);
		assertEquals(test.EndDate, DATE_2014_09_30);
		assertEquals(test.Currency, GBP);
		assertEquals(test.PaymentPeriods, ImmutableList.of(RPP1));
		assertEquals(test.PaymentEvents, ImmutableList.of(NOTIONAL_EXCHANGE));
	  }

	  public virtual void test_builder_invalidMixedCurrency()
	  {
		assertThrowsIllegalArg(() => ResolvedSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP3).paymentEvents(NOTIONAL_EXCHANGE).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_findPaymentPeriod()
	  {
		ResolvedSwapLeg test = ResolvedSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1, RPP2).build();
		assertEquals(test.findPaymentPeriod(RPP1.StartDate), null);
		assertEquals(test.findPaymentPeriod(RPP1.StartDate.plusDays(1)), RPP1);
		assertEquals(test.findPaymentPeriod(RPP1.EndDate), RPP1);
		assertEquals(test.findPaymentPeriod(RPP2.StartDate), RPP1);
		assertEquals(test.findPaymentPeriod(RPP2.StartDate.plusDays(1)), RPP2);
		assertEquals(test.findPaymentPeriod(RPP2.EndDate), RPP2);
		assertEquals(test.findPaymentPeriod(RPP2.EndDate.plusDays(1)), null);
	  }

	  public virtual void test_collectIndices()
	  {
		ResolvedSwapLeg test = ResolvedSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1).paymentEvents(NOTIONAL_EXCHANGE).build();
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of(GBP_LIBOR_3M));
	  }

	  public virtual void test_findNotional()
	  {
		ResolvedSwapLeg test = ResolvedSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1, RPP2).build();
		// Date is before the start date
		assertEquals(test.findNotional(RPP1.StartDate.minusMonths(1)), RPP1.NotionalAmount);
		// Date is on the start date
		assertEquals(test.findNotional(RPP1.StartDate), RPP1.NotionalAmount);
		// Date is after the start date
		assertEquals(test.findNotional(RPP1.StartDate.plusDays(1)), RPP1.NotionalAmount);
		// Date is before the end date
		assertEquals(test.findNotional(RPP2.EndDate.minusDays(1)), RPP2.NotionalAmount);
		// Date is on the end date
		assertEquals(test.findNotional(RPP2.EndDate), RPP2.NotionalAmount);
		// Date is after the end date
		assertEquals(test.findNotional(RPP2.EndDate.plusMonths(1)), RPP2.NotionalAmount);
	  }

	  public virtual void test_findNotionalKnownAmount()
	  {
		Payment payment = Payment.of(GBP, 1000, LocalDate.of(2011, 3, 8));
		SchedulePeriod schedulePeriod = SchedulePeriod.of(LocalDate.of(2010, 3, 8), LocalDate.of(2011, 3, 8));
		KnownAmountSwapPaymentPeriod paymentPeriod = KnownAmountSwapPaymentPeriod.of(payment, schedulePeriod);
		ResolvedSwapLeg test = ResolvedSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(paymentPeriod).build();
		// Date is before the start date
		assertEquals(test.findNotional(RPP1.StartDate.minusMonths(1)), null);
		// Date is on the start date
		assertEquals(test.findNotional(RPP1.StartDate), null);
		// Date is after the start date
		assertEquals(test.findNotional(RPP1.StartDate.plusDays(1)), null);
		// Date is before the end date
		assertEquals(test.findNotional(RPP2.EndDate.minusDays(1)), null);
		// Date is on the end date
		assertEquals(test.findNotional(RPP2.EndDate), null);
		// Date is after the end date
		assertEquals(test.findNotional(RPP2.EndDate.plusMonths(1)), null);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ResolvedSwapLeg test = ResolvedSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1).paymentEvents(NOTIONAL_EXCHANGE).build();
		coverImmutableBean(test);
		ResolvedSwapLeg test2 = ResolvedSwapLeg.builder().type(FIXED).payReceive(PAY).paymentPeriods(RPP2).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		ResolvedSwapLeg test = ResolvedSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1).paymentEvents(NOTIONAL_EXCHANGE).build();
		assertSerialization(test);
	  }

	}

}