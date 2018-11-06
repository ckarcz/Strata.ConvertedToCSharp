using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.index
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

	/// <summary>
	/// A trade in a futures contract based on an Ibor index, resolved for pricing.
	/// <para>
	/// This is the resolved form of <seealso cref="IborFutureTrade"/> and is the primary input to the pricers.
	/// Applications will typically create a {@code ResolvedIborFutureTrade} from a {@code IborFutureTrade}
	/// using <seealso cref="IborFutureTrade#resolve(ReferenceData)"/>.
	/// </para>
	/// <para>
	/// A {@code ResolvedIborFutureTrade} is bound to data that changes over time, such as holiday calendars.
	/// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// 
	/// <h4>Price</h4>
	/// The price of an Ibor future is based on the interest rate of the underlying index.
	/// It is defined as {@code (100 - percentRate)}.
	/// </para>
	/// <para>
	/// Strata uses <i>decimal prices</i> for Ibor futures in the trade model, pricers and market data.
	/// The decimal price is based on the decimal rate equivalent to the percentage.
	/// For example, a price of 99.32 implies an interest rate of 0.68% which is represented in Strata by 0.9932.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(constructorScope = "package") public final class ResolvedIborFutureTrade implements com.opengamma.strata.product.ResolvedTrade, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ResolvedIborFutureTrade : ResolvedTrade, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.product.PortfolioItemInfo info;
		private readonly PortfolioItemInfo info;
	  /// <summary>
	  /// The future that was traded.
	  /// <para>
	  /// The product captures the contracted financial details of the trade.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final ResolvedIborFuture product;
	  private readonly ResolvedIborFuture product;
	  /// <summary>
	  /// The quantity that was traded.
	  /// <para>
	  /// This is the number of contracts that were traded.
	  /// This will be positive if buying and negative if selling.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double quantity;
	  private readonly double quantity;
	  /// <summary>
	  /// The price that was traded, together with the trade date, optional.
	  /// <para>
	  /// This is the price agreed when the trade occurred, in decimal form.
	  /// Strata uses <i>decimal prices</i> for Ibor futures in the trade model, pricers and market data.
	  /// The decimal price is based on the decimal rate equivalent to the percentage.
	  /// For example, a price of 99.32 implies an interest rate of 0.68% which is represented in Strata by 0.9932.
	  /// </para>
	  /// <para>
	  /// This is optional to allow the class to be used to price both trades and positions.
	  /// When the instance represents a trade, the traded price should be present.
	  /// When the instance represents a position, the traded price should be empty.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.product.TradedPrice tradedPrice;
	  private readonly TradedPrice tradedPrice;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.info_Renamed = TradeInfo.empty();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedIborFutureTrade}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ResolvedIborFutureTrade.Meta meta()
	  {
		return ResolvedIborFutureTrade.Meta.INSTANCE;
	  }

	  static ResolvedIborFutureTrade()
	  {
		MetaBean.register(ResolvedIborFutureTrade.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ResolvedIborFutureTrade.Builder builder()
	  {
		return new ResolvedIborFutureTrade.Builder();
	  }

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="info">  the value of the property, not null </param>
	  /// <param name="product">  the value of the property, not null </param>
	  /// <param name="quantity">  the value of the property </param>
	  /// <param name="tradedPrice">  the value of the property </param>
	  internal ResolvedIborFutureTrade(PortfolioItemInfo info, ResolvedIborFuture product, double quantity, TradedPrice tradedPrice)
	  {
		JodaBeanUtils.notNull(info, "info");
		JodaBeanUtils.notNull(product, "product");
		this.info = info;
		this.product = product;
		this.quantity = quantity;
		this.tradedPrice = tradedPrice;
	  }

	  public override ResolvedIborFutureTrade.Meta metaBean()
	  {
		return ResolvedIborFutureTrade.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the additional information, defaulted to an empty instance.
	  /// <para>
	  /// This allows additional information to be attached.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PortfolioItemInfo Info
	  {
		  get
		  {
			return info;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the future that was traded.
	  /// <para>
	  /// The product captures the contracted financial details of the trade.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ResolvedIborFuture Product
	  {
		  get
		  {
			return product;
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
	  /// Gets the price that was traded, together with the trade date, optional.
	  /// <para>
	  /// This is the price agreed when the trade occurred, in decimal form.
	  /// Strata uses <i>decimal prices</i> for Ibor futures in the trade model, pricers and market data.
	  /// The decimal price is based on the decimal rate equivalent to the percentage.
	  /// For example, a price of 99.32 implies an interest rate of 0.68% which is represented in Strata by 0.9932.
	  /// </para>
	  /// <para>
	  /// This is optional to allow the class to be used to price both trades and positions.
	  /// When the instance represents a trade, the traded price should be present.
	  /// When the instance represents a position, the traded price should be empty.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<TradedPrice> TradedPrice
	  {
		  get
		  {
			return Optional.ofNullable(tradedPrice);
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
		  ResolvedIborFutureTrade other = (ResolvedIborFutureTrade) obj;
		  return JodaBeanUtils.equal(info, other.info) && JodaBeanUtils.equal(product, other.product) && JodaBeanUtils.equal(quantity, other.quantity) && JodaBeanUtils.equal(tradedPrice, other.tradedPrice);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(info);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(product);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(quantity);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(tradedPrice);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("ResolvedIborFutureTrade{");
		buf.Append("info").Append('=').Append(info).Append(',').Append(' ');
		buf.Append("product").Append('=').Append(product).Append(',').Append(' ');
		buf.Append("quantity").Append('=').Append(quantity).Append(',').Append(' ');
		buf.Append("tradedPrice").Append('=').Append(JodaBeanUtils.ToString(tradedPrice));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedIborFutureTrade}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  info_Renamed = DirectMetaProperty.ofImmutable(this, "info", typeof(ResolvedIborFutureTrade), typeof(PortfolioItemInfo));
			  product_Renamed = DirectMetaProperty.ofImmutable(this, "product", typeof(ResolvedIborFutureTrade), typeof(ResolvedIborFuture));
			  quantity_Renamed = DirectMetaProperty.ofImmutable(this, "quantity", typeof(ResolvedIborFutureTrade), Double.TYPE);
			  tradedPrice_Renamed = DirectMetaProperty.ofImmutable(this, "tradedPrice", typeof(ResolvedIborFutureTrade), typeof(TradedPrice));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "info", "product", "quantity", "tradedPrice");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code info} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PortfolioItemInfo> info_Renamed;
		/// <summary>
		/// The meta-property for the {@code product} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ResolvedIborFuture> product_Renamed;
		/// <summary>
		/// The meta-property for the {@code quantity} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> quantity_Renamed;
		/// <summary>
		/// The meta-property for the {@code tradedPrice} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<TradedPrice> tradedPrice_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "info", "product", "quantity", "tradedPrice");
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
			case -1285004149: // quantity
			  return quantity_Renamed;
			case -1873824343: // tradedPrice
			  return tradedPrice_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ResolvedIborFutureTrade.Builder builder()
		{
		  return new ResolvedIborFutureTrade.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ResolvedIborFutureTrade);
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
		public MetaProperty<PortfolioItemInfo> info()
		{
		  return info_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code product} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ResolvedIborFuture> product()
		{
		  return product_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code quantity} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> quantity()
		{
		  return quantity_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code tradedPrice} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<TradedPrice> tradedPrice()
		{
		  return tradedPrice_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  return ((ResolvedIborFutureTrade) bean).Info;
			case -309474065: // product
			  return ((ResolvedIborFutureTrade) bean).Product;
			case -1285004149: // quantity
			  return ((ResolvedIborFutureTrade) bean).Quantity;
			case -1873824343: // tradedPrice
			  return ((ResolvedIborFutureTrade) bean).tradedPrice;
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
	  /// The bean-builder for {@code ResolvedIborFutureTrade}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ResolvedIborFutureTrade>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PortfolioItemInfo info_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ResolvedIborFuture product_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double quantity_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal TradedPrice tradedPrice_Renamed;

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
		internal Builder(ResolvedIborFutureTrade beanToCopy)
		{
		  this.info_Renamed = beanToCopy.Info;
		  this.product_Renamed = beanToCopy.Product;
		  this.quantity_Renamed = beanToCopy.Quantity;
		  this.tradedPrice_Renamed = beanToCopy.tradedPrice;
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
			case -1285004149: // quantity
			  return quantity_Renamed;
			case -1873824343: // tradedPrice
			  return tradedPrice_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  this.info_Renamed = (PortfolioItemInfo) newValue;
			  break;
			case -309474065: // product
			  this.product_Renamed = (ResolvedIborFuture) newValue;
			  break;
			case -1285004149: // quantity
			  this.quantity_Renamed = (double?) newValue.Value;
			  break;
			case -1873824343: // tradedPrice
			  this.tradedPrice_Renamed = (TradedPrice) newValue;
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

		public override ResolvedIborFutureTrade build()
		{
		  return new ResolvedIborFutureTrade(info_Renamed, product_Renamed, quantity_Renamed, tradedPrice_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the additional information, defaulted to an empty instance.
		/// <para>
		/// This allows additional information to be attached.
		/// </para>
		/// </summary>
		/// <param name="info">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder info(PortfolioItemInfo info)
		{
		  JodaBeanUtils.notNull(info, "info");
		  this.info_Renamed = info;
		  return this;
		}

		/// <summary>
		/// Sets the future that was traded.
		/// <para>
		/// The product captures the contracted financial details of the trade.
		/// </para>
		/// </summary>
		/// <param name="product">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder product(ResolvedIborFuture product)
		{
		  JodaBeanUtils.notNull(product, "product");
		  this.product_Renamed = product;
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
		/// Sets the price that was traded, together with the trade date, optional.
		/// <para>
		/// This is the price agreed when the trade occurred, in decimal form.
		/// Strata uses <i>decimal prices</i> for Ibor futures in the trade model, pricers and market data.
		/// The decimal price is based on the decimal rate equivalent to the percentage.
		/// For example, a price of 99.32 implies an interest rate of 0.68% which is represented in Strata by 0.9932.
		/// </para>
		/// <para>
		/// This is optional to allow the class to be used to price both trades and positions.
		/// When the instance represents a trade, the traded price should be present.
		/// When the instance represents a position, the traded price should be empty.
		/// </para>
		/// </summary>
		/// <param name="tradedPrice">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder tradedPrice(TradedPrice tradedPrice)
		{
		  this.tradedPrice_Renamed = tradedPrice;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("ResolvedIborFutureTrade.Builder{");
		  buf.Append("info").Append('=').Append(JodaBeanUtils.ToString(info_Renamed)).Append(',').Append(' ');
		  buf.Append("product").Append('=').Append(JodaBeanUtils.ToString(product_Renamed)).Append(',').Append(' ');
		  buf.Append("quantity").Append('=').Append(JodaBeanUtils.ToString(quantity_Renamed)).Append(',').Append(' ');
		  buf.Append("tradedPrice").Append('=').Append(JodaBeanUtils.ToString(tradedPrice_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}