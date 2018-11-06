/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.LocalDateUtils.plusDays;


	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Named = com.opengamma.strata.collect.named.Named;

	/// <summary>
	/// A holiday calendar, classifying dates as holidays or business days.
	/// <para>
	/// Many calculations in finance require knowledge of whether a date is a business day or not.
	/// This class encapsulates that knowledge, with each day treated as a holiday or a business day.
	/// Weekends are effectively treated as a special kind of holiday.
	/// </para>
	/// <para>
	/// Applications should refer to holidays using <seealso cref="HolidayCalendarId"/>.
	/// The identifier must be <seealso cref="HolidayCalendarId#resolve(ReferenceData) resolved"/>
	/// to a <seealso cref="HolidayCalendar"/> before the holiday data methods can be accessed.
	/// See <seealso cref="HolidayCalendarIds"/> for a standard set of identifiers available in <seealso cref="ReferenceData#standard()"/>.
	/// </para>
	/// <para>
	/// All implementations of this interface must be immutable and thread-safe.
	/// 
	/// </para>
	/// </summary>
	/// <seealso cref= ImmutableHolidayCalendar </seealso>
	public interface HolidayCalendar : Named
	{

	  /// <summary>
	  /// Checks if the specified date is a holiday.
	  /// <para>
	  /// This is the opposite of <seealso cref="#isBusinessDay(LocalDate)"/>.
	  /// A weekend is treated as a holiday.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to check </param>
	  /// <returns> true if the specified date is a holiday </returns>
	  /// <exception cref="IllegalArgumentException"> if the date is outside the supported range </exception>
	  bool isHoliday(LocalDate date);

	  /// <summary>
	  /// Checks if the specified date is a business day.
	  /// <para>
	  /// This is the opposite of <seealso cref="#isHoliday(LocalDate)"/>.
	  /// A weekend is treated as a holiday.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to check </param>
	  /// <returns> true if the specified date is a business day </returns>
	  /// <exception cref="IllegalArgumentException"> if the date is outside the supported range </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default boolean isBusinessDay(java.time.LocalDate date)
	//  {
	//	return !isHoliday(date);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an adjuster that changes the date.
	  /// <para>
	  /// The adjuster is intended to be used with the method <seealso cref="Temporal#with(TemporalAdjuster)"/>.
	  /// For example:
	  /// <pre>
	  /// threeDaysLater = date.with(businessDays.adjustBy(3));
	  /// twoDaysEarlier = date.with(businessDays.adjustBy(-2));
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amount">  the number of business days to adjust by </param>
	  /// <returns> the first business day after this one </returns>
	  /// <exception cref="IllegalArgumentException"> if the calculation is outside the supported range </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.temporal.TemporalAdjuster adjustBy(int amount)
	//  {
	//	return TemporalAdjusters.ofDateAdjuster(date -> shift(date, amount));
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Shifts the date by the specified number of business days.
	  /// <para>
	  /// If the amount is zero, the input date is returned.
	  /// If the amount is positive, later business days are chosen.
	  /// If the amount is negative, earlier business days are chosen.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to adjust </param>
	  /// <param name="amount">  the number of business days to adjust by </param>
	  /// <returns> the shifted date </returns>
	  /// <exception cref="IllegalArgumentException"> if the calculation is outside the supported range </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.LocalDate shift(java.time.LocalDate date, int amount)
	//  {
	//	LocalDate adjusted = date;
	//	if (amount > 0)
	//	{
	//	  for (int i = 0; i < amount; i++)
	//	  {
	//		adjusted = next(adjusted);
	//	  }
	//	}
	//	else if (amount < 0)
	//	{
	//	  for (int i = 0; i > amount; i--)
	//	  {
	//		adjusted = previous(adjusted);
	//	  }
	//	}
	//	return adjusted;
	//  }

	  /// <summary>
	  /// Finds the next business day, always returning a later date.
	  /// <para>
	  /// Given a date, this method returns the next business day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to adjust </param>
	  /// <returns> the first business day after the input date </returns>
	  /// <exception cref="IllegalArgumentException"> if the calculation is outside the supported range </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.LocalDate next(java.time.LocalDate date)
	//  {
	//	LocalDate next = plusDays(date, 1);
	//	return isHoliday(next) ? next(next) : next;
	//  }

	  /// <summary>
	  /// Finds the next business day, returning the input date if it is a business day.
	  /// <para>
	  /// Given a date, this method returns a business day.
	  /// If the input date is a business day, it is returned.
	  /// Otherwise, the next business day is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to adjust </param>
	  /// <returns> the input date if it is a business day, or the next business day </returns>
	  /// <exception cref="IllegalArgumentException"> if the calculation is outside the supported range </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.LocalDate nextOrSame(java.time.LocalDate date)
	//  {
	//	return isHoliday(date) ? next(date) : date;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the previous business day, always returning an earlier date.
	  /// <para>
	  /// Given a date, this method returns the previous business day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to adjust </param>
	  /// <returns> the first business day before the input date </returns>
	  /// <exception cref="IllegalArgumentException"> if the calculation is outside the supported range </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.LocalDate previous(java.time.LocalDate date)
	//  {
	//	LocalDate previous = plusDays(date, -1);
	//	return isHoliday(previous) ? previous(previous) : previous;
	//  }

	  /// <summary>
	  /// Finds the previous business day, returning the input date if it is a business day.
	  /// <para>
	  /// Given a date, this method returns a business day.
	  /// If the input date is a business day, it is returned.
	  /// Otherwise, the previous business day is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to adjust </param>
	  /// <returns> the input date if it is a business day, or the previous business day </returns>
	  /// <exception cref="IllegalArgumentException"> if the calculation is outside the supported range </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.LocalDate previousOrSame(java.time.LocalDate date)
	//  {
	//	return isHoliday(date) ? previous(date) : date;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the next business day within the month, returning the input date if it is a business day,
	  /// or the last business day of the month if the next business day is in a different month.
	  /// <para>
	  /// Given a date, this method returns a business day.
	  /// If the input date is a business day, it is returned.
	  /// If the next business day is within the same month, it is returned.
	  /// Otherwise, the last business day of the month is returned.
	  /// </para>
	  /// <para>
	  /// Note that the result of this method may be earlier than the input date.
	  /// </para>
	  /// <para>
	  /// This corresponds to the <seealso cref="BusinessDayConventions#MODIFIED_FOLLOWING modified following"/>
	  /// business day convention.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to adjust </param>
	  /// <returns> the input date if it is a business day, the next business day if within the same month
	  ///   or the last business day of the month </returns>
	  /// <exception cref="IllegalArgumentException"> if the calculation is outside the supported range </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.LocalDate nextSameOrLastInMonth(java.time.LocalDate date)
	//  {
	//	LocalDate nextOrSame = nextOrSame(date);
	//	return (nextOrSame.getMonthValue() != date.getMonthValue() ? previous(date) : nextOrSame);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if the specified date is the last business day of the month.
	  /// <para>
	  /// This returns true if the date specified is the last valid business day of the month.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to check </param>
	  /// <returns> true if the specified date is the last business day of the month </returns>
	  /// <exception cref="IllegalArgumentException"> if the date is outside the supported range </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default boolean isLastBusinessDayOfMonth(java.time.LocalDate date)
	//  {
	//	return isBusinessDay(date) && next(date).getMonthValue() != date.getMonthValue();
	//  }

	  /// <summary>
	  /// Calculates the last business day of the month.
	  /// <para>
	  /// Given a date, this method returns the date of the last business day of the month.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to check </param>
	  /// <returns> true if the specified date is the last business day of the month </returns>
	  /// <exception cref="IllegalArgumentException"> if the date is outside the supported range </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.LocalDate lastBusinessDayOfMonth(java.time.LocalDate date)
	//  {
	//	return previousOrSame(date.withDayOfMonth(date.lengthOfMonth()));
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the number of business days between two dates.
	  /// <para>
	  /// This calculates the number of business days within the range.
	  /// If the dates are equal, zero is returned.
	  /// If the end is before the start, an exception is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="startInclusive">  the start date </param>
	  /// <param name="endExclusive">  the end date </param>
	  /// <returns> the total number of business days between the start and end date </returns>
	  /// <exception cref="IllegalArgumentException"> if either date is outside the supported range </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default int daysBetween(java.time.LocalDate startInclusive, java.time.LocalDate endExclusive)
	//  {
	//	ArgChecker.inOrderOrEqual(startInclusive, endExclusive, "startInclusive", "endExclusive");
	//	return Math.toIntExact(LocalDateUtils.stream(startInclusive, endExclusive).filter(this::isBusinessDay).count());
	//  }

	  /// <summary>
	  /// Gets the stream of business days between the two dates.
	  /// <para>
	  /// This method will treat weekends as holidays.
	  /// If the dates are equal, an empty stream is returned.
	  /// If the end is before the start, an exception is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="startInclusive">  the start date </param>
	  /// <param name="endExclusive">  the end date </param>
	  /// <returns> the stream of business days </returns>
	  /// <exception cref="IllegalArgumentException"> if either date is outside the supported range </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.util.stream.Stream<java.time.LocalDate> businessDays(java.time.LocalDate startInclusive, java.time.LocalDate endExclusive)
	//  {
	//	ArgChecker.inOrderOrEqual(startInclusive, endExclusive, "startInclusive", "endExclusive");
	//	return LocalDateUtils.stream(startInclusive, endExclusive).filter(this::isBusinessDay);
	//  }

	  /// <summary>
	  /// Gets the stream of holidays between the two dates.
	  /// <para>
	  /// This method will treat weekends as holidays.
	  /// If the dates are equal, an empty stream is returned.
	  /// If the end is before the start, an exception is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="startInclusive">  the start date </param>
	  /// <param name="endExclusive">  the end date </param>
	  /// <returns> the stream of holidays </returns>
	  /// <exception cref="IllegalArgumentException"> if either date is outside the supported range </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.util.stream.Stream<java.time.LocalDate> holidays(java.time.LocalDate startInclusive, java.time.LocalDate endExclusive)
	//  {
	//	ArgChecker.inOrderOrEqual(startInclusive, endExclusive, "startInclusive", "endExclusive");
	//	return LocalDateUtils.stream(startInclusive, endExclusive).filter(this::isHoliday);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Combines this holiday calendar with another.
	  /// <para>
	  /// The resulting calendar will declare a day as a business day if it is a
	  /// business day in both source calendars.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other holiday calendar </param>
	  /// <returns> the combined calendar </returns>
	  /// <exception cref="IllegalArgumentException"> if unable to combine the calendars </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default HolidayCalendar combinedWith(HolidayCalendar other)
	//  {
	//	if (this.equals(other))
	//	{
	//	  return this;
	//	}
	//	if (other == HolidayCalendars.NO_HOLIDAYS)
	//	{
	//	  return this;
	//	}
	//	return new CombinedHolidayCalendar(this, other);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the identifier for the calendar.
	  /// <para>
	  /// This identifier is used to locate the index in <seealso cref="ReferenceData"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the identifier </returns>
	  HolidayCalendarId Id {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name that identifies this calendar.
	  /// <para>
	  /// This is the name associated with the <seealso cref="HolidayCalendarId identifier"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the name </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default String getName()
	//  {
	//	return getId().getName();
	//  }

	}

}