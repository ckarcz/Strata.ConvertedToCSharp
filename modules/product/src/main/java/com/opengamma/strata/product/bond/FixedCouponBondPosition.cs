using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using DerivedProperty = org.joda.beans.gen.DerivedProperty;
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Resolvable = com.opengamma.strata.basics.Resolvable;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using SummarizerUtils = com.opengamma.strata.product.common.SummarizerUtils;

	/// <summary>
	/// A position in a fixed coupon bond.
	/// <para>
	/// A position in an underlying <seealso cref="FixedCouponBond"/>.
	/// </para>
	/// <para>
	/// The net quantity of the position is stored using two fields - {@code longQuantity} and {@code shortQuantity}.
	/// These two fields must not be negative.
	/// In many cases, only a long quantity or short quantity will be present with the other set to zero.
	/// However it is also possible for both to be non-zero, allowing long and short positions to be treated separately.
	/// The net quantity is available via <seealso cref="#getQuantity()"/>.
	/// 
	/// <h4>Price</h4>
	/// Strata uses <i>decimal prices</i> for bonds in the trade model, pricers and market data.
	/// For example, a price of 99.32% is represented in Strata by 0.9932.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(constructorScope = "package") public final class FixedCouponBondPosition implements com.opengamma.strata.product.SecuritizedProductPosition<FixedCouponBond>, com.opengamma.strata.basics.Resolvable<ResolvedFixedCouponBondTrade>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FixedCouponBondPosition : SecuritizedProductPosition<FixedCouponBond>, Resolvable<ResolvedFixedCouponBondTrade>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.product.PositionInfo info;
		private readonly PositionInfo info;
	  /// <summary>
	  /// The bond that was traded.
	  /// <para>
	  /// The product captures the contracted financial details.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final FixedCouponBond product;
	  private readonly FixedCouponBond product;
	  /// <summary>
	  /// The long quantity of the security.
	  /// <para>
	  /// This is the quantity of the underlying security that is held.
	  /// The quantity cannot be negative, as that would imply short selling.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative") private final double longQuantity;
	  private readonly double longQuantity;
	  /// <summary>
	  /// The short quantity of the security.
	  /// <para>
	  /// This is the quantity of the underlying security that has been short sold.
	  /// The quantity cannot be negative, as that would imply the position is long.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative") private final double shortQuantity;
	  private readonly double shortQuantity;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from position information, product and net quantity.
	  /// <para>
	  /// The net quantity is the long quantity minus the short quantity, which may be negative.
	  /// If the quantity is positive it is treated as a long quantity.
	  /// Otherwise it is treated as a short quantity.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="positionInfo">  the position information </param>
	  /// <param name="product">  the underlying product </param>
	  /// <param name="netQuantity">  the net quantity of the underlying security </param>
	  /// <returns> the position </returns>
	  public static FixedCouponBondPosition ofNet(PositionInfo positionInfo, FixedCouponBond product, double netQuantity)
	  {
		double longQuantity = netQuantity >= 0 ? netQuantity : 0;
		double shortQuantity = netQuantity >= 0 ? 0 : -netQuantity;
		return new FixedCouponBondPosition(positionInfo, product, longQuantity, shortQuantity);
	  }

	  /// <summary>
	  /// Obtains an instance from position information, product, long quantity and short quantity.
	  /// <para>
	  /// The long quantity and short quantity must be zero or positive, not negative.
	  /// In many cases, only a long quantity or short quantity will be present with the other set to zero.
	  /// However it is also possible for both to be non-zero, allowing long and short positions to be treated separately.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="positionInfo">  the position information </param>
	  /// <param name="product">  the underlying product </param>
	  /// <param name="longQuantity">  the long quantity of the underlying security </param>
	  /// <param name="shortQuantity">  the short quantity of the underlying security </param>
	  /// <returns> the position </returns>
	  public static FixedCouponBondPosition ofLongShort(PositionInfo positionInfo, FixedCouponBond product, double longQuantity, double shortQuantity)
	  {

		return new FixedCouponBondPosition(positionInfo, product, longQuantity, shortQuantity);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.info_Renamed = PositionInfo.empty();
	  }

	  //-------------------------------------------------------------------------
	  public override SecurityId SecurityId
	  {
		  get
		  {
			return product.SecurityId;
		  }
	  }

	  public override Currency Currency
	  {
		  get
		  {
			return product.Currency;
		  }
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @DerivedProperty public double getQuantity()
	  public override double Quantity
	  {
		  get
		  {
			return longQuantity - shortQuantity;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public FixedCouponBondPosition withInfo(PositionInfo info)
	  {
		return new FixedCouponBondPosition(info, product, longQuantity, shortQuantity);
	  }

	  public FixedCouponBondPosition withQuantity(double quantity)
	  {
		return FixedCouponBondPosition.ofNet(info, product, quantity);
	  }

	  //-------------------------------------------------------------------------
	  public override PortfolioItemSummary summarize()
	  {
		// ID x 200
		string description = SecurityId.StandardId.Value + " x " + SummarizerUtils.value(Quantity);
		return SummarizerUtils.summary(this, ProductType.BOND, description, Currency);
	  }

	  public ResolvedFixedCouponBondTrade resolve(ReferenceData refData)
	  {
		ResolvedFixedCouponBond resolved = product.resolve(refData);
		return new ResolvedFixedCouponBondTrade(info, resolved, Quantity, null);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FixedCouponBondPosition}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FixedCouponBondPosition.Meta meta()
	  {
		return FixedCouponBondPosition.Meta.INSTANCE;
	  }

	  static FixedCouponBondPosition()
	  {
		MetaBean.register(FixedCouponBondPosition.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static FixedCouponBondPosition.Builder builder()
	  {
		return new FixedCouponBondPosition.Builder();
	  }

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="info">  the value of the property, not null </param>
	  /// <param name="product">  the value of the property, not null </param>
	  /// <param name="longQuantity">  the value of the property </param>
	  /// <param name="shortQuantity">  the value of the property </param>
	  internal FixedCouponBondPosition(PositionInfo info, FixedCouponBond product, double longQuantity, double shortQuantity)
	  {
		JodaBeanUtils.notNull(info, "info");
		JodaBeanUtils.notNull(product, "product");
		ArgChecker.notNegative(longQuantity, "longQuantity");
		ArgChecker.notNegative(shortQuantity, "shortQuantity");
		this.info = info;
		this.product = product;
		this.longQuantity = longQuantity;
		this.shortQuantity = shortQuantity;
	  }

	  public override FixedCouponBondPosition.Meta metaBean()
	  {
		return FixedCouponBondPosition.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the additional position information, defaulted to an empty instance.
	  /// <para>
	  /// This allows additional information to be attached to the position.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public override PositionInfo Info
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
	  /// The product captures the contracted financial details.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public override FixedCouponBond Product
	  {
		  get
		  {
			return product;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the long quantity of the security.
	  /// <para>
	  /// This is the quantity of the underlying security that is held.
	  /// The quantity cannot be negative, as that would imply short selling.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double LongQuantity
	  {
		  get
		  {
			return longQuantity;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the short quantity of the security.
	  /// <para>
	  /// This is the quantity of the underlying security that has been short sold.
	  /// The quantity cannot be negative, as that would imply the position is long.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double ShortQuantity
	  {
		  get
		  {
			return shortQuantity;
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
		  FixedCouponBondPosition other = (FixedCouponBondPosition) obj;
		  return JodaBeanUtils.equal(info, other.info) && JodaBeanUtils.equal(product, other.product) && JodaBeanUtils.equal(longQuantity, other.longQuantity) && JodaBeanUtils.equal(shortQuantity, other.shortQuantity);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(info);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(product);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(longQuantity);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(shortQuantity);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("FixedCouponBondPosition{");
		buf.Append("info").Append('=').Append(info).Append(',').Append(' ');
		buf.Append("product").Append('=').Append(product).Append(',').Append(' ');
		buf.Append("longQuantity").Append('=').Append(longQuantity).Append(',').Append(' ');
		buf.Append("shortQuantity").Append('=').Append(JodaBeanUtils.ToString(shortQuantity));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FixedCouponBondPosition}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  info_Renamed = DirectMetaProperty.ofImmutable(this, "info", typeof(FixedCouponBondPosition), typeof(PositionInfo));
			  product_Renamed = DirectMetaProperty.ofImmutable(this, "product", typeof(FixedCouponBondPosition), typeof(FixedCouponBond));
			  longQuantity_Renamed = DirectMetaProperty.ofImmutable(this, "longQuantity", typeof(FixedCouponBondPosition), Double.TYPE);
			  shortQuantity_Renamed = DirectMetaProperty.ofImmutable(this, "shortQuantity", typeof(FixedCouponBondPosition), Double.TYPE);
			  quantity_Renamed = DirectMetaProperty.ofDerived(this, "quantity", typeof(FixedCouponBondPosition), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "info", "product", "longQuantity", "shortQuantity", "quantity");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code info} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PositionInfo> info_Renamed;
		/// <summary>
		/// The meta-property for the {@code product} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FixedCouponBond> product_Renamed;
		/// <summary>
		/// The meta-property for the {@code longQuantity} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> longQuantity_Renamed;
		/// <summary>
		/// The meta-property for the {@code shortQuantity} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> shortQuantity_Renamed;
		/// <summary>
		/// The meta-property for the {@code quantity} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> quantity_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "info", "product", "longQuantity", "shortQuantity", "quantity");
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
			case 611668775: // longQuantity
			  return longQuantity_Renamed;
			case -2094395097: // shortQuantity
			  return shortQuantity_Renamed;
			case -1285004149: // quantity
			  return quantity_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override FixedCouponBondPosition.Builder builder()
		{
		  return new FixedCouponBondPosition.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FixedCouponBondPosition);
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
		public MetaProperty<PositionInfo> info()
		{
		  return info_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code product} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FixedCouponBond> product()
		{
		  return product_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code longQuantity} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> longQuantity()
		{
		  return longQuantity_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code shortQuantity} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> shortQuantity()
		{
		  return shortQuantity_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code quantity} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> quantity()
		{
		  return quantity_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  return ((FixedCouponBondPosition) bean).Info;
			case -309474065: // product
			  return ((FixedCouponBondPosition) bean).Product;
			case 611668775: // longQuantity
			  return ((FixedCouponBondPosition) bean).LongQuantity;
			case -2094395097: // shortQuantity
			  return ((FixedCouponBondPosition) bean).ShortQuantity;
			case -1285004149: // quantity
			  return ((FixedCouponBondPosition) bean).Quantity;
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
	  /// The bean-builder for {@code FixedCouponBondPosition}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<FixedCouponBondPosition>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PositionInfo info_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FixedCouponBond product_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double longQuantity_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double shortQuantity_Renamed;

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
		internal Builder(FixedCouponBondPosition beanToCopy)
		{
		  this.info_Renamed = beanToCopy.Info;
		  this.product_Renamed = beanToCopy.Product;
		  this.longQuantity_Renamed = beanToCopy.LongQuantity;
		  this.shortQuantity_Renamed = beanToCopy.ShortQuantity;
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
			case 611668775: // longQuantity
			  return longQuantity_Renamed;
			case -2094395097: // shortQuantity
			  return shortQuantity_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  this.info_Renamed = (PositionInfo) newValue;
			  break;
			case -309474065: // product
			  this.product_Renamed = (FixedCouponBond) newValue;
			  break;
			case 611668775: // longQuantity
			  this.longQuantity_Renamed = (double?) newValue.Value;
			  break;
			case -2094395097: // shortQuantity
			  this.shortQuantity_Renamed = (double?) newValue.Value;
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

		public override FixedCouponBondPosition build()
		{
		  return new FixedCouponBondPosition(info_Renamed, product_Renamed, longQuantity_Renamed, shortQuantity_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the additional position information, defaulted to an empty instance.
		/// <para>
		/// This allows additional information to be attached to the position.
		/// </para>
		/// </summary>
		/// <param name="info">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder info(PositionInfo info)
		{
		  JodaBeanUtils.notNull(info, "info");
		  this.info_Renamed = info;
		  return this;
		}

		/// <summary>
		/// Sets the bond that was traded.
		/// <para>
		/// The product captures the contracted financial details.
		/// </para>
		/// </summary>
		/// <param name="product">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder product(FixedCouponBond product)
		{
		  JodaBeanUtils.notNull(product, "product");
		  this.product_Renamed = product;
		  return this;
		}

		/// <summary>
		/// Sets the long quantity of the security.
		/// <para>
		/// This is the quantity of the underlying security that is held.
		/// The quantity cannot be negative, as that would imply short selling.
		/// </para>
		/// </summary>
		/// <param name="longQuantity">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder longQuantity(double longQuantity)
		{
		  ArgChecker.notNegative(longQuantity, "longQuantity");
		  this.longQuantity_Renamed = longQuantity;
		  return this;
		}

		/// <summary>
		/// Sets the short quantity of the security.
		/// <para>
		/// This is the quantity of the underlying security that has been short sold.
		/// The quantity cannot be negative, as that would imply the position is long.
		/// </para>
		/// </summary>
		/// <param name="shortQuantity">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder shortQuantity(double shortQuantity)
		{
		  ArgChecker.notNegative(shortQuantity, "shortQuantity");
		  this.shortQuantity_Renamed = shortQuantity;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("FixedCouponBondPosition.Builder{");
		  buf.Append("info").Append('=').Append(JodaBeanUtils.ToString(info_Renamed)).Append(',').Append(' ');
		  buf.Append("product").Append('=').Append(JodaBeanUtils.ToString(product_Renamed)).Append(',').Append(' ');
		  buf.Append("longQuantity").Append('=').Append(JodaBeanUtils.ToString(longQuantity_Renamed)).Append(',').Append(' ');
		  buf.Append("shortQuantity").Append('=').Append(JodaBeanUtils.ToString(shortQuantity_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}