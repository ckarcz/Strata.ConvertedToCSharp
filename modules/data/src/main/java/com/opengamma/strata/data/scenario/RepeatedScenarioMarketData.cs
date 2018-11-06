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

	using Preconditions = com.google.common.@base.Preconditions;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;

	/// <summary>
	/// Market data across one or more scenarios based on a single set of market data.
	/// <para>
	/// This implementation of <seealso cref="ScenarioMarketData"/> returns the same <seealso cref="MarketData"/>
	/// instance for each scenario.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private", constructorScope = "package") final class RepeatedScenarioMarketData implements ScenarioMarketData, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	internal sealed class RepeatedScenarioMarketData : ScenarioMarketData, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegativeOrZero", overrideGet = true) private final int scenarioCount;
		private readonly int scenarioCount;
	  /// <summary>
	  /// The underlying market data.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.data.MarketData underlying;
	  private readonly MarketData underlying;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a valuation date, map of values and time-series.
	  /// <para>
	  /// The valuation date and map of values must have the same number of scenarios.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="scenarioCount">  the number of scenarios </param>
	  /// <param name="marketData">  the single set of market data </param>
	  /// <returns> the scenario market data </returns>
	  public static RepeatedScenarioMarketData of(int scenarioCount, MarketData marketData)
	  {
		return new RepeatedScenarioMarketData(scenarioCount, marketData);
	  }

	  //-------------------------------------------------------------------------
	  public MarketDataBox<LocalDate> ValuationDate
	  {
		  get
		  {
			return MarketDataBox.ofSingleValue(underlying.ValuationDate);
		  }
	  }

	  public override Stream<MarketData> scenarios()
	  {
		return IntStream.range(0, ScenarioCount).mapToObj(scenarioIndex => underlying);
	  }

	  public override MarketData scenario(int scenarioIndex)
	  {
		Preconditions.checkElementIndex(scenarioIndex, scenarioCount, "scenarioIndex");
		return underlying;
	  }

	  //-------------------------------------------------------------------------
	  public override bool containsValue<T1>(MarketDataId<T1> id)
	  {
		return underlying.containsValue(id);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public <T> MarketDataBox<T> getValue(com.opengamma.strata.data.MarketDataId<T> id)
	  public override MarketDataBox<T> getValue<T>(MarketDataId<T> id)
	  {
		return MarketDataBox.ofSingleValue(underlying.getValue(id));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public <T> java.util.Optional<MarketDataBox<T>> findValue(com.opengamma.strata.data.MarketDataId<T> id)
	  public Optional<MarketDataBox<T>> findValue<T>(MarketDataId<T> id)
	  {
		return underlying.findValue(id).map(v => MarketDataBox.ofSingleValue(v));
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

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <T> java.util.Set<com.opengamma.strata.data.MarketDataId<T>> findIds(com.opengamma.strata.data.MarketDataName<T> name)
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
	  /// The meta-bean for {@code RepeatedScenarioMarketData}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static RepeatedScenarioMarketData.Meta meta()
	  {
		return RepeatedScenarioMarketData.Meta.INSTANCE;
	  }

	  static RepeatedScenarioMarketData()
	  {
		MetaBean.register(RepeatedScenarioMarketData.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="scenarioCount">  the value of the property </param>
	  /// <param name="underlying">  the value of the property, not null </param>
	  internal RepeatedScenarioMarketData(int scenarioCount, MarketData underlying)
	  {
		ArgChecker.notNegativeOrZero(scenarioCount, "scenarioCount");
		JodaBeanUtils.notNull(underlying, "underlying");
		this.scenarioCount = scenarioCount;
		this.underlying = underlying;
	  }

	  public override RepeatedScenarioMarketData.Meta metaBean()
	  {
		return RepeatedScenarioMarketData.Meta.INSTANCE;
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
	  /// Gets the underlying market data. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public MarketData Underlying
	  {
		  get
		  {
			return underlying;
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
		  RepeatedScenarioMarketData other = (RepeatedScenarioMarketData) obj;
		  return (scenarioCount == other.scenarioCount) && JodaBeanUtils.equal(underlying, other.underlying);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(scenarioCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(underlying);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("RepeatedScenarioMarketData{");
		buf.Append("scenarioCount").Append('=').Append(scenarioCount).Append(',').Append(' ');
		buf.Append("underlying").Append('=').Append(JodaBeanUtils.ToString(underlying));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code RepeatedScenarioMarketData}.
	  /// </summary>
	  internal sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  scenarioCount_Renamed = DirectMetaProperty.ofImmutable(this, "scenarioCount", typeof(RepeatedScenarioMarketData), Integer.TYPE);
			  underlying_Renamed = DirectMetaProperty.ofImmutable(this, "underlying", typeof(RepeatedScenarioMarketData), typeof(MarketData));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "scenarioCount", "underlying");
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
		/// The meta-property for the {@code underlying} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<MarketData> underlying_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "scenarioCount", "underlying");
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
			case -1770633379: // underlying
			  return underlying_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends RepeatedScenarioMarketData> builder()
		public override BeanBuilder<RepeatedScenarioMarketData> builder()
		{
		  return new RepeatedScenarioMarketData.Builder();
		}

		public override Type beanType()
		{
		  return typeof(RepeatedScenarioMarketData);
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
		/// The meta-property for the {@code underlying} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<MarketData> underlying()
		{
		  return underlying_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1203198113: // scenarioCount
			  return ((RepeatedScenarioMarketData) bean).ScenarioCount;
			case -1770633379: // underlying
			  return ((RepeatedScenarioMarketData) bean).Underlying;
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
	  /// The bean-builder for {@code RepeatedScenarioMarketData}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<RepeatedScenarioMarketData>
	  {

		internal int scenarioCount;
		internal MarketData underlying;

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
			case -1770633379: // underlying
			  return underlying;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1203198113: // scenarioCount
			  this.scenarioCount = (int?) newValue.Value;
			  break;
			case -1770633379: // underlying
			  this.underlying = (MarketData) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override RepeatedScenarioMarketData build()
		{
		  return new RepeatedScenarioMarketData(scenarioCount, underlying);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("RepeatedScenarioMarketData.Builder{");
		  buf.Append("scenarioCount").Append('=').Append(JodaBeanUtils.ToString(scenarioCount)).Append(',').Append(' ');
		  buf.Append("underlying").Append('=').Append(JodaBeanUtils.ToString(underlying));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}