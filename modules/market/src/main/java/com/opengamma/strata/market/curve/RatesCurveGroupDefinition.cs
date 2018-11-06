using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableMap;


	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Sets = com.google.common.collect.Sets;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FloatingRateIndex = com.opengamma.strata.basics.index.FloatingRateIndex;
	using FloatingRateName = com.opengamma.strata.basics.index.FloatingRateName;
	using Index = com.opengamma.strata.basics.index.Index;
	using PriceIndex = com.opengamma.strata.basics.index.PriceIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MapStream = com.opengamma.strata.collect.MapStream;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;

	/// <summary>
	/// Provides the definition of how to calibrate a group of curves.
	/// <para>
	/// A curve group contains one or more entries, each of which contains the definition of a curve
	/// and a set of currencies and indices specifying how the curve is to be used.
	/// The currencies are used to specify that the curve is to be used as a discount curve.
	/// The indices are used to specify that the curve is to be used as a forward curve.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class RatesCurveGroupDefinition implements CurveGroupDefinition, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class RatesCurveGroupDefinition : CurveGroupDefinition, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final CurveGroupName name;
		private readonly CurveGroupName name;
	  /// <summary>
	  /// The configuration for building the curves in the group.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableList<RatesCurveGroupEntry> entries;
	  private readonly ImmutableList<RatesCurveGroupEntry> entries;
	  /// <summary>
	  /// Definitions which specify how the curves are calibrated.
	  /// <para>
	  /// Curve definitions are required for curves that need to be calibrated. A definition is not necessary if
	  /// the curve is not built by the Strata curve calibrator.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", builderType = "List<? extends CurveDefinition>") private final com.google.common.collect.ImmutableList<CurveDefinition> curveDefinitions;
	  private readonly ImmutableList<CurveDefinition> curveDefinitions;
	  /// <summary>
	  /// Definitions which specify which seasonality should be used for some price index curves.
	  /// <para>
	  /// If a curve linked to a price index does not have an entry in the map, no seasonality is used for that curve.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<CurveName, SeasonalityDefinition> seasonalityDefinitions;
	  private readonly ImmutableMap<CurveName, SeasonalityDefinition> seasonalityDefinitions;
	  /// <summary>
	  /// The flag indicating if the Jacobian matrices should be computed and stored in metadata or not.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final boolean computeJacobian;
	  private readonly bool computeJacobian;
	  /// <summary>
	  /// The flag indicating if present value sensitivity to market quotes should be computed and stored in metadata or not.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final boolean computePvSensitivityToMarketQuote;
	  private readonly bool computePvSensitivityToMarketQuote;

	  /// <summary>
	  /// Entries for the curves, keyed by the curve name.
	  /// </summary>
	  [NonSerialized]
	  private readonly ImmutableMap<CurveName, RatesCurveGroupEntry> entriesByName; // not a property
	  /// <summary>
	  /// Definitions for the curves, keyed by the curve name.
	  /// </summary>
	  [NonSerialized]
	  private readonly ImmutableMap<CurveName, CurveDefinition> curveDefinitionsByName; // not a property

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a mutable builder for building the definition for a curve group.
	  /// </summary>
	  /// <returns> a mutable builder for building the definition for a curve group </returns>
	  public static RatesCurveGroupDefinitionBuilder builder()
	  {
		return new RatesCurveGroupDefinitionBuilder();
	  }

	  /// <summary>
	  /// Returns a curve group definition with the specified name and containing the specified entries.
	  /// <para>
	  /// The Jacobian matrices are computed. The Present Value sensitivity to Market quotes are not computed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name of the curve group </param>
	  /// <param name="entries">  entries describing the curves in the group </param>
	  /// <param name="curveDefinitions">  definitions which specify how the curves are calibrated </param>
	  /// <returns> a curve group definition with the specified name and containing the specified entries </returns>
	  public static RatesCurveGroupDefinition of(CurveGroupName name, ICollection<RatesCurveGroupEntry> entries, ICollection<CurveDefinition> curveDefinitions)
	  {

		return new RatesCurveGroupDefinition(name, entries, curveDefinitions, ImmutableMap.of(), true, false);
	  }

	  /// <summary>
	  /// Returns a curve group definition with the specified name and containing the specified entries and seasonality.
	  /// <para>
	  /// The Jacobian matrices are computed. The Present Value sensitivity to Market quotes are not computed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name of the curve group </param>
	  /// <param name="entries">  entries describing the curves in the group </param>
	  /// <param name="curveDefinitions">  definitions which specify how the curves are calibrated </param>
	  /// <param name="seasonalityDefinitions">  definitions which specify the seasonality to use for different curves </param>
	  /// <returns> a curve group definition with the specified name and containing the specified entries </returns>
	  public static RatesCurveGroupDefinition of(CurveGroupName name, ICollection<RatesCurveGroupEntry> entries, ICollection<CurveDefinition> curveDefinitions, IDictionary<CurveName, SeasonalityDefinition> seasonalityDefinitions)
	  {

		return new RatesCurveGroupDefinition(name, entries, curveDefinitions, seasonalityDefinitions, true, false);
	  }

	  /// <summary>
	  /// Package-private constructor used by the builder.
	  /// </summary>
	  /// <param name="name">  the name of the curve group </param>
	  /// <param name="entries">  details of the curves in the group </param>
	  /// <param name="curveDefinitions">  definitions which specify how the curves are calibrated </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor RatesCurveGroupDefinition(CurveGroupName name, java.util.Collection<RatesCurveGroupEntry> entries, java.util.Collection<? extends CurveDefinition> curveDefinitions, java.util.Map<CurveName, SeasonalityDefinition> seasonalityDefinitions, boolean computeJacobian, boolean computePvSensitivityToMarketQuote)
	  internal RatesCurveGroupDefinition<T1>(CurveGroupName name, ICollection<RatesCurveGroupEntry> entries, ICollection<T1> curveDefinitions, IDictionary<CurveName, SeasonalityDefinition> seasonalityDefinitions, bool computeJacobian, bool computePvSensitivityToMarketQuote) where T1 : CurveDefinition
	  {

		this.name = ArgChecker.notNull(name, "name");
		this.entries = ImmutableList.copyOf(entries);
		this.curveDefinitions = ImmutableList.copyOf(curveDefinitions);
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		this.entriesByName = entries.collect(toImmutableMap(entry => entry.CurveName, entry => entry));
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		this.curveDefinitionsByName = curveDefinitions.collect(toImmutableMap(def => def.Name, def => def));
		this.computeJacobian = computeJacobian;
		this.computePvSensitivityToMarketQuote = computePvSensitivityToMarketQuote;
		this.seasonalityDefinitions = ImmutableMap.copyOf(seasonalityDefinitions);
		validate();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ISet<CurveName> missingEntries = Sets.difference(curveDefinitionsByName.Keys, entriesByName.Keys);
		if (missingEntries.Count > 0)
		{
		  throw new System.ArgumentException("An entry must be provided for every curve definition but the following " + "curves have a definition but no entry: " + missingEntries);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (builder.computePvSensitivityToMarketQuote)
		{
		  builder.computeJacobian = true;
		}
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new RatesCurveGroupDefinition(name, entries, curveDefinitions, seasonalityDefinitions, computeJacobian, computePvSensitivityToMarketQuote);
	  }

	  //-------------------------------------------------------------------------
	  public RatesCurveGroupId createGroupId(ObservableSource source)
	  {
		return RatesCurveGroupId.of(name, source);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a filtered version of this definition with no invalid nodes.
	  /// <para>
	  /// A curve is formed of a number of nodes, each of which has an associated date.
	  /// To be valid, the curve node dates must be in order from earliest to latest.
	  /// This method applies rules to remove invalid nodes.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the resolved definition, that should be used in preference to this one </returns>
	  /// <exception cref="IllegalArgumentException"> if the curve nodes are invalid </exception>
	  public RatesCurveGroupDefinition filtered(LocalDate valuationDate, ReferenceData refData)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<CurveDefinition> filtered = curveDefinitions.Select(ncd => ncd.filtered(valuationDate, refData)).collect(toImmutableList());
		return new RatesCurveGroupDefinition(name, entries, filtered, seasonalityDefinitions, computeJacobian, computePvSensitivityToMarketQuote);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a definition that is bound to a time-series.
	  /// <para>
	  /// Curves related to a price index are better described when a starting point is added
	  /// with the last fixing in the time series. This method finds price index curves, and ensures
	  /// that they are unique (not used for any other index or discounting). Each price index
	  /// curve is then bound to the matching time-series with the last fixing month equal to
	  /// the last element in the time series which is in the past.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <param name="tsMap">  the map of index to time series </param>
	  /// <returns> the new instance </returns>
	  public RatesCurveGroupDefinition bindTimeSeries(LocalDate valuationDate, IDictionary<Index, LocalDateDoubleTimeSeries> tsMap)
	  {
		ImmutableList.Builder<CurveDefinition> boundCurveDefinitions = ImmutableList.builder();
		foreach (RatesCurveGroupEntry entry in entries)
		{
		  CurveName name = entry.CurveName;
		  CurveDefinition curveDef = curveDefinitionsByName.get(name);
		  ISet<Index> indices = entry.Indices;
		  bool containsPriceIndex = indices.Any(i => i is PriceIndex);
		  if (containsPriceIndex)
		  {
			// check only one curve for Price Index and find time-series last value
			ArgChecker.isTrue(indices.Count == 1, "Price index curve must not relate to another index or discounting: " + name);
			Index index = indices.GetEnumerator().next();
			LocalDateDoubleTimeSeries ts = tsMap[index];
			ArgChecker.notNull(ts, "Price index curve must have associated time-series: " + index.ToString());
			// retrieve last fixing for months before the valuation date
			LocalDateDoubleTimeSeries tsPast = ts.subSeries(ts.EarliestDate, valuationDate);
			ArgChecker.isFalse(ts.Empty, "Price index curve must have associated time-series with at least one element in the past:" + index.ToString());
			ArgChecker.isTrue(curveDef is NodalCurveDefinition, "curve definition for inflation curve must be NodalCurveDefinition");
			YearMonth lastFixingMonth = YearMonth.from(tsPast.LatestDate);
			double lastFixingValue = tsPast.LatestValue;
			InflationNodalCurveDefinition seasonalCurveDef = new InflationNodalCurveDefinition((NodalCurveDefinition) curveDef, lastFixingMonth, lastFixingValue, seasonalityDefinitions.get(name));
			boundCurveDefinitions.add(seasonalCurveDef);
		  }
		  else
		  {
			// no price index
			boundCurveDefinitions.add(curveDef);
		  }
		}
		return this.withCurveDefinitions(boundCurveDefinitions.build());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the discount curve name for the specified currency.
	  /// <para>
	  /// If the curve name is not found, optional empty is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="discountCurrency">  the currency to find a discount curve name for </param>
	  /// <returns> the curve name </returns>
	  public Optional<CurveName> findDiscountCurveName(Currency discountCurrency)
	  {
		return entries.Where(entry => entry.DiscountCurrencies.contains(discountCurrency)).First().Select(entry => entry.CurveName);
	  }

	  /// <summary>
	  /// Finds the forward curve name for the specified index.
	  /// <para>
	  /// If the curve name is not found, optional empty is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forwardIndex">  the index to find a forward curve name for </param>
	  /// <returns> the curve name </returns>
	  public Optional<CurveName> findForwardCurveName(Index forwardIndex)
	  {
		return entries.Where(entry => entry.Indices.contains(forwardIndex)).First().Select(entry => entry.CurveName);
	  }

	  /// <summary>
	  /// Finds the forward curve names for the specified floating rate name.
	  /// <para>
	  /// If the curve name is not found, optional empty is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forwardName">  the floating rate name to find a forward curve name for </param>
	  /// <returns> the set of curve names </returns>
	  public ImmutableSet<CurveName> findForwardCurveNames(FloatingRateName forwardName)
	  {
		ImmutableSet.Builder<CurveName> result = ImmutableSet.builder();
		FloatingRateName normalized = forwardName.normalized();
		foreach (RatesCurveGroupEntry entry in entries)
		{
		  foreach (Index index in entry.Indices)
		  {
			if (index is FloatingRateIndex)
			{
			  FloatingRateName frName = ((FloatingRateIndex) index).FloatingRateName;
			  if (frName.Equals(normalized))
			  {
				result.add(entry.CurveName);
				break;
			  }
			}
		  }
		}
		return result.build();
	  }

	  /// <summary>
	  /// Finds the entry for the curve with the specified name.
	  /// <para>
	  /// If the curve is not found, optional empty is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveName">  the name of the curve </param>
	  /// <returns> the entry for the curve with the specified name </returns>
	  public Optional<RatesCurveGroupEntry> findEntry(CurveName curveName)
	  {
		return Optional.ofNullable(entriesByName.get(curveName));
	  }

	  /// <summary>
	  /// Finds the definition for the curve with the specified name.
	  /// <para>
	  /// If the curve is not found, optional empty is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveName">  the name of the curve </param>
	  /// <returns> the definition for the curve with the specified name </returns>
	  public Optional<CurveDefinition> findCurveDefinition(CurveName curveName)
	  {
		return Optional.ofNullable(curveDefinitionsByName.get(curveName));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates the curve metadata for each definition.
	  /// <para>
	  /// This method returns a list of metadata, one for each curve definition.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the metadata </returns>
	  public ImmutableList<CurveMetadata> metadata(LocalDate valuationDate, ReferenceData refData)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return curveDefinitionsByName.values().Select(curveDef => curveDef.metadata(valuationDate, refData)).collect(toImmutableList());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the total number of parameters in the group.
	  /// <para>
	  /// This returns the total number of parameters in the group, which equals the number of nodes.
	  /// The result of <seealso cref="#resolvedTrades(MarketData, ReferenceData)"/>, and
	  /// <seealso cref="#initialGuesses(MarketData)"/> will be of this size.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the number of parameters </returns>
	  public int TotalParameterCount
	  {
		  get
		  {
			return curveDefinitionsByName.entrySet().Select(entry => entry.Value.ParameterCount).Sum();
		  }
	  }

	  /// <summary>
	  /// Creates a list of trades representing the instrument at each node.
	  /// <para>
	  /// This uses the observed market data to build the trade that each node represents.
	  /// The result combines the list of trades from each curve in order.
	  /// Each trade is created with a quantity of 1.
	  /// The valuation date is defined by the market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketData">  the market data required to build a trade for the instrument, including the valuation date </param>
	  /// <param name="refData">  the reference data, used to resolve the trades </param>
	  /// <returns> the list of all trades </returns>
	  public ImmutableList<ResolvedTrade> resolvedTrades(MarketData marketData, ReferenceData refData)
	  {
		return curveDefinitionsByName.values().stream().flatMap(curveDef => curveDef.Nodes.stream()).map(node => node.resolvedTrade(1d, marketData, refData)).collect(toImmutableList());
	  }

	  /// <summary>
	  /// Gets the list of all initial guesses.
	  /// <para>
	  /// This returns a list that combines the list of initial guesses from each curve in order.
	  /// The valuation date is defined by the market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketData">  the market data required to build a trade for the instrument, including the valuation date </param>
	  /// <returns> the list of all initial guesses </returns>
	  public ImmutableList<double> initialGuesses(MarketData marketData)
	  {
		ImmutableList.Builder<double> result = ImmutableList.builder();
		foreach (CurveDefinition defn in curveDefinitions)
		{
		  result.addAll(defn.initialGuess(marketData));
		}
		return result.build();
	  }

	  /// <summary>
	  /// Returns a copy of this object containing the specified curve definitions.
	  /// <para>
	  /// Curves are ignored if there is no entry in this definition with the same curve name.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveDefinitions">  curve definitions </param>
	  /// <returns> a copy of this object containing the specified curve definitions </returns>
	  public RatesCurveGroupDefinition withCurveDefinitions(IList<CurveDefinition> curveDefinitions)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ISet<CurveName> curveNames = entries.Select(entry => entry.CurveName).collect(toSet());
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<CurveDefinition> filteredDefinitions = curveDefinitions.Where(def => curveNames.Contains(def.Name)).collect(toImmutableList());
		return new RatesCurveGroupDefinition(name, entries, filteredDefinitions, seasonalityDefinitions, computeJacobian, computePvSensitivityToMarketQuote);
	  }

	  /// <summary>
	  /// Returns a copy of this object containing the specified seasonality definitions.
	  /// <para>
	  /// Seasonality definitions are ignored if there is no entry in this definition with the same curve name.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="seasonalityDefinitions">  seasonality definitions </param>
	  /// <returns> a copy of this object containing the specified seasonality definitions </returns>
	  public RatesCurveGroupDefinition withSeasonalityDefinitions(IDictionary<CurveName, SeasonalityDefinition> seasonalityDefinitions)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ISet<CurveName> curveNames = entries.Select(entry => entry.CurveName).collect(toSet());
		IDictionary<CurveName, SeasonalityDefinition> filteredDefinitions = MapStream.of(seasonalityDefinitions).filterKeys(cn => curveNames.Contains(cn)).toMap();
		return new RatesCurveGroupDefinition(name, entries, curveDefinitions, filteredDefinitions, computeJacobian, computePvSensitivityToMarketQuote);
	  }

	  /// <summary>
	  /// Returns a copy of this definition with a different name.
	  /// </summary>
	  /// <param name="name">  the name of the new curve group definition </param>
	  /// <returns> a copy of this curve group definition with a different name </returns>
	  public RatesCurveGroupDefinition withName(CurveGroupName name)
	  {
		return new RatesCurveGroupDefinition(name, entries, curveDefinitions, seasonalityDefinitions, computeJacobian, computePvSensitivityToMarketQuote);
	  }

	  /// <summary>
	  /// Combines this definition with another one.
	  /// <para>
	  /// This combines the curve definitions, curve entries and seasonality with those from the other definition.
	  /// An exception is thrown if unable to merge, such as if the curve definitions clash.
	  /// The group name will be taken from this definition only.
	  /// The seasonality will be taken from this definition only if there is a clash.
	  /// The boolean flags will be combined using logical OR.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other definition </param>
	  /// <returns> the combined curve group definition </returns>
	  /// <exception cref="IllegalArgumentException"> if unable to merge </exception>
	  public RatesCurveGroupDefinition combinedWith(RatesCurveGroupDefinition other)
	  {
		// merge definitions
		IDictionary<CurveName, CurveDefinition> combinedDefinitions = new LinkedHashMap<CurveName, CurveDefinition>(this.curveDefinitionsByName);
		foreach (CurveDefinition otherDefn in other.curveDefinitions)
		{
		  CurveDefinition thisDefn = this.curveDefinitionsByName.get(otherDefn.Name);
		  if (thisDefn == null)
		  {
			combinedDefinitions[otherDefn.Name] = otherDefn;
		  }
		  else if (!thisDefn.Equals(otherDefn))
		  {
			throw new System.ArgumentException("Curve definitions clash: " + thisDefn.Name);
		  }
		}
		// merge entries
		IDictionary<CurveName, RatesCurveGroupEntry> combinedEntries = new LinkedHashMap<CurveName, RatesCurveGroupEntry>(this.entriesByName);
		foreach (RatesCurveGroupEntry otherEntry in other.entries)
		{
		  RatesCurveGroupEntry thisEntry = this.entriesByName.get(otherEntry.CurveName);
		  if (thisEntry == null)
		  {
			combinedEntries[otherEntry.CurveName] = otherEntry;
		  }
		  else
		  {
			combinedEntries[otherEntry.CurveName] = thisEntry.merge(otherEntry);
		  }
		}
		// merge definitions
		IDictionary<CurveName, SeasonalityDefinition> combinedSeasonality = new LinkedHashMap<CurveName, SeasonalityDefinition>(this.seasonalityDefinitions);
		foreach (KeyValuePair<CurveName, SeasonalityDefinition> otherEntry in other.seasonalityDefinitions.entrySet())
		{
		  SeasonalityDefinition thisDefn = this.seasonalityDefinitions.get(otherEntry.Key);
		  if (thisDefn == null)
		  {
			combinedSeasonality[otherEntry.Key] = otherEntry.Value;
		  }
		  else
		  {
			throw new System.ArgumentException("Curve definitions clash: " + otherEntry.Key);
		  }
		}
		return new RatesCurveGroupDefinition(name, combinedEntries.Values, combinedDefinitions.Values, combinedSeasonality, this.computeJacobian | other.computeJacobian, this.computePvSensitivityToMarketQuote | other.computePvSensitivityToMarketQuote);
	  }

	  /// <summary>
	  /// Converts to builder.
	  /// </summary>
	  /// <returns> the builder </returns>
	  public RatesCurveGroupDefinitionBuilder toBuilder()
	  {
		return new RatesCurveGroupDefinitionBuilder(name, entriesByName, curveDefinitionsByName, seasonalityDefinitions, computeJacobian, computePvSensitivityToMarketQuote);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code RatesCurveGroupDefinition}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static RatesCurveGroupDefinition.Meta meta()
	  {
		return RatesCurveGroupDefinition.Meta.INSTANCE;
	  }

	  static RatesCurveGroupDefinition()
	  {
		MetaBean.register(RatesCurveGroupDefinition.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override RatesCurveGroupDefinition.Meta metaBean()
	  {
		return RatesCurveGroupDefinition.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name of the curve group. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveGroupName Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the configuration for building the curves in the group. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<RatesCurveGroupEntry> Entries
	  {
		  get
		  {
			return entries;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets definitions which specify how the curves are calibrated.
	  /// <para>
	  /// Curve definitions are required for curves that need to be calibrated. A definition is not necessary if
	  /// the curve is not built by the Strata curve calibrator.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<CurveDefinition> CurveDefinitions
	  {
		  get
		  {
			return curveDefinitions;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets definitions which specify which seasonality should be used for some price index curves.
	  /// <para>
	  /// If a curve linked to a price index does not have an entry in the map, no seasonality is used for that curve.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<CurveName, SeasonalityDefinition> SeasonalityDefinitions
	  {
		  get
		  {
			return seasonalityDefinitions;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the flag indicating if the Jacobian matrices should be computed and stored in metadata or not. </summary>
	  /// <returns> the value of the property </returns>
	  public bool ComputeJacobian
	  {
		  get
		  {
			return computeJacobian;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the flag indicating if present value sensitivity to market quotes should be computed and stored in metadata or not. </summary>
	  /// <returns> the value of the property </returns>
	  public bool ComputePvSensitivityToMarketQuote
	  {
		  get
		  {
			return computePvSensitivityToMarketQuote;
		  }
	  }

	  //-----------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  RatesCurveGroupDefinition other = (RatesCurveGroupDefinition) obj;
		  return JodaBeanUtils.equal(name, other.name) && JodaBeanUtils.equal(entries, other.entries) && JodaBeanUtils.equal(curveDefinitions, other.curveDefinitions) && JodaBeanUtils.equal(seasonalityDefinitions, other.seasonalityDefinitions) && (computeJacobian == other.computeJacobian) && (computePvSensitivityToMarketQuote == other.computePvSensitivityToMarketQuote);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(name);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(entries);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(curveDefinitions);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(seasonalityDefinitions);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(computeJacobian);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(computePvSensitivityToMarketQuote);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(224);
		buf.Append("RatesCurveGroupDefinition{");
		buf.Append("name").Append('=').Append(name).Append(',').Append(' ');
		buf.Append("entries").Append('=').Append(entries).Append(',').Append(' ');
		buf.Append("curveDefinitions").Append('=').Append(curveDefinitions).Append(',').Append(' ');
		buf.Append("seasonalityDefinitions").Append('=').Append(seasonalityDefinitions).Append(',').Append(' ');
		buf.Append("computeJacobian").Append('=').Append(computeJacobian).Append(',').Append(' ');
		buf.Append("computePvSensitivityToMarketQuote").Append('=').Append(JodaBeanUtils.ToString(computePvSensitivityToMarketQuote));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code RatesCurveGroupDefinition}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(RatesCurveGroupDefinition), typeof(CurveGroupName));
			  entries_Renamed = DirectMetaProperty.ofImmutable(this, "entries", typeof(RatesCurveGroupDefinition), (Type) typeof(ImmutableList));
			  curveDefinitions_Renamed = DirectMetaProperty.ofImmutable(this, "curveDefinitions", typeof(RatesCurveGroupDefinition), (Type) typeof(ImmutableList));
			  seasonalityDefinitions_Renamed = DirectMetaProperty.ofImmutable(this, "seasonalityDefinitions", typeof(RatesCurveGroupDefinition), (Type) typeof(ImmutableMap));
			  computeJacobian_Renamed = DirectMetaProperty.ofImmutable(this, "computeJacobian", typeof(RatesCurveGroupDefinition), Boolean.TYPE);
			  computePvSensitivityToMarketQuote_Renamed = DirectMetaProperty.ofImmutable(this, "computePvSensitivityToMarketQuote", typeof(RatesCurveGroupDefinition), Boolean.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "entries", "curveDefinitions", "seasonalityDefinitions", "computeJacobian", "computePvSensitivityToMarketQuote");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code name} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveGroupName> name_Renamed;
		/// <summary>
		/// The meta-property for the {@code entries} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<RatesCurveGroupEntry>> entries = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "entries", RatesCurveGroupDefinition.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<RatesCurveGroupEntry>> entries_Renamed;
		/// <summary>
		/// The meta-property for the {@code curveDefinitions} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<CurveDefinition>> curveDefinitions = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "curveDefinitions", RatesCurveGroupDefinition.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<CurveDefinition>> curveDefinitions_Renamed;
		/// <summary>
		/// The meta-property for the {@code seasonalityDefinitions} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<CurveName, SeasonalityDefinition>> seasonalityDefinitions = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "seasonalityDefinitions", RatesCurveGroupDefinition.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<CurveName, SeasonalityDefinition>> seasonalityDefinitions_Renamed;
		/// <summary>
		/// The meta-property for the {@code computeJacobian} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<bool> computeJacobian_Renamed;
		/// <summary>
		/// The meta-property for the {@code computePvSensitivityToMarketQuote} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<bool> computePvSensitivityToMarketQuote_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "entries", "curveDefinitions", "seasonalityDefinitions", "computeJacobian", "computePvSensitivityToMarketQuote");
		internal IDictionary<string, MetaProperty<object>> metaPropertyMap$;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Meta()
		{
			if (!InstanceFieldsInitialized)
			{
				InitializeInstanceFields();
				InstanceFieldsInitialized = true;
			}
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override protected org.joda.beans.MetaProperty<?> metaPropertyGet(String propertyName)
		protected internal override MetaProperty<object> metaPropertyGet(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name_Renamed;
			case -1591573360: // entries
			  return entries_Renamed;
			case -336166639: // curveDefinitions
			  return curveDefinitions_Renamed;
			case 1051792832: // seasonalityDefinitions
			  return seasonalityDefinitions_Renamed;
			case -1730091410: // computeJacobian
			  return computeJacobian_Renamed;
			case -2061625469: // computePvSensitivityToMarketQuote
			  return computePvSensitivityToMarketQuote_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends RatesCurveGroupDefinition> builder()
		public override BeanBuilder<RatesCurveGroupDefinition> builder()
		{
		  return new RatesCurveGroupDefinition.Builder();
		}

		public override Type beanType()
		{
		  return typeof(RatesCurveGroupDefinition);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code name} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveGroupName> name()
		{
		  return name_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code entries} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<RatesCurveGroupEntry>> entries()
		{
		  return entries_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code curveDefinitions} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<CurveDefinition>> curveDefinitions()
		{
		  return curveDefinitions_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code seasonalityDefinitions} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<CurveName, SeasonalityDefinition>> seasonalityDefinitions()
		{
		  return seasonalityDefinitions_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code computeJacobian} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<bool> computeJacobian()
		{
		  return computeJacobian_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code computePvSensitivityToMarketQuote} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<bool> computePvSensitivityToMarketQuote()
		{
		  return computePvSensitivityToMarketQuote_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return ((RatesCurveGroupDefinition) bean).Name;
			case -1591573360: // entries
			  return ((RatesCurveGroupDefinition) bean).Entries;
			case -336166639: // curveDefinitions
			  return ((RatesCurveGroupDefinition) bean).CurveDefinitions;
			case 1051792832: // seasonalityDefinitions
			  return ((RatesCurveGroupDefinition) bean).SeasonalityDefinitions;
			case -1730091410: // computeJacobian
			  return ((RatesCurveGroupDefinition) bean).ComputeJacobian;
			case -2061625469: // computePvSensitivityToMarketQuote
			  return ((RatesCurveGroupDefinition) bean).ComputePvSensitivityToMarketQuote;
		  }
		  return base.propertyGet(bean, propertyName, quiet);
		}

		protected internal override void propertySet(Bean bean, string propertyName, object newValue, bool quiet)
		{
		  metaProperty(propertyName);
		  if (quiet)
		  {
			return;
		  }
		  throw new System.NotSupportedException("Property cannot be written: " + propertyName);
		}

	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The bean-builder for {@code RatesCurveGroupDefinition}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<RatesCurveGroupDefinition>
	  {

		internal CurveGroupName name;
		internal IList<RatesCurveGroupEntry> entries = ImmutableList.of();
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.List<? extends CurveDefinition> curveDefinitions = com.google.common.collect.ImmutableList.of();
		internal IList<CurveDefinition> curveDefinitions = ImmutableList.of();
		internal IDictionary<CurveName, SeasonalityDefinition> seasonalityDefinitions = ImmutableMap.of();
		internal bool computeJacobian;
		internal bool computePvSensitivityToMarketQuote;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name;
			case -1591573360: // entries
			  return entries;
			case -336166639: // curveDefinitions
			  return curveDefinitions;
			case 1051792832: // seasonalityDefinitions
			  return seasonalityDefinitions;
			case -1730091410: // computeJacobian
			  return computeJacobian;
			case -2061625469: // computePvSensitivityToMarketQuote
			  return computePvSensitivityToMarketQuote;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Builder set(String propertyName, Object newValue)
		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  this.name = (CurveGroupName) newValue;
			  break;
			case -1591573360: // entries
			  this.entries = (IList<RatesCurveGroupEntry>) newValue;
			  break;
			case -336166639: // curveDefinitions
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.curveDefinitions = (java.util.List<? extends CurveDefinition>) newValue;
			  this.curveDefinitions = (IList<CurveDefinition>) newValue;
			  break;
			case 1051792832: // seasonalityDefinitions
			  this.seasonalityDefinitions = (IDictionary<CurveName, SeasonalityDefinition>) newValue;
			  break;
			case -1730091410: // computeJacobian
			  this.computeJacobian = (bool?) newValue.Value;
			  break;
			case -2061625469: // computePvSensitivityToMarketQuote
			  this.computePvSensitivityToMarketQuote = (bool?) newValue.Value;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override RatesCurveGroupDefinition build()
		{
		  preBuild(this);
		  return new RatesCurveGroupDefinition(name, entries, curveDefinitions, seasonalityDefinitions, computeJacobian, computePvSensitivityToMarketQuote);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(224);
		  buf.Append("RatesCurveGroupDefinition.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name)).Append(',').Append(' ');
		  buf.Append("entries").Append('=').Append(JodaBeanUtils.ToString(entries)).Append(',').Append(' ');
		  buf.Append("curveDefinitions").Append('=').Append(JodaBeanUtils.ToString(curveDefinitions)).Append(',').Append(' ');
		  buf.Append("seasonalityDefinitions").Append('=').Append(JodaBeanUtils.ToString(seasonalityDefinitions)).Append(',').Append(' ');
		  buf.Append("computeJacobian").Append('=').Append(JodaBeanUtils.ToString(computeJacobian)).Append(',').Append(' ');
		  buf.Append("computePvSensitivityToMarketQuote").Append('=').Append(JodaBeanUtils.ToString(computePvSensitivityToMarketQuote));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}