/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{

	/// <summary>
	/// Identifiers for common holiday calendars.
	/// <para>
	/// The constants defined here are identifiers, used to locate instances of
	/// <seealso cref="HolidayCalendar"/> from <seealso cref="ReferenceData"/>.
	/// </para>
	/// <para>
	/// All the constants defined here will be available from <seealso cref="ReferenceData#standard()"/>.
	/// The associated holiday data may or may not be sufficient for your production needs.
	/// </para>
	/// <para>
	/// The standard holiday data was obtained by direct research - it was not derived from a vendor
	/// of holiday calendar data. Two approaches are available to add or change the data.
	/// Firstly, applications can provide their own {@code ReferenceData} implementation, mapping
	/// the identifier to any data desired. Secondly, the standard data can be amended by following
	/// the instructions in {@code HolidayCalendar.ini}.
	/// </para>
	/// </summary>
	public sealed class HolidayCalendarIds
	{

	  /// <summary>
	  /// An identifier for a calendar declaring no holidays and no weekends, with code 'NoHolidays'.
	  /// <para>
	  /// This calendar has the effect of making every day a business day.
	  /// It is often used to indicate that a holiday calendar does not apply.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId NO_HOLIDAYS = HolidayCalendarId.of("NoHolidays");
	  /// <summary>
	  /// An identifier for a calendar declaring all days as business days
	  /// except Saturday/Sunday weekends, with code 'SatSun'.
	  /// <para>
	  /// This calendar is mostly useful in testing scenarios.
	  /// Note that not all countries use Saturday and Sunday weekends.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId SAT_SUN = HolidayCalendarId.of("Sat/Sun");
	  /// <summary>
	  /// An identifier for a calendar declaring all days as business days
	  /// except Friday/Saturday weekends, with code 'FriSat'.
	  /// <para>
	  /// This calendar is mostly useful in testing scenarios.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId FRI_SAT = HolidayCalendarId.of("Fri/Sat");
	  /// <summary>
	  /// An identifier for a calendar declaring all days as business days
	  /// except Thursday/Friday weekends, with code 'ThuFri'.
	  /// <para>
	  /// This calendar is mostly useful in testing scenarios.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId THU_FRI = HolidayCalendarId.of("Thu/Fri");

	  /// <summary>
	  /// An identifier for the holiday calendar of London, United Kingdom, with code 'GBLO'.
	  /// <para>
	  /// This constant references the calendar for London bank holidays.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId GBLO = HolidayCalendarId.of("GBLO");
	  /// <summary>
	  /// An identifier for the holiday calendar of Paris, France, with code 'FRPA'.
	  /// <para>
	  /// This constant references the calendar for Paris public holidays.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId FRPA = HolidayCalendarId.of("FRPA");
	  /// <summary>
	  /// An identifier for the holiday calendar of Frankfurt, Germany, with code 'DEFR'.
	  /// <para>
	  /// This constant references the calendar for Frankfurt public holidays.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId DEFR = HolidayCalendarId.of("DEFR");
	  /// <summary>
	  /// An identifier for the holiday calendar of Zurich, Switzerland, with code 'EUTA'.
	  /// <para>
	  /// This constant references the calendar for Zurich public holidays.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId CHZU = HolidayCalendarId.of("CHZU");
	  /// <summary>
	  /// An identifier for the holiday calendar of the European Union TARGET system, with code 'EUTA'.
	  /// <para>
	  /// This constant references the calendar for the TARGET interbank payment system holidays.
	  /// </para>
	  /// <para>
	  /// Referenced by the 2006 ISDA definitions 1.8.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId EUTA = HolidayCalendarId.of("EUTA");
	  /// <summary>
	  /// An identifier for the holiday calendar of United States Government Securities, with code 'USGS'.
	  /// <para>
	  /// This constant references the calendar for United States Government Securities as per SIFMA.
	  /// </para>
	  /// <para>
	  /// Referenced by the 2006 ISDA definitions 1.11.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId USGS = HolidayCalendarId.of("USGS");
	  /// <summary>
	  /// An identifier for the holiday calendar of New York, United States, with code 'USNY'.
	  /// <para>
	  /// This constant references the calendar for New York holidays.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId USNY = HolidayCalendarId.of("USNY");
	  /// <summary>
	  /// An identifier for the holiday calendar of the Federal Reserve Bank of New York, with code 'NYFD'.
	  /// <para>
	  /// This constant references the calendar for the Federal Reserve Bank of New York holidays.
	  /// </para>
	  /// <para>
	  /// Referenced by the 2006 ISDA definitions 1.9.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId NYFD = HolidayCalendarId.of("NYFD");
	  /// <summary>
	  /// An identifier for the holiday calendar of the New York Stock Exchange, with code 'NYSE'.
	  /// <para>
	  /// This constant references the calendar for the New York Stock Exchange.
	  /// </para>
	  /// <para>
	  /// Referenced by the 2006 ISDA definitions 1.10.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId NYSE = HolidayCalendarId.of("NYSE");
	  /// <summary>
	  /// An identifier for the holiday calendar of Tokyo, Japan, with code 'JPTO'.
	  /// <para>
	  /// This constant references the calendar for Tokyo bank holidays.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId JPTO = HolidayCalendarId.of("JPTO");

	  /// <summary>
	  /// An identifier for the holiday calendar of Sydney, Australia, with code 'AUSY'.
	  /// <para>
	  /// This constant references the calendar for Sydney bank holidays.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId AUSY = HolidayCalendarId.of("AUSY");
	  /// <summary>
	  /// An identifier for the holiday calendar of Brazil, with code 'BRBD'.
	  /// <para>
	  /// This constant references the combined calendar for Brazil bank holidays.
	  /// This unites city-level calendars.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId BRBD = HolidayCalendarId.of("BRBD");
	  /// <summary>
	  /// An identifier for the holiday calendar of Montreal, Canada, with code 'CAMO'.
	  /// <para>
	  /// This constant references the calendar for Montreal bank holidays.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId CAMO = HolidayCalendarId.of("CAMO");
	  /// <summary>
	  /// An identifier for the holiday calendar of Toronto, Canada, with code 'CATO'.
	  /// <para>
	  /// This constant references the calendar for Toronto bank holidays.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId CATO = HolidayCalendarId.of("CATO");
	  /// <summary>
	  /// An identifier for the holiday calendar of Prague, Czech Republic, with code 'CZPR'.
	  /// <para>
	  /// This constant references the calendar for Prague bank holidays.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId CZPR = HolidayCalendarId.of("CZPR");
	  /// <summary>
	  /// An identifier for the holiday calendar of Copenhagen, Denmark, with code 'DKCO'.
	  /// <para>
	  /// This constant references the calendar for Copenhagen bank holidays.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId DKCO = HolidayCalendarId.of("DKCO");
	  /// <summary>
	  /// An identifier for the holiday calendar of Budapest, Hungary, with code 'HUBU'.
	  /// <para>
	  /// This constant references the calendar for Budapest bank holidays.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId HUBU = HolidayCalendarId.of("HUBU");
	  /// <summary>
	  /// An identifier for the holiday calendar of Mexico City, Mexico, with code 'MXMC'.
	  /// <para>
	  /// This constant references the calendar for Mexico City bank holidays.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId MXMC = HolidayCalendarId.of("MXMC");
	  /// <summary>
	  /// An identifier for the holiday calendar of Oslo, Norway, with code 'NOOS'.
	  /// <para>
	  /// This constant references the calendar for Oslo bank holidays.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId NOOS = HolidayCalendarId.of("NOOS");
	  /// <summary>
	  /// An identifier for the holiday calendar of Auckland, New Zealand, with code 'NZAU'.
	  /// <para>
	  /// This constant references the calendar for Auckland bank holidays.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId NZAU = HolidayCalendarId.of("NZAU");
	  /// <summary>
	  /// An identifier for the holiday calendar of Wellington, New Zealand, with code 'NZWE'.
	  /// <para>
	  /// This constant references the calendar for Wellington bank holidays.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId NZWE = HolidayCalendarId.of("NZWE");
	  /// <summary>
	  /// An identifier for the holiday calendar of Warsaw, Poland, with code 'PLWA'.
	  /// <para>
	  /// This constant references the calendar for Warsaw bank holidays.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId PLWA = HolidayCalendarId.of("PLWA");
	  /// <summary>
	  /// An identifier for the holiday calendar of Stockholm, Sweden, with code 'SEST'.
	  /// <para>
	  /// This constant references the calendar for Stockholm bank holidays.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId SEST = HolidayCalendarId.of("SEST");
	  /// <summary>
	  /// An identifier for the holiday calendar of Johannesburg, South Africa, with code 'ZAJO'.
	  /// <para>
	  /// This constant references the calendar for Johannesburg bank holidays.
	  /// </para>
	  /// </summary>
	  public static readonly HolidayCalendarId ZAJO = HolidayCalendarId.of("ZAJO");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private HolidayCalendarIds()
	  {
	  }

	}

}