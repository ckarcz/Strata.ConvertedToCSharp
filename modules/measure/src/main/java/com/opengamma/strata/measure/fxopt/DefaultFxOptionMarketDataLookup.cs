using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fxopt
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
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using CalculationParameter = com.opengamma.strata.calc.runner.CalculationParameter;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using Messages = com.opengamma.strata.collect.Messages;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using FxOptionVolatilities = com.opengamma.strata.pricer.fxopt.FxOptionVolatilities;
	using FxOptionVolatilitiesId = com.opengamma.strata.pricer.fxopt.FxOptionVolatilitiesId;

	/// <summary>
	/// The FX options lookup, used to select volatilities for pricing.
	/// <para>
	/// This provides FX options volatilities by currency pair.
	/// </para>
	/// <para>
	/// The lookup implements <seealso cref="CalculationParameter"/> and is used by passing it
	/// as an argument to <seealso cref="CalculationRules"/>. It provides the link between the
	/// data that the function needs and the data that is available in <seealso cref="ScenarioMarketData"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") final class DefaultFxOptionMarketDataLookup implements FxOptionMarketDataLookup, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	internal sealed class DefaultFxOptionMarketDataLookup : FxOptionMarketDataLookup, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.basics.currency.CurrencyPair, com.opengamma.strata.pricer.fxopt.FxOptionVolatilitiesId> volatilityIds;
		private readonly ImmutableMap<CurrencyPair, FxOptionVolatilitiesId> volatilityIds;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance based on a single mapping from currency pair to volatility identifier.
	  /// <para>
	  /// The lookup provides volatilities for the specified currency pair.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currencyPair">  the currency pair </param>
	  /// <param name="volatilityId">  the volatility identifier </param>
	  /// <returns> the FX options lookup containing the specified mapping </returns>
	  public static DefaultFxOptionMarketDataLookup of(CurrencyPair currencyPair, FxOptionVolatilitiesId volatilityId)
	  {
		return new DefaultFxOptionMarketDataLookup(ImmutableMap.of(currencyPair, volatilityId));
	  }

	  /// <summary>
	  /// Obtains an instance based on a map of volatility identifiers.
	  /// <para>
	  /// The map is used to specify the appropriate volatilities to use for each currency pair.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="volatilityIds">  the volatility identifiers, keyed by currency pair </param>
	  /// <returns> the FX options lookup containing the specified volatilities </returns>
	  public static DefaultFxOptionMarketDataLookup of(IDictionary<CurrencyPair, FxOptionVolatilitiesId> volatilityIds)
	  {
		return new DefaultFxOptionMarketDataLookup(volatilityIds);
	  }

	  //-------------------------------------------------------------------------
	  public ImmutableSet<CurrencyPair> VolatilityCurrencyPairs
	  {
		  get
		  {
			return volatilityIds.Keys;
		  }
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public com.google.common.collect.ImmutableSet<com.opengamma.strata.data.MarketDataId<?>> getVolatilityIds(com.opengamma.strata.basics.currency.CurrencyPair currencyPair)
	  public ImmutableSet<MarketDataId<object>> getVolatilityIds(CurrencyPair currencyPair)
	  {
		FxOptionVolatilitiesId id = volatilityIds.get(currencyPair);
		if (id == null)
		{
		  throw new System.ArgumentException(msgPairNotFound(currencyPair));
		}
		return ImmutableSet.of(id);
	  }

	  //-------------------------------------------------------------------------
	  public FunctionRequirements requirements(ISet<CurrencyPair> currencyPairs)
	  {
		foreach (CurrencyPair currencyPair in currencyPairs)
		{
		  if (!volatilityIds.Keys.Contains(currencyPair))
		  {
			throw new System.ArgumentException(msgPairNotFound(currencyPair));
		  }
		}
		return FunctionRequirements.builder().valueRequirements(ImmutableSet.copyOf(volatilityIds.values())).build();
	  }

	  //-------------------------------------------------------------------------
	  public FxOptionVolatilities volatilities(CurrencyPair currencyPair, MarketData marketData)
	  {
		FxOptionVolatilitiesId volatilityId = volatilityIds.get(currencyPair);
		if (volatilityId == null)
		{
		  throw new MarketDataNotFoundException(msgPairNotFound(currencyPair));
		}
		return marketData.getValue(volatilityId);
	  }

	  //-------------------------------------------------------------------------
	  private string msgPairNotFound(CurrencyPair currencyPair)
	  {
		return Messages.format("FxOption lookup has no volatilities defined for currency pair '{}'", currencyPair);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DefaultFxOptionMarketDataLookup}.
	  /// </summary>
	  private static readonly TypedMetaBean<DefaultFxOptionMarketDataLookup> META_BEAN = LightMetaBean.of(typeof(DefaultFxOptionMarketDataLookup), MethodHandles.lookup(), new string[] {"volatilityIds"}, ImmutableMap.of());

	  /// <summary>
	  /// The meta-bean for {@code DefaultFxOptionMarketDataLookup}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<DefaultFxOptionMarketDataLookup> meta()
	  {
		return META_BEAN;
	  }

	  static DefaultFxOptionMarketDataLookup()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private DefaultFxOptionMarketDataLookup(IDictionary<CurrencyPair, FxOptionVolatilitiesId> volatilityIds)
	  {
		JodaBeanUtils.notNull(volatilityIds, "volatilityIds");
		this.volatilityIds = ImmutableMap.copyOf(volatilityIds);
	  }

	  public override TypedMetaBean<DefaultFxOptionMarketDataLookup> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the volatility identifiers, keyed by currency pair. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<CurrencyPair, FxOptionVolatilitiesId> VolatilityIds
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
		  DefaultFxOptionMarketDataLookup other = (DefaultFxOptionMarketDataLookup) obj;
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
		buf.Append("DefaultFxOptionMarketDataLookup{");
		buf.Append("volatilityIds").Append('=').Append(JodaBeanUtils.ToString(volatilityIds));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}