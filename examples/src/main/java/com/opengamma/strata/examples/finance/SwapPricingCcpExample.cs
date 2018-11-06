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
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using ExampleData = com.opengamma.strata.examples.marketdata.ExampleData;
	using FixingSeriesCsvLoader = com.opengamma.strata.loader.csv.FixingSeriesCsvLoader;
	using QuotesCsvLoader = com.opengamma.strata.loader.csv.QuotesCsvLoader;
	using RatesCalibrationCsvLoader = com.opengamma.strata.loader.csv.RatesCalibrationCsvLoader;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using Measures = com.opengamma.strata.measure.Measures;
	using StandardComponents = com.opengamma.strata.measure.StandardComponents;
	using TradeCounterpartyCalculationParameter = com.opengamma.strata.measure.calc.TradeCounterpartyCalculationParameter;
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
	/// Example to illustrate using the engine to price a swap.
	/// <para>
	/// This makes use of the example engine and the example market data environment.
	/// </para>
	/// </summary>
	public class SwapPricingCcpExample
	{

	  /// <summary>
	  /// The valuation date.
	  /// </summary>
	  private static readonly LocalDate VAL_DATE = LocalDate.of(2015, 7, 21);
	  /// <summary>
	  /// The curve group name.
	  /// </summary>
	  private static readonly CurveGroupName CURVE_GROUP_NAME_CCP1 = CurveGroupName.of("USD-DSCON-LIBOR3M");
	  private static readonly CurveGroupName CURVE_GROUP_NAME_CCP2 = CurveGroupName.of("USD-DSCON-LIBOR3M-CCP2");
	  /// <summary>
	  /// The location of the data files.
	  /// </summary>
	  private const string PATH_CONFIG = "src/main/resources/";
	  /// <summary>
	  /// The location of the curve calibration groups file for CCP1 and CCP2.
	  /// </summary>
	  private static readonly ResourceLocator GROUPS_RESOURCE_CCP1 = ResourceLocator.ofFile(new File(PATH_CONFIG + "example-calibration/curves/groups.csv"));
	  private static readonly ResourceLocator GROUPS_RESOURCE_CCP2 = ResourceLocator.ofFile(new File(PATH_CONFIG + "example-calibration/curves/groups-ccp2.csv"));
	  /// <summary>
	  /// The location of the curve calibration settings file for CCP1 and CCP2.
	  /// </summary>
	  private static readonly ResourceLocator SETTINGS_RESOURCE_CCP1 = ResourceLocator.ofFile(new File(PATH_CONFIG + "example-calibration/curves/settings.csv"));
	  private static readonly ResourceLocator SETTINGS_RESOURCE_CCP2 = ResourceLocator.ofFile(new File(PATH_CONFIG + "example-calibration/curves/settings-ccp2.csv"));
	  /// <summary>
	  /// The location of the curve calibration nodes file for CCP1 and CCP2.
	  /// </summary>
	  private static readonly ResourceLocator CALIBRATION_RESOURCE_CCP1 = ResourceLocator.ofFile(new File(PATH_CONFIG + "example-calibration/curves/calibrations.csv"));
	  private static readonly ResourceLocator CALIBRATION_RESOURCE_CCP2 = ResourceLocator.ofFile(new File(PATH_CONFIG + "example-calibration/curves/calibrations-ccp2.csv"));
	  /// <summary>
	  /// The location of the market quotes file for CCP1 and CCP2.
	  /// </summary>
	  private static readonly ResourceLocator QUOTES_RESOURCE_CCP1 = ResourceLocator.ofFile(new File(PATH_CONFIG + "example-calibration/quotes/quotes.csv"));
	  private static readonly ResourceLocator QUOTES_RESOURCE_CCP2 = ResourceLocator.ofFile(new File(PATH_CONFIG + "example-calibration/quotes/quotes-ccp2.csv"));
	  /// <summary>
	  /// The location of the historical fixing file.
	  /// </summary>
	  private static readonly ResourceLocator FIXINGS_RESOURCE = ResourceLocator.ofFile(new File(PATH_CONFIG + "example-marketdata/historical-fixings/usd-libor-3m.csv"));

	  /// <summary>
	  /// The first counterparty.
	  /// </summary>
	  private static readonly StandardId CCP1_ID = StandardId.of("example", "CCP-1");
	  /// <summary>
	  /// The second counterparty.
	  /// </summary>
	  private static readonly StandardId CCP2_ID = StandardId.of("example", "CCP-2");

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
		IList<Column> columns = ImmutableList.of(Column.of(Measures.PRESENT_VALUE), Column.of(Measures.PAR_RATE), Column.of(Measures.PV01_MARKET_QUOTE_BUCKETED), Column.of(Measures.PV01_CALIBRATED_BUCKETED));

		// load quotes
		ImmutableMap<QuoteId, double> quotesCcp1 = QuotesCsvLoader.load(VAL_DATE, QUOTES_RESOURCE_CCP1);
		ImmutableMap<QuoteId, double> quotesCcp2 = QuotesCsvLoader.load(VAL_DATE, QUOTES_RESOURCE_CCP2);

		// load fixings
		ImmutableMap<ObservableId, LocalDateDoubleTimeSeries> fixings = FixingSeriesCsvLoader.load(FIXINGS_RESOURCE);

		// create the market data
		MarketData marketData = ImmutableMarketData.builder(VAL_DATE).addValueMap(quotesCcp1).addValueMap(quotesCcp2).addTimeSeriesMap(fixings).build();

		// the reference data, such as holidays and securities
		ReferenceData refData = ReferenceData.standard();

		// load the curve definition
		IDictionary<CurveGroupName, RatesCurveGroupDefinition> defnsCcp1 = RatesCalibrationCsvLoader.load(GROUPS_RESOURCE_CCP1, SETTINGS_RESOURCE_CCP1, CALIBRATION_RESOURCE_CCP1);
		IDictionary<CurveGroupName, RatesCurveGroupDefinition> defnsCcp2 = RatesCalibrationCsvLoader.load(GROUPS_RESOURCE_CCP2, SETTINGS_RESOURCE_CCP2, CALIBRATION_RESOURCE_CCP2);
		RatesCurveGroupDefinition curveGroupDefinitionCcp1 = defnsCcp1[CURVE_GROUP_NAME_CCP1].filtered(VAL_DATE, refData);
		RatesCurveGroupDefinition curveGroupDefinitionCcp2 = defnsCcp2[CURVE_GROUP_NAME_CCP2].filtered(VAL_DATE, refData);

		// the configuration that defines how to create the curves when a curve group is requested
		MarketDataConfig marketDataConfig = MarketDataConfig.builder().add(CURVE_GROUP_NAME_CCP1, curveGroupDefinitionCcp1).add(CURVE_GROUP_NAME_CCP2, curveGroupDefinitionCcp2).build();

		// the complete set of rules for calculating measures
		CalculationFunctions functions = StandardComponents.calculationFunctions();
		RatesMarketDataLookup ratesLookupCcp1 = RatesMarketDataLookup.of(curveGroupDefinitionCcp1);
		RatesMarketDataLookup ratesLookupCcp2 = RatesMarketDataLookup.of(curveGroupDefinitionCcp2);
		// choose RatesMarketDataLookup instance based on counterparty
		TradeCounterpartyCalculationParameter perCounterparty = TradeCounterpartyCalculationParameter.of(ImmutableMap.of(CCP1_ID, ratesLookupCcp1, CCP2_ID, ratesLookupCcp2), ratesLookupCcp1);
		CalculationRules rules = CalculationRules.of(functions, perCounterparty);

		// calibrate the curves and calculate the results
		MarketDataRequirements reqs = MarketDataRequirements.of(rules, trades, columns, refData);
		MarketData calibratedMarketData = marketDataFactory().create(reqs, marketDataConfig, marketData, refData);
		Results results = runner.calculate(rules, trades, columns, calibratedMarketData, refData);

		// use the report runner to transform the engine results into a trade report
		ReportCalculationResults calculationResults = ReportCalculationResults.of(VAL_DATE, trades, columns, results, functions, refData);
		TradeReportTemplate reportTemplate = ExampleData.loadTradeReportTemplate("swap-report-template2");
		TradeReport tradeReport = TradeReport.of(calculationResults, reportTemplate);
		tradeReport.writeAsciiTable(System.out);
	  }

	  //-----------------------------------------------------------------------  
	  // create swap trades
	  private static IList<Trade> createSwapTrades()
	  {
		return ImmutableList.of(createVanillaFixedVsLibor3mSwap(CCP1_ID), createVanillaFixedVsLibor3mSwap(CCP2_ID));
	  }

	  //-----------------------------------------------------------------------  
	  // create a vanilla fixed vs libor 3m swap
	  private static Trade createVanillaFixedVsLibor3mSwap(StandardId ctptyId)
	  {
		TradeInfo tradeInfo = TradeInfo.builder().id(StandardId.of("example", "1")).addAttribute(AttributeType.DESCRIPTION, "Fixed vs Libor 3m").counterparty(ctptyId).settlementDate(LocalDate.of(2014, 9, 12)).build();
		return FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M.toTrade(tradeInfo, LocalDate.of(2014, 9, 12), LocalDate.of(2021, 9, 12), BuySell.BUY, 100_000_000, 0.015); // the fixed interest rate
	  }

	}

}