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
//	import static org.testng.Assert.assertFalse;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using DatedParameterMetadata = com.opengamma.strata.market.param.DatedParameterMetadata;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using TenorDateParameterMetadata = com.opengamma.strata.market.param.TenorDateParameterMetadata;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;
	using ThreeLegBasisSwapConventions = com.opengamma.strata.product.swap.type.ThreeLegBasisSwapConventions;
	using ThreeLegBasisSwapTemplate = com.opengamma.strata.product.swap.type.ThreeLegBasisSwapTemplate;

	/// <summary>
	/// Test <seealso cref="ThreeLegBasisSwapCurveNode"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ThreeLegBasisSwapCurveNodeTest
	public class ThreeLegBasisSwapCurveNodeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = date(2015, 6, 30);
	  private static readonly ThreeLegBasisSwapTemplate TEMPLATE = ThreeLegBasisSwapTemplate.of(Period.ZERO, TENOR_10Y, ThreeLegBasisSwapConventions.EUR_FIXED_1Y_EURIBOR_3M_EURIBOR_6M);
	  private static readonly ThreeLegBasisSwapTemplate TEMPLATE2 = ThreeLegBasisSwapTemplate.of(Period.ofMonths(1), TENOR_10Y, ThreeLegBasisSwapConventions.EUR_FIXED_1Y_EURIBOR_3M_EURIBOR_6M);
	  private static readonly QuoteId QUOTE_ID = QuoteId.of(StandardId.of("OG-Ticker", "EUR-BS36-10Y"));
	  private static readonly QuoteId QUOTE_ID2 = QuoteId.of(StandardId.of("OG-Ticker", "Test"));
	  private const double SPREAD = 0.0015;
	  private const string LABEL = "Label";
	  private const string LABEL_AUTO = "10Y";

	  public virtual void test_builder()
	  {
		ThreeLegBasisSwapCurveNode test = ThreeLegBasisSwapCurveNode.builder().label(LABEL).template(TEMPLATE).rateId(QUOTE_ID).additionalSpread(SPREAD).build();
		assertEquals(test.Label, LABEL);
		assertEquals(test.RateId, QUOTE_ID);
		assertEquals(test.AdditionalSpread, SPREAD);
		assertEquals(test.Template, TEMPLATE);
		assertEquals(test.Date, CurveNodeDate.END);
	  }

	  public virtual void test_of_noSpread()
	  {
		ThreeLegBasisSwapCurveNode test = ThreeLegBasisSwapCurveNode.of(TEMPLATE, QUOTE_ID);
		assertEquals(test.Label, LABEL_AUTO);
		assertEquals(test.RateId, QUOTE_ID);
		assertEquals(test.AdditionalSpread, 0.0d);
		assertEquals(test.Template, TEMPLATE);
	  }

	  public virtual void test_of_withSpread()
	  {
		ThreeLegBasisSwapCurveNode test = ThreeLegBasisSwapCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		assertEquals(test.Label, LABEL_AUTO);
		assertEquals(test.RateId, QUOTE_ID);
		assertEquals(test.AdditionalSpread, SPREAD);
		assertEquals(test.Template, TEMPLATE);
	  }

	  public virtual void test_of_withSpreadAndLabel()
	  {
		ThreeLegBasisSwapCurveNode test = ThreeLegBasisSwapCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD, LABEL);
		assertEquals(test.Label, LABEL);
		assertEquals(test.RateId, QUOTE_ID);
		assertEquals(test.AdditionalSpread, SPREAD);
		assertEquals(test.Template, TEMPLATE);
	  }

	  public virtual void test_requirements()
	  {
		ThreeLegBasisSwapCurveNode test = ThreeLegBasisSwapCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		ISet<ObservableId> set = test.requirements();
		IEnumerator<ObservableId> itr = set.GetEnumerator();
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(itr.next(), QUOTE_ID);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertFalse(itr.hasNext());
	  }

	  public virtual void test_trade()
	  {
		ThreeLegBasisSwapCurveNode node = ThreeLegBasisSwapCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		double rate = 0.125;
		double quantity = -1234.56;
		MarketData marketData = ImmutableMarketData.builder(VAL_DATE).addValue(QUOTE_ID, rate).build();
		SwapTrade trade = node.trade(quantity, marketData, REF_DATA);
		SwapTrade expected = TEMPLATE.createTrade(VAL_DATE, BUY, -quantity, rate + SPREAD, REF_DATA);
		assertEquals(trade, expected);
	  }

	  public virtual void test_trade_noMarketData()
	  {
		ThreeLegBasisSwapCurveNode node = ThreeLegBasisSwapCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		MarketData marketData = MarketData.empty(VAL_DATE);
		assertThrows(() => node.trade(1d, marketData, REF_DATA), typeof(MarketDataNotFoundException));
	  }

	  public virtual void test_initialGuess()
	  {
		ThreeLegBasisSwapCurveNode node = ThreeLegBasisSwapCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		LocalDate valuationDate = LocalDate.of(2015, 1, 22);
		double rate = 0.035;
		MarketData marketData = ImmutableMarketData.builder(valuationDate).addValue(QUOTE_ID, rate).build();
		assertEquals(node.initialGuess(marketData, ValueType.ZERO_RATE), 0d);
		assertEquals(node.initialGuess(marketData, ValueType.DISCOUNT_FACTOR), 1.0d);
	  }

	  public virtual void test_metadata_end()
	  {
		ThreeLegBasisSwapCurveNode node = ThreeLegBasisSwapCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		LocalDate valuationDate = LocalDate.of(2015, 1, 22);
		ParameterMetadata metadata = node.metadata(valuationDate, REF_DATA);
		assertEquals(((TenorDateParameterMetadata) metadata).Date, LocalDate.of(2025, 1, 27));
		assertEquals(((TenorDateParameterMetadata) metadata).Tenor, Tenor.TENOR_10Y);
	  }

	  public virtual void test_metadata_fixed()
	  {
		LocalDate nodeDate = VAL_DATE.plusMonths(1);
		ThreeLegBasisSwapCurveNode node = ThreeLegBasisSwapCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD, LABEL).withDate(CurveNodeDate.of(nodeDate));
		DatedParameterMetadata metadata = node.metadata(VAL_DATE, REF_DATA);
		assertEquals(metadata.Date, nodeDate);
		assertEquals(metadata.Label, node.Label);
	  }

	  public virtual void test_metadata_last_fixing()
	  {
		ThreeLegBasisSwapCurveNode node = ThreeLegBasisSwapCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD, LABEL).withDate(CurveNodeDate.LAST_FIXING);
		LocalDate valuationDate = LocalDate.of(2015, 1, 22);
		LocalDate fixingExpected = LocalDate.of(2024, 7, 24);
		DatedParameterMetadata metadata = node.metadata(valuationDate, REF_DATA);
		assertEquals(metadata.Date, fixingExpected);
		assertEquals(metadata.Label, node.Label);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ThreeLegBasisSwapCurveNode test = ThreeLegBasisSwapCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		coverImmutableBean(test);
		ThreeLegBasisSwapCurveNode test2 = ThreeLegBasisSwapCurveNode.of(TEMPLATE2, QUOTE_ID2, 0.1);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		ThreeLegBasisSwapCurveNode test = ThreeLegBasisSwapCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		assertSerialization(test);
	  }

	}

}