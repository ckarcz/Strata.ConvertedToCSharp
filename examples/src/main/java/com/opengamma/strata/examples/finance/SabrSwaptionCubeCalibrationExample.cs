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
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.examples.finance.SwaptionCubeData.DATA_ARRAY_FULL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.examples.finance.SwaptionCubeData.DATA_ARRAY_SPARSE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.examples.finance.SwaptionCubeData.EXPIRIES;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.examples.finance.SwaptionCubeData.MONEYNESS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.examples.finance.SwaptionCubeData.TENORS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.NORMAL_VOLATILITY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.SIMPLE_MONEYNESS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_6M;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using QuotesCsvLoader = com.opengamma.strata.loader.csv.QuotesCsvLoader;
	using RatesCalibrationCsvLoader = com.opengamma.strata.loader.csv.RatesCalibrationCsvLoader;
	using ValueType = com.opengamma.strata.market.ValueType;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using ConstantSurface = com.opengamma.strata.market.surface.ConstantSurface;
	using DefaultSurfaceMetadata = com.opengamma.strata.market.surface.DefaultSurfaceMetadata;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;
	using CalibrationMeasures = com.opengamma.strata.pricer.curve.CalibrationMeasures;
	using RatesCurveCalibrator = com.opengamma.strata.pricer.curve.RatesCurveCalibrator;
	using NormalFormulaRepository = com.opengamma.strata.pricer.impl.option.NormalFormulaRepository;
	using RawOptionData = com.opengamma.strata.pricer.option.RawOptionData;
	using TenorRawOptionData = com.opengamma.strata.pricer.option.TenorRawOptionData;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using SabrParametersSwaptionVolatilities = com.opengamma.strata.pricer.swaption.SabrParametersSwaptionVolatilities;
	using SabrSwaptionCalibrator = com.opengamma.strata.pricer.swaption.SabrSwaptionCalibrator;
	using SabrSwaptionDefinition = com.opengamma.strata.pricer.swaption.SabrSwaptionDefinition;
	using SwaptionVolatilitiesName = com.opengamma.strata.pricer.swaption.SwaptionVolatilitiesName;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;

	/// <summary>
	/// Analysis of swaption cube calibration with shifted SABR smile function.
	/// </summary>
	public class SabrSwaptionCubeCalibrationExample
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private static readonly LocalDate CALIBRATION_DATE = LocalDate.of(2016, 2, 29);
	  private static readonly ZonedDateTime CALIBRATION_TIME = CALIBRATION_DATE.atTime(10, 0).atZone(ZoneId.of("Europe/Berlin"));

	  private static readonly SabrSwaptionCalibrator SABR_CALIBRATION = SabrSwaptionCalibrator.DEFAULT;

	  private const string BASE_DIR = "src/main/resources/";
	  private const string GROUPS_FILE = "example-calibration/curves/EUR-DSCONOIS-E3BS-E6IRS-group.csv";
	  private const string SETTINGS_FILE = "example-calibration/curves/EUR-DSCONOIS-E3BS-E6IRS-settings.csv";
	  private const string NODES_FILE = "example-calibration/curves/EUR-DSCONOIS-E3BS-E6IRS-nodes.csv";
	  private const string QUOTES_FILE = "example-calibration/quotes/quotes-20160229-eur.csv";
	  private static readonly RatesCurveGroupDefinition CONFIGS = RatesCalibrationCsvLoader.load(ResourceLocator.of(BASE_DIR + GROUPS_FILE), ResourceLocator.of(BASE_DIR + SETTINGS_FILE), ResourceLocator.of(BASE_DIR + NODES_FILE)).get(CurveGroupName.of("EUR-DSCONOIS-E3BS-E6IRS"));
	  private static readonly IDictionary<QuoteId, double> MAP_MQ = QuotesCsvLoader.load(CALIBRATION_DATE, ImmutableList.of(ResourceLocator.of(BASE_DIR + QUOTES_FILE)));
	  private static readonly ImmutableMarketData MARKET_QUOTES = ImmutableMarketData.of(CALIBRATION_DATE, MAP_MQ);

	  private static readonly CalibrationMeasures CALIBRATION_MEASURES = CalibrationMeasures.PAR_SPREAD;
	  private static readonly RatesCurveCalibrator CALIBRATOR = RatesCurveCalibrator.of(1e-9, 1e-9, 100, CALIBRATION_MEASURES);
	  private static readonly RatesProvider MULTICURVE = CALIBRATOR.calibrate(CONFIGS, MARKET_QUOTES, REF_DATA);

	  private static readonly DiscountingSwapProductPricer SWAP_PRICER = DiscountingSwapProductPricer.DEFAULT;

	  private static readonly int NB_EXPIRIES = EXPIRIES.size();
	  private static readonly int NB_TENORS = TENORS.size();
	  private static readonly TenorRawOptionData DATA_FULL = rawData(DATA_ARRAY_FULL);
	  private static readonly TenorRawOptionData DATA_SPARSE = rawData(DATA_ARRAY_SPARSE);
	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly SwaptionVolatilitiesName NAME_SABR = SwaptionVolatilitiesName.of("Calibrated-SABR");
	  private static readonly SabrSwaptionDefinition DEFINITION = SabrSwaptionDefinition.of(NAME_SABR, EUR_FIXED_1Y_EURIBOR_6M, ACT_365F, INTERPOLATOR_2D);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Runs the calibration of swaptions and print the calibrated smile results on the console.
	  /// </summary>
	  /// <param name="args">  -s to use the sparse data, i.e. a cube with missing data points </param>
	  public static void Main(string[] args)
	  {

		// select data
		TenorRawOptionData data = DATA_FULL;
		if (args.Length > 0)
		{
		  if (args[0].Equals("-s"))
		  {
			data = DATA_SPARSE;
		  }
		}
		Console.WriteLine("Start calibration");
		double beta = 0.50;
		SurfaceMetadata betaMetadata = DefaultSurfaceMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).zValueType(ValueType.SABR_BETA).surfaceName("Beta").build();
		Surface betaSurface = ConstantSurface.of(betaMetadata, beta);
		double shift = 0.0300;
		Surface shiftSurface = ConstantSurface.of("Shift", shift);
		SabrParametersSwaptionVolatilities calibrated = SABR_CALIBRATION.calibrateWithFixedBetaAndShift(DEFINITION, CALIBRATION_TIME, data, MULTICURVE, betaSurface, shiftSurface);
		Console.WriteLine("End calibration");
		/* Graph calibration */
		int nbStrikesGraph = 50;
		double moneyMin = -0.0250;
		double moneyMax = +0.0300;
		double[] moneyGraph = new double[nbStrikesGraph + 1];
		for (int i = 0; i < nbStrikesGraph + 1; i++)
		{
		  moneyGraph[i] = moneyMin + i * (moneyMax - moneyMin) / nbStrikesGraph;
		}
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][][] strikesGraph = new double[NB_TENORS][NB_EXPIRIES][nbStrikesGraph + 1];
		double[][][] strikesGraph = RectangularArrays.ReturnRectangularDoubleArray(NB_TENORS, NB_EXPIRIES, nbStrikesGraph + 1);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][][] volLNGraph = new double[NB_TENORS][NB_EXPIRIES][nbStrikesGraph + 1];
		double[][][] volLNGraph = RectangularArrays.ReturnRectangularDoubleArray(NB_TENORS, NB_EXPIRIES, nbStrikesGraph + 1);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][][] volNGraph = new double[NB_TENORS][NB_EXPIRIES][nbStrikesGraph + 1];
		double[][][] volNGraph = RectangularArrays.ReturnRectangularDoubleArray(NB_TENORS, NB_EXPIRIES, nbStrikesGraph + 1);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] parRate = new double[NB_TENORS][NB_EXPIRIES];
		double[][] parRate = RectangularArrays.ReturnRectangularDoubleArray(NB_TENORS, NB_EXPIRIES);
		for (int looptenor = 0; looptenor < TENORS.size(); looptenor++)
		{
		  double tenor = TENORS.get(looptenor).get(ChronoUnit.YEARS);
		  for (int loopexpiry = 0; loopexpiry < EXPIRIES.size(); loopexpiry++)
		  {
			LocalDate expiry = EUR_FIXED_1Y_EURIBOR_6M.FloatingLeg.StartDateBusinessDayAdjustment.adjust(CALIBRATION_DATE.plus(EXPIRIES.get(loopexpiry)), REF_DATA);
			LocalDate effectiveDate = EUR_FIXED_1Y_EURIBOR_6M.calculateSpotDateFromTradeDate(expiry, REF_DATA);
			LocalDate endDate = effectiveDate.plus(TENORS.get(looptenor));
			SwapTrade swap = EUR_FIXED_1Y_EURIBOR_6M.toTrade(CALIBRATION_DATE, effectiveDate, endDate, BuySell.BUY, 1.0, 0.0);
			parRate[looptenor][loopexpiry] = SWAP_PRICER.parRate(swap.resolve(REF_DATA).Product, MULTICURVE);
			ZonedDateTime expiryDateTime = expiry.atTime(11, 0).atZone(ZoneId.of("Europe/Berlin"));
			double time = calibrated.relativeTime(expiryDateTime);
			for (int i = 0; i < nbStrikesGraph + 1; i++)
			{
			  strikesGraph[looptenor][loopexpiry][i] = parRate[looptenor][loopexpiry] + moneyGraph[i];
			  volLNGraph[looptenor][loopexpiry][i] = calibrated.volatility(expiryDateTime, tenor, strikesGraph[looptenor][loopexpiry][i], parRate[looptenor][loopexpiry]);
			  volNGraph[looptenor][loopexpiry][i] = NormalFormulaRepository.impliedVolatilityFromBlackApproximated(parRate[looptenor][loopexpiry] + shift, strikesGraph[looptenor][loopexpiry][i] + shift, time, volLNGraph[looptenor][loopexpiry][i]);
			}
		  }
		}

		/* Graph export */
		string svn = "Moneyness";
		for (int looptenor = 0; looptenor < TENORS.size(); looptenor++)
		{
		  for (int loopexpiry = 0; loopexpiry < EXPIRIES.size(); loopexpiry++)
		  {
			svn = svn + ", Strike_" + EXPIRIES.get(loopexpiry).ToString() + "x" + TENORS.get(looptenor).ToString() + ", NormalVol_" + EXPIRIES.get(loopexpiry).ToString() + "x" + TENORS.get(looptenor).ToString();
		  }
		}
		svn = svn + "\n";
		for (int i = 0; i < nbStrikesGraph + 1; i++)
		{
		  svn = svn + moneyGraph[i];
		  for (int looptenor = 0; looptenor < TENORS.size(); looptenor++)
		  {
			for (int loopexpiry = 0; loopexpiry < EXPIRIES.size(); loopexpiry++)
			{
			  svn = svn + ", " + strikesGraph[looptenor][loopexpiry][i];
			  svn = svn + ", " + volNGraph[looptenor][loopexpiry][i];
			}
		  }
		  svn = svn + "\n";
		}
		Console.WriteLine(svn);
	  }

	  private static TenorRawOptionData rawData(double[][][] dataArray)
	  {
		IDictionary<Tenor, RawOptionData> raw = new SortedDictionary<Tenor, RawOptionData>();
		for (int looptenor = 0; looptenor < dataArray.Length; looptenor++)
		{
		  DoubleMatrix matrix = DoubleMatrix.ofUnsafe(dataArray[looptenor]);
		  raw[TENORS.get(looptenor)] = RawOptionData.of(EXPIRIES, MONEYNESS, SIMPLE_MONEYNESS, matrix, NORMAL_VOLATILITY);
		}
		return TenorRawOptionData.of(raw);
	  }

	}

}