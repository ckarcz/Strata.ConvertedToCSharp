/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertSame;


	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;

	/// <summary>
	/// Test <seealso cref="RepeatedScenarioMarketData"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class RepeatedScenarioMarketDataTest
	public class RepeatedScenarioMarketDataTest
	{

	  private static readonly LocalDate VAL_DATE = date(2015, 6, 30);
	  private static readonly TestingNamedId ID1 = new TestingNamedId("1");
	  private static readonly TestingNamedId ID2 = new TestingNamedId("2");
	  private static readonly TestingNamedId ID3 = new TestingNamedId("3");
	  private static readonly TestingObservableId ID4 = new TestingObservableId("4");
	  private const string VAL1 = "1";
	  private const string VAL2 = "2";
	  private static readonly LocalDateDoubleTimeSeries TIME_SERIES = LocalDateDoubleTimeSeries.builder().put(date(2011, 3, 8), 1.1).put(date(2011, 3, 10), 1.2).build();
	  private static readonly ImmutableMarketData BASE_DATA = baseData();

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		RepeatedScenarioMarketData test = RepeatedScenarioMarketData.of(2, BASE_DATA);
		assertEquals(test.ScenarioCount, 2);
		assertEquals(test.Underlying, BASE_DATA);
		assertEquals(test.ValuationDate, MarketDataBox.ofSingleValue(VAL_DATE));
		assertEquals(test.containsValue(ID1), true);
		assertEquals(test.containsValue(ID2), true);
		assertEquals(test.containsValue(ID3), false);
		assertEquals(test.getValue(ID1), MarketDataBox.ofSingleValue(VAL1));
		assertEquals(test.getValue(ID2), MarketDataBox.ofSingleValue(VAL2));
		assertThrows(() => test.getValue(ID3), typeof(MarketDataNotFoundException));
		assertEquals(test.findValue(ID1), MarketDataBox.ofSingleValue(VAL1));
		assertEquals(test.findValue(ID2), MarketDataBox.ofSingleValue(VAL2));
		assertEquals(test.findValue(ID3), null);
		assertEquals(test.Ids, ImmutableSet.of(ID1, ID2));
		assertEquals(test.findIds(ID1.MarketDataName), ImmutableSet.of(ID1));
		assertEquals(test.getTimeSeries(ID4), TIME_SERIES);
	  }

	  public virtual void test_scenarios()
	  {
		RepeatedScenarioMarketData test = RepeatedScenarioMarketData.of(2, BASE_DATA);
		assertEquals(test.scenarios().count(), 2);
		test.scenarios().forEach(md => assertSame(md, BASE_DATA));
	  }

	  public virtual void test_scenario_byIndex()
	  {
		RepeatedScenarioMarketData test = RepeatedScenarioMarketData.of(2, BASE_DATA);
		assertSame(test.scenario(0), BASE_DATA);
		assertSame(test.scenario(1), BASE_DATA);
		assertThrows(() => test.scenario(-1), typeof(System.IndexOutOfRangeException));
		assertThrows(() => test.scenario(2), typeof(System.IndexOutOfRangeException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		RepeatedScenarioMarketData test = RepeatedScenarioMarketData.of(2, BASE_DATA);
		coverImmutableBean(test);
		RepeatedScenarioMarketData test2 = RepeatedScenarioMarketData.of(1, baseData2());
		coverBeanEquals(test, test2);
	  }

	  public virtual void serialization()
	  {
		RepeatedScenarioMarketData test = RepeatedScenarioMarketData.of(2, BASE_DATA);
		assertSerialization(test);
	  }

	  //-------------------------------------------------------------------------
	  private static ImmutableMarketData baseData()
	  {
		return ImmutableMarketData.builder(VAL_DATE).addValue(ID1, VAL1).addValue(ID2, VAL2).addTimeSeriesMap(ImmutableMap.of(ID4, TIME_SERIES)).build();
	  }

	  private static ImmutableMarketData baseData2()
	  {
		return ImmutableMarketData.builder(VAL_DATE).addValue(ID1, VAL1).addTimeSeriesMap(ImmutableMap.of(ID4, TIME_SERIES)).build();
	  }

	}

}