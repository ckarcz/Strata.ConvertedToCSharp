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
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using Messages = com.opengamma.strata.collect.Messages;
	using CsvFile = com.opengamma.strata.collect.io.CsvFile;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using UnicodeBom = com.opengamma.strata.collect.io.UnicodeBom;
	using FxRateId = com.opengamma.strata.data.FxRateId;

	/// <summary>
	/// Loads a set of FX rates into memory from CSV resources.
	/// <para>
	/// The rates are expected to be in a CSV format, with the following header row:<br />
	/// {@code Valuation Date, Currency Pair, Value}.
	/// <ul>
	/// <li>The 'Valuation Date' column provides the valuation date, allowing data from different
	///  days to be stored in the same file
	/// <li>The 'Currency Pair' column is the currency pair in the format 'EUR/USD'.
	/// <li>The 'Value' column is the value of the rate.
	/// </ul>
	/// </para>
	/// <para>
	/// Each file may contain entries for many different dates.
	/// </para>
	/// <para>
	/// For example:
	/// <pre>
	/// Valuation Date, Currency Pair, Value
	/// 2014-01-22, EUR/USD, 1.10
	/// 2014-01-22, GBP/USD, 1.50
	/// 2014-01-23, EUR/USD, 1.11
	/// </pre>
	/// Note that Microsoft Excel prefers the CSV file to have no space after the comma.
	/// </para>
	/// <para>
	/// CSV files sometimes contain a Unicode Byte Order Mark.
	/// Callers are responsible for handling this, such as by using <seealso cref="UnicodeBom"/>.
	/// </para>
	/// </summary>
	public sealed class FxRatesCsvLoader
	{

	  // CSV column headers
	  private const string DATE_FIELD = "Valuation Date";
	  private const string CURRENCY_PAIR_FIELD = "Currency Pair";
	  private const string VALUE_FIELD = "Value";

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Loads one or more CSV format FX rate files for a specific date.
	  /// <para>
	  /// Only those rates that match the specified date will be loaded.
	  /// </para>
	  /// <para>
	  /// If the files contain a duplicate entry an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketDataDate">  the date to load </param>
	  /// <param name="resources">  the CSV resources </param>
	  /// <returns> the loaded FX rates, mapped by <seealso cref="FxRateId rate ID"/> </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableMap<FxRateId, FxRate> load(LocalDate marketDataDate, params ResourceLocator[] resources)
	  {
		return load(marketDataDate, Arrays.asList(resources));
	  }

	  /// <summary>
	  /// Loads one or more CSV format FX rate files for a specific date.
	  /// <para>
	  /// Only those rates that match the specified date will be loaded.
	  /// </para>
	  /// <para>
	  /// If the files contain a duplicate entry an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketDataDate">  the date to load </param>
	  /// <param name="resources">  the CSV resources </param>
	  /// <returns> the loaded FX rates, mapped by <seealso cref="FxRateId rate ID"/> </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableMap<FxRateId, FxRate> load(LocalDate marketDataDate, ICollection<ResourceLocator> resources)
	  {
		ICollection<CharSource> charSources = resources.Select(r => r.CharSource).ToList();
		return parse(d => marketDataDate.Equals(d), charSources).getOrDefault(marketDataDate, ImmutableMap.of());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Loads one or more CSV format FX rate files for a set of dates.
	  /// <para>
	  /// Only those rates that match one of the specified dates will be loaded.
	  /// </para>
	  /// <para>
	  /// If the files contain a duplicate entry an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketDataDates">  the set of dates to load </param>
	  /// <param name="resources">  the CSV resources </param>
	  /// <returns> the loaded FX rates, mapped by <seealso cref="LocalDate"/> and <seealso cref="FxRateId rate ID"/> </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableMap<LocalDate, ImmutableMap<FxRateId, FxRate>> load(ISet<LocalDate> marketDataDates, params ResourceLocator[] resources)
	  {

		return load(marketDataDates, Arrays.asList(resources));
	  }

	  /// <summary>
	  /// Loads one or more CSV format FX rate files for a set of dates.
	  /// <para>
	  /// Only those rates that match one of the specified dates will be loaded.
	  /// </para>
	  /// <para>
	  /// If the files contain a duplicate entry an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketDataDates">  the dates to load </param>
	  /// <param name="resources">  the CSV resources </param>
	  /// <returns> the loaded FX rates, mapped by <seealso cref="LocalDate"/> and <seealso cref="FxRateId rate ID"/> </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableMap<LocalDate, ImmutableMap<FxRateId, FxRate>> load(ISet<LocalDate> marketDataDates, ICollection<ResourceLocator> resources)
	  {

		ICollection<CharSource> charSources = resources.Select(r => r.CharSource).ToList();
		return parse(d => marketDataDates.Contains(d), charSources);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Loads one or more CSV format FX rate files.
	  /// <para>
	  /// All dates that are found will be returned.
	  /// </para>
	  /// <para>
	  /// If the files contain a duplicate entry an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resources">  the CSV resources </param>
	  /// <returns> the loaded FX rates, mapped by <seealso cref="LocalDate"/> and <seealso cref="FxRateId rate ID"/> </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableMap<LocalDate, ImmutableMap<FxRateId, FxRate>> loadAllDates(params ResourceLocator[] resources)
	  {
		return loadAllDates(Arrays.asList(resources));
	  }

	  /// <summary>
	  /// Loads one or more CSV format FX rate files.
	  /// <para>
	  /// All dates that are found will be returned.
	  /// </para>
	  /// <para>
	  /// If the files contain a duplicate entry an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resources">  the CSV resources </param>
	  /// <returns> the loaded FX rates, mapped by <seealso cref="LocalDate"/> and <seealso cref="FxRateId rate ID"/> </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableMap<LocalDate, ImmutableMap<FxRateId, FxRate>> loadAllDates(ICollection<ResourceLocator> resources)
	  {

		ICollection<CharSource> charSources = resources.Select(r => r.CharSource).ToList();
		return parse(d => true, charSources);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses one or more CSV format FX rate files.
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
	  /// <returns> the loaded FX rates, mapped by <seealso cref="LocalDate"/> and <seealso cref="FxRateId rate ID"/> </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableMap<LocalDate, ImmutableMap<FxRateId, FxRate>> parse(System.Predicate<LocalDate> datePredicate, ICollection<CharSource> charSources)
	  {

		// builder ensures keys can only be seen once
		IDictionary<LocalDate, ImmutableMap.Builder<FxRateId, FxRate>> mutableMap = new Dictionary<LocalDate, ImmutableMap.Builder<FxRateId, FxRate>>();
		foreach (CharSource charSource in charSources)
		{
		  parseSingle(datePredicate, charSource, mutableMap);
		}
		ImmutableMap.Builder<LocalDate, ImmutableMap<FxRateId, FxRate>> builder = ImmutableMap.builder();
		foreach (KeyValuePair<LocalDate, ImmutableMap.Builder<FxRateId, FxRate>> entry in mutableMap.SetOfKeyValuePairs())
		{
		  builder.put(entry.Key, entry.Value.build());
		}
		return builder.build();
	  }

	  // loads a single CSV file, filtering by date
	  private static void parseSingle(System.Predicate<LocalDate> datePredicate, CharSource resource, IDictionary<LocalDate, ImmutableMap.Builder<FxRateId, FxRate>> mutableMap)
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
			  string currencyPairStr = row.getField(CURRENCY_PAIR_FIELD);
			  string valueStr = row.getField(VALUE_FIELD);
			  CurrencyPair currencyPair = CurrencyPair.parse(currencyPairStr);
			  double value = Convert.ToDouble(valueStr);

			  ImmutableMap.Builder<FxRateId, FxRate> builderForDate = mutableMap.computeIfAbsent(date, k => ImmutableMap.builder());
			  builderForDate.put(FxRateId.of(currencyPair), FxRate.of(currencyPair, value));
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
	  private FxRatesCsvLoader()
	  {
	  }
	}

}