using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.finance
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.measure.StandardComponents.marketDataFactory;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using CalculationRunner = com.opengamma.strata.calc.CalculationRunner;
	using Column = com.opengamma.strata.calc.Column;
	using Results = com.opengamma.strata.calc.Results;
	using MarketDataConfig = com.opengamma.strata.calc.marketdata.MarketDataConfig;
	using MarketDataRequirements = com.opengamma.strata.calc.marketdata.MarketDataRequirements;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using Result = com.opengamma.strata.collect.result.Result;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using MarketData = com.opengamma.strata.data.MarketData;
	using QuotesCsvLoader = com.opengamma.strata.loader.csv.QuotesCsvLoader;
	using RatesCalibrationCsvLoader = com.opengamma.strata.loader.csv.RatesCalibrationCsvLoader;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using IborFixingDepositCurveNode = com.opengamma.strata.market.curve.node.IborFixingDepositCurveNode;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using Measures = com.opengamma.strata.measure.Measures;
	using StandardComponents = com.opengamma.strata.measure.StandardComponents;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using Trade = com.opengamma.strata.product.Trade;

	/// <summary>
	/// Test for curve calibration with 2 curves in USD.
	/// <para>
	/// One curve is used for Discounting and Fed Fund forward.
	/// The other curve is used for Libor 3M forward.
	/// The Libor forward curve is interpolated directly on forward rates, not on discount factors or zero-rates.
	/// </para>
	/// <para>
	/// Curve configuration and market data loaded from csv files.
	/// Tests that the trades used for calibration have a PV of 0.
	/// </para>
	/// </summary>
	public class CalibrationSimpleForwardCheckExample
	{

	  /// <summary>
	  /// The valuation date.
	  /// </summary>
	  private static readonly LocalDate VAL_DATE = LocalDate.of(2015, 7, 21);

	  /// <summary>
	  /// The tolerance to use.
	  /// </summary>
	  private const double TOLERANCE_PV = 1.0E-8;
	  /// <summary>
	  /// The curve group name.
	  /// </summary>
	  private static readonly CurveGroupName CURVE_GROUP_NAME = CurveGroupName.of("USD-DSCON-LIBOR3M");

	  /// <summary>
	  /// The location of the data files.
	  /// </summary>
	  private const string PATH_CONFIG = "src/main/resources/example-calibration/";
	  /// <summary>
	  /// The location of the curve calibration groups file.
	  /// </summary>
	  private static readonly ResourceLocator GROUPS_RESOURCE = ResourceLocator.ofFile(new File(PATH_CONFIG + "curves/groups.csv"));
	  /// <summary>
	  /// The location of the curve calibration settings file.
	  /// </summary>
	  private static readonly ResourceLocator SETTINGS_RESOURCE = ResourceLocator.ofFile(new File(PATH_CONFIG + "curves/settings-fwd.csv"));
	  /// <summary>
	  /// The location of the curve calibration nodes file.
	  /// </summary>
	  private static readonly ResourceLocator CALIBRATION_RESOURCE = ResourceLocator.ofFile(new File(PATH_CONFIG + "curves/calibrations.csv"));
	  /// <summary>
	  /// The location of the market quotes file.
	  /// </summary>
	  private static readonly ResourceLocator QUOTES_RESOURCE = ResourceLocator.ofFile(new File(PATH_CONFIG + "quotes/quotes.csv"));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Runs the calibration and checks that all the trades used in the curve calibration have a PV of 0.
	  /// </summary>
	  /// <param name="args">  -p to run the performance estimate </param>
	  public static void Main(string[] args)
	  {

		Console.WriteLine("Starting curve calibration: configuration and data loaded from files");
		Pair<IList<Trade>, Results> results = calculate();
		Console.WriteLine("Computed PV for all instruments used in the calibration set");

		// check that all trades have a PV of near 0
		for (int i = 0; i < results.First.Count; i++)
		{
		  Trade trade = results.First[i];
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> pv = results.getSecond().getCells().get(i);
		  Result<object> pv = results.Second.Cells.get(i);
		  string output = "  |--> PV for " + trade.GetType().Name + " computed: " + pv.Success;
		  object pvValue = pv.Value;
		  ArgChecker.isTrue(pvValue is CurrencyAmount, "result type");
		  CurrencyAmount ca = (CurrencyAmount) pvValue;
		  output += " with value: " + ca;
		  Console.WriteLine(output);
		  ArgChecker.isTrue(Math.Abs(ca.Amount) < TOLERANCE_PV, "PV should be small");
		}

		// optionally test performance
		if (args.Length > 0)
		{
		  if (args[0].Equals("-p"))
		  {
			performanceCalibrationPricing();
		  }
		}
		Console.WriteLine("Checked PV for all instruments used in the calibration set are near to zero");
	  }

	  // Example of performance: loading data from file, calibration and PV
	  private static void performanceCalibrationPricing()
	  {
		int nbTests = 10;
		int nbRep = 3;
		int count = 0;

		for (int i = 0; i < nbRep; i++)
		{
		  long startTime = DateTimeHelper.CurrentUnixTimeMillis();
		  for (int looprep = 0; looprep < nbTests; looprep++)
		  {
			Results r = calculate().Second;
			count += r.ColumnCount + r.RowCount;
		  }
		  long endTime = DateTimeHelper.CurrentUnixTimeMillis();
		  Console.WriteLine("Performance: " + nbTests + " config load + curve calibrations + pv check (1 thread) in " + (endTime - startTime) + " ms");
		  // Previous run: 400 ms for 10 cycles
		}
		if (count == 0)
		{
		  Console.WriteLine("Avoiding hotspot: " + count);
		}
	  }

	  //-------------------------------------------------------------------------
	  // setup calculation runner component, which needs life-cycle management
	  // a typical application might use dependency injection to obtain the instance
	  private static Pair<IList<Trade>, Results> calculate()
	  {
		using (CalculationRunner runner = CalculationRunner.ofMultiThreaded())
		{
		  return calculate(runner);
		}
	  }

	  // calculates the PV results for the instruments used in calibration from the config
	  private static Pair<IList<Trade>, Results> calculate(CalculationRunner runner)
	  {
		// the reference data, such as holidays and securities
		ReferenceData refData = ReferenceData.standard();

		// load quotes
		ImmutableMap<QuoteId, double> quotes = QuotesCsvLoader.load(VAL_DATE, QUOTES_RESOURCE);

		// create the market data
		MarketData marketData = ImmutableMarketData.of(VAL_DATE, quotes);

		// load the curve definition
		IDictionary<CurveGroupName, RatesCurveGroupDefinition> defns = RatesCalibrationCsvLoader.load(GROUPS_RESOURCE, SETTINGS_RESOURCE, CALIBRATION_RESOURCE);
		RatesCurveGroupDefinition curveGroupDefinition = defns[CURVE_GROUP_NAME].filtered(VAL_DATE, refData);

		// extract the trades used for calibration
		IList<Trade> trades = curveGroupDefinition.CurveDefinitions.stream().flatMap(defn => defn.Nodes.stream()).filter(node => !(node is IborFixingDepositCurveNode)).map(node => node.trade(1d, marketData, refData)).collect(toImmutableList());

		// the columns, specifying the measures to be calculated
		IList<Column> columns = ImmutableList.of(Column.of(Measures.PRESENT_VALUE));

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
		return Pair.of(trades, results);
	  }

	}

}