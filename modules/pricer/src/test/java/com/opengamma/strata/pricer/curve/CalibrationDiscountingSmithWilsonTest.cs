using System.Collections.Generic;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedIborSwapConventions.GBP_FIXED_6M_LIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using Index = com.opengamma.strata.basics.index.Index;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
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
	using ParameterizedFunctionalCurveDefinition = com.opengamma.strata.market.curve.ParameterizedFunctionalCurveDefinition;
	using FixedIborSwapCurveNode = com.opengamma.strata.market.curve.node.FixedIborSwapCurveNode;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using SmithWilsonCurveFunction = com.opengamma.strata.math.impl.interpolation.SmithWilsonCurveFunction;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;
	using FixedIborSwapTemplate = com.opengamma.strata.product.swap.type.FixedIborSwapTemplate;

	/// <summary>
	/// Curve calibration example with <seealso cref="SmithWilsonCurveFunction"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CalibrationDiscountingSmithWilsonTest
	public class CalibrationDiscountingSmithWilsonTest
	{

	  private static readonly LocalDate VAL_DATE = LocalDate.of(2015, 7, 21);
	  private static readonly DayCount CURVE_DC = ACT_365F;
	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const string SCHEME = "CALIBRATION";

	  /// <summary>
	  /// Curve name </summary>
	  private static readonly CurveName CURVE_NAME = CurveName.of("GBP-ALL-IRS6M");
	  /// <summary>
	  /// Curves associations to currencies and indices. </summary>
	  private static readonly IDictionary<CurveName, Currency> DSC_NAMES = new Dictionary<CurveName, Currency>();
	  private static readonly IDictionary<CurveName, ISet<Index>> IDX_NAMES = new Dictionary<CurveName, ISet<Index>>();
	  private static readonly ISet<Index> IBOR_INDICES = new HashSet<Index>();
	  static CalibrationDiscountingSmithWilsonTest()
	  {
		IBOR_INDICES.Add(GBP_LIBOR_6M);
		DSC_NAMES[CURVE_NAME] = GBP;
		IDX_NAMES[CURVE_NAME] = IBOR_INDICES;
		for (int i = 0; i < FWD6_NB_NODES; i++)
		{
		  ALL_NODES[i] = FixedIborSwapCurveNode.of(FixedIborSwapTemplate.of(Period.ZERO, Tenor.of(FWD6_IRS_TENORS[i]), GBP_FIXED_6M_LIBOR_6M), QuoteId.of(StandardId.of(SCHEME, FWD6_ID_VALUE[i])));
		  NODE_TIMES[i] = CURVE_DC.relativeYearFraction(VAL_DATE, ALL_NODES[i].date(VAL_DATE, REF_DATA));
		}
		ImmutableMarketDataBuilder builder = ImmutableMarketData.builder(VAL_DATE);
		for (int i = 0; i < FWD6_NB_NODES; i++)
		{
		  builder.addValue(QuoteId.of(StandardId.of(SCHEME, FWD6_ID_VALUE[i])), FWD6_MARKET_QUOTES[i]);
		}
		ALL_QUOTES = builder.build();
		IList<CurveNode[]> groupNodes = new List<CurveNode[]>();
		groupNodes.Add(ALL_NODES);
		CURVES_NODES.Add(groupNodes);
		IList<CurveMetadata> groupMetadata = new List<CurveMetadata>();
		groupMetadata.Add(DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.DISCOUNT_FACTOR).dayCount(CURVE_DC).build());
		CURVES_METADATA.Add(groupMetadata);
	  }

	  /// <summary>
	  /// Market values </summary>
	  private static readonly double[] FWD6_MARKET_QUOTES = new double[] {0.0273403667327403, 0.0327205345299401, 0.0336112121443886, 0.0346854377006694, 0.0395043823351044, 0.0425511326946310, 0.0475939564387996};
	  private static readonly int FWD6_NB_NODES = FWD6_MARKET_QUOTES.Length;
	  private static readonly string[] FWD6_ID_VALUE = new string[] {"IRS1Y", "IRS3Y", "IRS5Y", "IRS7Y", "IRS10Y", "IRS15Y", "IRS20Y", "IRS30Y"};
	  /// <summary>
	  /// Nodes for the Fwd 3M GBP curve </summary>
	  private static readonly CurveNode[] ALL_NODES = new CurveNode[FWD6_NB_NODES];
	  private static readonly double[] NODE_TIMES = new double[FWD6_NB_NODES];
	  /// <summary>
	  /// Tenors for the Fwd 3M GBP swaps </summary>
	  private static readonly Period[] FWD6_IRS_TENORS = new Period[] {Period.ofYears(3), Period.ofYears(5), Period.ofYears(7), Period.ofYears(10), Period.ofYears(15), Period.ofYears(20), Period.ofYears(30)};

	  /// <summary>
	  /// All quotes for the curve calibration </summary>
	  private static readonly MarketData ALL_QUOTES;

	  /// <summary>
	  /// All nodes by groups. </summary>
	  private static readonly IList<IList<CurveNode[]>> CURVES_NODES = new List<IList<CurveNode[]>>();

	  /// <summary>
	  /// All metadata by groups </summary>
	  private static readonly IList<IList<CurveMetadata>> CURVES_METADATA = new List<IList<CurveMetadata>>();
	  private static readonly DiscountingSwapProductPricer SWAP_PRICER = DiscountingSwapProductPricer.DEFAULT;
	  private static readonly RatesCurveCalibrator CALIBRATOR = RatesCurveCalibrator.of(1e-9, 1e-9, 100);

	  /// <summary>
	  /// Test with CurveGroupDefinition </summary>
	  private const string CURVE_GROUP_NAME_STR = "GBP-SINGLE-CURVE";
	  private static readonly CurveGroupName CURVE_GROUP_NAME = CurveGroupName.of(CURVE_GROUP_NAME_STR);
	  private static readonly SmithWilsonCurveFunction SW_CURVE = SmithWilsonCurveFunction.DEFAULT;
	  private const double ALPHA = 0.186649;
	  private static readonly System.Func<DoubleArray, double, double> VALUE_FUNCTION = (DoubleArray t, double? u) =>
	  {
  return SW_CURVE.value(u.Value, ALPHA, DoubleArray.copyOf(NODE_TIMES), t);
	  };
	  private static readonly System.Func<DoubleArray, double, double> DERIVATIVE_FUNCTION = (DoubleArray t, double? u) =>
	  {
	  return SW_CURVE.firstDerivative(u.Value, ALPHA, DoubleArray.copyOf(NODE_TIMES), t);
	  };
	  private static readonly System.Func<DoubleArray, double, DoubleArray> SENSI_FUNCTION = (DoubleArray t, double? u) =>
	  {
	  return SW_CURVE.parameterSensitivity(u.Value, ALPHA, DoubleArray.copyOf(NODE_TIMES));
	  };
	  private static readonly ParameterizedFunctionalCurveDefinition CURVE_DEFN = ParameterizedFunctionalCurveDefinition.builder().name(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.DISCOUNT_FACTOR).dayCount(CURVE_DC).initialGuess(DoubleArray.filled(FWD6_NB_NODES, 0d).toList()).valueFunction(VALUE_FUNCTION).derivativeFunction(DERIVATIVE_FUNCTION).sensitivityFunction(SENSI_FUNCTION).nodes(ALL_NODES).build();
	  private static readonly RatesCurveGroupDefinition CURVE_GROUP_DEFN = RatesCurveGroupDefinition.builder().name(CURVE_GROUP_NAME).addCurve(CURVE_DEFN, GBP, GBP_LIBOR_6M).build();
	  /// <summary>
	  /// expected discount factor values (Source: EIOPA - European Insurance and Occupational Pensions Authority) </summary>
	  private static readonly DoubleArray DSC_EXP = DoubleArray.ofUnsafe(new double[] {1d, 0.9784596573108600, 0.9540501162514210, 0.9242332228570250, 0.8885036235533640, 0.8529525938462010, 0.8225785258044620, 0.7955993406787280, 0.7693449840658110, 0.7422694544490090, 0.7132193803280200, 0.6816197350030370, 0.6485921254081380, 0.6153797190254940, 0.5829431201155780, 0.5520276608703360, 0.5230330826657420, 0.4955678268458750, 0.4691264946001880, 0.4432705503035050, 0.4176082714808270, 0.3919343348393330, 0.3666298787856500, 0.3421032317280810, 0.3186497485892540, 0.2964788703398110, 0.2757355981614060, 0.2565175652984340, 0.2388886512264410, 0.2228898936657480, 0.2085483042428570, 0.1958299034344640, 0.1845026251379590, 0.1743314425719990, 0.1651272569309040, 0.1567376199945500, 0.1490393434620470, 0.1419326107937100, 0.1353362856089140, 0.1291841730078400, 0.1234220398105030, 0.1180052392228150, 0.1128968169062150, 0.1080660004841850, 0.1034869944717180, 0.0991380185020832, 0.0950005393771045, 0.0910586575419039, 0.0872986166075841, 0.0837084109337509, 0.0802774713699451, 0.0769964133060395, 0.0738568344075488, 0.0708511519806606, 0.0679724719574663, 0.0652144831209293, 0.0625713714864361, 0.0600377507899026, 0.0576086058551821, 0.0552792462687688, 0.0530452683116555, 0.0509025235138388, 0.0488470925280131, 0.0468752632826767, 0.0449835125849284, 0.0431684905105571, 0.0414270070523355, 0.0397560206036511, 0.0381526279392532, 0.0366140554223544, 0.0351376512211047, 0.0337208783603299, 0.0323613084686289, 0.0310566161082008, 0.0298045735965500, 0.0286030462465983, 0.0274499879656266, 0.0263434371645669, 0.0252815129380532, 0.0242624114827481, 0.0232844027271681, 0.0223458271508077, 0.0214450927740423, 0.0205806723032541, 0.0197511004180213, 0.0189549711891452, 0.0181909356178633, 0.0174576992878771, 0.0167540201228721, 0.0160787062430671, 0.0154306139150397, 0.0148086455896693, 0.0142117480235254, 0.0136389104794540, 0.0130891630024586, 0.0125615747672780, 0.0120552524943231, 0.0115693389308603, 0.0111030113945241, 0.0106554803764249, 0.0102259882012646, 0.0098138077420219, 0.0094182411868912, 0.0090386188562798, 0.0086742980677716, 0.0083246620470656, 0.0079891188829930, 0.0076671005247963, 0.0073580618199396, 0.0070614795907931, 0.0067768517486054, 0.0065036964432469, 0.0062415512472709, 0.0059899723728984, 0.0057485339205936, 0.0055168271579503, 0.0052944598276626, 0.0050810554834040, 0.0048762528524868, 0.0046797052242205, 0.0044910798629311});
	  /// <summary>
	  /// Constants </summary>
	  private const double TOLERANCE_PV = 1.0E-6;
	  private const double ONE_BP = 1.0e-4;
	  private const double ONE_PC = 1.0e-2;

	  //-------------------------------------------------------------------------
	  public virtual void calibration_test()
	  {
		RatesProvider result2 = CALIBRATOR.calibrate(CURVE_GROUP_DEFN, ALL_QUOTES, REF_DATA);
		// pv test
		CurveNode[] fwd3Nodes = CURVES_NODES[0][0];
		IList<ResolvedTrade> fwd3Trades = new List<ResolvedTrade>();
		for (int i = 0; i < fwd3Nodes.Length; i++)
		{
		  fwd3Trades.Add(fwd3Nodes[i].resolvedTrade(1d, ALL_QUOTES, REF_DATA));
		}
		for (int i = 0; i < FWD6_NB_NODES; i++)
		{
		  MultiCurrencyAmount pvIrs2 = SWAP_PRICER.presentValue(((ResolvedSwapTrade) fwd3Trades[i]).Product, result2);
		  assertEquals(pvIrs2.getAmount(GBP).Amount, 0.0, TOLERANCE_PV);
		}
		// regression test for curve
		DiscountFactors dsc = result2.discountFactors(GBP);
		double prevDsc = 0d;
		for (int i = 0; i < 121; ++i)
		{
		  double time = ((double) i);
		  double curDsc = dsc.discountFactor(time);
		  if (i > 59)
		  {
			double fwd = prevDsc / curDsc - 1d;
			assertEquals(fwd, 0.042, 2d * ONE_BP);
		  }
		  assertEquals(curDsc, DSC_EXP.get(i), ONE_PC);
		  prevDsc = curDsc;
		}
	  }

	}

}