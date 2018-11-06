using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.fpml
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.AUD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.CHF;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.INR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.JPY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.THIRTY_360_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.THIRTY_E_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.AUSY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.CHZU;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.FRPA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.JPTO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.SAT_SUN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.AUD_BBSW_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.CHF_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.CHF_LIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_LIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.EUR_EONIA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertEqualsBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.BUY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.SELL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.fail;


	using Bean = org.joda.beans.Bean;
	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableListMultimap = com.google.common.collect.ImmutableListMultimap;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Iterables = com.google.common.collect.Iterables;
	using ByteSource = com.google.common.io.ByteSource;
	using ImmutableReferenceData = com.opengamma.strata.basics.ImmutableReferenceData;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using HolidayCalendars = com.opengamma.strata.basics.date.HolidayCalendars;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using ImmutableFxIndex = com.opengamma.strata.basics.index.ImmutableFxIndex;
	using PriceIndices = com.opengamma.strata.basics.index.PriceIndices;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConvention = com.opengamma.strata.basics.schedule.RollConvention;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using ValueAdjustment = com.opengamma.strata.basics.value.ValueAdjustment;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using ValueStep = com.opengamma.strata.basics.value.ValueStep;
	using ValueStepSequence = com.opengamma.strata.basics.value.ValueStepSequence;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using XmlElement = com.opengamma.strata.collect.io.XmlElement;
	using Trade = com.opengamma.strata.product.Trade;
	using LongShort = com.opengamma.strata.product.common.LongShort;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using Cds = com.opengamma.strata.product.credit.Cds;
	using CdsIndex = com.opengamma.strata.product.credit.CdsIndex;
	using CdsIndexTrade = com.opengamma.strata.product.credit.CdsIndexTrade;
	using CdsTrade = com.opengamma.strata.product.credit.CdsTrade;
	using TermDeposit = com.opengamma.strata.product.deposit.TermDeposit;
	using TermDepositTrade = com.opengamma.strata.product.deposit.TermDepositTrade;
	using Fra = com.opengamma.strata.product.fra.Fra;
	using FraDiscountingMethod = com.opengamma.strata.product.fra.FraDiscountingMethod;
	using FraTrade = com.opengamma.strata.product.fra.FraTrade;
	using FxNdf = com.opengamma.strata.product.fx.FxNdf;
	using FxNdfTrade = com.opengamma.strata.product.fx.FxNdfTrade;
	using FxSingle = com.opengamma.strata.product.fx.FxSingle;
	using FxSingleTrade = com.opengamma.strata.product.fx.FxSingleTrade;
	using FxSwap = com.opengamma.strata.product.fx.FxSwap;
	using FxSwapTrade = com.opengamma.strata.product.fx.FxSwapTrade;
	using BulletPayment = com.opengamma.strata.product.payment.BulletPayment;
	using BulletPaymentTrade = com.opengamma.strata.product.payment.BulletPaymentTrade;
	using FixedRateComputation = com.opengamma.strata.product.rate.FixedRateComputation;
	using IborInterpolatedRateComputation = com.opengamma.strata.product.rate.IborInterpolatedRateComputation;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;
	using CompoundingMethod = com.opengamma.strata.product.swap.CompoundingMethod;
	using FixedRateCalculation = com.opengamma.strata.product.swap.FixedRateCalculation;
	using IborRateCalculation = com.opengamma.strata.product.swap.IborRateCalculation;
	using IborRateResetMethod = com.opengamma.strata.product.swap.IborRateResetMethod;
	using IborRateStubCalculation = com.opengamma.strata.product.swap.IborRateStubCalculation;
	using InflationRateCalculation = com.opengamma.strata.product.swap.InflationRateCalculation;
	using NotionalSchedule = com.opengamma.strata.product.swap.NotionalSchedule;
	using OvernightRateCalculation = com.opengamma.strata.product.swap.OvernightRateCalculation;
	using PaymentSchedule = com.opengamma.strata.product.swap.PaymentSchedule;
	using PriceIndexCalculationMethod = com.opengamma.strata.product.swap.PriceIndexCalculationMethod;
	using RateAccrualPeriod = com.opengamma.strata.product.swap.RateAccrualPeriod;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using RatePaymentPeriod = com.opengamma.strata.product.swap.RatePaymentPeriod;
	using ResetSchedule = com.opengamma.strata.product.swap.ResetSchedule;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;
	using SwapLegType = com.opengamma.strata.product.swap.SwapLegType;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;
	using PhysicalSwaptionSettlement = com.opengamma.strata.product.swaption.PhysicalSwaptionSettlement;
	using Swaption = com.opengamma.strata.product.swaption.Swaption;
	using SwaptionTrade = com.opengamma.strata.product.swaption.SwaptionTrade;

	/// <summary>
	/// Test <seealso cref="FpmlDocumentParser"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FpmlDocumentParserTest
	public class FpmlDocumentParserTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly HolidayCalendarId GBLO_USNY = GBLO.combinedWith(USNY);
	  private static readonly HolidayCalendarId GBLO_EUTA = GBLO.combinedWith(EUTA);
	  private static readonly HolidayCalendarId GBLO_USNY_JPTO = GBLO.combinedWith(USNY).combinedWith(JPTO);

	  //-------------------------------------------------------------------------
	  public virtual void bulletPayment()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/ird-ex28-bullet-payments.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party1")).parseTrades(resource);
		assertEquals(trades.Count, 1);
		Trade trade = trades[0];
		assertEquals(trade.GetType(), typeof(BulletPaymentTrade));
		BulletPaymentTrade bpTrade = (BulletPaymentTrade) trade;
		assertEquals(bpTrade.Info.TradeDate, date(2001, 4, 29));
		BulletPayment bp = bpTrade.Product;
		assertEquals(bp.PayReceive, PAY);
		assertEquals(bp.Date, AdjustableDate.of(date(2001, 7, 27), BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO_USNY)));
		assertEquals(bp.Value, CurrencyAmount.of(USD, 15000));
	  }

	  public virtual void bulletPayment_twoTradesTwoParties()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/bullet-payment-weird.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		FpmlPartySelector selector = FpmlPartySelector.matchingRegex(Pattern.compile("Party1[ab]"));
		IList<Trade> trades = FpmlDocumentParser.of(selector).parseTrades(resource);
		assertEquals(trades.Count, 2);
		Trade trade0 = trades[0];
		assertEquals(trade0.GetType(), typeof(BulletPaymentTrade));
		BulletPaymentTrade bpTrade0 = (BulletPaymentTrade) trade0;
		assertEquals(bpTrade0.Info.TradeDate, date(2001, 4, 29));
		assertEquals(bpTrade0.Info.Id.get().Value, "123");
		BulletPayment bp0 = bpTrade0.Product;
		assertEquals(bp0.PayReceive, PAY);
		assertEquals(bp0.Date, AdjustableDate.of(date(2001, 7, 27), BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO_USNY)));
		assertEquals(bp0.Value, CurrencyAmount.of(USD, 15000));
		Trade trade1 = trades[1];
		assertEquals(trade1.GetType(), typeof(BulletPaymentTrade));
		BulletPaymentTrade bpTrade1 = (BulletPaymentTrade) trade1;
		assertEquals(bpTrade1.Info.TradeDate, date(2001, 4, 29));
		assertEquals(bpTrade1.Info.Id.get().Value, "124");
		BulletPayment bp1 = bpTrade1.Product;
		assertEquals(bp1.PayReceive, RECEIVE);
		assertEquals(bp1.Date, AdjustableDate.of(date(2001, 8, 27), BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO_USNY)));
		assertEquals(bp1.Value, CurrencyAmount.of(USD, 15000));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void termDeposit()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/td-ex01-simple-term-deposit.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party1")).parseTrades(resource);
		assertEquals(trades.Count, 1);
		Trade trade = trades[0];
		assertEquals(trade.GetType(), typeof(TermDepositTrade));
		TermDepositTrade tdTrade = (TermDepositTrade) trade;
		assertEquals(tdTrade.Info.TradeDate, date(2002, 2, 14));
		TermDeposit td = tdTrade.Product;
		assertEquals(td.BuySell, BUY);
		assertEquals(td.StartDate, date(2002, 2, 14));
		assertEquals(td.EndDate, date(2002, 2, 15));
		assertEquals(td.Currency, CHF);
		assertEquals(td.Notional, 25000000d);
		assertEquals(td.Rate, 0.04);
		assertEquals(td.DayCount, ACT_360);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void fxSpot()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/fx-ex01-fx-spot.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party1")).parseTrades(resource);
		assertEquals(trades.Count, 1);
		Trade trade = trades[0];
		assertEquals(trade.GetType(), typeof(FxSingleTrade));
		FxSingleTrade fxTrade = (FxSingleTrade) trade;
		assertEquals(fxTrade.Info.TradeDate, date(2001, 10, 23));
		FxSingle fx = fxTrade.Product;
		assertEquals(fx.BaseCurrencyPayment, Payment.of(GBP, 10000000, date(2001, 10, 25)));
		assertEquals(fx.CounterCurrencyPayment, Payment.of(USD, -14800000, date(2001, 10, 25)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void fxForward()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/fx-ex03-fx-fwd.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party1")).parseTrades(resource);
		assertEquals(trades.Count, 1);
		Trade trade = trades[0];
		assertEquals(trade.GetType(), typeof(FxSingleTrade));
		FxSingleTrade fxTrade = (FxSingleTrade) trade;
		assertEquals(fxTrade.Info.TradeDate, date(2001, 11, 19));
		FxSingle fx = fxTrade.Product;
		assertEquals(fx.BaseCurrencyPayment, Payment.of(EUR, 10000000, date(2001, 12, 21)));
		assertEquals(fx.CounterCurrencyPayment, Payment.of(USD, -9175000, date(2001, 12, 21)));
	  }

	  public virtual void fxForward_splitDate()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/fx-ex03-fx-fwd-split-date.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party1")).parseTrades(resource);
		assertEquals(trades.Count, 1);
		Trade trade = trades[0];
		assertEquals(trade.GetType(), typeof(FxSingleTrade));
		FxSingleTrade fxTrade = (FxSingleTrade) trade;
		assertEquals(fxTrade.Info.TradeDate, date(2001, 11, 19));
		FxSingle fx = fxTrade.Product;
		assertEquals(fx.BaseCurrencyPayment, Payment.of(EUR, 10000000, date(2001, 12, 21)));
		assertEquals(fx.CounterCurrencyPayment, Payment.of(USD, -9175000, date(2001, 12, 22)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void fxNdf()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/fx-ex07-non-deliverable-forward.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party1")).parseTrades(resource);
		assertEquals(trades.Count, 1);
		Trade trade = trades[0];
		assertEquals(trade.GetType(), typeof(FxNdfTrade));
		FxNdfTrade fxTrade = (FxNdfTrade) trade;
		assertEquals(fxTrade.Info.TradeDate, date(2002, 1, 9));
		FxNdf fx = fxTrade.Product;
		assertEquals(fx.SettlementCurrencyNotional, CurrencyAmount.of(USD, 10000000));
		assertEquals(fx.AgreedFxRate, FxRate.of(USD, INR, 43.4));
		assertEquals(fx.Index, ImmutableFxIndex.builder().name("Reuters/RBIB/14:30").currencyPair(CurrencyPair.of(USD, INR)).fixingCalendar(USNY).maturityDateOffset(DaysAdjustment.ofCalendarDays(-2)).build());
		assertEquals(fx.PaymentDate, date(2002, 4, 11));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void fxSwap()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/fx-ex08-fx-swap.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party1")).parseTrades(resource);
		assertEquals(trades.Count, 1);
		Trade trade = trades[0];
		assertEquals(trade.GetType(), typeof(FxSwapTrade));
		FxSwapTrade fxTrade = (FxSwapTrade) trade;
		assertEquals(fxTrade.Info.TradeDate, date(2002, 1, 23));
		FxSwap fx = fxTrade.Product;
		FxSingle nearLeg = fx.NearLeg;
		assertEquals(nearLeg.BaseCurrencyPayment, Payment.of(GBP, 10000000, date(2002, 1, 25)));
		assertEquals(nearLeg.CounterCurrencyPayment, Payment.of(USD, -14800000, date(2002, 1, 25)));
		FxSingle farLeg = fx.FarLeg;
		assertEquals(farLeg.BaseCurrencyPayment, Payment.of(GBP, -10000000, date(2002, 2, 25)));
		assertEquals(farLeg.CounterCurrencyPayment, Payment.of(USD, 15000000, date(2002, 2, 25)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void swaption()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/ird-ex10-euro-swaption-relative.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party1")).parseTrades(resource);
		assertEquals(trades.Count, 1);
		Trade trade = trades[0];
		assertEquals(trade.GetType(), typeof(SwaptionTrade));
		SwaptionTrade swaptionTrade = (SwaptionTrade) trade;
		assertEquals(swaptionTrade.Info.TradeDate, date(1992, 8, 30));
		Swaption swaption = swaptionTrade.Product;

		//Test the parsing of the underlying swap
		Swap swap = swaption.Underlying;
		NotionalSchedule notional = NotionalSchedule.of(EUR, 50000000d);
		RateCalculationSwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.PAY).accrualSchedule(PeriodicSchedule.builder().startDate(date(1994, 12, 14)).endDate(date(1999, 12, 14)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, FRPA)).frequency(Frequency.P6M).rollConvention(RollConvention.ofDayOfMonth(14)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, FRPA))).build()).notionalSchedule(notional).calculation(IborRateCalculation.builder().index(EUR_LIBOR_6M).dayCount(ACT_360).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, GBLO)).build()).build();
		RateCalculationSwapLeg recLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(date(1994, 12, 14)).endDate(date(1999, 12, 14)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, FRPA)).frequency(Frequency.P12M).rollConvention(RollConvention.ofDayOfMonth(14)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P12M).paymentDateOffset(DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, FRPA))).build()).notionalSchedule(notional).calculation(FixedRateCalculation.builder().dayCount(THIRTY_E_360).rate(ValueSchedule.of(0.06)).build()).build();
		assertEqualsBean((Bean) swap.Legs.get(0), payLeg);
		assertEqualsBean((Bean) swap.Legs.get(1), recLeg);

		//Test the parsing of the option part of the swaption
		Swap underylingSwap = Swap.of(payLeg, recLeg);
		AdjustableDate expiryDate = AdjustableDate.of(LocalDate.of(1993, 8, 28), BusinessDayAdjustment.of(FOLLOWING, GBLO_EUTA));
		LocalTime expiryTime = LocalTime.of(11, 0, 0);
		ZoneId expiryZone = ZoneId.of("Europe/Brussels");
		Swaption swaptionExpected = Swaption.builder().expiryDate(expiryDate).expiryZone(expiryZone).expiryTime(expiryTime).longShort(LongShort.LONG).swaptionSettlement(PhysicalSwaptionSettlement.DEFAULT).underlying(underylingSwap).build();
		assertEqualsBean((Bean) swaption, swaptionExpected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void fra()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/ird-ex08-fra.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party2")).parseTrades(resource);
		assertFra(trades, false);
	  }

	  private void assertFra(IList<Trade> trades, bool interpolatedParty1)
	  {
		assertEquals(trades.Count, 1);
		Trade trade = trades[0];
		assertEquals(trade.GetType(), typeof(FraTrade));
		FraTrade fraTrade = (FraTrade) trade;
		assertEquals(fraTrade.Info.TradeDate, date(1991, 5, 14));
		StandardId party1id = StandardId.of("http://www.hsbc.com/swaps/trade-id", "MB87623");
		StandardId party2id = StandardId.of("http://www.abnamro.com/swaps/trade-id", "AA9876");
		assertEquals(fraTrade.Info.Id, (interpolatedParty1 ? party1id : party2id));
		Fra fra = fraTrade.Product;
		assertEquals(fra.BuySell, interpolatedParty1 ? BUY : SELL);
		assertEquals(fra.StartDate, date(1991, 7, 17));
		assertEquals(fra.EndDate, date(1992, 1, 17));
		assertEquals(fra.BusinessDayAdjustment, null);
		assertEquals(fra.PaymentDate.Unadjusted, date(1991, 7, 17));
		assertEquals(fra.PaymentDate.Adjustment, BusinessDayAdjustment.of(FOLLOWING, CHZU));
		assertEquals(fra.FixingDateOffset.Days, -2);
		assertEquals(fra.FixingDateOffset.Calendar, GBLO);
		assertEquals(fra.FixingDateOffset.Adjustment, BusinessDayAdjustment.NONE);
		assertEquals(fra.DayCount, ACT_360);
		assertEquals(fra.Currency, CHF);
		assertEquals(fra.Notional, 25000000d);
		assertEquals(fra.FixedRate, 0.04d);
		assertEquals(fra.Index, interpolatedParty1 ? CHF_LIBOR_3M : CHF_LIBOR_6M);
		assertEquals(fra.IndexInterpolated, interpolatedParty1 ? CHF_LIBOR_6M : null);
		assertEquals(fra.Discounting, FraDiscountingMethod.ISDA);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void fra_noParty()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/ird-ex08-fra.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.any()).parseTrades(resource);
		assertEquals(trades.Count, 1);
		Trade trade = trades[0];
		assertEquals(trade.GetType(), typeof(FraTrade));
		FraTrade fraTrade = (FraTrade) trade;
		assertEquals(fraTrade.Info.TradeDate, date(1991, 5, 14));
		Fra fra = fraTrade.Product;
		assertEquals(fra.BuySell, BUY);
		assertEquals(fra.StartDate, date(1991, 7, 17));
		assertEquals(fra.EndDate, date(1992, 1, 17));
		assertEquals(fra.BusinessDayAdjustment, null);
		assertEquals(fra.PaymentDate.Unadjusted, date(1991, 7, 17));
		assertEquals(fra.PaymentDate.Adjustment, BusinessDayAdjustment.of(FOLLOWING, CHZU));
		assertEquals(fra.FixingDateOffset.Days, -2);
		assertEquals(fra.FixingDateOffset.Calendar, GBLO);
		assertEquals(fra.FixingDateOffset.Adjustment, BusinessDayAdjustment.NONE);
		assertEquals(fra.DayCount, ACT_360);
		assertEquals(fra.Currency, CHF);
		assertEquals(fra.Notional, 25000000d);
		assertEquals(fra.FixedRate, 0.04d);
		assertEquals(fra.Index, CHF_LIBOR_6M);
		assertEquals(fra.IndexInterpolated, null);
		assertEquals(fra.Discounting, FraDiscountingMethod.ISDA);
		// check same when using a specific selector instead of FpmlPartySelector.any()
		IList<Trade> trades2 = FpmlDocumentParser.of(allParties => ImmutableList.of()).parseTrades(resource);
		assertEquals(trades2, trades);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void fra_interpolated()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/ird-ex08-fra-interpolated.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party1")).parseTrades(resource);
		assertFra(trades, true);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void fra_namespace()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/ird-ex08-fra-namespace.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party2")).parseTrades(resource);
		assertFra(trades, false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void fra_wrapper1()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/ird-ex08-fra-wrapper1.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party2")).parseTrades(resource);
		assertFra(trades, false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void fra_wrapper2()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/ird-ex08-fra-wrapper2.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party2")).parseTrades(resource);
		assertFra(trades, false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void fra_wrapper_clearingStatus()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/ird-ex08-fra-wrapper-clearing-status.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party2")).parseTrades(resource);
		assertFra(trades, false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void vanillaSwap()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/ird-ex01-vanilla-swap.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party1")).parseTrades(resource);
		assertEquals(trades.Count, 1);
		Trade trade = trades[0];
		assertEquals(trade.GetType(), typeof(SwapTrade));
		SwapTrade swapTrade = (SwapTrade) trade;
		assertEquals(swapTrade.Info.TradeDate, date(1994, 12, 12));
		Swap swap = swapTrade.Product;
		NotionalSchedule notional = NotionalSchedule.of(EUR, 50000000d);
		RateCalculationSwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.PAY).accrualSchedule(PeriodicSchedule.builder().startDate(date(1994, 12, 14)).endDate(date(1999, 12, 14)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, FRPA)).frequency(Frequency.P6M).rollConvention(RollConvention.ofDayOfMonth(14)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, FRPA))).build()).notionalSchedule(notional).calculation(IborRateCalculation.builder().index(EUR_LIBOR_6M).dayCount(ACT_360).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, GBLO)).build()).build();
		RateCalculationSwapLeg recLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(date(1994, 12, 14)).endDate(date(1999, 12, 14)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, FRPA)).frequency(Frequency.P12M).rollConvention(RollConvention.ofDayOfMonth(14)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P12M).paymentDateOffset(DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, FRPA))).build()).notionalSchedule(notional).calculation(FixedRateCalculation.builder().dayCount(THIRTY_E_360).rate(ValueSchedule.of(0.06)).build()).build();
		assertEqualsBean((Bean) swap.Legs.get(0), payLeg);
		assertEqualsBean((Bean) swap.Legs.get(1), recLeg);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void stubAmortizedSwap()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/ird-ex02-stub-amort-swap.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party1")).parseTrades(resource);
		assertEquals(trades.Count, 1);
		Trade trade = trades[0];
		assertEquals(trade.GetType(), typeof(SwapTrade));
		SwapTrade swapTrade = (SwapTrade) trade;
		assertEquals(swapTrade.Info.TradeDate, date(1994, 12, 12));
		Swap swap = swapTrade.Product;

		NotionalSchedule notional = NotionalSchedule.builder().currency(EUR).amount(ValueSchedule.builder().initialValue(50000000d).steps(ValueStep.of(date(1995, 12, 14), ValueAdjustment.ofReplace(40000000d)), ValueStep.of(date(1996, 12, 14), ValueAdjustment.ofReplace(30000000d)), ValueStep.of(date(1997, 12, 14), ValueAdjustment.ofReplace(20000000d)), ValueStep.of(date(1998, 12, 14), ValueAdjustment.ofReplace(10000000d))).build()).build();
		RateCalculationSwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(date(1995, 1, 16)).endDate(date(1999, 12, 14)).firstRegularStartDate(date(1995, 6, 14)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA)).frequency(Frequency.P6M).rollConvention(RollConvention.ofDayOfMonth(14)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA))).firstRegularStartDate(date(1995, 6, 14)).build()).notionalSchedule(notional).calculation(IborRateCalculation.builder().index(EUR_LIBOR_6M).dayCount(ACT_360).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, GBLO)).initialStub(IborRateStubCalculation.ofIborInterpolatedRate(EUR_LIBOR_3M, EUR_LIBOR_6M)).build()).build();
		RateCalculationSwapLeg recLeg = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(date(1995, 1, 16)).endDate(date(1999, 12, 14)).firstRegularStartDate(date(1995, 12, 14)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA)).frequency(Frequency.P12M).rollConvention(RollConvention.ofDayOfMonth(14)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P12M).paymentDateOffset(DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA))).firstRegularStartDate(date(1995, 12, 14)).build()).notionalSchedule(notional).calculation(FixedRateCalculation.builder().dayCount(THIRTY_E_360).rate(ValueSchedule.of(0.06)).build()).build();
		assertEqualsBean((Bean) swap.Legs.get(0), payLeg);
		assertEqualsBean((Bean) swap.Legs.get(1), recLeg);
	  }

	  public virtual void stubAmortizedSwap_cashflows()
	  {
		// cashflows from ird-ex02-stub-amort-swap.xml with Sat/Sun holidays only
		NotionalSchedule notional = NotionalSchedule.builder().currency(EUR).amount(ValueSchedule.builder().initialValue(50000000d).steps(ValueStep.of(date(1995, 12, 14), ValueAdjustment.ofReplace(40000000d)), ValueStep.of(date(1996, 12, 14), ValueAdjustment.ofReplace(30000000d)), ValueStep.of(date(1997, 12, 14), ValueAdjustment.ofReplace(20000000d)), ValueStep.of(date(1998, 12, 14), ValueAdjustment.ofReplace(10000000d))).build()).build();
		RateCalculationSwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(date(1995, 1, 16)).endDate(date(1999, 12, 14)).firstRegularStartDate(date(1995, 6, 14)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, SAT_SUN)).frequency(Frequency.P6M).rollConvention(RollConvention.ofDayOfMonth(14)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, SAT_SUN))).build()).notionalSchedule(notional).calculation(IborRateCalculation.builder().index(EUR_LIBOR_6M).dayCount(ACT_360).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, SAT_SUN)).initialStub(IborRateStubCalculation.ofIborInterpolatedRate(EUR_LIBOR_3M, EUR_LIBOR_6M)).build()).build();
		RateCalculationSwapLeg recLeg = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(date(1995, 1, 16)).endDate(date(1999, 12, 14)).firstRegularStartDate(date(1995, 12, 14)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, SAT_SUN)).frequency(Frequency.P12M).rollConvention(RollConvention.ofDayOfMonth(14)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P12M).paymentDateOffset(DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, SAT_SUN))).build()).notionalSchedule(notional).calculation(FixedRateCalculation.builder().dayCount(THIRTY_E_360).rate(ValueSchedule.of(0.06)).build()).build();
		ImmutableReferenceData refData = ImmutableReferenceData.of(ImmutableMap.of(HolidayCalendarIds.GBLO, HolidayCalendars.SAT_SUN, HolidayCalendarIds.EUTA, HolidayCalendars.SAT_SUN, HolidayCalendarIds.SAT_SUN, HolidayCalendars.SAT_SUN, HolidayCalendarIds.NO_HOLIDAYS, HolidayCalendars.NO_HOLIDAYS));
		ResolvedSwapLeg expandedPayLeg = payLeg.resolve(refData);
		assertEquals(expandedPayLeg.PaymentPeriods.size(), 10);
		assertIborPaymentPeriod(expandedPayLeg, 0, "1995-06-14", "1995-01-16", "1995-06-14", 50000000d, "1995-01-12");
		assertIborPaymentPeriod(expandedPayLeg, 1, "1995-12-14", "1995-06-14", "1995-12-14", 50000000d, "1995-06-12");
		assertIborPaymentPeriod(expandedPayLeg, 2, "1996-06-14", "1995-12-14", "1996-06-14", 40000000d, "1995-12-12");
		assertIborPaymentPeriod(expandedPayLeg, 3, "1996-12-16", "1996-06-14", "1996-12-16", 40000000d, "1996-06-12");
		assertIborPaymentPeriod(expandedPayLeg, 4, "1997-06-16", "1996-12-16", "1997-06-16", 30000000d, "1996-12-12");
		assertIborPaymentPeriod(expandedPayLeg, 5, "1997-12-15", "1997-06-16", "1997-12-15", 30000000d, "1997-06-12");
		assertIborPaymentPeriod(expandedPayLeg, 6, "1998-06-15", "1997-12-15", "1998-06-15", 20000000d, "1997-12-11");
		assertIborPaymentPeriod(expandedPayLeg, 7, "1998-12-14", "1998-06-15", "1998-12-14", 20000000d, "1998-06-11");
		assertIborPaymentPeriod(expandedPayLeg, 8, "1999-06-14", "1998-12-14", "1999-06-14", 10000000d, "1998-12-10");
		assertIborPaymentPeriod(expandedPayLeg, 9, "1999-12-14", "1999-06-14", "1999-12-14", 10000000d, "1999-06-10");
		ResolvedSwapLeg expandedRecLeg = recLeg.resolve(refData);
		assertEquals(expandedRecLeg.PaymentPeriods.size(), 5);
		assertFixedPaymentPeriod(expandedRecLeg, 0, "1995-12-14", "1995-01-16", "1995-12-14", 50000000d, 0.06d);
		assertFixedPaymentPeriod(expandedRecLeg, 1, "1996-12-16", "1995-12-14", "1996-12-16", 40000000d, 0.06d);
		assertFixedPaymentPeriod(expandedRecLeg, 2, "1997-12-15", "1996-12-16", "1997-12-15", 30000000d, 0.06d);
		assertFixedPaymentPeriod(expandedRecLeg, 3, "1998-12-14", "1997-12-15", "1998-12-14", 20000000d, 0.06d);
		assertFixedPaymentPeriod(expandedRecLeg, 4, "1999-12-14", "1998-12-14", "1999-12-14", 10000000d, 0.06d);
	  }

	  public virtual void stubAmortizedSwap2()
	  {
		// example where notionalStepParameters are used instead of explicit steps
		// fixed and float legs express notionalStepParameters in two different ways, but they resolve to same object model
		string location = "classpath:com/opengamma/strata/loader/fpml/ird-ex02-stub-amort-swap2.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party1")).parseTrades(resource);
		assertEquals(trades.Count, 1);
		Trade trade = trades[0];
		assertEquals(trade.GetType(), typeof(SwapTrade));
		SwapTrade swapTrade = (SwapTrade) trade;
		assertEquals(swapTrade.Info.TradeDate, date(1994, 12, 12));
		Swap swap = swapTrade.Product;

		NotionalSchedule notionalFloat = NotionalSchedule.builder().currency(EUR).amount(ValueSchedule.builder().initialValue(50000000d).stepSequence(ValueStepSequence.of(date(1995, 12, 14), date(1998, 12, 14), Frequency.P12M, ValueAdjustment.ofDeltaAmount(-10000000d))).build()).build();
		RateCalculationSwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(date(1995, 1, 16)).endDate(date(1999, 12, 14)).firstRegularStartDate(date(1995, 6, 14)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA)).frequency(Frequency.P6M).rollConvention(RollConvention.ofDayOfMonth(14)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA))).firstRegularStartDate(date(1995, 6, 14)).build()).notionalSchedule(notionalFloat).calculation(IborRateCalculation.builder().index(EUR_LIBOR_6M).dayCount(ACT_360).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, GBLO)).initialStub(IborRateStubCalculation.ofIborInterpolatedRate(EUR_LIBOR_3M, EUR_LIBOR_6M)).build()).build();
		RateCalculationSwapLeg recLeg = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(date(1995, 1, 16)).endDate(date(1999, 12, 14)).firstRegularStartDate(date(1995, 12, 14)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA)).frequency(Frequency.P12M).rollConvention(RollConvention.ofDayOfMonth(14)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P12M).paymentDateOffset(DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA))).firstRegularStartDate(date(1995, 12, 14)).build()).notionalSchedule(notionalFloat).calculation(FixedRateCalculation.builder().dayCount(THIRTY_E_360).rate(ValueSchedule.of(0.06)).build()).build();
		assertEqualsBean((Bean) swap.Legs.get(0), payLeg);
		assertEqualsBean((Bean) swap.Legs.get(1), recLeg);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void compoundSwap()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/ird-ex03-compound-swap.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party1")).parseTrades(resource);
		assertEquals(trades.Count, 1);
		Trade trade = trades[0];
		assertEquals(trade.GetType(), typeof(SwapTrade));
		SwapTrade swapTrade = (SwapTrade) trade;
		assertEquals(swapTrade.Info.TradeDate, date(2000, 4, 25));
		Swap swap = swapTrade.Product;

		NotionalSchedule notional = NotionalSchedule.of(USD, 100000000d);
		RateCalculationSwapLeg recLeg = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(date(2000, 4, 27)).endDate(date(2002, 4, 27)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO_USNY)).frequency(Frequency.P3M).rollConvention(RollConvention.ofDayOfMonth(27)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.ofBusinessDays(5, GBLO_USNY)).compoundingMethod(CompoundingMethod.FLAT).build()).notionalSchedule(notional).calculation(IborRateCalculation.builder().index(USD_LIBOR_3M).dayCount(ACT_360).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, GBLO)).build()).build();
		RateCalculationSwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(date(2000, 4, 27)).endDate(date(2002, 4, 27)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO_USNY)).frequency(Frequency.P6M).rollConvention(RollConvention.ofDayOfMonth(27)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.ofBusinessDays(5, GBLO_USNY)).build()).notionalSchedule(notional).calculation(FixedRateCalculation.builder().dayCount(THIRTY_360_ISDA).rate(ValueSchedule.of(0.0585)).build()).build();
		assertEqualsBean((Bean) swap.Legs.get(0), recLeg);
		assertEqualsBean((Bean) swap.Legs.get(1), payLeg);
	  }

	  public virtual void compoundSwap_cashFlows()
	  {
		// cashflows from ird-ex02-stub-amort-swap.xml with Sat/Sun holidays only
		NotionalSchedule notional = NotionalSchedule.of(USD, 100000000d);
		RateCalculationSwapLeg recLeg = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(date(2000, 4, 27)).endDate(date(2002, 4, 27)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, SAT_SUN)).frequency(Frequency.P3M).rollConvention(RollConvention.ofDayOfMonth(27)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.ofBusinessDays(5, SAT_SUN)).compoundingMethod(CompoundingMethod.FLAT).build()).notionalSchedule(notional).calculation(IborRateCalculation.builder().index(USD_LIBOR_3M).dayCount(ACT_360).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, GBLO)).build()).build();
		RateCalculationSwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(date(2000, 4, 27)).endDate(date(2002, 4, 27)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, SAT_SUN)).frequency(Frequency.P6M).rollConvention(RollConvention.ofDayOfMonth(27)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.ofBusinessDays(5, SAT_SUN)).build()).notionalSchedule(notional).calculation(FixedRateCalculation.builder().dayCount(THIRTY_360_ISDA).rate(ValueSchedule.of(0.0585)).build()).build();
		ImmutableReferenceData refData = ImmutableReferenceData.of(ImmutableMap.of(HolidayCalendarIds.GBLO, HolidayCalendars.SAT_SUN, HolidayCalendarIds.EUTA, HolidayCalendars.SAT_SUN, HolidayCalendarIds.USNY, HolidayCalendars.SAT_SUN, HolidayCalendarIds.SAT_SUN, HolidayCalendars.SAT_SUN, HolidayCalendarIds.NO_HOLIDAYS, HolidayCalendars.NO_HOLIDAYS));
		ResolvedSwapLeg expandedRecLeg = recLeg.resolve(refData);
		assertEquals(expandedRecLeg.PaymentPeriods.size(), 4);
		assertIborPaymentPeriodCpd(expandedRecLeg, 0, 0, "2000-11-03", "2000-04-27", "2000-07-27", 100000000d, "2000-04-25");
		assertIborPaymentPeriodCpd(expandedRecLeg, 0, 1, "2000-11-03", "2000-07-27", "2000-10-27", 100000000d, "2000-07-25");
		assertIborPaymentPeriodCpd(expandedRecLeg, 1, 0, "2001-05-04", "2000-10-27", "2001-01-29", 100000000d, "2000-10-25");
		assertIborPaymentPeriodCpd(expandedRecLeg, 1, 1, "2001-05-04", "2001-01-29", "2001-04-27", 100000000d, "2001-01-25");
		assertIborPaymentPeriodCpd(expandedRecLeg, 2, 0, "2001-11-05", "2001-04-27", "2001-07-27", 100000000d, "2001-04-25");
		assertIborPaymentPeriodCpd(expandedRecLeg, 2, 1, "2001-11-05", "2001-07-27", "2001-10-29", 100000000d, "2001-07-25");
		// final cashflow dates do not match with GBLO, USNY, GBLO+USNY or SAT_SUN
	//    assertIborPaymentPeriodCpd(expandedRecLeg, 3, 0, "2002-05-06", "2001-10-29", "2002-01-29", 100000000d, "2001-10-25");
	//    assertIborPaymentPeriodCpd(expandedRecLeg, 3, 1, "2002-05-06", "2002-01-29", "2002-04-29", 100000000d, "2002-01-25");
		ResolvedSwapLeg expandedPayLeg = payLeg.resolve(refData);
		assertEquals(expandedPayLeg.PaymentPeriods.size(), 4);
		assertFixedPaymentPeriod(expandedPayLeg, 0, "2000-11-03", "2000-04-27", "2000-10-27", 100000000d, 0.0585d);
		assertFixedPaymentPeriod(expandedPayLeg, 1, "2001-05-04", "2000-10-27", "2001-04-27", 100000000d, 0.0585d);
		assertFixedPaymentPeriod(expandedPayLeg, 2, "2001-11-05", "2001-04-27", "2001-10-29", 100000000d, 0.0585d);
		assertFixedPaymentPeriod(expandedPayLeg, 3, "2002-05-06", "2001-10-29", "2002-04-29", 100000000d, 0.0585d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void dualSwap()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/ird-ex05-long-stub-swap.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party1")).parseTrades(resource);
		assertEquals(trades.Count, 1);
		Trade trade = trades[0];
		assertEquals(trade.GetType(), typeof(SwapTrade));
		SwapTrade swapTrade = (SwapTrade) trade;
		assertEquals(swapTrade.Info.TradeDate, date(2000, 4, 3));
		Swap swap = swapTrade.Product;

		NotionalSchedule notional = NotionalSchedule.of(EUR, 75000000d);
		RateCalculationSwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(date(2000, 4, 5)).firstRegularStartDate(date(2000, 10, 5)).lastRegularEndDate(date(2004, 10, 5)).endDate(date(2005, 1, 5)).overrideStartDate(AdjustableDate.of(date(2000, 3, 5), BusinessDayAdjustment.NONE)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.of(FOLLOWING, EUTA)).frequency(Frequency.P6M).rollConvention(RollConvention.ofDayOfMonth(5)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(FOLLOWING, EUTA))).firstRegularStartDate(date(2000, 10, 5)).build()).notionalSchedule(notional).calculation(IborRateCalculation.builder().dayCount(ACT_360).index(EUR_EURIBOR_6M).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, EUTA)).spread(ValueSchedule.of(0.001)).initialStub(IborRateStubCalculation.ofFixedRate(0.05125)).finalStub(IborRateStubCalculation.ofIborRate(EUR_EURIBOR_3M)).build()).build();
		RateCalculationSwapLeg recLeg = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(date(2000, 4, 5)).firstRegularStartDate(date(2000, 10, 5)).lastRegularEndDate(date(2004, 10, 5)).endDate(date(2005, 1, 5)).overrideStartDate(AdjustableDate.of(date(2000, 3, 5), BusinessDayAdjustment.NONE)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.of(FOLLOWING, EUTA)).frequency(Frequency.P12M).rollConvention(RollConvention.ofDayOfMonth(5)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P12M).paymentDateOffset(DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(FOLLOWING, EUTA))).firstRegularStartDate(date(2000, 10, 5)).build()).notionalSchedule(notional).calculation(FixedRateCalculation.builder().dayCount(THIRTY_360_ISDA).rate(ValueSchedule.of(0.0525)).build()).build();
		assertEqualsBean((Bean) swap.Legs.get(0), payLeg);
		assertEqualsBean((Bean) swap.Legs.get(1), recLeg);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void oisSwap()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/ird-ex07-ois-swap.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party1")).parseTrades(resource);
		assertEquals(trades.Count, 1);
		Trade trade = trades[0];
		assertEquals(trade.GetType(), typeof(SwapTrade));
		SwapTrade swapTrade = (SwapTrade) trade;
		assertEquals(swapTrade.Info.TradeDate, date(2001, 1, 25));
		Swap swap = swapTrade.Product;

		NotionalSchedule notional = NotionalSchedule.of(EUR, 100000000d);
		RateCalculationSwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(date(2001, 1, 29)).endDate(date(2001, 4, 29)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA)).frequency(Frequency.TERM).rollConvention(RollConventions.NONE).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.TERM).paymentDateOffset(DaysAdjustment.ofBusinessDays(1, EUTA)).build()).notionalSchedule(notional).calculation(OvernightRateCalculation.builder().dayCount(ACT_360).index(EUR_EONIA).build()).build();
		RateCalculationSwapLeg recLeg = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(date(2001, 1, 29)).endDate(date(2001, 4, 29)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA)).frequency(Frequency.TERM).rollConvention(RollConventions.NONE).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.TERM).paymentDateOffset(DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA))).build()).notionalSchedule(notional).calculation(FixedRateCalculation.builder().dayCount(ACT_360).rate(ValueSchedule.of(0.051)).build()).build();
		assertEqualsBean((Bean) swap.Legs.get(0), payLeg);
		assertEqualsBean((Bean) swap.Legs.get(1), recLeg);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void compoundAverageSwap()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/ird-ex30-swap-comp-avg-relative-date.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party1")).parseTrades(resource);
		assertEquals(trades.Count, 1);
		Trade trade = trades[0];
		assertEquals(trade.GetType(), typeof(SwapTrade));
		SwapTrade swapTrade = (SwapTrade) trade;
		assertEquals(swapTrade.Info.TradeDate, date(2005, 7, 31));
		Swap swap = swapTrade.Product;

		NotionalSchedule notional = NotionalSchedule.of(USD, 100000000d);
		RateCalculationSwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(date(2005, 8, 2)).endDate(date(2007, 8, 2)).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO_USNY)).frequency(Frequency.P6M).rollConvention(RollConventions.NONE).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO_USNY))).build()).notionalSchedule(notional).calculation(FixedRateCalculation.builder().dayCount(THIRTY_360_ISDA).rate(ValueSchedule.of(0.0003)).build()).build();
		RateCalculationSwapLeg recLeg = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(date(2005, 8, 2)).endDate(date(2007, 8, 2)).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO_USNY)).frequency(Frequency.P3M).rollConvention(RollConventions.NONE).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO_USNY))).compoundingMethod(CompoundingMethod.STRAIGHT).build()).notionalSchedule(notional).calculation(IborRateCalculation.builder().index(USD_LIBOR_6M).resetPeriods(ResetSchedule.builder().resetFrequency(Frequency.P1M).resetMethod(IborRateResetMethod.UNWEIGHTED).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO)).build()).dayCount(ACT_360).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, GBLO)).build()).build();
		assertEqualsBean((Bean) swap.Legs.get(0), payLeg);
		assertEqualsBean((Bean) swap.Legs.get(1), recLeg);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void zeroCouponSwap()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/ird-ex32-zero-coupon-swap.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party1")).parseTrades(resource);
		assertEquals(trades.Count, 1);
		Trade trade = trades[0];
		assertEquals(trade.GetType(), typeof(SwapTrade));
		SwapTrade swapTrade = (SwapTrade) trade;
		assertEquals(swapTrade.Info.TradeDate, date(2005, 2, 20));
		Swap swap = swapTrade.Product;

		NotionalSchedule notional = NotionalSchedule.of(GBP, 100000d);
		RateCalculationSwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(date(2005, 2, 22)).endDate(date(2035, 2, 22)).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO)).frequency(Frequency.P12M).rollConvention(RollConventions.NONE).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.TERM).paymentDateOffset(DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO))).build()).notionalSchedule(notional).calculation(FixedRateCalculation.builder().dayCount(THIRTY_E_360).rate(ValueSchedule.of(0.03)).build()).build();
		RateCalculationSwapLeg recLeg = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(date(2005, 2, 22)).endDate(date(2035, 2, 22)).startDateBusinessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO)).businessDayAdjustment(BusinessDayAdjustment.NONE).endDateBusinessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO)).frequency(Frequency.P3M).rollConvention(RollConvention.ofDayOfMonth(22)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.TERM).paymentDateOffset(DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO))).compoundingMethod(CompoundingMethod.FLAT).build()).notionalSchedule(notional).calculation(IborRateCalculation.builder().index(GBP_LIBOR_6M).dayCount(ACT_360).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, GBLO)).build()).build();
		assertEqualsBean((Bean) swap.Legs.get(0), payLeg);
		assertEqualsBean((Bean) swap.Legs.get(1), recLeg);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void inverseFloaterSwap()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/ird-ex35-inverse-floater-inverse-vs-floating.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party1")).parseTrades(resource);
		assertEquals(trades.Count, 1);
		Trade trade = trades[0];
		assertEquals(trade.GetType(), typeof(SwapTrade));
		SwapTrade swapTrade = (SwapTrade) trade;
		assertEquals(swapTrade.Info.TradeDate, date(2009, 4, 29));
		Swap swap = swapTrade.Product;

		NotionalSchedule notional = NotionalSchedule.of(USD, 100000000d);
		RateCalculationSwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(date(2009, 8, 30)).endDate(date(2011, 8, 30)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, USNY)).frequency(Frequency.P3M).rollConvention(RollConvention.ofDayOfMonth(30)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P3M).paymentDateOffset(DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, USNY))).build()).notionalSchedule(notional).calculation(IborRateCalculation.builder().index(USD_LIBOR_3M).dayCount(ACT_360).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, GBLO)).gearing(ValueSchedule.of(-1)).spread(ValueSchedule.of(0.0325)).build()).build();
		RateCalculationSwapLeg recLeg = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(date(2009, 8, 30)).endDate(date(2011, 8, 30)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, USNY)).frequency(Frequency.P6M).rollConvention(RollConvention.ofDayOfMonth(30)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, USNY))).build()).notionalSchedule(notional).calculation(IborRateCalculation.builder().index(USD_LIBOR_6M).dayCount(THIRTY_360_ISDA).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, USNY)).build()).build();
		assertEqualsBean((Bean) swap.Legs.get(0), payLeg);
		assertEqualsBean((Bean) swap.Legs.get(1), recLeg);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void iborFloatingLegNoResetDates()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/ird-ibor-no-reset-dates.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party1")).parseTrades(resource);
		assertEquals(trades.Count, 1);
		Trade trade = trades[0];
		assertEquals(trade.GetType(), typeof(SwapTrade));
		SwapTrade swapTrade = (SwapTrade) trade;
		Swap swap = swapTrade.Product;

		IList<SwapLeg> floatLegs = swap.getLegs(SwapLegType.IBOR);
		assertEquals(floatLegs.Count, 1);
		SwapLeg floatLeg = Iterables.getOnlyElement(floatLegs);

		RateCalculationSwapLeg expectedFloatLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.PAY).accrualSchedule(PeriodicSchedule.builder().startDate(date(2018, 9, 28)).endDate(date(2019, 9, 29)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, AUSY)).frequency(Frequency.P3M).rollConvention(RollConvention.ofDayOfMonth(29)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P3M).paymentDateOffset(DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, AUSY))).build()).notionalSchedule(NotionalSchedule.of(AUD, 500000000)).calculation(IborRateCalculation.builder().index(AUD_BBSW_3M).dayCount(ACT_365F).fixingDateOffset(DaysAdjustment.ofBusinessDays(-1, AUSY)).build()).build();
		assertEqualsBean((Bean) floatLeg, expectedFloatLeg);
	  }

	  public virtual void oisFloatingLegNoResetDates()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/ird-ois-no-reset-dates.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party1")).parseTrades(resource);
		assertEquals(trades.Count, 1);
		Trade trade = trades[0];
		assertEquals(trade.GetType(), typeof(SwapTrade));
		SwapTrade swapTrade = (SwapTrade) trade;
		Swap swap = swapTrade.Product;

		IList<SwapLeg> oisLegs = swap.getLegs(SwapLegType.OVERNIGHT);
		assertEquals(oisLegs.Count, 1);
		SwapLeg oisLeg = Iterables.getOnlyElement(oisLegs);

		RateCalculationSwapLeg expectedOisLeg = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(date(2001, 1, 29)).endDate(date(2001, 4, 29)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA)).frequency(Frequency.TERM).rollConvention(RollConventions.NONE).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.TERM).paymentDateOffset(DaysAdjustment.ofBusinessDays(1, EUTA)).build()).notionalSchedule(NotionalSchedule.of(EUR, 100000000)).calculation(OvernightRateCalculation.builder().dayCount(ACT_360).index(EUR_EONIA).build()).build();
		assertEqualsBean((Bean) oisLeg, expectedOisLeg);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void inflationSwap()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/inflation-swap-ex01-yoy.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party2")).parseTrades(resource);
		assertEquals(trades.Count, 1);
		Trade trade = trades[0];
		assertEquals(trade.GetType(), typeof(SwapTrade));
		SwapTrade swapTrade = (SwapTrade) trade;
		assertEquals(swapTrade.Info.TradeDate, date(2003, 11, 15));
		Swap swap = swapTrade.Product;

		NotionalSchedule notional = NotionalSchedule.of(EUR, 1d);
		RateCalculationSwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(date(2003, 11, 20)).endDate(date(2007, 11, 20)).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA)).overrideStartDate(AdjustableDate.of(date(2003, 11, 12), BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA))).frequency(Frequency.P12M).rollConvention(RollConventions.DAY_20).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P12M).paymentDateOffset(DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA))).firstRegularStartDate(date(2004, 11, 20)).build()).notionalSchedule(notional).calculation(FixedRateCalculation.builder().dayCount(THIRTY_360_ISDA).rate(ValueSchedule.of(0.01)).build()).build();
		RateCalculationSwapLeg recLeg = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(date(2003, 11, 20)).endDate(date(2007, 11, 20)).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA)).overrideStartDate(AdjustableDate.of(date(2003, 11, 12), BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA))).frequency(Frequency.P3M).rollConvention(RollConventions.DAY_20).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P12M).paymentDateOffset(DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA))).compoundingMethod(CompoundingMethod.NONE).firstRegularStartDate(date(2004, 11, 20)).build()).notionalSchedule(notional).calculation(InflationRateCalculation.builder().index(PriceIndices.US_CPI_U).lag(Period.ofMonths(3)).indexCalculationMethod(PriceIndexCalculationMethod.INTERPOLATED).build()).build();
		assertEqualsBean((Bean) swap.Legs.get(0), payLeg);
		assertEqualsBean((Bean) swap.Legs.get(1), recLeg);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void cds01()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/cd-ex01-long-asia-corp-fixreg.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party2")).parseTrades(resource);
		assertEquals(trades.Count, 1);
		CdsTrade cdsTrade = (CdsTrade) trades[0];
		assertEquals(cdsTrade.Info.TradeDate, date(2002, 12, 4));

		Cds expected = Cds.builder().buySell(BUY).legalEntityId(StandardId.of("http://www.fpml.org/spec/2003/entity-id-RED-1-0", "004CC9")).currency(JPY).notional(500000000d).paymentSchedule(PeriodicSchedule.builder().startDate(date(2002, 12, 5)).endDate(date(2007, 12, 5)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).endDateBusinessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO_USNY_JPTO)).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO_USNY_JPTO)).firstRegularStartDate(date(2003, 3, 5)).frequency(Frequency.P3M).rollConvention(RollConventions.DAY_5).build()).fixedRate(0.007).dayCount(ACT_360).build();
		assertEqualsBean(cdsTrade.Product, expected);
		assertEquals(cdsTrade.UpfrontFee.Present, false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void cds02()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/cd-ex02-2003-short-asia-corp-fixreg.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party2")).parseTrades(resource);
		assertEquals(trades.Count, 1);
		CdsTrade cdsTrade = (CdsTrade) trades[0];
		assertEquals(cdsTrade.Info.TradeDate, date(2002, 12, 4));

		Cds expected = Cds.builder().buySell(SELL).legalEntityId(StandardId.of("http://www.fpml.org/coding-scheme/external/entity-id-RED-1-0", "008FAQ")).currency(JPY).notional(500000000d).paymentSchedule(PeriodicSchedule.builder().startDate(date(2002, 12, 5)).endDate(date(2007, 12, 5)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).endDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.NONE).firstRegularStartDate(date(2003, 3, 5)).frequency(Frequency.P3M).rollConvention(RollConventions.DAY_5).build()).fixedRate(0.007).dayCount(ACT_360).build();
		assertEqualsBean(cdsTrade.Product, expected);
		assertEquals(cdsTrade.UpfrontFee.Present, false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void cdsIndex01()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/cdindex-ex01-cdx.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party2")).parseTrades(resource);
		assertEquals(trades.Count, 1);
		CdsIndexTrade cdsTrade = (CdsIndexTrade) trades[0];
		assertEquals(cdsTrade.Info.TradeDate, date(2005, 1, 24));

		CdsIndex expected = CdsIndex.builder().buySell(BUY).cdsIndexId(StandardId.of("CDX-Name", "Dow Jones CDX NA IG.2")).currency(USD).notional(25000000d).paymentSchedule(PeriodicSchedule.builder().startDate(date(2004, 3, 23)).endDate(date(2009, 3, 20)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).endDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.NONE).frequency(Frequency.P3M).build()).fixedRate(0.0060).dayCount(ACT_360).build();
		assertEqualsBean(cdsTrade.Product, expected);
		assertEquals(cdsTrade.UpfrontFee.get(), AdjustablePayment.of(USD, 16000, AdjustableDate.of(date(2004, 3, 23))));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "parse") public static Object[][] data_parse()
	  public static object[][] data_parse()
	  {
		return new object[][]
		{
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cd-ex01-long-asia-corp-fixreg.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cd-ex02-2003-short-asia-corp-fixreg.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cd-ex02-short-asia-corp-fixreg.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cd-ex03-long-aussie-corp-fixreg.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cd-ex04-short-aussie-corp-fixreg.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cd-ex05-long-emasia-corp-fixreg.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cd-ex06-long-emeur-sov-fixreg.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cd-ex07-2003-long-euro-corp-fixreg.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cd-ex07-long-euro-corp-fixreg.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cd-ex08-2003-short-euro-corp-fixreg.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cd-ex08-short-euro-corp-fixreg.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cd-ex09-long-euro-sov-fixreg.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cd-ex10-2003-long-us-corp-fixreg.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cd-ex10-long-us-corp-fixreg.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cd-ex11-2003-short-us-corp-fixreg.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cd-ex11-short-us-corp-fixreg.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cd-ex12-long-emasia-sov-fixreg.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cd-ex13-long-asia-sov-fixreg.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cd-ex14-long-emlatin-corp-fixreg.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cd-ex15-long-emlatin-sov-fixreg.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cd-ex16-short-us-corp-fixreg-recovery-factor.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cd-ex17-short-us-corp-portfolio-compression.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cd-ex18-standard-north-american-corp.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/cdindex-ex01-cdx.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/fx-ex01-fx-spot.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/fx-ex02-spot-cross-w-side-rates.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/fx-ex03-fx-fwd.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/fx-ex04-fx-fwd-w-settlement.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/fx-ex05-fx-fwd-w-ssi.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/fx-ex06-fx-fwd-w-splits.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/fx-ex07-non-deliverable-forward.xml"},
			new object[] {"classpath:com/opengamma/strata/loader/fpml/fx-ex08-fx-swap.xml"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "parse") public void parse(String location)
	  public virtual void parse(string location)
	  {
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.matching("Party2")).parseTrades(resource);
		assertEquals(trades.Count, 1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void noTrades()
	  {
		XmlElement rootEl = XmlElement.ofChildren("dataDocument", ImmutableList.of());
		IList<Trade> trades = FpmlDocumentParser.of(FpmlPartySelector.any()).parseTrades(rootEl, ImmutableMap.of());
		assertEquals(trades.Count, 0);
	  }

	  public virtual void badTradeDate()
	  {
		XmlElement tradeDateEl = XmlElement.ofContent("tradeDate", "2000/06/30");
		XmlElement tradeHeaderEl = XmlElement.ofChildren("tradeHeader", ImmutableList.of(tradeDateEl));
		XmlElement tradeTypeEl = XmlElement.ofContent("foo", "fakeTradeType");
		XmlElement tradeEl = XmlElement.ofChildren("trade", ImmutableList.of(tradeHeaderEl, tradeTypeEl));
		XmlElement rootEl = XmlElement.ofChildren("dataDocument", ImmutableList.of(tradeEl));
		FpmlParserPlugin tradeParser = new FpmlParserPluginAnonymousInnerClass(this, tradeEl);
		FpmlDocumentParser parser = FpmlDocumentParser.of(FpmlPartySelector.any(), FpmlTradeInfoParserPlugin.standard(), ImmutableMap.of("foo", tradeParser));
		assertThrows(() => parser.parseTrades(rootEl, ImmutableMap.of()), typeof(DateTimeParseException), ".*2000/06/30.*");
	  }

	  private class FpmlParserPluginAnonymousInnerClass : FpmlParserPlugin
	  {
		  private readonly FpmlDocumentParserTest outerInstance;

		  private XmlElement tradeEl;

		  public FpmlParserPluginAnonymousInnerClass(FpmlDocumentParserTest outerInstance, XmlElement tradeEl)
		  {
			  this.outerInstance = outerInstance;
			  this.tradeEl = tradeEl;
		  }

		  public Trade parseTrade(FpmlDocument document, XmlElement tradeEl)
		  {
			document.parseTradeInfo(tradeEl); // expected to throw an exception
			throw new System.NotSupportedException();
		  }

		  public string Name
		  {
			  get
			  {
				return "foo";
			  }
		  }
	  }

	  public virtual void unknownProduct()
	  {
		XmlElement tradeDateEl = XmlElement.ofContent("tradeDate", "2000-06-30");
		XmlElement tradeHeaderEl = XmlElement.ofChildren("tradeHeader", ImmutableList.of(tradeDateEl));
		XmlElement unknownEl = XmlElement.ofChildren("unknown", ImmutableList.of());
		XmlElement tradeEl = XmlElement.ofChildren("trade", ImmutableList.of(tradeHeaderEl, unknownEl));
		XmlElement rootEl = XmlElement.ofChildren("dataDocument", ImmutableList.of(tradeEl));
		FpmlDocumentParser parser = FpmlDocumentParser.of(FpmlPartySelector.any());
		assertThrows(() => parser.parseTrades(rootEl, ImmutableMap.of()), typeof(FpmlParseException), ".*unknown.*");
	  }

	  public virtual void badSelector()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/ird-ex08-fra.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		FpmlDocumentParser parser = FpmlDocumentParser.of(allParties => ImmutableList.of("rubbish"));
		assertThrows(() => parser.parseTrades(resource), typeof(FpmlParseException), "Selector returned an ID .*");
	  }

	  public virtual void notFpml()
	  {
		string location = "classpath:com/opengamma/strata/loader/fpml/not-fpml.xml";
		ByteSource resource = ResourceLocator.of(location).ByteSource;
		FpmlDocumentParser parser = FpmlDocumentParser.of(FpmlPartySelector.any());
		assertThrows(() => parser.parseTrades(resource), typeof(FpmlParseException), "Unable to find FpML root element.*");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void document()
	  {
		XmlElement tradeDateEl = XmlElement.ofContent("tradeDate", "2000-06-30");
		XmlElement tradeHeaderEl = XmlElement.ofChildren("tradeHeader", ImmutableList.of(tradeDateEl));
		XmlElement tradeEl = XmlElement.ofChildren("trade", ImmutableMap.of("href", "foo"), ImmutableList.of(tradeHeaderEl));
		XmlElement rootEl = XmlElement.ofChildren("dataDocument", ImmutableList.of(tradeEl));
		FpmlDocument test = new FpmlDocument(rootEl, ImmutableMap.of(), FpmlPartySelector.any(), FpmlTradeInfoParserPlugin.standard(), REF_DATA);
		assertEquals(test.FpmlRoot, rootEl);
		assertEquals(test.Parties, ImmutableListMultimap.of());
		assertEquals(test.References, ImmutableMap.of());
		assertEquals(test.OurPartyHrefIds, ImmutableList.of());
		assertThrows(() => test.lookupReference(tradeEl), typeof(FpmlParseException), ".*reference not found.*");
		assertThrows(() => test.validateNotPresent(tradeEl, "tradeHeader"), typeof(FpmlParseException), ".*tradeHeader.*");
	  }

	  public virtual void documentFrequency()
	  {
		XmlElement tradeDateEl = XmlElement.ofContent("tradeDate", "2000-06-30");
		XmlElement tradeHeaderEl = XmlElement.ofChildren("tradeHeader", ImmutableList.of(tradeDateEl));
		XmlElement tradeEl = XmlElement.ofChildren("trade", ImmutableMap.of("href", "foo"), ImmutableList.of(tradeHeaderEl));
		XmlElement rootEl = XmlElement.ofChildren("dataDocument", ImmutableList.of(tradeEl));
		FpmlDocument test = new FpmlDocument(rootEl, ImmutableMap.of(), FpmlPartySelector.any(), FpmlTradeInfoParserPlugin.standard(), REF_DATA);
		assertEquals(test.convertFrequency("1", "M"), Frequency.P1M);
		assertEquals(test.convertFrequency("12", "M"), Frequency.P12M);
		assertEquals(test.convertFrequency("1", "Y"), Frequency.P12M);
		assertEquals(test.convertFrequency("13", "Y"), Frequency.of(Period.ofYears(13)));
	  }

	  public virtual void documentTenor()
	  {
		XmlElement tradeDateEl = XmlElement.ofContent("tradeDate", "2000-06-30");
		XmlElement tradeHeaderEl = XmlElement.ofChildren("tradeHeader", ImmutableList.of(tradeDateEl));
		XmlElement tradeEl = XmlElement.ofChildren("trade", ImmutableMap.of("href", "foo"), ImmutableList.of(tradeHeaderEl));
		XmlElement rootEl = XmlElement.ofChildren("dataDocument", ImmutableList.of(tradeEl));
		FpmlDocument test = new FpmlDocument(rootEl, ImmutableMap.of(), FpmlPartySelector.any(), FpmlTradeInfoParserPlugin.standard(), REF_DATA);
		assertEquals(test.convertIndexTenor("1", "M"), Tenor.TENOR_1M);
		assertEquals(test.convertIndexTenor("12", "M"), Tenor.TENOR_12M);
		assertEquals(test.convertIndexTenor("1", "Y"), Tenor.TENOR_12M);
		assertEquals(test.convertIndexTenor("13", "Y"), Tenor.of(Period.ofYears(13)));
	  }

	  //-------------------------------------------------------------------------
	  private void assertIborPaymentPeriod(ResolvedSwapLeg expandedPayLeg, int index, string paymentDateStr, string startDateStr, string endDateStr, double notional, string fixingDateStr)
	  {

		RatePaymentPeriod pp = (RatePaymentPeriod) expandedPayLeg.PaymentPeriods.get(index);
		assertEquals(pp.PaymentDate.ToString(), paymentDateStr);
		assertEquals(Math.Abs(pp.Notional), notional);
		assertEquals(pp.AccrualPeriods.size(), 1);
		RateAccrualPeriod ap = pp.AccrualPeriods.get(0);
		assertEquals(ap.StartDate.ToString(), startDateStr);
		assertEquals(ap.EndDate.ToString(), endDateStr);
		if (ap.RateComputation is IborInterpolatedRateComputation)
		{
		  assertEquals(((IborInterpolatedRateComputation) ap.RateComputation).FixingDate.ToString(), fixingDateStr);
		}
		else if (ap.RateComputation is IborRateComputation)
		{
		  assertEquals(((IborRateComputation) ap.RateComputation).FixingDate.ToString(), fixingDateStr);
		}
		else
		{
		  fail();
		}
	  }

	  private void assertIborPaymentPeriodCpd(ResolvedSwapLeg expandedPayLeg, int paymentIndex, int accrualIndex, string paymentDateStr, string startDateStr, string endDateStr, double notional, string fixingDateStr)
	  {

		RatePaymentPeriod pp = (RatePaymentPeriod) expandedPayLeg.PaymentPeriods.get(paymentIndex);
		assertEquals(pp.PaymentDate.ToString(), paymentDateStr);
		assertEquals(Math.Abs(pp.Notional), notional);
		assertEquals(pp.AccrualPeriods.size(), 2);
		RateAccrualPeriod ap = pp.AccrualPeriods.get(accrualIndex);
		assertEquals(ap.StartDate.ToString(), startDateStr);
		assertEquals(ap.EndDate.ToString(), endDateStr);
		if (ap.RateComputation is IborInterpolatedRateComputation)
		{
		  assertEquals(((IborInterpolatedRateComputation) ap.RateComputation).FixingDate.ToString(), fixingDateStr);
		}
		else if (ap.RateComputation is IborRateComputation)
		{
		  assertEquals(((IborRateComputation) ap.RateComputation).FixingDate.ToString(), fixingDateStr);
		}
		else
		{
		  fail();
		}
	  }

	  private void assertFixedPaymentPeriod(ResolvedSwapLeg expandedPayLeg, int index, string paymentDateStr, string startDateStr, string endDateStr, double notional, double rate)
	  {

		RatePaymentPeriod pp = (RatePaymentPeriod) expandedPayLeg.PaymentPeriods.get(index);
		assertEquals(pp.PaymentDate.ToString(), paymentDateStr);
		assertEquals(Math.Abs(pp.Notional), notional);
		assertEquals(pp.AccrualPeriods.size(), 1);
		RateAccrualPeriod ap = pp.AccrualPeriods.get(0);
		assertEquals(ap.StartDate.ToString(), startDateStr);
		assertEquals(ap.EndDate.ToString(), endDateStr);
		if (ap.RateComputation is FixedRateComputation)
		{
		  assertEquals(((FixedRateComputation) ap.RateComputation).Rate, rate);
		}
		else
		{
		  fail();
		}
	  }

	}

}