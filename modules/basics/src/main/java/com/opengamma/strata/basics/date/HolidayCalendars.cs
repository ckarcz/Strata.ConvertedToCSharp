/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
	using Splitter = com.google.common.@base.Splitter;
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Constants and implementations for standard holiday calendars.
	/// <para>
	/// The purpose of each holiday calendar is to define whether a date is a holiday or a business day.
	/// The standard holiday calendar data is provided by direct research and is not derived
	/// from a vendor of holiday calendar data. The implementation is defined by {@code HolidayCalendar.ini},
	/// The data may or may not be sufficient for your production needs.
	/// </para>
	/// <para>
	/// Applications should refer to holidays using <seealso cref="HolidayCalendarId"/>.
	/// The identifier must be <seealso cref="HolidayCalendarId#resolve(ReferenceData) resolved"/>
	/// to a <seealso cref="HolidayCalendar"/> before holidays can be accessed.
	/// </para>
	/// </summary>
	public sealed class HolidayCalendars
	{

	  /// <summary>
	  /// Decorates a {@code ReferenceData} instance such that all requests for
	  /// a {@code HolidayCalendarId} will return a value.
	  /// <para>
	  /// If the <seealso cref="HolidayCalendarId"/> is not found in the underlying reference data,
	  /// an instance with Saturday/Sunday holidays will be returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="underlying">  the underlying instance </param>
	  /// <returns> the holiday safe reference data </returns>
	  public static ReferenceData defaultingReferenceData(ReferenceData underlying)
	  {
		return new HolidaySafeReferenceData(underlying);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// An instance declaring no holidays and no weekends.
	  /// <para>
	  /// This calendar has the effect of making every day a business day.
	  /// It is often used to indicate that a holiday calendar does not apply.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendar NO_HOLIDAYS = NoHolidaysCalendar.INSTANCE;
	  /// <summary>
	  /// An instance declaring all days as business days except Saturday/Sunday weekends.
	  /// <para>
	  /// This calendar is mostly useful in testing scenarios.
	  /// Note that not all countries use Saturday and Sunday weekends.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendar SAT_SUN = WeekendHolidayCalendar.SAT_SUN;
	  /// <summary>
	  /// An instance declaring all days as business days except Friday/Saturday weekends.
	  /// <para>
	  /// This calendar is mostly useful in testing scenarios.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendar FRI_SAT = WeekendHolidayCalendar.FRI_SAT;
	  /// <summary>
	  /// An instance declaring all days as business days except Thursday/Friday weekends.
	  /// <para>
	  /// This calendar is mostly useful in testing scenarios.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendar THU_FRI = WeekendHolidayCalendar.THU_FRI;

	  // This constant must be after the constants above in the source file.
	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  private static readonly ExtendedEnum<HolidayCalendar> ENUM_LOOKUP = ExtendedEnum.of(typeof(HolidayCalendar));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the set of standard holiday calendars.
	  /// <para>
	  /// The unique name identifies the calendar in the <i>standard</i> source of calendars.
	  /// The standard source is loaded at startup based on the {@code HolidayCalendar.ini} file.
	  /// </para>
	  /// <para>
	  /// Applications should generally avoid using this method.
	  /// Instead, applications should refer to holidays using <seealso cref="HolidayCalendarId"/>,
	  /// resolving them using a <seealso cref="ReferenceData"/>.
	  /// </para>
	  /// <para>
	  /// It is possible to combine two or more calendars using the '+' symbol.
	  /// For example, 'GBLO+USNY' will combine the separate 'GBLO' and 'USNY' calendars.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="uniqueName">  the unique name of the calendar </param>
	  /// <returns> the holiday calendar </returns>
	  public static HolidayCalendar of(string uniqueName)
	  {
		if (uniqueName.Contains("+"))
		{
		  return Splitter.on('+').splitToList(uniqueName).Select(HolidayCalendars.of).Aggregate(NO_HOLIDAYS, HolidayCalendar.combinedWith);
		}
		return ENUM_LOOKUP.lookup(uniqueName);
	  }

	  /// <summary>
	  /// Gets the extended enum helper.
	  /// <para>
	  /// This helper allows instances of the calendar to be looked up.
	  /// It also provides the complete set of available instances.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the extended enum helper </returns>
	  public static ExtendedEnum<HolidayCalendar> extendedEnum()
	  {
		return HolidayCalendars.ENUM_LOOKUP;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private HolidayCalendars()
	  {
	  }

	}

}