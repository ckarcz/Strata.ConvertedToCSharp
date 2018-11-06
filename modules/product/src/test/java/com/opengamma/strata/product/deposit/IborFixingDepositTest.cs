/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.deposit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;

	/// <summary>
	/// Test <seealso cref="IborFixingDeposit"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborFixingDepositTest
	public class IborFixingDepositTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const BuySell SELL = BuySell.SELL;
	  private static readonly LocalDate START_DATE = LocalDate.of(2015, 1, 19);
	  private static readonly LocalDate END_DATE = LocalDate.of(2015, 7, 19);
	  private const double NOTIONAL = 100000000d;
	  private const double RATE = 0.0250;
	  private static readonly BusinessDayAdjustment BDA_MOD_FOLLOW = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO);
	  private static readonly DaysAdjustment DAY_ADJ = DaysAdjustment.ofBusinessDays(1, GBLO);

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_full()
	  {
		IborFixingDeposit test = IborFixingDeposit.builder().buySell(SELL).notional(NOTIONAL).currency(GBP).startDate(START_DATE).endDate(END_DATE).businessDayAdjustment(BDA_MOD_FOLLOW).fixingDateOffset(DAY_ADJ).dayCount(ACT_365F).index(GBP_LIBOR_6M).fixedRate(RATE).build();
		assertEquals(test.BusinessDayAdjustment.get(), BDA_MOD_FOLLOW);
		assertEquals(test.BuySell, SELL);
		assertEquals(test.FixingDateOffset, DAY_ADJ);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.Currency, GBP);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.StartDate, START_DATE);
		assertEquals(test.EndDate, END_DATE);
		assertEquals(test.Index, GBP_LIBOR_6M);
		assertEquals(test.FixedRate, RATE);
		assertEquals(test.CrossCurrency, false);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(GBP));
		assertEquals(test.allCurrencies(), ImmutableSet.of(GBP));
	  }

	  public virtual void test_builder_minimum()
	  {
		IborFixingDeposit test = IborFixingDeposit.builder().buySell(SELL).notional(NOTIONAL).startDate(START_DATE).endDate(END_DATE).businessDayAdjustment(BDA_MOD_FOLLOW).index(GBP_LIBOR_6M).fixedRate(RATE).build();
		assertEquals(test.BusinessDayAdjustment.get(), BDA_MOD_FOLLOW);
		assertEquals(test.BuySell, SELL);
		assertEquals(test.FixingDateOffset, GBP_LIBOR_6M.FixingDateOffset);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.Currency, GBP);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.StartDate, START_DATE);
		assertEquals(test.EndDate, END_DATE);
		assertEquals(test.Index, GBP_LIBOR_6M);
		assertEquals(test.FixedRate, RATE);
	  }

	  public virtual void test_builder_wrongDates()
	  {
		assertThrowsIllegalArg(() => IborFixingDeposit.builder().buySell(SELL).notional(NOTIONAL).startDate(LocalDate.of(2015, 9, 19)).endDate(END_DATE).businessDayAdjustment(BDA_MOD_FOLLOW).index(GBP_LIBOR_6M).fixedRate(RATE).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		IborFixingDeposit @base = IborFixingDeposit.builder().buySell(SELL).notional(NOTIONAL).startDate(START_DATE).endDate(END_DATE).businessDayAdjustment(BDA_MOD_FOLLOW).index(GBP_LIBOR_6M).fixedRate(RATE).build();
		ResolvedIborFixingDeposit test = @base.resolve(REF_DATA);
		LocalDate expectedEndDate = BDA_MOD_FOLLOW.adjust(END_DATE, REF_DATA);
		double expectedYearFraction = ACT_365F.yearFraction(START_DATE, expectedEndDate);
		IborRateComputation expectedObservation = IborRateComputation.of(GBP_LIBOR_6M, GBP_LIBOR_6M.FixingDateOffset.adjust(START_DATE, REF_DATA), REF_DATA);
		assertEquals(test.Currency, GBP);
		assertEquals(test.StartDate, START_DATE);
		assertEquals(test.EndDate, expectedEndDate);
		assertEquals(test.FloatingRate, expectedObservation);
		assertEquals(test.Notional, -NOTIONAL);
		assertEquals(test.FixedRate, RATE);
		assertEquals(test.YearFraction, expectedYearFraction);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		IborFixingDeposit test1 = IborFixingDeposit.builder().buySell(SELL).notional(NOTIONAL).startDate(START_DATE).endDate(END_DATE).businessDayAdjustment(BDA_MOD_FOLLOW).index(GBP_LIBOR_6M).fixedRate(RATE).build();
		coverImmutableBean(test1);
		IborFixingDeposit test2 = IborFixingDeposit.builder().buySell(BuySell.BUY).notional(NOTIONAL).startDate(LocalDate.of(2015, 1, 19)).endDate(LocalDate.of(2015, 4, 19)).businessDayAdjustment(BDA_MOD_FOLLOW).index(GBP_LIBOR_3M).fixedRate(0.015).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		IborFixingDeposit test = IborFixingDeposit.builder().buySell(SELL).notional(NOTIONAL).startDate(START_DATE).endDate(END_DATE).businessDayAdjustment(BDA_MOD_FOLLOW).index(GBP_LIBOR_6M).fixedRate(RATE).build();
		assertSerialization(test);
	  }

	}

}