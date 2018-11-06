using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Messages = com.opengamma.strata.collect.Messages;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;

	/// <summary>
	/// A set of market data where an item has been added or overridden.
	/// <para>
	/// This decorates an underlying instance to add or replace a single identifier.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") final class ExtendedScenarioMarketData<T> implements ScenarioMarketData, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	internal sealed class ExtendedScenarioMarketData<T> : ScenarioMarketData, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.data.MarketDataId<T> id;
		private readonly MarketDataId<T> id;
	  /// <summary>
	  /// The additional market data value.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final MarketDataBox<T> value;
	  private readonly MarketDataBox<T> value;
	  /// <summary>
	  /// The underlying market data.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final ScenarioMarketData underlying;
	  private readonly ScenarioMarketData underlying;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance that decorates the underlying market data.
	  /// <para>
	  /// The specified identifier can be queried in the result, returning the specified value.
	  /// All other identifiers are queried in the underlying market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="id">  the additional market data identifier </param>
	  /// <param name="value">  the additional market data value </param>
	  /// <param name="underlying">  the underlying market data </param>
	  /// <returns> a market data instance that decorates the original adding/overriding the specified identifier </returns>
	  public static ExtendedScenarioMarketData<T> of<T>(MarketDataId<T> id, MarketDataBox<T> value, ScenarioMarketData underlying)
	  {

		return new ExtendedScenarioMarketData<T>(id, value, underlying);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (value.ScenarioValue && value.ScenarioCount != underlying.ScenarioCount)
		{
		  throw new System.ArgumentException(Messages.format("Scenario count mismatch: value has {} scenarios but this market data has {}", value.ScenarioCount, underlying.ScenarioCount));
		}
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
		if (this.id.Equals(id))
		{
		  return true;
		}
		return underlying.containsValue(id);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <R> MarketDataBox<R> getValue(com.opengamma.strata.data.MarketDataId<R> id)
	  public override MarketDataBox<R> getValue<R>(MarketDataId<R> id)
	  {
		if (this.id.Equals(id))
		{
		  return (MarketDataBox<R>) value;
		}
		return underlying.getValue(id);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <R> java.util.Optional<MarketDataBox<R>> findValue(com.opengamma.strata.data.MarketDataId<R> id)
	  public Optional<MarketDataBox<R>> findValue<R>(MarketDataId<R> id)
	  {
		if (this.id.Equals(id))
		{
		  return ((MarketDataBox<R>) value);
		}
		return underlying.findValue(id);
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Set<com.opengamma.strata.data.MarketDataId<?>> getIds()
	  public ISet<MarketDataId<object>> Ids
	  {
		  get
		  {
	//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	//ORIGINAL LINE: return com.google.common.collect.ImmutableSet.builder<com.opengamma.strata.data.MarketDataId<?>>().addAll(underlying.getIds()).add(id).build();
			return ImmutableSet.builder<MarketDataId<object>>().addAll(underlying.Ids).add(id).build();
		  }
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <R> java.util.Set<com.opengamma.strata.data.MarketDataId<R>> findIds(com.opengamma.strata.data.MarketDataName<R> name)
	  public ISet<MarketDataId<R>> findIds<R>(MarketDataName<R> name)
	  {
		ISet<MarketDataId<R>> ids = underlying.findIds(name);
		if (id is NamedMarketDataId)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.NamedMarketDataId<?> named = (com.opengamma.strata.data.NamedMarketDataId<?>) id;
		  NamedMarketDataId<object> named = (NamedMarketDataId<object>) id;
		  if (named.MarketDataName.Equals(name))
		  {
			return ImmutableSet.builder<MarketDataId<R>>().addAll(ids).add((MarketDataId<R>) id).build();
		  }
		}
		return ids;
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
	  /// The meta-bean for {@code ExtendedScenarioMarketData}.
	  /// </summary>
	  private static readonly MetaBean META_BEAN = LightMetaBean.of(typeof(ExtendedScenarioMarketData), MethodHandles.lookup(), new string[] {"id", "value", "underlying"}, new object[0]);

	  /// <summary>
	  /// The meta-bean for {@code ExtendedScenarioMarketData}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static MetaBean meta()
	  {
		return META_BEAN;
	  }

	  static ExtendedScenarioMarketData()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private ExtendedScenarioMarketData(MarketDataId<T> id, MarketDataBox<T> value, ScenarioMarketData underlying)
	  {
		JodaBeanUtils.notNull(id, "id");
		JodaBeanUtils.notNull(value, "value");
		JodaBeanUtils.notNull(underlying, "underlying");
		this.id = id;
		this.value = value;
		this.underlying = underlying;
		validate();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public org.joda.beans.TypedMetaBean<ExtendedScenarioMarketData<T>> metaBean()
	  public override TypedMetaBean<ExtendedScenarioMarketData<T>> metaBean()
	  {
		return (TypedMetaBean<ExtendedScenarioMarketData<T>>) META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the additional market data identifier. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public MarketDataId<T> Id
	  {
		  get
		  {
			return id;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the additional market data value. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public MarketDataBox<T> Value
	  {
		  get
		  {
			return value;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying market data. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ScenarioMarketData Underlying
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
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: ExtendedScenarioMarketData<?> other = (ExtendedScenarioMarketData<?>) obj;
		  ExtendedScenarioMarketData<object> other = (ExtendedScenarioMarketData<object>) obj;
		  return JodaBeanUtils.equal(id, other.id) && JodaBeanUtils.equal(value, other.value) && JodaBeanUtils.equal(underlying, other.underlying);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(id);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(value);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(underlying);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("ExtendedScenarioMarketData{");
		buf.Append("id").Append('=').Append(id).Append(',').Append(' ');
		buf.Append("value").Append('=').Append(value).Append(',').Append(' ');
		buf.Append("underlying").Append('=').Append(JodaBeanUtils.ToString(underlying));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}