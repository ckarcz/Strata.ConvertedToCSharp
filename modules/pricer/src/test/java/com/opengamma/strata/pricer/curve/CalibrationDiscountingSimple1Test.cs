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
//	import static com.opengamma.strata.product.swap.type.FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using Index = com.opengamma.strata.basics.index.Index;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using ImmutableMarketDataBuilder = com.opengamma.strata.data.ImmutableMarketDataBuilder;
	using MarketData = com.opengamma.strata.data.MarketData;
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
	using FraCurveNode = com.opengamma.strata.market.curve.node.FraCurveNode;
	using IborFixingDepositCurveNode = com.opengamma.strata.market.curve.node.IborFixingDepositCurveNode;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using DiscountingIborFixingDepositProductPricer = com.opengamma.strata.pricer.deposit.DiscountingIborFixingDepositProductPricer;
	using DiscountingFraTradePricer = com.opengamma.strata.pricer.fra.DiscountingFraTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;
	using ResolvedIborFixingDepositTrade = com.opengamma.strata.product.deposit.ResolvedIborFixingDepositTrade;
	using IborFixingDepositTemplate = com.opengamma.strata.product.deposit.type.IborFixingDepositTemplate;
	using ResolvedFraTrade = com.opengamma.strata.product.fra.ResolvedFraTrade;
	using FraTemplate = com.opengamma.strata.product.fra.type.FraTemplate;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;
	using FixedIborSwapTemplate = com.opengamma.strata.product.swap.type.FixedIborSwapTemplate;

	/// <summary>
	/// Test curve calibration
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CalibrationDiscountingSimple1Test
	public class CalibrationDiscountingSimple1Test
	{

	  private static readonly LocalDate VAL_DATE = LocalDate.of(2015, 7, 21);

	  private static readonly CurveInterpolator INTERPOLATOR_LINEAR = CurveInterpolators.LINEAR;
	  private static readonly CurveExtrapolator EXTRAPOLATOR_FLAT = CurveExtrapolators.FLAT;
	  private static readonly DayCount CURVE_DC = ACT_365F;

	  // reference data
	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private const string SCHEME = "CALIBRATION";

	  /// <summary>
	  /// Curve name </summary>
	  private const string ALL_NAME = "USD-ALL-FRAIRS3M";
	  private static readonly CurveName ALL_CURVE_NAME = CurveName.of(ALL_NAME);
	  /// <summary>
	  /// Curves associations to currencies and indices. </summary>
	  private static readonly IDictionary<CurveName, Currency> DSC_NAMES = new Dictionary<CurveName, Currency>();
	  private static readonly IDictionary<CurveName, ISet<Index>> IDX_NAMES = new Dictionary<CurveName, ISet<Index>>();
	  private static readonly ISet<Index> IBOR_INDICES = new HashSet<Index>();
	  static CalibrationDiscountingSimple1Test()
	  {
		IBOR_INDICES.Add(USD_LIBOR_3M);
		DSC_NAMES[ALL_CURVE_NAME] = USD;
		IDX_NAMES[ALL_CURVE_NAME] = IBOR_INDICES;
		ALL_NODES[0] = IborFixingDepositCurveNode.of(IborFixingDepositTemplate.of(USD_LIBOR_3M), QuoteId.of(StandardId.of(SCHEME, FWD3_ID_VALUE[0])));
		for (int i = 0; i < FWD3_NB_FRA_NODES; i++)
		{
		  ALL_NODES[i + 1] = FraCurveNode.of(FraTemplate.of(FWD3_FRA_TENORS[i], USD_LIBOR_3M), QuoteId.of(StandardId.of(SCHEME, FWD3_ID_VALUE[1])));
		}
		for (int i = 0; i < FWD3_NB_IRS_NODES; i++)
		{
		  ALL_NODES[i + 1 + FWD3_NB_FRA_NODES] = FixedIborSwapCurveNode.of(FixedIborSwapTemplate.of(Period.ZERO, Tenor.of(FWD3_IRS_TENORS[i]), USD_FIXED_6M_LIBOR_3M), QuoteId.of(StandardId.of(SCHEME, FWD3_ID_VALUE[i])));
		}
		ImmutableMarketDataBuilder builder = ImmutableMarketData.builder(VAL_DATE);
		for (int i = 0; i < FWD3_NB_NODES; i++)
		{
		  builder.addValue(QuoteId.of(StandardId.of(SCHEME, FWD3_ID_VALUE[i])), FWD3_MARKET_QUOTES[i]);
		}
		ALL_QUOTES = builder.build();
		IList<CurveNode[]> groupNodes = new List<CurveNode[]>();
		groupNodes.Add(ALL_NODES);
		CURVES_NODES.Add(groupNodes);
		IList<CurveMetadata> groupMetadata = new List<CurveMetadata>();
		groupMetadata.Add(DefaultCurveMetadata.builder().curveName(ALL_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(CURVE_DC).build());
		CURVES_METADATA.Add(groupMetadata);
	  }

	  /// <summary>
	  /// Market values for the Fwd 3M USD curve </summary>
	  private static readonly double[] FWD3_MARKET_QUOTES = new double[] {0.0420, 0.0420, 0.0420, 0.0420, 0.0430, 0.0470, 0.0540, 0.0570, 0.0600};
	  private static readonly int FWD3_NB_NODES = FWD3_MARKET_QUOTES.Length;
	  private static readonly string[] FWD3_ID_VALUE = new string[] {"Fixing", "FRA3Mx6M", "FRA6Mx9M", "IRS1Y", "IRS2Y", "IRS3Y", "IRS5Y", "IRS7Y", "IRS10Y"};
	  /// <summary>
	  /// Nodes for the Fwd 3M USD curve </summary>
	  private static readonly CurveNode[] ALL_NODES = new CurveNode[FWD3_NB_NODES];
	  /// <summary>
	  /// Tenors for the Fwd 3M USD swaps </summary>
	  private static readonly Period[] FWD3_FRA_TENORS = new Period[] {Period.ofMonths(3), Period.ofMonths(6)};
	  private static readonly int FWD3_NB_FRA_NODES = FWD3_FRA_TENORS.Length;
	  private static readonly Period[] FWD3_IRS_TENORS = new Period[] {Period.ofYears(1), Period.ofYears(2), Period.ofYears(3), Period.ofYears(5), Period.ofYears(7), Period.ofYears(10)};
	  private static readonly int FWD3_NB_IRS_NODES = FWD3_IRS_TENORS.Length;

	  /// <summary>
	  /// All quotes for the curve calibration </summary>
	  private static readonly MarketData ALL_QUOTES;

	  /// <summary>
	  /// All nodes by groups. </summary>
	  private static readonly IList<IList<CurveNode[]>> CURVES_NODES = new List<IList<CurveNode[]>>();

	  /// <summary>
	  /// All metadata by groups </summary>
	  private static readonly IList<IList<CurveMetadata>> CURVES_METADATA = new List<IList<CurveMetadata>>();

	  private static readonly DiscountingIborFixingDepositProductPricer FIXING_PRICER = DiscountingIborFixingDepositProductPricer.DEFAULT;
	  private static readonly DiscountingFraTradePricer FRA_PRICER = DiscountingFraTradePricer.DEFAULT;
	  private static readonly DiscountingSwapProductPricer SWAP_PRICER = DiscountingSwapProductPricer.DEFAULT;

	  private static readonly RatesCurveCalibrator CALIBRATOR = RatesCurveCalibrator.of(1e-9, 1e-9, 100);

	  // Constants
	  private const double TOLERANCE_PV = 1.0E-6;

	  /// <summary>
	  /// Test with CurveGroupDefinition </summary>
	  private const string CURVE_GROUP_NAME_STR = "USD-SINGLE-CURVE";
	  private static readonly CurveGroupName CURVE_GROUP_NAME = CurveGroupName.of(CURVE_GROUP_NAME_STR);
	  private static readonly InterpolatedNodalCurveDefinition CURVE_DEFN = InterpolatedNodalCurveDefinition.builder().name(ALL_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(CURVE_DC).interpolator(INTERPOLATOR_LINEAR).extrapolatorLeft(EXTRAPOLATOR_FLAT).extrapolatorRight(EXTRAPOLATOR_FLAT).nodes(ALL_NODES).build();
	  private static readonly RatesCurveGroupDefinition CURVE_GROUP_DEFN = RatesCurveGroupDefinition.builder().name(CURVE_GROUP_NAME).addCurve(CURVE_DEFN, USD, USD_LIBOR_3M).build();

	  //-------------------------------------------------------------------------
	  public virtual void calibration_present_value()
	  {
		RatesProvider result2 = CALIBRATOR.calibrate(CURVE_GROUP_DEFN, ALL_QUOTES, REF_DATA);
		// Test PV
		CurveNode[] fwd3Nodes = CURVES_NODES[0][0];
		IList<ResolvedTrade> fwd3Trades = new List<ResolvedTrade>();
		for (int i = 0; i < fwd3Nodes.Length; i++)
		{
		  fwd3Trades.Add(fwd3Nodes[i].resolvedTrade(1d, ALL_QUOTES, REF_DATA));
		}
		// Fixing 
		CurrencyAmount pvFixing2 = FIXING_PRICER.presentValue(((ResolvedIborFixingDepositTrade) fwd3Trades[0]).Product, result2);
		assertEquals(pvFixing2.Amount, 0.0, TOLERANCE_PV);
		// FRA
		for (int i = 0; i < FWD3_NB_FRA_NODES; i++)
		{
		  CurrencyAmount pvFra2 = FRA_PRICER.presentValue(((ResolvedFraTrade) fwd3Trades[i + 1]), result2);
		  assertEquals(pvFra2.Amount, 0.0, TOLERANCE_PV);
		}
		// IRS
		for (int i = 0; i < FWD3_NB_IRS_NODES; i++)
		{
		  MultiCurrencyAmount pvIrs2 = SWAP_PRICER.presentValue(((ResolvedSwapTrade) fwd3Trades[i + 1 + FWD3_NB_FRA_NODES]).Product, result2);
		  assertEquals(pvIrs2.getAmount(USD).Amount, 0.0, TOLERANCE_PV);
		}
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(enabled = false) void performance()
	  internal virtual void performance()
	  {
		long startTime, endTime;
		int nbTests = 100;
		int nbRep = 5;
		int count = 0;

		for (int i = 0; i < nbRep; i++)
		{
		  startTime = DateTimeHelper.CurrentUnixTimeMillis();
		  for (int looprep = 0; looprep < nbTests; looprep++)
		  {
			RatesProvider result = CALIBRATOR.calibrate(CURVE_GROUP_DEFN, ALL_QUOTES, REF_DATA);
			count += result.ValuationDate.DayOfMonth;
		  }
		  endTime = DateTimeHelper.CurrentUnixTimeMillis();
		  Console.WriteLine("Performance: " + nbTests + " calibrations for 1 curve with 9 nodes in " + (endTime - startTime) + " ms.");
		}
		Console.WriteLine("Avoiding hotspot: " + count);
		// Previous run: 290 ms for 100 calibrations (1 curve - 9 nodes)
	  }

	}

}