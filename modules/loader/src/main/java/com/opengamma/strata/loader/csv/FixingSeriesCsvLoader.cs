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
	using CharSource = com.google.common.io.CharSource;
	using Index = com.opengamma.strata.basics.index.Index;
	using PriceIndex = com.opengamma.strata.basics.index.PriceIndex;
	using MapStream = com.opengamma.strata.collect.MapStream;
	using Messages = com.opengamma.strata.collect.Messages;
	using CsvFile = com.opengamma.strata.collect.io.CsvFile;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using UnicodeBom = com.opengamma.strata.collect.io.UnicodeBom;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using LocalDateDoubleTimeSeriesBuilder = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeriesBuilder;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;

	/// <summary>
	/// Loads a set of historical fixing series into memory from CSV resources.
	/// <para>
	/// The resources are expected to be in a CSV format, with the following header row:<br />
	/// {@code Reference, Date, Value}.
	/// <ul>
	/// <li>The 'Reference' column is the name of the index that the data is for, such as 'USD-LIBOR-3M'.
	/// <li>The 'Date' column is the date that the fixing was taken, this should be a year-month for price indices.
	/// <li>The 'Value' column is the fixed value.
	/// </ul>
	/// </para>
	/// <para>
	/// Each fixing series must be contained entirely within a single resource, but each resource may
	/// contain more than one series. The fixing series points do not need to be ordered.
	/// </para>
	/// <para>
	/// For example:
	/// <pre>
	/// Reference, Date, Value
	/// USD-LIBOR-3M, 1971-01-04, 0.065
	/// USD-LIBOR-3M, 1971-01-05, 0.0638
	/// USD-LIBOR-3M, 1971-01-06, 0.0638
	/// </pre>
	/// Note that Microsoft Excel prefers the CSV file to have no space after the comma.
	/// </para>
	/// <para>
	/// CSV files sometimes contain a Unicode Byte Order Mark.
	/// Callers are responsible for handling this, such as by using <seealso cref="UnicodeBom"/>.
	/// </para>
	/// </summary>
	public sealed class FixingSeriesCsvLoader
	{

	  // CSV column headers
	  private const string REFERENCE_FIELD = "Reference";
	  private const string DATE_FIELD = "Date";
	  private const string VALUE_FIELD = "Value";

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Loads one or more CSV format fixing series files.
	  /// <para>
	  /// If the files contain a duplicate entry an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resources">  the fixing series CSV resources </param>
	  /// <returns> the loaded fixing series, mapped by <seealso cref="ObservableId observable ID"/> </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableMap<ObservableId, LocalDateDoubleTimeSeries> load(params ResourceLocator[] resources)
	  {
		return load(Arrays.asList(resources));
	  }

	  /// <summary>
	  /// Loads one or more CSV format fixing series files.
	  /// <para>
	  /// If the files contain a duplicate entry an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resources">  the fixing series CSV resources </param>
	  /// <returns> the loaded fixing series, mapped by <seealso cref="ObservableId observable ID"/> </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableMap<ObservableId, LocalDateDoubleTimeSeries> load(ICollection<ResourceLocator> resources)
	  {
		ICollection<CharSource> charSources = resources.Select(r => r.CharSource).ToList();
		return parse(charSources);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses one or more CSV format fixing series files.
	  /// <para>
	  /// If the files contain a duplicate entry an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="charSources">  the fixing series CSV character sources </param>
	  /// <returns> the loaded fixing series, mapped by <seealso cref="ObservableId observable ID"/> </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableMap<ObservableId, LocalDateDoubleTimeSeries> parse(ICollection<CharSource> charSources)
	  {
		// builder ensures keys can only be seen once
		ImmutableMap.Builder<ObservableId, LocalDateDoubleTimeSeries> builder = ImmutableMap.builder();
		foreach (CharSource charSource in charSources)
		{
		  builder.putAll(parseSingle(charSource));
		}
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  // loads a single fixing series CSV file
	  private static ImmutableMap<ObservableId, LocalDateDoubleTimeSeries> parseSingle(CharSource resource)
	  {
		IDictionary<ObservableId, LocalDateDoubleTimeSeriesBuilder> builders = new Dictionary<ObservableId, LocalDateDoubleTimeSeriesBuilder>();
		try
		{
		  CsvFile csv = CsvFile.of(resource, true);
		  foreach (CsvRow row in csv.rows())
		  {
			string referenceStr = row.getField(REFERENCE_FIELD);
			string dateStr = row.getField(DATE_FIELD);
			string valueStr = row.getField(VALUE_FIELD);

			Index index = LoaderUtils.findIndex(referenceStr);
			ObservableId id = IndexQuoteId.of(index);
			double value = double.Parse(valueStr);
			LocalDate date;
			if (index is PriceIndex)
			{
			  try
			  {
				YearMonth ym = LoaderUtils.parseYearMonth(dateStr);
				date = ym.atEndOfMonth();
			  }
			  catch (Exception)
			  {
				date = LoaderUtils.parseDate(dateStr);
				if (date.DayOfMonth != date.lengthOfMonth())
				{
				  throw new System.ArgumentException(Messages.format("Fixing Series CSV loader for price index must have date at end of month: {}", resource));
				}
			  }
			}
			else
			{
			  date = LoaderUtils.parseDate(dateStr);
			}

			LocalDateDoubleTimeSeriesBuilder builder = builders.computeIfAbsent(id, k => LocalDateDoubleTimeSeries.builder());
			builder.put(date, value);
		  }
		}
		catch (Exception ex)
		{
		  throw new System.ArgumentException(Messages.format("Error processing resource as CSV file: {}", resource), ex);
		}
		return MapStream.of(builders).mapValues(builder => builder.build()).toMap();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private FixingSeriesCsvLoader()
	  {
	  }

	}

}