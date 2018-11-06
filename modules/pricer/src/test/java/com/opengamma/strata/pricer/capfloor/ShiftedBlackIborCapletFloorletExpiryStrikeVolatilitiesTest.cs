using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
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
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.TIME_SQUARE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using SimpleStrike = com.opengamma.strata.market.option.SimpleStrike;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;
	using GenericVolatilitySurfaceYearFractionParameterMetadata = com.opengamma.strata.pricer.common.GenericVolatilitySurfaceYearFractionParameterMetadata;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;
	using PutCall = com.opengamma.strata.product.common.PutCall;

	/// <summary>
	/// Test <seealso cref="ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ShiftedBlackIborCapletFloorletExpiryStrikeVolatilitiesTest
	public class ShiftedBlackIborCapletFloorletExpiryStrikeVolatilitiesTest
	{

	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly DoubleArray TIME = DoubleArray.of(0.25, 0.25, 0.25, 0.25, 0.5, 0.5, 0.5, 0.5, 1.0, 1.0, 1.0, 1.0);
	  private static readonly DoubleArray STRIKE = DoubleArray.of(-0.005, 0.005, 0.01, 0.025, -0.005, 0.005, 0.01, 0.025, -0.005, 0.005, 0.01, 0.025);
	  private static readonly DoubleArray VOL = DoubleArray.of(0.14, 0.14, 0.13, 0.12, 0.12, 0.13, 0.12, 0.11, 0.1, 0.12, 0.11, 0.1);
	  private static readonly SurfaceMetadata METADATA;
	  static ShiftedBlackIborCapletFloorletExpiryStrikeVolatilitiesTest()
	  {
		IList<GenericVolatilitySurfaceYearFractionParameterMetadata> list = new List<GenericVolatilitySurfaceYearFractionParameterMetadata>();
		int nData = TIME.size();
		for (int i = 0; i < nData; ++i)
		{
		  GenericVolatilitySurfaceYearFractionParameterMetadata parameterMetadata = GenericVolatilitySurfaceYearFractionParameterMetadata.of(TIME.get(i), SimpleStrike.of(STRIKE.get(i)));
		  list.Add(parameterMetadata);
		}
		METADATA = Surfaces.blackVolatilityByExpiryStrike("CAP_VOL", ACT_365F).withParameterMetadata(list);
	  }
	  private static readonly InterpolatedNodalSurface SURFACE = InterpolatedNodalSurface.of(METADATA, TIME, STRIKE, VOL, INTERPOLATOR_2D);
	  private const double SHIFT = 0.02;
	  private static readonly ConstantCurve CURVE = ConstantCurve.of("shift parameter", SHIFT);
	  private static readonly LocalDate VAL_DATE = date(2015, 2, 17);
	  private static readonly LocalTime VAL_TIME = LocalTime.of(13, 45);
	  private static readonly ZoneId LONDON_ZONE = ZoneId.of("Europe/London");
	  private static readonly ZonedDateTime VAL_DATE_TIME = VAL_DATE.atTime(VAL_TIME).atZone(LONDON_ZONE);
	  private static readonly ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities VOLS = ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities.of(GBP_LIBOR_3M, VAL_DATE_TIME, SURFACE, CURVE);

	  private static readonly ZonedDateTime[] TEST_OPTION_EXPIRY = new ZonedDateTime[] {dateUtc(2015, 2, 17), dateUtc(2015, 5, 17), dateUtc(2015, 6, 17), dateUtc(2017, 2, 17)};
	  private static readonly int NB_TEST = TEST_OPTION_EXPIRY.Length;
	  private static readonly double[] TEST_STRIKE = new double[] {-0.01, 0.003, 0.016, 0.032};
	  private static readonly double[] TEST_SENSITIVITY = new double[] {1.0, -34.0, 12.0, 0.1};
	  private const double TEST_FORWARD = 0.015; // not used internally

	  private const double TOLERANCE_VOL = 1.0E-10;

	  public virtual void test_getter()
	  {
		assertEquals(VOLS.ValuationDate, VAL_DATE);
		assertEquals(VOLS.Index, GBP_LIBOR_3M);
		assertEquals(VOLS.Surface, SURFACE);
		assertEquals(VOLS.ParameterCount, TIME.size());
		assertEquals(VOLS.findData(CURVE.Name).get(), CURVE);
		assertEquals(VOLS.findData(SURFACE.Name).get(), SURFACE);
		assertFalse(VOLS.findData(CurveName.of("foo")).Present);
		int nParams = VOLS.ParameterCount;
		double newValue = 152d;
		for (int i = 0; i < nParams; ++i)
		{
		  assertEquals(VOLS.getParameter(i), SURFACE.getParameter(i));
		  assertEquals(VOLS.getParameterMetadata(i), SURFACE.getParameterMetadata(i));
		  assertEquals(VOLS.withParameter(i, newValue), ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities.of(GBP_LIBOR_3M, VAL_DATE_TIME, SURFACE.withParameter(i, newValue), CURVE));
		  assertEquals(VOLS.withPerturbation((n, v, m) => 2d * v), ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities.of(GBP_LIBOR_3M, VAL_DATE_TIME, SURFACE.withPerturbation((n, v, m) => 2d * v), CURVE));
		}
	  }

	  public virtual void test_price_formula()
	  {
		double sampleVol = 0.2;
		for (int i = 0; i < NB_TEST; i++)
		{
		  double expiryTime = VOLS.relativeTime(TEST_OPTION_EXPIRY[i]);
		  for (int j = 0; j < NB_TEST; j++)
		  {
			foreach (PutCall putCall in new PutCall[] {PutCall.CALL, PutCall.PUT})
			{
			  assertEquals(VOLS.price(expiryTime, putCall, TEST_STRIKE[j], TEST_FORWARD, sampleVol), BlackFormulaRepository.price(TEST_FORWARD + SHIFT, TEST_STRIKE[j] + SHIFT, expiryTime, sampleVol, putCall.Call));
			  assertEquals(VOLS.priceDelta(expiryTime, putCall, TEST_STRIKE[j], TEST_FORWARD, sampleVol), BlackFormulaRepository.delta(TEST_FORWARD + SHIFT, TEST_STRIKE[j] + SHIFT, expiryTime, sampleVol, putCall.Call));
			  assertEquals(VOLS.priceGamma(expiryTime, putCall, TEST_STRIKE[j], TEST_FORWARD, sampleVol), BlackFormulaRepository.gamma(TEST_FORWARD + SHIFT, TEST_STRIKE[j] + SHIFT, expiryTime, sampleVol));
			  assertEquals(VOLS.priceTheta(expiryTime, putCall, TEST_STRIKE[j], TEST_FORWARD, sampleVol), BlackFormulaRepository.driftlessTheta(TEST_FORWARD + SHIFT, TEST_STRIKE[j] + SHIFT, expiryTime, sampleVol));
			  assertEquals(VOLS.priceVega(expiryTime, putCall, TEST_STRIKE[j], TEST_FORWARD, sampleVol), BlackFormulaRepository.vega(TEST_FORWARD + SHIFT, TEST_STRIKE[j] + SHIFT, expiryTime, sampleVol));
			}
		  }
		}
	  }

	  public virtual void test_relativeTime()
	  {
		double test1 = VOLS.relativeTime(VAL_DATE_TIME);
		assertEquals(test1, 0d);
		double test2 = VOLS.relativeTime(date(2018, 2, 17).atStartOfDay(LONDON_ZONE));
		double test3 = VOLS.relativeTime(date(2012, 2, 17).atStartOfDay(LONDON_ZONE));
		assertEquals(test2, -test3); // consistency checked
	  }

	  public virtual void test_volatility()
	  {
		for (int i = 0; i < NB_TEST; i++)
		{
		  double expiryTime = VOLS.relativeTime(TEST_OPTION_EXPIRY[i]);
		  for (int j = 0; j < NB_TEST; ++j)
		  {
			double volExpected = SURFACE.zValue(expiryTime, TEST_STRIKE[j] + SHIFT);
			double volComputed = VOLS.volatility(TEST_OPTION_EXPIRY[i], TEST_STRIKE[j], TEST_FORWARD);
			assertEquals(volComputed, volExpected, TOLERANCE_VOL);
		  }
		}
	  }

	  public virtual void test_volatility_sensitivity()
	  {
		double eps = 1.0e-6;
		int nData = TIME.size();
		for (int i = 0; i < NB_TEST; i++)
		{
		  for (int k = 0; k < NB_TEST; k++)
		  {
			double expiryTime = VOLS.relativeTime(TEST_OPTION_EXPIRY[i]);
			IborCapletFloorletSensitivity point = IborCapletFloorletSensitivity.of(VOLS.Name, expiryTime, TEST_STRIKE[k], TEST_FORWARD, GBP, TEST_SENSITIVITY[i]);
			double[] sensFd = new double[nData];
			for (int j = 0; j < nData; j++)
			{
			  DoubleArray volDataUp = VOL.subArray(0, nData).with(j, VOL.get(j) + eps);
			  DoubleArray volDataDw = VOL.subArray(0, nData).with(j, VOL.get(j) - eps);
			  InterpolatedNodalSurface paramUp = InterpolatedNodalSurface.of(METADATA, TIME, STRIKE, volDataUp, INTERPOLATOR_2D);
			  InterpolatedNodalSurface paramDw = InterpolatedNodalSurface.of(METADATA, TIME, STRIKE, volDataDw, INTERPOLATOR_2D);
			  ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities provUp = ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities.of(GBP_LIBOR_3M, VAL_DATE_TIME, paramUp, CURVE);
			  ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities provDw = ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities.of(GBP_LIBOR_3M, VAL_DATE_TIME, paramDw, CURVE);
			  double volUp = provUp.volatility(TEST_OPTION_EXPIRY[i], TEST_STRIKE[k], TEST_FORWARD);
			  double volDw = provDw.volatility(TEST_OPTION_EXPIRY[i], TEST_STRIKE[k], TEST_FORWARD);
			  double fd = 0.5 * (volUp - volDw) / eps;
			  sensFd[j] = fd * TEST_SENSITIVITY[i];
			}
			CurrencyParameterSensitivity sensActual = VOLS.parameterSensitivity(point).Sensitivities.get(0);
			double[] computed = sensActual.Sensitivity.toArray();
			assertTrue(DoubleArrayMath.fuzzyEquals(computed, sensFd, eps));
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(VOLS);
		ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities vols = ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities.of(USD_LIBOR_3M, VAL_DATE_TIME.plusMonths(1), InterpolatedNodalSurface.of(METADATA, TIME, STRIKE, VOL, GridSurfaceInterpolator.of(TIME_SQUARE, LINEAR)), ConstantCurve.of("shift", 0.05));
		coverBeanEquals(VOLS, vols);
	  }

	}

}