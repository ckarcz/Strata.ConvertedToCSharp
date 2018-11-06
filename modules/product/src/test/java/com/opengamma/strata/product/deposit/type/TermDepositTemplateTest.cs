/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.deposit.type
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
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
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

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// Test <seealso cref="TermDepositTemplate"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TermDepositTemplateTest
	public class TermDepositTemplateTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly BusinessDayAdjustment BDA_MOD_FOLLOW = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA);
	  private static readonly DaysAdjustment PLUS_TWO_DAYS = DaysAdjustment.ofBusinessDays(2, EUTA);
	  private static readonly TermDepositConvention CONVENTION = TermDepositConventions.EUR_DEPOSIT_T2;
	  private static readonly Period DEPOSIT_PERIOD = Period.ofMonths(3);

	  public virtual void test_builder()
	  {
		TermDepositTemplate test = TermDepositTemplate.builder().convention(CONVENTION).depositPeriod(DEPOSIT_PERIOD).build();
		assertEquals(test.Convention, CONVENTION);
		assertEquals(test.DepositPeriod, DEPOSIT_PERIOD);
	  }

	  public virtual void test_builder_negativePeriod()
	  {
		assertThrowsIllegalArg(() => TermDepositTemplate.builder().convention(CONVENTION).depositPeriod(Period.ofMonths(-2)).build());
	  }

	  public virtual void test_of()
	  {
		TermDepositTemplate test = TermDepositTemplate.of(DEPOSIT_PERIOD, CONVENTION);
		assertEquals(test.Convention, CONVENTION);
		assertEquals(test.DepositPeriod, DEPOSIT_PERIOD);
	  }

	  public virtual void test_createTrade()
	  {
		TermDepositTemplate template = TermDepositTemplate.of(DEPOSIT_PERIOD, CONVENTION);
		LocalDate tradeDate = LocalDate.of(2015, 1, 23);
		BuySell buy = BuySell.BUY;
		double notional = 2_000_000d;
		double rate = 0.0125;
		TermDepositTrade trade = template.createTrade(tradeDate, buy, notional, rate, REF_DATA);
		TradeInfo tradeInfoExpected = TradeInfo.of(tradeDate);
		LocalDate startDateExpected = PLUS_TWO_DAYS.adjust(tradeDate, REF_DATA);
		LocalDate endDateExpected = startDateExpected.plus(DEPOSIT_PERIOD);
		TermDeposit productExpected = TermDeposit.builder().buySell(buy).currency(EUR).notional(notional).businessDayAdjustment(BDA_MOD_FOLLOW).startDate(startDateExpected).endDate(endDateExpected).rate(rate).dayCount(ACT_360).build();
		assertEquals(trade.Info, tradeInfoExpected);
		assertEquals(trade.Product, productExpected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		TermDepositTemplate test1 = TermDepositTemplate.of(DEPOSIT_PERIOD, CONVENTION);
		coverImmutableBean(test1);
		TermDepositTemplate test2 = TermDepositTemplate.of(Period.ofMonths(6), ImmutableTermDepositConvention.of("GBP-Dep", GBP, BDA_MOD_FOLLOW, ACT_365F, DaysAdjustment.ofBusinessDays(2, GBLO)));
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		TermDepositTemplate test = TermDepositTemplate.of(DEPOSIT_PERIOD, CONVENTION);
		assertSerialization(test);
	  }

	}

}