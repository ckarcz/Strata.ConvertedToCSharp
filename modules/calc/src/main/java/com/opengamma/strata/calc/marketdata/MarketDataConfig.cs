using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.marketdata
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


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

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using MapStream = com.opengamma.strata.collect.MapStream;
	using Messages = com.opengamma.strata.collect.Messages;
	using TypedString = com.opengamma.strata.collect.TypedString;

	/// <summary>
	/// Configuration required for building non-observable market data, for example curves or surfaces.
	/// <para>
	/// This class is effectively a map of arbitrary objects, keyed by their type and a name.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private", constructorScope = "package") public final class MarketDataConfig implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class MarketDataConfig : ImmutableBean
	{

	  /// <summary>
	  /// An empty set of market data configuration. </summary>
	  private static readonly MarketDataConfig EMPTY = new MarketDataConfig(ImmutableMap.of(), ImmutableMap.of());

	  /// <summary>
	  /// The configuration objects, keyed by their type and name. </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", get = "private") private final com.google.common.collect.ImmutableMap<Class, SingleTypeMarketDataConfig> configs;
	  private readonly ImmutableMap<Type, SingleTypeMarketDataConfig> configs;

	  /// <summary>
	  /// The configuration objects where there is only one instance per type. </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", get = "private") private final com.google.common.collect.ImmutableMap<Class, Object> defaultConfigs;
	  private readonly ImmutableMap<Type, object> defaultConfigs;

	  /// <summary>
	  /// Returns an empty set of market data configuration.
	  /// </summary>
	  /// <returns> an empty set of market data configuration </returns>
	  public static MarketDataConfig empty()
	  {
		return MarketDataConfig.EMPTY;
	  }

	  /// <summary>
	  /// Returns a mutable builder for building an instance of {@code MarketDataConfig}.
	  /// </summary>
	  /// <returns> a mutable builder for building an instance of {@code MarketDataConfig} </returns>
	  public static MarketDataConfigBuilder builder()
	  {
		return new MarketDataConfigBuilder();
	  }

	  /// <summary>
	  /// Returns the configuration object with the specified type and name if available.
	  /// </summary>
	  /// <param name="type"> the type of the configuration object </param>
	  /// <param name="name"> the name of the configuration object </param>
	  /// @param <T> the type of the configuration object </param>
	  /// <returns> the configuration with the specified type and name </returns>
	  /// <exception cref="IllegalArgumentException"> if no configuration is found with the specified type and name </exception>
	  public T get<T>(Type<T> type, string name)
	  {
		SingleTypeMarketDataConfig typeConfigs = configs.get(type);
		// simple match
		if (typeConfigs != null)
		{
		  object config = typeConfigs.get(name);
		  if (config == null)
		  {
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
			throw new System.ArgumentException(Messages.format("No configuration found with type {} and name {}", type.FullName, name));
		  }
		  return type.cast(config);
		}
		// try looking for subclasses of an interface
		if (!type.IsInterface)
		{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		  throw new System.ArgumentException("No configuration found for type " + type.FullName);
		}
		ImmutableList<SingleTypeMarketDataConfig> potentialTypes = MapStream.of(configs).filterKeys(type.isAssignableFrom).map((t, config) => config).filter(config => config.ConfigObjects.containsKey(name)).collect(toImmutableList());
		if (potentialTypes.Empty)
		{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		  throw new System.ArgumentException("No configuration found for type " + type.FullName + " or subclasses");
		}
		if (potentialTypes.size() > 1)
		{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		  throw new System.ArgumentException("Multiple configuration found for type " + type.FullName);
		}
		return type.cast(potentialTypes.get(0).get(name));
	  }

	  /// <summary>
	  /// Returns the configuration object with the specified type and name if available.
	  /// </summary>
	  /// <param name="type"> the type of the configuration object </param>
	  /// <param name="name"> the name of the configuration object </param>
	  /// @param <T> the type of the configuration object </param>
	  /// <returns> the configuration with the specified type and name </returns>
	  /// <exception cref="IllegalArgumentException"> if no configuration is found with the specified type and name </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public <T> T get(Class<T> type, com.opengamma.strata.collect.TypedString<?> name)
	  public T get<T, T1>(Type<T> type, TypedString<T1> name)
	  {
		return get(type, name.Name);
	  }

	  /// <summary>
	  /// Returns an item of configuration that is the default of its type.
	  /// <para>
	  /// There can only be one default item for each type.
	  /// </para>
	  /// <para>
	  /// There is a class of configuration where there is always a one value shared between all calculations.
	  /// An example is the configuration which specifies which market quote to use when building FX rates for
	  /// a currency pair. All calculations use the same set of FX rates obtained from the same underlying
	  /// market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="type"> the type of the configuration object </param>
	  /// @param <T> the type of the configuration object </param>
	  /// <returns> the configuration with the specified type </returns>
	  /// <exception cref="IllegalArgumentException"> if no configuration is found with the specified type and name </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public <T> T get(Class<T> type)
	  public T get<T>(Type<T> type)
	  {
		object config = defaultConfigs.get(type);

		if (config == null)
		{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		  throw new System.ArgumentException("No default configuration found with type " + type.FullName);
		}
		return (T) config;
	  }

	  /// <summary>
	  /// Returns an item of configuration that is the default of its type.
	  /// <para>
	  /// There can only be one default item for each type.
	  /// </para>
	  /// <para>
	  /// There is a class of configuration where there is always a one value shared between all calculations.
	  /// An example is the configuration which specifies which market quote to use when building FX rates for
	  /// a currency pair. All calculations use the same set of FX rates obtained from the same underlying
	  /// market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="type"> the type of the configuration object </param>
	  /// @param <T> the type of the configuration object </param>
	  /// <returns> the configuration with the specified type, empty if not found </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public <T> java.util.Optional<T> find(Class<T> type)
	  public Optional<T> find<T>(Type<T> type)
	  {
		return Optional.ofNullable((T) defaultConfigs.get(type));
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code MarketDataConfig}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static MarketDataConfig.Meta meta()
	  {
		return MarketDataConfig.Meta.INSTANCE;
	  }

	  static MarketDataConfig()
	  {
		MetaBean.register(MarketDataConfig.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="configs">  the value of the property, not null </param>
	  /// <param name="defaultConfigs">  the value of the property, not null </param>
	  internal MarketDataConfig(IDictionary<Type, SingleTypeMarketDataConfig> configs, IDictionary<Type, object> defaultConfigs)
	  {
		JodaBeanUtils.notNull(configs, "configs");
		JodaBeanUtils.notNull(defaultConfigs, "defaultConfigs");
		this.configs = ImmutableMap.copyOf(configs);
		this.defaultConfigs = ImmutableMap.copyOf(defaultConfigs);
	  }

	  public override MarketDataConfig.Meta metaBean()
	  {
		return MarketDataConfig.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the configuration objects, keyed by their type and name. </summary>
	  /// <returns> the value of the property, not null </returns>
	  private ImmutableMap<Type, SingleTypeMarketDataConfig> Configs
	  {
		  get
		  {
			return configs;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the configuration objects where there is only one instance per type. </summary>
	  /// <returns> the value of the property, not null </returns>
	  private ImmutableMap<Type, object> DefaultConfigs
	  {
		  get
		  {
			return defaultConfigs;
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
		  MarketDataConfig other = (MarketDataConfig) obj;
		  return JodaBeanUtils.equal(configs, other.configs) && JodaBeanUtils.equal(defaultConfigs, other.defaultConfigs);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(configs);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(defaultConfigs);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("MarketDataConfig{");
		buf.Append("configs").Append('=').Append(configs).Append(',').Append(' ');
		buf.Append("defaultConfigs").Append('=').Append(JodaBeanUtils.ToString(defaultConfigs));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code MarketDataConfig}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  configs_Renamed = DirectMetaProperty.ofImmutable(this, "configs", typeof(MarketDataConfig), (Type) typeof(ImmutableMap));
			  defaultConfigs_Renamed = DirectMetaProperty.ofImmutable(this, "defaultConfigs", typeof(MarketDataConfig), (Type) typeof(ImmutableMap));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "configs", "defaultConfigs");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code configs} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<Class, SingleTypeMarketDataConfig>> configs = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "configs", MarketDataConfig.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<Type, SingleTypeMarketDataConfig>> configs_Renamed;
		/// <summary>
		/// The meta-property for the {@code defaultConfigs} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<Class, Object>> defaultConfigs = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "defaultConfigs", MarketDataConfig.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<Type, object>> defaultConfigs_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "configs", "defaultConfigs");
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
			case 951117169: // configs
			  return configs_Renamed;
			case -1339733008: // defaultConfigs
			  return defaultConfigs_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends MarketDataConfig> builder()
		public override BeanBuilder<MarketDataConfig> builder()
		{
		  return new MarketDataConfig.Builder();
		}

		public override Type beanType()
		{
		  return typeof(MarketDataConfig);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code configs} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<Type, SingleTypeMarketDataConfig>> configs()
		{
		  return configs_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code defaultConfigs} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<Type, object>> defaultConfigs()
		{
		  return defaultConfigs_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 951117169: // configs
			  return ((MarketDataConfig) bean).Configs;
			case -1339733008: // defaultConfigs
			  return ((MarketDataConfig) bean).DefaultConfigs;
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
	  /// The bean-builder for {@code MarketDataConfig}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<MarketDataConfig>
	  {

		internal IDictionary<Type, SingleTypeMarketDataConfig> configs = ImmutableMap.of();
		internal IDictionary<Type, object> defaultConfigs = ImmutableMap.of();

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
			case 951117169: // configs
			  return configs;
			case -1339733008: // defaultConfigs
			  return defaultConfigs;
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
			case 951117169: // configs
			  this.configs = (IDictionary<Type, SingleTypeMarketDataConfig>) newValue;
			  break;
			case -1339733008: // defaultConfigs
			  this.defaultConfigs = (IDictionary<Type, object>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override MarketDataConfig build()
		{
		  return new MarketDataConfig(configs, defaultConfigs);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("MarketDataConfig.Builder{");
		  buf.Append("configs").Append('=').Append(JodaBeanUtils.ToString(configs)).Append(',').Append(' ');
		  buf.Append("defaultConfigs").Append('=').Append(JodaBeanUtils.ToString(defaultConfigs));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}