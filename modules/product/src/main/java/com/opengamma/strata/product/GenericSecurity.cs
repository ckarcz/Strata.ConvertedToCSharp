﻿using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BondFutureOptionSecurity = com.opengamma.strata.product.bond.BondFutureOptionSecurity;
	using FixedCouponBondSecurity = com.opengamma.strata.product.bond.FixedCouponBondSecurity;
	using IborFutureSecurity = com.opengamma.strata.product.index.IborFutureSecurity;

	/// <summary>
	/// A generic security, defined in terms of the value of each tick.
	/// <para>
	/// In most cases, applications will choose to represent information about securities
	/// using the relevant type, such as <seealso cref="FixedCouponBondSecurity"/>,
	/// <seealso cref="BondFutureOptionSecurity"/> or <seealso cref="IborFutureSecurity"/>.
	/// Sometimes however, it can be useful to store minimal information about the security,
	/// expressing just the tick size and tick value.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class GenericSecurity implements Security, SecuritizedProduct, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class GenericSecurity : Security, SecuritizedProduct, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final SecurityInfo info;
		private readonly SecurityInfo info;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from security information, tick size and tick value.
	  /// </summary>
	  /// <param name="securityInfo">  the security information </param>
	  /// <returns> the security </returns>
	  public static GenericSecurity of(SecurityInfo securityInfo)
	  {
		return new GenericSecurity(securityInfo);
	  }

	  //-------------------------------------------------------------------------
	  public SecurityId SecurityId
	  {
		  get
		  {
			return Security.this.SecurityId;
		  }
	  }

	  public Currency Currency
	  {
		  get
		  {
			return Security.this.Currency;
		  }
	  }

	  public ImmutableSet<SecurityId> UnderlyingIds
	  {
		  get
		  {
			return ImmutableSet.of();
		  }
	  }

	  //-------------------------------------------------------------------------
	  public GenericSecurity withInfo(SecurityInfo info)
	  {
		return new GenericSecurity(info);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates the associated product, which simply returns {@code this}.
	  /// <para>
	  /// The product associated with a security normally returns the financial model used for pricing.
	  /// In the case of a {@code GenericSecurity}, no underlying financial model is available.
	  /// As such, the {@code GenericSecurity} is the product.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="refData">  the reference data to use </param>
	  /// <returns> this security </returns>
	  public GenericSecurity createProduct(ReferenceData refData)
	  {
		return this;
	  }

	  public GenericSecurityTrade createTrade(TradeInfo info, double quantity, double tradePrice, ReferenceData refData)
	  {
		return new GenericSecurityTrade(info, this, quantity, tradePrice);
	  }

	  public GenericSecurityPosition createPosition(PositionInfo tradeInfo, double quantity, ReferenceData refData)
	  {
		return GenericSecurityPosition.ofNet(tradeInfo, this, quantity);
	  }

	  public GenericSecurityPosition createPosition(PositionInfo positionInfo, double longQuantity, double shortQuantity, ReferenceData refData)
	  {

		return GenericSecurityPosition.ofLongShort(positionInfo, this, longQuantity, shortQuantity);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code GenericSecurity}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static GenericSecurity.Meta meta()
	  {
		return GenericSecurity.Meta.INSTANCE;
	  }

	  static GenericSecurity()
	  {
		MetaBean.register(GenericSecurity.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private GenericSecurity(SecurityInfo info)
	  {
		JodaBeanUtils.notNull(info, "info");
		this.info = info;
	  }

	  public override GenericSecurity.Meta metaBean()
	  {
		return GenericSecurity.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the standard security information.
	  /// <para>
	  /// This includes the security identifier.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SecurityInfo Info
	  {
		  get
		  {
			return info;
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
		  GenericSecurity other = (GenericSecurity) obj;
		  return JodaBeanUtils.equal(info, other.info);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(info);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(64);
		buf.Append("GenericSecurity{");
		buf.Append("info").Append('=').Append(JodaBeanUtils.ToString(info));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code GenericSecurity}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  info_Renamed = DirectMetaProperty.ofImmutable(this, "info", typeof(GenericSecurity), typeof(SecurityInfo));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "info");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code info} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SecurityInfo> info_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "info");
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
			case 3237038: // info
			  return info_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends GenericSecurity> builder()
		public override BeanBuilder<GenericSecurity> builder()
		{
		  return new GenericSecurity.Builder();
		}

		public override Type beanType()
		{
		  return typeof(GenericSecurity);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code info} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SecurityInfo> info()
		{
		  return info_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  return ((GenericSecurity) bean).Info;
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
	  /// The bean-builder for {@code GenericSecurity}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<GenericSecurity>
	  {

		internal SecurityInfo info;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  return info;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  this.info = (SecurityInfo) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override GenericSecurity build()
		{
		  return new GenericSecurity(info);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(64);
		  buf.Append("GenericSecurity.Builder{");
		  buf.Append("info").Append('=').Append(JodaBeanUtils.ToString(info));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}