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
//	import static com.opengamma.strata.measure.StandardComponents.marketDataFactory;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
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
	using MarketData = com.opengamma.strata.data.MarketData;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using ExampleMarketData = com.opengamma.strata.examples.marketdata.ExampleMarketData;
	using ExampleMarketDataBuilder = com.opengamma.strata.examples.marketdata.ExampleMarketDataBuilder;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using CurveParallelShifts = com.opengamma.strata.market.curve.CurveParallelShifts;
	using Measures = com.opengamma.strata.measure.Measures;
	using StandardComponents = com.opengamma.strata.measure.StandardComponents;
	using Trade = com.opengamma.strata.product.Trade;
	using AttributeType = com.opengamma.strata.product.AttributeType;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using FixedRateCalculation = com.opengamma.strata.product.swap.FixedRateCalculation;
	using IborRateCalculation = com.opengamma.strata.product.swap.IborRateCalculation;
	using NotionalSchedule = com.opengamma.strata.product.swap.NotionalSchedule;
	using PaymentSchedule = com.opengamma.strata.product.swap.PaymentSchedule;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;

	/// <summary>
	/// Example to illustrate using the scenario framework to apply shifts to calibrated curves.
	/// <para>
	/// Two scenarios are run:
	/// <ul>
	///   <li>A base scenario with no perturbations applied to the market data</li>
	///   <li>A scenario with a 1 basis point shift applied to all curves</li>
	/// </ul>
	/// Present value and PV01 are calculated for a single swap. The present value from the second scenario
	/// is compared to the sum of the present value and PV01 from the base scenario.
	/// </para>
	/// <para>
	/// This makes use of the example engine and the example market data environment.
	/// </para>
	/// </summary>
	public class CurveScenarioExample
	{

	  private const double ONE_BP = 1e-4;

	  /// <summary>
	  /// Runs the example, pricing the instruments, producing the output as an ASCII table.
	  /// </summary>
	  /// <param name="args">  ignored </param>
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
		// the trade that will have measures calculated
		IList<Trade> trades = ImmutableList.of(createVanillaFixedVsLibor3mSwap());

		// the columns, specifying the measures to be calculated
		IList<Column> columns = ImmutableList.of(Column.of(Measures.PRESENT_VALUE), Column.of(Measures.PV01_CALIBRATED_SUM));

		// use the built-in example market data
		ExampleMarketDataBuilder marketDataBuilder = ExampleMarketData.builder();

		// the complete set of rules for calculating measures
		LocalDate valuationDate = LocalDate.of(2014, 1, 22);
		CalculationFunctions functions = StandardComponents.calculationFunctions();
		CalculationRules rules = CalculationRules.of(functions, Currency.USD, marketDataBuilder.ratesLookup(valuationDate));

		// mappings that select which market data to apply perturbations to
		// this applies the perturbations above to all curves
		PerturbationMapping<Curve> mapping = PerturbationMapping.of(MarketDataFilter.ofIdType(typeof(CurveId)), CurveParallelShifts.absolute(0, ONE_BP));

		// create a scenario definition containing the single mapping above
		// this creates two scenarios - one for each perturbation in the mapping
		ScenarioDefinition scenarioDefinition = ScenarioDefinition.ofMappings(mapping);

		// build a market data snapshot for the valuation date
		MarketData marketData = marketDataBuilder.buildSnapshot(valuationDate);

		// the reference data, such as holidays and securities
		ReferenceData refData = ReferenceData.standard();

		// calculate the results
		MarketDataRequirements reqs = MarketDataRequirements.of(rules, trades, columns, refData);
		ScenarioMarketData scenarioMarketData = marketDataFactory().createMultiScenario(reqs, MarketDataConfig.empty(), marketData, refData, scenarioDefinition);
		Results results = runner.calculateMultiScenario(rules, trades, columns, scenarioMarketData, refData);

		// TODO Replace the results processing below with a report once the reporting framework supports scenarios

		// The results are lists of currency amounts containing one value for each scenario
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.scenario.ScenarioArray<?> pvList = (com.opengamma.strata.data.scenario.ScenarioArray<?>) results.get(0, 0).getValue();
		ScenarioArray<object> pvList = (ScenarioArray<object>) results.get(0, 0).Value;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.scenario.ScenarioArray<?> pv01List = (com.opengamma.strata.data.scenario.ScenarioArray<?>) results.get(0, 1).getValue();
		ScenarioArray<object> pv01List = (ScenarioArray<object>) results.get(0, 1).Value;

		double pvBase = ((CurrencyAmount) pvList.get(0)).Amount;
		double pvShifted = ((CurrencyAmount) pvList.get(1)).Amount;
		double pv01Base = ((CurrencyAmount) pv01List.get(0)).Amount;
		NumberFormat numberFormat = new DecimalFormat("###,##0.00", new DecimalFormatSymbols(Locale.ENGLISH));

		Console.WriteLine("                         PV (base) = " + numberFormat.format(pvBase));
		Console.WriteLine("             PV (1 bp curve shift) = " + numberFormat.format(pvShifted));
		Console.WriteLine("PV01 (algorithmic differentiation) = " + numberFormat.format(pv01Base));
		Console.WriteLine("          PV01 (finite difference) = " + numberFormat.format(pvShifted - pvBase));
	  }

	  //-----------------------------------------------------------------------
	  // create a vanilla fixed vs libor 3m swap
	  private static Trade createVanillaFixedVsLibor3mSwap()
	  {
		NotionalSchedule notional = NotionalSchedule.of(Currency.USD, 100_000_000);

		SwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.PAY).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2021, 9, 12)).frequency(Frequency.P6M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(FixedRateCalculation.of(0.015, DayCounts.THIRTY_U_360)).build();

		SwapLeg receiveLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2021, 9, 12)).frequency(Frequency.P3M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(IborRateCalculation.of(IborIndices.USD_LIBOR_3M)).build();

		return SwapTrade.builder().product(Swap.of(payLeg, receiveLeg)).info(TradeInfo.builder().addAttribute(AttributeType.DESCRIPTION, "Fixed vs Libor 3m").counterparty(StandardId.of("example", "A")).settlementDate(LocalDate.of(2014, 9, 12)).build()).build();
	  }
	}

}