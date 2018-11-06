/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using LegalEntityGroup = com.opengamma.strata.market.curve.LegalEntityGroup;
	using RepoGroup = com.opengamma.strata.market.curve.RepoGroup;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using LegalEntityId = com.opengamma.strata.product.LegalEntityId;
	using SecurityId = com.opengamma.strata.product.SecurityId;

	/// <summary>
	/// Test <seealso cref="ImmutableLegalEntityDiscountingProvider"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ImmutableLegalEntityDiscountingProviderTest
	public class ImmutableLegalEntityDiscountingProviderTest
	{

	  private static readonly LocalDate DATE = date(2015, 6, 4);
	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;

	  private static readonly CurveName NAME_REPO = CurveName.of("TestRepoCurve");
	  private static readonly CurveMetadata METADATA_REPO = Curves.zeroRates(NAME_REPO, ACT_365F);
	  private static readonly InterpolatedNodalCurve CURVE_REPO = InterpolatedNodalCurve.of(METADATA_REPO, DoubleArray.of(0, 10), DoubleArray.of(1, 2), INTERPOLATOR);
	  private static readonly ZeroRateDiscountFactors DSC_FACTORS_REPO = ZeroRateDiscountFactors.of(GBP, DATE, CURVE_REPO);
	  private static readonly RepoGroup GROUP_REPO_SECURITY = RepoGroup.of("ISSUER1 BND 5Y");
	  private static readonly RepoGroup GROUP_REPO_ISSUER = RepoGroup.of("ISSUER1");
	  private static readonly SecurityId ID_SECURITY = SecurityId.of("OG-Ticker", "Bond-5Y");

	  private static readonly CurveName NAME_ISSUER = CurveName.of("TestIssuerCurve");
	  private static readonly CurveMetadata METADATA_ISSUER = Curves.zeroRates(NAME_ISSUER, ACT_365F);
	  private static readonly InterpolatedNodalCurve CURVE_ISSUER = InterpolatedNodalCurve.of(METADATA_ISSUER, DoubleArray.of(0, 15), DoubleArray.of(1, 2.5), INTERPOLATOR);
	  private static readonly ZeroRateDiscountFactors DSC_FACTORS_ISSUER = ZeroRateDiscountFactors.of(GBP, DATE, CURVE_ISSUER);
	  private static readonly LegalEntityGroup GROUP_ISSUER = LegalEntityGroup.of("ISSUER1");
	  private static readonly LegalEntityId ID_ISSUER = LegalEntityId.of("OG-Ticker", "Issuer-1");

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		ImmutableLegalEntityDiscountingProvider test = ImmutableLegalEntityDiscountingProvider.builder().issuerCurves(ImmutableMap.of(Pair.of(GROUP_ISSUER, GBP), DSC_FACTORS_ISSUER)).issuerCurveGroups(ImmutableMap.of(ID_ISSUER, GROUP_ISSUER)).repoCurves(ImmutableMap.of(Pair.of(GROUP_REPO_SECURITY, GBP), DSC_FACTORS_REPO)).repoCurveSecurityGroups(ImmutableMap.of(ID_SECURITY, GROUP_REPO_SECURITY)).valuationDate(DATE).build();
		assertEquals(test.issuerCurveDiscountFactors(ID_ISSUER, GBP), IssuerCurveDiscountFactors.of(DSC_FACTORS_ISSUER, GROUP_ISSUER));
		assertEquals(test.repoCurveDiscountFactors(ID_SECURITY, ID_ISSUER, GBP), RepoCurveDiscountFactors.of(DSC_FACTORS_REPO, GROUP_REPO_SECURITY));
		assertThrowsIllegalArg(() => test.repoCurveDiscountFactors(ID_ISSUER, GBP));
		assertEquals(test.ValuationDate, DATE);
	  }

	  public virtual void test_builder_noValuationDate()
	  {
		ImmutableLegalEntityDiscountingProvider test = ImmutableLegalEntityDiscountingProvider.builder().issuerCurves(ImmutableMap.of(Pair.of(GROUP_ISSUER, GBP), DSC_FACTORS_ISSUER)).issuerCurveGroups(ImmutableMap.of(ID_ISSUER, GROUP_ISSUER)).repoCurves(ImmutableMap.of(Pair.of(GROUP_REPO_ISSUER, GBP), DSC_FACTORS_REPO)).repoCurveGroups(ImmutableMap.of(ID_ISSUER, GROUP_REPO_ISSUER)).build();
		assertEquals(test.issuerCurveDiscountFactors(ID_ISSUER, GBP), IssuerCurveDiscountFactors.of(DSC_FACTORS_ISSUER, GROUP_ISSUER));
		assertEquals(test.repoCurveDiscountFactors(ID_SECURITY, ID_ISSUER, GBP), RepoCurveDiscountFactors.of(DSC_FACTORS_REPO, GROUP_REPO_ISSUER));
		assertEquals(test.repoCurveDiscountFactors(ID_ISSUER, GBP), RepoCurveDiscountFactors.of(DSC_FACTORS_REPO, GROUP_REPO_ISSUER));
		assertEquals(test.ValuationDate, DATE);

	  }

	  public virtual void test_builder_noRepoRate()
	  {
		ImmutableLegalEntityDiscountingProvider test = ImmutableLegalEntityDiscountingProvider.builder().issuerCurveGroups(ImmutableMap.of(ID_ISSUER, GROUP_ISSUER)).issuerCurves(ImmutableMap.of(Pair.of(GROUP_ISSUER, GBP), DSC_FACTORS_ISSUER)).build();
		assertEquals(test.issuerCurveDiscountFactors(ID_ISSUER, GBP), IssuerCurveDiscountFactors.of(DSC_FACTORS_ISSUER, GROUP_ISSUER));
		assertEquals(test.ValuationDate, DATE);
	  }

	  public virtual void test_builder_fail()
	  {
		// no relevant map for repo curve
		assertThrowsIllegalArg(() => ImmutableLegalEntityDiscountingProvider.builder().issuerCurves(ImmutableMap.of(Pair.of(GROUP_ISSUER, GBP), DSC_FACTORS_ISSUER)).issuerCurveGroups(ImmutableMap.of(ID_ISSUER, GROUP_ISSUER)).repoCurves(ImmutableMap.of(Pair.of(GROUP_REPO_ISSUER, GBP), DSC_FACTORS_REPO)).repoCurveGroups(ImmutableMap.of(ID_ISSUER, RepoGroup.of("ISSUER2 BND 5Y"))).build());
		// no relevant map for issuer curve
		assertThrowsIllegalArg(() => ImmutableLegalEntityDiscountingProvider.builder().issuerCurves(ImmutableMap.of(Pair.of(GROUP_ISSUER, GBP), DSC_FACTORS_ISSUER)).issuerCurveGroups(ImmutableMap.of(ID_ISSUER, LegalEntityGroup.of("ISSUER2"))).repoCurves(ImmutableMap.of(Pair.of(GROUP_REPO_ISSUER, GBP), DSC_FACTORS_REPO)).repoCurveGroups(ImmutableMap.of(ID_ISSUER, GROUP_REPO_ISSUER)).build());
		// issuer curve and valuation date are missing
		assertThrowsIllegalArg(() => ImmutableLegalEntityDiscountingProvider.builder().issuerCurveGroups(ImmutableMap.of(ID_ISSUER, GROUP_ISSUER)).repoCurves(ImmutableMap.of(Pair.of(GROUP_REPO_SECURITY, GBP), DSC_FACTORS_REPO)).repoCurveSecurityGroups(ImmutableMap.of(ID_SECURITY, GROUP_REPO_SECURITY)).build());
		// issuer curve date is different from valuation date
		DiscountFactors dscFactorIssuer = ZeroRateDiscountFactors.of(GBP, date(2015, 6, 14), CURVE_ISSUER);
		assertThrowsIllegalArg(() => ImmutableLegalEntityDiscountingProvider.builder().issuerCurves(ImmutableMap.of(Pair.of(GROUP_ISSUER, GBP), dscFactorIssuer)).issuerCurveGroups(ImmutableMap.of(ID_ISSUER, GROUP_ISSUER)).repoCurves(ImmutableMap.of(Pair.of(GROUP_REPO_SECURITY, GBP), DSC_FACTORS_REPO)).repoCurveSecurityGroups(ImmutableMap.of(ID_SECURITY, GROUP_REPO_SECURITY)).valuationDate(DATE).build());
		// repo curve rate is different from valuation date
		DiscountFactors dscFactorRepo = ZeroRateDiscountFactors.of(GBP, date(2015, 6, 14), CURVE_REPO);
		assertThrowsIllegalArg(() => ImmutableLegalEntityDiscountingProvider.builder().issuerCurves(ImmutableMap.of(Pair.of(GROUP_ISSUER, GBP), DSC_FACTORS_ISSUER)).issuerCurveGroups(ImmutableMap.of(ID_ISSUER, GROUP_ISSUER)).repoCurves(ImmutableMap.of(Pair.of(GROUP_REPO_SECURITY, GBP), dscFactorRepo)).repoCurveSecurityGroups(ImmutableMap.of(ID_SECURITY, GROUP_REPO_SECURITY)).valuationDate(DATE).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_discountFactor_notFound()
	  {
		LegalEntityId issuerId = LegalEntityId.of("OG-Ticker", "Issuer-2");
		LegalEntityGroup issuerGroup = LegalEntityGroup.of("ISSUER2");
		RepoGroup repoGroup = RepoGroup.of("ISSUER2 BND 5Y");
		SecurityId securityId = SecurityId.of("OG-Ticker", "Issuer-2-bond-5Y");
		ImmutableLegalEntityDiscountingProvider test = ImmutableLegalEntityDiscountingProvider.builder().issuerCurves(ImmutableMap.of(Pair.of(GROUP_ISSUER, GBP), DSC_FACTORS_ISSUER)).issuerCurveGroups(ImmutableMap.of(ID_ISSUER, GROUP_ISSUER, issuerId, issuerGroup)).repoCurves(ImmutableMap.of(Pair.of(GROUP_REPO_SECURITY, GBP), DSC_FACTORS_REPO)).repoCurveGroups(ImmutableMap.of(issuerId, repoGroup)).repoCurveSecurityGroups(ImmutableMap.of(ID_SECURITY, GROUP_REPO_SECURITY)).valuationDate(DATE).build();
		assertThrowsIllegalArg(() => test.issuerCurveDiscountFactors(ID_ISSUER, USD));
		assertThrowsIllegalArg(() => test.issuerCurveDiscountFactors(LegalEntityId.of("OG-Ticker", "foo"), GBP));
		assertThrowsIllegalArg(() => test.issuerCurveDiscountFactors(issuerId, GBP));
		assertThrowsIllegalArg(() => test.repoCurveDiscountFactors(ID_SECURITY, ID_ISSUER, USD));
		assertThrowsIllegalArg(() => test.repoCurveDiscountFactors(SecurityId.of("OG-Ticker", "foo-bond"), LegalEntityId.of("OG-Ticker", "foo"), GBP));
		assertThrowsIllegalArg(() => test.repoCurveDiscountFactors(securityId, issuerId, GBP));
	  }

	  public virtual void test_curveParameterSensitivity()
	  {
		ImmutableLegalEntityDiscountingProvider test = ImmutableLegalEntityDiscountingProvider.builder().issuerCurves(ImmutableMap.of(Pair.of(GROUP_ISSUER, GBP), DSC_FACTORS_ISSUER)).issuerCurveGroups(ImmutableMap.of(ID_ISSUER, GROUP_ISSUER)).repoCurves(ImmutableMap.of(Pair.of(GROUP_REPO_ISSUER, GBP), DSC_FACTORS_REPO)).repoCurveGroups(ImmutableMap.of(ID_ISSUER, GROUP_REPO_ISSUER)).valuationDate(DATE).build();
		LocalDate refDate = date(2018, 11, 24);
		IssuerCurveZeroRateSensitivity sensi1 = test.issuerCurveDiscountFactors(ID_ISSUER, GBP).zeroRatePointSensitivity(refDate, GBP);
		RepoCurveZeroRateSensitivity sensi2 = test.repoCurveDiscountFactors(ID_SECURITY, ID_ISSUER, GBP).zeroRatePointSensitivity(refDate, GBP);
		PointSensitivities sensi = PointSensitivities.of(sensi1, sensi2);
		CurrencyParameterSensitivities computed = test.parameterSensitivity(sensi);
		CurrencyParameterSensitivities expected = DSC_FACTORS_ISSUER.parameterSensitivity(sensi1.createZeroRateSensitivity()).combinedWith(DSC_FACTORS_REPO.parameterSensitivity(sensi2.createZeroRateSensitivity()));
		assertTrue(computed.equalWithTolerance(expected, 1.0e-12));
	  }

	  public virtual void test_curveParameterSensitivity_noSensi()
	  {
		ImmutableLegalEntityDiscountingProvider test = ImmutableLegalEntityDiscountingProvider.builder().issuerCurves(ImmutableMap.of(Pair.of(GROUP_ISSUER, GBP), DSC_FACTORS_ISSUER)).issuerCurveGroups(ImmutableMap.of(ID_ISSUER, GROUP_ISSUER)).repoCurves(ImmutableMap.of(Pair.of(GROUP_REPO_ISSUER, GBP), DSC_FACTORS_REPO)).repoCurveGroups(ImmutableMap.of(ID_ISSUER, GROUP_REPO_ISSUER)).valuationDate(DATE).build();
		ZeroRateSensitivity sensi = ZeroRateSensitivity.of(USD, DSC_FACTORS_ISSUER.relativeYearFraction(date(2018, 11, 24)), 25d);
		CurrencyParameterSensitivities computed = test.parameterSensitivity(sensi.build());
		assertEquals(computed, CurrencyParameterSensitivities.empty());
	  }

	  public virtual void test_findData()
	  {
		ImmutableLegalEntityDiscountingProvider test = ImmutableLegalEntityDiscountingProvider.builder().issuerCurves(ImmutableMap.of(Pair.of(GROUP_ISSUER, GBP), DSC_FACTORS_ISSUER)).issuerCurveGroups(ImmutableMap.of(ID_ISSUER, GROUP_ISSUER)).repoCurves(ImmutableMap.of(Pair.of(GROUP_REPO_ISSUER, GBP), DSC_FACTORS_REPO)).repoCurveGroups(ImmutableMap.of(ID_ISSUER, GROUP_REPO_ISSUER)).valuationDate(DATE).build();
		assertEquals(test.findData(DSC_FACTORS_ISSUER.Curve.Name), DSC_FACTORS_ISSUER.Curve);
		assertEquals(test.findData(DSC_FACTORS_REPO.Curve.Name), DSC_FACTORS_REPO.Curve);
		assertEquals(test.findData(CurveName.of("Rubbish")), null);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ImmutableLegalEntityDiscountingProvider test1 = ImmutableLegalEntityDiscountingProvider.builder().issuerCurves(ImmutableMap.of(Pair.of(GROUP_ISSUER, GBP), DSC_FACTORS_ISSUER)).issuerCurveGroups(ImmutableMap.of(ID_ISSUER, GROUP_ISSUER)).repoCurves(ImmutableMap.of(Pair.of(GROUP_REPO_ISSUER, GBP), DSC_FACTORS_REPO)).repoCurveGroups(ImmutableMap.of(ID_ISSUER, GROUP_REPO_ISSUER)).build();
		coverImmutableBean(test1);
		LocalDate val = date(2015, 6, 14);
		DiscountFactors dscFactorIssuer = ZeroRateDiscountFactors.of(GBP, val, CURVE_ISSUER);
		DiscountFactors dscFactorRepo = ZeroRateDiscountFactors.of(GBP, val, CURVE_REPO);
		ImmutableLegalEntityDiscountingProvider test2 = ImmutableLegalEntityDiscountingProvider.builder().issuerCurves(ImmutableMap.of(Pair.of(GROUP_ISSUER, GBP), dscFactorIssuer)).issuerCurveGroups(ImmutableMap.of(LegalEntityId.of("OG-Ticker", "foo"), GROUP_ISSUER)).repoCurves(ImmutableMap.of(Pair.of(RepoGroup.of("ISSUER2 BND 5Y"), GBP), dscFactorRepo)).repoCurveSecurityGroups(ImmutableMap.of(ID_SECURITY, RepoGroup.of("ISSUER2 BND 5Y"))).build();
		coverBeanEquals(test1, test2);
	  }

	}

}