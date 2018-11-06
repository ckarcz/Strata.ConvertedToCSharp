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
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// An instrument representing an exchange traded derivative (ETD) future.
	/// <para>
	/// A security representing a standardized contact between two parties to buy or sell an asset at a
	/// future date for an agreed price.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class EtdFutureSecurity implements EtdSecurity, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class EtdFutureSecurity : EtdSecurity, ImmutableBean
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

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a contract specification, expiry year-month and variant.
	  /// <para>
	  /// The security identifier will be automatically created using <seealso cref="EtdIdUtils"/>.
	  /// The specification must be for a future.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="spec">  the future contract specification </param>
	  /// <param name="expiry">  the expiry year-month of the future </param>
	  /// <param name="variant">  the variant of the ETD, such as 'Monthly', 'Weekly, 'Daily' or 'Flex' </param>
	  /// <returns> a future security based on this contract specification </returns>
	  /// <exception cref="IllegalStateException"> if the product type of the contract specification is not {@code FUTURE} </exception>
	  public static EtdFutureSecurity of(EtdContractSpec spec, YearMonth expiry, EtdVariant variant)
	  {
		if (spec.Type != EtdType.FUTURE)
		{
		  throw new System.InvalidOperationException(Messages.format("Cannot create an EtdFutureSecurity from a contract specification of type '{}'", spec.Type));
		}
		SecurityId securityId = EtdIdUtils.futureId(spec.ExchangeId, spec.ContractCode, expiry, variant);
		return EtdFutureSecurity.builder().info(SecurityInfo.of(securityId, spec.PriceInfo)).contractSpecId(spec.Id).expiry(expiry).variant(variant).build();
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
			return EtdType.FUTURE;
		  }
	  }

	  public EtdFutureSecurity withInfo(SecurityInfo info)
	  {
		return toBuilder().info(info).build();
	  }

	  public EtdFutureSecurity createProduct(ReferenceData refData)
	  {
		return this;
	  }

	  public EtdFutureTrade createTrade(TradeInfo tradeInfo, double quantity, double tradePrice, ReferenceData refData)
	  {
		return EtdFutureTrade.builder().info(tradeInfo).quantity(quantity).price(tradePrice).security(this).build();
	  }

	  public EtdFuturePosition createPosition(PositionInfo positionInfo, double quantity, ReferenceData refData)
	  {
		return EtdFuturePosition.ofNet(positionInfo, this, quantity);
	  }

	  public EtdFuturePosition createPosition(PositionInfo positionInfo, double longQuantity, double shortQuantity, ReferenceData refData)
	  {

		return EtdFuturePosition.ofLongShort(positionInfo, this, longQuantity, shortQuantity);
	  }

	  /// <summary>
	  /// Summarizes this ETD future into string form.
	  /// </summary>
	  /// <returns> the summary description </returns>
	  public string summaryDescription()
	  {
		return variant.Code + expiry.format(YM_FORMAT);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code EtdFutureSecurity}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static EtdFutureSecurity.Meta meta()
	  {
		return EtdFutureSecurity.Meta.INSTANCE;
	  }

	  static EtdFutureSecurity()
	  {
		MetaBean.register(EtdFutureSecurity.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static EtdFutureSecurity.Builder builder()
	  {
		return new EtdFutureSecurity.Builder();
	  }

	  private EtdFutureSecurity(SecurityInfo info, EtdContractSpecId contractSpecId, YearMonth expiry, EtdVariant variant)
	  {
		JodaBeanUtils.notNull(info, "info");
		JodaBeanUtils.notNull(contractSpecId, "contractSpecId");
		JodaBeanUtils.notNull(expiry, "expiry");
		JodaBeanUtils.notNull(variant, "variant");
		this.info = info;
		this.contractSpecId = contractSpecId;
		this.expiry = expiry;
		this.variant = variant;
	  }

	  public override EtdFutureSecurity.Meta metaBean()
	  {
		return EtdFutureSecurity.Meta.INSTANCE;
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
		  EtdFutureSecurity other = (EtdFutureSecurity) obj;
		  return JodaBeanUtils.equal(info, other.info) && JodaBeanUtils.equal(contractSpecId, other.contractSpecId) && JodaBeanUtils.equal(expiry, other.expiry) && JodaBeanUtils.equal(variant, other.variant);
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
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("EtdFutureSecurity{");
		buf.Append("info").Append('=').Append(info).Append(',').Append(' ');
		buf.Append("contractSpecId").Append('=').Append(contractSpecId).Append(',').Append(' ');
		buf.Append("expiry").Append('=').Append(expiry).Append(',').Append(' ');
		buf.Append("variant").Append('=').Append(JodaBeanUtils.ToString(variant));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code EtdFutureSecurity}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  info_Renamed = DirectMetaProperty.ofImmutable(this, "info", typeof(EtdFutureSecurity), typeof(SecurityInfo));
			  contractSpecId_Renamed = DirectMetaProperty.ofImmutable(this, "contractSpecId", typeof(EtdFutureSecurity), typeof(EtdContractSpecId));
			  expiry_Renamed = DirectMetaProperty.ofImmutable(this, "expiry", typeof(EtdFutureSecurity), typeof(YearMonth));
			  variant_Renamed = DirectMetaProperty.ofImmutable(this, "variant", typeof(EtdFutureSecurity), typeof(EtdVariant));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "info", "contractSpecId", "expiry", "variant");
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
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "info", "contractSpecId", "expiry", "variant");
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
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override EtdFutureSecurity.Builder builder()
		{
		  return new EtdFutureSecurity.Builder();
		}

		public override Type beanType()
		{
		  return typeof(EtdFutureSecurity);
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

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  return ((EtdFutureSecurity) bean).Info;
			case 948987368: // contractSpecId
			  return ((EtdFutureSecurity) bean).ContractSpecId;
			case -1289159373: // expiry
			  return ((EtdFutureSecurity) bean).Expiry;
			case 236785797: // variant
			  return ((EtdFutureSecurity) bean).Variant;
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
	  /// The bean-builder for {@code EtdFutureSecurity}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<EtdFutureSecurity>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SecurityInfo info_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal EtdContractSpecId contractSpecId_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal YearMonth expiry_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal EtdVariant variant_Renamed;

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
		internal Builder(EtdFutureSecurity beanToCopy)
		{
		  this.info_Renamed = beanToCopy.Info;
		  this.contractSpecId_Renamed = beanToCopy.ContractSpecId;
		  this.expiry_Renamed = beanToCopy.Expiry;
		  this.variant_Renamed = beanToCopy.Variant;
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

		public override EtdFutureSecurity build()
		{
		  return new EtdFutureSecurity(info_Renamed, contractSpecId_Renamed, expiry_Renamed, variant_Renamed);
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

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("EtdFutureSecurity.Builder{");
		  buf.Append("info").Append('=').Append(JodaBeanUtils.ToString(info_Renamed)).Append(',').Append(' ');
		  buf.Append("contractSpecId").Append('=').Append(JodaBeanUtils.ToString(contractSpecId_Renamed)).Append(',').Append(' ');
		  buf.Append("expiry").Append('=').Append(JodaBeanUtils.ToString(expiry_Renamed)).Append(',').Append(' ');
		  buf.Append("variant").Append('=').Append(JodaBeanUtils.ToString(variant_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}