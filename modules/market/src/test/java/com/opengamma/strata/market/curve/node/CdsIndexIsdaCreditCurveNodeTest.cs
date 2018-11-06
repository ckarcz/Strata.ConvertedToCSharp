/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
//	import static com.opengamma.strata.product.common.BuySell.SELL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using LabelDateParameterMetadata = com.opengamma.strata.market.param.LabelDateParameterMetadata;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using TenorDateParameterMetadata = com.opengamma.strata.market.param.TenorDateParameterMetadata;
	using CdsIndex = com.opengamma.strata.product.credit.CdsIndex;
	using CdsIndexCalibrationTrade = com.opengamma.strata.product.credit.CdsIndexCalibrationTrade;
	using CdsIndexTrade = com.opengamma.strata.product.credit.CdsIndexTrade;
	using CdsQuote = com.opengamma.strata.product.credit.CdsQuote;
	using CdsTrade = com.opengamma.strata.product.credit.CdsTrade;
	using CdsConventions = com.opengamma.strata.product.credit.type.CdsConventions;
	using CdsQuoteConvention = com.opengamma.strata.product.credit.type.CdsQuoteConvention;
	using CdsTemplate = com.opengamma.strata.product.credit.type.CdsTemplate;
	using DatesCdsTemplate = com.opengamma.strata.product.credit.type.DatesCdsTemplate;
	using TenorCdsTemplate = com.opengamma.strata.product.credit.type.TenorCdsTemplate;

	/// <summary>
	/// Test <seealso cref="CdsIndexIsdaCreditCurveNode"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CdsIndexIsdaCreditCurveNodeTest
	public class CdsIndexIsdaCreditCurveNodeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = date(2015, 6, 30);
	  private static readonly CdsTemplate TEMPLATE = TenorCdsTemplate.of(TENOR_10Y, CdsConventions.USD_STANDARD);
	  private static readonly LocalDate START_DATE = LocalDate.of(2015, 5, 20);
	  private static readonly LocalDate END_DATE = LocalDate.of(2020, 10, 20);
	  private static readonly CdsTemplate TEMPLATE_NS = DatesCdsTemplate.of(START_DATE, END_DATE, CdsConventions.EUR_GB_STANDARD);
	  private static readonly QuoteId QUOTE_ID = QuoteId.of(StandardId.of("OG-Ticker", "Cds1"));
	  private const string LABEL = "Label";
	  private const string LABEL_AUTO = "10Y";
	  private static readonly StandardId INDEX_ID = StandardId.of("OG", "ABCXX");
	  private static readonly ImmutableList<StandardId> LEGAL_ENTITIES = ImmutableList.of(StandardId.of("OG", "ABC1"), StandardId.of("OG", "ABC2"), StandardId.of("OG", "ABC3"), StandardId.of("OG", "ABC4"));

	  public virtual void test_builder()
	  {
		CdsIndexIsdaCreditCurveNode test = CdsIndexIsdaCreditCurveNode.builder().label(LABEL).template(TEMPLATE).observableId(QUOTE_ID).quoteConvention(CdsQuoteConvention.PAR_SPREAD).cdsIndexId(INDEX_ID).legalEntityIds(LEGAL_ENTITIES).build();
		assertEquals(test.Label, LABEL);
		assertEquals(test.CdsIndexId, INDEX_ID);
		assertEquals(test.LegalEntityIds, LEGAL_ENTITIES);
		assertEquals(test.ObservableId, QUOTE_ID);
		assertEquals(test.Template, TEMPLATE);
		assertEquals(test.date(VAL_DATE, REF_DATA), date(2025, 6, 20));
	  }

	  public virtual void test_of_quotedSpread()
	  {
		CdsIndexIsdaCreditCurveNode test = CdsIndexIsdaCreditCurveNode.ofQuotedSpread(TEMPLATE, QUOTE_ID, INDEX_ID, LEGAL_ENTITIES, 0.01);
		assertEquals(test.Label, LABEL_AUTO);
		assertEquals(test.CdsIndexId, INDEX_ID);
		assertEquals(test.LegalEntityIds, LEGAL_ENTITIES);
		assertEquals(test.ObservableId, QUOTE_ID);
		assertEquals(test.Template, TEMPLATE);
		assertEquals(test.date(VAL_DATE, REF_DATA), date(2025, 6, 20));
	  }

	  public virtual void test_of_pardSpread()
	  {
		CdsIndexIsdaCreditCurveNode test = CdsIndexIsdaCreditCurveNode.ofParSpread(TEMPLATE_NS, QUOTE_ID, INDEX_ID, LEGAL_ENTITIES);
		assertEquals(test.Label, END_DATE.ToString());
		assertEquals(test.CdsIndexId, INDEX_ID);
		assertEquals(test.LegalEntityIds, LEGAL_ENTITIES);
		assertEquals(test.ObservableId, QUOTE_ID);
		assertEquals(test.Template, TEMPLATE_NS);
		assertEquals(test.date(VAL_DATE, REF_DATA), END_DATE);
	  }

	  public virtual void test_of_pointsUpfront()
	  {
		CdsIndexIsdaCreditCurveNode test = CdsIndexIsdaCreditCurveNode.ofPointsUpfront(TEMPLATE, QUOTE_ID, INDEX_ID, LEGAL_ENTITIES, 0.01);
		assertEquals(test.Label, LABEL_AUTO);
		assertEquals(test.CdsIndexId, INDEX_ID);
		assertEquals(test.LegalEntityIds, LEGAL_ENTITIES);
		assertEquals(test.ObservableId, QUOTE_ID);
		assertEquals(test.Template, TEMPLATE);
		assertEquals(test.date(VAL_DATE, REF_DATA), date(2025, 6, 20));
	  }

	  public virtual void test_build_fail_noRate()
	  {
		assertThrows(() => CdsIndexIsdaCreditCurveNode.builder().template(TEMPLATE).observableId(QUOTE_ID).cdsIndexId(INDEX_ID).legalEntityIds(LEGAL_ENTITIES).quoteConvention(CdsQuoteConvention.QUOTED_SPREAD).build(), typeof(System.ArgumentException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_trade()
	  {
		CdsIndexIsdaCreditCurveNode node = CdsIndexIsdaCreditCurveNode.ofQuotedSpread(TEMPLATE, QUOTE_ID, INDEX_ID, LEGAL_ENTITIES, 0.01);
		double rate = 0.0125;
		double quantity = -1234.56;
		MarketData marketData = ImmutableMarketData.builder(VAL_DATE).addValue(QUOTE_ID, rate).build();
		CdsIndexCalibrationTrade trade = node.trade(quantity, marketData, REF_DATA);
		CdsTrade cdsTrade = TEMPLATE.createTrade(INDEX_ID, VAL_DATE, SELL, -quantity, 0.01, REF_DATA);
		CdsIndex cdsIndex = CdsIndex.of(SELL, INDEX_ID, LEGAL_ENTITIES, TEMPLATE.Convention.Currency, -quantity, date(2015, 6, 20), date(2025, 6, 20), Frequency.P3M, TEMPLATE.Convention.SettlementDateOffset.Calendar, 0.01);
		CdsIndex cdsIndexMod = cdsIndex.toBuilder().paymentSchedule(cdsIndex.PaymentSchedule.toBuilder().rollConvention(RollConventions.DAY_20).startDateBusinessDayAdjustment(cdsIndex.PaymentSchedule.BusinessDayAdjustment).build()).build();
		CdsIndexTrade expected = CdsIndexTrade.builder().product(cdsIndexMod).info(cdsTrade.Info).build();
		assertEquals(trade.UnderlyingTrade, expected);
		assertEquals(trade.Quote, CdsQuote.of(CdsQuoteConvention.QUOTED_SPREAD, rate));

		CdsIndexIsdaCreditCurveNode node1 = CdsIndexIsdaCreditCurveNode.ofParSpread(TEMPLATE, QUOTE_ID, INDEX_ID, LEGAL_ENTITIES);
		CdsTrade cdsTrade1 = TEMPLATE.createTrade(INDEX_ID, VAL_DATE, SELL, -quantity, rate, REF_DATA);
		CdsIndexCalibrationTrade trade1 = node1.trade(quantity, marketData, REF_DATA);
		CdsIndex cdsIndex1 = CdsIndex.of(SELL, INDEX_ID, LEGAL_ENTITIES, TEMPLATE.Convention.Currency, -quantity, date(2015, 6, 20), date(2025, 6, 20), Frequency.P3M, TEMPLATE.Convention.SettlementDateOffset.Calendar, rate);
		CdsIndex cdsIndexMod1 = cdsIndex1.toBuilder().paymentSchedule(cdsIndex.PaymentSchedule.toBuilder().rollConvention(RollConventions.DAY_20).startDateBusinessDayAdjustment(cdsIndex1.PaymentSchedule.BusinessDayAdjustment).build()).build();
		CdsIndexTrade expected1 = CdsIndexTrade.builder().product(cdsIndexMod1).info(cdsTrade1.Info).build();
		assertEquals(trade1.UnderlyingTrade, expected1);
		assertEquals(trade1.Quote, CdsQuote.of(CdsQuoteConvention.PAR_SPREAD, rate));
	  }

	  public virtual void test_trade_noMarketData()
	  {
		CdsIndexIsdaCreditCurveNode node = CdsIndexIsdaCreditCurveNode.ofParSpread(TEMPLATE, QUOTE_ID, INDEX_ID, LEGAL_ENTITIES);
		MarketData marketData = MarketData.empty(VAL_DATE);
		assertThrows(() => node.trade(1d, marketData, REF_DATA), typeof(MarketDataNotFoundException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_metadata_tenor()
	  {
		CdsIndexIsdaCreditCurveNode node = CdsIndexIsdaCreditCurveNode.ofQuotedSpread(TEMPLATE, QUOTE_ID, INDEX_ID, LEGAL_ENTITIES, 0.01);
		LocalDate nodeDate = LocalDate.of(2015, 1, 22);
		ParameterMetadata metadata = node.metadata(nodeDate);
		assertEquals(((TenorDateParameterMetadata) metadata).Date, nodeDate);
		assertEquals(((TenorDateParameterMetadata) metadata).Tenor, Tenor.TENOR_10Y);
	  }

	  public virtual void test_metadata_dates()
	  {
		CdsIndexIsdaCreditCurveNode node = CdsIndexIsdaCreditCurveNode.ofParSpread(TEMPLATE_NS, QUOTE_ID, INDEX_ID, LEGAL_ENTITIES);
		ParameterMetadata metadata = node.metadata(END_DATE);
		assertEquals(((LabelDateParameterMetadata) metadata).Date, END_DATE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CdsIndexIsdaCreditCurveNode test1 = CdsIndexIsdaCreditCurveNode.ofQuotedSpread(TEMPLATE, QUOTE_ID, INDEX_ID, LEGAL_ENTITIES, 0.01);
		coverImmutableBean(test1);
		CdsIndexIsdaCreditCurveNode test2 = CdsIndexIsdaCreditCurveNode.ofPointsUpfront(TenorCdsTemplate.of(TENOR_10Y, CdsConventions.EUR_GB_STANDARD), QuoteId.of(StandardId.of("OG-Ticker", "Cdx2")), StandardId.of("OG", "DEF"), ImmutableList.of(StandardId.of("OG", "DEF1"), StandardId.of("OG", "DEF2")), 0.01);
		QuoteId.of(StandardId.of("OG-Ticker", "Deposit2"));
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		CdsIndexIsdaCreditCurveNode test = CdsIndexIsdaCreditCurveNode.ofQuotedSpread(TEMPLATE, QUOTE_ID, INDEX_ID, LEGAL_ENTITIES, 0.01);
		assertSerialization(test);
	  }

	}

}