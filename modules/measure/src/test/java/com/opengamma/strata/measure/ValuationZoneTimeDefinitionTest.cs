﻿/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;

	/// <summary>
	/// Test <seealso cref="ValuationZoneTimeDefinition"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ValuationZoneTimeDefinitionTest
	public class ValuationZoneTimeDefinitionTest
	{

	  private static readonly LocalTime LOCAL_TIME_1 = LocalTime.of(12, 20);
	  private static readonly LocalTime LOCAL_TIME_2 = LocalTime.of(10, 10);
	  private static readonly LocalTime LOCAL_TIME_3 = LocalTime.of(9, 35);
	  private static readonly LocalTime LOCAL_TIME_4 = LocalTime.of(15, 12);
	  private static readonly ZoneId ZONE_ID = ZoneId.of("America/Chicago");

	  public virtual void test_of()
	  {
		ValuationZoneTimeDefinition test = ValuationZoneTimeDefinition.of(LocalTime.MIDNIGHT, ZONE_ID, LOCAL_TIME_1, LOCAL_TIME_2, LOCAL_TIME_3);
		assertEquals(test.LocalTimes, ImmutableList.of(LOCAL_TIME_1, LOCAL_TIME_2, LOCAL_TIME_3));
		assertEquals(test.ZoneId, ZONE_ID);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toZonedDateTime_scenario()
	  {
		ValuationZoneTimeDefinition test = ValuationZoneTimeDefinition.of(LocalTime.MIDNIGHT, ZONE_ID, LOCAL_TIME_1, LOCAL_TIME_2, LOCAL_TIME_3);
		MarketDataBox<LocalDate> dates = MarketDataBox.ofScenarioValues(LocalDate.of(2016, 10, 21), LocalDate.of(2016, 10, 22), LocalDate.of(2016, 10, 23));
		MarketDataBox<ZonedDateTime> computed = test.toZonedDateTime(dates);
		MarketDataBox<ZonedDateTime> expected = MarketDataBox.ofScenarioValue(ScenarioArray.of(dates.getValue(0).atTime(LOCAL_TIME_1).atZone(ZONE_ID), dates.getValue(1).atTime(LOCAL_TIME_2).atZone(ZONE_ID), dates.getValue(2).atTime(LOCAL_TIME_3).atZone(ZONE_ID)));
		assertEquals(computed, expected);
	  }

	  public virtual void test_toZonedDateTime_scenario_default()
	  {
		ValuationZoneTimeDefinition test = ValuationZoneTimeDefinition.of(LOCAL_TIME_1, ZONE_ID);
		MarketDataBox<LocalDate> dates = MarketDataBox.ofScenarioValues(LocalDate.of(2016, 10, 21), LocalDate.of(2016, 10, 22), LocalDate.of(2016, 10, 23));
		MarketDataBox<ZonedDateTime> computed = test.toZonedDateTime(dates);
		MarketDataBox<ZonedDateTime> expected = MarketDataBox.ofScenarioValue(ScenarioArray.of(dates.getValue(0).atTime(LOCAL_TIME_1).atZone(ZONE_ID), dates.getValue(1).atTime(LOCAL_TIME_1).atZone(ZONE_ID), dates.getValue(2).atTime(LOCAL_TIME_1).atZone(ZONE_ID)));
		assertEquals(computed, expected);
	  }

	  public virtual void test_toZonedDateTime_scenario_long()
	  {
		ValuationZoneTimeDefinition test = ValuationZoneTimeDefinition.of(LOCAL_TIME_1, ZONE_ID, LOCAL_TIME_1, LOCAL_TIME_2);
		MarketDataBox<LocalDate> dates = MarketDataBox.ofScenarioValues(LocalDate.of(2016, 10, 21), LocalDate.of(2016, 10, 22), LocalDate.of(2016, 10, 23));
		MarketDataBox<ZonedDateTime> computed = test.toZonedDateTime(dates);
		MarketDataBox<ZonedDateTime> expected = MarketDataBox.ofScenarioValue(ScenarioArray.of(dates.getValue(0).atTime(LOCAL_TIME_1).atZone(ZONE_ID), dates.getValue(1).atTime(LOCAL_TIME_2).atZone(ZONE_ID), dates.getValue(2).atTime(LOCAL_TIME_1).atZone(ZONE_ID)));
		assertEquals(computed, expected);
	  }

	  public virtual void test_toZonedDateTime_single()
	  {
		ValuationZoneTimeDefinition test = ValuationZoneTimeDefinition.of(LOCAL_TIME_4, ZONE_ID);
		MarketDataBox<LocalDate> dates = MarketDataBox.ofSingleValue(LocalDate.of(2016, 10, 21));
		MarketDataBox<ZonedDateTime> computed = test.toZonedDateTime(dates);
		MarketDataBox<ZonedDateTime> expected = MarketDataBox.ofSingleValue(dates.SingleValue.atTime(LOCAL_TIME_4).atZone(ZONE_ID));
		assertEquals(computed, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ValuationZoneTimeDefinition test1 = ValuationZoneTimeDefinition.of(LOCAL_TIME_1, ZONE_ID, LOCAL_TIME_2);
		coverImmutableBean(test1);
		ValuationZoneTimeDefinition test2 = ValuationZoneTimeDefinition.of(LOCAL_TIME_4, ZoneId.of("Europe/London"));
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		ValuationZoneTimeDefinition test = ValuationZoneTimeDefinition.of(LOCAL_TIME_1, ZONE_ID);
		assertSerialization(test);
	  }

	}

}