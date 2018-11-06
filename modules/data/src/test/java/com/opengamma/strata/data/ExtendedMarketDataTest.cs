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
	/// Test <seealso cref="ExtendedMarketData"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ExtendedMarketDataTest
	public class ExtendedMarketDataTest
	{

	  private static readonly LocalDate VAL_DATE = date(2015, 6, 30);
	  private static readonly TestingNamedId ID1 = new TestingNamedId("1");
	  private static readonly TestingNamedId ID2 = new TestingNamedId("2");
	  private static readonly TestingNamedId ID3 = new TestingNamedId("3");
	  private static readonly TestingObservableId ID4 = new TestingObservableId("4");
	  private const string VAL1 = "1";
	  private const string VAL2 = "2";
	  private const string VAL3 = "3";
	  private static readonly LocalDateDoubleTimeSeries TIME_SERIES = LocalDateDoubleTimeSeries.builder().put(date(2011, 3, 8), 1.1).put(date(2011, 3, 10), 1.2).build();
	  private static readonly ImmutableMarketData BASE_DATA = baseData();

	  //-------------------------------------------------------------------------
	  public virtual void of_addition()
	  {
		ExtendedMarketData<string> test = ExtendedMarketData.of(ID3, VAL3, BASE_DATA);
		assertEquals(test.Id, ID3);
		assertEquals(test.Value, VAL3);
		assertEquals(test.ValuationDate, VAL_DATE);
		assertEquals(test.containsValue(ID1), true);
		assertEquals(test.containsValue(ID2), true);
		assertEquals(test.containsValue(ID3), true);
		assertEquals(test.containsValue(ID4), false);
		assertEquals(test.getValue(ID1), VAL1);
		assertEquals(test.getValue(ID2), VAL2);
		assertEquals(test.getValue(ID3), VAL3);
		assertThrows(() => test.getValue(ID4), typeof(MarketDataNotFoundException));
		assertEquals(test.findValue(ID1), VAL1);
		assertEquals(test.findValue(ID2), VAL2);
		assertEquals(test.findValue(ID3), VAL3);
		assertEquals(test.findValue(ID4), null);
		assertEquals(test.Ids, ImmutableSet.of(ID1, ID2, ID3));
		assertEquals(test.findIds(ID1.MarketDataName), ImmutableSet.of(ID1));
		assertEquals(test.findIds(ID3.MarketDataName), ImmutableSet.of(ID3));
		assertEquals(test.getTimeSeries(ID4), TIME_SERIES);
	  }

	  public virtual void of_override()
	  {
		ExtendedMarketData<string> test = ExtendedMarketData.of(ID1, VAL3, BASE_DATA);
		assertEquals(test.Id, ID1);
		assertEquals(test.Value, VAL3);
		assertEquals(test.ValuationDate, VAL_DATE);
		assertEquals(test.containsValue(ID1), true);
		assertEquals(test.containsValue(ID2), true);
		assertEquals(test.containsValue(ID3), false);
		assertEquals(test.getValue(ID1), VAL3);
		assertEquals(test.getValue(ID2), VAL2);
		assertThrows(() => test.getValue(ID3), typeof(MarketDataNotFoundException));
		assertEquals(test.findValue(ID1), VAL3);
		assertEquals(test.findValue(ID2), VAL2);
		assertEquals(test.findValue(ID3), null);
		assertEquals(test.Ids, ImmutableSet.of(ID1, ID2));
		assertEquals(test.getTimeSeries(ID4), TIME_SERIES);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ExtendedMarketData<string> test = ExtendedMarketData.of(ID1, VAL1, BASE_DATA);
		coverImmutableBean(test);
		ExtendedMarketData<string> test2 = ExtendedMarketData.of(ID2, VAL2, ImmutableMarketData.of(VAL_DATE, ImmutableMap.of()));
		coverBeanEquals(test, test2);
	  }

	  public virtual void serialization()
	  {
		ExtendedMarketData<string> test = ExtendedMarketData.of(ID1, VAL3, BASE_DATA);
		assertSerialization(test);
	  }

	  private static ImmutableMarketData baseData()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<MarketDataId<?>, Object> dataMap = com.google.common.collect.ImmutableMap.of(ID1, VAL1, ID2, VAL2);
		IDictionary<MarketDataId<object>, object> dataMap = ImmutableMap.of(ID1, VAL1, ID2, VAL2);
		IDictionary<ObservableId, LocalDateDoubleTimeSeries> timeSeriesMap = ImmutableMap.of(ID4, TIME_SERIES);
		return ImmutableMarketData.builder(VAL_DATE).values(dataMap).timeSeries(timeSeriesMap).build();
	  }

	}

}