/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{

	/// <summary>
	/// Functional interface that can adjust a date.
	/// <para>
	/// This extends <seealso cref="TemporalAdjuster"/> for those cases where the temporal to
	/// be adjusted is an ISO-8601 date.
	/// </para>
	/// </summary>
	public interface DateAdjuster : TemporalAdjuster
	{

	  /// <summary>
	  /// Adjusts the date according to the rules of the implementation.
	  /// <para>
	  /// Implementations must specify how the date is adjusted.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to adjust </param>
	  /// <returns> the adjusted date </returns>
	  /// <exception cref="DateTimeException"> if unable to make the adjustment </exception>
	  /// <exception cref="ArithmeticException"> if numeric overflow occurs </exception>
	  LocalDate adjust(LocalDate date);

	  /// <summary>
	  /// Adjusts the temporal according to the rules of the implementation.
	  /// <para>
	  /// This method implements <seealso cref="TemporalAdjuster"/> by calling <seealso cref="#adjust(LocalDate)"/>.
	  /// Note that conversion to {@code LocalDate} ignores the calendar system
	  /// of the input, which is the desired behaviour in this case.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="temporal">  the temporal to adjust </param>
	  /// <returns> the adjusted temporal </returns>
	  /// <exception cref="DateTimeException"> if unable to make the adjustment </exception>
	  /// <exception cref="ArithmeticException"> if numeric overflow occurs </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.temporal.Temporal adjustInto(java.time.temporal.Temporal temporal)
	//  {
	//	// conversion to LocalDate ensures that other calendar systems are ignored
	//	return temporal.with(adjust(LocalDate.from(temporal)));
	//  }

	}

}