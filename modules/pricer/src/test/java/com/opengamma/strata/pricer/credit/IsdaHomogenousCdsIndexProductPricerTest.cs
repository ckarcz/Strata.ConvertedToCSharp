using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.SAT_SUN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.common.PriceType.CLEAN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.common.PriceType.DIRTY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.BUY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.SELL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Builder = com.google.common.collect.ImmutableList.Builder;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using ValueType = com.opengamma.strata.market.ValueType;
	using ConstantNodalCurve = com.opengamma.strata.market.curve.ConstantNodalCurve;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
	using DefaultCurveMetadata = com.opengamma.strata.market.curve.DefaultCurveMetadata;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using PriceType = com.opengamma.strata.pricer.common.PriceType;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using CdsIndex = com.opengamma.strata.product.credit.CdsIndex;
	using ResolvedCdsIndex = com.opengamma.strata.product.credit.ResolvedCdsIndex;

	/// <summary>
	/// Test <seealso cref="IsdaHomogenousCdsIndexTradePricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IsdaHomogenousCdsIndexProductPricerTest
	public class IsdaHomogenousCdsIndexProductPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly StandardId INDEX_ID = StandardId.of("OG", "ABCXX");
	  private static readonly ImmutableList<StandardId> LEGAL_ENTITIES;
	  static IsdaHomogenousCdsIndexProductPricerTest()
	  {
		ImmutableList.Builder<StandardId> builder = ImmutableList.builder();
		for (int i = 0; i < 97; ++i)
		{
		  builder.add(StandardId.of("OG", i.ToString()));
		}
		LEGAL_ENTITIES = builder.build();
	  }
	  private static readonly LocalDate VALUATION_DATE = LocalDate.of(2014, 2, 13);
	  private static readonly DoubleArray TIME_YC = DoubleArray.ofUnsafe(new double[] {0.08767123287671233, 0.1726027397260274, 0.2602739726027397, 0.5095890410958904, 1.010958904109589, 2.010958904109589, 3.0136986301369864, 4.0191780821917815, 5.016438356164384, 6.013698630136987, 7.016438356164384, 8.016438356164384, 9.016438356164384, 10.021917808219179, 12.01917808219178, 15.027397260273974, 20.024657534246575, 25.027397260273972, 30.030136986301372});
	  private static readonly DoubleArray RATE_YC = DoubleArray.ofUnsafe(new double[] {0.0015967771993938666, 0.002000101499768777, 0.002363431670279865, 0.003338175293899776, 0.005634608399714134, 0.00440326902435394, 0.007809961130263494, 0.011941089607974827, 0.015908558015433557, 0.019426790989545677, 0.022365655212981644, 0.02480329609280203, 0.02681632723967965, 0.028566047406753222, 0.031343018999443514, 0.03409375145707815, 0.036451406286344155, 0.0374228389649933, 0.037841116301420584});
	  private static readonly DefaultCurveMetadata METADATA_YC = DefaultCurveMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).curveName("yield").dayCount(ACT_365F).build();
	  private static readonly InterpolatedNodalCurve NODAL_YC = InterpolatedNodalCurve.of(METADATA_YC, TIME_YC, RATE_YC, CurveInterpolators.PRODUCT_LINEAR, CurveExtrapolators.FLAT, CurveExtrapolators.PRODUCT_LINEAR);
	  private const double TIME_CC_SINGLE = 4.852054794520548;
	  private const double RATE_CC_SINGLE = 0.04666317621551129;
	  private static readonly double INDEX_FACTOR = 93d / 97d;
	  private static readonly DefaultCurveMetadata METADATA_CC_SINGLE = DefaultCurveMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).curveName("credit_single").dayCount(ACT_365F).addInfo(CurveInfoType.CDS_INDEX_FACTOR, INDEX_FACTOR).build();
	  private static readonly ConstantNodalCurve NODAL_CC_SINGLE = ConstantNodalCurve.of(METADATA_CC_SINGLE, TIME_CC_SINGLE, RATE_CC_SINGLE);
	  private static readonly DoubleArray TIME_CC = DoubleArray.ofUnsafe(new double[] {1.2054794520547945, 1.7095890410958905, 2.712328767123288, 3.712328767123288, 4.712328767123288, 5.712328767123288, 7.715068493150685, 10.717808219178082});
	  private static readonly DoubleArray RATE_CC = DoubleArray.ofUnsafe(new double[] {0.009950492020354761, 0.01203385973637765, 0.01418821591480718, 0.01684815168721049, 0.01974873350586718, 0.023084203422383043, 0.02696911931489543, 0.029605642651816415});
	  private static readonly DefaultCurveMetadata METADATA_CC = DefaultCurveMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).curveName("credit").dayCount(ACT_365F).addInfo(CurveInfoType.CDS_INDEX_FACTOR, INDEX_FACTOR).build();
	  private static readonly InterpolatedNodalCurve NODAL_CC = InterpolatedNodalCurve.of(METADATA_CC, TIME_CC, RATE_CC, CurveInterpolators.PRODUCT_LINEAR, CurveExtrapolators.FLAT, CurveExtrapolators.PRODUCT_LINEAR);
	  private const double RECOVERY_RATE = 0.3;
	  private static readonly CreditRatesProvider RATES_PROVIDER_SINGLE = createCreditRatesProviderSingle(VALUATION_DATE, true);
	  private static readonly CreditRatesProvider RATES_PROVIDER = createCreditRatesProviderSingle(VALUATION_DATE, false);

	  private const double NOTIONAL = 1.0e8;
	  private static readonly LocalDate START_DATE = LocalDate.of(2013, 12, 20);
	  private static readonly LocalDate MATURITY_DATE = LocalDate.of(2018, 12, 20);
	  private const double COUPON = 0.05;
	  private static readonly ResolvedCdsIndex PRODUCT = CdsIndex.of(BUY, INDEX_ID, LEGAL_ENTITIES, USD, NOTIONAL, START_DATE, MATURITY_DATE, P3M, SAT_SUN, COUPON).resolve(REF_DATA);
	  private static readonly ResolvedCdsIndex PRODUCT_SELL = CdsIndex.of(SELL, INDEX_ID, LEGAL_ENTITIES, USD, NOTIONAL, START_DATE, MATURITY_DATE, P3M, SAT_SUN, COUPON).resolve(REF_DATA);
	  private static readonly LocalDate SETTLEMENT_STD = PRODUCT.SettlementDateOffset.adjust(VALUATION_DATE, REF_DATA);

	  private const double TOL = 1.0e-14;
	  private const double EPS = 1.0e-6;
	  private static readonly IsdaHomogenousCdsIndexProductPricer PRICER = IsdaHomogenousCdsIndexProductPricer.DEFAULT;
	  private static readonly IsdaHomogenousCdsIndexProductPricer PRICER_MARKIT = new IsdaHomogenousCdsIndexProductPricer(AccrualOnDefaultFormula.MARKIT_FIX);
	  private static readonly IsdaHomogenousCdsIndexProductPricer PRICER_OG = new IsdaHomogenousCdsIndexProductPricer(AccrualOnDefaultFormula.CORRECT);
	  private static readonly RatesFiniteDifferenceSensitivityCalculator CALC_FD = new RatesFiniteDifferenceSensitivityCalculator(EPS);

	  //-------------------------------------------------------------------------
	  public virtual void accFormulaTest()
	  {
		assertEquals(PRICER.AccrualOnDefaultFormula, AccrualOnDefaultFormula.ORIGINAL_ISDA);
		assertEquals(PRICER_MARKIT.AccrualOnDefaultFormula, AccrualOnDefaultFormula.MARKIT_FIX);
		assertEquals(PRICER_OG.AccrualOnDefaultFormula, AccrualOnDefaultFormula.CORRECT);
	  }

	  public virtual void test_regression()
	  {
		CurrencyAmount cleanPvOg = PRICER_OG.presentValue(PRODUCT, RATES_PROVIDER_SINGLE, SETTLEMENT_STD, CLEAN, REF_DATA);
		assertEquals(cleanPvOg.Amount, -7305773.195876285, NOTIONAL * TOL);
		assertEquals(cleanPvOg.Currency, USD);
		CurrencyAmount dirtyPvOg = PRICER_OG.presentValue(PRODUCT, RATES_PROVIDER_SINGLE, SETTLEMENT_STD, DIRTY, REF_DATA);
		assertEquals(dirtyPvOg.Amount, -8051477.663230239, NOTIONAL * TOL);
		assertEquals(dirtyPvOg.Currency, USD);
		double cleanPriceOg = PRICER_OG.price(PRODUCT, RATES_PROVIDER_SINGLE, SETTLEMENT_STD, CLEAN, REF_DATA);
		assertEquals(cleanPriceOg, -0.07619999999999996, TOL);
		double dirtyPriceOg = PRICER_OG.price(PRODUCT, RATES_PROVIDER_SINGLE, SETTLEMENT_STD, DIRTY, REF_DATA);
		assertEquals(dirtyPriceOg, -0.08397777777777776, TOL);
	  }

	  public virtual void endedTest()
	  {
		LocalDate valuationDate = PRODUCT.ProtectionEndDate.plusDays(1);
		CreditRatesProvider provider = createCreditRatesProviderSingle(valuationDate, false);
		double price = PRICER.price(PRODUCT, provider, SETTLEMENT_STD, CLEAN, REF_DATA);
		assertEquals(price, 0d);
		CurrencyAmount pv = PRICER.presentValue(PRODUCT, provider, SETTLEMENT_STD, CLEAN, REF_DATA);
		assertEquals(pv, CurrencyAmount.zero(USD));
		assertThrowsIllegalArg(() => PRICER.parSpread(PRODUCT, provider, SETTLEMENT_STD, REF_DATA));
		CurrencyAmount rpv01 = PRICER.rpv01(PRODUCT, provider, SETTLEMENT_STD, CLEAN, REF_DATA);
		assertEquals(rpv01, CurrencyAmount.zero(USD));
		CurrencyAmount recovery01 = PRICER.recovery01(PRODUCT, provider, SETTLEMENT_STD, REF_DATA);
		assertEquals(recovery01, CurrencyAmount.zero(USD));
		PointSensitivityBuilder sensi = PRICER.presentValueSensitivity(PRODUCT, provider, SETTLEMENT_STD, REF_DATA);
		assertEquals(sensi, PointSensitivityBuilder.none());
		PointSensitivityBuilder sensiPrice = PRICER.priceSensitivity(PRODUCT, provider, SETTLEMENT_STD, REF_DATA);
		assertEquals(sensiPrice, PointSensitivityBuilder.none());
		assertThrowsIllegalArg(() => PRICER.parSpreadSensitivity(PRODUCT, provider, SETTLEMENT_STD, REF_DATA));
		JumpToDefault jumpToDefault = PRICER.jumpToDefault(PRODUCT, provider, SETTLEMENT_STD, REF_DATA);
		assertEquals(jumpToDefault, JumpToDefault.of(USD, ImmutableMap.of(INDEX_ID, 0d)));
		CurrencyAmount expectedLoss = PRICER.expectedLoss(PRODUCT, provider);
		assertEquals(expectedLoss, CurrencyAmount.zero(USD));
	  }

	  public virtual void consistencyTest()
	  {
		CurrencyAmount pv = PRICER.presentValue(PRODUCT, RATES_PROVIDER, SETTLEMENT_STD, CLEAN, REF_DATA);
		CurrencyAmount pvSell = PRICER.presentValue(PRODUCT_SELL, RATES_PROVIDER, SETTLEMENT_STD, CLEAN, REF_DATA);
		CurrencyAmount rpv01 = PRICER.rpv01(PRODUCT, RATES_PROVIDER, SETTLEMENT_STD, CLEAN, REF_DATA);
		CurrencyAmount rpv01Sell = PRICER.rpv01(PRODUCT_SELL, RATES_PROVIDER, SETTLEMENT_STD, CLEAN, REF_DATA);
		CurrencyAmount recovery01 = PRICER.recovery01(PRODUCT, RATES_PROVIDER, SETTLEMENT_STD, REF_DATA);
		CurrencyAmount recovery01Sell = PRICER.recovery01(PRODUCT_SELL, RATES_PROVIDER, SETTLEMENT_STD, REF_DATA);
		double spread = PRICER.parSpread(PRODUCT, RATES_PROVIDER, SETTLEMENT_STD, REF_DATA);
		assertEquals(pv.Currency, USD);
		assertEquals(pvSell.Currency, USD);
		assertEquals(rpv01.Currency, USD);
		assertEquals(rpv01Sell.Currency, USD);
		assertEquals(recovery01.Currency, USD);
		assertEquals(recovery01Sell.Currency, USD);
		assertEquals(pv.Amount, -(1d - RECOVERY_RATE) * recovery01.Amount - PRODUCT.FixedRate * rpv01.Amount, NOTIONAL * TOL);
		assertEquals(pv.Amount, -pvSell.Amount, NOTIONAL * TOL);
		assertEquals(rpv01.Amount, -rpv01Sell.Amount, NOTIONAL * TOL);
		assertEquals(recovery01.Amount, -recovery01Sell.Amount, NOTIONAL * TOL);
		assertEquals(spread, -(1d - RECOVERY_RATE) * recovery01.Amount / rpv01.Amount, TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void pvSensitivityTest()
	  {
		PointSensitivityBuilder point = PRICER.presentValueSensitivity(PRODUCT, RATES_PROVIDER, SETTLEMENT_STD, REF_DATA);
		CurrencyParameterSensitivities res = RATES_PROVIDER.parameterSensitivity(point.build());
		CurrencyParameterSensitivities exp = CALC_FD.sensitivity(RATES_PROVIDER, p => PRICER.presentValue(PRODUCT, p, SETTLEMENT_STD, CLEAN, REF_DATA));
		equalWithRelativeTolerance(res, exp, NOTIONAL * EPS);
		PointSensitivityBuilder pointMarkit = PRICER_MARKIT.presentValueSensitivity(PRODUCT, RATES_PROVIDER, SETTLEMENT_STD, REF_DATA);
		CurrencyParameterSensitivities resMarkit = RATES_PROVIDER.parameterSensitivity(pointMarkit.build());
		CurrencyParameterSensitivities expMarkit = CALC_FD.sensitivity(RATES_PROVIDER, p => PRICER_MARKIT.presentValue(PRODUCT, p, SETTLEMENT_STD, CLEAN, REF_DATA));
		equalWithRelativeTolerance(resMarkit, expMarkit, NOTIONAL * EPS);
		PointSensitivityBuilder pointOg = PRICER_OG.presentValueSensitivity(PRODUCT, RATES_PROVIDER, SETTLEMENT_STD, REF_DATA);
		CurrencyParameterSensitivities resOg = RATES_PROVIDER.parameterSensitivity(pointOg.build());
		CurrencyParameterSensitivities expOg = CALC_FD.sensitivity(RATES_PROVIDER, p => PRICER_OG.presentValue(PRODUCT, p, SETTLEMENT_STD, CLEAN, REF_DATA));
		equalWithRelativeTolerance(resOg, expOg, NOTIONAL * EPS);
	  }

	  public virtual void priceSensitivityTest()
	  {
		PointSensitivityBuilder point = PRICER.priceSensitivity(PRODUCT, RATES_PROVIDER, SETTLEMENT_STD, REF_DATA);
		CurrencyParameterSensitivities res = RATES_PROVIDER.parameterSensitivity(point.build());
		CurrencyParameterSensitivities exp = CALC_FD.sensitivity(RATES_PROVIDER, p => CurrencyAmount.of(USD, PRICER.price(PRODUCT, p, SETTLEMENT_STD, CLEAN, REF_DATA)));
		equalWithRelativeTolerance(res, exp, NOTIONAL * EPS);
		PointSensitivityBuilder pointMarkit = PRICER_MARKIT.priceSensitivity(PRODUCT, RATES_PROVIDER, SETTLEMENT_STD, REF_DATA);
		CurrencyParameterSensitivities resMarkit = RATES_PROVIDER.parameterSensitivity(pointMarkit.build());
		CurrencyParameterSensitivities expMarkit = CALC_FD.sensitivity(RATES_PROVIDER, p => CurrencyAmount.of(USD, PRICER_MARKIT.price(PRODUCT, p, SETTLEMENT_STD, CLEAN, REF_DATA)));
		equalWithRelativeTolerance(resMarkit, expMarkit, NOTIONAL * EPS);
		PointSensitivityBuilder pointOg = PRICER_OG.priceSensitivity(PRODUCT, RATES_PROVIDER, SETTLEMENT_STD, REF_DATA);
		CurrencyParameterSensitivities resOg = RATES_PROVIDER.parameterSensitivity(pointOg.build());
		CurrencyParameterSensitivities expOg = CALC_FD.sensitivity(RATES_PROVIDER, p => CurrencyAmount.of(USD, PRICER_OG.price(PRODUCT, p, SETTLEMENT_STD, CLEAN, REF_DATA)));
		equalWithRelativeTolerance(resOg, expOg, NOTIONAL * EPS);
	  }

	  public virtual void parSpreadSensitivityTest()
	  {
		PointSensitivityBuilder point = PRICER.parSpreadSensitivity(PRODUCT, RATES_PROVIDER, SETTLEMENT_STD, REF_DATA);
		CurrencyParameterSensitivities res = RATES_PROVIDER.parameterSensitivity(point.build());
		CurrencyParameterSensitivities exp = CALC_FD.sensitivity(RATES_PROVIDER, p => CurrencyAmount.of(USD, PRICER.parSpread(PRODUCT, p, SETTLEMENT_STD, REF_DATA)));
		equalWithRelativeTolerance(res, exp, NOTIONAL * EPS);
		PointSensitivityBuilder pointMarkit = PRICER_MARKIT.parSpreadSensitivity(PRODUCT, RATES_PROVIDER, SETTLEMENT_STD, REF_DATA);
		CurrencyParameterSensitivities resMarkit = RATES_PROVIDER.parameterSensitivity(pointMarkit.build());
		CurrencyParameterSensitivities expMarkit = CALC_FD.sensitivity(RATES_PROVIDER, p => CurrencyAmount.of(USD, PRICER_MARKIT.parSpread(PRODUCT, p, SETTLEMENT_STD, REF_DATA)));
		equalWithRelativeTolerance(resMarkit, expMarkit, NOTIONAL * EPS);
		PointSensitivityBuilder pointOg = PRICER_OG.parSpreadSensitivity(PRODUCT, RATES_PROVIDER, SETTLEMENT_STD, REF_DATA);
		CurrencyParameterSensitivities resOg = RATES_PROVIDER.parameterSensitivity(pointOg.build());
		CurrencyParameterSensitivities expOg = CALC_FD.sensitivity(RATES_PROVIDER, p => CurrencyAmount.of(USD, PRICER_OG.parSpread(PRODUCT, p, SETTLEMENT_STD, REF_DATA)));
		equalWithRelativeTolerance(resOg, expOg, NOTIONAL * EPS);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void jumpToDefaultTest()
	  {
		JumpToDefault computed = PRICER.jumpToDefault(PRODUCT, RATES_PROVIDER, SETTLEMENT_STD, REF_DATA);
		LocalDate stepinDate = PRODUCT.StepinDateOffset.adjust(VALUATION_DATE, REF_DATA);
		double dirtyPvMod = PRICER.presentValue(PRODUCT, RATES_PROVIDER, SETTLEMENT_STD, PriceType.DIRTY, REF_DATA).Amount / INDEX_FACTOR;
		double accrued = PRODUCT.accruedYearFraction(stepinDate) * PRODUCT.FixedRate * PRODUCT.BuySell.normalize(NOTIONAL);
		double protection = PRODUCT.BuySell.normalize(NOTIONAL) * (1d - RECOVERY_RATE);
		double expected = (protection - accrued - dirtyPvMod) / ((double) LEGAL_ENTITIES.size());
		assertEquals(computed.Currency, USD);
		assertTrue(computed.Amounts.size() == 1);
		assertEquals(computed.Amounts.get(INDEX_ID), expected, NOTIONAL * TOL);

	  }

	  public virtual void expectedLossTest()
	  {
		CurrencyAmount computed = PRICER.expectedLoss(PRODUCT, RATES_PROVIDER);
		double survivalProbability = RATES_PROVIDER.survivalProbabilities(INDEX_ID, USD).survivalProbability(PRODUCT.ProtectionEndDate);
		double expected = (1d - RECOVERY_RATE) * (1d - survivalProbability) * NOTIONAL * INDEX_FACTOR;
		assertEquals(computed.Currency, USD);
		assertEquals(computed.Amount, expected, NOTIONAL * TOL);
	  }

	  //-------------------------------------------------------------------------
	  private static CreditRatesProvider createCreditRatesProviderSingle(LocalDate valuationDate, bool isSingle)
	  {
		IsdaCreditDiscountFactors yc = IsdaCreditDiscountFactors.of(USD, valuationDate, NODAL_YC);
		CreditDiscountFactors cc = isSingle ? IsdaCreditDiscountFactors.of(USD, valuationDate, NODAL_CC_SINGLE) : IsdaCreditDiscountFactors.of(USD, valuationDate, NODAL_CC);
		ConstantRecoveryRates rr = ConstantRecoveryRates.of(INDEX_ID, valuationDate, RECOVERY_RATE);
		return ImmutableCreditRatesProvider.builder().valuationDate(valuationDate).creditCurves(ImmutableMap.of(Pair.of(INDEX_ID, USD), LegalEntitySurvivalProbabilities.of(INDEX_ID, cc))).discountCurves(ImmutableMap.of(USD, yc)).recoveryRateCurves(ImmutableMap.of(INDEX_ID, rr)).build();
	  }

	  private void equalWithRelativeTolerance(CurrencyParameterSensitivities computed, CurrencyParameterSensitivities expected, double tolerance)
	  {

		IList<CurrencyParameterSensitivity> mutable = new List<CurrencyParameterSensitivity>(expected.Sensitivities);
		// for each sensitivity in this instance, find matching in other instance
		foreach (CurrencyParameterSensitivity sens1 in computed.Sensitivities)
		{
		  // list is already sorted so binary search is safe
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		  int index = Collections.binarySearch(mutable, sens1, CurrencyParameterSensitivity::compareKey);
		  if (index >= 0)
		  {
			// matched, so must be equal
			CurrencyParameterSensitivity sens2 = mutable[index];
			equalZeroWithRelativeTolerance(sens1.Sensitivity, sens2.Sensitivity, tolerance);
			mutable.RemoveAt(index);
		  }
		  else
		  {
			// did not match, so must be zero
			assertTrue(sens1.Sensitivity.equalZeroWithTolerance(tolerance));
		  }
		}
		// all that remain from other instance must be zero
		foreach (CurrencyParameterSensitivity sens2 in mutable)
		{
		  assertTrue(sens2.Sensitivity.equalZeroWithTolerance(tolerance));
		}
	  }

	  private void equalZeroWithRelativeTolerance(DoubleArray computed, DoubleArray expected, double tolerance)
	  {
		int size = expected.size();
		assertEquals(size, computed.size());
		for (int i = 0; i < size; i++)
		{
		  double @ref = Math.Max(1d, Math.Abs(expected.get(i)));
		  assertEquals(computed.get(i), expected.get(i), tolerance * @ref);
		}
	  }

	}

}