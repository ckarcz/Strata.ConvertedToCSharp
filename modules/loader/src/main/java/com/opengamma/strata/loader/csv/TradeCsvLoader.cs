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
	using GenericSecurityTrade = com.opengamma.strata.product.GenericSecurityTrade;
	using ResolvableSecurityTrade = com.opengamma.strata.product.ResolvableSecurityTrade;
	using SecurityQuantityTrade = com.opengamma.strata.product.SecurityQuantityTrade;
	using SecurityTrade = com.opengamma.strata.product.SecurityTrade;
	using Trade = com.opengamma.strata.product.Trade;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using TradeInfoBuilder = com.opengamma.strata.product.TradeInfoBuilder;
	using TermDepositTrade = com.opengamma.strata.product.deposit.TermDepositTrade;
	using TermDepositConventions = com.opengamma.strata.product.deposit.type.TermDepositConventions;
	using FraTrade = com.opengamma.strata.product.fra.FraTrade;
	using FraConventions = com.opengamma.strata.product.fra.type.FraConventions;
	using FxSingleTrade = com.opengamma.strata.product.fx.FxSingleTrade;
	using FxSwapTrade = com.opengamma.strata.product.fx.FxSwapTrade;
	using FxTrade = com.opengamma.strata.product.fx.FxTrade;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;
	using SingleCurrencySwapConvention = com.opengamma.strata.product.swap.type.SingleCurrencySwapConvention;

	/// <summary>
	/// Loads trades from CSV files.
	/// <para>
	/// The trades are expected to be in a CSV format known to Strata.
	/// The parser is flexible, understanding a number of different ways to define each trade.
	/// Columns may occur in any order.
	/// 
	/// <h4>Common</h4>
	/// </para>
	/// <para>
	/// The following standard columns are supported:<br />
	/// <ul>
	/// <li>The 'Strata Trade Type' column is required, and must be the instrument type,
	///   such as 'Fra' or 'Swap'
	/// <li>The 'Id Scheme' column is optional, and is the name of the scheme that the trade
	///   identifier is unique within, such as 'OG-Trade'
	/// <li>The 'Id' column is optional, and is the identifier of the trade,
	///   such as 'FRA12345'
	/// <li>The 'Counterparty Scheme' column is optional, and is the name of the scheme that the trade
	///   identifier is unique within, such as 'OG-Counterparty'
	/// <li>The 'Counterparty' column is optional, and is the identifier of the trade's counterparty,
	///   such as 'Bank-A'
	/// <li>The 'Trade Date' column is optional, and is the date that the trade occurred, such as '2017-08-01'
	/// <li>The 'Trade Time' column is optional, and is the time of day that the trade occurred,
	///   such as '11:30'
	/// <li>The 'Trade Zone' column is optional, and is the time-zone that the trade occurred,
	///   such as 'Europe/London'
	/// <li>The 'Settlement Date' column is optional, and is the date that the trade settles, such as '2017-08-01'
	/// </ul>
	/// 
	/// <h4>Fra</h4>
	/// </para>
	/// <para>
	/// The following columns are supported for 'Fra' trades:
	/// <ul>
	/// <li>'Buy Sell' - mandatory
	/// <li>'Notional' - mandatory
	/// <li>'Fixed Rate' - mandatory, percentage
	/// <li>'Convention' - see below, see <seealso cref="FraConventions"/>
	/// <li>'Period To Start' - see below
	/// <li>'Start Date' - see below
	/// <li>'End Date' - see below
	/// <li>'Index' - see below
	/// <li>'Interpolated Index' - see below
	/// <li>'Day Count' - see below
	/// <li>'Date Convention' - optional
	/// <li>'Date Calendar' - optional
	/// </ul>
	/// </para>
	/// <para>
	/// Valid combinations to define a FRA are:
	/// <ul>
	/// <li>'Convention', 'Trade Date', 'Period To Start'
	/// <li>'Convention', 'Start Date', 'End Date'
	/// <li>'Index', 'Start Date', 'End Date' plus optionally 'Interpolated Index', 'Day Count'
	/// </ul>
	/// 
	/// <h4>Swap</h4>
	/// </para>
	/// <para>
	/// The following columns are supported for 'Swap' trades:
	/// <ul>
	/// <li>'Buy Sell' - mandatory
	/// <li>'Notional' - mandatory
	/// <li>'Fixed Rate' - mandatory, percentage (treated as the spread for some swap types)
	/// <li>'Convention' - mandatory, see <seealso cref="SingleCurrencySwapConvention"/> implementations
	/// <li>'Period To Start'- see below
	/// <li>'Tenor'- see below
	/// <li>'Start Date'- see below
	/// <li>'End Date'- see below
	/// <li>'Roll Convention' - optional
	/// <li>'Stub Convention' - optional
	/// <li>'First Regular Start Date' - optional
	/// <li>'Last Regular End Date' - optional
	/// <li>'Date Convention' - optional
	/// <li>'Date Calendar' - optional
	/// </ul>
	/// </para>
	/// <para>
	/// Valid combinations to define a Swap are:
	/// <ul>
	/// <li>'Convention', 'Trade Date', 'Period To Start', 'Tenor'
	/// <li>'Convention', 'Start Date', 'End Date'
	/// <li>'Convention', 'Start Date', 'Tenor'
	/// <li>Explicitly by defining each leg (not detailed here)
	/// </ul>
	/// 
	/// <h4>Term Deposit</h4>
	/// </para>
	/// <para>
	/// The following columns are supported for 'TermDeposit' trades:
	/// <ul>
	/// <li>'Buy Sell' - mandatory
	/// <li>'Notional' - mandatory
	/// <li>'Fixed Rate' - mandatory, percentage
	/// <li>'Convention'- see below, see <seealso cref="TermDepositConventions"/> implementations
	/// <li>'Tenor'- see below
	/// <li>'Start Date'- see below
	/// <li>'End Date'- see below
	/// <li>'Currency'- see below
	/// <li>'Day Count'- see below
	/// <li>'Date Convention' - optional
	/// <li>'Date Calendar' - optional
	/// </ul>
	/// </para>
	/// <para>
	/// Valid combinations to define a Term Deposit are:
	/// <ul>
	/// <li>'Convention', 'Trade Date', 'Period To Start'
	/// <li>'Convention', 'Start Date', 'End Date'
	/// <li>'Start Date', 'End Date', 'Currency', 'Day Count'
	/// </ul>
	/// 
	/// <h4>FX Singles</h4>
	/// </para>
	/// <para>
	/// The following columns are supported for 'FX Singles' (FX Spots and FX Forwards) trades:
	/// <ul>
	/// <li>'Buy Sell' - optional, if not present notional must be signed
	/// <li>'Currency' - mandatory
	/// <li>'Notional' - mandatory
	/// <li>'FX Rate' - mandatory
	/// <li>'Payment Date - mandatory'
	/// <li>'Payment Date Convention' - optional field. See <seealso cref="com.opengamma.strata.basics.date.BusinessDayConventions"/> for possible values.
	/// <li>'Payment Date Calendar' - optional field. See <seealso cref="com.opengamma.strata.basics.date.HolidayCalendarIds"/> for possible values.
	/// </ul>
	/// 
	/// <h4>FX Swaps</h4>
	/// </para>
	/// <para>
	/// The following columns are supported for 'FxSwap' trades:
	/// <ul>
	/// <li>'Buy Sell' - optional, if not present notional must be signed
	/// <li>'Currency' - mandatory
	/// <li>'Notional' - mandatory
	/// <li>'FX Rate' - mandatory
	/// <li>Payment Date - mandatory
	/// <li>'Far FX Rate' - mandatory
	/// <li>'Far Payment Date' - mandatory
	/// <li>'Payment Date Convention' - optional field. See <seealso cref="com.opengamma.strata.basics.date.BusinessDayConventions"/> for possible values.
	/// <li>'Payment Date Calendar' - optional field. See <seealso cref="com.opengamma.strata.basics.date.HolidayCalendarIds"/> for possible values.
	/// </ul>
	/// 
	/// <h4>Security</h4>
	/// </para>
	/// <para>
	/// The following columns are supported for 'Security' trades:
	/// <ul>
	/// <li>'Security Id Scheme' - optional, defaults to 'OG-Security'
	/// <li>'Security Id' - mandatory
	/// <li>'Quantity' - see below
	/// <li>'Long Quantity' - see below
	/// <li>'Short Quantity' - see below
	/// <li>'Price' - optional
	/// </ul>
	/// </para>
	/// <para>
	/// The quantity will normally be set from the 'Quantity' column.
	/// If that column is not found, the 'Long Quantity' and 'Short Quantity' columns will be used instead.
	/// </para>
	/// </summary>
	public sealed class TradeCsvLoader
	{

	  // default schemes
	  private const string DEFAULT_TRADE_SCHEME = "OG-Trade";
	  private const string DEFAULT_CPTY_SCHEME = "OG-Counterparty";

	  // shared CSV headers
	  internal const string TRADE_DATE_FIELD = "Trade Date";
	  internal const string CONVENTION_FIELD = "Convention";
	  internal const string BUY_SELL_FIELD = "Buy Sell";
	  internal const string DIRECTION_FIELD = "Direction";
	  internal const string CURRENCY_FIELD = "Currency";
	  internal const string NOTIONAL_FIELD = "Notional";
	  internal const string INDEX_FIELD = "Index";
	  internal const string INTERPOLATED_INDEX_FIELD = "Interpolated Index";
	  internal const string FIXED_RATE_FIELD = "Fixed Rate";
	  internal const string PERIOD_TO_START_FIELD = "Period To Start";
	  internal const string TENOR_FIELD = "Tenor";
	  internal const string START_DATE_FIELD = "Start Date";
	  internal const string END_DATE_FIELD = "End Date";
	  internal const string DATE_ADJ_CNV_FIELD = "Date Convention";
	  internal const string DATE_ADJ_CAL_FIELD = "Date Calendar";
	  internal const string DAY_COUNT_FIELD = "Day Count";
	  internal const string FX_RATE_FIELD = "FX Rate";

	  // CSV column headers
	  private const string TYPE_FIELD = "Strata Trade Type";
	  private const string ID_SCHEME_FIELD = "Id Scheme";
	  private const string ID_FIELD = "Id";
	  private const string CPTY_SCHEME_FIELD = "Counterparty Scheme";
	  private const string CPTY_FIELD = "Counterparty";
	  private const string TRADE_TIME_FIELD = "Trade Time";
	  private const string TRADE_ZONE_FIELD = "Trade Zone";
	  private const string SETTLEMENT_DATE_FIELD = "Settlement Date";

	  /// <summary>
	  /// The resolver, providing additional information.
	  /// </summary>
	  private readonly TradeCsvInfoResolver resolver;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance that uses the standard set of reference data.
	  /// </summary>
	  /// <returns> the loader </returns>
	  public static TradeCsvLoader standard()
	  {
		return new TradeCsvLoader(TradeCsvInfoResolver.standard());
	  }

	  /// <summary>
	  /// Obtains an instance that uses the specified set of reference data.
	  /// </summary>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the loader </returns>
	  public static TradeCsvLoader of(ReferenceData refData)
	  {
		return new TradeCsvLoader(TradeCsvInfoResolver.of(refData));
	  }

	  /// <summary>
	  /// Obtains an instance that uses the specified resolver for additional information.
	  /// </summary>
	  /// <param name="resolver">  the resolver used to parse additional information </param>
	  /// <returns> the loader </returns>
	  public static TradeCsvLoader of(TradeCsvInfoResolver resolver)
	  {
		return new TradeCsvLoader(resolver);
	  }

	  // restricted constructor
	  private TradeCsvLoader(TradeCsvInfoResolver resolver)
	  {
		this.resolver = ArgChecker.notNull(resolver, "resolver");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Loads one or more CSV format trade files.
	  /// <para>
	  /// CSV files sometimes contain a Unicode Byte Order Mark.
	  /// This method uses <seealso cref="UnicodeBom"/> to interpret it.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resources">  the CSV resources </param>
	  /// <returns> the loaded trades, trade-level errors are captured in the result </returns>
	  public ValueWithFailures<IList<Trade>> load(params ResourceLocator[] resources)
	  {
		return load(Arrays.asList(resources));
	  }

	  /// <summary>
	  /// Loads one or more CSV format trade files.
	  /// <para>
	  /// CSV files sometimes contain a Unicode Byte Order Mark.
	  /// This method uses <seealso cref="UnicodeBom"/> to interpret it.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resources">  the CSV resources </param>
	  /// <returns> the loaded trades, all errors are captured in the result </returns>
	  public ValueWithFailures<IList<Trade>> load(ICollection<ResourceLocator> resources)
	  {
		ICollection<CharSource> charSources = resources.Select(r => UnicodeBom.toCharSource(r.ByteSource)).ToList();
		return parse(charSources);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks whether the source is a CSV format trade file.
	  /// <para>
	  /// This parses the headers as CSV and checks that mandatory headers are present.
	  /// This is determined entirely from the 'Strata Trade Type' column.
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
	  /// Parses one or more CSV format trade files.
	  /// <para>
	  /// CSV files sometimes contain a Unicode Byte Order Mark.
	  /// Callers are responsible for handling this, such as by using <seealso cref="UnicodeBom"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="charSources">  the CSV character sources </param>
	  /// <returns> the loaded trades, all errors are captured in the result </returns>
	  public ValueWithFailures<IList<Trade>> parse(ICollection<CharSource> charSources)
	  {
		return parse(charSources, typeof(Trade));
	  }

	  /// <summary>
	  /// Parses one or more CSV format trade files with an error-creating type filter.
	  /// <para>
	  /// A list of types is specified to filter the trades.
	  /// Trades that do not match the type will be included in the failure list.
	  /// </para>
	  /// <para>
	  /// CSV files sometimes contain a Unicode Byte Order Mark.
	  /// Callers are responsible for handling this, such as by using <seealso cref="UnicodeBom"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="charSources">  the CSV character sources </param>
	  /// <param name="tradeTypes">  the trade types to return </param>
	  /// <returns> the loaded trades, all errors are captured in the result </returns>
	  public ValueWithFailures<IList<Trade>> parse(ICollection<CharSource> charSources, IList<Type> tradeTypes)
	  {

		ValueWithFailures<IList<Trade>> parsed = parse(charSources, typeof(Trade));
		IList<Trade> valid = new List<Trade>();
		IList<FailureItem> failures = new List<FailureItem>(parsed.Failures);
		foreach (Trade trade in parsed.Value)
		{
		  if (tradeTypes.Contains(trade.GetType()))
		  {
			valid.Add(trade);
		  }
		  else
		  {
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
			failures.Add(FailureItem.of(FailureReason.PARSING, "Trade type not allowed {tradeType}, only these types are supported: {}", trade.GetType().FullName, tradeTypes.Select(t => t.SimpleName).collect(joining(", "))));
		  }
		}
		return ValueWithFailures.of(valid, failures);
	  }

	  /// <summary>
	  /// Parses one or more CSV format trade files with a quiet type filter.
	  /// <para>
	  /// A type is specified to filter the trades.
	  /// Trades that do not match the type are silently dropped.
	  /// </para>
	  /// <para>
	  /// CSV files sometimes contain a Unicode Byte Order Mark.
	  /// Callers are responsible for handling this, such as by using <seealso cref="UnicodeBom"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the trade type </param>
	  /// <param name="charSources">  the CSV character sources </param>
	  /// <param name="tradeType">  the trade type to return </param>
	  /// <returns> the loaded trades, all errors are captured in the result </returns>
	  public ValueWithFailures<IList<T>> parse<T>(ICollection<CharSource> charSources, Type<T> tradeType) where T : com.opengamma.strata.product.Trade
	  {
		try
		{
		  ValueWithFailures<IList<T>> result = ValueWithFailures.of(ImmutableList.of());
		  foreach (CharSource charSource in charSources)
		  {
			ValueWithFailures<IList<T>> singleResult = parseFile(charSource, tradeType);
			result = result.combinedWith(singleResult, Guavate.concatToList);
		  }
		  return result;

		}
		catch (Exception ex)
		{
		  return ValueWithFailures.of(ImmutableList.of(), FailureItem.of(FailureReason.ERROR, ex));
		}
	  }

	  // loads a single CSV file, filtering by trade type
	  private ValueWithFailures<IList<T>> parseFile<T>(CharSource charSource, Type<T> tradeType) where T : com.opengamma.strata.product.Trade
	  {
		try
		{
				using (CsvIterator csv = CsvIterator.of(charSource, true))
				{
			  if (!csv.headers().contains(TYPE_FIELD))
			  {
				return ValueWithFailures.of(ImmutableList.of(), FailureItem.of(FailureReason.PARSING, "CSV file does not contain '{header}' header: {}", TYPE_FIELD, charSource));
			  }
			  return parseFile(csv, tradeType);
        
				}
		}
		catch (Exception ex)
		{
		  return ValueWithFailures.of(ImmutableList.of(), FailureItem.of(FailureReason.PARSING, ex, "CSV file could not be parsed: {exceptionMessage}: {}", ex.Message, charSource));
		}
	  }

	  // loads a single CSV file
	  private ValueWithFailures<IList<T>> parseFile<T>(CsvIterator csv, Type<T> tradeType) where T : com.opengamma.strata.product.Trade
	  {
		IList<T> trades = new List<T>();
		IList<FailureItem> failures = new List<FailureItem>();
		while (csv.hasNext())
		{
		  CsvRow row = csv.next();
		  try
		  {
			string typeRaw = row.getField(TYPE_FIELD);
			TradeInfo info = parseTradeInfo(row);
			switch (typeRaw.ToUpper(Locale.ENGLISH))
			{
			  case "FRA":
				if (tradeType == typeof(FraTrade) || tradeType == typeof(Trade))
				{
				  trades.Add(tradeType.cast(FraTradeCsvLoader.parse(row, info, resolver)));
				}
				break;
			  case "SECURITY":
				if (tradeType == typeof(SecurityTrade) || tradeType == typeof(GenericSecurityTrade) || tradeType == typeof(ResolvableSecurityTrade) || tradeType == typeof(Trade))
				{
				  SecurityQuantityTrade parsed = SecurityCsvLoader.parseTrade(row, info, resolver);
				  if (tradeType.IsInstanceOfType(parsed))
				  {
					trades.Add(tradeType.cast(parsed));
				  }
				}
				break;
			  case "SWAP":
				if (tradeType == typeof(SwapTrade) || tradeType == typeof(Trade))
				{
				  IList<CsvRow> variableRows = new List<CsvRow>();
				  while (csv.hasNext() && csv.peek().getField(TYPE_FIELD).ToUpper(Locale.ENGLISH).Equals("VARIABLE"))
				  {
					variableRows.Add(csv.next());
				  }
				  trades.Add(tradeType.cast(SwapTradeCsvLoader.parse(row, variableRows, info, resolver)));
				}
				break;
			  case "TERMDEPOSIT":
			  case "TERM DEPOSIT":
				if (tradeType == typeof(TermDepositTrade) || tradeType == typeof(Trade))
				{
				  trades.Add(tradeType.cast(TermDepositTradeCsvLoader.parse(row, info, resolver)));
				}
				break;
			  case "VARIABLE":
				failures.Add(FailureItem.of(FailureReason.PARSING, "CSV file contained a 'Variable' type at line {lineNumber} that was not preceeded by a 'Swap'", row.lineNumber()));
				break;
			  case "FX":
			  case "FXSINGLE":
			  case "FX SINGLE":
				if (tradeType == typeof(FxSingleTrade) || tradeType == typeof(FxTrade) || tradeType == typeof(Trade))
				{
				  trades.Add(tradeType.cast(FxSingleTradeCsvLoader.parse(row, info, resolver)));
				}
				break;
			  case "FXSWAP":
			  case "FX SWAP":
				if (tradeType == typeof(FxSwapTrade) || tradeType == typeof(FxTrade) || tradeType == typeof(Trade))
				{
				  trades.Add(tradeType.cast(FxSwapTradeCsvLoader.parse(row, info, resolver)));
				}
				break;
			  default:
				failures.Add(FailureItem.of(FailureReason.PARSING, "CSV file trade type '{tradeType}' is not known at line {lineNumber}", typeRaw, row.lineNumber()));
				break;
			}
		  }
		  catch (Exception ex)
		  {
			failures.Add(FailureItem.of(FailureReason.PARSING, ex, "CSV file trade could not be parsed at line {lineNumber}: {exceptionMessage}", row.lineNumber(), ex.Message));
		  }
		}
		return ValueWithFailures.of(trades, failures);
	  }

	  // parse the trade info
	  private TradeInfo parseTradeInfo(CsvRow row)
	  {
		TradeInfoBuilder infoBuilder = TradeInfo.builder();
		string scheme = row.findField(ID_SCHEME_FIELD).orElse(DEFAULT_TRADE_SCHEME);
		row.findValue(ID_FIELD).ifPresent(id => infoBuilder.id(StandardId.of(scheme, id)));
		string schemeCpty = row.findValue(CPTY_SCHEME_FIELD).orElse(DEFAULT_CPTY_SCHEME);
		row.findValue(CPTY_FIELD).ifPresent(cpty => infoBuilder.counterparty(StandardId.of(schemeCpty, cpty)));
		row.findValue(TRADE_DATE_FIELD).ifPresent(dateStr => infoBuilder.tradeDate(LoaderUtils.parseDate(dateStr)));
		row.findValue(TRADE_TIME_FIELD).ifPresent(timeStr => infoBuilder.tradeTime(LoaderUtils.parseTime(timeStr)));
		row.findValue(TRADE_ZONE_FIELD).ifPresent(zoneStr => infoBuilder.zone(ZoneId.of(zoneStr)));
		row.findValue(SETTLEMENT_DATE_FIELD).ifPresent(dateStr => infoBuilder.settlementDate(LoaderUtils.parseDate(dateStr)));
		resolver.parseTradeInfo(row, infoBuilder);
		return infoBuilder.build();
	  }

	}

}