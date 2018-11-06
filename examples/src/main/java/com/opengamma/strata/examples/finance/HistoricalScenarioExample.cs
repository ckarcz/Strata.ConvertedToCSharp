using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.finance
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.measure.StandardComponents.marketDataFactory;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using CalculationRunner = com.opengamma.strata.calc.CalculationRunner;
	using Column = com.opengamma.strata.calc.Column;
	using Results = com.opengamma.strata.calc.Results;
	using MarketDataConfig = com.opengamma.strata.calc.marketdata.MarketDataConfig;
	using MarketDataFilter = com.opengamma.strata.calc.marketdata.MarketDataFilter;
	using MarketDataRequirements = com.opengamma.strata.calc.marketdata.MarketDataRequirements;
	using PerturbationMapping = com.opengamma.strata.calc.marketdata.PerturbationMapping;
	using ScenarioDefinition = com.opengamma.strata.calc.marketdata.ScenarioDefinition;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using Messages = com.opengamma.strata.collect.Messages;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using ExampleMarketDataBuilder = com.opengamma.strata.examples.marketdata.ExampleMarketDataBuilder;
	using ShiftType = com.opengamma.strata.market.ShiftType;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using RatesCurveGroup = com.opengamma.strata.market.curve.RatesCurveGroup;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using ParameterizedData = com.opengamma.strata.market.param.ParameterizedData;
	using PointShifts = com.opengamma.strata.market.param.PointShifts;
	using PointShiftsBuilder = com.opengamma.strata.market.param.PointShiftsBuilder;
	using Measures = com.opengamma.strata.measure.Measures;
	using StandardComponents = com.opengamma.strata.measure.StandardComponents;
	using Trade = com.opengamma.strata.product.Trade;
	using AttributeType = com.opengamma.strata.product.AttributeType;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using IborRateCalculation = com.opengamma.strata.product.swap.IborRateCalculation;
	using NotionalSchedule = com.opengamma.strata.product.swap.NotionalSchedule;
	using PaymentSchedule = com.opengamma.strata.product.swap.PaymentSchedule;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;

	/// <summary>
	/// Example to illustrate using the engine to run a set of historical scenarios on a single swap
	/// to produce a P&L series. This P&L series could then be used to calculate historical VaR.
	/// <para>
	/// In this example we are provided with market data containing:
	/// <li>a complete snapshot to value the swap on the valuation date (curves only as the swap is forward-starting)
	/// <li>a series of historical curves for every date leading up to the valuation date
	/// </para>
	/// <para>
	/// The differences between the zero rates in consecutive historical curves (dates d-1 and d)
	/// are used to generate a scenario, later attributed to date d, containing these relative curve
	/// shifts. The swap is then valued on the valuation date, applying each scenario to the base
	/// snapshot from the valuation date, to produce a PV series. A P&L series is then generated from
	/// this.
	/// </para>
	/// <para>
	/// Instead of generating the perturbations on-the-fly from real data as in this example, the
	/// scenario could be pre-generated and stored, or generated in any other way.
	/// </para>
	/// </summary>
	public class HistoricalScenarioExample
	{

	  private const string MARKET_DATA_RESOURCE_ROOT = "example-historicalscenario-marketdata";

	  public static void Main(string[] args)
	  {
		// setup calculation runner component, which needs life-cycle management
		// a typical application might use dependency injection to obtain the instance
		using (CalculationRunner runner = CalculationRunner.ofMultiThreaded())
		{
		  calculate(runner);
		}
	  }

	  // obtains the data and calculates the grid of results
	  private static void calculate(CalculationRunner runner)
	  {
		// the trades for which to calculate a P&L series
		IList<Trade> trades = ImmutableList.of(createTrade());

		// the columns, specifying the measures to be calculated
		IList<Column> columns = ImmutableList.of(Column.of(Measures.PRESENT_VALUE));

		// use the built-in example historical scenario market data
		ExampleMarketDataBuilder marketDataBuilder = ExampleMarketDataBuilder.ofResource(MARKET_DATA_RESOURCE_ROOT);

		// the complete set of rules for calculating measures
		CalculationFunctions functions = StandardComponents.calculationFunctions();
		CalculationRules rules = CalculationRules.of(functions, marketDataBuilder.ratesLookup(LocalDate.of(2015, 4, 23)));

		// load the historical calibrated curves from which we will build our scenarios
		// these curves are provided in the example data environment
		SortedDictionary<LocalDate, RatesCurveGroup> historicalCurves = marketDataBuilder.loadAllRatesCurves();

		// sorted list of dates for the available series of curves
		// the entries in the P&L vector we produce will correspond to these dates
		IList<LocalDate> scenarioDates = new List<LocalDate>(historicalCurves.Keys);

		// build the historical scenarios
		ScenarioDefinition historicalScenarios = buildHistoricalScenarios(historicalCurves, scenarioDates);

		// build a market data snapshot for the valuation date
		// this is the base snapshot which will be perturbed by the scenarios
		LocalDate valuationDate = LocalDate.of(2015, 4, 23);
		MarketData marketData = marketDataBuilder.buildSnapshot(valuationDate);

		// the reference data, such as holidays and securities
		ReferenceData refData = ReferenceData.standard();

		// calculate the results
		MarketDataRequirements reqs = MarketDataRequirements.of(rules, trades, columns, refData);
		ScenarioMarketData scenarioMarketData = marketDataFactory().createMultiScenario(reqs, MarketDataConfig.empty(), marketData, refData, historicalScenarios);
		Results results = runner.calculateMultiScenario(rules, trades, columns, scenarioMarketData, refData);

		// the results contain the one measure requested (Present Value) for each scenario
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.scenario.ScenarioArray<?> scenarioValuations = (com.opengamma.strata.data.scenario.ScenarioArray<?>) results.get(0, 0).getValue();
		ScenarioArray<object> scenarioValuations = (ScenarioArray<object>) results.get(0, 0).Value;
		outputPnl(scenarioDates, scenarioValuations);
	  }

	  private static ScenarioDefinition buildHistoricalScenarios(IDictionary<LocalDate, RatesCurveGroup> historicalCurves, IList<LocalDate> scenarioDates)
	  {

		// extract the curves to perturb
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<Curve> usdDiscountCurves = scenarioDates.Select(date => historicalCurves[date]).Select(group => group.findDiscountCurve(Currency.USD).get()).collect(toImmutableList());

//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<Curve> libor3mCurves = scenarioDates.Select(date => historicalCurves[date]).Select(group => group.findForwardCurve(IborIndices.USD_LIBOR_3M).get()).collect(toImmutableList());

//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<Curve> libor6mCurves = scenarioDates.Select(date => historicalCurves[date]).Select(group => group.findForwardCurve(IborIndices.USD_LIBOR_6M).get()).collect(toImmutableList());

		// create mappings which will cause the point shift perturbations generated above
		// to be applied to the correct curves
		PerturbationMapping<ParameterizedData> discountCurveMappings = PerturbationMapping.of(MarketDataFilter.ofName(CurveName.of("USD-Disc")), buildShifts(usdDiscountCurves));

		PerturbationMapping<ParameterizedData> libor3mMappings = PerturbationMapping.of(MarketDataFilter.ofName(CurveName.of("USD-3ML")), buildShifts(libor3mCurves));

		PerturbationMapping<ParameterizedData> libor6mMappings = PerturbationMapping.of(MarketDataFilter.ofName(CurveName.of("USD-6ML")), buildShifts(libor6mCurves));

		// create a scenario definition from these mappings
		return ScenarioDefinition.ofMappings(discountCurveMappings, libor3mMappings, libor6mMappings);
	  }

	  private static PointShifts buildShifts(IList<Curve> historicalCurves)
	  {
		PointShiftsBuilder builder = PointShifts.builder(ShiftType.ABSOLUTE);

		for (int scenarioIndex = 1; scenarioIndex < historicalCurves.Count; scenarioIndex++)
		{
		  Curve previousCurve = historicalCurves[scenarioIndex - 1];
		  Curve curve = historicalCurves[scenarioIndex];

		  // build up the shifts to apply to each node
		  // these are calculated as the actual change in the zero rate at that node between the two scenario dates
		  for (int curveNodeIdx = 0; curveNodeIdx < curve.ParameterCount; curveNodeIdx++)
		  {
			double zeroRate = curve.getParameter(curveNodeIdx);
			double previousZeroRate = previousCurve.getParameter(curveNodeIdx);
			double shift = (zeroRate - previousZeroRate);
			// the parameter metadata is used to identify a node to apply a perturbation to
			builder.addShift(scenarioIndex, curve.getParameterMetadata(curveNodeIdx).Identifier, shift);
		  }
		}
		return builder.build();
	  }

	  private static void outputPnl<T1>(IList<LocalDate> scenarioDates, ScenarioArray<T1> scenarioValuations)
	  {
		NumberFormat numberFormat = new DecimalFormat("0.00", new DecimalFormatSymbols(Locale.ENGLISH));
		double basePv = ((CurrencyAmount) scenarioValuations.get(0)).Amount;
		Console.WriteLine("Base PV (USD): " + numberFormat.format(basePv));
		Console.WriteLine();
		Console.WriteLine("P&L series (USD):");
		for (int i = 1; i < scenarioValuations.ScenarioCount; i++)
		{
		  double scenarioPv = ((CurrencyAmount) scenarioValuations.get(i)).Amount;
		  double pnl = scenarioPv - basePv;
		  LocalDate scenarioDate = scenarioDates[i];
		  Console.WriteLine(Messages.format("{} = {}", scenarioDate, numberFormat.format(pnl)));
		}
	  }

	  //-------------------------------------------------------------------------
	  // create a libor 3m vs libor 6m swap
	  private static Trade createTrade()
	  {
		NotionalSchedule notional = NotionalSchedule.of(Currency.USD, 1_000_000);

		SwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.PAY).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2015, 9, 11)).endDate(LocalDate.of(2021, 9, 11)).frequency(Frequency.P3M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(IborRateCalculation.of(IborIndices.USD_LIBOR_3M)).build();

		SwapLeg receiveLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2015, 9, 11)).endDate(LocalDate.of(2021, 9, 11)).frequency(Frequency.P6M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(IborRateCalculation.of(IborIndices.USD_LIBOR_6M)).build();

		return SwapTrade.builder().product(Swap.of(payLeg, receiveLeg)).info(TradeInfo.builder().id(StandardId.of("example", "1")).addAttribute(AttributeType.DESCRIPTION, "Libor 3m vs Libor 6m").counterparty(StandardId.of("example", "A")).settlementDate(LocalDate.of(2015, 9, 11)).build()).build();
	  }

	}

}