/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using Tenor = com.opengamma.strata.basics.date.Tenor;

	/// <summary>
	/// A index of interest rates, such as an Overnight or Inter-Bank rate.
	/// <para>
	/// Many financial products require knowledge of interest rate indices, such as Libor.
	/// Implementations of this interface define these indices.
	/// See <seealso cref="IborIndex"/> and <seealso cref="OvernightIndex"/>.
	/// </para>
	/// <para>
	/// All implementations of this interface must be immutable and thread-safe.
	/// </para>
	/// </summary>
	public interface RateIndex : FloatingRateIndex
	{

	  /// <summary>
	  /// Gets the calendar that determines which dates are fixing dates.
	  /// <para>
	  /// The rate will be fixed on each business day in this calendar.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the calendar used to determine the fixing dates of the index </returns>
	  HolidayCalendarId FixingCalendar {get;}

	  /// <summary>
	  /// Gets the tenor of the index.
	  /// </summary>
	  /// <returns> the tenor </returns>
	  Tenor Tenor {get;}

	}

}