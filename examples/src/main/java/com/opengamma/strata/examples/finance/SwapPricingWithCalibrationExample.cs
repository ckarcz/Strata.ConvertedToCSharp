using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.finance
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.measure.StandardComponents.marketDataFactory;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using CalculationRunner = com.opengamma.strata.calc.CalculationRunner;
	using Column = com.opengamma.strata.calc.Column;
	using Results = com.opengamma.strata.calc.Results;
	using MarketDataConfig = com.opengamma.strata.calc.marketdata.MarketDataConfig;
	using MarketDataRequirements = com.opengamma.strata.calc.marketdata.MarketDataRequirements;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using ExampleData = com.opengamma.strata.examples.marketdata.ExampleData;
	using FixingSeriesCsvLoader = com.opengamma.strata.loader.csv.FixingSeriesCsvLoader;
	using QuotesCsvLoader = com.opengamma.strata.loader.csv.QuotesCsvLoader;
	using RatesCalibrationCsvLoader = com.opengamma.strata.loader.csv.RatesCalibrationCsvLoader;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using AdvancedMeasures = com.opengamma.strata.measure.AdvancedMeasures;
	using Measures = com.opengamma.strata.measure.Measures;
	using StandardComponents = com.opengamma.strata.measure.StandardComponents;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using Trade = com.opengamma.strata.product.Trade;
	using AttributeType = com.opengamma.strata.product.AttributeType;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using FixedIborSwapConventions = com.opengamma.strata.product.swap.type.FixedIborSwapConventions;
	using ReportCalculationResults = com.opengamma.strata.report.ReportCalculationResults;
	using TradeReport = com.opengamma.strata.report.trade.TradeReport;
	using TradeReportTemplate = com.opengamma.strata.report.trade.TradeReportTemplate;

	/// <summary>
	/// Example to illustrate using the calculation API to price a swap.
	/// <para>
	/// This makes use of the example market data environment.
	/// </para>
	/// </summary>
	public class SwapPricingWithCalibrationExample
	{

	  /// <summary>
	  /// The valuation date.
	  /// </summary>
	  private static readonly LocalDate VAL_DATE = LocalDate.of(2015, 7, 21);
	  /// <summary>
	  /// The curve group name.
	  /// </summary>
	  private static readonly CurveGroupName CURVE_GROUP_NAME = CurveGroupName.of("USD-DSCON-LIBOR3M");
	  /// <summary>
	  /// The location of the data files.
	  /// </summary>
	  private const string PATH_CONFIG = "src/main/resources/";
	  /// <summary>
	  /// The location of the curve calibration groups file.
	  /// </summary>
	  private static readonly ResourceLocator GROUPS_RESOURCE = ResourceLocator.ofFile(new File(PATH_CONFIG + "example-calibration/curves/groups.csv"));
	  /// <summary>
	  /// The location of the curve calibration settings file.
	  /// </summary>
	  private static readonly ResourceLocator SETTINGS_RESOURCE = ResourceLocator.ofFile(new File(PATH_CONFIG + "example-calibration/curves/settings.csv"));
	  /// <summary>
	  /// The location of the curve calibration nodes file.
	  /// </summary>
	  private static readonly ResourceLocator CALIBRATION_RESOURCE = ResourceLocator.ofFile(new File(PATH_CONFIG + "example-calibration/curves/calibrations.csv"));
	  /// <summary>
	  /// The location of the market quotes file.
	  /// </summary>
	  private static readonly ResourceLocator QUOTES_RESOURCE = ResourceLocator.ofFile(new File(PATH_CONFIG + "example-calibration/quotes/quotes.csv"));
	  /// <summary>
	  /// The location of the historical fixing file.
	  /// </summary>
	  private static readonly ResourceLocator FIXINGS_RESOURCE = ResourceLocator.ofFile(new File(PATH_CONFIG + "example-marketdata/historical-fixings/usd-libor-3m.csv"));

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
		IList<Trade> trades = createSwapTrades();

		// the columns, specifying the measures to be calculated
		IList<Column> columns = ImmutableList.of(Column.of(Measures.LEG_INITIAL_NOTIONAL), Column.of(Measures.PRESENT_VALUE), Column.of(Measures.LEG_PRESENT_VALUE), Column.of(Measures.PV01_CALIBRATED_SUM), Column.of(Measures.PAR_RATE), Column.of(Measures.ACCRUED_INTEREST), Column.of(Measures.PV01_CALIBRATED_BUCKETED), Column.of(AdvancedMeasures.PV01_SEMI_PARALLEL_GAMMA_BUCKETED));

		// load quotes
		ImmutableMap<QuoteId, double> quotes = QuotesCsvLoader.load(VAL_DATE, QUOTES_RESOURCE);

		// load fixings
		ImmutableMap<ObservableId, LocalDateDoubleTimeSeries> fixings = FixingSeriesCsvLoader.load(FIXINGS_RESOURCE);

		// create the market data
		MarketData marketData = MarketData.of(VAL_DATE, quotes, fixings);

		// the reference data, such as holidays and securities
		ReferenceData refData = ReferenceData.standard();

		// load the curve definition
		IDictionary<CurveGroupName, RatesCurveGroupDefinition> defns = RatesCalibrationCsvLoader.load(GROUPS_RESOURCE, SETTINGS_RESOURCE, CALIBRATION_RESOURCE);
		RatesCurveGroupDefinition curveGroupDefinition = defns[CURVE_GROUP_NAME].filtered(VAL_DATE, refData);

		// the configuration that defines how to create the curves when a curve group is requested
		MarketDataConfig marketDataConfig = MarketDataConfig.builder().add(CURVE_GROUP_NAME, curveGroupDefinition).build();

		// the complete set of rules for calculating measures
		CalculationFunctions functions = StandardComponents.calculationFunctions();
		RatesMarketDataLookup ratesLookup = RatesMarketDataLookup.of(curveGroupDefinition);
		CalculationRules rules = CalculationRules.of(functions, ratesLookup);

		// calibrate the curves and calculate the results
		MarketDataRequirements reqs = MarketDataRequirements.of(rules, trades, columns, refData);
		MarketData calibratedMarketData = marketDataFactory().create(reqs, marketDataConfig, marketData, refData);
		Results results = runner.calculate(rules, trades, columns, calibratedMarketData, refData);

		// use the report runner to transform the engine results into a trade report
		ReportCalculationResults calculationResults = ReportCalculationResults.of(VAL_DATE, trades, columns, results, functions, refData);
		TradeReportTemplate reportTemplate = ExampleData.loadTradeReportTemplate("swap-report-template");
		TradeReport tradeReport = TradeReport.of(calculationResults, reportTemplate);
		tradeReport.writeAsciiTable(System.out);
	  }

	  //-----------------------------------------------------------------------  
	  // create swap trades
	  private static IList<Trade> createSwapTrades()
	  {
		return ImmutableList.of(createVanillaFixedVsLibor3mSwap());
	  }

	  //-----------------------------------------------------------------------  
	  // create a vanilla fixed vs libor 3m swap
	  private static Trade createVanillaFixedVsLibor3mSwap()
	  {
		TradeInfo tradeInfo = TradeInfo.builder().id(StandardId.of("example", "1")).addAttribute(AttributeType.DESCRIPTION, "Fixed vs Libor 3m").counterparty(StandardId.of("example", "A")).settlementDate(LocalDate.of(2014, 9, 12)).build();
		return FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M.toTrade(tradeInfo, LocalDate.of(2014, 9, 12), LocalDate.of(2021, 9, 12), BuySell.BUY, 100_000_000, 0.015); // the fixed interest rate
	  }

	}

}