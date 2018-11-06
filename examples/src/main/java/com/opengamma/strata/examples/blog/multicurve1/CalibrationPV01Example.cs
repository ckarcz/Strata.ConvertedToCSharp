using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.blog.multicurve1
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedIborSwapConventions.GBP_FIXED_6M_LIBOR_6M;


	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using ExportUtils = com.opengamma.strata.examples.data.export.ExportUtils;
	using QuotesCsvLoader = com.opengamma.strata.loader.csv.QuotesCsvLoader;
	using RatesCalibrationCsvLoader = com.opengamma.strata.loader.csv.RatesCalibrationCsvLoader;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using CalibrationMeasures = com.opengamma.strata.pricer.curve.CalibrationMeasures;
	using RatesCurveCalibrator = com.opengamma.strata.pricer.curve.RatesCurveCalibrator;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using DiscountingSwapTradePricer = com.opengamma.strata.pricer.swap.DiscountingSwapTradePricer;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;

	/// <summary>
	/// Calibrates one set of curve, computes sensitivity (Bucketed PV01) and exports results in Excel for visualization.
	/// <para>
	/// Code used for the blog "Strata and multi-curve - Blog 1: Curve calibration and bucketed PV01" available at
	/// https://opengamma.com/blog/strata-and-multi-curve-curve-calibration-and-bucketed-pv01
	/// </para>
	/// </summary>
	public class CalibrationPV01Example
	{

	  /* Reference data contains calendar. Here we use build-in holiday calendar. 
	   * It is possible to override them with customized versions.*/
	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private static readonly LocalDate VALUATION_DATE = LocalDate.of(2016, 8, 1);

	  // Configuration with discounting curve using OIS up to final maturity; Libor forward curve using IRS.
	  private const string CONFIG_STR = "GBP-DSCONOIS-L6MIRS-FRTB";
	  private static readonly CurveGroupName CONFIG_NAME = CurveGroupName.of(CONFIG_STR);

	  /* Swap description. */
	  private static readonly Period SWAP_TENOR = Period.ofYears(7);
	  private static readonly Period SWAP_PERIOD_TO_START = Period.ofMonths(3);
	  private const double SWAP_COUPON = 0.025;
	  private const double SWAP_NOTIONAL = 10_000_000;

	  /* Path to files */
	  private const string PATH_CONFIG = "src/main/resources/example-calibration/curves/";
	  private const string PATH_QUOTES = "src/main/resources/example-calibration/quotes/";
	  private const string PATH_RESULTS = "target/example-output/";
	  /* Files utilities */
	  private const string SUFFIX_CSV = ".csv";
	  private const string GROUPS_SUFFIX = "-group";
	  private const string NODES_SUFFIX = "-nodes";
	  private const string SETTINGS_SUFFIX = "-linear-settings";

	  private static readonly ResourceLocator GROUP_RESOURCE = ResourceLocator.of(PATH_CONFIG + CONFIG_STR + "/" + CONFIG_STR + GROUPS_SUFFIX + SUFFIX_CSV);
	  private static readonly ResourceLocator SETTINGS_RESOURCE = ResourceLocator.of(PATH_CONFIG + CONFIG_STR + "/" + CONFIG_STR + SETTINGS_SUFFIX + SUFFIX_CSV);
	  private static readonly ResourceLocator NODES_RESOURCE = ResourceLocator.of(PATH_CONFIG + CONFIG_STR + "/" + CONFIG_STR + NODES_SUFFIX + SUFFIX_CSV);

	  /* Raw data */
	  private static readonly string QUOTES_FILE = PATH_QUOTES + "MARKET-QUOTES-GBP-20160801.csv";
	  private static readonly IDictionary<QuoteId, double> MAP_MQ = QuotesCsvLoader.load(VALUATION_DATE, ResourceLocator.of(QUOTES_FILE));
	  private static readonly ImmutableMarketData MARKET_QUOTES = ImmutableMarketData.builder(VALUATION_DATE).values(MAP_MQ).build();

	  private static readonly CalibrationMeasures CALIBRATION_MEASURES = CalibrationMeasures.PAR_SPREAD;
	  private static readonly RatesCurveCalibrator CALIBRATOR = RatesCurveCalibrator.of(1e-9, 1e-9, 100, CALIBRATION_MEASURES);

	  private static readonly DiscountingSwapTradePricer PRICER_SWAP = DiscountingSwapTradePricer.DEFAULT;
	  private static readonly MarketQuoteSensitivityCalculator MQC = MarketQuoteSensitivityCalculator.DEFAULT;

	  private const double BP1 = 1.0E-4; // Scaling by 1 bp.

	  public static void Main(string[] arg)
	  {

		/* Load the curve configurations from csv files */
		IDictionary<CurveGroupName, RatesCurveGroupDefinition> configs = RatesCalibrationCsvLoader.load(GROUP_RESOURCE, SETTINGS_RESOURCE, NODES_RESOURCE);

		/* Calibrate curves */
		ImmutableRatesProvider multicurve = CALIBRATOR.calibrate(configs[CONFIG_NAME], MARKET_QUOTES, REF_DATA);

		/* Construct a swap */
		ResolvedSwapTrade swap = GBP_FIXED_6M_LIBOR_6M.createTrade(VALUATION_DATE, SWAP_PERIOD_TO_START, Tenor.of(SWAP_TENOR), BuySell.BUY, SWAP_NOTIONAL, SWAP_COUPON, REF_DATA).resolve(REF_DATA);

		/* Computes PV and bucketed PV01 */
		MultiCurrencyAmount pv = PRICER_SWAP.presentValue(swap, multicurve);
		PointSensitivities pts = PRICER_SWAP.presentValueSensitivity(swap, multicurve);
		CurrencyParameterSensitivities ps = multicurve.parameterSensitivity(pts);
		CurrencyParameterSensitivities mqs = MQC.sensitivity(ps, multicurve);

		/* Export to csv files. */
		ExportUtils.export(mqs, BP1, PATH_RESULTS + CONFIG_STR + "-delta" + SUFFIX_CSV);
		ExportUtils.export(pv, PATH_RESULTS + CONFIG_STR + "-pv" + SUFFIX_CSV);

		Console.WriteLine("Calibration and export finished: " + CONFIG_STR);

	  }

	}

}