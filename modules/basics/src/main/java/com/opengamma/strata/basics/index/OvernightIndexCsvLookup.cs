using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using CsvFile = com.opengamma.strata.collect.io.CsvFile;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using ResourceConfig = com.opengamma.strata.collect.io.ResourceConfig;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using NamedLookup = com.opengamma.strata.collect.named.NamedLookup;

	/// <summary>
	/// Loads standard Overnight Index implementations from CSV.
	/// <para>
	/// See <seealso cref="OvernightIndices"/> for the description of each.
	/// </para>
	/// </summary>
	internal sealed class OvernightIndexCsvLookup : NamedLookup<OvernightIndex>
	{

	  // https://developers.opengamma.com/quantitative-research/Interest-Rate-Instruments-and-Market-Conventions.pdf

	  /// <summary>
	  /// The logger.
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
	  private static readonly Logger log = Logger.getLogger(typeof(OvernightIndexCsvLookup).FullName);
	  /// <summary>
	  /// The singleton instance of the lookup.
	  /// </summary>
	  public static readonly OvernightIndexCsvLookup INSTANCE = new OvernightIndexCsvLookup();

	  // CSV column headers
	  private const string NAME_FIELD = "Name";
	  private const string CURRENCY_FIELD = "Currency";
	  private const string ACTIVE_FIELD = "Active";
	  private const string DAY_COUNT_FIELD = "Day Count";
	  private const string FIXING_CALENDAR_FIELD = "Fixing Calendar";
	  private const string PUBLICATION_DAYS_FIELD = "Publication Offset Days";
	  private const string EFFECTIVE_DAYS_FIELD = "Effective Offset Days";
	  private const string FIXED_LEG_DAY_COUNT = "Fixed Leg Day Count";

	  /// <summary>
	  /// The cache by name.
	  /// </summary>
	  private static readonly ImmutableMap<string, OvernightIndex> BY_NAME = loadFromCsv();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private OvernightIndexCsvLookup()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public IDictionary<string, OvernightIndex> lookupAll()
	  {
		return BY_NAME;
	  }

	  private static ImmutableMap<string, OvernightIndex> loadFromCsv()
	  {
		IList<ResourceLocator> resources = ResourceConfig.orderedResources("OvernightIndexData.csv");
		IDictionary<string, OvernightIndex> map = new Dictionary<string, OvernightIndex>();
		foreach (ResourceLocator resource in resources)
		{
		  try
		  {
			CsvFile csv = CsvFile.of(resource.CharSource, true);
			foreach (CsvRow row in csv.rows())
			{
			  OvernightIndex parsed = parseOvernightIndex(row);
			  map[parsed.Name] = parsed;
			  if (!map.ContainsKey(parsed.Name.ToUpper(Locale.ENGLISH))) map.Add(parsed.Name.ToUpper(Locale.ENGLISH), parsed);
			}
		  }
		  catch (Exception ex)
		  {
			log.log(Level.SEVERE, "Error processing resource as Overnight Index CSV file: " + resource, ex);
			return ImmutableMap.of();
		  }
		}
		return ImmutableMap.copyOf(map);
	  }

	  private static OvernightIndex parseOvernightIndex(CsvRow row)
	  {
		string name = row.getValue(NAME_FIELD);
		Currency currency = Currency.parse(row.getValue(CURRENCY_FIELD));
		bool active = bool.Parse(row.getValue(ACTIVE_FIELD));
		DayCount dayCount = DayCount.of(row.getValue(DAY_COUNT_FIELD));
		HolidayCalendarId fixingCal = HolidayCalendarId.of(row.getValue(FIXING_CALENDAR_FIELD));
		int publicationDays = int.Parse(row.getValue(PUBLICATION_DAYS_FIELD));
		int effectiveDays = int.Parse(row.getValue(EFFECTIVE_DAYS_FIELD));
		DayCount fixedLegDayCount = DayCount.of(row.getValue(FIXED_LEG_DAY_COUNT));
		// build result
		return ImmutableOvernightIndex.builder().name(name).currency(currency).active(active).dayCount(dayCount).fixingCalendar(fixingCal).publicationDateOffset(publicationDays).effectiveDateOffset(effectiveDays).defaultFixedLegDayCount(fixedLegDayCount).build();
	  }

	}

}