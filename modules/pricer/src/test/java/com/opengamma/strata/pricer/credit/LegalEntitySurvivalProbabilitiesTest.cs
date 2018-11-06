using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using StandardId = com.opengamma.strata.basics.StandardId;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;

	/// <summary>
	/// Test <seealso cref="LegalEntitySurvivalProbabilities"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LegalEntitySurvivalProbabilitiesTest
	public class LegalEntitySurvivalProbabilitiesTest
	{
	  private static readonly LocalDate VALUATION = LocalDate.of(2016, 5, 6);
	  private static readonly DoubleArray TIME = DoubleArray.ofUnsafe(new double[] {0.09041095890410959, 0.16712328767123288, 0.2547945205479452, 0.5041095890410959, 0.7534246575342466, 1.0054794520547945, 2.0054794520547947, 3.008219178082192, 4.013698630136987, 5.010958904109589, 6.008219178082192, 7.010958904109589, 8.01095890410959, 9.01095890410959, 10.016438356164384, 12.013698630136986, 15.021917808219179, 20.01917808219178, 30.024657534246575});
	  private static readonly DoubleArray RATE = DoubleArray.ofUnsafe(new double[] {-0.002078655697855299, -0.001686438401304855, -0.0013445486228483379, -4.237819925898475E-4, 2.5142499469348057E-5, 5.935063895780138E-4, -3.247081037469503E-4, 6.147182786549223E-4, 0.0019060597240545122, 0.0033125742254568815, 0.0047766352312329455, 0.0062374324537341225, 0.007639664176639106, 0.008971003650150983, 0.010167545380711455, 0.012196853322376243, 0.01441082634734099, 0.016236611610989507, 0.01652439910865982});
	  private static readonly CurveName CURVE_NAME = CurveName.of("yieldUsd");
	  private static readonly CreditDiscountFactors DFS = IsdaCreditDiscountFactors.of(USD, VALUATION, CURVE_NAME, TIME, RATE, ACT_365F);
	  private static readonly StandardId LEGAL_ENTITY = StandardId.of("OG", "ABC");
	  private static readonly LocalDate DATE_AFTER = LocalDate.of(2017, 2, 24);

	  public virtual void test_of()
	  {
		LegalEntitySurvivalProbabilities test = LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY, DFS);
		assertEquals(test.Currency, USD);
		assertEquals(test.LegalEntityId, LEGAL_ENTITY);
		assertEquals(test.ParameterKeys, TIME);
		assertEquals(test.SurvivalProbabilities, DFS);
		assertEquals(test.ValuationDate, VALUATION);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_survivalProbability()
	  {
		LegalEntitySurvivalProbabilities test = LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY, DFS);
		assertEquals(test.survivalProbability(DATE_AFTER), DFS.discountFactor(DATE_AFTER));
	  }

	  public virtual void test_zeroRate()
	  {
		LegalEntitySurvivalProbabilities test = LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY, DFS);
		double relativeYearFraction = ACT_365F.relativeYearFraction(VALUATION, DATE_AFTER);
		double discountFactor = test.survivalProbability(DATE_AFTER);
		double zeroRate = test.zeroRate(relativeYearFraction);
		assertEquals(Math.Exp(-zeroRate * relativeYearFraction), discountFactor);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_zeroRatePointSensitivity()
	  {
		LegalEntitySurvivalProbabilities test = LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY, DFS);
		CreditCurveZeroRateSensitivity expected = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, DFS.zeroRatePointSensitivity(DATE_AFTER));
		assertEquals(test.zeroRatePointSensitivity(DATE_AFTER), expected);
	  }

	  public virtual void test_zeroRatePointSensitivity_sensitivityCurrency()
	  {
		LegalEntitySurvivalProbabilities test = LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY, DFS);
		CreditCurveZeroRateSensitivity expected = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, DFS.zeroRatePointSensitivity(DATE_AFTER, GBP));
		assertEquals(test.zeroRatePointSensitivity(DATE_AFTER, GBP), expected);
	  }

	  public virtual void test_zeroRatePointSensitivity_yearFraction()
	  {
		double yearFraction = DFS.relativeYearFraction(DATE_AFTER);
		LegalEntitySurvivalProbabilities test = LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY, DFS);
		CreditCurveZeroRateSensitivity expected = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, DFS.zeroRatePointSensitivity(yearFraction));
		assertEquals(test.zeroRatePointSensitivity(yearFraction), expected);
	  }

	  public virtual void test_zeroRatePointSensitivity_sensitivityCurrency_yearFraction()
	  {
		double yearFraction = DFS.relativeYearFraction(DATE_AFTER);
		LegalEntitySurvivalProbabilities test = LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY, DFS);
		CreditCurveZeroRateSensitivity expected = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, DFS.zeroRatePointSensitivity(yearFraction, GBP));
		assertEquals(test.zeroRatePointSensitivity(yearFraction, GBP), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_unitParameterSensitivity()
	  {
		LegalEntitySurvivalProbabilities test = LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY, DFS);
		CreditCurveZeroRateSensitivity sens = test.zeroRatePointSensitivity(DATE_AFTER);
		CurrencyParameterSensitivities expected = DFS.parameterSensitivity(DFS.zeroRatePointSensitivity(DATE_AFTER));
		assertEquals(test.parameterSensitivity(sens), expected);
	  }

	  //-------------------------------------------------------------------------
	  // proper end-to-end FD tests are in pricer test
	  public virtual void test_parameterSensitivity()
	  {
		LegalEntitySurvivalProbabilities test = LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY, DFS);
		CreditCurveZeroRateSensitivity point = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, ZeroRateSensitivity.of(USD, 1d, 1d));
		assertEquals(test.parameterSensitivity(point).size(), 1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		LegalEntitySurvivalProbabilities test1 = LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY, DFS);
		coverImmutableBean(test1);
		LegalEntitySurvivalProbabilities test2 = LegalEntitySurvivalProbabilities.of(StandardId.of("OG", "CCC"), IsdaCreditDiscountFactors.of(GBP, VALUATION, CURVE_NAME, DoubleArray.of(5.0), DoubleArray.of(0.014), ACT_365F));
		coverBeanEquals(test1, test2);
	  }

	}

}