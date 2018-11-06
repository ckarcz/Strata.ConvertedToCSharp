﻿using System;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.swaption
{

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MarketData = com.opengamma.strata.data.MarketData;
	using SwaptionVolatilities = com.opengamma.strata.pricer.swaption.SwaptionVolatilities;

	/// <summary>
	/// The default market data for swaptions.
	/// <para>
	/// This uses a <seealso cref="SwaptionMarketDataLookup"/> to provide a view on <seealso cref="MarketData"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") final class DefaultSwaptionMarketData implements SwaptionMarketData, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	internal sealed class DefaultSwaptionMarketData : SwaptionMarketData, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final SwaptionMarketDataLookup lookup;
		private readonly SwaptionMarketDataLookup lookup;
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
	  public static DefaultSwaptionMarketData of(SwaptionMarketDataLookup lookup, MarketData marketData)
	  {

		return new DefaultSwaptionMarketData(lookup, marketData);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private DefaultSwaptionMarketData(SwaptionMarketDataLookup lookup, com.opengamma.strata.data.MarketData marketData)
	  private DefaultSwaptionMarketData(SwaptionMarketDataLookup lookup, MarketData marketData)
	  {

		this.lookup = ArgChecker.notNull(lookup, "lookup");
		this.marketData = ArgChecker.notNull(marketData, "marketData");
	  }

	  //-------------------------------------------------------------------------
	  public SwaptionMarketData withMarketData(MarketData marketData)
	  {
		return DefaultSwaptionMarketData.of(lookup, marketData);
	  }

	  //-------------------------------------------------------------------------
	  public SwaptionVolatilities volatilities(IborIndex index)
	  {
		return lookup.volatilities(index, marketData);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DefaultSwaptionMarketData}.
	  /// </summary>
	  private static readonly TypedMetaBean<DefaultSwaptionMarketData> META_BEAN = LightMetaBean.of(typeof(DefaultSwaptionMarketData), MethodHandles.lookup(), new string[] {"lookup", "marketData"}, new object[0]);

	  /// <summary>
	  /// The meta-bean for {@code DefaultSwaptionMarketData}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<DefaultSwaptionMarketData> meta()
	  {
		return META_BEAN;
	  }

	  static DefaultSwaptionMarketData()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override TypedMetaBean<DefaultSwaptionMarketData> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the lookup. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SwaptionMarketDataLookup Lookup
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
		  DefaultSwaptionMarketData other = (DefaultSwaptionMarketData) obj;
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
		buf.Append("DefaultSwaptionMarketData{");
		buf.Append("lookup").Append('=').Append(lookup).Append(',').Append(' ');
		buf.Append("marketData").Append('=').Append(JodaBeanUtils.ToString(marketData));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}