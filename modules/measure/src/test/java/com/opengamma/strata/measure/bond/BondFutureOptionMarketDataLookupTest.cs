/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.bond
{
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
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using TestMarketDataMap = com.opengamma.strata.measure.curve.TestMarketDataMap;
	using BondFutureVolatilities = com.opengamma.strata.pricer.bond.BondFutureVolatilities;
	using BondFutureVolatilitiesId = com.opengamma.strata.pricer.bond.BondFutureVolatilitiesId;
	using SecurityId = com.opengamma.strata.product.SecurityId;

	/// <summary>
	/// Test <seealso cref="BondFutureOptionMarketDataLookup"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BondFutureOptionMarketDataLookupTest
	public class BondFutureOptionMarketDataLookupTest
	{

	  private static readonly BondFutureVolatilitiesId VOL_ID1 = BondFutureVolatilitiesId.of("ID1");
	  private static readonly BondFutureVolatilities MOCK_VOLS = mock(typeof(BondFutureVolatilities));
	  private static readonly MarketData MOCK_MARKET_DATA = mock(typeof(MarketData));
	  private static readonly ScenarioMarketData MOCK_CALC_MARKET_DATA = mock(typeof(ScenarioMarketData));
	  private static readonly SecurityId SEC_OG1 = SecurityId.of("OG", "1");
	  private static readonly SecurityId SEC_OG2 = SecurityId.of("OG", "2");
	  private static readonly SecurityId SEC_OG3 = SecurityId.of("OG", "3");

	  static BondFutureOptionMarketDataLookupTest()
	  {
		when(MOCK_MARKET_DATA.getValue(VOL_ID1)).thenReturn(MOCK_VOLS);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_single()
	  {
		BondFutureOptionMarketDataLookup test = BondFutureOptionMarketDataLookup.of(SEC_OG1, VOL_ID1);
		assertEquals(test.queryType(), typeof(BondFutureOptionMarketDataLookup));
		assertEquals(test.VolatilitySecurityIds, ImmutableSet.of(SEC_OG1));
		assertEquals(test.getVolatilityIds(SEC_OG1), ImmutableSet.of(VOL_ID1));
		assertThrowsIllegalArg(() => test.getVolatilityIds(SEC_OG2));

		assertEquals(test.requirements(SEC_OG1), FunctionRequirements.builder().valueRequirements(VOL_ID1).build());
		assertEquals(test.requirements(ImmutableSet.of(SEC_OG1)), FunctionRequirements.builder().valueRequirements(VOL_ID1).build());
		assertThrowsIllegalArg(() => test.requirements(ImmutableSet.of(SEC_OG3)));
	  }

	  public virtual void test_of_map()
	  {
		ImmutableMap<SecurityId, BondFutureVolatilitiesId> ids = ImmutableMap.of(SEC_OG1, VOL_ID1, SEC_OG2, VOL_ID1);
		BondFutureOptionMarketDataLookup test = BondFutureOptionMarketDataLookup.of(ids);
		assertEquals(test.queryType(), typeof(BondFutureOptionMarketDataLookup));
		assertEquals(test.VolatilitySecurityIds, ImmutableSet.of(SEC_OG1, SEC_OG2));
		assertEquals(test.getVolatilityIds(SEC_OG1), ImmutableSet.of(VOL_ID1));
		assertThrowsIllegalArg(() => test.getVolatilityIds(SEC_OG3));

		assertEquals(test.requirements(SEC_OG1), FunctionRequirements.builder().valueRequirements(VOL_ID1).build());
		assertEquals(test.requirements(ImmutableSet.of(SEC_OG1)), FunctionRequirements.builder().valueRequirements(VOL_ID1).build());
		assertThrowsIllegalArg(() => test.requirements(ImmutableSet.of(SEC_OG3)));

		assertEquals(test.volatilities(SEC_OG1, MOCK_MARKET_DATA), MOCK_VOLS);
		assertThrowsIllegalArg(() => test.volatilities(SEC_OG3, MOCK_MARKET_DATA));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_marketDataView()
	  {
		BondFutureOptionMarketDataLookup test = BondFutureOptionMarketDataLookup.of(SEC_OG1, VOL_ID1);
		LocalDate valDate = date(2015, 6, 30);
		ScenarioMarketData md = new TestMarketDataMap(valDate, ImmutableMap.of(), ImmutableMap.of());
		BondFutureOptionScenarioMarketData multiScenario = test.marketDataView(md);
		assertEquals(multiScenario.Lookup, test);
		assertEquals(multiScenario.MarketData, md);
		assertEquals(multiScenario.ScenarioCount, 1);
		BondFutureOptionMarketData scenario = multiScenario.scenario(0);
		assertEquals(scenario.Lookup, test);
		assertEquals(scenario.MarketData, md.scenario(0));
		assertEquals(scenario.ValuationDate, valDate);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		DefaultBondFutureOptionMarketDataLookup test = DefaultBondFutureOptionMarketDataLookup.of(ImmutableMap.of(SEC_OG1, VOL_ID1, SEC_OG2, VOL_ID1));
		coverImmutableBean(test);
		DefaultBondFutureOptionMarketDataLookup test2 = DefaultBondFutureOptionMarketDataLookup.of(SEC_OG1, VOL_ID1);
		coverBeanEquals(test, test2);

		coverImmutableBean((ImmutableBean) test.marketDataView(MOCK_CALC_MARKET_DATA));
		coverImmutableBean((ImmutableBean) test.marketDataView(MOCK_MARKET_DATA));
	  }

	  public virtual void test_serialization()
	  {
		DefaultBondFutureOptionMarketDataLookup test = DefaultBondFutureOptionMarketDataLookup.of(ImmutableMap.of(SEC_OG1, VOL_ID1, SEC_OG2, VOL_ID1));
		assertSerialization(test);
	  }

	}

}