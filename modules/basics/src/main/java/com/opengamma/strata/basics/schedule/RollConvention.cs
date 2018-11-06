/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.schedule
{

	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using Dom = com.opengamma.strata.basics.schedule.DayRollConventions.Dom;
	using Dow = com.opengamma.strata.basics.schedule.DayRollConventions.Dow;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;
	using Named = com.opengamma.strata.collect.named.Named;

	/// <summary>
	/// A convention defining how to roll dates.
	/// <para>
	/// A <seealso cref="PeriodicSchedule periodic schedule"/> is determined using a periodic frequency.
	/// When applying the frequency, the roll convention is used to fine tune the dates.
	/// This might involve selecting the last day of the month, or the third Wednesday.
	/// </para>
	/// <para>
	/// To get the next date in the schedule, take the base date and the
	/// <seealso cref="Frequency periodic frequency"/>. Once this date is calculated,
	/// the roll convention is applied to produce the next schedule date.
	/// </para>
	/// <para>
	/// The most common implementations are provided as constants on <seealso cref="RollConventions"/>.
	/// Additional implementations may be added by implementing this interface.
	/// </para>
	/// <para>
	/// All implementations of this interface must be immutable and thread-safe.
	/// </para>
	/// </summary>
	public interface RollConvention : Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the roll convention </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static RollConvention of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static RollConvention of(String uniqueName)
	//  {
	//	ArgChecker.notNull(uniqueName, "uniqueName");
	//	return extendedEnum().lookup(uniqueName);
	//  }

	  /// <summary>
	  /// Obtains an instance from the day-of-month.
	  /// <para>
	  /// This convention will adjust the input date to the specified day-of-month.
	  /// The year and month of the result date will be the same as the input date.
	  /// It is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// <para>
	  /// If the month being adjusted has a length less than the requested day-of-month
	  /// then the last valid day-of-month will be chosen. As such, passing 31 to this
	  /// method is equivalent to selecting the end-of-month convention.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="dayOfMonth">  the day-of-month, from 1 to 31 </param>
	  /// <returns> the roll convention </returns>
	  /// <exception cref="IllegalArgumentException"> if the day-of-month is invalid </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static RollConvention ofDayOfMonth(int dayOfMonth)
	//  {
	//	return Dom.of(dayOfMonth);
	//  }

	  /// <summary>
	  /// Obtains an instance from the day-of-week.
	  /// <para>
	  /// This convention will adjust the input date to the specified day-of-week.
	  /// It is intended for use with periods that are a multiple of weeks.
	  /// </para>
	  /// <para>
	  /// In {@code adjust()}, if the input date is not the required day-of-week,
	  /// then the next occurrence of the day-of-week is selected, up to 6 days later.
	  /// </para>
	  /// <para>
	  /// In {@code next()}, the day-of-week is selected after the frequency is added.
	  /// If the calculated date is not the required day-of-week, then the next occurrence
	  /// of the day-of-week is selected, up to 6 days later.
	  /// </para>
	  /// <para>
	  /// In {@code previous()}, the day-of-week is selected after the frequency is subtracted.
	  /// If the calculated date is not the required day-of-week, then the previous occurrence
	  /// of the day-of-week is selected, up to 6 days earlier.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="dayOfWeek">  the day-of-week </param>
	  /// <returns> the roll convention </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static RollConvention ofDayOfWeek(java.time.DayOfWeek dayOfWeek)
	//  {
	//	return Dow.of(dayOfWeek);
	//  }

	  /// <summary>
	  /// Gets the extended enum helper.
	  /// <para>
	  /// This helper allows instances of the convention to be looked up.
	  /// It also provides the complete set of available instances.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the extended enum helper </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static com.opengamma.strata.collect.named.ExtendedEnum<RollConvention> extendedEnum()
	//  {
	//	return RollConventions.ENUM_LOOKUP;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adjusts the date according to the rules of the roll convention.
	  /// <para>
	  /// See the description of each roll convention to understand the rule applied.
	  /// </para>
	  /// <para>
	  /// It is recommended to use {@code next()} and {@code previous()} rather than
	  /// directly using this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to adjust </param>
	  /// <returns> the adjusted temporal </returns>
	  LocalDate adjust(LocalDate date);

	  /// <summary>
	  /// Checks if the date matches the rules of the roll convention.
	  /// <para>
	  /// See the description of each roll convention to understand the rule applied.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to check </param>
	  /// <returns> true if the date matches this convention </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default boolean matches(java.time.LocalDate date)
	//  {
	//	ArgChecker.notNull(date, "date");
	//	return date.equals(adjust(date));
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the next date in the sequence after the input date.
	  /// <para>
	  /// This takes the input date, adds the periodic frequency and adjusts the date
	  /// as necessary to match the roll convention rules.
	  /// The result will always be after the input date.
	  /// </para>
	  /// <para>
	  /// The default implementation is suitable for month-based conventions.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to adjust </param>
	  /// <param name="periodicFrequency">  the periodic frequency of the schedule </param>
	  /// <returns> the adjusted date </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.LocalDate next(java.time.LocalDate date, Frequency periodicFrequency)
	//  {
	//	ArgChecker.notNull(date, "date");
	//	ArgChecker.notNull(periodicFrequency, "periodicFrequency");
	//	LocalDate calculated = adjust(date.plus(periodicFrequency));
	//	if (calculated.isAfter(date) == false)
	//	{
	//	  calculated = adjust(date.plusMonths(1));
	//	}
	//	return calculated;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the previous date in the sequence after the input date.
	  /// <para>
	  /// This takes the input date, subtracts the periodic frequency and adjusts the date
	  /// as necessary to match the roll convention rules.
	  /// The result will always be before the input date.
	  /// </para>
	  /// <para>
	  /// The default implementation is suitable for month-based conventions.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to adjust </param>
	  /// <param name="periodicFrequency">  the periodic frequency of the schedule </param>
	  /// <returns> the adjusted date </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.LocalDate previous(java.time.LocalDate date, Frequency periodicFrequency)
	//  {
	//	ArgChecker.notNull(date, "date");
	//	ArgChecker.notNull(periodicFrequency, "periodicFrequency");
	//	LocalDate calculated = adjust(date.minus(periodicFrequency));
	//	if (calculated.isBefore(date) == false)
	//	{
	//	  calculated = adjust(date.minusMonths(1));
	//	}
	//	return calculated;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the day-of-month that the roll convention implies.
	  /// <para>
	  /// This extracts the day-of-month for simple roll conventions.
	  /// The numeric roll conventions will return their day-of-month.
	  /// The 'EOM' convention will return 31.
	  /// All other conventions will return zero.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the day-of-month that the roll convention implies, zero if not applicable </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default int getDayOfMonth()
	//  {
	//	return 0;
	//  }

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

	}

}