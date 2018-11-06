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
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using QuotesCsvLoader = com.opengamma.strata.loader.csv.QuotesCsvLoader;
	using RatesCalibrationCsvLoader = com.opengamma.strata.loader.csv.RatesCalibrationCsvLoader;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveDefinition = com.opengamma.strata.market.curve.CurveDefinition;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurveNode = com.opengamma.strata.market.curve.CurveNode;
	using CurveParameterSize = com.opengamma.strata.market.curve.CurveParameterSize;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using NotionalEquivalentCalculator = com.opengamma.strata.pricer.sensitivity.NotionalEquivalentCalculator;
	using DiscountingSwapTradePricer = com.opengamma.strata.pricer.swap.DiscountingSwapTradePricer;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;
	using Trade = com.opengamma.strata.product.Trade;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;
	using ThreeLegBasisSwapConventions = com.opengamma.strata.product.swap.type.ThreeLegBasisSwapConventions;


	/// <summary>
	/// Test the notional equivalent computation based on present value sensitivity to quote in 
	/// the calibrated curves by <seealso cref="RatesCurveCalibrator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CalibrationNotionalEquivalentTest
	public class CalibrationNotionalEquivalentTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private static readonly LocalDate VALUATION_DATE = LocalDate.of(2016, 2, 29);

	  private const string BASE_DIR = "src/test/resources/";
	  private const string GROUPS_FILE = "curve-config/EUR-DSCONOIS-E3BS-E6IRS-group.csv";
	  private const string SETTINGS_FILE = "curve-config/EUR-DSCONOIS-E3BS-E6IRS-settings.csv";
	  private const string NODES_FILE = "curve-config/EUR-DSCONOIS-E3BS-E6IRS-nodes.csv";
	  private const string QUOTES_FILE = "quotes/quotes-20160229-eur.csv";

	  private static readonly CalibrationMeasures CALIBRATION_MEASURES = CalibrationMeasures.PAR_SPREAD;
	  private static readonly RatesCurveCalibrator CALIBRATOR = RatesCurveCalibrator.of(1e-9, 1e-9, 100, CALIBRATION_MEASURES);
	  private static readonly CalibrationMeasures PV_MEASURES = CalibrationMeasures.of("PresentValue", PresentValueCalibrationMeasure.FRA_PV, PresentValueCalibrationMeasure.IBOR_FIXING_DEPOSIT_PV, PresentValueCalibrationMeasure.IBOR_FUTURE_PV, PresentValueCalibrationMeasure.SWAP_PV, PresentValueCalibrationMeasure.TERM_DEPOSIT_PV);
	  private static readonly DiscountingSwapTradePricer PRICER_SWAP_TRADE = DiscountingSwapTradePricer.DEFAULT;
	  private static readonly MarketQuoteSensitivityCalculator MQSC = MarketQuoteSensitivityCalculator.DEFAULT;
	  private static readonly NotionalEquivalentCalculator NEC = NotionalEquivalentCalculator.DEFAULT;

	  private static readonly ResourceLocator QUOTES_RESOURCES = ResourceLocator.of(BASE_DIR + QUOTES_FILE);
	  private static readonly ImmutableMap<QuoteId, double> QUOTES = QuotesCsvLoader.load(VALUATION_DATE, QUOTES_RESOURCES);
	  private static readonly ImmutableMarketData MARKET_QUOTES = ImmutableMarketData.of(VALUATION_DATE, QUOTES);
	  private static readonly RatesCurveGroupDefinition GROUP_DEFINITION = RatesCalibrationCsvLoader.load(ResourceLocator.of(BASE_DIR + GROUPS_FILE), ResourceLocator.of(BASE_DIR + SETTINGS_FILE), ResourceLocator.of(BASE_DIR + NODES_FILE)).get(CurveGroupName.of("EUR-DSCONOIS-E3BS-E6IRS"));
	  private static readonly RatesCurveGroupDefinition GROUP_DEFINITION_NO_INFO = GROUP_DEFINITION.toBuilder().computeJacobian(false).computePvSensitivityToMarketQuote(false).build();
	  private static readonly RatesCurveGroupDefinition GROUP_DEFINITION_PV_SENSI = GROUP_DEFINITION.toBuilder().computeJacobian(true).computePvSensitivityToMarketQuote(true).build();

	  private const double TOLERANCE_PV = 1.0E-8;
	  private const double TOLERANCE_PV_DELTA = 1.0E-2;

	  public virtual void check_pv_with_measures()
	  {
		ImmutableRatesProvider multicurve = CALIBRATOR.calibrate(GROUP_DEFINITION, MARKET_QUOTES, REF_DATA);
		// the trades used for calibration
		IList<ResolvedTrade> trades = new List<ResolvedTrade>();
		ImmutableList<CurveDefinition> curveGroups = GROUP_DEFINITION.CurveDefinitions;
		foreach (CurveDefinition entry in curveGroups)
		{
		  ImmutableList<CurveNode> nodes = entry.Nodes;
		  foreach (CurveNode node in nodes)
		  {
			trades.Add(node.resolvedTrade(1d, MARKET_QUOTES, REF_DATA));
		  }
		}
		// Check PV = 0
		foreach (ResolvedTrade trade in trades)
		{
		  double pv = PV_MEASURES.value(trade, multicurve);
		  assertEquals(pv, 0.0, TOLERANCE_PV);
		}
	  }

	  public virtual void check_pv_sensitivity()
	  {
		ImmutableRatesProvider multicurve = CALIBRATOR.calibrate(GROUP_DEFINITION_PV_SENSI, MARKET_QUOTES, REF_DATA);
		// the trades used for calibration
		IDictionary<CurveName, IList<Trade>> trades = new Dictionary<CurveName, IList<Trade>>();
		IDictionary<CurveName, IList<ResolvedTrade>> resolvedTrades = new Dictionary<CurveName, IList<ResolvedTrade>>();
		ImmutableList<CurveDefinition> curveGroups = GROUP_DEFINITION.CurveDefinitions;
		ImmutableList.Builder<CurveParameterSize> builder = ImmutableList.builder();
		foreach (CurveDefinition entry in curveGroups)
		{
		  ImmutableList<CurveNode> nodes = entry.Nodes;
		  IList<Trade> tradesCurve = new List<Trade>();
		  IList<ResolvedTrade> resolvedTradesCurve = new List<ResolvedTrade>();
		  foreach (CurveNode node in nodes)
		  {
			tradesCurve.Add(node.trade(1d, MARKET_QUOTES, REF_DATA));
			resolvedTradesCurve.Add(node.resolvedTrade(1d, MARKET_QUOTES, REF_DATA));
		  }
		  trades[entry.Name] = tradesCurve;
		  resolvedTrades[entry.Name] = resolvedTradesCurve;
		  builder.add(entry.toCurveParameterSize());
		}
		ImmutableList<CurveParameterSize> order = builder.build(); // order of the curves
		// Check CurveInfo present and sensitivity as expected
		IDictionary<CurveName, DoubleArray> mqsGroup = new Dictionary<CurveName, DoubleArray>();
		int nodeIndex = 0;
		foreach (CurveParameterSize cps in order)
		{
		  int nbParameters = cps.ParameterCount;
		  double[] mqsCurve = new double[nbParameters];
		  for (int looptrade = 0; looptrade < nbParameters; looptrade++)
		  {
			DoubleArray mqsNode = PV_MEASURES.derivative(resolvedTrades[cps.Name][looptrade], multicurve, order);
			mqsCurve[looptrade] = mqsNode.get(nodeIndex);
			nodeIndex++;
		  }
		  Optional<Curve> curve = multicurve.findData(cps.Name);
		  DoubleArray pvSensitivityExpected = DoubleArray.ofUnsafe(mqsCurve);
		  mqsGroup[cps.Name] = pvSensitivityExpected;
		  assertTrue(curve.Present);
		  assertTrue(curve.get().Metadata.findInfo(CurveInfoType.PV_SENSITIVITY_TO_MARKET_QUOTE).Present);
		  DoubleArray pvSensitivityMetadata = curve.get().Metadata.findInfo(CurveInfoType.PV_SENSITIVITY_TO_MARKET_QUOTE).get();
		  assertTrue(pvSensitivityExpected.equalWithTolerance(pvSensitivityMetadata, 1.0E-10));
		}
	  }

	  public virtual void check_equivalent_notional()
	  {
		ImmutableRatesProvider multicurve = CALIBRATOR.calibrate(GROUP_DEFINITION_PV_SENSI, MARKET_QUOTES, REF_DATA);
		// Create notional equivalent for a basis trade
		ResolvedSwapTrade trade = ThreeLegBasisSwapConventions.EUR_FIXED_1Y_EURIBOR_3M_EURIBOR_6M.createTrade(VALUATION_DATE, Period.ofMonths(7), Tenor.TENOR_6Y, BuySell.SELL, 1_000_000, 0.03, REF_DATA).resolve(REF_DATA);
		PointSensitivities pts = PRICER_SWAP_TRADE.presentValueSensitivity(trade, multicurve);
		CurrencyParameterSensitivities ps = multicurve.parameterSensitivity(pts);
		CurrencyParameterSensitivities mqs = MQSC.sensitivity(ps, multicurve);
		CurrencyParameterSensitivities notionalEquivalent = NEC.notionalEquivalent(mqs, multicurve);
		// Check metadata are same as market quote sensitivities.
		foreach (CurrencyParameterSensitivity sensi in mqs.Sensitivities)
		{
		  assertEquals(notionalEquivalent.getSensitivity(sensi.MarketDataName, sensi.Currency).ParameterMetadata, sensi.ParameterMetadata);
		}
		// Check sensitivity: trade sensitivity = sum(notional equivalent sensitivities)
		int totalNbParameters = 0;
		IDictionary<CurveName, IList<ResolvedTrade>> equivalentTrades = new Dictionary<CurveName, IList<ResolvedTrade>>();
		ImmutableList<CurveDefinition> curveGroups = GROUP_DEFINITION.CurveDefinitions;
		ImmutableList.Builder<CurveParameterSize> builder = ImmutableList.builder();
		foreach (CurveDefinition entry in curveGroups)
		{
		  totalNbParameters += entry.ParameterCount;
		  DoubleArray notionalCurve = notionalEquivalent.getSensitivity(entry.Name, Currency.EUR).Sensitivity;
		  ImmutableList<CurveNode> nodes = entry.Nodes;
		  IList<ResolvedTrade> resolvedTradesCurve = new List<ResolvedTrade>();
		  for (int i = 0; i < nodes.size(); i++)
		  {
			resolvedTradesCurve.Add(nodes.get(i).resolvedTrade(notionalCurve.get(i), MARKET_QUOTES, REF_DATA));
		  }
		  equivalentTrades[entry.Name] = resolvedTradesCurve;
		  builder.add(entry.toCurveParameterSize());
		}
		ImmutableList<CurveParameterSize> order = builder.build(); // order of the curves
		DoubleArray totalSensitivity = DoubleArray.filled(totalNbParameters);
		foreach (KeyValuePair<CurveName, IList<ResolvedTrade>> entry in equivalentTrades.SetOfKeyValuePairs())
		{
		  foreach (ResolvedTrade t in entry.Value)
		  {
			totalSensitivity = totalSensitivity.plus(PV_MEASURES.derivative(t, multicurve, order));
		  }
		}
		DoubleArray instrumentSensi = PV_MEASURES.derivative(trade, multicurve, order);
		assertTrue(totalSensitivity.equalWithTolerance(instrumentSensi, TOLERANCE_PV_DELTA));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unused") @Test(enabled = false) public void performance()
	  public virtual void performance()
	  {
		long start, end;
		int nbRep = 5;
		int nbTests = 10;

		for (int looprep = 0; looprep < nbRep; looprep++)
		{
		  Console.WriteLine("Calibration time");

		  start = DateTimeHelper.CurrentUnixTimeMillis();
		  for (int i = 0; i < nbTests; i++)
		  {
			ImmutableRatesProvider multicurve1 = CALIBRATOR.calibrate(GROUP_DEFINITION_NO_INFO, MARKET_QUOTES, REF_DATA);
		  }
		  end = DateTimeHelper.CurrentUnixTimeMillis();
		  Console.WriteLine("  |--> calibration only: " + (end - start) + " ms for " + nbTests + " runs.");

		  start = DateTimeHelper.CurrentUnixTimeMillis();
		  for (int i = 0; i < nbTests; i++)
		  {
			ImmutableRatesProvider multicurve1 = CALIBRATOR.calibrate(GROUP_DEFINITION, MARKET_QUOTES, REF_DATA);
		  }
		  end = DateTimeHelper.CurrentUnixTimeMillis();
		  Console.WriteLine("  |--> calibration and Jacobian: " + (end - start) + " ms for " + nbTests + " runs.");

		  start = DateTimeHelper.CurrentUnixTimeMillis();
		  for (int i = 0; i < nbTests; i++)
		  {
			ImmutableRatesProvider multicurve1 = CALIBRATOR.calibrate(GROUP_DEFINITION_PV_SENSI, MARKET_QUOTES, REF_DATA);
		  }
		  end = DateTimeHelper.CurrentUnixTimeMillis();
		  Console.WriteLine("  |--> calibration, Jacobian and PV sensi MQ: " + (end - start) + " ms for " + nbTests + " runs.");
		}

	  }

	}

}