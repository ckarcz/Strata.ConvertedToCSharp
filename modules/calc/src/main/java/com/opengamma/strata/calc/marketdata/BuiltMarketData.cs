using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using Failure = com.opengamma.strata.collect.result.Failure;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using ObservableId = com.opengamma.strata.data.ObservableId;

	/// <summary>
	/// Market data that has been built.
	/// <para>
	/// The <seealso cref="MarketDataFactory"/> can be used to build market data from external
	/// sources and by calibration. This implementation of <seealso cref="MarketData"/>
	/// provides the result, and includes all the market data, such as quotes and curves.
	/// </para>
	/// <para>
	/// This implementation differs from <seealso cref="ImmutableMarketData"/> because it
	/// stores the failures that occurred during the build process.
	/// These errors are exposed to users when data is queried.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private", constructorScope = "package") public final class BuiltMarketData implements com.opengamma.strata.data.MarketData, org.joda.beans.ImmutableBean
	public sealed class BuiltMarketData : MarketData, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final BuiltScenarioMarketData underlying;
		private readonly BuiltScenarioMarketData underlying;

	  //-------------------------------------------------------------------------
	  public LocalDate ValuationDate
	  {
		  get
		  {
			return underlying.ValuationDate.SingleValue;
		  }
	  }

	  public override bool containsValue<T1>(MarketDataId<T1> id)
	  {
		return underlying.containsValue(id);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public <T> T getValue(com.opengamma.strata.data.MarketDataId<T> id)
	  public override T getValue<T>(MarketDataId<T> id)
	  {
		return underlying.getValue(id).SingleValue;
	  }

	  public Optional<T> findValue<T>(MarketDataId<T> id)
	  {
		return underlying.findValue(id).map(v => v.SingleValue);
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

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the failures when building single market data values.
	  /// </summary>
	  /// <returns> the single value failures </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.google.common.collect.ImmutableMap<com.opengamma.strata.data.MarketDataId<?>, com.opengamma.strata.collect.result.Failure> getValueFailures()
	  public ImmutableMap<MarketDataId<object>, Failure> ValueFailures
	  {
		  get
		  {
			return underlying.ValueFailures;
		  }
	  }

	  /// <summary>
	  /// Gets the failures that occurred when building time series of market data values.
	  /// </summary>
	  /// <returns> the time-series value failures </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.google.common.collect.ImmutableMap<com.opengamma.strata.data.MarketDataId<?>, com.opengamma.strata.collect.result.Failure> getTimeSeriesFailures()
	  public ImmutableMap<MarketDataId<object>, Failure> TimeSeriesFailures
	  {
		  get
		  {
			return underlying.TimeSeriesFailures;
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code BuiltMarketData}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static BuiltMarketData.Meta meta()
	  {
		return BuiltMarketData.Meta.INSTANCE;
	  }

	  static BuiltMarketData()
	  {
		MetaBean.register(BuiltMarketData.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="underlying">  the value of the property, not null </param>
	  internal BuiltMarketData(BuiltScenarioMarketData underlying)
	  {
		JodaBeanUtils.notNull(underlying, "underlying");
		this.underlying = underlying;
	  }

	  public override BuiltMarketData.Meta metaBean()
	  {
		return BuiltMarketData.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying market data. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public BuiltScenarioMarketData Underlying
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
		  BuiltMarketData other = (BuiltMarketData) obj;
		  return JodaBeanUtils.equal(underlying, other.underlying);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(underlying);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(64);
		buf.Append("BuiltMarketData{");
		buf.Append("underlying").Append('=').Append(JodaBeanUtils.ToString(underlying));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code BuiltMarketData}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  underlying_Renamed = DirectMetaProperty.ofImmutable(this, "underlying", typeof(BuiltMarketData), typeof(BuiltScenarioMarketData));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "underlying");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code underlying} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BuiltScenarioMarketData> underlying_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "underlying");
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
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends BuiltMarketData> builder()
		public override BeanBuilder<BuiltMarketData> builder()
		{
		  return new BuiltMarketData.Builder();
		}

		public override Type beanType()
		{
		  return typeof(BuiltMarketData);
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
		public MetaProperty<BuiltScenarioMarketData> underlying()
		{
		  return underlying_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1770633379: // underlying
			  return ((BuiltMarketData) bean).Underlying;
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
	  /// The bean-builder for {@code BuiltMarketData}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<BuiltMarketData>
	  {

		internal BuiltScenarioMarketData underlying;

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
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1770633379: // underlying
			  this.underlying = (BuiltScenarioMarketData) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override BuiltMarketData build()
		{
		  return new BuiltMarketData(underlying);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(64);
		  buf.Append("BuiltMarketData.Builder{");
		  buf.Append("underlying").Append('=').Append(JodaBeanUtils.ToString(underlying));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}