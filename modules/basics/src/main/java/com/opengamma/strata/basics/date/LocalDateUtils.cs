using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{

	/// <summary>
	/// Utilities for working with {@code LocalDate}.
	/// </summary>
	internal sealed class LocalDateUtils
	{

	  // First day-of-month minus one for a standard year
	  // array length 13 with element zero ignored, so month 1 to 12 can be queried directly
	  private static readonly int[] STANDARD = new int[] {0, 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334};
	  // First day-of-month minus one for a leap year
	  // array length 13 with element zero ignored, so month 1 to 12 can be queried directly
	  private static readonly int[] LEAP = new int[] {0, 0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335};

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private LocalDateUtils()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the day-of-year of the date.
	  /// <para>
	  /// Faster than the JDK method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to query </param>
	  /// <returns> the day-of-year </returns>
	  internal static int doy(LocalDate date)
	  {
		int[] lookup = (date.LeapYear ? LEAP : STANDARD);
		return lookup[date.MonthValue] + date.DayOfMonth;
	  }

	  /// <summary>
	  /// Adds a number of days to the date.
	  /// <para>
	  /// Faster than the JDK method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to add to </param>
	  /// <param name="daysToAdd">  the days to add </param>
	  /// <returns> the new date </returns>
	  internal static LocalDate plusDays(LocalDate date, int daysToAdd)
	  {
		if (daysToAdd == 0)
		{
		  return date;
		}
		// add the days to the current day-of-month
		// if it is guaranteed to be in this month or the next month then fast path it
		// (59th Jan is 28th Feb, 59th Feb is 31st Mar)
		long dom = date.DayOfMonth + daysToAdd;
		if (dom > 0 && dom <= 59)
		{
		  int monthLen = date.lengthOfMonth();
		  int month = date.MonthValue;
		  int year = date.Year;
		  if (dom <= monthLen)
		  {
			return LocalDate.of(year, month, (int) dom);
		  }
		  else if (month < 12)
		  {
			return LocalDate.of(year, month + 1, (int)(dom - monthLen));
		  }
		  else
		  {
			return LocalDate.of(year + 1, 1, (int)(dom - monthLen));
		  }
		}
		long mjDay = Math.addExact(date.toEpochDay(), daysToAdd);
		return LocalDate.ofEpochDay(mjDay);
	  }

	  /// <summary>
	  /// Returns the number of days between two dates.
	  /// <para>
	  /// Faster than the JDK method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="firstDate">  the first date </param>
	  /// <param name="secondDate">  the second date, after the first </param>
	  /// <returns> the new date </returns>
	  internal static long daysBetween(LocalDate firstDate, LocalDate secondDate)
	  {
		int firstYear = firstDate.Year;
		int secondYear = secondDate.Year;
		if (firstYear == secondYear)
		{
		  return doy(secondDate) - doy(firstDate);
		}
		if ((firstYear + 1) == secondYear)
		{
		  return (firstDate.lengthOfYear() - doy(firstDate)) + doy(secondDate);
		}
		return secondDate.toEpochDay() - firstDate.toEpochDay();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Streams the set of dates included in the range.
	  /// <para>
	  /// This returns a stream consisting of each date in the range.
	  /// The stream is ordered.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="startInclusive">  the start date </param>
	  /// <param name="endExclusive">  the end date </param>
	  /// <returns> the stream of dates from the start to the end </returns>
	  internal static Stream<LocalDate> stream(LocalDate startInclusive, LocalDate endExclusive)
	  {
		IEnumerator<LocalDate> it = new IteratorAnonymousInnerClass(startInclusive, endExclusive);
		long count = endExclusive.toEpochDay() - startInclusive.toEpochDay() + 1;
		Spliterator<LocalDate> spliterator = Spliterators.spliterator(it, count, Spliterator.IMMUTABLE | Spliterator.NONNULL | Spliterator.DISTINCT | Spliterator.ORDERED | Spliterator.SORTED | Spliterator.SIZED | Spliterator.SUBSIZED);
		return StreamSupport.stream(spliterator, false);
	  }

	  private class IteratorAnonymousInnerClass : IEnumerator<LocalDate>
	  {
		  private LocalDate startInclusive;
		  private LocalDate endExclusive;

		  public IteratorAnonymousInnerClass(LocalDate startInclusive, LocalDate endExclusive)
		  {
			  this.startInclusive = startInclusive;
			  this.endExclusive = endExclusive;
		  }

		  private LocalDate current = startInclusive;

		  public LocalDate next()
		  {
			LocalDate result = current;
			current = plusDays(current, 1);
			return result;
		  }

		  public bool hasNext()
		  {
			return current.isBefore(endExclusive);
		  }
	  }

	}

}