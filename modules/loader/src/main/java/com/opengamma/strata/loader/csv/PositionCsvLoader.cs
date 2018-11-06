using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{


	using ImmutableList = com.google.common.collect.ImmutableList;
	using CharSource = com.google.common.io.CharSource;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Guavate = com.opengamma.strata.collect.Guavate;
	using CsvIterator = com.opengamma.strata.collect.io.CsvIterator;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using UnicodeBom = com.opengamma.strata.collect.io.UnicodeBom;
	using FailureItem = com.opengamma.strata.collect.result.FailureItem;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using ValueWithFailures = com.opengamma.strata.collect.result.ValueWithFailures;
	using GenericSecurityPosition = com.opengamma.strata.product.GenericSecurityPosition;
	using Position = com.opengamma.strata.product.Position;
	using PositionInfo = com.opengamma.strata.product.PositionInfo;
	using PositionInfoBuilder = com.opengamma.strata.product.PositionInfoBuilder;
	using ResolvableSecurityPosition = com.opengamma.strata.product.ResolvableSecurityPosition;
	using SecurityPosition = com.opengamma.strata.product.SecurityPosition;
	using EtdContractSpec = com.opengamma.strata.product.etd.EtdContractSpec;
	using EtdContractSpecId = com.opengamma.strata.product.etd.EtdContractSpecId;
	using EtdFuturePosition = com.opengamma.strata.product.etd.EtdFuturePosition;
	using EtdIdUtils = com.opengamma.strata.product.etd.EtdIdUtils;
	using EtdOptionPosition = com.opengamma.strata.product.etd.EtdOptionPosition;
	using EtdOptionType = com.opengamma.strata.product.etd.EtdOptionType;
	using EtdPosition = com.opengamma.strata.product.etd.EtdPosition;
	using EtdSettlementType = com.opengamma.strata.product.etd.EtdSettlementType;

	/// <summary>
	/// Loads positions from CSV files.
	/// <para>
	/// The positions are expected to be in a CSV format known to Strata.
	/// The parser is flexible, understanding a number of different ways to define each position.
	/// Columns may occur in any order.
	/// 
	/// <h4>Common</h4>
	/// </para>
	/// <para>
	/// The following standard columns are supported:<br />
	/// <ul>
	/// <li>The 'Strata Position Type' column is optional, but mandatory when checking headers
	/// to see if the file is a known format. It defines the instrument type,
	///   'SEC' or'Security' for standard securities,
	///   'FUT' or 'Future' for ETD futures, and
	///   'OPT' or 'Option' for ETD options.
	///   If absent, the type is derived based on the presence or absence of the 'Expiry' column.
	/// <li>The 'Id Scheme' column is optional, and is the name of the scheme that the position
	///   identifier is unique within, such as 'OG-Position'.
	/// <li>The 'Id' column is optional, and is the identifier of the position,
	///   such as 'POS12345'.
	/// </ul>
	/// 
	/// <h4>SEC/Security</h4>
	/// </para>
	/// <para>
	/// The following columns are supported:
	/// <ul>
	/// <li>'Security Id Scheme' - optional, defaults to 'OG-Security'
	/// <li>'Security Id' - mandatory
	/// <li>'Quantity' - see below
	/// <li>'Long Quantity' - see below
	/// <li>'Short Quantity' - see below
	/// </ul>
	/// </para>
	/// <para>
	/// The quantity will normally be set from the 'Quantity' column.
	/// If that column is not found, the 'Long Quantity' and 'Short Quantity' columns will be used instead.
	/// 
	/// <h4>FUT/Future</h4>
	/// </para>
	/// <para>
	/// The following columns are supported:
	/// <ul>
	/// <li>'Exchange' - mandatory, the MIC code of the exchange where the ETD is traded
	/// <li>'Contract Code' - mandatory, the contract code of the ETD at the exchange
	/// <li>'Quantity' - see below
	/// <li>'Long Quantity' - see below
	/// <li>'Short Quantity' - see below
	/// <li>'Expiry' - mandatory, the year-month of the expiry, in the format 'yyyy-MM'
	/// <li>'Expiry Week' - optional, only used to obtain a weekly-expiring ETD
	/// <li>'Expiry Day' - optional, only used to obtain a daily-expiring ETD, or Flex
	/// <li>'Settlement Type' - optional, only used for Flex, see <seealso cref="EtdSettlementType"/>
	/// </ul>
	/// </para>
	/// <para>
	/// The exchange and contract code are combined to form an <seealso cref="EtdContractSpecId"/> which is
	/// resolved in <seealso cref="ReferenceData"/> to find additional details about the ETD.
	/// This process can be changed by providing an alternative <seealso cref="PositionCsvInfoResolver"/>.
	/// </para>
	/// <para>
	/// The quantity will normally be set from the 'Quantity' column.
	/// If that column is not found, the 'Long Quantity' and 'Short Quantity' columns will be used instead.
	/// </para>
	/// <para>
	/// The expiry is normally controlled using just the 'Expiry' column.
	/// Flex options will also set the 'Expiry Day' and 'Settlement Type'.
	/// 
	/// <h4>OPT/Option</h4>
	/// </para>
	/// <para>
	/// The following columns are supported:
	/// <ul>
	/// <li>'Exchange' - mandatory, the MIC code of the exchange where the ETD is traded
	/// <li>'Contract Code' - mandatory, the contract code of the ETD at the exchange
	/// <li>'Quantity' - see below
	/// <li>'Long Quantity' - see below
	/// <li>'Short Quantity' - see below
	/// <li>'Expiry' - mandatory, the year-month of the expiry, in the format 'yyyy-MM'
	/// <li>'Expiry Week' - optional, only used to obtain a weekly-expiring ETD
	/// <li>'Expiry Day' - optional, only used to obtain a daily-expiring ETD, or Flex
	/// <li>'Settlement Type' - optional, only used for Flex, see <seealso cref="EtdSettlementType"/>
	/// <li>'Exercise Style' - optional,  only used for Flex, see <seealso cref="EtdOptionType"/>
	/// <li>'Put Call' - mandatory,  'Put', 'P', 'Call' or 'C'
	/// <li>'Exercise Price' - mandatory,  the strike price, such as 1.23
	/// <li>'Version' - optional, the version of the contract, not widely used, defaults to zero
	/// <li>'Underlying Expiry' - optional, the expiry year-month of the underlying instrument if applicable, in the format 'yyyy-MM'
	/// </ul>
	/// </para>
	/// <para>
	/// The exchange and contract code are combined to form an <seealso cref="EtdContractSpecId"/> which is
	/// resolved in <seealso cref="ReferenceData"/> to find additional details about the ETD.
	/// This process can be changed by providing an alternative <seealso cref="PositionCsvInfoResolver"/>.
	/// </para>
	/// <para>
	/// The quantity will normally be set from the 'Quantity' column.
	/// If that column is not found, the 'Long Quantity' and 'Short Quantity' columns will be used instead.
	/// </para>
	/// <para>
	/// The expiry is normally controlled using just the 'Expiry' column.
	/// Flex options will also set the 'Expiry Day', 'Settlement Type' and 'Exercise Style'.
	/// </para>
	/// </summary>
	public sealed class PositionCsvLoader
	{

	  // default schemes
	  internal const string DEFAULT_POSITION_SCHEME = "OG-Position";
	  internal const string DEFAULT_SECURITY_SCHEME = "OG-Security";

	  // CSV column headers
	  private const string TYPE_FIELD = "Strata Position Type";
	  private const string ID_SCHEME_FIELD = "Id Scheme";
	  private const string ID_FIELD = "Id";

	  /// <summary>
	  /// The resolver, providing additional information.
	  /// </summary>
	  private readonly PositionCsvInfoResolver resolver;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance that uses the standard set of reference data.
	  /// </summary>
	  /// <returns> the loader </returns>
	  public static PositionCsvLoader standard()
	  {
		return new PositionCsvLoader(PositionCsvInfoResolver.standard());
	  }

	  /// <summary>
	  /// Obtains an instance that uses the specified set of reference data.
	  /// </summary>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the loader </returns>
	  public static PositionCsvLoader of(ReferenceData refData)
	  {
		return new PositionCsvLoader(PositionCsvInfoResolver.of(refData));
	  }

	  /// <summary>
	  /// Obtains an instance that uses the specified resolver for additional information.
	  /// </summary>
	  /// <param name="resolver">  the resolver used to parse additional information </param>
	  /// <returns> the loader </returns>
	  public static PositionCsvLoader of(PositionCsvInfoResolver resolver)
	  {
		return new PositionCsvLoader(resolver);
	  }

	  // restricted constructor
	  private PositionCsvLoader(PositionCsvInfoResolver resolver)
	  {
		this.resolver = ArgChecker.notNull(resolver, "resolver");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Loads one or more CSV format position files.
	  /// <para>
	  /// CSV files sometimes contain a Unicode Byte Order Mark.
	  /// This method uses <seealso cref="UnicodeBom"/> to interpret it.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resources">  the CSV resources </param>
	  /// <returns> the loaded positions, position-level errors are captured in the result </returns>
	  public ValueWithFailures<IList<Position>> load(params ResourceLocator[] resources)
	  {
		return load(Arrays.asList(resources));
	  }

	  /// <summary>
	  /// Loads one or more CSV format position files.
	  /// <para>
	  /// CSV files sometimes contain a Unicode Byte Order Mark.
	  /// This method uses <seealso cref="UnicodeBom"/> to interpret it.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resources">  the CSV resources </param>
	  /// <returns> the loaded positions, all errors are captured in the result </returns>
	  public ValueWithFailures<IList<Position>> load(ICollection<ResourceLocator> resources)
	  {
		ICollection<CharSource> charSources = resources.Select(r => UnicodeBom.toCharSource(r.ByteSource)).ToList();
		return parse(charSources);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks whether the source is a CSV format position file.
	  /// <para>
	  /// This parses the headers as CSV and checks that mandatory headers are present.
	  /// This is determined entirely from the 'Strata Position Type' column.
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
			  return csv.containsHeader(TYPE_FIELD);
				}
		}
		catch (Exception)
		{
		  return false;
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses one or more CSV format position files, returning ETD futures and
	  /// options using information from reference data.
	  /// <para>
	  /// When an ETD row is found, reference data is used to find the correct security.
	  /// This uses <seealso cref="EtdContractSpec"/> by default, although this can be overridden in the resolver.
	  /// Futures and options will be returned as <seealso cref="EtdFuturePosition"/> and <seealso cref="EtdOptionPosition"/>.
	  /// </para>
	  /// <para>
	  /// CSV files sometimes contain a Unicode Byte Order Mark.
	  /// Callers are responsible for handling this, such as by using <seealso cref="UnicodeBom"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="charSources">  the CSV character sources </param>
	  /// <returns> the loaded positions, all errors are captured in the result </returns>
	  public ValueWithFailures<IList<Position>> parse(ICollection<CharSource> charSources)
	  {
		return parse(charSources, typeof(Position));
	  }

	  /// <summary>
	  /// Parses one or more CSV format position files, returning ETD futures and
	  /// options by identifier without using reference data.
	  /// <para>
	  /// When an ETD row is found, <seealso cref="EtdIdUtils"/> is used to create an identifier.
	  /// The identifier is used to create a <seealso cref="SecurityPosition"/>, with no call to reference data.
	  /// </para>
	  /// <para>
	  /// CSV files sometimes contain a Unicode Byte Order Mark.
	  /// Callers are responsible for handling this, such as by using <seealso cref="UnicodeBom"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="charSources">  the CSV character sources </param>
	  /// <returns> the loaded positions, all errors are captured in the result </returns>
	  public ValueWithFailures<IList<SecurityPosition>> parseLightweight(ICollection<CharSource> charSources)
	  {
		return parse(charSources, typeof(SecurityPosition));
	  }

	  /// <summary>
	  /// Parses one or more CSV format position files.
	  /// <para>
	  /// A type is specified to filter the positions.
	  /// If the type is <seealso cref="SecurityPosition"/>, then ETD parsing will proceed as per <seealso cref="#parseLightweight(Collection)"/>.
	  /// Otherwise, ETD parsing will proceed as per <seealso cref="#parse(Collection)"/>.
	  /// </para>
	  /// <para>
	  /// CSV files sometimes contain a Unicode Byte Order Mark.
	  /// Callers are responsible for handling this, such as by using <seealso cref="UnicodeBom"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the position type </param>
	  /// <param name="charSources">  the CSV character sources </param>
	  /// <param name="positionType">  the position type to return </param>
	  /// <returns> the loaded positions, all errors are captured in the result </returns>
	  public ValueWithFailures<IList<T>> parse<T>(ICollection<CharSource> charSources, Type<T> positionType) where T : com.opengamma.strata.product.Position
	  {
		try
		{
		  ValueWithFailures<IList<T>> result = ValueWithFailures.of(ImmutableList.of());
		  foreach (CharSource charSource in charSources)
		  {
			ValueWithFailures<IList<T>> singleResult = parseFile(charSource, positionType);
			result = result.combinedWith(singleResult, Guavate.concatToList);
		  }
		  return result;

		}
		catch (Exception ex)
		{
		  return ValueWithFailures.of(ImmutableList.of(), FailureItem.of(FailureReason.ERROR, ex));
		}
	  }

	  // loads a single CSV file, filtering by position type
	  private ValueWithFailures<IList<T>> parseFile<T>(CharSource charSource, Type<T> positionType) where T : com.opengamma.strata.product.Position
	  {
		try
		{
				using (CsvIterator csv = CsvIterator.of(charSource, true))
				{
			  if (!csv.headers().contains(TYPE_FIELD))
			  {
				return ValueWithFailures.of(ImmutableList.of(), FailureItem.of(FailureReason.PARSING, "CSV file does not contain '{header}' header: {}", TYPE_FIELD, charSource));
			  }
			  return parseFile(csv, positionType);
        
				}
		}
		catch (Exception ex)
		{
		  return ValueWithFailures.of(ImmutableList.of(), FailureItem.of(FailureReason.PARSING, ex, "CSV file could not be parsed: {exceptionMessage}: {}", ex.Message, charSource));
		}
	  }

	  // loads a single CSV file
	  private ValueWithFailures<IList<T>> parseFile<T>(CsvIterator csv, Type<T> posType) where T : com.opengamma.strata.product.Position
	  {
		IList<T> positions = new List<T>();
		IList<FailureItem> failures = new List<FailureItem>();
		int line = 2;
		foreach (CsvRow row in (IEnumerable<CsvRow>)() => csv)
		{
		  try
		  {
			PositionInfo info = parsePositionInfo(row);
			Optional<string> typeRawOpt = row.findValue(TYPE_FIELD);
			if (typeRawOpt.Present)
			{
			  // type specified
			  string type = typeRawOpt.get().ToUpper(Locale.ENGLISH);
			  switch (type.ToUpper(Locale.ENGLISH))
			  {
				case "SEC":
				case "SECURITY":
				  if (posType == typeof(SecurityPosition) || posType == typeof(ResolvableSecurityPosition))
				  {
					positions.Add(posType.cast(SecurityCsvLoader.parseSecurityPosition(row, info, resolver)));
				  }
				  else if (posType == typeof(GenericSecurityPosition) || posType == typeof(Position))
				  {
					Position parsed = SecurityCsvLoader.parseNonEtdPosition(row, info, resolver);
					if (posType.IsInstanceOfType(parsed))
					{
					  positions.Add(posType.cast(parsed));
					}
				  }
				  break;
				case "FUT":
				case "FUTURE":
				  if (posType == typeof(EtdPosition) || posType == typeof(EtdFuturePosition) || posType == typeof(ResolvableSecurityPosition) || posType == typeof(Position))
				  {
					positions.Add(posType.cast((Position) resolver.parseEtdFuturePosition(row, info)));
				  }
				  else if (posType == typeof(SecurityPosition))
				  {
					positions.Add(posType.cast(resolver.parseEtdFutureSecurityPosition(row, info)));
				  }
				  break;
				case "OPT":
				case "OPTION":
				  if (posType == typeof(EtdPosition) || posType == typeof(EtdOptionPosition) || posType == typeof(ResolvableSecurityPosition) || posType == typeof(Position))
				  {
					positions.Add(posType.cast(resolver.parseEtdOptionPosition(row, info)));
				  }
				  else if (posType == typeof(SecurityPosition))
				  {
					positions.Add(posType.cast(resolver.parseEtdOptionSecurityPosition(row, info)));
				  }
				  break;
				default:
				  failures.Add(FailureItem.of(FailureReason.PARSING, "CSV file position type '{positionType}' is not known at line {lineNumber}", typeRawOpt.get(), line));
				  break;
			  }
			}
			else
			{
			  // infer type
			  if (posType == typeof(SecurityPosition))
			  {
				positions.Add(posType.cast(SecurityCsvLoader.parsePositionLightweight(row, info, resolver)));
			  }
			  else
			  {
				Position position = SecurityCsvLoader.parsePosition(row, info, resolver);
				if (posType.IsInstanceOfType(position))
				{
				  positions.Add(posType.cast(position));
				}
			  }
			}
		  }
		  catch (Exception ex)
		  {
			failures.Add(FailureItem.of(FailureReason.PARSING, ex, "CSV file position could not be parsed at line {lineNumber}: {exceptionMessage}", line, ex.Message));
		  }
		  line++;
		}
		return ValueWithFailures.of(positions, failures);
	  }

	  // parse the position info
	  private PositionInfo parsePositionInfo(CsvRow row)
	  {
		PositionInfoBuilder infoBuilder = PositionInfo.builder();
		string scheme = row.findField(ID_SCHEME_FIELD).orElse(DEFAULT_POSITION_SCHEME);
		row.findValue(ID_FIELD).ifPresent(id => infoBuilder.id(StandardId.of(scheme, id)));
		resolver.parsePositionInfo(row, infoBuilder);
		return infoBuilder.build();
	  }

	}

}