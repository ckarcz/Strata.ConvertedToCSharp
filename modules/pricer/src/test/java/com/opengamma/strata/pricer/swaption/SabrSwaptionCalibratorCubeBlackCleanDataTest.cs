using System;
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
//	import static com.opengamma.strata.pricer.swaption.SwaptionCubeData.ATM_LOGNORMAL_SIMPLE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swaption.SwaptionCubeData.DATA_LOGNORMAL_ATM_SIMPLE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swaption.SwaptionCubeData.DAY_COUNT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swaption.SwaptionCubeData.EXPIRIES_SIMPLE_2;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swaption.SwaptionCubeData.TENORS_SIMPLE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using QuotesCsvLoader = com.opengamma.strata.loader.csv.QuotesCsvLoader;
	using RatesCalibrationCsvLoader = com.opengamma.strata.loader.csv.RatesCalibrationCsvLoader;
	using ValueType = com.opengamma.strata.market.ValueType;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ConstantSurface = com.opengamma.strata.market.surface.ConstantSurface;
	using DefaultSurfaceMetadata = com.opengamma.strata.market.surface.DefaultSurfaceMetadata;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;
	using CalibrationMeasures = com.opengamma.strata.pricer.curve.CalibrationMeasures;
	using RatesCurveCalibrator = com.opengamma.strata.pricer.curve.RatesCurveCalibrator;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;
	using TenorRawOptionData = com.opengamma.strata.pricer.option.TenorRawOptionData;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using PutCall = com.opengamma.strata.product.common.PutCall;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;

	/// <summary>
	/// Tests <seealso cref="SabrSwaptionCalibrator"/> for a cube. Reduced and simplify data set to avoid very slow test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrSwaptionCalibratorCubeBlackCleanDataTest
	public class SabrSwaptionCalibratorCubeBlackCleanDataTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private static readonly LocalDate CALIBRATION_DATE = LocalDate.of(2016, 2, 29);
	  private static readonly ZonedDateTime CALIBRATION_TIME = CALIBRATION_DATE.atTime(10, 0).atZone(ZoneId.of("Europe/Berlin"));

	  private static readonly SabrSwaptionCalibrator SABR_CALIBRATION = SabrSwaptionCalibrator.DEFAULT;

	  private const string BASE_DIR = "src/test/resources/";
	  private const string GROUPS_FILE = "curve-config/EUR-DSCONOIS-E3BS-E6IRS-group.csv";
	  private const string SETTINGS_FILE = "curve-config/EUR-DSCONOIS-E3BS-E6IRS-settings.csv";
	  private const string NODES_FILE = "curve-config/EUR-DSCONOIS-E3BS-E6IRS-nodes.csv";
	  private const string QUOTES_FILE = "quotes/quotes-simplified-eur.csv";
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

	  static SabrSwaptionCalibratorCubeBlackCleanDataTest()
	  {
		EXPIRIES.Add(Period.ofMonths(1));
		EXPIRIES.Add(Period.ofMonths(6));
		EXPIRIES.Add(Period.ofYears(1));
		TENORS.Add(Tenor.TENOR_1Y);
		TENORS.Add(Tenor.TENOR_2Y);
	  }

	  private static readonly double[][][] DATA_LOGNORMAL = new double[][][]
	  {
		  new double[][]
		  {
			  new double[] {0.60, 0.58, 0.565, 0.555, 0.55, 0.545, 0.545, 0.55},
			  new double[] {0.60, 0.58, 0.565, 0.555, 0.55, 0.545, 0.545, 0.55},
			  new double[] {0.60, 0.58, 0.565, 0.555, 0.55, 0.545, 0.545, 0.55}
		  },
		  new double[][]
		  {
			  new double[] {0.60, 0.58, 0.565, 0.555, 0.55, 0.545, 0.545, 0.55},
			  new double[] {0.60, 0.58, 0.565, 0.555, 0.55, 0.545, 0.545, 0.55},
			  new double[] {0.60, 0.58, 0.565, 0.555, 0.55, 0.545, 0.545, 0.55}
		  }
	  };
	  private static readonly TenorRawOptionData DATA_SPARSE = SabrSwaptionCalibratorSmileTestUtils.rawData(TENORS, EXPIRIES, ValueType.SIMPLE_MONEYNESS, MONEYNESS, ValueType.BLACK_VOLATILITY, DATA_LOGNORMAL);
	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly SwaptionVolatilitiesName NAME_SABR = SwaptionVolatilitiesName.of("Calibrated-SABR");
	  private static readonly SabrSwaptionDefinition DEFINITION = SabrSwaptionDefinition.of(NAME_SABR, EUR_FIXED_1Y_EURIBOR_6M, DAY_COUNT, INTERPOLATOR_2D);

	  private const double TOLERANCE_PRICE_CALIBRATION_LS = 1.0E-3; // Calibration Least Square; result not exact
	  private const double TOLERANCE_PRICE_CALIBRATION_ROOT = 1.0E-6; // Calibration root finding
	  private const double TOLERANCE_PARAM_SENSITIVITY = 4.0E-2;
	  private const double TOLERANCE_EXPIRY = 1.0E-6;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void log_normal_cube()
	  public virtual void log_normal_cube()
	  {
		double beta = 0.50;
		Surface betaSurface = ConstantSurface.of("Beta", beta).withMetadata(DefaultSurfaceMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).zValueType(ValueType.SABR_BETA).surfaceName("Beta").build());
		double shift = 0.0000;
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
			  if (!double.IsNaN(DATA_LOGNORMAL[looptenor][loopexpiry][loopmoney]))
			  {
				double strike = parRate + MONEYNESS.get(loopmoney);
				double volBlack = calibrated.volatility(expiryDateTime, tenor, strike, parRate);
				double priceComputed = BlackFormulaRepository.price(parRate + shift, parRate + MONEYNESS.get(loopmoney) + shift, time, volBlack, true);
				double priceLogNormal = BlackFormulaRepository.price(parRate, parRate + MONEYNESS.get(loopmoney), time, DATA_LOGNORMAL[looptenor][loopexpiry][loopmoney], true);
				assertEquals(priceComputed, priceLogNormal, TOLERANCE_PRICE_CALIBRATION_LS);
			  }
			}
		  }
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(enabled = true) public void log_normal_atm()
	  public virtual void log_normal_atm()
	  {
		double beta = 0.50;
		Surface betaSurface = ConstantSurface.of("Beta", beta).withMetadata(DefaultSurfaceMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).zValueType(ValueType.SABR_BETA).surfaceName("Beta").build());
		double shift = 0.0000;
		Surface shiftSurface = ConstantSurface.of("Shift", shift).withMetadata(DefaultSurfaceMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).surfaceName("Shift").build());
		SabrParametersSwaptionVolatilities calibratedSmile = SABR_CALIBRATION.calibrateWithFixedBetaAndShift(DEFINITION, CALIBRATION_TIME, DATA_SPARSE, MULTICURVE, betaSurface, shiftSurface);

		SabrParametersSwaptionVolatilities calibratedAtm = SABR_CALIBRATION.calibrateAlphaWithAtm(NAME_SABR, calibratedSmile, MULTICURVE, ATM_LOGNORMAL_SIMPLE, TENORS_SIMPLE, EXPIRIES_SIMPLE_2, INTERPOLATOR_2D);
		int nbExp = EXPIRIES_SIMPLE_2.size();
		int nbTenor = TENORS_SIMPLE.size();
		for (int loopexpiry = 0; loopexpiry < nbExp; loopexpiry++)
		{
		  for (int looptenor = 0; looptenor < nbTenor; looptenor++)
		  {
			double tenor = TENORS_SIMPLE.get(looptenor).get(ChronoUnit.YEARS);
			LocalDate expiry = EUR_FIXED_1Y_EURIBOR_6M.FloatingLeg.StartDateBusinessDayAdjustment.adjust(CALIBRATION_DATE.plus(EXPIRIES_SIMPLE_2.get(loopexpiry)), REF_DATA);
			LocalDate effectiveDate = EUR_FIXED_1Y_EURIBOR_6M.calculateSpotDateFromTradeDate(expiry, REF_DATA);
			LocalDate endDate = effectiveDate.plus(TENORS_SIMPLE.get(looptenor));
			SwapTrade swap = EUR_FIXED_1Y_EURIBOR_6M.toTrade(CALIBRATION_DATE, effectiveDate, endDate, BuySell.BUY, 1.0, 0.0);
			double parRate = SWAP_PRICER.parRate(swap.resolve(REF_DATA).Product, MULTICURVE);
			ZonedDateTime expiryDateTime = expiry.atTime(11, 0).atZone(ZoneId.of("Europe/Berlin"));
			double time = calibratedAtm.relativeTime(expiryDateTime);
			double volBlack = calibratedAtm.volatility(expiryDateTime, tenor, parRate, parRate);
			double priceComputed = calibratedAtm.price(time, tenor, PutCall.CALL, parRate, parRate, volBlack);
			double priceBlack = BlackFormulaRepository.price(parRate, parRate, time, DATA_LOGNORMAL_ATM_SIMPLE[looptenor + loopexpiry * nbTenor], true);
			assertEquals(priceComputed, priceBlack, TOLERANCE_PRICE_CALIBRATION_ROOT);
		  }
		}
	  }

	  /// <summary>
	  /// Check that the sensitivities of parameters with respect to data is stored in the metadata.
	  /// Compare the sensitivities to a finite difference approximation.
	  /// This test is relatively slow as it calibrates the full surface multiple times.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void log_normal_cube_sensitivity()
	  public virtual void log_normal_cube_sensitivity()
	  {
		double beta = 1.0;
		Surface betaSurface = ConstantSurface.of("Beta", beta).withMetadata(DefaultSurfaceMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).zValueType(ValueType.SABR_BETA).surfaceName("Beta").build());
		double shift = 0.0000;
		Surface shiftSurface = ConstantSurface.of("Shift", shift).withMetadata(DefaultSurfaceMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).surfaceName("Shift").build());
		SabrParametersSwaptionVolatilities calibrated = SABR_CALIBRATION.calibrateWithFixedBetaAndShift(DEFINITION, CALIBRATION_TIME, DATA_SPARSE, MULTICURVE, betaSurface, shiftSurface);
		double fdShift = 1.0E-5;

		SurfaceMetadata alphaMetadata = calibrated.Parameters.AlphaSurface.Metadata;
		Optional<IList<ParameterMetadata>> alphaParameterMetadataOption = alphaMetadata.ParameterMetadata;
		assertTrue(alphaParameterMetadataOption.Present);
		IList<ParameterMetadata> alphaParameterMetadata = alphaParameterMetadataOption.get();
		IList<DoubleArray> alphaJacobian = calibrated.DataSensitivityAlpha.get();
		SurfaceMetadata rhoMetadata = calibrated.Parameters.RhoSurface.Metadata;
		Optional<IList<ParameterMetadata>> rhoParameterMetadataOption = rhoMetadata.ParameterMetadata;
		assertTrue(rhoParameterMetadataOption.Present);
		IList<ParameterMetadata> rhoParameterMetadata = rhoParameterMetadataOption.get();
		IList<DoubleArray> rhoJacobian = calibrated.DataSensitivityRho.get();
		SurfaceMetadata nuMetadata = calibrated.Parameters.NuSurface.Metadata;
		Optional<IList<ParameterMetadata>> nuParameterMetadataOption = nuMetadata.ParameterMetadata;
		assertTrue(nuParameterMetadataOption.Present);
		IList<ParameterMetadata> nuParameterMetadata = nuParameterMetadataOption.get();
		IList<DoubleArray> nuJacobian = calibrated.DataSensitivityNu.get();

		int surfacePointIndex = 0;
		for (int loopexpiry = 0; loopexpiry < EXPIRIES.Count; loopexpiry++)
		{
		  for (int looptenor = 0; looptenor < TENORS.Count; looptenor++)
		  {
			Tenor tenor = TENORS[looptenor];
			double tenorYears = tenor.get(ChronoUnit.YEARS);
			LocalDate expiry = EUR_FIXED_1Y_EURIBOR_6M.FloatingLeg.StartDateBusinessDayAdjustment.adjust(CALIBRATION_DATE.plus(EXPIRIES[loopexpiry]), REF_DATA);
			ZonedDateTime expiryDateTime = expiry.atTime(11, 0).atZone(ZoneId.of("Europe/Berlin"));
			double time = calibrated.relativeTime(expiryDateTime);
			Pair<DoubleArray, DoubleArray> ds = DATA_SPARSE.getData(tenor).availableSmileAtExpiry(EXPIRIES[loopexpiry]);
			if (!ds.First.Empty)
			{
			  int availableDataIndex = 0;

			  ParameterMetadata alphaPM = alphaParameterMetadata[surfacePointIndex];
			  assertTrue(alphaPM is SwaptionSurfaceExpiryTenorParameterMetadata);
			  SwaptionSurfaceExpiryTenorParameterMetadata pmAlphaSabr = (SwaptionSurfaceExpiryTenorParameterMetadata) alphaPM;
			  assertEquals(tenorYears, pmAlphaSabr.Tenor);
			  assertEquals(time, pmAlphaSabr.YearFraction, TOLERANCE_EXPIRY);
			  DoubleArray alphaSensitivityToData = alphaJacobian[surfacePointIndex];
			  ParameterMetadata rhoPM = rhoParameterMetadata[surfacePointIndex];
			  assertTrue(rhoPM is SwaptionSurfaceExpiryTenorParameterMetadata);
			  SwaptionSurfaceExpiryTenorParameterMetadata pmRhoSabr = (SwaptionSurfaceExpiryTenorParameterMetadata) rhoPM;
			  assertEquals(tenorYears, pmRhoSabr.Tenor);
			  assertEquals(time, pmRhoSabr.YearFraction, TOLERANCE_EXPIRY);
			  DoubleArray rhoSensitivityToData = rhoJacobian[surfacePointIndex];
			  ParameterMetadata nuPM = nuParameterMetadata[surfacePointIndex];
			  assertTrue(nuPM is SwaptionSurfaceExpiryTenorParameterMetadata);
			  SwaptionSurfaceExpiryTenorParameterMetadata pmNuSabr = (SwaptionSurfaceExpiryTenorParameterMetadata) nuPM;
			  assertEquals(tenorYears, pmNuSabr.Tenor);
			  assertEquals(time, pmNuSabr.YearFraction, TOLERANCE_EXPIRY);
			  DoubleArray nuSensitivityToData = nuJacobian[surfacePointIndex];

			  for (int loopmoney = 0; loopmoney < MONEYNESS.size(); loopmoney++)
			  {
				if (!double.IsNaN(DATA_LOGNORMAL[looptenor][loopexpiry][loopmoney]))
				{
				  double[] alphaShifted = new double[2];
				  double[] rhoShifted = new double[2];
				  double[] nuShifted = new double[2];
				  for (int loopsign = 0; loopsign < 2; loopsign++)
				  {
					TenorRawOptionData dataShifted = SabrSwaptionCalibratorSmileTestUtils.rawDataShiftPoint(TENORS, EXPIRIES, ValueType.SIMPLE_MONEYNESS, MONEYNESS, ValueType.BLACK_VOLATILITY, DATA_LOGNORMAL, looptenor, loopexpiry, loopmoney, (2 * loopsign - 1) * fdShift);
					SabrParametersSwaptionVolatilities calibratedShifted = SABR_CALIBRATION.calibrateWithFixedBetaAndShift(DEFINITION, CALIBRATION_TIME, dataShifted, MULTICURVE, betaSurface, shiftSurface);
					alphaShifted[loopsign] = calibratedShifted.Parameters.AlphaSurface.zValue(time, tenorYears);
					rhoShifted[loopsign] = calibratedShifted.Parameters.RhoSurface.zValue(time, tenorYears);
					nuShifted[loopsign] = calibratedShifted.Parameters.NuSurface.zValue(time, tenorYears);
				  }
				  double alphaSensitivityComputed = alphaSensitivityToData.get(availableDataIndex);
				  double alphaSensitivityExpected = (alphaShifted[1] - alphaShifted[0]) / (2 * fdShift);
				  checkAcceptable(alphaSensitivityComputed, alphaSensitivityExpected, TOLERANCE_PARAM_SENSITIVITY, "Alpha: " + looptenor + " / " + loopexpiry + " / " + loopmoney);
				  double rhoSensitivityComputed = rhoSensitivityToData.get(availableDataIndex);
				  double rhoSensitivityExpected = (rhoShifted[1] - rhoShifted[0]) / (2 * fdShift);
				  checkAcceptable(rhoSensitivityComputed, rhoSensitivityExpected, TOLERANCE_PARAM_SENSITIVITY, "Rho: " + looptenor + " / " + loopexpiry + " / " + loopmoney);
				  double nuSensitivityComputed = nuSensitivityToData.get(availableDataIndex);
				  double nuSensitivityExpected = (nuShifted[1] - nuShifted[0]) / (2 * fdShift);
				  checkAcceptable(nuSensitivityComputed, nuSensitivityExpected, TOLERANCE_PARAM_SENSITIVITY, "Nu: " + looptenor + " / " + loopexpiry + " / " + loopmoney);
				  availableDataIndex++;
				}
			  }
			  surfacePointIndex++;
			}
		  }
		}
	  }

	  private static void checkAcceptable(double computed, double actual, double tolerance, string msg)
	  {
		assertTrue((Math.Abs(computed - actual) < tolerance) || (Math.Abs((computed - actual) / actual) < tolerance), msg);
	  }

	}

}