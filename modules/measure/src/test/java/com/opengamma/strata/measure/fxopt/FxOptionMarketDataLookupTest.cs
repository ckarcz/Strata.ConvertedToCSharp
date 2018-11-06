/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
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
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using TestMarketDataMap = com.opengamma.strata.measure.curve.TestMarketDataMap;
	using FxOptionVolatilities = com.opengamma.strata.pricer.fxopt.FxOptionVolatilities;
	using FxOptionVolatilitiesId = com.opengamma.strata.pricer.fxopt.FxOptionVolatilitiesId;

	/// <summary>
	/// Test <seealso cref="FxOptionMarketDataLookup"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxOptionMarketDataLookupTest
	public class FxOptionMarketDataLookupTest
	{

	  private static readonly FxOptionVolatilitiesId VOL_ID1 = FxOptionVolatilitiesId.of("EURUSD1");
	  private static readonly FxOptionVolatilities MOCK_VOLS = mock(typeof(FxOptionVolatilities));
	  private static readonly MarketData MOCK_MARKET_DATA = mock(typeof(MarketData));
	  private static readonly ScenarioMarketData MOCK_CALC_MARKET_DATA = mock(typeof(ScenarioMarketData));
	  private static readonly CurrencyPair EUR_USD = CurrencyPair.of(EUR, USD);
	  private static readonly CurrencyPair GBP_USD = CurrencyPair.of(GBP, USD);
	  private static readonly CurrencyPair EUR_GBP = CurrencyPair.of(EUR, GBP);

	  static FxOptionMarketDataLookupTest()
	  {
		when(MOCK_MARKET_DATA.getValue(VOL_ID1)).thenReturn(MOCK_VOLS);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_single()
	  {
		FxOptionMarketDataLookup test = FxOptionMarketDataLookup.of(EUR_USD, VOL_ID1);
		assertEquals(test.queryType(), typeof(FxOptionMarketDataLookup));
		assertEquals(test.VolatilityCurrencyPairs, ImmutableSet.of(EUR_USD));
		assertEquals(test.getVolatilityIds(EUR_USD), ImmutableSet.of(VOL_ID1));
		assertThrowsIllegalArg(() => test.getVolatilityIds(GBP_USD));

		assertEquals(test.requirements(EUR_USD), FunctionRequirements.builder().valueRequirements(VOL_ID1).build());
		assertEquals(test.requirements(ImmutableSet.of(EUR_USD)), FunctionRequirements.builder().valueRequirements(VOL_ID1).build());
		assertThrowsIllegalArg(() => test.requirements(ImmutableSet.of(EUR_GBP)));
	  }

	  public virtual void test_of_map()
	  {
		ImmutableMap<CurrencyPair, FxOptionVolatilitiesId> ids = ImmutableMap.of(EUR_USD, VOL_ID1, GBP_USD, VOL_ID1);
		FxOptionMarketDataLookup test = FxOptionMarketDataLookup.of(ids);
		assertEquals(test.queryType(), typeof(FxOptionMarketDataLookup));
		assertEquals(test.VolatilityCurrencyPairs, ImmutableSet.of(EUR_USD, GBP_USD));
		assertEquals(test.getVolatilityIds(EUR_USD), ImmutableSet.of(VOL_ID1));
		assertThrowsIllegalArg(() => test.getVolatilityIds(EUR_GBP));

		assertEquals(test.requirements(EUR_USD), FunctionRequirements.builder().valueRequirements(VOL_ID1).build());
		assertEquals(test.requirements(ImmutableSet.of(EUR_USD)), FunctionRequirements.builder().valueRequirements(VOL_ID1).build());
		assertThrowsIllegalArg(() => test.requirements(ImmutableSet.of(EUR_GBP)));

		assertEquals(test.volatilities(EUR_USD, MOCK_MARKET_DATA), MOCK_VOLS);
		assertThrowsIllegalArg(() => test.volatilities(EUR_GBP, MOCK_MARKET_DATA));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_marketDataView()
	  {
		FxOptionMarketDataLookup test = FxOptionMarketDataLookup.of(EUR_USD, VOL_ID1);
		LocalDate valDate = date(2015, 6, 30);
		ScenarioMarketData md = new TestMarketDataMap(valDate, ImmutableMap.of(), ImmutableMap.of());
		FxOptionScenarioMarketData multiScenario = test.marketDataView(md);
		assertEquals(multiScenario.Lookup, test);
		assertEquals(multiScenario.MarketData, md);
		assertEquals(multiScenario.ScenarioCount, 1);
		FxOptionMarketData scenario = multiScenario.scenario(0);
		assertEquals(scenario.Lookup, test);
		assertEquals(scenario.MarketData, md.scenario(0));
		assertEquals(scenario.ValuationDate, valDate);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		DefaultFxOptionMarketDataLookup test = DefaultFxOptionMarketDataLookup.of(ImmutableMap.of(EUR_USD, VOL_ID1, GBP_USD, VOL_ID1));
		coverImmutableBean(test);
		DefaultFxOptionMarketDataLookup test2 = DefaultFxOptionMarketDataLookup.of(EUR_USD, VOL_ID1);
		coverBeanEquals(test, test2);

		coverImmutableBean((ImmutableBean) test.marketDataView(MOCK_CALC_MARKET_DATA));
		coverImmutableBean((ImmutableBean) test.marketDataView(MOCK_MARKET_DATA));
	  }

	  public virtual void test_serialization()
	  {
		DefaultFxOptionMarketDataLookup test = DefaultFxOptionMarketDataLookup.of(ImmutableMap.of(EUR_USD, VOL_ID1, GBP_USD, VOL_ID1));
		assertSerialization(test);
	  }

	}

}