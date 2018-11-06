using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableMap;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using CharSource = com.google.common.io.CharSource;
	using DoubleMath = com.google.common.math.DoubleMath;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using Messages = com.opengamma.strata.collect.Messages;
	using CsvFile = com.opengamma.strata.collect.io.CsvFile;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using UnicodeBom = com.opengamma.strata.collect.io.UnicodeBom;
	using FieldName = com.opengamma.strata.data.FieldName;
	using CurveDefinition = com.opengamma.strata.market.curve.CurveDefinition;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurveNode = com.opengamma.strata.market.curve.CurveNode;
	using CurveNodeClashAction = com.opengamma.strata.market.curve.CurveNodeClashAction;
	using CurveNodeDate = com.opengamma.strata.market.curve.CurveNodeDate;
	using CurveNodeDateOrder = com.opengamma.strata.market.curve.CurveNodeDateOrder;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using SeasonalityDefinition = com.opengamma.strata.market.curve.SeasonalityDefinition;
	using FixedIborSwapCurveNode = com.opengamma.strata.market.curve.node.FixedIborSwapCurveNode;
	using FixedInflationSwapCurveNode = com.opengamma.strata.market.curve.node.FixedInflationSwapCurveNode;
	using FixedOvernightSwapCurveNode = com.opengamma.strata.market.curve.node.FixedOvernightSwapCurveNode;
	using FraCurveNode = com.opengamma.strata.market.curve.node.FraCurveNode;
	using FxSwapCurveNode = com.opengamma.strata.market.curve.node.FxSwapCurveNode;
	using IborFixingDepositCurveNode = com.opengamma.strata.market.curve.node.IborFixingDepositCurveNode;
	using IborFutureCurveNode = com.opengamma.strata.market.curve.node.IborFutureCurveNode;
	using IborIborSwapCurveNode = com.opengamma.strata.market.curve.node.IborIborSwapCurveNode;
	using OvernightIborSwapCurveNode = com.opengamma.strata.market.curve.node.OvernightIborSwapCurveNode;
	using TermDepositCurveNode = com.opengamma.strata.market.curve.node.TermDepositCurveNode;
	using ThreeLegBasisSwapCurveNode = com.opengamma.strata.market.curve.node.ThreeLegBasisSwapCurveNode;
	using XCcyIborIborSwapCurveNode = com.opengamma.strata.market.curve.node.XCcyIborIborSwapCurveNode;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using IborFixingDepositConvention = com.opengamma.strata.product.deposit.type.IborFixingDepositConvention;
	using IborFixingDepositTemplate = com.opengamma.strata.product.deposit.type.IborFixingDepositTemplate;
	using TermDepositConvention = com.opengamma.strata.product.deposit.type.TermDepositConvention;
	using TermDepositTemplate = com.opengamma.strata.product.deposit.type.TermDepositTemplate;
	using FraConvention = com.opengamma.strata.product.fra.type.FraConvention;
	using FraTemplate = com.opengamma.strata.product.fra.type.FraTemplate;
	using FxSwapConvention = com.opengamma.strata.product.fx.type.FxSwapConvention;
	using FxSwapTemplate = com.opengamma.strata.product.fx.type.FxSwapTemplate;
	using IborFutureConvention = com.opengamma.strata.product.index.type.IborFutureConvention;
	using IborFutureTemplate = com.opengamma.strata.product.index.type.IborFutureTemplate;
	using FixedIborSwapConvention = com.opengamma.strata.product.swap.type.FixedIborSwapConvention;
	using FixedIborSwapTemplate = com.opengamma.strata.product.swap.type.FixedIborSwapTemplate;
	using FixedInflationSwapConvention = com.opengamma.strata.product.swap.type.FixedInflationSwapConvention;
	using FixedInflationSwapTemplate = com.opengamma.strata.product.swap.type.FixedInflationSwapTemplate;
	using FixedOvernightSwapConvention = com.opengamma.strata.product.swap.type.FixedOvernightSwapConvention;
	using FixedOvernightSwapTemplate = com.opengamma.strata.product.swap.type.FixedOvernightSwapTemplate;
	using IborIborSwapConvention = com.opengamma.strata.product.swap.type.IborIborSwapConvention;
	using IborIborSwapTemplate = com.opengamma.strata.product.swap.type.IborIborSwapTemplate;
	using OvernightIborSwapConvention = com.opengamma.strata.product.swap.type.OvernightIborSwapConvention;
	using OvernightIborSwapTemplate = com.opengamma.strata.product.swap.type.OvernightIborSwapTemplate;
	using ThreeLegBasisSwapConvention = com.opengamma.strata.product.swap.type.ThreeLegBasisSwapConvention;
	using ThreeLegBasisSwapTemplate = com.opengamma.strata.product.swap.type.ThreeLegBasisSwapTemplate;
	using XCcyIborIborSwapConvention = com.opengamma.strata.product.swap.type.XCcyIborIborSwapConvention;
	using XCcyIborIborSwapTemplate = com.opengamma.strata.product.swap.type.XCcyIborIborSwapTemplate;

	/// <summary>
	/// Loads a set of definitions to calibrate rates curves by reading from CSV resources.
	/// <para>
	/// There are three type of CSV files.
	/// </para>
	/// <para>
	/// The first file is the curve group metadata file.
	/// This file has the following header row:<br />
	/// {@code Group Name, Curve Type, Reference, Curve Name}.
	/// <ul>
	/// <li>The 'Group Name' column is the name of the group of curves.
	/// <li>The 'Curve Type' column is the type of the curve, "forward" or "discount".
	/// <li>The 'Reference' column is the reference the curve is used for, such as "USD" or "USD-LIBOR-3M".
	/// <li>The 'Curve Name' column is the name of the curve.
	/// </ul>
	/// </para>
	/// <para>
	/// The second file is the curve settings metadata file.
	/// This file has the following header row:<br />
	/// {@code Curve Name, Value Type, Day Count, Interpolator, Left Extrapolator, Right Extrapolator}.
	/// <ul>
	/// <li>The 'Curve Name' column is the name of the curve.
	/// <li>The 'Value Type' column is the type of data in the curve, "zero" for zero rates, or "df" for discount factors.
	/// <li>The 'Day Count' column is the name of the day count, such as "Act/365F".
	/// <li>The 'Interpolator' and extrapolator columns define the interpolator to use.
	/// </ul>
	/// </para>
	/// <para>
	/// The third file is the curve calibration nodes file.
	/// This file has the following header row:<br />
	/// {@code Curve Name,Label,Symbology,Ticker,Field Name,Type,Convention,Time,Date,Min Gap,Clash Action,Spread}.
	/// <ul>
	/// <li>The 'Curve Name' column is the name of the curve.
	/// <li>The 'Label' column is the label used to refer to the node.
	/// <li>The 'Symbology' column is the symbology scheme applicable to the ticker used for the market price.
	/// <li>The 'Ticker' column is the identifier within the symbology used for the market price.
	/// <li>The 'Field Name' column is the field name used for the market price, defaulted to "MarketValue", allowing
	///  fields such as 'Bid' or 'Ask' to be specified.
	/// <li>The 'Type' column is the type of the instrument, such as "FRA" or "OIS".
	/// <li>The 'Convention' column is the name of the convention to use.
	/// <li>The 'Time' column is the description of the time, such as "1Y" for a 1 year swap, or "3Mx6M" for a FRA.
	/// <li>The optional 'Date' column is the date to use for the node, defaults to "End", but can be
	///  set to "LastFixing" or a yyyy-MM-dd date.
	/// <li>The optional 'Min Gap' column is the minimum gap between this node and the adjacent nodes.
	/// <li>The optional 'Clash Action' column is the action to perform if the nodes are closer than the minimum gap
	///  or in the wrong order, defaults to "Exception", but can be set to "DropThis" or "DropOther".
	/// <li>The optional 'Spread' column is the spread to add to the instrument.
	/// </ul>
	/// </para>
	/// <para>
	/// Each curve must be contained entirely within a single file, but each file may contain more than
	/// one curve. The curve points do not need to be ordered.
	/// </para>
	/// <para>
	/// CSV files sometimes contain a Unicode Byte Order Mark.
	/// Callers are responsible for handling this, such as by using <seealso cref="UnicodeBom"/>.
	/// </para>
	/// </summary>
	public sealed class RatesCalibrationCsvLoader
	{

	  // CSV column headers
	  private const string CURVE_NAME = "Curve Name";
	  private const string CURVE_LABEL = "Label";
	  private const string CURVE_SYMBOLOGY_QUOTE = "Symbology";
	  private const string CURVE_TICKER_QUOTE = "Ticker";
	  private const string CURVE_FIELD_QUOTE = "Field Name";
	  private const string CURVE_TYPE = "Type";
	  private const string CURVE_CONVENTION = "Convention";
	  private const string CURVE_TIME = "Time";
	  private const string CURVE_DATE = "Date";
	  private const string CURVE_SPREAD = "Spread";
	  private const string CURVE_MIN_GAP = "Min Gap";
	  private const string CURVE_CLASH_ACTION = "Clash Action";

	  // Regex to parse FRA time string
	  private static readonly Pattern FRA_TIME_REGEX = Pattern.compile("P?([0-9]+)M? ?X ?P?([0-9]+)M?");
	  // Regex to parse future time string
	  private static readonly Pattern FUT_TIME_REGEX = Pattern.compile("P?((?:[0-9]+D)?(?:[0-9]+W)?(?:[0-9]+M)?) ?[+] ?([0-9]+)");
	  // Regex to parse future month string
	  private static readonly Pattern FUT_MONTH_REGEX = Pattern.compile("([A-Z][A-Z][A-Z][0-9][0-9])");
	  // Regex to parse simple time string with years, months and days
	  private static readonly Pattern SIMPLE_YMD_TIME_REGEX = Pattern.compile("P?(([0-9]+Y)?([0-9]+M)?([0-9]+W)?([0-9]+D)?)");
	  // Regex to parse simple time string with years and months
	  private static readonly Pattern SIMPLE_YM_TIME_REGEX = Pattern.compile("P?(([0-9]+Y)?([0-9]+M)?)");
	  // Regex to parse simple time string with days
	  private static readonly Pattern SIMPLE_DAYS_REGEX = Pattern.compile("P?([0-9]+D)?");
	  // parse year-month
	  private static readonly DateTimeFormatter YM_FORMATTER = new DateTimeFormatterBuilder().parseCaseInsensitive().appendPattern("MMMuu").toFormatter(Locale.ENGLISH);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Loads one or more CSV format curve calibration files.
	  /// <para>
	  /// If the files contain a duplicate entry an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="groupsResource">  the curve groups CSV resource </param>
	  /// <param name="settingsResource">  the curve settings CSV resource </param>
	  /// <param name="curveNodeResources">  the CSV resources for curve nodes </param>
	  /// <returns> the group definitions, mapped by name </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableMap<CurveGroupName, RatesCurveGroupDefinition> load(ResourceLocator groupsResource, ResourceLocator settingsResource, params ResourceLocator[] curveNodeResources)
	  {

		return load(groupsResource, settingsResource, ImmutableList.copyOf(curveNodeResources));
	  }

	  /// <summary>
	  /// Loads one or more CSV format curve calibration files.
	  /// <para>
	  /// If the files contain a duplicate entry an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="groupsResource">  the curve groups CSV resource </param>
	  /// <param name="settingsResource">  the curve settings CSV resource </param>
	  /// <param name="curveNodeResources">  the CSV resources for curve nodes </param>
	  /// <returns> the group definitions, mapped by name </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableMap<CurveGroupName, RatesCurveGroupDefinition> load(ResourceLocator groupsResource, ResourceLocator settingsResource, ICollection<ResourceLocator> curveNodeResources)
	  {

		ICollection<CharSource> curveNodeCharSources = curveNodeResources.Select(r => r.CharSource).ToList();
		return parse(groupsResource.CharSource, settingsResource.CharSource, curveNodeCharSources);
	  }

	  /// <summary>
	  /// Loads one or more CSV format curve calibration files with seasonality.
	  /// <para>
	  /// If the files contain a duplicate entry an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="groupsResource">  the curve groups CSV resource </param>
	  /// <param name="settingsResource">  the curve settings CSV resource </param>
	  /// <param name="seasonalityResource">  the curve seasonality CSV resource </param>
	  /// <param name="curveNodeResources">  the CSV resources for curve nodes </param>
	  /// <returns> the group definitions, mapped by name </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableMap<CurveGroupName, RatesCurveGroupDefinition> loadWithSeasonality(ResourceLocator groupsResource, ResourceLocator settingsResource, ResourceLocator seasonalityResource, ICollection<ResourceLocator> curveNodeResources)
	  {

		ICollection<CharSource> curveNodeCharSources = curveNodeResources.Select(r => r.CharSource).ToList();
		return parseWithSeasonality(groupsResource.CharSource, settingsResource.CharSource, seasonalityResource.CharSource, curveNodeCharSources);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses one or more CSV format curve calibration files.
	  /// <para>
	  /// If the files contain a duplicate entry an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="groupsCharSource">  the curve groups CSV character source </param>
	  /// <param name="settingsCharSource">  the curve settings CSV character source </param>
	  /// <param name="curveNodeCharSources">  the CSV character sources for curve nodes </param>
	  /// <returns> the group definitions, mapped by name </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableMap<CurveGroupName, RatesCurveGroupDefinition> parse(CharSource groupsCharSource, CharSource settingsCharSource, ICollection<CharSource> curveNodeCharSources)
	  {

		return parse0(groupsCharSource, settingsCharSource, ImmutableMap.of(), curveNodeCharSources);
	  }

	  /// <summary>
	  /// Parses one or more CSV format curve calibration files with seasonality.
	  /// <para>
	  /// If the files contain a duplicate entry an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="groupsCharSource">  the curve groups CSV character source </param>
	  /// <param name="settingsCharSource">  the curve settings CSV character source </param>
	  /// <param name="seasonalityResource">  the seasonality CSV character source </param>
	  /// <param name="curveNodeCharSources">  the CSV character sources for curve nodes </param>
	  /// <returns> the group definitions, mapped by name </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableMap<CurveGroupName, RatesCurveGroupDefinition> parseWithSeasonality(CharSource groupsCharSource, CharSource settingsCharSource, CharSource seasonalityResource, ICollection<CharSource> curveNodeCharSources)
	  {

		IDictionary<CurveName, SeasonalityDefinition> seasonality = SeasonalityDefinitionCsvLoader.parseSeasonalityDefinitions(seasonalityResource);
		return parse0(groupsCharSource, settingsCharSource, seasonality, curveNodeCharSources);
	  }

	  // parse based on pre-parsed seasonality
	  private static ImmutableMap<CurveGroupName, RatesCurveGroupDefinition> parse0(CharSource groupsCharSource, CharSource settingsCharSource, IDictionary<CurveName, SeasonalityDefinition> seasonality, ICollection<CharSource> curveNodeCharSources)
	  {

		// load curve groups and settings
		IList<RatesCurveGroupDefinition> curveGroups = RatesCurveGroupDefinitionCsvLoader.parseCurveGroupDefinitions(groupsCharSource);
		IDictionary<CurveName, LoadedCurveSettings> settingsMap = RatesCurvesCsvLoader.parseCurveSettings(settingsCharSource);

		// load curve definitions
		IList<CurveDefinition> curveDefinitions = curveNodeCharSources.stream().flatMap(res => parseSingle(res, settingsMap).stream()).collect(toImmutableList());

		// Add the curve definitions to the curve group definitions
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return curveGroups.Select(groupDefinition => groupDefinition.withCurveDefinitions(curveDefinitions).withSeasonalityDefinitions(seasonality)).collect(toImmutableMap(groupDefinition => groupDefinition.Name));
	  }

	  //-------------------------------------------------------------------------
	  // loads a single curves CSV file
	  // requestedDate can be null, meaning load all dates
	  private static IList<CurveDefinition> parseSingle(CharSource resource, IDictionary<CurveName, LoadedCurveSettings> settingsMap)
	  {

		CsvFile csv = CsvFile.of(resource, true);
		IDictionary<CurveName, IList<CurveNode>> allNodes = new Dictionary<CurveName, IList<CurveNode>>();
		foreach (CsvRow row in csv.rows())
		{
		  string curveNameStr = row.getField(CURVE_NAME);
		  string label = row.getField(CURVE_LABEL);
		  string symbologyQuoteStr = row.getField(CURVE_SYMBOLOGY_QUOTE);
		  string tickerQuoteStr = row.getField(CURVE_TICKER_QUOTE);
		  string fieldQuoteStr = row.getField(CURVE_FIELD_QUOTE);
		  string typeStr = row.getField(CURVE_TYPE);
		  string conventionStr = row.getField(CURVE_CONVENTION);
		  string timeStr = row.getField(CURVE_TIME);
		  string dateStr = row.findField(CURVE_DATE).orElse("");
		  string minGapStr = row.findField(CURVE_MIN_GAP).orElse("");
		  string clashActionStr = row.findField(CURVE_CLASH_ACTION).orElse("");
		  string spreadStr = row.findField(CURVE_SPREAD).orElse("");

		  CurveName curveName = CurveName.of(curveNameStr);
		  StandardId quoteStandardId = StandardId.of(symbologyQuoteStr, tickerQuoteStr);
		  FieldName quoteField = fieldQuoteStr.Length == 0 ? FieldName.MARKET_VALUE : FieldName.of(fieldQuoteStr);
		  QuoteId quoteId = QuoteId.of(quoteStandardId, quoteField);
		  double spread = spreadStr.Length == 0 ? 0d : double.Parse(spreadStr);
		  CurveNodeDate date = parseDate(dateStr);
		  CurveNodeDateOrder order = parseDateOrder(minGapStr, clashActionStr);

		  IList<CurveNode> curveNodes = allNodes.computeIfAbsent(curveName, k => new List<CurveNode>());
		  curveNodes.Add(createCurveNode(typeStr, conventionStr, timeStr, label, quoteId, spread, date, order));
		}
		return buildCurveDefinition(settingsMap, allNodes);
	  }

	  // parse date order
	  private static CurveNodeDate parseDate(string dateStr)
	  {
		if (dateStr.Length == 0)
		{
		  return CurveNodeDate.END;
		}
		if (dateStr.Length == 10 && dateStr[4] == '-' && dateStr[7] == '-')
		{
		  return CurveNodeDate.of(LoaderUtils.parseDate(dateStr));
		}
		string dateUpper = dateStr.ToUpper(Locale.ENGLISH);
		if (dateUpper.Equals("END"))
		{
		  return CurveNodeDate.END;
		}
		if (dateUpper.Equals("LASTFIXING"))
		{
		  return CurveNodeDate.LAST_FIXING;
		}
		throw new System.ArgumentException(Messages.format("Invalid format for node date, should be date in 'yyyy-MM-dd' format, 'End' or 'LastFixing': {}", dateUpper));
	  }

	  // parse date order
	  private static CurveNodeDateOrder parseDateOrder(string minGapStr, string clashActionStr)
	  {
		CurveNodeClashAction clashAction = clashActionStr.Length == 0 ? CurveNodeClashAction.EXCEPTION : CurveNodeClashAction.of(clashActionStr);
		if (minGapStr.Length == 0)
		{
		  return CurveNodeDateOrder.of(1, clashAction);
		}
		Matcher matcher = SIMPLE_DAYS_REGEX.matcher(minGapStr.ToUpper(Locale.ENGLISH));
		if (!matcher.matches())
		{
		  throw new System.ArgumentException(Messages.format("Invalid days format for minimum gap, should be 2D or P2D: {}", minGapStr));
		}
		Period minGap = Period.parse("P" + matcher.group(1));
		return CurveNodeDateOrder.of(minGap.Days, clashAction);
	  }

	  // build the curves
	  private static IList<CurveDefinition> buildCurveDefinition(IDictionary<CurveName, LoadedCurveSettings> settingsMap, IDictionary<CurveName, IList<CurveNode>> allNodes)
	  {

		ImmutableList.Builder<CurveDefinition> results = ImmutableList.builder();

		foreach (KeyValuePair<CurveName, IList<CurveNode>> entry in allNodes.SetOfKeyValuePairs())
		{
		  CurveName name = entry.Key;
		  LoadedCurveSettings settings = settingsMap[name];

		  if (settings == null)
		  {
			throw new System.ArgumentException(Messages.format("Missing settings for curve: {}", name));
		  }
		  results.add(settings.createCurveDefinition(entry.Value));
		}
		return results.build();
	  }

	  //-------------------------------------------------------------------------
	  // create the curve node
	  private static CurveNode createCurveNode(string typeStr, string conventionStr, string timeStr, string label, QuoteId quoteId, double spread, CurveNodeDate date, CurveNodeDateOrder order)
	  {

		if ("DEP".Equals(typeStr, StringComparison.OrdinalIgnoreCase) || "TermDeposit".Equals(typeStr, StringComparison.OrdinalIgnoreCase))
		{
		  return curveTermDepositCurveNode(conventionStr, timeStr, label, quoteId, spread, date, order);
		}
		if ("FIX".Equals(typeStr, StringComparison.OrdinalIgnoreCase) || "IborFixingDeposit".Equals(typeStr, StringComparison.OrdinalIgnoreCase))
		{
		  return curveIborFixingDepositCurveNode(conventionStr, label, quoteId, spread, date, order);
		}
		if ("FRA".Equals(typeStr, StringComparison.OrdinalIgnoreCase))
		{
		  return curveFraCurveNode(conventionStr, timeStr, label, quoteId, spread, date, order);
		}
		if ("IFU".Equals(typeStr, StringComparison.OrdinalIgnoreCase) || "IborFuture".Equals(typeStr, StringComparison.OrdinalIgnoreCase))
		{
		  return curveIborFutureCurveNode(conventionStr, timeStr, label, quoteId, spread, date, order);
		}
		if ("OIS".Equals(typeStr, StringComparison.OrdinalIgnoreCase) || "FixedOvernightSwap".Equals(typeStr, StringComparison.OrdinalIgnoreCase))
		{
		  return curveFixedOvernightCurveNode(conventionStr, timeStr, label, quoteId, spread, date, order);
		}
		if ("IRS".Equals(typeStr, StringComparison.OrdinalIgnoreCase) || "FixedIborSwap".Equals(typeStr, StringComparison.OrdinalIgnoreCase))
		{
		  return curveFixedIborCurveNode(conventionStr, timeStr, label, quoteId, spread, date, order);
		}
		if ("BAS".Equals(typeStr, StringComparison.OrdinalIgnoreCase) || "IborIborSwap".Equals(typeStr, StringComparison.OrdinalIgnoreCase))
		{
		  return curveIborIborCurveNode(conventionStr, timeStr, label, quoteId, spread, date, order);
		}
		if ("BS3".Equals(typeStr, StringComparison.OrdinalIgnoreCase) || "ThreeLegBasisSwap".Equals(typeStr, StringComparison.OrdinalIgnoreCase))
		{
		  return curveThreeLegBasisCurveNode(conventionStr, timeStr, label, quoteId, spread, date, order);
		}
		if ("ONI".Equals(typeStr, StringComparison.OrdinalIgnoreCase) || "OvernightIborBasisSwap".Equals(typeStr, StringComparison.OrdinalIgnoreCase))
		{
		  return curveOvernightIborCurveNode(conventionStr, timeStr, label, quoteId, spread, date, order);
		}
		if ("XCS".Equals(typeStr, StringComparison.OrdinalIgnoreCase) || "XCcyIborIborSwap".Equals(typeStr, StringComparison.OrdinalIgnoreCase))
		{
		  return curveXCcyIborIborCurveNode(conventionStr, timeStr, label, quoteId, spread, date, order);
		}
		if ("FXS".Equals(typeStr, StringComparison.OrdinalIgnoreCase) || "FxSwap".Equals(typeStr, StringComparison.OrdinalIgnoreCase))
		{
		  return curveFxSwapCurveNode(conventionStr, timeStr, label, quoteId, spread, date, order);
		}
		if ("INF".Equals(typeStr, StringComparison.OrdinalIgnoreCase) || "FixedInflationSwap".Equals(typeStr, StringComparison.OrdinalIgnoreCase))
		{
		  return curveFixedInflationCurveNode(conventionStr, timeStr, label, quoteId, spread, date, order);
		}
		throw new System.ArgumentException(Messages.format("Invalid curve node type: {}", typeStr));
	  }

	  private static CurveNode curveTermDepositCurveNode(string conventionStr, string timeStr, string label, QuoteId quoteId, double spread, CurveNodeDate date, CurveNodeDateOrder order)
	  {

		Matcher matcher = SIMPLE_YMD_TIME_REGEX.matcher(timeStr.ToUpper(Locale.ENGLISH));
		if (!matcher.matches())
		{
		  throw new System.ArgumentException(Messages.format("Invalid time format for Term Deposit: {}", timeStr));
		}
		Period periodToEnd = Period.parse("P" + matcher.group(1));
		TermDepositConvention convention = TermDepositConvention.of(conventionStr);
		TermDepositTemplate template = TermDepositTemplate.of(periodToEnd, convention);
		return TermDepositCurveNode.builder().template(template).rateId(quoteId).additionalSpread(spread).label(label).date(date).dateOrder(order).build();
	  }

	  private static CurveNode curveIborFixingDepositCurveNode(string conventionStr, string label, QuoteId quoteId, double spread, CurveNodeDate date, CurveNodeDateOrder order)
	  {

		IborFixingDepositConvention convention = IborFixingDepositConvention.of(conventionStr);
		IborFixingDepositTemplate template = IborFixingDepositTemplate.of(convention.Index.Tenor.Period, convention);
		return IborFixingDepositCurveNode.builder().template(template).rateId(quoteId).additionalSpread(spread).label(label).date(date).dateOrder(order).build();
	  }

	  private static CurveNode curveFraCurveNode(string conventionStr, string timeStr, string label, QuoteId quoteId, double spread, CurveNodeDate date, CurveNodeDateOrder order)
	  {

		Matcher matcher = FRA_TIME_REGEX.matcher(timeStr.ToUpper(Locale.ENGLISH));
		if (!matcher.matches())
		{
		  throw new System.ArgumentException(Messages.format("Invalid time format for FRA: {}", timeStr));
		}
		Period periodToStart = Period.parse("P" + matcher.group(1) + "M");
		Period periodToEnd = Period.parse("P" + matcher.group(2) + "M");

		FraConvention convention = FraConvention.of(conventionStr);
		FraTemplate template = FraTemplate.of(periodToStart, periodToEnd, convention);
		return FraCurveNode.builder().template(template).rateId(quoteId).additionalSpread(spread).label(label).date(date).dateOrder(order).build();
	  }

	  private static CurveNode curveIborFutureCurveNode(string conventionStr, string timeStr, string label, QuoteId quoteId, double spread, CurveNodeDate date, CurveNodeDateOrder order)
	  {

		Matcher matcher = FUT_TIME_REGEX.matcher(timeStr.ToUpper(Locale.ENGLISH));
		if (matcher.matches())
		{
		  Period periodToStart = Period.parse("P" + matcher.group(1));
		  int sequenceNumber = int.Parse(matcher.group(2));
		  IborFutureConvention convention = IborFutureConvention.of(conventionStr);
		  IborFutureTemplate template = IborFutureTemplate.of(periodToStart, sequenceNumber, convention);
		  return IborFutureCurveNode.builder().template(template).rateId(quoteId).additionalSpread(spread).label(label).date(date).dateOrder(order).build();
		}
		Matcher matcher2 = FUT_MONTH_REGEX.matcher(timeStr.ToUpper(Locale.ENGLISH));
		if (matcher2.matches())
		{
		  YearMonth yearMonth = YearMonth.parse(matcher2.group(1), YM_FORMATTER);
		  IborFutureConvention convention = IborFutureConvention.of(conventionStr);
		  IborFutureTemplate template = IborFutureTemplate.of(yearMonth, convention);
		  return IborFutureCurveNode.builder().template(template).rateId(quoteId).additionalSpread(spread).label(label).date(date).dateOrder(order).build();
		}
		throw new System.ArgumentException(Messages.format("Invalid time format for Ibor Future: {}", timeStr));
	  }

	  //-------------------------------------------------------------------------
	  private static CurveNode curveFixedOvernightCurveNode(string conventionStr, string timeStr, string label, QuoteId quoteId, double spread, CurveNodeDate date, CurveNodeDateOrder order)
	  {

		Matcher matcher = SIMPLE_YMD_TIME_REGEX.matcher(timeStr.ToUpper(Locale.ENGLISH));
		if (!matcher.matches())
		{
		  throw new System.ArgumentException(Messages.format("Invalid time format for Fixed-Overnight swap: {}", timeStr));
		}
		Period periodToEnd = Period.parse("P" + matcher.group(1));
		FixedOvernightSwapConvention convention = FixedOvernightSwapConvention.of(conventionStr);
		FixedOvernightSwapTemplate template = FixedOvernightSwapTemplate.of(Tenor.of(periodToEnd), convention);
		return FixedOvernightSwapCurveNode.builder().template(template).rateId(quoteId).additionalSpread(spread).label(label).date(date).dateOrder(order).build();
	  }

	  private static CurveNode curveFixedIborCurveNode(string conventionStr, string timeStr, string label, QuoteId quoteId, double spread, CurveNodeDate date, CurveNodeDateOrder order)
	  {

		Matcher matcher = SIMPLE_YMD_TIME_REGEX.matcher(timeStr.ToUpper(Locale.ENGLISH));
		if (!matcher.matches())
		{
		  throw new System.ArgumentException(Messages.format("Invalid time format for Fixed-Ibor swap: {}", timeStr));
		}
		Period periodToEnd = Period.parse("P" + matcher.group(1));
		FixedIborSwapConvention convention = FixedIborSwapConvention.of(conventionStr);
		FixedIborSwapTemplate template = FixedIborSwapTemplate.of(Tenor.of(periodToEnd), convention);
		return FixedIborSwapCurveNode.builder().template(template).rateId(quoteId).additionalSpread(spread).label(label).date(date).dateOrder(order).build();
	  }

	  private static CurveNode curveIborIborCurveNode(string conventionStr, string timeStr, string label, QuoteId quoteId, double spread, CurveNodeDate date, CurveNodeDateOrder order)
	  {

		Matcher matcher = SIMPLE_YM_TIME_REGEX.matcher(timeStr.ToUpper(Locale.ENGLISH));
		if (!matcher.matches())
		{
		  throw new System.ArgumentException(Messages.format("Invalid time format for Ibor-Ibor swap: {}", timeStr));
		}
		Period periodToEnd = Period.parse("P" + matcher.group(1));
		IborIborSwapConvention convention = IborIborSwapConvention.of(conventionStr);
		IborIborSwapTemplate template = IborIborSwapTemplate.of(Tenor.of(periodToEnd), convention);
		return IborIborSwapCurveNode.builder().template(template).rateId(quoteId).additionalSpread(spread).label(label).date(date).dateOrder(order).build();
	  }

	  private static CurveNode curveThreeLegBasisCurveNode(string conventionStr, string timeStr, string label, QuoteId quoteId, double spread, CurveNodeDate date, CurveNodeDateOrder order)
	  {

		Matcher matcher = SIMPLE_YM_TIME_REGEX.matcher(timeStr.ToUpper(Locale.ENGLISH));
		if (!matcher.matches())
		{
		  throw new System.ArgumentException(Messages.format("Invalid time format for Three legs basis swap: {}", timeStr));
		}
		Period periodToEnd = Period.parse("P" + matcher.group(1));
		ThreeLegBasisSwapConvention convention = ThreeLegBasisSwapConvention.of(conventionStr);
		ThreeLegBasisSwapTemplate template = ThreeLegBasisSwapTemplate.of(Tenor.of(periodToEnd), convention);
		return ThreeLegBasisSwapCurveNode.builder().template(template).rateId(quoteId).additionalSpread(spread).label(label).date(date).dateOrder(order).build();
	  }

	  private static CurveNode curveXCcyIborIborCurveNode(string conventionStr, string timeStr, string label, QuoteId quoteId, double spread, CurveNodeDate date, CurveNodeDateOrder order)
	  {

		Matcher matcher = SIMPLE_YM_TIME_REGEX.matcher(timeStr.ToUpper(Locale.ENGLISH));
		if (!matcher.matches())
		{
		  throw new System.ArgumentException(Messages.format("Invalid time format for Cross Currency Swap: {}", timeStr));
		}
		Period periodToEnd = Period.parse("P" + matcher.group(1));
		XCcyIborIborSwapConvention convention = XCcyIborIborSwapConvention.of(conventionStr);
		XCcyIborIborSwapTemplate template = XCcyIborIborSwapTemplate.of(Tenor.of(periodToEnd), convention);
		return XCcyIborIborSwapCurveNode.builder().template(template).spreadId(quoteId).additionalSpread(spread).label(label).date(date).dateOrder(order).build();
	  }

	  private static CurveNode curveOvernightIborCurveNode(string conventionStr, string timeStr, string label, QuoteId quoteId, double spread, CurveNodeDate date, CurveNodeDateOrder order)
	  {

		Matcher matcher = SIMPLE_YMD_TIME_REGEX.matcher(timeStr.ToUpper(Locale.ENGLISH));
		if (!matcher.matches())
		{
		  throw new System.ArgumentException(Messages.format("Invalid time format for Overnight-Ibor swap: {}", timeStr));
		}
		Period periodToEnd = Period.parse("P" + matcher.group(1));
		OvernightIborSwapConvention convention = OvernightIborSwapConvention.of(conventionStr);
		OvernightIborSwapTemplate template = OvernightIborSwapTemplate.of(Tenor.of(periodToEnd), convention);
		return OvernightIborSwapCurveNode.builder().template(template).rateId(quoteId).additionalSpread(spread).label(label).date(date).dateOrder(order).build();
	  }

	  private static CurveNode curveFxSwapCurveNode(string conventionStr, string timeStr, string label, QuoteId quoteId, double spread, CurveNodeDate date, CurveNodeDateOrder order)
	  {

		if (!DoubleMath.fuzzyEquals(spread, 0d, 1e-10d))
		{
		  throw new System.ArgumentException("Additional spread must be zero for FX swaps");
		}
		Matcher matcher = SIMPLE_YMD_TIME_REGEX.matcher(timeStr.ToUpper(Locale.ENGLISH));
		if (!matcher.matches())
		{
		  throw new System.ArgumentException(Messages.format("Invalid time format for FX swap: {}", timeStr));
		}
		Period periodToEnd = Period.parse("P" + matcher.group(1));
		FxSwapConvention convention = FxSwapConvention.of(conventionStr);
		FxSwapTemplate template = FxSwapTemplate.of(periodToEnd, convention);
		return FxSwapCurveNode.builder().template(template).farForwardPointsId(quoteId).label(label).date(date).dateOrder(order).build();
	  }

	  private static CurveNode curveFixedInflationCurveNode(string conventionStr, string timeStr, string label, QuoteId quoteId, double spread, CurveNodeDate date, CurveNodeDateOrder order)
	  {

		Matcher matcher = SIMPLE_YM_TIME_REGEX.matcher(timeStr.ToUpper(Locale.ENGLISH));
		if (!matcher.matches())
		{
		  throw new System.ArgumentException(Messages.format("Invalid time format for Fixed-Inflation swap: {}", timeStr));
		}
		Period periodToEnd = Period.parse("P" + matcher.group(1));
		FixedInflationSwapConvention convention = FixedInflationSwapConvention.of(conventionStr);
		FixedInflationSwapTemplate template = FixedInflationSwapTemplate.of(Tenor.of(periodToEnd), convention);
		return FixedInflationSwapCurveNode.builder().template(template).rateId(quoteId).additionalSpread(spread).label(label).date(date).dateOrder(order).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private RatesCalibrationCsvLoader()
	  {
	  }

	}

}