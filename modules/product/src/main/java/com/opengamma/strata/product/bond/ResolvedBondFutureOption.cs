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
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Rounding = com.opengamma.strata.basics.value.Rounding;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PutCall = com.opengamma.strata.product.common.PutCall;
	using FutureOptionPremiumStyle = com.opengamma.strata.product.option.FutureOptionPremiumStyle;

	/// <summary>
	/// A futures option contract based on a basket of fixed coupon bonds, resolved for pricing.
	/// <para>
	/// This is the resolved form of <seealso cref="BondFutureOption"/> and is an input to the pricers.
	/// Applications will typically create a {@code ResolvedBondFutureOption} from a {@code BondFutureOption}
	/// using <seealso cref="BondFutureOption#resolve(ReferenceData)"/>.
	/// </para>
	/// <para>
	/// A {@code ResolvedBondFutureOption} is bound to data that changes over time, such as holiday calendars.
	/// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// 
	/// <h4>Price</h4>
	/// Strata uses <i>decimal prices</i> for bond futures options in the trade model, pricers and market data.
	/// This is coherent with the pricing of <seealso cref="BondFuture"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(constructorScope = "package") public final class ResolvedBondFutureOption implements com.opengamma.strata.product.ResolvedProduct, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ResolvedBondFutureOption : ResolvedProduct, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.SecurityId securityId;
		private readonly SecurityId securityId;
	  /// <summary>
	  /// Whether the option is put or call.
	  /// <para>
	  /// A call gives the owner the right, but not obligation, to buy the underlying at
	  /// an agreed price in the future. A put gives a similar option to sell.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final com.opengamma.strata.product.common.PutCall putCall;
	  private readonly PutCall putCall;
	  /// <summary>
	  /// The strike price, represented in decimal form.
	  /// <para>
	  /// This is the price at which the option applies and refers to the price of the underlying future.
	  /// This must be represented in decimal form.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double strikePrice;
	  private readonly double strikePrice;
	  /// <summary>
	  /// The expiry of the option.
	  /// <para>
	  /// The date must not be after last trade date of the underlying future.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.ZonedDateTime expiry;
	  private readonly ZonedDateTime expiry;
	  /// <summary>
	  /// The style of the option premium.
	  /// <para>
	  /// The two options are daily margining and upfront premium.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.option.FutureOptionPremiumStyle premiumStyle;
	  private readonly FutureOptionPremiumStyle premiumStyle;
	  /// <summary>
	  /// The definition of how to round the option price, defaulted to no rounding.
	  /// <para>
	  /// The price is represented in decimal form, not percentage form.
	  /// As such, the decimal places expressed by the rounding refers to this decimal form.
	  /// For example, the common market price of 99.7125 is represented as 0.997125 which
	  /// has 6 decimal places.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.value.Rounding rounding;
	  private readonly Rounding rounding;
	  /// <summary>
	  /// The underlying future.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final ResolvedBondFuture underlyingFuture;
	  private readonly ResolvedBondFuture underlyingFuture;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.rounding(Rounding.none());
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		LocalDate lastTradeDate = underlyingFuture.LastTradeDate;
		ArgChecker.inOrderOrEqual(expiry.toLocalDate(), lastTradeDate, "expiry.date", "underlyingFuture.lastTradeDate");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the expiry date of the option.
	  /// </summary>
	  /// <returns> the expiry date </returns>
	  public LocalDate ExpiryDate
	  {
		  get
		  {
			return expiry.toLocalDate();
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedBondFutureOption}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ResolvedBondFutureOption.Meta meta()
	  {
		return ResolvedBondFutureOption.Meta.INSTANCE;
	  }

	  static ResolvedBondFutureOption()
	  {
		MetaBean.register(ResolvedBondFutureOption.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ResolvedBondFutureOption.Builder builder()
	  {
		return new ResolvedBondFutureOption.Builder();
	  }

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="securityId">  the value of the property, not null </param>
	  /// <param name="putCall">  the value of the property </param>
	  /// <param name="strikePrice">  the value of the property </param>
	  /// <param name="expiry">  the value of the property, not null </param>
	  /// <param name="premiumStyle">  the value of the property, not null </param>
	  /// <param name="rounding">  the value of the property, not null </param>
	  /// <param name="underlyingFuture">  the value of the property, not null </param>
	  internal ResolvedBondFutureOption(SecurityId securityId, PutCall putCall, double strikePrice, ZonedDateTime expiry, FutureOptionPremiumStyle premiumStyle, Rounding rounding, ResolvedBondFuture underlyingFuture)
	  {
		JodaBeanUtils.notNull(securityId, "securityId");
		JodaBeanUtils.notNull(expiry, "expiry");
		JodaBeanUtils.notNull(premiumStyle, "premiumStyle");
		JodaBeanUtils.notNull(rounding, "rounding");
		JodaBeanUtils.notNull(underlyingFuture, "underlyingFuture");
		this.securityId = securityId;
		this.putCall = putCall;
		this.strikePrice = strikePrice;
		this.expiry = expiry;
		this.premiumStyle = premiumStyle;
		this.rounding = rounding;
		this.underlyingFuture = underlyingFuture;
		validate();
	  }

	  public override ResolvedBondFutureOption.Meta metaBean()
	  {
		return ResolvedBondFutureOption.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the security identifier.
	  /// <para>
	  /// This identifier uniquely identifies the security within the system.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SecurityId SecurityId
	  {
		  get
		  {
			return securityId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets whether the option is put or call.
	  /// <para>
	  /// A call gives the owner the right, but not obligation, to buy the underlying at
	  /// an agreed price in the future. A put gives a similar option to sell.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public PutCall PutCall
	  {
		  get
		  {
			return putCall;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the strike price, represented in decimal form.
	  /// <para>
	  /// This is the price at which the option applies and refers to the price of the underlying future.
	  /// This must be represented in decimal form.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double StrikePrice
	  {
		  get
		  {
			return strikePrice;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the expiry of the option.
	  /// <para>
	  /// The date must not be after last trade date of the underlying future.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ZonedDateTime Expiry
	  {
		  get
		  {
			return expiry;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the style of the option premium.
	  /// <para>
	  /// The two options are daily margining and upfront premium.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FutureOptionPremiumStyle PremiumStyle
	  {
		  get
		  {
			return premiumStyle;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the definition of how to round the option price, defaulted to no rounding.
	  /// <para>
	  /// The price is represented in decimal form, not percentage form.
	  /// As such, the decimal places expressed by the rounding refers to this decimal form.
	  /// For example, the common market price of 99.7125 is represented as 0.997125 which
	  /// has 6 decimal places.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Rounding Rounding
	  {
		  get
		  {
			return rounding;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying future. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ResolvedBondFuture UnderlyingFuture
	  {
		  get
		  {
			return underlyingFuture;
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
		  ResolvedBondFutureOption other = (ResolvedBondFutureOption) obj;
		  return JodaBeanUtils.equal(securityId, other.securityId) && JodaBeanUtils.equal(putCall, other.putCall) && JodaBeanUtils.equal(strikePrice, other.strikePrice) && JodaBeanUtils.equal(expiry, other.expiry) && JodaBeanUtils.equal(premiumStyle, other.premiumStyle) && JodaBeanUtils.equal(rounding, other.rounding) && JodaBeanUtils.equal(underlyingFuture, other.underlyingFuture);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(securityId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(putCall);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(strikePrice);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(expiry);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(premiumStyle);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rounding);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(underlyingFuture);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(256);
		buf.Append("ResolvedBondFutureOption{");
		buf.Append("securityId").Append('=').Append(securityId).Append(',').Append(' ');
		buf.Append("putCall").Append('=').Append(putCall).Append(',').Append(' ');
		buf.Append("strikePrice").Append('=').Append(strikePrice).Append(',').Append(' ');
		buf.Append("expiry").Append('=').Append(expiry).Append(',').Append(' ');
		buf.Append("premiumStyle").Append('=').Append(premiumStyle).Append(',').Append(' ');
		buf.Append("rounding").Append('=').Append(rounding).Append(',').Append(' ');
		buf.Append("underlyingFuture").Append('=').Append(JodaBeanUtils.ToString(underlyingFuture));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedBondFutureOption}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  securityId_Renamed = DirectMetaProperty.ofImmutable(this, "securityId", typeof(ResolvedBondFutureOption), typeof(SecurityId));
			  putCall_Renamed = DirectMetaProperty.ofImmutable(this, "putCall", typeof(ResolvedBondFutureOption), typeof(PutCall));
			  strikePrice_Renamed = DirectMetaProperty.ofImmutable(this, "strikePrice", typeof(ResolvedBondFutureOption), Double.TYPE);
			  expiry_Renamed = DirectMetaProperty.ofImmutable(this, "expiry", typeof(ResolvedBondFutureOption), typeof(ZonedDateTime));
			  premiumStyle_Renamed = DirectMetaProperty.ofImmutable(this, "premiumStyle", typeof(ResolvedBondFutureOption), typeof(FutureOptionPremiumStyle));
			  rounding_Renamed = DirectMetaProperty.ofImmutable(this, "rounding", typeof(ResolvedBondFutureOption), typeof(Rounding));
			  underlyingFuture_Renamed = DirectMetaProperty.ofImmutable(this, "underlyingFuture", typeof(ResolvedBondFutureOption), typeof(ResolvedBondFuture));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "securityId", "putCall", "strikePrice", "expiry", "premiumStyle", "rounding", "underlyingFuture");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code securityId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SecurityId> securityId_Renamed;
		/// <summary>
		/// The meta-property for the {@code putCall} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PutCall> putCall_Renamed;
		/// <summary>
		/// The meta-property for the {@code strikePrice} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> strikePrice_Renamed;
		/// <summary>
		/// The meta-property for the {@code expiry} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ZonedDateTime> expiry_Renamed;
		/// <summary>
		/// The meta-property for the {@code premiumStyle} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FutureOptionPremiumStyle> premiumStyle_Renamed;
		/// <summary>
		/// The meta-property for the {@code rounding} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Rounding> rounding_Renamed;
		/// <summary>
		/// The meta-property for the {@code underlyingFuture} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ResolvedBondFuture> underlyingFuture_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "securityId", "putCall", "strikePrice", "expiry", "premiumStyle", "rounding", "underlyingFuture");
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
			case 1574023291: // securityId
			  return securityId_Renamed;
			case -219971059: // putCall
			  return putCall_Renamed;
			case 50946231: // strikePrice
			  return strikePrice_Renamed;
			case -1289159373: // expiry
			  return expiry_Renamed;
			case -1257652838: // premiumStyle
			  return premiumStyle_Renamed;
			case -142444: // rounding
			  return rounding_Renamed;
			case -165476480: // underlyingFuture
			  return underlyingFuture_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ResolvedBondFutureOption.Builder builder()
		{
		  return new ResolvedBondFutureOption.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ResolvedBondFutureOption);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code securityId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SecurityId> securityId()
		{
		  return securityId_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code putCall} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PutCall> putCall()
		{
		  return putCall_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code strikePrice} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> strikePrice()
		{
		  return strikePrice_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code expiry} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ZonedDateTime> expiry()
		{
		  return expiry_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code premiumStyle} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FutureOptionPremiumStyle> premiumStyle()
		{
		  return premiumStyle_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code rounding} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Rounding> rounding()
		{
		  return rounding_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code underlyingFuture} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ResolvedBondFuture> underlyingFuture()
		{
		  return underlyingFuture_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1574023291: // securityId
			  return ((ResolvedBondFutureOption) bean).SecurityId;
			case -219971059: // putCall
			  return ((ResolvedBondFutureOption) bean).PutCall;
			case 50946231: // strikePrice
			  return ((ResolvedBondFutureOption) bean).StrikePrice;
			case -1289159373: // expiry
			  return ((ResolvedBondFutureOption) bean).Expiry;
			case -1257652838: // premiumStyle
			  return ((ResolvedBondFutureOption) bean).PremiumStyle;
			case -142444: // rounding
			  return ((ResolvedBondFutureOption) bean).Rounding;
			case -165476480: // underlyingFuture
			  return ((ResolvedBondFutureOption) bean).UnderlyingFuture;
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
	  /// The bean-builder for {@code ResolvedBondFutureOption}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ResolvedBondFutureOption>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SecurityId securityId_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PutCall putCall_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double strikePrice_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ZonedDateTime expiry_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FutureOptionPremiumStyle premiumStyle_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Rounding rounding_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ResolvedBondFuture underlyingFuture_Renamed;

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
		internal Builder(ResolvedBondFutureOption beanToCopy)
		{
		  this.securityId_Renamed = beanToCopy.SecurityId;
		  this.putCall_Renamed = beanToCopy.PutCall;
		  this.strikePrice_Renamed = beanToCopy.StrikePrice;
		  this.expiry_Renamed = beanToCopy.Expiry;
		  this.premiumStyle_Renamed = beanToCopy.PremiumStyle;
		  this.rounding_Renamed = beanToCopy.Rounding;
		  this.underlyingFuture_Renamed = beanToCopy.UnderlyingFuture;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1574023291: // securityId
			  return securityId_Renamed;
			case -219971059: // putCall
			  return putCall_Renamed;
			case 50946231: // strikePrice
			  return strikePrice_Renamed;
			case -1289159373: // expiry
			  return expiry_Renamed;
			case -1257652838: // premiumStyle
			  return premiumStyle_Renamed;
			case -142444: // rounding
			  return rounding_Renamed;
			case -165476480: // underlyingFuture
			  return underlyingFuture_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1574023291: // securityId
			  this.securityId_Renamed = (SecurityId) newValue;
			  break;
			case -219971059: // putCall
			  this.putCall_Renamed = (PutCall) newValue;
			  break;
			case 50946231: // strikePrice
			  this.strikePrice_Renamed = (double?) newValue.Value;
			  break;
			case -1289159373: // expiry
			  this.expiry_Renamed = (ZonedDateTime) newValue;
			  break;
			case -1257652838: // premiumStyle
			  this.premiumStyle_Renamed = (FutureOptionPremiumStyle) newValue;
			  break;
			case -142444: // rounding
			  this.rounding_Renamed = (Rounding) newValue;
			  break;
			case -165476480: // underlyingFuture
			  this.underlyingFuture_Renamed = (ResolvedBondFuture) newValue;
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

		public override ResolvedBondFutureOption build()
		{
		  return new ResolvedBondFutureOption(securityId_Renamed, putCall_Renamed, strikePrice_Renamed, expiry_Renamed, premiumStyle_Renamed, rounding_Renamed, underlyingFuture_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the security identifier.
		/// <para>
		/// This identifier uniquely identifies the security within the system.
		/// </para>
		/// </summary>
		/// <param name="securityId">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder securityId(SecurityId securityId)
		{
		  JodaBeanUtils.notNull(securityId, "securityId");
		  this.securityId_Renamed = securityId;
		  return this;
		}

		/// <summary>
		/// Sets whether the option is put or call.
		/// <para>
		/// A call gives the owner the right, but not obligation, to buy the underlying at
		/// an agreed price in the future. A put gives a similar option to sell.
		/// </para>
		/// </summary>
		/// <param name="putCall">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder putCall(PutCall putCall)
		{
		  this.putCall_Renamed = putCall;
		  return this;
		}

		/// <summary>
		/// Sets the strike price, represented in decimal form.
		/// <para>
		/// This is the price at which the option applies and refers to the price of the underlying future.
		/// This must be represented in decimal form.
		/// </para>
		/// </summary>
		/// <param name="strikePrice">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder strikePrice(double strikePrice)
		{
		  this.strikePrice_Renamed = strikePrice;
		  return this;
		}

		/// <summary>
		/// Sets the expiry of the option.
		/// <para>
		/// The date must not be after last trade date of the underlying future.
		/// </para>
		/// </summary>
		/// <param name="expiry">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder expiry(ZonedDateTime expiry)
		{
		  JodaBeanUtils.notNull(expiry, "expiry");
		  this.expiry_Renamed = expiry;
		  return this;
		}

		/// <summary>
		/// Sets the style of the option premium.
		/// <para>
		/// The two options are daily margining and upfront premium.
		/// </para>
		/// </summary>
		/// <param name="premiumStyle">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder premiumStyle(FutureOptionPremiumStyle premiumStyle)
		{
		  JodaBeanUtils.notNull(premiumStyle, "premiumStyle");
		  this.premiumStyle_Renamed = premiumStyle;
		  return this;
		}

		/// <summary>
		/// Sets the definition of how to round the option price, defaulted to no rounding.
		/// <para>
		/// The price is represented in decimal form, not percentage form.
		/// As such, the decimal places expressed by the rounding refers to this decimal form.
		/// For example, the common market price of 99.7125 is represented as 0.997125 which
		/// has 6 decimal places.
		/// </para>
		/// </summary>
		/// <param name="rounding">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder rounding(Rounding rounding)
		{
		  JodaBeanUtils.notNull(rounding, "rounding");
		  this.rounding_Renamed = rounding;
		  return this;
		}

		/// <summary>
		/// Sets the underlying future. </summary>
		/// <param name="underlyingFuture">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder underlyingFuture(ResolvedBondFuture underlyingFuture)
		{
		  JodaBeanUtils.notNull(underlyingFuture, "underlyingFuture");
		  this.underlyingFuture_Renamed = underlyingFuture;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(256);
		  buf.Append("ResolvedBondFutureOption.Builder{");
		  buf.Append("securityId").Append('=').Append(JodaBeanUtils.ToString(securityId_Renamed)).Append(',').Append(' ');
		  buf.Append("putCall").Append('=').Append(JodaBeanUtils.ToString(putCall_Renamed)).Append(',').Append(' ');
		  buf.Append("strikePrice").Append('=').Append(JodaBeanUtils.ToString(strikePrice_Renamed)).Append(',').Append(' ');
		  buf.Append("expiry").Append('=').Append(JodaBeanUtils.ToString(expiry_Renamed)).Append(',').Append(' ');
		  buf.Append("premiumStyle").Append('=').Append(JodaBeanUtils.ToString(premiumStyle_Renamed)).Append(',').Append(' ');
		  buf.Append("rounding").Append('=').Append(JodaBeanUtils.ToString(rounding_Renamed)).Append(',').Append(' ');
		  buf.Append("underlyingFuture").Append('=').Append(JodaBeanUtils.ToString(underlyingFuture_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}