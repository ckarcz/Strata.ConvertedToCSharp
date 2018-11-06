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

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using Messages = com.opengamma.strata.collect.Messages;
	using Failure = com.opengamma.strata.collect.result.Failure;
	using FailureException = com.opengamma.strata.collect.result.FailureException;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using FxRateId = com.opengamma.strata.data.FxRateId;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using ImmutableScenarioMarketData = com.opengamma.strata.data.scenario.ImmutableScenarioMarketData;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// Market data that has been built.
	/// <para>
	/// The <seealso cref="MarketDataFactory"/> can be used to build market data from external
	/// sources and by calibration. This implementation of <seealso cref="ScenarioMarketData"/>
	/// provides the result, and includes all the market data, such as quotes and curves.
	/// </para>
	/// <para>
	/// This implementation differs from <seealso cref="ImmutableScenarioMarketData"/> because it
	/// stores the failures that occurred during the build process.
	/// These errors are exposed to users when data is queried.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private", constructorScope = "package") public final class BuiltScenarioMarketData implements com.opengamma.strata.data.scenario.ScenarioMarketData, org.joda.beans.ImmutableBean
	public sealed class BuiltScenarioMarketData : ScenarioMarketData, ImmutableBean
	{

	  /// <summary>
	  /// An instance containing no market data. </summary>
	  private static readonly BuiltScenarioMarketData EMPTY = new BuiltScenarioMarketData(ImmutableScenarioMarketData.empty(), ImmutableMap.of(), ImmutableMap.of());

	  /// <summary>
	  /// The underlying market data.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.data.scenario.ImmutableScenarioMarketData underlying;
	  private readonly ImmutableScenarioMarketData underlying;
	  /// <summary>
	  /// The failures when building single market data values.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", builderType = "Map<? extends MarketDataId<?>, Failure>") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.data.MarketDataId<?>, com.opengamma.strata.collect.result.Failure> valueFailures;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  private readonly ImmutableMap<MarketDataId<object>, Failure> valueFailures;
	  /// <summary>
	  /// The failures that occurred when building time series of market data values.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", builderType = "Map<? extends MarketDataId<?>, Failure>") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.data.MarketDataId<?>, com.opengamma.strata.collect.result.Failure> timeSeriesFailures;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  private readonly ImmutableMap<MarketDataId<object>, Failure> timeSeriesFailures;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a mutable builder that can be used to create an instance of the market data.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date associated with the market data </param>
	  /// <returns> the mutable builder </returns>
	  internal static BuiltScenarioMarketDataBuilder builder(LocalDate valuationDate)
	  {
		return new BuiltScenarioMarketDataBuilder(valuationDate);
	  }

	  /// <summary>
	  /// Creates a mutable builder that can be used to create an instance of the market data.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation dates associated with the market data, one for each scenario </param>
	  /// <returns> the mutable builder </returns>
	  internal static BuiltScenarioMarketDataBuilder builder(MarketDataBox<LocalDate> valuationDate)
	  {
		return new BuiltScenarioMarketDataBuilder(valuationDate);
	  }

	  /// <summary>
	  /// Returns an empty set of market data.
	  /// </summary>
	  /// <returns> an empty set of market data </returns>
	  internal static BuiltScenarioMarketData empty()
	  {
		return EMPTY;
	  }

	  //-------------------------------------------------------------------------
	  public MarketDataBox<LocalDate> ValuationDate
	  {
		  get
		  {
			return underlying.ValuationDate;
		  }
	  }

	  public int ScenarioCount
	  {
		  get
		  {
			return underlying.ScenarioCount;
		  }
	  }

	  public override bool containsValue<T1>(MarketDataId<T1> id)
	  {
		return underlying.containsValue(id);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public <T> com.opengamma.strata.data.scenario.MarketDataBox<T> getValue(com.opengamma.strata.data.MarketDataId<T> id)
	  public override MarketDataBox<T> getValue<T>(MarketDataId<T> id)
	  {
		// this code exists to ensure that the error messages from market data building
		// are exposed to users when the failures are not checked

		// a special case for FX rates containing the same currency twice
		if (id is FxRateId && ((FxRateId) id).Pair.Identity)
		{
		  FxRateId fxRateId = (FxRateId) id;
		  FxRate identityRate = FxRate.of(fxRateId.Pair, 1);
		  return MarketDataBox.ofSingleValue((T) identityRate);
		}

		// find the data and check it against the failures
		Optional<MarketDataBox<T>> opt = underlying.findValue(id);
		if (!opt.Present)
		{
		  Failure failure = valueFailures.get(id);
		  if (failure != null)
		  {
			throw new FailureException(failure);
		  }
		  throw new MarketDataNotFoundException(Messages.format("Market data not found for identifier '{}' of type '{}'", id, id.GetType().Name));
		}
		return opt.get();
	  }

	  public Optional<MarketDataBox<T>> findValue<T>(MarketDataId<T> id)
	  {
		return underlying.findValue(id);
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Set<com.opengamma.strata.data.MarketDataId<?>> getIds()
	  public ISet<MarketDataId<object>> Ids
	  {
		  get
		  {
			return underlying.Ids;
		  }
	  }

	  public ISet<MarketDataId<T>> findIds<T>(MarketDataName<T> name)
	  {
		return underlying.findIds(name);
	  }

	  public ISet<ObservableId> TimeSeriesIds
	  {
		  get
		  {
			return underlying.TimeSeriesIds;
		  }
	  }

	  public LocalDateDoubleTimeSeries getTimeSeries(ObservableId id)
	  {
		return underlying.getTimeSeries(id);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code BuiltScenarioMarketData}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static BuiltScenarioMarketData.Meta meta()
	  {
		return BuiltScenarioMarketData.Meta.INSTANCE;
	  }

	  static BuiltScenarioMarketData()
	  {
		MetaBean.register(BuiltScenarioMarketData.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="underlying">  the value of the property, not null </param>
	  /// <param name="valueFailures">  the value of the property, not null </param>
	  /// <param name="timeSeriesFailures">  the value of the property, not null </param>
	  internal BuiltScenarioMarketData<T1, T2>(ImmutableScenarioMarketData underlying, IDictionary<T1> valueFailures, IDictionary<T2> timeSeriesFailures) where T1 : com.opengamma.strata.data.MarketDataId<T1> where T2 : com.opengamma.strata.data.MarketDataId<T2>
	  {
		JodaBeanUtils.notNull(underlying, "underlying");
		JodaBeanUtils.notNull(valueFailures, "valueFailures");
		JodaBeanUtils.notNull(timeSeriesFailures, "timeSeriesFailures");
		this.underlying = underlying;
		this.valueFailures = ImmutableMap.copyOf(valueFailures);
		this.timeSeriesFailures = ImmutableMap.copyOf(timeSeriesFailures);
	  }

	  public override BuiltScenarioMarketData.Meta metaBean()
	  {
		return BuiltScenarioMarketData.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying market data. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableScenarioMarketData Underlying
	  {
		  get
		  {
			return underlying;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the failures when building single market data values. </summary>
	  /// <returns> the value of the property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.google.common.collect.ImmutableMap<com.opengamma.strata.data.MarketDataId<?>, com.opengamma.strata.collect.result.Failure> getValueFailures()
	  public ImmutableMap<MarketDataId<object>, Failure> ValueFailures
	  {
		  get
		  {
			return valueFailures;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the failures that occurred when building time series of market data values. </summary>
	  /// <returns> the value of the property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.google.common.collect.ImmutableMap<com.opengamma.strata.data.MarketDataId<?>, com.opengamma.strata.collect.result.Failure> getTimeSeriesFailures()
	  public ImmutableMap<MarketDataId<object>, Failure> TimeSeriesFailures
	  {
		  get
		  {
			return timeSeriesFailures;
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
		  BuiltScenarioMarketData other = (BuiltScenarioMarketData) obj;
		  return JodaBeanUtils.equal(underlying, other.underlying) && JodaBeanUtils.equal(valueFailures, other.valueFailures) && JodaBeanUtils.equal(timeSeriesFailures, other.timeSeriesFailures);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(underlying);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(valueFailures);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(timeSeriesFailures);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("BuiltScenarioMarketData{");
		buf.Append("underlying").Append('=').Append(underlying).Append(',').Append(' ');
		buf.Append("valueFailures").Append('=').Append(valueFailures).Append(',').Append(' ');
		buf.Append("timeSeriesFailures").Append('=').Append(JodaBeanUtils.ToString(timeSeriesFailures));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code BuiltScenarioMarketData}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  underlying_Renamed = DirectMetaProperty.ofImmutable(this, "underlying", typeof(BuiltScenarioMarketData), typeof(ImmutableScenarioMarketData));
			  valueFailures_Renamed = DirectMetaProperty.ofImmutable(this, "valueFailures", typeof(BuiltScenarioMarketData), (Type) typeof(ImmutableMap));
			  timeSeriesFailures_Renamed = DirectMetaProperty.ofImmutable(this, "timeSeriesFailures", typeof(BuiltScenarioMarketData), (Type) typeof(ImmutableMap));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "underlying", "valueFailures", "timeSeriesFailures");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code underlying} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableScenarioMarketData> underlying_Renamed;
		/// <summary>
		/// The meta-property for the {@code valueFailures} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.data.MarketDataId<?>, com.opengamma.strata.collect.result.Failure>> valueFailures = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "valueFailures", BuiltScenarioMarketData.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		internal MetaProperty<ImmutableMap<MarketDataId<object>, Failure>> valueFailures_Renamed;
		/// <summary>
		/// The meta-property for the {@code timeSeriesFailures} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.data.MarketDataId<?>, com.opengamma.strata.collect.result.Failure>> timeSeriesFailures = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "timeSeriesFailures", BuiltScenarioMarketData.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		internal MetaProperty<ImmutableMap<MarketDataId<object>, Failure>> timeSeriesFailures_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "underlying", "valueFailures", "timeSeriesFailures");
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
			case -1770633379: // underlying
			  return underlying_Renamed;
			case -68881222: // valueFailures
			  return valueFailures_Renamed;
			case -1580093459: // timeSeriesFailures
			  return timeSeriesFailures_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends BuiltScenarioMarketData> builder()
		public override BeanBuilder<BuiltScenarioMarketData> builder()
		{
		  return new BuiltScenarioMarketData.Builder();
		}

		public override Type beanType()
		{
		  return typeof(BuiltScenarioMarketData);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code underlying} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableScenarioMarketData> underlying()
		{
		  return underlying_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code valueFailures} property. </summary>
		/// <returns> the meta-property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.data.MarketDataId<?>, com.opengamma.strata.collect.result.Failure>> valueFailures()
		public MetaProperty<ImmutableMap<MarketDataId<object>, Failure>> valueFailures()
		{
		  return valueFailures_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code timeSeriesFailures} property. </summary>
		/// <returns> the meta-property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.data.MarketDataId<?>, com.opengamma.strata.collect.result.Failure>> timeSeriesFailures()
		public MetaProperty<ImmutableMap<MarketDataId<object>, Failure>> timeSeriesFailures()
		{
		  return timeSeriesFailures_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1770633379: // underlying
			  return ((BuiltScenarioMarketData) bean).Underlying;
			case -68881222: // valueFailures
			  return ((BuiltScenarioMarketData) bean).ValueFailures;
			case -1580093459: // timeSeriesFailures
			  return ((BuiltScenarioMarketData) bean).TimeSeriesFailures;
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
	  /// The bean-builder for {@code BuiltScenarioMarketData}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<BuiltScenarioMarketData>
	  {

		internal ImmutableScenarioMarketData underlying;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.Map<? extends com.opengamma.strata.data.MarketDataId<?>, com.opengamma.strata.collect.result.Failure> valueFailures = com.google.common.collect.ImmutableMap.of();
		internal IDictionary<MarketDataId<object>, Failure> valueFailures = ImmutableMap.of();
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.Map<? extends com.opengamma.strata.data.MarketDataId<?>, com.opengamma.strata.collect.result.Failure> timeSeriesFailures = com.google.common.collect.ImmutableMap.of();
		internal IDictionary<MarketDataId<object>, Failure> timeSeriesFailures = ImmutableMap.of();

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
			case -1770633379: // underlying
			  return underlying;
			case -68881222: // valueFailures
			  return valueFailures;
			case -1580093459: // timeSeriesFailures
			  return timeSeriesFailures;
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
			case -1770633379: // underlying
			  this.underlying = (ImmutableScenarioMarketData) newValue;
			  break;
			case -68881222: // valueFailures
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.valueFailures = (java.util.Map<? extends com.opengamma.strata.data.MarketDataId<?>, com.opengamma.strata.collect.result.Failure>) newValue;
			  this.valueFailures = (IDictionary<MarketDataId<object>, Failure>) newValue;
			  break;
			case -1580093459: // timeSeriesFailures
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.timeSeriesFailures = (java.util.Map<? extends com.opengamma.strata.data.MarketDataId<?>, com.opengamma.strata.collect.result.Failure>) newValue;
			  this.timeSeriesFailures = (IDictionary<MarketDataId<object>, Failure>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override BuiltScenarioMarketData build()
		{
		  return new BuiltScenarioMarketData(underlying, valueFailures, timeSeriesFailures);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("BuiltScenarioMarketData.Builder{");
		  buf.Append("underlying").Append('=').Append(JodaBeanUtils.ToString(underlying)).Append(',').Append(' ');
		  buf.Append("valueFailures").Append('=').Append(JodaBeanUtils.ToString(valueFailures)).Append(',').Append(' ');
		  buf.Append("timeSeriesFailures").Append('=').Append(JodaBeanUtils.ToString(timeSeriesFailures));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}