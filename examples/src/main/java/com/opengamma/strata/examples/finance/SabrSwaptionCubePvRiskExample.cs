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
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
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
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using ConstantSurface = com.opengamma.strata.market.surface.ConstantSurface;
	using DefaultSurfaceMetadata = com.opengamma.strata.market.surface.DefaultSurfaceMetadata;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;
	using CalibrationMeasures = com.opengamma.strata.pricer.curve.CalibrationMeasures;
	using RatesCurveCalibrator = com.opengamma.strata.pricer.curve.RatesCurveCalibrator;
	using RawOptionData = com.opengamma.strata.pricer.option.RawOptionData;
	using TenorRawOptionData = com.opengamma.strata.pricer.option.TenorRawOptionData;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SabrParametersSwaptionVolatilities = com.opengamma.strata.pricer.swaption.SabrParametersSwaptionVolatilities;
	using SabrSwaptionCalibrator = com.opengamma.strata.pricer.swaption.SabrSwaptionCalibrator;
	using SabrSwaptionDefinition = com.opengamma.strata.pricer.swaption.SabrSwaptionDefinition;
	using SabrSwaptionPhysicalProductPricer = com.opengamma.strata.pricer.swaption.SabrSwaptionPhysicalProductPricer;
	using SwaptionVolatilitiesName = com.opengamma.strata.pricer.swaption.SwaptionVolatilitiesName;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using LongShort = com.opengamma.strata.product.common.LongShort;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;
	using PhysicalSwaptionSettlement = com.opengamma.strata.product.swaption.PhysicalSwaptionSettlement;
	using ResolvedSwaption = com.opengamma.strata.product.swaption.ResolvedSwaption;
	using Swaption = com.opengamma.strata.product.swaption.Swaption;

	/// <summary>
	/// Analysis of the pricing and risk of a swaption with calibrated SABR parameters.
	/// </summary>
	public class SabrSwaptionCubePvRiskExample
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

	  private static readonly SabrSwaptionPhysicalProductPricer SWAPTION_PRICER = SabrSwaptionPhysicalProductPricer.DEFAULT;

	  private static readonly TenorRawOptionData DATA_FULL = rawData(DATA_ARRAY_FULL);
	  private static readonly TenorRawOptionData DATA_SPARSE = rawData(DATA_ARRAY_SPARSE);
	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly SwaptionVolatilitiesName NAME_SABR = SwaptionVolatilitiesName.of("Calibrated-SABR");
	  private static readonly SabrSwaptionDefinition DEFINITION = SabrSwaptionDefinition.of(NAME_SABR, EUR_FIXED_1Y_EURIBOR_6M, ACT_365F, INTERPOLATOR_2D);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Runs the calibration of SABR on swaptions and print on the console the present value, bucketed PV01 and 
	  /// the bucketed Vega of a 18M x 4Y swaption.
	  /// </summary>
	  /// <param name="args">  -s to use the spares data </param>
	  public static void Main(string[] args)
	  {

		long start, end;

		// Swaption description
		BuySell payer = BuySell.BUY;
		Period expiry = Period.ofMonths(18);
		double notional = 1_000_000;
		double strike = 0.0100;
		Tenor tenor = Tenor.TENOR_4Y;
		LocalDate expiryDate = EUR_FIXED_1Y_EURIBOR_6M.FloatingLeg.StartDateBusinessDayAdjustment.adjust(CALIBRATION_DATE.plus(expiry), REF_DATA);
		SwapTrade underlying = EUR_FIXED_1Y_EURIBOR_6M.createTrade(expiryDate, tenor, payer, notional, strike, REF_DATA);
		Swaption swaption = Swaption.builder().expiryDate(AdjustableDate.of(expiryDate)).expiryTime(LocalTime.of(11, 0x0)).expiryZone(ZoneId.of("Europe/Berlin")).underlying(underlying.Product).longShort(LongShort.LONG).swaptionSettlement(PhysicalSwaptionSettlement.DEFAULT).build();
		ResolvedSwaption resolvedSwaption = swaption.resolve(REF_DATA);

		// select data
		TenorRawOptionData data = DATA_FULL;
		if (args.Length > 0)
		{
		  if (args[0].Equals("-s"))
		  {
			data = DATA_SPARSE;
		  }
		}

		start = DateTimeHelper.CurrentUnixTimeMillis();
		// Curve calibration 
		RatesProvider multicurve = CALIBRATOR.calibrate(CONFIGS, MARKET_QUOTES, REF_DATA);
		end = DateTimeHelper.CurrentUnixTimeMillis();
		Console.WriteLine("Curve calibration time: " + (end - start) + " ms.");

		// SABR calibration 
		start = DateTimeHelper.CurrentUnixTimeMillis();
		double beta = 0.50;
		SurfaceMetadata betaMetadata = DefaultSurfaceMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).zValueType(ValueType.SABR_BETA).surfaceName("Beta").build();
		Surface betaSurface = ConstantSurface.of(betaMetadata, beta);
		double shift = 0.0300;
		Surface shiftSurface = ConstantSurface.of("SABR-Shift", shift);
		SabrParametersSwaptionVolatilities sabr = SABR_CALIBRATION.calibrateWithFixedBetaAndShift(DEFINITION, CALIBRATION_TIME, data, multicurve, betaSurface, shiftSurface);
		end = DateTimeHelper.CurrentUnixTimeMillis();
		Console.WriteLine("SABR calibration time: " + (end - start) + " ms.");

		// Price and risk
		Console.WriteLine("Risk measures: ");
		start = DateTimeHelper.CurrentUnixTimeMillis();
		CurrencyAmount pv = SWAPTION_PRICER.presentValue(resolvedSwaption, multicurve, sabr);
		Console.WriteLine("  |-> PV: " + pv.ToString());

		PointSensitivities deltaPts = SWAPTION_PRICER.presentValueSensitivityRatesStickyModel(resolvedSwaption, multicurve, sabr).build();
		CurrencyParameterSensitivities deltaBucketed = multicurve.parameterSensitivity(deltaPts);
		Console.WriteLine("  |-> Delta bucketed: " + deltaBucketed.ToString());

		PointSensitivities vegaPts = SWAPTION_PRICER.presentValueSensitivityModelParamsSabr(resolvedSwaption, multicurve, sabr).build();
		Console.WriteLine("  |-> Vega point: " + vegaPts.ToString());

		CurrencyParameterSensitivities vegaBucketed = sabr.parameterSensitivity(vegaPts);
		for (int i = 0; i < vegaBucketed.size(); i++)
		{
		  Console.WriteLine("  |-> Vega bucketed: " + vegaBucketed.Sensitivities.get(i));
		}

		end = DateTimeHelper.CurrentUnixTimeMillis();
		Console.WriteLine("PV and risk time: " + (end - start) + " ms.");
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