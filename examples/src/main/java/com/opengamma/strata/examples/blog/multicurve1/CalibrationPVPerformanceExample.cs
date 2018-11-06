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
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using QuotesCsvLoader = com.opengamma.strata.loader.csv.QuotesCsvLoader;
	using RatesCalibrationCsvLoader = com.opengamma.strata.loader.csv.RatesCalibrationCsvLoader;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
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
	/// Calibrates one set of curve, computes sensitivity (Bucketed PV01) and estimate computation time.
	/// <para>
	/// Code used for the blog "Strata and multi-curve - Blog 1: Curve calibration and bucketed PV01" available at
	/// https://opengamma.com/blog/strata-and-multi-curve-curve-calibration-and-bucketed-pv01
	/// </para>
	/// </summary>
	public class CalibrationPVPerformanceExample
	{

	  /* Reference data contains calendar. Here we use build-in holiday calendar. 
	   * It is possible to override them with customized versions.*/
	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private static readonly LocalDate VALUATION_DATE = LocalDate.of(2016, 8, 1);

	  // Configuration with discounting curve using OIS up to final maturity; Libor forward curve using IRS.
	  private const string CONFIG_STR = "GBP-DSCONOIS-L6MIRS";
	  private static readonly CurveGroupName CONFIG_NAME = CurveGroupName.of(CONFIG_STR);

	  /* Swap description. */
	  private static readonly Period SWAP_PERIOD_TO_START = Period.ofMonths(3);
	  private const double SWAP_COUPON = 0.0250;
	  private const double SWAP_NOTIONAL = 10_000_000;

	  /* Path to files */
	  private const string PATH_CONFIG = "src/main/resources/example-calibration/curves/";
	  private const string PATH_QUOTES = "src/main/resources/example-calibration/quotes/";
	  /* Files utilities */
	  private const string SUFFIX_CSV = ".csv";
	  private const string GROUPS_SUFFIX = "-group";
	  private const string NODES_SUFFIX = "-nodes";
	  private const string SETTINGS_SUFFIX = "-settings";

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

	  private const int NB_COUPONS = 100;
	  private const double SWAP_COUPON_RANGE = 0.0100;
	  private const int NB_TENORS = 20;
	  private const int TENOR_START = 1;

	  public static void Main(string[] arg)
	  {

		int nbRrpWarm = 2;
		int nbRunPerf = 2;

		/* Load the curve configurations from csv files */
		IDictionary<CurveGroupName, RatesCurveGroupDefinition> configs = RatesCalibrationCsvLoader.load(GROUP_RESOURCE, SETTINGS_RESOURCE, NODES_RESOURCE);

		/* Construct a swaps */
		ResolvedSwapTrade[] swaps = new ResolvedSwapTrade[NB_COUPONS * NB_TENORS];
		for (int loopswap = 0; loopswap < NB_COUPONS; loopswap++)
		{
		  for (int looptenor = 0; looptenor < NB_TENORS; looptenor++)
		  {
			double coupon = SWAP_COUPON + loopswap * SWAP_COUPON_RANGE / NB_COUPONS;
			swaps[looptenor * NB_COUPONS + loopswap] = GBP_FIXED_6M_LIBOR_6M.createTrade(VALUATION_DATE, SWAP_PERIOD_TO_START, Tenor.of(Period.ofYears(TENOR_START + looptenor)), BuySell.BUY, SWAP_NOTIONAL, coupon, REF_DATA).resolve(REF_DATA);
		  }
		}

		/* Warm-up */
		Pair<MultiCurrencyAmount[], CurrencyParameterSensitivities[]> r = Pair.of(new MultiCurrencyAmount[0], new CurrencyParameterSensitivities[0]);
		for (int i = 0; i < nbRrpWarm; i++)
		{
		  r = computation(configs, swaps);
		}

		long start, end;
		start = DateTimeHelper.CurrentUnixTimeMillis();
		for (int i = 0; i < nbRunPerf; i++)
		{
		  r = computation(configs, swaps);
		}

		end = DateTimeHelper.CurrentUnixTimeMillis();
		Console.WriteLine("Computation time: " + (end - start) + " ms");

		Console.WriteLine("Performance estimate for curve calibration, " + (NB_COUPONS * NB_TENORS) + " trades and " + nbRunPerf + " repetitions.\n" + Arrays.ToString(r.First) + Arrays.ToString(r.Second));
	  }

	  private static Pair<MultiCurrencyAmount[], CurrencyParameterSensitivities[]> computation(IDictionary<CurveGroupName, RatesCurveGroupDefinition> configs, ResolvedSwapTrade[] swaps)
	  {

		int nbSwaps = swaps.Length;

		/* Calibrate curves */
		ImmutableRatesProvider multicurve = CALIBRATOR.calibrate(configs[CONFIG_NAME], MARKET_QUOTES, REF_DATA);

		/* Computes PV and bucketed PV01 */
		MultiCurrencyAmount[] pv = new MultiCurrencyAmount[nbSwaps];
		CurrencyParameterSensitivities[] mqs = new CurrencyParameterSensitivities[nbSwaps];
		for (int loopswap = 0; loopswap < nbSwaps; loopswap++)
		{
		  pv[loopswap] = PRICER_SWAP.presentValue(swaps[loopswap], multicurve);
		  PointSensitivities pts = PRICER_SWAP.presentValueSensitivity(swaps[loopswap], multicurve);
		  CurrencyParameterSensitivities ps = multicurve.parameterSensitivity(pts);
		  mqs[loopswap] = MQC.sensitivity(ps, multicurve);
		}

		return Pair.of(pv, mqs);

	  }

	}

}