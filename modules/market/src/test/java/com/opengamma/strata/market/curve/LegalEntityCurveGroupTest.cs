using System.Collections.Generic;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.JPY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Pair = com.opengamma.strata.collect.tuple.Pair;

	/// <summary>
	/// Test <seealso cref="LegalEntityCurveGroup"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LegalEntityCurveGroupTest
	public class LegalEntityCurveGroupTest
	{

	  private static readonly CurveGroupName NAME1 = CurveGroupName.of("TestGroup1");
	  private static readonly CurveGroupName NAME2 = CurveGroupName.of("TestGroup2");
	  private static readonly CurveName REPO_NAME = CurveName.of("Repo");
	  private static readonly CurveName ISSUER_NAME1 = CurveName.of("Issuer1Gbp");
	  private static readonly CurveName ISSUER_NAME2 = CurveName.of("Issuer1Usd");
	  private static readonly CurveName ISSUER_NAME3 = CurveName.of("Issuer2");
	  private static readonly RepoGroup REPO_GROUP = RepoGroup.of("RepoGroup");
	  private static readonly LegalEntityGroup LEGAL_ENTITY_GROUP1 = LegalEntityGroup.of("LegalEntityGroup1");
	  private static readonly LegalEntityGroup LEGAL_ENTITY_GROUP2 = LegalEntityGroup.of("LegalEntityGroup2");
	  private static readonly Curve REPO_CURVE = ConstantCurve.of(REPO_NAME, 0.99);
	  private static readonly IDictionary<Pair<RepoGroup, Currency>, Curve> REPO_CURVES = ImmutableMap.of(Pair.of(REPO_GROUP, GBP), REPO_CURVE, Pair.of(REPO_GROUP, USD), REPO_CURVE);
	  private static readonly Curve ISSUER_CURVE1 = ConstantCurve.of(ISSUER_NAME1, 0.5);
	  private static readonly Curve ISSUER_CURVE2 = ConstantCurve.of(ISSUER_NAME2, 0.6);
	  private static readonly Curve ISSUER_CURVE3 = ConstantCurve.of(ISSUER_NAME3, 0.7);
	  private static readonly IDictionary<Pair<LegalEntityGroup, Currency>, Curve> ISSUER_CURVES = ImmutableMap.of(Pair.of(LEGAL_ENTITY_GROUP1, GBP), ISSUER_CURVE1, Pair.of(LEGAL_ENTITY_GROUP1, USD), ISSUER_CURVE2, Pair.of(LEGAL_ENTITY_GROUP2, GBP), ISSUER_CURVE3);

	  public virtual void test_of()
	  {
		LegalEntityCurveGroup test = LegalEntityCurveGroup.of(NAME1, REPO_CURVES, ISSUER_CURVES);
		assertEquals(test.Name, NAME1);
		assertEquals(test.RepoCurves, REPO_CURVES);
		assertEquals(test.IssuerCurves, ISSUER_CURVES);
		assertEquals(test.findCurve(REPO_NAME).get(), REPO_CURVE);
		assertEquals(test.findCurve(ISSUER_NAME1).get(), ISSUER_CURVE1);
		assertEquals(test.findCurve(ISSUER_NAME2).get(), ISSUER_CURVE2);
		assertEquals(test.findCurve(ISSUER_NAME3).get(), ISSUER_CURVE3);
		assertFalse(test.findCurve(CurveName.of("foo")).Present);
		assertEquals(test.findRepoCurve(REPO_GROUP, GBP).get(), REPO_CURVE);
		assertEquals(test.findRepoCurve(REPO_GROUP, USD).get(), REPO_CURVE);
		assertFalse(test.findRepoCurve(REPO_GROUP, JPY).Present);
		assertEquals(test.findIssuerCurve(LEGAL_ENTITY_GROUP1, GBP).get(), ISSUER_CURVE1);
		assertEquals(test.findIssuerCurve(LEGAL_ENTITY_GROUP1, USD).get(), ISSUER_CURVE2);
		assertEquals(test.findIssuerCurve(LEGAL_ENTITY_GROUP2, GBP).get(), ISSUER_CURVE3);
		assertFalse(test.findIssuerCurve(LEGAL_ENTITY_GROUP2, USD).Present);
	  }

	  public virtual void test_builder()
	  {
		LegalEntityCurveGroup test = LegalEntityCurveGroup.builder().name(NAME2).repoCurves(REPO_CURVES).issuerCurves(ISSUER_CURVES).build();
		assertEquals(test.Name, NAME2);
		assertEquals(test.RepoCurves, REPO_CURVES);
		assertEquals(test.IssuerCurves, ISSUER_CURVES);
	  }

	  public virtual void stream()
	  {
		LegalEntityCurveGroup test = LegalEntityCurveGroup.of(NAME1, REPO_CURVES, ISSUER_CURVES);
		IList<Curve> expectedAll = ImmutableList.builder<Curve>().add(REPO_CURVE).add(ISSUER_CURVE1).add(ISSUER_CURVE2).add(ISSUER_CURVE3).build();
		test.ToList().containsAll(expectedAll);
		IList<Curve> expectedRepo = ImmutableList.builder<Curve>().add(REPO_CURVE).build();
		test.repoCurveStream().collect(Collectors.toList()).containsAll(expectedRepo);
		IList<Curve> expectedIssuer = ImmutableList.builder<Curve>().add(ISSUER_CURVE1).add(ISSUER_CURVE2).add(ISSUER_CURVE3).build();
		test.issuerCurveStream().collect(Collectors.toList()).containsAll(expectedIssuer);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		LegalEntityCurveGroup test1 = LegalEntityCurveGroup.of(NAME1, REPO_CURVES, ISSUER_CURVES);
		coverImmutableBean(test1);
		LegalEntityCurveGroup test2 = LegalEntityCurveGroup.of(NAME1, ImmutableMap.of(), ImmutableMap.of());
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		LegalEntityCurveGroup test = LegalEntityCurveGroup.of(NAME1, REPO_CURVES, ISSUER_CURVES);
		assertSerialization(test);
	  }

	}

}