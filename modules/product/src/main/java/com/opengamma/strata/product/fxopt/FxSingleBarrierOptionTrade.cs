using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fxopt
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
	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using SummarizerUtils = com.opengamma.strata.product.common.SummarizerUtils;
	using FxTrade = com.opengamma.strata.product.fx.FxTrade;

	/// <summary>
	/// A trade in an FX single barrier option.
	/// <para>
	/// An Over-The-Counter (OTC) trade in an <seealso cref="FxSingleBarrierOption"/>.
	/// </para>
	/// <para>
	/// An FX option is a financial instrument that provides an option to exchange two currencies at a specified future time 
	/// only when barrier event occurs (knock-in option) or does not occur (knock-out option).
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class FxSingleBarrierOptionTrade implements com.opengamma.strata.product.fx.FxTrade, com.opengamma.strata.product.ResolvableTrade<ResolvedFxSingleBarrierOptionTrade>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FxSingleBarrierOptionTrade : FxTrade, ResolvableTrade<ResolvedFxSingleBarrierOptionTrade>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.product.TradeInfo info;
		private readonly TradeInfo info;
	  /// <summary>
	  /// The FX option product that was agreed when the trade occurred.
	  /// <para>
	  /// The product captures the contracted financial details of the trade.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final FxSingleBarrierOption product;
	  private readonly FxSingleBarrierOption product;
	  /// <summary>
	  /// The premium of the FX option.
	  /// <para>
	  /// The premium sign should be compatible with the product Long/Short flag.
	  /// This means that the premium is negative for long and positive for short.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.AdjustablePayment premium;
	  private readonly AdjustablePayment premium;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.info_Renamed = TradeInfo.empty();
	  }

	  //-------------------------------------------------------------------------
	  public FxSingleBarrierOptionTrade withInfo(TradeInfo info)
	  {
		return new FxSingleBarrierOptionTrade(info, product, premium);
	  }

	  //-------------------------------------------------------------------------
	  public PortfolioItemSummary summarize()
	  {
		// Long Barrier Pay USD 1mm Premium USD 100k @ GBP/USD 1.32 : 21Jan18
		StringBuilder buf = new StringBuilder(96);
		CurrencyAmount @base = product.UnderlyingOption.Underlying.BaseCurrencyAmount;
		CurrencyAmount counter = product.UnderlyingOption.Underlying.CounterCurrencyAmount;
		buf.Append(product.UnderlyingOption.LongShort);
		buf.Append(" Barrier ");
		buf.Append(SummarizerUtils.fx(@base, counter));
		buf.Append(" Premium ");
		buf.Append(SummarizerUtils.amount(premium.Value.mapAmount(v => Math.Abs(v))));
		buf.Append(" : ");
		buf.Append(SummarizerUtils.date(product.UnderlyingOption.ExpiryDate));
		CurrencyPair currencyPair = product.CurrencyPair;
		return SummarizerUtils.summary(this, ProductType.FX_SINGLE_BARRIER_OPTION, buf.ToString(), currencyPair.Base, currencyPair.Counter);
	  }

	  public ResolvedFxSingleBarrierOptionTrade resolve(ReferenceData refData)
	  {
		return ResolvedFxSingleBarrierOptionTrade.builder().info(info).product(product.resolve(refData)).premium(premium.resolve(refData)).build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxSingleBarrierOptionTrade}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FxSingleBarrierOptionTrade.Meta meta()
	  {
		return FxSingleBarrierOptionTrade.Meta.INSTANCE;
	  }

	  static FxSingleBarrierOptionTrade()
	  {
		MetaBean.register(FxSingleBarrierOptionTrade.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static FxSingleBarrierOptionTrade.Builder builder()
	  {
		return new FxSingleBarrierOptionTrade.Builder();
	  }

	  private FxSingleBarrierOptionTrade(TradeInfo info, FxSingleBarrierOption product, AdjustablePayment premium)
	  {
		JodaBeanUtils.notNull(info, "info");
		JodaBeanUtils.notNull(product, "product");
		JodaBeanUtils.notNull(premium, "premium");
		this.info = info;
		this.product = product;
		this.premium = premium;
	  }

	  public override FxSingleBarrierOptionTrade.Meta metaBean()
	  {
		return FxSingleBarrierOptionTrade.Meta.INSTANCE;
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
	  /// Gets the FX option product that was agreed when the trade occurred.
	  /// <para>
	  /// The product captures the contracted financial details of the trade.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FxSingleBarrierOption Product
	  {
		  get
		  {
			return product;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the premium of the FX option.
	  /// <para>
	  /// The premium sign should be compatible with the product Long/Short flag.
	  /// This means that the premium is negative for long and positive for short.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public AdjustablePayment Premium
	  {
		  get
		  {
			return premium;
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
		  FxSingleBarrierOptionTrade other = (FxSingleBarrierOptionTrade) obj;
		  return JodaBeanUtils.equal(info, other.info) && JodaBeanUtils.equal(product, other.product) && JodaBeanUtils.equal(premium, other.premium);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(info);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(product);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(premium);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("FxSingleBarrierOptionTrade{");
		buf.Append("info").Append('=').Append(info).Append(',').Append(' ');
		buf.Append("product").Append('=').Append(product).Append(',').Append(' ');
		buf.Append("premium").Append('=').Append(JodaBeanUtils.ToString(premium));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxSingleBarrierOptionTrade}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  info_Renamed = DirectMetaProperty.ofImmutable(this, "info", typeof(FxSingleBarrierOptionTrade), typeof(TradeInfo));
			  product_Renamed = DirectMetaProperty.ofImmutable(this, "product", typeof(FxSingleBarrierOptionTrade), typeof(FxSingleBarrierOption));
			  premium_Renamed = DirectMetaProperty.ofImmutable(this, "premium", typeof(FxSingleBarrierOptionTrade), typeof(AdjustablePayment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "info", "product", "premium");
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
		internal MetaProperty<FxSingleBarrierOption> product_Renamed;
		/// <summary>
		/// The meta-property for the {@code premium} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<AdjustablePayment> premium_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "info", "product", "premium");
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
			case -318452137: // premium
			  return premium_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override FxSingleBarrierOptionTrade.Builder builder()
		{
		  return new FxSingleBarrierOptionTrade.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FxSingleBarrierOptionTrade);
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
		public MetaProperty<FxSingleBarrierOption> product()
		{
		  return product_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code premium} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<AdjustablePayment> premium()
		{
		  return premium_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  return ((FxSingleBarrierOptionTrade) bean).Info;
			case -309474065: // product
			  return ((FxSingleBarrierOptionTrade) bean).Product;
			case -318452137: // premium
			  return ((FxSingleBarrierOptionTrade) bean).Premium;
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
	  /// The bean-builder for {@code FxSingleBarrierOptionTrade}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<FxSingleBarrierOptionTrade>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal TradeInfo info_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FxSingleBarrierOption product_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal AdjustablePayment premium_Renamed;

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
		internal Builder(FxSingleBarrierOptionTrade beanToCopy)
		{
		  this.info_Renamed = beanToCopy.Info;
		  this.product_Renamed = beanToCopy.Product;
		  this.premium_Renamed = beanToCopy.Premium;
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
			case -318452137: // premium
			  return premium_Renamed;
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
			  this.product_Renamed = (FxSingleBarrierOption) newValue;
			  break;
			case -318452137: // premium
			  this.premium_Renamed = (AdjustablePayment) newValue;
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

		public override FxSingleBarrierOptionTrade build()
		{
		  return new FxSingleBarrierOptionTrade(info_Renamed, product_Renamed, premium_Renamed);
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
		/// Sets the FX option product that was agreed when the trade occurred.
		/// <para>
		/// The product captures the contracted financial details of the trade.
		/// </para>
		/// </summary>
		/// <param name="product">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder product(FxSingleBarrierOption product)
		{
		  JodaBeanUtils.notNull(product, "product");
		  this.product_Renamed = product;
		  return this;
		}

		/// <summary>
		/// Sets the premium of the FX option.
		/// <para>
		/// The premium sign should be compatible with the product Long/Short flag.
		/// This means that the premium is negative for long and positive for short.
		/// </para>
		/// </summary>
		/// <param name="premium">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder premium(AdjustablePayment premium)
		{
		  JodaBeanUtils.notNull(premium, "premium");
		  this.premium_Renamed = premium;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("FxSingleBarrierOptionTrade.Builder{");
		  buf.Append("info").Append('=').Append(JodaBeanUtils.ToString(info_Renamed)).Append(',').Append(' ');
		  buf.Append("product").Append('=').Append(JodaBeanUtils.ToString(product_Renamed)).Append(',').Append(' ');
		  buf.Append("premium").Append('=').Append(JodaBeanUtils.ToString(premium_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}