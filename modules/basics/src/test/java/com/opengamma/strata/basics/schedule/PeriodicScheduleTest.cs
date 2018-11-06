using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.schedule
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_PRECEDING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.PRECEDING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.JPTO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.NO_HOLIDAYS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.SAT_SUN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P12M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P2M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.TERM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.DAY_11;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.DAY_17;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.DAY_24;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.DAY_28;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.DAY_29;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.DAY_30;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.DAY_4;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.EOM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.IMM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.SFE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.StubConvention.LONG_FINAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.StubConvention.LONG_INITIAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.StubConvention.SHORT_FINAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.StubConvention.SHORT_INITIAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.StubConvention.SMART_FINAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.StubConvention.SMART_INITIAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.list;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConvention = com.opengamma.strata.basics.date.BusinessDayConvention;
	using HolidayCalendar = com.opengamma.strata.basics.date.HolidayCalendar;

	/// <summary>
	/// Test <seealso cref="PeriodicSchedule"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PeriodicScheduleTest
	public class PeriodicScheduleTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly RollConvention ROLL_NONE = RollConventions.NONE;
	  private const StubConvention STUB_NONE = StubConvention.NONE;
	  private const StubConvention STUB_BOTH = StubConvention.BOTH;
	  private static readonly BusinessDayAdjustment BDA = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, SAT_SUN);
	  private static readonly BusinessDayAdjustment BDA_JPY_MF = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, JPTO);
	  private static readonly BusinessDayAdjustment BDA_JPY_P = BusinessDayAdjustment.of(PRECEDING, JPTO);
	  private static readonly BusinessDayAdjustment BDA_NONE = BusinessDayAdjustment.NONE;
	  private static readonly LocalDate NOV_29_2013 = date(2013, NOVEMBER, 29); // Fri
	  private static readonly LocalDate NOV_30_2013 = date(2013, NOVEMBER, 30); // Sat
	  private static readonly LocalDate FEB_28 = date(2014, FEBRUARY, 28); // Fri
	  private static readonly LocalDate APR_01 = date(2014, APRIL, 1); // Tue
	  private static readonly LocalDate MAY_17 = date(2014, MAY, 17); // Sat
	  private static readonly LocalDate MAY_19 = date(2014, MAY, 19); // Mon
	  private static readonly LocalDate MAY_30 = date(2014, MAY, 30); // Fri
	  private static readonly LocalDate MAY_31 = date(2014, MAY, 31); // Sat
	  private static readonly LocalDate JUN_03 = date(2014, JUNE, 3); // Tue
	  private static readonly LocalDate JUN_04 = date(2014, JUNE, 4); // Wed
	  private static readonly LocalDate JUN_10 = date(2014, JUNE, 10); // Tue
	  private static readonly LocalDate JUN_11 = date(2014, JUNE, 11); // Wed
	  private static readonly LocalDate JUN_17 = date(2014, JUNE, 17); // Tue
	  private static readonly LocalDate JUL_04 = date(2014, JULY, 4); // Fri
	  private static readonly LocalDate JUL_11 = date(2014, JULY, 11); // Fri
	  private static readonly LocalDate JUL_17 = date(2014, JULY, 17); // Thu
	  private static readonly LocalDate JUL_30 = date(2014, JULY, 30); // Wed
	  private static readonly LocalDate AUG_04 = date(2014, AUGUST, 4); // Mon
	  private static readonly LocalDate AUG_11 = date(2014, AUGUST, 11); // Mon
	  private static readonly LocalDate AUG_17 = date(2014, AUGUST, 17); // Sun
	  private static readonly LocalDate AUG_18 = date(2014, AUGUST, 18); // Mon
	  private static readonly LocalDate AUG_29 = date(2014, AUGUST, 29); // Fri
	  private static readonly LocalDate AUG_30 = date(2014, AUGUST, 30); // Sat
	  private static readonly LocalDate AUG_31 = date(2014, AUGUST, 31); // Sun
	  private static readonly LocalDate SEP_04 = date(2014, SEPTEMBER, 4); // Thu
	  private static readonly LocalDate SEP_05 = date(2014, SEPTEMBER, 5); // Fri
	  private static readonly LocalDate SEP_10 = date(2014, SEPTEMBER, 10); // Wed
	  private static readonly LocalDate SEP_11 = date(2014, SEPTEMBER, 11); // Thu
	  private static readonly LocalDate SEP_17 = date(2014, SEPTEMBER, 17); // Wed
	  private static readonly LocalDate SEP_18 = date(2014, SEPTEMBER, 18); // Thu
	  private static readonly LocalDate SEP_30 = date(2014, SEPTEMBER, 30); // Tue
	  private static readonly LocalDate OCT_17 = date(2014, OCTOBER, 17); // Fri
	  private static readonly LocalDate OCT_30 = date(2014, OCTOBER, 30); // Thu
	  private static readonly LocalDate NOV_28 = date(2014, NOVEMBER, 28); // Fri
	  private static readonly LocalDate NOV_30 = date(2014, NOVEMBER, 30); // Sun

	  //-------------------------------------------------------------------------
	  public virtual void test_of_LocalDateEomFalse()
	  {
		PeriodicSchedule test = PeriodicSchedule.of(JUN_04, SEP_17, P1M, BDA, SHORT_INITIAL, false);
		assertEquals(test.StartDate, JUN_04);
		assertEquals(test.EndDate, SEP_17);
		assertEquals(test.Frequency, P1M);
		assertEquals(test.BusinessDayAdjustment, BDA);
		assertEquals(test.StartDateBusinessDayAdjustment, null);
		assertEquals(test.EndDateBusinessDayAdjustment, null);
		assertEquals(test.StubConvention, SHORT_INITIAL);
		assertEquals(test.RollConvention, null);
		assertEquals(test.FirstRegularStartDate, null);
		assertEquals(test.LastRegularEndDate, null);
		assertEquals(test.OverrideStartDate, null);
		assertEquals(test.calculatedRollConvention(), DAY_17);
		assertEquals(test.calculatedFirstRegularStartDate(), JUN_04);
		assertEquals(test.calculatedLastRegularEndDate(), SEP_17);
		assertEquals(test.calculatedStartDate(), AdjustableDate.of(JUN_04, BDA));
		assertEquals(test.calculatedEndDate(), AdjustableDate.of(SEP_17, BDA));
	  }

	  public virtual void test_of_LocalDateEomTrue()
	  {
		PeriodicSchedule test = PeriodicSchedule.of(JUN_04, SEP_17, P1M, BDA, SHORT_FINAL, true);
		assertEquals(test.StartDate, JUN_04);
		assertEquals(test.EndDate, SEP_17);
		assertEquals(test.Frequency, P1M);
		assertEquals(test.BusinessDayAdjustment, BDA);
		assertEquals(test.StartDateBusinessDayAdjustment, null);
		assertEquals(test.EndDateBusinessDayAdjustment, null);
		assertEquals(test.StubConvention, SHORT_FINAL);
		assertEquals(test.RollConvention, EOM);
		assertEquals(test.FirstRegularStartDate, null);
		assertEquals(test.LastRegularEndDate, null);
		assertEquals(test.OverrideStartDate, null);
		assertEquals(test.calculatedRollConvention(), DAY_4);
		assertEquals(test.calculatedFirstRegularStartDate(), JUN_04);
		assertEquals(test.calculatedLastRegularEndDate(), SEP_17);
		assertEquals(test.calculatedStartDate(), AdjustableDate.of(JUN_04, BDA));
		assertEquals(test.calculatedEndDate(), AdjustableDate.of(SEP_17, BDA));
	  }

	  public virtual void test_of_LocalDateEom_null()
	  {
		assertThrowsIllegalArg(() => PeriodicSchedule.of(null, SEP_17, P1M, BDA, SHORT_INITIAL, false));
		assertThrowsIllegalArg(() => PeriodicSchedule.of(JUN_04, null, P1M, BDA, SHORT_INITIAL, false));
		assertThrowsIllegalArg(() => PeriodicSchedule.of(JUN_04, SEP_17, null, BDA, SHORT_INITIAL, false));
		assertThrowsIllegalArg(() => PeriodicSchedule.of(JUN_04, SEP_17, P1M, null, SHORT_INITIAL, false));
		assertThrowsIllegalArg(() => PeriodicSchedule.of(JUN_04, SEP_17, P1M, BDA, null, false));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_LocalDateRoll()
	  {
		PeriodicSchedule test = PeriodicSchedule.of(JUN_04, SEP_17, P1M, BDA, SHORT_INITIAL, DAY_17);
		assertEquals(test.StartDate, JUN_04);
		assertEquals(test.EndDate, SEP_17);
		assertEquals(test.Frequency, P1M);
		assertEquals(test.BusinessDayAdjustment, BDA);
		assertEquals(test.StartDateBusinessDayAdjustment, null);
		assertEquals(test.EndDateBusinessDayAdjustment, null);
		assertEquals(test.StubConvention, SHORT_INITIAL);
		assertEquals(test.RollConvention, DAY_17);
		assertEquals(test.FirstRegularStartDate, null);
		assertEquals(test.LastRegularEndDate, null);
		assertEquals(test.OverrideStartDate, null);
		assertEquals(test.calculatedRollConvention(), DAY_17);
		assertEquals(test.calculatedFirstRegularStartDate(), JUN_04);
		assertEquals(test.calculatedLastRegularEndDate(), SEP_17);
		assertEquals(test.calculatedStartDate(), AdjustableDate.of(JUN_04, BDA));
		assertEquals(test.calculatedEndDate(), AdjustableDate.of(SEP_17, BDA));
	  }

	  public virtual void test_of_LocalDateRoll_null()
	  {
		assertThrowsIllegalArg(() => PeriodicSchedule.of(null, SEP_17, P1M, BDA, SHORT_INITIAL, DAY_17));
		assertThrowsIllegalArg(() => PeriodicSchedule.of(JUN_04, null, P1M, BDA, SHORT_INITIAL, DAY_17));
		assertThrowsIllegalArg(() => PeriodicSchedule.of(JUN_04, SEP_17, null, BDA, SHORT_INITIAL, DAY_17));
		assertThrowsIllegalArg(() => PeriodicSchedule.of(JUN_04, SEP_17, P1M, null, SHORT_INITIAL, DAY_17));
		assertThrowsIllegalArg(() => PeriodicSchedule.of(JUN_04, SEP_17, P1M, BDA, null, DAY_17));
		assertThrowsIllegalArg(() => PeriodicSchedule.of(JUN_04, SEP_17, P1M, BDA, SHORT_INITIAL, null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_invalidDateOrder()
	  {
		// start vs end
		assertThrowsIllegalArg(() => createDates(SEP_17, SEP_17, null, null));
		assertThrowsIllegalArg(() => createDates(SEP_17, JUN_04, null, null));
		// first/last regular vs start/end
		assertThrowsIllegalArg(() => createDates(JUN_04, SEP_17, JUN_03, null));
		assertThrowsIllegalArg(() => createDates(JUN_04, SEP_17, null, SEP_18));
		// first regular vs last regular
		assertThrowsIllegalArg(() => createDates(JUN_04, SEP_17, SEP_05, SEP_05));
		assertThrowsIllegalArg(() => createDates(JUN_04, SEP_17, SEP_05, SEP_04));
		// first regular vs override start date
		assertThrowsIllegalArg(() => PeriodicSchedule.builder().startDate(JUN_04).endDate(SEP_17).frequency(P1M).businessDayAdjustment(BDA).firstRegularStartDate(JUL_17).overrideStartDate(AdjustableDate.of(AUG_04)).build());
	  }

	  private PeriodicSchedule createDates(LocalDate start, LocalDate end, LocalDate first, LocalDate last)
	  {
		return PeriodicSchedule.builder().startDate(start).endDate(end).frequency(P1M).businessDayAdjustment(BDA).firstRegularStartDate(first).lastRegularEndDate(last).build();
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "generation") public static Object[][] data_generation()
	  public static object[][] data_generation()
	  {
		return new object[][]
		{
			new object[] {JUN_17, SEP_17, P1M, null, null, BDA, null, null, null, list(JUN_17, JUL_17, AUG_17, SEP_17), list(JUN_17, JUL_17, AUG_18, SEP_17), DAY_17},
			new object[] {JUN_17, SEP_17, P1M, STUB_NONE, null, BDA, null, null, null, list(JUN_17, JUL_17, AUG_17, SEP_17), list(JUN_17, JUL_17, AUG_18, SEP_17), DAY_17},
			new object[] {JUN_17, JUL_17, P1M, STUB_NONE, null, BDA, null, null, null, list(JUN_17, JUL_17), list(JUN_17, JUL_17), DAY_17},
			new object[] {JUN_04, SEP_17, P1M, SHORT_INITIAL, null, BDA, null, null, null, list(JUN_04, JUN_17, JUL_17, AUG_17, SEP_17), list(JUN_04, JUN_17, JUL_17, AUG_18, SEP_17), DAY_17},
			new object[] {JUN_17, SEP_17, P1M, SHORT_INITIAL, null, BDA, null, null, null, list(JUN_17, JUL_17, AUG_17, SEP_17), list(JUN_17, JUL_17, AUG_18, SEP_17), DAY_17},
			new object[] {JUN_17, JUL_04, P1M, SHORT_INITIAL, null, BDA, null, null, null, list(JUN_17, JUL_04), list(JUN_17, JUL_04), DAY_4},
			new object[] {date(2011, 6, 28), date(2011, 6, 30), P1M, SHORT_INITIAL, EOM, BDA, null, null, null, list(date(2011, 6, 28), date(2011, 6, 30)), list(date(2011, 6, 28), date(2011, 6, 30)), EOM},
			new object[] {date(2014, 12, 12), date(2015, 8, 24), P3M, SHORT_INITIAL, null, BDA, null, null, null, list(date(2014, 12, 12), date(2015, 2, 24), date(2015, 5, 24), date(2015, 8, 24)), list(date(2014, 12, 12), date(2015, 2, 24), date(2015, 5, 25), date(2015, 8, 24)), DAY_24},
			new object[] {date(2014, 12, 12), date(2015, 8, 24), P3M, SHORT_INITIAL, RollConventions.NONE, BDA, null, null, null, list(date(2014, 12, 12), date(2015, 2, 24), date(2015, 5, 24), date(2015, 8, 24)), list(date(2014, 12, 12), date(2015, 2, 24), date(2015, 5, 25), date(2015, 8, 24)), DAY_24},
			new object[] {date(2014, 11, 24), date(2015, 8, 24), P3M, null, RollConventions.NONE, BDA, null, null, null, list(date(2014, 11, 24), date(2015, 2, 24), date(2015, 5, 24), date(2015, 8, 24)), list(date(2014, 11, 24), date(2015, 2, 24), date(2015, 5, 25), date(2015, 8, 24)), DAY_24},
			new object[] {JUN_04, SEP_17, P1M, LONG_INITIAL, null, BDA, null, null, null, list(JUN_04, JUL_17, AUG_17, SEP_17), list(JUN_04, JUL_17, AUG_18, SEP_17), DAY_17},
			new object[] {JUN_17, SEP_17, P1M, LONG_INITIAL, null, BDA, null, null, null, list(JUN_17, JUL_17, AUG_17, SEP_17), list(JUN_17, JUL_17, AUG_18, SEP_17), DAY_17},
			new object[] {JUN_17, JUL_04, P1M, LONG_INITIAL, null, BDA, null, null, null, list(JUN_17, JUL_04), list(JUN_17, JUL_04), DAY_4},
			new object[] {JUN_17, AUG_04, P1M, LONG_INITIAL, null, BDA, null, null, null, list(JUN_17, AUG_04), list(JUN_17, AUG_04), DAY_4},
			new object[] {JUN_04, SEP_17, P1M, SMART_INITIAL, null, BDA, null, null, null, list(JUN_04, JUN_17, JUL_17, AUG_17, SEP_17), list(JUN_04, JUN_17, JUL_17, AUG_18, SEP_17), DAY_17},
			new object[] {JUN_10, SEP_17, P1M, SMART_INITIAL, null, BDA, null, null, null, list(JUN_10, JUN_17, JUL_17, AUG_17, SEP_17), list(JUN_10, JUN_17, JUL_17, AUG_18, SEP_17), DAY_17},
			new object[] {JUN_11, SEP_17, P1M, SMART_INITIAL, null, BDA, null, null, null, list(JUN_11, JUL_17, AUG_17, SEP_17), list(JUN_11, JUL_17, AUG_18, SEP_17), DAY_17},
			new object[] {JUN_17, JUL_04, P1M, SMART_INITIAL, null, BDA, null, null, null, list(JUN_17, JUL_04), list(JUN_17, JUL_04), DAY_4},
			new object[] {JUN_04, SEP_17, P1M, SHORT_FINAL, null, BDA, null, null, null, list(JUN_04, JUL_04, AUG_04, SEP_04, SEP_17), list(JUN_04, JUL_04, AUG_04, SEP_04, SEP_17), DAY_4},
			new object[] {JUN_17, SEP_17, P1M, SHORT_FINAL, null, BDA, null, null, null, list(JUN_17, JUL_17, AUG_17, SEP_17), list(JUN_17, JUL_17, AUG_18, SEP_17), DAY_17},
			new object[] {JUN_17, JUL_04, P1M, SHORT_FINAL, null, BDA, null, null, null, list(JUN_17, JUL_04), list(JUN_17, JUL_04), DAY_17},
			new object[] {date(2011, 6, 28), date(2011, 6, 30), P1M, SHORT_FINAL, EOM, BDA, null, null, null, list(date(2011, 6, 28), date(2011, 6, 30)), list(date(2011, 6, 28), date(2011, 6, 30)), DAY_28},
			new object[] {date(2014, 11, 29), date(2015, 9, 2), P3M, SHORT_FINAL, null, BDA, null, null, null, list(date(2014, 11, 29), date(2015, 2, 28), date(2015, 5, 29), date(2015, 8, 29), date(2015, 9, 2)), list(date(2014, 11, 28), date(2015, 2, 27), date(2015, 5, 29), date(2015, 8, 31), date(2015, 9, 2)), DAY_29},
			new object[] {date(2014, 11, 29), date(2015, 9, 2), P3M, SHORT_FINAL, RollConventions.NONE, BDA, null, null, null, list(date(2014, 11, 29), date(2015, 2, 28), date(2015, 5, 29), date(2015, 8, 29), date(2015, 9, 2)), list(date(2014, 11, 28), date(2015, 2, 27), date(2015, 5, 29), date(2015, 8, 31), date(2015, 9, 2)), DAY_29},
			new object[] {JUN_04, SEP_17, P1M, LONG_FINAL, null, BDA, null, null, null, list(JUN_04, JUL_04, AUG_04, SEP_17), list(JUN_04, JUL_04, AUG_04, SEP_17), DAY_4},
			new object[] {JUN_17, SEP_17, P1M, LONG_FINAL, null, BDA, null, null, null, list(JUN_17, JUL_17, AUG_17, SEP_17), list(JUN_17, JUL_17, AUG_18, SEP_17), DAY_17},
			new object[] {JUN_17, JUL_04, P1M, LONG_FINAL, null, BDA, null, null, null, list(JUN_17, JUL_04), list(JUN_17, JUL_04), DAY_17},
			new object[] {JUN_17, AUG_04, P1M, LONG_FINAL, null, BDA, null, null, null, list(JUN_17, AUG_04), list(JUN_17, AUG_04), DAY_17},
			new object[] {JUN_04, SEP_17, P1M, SMART_FINAL, null, BDA, null, null, null, list(JUN_04, JUL_04, AUG_04, SEP_04, SEP_17), list(JUN_04, JUL_04, AUG_04, SEP_04, SEP_17), DAY_4},
			new object[] {JUN_04, SEP_11, P1M, SMART_FINAL, null, BDA, null, null, null, list(JUN_04, JUL_04, AUG_04, SEP_04, SEP_11), list(JUN_04, JUL_04, AUG_04, SEP_04, SEP_11), DAY_4},
			new object[] {JUN_04, SEP_10, P1M, SMART_FINAL, null, BDA, null, null, null, list(JUN_04, JUL_04, AUG_04, SEP_10), list(JUN_04, JUL_04, AUG_04, SEP_10), DAY_4},
			new object[] {JUN_17, JUL_04, P1M, SMART_FINAL, null, BDA, null, null, null, list(JUN_17, JUL_04), list(JUN_17, JUL_04), DAY_17},
			new object[] {JUN_04, SEP_17, P1M, null, null, BDA, JUN_17, null, null, list(JUN_04, JUN_17, JUL_17, AUG_17, SEP_17), list(JUN_04, JUN_17, JUL_17, AUG_18, SEP_17), DAY_17},
			new object[] {JUN_04, SEP_17, P1M, SHORT_INITIAL, null, BDA, JUN_17, null, null, list(JUN_04, JUN_17, JUL_17, AUG_17, SEP_17), list(JUN_04, JUN_17, JUL_17, AUG_18, SEP_17), DAY_17},
			new object[] {JUN_17, SEP_17, P1M, null, null, BDA, JUN_17, null, null, list(JUN_17, JUL_17, AUG_17, SEP_17), list(JUN_17, JUL_17, AUG_18, SEP_17), DAY_17},
			new object[] {JUN_04, SEP_17, P1M, null, null, BDA, null, AUG_04, null, list(JUN_04, JUL_04, AUG_04, SEP_17), list(JUN_04, JUL_04, AUG_04, SEP_17), DAY_4},
			new object[] {JUN_04, SEP_17, P1M, SHORT_FINAL, null, BDA, null, AUG_04, null, list(JUN_04, JUL_04, AUG_04, SEP_17), list(JUN_04, JUL_04, AUG_04, SEP_17), DAY_4},
			new object[] {JUN_17, SEP_17, P1M, null, null, BDA, null, AUG_17, null, list(JUN_17, JUL_17, AUG_17, SEP_17), list(JUN_17, JUL_17, AUG_18, SEP_17), DAY_17},
			new object[] {JUN_04, SEP_17, P1M, null, null, BDA, JUL_11, AUG_11, null, list(JUN_04, JUL_11, AUG_11, SEP_17), list(JUN_04, JUL_11, AUG_11, SEP_17), DAY_11},
			new object[] {JUN_04, OCT_17, P1M, STUB_BOTH, null, BDA, JUL_11, SEP_11, null, list(JUN_04, JUL_11, AUG_11, SEP_11, OCT_17), list(JUN_04, JUL_11, AUG_11, SEP_11, OCT_17), DAY_11},
			new object[] {JUN_17, SEP_17, P1M, null, null, BDA, JUN_17, SEP_17, null, list(JUN_17, JUL_17, AUG_17, SEP_17), list(JUN_17, JUL_17, AUG_18, SEP_17), DAY_17},
			new object[] {NOV_30_2013, NOV_30, P3M, STUB_NONE, null, BDA, null, null, null, list(NOV_30_2013, FEB_28, MAY_30, AUG_30, NOV_30), list(NOV_29_2013, FEB_28, MAY_30, AUG_29, NOV_28), DAY_30},
			new object[] {NOV_30_2013, NOV_30, P3M, STUB_NONE, EOM, BDA, null, null, null, list(NOV_30_2013, FEB_28, MAY_31, AUG_31, NOV_30), list(NOV_29_2013, FEB_28, MAY_30, AUG_29, NOV_28), EOM},
			new object[] {MAY_30, NOV_30, P3M, STUB_NONE, EOM, BDA, null, null, null, list(MAY_31, AUG_31, NOV_30), list(MAY_30, AUG_29, NOV_28), EOM},
			new object[] {MAY_30, NOV_30, P3M, null, EOM, BDA, null, null, null, list(MAY_31, AUG_31, NOV_30), list(MAY_30, AUG_29, NOV_28), EOM},
			new object[] {MAY_30, NOV_30, P3M, null, EOM, BDA, null, null, BDA_NONE, list(MAY_31, AUG_31, NOV_30), list(MAY_30, AUG_29, NOV_28), EOM},
			new object[] {MAY_30, NOV_30, P3M, null, DAY_30, BDA, null, null, null, list(MAY_30, AUG_30, NOV_30), list(MAY_30, AUG_29, NOV_28), DAY_30},
			new object[] {JUL_30, OCT_30, P1M, null, EOM, BDA, null, null, null, list(JUL_30, AUG_30, SEP_30, OCT_30), list(JUL_30, AUG_29, SEP_30, OCT_30), DAY_30},
			new object[] {date(2014, 1, 3), SEP_17, P3M, STUB_BOTH, EOM, BDA, FEB_28, AUG_31, null, list(date(2014, 1, 3), FEB_28, MAY_31, AUG_31, SEP_17), list(date(2014, 1, 3), FEB_28, MAY_30, AUG_29, SEP_17), EOM},
			new object[] {NOV_29_2013, NOV_30, P3M, STUB_NONE, EOM, BDA, null, null, BDA_NONE, list(NOV_30_2013, FEB_28, MAY_31, AUG_31, NOV_30), list(NOV_29_2013, FEB_28, MAY_30, AUG_29, NOV_28), EOM},
			new object[] {NOV_29_2013, NOV_30, P3M, null, EOM, BDA, null, null, BDA_NONE, list(NOV_30_2013, FEB_28, MAY_31, AUG_31, NOV_30), list(NOV_29_2013, FEB_28, MAY_30, AUG_29, NOV_28), EOM},
			new object[] {date(2011, 6, 2), date(2011, 8, 31), P1M, SHORT_INITIAL, null, BDA, null, null, null, list(date(2011, 6, 2), date(2011, 6, 30), date(2011, 7, 31), date(2011, 8, 31)), list(date(2011, 6, 2), date(2011, 6, 30), date(2011, 7, 29), date(2011, 8, 31)), EOM},
			new object[] {date(2011, 6, 2), date(2011, 8, 31), P1M, null, null, BDA, date(2011, 6, 30), null, null, list(date(2011, 6, 2), date(2011, 6, 30), date(2011, 7, 31), date(2011, 8, 31)), list(date(2011, 6, 2), date(2011, 6, 30), date(2011, 7, 29), date(2011, 8, 31)), EOM},
			new object[] {date(2011, 7, 31), date(2011, 10, 10), P1M, null, null, BDA, null, date(2011, 9, 30), null, list(date(2011, 7, 31), date(2011, 8, 31), date(2011, 9, 30), date(2011, 10, 10)), list(date(2011, 7, 29), date(2011, 8, 31), date(2011, 9, 30), date(2011, 10, 10)), EOM},
			new object[] {date(2011, 2, 2), date(2011, 5, 30), P1M, null, null, BDA, date(2011, 2, 28), null, null, list(date(2011, 2, 2), date(2011, 2, 28), date(2011, 3, 30), date(2011, 4, 30), date(2011, 5, 30)), list(date(2011, 2, 2), date(2011, 2, 28), date(2011, 3, 30), date(2011, 4, 29), date(2011, 5, 30)), DAY_30},
			new object[] {JUL_17, OCT_17, P1M, null, DAY_17, BDA, null, null, BDA_NONE, list(JUL_17, AUG_17, SEP_17, OCT_17), list(JUL_17, AUG_18, SEP_17, OCT_17), DAY_17},
			new object[] {AUG_18, OCT_17, P1M, null, DAY_17, BDA, null, null, BDA_NONE, list(AUG_17, SEP_17, OCT_17), list(AUG_18, SEP_17, OCT_17), DAY_17},
			new object[] {JUL_11, OCT_17, P1M, null, DAY_17, BDA, AUG_18, null, BDA_NONE, list(JUL_11, AUG_17, SEP_17, OCT_17), list(JUL_11, AUG_18, SEP_17, OCT_17), DAY_17},
			new object[] {JUL_17, OCT_17, P1M, null, DAY_17, BDA, null, AUG_18, BDA_NONE, list(JUL_17, AUG_17, OCT_17), list(JUL_17, AUG_18, OCT_17), DAY_17},
			new object[] {APR_01, OCT_17, P1M, null, DAY_17, BDA, MAY_19, AUG_18, BDA_NONE, list(APR_01, MAY_17, JUN_17, JUL_17, AUG_17, OCT_17), list(APR_01, MAY_19, JUN_17, JUL_17, AUG_18, OCT_17), DAY_17},
			new object[] {JUL_17, AUG_18, P1M, null, DAY_17, BDA, null, null, BDA_NONE, list(JUL_17, AUG_17), list(JUL_17, AUG_18), DAY_17},
			new object[] {JUL_17, AUG_18, P1M, null, DAY_17, BDA, null, null, BDA, list(JUL_17, AUG_17), list(JUL_17, AUG_18), DAY_17},
			new object[] {JUN_04, SEP_17, TERM, STUB_NONE, null, BDA, null, null, null, list(JUN_04, SEP_17), list(JUN_04, SEP_17), ROLL_NONE},
			new object[] {JUN_04, SEP_17, P12M, SHORT_INITIAL, null, BDA, SEP_17, null, null, list(JUN_04, SEP_17), list(JUN_04, SEP_17), DAY_17},
			new object[] {JUN_04, SEP_17, P12M, SHORT_INITIAL, null, BDA, null, JUN_04, null, list(JUN_04, SEP_17), list(JUN_04, SEP_17), DAY_4},
			new object[] {date(2014, 9, 24), date(2016, 11, 24), Frequency.ofYears(2), SHORT_INITIAL, null, BDA, null, null, null, list(date(2014, 9, 24), date(2014, 11, 24), date(2016, 11, 24)), list(date(2014, 9, 24), date(2014, 11, 24), date(2016, 11, 24)), DAY_24},
			new object[] {date(2014, 9, 17), date(2014, 10, 15), P1M, STUB_NONE, IMM, BDA, null, null, null, list(date(2014, 9, 17), date(2014, 10, 15)), list(date(2014, 9, 17), date(2014, 10, 15)), IMM},
			new object[] {date(2014, 9, 17), date(2014, 10, 15), TERM, STUB_NONE, IMM, BDA, null, null, null, list(date(2014, 9, 17), date(2014, 10, 15)), list(date(2014, 9, 17), date(2014, 10, 15)), IMM},
			new object[] {date(2014, 9, 17), date(2014, 10, 15), Frequency.ofDays(2), STUB_NONE, IMM, BDA, null, null, null, list(date(2014, 9, 17), date(2014, 10, 15)), list(date(2014, 9, 17), date(2014, 10, 15)), IMM},
			new object[] {date(2014, 9, 17), date(2014, 10, 1), Frequency.ofDays(2), STUB_NONE, IMM, BDA, null, null, null, list(date(2014, 9, 17), date(2014, 10, 1)), list(date(2014, 9, 17), date(2014, 10, 1)), IMM},
			new object[] {date(2018, 3, 22), date(2020, 0x3, 18), P6M, STUB_NONE, IMM, BDA_JPY_MF, null, null, BDA_NONE, list(date(2018, 3, 21), date(2018, 9, 19), date(2019, 3, 20), date(2019, 9, 18), date(2020, 3, 18)), list(date(2018, 3, 22), date(2018, 9, 19), date(2019, 3, 20), date(2019, 9, 18), date(2020, 3, 18)), IMM},
			new object[] {date(2018, 3, 20), date(2019, 0x3, 20), P6M, STUB_NONE, IMM, BDA_JPY_P, null, null, BDA_NONE, list(date(2018, 3, 21), date(2018, 9, 19), date(2019, 3, 20)), list(date(2018, 3, 20), date(2018, 9, 19), date(2019, 3, 20)), IMM},
			new object[] {date(2018, 3, 22), date(2019, 0x3, 20), P6M, null, IMM, BDA_JPY_MF, null, null, BDA_NONE, list(date(2018, 3, 21), date(2018, 9, 19), date(2019, 3, 20)), list(date(2018, 3, 22), date(2018, 9, 19), date(2019, 3, 20)), IMM},
			new object[] {date(2017, 9, 2), date(2018, 9, 19), P6M, LONG_INITIAL, IMM, BDA_JPY_MF, date(2018, 3, 22), null, BDA_NONE, list(date(2017, 9, 2), date(2018, 3, 21), date(2018, 9, 19)), list(date(2017, 9, 2), date(2018, 3, 22), date(2018, 9, 19)), IMM},
			new object[] {date(2018, 1, 2), date(2018, 9, 19), P6M, null, IMM, BDA_JPY_MF, date(2018, 3, 22), null, BDA_NONE, list(date(2018, 1, 2), date(2018, 3, 21), date(2018, 9, 19)), list(date(2018, 1, 2), date(2018, 3, 22), date(2018, 9, 19)), IMM},
			new object[] {date(2017, 3, 15), date(2018, 5, 19), P6M, null, IMM, BDA_JPY_MF, null, date(2018, 3, 22), BDA_NONE, list(date(2017, 3, 15), date(2017, 9, 20), date(2018, 3, 21), date(2018, 5, 19)), list(date(2017, 3, 15), date(2017, 9, 20), date(2018, 3, 22), date(2018, 5, 21)), IMM},
			new object[] {date(2015, 1, 30), date(2015, 4, 30), P1M, STUB_NONE, DAY_30, BDA, null, null, null, list(date(2015, 1, 30), date(2015, 2, 28), date(2015, 3, 30), date(2015, 4, 30)), list(date(2015, 1, 30), date(2015, 2, 27), date(2015, 3, 30), date(2015, 4, 30)), DAY_30},
			new object[] {date(2015, 2, 28), date(2015, 4, 30), P1M, STUB_NONE, DAY_30, BDA, null, null, null, list(date(2015, 2, 28), date(2015, 3, 30), date(2015, 4, 30)), list(date(2015, 2, 27), date(2015, 3, 30), date(2015, 4, 30)), DAY_30},
			new object[] {date(2015, 2, 28), date(2015, 4, 30), P1M, SHORT_INITIAL, DAY_30, BDA, null, null, null, list(date(2015, 2, 28), date(2015, 3, 30), date(2015, 4, 30)), list(date(2015, 2, 27), date(2015, 3, 30), date(2015, 4, 30)), DAY_30}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "generation") public void test_monthly_schedule(java.time.LocalDate start, java.time.LocalDate end, Frequency freq, StubConvention stubConv, RollConvention rollConv, com.opengamma.strata.basics.date.BusinessDayAdjustment businessDayAdjustment, java.time.LocalDate firstReg, java.time.LocalDate lastReg, com.opengamma.strata.basics.date.BusinessDayAdjustment startBusDayAdjustment, java.util.List<java.time.LocalDate> unadjusted, java.util.List<java.time.LocalDate> adjusted, RollConvention expRoll)
	  public virtual void test_monthly_schedule(LocalDate start, LocalDate end, Frequency freq, StubConvention stubConv, RollConvention rollConv, BusinessDayAdjustment businessDayAdjustment, LocalDate firstReg, LocalDate lastReg, BusinessDayAdjustment startBusDayAdjustment, IList<LocalDate> unadjusted, IList<LocalDate> adjusted, RollConvention expRoll)
	  {

		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(start).endDate(end).frequency(freq).startDateBusinessDayAdjustment(startBusDayAdjustment).businessDayAdjustment(businessDayAdjustment).stubConvention(stubConv).rollConvention(rollConv).firstRegularStartDate(firstReg).lastRegularEndDate(lastReg).build();
		Schedule test = defn.createSchedule(REF_DATA);
		assertEquals(test.size(), unadjusted.Count - 1);
		for (int i = 0; i < test.size(); i++)
		{
		  SchedulePeriod period = test.getPeriod(i);
		  assertEquals(period.UnadjustedStartDate, unadjusted[i]);
		  assertEquals(period.UnadjustedEndDate, unadjusted[i + 1]);
		  assertEquals(period.StartDate, adjusted[i]);
		  assertEquals(period.EndDate, adjusted[i + 1]);
		}
		assertEquals(test.Frequency, freq);
		assertEquals(test.RollConvention, expRoll);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "generation") public void test_monthly_schedule_withOverride(java.time.LocalDate start, java.time.LocalDate end, Frequency freq, StubConvention stubConv, RollConvention rollConv, com.opengamma.strata.basics.date.BusinessDayAdjustment businessDayAdjustment, java.time.LocalDate firstReg, java.time.LocalDate lastReg, com.opengamma.strata.basics.date.BusinessDayAdjustment startBusDayAdjustment, java.util.List<java.time.LocalDate> unadjusted, java.util.List<java.time.LocalDate> adjusted, RollConvention expRoll)
	  public virtual void test_monthly_schedule_withOverride(LocalDate start, LocalDate end, Frequency freq, StubConvention stubConv, RollConvention rollConv, BusinessDayAdjustment businessDayAdjustment, LocalDate firstReg, LocalDate lastReg, BusinessDayAdjustment startBusDayAdjustment, IList<LocalDate> unadjusted, IList<LocalDate> adjusted, RollConvention expRoll)
	  {

		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(start).endDate(end).frequency(freq).startDateBusinessDayAdjustment(startBusDayAdjustment).businessDayAdjustment(businessDayAdjustment).stubConvention(stubConv).rollConvention(rollConv).firstRegularStartDate(firstReg).lastRegularEndDate(lastReg).overrideStartDate(AdjustableDate.of(date(2011, 1, 9), BusinessDayAdjustment.of(FOLLOWING, SAT_SUN))).build();
		Schedule test = defn.createSchedule(REF_DATA);
		assertEquals(test.size(), unadjusted.Count - 1);
		SchedulePeriod period0 = test.getPeriod(0);
		assertEquals(period0.UnadjustedStartDate, date(2011, 1, 9));
		assertEquals(period0.UnadjustedEndDate, unadjusted[1]);
		assertEquals(period0.StartDate, date(2011, 1, 10));
		assertEquals(period0.EndDate, adjusted[1]);
		for (int i = 1; i < test.size(); i++)
		{
		  SchedulePeriod period = test.getPeriod(i);
		  assertEquals(period.UnadjustedStartDate, unadjusted[i]);
		  assertEquals(period.UnadjustedEndDate, unadjusted[i + 1]);
		  assertEquals(period.StartDate, adjusted[i]);
		  assertEquals(period.EndDate, adjusted[i + 1]);
		}
		assertEquals(test.Frequency, freq);
		assertEquals(test.RollConvention, expRoll);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "generation") public void test_monthly_unadjusted(java.time.LocalDate start, java.time.LocalDate end, Frequency freq, StubConvention stubConv, RollConvention rollConv, com.opengamma.strata.basics.date.BusinessDayAdjustment businessDayAdjustment, java.time.LocalDate firstReg, java.time.LocalDate lastReg, com.opengamma.strata.basics.date.BusinessDayAdjustment startBusDayAdjustment, java.util.List<java.time.LocalDate> unadjusted, java.util.List<java.time.LocalDate> adjusted, RollConvention expRoll)
	  public virtual void test_monthly_unadjusted(LocalDate start, LocalDate end, Frequency freq, StubConvention stubConv, RollConvention rollConv, BusinessDayAdjustment businessDayAdjustment, LocalDate firstReg, LocalDate lastReg, BusinessDayAdjustment startBusDayAdjustment, IList<LocalDate> unadjusted, IList<LocalDate> adjusted, RollConvention expRoll)
	  {

		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(start).endDate(end).frequency(freq).startDateBusinessDayAdjustment(startBusDayAdjustment).businessDayAdjustment(businessDayAdjustment).stubConvention(stubConv).rollConvention(rollConv).firstRegularStartDate(firstReg).lastRegularEndDate(lastReg).build();
		ImmutableList<LocalDate> test = defn.createUnadjustedDates(REF_DATA);
		assertEquals(test, unadjusted);
		// createUnadjustedDates() does not work as expected without ReferenceData
		if (startBusDayAdjustment == null && !EOM.Equals(rollConv))
		{
		  ImmutableList<LocalDate> testNoRefData = defn.createUnadjustedDates();
		  assertEquals(testNoRefData, unadjusted);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "generation") public void test_monthly_unadjusted_withOverride(java.time.LocalDate start, java.time.LocalDate end, Frequency freq, StubConvention stubConv, RollConvention rollConv, com.opengamma.strata.basics.date.BusinessDayAdjustment businessDayAdjustment, java.time.LocalDate firstReg, java.time.LocalDate lastReg, com.opengamma.strata.basics.date.BusinessDayAdjustment startBusDayAdjustment, java.util.List<java.time.LocalDate> unadjusted, java.util.List<java.time.LocalDate> adjusted, RollConvention expRoll)
	  public virtual void test_monthly_unadjusted_withOverride(LocalDate start, LocalDate end, Frequency freq, StubConvention stubConv, RollConvention rollConv, BusinessDayAdjustment businessDayAdjustment, LocalDate firstReg, LocalDate lastReg, BusinessDayAdjustment startBusDayAdjustment, IList<LocalDate> unadjusted, IList<LocalDate> adjusted, RollConvention expRoll)
	  {

		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(start).endDate(end).frequency(freq).startDateBusinessDayAdjustment(startBusDayAdjustment).businessDayAdjustment(businessDayAdjustment).stubConvention(stubConv).rollConvention(rollConv).firstRegularStartDate(firstReg).lastRegularEndDate(lastReg).overrideStartDate(AdjustableDate.of(date(2011, 1, 9), BusinessDayAdjustment.of(FOLLOWING, SAT_SUN))).build();
		ImmutableList<LocalDate> test = defn.createUnadjustedDates(REF_DATA);
		assertEquals(test.get(0), date(2011, 1, 9));
		assertEquals(test.subList(1, test.size()), unadjusted.subList(1, test.size()));
		// createUnadjustedDates() does not work as expected without ReferenceData
		if (startBusDayAdjustment == null && !EOM.Equals(rollConv))
		{
		  ImmutableList<LocalDate> testNoRefData = defn.createUnadjustedDates();
		  assertEquals(testNoRefData.get(0), date(2011, 1, 9));
		  assertEquals(testNoRefData.subList(1, testNoRefData.size()), unadjusted.subList(1, testNoRefData.size()));
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "generation") public void test_monthly_adjusted(java.time.LocalDate start, java.time.LocalDate end, Frequency freq, StubConvention stubConv, RollConvention rollConv, com.opengamma.strata.basics.date.BusinessDayAdjustment businessDayAdjustment, java.time.LocalDate firstReg, java.time.LocalDate lastReg, com.opengamma.strata.basics.date.BusinessDayAdjustment startBusDayAdjustment, java.util.List<java.time.LocalDate> unadjusted, java.util.List<java.time.LocalDate> adjusted, RollConvention expRoll)
	  public virtual void test_monthly_adjusted(LocalDate start, LocalDate end, Frequency freq, StubConvention stubConv, RollConvention rollConv, BusinessDayAdjustment businessDayAdjustment, LocalDate firstReg, LocalDate lastReg, BusinessDayAdjustment startBusDayAdjustment, IList<LocalDate> unadjusted, IList<LocalDate> adjusted, RollConvention expRoll)
	  {

		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(start).endDate(end).frequency(freq).startDateBusinessDayAdjustment(startBusDayAdjustment).businessDayAdjustment(businessDayAdjustment).stubConvention(stubConv).rollConvention(rollConv).firstRegularStartDate(firstReg).lastRegularEndDate(lastReg).build();
		ImmutableList<LocalDate> test = defn.createAdjustedDates(REF_DATA);
		assertEquals(test, adjusted);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "generation") public void test_monthly_adjusted_withOverride(java.time.LocalDate start, java.time.LocalDate end, Frequency freq, StubConvention stubConv, RollConvention rollConv, com.opengamma.strata.basics.date.BusinessDayAdjustment businessDayAdjustment, java.time.LocalDate firstReg, java.time.LocalDate lastReg, com.opengamma.strata.basics.date.BusinessDayAdjustment startBusDayAdjustment, java.util.List<java.time.LocalDate> unadjusted, java.util.List<java.time.LocalDate> adjusted, RollConvention expRoll)
	  public virtual void test_monthly_adjusted_withOverride(LocalDate start, LocalDate end, Frequency freq, StubConvention stubConv, RollConvention rollConv, BusinessDayAdjustment businessDayAdjustment, LocalDate firstReg, LocalDate lastReg, BusinessDayAdjustment startBusDayAdjustment, IList<LocalDate> unadjusted, IList<LocalDate> adjusted, RollConvention expRoll)
	  {

		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(start).endDate(end).frequency(freq).startDateBusinessDayAdjustment(startBusDayAdjustment).businessDayAdjustment(businessDayAdjustment).stubConvention(stubConv).rollConvention(rollConv).firstRegularStartDate(firstReg).lastRegularEndDate(lastReg).overrideStartDate(AdjustableDate.of(date(2011, 1, 9), BusinessDayAdjustment.of(FOLLOWING, SAT_SUN))).build();
		ImmutableList<LocalDate> test = defn.createAdjustedDates(REF_DATA);
		assertEquals(test.get(0), date(2011, 1, 10));
		assertEquals(test.subList(1, test.size()), adjusted.subList(1, test.size()));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_override_fallbackWhenStartDateMismatch()
	  {
		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(JUL_04).endDate(SEP_17).overrideStartDate(AdjustableDate.of(JUN_17, BusinessDayAdjustment.of(FOLLOWING, SAT_SUN))).frequency(P1M).businessDayAdjustment(BDA).rollConvention(DAY_17).build();
		Schedule test = defn.createSchedule(REF_DATA);
		assertEquals(test.size(), 3);
		SchedulePeriod period0 = test.getPeriod(0);
		assertEquals(period0.UnadjustedStartDate, JUN_17);
		assertEquals(period0.UnadjustedEndDate, JUL_17);
		assertEquals(period0.StartDate, JUN_17);
		assertEquals(period0.EndDate, JUL_17);
		SchedulePeriod period1 = test.getPeriod(1);
		assertEquals(period1.UnadjustedStartDate, JUL_17);
		assertEquals(period1.UnadjustedEndDate, AUG_17);
		assertEquals(period1.StartDate, JUL_17);
		assertEquals(period1.EndDate, AUG_18);
		SchedulePeriod period2 = test.getPeriod(2);
		assertEquals(period2.UnadjustedStartDate, AUG_17);
		assertEquals(period2.UnadjustedEndDate, SEP_17);
		assertEquals(period2.StartDate, AUG_18);
		assertEquals(period2.EndDate, SEP_17);
	  }

	  public virtual void test_override_fallbackWhenStartDateMismatchEndStub()
	  {
		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(JUL_04).endDate(SEP_04).overrideStartDate(AdjustableDate.of(JUN_17, BusinessDayAdjustment.of(FOLLOWING, SAT_SUN))).frequency(P1M).businessDayAdjustment(BDA).rollConvention(DAY_17).lastRegularEndDate(AUG_17).build();
		Schedule test = defn.createSchedule(REF_DATA);
		assertEquals(test.size(), 3);
		SchedulePeriod period0 = test.getPeriod(0);
		assertEquals(period0.UnadjustedStartDate, JUN_17);
		assertEquals(period0.UnadjustedEndDate, JUL_17);
		assertEquals(period0.StartDate, JUN_17);
		assertEquals(period0.EndDate, JUL_17);
		SchedulePeriod period1 = test.getPeriod(1);
		assertEquals(period1.UnadjustedStartDate, JUL_17);
		assertEquals(period1.UnadjustedEndDate, AUG_17);
		assertEquals(period1.StartDate, JUL_17);
		assertEquals(period1.EndDate, AUG_18);
		SchedulePeriod period2 = test.getPeriod(2);
		assertEquals(period2.UnadjustedStartDate, AUG_17);
		assertEquals(period2.UnadjustedEndDate, SEP_04);
		assertEquals(period2.StartDate, AUG_18);
		assertEquals(period2.EndDate, SEP_04);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_startEndAdjust()
	  {
		BusinessDayAdjustment bda1 = BusinessDayAdjustment.of(PRECEDING, SAT_SUN);
		BusinessDayAdjustment bda2 = BusinessDayAdjustment.of(MODIFIED_PRECEDING, SAT_SUN);
		PeriodicSchedule test = PeriodicSchedule.builder().startDate(date(2014, 10, 4)).endDate(date(2015, 4, 4)).frequency(P3M).businessDayAdjustment(BDA).startDateBusinessDayAdjustment(bda1).endDateBusinessDayAdjustment(bda2).stubConvention(STUB_NONE).build();
		assertEquals(test.calculatedStartDate(), AdjustableDate.of(date(2014, 10, 4), bda1));
		assertEquals(test.calculatedEndDate(), AdjustableDate.of(date(2015, 4, 4), bda2));
		assertEquals(test.createUnadjustedDates(), list(date(2014, 10, 4), date(2015, 1, 4), date(2015, 4, 4)));
		assertEquals(test.createAdjustedDates(REF_DATA), list(date(2014, 10, 3), date(2015, 1, 5), date(2015, 4, 3)));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = ScheduleException.class) public void test_none_badStub()
	  public virtual void test_none_badStub()
	  {
		// Jun 4th to Sep 17th requires a stub, but NONE specified
		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(JUN_04).endDate(SEP_17).frequency(P1M).businessDayAdjustment(BDA).stubConvention(STUB_NONE).rollConvention(DAY_4).firstRegularStartDate(null).lastRegularEndDate(null).build();
		defn.createUnadjustedDates();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = ScheduleException.class) public void test_none_stubDate()
	  public virtual void test_none_stubDate()
	  {
		// Jun 17th to Sep 17th is correct for NONE stub convention, but firstRegularStartDate specified
		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(JUN_17).endDate(SEP_17).frequency(P1M).businessDayAdjustment(BDA).stubConvention(STUB_NONE).rollConvention(DAY_4).firstRegularStartDate(JUL_17).lastRegularEndDate(null).build();
		defn.createUnadjustedDates();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = ScheduleException.class) public void test_both_badStub()
	  public virtual void test_both_badStub()
	  {
		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(JUN_17).endDate(SEP_17).frequency(P1M).businessDayAdjustment(BDA).stubConvention(STUB_BOTH).rollConvention(null).firstRegularStartDate(JUN_17).lastRegularEndDate(SEP_17).build();
		defn.createUnadjustedDates();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = ScheduleException.class) public void test_backwards_badStub()
	  public virtual void test_backwards_badStub()
	  {
		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(JUN_17).endDate(SEP_17).frequency(P1M).businessDayAdjustment(BDA).stubConvention(SHORT_INITIAL).rollConvention(DAY_11).firstRegularStartDate(null).lastRegularEndDate(null).build();
		defn.createUnadjustedDates();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = ScheduleException.class) public void test_forwards_badStub()
	  public virtual void test_forwards_badStub()
	  {
		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(JUN_17).endDate(SEP_17).frequency(P1M).businessDayAdjustment(BDA).stubConvention(SHORT_FINAL).rollConvention(DAY_11).firstRegularStartDate(null).lastRegularEndDate(null).build();
		defn.createUnadjustedDates();
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = ScheduleException.class) public void test_termFrequency_badInitialStub()
	  public virtual void test_termFrequency_badInitialStub()
	  {
		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(JUN_04).endDate(SEP_17).frequency(TERM).businessDayAdjustment(BDA).stubConvention(STUB_NONE).rollConvention(DAY_4).firstRegularStartDate(JUN_17).lastRegularEndDate(null).build();
		defn.createUnadjustedDates();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = ScheduleException.class) public void test_termFrequency_badFinalStub()
	  public virtual void test_termFrequency_badFinalStub()
	  {
		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(JUN_04).endDate(SEP_17).frequency(TERM).businessDayAdjustment(BDA).stubConvention(STUB_NONE).rollConvention(DAY_4).firstRegularStartDate(null).lastRegularEndDate(SEP_04).build();
		defn.createUnadjustedDates();
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_emptyWhenAdjusted_term_createUnadjustedDates()
	  {
		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(date(2015, 5, 29)).endDate(date(2015, 5, 31)).frequency(TERM).businessDayAdjustment(BDA).stubConvention(null).rollConvention(null).firstRegularStartDate(null).lastRegularEndDate(null).build();
		ImmutableList<LocalDate> test = defn.createUnadjustedDates();
		assertEquals(test, list(date(2015, 5, 29), date(2015, 5, 31)));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = ScheduleException.class, expectedExceptionsMessageRegExp = ".*duplicate adjusted dates.*") public void test_emptyWhenAdjusted_term_createAdjustedDates()
	  public virtual void test_emptyWhenAdjusted_term_createAdjustedDates()
	  {
		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(date(2015, 5, 29)).endDate(date(2015, 5, 31)).frequency(TERM).businessDayAdjustment(BDA).stubConvention(null).rollConvention(null).firstRegularStartDate(null).lastRegularEndDate(null).build();
		defn.createAdjustedDates(REF_DATA);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = ScheduleException.class, expectedExceptionsMessageRegExp = ".*duplicate adjusted dates.*") public void test_emptyWhenAdjusted_term_createSchedule()
	  public virtual void test_emptyWhenAdjusted_term_createSchedule()
	  {
		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(date(2015, 5, 29)).endDate(date(2015, 5, 31)).frequency(TERM).businessDayAdjustment(BDA).stubConvention(null).rollConvention(null).firstRegularStartDate(null).lastRegularEndDate(null).build();
		defn.createSchedule(REF_DATA);
	  }

	  public virtual void test_emptyWhenAdjusted_twoPeriods_createUnadjustedDates()
	  {
		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(date(2015, 5, 27)).endDate(date(2015, 5, 31)).frequency(Frequency.ofDays(2)).businessDayAdjustment(BDA).stubConvention(STUB_NONE).rollConvention(null).firstRegularStartDate(null).lastRegularEndDate(null).build();
		ImmutableList<LocalDate> test = defn.createUnadjustedDates();
		assertEquals(test, list(date(2015, 5, 27), date(2015, 5, 29), date(2015, 5, 31)));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = ScheduleException.class, expectedExceptionsMessageRegExp = ".*duplicate adjusted dates.*") public void test_emptyWhenAdjusted_twoPeriods_createAdjustedDates()
	  public virtual void test_emptyWhenAdjusted_twoPeriods_createAdjustedDates()
	  {
		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(date(2015, 5, 27)).endDate(date(2015, 5, 31)).frequency(Frequency.ofDays(2)).businessDayAdjustment(BDA).stubConvention(STUB_NONE).rollConvention(null).firstRegularStartDate(null).lastRegularEndDate(null).build();
		defn.createAdjustedDates(REF_DATA);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = ScheduleException.class, expectedExceptionsMessageRegExp = ".*duplicate adjusted dates.*") public void test_emptyWhenAdjusted_twoPeriods_createSchedule()
	  public virtual void test_emptyWhenAdjusted_twoPeriods_createSchedule()
	  {
		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(date(2015, 5, 27)).endDate(date(2015, 5, 31)).frequency(Frequency.ofDays(2)).businessDayAdjustment(BDA).stubConvention(STUB_NONE).rollConvention(null).firstRegularStartDate(null).lastRegularEndDate(null).build();
		defn.createSchedule(REF_DATA);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = ScheduleException.class, expectedExceptionsMessageRegExp = "Schedule calculation resulted in invalid period") public void test_brokenWhenAdjusted_twoPeriods_createSchedule()
	  public virtual void test_brokenWhenAdjusted_twoPeriods_createSchedule()
	  {
		// generate unadjusted dates that are sorted (Wed, then Fri, then Sun)
		// use weird BusinessDayConvention to move Sunday back to Thursday
		// result is adjusted dates that are not sorted (Wed, then Fri, then Thu)
		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(date(2015, 5, 27)).endDate(date(2015, 5, 31)).frequency(Frequency.ofDays(2)).businessDayAdjustment(BusinessDayAdjustment.of(new BusinessDayConventionAnonymousInnerClass(this)
		   , NO_HOLIDAYS)).stubConvention(STUB_NONE).rollConvention(null).firstRegularStartDate(null).lastRegularEndDate(null).build();
		defn.createSchedule(REF_DATA);
	  }

	  private class BusinessDayConventionAnonymousInnerClass : BusinessDayConvention
	  {
		  private readonly PeriodicScheduleTest outerInstance;

		  public BusinessDayConventionAnonymousInnerClass(PeriodicScheduleTest outerInstance)
		  {
			  this.outerInstance = outerInstance;
		  }

		  public string Name
		  {
			  get
			  {
				return "TestBack3OnSun";
			  }
		  }

		  public LocalDate adjust(LocalDate date, HolidayCalendar calendar)
		  {
			return (date.DayOfWeek == SUNDAY ? date.minusDays(3) : date);
		  }
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = ScheduleException.class, expectedExceptionsMessageRegExp = ".*duplicate unadjusted dates.*") public void test_emptyWhenAdjusted_badRoll_createUnadjustedDates()
	  public virtual void test_emptyWhenAdjusted_badRoll_createUnadjustedDates()
	  {
		RollConvention roll = new RollConventionAnonymousInnerClass(this);
		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(date(2015, 5, 27)).endDate(date(2015, 5, 31)).frequency(Frequency.ofDays(2)).businessDayAdjustment(BDA).stubConvention(STUB_NONE).rollConvention(roll).firstRegularStartDate(null).lastRegularEndDate(null).build();
		defn.createUnadjustedDates();
	  }

	  private class RollConventionAnonymousInnerClass : RollConvention
	  {
		  private readonly PeriodicScheduleTest outerInstance;

		  public RollConventionAnonymousInnerClass(PeriodicScheduleTest outerInstance)
		  {
			  this.outerInstance = outerInstance;
		  }

		  private bool seen;

		  public string Name
		  {
			  get
			  {
				return "Test";
			  }
		  }

		  public LocalDate adjust(LocalDate date)
		  {
			return date;
		  }

		  public LocalDate next(LocalDate date, Frequency frequency)
		  {
			if (seen)
			{
			  return date.plus(frequency);
			}
			else
			{
			  seen = true;
			  return date;
			}
		  }
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "generation") public void coverage_equals(java.time.LocalDate start, java.time.LocalDate end, Frequency freq, StubConvention stubConv, RollConvention rollConv, com.opengamma.strata.basics.date.BusinessDayAdjustment busDayAdjustment, java.time.LocalDate firstReg, java.time.LocalDate lastReg, com.opengamma.strata.basics.date.BusinessDayAdjustment startBusDayAdjustment, java.util.List<java.time.LocalDate> unadjusted, java.util.List<java.time.LocalDate> adjusted, RollConvention expRoll)
	  public virtual void coverage_equals(LocalDate start, LocalDate end, Frequency freq, StubConvention stubConv, RollConvention rollConv, BusinessDayAdjustment busDayAdjustment, LocalDate firstReg, LocalDate lastReg, BusinessDayAdjustment startBusDayAdjustment, IList<LocalDate> unadjusted, IList<LocalDate> adjusted, RollConvention expRoll)
	  {

		PeriodicSchedule a1 = of(start, end, freq, busDayAdjustment, stubConv, rollConv, firstReg, lastReg, null, null, null);
		PeriodicSchedule a2 = of(start, end, freq, busDayAdjustment, stubConv, rollConv, firstReg, lastReg, null, null, null);
		PeriodicSchedule b = of(LocalDate.MIN, end, freq, busDayAdjustment, stubConv, rollConv, firstReg, lastReg, null, null, null);
		PeriodicSchedule c = of(start, LocalDate.MAX, freq, busDayAdjustment, stubConv, rollConv, firstReg, lastReg, null, null, null);
		PeriodicSchedule d = of(start, end, freq == P1M ? P3M : P1M, busDayAdjustment, stubConv, rollConv, firstReg, lastReg, null, null, null);
		PeriodicSchedule e = of(start, end, freq, BDA_NONE, stubConv, rollConv, firstReg, lastReg, null, null, null);
		PeriodicSchedule f = of(start, end, freq, busDayAdjustment, stubConv == STUB_NONE ? SHORT_FINAL : STUB_NONE, rollConv, firstReg, lastReg, null, null, null);
		PeriodicSchedule g = of(start, end, freq, busDayAdjustment, stubConv, SFE, firstReg, lastReg, null, null, null);
		PeriodicSchedule h = of(start, end, freq, busDayAdjustment, stubConv, rollConv, start.plusDays(1), null, null, null, null);
		PeriodicSchedule i = of(start, end, freq, busDayAdjustment, stubConv, rollConv, null, end.minusDays(1), null, null, null);
		PeriodicSchedule j = of(start, end, freq, busDayAdjustment, stubConv, rollConv, firstReg, lastReg, BDA, null, null);
		PeriodicSchedule k = of(start, end, freq, busDayAdjustment, stubConv, rollConv, firstReg, lastReg, null, BDA, null);
		PeriodicSchedule m = of(start, end, freq, busDayAdjustment, stubConv, rollConv, firstReg, lastReg, null, null, AdjustableDate.of(start.minusDays(1)));
		assertEquals(a1.Equals(a1), true);
		assertEquals(a1.Equals(a2), true);
		assertEquals(a1.Equals(b), false);
		assertEquals(a1.Equals(c), false);
		assertEquals(a1.Equals(d), false);
		assertEquals(a1.Equals(e), false);
		assertEquals(a1.Equals(f), false);
		assertEquals(a1.Equals(g), false);
		assertEquals(a1.Equals(h), false);
		assertEquals(a1.Equals(i), false);
		assertEquals(a1.Equals(j), false);
		assertEquals(a1.Equals(k), false);
		assertEquals(a1.Equals(m), false);
	  }

	  private PeriodicSchedule of(LocalDate start, LocalDate end, Frequency freq, BusinessDayAdjustment bda, StubConvention stubConv, RollConvention rollConv, LocalDate firstReg, LocalDate lastReg, BusinessDayAdjustment startBda, BusinessDayAdjustment endBda, AdjustableDate overrideStartDate)
	  {
		return PeriodicSchedule.builder().startDate(start).endDate(end).frequency(freq).businessDayAdjustment(bda).startDateBusinessDayAdjustment(startBda).endDateBusinessDayAdjustment(endBda).stubConvention(stubConv).rollConvention(rollConv).firstRegularStartDate(firstReg).lastRegularEndDate(lastReg).overrideStartDate(overrideStartDate).build();
	  }

	  public virtual void coverage_builder()
	  {
		PeriodicSchedule test = PeriodicSchedule.builder().startDate(JUL_17).endDate(SEP_17).frequency(P2M).businessDayAdjustment(BDA_NONE).startDateBusinessDayAdjustment(BDA_NONE).endDateBusinessDayAdjustment(BDA_NONE).stubConvention(STUB_NONE).rollConvention(EOM).firstRegularStartDate(JUL_17).lastRegularEndDate(SEP_17).overrideStartDate(AdjustableDate.of(JUL_11)).build();
		assertEquals(test.StartDate, JUL_17);
		assertEquals(test.EndDate, SEP_17);
		assertEquals(test.calculatedStartDate(), AdjustableDate.of(JUL_11, BDA_NONE));
		assertEquals(test.calculatedEndDate(), AdjustableDate.of(SEP_17, BDA_NONE));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		BusinessDayAdjustment bda = BusinessDayAdjustment.of(FOLLOWING, SAT_SUN);
		PeriodicSchedule defn = PeriodicSchedule.of(date(2014, JUNE, 4), date(2014, SEPTEMBER, 17), P1M, bda, SHORT_INITIAL, false);
		coverImmutableBean(defn);
	  }

	  public virtual void test_serialization()
	  {
		BusinessDayAdjustment bda = BusinessDayAdjustment.of(FOLLOWING, SAT_SUN);
		PeriodicSchedule defn = PeriodicSchedule.of(date(2014, JUNE, 4), date(2014, SEPTEMBER, 17), P1M, bda, SHORT_INITIAL, false);
		assertSerialization(defn);
	  }

	}

}