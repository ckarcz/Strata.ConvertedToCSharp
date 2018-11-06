using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{


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
	using MapStream = com.opengamma.strata.collect.MapStream;
	using Messages = com.opengamma.strata.collect.Messages;
	using CsvFile = com.opengamma.strata.collect.io.CsvFile;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using ValueType = com.opengamma.strata.market.ValueType;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using LegalEntityCurveGroup = com.opengamma.strata.market.curve.LegalEntityCurveGroup;
	using LegalEntityGroup = com.opengamma.strata.market.curve.LegalEntityGroup;
	using RepoGroup = com.opengamma.strata.market.curve.RepoGroup;
	using CurveExtrapolator = com.opengamma.strata.market.curve.interpolator.CurveExtrapolator;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;

	/// <summary>
	/// Loads a set of legal entity rates curves into memory by reading from CSV resources.
	/// <para>
	/// There are three type of CSV files.
	/// </para>
	/// <para>
	/// The first file is the legal entity curve group metadata file.
	/// This file has the following header row:<br />
	/// {@code Group Name, Curve Type, Reference, Currency, Curve Name}.
	/// <ul>
	/// <li>The 'Group Name' column is the name of the group of curves.
	/// <li>The 'Curve Type' column is the type of the curve, "repo" or "issuer".
	/// <li>The 'Reference' column is the reference group for which the curve is used, legal entity group or repo group.
	/// <li>The 'Currency' column is the reference currency for which the curve is used.
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
	/// <li>The 'Interpolator' column defines the interpolator to use.
	/// <li>The 'Left Extrapolator' and 'Right Extrapolator' columns define the extrapolators to use.
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
	/// The files must contain at least one repo curve and one issuer curve.
	/// </para>
	/// </summary>
	public class LegalEntityRatesCurvesCsvLoader
	{

	  // Column headers for legal entity curve group
	  private const string GROUPS_NAME = "Group Name";
	  private const string GROUPS_CURVE_TYPE = "Curve Type";
	  private const string GROUPS_REFERENCE = "Reference";
	  private const string GROUPS_CURRENCY = "Currency";
	  private const string GROUPS_CURVE_NAME = "Curve Name";

	  // Names used in the curve type column in the legal entity curve group
	  private const string REPO = "repo";
	  private const string ISSUER = "issuer";

	  // Column headers for curve setting 
	  private const string SETTINGS_CURVE_NAME = "Curve Name";
	  private const string SETTINGS_VALUE_TYPE = "Value Type";
	  private const string SETTINGS_DAY_COUNT = "Day Count";
	  private const string SETTINGS_INTERPOLATOR = "Interpolator";
	  private const string SETTINGS_LEFT_EXTRAPOLATOR = "Left Extrapolator";
	  private const string SETTINGS_RIGHT_EXTRAPOLATOR = "Right Extrapolator";

	  // Column headers for curve nodes
	  private const string CURVE_DATE = "Valuation Date";
	  private const string CURVE_NAME = "Curve Name";
	  private const string CURVE_POINT_DATE = "Date";
	  private const string CURVE_POINT_VALUE = "Value";
	  private const string CURVE_POINT_LABEL = "Label";

	  /// <summary>
	  /// Names used in CSV file for value types.
	  /// </summary>
	  private static readonly BiMap<string, ValueType> VALUE_TYPE_MAP = ImmutableBiMap.of("zero", ValueType.ZERO_RATE, "df", ValueType.DISCOUNT_FACTOR);

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
	  public static ImmutableList<LegalEntityCurveGroup> load(LocalDate marketDataDate, ResourceLocator groupsResource, ResourceLocator settingsResource, ICollection<ResourceLocator> curveValueResources)
	  {

		ICollection<CharSource> curveCharSources = curveValueResources.Select(r => r.CharSource).ToList();
		ListMultimap<LocalDate, LegalEntityCurveGroup> map = parse(d => marketDataDate.Equals(d), groupsResource.CharSource, settingsResource.CharSource, curveCharSources);
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
	  public static ImmutableListMultimap<LocalDate, LegalEntityCurveGroup> loadAllDates(ResourceLocator groupsResource, ResourceLocator settingsResource, ICollection<ResourceLocator> curveValueResources)
	  {

		ICollection<CharSource> curveCharSources = curveValueResources.Select(r => r.CharSource).ToList();
		return parse(d => true, groupsResource.CharSource, settingsResource.CharSource, curveCharSources);
	  }

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
	  public static ImmutableListMultimap<LocalDate, LegalEntityCurveGroup> parse(System.Predicate<LocalDate> datePredicate, CharSource groupsCharSource, CharSource settingsCharSource, ICollection<CharSource> curveValueCharSources)
	  {

		IDictionary<CurveGroupName, IDictionary<Pair<RepoGroup, Currency>, CurveName>> repoGroups = new LinkedHashMap<CurveGroupName, IDictionary<Pair<RepoGroup, Currency>, CurveName>>();
		IDictionary<CurveGroupName, IDictionary<Pair<LegalEntityGroup, Currency>, CurveName>> legalEntityGroups = new LinkedHashMap<CurveGroupName, IDictionary<Pair<LegalEntityGroup, Currency>, CurveName>>();
		parseCurveMaps(groupsCharSource, repoGroups, legalEntityGroups);
		IDictionary<LocalDate, IDictionary<CurveName, Curve>> allCurves = parseCurves(datePredicate, settingsCharSource, curveValueCharSources);
		ImmutableListMultimap.Builder<LocalDate, LegalEntityCurveGroup> builder = ImmutableListMultimap.builder();

		foreach (KeyValuePair<LocalDate, IDictionary<CurveName, Curve>> curveEntry in allCurves.SetOfKeyValuePairs())
		{
		  LocalDate date = curveEntry.Key;
		  IDictionary<CurveName, Curve> curves = curveEntry.Value;
		  foreach (KeyValuePair<CurveGroupName, IDictionary<Pair<RepoGroup, Currency>, CurveName>> repoEntry in repoGroups.SetOfKeyValuePairs())
		  {
			CurveGroupName groupName = repoEntry.Key;
			IDictionary<Pair<RepoGroup, Currency>, Curve> repoCurves = MapStream.of(repoEntry.Value).mapValues(name => queryCurve(name, curves, date, groupName, "Repo")).toMap();
			IDictionary<Pair<LegalEntityGroup, Currency>, Curve> issuerCurves = MapStream.of(legalEntityGroups[groupName]).mapValues(name => queryCurve(name, curves, date, groupName, "Issuer")).toMap();
			builder.put(date, LegalEntityCurveGroup.of(groupName, repoCurves, issuerCurves));
		  }
		}
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
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

	  private static IDictionary<CurveName, LoadedCurveSettings> parseCurveSettings(CharSource settingsResource)
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
		  ValueType valueType = VALUE_TYPE_MAP.get(valueTypeStr.ToLower(Locale.ENGLISH));
		  CurveInterpolator interpolator = CurveInterpolator.of(interpolatorStr);
		  CurveExtrapolator leftExtrap = CurveExtrapolator.of(leftExtrapolatorStr);
		  CurveExtrapolator rightExtrap = CurveExtrapolator.of(rightExtrapolatorStr);
		  // ONE_ONE day count is not used
		  DayCount dayCount = LoaderUtils.parseDayCount(dayCountStr);
		  LoadedCurveSettings settings = LoadedCurveSettings.of(curveName, ValueType.YEAR_FRACTION, valueType, dayCount, interpolator, leftExtrap, rightExtrap);
		  builder.put(curveName, settings);
		}
		return builder.build();
	  }

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
	  private static void parseCurveMaps(CharSource groupsCharSource, IDictionary<CurveGroupName, IDictionary<Pair<RepoGroup, Currency>, CurveName>> repoGroups, IDictionary<CurveGroupName, IDictionary<Pair<LegalEntityGroup, Currency>, CurveName>> legalEntityGroups)
	  {
		CsvFile csv = CsvFile.of(groupsCharSource, true);
		foreach (CsvRow row in csv.rows())
		{
		  string curveGroupStr = row.getField(GROUPS_NAME);
		  string curveTypeStr = row.getField(GROUPS_CURVE_TYPE);
		  string referenceStr = row.getField(GROUPS_REFERENCE);
		  string currencyStr = row.getField(GROUPS_CURRENCY);
		  string curveNameStr = row.getField(GROUPS_CURVE_NAME);
		  CurveName curveName = CurveName.of(curveNameStr);
		  createKey(curveName, CurveGroupName.of(curveGroupStr), curveTypeStr, referenceStr, currencyStr, repoGroups, legalEntityGroups);
		}
	  }

	  private static void createKey(CurveName curveName, CurveGroupName curveGroup, string curveTypeStr, string referenceStr, string currencyStr, IDictionary<CurveGroupName, IDictionary<Pair<RepoGroup, Currency>, CurveName>> repoGroups, IDictionary<CurveGroupName, IDictionary<Pair<LegalEntityGroup, Currency>, CurveName>> legalEntityGroups)
	  {

		Currency currency = Currency.of(currencyStr);
		if (REPO.Equals(curveTypeStr.ToLower(Locale.ENGLISH), StringComparison.OrdinalIgnoreCase))
		{
		  RepoGroup repoGroup = RepoGroup.of(referenceStr);
		  repoGroups.computeIfAbsent(curveGroup, k => new LinkedHashMap<>()).put(Pair.of(repoGroup, currency), curveName);
		}
		else if (ISSUER.Equals(curveTypeStr.ToLower(Locale.ENGLISH), StringComparison.OrdinalIgnoreCase))
		{
		  LegalEntityGroup legalEntiryGroup = LegalEntityGroup.of(referenceStr);
		  legalEntityGroups.computeIfAbsent(curveGroup, k => new LinkedHashMap<>()).put(Pair.of(legalEntiryGroup, currency), curveName);
		}
		else
		{
		  throw new System.ArgumentException(Messages.format("Unsupported curve type: {}", curveTypeStr));
		}
	  }

	  //-------------------------------------------------------------------------
	  private static Curve queryCurve(CurveName name, IDictionary<CurveName, Curve> curves, LocalDate date, CurveGroupName groupName, string curveType)
	  {

		Curve curve = curves[name];
		if (curve == null)
		{
		  throw new System.ArgumentException(curveType + " curve values for " + name.ToString() + " in group " + groupName.Name + " are missing on " + date.ToString());
		}
		return curve;
	  }

	}

}