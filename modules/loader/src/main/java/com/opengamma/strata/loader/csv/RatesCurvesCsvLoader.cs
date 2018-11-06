using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ONE_ONE;


	using BiMap = com.google.common.collect.BiMap;
	using ImmutableBiMap = com.google.common.collect.ImmutableBiMap;
	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableListMultimap = com.google.common.collect.ImmutableListMultimap;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableMultimap = com.google.common.collect.ImmutableMultimap;
	using ListMultimap = com.google.common.collect.ListMultimap;
	using Multimap = com.google.common.collect.Multimap;
	using CharSource = com.google.common.io.CharSource;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using Index = com.opengamma.strata.basics.index.Index;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using CsvFile = com.opengamma.strata.collect.io.CsvFile;
	using CsvOutput = com.opengamma.strata.collect.io.CsvOutput;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using UnicodeBom = com.opengamma.strata.collect.io.UnicodeBom;
	using ValueType = com.opengamma.strata.market.ValueType;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using RatesCurveGroup = com.opengamma.strata.market.curve.RatesCurveGroup;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using CurveExtrapolator = com.opengamma.strata.market.curve.interpolator.CurveExtrapolator;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using DatedParameterMetadata = com.opengamma.strata.market.param.DatedParameterMetadata;

	/// <summary>
	/// Loads a set of rates curves into memory by reading from CSV resources.
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
	/// The third file is the curve values file.
	/// This file has the following header row:<br />
	/// {@code Valuation Date, Curve Name, Date, Value, Label}.
	/// <ul>
	/// <li>The 'Valuation Date' column provides the valuation date, allowing data from different
	///  days to be stored in the same file
	/// <li>The 'Curve Name' column is the name of the curve.
	/// <li>The 'Date' column is the date associated with the node.
	/// <li>The 'Value' column is value of the curve at the date.
	/// <li>The 'Label' column is the label used to refer to the node.
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
	public sealed class RatesCurvesCsvLoader
	{

	  // CSV column headers
	  private const string SETTINGS_CURVE_NAME = "Curve Name";
	  private const string SETTINGS_VALUE_TYPE = "Value Type";
	  private const string SETTINGS_DAY_COUNT = "Day Count";
	  private const string SETTINGS_INTERPOLATOR = "Interpolator";
	  private const string SETTINGS_LEFT_EXTRAPOLATOR = "Left Extrapolator";
	  private const string SETTINGS_RIGHT_EXTRAPOLATOR = "Right Extrapolator";
	  private static readonly ImmutableList<string> HEADERS_SETTINGS = ImmutableList.of(SETTINGS_CURVE_NAME, SETTINGS_VALUE_TYPE, SETTINGS_DAY_COUNT, SETTINGS_INTERPOLATOR, SETTINGS_LEFT_EXTRAPOLATOR, SETTINGS_RIGHT_EXTRAPOLATOR);

	  private const string CURVE_DATE = "Valuation Date";
	  private const string CURVE_NAME = "Curve Name";
	  private const string CURVE_POINT_DATE = "Date";
	  private const string CURVE_POINT_VALUE = "Value";
	  private const string CURVE_POINT_LABEL = "Label";
	  private static readonly ImmutableList<string> HEADERS_NODES = ImmutableList.of(CURVE_DATE, CURVE_NAME, CURVE_POINT_DATE, CURVE_POINT_VALUE, CURVE_POINT_LABEL);

	  /// <summary>
	  /// Names used in CSV file for value types.
	  /// </summary>
	  private static readonly BiMap<string, ValueType> VALUE_TYPE_MAP = ImmutableBiMap.of("zero", ValueType.ZERO_RATE, "df", ValueType.DISCOUNT_FACTOR, "forward", ValueType.FORWARD_RATE, "priceindex", ValueType.PRICE_INDEX);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Loads one or more CSV format curve files for a specific date.
	  /// <para>
	  /// Only those quotes that match the specified date will be loaded.
	  /// </para>
	  /// <para>
	  /// If the files contain a duplicate entry an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketDataDate">  the curve date to load </param>
	  /// <param name="groupsResource">  the curve groups CSV resource </param>
	  /// <param name="settingsResource">  the curve settings CSV resource </param>
	  /// <param name="curveValueResources">  the CSV resources for curves </param>
	  /// <returns> the loaded curves, mapped by an identifying key </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableList<RatesCurveGroup> load(LocalDate marketDataDate, ResourceLocator groupsResource, ResourceLocator settingsResource, ICollection<ResourceLocator> curveValueResources)
	  {

		ICollection<CharSource> curveCharSources = curveValueResources.Select(r => r.CharSource).ToList();
		ListMultimap<LocalDate, RatesCurveGroup> map = parse(d => marketDataDate.Equals(d), groupsResource.CharSource, settingsResource.CharSource, curveCharSources);
		return ImmutableList.copyOf(map.get(marketDataDate));
	  }

	  /// <summary>
	  /// Loads one or more CSV format curve files for all available dates.
	  /// <para>
	  /// If the files contain a duplicate entry an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="groupsResource">  the curve groups CSV resource </param>
	  /// <param name="settingsResource">  the curve settings CSV resource </param>
	  /// <param name="curveValueResources">  the CSV resources for curves </param>
	  /// <returns> the loaded curves, mapped by date and identifier </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableListMultimap<LocalDate, RatesCurveGroup> loadAllDates(ResourceLocator groupsResource, ResourceLocator settingsResource, ICollection<ResourceLocator> curveValueResources)
	  {

		ICollection<CharSource> curveCharSources = curveValueResources.Select(r => r.CharSource).ToList();
		return parse(d => true, groupsResource.CharSource, settingsResource.CharSource, curveCharSources);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses one or more CSV format curve files for all available dates.
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
	  /// <param name="groupsCharSource">  the curve groups CSV character source </param>
	  /// <param name="settingsCharSource">  the curve settings CSV character source </param>
	  /// <param name="curveValueCharSources">  the CSV character sources for curves </param>
	  /// <returns> the loaded curves, mapped by date and identifier </returns>
	  /// <exception cref="IllegalArgumentException"> if the files contain a duplicate entry </exception>
	  public static ImmutableListMultimap<LocalDate, RatesCurveGroup> parse(System.Predicate<LocalDate> datePredicate, CharSource groupsCharSource, CharSource settingsCharSource, ICollection<CharSource> curveValueCharSources)
	  {

		IList<RatesCurveGroupDefinition> curveGroups = RatesCurveGroupDefinitionCsvLoader.parseCurveGroupDefinitions(groupsCharSource);
		IDictionary<LocalDate, IDictionary<CurveName, Curve>> curves = parseCurves(datePredicate, settingsCharSource, curveValueCharSources);
		ImmutableListMultimap.Builder<LocalDate, RatesCurveGroup> builder = ImmutableListMultimap.builder();

		foreach (RatesCurveGroupDefinition groupDefinition in curveGroups)
		{
		  foreach (KeyValuePair<LocalDate, IDictionary<CurveName, Curve>> entry in curves.SetOfKeyValuePairs())
		  {
			RatesCurveGroup curveGroup = RatesCurveGroup.ofCurves(groupDefinition, entry.Value.values());
			builder.put(entry.Key, curveGroup);
		  }
		}
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  // loads the curves, filtering by date
	  private static IDictionary<LocalDate, IDictionary<CurveName, Curve>> parseCurves(System.Predicate<LocalDate> datePredicate, CharSource settingsResource, ICollection<CharSource> curvesResources)
	  {

		// load curve settings
		IDictionary<CurveName, LoadedCurveSettings> settingsMap = parseCurveSettings(settingsResource);

		// load curves, ensuring curves only be seen once within a date
		IDictionary<LocalDate, IDictionary<CurveName, Curve>> resultMap = new SortedDictionary<LocalDate, IDictionary<CurveName, Curve>>();
		foreach (CharSource curvesResource in curvesResources)
		{
		  Multimap<LocalDate, Curve> fileCurvesByDate = parseSingle(datePredicate, curvesResource, settingsMap);
		  // Ensure curve names are unique, with a good error message
		  foreach (LocalDate date in fileCurvesByDate.Keys)
		  {
			ICollection<Curve> fileCurves = fileCurvesByDate.get(date);
			IDictionary<CurveName, Curve> resultCurves = resultMap.computeIfAbsent(date, d => new Dictionary<CurveName, Curve>());
			foreach (Curve fileCurve in fileCurves)
			{
			  if (resultCurves.put(fileCurve.Name, fileCurve) != null)
			  {
				throw new System.ArgumentException("Rates curve loader found multiple curves with the same name: " + fileCurve.Name);
			  }
			}
		  }
		}
		return resultMap;
	  }

	  //-------------------------------------------------------------------------
	  // loads the curve settings CSV file
	  internal static IDictionary<CurveName, LoadedCurveSettings> parseCurveSettings(CharSource settingsResource)
	  {
		ImmutableMap.Builder<CurveName, LoadedCurveSettings> builder = ImmutableMap.builder();
		CsvFile csv = CsvFile.of(settingsResource, true);
		foreach (CsvRow row in csv.rows())
		{
		  string curveNameStr = row.getField(SETTINGS_CURVE_NAME);
		  string valueTypeStr = row.getField(SETTINGS_VALUE_TYPE);
		  string dayCountStr = row.getField(SETTINGS_DAY_COUNT);
		  string interpolatorStr = row.getField(SETTINGS_INTERPOLATOR);
		  string leftExtrapolatorStr = row.getField(SETTINGS_LEFT_EXTRAPOLATOR);
		  string rightExtrapolatorStr = row.getField(SETTINGS_RIGHT_EXTRAPOLATOR);

		  if (!VALUE_TYPE_MAP.containsKey(valueTypeStr.ToLower(Locale.ENGLISH)))
		  {
			throw new System.ArgumentException(Messages.format("Unsupported {} in curve settings: {}", SETTINGS_VALUE_TYPE, valueTypeStr));
		  }

		  CurveName curveName = CurveName.of(curveNameStr);
		  ValueType yValueType = VALUE_TYPE_MAP.get(valueTypeStr.ToLower(Locale.ENGLISH));
		  CurveInterpolator interpolator = CurveInterpolator.of(interpolatorStr);
		  CurveExtrapolator leftExtrap = CurveExtrapolator.of(leftExtrapolatorStr);
		  CurveExtrapolator rightExtrap = CurveExtrapolator.of(rightExtrapolatorStr);

		  bool isPriceIndex = yValueType.Equals(ValueType.PRICE_INDEX);
		  ValueType xValueType = isPriceIndex ? ValueType.MONTHS : ValueType.YEAR_FRACTION;
		  DayCount dayCount = isPriceIndex ? ONE_ONE : LoaderUtils.parseDayCount(dayCountStr);
		  LoadedCurveSettings settings = LoadedCurveSettings.of(curveName, xValueType, yValueType, dayCount, interpolator, leftExtrap, rightExtrap);
		  builder.put(curveName, settings);
		}
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  // loads a single curves CSV file
	  // requestedDate can be null, meaning load all dates
	  private static Multimap<LocalDate, Curve> parseSingle(System.Predicate<LocalDate> datePredicate, CharSource curvesResource, IDictionary<CurveName, LoadedCurveSettings> settingsMap)
	  {

		CsvFile csv = CsvFile.of(curvesResource, true);
		IDictionary<LoadedCurveKey, IList<LoadedCurveNode>> allNodes = new Dictionary<LoadedCurveKey, IList<LoadedCurveNode>>();
		foreach (CsvRow row in csv.rows())
		{
		  string dateStr = row.getField(CURVE_DATE);
		  string curveNameStr = row.getField(CURVE_NAME);
		  string pointDateStr = row.getField(CURVE_POINT_DATE);
		  string pointValueStr = row.getField(CURVE_POINT_VALUE);
		  string pointLabel = row.getField(CURVE_POINT_LABEL);

		  LocalDate date = LoaderUtils.parseDate(dateStr);
		  if (datePredicate(date))
		  {
			LocalDate pointDate = LoaderUtils.parseDate(pointDateStr);
			double pointValue = Convert.ToDouble(pointValueStr);

			LoadedCurveKey key = LoadedCurveKey.of(date, CurveName.of(curveNameStr));
			IList<LoadedCurveNode> curveNodes = allNodes.computeIfAbsent(key, k => new List<LoadedCurveNode>());
			curveNodes.Add(LoadedCurveNode.of(pointDate, pointValue, pointLabel));
		  }
		}
		return buildCurves(settingsMap, allNodes);
	  }

	  // build the curves
	  private static Multimap<LocalDate, Curve> buildCurves(IDictionary<CurveName, LoadedCurveSettings> settingsMap, IDictionary<LoadedCurveKey, IList<LoadedCurveNode>> allNodes)
	  {

		ImmutableMultimap.Builder<LocalDate, Curve> results = ImmutableMultimap.builder();

		foreach (KeyValuePair<LoadedCurveKey, IList<LoadedCurveNode>> entry in allNodes.SetOfKeyValuePairs())
		{
		  LoadedCurveKey key = entry.Key;
		  LoadedCurveSettings settings = settingsMap[key.CurveName];

		  if (settings == null)
		  {
			throw new System.ArgumentException(Messages.format("Missing settings for curve: {}", key));
		  }
		  results.put(key.CurveDate, settings.createCurve(key.CurveDate, entry.Value));
		}
		return results.build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Writes the curve settings in a CSV format to a file.
	  /// </summary>
	  /// <param name="file">  the file </param>
	  /// <param name="group">  the curve group </param>
	  public static void writeCurveSettings(File file, RatesCurveGroup group)
	  {
		try
		{
				using (Writer writer = new StreamWriter(new FileStream(file, FileMode.Create, FileAccess.Write), Encoding.UTF8))
				{
			  writeCurveSettings(writer, group);
				}
		}
		catch (IOException ex)
		{
		  throw new UncheckedIOException(ex);
		}
	  }

	  /// <summary>
	  /// Writes the curve settings in a CSV format to an appendable.
	  /// </summary>
	  /// <param name="underlying">  the underlying appendable destination </param>
	  /// <param name="group">  the curve group </param>
	  public static void writeCurveSettings(Appendable underlying, RatesCurveGroup group)
	  {
		CsvOutput csv = CsvOutput.standard(underlying);
		// header
		csv.writeLine(HEADERS_SETTINGS);
		// rows
		IDictionary<Currency, Curve> discountingCurves = group.DiscountCurves;
		ISet<CurveName> names = new HashSet<CurveName>();
		foreach (KeyValuePair<Currency, Curve> entry in discountingCurves.SetOfKeyValuePairs())
		{
		  Curve curve = entry.Value;
		  csv.writeLine(curveSettings(curve));
		  names.Add(curve.Name);
		}
		IDictionary<Index, Curve> forwardCurves = group.ForwardCurves;
		foreach (KeyValuePair<Index, Curve> entry in forwardCurves.SetOfKeyValuePairs())
		{
		  Curve curve = entry.Value;
		  if (!names.Contains(curve.Name))
		  {
			csv.writeLine(curveSettings(curve));
			names.Add(curve.Name);
		  }
		}
	  }

	  private static IList<string> curveSettings(Curve curve)
	  {
		ArgChecker.isTrue(curve is InterpolatedNodalCurve, "Curve must be an InterpolatedNodalCurve");
		if (!VALUE_TYPE_MAP.inverse().containsKey(curve.Metadata.YValueType))
		{
		  throw new System.ArgumentException(Messages.format("Unsupported ValueType in curve settings: {}", curve.Metadata.YValueType));
		}
		InterpolatedNodalCurve interpolatedCurve = (InterpolatedNodalCurve) curve;
		IList<string> line = new List<string>();
		line.Add(curve.Name.Name);
		line.Add(VALUE_TYPE_MAP.inverse().get(curve.Metadata.YValueType));
		line.Add(curve.Metadata.getInfo(CurveInfoType.DAY_COUNT).ToString());
		line.Add(interpolatedCurve.Interpolator.ToString());
		line.Add(interpolatedCurve.ExtrapolatorLeft.ToString());
		line.Add(interpolatedCurve.ExtrapolatorRight.ToString());
		return line;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Writes the curve groups definition in a CSV format to a file.
	  /// </summary>
	  /// <param name="file">  the file </param>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <param name="group">  the curve group </param>
	  public static void writeCurveNodes(File file, LocalDate valuationDate, RatesCurveGroup group)
	  {
		try
		{
				using (Writer writer = new StreamWriter(new FileStream(file, FileMode.Create, FileAccess.Write), Encoding.UTF8))
				{
			  writeCurveNodes(writer, valuationDate, group);
				}
		}
		catch (IOException ex)
		{
		  throw new UncheckedIOException(ex);
		}
	  }

	  /// <summary>
	  /// Writes the curve nodes in a CSV format to an appendable.
	  /// </summary>
	  /// <param name="underlying">  the underlying appendable destination </param>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <param name="group">  the curve group </param>
	  public static void writeCurveNodes(Appendable underlying, LocalDate valuationDate, RatesCurveGroup group)
	  {
		CsvOutput csv = CsvOutput.standard(underlying);
		// header
		csv.writeLine(HEADERS_NODES);
		// rows
		string valuationDateStr = valuationDate.ToString();
		IDictionary<Currency, Curve> discountingCurves = group.DiscountCurves;
		ISet<CurveName> names = new HashSet<CurveName>();
		foreach (KeyValuePair<Currency, Curve> entry in discountingCurves.SetOfKeyValuePairs())
		{
		  Curve curve = entry.Value;
		  nodeLines(valuationDateStr, curve, csv);
		  names.Add(curve.Name);
		}
		IDictionary<Index, Curve> forwardCurves = group.ForwardCurves;
		foreach (KeyValuePair<Index, Curve> entry in forwardCurves.SetOfKeyValuePairs())
		{
		  Curve curve = entry.Value;
		  if (!names.Contains(curve.Name))
		  {
			nodeLines(valuationDateStr, curve, csv);
			names.Add(curve.Name);
		  }
		}
	  }

	  // add each node to the csv file
	  private static void nodeLines(string valuationDateStr, Curve curve, CsvOutput csv)
	  {
		ArgChecker.isTrue(curve is InterpolatedNodalCurve, "interpolated");
		InterpolatedNodalCurve interpolatedCurve = (InterpolatedNodalCurve) curve;
		int nbPoints = interpolatedCurve.XValues.size();
		for (int i = 0; i < nbPoints; i++)
		{
		  ArgChecker.isTrue(interpolatedCurve.getParameterMetadata(i) is DatedParameterMetadata, "Curve metadata must contain a date, but was " + interpolatedCurve.getParameterMetadata(i).GetType().Name);
		  DatedParameterMetadata metadata = (DatedParameterMetadata) interpolatedCurve.getParameterMetadata(i);
		  IList<string> line = new List<string>();
		  line.Add(valuationDateStr);
		  line.Add(curve.Name.Name.ToString());
		  line.Add(metadata.Date.ToString());
		  line.Add(decimal.valueOf(interpolatedCurve.YValues.get(i)).toPlainString());
		  line.Add(metadata.Label);
		  csv.writeLine(line);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private RatesCurvesCsvLoader()
	  {
	  }

	}

}