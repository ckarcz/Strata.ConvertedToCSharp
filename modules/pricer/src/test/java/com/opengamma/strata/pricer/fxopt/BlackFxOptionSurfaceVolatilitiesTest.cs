using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using DefaultSurfaceMetadata = com.opengamma.strata.market.surface.DefaultSurfaceMetadata;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using NodalSurface = com.opengamma.strata.market.surface.NodalSurface;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;

	/// <summary>
	/// Test <seealso cref="BlackFxOptionSurfaceVolatilities"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BlackFxOptionSurfaceVolatilitiesTest
	public class BlackFxOptionSurfaceVolatilitiesTest
	{

	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly DoubleArray TIMES = DoubleArray.of(0.25, 0.25, 0.25, 0.50, 0.50, 0.50, 1.00, 1.00, 1.00);
	  private static readonly DoubleArray STRIKES = DoubleArray.of(0.7, 0.8, 0.9, 0.7, 0.8, 0.9, 0.7, 0.8, 0.9);
	  private static readonly DoubleArray VOL_ARRAY = DoubleArray.of(0.011, 0.012, 0.010, 0.012, 0.013, 0.011, 0.013, 0.014, 0.014);
	  private static readonly SurfaceMetadata METADATA = DefaultSurfaceMetadata.builder().surfaceName("Test").xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.STRIKE).zValueType(ValueType.BLACK_VOLATILITY).dayCount(ACT_365F).build();
	  private static readonly InterpolatedNodalSurface SURFACE = InterpolatedNodalSurface.of(METADATA, TIMES, STRIKES, VOL_ARRAY, INTERPOLATOR_2D);
	  private static readonly LocalDate VAL_DATE = date(2015, 2, 17);
	  private static readonly LocalTime VAL_TIME = LocalTime.of(13, 45);
	  private static readonly ZoneId LONDON_ZONE = ZoneId.of("Europe/London");
	  private static readonly ZonedDateTime VAL_DATE_TIME = VAL_DATE.atTime(VAL_TIME).atZone(LONDON_ZONE);
	  private static readonly CurrencyPair CURRENCY_PAIR = CurrencyPair.of(EUR, GBP);

	  private static readonly BlackFxOptionSurfaceVolatilities VOLS = BlackFxOptionSurfaceVolatilities.of(CURRENCY_PAIR, VAL_DATE_TIME, SURFACE);

	  private static readonly LocalTime TIME = LocalTime.of(11, 45);
	  private static readonly ZonedDateTime[] TEST_EXPIRY = new ZonedDateTime[] {date(2015, 2, 17).atTime(LocalTime.MIDNIGHT).atZone(LONDON_ZONE), date(2015, 9, 17).atTime(TIME).atZone(LONDON_ZONE), date(2016, 6, 17).atTime(TIME).atZone(LONDON_ZONE), date(2018, 7, 17).atTime(TIME).atZone(LONDON_ZONE)};
	  private static readonly double[] FORWARD = new double[] {0.85, 0.82, 0.77, 0.76};
	  private static readonly int NB_EXPIRY = TEST_EXPIRY.Length;
	  private static readonly double[] TEST_STRIKE = new double[] {0.65, 0.73, 0.85, 0.92};
	  private static readonly int NB_STRIKE = TEST_STRIKE.Length;

	  private const double TOLERANCE = 1.0E-12;
	  private const double EPS = 1.0E-7;

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		BlackFxOptionSurfaceVolatilities test = BlackFxOptionSurfaceVolatilities.builder().currencyPair(CURRENCY_PAIR).surface(SURFACE).valuationDateTime(VAL_DATE_TIME).build();
		assertEquals(test.ValuationDateTime, VAL_DATE_TIME);
		assertEquals(test.CurrencyPair, CURRENCY_PAIR);
		assertEquals(test.Surface, SURFACE);
		assertEquals(VOLS, test);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_volatility()
	  {
		for (int i = 0; i < NB_EXPIRY; i++)
		{
		  double expiryTime = VOLS.relativeTime(TEST_EXPIRY[i]);
		  for (int j = 0; j < NB_STRIKE; ++j)
		  {
			double volExpected = SURFACE.zValue(expiryTime, TEST_STRIKE[j]);
			double volComputed = VOLS.volatility(CURRENCY_PAIR, TEST_EXPIRY[i], TEST_STRIKE[j], FORWARD[i]);
			assertEquals(volComputed, volExpected, TOLERANCE);
		  }
		}
	  }

	  public virtual void test_volatility_inverse()
	  {
		for (int i = 0; i < NB_EXPIRY; i++)
		{
		  double expiryTime = VOLS.relativeTime(TEST_EXPIRY[i]);
		  for (int j = 0; j < NB_STRIKE; ++j)
		  {
			double volExpected = SURFACE.zValue(expiryTime, TEST_STRIKE[j]);
			double volComputed = VOLS.volatility(CURRENCY_PAIR.inverse(), TEST_EXPIRY[i], 1d / TEST_STRIKE[j], 1d / FORWARD[i]);
			assertEquals(volComputed, volExpected, TOLERANCE);
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_nodeSensitivity()
	  {
		for (int i = 0; i < NB_EXPIRY; i++)
		{
		  for (int j = 0; j < NB_STRIKE; ++j)
		  {
			double timeToExpiry = VOLS.relativeTime(TEST_EXPIRY[i]);
			FxOptionSensitivity sensi = FxOptionSensitivity.of(VOLS.Name, CURRENCY_PAIR, timeToExpiry, TEST_STRIKE[j], FORWARD[i], GBP, 1d);
			CurrencyParameterSensitivities computed = VOLS.parameterSensitivity(sensi);
			for (int k = 0; k < SURFACE.ParameterCount; k++)
			{
			  double value = computed.Sensitivities.get(0).Sensitivity.get(k);
			  double nodeExpiry = SURFACE.XValues.get(k);
			  double nodeStrike = SURFACE.YValues.get(k);
			  double expected = nodeSensitivity(VOLS, CURRENCY_PAIR, TEST_EXPIRY[i], TEST_STRIKE[j], FORWARD[i], nodeExpiry, nodeStrike);
			  assertEquals(value, expected, EPS);
			}
		  }
		}
	  }

	  public virtual void test_nodeSensitivity_inverse()
	  {
		for (int i = 0; i < NB_EXPIRY; i++)
		{
		  for (int j = 0; j < NB_STRIKE; ++j)
		  {
			double timeToExpiry = VOLS.relativeTime(TEST_EXPIRY[i]);
			FxOptionSensitivity sensi = FxOptionSensitivity.of(VOLS.Name, CURRENCY_PAIR.inverse(), timeToExpiry, 1d / TEST_STRIKE[j], 1d / FORWARD[i], GBP, 1d);
			CurrencyParameterSensitivities computed = VOLS.parameterSensitivity(sensi);
			for (int k = 0; k < SURFACE.ParameterCount; k++)
			{
			  double value = computed.Sensitivities.get(0).Sensitivity.get(k);
			  double nodeExpiry = SURFACE.XValues.get(k);
			  double nodeStrike = SURFACE.YValues.get(k);
			  double expected = nodeSensitivity(VOLS, CURRENCY_PAIR.inverse(), TEST_EXPIRY[i], 1d / TEST_STRIKE[j], 1d / FORWARD[i], nodeExpiry, nodeStrike);
			  assertEquals(value, expected, EPS);
			}
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		BlackFxOptionSurfaceVolatilities test1 = BlackFxOptionSurfaceVolatilities.of(CURRENCY_PAIR, VAL_DATE_TIME, SURFACE);
		coverImmutableBean(test1);
		BlackFxOptionSurfaceVolatilities test2 = BlackFxOptionSurfaceVolatilities.of(CURRENCY_PAIR.inverse(), ZonedDateTime.of(2015, 12, 21, 11, 15, 0, 0, ZoneId.of("Z")), SURFACE);
		coverBeanEquals(test1, test2);
	  }

	  //-------------------------------------------------------------------------
	  // bumping a node point at (nodeExpiry, nodeStrike)
	  private double nodeSensitivity(BlackFxOptionSurfaceVolatilities provider, CurrencyPair pair, ZonedDateTime expiry, double strike, double forward, double nodeExpiry, double nodeStrike)
	  {

		NodalSurface surface = (NodalSurface) provider.Surface;
		DoubleArray xValues = surface.XValues;
		DoubleArray yValues = surface.YValues;
		DoubleArray zValues = surface.ZValues;
		int nData = xValues.size();
		int index = -1;
		for (int i = 0; i < nData; ++i)
		{
		  if (Math.Abs(xValues.get(i) - nodeExpiry) < TOLERANCE && Math.Abs(yValues.get(i) - nodeStrike) < TOLERANCE)
		  {
			index = i;
		  }
		}
		Surface surfaceUp = surface.withZValues(zValues.with(index, zValues.get(index) + EPS));
		Surface surfaceDw = surface.withZValues(zValues.with(index, zValues.get(index) - EPS));
		BlackFxOptionSurfaceVolatilities provUp = BlackFxOptionSurfaceVolatilities.of(CURRENCY_PAIR, VAL_DATE_TIME, surfaceUp);
		BlackFxOptionSurfaceVolatilities provDw = BlackFxOptionSurfaceVolatilities.of(CURRENCY_PAIR, VAL_DATE_TIME, surfaceDw);
		double volUp = provUp.volatility(pair, expiry, strike, forward);
		double volDw = provDw.volatility(pair, expiry, strike, forward);
		return 0.5 * (volUp - volDw) / EPS;
	  }

	}

}