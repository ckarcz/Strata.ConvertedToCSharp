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
//	import static com.opengamma.strata.pricer.swaption.SwaptionCubeData.ATM_NORMAL_SIMPLE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swaption.SwaptionCubeData.DATA_ARRAY_SPARSE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swaption.SwaptionCubeData.DATA_DATE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swaption.SwaptionCubeData.DATA_NORMAL_ATM_SIMPLE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swaption.SwaptionCubeData.DATA_NORMAL_SIMPLE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swaption.SwaptionCubeData.DATA_TIME;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swaption.SwaptionCubeData.DAY_COUNT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swaption.SwaptionCubeData.EXPIRIES_SIMPLE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swaption.SwaptionCubeData.EXPIRIES_SIMPLE_2;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swaption.SwaptionCubeData.MONEYNESS;
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
//ORIGINAL LINE: @Test public class SabrSwaptionCalibratorCubeNormalSimpleDataTest
	public class SabrSwaptionCalibratorCubeNormalSimpleDataTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private static readonly LocalDate CALIBRATION_DATE = DATA_DATE;
	  private static readonly ZonedDateTime CALIBRATION_TIME = DATA_TIME;

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

	  private static readonly TenorRawOptionData DATA_SIMPLE = SabrSwaptionCalibratorSmileTestUtils.rawData(TENORS_SIMPLE, EXPIRIES_SIMPLE, ValueType.SIMPLE_MONEYNESS, MONEYNESS, ValueType.NORMAL_VOLATILITY, DATA_NORMAL_SIMPLE);
	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly SwaptionVolatilitiesName NAME_SABR = SwaptionVolatilitiesName.of("Calibrated-SABR");
	  private static readonly SabrSwaptionDefinition DEFINITION = SabrSwaptionDefinition.of(NAME_SABR, EUR_FIXED_1Y_EURIBOR_6M, DAY_COUNT, INTERPOLATOR_2D);

	  private const double TOLERANCE_PRICE_CALIBRATION_LS = 5.0E-4; // Calibration Least Square; result not exact
	  private const double TOLERANCE_PRICE_CALIBRATION_ROOT = 1.0E-6; // Calibration root finding
	  private const double TOLERANCE_PARAM_SENSITIVITY = 3.0E-2;
	  private const double TOLERANCE_PARAM_SENSITIVITY_NU = 9.0E-2;
	  private const double TOLERANCE_EXPIRY = 1.0E-6;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void normal_cube()
	  public virtual void normal_cube()
	  {
		double beta = 0.50;
		Surface betaSurface = ConstantSurface.of("Beta", beta).withMetadata(DefaultSurfaceMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).zValueType(ValueType.SABR_BETA).surfaceName("Beta").build());
		double shift = 0.0300;
		Surface shiftSurface = ConstantSurface.of("Shift", shift).withMetadata(DefaultSurfaceMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).surfaceName("Shift").build());
		SabrParametersSwaptionVolatilities calibrated = SABR_CALIBRATION.calibrateWithFixedBetaAndShift(DEFINITION, CALIBRATION_TIME, DATA_SIMPLE, MULTICURVE, betaSurface, shiftSurface);

		for (int looptenor = 0; looptenor < TENORS_SIMPLE.size(); looptenor++)
		{
		  double tenor = TENORS_SIMPLE.get(looptenor).get(ChronoUnit.YEARS);
		  for (int loopexpiry = 0; loopexpiry < EXPIRIES_SIMPLE.size(); loopexpiry++)
		  {
			LocalDate expiry = EUR_FIXED_1Y_EURIBOR_6M.FloatingLeg.StartDateBusinessDayAdjustment.adjust(CALIBRATION_DATE.plus(EXPIRIES_SIMPLE.get(loopexpiry)), REF_DATA);
			LocalDate effectiveDate = EUR_FIXED_1Y_EURIBOR_6M.calculateSpotDateFromTradeDate(expiry, REF_DATA);
			LocalDate endDate = effectiveDate.plus(TENORS_SIMPLE.get(looptenor));
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

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unused") @Test public void normal_atm()
	  public virtual void normal_atm()
	  {
		double beta = 0.50;
		Surface betaSurface = ConstantSurface.of("Beta", beta).withMetadata(DefaultSurfaceMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).zValueType(ValueType.SABR_BETA).surfaceName("Beta").build());
		double shift = 0.0300;
		Surface shiftSurface = ConstantSurface.of("Shift", shift).withMetadata(DefaultSurfaceMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).surfaceName("Shift").build());
		SabrParametersSwaptionVolatilities calibratedSmile = SABR_CALIBRATION.calibrateWithFixedBetaAndShift(DEFINITION, CALIBRATION_TIME, DATA_SIMPLE, MULTICURVE, betaSurface, shiftSurface);
		SabrParametersSwaptionVolatilities calibratedAtm = SABR_CALIBRATION.calibrateAlphaWithAtm(NAME_SABR, calibratedSmile, MULTICURVE, ATM_NORMAL_SIMPLE, TENORS_SIMPLE, EXPIRIES_SIMPLE_2, INTERPOLATOR_2D);
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
			double priceComputed = BlackFormulaRepository.price(parRate + shift, parRate + shift, time, volBlack, true);
			double priceNormal = NormalFormulaRepository.price(parRate, parRate, time, DATA_NORMAL_ATM_SIMPLE[looptenor + loopexpiry * nbTenor], PutCall.CALL);
			assertEquals(priceComputed, priceNormal, TOLERANCE_PRICE_CALIBRATION_ROOT);
		  }
		}
	  }

	  /// <summary>
	  /// Check that the sensitivities of parameters with respect to data is stored in the metadata.
	  /// Compare the sensitivities to a finite difference approximation.
	  /// This test is relatively slow as it calibrates the full surface multiple times.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void normal_cube_sensitivity()
	  public virtual void normal_cube_sensitivity()
	  {
		double beta = 0.50;
		Surface betaSurface = ConstantSurface.of("Beta", beta).withMetadata(DefaultSurfaceMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).zValueType(ValueType.SABR_BETA).surfaceName("Beta").build());
		double shift = 0.0300;
		Surface shiftSurface = ConstantSurface.of("Shift", shift).withMetadata(DefaultSurfaceMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).surfaceName("Shift").build());
		SabrParametersSwaptionVolatilities calibrated = SABR_CALIBRATION.calibrateWithFixedBetaAndShift(DEFINITION, CALIBRATION_TIME, DATA_SIMPLE, MULTICURVE, betaSurface, shiftSurface);
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
		for (int loopexpiry = 0; loopexpiry < EXPIRIES_SIMPLE.size(); loopexpiry++)
		{
		  for (int looptenor = 0; looptenor < TENORS_SIMPLE.size(); looptenor++)
		  {
			Tenor tenor = TENORS_SIMPLE.get(looptenor);
			double tenorYear = tenor.get(ChronoUnit.YEARS);
			LocalDate expiry = EUR_FIXED_1Y_EURIBOR_6M.FloatingLeg.StartDateBusinessDayAdjustment.adjust(CALIBRATION_DATE.plus(EXPIRIES_SIMPLE.get(loopexpiry)), REF_DATA);
			ZonedDateTime expiryDateTime = expiry.atTime(11, 0).atZone(ZoneId.of("Europe/Berlin"));
			double time = calibrated.relativeTime(expiryDateTime);
			Pair<DoubleArray, DoubleArray> ds = DATA_SIMPLE.getData(tenor).availableSmileAtExpiry(EXPIRIES_SIMPLE.get(loopexpiry));
			if (!ds.First.Empty)
			{
			  int availableDataIndex = 0;

			  ParameterMetadata alphaPM = alphaParameterMetadata[surfacePointIndex];
			  assertTrue(alphaPM is SwaptionSurfaceExpiryTenorParameterMetadata);
			  SwaptionSurfaceExpiryTenorParameterMetadata pmAlphaSabr = (SwaptionSurfaceExpiryTenorParameterMetadata) alphaPM;
			  assertEquals(tenorYear, pmAlphaSabr.Tenor);
			  assertEquals(time, pmAlphaSabr.YearFraction, TOLERANCE_EXPIRY);
			  DoubleArray alphaSensitivityToData = alphaJacobian[surfacePointIndex];
			  ParameterMetadata rhoPM = rhoParameterMetadata[surfacePointIndex];
			  assertTrue(rhoPM is SwaptionSurfaceExpiryTenorParameterMetadata);
			  SwaptionSurfaceExpiryTenorParameterMetadata pmRhoSabr = (SwaptionSurfaceExpiryTenorParameterMetadata) rhoPM;
			  assertEquals(tenorYear, pmRhoSabr.Tenor);
			  assertEquals(time, pmRhoSabr.YearFraction, TOLERANCE_EXPIRY);
			  DoubleArray rhoSensitivityToData = rhoJacobian[surfacePointIndex];
			  ParameterMetadata nuPM = nuParameterMetadata[surfacePointIndex];
			  assertTrue(nuPM is SwaptionSurfaceExpiryTenorParameterMetadata);
			  SwaptionSurfaceExpiryTenorParameterMetadata pmNuSabr = (SwaptionSurfaceExpiryTenorParameterMetadata) nuPM;
			  assertEquals(tenorYear, pmNuSabr.Tenor);
			  assertEquals(time, pmNuSabr.YearFraction, TOLERANCE_EXPIRY);
			  DoubleArray nuSensitivityToData = nuJacobian[surfacePointIndex];

			  for (int loopmoney = 0; loopmoney < MONEYNESS.size(); loopmoney++)
			  {
				if (!double.IsNaN(DATA_NORMAL_SIMPLE[looptenor][loopexpiry][loopmoney]))
				{
				  double[] alphaShifted = new double[2];
				  double[] rhoShifted = new double[2];
				  double[] nuShifted = new double[2];
				  for (int loopsign = 0; loopsign < 2; loopsign++)
				  {
					TenorRawOptionData dataShifted = SabrSwaptionCalibratorSmileTestUtils.rawDataShiftPoint(TENORS_SIMPLE, EXPIRIES_SIMPLE, ValueType.SIMPLE_MONEYNESS, MONEYNESS, ValueType.NORMAL_VOLATILITY, DATA_NORMAL_SIMPLE, looptenor, loopexpiry, loopmoney, (2 * loopsign - 1) * fdShift);
					SabrParametersSwaptionVolatilities calibratedShifted = SABR_CALIBRATION.calibrateWithFixedBetaAndShift(DEFINITION, CALIBRATION_TIME, dataShifted, MULTICURVE, betaSurface, shiftSurface);
					alphaShifted[loopsign] = calibratedShifted.Parameters.AlphaSurface.zValue(time, tenorYear);
					rhoShifted[loopsign] = calibratedShifted.Parameters.RhoSurface.zValue(time, tenorYear);
					nuShifted[loopsign] = calibratedShifted.Parameters.NuSurface.zValue(time, tenorYear);
				  }
				  double alphaSensitivityComputed = alphaSensitivityToData.get(availableDataIndex);
				  double alphaSensitivityExpected = (alphaShifted[1] - alphaShifted[0]) / (2 * fdShift);
				  SabrSwaptionCalibratorSmileTestUtils.checkAcceptable(alphaSensitivityComputed, alphaSensitivityExpected, TOLERANCE_PARAM_SENSITIVITY, "Alpha: " + looptenor + " / " + loopexpiry + " / " + loopmoney);
				  double rhoSensitivityComputed = rhoSensitivityToData.get(availableDataIndex);
				  double rhoSensitivityExpected = (rhoShifted[1] - rhoShifted[0]) / (2 * fdShift);
				  SabrSwaptionCalibratorSmileTestUtils.checkAcceptable(rhoSensitivityComputed, rhoSensitivityExpected, TOLERANCE_PARAM_SENSITIVITY, "Rho: " + looptenor + " / " + loopexpiry + " / " + loopmoney);
				  double nuSensitivityComputed = nuSensitivityToData.get(availableDataIndex);
				  double nuSensitivityExpected = (nuShifted[1] - nuShifted[0]) / (2 * fdShift);
				  SabrSwaptionCalibratorSmileTestUtils.checkAcceptable(nuSensitivityComputed, nuSensitivityExpected, TOLERANCE_PARAM_SENSITIVITY_NU, "Nu: " + looptenor + " / " + loopexpiry + " / " + loopmoney);
				  availableDataIndex++;
				}
			  }
			  surfacePointIndex++;
			}
		  }
		}
	  }

	}

}