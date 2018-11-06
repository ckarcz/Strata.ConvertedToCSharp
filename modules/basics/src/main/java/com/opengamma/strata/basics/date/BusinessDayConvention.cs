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
	/// A convention defining how to adjust a date if it falls on a day other than a business day.
	/// <para>
	/// The purpose of this convention is to define how to handle non-business days.
	/// When processing dates in finance, it is typically intended that non-business days,
	/// such as weekends and holidays, are converted to a nearby valid business day.
	/// The convention, in conjunction with a <seealso cref="HolidayCalendar holiday calendar"/>,
	/// defines exactly how the adjustment should be made.
	/// </para>
	/// <para>
	/// The most common implementations are provided in <seealso cref="BusinessDayConventions"/>.
	/// Additional implementations may be added by implementing this interface.
	/// </para>
	/// <para>
	/// All implementations of this interface must be immutable and thread-safe.
	/// </para>
	/// </summary>
	public interface BusinessDayConvention : Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the business convention </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static BusinessDayConvention of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static BusinessDayConvention of(String uniqueName)
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
//	  public static com.opengamma.strata.collect.named.ExtendedEnum<BusinessDayConvention> extendedEnum()
	//  {
	//	return BusinessDayConventions.ENUM_LOOKUP;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adjusts the date as necessary if it is not a business day.
	  /// <para>
	  /// If the date is a business day it will be returned unaltered.
	  /// If the date is not a business day, the convention will be applied.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to adjust </param>
	  /// <param name="calendar">  the calendar that defines holidays and business days </param>
	  /// <returns> the adjusted date </returns>
	  LocalDate adjust(LocalDate date, HolidayCalendar calendar);

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