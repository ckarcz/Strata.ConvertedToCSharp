using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.rate
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.GBP_SONIA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.US_CPI_U;
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
//	import static org.mockito.Mockito.mock;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanSer = org.joda.beans.ser.JodaBeanSer;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;
	using Index = com.opengamma.strata.basics.index.Index;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using FxRateLookup = com.opengamma.strata.calc.runner.FxRateLookup;
	using FxRateId = com.opengamma.strata.data.FxRateId;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataFxRateProvider = com.opengamma.strata.data.MarketDataFxRateProvider;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using RatesCurveGroup = com.opengamma.strata.market.curve.RatesCurveGroup;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;
	using TestMarketDataMap = com.opengamma.strata.measure.curve.TestMarketDataMap;
	using SimpleDiscountFactors = com.opengamma.strata.pricer.SimpleDiscountFactors;
	using DiscountIborIndexRates = com.opengamma.strata.pricer.rate.DiscountIborIndexRates;
	using DiscountOvernightIndexRates = com.opengamma.strata.pricer.rate.DiscountOvernightIndexRates;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;

	/// <summary>
	/// Test <seealso cref="RatesMarketDataLookup"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class RatesMarketDataLookupTest
	public class RatesMarketDataLookupTest
	{

	  private static readonly CurveId CURVE_ID_DSC = CurveId.of("Group", "USD-DSC");
	  private static readonly CurveId CURVE_ID_FWD = CurveId.of("Group", "USD-L3M");
	  private static readonly ObservableSource OBS_SOURCE = ObservableSource.of("Vendor");
	  private static readonly MarketData MOCK_MARKET_DATA = mock(typeof(MarketData));
	  private static readonly ScenarioMarketData MOCK_CALC_MARKET_DATA = mock(typeof(ScenarioMarketData));

	  //-------------------------------------------------------------------------
	  public virtual void test_of_map()
	  {
		ImmutableMap<Currency, CurveId> discounts = ImmutableMap.of(USD, CURVE_ID_DSC);
		ImmutableMap<Index, CurveId> forwards = ImmutableMap.of(USD_LIBOR_3M, CURVE_ID_FWD);
		RatesMarketDataLookup test = RatesMarketDataLookup.of(discounts, forwards);
		assertEquals(test.queryType(), typeof(RatesMarketDataLookup));
		assertEquals(test.DiscountCurrencies, ImmutableSet.of(USD));
		assertEquals(test.getDiscountMarketDataIds(USD), ImmutableSet.of(CURVE_ID_DSC));
		assertEquals(test.ForwardIndices, ImmutableSet.of(USD_LIBOR_3M));
		assertEquals(test.getForwardMarketDataIds(USD_LIBOR_3M), ImmutableSet.of(CURVE_ID_FWD));
		assertThrowsIllegalArg(() => test.getDiscountMarketDataIds(GBP));
		assertThrowsIllegalArg(() => test.getForwardMarketDataIds(GBP_LIBOR_3M));
		assertEquals(test.ObservableSource, ObservableSource.NONE);
		assertEquals(test.FxRateLookup, FxRateLookup.ofRates());

		assertEquals(test.requirements(USD), FunctionRequirements.builder().valueRequirements(CURVE_ID_DSC).outputCurrencies(USD).build());
		assertEquals(test.requirements(USD, USD_LIBOR_3M), FunctionRequirements.builder().valueRequirements(CURVE_ID_DSC, CURVE_ID_FWD).timeSeriesRequirements(IndexQuoteId.of(USD_LIBOR_3M)).outputCurrencies(USD).build());
		assertEquals(test.requirements(ImmutableSet.of(USD), ImmutableSet.of(USD_LIBOR_3M)), FunctionRequirements.builder().valueRequirements(CURVE_ID_DSC, CURVE_ID_FWD).timeSeriesRequirements(IndexQuoteId.of(USD_LIBOR_3M)).outputCurrencies(USD).build());
		assertThrowsIllegalArg(() => test.requirements(ImmutableSet.of(USD), ImmutableSet.of(GBP_LIBOR_3M)));

		assertEquals(test.ratesProvider(MOCK_MARKET_DATA), DefaultLookupRatesProvider.of((DefaultRatesMarketDataLookup) test, MOCK_MARKET_DATA));
	  }

	  public virtual void test_of_groupNameAndMap()
	  {
		ImmutableMap<Currency, CurveName> discounts = ImmutableMap.of(USD, CURVE_ID_DSC.CurveName);
		ImmutableMap<Index, CurveName> forwards = ImmutableMap.of(USD_LIBOR_3M, CURVE_ID_FWD.CurveName);
		RatesMarketDataLookup test = RatesMarketDataLookup.of(CURVE_ID_DSC.CurveGroupName, discounts, forwards);
		assertEquals(test.queryType(), typeof(RatesMarketDataLookup));
		assertEquals(test.DiscountCurrencies, ImmutableSet.of(USD));
		assertEquals(test.getDiscountMarketDataIds(USD), ImmutableSet.of(CURVE_ID_DSC));
		assertEquals(test.ForwardIndices, ImmutableSet.of(USD_LIBOR_3M));
		assertEquals(test.getForwardMarketDataIds(USD_LIBOR_3M), ImmutableSet.of(CURVE_ID_FWD));
		assertThrowsIllegalArg(() => test.getDiscountMarketDataIds(GBP));
		assertThrowsIllegalArg(() => test.getForwardMarketDataIds(GBP_LIBOR_3M));
	  }

	  public virtual void test_of_curveGroup()
	  {
		ImmutableMap<Currency, Curve> discounts = ImmutableMap.of(USD, ConstantCurve.of(CURVE_ID_DSC.CurveName, 1));
		ImmutableMap<Index, Curve> forwards = ImmutableMap.of(USD_LIBOR_3M, ConstantCurve.of(CURVE_ID_FWD.CurveName, 1));
		RatesCurveGroup group = RatesCurveGroup.of(CURVE_ID_DSC.CurveGroupName, discounts, forwards);
		RatesMarketDataLookup test = RatesMarketDataLookup.of(group);
		assertEquals(test.queryType(), typeof(RatesMarketDataLookup));
		assertEquals(test.DiscountCurrencies, ImmutableSet.of(USD));
		assertEquals(test.getDiscountMarketDataIds(USD), ImmutableSet.of(CURVE_ID_DSC));
		assertEquals(test.ForwardIndices, ImmutableSet.of(USD_LIBOR_3M));
		assertEquals(test.getForwardMarketDataIds(USD_LIBOR_3M), ImmutableSet.of(CURVE_ID_FWD));
		assertThrowsIllegalArg(() => test.getDiscountMarketDataIds(GBP));
		assertThrowsIllegalArg(() => test.getForwardMarketDataIds(GBP_LIBOR_3M));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_marketDataView()
	  {
		ImmutableMap<Currency, CurveId> discounts = ImmutableMap.of(USD, CURVE_ID_DSC);
		ImmutableMap<Index, CurveId> forwards = ImmutableMap.of(USD_LIBOR_3M, CURVE_ID_FWD);
		RatesMarketDataLookup test = RatesMarketDataLookup.of(discounts, forwards);
		LocalDate valDate = date(2015, 6, 30);
		ScenarioMarketData md = new TestMarketDataMap(valDate, ImmutableMap.of(), ImmutableMap.of());
		RatesScenarioMarketData multiScenario = test.marketDataView(md);
		assertEquals(multiScenario.Lookup, test);
		assertEquals(multiScenario.MarketData, md);
		assertEquals(multiScenario.ScenarioCount, 1);
		RatesMarketData scenario = multiScenario.scenario(0);
		assertEquals(scenario.Lookup, test);
		assertEquals(scenario.MarketData, md.scenario(0));
		assertEquals(scenario.ValuationDate, valDate);
	  }

	  public virtual void test_ratesProvider()
	  {
		ImmutableMap<Currency, CurveId> discounts = ImmutableMap.of(USD, CURVE_ID_DSC);
		ImmutableMap<Index, CurveId> forwards = ImmutableMap.of(USD_FED_FUND, CURVE_ID_DSC, USD_LIBOR_3M, CURVE_ID_FWD, US_CPI_U, CURVE_ID_FWD);
		RatesMarketDataLookup test = RatesMarketDataLookup.of(discounts, forwards);
		LocalDate valDate = date(2015, 6, 30);
		Curve dscCurve = ConstantCurve.of(Curves.discountFactors(CURVE_ID_DSC.CurveName, ACT_360), 1d);
		Curve fwdCurve = ConstantCurve.of(Curves.discountFactors(CURVE_ID_FWD.CurveName, ACT_360), 2d);
		MarketData md = ImmutableMarketData.of(valDate, ImmutableMap.of(CURVE_ID_DSC, dscCurve, CURVE_ID_FWD, fwdCurve));
		RatesProvider ratesProvider = test.ratesProvider(md);
		assertEquals(ratesProvider.ValuationDate, valDate);
		assertEquals(ratesProvider.findData(CURVE_ID_DSC.CurveName), dscCurve);
		assertEquals(ratesProvider.findData(CURVE_ID_FWD.CurveName), fwdCurve);
		assertEquals(ratesProvider.findData(CurveName.of("Rubbish")), null);
		assertEquals(ratesProvider.IborIndices, ImmutableSet.of(USD_LIBOR_3M));
		assertEquals(ratesProvider.OvernightIndices, ImmutableSet.of(USD_FED_FUND));
		assertEquals(ratesProvider.PriceIndices, ImmutableSet.of(US_CPI_U));
		assertEquals(ratesProvider.TimeSeriesIndices, ImmutableSet.of());
		// check discount factors
		SimpleDiscountFactors df = (SimpleDiscountFactors) ratesProvider.discountFactors(USD);
		assertEquals(df.Curve.Name, dscCurve.Name);
		assertThrowsIllegalArg(() => ratesProvider.discountFactors(GBP));
		// check Ibor
		DiscountIborIndexRates ibor = (DiscountIborIndexRates) ratesProvider.iborIndexRates(USD_LIBOR_3M);
		SimpleDiscountFactors iborDf = (SimpleDiscountFactors) ibor.DiscountFactors;
		assertEquals(iborDf.Curve.Name, fwdCurve.Name);
		assertThrowsIllegalArg(() => ratesProvider.iborIndexRates(GBP_LIBOR_3M));
		// check Overnight
		DiscountOvernightIndexRates on = (DiscountOvernightIndexRates) ratesProvider.overnightIndexRates(USD_FED_FUND);
		SimpleDiscountFactors onDf = (SimpleDiscountFactors) on.DiscountFactors;
		assertEquals(onDf.Curve.Name, dscCurve.Name);
		assertThrowsIllegalArg(() => ratesProvider.overnightIndexRates(GBP_SONIA));
		// check price curve must be interpolated
		assertThrowsIllegalArg(() => ratesProvider.priceIndexValues(US_CPI_U));
		// to immutable
		ImmutableRatesProvider expectedImmutable = ImmutableRatesProvider.builder(valDate).fxRateProvider(MarketDataFxRateProvider.of(md)).discountCurve(USD, dscCurve).indexCurve(USD_FED_FUND, dscCurve).indexCurve(USD_LIBOR_3M, fwdCurve).indexCurve(US_CPI_U, fwdCurve).build();
		assertEquals(ratesProvider.toImmutableRatesProvider(), expectedImmutable);
	  }

	  public virtual void test_fxProvider()
	  {
		RatesMarketDataLookup test = RatesMarketDataLookup.of(ImmutableMap.of(), ImmutableMap.of());
		LocalDate valDate = date(2015, 6, 30);
		FxRateId gbpUsdId = FxRateId.of(GBP, USD);
		FxRate gbpUsdRate = FxRate.of(GBP, USD, 1.6);
		MarketData md = ImmutableMarketData.of(valDate, ImmutableMap.of(gbpUsdId, gbpUsdRate));
		FxRateProvider fxProvider = test.fxRateProvider(md);
		assertEquals(fxProvider.fxRate(GBP, USD), 1.6);
		assertEquals(test.marketDataView(md).fxRateProvider().fxRate(GBP, USD), 1.6);
		assertThrows(() => fxProvider.fxRate(EUR, USD), typeof(MarketDataNotFoundException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ImmutableMap<Currency, CurveId> discounts = ImmutableMap.of(USD, CURVE_ID_DSC);
		ImmutableMap<Index, CurveId> forwards = ImmutableMap.of(USD_LIBOR_3M, CURVE_ID_FWD);
		DefaultRatesMarketDataLookup test = DefaultRatesMarketDataLookup.of(discounts, forwards, ObservableSource.NONE, FxRateLookup.ofRates());
		coverImmutableBean(test);

		ImmutableMap<Currency, CurveId> discounts2 = ImmutableMap.of(GBP, CURVE_ID_DSC);
		ImmutableMap<Index, CurveId> forwards2 = ImmutableMap.of(GBP_LIBOR_3M, CURVE_ID_FWD);
		DefaultRatesMarketDataLookup test2 = DefaultRatesMarketDataLookup.of(discounts2, forwards2, OBS_SOURCE, FxRateLookup.ofRates(EUR));
		coverBeanEquals(test, test2);

		// related coverage
		coverImmutableBean((ImmutableBean) test.marketDataView(MOCK_CALC_MARKET_DATA));
		DefaultRatesScenarioMarketData.meta();

		coverImmutableBean((ImmutableBean) test.marketDataView(MOCK_MARKET_DATA));
		DefaultRatesMarketData.meta();

		coverImmutableBean((ImmutableBean) test.marketDataView(MOCK_MARKET_DATA).ratesProvider());
		DefaultLookupRatesProvider.meta();
	  }

	  public virtual void test_serialization()
	  {
		ImmutableMap<Currency, CurveId> discounts = ImmutableMap.of(USD, CURVE_ID_DSC);
		ImmutableMap<Index, CurveId> forwards = ImmutableMap.of(USD_LIBOR_3M, CURVE_ID_FWD);
		DefaultRatesMarketDataLookup test = DefaultRatesMarketDataLookup.of(discounts, forwards, ObservableSource.NONE, FxRateLookup.ofRates());
		assertSerialization(test);
		Curve curve = ConstantCurve.of(Curves.discountFactors("DSC", ACT_360), 0.99);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<? extends com.opengamma.strata.data.MarketDataId<?>, ?> valuesMap = com.google.common.collect.ImmutableMap.of(CURVE_ID_DSC, curve, CURVE_ID_FWD, curve);
		IDictionary<MarketDataId<object>, ?> valuesMap = ImmutableMap.of(CURVE_ID_DSC, curve, CURVE_ID_FWD, curve);
		MarketData md = MarketData.of(date(2016, 6, 30), valuesMap);
		assertSerialization(test.marketDataView(md));
		assertSerialization(test.ratesProvider(md));
	  }

	  public virtual void test_jodaSerialization()
	  {
		ImmutableMap<Currency, CurveId> discounts = ImmutableMap.of(USD, CURVE_ID_DSC);
		ImmutableMap<Index, CurveId> forwards = ImmutableMap.of(USD_LIBOR_3M, CURVE_ID_FWD);
		DefaultRatesMarketDataLookup test = DefaultRatesMarketDataLookup.of(discounts, forwards, ObservableSource.NONE, FxRateLookup.ofRates());
		string xml = JodaBeanSer.PRETTY.xmlWriter().write(test);
		assertEquals(xml.Contains("<entry key=\"USD-LIBOR-3M\">"), true);
		assertEquals(xml.Contains("<fixingDateOffset>"), false);
		assertEquals(xml.Contains("<effectiveDateOffset>"), false);
	  }

	}

}