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
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ExchangeId = com.opengamma.strata.product.common.ExchangeId;
	using PutCall = com.opengamma.strata.product.common.PutCall;

	/// <summary>
	/// The contract specification defining an Exchange Traded Derivative (ETD) product.
	/// <para>
	/// This can represent a future or an option. Instances of <seealso cref="EtdOptionSecurity"/> or <seealso cref="EtdFutureSecurity"/>
	/// can be created using the {@code createFuture} and {@code createOption} methods and providing the information
	/// required to fully define the contract such as the expiry, strike price and put / call.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private", constructorScope = "package") public final class EtdContractSpec implements com.opengamma.strata.product.Attributes, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class EtdContractSpec : Attributes, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final EtdContractSpecId id;
		private readonly EtdContractSpecId id;
	  /// <summary>
	  /// The type of the contract - future or option.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final EtdType type;
	  private readonly EtdType type;
	  /// <summary>
	  /// The ID of the exchange where the instruments derived from the product are traded.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.common.ExchangeId exchangeId;
	  private readonly ExchangeId exchangeId;
	  /// <summary>
	  /// The code supplied by the exchange for use in clearing and margining, such as in SPAN.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final EtdContractCode contractCode;
	  private readonly EtdContractCode contractCode;
	  /// <summary>
	  /// The human readable description of the product.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty") private final String description;
	  private readonly string description;
	  /// <summary>
	  /// The information about the security price.
	  /// This includes details of the currency, tick size, tick value, contract size.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.SecurityPriceInfo priceInfo;
	  private readonly SecurityPriceInfo priceInfo;
	  /// <summary>
	  /// The attributes.
	  /// <para>
	  /// Attributes provide the ability to associate arbitrary information
	  /// with a security contract specification in a key-value map.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.product.AttributeType<?>, Object> attributes;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  private readonly ImmutableMap<AttributeType<object>, object> attributes;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a builder for building instances of {@code EtdContractSpec}.
	  /// <para>
	  /// The builder will create an identifier using <seealso cref="EtdIdUtils"/> if it is not set.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a builder for building instances of {@code EtdContractSpec} </returns>
	  public static EtdContractSpecBuilder builder()
	  {
		return new EtdContractSpecBuilder();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the attribute associated with the specified type.
	  /// <para>
	  /// This method obtains the specified attribute.
	  /// This allows an attribute about a security to be obtained if available.
	  /// </para>
	  /// <para>
	  /// If the attribute is not found, optional empty is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the result </param>
	  /// <param name="type">  the type to find </param>
	  /// <returns> the attribute value </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <T> java.util.Optional<T> findAttribute(com.opengamma.strata.product.AttributeType<T> type)
	  public Optional<T> findAttribute<T>(AttributeType<T> type)
	  {
		return Optional.ofNullable((T) attributes.get(type));
	  }

	  public EtdContractSpec withAttribute<T>(AttributeType<T> type, T value)
	  {
		// ImmutableMap.Builder would not provide Map.put semantics
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.product.AttributeType<?>, Object> updatedAttributes = new java.util.HashMap<>(attributes);
		IDictionary<AttributeType<object>, object> updatedAttributes = new Dictionary<AttributeType<object>, object>(attributes);
		updatedAttributes[type] = value;
		return new EtdContractSpec(id, this.type, exchangeId, contractCode, description, priceInfo, updatedAttributes);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a future security based on this contract specification.
	  /// <para>
	  /// The security identifier will be automatically created using <seealso cref="EtdIdUtils"/>.
	  /// The <seealso cref="#getType() type"/> must be <seealso cref="EtdType#FUTURE"/> otherwise an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiryMonth">  the expiry month of the future </param>
	  /// <param name="variant">  the variant of the ETD, such as 'Monthly', 'Weekly, 'Daily' or 'Flex' </param>
	  /// <returns> a future security based on this contract specification </returns>
	  /// <exception cref="IllegalStateException"> if the product type of the contract specification is not {@code FUTURE} </exception>
	  public EtdFutureSecurity createFuture(YearMonth expiryMonth, EtdVariant variant)
	  {
		return EtdFutureSecurity.of(this, expiryMonth, variant);
	  }

	  /// <summary>
	  /// Creates an option security based on this contract specification.
	  /// <para>
	  /// The security identifier will be automatically created using <seealso cref="EtdIdUtils"/>.
	  /// The <seealso cref="#getType() type"/> must be <seealso cref="EtdType#OPTION"/> otherwise an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiryMonth">  the expiry month of the option </param>
	  /// <param name="variant">  the variant of the ETD, such as 'Monthly', 'Weekly, 'Daily' or 'Flex' </param>
	  /// <param name="version">  the non-negative version, zero by default </param>
	  /// <param name="putCall">  whether the option is a put or call </param>
	  /// <param name="strikePrice">  the strike price of the option </param>
	  /// <returns> an option security based on this contract specification </returns>
	  /// <exception cref="IllegalStateException"> if the product type of the contract specification is not {@code OPTION} </exception>
	  public EtdOptionSecurity createOption(YearMonth expiryMonth, EtdVariant variant, int version, PutCall putCall, double strikePrice)
	  {

		return EtdOptionSecurity.of(this, expiryMonth, variant, version, putCall, strikePrice);
	  }

	  /// <summary>
	  /// Creates an option security based on this contract specification.
	  /// <para>
	  /// The security identifier will be automatically created using <seealso cref="EtdIdUtils"/>.
	  /// The <seealso cref="#getType() type"/> must be <seealso cref="EtdType#OPTION"/> otherwise an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiryMonth">  the expiry month of the option </param>
	  /// <param name="variant">  the variant of the ETD, such as 'Monthly', 'Weekly, 'Daily' or 'Flex' </param>
	  /// <param name="version">  the non-negative version, zero by default </param>
	  /// <param name="putCall">  whether the option is a put or call </param>
	  /// <param name="strikePrice">  the strike price of the option </param>
	  /// <param name="underlyingExpiryMonth">  the expiry of the underlying instrument, such as a future </param>
	  /// <returns> an option security based on this contract specification </returns>
	  /// <exception cref="IllegalStateException"> if the product type of the contract specification is not {@code OPTION} </exception>
	  public EtdOptionSecurity createOption(YearMonth expiryMonth, EtdVariant variant, int version, PutCall putCall, double strikePrice, YearMonth underlyingExpiryMonth)
	  {

		return EtdOptionSecurity.of(this, expiryMonth, variant, version, putCall, strikePrice, underlyingExpiryMonth);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code EtdContractSpec}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static EtdContractSpec.Meta meta()
	  {
		return EtdContractSpec.Meta.INSTANCE;
	  }

	  static EtdContractSpec()
	  {
		MetaBean.register(EtdContractSpec.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="id">  the value of the property, not null </param>
	  /// <param name="type">  the value of the property, not null </param>
	  /// <param name="exchangeId">  the value of the property, not null </param>
	  /// <param name="contractCode">  the value of the property, not null </param>
	  /// <param name="description">  the value of the property, not empty </param>
	  /// <param name="priceInfo">  the value of the property, not null </param>
	  /// <param name="attributes">  the value of the property, not null </param>
	  internal EtdContractSpec<T1>(EtdContractSpecId id, EtdType type, ExchangeId exchangeId, EtdContractCode contractCode, string description, SecurityPriceInfo priceInfo, IDictionary<T1> attributes)
	  {
		JodaBeanUtils.notNull(id, "id");
		JodaBeanUtils.notNull(type, "type");
		JodaBeanUtils.notNull(exchangeId, "exchangeId");
		JodaBeanUtils.notNull(contractCode, "contractCode");
		JodaBeanUtils.notEmpty(description, "description");
		JodaBeanUtils.notNull(priceInfo, "priceInfo");
		JodaBeanUtils.notNull(attributes, "attributes");
		this.id = id;
		this.type = type;
		this.exchangeId = exchangeId;
		this.contractCode = contractCode;
		this.description = description;
		this.priceInfo = priceInfo;
		this.attributes = ImmutableMap.copyOf(attributes);
	  }

	  public override EtdContractSpec.Meta metaBean()
	  {
		return EtdContractSpec.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the ID of this contract specification.
	  /// <para>
	  /// When building, this will be defaulted using <seealso cref="EtdIdUtils"/>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public EtdContractSpecId Id
	  {
		  get
		  {
			return id;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the type of the contract - future or option. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public EtdType Type
	  {
		  get
		  {
			return type;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the ID of the exchange where the instruments derived from the product are traded. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ExchangeId ExchangeId
	  {
		  get
		  {
			return exchangeId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the code supplied by the exchange for use in clearing and margining, such as in SPAN. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public EtdContractCode ContractCode
	  {
		  get
		  {
			return contractCode;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the human readable description of the product. </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public string Description
	  {
		  get
		  {
			return description;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the information about the security price.
	  /// This includes details of the currency, tick size, tick value, contract size. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SecurityPriceInfo PriceInfo
	  {
		  get
		  {
			return priceInfo;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the attributes.
	  /// <para>
	  /// Attributes provide the ability to associate arbitrary information
	  /// with a security contract specification in a key-value map.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.google.common.collect.ImmutableMap<com.opengamma.strata.product.AttributeType<?>, Object> getAttributes()
	  public ImmutableMap<AttributeType<object>, object> Attributes
	  {
		  get
		  {
			return attributes;
		  }
	  }

	  //-----------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  EtdContractSpec other = (EtdContractSpec) obj;
		  return JodaBeanUtils.equal(id, other.id) && JodaBeanUtils.equal(type, other.type) && JodaBeanUtils.equal(exchangeId, other.exchangeId) && JodaBeanUtils.equal(contractCode, other.contractCode) && JodaBeanUtils.equal(description, other.description) && JodaBeanUtils.equal(priceInfo, other.priceInfo) && JodaBeanUtils.equal(attributes, other.attributes);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(id);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(type);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(exchangeId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(contractCode);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(description);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(priceInfo);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(attributes);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(256);
		buf.Append("EtdContractSpec{");
		buf.Append("id").Append('=').Append(id).Append(',').Append(' ');
		buf.Append("type").Append('=').Append(type).Append(',').Append(' ');
		buf.Append("exchangeId").Append('=').Append(exchangeId).Append(',').Append(' ');
		buf.Append("contractCode").Append('=').Append(contractCode).Append(',').Append(' ');
		buf.Append("description").Append('=').Append(description).Append(',').Append(' ');
		buf.Append("priceInfo").Append('=').Append(priceInfo).Append(',').Append(' ');
		buf.Append("attributes").Append('=').Append(JodaBeanUtils.ToString(attributes));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code EtdContractSpec}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  id_Renamed = DirectMetaProperty.ofImmutable(this, "id", typeof(EtdContractSpec), typeof(EtdContractSpecId));
			  type_Renamed = DirectMetaProperty.ofImmutable(this, "type", typeof(EtdContractSpec), typeof(EtdType));
			  exchangeId_Renamed = DirectMetaProperty.ofImmutable(this, "exchangeId", typeof(EtdContractSpec), typeof(ExchangeId));
			  contractCode_Renamed = DirectMetaProperty.ofImmutable(this, "contractCode", typeof(EtdContractSpec), typeof(EtdContractCode));
			  description_Renamed = DirectMetaProperty.ofImmutable(this, "description", typeof(EtdContractSpec), typeof(string));
			  priceInfo_Renamed = DirectMetaProperty.ofImmutable(this, "priceInfo", typeof(EtdContractSpec), typeof(SecurityPriceInfo));
			  attributes_Renamed = DirectMetaProperty.ofImmutable(this, "attributes", typeof(EtdContractSpec), (Type) typeof(ImmutableMap));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "id", "type", "exchangeId", "contractCode", "description", "priceInfo", "attributes");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code id} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<EtdContractSpecId> id_Renamed;
		/// <summary>
		/// The meta-property for the {@code type} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<EtdType> type_Renamed;
		/// <summary>
		/// The meta-property for the {@code exchangeId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ExchangeId> exchangeId_Renamed;
		/// <summary>
		/// The meta-property for the {@code contractCode} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<EtdContractCode> contractCode_Renamed;
		/// <summary>
		/// The meta-property for the {@code description} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<string> description_Renamed;
		/// <summary>
		/// The meta-property for the {@code priceInfo} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SecurityPriceInfo> priceInfo_Renamed;
		/// <summary>
		/// The meta-property for the {@code attributes} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.product.AttributeType<?>, Object>> attributes = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "attributes", EtdContractSpec.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		internal MetaProperty<ImmutableMap<AttributeType<object>, object>> attributes_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "id", "type", "exchangeId", "contractCode", "description", "priceInfo", "attributes");
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
			case 3355: // id
			  return id_Renamed;
			case 3575610: // type
			  return type_Renamed;
			case 913218206: // exchangeId
			  return exchangeId_Renamed;
			case -1402840545: // contractCode
			  return contractCode_Renamed;
			case -1724546052: // description
			  return description_Renamed;
			case -2126070377: // priceInfo
			  return priceInfo_Renamed;
			case 405645655: // attributes
			  return attributes_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends EtdContractSpec> builder()
		public override BeanBuilder<EtdContractSpec> builder()
		{
		  return new EtdContractSpec.Builder();
		}

		public override Type beanType()
		{
		  return typeof(EtdContractSpec);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code id} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<EtdContractSpecId> id()
		{
		  return id_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code type} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<EtdType> type()
		{
		  return type_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code exchangeId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ExchangeId> exchangeId()
		{
		  return exchangeId_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code contractCode} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<EtdContractCode> contractCode()
		{
		  return contractCode_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code description} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<string> description()
		{
		  return description_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code priceInfo} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SecurityPriceInfo> priceInfo()
		{
		  return priceInfo_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code attributes} property. </summary>
		/// <returns> the meta-property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.product.AttributeType<?>, Object>> attributes()
		public MetaProperty<ImmutableMap<AttributeType<object>, object>> attributes()
		{
		  return attributes_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3355: // id
			  return ((EtdContractSpec) bean).Id;
			case 3575610: // type
			  return ((EtdContractSpec) bean).Type;
			case 913218206: // exchangeId
			  return ((EtdContractSpec) bean).ExchangeId;
			case -1402840545: // contractCode
			  return ((EtdContractSpec) bean).ContractCode;
			case -1724546052: // description
			  return ((EtdContractSpec) bean).Description;
			case -2126070377: // priceInfo
			  return ((EtdContractSpec) bean).PriceInfo;
			case 405645655: // attributes
			  return ((EtdContractSpec) bean).Attributes;
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
	  /// The bean-builder for {@code EtdContractSpec}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<EtdContractSpec>
	  {

		internal EtdContractSpecId id;
		internal EtdType type;
		internal ExchangeId exchangeId;
		internal EtdContractCode contractCode;
		internal string description;
		internal SecurityPriceInfo priceInfo;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.Map<com.opengamma.strata.product.AttributeType<?>, Object> attributes = com.google.common.collect.ImmutableMap.of();
		internal IDictionary<AttributeType<object>, object> attributes = ImmutableMap.of();

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3355: // id
			  return id;
			case 3575610: // type
			  return type;
			case 913218206: // exchangeId
			  return exchangeId;
			case -1402840545: // contractCode
			  return contractCode;
			case -1724546052: // description
			  return description;
			case -2126070377: // priceInfo
			  return priceInfo;
			case 405645655: // attributes
			  return attributes;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Builder set(String propertyName, Object newValue)
		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3355: // id
			  this.id = (EtdContractSpecId) newValue;
			  break;
			case 3575610: // type
			  this.type = (EtdType) newValue;
			  break;
			case 913218206: // exchangeId
			  this.exchangeId = (ExchangeId) newValue;
			  break;
			case -1402840545: // contractCode
			  this.contractCode = (EtdContractCode) newValue;
			  break;
			case -1724546052: // description
			  this.description = (string) newValue;
			  break;
			case -2126070377: // priceInfo
			  this.priceInfo = (SecurityPriceInfo) newValue;
			  break;
			case 405645655: // attributes
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.attributes = (java.util.Map<com.opengamma.strata.product.AttributeType<?>, Object>) newValue;
			  this.attributes = (IDictionary<AttributeType<object>, object>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override EtdContractSpec build()
		{
		  return new EtdContractSpec(id, type, exchangeId, contractCode, description, priceInfo, attributes);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(256);
		  buf.Append("EtdContractSpec.Builder{");
		  buf.Append("id").Append('=').Append(JodaBeanUtils.ToString(id)).Append(',').Append(' ');
		  buf.Append("type").Append('=').Append(JodaBeanUtils.ToString(type)).Append(',').Append(' ');
		  buf.Append("exchangeId").Append('=').Append(JodaBeanUtils.ToString(exchangeId)).Append(',').Append(' ');
		  buf.Append("contractCode").Append('=').Append(JodaBeanUtils.ToString(contractCode)).Append(',').Append(' ');
		  buf.Append("description").Append('=').Append(JodaBeanUtils.ToString(description)).Append(',').Append(' ');
		  buf.Append("priceInfo").Append('=').Append(JodaBeanUtils.ToString(priceInfo)).Append(',').Append(' ');
		  buf.Append("attributes").Append('=').Append(JodaBeanUtils.ToString(attributes));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}