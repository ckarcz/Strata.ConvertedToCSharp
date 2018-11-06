using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.PRECEDING;


	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConvention = com.opengamma.strata.basics.date.BusinessDayConvention;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using PeriodAdditionConvention = com.opengamma.strata.basics.date.PeriodAdditionConvention;
	using PeriodAdditionConventions = com.opengamma.strata.basics.date.PeriodAdditionConventions;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using TenorAdjustment = com.opengamma.strata.basics.date.TenorAdjustment;
	using CsvFile = com.opengamma.strata.collect.io.CsvFile;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using ResourceConfig = com.opengamma.strata.collect.io.ResourceConfig;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using NamedLookup = com.opengamma.strata.collect.named.NamedLookup;

	/// <summary>
	/// Loads standard Ibor Index implementations from CSV.
	/// <para>
	/// See <seealso cref="IborIndices"/> for the description of each.
	/// </para>
	/// </summary>
	internal sealed class IborIndexCsvLookup : NamedLookup<IborIndex>
	{

	  // https://developers.opengamma.com/quantitative-research/Interest-Rate-Instruments-and-Market-Conventions.pdf
	  // LIBOR - http://www.bbalibor.com/technical-aspects/fixing-value-and-maturity
	  // different rules for overnight
	  // EURIBOR - http://www.bbalibor.com/technical-aspects/fixing-value-and-maturity
	  // EURIBOR - http://www.emmi-benchmarks.eu/assets/files/Euribor_code_conduct.pdf
	  // TIBOR - http://www.jbatibor.or.jp/english/public/

	  /// <summary>
	  /// The logger.
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
	  private static readonly Logger log = Logger.getLogger(typeof(IborIndexCsvLookup).FullName);
	  /// <summary>
	  /// The singleton instance of the lookup.
	  /// </summary>
	  public static readonly IborIndexCsvLookup INSTANCE = new IborIndexCsvLookup();

	  // CSV column headers
	  private const string NAME_FIELD = "Name";
	  private const string CURRENCY_FIELD = "Currency";
	  private const string ACTIVE_FIELD = "Active";
	  private const string DAY_COUNT_FIELD = "Day Count";
	  private const string FIXING_CALENDAR_FIELD = "Fixing Calendar";
	  private const string OFFSET_DAYS_FIELD = "Offset Days";
	  private const string OFFSET_CALENDAR_FIELD = "Offset Calendar";
	  private const string EFFECTIVE_DATE_CALENDAR_FIELD = "Effective Date Calendar";
	  private const string TENOR_FIELD = "Tenor";
	  private const string TENOR_CONVENTION_FIELD = "Tenor Convention";
	  private const string FIXING_TIME_FIELD = "FixingTime";
	  private const string FIXING_ZONE_FIELD = "FixingZone";
	  private const string FIXED_LEG_DAY_COUNT = "Fixed Leg Day Count";

	  /// <summary>
	  /// The time formatter.
	  /// </summary>
	  private static readonly DateTimeFormatter TIME_FORMAT = DateTimeFormatter.ofPattern("HH[:mm]", Locale.ENGLISH);
	  /// <summary>
	  /// The cache by name.
	  /// </summary>
	  private static readonly ImmutableMap<string, IborIndex> BY_NAME = loadFromCsv();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private IborIndexCsvLookup()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public IDictionary<string, IborIndex> lookupAll()
	  {
		return BY_NAME;
	  }

	  private static ImmutableMap<string, IborIndex> loadFromCsv()
	  {
		IList<ResourceLocator> resources = ResourceConfig.orderedResources("IborIndexData.csv");
		IDictionary<string, IborIndex> map = new Dictionary<string, IborIndex>();
		foreach (ResourceLocator resource in resources)
		{
		  try
		  {
			CsvFile csv = CsvFile.of(resource.CharSource, true);
			foreach (CsvRow row in csv.rows())
			{
			  IborIndex parsed = parseIborIndex(row);
			  map[parsed.Name] = parsed;
			  if (!map.ContainsKey(parsed.Name.ToUpper(Locale.ENGLISH))) map.Add(parsed.Name.ToUpper(Locale.ENGLISH), parsed);
			}
		  }
		  catch (Exception ex)
		  {
			log.log(Level.SEVERE, "Error processing resource as Ibor Index CSV file: " + resource, ex);
			return ImmutableMap.of();
		  }
		}
		return ImmutableMap.copyOf(map);
	  }

	  private static IborIndex parseIborIndex(CsvRow row)
	  {
		string name = row.getValue(NAME_FIELD);
		Currency currency = Currency.parse(row.getValue(CURRENCY_FIELD));
		bool active = bool.Parse(row.getValue(ACTIVE_FIELD));
		DayCount dayCount = DayCount.of(row.getValue(DAY_COUNT_FIELD));
		HolidayCalendarId fixingCal = HolidayCalendarId.of(row.getValue(FIXING_CALENDAR_FIELD));
		int offsetDays = int.Parse(row.getValue(OFFSET_DAYS_FIELD));
		HolidayCalendarId offsetCal = HolidayCalendarId.of(row.getValue(OFFSET_CALENDAR_FIELD));
		HolidayCalendarId effectiveCal = HolidayCalendarId.of(row.getValue(EFFECTIVE_DATE_CALENDAR_FIELD));
		Tenor tenor = Tenor.parse(row.getValue(TENOR_FIELD));
		LocalTime time = LocalTime.parse(row.getValue(FIXING_TIME_FIELD), TIME_FORMAT);
		ZoneId zoneId = ZoneId.of(row.getValue(FIXING_ZONE_FIELD));
		DayCount fixedLegDayCount = DayCount.of(row.getValue(FIXED_LEG_DAY_COUNT));

		// interpret CSV
		DaysAdjustment fixingOffset = DaysAdjustment.ofBusinessDays(-offsetDays, offsetCal, BusinessDayAdjustment.of(PRECEDING, fixingCal)).normalized();
		DaysAdjustment effectiveOffset = DaysAdjustment.ofBusinessDays(offsetDays, offsetCal, BusinessDayAdjustment.of(FOLLOWING, effectiveCal)).normalized();

		// convention can be two different things
		PeriodAdditionConvention periodAdditionConvention = PeriodAdditionConvention.extendedEnum().find(row.getField(TENOR_CONVENTION_FIELD)).orElse(PeriodAdditionConventions.NONE);
		BusinessDayConvention tenorBusinessConvention = BusinessDayConvention.extendedEnum().find(row.getField(TENOR_CONVENTION_FIELD)).orElse(isEndOfMonth(periodAdditionConvention) ? MODIFIED_FOLLOWING : FOLLOWING);
		BusinessDayAdjustment adj = BusinessDayAdjustment.of(tenorBusinessConvention, effectiveCal);
		TenorAdjustment tenorAdjustment = TenorAdjustment.of(tenor, periodAdditionConvention, adj);

		// build result
		return ImmutableIborIndex.builder().name(name).currency(currency).active(active).dayCount(dayCount).fixingCalendar(fixingCal).fixingDateOffset(fixingOffset).effectiveDateOffset(effectiveOffset).maturityDateOffset(tenorAdjustment).fixingTime(time).fixingZone(zoneId).defaultFixedLegDayCount(fixedLegDayCount).build();
	  }

	  private static bool isEndOfMonth(PeriodAdditionConvention tenorConvention)
	  {
		return tenorConvention.Equals(PeriodAdditionConventions.LAST_BUSINESS_DAY) || tenorConvention.Equals(PeriodAdditionConventions.LAST_DAY);
	  }

	}

}