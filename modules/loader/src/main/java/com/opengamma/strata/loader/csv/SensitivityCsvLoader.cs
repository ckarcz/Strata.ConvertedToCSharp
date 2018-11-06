using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.result.FailureReason.PARSING;


	using ArrayListMultimap = com.google.common.collect.ArrayListMultimap;
	using ImmutableListMultimap = com.google.common.collect.ImmutableListMultimap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ListMultimap = com.google.common.collect.ListMultimap;
	using CharSource = com.google.common.io.CharSource;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using FloatingRateName = com.opengamma.strata.basics.index.FloatingRateName;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using CsvIterator = com.opengamma.strata.collect.io.CsvIterator;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using UnicodeBom = com.opengamma.strata.collect.io.UnicodeBom;
	using FailureItem = com.opengamma.strata.collect.result.FailureItem;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using ValueWithFailures = com.opengamma.strata.collect.result.ValueWithFailures;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using LabelDateParameterMetadata = com.opengamma.strata.market.param.LabelDateParameterMetadata;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using TenorDateParameterMetadata = com.opengamma.strata.market.param.TenorDateParameterMetadata;
	using TenorParameterMetadata = com.opengamma.strata.market.param.TenorParameterMetadata;
	using TenoredParameterMetadata = com.opengamma.strata.market.param.TenoredParameterMetadata;
	using CurveSensitivities = com.opengamma.strata.market.sensitivity.CurveSensitivities;
	using CurveSensitivitiesBuilder = com.opengamma.strata.market.sensitivity.CurveSensitivitiesBuilder;
	using CurveSensitivitiesType = com.opengamma.strata.market.sensitivity.CurveSensitivitiesType;
	using PortfolioItemInfo = com.opengamma.strata.product.PortfolioItemInfo;

	/// <summary>
	/// Loads sensitivities from CSV files.
	/// <para>
	/// The sensitivities are expected to be in a CSV format known to Strata.
	/// The parser currently supports two different CSV formats.
	/// Columns may occur in any order.
	/// 
	/// <h4>Standard format</h4>
	/// </para>
	/// <para>
	/// The following columns are supported:
	/// <ul>
	/// <li>'Id Scheme' (optional) - the name of the scheme that the identifier is unique within, defaulted to 'OG-Sensitivity'.
	/// <li>'Id' (optional) - the identifier of the sensitivity, such as 'SENS12345'.
	/// <li>'Reference' - a currency, floating rate name, index name or curve name.
	///   The standard reference name for a discount curve is the currency, such as 'GBP'.
	///   The standard reference name for a forward curve is the index name, such as 'GBP-LIBOR-3M'.
	///   Any curve name may be used however, which will be specific to the market data setup.
	/// <li>'Sensitivity Type' - defines the type of the sensitivity value, such as 'ZeroRateDelta' or 'ZeroRateGamma'.
	/// <li>'Sensitivity Tenor' - the tenor of the bucketed sensitivity, such as '1Y'.
	/// <li>'Sensitivity Date' (optional) - the date of the bucketed sensitivity, such as '2018-06-01'.
	/// <li>'Currency' (optional) - the currency of each sensitivity value, such as 'GBP'.
	///   If omitted, the currency will be implied from the reference, which must start with the currency.
	/// <li>'Value' - the sensitivity value
	/// </ul>
	/// </para>
	/// <para>
	/// The identifier columns are not normally present as the identifier is completely optional.
	/// If present, the values must be repeated for each row that forms part of one sensitivity.
	/// If the parser finds a different identifier, it will create a second sensitivity instance.
	/// </para>
	/// <para>
	/// When parsing the value column, if the cell is empty, the combination of type/reference/tenor/value
	/// will not be added to the result, so use an explicit zero to include a zero value.
	/// 
	/// <h4>List format</h4>
	/// </para>
	/// <para>
	/// The following columns are supported:
	/// <ul>
	/// <li>'Id Scheme' (optional) - the name of the scheme that the identifier is unique within, defaulted to 'OG-Sensitivity'.
	/// <li>'Id' (optional) - the identifier of the sensitivity, such as 'SENS12345'.
	/// <li>'Reference' - a currency, floating rate name, index name or curve name.
	///   The standard reference name for a discount curve is the currency, such as 'GBP'.
	///   The standard reference name for a forward curve is the index name, such as 'GBP-LIBOR-3M'.
	///   Any curve name may be used however, which will be specific to the market data setup.
	/// <li>'Sensitivity Tenor' - the tenor of the bucketed sensitivity, such as '1Y'.
	/// <li>'Sensitivity Date' (optional) - the date of the bucketed sensitivity, such as '2018-06-01'.
	/// <li>'Currency' (optional) - the currency of each sensitivity value, such as 'GBP'.
	///   If omitted, the currency will be implied from the reference, which must start with the currency.
	/// <li>one or more sensitivity value columns, the type of the sensitivity is specified by the header name,
	///   such as 'ZeroRateDelta'.
	/// </ul>
	/// </para>
	/// <para>
	/// The identifier columns are not normally present as the identifier is completely optional.
	/// If present, the values must be repeated for each row that forms part of one sensitivity.
	/// If the parser finds a different identifier, it will create a second sensitivity instance.
	/// </para>
	/// <para>
	/// When parsing the value columns, if the cell is empty, the combination of type/reference/tenor/value
	/// will not be added to the result, so use an explicit zero to include a zero value.
	/// 
	/// <h4>Grid format</h4>
	/// </para>
	/// <para>
	/// The following columns are supported:<br />
	/// <ul>
	/// <li>'Id Scheme' (optional) - the name of the scheme that the identifier is unique within, defaulted to 'OG-Sensitivity'.
	/// <li>'Id' (optional) - the identifier of the sensitivity, such as 'SENS12345'.
	/// <li>'Sensitivity Type' - defines the type of the sensitivity value, such as 'ZeroRateDelta' or 'ZeroRateGamma'.
	/// <li>'Sensitivity Tenor' - the tenor of the bucketed sensitivity, such as '1Y'.
	/// <li>'Sensitivity Date' (optional) - the date of the bucketed sensitivity, such as '2018-06-01'.
	/// <li>'Currency' (optional) - the currency of each sensitivity value, such as 'GBP'.
	///   If omitted, the currency will be implied from the reference, which must start with the currency.
	/// <li>one or more sensitivity value columns, the reference of the sensitivity is specified by the header name.
	///   The reference can be a currency, floating rate name, index name or curve name.
	///   The standard reference name for a discount curve is the currency, such as 'GBP'.
	///   The standard reference name for a forward curve is the index name, such as 'GBP-LIBOR-3M'.
	///   Any curve name may be used however, which will be specific to the market data setup.
	/// </ul>
	/// </para>
	/// <para>
	/// The identifier columns are not normally present as the identifier is completely optional.
	/// If present, the values must be repeated for each row that forms part of one sensitivity.
	/// If the parser finds a different identifier, it will create a second sensitivity instance.
	/// </para>
	/// <para>
	/// When parsing the value columns, if the cell is empty, the combination of type/reference/tenor/value
	/// will not be added to the result, so use an explicit zero to include a zero value.
	/// 
	/// <h4>Resolver</h4>
	/// The standard resolver will ensure that the sensitivity always has a tenor and
	/// implements <seealso cref="TenoredParameterMetadata"/>.
	/// The resolver can be adjusted to allow date-only metadata (thereby making the 'Sensitivity Tenor' column optional.
	/// The resolver can manipulate the tenor and/or curve name that is parsed if desired.
	/// </para>
	/// </summary>
	public sealed class SensitivityCsvLoader
	{

	  // default schemes
	  internal const string DEFAULT_SCHEME = "OG-Sensitivity";

	  // CSV column headers
	  private const string ID_SCHEME_HEADER = "Id Scheme";
	  private const string ID_HEADER = "Id";
	  internal const string REFERENCE_HEADER = "Reference";
	  internal const string TYPE_HEADER = "Sensitivity Type";
	  internal const string TENOR_HEADER = "Sensitivity Tenor";
	  internal const string DATE_HEADER = "Sensitivity Date";
	  internal const string CURRENCY_HEADER = "Currency";
	  internal const string VALUE_HEADER = "Value";
	  private static readonly ImmutableSet<string> TYPE_HEADERS = ImmutableSet.of(ID_SCHEME_HEADER.ToLower(Locale.ENGLISH), ID_HEADER.ToLower(Locale.ENGLISH), REFERENCE_HEADER.ToLower(Locale.ENGLISH), TENOR_HEADER.ToLower(Locale.ENGLISH), DATE_HEADER.ToLower(Locale.ENGLISH), CURRENCY_HEADER.ToLower(Locale.ENGLISH));
	  private static readonly ImmutableSet<string> REF_HEADERS = ImmutableSet.of(ID_SCHEME_HEADER.ToLower(Locale.ENGLISH), ID_HEADER.ToLower(Locale.ENGLISH), TYPE_HEADER.ToLower(Locale.ENGLISH), TENOR_HEADER.ToLower(Locale.ENGLISH), DATE_HEADER.ToLower(Locale.ENGLISH), CURRENCY_HEADER.ToLower(Locale.ENGLISH));

	  /// <summary>
	  /// The resolver, providing additional information.
	  /// </summary>
	  private readonly SensitivityCsvInfoResolver resolver;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance that uses the standard set of reference data.
	  /// </summary>
	  /// <returns> the loader </returns>
	  public static SensitivityCsvLoader standard()
	  {
		return new SensitivityCsvLoader(SensitivityCsvInfoResolver.standard());
	  }

	  /// <summary>
	  /// Obtains an instance that uses the specified set of reference data.
	  /// </summary>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the loader </returns>
	  public static SensitivityCsvLoader of(ReferenceData refData)
	  {
		return new SensitivityCsvLoader(SensitivityCsvInfoResolver.of(refData));
	  }

	  /// <summary>
	  /// Obtains an instance that uses the specified resolver for additional information.
	  /// </summary>
	  /// <param name="resolver">  the resolver used to parse additional information </param>
	  /// <returns> the loader </returns>
	  public static SensitivityCsvLoader of(SensitivityCsvInfoResolver resolver)
	  {
		return new SensitivityCsvLoader(resolver);
	  }

	  // restricted constructor
	  private SensitivityCsvLoader(SensitivityCsvInfoResolver resolver)
	  {
		this.resolver = ArgChecker.notNull(resolver, "resolver");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks whether the source is a CSV format sensitivities file.
	  /// <para>
	  /// This parses the headers as CSV and checks that mandatory headers are present.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="charSource">  the CSV character source to check </param>
	  /// <returns> true if the source is a CSV file with known headers, false otherwise </returns>
	  public bool isKnownFormat(CharSource charSource)
	  {
		try
		{
				using (CsvIterator csv = CsvIterator.of(charSource, true))
				{
			  if (!csv.containsHeader(TENOR_HEADER) && !csv.containsHeader(DATE_HEADER))
			  {
				return false;
			  }
			  if (csv.containsHeader(REFERENCE_HEADER) && csv.containsHeader(TYPE_HEADER) && csv.containsHeader(VALUE_HEADER))
			  {
				return true; // standard format
			  }
			  else if (csv.containsHeader(REFERENCE_HEADER) || csv.containsHeader(TYPE_HEADER))
			  {
				return true; // list or grid format
			  }
			  else
			  {
				return csv.headers().Any(SensitivityCsvLoader.knownReference); // implied grid format
			  }
				}
		}
		catch (Exception)
		{
		  return false;
		}
	  }

	  // for historical compatibility, we determine known format by looking for these specific things
	  // the new approach is to require either the 'Reference' or the 'Sensitivity Type' column
	  private static bool knownReference(string refStr)
	  {
		try
		{
		  Optional<IborIndex> ibor = IborIndex.extendedEnum().find(refStr);
		  if (ibor.Present)
		  {
			return true;
		  }
		  else
		  {
			Optional<FloatingRateName> frName = FloatingRateName.extendedEnum().find(refStr);
			if (frName.Present)
			{
			  return true;
			}
			else if (refStr.Length == 3)
			{
			  Currency.of(refStr); // this may throw an exception validating the string
			  return true;
			}
			else
			{
			  return false;
			}
		  }
		}
		catch (Exception)
		{
		  return false;
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Loads one or more CSV format sensitivities files.
	  /// <para>
	  /// In most cases each file contains one sensitivity instance, however the file format is capable
	  /// of representing any number.
	  /// </para>
	  /// <para>
	  /// Within a single file and identifier, the same combination of type, reference and tenor must not be repeated.
	  /// No checks are performed between different input files.
	  /// It may be useful to merge the sensitivities in the resulting list in a separate step after parsing.
	  /// </para>
	  /// <para>
	  /// CSV files sometimes contain a Unicode Byte Order Mark.
	  /// This method uses <seealso cref="UnicodeBom"/> to interpret it.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resources">  the CSV resources </param>
	  /// <returns> the sensitivities keyed by identifier, parsing errors are captured in the result </returns>
	  public ValueWithFailures<ListMultimap<string, CurveSensitivities>> load(ICollection<ResourceLocator> resources)
	  {
		ICollection<CharSource> charSources = resources.Select(r => UnicodeBom.toCharSource(r.ByteSource)).ToList();
		return parse(charSources);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses one or more CSV format position files, returning sensitivities.
	  /// <para>
	  /// The standard way to write sensitivities files is for each file to contain one sensitivity instance.
	  /// The file format can handle multiple instances per file, where each instance has a separate identifier.
	  /// Most files will not have the identifier columns, thus the identifier will be the empty string.
	  /// </para>
	  /// <para>
	  /// The returned multimap is keyed by identifier. The value will contain one entry for each instance.
	  /// If desired, the results can be reduced using <seealso cref="CurveSensitivities#mergedWith(CurveSensitivities)"/>
	  /// to merge those with the same identifier.
	  /// </para>
	  /// <para>
	  /// CSV files sometimes contain a Unicode Byte Order Mark.
	  /// Callers are responsible for handling this, such as by using <seealso cref="UnicodeBom"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="charSources">  the CSV character sources </param>
	  /// <returns> the loaded sensitivities, parsing errors are captured in the result </returns>
	  public ValueWithFailures<ListMultimap<string, CurveSensitivities>> parse(ICollection<CharSource> charSources)
	  {
		ListMultimap<string, CurveSensitivities> parsed = ArrayListMultimap.create();
		IList<FailureItem> failures = new List<FailureItem>();
		foreach (CharSource charSource in charSources)
		{
		  parse(charSource, parsed, failures);
		}
		return ValueWithFailures.of(ImmutableListMultimap.copyOf(parsed), failures);
	  }

	  // parse a single file
	  private void parse(CharSource charSource, ListMultimap<string, CurveSensitivities> parsed, IList<FailureItem> failures)
	  {

		try
		{
				using (CsvIterator csv = CsvIterator.of(charSource, true))
				{
			  if (!csv.containsHeader(TENOR_HEADER) && !csv.containsHeader(DATE_HEADER))
			  {
				failures.Add(FailureItem.of(FailureReason.PARSING, "CSV file could not be parsed as sensitivities, invalid format"));
			  }
			  else if (csv.containsHeader(REFERENCE_HEADER) && csv.containsHeader(TYPE_HEADER) && csv.containsHeader(VALUE_HEADER))
			  {
				parseStandardFormat(csv, parsed, failures);
			  }
			  else if (csv.containsHeader(REFERENCE_HEADER))
			  {
				parseListFormat(csv, parsed, failures);
			  }
			  else
			  {
				parseGridFormat(csv, parsed, failures);
			  }
				}
		}
		catch (Exception ex)
		{
		  failures.Add(FailureItem.of(FailureReason.PARSING, ex, "CSV file could not be parsed: {}", ex.Message));
		}
	  }

	  //-------------------------------------------------------------------------
	  // parses the file in standard format
	  private void parseStandardFormat(CsvIterator csv, ListMultimap<string, CurveSensitivities> parsed, IList<FailureItem> failures)
	  {

		// loop around all rows, peeking to match batches with the same identifier
		// no exception catch at this level to avoid infinite loops
		while (csv.hasNext())
		{
		  CsvRow peekedRow = csv.peek();
		  PortfolioItemInfo info = parseInfo(peekedRow);
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		  string id = info.Id.map(StandardId::toString).orElse("");

		  // process in batches, where the ID is the same
		  CurveSensitivitiesBuilder builder = CurveSensitivities.builder(info);
		  IList<CsvRow> batchRows = csv.nextBatch(r => matchId(r, id));
		  foreach (CsvRow batchRow in batchRows)
		  {
			try
			{
			  CurveName reference = CurveName.of(batchRow.getValue(REFERENCE_HEADER));
			  CurveName resolvedCurveName = resolver.checkCurveName(reference);
			  CurveSensitivitiesType type = CurveSensitivitiesType.of(batchRow.getValue(TYPE_HEADER));
			  ParameterMetadata metadata = parseMetadata(batchRow, false);
			  Currency currency = parseCurrency(batchRow, reference);
			  string valueStr = batchRow.getField(VALUE_HEADER);
			  if (valueStr.Length > 0)
			  {
				double value = LoaderUtils.parseDouble(valueStr);
				builder.add(type, resolvedCurveName, currency, metadata, value);
			  }

			}
			catch (System.ArgumentException ex)
			{
			  failures.Add(FailureItem.of(PARSING, "CSV file could not be parsed at line {}: {}", batchRow.lineNumber(), ex.Message));
			}
		  }
		  CurveSensitivities sens = builder.build();
		  if (!sens.TypedSensitivities.Empty)
		  {
			parsed.put(sens.Id.map(object.toString).orElse(""), sens);
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  // parses the file in list format
	  private void parseListFormat(CsvIterator csv, ListMultimap<string, CurveSensitivities> parsed, IList<FailureItem> failures)
	  {

		// find the applicable type columns
		IDictionary<string, CurveSensitivitiesType> types = new LinkedHashMap<string, CurveSensitivitiesType>();
		foreach (string header in csv.headers())
		{
		  string headerLowerCase = header.ToLower(Locale.ENGLISH);
		  if (!TYPE_HEADERS.contains(headerLowerCase) && !resolver.isInfoColumn(headerLowerCase))
		  {
			types[header] = CurveSensitivitiesType.of(header.Replace(" ", ""));
		  }
		}

		// loop around all rows, peeking to match batches with the same identifier
		// no exception catch at this level to avoid infinite loops
		while (csv.hasNext())
		{
		  CsvRow peekedRow = csv.peek();
		  PortfolioItemInfo info = parseInfo(peekedRow);
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		  string id = info.Id.map(StandardId::toString).orElse("");

		  // process in batches, where the ID is the same
		  CurveSensitivitiesBuilder builder = CurveSensitivities.builder(info);
		  IList<CsvRow> batchRows = csv.nextBatch(r => matchId(r, id));
		  foreach (CsvRow batchRow in batchRows)
		  {
			try
			{
			  ParameterMetadata metadata = parseMetadata(batchRow, true);
			  CurveName reference = CurveName.of(batchRow.getValue(REFERENCE_HEADER));
			  CurveName resolvedCurveName = resolver.checkCurveName(reference);
			  foreach (KeyValuePair<string, CurveSensitivitiesType> entry in types.SetOfKeyValuePairs())
			  {
				CurveSensitivitiesType type = entry.Value;
				string valueStr = batchRow.getField(entry.Key);
				Currency currency = parseCurrency(batchRow, reference);
				if (valueStr.Length > 0)
				{
				  double value = LoaderUtils.parseDouble(valueStr);
				  builder.add(type, resolvedCurveName, currency, metadata, value);
				}
			  }

			}
			catch (System.ArgumentException ex)
			{
			  failures.Add(FailureItem.of(PARSING, "CSV file could not be parsed at line {}: {}", batchRow.lineNumber(), ex.Message));
			}
		  }
		  CurveSensitivities sens = builder.build();
		  if (!sens.TypedSensitivities.Empty)
		  {
			parsed.put(sens.Id.map(object.toString).orElse(""), sens);
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  // parses the file in grid format
	  private void parseGridFormat(CsvIterator csv, ListMultimap<string, CurveSensitivities> parsed, IList<FailureItem> failures)
	  {

		// find the applicable reference columns
		IDictionary<string, CurveName> references = new LinkedHashMap<string, CurveName>();
		foreach (string header in csv.headers())
		{
		  string headerLowerCase = header.ToLower(Locale.ENGLISH);
		  if (!REF_HEADERS.contains(headerLowerCase) && !resolver.isInfoColumn(headerLowerCase))
		  {
			references[header] = CurveName.of(header);
		  }
		}

		// loop around all rows, peeking to match batches with the same identifier
		// no exception catch at this level to avoid infinite loops
		while (csv.hasNext())
		{
		  CsvRow peekedRow = csv.peek();
		  PortfolioItemInfo info = parseInfo(peekedRow);
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		  string id = info.Id.map(StandardId::toString).orElse("");

		  // process in batches, where the ID is the same
		  CurveSensitivitiesBuilder builder = CurveSensitivities.builder(info);
		  IList<CsvRow> batchRows = csv.nextBatch(r => matchId(r, id));
		  foreach (CsvRow batchRow in batchRows)
		  {
			try
			{
			  ParameterMetadata metadata = parseMetadata(batchRow, true);
			  CurveSensitivitiesType type = batchRow.findValue(TYPE_HEADER).map(str => CurveSensitivitiesType.of(str)).orElse(CurveSensitivitiesType.ZERO_RATE_DELTA);
			  foreach (KeyValuePair<string, CurveName> entry in references.SetOfKeyValuePairs())
			  {
				CurveName reference = entry.Value;
				CurveName resolvedCurveName = resolver.checkCurveName(reference);
				string valueStr = batchRow.getField(entry.Key);
				Currency currency = parseCurrency(batchRow, reference);
				if (valueStr.Length > 0)
				{
				  double value = LoaderUtils.parseDouble(valueStr);
				  builder.add(type, resolvedCurveName, currency, metadata, value);
				}
			  }

			}
			catch (System.ArgumentException ex)
			{
			  failures.Add(FailureItem.of(PARSING, "CSV file could not be parsed at line {}: {}", batchRow.lineNumber(), ex.Message));
			}
		  }
		  CurveSensitivities sens = builder.build();
		  if (!sens.TypedSensitivities.Empty)
		  {
			parsed.put(sens.Id.map(object.toString).orElse(""), sens);
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  // parses the currency as a column or from the reference
	  private static Currency parseCurrency(CsvRow row, CurveName reference)
	  {
		Optional<string> currencyStr = row.findValue(CURRENCY_HEADER);
		if (currencyStr.Present)
		{
		  return LoaderUtils.parseCurrency(currencyStr.get());
		}
		string referenceStr = reference.Name.ToUpper(Locale.ENGLISH);
		try
		{
		  Optional<IborIndex> ibor = IborIndex.extendedEnum().find(referenceStr);
		  if (ibor.Present)
		  {
			return ibor.get().Currency;
		  }
		  else
		  {
			Optional<FloatingRateName> frName = FloatingRateName.extendedEnum().find(referenceStr);
			if (frName.Present)
			{
			  return frName.get().Currency;
			}
			else if (referenceStr.Length == 3)
			{
			  return Currency.of(referenceStr);
			}
			else if (referenceStr.Length > 3 && referenceStr[3] == '-' || referenceStr[3] == '_')
			{
			  return LoaderUtils.parseCurrency(referenceStr.Substring(0, 3));
			}
			else
			{
			  // drop out to exception
			}
		  }
		}
		catch (Exception)
		{
		  // drop out to exception
		}
		throw new System.ArgumentException("Unable to parse currency from reference, consider adding a 'Currency' column");
	  }

	  // parses the currency as a column or from the reference
	  private ParameterMetadata parseMetadata(CsvRow row, bool lenientDateParsing)
	  {
		// parse the tenor and date fields
		Optional<Tenor> tenorOpt = row.findValue(TENOR_HEADER).flatMap(LoaderUtils.tryParseTenor);
		Optional<LocalDate> dateOpt = row.findValue(DATE_HEADER).map(LoaderUtils.parseDate);
		Optional<string> tenorStrOpt = row.findValue(TENOR_HEADER);
		if (tenorStrOpt.Present && !tenorOpt.Present)
		{
		  if (lenientDateParsing && !dateOpt.Present && !resolver.TenorRequired)
		  {
			try
			{
			  dateOpt = tenorStrOpt.map(LoaderUtils.parseDate);
			}
			catch (Exception)
			{
			  // hide this exception, as this is a historic format
			  throw new System.ArgumentException(Messages.format("Invalid tenor '{}', must be expressed as nD, nW, nM or nY", tenorStrOpt.get()));
			}
		  }
		  else
		  {
			throw new System.ArgumentException(Messages.format("Invalid tenor '{}', must be expressed as nD, nW, nM or nY", tenorStrOpt.get()));
		  }
		}
		// build correct metadata based on the parsed fields
		if (tenorOpt.Present)
		{
		  Tenor tenor = resolver.checkSensitivityTenor(tenorOpt.get());
		  if (dateOpt.Present)
		  {
			return TenorDateParameterMetadata.of(dateOpt.get(), tenor);
		  }
		  else
		  {
			return TenorParameterMetadata.of(tenor);
		  }
		}
		else if (resolver.TenorRequired)
		{
		  throw new System.ArgumentException(Messages.format("Missing value for '{}' column", TENOR_HEADER));
		}
		else if (dateOpt.Present)
		{
		  return LabelDateParameterMetadata.of(dateOpt.get(), dateOpt.get().ToString());
		}
		else
		{
		  throw new System.ArgumentException(Messages.format("Unable to parse tenor or date, check '{}' and '{}' columns", TENOR_HEADER, DATE_HEADER));
		}
	  }

	  //-------------------------------------------------------------------------
	  // parse the sensitivity info
	  private PortfolioItemInfo parseInfo(CsvRow row)
	  {
		PortfolioItemInfo info = PortfolioItemInfo.empty();
		string scheme = row.findValue(ID_SCHEME_HEADER).orElse(DEFAULT_SCHEME);
		StandardId id = row.findValue(ID_HEADER).map(str => StandardId.of(scheme, str)).orElse(null);
		if (id != null)
		{
		  info = info.withId(id);
		}
		return resolver.parseSensitivityInfo(row, info);
	  }

	  // checks if the identifier in the row matches the previous one
	  private static bool matchId(CsvRow row, string id)
	  {
		string scheme = row.findValue(ID_SCHEME_HEADER).orElse(DEFAULT_SCHEME);
		string rowId = row.findValue(ID_HEADER).map(str => StandardId.of(scheme, str).ToString()).orElse("");
		return id.Equals(rowId);
	  }

	}

}