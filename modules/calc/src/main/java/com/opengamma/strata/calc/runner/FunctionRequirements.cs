using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
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

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Sets = com.google.common.collect.Sets;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;

	/// <summary>
	/// Specifies the market data required for a function to perform a calculation.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class FunctionRequirements implements org.joda.beans.ImmutableBean
	public sealed class FunctionRequirements : ImmutableBean
	{

	  /// <summary>
	  /// An empty set of requirements.
	  /// </summary>
	  private static readonly FunctionRequirements EMPTY = FunctionRequirements.builder().build();

	  /// <summary>
	  /// The market data identifiers of the values required for the calculation.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableSet<? extends com.opengamma.strata.data.MarketDataId<?>> valueRequirements;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  private readonly ImmutableSet<MarketDataId<object>> valueRequirements;
	  /// <summary>
	  /// The market data identifiers of the time-series of required for the calculation.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableSet<com.opengamma.strata.data.ObservableId> timeSeriesRequirements;
	  private readonly ImmutableSet<ObservableId> timeSeriesRequirements;
	  /// <summary>
	  /// The currencies used in the calculation results.
	  /// <para>
	  /// This cause FX rates to be requested that allow conversion between the currencies
	  /// specified and the reporting currency.
	  /// It will be possible to obtain any FX rate pair for these currencies.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableSet<com.opengamma.strata.basics.currency.Currency> outputCurrencies;
	  private readonly ImmutableSet<Currency> outputCurrencies;
	  /// <summary>
	  /// The source of market data for FX, quotes and other observable market data.
	  /// <para>
	  /// This is used to control the source of observable market data.
	  /// By default, this will be <seealso cref="ObservableSource#NONE"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.data.ObservableSource observableSource;
	  private readonly ObservableSource observableSource;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an empty set of requirements.
	  /// </summary>
	  /// <returns> an empty set of requirements </returns>
	  public static FunctionRequirements empty()
	  {
		return EMPTY;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.observableSource_Renamed = ObservableSource.NONE;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Combines these requirements with another set.
	  /// <para>
	  /// The result contains the union of the two sets of requirements.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other requirements </param>
	  /// <returns> the combined requirements </returns>
	  public FunctionRequirements combinedWith(FunctionRequirements other)
	  {
		return builder().valueRequirements(Sets.union(valueRequirements, other.valueRequirements)).timeSeriesRequirements(Sets.union(timeSeriesRequirements, other.timeSeriesRequirements)).outputCurrencies(Sets.union(outputCurrencies, other.outputCurrencies)).observableSource(!this.observableSource.Equals(ObservableSource.NONE) ? this.observableSource : other.observableSource).build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FunctionRequirements}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FunctionRequirements.Meta meta()
	  {
		return FunctionRequirements.Meta.INSTANCE;
	  }

	  static FunctionRequirements()
	  {
		MetaBean.register(FunctionRequirements.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static FunctionRequirements.Builder builder()
	  {
		return new FunctionRequirements.Builder();
	  }

	  private FunctionRequirements<T1>(ISet<T1> valueRequirements, ISet<ObservableId> timeSeriesRequirements, ISet<Currency> outputCurrencies, ObservableSource observableSource) where T1 : com.opengamma.strata.data.MarketDataId<T1>
	  {
		JodaBeanUtils.notNull(valueRequirements, "valueRequirements");
		JodaBeanUtils.notNull(timeSeriesRequirements, "timeSeriesRequirements");
		JodaBeanUtils.notNull(outputCurrencies, "outputCurrencies");
		JodaBeanUtils.notNull(observableSource, "observableSource");
		this.valueRequirements = ImmutableSet.copyOf(valueRequirements);
		this.timeSeriesRequirements = ImmutableSet.copyOf(timeSeriesRequirements);
		this.outputCurrencies = ImmutableSet.copyOf(outputCurrencies);
		this.observableSource = observableSource;
	  }

	  public override FunctionRequirements.Meta metaBean()
	  {
		return FunctionRequirements.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market data identifiers of the values required for the calculation. </summary>
	  /// <returns> the value of the property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.google.common.collect.ImmutableSet<? extends com.opengamma.strata.data.MarketDataId<?>> getValueRequirements()
	  public ImmutableSet<MarketDataId<object>> ValueRequirements
	  {
		  get
		  {
			return valueRequirements;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market data identifiers of the time-series of required for the calculation. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableSet<ObservableId> TimeSeriesRequirements
	  {
		  get
		  {
			return timeSeriesRequirements;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currencies used in the calculation results.
	  /// <para>
	  /// This cause FX rates to be requested that allow conversion between the currencies
	  /// specified and the reporting currency.
	  /// It will be possible to obtain any FX rate pair for these currencies.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableSet<Currency> OutputCurrencies
	  {
		  get
		  {
			return outputCurrencies;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the source of market data for FX, quotes and other observable market data.
	  /// <para>
	  /// This is used to control the source of observable market data.
	  /// By default, this will be <seealso cref="ObservableSource#NONE"/>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ObservableSource ObservableSource
	  {
		  get
		  {
			return observableSource;
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
		  FunctionRequirements other = (FunctionRequirements) obj;
		  return JodaBeanUtils.equal(valueRequirements, other.valueRequirements) && JodaBeanUtils.equal(timeSeriesRequirements, other.timeSeriesRequirements) && JodaBeanUtils.equal(outputCurrencies, other.outputCurrencies) && JodaBeanUtils.equal(observableSource, other.observableSource);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(valueRequirements);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(timeSeriesRequirements);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(outputCurrencies);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(observableSource);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("FunctionRequirements{");
		buf.Append("valueRequirements").Append('=').Append(valueRequirements).Append(',').Append(' ');
		buf.Append("timeSeriesRequirements").Append('=').Append(timeSeriesRequirements).Append(',').Append(' ');
		buf.Append("outputCurrencies").Append('=').Append(outputCurrencies).Append(',').Append(' ');
		buf.Append("observableSource").Append('=').Append(JodaBeanUtils.ToString(observableSource));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FunctionRequirements}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  valueRequirements_Renamed = DirectMetaProperty.ofImmutable(this, "valueRequirements", typeof(FunctionRequirements), (Type) typeof(ImmutableSet));
			  timeSeriesRequirements_Renamed = DirectMetaProperty.ofImmutable(this, "timeSeriesRequirements", typeof(FunctionRequirements), (Type) typeof(ImmutableSet));
			  outputCurrencies_Renamed = DirectMetaProperty.ofImmutable(this, "outputCurrencies", typeof(FunctionRequirements), (Type) typeof(ImmutableSet));
			  observableSource_Renamed = DirectMetaProperty.ofImmutable(this, "observableSource", typeof(FunctionRequirements), typeof(ObservableSource));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "valueRequirements", "timeSeriesRequirements", "outputCurrencies", "observableSource");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code valueRequirements} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableSet<? extends com.opengamma.strata.data.MarketDataId<?>>> valueRequirements = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "valueRequirements", FunctionRequirements.class, (Class) com.google.common.collect.ImmutableSet.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		internal MetaProperty<ImmutableSet<MarketDataId<object>>> valueRequirements_Renamed;
		/// <summary>
		/// The meta-property for the {@code timeSeriesRequirements} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableSet<com.opengamma.strata.data.ObservableId>> timeSeriesRequirements = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "timeSeriesRequirements", FunctionRequirements.class, (Class) com.google.common.collect.ImmutableSet.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableSet<ObservableId>> timeSeriesRequirements_Renamed;
		/// <summary>
		/// The meta-property for the {@code outputCurrencies} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableSet<com.opengamma.strata.basics.currency.Currency>> outputCurrencies = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "outputCurrencies", FunctionRequirements.class, (Class) com.google.common.collect.ImmutableSet.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableSet<Currency>> outputCurrencies_Renamed;
		/// <summary>
		/// The meta-property for the {@code observableSource} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ObservableSource> observableSource_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "valueRequirements", "timeSeriesRequirements", "outputCurrencies", "observableSource");
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
			case -1938886495: // valueRequirements
			  return valueRequirements_Renamed;
			case -1437279660: // timeSeriesRequirements
			  return timeSeriesRequirements_Renamed;
			case -1022597040: // outputCurrencies
			  return outputCurrencies_Renamed;
			case 1793526590: // observableSource
			  return observableSource_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override FunctionRequirements.Builder builder()
		{
		  return new FunctionRequirements.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FunctionRequirements);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code valueRequirements} property. </summary>
		/// <returns> the meta-property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public org.joda.beans.MetaProperty<com.google.common.collect.ImmutableSet<? extends com.opengamma.strata.data.MarketDataId<?>>> valueRequirements()
		public MetaProperty<ImmutableSet<MarketDataId<object>>> valueRequirements()
		{
		  return valueRequirements_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code timeSeriesRequirements} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableSet<ObservableId>> timeSeriesRequirements()
		{
		  return timeSeriesRequirements_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code outputCurrencies} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableSet<Currency>> outputCurrencies()
		{
		  return outputCurrencies_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code observableSource} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ObservableSource> observableSource()
		{
		  return observableSource_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1938886495: // valueRequirements
			  return ((FunctionRequirements) bean).ValueRequirements;
			case -1437279660: // timeSeriesRequirements
			  return ((FunctionRequirements) bean).TimeSeriesRequirements;
			case -1022597040: // outputCurrencies
			  return ((FunctionRequirements) bean).OutputCurrencies;
			case 1793526590: // observableSource
			  return ((FunctionRequirements) bean).ObservableSource;
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
	  /// The bean-builder for {@code FunctionRequirements}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<FunctionRequirements>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.Set<? extends com.opengamma.strata.data.MarketDataId<?>> valueRequirements = com.google.common.collect.ImmutableSet.of();
		internal ISet<MarketDataId<object>> valueRequirements_Renamed = ImmutableSet.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ISet<ObservableId> timeSeriesRequirements_Renamed = ImmutableSet.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ISet<Currency> outputCurrencies_Renamed = ImmutableSet.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ObservableSource observableSource_Renamed;

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
		internal Builder(FunctionRequirements beanToCopy)
		{
		  this.valueRequirements_Renamed = beanToCopy.ValueRequirements;
		  this.timeSeriesRequirements_Renamed = beanToCopy.TimeSeriesRequirements;
		  this.outputCurrencies_Renamed = beanToCopy.OutputCurrencies;
		  this.observableSource_Renamed = beanToCopy.ObservableSource;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1938886495: // valueRequirements
			  return valueRequirements_Renamed;
			case -1437279660: // timeSeriesRequirements
			  return timeSeriesRequirements_Renamed;
			case -1022597040: // outputCurrencies
			  return outputCurrencies_Renamed;
			case 1793526590: // observableSource
			  return observableSource_Renamed;
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
			case -1938886495: // valueRequirements
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.valueRequirements = (java.util.Set<? extends com.opengamma.strata.data.MarketDataId<?>>) newValue;
			  this.valueRequirements_Renamed = (ISet<MarketDataId<object>>) newValue;
			  break;
			case -1437279660: // timeSeriesRequirements
			  this.timeSeriesRequirements_Renamed = (ISet<ObservableId>) newValue;
			  break;
			case -1022597040: // outputCurrencies
			  this.outputCurrencies_Renamed = (ISet<Currency>) newValue;
			  break;
			case 1793526590: // observableSource
			  this.observableSource_Renamed = (ObservableSource) newValue;
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

		public override FunctionRequirements build()
		{
		  return new FunctionRequirements(valueRequirements_Renamed, timeSeriesRequirements_Renamed, outputCurrencies_Renamed, observableSource_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the market data identifiers of the values required for the calculation. </summary>
		/// <param name="valueRequirements">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder valueRequirements<T1>(ISet<T1> valueRequirements) where T1 : com.opengamma.strata.data.MarketDataId<T1>
		{
		  JodaBeanUtils.notNull(valueRequirements, "valueRequirements");
		  this.valueRequirements_Renamed = valueRequirements;
		  return this;
		}

		/// <summary>
		/// Sets the {@code valueRequirements} property in the builder
		/// from an array of objects. </summary>
		/// <param name="valueRequirements">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SafeVarargs public final Builder valueRequirements(com.opengamma.strata.data.MarketDataId<?>... valueRequirements)
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		public Builder valueRequirements(params MarketDataId<object>[] valueRequirements)
		{
		  return this.valueRequirements(ImmutableSet.copyOf(valueRequirements));
		}

		/// <summary>
		/// Sets the market data identifiers of the time-series of required for the calculation. </summary>
		/// <param name="timeSeriesRequirements">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder timeSeriesRequirements(ISet<ObservableId> timeSeriesRequirements)
		{
		  JodaBeanUtils.notNull(timeSeriesRequirements, "timeSeriesRequirements");
		  this.timeSeriesRequirements_Renamed = timeSeriesRequirements;
		  return this;
		}

		/// <summary>
		/// Sets the {@code timeSeriesRequirements} property in the builder
		/// from an array of objects. </summary>
		/// <param name="timeSeriesRequirements">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder timeSeriesRequirements(params ObservableId[] timeSeriesRequirements)
		{
		  return this.timeSeriesRequirements(ImmutableSet.copyOf(timeSeriesRequirements));
		}

		/// <summary>
		/// Sets the currencies used in the calculation results.
		/// <para>
		/// This cause FX rates to be requested that allow conversion between the currencies
		/// specified and the reporting currency.
		/// It will be possible to obtain any FX rate pair for these currencies.
		/// </para>
		/// </summary>
		/// <param name="outputCurrencies">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder outputCurrencies(ISet<Currency> outputCurrencies)
		{
		  JodaBeanUtils.notNull(outputCurrencies, "outputCurrencies");
		  this.outputCurrencies_Renamed = outputCurrencies;
		  return this;
		}

		/// <summary>
		/// Sets the {@code outputCurrencies} property in the builder
		/// from an array of objects. </summary>
		/// <param name="outputCurrencies">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder outputCurrencies(params Currency[] outputCurrencies)
		{
		  return this.outputCurrencies(ImmutableSet.copyOf(outputCurrencies));
		}

		/// <summary>
		/// Sets the source of market data for FX, quotes and other observable market data.
		/// <para>
		/// This is used to control the source of observable market data.
		/// By default, this will be <seealso cref="ObservableSource#NONE"/>.
		/// </para>
		/// </summary>
		/// <param name="observableSource">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder observableSource(ObservableSource observableSource)
		{
		  JodaBeanUtils.notNull(observableSource, "observableSource");
		  this.observableSource_Renamed = observableSource;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("FunctionRequirements.Builder{");
		  buf.Append("valueRequirements").Append('=').Append(JodaBeanUtils.ToString(valueRequirements_Renamed)).Append(',').Append(' ');
		  buf.Append("timeSeriesRequirements").Append('=').Append(JodaBeanUtils.ToString(timeSeriesRequirements_Renamed)).Append(',').Append(' ');
		  buf.Append("outputCurrencies").Append('=').Append(JodaBeanUtils.ToString(outputCurrencies_Renamed)).Append(',').Append(' ');
		  buf.Append("observableSource").Append('=').Append(JodaBeanUtils.ToString(observableSource_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}