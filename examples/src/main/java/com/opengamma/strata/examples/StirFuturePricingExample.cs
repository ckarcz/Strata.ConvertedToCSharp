using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
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
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using Trade = com.opengamma.strata.product.Trade;
	using AttributeType = com.opengamma.strata.product.AttributeType;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using IborFutureTrade = com.opengamma.strata.product.index.IborFutureTrade;
	using IborFutureConventions = com.opengamma.strata.product.index.type.IborFutureConventions;
	using ReportCalculationResults = com.opengamma.strata.report.ReportCalculationResults;
	using TradeReport = com.opengamma.strata.report.trade.TradeReport;
	using TradeReportTemplate = com.opengamma.strata.report.trade.TradeReportTemplate;

	/// <summary>
	/// Example to illustrate using the engine to price an Ibor Future (STIR).
	/// <para>
	/// This makes use of the example engine and the example market data environment.
	/// </para>
	/// </summary>
	public class StirFuturePricingExample
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
		// the reference data, such as holidays and securities
		ReferenceData refData = ReferenceData.standard();

		// the trades that will have measures calculated
		IList<Trade> trades = ImmutableList.of(createTrade1(refData), createTrade2(refData));

		// the columns, specifying the measures to be calculated
		IList<Column> columns = ImmutableList.of(Column.of(Measures.PRESENT_VALUE), Column.of(Measures.PV01_CALIBRATED_SUM), Column.of(Measures.PAR_SPREAD), Column.of(Measures.PV01_CALIBRATED_BUCKETED));

		// use the built-in example market data
		LocalDate valuationDate = LocalDate.of(2014, 1, 22);
		ExampleMarketDataBuilder marketDataBuilder = ExampleMarketData.builder();
		MarketData marketData = marketDataBuilder.buildSnapshot(valuationDate);

		// the complete set of rules for calculating measures
		CalculationFunctions functions = StandardComponents.calculationFunctions();
		CalculationRules rules = CalculationRules.of(functions, marketDataBuilder.ratesLookup(valuationDate));

		// calculate the results
		Results results = runner.calculate(rules, trades, columns, marketData, refData);

		// use the report runner to transform the engine results into a trade report
		ReportCalculationResults calculationResults = ReportCalculationResults.of(valuationDate, trades, columns, results, functions, refData);

		TradeReportTemplate reportTemplate = ExampleData.loadTradeReportTemplate("stir-future-report-template");
		TradeReport tradeReport = TradeReport.of(calculationResults, reportTemplate);
		tradeReport.writeAsciiTable(System.out);
	  }

	  //-----------------------------------------------------------------------  
	  // create a trade
	  private static Trade createTrade1(ReferenceData refData)
	  {
		SecurityId secId = SecurityId.of("OG-Future", "Ibor-USD-LIBOR-3M-Mar15");
		IborFutureTrade trade = IborFutureConventions.USD_LIBOR_3M_QUARTERLY_IMM.createTrade(LocalDate.of(2014, 9, 12), secId, Period.ofMonths(1), 2, 5, 1_000_000, 0.9998, refData);
		return trade.toBuilder().info(TradeInfo.builder().id(StandardId.of("example", "1")).addAttribute(AttributeType.DESCRIPTION, "Mar15 IMM Ibor Future").counterparty(StandardId.of("example", "A")).tradeDate(LocalDate.of(2014, 9, 12)).settlementDate(LocalDate.of(2014, 9, 14)).build()).quantity(20).price(0.9997).build();
	  }

	  // create a trade
	  private static Trade createTrade2(ReferenceData refData)
	  {
		SecurityId secId = SecurityId.of("OG-Future", "Ibor-USD-LIBOR-3M-Jun15");
		IborFutureTrade trade = IborFutureConventions.USD_LIBOR_3M_QUARTERLY_IMM.createTrade(LocalDate.of(2014, 9, 12), secId, Period.ofMonths(1), 3, 10, 1_000_000, 0.9996, refData);
		return trade.toBuilder().info(TradeInfo.builder().id(StandardId.of("example", "1")).addAttribute(AttributeType.DESCRIPTION, "Jun15 IMM Ibor Future").counterparty(StandardId.of("example", "A")).tradeDate(LocalDate.of(2014, 9, 12)).settlementDate(LocalDate.of(2014, 9, 14)).build()).quantity(20).price(0.9997).build();
	  }

	}

}