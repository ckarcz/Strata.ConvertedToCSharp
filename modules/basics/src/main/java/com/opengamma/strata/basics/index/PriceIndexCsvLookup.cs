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
	using Country = com.opengamma.strata.basics.location.Country;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using CsvFile = com.opengamma.strata.collect.io.CsvFile;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using ResourceConfig = com.opengamma.strata.collect.io.ResourceConfig;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using NamedLookup = com.opengamma.strata.collect.named.NamedLookup;

	/// <summary>
	/// Loads standard Price Index implementations from CSV.
	/// <para>
	/// See <seealso cref="PriceIndices"/> for the description of each.
	/// </para>
	/// </summary>
	internal sealed class PriceIndexCsvLookup : NamedLookup<PriceIndex>
	{

	  // https://developers.opengamma.com/quantitative-research/Interest-Rate-Instruments-and-Market-Conventions.pdf

	  /// <summary>
	  /// The logger.
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
	  private static readonly Logger log = Logger.getLogger(typeof(PriceIndexCsvLookup).FullName);
	  /// <summary>
	  /// The singleton instance of the lookup.
	  /// </summary>
	  public static readonly PriceIndexCsvLookup INSTANCE = new PriceIndexCsvLookup();

	  // CSV column headers
	  private const string NAME_FIELD = "Name";
	  private const string CURRENCY_FIELD = "Currency";
	  private const string COUNTRY_FIELD = "Country";
	  private const string ACTIVE_FIELD = "Active";
	  private const string PUBLICATION_FREQUENCY_FIELD = "Publication Frequency";

	  /// <summary>
	  /// The cache by name.
	  /// </summary>
	  private static readonly ImmutableMap<string, PriceIndex> BY_NAME = loadFromCsv();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private PriceIndexCsvLookup()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public IDictionary<string, PriceIndex> lookupAll()
	  {
		return BY_NAME;
	  }

	  private static ImmutableMap<string, PriceIndex> loadFromCsv()
	  {
		IList<ResourceLocator> resources = ResourceConfig.orderedResources("PriceIndexData.csv");
		IDictionary<string, PriceIndex> map = new Dictionary<string, PriceIndex>();
		foreach (ResourceLocator resource in resources)
		{
		  try
		  {
			CsvFile csv = CsvFile.of(resource.CharSource, true);
			foreach (CsvRow row in csv.rows())
			{
			  PriceIndex parsed = parsePriceIndex(row);
			  map[parsed.Name] = parsed;
			  if (!map.ContainsKey(parsed.Name.ToUpper(Locale.ENGLISH))) map.Add(parsed.Name.ToUpper(Locale.ENGLISH), parsed);
			}
		  }
		  catch (Exception ex)
		  {
			log.log(Level.SEVERE, "Error processing resource as Price Index CSV file: " + resource, ex);
			return ImmutableMap.of();
		  }
		}
		return ImmutableMap.copyOf(map);
	  }

	  private static PriceIndex parsePriceIndex(CsvRow row)
	  {
		string name = row.getField(NAME_FIELD);
		Currency currency = Currency.parse(row.getField(CURRENCY_FIELD));
		Country region = Country.of(row.getField(COUNTRY_FIELD));
		bool active = bool.Parse(row.getField(ACTIVE_FIELD));
		Frequency frequency = Frequency.parse(row.getField(PUBLICATION_FREQUENCY_FIELD));
		// build result
		return ImmutablePriceIndex.builder().name(name).currency(currency).region(region).active(active).publicationFrequency(frequency).build();
	  }

	}

}