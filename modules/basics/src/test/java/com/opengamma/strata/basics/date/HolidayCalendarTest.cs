/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertSame;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertThrows;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;

	/// <summary>
	/// Test <seealso cref="HolidayCalendar"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class HolidayCalendarTest
	public class HolidayCalendarTest
	{

	  private static readonly LocalDate MON_2014_07_07 = LocalDate.of(2014, 7, 7);
	  private static readonly LocalDate WED_2014_07_09 = LocalDate.of(2014, 7, 9);
	  private static readonly LocalDate THU_2014_07_10 = LocalDate.of(2014, 7, 10);
	  private static readonly LocalDate FRI_2014_07_11 = LocalDate.of(2014, 7, 11);
	  private static readonly LocalDate SAT_2014_07_12 = LocalDate.of(2014, 7, 12);
	  private static readonly LocalDate SUN_2014_07_13 = LocalDate.of(2014, 7, 13);
	  private static readonly LocalDate MON_2014_07_14 = LocalDate.of(2014, 7, 14);
	  private static readonly LocalDate TUE_2014_07_15 = LocalDate.of(2014, 7, 15);
	  private static readonly LocalDate WED_2014_07_16 = LocalDate.of(2014, 7, 16);
	  private static readonly LocalDate THU_2014_07_17 = LocalDate.of(2014, 7, 17);
	  private static readonly LocalDate FRI_2014_07_18 = LocalDate.of(2014, 7, 18);
	  private static readonly LocalDate SAT_2014_07_19 = LocalDate.of(2014, 7, 19);
	  private static readonly LocalDate SUN_2014_07_20 = LocalDate.of(2014, 7, 20);
	  private static readonly LocalDate MON_2014_07_21 = LocalDate.of(2014, 7, 21);
	  private static readonly LocalDate TUE_2014_07_22 = LocalDate.of(2014, 7, 22);
	  private static readonly LocalDate WED_2014_07_23 = LocalDate.of(2014, 7, 23);

	  private static readonly LocalDate WED_2014_07_30 = LocalDate.of(2014, 7, 30);
	  private static readonly LocalDate THU_2014_07_31 = LocalDate.of(2014, 7, 31);
	  private const object ANOTHER_TYPE = "";

	  //-------------------------------------------------------------------------
	  public virtual void test_NO_HOLIDAYS()
	  {
		HolidayCalendar test = HolidayCalendars.NO_HOLIDAYS;
		LocalDateUtils.stream(LocalDate.of(2011, 1, 1), LocalDate.of(2015, 1, 31)).forEach(date =>
		{
		assertEquals(test.isBusinessDay(date), true);
		assertEquals(test.isHoliday(date), false);
		});
		assertEquals(test.Name, "NoHolidays");
		assertEquals(test.ToString(), "HolidayCalendar[NoHolidays]");
	  }

	  public virtual void test_NO_HOLIDAYS_of()
	  {
		HolidayCalendar test = HolidayCalendars.of("NoHolidays");
		assertEquals(test, HolidayCalendars.NO_HOLIDAYS);
	  }

	  public virtual void test_NO_HOLIDAYS_shift()
	  {
		assertEquals(HolidayCalendars.NO_HOLIDAYS.shift(FRI_2014_07_11, 2), SUN_2014_07_13);
		assertEquals(HolidayCalendars.NO_HOLIDAYS.shift(SUN_2014_07_13, -2), FRI_2014_07_11);
	  }

	  public virtual void test_NO_HOLIDAYS_next()
	  {
		assertEquals(HolidayCalendars.NO_HOLIDAYS.next(FRI_2014_07_11), SAT_2014_07_12);
		assertEquals(HolidayCalendars.NO_HOLIDAYS.next(SAT_2014_07_12), SUN_2014_07_13);
	  }

	  public virtual void test_NO_HOLIDAYS_nextOrSame()
	  {
		assertEquals(HolidayCalendars.NO_HOLIDAYS.nextOrSame(FRI_2014_07_11), FRI_2014_07_11);
		assertEquals(HolidayCalendars.NO_HOLIDAYS.nextOrSame(SAT_2014_07_12), SAT_2014_07_12);
	  }

	  public virtual void test_NO_HOLIDAYS_previous()
	  {
		assertEquals(HolidayCalendars.NO_HOLIDAYS.previous(SAT_2014_07_12), FRI_2014_07_11);
		assertEquals(HolidayCalendars.NO_HOLIDAYS.previous(SUN_2014_07_13), SAT_2014_07_12);
	  }

	  public virtual void test_NO_HOLIDAYS_previousOrSame()
	  {
		assertEquals(HolidayCalendars.NO_HOLIDAYS.previousOrSame(SAT_2014_07_12), SAT_2014_07_12);
		assertEquals(HolidayCalendars.NO_HOLIDAYS.previousOrSame(SUN_2014_07_13), SUN_2014_07_13);
	  }

	  public virtual void test_NO_HOLIDAYS_nextSameOrLastInMonth()
	  {
		assertEquals(HolidayCalendars.NO_HOLIDAYS.nextSameOrLastInMonth(FRI_2014_07_11), FRI_2014_07_11);
		assertEquals(HolidayCalendars.NO_HOLIDAYS.nextSameOrLastInMonth(SAT_2014_07_12), SAT_2014_07_12);
	  }

	  public virtual void test_NO_HOLIDAYS_daysBetween_LocalDateLocalDate()
	  {
		assertEquals(HolidayCalendars.NO_HOLIDAYS.daysBetween(FRI_2014_07_11, MON_2014_07_14), 3);
	  }

	  public virtual void test_NO_HOLIDAYS_combineWith()
	  {
		HolidayCalendar @base = new MockHolCal();
		HolidayCalendar test = HolidayCalendars.NO_HOLIDAYS.combinedWith(@base);
		assertSame(test, @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_SAT_SUN()
	  {
		HolidayCalendar test = HolidayCalendars.SAT_SUN;
		LocalDateUtils.stream(LocalDate.of(2011, 1, 1), LocalDate.of(2015, 1, 31)).forEach(date =>
		{
		bool isBusinessDay = date.DayOfWeek != SATURDAY && date.DayOfWeek != SUNDAY;
		assertEquals(test.isBusinessDay(date), isBusinessDay);
		assertEquals(test.isHoliday(date), !isBusinessDay);
		});
		assertEquals(test.Name, "Sat/Sun");
		assertEquals(test.ToString(), "HolidayCalendar[Sat/Sun]");
	  }

	  public virtual void test_SAT_SUN_of()
	  {
		HolidayCalendar test = HolidayCalendars.of("Sat/Sun");
		assertEquals(test, HolidayCalendars.SAT_SUN);
	  }

	  public virtual void test_SAT_SUN_shift()
	  {
		ImmutableHolidayCalendar equivalent = ImmutableHolidayCalendar.of(HolidayCalendarId.of("TEST-SAT-SUN"), ImmutableList.of(), ImmutableList.of(SATURDAY, SUNDAY));
		assertSatSun(equivalent);
		assertSatSun(HolidayCalendars.SAT_SUN);
	  }

	  private void assertSatSun(HolidayCalendar test)
	  {
		assertEquals(test.shift(THU_2014_07_10, 2), MON_2014_07_14);
		assertEquals(test.shift(FRI_2014_07_11, 2), TUE_2014_07_15);
		assertEquals(test.shift(SUN_2014_07_13, 2), TUE_2014_07_15);
		assertEquals(test.shift(MON_2014_07_14, 2), WED_2014_07_16);

		assertEquals(test.shift(FRI_2014_07_11, -2), WED_2014_07_09);
		assertEquals(test.shift(SAT_2014_07_12, -2), THU_2014_07_10);
		assertEquals(test.shift(SUN_2014_07_13, -2), THU_2014_07_10);
		assertEquals(test.shift(MON_2014_07_14, -2), THU_2014_07_10);
		assertEquals(test.shift(TUE_2014_07_15, -2), FRI_2014_07_11);
		assertEquals(test.shift(WED_2014_07_16, -2), MON_2014_07_14);

		assertEquals(test.shift(FRI_2014_07_11, 5), FRI_2014_07_18);
		assertEquals(test.shift(FRI_2014_07_11, 6), MON_2014_07_21);

		assertEquals(test.shift(FRI_2014_07_18, -5), FRI_2014_07_11);
		assertEquals(test.shift(MON_2014_07_21, -6), FRI_2014_07_11);

		assertEquals(test.shift(SAT_2014_07_12, 5), FRI_2014_07_18);
		assertEquals(test.shift(SAT_2014_07_12, -5), MON_2014_07_07);
	  }

	  public virtual void test_SAT_SUN_next()
	  {
		assertEquals(HolidayCalendars.SAT_SUN.next(THU_2014_07_10), FRI_2014_07_11);
		assertEquals(HolidayCalendars.SAT_SUN.next(FRI_2014_07_11), MON_2014_07_14);
		assertEquals(HolidayCalendars.SAT_SUN.next(SAT_2014_07_12), MON_2014_07_14);
		assertEquals(HolidayCalendars.SAT_SUN.next(SAT_2014_07_12), MON_2014_07_14);
	  }

	  public virtual void test_SAT_SUN_previous()
	  {
		assertEquals(HolidayCalendars.SAT_SUN.previous(SAT_2014_07_12), FRI_2014_07_11);
		assertEquals(HolidayCalendars.SAT_SUN.previous(SUN_2014_07_13), FRI_2014_07_11);
		assertEquals(HolidayCalendars.SAT_SUN.previous(MON_2014_07_14), FRI_2014_07_11);
		assertEquals(HolidayCalendars.SAT_SUN.previous(TUE_2014_07_15), MON_2014_07_14);
	  }

	  public virtual void test_SAT_SUN_daysBetween_LocalDateLocalDate()
	  {
		assertEquals(HolidayCalendars.SAT_SUN.daysBetween(FRI_2014_07_11, MON_2014_07_14), 1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_FRI_SAT()
	  {
		HolidayCalendar test = HolidayCalendars.FRI_SAT;
		LocalDateUtils.stream(LocalDate.of(2011, 1, 1), LocalDate.of(2015, 1, 31)).forEach(date =>
		{
		bool isBusinessDay = date.DayOfWeek != FRIDAY && date.DayOfWeek != SATURDAY;
		assertEquals(test.isBusinessDay(date), isBusinessDay);
		assertEquals(test.isHoliday(date), !isBusinessDay);
		});
		assertEquals(test.Name, "Fri/Sat");
		assertEquals(test.ToString(), "HolidayCalendar[Fri/Sat]");
	  }

	  public virtual void test_FRI_SAT_of()
	  {
		HolidayCalendar test = HolidayCalendars.of("Fri/Sat");
		assertEquals(test, HolidayCalendars.FRI_SAT);
	  }

	  public virtual void test_FRI_SAT_shift()
	  {
		assertEquals(HolidayCalendars.FRI_SAT.shift(THU_2014_07_10, 2), MON_2014_07_14);
		assertEquals(HolidayCalendars.FRI_SAT.shift(FRI_2014_07_11, 2), MON_2014_07_14);
		assertEquals(HolidayCalendars.FRI_SAT.shift(SUN_2014_07_13, 2), TUE_2014_07_15);
		assertEquals(HolidayCalendars.FRI_SAT.shift(MON_2014_07_14, 2), WED_2014_07_16);

		assertEquals(HolidayCalendars.FRI_SAT.shift(FRI_2014_07_11, -2), WED_2014_07_09);
		assertEquals(HolidayCalendars.FRI_SAT.shift(SAT_2014_07_12, -2), WED_2014_07_09);
		assertEquals(HolidayCalendars.FRI_SAT.shift(SUN_2014_07_13, -2), WED_2014_07_09);
		assertEquals(HolidayCalendars.FRI_SAT.shift(MON_2014_07_14, -2), THU_2014_07_10);
		assertEquals(HolidayCalendars.FRI_SAT.shift(TUE_2014_07_15, -2), SUN_2014_07_13);
		assertEquals(HolidayCalendars.FRI_SAT.shift(WED_2014_07_16, -2), MON_2014_07_14);

		assertEquals(HolidayCalendars.FRI_SAT.shift(THU_2014_07_10, 5), THU_2014_07_17);
		assertEquals(HolidayCalendars.FRI_SAT.shift(THU_2014_07_10, 6), SUN_2014_07_20);

		assertEquals(HolidayCalendars.FRI_SAT.shift(THU_2014_07_17, -5), THU_2014_07_10);
		assertEquals(HolidayCalendars.FRI_SAT.shift(SUN_2014_07_20, -6), THU_2014_07_10);
	  }

	  public virtual void test_FRI_SAT_next()
	  {
		assertEquals(HolidayCalendars.FRI_SAT.next(WED_2014_07_09), THU_2014_07_10);
		assertEquals(HolidayCalendars.FRI_SAT.next(THU_2014_07_10), SUN_2014_07_13);
		assertEquals(HolidayCalendars.FRI_SAT.next(FRI_2014_07_11), SUN_2014_07_13);
		assertEquals(HolidayCalendars.FRI_SAT.next(SAT_2014_07_12), SUN_2014_07_13);
		assertEquals(HolidayCalendars.FRI_SAT.next(SUN_2014_07_13), MON_2014_07_14);
	  }

	  public virtual void test_FRI_SAT_previous()
	  {
		assertEquals(HolidayCalendars.FRI_SAT.previous(FRI_2014_07_11), THU_2014_07_10);
		assertEquals(HolidayCalendars.FRI_SAT.previous(SAT_2014_07_12), THU_2014_07_10);
		assertEquals(HolidayCalendars.FRI_SAT.previous(SUN_2014_07_13), THU_2014_07_10);
		assertEquals(HolidayCalendars.FRI_SAT.previous(MON_2014_07_14), SUN_2014_07_13);
	  }

	  public virtual void test_FRI_SAT_daysBetween_LocalDateLocalDate()
	  {
		assertEquals(HolidayCalendars.FRI_SAT.daysBetween(FRI_2014_07_11, MON_2014_07_14), 1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_THU_FRI()
	  {
		HolidayCalendar test = HolidayCalendars.THU_FRI;
		LocalDateUtils.stream(LocalDate.of(2011, 1, 1), LocalDate.of(2015, 1, 31)).forEach(date =>
		{
		bool isBusinessDay = date.DayOfWeek != THURSDAY && date.DayOfWeek != FRIDAY;
		assertEquals(test.isBusinessDay(date), isBusinessDay);
		assertEquals(test.isHoliday(date), !isBusinessDay);
		});
		assertEquals(test.Name, "Thu/Fri");
		assertEquals(test.ToString(), "HolidayCalendar[Thu/Fri]");
	  }

	  public virtual void test_THU_FRI_of()
	  {
		HolidayCalendar test = HolidayCalendars.of("Thu/Fri");
		assertEquals(test, HolidayCalendars.THU_FRI);
	  }

	  public virtual void test_THU_FRI_shift()
	  {
		assertEquals(HolidayCalendars.THU_FRI.shift(WED_2014_07_09, 2), SUN_2014_07_13);
		assertEquals(HolidayCalendars.THU_FRI.shift(THU_2014_07_10, 2), SUN_2014_07_13);
		assertEquals(HolidayCalendars.THU_FRI.shift(FRI_2014_07_11, 2), SUN_2014_07_13);
		assertEquals(HolidayCalendars.THU_FRI.shift(SAT_2014_07_12, 2), MON_2014_07_14);

		assertEquals(HolidayCalendars.THU_FRI.shift(FRI_2014_07_18, -2), TUE_2014_07_15);
		assertEquals(HolidayCalendars.THU_FRI.shift(SAT_2014_07_19, -2), TUE_2014_07_15);
		assertEquals(HolidayCalendars.THU_FRI.shift(SUN_2014_07_20, -2), WED_2014_07_16);
		assertEquals(HolidayCalendars.THU_FRI.shift(MON_2014_07_21, -2), SAT_2014_07_19);

		assertEquals(HolidayCalendars.THU_FRI.shift(WED_2014_07_09, 5), WED_2014_07_16);
		assertEquals(HolidayCalendars.THU_FRI.shift(WED_2014_07_09, 6), SAT_2014_07_19);

		assertEquals(HolidayCalendars.THU_FRI.shift(WED_2014_07_16, -5), WED_2014_07_09);
		assertEquals(HolidayCalendars.THU_FRI.shift(SAT_2014_07_19, -6), WED_2014_07_09);
	  }

	  public virtual void test_THU_FRI_next()
	  {
		assertEquals(HolidayCalendars.THU_FRI.next(WED_2014_07_09), SAT_2014_07_12);
		assertEquals(HolidayCalendars.THU_FRI.next(THU_2014_07_10), SAT_2014_07_12);
		assertEquals(HolidayCalendars.THU_FRI.next(FRI_2014_07_11), SAT_2014_07_12);
		assertEquals(HolidayCalendars.THU_FRI.next(SAT_2014_07_12), SUN_2014_07_13);
	  }

	  public virtual void test_THU_FRI_previous()
	  {
		assertEquals(HolidayCalendars.THU_FRI.previous(THU_2014_07_10), WED_2014_07_09);
		assertEquals(HolidayCalendars.THU_FRI.previous(FRI_2014_07_11), WED_2014_07_09);
		assertEquals(HolidayCalendars.THU_FRI.previous(SAT_2014_07_12), WED_2014_07_09);
		assertEquals(HolidayCalendars.THU_FRI.previous(SUN_2014_07_13), SAT_2014_07_12);
	  }

	  public virtual void test_THU_FRI_daysBetween_LocalDateLocalDate()
	  {
		assertEquals(HolidayCalendars.THU_FRI.daysBetween(FRI_2014_07_11, MON_2014_07_14), 2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_combined()
	  {
		HolidayCalendar test = HolidayCalendars.of("Thu/Fri+Fri/Sat");
		assertEquals(test.Name, "Fri/Sat+Thu/Fri");
		assertEquals(test.ToString(), "HolidayCalendar[Fri/Sat+Thu/Fri]");

		HolidayCalendar test2 = HolidayCalendars.of("Thu/Fri+Fri/Sat");
		assertEquals(test, test2);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "shift") public static Object[][] data_shift()
	  public static object[][] data_shift()
	  {
		return new object[][]
		{
			new object[] {THU_2014_07_10, 1, FRI_2014_07_11},
			new object[] {FRI_2014_07_11, 1, MON_2014_07_14},
			new object[] {SAT_2014_07_12, 1, MON_2014_07_14},
			new object[] {SUN_2014_07_13, 1, MON_2014_07_14},
			new object[] {MON_2014_07_14, 1, TUE_2014_07_15},
			new object[] {TUE_2014_07_15, 1, THU_2014_07_17},
			new object[] {WED_2014_07_16, 1, THU_2014_07_17},
			new object[] {THU_2014_07_17, 1, MON_2014_07_21},
			new object[] {FRI_2014_07_18, 1, MON_2014_07_21},
			new object[] {SAT_2014_07_19, 1, MON_2014_07_21},
			new object[] {SUN_2014_07_20, 1, MON_2014_07_21},
			new object[] {MON_2014_07_21, 1, TUE_2014_07_22},
			new object[] {THU_2014_07_10, 2, MON_2014_07_14},
			new object[] {FRI_2014_07_11, 2, TUE_2014_07_15},
			new object[] {SAT_2014_07_12, 2, TUE_2014_07_15},
			new object[] {SUN_2014_07_13, 2, TUE_2014_07_15},
			new object[] {MON_2014_07_14, 2, THU_2014_07_17},
			new object[] {TUE_2014_07_15, 2, MON_2014_07_21},
			new object[] {WED_2014_07_16, 2, MON_2014_07_21},
			new object[] {THU_2014_07_17, 2, TUE_2014_07_22},
			new object[] {FRI_2014_07_18, 2, TUE_2014_07_22},
			new object[] {SAT_2014_07_19, 2, TUE_2014_07_22},
			new object[] {SUN_2014_07_20, 2, TUE_2014_07_22},
			new object[] {MON_2014_07_21, 2, WED_2014_07_23},
			new object[] {THU_2014_07_10, 0, THU_2014_07_10},
			new object[] {FRI_2014_07_11, 0, FRI_2014_07_11},
			new object[] {SAT_2014_07_12, 0, SAT_2014_07_12},
			new object[] {SUN_2014_07_13, 0, SUN_2014_07_13},
			new object[] {MON_2014_07_14, 0, MON_2014_07_14},
			new object[] {TUE_2014_07_15, 0, TUE_2014_07_15},
			new object[] {WED_2014_07_16, 0, WED_2014_07_16},
			new object[] {THU_2014_07_17, 0, THU_2014_07_17},
			new object[] {FRI_2014_07_18, 0, FRI_2014_07_18},
			new object[] {SAT_2014_07_19, 0, SAT_2014_07_19},
			new object[] {SUN_2014_07_20, 0, SUN_2014_07_20},
			new object[] {MON_2014_07_21, 0, MON_2014_07_21},
			new object[] {FRI_2014_07_11, -1, THU_2014_07_10},
			new object[] {SAT_2014_07_12, -1, FRI_2014_07_11},
			new object[] {SUN_2014_07_13, -1, FRI_2014_07_11},
			new object[] {MON_2014_07_14, -1, FRI_2014_07_11},
			new object[] {TUE_2014_07_15, -1, MON_2014_07_14},
			new object[] {WED_2014_07_16, -1, TUE_2014_07_15},
			new object[] {THU_2014_07_17, -1, TUE_2014_07_15},
			new object[] {FRI_2014_07_18, -1, THU_2014_07_17},
			new object[] {SAT_2014_07_19, -1, THU_2014_07_17},
			new object[] {SUN_2014_07_20, -1, THU_2014_07_17},
			new object[] {MON_2014_07_21, -1, THU_2014_07_17},
			new object[] {TUE_2014_07_22, -1, MON_2014_07_21},
			new object[] {FRI_2014_07_11, -2, WED_2014_07_09},
			new object[] {SAT_2014_07_12, -2, THU_2014_07_10},
			new object[] {SUN_2014_07_13, -2, THU_2014_07_10},
			new object[] {MON_2014_07_14, -2, THU_2014_07_10},
			new object[] {TUE_2014_07_15, -2, FRI_2014_07_11},
			new object[] {WED_2014_07_16, -2, MON_2014_07_14},
			new object[] {THU_2014_07_17, -2, MON_2014_07_14},
			new object[] {FRI_2014_07_18, -2, TUE_2014_07_15},
			new object[] {SAT_2014_07_19, -2, TUE_2014_07_15},
			new object[] {SUN_2014_07_20, -2, TUE_2014_07_15},
			new object[] {MON_2014_07_21, -2, TUE_2014_07_15},
			new object[] {TUE_2014_07_22, -2, THU_2014_07_17}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "shift") public void test_shift(java.time.LocalDate date, int amount, java.time.LocalDate expected)
	  public virtual void test_shift(LocalDate date, int amount, LocalDate expected)
	  {
		// 16th, 18th, Sat, Sun
		HolidayCalendar test = new MockHolCal();
		assertEquals(test.shift(date, amount), expected);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "shift") public void test_adjustBy(java.time.LocalDate date, int amount, java.time.LocalDate expected)
	  public virtual void test_adjustBy(LocalDate date, int amount, LocalDate expected)
	  {
		HolidayCalendar test = new MockHolCal();
		assertEquals(date.with(test.adjustBy(amount)), expected);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "next") public static Object[][] data_next()
	  public static object[][] data_next()
	  {
		return new object[][]
		{
			new object[] {THU_2014_07_10, FRI_2014_07_11},
			new object[] {FRI_2014_07_11, MON_2014_07_14},
			new object[] {SAT_2014_07_12, MON_2014_07_14},
			new object[] {SUN_2014_07_13, MON_2014_07_14},
			new object[] {MON_2014_07_14, TUE_2014_07_15},
			new object[] {TUE_2014_07_15, THU_2014_07_17},
			new object[] {WED_2014_07_16, THU_2014_07_17},
			new object[] {THU_2014_07_17, MON_2014_07_21},
			new object[] {FRI_2014_07_18, MON_2014_07_21},
			new object[] {SAT_2014_07_19, MON_2014_07_21},
			new object[] {SUN_2014_07_20, MON_2014_07_21},
			new object[] {MON_2014_07_21, TUE_2014_07_22}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "next") public void test_next(java.time.LocalDate date, java.time.LocalDate expectedNext)
	  public virtual void test_next(LocalDate date, LocalDate expectedNext)
	  {
		HolidayCalendar test = new MockHolCal();
		assertEquals(test.next(date), expectedNext);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "nextOrSame") public static Object[][] data_nextOrSame()
	  public static object[][] data_nextOrSame()
	  {
		return new object[][]
		{
			new object[] {THU_2014_07_10, THU_2014_07_10},
			new object[] {FRI_2014_07_11, FRI_2014_07_11},
			new object[] {SAT_2014_07_12, MON_2014_07_14},
			new object[] {SUN_2014_07_13, MON_2014_07_14},
			new object[] {MON_2014_07_14, MON_2014_07_14},
			new object[] {TUE_2014_07_15, TUE_2014_07_15},
			new object[] {WED_2014_07_16, THU_2014_07_17},
			new object[] {THU_2014_07_17, THU_2014_07_17},
			new object[] {FRI_2014_07_18, MON_2014_07_21},
			new object[] {SAT_2014_07_19, MON_2014_07_21},
			new object[] {SUN_2014_07_20, MON_2014_07_21},
			new object[] {MON_2014_07_21, MON_2014_07_21}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "nextOrSame") public void test_nextOrSame(java.time.LocalDate date, java.time.LocalDate expectedNext)
	  public virtual void test_nextOrSame(LocalDate date, LocalDate expectedNext)
	  {
		HolidayCalendar test = new MockHolCal();
		assertEquals(test.nextOrSame(date), expectedNext);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "previous") public static Object[][] data_previous()
	  public static object[][] data_previous()
	  {
		return new object[][]
		{
			new object[] {FRI_2014_07_11, THU_2014_07_10},
			new object[] {SAT_2014_07_12, FRI_2014_07_11},
			new object[] {SUN_2014_07_13, FRI_2014_07_11},
			new object[] {MON_2014_07_14, FRI_2014_07_11},
			new object[] {TUE_2014_07_15, MON_2014_07_14},
			new object[] {WED_2014_07_16, TUE_2014_07_15},
			new object[] {THU_2014_07_17, TUE_2014_07_15},
			new object[] {FRI_2014_07_18, THU_2014_07_17},
			new object[] {SAT_2014_07_19, THU_2014_07_17},
			new object[] {SUN_2014_07_20, THU_2014_07_17},
			new object[] {MON_2014_07_21, THU_2014_07_17},
			new object[] {TUE_2014_07_22, MON_2014_07_21}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "previous") public void test_previous(java.time.LocalDate date, java.time.LocalDate expectedPrevious)
	  public virtual void test_previous(LocalDate date, LocalDate expectedPrevious)
	  {
		HolidayCalendar test = new MockHolCal();
		assertEquals(test.previous(date), expectedPrevious);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "previousOrSame") public static Object[][] data_previousOrSame()
	  public static object[][] data_previousOrSame()
	  {
		return new object[][]
		{
			new object[] {FRI_2014_07_11, FRI_2014_07_11},
			new object[] {SAT_2014_07_12, FRI_2014_07_11},
			new object[] {SUN_2014_07_13, FRI_2014_07_11},
			new object[] {MON_2014_07_14, MON_2014_07_14},
			new object[] {TUE_2014_07_15, TUE_2014_07_15},
			new object[] {WED_2014_07_16, TUE_2014_07_15},
			new object[] {THU_2014_07_17, THU_2014_07_17},
			new object[] {FRI_2014_07_18, THU_2014_07_17},
			new object[] {SAT_2014_07_19, THU_2014_07_17},
			new object[] {SUN_2014_07_20, THU_2014_07_17},
			new object[] {MON_2014_07_21, MON_2014_07_21},
			new object[] {TUE_2014_07_22, TUE_2014_07_22}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "previousOrSame") public void test_previousOrSame(java.time.LocalDate date, java.time.LocalDate expectedPrevious)
	  public virtual void test_previousOrSame(LocalDate date, LocalDate expectedPrevious)
	  {
		HolidayCalendar test = new MockHolCal();
		assertEquals(test.previousOrSame(date), expectedPrevious);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "nextSameOrLastInMonth") public static Object[][] data_nextSameOrLastInMonth()
	  public static object[][] data_nextSameOrLastInMonth()
	  {
		return new object[][]
		{
			new object[] {THU_2014_07_10, THU_2014_07_10},
			new object[] {FRI_2014_07_11, FRI_2014_07_11},
			new object[] {SAT_2014_07_12, MON_2014_07_14},
			new object[] {SUN_2014_07_13, MON_2014_07_14},
			new object[] {MON_2014_07_14, MON_2014_07_14},
			new object[] {TUE_2014_07_15, TUE_2014_07_15},
			new object[] {WED_2014_07_16, THU_2014_07_17},
			new object[] {THU_2014_07_17, THU_2014_07_17},
			new object[] {FRI_2014_07_18, MON_2014_07_21},
			new object[] {SAT_2014_07_19, MON_2014_07_21},
			new object[] {SUN_2014_07_20, MON_2014_07_21},
			new object[] {MON_2014_07_21, MON_2014_07_21},
			new object[] {THU_2014_07_31, WED_2014_07_30}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "nextSameOrLastInMonth") public void test_nextLastOrSame(java.time.LocalDate date, java.time.LocalDate expectedNext)
	  public virtual void test_nextLastOrSame(LocalDate date, LocalDate expectedNext)
	  {
		// mock calendar has Sat/Sun plus 16th, 18th and 31st as holidays
		HolidayCalendar test = new MockHolCal();
		assertEquals(test.nextSameOrLastInMonth(date), expectedNext);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "lastBusinessDayOfMonth") public static Object[][] data_lastBusinessDayOfMonth()
	  public static object[][] data_lastBusinessDayOfMonth()
	  {
		return new object[][]
		{
			new object[] {date(2014, 6, 26), date(2014, 6, 27)},
			new object[] {date(2014, 6, 27), date(2014, 6, 27)},
			new object[] {date(2014, 6, 28), date(2014, 6, 27)},
			new object[] {date(2014, 6, 29), date(2014, 6, 27)},
			new object[] {date(2014, 6, 30), date(2014, 6, 27)},
			new object[] {date(2014, 7, 29), date(2014, 7, 30)},
			new object[] {date(2014, 7, 30), date(2014, 7, 30)},
			new object[] {date(2014, 7, 31), date(2014, 7, 30)},
			new object[] {date(2014, 8, 28), date(2014, 8, 29)},
			new object[] {date(2014, 8, 29), date(2014, 8, 29)},
			new object[] {date(2014, 8, 30), date(2014, 8, 29)},
			new object[] {date(2014, 8, 31), date(2014, 8, 29)},
			new object[] {date(2014, 9, 28), date(2014, 9, 30)},
			new object[] {date(2014, 9, 29), date(2014, 9, 30)},
			new object[] {date(2014, 9, 30), date(2014, 9, 30)}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "lastBusinessDayOfMonth") public void test_lastBusinessDayOfMonth(java.time.LocalDate date, java.time.LocalDate expectedEom)
	  public virtual void test_lastBusinessDayOfMonth(LocalDate date, LocalDate expectedEom)
	  {
		HolidayCalendar test = new MockEomHolCal();
		assertEquals(test.lastBusinessDayOfMonth(date), expectedEom);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "lastBusinessDayOfMonth") public void test_isLastBusinessDayOfMonth(java.time.LocalDate date, java.time.LocalDate expectedEom)
	  public virtual void test_isLastBusinessDayOfMonth(LocalDate date, LocalDate expectedEom)
	  {
		HolidayCalendar test = new MockEomHolCal();
		assertEquals(test.isLastBusinessDayOfMonth(date), date.Equals(expectedEom));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "daysBetween") public static Object[][] data_daysBetween()
	  public static object[][] data_daysBetween()
	  {
		return new object[][]
		{
			new object[] {FRI_2014_07_11, FRI_2014_07_11, 0},
			new object[] {FRI_2014_07_11, SAT_2014_07_12, 1},
			new object[] {FRI_2014_07_11, SUN_2014_07_13, 1},
			new object[] {FRI_2014_07_11, MON_2014_07_14, 1},
			new object[] {FRI_2014_07_11, TUE_2014_07_15, 2},
			new object[] {FRI_2014_07_11, WED_2014_07_16, 3},
			new object[] {FRI_2014_07_11, THU_2014_07_17, 3},
			new object[] {FRI_2014_07_11, FRI_2014_07_18, 4},
			new object[] {FRI_2014_07_11, SAT_2014_07_19, 4},
			new object[] {FRI_2014_07_11, SUN_2014_07_20, 4},
			new object[] {FRI_2014_07_11, MON_2014_07_21, 4},
			new object[] {FRI_2014_07_11, TUE_2014_07_22, 5}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "daysBetween") public void test_daysBetween_LocalDateLocalDate(java.time.LocalDate start, java.time.LocalDate end, int expected)
	  public virtual void test_daysBetween_LocalDateLocalDate(LocalDate start, LocalDate end, int expected)
	  {
		HolidayCalendar test = new MockHolCal();
		assertEquals(test.daysBetween(start, end), expected);
	  }

	  public virtual void test_daysBetween_LocalDateLocalDate_endBeforeStart()
	  {
		HolidayCalendar test = new MockHolCal();
		assertThrows(typeof(System.ArgumentException), () => test.daysBetween(TUE_2014_07_15, MON_2014_07_14));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "businessDays") public static Object[][] data_businessDays()
	  public static object[][] data_businessDays()
	  {
		return new object[][]
		{
			new object[] {FRI_2014_07_11, FRI_2014_07_11, ImmutableList.of()},
			new object[] {FRI_2014_07_11, SAT_2014_07_12, ImmutableList.of(FRI_2014_07_11)},
			new object[] {FRI_2014_07_11, SUN_2014_07_13, ImmutableList.of(FRI_2014_07_11)},
			new object[] {FRI_2014_07_11, MON_2014_07_14, ImmutableList.of(FRI_2014_07_11)},
			new object[] {FRI_2014_07_11, TUE_2014_07_15, ImmutableList.of(FRI_2014_07_11, MON_2014_07_14)},
			new object[] {FRI_2014_07_11, WED_2014_07_16, ImmutableList.of(FRI_2014_07_11, MON_2014_07_14, TUE_2014_07_15)},
			new object[] {FRI_2014_07_11, THU_2014_07_17, ImmutableList.of(FRI_2014_07_11, MON_2014_07_14, TUE_2014_07_15)},
			new object[] {FRI_2014_07_11, FRI_2014_07_18, ImmutableList.of(FRI_2014_07_11, MON_2014_07_14, TUE_2014_07_15, THU_2014_07_17)},
			new object[] {FRI_2014_07_11, SAT_2014_07_19, ImmutableList.of(FRI_2014_07_11, MON_2014_07_14, TUE_2014_07_15, THU_2014_07_17)},
			new object[] {FRI_2014_07_11, SUN_2014_07_20, ImmutableList.of(FRI_2014_07_11, MON_2014_07_14, TUE_2014_07_15, THU_2014_07_17)},
			new object[] {FRI_2014_07_11, MON_2014_07_21, ImmutableList.of(FRI_2014_07_11, MON_2014_07_14, TUE_2014_07_15, THU_2014_07_17)},
			new object[] {FRI_2014_07_11, TUE_2014_07_22, ImmutableList.of(FRI_2014_07_11, MON_2014_07_14, TUE_2014_07_15, THU_2014_07_17, MON_2014_07_21)}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "businessDays") public void test_businessDays_LocalDateLocalDate(java.time.LocalDate start, java.time.LocalDate end, com.google.common.collect.ImmutableList<java.time.LocalDate> expected)
	  public virtual void test_businessDays_LocalDateLocalDate(LocalDate start, LocalDate end, ImmutableList<LocalDate> expected)
	  {
		HolidayCalendar test = new MockHolCal();
		assertEquals(test.businessDays(start, end).collect(toImmutableList()), expected);
	  }

	  public virtual void test_businessDays_LocalDateLocalDate_endBeforeStart()
	  {
		HolidayCalendar test = new MockHolCal();
		assertThrows(typeof(System.ArgumentException), () => test.businessDays(TUE_2014_07_15, MON_2014_07_14));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "holidays") public static Object[][] data_holidays()
	  public static object[][] data_holidays()
	  {
		return new object[][]
		{
			new object[] {FRI_2014_07_11, FRI_2014_07_11, ImmutableList.of()},
			new object[] {FRI_2014_07_11, SAT_2014_07_12, ImmutableList.of()},
			new object[] {FRI_2014_07_11, SUN_2014_07_13, ImmutableList.of(SAT_2014_07_12)},
			new object[] {FRI_2014_07_11, MON_2014_07_14, ImmutableList.of(SAT_2014_07_12, SUN_2014_07_13)},
			new object[] {FRI_2014_07_11, TUE_2014_07_15, ImmutableList.of(SAT_2014_07_12, SUN_2014_07_13)},
			new object[] {FRI_2014_07_11, WED_2014_07_16, ImmutableList.of(SAT_2014_07_12, SUN_2014_07_13)},
			new object[] {FRI_2014_07_11, THU_2014_07_17, ImmutableList.of(SAT_2014_07_12, SUN_2014_07_13, WED_2014_07_16)},
			new object[] {FRI_2014_07_11, FRI_2014_07_18, ImmutableList.of(SAT_2014_07_12, SUN_2014_07_13, WED_2014_07_16)},
			new object[] {FRI_2014_07_11, SAT_2014_07_19, ImmutableList.of(SAT_2014_07_12, SUN_2014_07_13, WED_2014_07_16, FRI_2014_07_18)},
			new object[] {FRI_2014_07_11, SUN_2014_07_20, ImmutableList.of(SAT_2014_07_12, SUN_2014_07_13, WED_2014_07_16, FRI_2014_07_18, SAT_2014_07_19)},
			new object[] {FRI_2014_07_11, MON_2014_07_21, ImmutableList.of(SAT_2014_07_12, SUN_2014_07_13, WED_2014_07_16, FRI_2014_07_18, SAT_2014_07_19, SUN_2014_07_20)},
			new object[] {FRI_2014_07_11, TUE_2014_07_22, ImmutableList.of(SAT_2014_07_12, SUN_2014_07_13, WED_2014_07_16, FRI_2014_07_18, SAT_2014_07_19, SUN_2014_07_20)}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "holidays") public void test_holidays_LocalDateLocalDate(java.time.LocalDate start, java.time.LocalDate end, com.google.common.collect.ImmutableList<java.time.LocalDate> expected)
	  public virtual void test_holidays_LocalDateLocalDate(LocalDate start, LocalDate end, ImmutableList<LocalDate> expected)
	  {
		HolidayCalendar test = new MockHolCal();
		assertEquals(test.holidays(start, end).collect(toImmutableList()), expected);
	  }

	  public virtual void test_holidays_LocalDateLocalDate_endBeforeStart()
	  {
		HolidayCalendar test = new MockHolCal();
		assertThrows(typeof(System.ArgumentException), () => test.holidays(TUE_2014_07_15, MON_2014_07_14));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combinedWith()
	  {
		HolidayCalendar base1 = new MockHolCal();
		HolidayCalendar base2 = HolidayCalendars.FRI_SAT;
		HolidayCalendar test = base1.combinedWith(base2);
		assertEquals(test.ToString(), "HolidayCalendar[Fri/Sat+Mock]");
		assertEquals(test.Name, "Fri/Sat+Mock");
		assertEquals(test.Equals(base1.combinedWith(base2)), true);
		assertEquals(test.Equals(ANOTHER_TYPE), false);
		assertEquals(test.Equals(null), false);
		assertEquals(test.GetHashCode(), base1.combinedWith(base2).GetHashCode());

		assertEquals(test.isHoliday(THU_2014_07_10), false);
		assertEquals(test.isHoliday(FRI_2014_07_11), true);
		assertEquals(test.isHoliday(SAT_2014_07_12), true);
		assertEquals(test.isHoliday(SUN_2014_07_13), true);
		assertEquals(test.isHoliday(MON_2014_07_14), false);
		assertEquals(test.isHoliday(TUE_2014_07_15), false);
		assertEquals(test.isHoliday(WED_2014_07_16), true);
		assertEquals(test.isHoliday(THU_2014_07_17), false);
		assertEquals(test.isHoliday(FRI_2014_07_18), true);
		assertEquals(test.isHoliday(SAT_2014_07_19), true);
		assertEquals(test.isHoliday(SUN_2014_07_20), true);
		assertEquals(test.isHoliday(MON_2014_07_21), false);
	  }

	  public virtual void test_combineWith_same()
	  {
		HolidayCalendar @base = new MockHolCal();
		HolidayCalendar test = @base.combinedWith(@base);
		assertSame(test, @base);
	  }

	  public virtual void test_combineWith_none()
	  {
		HolidayCalendar @base = new MockHolCal();
		HolidayCalendar test = @base.combinedWith(HolidayCalendars.NO_HOLIDAYS);
		assertSame(test, @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_extendedEnum()
	  {
		assertEquals(HolidayCalendars.extendedEnum().lookupAll().get("NoHolidays"), HolidayCalendars.NO_HOLIDAYS);
	  }

	  //-------------------------------------------------------------------------
	  internal class MockHolCal : HolidayCalendar
	  {
		public virtual bool isHoliday(LocalDate date)
		{
		  return date.DayOfMonth == 16 || date.DayOfMonth == 18 || date.DayOfMonth == 31 || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		}

		public virtual HolidayCalendarId Id
		{
			get
			{
			  return HolidayCalendarId.of("Mock");
			}
		}
	  }

	  internal class MockEomHolCal : HolidayCalendar
	  {
		public virtual bool isHoliday(LocalDate date)
		{
		  return (date.Month == JUNE && date.DayOfMonth == 30) || (date.Month == JULY && date.DayOfMonth == 31) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		}

		public virtual HolidayCalendarId Id
		{
			get
			{
			  return HolidayCalendarId.of("MockEom");
			}
		}
	  }

	}

}