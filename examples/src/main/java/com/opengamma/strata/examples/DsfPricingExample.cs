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
	using Tenor = com.opengamma.strata.basics.date.Tenor;
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
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using Dsf = com.opengamma.strata.product.dsf.Dsf;
	using DsfTrade = com.opengamma.strata.product.dsf.DsfTrade;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using FixedIborSwapConventions = com.opengamma.strata.product.swap.type.FixedIborSwapConventions;
	using ReportCalculationResults = com.opengamma.strata.report.ReportCalculationResults;
	using TradeReport = com.opengamma.strata.report.trade.TradeReport;
	using TradeReportTemplate = com.opengamma.strata.report.trade.TradeReportTemplate;

	/// <summary>
	/// Example to illustrate using the engine to price a Deliverable Swap Future (DSF).
	/// <para>
	/// This makes use of the example engine and the example market data environment.
	/// </para>
	/// </summary>
	public class DsfPricingExample
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
		IList<Column> columns = ImmutableList.of(Column.of(Measures.PRESENT_VALUE), Column.of(Measures.PV01_CALIBRATED_SUM), Column.of(Measures.PV01_CALIBRATED_BUCKETED));

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

		TradeReportTemplate reportTemplate = ExampleData.loadTradeReportTemplate("dsf-report-template");
		TradeReport tradeReport = TradeReport.of(calculationResults, reportTemplate);
		tradeReport.writeAsciiTable(System.out);
	  }

	  //-----------------------------------------------------------------------  
	  // create a trade
	  private static Trade createTrade1(ReferenceData refData)
	  {
		Swap swap = FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M.createTrade(LocalDate.of(2015, 3, 18), Tenor.TENOR_5Y, BuySell.SELL, 1, 0.02, refData).Product;

		Dsf product = Dsf.builder().securityId(SecurityId.of("OG-Future", "CME-F1U-Mar15")).lastTradeDate(LocalDate.of(2015, 3, 16)).deliveryDate(LocalDate.of(2015, 3, 18)).notional(100_000).underlyingSwap(swap).build();

		return DsfTrade.builder().info(TradeInfo.builder().id(StandardId.of("example", "1")).addAttribute(AttributeType.DESCRIPTION, "CME-5Y-DSF Mar15").counterparty(StandardId.of("mn", "Dealer G")).tradeDate(LocalDate.of(2015, 3, 18)).settlementDate(LocalDate.of(2015, 3, 18)).build()).product(product).quantity(20).price(1.0075).build();
	  }

	  // create a trade
	  private static Trade createTrade2(ReferenceData refData)
	  {
		Swap swap = FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M.createTrade(LocalDate.of(2015, 6, 17), Tenor.TENOR_5Y, BuySell.SELL, 1, 0.02, refData).Product;

		Dsf product = Dsf.builder().securityId(SecurityId.of("OG-Future", "CME-F1U-Jun15")).lastTradeDate(LocalDate.of(2015, 6, 15)).deliveryDate(LocalDate.of(2015, 6, 17)).notional(100_000).underlyingSwap(swap).build();

		return DsfTrade.builder().info(TradeInfo.builder().id(StandardId.of("example", "2")).addAttribute(AttributeType.DESCRIPTION, "CME-5Y-DSF Jun15").counterparty(StandardId.of("mn", "Dealer G")).tradeDate(LocalDate.of(2015, 6, 17)).settlementDate(LocalDate.of(2015, 6, 17)).build()).product(product).quantity(20).price(1.0085).build();
	  }

	}

}