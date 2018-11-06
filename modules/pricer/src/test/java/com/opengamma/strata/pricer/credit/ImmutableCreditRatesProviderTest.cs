/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.JPY;
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
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using FxForwardSensitivity = com.opengamma.strata.pricer.fx.FxForwardSensitivity;

	/// <summary>
	/// Test <seealso cref="ImmutableCreditRatesProvider"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ImmutableCreditRatesProviderTest
	public class ImmutableCreditRatesProviderTest
	{

	  private static readonly LocalDate VALUATION = LocalDate.of(2015, 2, 11);

	  private const double RECOVERY_RATE_ABC = 0.25;
	  private const double RECOVERY_RATE_DEF = 0.35;
	  private static readonly StandardId LEGAL_ENTITY_ABC = StandardId.of("OG", "ABC");
	  private static readonly StandardId LEGAL_ENTITY_DEF = StandardId.of("OG", "DEF");
	  private static readonly ConstantRecoveryRates RR_ABC = ConstantRecoveryRates.of(LEGAL_ENTITY_ABC, VALUATION, RECOVERY_RATE_ABC);
	  private static readonly ConstantRecoveryRates RR_DEF = ConstantRecoveryRates.of(LEGAL_ENTITY_DEF, VALUATION, RECOVERY_RATE_DEF);

	  // discount curves
	  private static readonly DoubleArray TIME_DSC_USD = DoubleArray.ofUnsafe(new double[] {1.0, 2.0, 5.0, 10.0, 20.0, 30.0});
	  private static readonly DoubleArray RATE_DSC_USD = DoubleArray.ofUnsafe(new double[] {0.015, 0.019, 0.016, 0.012, 0.01, 0.005});
	  private static readonly CurveName NAME_DSC_USD = CurveName.of("yieldUsd");
	  private static readonly IsdaCreditDiscountFactors DSC_USD = IsdaCreditDiscountFactors.of(USD, VALUATION, NAME_DSC_USD, TIME_DSC_USD, RATE_DSC_USD, ACT_365F);
	  private static readonly DoubleArray TIME_DSC_JPY = DoubleArray.ofUnsafe(new double[] {1.0, 5.0, 10.0, 20.0});
	  private static readonly DoubleArray RATE_DSC_JPY = DoubleArray.ofUnsafe(new double[] {0.01, 0.011, 0.007, 0.002});
	  private static readonly CurveName NAME_DSC_JPY = CurveName.of("yieldJpy");
	  private static readonly IsdaCreditDiscountFactors DSC_JPY = IsdaCreditDiscountFactors.of(JPY, VALUATION, NAME_DSC_JPY, TIME_DSC_JPY, RATE_DSC_JPY, ACT_365F);
	  // credit curves
	  private static readonly DoubleArray TIME_CRD_ABC_USD = DoubleArray.ofUnsafe(new double[] {1.0, 3.0, 5.0, 7.0, 10.0});
	  private static readonly DoubleArray RATE_CRD_ABC_USD = DoubleArray.ofUnsafe(new double[] {0.005, 0.006, 0.004, 0.012, 0.01});
	  private static readonly CurveName NAME_CRD_ABC_USD = CurveName.of("creditAbc_usd");
	  private static readonly IsdaCreditDiscountFactors CRD_ABC_USD = IsdaCreditDiscountFactors.of(USD, VALUATION, NAME_CRD_ABC_USD, TIME_CRD_ABC_USD, RATE_CRD_ABC_USD, ACT_365F);
	  private static readonly DoubleArray TIME_CRD_ABC_JPY = DoubleArray.ofUnsafe(new double[] {1.0, 3.0, 5.0, 7.0, 10.0});
	  private static readonly DoubleArray RATE_CRD_ABC_JPY = DoubleArray.ofUnsafe(new double[] {0.005, 0.006, 0.004, 0.012, 0.01});
	  private static readonly CurveName NAME_CRD_ABC_JPY = CurveName.of("creditAbc_jpy");
	  private static readonly IsdaCreditDiscountFactors CRD_ABC_JPY = IsdaCreditDiscountFactors.of(JPY, VALUATION, NAME_CRD_ABC_JPY, TIME_CRD_ABC_JPY, RATE_CRD_ABC_JPY, ACT_365F);
	  private static readonly DoubleArray TIME_CRD_DEF = DoubleArray.ofUnsafe(new double[] {3.0, 5.0, 10.0});
	  private static readonly DoubleArray RATE_CRD_DEF = DoubleArray.ofUnsafe(new double[] {0.005, 0.006, 0.004});
	  private static readonly CurveName NAME_CRD_DEF = CurveName.of("creditDef");
	  private static readonly IsdaCreditDiscountFactors CRD_DEF = IsdaCreditDiscountFactors.of(JPY, VALUATION, NAME_CRD_DEF, TIME_CRD_DEF, RATE_CRD_DEF, ACT_365F);

	  public virtual void test_getter()
	  {
		ImmutableCreditRatesProvider test = ImmutableCreditRatesProvider.builder().valuationDate(VALUATION).creditCurves(ImmutableMap.of(Pair.of(LEGAL_ENTITY_ABC, USD), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_ABC, CRD_ABC_USD), Pair.of(LEGAL_ENTITY_ABC, JPY), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_ABC, CRD_ABC_JPY), Pair.of(LEGAL_ENTITY_DEF, JPY), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_DEF, CRD_DEF))).discountCurves(ImmutableMap.of(USD, DSC_USD, JPY, DSC_JPY)).recoveryRateCurves(ImmutableMap.of(LEGAL_ENTITY_ABC, RR_ABC, LEGAL_ENTITY_DEF, RR_DEF)).build();
		assertEquals(test.discountFactors(USD), DSC_USD);
		assertEquals(test.discountFactors(JPY), DSC_JPY);
		assertEquals(test.survivalProbabilities(LEGAL_ENTITY_ABC, USD), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_ABC, CRD_ABC_USD));
		assertEquals(test.survivalProbabilities(LEGAL_ENTITY_ABC, JPY), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_ABC, CRD_ABC_JPY));
		assertEquals(test.survivalProbabilities(LEGAL_ENTITY_DEF, JPY), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_DEF, CRD_DEF));
		assertEquals(test.recoveryRates(LEGAL_ENTITY_ABC), RR_ABC);
		assertEquals(test.recoveryRates(LEGAL_ENTITY_DEF), RR_DEF);
		StandardId entity = StandardId.of("OG", "NONE");
		assertThrowsIllegalArg(() => test.discountFactors(EUR));
		assertThrowsIllegalArg(() => test.survivalProbabilities(LEGAL_ENTITY_DEF, USD));
		assertThrowsIllegalArg(() => test.survivalProbabilities(entity, USD));
		assertThrowsIllegalArg(() => test.recoveryRates(entity));
	  }

	  public virtual void test_valuationDateMismatch()
	  {
		ConstantRecoveryRates rr_wrong = ConstantRecoveryRates.of(LEGAL_ENTITY_ABC, VALUATION.plusWeeks(1), RECOVERY_RATE_ABC);
		assertThrowsIllegalArg(() => ImmutableCreditRatesProvider.builder().valuationDate(VALUATION).creditCurves(ImmutableMap.of(Pair.of(LEGAL_ENTITY_ABC, USD), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_ABC, CRD_ABC_USD), Pair.of(LEGAL_ENTITY_ABC, JPY), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_ABC, CRD_ABC_JPY), Pair.of(LEGAL_ENTITY_DEF, JPY), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_DEF, CRD_DEF))).discountCurves(ImmutableMap.of(USD, DSC_USD, JPY, DSC_JPY)).recoveryRateCurves(ImmutableMap.of(LEGAL_ENTITY_ABC, rr_wrong, LEGAL_ENTITY_DEF, RR_DEF)).build());
		IsdaCreditDiscountFactors crd_wrong = IsdaCreditDiscountFactors.of(JPY, VALUATION.plusWeeks(1), NAME_CRD_DEF, TIME_CRD_DEF, RATE_CRD_DEF, ACT_365F);
		assertThrowsIllegalArg(() => ImmutableCreditRatesProvider.builder().valuationDate(VALUATION).creditCurves(ImmutableMap.of(Pair.of(LEGAL_ENTITY_ABC, USD), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_ABC, CRD_ABC_USD), Pair.of(LEGAL_ENTITY_ABC, JPY), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_ABC, CRD_ABC_JPY), Pair.of(LEGAL_ENTITY_DEF, JPY), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_DEF, crd_wrong))).discountCurves(ImmutableMap.of(USD, DSC_USD, JPY, DSC_JPY)).recoveryRateCurves(ImmutableMap.of(LEGAL_ENTITY_ABC, RR_ABC, LEGAL_ENTITY_DEF, RR_DEF)).build());
		IsdaCreditDiscountFactors dsc_wrong = IsdaCreditDiscountFactors.of(USD, VALUATION.plusWeeks(1), NAME_DSC_USD, TIME_DSC_USD, RATE_DSC_USD, ACT_365F);
		assertThrowsIllegalArg(() => ImmutableCreditRatesProvider.builder().valuationDate(VALUATION).creditCurves(ImmutableMap.of(Pair.of(LEGAL_ENTITY_ABC, USD), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_ABC, CRD_ABC_USD), Pair.of(LEGAL_ENTITY_ABC, JPY), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_ABC, CRD_ABC_JPY), Pair.of(LEGAL_ENTITY_DEF, JPY), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_DEF, CRD_DEF))).discountCurves(ImmutableMap.of(USD, dsc_wrong, JPY, DSC_JPY)).recoveryRateCurves(ImmutableMap.of(LEGAL_ENTITY_ABC, RR_ABC, LEGAL_ENTITY_DEF, RR_DEF)).build());
	  }

	  public virtual void test_parameterSensitivity()
	  {
		ZeroRateSensitivity zeroPt = ZeroRateSensitivity.of(USD, 10d, 5d);
		CreditCurveZeroRateSensitivity creditPt = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY_ABC, JPY, 2d, 3d);
		FxForwardSensitivity fxPt = FxForwardSensitivity.of(CurrencyPair.of(JPY, USD), USD, LocalDate.of(2017, 2, 14), 15d);
		CreditRatesProvider test = ImmutableCreditRatesProvider.builder().creditCurves(ImmutableMap.of(Pair.of(LEGAL_ENTITY_ABC, USD), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_ABC, CRD_ABC_USD), Pair.of(LEGAL_ENTITY_ABC, JPY), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_ABC, CRD_ABC_JPY), Pair.of(LEGAL_ENTITY_DEF, JPY), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_DEF, CRD_DEF))).discountCurves(ImmutableMap.of(USD, DSC_USD, JPY, DSC_JPY)).recoveryRateCurves(ImmutableMap.of(LEGAL_ENTITY_ABC, RR_ABC, LEGAL_ENTITY_DEF, RR_DEF)).valuationDate(VALUATION).build();
		CurrencyParameterSensitivities computed = test.parameterSensitivity(zeroPt.combinedWith(creditPt).combinedWith(fxPt).build());
		CurrencyParameterSensitivities expected = DSC_USD.parameterSensitivity(zeroPt).combinedWith(LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_ABC, CRD_ABC_JPY).parameterSensitivity(creditPt));
		assertTrue(computed.equalWithTolerance(expected, 1.0e-14));
	  }

	  public virtual void test_singleCreditCurveParameterSensitivity()
	  {
		ZeroRateSensitivity zeroPt = ZeroRateSensitivity.of(USD, 10d, 5d);
		CreditCurveZeroRateSensitivity creditPt = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY_ABC, JPY, 2d, 3d);
		FxForwardSensitivity fxPt = FxForwardSensitivity.of(CurrencyPair.of(JPY, USD), USD, LocalDate.of(2017, 2, 14), 15d);
		CreditRatesProvider test = ImmutableCreditRatesProvider.builder().creditCurves(ImmutableMap.of(Pair.of(LEGAL_ENTITY_ABC, USD), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_ABC, CRD_ABC_USD), Pair.of(LEGAL_ENTITY_ABC, JPY), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_ABC, CRD_ABC_JPY), Pair.of(LEGAL_ENTITY_DEF, JPY), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_DEF, CRD_DEF))).discountCurves(ImmutableMap.of(USD, DSC_USD, JPY, DSC_JPY)).recoveryRateCurves(ImmutableMap.of(LEGAL_ENTITY_ABC, RR_ABC, LEGAL_ENTITY_DEF, RR_DEF)).valuationDate(VALUATION).build();
		CurrencyParameterSensitivities computed = CurrencyParameterSensitivities.of(test.singleCreditCurveParameterSensitivity(zeroPt.combinedWith(creditPt).combinedWith(fxPt).build(), LEGAL_ENTITY_ABC, JPY));
		CurrencyParameterSensitivities expected = LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_ABC, CRD_ABC_JPY).parameterSensitivity(creditPt);
		assertTrue(computed.equalWithTolerance(expected, 1.0e-14));
	  }

	  public virtual void test_singleDiscountCurveParameterSensitivity()
	  {
		ZeroRateSensitivity zeroPt = ZeroRateSensitivity.of(USD, 10d, 5d);
		CreditCurveZeroRateSensitivity creditPt = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY_ABC, JPY, 2d, 3d);
		FxForwardSensitivity fxPt = FxForwardSensitivity.of(CurrencyPair.of(JPY, USD), USD, LocalDate.of(2017, 2, 14), 15d);
		CreditRatesProvider test = ImmutableCreditRatesProvider.builder().creditCurves(ImmutableMap.of(Pair.of(LEGAL_ENTITY_ABC, USD), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_ABC, CRD_ABC_USD), Pair.of(LEGAL_ENTITY_ABC, JPY), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_ABC, CRD_ABC_JPY), Pair.of(LEGAL_ENTITY_DEF, JPY), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_DEF, CRD_DEF))).discountCurves(ImmutableMap.of(USD, DSC_USD, JPY, DSC_JPY)).recoveryRateCurves(ImmutableMap.of(LEGAL_ENTITY_ABC, RR_ABC, LEGAL_ENTITY_DEF, RR_DEF)).valuationDate(VALUATION).build();
		CurrencyParameterSensitivities computed = CurrencyParameterSensitivities.of(test.singleDiscountCurveParameterSensitivity(zeroPt.combinedWith(creditPt).combinedWith(fxPt).build(), USD));
		CurrencyParameterSensitivities expected = DSC_USD.parameterSensitivity(zeroPt);
		assertTrue(computed.equalWithTolerance(expected, 1.0e-14));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ImmutableCreditRatesProvider test1 = ImmutableCreditRatesProvider.builder().creditCurves(ImmutableMap.of(Pair.of(LEGAL_ENTITY_ABC, USD), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_ABC, CRD_ABC_USD))).discountCurves(ImmutableMap.of(USD, DSC_USD)).recoveryRateCurves(ImmutableMap.of(LEGAL_ENTITY_ABC, RR_ABC)).valuationDate(VALUATION).build();
		coverImmutableBean(test1);
		IsdaCreditDiscountFactors dsc = IsdaCreditDiscountFactors.of(JPY, VALUATION.plusDays(1), NAME_DSC_JPY, TIME_DSC_JPY, RATE_DSC_JPY, ACT_365F);
		IsdaCreditDiscountFactors hzd = IsdaCreditDiscountFactors.of(JPY, VALUATION.plusDays(1), NAME_CRD_DEF, TIME_CRD_DEF, RATE_CRD_DEF, ACT_365F);
		ConstantRecoveryRates rr = ConstantRecoveryRates.of(LEGAL_ENTITY_DEF, VALUATION.plusDays(1), RECOVERY_RATE_DEF);
		ImmutableCreditRatesProvider test2 = ImmutableCreditRatesProvider.builder().creditCurves(ImmutableMap.of(Pair.of(LEGAL_ENTITY_DEF, JPY), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_DEF, hzd))).discountCurves(ImmutableMap.of(JPY, dsc)).recoveryRateCurves(ImmutableMap.of(LEGAL_ENTITY_DEF, rr)).valuationDate(VALUATION.plusDays(1)).build();
		coverBeanEquals(test1, test2);
	  }

	}

}