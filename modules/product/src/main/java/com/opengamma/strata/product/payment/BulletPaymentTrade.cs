﻿using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.payment
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using SummarizerUtils = com.opengamma.strata.product.common.SummarizerUtils;

	/// <summary>
	/// A bullet payment trade.
	/// <para>
	/// An Over-The-Counter (OTC) trade where one party makes a payment to another.
	/// The reason for the payment is not captured.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class BulletPaymentTrade implements com.opengamma.strata.product.ProductTrade, com.opengamma.strata.product.ResolvableTrade<ResolvedBulletPaymentTrade>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class BulletPaymentTrade : ProductTrade, ResolvableTrade<ResolvedBulletPaymentTrade>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.product.TradeInfo info;
		private readonly TradeInfo info;
	  /// <summary>
	  /// The product that was agreed when the trade occurred.
	  /// <para>
	  /// The product captures the contracted financial details of the trade.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final BulletPayment product;
	  private readonly BulletPayment product;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance of a Bullet Payment trade.
	  /// </summary>
	  /// <param name="info">  the trade info </param>
	  /// <param name="product">  the product </param>
	  /// <returns> the trade </returns>
	  public static BulletPaymentTrade of(TradeInfo info, BulletPayment product)
	  {
		return new BulletPaymentTrade(info, product);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.info_Renamed = TradeInfo.empty();
	  }

	  //-------------------------------------------------------------------------
	  public BulletPaymentTrade withInfo(TradeInfo info)
	  {
		return new BulletPaymentTrade(info, product);
	  }

	  //-------------------------------------------------------------------------
	  public PortfolioItemSummary summarize()
	  {
		// Pay USD 2mm : 21Jan18
		StringBuilder buf = new StringBuilder(64);
		buf.Append(product.PayReceive);
		buf.Append(' ');
		buf.Append(SummarizerUtils.amount(product.Value));
		buf.Append(" : ");
		buf.Append(SummarizerUtils.date(product.Date.Unadjusted));
		return SummarizerUtils.summary(this, ProductType.BULLET_PAYMENT, buf.ToString(), product.Currency);
	  }

	  public ResolvedBulletPaymentTrade resolve(ReferenceData refData)
	  {
		return ResolvedBulletPaymentTrade.of(info, product.resolve(refData));
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code BulletPaymentTrade}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static BulletPaymentTrade.Meta meta()
	  {
		return BulletPaymentTrade.Meta.INSTANCE;
	  }

	  static BulletPaymentTrade()
	  {
		MetaBean.register(BulletPaymentTrade.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static BulletPaymentTrade.Builder builder()
	  {
		return new BulletPaymentTrade.Builder();
	  }

	  private BulletPaymentTrade(TradeInfo info, BulletPayment product)
	  {
		JodaBeanUtils.notNull(info, "info");
		JodaBeanUtils.notNull(product, "product");
		this.info = info;
		this.product = product;
	  }

	  public override BulletPaymentTrade.Meta metaBean()
	  {
		return BulletPaymentTrade.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the additional trade information, defaulted to an empty instance.
	  /// <para>
	  /// This allows additional information to be attached to the trade.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public TradeInfo Info
	  {
		  get
		  {
			return info;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the product that was agreed when the trade occurred.
	  /// <para>
	  /// The product captures the contracted financial details of the trade.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public BulletPayment Product
	  {
		  get
		  {
			return product;
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
		  BulletPaymentTrade other = (BulletPaymentTrade) obj;
		  return JodaBeanUtils.equal(info, other.info) && JodaBeanUtils.equal(product, other.product);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(info);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(product);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("BulletPaymentTrade{");
		buf.Append("info").Append('=').Append(info).Append(',').Append(' ');
		buf.Append("product").Append('=').Append(JodaBeanUtils.ToString(product));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code BulletPaymentTrade}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  info_Renamed = DirectMetaProperty.ofImmutable(this, "info", typeof(BulletPaymentTrade), typeof(TradeInfo));
			  product_Renamed = DirectMetaProperty.ofImmutable(this, "product", typeof(BulletPaymentTrade), typeof(BulletPayment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "info", "product");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code info} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<TradeInfo> info_Renamed;
		/// <summary>
		/// The meta-property for the {@code product} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BulletPayment> product_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "info", "product");
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
			case -309474065: // product
			  return product_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override BulletPaymentTrade.Builder builder()
		{
		  return new BulletPaymentTrade.Builder();
		}

		public override Type beanType()
		{
		  return typeof(BulletPaymentTrade);
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
		public MetaProperty<TradeInfo> info()
		{
		  return info_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code product} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BulletPayment> product()
		{
		  return product_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  return ((BulletPaymentTrade) bean).Info;
			case -309474065: // product
			  return ((BulletPaymentTrade) bean).Product;
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
	  /// The bean-builder for {@code BulletPaymentTrade}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<BulletPaymentTrade>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal TradeInfo info_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BulletPayment product_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		  applyDefaults(this);
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(BulletPaymentTrade beanToCopy)
		{
		  this.info_Renamed = beanToCopy.Info;
		  this.product_Renamed = beanToCopy.Product;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  return info_Renamed;
			case -309474065: // product
			  return product_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  this.info_Renamed = (TradeInfo) newValue;
			  break;
			case -309474065: // product
			  this.product_Renamed = (BulletPayment) newValue;
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

		public override BulletPaymentTrade build()
		{
		  return new BulletPaymentTrade(info_Renamed, product_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the additional trade information, defaulted to an empty instance.
		/// <para>
		/// This allows additional information to be attached to the trade.
		/// </para>
		/// </summary>
		/// <param name="info">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder info(TradeInfo info)
		{
		  JodaBeanUtils.notNull(info, "info");
		  this.info_Renamed = info;
		  return this;
		}

		/// <summary>
		/// Sets the product that was agreed when the trade occurred.
		/// <para>
		/// The product captures the contracted financial details of the trade.
		/// </para>
		/// </summary>
		/// <param name="product">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder product(BulletPayment product)
		{
		  JodaBeanUtils.notNull(product, "product");
		  this.product_Renamed = product;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("BulletPaymentTrade.Builder{");
		  buf.Append("info").Append('=').Append(JodaBeanUtils.ToString(info_Renamed)).Append(',').Append(' ');
		  buf.Append("product").Append('=').Append(JodaBeanUtils.ToString(product_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}