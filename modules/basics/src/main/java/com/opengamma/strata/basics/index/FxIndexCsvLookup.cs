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
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using CsvFile = com.opengamma.strata.collect.io.CsvFile;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using ResourceConfig = com.opengamma.strata.collect.io.ResourceConfig;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using NamedLookup = com.opengamma.strata.collect.named.NamedLookup;

	/// <summary>
	/// Loads standard FX Index implementations from CSV.
	/// <para>
	/// See <seealso cref="FxIndices"/> for the description of each.
	/// </para>
	/// </summary>
	internal sealed class FxIndexCsvLookup : NamedLookup<FxIndex>
	{

	  // https://developers.opengamma.com/quantitative-research/Interest-Rate-Instruments-and-Market-Conventions.pdf

	  /// <summary>
	  /// The logger.
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
	  private static readonly Logger log = Logger.getLogger(typeof(FxIndexCsvLookup).FullName);
	  /// <summary>
	  /// The singleton instance of the lookup.
	  /// </summary>
	  public static readonly FxIndexCsvLookup INSTANCE = new FxIndexCsvLookup();

	  // CSV column headers
	  private const string NAME_FIELD = "Name";
	  private const string BASE_CURRENCY_FIELD = "Base Currency";
	  private const string COUNTER_CURRENCY_FIELD = "Counter Currency";
	  private const string FIXING_CALENDAR_FIELD = "Fixing Calendar";
	  private const string MATURITY_DAYS_FIELD = "Maturity Days";
	  private const string MATURITY_CALENDAR_FIELD = "Maturity Calendar";

	  /// <summary>
	  /// The cache by name.
	  /// </summary>
	  private static readonly ImmutableMap<string, FxIndex> BY_NAME = loadFromCsv();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private FxIndexCsvLookup()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public IDictionary<string, FxIndex> lookupAll()
	  {
		return BY_NAME;
	  }

	  private static ImmutableMap<string, FxIndex> loadFromCsv()
	  {
		IList<ResourceLocator> resources = ResourceConfig.orderedResources("FxIndexData.csv");
		IDictionary<string, FxIndex> map = new Dictionary<string, FxIndex>();
		foreach (ResourceLocator resource in resources)
		{
		  try
		  {
			CsvFile csv = CsvFile.of(resource.CharSource, true);
			foreach (CsvRow row in csv.rows())
			{
			  FxIndex parsed = parseFxIndex(row);
			  map[parsed.Name] = parsed;
			  if (!map.ContainsKey(parsed.Name.ToUpper(Locale.ENGLISH))) map.Add(parsed.Name.ToUpper(Locale.ENGLISH), parsed);
			}
		  }
		  catch (Exception ex)
		  {
			log.log(Level.SEVERE, "Error processing resource as FX Index CSV file: " + resource, ex);
			return ImmutableMap.of();
		  }
		}
		return ImmutableMap.copyOf(map);
	  }

	  private static FxIndex parseFxIndex(CsvRow row)
	  {
		string name = row.getField(NAME_FIELD);
		Currency baseCurrency = Currency.parse(row.getField(BASE_CURRENCY_FIELD));
		Currency counterCurrency = Currency.parse(row.getField(COUNTER_CURRENCY_FIELD));
		HolidayCalendarId fixingCal = HolidayCalendarId.of(row.getField(FIXING_CALENDAR_FIELD));
		int maturityDays = int.Parse(row.getField(MATURITY_DAYS_FIELD));
		HolidayCalendarId maturityCal = HolidayCalendarId.of(row.getField(MATURITY_CALENDAR_FIELD));
		// build result
		return ImmutableFxIndex.builder().name(name).currencyPair(CurrencyPair.of(baseCurrency, counterCurrency)).fixingCalendar(fixingCal).maturityDateOffset(DaysAdjustment.ofBusinessDays(maturityDays, maturityCal)).build();
	  }

	}

}