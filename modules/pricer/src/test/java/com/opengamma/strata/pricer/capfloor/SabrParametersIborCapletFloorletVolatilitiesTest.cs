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
//	import static com.opengamma.strata.market.model.SabrParameterType.SHIFT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using SurfaceName = com.opengamma.strata.market.surface.SurfaceName;
	using SabrParameters = com.opengamma.strata.pricer.model.SabrParameters;

	/// <summary>
	/// Test <seealso cref="SabrParametersIborCapletFloorletVolatilities"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrParametersIborCapletFloorletVolatilitiesTest
	public class SabrParametersIborCapletFloorletVolatilitiesTest
	{

	  private static readonly LocalDate DATE = LocalDate.of(2014, 1, 3);
	  private static readonly LocalTime TIME = LocalTime.of(10, 0);
	  private static readonly ZoneId ZONE = ZoneId.of("Europe/London");
	  private static readonly ZonedDateTime DATE_TIME = DATE.atTime(TIME).atZone(ZONE);
	  private static readonly SabrParameters PARAM = IborCapletFloorletSabrRateVolatilityDataSet.SABR_PARAM;

	  private static readonly ZonedDateTime[] TEST_OPTION_EXPIRY = new ZonedDateTime[] {dateUtc(2014, 1, 3), dateUtc(2015, 1, 3), dateUtc(2016, 4, 21), dateUtc(2017, 1, 3)};
	  private static readonly int NB_TEST = TEST_OPTION_EXPIRY.Length;
	  private const double TEST_FORWARD = 0.025;
	  private static readonly double[] TEST_STRIKE = new double[] {0.02, 0.025, 0.03};
	  private static readonly int NB_STRIKE = TEST_STRIKE.Length;
	  internal static readonly IborCapletFloorletVolatilitiesName NAME = IborCapletFloorletSabrRateVolatilityDataSet.NAME;
	  internal static readonly IborCapletFloorletVolatilitiesName NAME2 = IborCapletFloorletVolatilitiesName.of("Test-SABR2");
	  private const double TOLERANCE_VOL = 1.0E-10;

	  public virtual void test_of()
	  {
		SabrParametersIborCapletFloorletVolatilities test = SabrParametersIborCapletFloorletVolatilities.of(NAME, EUR_EURIBOR_3M, DATE_TIME, PARAM);
		assertEquals(test.Index, EUR_EURIBOR_3M);
		assertEquals(test.DayCount, ACT_ACT_ISDA);
		assertEquals(test.Parameters, PARAM);
		assertEquals(test.ValuationDateTime, DATE_TIME);
		assertEquals(test.ParameterCount, PARAM.ParameterCount);
		int nParams = PARAM.ParameterCount;
		double newValue = 152d;
		for (int i = 0; i < nParams; ++i)
		{
		  assertEquals(test.getParameter(i), PARAM.getParameter(i));
		  assertEquals(test.getParameterMetadata(i), PARAM.getParameterMetadata(i));
		  assertEquals(test.withParameter(i, newValue), SabrParametersIborCapletFloorletVolatilities.of(NAME, EUR_EURIBOR_3M, DATE_TIME, PARAM.withParameter(i, newValue)));
		  assertEquals(test.withPerturbation((n, v, m) => 2d * v), SabrParametersIborCapletFloorletVolatilities.of(NAME, EUR_EURIBOR_3M, DATE_TIME, PARAM.withPerturbation((n, v, m) => 2d * v)));
		}
	  }

	  public virtual void test_findData()
	  {
		SabrParametersIborCapletFloorletVolatilities test = SabrParametersIborCapletFloorletVolatilities.of(NAME, EUR_EURIBOR_3M, DATE_TIME, PARAM);
		assertEquals(test.findData(PARAM.AlphaCurve.Name), PARAM.AlphaCurve);
		assertEquals(test.findData(PARAM.BetaCurve.Name), PARAM.BetaCurve);
		assertEquals(test.findData(PARAM.RhoCurve.Name), PARAM.RhoCurve);
		assertEquals(test.findData(PARAM.NuCurve.Name), PARAM.NuCurve);
		assertEquals(test.findData(PARAM.ShiftCurve.Name), PARAM.ShiftCurve);
		assertEquals(test.findData(SurfaceName.of("Rubbish")), null);
	  }

	  public virtual void test_calc()
	  {
		SabrParametersIborCapletFloorletVolatilities test = SabrParametersIborCapletFloorletVolatilities.of(NAME, EUR_EURIBOR_3M, DATE_TIME, PARAM);
		assertEquals(test.alpha(1.56), PARAM.alpha(1.56));
		assertEquals(test.beta(1.56), PARAM.beta(1.56));
		assertEquals(test.rho(1.56), PARAM.rho(1.56));
		assertEquals(test.nu(1.56), PARAM.nu(1.56));
		assertEquals(test.shift(1.56), PARAM.shift(1.56));
	  }

	  public virtual void test_relativeTime()
	  {
		SabrParametersIborCapletFloorletVolatilities prov = SabrParametersIborCapletFloorletVolatilities.of(NAME, EUR_EURIBOR_3M, DATE_TIME, PARAM);
		double test1 = prov.relativeTime(DATE_TIME);
		assertEquals(test1, 0d);
		double test2 = prov.relativeTime(DATE_TIME.plusYears(2));
		double test3 = prov.relativeTime(DATE_TIME.minusYears(2));
		assertEquals(test2, -test3, 1e-2);
	  }

	  public virtual void test_volatility()
	  {
		SabrParametersIborCapletFloorletVolatilities prov = SabrParametersIborCapletFloorletVolatilities.of(NAME, EUR_EURIBOR_3M, DATE_TIME, PARAM);
		for (int i = 0; i < NB_TEST; i++)
		{
		  for (int j = 0; j < NB_STRIKE; ++j)
		  {
			double expiryTime = prov.relativeTime(TEST_OPTION_EXPIRY[i]);
			double volExpected = PARAM.volatility(expiryTime, TEST_STRIKE[j], TEST_FORWARD);
			double volComputed = prov.volatility(TEST_OPTION_EXPIRY[i], TEST_STRIKE[j], TEST_FORWARD);
			assertEquals(volComputed, volExpected, TOLERANCE_VOL);
			ValueDerivatives volAdjExpected = PARAM.volatilityAdjoint(expiryTime, TEST_STRIKE[j], TEST_FORWARD);
			ValueDerivatives volAdjComputed = prov.volatilityAdjoint(expiryTime, TEST_STRIKE[j], TEST_FORWARD);
			assertEquals(volAdjComputed.Value, volExpected, TOLERANCE_VOL);
			assertTrue(DoubleArrayMath.fuzzyEquals(volAdjComputed.Derivatives.toArray(), volAdjExpected.Derivatives.toArray(), TOLERANCE_VOL));
		  }
		}
	  }

	  public virtual void test_parameterSensitivity()
	  {
		double alphaSensi = 2.24, betaSensi = 3.45, rhoSensi = -2.12, nuSensi = -0.56, shiftSensi = 2.5;
		SabrParametersIborCapletFloorletVolatilities prov = SabrParametersIborCapletFloorletVolatilities.of(NAME, EUR_EURIBOR_3M, DATE_TIME, PARAM);
		for (int i = 0; i < NB_TEST; i++)
		{
		  double expiryTime = prov.relativeTime(TEST_OPTION_EXPIRY[i]);
		  PointSensitivities point = PointSensitivities.of(IborCapletFloorletSabrSensitivity.of(NAME, expiryTime, ALPHA, EUR, alphaSensi), IborCapletFloorletSabrSensitivity.of(NAME, expiryTime, BETA, EUR, betaSensi), IborCapletFloorletSabrSensitivity.of(NAME, expiryTime, RHO, EUR, rhoSensi), IborCapletFloorletSabrSensitivity.of(NAME, expiryTime, NU, EUR, nuSensi), IborCapletFloorletSabrSensitivity.of(NAME, expiryTime, SHIFT, EUR, shiftSensi));
		  CurrencyParameterSensitivities sensiComputed = prov.parameterSensitivity(point);
		  UnitParameterSensitivity alphaSensitivities = prov.Parameters.AlphaCurve.yValueParameterSensitivity(expiryTime);
		  UnitParameterSensitivity betaSensitivities = prov.Parameters.BetaCurve.yValueParameterSensitivity(expiryTime);
		  UnitParameterSensitivity rhoSensitivities = prov.Parameters.RhoCurve.yValueParameterSensitivity(expiryTime);
		  UnitParameterSensitivity nuSensitivities = prov.Parameters.NuCurve.yValueParameterSensitivity(expiryTime);
		  UnitParameterSensitivity shiftSensitivities = prov.Parameters.ShiftCurve.yValueParameterSensitivity(expiryTime);
		  CurrencyParameterSensitivity alphaSensiObj = sensiComputed.getSensitivity(IborCapletFloorletSabrRateVolatilityDataSet.META_ALPHA.CurveName, EUR);
		  CurrencyParameterSensitivity betaSensiObj = sensiComputed.getSensitivity(IborCapletFloorletSabrRateVolatilityDataSet.META_BETA.CurveName, EUR);
		  CurrencyParameterSensitivity rhoSensiObj = sensiComputed.getSensitivity(IborCapletFloorletSabrRateVolatilityDataSet.META_RHO.CurveName, EUR);
		  CurrencyParameterSensitivity nuSensiObj = sensiComputed.getSensitivity(IborCapletFloorletSabrRateVolatilityDataSet.META_NU.CurveName, EUR);
		  CurrencyParameterSensitivity shiftSensiObj = sensiComputed.getSensitivity(IborCapletFloorletSabrRateVolatilityDataSet.META_SHIFT.CurveName, EUR);
		  DoubleArray alphaNodeSensiComputed = alphaSensiObj.Sensitivity;
		  DoubleArray betaNodeSensiComputed = betaSensiObj.Sensitivity;
		  DoubleArray rhoNodeSensiComputed = rhoSensiObj.Sensitivity;
		  DoubleArray nuNodeSensiComputed = nuSensiObj.Sensitivity;
		  DoubleArray shiftNodeSensiComputed = shiftSensiObj.Sensitivity;
		  assertEquals(alphaSensitivities.Sensitivity.size(), alphaNodeSensiComputed.size());
		  assertEquals(betaSensitivities.Sensitivity.size(), betaNodeSensiComputed.size());
		  assertEquals(rhoSensitivities.Sensitivity.size(), rhoNodeSensiComputed.size());
		  assertEquals(nuSensitivities.Sensitivity.size(), nuNodeSensiComputed.size());
		  assertEquals(shiftSensitivities.Sensitivity.size(), shiftNodeSensiComputed.size());
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
		  for (int k = 0; k < shiftNodeSensiComputed.size(); ++k)
		  {
			assertEquals(shiftNodeSensiComputed.get(k), shiftSensitivities.Sensitivity.get(k) * shiftSensi, TOLERANCE_VOL);
		  }
		}
	  }

	  public virtual void test_parameterSensitivity_multi()
	  {
		double[] points1 = new double[] {2.24, 3.45, -2.12, -0.56};
		double[] points2 = new double[] {-0.145, 1.01, -5.0, -11.0};
		double[] points3 = new double[] {1.3, -4.32, 2.1, -7.18};
		SabrParametersIborCapletFloorletVolatilities prov = SabrParametersIborCapletFloorletVolatilities.of(NAME, EUR_EURIBOR_3M, DATE_TIME, PARAM);
		double expiryTime0 = prov.relativeTime(TEST_OPTION_EXPIRY[0]);
		double expiryTime3 = prov.relativeTime(TEST_OPTION_EXPIRY[3]);
		for (int i = 0; i < NB_TEST; i++)
		{
		  PointSensitivities sensi1 = PointSensitivities.of(IborCapletFloorletSabrSensitivity.of(NAME, expiryTime0, ALPHA, EUR, points1[0]), IborCapletFloorletSabrSensitivity.of(NAME, expiryTime0, BETA, EUR, points1[1]), IborCapletFloorletSabrSensitivity.of(NAME, expiryTime0, RHO, EUR, points1[2]), IborCapletFloorletSabrSensitivity.of(NAME, expiryTime0, NU, EUR, points1[3]));
		  PointSensitivities sensi2 = PointSensitivities.of(IborCapletFloorletSabrSensitivity.of(NAME, expiryTime0, ALPHA, EUR, points2[0]), IborCapletFloorletSabrSensitivity.of(NAME, expiryTime0, BETA, EUR, points2[1]), IborCapletFloorletSabrSensitivity.of(NAME, expiryTime0, RHO, EUR, points2[2]), IborCapletFloorletSabrSensitivity.of(NAME, expiryTime0, NU, EUR, points2[3]));
		  PointSensitivities sensi3 = PointSensitivities.of(IborCapletFloorletSabrSensitivity.of(NAME, expiryTime3, ALPHA, EUR, points3[0]), IborCapletFloorletSabrSensitivity.of(NAME, expiryTime3, BETA, EUR, points3[1]), IborCapletFloorletSabrSensitivity.of(NAME, expiryTime3, RHO, EUR, points3[2]), IborCapletFloorletSabrSensitivity.of(NAME, expiryTime3, NU, EUR, points3[3]));
		  PointSensitivities sensis = sensi1.combinedWith(sensi2).combinedWith(sensi3).normalized();
		  CurrencyParameterSensitivities computed = prov.parameterSensitivity(sensis);
		  CurrencyParameterSensitivities expected = prov.parameterSensitivity(sensi1).combinedWith(prov.parameterSensitivity(sensi2)).combinedWith(prov.parameterSensitivity(sensi3));
		  DoubleArrayMath.fuzzyEquals(computed.getSensitivity(PARAM.AlphaCurve.Name, EUR).Sensitivity.toArray(), expected.getSensitivity(PARAM.AlphaCurve.Name, EUR).Sensitivity.toArray(), TOLERANCE_VOL);
		  DoubleArrayMath.fuzzyEquals(computed.getSensitivity(PARAM.BetaCurve.Name, EUR).Sensitivity.toArray(), expected.getSensitivity(PARAM.BetaCurve.Name, EUR).Sensitivity.toArray(), TOLERANCE_VOL);
		  DoubleArrayMath.fuzzyEquals(computed.getSensitivity(PARAM.RhoCurve.Name, EUR).Sensitivity.toArray(), expected.getSensitivity(PARAM.RhoCurve.Name, EUR).Sensitivity.toArray(), TOLERANCE_VOL);
		  DoubleArrayMath.fuzzyEquals(computed.getSensitivity(PARAM.NuCurve.Name, EUR).Sensitivity.toArray(), expected.getSensitivity(PARAM.NuCurve.Name, EUR).Sensitivity.toArray(), TOLERANCE_VOL);
		}
	  }

	  public virtual void coverage()
	  {
		SabrParametersIborCapletFloorletVolatilities test1 = SabrParametersIborCapletFloorletVolatilities.of(NAME, EUR_EURIBOR_3M, DATE_TIME, PARAM);
		coverImmutableBean(test1);
		SabrParametersIborCapletFloorletVolatilities test2 = SabrParametersIborCapletFloorletVolatilities.of(NAME2, IborIndices.EUR_LIBOR_3M, DATE_TIME.plusDays(1), IborCapletFloorletSabrRateVolatilityDataSet.SABR_PARAM_FLAT);
		coverBeanEquals(test1, test2);
	  }

	}

}