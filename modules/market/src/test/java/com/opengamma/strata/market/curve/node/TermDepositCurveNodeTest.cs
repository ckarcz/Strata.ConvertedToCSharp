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
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
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
//	import static org.testng.Assert.assertFalse;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using DatedParameterMetadata = com.opengamma.strata.market.param.DatedParameterMetadata;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using TenorDateParameterMetadata = com.opengamma.strata.market.param.TenorDateParameterMetadata;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using TermDeposit = com.opengamma.strata.product.deposit.TermDeposit;
	using TermDepositTrade = com.opengamma.strata.product.deposit.TermDepositTrade;
	using TermDepositConvention = com.opengamma.strata.product.deposit.type.TermDepositConvention;
	using TermDepositConventions = com.opengamma.strata.product.deposit.type.TermDepositConventions;
	using TermDepositTemplate = com.opengamma.strata.product.deposit.type.TermDepositTemplate;

	/// <summary>
	/// Test <seealso cref="TermDepositCurveNode"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TermDepositCurveNodeTest
	public class TermDepositCurveNodeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = date(2015, 6, 30);
	  private static readonly BusinessDayAdjustment BDA_MOD_FOLLOW = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA);
	  private static readonly DaysAdjustment PLUS_TWO_DAYS = DaysAdjustment.ofBusinessDays(2, EUTA);
	  private static readonly TermDepositConvention CONVENTION = TermDepositConventions.EUR_DEPOSIT_T2;
	  private static readonly Period DEPOSIT_PERIOD = Period.ofMonths(3);
	  private static readonly TermDepositTemplate TEMPLATE = TermDepositTemplate.of(DEPOSIT_PERIOD, CONVENTION);
	  private static readonly QuoteId QUOTE_ID = QuoteId.of(StandardId.of("OG-Ticker", "Deposit1"));
	  private const double SPREAD = 0.0015;
	  private const string LABEL = "Label";
	  private const string LABEL_AUTO = "3M";

	  public virtual void test_builder()
	  {
		TermDepositCurveNode test = TermDepositCurveNode.builder().label(LABEL).template(TEMPLATE).rateId(QUOTE_ID).additionalSpread(SPREAD).date(CurveNodeDate.LAST_FIXING).build();
		assertEquals(test.Label, LABEL);
		assertEquals(test.RateId, QUOTE_ID);
		assertEquals(test.AdditionalSpread, SPREAD);
		assertEquals(test.Template, TEMPLATE);
		assertEquals(test.Date, CurveNodeDate.LAST_FIXING);
	  }

	  public virtual void test_builder_defaults()
	  {
		TermDepositCurveNode test = TermDepositCurveNode.builder().label(LABEL).template(TEMPLATE).rateId(QUOTE_ID).additionalSpread(SPREAD).build();
		assertEquals(test.Label, LABEL);
		assertEquals(test.RateId, QUOTE_ID);
		assertEquals(test.AdditionalSpread, SPREAD);
		assertEquals(test.Template, TEMPLATE);
		assertEquals(test.Date, CurveNodeDate.END);
	  }

	  public virtual void test_of_noSpread()
	  {
		TermDepositCurveNode test = TermDepositCurveNode.of(TEMPLATE, QUOTE_ID);
		assertEquals(test.Label, LABEL_AUTO);
		assertEquals(test.RateId, QUOTE_ID);
		assertEquals(test.AdditionalSpread, 0.0d);
		assertEquals(test.Template, TEMPLATE);
	  }

	  public virtual void test_of_withSpread()
	  {
		TermDepositCurveNode test = TermDepositCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		assertEquals(test.Label, LABEL_AUTO);
		assertEquals(test.RateId, QUOTE_ID);
		assertEquals(test.AdditionalSpread, SPREAD);
		assertEquals(test.Template, TEMPLATE);
	  }

	  public virtual void test_of_withSpreadAndLabel()
	  {
		TermDepositCurveNode test = TermDepositCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD, LABEL);
		assertEquals(test.Label, LABEL);
		assertEquals(test.RateId, QUOTE_ID);
		assertEquals(test.AdditionalSpread, SPREAD);
		assertEquals(test.Template, TEMPLATE);
	  }

	  public virtual void test_requirements()
	  {
		TermDepositCurveNode test = TermDepositCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		ISet<ObservableId> set = test.requirements();
		IEnumerator<ObservableId> itr = set.GetEnumerator();
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(itr.next(), QUOTE_ID);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertFalse(itr.hasNext());
	  }

	  public virtual void test_trade()
	  {
		TermDepositCurveNode node = TermDepositCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		double rate = 0.035;
		MarketData marketData = ImmutableMarketData.builder(VAL_DATE).addValue(QUOTE_ID, rate).build();
		TermDepositTrade trade = node.trade(1d, marketData, REF_DATA);
		LocalDate startDateExpected = PLUS_TWO_DAYS.adjust(VAL_DATE, REF_DATA);
		LocalDate endDateExpected = startDateExpected.plus(DEPOSIT_PERIOD);
		TermDeposit depositExpected = TermDeposit.builder().buySell(BuySell.BUY).currency(EUR).dayCount(ACT_360).startDate(startDateExpected).endDate(endDateExpected).notional(1.0d).businessDayAdjustment(BDA_MOD_FOLLOW).rate(rate + SPREAD).build();
		TradeInfo tradeInfoExpected = TradeInfo.builder().tradeDate(VAL_DATE).build();
		assertEquals(trade.Product, depositExpected);
		assertEquals(trade.Info, tradeInfoExpected);
	  }

	  public virtual void test_trade_noMarketData()
	  {
		TermDepositCurveNode node = TermDepositCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		LocalDate valuationDate = LocalDate.of(2015, 1, 22);
		MarketData marketData = MarketData.empty(valuationDate);
		assertThrows(() => node.trade(1d, marketData, REF_DATA), typeof(MarketDataNotFoundException));
	  }

	  public virtual void test_initialGuess()
	  {
		TermDepositCurveNode node = TermDepositCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		double rate = 0.035;
		MarketData marketData = ImmutableMarketData.builder(VAL_DATE).addValue(QUOTE_ID, rate).build();
		assertEquals(node.initialGuess(marketData, ValueType.ZERO_RATE), rate);
		assertEquals(node.initialGuess(marketData, ValueType.FORWARD_RATE), rate);
		assertEquals(node.initialGuess(marketData, ValueType.DISCOUNT_FACTOR), Math.Exp(-rate * 0.25), 1.0e-12);
	  }

	  public virtual void test_metadata_end()
	  {
		TermDepositCurveNode node = TermDepositCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		LocalDate valuationDate = LocalDate.of(2015, 1, 22);
		ParameterMetadata metadata = node.metadata(valuationDate, REF_DATA);
		assertEquals(((TenorDateParameterMetadata) metadata).Date, LocalDate.of(2015, 4, 27));
		assertEquals(((TenorDateParameterMetadata) metadata).Tenor, Tenor.TENOR_3M);
	  }

	  public virtual void test_metadata_fixed()
	  {
		LocalDate nodeDate = VAL_DATE.plusMonths(1);
		TermDepositCurveNode node = TermDepositCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD).withDate(CurveNodeDate.of(nodeDate));
		LocalDate valuationDate = LocalDate.of(2015, 1, 22);
		DatedParameterMetadata metadata = node.metadata(valuationDate, REF_DATA);
		assertEquals(metadata.Date, nodeDate);
		assertEquals(metadata.Label, node.Label);
	  }

	  public virtual void test_metadata_last_fixing()
	  {
		TermDepositCurveNode node = TermDepositCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD).withDate(CurveNodeDate.LAST_FIXING);
		assertThrowsWithCause(() => node.metadata(VAL_DATE, REF_DATA), typeof(System.NotSupportedException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		TermDepositCurveNode test = TermDepositCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		coverImmutableBean(test);
		TermDepositCurveNode test2 = TermDepositCurveNode.of(TermDepositTemplate.of(Period.ofMonths(1), CONVENTION), QuoteId.of(StandardId.of("OG-Ticker", "Deposit2")));
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		TermDepositCurveNode test = TermDepositCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		assertSerialization(test);
	  }

	}

}