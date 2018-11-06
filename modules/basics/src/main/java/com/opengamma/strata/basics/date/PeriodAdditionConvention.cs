/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{

	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;
	using Named = com.opengamma.strata.collect.named.Named;

	/// <summary>
	/// A convention defining how a period is added to a date.
	/// <para>
	/// The purpose of this convention is to define how to handle the addition of a period.
	/// The default implementations include two different end-of-month rules.
	/// The convention is generally only applicable for month-based periods.
	/// </para>
	/// <para>
	/// The most common implementations are provided in <seealso cref="PeriodAdditionConventions"/>.
	/// Additional implementations may be added by implementing this interface.
	/// </para>
	/// <para>
	/// All implementations of this interface must be immutable and thread-safe.
	/// </para>
	/// </summary>
	public interface PeriodAdditionConvention : Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the addition convention </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static PeriodAdditionConvention of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static PeriodAdditionConvention of(String uniqueName)
	//  {
	//	return extendedEnum().lookup(uniqueName);
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
//	  public static com.opengamma.strata.collect.named.ExtendedEnum<PeriodAdditionConvention> extendedEnum()
	//  {
	//	return PeriodAdditionConventions.ENUM_LOOKUP;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adjusts the base date, adding the period and applying the convention rule.
	  /// <para>
	  /// The adjustment occurs in two steps.
	  /// First, the period is added to the based date to create the end date.
	  /// Second, the end date is adjusted by the convention rules.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="baseDate">  the base date to add to </param>
	  /// <param name="period">  the period to add </param>
	  /// <param name="calendar">  the holiday calendar to use </param>
	  /// <returns> the adjusted date </returns>
	  LocalDate adjust(LocalDate baseDate, Period period, HolidayCalendar calendar);

	  /// <summary>
	  /// Checks whether the convention requires a month-based period.
	  /// <para>
	  /// A month-based period contains only months and/or years, and not days.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if the convention requires a month-based period </returns>
	  bool MonthBased {get;}

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