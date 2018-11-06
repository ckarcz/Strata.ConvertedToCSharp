/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.deposit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
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
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// Test <seealso cref="TermDeposit"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TermDepositTest
	public class TermDepositTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const BuySell SELL = BuySell.SELL;
	  private static readonly LocalDate START_DATE = LocalDate.of(2015, 1, 19);
	  private static readonly LocalDate END_DATE = LocalDate.of(2015, 7, 19);
	  private const double NOTIONAL = 100000000d;
	  private const double RATE = 0.0250;
	  private static readonly BusinessDayAdjustment BDA_MOD_FOLLOW = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO);
	  private const double EPS = 1.0e-14;

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		TermDeposit test = TermDeposit.builder().buySell(SELL).startDate(START_DATE).endDate(END_DATE).businessDayAdjustment(BDA_MOD_FOLLOW).dayCount(ACT_365F).notional(NOTIONAL).currency(GBP).rate(RATE).build();
		assertEquals(test.BuySell, SELL);
		assertEquals(test.StartDate, START_DATE);
		assertEquals(test.EndDate, END_DATE);
		assertEquals(test.BusinessDayAdjustment.get(), BDA_MOD_FOLLOW);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.Rate, RATE);
		assertEquals(test.Currency, GBP);
		assertEquals(test.CrossCurrency, false);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(GBP));
		assertEquals(test.allCurrencies(), ImmutableSet.of(GBP));
	  }

	  public virtual void test_builder_wrongDates()
	  {
		assertThrowsIllegalArg(() => TermDeposit.builder().buySell(SELL).startDate(START_DATE).endDate(LocalDate.of(2014, 10, 19)).businessDayAdjustment(BDA_MOD_FOLLOW).dayCount(ACT_365F).notional(NOTIONAL).currency(EUR).rate(RATE).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		TermDeposit @base = TermDeposit.builder().buySell(SELL).startDate(START_DATE).endDate(END_DATE).businessDayAdjustment(BDA_MOD_FOLLOW).dayCount(ACT_365F).notional(NOTIONAL).currency(GBP).rate(RATE).build();
		ResolvedTermDeposit test = @base.resolve(REF_DATA);
		LocalDate expectedEndDate = BDA_MOD_FOLLOW.adjust(END_DATE, REF_DATA);
		double expectedYearFraction = ACT_365F.yearFraction(START_DATE, expectedEndDate);
		assertEquals(test.StartDate, START_DATE);
		assertEquals(test.EndDate, expectedEndDate);
		assertEquals(test.Notional, -NOTIONAL);
		assertEquals(test.YearFraction, expectedYearFraction, EPS);
		assertEquals(test.Interest, -RATE * expectedYearFraction * NOTIONAL, NOTIONAL * EPS);
		assertEquals(test.Rate, RATE);
		assertEquals(test.Currency, GBP);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		TermDeposit test1 = TermDeposit.builder().buySell(SELL).startDate(START_DATE).endDate(END_DATE).businessDayAdjustment(BDA_MOD_FOLLOW).dayCount(ACT_365F).notional(NOTIONAL).currency(GBP).rate(RATE).build();
		coverImmutableBean(test1);
		TermDeposit test2 = TermDeposit.builder().buySell(BuySell.BUY).startDate(LocalDate.of(2015, 1, 21)).endDate(LocalDate.of(2015, 7, 21)).dayCount(ACT_360).notional(NOTIONAL).currency(EUR).rate(RATE).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		TermDeposit test = TermDeposit.builder().buySell(SELL).startDate(START_DATE).endDate(END_DATE).businessDayAdjustment(BDA_MOD_FOLLOW).dayCount(ACT_365F).notional(NOTIONAL).currency(GBP).rate(RATE).build();
		assertSerialization(test);
	  }

	}

}