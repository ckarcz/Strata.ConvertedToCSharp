using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swaption.SwaptionCubeData.DAY_COUNT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
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
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;
	using CalibrationMeasures = com.opengamma.strata.pricer.curve.CalibrationMeasures;
	using RatesCurveCalibrator = com.opengamma.strata.pricer.curve.RatesCurveCalibrator;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;
	using TenorRawOptionData = com.opengamma.strata.pricer.option.TenorRawOptionData;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;

	/// <summary>
	/// Tests <seealso cref="SabrSwaptionCalibrator"/> for a cube. Realistic dimension and data.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrSwaptionCalibratorCubeBlackExtremeDataTest
	public class SabrSwaptionCalibratorCubeBlackExtremeDataTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private static readonly LocalDate CALIBRATION_DATE = LocalDate.of(2016, 2, 29);
	  private static readonly ZonedDateTime CALIBRATION_TIME = CALIBRATION_DATE.atTime(10, 0).atZone(ZoneId.of("Europe/Berlin"));

	  private static readonly SabrSwaptionCalibrator SABR_CALIBRATION = SabrSwaptionCalibrator.DEFAULT;

	  private const string BASE_DIR = "src/test/resources/";
	  private const string GROUPS_FILE = "curve-config/EUR-DSCONOIS-E3BS-E6IRS-group.csv";
	  private const string SETTINGS_FILE = "curve-config/EUR-DSCONOIS-E3BS-E6IRS-settings.csv";
	  private const string NODES_FILE = "curve-config/EUR-DSCONOIS-E3BS-E6IRS-nodes.csv";
	  private const string QUOTES_FILE = "quotes/quotes-20160229-eur.csv";
	  private static readonly RatesCurveGroupDefinition CONFIGS = RatesCalibrationCsvLoader.load(ResourceLocator.of(BASE_DIR + GROUPS_FILE), ResourceLocator.of(BASE_DIR + SETTINGS_FILE), ResourceLocator.of(BASE_DIR + NODES_FILE)).get(CurveGroupName.of("EUR-DSCONOIS-E3BS-E6IRS"));
	  private static readonly IDictionary<QuoteId, double> MAP_MQ = QuotesCsvLoader.load(CALIBRATION_DATE, ImmutableList.of(ResourceLocator.of(BASE_DIR + QUOTES_FILE)));
	  private static readonly ImmutableMarketData MARKET_QUOTES = ImmutableMarketData.of(CALIBRATION_DATE, MAP_MQ);

	  private static readonly CalibrationMeasures CALIBRATION_MEASURES = CalibrationMeasures.PAR_SPREAD;
	  private static readonly RatesCurveCalibrator CALIBRATOR = RatesCurveCalibrator.of(1e-9, 1e-9, 100, CALIBRATION_MEASURES);
	  private static readonly RatesProvider MULTICURVE = CALIBRATOR.calibrate(CONFIGS, MARKET_QUOTES, REF_DATA);

	  private static readonly DiscountingSwapProductPricer SWAP_PRICER = DiscountingSwapProductPricer.DEFAULT;

	  private static readonly DoubleArray MONEYNESS = DoubleArray.of(-0.0100, -0.0050, -0.0025, 0.0000, 0.0025, 0.0050, 0.0100, 0.0200);
	  private static readonly IList<Period> EXPIRIES = new List<Period>();
	  private static readonly IList<Tenor> TENORS = new List<Tenor>();
	  static SabrSwaptionCalibratorCubeBlackExtremeDataTest()
	  {
		EXPIRIES.Add(Period.ofMonths(1));
		EXPIRIES.Add(Period.ofMonths(3));
		EXPIRIES.Add(Period.ofMonths(6));
		EXPIRIES.Add(Period.ofYears(1));
		EXPIRIES.Add(Period.ofYears(2));
		EXPIRIES.Add(Period.ofYears(5));
		TENORS.Add(Tenor.TENOR_1Y);
		TENORS.Add(Tenor.TENOR_2Y);
		TENORS.Add(Tenor.TENOR_5Y);
		TENORS.Add(Tenor.TENOR_10Y);
	  }

	  private static readonly double[][][] DATA_LOGNORMAL_SPARSE = new double[][][]
	  {
		  new double[][]
		  {
			  new double[] {Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN},
			  new double[] {Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN},
			  new double[] {Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN},
			  new double[] {Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN},
			  new double[] {Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN},
			  new double[] {Double.NaN, Double.NaN, Double.NaN, Double.NaN, 1.4019, 1.0985, 0.8441, 0.6468}
		  },
		  new double[][]
		  {
			  new double[] {Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN},
			  new double[] {Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN},
			  new double[] {Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN},
			  new double[] {Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN},
			  new double[] {Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN},
			  new double[] {Double.NaN, 1.969, Double.NaN, 1.2894, 1.0152, 0.8718, 0.7142, 0.5712}
		  },
		  new double[][]
		  {
			  new double[] {Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN},
			  new double[] {Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN},
			  new double[] {Double.NaN, Double.NaN, Double.NaN, Double.NaN, 6.2852, 4.2545, 2.5079, 1.8524},
			  new double[] {Double.NaN, Double.NaN, Double.NaN, Double.NaN, 4.0593, 2.2422, 1.5647, 1.2192},
			  new double[] {Double.NaN, Double.NaN, 3.1613, 2.8434, 1.5934, 1.2644, 0.9856, 0.7885},
			  new double[] {2.0291, 1.2216, 0.9355, 0.7936, 0.7047, 0.643, 0.5625, 0.4793}
		  },
		  new double[][]
		  {
			  new double[] {Double.NaN, 6.6443, 1.8125, 1.3324, 1.1296, 1.0273, 0.9392, 0.8961},
			  new double[] {Double.NaN, 4.3337, 1.6752, 1.2496, 1.0687, 0.9785, 0.9032, 0.8712},
			  new double[] {Double.NaN, 3.2343, 1.5785, 1.2105, 1.0415, 0.9499, 0.8629, 0.8123},
			  new double[] {Double.NaN, 2.3148, 1.3951, 1.1006, 0.9474, 0.8544, 0.751, 0.6684},
			  new double[] {Double.NaN, 1.5092, 1.1069, 0.9209, 0.8095, 0.7348, 0.6416, 0.5518},
			  new double[] {2.1248, 0.8566, 0.734, 0.6542, 0.5972, 0.5541, 0.4929, 0.4218}
		  }
	  };
	  private static readonly TenorRawOptionData DATA_SPARSE = SabrSwaptionCalibratorSmileTestUtils.rawData(TENORS, EXPIRIES, ValueType.SIMPLE_MONEYNESS, MONEYNESS, ValueType.BLACK_VOLATILITY, DATA_LOGNORMAL_SPARSE);
	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly SwaptionVolatilitiesName NAME_SABR = SwaptionVolatilitiesName.of("Calibrated-SABR");
	  private static readonly SabrSwaptionDefinition DEFINITION = SabrSwaptionDefinition.of(NAME_SABR, EUR_FIXED_1Y_EURIBOR_6M, DAY_COUNT, INTERPOLATOR_2D);

	  private const double TOLERANCE_PRICE_CALIBRATION_LS = 1.0E-3; // Calibration Least Square; result not exact

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void log_normal_cube()
	  public virtual void log_normal_cube()
	  {
		double beta = 0.50;
		Surface betaSurface = ConstantSurface.of("Beta", beta).withMetadata(DefaultSurfaceMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).zValueType(ValueType.SABR_BETA).surfaceName("Beta").build());
		double shift = 0.0300;
		Surface shiftSurface = ConstantSurface.of("Shift", shift).withMetadata(DefaultSurfaceMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).surfaceName("Shift").build());
		SabrParametersSwaptionVolatilities calibrated = SABR_CALIBRATION.calibrateWithFixedBetaAndShift(DEFINITION, CALIBRATION_TIME, DATA_SPARSE, MULTICURVE, betaSurface, shiftSurface);

		for (int looptenor = 0; looptenor < TENORS.Count; looptenor++)
		{
		  double tenor = TENORS[looptenor].get(ChronoUnit.YEARS);
		  for (int loopexpiry = 0; loopexpiry < EXPIRIES.Count; loopexpiry++)
		  {
			LocalDate expiry = EUR_FIXED_1Y_EURIBOR_6M.FloatingLeg.StartDateBusinessDayAdjustment.adjust(CALIBRATION_DATE.plus(EXPIRIES[loopexpiry]), REF_DATA);
			LocalDate effectiveDate = EUR_FIXED_1Y_EURIBOR_6M.calculateSpotDateFromTradeDate(expiry, REF_DATA);
			LocalDate endDate = effectiveDate.plus(TENORS[looptenor]);
			SwapTrade swap = EUR_FIXED_1Y_EURIBOR_6M.toTrade(CALIBRATION_DATE, effectiveDate, endDate, BuySell.BUY, 1.0, 0.0);
			double parRate = SWAP_PRICER.parRate(swap.resolve(REF_DATA).Product, MULTICURVE);
			ZonedDateTime expiryDateTime = expiry.atTime(11, 0).atZone(ZoneId.of("Europe/Berlin"));
			double time = calibrated.relativeTime(expiryDateTime);
			for (int loopmoney = 0; loopmoney < MONEYNESS.size(); loopmoney++)
			{
			  if (!double.IsNaN(DATA_LOGNORMAL_SPARSE[looptenor][loopexpiry][loopmoney]))
			  {
				double strike = parRate + MONEYNESS.get(loopmoney);
				double volBlack = calibrated.volatility(expiryDateTime, tenor, strike, parRate);
				double priceComputed = BlackFormulaRepository.price(parRate + shift, parRate + MONEYNESS.get(loopmoney) + shift, time, volBlack, true);
				double priceLogNormal = BlackFormulaRepository.price(parRate, parRate + MONEYNESS.get(loopmoney), time, DATA_LOGNORMAL_SPARSE[looptenor][loopexpiry][loopmoney], true);
				if (strike > 0.0025)
				{ // Test only for strikes above 25bps
				  assertEquals(priceComputed, priceLogNormal, TOLERANCE_PRICE_CALIBRATION_LS);
				}
			  }
			}
		  }
		}
	  }

	}

}