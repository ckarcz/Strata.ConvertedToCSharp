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

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// A container for market data configuration objects which all have the same type.
	/// <para>
	/// This class wraps a map where the keys are the names of the configuration objects and the values
	/// are the configuration objects themselves.
	/// </para>
	/// <para>
	/// This class only exists to enable serialization. Market data configuration objects are referenced by
	/// name and type. The obvious implementation strategy is to use a pair of type and name to as keys
	/// when storing them in a map. Unfortunately this won't serialize correctly using Joda Beans so it
	/// is necessary to store them over two levels. {@code MarketDataConfig} contains a map of
	/// {@code SingleTypeMarketDataConfig} instances keyed by the type of the config objects, and
	/// the {@code SingleTypeMarketDataConfig} instances contains the configuration objects keyed by name.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition final class SingleTypeMarketDataConfig implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	internal sealed class SingleTypeMarketDataConfig : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final Class configType;
		private readonly Type configType;

	  /// <summary>
	  /// The configuration objects, keyed by name. </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<String, Object> configObjects;
	  private readonly ImmutableMap<string, object> configObjects;

	  /// <summary>
	  /// Returns the configuration object with the specified name if found or throws an exception if not.
	  /// </summary>
	  /// <param name="name">  the name of the configuration object </param>
	  /// <returns> the named object if available </returns>
	  /// <exception cref="IllegalArgumentException"> if no configuration is found with the specified name </exception>
	  internal object get(string name)
	  {
		object config = configObjects.get(name);

		if (config == null)
		{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		  throw new System.ArgumentException(Messages.format("No configuration found with type {} and name {}", configType.FullName, name));
		}
		return config;
	  }

	  /// <summary>
	  /// Returns a copy of this set of configuration with the specified object added.
	  /// <para>
	  /// The configuration object must be the same type os this object's {@code configType}.
	  /// </para>
	  /// <para>
	  /// If an object is already present with the specified name it will be replaced.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name of the configuration object </param>
	  /// <param name="config">  the configuration object </param>
	  /// <returns> a copy of this set of configuration with the specified object added </returns>
	  /// <exception cref="IllegalArgumentException"> if the configuration object is not of the required type </exception>
	  internal SingleTypeMarketDataConfig withConfig(string name, object config)
	  {
		ArgChecker.notEmpty(name, "name");
		ArgChecker.notNull(config, "config");

		if (!configType.Equals(config.GetType()))
		{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		  throw new System.ArgumentException(Messages.format("Configuration object {} is not of the required type {}", config, configType.FullName));
		}
		// Use a hash map to allow values to be overwritten, preserving normal map semantics.
		// ImmutableMap builder throws an exception when there are duplicate keys.
		Dictionary<string, object> configCopy = new Dictionary<string, object>(configObjects);
		configCopy[name] = config;
		return new SingleTypeMarketDataConfig(configType, configCopy);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code SingleTypeMarketDataConfig}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static SingleTypeMarketDataConfig.Meta meta()
	  {
		return SingleTypeMarketDataConfig.Meta.INSTANCE;
	  }

	  static SingleTypeMarketDataConfig()
	  {
		MetaBean.register(SingleTypeMarketDataConfig.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  internal static SingleTypeMarketDataConfig.Builder builder()
	  {
		return new SingleTypeMarketDataConfig.Builder();
	  }

	  private SingleTypeMarketDataConfig(Type configType, IDictionary<string, object> configObjects)
	  {
		JodaBeanUtils.notNull(configType, "configType");
		JodaBeanUtils.notNull(configObjects, "configObjects");
		this.configType = configType;
		this.configObjects = ImmutableMap.copyOf(configObjects);
	  }

	  public override SingleTypeMarketDataConfig.Meta metaBean()
	  {
		return SingleTypeMarketDataConfig.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the type of the configuration objects. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Type ConfigType
	  {
		  get
		  {
			return configType;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the configuration objects, keyed by name. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<string, object> ConfigObjects
	  {
		  get
		  {
			return configObjects;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Returns a builder that allows this bean to be mutated. </summary>
	  /// <returns> the mutable builder, not null </returns>
	  internal Builder toBuilder()
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
		  SingleTypeMarketDataConfig other = (SingleTypeMarketDataConfig) obj;
		  return JodaBeanUtils.equal(configType, other.configType) && JodaBeanUtils.equal(configObjects, other.configObjects);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(configType);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(configObjects);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("SingleTypeMarketDataConfig{");
		buf.Append("configType").Append('=').Append(configType).Append(',').Append(' ');
		buf.Append("configObjects").Append('=').Append(JodaBeanUtils.ToString(configObjects));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code SingleTypeMarketDataConfig}.
	  /// </summary>
	  internal sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  configType_Renamed = DirectMetaProperty.ofImmutable(this, "configType", typeof(SingleTypeMarketDataConfig), (Type) typeof(Type));
			  configObjects_Renamed = DirectMetaProperty.ofImmutable(this, "configObjects", typeof(SingleTypeMarketDataConfig), (Type) typeof(ImmutableMap));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "configType", "configObjects");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code configType} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<Class> configType = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "configType", SingleTypeMarketDataConfig.class, (Class) Class.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Type> configType_Renamed;
		/// <summary>
		/// The meta-property for the {@code configObjects} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<String, Object>> configObjects = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "configObjects", SingleTypeMarketDataConfig.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<string, object>> configObjects_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "configType", "configObjects");
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
			case 831526300: // configType
			  return configType_Renamed;
			case 2117143410: // configObjects
			  return configObjects_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override SingleTypeMarketDataConfig.Builder builder()
		{
		  return new SingleTypeMarketDataConfig.Builder();
		}

		public override Type beanType()
		{
		  return typeof(SingleTypeMarketDataConfig);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code configType} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Type> configType()
		{
		  return configType_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code configObjects} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<string, object>> configObjects()
		{
		  return configObjects_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 831526300: // configType
			  return ((SingleTypeMarketDataConfig) bean).ConfigType;
			case 2117143410: // configObjects
			  return ((SingleTypeMarketDataConfig) bean).ConfigObjects;
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
	  /// The bean-builder for {@code SingleTypeMarketDataConfig}.
	  /// </summary>
	  internal sealed class Builder : DirectFieldsBeanBuilder<SingleTypeMarketDataConfig>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Type configType_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IDictionary<string, object> configObjects_Renamed = ImmutableMap.of();

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(SingleTypeMarketDataConfig beanToCopy)
		{
		  this.configType_Renamed = beanToCopy.ConfigType;
		  this.configObjects_Renamed = beanToCopy.ConfigObjects;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 831526300: // configType
			  return configType_Renamed;
			case 2117143410: // configObjects
			  return configObjects_Renamed;
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
			case 831526300: // configType
			  this.configType_Renamed = (Type) newValue;
			  break;
			case 2117143410: // configObjects
			  this.configObjects_Renamed = (IDictionary<string, object>) newValue;
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

		public override SingleTypeMarketDataConfig build()
		{
		  return new SingleTypeMarketDataConfig(configType_Renamed, configObjects_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the type of the configuration objects. </summary>
		/// <param name="configType">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder configType(Type configType)
		{
		  JodaBeanUtils.notNull(configType, "configType");
		  this.configType_Renamed = configType;
		  return this;
		}

		/// <summary>
		/// Sets the configuration objects, keyed by name. </summary>
		/// <param name="configObjects">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder configObjects(IDictionary<string, object> configObjects)
		{
		  JodaBeanUtils.notNull(configObjects, "configObjects");
		  this.configObjects_Renamed = configObjects;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("SingleTypeMarketDataConfig.Builder{");
		  buf.Append("configType").Append('=').Append(JodaBeanUtils.ToString(configType_Renamed)).Append(',').Append(' ');
		  buf.Append("configObjects").Append('=').Append(JodaBeanUtils.ToString(configObjects_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}