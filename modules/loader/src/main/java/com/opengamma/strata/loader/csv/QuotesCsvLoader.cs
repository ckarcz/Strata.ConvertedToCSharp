using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{


	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Builder = com.google.common.collect.ImmutableMap.Builder;
	using CharSource = com.google.common.io.CharSource;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Messages = com.opengamma.strata.collect.Messages;
	using CsvFile = com.opengamma.strata.collect.io.CsvFile;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using UnicodeBom = com.opengamma.strata.collect.io.UnicodeBom;
	using FieldName = com.opengamma.strata.data.FieldName;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;

	/// <summary>
	/// Loads a set of quotes into memory from CSV resources.
	/// <para>
	/// The quotes are expected to be in a CSV format, with the following header row:<br />
	/// {@code Valuation Date, Symbology, Ticker, Value}.
	/// <ul>
	/// <li>The 'Valuation Date' column provides the valuation date, allowing data from different
	///  days to be stored in the same file
	/// <li>The 'Symbology' column is the symbology scheme applicable to the ticker.
	/// <li>The 'Ticker' column is the identifier within the symbology.
	/// <li>The 'Field Name' column is the field name, defaulted to 'MarketValue', allowing
	///  fields such as 'Bid' or 'Ask' to be specified.
	/// <li>The 'Value' column is the value of the ticker.
	/// </ul>
	/// </para>
	/// <para>
	/// Each quotes file may contain entries for many different dates.
	/// </para>
	/// <para>
	/// For example:
	/// <pre>
	/// Valuation Date, Symbology, Ticker, Field Name, Value
	/// 2014-01-22, OG-Future, Eurex-FGBL-Mar14, MarketValue, 150.43
	/// 2014-01-22, OG-FutOpt, Eurex-OGBL-Mar14-C150, MarketValue, 1.5
	/// 2014-01-22, OG-Future, CME-ED-Mar14, MarketValue, 99.620
	/// </pre>
	/// Note that Microsoft Excel prefers the CSV file to have no space after the comma.
	/// </para>
	/// <para>
	/// CSV files sometimes contain a Unicode Byte Order Mark.
	/// Callers are responsible for handling this, such as by using <seealso cref="UnicodeBom"/>.
	/// </para>
	/// </summary>
	public sealed class QuotesCsvLoader
	{

	  // CSV column headers
	  private const string DATE_FIELD = "Valuation Date";
	  private const string SYMBOLOGY_FIELD = "Symbology";
	  private const string TICKER_FIELD = "Ticker";
	  private const string FIELD_NAME_FIELD = "Field Name";
	  private const string VALUE_FIELD = "Value";

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Loads one or more CSV format quote files for a specific date.
	  /// <para>
	  /// Only those quotes that match the specified date will be loaded.
	  /// </para>
	  /// <para>
	  /// If the files contain a duplicate entry an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketDataDate">  the date to load </param>
	  /// <param name="resources">  the CSV resources </param>
	  /// <returns> the loaded quotes, mapped by <seealso cref="QuoteId quote ID"/> </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableMap<QuoteId, double> load(LocalDate marketDataDate, params ResourceLocator[] resources)
	  {
		return load(marketDataDate, Arrays.asList(resources));
	  }

	  /// <summary>
	  /// Loads one or more CSV format quote files for a specific date.
	  /// <para>
	  /// Only those quotes that match the specified date will be loaded.
	  /// </para>
	  /// <para>
	  /// If the files contain a duplicate entry an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketDataDate">  the date to load </param>
	  /// <param name="resources">  the CSV resources </param>
	  /// <returns> the loaded quotes, mapped by <seealso cref="QuoteId quote ID"/> </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableMap<QuoteId, double> load(LocalDate marketDataDate, ICollection<ResourceLocator> resources)
	  {
		ICollection<CharSource> charSources = resources.Select(r => r.CharSource).ToList();
		return parse(d => marketDataDate.Equals(d), charSources).getOrDefault(marketDataDate, ImmutableMap.of());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Loads one or more CSV format quote files for a set of dates.
	  /// <para>
	  /// Only those quotes that match one of the specified dates will be loaded.
	  /// </para>
	  /// <para>
	  /// If the files contain a duplicate entry an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketDataDates">  the set of dates to load </param>
	  /// <param name="resources">  the CSV resources </param>
	  /// <returns> the loaded quotes, mapped by <seealso cref="LocalDate"/> and <seealso cref="QuoteId quote ID"/> </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableMap<LocalDate, ImmutableMap<QuoteId, double>> load(ISet<LocalDate> marketDataDates, params ResourceLocator[] resources)
	  {

		return load(marketDataDates, Arrays.asList(resources));
	  }

	  /// <summary>
	  /// Loads one or more CSV format quote files for a set of dates.
	  /// <para>
	  /// Only those quotes that match one of the specified dates will be loaded.
	  /// </para>
	  /// <para>
	  /// If the files contain a duplicate entry an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketDataDates">  the dates to load </param>
	  /// <param name="resources">  the CSV resources </param>
	  /// <returns> the loaded quotes, mapped by <seealso cref="LocalDate"/> and <seealso cref="QuoteId quote ID"/> </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableMap<LocalDate, ImmutableMap<QuoteId, double>> load(ISet<LocalDate> marketDataDates, ICollection<ResourceLocator> resources)
	  {

		ICollection<CharSource> charSources = resources.Select(r => r.CharSource).ToList();
		return parse(d => marketDataDates.Contains(d), charSources);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Loads one or more CSV format quote files.
	  /// <para>
	  /// All dates that are found will be returned.
	  /// </para>
	  /// <para>
	  /// If the files contain a duplicate entry an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resources">  the CSV resources </param>
	  /// <returns> the loaded quotes, mapped by <seealso cref="LocalDate"/> and <seealso cref="QuoteId quote ID"/> </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableMap<LocalDate, ImmutableMap<QuoteId, double>> loadAllDates(params ResourceLocator[] resources)
	  {
		return loadAllDates(Arrays.asList(resources));
	  }

	  /// <summary>
	  /// Loads one or more CSV format quote files.
	  /// <para>
	  /// All dates that are found will be returned.
	  /// </para>
	  /// <para>
	  /// If the files contain a duplicate entry an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resources">  the CSV resources </param>
	  /// <returns> the loaded quotes, mapped by <seealso cref="LocalDate"/> and <seealso cref="QuoteId quote ID"/> </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableMap<LocalDate, ImmutableMap<QuoteId, double>> loadAllDates(ICollection<ResourceLocator> resources)
	  {

		ICollection<CharSource> charSources = resources.Select(r => r.CharSource).ToList();
		return parse(d => true, charSources);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses one or more CSV format quote files.
	  /// <para>
	  /// A predicate is specified that is used to filter the dates that are returned.
	  /// This could match a single date, a set of dates or all dates.
	  /// </para>
	  /// <para>
	  /// If the files contain a duplicate entry an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="datePredicate">  the predicate used to select the dates </param>
	  /// <param name="charSources">  the CSV character sources </param>
	  /// <returns> the loaded quotes, mapped by <seealso cref="LocalDate"/> and <seealso cref="QuoteId quote ID"/> </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableMap<LocalDate, ImmutableMap<QuoteId, double>> parse(System.Predicate<LocalDate> datePredicate, ICollection<CharSource> charSources)
	  {

		// builder ensures keys can only be seen once
		IDictionary<LocalDate, ImmutableMap.Builder<QuoteId, double>> mutableMap = new Dictionary<LocalDate, ImmutableMap.Builder<QuoteId, double>>();
		foreach (CharSource charSource in charSources)
		{
		  parseSingle(datePredicate, charSource, mutableMap);
		}
		ImmutableMap.Builder<LocalDate, ImmutableMap<QuoteId, double>> builder = ImmutableMap.builder();
		foreach (KeyValuePair<LocalDate, ImmutableMap.Builder<QuoteId, double>> entry in mutableMap.SetOfKeyValuePairs())
		{
		  builder.put(entry.Key, entry.Value.build());
		}
		return builder.build();
	  }

	  // loads a single CSV file, filtering by date
	  private static void parseSingle(System.Predicate<LocalDate> datePredicate, CharSource resource, IDictionary<LocalDate, ImmutableMap.Builder<QuoteId, double>> mutableMap)
	  {

		try
		{
		  CsvFile csv = CsvFile.of(resource, true);
		  foreach (CsvRow row in csv.rows())
		  {
			string dateText = row.getField(DATE_FIELD);
			LocalDate date = LoaderUtils.parseDate(dateText);
			if (datePredicate(date))
			{
			  string symbologyStr = row.getField(SYMBOLOGY_FIELD);
			  string tickerStr = row.getField(TICKER_FIELD);
			  string fieldNameStr = row.getField(FIELD_NAME_FIELD);
			  string valueStr = row.getField(VALUE_FIELD);

			  double value = Convert.ToDouble(valueStr);
			  StandardId id = StandardId.of(symbologyStr, tickerStr);
			  FieldName fieldName = fieldNameStr.Length == 0 ? FieldName.MARKET_VALUE : FieldName.of(fieldNameStr);

			  ImmutableMap.Builder<QuoteId, double> builderForDate = mutableMap.computeIfAbsent(date, k => ImmutableMap.builder());
			  builderForDate.put(QuoteId.of(id, fieldName), value);
			}
		  }
		}
		catch (Exception ex)
		{
		  throw new System.ArgumentException(Messages.format("Error processing resource as CSV file: {}", resource), ex);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private QuotesCsvLoader()
	  {
	  }

	}

}