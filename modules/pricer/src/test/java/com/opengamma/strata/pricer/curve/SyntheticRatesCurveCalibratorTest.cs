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
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using Index = com.opengamma.strata.basics.index.Index;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using FxRateId = com.opengamma.strata.data.FxRateId;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using MarketData = com.opengamma.strata.data.MarketData;
	using FxRatesCsvLoader = com.opengamma.strata.loader.csv.FxRatesCsvLoader;
	using QuotesCsvLoader = com.opengamma.strata.loader.csv.QuotesCsvLoader;
	using RatesCalibrationCsvLoader = com.opengamma.strata.loader.csv.RatesCalibrationCsvLoader;
	using CurveDefinition = com.opengamma.strata.market.curve.CurveDefinition;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using CurveNode = com.opengamma.strata.market.curve.CurveNode;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;
	using SwapLegType = com.opengamma.strata.product.swap.SwapLegType;

	/// <summary>
	/// Tests <seealso cref="SyntheticRatesCurveCalibrator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SyntheticRatesCurveCalibratorTest
	public class SyntheticRatesCurveCalibratorTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VALUATION_DATE = LocalDate.of(2015, 11, 20);

	  // Configuration and data stored in csv to avoid long code description of the input data
	  private const string CONFIG_PATH = "src/test/resources/curve-config/";
	  private const string QUOTES_PATH = "src/test/resources/quotes/";
	  // Group input based on FRA and basis swaps for EURIBOR3M  
	  private const string GROUPS_IN_EUR_FILE = "EUR-DSCONOIS-E3BS-E6IRS-group.csv";
	  private const string SETTINGS_IN_EUR_FILE = "EUR-DSCONOIS-E3BS-E6IRS-settings.csv";
	  private const string NODES_IN_EUR_FILE = "EUR-DSCONOIS-E3BS-E6IRS-nodes.csv";
	  private static readonly RatesCurveGroupDefinition GROUPS_IN_EUR = RatesCalibrationCsvLoader.load(ResourceLocator.of(CONFIG_PATH + GROUPS_IN_EUR_FILE), ResourceLocator.of(CONFIG_PATH + SETTINGS_IN_EUR_FILE), ResourceLocator.of(CONFIG_PATH + NODES_IN_EUR_FILE)).get(CurveGroupName.of("EUR-DSCONOIS-E3BS-E6IRS"));
	  private const string GROUPS_IN_USDEUR_FILE = "USD-EUR-DSCONOIS-L3IRS-DSCFXXCCY33-E3IRS-group.csv";
	  private const string SETTINGS_IN_USDEUR_FILE = "USD-EUR-DSCONOIS-L3IRS-DSCFXXCCY33-E3IRS-settings.csv";
	  private const string NODES_IN_USDEUR_FILE = "USD-EUR-DSCONOIS-L3IRS-DSCFXXCCY33-E3IRS-nodes.csv";
	  private static readonly RatesCurveGroupDefinition GROUPS_IN_USDEUR = RatesCalibrationCsvLoader.load(ResourceLocator.of(CONFIG_PATH + GROUPS_IN_USDEUR_FILE), ResourceLocator.of(CONFIG_PATH + SETTINGS_IN_USDEUR_FILE), ResourceLocator.of(CONFIG_PATH + NODES_IN_USDEUR_FILE)).get(CurveGroupName.of("USD-EUR-DSCONOIS-L3IRS-DSCFXXCCY33-E3IRS"));
	  // Group with synthetic curves, all nodes based on deposit or Fixed v Floating swaps
	  private const string GROUPS_SY_EUR_FILE = "FRTB-EUR-group.csv";
	  private const string SETTINGS_SY_EUR_FILE = "FRTB-EUR-settings.csv";
	  private const string NODES_SY_EUR_FILE = "FRTB-EUR-nodes.csv";
	  private static readonly RatesCurveGroupDefinition GROUPS_SYN_EUR = RatesCalibrationCsvLoader.load(ResourceLocator.of(CONFIG_PATH + GROUPS_SY_EUR_FILE), ResourceLocator.of(CONFIG_PATH + SETTINGS_SY_EUR_FILE), ResourceLocator.of(CONFIG_PATH + NODES_SY_EUR_FILE)).get(CurveGroupName.of("BIMM-EUR"));
	  private const string GROUPS_SY_USDEUR_FILE = "FRTB-USD-EUR-group.csv";
	  private const string SETTINGS_SY_USDEUR_FILE = "FRTB-USD-EUR-settings.csv";
	  private const string NODES_SY_USDEUR_FILE = "FRTB-USD-EUR-nodes.csv";
	  private static readonly RatesCurveGroupDefinition GROUPS_SYN_USDEUR = RatesCalibrationCsvLoader.load(ResourceLocator.of(CONFIG_PATH + GROUPS_SY_USDEUR_FILE), ResourceLocator.of(CONFIG_PATH + SETTINGS_SY_USDEUR_FILE), ResourceLocator.of(CONFIG_PATH + NODES_SY_USDEUR_FILE)).get(CurveGroupName.of("FRTB-USD-EUR"));
	  private const string QUOTES_EUR_FILE = "quotes-20151120-eur.csv";
	  private const string QUOTES_USDEUR_FILE = "MARKET_QUOTES_EUR_USD_20151120.csv";
	  private const string QUOTE_FX_FILE = "MARKET-QUOTES-FX-20151120.csv";
	  private static readonly IDictionary<QuoteId, double> MQ_EUR_INPUT = QuotesCsvLoader.load(VALUATION_DATE, ImmutableList.of(ResourceLocator.of(QUOTES_PATH + QUOTES_EUR_FILE)));
	  private static readonly ImmutableMarketData MARKET_QUOTES_EUR_INPUT = ImmutableMarketData.of(VALUATION_DATE, MQ_EUR_INPUT);
	  private static readonly IDictionary<QuoteId, double> MQ_USDEUR_INPUT = QuotesCsvLoader.load(VALUATION_DATE, ImmutableList.of(ResourceLocator.of(QUOTES_PATH + QUOTES_USDEUR_FILE)));
	  private static readonly IDictionary<FxRateId, FxRate> MAP_FX = FxRatesCsvLoader.load(VALUATION_DATE, ResourceLocator.of(QUOTES_PATH + QUOTE_FX_FILE));
	  private static readonly ImmutableMarketData MARKET_QUOTES_USDEUR_INPUT = ImmutableMarketData.builder(VALUATION_DATE).addValueMap(MQ_USDEUR_INPUT).addValueMap(MAP_FX).build();
	  private static readonly IDictionary<Index, LocalDateDoubleTimeSeries> TS_LARGE = new Dictionary<Index, LocalDateDoubleTimeSeries>();
	  private static readonly MarketData TS_LARGE_MD;
	  static SyntheticRatesCurveCalibratorTest()
	  { // Fixing unnaturally high to see the difference in the calibration
		LocalDateDoubleTimeSeries tsEur3 = LocalDateDoubleTimeSeries.builder().put(VALUATION_DATE, 0.0200).build();
		LocalDateDoubleTimeSeries tsEur6 = LocalDateDoubleTimeSeries.builder().put(VALUATION_DATE, 0.0250).build();
		TS_LARGE[EUR_EURIBOR_3M] = tsEur3;
		TS_LARGE[EUR_EURIBOR_6M] = tsEur6;
		TS_LARGE_MD = ImmutableMarketData.builder(VALUATION_DATE).addTimeSeries(IndexQuoteId.of(EUR_EURIBOR_3M), tsEur3).addTimeSeries(IndexQuoteId.of(EUR_EURIBOR_6M), tsEur6).build();
	  }
	  private static readonly RatesCurveCalibrator CALIBRATOR = RatesCurveCalibrator.standard();
	  private static readonly CalibrationMeasures MQ_MEASURES = CalibrationMeasures.MARKET_QUOTE;
	  private static readonly SyntheticRatesCurveCalibrator CALIBRATOR_SYNTHETIC = SyntheticRatesCurveCalibrator.of(CALIBRATOR, MQ_MEASURES);

	  private static readonly ImmutableRatesProvider MULTICURVE_INPUT_EUR_TSEMPTY = CALIBRATOR.calibrate(GROUPS_IN_EUR, MARKET_QUOTES_EUR_INPUT, REF_DATA);
	  private static readonly RatesProvider MULTICURVE_INPUT_EUR_TSLARGE = CALIBRATOR.calibrate(GROUPS_IN_EUR, MARKET_QUOTES_EUR_INPUT.combinedWith(TS_LARGE_MD), REF_DATA);
	  private static readonly ImmutableRatesProvider MULTICURVE_INPUT_USDEUR_TSEMPTY = CALIBRATOR.calibrate(GROUPS_IN_USDEUR, MARKET_QUOTES_USDEUR_INPUT, REF_DATA);

	  private const double TOLERANCE_MQ = 1.0E-8;

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		SyntheticRatesCurveCalibrator test = SyntheticRatesCurveCalibrator.of(CALIBRATOR, MQ_MEASURES);
		assertEquals(test.Measures, MQ_MEASURES);
		assertEquals(test.Calibrator, CALIBRATOR);
		assertEquals(test.ToString(), "SyntheticCurveCalibrator[CurveCalibrator[ParSpread], MarketQuote]");
	  }

	  //-------------------------------------------------------------------------
	  // Check market data computation
	  public virtual void market_data()
	  {
		RatesCurveGroupDefinition group = GROUPS_SYN_EUR;
		RatesProvider multicurveTsLarge = MULTICURVE_INPUT_EUR_TSEMPTY.toBuilder().timeSeries(TS_LARGE).build();
		MarketData madTsEmpty = CALIBRATOR_SYNTHETIC.marketData(group, MULTICURVE_INPUT_EUR_TSEMPTY, REF_DATA);
		MarketData madTsLarge = CALIBRATOR_SYNTHETIC.marketData(group, multicurveTsLarge, REF_DATA);
		assertEquals(madTsEmpty.ValuationDate, VALUATION_DATE);
		foreach (CurveDefinition entry in group.CurveDefinitions)
		{
		  ImmutableList<CurveNode> nodes = entry.Nodes;
		  foreach (CurveNode node in nodes)
		  {
			ResolvedTrade tradeTsEmpty = node.resolvedTrade(1d, madTsEmpty, REF_DATA);
			double mqTsEmpty = MQ_MEASURES.value(tradeTsEmpty, MULTICURVE_INPUT_EUR_TSEMPTY);
			assertEquals(mqTsEmpty, (double?) madTsEmpty.getValue(node.requirements().GetEnumerator().next()), TOLERANCE_MQ);
			ResolvedTrade tradeTsLarge = node.resolvedTrade(1d, madTsLarge, REF_DATA);
			double mqTsLarge = MQ_MEASURES.value(tradeTsLarge, multicurveTsLarge);
			assertEquals(mqTsLarge, (double?) madTsLarge.getValue(node.requirements().GetEnumerator().next()), TOLERANCE_MQ);
			// Market Quote for Fixed v ibor swaps should have changed with the fixing
			if ((tradeTsLarge is ResolvedSwapTrade) && (((ResolvedSwapTrade) tradeTsLarge)).Product.getLegs(SwapLegType.IBOR).size() == 1)
			{
			  assertTrue(Math.Abs(mqTsEmpty - mqTsLarge) > TOLERANCE_MQ);
			}
		  }
		}
		assertEquals(madTsEmpty.TimeSeriesIds, ImmutableSet.of());
		assertEquals(madTsLarge.TimeSeriesIds, ImmutableSet.of(IndexQuoteId.of(EUR_EURIBOR_3M), IndexQuoteId.of(EUR_EURIBOR_6M)));
	  }

	  // Check synthetic calibration in case no definitions
	  public virtual void calibrate_noDefinitions()
	  {
		RatesCurveGroupDefinition empty = RatesCurveGroupDefinition.of(CurveGroupName.of("Group"), ImmutableList.of(), ImmutableList.of());
		MarketData mad = CALIBRATOR_SYNTHETIC.marketData(empty, MULTICURVE_INPUT_EUR_TSLARGE, REF_DATA);
		RatesProvider multicurveSyn = CALIBRATOR_SYNTHETIC.calibrate(empty, MULTICURVE_INPUT_EUR_TSLARGE, REF_DATA);
		assertEquals(multicurveSyn.DiscountCurrencies, ImmutableSet.of());
		assertEquals(multicurveSyn.IborIndices, ImmutableSet.of());
		assertEquals(multicurveSyn.OvernightIndices, ImmutableSet.of());
		assertEquals(multicurveSyn.PriceIndices, ImmutableSet.of());
		assertEquals(mad.TimeSeriesIds, ImmutableSet.of());
	  }

	  // Check synthetic calibration in case no time-series is present
	  public virtual void calibrate_ts_empty()
	  {
		MarketData mad = CALIBRATOR_SYNTHETIC.marketData(GROUPS_SYN_EUR, MULTICURVE_INPUT_EUR_TSEMPTY, REF_DATA);
		RatesProvider multicurveSyn = CALIBRATOR_SYNTHETIC.calibrate(GROUPS_SYN_EUR, MULTICURVE_INPUT_EUR_TSEMPTY, REF_DATA);
		foreach (CurveDefinition entry in GROUPS_SYN_EUR.CurveDefinitions)
		{
		  ImmutableList<CurveNode> nodes = entry.Nodes;
		  foreach (CurveNode node in nodes)
		  {
			ResolvedTrade trade = node.resolvedTrade(1d, mad, REF_DATA);
			double mqIn = MQ_MEASURES.value(trade, MULTICURVE_INPUT_EUR_TSEMPTY);
			double mqSy = MQ_MEASURES.value(trade, multicurveSyn);
			assertEquals(mqIn, mqSy, TOLERANCE_MQ);
		  }
		}
		assertEquals(mad.TimeSeriesIds, ImmutableSet.of());
	  }

	  // Check synthetic calibration in the case of existing time-series with fixing on the valuation date
	  public virtual void calibrate_ts_vd()
	  {
		SyntheticRatesCurveCalibrator calibratorDefault = SyntheticRatesCurveCalibrator.standard();
		MarketData mad = calibratorDefault.marketData(GROUPS_SYN_EUR, MULTICURVE_INPUT_EUR_TSLARGE, REF_DATA);
		RatesProvider multicurveSyn = CALIBRATOR_SYNTHETIC.calibrate(GROUPS_SYN_EUR, MULTICURVE_INPUT_EUR_TSLARGE, REF_DATA);
		foreach (CurveDefinition entry in GROUPS_SYN_EUR.CurveDefinitions)
		{
		  ImmutableList<CurveNode> nodes = entry.Nodes;
		  foreach (CurveNode node in nodes)
		  {
			ResolvedTrade trade = node.resolvedTrade(1d, mad, REF_DATA);
			double mqIn = MQ_MEASURES.value(trade, MULTICURVE_INPUT_EUR_TSLARGE);
			double mqSy = MQ_MEASURES.value(trade, multicurveSyn);
			assertEquals(mqIn, mqSy, TOLERANCE_MQ);
		  }
		}
	  }

	  // Check FX rates are transfered in multi-currency cases.
	  public virtual void calibrate_xccy()
	  {
		RatesProvider multicurveSyn = CALIBRATOR_SYNTHETIC.calibrate(GROUPS_SYN_USDEUR, MULTICURVE_INPUT_USDEUR_TSEMPTY, REF_DATA);
		double eurUsdInput = MULTICURVE_INPUT_USDEUR_TSEMPTY.fxRate(Currency.EUR, Currency.USD);
		double eurUsdSynthetic = multicurveSyn.fxRate(Currency.EUR, Currency.USD);
		assertEquals(eurUsdInput, eurUsdSynthetic, TOLERANCE_MQ);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(enabled = false) public void performance()
	  public virtual void performance()
	  {
		long start, end;
		int nbReps = 4;
		int nbTests = 100;
		for (int looprep = 0; looprep < nbReps; looprep++)
		{
		  start = DateTimeHelper.CurrentUnixTimeMillis();
		  int hs = 0;
		  for (int looptest = 0; looptest < nbTests; looptest++)
		  {
			RatesProvider multicurve = CALIBRATOR.calibrate(GROUPS_IN_EUR, MARKET_QUOTES_EUR_INPUT.combinedWith(TS_LARGE_MD), REF_DATA);
			hs += multicurve.ValuationDate.DayOfMonth;
		  }
		  end = DateTimeHelper.CurrentUnixTimeMillis();
		  Console.WriteLine("Initial curve calibration time: " + (end - start) + " ms for " + nbTests + " calibrations.  " + hs);
		}
		for (int looprep = 0; looprep < nbReps; looprep++)
		{
		  start = DateTimeHelper.CurrentUnixTimeMillis();
		  int hs = 0;
		  for (int looptest = 0; looptest < nbTests; looptest++)
		  {
			RatesProvider multicurve1 = CALIBRATOR.calibrate(GROUPS_IN_EUR, MARKET_QUOTES_EUR_INPUT.combinedWith(TS_LARGE_MD), REF_DATA);
			RatesProvider multicurve2 = CALIBRATOR_SYNTHETIC.calibrate(GROUPS_SYN_EUR, multicurve1, REF_DATA);
			hs += multicurve2.ValuationDate.DayOfMonth;
		  }
		  end = DateTimeHelper.CurrentUnixTimeMillis();
		  Console.WriteLine("Initial + synthetic curve calibration time: " + (end - start) + " ms for " + nbTests + " calibrations.  " + hs);
		}
		// Calibration time of the (initial + synthetic) curves is roughly twice as long as the initial calibration on its
		// own. There is almost no overhead to compute the synthetic quotes used as input to the second calibration.
	  }

	}

}