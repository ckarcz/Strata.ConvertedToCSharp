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

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Messages = com.opengamma.strata.collect.Messages;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;

	/// <summary>
	/// A set of market data which combines two underlying sets of data.
	/// <para>
	/// If the same item of data is available in both sets, it will be taken from the first.
	/// </para>
	/// <para>
	/// The underlying sets must contain the same number of scenarios, or one of them must have one scenario.
	/// If one of the underlying sets of data has one scenario the combined set will have the scenario count
	/// of the other set.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light", constructorScope = "package") final class CombinedScenarioMarketData implements ScenarioMarketData, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	internal sealed class CombinedScenarioMarketData : ScenarioMarketData, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final ScenarioMarketData underlying1;
		private readonly ScenarioMarketData underlying1;
	  /// <summary>
	  /// The second set of market data.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final ScenarioMarketData underlying2;
	  private readonly ScenarioMarketData underlying2;
	  /// <summary>
	  /// The number of scenarios for which market data is available.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(overrideGet = true) private final int scenarioCount;
	  private readonly int scenarioCount;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a new instance.
	  /// </summary>
	  /// <param name="underlying1">  the first underlying set of market data </param>
	  /// <param name="underlying2">  the second underlying set of market data </param>
	  internal CombinedScenarioMarketData(ScenarioMarketData underlying1, ScenarioMarketData underlying2)
	  {
		this.underlying1 = underlying1;
		this.underlying2 = underlying2;

		if (underlying1.ScenarioCount == 1)
		{
		  scenarioCount = underlying2.ScenarioCount;
		}
		else if (underlying2.ScenarioCount == 1)
		{
		  scenarioCount = underlying1.ScenarioCount;
		}
		else if (underlying1.ScenarioCount == underlying2.ScenarioCount)
		{
		  scenarioCount = underlying1.ScenarioCount;
		}
		else
		{
		  throw new System.ArgumentException(Messages.format("When combining scenario market data, both sets of data must have the same number of scenarios or one " + "of them must have one scenario. Found {} and {} scenarios", underlying1.ScenarioCount, underlying2.ScenarioCount));
		}
	  }

	  //-------------------------------------------------------------------------
	  public MarketDataBox<LocalDate> ValuationDate
	  {
		  get
		  {
			return underlying1.ValuationDate;
		  }
	  }

	  public override bool containsValue<T1>(MarketDataId<T1> id)
	  {
		return underlying1.containsValue(id) || underlying2.containsValue(id);
	  }

	  public override MarketDataBox<T> getValue<T>(MarketDataId<T> id)
	  {
		Optional<MarketDataBox<T>> value1 = underlying1.findValue(id);
		return value1.Present ? value1.get() : underlying2.getValue(id);
	  }

	  public Optional<MarketDataBox<T>> findValue<T>(MarketDataId<T> id)
	  {
		Optional<MarketDataBox<T>> value1 = underlying1.findValue(id);
		return value1.Present ? value1 : underlying2.findValue(id);
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Set<com.opengamma.strata.data.MarketDataId<?>> getIds()
	  public ISet<MarketDataId<object>> Ids
	  {
		  get
		  {
	//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	//ORIGINAL LINE: return com.google.common.collect.ImmutableSet.builder<com.opengamma.strata.data.MarketDataId<?>>().addAll(underlying1.getIds()).addAll(underlying2.getIds()).build();
			return ImmutableSet.builder<MarketDataId<object>>().addAll(underlying1.Ids).addAll(underlying2.Ids).build();
		  }
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <T> java.util.Set<com.opengamma.strata.data.MarketDataId<T>> findIds(com.opengamma.strata.data.MarketDataName<T> name)
	  public ISet<MarketDataId<T>> findIds<T>(MarketDataName<T> name)
	  {
		return ImmutableSet.builder<MarketDataId<T>>().addAll(underlying1.findIds(name)).addAll(underlying2.findIds(name)).build();
	  }

	  public ISet<ObservableId> TimeSeriesIds
	  {
		  get
		  {
			return ImmutableSet.builder<ObservableId>().addAll(underlying1.TimeSeriesIds).addAll(underlying2.TimeSeriesIds).build();
		  }
	  }

	  public LocalDateDoubleTimeSeries getTimeSeries(ObservableId id)
	  {
		LocalDateDoubleTimeSeries timeSeries = underlying1.getTimeSeries(id);
		return !timeSeries.Empty ? timeSeries : underlying2.getTimeSeries(id);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CombinedScenarioMarketData}.
	  /// </summary>
	  private static readonly TypedMetaBean<CombinedScenarioMarketData> META_BEAN = LightMetaBean.of(typeof(CombinedScenarioMarketData), MethodHandles.lookup(), new string[] {"underlying1", "underlying2", "scenarioCount"}, new object[0]);

	  /// <summary>
	  /// The meta-bean for {@code CombinedScenarioMarketData}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<CombinedScenarioMarketData> meta()
	  {
		return META_BEAN;
	  }

	  static CombinedScenarioMarketData()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="underlying1">  the value of the property, not null </param>
	  /// <param name="underlying2">  the value of the property, not null </param>
	  /// <param name="scenarioCount">  the value of the property </param>
	  internal CombinedScenarioMarketData(ScenarioMarketData underlying1, ScenarioMarketData underlying2, int scenarioCount)
	  {
		JodaBeanUtils.notNull(underlying1, "underlying1");
		JodaBeanUtils.notNull(underlying2, "underlying2");
		this.underlying1 = underlying1;
		this.underlying2 = underlying2;
		this.scenarioCount = scenarioCount;
	  }

	  public override TypedMetaBean<CombinedScenarioMarketData> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the first set of market data. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ScenarioMarketData Underlying1
	  {
		  get
		  {
			return underlying1;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the second set of market data. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ScenarioMarketData Underlying2
	  {
		  get
		  {
			return underlying2;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of scenarios for which market data is available. </summary>
	  /// <returns> the value of the property </returns>
	  public int ScenarioCount
	  {
		  get
		  {
			return scenarioCount;
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
		  CombinedScenarioMarketData other = (CombinedScenarioMarketData) obj;
		  return JodaBeanUtils.equal(underlying1, other.underlying1) && JodaBeanUtils.equal(underlying2, other.underlying2) && (scenarioCount == other.scenarioCount);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(underlying1);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(underlying2);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(scenarioCount);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("CombinedScenarioMarketData{");
		buf.Append("underlying1").Append('=').Append(underlying1).Append(',').Append(' ');
		buf.Append("underlying2").Append('=').Append(underlying2).Append(',').Append(' ');
		buf.Append("scenarioCount").Append('=').Append(JodaBeanUtils.ToString(scenarioCount));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}