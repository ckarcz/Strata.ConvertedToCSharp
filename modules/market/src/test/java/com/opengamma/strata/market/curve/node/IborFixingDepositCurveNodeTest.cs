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
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_6M;
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


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
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
	using IborFixingDeposit = com.opengamma.strata.product.deposit.IborFixingDeposit;
	using IborFixingDepositTrade = com.opengamma.strata.product.deposit.IborFixingDepositTrade;
	using ResolvedIborFixingDeposit = com.opengamma.strata.product.deposit.ResolvedIborFixingDeposit;
	using IborFixingDepositTemplate = com.opengamma.strata.product.deposit.type.IborFixingDepositTemplate;
	using ImmutableIborFixingDepositConvention = com.opengamma.strata.product.deposit.type.ImmutableIborFixingDepositConvention;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;

	/// <summary>
	/// Test <seealso cref="IborFixingDepositCurveNode"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborFixingDepositCurveNodeTest
	public class IborFixingDepositCurveNodeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = date(2015, 6, 30);
	  private static readonly QuoteId QUOTE_ID = QuoteId.of(StandardId.of("OG-Ticker", "Deposit1"));
	  private const double SPREAD = 0.0015;
	  private const string LABEL = "Label";
	  private const string LABEL_AUTO = "3M";
	  private static readonly IborFixingDepositTemplate TEMPLATE = IborFixingDepositTemplate.of(EUR_LIBOR_3M);

	  public virtual void test_builder()
	  {
		IborFixingDepositCurveNode test = IborFixingDepositCurveNode.builder().label(LABEL).rateId(QUOTE_ID).template(TEMPLATE).additionalSpread(SPREAD).build();
		assertEquals(test.Label, LABEL);
		assertEquals(test.RateId, QUOTE_ID);
		assertEquals(test.AdditionalSpread, SPREAD);
		assertEquals(test.Template, TEMPLATE);
		assertEquals(test.Date, CurveNodeDate.END);
	  }

	  public virtual void test_of_noSpread()
	  {
		IborFixingDepositCurveNode test = IborFixingDepositCurveNode.of(TEMPLATE, QUOTE_ID);
		assertEquals(test.Label, LABEL_AUTO);
		assertEquals(test.RateId, QUOTE_ID);
		assertEquals(test.AdditionalSpread, 0.0);
		assertEquals(test.Template, TEMPLATE);
	  }

	  public virtual void test_of_withSpread()
	  {
		IborFixingDepositCurveNode test = IborFixingDepositCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		assertEquals(test.Label, LABEL_AUTO);
		assertEquals(test.RateId, QUOTE_ID);
		assertEquals(test.AdditionalSpread, SPREAD);
		assertEquals(test.Template, TEMPLATE);
	  }

	  public virtual void test_of_withSpreadAndLabel()
	  {
		IborFixingDepositCurveNode test = IborFixingDepositCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD, LABEL);
		assertEquals(test.Label, LABEL);
		assertEquals(test.RateId, QUOTE_ID);
		assertEquals(test.AdditionalSpread, SPREAD);
		assertEquals(test.Template, TEMPLATE);
	  }

	  public virtual void test_requirements()
	  {
		IborFixingDepositCurveNode test = IborFixingDepositCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		ISet<ObservableId> set = test.requirements();
		IEnumerator<ObservableId> itr = set.GetEnumerator();
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(itr.next(), QUOTE_ID);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertFalse(itr.hasNext());
	  }

	  public virtual void test_trade()
	  {
		IborFixingDepositCurveNode node = IborFixingDepositCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		LocalDate valuationDate = LocalDate.of(2015, 1, 22);
		double rate = 0.035;
		MarketData marketData = ImmutableMarketData.builder(valuationDate).addValue(QUOTE_ID, rate).build();
		IborFixingDepositTrade trade = node.trade(1d, marketData, REF_DATA);
		ImmutableIborFixingDepositConvention conv = (ImmutableIborFixingDepositConvention) TEMPLATE.Convention;
		LocalDate startDateExpected = conv.SpotDateOffset.adjust(valuationDate, REF_DATA);
		LocalDate endDateExpected = startDateExpected.plus(TEMPLATE.DepositPeriod);
		IborFixingDeposit depositExpected = IborFixingDeposit.builder().buySell(BuySell.BUY).index(EUR_LIBOR_3M).startDate(startDateExpected).endDate(endDateExpected).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUR_LIBOR_3M.FixingCalendar)).notional(1.0d).fixedRate(rate + SPREAD).build();
		TradeInfo tradeInfoExpected = TradeInfo.builder().tradeDate(valuationDate).build();
		assertEquals(trade.Product, depositExpected);
		assertEquals(trade.Info, tradeInfoExpected);
	  }

	  public virtual void test_trade_noMarketData()
	  {
		IborFixingDepositCurveNode node = IborFixingDepositCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		MarketData marketData = MarketData.empty(VAL_DATE);
		assertThrows(() => node.trade(1d, marketData, REF_DATA), typeof(MarketDataNotFoundException));
	  }

	  public virtual void test_initialGuess()
	  {
		IborFixingDepositCurveNode node = IborFixingDepositCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		double rate = 0.035;
		MarketData marketData = ImmutableMarketData.builder(VAL_DATE).addValue(QUOTE_ID, rate).build();
		assertEquals(node.initialGuess(marketData, ValueType.ZERO_RATE), rate);
		assertEquals(node.initialGuess(marketData, ValueType.FORWARD_RATE), rate);
		assertEquals(node.initialGuess(marketData, ValueType.DISCOUNT_FACTOR), Math.Exp(-rate * 0.25d), 1.0E-12);
	  }

	  public virtual void test_metadata_end()
	  {
		IborFixingDepositCurveNode node = IborFixingDepositCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		LocalDate valuationDate = LocalDate.of(2015, 1, 22);
		ParameterMetadata metadata = node.metadata(valuationDate, REF_DATA);
		assertEquals(((TenorDateParameterMetadata) metadata).Date, LocalDate.of(2015, 4, 27));
		assertEquals(((TenorDateParameterMetadata) metadata).Tenor, Tenor.TENOR_3M);
	  }

	  public virtual void test_metadata_fixed()
	  {
		LocalDate nodeDate = VAL_DATE.plusMonths(1);
		IborFixingDepositCurveNode node = IborFixingDepositCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD).withDate(CurveNodeDate.of(nodeDate));
		DatedParameterMetadata metadata = node.metadata(VAL_DATE, REF_DATA);
		assertEquals(metadata.Date, nodeDate);
		assertEquals(metadata.Label, node.Label);
	  }

	  public virtual void test_metadata_last_fixing()
	  {
		IborFixingDepositCurveNode node = IborFixingDepositCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD).withDate(CurveNodeDate.LAST_FIXING);
		ImmutableMarketData marketData = ImmutableMarketData.builder(VAL_DATE).addValue(QUOTE_ID, 0.0d).build();
		IborFixingDepositTrade trade = node.trade(1d, marketData, REF_DATA);
		ResolvedIborFixingDeposit product = trade.Product.resolve(REF_DATA);
		LocalDate fixingDate = ((IborRateComputation) product.FloatingRate).FixingDate;
		DatedParameterMetadata metadata = node.metadata(VAL_DATE, REF_DATA);
		assertEquals(((TenorDateParameterMetadata) metadata).Date, fixingDate);
		assertEquals(((TenorDateParameterMetadata) metadata).Tenor.Period, TEMPLATE.DepositPeriod);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		IborFixingDepositCurveNode test = IborFixingDepositCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		coverImmutableBean(test);
		IborFixingDepositCurveNode test2 = IborFixingDepositCurveNode.of(IborFixingDepositTemplate.of(GBP_LIBOR_6M), QuoteId.of(StandardId.of("OG-Ticker", "Deposit2")));
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		IborFixingDepositCurveNode test = IborFixingDepositCurveNode.of(TEMPLATE, QUOTE_ID, SPREAD);
		assertSerialization(test);
	  }

	}

}