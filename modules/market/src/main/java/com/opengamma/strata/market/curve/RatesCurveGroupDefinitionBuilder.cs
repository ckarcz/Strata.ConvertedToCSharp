using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Lists = com.google.common.collect.Lists;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Index = com.opengamma.strata.basics.index.Index;
	using RateIndex = com.opengamma.strata.basics.index.RateIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// A mutable builder for creating instances of {@code CurveGroupDefinition}.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public final class RatesCurveGroupDefinitionBuilder
	public sealed class RatesCurveGroupDefinitionBuilder
	{

	  /// <summary>
	  /// The name of the curve group.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private CurveGroupName name_Renamed;
	  /// <summary>
	  /// The entries in the curve group.
	  /// </summary>
	  private readonly IDictionary<CurveName, RatesCurveGroupEntry> entries;
	  /// <summary>
	  /// The definitions specifying how the curves are calibrated.
	  /// </summary>
	  private readonly IDictionary<CurveName, CurveDefinition> curveDefinitions;
	  /// <summary>
	  /// The definitions specifying which seasonality should be used some some price index curves.
	  /// </summary>
	  private readonly IDictionary<CurveName, SeasonalityDefinition> seasonalityDefinitions;
	  /// <summary>
	  /// Flag indicating if the Jacobian matrices should be computed and stored in metadata or not.
	  /// The default value is 'true'.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private bool computeJacobian_Renamed = true;
	  /// <summary>
	  /// Flag indicating if present value sensitivity to market quotes should be computed and stored in metadata or not.
	  /// The default value is 'false'.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private bool computePvSensitivityToMarketQuote_Renamed;

	  internal RatesCurveGroupDefinitionBuilder()
	  {
		this.entries = new LinkedHashMap<>();
		this.curveDefinitions = new LinkedHashMap<>();
		this.seasonalityDefinitions = new LinkedHashMap<>();
	  }

	  internal RatesCurveGroupDefinitionBuilder(CurveGroupName name, IDictionary<CurveName, RatesCurveGroupEntry> entries, IDictionary<CurveName, CurveDefinition> curveDefinitions, IDictionary<CurveName, SeasonalityDefinition> seasonalityDefinitions, bool computeJacobian, bool computePvSensitivityToMarketQuote)
	  {
		this.name_Renamed = name;
		this.entries = entries;
		this.curveDefinitions = curveDefinitions;
		this.seasonalityDefinitions = seasonalityDefinitions;
		this.computeJacobian_Renamed = computeJacobian;
		this.computePvSensitivityToMarketQuote_Renamed = computePvSensitivityToMarketQuote;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Sets the name of the curve group definition.
	  /// </summary>
	  /// <param name="name">  the name of the curve group, not empty </param>
	  /// <returns> this builder </returns>
	  public RatesCurveGroupDefinitionBuilder name(CurveGroupName name)
	  {
		this.name_Renamed = ArgChecker.notNull(name, "name");
		return this;
	  }

	  /// <summary>
	  /// Sets the 'compute Jacobian' flag of the curve group definition.
	  /// </summary>
	  /// <param name="computeJacobian">  the flag indicating if the Jacobian matrices should be
	  ///   computed and stored in metadata or not </param>
	  /// <returns> this builder </returns>
	  public RatesCurveGroupDefinitionBuilder computeJacobian(bool computeJacobian)
	  {
		this.computeJacobian_Renamed = computeJacobian;
		return this;
	  }

	  /// <summary>
	  /// Sets the 'compute PV sensitivity to market quote' flag of the curve group definition.
	  /// <para>
	  /// If set, the Jacobian matrices will also be calculated, even if not requested.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="computePvSensitivityToMarketQuote">  the flag indicating if present value sensitivity
	  ///   to market quotes should be computed and stored in metadata or not </param>
	  /// <returns> this builder </returns>
	  public RatesCurveGroupDefinitionBuilder computePvSensitivityToMarketQuote(bool computePvSensitivityToMarketQuote)
	  {
		this.computePvSensitivityToMarketQuote_Renamed = computePvSensitivityToMarketQuote;
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds the definition of a discount curve to the curve group definition.
	  /// </summary>
	  /// <param name="curveDefinition">  the discount curve configuration </param>
	  /// <param name="otherCurrencies">  additional currencies for which the curve can provide discount factors </param>
	  /// <param name="currency">  the currency for which the curve provides discount rates </param>
	  /// <returns> this builder </returns>
	  public RatesCurveGroupDefinitionBuilder addDiscountCurve(CurveDefinition curveDefinition, Currency currency, params Currency[] otherCurrencies)
	  {

		ArgChecker.notNull(curveDefinition, "curveDefinition");
		ArgChecker.notNull(currency, "currency");
		RatesCurveGroupEntry entry = RatesCurveGroupEntry.builder().curveName(curveDefinition.Name).discountCurrencies(ImmutableSet.copyOf(Lists.asList(currency, otherCurrencies))).build();
		return merge(entry, curveDefinition);
	  }

	  /// <summary>
	  /// Adds the definition of a discount curve to the curve group definition.
	  /// <para>
	  /// A curve added with this method cannot be calibrated by the market data system as it does not include
	  /// a curve definition. It is intended to be used with curves which are supplied by the user.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveName">  the name of the curve </param>
	  /// <param name="otherCurrencies">  additional currencies for which the curve can provide discount factors </param>
	  /// <param name="currency">  the currency for which the curve provides discount rates </param>
	  /// <returns> this builder </returns>
	  public RatesCurveGroupDefinitionBuilder addDiscountCurve(CurveName curveName, Currency currency, params Currency[] otherCurrencies)
	  {

		ArgChecker.notNull(curveName, "curveName");
		ArgChecker.notNull(currency, "currency");
		RatesCurveGroupEntry entry = RatesCurveGroupEntry.builder().curveName(curveName).discountCurrencies(ImmutableSet.copyOf(Lists.asList(currency, otherCurrencies))).build();
		return mergeEntry(entry);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds the definition of a forward curve to the curve group definition.
	  /// </summary>
	  /// <param name="curveDefinition">  the definition of the forward curve </param>
	  /// <param name="index">  the index for which the curve provides forward rates </param>
	  /// <param name="otherIndices">  the additional indices for which the curve provides forward rates </param>
	  /// <returns> this builder </returns>
	  public RatesCurveGroupDefinitionBuilder addForwardCurve(CurveDefinition curveDefinition, Index index, params Index[] otherIndices)
	  {

		ArgChecker.notNull(curveDefinition, "curveDefinition");
		ArgChecker.notNull(index, "index");
		RatesCurveGroupEntry entry = RatesCurveGroupEntry.builder().curveName(curveDefinition.Name).indices(indices(index, otherIndices)).build();
		return merge(entry, curveDefinition);
	  }

	  /// <summary>
	  /// Adds the definition of a forward curve to the curve group definition.
	  /// <para>
	  /// A curve added with this method cannot be calibrated by the market data system as it does not include
	  /// a curve definition. It is intended to be used with curves which are supplied by the user.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveName">  the name of the curve </param>
	  /// <param name="index">  the index for which the curve provides forward rates </param>
	  /// <param name="otherIndices">  the additional indices for which the curve provides forward rates </param>
	  /// <returns> this builder </returns>
	  public RatesCurveGroupDefinitionBuilder addForwardCurve(CurveName curveName, Index index, params Index[] otherIndices)
	  {

		ArgChecker.notNull(curveName, "curveName");
		ArgChecker.notNull(index, "index");

		RatesCurveGroupEntry entry = RatesCurveGroupEntry.builder().curveName(curveName).indices(indices(index, otherIndices)).build();
		return mergeEntry(entry);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds the definition of a curve to the curve group definition which is used to provide
	  /// discount rates and forward rates.
	  /// </summary>
	  /// <param name="curveDefinition">  the definition of the forward curve </param>
	  /// <param name="currency">  the currency for which the curve provides discount rates </param>
	  /// <param name="index">  the index for which the curve provides forward rates </param>
	  /// <param name="otherIndices">  the additional indices for which the curve provides forward rates </param>
	  /// <returns> this builder </returns>
	  public RatesCurveGroupDefinitionBuilder addCurve(CurveDefinition curveDefinition, Currency currency, RateIndex index, params RateIndex[] otherIndices)
	  {

		ArgChecker.notNull(curveDefinition, "curveDefinition");
		ArgChecker.notNull(currency, "currency");
		ArgChecker.notNull(index, "index");

		RatesCurveGroupEntry entry = RatesCurveGroupEntry.builder().curveName(curveDefinition.Name).discountCurrencies(ImmutableSet.of(currency)).indices(indices(index, otherIndices)).build();
		return merge(entry, curveDefinition);
	  }

	  /// <summary>
	  /// Adds a curve to the curve group definition which is used to provide discount rates and forward rates.
	  /// <para>
	  /// A curve added with this method cannot be calibrated by the market data system as it does not include
	  /// a curve definition. It is intended to be used with curves which are supplied by the user.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveName">  the name of the curve </param>
	  /// <param name="currency">  the currency for which the curve provides discount rates </param>
	  /// <param name="index">  the index for which the curve provides forward rates </param>
	  /// <param name="otherIndices">  the additional indices for which the curve provides forward rates </param>
	  /// <returns> this builder </returns>
	  public RatesCurveGroupDefinitionBuilder addCurve(CurveName curveName, Currency currency, RateIndex index, params RateIndex[] otherIndices)
	  {

		RatesCurveGroupEntry entry = RatesCurveGroupEntry.builder().curveName(curveName).discountCurrencies(ImmutableSet.of(currency)).indices(indices(index, otherIndices)).build();
		return mergeEntry(entry);
	  }

	  /// <summary>
	  /// Adds a seasonality to the curve group definition.
	  /// </summary>
	  /// <param name="curveName">  the name of the curve </param>
	  /// <param name="seasonalityDefinition">  the seasonality associated to the curve </param>
	  /// <returns> this builder </returns>
	  public RatesCurveGroupDefinitionBuilder addSeasonality(CurveName curveName, SeasonalityDefinition seasonalityDefinition)
	  {

		seasonalityDefinitions[curveName] = seasonalityDefinition;
		return this;
	  }

	  //-------------------------------------------------------------------------
	  // merges the definition and entry
	  private RatesCurveGroupDefinitionBuilder merge(RatesCurveGroupEntry newEntry, CurveDefinition curveDefinition)
	  {
		curveDefinitions[curveDefinition.Name] = curveDefinition;
		return mergeEntry(newEntry);
	  }

	  // merges the specified entry with those already stored
	  private RatesCurveGroupDefinitionBuilder mergeEntry(RatesCurveGroupEntry newEntry)
	  {
		CurveName curveName = newEntry.CurveName;
		RatesCurveGroupEntry existingEntry = entries[curveName];
		RatesCurveGroupEntry entry = existingEntry == null ? newEntry : existingEntry.merge(newEntry);
		entries[curveName] = entry;
		return this;
	  }

	  /// <summary>
	  /// Returns a set containing any Ibor indices in the arguments.
	  /// </summary>
	  private static ISet<Index> indices(Index index, params Index[] otherIndices)
	  {
		// The type parameter is needed for the benefit of the Eclipse compiler
		return ImmutableSet.builder<Index>().add(index).add(otherIndices).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Builds the definition of the curve group from the data in this object.
	  /// </summary>
	  /// <returns> the definition of the curve group built from the data in this object </returns>
	  public RatesCurveGroupDefinition build()
	  {
		// note that this defaults the jacobian flag based on the market quote flag
		return new RatesCurveGroupDefinition(name_Renamed, entries.Values, curveDefinitions.Values, seasonalityDefinitions, computeJacobian_Renamed || computePvSensitivityToMarketQuote_Renamed, computePvSensitivityToMarketQuote_Renamed);
	  }

	}

}