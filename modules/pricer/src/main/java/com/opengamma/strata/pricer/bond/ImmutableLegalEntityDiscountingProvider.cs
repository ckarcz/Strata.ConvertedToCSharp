using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using LegalEntityGroup = com.opengamma.strata.market.curve.LegalEntityGroup;
	using RepoGroup = com.opengamma.strata.market.curve.RepoGroup;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using LegalEntityId = com.opengamma.strata.product.LegalEntityId;
	using SecurityId = com.opengamma.strata.product.SecurityId;

	/// <summary>
	/// An immutable provider of data for bond pricing, based on repo and issuer discounting.
	/// <para>
	/// This used to price bonds issued by a legal entity.
	/// The data to do this includes discount factors of repo curves and issuer curves.
	/// If the bond is inflation linked, the price index data is obtained from <seealso cref="RatesProvider"/>.
	/// </para>
	/// <para>
	/// Two types of discount factors are provided by this class.
	/// Repo curves are looked up using either the security ID of the bond, or the issuer (legal entity).
	/// Issuer curves are only looked up using the issuer (legal entity).
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ImmutableLegalEntityDiscountingProvider implements LegalEntityDiscountingProvider, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ImmutableLegalEntityDiscountingProvider : LegalEntityDiscountingProvider, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.LocalDate valuationDate;
		private readonly LocalDate valuationDate;
	  /// <summary>
	  /// The groups used to find a repo curve by security.
	  /// <para>
	  /// This maps the security ID to a group.
	  /// The group is used to find the curve in {@code repoCurves}.
	  /// </para>
	  /// </summary>
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
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.market.curve.RepoGroup, com.opengamma.strata.basics.currency.Currency>, com.opengamma.strata.pricer.DiscountFactors> repoCurves;
	  private readonly ImmutableMap<Pair<RepoGroup, Currency>, DiscountFactors> repoCurves;
	  /// <summary>
	  /// The groups used to find an issuer curve by legal entity.
	  /// <para>
	  /// This maps the legal entity ID to a group.
	  /// The group is used to find the curve in {@code issuerCurves}.
	  /// </para>
	  /// <para>
	  /// This property was renamed in version 1.1 of Strata from {@code legalEntityMap}.
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
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.market.curve.LegalEntityGroup, com.opengamma.strata.basics.currency.Currency>, com.opengamma.strata.pricer.DiscountFactors> issuerCurves;
	  private readonly ImmutableMap<Pair<LegalEntityGroup, Currency>, DiscountFactors> issuerCurves;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (builder.valuationDate_Renamed == null && builder.issuerCurves_Renamed.Count > 0)
		{
		  builder.valuationDate_Renamed = builder.issuerCurves_Renamed.Values.GetEnumerator().next().ValuationDate;
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		foreach (KeyValuePair<Pair<RepoGroup, Currency>, DiscountFactors> entry in repoCurves.entrySet())
		{
		  if (!entry.Value.ValuationDate.isEqual(valuationDate))
		  {
			throw new System.ArgumentException("Invalid valuation date for the repo curve: " + entry.Value);
		  }
		  RepoGroup group = entry.Key.First;
		  if (!repoCurveGroups.containsValue(group) && !repoCurveSecurityGroups.containsValue(group))
		  {
			throw new System.ArgumentException("No map to the repo group from ID: " + group);
		  }
		}
		foreach (KeyValuePair<Pair<LegalEntityGroup, Currency>, DiscountFactors> entry in issuerCurves.entrySet())
		{
		  if (!entry.Value.ValuationDate.isEqual(valuationDate))
		  {
			throw new System.ArgumentException("Invalid valuation date for the issuer curve: " + entry.Value);
		  }
		  if (!issuerCurveGroups.containsValue(entry.Key.First))
		  {
			throw new System.ArgumentException("No map to the legal entity group from ID: " + entry.Key.First);
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public RepoCurveDiscountFactors repoCurveDiscountFactors(SecurityId securityId, LegalEntityId issuerId, Currency currency)
	  {
		RepoGroup repoGroup = repoCurveSecurityGroups.get(securityId);
		if (repoGroup == null)
		{
		  repoGroup = repoCurveGroups.get(issuerId);
		  if (repoGroup == null)
		  {
			throw new System.ArgumentException("Unable to find map for ID: " + securityId + ", " + issuerId);
		  }
		}
		return repoCurveDiscountFactors(repoGroup, currency);
	  }

	  public RepoCurveDiscountFactors repoCurveDiscountFactors(LegalEntityId issuerId, Currency currency)
	  {
		RepoGroup repoGroup = repoCurveGroups.get(issuerId);
		if (repoGroup == null)
		{
		  throw new System.ArgumentException("Unable to find map for ID: " + issuerId);
		}
		return repoCurveDiscountFactors(repoGroup, currency);
	  }

	  // lookup the discount factors for the repo group
	  private RepoCurveDiscountFactors repoCurveDiscountFactors(RepoGroup repoGroup, Currency currency)
	  {
		DiscountFactors discountFactors = repoCurves.get(Pair.of(repoGroup, currency));
		if (discountFactors == null)
		{
		  throw new System.ArgumentException("Unable to find repo curve: " + repoGroup + ", " + currency);
		}
		return RepoCurveDiscountFactors.of(discountFactors, repoGroup);
	  }

	  //-------------------------------------------------------------------------
	  public IssuerCurveDiscountFactors issuerCurveDiscountFactors(LegalEntityId issuerId, Currency currency)
	  {
		LegalEntityGroup legalEntityGroup = issuerCurveGroups.get(issuerId);
		if (legalEntityGroup == null)
		{
		  throw new System.ArgumentException("Unable to find map for ID: " + issuerId);
		}
		return issuerCurveDiscountFactors(legalEntityGroup, currency);
	  }

	  // lookup the discount factors for the legal entity group
	  private IssuerCurveDiscountFactors issuerCurveDiscountFactors(LegalEntityGroup legalEntityGroup, Currency currency)
	  {
		DiscountFactors discountFactors = issuerCurves.get(Pair.of(legalEntityGroup, currency));
		if (discountFactors == null)
		{
		  throw new System.ArgumentException("Unable to find issuer curve: " + legalEntityGroup + ", " + currency);
		}
		return IssuerCurveDiscountFactors.of(discountFactors, legalEntityGroup);
	  }

	  //-------------------------------------------------------------------------
	  public CurrencyParameterSensitivities parameterSensitivity(PointSensitivities pointSensitivities)
	  {
		CurrencyParameterSensitivities sens = CurrencyParameterSensitivities.empty();
		foreach (PointSensitivity point in pointSensitivities.Sensitivities)
		{
		  if (point is RepoCurveZeroRateSensitivity)
		  {
			RepoCurveZeroRateSensitivity pt = (RepoCurveZeroRateSensitivity) point;
			RepoCurveDiscountFactors factors = repoCurveDiscountFactors(pt.RepoGroup, pt.CurveCurrency);
			sens = sens.combinedWith(factors.parameterSensitivity(pt));
		  }
		  else if (point is IssuerCurveZeroRateSensitivity)
		  {
			IssuerCurveZeroRateSensitivity pt = (IssuerCurveZeroRateSensitivity) point;
			IssuerCurveDiscountFactors factors = issuerCurveDiscountFactors(pt.LegalEntityGroup, pt.CurveCurrency);
			sens = sens.combinedWith(factors.parameterSensitivity(pt));
		  }
		}
		return sens;
	  }

	  //-------------------------------------------------------------------------
	  public T data<T>(MarketDataId<T> id)
	  {
		throw new System.ArgumentException("Unknown identifier: " + id.ToString());
	  }

	  public Optional<T> findData<T>(MarketDataName<T> name)
	  {
		if (name is CurveName)
		{
		  return Stream.concat(repoCurves.values().stream(), issuerCurves.values().stream()).map(df => df.findData(name)).filter(opt => opt.Present).map(opt => opt.get()).findFirst();
		}
		return null;
	  }

	  public ImmutableLegalEntityDiscountingProvider toImmutableLegalEntityDiscountingProvider()
	  {
		return this;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableLegalEntityDiscountingProvider}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ImmutableLegalEntityDiscountingProvider.Meta meta()
	  {
		return ImmutableLegalEntityDiscountingProvider.Meta.INSTANCE;
	  }

	  static ImmutableLegalEntityDiscountingProvider()
	  {
		MetaBean.register(ImmutableLegalEntityDiscountingProvider.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ImmutableLegalEntityDiscountingProvider.Builder builder()
	  {
		return new ImmutableLegalEntityDiscountingProvider.Builder();
	  }

	  private ImmutableLegalEntityDiscountingProvider(LocalDate valuationDate, IDictionary<SecurityId, RepoGroup> repoCurveSecurityGroups, IDictionary<LegalEntityId, RepoGroup> repoCurveGroups, IDictionary<Pair<RepoGroup, Currency>, DiscountFactors> repoCurves, IDictionary<LegalEntityId, LegalEntityGroup> issuerCurveGroups, IDictionary<Pair<LegalEntityGroup, Currency>, DiscountFactors> issuerCurves)
	  {
		JodaBeanUtils.notNull(valuationDate, "valuationDate");
		JodaBeanUtils.notNull(repoCurveSecurityGroups, "repoCurveSecurityGroups");
		JodaBeanUtils.notNull(repoCurveGroups, "repoCurveGroups");
		JodaBeanUtils.notNull(repoCurves, "repoCurves");
		JodaBeanUtils.notNull(issuerCurveGroups, "issuerCurveGroups");
		JodaBeanUtils.notNull(issuerCurves, "issuerCurves");
		this.valuationDate = valuationDate;
		this.repoCurveSecurityGroups = ImmutableMap.copyOf(repoCurveSecurityGroups);
		this.repoCurveGroups = ImmutableMap.copyOf(repoCurveGroups);
		this.repoCurves = ImmutableMap.copyOf(repoCurves);
		this.issuerCurveGroups = ImmutableMap.copyOf(issuerCurveGroups);
		this.issuerCurves = ImmutableMap.copyOf(issuerCurves);
		validate();
	  }

	  public override ImmutableLegalEntityDiscountingProvider.Meta metaBean()
	  {
		return ImmutableLegalEntityDiscountingProvider.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the valuation date.
	  /// All curves and other data items in this provider are calibrated for this date. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate ValuationDate
	  {
		  get
		  {
			return valuationDate;
		  }
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
	  public ImmutableMap<Pair<RepoGroup, Currency>, DiscountFactors> RepoCurves
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
	  /// <para>
	  /// This property was renamed in version 1.1 of Strata from {@code legalEntityMap}.
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
	  public ImmutableMap<Pair<LegalEntityGroup, Currency>, DiscountFactors> IssuerCurves
	  {
		  get
		  {
			return issuerCurves;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Returns a builder that allows this bean to be mutated. </summary>
	  /// <returns> the mutable builder, not null </returns>
	  public Builder toBuilder()
	  {
		return new Builder(this);
	  }

	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  ImmutableLegalEntityDiscountingProvider other = (ImmutableLegalEntityDiscountingProvider) obj;
		  return JodaBeanUtils.equal(valuationDate, other.valuationDate) && JodaBeanUtils.equal(repoCurveSecurityGroups, other.repoCurveSecurityGroups) && JodaBeanUtils.equal(repoCurveGroups, other.repoCurveGroups) && JodaBeanUtils.equal(repoCurves, other.repoCurves) && JodaBeanUtils.equal(issuerCurveGroups, other.issuerCurveGroups) && JodaBeanUtils.equal(issuerCurves, other.issuerCurves);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(valuationDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(repoCurveSecurityGroups);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(repoCurveGroups);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(repoCurves);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(issuerCurveGroups);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(issuerCurves);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(224);
		buf.Append("ImmutableLegalEntityDiscountingProvider{");
		buf.Append("valuationDate").Append('=').Append(valuationDate).Append(',').Append(' ');
		buf.Append("repoCurveSecurityGroups").Append('=').Append(repoCurveSecurityGroups).Append(',').Append(' ');
		buf.Append("repoCurveGroups").Append('=').Append(repoCurveGroups).Append(',').Append(' ');
		buf.Append("repoCurves").Append('=').Append(repoCurves).Append(',').Append(' ');
		buf.Append("issuerCurveGroups").Append('=').Append(issuerCurveGroups).Append(',').Append(' ');
		buf.Append("issuerCurves").Append('=').Append(JodaBeanUtils.ToString(issuerCurves));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableLegalEntityDiscountingProvider}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  valuationDate_Renamed = DirectMetaProperty.ofImmutable(this, "valuationDate", typeof(ImmutableLegalEntityDiscountingProvider), typeof(LocalDate));
			  repoCurveSecurityGroups_Renamed = DirectMetaProperty.ofImmutable(this, "repoCurveSecurityGroups", typeof(ImmutableLegalEntityDiscountingProvider), (Type) typeof(ImmutableMap));
			  repoCurveGroups_Renamed = DirectMetaProperty.ofImmutable(this, "repoCurveGroups", typeof(ImmutableLegalEntityDiscountingProvider), (Type) typeof(ImmutableMap));
			  repoCurves_Renamed = DirectMetaProperty.ofImmutable(this, "repoCurves", typeof(ImmutableLegalEntityDiscountingProvider), (Type) typeof(ImmutableMap));
			  issuerCurveGroups_Renamed = DirectMetaProperty.ofImmutable(this, "issuerCurveGroups", typeof(ImmutableLegalEntityDiscountingProvider), (Type) typeof(ImmutableMap));
			  issuerCurves_Renamed = DirectMetaProperty.ofImmutable(this, "issuerCurves", typeof(ImmutableLegalEntityDiscountingProvider), (Type) typeof(ImmutableMap));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "valuationDate", "repoCurveSecurityGroups", "repoCurveGroups", "repoCurves", "issuerCurveGroups", "issuerCurves");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code valuationDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> valuationDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code repoCurveSecurityGroups} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.product.SecurityId, com.opengamma.strata.market.curve.RepoGroup>> repoCurveSecurityGroups = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "repoCurveSecurityGroups", ImmutableLegalEntityDiscountingProvider.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<SecurityId, RepoGroup>> repoCurveSecurityGroups_Renamed;
		/// <summary>
		/// The meta-property for the {@code repoCurveGroups} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.product.LegalEntityId, com.opengamma.strata.market.curve.RepoGroup>> repoCurveGroups = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "repoCurveGroups", ImmutableLegalEntityDiscountingProvider.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<LegalEntityId, RepoGroup>> repoCurveGroups_Renamed;
		/// <summary>
		/// The meta-property for the {@code repoCurves} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.market.curve.RepoGroup, com.opengamma.strata.basics.currency.Currency>, com.opengamma.strata.pricer.DiscountFactors>> repoCurves = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "repoCurves", ImmutableLegalEntityDiscountingProvider.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<Pair<RepoGroup, Currency>, DiscountFactors>> repoCurves_Renamed;
		/// <summary>
		/// The meta-property for the {@code issuerCurveGroups} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.product.LegalEntityId, com.opengamma.strata.market.curve.LegalEntityGroup>> issuerCurveGroups = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "issuerCurveGroups", ImmutableLegalEntityDiscountingProvider.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<LegalEntityId, LegalEntityGroup>> issuerCurveGroups_Renamed;
		/// <summary>
		/// The meta-property for the {@code issuerCurves} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.market.curve.LegalEntityGroup, com.opengamma.strata.basics.currency.Currency>, com.opengamma.strata.pricer.DiscountFactors>> issuerCurves = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "issuerCurves", ImmutableLegalEntityDiscountingProvider.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<Pair<LegalEntityGroup, Currency>, DiscountFactors>> issuerCurves_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "valuationDate", "repoCurveSecurityGroups", "repoCurveGroups", "repoCurves", "issuerCurveGroups", "issuerCurves");
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
			case 113107279: // valuationDate
			  return valuationDate_Renamed;
			case -1749299407: // repoCurveSecurityGroups
			  return repoCurveSecurityGroups_Renamed;
			case -1279842095: // repoCurveGroups
			  return repoCurveGroups_Renamed;
			case 587630454: // repoCurves
			  return repoCurves_Renamed;
			case 1830129450: // issuerCurveGroups
			  return issuerCurveGroups_Renamed;
			case -1909076611: // issuerCurves
			  return issuerCurves_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ImmutableLegalEntityDiscountingProvider.Builder builder()
		{
		  return new ImmutableLegalEntityDiscountingProvider.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ImmutableLegalEntityDiscountingProvider);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code valuationDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> valuationDate()
		{
		  return valuationDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code repoCurveSecurityGroups} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<SecurityId, RepoGroup>> repoCurveSecurityGroups()
		{
		  return repoCurveSecurityGroups_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code repoCurveGroups} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<LegalEntityId, RepoGroup>> repoCurveGroups()
		{
		  return repoCurveGroups_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code repoCurves} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<Pair<RepoGroup, Currency>, DiscountFactors>> repoCurves()
		{
		  return repoCurves_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code issuerCurveGroups} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<LegalEntityId, LegalEntityGroup>> issuerCurveGroups()
		{
		  return issuerCurveGroups_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code issuerCurves} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<Pair<LegalEntityGroup, Currency>, DiscountFactors>> issuerCurves()
		{
		  return issuerCurves_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 113107279: // valuationDate
			  return ((ImmutableLegalEntityDiscountingProvider) bean).ValuationDate;
			case -1749299407: // repoCurveSecurityGroups
			  return ((ImmutableLegalEntityDiscountingProvider) bean).RepoCurveSecurityGroups;
			case -1279842095: // repoCurveGroups
			  return ((ImmutableLegalEntityDiscountingProvider) bean).RepoCurveGroups;
			case 587630454: // repoCurves
			  return ((ImmutableLegalEntityDiscountingProvider) bean).RepoCurves;
			case 1830129450: // issuerCurveGroups
			  return ((ImmutableLegalEntityDiscountingProvider) bean).IssuerCurveGroups;
			case -1909076611: // issuerCurves
			  return ((ImmutableLegalEntityDiscountingProvider) bean).IssuerCurves;
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
	  /// The bean-builder for {@code ImmutableLegalEntityDiscountingProvider}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ImmutableLegalEntityDiscountingProvider>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate valuationDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IDictionary<SecurityId, RepoGroup> repoCurveSecurityGroups_Renamed = ImmutableMap.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IDictionary<LegalEntityId, RepoGroup> repoCurveGroups_Renamed = ImmutableMap.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IDictionary<Pair<RepoGroup, Currency>, DiscountFactors> repoCurves_Renamed = ImmutableMap.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IDictionary<LegalEntityId, LegalEntityGroup> issuerCurveGroups_Renamed = ImmutableMap.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IDictionary<Pair<LegalEntityGroup, Currency>, DiscountFactors> issuerCurves_Renamed = ImmutableMap.of();

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ImmutableLegalEntityDiscountingProvider beanToCopy)
		{
		  this.valuationDate_Renamed = beanToCopy.ValuationDate;
		  this.repoCurveSecurityGroups_Renamed = beanToCopy.RepoCurveSecurityGroups;
		  this.repoCurveGroups_Renamed = beanToCopy.RepoCurveGroups;
		  this.repoCurves_Renamed = beanToCopy.RepoCurves;
		  this.issuerCurveGroups_Renamed = beanToCopy.IssuerCurveGroups;
		  this.issuerCurves_Renamed = beanToCopy.IssuerCurves;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 113107279: // valuationDate
			  return valuationDate_Renamed;
			case -1749299407: // repoCurveSecurityGroups
			  return repoCurveSecurityGroups_Renamed;
			case -1279842095: // repoCurveGroups
			  return repoCurveGroups_Renamed;
			case 587630454: // repoCurves
			  return repoCurves_Renamed;
			case 1830129450: // issuerCurveGroups
			  return issuerCurveGroups_Renamed;
			case -1909076611: // issuerCurves
			  return issuerCurves_Renamed;
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
			case 113107279: // valuationDate
			  this.valuationDate_Renamed = (LocalDate) newValue;
			  break;
			case -1749299407: // repoCurveSecurityGroups
			  this.repoCurveSecurityGroups_Renamed = (IDictionary<SecurityId, RepoGroup>) newValue;
			  break;
			case -1279842095: // repoCurveGroups
			  this.repoCurveGroups_Renamed = (IDictionary<LegalEntityId, RepoGroup>) newValue;
			  break;
			case 587630454: // repoCurves
			  this.repoCurves_Renamed = (IDictionary<Pair<RepoGroup, Currency>, DiscountFactors>) newValue;
			  break;
			case 1830129450: // issuerCurveGroups
			  this.issuerCurveGroups_Renamed = (IDictionary<LegalEntityId, LegalEntityGroup>) newValue;
			  break;
			case -1909076611: // issuerCurves
			  this.issuerCurves_Renamed = (IDictionary<Pair<LegalEntityGroup, Currency>, DiscountFactors>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override Builder set<T1>(MetaProperty<T1> property, object value)
		{
		  base.set(property, value);
		  return this;
		}

		public override ImmutableLegalEntityDiscountingProvider build()
		{
		  preBuild(this);
		  return new ImmutableLegalEntityDiscountingProvider(valuationDate_Renamed, repoCurveSecurityGroups_Renamed, repoCurveGroups_Renamed, repoCurves_Renamed, issuerCurveGroups_Renamed, issuerCurves_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the valuation date.
		/// All curves and other data items in this provider are calibrated for this date. </summary>
		/// <param name="valuationDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder valuationDate(LocalDate valuationDate)
		{
		  JodaBeanUtils.notNull(valuationDate, "valuationDate");
		  this.valuationDate_Renamed = valuationDate;
		  return this;
		}

		/// <summary>
		/// Sets the groups used to find a repo curve by security.
		/// <para>
		/// This maps the security ID to a group.
		/// The group is used to find the curve in {@code repoCurves}.
		/// </para>
		/// </summary>
		/// <param name="repoCurveSecurityGroups">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder repoCurveSecurityGroups(IDictionary<SecurityId, RepoGroup> repoCurveSecurityGroups)
		{
		  JodaBeanUtils.notNull(repoCurveSecurityGroups, "repoCurveSecurityGroups");
		  this.repoCurveSecurityGroups_Renamed = repoCurveSecurityGroups;
		  return this;
		}

		/// <summary>
		/// Sets the groups used to find a repo curve by legal entity.
		/// <para>
		/// This maps the legal entity ID to a group.
		/// The group is used to find the curve in {@code repoCurves}.
		/// </para>
		/// </summary>
		/// <param name="repoCurveGroups">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder repoCurveGroups(IDictionary<LegalEntityId, RepoGroup> repoCurveGroups)
		{
		  JodaBeanUtils.notNull(repoCurveGroups, "repoCurveGroups");
		  this.repoCurveGroups_Renamed = repoCurveGroups;
		  return this;
		}

		/// <summary>
		/// Sets the repo curves, keyed by group and currency.
		/// The curve data, predicting the future, associated with each repo group and currency. </summary>
		/// <param name="repoCurves">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder repoCurves(IDictionary<Pair<RepoGroup, Currency>, DiscountFactors> repoCurves)
		{
		  JodaBeanUtils.notNull(repoCurves, "repoCurves");
		  this.repoCurves_Renamed = repoCurves;
		  return this;
		}

		/// <summary>
		/// Sets the groups used to find an issuer curve by legal entity.
		/// <para>
		/// This maps the legal entity ID to a group.
		/// The group is used to find the curve in {@code issuerCurves}.
		/// </para>
		/// <para>
		/// This property was renamed in version 1.1 of Strata from {@code legalEntityMap}.
		/// </para>
		/// </summary>
		/// <param name="issuerCurveGroups">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder issuerCurveGroups(IDictionary<LegalEntityId, LegalEntityGroup> issuerCurveGroups)
		{
		  JodaBeanUtils.notNull(issuerCurveGroups, "issuerCurveGroups");
		  this.issuerCurveGroups_Renamed = issuerCurveGroups;
		  return this;
		}

		/// <summary>
		/// Sets the issuer curves, keyed by group and currency.
		/// The curve data, predicting the future, associated with each legal entity group and currency. </summary>
		/// <param name="issuerCurves">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder issuerCurves(IDictionary<Pair<LegalEntityGroup, Currency>, DiscountFactors> issuerCurves)
		{
		  JodaBeanUtils.notNull(issuerCurves, "issuerCurves");
		  this.issuerCurves_Renamed = issuerCurves;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(224);
		  buf.Append("ImmutableLegalEntityDiscountingProvider.Builder{");
		  buf.Append("valuationDate").Append('=').Append(JodaBeanUtils.ToString(valuationDate_Renamed)).Append(',').Append(' ');
		  buf.Append("repoCurveSecurityGroups").Append('=').Append(JodaBeanUtils.ToString(repoCurveSecurityGroups_Renamed)).Append(',').Append(' ');
		  buf.Append("repoCurveGroups").Append('=').Append(JodaBeanUtils.ToString(repoCurveGroups_Renamed)).Append(',').Append(' ');
		  buf.Append("repoCurves").Append('=').Append(JodaBeanUtils.ToString(repoCurves_Renamed)).Append(',').Append(' ');
		  buf.Append("issuerCurveGroups").Append('=').Append(JodaBeanUtils.ToString(issuerCurveGroups_Renamed)).Append(',').Append(' ');
		  buf.Append("issuerCurves").Append('=').Append(JodaBeanUtils.ToString(issuerCurves_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}