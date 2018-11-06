using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.index
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
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using Index = com.opengamma.strata.basics.index.Index;
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using CalculationParameter = com.opengamma.strata.calc.runner.CalculationParameter;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using Messages = com.opengamma.strata.collect.Messages;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using IborFutureOptionVolatilities = com.opengamma.strata.pricer.index.IborFutureOptionVolatilities;
	using IborFutureOptionVolatilitiesId = com.opengamma.strata.pricer.index.IborFutureOptionVolatilitiesId;

	/// <summary>
	/// The Ibor future option lookup, used to select volatilities for pricing.
	/// <para>
	/// This provides Ibor future option volatilities by index.
	/// </para>
	/// <para>
	/// The lookup implements <seealso cref="CalculationParameter"/> and is used by passing it
	/// as an argument to <seealso cref="CalculationRules"/>. It provides the link between the
	/// data that the function needs and the data that is available in <seealso cref="ScenarioMarketData"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") final class DefaultIborFutureOptionMarketDataLookup implements IborFutureOptionMarketDataLookup, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	internal sealed class DefaultIborFutureOptionMarketDataLookup : IborFutureOptionMarketDataLookup, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.basics.index.IborIndex, com.opengamma.strata.pricer.index.IborFutureOptionVolatilitiesId> volatilityIds;
		private readonly ImmutableMap<IborIndex, IborFutureOptionVolatilitiesId> volatilityIds;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance based on a single mapping from index to volatility identifier.
	  /// <para>
	  /// The lookup provides volatilities for the specified index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the Ibor index </param>
	  /// <param name="volatilityId">  the volatility identifier </param>
	  /// <returns> the Ibor future option lookup containing the specified mapping </returns>
	  public static DefaultIborFutureOptionMarketDataLookup of(IborIndex index, IborFutureOptionVolatilitiesId volatilityId)
	  {
		return new DefaultIborFutureOptionMarketDataLookup(ImmutableMap.of(index, volatilityId));
	  }

	  /// <summary>
	  /// Obtains an instance based on a map of volatility identifiers.
	  /// <para>
	  /// The map is used to specify the appropriate volatilities to use for each index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="volatilityIds">  the volatility identifiers, keyed by index </param>
	  /// <returns> the Ibor future option lookup containing the specified volatilities </returns>
	  public static DefaultIborFutureOptionMarketDataLookup of(IDictionary<IborIndex, IborFutureOptionVolatilitiesId> volatilityIds)
	  {
		return new DefaultIborFutureOptionMarketDataLookup(volatilityIds);
	  }

	  //-------------------------------------------------------------------------
	  public ImmutableSet<IborIndex> VolatilityIndices
	  {
		  get
		  {
			return volatilityIds.Keys;
		  }
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public com.google.common.collect.ImmutableSet<com.opengamma.strata.data.MarketDataId<?>> getVolatilityIds(com.opengamma.strata.basics.index.IborIndex index)
	  public ImmutableSet<MarketDataId<object>> getVolatilityIds(IborIndex index)
	  {
		IborFutureOptionVolatilitiesId id = volatilityIds.get(index);
		if (id == null)
		{
		  throw new System.ArgumentException(msgIndexNotFound(index));
		}
		return ImmutableSet.of(id);
	  }

	  //-------------------------------------------------------------------------
	  public FunctionRequirements requirements(ISet<IborIndex> indices)
	  {
		ISet<IborFutureOptionVolatilitiesId> volIds = new HashSet<IborFutureOptionVolatilitiesId>();
		foreach (Index index in indices)
		{
		  if (!volatilityIds.Keys.Contains(index))
		  {
			throw new System.ArgumentException(msgIndexNotFound(index));
		  }
		  volIds.Add(volatilityIds.get(index));
		}
		return FunctionRequirements.builder().valueRequirements(volIds).build();
	  }

	  //-------------------------------------------------------------------------
	  public IborFutureOptionVolatilities volatilities(IborIndex index, MarketData marketData)
	  {
		IborFutureOptionVolatilitiesId volatilityId = volatilityIds.get(index);
		if (volatilityId == null)
		{
		  throw new MarketDataNotFoundException(msgIndexNotFound(index));
		}
		return marketData.getValue(volatilityId);
	  }

	  //-------------------------------------------------------------------------
	  private string msgIndexNotFound(Index index)
	  {
		return Messages.format("IborFutureOption lookup has no volatilities defined for index '{}'", index);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DefaultIborFutureOptionMarketDataLookup}.
	  /// </summary>
	  private static readonly TypedMetaBean<DefaultIborFutureOptionMarketDataLookup> META_BEAN = LightMetaBean.of(typeof(DefaultIborFutureOptionMarketDataLookup), MethodHandles.lookup(), new string[] {"volatilityIds"}, ImmutableMap.of());

	  /// <summary>
	  /// The meta-bean for {@code DefaultIborFutureOptionMarketDataLookup}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<DefaultIborFutureOptionMarketDataLookup> meta()
	  {
		return META_BEAN;
	  }

	  static DefaultIborFutureOptionMarketDataLookup()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private DefaultIborFutureOptionMarketDataLookup(IDictionary<IborIndex, IborFutureOptionVolatilitiesId> volatilityIds)
	  {
		JodaBeanUtils.notNull(volatilityIds, "volatilityIds");
		this.volatilityIds = ImmutableMap.copyOf(volatilityIds);
	  }

	  public override TypedMetaBean<DefaultIborFutureOptionMarketDataLookup> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the volatility identifiers, keyed by index. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<IborIndex, IborFutureOptionVolatilitiesId> VolatilityIds
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
		  DefaultIborFutureOptionMarketDataLookup other = (DefaultIborFutureOptionMarketDataLookup) obj;
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
		buf.Append("DefaultIborFutureOptionMarketDataLookup{");
		buf.Append("volatilityIds").Append('=').Append(JodaBeanUtils.ToString(volatilityIds));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}