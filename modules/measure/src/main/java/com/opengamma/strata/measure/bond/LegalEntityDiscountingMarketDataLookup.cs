/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.bond
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using CalculationParameter = com.opengamma.strata.calc.runner.CalculationParameter;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using MapStream = com.opengamma.strata.collect.MapStream;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using LegalEntityCurveGroup = com.opengamma.strata.market.curve.LegalEntityCurveGroup;
	using LegalEntityGroup = com.opengamma.strata.market.curve.LegalEntityGroup;
	using RepoGroup = com.opengamma.strata.market.curve.RepoGroup;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;
	using LegalEntityId = com.opengamma.strata.product.LegalEntityId;
	using SecurityId = com.opengamma.strata.product.SecurityId;

	/// <summary>
	/// The lookup that provides access to legal entity discounting in market data.
	/// <para>
	/// The legal entity discounting market lookup provides access to repo and issuer curves.
	/// </para>
	/// <para>
	/// The lookup implements <seealso cref="CalculationParameter"/> and is used by passing it
	/// as an argument to <seealso cref="CalculationRules"/>. It provides the link between the
	/// data that the function needs and the data that is available in <seealso cref="ScenarioMarketData"/>.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable.
	/// </para>
	/// </summary>
	public interface LegalEntityDiscountingMarketDataLookup : CalculationParameter
	{

	  /// <summary>
	  /// Obtains an instance based on a maps for repo and issuer curves.
	  /// <para>
	  /// Both the repo and issuer curves are defined in two parts.
	  /// The first part maps the issuer ID to a group, and the second part maps the
	  /// group and currency to the identifier of the curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="repoCurveSecurityGroups">  the per security repo curve groups overrides, mapping security ID to group </param>
	  /// <param name="repoCurveGroups">  the repo curve groups, mapping issuer ID to group </param>
	  /// <param name="repoCurveIds">  the repo curve identifiers </param>
	  /// <param name="issuerCurveGroups">  the issuer curve groups, mapping issuer ID to group </param>
	  /// <param name="issuerCurveIds">  the issuer curves identifiers, keyed by issuer ID and currency </param>
	  /// <returns> the rates lookup containing the specified curves </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static LegalEntityDiscountingMarketDataLookup of(java.util.Map<com.opengamma.strata.product.SecurityId, com.opengamma.strata.market.curve.RepoGroup> repoCurveSecurityGroups, java.util.Map<com.opengamma.strata.product.LegalEntityId, com.opengamma.strata.market.curve.RepoGroup> repoCurveGroups, java.util.Map<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.market.curve.RepoGroup, com.opengamma.strata.basics.currency.Currency>, com.opengamma.strata.market.curve.CurveId> repoCurveIds, java.util.Map<com.opengamma.strata.product.LegalEntityId, com.opengamma.strata.market.curve.LegalEntityGroup> issuerCurveGroups, java.util.Map<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.market.curve.LegalEntityGroup, com.opengamma.strata.basics.currency.Currency>, com.opengamma.strata.market.curve.CurveId> issuerCurveIds)
	//  {
	//
	//	return DefaultLegalEntityDiscountingMarketDataLookup.of(repoCurveSecurityGroups, repoCurveGroups, repoCurveIds, issuerCurveGroups, issuerCurveIds, ObservableSource.NONE);
	//  }

	  /// <summary>
	  /// Obtains an instance based on a maps for repo and issuer curves.
	  /// <para>
	  /// Both the repo and issuer curves are defined in two parts.
	  /// The first part maps the issuer ID to a group, and the second part maps the
	  /// group and currency to the identifier of the curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="repoCurveSecurityGroups">  the per security repo curve groups overrides, mapping security ID to group </param>
	  /// <param name="repoCurveGroups">  the repo curve groups, mapping issuer ID to group </param>
	  /// <param name="repoCurveIds">  the repo curve identifiers </param>
	  /// <param name="issuerCurveGroups">  the issuer curve groups, mapping issuer ID to group </param>
	  /// <param name="issuerCurveIds">  the issuer curves identifiers, keyed by issuer ID and currency </param>
	  /// <param name="obsSource">  the source of market data for quotes and other observable market data </param>
	  /// <returns> the rates lookup containing the specified curves </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static LegalEntityDiscountingMarketDataLookup of(java.util.Map<com.opengamma.strata.product.SecurityId, com.opengamma.strata.market.curve.RepoGroup> repoCurveSecurityGroups, java.util.Map<com.opengamma.strata.product.LegalEntityId, com.opengamma.strata.market.curve.RepoGroup> repoCurveGroups, java.util.Map<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.market.curve.RepoGroup, com.opengamma.strata.basics.currency.Currency>, com.opengamma.strata.market.curve.CurveId> repoCurveIds, java.util.Map<com.opengamma.strata.product.LegalEntityId, com.opengamma.strata.market.curve.LegalEntityGroup> issuerCurveGroups, java.util.Map<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.market.curve.LegalEntityGroup, com.opengamma.strata.basics.currency.Currency>, com.opengamma.strata.market.curve.CurveId> issuerCurveIds, com.opengamma.strata.data.ObservableSource obsSource)
	//  {
	//
	//	return DefaultLegalEntityDiscountingMarketDataLookup.of(repoCurveSecurityGroups, repoCurveGroups, repoCurveIds, issuerCurveGroups, issuerCurveIds, obsSource);
	//  }

	  /// <summary>
	  /// Obtains an instance based on a maps for repo and issuer curves.
	  /// <para>
	  /// Both the repo and issuer curves are defined in two parts.
	  /// The first part maps the issuer ID to a group, and the second part maps the
	  /// group and currency to the identifier of the curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="repoCurveGroups">  the repo curve groups, mapping issuer ID to group </param>
	  /// <param name="repoCurveIds">  the repo curve identifiers </param>
	  /// <param name="issuerCurveGroups">  the issuer curve groups, mapping issuer ID to group </param>
	  /// <param name="issuerCurveIds">  the issuer curves identifiers, keyed by issuer ID and currency </param>
	  /// <returns> the rates lookup containing the specified curves </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static LegalEntityDiscountingMarketDataLookup of(java.util.Map<com.opengamma.strata.product.LegalEntityId, com.opengamma.strata.market.curve.RepoGroup> repoCurveGroups, java.util.Map<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.market.curve.RepoGroup, com.opengamma.strata.basics.currency.Currency>, com.opengamma.strata.market.curve.CurveId> repoCurveIds, java.util.Map<com.opengamma.strata.product.LegalEntityId, com.opengamma.strata.market.curve.LegalEntityGroup> issuerCurveGroups, java.util.Map<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.market.curve.LegalEntityGroup, com.opengamma.strata.basics.currency.Currency>, com.opengamma.strata.market.curve.CurveId> issuerCurveIds)
	//  {
	//
	//	return DefaultLegalEntityDiscountingMarketDataLookup.of(ImmutableMap.of(), repoCurveGroups, repoCurveIds, issuerCurveGroups, issuerCurveIds, ObservableSource.NONE);
	//  }

	  /// <summary>
	  /// Obtains an instance based on a maps for repo and issuer curves.
	  /// <para>
	  /// Both the repo and issuer curves are defined in two parts.
	  /// The first part maps the issuer ID to a group, and the second part maps the
	  /// group and currency to the identifier of the curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="repoCurveGroups">  the repo curve groups, mapping issuer ID to group </param>
	  /// <param name="repoCurveIds">  the repo curve identifiers </param>
	  /// <param name="issuerCurveGroups">  the issuer curve groups, mapping issuer ID to group </param>
	  /// <param name="issuerCurveIds">  the issuer curves identifiers, keyed by issuer ID and currency </param>
	  /// <param name="obsSource">  the source of market data for quotes and other observable market data </param>
	  /// <returns> the rates lookup containing the specified curves </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static LegalEntityDiscountingMarketDataLookup of(java.util.Map<com.opengamma.strata.product.LegalEntityId, com.opengamma.strata.market.curve.RepoGroup> repoCurveGroups, java.util.Map<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.market.curve.RepoGroup, com.opengamma.strata.basics.currency.Currency>, com.opengamma.strata.market.curve.CurveId> repoCurveIds, java.util.Map<com.opengamma.strata.product.LegalEntityId, com.opengamma.strata.market.curve.LegalEntityGroup> issuerCurveGroups, java.util.Map<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.market.curve.LegalEntityGroup, com.opengamma.strata.basics.currency.Currency>, com.opengamma.strata.market.curve.CurveId> issuerCurveIds, com.opengamma.strata.data.ObservableSource obsSource)
	//  {
	//
	//	return DefaultLegalEntityDiscountingMarketDataLookup.of(ImmutableMap.of(), repoCurveGroups, repoCurveIds, issuerCurveGroups, issuerCurveIds, obsSource);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance based on a curve group and group maps.
	  /// <para>
	  /// The two maps define mapping from the issuer ID to a group.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveGroup">  the curve group to base the lookup on </param>
	  /// <param name="repoCurveSecurityGroups">  the per security repo curve groups overrides, mapping security ID to group </param>
	  /// <param name="repoCurveGroups">  the repo curve groups, mapping issuer ID to group </param>
	  /// <param name="issuerCurveGroups">  the issuer curve groups, mapping issuer ID to group </param>
	  /// <returns> the rates lookup containing the specified curves  </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static LegalEntityDiscountingMarketDataLookup of(com.opengamma.strata.market.curve.LegalEntityCurveGroup curveGroup, java.util.Map<com.opengamma.strata.product.SecurityId, com.opengamma.strata.market.curve.RepoGroup> repoCurveSecurityGroups, java.util.Map<com.opengamma.strata.product.LegalEntityId, com.opengamma.strata.market.curve.RepoGroup> repoCurveGroups, java.util.Map<com.opengamma.strata.product.LegalEntityId, com.opengamma.strata.market.curve.LegalEntityGroup> issuerCurveGroups)
	//  {
	//
	//	CurveGroupName groupName = curveGroup.getName();
	//	Map<Pair<RepoGroup, Currency>, CurveId> repoCurveIds = MapStream.of(curveGroup.getRepoCurves()).mapValues(c -> CurveId.of(groupName, c.getName())).toMap();
	//	Map<Pair<LegalEntityGroup, Currency>, CurveId> issuerCurveIds = MapStream.of(curveGroup.getIssuerCurves()).mapValues(c -> CurveId.of(groupName, c.getName())).toMap();
	//	return DefaultLegalEntityDiscountingMarketDataLookup.of(repoCurveSecurityGroups, repoCurveGroups, repoCurveIds, issuerCurveGroups, issuerCurveIds, ObservableSource.NONE);
	//  }

	  /// <summary>
	  /// Obtains an instance based on a curve group and group maps.
	  /// <para>
	  /// The two maps define mapping from the issuer ID to a group.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveGroup">  the curve group to base the lookup on </param>
	  /// <param name="repoCurveGroups">  the repo curve groups, mapping issuer ID to group </param>
	  /// <param name="issuerCurveGroups">  the issuer curve groups, mapping issuer ID to group </param>
	  /// <returns> the rates lookup containing the specified curves  </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static LegalEntityDiscountingMarketDataLookup of(com.opengamma.strata.market.curve.LegalEntityCurveGroup curveGroup, java.util.Map<com.opengamma.strata.product.LegalEntityId, com.opengamma.strata.market.curve.RepoGroup> repoCurveGroups, java.util.Map<com.opengamma.strata.product.LegalEntityId, com.opengamma.strata.market.curve.LegalEntityGroup> issuerCurveGroups)
	//  {
	//
	//	return of(curveGroup, ImmutableMap.of(), repoCurveGroups, issuerCurveGroups);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance based on maps for repo curves.
	  /// <para>
	  /// The repo curves are defined in two parts.
	  /// The first part maps the issuer ID to a group, and the second part maps the
	  /// group and currency to the identifier of the curve.
	  /// </para>
	  /// <para>
	  /// Issuer curves are not defined in the instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="repoCurveGroups">  the repo curve groups, mapping issuer ID to group </param>
	  /// <param name="repoCurveIds">  the repo curve identifiers, keyed by repo group and currency </param>
	  /// <returns> the rates lookup containing the specified curves </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static LegalEntityDiscountingMarketDataLookup of(java.util.Map<com.opengamma.strata.product.LegalEntityId, com.opengamma.strata.market.curve.RepoGroup> repoCurveGroups, java.util.Map<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.market.curve.RepoGroup, com.opengamma.strata.basics.currency.Currency>, com.opengamma.strata.market.curve.CurveId> repoCurveIds)
	//  {
	//
	//	return LegalEntityDiscountingMarketDataLookup.of(repoCurveGroups, repoCurveIds, ObservableSource.NONE);
	//  }

	  /// <summary>
	  /// Obtains an instance based on maps for repo curves.
	  /// <para>
	  /// The repo curves are defined in two parts.
	  /// The first part maps the issuer ID to a group, and the second part maps the
	  /// group and currency to the identifier of the curve.
	  /// </para>
	  /// <para>
	  /// Issuer curves are not defined in the instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="repoCurveGroups">  the repo curve groups, mapping issuer ID to group </param>
	  /// <param name="repoCurveIds">  the repo curve identifiers, keyed by repo group and currency </param>
	  /// <param name="obsSource">  the source of market data for quotes and other observable market data </param>
	  /// <returns> the rates lookup containing the specified curves </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static LegalEntityDiscountingMarketDataLookup of(java.util.Map<com.opengamma.strata.product.LegalEntityId, com.opengamma.strata.market.curve.RepoGroup> repoCurveGroups, java.util.Map<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.market.curve.RepoGroup, com.opengamma.strata.basics.currency.Currency>, com.opengamma.strata.market.curve.CurveId> repoCurveIds, com.opengamma.strata.data.ObservableSource obsSource)
	//  {
	//
	//	return DefaultLegalEntityDiscountingMarketDataLookup.of(repoCurveGroups, repoCurveIds, obsSource);
	//  }

	  /// <summary>
	  /// Obtains an instance based on a curve group and group map.
	  /// <para>
	  /// The two maps define mapping from the issuer ID to a group.
	  /// </para>
	  /// <para>
	  /// Issuer curves are not defined in the instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveGroup">  the curve group to base the lookup on </param>
	  /// <param name="repoCurveGroups">  the repo curve groups, mapping issuer ID to group </param>
	  /// <returns> the rates lookup containing the specified curves  </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static LegalEntityDiscountingMarketDataLookup of(com.opengamma.strata.market.curve.LegalEntityCurveGroup curveGroup, java.util.Map<com.opengamma.strata.product.LegalEntityId, com.opengamma.strata.market.curve.RepoGroup> repoCurveGroups)
	//  {
	//
	//	CurveGroupName groupName = curveGroup.getName();
	//	Map<Pair<RepoGroup, Currency>, CurveId> repoCurveIds = MapStream.of(curveGroup.getRepoCurves()).mapValues(c -> CurveId.of(groupName, c.getName())).toMap();
	//	return LegalEntityDiscountingMarketDataLookup.of(repoCurveGroups, repoCurveIds, ObservableSource.NONE);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the type that the lookup will be queried by.
	  /// <para>
	  /// This returns {@code LegalEntityDiscountingMarketDataLookup.class}.
	  /// When querying parameters using <seealso cref="CalculationParameters#findParameter(Class)"/>,
	  /// {@code LegalEntityDiscountingMarketDataLookup.class} must be passed in to find the instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the type of the parameter implementation </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default Class queryType()
	//  {
	//	return LegalEntityDiscountingMarketDataLookup.class;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates market data requirements for the specified security and issuer.
	  /// </summary>
	  /// <param name="securityId">  the security ID </param>
	  /// <param name="issuerId">  the legal entity issuer ID </param>
	  /// <param name="currency">  the currency of the security </param>
	  /// <returns> the requirements </returns>
	  /// <exception cref="IllegalArgumentException"> if unable to create requirements </exception>
	  FunctionRequirements requirements(SecurityId securityId, LegalEntityId issuerId, Currency currency);

	  /// <summary>
	  /// Creates market data requirements for the specified issuer.
	  /// </summary>
	  /// <param name="issuerId">  the legal entity issuer ID </param>
	  /// <param name="currency">  the currency of the security </param>
	  /// <returns> the requirements </returns>
	  /// <exception cref="IllegalArgumentException"> if unable to create requirements </exception>
	  FunctionRequirements requirements(LegalEntityId issuerId, Currency currency);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a filtered view of the complete set of market data.
	  /// <para>
	  /// This method returns an instance that binds the lookup to the market data.
	  /// The input is <seealso cref="ScenarioMarketData"/>, which contains market data for all scenarios.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketData">  the complete set of market data for all scenarios </param>
	  /// <returns> the filtered market data </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default LegalEntityDiscountingScenarioMarketData marketDataView(com.opengamma.strata.data.scenario.ScenarioMarketData marketData)
	//  {
	//	return DefaultLegalEntityDiscountingScenarioMarketData.of(this, marketData);
	//  }

	  /// <summary>
	  /// Obtains a filtered view of the complete set of market data.
	  /// <para>
	  /// This method returns an instance that binds the lookup to the market data.
	  /// The input is <seealso cref="MarketData"/>, which contains market data for one scenario.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketData">  the complete set of market data for one scenario </param>
	  /// <returns> the filtered market data </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default LegalEntityDiscountingMarketData marketDataView(com.opengamma.strata.data.MarketData marketData)
	//  {
	//	return DefaultLegalEntityDiscountingMarketData.of(this, marketData);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a discounting provider based on the specified market data.
	  /// <para>
	  /// This provides a <seealso cref="LegalEntityDiscountingProvider"/> suitable for pricing a product.
	  /// Although this method can be used directly, it is typically invoked indirectly
	  /// via <seealso cref="LegalEntityDiscountingMarketData"/>:
	  /// <pre>
	  ///  // bind the baseData to this lookup
	  ///  LegalEntityDiscountingMarketData view = lookup.marketView(baseData);
	  /// 
	  ///  // pass around RatesMarketData within the function to use in pricing
	  ///  LegalEntityDiscountingProvider provider = view.discountingProvider();
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketData">  the complete set of market data for one scenario </param>
	  /// <returns> the discounting provider </returns>
	  LegalEntityDiscountingProvider discountingProvider(MarketData marketData);

	}

}