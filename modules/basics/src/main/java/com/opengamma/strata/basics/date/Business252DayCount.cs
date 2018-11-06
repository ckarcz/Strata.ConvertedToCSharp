using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{

	using NamedLookup = com.opengamma.strata.collect.named.NamedLookup;

	/// <summary>
	/// Day count convention for 'Bus/252'.
	/// <para>
	/// This day count is based on a holiday calendar, which is stored in the day count
	/// and referenced in the name.
	/// </para>
	/// </summary>
	internal sealed class Business252DayCount : NamedLookup<DayCount>
	{

	  /// <summary>
	  /// The singleton instance of the lookup.
	  /// </summary>
	  public static readonly Business252DayCount INSTANCE = new Business252DayCount();

	  /// <summary>
	  /// The cache of day count by name.
	  /// </summary>
	  private static readonly ConcurrentMap<string, DayCount> BY_NAME = new ConcurrentDictionary<string, DayCount>();
	  /// <summary>
	  /// The cache of day count by calendar.
	  /// </summary>
	  private static readonly ConcurrentMap<string, DayCount> BY_CALENDAR = new ConcurrentDictionary<string, DayCount>();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private Business252DayCount()
	  {
	  }

	  // obtains the day count
	  internal DayCount of(HolidayCalendar calendar)
	  {
		return BY_CALENDAR.computeIfAbsent(calendar.Name, this.createByCalendarName);
	  }

	  private DayCount createByCalendarName(string calendarName)
	  {
		return lookup("Bus/252 " + calendarName);
	  }

	  //-------------------------------------------------------------------------
	  public override DayCount lookup(string name)
	  {
		DayCount value = BY_NAME.get(name);
		if (value == null)
		{
		  if (name.regionMatches(true, 0, "Bus/252 ", 0, 8))
		  {
			HolidayCalendar cal = HolidayCalendars.of(name.Substring(8)); // load from standard calendars
			string correctName = "Bus/252 " + cal.Name;
			DayCount created = new Bus252(correctName, cal);
			value = BY_NAME.computeIfAbsent(correctName, k => created);
			BY_NAME.putIfAbsent(correctName.ToUpper(Locale.ENGLISH), created);
		  }
		}
		return value;
	  }

	  public IDictionary<string, DayCount> lookupAll()
	  {
		return BY_NAME;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Implementation of the day-of-month roll convention.
	  /// </summary>
	  [Serializable]
	  internal sealed class Bus252 : DayCount
	  {

		internal const long serialVersionUID = 1L;

		internal readonly string name;
		[NonSerialized]
		internal readonly HolidayCalendar calendar;

		internal Bus252(string name, HolidayCalendar calendar)
		{
		  this.name = name;
		  this.calendar = calendar;
		}

		// resolve instance
		internal object readResolve()
		{
		  return DayCount.of(name);
		}

		public double yearFraction(LocalDate firstDate, LocalDate secondDate, DayCount_ScheduleInfo scheduleInfo)
		{
		  return calendar.daysBetween(firstDate, secondDate) / 252d;
		}

		public int days(LocalDate firstDate, LocalDate secondDate)
		{
		  return calendar.daysBetween(firstDate, secondDate);
		}

		//-------------------------------------------------------------------------
		public string Name
		{
			get
			{
			  return name;
			}
		}

		public override bool Equals(object obj)
		{
		  if (obj == this)
		  {
			return true;
		  }
		  if (obj is Bus252)
		  {
			return ((Bus252) obj).name.Equals(name);
		  }
		  return false;
		}

		public override int GetHashCode()
		{
		  return name.GetHashCode();
		}

		public override string ToString()
		{
		  return name;
		}

	  }

	}

}