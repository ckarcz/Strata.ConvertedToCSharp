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


	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using Index = com.opengamma.strata.basics.index.Index;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using ImmutableMarketDataBuilder = com.opengamma.strata.data.ImmutableMarketDataBuilder;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ValueType = com.opengamma.strata.market.ValueType;
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
	using FraCurveNode = com.opengamma.strata.market.curve.node.FraCurveNode;
	using IborFixingDepositCurveNode = com.opengamma.strata.market.curve.node.IborFixingDepositCurveNode;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using IborFixingDepositTemplate = com.opengamma.strata.product.deposit.type.IborFixingDepositTemplate;
	using FraTemplate = com.opengamma.strata.product.fra.type.FraTemplate;
	using FixedIborSwapTemplate = com.opengamma.strata.product.swap.type.FixedIborSwapTemplate;
	using FixedOvernightSwapTemplate = com.opengamma.strata.product.swap.type.FixedOvernightSwapTemplate;

	public class CalibrationEurStandard
	{

	  private static readonly DayCount CURVE_DC = ACT_365F;

	  // reference data
	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private const string SCHEME = "CALIBRATION";

	  /// <summary>
	  /// Curve names </summary>
	  private const string DSCON_NAME = "EUR_EONIA_EOD";
	  public static readonly CurveName DSCON_CURVE_NAME = CurveName.of(DSCON_NAME);
	  private const string FWD3_NAME = "EUR_EURIBOR_3M";
	  public static readonly CurveName FWD3_CURVE_NAME = CurveName.of(FWD3_NAME);
	  private const string FWD6_NAME = "EUR_EURIBOR_6M";
	  public static readonly CurveName FWD6_CURVE_NAME = CurveName.of(FWD6_NAME);
	  private const string CURVE_GROUP_NAME_STR = "EUR-DSCON-EURIBOR3M-EURIBOR6M";
	  private static readonly CurveGroupName CURVE_GROUP_NAME = CurveGroupName.of(CURVE_GROUP_NAME_STR);
	  /// <summary>
	  /// Curves associations to currencies and indices. </summary>
	  private static readonly IDictionary<CurveName, Currency> DSC_NAMES = new Dictionary<CurveName, Currency>();
	  private static readonly IDictionary<CurveName, ISet<Index>> IDX_NAMES = new Dictionary<CurveName, ISet<Index>>();
	  static CalibrationEurStandard()
	  {
		DSC_NAMES[DSCON_CURVE_NAME] = EUR;
		ISet<Index> eurEoniaSet = new HashSet<Index>();
		eurEoniaSet.Add(EUR_EONIA);
		IDX_NAMES[DSCON_CURVE_NAME] = eurEoniaSet;
		ISet<Index> eurEuribor3Set = new HashSet<Index>();
		eurEuribor3Set.Add(EUR_EURIBOR_3M);
		IDX_NAMES[FWD3_CURVE_NAME] = eurEuribor3Set;
		ISet<Index> eurEuriabor6Set = new HashSet<Index>();
		eurEuriabor6Set.Add(EUR_EURIBOR_6M);
		IDX_NAMES[FWD6_CURVE_NAME] = eurEuriabor6Set;
	  }
	  private static readonly CurveInterpolator INTERPOLATOR_LINEAR = CurveInterpolators.LINEAR;
	  private static readonly CurveExtrapolator EXTRAPOLATOR_FLAT = CurveExtrapolators.FLAT;

	  private static readonly RatesCurveCalibrator CALIBRATOR = RatesCurveCalibrator.of(1e-9, 1e-9, 100);

	  public static RatesProvider calibrateEurStandard(LocalDate valuationDate, double[] dscOisQuotes, Period[] dscOisTenors, double fwd3FixingQuote, double[] fwd3FraQuotes, double[] fwd3IrsQuotes, Period[] fwd3FraTenors, Period[] fwd3IrsTenors, double fwd6FixingQuote, double[] fwd6FraQuotes, double[] fwd6IrsQuotes, Period[] fwd6FraTenors, Period[] fwd6IrsTenors)
	  {
		/* Curve Discounting/EUR-EONIA */
		string[] dscIdValues = CalibrationEurStandard.dscIdValues(dscOisTenors);
		/* Curve EUR-EURIBOR-3M */
		double[] fwd3MarketQuotes = fwdMarketQuotes(fwd3FixingQuote, fwd3FraQuotes, fwd3IrsQuotes);
		string[] fwd3IdValues = fwdIdValue(3, fwd3FixingQuote, fwd3FraQuotes, fwd3IrsQuotes, fwd3FraTenors, fwd3IrsTenors);
		/* Curve EUR-EURIBOR-6M */
		double[] fwd6MarketQuotes = fwdMarketQuotes(fwd6FixingQuote, fwd6FraQuotes, fwd6IrsQuotes);
		string[] fwd6IdValues = fwdIdValue(6, fwd6FixingQuote, fwd6FraQuotes, fwd6IrsQuotes, fwd6FraTenors, fwd6IrsTenors);
		/* All quotes for the curve calibration */
		MarketData allQuotes = CalibrationEurStandard.allQuotes(valuationDate, dscOisQuotes, dscIdValues, fwd3MarketQuotes, fwd3IdValues, fwd6MarketQuotes, fwd6IdValues);
		/* All nodes by groups. */
		RatesCurveGroupDefinition config = CalibrationEurStandard.config(dscOisTenors, dscIdValues, fwd3FraTenors, fwd3IrsTenors, fwd3IdValues, fwd6FraTenors, fwd6IrsTenors, fwd6IdValues);
		/* Results */
		return CALIBRATOR.calibrate(config, allQuotes, REF_DATA);
	  }

	  public static string[] dscIdValues(Period[] dscOisTenors)
	  {
		string[] dscIdValues = new string[dscOisTenors.Length];
		for (int i = 0; i < dscOisTenors.Length; i++)
		{
		  dscIdValues[i] = "OIS" + dscOisTenors[i].ToString();
		}
		return dscIdValues;
	  }

	  public static string[] fwdIdValue(int tenor, double fwdFixingQuote, double[] fwdFraQuotes, double[] fwdIrsQuotes, Period[] fwdFraTenors, Period[] fwdIrsTenors)
	  {
		string[] fwdIdValue = new string[1 + fwdFraQuotes.Length + fwdIrsQuotes.Length];
		fwdIdValue[0] = "FIXING" + tenor + "M";
		for (int i = 0; i < fwdFraQuotes.Length; i++)
		{
		  fwdIdValue[i + 1] = "FRA" + fwdFraTenors[i].ToString() + "x" + fwdFraTenors[i].plusMonths(tenor).ToString();
		}
		for (int i = 0; i < fwdIrsQuotes.Length; i++)
		{
		  fwdIdValue[i + 1 + fwdFraQuotes.Length] = "IRS" + tenor + "M-" + fwdIrsTenors[i].ToString();
		}
		return fwdIdValue;
	  }

	  public static double[] fwdMarketQuotes(double fwdFixingQuote, double[] fwdFraQuotes, double[] fwdIrsQuotes)
	  {
		int fwdNbFraNodes = fwdFraQuotes.Length;
		int fwdNbIrsNodes = fwdIrsQuotes.Length;
		int fwdNbNodes = 1 + fwdNbFraNodes + fwdNbIrsNodes;
		double[] fwdMarketQuotes = new double[fwdNbNodes];
		fwdMarketQuotes[0] = fwdFixingQuote;
		Array.Copy(fwdFraQuotes, 0, fwdMarketQuotes, 1, fwdNbFraNodes);
		Array.Copy(fwdIrsQuotes, 0, fwdMarketQuotes, 1 + fwdNbFraNodes, fwdNbIrsNodes);
		return fwdMarketQuotes;
	  }

	  public static RatesCurveGroupDefinition config(Period[] dscOisTenors, string[] dscIdValues, Period[] fwd3FraTenors, Period[] fwd3IrsTenors, string[] fwd3IdValues, Period[] fwd6FraTenors, Period[] fwd6IrsTenors, string[] fwd6IdValues)
	  {
		CurveNode[] dscNodes = new CurveNode[dscOisTenors.Length];
		for (int i = 0; i < dscOisTenors.Length; i++)
		{
		  dscNodes[i] = FixedOvernightSwapCurveNode.of(FixedOvernightSwapTemplate.of(Period.ZERO, Tenor.of(dscOisTenors[i]), EUR_FIXED_1Y_EONIA_OIS), QuoteId.of(StandardId.of(SCHEME, dscIdValues[i])));
		}
		CurveNode[] fwd3Nodes = new CurveNode[fwd3IdValues.Length];
		fwd3Nodes[0] = IborFixingDepositCurveNode.of(IborFixingDepositTemplate.of(EUR_EURIBOR_3M), QuoteId.of(StandardId.of(SCHEME, fwd3IdValues[0])));
		for (int i = 0; i < fwd3FraTenors.Length; i++)
		{
		  fwd3Nodes[i + 1] = FraCurveNode.of(FraTemplate.of(fwd3FraTenors[i], EUR_EURIBOR_3M), QuoteId.of(StandardId.of(SCHEME, fwd3IdValues[i + 1])));
		}
		for (int i = 0; i < fwd3IrsTenors.Length; i++)
		{
		  fwd3Nodes[i + 1 + fwd3FraTenors.Length] = FixedIborSwapCurveNode.of(FixedIborSwapTemplate.of(Period.ZERO, Tenor.of(fwd3IrsTenors[i]), EUR_FIXED_1Y_EURIBOR_3M), QuoteId.of(StandardId.of(SCHEME, fwd3IdValues[i + 1 + fwd3FraTenors.Length])));
		}
		CurveNode[] fwd6Nodes = new CurveNode[fwd6IdValues.Length];
		fwd6Nodes[0] = IborFixingDepositCurveNode.of(IborFixingDepositTemplate.of(EUR_EURIBOR_6M), QuoteId.of(StandardId.of(SCHEME, fwd6IdValues[0])));
		for (int i = 0; i < fwd6FraTenors.Length; i++)
		{
		  fwd6Nodes[i + 1] = FraCurveNode.of(FraTemplate.of(fwd6FraTenors[i], EUR_EURIBOR_6M), QuoteId.of(StandardId.of(SCHEME, fwd6IdValues[i + 1])));
		}
		for (int i = 0; i < fwd6IrsTenors.Length; i++)
		{
		  fwd6Nodes[i + 1 + fwd6FraTenors.Length] = FixedIborSwapCurveNode.of(FixedIborSwapTemplate.of(Period.ZERO, Tenor.of(fwd6IrsTenors[i]), EUR_FIXED_1Y_EURIBOR_6M), QuoteId.of(StandardId.of(SCHEME, fwd6IdValues[i + 1 + fwd6FraTenors.Length])));
		}
		InterpolatedNodalCurveDefinition DSC_CURVE_DEFN = InterpolatedNodalCurveDefinition.builder().name(DSCON_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(CURVE_DC).interpolator(INTERPOLATOR_LINEAR).extrapolatorLeft(EXTRAPOLATOR_FLAT).extrapolatorRight(EXTRAPOLATOR_FLAT).nodes(dscNodes).build();
		InterpolatedNodalCurveDefinition FWD3_CURVE_DEFN = InterpolatedNodalCurveDefinition.builder().name(FWD3_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(CURVE_DC).interpolator(INTERPOLATOR_LINEAR).extrapolatorLeft(EXTRAPOLATOR_FLAT).extrapolatorRight(EXTRAPOLATOR_FLAT).nodes(fwd3Nodes).build();
		InterpolatedNodalCurveDefinition FWD6_CURVE_DEFN = InterpolatedNodalCurveDefinition.builder().name(FWD6_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(CURVE_DC).interpolator(INTERPOLATOR_LINEAR).extrapolatorLeft(EXTRAPOLATOR_FLAT).extrapolatorRight(EXTRAPOLATOR_FLAT).nodes(fwd6Nodes).build();
		return RatesCurveGroupDefinition.builder().name(CURVE_GROUP_NAME).addCurve(DSC_CURVE_DEFN, EUR, EUR_EONIA).addForwardCurve(FWD3_CURVE_DEFN, EUR_EURIBOR_3M).addForwardCurve(FWD6_CURVE_DEFN, EUR_EURIBOR_6M).build();
	  }

	  public static MarketData allQuotes(LocalDate valuationDate, double[] dscOisQuotes, string[] dscIdValues, double[] fwd3MarketQuotes, string[] fwd3IdValue, double[] fwd6MarketQuotes, string[] fwd6IdValue)
	  {
		/* All quotes for the curve calibration */
		ImmutableMarketDataBuilder builder = ImmutableMarketData.builder(valuationDate);
		for (int i = 0; i < dscOisQuotes.Length; i++)
		{
		  builder.addValue(QuoteId.of(StandardId.of(SCHEME, dscIdValues[i])), dscOisQuotes[i]);
		}
		for (int i = 0; i < fwd3MarketQuotes.Length; i++)
		{
		  builder.addValue(QuoteId.of(StandardId.of(SCHEME, fwd3IdValue[i])), fwd3MarketQuotes[i]);
		}
		for (int i = 0; i < fwd6MarketQuotes.Length; i++)
		{
		  builder.addValue(QuoteId.of(StandardId.of(SCHEME, fwd6IdValue[i])), fwd6MarketQuotes[i]);
		}
		return builder.build();
	  }

	}

}