using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.node
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsWithCause;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
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
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using FxSwapTrade = com.opengamma.strata.product.fx.FxSwapTrade;
	using FxSwapTemplate = com.opengamma.strata.product.fx.type.FxSwapTemplate;
	using ImmutableFxSwapConvention = com.opengamma.strata.product.fx.type.ImmutableFxSwapConvention;

	/// <summary>
	/// Test <seealso cref="FxSwapCurveNode"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxSwapCurveNodeTest
	public class FxSwapCurveNodeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = date(2015, 6, 30);
	  private static readonly CurrencyPair EUR_USD = CurrencyPair.of(Currency.EUR, Currency.USD);
	  private static readonly HolidayCalendarId EUTA_USNY = EUTA.combinedWith(USNY);
	  private static readonly DaysAdjustment PLUS_TWO_DAYS = DaysAdjustment.ofBusinessDays(2, EUTA_USNY);
	  private static readonly ImmutableFxSwapConvention CONVENTION = ImmutableFxSwapConvention.of(EUR_USD, PLUS_TWO_DAYS);
	  private static readonly Period NEAR_PERIOD = Period.ofMonths(3);
	  private static readonly Period FAR_PERIOD = Period.ofMonths(6);
	  private static readonly FxSwapTemplate TEMPLATE = FxSwapTemplate.of(NEAR_PERIOD, FAR_PERIOD, CONVENTION);

	  private static readonly ObservableSource OBS_SOURCE = ObservableSource.of("Vendor");
	  private static readonly FxRateId FX_RATE_ID = FxRateId.of(EUR_USD);
	  private static readonly FxRateId FX_RATE_ID2 = FxRateId.of(EUR_USD, OBS_SOURCE);
	  private static readonly QuoteId QUOTE_ID_PTS = QuoteId.of(StandardId.of("OG-Ticker", "EUR_USD_3M_6M"));
	  private static readonly QuoteId QUOTE_ID_PTS2 = QuoteId.of(StandardId.of("OG-Ticker", "EUR_USD_3M_6M2"));
	  private static readonly FxRate FX_RATE_NEAR = FxRate.of(EUR_USD, 1.30d);
	  private const double FX_RATE_PTS = 0.0050d;
	  private const string LABEL = "Label";
	  private const string LABEL_AUTO = "6M";
	  private static readonly MarketData MARKET_DATA = ImmutableMarketData.builder(VAL_DATE).addValue(FX_RATE_ID, FX_RATE_NEAR).addValue(QUOTE_ID_PTS, FX_RATE_PTS).build();

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		FxSwapCurveNode test = FxSwapCurveNode.builder().label(LABEL).template(TEMPLATE).fxRateId(FX_RATE_ID2).farForwardPointsId(QUOTE_ID_PTS).date(CurveNodeDate.LAST_FIXING).build();
		assertEquals(test.Label, LABEL);
		assertEquals(test.FxRateId, FX_RATE_ID2);
		assertEquals(test.FarForwardPointsId, QUOTE_ID_PTS);
		assertEquals(test.Template, TEMPLATE);
		assertEquals(test.Date, CurveNodeDate.LAST_FIXING);
	  }

	  public virtual void test_builder_defaults()
	  {
		FxSwapCurveNode test = FxSwapCurveNode.builder().template(TEMPLATE).farForwardPointsId(QUOTE_ID_PTS).build();
		assertEquals(test.Label, LABEL_AUTO);
		assertEquals(test.FxRateId, FX_RATE_ID);
		assertEquals(test.FarForwardPointsId, QUOTE_ID_PTS);
		assertEquals(test.Template, TEMPLATE);
		assertEquals(test.Date, CurveNodeDate.END);
	  }

	  public virtual void test_builder_noTemplate()
	  {
		assertThrowsIllegalArg(() => FxSwapCurveNode.builder().label(LABEL).farForwardPointsId(QUOTE_ID_PTS).build());
	  }

	  public virtual void test_of()
	  {
		FxSwapCurveNode test = FxSwapCurveNode.of(TEMPLATE, QUOTE_ID_PTS);
		assertEquals(test.Label, LABEL_AUTO);
		assertEquals(test.FxRateId, FX_RATE_ID);
		assertEquals(test.FarForwardPointsId, QUOTE_ID_PTS);
		assertEquals(test.Template, TEMPLATE);
	  }

	  public virtual void test_of_withLabel()
	  {
		FxSwapCurveNode test = FxSwapCurveNode.of(TEMPLATE, QUOTE_ID_PTS, LABEL);
		assertEquals(test.Label, LABEL);
		assertEquals(test.FxRateId, FX_RATE_ID);
		assertEquals(test.FarForwardPointsId, QUOTE_ID_PTS);
		assertEquals(test.Template, TEMPLATE);
	  }

	  public virtual void test_requirements()
	  {
		FxSwapCurveNode test = FxSwapCurveNode.of(TEMPLATE, QUOTE_ID_PTS);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Set<? extends com.opengamma.strata.data.MarketDataId<?>> setExpected = com.google.common.collect.ImmutableSet.of(FX_RATE_ID, QUOTE_ID_PTS);
		ISet<MarketDataId<object>> setExpected = ImmutableSet.of(FX_RATE_ID, QUOTE_ID_PTS);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Set<? extends com.opengamma.strata.data.MarketDataId<?>> set = test.requirements();
		ISet<MarketDataId<object>> set = test.requirements();
		assertTrue(set.SetEquals(setExpected));
	  }

	  public virtual void test_trade()
	  {
		FxSwapCurveNode node = FxSwapCurveNode.of(TEMPLATE, QUOTE_ID_PTS);
		FxSwapTrade trade = node.trade(1d, MARKET_DATA, REF_DATA);
		double rate = FX_RATE_NEAR.fxRate(EUR_USD);
		FxSwapTrade expected = TEMPLATE.createTrade(VAL_DATE, BuySell.BUY, 1.0, rate, FX_RATE_PTS, REF_DATA);
		assertEquals(trade, expected);
		assertEquals(node.resolvedTrade(1d, MARKET_DATA, REF_DATA), trade.resolve(REF_DATA));
	  }

	  public virtual void test_trade_noMarketData()
	  {
		FxSwapCurveNode node = FxSwapCurveNode.of(TEMPLATE, QUOTE_ID_PTS);
		MarketData marketData = MarketData.empty(VAL_DATE);
		assertThrows(() => node.trade(1d, marketData, REF_DATA), typeof(MarketDataNotFoundException));
	  }

	  public virtual void test_initialGuess()
	  {
		FxSwapCurveNode node = FxSwapCurveNode.of(TEMPLATE, QUOTE_ID_PTS);
		assertEquals(node.initialGuess(MARKET_DATA, ValueType.ZERO_RATE), 0.0d);
		assertEquals(node.initialGuess(MARKET_DATA, ValueType.DISCOUNT_FACTOR), 1.0d);
	  }

	  public virtual void test_metadata_end()
	  {
		FxSwapCurveNode node = FxSwapCurveNode.of(TEMPLATE, QUOTE_ID_PTS);
		LocalDate valuationDate = LocalDate.of(2015, 1, 22);
		LocalDate endDate = CONVENTION.BusinessDayAdjustment.adjust(CONVENTION.SpotDateOffset.adjust(valuationDate, REF_DATA).plus(FAR_PERIOD), REF_DATA);
		ParameterMetadata metadata = node.metadata(valuationDate, REF_DATA);
		assertEquals(((TenorDateParameterMetadata) metadata).Date, endDate);
		assertEquals(((TenorDateParameterMetadata) metadata).Tenor, Tenor.of(FAR_PERIOD));
	  }

	  public virtual void test_metadata_fixed()
	  {
		LocalDate nodeDate = VAL_DATE.plusMonths(1);
		FxSwapCurveNode node = FxSwapCurveNode.of(TEMPLATE, QUOTE_ID_PTS).withDate(CurveNodeDate.of(nodeDate));
		LocalDate valuationDate = LocalDate.of(2015, 1, 22);
		DatedParameterMetadata metadata = node.metadata(valuationDate, REF_DATA);
		assertEquals(metadata.Date, nodeDate);
		assertEquals(metadata.Label, node.Label);
	  }

	  public virtual void test_metadata_last_fixing()
	  {
		FxSwapCurveNode node = FxSwapCurveNode.of(TEMPLATE, QUOTE_ID_PTS).withDate(CurveNodeDate.LAST_FIXING);
		assertThrowsWithCause(() => node.metadata(VAL_DATE, REF_DATA), typeof(System.NotSupportedException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FxSwapCurveNode test = FxSwapCurveNode.of(TEMPLATE, QUOTE_ID_PTS);
		coverImmutableBean(test);
		FxSwapCurveNode test2 = FxSwapCurveNode.builder().label(LABEL).template(FxSwapTemplate.of(Period.ZERO, FAR_PERIOD, CONVENTION)).fxRateId(FX_RATE_ID2).farForwardPointsId(QUOTE_ID_PTS2).date(CurveNodeDate.LAST_FIXING).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		FxSwapCurveNode test = FxSwapCurveNode.of(TEMPLATE, QUOTE_ID_PTS);
		assertSerialization(test);
	  }

	}

}