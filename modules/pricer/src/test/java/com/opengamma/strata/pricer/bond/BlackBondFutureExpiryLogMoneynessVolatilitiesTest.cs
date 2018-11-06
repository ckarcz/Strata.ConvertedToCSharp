using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.dateUtc;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ValueType = com.opengamma.strata.market.ValueType;
	using LogMoneynessStrike = com.opengamma.strata.market.option.LogMoneynessStrike;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using DefaultSurfaceMetadata = com.opengamma.strata.market.surface.DefaultSurfaceMetadata;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using SurfaceName = com.opengamma.strata.market.surface.SurfaceName;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;
	using GenericVolatilitySurfaceYearFractionParameterMetadata = com.opengamma.strata.pricer.common.GenericVolatilitySurfaceYearFractionParameterMetadata;

	/// <summary>
	/// Test <seealso cref="BlackBondFutureExpiryLogMoneynessVolatilities"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BlackBondFutureExpiryLogMoneynessVolatilitiesTest
	public class BlackBondFutureExpiryLogMoneynessVolatilitiesTest
	{

	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly DoubleArray TIME = DoubleArray.of(0.25, 0.25, 0.25, 0.25, 0.50, 0.50, 0.50, 0.50, 1.00, 1.00, 1.00, 1.00);
	  private static readonly DoubleArray MONEYNESS = DoubleArray.of(-0.02, -0.01, 0.00, 0.01, -0.02, -0.01, 0.00, 0.01, -0.02, -0.01, 0.00, 0.01);
	  private static readonly DoubleArray VOL = DoubleArray.of(0.01, 0.011, 0.012, 0.010, 0.011, 0.012, 0.013, 0.012, 0.012, 0.013, 0.014, 0.014);
	  private static readonly SurfaceMetadata METADATA;
	  static BlackBondFutureExpiryLogMoneynessVolatilitiesTest()
	  {
		IList<GenericVolatilitySurfaceYearFractionParameterMetadata> list = new List<GenericVolatilitySurfaceYearFractionParameterMetadata>();
		int nData = TIME.size();
		for (int i = 0; i < nData; ++i)
		{
		  GenericVolatilitySurfaceYearFractionParameterMetadata parameterMetadata = GenericVolatilitySurfaceYearFractionParameterMetadata.of(TIME.get(i), LogMoneynessStrike.of(MONEYNESS.get(i)));
		  list.Add(parameterMetadata);
		}
		METADATA = DefaultSurfaceMetadata.builder().surfaceName(SurfaceName.of("GOVT1-BOND-FUT-VOL")).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.LOG_MONEYNESS).zValueType(ValueType.BLACK_VOLATILITY).dayCount(ACT_365F).parameterMetadata(list).build();
	  }
	  private static readonly InterpolatedNodalSurface SURFACE = InterpolatedNodalSurface.of(METADATA, TIME, MONEYNESS, VOL, INTERPOLATOR_2D);
	  private static readonly LocalDate VAL_DATE = date(2015, 2, 17);
	  private static readonly LocalTime VAL_TIME = LocalTime.of(13, 45);
	  private static readonly ZoneId LONDON_ZONE = ZoneId.of("Europe/London");
	  private static readonly ZonedDateTime VAL_DATE_TIME = VAL_DATE.atTime(VAL_TIME).atZone(LONDON_ZONE);
	  private static readonly BlackBondFutureExpiryLogMoneynessVolatilities VOLS = BlackBondFutureExpiryLogMoneynessVolatilities.of(VAL_DATE_TIME, SURFACE);

	  private static readonly ZonedDateTime[] TEST_OPTION_EXPIRY = new ZonedDateTime[] {dateUtc(2015, 2, 17), dateUtc(2015, 5, 17), dateUtc(2015, 6, 17), dateUtc(2017, 2, 17)};
	  private static readonly int NB_TEST = TEST_OPTION_EXPIRY.Length;
	  private static readonly LocalDate[] TEST_FUTURE_EXPIRY = new LocalDate[] {date(2015, 2, 17), date(2015, 5, 17), date(2015, 5, 17), date(2015, 5, 17)};
	  private static readonly double[] TEST_STRIKE_PRICE = new double[] {0.985, 0.985, 0.985, 0.985};
	  private static readonly double[] TEST_FUTURE_PRICE = new double[] {0.98, 0.985, 1.00, 1.01};
	  //  private static final double[] TEST_SENSITIVITY = new double[] {9.2, 16.0, 1.8, 5.7 };
	  private static readonly double[] TEST_SENSITIVITY = new double[] {1.0, 1.0, 1.0, 1.0};

	  private const double TOLERANCE_VOL = 1.0E-10;

	  //-------------------------------------------------------------------------
	  public virtual void test_valuationDate()
	  {
		assertEquals(VOLS.ValuationDateTime, VAL_DATE_TIME);
	  }

	  public virtual void test_volatility()
	  {
		for (int i = 0; i < NB_TEST; i++)
		{
		  double expiryTime = VOLS.relativeTime(TEST_OPTION_EXPIRY[i]);
		  double volExpected = SURFACE.zValue(expiryTime, Math.Log(TEST_STRIKE_PRICE[i] / TEST_FUTURE_PRICE[i]));
		  double volComputed = VOLS.volatility(TEST_OPTION_EXPIRY[i], TEST_FUTURE_EXPIRY[i], TEST_STRIKE_PRICE[i], TEST_FUTURE_PRICE[i]);
		  assertEquals(volComputed, volExpected, TOLERANCE_VOL);
		}
	  }

	  public virtual void test_volatility_sensitivity()
	  {
		double eps = 1.0e-6;
		int nData = TIME.size();
		for (int i = 0; i < NB_TEST; i++)
		{
		  double expiry = VOLS.relativeTime(TEST_OPTION_EXPIRY[i]);
		  BondFutureOptionSensitivity point = BondFutureOptionSensitivity.of(VOLS.Name, expiry, TEST_FUTURE_EXPIRY[i], TEST_STRIKE_PRICE[i], TEST_FUTURE_PRICE[i], USD, TEST_SENSITIVITY[i]);
		  CurrencyParameterSensitivity sensActual = VOLS.parameterSensitivity(point).Sensitivities.get(0);
		  double[] computed = sensActual.Sensitivity.toArray();
		  for (int j = 0; j < nData; j++)
		  {
			DoubleArray volDataUp = VOL.with(j, VOL.get(j) + eps);
			DoubleArray volDataDw = VOL.with(j, VOL.get(j) - eps);
			InterpolatedNodalSurface paramUp = InterpolatedNodalSurface.of(METADATA, TIME, MONEYNESS, volDataUp, INTERPOLATOR_2D);
			InterpolatedNodalSurface paramDw = InterpolatedNodalSurface.of(METADATA, TIME, MONEYNESS, volDataDw, INTERPOLATOR_2D);
			BlackBondFutureExpiryLogMoneynessVolatilities provUp = BlackBondFutureExpiryLogMoneynessVolatilities.of(VAL_DATE_TIME, paramUp);
			BlackBondFutureExpiryLogMoneynessVolatilities provDw = BlackBondFutureExpiryLogMoneynessVolatilities.of(VAL_DATE_TIME, paramDw);
			double volUp = provUp.volatility(expiry, TEST_FUTURE_EXPIRY[i], TEST_STRIKE_PRICE[i], TEST_FUTURE_PRICE[i]);
			double volDw = provDw.volatility(expiry, TEST_FUTURE_EXPIRY[i], TEST_STRIKE_PRICE[i], TEST_FUTURE_PRICE[i]);
			double fd = 0.5 * (volUp - volDw) / eps;
			assertEquals(computed[j], fd, eps);
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		BlackBondFutureExpiryLogMoneynessVolatilities test1 = BlackBondFutureExpiryLogMoneynessVolatilities.of(VAL_DATE_TIME, SURFACE);
		coverImmutableBean(test1);
		BlackBondFutureExpiryLogMoneynessVolatilities test2 = BlackBondFutureExpiryLogMoneynessVolatilities.of(VAL_DATE_TIME.plusDays(1), SURFACE.withParameter(0, 1d));
		coverBeanEquals(test1, test2);
	  }

	}

}