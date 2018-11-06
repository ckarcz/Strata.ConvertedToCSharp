using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
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
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using SummarizerUtils = com.opengamma.strata.product.common.SummarizerUtils;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;

	/// <summary>
	/// A trade representing a capital indexed bond.
	/// <para>
	/// A trade in an underlying <seealso cref="CapitalIndexedBond"/>.
	/// 
	/// <h4>Price</h4>
	/// Strata uses <i>decimal prices</i> for bonds in the trade model, pricers and market data.
	/// For example, a price of 99.32% is represented in Strata by 0.9932.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(constructorScope = "package") public final class CapitalIndexedBondTrade implements com.opengamma.strata.product.SecuritizedProductTrade<CapitalIndexedBond>, com.opengamma.strata.product.ResolvableTrade<ResolvedCapitalIndexedBondTrade>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CapitalIndexedBondTrade : SecuritizedProductTrade<CapitalIndexedBond>, ResolvableTrade<ResolvedCapitalIndexedBondTrade>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.product.TradeInfo info;
		private readonly TradeInfo info;
	  /// <summary>
	  /// The bond that was traded.
	  /// <para>
	  /// The product captures the contracted financial details of the trade.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final CapitalIndexedBond product;
	  private readonly CapitalIndexedBond product;
	  /// <summary>
	  /// The quantity that was traded.
	  /// <para>
	  /// This will be positive if buying and negative if selling.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(overrideGet = true) private final double quantity;
	  private readonly double quantity;
	  /// <summary>
	  /// The <i>clean</i> price at which the bond was traded.
	  /// <para>
	  /// The "clean" price excludes any accrued interest.
	  /// </para>
	  /// <para>
	  /// Strata uses <i>decimal prices</i> for bonds in the trade model, pricers and market data.
	  /// For example, a price of 99.32% is represented in Strata by 0.9932.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative", overrideGet = true) private final double price;
	  private readonly double price;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.info_Renamed = TradeInfo.empty();
	  }

	  //-------------------------------------------------------------------------
	  public CapitalIndexedBondTrade withInfo(TradeInfo info)
	  {
		return new CapitalIndexedBondTrade(info, product, quantity, price);
	  }

	  public CapitalIndexedBondTrade withQuantity(double quantity)
	  {
		return new CapitalIndexedBondTrade(info, product, quantity, price);
	  }

	  public CapitalIndexedBondTrade withPrice(double price)
	  {
		return new CapitalIndexedBondTrade(info, product, quantity, price);
	  }

	  //-------------------------------------------------------------------------
	  public override PortfolioItemSummary summarize()
	  {
		// ID x 200
		string description = SecurityId.StandardId.Value + " x " + SummarizerUtils.value(Quantity);
		return SummarizerUtils.summary(this, ProductType.BOND, description, Currency);
	  }

	  public ResolvedCapitalIndexedBondTrade resolve(ReferenceData refData)
	  {
		ResolvedCapitalIndexedBond resolvedProduct = product.resolve(refData);
		LocalDate settlementDate = calculateSettlementDate(refData);

		double accruedInterest = resolvedProduct.accruedInterest(settlementDate) / product.Notional;
		if (settlementDate.isBefore(resolvedProduct.StartDate))
		{
		  throw new System.ArgumentException("Settlement date must not be before bond starts");
		}
		BondPaymentPeriod settlePeriod;
		if (product.YieldConvention.Equals(CapitalIndexedBondYieldConvention.GB_IL_FLOAT))
		{
		  settlePeriod = KnownAmountBondPaymentPeriod.of(Payment.of(product.Currency, -product.Notional * quantity * (price + accruedInterest), settlementDate), SchedulePeriod.of(resolvedProduct.StartDate, settlementDate, product.AccrualSchedule.StartDate, settlementDate));
		}
		else
		{
		  RateComputation rateComputation = product.RateCalculation.createRateComputation(settlementDate);
		  settlePeriod = CapitalIndexedBondPaymentPeriod.builder().startDate(resolvedProduct.StartDate).unadjustedStartDate(product.AccrualSchedule.StartDate).endDate(settlementDate).rateComputation(rateComputation).currency(product.Currency).notional(-product.Notional * quantity * (price + accruedInterest)).realCoupon(1d).build();
		}

		return ResolvedCapitalIndexedBondTrade.builder().info(info).product(resolvedProduct).quantity(quantity).settlement(ResolvedCapitalIndexedBondSettlement.of(settlementDate, price, settlePeriod)).build();
	  }

	  // calculates the settlement date from the trade date if necessary
	  private LocalDate calculateSettlementDate(ReferenceData refData)
	  {
		if (info.SettlementDate.Present)
		{
		  return info.SettlementDate.get();
		}
		if (info.TradeDate.Present)
		{
		  LocalDate tradeDate = info.TradeDate.get();
		  return product.SettlementDateOffset.adjust(tradeDate, refData);
		}
		throw new System.InvalidOperationException("CapitalIndexedBondTrade.resolve() requires either trade date or settlement date");
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CapitalIndexedBondTrade}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CapitalIndexedBondTrade.Meta meta()
	  {
		return CapitalIndexedBondTrade.Meta.INSTANCE;
	  }

	  static CapitalIndexedBondTrade()
	  {
		MetaBean.register(CapitalIndexedBondTrade.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static CapitalIndexedBondTrade.Builder builder()
	  {
		return new CapitalIndexedBondTrade.Builder();
	  }

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="info">  the value of the property, not null </param>
	  /// <param name="product">  the value of the property, not null </param>
	  /// <param name="quantity">  the value of the property </param>
	  /// <param name="price">  the value of the property </param>
	  internal CapitalIndexedBondTrade(TradeInfo info, CapitalIndexedBond product, double quantity, double price)
	  {
		JodaBeanUtils.notNull(info, "info");
		JodaBeanUtils.notNull(product, "product");
		ArgChecker.notNegative(price, "price");
		this.info = info;
		this.product = product;
		this.quantity = quantity;
		this.price = price;
	  }

	  public override CapitalIndexedBondTrade.Meta metaBean()
	  {
		return CapitalIndexedBondTrade.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the additional trade information, defaulted to an empty instance.
	  /// <para>
	  /// This allows additional information to be attached to the trade.
	  /// Either the trade or settlement date is required when calling <seealso cref="CapitalIndexedBondTrade#resolve(ReferenceData)"/>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public override TradeInfo Info
	  {
		  get
		  {
			return info;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the bond that was traded.
	  /// <para>
	  /// The product captures the contracted financial details of the trade.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public override CapitalIndexedBond Product
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
	  /// This will be positive if buying and negative if selling.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public override double Quantity
	  {
		  get
		  {
			return quantity;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the <i>clean</i> price at which the bond was traded.
	  /// <para>
	  /// The "clean" price excludes any accrued interest.
	  /// </para>
	  /// <para>
	  /// Strata uses <i>decimal prices</i> for bonds in the trade model, pricers and market data.
	  /// For example, a price of 99.32% is represented in Strata by 0.9932.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public override double Price
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
		  CapitalIndexedBondTrade other = (CapitalIndexedBondTrade) obj;
		  return JodaBeanUtils.equal(info, other.info) && JodaBeanUtils.equal(product, other.product) && JodaBeanUtils.equal(quantity, other.quantity) && JodaBeanUtils.equal(price, other.price);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(info);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(product);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(quantity);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(price);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("CapitalIndexedBondTrade{");
		buf.Append("info").Append('=').Append(info).Append(',').Append(' ');
		buf.Append("product").Append('=').Append(product).Append(',').Append(' ');
		buf.Append("quantity").Append('=').Append(quantity).Append(',').Append(' ');
		buf.Append("price").Append('=').Append(JodaBeanUtils.ToString(price));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CapitalIndexedBondTrade}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  info_Renamed = DirectMetaProperty.ofImmutable(this, "info", typeof(CapitalIndexedBondTrade), typeof(TradeInfo));
			  product_Renamed = DirectMetaProperty.ofImmutable(this, "product", typeof(CapitalIndexedBondTrade), typeof(CapitalIndexedBond));
			  quantity_Renamed = DirectMetaProperty.ofImmutable(this, "quantity", typeof(CapitalIndexedBondTrade), Double.TYPE);
			  price_Renamed = DirectMetaProperty.ofImmutable(this, "price", typeof(CapitalIndexedBondTrade), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "info", "product", "quantity", "price");
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
		internal MetaProperty<CapitalIndexedBond> product_Renamed;
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
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "info", "product", "quantity", "price");
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
			case 106934601: // price
			  return price_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override CapitalIndexedBondTrade.Builder builder()
		{
		  return new CapitalIndexedBondTrade.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CapitalIndexedBondTrade);
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
		public MetaProperty<CapitalIndexedBond> product()
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
			  return ((CapitalIndexedBondTrade) bean).Info;
			case -309474065: // product
			  return ((CapitalIndexedBondTrade) bean).Product;
			case -1285004149: // quantity
			  return ((CapitalIndexedBondTrade) bean).Quantity;
			case 106934601: // price
			  return ((CapitalIndexedBondTrade) bean).Price;
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
	  /// The bean-builder for {@code CapitalIndexedBondTrade}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<CapitalIndexedBondTrade>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal TradeInfo info_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CapitalIndexedBond product_Renamed;
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
		internal Builder(CapitalIndexedBondTrade beanToCopy)
		{
		  this.info_Renamed = beanToCopy.Info;
		  this.product_Renamed = beanToCopy.Product;
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
			case -309474065: // product
			  return product_Renamed;
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
			case -309474065: // product
			  this.product_Renamed = (CapitalIndexedBond) newValue;
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

		public override CapitalIndexedBondTrade build()
		{
		  return new CapitalIndexedBondTrade(info_Renamed, product_Renamed, quantity_Renamed, price_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the additional trade information, defaulted to an empty instance.
		/// <para>
		/// This allows additional information to be attached to the trade.
		/// Either the trade or settlement date is required when calling <seealso cref="CapitalIndexedBondTrade#resolve(ReferenceData)"/>.
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
		/// Sets the bond that was traded.
		/// <para>
		/// The product captures the contracted financial details of the trade.
		/// </para>
		/// </summary>
		/// <param name="product">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder product(CapitalIndexedBond product)
		{
		  JodaBeanUtils.notNull(product, "product");
		  this.product_Renamed = product;
		  return this;
		}

		/// <summary>
		/// Sets the quantity that was traded.
		/// <para>
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
		/// Sets the <i>clean</i> price at which the bond was traded.
		/// <para>
		/// The "clean" price excludes any accrued interest.
		/// </para>
		/// <para>
		/// Strata uses <i>decimal prices</i> for bonds in the trade model, pricers and market data.
		/// For example, a price of 99.32% is represented in Strata by 0.9932.
		/// </para>
		/// </summary>
		/// <param name="price">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder price(double price)
		{
		  ArgChecker.notNegative(price, "price");
		  this.price_Renamed = price;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("CapitalIndexedBondTrade.Builder{");
		  buf.Append("info").Append('=').Append(JodaBeanUtils.ToString(info_Renamed)).Append(',').Append(' ');
		  buf.Append("product").Append('=').Append(JodaBeanUtils.ToString(product_Renamed)).Append(',').Append(' ');
		  buf.Append("quantity").Append('=').Append(JodaBeanUtils.ToString(quantity_Renamed)).Append(',').Append(' ');
		  buf.Append("price").Append('=').Append(JodaBeanUtils.ToString(price_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}