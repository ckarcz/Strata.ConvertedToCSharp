/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.NO_HOLIDAYS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.SAT_SUN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;

	/// <summary>
	/// Test <seealso cref="DaysAdjustment"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DaysAdjustmentTest
	public class DaysAdjustmentTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly HolidayCalendarId WED_THU = HolidayCalendarId.of("WedThu");
	  private static readonly BusinessDayAdjustment BDA_NONE = BusinessDayAdjustment.NONE;
	  private static readonly BusinessDayAdjustment BDA_FOLLOW_SAT_SUN = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, SAT_SUN);
	  private static readonly BusinessDayAdjustment BDA_FOLLOW_WED_THU = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, WED_THU);

	  //-------------------------------------------------------------------------
	  public virtual void test_NONE()
	  {
		DaysAdjustment test = DaysAdjustment.NONE;
		assertEquals(test.Days, 0);
		assertEquals(test.Calendar, NO_HOLIDAYS);
		assertEquals(test.Adjustment, BDA_NONE);
		assertEquals(test.ToString(), "0 calendar days");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_ofCalendarDays1_oneDay()
	  {
		DaysAdjustment test = DaysAdjustment.ofCalendarDays(1);
		assertEquals(test.Days, 1);
		assertEquals(test.Calendar, NO_HOLIDAYS);
		assertEquals(test.Adjustment, BDA_NONE);
		assertEquals(test.ToString(), "1 calendar day");
	  }

	  public virtual void test_ofCalendarDays1_threeDays()
	  {
		DaysAdjustment test = DaysAdjustment.ofCalendarDays(3);
		assertEquals(test.Days, 3);
		assertEquals(test.Calendar, NO_HOLIDAYS);
		assertEquals(test.Adjustment, BDA_NONE);
		assertEquals(test.ToString(), "3 calendar days");
	  }

	  public virtual void test_ofCalendarDays1_adjust()
	  {
		DaysAdjustment test = DaysAdjustment.ofCalendarDays(2);
		LocalDate @base = date(2014, 8, 15); // Fri
		assertEquals(test.adjust(@base, REF_DATA), date(2014, 8, 17)); // Sun
		assertEquals(test.resolve(REF_DATA).adjust(@base), date(2014, 8, 17)); // Sun
	  }

	  public virtual void test_ofCalendarDays2_oneDay()
	  {
		DaysAdjustment test = DaysAdjustment.ofCalendarDays(1, BDA_FOLLOW_SAT_SUN);
		assertEquals(test.Days, 1);
		assertEquals(test.Calendar, NO_HOLIDAYS);
		assertEquals(test.Adjustment, BDA_FOLLOW_SAT_SUN);
		assertEquals(test.ToString(), "1 calendar day then apply Following using calendar Sat/Sun");
	  }

	  public virtual void test_ofCalendarDays2_fourDays()
	  {
		DaysAdjustment test = DaysAdjustment.ofCalendarDays(4, BDA_FOLLOW_SAT_SUN);
		assertEquals(test.Days, 4);
		assertEquals(test.Calendar, NO_HOLIDAYS);
		assertEquals(test.Adjustment, BDA_FOLLOW_SAT_SUN);
		assertEquals(test.ToString(), "4 calendar days then apply Following using calendar Sat/Sun");
	  }

	  public virtual void test_ofCalendarDays2_adjust()
	  {
		DaysAdjustment test = DaysAdjustment.ofCalendarDays(2, BDA_FOLLOW_SAT_SUN);
		LocalDate @base = date(2014, 8, 15); // Fri
		assertEquals(test.adjust(@base, REF_DATA), date(2014, 8, 18)); // Mon
		assertEquals(test.resolve(REF_DATA).adjust(@base), date(2014, 8, 18)); // Mon
	  }

	  public virtual void test_ofCalendarDays2_null()
	  {
		assertThrowsIllegalArg(() => DaysAdjustment.ofCalendarDays(2, null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_ofBusinessDays2_oneDay()
	  {
		DaysAdjustment test = DaysAdjustment.ofBusinessDays(1, SAT_SUN);
		assertEquals(test.Days, 1);
		assertEquals(test.Calendar, SAT_SUN);
		assertEquals(test.Adjustment, BDA_NONE);
		assertEquals(test.ToString(), "1 business day using calendar Sat/Sun");
	  }

	  public virtual void test_ofBusinessDays2_threeDays()
	  {
		DaysAdjustment test = DaysAdjustment.ofBusinessDays(3, SAT_SUN);
		assertEquals(test.Days, 3);
		assertEquals(test.Calendar, SAT_SUN);
		assertEquals(test.Adjustment, BDA_NONE);
		assertEquals(test.ToString(), "3 business days using calendar Sat/Sun");
	  }

	  public virtual void test_ofBusinessDays2_adjust()
	  {
		DaysAdjustment test = DaysAdjustment.ofBusinessDays(2, SAT_SUN);
		LocalDate @base = date(2014, 8, 15); // Fri
		assertEquals(test.adjust(@base, REF_DATA), date(2014, 8, 19)); // Tue
		assertEquals(test.resolve(REF_DATA).adjust(@base), date(2014, 8, 19)); // Tue
	  }

	  public virtual void test_ofBusinessDays2_null()
	  {
		assertThrowsIllegalArg(() => DaysAdjustment.ofBusinessDays(2, null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_ofBusinessDays3_oneDay()
	  {
		DaysAdjustment test = DaysAdjustment.ofBusinessDays(1, SAT_SUN, BDA_FOLLOW_WED_THU);
		assertEquals(test.Days, 1);
		assertEquals(test.Calendar, SAT_SUN);
		assertEquals(test.Adjustment, BDA_FOLLOW_WED_THU);
		assertEquals(test.ToString(), "1 business day using calendar Sat/Sun then apply Following using " + "calendar WedThu");
	  }

	  public virtual void test_ofBusinessDays3_fourDays()
	  {
		DaysAdjustment test = DaysAdjustment.ofBusinessDays(4, SAT_SUN, BDA_FOLLOW_WED_THU);
		assertEquals(test.Days, 4);
		assertEquals(test.Calendar, SAT_SUN);
		assertEquals(test.Adjustment, BDA_FOLLOW_WED_THU);
		assertEquals(test.ToString(), "4 business days using calendar Sat/Sun then apply Following using " + "calendar WedThu");
	  }

	  public virtual void test_ofBusinessDays3_adjust()
	  {
		ImmutableHolidayCalendar cal = ImmutableHolidayCalendar.of(WED_THU, ImmutableList.of(), WEDNESDAY, THURSDAY);
		ReferenceData refData = ImmutableReferenceData.of(ImmutableMap.of(WED_THU, cal)).combinedWith(REF_DATA);
		DaysAdjustment test = DaysAdjustment.ofBusinessDays(3, SAT_SUN, BDA_FOLLOW_WED_THU);
		LocalDate @base = date(2014, 8, 15); // Fri
		assertEquals(test.adjust(@base, refData), date(2014, 8, 22)); // Fri (3 days gives Wed, following moves to Fri)
		assertEquals(test.resolve(refData).adjust(@base), date(2014, 8, 22)); // Fri (3 days gives Wed, following moves to Fri)
	  }

	  public virtual void test_ofBusinessDays3_null()
	  {
		assertThrowsIllegalArg(() => DaysAdjustment.ofBusinessDays(3, null, BDA_FOLLOW_SAT_SUN));
		assertThrowsIllegalArg(() => DaysAdjustment.ofBusinessDays(3, SAT_SUN, null));
		assertThrowsIllegalArg(() => DaysAdjustment.ofBusinessDays(3, null, null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_getResultCalendar1()
	  {
		DaysAdjustment test = DaysAdjustment.ofBusinessDays(3, SAT_SUN);
		assertEquals(test.ResultCalendar, SAT_SUN);
	  }

	  public virtual void test_getResultCalendar2()
	  {
		DaysAdjustment test = DaysAdjustment.ofBusinessDays(3, SAT_SUN, BDA_FOLLOW_WED_THU);
		assertEquals(test.ResultCalendar, WED_THU);
	  }

	  public virtual void test_getResultCalendar3()
	  {
		DaysAdjustment test = DaysAdjustment.ofCalendarDays(3);
		assertEquals(test.ResultCalendar, NO_HOLIDAYS);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_normalized()
	  {
		DaysAdjustment zeroDays = DaysAdjustment.ofCalendarDays(0, BDA_FOLLOW_SAT_SUN);
		DaysAdjustment zeroDaysWithCalendar = DaysAdjustment.ofBusinessDays(0, WED_THU, BDA_FOLLOW_SAT_SUN);
		DaysAdjustment twoDays = DaysAdjustment.ofCalendarDays(2, BDA_FOLLOW_SAT_SUN);
		DaysAdjustment twoDaysWithCalendar = DaysAdjustment.ofBusinessDays(2, WED_THU, BDA_FOLLOW_SAT_SUN);
		DaysAdjustment twoDaysWithSameCalendar = DaysAdjustment.ofBusinessDays(2, SAT_SUN, BDA_FOLLOW_SAT_SUN);
		DaysAdjustment twoDaysWithNoAdjust = DaysAdjustment.ofBusinessDays(2, SAT_SUN);
		assertEquals(zeroDays.normalized(), zeroDays);
		assertEquals(zeroDaysWithCalendar.normalized(), zeroDays);
		assertEquals(twoDays.normalized(), twoDays);
		assertEquals(twoDaysWithCalendar.normalized(), twoDaysWithCalendar);
		assertEquals(twoDaysWithSameCalendar.normalized(), twoDaysWithNoAdjust);
		assertEquals(twoDaysWithNoAdjust.normalized(), twoDaysWithNoAdjust);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void Equals()
	  {
		DaysAdjustment a = DaysAdjustment.ofBusinessDays(3, NO_HOLIDAYS, BDA_FOLLOW_SAT_SUN);
		DaysAdjustment b = DaysAdjustment.ofBusinessDays(4, NO_HOLIDAYS, BDA_FOLLOW_SAT_SUN);
		DaysAdjustment c = DaysAdjustment.ofBusinessDays(3, WED_THU, BDA_FOLLOW_SAT_SUN);
		DaysAdjustment d = DaysAdjustment.ofBusinessDays(3, NO_HOLIDAYS, BDA_FOLLOW_WED_THU);
		assertEquals(a.Equals(b), false);
		assertEquals(a.Equals(c), false);
		assertEquals(a.Equals(d), false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(DaysAdjustment.ofCalendarDays(4, BDA_FOLLOW_SAT_SUN));
	  }

	  public virtual void coverage_builder()
	  {
		DaysAdjustment test = DaysAdjustment.builder().days(1).calendar(SAT_SUN).adjustment(BDA_FOLLOW_WED_THU).build();
		assertEquals(test.Days, 1);
		assertEquals(test.Calendar, SAT_SUN);
		assertEquals(test.Adjustment, BDA_FOLLOW_WED_THU);
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(DaysAdjustment.ofCalendarDays(4, BDA_FOLLOW_SAT_SUN));
	  }

	}

}