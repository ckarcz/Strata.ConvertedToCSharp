using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swap.e2e
{


	using HolidayCalendar = com.opengamma.strata.basics.date.HolidayCalendar;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using ImmutableHolidayCalendar = com.opengamma.strata.basics.date.ImmutableHolidayCalendar;

	/// <summary>
	/// Dummy calendar for end-to-end tests.
	/// This exists primarily to match numbers against older ones.
	/// </summary>
	public class CalendarUSD
	{

	  public static readonly HolidayCalendarId NYC = HolidayCalendarId.of("TestNYC");
	  public static readonly HolidayCalendar NYC_CALENDAR;
	  static CalendarUSD()
	  {
		IList<LocalDate> holidays = new List<LocalDate>();
		int startYear = 2013;
		int endYear = 2063;
		for (int i = startYear; i <= endYear; i++)
		{
		  holidays.Add(LocalDate.of(i, 1, 1));
		  holidays.Add(LocalDate.of(i, 7, 4));
		  holidays.Add(LocalDate.of(i, 11, 11));
		  holidays.Add(LocalDate.of(i, 12, 25));
		}
		holidays.Add(LocalDate.of(2014, 1, 20));
		holidays.Add(LocalDate.of(2014, 2, 17));
		holidays.Add(LocalDate.of(2014, 5, 26));
		holidays.Add(LocalDate.of(2014, 9, 1));
		holidays.Add(LocalDate.of(2014, 10, 13));
		holidays.Add(LocalDate.of(2014, 11, 27));
		holidays.Add(LocalDate.of(2015, 1, 19));
		holidays.Add(LocalDate.of(2015, 2, 16));
		holidays.Add(LocalDate.of(2015, 5, 25));
		holidays.Add(LocalDate.of(2015, 9, 7));
		holidays.Add(LocalDate.of(2015, 10, 12));
		holidays.Add(LocalDate.of(2015, 11, 26));
		holidays.Add(LocalDate.of(2016, 1, 18));
		holidays.Add(LocalDate.of(2016, 2, 15));
		holidays.Add(LocalDate.of(2016, 5, 30));
		holidays.Add(LocalDate.of(2016, 9, 5));
		holidays.Add(LocalDate.of(2016, 10, 10));
		holidays.Add(LocalDate.of(2016, 11, 24));
		holidays.Add(LocalDate.of(2016, 12, 26));
		holidays.Add(LocalDate.of(2017, 1, 2));
		holidays.Add(LocalDate.of(2017, 1, 16));
		holidays.Add(LocalDate.of(2017, 2, 20));
		holidays.Add(LocalDate.of(2017, 5, 29));
		holidays.Add(LocalDate.of(2017, 9, 4));
		holidays.Add(LocalDate.of(2017, 10, 9));
		holidays.Add(LocalDate.of(2017, 11, 23));
		NYC_CALENDAR = ImmutableHolidayCalendar.of(NYC, holidays, SATURDAY, SUNDAY);
	  }

	}

}