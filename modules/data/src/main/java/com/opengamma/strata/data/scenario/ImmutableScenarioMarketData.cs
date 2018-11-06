using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableSet;


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
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MapStream = com.opengamma.strata.collect.MapStream;
	using Messages = com.opengamma.strata.collect.Messages;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;

	/// <summary>
	/// An immutable set of market data across one or more scenarios.
	/// <para>
	/// This is the standard immutable implementation of <seealso cref="ScenarioMarketData"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private", constructorScope = "package") public final class ImmutableScenarioMarketData implements ScenarioMarketData, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ImmutableScenarioMarketData : ScenarioMarketData, ImmutableBean
	{

	  /// <summary>
	  /// An empty instance. </summary>
	  private static readonly ImmutableScenarioMarketData EMPTY = new ImmutableScenarioMarketData(0, MarketDataBox.empty(), ImmutableMap.of(), ImmutableMap.of());

	  /// <summary>
	  /// The number of scenarios.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative", overrideGet = true) private final int scenarioCount;
	  private readonly int scenarioCount;
	  /// <summary>
	  /// The valuation date associated with each scenario.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final MarketDataBox<java.time.LocalDate> valuationDate;
	  private readonly MarketDataBox<LocalDate> valuationDate;
	  /// <summary>
	  /// The individual items of market data.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", builderType = "Map<? extends MarketDataId<?>, MarketDataBox<?>>") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.data.MarketDataId<?>, MarketDataBox<?>> values;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  private readonly ImmutableMap<MarketDataId<object>, MarketDataBox<object>> values;
	  /// <summary>
	  /// The time-series of market data values.
	  /// <para>
	  /// If a request is made for a time-series that is not in the map, an empty series will be returned.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", builderType = "Map<? extends ObservableId, LocalDateDoubleTimeSeries>") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.data.ObservableId, com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries> timeSeries;
	  private readonly ImmutableMap<ObservableId, LocalDateDoubleTimeSeries> timeSeries;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a valuation date, map of values and time-series.
	  /// <para>
	  /// The valuation date and map of values must have the same number of scenarios.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="scenarioCount">  the number of scenarios </param>
	  /// <param name="valuationDate">  the valuation dates associated with all scenarios </param>
	  /// <param name="values">  the market data values, one for each scenario </param>
	  /// <param name="timeSeries">  the time-series </param>
	  /// <returns> a set of market data containing the values in the map </returns>
	  public static ImmutableScenarioMarketData of<T1, T2>(int scenarioCount, LocalDate valuationDate, IDictionary<T1> values, IDictionary<T2> timeSeries) where T1 : com.opengamma.strata.data.MarketDataId<T1> where T2 : com.opengamma.strata.data.ObservableId
	  {

		return of(scenarioCount, MarketDataBox.ofSingleValue(valuationDate), values, timeSeries);
	  }

	  /// <summary>
	  /// Obtains an instance from a valuation date, map of values and time-series.
	  /// <para>
	  /// The valuation date and map of values must have the same number of scenarios.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="scenarioCount">  the number of scenarios </param>
	  /// <param name="valuationDate">  the valuation dates associated with the market data, one for each scenario </param>
	  /// <param name="values">  the market data values, one for each scenario </param>
	  /// <param name="timeSeries">  the time-series </param>
	  /// <returns> a set of market data containing the values in the map </returns>
	  public static ImmutableScenarioMarketData of<T1, T2>(int scenarioCount, MarketDataBox<LocalDate> valuationDate, IDictionary<T1> values, IDictionary<T2> timeSeries) where T1 : com.opengamma.strata.data.MarketDataId<T1> where T2 : com.opengamma.strata.data.ObservableId
	  {

		MapStream.of(values).forEach((key, value) => checkType(key, value, scenarioCount));
		return new ImmutableScenarioMarketData(scenarioCount, valuationDate, values, timeSeries);
	  }

	  // checks the value is an instance of the market data type of the id
	  internal static void checkType<T1, T2>(MarketDataId<T1> id, MarketDataBox<T2> box, int scenarioCount)
	  {
		if (box == null)
		{
		  throw new System.ArgumentException(Messages.format("Value for identifier '{}' must not be null", id));
		}
		if (box.ScenarioValue && box.ScenarioCount != scenarioCount)
		{
		  throw new System.ArgumentException(Messages.format("Value for identifier '{}' should have had {} scenarios but had {}", id, scenarioCount, box.ScenarioCount));
		}
		if (box.ScenarioCount > 0 && !id.MarketDataType.IsInstanceOfType(box.getValue(0)))
		{
		  throw new System.InvalidCastException(Messages.format("Value for identifier '{}' does not implement expected type '{}': '{}'", id, id.MarketDataType.Name, box));
		}
	  }

	  /// <summary>
	  /// Obtains a market data instance that contains no data and has no scenarios.
	  /// </summary>
	  /// <returns> an empty instance </returns>
	  public static ImmutableScenarioMarketData empty()
	  {
		return EMPTY;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a mutable builder that can be used to create an instance of the market data.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date associated with the market data </param>
	  /// <returns> the mutable builder </returns>
	  public static ImmutableScenarioMarketDataBuilder builder(LocalDate valuationDate)
	  {
		return new ImmutableScenarioMarketDataBuilder(valuationDate);
	  }

	  /// <summary>
	  /// Creates a mutable builder that can be used to create an instance of the market data.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation dates associated with the market data, one for each scenario </param>
	  /// <returns> the mutable builder </returns>
	  public static ImmutableScenarioMarketDataBuilder builder(MarketDataBox<LocalDate> valuationDate)
	  {

		return new ImmutableScenarioMarketDataBuilder(valuationDate);
	  }

	  //-------------------------------------------------------------------------
	  public override bool containsValue<T1>(MarketDataId<T1> id)
	  {
		// overridden for performance
		return values.containsKey(id);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public <T> MarketDataBox<T> getValue(com.opengamma.strata.data.MarketDataId<T> id)
	  public override MarketDataBox<T> getValue<T>(MarketDataId<T> id)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") MarketDataBox<T> value = (MarketDataBox<T>) values.get(id);
		  MarketDataBox<T> value = (MarketDataBox<T>) values.get(id);
		if (value == null)
		{
		  throw new MarketDataNotFoundException(msgValueNotFound(id));
		}
		return value;
	  }

	  // extracted to aid inlining performance
	  private string msgValueNotFound<T1>(MarketDataId<T1> id)
	  {
		return Messages.format("Market data not found for identifier '{}' of type '{}'", id, id.GetType().Name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public <T> java.util.Optional<MarketDataBox<T>> findValue(com.opengamma.strata.data.MarketDataId<T> id)
	  public Optional<MarketDataBox<T>> findValue<T>(MarketDataId<T> id)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") MarketDataBox<T> value = (MarketDataBox<T>) values.get(id);
		  MarketDataBox<T> value = (MarketDataBox<T>) values.get(id);
		return Optional.ofNullable(value);
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Set<com.opengamma.strata.data.MarketDataId<?>> getIds()
	  public ISet<MarketDataId<object>> Ids
	  {
		  get
		  {
			return values.Keys;
		  }
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <T> java.util.Set<com.opengamma.strata.data.MarketDataId<T>> findIds(com.opengamma.strata.data.MarketDataName<T> name)
	  public ISet<MarketDataId<T>> findIds<T>(MarketDataName<T> name)
	  {
		// no type check against id.getMarketDataType() as checked in factory
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return values.keySet().stream().filter(id -> id instanceof com.opengamma.strata.data.NamedMarketDataId).filter(id -> ((com.opengamma.strata.data.NamedMarketDataId<?>) id).getMarketDataName().equals(name)).map(id -> (com.opengamma.strata.data.MarketDataId<T>) id).collect(toImmutableSet());
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return values.Keys.Where(id => id is NamedMarketDataId).Where(id => ((NamedMarketDataId<object>) id).MarketDataName.Equals(name)).Select(id => (MarketDataId<T>) id).collect(toImmutableSet());
	  }

	  public ISet<ObservableId> TimeSeriesIds
	  {
		  get
		  {
			return timeSeries.Keys;
		  }
	  }

	  public LocalDateDoubleTimeSeries getTimeSeries(ObservableId id)
	  {
		LocalDateDoubleTimeSeries found = timeSeries.get(id);
		return found == null ? LocalDateDoubleTimeSeries.empty() : found;
	  }

	  /// <summary>
	  /// Returns set of market data which combines the data from this set of data with another set.
	  /// <para>
	  /// If the same item of data is available in both sets, it will be taken from this set.
	  /// </para>
	  /// <para>
	  /// Both sets of data must contain the same number of scenarios, or one of them must have one scenario.
	  /// If one of the sets of data has one scenario, the combined set will have the scenario count
	  /// of the other set.
	  /// </para>
	  /// <para>
	  /// The valuation dates are taken from this set of data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  another set of market data </param>
	  /// <returns> a set of market data combining the data in this set with the data in the other </returns>
	  public ImmutableScenarioMarketData combinedWith(ImmutableScenarioMarketData other)
	  {
		if (scenarioCount != 1 && other.scenarioCount != 1 && scenarioCount != other.scenarioCount)
		{
		  throw new System.ArgumentException(Messages.format("When merging scenario market data, both sets of data must have the same number of scenarios or " + "at least one of them must have one scenario. Number of scenarios: {} and {}", scenarioCount, other.scenarioCount));
		}
		int mergedCount = Math.Max(scenarioCount, other.scenarioCount);
		// Use HashMap because it allows values to be overwritten. ImmutableMap builders throw an exception if a value
		// is added using a key which is already present
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, MarketDataBox<?>> values = new java.util.HashMap<>(other.values);
		IDictionary<MarketDataId<object>, MarketDataBox<object>> values = new Dictionary<MarketDataId<object>, MarketDataBox<object>>(other.values);
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		values.putAll(this.values);
		IDictionary<ObservableId, LocalDateDoubleTimeSeries> timeSeries = new Dictionary<ObservableId, LocalDateDoubleTimeSeries>(other.timeSeries);
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		timeSeries.putAll(this.timeSeries);
		return new ImmutableScenarioMarketData(mergedCount, valuationDate, values, timeSeries);
	  }

	  public override ScenarioMarketData combinedWith(ScenarioMarketData other)
	  {
		if (other is ImmutableScenarioMarketData)
		{
		  return combinedWith((ImmutableScenarioMarketData) other);
		}
		else
		{
		  return ScenarioMarketData.this.combinedWith(other);
		}
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableScenarioMarketData}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ImmutableScenarioMarketData.Meta meta()
	  {
		return ImmutableScenarioMarketData.Meta.INSTANCE;
	  }

	  static ImmutableScenarioMarketData()
	  {
		MetaBean.register(ImmutableScenarioMarketData.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="scenarioCount">  the value of the property </param>
	  /// <param name="valuationDate">  the value of the property, not null </param>
	  /// <param name="values">  the value of the property, not null </param>
	  /// <param name="timeSeries">  the value of the property, not null </param>
	  internal ImmutableScenarioMarketData<T1, T2>(int scenarioCount, MarketDataBox<LocalDate> valuationDate, IDictionary<T1> values, IDictionary<T2> timeSeries) where T1 : com.opengamma.strata.data.MarketDataId<T1> where T2 : com.opengamma.strata.data.ObservableId
	  {
		ArgChecker.notNegative(scenarioCount, "scenarioCount");
		JodaBeanUtils.notNull(valuationDate, "valuationDate");
		JodaBeanUtils.notNull(values, "values");
		JodaBeanUtils.notNull(timeSeries, "timeSeries");
		this.scenarioCount = scenarioCount;
		this.valuationDate = valuationDate;
		this.values = ImmutableMap.copyOf(values);
		this.timeSeries = ImmutableMap.copyOf(timeSeries);
	  }

	  public override ImmutableScenarioMarketData.Meta metaBean()
	  {
		return ImmutableScenarioMarketData.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of scenarios. </summary>
	  /// <returns> the value of the property </returns>
	  public int ScenarioCount
	  {
		  get
		  {
			return scenarioCount;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the valuation date associated with each scenario. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public MarketDataBox<LocalDate> ValuationDate
	  {
		  get
		  {
			return valuationDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the individual items of market data. </summary>
	  /// <returns> the value of the property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.google.common.collect.ImmutableMap<com.opengamma.strata.data.MarketDataId<?>, MarketDataBox<?>> getValues()
	  public ImmutableMap<MarketDataId<object>, MarketDataBox<object>> Values
	  {
		  get
		  {
			return values;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the time-series of market data values.
	  /// <para>
	  /// If a request is made for a time-series that is not in the map, an empty series will be returned.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<ObservableId, LocalDateDoubleTimeSeries> TimeSeries
	  {
		  get
		  {
			return timeSeries;
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
		  ImmutableScenarioMarketData other = (ImmutableScenarioMarketData) obj;
		  return (scenarioCount == other.scenarioCount) && JodaBeanUtils.equal(valuationDate, other.valuationDate) && JodaBeanUtils.equal(values, other.values) && JodaBeanUtils.equal(timeSeries, other.timeSeries);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(scenarioCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(valuationDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(values);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(timeSeries);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("ImmutableScenarioMarketData{");
		buf.Append("scenarioCount").Append('=').Append(scenarioCount).Append(',').Append(' ');
		buf.Append("valuationDate").Append('=').Append(valuationDate).Append(',').Append(' ');
		buf.Append("values").Append('=').Append(values).Append(',').Append(' ');
		buf.Append("timeSeries").Append('=').Append(JodaBeanUtils.ToString(timeSeries));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableScenarioMarketData}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  scenarioCount_Renamed = DirectMetaProperty.ofImmutable(this, "scenarioCount", typeof(ImmutableScenarioMarketData), Integer.TYPE);
			  valuationDate_Renamed = DirectMetaProperty.ofImmutable(this, "valuationDate", typeof(ImmutableScenarioMarketData), (Type) typeof(MarketDataBox));
			  values_Renamed = DirectMetaProperty.ofImmutable(this, "values", typeof(ImmutableScenarioMarketData), (Type) typeof(ImmutableMap));
			  timeSeries_Renamed = DirectMetaProperty.ofImmutable(this, "timeSeries", typeof(ImmutableScenarioMarketData), (Type) typeof(ImmutableMap));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "scenarioCount", "valuationDate", "values", "timeSeries");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code scenarioCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<int> scenarioCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code valuationDate} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<MarketDataBox<java.time.LocalDate>> valuationDate = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "valuationDate", ImmutableScenarioMarketData.class, (Class) MarketDataBox.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<MarketDataBox<LocalDate>> valuationDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code values} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.data.MarketDataId<?>, MarketDataBox<?>>> values = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "values", ImmutableScenarioMarketData.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		internal MetaProperty<ImmutableMap<MarketDataId<object>, MarketDataBox<object>>> values_Renamed;
		/// <summary>
		/// The meta-property for the {@code timeSeries} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.data.ObservableId, com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries>> timeSeries = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "timeSeries", ImmutableScenarioMarketData.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<ObservableId, LocalDateDoubleTimeSeries>> timeSeries_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "scenarioCount", "valuationDate", "values", "timeSeries");
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
			case -1203198113: // scenarioCount
			  return scenarioCount_Renamed;
			case 113107279: // valuationDate
			  return valuationDate_Renamed;
			case -823812830: // values
			  return values_Renamed;
			case 779431844: // timeSeries
			  return timeSeries_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ImmutableScenarioMarketData> builder()
		public override BeanBuilder<ImmutableScenarioMarketData> builder()
		{
		  return new ImmutableScenarioMarketData.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ImmutableScenarioMarketData);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code scenarioCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<int> scenarioCount()
		{
		  return scenarioCount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code valuationDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<MarketDataBox<LocalDate>> valuationDate()
		{
		  return valuationDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code values} property. </summary>
		/// <returns> the meta-property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.data.MarketDataId<?>, MarketDataBox<?>>> values()
		public MetaProperty<ImmutableMap<MarketDataId<object>, MarketDataBox<object>>> values()
		{
		  return values_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code timeSeries} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<ObservableId, LocalDateDoubleTimeSeries>> timeSeries()
		{
		  return timeSeries_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1203198113: // scenarioCount
			  return ((ImmutableScenarioMarketData) bean).ScenarioCount;
			case 113107279: // valuationDate
			  return ((ImmutableScenarioMarketData) bean).ValuationDate;
			case -823812830: // values
			  return ((ImmutableScenarioMarketData) bean).Values;
			case 779431844: // timeSeries
			  return ((ImmutableScenarioMarketData) bean).TimeSeries;
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
	  /// The bean-builder for {@code ImmutableScenarioMarketData}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<ImmutableScenarioMarketData>
	  {

		internal int scenarioCount;
		internal MarketDataBox<LocalDate> valuationDate;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.Map<? extends com.opengamma.strata.data.MarketDataId<?>, MarketDataBox<?>> values = com.google.common.collect.ImmutableMap.of();
		internal IDictionary<MarketDataId<object>, MarketDataBox<object>> values = ImmutableMap.of();
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.Map<? extends com.opengamma.strata.data.ObservableId, com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries> timeSeries = com.google.common.collect.ImmutableMap.of();
		internal IDictionary<ObservableId, LocalDateDoubleTimeSeries> timeSeries = ImmutableMap.of();

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
			case -1203198113: // scenarioCount
			  return scenarioCount;
			case 113107279: // valuationDate
			  return valuationDate;
			case -823812830: // values
			  return values;
			case 779431844: // timeSeries
			  return timeSeries;
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
			case -1203198113: // scenarioCount
			  this.scenarioCount = (int?) newValue.Value;
			  break;
			case 113107279: // valuationDate
			  this.valuationDate = (MarketDataBox<LocalDate>) newValue;
			  break;
			case -823812830: // values
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.values = (java.util.Map<? extends com.opengamma.strata.data.MarketDataId<?>, MarketDataBox<?>>) newValue;
			  this.values = (IDictionary<MarketDataId<object>, MarketDataBox<object>>) newValue;
			  break;
			case 779431844: // timeSeries
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.timeSeries = (java.util.Map<? extends com.opengamma.strata.data.ObservableId, com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries>) newValue;
			  this.timeSeries = (IDictionary<ObservableId, LocalDateDoubleTimeSeries>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override ImmutableScenarioMarketData build()
		{
		  return new ImmutableScenarioMarketData(scenarioCount, valuationDate, values, timeSeries);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("ImmutableScenarioMarketData.Builder{");
		  buf.Append("scenarioCount").Append('=').Append(JodaBeanUtils.ToString(scenarioCount)).Append(',').Append(' ');
		  buf.Append("valuationDate").Append('=').Append(JodaBeanUtils.ToString(valuationDate)).Append(',').Append(' ');
		  buf.Append("values").Append('=').Append(JodaBeanUtils.ToString(values)).Append(',').Append(' ');
		  buf.Append("timeSeries").Append('=').Append(JodaBeanUtils.ToString(timeSeries));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}