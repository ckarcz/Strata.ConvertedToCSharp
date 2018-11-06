using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;
	using MinimalMetaBean = org.joda.beans.impl.direct.MinimalMetaBean;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// Additional information about a portfolio item instance.
	/// <para>
	/// This allows additional information about an item to be associated.
	/// It is kept in a separate object as the information is optional for pricing.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "minimal", builderScope = "private") final class ItemInfo implements PortfolioItemInfo, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	internal sealed class ItemInfo : PortfolioItemInfo, ImmutableBean
	{

	  /// <summary>
	  /// An empty instance of {@code ItemInfo}.
	  /// </summary>
	  private static readonly ItemInfo EMPTY = new ItemInfo(null, ImmutableMap.of());

	  /// <summary>
	  /// The primary identifier for the item, optional.
	  /// <para>
	  /// The identifier is used to identify the item.
	  /// It will typically be an identifier in an external data system.
	  /// </para>
	  /// <para>
	  /// An item may have multiple active identifiers. Any identifier may be chosen here.
	  /// Certain uses of the identifier, such as storage in a database, require that the
	  /// identifier does not change over time, and this should be considered best practice.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional", overrideGet = true) private final com.opengamma.strata.basics.StandardId id;
	  private readonly StandardId id;
	  /// <summary>
	  /// The item attributes.
	  /// <para>
	  /// Trade attributes provide the ability to associate arbitrary information in a key-value map.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<AttributeType<?>, Object> attributes;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  private readonly ImmutableMap<AttributeType<object>, object> attributes;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an empty instance, with no values or attributes.
	  /// </summary>
	  /// <returns> the empty instance </returns>
	  internal static ItemInfo empty()
	  {
		return EMPTY;
	  }

	  //-------------------------------------------------------------------------
	  public ItemInfo withId(StandardId identifier)
	  {
		return new ItemInfo(identifier, attributes);
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
//ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <T> ItemInfo withAttribute(AttributeType<T> type, T value)
	  public ItemInfo withAttribute<T>(AttributeType<T> type, T value)
	  {
		// ImmutableMap.Builder would not provide Map.put semantics
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<AttributeType<?>, Object> updatedAttributes = new java.util.HashMap<>(attributes);
		IDictionary<AttributeType<object>, object> updatedAttributes = new Dictionary<AttributeType<object>, object>(attributes);
		updatedAttributes[type] = value;
		return new ItemInfo(id, updatedAttributes);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ItemInfo}.
	  /// </summary>
	  private static readonly TypedMetaBean<ItemInfo> META_BEAN = MinimalMetaBean.of(typeof(ItemInfo), new string[] {"id", "attributes"}, () => new ItemInfo.Builder(), b => b.id, b => b.Attributes);

	  /// <summary>
	  /// The meta-bean for {@code ItemInfo}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<ItemInfo> meta()
	  {
		return META_BEAN;
	  }

	  static ItemInfo()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private ItemInfo<T1>(StandardId id, IDictionary<T1> attributes)
	  {
		JodaBeanUtils.notNull(attributes, "attributes");
		this.id = id;
		this.attributes = ImmutableMap.copyOf(attributes);
	  }

	  public override TypedMetaBean<ItemInfo> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the primary identifier for the item, optional.
	  /// <para>
	  /// The identifier is used to identify the item.
	  /// It will typically be an identifier in an external data system.
	  /// </para>
	  /// <para>
	  /// An item may have multiple active identifiers. Any identifier may be chosen here.
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
	  /// Gets the item attributes.
	  /// <para>
	  /// Trade attributes provide the ability to associate arbitrary information in a key-value map.
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
		  ItemInfo other = (ItemInfo) obj;
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
		buf.Append("ItemInfo{");
		buf.Append("id").Append('=').Append(id).Append(',').Append(' ');
		buf.Append("attributes").Append('=').Append(JodaBeanUtils.ToString(attributes));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The bean-builder for {@code ItemInfo}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<ItemInfo>
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

		public override ItemInfo build()
		{
		  return new ItemInfo(id, attributes);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("ItemInfo.Builder{");
		  buf.Append("id").Append('=').Append(JodaBeanUtils.ToString(id)).Append(',').Append(' ');
		  buf.Append("attributes").Append('=').Append(JodaBeanUtils.ToString(attributes));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}