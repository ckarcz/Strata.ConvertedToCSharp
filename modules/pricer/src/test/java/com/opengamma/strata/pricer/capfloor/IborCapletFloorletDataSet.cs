/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;


	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using FxMatrix = com.opengamma.strata.basics.currency.FxMatrix;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;

	/// <summary>
	/// Data set of Ibor caplet/floorlet.
	/// </summary>
	public class IborCapletFloorletDataSet
	{

	  // Rates provider
	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;
	  private static readonly DoubleArray DSC_TIME = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 10.0);
	  private static readonly DoubleArray DSC_RATE = DoubleArray.of(0.0150, 0.0125, 0.0150, 0.0175, 0.0150, 0.0150);
	  /// <summary>
	  /// discounting curve name </summary>
	  public static readonly CurveName DSC_NAME = CurveName.of("EUR Dsc");
	  private static readonly CurveMetadata META_DSC = Curves.zeroRates(DSC_NAME, ACT_ACT_ISDA);
	  private static readonly InterpolatedNodalCurve DSC_CURVE = InterpolatedNodalCurve.of(META_DSC, DSC_TIME, DSC_RATE, INTERPOLATOR);
	  private static readonly DoubleArray FWD3_TIME = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 3.0, 4.0, 5.0, 10.0);
	  private static readonly DoubleArray FWD3_RATE = DoubleArray.of(0.0150, 0.0125, 0.0150, 0.0175, 0.0175, 0.0190, 0.0200, 0.0210);
	  /// <summary>
	  /// Forward curve name </summary>
	  public static readonly CurveName FWD3_NAME = CurveName.of("EUR EURIBOR 3M");
	  private static readonly CurveMetadata META_FWD3 = Curves.zeroRates(FWD3_NAME, ACT_ACT_ISDA);
	  private static readonly InterpolatedNodalCurve FWD3_CURVE = InterpolatedNodalCurve.of(META_FWD3, FWD3_TIME, FWD3_RATE, INTERPOLATOR);
	  private static readonly DoubleArray FWD6_TIME = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 10.0);
	  private static readonly DoubleArray FWD6_RATE = DoubleArray.of(0.0150, 0.0125, 0.0150, 0.0175, 0.0150, 0.0150);
	  /// <summary>
	  /// Forward curve name </summary>
	  public static readonly CurveName FWD6_NAME = CurveName.of("EUR EURIBOR 6M");
	  private static readonly CurveMetadata META_FWD6 = Curves.zeroRates(FWD6_NAME, ACT_ACT_ISDA);
	  private static readonly InterpolatedNodalCurve FWD6_CURVE = InterpolatedNodalCurve.of(META_FWD6, FWD6_TIME, FWD6_RATE, INTERPOLATOR);

	  /// <summary>
	  /// Creates rates provider with specified valuation date.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <returns>  the rates provider </returns>
	  public static ImmutableRatesProvider createRatesProvider(LocalDate valuationDate)
	  {
		return ImmutableRatesProvider.builder(valuationDate).discountCurves(ImmutableMap.of(EUR, DSC_CURVE)).indexCurves(ImmutableMap.of(EUR_EURIBOR_3M, FWD3_CURVE, EUR_EURIBOR_6M, FWD6_CURVE)).fxRateProvider(FxMatrix.empty()).build();
	  }

	  /// <summary>
	  /// Creates rates provider with specified valuation date and time series of the index.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <param name="index">  the index </param>
	  /// <param name="timeSeries">  the time series </param>
	  /// <returns>  the rates provider </returns>
	  public static ImmutableRatesProvider createRatesProvider(LocalDate valuationDate, IborIndex index, LocalDateDoubleTimeSeries timeSeries)
	  {
		return ImmutableRatesProvider.builder(valuationDate).discountCurves(ImmutableMap.of(EUR, DSC_CURVE)).indexCurves(ImmutableMap.of(EUR_EURIBOR_3M, FWD3_CURVE, EUR_EURIBOR_6M, FWD6_CURVE)).fxRateProvider(FxMatrix.empty()).timeSeries(index, timeSeries).build();
	  }

	  // Black volatilities provider
	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly DoubleArray EXPIRIES = DoubleArray.of(0.5, 0.5, 0.5, 1.0, 1.0, 1.0, 5.0, 5.0, 5.0);
	  private static readonly DoubleArray STRIKES = DoubleArray.of(0.01, 0.02, 0.03, 0.01, 0.02, 0.03, 0.01, 0.02, 0.03);
	  private static readonly DoubleArray BLACK_VOLS = DoubleArray.of(0.35, 0.30, 0.28, 0.34, 0.25, 0.23, 0.25, 0.20, 0.18);
	  private static readonly SurfaceMetadata BLACK_METADATA = Surfaces.blackVolatilityByExpiryStrike("Black Vol", ACT_ACT_ISDA);
	  private static readonly Surface BLACK_SURFACE_EXP_STR = InterpolatedNodalSurface.of(BLACK_METADATA, EXPIRIES, STRIKES, BLACK_VOLS, INTERPOLATOR_2D);

	  // Black volatilities provider with shift
	  /// <summary>
	  /// constant shift </summary>
	  public const double SHIFT = 5.0e-2;
	  private static readonly DoubleArray SHIFTED_STRIKES = DoubleArray.of(STRIKES.size(), i => STRIKES.get(i) + SHIFT);
	  private static readonly SurfaceMetadata SHIFTED_BLACK_METADATA = Surfaces.blackVolatilityByExpiryStrike("Shifted Black vol", ACT_ACT_ISDA);
	  private static readonly Surface SHIFTED_BLACK_SURFACE_EXP_STR = InterpolatedNodalSurface.of(SHIFTED_BLACK_METADATA, EXPIRIES, SHIFTED_STRIKES, BLACK_VOLS, INTERPOLATOR_2D);
	  private static readonly Curve SHIFT_CURVE = ConstantCurve.of("const shift", SHIFT);

	  /// <summary>
	  /// Creates volatilities provider with specified date and index.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <param name="index">  the index </param>
	  /// <returns>  the volatilities provider </returns>
	  public static BlackIborCapletFloorletExpiryStrikeVolatilities createBlackVolatilities(ZonedDateTime valuationDate, IborIndex index)
	  {
		return BlackIborCapletFloorletExpiryStrikeVolatilities.of(index, valuationDate, BLACK_SURFACE_EXP_STR);
	  }

	  /// <summary>
	  /// Creates shifted Black volatilities provider with specified date and index.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <param name="index">  the index </param>
	  /// <returns>  the volatilities provider </returns>
	  public static ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities createShiftedBlackVolatilities(ZonedDateTime valuationDate, IborIndex index)
	  {
		return ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities.of(index, valuationDate, SHIFTED_BLACK_SURFACE_EXP_STR, SHIFT_CURVE);
	  }

	  // Normal volatilities provider
	  private static readonly DoubleArray NORMAL_VOLS = DoubleArray.of(0.09, 0.08, 0.05, 0.07, 0.05, 0.04, 0.06, 0.05, 0.03);
	  private static readonly SurfaceMetadata NORMAL_METADATA = Surfaces.normalVolatilityByExpiryStrike("Normal Vol", ACT_ACT_ISDA);
	  private static readonly Surface NORMAL_SURFACE_EXP_STR = InterpolatedNodalSurface.of(NORMAL_METADATA, EXPIRIES, STRIKES, NORMAL_VOLS, INTERPOLATOR_2D);

	  /// <summary>
	  /// Creates volatilities provider with specified date and index.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <param name="index">  the index </param>
	  /// <returns>  the volatilities provider </returns>
	  public static NormalIborCapletFloorletExpiryStrikeVolatilities createNormalVolatilities(ZonedDateTime valuationDate, IborIndex index)
	  {
		return NormalIborCapletFloorletExpiryStrikeVolatilities.of(index, valuationDate, NORMAL_SURFACE_EXP_STR);
	  }

	}

}