﻿using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using NamedMarketDataId = com.opengamma.strata.data.NamedMarketDataId;

	/// <summary>
	/// An identifier used to access bond future volatilities by name.
	/// <para>
	/// This is used when there is a need to obtain an instance of <seealso cref="BondFutureVolatilities"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light", cacheHashCode = true) public final class BondFutureVolatilitiesId implements com.opengamma.strata.data.NamedMarketDataId<BondFutureVolatilities>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class BondFutureVolatilitiesId : NamedMarketDataId<BondFutureVolatilities>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final BondFutureVolatilitiesName name;
		private readonly BondFutureVolatilitiesName name;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an identifier used to find bond future volatilities.
	  /// </summary>
	  /// <param name="name">  the name </param>
	  /// <returns> an identifier for the volatilities </returns>
	  public static BondFutureVolatilitiesId of(string name)
	  {
		return new BondFutureVolatilitiesId(BondFutureVolatilitiesName.of(name));
	  }

	  /// <summary>
	  /// Obtains an identifier used to find bond future volatilities.
	  /// </summary>
	  /// <param name="name">  the name </param>
	  /// <returns> an identifier for the volatilities </returns>
	  public static BondFutureVolatilitiesId of(BondFutureVolatilitiesName name)
	  {
		return new BondFutureVolatilitiesId(name);
	  }

	  //-------------------------------------------------------------------------
	  public override Type<BondFutureVolatilities> MarketDataType
	  {
		  get
		  {
			return typeof(BondFutureVolatilities);
		  }
	  }

	  public MarketDataName<BondFutureVolatilities> MarketDataName
	  {
		  get
		  {
			return name;
		  }
	  }

	  public override string ToString()
	  {
		return "BondFutureVolatilitiesId:" + name;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code BondFutureVolatilitiesId}.
	  /// </summary>
	  private static readonly TypedMetaBean<BondFutureVolatilitiesId> META_BEAN = LightMetaBean.of(typeof(BondFutureVolatilitiesId), MethodHandles.lookup(), new string[] {"name"}, new object[0]);

	  /// <summary>
	  /// The meta-bean for {@code BondFutureVolatilitiesId}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<BondFutureVolatilitiesId> meta()
	  {
		return META_BEAN;
	  }

	  static BondFutureVolatilitiesId()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// The cached hash code, using the racy single-check idiom.
	  /// </summary>
	  [NonSerialized]
	  private int cacheHashCode;

	  private BondFutureVolatilitiesId(BondFutureVolatilitiesName name)
	  {
		JodaBeanUtils.notNull(name, "name");
		this.name = name;
	  }

	  public override TypedMetaBean<BondFutureVolatilitiesId> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name of the volatilities. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public BondFutureVolatilitiesName Name
	  {
		  get
		  {
			return name;
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
		  BondFutureVolatilitiesId other = (BondFutureVolatilitiesId) obj;
		  return JodaBeanUtils.equal(name, other.name);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = cacheHashCode;
		if (hash == 0)
		{
		  hash = this.GetType().GetHashCode();
		  hash = hash * 31 + JodaBeanUtils.GetHashCode(name);
		  cacheHashCode = hash;
		}
		return hash;
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}