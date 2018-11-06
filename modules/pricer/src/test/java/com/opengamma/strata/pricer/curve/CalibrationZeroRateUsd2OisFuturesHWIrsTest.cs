using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedOvernightSwapConventions.USD_FIXED_1Y_FED_FUND_OIS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DateSequences = com.opengamma.strata.basics.date.DateSequences;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using Index = com.opengamma.strata.basics.index.Index;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using ImmutableMarketDataBuilder = com.opengamma.strata.data.ImmutableMarketDataBuilder;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using ValueType = com.opengamma.strata.market.ValueType;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurveNode = com.opengamma.strata.market.curve.CurveNode;
	using DefaultCurveMetadata = com.opengamma.strata.market.curve.DefaultCurveMetadata;
	using InterpolatedNodalCurveDefinition = com.opengamma.strata.market.curve.InterpolatedNodalCurveDefinition;
	using CurveExtrapolator = com.opengamma.strata.market.curve.interpolator.CurveExtrapolator;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using FixedIborSwapCurveNode = com.opengamma.strata.market.curve.node.FixedIborSwapCurveNode;
	using FixedOvernightSwapCurveNode = com.opengamma.strata.market.curve.node.FixedOvernightSwapCurveNode;
	using IborFixingDepositCurveNode = com.opengamma.strata.market.curve.node.IborFixingDepositCurveNode;
	using IborFutureCurveNode = com.opengamma.strata.market.curve.node.IborFutureCurveNode;
	using TermDepositCurveNode = com.opengamma.strata.market.curve.node.TermDepositCurveNode;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using DiscountingIborFixingDepositProductPricer = com.opengamma.strata.pricer.deposit.DiscountingIborFixingDepositProductPricer;
	using DiscountingTermDepositProductPricer = com.opengamma.strata.pricer.deposit.DiscountingTermDepositProductPricer;
	using HullWhiteIborFutureTradePricer = com.opengamma.strata.pricer.index.HullWhiteIborFutureTradePricer;
	using HullWhiteOneFactorPiecewiseConstantParameters = com.opengamma.strata.pricer.model.HullWhiteOneFactorPiecewiseConstantParameters;
	using HullWhiteOneFactorPiecewiseConstantParametersProvider = com.opengamma.strata.pricer.model.HullWhiteOneFactorPiecewiseConstantParametersProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using ResolvedIborFixingDepositTrade = com.opengamma.strata.product.deposit.ResolvedIborFixingDepositTrade;
	using ResolvedTermDepositTrade = com.opengamma.strata.product.deposit.ResolvedTermDepositTrade;
	using IborFixingDepositTemplate = com.opengamma.strata.product.deposit.type.IborFixingDepositTemplate;
	using ImmutableTermDepositConvention = com.opengamma.strata.product.deposit.type.ImmutableTermDepositConvention;
	using TermDepositConvention = com.opengamma.strata.product.deposit.type.TermDepositConvention;
	using TermDepositTemplate = com.opengamma.strata.product.deposit.type.TermDepositTemplate;
	using ResolvedIborFutureTrade = com.opengamma.strata.product.index.ResolvedIborFutureTrade;
	using IborFutureConvention = com.opengamma.strata.product.index.type.IborFutureConvention;
	using IborFutureTemplate = com.opengamma.strata.product.index.type.IborFutureTemplate;
	using ImmutableIborFutureConvention = com.opengamma.strata.product.index.type.ImmutableIborFutureConvention;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;
	using FixedIborSwapConventions = com.opengamma.strata.product.swap.type.FixedIborSwapConventions;
	using FixedIborSwapTemplate = com.opengamma.strata.product.swap.type.FixedIborSwapTemplate;
	using FixedOvernightSwapTemplate = com.opengamma.strata.product.swap.type.FixedOvernightSwapTemplate;

	/// <summary>
	/// Test for curve calibration with 2 curves in USD.
	/// One curve is Discounting and Fed Fund forward and the other one is Libor 3M forward.
	/// The Forward 3M curve is calibrated in part to Ibor futures with convexity adjustment computed with Hull-White one factor model.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CalibrationZeroRateUsd2OisFuturesHWIrsTest
	public class CalibrationZeroRateUsd2OisFuturesHWIrsTest
	{

	  private static readonly LocalDate VAL_DATE = LocalDate.of(2015, 7, 21);

	  private static readonly CurveInterpolator INTERPOLATOR_LINEAR = CurveInterpolators.LINEAR;
	  private static readonly CurveExtrapolator EXTRAPOLATOR_FLAT = CurveExtrapolators.FLAT;
	  private static readonly DayCount CURVE_DC = ACT_365F;

	  // reference data
	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private const string SCHEME = "CALIBRATION";

	  /// <summary>
	  /// Curve names </summary>
	  private const string DSCON_NAME = "USD-DSCON-OIS";
	  private static readonly CurveName DSCON_CURVE_NAME = CurveName.of(DSCON_NAME);
	  private const string FWD3_NAME = "USD-LIBOR3M-FUTIRS";
	  private static readonly CurveName FWD3_CURVE_NAME = CurveName.of(FWD3_NAME);
	  /// <summary>
	  /// Curves associations to currencies and indices. </summary>
	  private static readonly IDictionary<CurveName, Currency> DSC_NAMES = new Dictionary<CurveName, Currency>();
	  private static readonly IDictionary<CurveName, ISet<Index>> IDX_NAMES = new Dictionary<CurveName, ISet<Index>>();
	  static CalibrationZeroRateUsd2OisFuturesHWIrsTest()
	  {
		DSC_NAMES[DSCON_CURVE_NAME] = USD;
		ISet<Index> usdFedFundSet = new HashSet<Index>();
		usdFedFundSet.Add(USD_FED_FUND);
		IDX_NAMES[DSCON_CURVE_NAME] = usdFedFundSet;
		ISet<Index> usdLibor3Set = new HashSet<Index>();
		usdLibor3Set.Add(USD_LIBOR_3M);
		IDX_NAMES[FWD3_CURVE_NAME] = usdLibor3Set;
		for (int i = 0; i < DSC_NB_DEPO_NODES; i++)
		{
		  BusinessDayAdjustment bda = BusinessDayAdjustment.of(FOLLOWING, USNY);
		  TermDepositConvention convention = ImmutableTermDepositConvention.of("USD-Dep", USD, bda, ACT_360, DaysAdjustment.ofBusinessDays(DSC_DEPO_OFFSET[i], USNY));
		  DSC_NODES[i] = TermDepositCurveNode.of(TermDepositTemplate.of(Period.ofDays(1), convention), QuoteId.of(StandardId.of(SCHEME, DSC_ID_VALUE[i])));
		}
		for (int i = 0; i < DSC_NB_OIS_NODES; i++)
		{
		  DSC_NODES[DSC_NB_DEPO_NODES + i] = FixedOvernightSwapCurveNode.of(FixedOvernightSwapTemplate.of(Period.ZERO, Tenor.of(DSC_OIS_TENORS[i]), USD_FIXED_1Y_FED_FUND_OIS), QuoteId.of(StandardId.of(SCHEME, DSC_ID_VALUE[DSC_NB_DEPO_NODES + i])));
		}
		FWD3_NODES[0] = IborFixingDepositCurveNode.of(IborFixingDepositTemplate.of(USD_LIBOR_3M), QuoteId.of(StandardId.of(SCHEME, FWD3_ID_VALUE[0])));
		IborFutureConvention convention = ImmutableIborFutureConvention.of(USD_LIBOR_3M, DateSequences.QUARTERLY_IMM);
		for (int i = 0; i < FWD3_NB_FUT_NODES; i++)
		{
		  IborFutureTemplate template = IborFutureTemplate.of(Period.ofDays(7), FWD3_FUT_SEQ[i], convention);
		  FWD3_NODES[i + 1] = IborFutureCurveNode.of(template, QuoteId.of(StandardId.of(SCHEME, FWD3_ID_VALUE[i + 1])));
		}
		for (int i = 0; i < FWD3_NB_IRS_NODES; i++)
		{
		  FWD3_NODES[i + 1 + FWD3_NB_FUT_NODES] = FixedIborSwapCurveNode.of(FixedIborSwapTemplate.of(Period.ZERO, Tenor.of(FWD3_IRS_TENORS[i]), USD_FIXED_6M_LIBOR_3M), QuoteId.of(StandardId.of(SCHEME, FWD3_ID_VALUE[i + 1 + FWD3_NB_FUT_NODES])));
		}
		ImmutableMarketDataBuilder builder = ImmutableMarketData.builder(VAL_DATE);
		for (int i = 0; i < DSC_NB_NODES; i++)
		{
		  builder.addValue(QuoteId.of(StandardId.of(SCHEME, DSC_ID_VALUE[i])), DSC_MARKET_QUOTES[i]);
		}
		for (int i = 0; i < FWD3_NB_NODES; i++)
		{
		  builder.addValue(QuoteId.of(StandardId.of(SCHEME, FWD3_ID_VALUE[i])), FWD3_MARKET_QUOTES[i]);
		}
		ALL_QUOTES = builder.build();
		IList<CurveNode[]> groupDsc = new List<CurveNode[]>();
		groupDsc.Add(DSC_NODES);
		CURVES_NODES.Add(groupDsc);
		IList<CurveNode[]> groupFwd3 = new List<CurveNode[]>();
		groupFwd3.Add(FWD3_NODES);
		CURVES_NODES.Add(groupFwd3);
		IList<CurveMetadata> groupDsc = new List<CurveMetadata>();
		groupDsc.Add(DefaultCurveMetadata.builder().curveName(DSCON_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(CURVE_DC).build());
		CURVES_METADATA.Add(groupDsc);
		IList<CurveMetadata> groupFwd3 = new List<CurveMetadata>();
		groupFwd3.Add(DefaultCurveMetadata.builder().curveName(FWD3_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(CURVE_DC).build());
		CURVES_METADATA.Add(groupFwd3);
	  }

	  /// <summary>
	  /// Data for USD-DSCON curve </summary>
	  /* Market values */
	  private static readonly double[] DSC_MARKET_QUOTES = new double[] {0.0005, 0.0005, 0.00072000, 0.00082000, 0.00093000, 0.00090000, 0.00105000, 0.00118500, 0.00318650, 0.00318650, 0.00704000, 0.01121500, 0.01515000, 0.01845500, 0.02111000, 0.02332000, 0.02513500, 0.02668500};
	  private static readonly int DSC_NB_NODES = DSC_MARKET_QUOTES.Length;
	  private static readonly string[] DSC_ID_VALUE = new string[] {"USD-ON", "USD-TN", "USD-OIS-1M", "USD-OIS-2M", "USD-OIS-3M", "USD-OIS-6M", "USD-OIS-9M", "USD-OIS-1Y", "USD-OIS-18M", "USD-OIS-2Y", "USD-OIS-3Y", "USD-OIS-4Y", "USD-OIS-5Y", "USD-OIS-6Y", "USD-OIS-7Y", "USD-OIS-8Y", "USD-OIS-9Y", "USD-OIS-10Y"};
	  /* Nodes */
	  private static readonly CurveNode[] DSC_NODES = new CurveNode[DSC_NB_NODES];
	  /* Tenors */
	  private static readonly int[] DSC_DEPO_OFFSET = new int[] {0, 1};
	  private static readonly int DSC_NB_DEPO_NODES = DSC_DEPO_OFFSET.Length;
	  private static readonly Period[] DSC_OIS_TENORS = new Period[] {Period.ofMonths(1), Period.ofMonths(2), Period.ofMonths(3), Period.ofMonths(6), Period.ofMonths(9), Period.ofYears(1), Period.ofMonths(18), Period.ofYears(2), Period.ofYears(3), Period.ofYears(4), Period.ofYears(5), Period.ofYears(6), Period.ofYears(7), Period.ofYears(8), Period.ofYears(9), Period.ofYears(10)};
	  private static readonly int DSC_NB_OIS_NODES = DSC_OIS_TENORS.Length;

	  /// <summary>
	  /// Data for USD-LIBOR3M curve </summary>
	  /* Market values */
	  private static readonly double[] FWD3_MARKET_QUOTES = new double[] {0.00236600, 0.9975, 0.9975, 0.9950, 0.9950, 0.9940, 0.9930, 0.9920, 0.9910, 0.00939150, 0.01380800, 0.01732000, 0.02000000, 0.02396200, 0.02500000, 0.02700000, 0.02930000};
	  private static readonly int FWD3_NB_NODES = FWD3_MARKET_QUOTES.Length;
	  private static readonly string[] FWD3_ID_VALUE = new string[] {"USD-Fixing-3M", "USD-ED1", "USD-ED2", "USD-ED3", "USD-ED4", "USD-ED5", "USD-ED6", "USD-ED7", "USD-ED8", "USD-IRS3M-3Y", "USD-IRS3M-4Y", "USD-IRS3M-5Y", "USD-IRS3M-6Y", "USD-IRS3M-7Y", "USD-IRS3M-8Y", "USD-IRS3M-9Y", "USD-IRS3M-10Y"};
	  /* Nodes */
	  private static readonly CurveNode[] FWD3_NODES = new CurveNode[FWD3_NB_NODES];
	  /* Tenors */
	  private static readonly int[] FWD3_FUT_SEQ = new int[] {1, 2, 3, 4, 5, 6, 7, 8};
	  private static readonly int FWD3_NB_FUT_NODES = FWD3_FUT_SEQ.Length;
	  private static readonly Period[] FWD3_IRS_TENORS = new Period[] {Period.ofYears(3), Period.ofYears(4), Period.ofYears(5), Period.ofYears(6), Period.ofYears(7), Period.ofYears(8), Period.ofYears(9), Period.ofYears(10)};
	  private static readonly int FWD3_NB_IRS_NODES = FWD3_IRS_TENORS.Length;

	  /// <summary>
	  /// All quotes for the curve calibration </summary>
	  private static readonly ImmutableMarketData ALL_QUOTES;

	  /// <summary>
	  /// All nodes by groups. </summary>
	  private static readonly IList<IList<CurveNode[]>> CURVES_NODES = new List<IList<CurveNode[]>>();

	  /// <summary>
	  /// All metadata by groups </summary>
	  private static readonly IList<IList<CurveMetadata>> CURVES_METADATA = new List<IList<CurveMetadata>>();

	  private static readonly DiscountingIborFixingDepositProductPricer FIXING_PRICER = DiscountingIborFixingDepositProductPricer.DEFAULT;
	  private static readonly HullWhiteIborFutureTradePricer FUT_PRICER = HullWhiteIborFutureTradePricer.DEFAULT;
	  private static readonly DiscountingSwapProductPricer SWAP_PRICER = DiscountingSwapProductPricer.DEFAULT;
	  private static readonly DiscountingTermDepositProductPricer DEPO_PRICER = DiscountingTermDepositProductPricer.DEFAULT;
	  private static readonly MarketQuoteSensitivityCalculator MQC = MarketQuoteSensitivityCalculator.DEFAULT;

	  // Create a HW one factor piecewise constant
	  private const double MEAN_REVERSION = 0.01;
	  private static readonly DoubleArray VOLATILITY = DoubleArray.of(0.01, 0.011, 0.012, 0.013, 0.014);
	  private static readonly DoubleArray VOLATILITY_TIME = DoubleArray.of(0.5, 1.0, 2.0, 5.0);
	  private static readonly HullWhiteOneFactorPiecewiseConstantParameters HW = HullWhiteOneFactorPiecewiseConstantParameters.of(MEAN_REVERSION, VOLATILITY, VOLATILITY_TIME);
	  private static readonly HullWhiteOneFactorPiecewiseConstantParametersProvider HW_PROVIDER = HullWhiteOneFactorPiecewiseConstantParametersProvider.of(HW, CURVE_DC, VAL_DATE.atStartOfDay(ZoneId.of("Europe/London")));

	  //Create a calibration measure for Ibor futures with the HW parameters used in the pricing
	  private static readonly TradeCalibrationMeasure<ResolvedIborFutureTrade> IBOR_FUT_PAR_SPREAD_HW = TradeCalibrationMeasure.of("IborFutureParSpreadHullWhite", typeof(ResolvedIborFutureTrade), (trade, p) => HullWhiteIborFutureTradePricer.DEFAULT.parSpread(trade, p, HW_PROVIDER, 0.0), (trade, p) => HullWhiteIborFutureTradePricer.DEFAULT.parSpreadSensitivityRates(trade, p, HW_PROVIDER));
	  private static readonly CalibrationMeasures HW_PAR_SPREAD = CalibrationMeasures.of(DSCON_NAME, IBOR_FUT_PAR_SPREAD_HW, TradeCalibrationMeasure.FRA_PAR_SPREAD, TradeCalibrationMeasure.SWAP_PAR_SPREAD, TradeCalibrationMeasure.FX_SWAP_PAR_SPREAD, TradeCalibrationMeasure.IBOR_FIXING_DEPOSIT_PAR_SPREAD, TradeCalibrationMeasure.TERM_DEPOSIT_PAR_SPREAD);
	  private static readonly RatesCurveCalibrator CALIBRATOR = RatesCurveCalibrator.of(1e-9, 1e-9, 100, HW_PAR_SPREAD);

	  // Constants
	  private const double TOLERANCE_PV = 1.0E-6;
	  private const double TOLERANCE_PV_DELTA = 1.0E+3;

	  private static readonly CurveGroupName CURVE_GROUP_NAME = CurveGroupName.of("USD-DSCON-LIBOR3M");
	  private static readonly InterpolatedNodalCurveDefinition DSC_CURVE_DEFN = InterpolatedNodalCurveDefinition.builder().name(DSCON_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(CURVE_DC).interpolator(INTERPOLATOR_LINEAR).extrapolatorLeft(EXTRAPOLATOR_FLAT).extrapolatorRight(EXTRAPOLATOR_FLAT).nodes(DSC_NODES).build();
	  private static readonly InterpolatedNodalCurveDefinition FWD3_CURVE_DEFN = InterpolatedNodalCurveDefinition.builder().name(FWD3_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(CURVE_DC).interpolator(INTERPOLATOR_LINEAR).extrapolatorLeft(EXTRAPOLATOR_FLAT).extrapolatorRight(EXTRAPOLATOR_FLAT).nodes(FWD3_NODES).build();
	  private static readonly RatesCurveGroupDefinition CURVE_GROUP_CONFIG = RatesCurveGroupDefinition.builder().name(CURVE_GROUP_NAME).addCurve(DSC_CURVE_DEFN, USD, USD_FED_FUND).addForwardCurve(FWD3_CURVE_DEFN, USD_LIBOR_3M).build();

	  //-------------------------------------------------------------------------
	  public virtual void calibration_present_value_oneGroup()
	  {
		RatesProvider result = CALIBRATOR.calibrate(CURVE_GROUP_CONFIG, ALL_QUOTES, REF_DATA);
		assertPresentValue(result);
	  }

	  public virtual void calibration_market_quote_sensitivity_one_group()
	  {
		double shift = 1.0E-6;
		System.Func<MarketData, RatesProvider> f = marketData => CALIBRATOR.calibrate(CURVE_GROUP_CONFIG, marketData, REF_DATA);
		calibration_market_quote_sensitivity_check(f, shift);
	  }

	  private void calibration_market_quote_sensitivity_check(System.Func<MarketData, RatesProvider> calibrator, double shift)
	  {
		double notional = 100_000_000.0;
		double spread = 0.0050;
		SwapTrade trade = FixedIborSwapConventions.USD_FIXED_1Y_LIBOR_3M.createTrade(VAL_DATE, Period.ofMonths(8), Tenor.TENOR_7Y, BuySell.BUY, notional, spread, REF_DATA);
		RatesProvider result = calibrator(ALL_QUOTES);
		ResolvedSwap product = trade.Product.resolve(REF_DATA);
		PointSensitivityBuilder pts = SWAP_PRICER.presentValueSensitivity(product, result);
		CurrencyParameterSensitivities ps = result.parameterSensitivity(pts.build());
		CurrencyParameterSensitivities mqs = MQC.sensitivity(ps, result);
		double pv0 = SWAP_PRICER.presentValue(product, result).getAmount(USD).Amount;
		double[] mqsDscComputed = mqs.getSensitivity(DSCON_CURVE_NAME, USD).Sensitivity.toArray();
		for (int i = 0; i < DSC_NB_NODES; i++)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, Object> map = new java.util.HashMap<>(ALL_QUOTES.getValues());
		  IDictionary<MarketDataId<object>, object> map = new Dictionary<MarketDataId<object>, object>(ALL_QUOTES.Values);
		  map[QuoteId.of(StandardId.of(SCHEME, DSC_ID_VALUE[i]))] = DSC_MARKET_QUOTES[i] + shift;
		  ImmutableMarketData marketData = ImmutableMarketData.of(VAL_DATE, map);
		  RatesProvider rpShifted = calibrator(marketData);
		  double pvS = SWAP_PRICER.presentValue(product, rpShifted).getAmount(USD).Amount;
		  assertEquals(mqsDscComputed[i], (pvS - pv0) / shift, TOLERANCE_PV_DELTA, "DSC - node " + i);
		}
		double[] mqsFwd3Computed = mqs.getSensitivity(FWD3_CURVE_NAME, USD).Sensitivity.toArray();
		for (int i = 0; i < FWD3_NB_NODES; i++)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, Object> map = new java.util.HashMap<>(ALL_QUOTES.getValues());
		  IDictionary<MarketDataId<object>, object> map = new Dictionary<MarketDataId<object>, object>(ALL_QUOTES.Values);
		  map[QuoteId.of(StandardId.of(SCHEME, FWD3_ID_VALUE[i]))] = FWD3_MARKET_QUOTES[i] + shift;
		  ImmutableMarketData marketData = ImmutableMarketData.of(VAL_DATE, map);
		  RatesProvider rpShifted = calibrator(marketData);
		  double pvS = SWAP_PRICER.presentValue(product, rpShifted).getAmount(USD).Amount;
		  assertEquals(mqsFwd3Computed[i], (pvS - pv0) / shift, TOLERANCE_PV_DELTA, "FWD3 - node " + i);
		}
	  }

	  private void assertPresentValue(RatesProvider result)
	  {
		// Test PV Dsc
		CurveNode[] dscNodes = CURVES_NODES[0][0];
		IList<ResolvedTrade> dscTrades = new List<ResolvedTrade>();
		for (int i = 0; i < dscNodes.Length; i++)
		{
		  dscTrades.Add(dscNodes[i].resolvedTrade(1d, ALL_QUOTES, REF_DATA));
		}
		// Depo
		for (int i = 0; i < DSC_NB_DEPO_NODES; i++)
		{
		  CurrencyAmount pvIrs = DEPO_PRICER.presentValue(((ResolvedTermDepositTrade) dscTrades[i]).Product, result);
		  assertEquals(pvIrs.Amount, 0.0, TOLERANCE_PV);
		}
		// OIS
		for (int i = 0; i < DSC_NB_OIS_NODES; i++)
		{
		  MultiCurrencyAmount pvIrs = SWAP_PRICER.presentValue(((ResolvedSwapTrade) dscTrades[DSC_NB_DEPO_NODES + i]).Product, result);
		  assertEquals(pvIrs.getAmount(USD).Amount, 0.0, TOLERANCE_PV);
		}
		// Test PV Fwd3
		CurveNode[] fwd3Nodes = CURVES_NODES[1][0];
		IList<ResolvedTrade> fwd3Trades = new List<ResolvedTrade>();
		for (int i = 0; i < fwd3Nodes.Length; i++)
		{
		  fwd3Trades.Add(fwd3Nodes[i].resolvedTrade(1d, ALL_QUOTES, REF_DATA));
		}
		// Fixing 
		CurrencyAmount pvFixing3 = FIXING_PRICER.presentValue(((ResolvedIborFixingDepositTrade) fwd3Trades[0]).Product, result);
		assertEquals(pvFixing3.Amount, 0.0, TOLERANCE_PV);
		// Futures
		for (int i = 0; i < FWD3_NB_FUT_NODES; i++)
		{
		  CurrencyAmount pvFut = FUT_PRICER.presentValue(((ResolvedIborFutureTrade) fwd3Trades[i + 1]), result, HW_PROVIDER, 0.0);
		  assertEquals(pvFut.Amount, 0.0, TOLERANCE_PV);
		}
		// IRS
		for (int i = 0; i < FWD3_NB_IRS_NODES; i++)
		{
		  MultiCurrencyAmount pvIrs = SWAP_PRICER.presentValue(((ResolvedSwapTrade) fwd3Trades[i + 1 + FWD3_NB_FUT_NODES]).Product, result);
		  assertEquals(pvIrs.getAmount(USD).Amount, 0.0, TOLERANCE_PV);
		}
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unused") @Test(enabled = false) void performance()
	  internal virtual void performance()
	  {
		long startTime, endTime;
		int nbTests = 100;
		int nbRep = 3;
		int count = 0;

		for (int i = 0; i < nbRep; i++)
		{
		  startTime = DateTimeHelper.CurrentUnixTimeMillis();
		  for (int looprep = 0; looprep < nbTests; looprep++)
		  {
			RatesProvider result = CALIBRATOR.calibrate(CURVE_GROUP_CONFIG, ALL_QUOTES, REF_DATA);
			count += result.ValuationDate.DayOfMonth;
		  }
		  endTime = DateTimeHelper.CurrentUnixTimeMillis();
		  Console.WriteLine("Performance: " + nbTests + " calibrations for 2 curves with 35 nodes in " + (endTime - startTime) + " ms.");
		}
		Console.WriteLine("Avoiding hotspot: " + count);
		// Previous run: 670 ms for 100 calibrations (2 curves simultaneous - 35 nodes)
	  }

	}

}