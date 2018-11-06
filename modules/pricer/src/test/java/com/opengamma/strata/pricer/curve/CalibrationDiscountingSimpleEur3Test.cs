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
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MarketData = com.opengamma.strata.data.MarketData;
	using CurveDefinition = com.opengamma.strata.market.curve.CurveDefinition;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using CurveNode = com.opengamma.strata.market.curve.CurveNode;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using DiscountingIborFixingDepositProductPricer = com.opengamma.strata.pricer.deposit.DiscountingIborFixingDepositProductPricer;
	using DiscountingFraProductPricer = com.opengamma.strata.pricer.fra.DiscountingFraProductPricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;
	using ResolvedIborFixingDepositTrade = com.opengamma.strata.product.deposit.ResolvedIborFixingDepositTrade;
	using ResolvedFraTrade = com.opengamma.strata.product.fra.ResolvedFraTrade;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;

	/// <summary>
	/// Test curve calibration
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CalibrationDiscountingSimpleEur3Test
	public class CalibrationDiscountingSimpleEur3Test
	{

	  private static readonly LocalDate VAL_DATE = LocalDate.of(2015, 7, 24);

	  // reference data
	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  /// <summary>
	  /// Data for EUR-DSCON curve </summary>
	  /* Market values */
	  private static readonly double[] DSC_MARKET_QUOTES = new double[] {0.0010, 0.0020, 0.0030, 0.0040};
	  /* Tenors */
	  private static readonly Period[] DSC_OIS_TENORS = new Period[] {Period.ofYears(2), Period.ofYears(5), Period.ofYears(10), Period.ofYears(30)};

	  /// <summary>
	  /// Data for EUR-EURIBOR3M curve </summary>
	  /* Market values */
	  private const double FWD3_FIXING_QUOTE = 0.0050;
	  private static readonly double[] FWD3_FRA_QUOTES = new double[] {0.0051, 0.0052, 0.0053};
	  private static readonly double[] FWD3_IRS_QUOTES = new double[] {0.0054, 0.0055, 0.0056, 0.0057};
	  /* Tenors */
	  private static readonly Period[] FWD3_FRA_TENORS = new Period[] {Period.ofMonths(3), Period.ofMonths(6), Period.ofMonths(9)};
	  private static readonly Period[] FWD3_IRS_TENORS = new Period[] {Period.ofYears(2), Period.ofYears(5), Period.ofYears(10), Period.ofYears(30)};

	  /// <summary>
	  /// Data for EUR-EURIBOR6M curve </summary>
	  /* Market values */
	  private const double FWD6_FIXING_QUOTE = 0.001;
	  private static readonly double[] FWD6_FRA_QUOTES = new double[] {0.011, 0.012};
	  private static readonly double[] FWD6_IRS_QUOTES = new double[] {0.013, 0.014, 0.015, 0.016, 0.017};
	  /* Tenors */
	  private static readonly Period[] FWD6_FRA_TENORS = new Period[] {Period.ofMonths(3), Period.ofMonths(6)};
	  private static readonly Period[] FWD6_IRS_TENORS = new Period[] {Period.ofYears(2), Period.ofYears(3), Period.ofYears(5), Period.ofYears(10), Period.ofYears(30)};

	  private static readonly DiscountingIborFixingDepositProductPricer PRICER_FIXING = DiscountingIborFixingDepositProductPricer.DEFAULT;
	  private static readonly DiscountingFraProductPricer PRICER_FRA = DiscountingFraProductPricer.DEFAULT;
	  private static readonly DiscountingSwapProductPricer SWAP_PRICER = DiscountingSwapProductPricer.DEFAULT;
	  private static readonly MarketQuoteSensitivityCalculator MQC = MarketQuoteSensitivityCalculator.DEFAULT;

	  // Constants
	  private const double TOLERANCE_PV = 1.0E-6;
	  private const double TOLERANCE_DELTA = 1.0E-10;


	  //-------------------------------------------------------------------------
	  public virtual void calibration_present_value()
	  {
		RatesProvider result = CalibrationEurStandard.calibrateEurStandard(VAL_DATE, DSC_MARKET_QUOTES, DSC_OIS_TENORS, FWD3_FIXING_QUOTE, FWD3_FRA_QUOTES, FWD3_IRS_QUOTES, FWD3_FRA_TENORS, FWD3_IRS_TENORS, FWD6_FIXING_QUOTE, FWD6_FRA_QUOTES, FWD6_IRS_QUOTES, FWD6_FRA_TENORS, FWD6_IRS_TENORS);

		/* Curve Discounting/EUR-EONIA */
		string[] dscIdValues = CalibrationEurStandard.dscIdValues(DSC_OIS_TENORS);
		/* Curve EUR-EURIBOR-3M */
		double[] fwd3MarketQuotes = CalibrationEurStandard.fwdMarketQuotes(FWD3_FIXING_QUOTE, FWD3_FRA_QUOTES, FWD3_IRS_QUOTES);
		string[] fwd3IdValue = CalibrationEurStandard.fwdIdValue(3, FWD3_FIXING_QUOTE, FWD3_FRA_QUOTES, FWD3_IRS_QUOTES, FWD3_FRA_TENORS, FWD3_IRS_TENORS);
		/* Curve EUR-EURIBOR-6M */
		double[] fwd6MarketQuotes = CalibrationEurStandard.fwdMarketQuotes(FWD6_FIXING_QUOTE, FWD6_FRA_QUOTES, FWD6_IRS_QUOTES);
		string[] fwd6IdValue = CalibrationEurStandard.fwdIdValue(6, FWD6_FIXING_QUOTE, FWD6_FRA_QUOTES, FWD6_IRS_QUOTES, FWD6_FRA_TENORS, FWD6_IRS_TENORS);
		/* All quotes for the curve calibration */
		MarketData allQuotes = CalibrationEurStandard.allQuotes(VAL_DATE, DSC_MARKET_QUOTES, dscIdValues, fwd3MarketQuotes, fwd3IdValue, fwd6MarketQuotes, fwd6IdValue);
		/* All nodes by groups. */
		RatesCurveGroupDefinition config = CalibrationEurStandard.config(DSC_OIS_TENORS, dscIdValues, FWD3_FRA_TENORS, FWD3_IRS_TENORS, fwd3IdValue, FWD6_FRA_TENORS, FWD6_IRS_TENORS, fwd6IdValue);

		ImmutableList<CurveDefinition> definitions = config.CurveDefinitions;
		// Test PV Dsc
		ImmutableList<CurveNode> dscNodes = definitions.get(0).Nodes;
		IList<ResolvedTrade> dscTrades = new List<ResolvedTrade>();
		for (int i = 0; i < dscNodes.size(); i++)
		{
		  dscTrades.Add(dscNodes.get(i).resolvedTrade(1d, allQuotes, REF_DATA));
		}
		// OIS
		for (int i = 0; i < DSC_MARKET_QUOTES.Length; i++)
		{
		  MultiCurrencyAmount pvIrs = SWAP_PRICER.presentValue(((ResolvedSwapTrade) dscTrades[i]).Product, result);
		  assertEquals(pvIrs.getAmount(EUR).Amount, 0.0, TOLERANCE_PV);
		}
		// Test PV Fwd3
		ImmutableList<CurveNode> fwd3Nodes = definitions.get(1).Nodes;
		IList<ResolvedTrade> fwd3Trades = new List<ResolvedTrade>();
		for (int i = 0; i < fwd3Nodes.size(); i++)
		{
		  fwd3Trades.Add(fwd3Nodes.get(i).resolvedTrade(1d, allQuotes, REF_DATA));
		}
		// FRA
		for (int i = 0; i < FWD3_FRA_QUOTES.Length; i++)
		{
		  CurrencyAmount pvFra = PRICER_FRA.presentValue(((ResolvedFraTrade) fwd3Trades[i + 1]).Product, result);
		  assertEquals(pvFra.Amount, 0.0, TOLERANCE_PV);
		}
		// IRS
		for (int i = 0; i < FWD3_IRS_QUOTES.Length; i++)
		{
		  MultiCurrencyAmount pvIrs = SWAP_PRICER.presentValue(((ResolvedSwapTrade) fwd3Trades[i + 1 + FWD3_FRA_QUOTES.Length]).Product, result);
		  assertEquals(pvIrs.getAmount(EUR).Amount, 0.0, TOLERANCE_PV);
		}
		// Test PV Fwd6
		ImmutableList<CurveNode> fwd6Nodes = definitions.get(2).Nodes;
		IList<ResolvedTrade> fwd6Trades = new List<ResolvedTrade>();
		for (int i = 0; i < fwd6Nodes.size(); i++)
		{
		  fwd6Trades.Add(fwd6Nodes.get(i).resolvedTrade(1d, allQuotes, REF_DATA));
		}
		// IRS
		for (int i = 0; i < FWD6_IRS_QUOTES.Length; i++)
		{
		  MultiCurrencyAmount pvIrs = SWAP_PRICER.presentValue(((ResolvedSwapTrade) fwd6Trades[i + 1 + FWD6_FRA_QUOTES.Length]).Product, result);
		  assertEquals(pvIrs.getAmount(EUR).Amount, 0.0, TOLERANCE_PV);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void calibration_transition_coherence_par_rate()
	  {
		RatesProvider provider = CalibrationEurStandard.calibrateEurStandard(VAL_DATE, DSC_MARKET_QUOTES, DSC_OIS_TENORS, FWD3_FIXING_QUOTE, FWD3_FRA_QUOTES, FWD3_IRS_QUOTES, FWD3_FRA_TENORS, FWD3_IRS_TENORS, FWD6_FIXING_QUOTE, FWD6_FRA_QUOTES, FWD6_IRS_QUOTES, FWD6_FRA_TENORS, FWD6_IRS_TENORS);

		/* Curve Discounting/EUR-EONIA */
		string[] dscIdValues = CalibrationEurStandard.dscIdValues(DSC_OIS_TENORS);
		/* Curve EUR-EURIBOR-3M */
		double[] fwd3MarketQuotes = CalibrationEurStandard.fwdMarketQuotes(FWD3_FIXING_QUOTE, FWD3_FRA_QUOTES, FWD3_IRS_QUOTES);
		string[] fwd3IdValue = CalibrationEurStandard.fwdIdValue(3, FWD3_FIXING_QUOTE, FWD3_FRA_QUOTES, FWD3_IRS_QUOTES, FWD3_FRA_TENORS, FWD3_IRS_TENORS);
		/* Curve EUR-EURIBOR-6M */
		double[] fwd6MarketQuotes = CalibrationEurStandard.fwdMarketQuotes(FWD6_FIXING_QUOTE, FWD6_FRA_QUOTES, FWD6_IRS_QUOTES);
		string[] fwd6IdValue = CalibrationEurStandard.fwdIdValue(6, FWD6_FIXING_QUOTE, FWD6_FRA_QUOTES, FWD6_IRS_QUOTES, FWD6_FRA_TENORS, FWD6_IRS_TENORS);
		/* All quotes for the curve calibration */
		MarketData allQuotes = CalibrationEurStandard.allQuotes(VAL_DATE, DSC_MARKET_QUOTES, dscIdValues, fwd3MarketQuotes, fwd3IdValue, fwd6MarketQuotes, fwd6IdValue);
		/* All nodes by groups. */
		RatesCurveGroupDefinition config = CalibrationEurStandard.config(DSC_OIS_TENORS, dscIdValues, FWD3_FRA_TENORS, FWD3_IRS_TENORS, fwd3IdValue, FWD6_FRA_TENORS, FWD6_IRS_TENORS, fwd6IdValue);

		ImmutableList<CurveDefinition> definitions = config.CurveDefinitions;
		// Test PV Dsc
		ImmutableList<CurveNode> dscNodes = definitions.get(0).Nodes;
		IList<ResolvedTrade> dscTrades = new List<ResolvedTrade>();
		for (int i = 0; i < dscNodes.size(); i++)
		{
		  dscTrades.Add(dscNodes.get(i).resolvedTrade(1d, allQuotes, REF_DATA));
		}
		// OIS
		for (int loopnode = 0; loopnode < DSC_MARKET_QUOTES.Length; loopnode++)
		{
		  PointSensitivities pts = SWAP_PRICER.parRateSensitivity(((ResolvedSwapTrade) dscTrades[loopnode]).Product, provider).build();
		  CurrencyParameterSensitivities ps = provider.parameterSensitivity(pts);
		  CurrencyParameterSensitivities mqs = MQC.sensitivity(ps, provider);
		  assertEquals(mqs.size(), 3); // Calibration of all curves simultaneously
		  CurrencyParameterSensitivity mqsDsc = mqs.getSensitivity(CalibrationEurStandard.DSCON_CURVE_NAME, EUR);
		  assertTrue(mqsDsc.MarketDataName.Equals(CalibrationEurStandard.DSCON_CURVE_NAME));
		  assertTrue(mqsDsc.Currency.Equals(EUR));
		  DoubleArray mqsData = mqsDsc.Sensitivity;
		  assertEquals(mqsData.size(), DSC_MARKET_QUOTES.Length);
		  for (int i = 0; i < mqsData.size(); i++)
		  {
			assertEquals(mqsData.get(i), (i == loopnode) ? 1.0 : 0.0, TOLERANCE_DELTA);
		  }
		}
		// Test PV Fwd3
		ImmutableList<CurveNode> fwd3Nodes = definitions.get(1).Nodes;
		IList<ResolvedTrade> fwd3Trades = new List<ResolvedTrade>();
		for (int i = 0; i < fwd3Nodes.size(); i++)
		{
		  fwd3Trades.Add(fwd3Nodes.get(i).resolvedTrade(1d, allQuotes, REF_DATA));
		}
		for (int loopnode = 0; loopnode < fwd3MarketQuotes.Length; loopnode++)
		{
		  PointSensitivities pts = null;
		  if (fwd3Trades[loopnode] is ResolvedIborFixingDepositTrade)
		  {
			pts = PRICER_FIXING.parSpreadSensitivity(((ResolvedIborFixingDepositTrade) fwd3Trades[loopnode]).Product, provider);
		  }
		  if (fwd3Trades[loopnode] is ResolvedFraTrade)
		  {
			pts = PRICER_FRA.parSpreadSensitivity(((ResolvedFraTrade) fwd3Trades[loopnode]).Product, provider);
		  }
		  if (fwd3Trades[loopnode] is ResolvedSwapTrade)
		  {
			pts = SWAP_PRICER.parSpreadSensitivity(((ResolvedSwapTrade) fwd3Trades[loopnode]).Product, provider).build();
		  }
		  CurrencyParameterSensitivities ps = provider.parameterSensitivity(pts);
		  CurrencyParameterSensitivities mqs = MQC.sensitivity(ps, provider);
		  assertEquals(mqs.size(), 3); // Calibration of all curves simultaneously
		  CurrencyParameterSensitivity mqsDsc = mqs.getSensitivity(CalibrationEurStandard.DSCON_CURVE_NAME, EUR);
		  CurrencyParameterSensitivity mqsFwd3 = mqs.getSensitivity(CalibrationEurStandard.FWD3_CURVE_NAME, EUR);
		  DoubleArray mqsDscData = mqsDsc.Sensitivity;
		  assertEquals(mqsDscData.size(), DSC_MARKET_QUOTES.Length);
		  for (int i = 0; i < mqsDscData.size(); i++)
		  {
			assertEquals(mqsDscData.get(i), 0.0, TOLERANCE_DELTA);
		  }
		  DoubleArray mqsFwd3Data = mqsFwd3.Sensitivity;
		  assertEquals(mqsFwd3Data.size(), fwd3MarketQuotes.Length);
		  for (int i = 0; i < mqsFwd3Data.size(); i++)
		  {
			assertEquals(mqsFwd3Data.get(i), (i == loopnode) ? 1.0 : 0.0, TOLERANCE_DELTA);
		  }
		}
		// Test PV Fwd6
		ImmutableList<CurveNode> fwd6Nodes = definitions.get(2).Nodes;
		IList<ResolvedTrade> fwd6Trades = new List<ResolvedTrade>();
		for (int i = 0; i < fwd6Nodes.size(); i++)
		{
		  fwd6Trades.Add(fwd6Nodes.get(i).resolvedTrade(1d, allQuotes, REF_DATA));
		}
		for (int loopnode = 0; loopnode < fwd6MarketQuotes.Length; loopnode++)
		{
		  PointSensitivities pts = null;
		  if (fwd6Trades[loopnode] is ResolvedIborFixingDepositTrade)
		  {
			pts = PRICER_FIXING.parSpreadSensitivity(((ResolvedIborFixingDepositTrade) fwd6Trades[loopnode]).Product, provider);
		  }
		  if (fwd6Trades[loopnode] is ResolvedFraTrade)
		  {
			pts = PRICER_FRA.parSpreadSensitivity(((ResolvedFraTrade) fwd6Trades[loopnode]).Product, provider);
		  }
		  if (fwd6Trades[loopnode] is ResolvedSwapTrade)
		  {
			pts = SWAP_PRICER.parSpreadSensitivity(((ResolvedSwapTrade) fwd6Trades[loopnode]).Product, provider).build();
		  }
		  CurrencyParameterSensitivities ps = provider.parameterSensitivity(pts);
		  CurrencyParameterSensitivities mqs = MQC.sensitivity(ps, provider);
		  assertEquals(mqs.size(), 3);
		  CurrencyParameterSensitivity mqsDsc = mqs.getSensitivity(CalibrationEurStandard.DSCON_CURVE_NAME, EUR);
		  CurrencyParameterSensitivity mqsFwd3 = mqs.getSensitivity(CalibrationEurStandard.FWD3_CURVE_NAME, EUR);
		  CurrencyParameterSensitivity mqsFwd6 = mqs.getSensitivity(CalibrationEurStandard.FWD6_CURVE_NAME, EUR);
		  DoubleArray mqsDscData = mqsDsc.Sensitivity;
		  assertEquals(mqsDscData.size(), DSC_MARKET_QUOTES.Length);
		  for (int i = 0; i < mqsDscData.size(); i++)
		  {
			assertEquals(mqsDscData.get(i), 0.0, TOLERANCE_DELTA);
		  }
		  DoubleArray mqsFwd3Data = mqsFwd3.Sensitivity;
		  assertEquals(mqsFwd3Data.size(), fwd3MarketQuotes.Length);
		  for (int i = 0; i < mqsFwd3Data.size(); i++)
		  {
			assertEquals(mqsFwd3Data.get(i), 0.0, TOLERANCE_DELTA);
		  }
		  DoubleArray mqsFwd6Data = mqsFwd6.Sensitivity;
		  assertEquals(mqsFwd6Data.size(), fwd6MarketQuotes.Length);
		  for (int i = 0; i < mqsFwd6Data.size(); i++)
		  {
			assertEquals(mqsFwd6Data.get(i), (i == loopnode) ? 1.0 : 0.0, TOLERANCE_DELTA);
		  }
		}

	  }

	}

}