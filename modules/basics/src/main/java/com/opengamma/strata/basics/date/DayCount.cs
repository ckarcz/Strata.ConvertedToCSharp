/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{

	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;
	using Named = com.opengamma.strata.collect.named.Named;

	/// <summary>
	/// A convention defining how to calculate fractions of a year.
	/// <para>
	/// The purpose of this convention is to define how to convert dates into numeric year fractions.
	/// The is of use when calculating accrued interest over time.
	/// </para>
	/// <para>
	/// The most common implementations are provided in <seealso cref="DayCounts"/>.
	/// Additional implementations may be added by implementing this interface.
	/// </para>
	/// <para>
	/// All implementations of this interface must be immutable and thread-safe.
	/// </para>
	/// </summary>
	public interface DayCount : Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the day count </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static DayCount of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static DayCount of(String uniqueName)
	//  {
	//	return extendedEnum().lookup(uniqueName);
	//  }

	  /// <summary>
	  /// Obtains an instance of the 'Bus/252' day count based on a specific calendar.
	  /// <para>
	  /// The 'Bus/252' day count is unusual in that it relies on a specific holiday calendar.
	  /// The calendar is stored within the day count.
	  /// </para>
	  /// <para>
	  /// To avoid widespread complexity in the system, the holiday calendar associated
	  /// with 'Bus/252' holiday calendars is looked up using the
	  /// <seealso cref="ReferenceData#standard() standard reference data"/>.
	  /// </para>
	  /// <para>
	  /// This day count is typically used in Brazil.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="calendar">  the holiday calendar </param>
	  /// <returns> the day count </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static DayCount ofBus252(HolidayCalendarId calendar)
	//  {
	//	return Business252DayCount.INSTANCE.of(calendar.resolve(ReferenceData.standard()));
	//  }

	  /// <summary>
	  /// Gets the extended enum helper.
	  /// <para>
	  /// This helper allows instances of the day count to be looked up.
	  /// It also provides the complete set of available instances.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the extended enum helper </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static com.opengamma.strata.collect.named.ExtendedEnum<DayCount> extendedEnum()
	//  {
	//	return DayCounts.ENUM_LOOKUP;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the year fraction between the specified dates.
	  /// <para>
	  /// Given two dates, this method returns the fraction of a year between these
	  /// dates according to the convention. The dates must be in order.
	  /// </para>
	  /// <para>
	  /// This uses a simple <seealso cref="ScheduleInfo"/> which has the end-of-month convention
	  /// set to true, but throws an exception for other methods.
	  /// Certain implementations of {@code DayCount} need the missing information,
	  /// and thus will throw an exception.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="firstDate">  the first date </param>
	  /// <param name="secondDate">  the second date, on or after the first date </param>
	  /// <returns> the year fraction </returns>
	  /// <exception cref="IllegalArgumentException"> if the dates are not in order </exception>
	  /// <exception cref="UnsupportedOperationException"> if the year fraction cannot be obtained </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default double yearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate)
	//  {
	//	return yearFraction(firstDate, secondDate, DayCounts.SIMPLE_SCHEDULE_INFO);
	//  }

	  /// <summary>
	  /// Gets the year fraction between the specified dates.
	  /// <para>
	  /// Given two dates, this method returns the fraction of a year between these
	  /// dates according to the convention. The dates must be in order.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="firstDate">  the first date </param>
	  /// <param name="secondDate">  the second date, on or after the first date </param>
	  /// <param name="scheduleInfo">  the schedule information </param>
	  /// <returns> the year fraction, zero or greater </returns>
	  /// <exception cref="IllegalArgumentException"> if the dates are not in order </exception>
	  /// <exception cref="UnsupportedOperationException"> if the year fraction cannot be obtained </exception>
	  double yearFraction(LocalDate firstDate, LocalDate secondDate, DayCount_ScheduleInfo scheduleInfo);

	  /// <summary>
	  /// Gets the relative year fraction between the specified dates.
	  /// <para>
	  /// Given two dates, this method returns the fraction of a year between these
	  /// dates according to the convention.
	  /// The result of this method will be negative if the first date is after the second date.
	  /// The result is calculated using <seealso cref="#yearFraction(LocalDate, LocalDate, ScheduleInfo)"/>.
	  /// </para>
	  /// <para>
	  /// This uses a simple <seealso cref="ScheduleInfo"/> which has the end-of-month convention
	  /// set to true, but throws an exception for other methods.
	  /// Certain implementations of {@code DayCount} need the missing information,
	  /// and thus will throw an exception.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="firstDate">  the first date </param>
	  /// <param name="secondDate">  the second date, which may be before the first date </param>
	  /// <returns> the year fraction, may be negative </returns>
	  /// <exception cref="UnsupportedOperationException"> if the year fraction cannot be obtained </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default double relativeYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate)
	//  {
	//	return relativeYearFraction(firstDate, secondDate, DayCounts.SIMPLE_SCHEDULE_INFO);
	//  }

	  /// <summary>
	  /// Gets the relative year fraction between the specified dates.
	  /// <para>
	  /// Given two dates, this method returns the fraction of a year between these
	  /// dates according to the convention.
	  /// The result of this method will be negative if the first date is after the second date.
	  /// The result is calculated using <seealso cref="#yearFraction(LocalDate, LocalDate, ScheduleInfo)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="firstDate">  the first date </param>
	  /// <param name="secondDate">  the second date, which may be before the first date </param>
	  /// <param name="scheduleInfo">  the schedule information </param>
	  /// <returns> the year fraction, may be negative </returns>
	  /// <exception cref="UnsupportedOperationException"> if the year fraction cannot be obtained </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default double relativeYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, DayCount_ScheduleInfo scheduleInfo)
	//  {
	//	if (secondDate.isBefore(firstDate))
	//	{
	//	  return -yearFraction(secondDate, firstDate, scheduleInfo);
	//	}
	//	return yearFraction(firstDate, secondDate, scheduleInfo);
	//  }

	  /// <summary>
	  /// Calculates the number of days between the specified dates using the rules of this day count.
	  /// <para>
	  /// A day count is typically defines as a count of days divided by a year estimate.
	  /// This method returns the count of days, which is the numerator of the division.
	  /// For example, the 'Act/Act' day count will return the actual number of days between
	  /// the two dates, but the '30/360 ISDA' will return a value based on 30 day months.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="firstDate">  the first date </param>
	  /// <param name="secondDate">  the second date, which may be before the first date </param>
	  /// <returns> the number of days, as determined by the day count </returns>
	  int days(LocalDate firstDate, LocalDate secondDate);

	  /// <summary>
	  /// Gets the name that uniquely identifies this convention.
	  /// <para>
	  /// This name is used in serialization and can be parsed using <seealso cref="#of(String)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the unique name </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ToString @Override public abstract String getName();
	  string Name {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Information about the schedule necessary to calculate the day count.
	  /// <para>
	  /// Some <seealso cref="DayCount"/> implementations require additional information about the schedule.
	  /// Implementations of this interface provide that information.
	  /// </para>
	  /// </summary>

	}

	  public interface DayCount_ScheduleInfo
	  {

	/// <summary>
	/// Gets the start date of the schedule.
	/// <para>
	/// The first date in the schedule.
	/// If the schedule adjusts for business days, then this is the adjusted date.
	/// </para>
	/// <para>
	/// This throws an exception by default.
	/// 
	/// </para>
	/// </summary>
	/// <returns> the schedule start date </returns>
	/// <exception cref="UnsupportedOperationException"> if the date cannot be obtained </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	public default java.time.LocalDate getStartDate()
	//{
	//  throw new UnsupportedOperationException("The start date of the schedule is required");
	//}

	/// <summary>
	/// Gets the end date of the schedule.
	/// <para>
	/// The last date in the schedule.
	/// If the schedule adjusts for business days, then this is the adjusted date.
	/// </para>
	/// <para>
	/// This throws an exception by default.
	/// 
	/// </para>
	/// </summary>
	/// <returns> the schedule end date </returns>
	/// <exception cref="UnsupportedOperationException"> if the date cannot be obtained </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	public default java.time.LocalDate getEndDate()
	//{
	//  throw new UnsupportedOperationException("The end date of the schedule is required");
	//}

	/// <summary>
	/// Gets the end date of the schedule period.
	/// <para>
	/// This is called when a day count requires the end date of the schedule period.
	/// </para>
	/// <para>
	/// This throws an exception by default.
	/// 
	/// </para>
	/// </summary>
	/// <param name="date">  the date to find the period end date for </param>
	/// <returns> the period end date </returns>
	/// <exception cref="UnsupportedOperationException"> if the date cannot be obtained </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	public default java.time.LocalDate getPeriodEndDate(java.time.LocalDate date)
	//{
	//  throw new UnsupportedOperationException("The end date of the schedule period is required");
	//}

	/// <summary>
	/// Gets the periodic frequency of the schedule period.
	/// <para>
	/// This is called when a day count requires the periodic frequency of the schedule.
	/// </para>
	/// <para>
	/// This throws an exception by default.
	/// 
	/// </para>
	/// </summary>
	/// <returns> the periodic frequency </returns>
	/// <exception cref="UnsupportedOperationException"> if the frequency cannot be obtained </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	public default com.opengamma.strata.basics.schedule.Frequency getFrequency()
	//{
	//  throw new UnsupportedOperationException("The frequency of the schedule is required");
	//}

	/// <summary>
	/// Checks if the end of month convention is in use.
	/// <para>
	/// This is called when a day count needs to know whether the end-of-month convention is in use.
	/// </para>
	/// <para>
	/// This is true by default.
	/// 
	/// </para>
	/// </summary>
	/// <returns> true if the end of month convention is in use </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	public default boolean isEndOfMonthConvention()
	//{
	//  return true;
	//}
	  }

}