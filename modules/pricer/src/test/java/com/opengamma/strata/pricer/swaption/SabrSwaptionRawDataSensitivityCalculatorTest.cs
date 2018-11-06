using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swaption.SwaptionCubeData.DATA_ARRAY_FULL;
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
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_6M;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
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
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using ConstantSurface = com.opengamma.strata.market.surface.ConstantSurface;
	using DefaultSurfaceMetadata = com.opengamma.strata.market.surface.DefaultSurfaceMetadata;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;
	using SabrExtrapolationReplicationCmsLegPricer = com.opengamma.strata.pricer.cms.SabrExtrapolationReplicationCmsLegPricer;
	using SabrExtrapolationReplicationCmsPeriodPricer = com.opengamma.strata.pricer.cms.SabrExtrapolationReplicationCmsPeriodPricer;
	using CalibrationMeasures = com.opengamma.strata.pricer.curve.CalibrationMeasures;
	using RatesCurveCalibrator = com.opengamma.strata.pricer.curve.RatesCurveCalibrator;
	using TenorRawOptionData = com.opengamma.strata.pricer.option.TenorRawOptionData;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using CmsLeg = com.opengamma.strata.product.cms.CmsLeg;
	using ResolvedCmsLeg = com.opengamma.strata.product.cms.ResolvedCmsLeg;
	using SwapIndex = com.opengamma.strata.product.swap.SwapIndex;
	using SwapIndices = com.opengamma.strata.product.swap.SwapIndices;

	/// <summary>
	/// Tests <seealso cref="SabrSwaptionRawDataSensitivityCalculator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrSwaptionRawDataSensitivityCalculatorTest
	public class SabrSwaptionRawDataSensitivityCalculatorTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  /* =====     Data     ===== */
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

	  private static readonly TenorRawOptionData DATA_RAW_FULL = SabrSwaptionCalibratorSmileTestUtils.rawData(TENORS, EXPIRIES, ValueType.SIMPLE_MONEYNESS, MONEYNESS, ValueType.NORMAL_VOLATILITY, DATA_ARRAY_FULL);
	  private static readonly TenorRawOptionData DATA_RAW_SPARSE = SabrSwaptionCalibratorSmileTestUtils.rawData(TENORS, EXPIRIES, ValueType.SIMPLE_MONEYNESS, MONEYNESS, ValueType.NORMAL_VOLATILITY, DATA_ARRAY_SPARSE);
	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly SwaptionVolatilitiesName NAME_SABR = SwaptionVolatilitiesName.of("Calibrated-SABR");
	  private static readonly SabrSwaptionDefinition DEFINITION = SabrSwaptionDefinition.of(NAME_SABR, EUR_FIXED_1Y_EURIBOR_6M, DAY_COUNT, INTERPOLATOR_2D);

	  private const double BETA = 0.50;
	  private static readonly Surface BETA_SURFACE = ConstantSurface.of("Beta", BETA).withMetadata(DefaultSurfaceMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).zValueType(ValueType.SABR_BETA).surfaceName("Beta").build());
	  private const double SHIFT_SABR = 0.0300;
	  private static readonly Surface SHIFT_SABR_SURFACE = ConstantSurface.of("Shift", SHIFT_SABR).withMetadata(DefaultSurfaceMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).surfaceName("Shift").build());
	  private static readonly SabrParametersSwaptionVolatilities SABR_CALIBRATED_FULL = SABR_CALIBRATION.calibrateWithFixedBetaAndShift(DEFINITION, CALIBRATION_TIME, DATA_RAW_FULL, MULTICURVE, BETA_SURFACE, SHIFT_SABR_SURFACE);
	  private static readonly SabrParametersSwaptionVolatilities SABR_CALIBRATED_SPARSE = SABR_CALIBRATION.calibrateWithFixedBetaAndShift(DEFINITION, CALIBRATION_TIME, DATA_RAW_SPARSE, MULTICURVE, BETA_SURFACE, SHIFT_SABR_SURFACE);

	  /* =====     Trades     ===== */
	  private static readonly LocalDate START = LocalDate.of(2016, 3, 7);
	  private static readonly LocalDate END = LocalDate.of(2021, 3, 7);
	  private static readonly Frequency FREQUENCY = Frequency.P12M;
	  private static readonly BusinessDayAdjustment BUSS_ADJ_EUR = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, EUTA);
	  private static readonly PeriodicSchedule SCHEDULE_EUR = PeriodicSchedule.of(START, END, FREQUENCY, BUSS_ADJ_EUR, StubConvention.NONE, RollConventions.NONE);
	  private static readonly SwapIndex INDEX = SwapIndices.EUR_EURIBOR_1100_5Y;
	  private const double FLOOR_VALUE = 0.014;
	  private static readonly ValueSchedule FLOOR_STRIKE = ValueSchedule.of(FLOOR_VALUE);
	  private const double NOTIONAL_VALUE = 100_000_000;
	  private static readonly ValueSchedule NOTIONAL = ValueSchedule.of(NOTIONAL_VALUE);
	  private static readonly ResolvedCmsLeg FLOOR_LEG = CmsLeg.builder().floorSchedule(FLOOR_STRIKE).index(INDEX).notional(NOTIONAL).payReceive(RECEIVE).paymentSchedule(SCHEDULE_EUR).build().resolve(REF_DATA);

	  /* =====     Pricers     ===== */
	  private const double CUT_OFF_STRIKE = 0.10;
	  private const double MU = 2.50;
	  private static readonly SabrExtrapolationReplicationCmsPeriodPricer PERIOD_PRICER = SabrExtrapolationReplicationCmsPeriodPricer.of(CUT_OFF_STRIKE, MU);
	  private static readonly SabrExtrapolationReplicationCmsLegPricer LEG_PRICER = new SabrExtrapolationReplicationCmsLegPricer(PERIOD_PRICER);

	  private static readonly SabrSwaptionRawDataSensitivityCalculator RDSC = SabrSwaptionRawDataSensitivityCalculator.DEFAULT;

	  /// <summary>
	  /// Compare the AD version of the sensitivity to a finite difference parallel bump of the smile.
	  /// Full data set, no missing data.
	  /// </summary>
	  public virtual void presentValueSensitivityRawDataParallelSensitivity_full()
	  {
		presentValueSensitivityRawDataParallelSensitivity(SABR_CALIBRATED_FULL, DATA_RAW_FULL);
	  }

	  /// <summary>
	  /// Compare the AD version of the sensitivity to a finite difference parallel bump of the smile.
	  /// Sparse data set, some raw data are missing in some smiles.
	  /// </summary>
	  public virtual void presentValueSensitivityRawDataParallelSensitivity_sparse()
	  {
		presentValueSensitivityRawDataParallelSensitivity(SABR_CALIBRATED_SPARSE, DATA_RAW_SPARSE);
	  }

	  private void presentValueSensitivityRawDataParallelSensitivity(SabrParametersSwaptionVolatilities sabrCalibrated, TenorRawOptionData dataRaw)
	  {

		PointSensitivities points = LEG_PRICER.presentValueSensitivityModelParamsSabr(FLOOR_LEG, MULTICURVE, sabrCalibrated).build();
		CurrencyParameterSensitivities sabrParametersSurfaceSensitivities = sabrCalibrated.parameterSensitivity(points);
		CurrencyParameterSensitivity parallelSensitivitiesSurface = RDSC.parallelSensitivity(sabrParametersSurfaceSensitivities, sabrCalibrated);
		DoubleArray sensitivityArray = parallelSensitivitiesSurface.Sensitivity;
		double fdShift = 1.0E-6;
		int surfacePointIndex = 0;
		for (int loopexpiry = 0; loopexpiry < EXPIRIES.size(); loopexpiry++)
		{
		  for (int looptenor = 0; looptenor < TENORS.size(); looptenor++)
		  {
			Tenor tenor = TENORS.get(looptenor);
			Pair<DoubleArray, DoubleArray> ds = dataRaw.getData(tenor).availableSmileAtExpiry(EXPIRIES.get(loopexpiry));
			if (!ds.First.Empty)
			{
			  double[] pv = new double[2]; // pv with shift up and down
			  for (int loopsign = 0; loopsign < 2; loopsign++)
			  {
				TenorRawOptionData dataShifted = SabrSwaptionCalibratorSmileTestUtils.rawDataShiftSmile(TENORS, EXPIRIES, ValueType.SIMPLE_MONEYNESS, MONEYNESS, ValueType.NORMAL_VOLATILITY, DATA_ARRAY_FULL, looptenor, loopexpiry, (2 * loopsign - 1) * fdShift);
				SabrParametersSwaptionVolatilities calibratedShifted = SABR_CALIBRATION.calibrateWithFixedBetaAndShift(DEFINITION, CALIBRATION_TIME, dataShifted, MULTICURVE, BETA_SURFACE, SHIFT_SABR_SURFACE);
				pv[loopsign] = LEG_PRICER.presentValue(FLOOR_LEG, MULTICURVE, calibratedShifted).Amount;
			  }
			  double sensitivityFd = (pv[1] - pv[0]) / (2 * fdShift); // FD sensitivity computation
			  SabrSwaptionCalibratorSmileTestUtils.checkAcceptable(sensitivityFd, sensitivityArray.get(surfacePointIndex), 0.10, "Tenor/Expiry: " + TENORS.get(looptenor) + " / " + EXPIRIES.get(loopexpiry));
			  surfacePointIndex++;
			}
		  }
		}
	  }

	}

}