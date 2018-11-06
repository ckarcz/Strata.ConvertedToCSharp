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
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using CalculationParameter = com.opengamma.strata.calc.runner.CalculationParameter;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using Messages = com.opengamma.strata.collect.Messages;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using BondFutureVolatilities = com.opengamma.strata.pricer.bond.BondFutureVolatilities;
	using BondFutureVolatilitiesId = com.opengamma.strata.pricer.bond.BondFutureVolatilitiesId;
	using SecurityId = com.opengamma.strata.product.SecurityId;

	/// <summary>
	/// The bond future options lookup, used to select volatilities for pricing.
	/// <para>
	/// This provides bond future volatilities by security ID.
	/// </para>
	/// <para>
	/// The lookup implements <seealso cref="CalculationParameter"/> and is used by passing it
	/// as an argument to <seealso cref="CalculationRules"/>. It provides the link between the
	/// data that the function needs and the data that is available in <seealso cref="ScenarioMarketData"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") final class DefaultBondFutureOptionMarketDataLookup implements BondFutureOptionMarketDataLookup, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	internal sealed class DefaultBondFutureOptionMarketDataLookup : BondFutureOptionMarketDataLookup, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.product.SecurityId, com.opengamma.strata.pricer.bond.BondFutureVolatilitiesId> volatilityIds;
		private readonly ImmutableMap<SecurityId, BondFutureVolatilitiesId> volatilityIds;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance based on a single mapping from security ID to volatility identifier.
	  /// <para>
	  /// The lookup provides volatilities for the specified security ID.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="securityId">  the security ID </param>
	  /// <param name="volatilityId">  the volatility identifier </param>
	  /// <returns> the bond future options lookup containing the specified mapping </returns>
	  public static DefaultBondFutureOptionMarketDataLookup of(SecurityId securityId, BondFutureVolatilitiesId volatilityId)
	  {

		return new DefaultBondFutureOptionMarketDataLookup(ImmutableMap.of(securityId, volatilityId));
	  }

	  /// <summary>
	  /// Obtains an instance based on a map of volatility identifiers.
	  /// <para>
	  /// The map is used to specify the appropriate volatilities to use for each security ID.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="volatilityIds">  the volatility identifiers, keyed by security ID </param>
	  /// <returns> the bond future options lookup containing the specified volatilities </returns>
	  public static DefaultBondFutureOptionMarketDataLookup of(IDictionary<SecurityId, BondFutureVolatilitiesId> volatilityIds)
	  {

		return new DefaultBondFutureOptionMarketDataLookup(volatilityIds);
	  }

	  //-------------------------------------------------------------------------
	  public ImmutableSet<SecurityId> VolatilitySecurityIds
	  {
		  get
		  {
			return volatilityIds.Keys;
		  }
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public com.google.common.collect.ImmutableSet<com.opengamma.strata.data.MarketDataId<?>> getVolatilityIds(com.opengamma.strata.product.SecurityId securityId)
	  public ImmutableSet<MarketDataId<object>> getVolatilityIds(SecurityId securityId)
	  {
		BondFutureVolatilitiesId id = volatilityIds.get(securityId);
		if (id == null)
		{
		  throw new System.ArgumentException(msgSecurityNotFound(securityId));
		}
		return ImmutableSet.of(id);
	  }

	  //-------------------------------------------------------------------------
	  public FunctionRequirements requirements(ISet<SecurityId> securityIds)
	  {
		ISet<BondFutureVolatilitiesId> volIds = new HashSet<BondFutureVolatilitiesId>();
		foreach (SecurityId securityId in securityIds)
		{
		  if (!volatilityIds.Keys.Contains(securityId))
		  {
			throw new System.ArgumentException(msgSecurityNotFound(securityId));
		  }
		  volIds.Add(volatilityIds.get(securityId));
		}
		return FunctionRequirements.builder().valueRequirements(volIds).build();
	  }

	  //-------------------------------------------------------------------------
	  public BondFutureVolatilities volatilities(SecurityId securityId, MarketData marketData)
	  {
		BondFutureVolatilitiesId volatilityId = volatilityIds.get(securityId);
		if (volatilityId == null)
		{
		  throw new MarketDataNotFoundException(msgSecurityNotFound(securityId));
		}
		return marketData.getValue(volatilityId);
	  }

	  //-------------------------------------------------------------------------
	  private string msgSecurityNotFound(SecurityId securityId)
	  {
		return Messages.format("BondFutureOption lookup has no volatilities defined for security ID '{}'", securityId);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DefaultBondFutureOptionMarketDataLookup}.
	  /// </summary>
	  private static readonly TypedMetaBean<DefaultBondFutureOptionMarketDataLookup> META_BEAN = LightMetaBean.of(typeof(DefaultBondFutureOptionMarketDataLookup), MethodHandles.lookup(), new string[] {"volatilityIds"}, ImmutableMap.of());

	  /// <summary>
	  /// The meta-bean for {@code DefaultBondFutureOptionMarketDataLookup}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<DefaultBondFutureOptionMarketDataLookup> meta()
	  {
		return META_BEAN;
	  }

	  static DefaultBondFutureOptionMarketDataLookup()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private DefaultBondFutureOptionMarketDataLookup(IDictionary<SecurityId, BondFutureVolatilitiesId> volatilityIds)
	  {
		JodaBeanUtils.notNull(volatilityIds, "volatilityIds");
		this.volatilityIds = ImmutableMap.copyOf(volatilityIds);
	  }

	  public override TypedMetaBean<DefaultBondFutureOptionMarketDataLookup> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the volatility identifiers, keyed by security ID. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<SecurityId, BondFutureVolatilitiesId> VolatilityIds
	  {
		  get
		  {
			return volatilityIds;
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
		  DefaultBondFutureOptionMarketDataLookup other = (DefaultBondFutureOptionMarketDataLookup) obj;
		  return JodaBeanUtils.equal(volatilityIds, other.volatilityIds);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(volatilityIds);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(64);
		buf.Append("DefaultBondFutureOptionMarketDataLookup{");
		buf.Append("volatilityIds").Append('=').Append(JodaBeanUtils.ToString(volatilityIds));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}