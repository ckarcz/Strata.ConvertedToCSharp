using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.cms
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
	using SummarizerUtils = com.opengamma.strata.product.common.SummarizerUtils;

	/// <summary>
	/// A trade in a constant maturity swap (CMS).
	/// <para>
	/// An Over-The-Counter (OTC) trade in a <seealso cref="Cms"/>.
	/// </para>
	/// <para>
	/// For example, a CMS trade might involve an agreement to exchange the difference between
	/// the fixed rate of 1% and the swap rate of 5-year 'GBP-FIXED-6M-LIBOR-6M' swaps every 6 months for 2 years.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class CmsTrade implements com.opengamma.strata.product.ProductTrade, com.opengamma.strata.product.ResolvableTrade<ResolvedCmsTrade>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CmsTrade : ProductTrade, ResolvableTrade<ResolvedCmsTrade>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.product.TradeInfo info;
		private readonly TradeInfo info;
	  /// <summary>
	  /// The CMS product that was agreed when the trade occurred.
	  /// <para>
	  /// The product captures the contracted financial details of the trade.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final Cms product;
	  private readonly Cms product;
	  /// <summary>
	  /// The optional premium of the product.
	  /// <para>
	  /// For certain CMS products, a premium is paid upfront. This typically occurs instead
	  /// of periodic payments based on fixed or Ibor rates over the lifetime of the product.
	  /// </para>
	  /// <para>
	  /// The premium sign must be compatible with the product Pay/Receive flag.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.basics.currency.AdjustablePayment premium;
	  private readonly AdjustablePayment premium;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.info_Renamed = TradeInfo.empty();
	  }

	  //-------------------------------------------------------------------------
	  public CmsTrade withInfo(TradeInfo info)
	  {
		return new CmsTrade(info, product, premium);
	  }

	  //-------------------------------------------------------------------------
	  public PortfolioItemSummary summarize()
	  {
		// 5Y USD 2mm Rec USD-LIBOR-1100-1Y Cap 1% / Pay Premium : 21Jan17-21Jan22
		StringBuilder buf = new StringBuilder(96);
		CmsLeg mainLeg = product.CmsLeg;
		buf.Append(SummarizerUtils.datePeriod(mainLeg.StartDate.Unadjusted, mainLeg.EndDate.Unadjusted));
		buf.Append(' ');
		buf.Append(SummarizerUtils.amount(mainLeg.Currency, mainLeg.Notional.InitialValue));
		buf.Append(' ');
		if (mainLeg.PayReceive.Receive)
		{
		  buf.Append("Rec ");
		  summarizeMainLeg(mainLeg, buf);
		  buf.Append(Premium.Present ? " / Pay Premium" : (product.PayLeg.Present ? " /  Pay Periodic" : ""));
		}
		else
		{
		  buf.Append(Premium.Present ? "Rec Premium / Pay " : (product.PayLeg.Present ? "Rec Periodic / Pay " : ""));
		  summarizeMainLeg(mainLeg, buf);
		}
		buf.Append(" : ");
		buf.Append(SummarizerUtils.dateRange(mainLeg.StartDate.Unadjusted, mainLeg.EndDate.Unadjusted));
		return SummarizerUtils.summary(this, ProductType.CMS, buf.ToString(), mainLeg.Currency);
	  }

	  // summarize the main leg
	  private void summarizeMainLeg(CmsLeg mainLeg, StringBuilder buf)
	  {
		buf.Append(mainLeg.Index);
		buf.Append(' ');
		if (mainLeg.CapSchedule.Present)
		{
		  buf.Append("Cap ");
		  buf.Append(SummarizerUtils.percent(mainLeg.CapSchedule.get().InitialValue));
		}
		if (mainLeg.FloorSchedule.Present)
		{
		  buf.Append("Floor ");
		  buf.Append(SummarizerUtils.percent(mainLeg.FloorSchedule.get().InitialValue));
		}
	  }

	  public ResolvedCmsTrade resolve(ReferenceData refData)
	  {
		return ResolvedCmsTrade.builder().info(info).product(product.resolve(refData)).premium(premium != null ? premium.resolve(refData) : null).build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CmsTrade}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CmsTrade.Meta meta()
	  {
		return CmsTrade.Meta.INSTANCE;
	  }

	  static CmsTrade()
	  {
		MetaBean.register(CmsTrade.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static CmsTrade.Builder builder()
	  {
		return new CmsTrade.Builder();
	  }

	  private CmsTrade(TradeInfo info, Cms product, AdjustablePayment premium)
	  {
		JodaBeanUtils.notNull(info, "info");
		JodaBeanUtils.notNull(product, "product");
		this.info = info;
		this.product = product;
		this.premium = premium;
	  }

	  public override CmsTrade.Meta metaBean()
	  {
		return CmsTrade.Meta.INSTANCE;
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
	  /// Gets the CMS product that was agreed when the trade occurred.
	  /// <para>
	  /// The product captures the contracted financial details of the trade.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Cms Product
	  {
		  get
		  {
			return product;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the optional premium of the product.
	  /// <para>
	  /// For certain CMS products, a premium is paid upfront. This typically occurs instead
	  /// of periodic payments based on fixed or Ibor rates over the lifetime of the product.
	  /// </para>
	  /// <para>
	  /// The premium sign must be compatible with the product Pay/Receive flag.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<AdjustablePayment> Premium
	  {
		  get
		  {
			return Optional.ofNullable(premium);
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
		  CmsTrade other = (CmsTrade) obj;
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
		buf.Append("CmsTrade{");
		buf.Append("info").Append('=').Append(info).Append(',').Append(' ');
		buf.Append("product").Append('=').Append(product).Append(',').Append(' ');
		buf.Append("premium").Append('=').Append(JodaBeanUtils.ToString(premium));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CmsTrade}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  info_Renamed = DirectMetaProperty.ofImmutable(this, "info", typeof(CmsTrade), typeof(TradeInfo));
			  product_Renamed = DirectMetaProperty.ofImmutable(this, "product", typeof(CmsTrade), typeof(Cms));
			  premium_Renamed = DirectMetaProperty.ofImmutable(this, "premium", typeof(CmsTrade), typeof(AdjustablePayment));
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
		internal MetaProperty<Cms> product_Renamed;
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

		public override CmsTrade.Builder builder()
		{
		  return new CmsTrade.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CmsTrade);
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
		public MetaProperty<Cms> product()
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
			  return ((CmsTrade) bean).Info;
			case -309474065: // product
			  return ((CmsTrade) bean).Product;
			case -318452137: // premium
			  return ((CmsTrade) bean).premium;
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
	  /// The bean-builder for {@code CmsTrade}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<CmsTrade>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal TradeInfo info_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Cms product_Renamed;
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
		internal Builder(CmsTrade beanToCopy)
		{
		  this.info_Renamed = beanToCopy.Info;
		  this.product_Renamed = beanToCopy.Product;
		  this.premium_Renamed = beanToCopy.premium;
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
			  this.product_Renamed = (Cms) newValue;
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

		public override CmsTrade build()
		{
		  return new CmsTrade(info_Renamed, product_Renamed, premium_Renamed);
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
		/// Sets the CMS product that was agreed when the trade occurred.
		/// <para>
		/// The product captures the contracted financial details of the trade.
		/// </para>
		/// </summary>
		/// <param name="product">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder product(Cms product)
		{
		  JodaBeanUtils.notNull(product, "product");
		  this.product_Renamed = product;
		  return this;
		}

		/// <summary>
		/// Sets the optional premium of the product.
		/// <para>
		/// For certain CMS products, a premium is paid upfront. This typically occurs instead
		/// of periodic payments based on fixed or Ibor rates over the lifetime of the product.
		/// </para>
		/// <para>
		/// The premium sign must be compatible with the product Pay/Receive flag.
		/// </para>
		/// </summary>
		/// <param name="premium">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder premium(AdjustablePayment premium)
		{
		  this.premium_Renamed = premium;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("CmsTrade.Builder{");
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