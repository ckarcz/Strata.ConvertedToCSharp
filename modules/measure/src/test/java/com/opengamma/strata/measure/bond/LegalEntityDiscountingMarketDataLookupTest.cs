/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
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
//	import static org.testng.Assert.assertEquals;


	using ImmutableBean = org.joda.beans.ImmutableBean;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using LegalEntityGroup = com.opengamma.strata.market.curve.LegalEntityGroup;
	using RepoGroup = com.opengamma.strata.market.curve.RepoGroup;
	using TestMarketDataMap = com.opengamma.strata.measure.curve.TestMarketDataMap;
	using SimpleDiscountFactors = com.opengamma.strata.pricer.SimpleDiscountFactors;
	using IssuerCurveDiscountFactors = com.opengamma.strata.pricer.bond.IssuerCurveDiscountFactors;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;
	using RepoCurveDiscountFactors = com.opengamma.strata.pricer.bond.RepoCurveDiscountFactors;
	using LegalEntityId = com.opengamma.strata.product.LegalEntityId;
	using SecurityId = com.opengamma.strata.product.SecurityId;

	/// <summary>
	/// Test <seealso cref="LegalEntityDiscountingMarketDataLookup"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LegalEntityDiscountingMarketDataLookupTest
	public class LegalEntityDiscountingMarketDataLookupTest
	{

	  private static readonly SecurityId SEC_A1 = SecurityId.of("OG-Bond", "A1");
	  private static readonly SecurityId SEC_A2 = SecurityId.of("OG-Bond", "A2");
	  private static readonly SecurityId SEC_B1 = SecurityId.of("OG-Bond", "B1");
	  private static readonly SecurityId SEC_C1 = SecurityId.of("OG-Bond", "C1");
	  private static readonly SecurityId SEC_D1 = SecurityId.of("OG-Bond", "D1");
	  private static readonly LegalEntityId ISSUER_A = LegalEntityId.of("OG-LegEnt", "A");
	  private static readonly LegalEntityId ISSUER_B = LegalEntityId.of("OG-LegEnt", "B");
	  private static readonly LegalEntityId ISSUER_C = LegalEntityId.of("OG-LegEnt", "C");
	  private static readonly LegalEntityId ISSUER_D = LegalEntityId.of("OG-LegEnt", "D");
	  private static readonly RepoGroup GROUP_REPO_X = RepoGroup.of("X");
	  private static readonly RepoGroup GROUP_REPO_Y = RepoGroup.of("Y");
	  private static readonly LegalEntityGroup GROUP_ISSUER_M = LegalEntityGroup.of("M");
	  private static readonly LegalEntityGroup GROUP_ISSUER_N = LegalEntityGroup.of("N");

	  private static readonly CurveId CURVE_ID_USD1 = CurveId.of("Group", "USD1");
	  private static readonly CurveId CURVE_ID_USD2 = CurveId.of("Group", "USD2");
	  private static readonly CurveId CURVE_ID_USD3 = CurveId.of("Group", "USD3");
	  private static readonly CurveId CURVE_ID_USD4 = CurveId.of("Group", "USD4");
	  private static readonly CurveId CURVE_ID_GBP1 = CurveId.of("Group", "GBP1");
	  private static readonly CurveId CURVE_ID_GBP2 = CurveId.of("Group", "GBP2");
	  private static readonly ObservableSource OBS_SOURCE = ObservableSource.of("Vendor");
	  private static readonly MarketData MOCK_MARKET_DATA = mock(typeof(MarketData));
	  private static readonly ScenarioMarketData MOCK_CALC_MARKET_DATA = mock(typeof(ScenarioMarketData));

	  //-------------------------------------------------------------------------
	  public virtual void test_of_map()
	  {
		ImmutableMap<SecurityId, RepoGroup> repoSecurityGroups = ImmutableMap.of(SEC_A1, GROUP_REPO_X);
		ImmutableMap<LegalEntityId, RepoGroup> repoGroups = ImmutableMap.of(ISSUER_A, GROUP_REPO_Y, ISSUER_B, GROUP_REPO_Y, ISSUER_C, GROUP_REPO_Y, ISSUER_D, GROUP_REPO_Y);
		ImmutableMap<Pair<RepoGroup, Currency>, CurveId> repoCurves = ImmutableMap.of(Pair.of(GROUP_REPO_X, USD), CURVE_ID_USD1, Pair.of(GROUP_REPO_Y, USD), CURVE_ID_USD2, Pair.of(GROUP_REPO_Y, GBP), CURVE_ID_GBP1);

		ImmutableMap<LegalEntityId, LegalEntityGroup> issuerGroups = ImmutableMap.of(ISSUER_A, GROUP_ISSUER_M, ISSUER_B, GROUP_ISSUER_N, ISSUER_C, GROUP_ISSUER_M);
		ImmutableMap<Pair<LegalEntityGroup, Currency>, CurveId> issuerCurves = ImmutableMap.of(Pair.of(GROUP_ISSUER_M, USD), CURVE_ID_USD3, Pair.of(GROUP_ISSUER_N, USD), CURVE_ID_USD4, Pair.of(GROUP_ISSUER_N, GBP), CURVE_ID_GBP2);

		LegalEntityDiscountingMarketDataLookup test = LegalEntityDiscountingMarketDataLookup.of(repoSecurityGroups, repoGroups, repoCurves, issuerGroups, issuerCurves);
		assertEquals(test.queryType(), typeof(LegalEntityDiscountingMarketDataLookup));

		assertEquals(test.requirements(SEC_A1, ISSUER_A, USD), FunctionRequirements.builder().valueRequirements(CURVE_ID_USD1, CURVE_ID_USD3).outputCurrencies(USD).build());
		assertEquals(test.requirements(SEC_A2, ISSUER_A, USD), FunctionRequirements.builder().valueRequirements(CURVE_ID_USD2, CURVE_ID_USD3).outputCurrencies(USD).build());
		assertEquals(test.requirements(SEC_B1, ISSUER_B, USD), FunctionRequirements.builder().valueRequirements(CURVE_ID_USD2, CURVE_ID_USD4).outputCurrencies(USD).build());
		assertEquals(test.requirements(SEC_B1, ISSUER_B, GBP), FunctionRequirements.builder().valueRequirements(CURVE_ID_GBP1, CURVE_ID_GBP2).outputCurrencies(GBP).build());
		assertThrowsIllegalArg(() => test.requirements(SEC_B1, LegalEntityId.of("XXX", "XXX"), GBP));
		assertThrowsIllegalArg(() => test.requirements(SecurityId.of("XXX", "XXX"), LegalEntityId.of("XXX", "XXX"), GBP));
		assertThrowsIllegalArg(() => test.requirements(SEC_A1, ISSUER_A, GBP));
		assertThrowsIllegalArg(() => test.requirements(SEC_C1, ISSUER_C, GBP));
		assertThrowsIllegalArg(() => test.requirements(SEC_D1, ISSUER_D, GBP));

		assertEquals(test.discountingProvider(MOCK_MARKET_DATA), DefaultLookupLegalEntityDiscountingProvider.of((DefaultLegalEntityDiscountingMarketDataLookup) test, MOCK_MARKET_DATA));
	  }

	  public virtual void test_of_repoMap()
	  {
		ImmutableMap<LegalEntityId, RepoGroup> repoGroups = ImmutableMap.of(ISSUER_A, GROUP_REPO_X, ISSUER_B, GROUP_REPO_Y, ISSUER_C, GROUP_REPO_Y, ISSUER_D, GROUP_REPO_Y);
		ImmutableMap<Pair<RepoGroup, Currency>, CurveId> repoCurves = ImmutableMap.of(Pair.of(GROUP_REPO_X, USD), CURVE_ID_USD1, Pair.of(GROUP_REPO_Y, USD), CURVE_ID_USD2, Pair.of(GROUP_REPO_Y, GBP), CURVE_ID_GBP1);
		LegalEntityDiscountingMarketDataLookup test = LegalEntityDiscountingMarketDataLookup.of(repoGroups, repoCurves);
		assertEquals(test.queryType(), typeof(LegalEntityDiscountingMarketDataLookup));

		assertEquals(test.requirements(ISSUER_A, USD), FunctionRequirements.builder().valueRequirements(CURVE_ID_USD1).outputCurrencies(USD).build());
		assertEquals(test.requirements(ISSUER_B, USD), FunctionRequirements.builder().valueRequirements(CURVE_ID_USD2).outputCurrencies(USD).build());
		assertEquals(test.requirements(ISSUER_B, GBP), FunctionRequirements.builder().valueRequirements(CURVE_ID_GBP1).outputCurrencies(GBP).build());
		assertThrowsIllegalArg(() => test.requirements(SEC_A2, ISSUER_A, USD));
		assertThrowsIllegalArg(() => test.requirements(LegalEntityId.of("XXX", "XXX"), GBP));
		assertThrowsIllegalArg(() => test.requirements(ISSUER_A, GBP));
		assertEquals(test.discountingProvider(MOCK_MARKET_DATA), DefaultLookupLegalEntityDiscountingProvider.of((DefaultLegalEntityDiscountingMarketDataLookup) test, MOCK_MARKET_DATA));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_map_invalid()
	  {
		ImmutableMap<SecurityId, RepoGroup> repoSecurityGroups = ImmutableMap.of(SEC_A1, GROUP_REPO_X);
		ImmutableMap<Pair<RepoGroup, Currency>, CurveId> repoCurves = ImmutableMap.of(Pair.of(GROUP_REPO_X, USD), CURVE_ID_USD1);

		ImmutableMap<LegalEntityId, LegalEntityGroup> issuerGroups = ImmutableMap.of(ISSUER_A, GROUP_ISSUER_M);
		ImmutableMap<Pair<LegalEntityGroup, Currency>, CurveId> issuerCurves = ImmutableMap.of(Pair.of(GROUP_ISSUER_M, USD), CURVE_ID_USD3);

		assertThrowsIllegalArg(() => LegalEntityDiscountingMarketDataLookup.of(repoSecurityGroups, ImmutableMap.of(), ImmutableMap.of(), issuerGroups, issuerCurves));
		assertThrowsIllegalArg(() => LegalEntityDiscountingMarketDataLookup.of(repoSecurityGroups, ImmutableMap.of(), repoCurves, issuerGroups, ImmutableMap.of()));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_marketDataView()
	  {
		ImmutableMap<SecurityId, RepoGroup> repoSecurityGroups = ImmutableMap.of(SEC_A1, GROUP_REPO_X);
		ImmutableMap<Pair<RepoGroup, Currency>, CurveId> repoCurves = ImmutableMap.of(Pair.of(GROUP_REPO_X, USD), CURVE_ID_USD1);

		ImmutableMap<LegalEntityId, LegalEntityGroup> issuerGroups = ImmutableMap.of(ISSUER_A, GROUP_ISSUER_M);
		ImmutableMap<Pair<LegalEntityGroup, Currency>, CurveId> issuerCurves = ImmutableMap.of(Pair.of(GROUP_ISSUER_M, USD), CURVE_ID_USD3);

		LegalEntityDiscountingMarketDataLookup test = LegalEntityDiscountingMarketDataLookup.of(repoSecurityGroups, ImmutableMap.of(), repoCurves, issuerGroups, issuerCurves);

		LocalDate valDate = date(2015, 6, 30);
		ScenarioMarketData md = new TestMarketDataMap(valDate, ImmutableMap.of(), ImmutableMap.of());
		LegalEntityDiscountingScenarioMarketData multiScenario = test.marketDataView(md);
		assertEquals(multiScenario.Lookup, test);
		assertEquals(multiScenario.MarketData, md);
		assertEquals(multiScenario.ScenarioCount, 1);
		LegalEntityDiscountingMarketData scenario = multiScenario.scenario(0);
		assertEquals(scenario.Lookup, test);
		assertEquals(scenario.MarketData, md.scenario(0));
		assertEquals(scenario.ValuationDate, valDate);
	  }

	  public virtual void test_bondDiscountingProvider()
	  {
		ImmutableMap<SecurityId, RepoGroup> repoSecurityGroups = ImmutableMap.of(SEC_A1, GROUP_REPO_X);
		ImmutableMap<LegalEntityId, RepoGroup> repoGroups = ImmutableMap.of(ISSUER_B, GROUP_REPO_X);
		ImmutableMap<Pair<RepoGroup, Currency>, CurveId> repoCurves = ImmutableMap.of(Pair.of(GROUP_REPO_X, USD), CURVE_ID_USD1);

		ImmutableMap<LegalEntityId, LegalEntityGroup> issuerGroups = ImmutableMap.of(ISSUER_A, GROUP_ISSUER_M);
		ImmutableMap<Pair<LegalEntityGroup, Currency>, CurveId> issuerCurves = ImmutableMap.of(Pair.of(GROUP_ISSUER_M, USD), CURVE_ID_USD3);

		LegalEntityDiscountingMarketDataLookup test = LegalEntityDiscountingMarketDataLookup.of(repoSecurityGroups, repoGroups, repoCurves, issuerGroups, issuerCurves);
		LocalDate valDate = date(2015, 6, 30);
		Curve repoCurve = ConstantCurve.of(Curves.discountFactors(CURVE_ID_USD1.CurveName, ACT_360), 1d);
		Curve issuerCurve = ConstantCurve.of(Curves.discountFactors(CURVE_ID_USD3.CurveName, ACT_360), 2d);
		MarketData md = ImmutableMarketData.of(valDate, ImmutableMap.of(CURVE_ID_USD1, repoCurve, CURVE_ID_USD3, issuerCurve));
		LegalEntityDiscountingProvider provider = test.discountingProvider(md);

		assertEquals(provider.ValuationDate, valDate);
		assertEquals(provider.findData(CURVE_ID_USD1.CurveName), repoCurve);
		assertEquals(provider.findData(CURVE_ID_USD3.CurveName), issuerCurve);
		assertEquals(provider.findData(CurveName.of("Rubbish")), null);
		// check repo
		RepoCurveDiscountFactors rcdf = provider.repoCurveDiscountFactors(SEC_A1, ISSUER_A, USD);
		SimpleDiscountFactors rdf = (SimpleDiscountFactors) rcdf.DiscountFactors;
		assertEquals(rdf.Curve.Name, repoCurve.Name);
		assertEquals(rcdf, provider.repoCurveDiscountFactors(SEC_B1, ISSUER_B, USD));
		assertThrowsIllegalArg(() => provider.repoCurveDiscountFactors(SEC_A1, ISSUER_A, GBP));
		assertThrowsIllegalArg(() => provider.repoCurveDiscountFactors(SEC_C1, ISSUER_C, USD));
		// check issuer
		IssuerCurveDiscountFactors icdf = provider.issuerCurveDiscountFactors(ISSUER_A, USD);
		SimpleDiscountFactors idf = (SimpleDiscountFactors) icdf.DiscountFactors;
		assertEquals(idf.Curve.Name, issuerCurve.Name);
		assertThrowsIllegalArg(() => provider.issuerCurveDiscountFactors(ISSUER_A, GBP));
		assertThrowsIllegalArg(() => provider.issuerCurveDiscountFactors(ISSUER_C, USD));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ImmutableMap<SecurityId, RepoGroup> repoSecurityGroups = ImmutableMap.of(SEC_A1, GROUP_REPO_X);
		ImmutableMap<LegalEntityId, RepoGroup> repoGroups = ImmutableMap.of(ISSUER_A, GROUP_REPO_Y, ISSUER_B, GROUP_REPO_Y);
		ImmutableMap<Pair<RepoGroup, Currency>, CurveId> repoCurves = ImmutableMap.of(Pair.of(GROUP_REPO_X, USD), CURVE_ID_USD1, Pair.of(GROUP_REPO_Y, USD), CURVE_ID_USD2, Pair.of(GROUP_REPO_Y, GBP), CURVE_ID_GBP1);

		ImmutableMap<LegalEntityId, LegalEntityGroup> issuerGroups = ImmutableMap.of(ISSUER_A, GROUP_ISSUER_M, ISSUER_B, GROUP_ISSUER_N);
		ImmutableMap<Pair<LegalEntityGroup, Currency>, CurveId> issuerCurves = ImmutableMap.of(Pair.of(GROUP_ISSUER_M, USD), CURVE_ID_USD3, Pair.of(GROUP_ISSUER_N, USD), CURVE_ID_USD4, Pair.of(GROUP_ISSUER_N, GBP), CURVE_ID_GBP2);

		LegalEntityDiscountingMarketDataLookup test = LegalEntityDiscountingMarketDataLookup.of(repoSecurityGroups, repoGroups, repoCurves, issuerGroups, issuerCurves);
		coverImmutableBean((ImmutableBean) test);

		ImmutableMap<LegalEntityId, RepoGroup> repoGroups2 = ImmutableMap.of();
		ImmutableMap<Pair<RepoGroup, Currency>, CurveId> repoCurves2 = ImmutableMap.of();
		ImmutableMap<LegalEntityId, LegalEntityGroup> issuerGroups2 = ImmutableMap.of();
		ImmutableMap<Pair<LegalEntityGroup, Currency>, CurveId> issuerCurves2 = ImmutableMap.of();

		LegalEntityDiscountingMarketDataLookup test2 = LegalEntityDiscountingMarketDataLookup.of(repoGroups2, repoCurves2, issuerGroups2, issuerCurves2, OBS_SOURCE);
		coverBeanEquals((ImmutableBean) test, (ImmutableBean) test2);

		// related coverage
		coverImmutableBean((ImmutableBean) test.marketDataView(MOCK_CALC_MARKET_DATA));
		DefaultLegalEntityDiscountingScenarioMarketData.meta();

		coverImmutableBean((ImmutableBean) test.marketDataView(MOCK_MARKET_DATA));
		DefaultLegalEntityDiscountingMarketData.meta();

		coverImmutableBean((ImmutableBean) test.marketDataView(MOCK_MARKET_DATA).discountingProvider());
		DefaultLookupLegalEntityDiscountingProvider.meta();
	  }

	  public virtual void test_serialization()
	  {
		ImmutableMap<SecurityId, RepoGroup> repoSecurityGroups = ImmutableMap.of(SEC_A1, GROUP_REPO_X);
		ImmutableMap<LegalEntityId, RepoGroup> repoGroups = ImmutableMap.of(ISSUER_A, GROUP_REPO_Y, ISSUER_B, GROUP_REPO_Y);
		ImmutableMap<Pair<RepoGroup, Currency>, CurveId> repoCurves = ImmutableMap.of(Pair.of(GROUP_REPO_X, USD), CURVE_ID_USD1, Pair.of(GROUP_REPO_Y, USD), CURVE_ID_USD2, Pair.of(GROUP_REPO_Y, GBP), CURVE_ID_GBP1);

		ImmutableMap<LegalEntityId, LegalEntityGroup> issuerGroups = ImmutableMap.of(ISSUER_A, GROUP_ISSUER_M, ISSUER_B, GROUP_ISSUER_N);
		ImmutableMap<Pair<LegalEntityGroup, Currency>, CurveId> issuerCurves = ImmutableMap.of(Pair.of(GROUP_ISSUER_M, USD), CURVE_ID_USD3, Pair.of(GROUP_ISSUER_N, USD), CURVE_ID_USD4, Pair.of(GROUP_ISSUER_N, GBP), CURVE_ID_GBP2);

		LegalEntityDiscountingMarketDataLookup test = LegalEntityDiscountingMarketDataLookup.of(repoSecurityGroups, repoGroups, repoCurves, issuerGroups, issuerCurves);
		assertSerialization(test);
	  }

	}

}