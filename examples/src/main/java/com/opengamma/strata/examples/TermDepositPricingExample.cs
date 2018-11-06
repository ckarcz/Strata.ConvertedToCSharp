using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using CalculationRunner = com.opengamma.strata.calc.CalculationRunner;
	using Column = com.opengamma.strata.calc.Column;
	using Results = com.opengamma.strata.calc.Results;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ExampleData = com.opengamma.strata.examples.marketdata.ExampleData;
	using ExampleMarketData = com.opengamma.strata.examples.marketdata.ExampleMarketData;
	using ExampleMarketDataBuilder = com.opengamma.strata.examples.marketdata.ExampleMarketDataBuilder;
	using Measures = com.opengamma.strata.measure.Measures;
	using StandardComponents = com.opengamma.strata.measure.StandardComponents;
	using Trade = com.opengamma.strata.product.Trade;
	using AttributeType = com.opengamma.strata.product.AttributeType;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using TermDeposit = com.opengamma.strata.product.deposit.TermDeposit;
	using TermDepositTrade = com.opengamma.strata.product.deposit.TermDepositTrade;
	using ReportCalculationResults = com.opengamma.strata.report.ReportCalculationResults;
	using TradeReport = com.opengamma.strata.report.trade.TradeReport;
	using TradeReportTemplate = com.opengamma.strata.report.trade.TradeReportTemplate;

	/// <summary>
	/// Example to illustrate using the engine to price a Term Deposit.
	/// <para>
	/// This makes use of the example engine and the example market data environment.
	/// </para>
	/// </summary>
	public class TermDepositPricingExample
	{

	  /// <summary>
	  /// Runs the example, pricing the instruments, producing the output as an ASCII table.
	  /// </summary>
	  /// <param name="args">  ignored </param>
	  public static void Main(string[] args)
	  {
		// setup calculation runner component, which needs life-cycle management
		// a typical application might use dependency injection to obtain the instance
		using (CalculationRunner runner = CalculationRunner.ofMultiThreaded())
		{
		  calculate(runner);
		}
	  }

	  // obtains the data and calculates the grid of results
	  private static void calculate(CalculationRunner runner)
	  {
		// the trades that will have measures calculated
		IList<Trade> trades = ImmutableList.of(createTrade1(), createTrade2());

		// the columns, specifying the measures to be calculated
		IList<Column> columns = ImmutableList.of(Column.of(Measures.PRESENT_VALUE), Column.of(Measures.PV01_CALIBRATED_SUM), Column.of(Measures.PAR_RATE), Column.of(Measures.PAR_SPREAD), Column.of(Measures.PV01_CALIBRATED_BUCKETED));

		// use the built-in example market data
		LocalDate valuationDate = LocalDate.of(2014, 1, 22);
		ExampleMarketDataBuilder marketDataBuilder = ExampleMarketData.builder();
		MarketData marketData = marketDataBuilder.buildSnapshot(valuationDate);

		// the complete set of rules for calculating measures
		CalculationFunctions functions = StandardComponents.calculationFunctions();
		CalculationRules rules = CalculationRules.of(functions, marketDataBuilder.ratesLookup(valuationDate));

		// the reference data, such as holidays and securities
		ReferenceData refData = ReferenceData.standard();

		// calculate the results
		Results results = runner.calculate(rules, trades, columns, marketData, refData);

		// use the report runner to transform the engine results into a trade report
		ReportCalculationResults calculationResults = ReportCalculationResults.of(valuationDate, trades, columns, results, functions, refData);

		TradeReportTemplate reportTemplate = ExampleData.loadTradeReportTemplate("term-deposit-report-template");
		TradeReport tradeReport = TradeReport.of(calculationResults, reportTemplate);
		tradeReport.writeAsciiTable(System.out);
	  }

	  //-----------------------------------------------------------------------  
	  // create a TermDeposit trade
	  private static Trade createTrade1()
	  {
		TermDeposit td = TermDeposit.builder().buySell(BuySell.BUY).startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2014, 12, 12)).businessDayAdjustment(BusinessDayAdjustment.of(FOLLOWING, HolidayCalendarIds.GBLO)).currency(Currency.USD).notional(10_000_000).dayCount(DayCounts.THIRTY_360_ISDA).rate(0.003).build();

		return TermDepositTrade.builder().product(td).info(TradeInfo.builder().id(StandardId.of("example", "1")).addAttribute(AttributeType.DESCRIPTION, "Deposit 10M at 3%").counterparty(StandardId.of("example", "A")).settlementDate(LocalDate.of(2014, 12, 16)).build()).build();
	  }

	  // create a TermDeposit trade
	  private static Trade createTrade2()
	  {
		TermDeposit td = TermDeposit.builder().buySell(BuySell.BUY).startDate(LocalDate.of(2014, 12, 12)).endDate(LocalDate.of(2015, 12, 12)).businessDayAdjustment(BusinessDayAdjustment.of(FOLLOWING, HolidayCalendarIds.GBLO)).currency(Currency.USD).notional(5_000_000).dayCount(DayCounts.THIRTY_360_ISDA).rate(0.0038).build();

		return TermDepositTrade.builder().product(td).info(TradeInfo.builder().id(StandardId.of("example", "2")).addAttribute(AttributeType.DESCRIPTION, "Deposit 5M at 3.8%").counterparty(StandardId.of("example", "A")).settlementDate(LocalDate.of(2015, 12, 16)).build()).build();
	  }

	}

}