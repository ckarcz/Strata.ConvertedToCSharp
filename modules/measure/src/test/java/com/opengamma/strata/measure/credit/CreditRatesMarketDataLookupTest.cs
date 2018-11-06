using System.Collections.Generic;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.credit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsRuntime;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.mock;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using ImmutableBean = org.joda.beans.ImmutableBean;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using ConstantNodalCurve = com.opengamma.strata.market.curve.ConstantNodalCurve;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using TestMarketDataMap = com.opengamma.strata.measure.curve.TestMarketDataMap;
	using ConstantRecoveryRates = com.opengamma.strata.pricer.credit.ConstantRecoveryRates;
	using CreditRatesProvider = com.opengamma.strata.pricer.credit.CreditRatesProvider;
	using IsdaCreditDiscountFactors = com.opengamma.strata.pricer.credit.IsdaCreditDiscountFactors;
	using LegalEntitySurvivalProbabilities = com.opengamma.strata.pricer.credit.LegalEntitySurvivalProbabilities;

	/// <summary>
	/// Test <seealso cref="CreditRatesMarketDataLookup"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CreditRatesMarketDataLookupTest
	public class CreditRatesMarketDataLookupTest
	{

	  private static readonly StandardId ISSUER_A = StandardId.of("OG-LegEnt", "A");
	  private static readonly StandardId ISSUER_B = StandardId.of("OG-LegEnt", "B");
	  private static readonly StandardId ISSUER_C = StandardId.of("OG-LegEnt", "C");

	  private static readonly CurveId CC_A_USD = CurveId.of("Group", "Credit-A-USD");
	  private static readonly CurveId CC_B_GBP = CurveId.of("Group", "Credit-B-GBP");
	  private static readonly CurveId CC_A_GBP = CurveId.of("Group", "Credit-A-GBP");
	  private static readonly CurveId DC_USD = CurveId.of("Group", "Dsc-USD");
	  private static readonly CurveId DC_GBP = CurveId.of("Group", "Dsc-GBP");
	  private static readonly CurveId RC_A = CurveId.of("Group", "Recovery-A");
	  private static readonly CurveId RC_B = CurveId.of("Group", "Recovery-B");
	  private static readonly ObservableSource OBS_SOURCE = ObservableSource.of("Vendor");
	  private static readonly MarketData MOCK_MARKET_DATA = mock(typeof(MarketData));
	  private static readonly ScenarioMarketData MOCK_CALC_MARKET_DATA = mock(typeof(ScenarioMarketData));

	  private static readonly CreditRatesMarketDataLookup LOOKUP;
	  private static readonly CreditRatesMarketDataLookup LOOKUP_WITH_SOURCE;
	  static CreditRatesMarketDataLookupTest()
	  {
		ImmutableMap<Pair<StandardId, Currency>, CurveId> creditCurve = ImmutableMap.of(Pair.of(ISSUER_A, USD), CC_A_USD, Pair.of(ISSUER_B, GBP), CC_B_GBP, Pair.of(ISSUER_A, GBP), CC_A_GBP);
		ImmutableMap<Currency, CurveId> discoutCurve = ImmutableMap.of(USD, DC_USD, GBP, DC_GBP);
		ImmutableMap<StandardId, CurveId> recoveryCurve = ImmutableMap.of(ISSUER_A, RC_A, ISSUER_B, RC_B);
		LOOKUP_WITH_SOURCE = CreditRatesMarketDataLookup.of(creditCurve, discoutCurve, recoveryCurve, OBS_SOURCE);
		LOOKUP = CreditRatesMarketDataLookup.of(creditCurve, discoutCurve, recoveryCurve);
	  }

	  public virtual void test_map()
	  {
		assertEquals(LOOKUP.queryType(), typeof(CreditRatesMarketDataLookup));
		assertEquals(LOOKUP_WITH_SOURCE.requirements(ISSUER_A, USD), FunctionRequirements.builder().observableSource(OBS_SOURCE).valueRequirements(CC_A_USD, DC_USD, RC_A).outputCurrencies(USD).build());
		assertEquals(LOOKUP_WITH_SOURCE.requirements(ISSUER_A, GBP), FunctionRequirements.builder().observableSource(OBS_SOURCE).valueRequirements(CC_A_GBP, DC_GBP, RC_A).outputCurrencies(GBP).build());
		assertEquals(LOOKUP_WITH_SOURCE.requirements(ISSUER_B, GBP), FunctionRequirements.builder().observableSource(OBS_SOURCE).valueRequirements(CC_B_GBP, DC_GBP, RC_B).outputCurrencies(GBP).build());
		assertEquals(LOOKUP.requirements(ISSUER_A, USD), FunctionRequirements.builder().valueRequirements(CC_A_USD, DC_USD, RC_A).outputCurrencies(USD).build());
		assertEquals(LOOKUP.requirements(ISSUER_A, GBP), FunctionRequirements.builder().valueRequirements(CC_A_GBP, DC_GBP, RC_A).outputCurrencies(GBP).build());
		assertEquals(LOOKUP.requirements(ISSUER_B, GBP), FunctionRequirements.builder().valueRequirements(CC_B_GBP, DC_GBP, RC_B).outputCurrencies(GBP).build());
		assertThrowsIllegalArg(() => LOOKUP.requirements(ISSUER_A, EUR));
		assertThrowsIllegalArg(() => LOOKUP.requirements(ISSUER_C, USD));
		assertEquals(LOOKUP.creditRatesProvider(MOCK_MARKET_DATA), DefaultLookupCreditRatesProvider.of((DefaultCreditRatesMarketDataLookup) LOOKUP, MOCK_MARKET_DATA));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_marketDataView()
	  {
		LocalDate valDate = LocalDate.of(2015, 6, 30);
		ScenarioMarketData md = new TestMarketDataMap(valDate, ImmutableMap.of(), ImmutableMap.of());
		CreditRatesScenarioMarketData multiScenario = LOOKUP_WITH_SOURCE.marketDataView(md);
		assertEquals(multiScenario.Lookup, LOOKUP_WITH_SOURCE);
		assertEquals(multiScenario.MarketData, md);
		assertEquals(multiScenario.ScenarioCount, 1);
		CreditRatesMarketData scenario = multiScenario.scenario(0);
		assertEquals(scenario.Lookup, LOOKUP_WITH_SOURCE);
		assertEquals(scenario.MarketData, md.scenario(0));
		assertEquals(scenario.ValuationDate, valDate);
	  }

	  public virtual void test_bondDiscountingProvider()
	  {
		LocalDate valDate = LocalDate.of(2015, 6, 30);
		Curve ccAUsd = ConstantNodalCurve.of(Curves.zeroRates(CC_A_USD.CurveName, ACT_365F), 0.5d, 1.5d);
		Curve ccBGbp = ConstantNodalCurve.of(Curves.zeroRates(CC_B_GBP.CurveName, ACT_365F), 0.5d, 2d);
		Curve ccAGbp = ConstantNodalCurve.of(Curves.zeroRates(CC_A_GBP.CurveName, ACT_365F), 0.5d, 3d);
		Curve dcGbp = ConstantNodalCurve.of(Curves.zeroRates(DC_GBP.CurveName, ACT_365F), 0.5d, 0.1d);
		Curve dcUsd = ConstantNodalCurve.of(Curves.zeroRates(DC_USD.CurveName, ACT_365F), 0.5d, 0.05d);
		Curve rcA = ConstantCurve.of(Curves.recoveryRates(RC_A.CurveName, ACT_365F), 0.5d);
		Curve rcB = ConstantCurve.of(Curves.recoveryRates(RC_B.CurveName, ACT_365F), 0.4234d);
		IDictionary<CurveId, Curve> curveMap = new Dictionary<CurveId, Curve>();
		curveMap[CC_A_USD] = ccAUsd;
		curveMap[CC_B_GBP] = ccBGbp;
		curveMap[CC_A_GBP] = ccAGbp;
		curveMap[DC_USD] = dcUsd;
		curveMap[DC_GBP] = dcGbp;
		curveMap[RC_A] = rcA;
		curveMap[RC_B] = rcB;
		MarketData md = ImmutableMarketData.of(valDate, ImmutableMap.copyOf(curveMap));
		CreditRatesProvider provider = LOOKUP_WITH_SOURCE.creditRatesProvider(md);

		assertEquals(provider.ValuationDate, valDate);
		assertEquals(provider.findData(CC_A_USD.CurveName), ccAUsd);
		assertEquals(provider.findData(DC_USD.CurveName), dcUsd);
		assertEquals(provider.findData(RC_B.CurveName), rcB);
		assertEquals(provider.findData(CurveName.of("Rubbish")), null);
		// check credit curve
		LegalEntitySurvivalProbabilities cc = provider.survivalProbabilities(ISSUER_A, GBP);
		IsdaCreditDiscountFactors ccUnder = (IsdaCreditDiscountFactors) cc.SurvivalProbabilities;
		assertEquals(ccUnder.Curve.Name, ccAGbp.Name);
		assertThrowsRuntime(() => provider.survivalProbabilities(ISSUER_B, USD));
		assertThrowsRuntime(() => provider.survivalProbabilities(ISSUER_C, USD));
		// check discount curve
		IsdaCreditDiscountFactors dc = (IsdaCreditDiscountFactors) provider.discountFactors(USD);
		assertEquals(dc.Curve.Name, dcUsd.Name);
		assertThrowsRuntime(() => provider.discountFactors(EUR));
		// check recovery rate curve
		ConstantRecoveryRates rc = (ConstantRecoveryRates) provider.recoveryRates(ISSUER_B);
		assertEquals(rc.RecoveryRate, rcB.getParameter(0));
		assertThrowsRuntime(() => provider.recoveryRates(ISSUER_C));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean((ImmutableBean) LOOKUP_WITH_SOURCE);
		ImmutableMap<Pair<StandardId, Currency>, CurveId> ccMap = ImmutableMap.of(Pair.of(ISSUER_A, USD), CC_A_USD);
		ImmutableMap<Currency, CurveId> dcMap = ImmutableMap.of(USD, DC_USD);
		ImmutableMap<StandardId, CurveId> rcMap = ImmutableMap.of(ISSUER_A, RC_A);
		CreditRatesMarketDataLookup test2 = CreditRatesMarketDataLookup.of(ccMap, dcMap, rcMap);
		coverBeanEquals((ImmutableBean) LOOKUP_WITH_SOURCE, (ImmutableBean) test2);

		// related coverage
		coverImmutableBean((ImmutableBean) LOOKUP_WITH_SOURCE.marketDataView(MOCK_CALC_MARKET_DATA));
		DefaultCreditRatesScenarioMarketData.meta();
		coverImmutableBean((ImmutableBean) LOOKUP_WITH_SOURCE.marketDataView(MOCK_MARKET_DATA));
		DefaultCreditRatesMarketData.meta();
		coverImmutableBean((ImmutableBean) LOOKUP_WITH_SOURCE.marketDataView(MOCK_MARKET_DATA).creditRatesProvider());
		DefaultLookupCreditRatesProvider.meta();
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(LOOKUP_WITH_SOURCE);
	  }

	}

}