using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.regression
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.measure.StandardComponents.marketDataFactory;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using MoreExecutors = com.google.common.util.concurrent.MoreExecutors;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using Column = com.opengamma.strata.calc.Column;
	using Results = com.opengamma.strata.calc.Results;
	using MarketDataConfig = com.opengamma.strata.calc.marketdata.MarketDataConfig;
	using MarketDataRequirements = com.opengamma.strata.calc.marketdata.MarketDataRequirements;
	using CalculationTaskRunner = com.opengamma.strata.calc.runner.CalculationTaskRunner;
	using CalculationTasks = com.opengamma.strata.calc.runner.CalculationTasks;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ExampleData = com.opengamma.strata.examples.marketdata.ExampleData;
	using ExampleMarketData = com.opengamma.strata.examples.marketdata.ExampleMarketData;
	using ExampleMarketDataBuilder = com.opengamma.strata.examples.marketdata.ExampleMarketDataBuilder;
	using Measures = com.opengamma.strata.measure.Measures;
	using StandardComponents = com.opengamma.strata.measure.StandardComponents;
	using Trade = com.opengamma.strata.product.Trade;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using FixedRateCalculation = com.opengamma.strata.product.swap.FixedRateCalculation;
	using IborRateCalculation = com.opengamma.strata.product.swap.IborRateCalculation;
	using NotionalSchedule = com.opengamma.strata.product.swap.NotionalSchedule;
	using PaymentSchedule = com.opengamma.strata.product.swap.PaymentSchedule;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;
	using ReportCalculationResults = com.opengamma.strata.report.ReportCalculationResults;
	using TradeReport = com.opengamma.strata.report.trade.TradeReport;
	using TradeReportTemplate = com.opengamma.strata.report.trade.TradeReportTemplate;

	/// <summary>
	/// Regression test for an example swap report.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SwapReportRegressionTest
	public class SwapReportRegressionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  /// <summary>
	  /// Tests the full set of results against a golden copy.
	  /// </summary>
	  public virtual void testResults()
	  {
		IList<Trade> trades = ImmutableList.of(createTrade1());

		IList<Column> columns = ImmutableList.of(Column.of(Measures.LEG_INITIAL_NOTIONAL), Column.of(Measures.PRESENT_VALUE), Column.of(Measures.LEG_PRESENT_VALUE), Column.of(Measures.PV01_CALIBRATED_SUM), Column.of(Measures.ACCRUED_INTEREST));

		ExampleMarketDataBuilder marketDataBuilder = ExampleMarketData.builder();

		LocalDate valuationDate = LocalDate.of(2009, 7, 31);
		CalculationRules rules = CalculationRules.of(StandardComponents.calculationFunctions(), Currency.USD, marketDataBuilder.ratesLookup(valuationDate));

		MarketData marketData = marketDataBuilder.buildSnapshot(valuationDate);

		// using the direct executor means there is no need to close/shutdown the runner
		CalculationTasks tasks = CalculationTasks.of(rules, trades, columns, REF_DATA);
		MarketDataRequirements reqs = tasks.requirements(REF_DATA);
		MarketData calibratedMarketData = marketDataFactory().create(reqs, MarketDataConfig.empty(), marketData, REF_DATA);
		CalculationTaskRunner runner = CalculationTaskRunner.of(MoreExecutors.newDirectExecutorService());
		Results results = runner.calculate(tasks, calibratedMarketData, REF_DATA);

		ReportCalculationResults calculationResults = ReportCalculationResults.of(valuationDate, trades, columns, results);

		TradeReportTemplate reportTemplate = ExampleData.loadTradeReportTemplate("swap-report-regression-test-template");
		TradeReport tradeReport = TradeReport.of(calculationResults, reportTemplate);

		string expectedResults = ExampleData.loadExpectedResults("swap-report");

		TradeReportRegressionTestUtils.assertAsciiTableEquals(tradeReport.toAsciiTableString(), expectedResults);
	  }

	  private static Trade createTrade1()
	  {
		NotionalSchedule notional = NotionalSchedule.of(Currency.USD, 12_000_000);

		PeriodicSchedule accrual = PeriodicSchedule.builder().startDate(LocalDate.of(2006, 2, 24)).endDate(LocalDate.of(2011, 2, 24)).frequency(Frequency.P3M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).build();

		PaymentSchedule payment = PaymentSchedule.builder().paymentFrequency(Frequency.P3M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, HolidayCalendarIds.USNY)).build();

		SwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.PAY).accrualSchedule(accrual).paymentSchedule(payment).notionalSchedule(notional).calculation(FixedRateCalculation.of(0.05004, DayCounts.ACT_360)).build();

		SwapLeg receiveLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.RECEIVE).accrualSchedule(accrual).paymentSchedule(payment).notionalSchedule(notional).calculation(IborRateCalculation.of(IborIndices.USD_LIBOR_3M)).build();

		return SwapTrade.builder().product(Swap.builder().legs(payLeg, receiveLeg).build()).info(TradeInfo.builder().id(StandardId.of("mn", "14248")).counterparty(StandardId.of("mn", "Dealer A")).settlementDate(LocalDate.of(2006, 2, 24)).build()).build();
	  }

	}

}