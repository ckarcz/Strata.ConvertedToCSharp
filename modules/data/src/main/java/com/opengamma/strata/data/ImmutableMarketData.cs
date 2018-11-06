using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data
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
	using MapStream = com.opengamma.strata.collect.MapStream;
	using Messages = com.opengamma.strata.collect.Messages;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;

	/// <summary>
	/// An immutable set of market data
	/// <para>
	/// This is the standard immutable implementation of <seealso cref="MarketData"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private", constructorScope = "package") public final class ImmutableMarketData implements MarketData, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ImmutableMarketData : MarketData, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.LocalDate valuationDate;
		private readonly LocalDate valuationDate;
	  /// <summary>
	  /// The market data values.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", builderType = "Map<? extends MarketDataId<?>, ?>") private final com.google.common.collect.ImmutableMap<MarketDataId<?>, Object> values;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  private readonly ImmutableMap<MarketDataId<object>, object> values;
	  /// <summary>
	  /// The time-series.
	  /// <para>
	  /// If a request is made for a time-series that is not in the map, an empty series will be returned.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<ObservableId, com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries> timeSeries;
	  private readonly ImmutableMap<ObservableId, LocalDateDoubleTimeSeries> timeSeries;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a valuation date and map of values.
	  /// <para>
	  /// Use the <seealso cref="#builder(LocalDate) builder"/> for more more complex use cases,
	  /// including setting time-series.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date associated with the market data </param>
	  /// <param name="values">  the market data values </param>
	  /// <returns> a set of market data containing the values in the map </returns>
	  /// <exception cref="ClassCastException"> if a value does not match the parameterized type associated with the identifier </exception>
	  public static ImmutableMarketData of<T1>(LocalDate valuationDate, IDictionary<T1> values) where T1 : MarketDataId<T1>
	  {
		MapStream.of(values).forEach((id, value) => checkType(id, value));
		return new ImmutableMarketData(valuationDate, values, ImmutableMap.of());
	  }

	  // checks the value is an instance of the market data type of the id
	  internal static void checkType<T1>(MarketDataId<T1> id, object value)
	  {
		if (!id.MarketDataType.IsInstanceOfType(value))
		{
		  if (value == null)
		  {
			throw new System.ArgumentException(Messages.format("Value for identifier '{}' must not be null", id));
		  }
		  throw new System.InvalidCastException(Messages.format("Value for identifier '{}' does not implement expected type '{}': '{}'", id, id.MarketDataType.Name, value));
		}
	  }

	  /// <summary>
	  /// Creates a builder that can be used to build an instance of {@code MarketData}.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <returns> the builder, not null </returns>
	  public static ImmutableMarketDataBuilder builder(LocalDate valuationDate)
	  {
		return new ImmutableMarketDataBuilder(valuationDate);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a builder populated with the same data as this instance.
	  /// </summary>
	  /// <returns> the mutable builder, not null </returns>
	  public ImmutableMarketDataBuilder toBuilder()
	  {
		return new ImmutableMarketDataBuilder(valuationDate, values, timeSeries);
	  }

	  //-------------------------------------------------------------------------
	  public override bool containsValue<T1>(MarketDataId<T1> id)
	  {
		// overridden for performance
		return values.containsKey(id);
	  }

	  /// <summary>
	  /// Combines this set of market data with another.
	  /// <para>
	  /// The result combines both sets of market data.
	  /// Values are taken from this set of market data if available, otherwise they are taken
	  /// from the other set.
	  /// </para>
	  /// <para>
	  /// The valuation dates of the sets of market data must be the same.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other market data </param>
	  /// <returns> the combined market data </returns>
	  public ImmutableMarketData combinedWith(ImmutableMarketData other)
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<MarketDataId<?>, Object> combinedValues = new java.util.HashMap<>(other.values);
		IDictionary<MarketDataId<object>, object> combinedValues = new Dictionary<MarketDataId<object>, object>(other.values);
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		combinedValues.putAll(values);
		Dictionary<ObservableId, LocalDateDoubleTimeSeries> combinedTimeSeries = new Dictionary<ObservableId, LocalDateDoubleTimeSeries>(other.timeSeries);
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		combinedTimeSeries.putAll(timeSeries);

		if (!valuationDate.Equals(other.valuationDate))
		{
		  throw new System.ArgumentException("Unable to combine market data instances with different valuation dates");
		}
		return new ImmutableMarketData(valuationDate, combinedValues, combinedTimeSeries);
	  }

	  public override MarketData combinedWith(MarketData other)
	  {
		if (!(other is ImmutableMarketData))
		{
		  return MarketData.this.combinedWith(other);
		}
		else
		{
		  return combinedWith((ImmutableMarketData) other);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public <T> T getValue(MarketDataId<T> id)
	  public override T getValue<T>(MarketDataId<T> id)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") T value = (T) values.get(id);
		  T value = (T) values.get(id);
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
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public <T> java.util.Optional<T> findValue(MarketDataId<T> id)
	  public Optional<T> findValue<T>(MarketDataId<T> id)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") T value = (T) values.get(id);
		  T value = (T) values.get(id);
		return Optional.ofNullable(value);
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Set<MarketDataId<?>> getIds()
	  public ISet<MarketDataId<object>> Ids
	  {
		  get
		  {
			return values.Keys;
		  }
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <T> java.util.Set<MarketDataId<T>> findIds(MarketDataName<T> name)
	  public ISet<MarketDataId<T>> findIds<T>(MarketDataName<T> name)
	  {
		// no type check against id.getMarketDataType() as checked in factory
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return values.keySet().stream().filter(id -> id instanceof NamedMarketDataId).filter(id -> ((NamedMarketDataId<?>) id).getMarketDataName().equals(name)).map(id -> (MarketDataId<T>) id).collect(toImmutableSet());
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

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableMarketData}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ImmutableMarketData.Meta meta()
	  {
		return ImmutableMarketData.Meta.INSTANCE;
	  }

	  static ImmutableMarketData()
	  {
		MetaBean.register(ImmutableMarketData.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="valuationDate">  the value of the property, not null </param>
	  /// <param name="values">  the value of the property, not null </param>
	  /// <param name="timeSeries">  the value of the property, not null </param>
	  internal ImmutableMarketData<T1>(LocalDate valuationDate, IDictionary<T1> values, IDictionary<ObservableId, LocalDateDoubleTimeSeries> timeSeries) where T1 : MarketDataId<T1>
	  {
		JodaBeanUtils.notNull(valuationDate, "valuationDate");
		JodaBeanUtils.notNull(values, "values");
		JodaBeanUtils.notNull(timeSeries, "timeSeries");
		this.valuationDate = valuationDate;
		this.values = ImmutableMap.copyOf(values);
		this.timeSeries = ImmutableMap.copyOf(timeSeries);
	  }

	  public override ImmutableMarketData.Meta metaBean()
	  {
		return ImmutableMarketData.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the valuation date associated with the market data. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate ValuationDate
	  {
		  get
		  {
			return valuationDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market data values. </summary>
	  /// <returns> the value of the property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.google.common.collect.ImmutableMap<MarketDataId<?>, Object> getValues()
	  public ImmutableMap<MarketDataId<object>, object> Values
	  {
		  get
		  {
			return values;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the time-series.
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
		  ImmutableMarketData other = (ImmutableMarketData) obj;
		  return JodaBeanUtils.equal(valuationDate, other.valuationDate) && JodaBeanUtils.equal(values, other.values) && JodaBeanUtils.equal(timeSeries, other.timeSeries);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(valuationDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(values);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(timeSeries);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("ImmutableMarketData{");
		buf.Append("valuationDate").Append('=').Append(valuationDate).Append(',').Append(' ');
		buf.Append("values").Append('=').Append(values).Append(',').Append(' ');
		buf.Append("timeSeries").Append('=').Append(JodaBeanUtils.ToString(timeSeries));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableMarketData}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  valuationDate_Renamed = DirectMetaProperty.ofImmutable(this, "valuationDate", typeof(ImmutableMarketData), typeof(LocalDate));
			  values_Renamed = DirectMetaProperty.ofImmutable(this, "values", typeof(ImmutableMarketData), (Type) typeof(ImmutableMap));
			  timeSeries_Renamed = DirectMetaProperty.ofImmutable(this, "timeSeries", typeof(ImmutableMarketData), (Type) typeof(ImmutableMap));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "valuationDate", "values", "timeSeries");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code valuationDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> valuationDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code values} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<MarketDataId<?>, Object>> values = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "values", ImmutableMarketData.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		internal MetaProperty<ImmutableMap<MarketDataId<object>, object>> values_Renamed;
		/// <summary>
		/// The meta-property for the {@code timeSeries} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<ObservableId, com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries>> timeSeries = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "timeSeries", ImmutableMarketData.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<ObservableId, LocalDateDoubleTimeSeries>> timeSeries_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "valuationDate", "values", "timeSeries");
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
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ImmutableMarketData> builder()
		public override BeanBuilder<ImmutableMarketData> builder()
		{
		  return new ImmutableMarketData.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ImmutableMarketData);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code valuationDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> valuationDate()
		{
		  return valuationDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code values} property. </summary>
		/// <returns> the meta-property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<MarketDataId<?>, Object>> values()
		public MetaProperty<ImmutableMap<MarketDataId<object>, object>> values()
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
			case 113107279: // valuationDate
			  return ((ImmutableMarketData) bean).ValuationDate;
			case -823812830: // values
			  return ((ImmutableMarketData) bean).Values;
			case 779431844: // timeSeries
			  return ((ImmutableMarketData) bean).TimeSeries;
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
	  /// The bean-builder for {@code ImmutableMarketData}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<ImmutableMarketData>
	  {

		internal LocalDate valuationDate;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.Map<? extends MarketDataId<?>, ?> values = com.google.common.collect.ImmutableMap.of();
		internal IDictionary<MarketDataId<object>, ?> values = ImmutableMap.of();
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
			case 113107279: // valuationDate
			  this.valuationDate = (LocalDate) newValue;
			  break;
			case -823812830: // values
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.values = (java.util.Map<? extends MarketDataId<?>, ?>) newValue;
			  this.values = (IDictionary<MarketDataId<object>, ?>) newValue;
			  break;
			case 779431844: // timeSeries
			  this.timeSeries = (IDictionary<ObservableId, LocalDateDoubleTimeSeries>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override ImmutableMarketData build()
		{
		  return new ImmutableMarketData(valuationDate, values, timeSeries);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("ImmutableMarketData.Builder{");
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