using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.node
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
//	import static org.testng.Assert.assertFalse;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using DatedParameterMetadata = com.opengamma.strata.market.param.DatedParameterMetadata;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using YearMonthDateParameterMetadata = com.opengamma.strata.market.param.YearMonthDateParameterMetadata;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using IborFutureTrade = com.opengamma.strata.product.index.IborFutureTrade;
	using IborFutureConvention = com.opengamma.strata.product.index.type.IborFutureConvention;
	using IborFutureConventions = com.opengamma.strata.product.index.type.IborFutureConventions;
	using IborFutureTemplate = com.opengamma.strata.product.index.type.IborFutureTemplate;

	/// <summary>
	/// Tests <seealso cref="IborFutureCurveNode"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborFutureCurveNodeTest
	public class IborFutureCurveNodeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = date(2015, 6, 30);
	  private static readonly IborFutureConvention CONVENTION = IborFutureConventions.USD_LIBOR_3M_QUARTERLY_IMM;
	  private static readonly Period PERIOD_TO_START = Period.ofMonths(2);
	  private const int NUMBER = 2;
	  private static readonly IborFutureTemplate TEMPLATE = IborFutureTemplate.of(PERIOD_TO_START, NUMBER, CONVENTION);
	  private static readonly StandardId STANDARD_ID = StandardId.of("OG-Ticker", "OG-EDH6");
	  private static readonly QuoteId QUOTE_ID = QuoteId.of(STANDARD_ID);
	  private const double SPREAD = 0.0001;
	  private const string LABEL = "Label";

	  private const double TOLERANCE_RATE = 1.0E-8;

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		IborFutureCurveNode test = IborFutureCurveNode.builder().label(LABEL).template(TEMPLATE).rateId(QUOTE_ID).additionalSpread(SPREAD).build();
		assertEquals(test.RateId, QUOTE_ID);
		assertEquals(test.AdditionalSpread, SPREAD);
		assertEquals(test.Template, TEMPLATE);
		assertEquals(test.Date, CurveNodeDate.END);
	  }

	  public virtual void test_of_no_spread()
	  {
		IborFutureCurveNode test = IborFutureCurveNode.of(TEMPLATE, QUOTE_ID);
		assertEquals(test.RateId, QUOTE_ID);
		assertEquals(test.AdditionalSpread, 0.0d);
		assertEquals(test.Template, TEMPLATE);
	  }

	  public virtual void test_of_withSpread()
	  {
		IborFutureCurveNode test = IborFutureCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		assertEquals(test.RateId, QUOTE_ID);
		assertEquals(test.AdditionalSpread, SPREAD);
		assertEquals(test.Template, TEMPLATE);
	  }

	  public virtual void test_of_withSpreadAndLabel()
	  {
		IborFutureCurveNode test = IborFutureCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD, LABEL);
		assertEquals(test.RateId, QUOTE_ID);
		assertEquals(test.AdditionalSpread, SPREAD);
		assertEquals(test.Template, TEMPLATE);
	  }

	  public virtual void test_requirements()
	  {
		IborFutureCurveNode test = IborFutureCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		ISet<ObservableId> set = test.requirements();
		IEnumerator<ObservableId> itr = set.GetEnumerator();
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(itr.next(), QUOTE_ID);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertFalse(itr.hasNext());
	  }

	  public virtual void test_trade()
	  {
		IborFutureCurveNode node = IborFutureCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		double price = 0.99;
		MarketData marketData = ImmutableMarketData.builder(VAL_DATE).addValue(QUOTE_ID, price).build();
		IborFutureTrade trade = node.trade(1d, marketData, REF_DATA);
		IborFutureTrade expected = TEMPLATE.createTrade(VAL_DATE, SecurityId.of(STANDARD_ID), 1L, 1.0, price + SPREAD, REF_DATA);
		assertEquals(trade, expected);
	  }

	  public virtual void test_trade_noMarketData()
	  {
		IborFutureCurveNode node = IborFutureCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		MarketData marketData = MarketData.empty(VAL_DATE);
		assertThrows(() => node.trade(1d, marketData, REF_DATA), typeof(MarketDataNotFoundException));
	  }

	  public virtual void test_initialGuess()
	  {
		IborFutureCurveNode node = IborFutureCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		double price = 0.99;
		MarketData marketData = ImmutableMarketData.builder(VAL_DATE).addValue(QUOTE_ID, price).build();
		assertEquals(node.initialGuess(marketData, ValueType.ZERO_RATE), 1.0 - price, TOLERANCE_RATE);
		assertEquals(node.initialGuess(marketData, ValueType.FORWARD_RATE), 1.0 - price, TOLERANCE_RATE);
		double approximateMaturity = TEMPLATE.approximateMaturity(VAL_DATE);
		double df = Math.Exp(-approximateMaturity * (1.0 - price));
		assertEquals(node.initialGuess(marketData, ValueType.DISCOUNT_FACTOR), df, TOLERANCE_RATE);
		assertEquals(node.initialGuess(marketData, ValueType.UNKNOWN), 0.0d, TOLERANCE_RATE);
	  }

	  public virtual void test_metadata_end()
	  {
		IborFutureCurveNode node = IborFutureCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD, LABEL);
		LocalDate date = LocalDate.of(2015, 10, 20);
		LocalDate referenceDate = TEMPLATE.calculateReferenceDateFromTradeDate(date, REF_DATA);
		LocalDate maturityDate = TEMPLATE.Index.calculateMaturityFromEffective(referenceDate, REF_DATA);
		ParameterMetadata metadata = node.metadata(date, REF_DATA);
		assertEquals(metadata.Label, LABEL);
		assertTrue(metadata is YearMonthDateParameterMetadata);
		assertEquals(((YearMonthDateParameterMetadata) metadata).Date, maturityDate);
		assertEquals(((YearMonthDateParameterMetadata) metadata).YearMonth, YearMonth.from(referenceDate));
	  }

	  public virtual void test_metadata_fixed()
	  {
		LocalDate nodeDate = VAL_DATE.plusMonths(1);
		IborFutureCurveNode node = IborFutureCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD, LABEL).withDate(CurveNodeDate.of(nodeDate));
		DatedParameterMetadata metadata = node.metadata(VAL_DATE, REF_DATA);
		assertEquals(metadata.Date, nodeDate);
		assertEquals(metadata.Label, node.Label);
	  }

	  public virtual void test_metadata_last_fixing()
	  {
		IborFutureCurveNode node = IborFutureCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD, LABEL).withDate(CurveNodeDate.LAST_FIXING);
		ImmutableMarketData marketData = ImmutableMarketData.builder(VAL_DATE).addValue(QUOTE_ID, 0.0d).build();
		IborFutureTrade trade = node.trade(1d, marketData, REF_DATA);
		LocalDate fixingDate = trade.Product.FixingDate;
		DatedParameterMetadata metadata = node.metadata(VAL_DATE, REF_DATA);
		assertEquals(metadata.Date, fixingDate);
		LocalDate referenceDate = TEMPLATE.calculateReferenceDateFromTradeDate(VAL_DATE, REF_DATA);
		assertEquals(((YearMonthDateParameterMetadata) metadata).YearMonth, YearMonth.from(referenceDate));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		IborFutureCurveNode test = IborFutureCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		coverImmutableBean(test);
		IborFutureCurveNode test2 = IborFutureCurveNode.of(IborFutureTemplate.of(PERIOD_TO_START, NUMBER, CONVENTION), QuoteId.of(StandardId.of("OG-Ticker", "Unknown")));
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		IborFutureCurveNode test = IborFutureCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		assertSerialization(test);
	  }

	}

}