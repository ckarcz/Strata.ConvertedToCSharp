/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.index.e2e
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.JPY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxMatrix = com.opengamma.strata.basics.currency.FxMatrix;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using Rounding = com.opengamma.strata.basics.value.Rounding;
	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using TradedPrice = com.opengamma.strata.product.TradedPrice;
	using IborFuture = com.opengamma.strata.product.index.IborFuture;
	using ResolvedIborFuture = com.opengamma.strata.product.index.ResolvedIborFuture;
	using ResolvedIborFutureTrade = com.opengamma.strata.product.index.ResolvedIborFutureTrade;

	/// <summary>
	/// End to end test on JPY-dominated trades.
	/// <para>
	/// The trades involve futures contract on 3m Euroyen TIBOR.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborFuturesJpyEnd2EndTest
	public class IborFuturesJpyEnd2EndTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double ONE_PERCENT = 1e-2;
	  private const double ONE_BASIS_POINT = 1e-4;
	  private const double HUNDRED = 100d;
	  private const double TOL = 1e-10;
	  private static readonly LocalDate TRADE_DATE = LocalDate.of(2016, 2, 10);
	  private static readonly LocalDate VALUATION = TRADE_DATE;
	  private const double NOTIONAL = 100_000_000D;
	  private const long QUANTITY = 1L;
	  private static readonly Rounding ROUNDING = Rounding.ofFractionalDecimalPlaces(2, 2);
	  private static readonly IborIndex TIBOR_EUROYEN_3M = IborIndices.JPY_TIBOR_EUROYEN_3M;
	  private static readonly HolidayCalendarId CALENDAR = TIBOR_EUROYEN_3M.FixingCalendar;
	  // curve
	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;
	  private static readonly double[] TIMES_FWD = new double[] {0.25956284153005466, 0.3442622950819672, 0.4262295081967213, 0.5109289617486339, 0.5956284153005464, 0.6775956284153005, 0.7622950819672131, 0.8442622950819673, 0.9290665468972228, 1.0139980537465378, 1.509888464705442, 2.013998053746538, 3.013998053746538, 4.0136612021857925, 5.013998053746538, 6.013998053746538, 7.013998053746538, 8.013661202185792, 9.013998053746537, 10.013998053746537, 12.013661202185792, 15.013998053746537, 20.01366120218579, 25.013998053746537, 30.013998053746537, 35.01399805374654, 40.013661202185794};
	  private static readonly double[] RATES_FWD = new double[] {0.0011675730858146669, 0.0013523368085561177, 0.001131049534280983, 8.583742384839034E-4, 8.470484635395976E-4, 6.767689351179286E-4, 5.413609808841509E-4, 4.4761361391297197E-4, 3.289892481714955E-4, 2.2424609900293199E-4, -5.55328245806963E-4, -5.582260143032516E-4, -0.0013213322970379335, -9.99374212934137E-4, -6.786337899984415E-4, -1.0429800511592125E-4, 4.58740553910201E-4, 0.0010754297421556789, 0.0017620501351558286, 0.0024797272826230436, 0.003796406741560559, 0.005230486692524101, 0.009225065993903405, 0.01128357115778175, 0.012172498030710542, 0.012953839426947904, 0.013546707965288615};
	  private static readonly CurveName NAME_FWD = CurveName.of("fwdCurve");
	  private static readonly CurveMetadata META_FWD = Curves.zeroRates(NAME_FWD, ACT_ACT_ISDA);
	  private static readonly InterpolatedNodalCurve CURVE_FWD = InterpolatedNodalCurve.of(META_FWD, DoubleArray.copyOf(TIMES_FWD), DoubleArray.copyOf(RATES_FWD), INTERPOLATOR);
	  private static readonly ImmutableRatesProvider RATES_PROVIDER = ImmutableRatesProvider.builder(VALUATION).fxRateProvider(FxMatrix.empty()).iborIndexCurve(TIBOR_EUROYEN_3M, CURVE_FWD).build();
	  // futures in March 2016
	  private static readonly LocalDate REFERENCE_MAR = RollConventions.IMM.adjust(LocalDate.of(2016, 3, 1));
	  private static readonly LocalDate LAST_TRADE_MAR = DaysAdjustment.ofBusinessDays(-2, CALENDAR).adjust(REFERENCE_MAR, REF_DATA);
	  private static readonly SecurityId FUTURE_SECURITY_ID_MAR = SecurityId.of("OG-Ticker", "EUROYEN3M-FUT-MAR");
	  private static readonly ResolvedIborFuture FUTURE_PRODUCT_MAR = IborFuture.builder().securityId(FUTURE_SECURITY_ID_MAR).currency(JPY).notional(NOTIONAL).lastTradeDate(LAST_TRADE_MAR).index(TIBOR_EUROYEN_3M).rounding(ROUNDING).build().resolve(REF_DATA);
	  private const double REF_PRICE_MAR = 99.9d;
	  private static readonly double REF_PRICE_MAR_DECIMAL = REF_PRICE_MAR * ONE_PERCENT;
	  private static readonly ResolvedIborFutureTrade FUTURE_TRADE_MAR = ResolvedIborFutureTrade.builder().product(FUTURE_PRODUCT_MAR).quantity(QUANTITY).tradedPrice(TradedPrice.of(TRADE_DATE, REF_PRICE_MAR_DECIMAL)).build();
	  // futures in June 2016
	  private static readonly LocalDate REFERENCE_JUN = RollConventions.IMM.adjust(LocalDate.of(2016, 6, 1));
	  private static readonly LocalDate LAST_TRADE_JUN = DaysAdjustment.ofBusinessDays(-2, CALENDAR).adjust(REFERENCE_JUN, REF_DATA);
	  private static readonly SecurityId FUTURE_SECURITY_ID_JUN = SecurityId.of("OG-Ticker", "EUROYEN3M-FUT-JUN");
	  private static readonly ResolvedIborFuture FUTURE_PRODUCT_JUN = IborFuture.builder().securityId(FUTURE_SECURITY_ID_JUN).currency(JPY).notional(NOTIONAL).lastTradeDate(LAST_TRADE_JUN).index(TIBOR_EUROYEN_3M).rounding(ROUNDING).build().resolve(REF_DATA);
	  private const double REF_PRICE_JUN = 100d;
	  private static readonly double REF_PRICE_JUN_DECIMAL = REF_PRICE_JUN * ONE_PERCENT;
	  private static readonly ResolvedIborFutureTrade FUTURE_TRADE_JUN = ResolvedIborFutureTrade.builder().product(FUTURE_PRODUCT_JUN).quantity(QUANTITY).tradedPrice(TradedPrice.of(TRADE_DATE, REF_PRICE_JUN_DECIMAL)).build();
	  // futures in September 2016
	  private static readonly LocalDate REFERENCE_SEP = RollConventions.IMM.adjust(LocalDate.of(2016, 9, 1));
	  private static readonly LocalDate LAST_TRADE_SEP = DaysAdjustment.ofBusinessDays(-2, CALENDAR).adjust(REFERENCE_SEP, REF_DATA);
	  private static readonly SecurityId FUTURE_SECURITY_ID_SEP = SecurityId.of("OG-Ticker", "EUROYEN3M-FUT-SEP");
	  private static readonly ResolvedIborFuture FUTURE_PRODUCT_SEP = IborFuture.builder().securityId(FUTURE_SECURITY_ID_SEP).currency(JPY).notional(NOTIONAL).lastTradeDate(LAST_TRADE_SEP).index(TIBOR_EUROYEN_3M).rounding(ROUNDING).build().resolve(REF_DATA);
	  private const double REF_PRICE_SEP = 100.075d;
	  private static readonly double REF_PRICE_SEP_DECIMAL = REF_PRICE_SEP * ONE_PERCENT;
	  private static readonly ResolvedIborFutureTrade FUTURE_TRADE_SEP = ResolvedIborFutureTrade.builder().product(FUTURE_PRODUCT_SEP).quantity(QUANTITY).tradedPrice(TradedPrice.of(TRADE_DATE, REF_PRICE_SEP_DECIMAL)).build();
	  // futures in June 2017
	  private static readonly LocalDate REFERENCE_JUN_MID = RollConventions.IMM.adjust(LocalDate.of(2017, 6, 1));
	  private static readonly LocalDate LAST_TRADE_JUN_MID = DaysAdjustment.ofBusinessDays(-2, CALENDAR).adjust(REFERENCE_JUN_MID, REF_DATA);
	  private static readonly SecurityId FUTURE_SECURITY_ID_JUN_MID = SecurityId.of("OG-Ticker", "EUROYEN3M-FUT-JUN_MID");
	  private static readonly ResolvedIborFuture FUTURE_PRODUCT_JUN_MID = IborFuture.builder().securityId(FUTURE_SECURITY_ID_JUN_MID).currency(JPY).notional(NOTIONAL).lastTradeDate(LAST_TRADE_JUN_MID).index(TIBOR_EUROYEN_3M).rounding(ROUNDING).build().resolve(REF_DATA);
	  private const double REF_PRICE_JUN_MID = 100.165d;
	  private static readonly double REF_PRICE_JUN_MID_DECIMAL = REF_PRICE_JUN_MID * ONE_PERCENT;
	  private static readonly ResolvedIborFutureTrade FUTURE_TRADE_JUN_MID = ResolvedIborFutureTrade.builder().product(FUTURE_PRODUCT_JUN_MID).quantity(QUANTITY).tradedPrice(TradedPrice.of(TRADE_DATE, REF_PRICE_JUN_MID_DECIMAL)).build();
	  // futures in March 2020
	  private static readonly LocalDate REFERENCE_MAR_LONG = RollConventions.IMM.adjust(LocalDate.of(2020, 3, 1));
	  private static readonly LocalDate LAST_TRADE_MAR_LONG = DaysAdjustment.ofBusinessDays(-2, CALENDAR).adjust(REFERENCE_MAR_LONG, REF_DATA);
	  private static readonly SecurityId FUTURE_SECURITY_ID_MAR_LONG = SecurityId.of("OG-Ticker", "EUROYEN3M-FUT-MAR_LONG");
	  private static readonly ResolvedIborFuture FUTURE_PRODUCT_MAR_LONG = IborFuture.builder().securityId(FUTURE_SECURITY_ID_MAR_LONG).currency(JPY).notional(NOTIONAL).lastTradeDate(LAST_TRADE_MAR_LONG).index(TIBOR_EUROYEN_3M).rounding(ROUNDING).build().resolve(REF_DATA);
	  private const double REF_PRICE_MAR_LONG = 99.815d;
	  private static readonly double REF_PRICE_MAR_LONG_DECIMAL = REF_PRICE_MAR_LONG * ONE_PERCENT;
	  private static readonly ResolvedIborFutureTrade FUTURE_TRADE_MAR_LONG = ResolvedIborFutureTrade.builder().product(FUTURE_PRODUCT_MAR_LONG).quantity(QUANTITY).tradedPrice(TradedPrice.of(TRADE_DATE, REF_PRICE_MAR_LONG_DECIMAL)).build();
	  // pricers
	  private static readonly DiscountingIborFutureProductPricer PRODUCT_PRICER = DiscountingIborFutureProductPricer.DEFAULT;
	  private static readonly DiscountingIborFutureTradePricer TRADE_PRICER = DiscountingIborFutureTradePricer.DEFAULT;

	  public virtual void price()
	  {
		// March 2016
		double priceMar = PRODUCT_PRICER.price(FUTURE_PRODUCT_MAR, RATES_PROVIDER) * HUNDRED;
		double priceMarRounded = FUTURE_PRODUCT_MAR.Rounding.round(priceMar);
		assertEquals(priceMar, 99.86104632448779, TOL * HUNDRED);
		assertEquals(priceMarRounded, 99.86, TOL * HUNDRED);
		// June 2016
		double priceJun = PRODUCT_PRICER.price(FUTURE_PRODUCT_JUN, RATES_PROVIDER) * HUNDRED;
		double priceJunRounded = FUTURE_PRODUCT_JUN.Rounding.round(priceJun);
		assertEquals(priceJun, 99.98475152702353, TOL * HUNDRED);
		assertEquals(priceJunRounded, 99.985, TOL * HUNDRED);
		// September 2016
		double priceSep = PRODUCT_PRICER.price(FUTURE_PRODUCT_SEP, RATES_PROVIDER) * HUNDRED;
		double priceSepRounded = FUTURE_PRODUCT_SEP.Rounding.round(priceSep);
		assertEquals(priceSep, 100.05224158750461, TOL * HUNDRED);
		assertEquals(priceSepRounded, 100.05, TOL * HUNDRED);
		// June 2017
		double priceJunMid = PRODUCT_PRICER.price(FUTURE_PRODUCT_JUN_MID, RATES_PROVIDER) * HUNDRED;
		double priceJunMidRounded = FUTURE_PRODUCT_JUN_MID.Rounding.round(priceJunMid);
		assertEquals(priceJunMid, 100.18108895230915, TOL * HUNDRED);
		assertEquals(priceJunMidRounded, 100.18, TOL * HUNDRED);
		// March 2020
		double priceMarLong = PRODUCT_PRICER.price(FUTURE_PRODUCT_MAR_LONG, RATES_PROVIDER) * HUNDRED;
		double priceMarLongRounded = FUTURE_PRODUCT_MAR_LONG.Rounding.round(priceMarLong);
		assertEquals(priceMarLong, 99.9582733152131, TOL * HUNDRED);
		assertEquals(priceMarLongRounded, 99.96, TOL * HUNDRED);
	  }

	  public virtual void priceSensitivity()
	  {
		// March 2016
		PointSensitivities pointMar = PRODUCT_PRICER.priceSensitivity(FUTURE_PRODUCT_MAR, RATES_PROVIDER).multipliedBy(HUNDRED * ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiMar = RATES_PROVIDER.parameterSensitivity(pointMar);
		double[] sensiFwdMar = new double[] {0.003743310260261194, -0.01313010637003998, -4.527622886220682E-4, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiMar.getSensitivity(NAME_FWD, JPY).Sensitivity.toArray(), sensiFwdMar, TOL));
		// June 2016
		PointSensitivities pointJun = PRODUCT_PRICER.priceSensitivity(FUTURE_PRODUCT_JUN, RATES_PROVIDER).multipliedBy(HUNDRED * ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiJun = RATES_PROVIDER.parameterSensitivity(pointJun);
		double[] sensiFwdJun = new double[] {0.0, 0.01347165823324645, 0.0, 0.0, -0.023308107101966076, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiJun.getSensitivity(NAME_FWD, JPY).Sensitivity.toArray(), sensiFwdJun, TOL));
		// September 2016
		PointSensitivities pointSep = PRODUCT_PRICER.priceSensitivity(FUTURE_PRODUCT_SEP, RATES_PROVIDER).multipliedBy(HUNDRED * ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiSep = RATES_PROVIDER.parameterSensitivity(pointSep);
		double[] sensiFwdSep = new double[] {0.0, 0.0, 0.0, 0.0, 0.01936692513656471, 0.0048417312841411864, 0.0, -0.027462515988551, -0.006580907103066675, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiSep.getSensitivity(NAME_FWD, JPY).Sensitivity.toArray(), sensiFwdSep, TOL));
		// June 2017
		PointSensitivities pointJunMid = PRODUCT_PRICER.priceSensitivity(FUTURE_PRODUCT_JUN_MID, RATES_PROVIDER).multipliedBy(HUNDRED * ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiJunMid = RATES_PROVIDER.parameterSensitivity(pointJunMid);
		double[] sensiFwdJunMid = new double[] {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.016154080854008976, -0.013340017892182532, -0.012672512226590141, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiJunMid.getSensitivity(NAME_FWD, JPY).Sensitivity.toArray(), sensiFwdJunMid, TOL));
		// March 2020
		PointSensitivities pointMarLong = PRODUCT_PRICER.priceSensitivity(FUTURE_PRODUCT_MAR_LONG, RATES_PROVIDER).multipliedBy(HUNDRED * ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiMarLong = RATES_PROVIDER.parameterSensitivity(pointMarLong);
		double[] sensiFwdMarLong = new double[] {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.03382389130551987, -0.043661005746776824, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiMarLong.getSensitivity(NAME_FWD, JPY).Sensitivity.toArray(), sensiFwdMarLong, TOL));
	  }

	  public virtual void presentValue()
	  {
		// March 2016
		CurrencyAmount pvMar = TRADE_PRICER.presentValue(FUTURE_TRADE_MAR, RATES_PROVIDER, REF_PRICE_MAR_DECIMAL);
		assertEquals(pvMar.Amount, -9738.418878056109, TOL * NOTIONAL);
		// June 2016
		CurrencyAmount pvJun = TRADE_PRICER.presentValue(FUTURE_TRADE_JUN, RATES_PROVIDER, REF_PRICE_JUN_DECIMAL);
		assertEquals(pvJun.Amount, -3812.1182441189885, TOL * NOTIONAL);
		// September 2016
		CurrencyAmount pvSep = TRADE_PRICER.presentValue(FUTURE_TRADE_SEP, RATES_PROVIDER, REF_PRICE_SEP_DECIMAL);
		assertEquals(pvSep.Amount, -5689.603123847395, TOL * NOTIONAL);
		// June 2017
		CurrencyAmount pvJunMid = TRADE_PRICER.presentValue(FUTURE_TRADE_JUN_MID, RATES_PROVIDER, REF_PRICE_JUN_MID_DECIMAL);
		assertEquals(pvJunMid.Amount, 4022.2380772829056, TOL * NOTIONAL);
		// March 2020
		CurrencyAmount pvMarLong = TRADE_PRICER.presentValue(FUTURE_TRADE_MAR_LONG, RATES_PROVIDER, REF_PRICE_MAR_LONG_DECIMAL);
		assertEquals(pvMarLong.Amount, 35818.328803278506, TOL * NOTIONAL);
	  }

	  public virtual void presentValueSensitivity()
	  {
		// March 2016
		PointSensitivities pointMar = TRADE_PRICER.presentValueSensitivity(FUTURE_TRADE_MAR, RATES_PROVIDER).multipliedBy(ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiMar = RATES_PROVIDER.parameterSensitivity(pointMar);
		double[] sensiFwdMar = new double[] {935.8275650652985, -3282.5265925099943, -113.19057215551703, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiMar.getSensitivity(NAME_FWD, JPY).Sensitivity.toArray(), sensiFwdMar, TOL));
		// June 2016
		PointSensitivities pointJun = TRADE_PRICER.presentValueSensitivity(FUTURE_TRADE_JUN, RATES_PROVIDER).multipliedBy(ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiJun = RATES_PROVIDER.parameterSensitivity(pointJun);
		double[] sensiFwdJun = new double[] {0.0, 3367.914558311612, 0.0, 0.0, -5827.0267754915185, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiJun.getSensitivity(NAME_FWD, JPY).Sensitivity.toArray(), sensiFwdJun, TOL));
		// September 2016
		PointSensitivities pointSep = TRADE_PRICER.presentValueSensitivity(FUTURE_TRADE_SEP, RATES_PROVIDER).multipliedBy(ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiSep = RATES_PROVIDER.parameterSensitivity(pointSep);
		double[] sensiFwdSep = new double[] {0.0, 0.0, 0.0, 0.0, 4841.731284141179, 1210.432821035297, 0.0, -6865.62899713775, -1645.2267757666687, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiSep.getSensitivity(NAME_FWD, JPY).Sensitivity.toArray(), sensiFwdSep, TOL));
		// June 2017
		PointSensitivities pointJunMid = TRADE_PRICER.presentValueSensitivity(FUTURE_TRADE_JUN_MID, RATES_PROVIDER).multipliedBy(ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiJunMid = RATES_PROVIDER.parameterSensitivity(pointJunMid);
		double[] sensiFwdJunMid = new double[] {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 4038.520213502244, -3335.0044730456357, -3168.128056647536, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiJunMid.getSensitivity(NAME_FWD, JPY).Sensitivity.toArray(), sensiFwdJunMid, TOL));
		// March 2020
		PointSensitivities pointMarLong = TRADE_PRICER.presentValueSensitivity(FUTURE_TRADE_MAR_LONG, RATES_PROVIDER).multipliedBy(ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiMarLong = RATES_PROVIDER.parameterSensitivity(pointMarLong);
		double[] sensiFwdMarLong = new double[] {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 8455.972826379962, -10915.251436694207, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiMarLong.getSensitivity(NAME_FWD, JPY).Sensitivity.toArray(), sensiFwdMarLong, TOL));
	  }

	  public virtual void parSpread()
	  {
		// March 2016
		double psMar = TRADE_PRICER.parSpread(FUTURE_TRADE_MAR, RATES_PROVIDER, REF_PRICE_MAR_DECIMAL) * HUNDRED;
		assertEquals(psMar, -0.038953675512221064, TOL * HUNDRED);
		// June 2016
		double psJun = TRADE_PRICER.parSpread(FUTURE_TRADE_JUN, RATES_PROVIDER, REF_PRICE_JUN_DECIMAL) * HUNDRED;
		assertEquals(psJun, -0.01524847297647014, TOL * HUNDRED);
		// September 2016
		double psSep = TRADE_PRICER.parSpread(FUTURE_TRADE_SEP, RATES_PROVIDER, REF_PRICE_SEP_DECIMAL) * HUNDRED;
		assertEquals(psSep, -0.022758412495393898, TOL * HUNDRED);
		// June 2017
		double psJunMid = TRADE_PRICER.parSpread(FUTURE_TRADE_JUN_MID, RATES_PROVIDER, REF_PRICE_JUN_MID_DECIMAL) * HUNDRED;
		assertEquals(psJunMid, 0.01608895230913454, TOL * HUNDRED);
		// March 2020
		double psMarLong = TRADE_PRICER.parSpread(FUTURE_TRADE_MAR_LONG, RATES_PROVIDER, REF_PRICE_MAR_LONG_DECIMAL) * HUNDRED;
		assertEquals(psMarLong, 0.14327331521311049, TOL * HUNDRED);
	  }

	  public virtual void parSpreadSensitivity()
	  {
		// March 2016
		PointSensitivities pointMar = TRADE_PRICER.parSpreadSensitivity(FUTURE_TRADE_MAR, RATES_PROVIDER).multipliedBy(HUNDRED * ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiMar = RATES_PROVIDER.parameterSensitivity(pointMar);
		double[] sensiFwdMar = new double[] {0.003743310260261194, -0.01313010637003998, -4.527622886220682E-4, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiMar.getSensitivity(NAME_FWD, JPY).Sensitivity.toArray(), sensiFwdMar, TOL));
		// June 2016
		PointSensitivities pointJun = TRADE_PRICER.parSpreadSensitivity(FUTURE_TRADE_JUN, RATES_PROVIDER).multipliedBy(HUNDRED * ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiJun = RATES_PROVIDER.parameterSensitivity(pointJun);
		double[] sensiFwdJun = new double[] {0.0, 0.01347165823324645, 0.0, 0.0, -0.023308107101966076, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiJun.getSensitivity(NAME_FWD, JPY).Sensitivity.toArray(), sensiFwdJun, TOL));
		// September 2016
		PointSensitivities pointSep = TRADE_PRICER.parSpreadSensitivity(FUTURE_TRADE_SEP, RATES_PROVIDER).multipliedBy(HUNDRED * ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiSep = RATES_PROVIDER.parameterSensitivity(pointSep);
		double[] sensiFwdSep = new double[] {0.0, 0.0, 0.0, 0.0, 0.01936692513656471, 0.0048417312841411864, 0.0, -0.027462515988551, -0.006580907103066675, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiSep.getSensitivity(NAME_FWD, JPY).Sensitivity.toArray(), sensiFwdSep, TOL));
		// June 2017
		PointSensitivities pointJunMid = TRADE_PRICER.parSpreadSensitivity(FUTURE_TRADE_JUN_MID, RATES_PROVIDER).multipliedBy(HUNDRED * ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiJunMid = RATES_PROVIDER.parameterSensitivity(pointJunMid);
		double[] sensiFwdJunMid = new double[] {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.016154080854008976, -0.013340017892182532, -0.012672512226590141, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiJunMid.getSensitivity(NAME_FWD, JPY).Sensitivity.toArray(), sensiFwdJunMid, TOL));
		// March 2020
		PointSensitivities pointMarLong = TRADE_PRICER.parSpreadSensitivity(FUTURE_TRADE_MAR_LONG, RATES_PROVIDER).multipliedBy(HUNDRED * ONE_BASIS_POINT);
		CurrencyParameterSensitivities sensiMarLong = RATES_PROVIDER.parameterSensitivity(pointMarLong);
		double[] sensiFwdMarLong = new double[] {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.03382389130551987, -0.043661005746776824, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
		assertTrue(DoubleArrayMath.fuzzyEquals(sensiMarLong.getSensitivity(NAME_FWD, JPY).Sensitivity.toArray(), sensiFwdMarLong, TOL));
	  }

	}

}