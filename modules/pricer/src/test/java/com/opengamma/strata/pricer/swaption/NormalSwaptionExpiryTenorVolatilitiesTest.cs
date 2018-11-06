using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{
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
//	import static com.opengamma.strata.collect.TestHelper.dateUtc;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoublesPair = com.opengamma.strata.collect.tuple.DoublesPair;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using SurfaceName = com.opengamma.strata.market.surface.SurfaceName;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;
	using FixedIborSwapConvention = com.opengamma.strata.product.swap.type.FixedIborSwapConvention;
	using FixedIborSwapConventions = com.opengamma.strata.product.swap.type.FixedIborSwapConventions;

	/// <summary>
	/// Test <seealso cref="NormalSwaptionExpiryTenorVolatilities"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NormalSwaptionExpiryTenorVolatilitiesTest
	public class NormalSwaptionExpiryTenorVolatilitiesTest
	{

	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly DoubleArray TIME = DoubleArray.of(0.25, 0.25, 0.25, 0.25, 0.5, 0.5, 0.5, 0.5, 1, 1, 1, 1);
	  private static readonly DoubleArray TENOR = DoubleArray.of(3, 5, 7, 10, 3, 5, 7, 10, 3, 5, 7, 10);
	  private static readonly DoubleArray VOL = DoubleArray.of(0.14, 0.14, 0.13, 0.12, 0.12, 0.13, 0.12, 0.11, 0.1, 0.12, 0.11, 0.1);

	  private static readonly FixedIborSwapConvention CONVENTION = FixedIborSwapConventions.GBP_FIXED_1Y_LIBOR_3M;
	  private static readonly SurfaceMetadata METADATA;
	  static NormalSwaptionExpiryTenorVolatilitiesTest()
	  {
		IList<SwaptionSurfaceExpiryTenorParameterMetadata> list = new List<SwaptionSurfaceExpiryTenorParameterMetadata>();
		int nData = TIME.size();
		for (int i = 0; i < nData; ++i)
		{
		  SwaptionSurfaceExpiryTenorParameterMetadata parameterMetadata = SwaptionSurfaceExpiryTenorParameterMetadata.of(TIME.get(i), TENOR.get(i));
		  list.Add(parameterMetadata);
		}
		METADATA = Surfaces.normalVolatilityByExpiryTenor("GOVT1-SWAPTION-VOL", ACT_365F).withParameterMetadata(list);
	  }
	  private static readonly InterpolatedNodalSurface SURFACE = InterpolatedNodalSurface.of(METADATA, TIME, TENOR, VOL, INTERPOLATOR_2D);
	  private static readonly LocalDate VAL_DATE = date(2015, 2, 17);
	  private static readonly LocalTime VAL_TIME = LocalTime.of(13, 45);
	  private static readonly ZoneId LONDON_ZONE = ZoneId.of("Europe/London");
	  private static readonly ZonedDateTime VAL_DATE_TIME = VAL_DATE.atTime(VAL_TIME).atZone(LONDON_ZONE);
	  private static readonly NormalSwaptionExpiryTenorVolatilities VOLS = NormalSwaptionExpiryTenorVolatilities.of(CONVENTION, VAL_DATE_TIME, SURFACE);

	  private static readonly ZonedDateTime[] TEST_OPTION_EXPIRY = new ZonedDateTime[] {dateUtc(2015, 2, 17), dateUtc(2015, 5, 17), dateUtc(2015, 6, 17), dateUtc(2017, 2, 17)};
	  private static readonly int NB_TEST = TEST_OPTION_EXPIRY.Length;
	  private static readonly double[] TEST_TENOR = new double[] {2.0, 6.0, 7.0, 15.0};
	  private static readonly double[] TEST_SENSITIVITY = new double[] {1.0, 1.0, 1.0, 1.0};
	  private const double TEST_FORWARD = 0.025; // not used internally
	  private const double TEST_STRIKE = 0.03; // not used internally

	  private const double TOLERANCE_VOL = 1.0E-10;

	  //-------------------------------------------------------------------------
	  public virtual void test_valuationDate()
	  {
		assertEquals(VOLS.ValuationDateTime, VAL_DATE_TIME);
	  }

	  public virtual void test_swapConvention()
	  {
		assertEquals(VOLS.Convention, CONVENTION);
	  }

	  public virtual void test_findData()
	  {
		assertEquals(VOLS.findData(SURFACE.Name), SURFACE);
		assertEquals(VOLS.findData(SurfaceName.of("Rubbish")), null);
	  }

	  public virtual void test_tenor()
	  {
		double test1 = VOLS.tenor(VAL_DATE, VAL_DATE);
		assertEquals(test1, 0d);
		double test2 = VOLS.tenor(VAL_DATE, date(2018, 2, 28));
		assertEquals(test2, 3d);
		double test3 = VOLS.tenor(VAL_DATE, date(2018, 2, 10));
		assertEquals(test3, 3d);
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
		  double volExpected = SURFACE.zValue(expiryTime, TEST_TENOR[i]);
		  double volComputed = VOLS.volatility(TEST_OPTION_EXPIRY[i], TEST_TENOR[i], TEST_STRIKE, TEST_FORWARD);
		  assertEquals(volComputed, volExpected, TOLERANCE_VOL);
		}
	  }

	  public virtual void test_volatility_sensitivity()
	  {
		double eps = 1.0e-6;
		int nData = TIME.size();
		for (int i = 0; i < NB_TEST; i++)
		{
		  double expiryTime = VOLS.relativeTime(TEST_OPTION_EXPIRY[i]);
		  SwaptionSensitivity point = SwaptionSensitivity.of(VOLS.Name, expiryTime, TEST_TENOR[i], TEST_STRIKE, TEST_FORWARD, GBP, TEST_SENSITIVITY[i]);
		  CurrencyParameterSensitivities sensActual = VOLS.parameterSensitivity(point);
		  CurrencyParameterSensitivity sensi = sensActual.getSensitivity(SURFACE.Name, GBP);
		  DoubleArray computed = sensi.Sensitivity;

		  IDictionary<DoublesPair, double> map = new Dictionary<DoublesPair, double>();
		  for (int j = 0; j < nData; ++j)
		  {
			DoubleArray volDataUp = VOL.subArray(0, nData).with(j, VOL.get(j) + eps);
			DoubleArray volDataDw = VOL.subArray(0, nData).with(j, VOL.get(j) - eps);
			InterpolatedNodalSurface paramUp = InterpolatedNodalSurface.of(METADATA, TIME, TENOR, volDataUp, INTERPOLATOR_2D);
			InterpolatedNodalSurface paramDw = InterpolatedNodalSurface.of(METADATA, TIME, TENOR, volDataDw, INTERPOLATOR_2D);
			NormalSwaptionExpiryTenorVolatilities provUp = NormalSwaptionExpiryTenorVolatilities.of(CONVENTION, VAL_DATE_TIME, paramUp);
			NormalSwaptionExpiryTenorVolatilities provDw = NormalSwaptionExpiryTenorVolatilities.of(CONVENTION, VAL_DATE_TIME, paramDw);
			double volUp = provUp.volatility(TEST_OPTION_EXPIRY[i], TEST_TENOR[i], TEST_STRIKE, TEST_FORWARD);
			double volDw = provDw.volatility(TEST_OPTION_EXPIRY[i], TEST_TENOR[i], TEST_STRIKE, TEST_FORWARD);
			double fd = 0.5 * (volUp - volDw) / eps;
			map[DoublesPair.of(TIME.get(j), TENOR.get(j))] = fd;
		  }
		  IList<ParameterMetadata> list = sensi.ParameterMetadata;
		  assertEquals(computed.size(), nData);
		  for (int j = 0; j < list.Count; ++j)
		  {
			SwaptionSurfaceExpiryTenorParameterMetadata metadata = (SwaptionSurfaceExpiryTenorParameterMetadata) list[i];
			double expected = map[DoublesPair.of(metadata.YearFraction, metadata.Tenor)];
			assertEquals(computed.get(i), expected, eps);
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		NormalSwaptionExpiryTenorVolatilities test1 = NormalSwaptionExpiryTenorVolatilities.of(CONVENTION, VAL_DATE_TIME, SURFACE);
		coverImmutableBean(test1);
		NormalSwaptionExpiryTenorVolatilities test2 = NormalSwaptionExpiryTenorVolatilities.of(CONVENTION, VAL_DATE.atStartOfDay(ZoneOffset.UTC), SURFACE);
		coverBeanEquals(test1, test2);
	  }

	}

}