using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableSet;


	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Sets = com.google.common.collect.Sets;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using CalculationParameter = com.opengamma.strata.calc.runner.CalculationParameter;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using Messages = com.opengamma.strata.collect.Messages;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using MarketData = com.opengamma.strata.data.MarketData;
	using NamedMarketDataId = com.opengamma.strata.data.NamedMarketDataId;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using LegalEntityGroup = com.opengamma.strata.market.curve.LegalEntityGroup;
	using RepoGroup = com.opengamma.strata.market.curve.RepoGroup;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;
	using LegalEntityId = com.opengamma.strata.product.LegalEntityId;
	using SecurityId = com.opengamma.strata.product.SecurityId;

	/// <summary>
	/// The legal entity discounting lookup, used to select curves for pricing.
	/// <para>
	/// This provides access to repo and issuer curves.
	/// </para>
	/// <para>
	/// The lookup implements <seealso cref="CalculationParameter"/> and is used by passing it
	/// as an argument to <seealso cref="CalculationRules"/>. It provides the link between the
	/// data that the function needs and the data that is available in <seealso cref="ScenarioMarketData"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") final class DefaultLegalEntityDiscountingMarketDataLookup implements LegalEntityDiscountingMarketDataLookup, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	internal sealed class DefaultLegalEntityDiscountingMarketDataLookup : LegalEntityDiscountingMarketDataLookup, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.product.SecurityId, com.opengamma.strata.market.curve.RepoGroup> repoCurveSecurityGroups;
		private readonly ImmutableMap<SecurityId, RepoGroup> repoCurveSecurityGroups;
	  /// <summary>
	  /// The groups used to find a repo curve by legal entity.
	  /// <para>
	  /// This maps the legal entity ID to a group.
	  /// The group is used to find the curve in {@code repoCurves}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.product.LegalEntityId, com.opengamma.strata.market.curve.RepoGroup> repoCurveGroups;
	  private readonly ImmutableMap<LegalEntityId, RepoGroup> repoCurveGroups;
	  /// <summary>
	  /// The repo curves, keyed by group and currency.
	  /// The curve data, predicting the future, associated with each repo group and currency.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.market.curve.RepoGroup, com.opengamma.strata.basics.currency.Currency>, com.opengamma.strata.market.curve.CurveId> repoCurves;
	  private readonly ImmutableMap<Pair<RepoGroup, Currency>, CurveId> repoCurves;
	  /// <summary>
	  /// The groups used to find an issuer curve by legal entity.
	  /// <para>
	  /// This maps the legal entity ID to a group.
	  /// The group is used to find the curve in {@code issuerCurves}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.product.LegalEntityId, com.opengamma.strata.market.curve.LegalEntityGroup> issuerCurveGroups;
	  private readonly ImmutableMap<LegalEntityId, LegalEntityGroup> issuerCurveGroups;
	  /// <summary>
	  /// The issuer curves, keyed by group and currency.
	  /// The curve data, predicting the future, associated with each legal entity group and currency.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.market.curve.LegalEntityGroup, com.opengamma.strata.basics.currency.Currency>, com.opengamma.strata.market.curve.CurveId> issuerCurves;
	  private readonly ImmutableMap<Pair<LegalEntityGroup, Currency>, CurveId> issuerCurves;
	  /// <summary>
	  /// The source of market data for quotes and other observable market data.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.data.ObservableSource observableSource;
	  private readonly ObservableSource observableSource;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance based on a maps for repo and issuer curves.
	  /// <para>
	  /// Both the repo and issuer curves are defined in two parts.
	  /// The first part maps the issuer ID to a group, and the second part maps the
	  /// group and currency to the identifier of the curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="repoCurveSecurityGroups">  the per security repo curve group overrides, mapping security ID to group </param>
	  /// <param name="repoCurveGroups">  the repo curve groups, mapping issuer ID to group </param>
	  /// <param name="repoCurveIds">  the repo curve identifiers, keyed by security ID or issuer ID and currency </param>
	  /// <param name="issuerCurveGroups">  the issuer curve groups, mapping issuer ID to group </param>
	  /// <param name="issuerCurveIds">  the issuer curves identifiers, keyed by issuer ID and currency </param>
	  /// <param name="obsSource">  the source of market data for quotes and other observable market data </param>
	  /// <returns> the rates lookup containing the specified curves </returns>
	  public static DefaultLegalEntityDiscountingMarketDataLookup of<T>(IDictionary<SecurityId, RepoGroup> repoCurveSecurityGroups, IDictionary<LegalEntityId, RepoGroup> repoCurveGroups, IDictionary<Pair<RepoGroup, Currency>, CurveId> repoCurveIds, IDictionary<LegalEntityId, LegalEntityGroup> issuerCurveGroups, IDictionary<Pair<LegalEntityGroup, Currency>, CurveId> issuerCurveIds, ObservableSource obsSource) where T : com.opengamma.strata.data.NamedMarketDataId<com.opengamma.strata.market.curve.Curve>
	  {

		return new DefaultLegalEntityDiscountingMarketDataLookup(repoCurveSecurityGroups, repoCurveGroups, repoCurveIds, issuerCurveGroups, issuerCurveIds, obsSource);
	  }

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
	  public static DefaultLegalEntityDiscountingMarketDataLookup of(IDictionary<LegalEntityId, RepoGroup> repoCurveGroups, IDictionary<Pair<RepoGroup, Currency>, CurveId> repoCurveIds, ObservableSource obsSource)
	  {

		return new DefaultLegalEntityDiscountingMarketDataLookup(ImmutableMap.of(), repoCurveGroups, repoCurveIds, ImmutableMap.of(), ImmutableMap.of(), obsSource);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ISet<RepoGroup> uniqueRepoGroups = new HashSet<RepoGroup>(repoCurveGroups.values());
		uniqueRepoGroups.addAll(repoCurveSecurityGroups.values());
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ISet<RepoGroup> uniqueRepoCurves = repoCurves.Keys.Select(p => p.First).collect(toImmutableSet());
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the java.util.Collection 'containsAll' method:
		if (!uniqueRepoCurves.containsAll(uniqueRepoGroups))
		{
		  throw new System.ArgumentException("Repo curve groups defined without matching curve mappings: " + Sets.difference(uniqueRepoGroups, uniqueRepoCurves));
		}
		ISet<LegalEntityGroup> uniqueIssuerGroups = new HashSet<LegalEntityGroup>(issuerCurveGroups.values());
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ISet<LegalEntityGroup> uniqueIssuerCurves = issuerCurves.Keys.Select(p => p.First).collect(toImmutableSet());
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the java.util.Collection 'containsAll' method:
		if (!uniqueIssuerCurves.containsAll(uniqueIssuerGroups))
		{
		  throw new System.ArgumentException("Issuer curve groups defined without matching curve mappings: " + Sets.difference(uniqueIssuerGroups, uniqueIssuerCurves));
		}
	  }

	  //-------------------------------------------------------------------------
	  public FunctionRequirements requirements(SecurityId securityId, LegalEntityId issuerId, Currency currency)
	  {
		// repo
		RepoGroup repoKey = repoCurveSecurityGroups.get(securityId);
		if (repoKey == null)
		{
		  repoKey = repoCurveGroups.get(issuerId);
		}
		if (repoKey == null)
		{
		  throw new System.ArgumentException(Messages.format("Legal entity discounting lookup has no repo curve defined for '{}' and '{}'", securityId, issuerId));
		}
		CurveId repoCurveId = repoCurves.get(Pair.of(repoKey, currency));
		if (repoCurveId == null)
		{
		  throw new System.ArgumentException(Messages.format("Legal entity discounting lookup has no repo curve defined for '{}' and '{}'", securityId, issuerId));
		}
		// issuer
		LegalEntityGroup issuerKey = issuerCurveGroups.get(issuerId);
		if (issuerKey == null)
		{
		  throw new System.ArgumentException(Messages.format("Legal entity discounting lookup has no issuer curve defined for '{}'", issuerId));
		}
		CurveId issuerCurveId = issuerCurves.get(Pair.of(issuerKey, currency));
		if (issuerCurveId == null)
		{
		  throw new System.ArgumentException(Messages.format("Legal entity discounting lookup has no issuer curve defined for '{}'", issuerId));
		}
		// result
		return FunctionRequirements.builder().valueRequirements(ImmutableSet.of(repoCurveId, issuerCurveId)).outputCurrencies(currency).observableSource(observableSource).build();
	  }

	  public FunctionRequirements requirements(LegalEntityId issuerId, Currency currency)
	  {
		// repo
		RepoGroup repoKey = repoCurveGroups.get(issuerId);
		if (repoKey == null)
		{
		  throw new System.ArgumentException(Messages.format("Legal entity discounting lookup has no repo curve defined for '{}'", issuerId));
		}
		CurveId repoCurveId = repoCurves.get(Pair.of(repoKey, currency));
		if (repoCurveId == null)
		{
		  throw new System.ArgumentException(Messages.format("Legal entity discounting lookup has no repo curve defined for '{}'", issuerId));
		}
		// result
		return FunctionRequirements.builder().valueRequirements(ImmutableSet.of(repoCurveId)).outputCurrencies(currency).observableSource(observableSource).build();
	  }

	  //-------------------------------------------------------------------------
	  public LegalEntityDiscountingProvider discountingProvider(MarketData marketData)
	  {
		return DefaultLookupLegalEntityDiscountingProvider.of(this, marketData);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DefaultLegalEntityDiscountingMarketDataLookup}.
	  /// </summary>
	  private static readonly TypedMetaBean<DefaultLegalEntityDiscountingMarketDataLookup> META_BEAN = LightMetaBean.of(typeof(DefaultLegalEntityDiscountingMarketDataLookup), MethodHandles.lookup(), new string[] {"repoCurveSecurityGroups", "repoCurveGroups", "repoCurves", "issuerCurveGroups", "issuerCurves", "observableSource"}, ImmutableMap.of(), ImmutableMap.of(), ImmutableMap.of(), ImmutableMap.of(), ImmutableMap.of(), null);

	  /// <summary>
	  /// The meta-bean for {@code DefaultLegalEntityDiscountingMarketDataLookup}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<DefaultLegalEntityDiscountingMarketDataLookup> meta()
	  {
		return META_BEAN;
	  }

	  static DefaultLegalEntityDiscountingMarketDataLookup()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private DefaultLegalEntityDiscountingMarketDataLookup(IDictionary<SecurityId, RepoGroup> repoCurveSecurityGroups, IDictionary<LegalEntityId, RepoGroup> repoCurveGroups, IDictionary<Pair<RepoGroup, Currency>, CurveId> repoCurves, IDictionary<LegalEntityId, LegalEntityGroup> issuerCurveGroups, IDictionary<Pair<LegalEntityGroup, Currency>, CurveId> issuerCurves, ObservableSource observableSource)
	  {
		JodaBeanUtils.notNull(repoCurveSecurityGroups, "repoCurveSecurityGroups");
		JodaBeanUtils.notNull(repoCurveGroups, "repoCurveGroups");
		JodaBeanUtils.notNull(repoCurves, "repoCurves");
		JodaBeanUtils.notNull(issuerCurveGroups, "issuerCurveGroups");
		JodaBeanUtils.notNull(issuerCurves, "issuerCurves");
		JodaBeanUtils.notNull(observableSource, "observableSource");
		this.repoCurveSecurityGroups = ImmutableMap.copyOf(repoCurveSecurityGroups);
		this.repoCurveGroups = ImmutableMap.copyOf(repoCurveGroups);
		this.repoCurves = ImmutableMap.copyOf(repoCurves);
		this.issuerCurveGroups = ImmutableMap.copyOf(issuerCurveGroups);
		this.issuerCurves = ImmutableMap.copyOf(issuerCurves);
		this.observableSource = observableSource;
		validate();
	  }

	  public override TypedMetaBean<DefaultLegalEntityDiscountingMarketDataLookup> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the groups used to find a repo curve by security.
	  /// <para>
	  /// This maps the security ID to a group.
	  /// The group is used to find the curve in {@code repoCurves}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<SecurityId, RepoGroup> RepoCurveSecurityGroups
	  {
		  get
		  {
			return repoCurveSecurityGroups;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the groups used to find a repo curve by legal entity.
	  /// <para>
	  /// This maps the legal entity ID to a group.
	  /// The group is used to find the curve in {@code repoCurves}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<LegalEntityId, RepoGroup> RepoCurveGroups
	  {
		  get
		  {
			return repoCurveGroups;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the repo curves, keyed by group and currency.
	  /// The curve data, predicting the future, associated with each repo group and currency. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<Pair<RepoGroup, Currency>, CurveId> RepoCurves
	  {
		  get
		  {
			return repoCurves;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the groups used to find an issuer curve by legal entity.
	  /// <para>
	  /// This maps the legal entity ID to a group.
	  /// The group is used to find the curve in {@code issuerCurves}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<LegalEntityId, LegalEntityGroup> IssuerCurveGroups
	  {
		  get
		  {
			return issuerCurveGroups;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the issuer curves, keyed by group and currency.
	  /// The curve data, predicting the future, associated with each legal entity group and currency. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<Pair<LegalEntityGroup, Currency>, CurveId> IssuerCurves
	  {
		  get
		  {
			return issuerCurves;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the source of market data for quotes and other observable market data. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ObservableSource ObservableSource
	  {
		  get
		  {
			return observableSource;
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
		  DefaultLegalEntityDiscountingMarketDataLookup other = (DefaultLegalEntityDiscountingMarketDataLookup) obj;
		  return JodaBeanUtils.equal(repoCurveSecurityGroups, other.repoCurveSecurityGroups) && JodaBeanUtils.equal(repoCurveGroups, other.repoCurveGroups) && JodaBeanUtils.equal(repoCurves, other.repoCurves) && JodaBeanUtils.equal(issuerCurveGroups, other.issuerCurveGroups) && JodaBeanUtils.equal(issuerCurves, other.issuerCurves) && JodaBeanUtils.equal(observableSource, other.observableSource);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(repoCurveSecurityGroups);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(repoCurveGroups);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(repoCurves);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(issuerCurveGroups);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(issuerCurves);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(observableSource);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(224);
		buf.Append("DefaultLegalEntityDiscountingMarketDataLookup{");
		buf.Append("repoCurveSecurityGroups").Append('=').Append(repoCurveSecurityGroups).Append(',').Append(' ');
		buf.Append("repoCurveGroups").Append('=').Append(repoCurveGroups).Append(',').Append(' ');
		buf.Append("repoCurves").Append('=').Append(repoCurves).Append(',').Append(' ');
		buf.Append("issuerCurveGroups").Append('=').Append(issuerCurveGroups).Append(',').Append(' ');
		buf.Append("issuerCurves").Append('=').Append(issuerCurves).Append(',').Append(' ');
		buf.Append("observableSource").Append('=').Append(JodaBeanUtils.ToString(observableSource));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}