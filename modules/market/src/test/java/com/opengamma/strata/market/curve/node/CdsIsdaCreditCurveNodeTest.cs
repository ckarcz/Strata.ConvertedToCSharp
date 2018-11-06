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

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using LabelDateParameterMetadata = com.opengamma.strata.market.param.LabelDateParameterMetadata;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using TenorDateParameterMetadata = com.opengamma.strata.market.param.TenorDateParameterMetadata;
	using CdsCalibrationTrade = com.opengamma.strata.product.credit.CdsCalibrationTrade;
	using CdsQuote = com.opengamma.strata.product.credit.CdsQuote;
	using CdsTrade = com.opengamma.strata.product.credit.CdsTrade;
	using CdsConventions = com.opengamma.strata.product.credit.type.CdsConventions;
	using CdsQuoteConvention = com.opengamma.strata.product.credit.type.CdsQuoteConvention;
	using CdsTemplate = com.opengamma.strata.product.credit.type.CdsTemplate;
	using DatesCdsTemplate = com.opengamma.strata.product.credit.type.DatesCdsTemplate;
	using TenorCdsTemplate = com.opengamma.strata.product.credit.type.TenorCdsTemplate;

	/// <summary>
	/// Test {@code CdsIsdaCreditCurveNode}.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CdsIsdaCreditCurveNodeTest
	public class CdsIsdaCreditCurveNodeTest
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
	  private static readonly StandardId LEGAL_ENTITY = StandardId.of("OG", "ABC");

	  public virtual void test_builder()
	  {
		CdsIsdaCreditCurveNode test = CdsIsdaCreditCurveNode.builder().label(LABEL).template(TEMPLATE).observableId(QUOTE_ID).quoteConvention(CdsQuoteConvention.PAR_SPREAD).legalEntityId(LEGAL_ENTITY).build();
		assertEquals(test.Label, LABEL);
		assertEquals(test.LegalEntityId, LEGAL_ENTITY);
		assertEquals(test.ObservableId, QUOTE_ID);
		assertEquals(test.Template, TEMPLATE);
		assertEquals(test.date(VAL_DATE, REF_DATA), date(2025, 6, 20));
	  }

	  public virtual void test_of_quotedSpread()
	  {
		CdsIsdaCreditCurveNode test = CdsIsdaCreditCurveNode.ofQuotedSpread(TEMPLATE, QUOTE_ID, LEGAL_ENTITY, 0.01);
		assertEquals(test.Label, LABEL_AUTO);
		assertEquals(test.LegalEntityId, LEGAL_ENTITY);
		assertEquals(test.ObservableId, QUOTE_ID);
		assertEquals(test.Template, TEMPLATE);
		assertEquals(test.date(VAL_DATE, REF_DATA), date(2025, 6, 20));
	  }

	  public virtual void test_of_pardSpread()
	  {
		CdsIsdaCreditCurveNode test = CdsIsdaCreditCurveNode.ofParSpread(TEMPLATE_NS, QUOTE_ID, LEGAL_ENTITY);
		assertEquals(test.Label, END_DATE.ToString());
		assertEquals(test.LegalEntityId, LEGAL_ENTITY);
		assertEquals(test.ObservableId, QUOTE_ID);
		assertEquals(test.Template, TEMPLATE_NS);
		assertEquals(test.date(VAL_DATE, REF_DATA), END_DATE);
	  }

	  public virtual void test_of_pointsUpfront()
	  {
		CdsIsdaCreditCurveNode test = CdsIsdaCreditCurveNode.ofPointsUpfront(TEMPLATE, QUOTE_ID, LEGAL_ENTITY, 0.01);
		assertEquals(test.Label, LABEL_AUTO);
		assertEquals(test.LegalEntityId, LEGAL_ENTITY);
		assertEquals(test.ObservableId, QUOTE_ID);
		assertEquals(test.Template, TEMPLATE);
		assertEquals(test.date(VAL_DATE, REF_DATA), date(2025, 6, 20));
	  }

	  public virtual void test_build_fail_noRate()
	  {
		assertThrows(() => CdsIsdaCreditCurveNode.builder().template(TEMPLATE).observableId(QUOTE_ID).legalEntityId(LEGAL_ENTITY).quoteConvention(CdsQuoteConvention.QUOTED_SPREAD).build(), typeof(System.ArgumentException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_trade()
	  {
		CdsIsdaCreditCurveNode node = CdsIsdaCreditCurveNode.ofQuotedSpread(TEMPLATE, QUOTE_ID, LEGAL_ENTITY, 0.01);
		double rate = 0.0125;
		double quantity = -1234.56;
		MarketData marketData = ImmutableMarketData.builder(VAL_DATE).addValue(QUOTE_ID, rate).build();
		CdsCalibrationTrade trade = node.trade(quantity, marketData, REF_DATA);
		CdsTrade expected = TEMPLATE.createTrade(LEGAL_ENTITY, VAL_DATE, SELL, -quantity, 0.01, REF_DATA);
		assertEquals(trade.UnderlyingTrade, expected);
		assertEquals(trade.Quote, CdsQuote.of(CdsQuoteConvention.QUOTED_SPREAD, rate));

		CdsIsdaCreditCurveNode node1 = CdsIsdaCreditCurveNode.ofParSpread(TEMPLATE, QUOTE_ID, LEGAL_ENTITY);
		CdsTrade expected1 = TEMPLATE.createTrade(LEGAL_ENTITY, VAL_DATE, SELL, -quantity, rate, REF_DATA);
		CdsCalibrationTrade trade1 = node1.trade(quantity, marketData, REF_DATA);
		assertEquals(trade1.UnderlyingTrade, expected1);
		assertEquals(trade1.Quote, CdsQuote.of(CdsQuoteConvention.PAR_SPREAD, rate));
	  }

	  public virtual void test_trade_noMarketData()
	  {
		CdsIsdaCreditCurveNode node = CdsIsdaCreditCurveNode.ofParSpread(TEMPLATE, QUOTE_ID, LEGAL_ENTITY);
		MarketData marketData = MarketData.empty(VAL_DATE);
		assertThrows(() => node.trade(1d, marketData, REF_DATA), typeof(MarketDataNotFoundException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_metadata_tenor()
	  {
		CdsIsdaCreditCurveNode node = CdsIsdaCreditCurveNode.ofQuotedSpread(TEMPLATE, QUOTE_ID, LEGAL_ENTITY, 0.01);
		LocalDate nodeDate = LocalDate.of(2015, 1, 22);
		ParameterMetadata metadata = node.metadata(nodeDate);
		assertEquals(((TenorDateParameterMetadata) metadata).Date, nodeDate);
		assertEquals(((TenorDateParameterMetadata) metadata).Tenor, Tenor.TENOR_10Y);
	  }

	  public virtual void test_metadata_dates()
	  {
		CdsIsdaCreditCurveNode node = CdsIsdaCreditCurveNode.ofParSpread(TEMPLATE_NS, QUOTE_ID, LEGAL_ENTITY);
		ParameterMetadata metadata = node.metadata(END_DATE);
		assertEquals(((LabelDateParameterMetadata) metadata).Date, END_DATE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CdsIsdaCreditCurveNode test1 = CdsIsdaCreditCurveNode.ofQuotedSpread(TEMPLATE, QUOTE_ID, LEGAL_ENTITY, 0.01);
		coverImmutableBean(test1);
		CdsIsdaCreditCurveNode test2 = CdsIsdaCreditCurveNode.ofPointsUpfront(TenorCdsTemplate.of(TENOR_10Y, CdsConventions.EUR_GB_STANDARD), QuoteId.of(StandardId.of("OG-Ticker", "Cds2")), StandardId.of("OG", "DEF"), 0.01);
		QuoteId.of(StandardId.of("OG-Ticker", "Deposit2"));
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		CdsIsdaCreditCurveNode test = CdsIsdaCreditCurveNode.ofQuotedSpread(TEMPLATE, QUOTE_ID, LEGAL_ENTITY, 0.01);
		assertSerialization(test);
	  }

	}

}