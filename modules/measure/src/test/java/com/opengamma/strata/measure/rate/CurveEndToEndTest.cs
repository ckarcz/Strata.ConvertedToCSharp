using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.rate
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.CollectProjectAssertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.measure.StandardComponents.marketDataFactory;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.measure.rate.CurveTestUtils.fixedIborSwapNode;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.measure.rate.CurveTestUtils.fraNode;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.measure.rate.CurveTestUtils.id;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.offset;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using MoreExecutors = com.google.common.util.concurrent.MoreExecutors;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using Column = com.opengamma.strata.calc.Column;
	using Results = com.opengamma.strata.calc.Results;
	using MarketDataConfig = com.opengamma.strata.calc.marketdata.MarketDataConfig;
	using MarketDataFactory = com.opengamma.strata.calc.marketdata.MarketDataFactory;
	using MarketDataRequirements = com.opengamma.strata.calc.marketdata.MarketDataRequirements;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using CalculationTaskRunner = com.opengamma.strata.calc.runner.CalculationTaskRunner;
	using CalculationTasks = com.opengamma.strata.calc.runner.CalculationTasks;
	using Result = com.opengamma.strata.collect.result.Result;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurveNode = com.opengamma.strata.market.curve.CurveNode;
	using InterpolatedNodalCurveDefinition = com.opengamma.strata.market.curve.InterpolatedNodalCurveDefinition;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using FixedIborSwapCurveNode = com.opengamma.strata.market.curve.node.FixedIborSwapCurveNode;
	using FraCurveNode = com.opengamma.strata.market.curve.node.FraCurveNode;
	using FraTradeCalculationFunction = com.opengamma.strata.measure.fra.FraTradeCalculationFunction;
	using SwapTradeCalculationFunction = com.opengamma.strata.measure.swap.SwapTradeCalculationFunction;
	using Trade = com.opengamma.strata.product.Trade;
	using FraTrade = com.opengamma.strata.product.fra.FraTrade;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;

	/// <summary>
	/// Test curves.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurveEndToEndTest
	public class CurveEndToEndTest
	{

	  /// <summary>
	  /// The maximum allowable PV when round-tripping an instrument used to calibrate a curve. </summary>
	  private const double PV_TOLERANCE = 5e-10;
	  /// <summary>
	  /// The reference data. </summary>
	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  /// <summary>
	  /// End-to-end test for curve calibration and round-tripping that uses the <seealso cref="MarketDataFactory"/>
	  /// to calibrate a curve and calculate PVs for the instruments at the curve nodes.
	  /// 
	  /// This tests the full pipeline of market data functions:
	  ///   - Par rates
	  ///   - Curve group (including calibration)
	  ///   - Individual curves
	  ///   - Discount factors
	  /// </summary>
	  public virtual void roundTripFraAndFixedFloatSwap()
	  {

		// Configuration and market data for the curve ---------------------------------

		string fra3x6 = "fra3x6";
		string fra6x9 = "fra6x9";
		string swap1y = "swap1y";
		string swap2y = "swap2y";
		string swap3y = "swap3y";

		FraCurveNode fra3x6Node = fraNode(3, fra3x6);
		FraCurveNode fra6x9Node = fraNode(6, fra6x9);
		FixedIborSwapCurveNode swap1yNode = fixedIborSwapNode(Tenor.TENOR_1Y, swap1y);
		FixedIborSwapCurveNode swap2yNode = fixedIborSwapNode(Tenor.TENOR_2Y, swap2y);
		FixedIborSwapCurveNode swap3yNode = fixedIborSwapNode(Tenor.TENOR_3Y, swap3y);

		IDictionary<ObservableId, double> parRateData = ImmutableMap.builder<ObservableId, double>().put(id(fra3x6), 0.0037).put(id(fra6x9), 0.0054).put(id(swap1y), 0.005).put(id(swap2y), 0.0087).put(id(swap3y), 0.012).build();

		LocalDate valuationDate = date(2011, 3, 8);

		// Build the trades from the node instruments
		MarketData quotes = ImmutableMarketData.of(valuationDate, parRateData);
		Trade fra3x6Trade = fra3x6Node.trade(1d, quotes, REF_DATA);
		Trade fra6x9Trade = fra6x9Node.trade(1d, quotes, REF_DATA);
		Trade swap1yTrade = swap1yNode.trade(1d, quotes, REF_DATA);
		Trade swap2yTrade = swap2yNode.trade(1d, quotes, REF_DATA);
		Trade swap3yTrade = swap3yNode.trade(1d, quotes, REF_DATA);

		IList<Trade> trades = ImmutableList.of(fra3x6Trade, fra6x9Trade, swap1yTrade, swap2yTrade, swap3yTrade);

		IList<CurveNode> nodes = ImmutableList.of(fra3x6Node, fra6x9Node, swap1yNode, swap2yNode, swap3yNode);
		CurveGroupName groupName = CurveGroupName.of("Curve Group");
		CurveName curveName = CurveName.of("FRA and Fixed-Float Swap Curve");

		InterpolatedNodalCurveDefinition curveDefn = InterpolatedNodalCurveDefinition.builder().name(curveName).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(DayCounts.ACT_ACT_ISDA).nodes(nodes).interpolator(CurveInterpolators.DOUBLE_QUADRATIC).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();

		RatesCurveGroupDefinition groupDefn = RatesCurveGroupDefinition.builder().name(groupName).addCurve(curveDefn, Currency.USD, IborIndices.USD_LIBOR_3M).build();

		MarketDataConfig marketDataConfig = MarketDataConfig.builder().add(groupName, groupDefn).build();

		// Rules for market data and calculations ---------------------------------

		RatesMarketDataLookup ratesLookup = RatesMarketDataLookup.of(groupDefn);
		CalculationRules calculationRules = CalculationRules.of(functions(), Currency.USD, ratesLookup);

		// Calculate the results and check the PVs for the node instruments are zero ----------------------

		IList<Column> columns = ImmutableList.of(Column.of(Measures.PRESENT_VALUE));
		MarketData knownMarketData = MarketData.of(date(2011, 3, 8), parRateData);

		// using the direct executor means there is no need to close/shutdown the runner
		CalculationTasks tasks = CalculationTasks.of(calculationRules, trades, columns, REF_DATA);
		MarketDataRequirements reqs = tasks.requirements(REF_DATA);
		MarketData enhancedMarketData = marketDataFactory().create(reqs, marketDataConfig, knownMarketData, REF_DATA);
		CalculationTaskRunner runner = CalculationTaskRunner.of(MoreExecutors.newDirectExecutorService());
		Results results = runner.calculate(tasks, enhancedMarketData, REF_DATA);

		results.Cells.ForEach(this.checkPvIsZero);
	  }

	  private void checkPvIsZero<T1>(Result<T1> result)
	  {
		assertThat(result).Success;
		assertThat(((CurrencyAmount) result.Value).Amount).isEqualTo(0, offset(PV_TOLERANCE));
	  }

	  //-----------------------------------------------------------------------------------------------------------
	  private static CalculationFunctions functions()
	  {
		return CalculationFunctions.of(ImmutableMap.of(typeof(SwapTrade), new SwapTradeCalculationFunction(), typeof(FraTrade), new FraTradeCalculationFunction()));
	  }

	}

}