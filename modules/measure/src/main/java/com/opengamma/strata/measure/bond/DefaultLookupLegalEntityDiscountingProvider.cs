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

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using LegalEntityGroup = com.opengamma.strata.market.curve.LegalEntityGroup;
	using RepoGroup = com.opengamma.strata.market.curve.RepoGroup;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using DiscountFactors = com.opengamma.strata.pricer.DiscountFactors;
	using ImmutableLegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.ImmutableLegalEntityDiscountingProvider;
	using IssuerCurveDiscountFactors = com.opengamma.strata.pricer.bond.IssuerCurveDiscountFactors;
	using IssuerCurveZeroRateSensitivity = com.opengamma.strata.pricer.bond.IssuerCurveZeroRateSensitivity;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;
	using RepoCurveDiscountFactors = com.opengamma.strata.pricer.bond.RepoCurveDiscountFactors;
	using RepoCurveZeroRateSensitivity = com.opengamma.strata.pricer.bond.RepoCurveZeroRateSensitivity;
	using LegalEntityId = com.opengamma.strata.product.LegalEntityId;
	using SecurityId = com.opengamma.strata.product.SecurityId;

	/// <summary>
	/// A legal entity discounting provider based on a discounting lookup.
	/// <para>
	/// This uses a <seealso cref="DefaultLegalEntityDiscountingMarketDataLookup"/> to provide a view on <seealso cref="MarketData"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") final class DefaultLookupLegalEntityDiscountingProvider implements com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	internal sealed class DefaultLookupLegalEntityDiscountingProvider : LegalEntityDiscountingProvider, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final DefaultLegalEntityDiscountingMarketDataLookup lookup;
		private readonly DefaultLegalEntityDiscountingMarketDataLookup lookup;
	  /// <summary>
	  /// The market data.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.data.MarketData marketData;
	  private readonly MarketData marketData;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance based on a lookup and market data.
	  /// <para>
	  /// The lookup provides the mapping to repo and issuer curve IDs.
	  /// The curves are in the market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="lookup">  the lookup </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the rates provider </returns>
	  public static DefaultLookupLegalEntityDiscountingProvider of(DefaultLegalEntityDiscountingMarketDataLookup lookup, MarketData marketData)
	  {

		return new DefaultLookupLegalEntityDiscountingProvider(lookup, marketData);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private DefaultLookupLegalEntityDiscountingProvider(DefaultLegalEntityDiscountingMarketDataLookup lookup, com.opengamma.strata.data.MarketData marketData)
	  private DefaultLookupLegalEntityDiscountingProvider(DefaultLegalEntityDiscountingMarketDataLookup lookup, MarketData marketData)
	  {

		this.lookup = ArgChecker.notNull(lookup, "lookup");
		this.marketData = ArgChecker.notNull(marketData, "marketData");
	  }

	  //-------------------------------------------------------------------------
	  public LocalDate ValuationDate
	  {
		  get
		  {
			return marketData.ValuationDate;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public RepoCurveDiscountFactors repoCurveDiscountFactors(SecurityId securityId, LegalEntityId issuerId, Currency currency)
	  {
		RepoGroup repoGroup = lookup.RepoCurveSecurityGroups.get(securityId);
		if (repoGroup == null)
		{
		  repoGroup = lookup.RepoCurveGroups.get(issuerId);
		  if (repoGroup == null)
		  {
			throw new MarketDataNotFoundException("Unable to find repo curve mapping for ID: " + securityId + ", " + issuerId);
		  }
		}
		return repoCurveDiscountFactors(repoGroup, currency);
	  }

	  public RepoCurveDiscountFactors repoCurveDiscountFactors(LegalEntityId issuerId, Currency currency)
	  {
		RepoGroup repoGroup = lookup.RepoCurveGroups.get(issuerId);
		if (repoGroup == null)
		{
		  throw new MarketDataNotFoundException("Unable to find repo curve mapping for ID: " + issuerId);
		}
		return repoCurveDiscountFactors(repoGroup, currency);
	  }

	  // lookup the discount factors for the repo group
	  private RepoCurveDiscountFactors repoCurveDiscountFactors(RepoGroup repoGroup, Currency currency)
	  {
		CurveId curveId = lookup.RepoCurves.get(Pair.of(repoGroup, currency));
		if (curveId == null)
		{
		  throw new MarketDataNotFoundException("Unable to find repo curve: " + repoGroup + ", " + currency);
		}
		Curve curve = marketData.getValue(curveId);
		DiscountFactors df = DiscountFactors.of(currency, ValuationDate, curve);
		return RepoCurveDiscountFactors.of(df, repoGroup);
	  }

	  //-------------------------------------------------------------------------
	  public IssuerCurveDiscountFactors issuerCurveDiscountFactors(LegalEntityId issuerId, Currency currency)
	  {
		LegalEntityGroup legalEntityGroup = lookup.IssuerCurveGroups.get(issuerId);
		if (legalEntityGroup == null)
		{
		  throw new MarketDataNotFoundException("Unable to find issuer curve mapping for ID: " + issuerId);
		}
		return issuerCurveDiscountFactors(legalEntityGroup, currency);
	  }

	  // lookup the discount factors for the legal entity group
	  private IssuerCurveDiscountFactors issuerCurveDiscountFactors(LegalEntityGroup legalEntityGroup, Currency currency)
	  {
		CurveId curveId = lookup.IssuerCurves.get(Pair.of(legalEntityGroup, currency));
		if (curveId == null)
		{
		  throw new MarketDataNotFoundException("Unable to find issuer curve: " + legalEntityGroup + ", " + currency);
		}
		Curve curve = marketData.getValue(curveId);
		DiscountFactors df = DiscountFactors.of(currency, ValuationDate, curve);
		return IssuerCurveDiscountFactors.of(df, legalEntityGroup);
	  }

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
	  public T data<T>(MarketDataId<T> key)
	  {
		return marketData.getValue(key);
	  }

	  public Optional<T> findData<T>(MarketDataName<T> name)
	  {
		return Stream.concat(lookup.RepoCurves.values().stream(), lookup.IssuerCurves.values().stream()).filter(id => id.MarketDataName.Equals(name)).findFirst().flatMap(id => marketData.findValue(id)).map(v => name.MarketDataType.cast(v));
	  }

	  public ImmutableLegalEntityDiscountingProvider toImmutableLegalEntityDiscountingProvider()
	  {
		// repo curves
		IDictionary<Pair<RepoGroup, Currency>, DiscountFactors> repoCurves = new Dictionary<Pair<RepoGroup, Currency>, DiscountFactors>();
		foreach (Pair<RepoGroup, Currency> pair in lookup.RepoCurves.Keys)
		{
		  CurveId curveId = lookup.RepoCurves.get(pair);
		  if (marketData.containsValue(curveId))
		  {
			Curve curve = marketData.getValue(curveId);
			repoCurves[pair] = DiscountFactors.of(pair.Second, ValuationDate, curve);
		  }
		}
		// issuer curves
		IDictionary<Pair<LegalEntityGroup, Currency>, DiscountFactors> issuerCurves = new Dictionary<Pair<LegalEntityGroup, Currency>, DiscountFactors>();
		foreach (Pair<LegalEntityGroup, Currency> pair in lookup.IssuerCurves.Keys)
		{
		  CurveId curveId = lookup.IssuerCurves.get(pair);
		  if (marketData.containsValue(curveId))
		  {
			Curve curve = marketData.getValue(curveId);
			issuerCurves[pair] = DiscountFactors.of(pair.Second, ValuationDate, curve);
		  }
		}
		// build result
		return ImmutableLegalEntityDiscountingProvider.builder().valuationDate(ValuationDate).repoCurveSecurityGroups(lookup.RepoCurveSecurityGroups).repoCurveGroups(lookup.RepoCurveGroups).repoCurves(repoCurves).issuerCurveGroups(lookup.IssuerCurveGroups).issuerCurves(issuerCurves).build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DefaultLookupLegalEntityDiscountingProvider}.
	  /// </summary>
	  private static readonly TypedMetaBean<DefaultLookupLegalEntityDiscountingProvider> META_BEAN = LightMetaBean.of(typeof(DefaultLookupLegalEntityDiscountingProvider), MethodHandles.lookup(), new string[] {"lookup", "marketData"}, new object[0]);

	  /// <summary>
	  /// The meta-bean for {@code DefaultLookupLegalEntityDiscountingProvider}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<DefaultLookupLegalEntityDiscountingProvider> meta()
	  {
		return META_BEAN;
	  }

	  static DefaultLookupLegalEntityDiscountingProvider()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override TypedMetaBean<DefaultLookupLegalEntityDiscountingProvider> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the lookup. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DefaultLegalEntityDiscountingMarketDataLookup Lookup
	  {
		  get
		  {
			return lookup;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market data. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public MarketData MarketData
	  {
		  get
		  {
			return marketData;
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
		  DefaultLookupLegalEntityDiscountingProvider other = (DefaultLookupLegalEntityDiscountingProvider) obj;
		  return JodaBeanUtils.equal(lookup, other.lookup) && JodaBeanUtils.equal(marketData, other.marketData);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(lookup);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(marketData);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("DefaultLookupLegalEntityDiscountingProvider{");
		buf.Append("lookup").Append('=').Append(lookup).Append(',').Append(' ');
		buf.Append("marketData").Append('=').Append(JodaBeanUtils.ToString(marketData));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}