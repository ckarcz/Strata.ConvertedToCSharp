using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.report
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.measure.StandardComponents.marketDataFactory;


	using JCommander = com.beust.jcommander.JCommander;
	using Parameter = com.beust.jcommander.Parameter;
	using ParameterException = com.beust.jcommander.ParameterException;
	using Strings = com.google.common.@base.Strings;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using CalculationRunner = com.opengamma.strata.calc.CalculationRunner;
	using Column = com.opengamma.strata.calc.Column;
	using Results = com.opengamma.strata.calc.Results;
	using MarketDataConfig = com.opengamma.strata.calc.marketdata.MarketDataConfig;
	using MarketDataRequirements = com.opengamma.strata.calc.marketdata.MarketDataRequirements;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using CalculationTasks = com.opengamma.strata.calc.runner.CalculationTasks;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ExampleMarketData = com.opengamma.strata.examples.marketdata.ExampleMarketData;
	using ExampleMarketDataBuilder = com.opengamma.strata.examples.marketdata.ExampleMarketDataBuilder;
	using StandardComponents = com.opengamma.strata.measure.StandardComponents;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using Trade = com.opengamma.strata.product.Trade;
	using Report = com.opengamma.strata.report.Report;
	using ReportCalculationResults = com.opengamma.strata.report.ReportCalculationResults;
	using ReportRequirements = com.opengamma.strata.report.ReportRequirements;
	using ReportRunner = com.opengamma.strata.report.ReportRunner;
	using ReportTemplate = com.opengamma.strata.report.ReportTemplate;
	using CashFlowReportRunner = com.opengamma.strata.report.cashflow.CashFlowReportRunner;
	using CashFlowReportTemplate = com.opengamma.strata.report.cashflow.CashFlowReportTemplate;
	using ReportOutputFormat = com.opengamma.strata.report.framework.format.ReportOutputFormat;
	using TradeReportRunner = com.opengamma.strata.report.trade.TradeReportRunner;
	using TradeReportTemplate = com.opengamma.strata.report.trade.TradeReportTemplate;

	/// <summary>
	/// Tool for running a report from the command line.
	/// </summary>
	public sealed class ReportRunnerTool : AutoCloseable
	{

	  /// <summary>
	  /// The calculation runner.
	  /// </summary>
	  private readonly CalculationRunner runner;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Parameter(names = {"-t", "--template"}, description = "Report template input file", required = true, converter = ReportTemplateParameterConverter.class) private com.opengamma.strata.report.ReportTemplate template;
	  private ReportTemplate template;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Parameter(names = {"-m", "--marketdata"}, description = "Market data root directory", validateValueWith = MarketDataRootValidator.class) private java.io.File marketDataRoot;
	  private File marketDataRoot;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Parameter(names = {"-p", "--portfolio"}, description = "Portfolio input file", required = true, converter = TradeListParameterConverter.class) private TradeList tradeList;
	  private TradeList tradeList;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Parameter(names = {"-d", "--date"}, description = "Valuation date, YYYY-MM-DD", required = true, converter = LocalDateParameterConverter.class) private java.time.LocalDate valuationDate;
	  private LocalDate valuationDate;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Parameter(names = {"-f", "--format"}, description = "Report output format, ascii or csv", converter = ReportOutputFormatParameterConverter.class) private com.opengamma.strata.report.framework.format.ReportOutputFormat format = com.opengamma.strata.report.framework.format.ReportOutputFormat.ASCII_TABLE;
	  private ReportOutputFormat format = ReportOutputFormat.ASCII_TABLE;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Parameter(names = {"-i", "--id"}, description = "An ID by which to select a single trade") private String idSearch;
	  private string idSearch;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Parameter(names = {"-h", "--help"}, description = "Displays this message", help = true) private boolean help;
	  private bool help;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Parameter(names = {"-v", "--version"}, description = "Prints the version of this tool", help = true) private boolean version;
	  private bool version;

	  /// <summary>
	  /// Runs the tool.
	  /// </summary>
	  /// <param name="args">  the command-line arguments </param>
	  public static void Main(string[] args)
	  {
		using (ReportRunnerTool reportRunner = new ReportRunnerTool(CalculationRunner.ofMultiThreaded()))
		{
		  JCommander commander = new JCommander(reportRunner);
		  commander.ProgramName = typeof(ReportRunnerTool).Name;
		  try
		  {
			commander.parse(args);
		  }
		  catch (ParameterException e)
		  {
			Console.Error.WriteLine("Error: " + e.Message);
			Console.Error.WriteLine();
			commander.usage();
			return;
		  }
		  if (reportRunner.help)
		  {
			commander.usage();
		  }
		  else if (reportRunner.version)
		  {
			string versionName = typeof(ReportRunnerTool).Assembly.ImplementationVersion;
			if (string.ReferenceEquals(versionName, null))
			{
			  versionName = "unknown";
			}
			Console.WriteLine("Strata Report Runner Tool, version " + versionName);
		  }
		  else
		  {
			try
			{
			  reportRunner.run();
			}
			catch (Exception e)
			{
			  Console.Error.WriteLine(Messages.format("Error: {}\n", e.Message));
			  commander.usage();
			}
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  // creates an instance
	  private ReportRunnerTool(CalculationRunner runner)
	  {
		this.runner = ArgChecker.notNull(runner, "runner");
	  }

	  //-------------------------------------------------------------------------
	  private void run()
	  {
		ReportRunner<ReportTemplate> reportRunner = getReportRunner(template);
		ReportRequirements requirements = reportRunner.requirements(template);
		ReportCalculationResults calculationResults = runCalculationRequirements(requirements);

		Report report = reportRunner.runReport(calculationResults, template);

		switch (format)
		{
		  case ReportOutputFormat.ASCII_TABLE:
			report.writeAsciiTable(System.out);
			break;
		  case ReportOutputFormat.CSV:
			report.writeCsv(System.out);
			break;
		}
	  }

	  private ReportCalculationResults runCalculationRequirements(ReportRequirements requirements)
	  {
		IList<Column> columns = requirements.TradeMeasureRequirements;

		ExampleMarketDataBuilder marketDataBuilder = marketDataRoot == null ? ExampleMarketData.builder() : ExampleMarketDataBuilder.ofPath(marketDataRoot.toPath());

		CalculationFunctions functions = StandardComponents.calculationFunctions();
		RatesMarketDataLookup ratesLookup = marketDataBuilder.ratesLookup(valuationDate);
		CalculationRules rules = CalculationRules.of(functions, ratesLookup);

		MarketData marketData = marketDataBuilder.buildSnapshot(valuationDate);

		IList<Trade> trades;

		if (Strings.nullToEmpty(idSearch).Trim().Empty)
		{
		  trades = tradeList.Trades;
		}
		else
		{
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		  trades = tradeList.Trades.Where(t => t.Info.Id.Present).Where(t => t.Info.Id.get().Value.Equals(idSearch)).collect(toImmutableList());
		  if (trades.Count > 1)
		  {
			throw new System.ArgumentException(Messages.format("More than one trade found matching ID: '{}'", idSearch));
		  }
		}
		if (trades.Count == 0)
		{
		  throw new System.ArgumentException("No trades found. Please check the input portfolio or trade ID filter.");
		}

		// the reference data, such as holidays and securities
		ReferenceData refData = ReferenceData.standard();

		// calculate the results
		CalculationTasks tasks = CalculationTasks.of(rules, trades, columns, refData);
		MarketDataRequirements reqs = tasks.requirements(refData);
		MarketData calibratedMarketData = marketDataFactory().create(reqs, MarketDataConfig.empty(), marketData, refData);
		Results results = runner.TaskRunner.calculate(tasks, calibratedMarketData, refData);

		return ReportCalculationResults.of(valuationDate, trades, requirements.TradeMeasureRequirements, results, functions, refData);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes"}) private com.opengamma.strata.report.ReportRunner<com.opengamma.strata.report.ReportTemplate> getReportRunner(com.opengamma.strata.report.ReportTemplate reportTemplate)
	  private ReportRunner<ReportTemplate> getReportRunner(ReportTemplate reportTemplate)
	  {
		// double-casts to achieve result type, allowing report runner to be used without external knowledge of template type
		if (reportTemplate is TradeReportTemplate)
		{
		  return (ReportRunner) TradeReportRunner.INSTANCE;
		}
		else if (reportTemplate is CashFlowReportTemplate)
		{
		  return (ReportRunner) CashFlowReportRunner.INSTANCE;
		}
		throw new System.ArgumentException(Messages.format("Unsupported report type: {}", reportTemplate.GetType().Name));
	  }

	  public override void close()
	  {
		runner.close();
	  }

	}

}