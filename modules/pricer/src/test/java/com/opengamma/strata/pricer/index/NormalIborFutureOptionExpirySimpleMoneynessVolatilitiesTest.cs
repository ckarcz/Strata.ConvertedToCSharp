/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_6M;
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
	using MoneynessType = com.opengamma.strata.market.model.MoneynessType;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;

	/// <summary>
	/// Tests <seealso cref="NormalIborFutureOptionExpirySimpleMoneynessVolatilities"/>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NormalIborFutureOptionExpirySimpleMoneynessVolatilitiesTest
	public class NormalIborFutureOptionExpirySimpleMoneynessVolatilitiesTest
	{

	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly DoubleArray TIMES = DoubleArray.of(0.25, 0.25, 0.25, 0.25, 0.5, 0.5, 0.5, 0.5, 1, 1, 1, 1);
	  private static readonly DoubleArray MONEYNESS_PRICES = DoubleArray.of(-0.02, -0.01, 0, 0.01, -0.02, -0.01, 0, 0.01, -0.02, -0.01, 0, 0.01);
	  private static readonly DoubleArray NORMAL_VOL_PRICES = DoubleArray.of(0.01, 0.011, 0.012, 0.010, 0.011, 0.012, 0.013, 0.012, 0.012, 0.013, 0.014, 0.014);
	  private static readonly DoubleArray MONEYNESS_RATES = DoubleArray.of(-0.01, 0, 0.01, 0.02, -0.01, 0, 0.01, 0.02, -0.01, 0, 0.01, 0.02);
	  private static readonly DoubleArray NORMAL_VOL_RATES = DoubleArray.of(0.010, 0.012, 0.011, 0.01, 0.012, 0.013, 0.012, 0.011, 0.014, 0.014, 0.013, 0.012);
	  private static readonly InterpolatedNodalSurface PARAMETERS_PRICE = InterpolatedNodalSurface.of(Surfaces.normalVolatilityByExpirySimpleMoneyness("Price", ACT_365F, MoneynessType.PRICE), TIMES, MONEYNESS_PRICES, NORMAL_VOL_PRICES, INTERPOLATOR_2D);
	  private static readonly InterpolatedNodalSurface PARAMETERS_RATE = InterpolatedNodalSurface.of(Surfaces.normalVolatilityByExpirySimpleMoneyness("Rate", ACT_365F, MoneynessType.RATES), TIMES, MONEYNESS_RATES, NORMAL_VOL_RATES, INTERPOLATOR_2D);

	  private static readonly LocalDate VAL_DATE = date(2015, 2, 17);
	  private static readonly LocalTime VAL_TIME = LocalTime.of(13, 45);
	  private static readonly ZoneId LONDON_ZONE = ZoneId.of("Europe/London");
	  private static readonly ZonedDateTime VAL_DATE_TIME = VAL_DATE.atTime(VAL_TIME).atZone(LONDON_ZONE);

	  private static readonly NormalIborFutureOptionExpirySimpleMoneynessVolatilities VOL_SIMPLE_MONEY_PRICE = NormalIborFutureOptionExpirySimpleMoneynessVolatilities.of(EUR_EURIBOR_3M, VAL_DATE_TIME, PARAMETERS_PRICE);

	  private static readonly NormalIborFutureOptionExpirySimpleMoneynessVolatilities VOL_SIMPLE_MONEY_RATE = NormalIborFutureOptionExpirySimpleMoneynessVolatilities.of(EUR_EURIBOR_3M, VAL_DATE_TIME, PARAMETERS_RATE);

	  private static readonly ZonedDateTime[] TEST_EXPIRY = new ZonedDateTime[] {dateUtc(2015, 2, 17), dateUtc(2015, 5, 17), dateUtc(2015, 6, 17), dateUtc(2017, 2, 17)};
	  private static readonly int NB_TEST = TEST_EXPIRY.Length;
	  private static readonly LocalDate[] TEST_FIXING = new LocalDate[] {date(2015, 2, 17), date(2015, 5, 17), date(2015, 5, 17), date(2015, 5, 17)};
	  private static readonly double[] TEST_STRIKE_PRICE = new double[] {0.985, 0.985, 0.985, 0.985};
	  private static readonly double[] TEST_FUTURE_PRICE = new double[] {0.98, 0.985, 1.00, 1.01};

	  private const double TOLERANCE_VOL = 1.0E-10;
	  private const double TOLERANCE_DELTA = 1.0E-2;

	  //-------------------------------------------------------------------------
	  public virtual void test_basics()
	  {
		assertEquals(VOL_SIMPLE_MONEY_PRICE.ValuationDate, VAL_DATE_TIME.toLocalDate());
		assertEquals(VOL_SIMPLE_MONEY_PRICE.ValuationDateTime, VAL_DATE_TIME);
		assertEquals(VOL_SIMPLE_MONEY_PRICE.Index, EUR_EURIBOR_3M);
		assertEquals(VOL_SIMPLE_MONEY_PRICE.Name, IborFutureOptionVolatilitiesName.of("Price"));
	  }

	  public virtual void test_volatility_price()
	  {
		for (int i = 0; i < NB_TEST; i++)
		{
		  double timeToExpiry = VOL_SIMPLE_MONEY_RATE.relativeTime(TEST_EXPIRY[i]);
		  double volExpected = PARAMETERS_PRICE.zValue(timeToExpiry, TEST_STRIKE_PRICE[i] - TEST_FUTURE_PRICE[i]);
		  double volComputed = VOL_SIMPLE_MONEY_PRICE.volatility(TEST_EXPIRY[i], TEST_FIXING[i], TEST_STRIKE_PRICE[i], TEST_FUTURE_PRICE[i]);
		  assertEquals(volComputed, volExpected, TOLERANCE_VOL);
		}
	  }

	  public virtual void test_volatility_rate()
	  {
		for (int i = 0; i < NB_TEST; i++)
		{
		  double timeToExpiry = VOL_SIMPLE_MONEY_RATE.relativeTime(TEST_EXPIRY[i]);
		  double volExpected = PARAMETERS_RATE.zValue(timeToExpiry, TEST_FUTURE_PRICE[i] - TEST_STRIKE_PRICE[i]);
		  double volComputed = VOL_SIMPLE_MONEY_RATE.volatility(TEST_EXPIRY[i], TEST_FIXING[i], TEST_STRIKE_PRICE[i], TEST_FUTURE_PRICE[i]);
		  assertEquals(volComputed, volExpected, TOLERANCE_VOL);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parameterSensitivity()
	  {
		double expiry = ACT_365F.relativeYearFraction(VAL_DATE, LocalDate.of(2015, 8, 14));
		LocalDate fixing = LocalDate.of(2016, 9, 14);
		double strikePrice = 1.0025;
		double futurePrice = 0.9975;
		double sensitivity = 123456;
		IborFutureOptionSensitivity point = IborFutureOptionSensitivity.of(VOL_SIMPLE_MONEY_RATE.Name, expiry, fixing, strikePrice, futurePrice, EUR, sensitivity);
		CurrencyParameterSensitivities ps = VOL_SIMPLE_MONEY_RATE.parameterSensitivity(point);
		double shift = 1.0E-6;
		double v0 = VOL_SIMPLE_MONEY_RATE.volatility(expiry, fixing, strikePrice, futurePrice);
		for (int i = 0; i < NORMAL_VOL_RATES.size(); i++)
		{
		  DoubleArray v = NORMAL_VOL_RATES.with(i, NORMAL_VOL_RATES.get(i) + shift);
		  InterpolatedNodalSurface param = InterpolatedNodalSurface.of(Surfaces.normalVolatilityByExpirySimpleMoneyness("Rate", ACT_365F, MoneynessType.RATES), TIMES, MONEYNESS_RATES, v, INTERPOLATOR_2D);
		  NormalIborFutureOptionExpirySimpleMoneynessVolatilities vol = NormalIborFutureOptionExpirySimpleMoneynessVolatilities.of(EUR_EURIBOR_3M, VAL_DATE_TIME, param);
		  double vP = vol.volatility(expiry, fixing, strikePrice, futurePrice);
		  double s = ps.getSensitivity(PARAMETERS_RATE.Name, EUR).Sensitivity.get(i);
		  assertEquals(s, (vP - v0) / shift * sensitivity, TOLERANCE_DELTA);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		NormalIborFutureOptionExpirySimpleMoneynessVolatilities test = NormalIborFutureOptionExpirySimpleMoneynessVolatilities.of(EUR_EURIBOR_3M, VAL_DATE_TIME, PARAMETERS_RATE);
		coverImmutableBean(test);
		NormalIborFutureOptionExpirySimpleMoneynessVolatilities test2 = NormalIborFutureOptionExpirySimpleMoneynessVolatilities.of(EUR_EURIBOR_6M, VAL_DATE_TIME.plusDays(1), PARAMETERS_PRICE);
		coverBeanEquals(test, test2);
	  }

	}

}