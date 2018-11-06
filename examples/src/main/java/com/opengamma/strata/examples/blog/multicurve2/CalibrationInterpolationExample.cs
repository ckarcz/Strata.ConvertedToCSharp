using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.blog.multicurve2
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
	using RatesCurveCalibrator = com.opengamma.strata.pricer.curve.RatesCurveCalibrator;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using DiscountingSwapTradePricer = com.opengamma.strata.pricer.swap.DiscountingSwapTradePricer;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;

	/// <summary>
	/// Calibrates one set of curve with several interpolators, computes sensitivity (Bucketed PV01) and exports results in Excel for visualization.
	/// <para>
	/// Code used for the blog "Strata and multi-curve - Blog 2: Interpolation and risk" available at
	/// XXX
	/// </para>
	/// </summary>
	public class CalibrationInterpolationExample
	{

	  /* Reference data contains calendar. Here we use build-in holiday calendar. 
	   * It is possible to override them with customized versions.*/
	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private static readonly LocalDate VALUATION_DATE = LocalDate.of(2016, 8, 1);

	  // Configuration with discounting curve using OIS up to final maturity; Libor forward curve using IRS.
	  private const string CONFIG_STR = "GBP-DSCONOIS-L6MIRS-FRTB";
	  private static readonly CurveGroupName CONFIG_NAME = CurveGroupName.of(CONFIG_STR);

	  /* Swap description. */
	  private static readonly Period SWAP_TENOR = Period.ofYears(8);
	  private static readonly Period SWAP_PERIOD_TO_START = Period.ofMonths(6);
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
	  private static readonly string[] SETTINGS_SUFFIX = new string[]{"-linear-settings", "-dq-settings", "-ncs-settings"};
	  private const int NB_SETTINGS = 3;

	  private static readonly ResourceLocator GROUP_RESOURCE = ResourceLocator.of(PATH_CONFIG + CONFIG_STR + "/" + CONFIG_STR + GROUPS_SUFFIX + SUFFIX_CSV);
	  private static readonly ResourceLocator[] SETTINGS_RESOURCE = new ResourceLocator[]{ResourceLocator.of(PATH_CONFIG + CONFIG_STR + "/" + CONFIG_STR + SETTINGS_SUFFIX[0] + SUFFIX_CSV), ResourceLocator.of(PATH_CONFIG + CONFIG_STR + "/" + CONFIG_STR + SETTINGS_SUFFIX[1] + SUFFIX_CSV), ResourceLocator.of(PATH_CONFIG + CONFIG_STR + "/" + CONFIG_STR + SETTINGS_SUFFIX[2] + SUFFIX_CSV)};
	  private static readonly ResourceLocator NODES_RESOURCE = ResourceLocator.of(PATH_CONFIG + CONFIG_STR + "/" + CONFIG_STR + NODES_SUFFIX + SUFFIX_CSV);

	  /* Raw data */
	  private static readonly string QUOTES_FILE = PATH_QUOTES + "MARKET-QUOTES-GBP-20160801.csv";
	  private static readonly IDictionary<QuoteId, double> MAP_MQ = QuotesCsvLoader.load(VALUATION_DATE, ResourceLocator.of(QUOTES_FILE));
	  private static readonly ImmutableMarketData MARKET_QUOTES = ImmutableMarketData.builder(VALUATION_DATE).values(MAP_MQ).build();

	  private static readonly RatesCurveCalibrator CALIBRATOR = RatesCurveCalibrator.standard();

	  private static readonly DiscountingSwapTradePricer PRICER_SWAP = DiscountingSwapTradePricer.DEFAULT;
	  private static readonly MarketQuoteSensitivityCalculator MQC = MarketQuoteSensitivityCalculator.DEFAULT;

	  private const double BP1 = 1.0E-4; // Scaling by 1 bp.

	  public static void Main(string[] arg)
	  {

		/* Load the curve configurations from csv files */
		IList<IDictionary<CurveGroupName, RatesCurveGroupDefinition>> configs = new List<IDictionary<CurveGroupName, RatesCurveGroupDefinition>>();
		for (int loopconfig = 0; loopconfig < NB_SETTINGS; loopconfig++)
		{
		  configs.Add(RatesCalibrationCsvLoader.load(GROUP_RESOURCE, SETTINGS_RESOURCE[loopconfig], NODES_RESOURCE));
		}

		/* Construct a swap */
		ResolvedSwapTrade swap = GBP_FIXED_6M_LIBOR_6M.createTrade(VALUATION_DATE, SWAP_PERIOD_TO_START, Tenor.of(SWAP_TENOR), BuySell.BUY, SWAP_NOTIONAL, SWAP_COUPON, REF_DATA).resolve(REF_DATA);

		/* Calibrate curves */
		ImmutableRatesProvider[] multicurve = new ImmutableRatesProvider[3];
		for (int loopconfig = 0; loopconfig < NB_SETTINGS; loopconfig++)
		{
		  multicurve[loopconfig] = CALIBRATOR.calibrate(configs[loopconfig][CONFIG_NAME], MARKET_QUOTES, REF_DATA);
		}

		/* Computes PV and bucketed PV01 */
		MultiCurrencyAmount[] pv = new MultiCurrencyAmount[NB_SETTINGS];
		CurrencyParameterSensitivities[] mqs = new CurrencyParameterSensitivities[NB_SETTINGS];
		for (int loopconfig = 0; loopconfig < NB_SETTINGS; loopconfig++)
		{
		  pv[loopconfig] = PRICER_SWAP.presentValue(swap, multicurve[loopconfig]);
		  PointSensitivities pts = PRICER_SWAP.presentValueSensitivity(swap, multicurve[loopconfig]);
		  CurrencyParameterSensitivities ps = multicurve[loopconfig].parameterSensitivity(pts);
		  mqs[loopconfig] = MQC.sensitivity(ps, multicurve[loopconfig]);
		}

		/* Export to csv files. */
		for (int loopconfig = 0; loopconfig < NB_SETTINGS; loopconfig++)
		{
		  ExportUtils.export(mqs[loopconfig], BP1, PATH_RESULTS + CONFIG_STR + SETTINGS_SUFFIX[loopconfig] + "-mqs" + SUFFIX_CSV);
		  ExportUtils.export(pv[loopconfig], PATH_RESULTS + CONFIG_STR + SETTINGS_SUFFIX[loopconfig] + "-pv" + SUFFIX_CSV);
		}

		Console.WriteLine("Calibration and export finished: " + CONFIG_STR);

	  }

	}

}