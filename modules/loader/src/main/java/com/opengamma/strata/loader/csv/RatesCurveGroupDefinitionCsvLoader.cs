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
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using LinkedHashMultimap = com.google.common.collect.LinkedHashMultimap;
	using Multimap = com.google.common.collect.Multimap;
	using CharSource = com.google.common.io.CharSource;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Index = com.opengamma.strata.basics.index.Index;
	using MapStream = com.opengamma.strata.collect.MapStream;
	using Messages = com.opengamma.strata.collect.Messages;
	using CsvFile = com.opengamma.strata.collect.io.CsvFile;
	using CsvOutput = com.opengamma.strata.collect.io.CsvOutput;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using UnicodeBom = com.opengamma.strata.collect.io.UnicodeBom;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using NodalCurveDefinition = com.opengamma.strata.market.curve.NodalCurveDefinition;
	using RatesCurveGroup = com.opengamma.strata.market.curve.RatesCurveGroup;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using RatesCurveGroupEntry = com.opengamma.strata.market.curve.RatesCurveGroupEntry;

	/// <summary>
	/// Loads a set of curve group definitions into memory by reading from CSV resources.
	/// <para>
	/// The CSV file has the following header row:<br />
	/// {@code Group Name, Curve Type, Reference, Curve Name}.
	/// 
	/// <ul>
	///   <li>The 'Group Name' column is the name of the group of curves.
	///   <li>The 'Curve Type' column is the type of the curve, "forward" or "discount".
	///   <li>The 'Reference' column is the reference the curve is used for, such as "USD" or "USD-LIBOR-3M".
	///   <li>The 'Curve Name' column is the name of the curve.
	/// </ul>
	/// </para>
	/// <para>
	/// CSV files sometimes contain a Unicode Byte Order Mark.
	/// Callers are responsible for handling this, such as by using <seealso cref="UnicodeBom"/>.
	/// 
	/// </para>
	/// </summary>
	/// <seealso cref= RatesCurveGroupDefinition </seealso>
	public sealed class RatesCurveGroupDefinitionCsvLoader
	{

	  // Column headers
	  private const string GROUPS_NAME = "Group Name";
	  private const string GROUPS_CURVE_TYPE = "Curve Type";
	  private const string GROUPS_REFERENCE = "Reference";
	  private const string GROUPS_CURVE_NAME = "Curve Name";
	  private static readonly ImmutableList<string> HEADERS = ImmutableList.of(GROUPS_NAME, GROUPS_CURVE_TYPE, GROUPS_REFERENCE, GROUPS_CURVE_NAME);

	  /// <summary>
	  /// Name used in the reference column of the CSV file for discount curves. </summary>
	  private const string DISCOUNT = "discount";

	  /// <summary>
	  /// Name used in the reference column of the CSV file for forward curves. </summary>
	  private const string FORWARD = "forward";

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Loads the curve groups definition CSV file.
	  /// <para>
	  /// The list of <seealso cref="NodalCurveDefinition"/> will be empty in the resulting definition.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="groupsResource">  the curve groups CSV resource </param>
	  /// <returns> the list of definitions </returns>
	  public static IList<RatesCurveGroupDefinition> loadCurveGroupDefinitions(ResourceLocator groupsResource)
	  {
		return parseCurveGroupDefinitions(groupsResource.CharSource);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses the curve groups definition CSV file.
	  /// <para>
	  /// The list of <seealso cref="NodalCurveDefinition"/> will be empty in the resulting definition.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="groupsCharSource">  the curve groups CSV character source </param>
	  /// <returns> the list of definitions </returns>
	  public static IList<RatesCurveGroupDefinition> parseCurveGroupDefinitions(CharSource groupsCharSource)
	  {
		IDictionary<CurveName, ISet<GroupAndReference>> curveGroups = new LinkedHashMap<CurveName, ISet<GroupAndReference>>();
		CsvFile csv = CsvFile.of(groupsCharSource, true);
		foreach (CsvRow row in csv.rows())
		{
		  string curveGroupStr = row.getField(GROUPS_NAME);
		  string curveTypeStr = row.getField(GROUPS_CURVE_TYPE);
		  string referenceStr = row.getField(GROUPS_REFERENCE);
		  string curveNameStr = row.getField(GROUPS_CURVE_NAME);

		  GroupAndReference gar = createKey(CurveGroupName.of(curveGroupStr), curveTypeStr, referenceStr);
		  CurveName curveName = CurveName.of(curveNameStr);
		  curveGroups.computeIfAbsent(curveName, k => new LinkedHashSet<>()).add(gar);
		}
		return buildCurveGroups(curveGroups);
	  }

	  //-------------------------------------------------------------------------
	  // parses the identifier
	  private static GroupAndReference createKey(CurveGroupName curveGroup, string curveTypeStr, string referenceStr)
	  {

		// discount and forward curves are supported
		if (FORWARD.Equals(curveTypeStr.ToLower(Locale.ENGLISH), StringComparison.OrdinalIgnoreCase))
		{
		  Index index = LoaderUtils.findIndex(referenceStr);
		  return new GroupAndReference(curveGroup, index);

		}
		else if (DISCOUNT.Equals(curveTypeStr.ToLower(Locale.ENGLISH), StringComparison.OrdinalIgnoreCase))
		{
		  Currency ccy = Currency.of(referenceStr);
		  return new GroupAndReference(curveGroup, ccy);

		}
		else
		{
		  throw new System.ArgumentException(Messages.format("Unsupported curve type: {}", curveTypeStr));
		}
	  }

	  /// <summary>
	  /// Builds a list of curve group definitions from the map of curves and their keys.
	  /// <para>
	  /// The keys specify which curve groups each curve belongs to and how it is used in the group, for example
	  /// as a discount curve for a particular currency or as a forward curve for an index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="garMap">  the map of name to keys </param>
	  /// <returns> a map of curve group name to curve group definition built from the curves </returns>
	  private static ImmutableList<RatesCurveGroupDefinition> buildCurveGroups(IDictionary<CurveName, ISet<GroupAndReference>> garMap)
	  {

		Multimap<CurveGroupName, RatesCurveGroupEntry> groups = LinkedHashMultimap.create();

		foreach (KeyValuePair<CurveName, ISet<GroupAndReference>> entry in garMap.SetOfKeyValuePairs())
		{
		  CurveName curveName = entry.Key;
		  ISet<GroupAndReference> curveIds = entry.Value;
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		  IDictionary<CurveGroupName, IList<GroupAndReference>> idsByGroup = curveIds.collect(groupingBy(p => p.groupName));

		  foreach (KeyValuePair<CurveGroupName, IList<GroupAndReference>> groupEntry in idsByGroup.SetOfKeyValuePairs())
		  {
			CurveGroupName groupName = groupEntry.Key;
			IList<GroupAndReference> gars = groupEntry.Value;
			groups.put(groupName, curveGroupEntry(curveName, gars));
		  }
		}
		return MapStream.of(groups.asMap()).map((name, entry) => RatesCurveGroupDefinition.of(name, entry, ImmutableList.of())).collect(toImmutableList());
	  }

	  /// <summary>
	  /// Creates a curve group entry for a curve from a list of keys from the same curve group.
	  /// </summary>
	  /// <param name="curveName">  the name of the curve </param>
	  /// <param name="gars">  the group-reference pairs </param>
	  /// <returns> a curve group entry built from the data in the IDs </returns>
	  private static RatesCurveGroupEntry curveGroupEntry(CurveName curveName, IList<GroupAndReference> gars)
	  {
		ISet<Currency> currencies = new LinkedHashSet<Currency>();
		ISet<Index> indices = new LinkedHashSet<Index>();

		foreach (GroupAndReference gar in gars)
		{
		  if (gar.currency != null)
		  {
			currencies.Add(gar.currency);
		  }
		  else
		  {
			indices.Add(gar.index);
		  }
		}
		return RatesCurveGroupEntry.builder().curveName(curveName).discountCurrencies(currencies).indices(indices).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Writes the curve groups definition in a CSV format to a file.
	  /// </summary>
	  /// <param name="file">  the destination for the CSV, such as a file </param>
	  /// <param name="groups">  the curve groups </param>
	  public static void writeCurveGroupDefinition(File file, params RatesCurveGroupDefinition[] groups)
	  {
		try
		{
				using (Writer writer = new StreamWriter(new FileStream(file, FileMode.Create, FileAccess.Write), Encoding.UTF8))
				{
			  writeCurveGroupDefinition(writer, groups);
				}
		}
		catch (IOException ex)
		{
		  throw new UncheckedIOException(ex);
		}
	  }

	  /// <summary>
	  /// Writes the curve groups definition in a CSV format to an appendable.
	  /// </summary>
	  /// <param name="underlying">  the underlying appendable destination </param>
	  /// <param name="groups">  the curve groups </param>
	  public static void writeCurveGroupDefinition(Appendable underlying, params RatesCurveGroupDefinition[] groups)
	  {
		CsvOutput csv = CsvOutput.standard(underlying);
		csv.writeLine(HEADERS);
		foreach (RatesCurveGroupDefinition group in groups)
		{
		  writeCurveGroupDefinition(csv, group);
		}
	  }

	  // write a single group definition to CSV
	  private static void writeCurveGroupDefinition(CsvOutput csv, RatesCurveGroupDefinition group)
	  {
		string groupName = group.Name.Name;
		foreach (RatesCurveGroupEntry entry in group.Entries)
		{
		  foreach (Currency currency in entry.DiscountCurrencies)
		  {
			csv.writeLine(ImmutableList.of(groupName, DISCOUNT, currency.ToString(), entry.CurveName.Name));
		  }
		  foreach (Index index in entry.Indices)
		  {
			csv.writeLine(ImmutableList.of(groupName, FORWARD, index.ToString(), entry.CurveName.Name));
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Writes the curve group in a CSV format to a file.
	  /// </summary>
	  /// <param name="file">  the file </param>
	  /// <param name="groups">  the curve groups </param>
	  public static void writeCurveGroup(File file, params RatesCurveGroup[] groups)
	  {
		try
		{
				using (Writer writer = new StreamWriter(new FileStream(file, FileMode.Create, FileAccess.Write), Encoding.UTF8))
				{
			  writeCurveGroup(writer, groups);
				}
		}
		catch (IOException ex)
		{
		  throw new UncheckedIOException(ex);
		}
	  }

	  /// <summary>
	  /// Writes the curve group in a CSV format to an appendable.
	  /// </summary>
	  /// <param name="underlying">  the underlying appendable destination </param>
	  /// <param name="groups">  the curve groups </param>
	  public static void writeCurveGroup(Appendable underlying, params RatesCurveGroup[] groups)
	  {
		CsvOutput csv = CsvOutput.standard(underlying);
		csv.writeLine(HEADERS);
		foreach (RatesCurveGroup group in groups)
		{
		  writeCurveGroup(csv, group);
		}
	  }

	  // write a single group to CSV
	  private static void writeCurveGroup(CsvOutput csv, RatesCurveGroup group)
	  {
		string groupName = group.Name.Name;
		IDictionary<Currency, Curve> discountingCurves = group.DiscountCurves;
		foreach (KeyValuePair<Currency, Curve> entry in discountingCurves.SetOfKeyValuePairs())
		{
		  IList<string> line = new List<string>(4);
		  line.Add(groupName);
		  line.Add(DISCOUNT);
		  line.Add(entry.Key.ToString());
		  line.Add(entry.Value.Name.Name);
		  csv.writeLine(line);
		}
		IDictionary<Index, Curve> forwardCurves = group.ForwardCurves;
		foreach (KeyValuePair<Index, Curve> entry in forwardCurves.SetOfKeyValuePairs())
		{
		  IList<string> line = new List<string>(4);
		  line.Add(groupName);
		  line.Add(FORWARD);
		  line.Add(entry.Key.ToString());
		  line.Add(entry.Value.Name.Name);
		  csv.writeLine(line);
		}
	  }

	  //-------------------------------------------------------------------------
	  // This class only has static methods
	  private RatesCurveGroupDefinitionCsvLoader()
	  {
	  }

	  //-------------------------------------------------------------------------
	  // data holder
	  private sealed class GroupAndReference
	  {
		internal readonly CurveGroupName groupName;
		internal readonly Currency currency;
		internal readonly Index index;

		internal GroupAndReference(CurveGroupName groupName, Currency currency)
		{
		  this.groupName = groupName;
		  this.currency = currency;
		  this.index = null;
		}

		internal GroupAndReference(CurveGroupName groupName, Index index)
		{
		  this.groupName = groupName;
		  this.currency = null;
		  this.index = index;
		}
	  }

	}

}