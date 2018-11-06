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
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// Additional information about a position.
	/// <para>
	/// This allows additional information about a position to be associated.
	/// It is kept in a separate object as the information is optional for pricing.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private", constructorScope = "package") public final class PositionInfo implements PortfolioItemInfo, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class PositionInfo : PortfolioItemInfo, ImmutableBean
	{

	  /// <summary>
	  /// An empty instance of {@code PositionInfo}.
	  /// </summary>
	  private static readonly PositionInfo EMPTY = new PositionInfo(null, ImmutableMap.of());

	  /// <summary>
	  /// The primary identifier for the position, optional.
	  /// <para>
	  /// The identifier is used to identify the position.
	  /// It will typically be an identifier in an external data system.
	  /// </para>
	  /// <para>
	  /// A position may have multiple active identifiers. Any identifier may be chosen here.
	  /// Certain uses of the identifier, such as storage in a database, require that the
	  /// identifier does not change over time, and this should be considered best practice.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional", overrideGet = true) private final com.opengamma.strata.basics.StandardId id;
	  private readonly StandardId id;
	  /// <summary>
	  /// The position attributes.
	  /// <para>
	  /// Position attributes provide the ability to associate arbitrary information in a key-value map.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<AttributeType<?>, Object> attributes;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  private readonly ImmutableMap<AttributeType<object>, object> attributes;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an empty instance, with no identifier or attributes.
	  /// </summary>
	  /// <returns> the empty instance </returns>
	  public static PositionInfo empty()
	  {
		return EMPTY;
	  }

	  /// <summary>
	  /// Obtains an instance with the specified position identifier.
	  /// </summary>
	  /// <param name="positionId">  the position identifier </param>
	  /// <returns> the position information </returns>
	  public static PositionInfo of(StandardId positionId)
	  {
		return new PositionInfo(positionId, ImmutableMap.of());
	  }

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean.
	  /// </summary>
	  /// <returns> the builder </returns>
	  public static PositionInfoBuilder builder()
	  {
		return new PositionInfoBuilder();
	  }

	  //-------------------------------------------------------------------------
	  public PositionInfo withId(StandardId identifier)
	  {
		return new PositionInfo(identifier, attributes);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public com.google.common.collect.ImmutableSet<AttributeType<?>> getAttributeTypes()
	  public ImmutableSet<AttributeType<object>> AttributeTypes
	  {
		  get
		  {
			return attributes.Keys;
		  }
	  }

	  public override T getAttribute<T>(AttributeType<T> type)
	  {
		return findAttribute(type).orElseThrow(() => new System.ArgumentException(Messages.format("Attribute not found for type '{}'", type)));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <T> java.util.Optional<T> findAttribute(AttributeType<T> type)
	  public Optional<T> findAttribute<T>(AttributeType<T> type)
	  {
		return Optional.ofNullable((T) attributes.get(type));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <T> PositionInfo withAttribute(AttributeType<T> type, T value)
	  public PositionInfo withAttribute<T>(AttributeType<T> type, T value)
	  {
		// ImmutableMap.Builder would not provide Map.put semantics
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<AttributeType<?>, Object> updatedAttributes = new java.util.HashMap<>(attributes);
		IDictionary<AttributeType<object>, object> updatedAttributes = new Dictionary<AttributeType<object>, object>(attributes);
		updatedAttributes[type] = value;
		return new PositionInfo(id, updatedAttributes);
	  }

	  /// <summary>
	  /// Returns a builder populated with the values of this instance.
	  /// </summary>
	  /// <returns> a builder populated with the values of this instance </returns>
	  public PositionInfoBuilder toBuilder()
	  {
		return new PositionInfoBuilder(id, attributes);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code PositionInfo}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static PositionInfo.Meta meta()
	  {
		return PositionInfo.Meta.INSTANCE;
	  }

	  static PositionInfo()
	  {
		MetaBean.register(PositionInfo.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="id">  the value of the property </param>
	  /// <param name="attributes">  the value of the property, not null </param>
	  internal PositionInfo<T1>(StandardId id, IDictionary<T1> attributes)
	  {
		JodaBeanUtils.notNull(attributes, "attributes");
		this.id = id;
		this.attributes = ImmutableMap.copyOf(attributes);
	  }

	  public override PositionInfo.Meta metaBean()
	  {
		return PositionInfo.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the primary identifier for the position, optional.
	  /// <para>
	  /// The identifier is used to identify the position.
	  /// It will typically be an identifier in an external data system.
	  /// </para>
	  /// <para>
	  /// A position may have multiple active identifiers. Any identifier may be chosen here.
	  /// Certain uses of the identifier, such as storage in a database, require that the
	  /// identifier does not change over time, and this should be considered best practice.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<StandardId> Id
	  {
		  get
		  {
			return Optional.ofNullable(id);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the position attributes.
	  /// <para>
	  /// Position attributes provide the ability to associate arbitrary information in a key-value map.
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
		  PositionInfo other = (PositionInfo) obj;
		  return JodaBeanUtils.equal(id, other.id) && JodaBeanUtils.equal(attributes, other.attributes);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(id);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(attributes);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("PositionInfo{");
		buf.Append("id").Append('=').Append(id).Append(',').Append(' ');
		buf.Append("attributes").Append('=').Append(JodaBeanUtils.ToString(attributes));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code PositionInfo}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  id_Renamed = DirectMetaProperty.ofImmutable(this, "id", typeof(PositionInfo), typeof(StandardId));
			  attributes_Renamed = DirectMetaProperty.ofImmutable(this, "attributes", typeof(PositionInfo), (Type) typeof(ImmutableMap));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "id", "attributes");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code id} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<StandardId> id_Renamed;
		/// <summary>
		/// The meta-property for the {@code attributes} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<AttributeType<?>, Object>> attributes = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "attributes", PositionInfo.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		internal MetaProperty<ImmutableMap<AttributeType<object>, object>> attributes_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "id", "attributes");
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
			case 405645655: // attributes
			  return attributes_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends PositionInfo> builder()
		public override BeanBuilder<PositionInfo> builder()
		{
		  return new PositionInfo.Builder();
		}

		public override Type beanType()
		{
		  return typeof(PositionInfo);
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
		public MetaProperty<StandardId> id()
		{
		  return id_Renamed;
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
			  return ((PositionInfo) bean).id;
			case 405645655: // attributes
			  return ((PositionInfo) bean).Attributes;
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
	  /// The bean-builder for {@code PositionInfo}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<PositionInfo>
	  {

		internal StandardId id;
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
			  this.id = (StandardId) newValue;
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

		public override PositionInfo build()
		{
		  return new PositionInfo(id, attributes);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("PositionInfo.Builder{");
		  buf.Append("id").Append('=').Append(JodaBeanUtils.ToString(id)).Append(',').Append(' ');
		  buf.Append("attributes").Append('=').Append(JodaBeanUtils.ToString(attributes));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}