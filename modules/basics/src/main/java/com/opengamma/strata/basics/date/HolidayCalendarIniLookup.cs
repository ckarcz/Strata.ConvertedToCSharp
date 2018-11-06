using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableSet;


	using VisibleForTesting = com.google.common.annotations.VisibleForTesting;
	using Splitter = com.google.common.@base.Splitter;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using IniFile = com.opengamma.strata.collect.io.IniFile;
	using PropertySet = com.opengamma.strata.collect.io.PropertySet;
	using ResourceConfig = com.opengamma.strata.collect.io.ResourceConfig;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using NamedLookup = com.opengamma.strata.collect.named.NamedLookup;

	/// <summary>
	/// Loads holiday calendar implementations from INI files.
	/// <para>
	/// These will form the standard holiday calendars available in <seealso cref="ReferenceData#standard()"/>.
	/// </para>
	/// </summary>
	internal sealed class HolidayCalendarIniLookup : NamedLookup<HolidayCalendar>
	{

	  /// <summary>
	  /// The logger.
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
	  private static readonly Logger log = Logger.getLogger(typeof(HolidayCalendarIniLookup).FullName);
	  /// <summary>
	  /// The singleton instance of the lookup.
	  /// </summary>
	  public static readonly HolidayCalendarIniLookup INSTANCE = new HolidayCalendarIniLookup();

	  /// <summary>
	  /// The Weekend key name.
	  /// </summary>
	  private const string WEEKEND_KEY = "Weekend";
	  /// <summary>
	  /// The lenient day-of-week parser.
	  /// </summary>
	  private static readonly DateTimeFormatter DOW_PARSER = new DateTimeFormatterBuilder().parseCaseInsensitive().parseLenient().appendText(DAY_OF_WEEK).toFormatter(Locale.ENGLISH);
	  /// <summary>
	  /// The lenient month-day parser.
	  /// </summary>
	  private static readonly DateTimeFormatter DAY_MONTH_PARSER = new DateTimeFormatterBuilder().parseCaseInsensitive().parseLenient().appendText(MONTH_OF_YEAR).appendOptional(new DateTimeFormatterBuilder().appendLiteral('-').toFormatter(Locale.ENGLISH)).appendValue(DAY_OF_MONTH).toFormatter(Locale.ENGLISH);

	  /// <summary>
	  /// The holiday calendars by name.
	  /// </summary>
	  private static readonly ImmutableMap<string, HolidayCalendar> BY_NAME = loadFromIni("HolidayCalendarData.ini");
	  /// <summary>
	  /// The default holiday calendars by currency.
	  /// </summary>
	  private static readonly ImmutableMap<Currency, HolidayCalendarId> BY_CURRENCY = loadDefaultsFromIni("HolidayCalendarDefaultData.ini");

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private HolidayCalendarIniLookup()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public IDictionary<string, HolidayCalendar> lookupAll()
	  {
		return BY_NAME;
	  }

	  // finds a default
	  internal HolidayCalendarId defaultByCurrency(Currency currency)
	  {
		HolidayCalendarId calId = BY_CURRENCY.get(currency);
		if (calId == null)
		{
		  throw new System.ArgumentException("No default Holiday Calendar for currency " + currency);
		}
		return calId;
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @VisibleForTesting static com.google.common.collect.ImmutableMap<String, HolidayCalendar> loadFromIni(String filename)
	  internal static ImmutableMap<string, HolidayCalendar> loadFromIni(string filename)
	  {
		IList<ResourceLocator> resources = ResourceConfig.orderedResources(filename);
		IDictionary<string, HolidayCalendar> map = new Dictionary<string, HolidayCalendar>();
		foreach (ResourceLocator resource in resources)
		{
		  try
		  {
			IniFile ini = IniFile.of(resource.CharSource);
			foreach (string sectionName in ini.sections())
			{
			  PropertySet section = ini.section(sectionName);
			  HolidayCalendar parsed = parseHolidayCalendar(sectionName, section);
			  map[parsed.Name] = parsed;
			  if (!map.ContainsKey(parsed.Name.ToUpper(Locale.ENGLISH))) map.Add(parsed.Name.ToUpper(Locale.ENGLISH), parsed);
			}
		  }
		  catch (Exception ex)
		  {
			log.log(Level.SEVERE, "Error processing resource as Holiday Calendar INI file: " + resource, ex);
			return ImmutableMap.of();
		  }
		}
		return ImmutableMap.copyOf(map);
	  }

	  private static HolidayCalendar parseHolidayCalendar(string calendarName, PropertySet section)
	  {
		string weekendStr = section.value(WEEKEND_KEY);
		ISet<DayOfWeek> weekends = parseWeekends(weekendStr);
		IList<LocalDate> holidays = new List<LocalDate>();
		foreach (string key in section.keys())
		{
		  if (key.Equals(WEEKEND_KEY))
		  {
			continue;
		  }
		  string value = section.value(key);
		  if (key.Length == 4)
		  {
			int year = int.Parse(key);
			((IList<LocalDate>)holidays).AddRange(parseYearDates(year, value));
		  }
		  else
		  {
			holidays.Add(LocalDate.parse(key));
		  }
		}
		// build result
		return ImmutableHolidayCalendar.of(HolidayCalendarId.of(calendarName), holidays, weekends);
	  }

	  // parse weekend format, such as 'Sat,Sun'
	  private static ISet<DayOfWeek> parseWeekends(string str)
	  {
		IList<string> split = Splitter.on(',').splitToList(str);
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return split.Select(v => DOW_PARSER.parse(v, DayOfWeek.from)).collect(toImmutableSet());
	  }

	  // parse year format, such as 'Jan1,Mar12,Dec25' or '2015-01-01,2015-03-12,2015-12-25'
	  private static IList<LocalDate> parseYearDates(int year, string str)
	  {
		IList<string> split = Splitter.on(',').splitToList(str);
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return split.Select(v => parseDate(year, v)).collect(toImmutableList());
	  }

	  private static LocalDate parseDate(int year, string str)
	  {
		try
		{
		  return MonthDay.parse(str, DAY_MONTH_PARSER).atYear(year);
		}
		catch (DateTimeParseException)
		{
		  LocalDate date = LocalDate.parse(str);
		  if (date.Year != year)
		  {
			throw new System.ArgumentException("Parsed date had incorrect year: " + str + ", but expected: " + year);
		  }
		  return date;
		}
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @VisibleForTesting static com.google.common.collect.ImmutableMap<com.opengamma.strata.basics.currency.Currency, HolidayCalendarId> loadDefaultsFromIni(String filename)
	  internal static ImmutableMap<Currency, HolidayCalendarId> loadDefaultsFromIni(string filename)
	  {
		IList<ResourceLocator> resources = ResourceConfig.orderedResources(filename);
		IDictionary<Currency, HolidayCalendarId> map = new Dictionary<Currency, HolidayCalendarId>();
		foreach (ResourceLocator resource in resources)
		{
		  try
		  {
			IniFile ini = IniFile.of(resource.CharSource);
			PropertySet section = ini.section("defaultByCurrency");
			foreach (string currencyCode in section.keys())
			{
			  map[Currency.of(currencyCode)] = HolidayCalendarId.of(section.value(currencyCode));
			}
		  }
		  catch (Exception ex)
		  {
			log.log(Level.SEVERE, "Error processing resource as Holiday Calendar Defaults INI file: " + resource, ex);
			return ImmutableMap.of();
		  }
		}
		return ImmutableMap.copyOf(map);
	  }

	}

}