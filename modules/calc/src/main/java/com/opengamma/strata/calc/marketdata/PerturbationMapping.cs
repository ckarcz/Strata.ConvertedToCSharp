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

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Messages = com.opengamma.strata.collect.Messages;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioPerturbation = com.opengamma.strata.data.scenario.ScenarioPerturbation;

	/// <summary>
	/// Contains a market data perturbation and a filter that decides what market data it applies to.
	/// <para>
	/// The mapping links the perturbation to be applied to the filter that decides whether it applies.
	/// The generic types of the filter can be for a subtype of the type that the perturbation applies to.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of the market data handled by the mapping </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class PerturbationMapping<T> implements org.joda.beans.ImmutableBean
	public sealed class PerturbationMapping<T> : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final Class<T> marketDataType;
		private readonly Type<T> marketDataType;

	  /// <summary>
	  /// The filter that decides whether the perturbation should be applied to a piece of market data. </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final MarketDataFilter<? extends T, ?> filter;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  private readonly MarketDataFilter<T, ?> filter;

	  /// <summary>
	  /// Perturbation that should be applied to market data as part of a scenario. </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.data.scenario.ScenarioPerturbation<T> perturbation;
	  private readonly ScenarioPerturbation<T> perturbation;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a mapping containing a single perturbation.
	  /// <para>
	  /// This uses the type from <seealso cref="ScenarioPerturbation"/> as the type of the mapping.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the market data handled by the mapping </param>
	  /// <param name="filter">  the filter used to choose the market data </param>
	  /// <param name="perturbation">  the perturbation applied to any market data matching the filter </param>
	  /// <returns> a mapping containing a single perturbation </returns>
	  public static PerturbationMapping<T> of<T, T1>(MarketDataFilter<T1> filter, ScenarioPerturbation<T> perturbation) where T1 : T
	  {
		return new PerturbationMapping<T>(perturbation.MarketDataType, filter, perturbation);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns true if the filter matches the market data ID and value.
	  /// </summary>
	  /// <param name="marketDataId">  the ID of a piece of market data </param>
	  /// <param name="marketData">  the market data value </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> true if the filter matches </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public boolean matches(com.opengamma.strata.data.MarketDataId<?> marketDataId, com.opengamma.strata.data.scenario.MarketDataBox<?> marketData, com.opengamma.strata.basics.ReferenceData refData)
	  public bool matches<T1, T2>(MarketDataId<T1> marketDataId, MarketDataBox<T2> marketData, ReferenceData refData)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("rawtypes") MarketDataFilter rawFilter = filter;
		  MarketDataFilter rawFilter = filter;

		return marketDataType.IsAssignableFrom(marketData.MarketDataType) && filter.MarketDataIdType.IsInstanceOfType(marketDataId) && rawFilter.matches(marketDataId, marketData, refData);
	  }

	  /// <summary>
	  /// Applies the perturbations in this mapping to an item of market data and returns the results.
	  /// <para>
	  /// This method should only be called after calling {@code #matches} and receiving a result of {@code true}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketData">  the market data value </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> a list of market data values derived from the input value by applying the perturbations </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public com.opengamma.strata.data.scenario.MarketDataBox<T> applyPerturbation(com.opengamma.strata.data.scenario.MarketDataBox<T> marketData, com.opengamma.strata.basics.ReferenceData refData)
	  public MarketDataBox<T> applyPerturbation(MarketDataBox<T> marketData, ReferenceData refData)
	  {
		if (!marketDataType.IsAssignableFrom(marketData.MarketDataType))
		{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		  throw new System.ArgumentException(Messages.format("Market data {} is not an instance of the required type {}", marketData, marketDataType.FullName));
		}
		return perturbation.applyTo(marketData, refData);
	  }

	  /// <summary>
	  /// Returns the number of scenarios for which this mapping can generate data.
	  /// </summary>
	  /// <returns> the number of scenarios for which this mapping can generate data </returns>
	  public int ScenarioCount
	  {
		  get
		  {
			return perturbation.ScenarioCount;
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code PerturbationMapping}. </summary>
	  /// <returns> the meta-bean, not null </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("rawtypes") public static PerturbationMapping.Meta meta()
	  public static PerturbationMapping.Meta meta()
	  {
		return PerturbationMapping.Meta.INSTANCE;
	  }

	  /// <summary>
	  /// The meta-bean for {@code PerturbationMapping}. </summary>
	  /// @param <R>  the bean's generic type </param>
	  /// <param name="cls">  the bean's generic type </param>
	  /// <returns> the meta-bean, not null </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public static <R> PerturbationMapping.Meta<R> metaPerturbationMapping(Class<R> cls)
	  public static PerturbationMapping.Meta<R> metaPerturbationMapping<R>(Type<R> cls)
	  {
		return PerturbationMapping.Meta.INSTANCE;
	  }

	  static PerturbationMapping()
	  {
		MetaBean.register(PerturbationMapping.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// @param <T>  the type </param>
	  /// <returns> the builder, not null </returns>
	  public static PerturbationMapping.Builder<T> builder<T>()
	  {
		return new PerturbationMapping.Builder<T>();
	  }

	  private PerturbationMapping<T1>(Type<T> marketDataType, MarketDataFilter<T1> filter, ScenarioPerturbation<T> perturbation) where T1 : T
	  {
		JodaBeanUtils.notNull(marketDataType, "marketDataType");
		JodaBeanUtils.notNull(filter, "filter");
		JodaBeanUtils.notNull(perturbation, "perturbation");
		this.marketDataType = marketDataType;
		this.filter = filter;
		this.perturbation = perturbation;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public PerturbationMapping.Meta<T> metaBean()
	  public override PerturbationMapping.Meta<T> metaBean()
	  {
		return PerturbationMapping.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the type of market data handled by this mapping. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Type<T> MarketDataType
	  {
		  get
		  {
			return marketDataType;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the filter that decides whether the perturbation should be applied to a piece of market data. </summary>
	  /// <returns> the value of the property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public MarketDataFilter<? extends T, ?> getFilter()
	  public MarketDataFilter<T, ?> Filter
	  {
		  get
		  {
			return filter;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets perturbation that should be applied to market data as part of a scenario. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ScenarioPerturbation<T> Perturbation
	  {
		  get
		  {
			return perturbation;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Returns a builder that allows this bean to be mutated. </summary>
	  /// <returns> the mutable builder, not null </returns>
	  public Builder<T> toBuilder()
	  {
		return new Builder<T>(this);
	  }

	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: PerturbationMapping<?> other = (PerturbationMapping<?>) obj;
		  PerturbationMapping<object> other = (PerturbationMapping<object>) obj;
		  return JodaBeanUtils.equal(marketDataType, other.marketDataType) && JodaBeanUtils.equal(filter, other.filter) && JodaBeanUtils.equal(perturbation, other.perturbation);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(marketDataType);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(filter);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(perturbation);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("PerturbationMapping{");
		buf.Append("marketDataType").Append('=').Append(marketDataType).Append(',').Append(' ');
		buf.Append("filter").Append('=').Append(filter).Append(',').Append(' ');
		buf.Append("perturbation").Append('=').Append(JodaBeanUtils.ToString(perturbation));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code PerturbationMapping}. </summary>
	  /// @param <T>  the type </param>
	  public sealed class Meta<T> : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  marketDataType_Renamed = DirectMetaProperty.ofImmutable(this, "marketDataType", typeof(PerturbationMapping), (Type) typeof(Type));
			  filter_Renamed = DirectMetaProperty.ofImmutable(this, "filter", typeof(PerturbationMapping), (Type) typeof(MarketDataFilter));
			  perturbation_Renamed = DirectMetaProperty.ofImmutable(this, "perturbation", typeof(PerturbationMapping), (Type) typeof(ScenarioPerturbation));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "marketDataType", "filter", "perturbation");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("rawtypes") static final Meta INSTANCE = new Meta();
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code marketDataType} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<Class<T>> marketDataType = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "marketDataType", PerturbationMapping.class, (Class) Class.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Type<T>> marketDataType_Renamed;
		/// <summary>
		/// The meta-property for the {@code filter} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<MarketDataFilter<? extends T, ?>> filter = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "filter", PerturbationMapping.class, (Class) MarketDataFilter.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		internal MetaProperty<MarketDataFilter<T, ?>> filter_Renamed;
		/// <summary>
		/// The meta-property for the {@code perturbation} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.opengamma.strata.data.scenario.ScenarioPerturbation<T>> perturbation = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "perturbation", PerturbationMapping.class, (Class) com.opengamma.strata.data.scenario.ScenarioPerturbation.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ScenarioPerturbation<T>> perturbation_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "marketDataType", "filter", "perturbation");
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
			case 843057760: // marketDataType
			  return marketDataType_Renamed;
			case -1274492040: // filter
			  return filter_Renamed;
			case -924739417: // perturbation
			  return perturbation_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override PerturbationMapping.Builder<T> builder()
		{
		  return new PerturbationMapping.Builder<T>();
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) @Override public Class beanType()
		public override Type beanType()
		{
		  return (Type) typeof(PerturbationMapping);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code marketDataType} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Type<T>> marketDataType()
		{
		  return marketDataType_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code filter} property. </summary>
		/// <returns> the meta-property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public org.joda.beans.MetaProperty<MarketDataFilter<? extends T, ?>> filter()
		public MetaProperty<MarketDataFilter<T, ?>> filter()
		{
		  return filter_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code perturbation} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ScenarioPerturbation<T>> perturbation()
		{
		  return perturbation_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 843057760: // marketDataType
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return ((PerturbationMapping<?>) bean).getMarketDataType();
			  return ((PerturbationMapping<object>) bean).MarketDataType;
			case -1274492040: // filter
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return ((PerturbationMapping<?>) bean).getFilter();
			  return ((PerturbationMapping<object>) bean).Filter;
			case -924739417: // perturbation
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return ((PerturbationMapping<?>) bean).getPerturbation();
			  return ((PerturbationMapping<object>) bean).Perturbation;
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
	  /// The bean-builder for {@code PerturbationMapping}. </summary>
	  /// @param <T>  the type </param>
	  public sealed class Builder<T> : DirectFieldsBeanBuilder<PerturbationMapping<T>>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Type<T> marketDataType_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private MarketDataFilter<? extends T, ?> filter;
		internal MarketDataFilter<T, ?> filter_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ScenarioPerturbation<T> perturbation_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(PerturbationMapping<T> beanToCopy)
		{
		  this.marketDataType_Renamed = beanToCopy.MarketDataType;
		  this.filter_Renamed = beanToCopy.Filter;
		  this.perturbation_Renamed = beanToCopy.Perturbation;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 843057760: // marketDataType
			  return marketDataType_Renamed;
			case -1274492040: // filter
			  return filter_Renamed;
			case -924739417: // perturbation
			  return perturbation_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Builder<T> set(String propertyName, Object newValue)
		public override Builder<T> set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 843057760: // marketDataType
			  this.marketDataType_Renamed = (Type<T>) newValue;
			  break;
			case -1274492040: // filter
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.filter = (MarketDataFilter<? extends T, ?>) newValue;
			  this.filter_Renamed = (MarketDataFilter<T, ?>) newValue;
			  break;
			case -924739417: // perturbation
			  this.perturbation_Renamed = (ScenarioPerturbation<T>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override Builder<T> set<T1>(MetaProperty<T1> property, object value)
		{
		  base.set(property, value);
		  return this;
		}

		public override PerturbationMapping<T> build()
		{
		  return new PerturbationMapping<T>(marketDataType_Renamed, filter_Renamed, perturbation_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the type of market data handled by this mapping. </summary>
		/// <param name="marketDataType">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder<T> marketDataType(Type<T> marketDataType)
		{
		  JodaBeanUtils.notNull(marketDataType, "marketDataType");
		  this.marketDataType_Renamed = marketDataType;
		  return this;
		}

		/// <summary>
		/// Sets the filter that decides whether the perturbation should be applied to a piece of market data. </summary>
		/// <param name="filter">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder<T> filter<T1>(MarketDataFilter<T1> filter) where T1 : T
		{
		  JodaBeanUtils.notNull(filter, "filter");
		  this.filter_Renamed = filter;
		  return this;
		}

		/// <summary>
		/// Sets perturbation that should be applied to market data as part of a scenario. </summary>
		/// <param name="perturbation">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder<T> perturbation(ScenarioPerturbation<T> perturbation)
		{
		  JodaBeanUtils.notNull(perturbation, "perturbation");
		  this.perturbation_Renamed = perturbation;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("PerturbationMapping.Builder{");
		  buf.Append("marketDataType").Append('=').Append(JodaBeanUtils.ToString(marketDataType_Renamed)).Append(',').Append(' ');
		  buf.Append("filter").Append('=').Append(JodaBeanUtils.ToString(filter_Renamed)).Append(',').Append(' ');
		  buf.Append("perturbation").Append('=').Append(JodaBeanUtils.ToString(perturbation_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}