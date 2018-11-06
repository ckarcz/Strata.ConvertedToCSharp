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
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.dateUtc;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.model.SabrParameterType.ALPHA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.model.SabrParameterType.BETA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.model.SabrParameterType.NU;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.model.SabrParameterType.RHO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ShiftType = com.opengamma.strata.market.ShiftType;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterizedData = com.opengamma.strata.market.param.ParameterizedData;
	using PointShifts = com.opengamma.strata.market.param.PointShifts;
	using PointShiftsBuilder = com.opengamma.strata.market.param.PointShiftsBuilder;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using SurfaceName = com.opengamma.strata.market.surface.SurfaceName;
	using SabrInterestRateParameters = com.opengamma.strata.pricer.model.SabrInterestRateParameters;
	using FixedIborSwapConvention = com.opengamma.strata.product.swap.type.FixedIborSwapConvention;

	/// <summary>
	/// Test <seealso cref="SabrSwaptionVolatilities"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrSwaptionVolatilitiesTest
	public class SabrSwaptionVolatilitiesTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate DATE = LocalDate.of(2014, 1, 3);
	  private static readonly LocalTime TIME = LocalTime.of(10, 0);
	  private static readonly ZoneId ZONE = ZoneId.of("Europe/London");
	  private static readonly ZonedDateTime DATE_TIME = DATE.atTime(TIME).atZone(ZONE);
	  private static readonly SabrInterestRateParameters PARAM = SwaptionSabrRateVolatilityDataSet.SABR_PARAM_SHIFT_USD;
	  private static readonly FixedIborSwapConvention CONV = SwaptionSabrRateVolatilityDataSet.SWAP_CONVENTION_USD;

	  private static readonly ZonedDateTime[] TEST_OPTION_EXPIRY = new ZonedDateTime[] {dateUtc(2014, 1, 3), dateUtc(2014, 1, 3), dateUtc(2015, 1, 3), dateUtc(2017, 1, 3)};
	  private static readonly int NB_TEST = TEST_OPTION_EXPIRY.Length;
	  private static readonly double[] TEST_TENOR = new double[] {2.0, 6.0, 7.0, 15.0};
	  private const double TEST_FORWARD = 0.025;
	  private static readonly double[] TEST_STRIKE = new double[] {0.02, 0.025, 0.03};
	  private static readonly int NB_STRIKE = TEST_STRIKE.Length;
	  internal static readonly SwaptionVolatilitiesName NAME = SwaptionVolatilitiesName.of("Test-SABR");
	  internal static readonly SwaptionVolatilitiesName NAME2 = SwaptionVolatilitiesName.of("Test-SABR2");

	  private const double TOLERANCE_VOL = 1.0E-10;

	  public virtual void test_of()
	  {
		SabrParametersSwaptionVolatilities test = SabrParametersSwaptionVolatilities.of(NAME, CONV, DATE_TIME, PARAM);
		assertEquals(test.Convention, CONV);
		assertEquals(test.DayCount, ACT_ACT_ISDA);
		assertEquals(test.Parameters, PARAM);
		assertEquals(test.ValuationDateTime, DATE_TIME);
	  }

	  public virtual void test_findData()
	  {
		SabrParametersSwaptionVolatilities test = SabrParametersSwaptionVolatilities.of(NAME, CONV, DATE_TIME, PARAM);
		assertEquals(test.findData(PARAM.AlphaSurface.Name), PARAM.AlphaSurface);
		assertEquals(test.findData(PARAM.BetaSurface.Name), PARAM.BetaSurface);
		assertEquals(test.findData(PARAM.RhoSurface.Name), PARAM.RhoSurface);
		assertEquals(test.findData(PARAM.NuSurface.Name), PARAM.NuSurface);
		assertEquals(test.findData(PARAM.ShiftSurface.Name), PARAM.ShiftSurface);
		assertEquals(test.findData(SurfaceName.of("Rubbish")), null);
	  }

	  public virtual void test_calc()
	  {
		SabrParametersSwaptionVolatilities test = SabrParametersSwaptionVolatilities.of(NAME, CONV, DATE_TIME, PARAM);
		assertEquals(test.alpha(1d, 2d), PARAM.alpha(1d, 2d));
		assertEquals(test.beta(1d, 2d), PARAM.beta(1d, 2d));
		assertEquals(test.rho(1d, 2d), PARAM.rho(1d, 2d));
		assertEquals(test.nu(1d, 2d), PARAM.nu(1d, 2d));
		assertEquals(test.shift(1d, 2d), PARAM.shift(1d, 2d));
	  }

	  public virtual void test_tenor()
	  {
		SabrParametersSwaptionVolatilities prov = SabrParametersSwaptionVolatilities.of(NAME, CONV, DATE_TIME, PARAM);
		double test1 = prov.tenor(DATE, DATE);
		assertEquals(test1, 0d);
		double test2 = prov.tenor(DATE, DATE.plusYears(2));
		double test3 = prov.tenor(DATE, DATE.minusYears(2));
		assertEquals(test2, -test3);
		double test4 = prov.tenor(DATE, LocalDate.of(2019, 2, 2));
		double test5 = prov.tenor(DATE, LocalDate.of(2018, 12, 31));
		assertEquals(test4, 5d);
		assertEquals(test5, 5d);
	  }

	  public virtual void test_relativeTime()
	  {
		SabrParametersSwaptionVolatilities prov = SabrParametersSwaptionVolatilities.of(NAME, CONV, DATE_TIME, PARAM);
		double test1 = prov.relativeTime(DATE_TIME);
		assertEquals(test1, 0d);
		double test2 = prov.relativeTime(DATE_TIME.plusYears(2));
		double test3 = prov.relativeTime(DATE_TIME.minusYears(2));
		assertEquals(test2, -test3, 1e-2);
	  }

	  public virtual void test_volatility()
	  {
		SabrParametersSwaptionVolatilities prov = SabrParametersSwaptionVolatilities.of(NAME, CONV, DATE_TIME, PARAM);
		for (int i = 0; i < NB_TEST; i++)
		{
		  for (int j = 0; j < NB_STRIKE; ++j)
		  {
			double expiryTime = prov.relativeTime(TEST_OPTION_EXPIRY[i]);
			double volExpected = PARAM.volatility(expiryTime, TEST_TENOR[i], TEST_STRIKE[j], TEST_FORWARD);
			double volComputed = prov.volatility(TEST_OPTION_EXPIRY[i], TEST_TENOR[i], TEST_STRIKE[j], TEST_FORWARD);
			assertEquals(volComputed, volExpected, TOLERANCE_VOL);
		  }
		}
	  }

	  public virtual void test_parameterSensitivity()
	  {
		double alphaSensi = 2.24, betaSensi = 3.45, rhoSensi = -2.12, nuSensi = -0.56;
		SabrParametersSwaptionVolatilities prov = SabrParametersSwaptionVolatilities.of(NAME, CONV, DATE_TIME, PARAM);
		for (int i = 0; i < NB_TEST; i++)
		{
		  double expiryTime = prov.relativeTime(TEST_OPTION_EXPIRY[i]);
		  PointSensitivities point = PointSensitivities.of(SwaptionSabrSensitivity.of(NAME, expiryTime, TEST_TENOR[i], ALPHA, USD, alphaSensi), SwaptionSabrSensitivity.of(NAME, expiryTime, TEST_TENOR[i], BETA, USD, betaSensi), SwaptionSabrSensitivity.of(NAME, expiryTime, TEST_TENOR[i], RHO, USD, rhoSensi), SwaptionSabrSensitivity.of(NAME, expiryTime, TEST_TENOR[i], NU, USD, nuSensi));
		  CurrencyParameterSensitivities sensiComputed = prov.parameterSensitivity(point);
		  UnitParameterSensitivity alphaSensitivities = prov.Parameters.AlphaSurface.zValueParameterSensitivity(expiryTime, TEST_TENOR[i]);
		  UnitParameterSensitivity betaSensitivities = prov.Parameters.BetaSurface.zValueParameterSensitivity(expiryTime, TEST_TENOR[i]);
		  UnitParameterSensitivity rhoSensitivities = prov.Parameters.RhoSurface.zValueParameterSensitivity(expiryTime, TEST_TENOR[i]);
		  UnitParameterSensitivity nuSensitivities = prov.Parameters.NuSurface.zValueParameterSensitivity(expiryTime, TEST_TENOR[i]);
		  CurrencyParameterSensitivity alphaSensiObj = sensiComputed.getSensitivity(SwaptionSabrRateVolatilityDataSet.META_ALPHA.SurfaceName, USD);
		  CurrencyParameterSensitivity betaSensiObj = sensiComputed.getSensitivity(SwaptionSabrRateVolatilityDataSet.META_BETA_USD.SurfaceName, USD);
		  CurrencyParameterSensitivity rhoSensiObj = sensiComputed.getSensitivity(SwaptionSabrRateVolatilityDataSet.META_RHO.SurfaceName, USD);
		  CurrencyParameterSensitivity nuSensiObj = sensiComputed.getSensitivity(SwaptionSabrRateVolatilityDataSet.META_NU.SurfaceName, USD);
		  DoubleArray alphaNodeSensiComputed = alphaSensiObj.Sensitivity;
		  DoubleArray betaNodeSensiComputed = betaSensiObj.Sensitivity;
		  DoubleArray rhoNodeSensiComputed = rhoSensiObj.Sensitivity;
		  DoubleArray nuNodeSensiComputed = nuSensiObj.Sensitivity;
		  assertEquals(alphaSensitivities.Sensitivity.size(), alphaNodeSensiComputed.size());
		  assertEquals(betaSensitivities.Sensitivity.size(), betaNodeSensiComputed.size());
		  assertEquals(rhoSensitivities.Sensitivity.size(), rhoNodeSensiComputed.size());
		  assertEquals(nuSensitivities.Sensitivity.size(), nuNodeSensiComputed.size());
		  for (int k = 0; k < alphaNodeSensiComputed.size(); ++k)
		  {
			assertEquals(alphaNodeSensiComputed.get(k), alphaSensitivities.Sensitivity.get(k) * alphaSensi, TOLERANCE_VOL);
		  }
		  for (int k = 0; k < betaNodeSensiComputed.size(); ++k)
		  {
			assertEquals(betaNodeSensiComputed.get(k), betaSensitivities.Sensitivity.get(k) * betaSensi, TOLERANCE_VOL);
		  }
		  for (int k = 0; k < rhoNodeSensiComputed.size(); ++k)
		  {
			assertEquals(rhoNodeSensiComputed.get(k), rhoSensitivities.Sensitivity.get(k) * rhoSensi, TOLERANCE_VOL);
		  }
		  for (int k = 0; k < nuNodeSensiComputed.size(); ++k)
		  {
			assertEquals(nuNodeSensiComputed.get(k), nuSensitivities.Sensitivity.get(k) * nuSensi, TOLERANCE_VOL);
		  }
		}
	  }

	  public virtual void test_parameterSensitivity_multi()
	  {
		double[] points1 = new double[] {2.24, 3.45, -2.12, -0.56};
		double[] points2 = new double[] {-0.145, 1.01, -5.0, -11.0};
		double[] points3 = new double[] {1.3, -4.32, 2.1, -7.18};
		SabrParametersSwaptionVolatilities prov = SabrParametersSwaptionVolatilities.of(NAME, CONV, DATE_TIME, PARAM);
		double expiryTime0 = prov.relativeTime(TEST_OPTION_EXPIRY[0]);
		double expiryTime3 = prov.relativeTime(TEST_OPTION_EXPIRY[3]);
		for (int i = 0; i < NB_TEST; i++)
		{
		  PointSensitivities sensi1 = PointSensitivities.of(SwaptionSabrSensitivity.of(NAME, expiryTime0, TEST_TENOR[i], ALPHA, USD, points1[0]), SwaptionSabrSensitivity.of(NAME, expiryTime0, TEST_TENOR[i], BETA, USD, points1[1]), SwaptionSabrSensitivity.of(NAME, expiryTime0, TEST_TENOR[i], RHO, USD, points1[2]), SwaptionSabrSensitivity.of(NAME, expiryTime0, TEST_TENOR[i], NU, USD, points1[3]));
		  PointSensitivities sensi2 = PointSensitivities.of(SwaptionSabrSensitivity.of(NAME, expiryTime0, TEST_TENOR[i], ALPHA, USD, points2[0]), SwaptionSabrSensitivity.of(NAME, expiryTime0, TEST_TENOR[i], BETA, USD, points2[1]), SwaptionSabrSensitivity.of(NAME, expiryTime0, TEST_TENOR[i], RHO, USD, points2[2]), SwaptionSabrSensitivity.of(NAME, expiryTime0, TEST_TENOR[i], NU, USD, points2[3]));
		  PointSensitivities sensi3 = PointSensitivities.of(SwaptionSabrSensitivity.of(NAME, expiryTime3, TEST_TENOR[i], ALPHA, USD, points3[0]), SwaptionSabrSensitivity.of(NAME, expiryTime3, TEST_TENOR[i], BETA, USD, points3[1]), SwaptionSabrSensitivity.of(NAME, expiryTime3, TEST_TENOR[i], RHO, USD, points3[2]), SwaptionSabrSensitivity.of(NAME, expiryTime3, TEST_TENOR[i], NU, USD, points3[3]));
		  PointSensitivities sensis = sensi1.combinedWith(sensi2).combinedWith(sensi3).normalized();
		  CurrencyParameterSensitivities computed = prov.parameterSensitivity(sensis);
		  CurrencyParameterSensitivities expected = prov.parameterSensitivity(sensi1).combinedWith(prov.parameterSensitivity(sensi2)).combinedWith(prov.parameterSensitivity(sensi3));
		  DoubleArrayMath.fuzzyEquals(computed.getSensitivity(PARAM.AlphaSurface.Name, USD).Sensitivity.toArray(), expected.getSensitivity(PARAM.AlphaSurface.Name, USD).Sensitivity.toArray(), TOLERANCE_VOL);
		  DoubleArrayMath.fuzzyEquals(computed.getSensitivity(PARAM.BetaSurface.Name, USD).Sensitivity.toArray(), expected.getSensitivity(PARAM.BetaSurface.Name, USD).Sensitivity.toArray(), TOLERANCE_VOL);
		  DoubleArrayMath.fuzzyEquals(computed.getSensitivity(PARAM.RhoSurface.Name, USD).Sensitivity.toArray(), expected.getSensitivity(PARAM.RhoSurface.Name, USD).Sensitivity.toArray(), TOLERANCE_VOL);
		  DoubleArrayMath.fuzzyEquals(computed.getSensitivity(PARAM.NuSurface.Name, USD).Sensitivity.toArray(), expected.getSensitivity(PARAM.NuSurface.Name, USD).Sensitivity.toArray(), TOLERANCE_VOL);
		}
	  }

	  public virtual void test_pointShifts()
	  {
		SabrParametersSwaptionVolatilities @base = SabrParametersSwaptionVolatilities.of(NAME, CONV, DATE_TIME, PARAM);
		PointShiftsBuilder builder = PointShifts.builder(ShiftType.ABSOLUTE);
		for (int i = 0; i < @base.ParameterCount; ++i)
		{
		  builder.addShift(0, @base.getParameterMetadata(i).Identifier, 0.1d * (i + 1d));
		  builder.addShift(1, @base.getParameterMetadata(i).Identifier, 10d * (i + 1d));
		}
		PointShifts shifts = builder.build();
		MarketDataBox<ParameterizedData> resBox = shifts.applyTo(MarketDataBox.ofSingleValue(@base), REF_DATA);
		SabrParametersSwaptionVolatilities computed0 = (SabrParametersSwaptionVolatilities) resBox.getValue(0);
		SabrParametersSwaptionVolatilities computed1 = (SabrParametersSwaptionVolatilities) resBox.getValue(1);
		for (int i = 0; i < @base.ParameterCount; ++i)
		{
		  assertEquals(computed0.getParameter(i), @base.getParameter(i) + 0.1d * (i + 1d));
		  assertEquals(computed1.getParameter(i), @base.getParameter(i) + 10d * (i + 1d));
		}
	  }

	  public virtual void coverage()
	  {
		SabrParametersSwaptionVolatilities test1 = SabrParametersSwaptionVolatilities.of(NAME, CONV, DATE_TIME, PARAM);
		coverImmutableBean(test1);
		SabrParametersSwaptionVolatilities test2 = SabrParametersSwaptionVolatilities.of(NAME2, SwaptionSabrRateVolatilityDataSet.SWAP_CONVENTION_EUR, DATE_TIME.plusDays(1), SwaptionSabrRateVolatilityDataSet.SABR_PARAM_USD);
		coverBeanEquals(test1, test2);
	  }
	}

}