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

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
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
	using CurveNodeDate = com.opengamma.strata.market.curve.CurveNodeDate;
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
	using TermDepositCurveNode = com.opengamma.strata.market.curve.node.TermDepositCurveNode;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using MeetingDatesDataSets = com.opengamma.strata.pricer.datasets.MeetingDatesDataSets;
	using DiscountingIborFixingDepositProductPricer = com.opengamma.strata.pricer.deposit.DiscountingIborFixingDepositProductPricer;
	using DiscountingTermDepositProductPricer = com.opengamma.strata.pricer.deposit.DiscountingTermDepositProductPricer;
	using DiscountingFraTradePricer = com.opengamma.strata.pricer.fra.DiscountingFraTradePricer;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
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
	/// The discounting curve is calibrated with the dates of the next FOMC meetings are nodes.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CalibrationDiscountFactorUsd2FomcDatesOisIrsTest
	public class CalibrationDiscountFactorUsd2FomcDatesOisIrsTest
	{

	  private static readonly LocalDate VAL_DATE_BD = LocalDate.of(2015, 7, 21);

	  private static readonly CurveInterpolator INTERPOLATOR_LINEAR = CurveInterpolators.LINEAR;
	  private static readonly CurveInterpolator INTERPOLATOR_LOGLINEAR = CurveInterpolators.LOG_LINEAR;
	  private static readonly CurveExtrapolator EXTRAPOLATOR_FLAT = CurveExtrapolators.FLAT;
	  private static readonly CurveExtrapolator EXTRAPOLATOR_EXP = CurveExtrapolators.EXPONENTIAL;
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
	  static CalibrationDiscountFactorUsd2FomcDatesOisIrsTest()
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
		TS_BD_LIBOR3M = ImmutableMarketData.builder(VAL_DATE_BD).addTimeSeries(IndexQuoteId.of(USD_LIBOR_3M), tsBdUsdLibor3M).build();
		for (int i = 0; i < DSC_NB_DEPO_NODES; i++)
		{
		  BusinessDayAdjustment bda = BusinessDayAdjustment.of(FOLLOWING, USNY);
		  TermDepositConvention convention = ImmutableTermDepositConvention.of("USD-Dep", USD, bda, ACT_360, DaysAdjustment.ofBusinessDays(DSC_DEPO_OFFSET[i], USNY));
		  LocalDate nodeDate = FOMC_NODES[i];
		  if (nodeDate != null)
		  {
			DSC_NODES[i] = TermDepositCurveNode.builder().template(TermDepositTemplate.of(Period.ofDays(1), convention)).rateId(QuoteId.of(StandardId.of(SCHEME, DSC_ID_VALUE[i]))).date(CurveNodeDate.of(nodeDate)).build();
		  }
		  else
		  {
			DSC_NODES[i] = TermDepositCurveNode.of(TermDepositTemplate.of(Period.ofDays(1), convention), QuoteId.of(StandardId.of(SCHEME, DSC_ID_VALUE[i])));
		  }
		}
		for (int i = 0; i < DSC_NB_OIS_NODES; i++)
		{
		  LocalDate nodeDate = FOMC_NODES[DSC_NB_DEPO_NODES + i];
		  if (nodeDate != null)
		  {
			DSC_NODES[DSC_NB_DEPO_NODES + i] = FixedOvernightSwapCurveNode.builder().template(FixedOvernightSwapTemplate.of(Period.ZERO, Tenor.of(DSC_OIS_TENORS[i]), USD_FIXED_1Y_FED_FUND_OIS)).rateId(QuoteId.of(StandardId.of(SCHEME, DSC_ID_VALUE[DSC_NB_DEPO_NODES + i]))).date(CurveNodeDate.of(nodeDate)).build();
		  }
		  else
		  {
			DSC_NODES[DSC_NB_DEPO_NODES + i] = FixedOvernightSwapCurveNode.of(FixedOvernightSwapTemplate.of(Period.ZERO, Tenor.of(DSC_OIS_TENORS[i]), USD_FIXED_1Y_FED_FUND_OIS), QuoteId.of(StandardId.of(SCHEME, DSC_ID_VALUE[DSC_NB_DEPO_NODES + i])));
		  }
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
	  private static readonly IDictionary<int, LocalDate> FOMC_NODES = fomcNodes(MeetingDatesDataSets.FOMC_MEETINGS, VAL_DATE_BD, 7); // select the next 7 FOMC dates as curve nodes
	  /* Market values */
	  private static readonly double[] DSC_MARKET_QUOTES = new double[] {0.001300, 0.001300, 0.001435, 0.001680, 0.002105, 0.002385, 0.002845, 0.003100, 0.003920, 0.005635, 0.007515, 0.010550, 0.013225, 0.05450};
	  private static readonly int DSC_NB_NODES = DSC_MARKET_QUOTES.Length;
	  private static readonly string[] DSC_ID_VALUE = new string[] {"USD-ON", "USD-TN", "OIS2M", "OIS3M", "OIS5M", "OIS6M", "OIS8M", "OIS9M", "OIS1Y", "OIS18M", "OIS2Y", "OIS3Y", "OIS4Y", "OIS5Y"};
	  /* Nodes */
	  private static readonly CurveNode[] DSC_NODES = new CurveNode[DSC_NB_NODES];
	  /* Tenors */
	  private static readonly int[] DSC_DEPO_OFFSET = new int[] {0, 1};
	  private static readonly int DSC_NB_DEPO_NODES = DSC_DEPO_OFFSET.Length;
	  private static readonly Period[] DSC_OIS_TENORS = new Period[] {Period.ofMonths(2), Period.ofMonths(3), Period.ofMonths(5), Period.ofMonths(6), Period.ofMonths(8), Period.ofMonths(9), Period.ofYears(1), Period.ofMonths(18), Period.ofYears(2), Period.ofYears(3), Period.ofYears(4), Period.ofYears(5)};
	  private static readonly int DSC_NB_OIS_NODES = DSC_OIS_TENORS.Length;

	  /// <summary>
	  /// Data for USD-LIBOR3M curve </summary>
	  /* Market values */
	  private static readonly double[] FWD3_MARKET_QUOTES = new double[] {0.00236600, 0.00258250, 0.00296050, 0.00294300, 0.00503000, 0.00939150, 0.01380800, 0.01732000};
	  private static readonly int FWD3_NB_NODES = FWD3_MARKET_QUOTES.Length;
	  private static readonly string[] FWD3_ID_VALUE = new string[] {"Fixing", "FRA3Mx6M", "FRA6Mx9M", "IRS1Y", "IRS2Y", "IRS3Y", "IRS4Y", "IRS5Y"};
	  /* Nodes */
	  private static readonly CurveNode[] FWD3_NODES = new CurveNode[FWD3_NB_NODES];
	  /* Tenors */
	  private static readonly Period[] FWD3_FRA_TENORS = new Period[] {Period.ofMonths(3), Period.ofMonths(6)};
	  private static readonly int FWD3_NB_FRA_NODES = FWD3_FRA_TENORS.Length;
	  private static readonly Period[] FWD3_IRS_TENORS = new Period[] {Period.ofYears(1), Period.ofYears(2), Period.ofYears(3), Period.ofYears(4), Period.ofYears(5)};
	  private static readonly int FWD3_NB_IRS_NODES = FWD3_IRS_TENORS.Length;

	  /// <summary>
	  /// All quotes for the curve calibration on good business day. </summary>
	  private static readonly ImmutableMarketData ALL_QUOTES_BD;

	  /// <summary>
	  /// All nodes by groups. </summary>
	  private static readonly IList<IList<CurveNode[]>> CURVES_NODES = new List<IList<CurveNode[]>>();

	  /// <summary>
	  /// All metadata by groups </summary>
	  private static readonly IList<IList<CurveMetadata>> CURVES_METADATA = new List<IList<CurveMetadata>>();

	  private static readonly DiscountingIborFixingDepositProductPricer FIXING_PRICER = DiscountingIborFixingDepositProductPricer.DEFAULT;
	  private static readonly DiscountingFraTradePricer FRA_PRICER = DiscountingFraTradePricer.DEFAULT;
	  private static readonly DiscountingSwapProductPricer SWAP_PRICER = DiscountingSwapProductPricer.DEFAULT;
	  private static readonly DiscountingTermDepositProductPricer DEPO_PRICER = DiscountingTermDepositProductPricer.DEFAULT;
	  private static readonly MarketQuoteSensitivityCalculator MQC = MarketQuoteSensitivityCalculator.DEFAULT;

	  private static readonly RatesCurveCalibrator CALIBRATOR = RatesCurveCalibrator.of(1e-9, 1e-9, 100);

	  // Constants
	  private const double TOLERANCE_PV = 1.0E-6;
	  private const double TOLERANCE_PV_DELTA = 2.0E+2;

	  private static readonly CurveGroupName CURVE_GROUP_NAME = CurveGroupName.of("USD-DSCON-LIBOR3M");
	  private static readonly InterpolatedNodalCurveDefinition DSC_CURVE_DEFN = InterpolatedNodalCurveDefinition.builder().name(DSCON_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.DISCOUNT_FACTOR).dayCount(CURVE_DC).interpolator(INTERPOLATOR_LOGLINEAR).extrapolatorLeft(EXTRAPOLATOR_EXP).extrapolatorRight(EXTRAPOLATOR_FLAT).nodes(DSC_NODES).build();
	  private static readonly InterpolatedNodalCurveDefinition FWD3_CURVE_DEFN = InterpolatedNodalCurveDefinition.builder().name(FWD3_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(CURVE_DC).interpolator(INTERPOLATOR_LINEAR).extrapolatorLeft(EXTRAPOLATOR_FLAT).extrapolatorRight(EXTRAPOLATOR_FLAT).nodes(FWD3_NODES).build();
	  private static readonly RatesCurveGroupDefinition CURVE_GROUP_CONFIG = RatesCurveGroupDefinition.builder().name(CURVE_GROUP_NAME).addCurve(DSC_CURVE_DEFN, USD, USD_FED_FUND).addForwardCurve(FWD3_CURVE_DEFN, USD_LIBOR_3M).build();

	  private static readonly RatesCurveGroupDefinition GROUP_1 = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("USD-DSCON")).addCurve(DSC_CURVE_DEFN, USD, USD_FED_FUND).build();
	  private static readonly RatesCurveGroupDefinition GROUP_2 = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("USD-LIBOR3M")).addForwardCurve(FWD3_CURVE_DEFN, USD_LIBOR_3M).build();
	  private static readonly ImmutableRatesProvider KNOWN_DATA = ImmutableRatesProvider.builder(VAL_DATE_BD).build();

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void calibration_present_value_oneGroup_no_fixing() throws java.io.IOException
	  public virtual void calibration_present_value_oneGroup_no_fixing()
	  {
		RatesProvider result = CALIBRATOR.calibrate(CURVE_GROUP_CONFIG, ALL_QUOTES_BD, REF_DATA);
		assertResult(result);
	  }

	  public virtual void calibration_present_value_oneGroup_fixing()
	  {
		RatesProvider result = CALIBRATOR.calibrate(CURVE_GROUP_CONFIG, ALL_QUOTES_BD.combinedWith(TS_BD_LIBOR3M), REF_DATA);
		assertResult(result);
	  }

	  public virtual void calibration_present_value_twoGroups()
	  {
		ImmutableRatesProvider result = CALIBRATOR.calibrate(ImmutableList.of(GROUP_1, GROUP_2), KNOWN_DATA, ALL_QUOTES_BD, REF_DATA);
		assertResult(result);
	  }

	  private void assertResult(RatesProvider result)
	  {
		// Test PV Dsc
		CurveNode[] dscNodes = CURVES_NODES[0][0];
		IList<ResolvedTrade> dscTrades = new List<ResolvedTrade>();
		for (int i = 0; i < dscNodes.Length; i++)
		{
		  dscTrades.Add(dscNodes[i].resolvedTrade(1d, ALL_QUOTES_BD, REF_DATA));
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
		  fwd3Trades.Add(fwd3Nodes[i].resolvedTrade(1d, ALL_QUOTES_BD, REF_DATA));
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
		// Previous run: 375 ms for 100 calibrations (2 curve simultaneous - 30 nodes)
	  }

	  // Select the relevant FOMC meeting dates from a list of meetings
	  private static IDictionary<int, LocalDate> fomcNodes(IList<LocalDate> meetings, LocalDate calibrationDate, int nbNodes)
	  {
		IDictionary<int, LocalDate> map = new Dictionary<int, LocalDate>();
		LocalDate currentDate = calibrationDate;
		int? node = 0;
		int i = 0;
		while ((i < meetings.Count) && (node.Value < nbNodes))
		{
		  if (meetings[i].isAfter(currentDate))
		  {
			map[node + 1] = meetings[i]; // fixed dates periods start after ON
			node++;
			currentDate = meetings[i];
		  }
		  i++;
		}
		return map;
	  }

	}

}