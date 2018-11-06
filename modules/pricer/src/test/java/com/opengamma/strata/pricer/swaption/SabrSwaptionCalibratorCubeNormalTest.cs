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
//	import static com.opengamma.strata.pricer.swaption.SwaptionCubeData.DATA_ARRAY_SPARSE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swaption.SwaptionCubeData.DAY_COUNT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swaption.SwaptionCubeData.EXPIRIES;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swaption.SwaptionCubeData.MONEYNESS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swaption.SwaptionCubeData.TENORS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
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
	using NormalFormulaRepository = com.opengamma.strata.pricer.impl.option.NormalFormulaRepository;
	using TenorRawOptionData = com.opengamma.strata.pricer.option.TenorRawOptionData;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using PutCall = com.opengamma.strata.product.common.PutCall;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;

	/// <summary>
	/// Tests <seealso cref="SabrSwaptionCalibrator"/> for a cube. Realistic dimension and data.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrSwaptionCalibratorCubeNormalTest
	public class SabrSwaptionCalibratorCubeNormalTest
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
	  private static readonly TenorRawOptionData DATA_SPARSE = SabrSwaptionCalibratorSmileTestUtils.rawData(TENORS, EXPIRIES, ValueType.SIMPLE_MONEYNESS, MONEYNESS, ValueType.NORMAL_VOLATILITY, DATA_ARRAY_SPARSE);
	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly SwaptionVolatilitiesName NAME_SABR = SwaptionVolatilitiesName.of("Calibrated-SABR");
	  private static readonly SabrSwaptionDefinition DEFINITION = SabrSwaptionDefinition.of(NAME_SABR, EUR_FIXED_1Y_EURIBOR_6M, DAY_COUNT, INTERPOLATOR_2D);

	  private const double TOLERANCE_PRICE_CALIBRATION_LS = 5.0E-4; // Calibration Least Square; result not exact

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void normal_cube()
	  public virtual void normal_cube()
	  {
		double beta = 0.50;
		Surface betaSurface = ConstantSurface.of("Beta", beta).withMetadata(DefaultSurfaceMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).zValueType(ValueType.SABR_BETA).surfaceName("Beta").build());
		double shift = 0.0300;
		Surface shiftSurface = ConstantSurface.of("Shift", shift).withMetadata(DefaultSurfaceMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).surfaceName("Shift").build());
		SabrParametersSwaptionVolatilities calibrated = SABR_CALIBRATION.calibrateWithFixedBetaAndShift(DEFINITION, CALIBRATION_TIME, DATA_SPARSE, MULTICURVE, betaSurface, shiftSurface);

		for (int looptenor = 0; looptenor < TENORS.size(); looptenor++)
		{
		  double tenor = TENORS.get(looptenor).get(ChronoUnit.YEARS);
		  for (int loopexpiry = 0; loopexpiry < EXPIRIES.size(); loopexpiry++)
		  {
			LocalDate expiry = EUR_FIXED_1Y_EURIBOR_6M.FloatingLeg.StartDateBusinessDayAdjustment.adjust(CALIBRATION_DATE.plus(EXPIRIES.get(loopexpiry)), REF_DATA);
			LocalDate effectiveDate = EUR_FIXED_1Y_EURIBOR_6M.calculateSpotDateFromTradeDate(expiry, REF_DATA);
			LocalDate endDate = effectiveDate.plus(TENORS.get(looptenor));
			SwapTrade swap = EUR_FIXED_1Y_EURIBOR_6M.toTrade(CALIBRATION_DATE, effectiveDate, endDate, BuySell.BUY, 1.0, 0.0);
			double parRate = SWAP_PRICER.parRate(swap.resolve(REF_DATA).Product, MULTICURVE);
			ZonedDateTime expiryDateTime = expiry.atTime(11, 0).atZone(ZoneId.of("Europe/Berlin"));
			double time = calibrated.relativeTime(expiryDateTime);
			for (int loopmoney = 0; loopmoney < MONEYNESS.size(); loopmoney++)
			{
			  if (!double.IsNaN(DATA_ARRAY_SPARSE[looptenor][loopexpiry][loopmoney]))
			  {
				double strike = parRate + MONEYNESS.get(loopmoney);
				double volBlack = calibrated.volatility(expiryDateTime, tenor, strike, parRate);
				double priceComputed = BlackFormulaRepository.price(parRate + shift, parRate + MONEYNESS.get(loopmoney) + shift, time, volBlack, true);
				double priceNormal = NormalFormulaRepository.price(parRate, parRate + MONEYNESS.get(loopmoney), time, DATA_ARRAY_SPARSE[looptenor][loopexpiry][loopmoney], PutCall.CALL);
				assertEquals(priceComputed, priceNormal, TOLERANCE_PRICE_CALIBRATION_LS);
			  }
			}
		  }
		}
	  }

	}

}