using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsRuntime;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertSame;


	using JodaBeanSer = org.joda.beans.ser.JodaBeanSer;
	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ImmutableSortedSet = com.google.common.collect.ImmutableSortedSet;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;

	/// <summary>
	/// Test <seealso cref="ImmutableHolidayCalendar"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ImmutableHolidayCalendarTest
	public class ImmutableHolidayCalendarTest
	{

	  private static readonly HolidayCalendarId TEST_ID = HolidayCalendarId.of("Test1");
	  private static readonly HolidayCalendarId TEST_ID2 = HolidayCalendarId.of("Test2");

	  private static readonly LocalDate MON_2014_06_30 = LocalDate.of(2014, 6, 30);
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
	  private static readonly LocalDate MON_2014_12_29 = LocalDate.of(2014, 12, 29);
	  private static readonly LocalDate TUE_2014_12_30 = LocalDate.of(2014, 12, 30);
	  private static readonly LocalDate WED_2014_12_31 = LocalDate.of(2014, 12, 31);

	  private static readonly LocalDate THU_2015_01_01 = LocalDate.of(2015, 1, 1);
	  private static readonly LocalDate FRI_2015_01_02 = LocalDate.of(2015, 1, 2);
	  private static readonly LocalDate SAT_2015_01_03 = LocalDate.of(2015, 1, 3);
	  private static readonly LocalDate MON_2015_01_05 = LocalDate.of(2015, 1, 5);
	  private static readonly LocalDate FRI_2015_02_27 = LocalDate.of(2015, 2, 27);
	  private static readonly LocalDate SAT_2015_02_28 = LocalDate.of(2015, 2, 28);
	  private static readonly LocalDate SAT_2015_03_28 = LocalDate.of(2015, 3, 28);
	  private static readonly LocalDate SUN_2015_03_29 = LocalDate.of(2015, 3, 29);
	  private static readonly LocalDate MON_2015_03_30 = LocalDate.of(2015, 3, 30);
	  private static readonly LocalDate TUE_2015_03_31 = LocalDate.of(2015, 3, 31);
	  private static readonly LocalDate WED_2015_04_01 = LocalDate.of(2015, 4, 1);
	  private static readonly LocalDate THU_2015_04_02 = LocalDate.of(2015, 4, 2);

	  private static readonly LocalDate SAT_2018_07_14 = LocalDate.of(2018, 7, 14);
	  private static readonly LocalDate SUN_2018_07_15 = LocalDate.of(2018, 7, 15);
	  private static readonly LocalDate MON_2018_07_16 = LocalDate.of(2018, 7, 16);
	  private static readonly LocalDate TUE_2018_07_17 = LocalDate.of(2018, 7, 17);
	  private static readonly LocalDate WED_2018_07_18 = LocalDate.of(2018, 7, 18);

	  private static readonly ImmutableHolidayCalendar HOLCAL_MON_WED = ImmutableHolidayCalendar.of(TEST_ID, ImmutableList.of(MON_2014_07_14, WED_2014_07_16), SATURDAY, SUNDAY);

	  private static readonly ImmutableHolidayCalendar HOLCAL_YEAR_END = ImmutableHolidayCalendar.of(HolidayCalendarId.of("TestYearEnd"), ImmutableList.of(TUE_2014_12_30, THU_2015_01_01), SATURDAY, SUNDAY);

	  private static readonly ImmutableHolidayCalendar HOLCAL_SAT_SUN = ImmutableHolidayCalendar.of(HolidayCalendarId.of("TestSatSun"), ImmutableList.of(), SATURDAY, SUNDAY);

	  private static readonly ImmutableHolidayCalendar HOLCAL_END_MONTH = ImmutableHolidayCalendar.of(HolidayCalendarId.of("TestEndOfMonth"), ImmutableList.of(MON_2014_06_30, THU_2014_07_31), SATURDAY, SUNDAY);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_IterableDayOfWeekDayOfWeek_null()
	  {
		IEnumerable<LocalDate> holidays = Arrays.asList(MON_2014_07_14, FRI_2014_07_18);
		assertThrowsRuntime(() => ImmutableHolidayCalendar.of(null, holidays, SATURDAY, SUNDAY));
		assertThrowsRuntime(() => ImmutableHolidayCalendar.of(TEST_ID, null, SATURDAY, SUNDAY));
		assertThrowsRuntime(() => ImmutableHolidayCalendar.of(TEST_ID, holidays, null, SUNDAY));
		assertThrowsRuntime(() => ImmutableHolidayCalendar.of(TEST_ID, holidays, SATURDAY, null));
	  }

	  public virtual void test_of_IterableIterable_null()
	  {
		IEnumerable<LocalDate> holidays = Arrays.asList(MON_2014_07_14, FRI_2014_07_18);
		IEnumerable<DayOfWeek> weekendDays = Arrays.asList(THURSDAY, FRIDAY);
		assertThrowsRuntime(() => ImmutableHolidayCalendar.of(null, holidays, weekendDays));
		assertThrowsRuntime(() => ImmutableHolidayCalendar.of(TEST_ID, null, weekendDays));
		assertThrowsRuntime(() => ImmutableHolidayCalendar.of(TEST_ID, holidays, null));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "createSatSunWeekend") public static Object[][] data_createSatSunWeekend()
	  public static object[][] data_createSatSunWeekend()
	  {
		return new object[][]
		{
			new object[] {FRI_2014_07_11, true},
			new object[] {SAT_2014_07_12, false},
			new object[] {SUN_2014_07_13, false},
			new object[] {MON_2014_07_14, false},
			new object[] {TUE_2014_07_15, true},
			new object[] {WED_2014_07_16, true},
			new object[] {THU_2014_07_17, true},
			new object[] {FRI_2014_07_18, false},
			new object[] {SAT_2014_07_19, false},
			new object[] {SUN_2014_07_20, false},
			new object[] {MON_2014_07_21, true}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "createSatSunWeekend") public void test_of_IterableDayOfWeekDayOfWeek_satSunWeekend(java.time.LocalDate date, boolean isBusinessDay)
	  public virtual void test_of_IterableDayOfWeekDayOfWeek_satSunWeekend(LocalDate date, bool isBusinessDay)
	  {
		IEnumerable<LocalDate> holidays = Arrays.asList(MON_2014_07_14, FRI_2014_07_18);
		ImmutableHolidayCalendar test = ImmutableHolidayCalendar.of(TEST_ID, holidays, SATURDAY, SUNDAY);
		assertEquals(test.isBusinessDay(date), isBusinessDay);
		assertEquals(test.isHoliday(date), !isBusinessDay);
		assertEquals(test.Holidays, ImmutableSortedSet.copyOf(holidays));
		assertEquals(test.WeekendDays, ImmutableSet.of(SATURDAY, SUNDAY));
		assertEquals(test.ToString(), "HolidayCalendar[" + TEST_ID.Name + "]");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "createSatSunWeekend") public void test_of_IterableIterable_satSunWeekend(java.time.LocalDate date, boolean isBusinessDay)
	  public virtual void test_of_IterableIterable_satSunWeekend(LocalDate date, bool isBusinessDay)
	  {
		IEnumerable<LocalDate> holidays = Arrays.asList(MON_2014_07_14, FRI_2014_07_18);
		IEnumerable<DayOfWeek> weekendDays = Arrays.asList(SATURDAY, SUNDAY);
		ImmutableHolidayCalendar test = ImmutableHolidayCalendar.of(TEST_ID, holidays, weekendDays);
		assertEquals(test.isBusinessDay(date), isBusinessDay);
		assertEquals(test.isHoliday(date), !isBusinessDay);
		assertEquals(test.Holidays, ImmutableSortedSet.copyOf(holidays));
		assertEquals(test.WeekendDays, ImmutableSet.of(SATURDAY, SUNDAY));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "createThuFriWeekend") public static Object[][] data_createThuFriWeekend()
	  public static object[][] data_createThuFriWeekend()
	  {
		return new object[][]
		{
			new object[] {FRI_2014_07_11, false},
			new object[] {SAT_2014_07_12, true},
			new object[] {SUN_2014_07_13, true},
			new object[] {MON_2014_07_14, false},
			new object[] {TUE_2014_07_15, true},
			new object[] {WED_2014_07_16, true},
			new object[] {THU_2014_07_17, false},
			new object[] {FRI_2014_07_18, false},
			new object[] {SAT_2014_07_19, false},
			new object[] {SUN_2014_07_20, true},
			new object[] {MON_2014_07_21, true}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "createThuFriWeekend") public void test_of_IterableDayOfWeekDayOfWeek_thuFriWeekend(java.time.LocalDate date, boolean isBusinessDay)
	  public virtual void test_of_IterableDayOfWeekDayOfWeek_thuFriWeekend(LocalDate date, bool isBusinessDay)
	  {
		IEnumerable<LocalDate> holidays = Arrays.asList(MON_2014_07_14, SAT_2014_07_19);
		ImmutableHolidayCalendar test = ImmutableHolidayCalendar.of(TEST_ID, holidays, THURSDAY, FRIDAY);
		assertEquals(test.isBusinessDay(date), isBusinessDay);
		assertEquals(test.isHoliday(date), !isBusinessDay);
		assertEquals(test.Holidays, ImmutableSortedSet.copyOf(holidays));
		assertEquals(test.WeekendDays, ImmutableSet.of(THURSDAY, FRIDAY));
		assertEquals(test.ToString(), "HolidayCalendar[" + TEST_ID.Name + "]");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "createThuFriWeekend") public void test_of_IterableIterable_thuFriWeekend(java.time.LocalDate date, boolean isBusinessDay)
	  public virtual void test_of_IterableIterable_thuFriWeekend(LocalDate date, bool isBusinessDay)
	  {
		IEnumerable<LocalDate> holidays = Arrays.asList(MON_2014_07_14, SAT_2014_07_19);
		IEnumerable<DayOfWeek> weekendDays = Arrays.asList(THURSDAY, FRIDAY);
		ImmutableHolidayCalendar test = ImmutableHolidayCalendar.of(TEST_ID, holidays, weekendDays);
		assertEquals(test.isBusinessDay(date), isBusinessDay);
		assertEquals(test.isHoliday(date), !isBusinessDay);
		assertEquals(test.Holidays, ImmutableSortedSet.copyOf(holidays));
		assertEquals(test.WeekendDays, ImmutableSet.of(THURSDAY, FRIDAY));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "createSunWeekend") public static Object[][] data_createSunWeekend()
	  public static object[][] data_createSunWeekend()
	  {
		return new object[][]
		{
			new object[] {FRI_2014_07_11, true},
			new object[] {SAT_2014_07_12, true},
			new object[] {SUN_2014_07_13, false},
			new object[] {MON_2014_07_14, false},
			new object[] {TUE_2014_07_15, true},
			new object[] {WED_2014_07_16, true},
			new object[] {THU_2014_07_17, false},
			new object[] {FRI_2014_07_18, true},
			new object[] {SAT_2014_07_19, true},
			new object[] {SUN_2014_07_20, false},
			new object[] {MON_2014_07_21, true}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "createSunWeekend") public void test_of_IterableDayOfWeekDayOfWeek_sunWeekend(java.time.LocalDate date, boolean isBusinessDay)
	  public virtual void test_of_IterableDayOfWeekDayOfWeek_sunWeekend(LocalDate date, bool isBusinessDay)
	  {
		IEnumerable<LocalDate> holidays = Arrays.asList(MON_2014_07_14, THU_2014_07_17);
		ImmutableHolidayCalendar test = ImmutableHolidayCalendar.of(TEST_ID, holidays, SUNDAY, SUNDAY);
		assertEquals(test.isBusinessDay(date), isBusinessDay);
		assertEquals(test.isHoliday(date), !isBusinessDay);
		assertEquals(test.Holidays, ImmutableSortedSet.copyOf(holidays));
		assertEquals(test.WeekendDays, ImmutableSet.of(SUNDAY));
		assertEquals(test.ToString(), "HolidayCalendar[" + TEST_ID.Name + "]");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "createSunWeekend") public void test_of_IterableIterable_sunWeekend(java.time.LocalDate date, boolean isBusinessDay)
	  public virtual void test_of_IterableIterable_sunWeekend(LocalDate date, bool isBusinessDay)
	  {
		IEnumerable<LocalDate> holidays = Arrays.asList(MON_2014_07_14, THU_2014_07_17);
		IEnumerable<DayOfWeek> weekendDays = Arrays.asList(SUNDAY);
		ImmutableHolidayCalendar test = ImmutableHolidayCalendar.of(TEST_ID, holidays, weekendDays);
		assertEquals(test.isBusinessDay(date), isBusinessDay);
		assertEquals(test.isHoliday(date), !isBusinessDay);
		assertEquals(test.Holidays, ImmutableSortedSet.copyOf(holidays));
		assertEquals(test.WeekendDays, ImmutableSet.of(SUNDAY));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "createThuFriSatWeekend") public static Object[][] data_createThuFriSatWeekend()
	  public static object[][] data_createThuFriSatWeekend()
	  {
		return new object[][]
		{
			new object[] {FRI_2014_07_11, false},
			new object[] {SAT_2014_07_12, false},
			new object[] {SUN_2014_07_13, true},
			new object[] {MON_2014_07_14, false},
			new object[] {TUE_2014_07_15, false},
			new object[] {WED_2014_07_16, true},
			new object[] {THU_2014_07_17, false},
			new object[] {FRI_2014_07_18, false},
			new object[] {SAT_2014_07_19, false},
			new object[] {SUN_2014_07_20, true},
			new object[] {MON_2014_07_21, true}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "createThuFriSatWeekend") public void test_of_IterableIterable_thuFriSatWeekend(java.time.LocalDate date, boolean isBusinessDay)
	  public virtual void test_of_IterableIterable_thuFriSatWeekend(LocalDate date, bool isBusinessDay)
	  {
		IEnumerable<LocalDate> holidays = Arrays.asList(MON_2014_07_14, TUE_2014_07_15);
		IEnumerable<DayOfWeek> weekendDays = Arrays.asList(THURSDAY, FRIDAY, SATURDAY);
		ImmutableHolidayCalendar test = ImmutableHolidayCalendar.of(TEST_ID, holidays, weekendDays);
		assertEquals(test.isBusinessDay(date), isBusinessDay);
		assertEquals(test.isHoliday(date), !isBusinessDay);
		assertEquals(test.Holidays, ImmutableSortedSet.copyOf(holidays));
		assertEquals(test.WeekendDays, ImmutableSet.of(THURSDAY, FRIDAY, SATURDAY));
		assertEquals(test.ToString(), "HolidayCalendar[" + TEST_ID.Name + "]");
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "createNoWeekends") public static Object[][] data_createNoWeekends()
	  public static object[][] data_createNoWeekends()
	  {
		return new object[][]
		{
			new object[] {FRI_2014_07_11, true},
			new object[] {SAT_2014_07_12, true},
			new object[] {SUN_2014_07_13, true},
			new object[] {MON_2014_07_14, false},
			new object[] {TUE_2014_07_15, true},
			new object[] {WED_2014_07_16, true},
			new object[] {THU_2014_07_17, true},
			new object[] {FRI_2014_07_18, false},
			new object[] {SAT_2014_07_19, true},
			new object[] {SUN_2014_07_20, true},
			new object[] {MON_2014_07_21, true}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "createNoWeekends") public void test_of_IterableIterable_noWeekends(java.time.LocalDate date, boolean isBusinessDay)
	  public virtual void test_of_IterableIterable_noWeekends(LocalDate date, bool isBusinessDay)
	  {
		IEnumerable<LocalDate> holidays = Arrays.asList(MON_2014_07_14, FRI_2014_07_18);
		IEnumerable<DayOfWeek> weekendDays = Arrays.asList();
		ImmutableHolidayCalendar test = ImmutableHolidayCalendar.of(TEST_ID, holidays, weekendDays);
		assertEquals(test.isBusinessDay(date), isBusinessDay);
		assertEquals(test.isHoliday(date), !isBusinessDay);
		assertEquals(test.Holidays, ImmutableSortedSet.copyOf(holidays));
		assertEquals(test.WeekendDays, ImmutableSet.of());
		assertEquals(test.ToString(), "HolidayCalendar[" + TEST_ID.Name + "]");
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "createNoHolidays") public static Object[][] data_createNoHolidays()
	  public static object[][] data_createNoHolidays()
	  {
		return new object[][]
		{
			new object[] {FRI_2014_07_11, false},
			new object[] {SAT_2014_07_12, false},
			new object[] {SUN_2014_07_13, true},
			new object[] {MON_2014_07_14, true},
			new object[] {TUE_2014_07_15, true},
			new object[] {WED_2014_07_16, true},
			new object[] {THU_2014_07_17, true},
			new object[] {FRI_2014_07_18, false},
			new object[] {SAT_2014_07_19, false},
			new object[] {SUN_2014_07_20, true},
			new object[] {MON_2014_07_21, true}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "createNoHolidays") public void test_of_IterableIterable_noHolidays(java.time.LocalDate date, boolean isBusinessDay)
	  public virtual void test_of_IterableIterable_noHolidays(LocalDate date, bool isBusinessDay)
	  {
		IEnumerable<LocalDate> holidays = Arrays.asList();
		IEnumerable<DayOfWeek> weekendDays = Arrays.asList(FRIDAY, SATURDAY);
		ImmutableHolidayCalendar test = ImmutableHolidayCalendar.of(TEST_ID, holidays, weekendDays);
		assertEquals(test.isBusinessDay(date), isBusinessDay);
		assertEquals(test.isHoliday(date), !isBusinessDay);
		assertEquals(test.Holidays, ImmutableSortedSet.copyOf(holidays));
		assertEquals(test.WeekendDays, ImmutableSet.of(FRIDAY, SATURDAY));
		assertEquals(test.ToString(), "HolidayCalendar[" + TEST_ID.Name + "]");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combined()
	  {
		ImmutableHolidayCalendar base1 = ImmutableHolidayCalendar.of(TEST_ID, ImmutableList.of(MON_2014_07_14), SATURDAY, SUNDAY);
		ImmutableHolidayCalendar base2 = ImmutableHolidayCalendar.of(TEST_ID2, ImmutableList.of(WED_2014_07_16), FRIDAY, SATURDAY);

		ImmutableHolidayCalendar test = ImmutableHolidayCalendar.combined(base1, base2);
		assertEquals(test.Id, base1.Id.combinedWith(base2.Id));
		assertEquals(test.Name, base1.Id.combinedWith(base2.Id).Name);
		assertEquals(test.Holidays, ImmutableList.of(MON_2014_07_14, WED_2014_07_16));
		assertEquals(test.WeekendDays, ImmutableSet.of(FRIDAY, SATURDAY, SUNDAY));
	  }

	  public virtual void test_combined_same()
	  {
		ImmutableHolidayCalendar @base = ImmutableHolidayCalendar.of(TEST_ID, ImmutableList.of(MON_2014_07_14), SATURDAY, SUNDAY);

		ImmutableHolidayCalendar test = ImmutableHolidayCalendar.combined(@base, @base);
		assertSame(test, @base);
	  }

	  public virtual void test_combined_differentStartYear1()
	  {
		IEnumerable<LocalDate> holidays1 = Arrays.asList(WED_2015_04_01);
		ImmutableHolidayCalendar base1 = ImmutableHolidayCalendar.of(TEST_ID, holidays1, SATURDAY, SUNDAY);
		IEnumerable<LocalDate> holidays2 = Arrays.asList(MON_2014_07_14, TUE_2015_03_31);
		ImmutableHolidayCalendar base2 = ImmutableHolidayCalendar.of(TEST_ID2, holidays2, SATURDAY, SUNDAY);
		HolidayCalendar test = ImmutableHolidayCalendar.combined(base1, base2);
		assertEquals(test.Name, "Test1+Test2");

		assertEquals(test.isHoliday(THU_2014_07_10), false);
		assertEquals(test.isHoliday(FRI_2014_07_11), false);
		assertEquals(test.isHoliday(SAT_2014_07_12), true);
		assertEquals(test.isHoliday(SUN_2014_07_13), true);
		assertEquals(test.isHoliday(MON_2014_07_14), true);
		assertEquals(test.isHoliday(TUE_2014_07_15), false);

		assertEquals(test.isHoliday(MON_2015_03_30), false);
		assertEquals(test.isHoliday(TUE_2015_03_31), true);
		assertEquals(test.isHoliday(WED_2015_04_01), true);
		assertEquals(test.isHoliday(THU_2015_04_02), false);
	  }

	  public virtual void test_combined_differentStartYear2()
	  {
		IEnumerable<LocalDate> holidays1 = Arrays.asList(MON_2014_07_14, TUE_2015_03_31);
		ImmutableHolidayCalendar base1 = ImmutableHolidayCalendar.of(TEST_ID, holidays1, SATURDAY, SUNDAY);
		IEnumerable<LocalDate> holidays2 = Arrays.asList(WED_2015_04_01);
		ImmutableHolidayCalendar base2 = ImmutableHolidayCalendar.of(TEST_ID2, holidays2, SATURDAY, SUNDAY);
		HolidayCalendar test = ImmutableHolidayCalendar.combined(base1, base2);
		assertEquals(test.Name, "Test1+Test2");

		assertEquals(test.isHoliday(THU_2014_07_10), false);
		assertEquals(test.isHoliday(FRI_2014_07_11), false);
		assertEquals(test.isHoliday(SAT_2014_07_12), true);
		assertEquals(test.isHoliday(SUN_2014_07_13), true);
		assertEquals(test.isHoliday(MON_2014_07_14), true);
		assertEquals(test.isHoliday(TUE_2014_07_15), false);

		assertEquals(test.isHoliday(MON_2015_03_30), false);
		assertEquals(test.isHoliday(TUE_2015_03_31), true);
		assertEquals(test.isHoliday(WED_2015_04_01), true);
		assertEquals(test.isHoliday(THU_2015_04_02), false);
	  }

	  public virtual void test_combined_splitYears()
	  {
		IEnumerable<LocalDate> holidays1 = Arrays.asList(TUE_2018_07_17);
		ImmutableHolidayCalendar base1 = ImmutableHolidayCalendar.of(TEST_ID, holidays1, SATURDAY, SUNDAY);
		IEnumerable<LocalDate> holidays2 = Arrays.asList(WED_2015_04_01);
		ImmutableHolidayCalendar base2 = ImmutableHolidayCalendar.of(TEST_ID2, holidays2, SATURDAY, SUNDAY);
		HolidayCalendar test = ImmutableHolidayCalendar.combined(base1, base2);
		assertEquals(test.Name, "Test1+Test2");

		assertEquals(test.isHoliday(SAT_2014_07_12), true);
		assertEquals(test.isHoliday(SUN_2014_07_13), true);

		assertEquals(test.isHoliday(SAT_2015_03_28), true);
		assertEquals(test.isHoliday(SUN_2015_03_29), true);
		assertEquals(test.isHoliday(MON_2015_03_30), false);
		assertEquals(test.isHoliday(TUE_2015_03_31), false);
		assertEquals(test.isHoliday(WED_2015_04_01), true);
		assertEquals(test.isHoliday(THU_2015_04_02), false);

		assertEquals(test.isHoliday(SAT_2018_07_14), true);
		assertEquals(test.isHoliday(SUN_2018_07_15), true);
		assertEquals(test.isHoliday(MON_2018_07_16), false);
		assertEquals(test.isHoliday(TUE_2018_07_17), true);
		assertEquals(test.isHoliday(WED_2018_07_18), false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_isBusinessDay_outOfRange()
	  {
		IEnumerable<LocalDate> holidays = Arrays.asList(MON_2014_07_14, TUE_2014_07_15);
		ImmutableHolidayCalendar test = ImmutableHolidayCalendar.of(TEST_ID, holidays, SATURDAY, SUNDAY);
		assertEquals(test.isBusinessDay(LocalDate.of(2013, 12, 31)), true);
		assertEquals(test.isBusinessDay(LocalDate.of(2015, 1, 1)), true);
		assertThrowsIllegalArg(() => test.isBusinessDay(LocalDate.MIN));
		assertThrowsIllegalArg(() => test.isBusinessDay(LocalDate.MAX));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "shift") public static Object[][] data_shift()
	  public static object[][] data_shift()
	  {
		return new object[][]
		{
			new object[] {THU_2014_07_10, 1, FRI_2014_07_11},
			new object[] {FRI_2014_07_11, 1, TUE_2014_07_15},
			new object[] {SAT_2014_07_12, 1, TUE_2014_07_15},
			new object[] {SUN_2014_07_13, 1, TUE_2014_07_15},
			new object[] {MON_2014_07_14, 1, TUE_2014_07_15},
			new object[] {TUE_2014_07_15, 1, THU_2014_07_17},
			new object[] {WED_2014_07_16, 1, THU_2014_07_17},
			new object[] {THU_2014_07_17, 1, FRI_2014_07_18},
			new object[] {FRI_2014_07_18, 1, MON_2014_07_21},
			new object[] {SAT_2014_07_19, 1, MON_2014_07_21},
			new object[] {SUN_2014_07_20, 1, MON_2014_07_21},
			new object[] {MON_2014_07_21, 1, TUE_2014_07_22},
			new object[] {THU_2014_07_10, 2, TUE_2014_07_15},
			new object[] {FRI_2014_07_11, 2, THU_2014_07_17},
			new object[] {SAT_2014_07_12, 2, THU_2014_07_17},
			new object[] {SUN_2014_07_13, 2, THU_2014_07_17},
			new object[] {MON_2014_07_14, 2, THU_2014_07_17},
			new object[] {TUE_2014_07_15, 2, FRI_2014_07_18},
			new object[] {WED_2014_07_16, 2, FRI_2014_07_18},
			new object[] {THU_2014_07_17, 2, MON_2014_07_21},
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
			new object[] {TUE_2014_07_15, -1, FRI_2014_07_11},
			new object[] {WED_2014_07_16, -1, TUE_2014_07_15},
			new object[] {THU_2014_07_17, -1, TUE_2014_07_15},
			new object[] {FRI_2014_07_18, -1, THU_2014_07_17},
			new object[] {SAT_2014_07_19, -1, FRI_2014_07_18},
			new object[] {SUN_2014_07_20, -1, FRI_2014_07_18},
			new object[] {MON_2014_07_21, -1, FRI_2014_07_18},
			new object[] {TUE_2014_07_22, -1, MON_2014_07_21},
			new object[] {FRI_2014_07_11, -2, WED_2014_07_09},
			new object[] {SAT_2014_07_12, -2, THU_2014_07_10},
			new object[] {SUN_2014_07_13, -2, THU_2014_07_10},
			new object[] {MON_2014_07_14, -2, THU_2014_07_10},
			new object[] {TUE_2014_07_15, -2, THU_2014_07_10},
			new object[] {WED_2014_07_16, -2, FRI_2014_07_11},
			new object[] {THU_2014_07_17, -2, FRI_2014_07_11},
			new object[] {FRI_2014_07_18, -2, TUE_2014_07_15},
			new object[] {SAT_2014_07_19, -2, THU_2014_07_17},
			new object[] {SUN_2014_07_20, -2, THU_2014_07_17},
			new object[] {MON_2014_07_21, -2, THU_2014_07_17},
			new object[] {TUE_2014_07_22, -2, FRI_2014_07_18}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "shift") public void test_shift(java.time.LocalDate date, int amount, java.time.LocalDate expected)
	  public virtual void test_shift(LocalDate date, int amount, LocalDate expected)
	  {
		assertEquals(HOLCAL_MON_WED.shift(date, amount), expected);
	  }

	  public virtual void test_shift_SatSun()
	  {
		assertEquals(HOLCAL_SAT_SUN.shift(SAT_2014_07_12, -2), THU_2014_07_10);
		assertEquals(HOLCAL_SAT_SUN.shift(SAT_2014_07_12, 2), TUE_2014_07_15);
	  }

	  public virtual void test_shift_range()
	  {
		assertEquals(HOLCAL_MON_WED.shift(date(2010, 1, 1), 1), date(2010, 1, 4));
		assertThrowsIllegalArg(() => HOLCAL_MON_WED.shift(LocalDate.MIN, 1));
		assertThrowsIllegalArg(() => HOLCAL_MON_WED.shift(LocalDate.MAX.minusDays(1), 1));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "shift") public void test_adjustBy(java.time.LocalDate date, int amount, java.time.LocalDate expected)
	  public virtual void test_adjustBy(LocalDate date, int amount, LocalDate expected)
	  {
		assertEquals(date.with(HOLCAL_MON_WED.adjustBy(amount)), expected);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "next") public static Object[][] data_next()
	  public static object[][] data_next()
	  {
		return new object[][]
		{
			new object[] {THU_2014_07_10, FRI_2014_07_11, HOLCAL_MON_WED},
			new object[] {FRI_2014_07_11, TUE_2014_07_15, HOLCAL_MON_WED},
			new object[] {SAT_2014_07_12, TUE_2014_07_15, HOLCAL_MON_WED},
			new object[] {SUN_2014_07_13, TUE_2014_07_15, HOLCAL_MON_WED},
			new object[] {MON_2014_07_14, TUE_2014_07_15, HOLCAL_MON_WED},
			new object[] {TUE_2014_07_15, THU_2014_07_17, HOLCAL_MON_WED},
			new object[] {WED_2014_07_16, THU_2014_07_17, HOLCAL_MON_WED},
			new object[] {THU_2014_07_17, FRI_2014_07_18, HOLCAL_MON_WED},
			new object[] {FRI_2014_07_18, MON_2014_07_21, HOLCAL_MON_WED},
			new object[] {SAT_2014_07_19, MON_2014_07_21, HOLCAL_MON_WED},
			new object[] {SUN_2014_07_20, MON_2014_07_21, HOLCAL_MON_WED},
			new object[] {MON_2014_07_21, TUE_2014_07_22, HOLCAL_MON_WED},
			new object[] {MON_2014_12_29, WED_2014_12_31, HOLCAL_YEAR_END},
			new object[] {TUE_2014_12_30, WED_2014_12_31, HOLCAL_YEAR_END},
			new object[] {WED_2014_12_31, FRI_2015_01_02, HOLCAL_YEAR_END},
			new object[] {THU_2015_01_01, FRI_2015_01_02, HOLCAL_YEAR_END},
			new object[] {FRI_2015_01_02, MON_2015_01_05, HOLCAL_YEAR_END},
			new object[] {SAT_2015_01_03, MON_2015_01_05, HOLCAL_YEAR_END},
			new object[] {TUE_2015_03_31, WED_2015_04_01, HOLCAL_YEAR_END},
			new object[] {SAT_2014_07_12, MON_2014_07_14, HOLCAL_SAT_SUN}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "next") public void test_next(java.time.LocalDate date, java.time.LocalDate expectedNext, HolidayCalendar cal)
	  public virtual void test_next(LocalDate date, LocalDate expectedNext, HolidayCalendar cal)
	  {
		assertEquals(cal.next(date), expectedNext);
	  }

	  public virtual void test_next_range()
	  {
		assertEquals(HOLCAL_MON_WED.next(date(2010, 1, 1)), date(2010, 1, 4));
		assertThrowsIllegalArg(() => HOLCAL_MON_WED.next(LocalDate.MIN));
		assertThrowsIllegalArg(() => HOLCAL_MON_WED.next(LocalDate.MAX.minusDays(1)));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "nextOrSame") public static Object[][] data_nextOrSame()
	  public static object[][] data_nextOrSame()
	  {
		return new object[][]
		{
			new object[] {THU_2014_07_10, THU_2014_07_10, HOLCAL_MON_WED},
			new object[] {FRI_2014_07_11, FRI_2014_07_11, HOLCAL_MON_WED},
			new object[] {SAT_2014_07_12, TUE_2014_07_15, HOLCAL_MON_WED},
			new object[] {SUN_2014_07_13, TUE_2014_07_15, HOLCAL_MON_WED},
			new object[] {MON_2014_07_14, TUE_2014_07_15, HOLCAL_MON_WED},
			new object[] {TUE_2014_07_15, TUE_2014_07_15, HOLCAL_MON_WED},
			new object[] {WED_2014_07_16, THU_2014_07_17, HOLCAL_MON_WED},
			new object[] {THU_2014_07_17, THU_2014_07_17, HOLCAL_MON_WED},
			new object[] {FRI_2014_07_18, FRI_2014_07_18, HOLCAL_MON_WED},
			new object[] {SAT_2014_07_19, MON_2014_07_21, HOLCAL_MON_WED},
			new object[] {SUN_2014_07_20, MON_2014_07_21, HOLCAL_MON_WED},
			new object[] {MON_2014_07_21, MON_2014_07_21, HOLCAL_MON_WED},
			new object[] {MON_2014_12_29, MON_2014_12_29, HOLCAL_YEAR_END},
			new object[] {TUE_2014_12_30, WED_2014_12_31, HOLCAL_YEAR_END},
			new object[] {WED_2014_12_31, WED_2014_12_31, HOLCAL_YEAR_END},
			new object[] {THU_2015_01_01, FRI_2015_01_02, HOLCAL_YEAR_END},
			new object[] {FRI_2015_01_02, FRI_2015_01_02, HOLCAL_YEAR_END},
			new object[] {SAT_2015_01_03, MON_2015_01_05, HOLCAL_YEAR_END},
			new object[] {TUE_2015_03_31, TUE_2015_03_31, HOLCAL_YEAR_END},
			new object[] {WED_2015_04_01, WED_2015_04_01, HOLCAL_YEAR_END},
			new object[] {SAT_2014_07_12, MON_2014_07_14, HOLCAL_SAT_SUN}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "nextOrSame") public void test_nextOrSame(java.time.LocalDate date, java.time.LocalDate expectedNext, HolidayCalendar cal)
	  public virtual void test_nextOrSame(LocalDate date, LocalDate expectedNext, HolidayCalendar cal)
	  {
		assertEquals(cal.nextOrSame(date), expectedNext);
	  }

	  public virtual void test_nextOrSame_range()
	  {
		assertEquals(HOLCAL_MON_WED.nextOrSame(date(2010, 1, 1)), date(2010, 1, 1));
		assertThrowsIllegalArg(() => HOLCAL_MON_WED.nextOrSame(LocalDate.MIN));
		assertThrowsIllegalArg(() => HOLCAL_MON_WED.nextOrSame(LocalDate.MAX));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "previous") public static Object[][] data_previous()
	  public static object[][] data_previous()
	  {
		return new object[][]
		{
			new object[] {FRI_2014_07_11, THU_2014_07_10, HOLCAL_MON_WED},
			new object[] {SAT_2014_07_12, FRI_2014_07_11, HOLCAL_MON_WED},
			new object[] {SUN_2014_07_13, FRI_2014_07_11, HOLCAL_MON_WED},
			new object[] {MON_2014_07_14, FRI_2014_07_11, HOLCAL_MON_WED},
			new object[] {TUE_2014_07_15, FRI_2014_07_11, HOLCAL_MON_WED},
			new object[] {WED_2014_07_16, TUE_2014_07_15, HOLCAL_MON_WED},
			new object[] {THU_2014_07_17, TUE_2014_07_15, HOLCAL_MON_WED},
			new object[] {FRI_2014_07_18, THU_2014_07_17, HOLCAL_MON_WED},
			new object[] {SAT_2014_07_19, FRI_2014_07_18, HOLCAL_MON_WED},
			new object[] {SUN_2014_07_20, FRI_2014_07_18, HOLCAL_MON_WED},
			new object[] {MON_2014_07_21, FRI_2014_07_18, HOLCAL_MON_WED},
			new object[] {TUE_2014_07_22, MON_2014_07_21, HOLCAL_MON_WED},
			new object[] {TUE_2014_12_30, MON_2014_12_29, HOLCAL_YEAR_END},
			new object[] {WED_2014_12_31, MON_2014_12_29, HOLCAL_YEAR_END},
			new object[] {THU_2015_01_01, WED_2014_12_31, HOLCAL_YEAR_END},
			new object[] {FRI_2015_01_02, WED_2014_12_31, HOLCAL_YEAR_END},
			new object[] {SAT_2015_01_03, FRI_2015_01_02, HOLCAL_YEAR_END},
			new object[] {MON_2015_01_05, FRI_2015_01_02, HOLCAL_YEAR_END},
			new object[] {WED_2015_04_01, TUE_2015_03_31, HOLCAL_YEAR_END},
			new object[] {SAT_2014_07_12, FRI_2014_07_11, HOLCAL_SAT_SUN}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "previous") public void test_previous(java.time.LocalDate date, java.time.LocalDate expectedPrevious, HolidayCalendar cal)
	  public virtual void test_previous(LocalDate date, LocalDate expectedPrevious, HolidayCalendar cal)
	  {
		assertEquals(cal.previous(date), expectedPrevious);
	  }

	  public virtual void test_previous_range()
	  {
		assertEquals(HOLCAL_MON_WED.previous(date(2010, 1, 1)), date(2009, 12, 31));
		assertThrowsIllegalArg(() => HOLCAL_MON_WED.previous(LocalDate.MIN.plusDays(1)));
		assertThrowsIllegalArg(() => HOLCAL_MON_WED.previous(LocalDate.MAX));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "previousOrSame") public static Object[][] data_previousOrSame()
	  public static object[][] data_previousOrSame()
	  {
		return new object[][]
		{
			new object[] {FRI_2014_07_11, FRI_2014_07_11, HOLCAL_MON_WED},
			new object[] {SAT_2014_07_12, FRI_2014_07_11, HOLCAL_MON_WED},
			new object[] {SUN_2014_07_13, FRI_2014_07_11, HOLCAL_MON_WED},
			new object[] {MON_2014_07_14, FRI_2014_07_11, HOLCAL_MON_WED},
			new object[] {TUE_2014_07_15, TUE_2014_07_15, HOLCAL_MON_WED},
			new object[] {WED_2014_07_16, TUE_2014_07_15, HOLCAL_MON_WED},
			new object[] {THU_2014_07_17, THU_2014_07_17, HOLCAL_MON_WED},
			new object[] {FRI_2014_07_18, FRI_2014_07_18, HOLCAL_MON_WED},
			new object[] {SAT_2014_07_19, FRI_2014_07_18, HOLCAL_MON_WED},
			new object[] {SUN_2014_07_20, FRI_2014_07_18, HOLCAL_MON_WED},
			new object[] {MON_2014_07_21, MON_2014_07_21, HOLCAL_MON_WED},
			new object[] {TUE_2014_07_22, TUE_2014_07_22, HOLCAL_MON_WED},
			new object[] {MON_2014_12_29, MON_2014_12_29, HOLCAL_YEAR_END},
			new object[] {TUE_2014_12_30, MON_2014_12_29, HOLCAL_YEAR_END},
			new object[] {WED_2014_12_31, WED_2014_12_31, HOLCAL_YEAR_END},
			new object[] {THU_2015_01_01, WED_2014_12_31, HOLCAL_YEAR_END},
			new object[] {FRI_2015_01_02, FRI_2015_01_02, HOLCAL_YEAR_END},
			new object[] {SAT_2015_01_03, FRI_2015_01_02, HOLCAL_YEAR_END},
			new object[] {MON_2015_01_05, MON_2015_01_05, HOLCAL_YEAR_END},
			new object[] {TUE_2015_03_31, TUE_2015_03_31, HOLCAL_YEAR_END},
			new object[] {WED_2015_04_01, WED_2015_04_01, HOLCAL_YEAR_END},
			new object[] {SAT_2014_07_12, FRI_2014_07_11, HOLCAL_SAT_SUN}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "previousOrSame") public void test_previousOrSame(java.time.LocalDate date, java.time.LocalDate expectedPrevious, HolidayCalendar cal)
	  public virtual void test_previousOrSame(LocalDate date, LocalDate expectedPrevious, HolidayCalendar cal)
	  {
		assertEquals(cal.previousOrSame(date), expectedPrevious);
	  }

	  public virtual void test_previousOrSame_range()
	  {
		assertEquals(HOLCAL_MON_WED.previousOrSame(date(2010, 1, 1)), date(2010, 1, 1));
		assertThrowsIllegalArg(() => HOLCAL_MON_WED.previousOrSame(LocalDate.MIN));
		assertThrowsIllegalArg(() => HOLCAL_MON_WED.previousOrSame(LocalDate.MAX));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "nextSameOrLastInMonth") public static Object[][] data_nextSameOrLastInMonth()
	  public static object[][] data_nextSameOrLastInMonth()
	  {
		return new object[][]
		{
			new object[] {THU_2014_07_10, THU_2014_07_10, HOLCAL_MON_WED},
			new object[] {FRI_2014_07_11, FRI_2014_07_11, HOLCAL_MON_WED},
			new object[] {SAT_2014_07_12, TUE_2014_07_15, HOLCAL_MON_WED},
			new object[] {SUN_2014_07_13, TUE_2014_07_15, HOLCAL_MON_WED},
			new object[] {MON_2014_07_14, TUE_2014_07_15, HOLCAL_MON_WED},
			new object[] {TUE_2014_07_15, TUE_2014_07_15, HOLCAL_MON_WED},
			new object[] {WED_2014_07_16, THU_2014_07_17, HOLCAL_MON_WED},
			new object[] {THU_2014_07_17, THU_2014_07_17, HOLCAL_MON_WED},
			new object[] {FRI_2014_07_18, FRI_2014_07_18, HOLCAL_MON_WED},
			new object[] {SAT_2014_07_19, MON_2014_07_21, HOLCAL_MON_WED},
			new object[] {SUN_2014_07_20, MON_2014_07_21, HOLCAL_MON_WED},
			new object[] {MON_2014_07_21, MON_2014_07_21, HOLCAL_MON_WED},
			new object[] {MON_2014_12_29, MON_2014_12_29, HOLCAL_YEAR_END},
			new object[] {TUE_2014_12_30, WED_2014_12_31, HOLCAL_YEAR_END},
			new object[] {WED_2014_12_31, WED_2014_12_31, HOLCAL_YEAR_END},
			new object[] {THU_2015_01_01, FRI_2015_01_02, HOLCAL_YEAR_END},
			new object[] {FRI_2015_01_02, FRI_2015_01_02, HOLCAL_YEAR_END},
			new object[] {SAT_2015_01_03, MON_2015_01_05, HOLCAL_YEAR_END},
			new object[] {TUE_2015_03_31, TUE_2015_03_31, HOLCAL_YEAR_END},
			new object[] {WED_2015_04_01, WED_2015_04_01, HOLCAL_YEAR_END},
			new object[] {SAT_2014_07_12, MON_2014_07_14, HOLCAL_SAT_SUN},
			new object[] {SAT_2015_02_28, FRI_2015_02_27, HOLCAL_SAT_SUN},
			new object[] {WED_2014_07_30, WED_2014_07_30, HOLCAL_END_MONTH},
			new object[] {THU_2014_07_31, WED_2014_07_30, HOLCAL_END_MONTH}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "nextSameOrLastInMonth") public void test_nextLastOrSame(java.time.LocalDate date, java.time.LocalDate expectedNext, HolidayCalendar cal)
	  public virtual void test_nextLastOrSame(LocalDate date, LocalDate expectedNext, HolidayCalendar cal)
	  {
		assertEquals(cal.nextSameOrLastInMonth(date), expectedNext);
	  }

	  public virtual void test_nextSameOrLastInMonth_range()
	  {
		assertEquals(HOLCAL_MON_WED.nextSameOrLastInMonth(date(2010, 1, 1)), date(2010, 1, 1));
		assertThrowsIllegalArg(() => HOLCAL_MON_WED.nextSameOrLastInMonth(LocalDate.MIN));
		assertThrowsIllegalArg(() => HOLCAL_MON_WED.nextSameOrLastInMonth(LocalDate.MAX));
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
		assertEquals(HOLCAL_END_MONTH.lastBusinessDayOfMonth(date), expectedEom);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "lastBusinessDayOfMonth") public void test_isLastBusinessDayOfMonth(java.time.LocalDate date, java.time.LocalDate expectedEom)
	  public virtual void test_isLastBusinessDayOfMonth(LocalDate date, LocalDate expectedEom)
	  {
		assertEquals(HOLCAL_END_MONTH.isLastBusinessDayOfMonth(date), date.Equals(expectedEom));
	  }

	  public virtual void test_lastBusinessDayOfMonth_satSun()
	  {
		assertEquals(HOLCAL_SAT_SUN.isLastBusinessDayOfMonth(MON_2014_06_30), true);
		assertEquals(HOLCAL_SAT_SUN.lastBusinessDayOfMonth(MON_2014_06_30), MON_2014_06_30);
	  }

	  public virtual void test_lastBusinessDayOfMonth_range()
	  {
		assertEquals(HOLCAL_END_MONTH.lastBusinessDayOfMonth(date(2010, 1, 1)), date(2010, 1, 29));
		assertThrowsIllegalArg(() => HOLCAL_END_MONTH.lastBusinessDayOfMonth(LocalDate.MIN));
		assertThrowsIllegalArg(() => HOLCAL_END_MONTH.lastBusinessDayOfMonth(LocalDate.MAX));
	  }

	  public virtual void test_isLastBusinessDayOfMonth_range()
	  {
		assertEquals(HOLCAL_END_MONTH.isLastBusinessDayOfMonth(date(2010, 1, 1)), false);
		assertThrowsIllegalArg(() => HOLCAL_END_MONTH.isLastBusinessDayOfMonth(LocalDate.MIN));
		assertThrowsIllegalArg(() => HOLCAL_END_MONTH.isLastBusinessDayOfMonth(LocalDate.MAX));
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
			new object[] {FRI_2014_07_11, TUE_2014_07_15, 1},
			new object[] {FRI_2014_07_11, WED_2014_07_16, 2},
			new object[] {FRI_2014_07_11, THU_2014_07_17, 2},
			new object[] {FRI_2014_07_11, FRI_2014_07_18, 3},
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
		assertEquals(HOLCAL_MON_WED.daysBetween(start, end), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combinedWith()
	  {
		IEnumerable<LocalDate> holidays1 = Arrays.asList(WED_2014_07_16);
		ImmutableHolidayCalendar base1 = ImmutableHolidayCalendar.of(TEST_ID, holidays1, SATURDAY, SUNDAY);
		IEnumerable<LocalDate> holidays2 = Arrays.asList(MON_2014_07_14);
		ImmutableHolidayCalendar base2 = ImmutableHolidayCalendar.of(TEST_ID2, holidays2, FRIDAY, SATURDAY);
		HolidayCalendar test = base1.combinedWith(base2);
		assertEquals(test.Name, "Test1+Test2");

		assertEquals(test.isHoliday(THU_2014_07_10), false);
		assertEquals(test.isHoliday(FRI_2014_07_11), true);
		assertEquals(test.isHoliday(SAT_2014_07_12), true);
		assertEquals(test.isHoliday(SUN_2014_07_13), true);
		assertEquals(test.isHoliday(MON_2014_07_14), true);
		assertEquals(test.isHoliday(TUE_2014_07_15), false);
		assertEquals(test.isHoliday(WED_2014_07_16), true);
		assertEquals(test.isHoliday(THU_2014_07_17), false);
		assertEquals(test.isHoliday(FRI_2014_07_18), true);
		assertEquals(test.isHoliday(SAT_2014_07_19), true);
		assertEquals(test.isHoliday(SUN_2014_07_20), true);
		assertEquals(test.isHoliday(MON_2014_07_21), false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combineWith_same()
	  {
		IEnumerable<LocalDate> holidays = Arrays.asList(WED_2014_07_16);
		ImmutableHolidayCalendar @base = ImmutableHolidayCalendar.of(TEST_ID, holidays, SATURDAY, SUNDAY);
		HolidayCalendar test = @base.combinedWith(@base);
		assertSame(test, @base);
	  }

	  public virtual void test_combineWith_none()
	  {
		IEnumerable<LocalDate> holidays = Arrays.asList(WED_2014_07_16);
		ImmutableHolidayCalendar @base = ImmutableHolidayCalendar.of(TEST_ID, holidays, SATURDAY, SUNDAY);
		HolidayCalendar test = @base.combinedWith(HolidayCalendars.NO_HOLIDAYS);
		assertSame(test, @base);
	  }

	  public virtual void test_combineWith_satSun()
	  {
		IEnumerable<LocalDate> holidays = Arrays.asList(WED_2014_07_16);
		ImmutableHolidayCalendar @base = ImmutableHolidayCalendar.of(TEST_ID, holidays, SATURDAY, SUNDAY);
		HolidayCalendar test = @base.combinedWith(HolidayCalendars.FRI_SAT);
		assertEquals(test.Name, "Fri/Sat+Test1");

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

	  //-------------------------------------------------------------------------
	  public virtual void test_broadCheck()
	  {
		LocalDate start = LocalDate.of(2010, 1, 1);
		LocalDate end = LocalDate.of(2020, 1, 1);
		Random random = new Random(547698);
		for (int i = 0; i < 10; i++)
		{
		  // create sample holiday dates
		  LocalDate date = start;
		  SortedSet<LocalDate> set = new SortedSet<LocalDate>();
		  while (date.isBefore(end))
		  {
			set.Add(date);
			date = date.plusDays(random.Next(10) + 1);
		  }
		  // check holiday calendar works using simple algorithm
		  ImmutableHolidayCalendar test = ImmutableHolidayCalendar.of(HolidayCalendarId.of("TestBroad" + i), set, SATURDAY, SUNDAY);
		  LocalDate checkDate = start;
		  while (checkDate.isBefore(end))
		  {
			DayOfWeek dow = checkDate.DayOfWeek;
			assertEquals(test.isHoliday(checkDate), dow == SATURDAY || dow == SUNDAY || set.Contains(checkDate));
			checkDate = checkDate.plusDays(1);
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equals()
	  {
		ImmutableHolidayCalendar a1 = ImmutableHolidayCalendar.of(TEST_ID, Arrays.asList(WED_2014_07_16), SATURDAY, SUNDAY);
		ImmutableHolidayCalendar a2 = ImmutableHolidayCalendar.of(TEST_ID, Arrays.asList(WED_2014_07_16), SATURDAY, SUNDAY);
		ImmutableHolidayCalendar b = ImmutableHolidayCalendar.of(TEST_ID2, Arrays.asList(WED_2014_07_16), SATURDAY, SUNDAY);
		ImmutableHolidayCalendar c = ImmutableHolidayCalendar.of(TEST_ID, Arrays.asList(THU_2014_07_10), SATURDAY, SUNDAY);
		assertEquals(a1.Equals(a2), true);
		assertEquals(a1.Equals(b), false);
		assertEquals(a1.Equals(c), true); // only name compared
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(HOLCAL_MON_WED);
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(HOLCAL_MON_WED);
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_readOldJodaFormat() throws java.io.IOException
	  public virtual void test_readOldJodaFormat()
	  {
		ResourceLocator file = ResourceLocator.ofClasspath("com/opengamma/strata/basics/date/ImmutableHolidayCalendar-Old.json");
		string str = file.CharSource.read();
		ImmutableHolidayCalendar cal = JodaBeanSer.PRETTY.jsonReader().read(str, typeof(ImmutableHolidayCalendar));
		assertEquals(cal.Id, HolidayCalendarId.of("NZAU"));
	  }

	}

}