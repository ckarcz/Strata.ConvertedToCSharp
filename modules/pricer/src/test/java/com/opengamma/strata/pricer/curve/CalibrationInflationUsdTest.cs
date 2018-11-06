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
//	import static com.opengamma.strata.basics.index.PriceIndices.US_CPI_U;
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
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using Index = com.opengamma.strata.basics.index.Index;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using ImmutableMarketDataBuilder = com.opengamma.strata.data.ImmutableMarketDataBuilder;
	using ValueType = com.opengamma.strata.market.ValueType;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurveNode = com.opengamma.strata.market.curve.CurveNode;
	using CurveNodeDate = com.opengamma.strata.market.curve.CurveNodeDate;
	using InterpolatedNodalCurveDefinition = com.opengamma.strata.market.curve.InterpolatedNodalCurveDefinition;
	using CurveExtrapolator = com.opengamma.strata.market.curve.interpolator.CurveExtrapolator;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using FixedInflationSwapCurveNode = com.opengamma.strata.market.curve.node.FixedInflationSwapCurveNode;
	using FixedOvernightSwapCurveNode = com.opengamma.strata.market.curve.node.FixedOvernightSwapCurveNode;
	using TermDepositCurveNode = com.opengamma.strata.market.curve.node.TermDepositCurveNode;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using DiscountingTermDepositProductPricer = com.opengamma.strata.pricer.deposit.DiscountingTermDepositProductPricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;
	using ResolvedTermDepositTrade = com.opengamma.strata.product.deposit.ResolvedTermDepositTrade;
	using ImmutableTermDepositConvention = com.opengamma.strata.product.deposit.type.ImmutableTermDepositConvention;
	using TermDepositConvention = com.opengamma.strata.product.deposit.type.TermDepositConvention;
	using TermDepositTemplate = com.opengamma.strata.product.deposit.type.TermDepositTemplate;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;
	using FixedInflationSwapConventions = com.opengamma.strata.product.swap.type.FixedInflationSwapConventions;
	using FixedInflationSwapTemplate = com.opengamma.strata.product.swap.type.FixedInflationSwapTemplate;
	using FixedOvernightSwapTemplate = com.opengamma.strata.product.swap.type.FixedOvernightSwapTemplate;

	/// <summary>
	/// Test for curve calibration with 2 curves in USD.
	/// One curve is Discounting and Fed Fund forward and the other one is USD CPI price index.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CalibrationInflationUsdTest
	public class CalibrationInflationUsdTest
	{

	  private static readonly LocalDate VAL_DATE = LocalDate.of(2015, 7, 21);

	  private static readonly CurveInterpolator INTERPOLATOR_LINEAR = CurveInterpolators.LINEAR;
	  private static readonly CurveExtrapolator EXTRAPOLATOR_FLAT = CurveExtrapolators.FLAT;
	  private static readonly CurveInterpolator INTERPOLATOR_LOGLINEAR = CurveInterpolators.LOG_LINEAR;
	  private static readonly CurveExtrapolator EXTRAPOLATOR_EXP = CurveExtrapolators.EXPONENTIAL;
	  private static readonly DayCount CURVE_DC = ACT_365F;

	  // reference data
	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private const string SCHEME = "CALIBRATION";

	  /// <summary>
	  /// Curve names </summary>
	  private const string DSCON_NAME = "USD-DSCON-OIS";
	  private static readonly CurveName DSCON_CURVE_NAME = CurveName.of(DSCON_NAME);
	  private const string CPI_NAME = "USD-CPI-ZC";
	  private static readonly CurveName CPI_CURVE_NAME = CurveName.of(CPI_NAME);
	  /// <summary>
	  /// Curves associations to currencies and indices. </summary>
	  private static readonly IDictionary<CurveName, Currency> DSC_NAMES = new Dictionary<CurveName, Currency>();
	  private static readonly IDictionary<CurveName, ISet<Index>> IDX_NAMES = new Dictionary<CurveName, ISet<Index>>();
	  private static readonly LocalDateDoubleTimeSeries TS_USD_CPI = LocalDateDoubleTimeSeries.builder().put(LocalDate.of(2015, 6, 30), 123.4).build();
	  static CalibrationInflationUsdTest()
	  {
		DSC_NAMES[DSCON_CURVE_NAME] = USD;
		ISet<Index> usdFedFundSet = new HashSet<Index>();
		usdFedFundSet.Add(USD_FED_FUND);
		IDX_NAMES[DSCON_CURVE_NAME] = usdFedFundSet;
		ISet<Index> usdLibor3Set = new HashSet<Index>();
		usdLibor3Set.Add(USD_LIBOR_3M);
		IDX_NAMES[CPI_CURVE_NAME] = usdLibor3Set;
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
		for (int i = 0; i < CPI_NB_NODES; i++)
		{
		  CPI_NODES[i] = FixedInflationSwapCurveNode.builder().template(FixedInflationSwapTemplate.of(Tenor.of(CPI_TENORS[i]), FixedInflationSwapConventions.USD_FIXED_ZC_US_CPI)).rateId(QuoteId.of(StandardId.of(SCHEME, CPI_ID_VALUE[i]))).date(CurveNodeDate.LAST_FIXING).build();
		}
		ImmutableMarketDataBuilder builder = ImmutableMarketData.builder(VAL_DATE);
		for (int i = 0; i < DSC_NB_NODES; i++)
		{
		  builder.addValue(QuoteId.of(StandardId.of(SCHEME, DSC_ID_VALUE[i])), DSC_MARKET_QUOTES[i]);
		}
		for (int i = 0; i < CPI_NB_NODES; i++)
		{
		  builder.addValue(QuoteId.of(StandardId.of(SCHEME, CPI_ID_VALUE[i])), CPI_MARKET_QUOTES[i]);
		}
		builder.addTimeSeries(IndexQuoteId.of(US_CPI_U), TS_USD_CPI);
		ALL_QUOTES = builder.build();
		IList<CurveNode[]> groupDsc = new List<CurveNode[]>();
		groupDsc.Add(DSC_NODES);
		CURVES_NODES.Add(groupDsc);
		IList<CurveNode[]> groupCpi = new List<CurveNode[]>();
		groupCpi.Add(CPI_NODES);
		CURVES_NODES.Add(groupCpi);
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
	  /// Data for USD-CPI curve </summary>
	  /* Market values */
	  private static readonly double[] CPI_MARKET_QUOTES = new double[] {0.0200, 0.0200, 0.0200, 0.0200, 0.0200};
	  private static readonly int CPI_NB_NODES = CPI_MARKET_QUOTES.Length;
	  private static readonly string[] CPI_ID_VALUE = new string[] {"USD-CPI-1Y", "USD-CPI-2Y", "USD-CPI-3Y", "USD-CPI-4Y", "USD-CPI-5Y"};
	  /* Nodes */
	  private static readonly CurveNode[] CPI_NODES = new CurveNode[CPI_NB_NODES];
	  /* Tenors */
	  private static readonly Period[] CPI_TENORS = new Period[] {Period.ofYears(1), Period.ofYears(2), Period.ofYears(3), Period.ofYears(4), Period.ofYears(5)};

	  /// <summary>
	  /// All quotes for the curve calibration </summary>
	  private static readonly ImmutableMarketData ALL_QUOTES;

	  /// <summary>
	  /// All nodes by groups. </summary>
	  private static readonly IList<IList<CurveNode[]>> CURVES_NODES = new List<IList<CurveNode[]>>();

	  private static readonly DiscountingSwapProductPricer SWAP_PRICER = DiscountingSwapProductPricer.DEFAULT;
	  private static readonly DiscountingTermDepositProductPricer DEPO_PRICER = DiscountingTermDepositProductPricer.DEFAULT;

	  private static readonly RatesCurveCalibrator CALIBRATOR = RatesCurveCalibrator.of(1e-9, 1e-9, 100);

	  // Constants
	  private const double TOLERANCE_PV = 1.0E-6;

	  private static readonly CurveGroupName CURVE_GROUP_NAME = CurveGroupName.of("USD-DSCON-LIBOR3M");
	  private static readonly InterpolatedNodalCurveDefinition DSC_CURVE_DEFN = InterpolatedNodalCurveDefinition.builder().name(DSCON_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(CURVE_DC).interpolator(INTERPOLATOR_LINEAR).extrapolatorLeft(EXTRAPOLATOR_FLAT).extrapolatorRight(EXTRAPOLATOR_FLAT).nodes(DSC_NODES).build();
	  private static readonly InterpolatedNodalCurveDefinition CPI_CURVE_UNDER_DEFN = InterpolatedNodalCurveDefinition.builder().name(CPI_CURVE_NAME).xValueType(ValueType.MONTHS).yValueType(ValueType.PRICE_INDEX).dayCount(CURVE_DC).interpolator(INTERPOLATOR_LOGLINEAR).extrapolatorLeft(EXTRAPOLATOR_FLAT).extrapolatorRight(EXTRAPOLATOR_EXP).nodes(CPI_NODES).build();
	  private static readonly RatesCurveGroupDefinition CURVE_GROUP_CONFIG = RatesCurveGroupDefinition.builder().name(CURVE_GROUP_NAME).addCurve(DSC_CURVE_DEFN, USD, USD_FED_FUND).addForwardCurve(CPI_CURVE_UNDER_DEFN, US_CPI_U).build();

	  //-------------------------------------------------------------------------
	  public virtual void calibration_present_value_oneGroup()
	  {
		RatesProvider result = CALIBRATOR.calibrate(CURVE_GROUP_CONFIG, ALL_QUOTES, REF_DATA);
		assertPresentValue(result);
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
		// Test PV Infaltion swaps
		CurveNode[] cpiNodes = CURVES_NODES[1][0];
		IList<ResolvedTrade> cpiTrades = new List<ResolvedTrade>();
		for (int i = 0; i < cpiNodes.Length; i++)
		{
		  cpiTrades.Add(cpiNodes[i].resolvedTrade(1d, ALL_QUOTES, REF_DATA));
		}
		// ZC swaps
		for (int i = 0; i < CPI_NB_NODES; i++)
		{
		  MultiCurrencyAmount pvInfl = SWAP_PRICER.presentValue(((ResolvedSwapTrade) cpiTrades[i]).Product, result);
		  assertEquals(pvInfl.getAmount(USD).Amount, 0.0, TOLERANCE_PV);
		}
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unused") @Test(enabled = false) public void performance()
	  public virtual void performance()
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
		// Previous run: 275 ms for 100 calibrations (2 curves simultaneous - 35 nodes)
	  }

	}

}