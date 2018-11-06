using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.EUR_EONIA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedOvernightSwapConventions.EUR_FIXED_1Y_EONIA_OIS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using ImmutableMarketDataBuilder = com.opengamma.strata.data.ImmutableMarketDataBuilder;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurveDefinition = com.opengamma.strata.market.curve.CurveDefinition;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurveNode = com.opengamma.strata.market.curve.CurveNode;
	using InterpolatedNodalCurveDefinition = com.opengamma.strata.market.curve.InterpolatedNodalCurveDefinition;
	using CurveExtrapolator = com.opengamma.strata.market.curve.interpolator.CurveExtrapolator;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using FixedIborSwapCurveNode = com.opengamma.strata.market.curve.node.FixedIborSwapCurveNode;
	using FixedOvernightSwapCurveNode = com.opengamma.strata.market.curve.node.FixedOvernightSwapCurveNode;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;
	using FixedIborSwapTemplate = com.opengamma.strata.product.swap.type.FixedIborSwapTemplate;
	using FixedOvernightSwapTemplate = com.opengamma.strata.product.swap.type.FixedOvernightSwapTemplate;

	/// <summary>
	/// Test curve calibration
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CalibrationDiscountingSimpleEurStdTenorsTest
	public class CalibrationDiscountingSimpleEurStdTenorsTest
	{

	  private static readonly LocalDate VAL_DATE = LocalDate.of(2015, 7, 24);

	  private static readonly CurveInterpolator INTERPOLATOR_LINEAR = CurveInterpolators.LINEAR;
	  private static readonly CurveExtrapolator EXTRAPOLATOR_FLAT = CurveExtrapolators.FLAT;
	  private static readonly DayCount CURVE_DC = ACT_365F;

	  // reference data
	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private const string SCHEME = "CALIBRATION";

	  /// <summary>
	  /// Curve names </summary>
	  private const string DSCON_NAME = "EUR_EONIA_EOD";
	  private static readonly CurveName DSCON_CURVE_NAME = CurveName.of(DSCON_NAME);
	  private const string FWD3_NAME = "EUR_EURIBOR_3M";
	  private static readonly CurveName FWD3_CURVE_NAME = CurveName.of(FWD3_NAME);
	  private const string FWD6_NAME = "EUR_EURIBOR_6M";
	  private static readonly CurveName FWD6_CURVE_NAME = CurveName.of(FWD6_NAME);

	  /// <summary>
	  /// Data for EUR-DSCON curve </summary>
	  /* Market values */
	  private static readonly double[] DSC_MARKET_QUOTES = new double[] {-0.0010787505441382185, 0.0016443214916477351, 0.00791319942756944, 0.014309183236345927};
	  private static readonly int DSC_NB_NODES = DSC_MARKET_QUOTES.Length;
	  private static readonly string[] DSC_ID_VALUE = new string[] {"OIS2Y", "OIS5Y", "OIS10Y", "OIS30Y"};
	  /* Nodes */
	  private static readonly CurveNode[] DSC_NODES = new CurveNode[DSC_NB_NODES];
	  /* Tenors */
	  private static readonly Period[] DSC_OIS_TENORS = new Period[] {Period.ofYears(2), Period.ofYears(5), Period.ofYears(10), Period.ofYears(30)};
	  private static readonly int DSC_NB_OIS_NODES = DSC_OIS_TENORS.Length;
	  static CalibrationDiscountingSimpleEurStdTenorsTest()
	  {
		for (int i = 0; i < DSC_NB_OIS_NODES; i++)
		{
		  DSC_NODES[i] = FixedOvernightSwapCurveNode.of(FixedOvernightSwapTemplate.of(Period.ZERO, Tenor.of(DSC_OIS_TENORS[i]), EUR_FIXED_1Y_EONIA_OIS), QuoteId.of(StandardId.of(SCHEME, DSC_ID_VALUE[i])));
		}
		for (int i = 0; i < FWD3_NB_IRS_NODES; i++)
		{
		  FWD3_NODES[i] = FixedIborSwapCurveNode.of(FixedIborSwapTemplate.of(Period.ZERO, Tenor.of(FWD3_IRS_TENORS[i]), EUR_FIXED_1Y_EURIBOR_3M), QuoteId.of(StandardId.of(SCHEME, FWD3_ID_VALUE[i])));
		}
		for (int i = 0; i < FWD6_NB_IRS_NODES; i++)
		{
		  FWD6_NODES[i] = FixedIborSwapCurveNode.of(FixedIborSwapTemplate.of(Period.ZERO, Tenor.of(FWD6_IRS_TENORS[i]), EUR_FIXED_1Y_EURIBOR_6M), QuoteId.of(StandardId.of(SCHEME, FWD6_ID_VALUE[i])));
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
		for (int i = 0; i < FWD6_NB_NODES; i++)
		{
		  builder.addValue(QuoteId.of(StandardId.of(SCHEME, FWD6_ID_VALUE[i])), FWD6_MARKET_QUOTES[i]);
		}
		ALL_QUOTES = builder.build();
	  }

	  /// <summary>
	  /// Data for EUR-LIBOR3M curve </summary>
	  /* Market values */
	  private static readonly double[] FWD3_MARKET_QUOTES = new double[] {0.00013533281680009178, 0.0031298573232152152, 0.009328861288116275, 0.015219571759282416};
	  private static readonly int FWD3_NB_NODES = FWD3_MARKET_QUOTES.Length;
	  private static readonly string[] FWD3_ID_VALUE = new string[] {"IRS3M_2Y", "IRS3M_5Y", "IRS3M_10Y", "IRS3M_30Y"};
	  /* Nodes */
	  private static readonly CurveNode[] FWD3_NODES = new CurveNode[FWD3_NB_NODES];
	  /* Tenors */
	  private static readonly Period[] FWD3_IRS_TENORS = new Period[] {Period.ofYears(2), Period.ofYears(5), Period.ofYears(10), Period.ofYears(30)};
	  private static readonly int FWD3_NB_IRS_NODES = FWD3_IRS_TENORS.Length;

	  /// <summary>
	  /// Data for EUR-EURIBOR6M curve </summary>
	  /* Market values */
	  private static readonly double[] FWD6_MARKET_QUOTES = new double[] {0.00013533281680009178, 0.0031298573232152152, 0.009328861288116275, 0.015219571759282416};
	  private static readonly int FWD6_NB_NODES = FWD3_MARKET_QUOTES.Length;
	  private static readonly string[] FWD6_ID_VALUE = new string[] {"IRS6M_2Y", "IRS6M_5Y", "IRS6M_10Y", "IRS6M_30Y"};
	  /* Nodes */
	  private static readonly CurveNode[] FWD6_NODES = new CurveNode[FWD3_NB_NODES];
	  /* Tenors */
	  private static readonly Period[] FWD6_IRS_TENORS = new Period[] {Period.ofYears(2), Period.ofYears(5), Period.ofYears(10), Period.ofYears(30)};
	  private static readonly int FWD6_NB_IRS_NODES = FWD6_IRS_TENORS.Length;

	  /// <summary>
	  /// All quotes for the curve calibration </summary>
	  private static readonly MarketData ALL_QUOTES;

	  private static readonly DiscountingSwapProductPricer SWAP_PRICER = DiscountingSwapProductPricer.DEFAULT;

	  private static readonly RatesCurveCalibrator CALIBRATOR = RatesCurveCalibrator.of(1e-9, 1e-9, 100);

	  // Constants
	  private const double TOLERANCE_PV = 1.0E-6;

	  /// <summary>
	  /// Test with CurveGroupDefinition </summary>
	  private const string CURVE_GROUP_NAME_STR = "EUR-DSCON-EURIBOR3M-EURIBOR6M";
	  private static readonly CurveGroupName CURVE_GROUP_NAME = CurveGroupName.of(CURVE_GROUP_NAME_STR);
	  private static readonly InterpolatedNodalCurveDefinition DSC_CURVE_DEFN = InterpolatedNodalCurveDefinition.builder().name(DSCON_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(CURVE_DC).interpolator(INTERPOLATOR_LINEAR).extrapolatorLeft(EXTRAPOLATOR_FLAT).extrapolatorRight(EXTRAPOLATOR_FLAT).nodes(DSC_NODES).build();
	  private static readonly InterpolatedNodalCurveDefinition FWD3_CURVE_DEFN = InterpolatedNodalCurveDefinition.builder().name(FWD3_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(CURVE_DC).interpolator(INTERPOLATOR_LINEAR).extrapolatorLeft(EXTRAPOLATOR_FLAT).extrapolatorRight(EXTRAPOLATOR_FLAT).nodes(FWD3_NODES).build();
	  private static readonly InterpolatedNodalCurveDefinition FWD6_CURVE_DEFN = InterpolatedNodalCurveDefinition.builder().name(FWD6_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(CURVE_DC).interpolator(INTERPOLATOR_LINEAR).extrapolatorLeft(EXTRAPOLATOR_FLAT).extrapolatorRight(EXTRAPOLATOR_FLAT).nodes(FWD3_NODES).build();
	  private static readonly RatesCurveGroupDefinition CURVE_GROUP_CONFIG = RatesCurveGroupDefinition.builder().name(CURVE_GROUP_NAME).addCurve(DSC_CURVE_DEFN, EUR, EUR_EONIA).addForwardCurve(FWD3_CURVE_DEFN, EUR_EURIBOR_3M).addForwardCurve(FWD6_CURVE_DEFN, EUR_EURIBOR_6M).build();

	  //-------------------------------------------------------------------------
	  public virtual void calibration_present_value()
	  {
		RatesProvider result = CALIBRATOR.calibrate(CURVE_GROUP_CONFIG, ALL_QUOTES, REF_DATA);

		ImmutableList<CurveDefinition> definitions = CURVE_GROUP_CONFIG.CurveDefinitions;
		// Test PV Dsc
		ImmutableList<CurveNode> dscNodes = definitions.get(0).Nodes;
		IList<ResolvedTrade> dscTrades = new List<ResolvedTrade>();
		for (int i = 0; i < dscNodes.size(); i++)
		{
		  dscTrades.Add(dscNodes.get(i).resolvedTrade(1d, ALL_QUOTES, REF_DATA));
		}
		// OIS
		for (int i = 0; i < DSC_NB_OIS_NODES; i++)
		{
		  MultiCurrencyAmount pvIrs = SWAP_PRICER.presentValue(((ResolvedSwapTrade) dscTrades[i]).Product, result);
		  assertEquals(pvIrs.getAmount(EUR).Amount, 0.0, TOLERANCE_PV);
		}
		// Test PV Fwd3
		ImmutableList<CurveNode> fwd3Nodes = definitions.get(1).Nodes;
		IList<ResolvedTrade> fwd3Trades = new List<ResolvedTrade>();
		for (int i = 0; i < fwd3Nodes.size(); i++)
		{
		  fwd3Trades.Add(fwd3Nodes.get(i).resolvedTrade(1d, ALL_QUOTES, REF_DATA));
		}
		// IRS
		for (int i = 0; i < FWD3_NB_IRS_NODES; i++)
		{
		  MultiCurrencyAmount pvIrs = SWAP_PRICER.presentValue(((ResolvedSwapTrade) fwd3Trades[i]).Product, result);
		  assertEquals(pvIrs.getAmount(EUR).Amount, 0.0, TOLERANCE_PV);
		}
		// Test PV Fwd6
		ImmutableList<CurveNode> fwd6Nodes = definitions.get(2).Nodes;
		IList<ResolvedTrade> fwd6Trades = new List<ResolvedTrade>();
		for (int i = 0; i < fwd6Nodes.size(); i++)
		{
		  fwd6Trades.Add(fwd6Nodes.get(i).resolvedTrade(1d, ALL_QUOTES, REF_DATA));
		}
		// IRS
		for (int i = 0; i < FWD6_NB_IRS_NODES; i++)
		{
		  MultiCurrencyAmount pvIrs = SWAP_PRICER.presentValue(((ResolvedSwapTrade) fwd6Trades[i]).Product, result);
		  assertEquals(pvIrs.getAmount(EUR).Amount, 0.0, TOLERANCE_PV);
		}
	  }

	}

}