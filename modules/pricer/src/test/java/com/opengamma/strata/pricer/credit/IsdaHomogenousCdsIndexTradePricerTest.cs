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
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.BUY;
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
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
	using DefaultCurveMetadata = com.opengamma.strata.market.curve.DefaultCurveMetadata;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PriceType = com.opengamma.strata.pricer.common.PriceType;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using CdsIndex = com.opengamma.strata.product.credit.CdsIndex;
	using ResolvedCdsIndex = com.opengamma.strata.product.credit.ResolvedCdsIndex;
	using ResolvedCdsIndexTrade = com.opengamma.strata.product.credit.ResolvedCdsIndexTrade;

	/// <summary>
	/// Test <seealso cref="IsdaHomogenousCdsIndexTradePricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IsdaHomogenousCdsIndexTradePricerTest
	public class IsdaHomogenousCdsIndexTradePricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VALUATION_DATE = LocalDate.of(2014, 1, 3);
	  private static readonly HolidayCalendarId CALENDAR = HolidayCalendarIds.USNY;
	  private static readonly StandardId INDEX_ID = StandardId.of("OG", "ABCXX");
	  private static readonly ImmutableList<StandardId> LEGAL_ENTITIES;
	  static IsdaHomogenousCdsIndexTradePricerTest()
	  {
		ImmutableList.Builder<StandardId> builder = ImmutableList.builder();
		for (int i = 0; i < 97; ++i)
		{
		  builder.add(StandardId.of("OG", i.ToString()));
		}
		LEGAL_ENTITIES = builder.build();
	  }

	  private static readonly DoubleArray TIME_YC = DoubleArray.ofUnsafe(new double[] {0.09041095890410959, 0.16712328767123288, 0.2547945205479452, 0.5041095890410959, 0.7534246575342466, 1.0054794520547945, 2.0054794520547947, 3.008219178082192, 4.013698630136987, 5.010958904109589, 6.008219178082192, 7.010958904109589, 8.01095890410959, 9.01095890410959, 10.016438356164384, 12.013698630136986, 15.021917808219179, 20.01917808219178, 30.024657534246575});
	  private static readonly DoubleArray RATE_YC = DoubleArray.ofUnsafe(new double[] {-0.002078655697855299, -0.001686438401304855, -0.0013445486228483379, -4.237819925898475E-4, 2.5142499469348057E-5, 5.935063895780138E-4, -3.247081037469503E-4, 6.147182786549223E-4, 0.0019060597240545122, 0.0033125742254568815, 0.0047766352312329455, 0.0062374324537341225, 0.007639664176639106, 0.008971003650150983, 0.010167545380711455, 0.012196853322376243, 0.01441082634734099, 0.016236611610989507, 0.01652439910865982});
	  private static readonly DefaultCurveMetadata METADATA_YC = DefaultCurveMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).curveName("yield").dayCount(ACT_365F).build();
	  private static readonly InterpolatedNodalCurve NODAL_YC = InterpolatedNodalCurve.of(METADATA_YC, TIME_YC, RATE_YC, CurveInterpolators.PRODUCT_LINEAR, CurveExtrapolators.FLAT, CurveExtrapolators.PRODUCT_LINEAR);
	  private static readonly IsdaCreditDiscountFactors YIELD_CRVE = IsdaCreditDiscountFactors.of(USD, VALUATION_DATE, NODAL_YC);

	  private static readonly DoubleArray TIME_CC = DoubleArray.ofUnsafe(new double[] {1.2054794520547945, 1.7095890410958905, 2.712328767123288, 3.712328767123288, 4.712328767123288, 5.712328767123288, 7.715068493150685, 10.717808219178082});
	  private static readonly DoubleArray RATE_CC = DoubleArray.ofUnsafe(new double[] {0.009950492020354761, 0.01203385973637765, 0.01418821591480718, 0.01684815168721049, 0.01974873350586718, 0.023084203422383043, 0.02696911931489543, 0.029605642651816415});
	  private static readonly double INDEX_FACTOR = 93d / 97d;
	  private static readonly DefaultCurveMetadata METADATA_CC = DefaultCurveMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).curveName("credit").dayCount(ACT_365F).addInfo(CurveInfoType.CDS_INDEX_FACTOR, INDEX_FACTOR).build();
	  private static readonly InterpolatedNodalCurve NODAL_CC = InterpolatedNodalCurve.of(METADATA_CC, TIME_CC, RATE_CC, CurveInterpolators.PRODUCT_LINEAR, CurveExtrapolators.FLAT, CurveExtrapolators.PRODUCT_LINEAR);
	  private static readonly CreditDiscountFactors CREDIT_CRVE = IsdaCreditDiscountFactors.of(USD, VALUATION_DATE, NODAL_CC);
	  private static readonly ConstantRecoveryRates RECOVERY_RATES = ConstantRecoveryRates.of(INDEX_ID, VALUATION_DATE, 0.25);
	  private static readonly CreditRatesProvider RATES_PROVIDER = ImmutableCreditRatesProvider.builder().valuationDate(VALUATION_DATE).creditCurves(ImmutableMap.of(Pair.of(INDEX_ID, USD), LegalEntitySurvivalProbabilities.of(INDEX_ID, CREDIT_CRVE))).discountCurves(ImmutableMap.of(USD, YIELD_CRVE)).recoveryRateCurves(ImmutableMap.of(INDEX_ID, RECOVERY_RATES)).build();

	  private const double NOTIONAL = 1.0e7;
	  private static readonly ResolvedCdsIndex PRODUCT = CdsIndex.of(BUY, INDEX_ID, LEGAL_ENTITIES, USD, NOTIONAL, LocalDate.of(2013, 12, 20), LocalDate.of(2020, 10, 20), P3M, CALENDAR, 0.015).resolve(REF_DATA);
	  private static readonly LocalDate SETTLEMENT_DATE = PRODUCT.SettlementDateOffset.adjust(VALUATION_DATE, REF_DATA);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.builder().tradeDate(VALUATION_DATE).settlementDate(SETTLEMENT_DATE).build();
	  private static readonly Payment UPFRONT = Payment.of(USD, -NOTIONAL * 0.2, SETTLEMENT_DATE);
	  private static readonly ResolvedCdsIndexTrade TRADE = ResolvedCdsIndexTrade.builder().product(PRODUCT).info(TRADE_INFO).upfrontFee(UPFRONT).build();
	  private static readonly ResolvedCdsIndexTrade TRADE_NO_SETTLE_DATE = ResolvedCdsIndexTrade.builder().product(PRODUCT).info(TradeInfo.of(VALUATION_DATE)).build();

	  private static readonly IsdaHomogenousCdsIndexTradePricer PRICER = IsdaHomogenousCdsIndexTradePricer.DEFAULT;
	  private static readonly IsdaHomogenousCdsIndexTradePricer PRICER_MF = new IsdaHomogenousCdsIndexTradePricer(AccrualOnDefaultFormula.MARKIT_FIX);
	  private static readonly IsdaHomogenousCdsIndexProductPricer PRICER_PRODUCT = IsdaHomogenousCdsIndexProductPricer.DEFAULT;
	  private static readonly IsdaHomogenousCdsIndexProductPricer PRICER_PRODUCT_MF = new IsdaHomogenousCdsIndexProductPricer(AccrualOnDefaultFormula.MARKIT_FIX);
	  private static readonly DiscountingPaymentPricer PRICER_PAYMENT = DiscountingPaymentPricer.DEFAULT;
	  private const double TOL = 1.0e-15;

	  public virtual void test_price()
	  {
		double computed = PRICER.price(TRADE, RATES_PROVIDER, PriceType.CLEAN, REF_DATA);
		double expected = PRICER_PRODUCT.price(PRODUCT, RATES_PROVIDER, SETTLEMENT_DATE, PriceType.CLEAN, REF_DATA);
		double computedMf = PRICER_MF.price(TRADE_NO_SETTLE_DATE, RATES_PROVIDER, PriceType.CLEAN, REF_DATA);
		double expectedMf = PRICER_PRODUCT_MF.price(PRODUCT, RATES_PROVIDER, SETTLEMENT_DATE, PriceType.CLEAN, REF_DATA);
		assertEquals(computed, expected, TOL);
		assertEquals(computedMf, expectedMf, TOL);
	  }

	  public virtual void test_parSpread()
	  {
		double computed = PRICER.parSpread(TRADE, RATES_PROVIDER, REF_DATA);
		double expected = PRICER_PRODUCT.parSpread(PRODUCT, RATES_PROVIDER, SETTLEMENT_DATE, REF_DATA);
		double computedMf = PRICER_MF.parSpread(TRADE_NO_SETTLE_DATE, RATES_PROVIDER, REF_DATA);
		double expectedMf = PRICER_PRODUCT_MF.parSpread(PRODUCT, RATES_PROVIDER, SETTLEMENT_DATE, REF_DATA);
		assertEquals(computed, expected, TOL);
		assertEquals(computedMf, expectedMf, TOL);
	  }

	  public virtual void test_priceSensitivity()
	  {
		PointSensitivities computed = PRICER.priceSensitivity(TRADE, RATES_PROVIDER, REF_DATA);
		PointSensitivities expected = PRICER_PRODUCT.priceSensitivity(PRODUCT, RATES_PROVIDER, SETTLEMENT_DATE, REF_DATA).build();
		PointSensitivities computedMf = PRICER_MF.priceSensitivity(TRADE_NO_SETTLE_DATE, RATES_PROVIDER, REF_DATA);
		PointSensitivities expectedMf = PRICER_PRODUCT_MF.priceSensitivity(PRODUCT, RATES_PROVIDER, SETTLEMENT_DATE, REF_DATA).build();
		assertTrue(computed.equalWithTolerance(expected, TOL));
		assertTrue(computedMf.equalWithTolerance(expectedMf, TOL));
	  }

	  public virtual void test_parSpreadSensitivity()
	  {
		PointSensitivities computed = PRICER.parSpreadSensitivity(TRADE, RATES_PROVIDER, REF_DATA);
		PointSensitivities expected = PRICER_PRODUCT.parSpreadSensitivity(PRODUCT, RATES_PROVIDER, SETTLEMENT_DATE, REF_DATA).build();
		PointSensitivities computedMf = PRICER_MF.parSpreadSensitivity(TRADE_NO_SETTLE_DATE, RATES_PROVIDER, REF_DATA);
		PointSensitivities expectedMf = PRICER_PRODUCT_MF.parSpreadSensitivity(PRODUCT, RATES_PROVIDER, SETTLEMENT_DATE, REF_DATA).build();
		assertTrue(computed.equalWithTolerance(expected, TOL));
		assertTrue(computedMf.equalWithTolerance(expectedMf, TOL));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		CurrencyAmount computed = PRICER.presentValue(TRADE, RATES_PROVIDER, PriceType.CLEAN, REF_DATA);
		CurrencyAmount expected = PRICER_PRODUCT.presentValue(PRODUCT, RATES_PROVIDER, VALUATION_DATE, PriceType.CLEAN, REF_DATA).plus(PRICER_PAYMENT.presentValue(UPFRONT, YIELD_CRVE.toDiscountFactors()));
		CurrencyAmount computedMf = PRICER_MF.presentValue(TRADE_NO_SETTLE_DATE, RATES_PROVIDER, PriceType.CLEAN, REF_DATA);
		CurrencyAmount expectedMf = PRICER_PRODUCT_MF.presentValue(PRODUCT, RATES_PROVIDER, VALUATION_DATE, PriceType.CLEAN, REF_DATA);
		assertEquals(computed.Amount, expected.Amount, TOL);
		assertEquals(computedMf.Amount, expectedMf.Amount, TOL);
	  }

	  public virtual void test_presentValueSensitivity()
	  {
		PointSensitivities computed = PRICER.presentValueSensitivity(TRADE, RATES_PROVIDER, REF_DATA);
		PointSensitivities expected = PRICER_PRODUCT.presentValueSensitivity(PRODUCT, RATES_PROVIDER, VALUATION_DATE, REF_DATA).combinedWith(PRICER_PAYMENT.presentValueSensitivity(UPFRONT, YIELD_CRVE.toDiscountFactors())).build();
		PointSensitivities computedMf = PRICER_MF.presentValueSensitivity(TRADE_NO_SETTLE_DATE, RATES_PROVIDER, REF_DATA);
		PointSensitivities expectedMf = PRICER_PRODUCT_MF.presentValueSensitivity(PRODUCT, RATES_PROVIDER, VALUATION_DATE, REF_DATA).build();
		assertTrue(computed.equalWithTolerance(expected, TOL));
		assertTrue(computedMf.equalWithTolerance(expectedMf, TOL));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueOnSettle()
	  {
		CurrencyAmount computed = PRICER.presentValueOnSettle(TRADE, RATES_PROVIDER, PriceType.CLEAN, REF_DATA);
		CurrencyAmount expected = PRICER_PRODUCT.presentValue(PRODUCT, RATES_PROVIDER, SETTLEMENT_DATE, PriceType.CLEAN, REF_DATA);
		CurrencyAmount computedMf = PRICER_MF.presentValueOnSettle(TRADE_NO_SETTLE_DATE, RATES_PROVIDER, PriceType.CLEAN, REF_DATA);
		CurrencyAmount expectedMf = PRICER_PRODUCT_MF.presentValue(PRODUCT, RATES_PROVIDER, SETTLEMENT_DATE, PriceType.CLEAN, REF_DATA);
		assertEquals(computed.Amount, expected.Amount, TOL);
		assertEquals(computedMf.Amount, expectedMf.Amount, TOL);
	  }

	  public virtual void test_rpv01OnSettle()
	  {
		CurrencyAmount computed = PRICER.rpv01OnSettle(TRADE, RATES_PROVIDER, PriceType.CLEAN, REF_DATA);
		CurrencyAmount expected = PRICER_PRODUCT.rpv01(PRODUCT, RATES_PROVIDER, SETTLEMENT_DATE, PriceType.CLEAN, REF_DATA);
		CurrencyAmount computedMf = PRICER_MF.rpv01OnSettle(TRADE_NO_SETTLE_DATE, RATES_PROVIDER, PriceType.CLEAN, REF_DATA);
		CurrencyAmount expectedMf = PRICER_PRODUCT_MF.rpv01(PRODUCT, RATES_PROVIDER, SETTLEMENT_DATE, PriceType.CLEAN, REF_DATA);
		assertEquals(computed.Amount, expected.Amount, TOL);
		assertEquals(computedMf.Amount, expectedMf.Amount, TOL);
	  }

	  public virtual void test_recovery01OnSettle()
	  {
		CurrencyAmount computed = PRICER.recovery01OnSettle(TRADE, RATES_PROVIDER, REF_DATA);
		CurrencyAmount expected = PRICER_PRODUCT.recovery01(PRODUCT, RATES_PROVIDER, SETTLEMENT_DATE, REF_DATA);
		CurrencyAmount computedMf = PRICER_MF.recovery01OnSettle(TRADE_NO_SETTLE_DATE, RATES_PROVIDER, REF_DATA);
		CurrencyAmount expectedMf = PRICER_PRODUCT_MF.recovery01(PRODUCT, RATES_PROVIDER, SETTLEMENT_DATE, REF_DATA);
		assertEquals(computed.Amount, expected.Amount, TOL);
		assertEquals(computedMf.Amount, expectedMf.Amount, TOL);
	  }

	  public virtual void test_presentValueOnSettleSensitivity()
	  {
		PointSensitivities computed = PRICER.presentValueOnSettleSensitivity(TRADE, RATES_PROVIDER, REF_DATA);
		PointSensitivities expected = PRICER_PRODUCT.presentValueSensitivity(PRODUCT, RATES_PROVIDER, SETTLEMENT_DATE, REF_DATA).build();
		PointSensitivities computedMf = PRICER_MF.presentValueOnSettleSensitivity(TRADE_NO_SETTLE_DATE, RATES_PROVIDER, REF_DATA);
		PointSensitivities expectedMf = PRICER_PRODUCT_MF.presentValueSensitivity(PRODUCT, RATES_PROVIDER, SETTLEMENT_DATE, REF_DATA).build();
		assertTrue(computed.equalWithTolerance(expected, TOL));
		assertTrue(computedMf.equalWithTolerance(expectedMf, TOL));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_jumpToDefault()
	  {
		JumpToDefault computed = PRICER.jumpToDefault(TRADE, RATES_PROVIDER, REF_DATA);
		JumpToDefault expected = PRICER_PRODUCT.jumpToDefault(PRODUCT, RATES_PROVIDER, SETTLEMENT_DATE, REF_DATA);
		assertEquals(computed, expected);
	  }

	  public virtual void test_expectedLoss()
	  {
		CurrencyAmount computed = PRICER.expectedLoss(TRADE, RATES_PROVIDER);
		CurrencyAmount expected = PRICER_PRODUCT.expectedLoss(PRODUCT, RATES_PROVIDER);
		assertEquals(computed, expected);
	  }

	}

}