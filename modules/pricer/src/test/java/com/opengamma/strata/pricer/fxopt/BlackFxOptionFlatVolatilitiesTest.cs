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
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using DefaultCurveMetadata = com.opengamma.strata.market.curve.DefaultCurveMetadata;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using NodalCurve = com.opengamma.strata.market.curve.NodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;

	/// <summary>
	/// Test <seealso cref="BlackFxOptionFlatVolatilities"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BlackFxOptionFlatVolatilitiesTest
	public class BlackFxOptionFlatVolatilitiesTest
	{

	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;
	  private static readonly DoubleArray TIMES = DoubleArray.of(0.5, 1.0, 3.0);
	  private static readonly DoubleArray VOL_ARRAY = DoubleArray.of(0.05, 0.09, 0.16);
	  private static readonly CurveMetadata METADATA = DefaultCurveMetadata.builder().curveName("Test").xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.BLACK_VOLATILITY).dayCount(ACT_365F).build();
	  private static readonly InterpolatedNodalCurve CURVE = InterpolatedNodalCurve.of(METADATA, TIMES, VOL_ARRAY, INTERPOLATOR);
	  private static readonly LocalDate VAL_DATE = date(2015, 2, 17);
	  private static readonly LocalTime VAL_TIME = LocalTime.of(13, 45);
	  private static readonly ZoneId LONDON_ZONE = ZoneId.of("Europe/London");
	  private static readonly ZonedDateTime VAL_DATE_TIME = VAL_DATE.atTime(VAL_TIME).atZone(LONDON_ZONE);
	  private static readonly CurrencyPair CURRENCY_PAIR = CurrencyPair.of(EUR, GBP);
	  private static readonly BlackFxOptionFlatVolatilities VOLS = BlackFxOptionFlatVolatilities.of(CURRENCY_PAIR, VAL_DATE_TIME, CURVE);

	  private static readonly LocalTime TIME = LocalTime.of(11, 45);
	  private static readonly ZonedDateTime[] TEST_EXPIRY = new ZonedDateTime[] {date(2015, 2, 17).atTime(LocalTime.MIDNIGHT).atZone(LONDON_ZONE), date(2015, 9, 17).atTime(TIME).atZone(LONDON_ZONE), date(2016, 6, 17).atTime(TIME).atZone(LONDON_ZONE), date(2018, 7, 17).atTime(TIME).atZone(LONDON_ZONE)};
	  private static readonly double[] FORWARD = new double[] {0.85, 0.82, 0.75, 0.68};
	  private static readonly int NB_EXPIRY = TEST_EXPIRY.Length;
	  private static readonly double[] TEST_STRIKE = new double[] {0.67, 0.81, 0.92};
	  private static readonly int NB_STRIKE = TEST_STRIKE.Length;

	  private const double TOLERANCE = 1.0E-12;
	  private const double EPS = 1.0E-7;

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		BlackFxOptionFlatVolatilities test = BlackFxOptionFlatVolatilities.builder().currencyPair(CURRENCY_PAIR).curve(CURVE).valuationDateTime(VAL_DATE_TIME).build();
		assertEquals(test.ValuationDateTime, VAL_DATE_TIME);
		assertEquals(test.CurrencyPair, CURRENCY_PAIR);
		assertEquals(test.Name, FxOptionVolatilitiesName.of(CURVE.Name.Name));
		assertEquals(test.Curve, CURVE);
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
			double volExpected = CURVE.yValue(expiryTime);
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
			double volExpected = CURVE.yValue(expiryTime);
			double volComputed = VOLS.volatility(CURRENCY_PAIR.inverse(), TEST_EXPIRY[i], 1d / TEST_STRIKE[j], 1d / FORWARD[i]);
			assertEquals(volComputed, volExpected, TOLERANCE);
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parameterSensitivity()
	  {
		for (int i = 0; i < NB_EXPIRY; i++)
		{
		  for (int j = 0; j < NB_STRIKE; ++j)
		  {
			double timeToExpiry = VOLS.relativeTime(TEST_EXPIRY[i]);
			FxOptionSensitivity sensi = FxOptionSensitivity.of(VOLS.Name, CURRENCY_PAIR, timeToExpiry, TEST_STRIKE[j], FORWARD[i], GBP, 1d);
			CurrencyParameterSensitivities computed = VOLS.parameterSensitivity(sensi);
			for (int k = 0; k < TIMES.size(); k++)
			{
			  double value = computed.Sensitivities.get(0).Sensitivity.get(k);
			  double nodeExpiry = TIMES.get(k);
			  double expected = nodeSensitivity(VOLS, CURRENCY_PAIR, TEST_EXPIRY[i], TEST_STRIKE[j], FORWARD[i], nodeExpiry);
			  assertEquals(value, expected, EPS);
			}
		  }
		}
	  }

	  public virtual void test_parameterSensitivity_inverse()
	  {
		for (int i = 0; i < NB_EXPIRY; i++)
		{
		  for (int j = 0; j < NB_STRIKE; ++j)
		  {
			double timeToExpiry = VOLS.relativeTime(TEST_EXPIRY[i]);
			FxOptionSensitivity sensi = FxOptionSensitivity.of(VOLS.Name, CURRENCY_PAIR.inverse(), timeToExpiry, 1d / TEST_STRIKE[j], 1d / FORWARD[i], GBP, 1d);
			CurrencyParameterSensitivities computed = VOLS.parameterSensitivity(sensi);
			for (int k = 0; k < TIMES.size(); k++)
			{
			  double value = computed.Sensitivities.get(0).Sensitivity.get(k);
			  double nodeExpiry = TIMES.get(k);
			  double expected = nodeSensitivity(VOLS, CURRENCY_PAIR.inverse(), TEST_EXPIRY[i], 1d / TEST_STRIKE[j], 1d / FORWARD[i], nodeExpiry);
			  assertEquals(value, expected, EPS);
			}
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		BlackFxOptionFlatVolatilities test1 = BlackFxOptionFlatVolatilities.of(CURRENCY_PAIR, VAL_DATE_TIME, CURVE);
		coverImmutableBean(test1);
		BlackFxOptionFlatVolatilities test2 = BlackFxOptionFlatVolatilities.of(CURRENCY_PAIR.inverse(), ZonedDateTime.of(2015, 12, 21, 11, 15, 0, 0, ZoneId.of("Z")), CURVE);
		coverBeanEquals(test1, test2);
	  }

	  //-------------------------------------------------------------------------
	  // bumping a node point at nodeExpiry
	  private double nodeSensitivity(BlackFxOptionFlatVolatilities provider, CurrencyPair pair, ZonedDateTime expiry, double strike, double forward, double nodeExpiry)
	  {

		NodalCurve curve = (NodalCurve) provider.Curve;
		DoubleArray xValues = curve.XValues;
		DoubleArray yValues = curve.YValues;

		int nData = xValues.size();
		int index = -1;
		for (int i = 0; i < nData; ++i)
		{
		  if (Math.Abs(xValues.get(i) - nodeExpiry) < TOLERANCE)
		  {
			index = i;
		  }
		}
		NodalCurve curveUp = curve.withYValues(yValues.with(index, yValues.get(index) + EPS));
		NodalCurve curveDw = curve.withYValues(yValues.with(index, yValues.get(index) - EPS));
		BlackFxOptionFlatVolatilities provUp = BlackFxOptionFlatVolatilities.of(CURRENCY_PAIR, VAL_DATE_TIME, curveUp);
		BlackFxOptionFlatVolatilities provDw = BlackFxOptionFlatVolatilities.of(CURRENCY_PAIR, VAL_DATE_TIME, curveDw);
		double volUp = provUp.volatility(pair, expiry, strike, forward);
		double volDw = provDw.volatility(pair, expiry, strike, forward);
		return 0.5 * (volUp - volDw) / EPS;
	  }

	}

}