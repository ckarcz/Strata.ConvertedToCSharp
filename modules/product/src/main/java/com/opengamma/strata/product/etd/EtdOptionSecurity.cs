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

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using PutCall = com.opengamma.strata.product.common.PutCall;

	/// <summary>
	/// An instrument representing an exchange traded derivative (ETD) option.
	/// <para>
	/// A security representing a standardized contract that gives the buyer the right but not the obligation to
	/// buy or sell an underlying asset at an agreed price.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class EtdOptionSecurity implements EtdSecurity, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class EtdOptionSecurity : EtdSecurity, ImmutableBean
	{

	  /// <summary>
	  /// YearMonth format. </summary>
	  private static readonly DateTimeFormatter YM_FORMAT = DateTimeFormatter.ofPattern("MMMuu", Locale.UK);

	  /// <summary>
	  /// The standard security information.
	  /// <para>
	  /// This includes the security identifier.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.product.SecurityInfo info;
	  private readonly SecurityInfo info;
	  /// <summary>
	  /// The ID of the contract specification from which this security is derived.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final EtdContractSpecId contractSpecId;
	  private readonly EtdContractSpecId contractSpecId;
	  /// <summary>
	  /// The year-month of the expiry.
	  /// <para>
	  /// Expiry will occur on a date implied by the variant of the ETD.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.YearMonth expiry;
	  private readonly YearMonth expiry;
	  /// <summary>
	  /// The variant of ETD.
	  /// <para>
	  /// This captures the variant of the ETD. The most common variant is 'Monthly'.
	  /// Other variants are 'Weekly', 'Daily' and 'Flex'.
	  /// </para>
	  /// <para>
	  /// When building, this defaults to 'Monthly'.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final EtdVariant variant;
	  private readonly EtdVariant variant;
	  /// <summary>
	  /// The version of the option, defaulted to zero.
	  /// <para>
	  /// Some options can have multiple versions, representing some kind of change over time.
	  /// Version zero is the baseline, version one and later indicates some kind of change occurred.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative") private final int version;
	  private readonly int version;
	  /// <summary>
	  /// Whether the option is a put or call.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.common.PutCall putCall;
	  private readonly PutCall putCall;
	  /// <summary>
	  /// The strike price, in decimal form, may be negative.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double strikePrice;
	  private readonly double strikePrice;
	  /// <summary>
	  /// The expiry year-month of the underlying instrument.
	  /// <para>
	  /// If an option has an underlying instrument, the expiry of that instrument can be specified here.
	  /// For example, you can have an option expiring in March on the underlying March future, or on the underlying June future.
	  /// Not all options have an underlying instrument, thus the property is optional.
	  /// </para>
	  /// <para>
	  /// In many cases, the expiry of the underlying instrument is the same as the expiry of the option.
	  /// In this case, the expiry is often omitted, even though it probably should not be.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final java.time.YearMonth underlyingExpiryMonth;
	  private readonly YearMonth underlyingExpiryMonth;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a contract specification, expiry year-month, variant, version, put/call and strike price.
	  /// <para>
	  /// The security identifier will be automatically created using <seealso cref="EtdIdUtils"/>.
	  /// The specification must be for an option.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="spec">  the option contract specification </param>
	  /// <param name="expiry">  the expiry year-month of the option </param>
	  /// <param name="variant">  the variant of the ETD, such as 'Monthly', 'Weekly, 'Daily' or 'Flex' </param>
	  /// <param name="version">  the non-negative version, zero if versioning does not apply </param>
	  /// <param name="putCall">  whether the option is a put or call </param>
	  /// <param name="strikePrice">  the strike price of the option </param>
	  /// <returns> an option security based on this contract specification </returns>
	  /// <exception cref="IllegalStateException"> if the product type of the contract specification is not {@code OPTION} </exception>
	  public static EtdOptionSecurity of(EtdContractSpec spec, YearMonth expiry, EtdVariant variant, int version, PutCall putCall, double strikePrice)
	  {

		return of(spec, expiry, variant, version, putCall, strikePrice, null);
	  }

	  /// <summary>
	  /// Obtains an instance from a contract specification, expiry year-month, variant,
	  /// version, put/call, strike price and underlying expiry.
	  /// <para>
	  /// The security identifier will be automatically created using <seealso cref="EtdIdUtils"/>.
	  /// The specification must be for an option.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="spec">  the option contract specification </param>
	  /// <param name="expiry">  the expiry year-month of the option </param>
	  /// <param name="variant">  the variant of the ETD, such as 'Monthly', 'Weekly, 'Daily' or 'Flex' </param>
	  /// <param name="version">  the non-negative version, zero if versioning does not apply </param>
	  /// <param name="putCall">  whether the option is a put or call </param>
	  /// <param name="strikePrice">  the strike price of the option </param>
	  /// <param name="underlyingExpiryMonth">  the expiry of the underlying instrument, such as a future, may be null </param>
	  /// <returns> an option security based on this contract specification </returns>
	  /// <exception cref="IllegalStateException"> if the product type of the contract specification is not {@code OPTION} </exception>
	  public static EtdOptionSecurity of(EtdContractSpec spec, YearMonth expiry, EtdVariant variant, int version, PutCall putCall, double strikePrice, YearMonth underlyingExpiryMonth)
	  {

		if (spec.Type != EtdType.OPTION)
		{
		  throw new System.InvalidOperationException(Messages.format("Cannot create an EtdOptionSecurity from a contract specification of type '{}'", spec.Type));
		}
		SecurityId securityId = EtdIdUtils.optionId(spec.ExchangeId, spec.ContractCode, expiry, variant, version, putCall, strikePrice, underlyingExpiryMonth);
		return EtdOptionSecurity.builder().info(SecurityInfo.of(securityId, spec.PriceInfo)).contractSpecId(spec.Id).expiry(expiry).variant(variant).version(version).putCall(putCall).strikePrice(strikePrice).underlyingExpiryMonth(underlyingExpiryMonth).build();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.variant_Renamed = EtdVariant.MONTHLY;
	  }

	  //-------------------------------------------------------------------------
	  public EtdType Type
	  {
		  get
		  {
			return EtdType.OPTION;
		  }
	  }

	  public EtdOptionSecurity withInfo(SecurityInfo info)
	  {
		return toBuilder().info(info).build();
	  }

	  public EtdOptionSecurity createProduct(ReferenceData refData)
	  {
		return this;
	  }

	  public EtdOptionTrade createTrade(TradeInfo tradeInfo, double quantity, double tradePrice, ReferenceData refData)
	  {
		return EtdOptionTrade.builder().info(tradeInfo).quantity(quantity).price(tradePrice).security(this).build();
	  }

	  public EtdOptionPosition createPosition(PositionInfo positionInfo, double quantity, ReferenceData refData)
	  {
		return EtdOptionPosition.ofNet(positionInfo, this, quantity);
	  }

	  public EtdOptionPosition createPosition(PositionInfo positionInfo, double longQuantity, double shortQuantity, ReferenceData refData)
	  {

		return EtdOptionPosition.ofLongShort(positionInfo, this, longQuantity, shortQuantity);
	  }

	  /// <summary>
	  /// Summarizes this ETD option into string form.
	  /// </summary>
	  /// <returns> the summary description </returns>
	  public string summaryDescription()
	  {
		string putCallStr = putCall == PutCall.PUT ? "P" : "C";
		string versionCode = version > 0 ? "V" + version + " " : "";

		NumberFormat f = NumberFormat.getIntegerInstance(Locale.ENGLISH);
		f.GroupingUsed = false;
		f.MaximumFractionDigits = 8;
		string strikeStr = f.format(strikePrice).replace('-', 'M');

		return expiry.format(YM_FORMAT) + variant.Code + " " + versionCode + putCallStr + strikeStr;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code EtdOptionSecurity}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static EtdOptionSecurity.Meta meta()
	  {
		return EtdOptionSecurity.Meta.INSTANCE;
	  }

	  static EtdOptionSecurity()
	  {
		MetaBean.register(EtdOptionSecurity.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static EtdOptionSecurity.Builder builder()
	  {
		return new EtdOptionSecurity.Builder();
	  }

	  private EtdOptionSecurity(SecurityInfo info, EtdContractSpecId contractSpecId, YearMonth expiry, EtdVariant variant, int version, PutCall putCall, double strikePrice, YearMonth underlyingExpiryMonth)
	  {
		JodaBeanUtils.notNull(info, "info");
		JodaBeanUtils.notNull(contractSpecId, "contractSpecId");
		JodaBeanUtils.notNull(expiry, "expiry");
		JodaBeanUtils.notNull(variant, "variant");
		ArgChecker.notNegative(version, "version");
		JodaBeanUtils.notNull(putCall, "putCall");
		this.info = info;
		this.contractSpecId = contractSpecId;
		this.expiry = expiry;
		this.variant = variant;
		this.version = version;
		this.putCall = putCall;
		this.strikePrice = strikePrice;
		this.underlyingExpiryMonth = underlyingExpiryMonth;
	  }

	  public override EtdOptionSecurity.Meta metaBean()
	  {
		return EtdOptionSecurity.Meta.INSTANCE;
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
	  /// Gets the ID of the contract specification from which this security is derived. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public EtdContractSpecId ContractSpecId
	  {
		  get
		  {
			return contractSpecId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the year-month of the expiry.
	  /// <para>
	  /// Expiry will occur on a date implied by the variant of the ETD.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public YearMonth Expiry
	  {
		  get
		  {
			return expiry;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the variant of ETD.
	  /// <para>
	  /// This captures the variant of the ETD. The most common variant is 'Monthly'.
	  /// Other variants are 'Weekly', 'Daily' and 'Flex'.
	  /// </para>
	  /// <para>
	  /// When building, this defaults to 'Monthly'.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public EtdVariant Variant
	  {
		  get
		  {
			return variant;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the version of the option, defaulted to zero.
	  /// <para>
	  /// Some options can have multiple versions, representing some kind of change over time.
	  /// Version zero is the baseline, version one and later indicates some kind of change occurred.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public int Version
	  {
		  get
		  {
			return version;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets whether the option is a put or call. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PutCall PutCall
	  {
		  get
		  {
			return putCall;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the strike price, in decimal form, may be negative. </summary>
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
	  /// Gets the expiry year-month of the underlying instrument.
	  /// <para>
	  /// If an option has an underlying instrument, the expiry of that instrument can be specified here.
	  /// For example, you can have an option expiring in March on the underlying March future, or on the underlying June future.
	  /// Not all options have an underlying instrument, thus the property is optional.
	  /// </para>
	  /// <para>
	  /// In many cases, the expiry of the underlying instrument is the same as the expiry of the option.
	  /// In this case, the expiry is often omitted, even though it probably should not be.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<YearMonth> UnderlyingExpiryMonth
	  {
		  get
		  {
			return Optional.ofNullable(underlyingExpiryMonth);
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
		  EtdOptionSecurity other = (EtdOptionSecurity) obj;
		  return JodaBeanUtils.equal(info, other.info) && JodaBeanUtils.equal(contractSpecId, other.contractSpecId) && JodaBeanUtils.equal(expiry, other.expiry) && JodaBeanUtils.equal(variant, other.variant) && (version == other.version) && JodaBeanUtils.equal(putCall, other.putCall) && JodaBeanUtils.equal(strikePrice, other.strikePrice) && JodaBeanUtils.equal(underlyingExpiryMonth, other.underlyingExpiryMonth);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(info);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(contractSpecId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(expiry);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(variant);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(version);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(putCall);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(strikePrice);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(underlyingExpiryMonth);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(288);
		buf.Append("EtdOptionSecurity{");
		buf.Append("info").Append('=').Append(info).Append(',').Append(' ');
		buf.Append("contractSpecId").Append('=').Append(contractSpecId).Append(',').Append(' ');
		buf.Append("expiry").Append('=').Append(expiry).Append(',').Append(' ');
		buf.Append("variant").Append('=').Append(variant).Append(',').Append(' ');
		buf.Append("version").Append('=').Append(version).Append(',').Append(' ');
		buf.Append("putCall").Append('=').Append(putCall).Append(',').Append(' ');
		buf.Append("strikePrice").Append('=').Append(strikePrice).Append(',').Append(' ');
		buf.Append("underlyingExpiryMonth").Append('=').Append(JodaBeanUtils.ToString(underlyingExpiryMonth));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code EtdOptionSecurity}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  info_Renamed = DirectMetaProperty.ofImmutable(this, "info", typeof(EtdOptionSecurity), typeof(SecurityInfo));
			  contractSpecId_Renamed = DirectMetaProperty.ofImmutable(this, "contractSpecId", typeof(EtdOptionSecurity), typeof(EtdContractSpecId));
			  expiry_Renamed = DirectMetaProperty.ofImmutable(this, "expiry", typeof(EtdOptionSecurity), typeof(YearMonth));
			  variant_Renamed = DirectMetaProperty.ofImmutable(this, "variant", typeof(EtdOptionSecurity), typeof(EtdVariant));
			  version_Renamed = DirectMetaProperty.ofImmutable(this, "version", typeof(EtdOptionSecurity), Integer.TYPE);
			  putCall_Renamed = DirectMetaProperty.ofImmutable(this, "putCall", typeof(EtdOptionSecurity), typeof(PutCall));
			  strikePrice_Renamed = DirectMetaProperty.ofImmutable(this, "strikePrice", typeof(EtdOptionSecurity), Double.TYPE);
			  underlyingExpiryMonth_Renamed = DirectMetaProperty.ofImmutable(this, "underlyingExpiryMonth", typeof(EtdOptionSecurity), typeof(YearMonth));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "info", "contractSpecId", "expiry", "variant", "version", "putCall", "strikePrice", "underlyingExpiryMonth");
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
		/// The meta-property for the {@code contractSpecId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<EtdContractSpecId> contractSpecId_Renamed;
		/// <summary>
		/// The meta-property for the {@code expiry} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<YearMonth> expiry_Renamed;
		/// <summary>
		/// The meta-property for the {@code variant} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<EtdVariant> variant_Renamed;
		/// <summary>
		/// The meta-property for the {@code version} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<int> version_Renamed;
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
		/// The meta-property for the {@code underlyingExpiryMonth} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<YearMonth> underlyingExpiryMonth_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "info", "contractSpecId", "expiry", "variant", "version", "putCall", "strikePrice", "underlyingExpiryMonth");
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
			case 948987368: // contractSpecId
			  return contractSpecId_Renamed;
			case -1289159373: // expiry
			  return expiry_Renamed;
			case 236785797: // variant
			  return variant_Renamed;
			case 351608024: // version
			  return version_Renamed;
			case -219971059: // putCall
			  return putCall_Renamed;
			case 50946231: // strikePrice
			  return strikePrice_Renamed;
			case 1929351536: // underlyingExpiryMonth
			  return underlyingExpiryMonth_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override EtdOptionSecurity.Builder builder()
		{
		  return new EtdOptionSecurity.Builder();
		}

		public override Type beanType()
		{
		  return typeof(EtdOptionSecurity);
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
		/// The meta-property for the {@code contractSpecId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<EtdContractSpecId> contractSpecId()
		{
		  return contractSpecId_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code expiry} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<YearMonth> expiry()
		{
		  return expiry_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code variant} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<EtdVariant> variant()
		{
		  return variant_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code version} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<int> version()
		{
		  return version_Renamed;
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
		/// The meta-property for the {@code underlyingExpiryMonth} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<YearMonth> underlyingExpiryMonth()
		{
		  return underlyingExpiryMonth_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  return ((EtdOptionSecurity) bean).Info;
			case 948987368: // contractSpecId
			  return ((EtdOptionSecurity) bean).ContractSpecId;
			case -1289159373: // expiry
			  return ((EtdOptionSecurity) bean).Expiry;
			case 236785797: // variant
			  return ((EtdOptionSecurity) bean).Variant;
			case 351608024: // version
			  return ((EtdOptionSecurity) bean).Version;
			case -219971059: // putCall
			  return ((EtdOptionSecurity) bean).PutCall;
			case 50946231: // strikePrice
			  return ((EtdOptionSecurity) bean).StrikePrice;
			case 1929351536: // underlyingExpiryMonth
			  return ((EtdOptionSecurity) bean).underlyingExpiryMonth;
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
	  /// The bean-builder for {@code EtdOptionSecurity}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<EtdOptionSecurity>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SecurityInfo info_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal EtdContractSpecId contractSpecId_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal YearMonth expiry_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal EtdVariant variant_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal int version_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PutCall putCall_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double strikePrice_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal YearMonth underlyingExpiryMonth_Renamed;

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
		internal Builder(EtdOptionSecurity beanToCopy)
		{
		  this.info_Renamed = beanToCopy.Info;
		  this.contractSpecId_Renamed = beanToCopy.ContractSpecId;
		  this.expiry_Renamed = beanToCopy.Expiry;
		  this.variant_Renamed = beanToCopy.Variant;
		  this.version_Renamed = beanToCopy.Version;
		  this.putCall_Renamed = beanToCopy.PutCall;
		  this.strikePrice_Renamed = beanToCopy.StrikePrice;
		  this.underlyingExpiryMonth_Renamed = beanToCopy.underlyingExpiryMonth;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  return info_Renamed;
			case 948987368: // contractSpecId
			  return contractSpecId_Renamed;
			case -1289159373: // expiry
			  return expiry_Renamed;
			case 236785797: // variant
			  return variant_Renamed;
			case 351608024: // version
			  return version_Renamed;
			case -219971059: // putCall
			  return putCall_Renamed;
			case 50946231: // strikePrice
			  return strikePrice_Renamed;
			case 1929351536: // underlyingExpiryMonth
			  return underlyingExpiryMonth_Renamed;
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
			case 948987368: // contractSpecId
			  this.contractSpecId_Renamed = (EtdContractSpecId) newValue;
			  break;
			case -1289159373: // expiry
			  this.expiry_Renamed = (YearMonth) newValue;
			  break;
			case 236785797: // variant
			  this.variant_Renamed = (EtdVariant) newValue;
			  break;
			case 351608024: // version
			  this.version_Renamed = (int?) newValue.Value;
			  break;
			case -219971059: // putCall
			  this.putCall_Renamed = (PutCall) newValue;
			  break;
			case 50946231: // strikePrice
			  this.strikePrice_Renamed = (double?) newValue.Value;
			  break;
			case 1929351536: // underlyingExpiryMonth
			  this.underlyingExpiryMonth_Renamed = (YearMonth) newValue;
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

		public override EtdOptionSecurity build()
		{
		  return new EtdOptionSecurity(info_Renamed, contractSpecId_Renamed, expiry_Renamed, variant_Renamed, version_Renamed, putCall_Renamed, strikePrice_Renamed, underlyingExpiryMonth_Renamed);
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
		/// Sets the ID of the contract specification from which this security is derived. </summary>
		/// <param name="contractSpecId">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder contractSpecId(EtdContractSpecId contractSpecId)
		{
		  JodaBeanUtils.notNull(contractSpecId, "contractSpecId");
		  this.contractSpecId_Renamed = contractSpecId;
		  return this;
		}

		/// <summary>
		/// Sets the year-month of the expiry.
		/// <para>
		/// Expiry will occur on a date implied by the variant of the ETD.
		/// </para>
		/// </summary>
		/// <param name="expiry">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder expiry(YearMonth expiry)
		{
		  JodaBeanUtils.notNull(expiry, "expiry");
		  this.expiry_Renamed = expiry;
		  return this;
		}

		/// <summary>
		/// Sets the variant of ETD.
		/// <para>
		/// This captures the variant of the ETD. The most common variant is 'Monthly'.
		/// Other variants are 'Weekly', 'Daily' and 'Flex'.
		/// </para>
		/// <para>
		/// When building, this defaults to 'Monthly'.
		/// </para>
		/// </summary>
		/// <param name="variant">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder variant(EtdVariant variant)
		{
		  JodaBeanUtils.notNull(variant, "variant");
		  this.variant_Renamed = variant;
		  return this;
		}

		/// <summary>
		/// Sets the version of the option, defaulted to zero.
		/// <para>
		/// Some options can have multiple versions, representing some kind of change over time.
		/// Version zero is the baseline, version one and later indicates some kind of change occurred.
		/// </para>
		/// </summary>
		/// <param name="version">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder version(int version)
		{
		  ArgChecker.notNegative(version, "version");
		  this.version_Renamed = version;
		  return this;
		}

		/// <summary>
		/// Sets whether the option is a put or call. </summary>
		/// <param name="putCall">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder putCall(PutCall putCall)
		{
		  JodaBeanUtils.notNull(putCall, "putCall");
		  this.putCall_Renamed = putCall;
		  return this;
		}

		/// <summary>
		/// Sets the strike price, in decimal form, may be negative. </summary>
		/// <param name="strikePrice">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder strikePrice(double strikePrice)
		{
		  this.strikePrice_Renamed = strikePrice;
		  return this;
		}

		/// <summary>
		/// Sets the expiry year-month of the underlying instrument.
		/// <para>
		/// If an option has an underlying instrument, the expiry of that instrument can be specified here.
		/// For example, you can have an option expiring in March on the underlying March future, or on the underlying June future.
		/// Not all options have an underlying instrument, thus the property is optional.
		/// </para>
		/// <para>
		/// In many cases, the expiry of the underlying instrument is the same as the expiry of the option.
		/// In this case, the expiry is often omitted, even though it probably should not be.
		/// </para>
		/// </summary>
		/// <param name="underlyingExpiryMonth">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder underlyingExpiryMonth(YearMonth underlyingExpiryMonth)
		{
		  this.underlyingExpiryMonth_Renamed = underlyingExpiryMonth;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(288);
		  buf.Append("EtdOptionSecurity.Builder{");
		  buf.Append("info").Append('=').Append(JodaBeanUtils.ToString(info_Renamed)).Append(',').Append(' ');
		  buf.Append("contractSpecId").Append('=').Append(JodaBeanUtils.ToString(contractSpecId_Renamed)).Append(',').Append(' ');
		  buf.Append("expiry").Append('=').Append(JodaBeanUtils.ToString(expiry_Renamed)).Append(',').Append(' ');
		  buf.Append("variant").Append('=').Append(JodaBeanUtils.ToString(variant_Renamed)).Append(',').Append(' ');
		  buf.Append("version").Append('=').Append(JodaBeanUtils.ToString(version_Renamed)).Append(',').Append(' ');
		  buf.Append("putCall").Append('=').Append(JodaBeanUtils.ToString(putCall_Renamed)).Append(',').Append(' ');
		  buf.Append("strikePrice").Append('=').Append(JodaBeanUtils.ToString(strikePrice_Renamed)).Append(',').Append(' ');
		  buf.Append("underlyingExpiryMonth").Append('=').Append(JodaBeanUtils.ToString(underlyingExpiryMonth_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}