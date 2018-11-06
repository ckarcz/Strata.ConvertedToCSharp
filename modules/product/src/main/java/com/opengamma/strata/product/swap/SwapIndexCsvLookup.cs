using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using CsvFile = com.opengamma.strata.collect.io.CsvFile;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using ResourceConfig = com.opengamma.strata.collect.io.ResourceConfig;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using NamedLookup = com.opengamma.strata.collect.named.NamedLookup;
	using FixedIborSwapConvention = com.opengamma.strata.product.swap.type.FixedIborSwapConvention;
	using FixedIborSwapTemplate = com.opengamma.strata.product.swap.type.FixedIborSwapTemplate;

	/// <summary>
	/// Loads standard Swap Index implementations from CSV.
	/// <para>
	/// See <seealso cref="SwapIndices"/> for the description of each.
	/// </para>
	/// </summary>
	internal sealed class SwapIndexCsvLookup : NamedLookup<SwapIndex>
	{

	  /// <summary>
	  /// The logger.
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
	  private static readonly Logger log = Logger.getLogger(typeof(SwapIndexCsvLookup).FullName);
	  /// <summary>
	  /// The singleton instance of the lookup.
	  /// </summary>
	  public static readonly SwapIndexCsvLookup INSTANCE = new SwapIndexCsvLookup();

	  private const string NAME_FIELD = "Name";
	  private const string ACTIVE_FIELD = "Active";
	  private const string CONVENTION_FIELD = "Convention";
	  private const string TENOR_FIELD = "Tenor";
	  private const string FIXING_TIME_FIELD = "FixingTime";
	  private const string FIXING_ZONE_FIELD = "FixingZone";

	  /// <summary>
	  /// The time formatter.
	  /// </summary>
	  private static readonly DateTimeFormatter TIME_FORMAT = DateTimeFormatter.ofPattern("HH[:mm]", Locale.ENGLISH);
	  /// <summary>
	  /// The cache by name.
	  /// </summary>
	  private static readonly ImmutableMap<string, SwapIndex> BY_NAME = loadFromCsv();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private SwapIndexCsvLookup()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public IDictionary<string, SwapIndex> lookupAll()
	  {
		return BY_NAME;
	  }

	  private static ImmutableMap<string, SwapIndex> loadFromCsv()
	  {
		IList<ResourceLocator> resources = ResourceConfig.orderedResources("SwapIndexData.csv");
		IDictionary<string, SwapIndex> map = new Dictionary<string, SwapIndex>();
		foreach (ResourceLocator resource in resources)
		{
		  try
		  {
			CsvFile csv = CsvFile.of(resource.CharSource, true);
			foreach (CsvRow row in csv.rows())
			{
			  SwapIndex parsed = parseSwapIndex(row);
			  map[parsed.Name] = parsed;
			  if (!map.ContainsKey(parsed.Name.ToUpper(Locale.ENGLISH))) map.Add(parsed.Name.ToUpper(Locale.ENGLISH), parsed);
			}
		  }
		  catch (Exception ex)
		  {
			log.log(Level.SEVERE, "Error processing resource as Swap Index CSV file: " + resource, ex);
			return ImmutableMap.of();
		  }
		}
		return ImmutableMap.copyOf(map);
	  }

	  private static SwapIndex parseSwapIndex(CsvRow row)
	  {
		string name = row.getField(NAME_FIELD);
		bool active = bool.Parse(row.getField(ACTIVE_FIELD));
		FixedIborSwapConvention convention = FixedIborSwapConvention.of(row.getField(CONVENTION_FIELD));
		Tenor tenor = Tenor.parse(row.getField(TENOR_FIELD));
		LocalTime time = LocalTime.parse(row.getField(FIXING_TIME_FIELD), TIME_FORMAT);
		ZoneId zoneId = ZoneId.of(row.getField(FIXING_ZONE_FIELD));
		// build result
		return ImmutableSwapIndex.builder().name(name).active(active).fixingTime(time).fixingZone(zoneId).template(FixedIborSwapTemplate.of(tenor, convention)).build();
	  }

	}

}