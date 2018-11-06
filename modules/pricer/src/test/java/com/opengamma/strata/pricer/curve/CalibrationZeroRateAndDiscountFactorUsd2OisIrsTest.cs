using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using Index = com.opengamma.strata.basics.index.Index;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
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
	using FraCurveNode = com.opengamma.strata.market.curve.node.FraCurveNode;
	using IborFixingDepositCurveNode = com.opengamma.strata.market.curve.node.IborFixingDepositCurveNode;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using DiscountingIborFixingDepositProductPricer = com.opengamma.strata.pricer.deposit.DiscountingIborFixingDepositProductPricer;
	using DiscountingFraTradePricer = com.opengamma.strata.pricer.fra.DiscountingFraTradePricer;
	using IborIndexRates = com.opengamma.strata.pricer.rate.IborIndexRates;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SimpleIborIndexRates = com.opengamma.strata.pricer.rate.SimpleIborIndexRates;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using ResolvedIborFixingDepositTrade = com.opengamma.strata.product.deposit.ResolvedIborFixingDepositTrade;
	using IborFixingDepositTemplate = com.opengamma.strata.product.deposit.type.IborFixingDepositTemplate;
	using ResolvedFraTrade = com.opengamma.strata.product.fra.ResolvedFraTrade;
	using FraTemplate = com.opengamma.strata.product.fra.type.FraTemplate;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;
	using FixedIborSwapConventions = com.opengamma.strata.product.swap.type.FixedIborSwapConventions;
	using FixedIborSwapTemplate = com.opengamma.strata.product.swap.type.FixedIborSwapTemplate;
	using FixedOvernightSwapTemplate = com.opengamma.strata.product.swap.type.FixedOvernightSwapTemplate;

	/// <summary>
	/// Test for curve calibration with 2 curves in USD.
	/// One curve is Discounting and Fed Fund forward and the other one is Libor 3M forward.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CalibrationZeroRateAndDiscountFactorUsd2OisIrsTest
	public class CalibrationZeroRateAndDiscountFactorUsd2OisIrsTest
	{

	  private static readonly LocalDate VAL_DATE_BD = LocalDate.of(2015, 7, 21);
	  private static readonly LocalDate VAL_DATE_HO = LocalDate.of(2015, 12, 25);

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
	  private const string FWD3_NAME = "USD-LIBOR3M-FRAIRS";
	  private static readonly CurveName FWD3_CURVE_NAME = CurveName.of(FWD3_NAME);
	  /// <summary>
	  /// Curves associations to currencies and indices. </summary>
	  private static readonly IDictionary<CurveName, Currency> DSC_NAMES = new Dictionary<CurveName, Currency>();
	  private static readonly IDictionary<CurveName, ISet<Index>> IDX_NAMES = new Dictionary<CurveName, ISet<Index>>();
	  private static readonly MarketData TS_EMPTY = MarketData.empty(VAL_DATE_BD);
	  private static readonly MarketData TS_BD_LIBOR3M;
	  private static readonly MarketData TS_HO_LIBOR3M;
	  static CalibrationZeroRateAndDiscountFactorUsd2OisIrsTest()
	  {
		DSC_NAMES[DSCON_CURVE_NAME] = USD;
		ISet<Index> usdFedFundSet = new HashSet<Index>();
		usdFedFundSet.Add(USD_FED_FUND);
		IDX_NAMES[DSCON_CURVE_NAME] = usdFedFundSet;
		ISet<Index> usdLibor3Set = new HashSet<Index>();
		usdLibor3Set.Add(USD_LIBOR_3M);
		IDX_NAMES[FWD3_CURVE_NAME] = usdLibor3Set;
		double fixingValue = 0.002345;
		LocalDateDoubleTimeSeries tsBdUsdLibor3M = LocalDateDoubleTimeSeries.builder().put(VAL_DATE_BD, fixingValue).build();
		LocalDate fixingDateHo = LocalDate.of(2015, 12, 24);
		LocalDateDoubleTimeSeries tsHoUsdLibor3M = LocalDateDoubleTimeSeries.builder().put(fixingDateHo, fixingValue).build();
		TS_BD_LIBOR3M = ImmutableMarketData.builder(VAL_DATE_BD).addTimeSeries(IndexQuoteId.of(USD_LIBOR_3M), tsBdUsdLibor3M).build();
		TS_HO_LIBOR3M = ImmutableMarketData.builder(VAL_DATE_HO).addTimeSeries(IndexQuoteId.of(USD_LIBOR_3M), tsHoUsdLibor3M).build();
		for (int i = 0; i < DSC_NB_OIS_NODES; i++)
		{
		  DSC_NODES[i] = FixedOvernightSwapCurveNode.of(FixedOvernightSwapTemplate.of(Period.ZERO, Tenor.of(DSC_OIS_TENORS[i]), USD_FIXED_1Y_FED_FUND_OIS), QuoteId.of(StandardId.of(SCHEME, DSC_ID_VALUE[i])));
		}
		FWD3_NODES[0] = IborFixingDepositCurveNode.of(IborFixingDepositTemplate.of(USD_LIBOR_3M), QuoteId.of(StandardId.of(SCHEME, FWD3_ID_VALUE[0])));
		for (int i = 0; i < FWD3_NB_FRA_NODES; i++)
		{
		  FWD3_NODES[i + 1] = FraCurveNode.of(FraTemplate.of(FWD3_FRA_TENORS[i], USD_LIBOR_3M), QuoteId.of(StandardId.of(SCHEME, FWD3_ID_VALUE[i + 1])));
		}
		for (int i = 0; i < FWD3_NB_IRS_NODES; i++)
		{
		  FWD3_NODES[i + 1 + FWD3_NB_FRA_NODES] = FixedIborSwapCurveNode.of(FixedIborSwapTemplate.of(Period.ZERO, Tenor.of(FWD3_IRS_TENORS[i]), USD_FIXED_6M_LIBOR_3M), QuoteId.of(StandardId.of(SCHEME, FWD3_ID_VALUE[i + 1 + FWD3_NB_FRA_NODES])));
		}
		ImmutableMarketDataBuilder builder = ImmutableMarketData.builder(VAL_DATE_BD);
		for (int i = 0; i < FWD3_NB_NODES; i++)
		{
		  builder.addValue(QuoteId.of(StandardId.of(SCHEME, FWD3_ID_VALUE[i])), FWD3_MARKET_QUOTES[i]);
		}
		for (int i = 0; i < DSC_NB_NODES; i++)
		{
		  builder.addValue(QuoteId.of(StandardId.of(SCHEME, DSC_ID_VALUE[i])), DSC_MARKET_QUOTES[i]);
		}
		ALL_QUOTES_BD = builder.build();
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
	  private static readonly double[] DSC_MARKET_QUOTES = new double[] {0.00072000, 0.00082000, 0.00093000, 0.00090000, 0.00105000, 0.00118500, 0.00318650, 0.00318650, 0.00704000, 0.01121500, 0.01515000, 0.01845500, 0.02111000, 0.02332000, 0.02513500, 0.02668500};
	  private static readonly int DSC_NB_NODES = DSC_MARKET_QUOTES.Length;
	  private static readonly string[] DSC_ID_VALUE = new string[] {"OIS1M", "OIS2M", "OIS3M", "OIS6M", "OIS9M", "OIS1Y", "OIS18M", "OIS2Y", "OIS3Y", "OIS4Y", "OIS5Y", "OIS6Y", "OIS7Y", "OIS8Y", "OIS9Y", "OIS10Y"};
	  /* Nodes */
	  private static readonly CurveNode[] DSC_NODES = new CurveNode[DSC_NB_NODES];
	  /* Tenors */
	  private static readonly Period[] DSC_OIS_TENORS = new Period[] {Period.ofMonths(1), Period.ofMonths(2), Period.ofMonths(3), Period.ofMonths(6), Period.ofMonths(9), Period.ofYears(1), Period.ofMonths(18), Period.ofYears(2), Period.ofYears(3), Period.ofYears(4), Period.ofYears(5), Period.ofYears(6), Period.ofYears(7), Period.ofYears(8), Period.ofYears(9), Period.ofYears(10)};
	  private static readonly int DSC_NB_OIS_NODES = DSC_OIS_TENORS.Length;

	  /// <summary>
	  /// Data for USD-LIBOR3M curve </summary>
	  /* Market values */
	  private static readonly double[] FWD3_MARKET_QUOTES = new double[] {0.00236600, 0.00258250, 0.00296050, 0.00294300, 0.00503000, 0.00939150, 0.01380800, 0.01732000, 0.02396200, 0.02930000, 0.03195000, 0.03423500, 0.03615500, 0.03696850, 0.03734500};
	  private static readonly int FWD3_NB_NODES = FWD3_MARKET_QUOTES.Length;
	  private static readonly string[] FWD3_ID_VALUE = new string[] {"Fixing", "FRA3Mx6M", "FRA6Mx9M", "IRS1Y", "IRS2Y", "IRS3Y", "IRS4Y", "IRS5Y", "IRS7Y", "IRS10Y", "IRS12Y", "IRS15Y", "IRS20Y", "IRS25Y", "IRS30Y"};
	  /* Nodes */
	  private static readonly CurveNode[] FWD3_NODES = new CurveNode[FWD3_NB_NODES];
	  /* Tenors */
	  private static readonly Period[] FWD3_FRA_TENORS = new Period[] {Period.ofMonths(3), Period.ofMonths(6)};
	  private static readonly int FWD3_NB_FRA_NODES = FWD3_FRA_TENORS.Length;
	  private static readonly Period[] FWD3_IRS_TENORS = new Period[] {Period.ofYears(1), Period.ofYears(2), Period.ofYears(3), Period.ofYears(4), Period.ofYears(5), Period.ofYears(7), Period.ofYears(10), Period.ofYears(12), Period.ofYears(15), Period.ofYears(20), Period.ofYears(25), Period.ofYears(30)};
	  private static readonly int FWD3_NB_IRS_NODES = FWD3_IRS_TENORS.Length;

	  /// <summary>
	  /// All quotes for the curve calibration on good business day. </summary>
	  private static readonly ImmutableMarketData ALL_QUOTES_BD;

	  /// <summary>
	  /// All quotes for the curve calibration on holiday. </summary>
	  private static readonly ImmutableMarketData ALL_QUOTES_HO = ALL_QUOTES_BD.toBuilder().valuationDate(VAL_DATE_HO).build();

	  /// <summary>
	  /// All nodes by groups. </summary>
	  private static readonly IList<IList<CurveNode[]>> CURVES_NODES = new List<IList<CurveNode[]>>();

	  /// <summary>
	  /// All metadata by groups </summary>
	  private static readonly IList<IList<CurveMetadata>> CURVES_METADATA = new List<IList<CurveMetadata>>();

	  private static readonly DiscountingIborFixingDepositProductPricer FIXING_PRICER = DiscountingIborFixingDepositProductPricer.DEFAULT;
	  private static readonly DiscountingFraTradePricer FRA_PRICER = DiscountingFraTradePricer.DEFAULT;
	  private static readonly DiscountingSwapProductPricer SWAP_PRICER = DiscountingSwapProductPricer.DEFAULT;
	  private static readonly MarketQuoteSensitivityCalculator MQC = MarketQuoteSensitivityCalculator.DEFAULT;

	  private static readonly RatesCurveCalibrator CALIBRATOR = RatesCurveCalibrator.of(1e-9, 1e-9, 100);

	  // Constants
	  private const double TOLERANCE_PV = 1.0E-6;
	  private const double TOLERANCE_PV_DELTA = 1.0E+2;

	  private static readonly CurveGroupName CURVE_GROUP_NAME = CurveGroupName.of("USD-DSCON-LIBOR3M");
	  private static readonly InterpolatedNodalCurveDefinition DSC_CURVE_DEFN = InterpolatedNodalCurveDefinition.builder().name(DSCON_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(CURVE_DC).interpolator(INTERPOLATOR_LINEAR).extrapolatorLeft(EXTRAPOLATOR_FLAT).extrapolatorRight(EXTRAPOLATOR_FLAT).nodes(DSC_NODES).build();
	  private static readonly InterpolatedNodalCurveDefinition FWD3_CURVE_DEFN = InterpolatedNodalCurveDefinition.builder().name(FWD3_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(CURVE_DC).interpolator(INTERPOLATOR_LINEAR).extrapolatorLeft(EXTRAPOLATOR_FLAT).extrapolatorRight(EXTRAPOLATOR_FLAT).nodes(FWD3_NODES).build();
	  private static readonly RatesCurveGroupDefinition CURVE_GROUP_CONFIG = RatesCurveGroupDefinition.builder().name(CURVE_GROUP_NAME).addCurve(DSC_CURVE_DEFN, USD, USD_FED_FUND).addForwardCurve(FWD3_CURVE_DEFN, USD_LIBOR_3M).build();

	  private static readonly RatesCurveGroupDefinition GROUP_1 = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("USD-DSCON")).addCurve(DSC_CURVE_DEFN, USD, USD_FED_FUND).build();
	  private static readonly RatesCurveGroupDefinition GROUP_2 = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("USD-LIBOR3M")).addForwardCurve(FWD3_CURVE_DEFN, USD_LIBOR_3M).build();
	  private static readonly ImmutableRatesProvider KNOWN_DATA = ImmutableRatesProvider.builder(VAL_DATE_BD).build();

	  //-------------------------------------------------------------------------
	  public virtual void calibration_present_value_oneGroup_no_fixing()
	  {
		RatesProvider result = CALIBRATOR.calibrate(CURVE_GROUP_CONFIG, ALL_QUOTES_BD, REF_DATA);
		assertResult(result, ALL_QUOTES_BD);
	  }

	  public virtual void calibration_present_value_oneGroup_fixing()
	  {
		RatesProvider result = CALIBRATOR.calibrate(CURVE_GROUP_CONFIG, ALL_QUOTES_BD.combinedWith(TS_BD_LIBOR3M), REF_DATA);
		assertResult(result, ALL_QUOTES_BD);
	  }

	  public virtual void calibration_present_value_oneGroup_holiday()
	  {
		RatesProvider result = CALIBRATOR.calibrate(CURVE_GROUP_CONFIG, ALL_QUOTES_HO.combinedWith(TS_HO_LIBOR3M), REF_DATA);
		assertResult(result, ALL_QUOTES_HO);
	  }

	  public virtual void calibration_present_value_twoGroups()
	  {
		RatesProvider result = CALIBRATOR.calibrate(ImmutableList.of(GROUP_1, GROUP_2), KNOWN_DATA, ALL_QUOTES_BD, REF_DATA);
		assertResult(result, ALL_QUOTES_BD);
	  }

	  private void assertResult(RatesProvider result, ImmutableMarketData allQuotes)
	  {
		// Test PV Dsc
		CurveNode[] dscNodes = CURVES_NODES[0][0];
		IList<ResolvedTrade> dscTrades = new List<ResolvedTrade>();
		for (int i = 0; i < dscNodes.Length; i++)
		{
		  dscTrades.Add(dscNodes[i].resolvedTrade(1d, allQuotes, REF_DATA));
		}
		// OIS
		for (int i = 0; i < DSC_NB_OIS_NODES; i++)
		{
		  MultiCurrencyAmount pvIrs = SWAP_PRICER.presentValue(((ResolvedSwapTrade) dscTrades[i]).Product, result);
		  assertEquals(pvIrs.getAmount(USD).Amount, 0.0, TOLERANCE_PV);
		}
		// Test PV Fwd3
		CurveNode[] fwd3Nodes = CURVES_NODES[1][0];
		IList<ResolvedTrade> fwd3Trades = new List<ResolvedTrade>();
		for (int i = 0; i < fwd3Nodes.Length; i++)
		{
		  fwd3Trades.Add(fwd3Nodes[i].resolvedTrade(1d, allQuotes, REF_DATA));
		}
		// Fixing 
		CurrencyAmount pvFixing = FIXING_PRICER.presentValue(((ResolvedIborFixingDepositTrade) fwd3Trades[0]).Product, result);
		assertEquals(pvFixing.Amount, 0.0, TOLERANCE_PV);
		// FRA
		for (int i = 0; i < FWD3_NB_FRA_NODES; i++)
		{
		  CurrencyAmount pvFra = FRA_PRICER.presentValue(((ResolvedFraTrade) fwd3Trades[i + 1]), result);
		  assertEquals(pvFra.Amount, 0.0, TOLERANCE_PV);
		}
		// IRS
		for (int i = 0; i < FWD3_NB_IRS_NODES; i++)
		{
		  MultiCurrencyAmount pvIrs = SWAP_PRICER.presentValue(((ResolvedSwapTrade) fwd3Trades[i + 1 + FWD3_NB_FRA_NODES]).Product, result);
		  assertEquals(pvIrs.getAmount(USD).Amount, 0.0, TOLERANCE_PV);
		}
	  }

	  public virtual void calibration_market_quote_sensitivity_one_group_no_fixing()
	  {
		double shift = 1.0E-6;
		System.Func<MarketData, RatesProvider> f = marketData => CALIBRATOR.calibrate(CURVE_GROUP_CONFIG, marketData, REF_DATA);
		calibration_market_quote_sensitivity_check(f, CURVE_GROUP_CONFIG, shift, TS_EMPTY);
	  }

	  public virtual void calibration_market_quote_sensitivity_one_group_fixing()
	  {
		double shift = 1.0E-6;
		System.Func<MarketData, RatesProvider> f = marketData => CALIBRATOR.calibrate(CURVE_GROUP_CONFIG, marketData, REF_DATA);
		calibration_market_quote_sensitivity_check(f, CURVE_GROUP_CONFIG, shift, TS_BD_LIBOR3M);
	  }

	  public virtual void calibration_market_quote_sensitivity_two_group()
	  {
		double shift = 1.0E-6;
		System.Func<MarketData, RatesProvider> calibrator = marketData => CALIBRATOR.calibrate(ImmutableList.of(GROUP_1, GROUP_2), KNOWN_DATA, marketData, REF_DATA);
		calibration_market_quote_sensitivity_check(calibrator, CURVE_GROUP_CONFIG, shift, TS_EMPTY);
	  }

	  private void calibration_market_quote_sensitivity_check(System.Func<MarketData, RatesProvider> calibrator, RatesCurveGroupDefinition config, double shift, MarketData ts)
	  {
		double notional = 100_000_000.0;
		double rate = 0.0400;
		SwapTrade trade = FixedIborSwapConventions.USD_FIXED_1Y_LIBOR_3M.createTrade(VAL_DATE_BD, Period.ofMonths(6), Tenor.TENOR_7Y, BuySell.BUY, notional, rate, REF_DATA);
		RatesProvider result = CALIBRATOR.calibrate(config, ALL_QUOTES_BD.combinedWith(ts), REF_DATA);
		ResolvedSwap product = trade.Product.resolve(REF_DATA);
		PointSensitivityBuilder pts = SWAP_PRICER.presentValueSensitivity(product, result);
		CurrencyParameterSensitivities ps = result.parameterSensitivity(pts.build());
		CurrencyParameterSensitivities mqs = MQC.sensitivity(ps, result);
		double pv0 = SWAP_PRICER.presentValue(product, result).getAmount(USD).Amount;
		double[] mqsDscComputed = mqs.getSensitivity(DSCON_CURVE_NAME, USD).Sensitivity.toArray();
		for (int i = 0; i < DSC_NB_NODES; i++)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, Object> map = new java.util.HashMap<>(ALL_QUOTES_BD.getValues());
		  IDictionary<MarketDataId<object>, object> map = new Dictionary<MarketDataId<object>, object>(ALL_QUOTES_BD.Values);
		  map[QuoteId.of(StandardId.of(SCHEME, DSC_ID_VALUE[i]))] = DSC_MARKET_QUOTES[i] + shift;
		  ImmutableMarketData marketData = ImmutableMarketData.of(VAL_DATE_BD, map);
		  RatesProvider rpShifted = calibrator(marketData.combinedWith(ts));
		  double pvS = SWAP_PRICER.presentValue(product, rpShifted).getAmount(USD).Amount;
		  assertEquals(mqsDscComputed[i], (pvS - pv0) / shift, TOLERANCE_PV_DELTA);
		}
		double[] mqsFwd3Computed = mqs.getSensitivity(FWD3_CURVE_NAME, USD).Sensitivity.toArray();
		for (int i = 0; i < FWD3_NB_NODES; i++)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, Object> map = new java.util.HashMap<>(ALL_QUOTES_BD.getValues());
		  IDictionary<MarketDataId<object>, object> map = new Dictionary<MarketDataId<object>, object>(ALL_QUOTES_BD.Values);
		  map[QuoteId.of(StandardId.of(SCHEME, FWD3_ID_VALUE[i]))] = FWD3_MARKET_QUOTES[i] + shift;
		  ImmutableMarketData marketData = ImmutableMarketData.of(VAL_DATE_BD, map);
		  RatesProvider rpShifted = calibrator(marketData.combinedWith(ts));
		  double pvS = SWAP_PRICER.presentValue(product, rpShifted).getAmount(USD).Amount;
		  assertEquals(mqsFwd3Computed[i], (pvS - pv0) / shift, TOLERANCE_PV_DELTA);
		}
	  }

	  /* Check calibration for discounting and forward curve interpolated on (pseudo-) discount factors. */
	  public virtual void calibration_present_value_discountCurve()
	  {
		CurveInterpolator interp = CurveInterpolators.LOG_LINEAR;
		CurveExtrapolator extrapRight = CurveExtrapolators.LOG_LINEAR;
		CurveExtrapolator extrapLeft = CurveExtrapolators.QUADRATIC_LEFT;
		InterpolatedNodalCurveDefinition dsc = InterpolatedNodalCurveDefinition.builder().name(DSCON_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.DISCOUNT_FACTOR).dayCount(CURVE_DC).interpolator(interp).extrapolatorLeft(extrapLeft).extrapolatorRight(extrapRight).nodes(DSC_NODES).build();
		InterpolatedNodalCurveDefinition fwd = InterpolatedNodalCurveDefinition.builder().name(FWD3_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.DISCOUNT_FACTOR).dayCount(CURVE_DC).interpolator(interp).extrapolatorLeft(extrapLeft).extrapolatorRight(extrapRight).nodes(FWD3_NODES).build();
		RatesCurveGroupDefinition config = RatesCurveGroupDefinition.builder().name(CURVE_GROUP_NAME).addCurve(dsc, USD, USD_FED_FUND).addForwardCurve(fwd, USD_LIBOR_3M).build();
		RatesProvider result = CALIBRATOR.calibrate(config, ALL_QUOTES_BD, REF_DATA);
		assertResult(result, ALL_QUOTES_BD);

		double shift = 1.0E-6;
		System.Func<MarketData, RatesProvider> f = marketData => CALIBRATOR.calibrate(config, marketData, REF_DATA);
		calibration_market_quote_sensitivity_check(f, config, shift, TS_EMPTY);
	  }

	  /* Check calibration for forward curve directly interpolated on forward rates. */
	  public virtual void calibration_present_value_simple_forward()
	  {
		InterpolatedNodalCurveDefinition dsc = InterpolatedNodalCurveDefinition.builder().name(DSCON_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(CURVE_DC).interpolator(INTERPOLATOR_LINEAR).extrapolatorLeft(EXTRAPOLATOR_FLAT).extrapolatorRight(EXTRAPOLATOR_FLAT).nodes(DSC_NODES).build();
		InterpolatedNodalCurveDefinition fwd = InterpolatedNodalCurveDefinition.builder().name(FWD3_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.FORWARD_RATE).dayCount(CURVE_DC).interpolator(INTERPOLATOR_LINEAR).extrapolatorLeft(EXTRAPOLATOR_FLAT).extrapolatorRight(EXTRAPOLATOR_FLAT).nodes(FWD3_NODES).build();
		RatesCurveGroupDefinition config = RatesCurveGroupDefinition.builder().name(CURVE_GROUP_NAME).addCurve(dsc, USD, USD_FED_FUND).addForwardCurve(fwd, USD_LIBOR_3M).build();
		RatesProvider result = CALIBRATOR.calibrate(config, ALL_QUOTES_BD, REF_DATA);
		assertResult(result, ALL_QUOTES_BD);
		IborIndexRates ibor3M = result.iborIndexRates(USD_LIBOR_3M);
		assertTrue(ibor3M is SimpleIborIndexRates, "USD-LIBOR-3M curve should be simple interpolation on forward rates");
		double shift = 1.0E-6;
		System.Func<MarketData, RatesProvider> f = marketData => CALIBRATOR.calibrate(config, marketData, REF_DATA);
		calibration_market_quote_sensitivity_check(f, config, shift, TS_EMPTY);
	  }

	  public virtual void calibration_present_value_discountCurve_clamped()
	  {
		CurveInterpolator interp = CurveInterpolators.LOG_NATURAL_SPLINE_DISCOUNT_FACTOR;
		CurveExtrapolator extrapRight = CurveExtrapolators.LOG_LINEAR;
		CurveExtrapolator extrapLeft = CurveExtrapolators.INTERPOLATOR;
		InterpolatedNodalCurveDefinition dsc = InterpolatedNodalCurveDefinition.builder().name(DSCON_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.DISCOUNT_FACTOR).dayCount(CURVE_DC).interpolator(interp).extrapolatorLeft(extrapLeft).extrapolatorRight(extrapRight).nodes(DSC_NODES).build();
		InterpolatedNodalCurveDefinition fwd = InterpolatedNodalCurveDefinition.builder().name(FWD3_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.DISCOUNT_FACTOR).dayCount(CURVE_DC).interpolator(interp).extrapolatorLeft(extrapLeft).extrapolatorRight(extrapRight).nodes(FWD3_NODES).build();
		RatesCurveGroupDefinition config = RatesCurveGroupDefinition.builder().name(CURVE_GROUP_NAME).addCurve(dsc, USD, USD_FED_FUND).addForwardCurve(fwd, USD_LIBOR_3M).build();
		RatesProvider result = CALIBRATOR.calibrate(config, ALL_QUOTES_BD, REF_DATA);
		assertResult(result, ALL_QUOTES_BD);

		double shift = 1.0E-6;
		System.Func<MarketData, RatesProvider> f = marketData => CALIBRATOR.calibrate(config, marketData, REF_DATA);
		calibration_market_quote_sensitivity_check(f, config, shift, TS_EMPTY);
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
			RatesProvider result = CALIBRATOR.calibrate(CURVE_GROUP_CONFIG, ALL_QUOTES_BD, REF_DATA);
			count += result.ValuationDate.DayOfMonth;
		  }
		  endTime = DateTimeHelper.CurrentUnixTimeMillis();
		  Console.WriteLine("Performance: " + nbTests + " calibrations for 2 curve with 30 nodes in " + (endTime - startTime) + " ms.");
		}
		Console.WriteLine("Avoiding hotspot: " + count);
		// Previous run: 1500 ms for 100 calibrations (2 curve simultaneous - 30 nodes)
	  }

	}

}