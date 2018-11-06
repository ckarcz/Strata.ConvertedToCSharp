using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.etd
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

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using SummarizerUtils = com.opengamma.strata.product.common.SummarizerUtils;

	/// <summary>
	/// A trade representing an ETD option.
	/// <para>
	/// A trade in an underlying <seealso cref="EtdOptionSecurity"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class EtdOptionTrade implements com.opengamma.strata.product.ResolvableSecurityTrade, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class EtdOptionTrade : ResolvableSecurityTrade, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.product.TradeInfo info;
		private readonly TradeInfo info;
	  /// <summary>
	  /// The security that was traded.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final EtdOptionSecurity security;
	  private readonly EtdOptionSecurity security;
	  /// <summary>
	  /// The quantity that was traded.
	  /// <para>
	  /// This is the number of contracts that were traded.
	  /// This will be positive if buying and negative if selling.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(overrideGet = true) private final double quantity;
	  private readonly double quantity;
	  /// <summary>
	  /// The price that was traded, in decimal form.
	  /// <para>
	  /// This is the price agreed when the trade occurred.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(overrideGet = true) private final double price;
	  private readonly double price;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from trade information, security, quantity and price.
	  /// </summary>
	  /// <param name="tradeInfo">  the trade information </param>
	  /// <param name="security">  the security that was traded </param>
	  /// <param name="quantity">  the quantity that was traded </param>
	  /// <param name="price">  the price that was traded </param>
	  /// <returns> the trade </returns>
	  public static EtdOptionTrade of(TradeInfo tradeInfo, EtdOptionSecurity security, double quantity, double price)
	  {

		return new EtdOptionTrade(tradeInfo, security, quantity, price);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.info_Renamed = TradeInfo.empty();
	  }

	  //-------------------------------------------------------------------------
	  public PortfolioItemSummary summarize()
	  {
		// O-ECAG-OGBS-201706-P1.50 x 200, Jun17 P1.50
		string option = security.summaryDescription();
		string description = SecurityId.StandardId.Value + " x " + SummarizerUtils.value(Quantity) + ", " + option;
		return SummarizerUtils.summary(this, ProductType.ETD_OPTION, description, Currency);
	  }

	  public SecurityId SecurityId
	  {
		  get
		  {
			return security.SecurityId;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public EtdOptionTrade withInfo(TradeInfo info)
	  {
		return new EtdOptionTrade(info, security, quantity, price);
	  }

	  public EtdOptionTrade withQuantity(double quantity)
	  {
		return new EtdOptionTrade(info, security, quantity, price);
	  }

	  public EtdOptionTrade withPrice(double price)
	  {
		return new EtdOptionTrade(info, security, quantity, price);
	  }

	  /// <summary>
	  /// Gets the currency of the trade.
	  /// <para>
	  /// This is typically the same as the currency of the product.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the trading currency </returns>
	  public Currency Currency
	  {
		  get
		  {
			return security.Currency;
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code EtdOptionTrade}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static EtdOptionTrade.Meta meta()
	  {
		return EtdOptionTrade.Meta.INSTANCE;
	  }

	  static EtdOptionTrade()
	  {
		MetaBean.register(EtdOptionTrade.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static EtdOptionTrade.Builder builder()
	  {
		return new EtdOptionTrade.Builder();
	  }

	  private EtdOptionTrade(TradeInfo info, EtdOptionSecurity security, double quantity, double price)
	  {
		JodaBeanUtils.notNull(info, "info");
		JodaBeanUtils.notNull(security, "security");
		this.info = info;
		this.security = security;
		this.quantity = quantity;
		this.price = price;
	  }

	  public override EtdOptionTrade.Meta metaBean()
	  {
		return EtdOptionTrade.Meta.INSTANCE;
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
	  /// Gets the security that was traded. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public EtdOptionSecurity Security
	  {
		  get
		  {
			return security;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the quantity that was traded.
	  /// <para>
	  /// This is the number of contracts that were traded.
	  /// This will be positive if buying and negative if selling.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double Quantity
	  {
		  get
		  {
			return quantity;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the price that was traded, in decimal form.
	  /// <para>
	  /// This is the price agreed when the trade occurred.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double Price
	  {
		  get
		  {
			return price;
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
		  EtdOptionTrade other = (EtdOptionTrade) obj;
		  return JodaBeanUtils.equal(info, other.info) && JodaBeanUtils.equal(security, other.security) && JodaBeanUtils.equal(quantity, other.quantity) && JodaBeanUtils.equal(price, other.price);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(info);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(security);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(quantity);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(price);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("EtdOptionTrade{");
		buf.Append("info").Append('=').Append(info).Append(',').Append(' ');
		buf.Append("security").Append('=').Append(security).Append(',').Append(' ');
		buf.Append("quantity").Append('=').Append(quantity).Append(',').Append(' ');
		buf.Append("price").Append('=').Append(JodaBeanUtils.ToString(price));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code EtdOptionTrade}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  info_Renamed = DirectMetaProperty.ofImmutable(this, "info", typeof(EtdOptionTrade), typeof(TradeInfo));
			  security_Renamed = DirectMetaProperty.ofImmutable(this, "security", typeof(EtdOptionTrade), typeof(EtdOptionSecurity));
			  quantity_Renamed = DirectMetaProperty.ofImmutable(this, "quantity", typeof(EtdOptionTrade), Double.TYPE);
			  price_Renamed = DirectMetaProperty.ofImmutable(this, "price", typeof(EtdOptionTrade), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "info", "security", "quantity", "price");
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
		/// The meta-property for the {@code security} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<EtdOptionSecurity> security_Renamed;
		/// <summary>
		/// The meta-property for the {@code quantity} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> quantity_Renamed;
		/// <summary>
		/// The meta-property for the {@code price} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> price_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "info", "security", "quantity", "price");
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
			case 949122880: // security
			  return security_Renamed;
			case -1285004149: // quantity
			  return quantity_Renamed;
			case 106934601: // price
			  return price_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override EtdOptionTrade.Builder builder()
		{
		  return new EtdOptionTrade.Builder();
		}

		public override Type beanType()
		{
		  return typeof(EtdOptionTrade);
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
		/// The meta-property for the {@code security} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<EtdOptionSecurity> security()
		{
		  return security_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code quantity} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> quantity()
		{
		  return quantity_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code price} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> price()
		{
		  return price_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  return ((EtdOptionTrade) bean).Info;
			case 949122880: // security
			  return ((EtdOptionTrade) bean).Security;
			case -1285004149: // quantity
			  return ((EtdOptionTrade) bean).Quantity;
			case 106934601: // price
			  return ((EtdOptionTrade) bean).Price;
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
	  /// The bean-builder for {@code EtdOptionTrade}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<EtdOptionTrade>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal TradeInfo info_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal EtdOptionSecurity security_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double quantity_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double price_Renamed;

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
		internal Builder(EtdOptionTrade beanToCopy)
		{
		  this.info_Renamed = beanToCopy.Info;
		  this.security_Renamed = beanToCopy.Security;
		  this.quantity_Renamed = beanToCopy.Quantity;
		  this.price_Renamed = beanToCopy.Price;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  return info_Renamed;
			case 949122880: // security
			  return security_Renamed;
			case -1285004149: // quantity
			  return quantity_Renamed;
			case 106934601: // price
			  return price_Renamed;
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
			case 949122880: // security
			  this.security_Renamed = (EtdOptionSecurity) newValue;
			  break;
			case -1285004149: // quantity
			  this.quantity_Renamed = (double?) newValue.Value;
			  break;
			case 106934601: // price
			  this.price_Renamed = (double?) newValue.Value;
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

		public override EtdOptionTrade build()
		{
		  return new EtdOptionTrade(info_Renamed, security_Renamed, quantity_Renamed, price_Renamed);
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
		/// Sets the security that was traded. </summary>
		/// <param name="security">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder security(EtdOptionSecurity security)
		{
		  JodaBeanUtils.notNull(security, "security");
		  this.security_Renamed = security;
		  return this;
		}

		/// <summary>
		/// Sets the quantity that was traded.
		/// <para>
		/// This is the number of contracts that were traded.
		/// This will be positive if buying and negative if selling.
		/// </para>
		/// </summary>
		/// <param name="quantity">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder quantity(double quantity)
		{
		  this.quantity_Renamed = quantity;
		  return this;
		}

		/// <summary>
		/// Sets the price that was traded, in decimal form.
		/// <para>
		/// This is the price agreed when the trade occurred.
		/// </para>
		/// </summary>
		/// <param name="price">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder price(double price)
		{
		  this.price_Renamed = price;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("EtdOptionTrade.Builder{");
		  buf.Append("info").Append('=').Append(JodaBeanUtils.ToString(info_Renamed)).Append(',').Append(' ');
		  buf.Append("security").Append('=').Append(JodaBeanUtils.ToString(security_Renamed)).Append(',').Append(' ');
		  buf.Append("quantity").Append('=').Append(JodaBeanUtils.ToString(quantity_Renamed)).Append(',').Append(' ');
		  buf.Append("price").Append('=').Append(JodaBeanUtils.ToString(price_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}