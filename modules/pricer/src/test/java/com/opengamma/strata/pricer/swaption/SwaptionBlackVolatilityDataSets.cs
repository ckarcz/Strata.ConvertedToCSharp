/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.THIRTY_U_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M;


	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ConstantSurface = com.opengamma.strata.market.surface.ConstantSurface;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;
	using FixedIborSwapConvention = com.opengamma.strata.product.swap.type.FixedIborSwapConvention;
	using FixedRateSwapLegConvention = com.opengamma.strata.product.swap.type.FixedRateSwapLegConvention;
	using IborRateSwapLegConvention = com.opengamma.strata.product.swap.type.IborRateSwapLegConvention;
	using ImmutableFixedIborSwapConvention = com.opengamma.strata.product.swap.type.ImmutableFixedIborSwapConvention;

	/// <summary>
	/// Black volatility data sets for testing.
	/// </summary>
	public class SwaptionBlackVolatilityDataSets
	{

	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(LINEAR, LINEAR);

	  //     =====     Standard figures for testing     =====
	  private static readonly DoubleArray TIMES = DoubleArray.of(0.5, 0.5, 0.5, 0.5, 0.5, 1, 1, 1, 1, 1, 5, 5, 5, 5, 5, 10, 10, 10, 10, 10);
	  private static readonly DoubleArray TENOR = DoubleArray.of(1, 2, 5, 10, 30, 1, 2, 5, 10, 30, 1, 2, 5, 10, 30, 1, 2, 5, 10, 30);
	  private static readonly DoubleArray BLACK_VOL = DoubleArray.of(0.45, 0.425, 0.4, 0.375, 0.35, 0.425, 0.4, 0.375, 0.35, 0.325, 0.4, 0.375, 0.35, 0.325, 0.3, 0.375, 0.35, 0.325, 0.3, 0.275);

	  private static readonly BusinessDayAdjustment MOD_FOL_US = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, USNY);
	  private static readonly FixedRateSwapLegConvention USD_FIXED_1Y_30U360 = FixedRateSwapLegConvention.of(USD, THIRTY_U_360, Frequency.P6M, MOD_FOL_US);
	  private static readonly IborRateSwapLegConvention USD_IBOR_LIBOR3M = IborRateSwapLegConvention.of(USD_LIBOR_3M);
	  public static readonly FixedIborSwapConvention USD_1Y_LIBOR3M = ImmutableFixedIborSwapConvention.of("USD-Swap", USD_FIXED_1Y_30U360, USD_IBOR_LIBOR3M);
	  private static readonly SurfaceMetadata METADATA_STD = Surfaces.blackVolatilityByExpiryTenor("Black Vol", ACT_365F);
	  private static readonly Surface SURFACE_STD = InterpolatedNodalSurface.of(METADATA_STD, TIMES, TENOR, BLACK_VOL, INTERPOLATOR_2D);

	  private static readonly LocalDate VAL_DATE_STD = LocalDate.of(2015, 8, 7);
	  private static readonly LocalTime VAL_TIME_STD = LocalTime.of(13, 45);
	  private static readonly ZoneId VAL_ZONE_STD = ZoneId.of("Europe/London");
	  /// <summary>
	  /// Black volatility provider </summary>
	  public static readonly BlackSwaptionExpiryTenorVolatilities BLACK_SWAPTION_VOLS_USD_STD = BlackSwaptionExpiryTenorVolatilities.of(USD_1Y_LIBOR3M, VAL_DATE_STD.atTime(VAL_TIME_STD).atZone(VAL_ZONE_STD), SURFACE_STD);

	  /// <summary>
	  /// constant volatility </summary>
	  public const double VOLATILITY = 0.20;
	  /// <summary>
	  /// metadata for constant surface </summary>
	  public static readonly SurfaceMetadata META_DATA = Surfaces.blackVolatilityByExpiryTenor("Constant Surface", ACT_365F);
	  private static readonly Surface CST_SURFACE = ConstantSurface.of(META_DATA, VOLATILITY);
	  /// <summary>
	  /// flat Black volatility provider </summary>
	  public static readonly BlackSwaptionExpiryTenorVolatilities BLACK_SWAPTION_VOLS_CST_USD = BlackSwaptionExpiryTenorVolatilities.of(USD_FIXED_6M_LIBOR_3M, VAL_DATE_STD.atTime(VAL_TIME_STD).atZone(VAL_ZONE_STD), CST_SURFACE);

	}

}