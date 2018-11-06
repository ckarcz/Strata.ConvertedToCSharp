using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;

	/// <summary>
	/// Test <seealso cref="MarketData"/> and <seealso cref="ImmutableMarketData"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class MarketDataTest
	public class MarketDataTest
	{

	  private static readonly LocalDate VAL_DATE = date(2015, 6, 30);
	  private static readonly TestingNamedId ID1 = new TestingNamedId("1");
	  private static readonly TestingNamedId ID2 = new TestingNamedId("2");
	  private static readonly TestingNamedId ID3 = new TestingNamedId("3");
	  private static readonly TestingObservableId ID4 = new TestingObservableId("4");
	  private static readonly TestingObservableId ID5 = new TestingObservableId("5");
	  private const string VAL1 = "1";
	  private const string VAL2 = "2";
	  private const string VAL3 = "3";
	  private static readonly LocalDateDoubleTimeSeries TIME_SERIES = LocalDateDoubleTimeSeries.builder().put(date(2011, 3, 8), 1.1).put(date(2011, 3, 10), 1.2).build();

	  //-------------------------------------------------------------------------
	  public virtual void test_of_2arg()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<MarketDataId<?>, Object> dataMap = com.google.common.collect.ImmutableMap.of(ID1, VAL1, ID2, VAL2);
		IDictionary<MarketDataId<object>, object> dataMap = ImmutableMap.of(ID1, VAL1, ID2, VAL2);
		MarketData test = MarketData.of(VAL_DATE, dataMap);

		assertEquals(test.containsValue(ID1), true);
		assertEquals(test.getValue(ID1), VAL1);
		assertEquals(test.findValue(ID1), VAL1);

		assertEquals(test.containsValue(ID2), true);
		assertEquals(test.getValue(ID2), VAL2);
		assertEquals(test.findValue(ID2), VAL2);

		assertEquals(test.containsValue(ID3), false);
		assertThrows(() => test.getValue(ID3), typeof(MarketDataNotFoundException));
		assertEquals(test.findValue(ID3), null);

		assertEquals(test.Ids, ImmutableSet.of(ID1, ID2));

		assertEquals(test.findIds(ID1.MarketDataName), ImmutableSet.of(ID1));
		assertEquals(test.findIds(new TestingName("Foo")), ImmutableSet.of());

		assertEquals(test.getTimeSeries(ID4), LocalDateDoubleTimeSeries.empty());
		assertEquals(test.getTimeSeries(ID5), LocalDateDoubleTimeSeries.empty());
	  }

	  public virtual void test_of_3arg()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<MarketDataId<?>, Object> dataMap = com.google.common.collect.ImmutableMap.of(ID1, VAL1);
		IDictionary<MarketDataId<object>, object> dataMap = ImmutableMap.of(ID1, VAL1);
		IDictionary<ObservableId, LocalDateDoubleTimeSeries> tsMap = ImmutableMap.of(ID5, TIME_SERIES);
		MarketData test = MarketData.of(VAL_DATE, dataMap, tsMap);

		assertEquals(test.containsValue(ID1), true);
		assertEquals(test.getValue(ID1), VAL1);
		assertEquals(test.findValue(ID1), VAL1);

		assertEquals(test.containsValue(ID2), false);
		assertThrows(() => test.getValue(ID2), typeof(MarketDataNotFoundException));
		assertEquals(test.findValue(ID2), null);

		assertEquals(test.Ids, ImmutableSet.of(ID1));

		assertEquals(test.getTimeSeries(ID4), LocalDateDoubleTimeSeries.empty());
		assertEquals(test.getTimeSeries(ID5), TIME_SERIES);
	  }

	  public virtual void test_of_badType()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<MarketDataId<?>, Object> dataMap = com.google.common.collect.ImmutableMap.of(ID1, 123d);
		IDictionary<MarketDataId<object>, object> dataMap = ImmutableMap.of(ID1, 123d);
		assertThrows(() => MarketData.of(VAL_DATE, dataMap), typeof(System.InvalidCastException));
	  }

	  public virtual void test_of_null()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<MarketDataId<?>, Object> dataMap = new java.util.HashMap<>();
		IDictionary<MarketDataId<object>, object> dataMap = new Dictionary<MarketDataId<object>, object>();
		dataMap[ID1] = null;
		assertThrowsIllegalArg(() => MarketData.of(VAL_DATE, dataMap));
	  }

	  public virtual void empty()
	  {
		MarketData test = MarketData.empty(VAL_DATE);

		assertEquals(test.containsValue(ID1), false);
		assertEquals(test.Ids, ImmutableSet.of());
		assertEquals(test.getTimeSeries(ID4), LocalDateDoubleTimeSeries.empty());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		ImmutableMarketData test = ImmutableMarketData.builder(VAL_DATE.plusDays(1)).valuationDate(VAL_DATE).addValue(ID1, "123").addValueUnsafe(ID2, "124").addValueMap(ImmutableMap.of(ID3, "201")).addTimeSeries(ID4, TIME_SERIES).build();
		assertEquals(test.ValuationDate, VAL_DATE);
		assertEquals(test.Values.get(ID1), "123");
		assertEquals(test.Values.get(ID2), "124");
		assertEquals(test.Ids, ImmutableSet.of(ID1, ID2, ID3));
		assertEquals(test.TimeSeries.get(ID4), TIME_SERIES);
	  }

	  public virtual void test_builder_badType()
	  {
		assertThrows(() => ImmutableMarketData.builder(VAL_DATE).addValueUnsafe(ID1, 123d), typeof(System.InvalidCastException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_defaultMethods()
	  {
		MarketData test = new MarketDataAnonymousInnerClass(this);
		assertEquals(test.ValuationDate, VAL_DATE);
		assertEquals(test.containsValue(ID1), true);
		assertEquals(test.containsValue(ID2), false);
		assertEquals(test.getValue(ID1), VAL1);
		assertThrows(() => test.getValue(ID2), typeof(MarketDataNotFoundException));
		assertEquals(test.findValue(ID1), VAL1);
		assertEquals(test.findValue(ID2), null);
	  }

	  private class MarketDataAnonymousInnerClass : MarketData
	  {
		  private readonly MarketDataTest outerInstance;

		  public MarketDataAnonymousInnerClass(MarketDataTest outerInstance)
		  {
			  this.outerInstance = outerInstance;
		  }


		  public LocalDate ValuationDate
		  {
			  get
			  {
				return VAL_DATE;
			  }
		  }

		  public LocalDateDoubleTimeSeries getTimeSeries(ObservableId id)
		  {
			return TIME_SERIES;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public <T> java.util.Optional<T> findValue(MarketDataId<T> id)
		  public Optional<T> findValue<T>(MarketDataId<T> id)
		  {
			return id.Equals(ID1) ? ((T) VAL1) : null;
		  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public java.util.Set<MarketDataId<?>> getIds()
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
	  public virtual void test_combinedWith_noClash()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<MarketDataId<?>, Object> dataMap1 = com.google.common.collect.ImmutableMap.of(ID1, VAL1);
		IDictionary<MarketDataId<object>, object> dataMap1 = ImmutableMap.of(ID1, VAL1);
		MarketData test1 = MarketData.of(VAL_DATE, dataMap1);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<MarketDataId<?>, Object> dataMap2 = com.google.common.collect.ImmutableMap.of(ID2, VAL2);
		IDictionary<MarketDataId<object>, object> dataMap2 = ImmutableMap.of(ID2, VAL2);
		MarketData test2 = MarketData.of(VAL_DATE, dataMap2);

		MarketData test = test1.combinedWith(test2);
		assertEquals(test.getValue(ID1), VAL1);
		assertEquals(test.getValue(ID2), VAL2);
		assertEquals(test.Ids, ImmutableSet.of(ID1, ID2));
	  }

	  public virtual void test_combinedWith_noClashSame()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<MarketDataId<?>, Object> dataMap1 = com.google.common.collect.ImmutableMap.of(ID1, VAL1);
		IDictionary<MarketDataId<object>, object> dataMap1 = ImmutableMap.of(ID1, VAL1);
		MarketData test1 = MarketData.of(VAL_DATE, dataMap1);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<MarketDataId<?>, Object> dataMap2 = com.google.common.collect.ImmutableMap.of(ID1, VAL1, ID2, VAL2);
		IDictionary<MarketDataId<object>, object> dataMap2 = ImmutableMap.of(ID1, VAL1, ID2, VAL2);
		MarketData test2 = MarketData.of(VAL_DATE, dataMap2);

		MarketData test = test1.combinedWith(test2);
		assertEquals(test.getValue(ID1), VAL1);
		assertEquals(test.getValue(ID2), VAL2);
		assertEquals(test.Ids, ImmutableSet.of(ID1, ID2));
	  }

	  public virtual void test_combinedWith_clash()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<MarketDataId<?>, Object> dataMap1 = com.google.common.collect.ImmutableMap.of(ID1, VAL1);
		IDictionary<MarketDataId<object>, object> dataMap1 = ImmutableMap.of(ID1, VAL1);
		MarketData test1 = MarketData.of(VAL_DATE, dataMap1);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<MarketDataId<?>, Object> dataMap2 = com.google.common.collect.ImmutableMap.of(ID1, VAL3);
		IDictionary<MarketDataId<object>, object> dataMap2 = ImmutableMap.of(ID1, VAL3);
		MarketData test2 = MarketData.of(VAL_DATE, dataMap2);
		MarketData combined = test1.combinedWith(test2);
		assertEquals(combined.getValue(ID1), VAL1);
		assertEquals(combined.Ids, ImmutableSet.of(ID1));
	  }

	  public virtual void test_combinedWith_dateMismatch()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<MarketDataId<?>, Object> dataMap1 = com.google.common.collect.ImmutableMap.of(ID1, VAL1);
		IDictionary<MarketDataId<object>, object> dataMap1 = ImmutableMap.of(ID1, VAL1);
		MarketData test1 = MarketData.of(VAL_DATE, dataMap1);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<MarketDataId<?>, Object> dataMap2 = com.google.common.collect.ImmutableMap.of(ID1, VAL3);
		IDictionary<MarketDataId<object>, object> dataMap2 = ImmutableMap.of(ID1, VAL3);
		MarketData test2 = MarketData.of(VAL_DATE.plusDays(1), dataMap2);
		assertThrowsIllegalArg(() => test1.combinedWith(test2));
	  }

	  /// <summary>
	  /// Tests the combinedWith method when the MarketData instances are not both ImmutableMarketData.
	  /// </summary>
	  public virtual void test_combinedWith_differentTypes()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<MarketDataId<?>, Object> dataMap1 = com.google.common.collect.ImmutableMap.of(ID1, VAL1, ID2, VAL2);
		IDictionary<MarketDataId<object>, object> dataMap1 = ImmutableMap.of(ID1, VAL1, ID2, VAL2);
		MarketData test1 = MarketData.of(VAL_DATE, dataMap1);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<MarketDataId<?>, Object> dataMap2 = com.google.common.collect.ImmutableMap.of(ID1, VAL1);
		IDictionary<MarketDataId<object>, object> dataMap2 = ImmutableMap.of(ID1, VAL1);
		MarketData test2 = MarketData.of(VAL_DATE, dataMap2);
		ExtendedMarketData<string> test3 = ExtendedMarketData.of(ID1, VAL3, test2);

		MarketData test = test3.combinedWith(test1);
		assertEquals(test.getValue(ID1), VAL3);
		assertEquals(test.getValue(ID2), VAL2);
		assertEquals(test.Ids, ImmutableSet.of(ID1, ID2));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withValue()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<MarketDataId<?>, Object> dataMap = com.google.common.collect.ImmutableMap.of(ID1, VAL1);
		IDictionary<MarketDataId<object>, object> dataMap = ImmutableMap.of(ID1, VAL1);
		MarketData test = MarketData.of(VAL_DATE, dataMap).withValue(ID1, VAL3);
		assertEquals(test.getValue(ID1), VAL3);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<MarketDataId<?>, Object> dataMap = com.google.common.collect.ImmutableMap.of(ID1, VAL1);
		IDictionary<MarketDataId<object>, object> dataMap = ImmutableMap.of(ID1, VAL1);
		ImmutableMarketData test = ImmutableMarketData.of(VAL_DATE, dataMap);
		coverImmutableBean(test);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<MarketDataId<?>, Object> dataMap2 = com.google.common.collect.ImmutableMap.of(ID2, VAL2);
		IDictionary<MarketDataId<object>, object> dataMap2 = ImmutableMap.of(ID2, VAL2);
		ImmutableMarketData test2 = ImmutableMarketData.of(VAL_DATE.minusDays(1), dataMap2);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<MarketDataId<?>, Object> dataMap = com.google.common.collect.ImmutableMap.of(ID1, VAL1);
		IDictionary<MarketDataId<object>, object> dataMap = ImmutableMap.of(ID1, VAL1);
		MarketData test = MarketData.of(VAL_DATE, dataMap);
		assertSerialization(test);
	  }

	}

}