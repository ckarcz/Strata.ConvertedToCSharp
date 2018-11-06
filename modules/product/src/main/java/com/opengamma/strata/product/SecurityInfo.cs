using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
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
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// Information about a security.
	/// <para>
	/// This provides common information about a security.
	/// This includes the identifier, information about the price and an extensible data map.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private", constructorScope = "package") public final class SecurityInfo implements Attributes, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class SecurityInfo : Attributes, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final SecurityId id;
		private readonly SecurityId id;
	  /// <summary>
	  /// The information about the security price.
	  /// <para>
	  /// This provides information about the security price.
	  /// This can be used to convert the price into a monetary value.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final SecurityPriceInfo priceInfo;
	  private readonly SecurityPriceInfo priceInfo;
	  /// <summary>
	  /// The security attributes.
	  /// <para>
	  /// Security attributes provide the ability to associate arbitrary information in a key-value map.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<AttributeType<?>, Object> attributes;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  private readonly ImmutableMap<AttributeType<object>, object> attributes;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the identifier, tick size and tick value.
	  /// <para>
	  /// This creates an instance, building the <seealso cref="SecurityPriceInfo"/> from
	  /// the tick size and tick value, setting the contract size to 1.
	  /// </para>
	  /// <para>
	  /// A {@code SecurityInfo} also contains a hash map of additional information,
	  /// keyed by <seealso cref="AttributeType"/>. This hash map may contain anything
	  /// of interest, and is populated using <seealso cref="#withAttribute(AttributeType, Object)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="id">  the security identifier </param>
	  /// <param name="tickSize">  the size of each tick, not negative or zero </param>
	  /// <param name="tickValue">  the value of each tick </param>
	  /// <returns> the security information </returns>
	  public static SecurityInfo of(SecurityId id, double tickSize, CurrencyAmount tickValue)
	  {
		return new SecurityInfo(id, SecurityPriceInfo.of(tickSize, tickValue), ImmutableMap.of());
	  }

	  /// <summary>
	  /// Obtains an instance from the identifier and pricing info.
	  /// <para>
	  /// A {@code SecurityInfo} also contains a hash map of additional information,
	  /// keyed by <seealso cref="AttributeType"/>. This hash map may contain anything
	  /// of interest, and is populated using <seealso cref="#withAttribute(AttributeType, Object)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="id">  the security identifier </param>
	  /// <param name="priceInfo">  the information about the price </param>
	  /// <returns> the security information </returns>
	  public static SecurityInfo of(SecurityId id, SecurityPriceInfo priceInfo)
	  {
		return new SecurityInfo(id, priceInfo, ImmutableMap.of());
	  }

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean.
	  /// </summary>
	  /// <returns> the builder, not null </returns>
	  public static SecurityInfoBuilder builder()
	  {
		return new SecurityInfoBuilder();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the attribute associated with the specified type.
	  /// <para>
	  /// This method obtains the specified attribute.
	  /// This allows an attribute about a security to be obtained if available.
	  /// </para>
	  /// <para>
	  /// If the attribute is not found, an exception is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the result </param>
	  /// <param name="type">  the type to find </param>
	  /// <returns> the attribute value </returns>
	  /// <exception cref="IllegalArgumentException"> if the attribute is not found </exception>
	  public override T getAttribute<T>(AttributeType<T> type)
	  {
		return findAttribute(type).orElseThrow(() => new System.ArgumentException(Messages.format("Attribute not found for type '{}'", type)));
	  }

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
//ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <T> java.util.Optional<T> findAttribute(AttributeType<T> type)
	  public Optional<T> findAttribute<T>(AttributeType<T> type)
	  {
		return Optional.ofNullable((T) attributes.get(type));
	  }

	  /// <summary>
	  /// Returns a copy of this instance with attribute added.
	  /// <para>
	  /// This returns a new instance with the specified attribute added.
	  /// The attribute is added using {@code Map.put(type, value)} semantics.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T> the type of the value </param>
	  /// <param name="type">  the type providing meaning to the value </param>
	  /// <param name="value">  the value </param>
	  /// <returns> a new instance based on this one with the attribute added </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <T> SecurityInfo withAttribute(AttributeType<T> type, T value)
	  public SecurityInfo withAttribute<T>(AttributeType<T> type, T value)
	  {
		// ImmutableMap.Builder would not provide Map.put semantics
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<AttributeType<?>, Object> updatedAttributes = new java.util.HashMap<>(attributes);
		IDictionary<AttributeType<object>, object> updatedAttributes = new Dictionary<AttributeType<object>, object>(attributes);
		updatedAttributes[type] = value;
		return new SecurityInfo(id, priceInfo, updatedAttributes);
	  }

	  /// <summary>
	  /// Returns a builder populated with the values of this instance.
	  /// </summary>
	  /// <returns> a builder populated with the values of this instance </returns>
	  public SecurityInfoBuilder toBuilder()
	  {
		return new SecurityInfoBuilder(id, priceInfo, attributes);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code SecurityInfo}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static SecurityInfo.Meta meta()
	  {
		return SecurityInfo.Meta.INSTANCE;
	  }

	  static SecurityInfo()
	  {
		MetaBean.register(SecurityInfo.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="id">  the value of the property, not null </param>
	  /// <param name="priceInfo">  the value of the property, not null </param>
	  /// <param name="attributes">  the value of the property, not null </param>
	  internal SecurityInfo<T1>(SecurityId id, SecurityPriceInfo priceInfo, IDictionary<T1> attributes)
	  {
		JodaBeanUtils.notNull(id, "id");
		JodaBeanUtils.notNull(priceInfo, "priceInfo");
		JodaBeanUtils.notNull(attributes, "attributes");
		this.id = id;
		this.priceInfo = priceInfo;
		this.attributes = ImmutableMap.copyOf(attributes);
	  }

	  public override SecurityInfo.Meta metaBean()
	  {
		return SecurityInfo.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the security identifier.
	  /// <para>
	  /// This identifier uniquely identifies the security within the system.
	  /// It is the key used to lookup the security in <seealso cref="ReferenceData"/>.
	  /// </para>
	  /// <para>
	  /// A real-world security will typically have multiple identifiers.
	  /// The only restriction placed on the identifier is that it is sufficiently
	  /// unique for the reference data lookup. As such, it is acceptable to use
	  /// an identifier from a well-known global or vendor symbology.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SecurityId Id
	  {
		  get
		  {
			return id;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the information about the security price.
	  /// <para>
	  /// This provides information about the security price.
	  /// This can be used to convert the price into a monetary value.
	  /// </para>
	  /// </summary>
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
	  /// Gets the security attributes.
	  /// <para>
	  /// Security attributes provide the ability to associate arbitrary information in a key-value map.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.google.common.collect.ImmutableMap<AttributeType<?>, Object> getAttributes()
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
		  SecurityInfo other = (SecurityInfo) obj;
		  return JodaBeanUtils.equal(id, other.id) && JodaBeanUtils.equal(priceInfo, other.priceInfo) && JodaBeanUtils.equal(attributes, other.attributes);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(id);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(priceInfo);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(attributes);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("SecurityInfo{");
		buf.Append("id").Append('=').Append(id).Append(',').Append(' ');
		buf.Append("priceInfo").Append('=').Append(priceInfo).Append(',').Append(' ');
		buf.Append("attributes").Append('=').Append(JodaBeanUtils.ToString(attributes));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code SecurityInfo}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  id_Renamed = DirectMetaProperty.ofImmutable(this, "id", typeof(SecurityInfo), typeof(SecurityId));
			  priceInfo_Renamed = DirectMetaProperty.ofImmutable(this, "priceInfo", typeof(SecurityInfo), typeof(SecurityPriceInfo));
			  attributes_Renamed = DirectMetaProperty.ofImmutable(this, "attributes", typeof(SecurityInfo), (Type) typeof(ImmutableMap));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "id", "priceInfo", "attributes");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code id} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SecurityId> id_Renamed;
		/// <summary>
		/// The meta-property for the {@code priceInfo} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SecurityPriceInfo> priceInfo_Renamed;
		/// <summary>
		/// The meta-property for the {@code attributes} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<AttributeType<?>, Object>> attributes = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "attributes", SecurityInfo.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		internal MetaProperty<ImmutableMap<AttributeType<object>, object>> attributes_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "id", "priceInfo", "attributes");
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
			case -2126070377: // priceInfo
			  return priceInfo_Renamed;
			case 405645655: // attributes
			  return attributes_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends SecurityInfo> builder()
		public override BeanBuilder<SecurityInfo> builder()
		{
		  return new SecurityInfo.Builder();
		}

		public override Type beanType()
		{
		  return typeof(SecurityInfo);
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
		public MetaProperty<SecurityId> id()
		{
		  return id_Renamed;
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
//ORIGINAL LINE: public org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<AttributeType<?>, Object>> attributes()
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
			  return ((SecurityInfo) bean).Id;
			case -2126070377: // priceInfo
			  return ((SecurityInfo) bean).PriceInfo;
			case 405645655: // attributes
			  return ((SecurityInfo) bean).Attributes;
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
	  /// The bean-builder for {@code SecurityInfo}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<SecurityInfo>
	  {

		internal SecurityId id;
		internal SecurityPriceInfo priceInfo;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.Map<AttributeType<?>, Object> attributes = com.google.common.collect.ImmutableMap.of();
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
			  this.id = (SecurityId) newValue;
			  break;
			case -2126070377: // priceInfo
			  this.priceInfo = (SecurityPriceInfo) newValue;
			  break;
			case 405645655: // attributes
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.attributes = (java.util.Map<AttributeType<?>, Object>) newValue;
			  this.attributes = (IDictionary<AttributeType<object>, object>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override SecurityInfo build()
		{
		  return new SecurityInfo(id, priceInfo, attributes);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("SecurityInfo.Builder{");
		  buf.Append("id").Append('=').Append(JodaBeanUtils.ToString(id)).Append(',').Append(' ');
		  buf.Append("priceInfo").Append('=').Append(JodaBeanUtils.ToString(priceInfo)).Append(',').Append(' ');
		  buf.Append("attributes").Append('=').Append(JodaBeanUtils.ToString(attributes));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}