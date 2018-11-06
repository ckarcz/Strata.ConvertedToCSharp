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
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.deposit.type.TermDepositConventions.USD_SHORT_DEPOSIT_T0;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.deposit.type.TermDepositConventions.USD_SHORT_DEPOSIT_T1;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.fx.type.FxSwapConventions.EUR_USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedOvernightSwapConventions.USD_FIXED_1Y_FED_FUND_OIS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using Index = com.opengamma.strata.basics.index.Index;
	using FxRateId = com.opengamma.strata.data.FxRateId;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using ImmutableMarketDataBuilder = com.opengamma.strata.data.ImmutableMarketDataBuilder;
	using MarketData = com.opengamma.strata.data.MarketData;
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
	using FixedOvernightSwapCurveNode = com.opengamma.strata.market.curve.node.FixedOvernightSwapCurveNode;
	using FxSwapCurveNode = com.opengamma.strata.market.curve.node.FxSwapCurveNode;
	using TermDepositCurveNode = com.opengamma.strata.market.curve.node.TermDepositCurveNode;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using DiscountingTermDepositProductPricer = com.opengamma.strata.pricer.deposit.DiscountingTermDepositProductPricer;
	using DiscountingFxSwapProductPricer = com.opengamma.strata.pricer.fx.DiscountingFxSwapProductPricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using ResolvedTermDepositTrade = com.opengamma.strata.product.deposit.ResolvedTermDepositTrade;
	using TermDepositTemplate = com.opengamma.strata.product.deposit.type.TermDepositTemplate;
	using ResolvedFxSwapTrade = com.opengamma.strata.product.fx.ResolvedFxSwapTrade;
	using FxSwapTemplate = com.opengamma.strata.product.fx.type.FxSwapTemplate;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;
	using FixedOvernightSwapTemplate = com.opengamma.strata.product.swap.type.FixedOvernightSwapTemplate;

	/// <summary>
	/// Test for curve calibration in USD and EUR.
	/// The USD curve is obtained by OIS and the EUR one by FX Swaps from USD.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CalibrationZeroRateUsdEur2OisFxTest
	public class CalibrationZeroRateUsdEur2OisFxTest
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
	  private const string EUR_DSC_STR = "EUR-DSC-FX";
	  private static readonly CurveName EUR_DSC_CURVE_NAME = CurveName.of(EUR_DSC_STR);
	  /// <summary>
	  /// Curves associations to currencies and indices. </summary>
	  private static readonly IDictionary<CurveName, Currency> DSC_NAMES = new Dictionary<CurveName, Currency>();
	  private static readonly IDictionary<CurveName, ISet<Index>> IDX_NAMES = new Dictionary<CurveName, ISet<Index>>();
	  static CalibrationZeroRateUsdEur2OisFxTest()
	  {
		DSC_NAMES[USD_DSCON_CURVE_NAME] = USD;
		ISet<Index> usdFedFundSet = new HashSet<Index>();
		usdFedFundSet.Add(USD_FED_FUND);
		IDX_NAMES[USD_DSCON_CURVE_NAME] = usdFedFundSet;
		USD_DSC_NODES[0] = TermDepositCurveNode.of(TermDepositTemplate.of(Period.ofDays(1), USD_SHORT_DEPOSIT_T0), QuoteId.of(StandardId.of(SCHEME, USD_DSC_ID_VALUE[0])));
		USD_DSC_NODES[1] = TermDepositCurveNode.of(TermDepositTemplate.of(Period.ofDays(1), USD_SHORT_DEPOSIT_T1), QuoteId.of(StandardId.of(SCHEME, USD_DSC_ID_VALUE[1])));
		for (int i = 0; i < USD_DSC_NB_OIS_NODES; i++)
		{
		  USD_DSC_NODES[USD_DSC_NB_DEPO_NODES + i] = FixedOvernightSwapCurveNode.of(FixedOvernightSwapTemplate.of(Period.ZERO, Tenor.of(USD_DSC_OIS_TENORS[i]), USD_FIXED_1Y_FED_FUND_OIS), QuoteId.of(StandardId.of(SCHEME, USD_DSC_ID_VALUE[USD_DSC_NB_DEPO_NODES + i])));
		}
		for (int i = 0; i < EUR_DSC_NB_FX_NODES; i++)
		{
		  EUR_DSC_NODES[i] = FxSwapCurveNode.of(FxSwapTemplate.of(EUR_DSC_FX_TENORS[i], EUR_USD), QuoteId.of(StandardId.of(SCHEME, EUR_DSC_ID_VALUE[i])));
		}
		ImmutableMarketDataBuilder builder = ImmutableMarketData.builder(VAL_DATE);
		for (int i = 0; i < USD_DSC_NB_NODES; i++)
		{
		  builder.addValue(QuoteId.of(StandardId.of(SCHEME, USD_DSC_ID_VALUE[i])), USD_DSC_MARKET_QUOTES[i]);
		}
		for (int i = 0; i < EUR_DSC_NB_NODES; i++)
		{
		  builder.addValue(QuoteId.of(StandardId.of(SCHEME, EUR_DSC_ID_VALUE[i])), EUR_DSC_MARKET_QUOTES[i]);
		}
		builder.addValue(FxRateId.of(EUR, USD), FX_RATE_EUR_USD);
		ALL_QUOTES = builder.build();
	  }

	  /// <summary>
	  /// Data FX * </summary>
	  private static readonly FxRate FX_RATE_EUR_USD = FxRate.of(EUR, USD, 1.10);
	  /// <summary>
	  /// Data for USD-DSCON curve </summary>
	  /* Market values */
	  private static readonly double[] USD_DSC_MARKET_QUOTES = new double[] {0.0016, 0.0022, 0.0013, 0.0016, 0.0020, 0.0026, 0.0033, 0.0039, 0.0053, 0.0066, 0.0090, 0.0111};
	  private static readonly int USD_DSC_NB_NODES = USD_DSC_MARKET_QUOTES.Length;
	  private static readonly string[] USD_DSC_ID_VALUE = new string[] {"USD-ON", "USD-TN", "USD-OIS-1M", "USD-OIS-2M", "USD-OIS-3M", "USD-OIS-6M", "USD-OIS-9M", "USD-OIS-1Y", "USD-OIS-18M", "USD-OIS-2Y", "USD-OIS-3Y", "USD-OIS-4Y"};
	  /* Nodes */
	  private static readonly CurveNode[] USD_DSC_NODES = new CurveNode[USD_DSC_NB_NODES];
	  /* Tenors */
	  private static readonly int[] USD_DSC_DEPO_OFFSET = new int[] {0, 1};
	  private static readonly int USD_DSC_NB_DEPO_NODES = USD_DSC_DEPO_OFFSET.Length;
	  private static readonly Period[] USD_DSC_OIS_TENORS = new Period[] {Period.ofMonths(1), Period.ofMonths(2), Period.ofMonths(3), Period.ofMonths(6), Period.ofMonths(9), Period.ofYears(1), Period.ofMonths(18), Period.ofYears(2), Period.ofYears(3), Period.ofYears(4)};
	  private static readonly int USD_DSC_NB_OIS_NODES = USD_DSC_OIS_TENORS.Length;
	  /// <summary>
	  /// Data for EUR-DSC curve </summary>
	  /* Market values */
	  private static readonly double[] EUR_DSC_MARKET_QUOTES = new double[] {0.0004, 0.0012, 0.0019, 0.0043, 0.0074, 0.0109, 0.0193, 0.0294, 0.0519, 0.0757};
	  private static readonly int EUR_DSC_NB_NODES = EUR_DSC_MARKET_QUOTES.Length;
	  private static readonly string[] EUR_DSC_ID_VALUE = new string[] {"EUR-USD-FX-1M", "EUR-USD-FX-2M", "EUR-USD-FX-3M", "EUR-USD-FX-6M", "EUR-USD-FX-9M", "EUR-USD-FX-1Y", "EUR-USD-FX-18M", "EUR-USD-FX-2Y", "EUR-USD-FX-3Y", "EUR-USD-FX-4Y"};
	  /* Nodes */
	  private static readonly CurveNode[] EUR_DSC_NODES = new CurveNode[EUR_DSC_NB_NODES];
	  /* Tenors */
	  private static readonly Period[] EUR_DSC_FX_TENORS = new Period[] {Period.ofMonths(1), Period.ofMonths(2), Period.ofMonths(3), Period.ofMonths(6), Period.ofMonths(9), Period.ofYears(1), Period.ofMonths(18), Period.ofYears(2), Period.ofYears(3), Period.ofYears(4)};
	  private static readonly int EUR_DSC_NB_FX_NODES = EUR_DSC_FX_TENORS.Length;

	  /// <summary>
	  /// All quotes for the curve calibration </summary>
	  private static readonly ImmutableMarketData ALL_QUOTES;

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
	  private static readonly InterpolatedNodalCurveDefinition EUR_DSC_CURVE_DEFN = InterpolatedNodalCurveDefinition.builder().name(EUR_DSC_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(CURVE_DC).interpolator(INTERPOLATOR_LINEAR).extrapolatorLeft(EXTRAPOLATOR_FLAT).extrapolatorRight(EXTRAPOLATOR_FLAT).nodes(EUR_DSC_NODES).build();
	  private static readonly RatesCurveGroupDefinition CURVE_GROUP_CONFIG = RatesCurveGroupDefinition.builder().name(CURVE_GROUP_NAME).addCurve(USD_DSC_CURVE_DEFN, USD, USD_FED_FUND).addDiscountCurve(EUR_DSC_CURVE_DEFN, EUR).build();

	  //-------------------------------------------------------------------------
	  public virtual void calibration_present_value_oneGroup()
	  {
		RatesProvider result = CALIBRATOR.calibrate(CURVE_GROUP_CONFIG, ALL_QUOTES, REF_DATA);
		assertPresentValue(result);
	  }

	  private void assertPresentValue(RatesProvider result)
	  {
		// Test PV USD;
		IList<ResolvedTrade> usdTrades = new List<ResolvedTrade>();
		for (int i = 0; i < USD_DSC_NODES.Length; i++)
		{
		  usdTrades.Add(USD_DSC_NODES[i].resolvedTrade(1d, ALL_QUOTES, REF_DATA));
		}
		// Depo
		for (int i = 0; i < USD_DSC_NB_DEPO_NODES; i++)
		{
		  CurrencyAmount pvDep = DEPO_PRICER.presentValue(((ResolvedTermDepositTrade) usdTrades[i]).Product, result);
		  assertEquals(pvDep.Amount, 0.0, TOLERANCE_PV);
		}
		// OIS
		for (int i = 0; i < USD_DSC_NB_OIS_NODES; i++)
		{
		  MultiCurrencyAmount pvOis = SWAP_PRICER.presentValue(((ResolvedSwapTrade) usdTrades[USD_DSC_NB_DEPO_NODES + i]).Product, result);
		  assertEquals(pvOis.getAmount(USD).Amount, 0.0, TOLERANCE_PV);
		}
		// Test PV EUR;
		IList<ResolvedTrade> eurTrades = new List<ResolvedTrade>();
		for (int i = 0; i < EUR_DSC_NODES.Length; i++)
		{
		  eurTrades.Add(EUR_DSC_NODES[i].resolvedTrade(1d, ALL_QUOTES, REF_DATA));
		}
		// Depo
		for (int i = 0; i < EUR_DSC_NB_FX_NODES; i++)
		{
		  MultiCurrencyAmount pvFx = FX_PRICER.presentValue(((ResolvedFxSwapTrade) eurTrades[i]).Product, result);
		  assertEquals(pvFx.convertedTo(USD, result).Amount, 0.0, TOLERANCE_PV);
		}
	  }

	  public virtual void calibration_market_quote_sensitivity_one_group()
	  {
		double shift = 1.0E-6;
		System.Func<MarketData, RatesProvider> f = ov => CALIBRATOR.calibrate(CURVE_GROUP_CONFIG, ov, REF_DATA);
		calibration_market_quote_sensitivity_check(f, shift);
	  }

	  private void calibration_market_quote_sensitivity_check(System.Func<MarketData, RatesProvider> calibrator, double shift)
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
		  ImmutableMarketData ov = ImmutableMarketData.of(VAL_DATE, map);
		  RatesProvider rpShifted = calibrator(ov);
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