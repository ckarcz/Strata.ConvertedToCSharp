using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.node
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_10Y;
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
//	import static com.opengamma.strata.product.common.BuySell.BUY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using FxRateId = com.opengamma.strata.data.FxRateId;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using DatedParameterMetadata = com.opengamma.strata.market.param.DatedParameterMetadata;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using TenorDateParameterMetadata = com.opengamma.strata.market.param.TenorDateParameterMetadata;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;
	using XCcyIborIborSwapConventions = com.opengamma.strata.product.swap.type.XCcyIborIborSwapConventions;
	using XCcyIborIborSwapTemplate = com.opengamma.strata.product.swap.type.XCcyIborIborSwapTemplate;

	/// <summary>
	/// Test <seealso cref="XCcyIborIborSwapCurveNode"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class XCcyIborIborSwapCurveNodeTest
	public class XCcyIborIborSwapCurveNodeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = date(2015, 6, 30);
	  private static readonly XCcyIborIborSwapTemplate TEMPLATE = XCcyIborIborSwapTemplate.of(Period.ZERO, TENOR_10Y, XCcyIborIborSwapConventions.EUR_EURIBOR_3M_USD_LIBOR_3M);
	  private static readonly XCcyIborIborSwapTemplate TEMPLATE2 = XCcyIborIborSwapTemplate.of(Period.ofMonths(1), TENOR_10Y, XCcyIborIborSwapConventions.EUR_EURIBOR_3M_USD_LIBOR_3M);
	  private static readonly QuoteId SPREAD_ID = QuoteId.of(StandardId.of("OG-Ticker", "USD-EUR-XCS-10Y"));
	  private static readonly QuoteId SPREAD_ID2 = QuoteId.of(StandardId.of("OG-Ticker", "Test"));
	  private static readonly ObservableSource OBS_SOURCE = ObservableSource.of("Vendor");
	  private static readonly FxRateId FX_RATE_ID = FxRateId.of(TEMPLATE.CurrencyPair);
	  private static readonly FxRateId FX_RATE_ID2 = FxRateId.of(TEMPLATE.CurrencyPair, OBS_SOURCE);
	  private const double SPREAD_XCS = 0.00125;
	  private static readonly FxRate FX_EUR_USD = FxRate.of(Currency.EUR, Currency.USD, 1.25);
	  private const double SPREAD_ADJ = 0.0015;
	  private const string LABEL = "Label";
	  private const string LABEL_AUTO = "10Y";
	  private static readonly MarketData MARKET_DATA = ImmutableMarketData.builder(VAL_DATE).addValue(SPREAD_ID, SPREAD_XCS).addValue(FX_RATE_ID, FX_EUR_USD).build();

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		XCcyIborIborSwapCurveNode test = XCcyIborIborSwapCurveNode.builder().label(LABEL).template(TEMPLATE).fxRateId(FX_RATE_ID2).spreadId(SPREAD_ID).additionalSpread(SPREAD_ADJ).build();
		assertEquals(test.Label, LABEL);
		assertEquals(test.FxRateId, FX_RATE_ID2);
		assertEquals(test.SpreadId, SPREAD_ID);
		assertEquals(test.AdditionalSpread, SPREAD_ADJ);
		assertEquals(test.Template, TEMPLATE);
		assertEquals(test.Date, CurveNodeDate.END);
	  }

	  public virtual void test_builder_defaults()
	  {
		XCcyIborIborSwapCurveNode test = XCcyIborIborSwapCurveNode.builder().template(TEMPLATE).spreadId(SPREAD_ID).build();
		assertEquals(test.Label, LABEL_AUTO);
		assertEquals(test.FxRateId, FX_RATE_ID);
		assertEquals(test.SpreadId, SPREAD_ID);
		assertEquals(test.Template, TEMPLATE);
		assertEquals(test.Date, CurveNodeDate.END);
	  }

	  public virtual void test_builder_noTemplate()
	  {
		assertThrowsIllegalArg(() => XCcyIborIborSwapCurveNode.builder().label(LABEL).spreadId(SPREAD_ID).build());
	  }

	  public virtual void test_of_noSpread()
	  {
		XCcyIborIborSwapCurveNode test = XCcyIborIborSwapCurveNode.of(TEMPLATE, SPREAD_ID);
		assertEquals(test.Label, LABEL_AUTO);
		assertEquals(test.SpreadId, SPREAD_ID);
		assertEquals(test.AdditionalSpread, 0.0d);
		assertEquals(test.Template, TEMPLATE);
	  }

	  public virtual void test_of_withSpread()
	  {
		XCcyIborIborSwapCurveNode test = XCcyIborIborSwapCurveNode.of(TEMPLATE, SPREAD_ID, SPREAD_ADJ);
		assertEquals(test.Label, LABEL_AUTO);
		assertEquals(test.SpreadId, SPREAD_ID);
		assertEquals(test.AdditionalSpread, SPREAD_ADJ);
		assertEquals(test.Template, TEMPLATE);
	  }

	  public virtual void test_of_withSpreadAndLabel()
	  {
		XCcyIborIborSwapCurveNode test = XCcyIborIborSwapCurveNode.of(TEMPLATE, SPREAD_ID, SPREAD_ADJ, LABEL);
		assertEquals(test.Label, LABEL);
		assertEquals(test.SpreadId, SPREAD_ID);
		assertEquals(test.AdditionalSpread, SPREAD_ADJ);
		assertEquals(test.Template, TEMPLATE);
	  }

	  public virtual void test_requirements()
	  {
		XCcyIborIborSwapCurveNode test = XCcyIborIborSwapCurveNode.of(TEMPLATE, SPREAD_ID, SPREAD_ADJ);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Set<? extends com.opengamma.strata.data.MarketDataId<?>> setExpected = com.google.common.collect.ImmutableSet.of(SPREAD_ID, FX_RATE_ID);
		ISet<MarketDataId<object>> setExpected = ImmutableSet.of(SPREAD_ID, FX_RATE_ID);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Set<? extends com.opengamma.strata.data.MarketDataId<?>> set = test.requirements();
		ISet<MarketDataId<object>> set = test.requirements();
		assertTrue(set.SetEquals(setExpected));
	  }

	  public virtual void test_trade()
	  {
		XCcyIborIborSwapCurveNode node = XCcyIborIborSwapCurveNode.of(TEMPLATE, SPREAD_ID, SPREAD_ADJ);
		double quantity = -1234.56;
		SwapTrade trade = node.trade(quantity, MARKET_DATA, REF_DATA);
		double rate = FX_EUR_USD.fxRate(Currency.EUR, Currency.USD);
		SwapTrade expected = TEMPLATE.createTrade(VAL_DATE, BUY, -quantity, rate, SPREAD_XCS + SPREAD_ADJ, REF_DATA);
		assertEquals(trade, expected);
		assertEquals(node.resolvedTrade(quantity, MARKET_DATA, REF_DATA), trade.resolve(REF_DATA));
	  }

	  public virtual void test_trade_noMarketData()
	  {
		XCcyIborIborSwapCurveNode node = XCcyIborIborSwapCurveNode.of(TEMPLATE, SPREAD_ID, SPREAD_ADJ);
		MarketData marketData = MarketData.empty(VAL_DATE);
		assertThrows(() => node.trade(1d, marketData, REF_DATA), typeof(MarketDataNotFoundException));
	  }

	  public virtual void test_initialGuess()
	  {
		XCcyIborIborSwapCurveNode node = XCcyIborIborSwapCurveNode.of(TEMPLATE, SPREAD_ID, SPREAD_ADJ);
		assertEquals(node.initialGuess(MARKET_DATA, ValueType.ZERO_RATE), 0d);
		assertEquals(node.initialGuess(MARKET_DATA, ValueType.DISCOUNT_FACTOR), 1.0d);
	  }

	  public virtual void test_metadata_end()
	  {
		XCcyIborIborSwapCurveNode node = XCcyIborIborSwapCurveNode.of(TEMPLATE, SPREAD_ID, SPREAD_ADJ);
		LocalDate valuationDate = LocalDate.of(2015, 1, 22);
		ParameterMetadata metadata = node.metadata(valuationDate, REF_DATA);
		// 2015-01-22 is Thursday, start is 2015-01-26, but 2025-01-26 is Sunday, so end is 2025-01-27
		assertEquals(((TenorDateParameterMetadata) metadata).Date, LocalDate.of(2025, 1, 27));
		assertEquals(((TenorDateParameterMetadata) metadata).Tenor, Tenor.TENOR_10Y);
	  }

	  public virtual void test_metadata_fixed()
	  {
		LocalDate nodeDate = VAL_DATE.plusMonths(1);
		XCcyIborIborSwapCurveNode node = XCcyIborIborSwapCurveNode.of(TEMPLATE, SPREAD_ID, SPREAD_ADJ, LABEL).withDate(CurveNodeDate.of(nodeDate));
		DatedParameterMetadata metadata = node.metadata(VAL_DATE, REF_DATA);
		assertEquals(metadata.Date, nodeDate);
		assertEquals(metadata.Label, node.Label);
	  }

	  public virtual void test_metadata_last_fixing()
	  {
		XCcyIborIborSwapCurveNode node = XCcyIborIborSwapCurveNode.of(TEMPLATE, SPREAD_ID, SPREAD_ADJ, LABEL).withDate(CurveNodeDate.LAST_FIXING);
		LocalDate valuationDate = LocalDate.of(2015, 1, 22);
		DatedParameterMetadata metadata = node.metadata(valuationDate, REF_DATA);
		LocalDate fixingExpected = LocalDate.of(2024, 10, 24);
		assertEquals(metadata.Date, fixingExpected);
		assertEquals(((TenorDateParameterMetadata) metadata).Tenor, TENOR_10Y);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		XCcyIborIborSwapCurveNode test = XCcyIborIborSwapCurveNode.of(TEMPLATE, SPREAD_ID, SPREAD_ADJ);
		coverImmutableBean(test);
		XCcyIborIborSwapCurveNode test2 = XCcyIborIborSwapCurveNode.builder().label(LABEL).template(TEMPLATE2).fxRateId(FX_RATE_ID2).spreadId(SPREAD_ID2).additionalSpread(0.1).date(CurveNodeDate.LAST_FIXING).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		XCcyIborIborSwapCurveNode test = XCcyIborIborSwapCurveNode.of(TEMPLATE, SPREAD_ID, SPREAD_ADJ);
		assertSerialization(test);
	  }

	}

}