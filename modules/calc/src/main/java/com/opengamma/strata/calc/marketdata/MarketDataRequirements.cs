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

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CalculationTasks = com.opengamma.strata.calc.runner.CalculationTasks;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using ObservableId = com.opengamma.strata.data.ObservableId;

	/// <summary>
	/// Requirements for market data.
	/// <para>
	/// This class is used as the input to <seealso cref="MarketDataFactory"/>.
	/// It includes the market data identifiers that the application needs.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private", constructorScope = "package") public final class MarketDataRequirements implements org.joda.beans.ImmutableBean
	public sealed class MarketDataRequirements : ImmutableBean
	{

	  /// <summary>
	  /// A set of requirements which specifies that no market data is required. </summary>
	  private static readonly MarketDataRequirements EMPTY = MarketDataRequirements.builder().build();

	  /// <summary>
	  /// Keys identifying the market data values required for the calculations. </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", builderType = "Set<? extends ObservableId>") private final com.google.common.collect.ImmutableSet<com.opengamma.strata.data.ObservableId> observables;
	  private readonly ImmutableSet<ObservableId> observables;

	  /// <summary>
	  /// Keys identifying the market data values required for the calculations. </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", builderType = "Set<? extends MarketDataId<?>>") private final com.google.common.collect.ImmutableSet<com.opengamma.strata.data.MarketDataId<?>> nonObservables;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  private readonly ImmutableSet<MarketDataId<object>> nonObservables;

	  /// <summary>
	  /// Keys identifying the time series of market data values required for the calculations. </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", builderType = "Set<? extends ObservableId>") private final com.google.common.collect.ImmutableSet<com.opengamma.strata.data.ObservableId> timeSeries;
	  private readonly ImmutableSet<ObservableId> timeSeries;

	  /// <summary>
	  /// The currencies in the calculation results. The market data must include FX rates in the
	  /// to allow conversion into the reporting currency. The FX rates must have the output currency as the base
	  /// currency and the reporting currency as the counter currency.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableSet<com.opengamma.strata.basics.currency.Currency> outputCurrencies;
	  private readonly ImmutableSet<Currency> outputCurrencies;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a set of targets, columns and rules.
	  /// <para>
	  /// The targets will typically be trades.
	  /// The columns represent the measures to calculate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="calculationRules">  the rules defining how the calculation is performed </param>
	  /// <param name="targets">  the targets for which values of the measures will be calculated </param>
	  /// <param name="columns">  the columns that will be calculated </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the market data requirements </returns>
	  public static MarketDataRequirements of<T1>(CalculationRules calculationRules, IList<T1> targets, IList<Column> columns, ReferenceData refData) where T1 : com.opengamma.strata.basics.CalculationTarget
	  {

		return CalculationTasks.of(calculationRules, targets, columns, refData).requirements(refData);
	  }

	  /// <summary>
	  /// Obtains an instance containing a single market data ID.
	  /// </summary>
	  /// <param name="id">  the ID of the only market data value required </param>
	  /// <returns> a set of requirements containing a single market data ID </returns>
	  public static MarketDataRequirements of<T1>(MarketDataId<T1> id)
	  {
		return builder().addValues(id).build();
	  }

	  /// <summary>
	  /// Obtains an instance specifying that no market data is required.
	  /// </summary>
	  /// <returns> a set of requirements specifying that no market data is required </returns>
	  public static MarketDataRequirements empty()
	  {
		return EMPTY;
	  }

	  /// <summary>
	  /// Returns an empty mutable builder for building up a set of requirements.
	  /// </summary>
	  /// <returns> an empty mutable builder for building up a set of requirements </returns>
	  public static MarketDataRequirementsBuilder builder()
	  {
		return new MarketDataRequirementsBuilder();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Merges multiple sets of requirements into a single set.
	  /// </summary>
	  /// <param name="requirements">  market data requirements </param>
	  /// <returns> a single set of requirements containing all the requirements from the input sets </returns>
	  public static MarketDataRequirements combine(IList<MarketDataRequirements> requirements)
	  {
		ImmutableSet.Builder<ObservableId> observablesBuilder = ImmutableSet.builder();
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.google.common.collect.ImmutableSet.Builder<com.opengamma.strata.data.MarketDataId<?>> nonObservablesBuilder = com.google.common.collect.ImmutableSet.builder();
		ImmutableSet.Builder<MarketDataId<object>> nonObservablesBuilder = ImmutableSet.builder();
		ImmutableSet.Builder<ObservableId> timeSeriesBuilder = ImmutableSet.builder();
		ImmutableSet.Builder<Currency> outputCurrenciesBuilder = ImmutableSet.builder();

		foreach (MarketDataRequirements req in requirements)
		{
		  observablesBuilder.addAll(req.observables);
		  nonObservablesBuilder.addAll(req.nonObservables);
		  timeSeriesBuilder.addAll(req.timeSeries);
		  outputCurrenciesBuilder.addAll(req.outputCurrencies);
		}
		return new MarketDataRequirements(observablesBuilder.build(), nonObservablesBuilder.build(), timeSeriesBuilder.build(), outputCurrenciesBuilder.build());
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code MarketDataRequirements}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static MarketDataRequirements.Meta meta()
	  {
		return MarketDataRequirements.Meta.INSTANCE;
	  }

	  static MarketDataRequirements()
	  {
		MetaBean.register(MarketDataRequirements.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="observables">  the value of the property, not null </param>
	  /// <param name="nonObservables">  the value of the property, not null </param>
	  /// <param name="timeSeries">  the value of the property, not null </param>
	  /// <param name="outputCurrencies">  the value of the property, not null </param>
	  internal MarketDataRequirements<T1, T2, T3>(ISet<T1> observables, ISet<T2> nonObservables, ISet<T3> timeSeries, ISet<Currency> outputCurrencies) where T1 : com.opengamma.strata.data.ObservableId where T2 : com.opengamma.strata.data.MarketDataId<T2> where T3 : com.opengamma.strata.data.ObservableId
	  {
		JodaBeanUtils.notNull(observables, "observables");
		JodaBeanUtils.notNull(nonObservables, "nonObservables");
		JodaBeanUtils.notNull(timeSeries, "timeSeries");
		JodaBeanUtils.notNull(outputCurrencies, "outputCurrencies");
		this.observables = ImmutableSet.copyOf(observables);
		this.nonObservables = ImmutableSet.copyOf(nonObservables);
		this.timeSeries = ImmutableSet.copyOf(timeSeries);
		this.outputCurrencies = ImmutableSet.copyOf(outputCurrencies);
	  }

	  public override MarketDataRequirements.Meta metaBean()
	  {
		return MarketDataRequirements.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets keys identifying the market data values required for the calculations. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableSet<ObservableId> Observables
	  {
		  get
		  {
			return observables;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets keys identifying the market data values required for the calculations. </summary>
	  /// <returns> the value of the property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.google.common.collect.ImmutableSet<com.opengamma.strata.data.MarketDataId<?>> getNonObservables()
	  public ImmutableSet<MarketDataId<object>> NonObservables
	  {
		  get
		  {
			return nonObservables;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets keys identifying the time series of market data values required for the calculations. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableSet<ObservableId> TimeSeries
	  {
		  get
		  {
			return timeSeries;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currencies in the calculation results. The market data must include FX rates in the
	  /// to allow conversion into the reporting currency. The FX rates must have the output currency as the base
	  /// currency and the reporting currency as the counter currency. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableSet<Currency> OutputCurrencies
	  {
		  get
		  {
			return outputCurrencies;
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
		  MarketDataRequirements other = (MarketDataRequirements) obj;
		  return JodaBeanUtils.equal(observables, other.observables) && JodaBeanUtils.equal(nonObservables, other.nonObservables) && JodaBeanUtils.equal(timeSeries, other.timeSeries) && JodaBeanUtils.equal(outputCurrencies, other.outputCurrencies);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(observables);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(nonObservables);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(timeSeries);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(outputCurrencies);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("MarketDataRequirements{");
		buf.Append("observables").Append('=').Append(observables).Append(',').Append(' ');
		buf.Append("nonObservables").Append('=').Append(nonObservables).Append(',').Append(' ');
		buf.Append("timeSeries").Append('=').Append(timeSeries).Append(',').Append(' ');
		buf.Append("outputCurrencies").Append('=').Append(JodaBeanUtils.ToString(outputCurrencies));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code MarketDataRequirements}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  observables_Renamed = DirectMetaProperty.ofImmutable(this, "observables", typeof(MarketDataRequirements), (Type) typeof(ImmutableSet));
			  nonObservables_Renamed = DirectMetaProperty.ofImmutable(this, "nonObservables", typeof(MarketDataRequirements), (Type) typeof(ImmutableSet));
			  timeSeries_Renamed = DirectMetaProperty.ofImmutable(this, "timeSeries", typeof(MarketDataRequirements), (Type) typeof(ImmutableSet));
			  outputCurrencies_Renamed = DirectMetaProperty.ofImmutable(this, "outputCurrencies", typeof(MarketDataRequirements), (Type) typeof(ImmutableSet));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "observables", "nonObservables", "timeSeries", "outputCurrencies");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code observables} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableSet<com.opengamma.strata.data.ObservableId>> observables = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "observables", MarketDataRequirements.class, (Class) com.google.common.collect.ImmutableSet.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableSet<ObservableId>> observables_Renamed;
		/// <summary>
		/// The meta-property for the {@code nonObservables} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableSet<com.opengamma.strata.data.MarketDataId<?>>> nonObservables = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "nonObservables", MarketDataRequirements.class, (Class) com.google.common.collect.ImmutableSet.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		internal MetaProperty<ImmutableSet<MarketDataId<object>>> nonObservables_Renamed;
		/// <summary>
		/// The meta-property for the {@code timeSeries} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableSet<com.opengamma.strata.data.ObservableId>> timeSeries = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "timeSeries", MarketDataRequirements.class, (Class) com.google.common.collect.ImmutableSet.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableSet<ObservableId>> timeSeries_Renamed;
		/// <summary>
		/// The meta-property for the {@code outputCurrencies} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableSet<com.opengamma.strata.basics.currency.Currency>> outputCurrencies = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "outputCurrencies", MarketDataRequirements.class, (Class) com.google.common.collect.ImmutableSet.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableSet<Currency>> outputCurrencies_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "observables", "nonObservables", "timeSeries", "outputCurrencies");
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
			case 121811856: // observables
			  return observables_Renamed;
			case 824041091: // nonObservables
			  return nonObservables_Renamed;
			case 779431844: // timeSeries
			  return timeSeries_Renamed;
			case -1022597040: // outputCurrencies
			  return outputCurrencies_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends MarketDataRequirements> builder()
		public override BeanBuilder<MarketDataRequirements> builder()
		{
		  return new MarketDataRequirements.Builder();
		}

		public override Type beanType()
		{
		  return typeof(MarketDataRequirements);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code observables} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableSet<ObservableId>> observables()
		{
		  return observables_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code nonObservables} property. </summary>
		/// <returns> the meta-property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public org.joda.beans.MetaProperty<com.google.common.collect.ImmutableSet<com.opengamma.strata.data.MarketDataId<?>>> nonObservables()
		public MetaProperty<ImmutableSet<MarketDataId<object>>> nonObservables()
		{
		  return nonObservables_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code timeSeries} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableSet<ObservableId>> timeSeries()
		{
		  return timeSeries_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code outputCurrencies} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableSet<Currency>> outputCurrencies()
		{
		  return outputCurrencies_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 121811856: // observables
			  return ((MarketDataRequirements) bean).Observables;
			case 824041091: // nonObservables
			  return ((MarketDataRequirements) bean).NonObservables;
			case 779431844: // timeSeries
			  return ((MarketDataRequirements) bean).TimeSeries;
			case -1022597040: // outputCurrencies
			  return ((MarketDataRequirements) bean).OutputCurrencies;
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
	  /// The bean-builder for {@code MarketDataRequirements}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<MarketDataRequirements>
	  {

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.Set<? extends com.opengamma.strata.data.ObservableId> observables = com.google.common.collect.ImmutableSet.of();
		internal ISet<ObservableId> observables = ImmutableSet.of();
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.Set<? extends com.opengamma.strata.data.MarketDataId<?>> nonObservables = com.google.common.collect.ImmutableSet.of();
		internal ISet<MarketDataId<object>> nonObservables = ImmutableSet.of();
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.Set<? extends com.opengamma.strata.data.ObservableId> timeSeries = com.google.common.collect.ImmutableSet.of();
		internal ISet<ObservableId> timeSeries = ImmutableSet.of();
		internal ISet<Currency> outputCurrencies = ImmutableSet.of();

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
			case 121811856: // observables
			  return observables;
			case 824041091: // nonObservables
			  return nonObservables;
			case 779431844: // timeSeries
			  return timeSeries;
			case -1022597040: // outputCurrencies
			  return outputCurrencies;
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
			case 121811856: // observables
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.observables = (java.util.Set<? extends com.opengamma.strata.data.ObservableId>) newValue;
			  this.observables = (ISet<ObservableId>) newValue;
			  break;
			case 824041091: // nonObservables
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.nonObservables = (java.util.Set<? extends com.opengamma.strata.data.MarketDataId<?>>) newValue;
			  this.nonObservables = (ISet<MarketDataId<object>>) newValue;
			  break;
			case 779431844: // timeSeries
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.timeSeries = (java.util.Set<? extends com.opengamma.strata.data.ObservableId>) newValue;
			  this.timeSeries = (ISet<ObservableId>) newValue;
			  break;
			case -1022597040: // outputCurrencies
			  this.outputCurrencies = (ISet<Currency>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override MarketDataRequirements build()
		{
		  return new MarketDataRequirements(observables, nonObservables, timeSeries, outputCurrencies);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("MarketDataRequirements.Builder{");
		  buf.Append("observables").Append('=').Append(JodaBeanUtils.ToString(observables)).Append(',').Append(' ');
		  buf.Append("nonObservables").Append('=').Append(JodaBeanUtils.ToString(nonObservables)).Append(',').Append(' ');
		  buf.Append("timeSeries").Append('=').Append(JodaBeanUtils.ToString(timeSeries)).Append(',').Append(' ');
		  buf.Append("outputCurrencies").Append('=').Append(JodaBeanUtils.ToString(outputCurrencies));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}