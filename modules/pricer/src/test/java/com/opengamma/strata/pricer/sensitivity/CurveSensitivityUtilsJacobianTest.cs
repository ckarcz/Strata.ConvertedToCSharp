using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.sensitivity
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_6M;
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
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using QuotesCsvLoader = com.opengamma.strata.loader.csv.QuotesCsvLoader;
	using RatesCalibrationCsvLoader = com.opengamma.strata.loader.csv.RatesCalibrationCsvLoader;
	using ValueType = com.opengamma.strata.market.ValueType;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurveParameterSize = com.opengamma.strata.market.curve.CurveParameterSize;
	using DefaultCurveMetadata = com.opengamma.strata.market.curve.DefaultCurveMetadata;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using JacobianCalibrationMatrix = com.opengamma.strata.market.curve.JacobianCalibrationMatrix;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using TenorParameterMetadata = com.opengamma.strata.market.param.TenorParameterMetadata;
	using CalibrationMeasures = com.opengamma.strata.pricer.curve.CalibrationMeasures;
	using RatesCurveCalibrator = com.opengamma.strata.pricer.curve.RatesCurveCalibrator;
	using DiscountingIborFixingDepositProductPricer = com.opengamma.strata.pricer.deposit.DiscountingIborFixingDepositProductPricer;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using ResolvedIborFixingDepositTrade = com.opengamma.strata.product.deposit.ResolvedIborFixingDepositTrade;
	using IborFixingDepositConvention = com.opengamma.strata.product.deposit.type.IborFixingDepositConvention;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;

	/// <summary>
	/// Tests <seealso cref="CurveSensitivityUtils"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurveSensitivityUtilsJacobianTest
	public class CurveSensitivityUtilsJacobianTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VALUATION_DATE = LocalDate.of(2015, 11, 20);

	  // Configuration and data stored in csv to avoid long code description of the input data
	  private const string CONFIG_PATH = "src/test/resources/curve-config/";
	  private const string QUOTES_PATH = "src/test/resources/quotes/";

	  // Quotes
	  private static readonly RatesCurveCalibrator CALIBRATOR = RatesCurveCalibrator.standard();
	  private const string QUOTES_FILE = "quotes-20151120-eur.csv";
	  private static readonly IDictionary<QuoteId, double> MQ_INPUT = QuotesCsvLoader.load(VALUATION_DATE, ImmutableList.of(ResourceLocator.of(QUOTES_PATH + QUOTES_FILE)));
	  private static readonly ImmutableMarketData MARKET_QUOTES_INPUT = ImmutableMarketData.of(VALUATION_DATE, MQ_INPUT);

	  // Group input based on IRS for EURIBOR6M  
	  public static readonly CurveName EUR_SINGLE_NAME = CurveName.of("EUR-ALLIRS");
	  private const string GROUPS_IN_1_FILE = "EUR-ALLIRS-group.csv";
	  private const string SETTINGS_IN_1_FILE = "EUR-ALLIRS-settings.csv";
	  private const string NODES_IN_1_FILE = "EUR-ALLIRS-STD-nodes.csv";
	  private static readonly RatesCurveGroupDefinition GROUPS_IN_1 = RatesCalibrationCsvLoader.load(ResourceLocator.of(CONFIG_PATH + GROUPS_IN_1_FILE), ResourceLocator.of(CONFIG_PATH + SETTINGS_IN_1_FILE), ResourceLocator.of(CONFIG_PATH + NODES_IN_1_FILE)).get(CurveGroupName.of("EUR-SINGLE"));
	  private static readonly RatesProvider MULTICURVE_EUR_SINGLE_CALIBRATED = CALIBRATOR.calibrate(GROUPS_IN_1, MARKET_QUOTES_INPUT, REF_DATA);

	  public static readonly CalibrationMeasures MARKET_QUOTE = CalibrationMeasures.MARKET_QUOTE;

	  public static readonly DiscountingSwapProductPricer PRICER_SWAP_PRODUCT = DiscountingSwapProductPricer.DEFAULT;
	  public static readonly DiscountingIborFixingDepositProductPricer PRICER_IBORFIX_PRODUCT = DiscountingIborFixingDepositProductPricer.DEFAULT;


	  public static readonly DoubleArray TIME_EUR = DoubleArray.of(1.0d / 365.0d, 1.0d / 12d, 0.25, 0.50, 1.00, 2.00, 3.00, 4.00, 5.00, 7.00, 10.0, 15.0, 20.0, 30.0);
	  public static readonly ImmutableRatesProvider MULTICURVE_EUR_SINGLE_INPUT;
	  static CurveSensitivityUtilsJacobianTest()
	  {
		Tenor[] tenors = new Tenor[] {Tenor.TENOR_1D, Tenor.TENOR_1M, Tenor.TENOR_3M, Tenor.TENOR_6M, Tenor.TENOR_1Y, Tenor.TENOR_2Y, Tenor.TENOR_3Y, Tenor.TENOR_4Y, Tenor.TENOR_5Y, Tenor.TENOR_7Y, Tenor.TENOR_10Y, Tenor.TENOR_15Y, Tenor.TENOR_20Y, Tenor.TENOR_30Y};
		IList<TenorParameterMetadata> metadataList = new List<TenorParameterMetadata>();
		for (int looptenor = 0; looptenor < tenors.Length; looptenor++)
		{
		  metadataList.Add(TenorParameterMetadata.of(tenors[looptenor]));
		}
		DoubleArray rate_eur = DoubleArray.of(0.0160, 0.0165, 0.0155, 0.0155, 0.0155, 0.0150, 0.0150, 0.0160, 0.0165, 0.0155, 0.0155, 0.0155, 0.0150, 0.0140);
		InterpolatedNodalCurve curve_single_eur = InterpolatedNodalCurve.builder().metadata(DefaultCurveMetadata.builder().curveName(EUR_SINGLE_NAME).parameterMetadata(metadataList).dayCount(ACT_365F).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).build()).xValues(TIME_EUR).yValues(rate_eur).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).interpolator(CurveInterpolators.LINEAR).build();
		MULTICURVE_EUR_SINGLE_INPUT = ImmutableRatesProvider.builder(VALUATION_DATE).discountCurve(EUR, curve_single_eur).iborIndexCurve(EUR_EURIBOR_6M, curve_single_eur).build();
		LIST_CURVE_NAMES_1.Add(CurveParameterSize.of(EUR_SINGLE_NAME, TIME_EUR.size()));
	  }
	  public static readonly IList<CurveParameterSize> LIST_CURVE_NAMES_1 = new List<CurveParameterSize>();

	  private const string OG_TICKER = "OG-Ticker";
	  private static readonly Tenor[] TENORS_STD_1 = new Tenor[] {Tenor.TENOR_2Y, Tenor.TENOR_5Y, Tenor.TENOR_10Y, Tenor.TENOR_30Y};
	  private static readonly string[] TICKERS_STD_1 = new string[] {"EUR-IRS6M-2Y", "EUR-IRS6M-5Y", "EUR-IRS6M-10Y", "EUR-IRS6M-30Y"};

	  private const double TOLERANCE_JAC = 1.0E-6;
	  private const double TOLERANCE_JAC_APPROX = 1.0E-2;

	  /// <summary>
	  /// Calibrate a single curve to 4 points. Use the resulting calibrated curves as starting point of the computation 
	  /// of a Jacobian. Compare the direct Jacobian and the one reconstructed from trades.
	  /// </summary>
	  public virtual void direct_one_curve()
	  {
		/* Create trades */
		IList<ResolvedTrade> trades = new List<ResolvedTrade>();
		IList<LocalDate> nodeDates = new List<LocalDate>();
		for (int looptenor = 0; looptenor < TENORS_STD_1.Length; looptenor++)
		{
		  ResolvedSwapTrade t0 = EUR_FIXED_1Y_EURIBOR_6M.createTrade(VALUATION_DATE, TENORS_STD_1[looptenor], BuySell.BUY, 1.0, 0.0, REF_DATA).resolve(REF_DATA);
		  double rate = MARKET_QUOTE.value(t0, MULTICURVE_EUR_SINGLE_CALIBRATED);
		  ResolvedSwapTrade t = EUR_FIXED_1Y_EURIBOR_6M.createTrade(VALUATION_DATE, TENORS_STD_1[looptenor], BuySell.BUY, 1.0, rate, REF_DATA).resolve(REF_DATA);
		  nodeDates.Add(t.Product.EndDate);
		  trades.Add(t);
		}
		/* Par rate sensitivity */
		System.Func<ResolvedTrade, CurrencyParameterSensitivities> sensitivityFunction = (t) => MULTICURVE_EUR_SINGLE_CALIBRATED.parameterSensitivity(PRICER_SWAP_PRODUCT.parRateSensitivity(((ResolvedSwapTrade) t).Product, MULTICURVE_EUR_SINGLE_CALIBRATED).build());
		DoubleMatrix jiComputed = CurveSensitivityUtils.jacobianFromMarketQuoteSensitivities(LIST_CURVE_NAMES_1, trades, sensitivityFunction);
		DoubleMatrix jiExpected = MULTICURVE_EUR_SINGLE_CALIBRATED.findData(EUR_SINGLE_NAME).get().Metadata.findInfo(CurveInfoType.JACOBIAN).get().JacobianMatrix;
		/* Comparison */
		assertEquals(jiComputed.rowCount(), jiExpected.rowCount());
		assertEquals(jiComputed.columnCount(), jiExpected.columnCount());
		for (int i = 0; i < jiComputed.rowCount(); i++)
		{
		  for (int j = 0; j < jiComputed.columnCount(); j++)
		  {
			assertEquals(jiComputed.get(i, j), jiExpected.get(i, j), TOLERANCE_JAC);
		  }
		}
	  }

	  /// <summary>
	  /// Start from a generic zero-coupon curve. Compute the (inverse) Jacobian matrix using linear projection to a small 
	  /// number of points and the Jacobian utility. Compare the direct Jacobian obtained by calibrating a curve
	  /// based on the trades with market quotes computed from the zero-coupon curve.
	  /// </summary>
	  public virtual void with_rebucketing_one_curve()
	  {
		/* Create trades */
		IList<ResolvedTrade> trades = new List<ResolvedTrade>();
		IList<LocalDate> nodeDates = new List<LocalDate>();
		double[] marketQuotes = new double[TENORS_STD_1.Length];
		for (int looptenor = 0; looptenor < TENORS_STD_1.Length; looptenor++)
		{
		  ResolvedSwapTrade t0 = EUR_FIXED_1Y_EURIBOR_6M.createTrade(VALUATION_DATE, TENORS_STD_1[looptenor], BuySell.BUY, 1.0, 0.0, REF_DATA).resolve(REF_DATA);
		  marketQuotes[looptenor] = MARKET_QUOTE.value(t0, MULTICURVE_EUR_SINGLE_INPUT);
		  ResolvedSwapTrade t = EUR_FIXED_1Y_EURIBOR_6M.createTrade(VALUATION_DATE, TENORS_STD_1[looptenor], BuySell.BUY, 1.0, marketQuotes[looptenor], REF_DATA).resolve(REF_DATA);
		  nodeDates.Add(t.Product.EndDate);
		  trades.Add(t);
		}
		System.Func<ResolvedTrade, CurrencyParameterSensitivities> sensitivityFunction = (t) => CurveSensitivityUtils.linearRebucketing(MULTICURVE_EUR_SINGLE_INPUT.parameterSensitivity(PRICER_SWAP_PRODUCT.parRateSensitivity(((ResolvedSwapTrade) t).Product, MULTICURVE_EUR_SINGLE_INPUT).build()), nodeDates, VALUATION_DATE);

		/* Market quotes for comparison */
		IDictionary<QuoteId, double> mqCmp = new Dictionary<QuoteId, double>();
		for (int looptenor = 0; looptenor < TENORS_STD_1.Length; looptenor++)
		{
		  mqCmp[QuoteId.of(StandardId.of(OG_TICKER, TICKERS_STD_1[looptenor]))] = marketQuotes[looptenor];
		}
		ImmutableMarketData marketQuotesObject = ImmutableMarketData.of(VALUATION_DATE, mqCmp);
		RatesProvider multicurveCmp = CALIBRATOR.calibrate(GROUPS_IN_1, marketQuotesObject, REF_DATA);

		/* Comparison */
		DoubleMatrix jiComputed = CurveSensitivityUtils.jacobianFromMarketQuoteSensitivities(LIST_CURVE_NAMES_1, trades, sensitivityFunction);
		DoubleMatrix jiExpected = multicurveCmp.findData(EUR_SINGLE_NAME).get().Metadata.findInfo(CurveInfoType.JACOBIAN).get().JacobianMatrix;
		assertEquals(jiComputed.rowCount(), jiExpected.rowCount());
		assertEquals(jiComputed.columnCount(), jiExpected.columnCount());
		for (int i = 0; i < jiComputed.rowCount(); i++)
		{
		  for (int j = 0; j < jiComputed.columnCount(); j++)
		  {
			assertEquals(jiComputed.get(i, j), jiExpected.get(i, j), TOLERANCE_JAC_APPROX);
			// The comparison is not perfect due to the incoherences introduced by the re-bucketing
		  }
		}
	  }


	  // Group input based on OIS for DSC-ON and IRS for EURIBOR6M  
	  public static readonly CurveName EUR_DSCON_OIS = CurveName.of("EUR-DSCON-OIS");
	  public static readonly CurveName EUR_EURIBOR6M_IRS = CurveName.of("EUR-EURIBOR6M-IRS");
	  private const string GROUPS_IN_2_FILE = "EUR-DSCONOIS-E6IRS-group.csv";
	  private const string SETTINGS_IN_2_FILE = "EUR-DSCONOIS-E6IRS-settings.csv";
	  private const string NODES_IN_2_FILE = "EUR-DSCONOIS-E6IRS-STD-nodes.csv";
	  private static readonly RatesCurveGroupDefinition GROUPS_IN_2 = RatesCalibrationCsvLoader.load(ResourceLocator.of(CONFIG_PATH + GROUPS_IN_2_FILE), ResourceLocator.of(CONFIG_PATH + SETTINGS_IN_2_FILE), ResourceLocator.of(CONFIG_PATH + NODES_IN_2_FILE)).get(CurveGroupName.of("EUR-DSCONOIS-E6IRS"));
	  private static readonly RatesProvider MULTICURVE_EUR_2_CALIBRATED = CALIBRATOR.calibrate(GROUPS_IN_2, MARKET_QUOTES_INPUT, REF_DATA);


	  private static readonly Tenor[] TENORS_STD_2_OIS = new Tenor[] {Tenor.TENOR_1M, Tenor.TENOR_3M, Tenor.TENOR_6M, Tenor.TENOR_1Y, Tenor.TENOR_2Y, Tenor.TENOR_5Y, Tenor.TENOR_10Y, Tenor.TENOR_30Y};
	  private static readonly Tenor[] TENORS_STD_2_IRS = new Tenor[] {Tenor.TENOR_1Y, Tenor.TENOR_2Y, Tenor.TENOR_5Y, Tenor.TENOR_10Y, Tenor.TENOR_30Y};

	  /// <summary>
	  /// Calibrate a single curve to 4 points. Use the resulting calibrated curves as starting point of the computation 
	  /// of a Jacobian. Compare the direct Jacobian and the one reconstructed from trades.
	  /// </summary>
	  public virtual void direct_two_curves()
	  {
		JacobianCalibrationMatrix jiObject = MULTICURVE_EUR_2_CALIBRATED.findData(EUR_DSCON_OIS).get().Metadata.findInfo(CurveInfoType.JACOBIAN).get();
		ImmutableList<CurveParameterSize> order = jiObject.Order; // To obtain the order of the curves in the jacobian

		/* Create trades */
		IList<ResolvedTrade> tradesDsc = new List<ResolvedTrade>();
		for (int looptenor = 0; looptenor < TENORS_STD_2_OIS.Length; looptenor++)
		{
		  ResolvedSwapTrade t0 = EUR_FIXED_1Y_EONIA_OIS.createTrade(VALUATION_DATE, TENORS_STD_2_OIS[looptenor], BuySell.BUY, 1.0, 0.0, REF_DATA).resolve(REF_DATA);
		  double rate = MARKET_QUOTE.value(t0, MULTICURVE_EUR_2_CALIBRATED);
		  ResolvedSwapTrade t = EUR_FIXED_1Y_EONIA_OIS.createTrade(VALUATION_DATE, TENORS_STD_2_OIS[looptenor], BuySell.BUY, 1.0, rate, REF_DATA).resolve(REF_DATA);
		  tradesDsc.Add(t);
		}
		IList<ResolvedTrade> tradesE3 = new List<ResolvedTrade>();
		// Fixing
		IborFixingDepositConvention c = IborFixingDepositConvention.of(EUR_EURIBOR_6M);
		ResolvedIborFixingDepositTrade fix0 = c.createTrade(VALUATION_DATE, EUR_EURIBOR_6M.Tenor.Period, BuySell.BUY, 1.0, 0.0, REF_DATA).resolve(REF_DATA);
		double rateFixing = MARKET_QUOTE.value(fix0, MULTICURVE_EUR_2_CALIBRATED);
		ResolvedIborFixingDepositTrade fix = c.createTrade(VALUATION_DATE, EUR_EURIBOR_6M.Tenor.Period, BuySell.BUY, 1.0, rateFixing, REF_DATA).resolve(REF_DATA);
		tradesE3.Add(fix);
		// IRS
		for (int looptenor = 0; looptenor < TENORS_STD_2_IRS.Length; looptenor++)
		{
		  ResolvedSwapTrade t0 = EUR_FIXED_1Y_EURIBOR_6M.createTrade(VALUATION_DATE, TENORS_STD_2_IRS[looptenor], BuySell.BUY, 1.0, 0.0, REF_DATA).resolve(REF_DATA);
		  double rate = MARKET_QUOTE.value(t0, MULTICURVE_EUR_2_CALIBRATED);
		  ResolvedSwapTrade t = EUR_FIXED_1Y_EURIBOR_6M.createTrade(VALUATION_DATE, TENORS_STD_2_IRS[looptenor], BuySell.BUY, 1.0, rate, REF_DATA).resolve(REF_DATA);
		  tradesE3.Add(t);
		}
		IList<ResolvedTrade> trades = new List<ResolvedTrade>();
		if (order.get(0).Name.Equals(EUR_DSCON_OIS))
		{
		  ((IList<ResolvedTrade>)trades).AddRange(tradesDsc);
		  ((IList<ResolvedTrade>)trades).AddRange(tradesE3);
		}
		else
		{
		  ((IList<ResolvedTrade>)trades).AddRange(tradesE3);
		  ((IList<ResolvedTrade>)trades).AddRange(tradesDsc);
		}
		/* Par rate sensitivity */
		System.Func<ResolvedTrade, CurrencyParameterSensitivities> sensitivityFunction = (t) => MULTICURVE_EUR_2_CALIBRATED.parameterSensitivity((t is ResolvedSwapTrade) ? PRICER_SWAP_PRODUCT.parRateSensitivity(((ResolvedSwapTrade) t).Product, MULTICURVE_EUR_2_CALIBRATED).build() : PRICER_IBORFIX_PRODUCT.parRateSensitivity(((ResolvedIborFixingDepositTrade) t).Product, MULTICURVE_EUR_2_CALIBRATED));
		DoubleMatrix jiComputed = CurveSensitivityUtils.jacobianFromMarketQuoteSensitivities(order, trades, sensitivityFunction);
		DoubleMatrix jiExpectedDsc = MULTICURVE_EUR_2_CALIBRATED.findData(EUR_DSCON_OIS).get().Metadata.getInfo(CurveInfoType.JACOBIAN).JacobianMatrix;
		DoubleMatrix jiExpectedE3 = MULTICURVE_EUR_2_CALIBRATED.findData(EUR_EURIBOR6M_IRS).get().Metadata.getInfo(CurveInfoType.JACOBIAN).JacobianMatrix;
		/* Comparison */
		assertEquals(jiComputed.rowCount(), jiExpectedDsc.rowCount() + jiExpectedE3.rowCount());
		assertEquals(jiComputed.columnCount(), jiExpectedDsc.columnCount());
		assertEquals(jiComputed.columnCount(), jiExpectedE3.columnCount());
		int shiftDsc = order.get(0).Name.Equals(EUR_DSCON_OIS) ? 0 : jiExpectedE3.rowCount();
		for (int i = 0; i < jiExpectedDsc.rowCount(); i++)
		{
		  for (int j = 0; j < jiExpectedDsc.columnCount(); j++)
		  {
			assertEquals(jiComputed.get(i + shiftDsc, j), jiExpectedDsc.get(i, j), TOLERANCE_JAC);
		  }
		}
		int shiftE3 = order.get(0).Name.Equals(EUR_DSCON_OIS) ? jiExpectedDsc.rowCount() : 0;
		for (int i = 0; i < jiExpectedE3.rowCount(); i++)
		{
		  for (int j = 0; j < jiExpectedDsc.columnCount(); j++)
		  {
			assertEquals(jiComputed.get(i + shiftE3, j), jiExpectedE3.get(i, j), TOLERANCE_JAC);
		  }
		}
	  }

	}

}