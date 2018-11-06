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
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.deposit.type.TermDepositConventions.USD_SHORT_DEPOSIT_T0;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.deposit.type.TermDepositConventions.USD_SHORT_DEPOSIT_T1;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.fx.type.FxSwapConventions.EUR_USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedOvernightSwapConventions.USD_FIXED_1Y_FED_FUND_OIS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.XCcyIborIborSwapConventions.EUR_EURIBOR_3M_USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using FxRateId = com.opengamma.strata.data.FxRateId;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using ImmutableMarketDataBuilder = com.opengamma.strata.data.ImmutableMarketDataBuilder;
	using MarketDataFxRateProvider = com.opengamma.strata.data.MarketDataFxRateProvider;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
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
	using FxSwapCurveNode = com.opengamma.strata.market.curve.node.FxSwapCurveNode;
	using IborFixingDepositCurveNode = com.opengamma.strata.market.curve.node.IborFixingDepositCurveNode;
	using TermDepositCurveNode = com.opengamma.strata.market.curve.node.TermDepositCurveNode;
	using XCcyIborIborSwapCurveNode = com.opengamma.strata.market.curve.node.XCcyIborIborSwapCurveNode;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using DiscountingIborFixingDepositProductPricer = com.opengamma.strata.pricer.deposit.DiscountingIborFixingDepositProductPricer;
	using DiscountingTermDepositProductPricer = com.opengamma.strata.pricer.deposit.DiscountingTermDepositProductPricer;
	using DiscountingFraTradePricer = com.opengamma.strata.pricer.fra.DiscountingFraTradePricer;
	using DiscountingFxSwapProductPricer = com.opengamma.strata.pricer.fx.DiscountingFxSwapProductPricer;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using ResolvedIborFixingDepositTrade = com.opengamma.strata.product.deposit.ResolvedIborFixingDepositTrade;
	using ResolvedTermDepositTrade = com.opengamma.strata.product.deposit.ResolvedTermDepositTrade;
	using IborFixingDepositTemplate = com.opengamma.strata.product.deposit.type.IborFixingDepositTemplate;
	using TermDepositTemplate = com.opengamma.strata.product.deposit.type.TermDepositTemplate;
	using ResolvedFraTrade = com.opengamma.strata.product.fra.ResolvedFraTrade;
	using FraTemplate = com.opengamma.strata.product.fra.type.FraTemplate;
	using ResolvedFxSwapTrade = com.opengamma.strata.product.fx.ResolvedFxSwapTrade;
	using FxSwapTemplate = com.opengamma.strata.product.fx.type.FxSwapTemplate;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;
	using FixedIborSwapTemplate = com.opengamma.strata.product.swap.type.FixedIborSwapTemplate;
	using FixedOvernightSwapTemplate = com.opengamma.strata.product.swap.type.FixedOvernightSwapTemplate;
	using XCcyIborIborSwapTemplate = com.opengamma.strata.product.swap.type.XCcyIborIborSwapTemplate;

	/// <summary>
	/// Test for curve calibration in USD and EUR.
	/// The USD curve is obtained by OIS and the EUR one by FX Swaps from USD.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CalibrationZeroRateUsdOisIrsEurFxXCcyIrsTest
	public class CalibrationZeroRateUsdOisIrsEurFxXCcyIrsTest
	{

	  private static readonly LocalDate VAL_DATE = LocalDate.of(2015, 11, 2);

	  private static readonly CurveInterpolator INTERPOLATOR_LINEAR = CurveInterpolators.LINEAR;
	  private static readonly CurveExtrapolator EXTRAPOLATOR_FLAT = CurveExtrapolators.FLAT;
	  private static readonly DayCount CURVE_DC = ACT_365F;

	  // reference data
	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private const string SCHEME = "CALIBRATION";

	  /// <summary>
	  /// Curve names </summary>
	  private const string USD_DSCON_STR = "USD-DSCON-OIS";
	  private static readonly CurveName USD_DSCON_CURVE_NAME = CurveName.of(USD_DSCON_STR);
	  private const string USD_FWD3_NAME = "USD-LIBOR3M-FRAIRS";
	  private static readonly CurveName USD_FWD3_CURVE_NAME = CurveName.of(USD_FWD3_NAME);
	  private const string EUR_DSC_STR = "EUR-DSC-FXXCCY";
	  private static readonly CurveName EUR_DSC_CURVE_NAME = CurveName.of(EUR_DSC_STR);
	  private const string EUR_FWD3_NAME = "EUR-EURIBOR3M-FRAIRS";
	  public static readonly CurveName EUR_FWD3_CURVE_NAME = CurveName.of(EUR_FWD3_NAME);

	  /// <summary>
	  /// Data FX * </summary>
	  private const double FX_RATE_EUR_USD = 1.10;
	  private const string EUR_USD_ID_VALUE = "EUR-USD";
	  /// <summary>
	  /// Data for USD-DSCON curve </summary>
	  /* Market values */
	  private static readonly double[] USD_DSC_MARKET_QUOTES = new double[] {0.0016, 0.0022, 0.0013, 0.0016, 0.0020, 0.0026, 0.0033, 0.0039, 0.0053, 0.0066, 0.0090, 0.0111, 0.0128, 0.0143, 0.0156, 0.0167, 0.0175, 0.0183};
	  private static readonly int USD_DSC_NB_NODES = USD_DSC_MARKET_QUOTES.Length;
	  private static readonly string[] USD_DSC_ID_VALUE = new string[] {"USD-ON", "USD-TN", "USD-OIS-1M", "USD-OIS-2M", "USD-OIS-3M", "USD-OIS-6M", "USD-OIS-9M", "USD-OIS-1Y", "USD-OIS-18M", "USD-OIS-2Y", "USD-OIS-3Y", "USD-OIS-4Y", "USD-OIS-5Y", "USD-OIS-6Y", "USD-OIS-7Y", "USD-OIS-8Y", "USD-OIS-9Y", "USD-OIS-10Y"};
	  /* Nodes */
	  private static readonly CurveNode[] USD_DSC_NODES = new CurveNode[USD_DSC_NB_NODES];
	  /* Tenors */
	  private static readonly Period[] USD_DSC_OIS_TENORS = new Period[] {Period.ofMonths(1), Period.ofMonths(2), Period.ofMonths(3), Period.ofMonths(6), Period.ofMonths(9), Period.ofYears(1), Period.ofMonths(18), Period.ofYears(2), Period.ofYears(3), Period.ofYears(4), Period.ofYears(5), Period.ofYears(6), Period.ofYears(7), Period.ofYears(8), Period.ofYears(9), Period.ofYears(10)};
	  private static readonly int USD_DSC_NB_OIS_NODES = USD_DSC_OIS_TENORS.Length;
	  static CalibrationZeroRateUsdOisIrsEurFxXCcyIrsTest()
	  {
		USD_DSC_NODES[0] = TermDepositCurveNode.of(TermDepositTemplate.of(Period.ofDays(1), USD_SHORT_DEPOSIT_T0), QuoteId.of(StandardId.of(SCHEME, USD_DSC_ID_VALUE[0])));
		USD_DSC_NODES[1] = TermDepositCurveNode.of(TermDepositTemplate.of(Period.ofDays(1), USD_SHORT_DEPOSIT_T1), QuoteId.of(StandardId.of(SCHEME, USD_DSC_ID_VALUE[1])));
		for (int i = 0; i < USD_DSC_NB_OIS_NODES; i++)
		{
		  USD_DSC_NODES[2 + i] = FixedOvernightSwapCurveNode.of(FixedOvernightSwapTemplate.of(Period.ZERO, Tenor.of(USD_DSC_OIS_TENORS[i]), USD_FIXED_1Y_FED_FUND_OIS), QuoteId.of(StandardId.of(SCHEME, USD_DSC_ID_VALUE[2 + i])));
		}
		USD_FWD3_NODES[0] = IborFixingDepositCurveNode.of(IborFixingDepositTemplate.of(USD_LIBOR_3M), QuoteId.of(StandardId.of(SCHEME, USD_FWD3_ID_VALUE[0])));
		for (int i = 0; i < USD_FWD3_NB_FRA_NODES; i++)
		{
		  USD_FWD3_NODES[i + 1] = FraCurveNode.of(FraTemplate.of(USD_FWD3_FRA_TENORS[i], USD_LIBOR_3M), QuoteId.of(StandardId.of(SCHEME, USD_FWD3_ID_VALUE[i + 1])));
		}
		for (int i = 0; i < USD_FWD3_NB_IRS_NODES; i++)
		{
		  USD_FWD3_NODES[i + 1 + USD_FWD3_NB_FRA_NODES] = FixedIborSwapCurveNode.of(FixedIborSwapTemplate.of(Period.ZERO, Tenor.of(USD_FWD3_IRS_TENORS[i]), USD_FIXED_6M_LIBOR_3M), QuoteId.of(StandardId.of(SCHEME, USD_FWD3_ID_VALUE[i + 1 + USD_FWD3_NB_FRA_NODES])));
		}
		for (int i = 0; i < EUR_DSC_NB_FX_NODES; i++)
		{
		  EUR_DSC_NODES[i] = FxSwapCurveNode.of(FxSwapTemplate.of(EUR_DSC_FX_TENORS[i], EUR_USD), QuoteId.of(StandardId.of(SCHEME, EUR_DSC_ID_VALUE[i])));
		}
		for (int i = 0; i < EUR_DSC_NB_XCCY_NODES; i++)
		{
		  EUR_DSC_NODES[EUR_DSC_NB_FX_NODES + i] = XCcyIborIborSwapCurveNode.of(XCcyIborIborSwapTemplate.of(Tenor.of(EUR_DSC_XCCY_TENORS[i]), EUR_EURIBOR_3M_USD_LIBOR_3M), QuoteId.of(StandardId.of(SCHEME, EUR_DSC_ID_VALUE[EUR_DSC_NB_FX_NODES + i])));
		}
		EUR_FWD3_NODES[0] = IborFixingDepositCurveNode.of(IborFixingDepositTemplate.of(EUR_EURIBOR_3M), QuoteId.of(StandardId.of(SCHEME, EUR_FWD3_ID_VALUE[0])));
		for (int i = 0; i < EUR_FWD3_NB_FRA_NODES; i++)
		{
		  EUR_FWD3_NODES[i + 1] = FraCurveNode.of(FraTemplate.of(EUR_FWD3_FRA_TENORS[i], EUR_EURIBOR_3M), QuoteId.of(StandardId.of(SCHEME, EUR_FWD3_ID_VALUE[i + 1])));
		}
		for (int i = 0; i < EUR_FWD3_NB_IRS_NODES; i++)
		{
		  EUR_FWD3_NODES[i + 1 + EUR_FWD3_NB_FRA_NODES] = FixedIborSwapCurveNode.of(FixedIborSwapTemplate.of(Period.ZERO, Tenor.of(EUR_FWD3_IRS_TENORS[i]), EUR_FIXED_1Y_EURIBOR_3M), QuoteId.of(StandardId.of(SCHEME, EUR_FWD3_ID_VALUE[i + 1 + EUR_FWD3_NB_FRA_NODES])));
		}
		ImmutableMarketDataBuilder builder = ImmutableMarketData.builder(VAL_DATE);
		for (int i = 0; i < USD_DSC_NB_NODES; i++)
		{
		  builder.addValue(QuoteId.of(StandardId.of(SCHEME, USD_DSC_ID_VALUE[i])), USD_DSC_MARKET_QUOTES[i]);
		}
		for (int i = 0; i < USD_FWD3_NB_NODES; i++)
		{
		  builder.addValue(QuoteId.of(StandardId.of(SCHEME, USD_FWD3_ID_VALUE[i])), USD_FWD3_MARKET_QUOTES[i]);
		}
		for (int i = 0; i < EUR_DSC_NB_NODES; i++)
		{
		  builder.addValue(QuoteId.of(StandardId.of(SCHEME, EUR_DSC_ID_VALUE[i])), EUR_DSC_MARKET_QUOTES[i]);
		}
		for (int i = 0; i < EUR_FWD3_NB_NODES; i++)
		{
		  builder.addValue(QuoteId.of(StandardId.of(SCHEME, EUR_FWD3_ID_VALUE[i])), EUR_FWD3_MARKET_QUOTES[i]);
		}
		builder.addValue(QuoteId.of(StandardId.of(SCHEME, EUR_USD_ID_VALUE)), FX_RATE_EUR_USD);
		builder.addValue(FxRateId.of(EUR, USD), FxRate.of(EUR, USD, FX_RATE_EUR_USD));
		ALL_QUOTES = builder.build();
	  }
	  /// <summary>
	  /// Data for USD-LIBOR3M curve </summary>
	  /* Market values */
	  private static readonly double[] USD_FWD3_MARKET_QUOTES = new double[] {0.003341, 0.0049, 0.0063, 0.0057, 0.0087, 0.0112, 0.0134, 0.0152, 0.0181, 0.0209};
	  private static readonly int USD_FWD3_NB_NODES = USD_FWD3_MARKET_QUOTES.Length;
	  private static readonly string[] USD_FWD3_ID_VALUE = new string[] {"USD-Fixing-3M", "USD-FRA3Mx6M", "USD-FRA6Mx9M", "USD-IRS3M-1Y", "USD-IRS3M-2Y", "USD-IRS3M-3Y", "USD-IRS3M-4Y", "USD-IRS3M-5Y", "USD-IRS3M-7Y", "USD-IRS3M-10Y"};
	  /* Nodes */
	  private static readonly CurveNode[] USD_FWD3_NODES = new CurveNode[USD_FWD3_NB_NODES];
	  /* Tenors */
	  private static readonly Period[] USD_FWD3_FRA_TENORS = new Period[] {Period.ofMonths(3), Period.ofMonths(6)};
	  private static readonly int USD_FWD3_NB_FRA_NODES = USD_FWD3_FRA_TENORS.Length;
	  private static readonly Period[] USD_FWD3_IRS_TENORS = new Period[] {Period.ofYears(1), Period.ofYears(2), Period.ofYears(3), Period.ofYears(4), Period.ofYears(5), Period.ofYears(7), Period.ofYears(10)};
	  private static readonly int USD_FWD3_NB_IRS_NODES = USD_FWD3_IRS_TENORS.Length;
	  /// <summary>
	  /// Data for EUR-DSC curve </summary>
	  /* Market values */
	  private static readonly double[] EUR_DSC_MARKET_QUOTES = new double[] {0.0004, 0.0012, 0.0019, 0.0043, 0.0074, 0.0109, -0.0034, -0.0036, -0.0038, -0.0039, -0.0040, -0.0039};
	  private static readonly int EUR_DSC_NB_NODES = EUR_DSC_MARKET_QUOTES.Length;
	  private static readonly string[] EUR_DSC_ID_VALUE = new string[] {"EUR-USD-FX-1M", "EUR-USD-FX-2M", "EUR-USD-FX-3M", "EUR-USD-FX-6M", "EUR-USD-FX-9M", "EUR-USD-FX-1Y", "EUR-USD-XCCY-2Y", "EUR-USD-XCCY-3Y", "EUR-USD-XCCY-4Y", "EUR-USD-XCCY-5Y", "EUR-USD-XCCY-7Y", "EUR-USD-XCCY-10Y"};
	  /* Nodes */
	  private static readonly CurveNode[] EUR_DSC_NODES = new CurveNode[EUR_DSC_NB_NODES];
	  /* Tenors */
	  private static readonly Period[] EUR_DSC_FX_TENORS = new Period[] {Period.ofMonths(1), Period.ofMonths(2), Period.ofMonths(3), Period.ofMonths(6), Period.ofMonths(9), Period.ofYears(1)};
	  private static readonly int EUR_DSC_NB_FX_NODES = EUR_DSC_FX_TENORS.Length;
	  private static readonly Period[] EUR_DSC_XCCY_TENORS = new Period[] {Period.ofYears(2), Period.ofYears(3), Period.ofYears(4), Period.ofYears(5), Period.ofYears(7), Period.ofYears(10)};
	  private static readonly int EUR_DSC_NB_XCCY_NODES = EUR_DSC_XCCY_TENORS.Length;

	  /// <summary>
	  /// Data for EUR-EURIBOR3M curve </summary>
	  /* Market values */
	  private static readonly double[] EUR_FWD3_MARKET_QUOTES = new double[] {-0.00066, -0.0010, -0.0006, -0.0012, -0.0010, -0.0004, 0.0006, 0.0019, 0.0047, 0.0085};
	  private static readonly int EUR_FWD3_NB_NODES = EUR_FWD3_MARKET_QUOTES.Length;
	  private static readonly string[] EUR_FWD3_ID_VALUE = new string[] {"EUR-Fixing-3M", "EUR-FRA3Mx6M", "EUR-FRA6Mx9M", "EUR-IRS3M-1Y", "EUR-IRS3M-2Y", "EUR-IRS3M-3Y", "EUR-IRS3M-4Y", "EUR-IRS3M-5Y", "EUR-IRS3M-7Y", "EUR-IRS3M-10Y"};
	  /* Nodes */
	  private static readonly CurveNode[] EUR_FWD3_NODES = new CurveNode[EUR_FWD3_NB_NODES];
	  /* Tenors */
	  private static readonly Period[] EUR_FWD3_FRA_TENORS = new Period[] {Period.ofMonths(3), Period.ofMonths(6)};
	  private static readonly int EUR_FWD3_NB_FRA_NODES = EUR_FWD3_FRA_TENORS.Length;
	  private static readonly Period[] EUR_FWD3_IRS_TENORS = new Period[] {Period.ofYears(1), Period.ofYears(2), Period.ofYears(3), Period.ofYears(4), Period.ofYears(5), Period.ofYears(7), Period.ofYears(10)};
	  private static readonly int EUR_FWD3_NB_IRS_NODES = EUR_FWD3_IRS_TENORS.Length;

	  /// <summary>
	  /// All quotes for the curve calibration </summary>
	  private static readonly ImmutableMarketData ALL_QUOTES;

	  private static readonly DiscountingIborFixingDepositProductPricer FIXING_PRICER = DiscountingIborFixingDepositProductPricer.DEFAULT;
	  private static readonly DiscountingFraTradePricer FRA_PRICER = DiscountingFraTradePricer.DEFAULT;
	  private static readonly DiscountingSwapProductPricer SWAP_PRICER = DiscountingSwapProductPricer.DEFAULT;
	  private static readonly DiscountingTermDepositProductPricer DEPO_PRICER = DiscountingTermDepositProductPricer.DEFAULT;
	  private static readonly DiscountingFxSwapProductPricer FX_PRICER = DiscountingFxSwapProductPricer.DEFAULT;
	  private static readonly MarketQuoteSensitivityCalculator MQC = MarketQuoteSensitivityCalculator.DEFAULT;

	  private static readonly RatesCurveCalibrator CALIBRATOR = RatesCurveCalibrator.of(1e-9, 1e-9, 100);

	  // Constants
	  private const double TOLERANCE_PV = 1.0E-6;
	  private const double TOLERANCE_PV_DELTA = 1.0E+3;

	  private static readonly CurveGroupName CURVE_GROUP_NAME = CurveGroupName.of("USD-DSCON-EUR-DSC");
	  private static readonly InterpolatedNodalCurveDefinition USD_DSC_CURVE_DEFN = InterpolatedNodalCurveDefinition.builder().name(USD_DSCON_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(CURVE_DC).interpolator(INTERPOLATOR_LINEAR).extrapolatorLeft(EXTRAPOLATOR_FLAT).extrapolatorRight(EXTRAPOLATOR_FLAT).nodes(USD_DSC_NODES).build();
	  private static readonly InterpolatedNodalCurveDefinition USD_FWD3_CURVE_DEFN = InterpolatedNodalCurveDefinition.builder().name(USD_FWD3_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(CURVE_DC).interpolator(INTERPOLATOR_LINEAR).extrapolatorLeft(EXTRAPOLATOR_FLAT).extrapolatorRight(EXTRAPOLATOR_FLAT).nodes(USD_FWD3_NODES).build();
	  private static readonly InterpolatedNodalCurveDefinition EUR_DSC_CURVE_DEFN = InterpolatedNodalCurveDefinition.builder().name(EUR_DSC_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(CURVE_DC).interpolator(INTERPOLATOR_LINEAR).extrapolatorLeft(EXTRAPOLATOR_FLAT).extrapolatorRight(EXTRAPOLATOR_FLAT).nodes(EUR_DSC_NODES).build();
	  private static readonly InterpolatedNodalCurveDefinition EUR_FWD3_CURVE_DEFN = InterpolatedNodalCurveDefinition.builder().name(EUR_FWD3_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(CURVE_DC).interpolator(INTERPOLATOR_LINEAR).extrapolatorLeft(EXTRAPOLATOR_FLAT).extrapolatorRight(EXTRAPOLATOR_FLAT).nodes(EUR_FWD3_NODES).build();
	  private static readonly RatesCurveGroupDefinition CURVE_GROUP_CONFIG = RatesCurveGroupDefinition.builder().name(CURVE_GROUP_NAME).addCurve(USD_DSC_CURVE_DEFN, USD, USD_FED_FUND).addForwardCurve(USD_FWD3_CURVE_DEFN, USD_LIBOR_3M).addDiscountCurve(EUR_DSC_CURVE_DEFN, EUR).addForwardCurve(EUR_FWD3_CURVE_DEFN, EUR_EURIBOR_3M).build();

	  private static readonly RatesCurveGroupDefinition GROUP_1 = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("USD-DSCON")).addCurve(USD_DSC_CURVE_DEFN, USD, USD_FED_FUND).build();
	  private static readonly RatesCurveGroupDefinition GROUP_2 = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("USD-LIBOR3M")).addForwardCurve(USD_FWD3_CURVE_DEFN, USD_LIBOR_3M).build();
	  private static readonly RatesCurveGroupDefinition GROUP_3 = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("EUR-DSC-EURIBOR3M")).addDiscountCurve(EUR_DSC_CURVE_DEFN, EUR).addForwardCurve(EUR_FWD3_CURVE_DEFN, EUR_EURIBOR_3M).build();
	  private static readonly ImmutableRatesProvider KNOWN_DATA = ImmutableRatesProvider.builder(VAL_DATE).fxRateProvider(MarketDataFxRateProvider.of(ALL_QUOTES)).build();

	  //-------------------------------------------------------------------------
	  public virtual void calibration_present_value_oneGroup()
	  {
		RatesProvider result = CALIBRATOR.calibrate(CURVE_GROUP_CONFIG, ALL_QUOTES, REF_DATA);
		assertPresentValue(result);
	  }

	  public virtual void calibration_present_value_threeGroups()
	  {
		RatesProvider result = CALIBRATOR.calibrate(ImmutableList.of(GROUP_1, GROUP_2, GROUP_3), KNOWN_DATA, ALL_QUOTES, REF_DATA);
		assertPresentValue(result);
	  }

	  private void assertPresentValue(RatesProvider result)
	  {
		// Test PV USD;
		IList<ResolvedTrade> usdTrades = new List<ResolvedTrade>();
		foreach (CurveNode USD_DSC_NODE in USD_DSC_NODES)
		{
		  usdTrades.Add(USD_DSC_NODE.resolvedTrade(1d, ALL_QUOTES, REF_DATA));
		}
		// Depo
		for (int i = 0; i < 2; i++)
		{
		  CurrencyAmount pvDep = DEPO_PRICER.presentValue(((ResolvedTermDepositTrade) usdTrades[i]).Product, result);
		  assertEquals(pvDep.Amount, 0.0, TOLERANCE_PV);
		}
		// OIS
		for (int i = 0; i < USD_DSC_NB_OIS_NODES; i++)
		{
		  MultiCurrencyAmount pvOis = SWAP_PRICER.presentValue(((ResolvedSwapTrade) usdTrades[2 + i]).Product, result);
		  assertEquals(pvOis.getAmount(USD).Amount, 0.0, TOLERANCE_PV);
		}
		// Test PV USD Fwd3
		IList<ResolvedTrade> fwd3Trades = new List<ResolvedTrade>();
		for (int i = 0; i < USD_FWD3_NB_NODES; i++)
		{
		  fwd3Trades.Add(USD_FWD3_NODES[i].resolvedTrade(1d, ALL_QUOTES, REF_DATA));
		}
		// Fixing 
		CurrencyAmount pvFixing = FIXING_PRICER.presentValue(((ResolvedIborFixingDepositTrade) fwd3Trades[0]).Product, result);
		assertEquals(pvFixing.Amount, 0.0, TOLERANCE_PV);
		// FRA
		for (int i = 0; i < USD_FWD3_NB_FRA_NODES; i++)
		{
		  CurrencyAmount pvFra = FRA_PRICER.presentValue(((ResolvedFraTrade) fwd3Trades[i + 1]), result);
		  assertEquals(pvFra.Amount, 0.0, TOLERANCE_PV);
		}
		// IRS
		for (int i = 0; i < USD_FWD3_NB_IRS_NODES; i++)
		{
		  MultiCurrencyAmount pvIrs = SWAP_PRICER.presentValue(((ResolvedSwapTrade) fwd3Trades[i + 1 + USD_FWD3_NB_FRA_NODES]).Product, result);
		  assertEquals(pvIrs.getAmount(USD).Amount, 0.0, TOLERANCE_PV);
		}
		// Test DSC EUR;
		IList<ResolvedTrade> eurTrades = new List<ResolvedTrade>();
		foreach (CurveNode EUR_DSC_NODE in EUR_DSC_NODES)
		{
		  eurTrades.Add(EUR_DSC_NODE.resolvedTrade(1d, ALL_QUOTES, REF_DATA));
		}
		// FX
		for (int i = 0; i < EUR_DSC_NB_FX_NODES; i++)
		{
		  MultiCurrencyAmount pvFx = FX_PRICER.presentValue(((ResolvedFxSwapTrade) eurTrades[i]).Product, result);
		  assertEquals(pvFx.convertedTo(USD, result).Amount, 0.0, TOLERANCE_PV);
		}
		// XCCY
		for (int i = 0; i < EUR_DSC_NB_XCCY_NODES; i++)
		{
		  MultiCurrencyAmount pvFx = SWAP_PRICER.presentValue(((ResolvedSwapTrade) eurTrades[EUR_DSC_NB_FX_NODES + i]).Product, result);
		  assertEquals(pvFx.convertedTo(USD, result).Amount, 0.0, TOLERANCE_PV);
		}
		// Test PV EUR Fwd3
		IList<ResolvedTrade> eurFwd3Trades = new List<ResolvedTrade>();
		for (int i = 0; i < EUR_FWD3_NB_NODES; i++)
		{
		  eurFwd3Trades.Add(EUR_FWD3_NODES[i].resolvedTrade(1d, ALL_QUOTES, REF_DATA));
		}
		// Fixing 
		CurrencyAmount eurPvFixing = FIXING_PRICER.presentValue(((ResolvedIborFixingDepositTrade) eurFwd3Trades[0]).Product, result);
		assertEquals(eurPvFixing.Amount, 0.0, TOLERANCE_PV);
		// FRA
		for (int i = 0; i < EUR_FWD3_NB_FRA_NODES; i++)
		{
		  CurrencyAmount pvFra = FRA_PRICER.presentValue(((ResolvedFraTrade) eurFwd3Trades[i + 1]), result);
		  assertEquals(pvFra.Amount, 0.0, TOLERANCE_PV);
		}
		// IRS
		for (int i = 0; i < EUR_FWD3_NB_IRS_NODES; i++)
		{
		  MultiCurrencyAmount pvIrs = SWAP_PRICER.presentValue(((ResolvedSwapTrade) eurFwd3Trades[i + 1 + EUR_FWD3_NB_FRA_NODES]).Product, result);
		  assertEquals(pvIrs.getAmount(EUR).Amount, 0.0, TOLERANCE_PV);
		}
	  }

	  public virtual void calibration_market_quote_sensitivity_one_group()
	  {
		double shift = 1.0E-6;
		System.Func<ImmutableMarketData, RatesProvider> f = marketData => CALIBRATOR.calibrate(CURVE_GROUP_CONFIG, marketData, REF_DATA);
		calibration_market_quote_sensitivity_check(f, shift);
	  }

	  private void calibration_market_quote_sensitivity_check(System.Func<ImmutableMarketData, RatesProvider> calibrator, double shift)
	  {
		double notional = 100_000_000.0;
		double fx = 1.1111;
		double fxPts = 0.0012;
		ResolvedFxSwapTrade trade = EUR_USD.createTrade(VAL_DATE, Period.ofWeeks(6), Period.ofMonths(5), BuySell.BUY, notional, fx, fxPts, REF_DATA).resolve(REF_DATA);
		RatesProvider result = CALIBRATOR.calibrate(CURVE_GROUP_CONFIG, ALL_QUOTES, REF_DATA);
		PointSensitivities pts = FX_PRICER.presentValueSensitivity(trade.Product, result);
		CurrencyParameterSensitivities ps = result.parameterSensitivity(pts);
		CurrencyParameterSensitivities mqs = MQC.sensitivity(ps, result);
		double pvUsd = FX_PRICER.presentValue(trade.Product, result).getAmount(USD).Amount;
		double pvEur = FX_PRICER.presentValue(trade.Product, result).getAmount(EUR).Amount;
		double[] mqsUsd1Computed = mqs.getSensitivity(USD_DSCON_CURVE_NAME, USD).Sensitivity.toArray();
		for (int i = 0; i < USD_DSC_NB_NODES; i++)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, Object> map = new java.util.HashMap<>(ALL_QUOTES.getValues());
		  IDictionary<MarketDataId<object>, object> map = new Dictionary<MarketDataId<object>, object>(ALL_QUOTES.Values);
		  map[QuoteId.of(StandardId.of(SCHEME, USD_DSC_ID_VALUE[i]))] = USD_DSC_MARKET_QUOTES[i] + shift;
		  ImmutableMarketData marketData = ImmutableMarketData.of(VAL_DATE, map);
		  RatesProvider rpShifted = calibrator(marketData);
		  double pvS = FX_PRICER.presentValue(trade.Product, rpShifted).getAmount(USD).Amount;
		  assertEquals(mqsUsd1Computed[i], (pvS - pvUsd) / shift, TOLERANCE_PV_DELTA);
		}
		double[] mqsUsd2Computed = mqs.getSensitivity(USD_DSCON_CURVE_NAME, EUR).Sensitivity.toArray();
		for (int i = 0; i < USD_DSC_NB_NODES; i++)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, Object> map = new java.util.HashMap<>(ALL_QUOTES.getValues());
		  IDictionary<MarketDataId<object>, object> map = new Dictionary<MarketDataId<object>, object>(ALL_QUOTES.Values);
		  map[QuoteId.of(StandardId.of(SCHEME, USD_DSC_ID_VALUE[i]))] = USD_DSC_MARKET_QUOTES[i] + shift;
		  ImmutableMarketData marketData = ImmutableMarketData.of(VAL_DATE, map);
		  RatesProvider rpShifted = calibrator(marketData);
		  double pvS = FX_PRICER.presentValue(trade.Product, rpShifted).getAmount(EUR).Amount;
		  assertEquals(mqsUsd2Computed[i], (pvS - pvEur) / shift, TOLERANCE_PV_DELTA);
		}
		double[] mqsEur1Computed = mqs.getSensitivity(EUR_DSC_CURVE_NAME, USD).Sensitivity.toArray();
		for (int i = 0; i < EUR_DSC_NB_NODES; i++)
		{
		  assertEquals(mqsEur1Computed[i], 0.0, TOLERANCE_PV_DELTA);
		}
		double[] mqsEur2Computed = mqs.getSensitivity(EUR_DSC_CURVE_NAME, EUR).Sensitivity.toArray();
		for (int i = 0; i < EUR_DSC_NB_NODES; i++)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, Object> map = new java.util.HashMap<>(ALL_QUOTES.getValues());
		  IDictionary<MarketDataId<object>, object> map = new Dictionary<MarketDataId<object>, object>(ALL_QUOTES.Values);
		  map[QuoteId.of(StandardId.of(SCHEME, EUR_DSC_ID_VALUE[i]))] = EUR_DSC_MARKET_QUOTES[i] + shift;
		  ImmutableMarketData marketData = ImmutableMarketData.of(VAL_DATE, map);
		  RatesProvider rpShifted = calibrator(marketData);
		  double pvS = FX_PRICER.presentValue(trade.Product, rpShifted).getAmount(EUR).Amount;
		  assertEquals(mqsEur2Computed[i], (pvS - pvEur) / shift, TOLERANCE_PV_DELTA, "Node " + i);
		}
	  }

	}

}