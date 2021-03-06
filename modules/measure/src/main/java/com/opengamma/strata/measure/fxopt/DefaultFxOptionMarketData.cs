﻿using System;
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
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MarketData = com.opengamma.strata.data.MarketData;
	using FxOptionVolatilities = com.opengamma.strata.pricer.fxopt.FxOptionVolatilities;

	/// <summary>
	/// The default market data for FX options.
	/// <para>
	/// This uses a <seealso cref="FxOptionMarketDataLookup"/> to provide a view on <seealso cref="MarketData"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") final class DefaultFxOptionMarketData implements FxOptionMarketData, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	internal sealed class DefaultFxOptionMarketData : FxOptionMarketData, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final FxOptionMarketDataLookup lookup;
		private readonly FxOptionMarketDataLookup lookup;
	  /// <summary>
	  /// The market data.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.data.MarketData marketData;
	  private readonly MarketData marketData;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance based on a lookup and market data.
	  /// <para>
	  /// The lookup knows how to obtain the volatilities from the market data.
	  /// This might involve accessing a surface or a cube.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="lookup">  the lookup </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the rates market view </returns>
	  public static DefaultFxOptionMarketData of(FxOptionMarketDataLookup lookup, MarketData marketData)
	  {

		return new DefaultFxOptionMarketData(lookup, marketData);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private DefaultFxOptionMarketData(FxOptionMarketDataLookup lookup, com.opengamma.strata.data.MarketData marketData)
	  private DefaultFxOptionMarketData(FxOptionMarketDataLookup lookup, MarketData marketData)
	  {

		this.lookup = ArgChecker.notNull(lookup, "lookup");
		this.marketData = ArgChecker.notNull(marketData, "marketData");
	  }

	  //-------------------------------------------------------------------------
	  public FxOptionMarketData withMarketData(MarketData marketData)
	  {
		return DefaultFxOptionMarketData.of(lookup, marketData);
	  }

	  //-------------------------------------------------------------------------
	  public FxOptionVolatilities volatilities(CurrencyPair currencyPair)
	  {
		return lookup.volatilities(currencyPair, marketData);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DefaultFxOptionMarketData}.
	  /// </summary>
	  private static readonly TypedMetaBean<DefaultFxOptionMarketData> META_BEAN = LightMetaBean.of(typeof(DefaultFxOptionMarketData), MethodHandles.lookup(), new string[] {"lookup", "marketData"}, new object[0]);

	  /// <summary>
	  /// The meta-bean for {@code DefaultFxOptionMarketData}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<DefaultFxOptionMarketData> meta()
	  {
		return META_BEAN;
	  }

	  static DefaultFxOptionMarketData()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override TypedMetaBean<DefaultFxOptionMarketData> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the lookup. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FxOptionMarketDataLookup Lookup
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
		  DefaultFxOptionMarketData other = (DefaultFxOptionMarketData) obj;
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
		buf.Append("DefaultFxOptionMarketData{");
		buf.Append("lookup").Append('=').Append(lookup).Append(',').Append(' ');
		buf.Append("marketData").Append('=').Append(JodaBeanUtils.ToString(marketData));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}