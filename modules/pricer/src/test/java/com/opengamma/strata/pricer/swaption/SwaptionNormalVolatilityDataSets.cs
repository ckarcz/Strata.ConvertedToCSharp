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


	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;
	using RatesProviderDataSets = com.opengamma.strata.pricer.datasets.RatesProviderDataSets;
	using FixedIborSwapConvention = com.opengamma.strata.product.swap.type.FixedIborSwapConvention;
	using FixedRateSwapLegConvention = com.opengamma.strata.product.swap.type.FixedRateSwapLegConvention;
	using IborRateSwapLegConvention = com.opengamma.strata.product.swap.type.IborRateSwapLegConvention;
	using ImmutableFixedIborSwapConvention = com.opengamma.strata.product.swap.type.ImmutableFixedIborSwapConvention;

	/// <summary>
	/// Black volatility data sets for testing.
	/// </summary>
	public class SwaptionNormalVolatilityDataSets
	{

	  private const double BP1 = 1.0E-4;

	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(LINEAR, LINEAR);

	  //     =====     Standard figures for testing     =====
	  private static readonly DoubleArray TIMES = DoubleArray.of(0.5, 0.5, 0.5, 0.5, 0.5, 1, 1, 1, 1, 1, 5, 5, 5, 5, 5, 10, 10, 10, 10, 10);
	  private static readonly DoubleArray TENORS = DoubleArray.of(1, 2, 5, 10, 30, 1, 2, 5, 10, 30, 1, 2, 5, 10, 30, 1, 2, 5, 10, 30);
	  private static readonly DoubleArray NORMAL_VOL = DoubleArray.of(0.010, 0.011, 0.012, 0.013, 0.014, 0.011, 0.012, 0.013, 0.014, 0.015, 0.012, 0.013, 0.014, 0.015, 0.016, 0.013, 0.014, 0.015, 0.016, 0.017);

	  private static readonly BusinessDayAdjustment MOD_FOL_US = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, USNY);
	  private static readonly FixedRateSwapLegConvention USD_FIXED_1Y_30U360 = FixedRateSwapLegConvention.of(USD, THIRTY_U_360, Frequency.P6M, MOD_FOL_US);
	  private static readonly IborRateSwapLegConvention USD_IBOR_LIBOR3M = IborRateSwapLegConvention.of(USD_LIBOR_3M);
	  public static readonly FixedIborSwapConvention USD_1Y_LIBOR3M = ImmutableFixedIborSwapConvention.of("USD-Swap", USD_FIXED_1Y_30U360, USD_IBOR_LIBOR3M);
	  private static readonly SurfaceMetadata METADATA = Surfaces.normalVolatilityByExpiryTenor("Normal Vol", ACT_365F);
	  private static readonly InterpolatedNodalSurface SURFACE_STD = InterpolatedNodalSurface.of(METADATA, TIMES, TENORS, NORMAL_VOL, INTERPOLATOR_2D);

	  private static readonly LocalDate VAL_DATE_STD = RatesProviderDataSets.VAL_DATE_2014_01_22;
	  private static readonly LocalTime VAL_TIME_STD = LocalTime.of(13, 45);
	  private static readonly ZoneId VAL_ZONE_STD = ZoneId.of("Europe/London");
	  private static readonly ZonedDateTime VAL_DATE_TIME_STD = VAL_DATE_STD.atTime(VAL_TIME_STD).atZone(VAL_ZONE_STD);
	  public static readonly NormalSwaptionExpiryTenorVolatilities NORMAL_SWAPTION_VOLS_USD_STD = NormalSwaptionExpiryTenorVolatilities.of(USD_1Y_LIBOR3M, VAL_DATE_TIME_STD, SURFACE_STD);

	  /// <summary>
	  /// Returns the swaption normal volatility surface shifted by a given amount. The shift is parallel. </summary>
	  /// <param name="shift">  the shift </param>
	  /// <returns> the swaption normal volatility surface </returns>
	  public static NormalSwaptionExpiryTenorVolatilities normalVolSwaptionProviderUsdStsShifted(double shift)
	  {
		DoubleArray volShifted = NORMAL_VOL.map(v => v + shift);
		return NormalSwaptionExpiryTenorVolatilities.of(USD_1Y_LIBOR3M, VAL_DATE_TIME_STD, SURFACE_STD.withZValues(volShifted));
	  }

	  public static NormalSwaptionExpiryTenorVolatilities normalVolSwaptionProviderUsdStd(LocalDate valuationDate)
	  {
		return NormalSwaptionExpiryTenorVolatilities.of(USD_1Y_LIBOR3M, valuationDate.atTime(VAL_TIME_STD).atZone(VAL_ZONE_STD), SURFACE_STD);
	  }

	  //     =====     Flat volatilities for testing     =====

	  private static readonly DoubleArray TIMES_FLAT = DoubleArray.of(0, 0, 100, 100);
	  private static readonly DoubleArray TENOR_FLAT = DoubleArray.of(0, 30, 0, 30);
	  private static readonly DoubleArray NORMAL_VOL_FLAT = DoubleArray.of(0.01, 0.01, 0.01, 0.01);
	  private static readonly InterpolatedNodalSurface SURFACE_FLAT = InterpolatedNodalSurface.of(METADATA, TIMES_FLAT, TENOR_FLAT, NORMAL_VOL_FLAT, INTERPOLATOR_2D);

	  public static readonly NormalSwaptionExpiryTenorVolatilities NORMAL_SWAPTION_VOLS_USD_FLAT = NormalSwaptionExpiryTenorVolatilities.of(USD_1Y_LIBOR3M, VAL_DATE_TIME_STD, SURFACE_FLAT);

	  //     =====     Market data as of 2014-03-20     =====

	  private static readonly DoubleArray TIMES_20150320 = DoubleArray.of(0.25, 0.25, 0.25, 0.25, 0.25, 0.50, 0.50, 0.50, 0.50, 0.50, 1.0, 1.0, 1.0, 1.0, 1.0, 2.0, 2.0, 2.0, 2.0, 2.0, 5.0, 5.0, 5.0, 5.0, 5.0, 10.0, 10.0, 10.0, 10.0, 10.0);
	  private static readonly DoubleArray TENORS_20150320 = DoubleArray.of(1.0, 2.0, 5.0, 10.0, 30.0, 1.0, 2.0, 5.0, 10.0, 30.0, 1.0, 2.0, 5.0, 10.0, 30.0, 1.0, 2.0, 5.0, 10.0, 30.0, 1.0, 2.0, 5.0, 10.0, 30.0, 1.0, 2.0, 5.0, 10.0, 30.0);
	  private static readonly DoubleArray NORMAL_VOL_20150320_BP = DoubleArray.of(43.6, 65.3, 88, 87.5, 88, 55.5, 72.2, 90.3, 89.3, 88.6, 72.6, 82.7, 91.6, 89.8, 87.3, 90.4, 91.9, 93.4, 84.7, 93.5, 99.3, 96.8, 94.3, 88.6, 77.3, 88.4, 85.9, 82.2, 76.7, 65.1); // 10Y
	  private static readonly DoubleArray NORMAL_VOL_20150320 = NORMAL_VOL_20150320_BP.map(v => v * BP1);
	  private static readonly Surface SURFACE_20150320 = InterpolatedNodalSurface.of(METADATA, TIMES_20150320, TENORS_20150320, NORMAL_VOL_20150320, INTERPOLATOR_2D);

	  private static readonly LocalDate VAL_DATE_20150320 = LocalDate.of(2015, 3, 20);
	  private static readonly LocalTime VAL_TIME_20150320 = LocalTime.of(18, 0x0);
	  private static readonly ZoneId VAL_ZONE_20150320 = ZoneId.of("Europe/London");

	  public static readonly NormalSwaptionExpiryTenorVolatilities NORMAL_SWAPTION_VOLS_USD_20150320 = NormalSwaptionExpiryTenorVolatilities.of(USD_1Y_LIBOR3M, VAL_DATE_20150320.atTime(VAL_TIME_20150320).atZone(VAL_ZONE_20150320), SURFACE_20150320);

	}

}