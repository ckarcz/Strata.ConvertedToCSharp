using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond.e2e
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.JPY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using Rounding = com.opengamma.strata.basics.value.Rounding;
	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
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
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using TradedPrice = com.opengamma.strata.product.TradedPrice;
	using BondFuture = com.opengamma.strata.product.bond.BondFuture;
	using FixedCouponBond = com.opengamma.strata.product.bond.FixedCouponBond;
	using FixedCouponBondYieldConvention = com.opengamma.strata.product.bond.FixedCouponBondYieldConvention;
	using ResolvedBondFuture = com.opengamma.strata.product.bond.ResolvedBondFuture;
	using ResolvedBondFutureTrade = com.opengamma.strata.product.bond.ResolvedBondFutureTrade;

	/// <summary>
	/// End to end test on JPY-dominated trades.
	/// <para>
	/// The trades involve futures contract on 10yr bonds.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Test public class BondFuturesJpyEnd2EndTest
	public class BondFuturesJpyEnd2EndTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double ONE_PERCENT = 1.0E-2;
	  private const double ONE_BASIS_POINT = 1.0E-4;
	  private const double HUNDRED = 100d;
	  private const double TOL = 1.0E-14;
	  private static readonly LocalDate VALUATION = LocalDate.of(2016, 2, 10);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.builder().tradeDate(VALUATION.minusDays(1)).build();
	  private const double NOTIONAL = 100_000_000D;
	  private const long QUANTITY = 1L;
	  // Fixed coupon bonds
	  private static readonly LegalEntityId ISSUER_ID = LegalEntityId.of("OG-Ticker", "GOVT");
	  private static readonly LegalEntityGroup GROUP_ISSUER = LegalEntityGroup.of("GOVT");
	  private static readonly RepoGroup GROUP_REPO = RepoGroup.of("GOVT BONDS");
	  private const FixedCouponBondYieldConvention YIELD_CONVENTION = FixedCouponBondYieldConvention.JP_SIMPLE;
	  /// <summary>
	  /// Notional of underlying bond </summary>
	  private static readonly HolidayCalendarId CALENDAR = HolidayCalendarIds.JPTO;
	  private static readonly DaysAdjustment SETTLEMENT_DAYS = DaysAdjustment.ofBusinessDays(3, CALENDAR);
	  private static readonly DayCount DAY_COUNT = DayCounts.NL_365;
	  private static readonly BusinessDayAdjustment BUSINESS_ADJUST = BusinessDayAdjustment.of(FOLLOWING, CALENDAR);
	  private const int NB_UND_BONDS = 14;
	  private static readonly double[] UND_RATES = new double[] {0.6, 0.8, 0.8, 0.6, 0.6, 0.6, 0.6, 0.5, 0.5, 0.3, 0.4, 0.4, 0.4, 0.3};
	  private static readonly LocalDate[] START_DATE = new LocalDate[] {LocalDate.of(2015, 3, 20), LocalDate.of(2015, 6, 20), LocalDate.of(2015, 9, 20), LocalDate.of(2015, 9, 20), LocalDate.of(2015, 12, 20), LocalDate.of(2015, 3, 20), LocalDate.of(2015, 6, 20), LocalDate.of(2015, 9, 20), LocalDate.of(2015, 12, 20), LocalDate.of(2015, 12, 20), LocalDate.of(2015, 3, 20), LocalDate.of(2015, 6, 20), LocalDate.of(2015, 9, 20), LocalDate.of(2015, 12, 20)};
	  private static readonly LocalDate[] MATURITY_DATE = new LocalDate[] {LocalDate.of(2023, 3, 20), LocalDate.of(2023, 6, 20), LocalDate.of(2023, 9, 20), LocalDate.of(2023, 9, 20), LocalDate.of(2023, 12, 20), LocalDate.of(2024, 3, 20), LocalDate.of(2024, 6, 20), LocalDate.of(2024, 9, 20), LocalDate.of(2024, 12, 20), LocalDate.of(2024, 12, 20), LocalDate.of(2025, 3, 20), LocalDate.of(2025, 6, 20), LocalDate.of(2025, 9, 20), LocalDate.of(2025, 12, 20)};
	  private static readonly StandardId[] BOND_SECURITY_ID = new StandardId[] {StandardId.of("OG-Ticker", "GOVT-BOND0"), StandardId.of("OG-Ticker", "GOVT-BOND1"), StandardId.of("OG-Ticker", "GOVT-BOND2"), StandardId.of("OG-Ticker", "GOVT-BOND3"), StandardId.of("OG-Ticker", "GOVT-BOND4"), StandardId.of("OG-Ticker", "GOVT-BOND5"), StandardId.of("OG-Ticker", "GOVT-BOND6"), StandardId.of("OG-Ticker", "GOVT-BOND7"), StandardId.of("OG-Ticker", "GOVT-BOND8"), StandardId.of("OG-Ticker", "GOVT-BOND9"), StandardId.of("OG-Ticker", "GOVT-BOND10"), StandardId.of("OG-Ticker", "GOVT-BOND11"), StandardId.of("OG-Ticker", "GOVT-BOND12"), StandardId.of("OG-Ticker", "GOVT-BOND13")};
	  private static readonly FixedCouponBond[] UND_BOND = new FixedCouponBond[NB_UND_BONDS];
	  static BondFuturesJpyEnd2EndTest()
	  {
		for (int i = 0; i < NB_UND_BONDS; ++i)
		{
		  PeriodicSchedule periodSchedule = PeriodicSchedule.of(START_DATE[i], MATURITY_DATE[i], Frequency.P6M, BUSINESS_ADJUST, StubConvention.SHORT_INITIAL, false);
		  FixedCouponBond product = FixedCouponBond.builder().securityId(SecurityId.of(BOND_SECURITY_ID[i])).dayCount(DAY_COUNT).fixedRate(UND_RATES[i] * ONE_PERCENT).legalEntityId(ISSUER_ID).currency(JPY).notional(NOTIONAL).accrualSchedule(periodSchedule).settlementDateOffset(SETTLEMENT_DAYS).yieldConvention(YIELD_CONVENTION).build();
		  UND_BOND[i] = product;
		}
		UND_BOND_SEP = new FixedCouponBond[NB_UND_BONDS - 2];
		Array.Copy(UND_BOND, 2, UND_BOND_SEP, 0, NB_UND_BONDS - 2);
		UND_BOND_JUN = new FixedCouponBond[NB_UND_BONDS - 1];
		Array.Copy(UND_BOND, 1, UND_BOND_JUN, 0, NB_UND_BONDS - 1);
		double[] timeIssuer = new double[] {0.25136612021857924, 0.4972677595628415, 1.0139980537465378, 2.013998053746538, 2.857833670184894, 3.857833670184894, 4.860655737704918, 5.857833670184894, 7.104409012650647, 7.857833670184894, 8.857923497267759, 9.863313122239688, 14.857833670184894, 19.857833670184895, 29.857833670184895, 39.11262819073284};
	  double[] rateIssuer = new double[] {-0.0013117084834668065, -0.0010851901424876163, -0.0020906775838723216, -0.0022137102045172784, -0.0022695678374162888, -0.0023424568490920798, -0.0021603059162879916, -0.0021667343131861225, -0.0018285921969274823, -0.001355094018965514, -6.763044056712535E-4, 1.9555294306801752E-4, 0.003944125562941363, 0.008054233458390252, 0.012276105941434846, 0.013537766297065804};
	  double[] timeRepo = new double[] {0.00273224043715847, 0.01912568306010929, 0.040983606557377046, 0.05737704918032787, 0.07923497267759563, 0.2459016393442623, 0.4972677595628415, 1.0002994236095515};
	  double[] rateRepo = new double[] {2.599662058772748E-4, -8.403529976927196E-4, -0.0010105103936934236, -0.0011506617573950931, -0.0012708071334455143, -0.00146106683851595, -0.0014710815100096722, -0.001481096281798276};
		CurveMetadata metaIssuer = Curves.zeroRates(ISSUER_CURVE_NAME, ACT_ACT_ISDA);
		InterpolatedNodalCurve curveIssuer = InterpolatedNodalCurve.of(metaIssuer, DoubleArray.copyOf(timeIssuer), DoubleArray.copyOf(rateIssuer), INTERPOLATOR);
		DiscountFactors dscIssuer = ZeroRateDiscountFactors.of(JPY, VALUATION, curveIssuer);
		CurveMetadata metaRepo = Curves.zeroRates(REPO_CURVE_NAME, ACT_ACT_ISDA);
		InterpolatedNodalCurve curve = InterpolatedNodalCurve.of(metaRepo, DoubleArray.copyOf(timeRepo), DoubleArray.copyOf(rateRepo), INTERPOLATOR);
		DiscountFactors dscRepo = ZeroRateDiscountFactors.of(JPY, VALUATION, curve);
		LED_PROVIDER = ImmutableLegalEntityDiscountingProvider.builder().issuerCurves(ImmutableMap.of(Pair.of(GROUP_ISSUER, JPY), dscIssuer)).issuerCurveGroups(ImmutableMap.of(ISSUER_ID, GROUP_ISSUER)).repoCurves(ImmutableMap.of(Pair.of(GROUP_REPO, JPY), dscRepo)).repoCurveGroups(ImmutableMap.of(ISSUER_ID, GROUP_REPO)).build();
	  }
	  private static readonly DaysAdjustment LAST_TRADE_ADJUST = DaysAdjustment.ofBusinessDays(-5, CALENDAR);
	  // Futures in September 2016
	  private static readonly FixedCouponBond[] UND_BOND_SEP;
	  private static readonly double?[] CF_SEP = new double?[] {0.706302, 0.695006, 0.686265, 0.677675, 0.669189, 0.654569, 0.646180, 0.633317, 0.631348, 0.623057, 0.614901, 0.599832};
	  private static readonly LocalDate EFFECTIVE_DATE_SEP = LocalDate.of(2016, 9, 20);
	  private static readonly LocalDate FIRST_DELIVERY_DATE_SEP = BUSINESS_ADJUST.adjust(EFFECTIVE_DATE_SEP, REF_DATA);
	  private static readonly LocalDate LAST_DELIVERY_DATE_SEP = BUSINESS_ADJUST.adjust(EFFECTIVE_DATE_SEP, REF_DATA);
	  private static readonly LocalDate LAST_TRADING_DATE_SEP = LAST_TRADE_ADJUST.adjust(LAST_DELIVERY_DATE_SEP, REF_DATA);
	  private static readonly LocalDate FIRST_NOTICE_DATE_SEP = LAST_TRADE_ADJUST.adjust(LAST_DELIVERY_DATE_SEP, REF_DATA);
	  private static readonly LocalDate LAST_NOTICE_DATE_SEP = LAST_TRADE_ADJUST.adjust(LAST_DELIVERY_DATE_SEP, REF_DATA);
	  private static readonly SecurityId FUTURE_SECURITY_ID_SEP = SecurityId.of("OG-Ticker", "GOVT-BOND-FUT-SEP");
	  private static readonly ResolvedBondFuture FUTURE_PRODUCT_SEP = BondFuture.builder().securityId(FUTURE_SECURITY_ID_SEP).deliveryBasket(UND_BOND_SEP).conversionFactors(CF_SEP).firstNoticeDate(FIRST_NOTICE_DATE_SEP).lastNoticeDate(LAST_NOTICE_DATE_SEP).firstDeliveryDate(FIRST_DELIVERY_DATE_SEP).lastDeliveryDate(LAST_DELIVERY_DATE_SEP).lastTradeDate(LAST_TRADING_DATE_SEP).rounding(Rounding.ofDecimalPlaces(2)).build().resolve(REF_DATA);
	  private static readonly ResolvedBondFutureTrade FUTURE_TRADE_SEP = ResolvedBondFutureTrade.builder().quantity(QUANTITY).product(FUTURE_PRODUCT_SEP).info(TRADE_INFO).build();
	  private const double REF_PRICE_SEP = 151.61;
	  // Futures in June 2016
	  private static readonly double?[] CF_JUN = new double?[] {0.706302, 0.697881, 0.686265, 0.677675, 0.669189, 0.660850, 0.646180, 0.637931, 0.624765, 0.623057, 0.614901, 0.606851, 0.591771};
	  private static readonly FixedCouponBond[] UND_BOND_JUN;
	  private static readonly LocalDate EFFECTIVE_DATE_JUN = LocalDate.of(2016, 6, 20);
	  private static readonly LocalDate FIRST_DELIVERY_DATE_JUN = BUSINESS_ADJUST.adjust(EFFECTIVE_DATE_JUN, REF_DATA);
	  private static readonly LocalDate LAST_DELIVERY_DATE_JUN = BUSINESS_ADJUST.adjust(EFFECTIVE_DATE_JUN, REF_DATA);
	  private static readonly LocalDate LAST_TRADING_DATE_JUN = LAST_TRADE_ADJUST.adjust(LAST_DELIVERY_DATE_JUN, REF_DATA);
	  private static readonly LocalDate FIRST_NOTICE_DATE_JUN = LAST_TRADE_ADJUST.adjust(LAST_DELIVERY_DATE_JUN, REF_DATA);
	  private static readonly LocalDate LAST_NOTICE_DATE_JUN = LAST_TRADE_ADJUST.adjust(LAST_DELIVERY_DATE_JUN, REF_DATA);
	  private static readonly SecurityId FUTURE_SECURITY_ID_JUN = SecurityId.of("OG-Ticker", "GOVT-BOND-FUT-JUN");
	  private static readonly ResolvedBondFuture FUTURE_PRODUCT_JUN = BondFuture.builder().securityId(FUTURE_SECURITY_ID_JUN).deliveryBasket(UND_BOND_JUN).conversionFactors(CF_JUN).firstNoticeDate(FIRST_NOTICE_DATE_JUN).lastNoticeDate(LAST_NOTICE_DATE_JUN).firstDeliveryDate(FIRST_DELIVERY_DATE_JUN).lastDeliveryDate(LAST_DELIVERY_DATE_JUN).lastTradeDate(LAST_TRADING_DATE_JUN).rounding(Rounding.ofDecimalPlaces(2)).build().resolve(REF_DATA);
	  private static readonly ResolvedBondFutureTrade FUTURE_TRADE_JUN = ResolvedBondFutureTrade.builder().product(FUTURE_PRODUCT_JUN).info(TRADE_INFO).quantity(QUANTITY).tradedPrice(TradedPrice.of(TRADE_INFO.TradeDate.get(), 123)).build();
	  private const double REF_PRICE_JUN = 151.73;
	  // Futures in March 2016
	  private static readonly double?[] CF_MAR = new double?[] {0.695006, 0.697881, 0.689613, 0.677675, 0.669189, 0.660850, 0.652611, 0.637931, 0.629786, 0.616327, 0.614901, 0.606851, 0.598933, 0.583818};
	  private static readonly LocalDate EFFECTIVE_DATE_MAR = LocalDate.of(2016, 3, 20);
	  private static readonly LocalDate FIRST_DELIVERY_DATE_MAR = BUSINESS_ADJUST.adjust(EFFECTIVE_DATE_MAR, REF_DATA);
	  private static readonly LocalDate LAST_DELIVERY_DATE_MAR = BUSINESS_ADJUST.adjust(EFFECTIVE_DATE_MAR, REF_DATA);
	  private static readonly LocalDate LAST_TRADING_DATE_MAR = LAST_TRADE_ADJUST.adjust(LAST_DELIVERY_DATE_MAR, REF_DATA);
	  private static readonly LocalDate FIRST_NOTICE_DATE_MAR = LAST_TRADE_ADJUST.adjust(LAST_DELIVERY_DATE_MAR, REF_DATA);
	  private static readonly LocalDate LAST_NOTICE_DATE_MAR = LAST_TRADE_ADJUST.adjust(LAST_DELIVERY_DATE_MAR, REF_DATA);
	  private static readonly SecurityId FUTURE_SECURITY_ID_MAR = SecurityId.of("OG-Ticker", "GOVT-BOND-FUT-MAR");
	  private static readonly ResolvedBondFuture FUTURE_PRODUCT_MAR = BondFuture.builder().securityId(FUTURE_SECURITY_ID_MAR).deliveryBasket(UND_BOND).conversionFactors(CF_MAR).firstNoticeDate(FIRST_NOTICE_DATE_MAR).lastNoticeDate(LAST_NOTICE_DATE_MAR).firstDeliveryDate(FIRST_DELIVERY_DATE_MAR).lastDeliveryDate(LAST_DELIVERY_DATE_MAR).lastTradeDate(LAST_TRADING_DATE_MAR).rounding(Rounding.ofDecimalPlaces(2)).build().resolve(REF_DATA);
	  private static readonly ResolvedBondFutureTrade FUTURE_TRADE_MAR = ResolvedBondFutureTrade.builder().quantity(QUANTITY).product(FUTURE_PRODUCT_MAR).info(TRADE_INFO).build();
	  private const double REF_PRICE_MAR = 152.25;
	  // Curves
	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;
	  private static readonly CurveName ISSUER_CURVE_NAME = CurveName.of("issuerCurve");
	  private static readonly CurveName REPO_CURVE_NAME = CurveName.of("repoCurve");
	  private static readonly LegalEntityDiscountingProvider LED_PROVIDER;
	  // Pricers
	  private static readonly DiscountingBondFutureProductPricer PRODUCT_PRICER = DiscountingBondFutureProductPricer.DEFAULT;
	  private static readonly DiscountingBondFutureTradePricer TRADE_PRICER = DiscountingBondFutureTradePricer.DEFAULT;

	  public virtual void price()
	  {
		// March
		double priceMar = PRODUCT_PRICER.price(FUTURE_PRODUCT_MAR, LED_PROVIDER) * HUNDRED;
		double priceMarRounded = FUTURE_PRODUCT_MAR.Rounding.round(priceMar);
		assertEquals(priceMar, 151.83071796298776, TOL * HUNDRED);
		assertEquals(priceMarRounded, 151.83, TOL * HUNDRED);
		// June
		double priceJun = PRODUCT_PRICER.price(FUTURE_PRODUCT_JUN, LED_PROVIDER) * HUNDRED;
		double priceJunRounded = FUTURE_PRODUCT_JUN.Rounding.round(priceJun);
		assertEquals(priceJun, 151.25027452317593, TOL * HUNDRED);
		assertEquals(priceJunRounded, 151.25, TOL * HUNDRED);
		// September
		double priceSep = PRODUCT_PRICER.price(FUTURE_PRODUCT_SEP, LED_PROVIDER) * HUNDRED;
		double priceSepRounded = FUTURE_PRODUCT_SEP.Rounding.round(priceSep);
		assertEquals(priceSep, 151.08452213883535, TOL * HUNDRED);
		assertEquals(priceSepRounded, 151.08, TOL * HUNDRED);
	  }

	  public virtual void priceSensitivity()
	  {
		// March
		PointSensitivities pointMar = PRODUCT_PRICER.priceSensitivity(FUTURE_PRODUCT_MAR, LED_PROVIDER).multipliedBy(HUNDRED * ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiMar = LED_PROVIDER.parameterSensitivity(pointMar);
		double[] sensiIssuerMar = new double[] {-4.795692708445902E-6, -2.0781215861310126E-5, -7.730767169573405E-5, -1.6071777740512183E-4, -2.3044416935116369E-4, -3.333307694739688E-4, -4.263036155523118E-4, -5.685365085703306E-4, -0.10407934097674876, -0.0, -0.0, -0.0, -0.0, -0.0, -0.0, -0.0};
		double[] sensiRepoMar = new double[] {0.0, 0.0, 0.0, 0.0, 0.001370140084809201, 3.3554451056551886E-4, 0.0, 0.0};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiMar.getSensitivity(ISSUER_CURVE_NAME, JPY).Sensitivity.toArray(), sensiIssuerMar, TOL));
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiMar.getSensitivity(REPO_CURVE_NAME, JPY).Sensitivity.toArray(), sensiRepoMar, TOL));
		// June
		PointSensitivities pointJun = PRODUCT_PRICER.priceSensitivity(FUTURE_PRODUCT_JUN, LED_PROVIDER).multipliedBy(HUNDRED * ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiJun = LED_PROVIDER.parameterSensitivity(pointJun);
		double[] sensiIssuerJun = new double[] {-1.1453989553600325E-5, -2.348926498286566E-5, -1.0106640809190963E-4, -1.9509367993719023E-4, -3.132622179286758E-4, -4.395002117284386E-4, -5.572262990208806E-4, -7.858225833901946E-4, -0.07087170775675304, -0.03539736978075175, -0.0, -0.0, -0.0, -0.0, -0.0, -0.0};
		double[] sensiRepoJun = new double[] {0.0, 0.0, 0.0, 0.0, 0.0, 0.003012223890022257, 0.0024215917547237764, 0.0};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiJun.getSensitivity(ISSUER_CURVE_NAME, JPY).Sensitivity.toArray(), sensiIssuerJun, TOL));
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiJun.getSensitivity(REPO_CURVE_NAME, JPY).Sensitivity.toArray(), sensiRepoJun, TOL));
		// September
		PointSensitivities pointSep = PRODUCT_PRICER.priceSensitivity(FUTURE_PRODUCT_SEP, LED_PROVIDER).multipliedBy(HUNDRED * ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiSep = LED_PROVIDER.parameterSensitivity(pointSep);
		double[] sensiIssuerSep = new double[] {-6.287268294968501E-6, -2.7244672992830814E-5, -1.0135221390528455E-4, -2.1070486533414349E-4, -3.021178394458564E-4, -4.370046427203812E-4, -5.588942763935072E-4, -7.453650144370277E-4, -0.03687605192905092, -0.07313888023068209, -0.0, -0.0, -0.0, -0.0, -0.0, -0.0};
		double[] sensiRepoSep = new double[] {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.007209056180693214, 0.0020653493968426154};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiSep.getSensitivity(ISSUER_CURVE_NAME, JPY).Sensitivity.toArray(), sensiIssuerSep, TOL));
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiSep.getSensitivity(REPO_CURVE_NAME, JPY).Sensitivity.toArray(), sensiRepoSep, TOL));
	  }

	  public virtual void presentValue()
	  {
		// March
		CurrencyAmount pvMar = TRADE_PRICER.presentValue(FUTURE_TRADE_MAR, LED_PROVIDER, REF_PRICE_MAR * ONE_PERCENT);
		assertEquals(pvMar.Amount, -419282.03701224923, TOL * NOTIONAL);
		// June
		CurrencyAmount pvJun = TRADE_PRICER.presentValue(FUTURE_TRADE_JUN, LED_PROVIDER, REF_PRICE_JUN * ONE_PERCENT);
		assertEquals(pvJun.Amount, -479725.4768240452, TOL * NOTIONAL);
		// September
		CurrencyAmount pvSep = TRADE_PRICER.presentValue(FUTURE_TRADE_SEP, LED_PROVIDER, REF_PRICE_SEP * ONE_PERCENT);
		assertEquals(pvSep.Amount, -525477.8611646593, TOL * NOTIONAL);
	  }

	  public virtual void presentValueSensitivity()
	  {
		// March
		PointSensitivities pointMar = TRADE_PRICER.presentValueSensitivity(FUTURE_TRADE_MAR, LED_PROVIDER).multipliedBy(ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiMar = LED_PROVIDER.parameterSensitivity(pointMar);
		double[] sensiIssuerMar = new double[] {-4.795692708445902, -20.78121586131013, -77.30767169573404, -160.71777740512184, -230.44416935116368, -333.3307694739688, -426.3036155523117, -568.5365085703306, -104079.34097674876, -0.0, -0.0, -0.0, -0.0, -0.0, -0.0, -0.0};
		double[] sensiRepoMar = new double[] {0.0, 0.0, 0.0, 0.0, 1370.1400848092012, 335.54451056551886, 0.0, 0.0};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiMar.getSensitivity(ISSUER_CURVE_NAME, JPY).Sensitivity.toArray(), sensiIssuerMar, TOL));
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiMar.getSensitivity(REPO_CURVE_NAME, JPY).Sensitivity.toArray(), sensiRepoMar, TOL));
		// June
		PointSensitivities pointJun = TRADE_PRICER.presentValueSensitivity(FUTURE_TRADE_JUN, LED_PROVIDER).multipliedBy(ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiJun = LED_PROVIDER.parameterSensitivity(pointJun);
		double[] sensiIssuerJun = new double[] {-11.453989553600326, -23.489264982865656, -101.06640809190962, -195.09367993719025, -313.2622179286758, -439.5002117284386, -557.2262990208807, -785.8225833901945, -70871.70775675304, -35397.369780751746, -0.0, -0.0, -0.0, -0.0, -0.0, -0.0};
		double[] sensiRepoJun = new double[] {0.0, 0.0, 0.0, 0.0, 0.0, 3012.223890022257, 2421.5917547237764, 0.0};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiJun.getSensitivity(ISSUER_CURVE_NAME, JPY).Sensitivity.toArray(), sensiIssuerJun, TOL));
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiJun.getSensitivity(REPO_CURVE_NAME, JPY).Sensitivity.toArray(), sensiRepoJun, TOL));
		// September
		PointSensitivities pointSep = TRADE_PRICER.presentValueSensitivity(FUTURE_TRADE_SEP, LED_PROVIDER).multipliedBy(ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiSep = LED_PROVIDER.parameterSensitivity(pointSep);
		double[] sensiIssuerSep = new double[] {-6.287268294968501, -27.244672992830814, -101.35221390528456, -210.7048653341435, -302.1178394458564, -437.0046427203812, -558.8942763935072, -745.3650144370276, -36876.05192905092, -73138.88023068209, -0.0, -0.0, -0.0, -0.0, -0.0, -0.0};
		double[] sensiRepoSep = new double[] {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 7209.056180693215, 2065.3493968426155};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiSep.getSensitivity(ISSUER_CURVE_NAME, JPY).Sensitivity.toArray(), sensiIssuerSep, TOL));
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiSep.getSensitivity(REPO_CURVE_NAME, JPY).Sensitivity.toArray(), sensiRepoSep, TOL));
	  }

	  public virtual void parSpread()
	  {
		// March
		double psMar = TRADE_PRICER.parSpread(FUTURE_TRADE_MAR, LED_PROVIDER, REF_PRICE_MAR * ONE_PERCENT) * HUNDRED;
		assertEquals(psMar, -0.4192820370122474, TOL * HUNDRED);
		// June
		double psJun = TRADE_PRICER.parSpread(FUTURE_TRADE_JUN, LED_PROVIDER, REF_PRICE_JUN * ONE_PERCENT) * HUNDRED;
		assertEquals(psJun, -0.47972547682404443, TOL * HUNDRED);
		// September
		double psSep = TRADE_PRICER.parSpread(FUTURE_TRADE_SEP, LED_PROVIDER, REF_PRICE_SEP * ONE_PERCENT) * HUNDRED;
		assertEquals(psSep, -0.5254778611646582, TOL * HUNDRED);
	  }

	  public virtual void parSpreadSensitivity()
	  {
		// March
		PointSensitivities pointMar = TRADE_PRICER.parSpreadSensitivity(FUTURE_TRADE_MAR, LED_PROVIDER).multipliedBy(HUNDRED * ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiMar = LED_PROVIDER.parameterSensitivity(pointMar);
		double[] sensiIssuerMar = new double[] {-4.795692708445902E-6, -2.0781215861310126E-5, -7.730767169573405E-5, -1.6071777740512183E-4, -2.3044416935116369E-4, -3.333307694739688E-4, -4.263036155523118E-4, -5.685365085703306E-4, -0.10407934097674876, -0.0, -0.0, -0.0, -0.0, -0.0, -0.0, -0.0};
		double[] sensiRepoMar = new double[] {0.0, 0.0, 0.0, 0.0, 0.001370140084809201, 3.3554451056551886E-4, 0.0, 0.0};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiMar.getSensitivity(ISSUER_CURVE_NAME, JPY).Sensitivity.toArray(), sensiIssuerMar, TOL));
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiMar.getSensitivity(REPO_CURVE_NAME, JPY).Sensitivity.toArray(), sensiRepoMar, TOL));
		// June
		PointSensitivities pointJun = TRADE_PRICER.parSpreadSensitivity(FUTURE_TRADE_JUN, LED_PROVIDER).multipliedBy(HUNDRED * ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiJun = LED_PROVIDER.parameterSensitivity(pointJun);
		double[] sensiIssuerJun = new double[] {-1.1453989553600325E-5, -2.348926498286566E-5, -1.0106640809190963E-4, -1.9509367993719023E-4, -3.132622179286758E-4, -4.395002117284386E-4, -5.572262990208806E-4, -7.858225833901946E-4, -0.07087170775675304, -0.03539736978075175, -0.0, -0.0, -0.0, -0.0, -0.0, -0.0};
		double[] sensiRepoJun = new double[] {0.0, 0.0, 0.0, 0.0, 0.0, 0.003012223890022257, 0.0024215917547237764, 0.0};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiJun.getSensitivity(ISSUER_CURVE_NAME, JPY).Sensitivity.toArray(), sensiIssuerJun, TOL));
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiJun.getSensitivity(REPO_CURVE_NAME, JPY).Sensitivity.toArray(), sensiRepoJun, TOL));
		// September
		PointSensitivities pointSep = TRADE_PRICER.parSpreadSensitivity(FUTURE_TRADE_SEP, LED_PROVIDER).multipliedBy(HUNDRED * ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiSep = LED_PROVIDER.parameterSensitivity(pointSep);
		double[] sensiIssuerSep = new double[] {-6.287268294968501E-6, -2.7244672992830814E-5, -1.0135221390528455E-4, -2.1070486533414349E-4, -3.021178394458564E-4, -4.370046427203812E-4, -5.588942763935072E-4, -7.453650144370277E-4, -0.03687605192905092, -0.07313888023068209, -0.0, -0.0, -0.0, -0.0, -0.0, -0.0};
		double[] sensiRepoSep = new double[] {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.007209056180693214, 0.0020653493968426154};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiSep.getSensitivity(ISSUER_CURVE_NAME, JPY).Sensitivity.toArray(), sensiIssuerSep, TOL));
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiSep.getSensitivity(REPO_CURVE_NAME, JPY).Sensitivity.toArray(), sensiRepoSep, TOL));
	  }

	}

}