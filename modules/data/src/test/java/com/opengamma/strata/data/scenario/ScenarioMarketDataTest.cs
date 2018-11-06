using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;


	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;

	/// <summary>
	/// Test <seealso cref="ScenarioMarketData"/> and <seealso cref="ImmutableScenarioMarketData"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ScenarioMarketDataTest
	public class ScenarioMarketDataTest
	{

	  private static readonly LocalDate VAL_DATE = date(2015, 6, 30);
	  private static readonly TestObservableId ID1 = TestObservableId.of("1");
	  private static readonly TestObservableId ID2 = TestObservableId.of("2");
	  private const double VAL1 = 1d;
	  private const double VAL2 = 2d;
	  private const double VAL3 = 3d;
	  private static readonly MarketDataBox<double> BOX1 = MarketDataBox.ofScenarioValues(VAL1, VAL2);
	  private static readonly MarketDataBox<double> BOX2 = MarketDataBox.ofScenarioValues(VAL3);
	  private static readonly LocalDateDoubleTimeSeries TIME_SERIES = LocalDateDoubleTimeSeries.builder().put(date(2011, 3, 8), 1.1).put(date(2011, 3, 10), 1.2).build();

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, MarketDataBox<?>> dataMap = com.google.common.collect.ImmutableMap.of(ID1, BOX1);
		IDictionary<MarketDataId<object>, MarketDataBox<object>> dataMap = ImmutableMap.of(ID1, BOX1);
		IDictionary<ObservableId, LocalDateDoubleTimeSeries> tsMap = ImmutableMap.of(ID1, TIME_SERIES);
		ScenarioMarketData test = ScenarioMarketData.of(2, VAL_DATE, dataMap, tsMap);
		assertThat(test.ValuationDate).isEqualTo(MarketDataBox.ofSingleValue(VAL_DATE));
		assertThat(test.containsValue(ID1)).True;
		assertThat(test.containsValue(ID2)).False;
		assertThat(test.getValue(ID1)).isEqualTo(BOX1);
		assertThrows(() => test.getValue(ID2), typeof(MarketDataNotFoundException));
		assertThat(test.findValue(ID1)).hasValue(BOX1);
		assertThat(test.findValue(ID2)).Empty;
		assertThat(test.Ids).isEqualTo(ImmutableSet.of(ID1));
		assertThat(test.getTimeSeries(ID1)).isEqualTo(TIME_SERIES);
		assertThat(test.getTimeSeries(ID2)).isEqualTo(LocalDateDoubleTimeSeries.empty());
	  }

	  public virtual void test_of_noScenarios()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, MarketDataBox<?>> dataMap = com.google.common.collect.ImmutableMap.of(ID1, MarketDataBox.empty());
		IDictionary<MarketDataId<object>, MarketDataBox<object>> dataMap = ImmutableMap.of(ID1, MarketDataBox.empty());
		ScenarioMarketData test = ScenarioMarketData.of(0, VAL_DATE, dataMap, ImmutableMap.of());
		assertThat(test.ValuationDate).isEqualTo(MarketDataBox.ofSingleValue(VAL_DATE));
		assertThat(test.containsValue(ID1)).True;
		assertThat(test.containsValue(ID2)).False;
		assertThat(test.getValue(ID1)).isEqualTo(MarketDataBox.empty());
		assertThrows(() => test.getValue(ID2), typeof(MarketDataNotFoundException));
		assertThat(test.findValue(ID1)).hasValue(MarketDataBox.empty());
		assertThat(test.findValue(ID2)).Empty;
		assertThat(test.Ids).isEqualTo(ImmutableSet.of(ID1));
		assertThat(test.getTimeSeries(ID1)).isEqualTo(LocalDateDoubleTimeSeries.empty());
		assertThat(test.getTimeSeries(ID2)).isEqualTo(LocalDateDoubleTimeSeries.empty());
	  }

	  public virtual void test_of_repeated()
	  {
		ScenarioMarketData test = ScenarioMarketData.of(1, MarketData.of(VAL_DATE, ImmutableMap.of(ID1, VAL1)));
		assertThat(test.ValuationDate).isEqualTo(MarketDataBox.ofSingleValue(VAL_DATE));
		assertThat(test.getValue(ID1)).isEqualTo(MarketDataBox.ofSingleValue(VAL1));
	  }

	  public virtual void test_empty()
	  {
		ScenarioMarketData test = ScenarioMarketData.empty();
		assertThat(test.ValuationDate).isEqualTo(MarketDataBox.empty());
		assertThat(test.containsValue(ID1)).False;
		assertThat(test.containsValue(ID2)).False;
		assertThrows(() => test.getValue(ID1), typeof(MarketDataNotFoundException));
		assertThrows(() => test.getValue(ID2), typeof(MarketDataNotFoundException));
		assertThat(test.findValue(ID1)).Empty;
		assertThat(test.findValue(ID2)).Empty;
		assertThat(test.Ids).isEqualTo(ImmutableSet.of());
		assertThat(test.getTimeSeries(ID1)).isEqualTo(LocalDateDoubleTimeSeries.empty());
		assertThat(test.getTimeSeries(ID2)).isEqualTo(LocalDateDoubleTimeSeries.empty());
	  }

	  public virtual void of_null()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, MarketDataBox<?>> dataMap = new java.util.HashMap<>();
		IDictionary<MarketDataId<object>, MarketDataBox<object>> dataMap = new Dictionary<MarketDataId<object>, MarketDataBox<object>>();
		dataMap[ID1] = null;
		IDictionary<ObservableId, LocalDateDoubleTimeSeries> tsMap = ImmutableMap.of(ID1, TIME_SERIES);
		assertThrows(() => ScenarioMarketData.of(2, VAL_DATE, dataMap, tsMap), typeof(System.ArgumentException));
	  }

	  public virtual void of_badType()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, MarketDataBox<?>> dataMap = com.google.common.collect.ImmutableMap.of(ID1, MarketDataBox.ofScenarioValues("", ""));
		IDictionary<MarketDataId<object>, MarketDataBox<object>> dataMap = ImmutableMap.of(ID1, MarketDataBox.ofScenarioValues("", ""));
		IDictionary<ObservableId, LocalDateDoubleTimeSeries> tsMap = ImmutableMap.of(ID1, TIME_SERIES);
		assertThrows(() => ScenarioMarketData.of(2, VAL_DATE, dataMap, tsMap), typeof(System.InvalidCastException));
	  }

	  public virtual void of_badScenarios()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, MarketDataBox<?>> dataMap = com.google.common.collect.ImmutableMap.of(ID1, MarketDataBox.ofScenarioValues(VAL1));
		IDictionary<MarketDataId<object>, MarketDataBox<object>> dataMap = ImmutableMap.of(ID1, MarketDataBox.ofScenarioValues(VAL1));
		IDictionary<ObservableId, LocalDateDoubleTimeSeries> tsMap = ImmutableMap.of(ID1, TIME_SERIES);
		assertThrows(() => ScenarioMarketData.of(2, VAL_DATE, dataMap, tsMap), typeof(System.ArgumentException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_defaultMethods()
	  {
		ScenarioMarketData test = new ScenarioMarketDataAnonymousInnerClass(this);
		assertThat(test.ValuationDate).isEqualTo(MarketDataBox.ofSingleValue(VAL_DATE));
		assertThat(test.containsValue(ID1)).True;
		assertThat(test.containsValue(ID2)).False;
		assertThat(test.getValue(ID1)).isEqualTo(BOX1);
		assertThrows(() => test.getValue(ID2), typeof(MarketDataNotFoundException));
		assertThat(test.findValue(ID1)).hasValue(BOX1);
		assertThat(test.findValue(ID2)).Empty;
	  }

	  private class ScenarioMarketDataAnonymousInnerClass : ScenarioMarketData
	  {
		  private readonly ScenarioMarketDataTest outerInstance;

		  public ScenarioMarketDataAnonymousInnerClass(ScenarioMarketDataTest outerInstance)
		  {
			  this.outerInstance = outerInstance;
		  }


		  public MarketDataBox<LocalDate> ValuationDate
		  {
			  get
			  {
				return MarketDataBox.ofSingleValue(VAL_DATE);
			  }
		  }

		  public LocalDateDoubleTimeSeries getTimeSeries(ObservableId id)
		  {
			return LocalDateDoubleTimeSeries.empty();
		  }

		  public int ScenarioCount
		  {
			  get
			  {
				return 2;
			  }
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public <T> java.util.Optional<MarketDataBox<T>> findValue(com.opengamma.strata.data.MarketDataId<T> id)
		  public Optional<MarketDataBox<T>> findValue<T>(MarketDataId<T> id)
		  {
			return id.Equals(ID1) ? ((MarketDataBox<T>) BOX1) : null;
		  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public java.util.Set<com.opengamma.strata.data.MarketDataId<?>> getIds()
		  public ISet<MarketDataId<object>> Ids
		  {
			  get
			  {
				return ImmutableSet.of();
			  }
		  }

		  public ISet<MarketDataId<T>> findIds<T>(MarketDataName<T> name)
		  {
			return ImmutableSet.of();
		  }

		  public ISet<ObservableId> TimeSeriesIds
		  {
			  get
			  {
				return ImmutableSet.of();
			  }
		  }
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_scenarios()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, MarketDataBox<?>> dataMap = com.google.common.collect.ImmutableMap.of(ID1, BOX1);
		IDictionary<MarketDataId<object>, MarketDataBox<object>> dataMap = ImmutableMap.of(ID1, BOX1);
		IDictionary<ObservableId, LocalDateDoubleTimeSeries> tsMap = ImmutableMap.of(ID1, TIME_SERIES);
		ScenarioMarketData test = ScenarioMarketData.of(2, VAL_DATE, dataMap, tsMap);

		MarketData scenario0 = test.scenario(0);
		MarketData scenario1 = test.scenario(1);
		assertThat(scenario0.getValue(ID1)).isEqualTo(BOX1.getValue(0));
		assertThat(scenario1.getValue(ID1)).isEqualTo(BOX1.getValue(1));
		IList<double> list = test.scenarios().map(s => s.getValue(ID1)).collect(toImmutableList());
		assertThat(list[0]).isEqualTo(BOX1.getValue(0));
		assertThat(list[1]).isEqualTo(BOX1.getValue(1));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, MarketDataBox<?>> dataMap = com.google.common.collect.ImmutableMap.of(ID1, BOX1);
		IDictionary<MarketDataId<object>, MarketDataBox<object>> dataMap = ImmutableMap.of(ID1, BOX1);
		IDictionary<ObservableId, LocalDateDoubleTimeSeries> tsMap = ImmutableMap.of(ID1, TIME_SERIES);
		ImmutableScenarioMarketData test = ImmutableScenarioMarketData.of(2, VAL_DATE, dataMap, tsMap);
		coverImmutableBean(test);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, MarketDataBox<?>> dataMap2 = com.google.common.collect.ImmutableMap.of(ID2, BOX2);
		IDictionary<MarketDataId<object>, MarketDataBox<object>> dataMap2 = ImmutableMap.of(ID2, BOX2);
		IDictionary<ObservableId, LocalDateDoubleTimeSeries> tsMap2 = ImmutableMap.of(ID2, TIME_SERIES);
		ImmutableScenarioMarketData test2 = ImmutableScenarioMarketData.of(1, VAL_DATE.plusDays(1), dataMap2, tsMap2);
		coverBeanEquals(test, test2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void getScenarioValueFromSingleValue()
	  {
		MarketDataBox<double> box = MarketDataBox.ofSingleValue(9d);
		TestMarketData marketData = new TestMarketData(box);
		TestArrayKey key = new TestArrayKey();
		TestDoubleArray array = marketData.getScenarioValue(key);
		assertThat(array.values).isEqualTo(DoubleArray.of(9, 9, 9));
	  }

	  public virtual void getScenarioValueFromRequestedScenarioValue()
	  {
		MarketDataBox<double> box = MarketDataBox.ofScenarioValue(new TestDoubleArray(DoubleArray.of(9d, 9d, 9d)));
		TestMarketData marketData = new TestMarketData(box);
		TestArrayKey key = new TestArrayKey();
		TestDoubleArray array = marketData.getScenarioValue(key);
		assertThat(array.values).isEqualTo(DoubleArray.of(9, 9, 9));
	  }

	  public virtual void getScenarioValueFromOtherScenarioValue()
	  {
		MarketDataBox<double> box = MarketDataBox.ofScenarioValues(9d, 9d, 9d);
		TestMarketData marketData = new TestMarketData(box);
		TestArrayKey key = new TestArrayKey();
		TestDoubleArray array = marketData.getScenarioValue(key);
		assertThat(array.values).isEqualTo(DoubleArray.of(9, 9, 9));
	  }

	  //--------------------------------------------------------------------------------------------------

	  private sealed class TestDoubleArray : ScenarioArray<double>
	  {

		internal readonly DoubleArray values;

		internal TestDoubleArray(DoubleArray values)
		{
		  this.values = values;
		}

		public double? get(int scenarioIndex)
		{
		  return values.get(scenarioIndex);
		}

		public int ScenarioCount
		{
			get
			{
			  return values.size();
			}
		}

		public override Stream<double> stream()
		{
		  return values.stream().boxed();
		}
	  }

	  //--------------------------------------------------------------------------------------------------

	  private sealed class TestId : MarketDataId<double>
	  {

		public Type<double> MarketDataType
		{
			get
			{
			  return typeof(Double);
			}
		}
	  }

	  //--------------------------------------------------------------------------------------------------

	  private sealed class TestArrayKey : ScenarioMarketDataId<double, TestDoubleArray>
	  {

		public MarketDataId<double> MarketDataId
		{
			get
			{
			  return new TestId();
			}
		}

		public Type<TestDoubleArray> ScenarioMarketDataType
		{
			get
			{
			  return typeof(TestDoubleArray);
			}
		}

		public TestDoubleArray createScenarioValue(MarketDataBox<double> marketDataBox, int scenarioCount)
		{
		  return new TestDoubleArray(DoubleArray.of(scenarioCount, i => marketDataBox.getValue(i)));
		}
	  }

	  //--------------------------------------------------------------------------------------------------

	  private sealed class TestMarketData : ScenarioMarketData
	  {

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final MarketDataBox<?> value;
		internal readonly MarketDataBox<object> value;

		internal TestMarketData<T1>(MarketDataBox<T1> value)
		{
		  this.value = value;
		}

		public MarketDataBox<LocalDate> ValuationDate
		{
			get
			{
			  throw new System.NotSupportedException("getValuationDate() not implemented");
			}
		}

		public int ScenarioCount
		{
			get
			{
			  return 3;
			}
		}

		public override Stream<MarketData> scenarios()
		{
		  throw new System.NotSupportedException("scenarios() not implemented");
		}

		public override MarketData scenario(int scenarioIndex)
		{
		  throw new System.NotSupportedException("scenario(int) not implemented");
		}

		public override bool containsValue<T1>(MarketDataId<T1> id)
		{
		  throw new System.NotSupportedException("containsValue(MarketDataKey) not implemented");
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <T> MarketDataBox<T> getValue(com.opengamma.strata.data.MarketDataId<T> id)
		public override MarketDataBox<T> getValue<T>(MarketDataId<T> id)
		{
		  return (MarketDataBox<T>) value;
		}

		public Optional<MarketDataBox<T>> findValue<T>(MarketDataId<T> id)
		{
		  throw new System.NotSupportedException("findValue not implemented");
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Set<com.opengamma.strata.data.MarketDataId<?>> getIds()
		public ISet<MarketDataId<object>> Ids
		{
			get
			{
			  return ImmutableSet.of();
			}
		}

		public ISet<MarketDataId<T>> findIds<T>(MarketDataName<T> name)
		{
		  return ImmutableSet.of();
		}

		public LocalDateDoubleTimeSeries getTimeSeries(ObservableId id)
		{
		  throw new System.NotSupportedException("getTimeSeries(ObservableKey) not implemented");
		}

		public ISet<ObservableId> TimeSeriesIds
		{
			get
			{
			  return ImmutableSet.of();
			}
		}
	  }
	}

}