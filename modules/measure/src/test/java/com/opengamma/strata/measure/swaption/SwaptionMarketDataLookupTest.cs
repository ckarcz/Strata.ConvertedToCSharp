/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.swaption
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.mock;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.when;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using TestMarketDataMap = com.opengamma.strata.measure.curve.TestMarketDataMap;
	using SwaptionVolatilities = com.opengamma.strata.pricer.swaption.SwaptionVolatilities;
	using SwaptionVolatilitiesId = com.opengamma.strata.pricer.swaption.SwaptionVolatilitiesId;

	/// <summary>
	/// Test <seealso cref="SwaptionMarketDataLookup"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SwaptionMarketDataLookupTest
	public class SwaptionMarketDataLookupTest
	{

	  private static readonly SwaptionVolatilitiesId VOL_ID1 = SwaptionVolatilitiesId.of("USD1");
	  private static readonly SwaptionVolatilities MOCK_VOLS = mock(typeof(SwaptionVolatilities));
	  private static readonly MarketData MOCK_MARKET_DATA = mock(typeof(MarketData));
	  private static readonly ScenarioMarketData MOCK_CALC_MARKET_DATA = mock(typeof(ScenarioMarketData));

	  static SwaptionMarketDataLookupTest()
	  {
		when(MOCK_MARKET_DATA.getValue(VOL_ID1)).thenReturn(MOCK_VOLS);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_single()
	  {
		SwaptionMarketDataLookup test = SwaptionMarketDataLookup.of(USD_LIBOR_3M, VOL_ID1);
		assertEquals(test.queryType(), typeof(SwaptionMarketDataLookup));
		assertEquals(test.VolatilityIndices, ImmutableSet.of(USD_LIBOR_3M));
		assertEquals(test.getVolatilityIds(USD_LIBOR_3M), ImmutableSet.of(VOL_ID1));
		assertThrowsIllegalArg(() => test.getVolatilityIds(GBP_LIBOR_3M));

		assertEquals(test.requirements(USD_LIBOR_3M), FunctionRequirements.builder().valueRequirements(VOL_ID1).build());
		assertEquals(test.requirements(ImmutableSet.of(USD_LIBOR_3M)), FunctionRequirements.builder().valueRequirements(VOL_ID1).build());
		assertThrowsIllegalArg(() => test.requirements(ImmutableSet.of(GBP_LIBOR_3M)));
	  }

	  public virtual void test_of_map()
	  {
		ImmutableMap<IborIndex, SwaptionVolatilitiesId> ids = ImmutableMap.of(USD_LIBOR_3M, VOL_ID1, USD_LIBOR_6M, VOL_ID1);
		SwaptionMarketDataLookup test = SwaptionMarketDataLookup.of(ids);
		assertEquals(test.queryType(), typeof(SwaptionMarketDataLookup));
		assertEquals(test.VolatilityIndices, ImmutableSet.of(USD_LIBOR_3M, USD_LIBOR_6M));
		assertEquals(test.getVolatilityIds(USD_LIBOR_3M), ImmutableSet.of(VOL_ID1));
		assertThrowsIllegalArg(() => test.getVolatilityIds(GBP_LIBOR_3M));

		assertEquals(test.requirements(USD_LIBOR_3M), FunctionRequirements.builder().valueRequirements(VOL_ID1).build());
		assertEquals(test.requirements(ImmutableSet.of(USD_LIBOR_3M)), FunctionRequirements.builder().valueRequirements(VOL_ID1).build());
		assertThrowsIllegalArg(() => test.requirements(ImmutableSet.of(GBP_LIBOR_3M)));

		assertEquals(test.volatilities(USD_LIBOR_3M, MOCK_MARKET_DATA), MOCK_VOLS);
		assertThrowsIllegalArg(() => test.volatilities(GBP_LIBOR_3M, MOCK_MARKET_DATA));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_marketDataView()
	  {
		SwaptionMarketDataLookup test = SwaptionMarketDataLookup.of(USD_LIBOR_3M, VOL_ID1);
		LocalDate valDate = date(2015, 6, 30);
		ScenarioMarketData md = new TestMarketDataMap(valDate, ImmutableMap.of(), ImmutableMap.of());
		SwaptionScenarioMarketData multiScenario = test.marketDataView(md);
		assertEquals(multiScenario.Lookup, test);
		assertEquals(multiScenario.MarketData, md);
		assertEquals(multiScenario.ScenarioCount, 1);
		SwaptionMarketData scenario = multiScenario.scenario(0);
		assertEquals(scenario.Lookup, test);
		assertEquals(scenario.MarketData, md.scenario(0));
		assertEquals(scenario.ValuationDate, valDate);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		DefaultSwaptionMarketDataLookup test = DefaultSwaptionMarketDataLookup.of(ImmutableMap.of(USD_LIBOR_3M, VOL_ID1, USD_LIBOR_6M, VOL_ID1));
		coverImmutableBean(test);
		DefaultSwaptionMarketDataLookup test2 = DefaultSwaptionMarketDataLookup.of(USD_LIBOR_3M, VOL_ID1);
		coverBeanEquals(test, test2);

		coverImmutableBean((ImmutableBean) test.marketDataView(MOCK_CALC_MARKET_DATA));
		coverImmutableBean((ImmutableBean) test.marketDataView(MOCK_MARKET_DATA));
	  }

	  public virtual void test_serialization()
	  {
		DefaultSwaptionMarketDataLookup test = DefaultSwaptionMarketDataLookup.of(ImmutableMap.of(USD_LIBOR_3M, VOL_ID1, USD_LIBOR_6M, VOL_ID1));
		assertSerialization(test);
	  }

	}

}