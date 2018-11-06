using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableSet;


	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using NamedMarketDataId = com.opengamma.strata.data.NamedMarketDataId;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// Test implementation of <seealso cref="ScenarioMarketData"/> backed by a map.
	/// </summary>
	public sealed class TestMarketDataMap : ScenarioMarketData
	{

	  private readonly MarketDataBox<LocalDate> valuationDate;

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<com.opengamma.strata.data.MarketDataId<?>, Object> valueMap;
	  private readonly IDictionary<MarketDataId<object>, object> valueMap;

	  private readonly IDictionary<ObservableId, LocalDateDoubleTimeSeries> timeSeriesMap;

	  public TestMarketDataMap<T1>(LocalDate valuationDate, IDictionary<T1> valueMap, IDictionary<ObservableId, LocalDateDoubleTimeSeries> timeSeriesMap)
	  {

		this.valuationDate = MarketDataBox.ofSingleValue(valuationDate);
		this.valueMap = valueMap;
		this.timeSeriesMap = timeSeriesMap;
	  }

	  public MarketDataBox<LocalDate> ValuationDate
	  {
		  get
		  {
			return valuationDate;
		  }
	  }

	  public int ScenarioCount
	  {
		  get
		  {
			return 1;
		  }
	  }

	  public override bool containsValue<T1>(MarketDataId<T1> id)
	  {
		return valueMap.ContainsKey(id);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public <T> com.opengamma.strata.data.scenario.MarketDataBox<T> getValue(com.opengamma.strata.data.MarketDataId<T> id)
	  public override MarketDataBox<T> getValue<T>(MarketDataId<T> id)
	  {
		T value = (T) valueMap[id];
		if (value != null)
		{
		  return MarketDataBox.ofSingleValue(value);
		}
		else
		{
		  throw new System.ArgumentException("No market data for " + id);
		}
	  }

	  public Optional<MarketDataBox<T>> findValue<T>(MarketDataId<T> id)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") T value = (T) valueMap.get(id);
		  T value = (T) valueMap[id];
		return value == null ? null : MarketDataBox.ofSingleValue(value);
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Set<com.opengamma.strata.data.MarketDataId<?>> getIds()
	  public ISet<MarketDataId<object>> Ids
	  {
		  get
		  {
			return ImmutableSet.copyOf(valueMap.Keys);
		  }
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <T> java.util.Set<com.opengamma.strata.data.MarketDataId<T>> findIds(com.opengamma.strata.data.MarketDataName<T> name)
	  public ISet<MarketDataId<T>> findIds<T>(MarketDataName<T> name)
	  {
		// no type check against id.getMarketDataType() as checked in factory
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return valueMap.keySet().stream().filter(id -> id instanceof com.opengamma.strata.data.NamedMarketDataId).filter(id -> ((com.opengamma.strata.data.NamedMarketDataId<?>) id).getMarketDataName().equals(name)).map(id -> (com.opengamma.strata.data.MarketDataId<T>) id).collect(toImmutableSet());
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return valueMap.Keys.Where(id => id is NamedMarketDataId).Where(id => ((NamedMarketDataId<object>) id).MarketDataName.Equals(name)).Select(id => (MarketDataId<T>) id).collect(toImmutableSet());
	  }

	  public ISet<ObservableId> TimeSeriesIds
	  {
		  get
		  {
			return timeSeriesMap.Keys;
		  }
	  }

	  public LocalDateDoubleTimeSeries getTimeSeries(ObservableId id)
	  {
		LocalDateDoubleTimeSeries timeSeries = timeSeriesMap[id];
		return timeSeries == null ? LocalDateDoubleTimeSeries.empty() : timeSeries;
	  }

	}

}