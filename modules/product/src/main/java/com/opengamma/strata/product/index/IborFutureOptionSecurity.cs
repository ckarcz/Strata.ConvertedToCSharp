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
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Rounding = com.opengamma.strata.basics.value.Rounding;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using PutCall = com.opengamma.strata.product.common.PutCall;
	using FutureOptionPremiumStyle = com.opengamma.strata.product.option.FutureOptionPremiumStyle;

	/// <summary>
	/// A security representing a futures option contract, based on an Ibor index.
	/// <para>
	/// An Ibor future option is a financial instrument that provides an option based on the future value of
	/// an Ibor index interest rate. The option is American, exercised at any point up to the exercise time.
	/// It handles options with either daily margining or upfront premium.
	/// </para>
	/// <para>
	/// An Ibor future option is also known as a <i>STIR future option</i> (Short Term Interest Rate).
	/// 
	/// <h4>Price</h4>
	/// The price of an Ibor future option is based on the price of the underlying future, the volatility
	/// and the time to expiry. The price of the at-the-money option tends to zero as expiry approaches.
	/// </para>
	/// <para>
	/// Strata uses <i>decimal prices</i> for Ibor future options in the trade model, pricers and market data.
	/// The decimal price is based on the decimal rate equivalent to the percentage.
	/// For example, an option price of 0.2 is related to a futures price of 99.32 that implies an
	/// interest rate of 0.68%. Strata represents the price of the future as 0.9932 and thus
	/// represents the price of the option as 0.002.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class IborFutureOptionSecurity implements com.opengamma.strata.product.Security, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class IborFutureOptionSecurity : Security, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.product.SecurityInfo info;
		private readonly SecurityInfo info;
	  /// <summary>
	  /// The currency that the option is traded in.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.currency.Currency currency;
	  private readonly Currency currency;
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
	  /// The strike price, in decimal form.
	  /// <para>
	  /// This is the price at which the option applies and refers to the price of the underlying future.
	  /// The rate implied by the strike can take negative values.
	  /// </para>
	  /// <para>
	  /// Strata uses <i>decimal prices</i> for Ibor futures in the trade model, pricers and market data.
	  /// The decimal price is based on the decimal rate equivalent to the percentage.
	  /// For example, a price of 99.32 implies an interest rate of 0.68% which is represented in Strata by 0.9932.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double strikePrice;
	  private readonly double strikePrice;
	  /// <summary>
	  /// The expiry date of the option.
	  /// <para>
	  /// The expiry date is related to the expiry time and time-zone.
	  /// The date must not be after last trade date of the underlying future.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate expiryDate;
	  private readonly LocalDate expiryDate;
	  /// <summary>
	  /// The expiry time of the option.
	  /// <para>
	  /// The expiry time is related to the expiry date and time-zone.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalTime expiryTime;
	  private readonly LocalTime expiryTime;
	  /// <summary>
	  /// The time-zone of the expiry time.
	  /// <para>
	  /// The expiry time-zone is related to the expiry date and time.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.ZoneId expiryZone;
	  private readonly ZoneId expiryZone;
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
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.value.Rounding rounding;
	  private readonly Rounding rounding;
	  /// <summary>
	  /// The identifier of the underlying future.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.SecurityId underlyingFutureId;
	  private readonly SecurityId underlyingFutureId;

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
		ArgChecker.isTrue(strikePrice < 2, "Strike price must be in decimal form, such as 0.993 for a 0.7% rate, but was: {}", strikePrice);
	  }

	  //-------------------------------------------------------------------------
	  public ImmutableSet<SecurityId> UnderlyingIds
	  {
		  get
		  {
			return ImmutableSet.of(underlyingFutureId);
		  }
	  }

	  //-------------------------------------------------------------------------
	  public IborFutureOptionSecurity withInfo(SecurityInfo info)
	  {
		return toBuilder().info(info).build();
	  }

	  //-------------------------------------------------------------------------
	  public IborFutureOption createProduct(ReferenceData refData)
	  {
		Security security = refData.getValue(underlyingFutureId);
		if (!(security is IborFutureSecurity))
		{
		  throw new System.InvalidCastException(Messages.format("{} underlying future '{}' resolved to '{}' when '{}' was expected", typeof(IborFutureOptionSecurity).Name, underlyingFutureId, security.GetType().Name, typeof(IborFutureSecurity).Name));
		}
		IborFutureSecurity futureSec = (IborFutureSecurity) security;
		IborFuture underlying = futureSec.createProduct(refData);
		return new IborFutureOption(SecurityId, putCall, strikePrice, expiryDate, expiryTime, expiryZone, premiumStyle, rounding, underlying);
	  }

	  public IborFutureOptionTrade createTrade(TradeInfo info, double quantity, double tradePrice, ReferenceData refData)
	  {

		return new IborFutureOptionTrade(info, createProduct(refData), quantity, tradePrice);
	  }

	  public IborFutureOptionPosition createPosition(PositionInfo positionInfo, double quantity, ReferenceData refData)
	  {
		return IborFutureOptionPosition.ofNet(positionInfo, createProduct(refData), quantity);
	  }

	  public IborFutureOptionPosition createPosition(PositionInfo positionInfo, double longQuantity, double shortQuantity, ReferenceData refData)
	  {

		return IborFutureOptionPosition.ofLongShort(positionInfo, createProduct(refData), longQuantity, shortQuantity);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborFutureOptionSecurity}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static IborFutureOptionSecurity.Meta meta()
	  {
		return IborFutureOptionSecurity.Meta.INSTANCE;
	  }

	  static IborFutureOptionSecurity()
	  {
		MetaBean.register(IborFutureOptionSecurity.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static IborFutureOptionSecurity.Builder builder()
	  {
		return new IborFutureOptionSecurity.Builder();
	  }

	  private IborFutureOptionSecurity(SecurityInfo info, Currency currency, PutCall putCall, double strikePrice, LocalDate expiryDate, LocalTime expiryTime, ZoneId expiryZone, FutureOptionPremiumStyle premiumStyle, Rounding rounding, SecurityId underlyingFutureId)
	  {
		JodaBeanUtils.notNull(info, "info");
		JodaBeanUtils.notNull(currency, "currency");
		JodaBeanUtils.notNull(expiryDate, "expiryDate");
		JodaBeanUtils.notNull(expiryTime, "expiryTime");
		JodaBeanUtils.notNull(expiryZone, "expiryZone");
		JodaBeanUtils.notNull(premiumStyle, "premiumStyle");
		JodaBeanUtils.notNull(rounding, "rounding");
		JodaBeanUtils.notNull(underlyingFutureId, "underlyingFutureId");
		this.info = info;
		this.currency = currency;
		this.putCall = putCall;
		this.strikePrice = strikePrice;
		this.expiryDate = expiryDate;
		this.expiryTime = expiryTime;
		this.expiryZone = expiryZone;
		this.premiumStyle = premiumStyle;
		this.rounding = rounding;
		this.underlyingFutureId = underlyingFutureId;
		validate();
	  }

	  public override IborFutureOptionSecurity.Meta metaBean()
	  {
		return IborFutureOptionSecurity.Meta.INSTANCE;
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
	  /// <summary>
	  /// Gets the currency that the option is traded in. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public override Currency Currency
	  {
		  get
		  {
			return currency;
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
	  /// Gets the strike price, in decimal form.
	  /// <para>
	  /// This is the price at which the option applies and refers to the price of the underlying future.
	  /// The rate implied by the strike can take negative values.
	  /// </para>
	  /// <para>
	  /// Strata uses <i>decimal prices</i> for Ibor futures in the trade model, pricers and market data.
	  /// The decimal price is based on the decimal rate equivalent to the percentage.
	  /// For example, a price of 99.32 implies an interest rate of 0.68% which is represented in Strata by 0.9932.
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
	  /// Gets the expiry date of the option.
	  /// <para>
	  /// The expiry date is related to the expiry time and time-zone.
	  /// The date must not be after last trade date of the underlying future.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate ExpiryDate
	  {
		  get
		  {
			return expiryDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the expiry time of the option.
	  /// <para>
	  /// The expiry time is related to the expiry date and time-zone.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalTime ExpiryTime
	  {
		  get
		  {
			return expiryTime;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the time-zone of the expiry time.
	  /// <para>
	  /// The expiry time-zone is related to the expiry date and time.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ZoneId ExpiryZone
	  {
		  get
		  {
			return expiryZone;
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
	  /// Gets the identifier of the underlying future. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SecurityId UnderlyingFutureId
	  {
		  get
		  {
			return underlyingFutureId;
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
		  IborFutureOptionSecurity other = (IborFutureOptionSecurity) obj;
		  return JodaBeanUtils.equal(info, other.info) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(putCall, other.putCall) && JodaBeanUtils.equal(strikePrice, other.strikePrice) && JodaBeanUtils.equal(expiryDate, other.expiryDate) && JodaBeanUtils.equal(expiryTime, other.expiryTime) && JodaBeanUtils.equal(expiryZone, other.expiryZone) && JodaBeanUtils.equal(premiumStyle, other.premiumStyle) && JodaBeanUtils.equal(rounding, other.rounding) && JodaBeanUtils.equal(underlyingFutureId, other.underlyingFutureId);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(info);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(putCall);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(strikePrice);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(expiryDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(expiryTime);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(expiryZone);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(premiumStyle);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rounding);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(underlyingFutureId);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(352);
		buf.Append("IborFutureOptionSecurity{");
		buf.Append("info").Append('=').Append(info).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("putCall").Append('=').Append(putCall).Append(',').Append(' ');
		buf.Append("strikePrice").Append('=').Append(strikePrice).Append(',').Append(' ');
		buf.Append("expiryDate").Append('=').Append(expiryDate).Append(',').Append(' ');
		buf.Append("expiryTime").Append('=').Append(expiryTime).Append(',').Append(' ');
		buf.Append("expiryZone").Append('=').Append(expiryZone).Append(',').Append(' ');
		buf.Append("premiumStyle").Append('=').Append(premiumStyle).Append(',').Append(' ');
		buf.Append("rounding").Append('=').Append(rounding).Append(',').Append(' ');
		buf.Append("underlyingFutureId").Append('=').Append(JodaBeanUtils.ToString(underlyingFutureId));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborFutureOptionSecurity}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  info_Renamed = DirectMetaProperty.ofImmutable(this, "info", typeof(IborFutureOptionSecurity), typeof(SecurityInfo));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(IborFutureOptionSecurity), typeof(Currency));
			  putCall_Renamed = DirectMetaProperty.ofImmutable(this, "putCall", typeof(IborFutureOptionSecurity), typeof(PutCall));
			  strikePrice_Renamed = DirectMetaProperty.ofImmutable(this, "strikePrice", typeof(IborFutureOptionSecurity), Double.TYPE);
			  expiryDate_Renamed = DirectMetaProperty.ofImmutable(this, "expiryDate", typeof(IborFutureOptionSecurity), typeof(LocalDate));
			  expiryTime_Renamed = DirectMetaProperty.ofImmutable(this, "expiryTime", typeof(IborFutureOptionSecurity), typeof(LocalTime));
			  expiryZone_Renamed = DirectMetaProperty.ofImmutable(this, "expiryZone", typeof(IborFutureOptionSecurity), typeof(ZoneId));
			  premiumStyle_Renamed = DirectMetaProperty.ofImmutable(this, "premiumStyle", typeof(IborFutureOptionSecurity), typeof(FutureOptionPremiumStyle));
			  rounding_Renamed = DirectMetaProperty.ofImmutable(this, "rounding", typeof(IborFutureOptionSecurity), typeof(Rounding));
			  underlyingFutureId_Renamed = DirectMetaProperty.ofImmutable(this, "underlyingFutureId", typeof(IborFutureOptionSecurity), typeof(SecurityId));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "info", "currency", "putCall", "strikePrice", "expiryDate", "expiryTime", "expiryZone", "premiumStyle", "rounding", "underlyingFutureId");
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
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
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
		/// The meta-property for the {@code expiryDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> expiryDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code expiryTime} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalTime> expiryTime_Renamed;
		/// <summary>
		/// The meta-property for the {@code expiryZone} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ZoneId> expiryZone_Renamed;
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
		/// The meta-property for the {@code underlyingFutureId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SecurityId> underlyingFutureId_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "info", "currency", "putCall", "strikePrice", "expiryDate", "expiryTime", "expiryZone", "premiumStyle", "rounding", "underlyingFutureId");
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
			case 575402001: // currency
			  return currency_Renamed;
			case -219971059: // putCall
			  return putCall_Renamed;
			case 50946231: // strikePrice
			  return strikePrice_Renamed;
			case -816738431: // expiryDate
			  return expiryDate_Renamed;
			case -816254304: // expiryTime
			  return expiryTime_Renamed;
			case -816069761: // expiryZone
			  return expiryZone_Renamed;
			case -1257652838: // premiumStyle
			  return premiumStyle_Renamed;
			case -142444: // rounding
			  return rounding_Renamed;
			case -109104965: // underlyingFutureId
			  return underlyingFutureId_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override IborFutureOptionSecurity.Builder builder()
		{
		  return new IborFutureOptionSecurity.Builder();
		}

		public override Type beanType()
		{
		  return typeof(IborFutureOptionSecurity);
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

		/// <summary>
		/// The meta-property for the {@code currency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> currency()
		{
		  return currency_Renamed;
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
		/// The meta-property for the {@code expiryDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> expiryDate()
		{
		  return expiryDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code expiryTime} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalTime> expiryTime()
		{
		  return expiryTime_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code expiryZone} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ZoneId> expiryZone()
		{
		  return expiryZone_Renamed;
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
		/// The meta-property for the {@code underlyingFutureId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SecurityId> underlyingFutureId()
		{
		  return underlyingFutureId_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  return ((IborFutureOptionSecurity) bean).Info;
			case 575402001: // currency
			  return ((IborFutureOptionSecurity) bean).Currency;
			case -219971059: // putCall
			  return ((IborFutureOptionSecurity) bean).PutCall;
			case 50946231: // strikePrice
			  return ((IborFutureOptionSecurity) bean).StrikePrice;
			case -816738431: // expiryDate
			  return ((IborFutureOptionSecurity) bean).ExpiryDate;
			case -816254304: // expiryTime
			  return ((IborFutureOptionSecurity) bean).ExpiryTime;
			case -816069761: // expiryZone
			  return ((IborFutureOptionSecurity) bean).ExpiryZone;
			case -1257652838: // premiumStyle
			  return ((IborFutureOptionSecurity) bean).PremiumStyle;
			case -142444: // rounding
			  return ((IborFutureOptionSecurity) bean).Rounding;
			case -109104965: // underlyingFutureId
			  return ((IborFutureOptionSecurity) bean).UnderlyingFutureId;
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
	  /// The bean-builder for {@code IborFutureOptionSecurity}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<IborFutureOptionSecurity>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SecurityInfo info_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Currency currency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PutCall putCall_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double strikePrice_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate expiryDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalTime expiryTime_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ZoneId expiryZone_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FutureOptionPremiumStyle premiumStyle_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Rounding rounding_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SecurityId underlyingFutureId_Renamed;

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
		internal Builder(IborFutureOptionSecurity beanToCopy)
		{
		  this.info_Renamed = beanToCopy.Info;
		  this.currency_Renamed = beanToCopy.Currency;
		  this.putCall_Renamed = beanToCopy.PutCall;
		  this.strikePrice_Renamed = beanToCopy.StrikePrice;
		  this.expiryDate_Renamed = beanToCopy.ExpiryDate;
		  this.expiryTime_Renamed = beanToCopy.ExpiryTime;
		  this.expiryZone_Renamed = beanToCopy.ExpiryZone;
		  this.premiumStyle_Renamed = beanToCopy.PremiumStyle;
		  this.rounding_Renamed = beanToCopy.Rounding;
		  this.underlyingFutureId_Renamed = beanToCopy.UnderlyingFutureId;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  return info_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case -219971059: // putCall
			  return putCall_Renamed;
			case 50946231: // strikePrice
			  return strikePrice_Renamed;
			case -816738431: // expiryDate
			  return expiryDate_Renamed;
			case -816254304: // expiryTime
			  return expiryTime_Renamed;
			case -816069761: // expiryZone
			  return expiryZone_Renamed;
			case -1257652838: // premiumStyle
			  return premiumStyle_Renamed;
			case -142444: // rounding
			  return rounding_Renamed;
			case -109104965: // underlyingFutureId
			  return underlyingFutureId_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  this.info_Renamed = (SecurityInfo) newValue;
			  break;
			case 575402001: // currency
			  this.currency_Renamed = (Currency) newValue;
			  break;
			case -219971059: // putCall
			  this.putCall_Renamed = (PutCall) newValue;
			  break;
			case 50946231: // strikePrice
			  this.strikePrice_Renamed = (double?) newValue.Value;
			  break;
			case -816738431: // expiryDate
			  this.expiryDate_Renamed = (LocalDate) newValue;
			  break;
			case -816254304: // expiryTime
			  this.expiryTime_Renamed = (LocalTime) newValue;
			  break;
			case -816069761: // expiryZone
			  this.expiryZone_Renamed = (ZoneId) newValue;
			  break;
			case -1257652838: // premiumStyle
			  this.premiumStyle_Renamed = (FutureOptionPremiumStyle) newValue;
			  break;
			case -142444: // rounding
			  this.rounding_Renamed = (Rounding) newValue;
			  break;
			case -109104965: // underlyingFutureId
			  this.underlyingFutureId_Renamed = (SecurityId) newValue;
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

		public override IborFutureOptionSecurity build()
		{
		  return new IborFutureOptionSecurity(info_Renamed, currency_Renamed, putCall_Renamed, strikePrice_Renamed, expiryDate_Renamed, expiryTime_Renamed, expiryZone_Renamed, premiumStyle_Renamed, rounding_Renamed, underlyingFutureId_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the standard security information.
		/// <para>
		/// This includes the security identifier.
		/// </para>
		/// </summary>
		/// <param name="info">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder info(SecurityInfo info)
		{
		  JodaBeanUtils.notNull(info, "info");
		  this.info_Renamed = info;
		  return this;
		}

		/// <summary>
		/// Sets the currency that the option is traded in. </summary>
		/// <param name="currency">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder currency(Currency currency)
		{
		  JodaBeanUtils.notNull(currency, "currency");
		  this.currency_Renamed = currency;
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
		/// Sets the strike price, in decimal form.
		/// <para>
		/// This is the price at which the option applies and refers to the price of the underlying future.
		/// The rate implied by the strike can take negative values.
		/// </para>
		/// <para>
		/// Strata uses <i>decimal prices</i> for Ibor futures in the trade model, pricers and market data.
		/// The decimal price is based on the decimal rate equivalent to the percentage.
		/// For example, a price of 99.32 implies an interest rate of 0.68% which is represented in Strata by 0.9932.
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
		/// Sets the expiry date of the option.
		/// <para>
		/// The expiry date is related to the expiry time and time-zone.
		/// The date must not be after last trade date of the underlying future.
		/// </para>
		/// </summary>
		/// <param name="expiryDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder expiryDate(LocalDate expiryDate)
		{
		  JodaBeanUtils.notNull(expiryDate, "expiryDate");
		  this.expiryDate_Renamed = expiryDate;
		  return this;
		}

		/// <summary>
		/// Sets the expiry time of the option.
		/// <para>
		/// The expiry time is related to the expiry date and time-zone.
		/// </para>
		/// </summary>
		/// <param name="expiryTime">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder expiryTime(LocalTime expiryTime)
		{
		  JodaBeanUtils.notNull(expiryTime, "expiryTime");
		  this.expiryTime_Renamed = expiryTime;
		  return this;
		}

		/// <summary>
		/// Sets the time-zone of the expiry time.
		/// <para>
		/// The expiry time-zone is related to the expiry date and time.
		/// </para>
		/// </summary>
		/// <param name="expiryZone">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder expiryZone(ZoneId expiryZone)
		{
		  JodaBeanUtils.notNull(expiryZone, "expiryZone");
		  this.expiryZone_Renamed = expiryZone;
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
		/// Sets the identifier of the underlying future. </summary>
		/// <param name="underlyingFutureId">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder underlyingFutureId(SecurityId underlyingFutureId)
		{
		  JodaBeanUtils.notNull(underlyingFutureId, "underlyingFutureId");
		  this.underlyingFutureId_Renamed = underlyingFutureId;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(352);
		  buf.Append("IborFutureOptionSecurity.Builder{");
		  buf.Append("info").Append('=').Append(JodaBeanUtils.ToString(info_Renamed)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("putCall").Append('=').Append(JodaBeanUtils.ToString(putCall_Renamed)).Append(',').Append(' ');
		  buf.Append("strikePrice").Append('=').Append(JodaBeanUtils.ToString(strikePrice_Renamed)).Append(',').Append(' ');
		  buf.Append("expiryDate").Append('=').Append(JodaBeanUtils.ToString(expiryDate_Renamed)).Append(',').Append(' ');
		  buf.Append("expiryTime").Append('=').Append(JodaBeanUtils.ToString(expiryTime_Renamed)).Append(',').Append(' ');
		  buf.Append("expiryZone").Append('=').Append(JodaBeanUtils.ToString(expiryZone_Renamed)).Append(',').Append(' ');
		  buf.Append("premiumStyle").Append('=').Append(JodaBeanUtils.ToString(premiumStyle_Renamed)).Append(',').Append(' ');
		  buf.Append("rounding").Append('=').Append(JodaBeanUtils.ToString(rounding_Renamed)).Append(',').Append(' ');
		  buf.Append("underlyingFutureId").Append('=').Append(JodaBeanUtils.ToString(underlyingFutureId_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}