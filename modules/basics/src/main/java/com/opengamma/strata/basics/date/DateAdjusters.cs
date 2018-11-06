/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{

	/// <summary>
	/// Date adjusters that perform useful operations on {@code LocalDate}.
	/// <para>
	/// This is a static utility class.
	/// Returned objects are immutable and thread-safe.
	/// </para>
	/// </summary>
	public sealed class DateAdjusters
	{

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private DateAdjusters()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance that finds the next leap day after the input date.
	  /// <para>
	  /// The adjuster returns the next occurrence of February 29 after the input date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> an adjuster that finds the next leap day </returns>
	  public static DateAdjuster nextLeapDay()
	  {
		return DateAdjusters.nextLeapDay;
	  }

	  /// <summary>
	  /// Finds the next leap day after the input date.
	  /// </summary>
	  /// <param name="input">  the input date </param>
	  /// <returns> the next leap day date </returns>
	  internal static LocalDate nextLeapDay(LocalDate input)
	  {
		// already a leap day, move forward either 4 or 8 years
		if (input.MonthValue == 2 && input.DayOfMonth == 29)
		{
		  return ensureLeapDay(input.Year + 4);
		}
		// handle if before February 29 in a leap year
		if (input.LeapYear && input.MonthValue <= 2)
		{
		  return LocalDate.of(input.Year, 2, 29);
		}
		// handle any other date
		return ensureLeapDay(((input.Year / 4) * 4) + 4);
	  }

	  /// <summary>
	  /// Obtains a date adjuster that finds the next leap day on or after the input date.
	  /// <para>
	  /// If the input date is February 29, the input date is returned unaltered.
	  /// Otherwise, the adjuster returns the next occurrence of February 29 after the input date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> an adjuster that finds the next leap day </returns>
	  public static DateAdjuster nextOrSameLeapDay()
	  {
		return DateAdjusters.nextOrSameLeapDay;
	  }

	  /// <summary>
	  /// Finds the next leap day on or after the input date.
	  /// <para>
	  /// If the input date is February 29, the input date is returned unaltered.
	  /// Otherwise, the adjuster returns the next occurrence of February 29 after the input date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="input">  the input date </param>
	  /// <returns> the next leap day date </returns>
	  internal static LocalDate nextOrSameLeapDay(LocalDate input)
	  {
		// already a leap day, return it
		if (input.MonthValue == 2 && input.DayOfMonth == 29)
		{
		  return input;
		}
		// handle if before February 29 in a leap year
		if (input.LeapYear && input.MonthValue <= 2)
		{
		  return LocalDate.of(input.Year, 2, 29);
		}
		// handle any other date
		return ensureLeapDay(((input.Year / 4) * 4) + 4);
	  }

	  // handle 2100, which is not a leap year
	  private static LocalDate ensureLeapDay(int possibleLeapYear)
	  {
		if (Year.isLeap(possibleLeapYear))
		{
		  return LocalDate.of(possibleLeapYear, 2, 29);
		}
		else
		{
		  return LocalDate.of(possibleLeapYear + 4, 2, 29);
		}
	  }

	}

}