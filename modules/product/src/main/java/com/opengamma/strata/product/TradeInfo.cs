using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	/// Additional information about a trade.
	/// <para>
	/// This allows additional information about a trade to be associated.
	/// It is kept in a separate object as the information is optional for pricing.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private", constructorScope = "package") public final class TradeInfo implements PortfolioItemInfo, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class TradeInfo : PortfolioItemInfo, ImmutableBean
	{

	  /// <summary>
	  /// An empty instance of {@code TradeInfo}.
	  /// </summary>
	  private static readonly TradeInfo EMPTY = TradeInfo.builder().build();

	  /// <summary>
	  /// The primary identifier for the trade, optional.
	  /// <para>
	  /// The identifier is used to identify the trade.
	  /// It will typically be an identifier in an external data system.
	  /// </para>
	  /// <para>
	  /// A trade may have multiple active identifiers. Any identifier may be chosen here.
	  /// Certain uses of the identifier, such as storage in a database, require that the
	  /// identifier does not change over time, and this should be considered best practice.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional", overrideGet = true) private final com.opengamma.strata.basics.StandardId id;
	  private readonly StandardId id;
	  /// <summary>
	  /// The counterparty identifier, optional.
	  /// <para>
	  /// An identifier used to specify the counterparty of the trade.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.basics.StandardId counterparty;
	  private readonly StandardId counterparty;
	  /// <summary>
	  /// The trade date, optional.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final java.time.LocalDate tradeDate;
	  private readonly LocalDate tradeDate;
	  /// <summary>
	  /// The trade time, optional.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final java.time.LocalTime tradeTime;
	  private readonly LocalTime tradeTime;
	  /// <summary>
	  /// The trade time-zone, optional.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final java.time.ZoneId zone;
	  private readonly ZoneId zone;
	  /// <summary>
	  /// The settlement date, optional.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final java.time.LocalDate settlementDate;
	  private readonly LocalDate settlementDate;
	  /// <summary>
	  /// The trade attributes.
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
	  public static TradeInfo empty()
	  {
		return EMPTY;
	  }

	  /// <summary>
	  /// Obtains an instance with the specified trade date.
	  /// </summary>
	  /// <param name="tradeDate">  the trade date </param>
	  /// <returns> the trade information </returns>
	  public static TradeInfo of(LocalDate tradeDate)
	  {
		return new TradeInfo(null, null, tradeDate, null, null, null, ImmutableMap.of());
	  }

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean.
	  /// </summary>
	  /// <returns> the builder, not null </returns>
	  public static TradeInfoBuilder builder()
	  {
		return new TradeInfoBuilder();
	  }

	  //-------------------------------------------------------------------------
	  public TradeInfo withId(StandardId identifier)
	  {
		return new TradeInfo(identifier, counterparty, tradeDate, tradeTime, zone, settlementDate, attributes);
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
//ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <T> TradeInfo withAttribute(AttributeType<T> type, T value)
	  public TradeInfo withAttribute<T>(AttributeType<T> type, T value)
	  {
		// ImmutableMap.Builder would not provide Map.put semantics
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<AttributeType<?>, Object> updatedAttributes = new java.util.HashMap<>(attributes);
		IDictionary<AttributeType<object>, object> updatedAttributes = new Dictionary<AttributeType<object>, object>(attributes);
		updatedAttributes[type] = value;
		return new TradeInfo(id, counterparty, tradeDate, tradeTime, zone, settlementDate, updatedAttributes);
	  }

	  /// <summary>
	  /// Returns a builder populated with the values of this instance.
	  /// </summary>
	  /// <returns> a builder populated with the values of this instance </returns>
	  public TradeInfoBuilder toBuilder()
	  {
		return new TradeInfoBuilder(id, counterparty, tradeDate, tradeTime, zone, settlementDate, attributes);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code TradeInfo}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TradeInfo.Meta meta()
	  {
		return TradeInfo.Meta.INSTANCE;
	  }

	  static TradeInfo()
	  {
		MetaBean.register(TradeInfo.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="id">  the value of the property </param>
	  /// <param name="counterparty">  the value of the property </param>
	  /// <param name="tradeDate">  the value of the property </param>
	  /// <param name="tradeTime">  the value of the property </param>
	  /// <param name="zone">  the value of the property </param>
	  /// <param name="settlementDate">  the value of the property </param>
	  /// <param name="attributes">  the value of the property, not null </param>
	  internal TradeInfo<T1>(StandardId id, StandardId counterparty, LocalDate tradeDate, LocalTime tradeTime, ZoneId zone, LocalDate settlementDate, IDictionary<T1> attributes)
	  {
		JodaBeanUtils.notNull(attributes, "attributes");
		this.id = id;
		this.counterparty = counterparty;
		this.tradeDate = tradeDate;
		this.tradeTime = tradeTime;
		this.zone = zone;
		this.settlementDate = settlementDate;
		this.attributes = ImmutableMap.copyOf(attributes);
	  }

	  public override TradeInfo.Meta metaBean()
	  {
		return TradeInfo.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the primary identifier for the trade, optional.
	  /// <para>
	  /// The identifier is used to identify the trade.
	  /// It will typically be an identifier in an external data system.
	  /// </para>
	  /// <para>
	  /// A trade may have multiple active identifiers. Any identifier may be chosen here.
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
	  /// Gets the counterparty identifier, optional.
	  /// <para>
	  /// An identifier used to specify the counterparty of the trade.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<StandardId> Counterparty
	  {
		  get
		  {
			return Optional.ofNullable(counterparty);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the trade date, optional. </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<LocalDate> TradeDate
	  {
		  get
		  {
			return Optional.ofNullable(tradeDate);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the trade time, optional. </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<LocalTime> TradeTime
	  {
		  get
		  {
			return Optional.ofNullable(tradeTime);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the trade time-zone, optional. </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<ZoneId> Zone
	  {
		  get
		  {
			return Optional.ofNullable(zone);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the settlement date, optional. </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<LocalDate> SettlementDate
	  {
		  get
		  {
			return Optional.ofNullable(settlementDate);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the trade attributes.
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
		  TradeInfo other = (TradeInfo) obj;
		  return JodaBeanUtils.equal(id, other.id) && JodaBeanUtils.equal(counterparty, other.counterparty) && JodaBeanUtils.equal(tradeDate, other.tradeDate) && JodaBeanUtils.equal(tradeTime, other.tradeTime) && JodaBeanUtils.equal(zone, other.zone) && JodaBeanUtils.equal(settlementDate, other.settlementDate) && JodaBeanUtils.equal(attributes, other.attributes);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(id);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(counterparty);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(tradeDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(tradeTime);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(zone);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(settlementDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(attributes);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(256);
		buf.Append("TradeInfo{");
		buf.Append("id").Append('=').Append(id).Append(',').Append(' ');
		buf.Append("counterparty").Append('=').Append(counterparty).Append(',').Append(' ');
		buf.Append("tradeDate").Append('=').Append(tradeDate).Append(',').Append(' ');
		buf.Append("tradeTime").Append('=').Append(tradeTime).Append(',').Append(' ');
		buf.Append("zone").Append('=').Append(zone).Append(',').Append(' ');
		buf.Append("settlementDate").Append('=').Append(settlementDate).Append(',').Append(' ');
		buf.Append("attributes").Append('=').Append(JodaBeanUtils.ToString(attributes));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code TradeInfo}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  id_Renamed = DirectMetaProperty.ofImmutable(this, "id", typeof(TradeInfo), typeof(StandardId));
			  counterparty_Renamed = DirectMetaProperty.ofImmutable(this, "counterparty", typeof(TradeInfo), typeof(StandardId));
			  tradeDate_Renamed = DirectMetaProperty.ofImmutable(this, "tradeDate", typeof(TradeInfo), typeof(LocalDate));
			  tradeTime_Renamed = DirectMetaProperty.ofImmutable(this, "tradeTime", typeof(TradeInfo), typeof(LocalTime));
			  zone_Renamed = DirectMetaProperty.ofImmutable(this, "zone", typeof(TradeInfo), typeof(ZoneId));
			  settlementDate_Renamed = DirectMetaProperty.ofImmutable(this, "settlementDate", typeof(TradeInfo), typeof(LocalDate));
			  attributes_Renamed = DirectMetaProperty.ofImmutable(this, "attributes", typeof(TradeInfo), (Type) typeof(ImmutableMap));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "id", "counterparty", "tradeDate", "tradeTime", "zone", "settlementDate", "attributes");
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
		/// The meta-property for the {@code counterparty} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<StandardId> counterparty_Renamed;
		/// <summary>
		/// The meta-property for the {@code tradeDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> tradeDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code tradeTime} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalTime> tradeTime_Renamed;
		/// <summary>
		/// The meta-property for the {@code zone} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ZoneId> zone_Renamed;
		/// <summary>
		/// The meta-property for the {@code settlementDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> settlementDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code attributes} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<AttributeType<?>, Object>> attributes = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "attributes", TradeInfo.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		internal MetaProperty<ImmutableMap<AttributeType<object>, object>> attributes_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "id", "counterparty", "tradeDate", "tradeTime", "zone", "settlementDate", "attributes");
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
			case -1651301782: // counterparty
			  return counterparty_Renamed;
			case 752419634: // tradeDate
			  return tradeDate_Renamed;
			case 752903761: // tradeTime
			  return tradeTime_Renamed;
			case 3744684: // zone
			  return zone_Renamed;
			case -295948169: // settlementDate
			  return settlementDate_Renamed;
			case 405645655: // attributes
			  return attributes_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends TradeInfo> builder()
		public override BeanBuilder<TradeInfo> builder()
		{
		  return new TradeInfo.Builder();
		}

		public override Type beanType()
		{
		  return typeof(TradeInfo);
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
		/// The meta-property for the {@code counterparty} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<StandardId> counterparty()
		{
		  return counterparty_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code tradeDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> tradeDate()
		{
		  return tradeDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code tradeTime} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalTime> tradeTime()
		{
		  return tradeTime_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code zone} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ZoneId> zone()
		{
		  return zone_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code settlementDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> settlementDate()
		{
		  return settlementDate_Renamed;
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
			  return ((TradeInfo) bean).id;
			case -1651301782: // counterparty
			  return ((TradeInfo) bean).counterparty;
			case 752419634: // tradeDate
			  return ((TradeInfo) bean).tradeDate;
			case 752903761: // tradeTime
			  return ((TradeInfo) bean).tradeTime;
			case 3744684: // zone
			  return ((TradeInfo) bean).zone;
			case -295948169: // settlementDate
			  return ((TradeInfo) bean).settlementDate;
			case 405645655: // attributes
			  return ((TradeInfo) bean).Attributes;
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
	  /// The bean-builder for {@code TradeInfo}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<TradeInfo>
	  {

		internal StandardId id;
		internal StandardId counterparty;
		internal LocalDate tradeDate;
		internal LocalTime tradeTime;
		internal ZoneId zone;
		internal LocalDate settlementDate;
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
			case -1651301782: // counterparty
			  return counterparty;
			case 752419634: // tradeDate
			  return tradeDate;
			case 752903761: // tradeTime
			  return tradeTime;
			case 3744684: // zone
			  return zone;
			case -295948169: // settlementDate
			  return settlementDate;
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
			case -1651301782: // counterparty
			  this.counterparty = (StandardId) newValue;
			  break;
			case 752419634: // tradeDate
			  this.tradeDate = (LocalDate) newValue;
			  break;
			case 752903761: // tradeTime
			  this.tradeTime = (LocalTime) newValue;
			  break;
			case 3744684: // zone
			  this.zone = (ZoneId) newValue;
			  break;
			case -295948169: // settlementDate
			  this.settlementDate = (LocalDate) newValue;
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

		public override TradeInfo build()
		{
		  return new TradeInfo(id, counterparty, tradeDate, tradeTime, zone, settlementDate, attributes);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(256);
		  buf.Append("TradeInfo.Builder{");
		  buf.Append("id").Append('=').Append(JodaBeanUtils.ToString(id)).Append(',').Append(' ');
		  buf.Append("counterparty").Append('=').Append(JodaBeanUtils.ToString(counterparty)).Append(',').Append(' ');
		  buf.Append("tradeDate").Append('=').Append(JodaBeanUtils.ToString(tradeDate)).Append(',').Append(' ');
		  buf.Append("tradeTime").Append('=').Append(JodaBeanUtils.ToString(tradeTime)).Append(',').Append(' ');
		  buf.Append("zone").Append('=').Append(JodaBeanUtils.ToString(zone)).Append(',').Append(' ');
		  buf.Append("settlementDate").Append('=').Append(JodaBeanUtils.ToString(settlementDate)).Append(',').Append(' ');
		  buf.Append("attributes").Append('=').Append(JodaBeanUtils.ToString(attributes));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}