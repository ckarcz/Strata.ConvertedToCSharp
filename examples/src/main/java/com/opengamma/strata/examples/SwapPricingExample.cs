using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using OvernightIndices = com.opengamma.strata.basics.index.OvernightIndices;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using CalculationRunner = com.opengamma.strata.calc.CalculationRunner;
	using Column = com.opengamma.strata.calc.Column;
	using Results = com.opengamma.strata.calc.Results;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ExampleData = com.opengamma.strata.examples.marketdata.ExampleData;
	using ExampleMarketData = com.opengamma.strata.examples.marketdata.ExampleMarketData;
	using ExampleMarketDataBuilder = com.opengamma.strata.examples.marketdata.ExampleMarketDataBuilder;
	using AdvancedMeasures = com.opengamma.strata.measure.AdvancedMeasures;
	using Measures = com.opengamma.strata.measure.Measures;
	using StandardComponents = com.opengamma.strata.measure.StandardComponents;
	using Trade = com.opengamma.strata.product.Trade;
	using AttributeType = com.opengamma.strata.product.AttributeType;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using CompoundingMethod = com.opengamma.strata.product.swap.CompoundingMethod;
	using FixedRateCalculation = com.opengamma.strata.product.swap.FixedRateCalculation;
	using IborRateCalculation = com.opengamma.strata.product.swap.IborRateCalculation;
	using IborRateStubCalculation = com.opengamma.strata.product.swap.IborRateStubCalculation;
	using NotionalSchedule = com.opengamma.strata.product.swap.NotionalSchedule;
	using OvernightAccrualMethod = com.opengamma.strata.product.swap.OvernightAccrualMethod;
	using OvernightRateCalculation = com.opengamma.strata.product.swap.OvernightRateCalculation;
	using PaymentSchedule = com.opengamma.strata.product.swap.PaymentSchedule;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;
	using FixedIborSwapConventions = com.opengamma.strata.product.swap.type.FixedIborSwapConventions;
	using FixedOvernightSwapConventions = com.opengamma.strata.product.swap.type.FixedOvernightSwapConventions;
	using ReportCalculationResults = com.opengamma.strata.report.ReportCalculationResults;
	using TradeReport = com.opengamma.strata.report.trade.TradeReport;
	using TradeReportTemplate = com.opengamma.strata.report.trade.TradeReportTemplate;

	/// <summary>
	/// Example to illustrate using the calculation API to price a swap.
	/// <para>
	/// This makes use of the example market data environment.
	/// </para>
	/// </summary>
	public class SwapPricingExample
	{

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
		// the trades that will have measures calculated
		IList<Trade> trades = createSwapTrades();

		// the columns, specifying the measures to be calculated
		IList<Column> columns = ImmutableList.of(Column.of(Measures.LEG_INITIAL_NOTIONAL), Column.of(Measures.PRESENT_VALUE), Column.of(Measures.LEG_PRESENT_VALUE), Column.of(Measures.PV01_CALIBRATED_SUM), Column.of(Measures.PAR_RATE), Column.of(Measures.ACCRUED_INTEREST), Column.of(Measures.PV01_CALIBRATED_BUCKETED), Column.of(AdvancedMeasures.PV01_SEMI_PARALLEL_GAMMA_BUCKETED));

		// use the built-in example market data
		LocalDate valuationDate = LocalDate.of(2014, 1, 22);
		ExampleMarketDataBuilder marketDataBuilder = ExampleMarketData.builder();
		MarketData marketData = marketDataBuilder.buildSnapshot(valuationDate);

		// the complete set of rules for calculating measures
		CalculationFunctions functions = StandardComponents.calculationFunctions();
		CalculationRules rules = CalculationRules.of(functions, marketDataBuilder.ratesLookup(valuationDate));

		// the reference data, such as holidays and securities
		ReferenceData refData = ReferenceData.standard();

		// calculate the results
		Results results = runner.calculate(rules, trades, columns, marketData, refData);

		// use the report runner to transform the engine results into a trade report
		ReportCalculationResults calculationResults = ReportCalculationResults.of(valuationDate, trades, columns, results, functions, refData);

		TradeReportTemplate reportTemplate = ExampleData.loadTradeReportTemplate("swap-report-template");
		TradeReport tradeReport = TradeReport.of(calculationResults, reportTemplate);
		tradeReport.writeAsciiTable(System.out);
	  }

	  //-----------------------------------------------------------------------  
	  // create swap trades
	  private static IList<Trade> createSwapTrades()
	  {
		return ImmutableList.of(createVanillaFixedVsLibor3mSwap(), createBasisLibor3mVsLibor6mWithSpreadSwap(), createOvernightAveragedWithSpreadVsLibor3mSwap(), createFixedVsLibor3mWithFixingSwap(), createFixedVsOvernightWithFixingSwap(), createStub3mFixedVsLibor3mSwap(), createStub1mFixedVsLibor3mSwap(), createInterpolatedStub3mFixedVsLibor6mSwap(), createInterpolatedStub4mFixedVsLibor6mSwap(), createZeroCouponFixedVsLibor3mSwap(), createCompoundingFixedVsFedFundsSwap(), createCompoundingFedFundsVsLibor3mSwap(), createCompoundingLibor6mVsLibor3mSwap(), createXCcyGbpLibor3mVsUsdLibor3mSwap(), createXCcyUsdFixedVsGbpLibor3mSwap(), createNotionalExchangeSwap());
	  }

	  //-----------------------------------------------------------------------  
	  // create a vanilla fixed vs libor 3m swap
	  private static Trade createVanillaFixedVsLibor3mSwap()
	  {
		TradeInfo tradeInfo = TradeInfo.builder().id(StandardId.of("example", "1")).addAttribute(AttributeType.DESCRIPTION, "Fixed vs Libor 3m").counterparty(StandardId.of("example", "A")).settlementDate(LocalDate.of(2014, 9, 12)).build();
		return FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M.toTrade(tradeInfo, LocalDate.of(2014, 9, 12), LocalDate.of(2021, 9, 12), BuySell.BUY, 100_000_000, 0.015); // the fixed interest rate
	  }

	  // create a libor 3m vs libor 6m basis swap with spread
	  private static Trade createBasisLibor3mVsLibor6mWithSpreadSwap()
	  {
		NotionalSchedule notional = NotionalSchedule.of(Currency.USD, 100_000_000);

		SwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.PAY).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 8, 27)).endDate(LocalDate.of(2024, 8, 27)).frequency(Frequency.P6M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(IborRateCalculation.of(IborIndices.USD_LIBOR_6M)).build();

		SwapLeg receiveLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 8, 27)).endDate(LocalDate.of(2024, 8, 27)).frequency(Frequency.P3M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(IborRateCalculation.builder().index(IborIndices.USD_LIBOR_3M).spread(ValueSchedule.of(0.001)).build()).build();

		return SwapTrade.builder().product(Swap.of(payLeg, receiveLeg)).info(TradeInfo.builder().id(StandardId.of("example", "2")).addAttribute(AttributeType.DESCRIPTION, "Libor 3m + spread vs Libor 6m").counterparty(StandardId.of("example", "A")).settlementDate(LocalDate.of(2014, 9, 12)).build()).build();
	  }

	  // Create an overnight averaged vs libor 3m swap with spread
	  private static Trade createOvernightAveragedWithSpreadVsLibor3mSwap()
	  {
		NotionalSchedule notional = NotionalSchedule.of(Currency.USD, 100_000_000);

		SwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.PAY).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2020, 9, 12)).frequency(Frequency.P3M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(IborRateCalculation.of(IborIndices.USD_LIBOR_3M)).build();

		SwapLeg receiveLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2020, 9, 12)).frequency(Frequency.P3M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(OvernightRateCalculation.builder().dayCount(DayCounts.ACT_360).index(OvernightIndices.USD_FED_FUND).accrualMethod(OvernightAccrualMethod.AVERAGED).spread(ValueSchedule.of(0.0025)).build()).build();

		return SwapTrade.builder().product(Swap.of(payLeg, receiveLeg)).info(TradeInfo.builder().id(StandardId.of("example", "3")).addAttribute(AttributeType.DESCRIPTION, "Fed Funds averaged + spread vs Libor 3m").counterparty(StandardId.of("example", "A")).settlementDate(LocalDate.of(2014, 9, 12)).build()).build();
	  }

	  // Create a vanilla fixed vs libor 3m swap with fixing
	  private static Trade createFixedVsLibor3mWithFixingSwap()
	  {
		TradeInfo tradeInfo = TradeInfo.builder().id(StandardId.of("example", "4")).addAttribute(AttributeType.DESCRIPTION, "Fixed vs libor 3m (with fixing)").counterparty(StandardId.of("example", "A")).settlementDate(LocalDate.of(2013, 9, 12)).build();
		return FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M.toTrade(tradeInfo, LocalDate.of(2013, 9, 12), LocalDate.of(2020, 9, 12), BuySell.BUY, 100_000_000, 0.015); // the fixed interest rate
	  }

	  // Create a fixed vs overnight swap with fixing
	  private static Trade createFixedVsOvernightWithFixingSwap()
	  {
		TradeInfo tradeInfo = TradeInfo.builder().id(StandardId.of("example", "5")).addAttribute(AttributeType.DESCRIPTION, "Fixed vs ON (with fixing)").counterparty(StandardId.of("example", "A")).settlementDate(LocalDate.of(2014, 1, 17)).build();
		return FixedOvernightSwapConventions.USD_FIXED_TERM_FED_FUND_OIS.toTrade(tradeInfo, LocalDate.of(2014, 1, 17), LocalDate.of(2014, 3, 17), BuySell.BUY, 100_000_000, 0.00123); // the fixed interest rate
	  }

	  // Create a fixed vs libor 3m swap
	  private static Trade createStub3mFixedVsLibor3mSwap()
	  {
		NotionalSchedule notional = NotionalSchedule.of(Currency.USD, 100_000_000);

		SwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.PAY).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2016, 6, 12)).frequency(Frequency.P3M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(IborRateCalculation.of(IborIndices.USD_LIBOR_3M)).build();

		SwapLeg receiveLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2016, 6, 12)).stubConvention(StubConvention.SHORT_INITIAL).frequency(Frequency.P6M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(FixedRateCalculation.of(0.01, DayCounts.THIRTY_U_360)).build();

		return SwapTrade.builder().product(Swap.of(payLeg, receiveLeg)).info(TradeInfo.builder().id(StandardId.of("example", "6")).addAttribute(AttributeType.DESCRIPTION, "Fixed vs Libor 3m (3m short initial stub)").counterparty(StandardId.of("example", "A")).settlementDate(LocalDate.of(2014, 9, 12)).build()).build();
	  }

	  // Create a fixed vs libor 3m swap
	  private static Trade createStub1mFixedVsLibor3mSwap()
	  {
		NotionalSchedule notional = NotionalSchedule.of(Currency.USD, 100_000_000);

		SwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.PAY).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2016, 7, 12)).frequency(Frequency.P3M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).stubConvention(StubConvention.SHORT_INITIAL).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(IborRateCalculation.of(IborIndices.USD_LIBOR_3M)).build();

		SwapLeg receiveLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2016, 7, 12)).stubConvention(StubConvention.SHORT_INITIAL).frequency(Frequency.P6M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(FixedRateCalculation.of(0.01, DayCounts.THIRTY_U_360)).build();

		return SwapTrade.builder().product(Swap.of(payLeg, receiveLeg)).info(TradeInfo.builder().id(StandardId.of("example", "7")).addAttribute(AttributeType.DESCRIPTION, "Fixed vs Libor 3m (1m short initial stub)").counterparty(StandardId.of("example", "A")).settlementDate(LocalDate.of(2014, 9, 12)).build()).build();
	  }

	  // Create a fixed vs libor 6m swap
	  private static Trade createInterpolatedStub3mFixedVsLibor6mSwap()
	  {
		NotionalSchedule notional = NotionalSchedule.of(Currency.USD, 100_000_000);

		SwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.PAY).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2016, 6, 12)).frequency(Frequency.P6M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).stubConvention(StubConvention.SHORT_INITIAL).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(IborRateCalculation.builder().index(IborIndices.USD_LIBOR_6M).initialStub(IborRateStubCalculation.ofIborInterpolatedRate(IborIndices.USD_LIBOR_3M, IborIndices.USD_LIBOR_6M)).build()).build();

		SwapLeg receiveLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2016, 6, 12)).stubConvention(StubConvention.SHORT_INITIAL).frequency(Frequency.P6M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(FixedRateCalculation.of(0.01, DayCounts.THIRTY_U_360)).build();

		return SwapTrade.builder().product(Swap.of(payLeg, receiveLeg)).info(TradeInfo.builder().id(StandardId.of("example", "8")).addAttribute(AttributeType.DESCRIPTION, "Fixed vs Libor 6m (interpolated 3m short initial stub)").counterparty(StandardId.of("example", "A")).settlementDate(LocalDate.of(2014, 9, 12)).build()).build();
	  }

	  // Create a fixed vs libor 6m swap
	  private static Trade createInterpolatedStub4mFixedVsLibor6mSwap()
	  {
		NotionalSchedule notional = NotionalSchedule.of(Currency.USD, 100_000_000);

		SwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.PAY).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2016, 7, 12)).frequency(Frequency.P6M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).stubConvention(StubConvention.SHORT_INITIAL).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(IborRateCalculation.builder().index(IborIndices.USD_LIBOR_6M).initialStub(IborRateStubCalculation.ofIborInterpolatedRate(IborIndices.USD_LIBOR_3M, IborIndices.USD_LIBOR_6M)).build()).build();

		SwapLeg receiveLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2016, 7, 12)).stubConvention(StubConvention.SHORT_INITIAL).frequency(Frequency.P6M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(FixedRateCalculation.of(0.01, DayCounts.THIRTY_U_360)).build();

		return SwapTrade.builder().product(Swap.of(payLeg, receiveLeg)).info(TradeInfo.builder().id(StandardId.of("example", "9")).addAttribute(AttributeType.DESCRIPTION, "Fixed vs Libor 6m (interpolated 4m short initial stub)").counterparty(StandardId.of("example", "A")).settlementDate(LocalDate.of(2014, 9, 12)).build()).build();
	  }

	  // Create a zero-coupon fixed vs libor 3m swap
	  private static Trade createZeroCouponFixedVsLibor3mSwap()
	  {
		NotionalSchedule notional = NotionalSchedule.of(Currency.USD, 100_000_000);

		SwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.PAY).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2021, 9, 12)).frequency(Frequency.P12M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.TERM).paymentDateOffset(DaysAdjustment.NONE).compoundingMethod(CompoundingMethod.STRAIGHT).build()).notionalSchedule(notional).calculation(FixedRateCalculation.of(0.015, DayCounts.THIRTY_U_360)).build();

		SwapLeg receiveLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2021, 9, 12)).frequency(Frequency.P3M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.TERM).paymentDateOffset(DaysAdjustment.NONE).compoundingMethod(CompoundingMethod.STRAIGHT).build()).notionalSchedule(notional).calculation(IborRateCalculation.of(IborIndices.USD_LIBOR_3M)).build();

		return SwapTrade.builder().product(Swap.of(payLeg, receiveLeg)).info(TradeInfo.builder().id(StandardId.of("example", "10")).addAttribute(AttributeType.DESCRIPTION, "Zero-coupon fixed vs libor 3m").counterparty(StandardId.of("example", "A")).settlementDate(LocalDate.of(2014, 9, 12)).build()).build();
	  }

	  // Create a compounding fixed vs fed funds swap
	  private static Trade createCompoundingFixedVsFedFundsSwap()
	  {
		NotionalSchedule notional = NotionalSchedule.of(Currency.USD, 100_000_000);

		SwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.PAY).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 2, 5)).endDate(LocalDate.of(2014, 4, 7)).frequency(Frequency.TERM).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.TERM).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(FixedRateCalculation.of(0.00123, DayCounts.ACT_360)).build();

		SwapLeg receiveLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 2, 5)).endDate(LocalDate.of(2014, 4, 7)).frequency(Frequency.TERM).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).stubConvention(StubConvention.SHORT_INITIAL).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.TERM).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(OvernightRateCalculation.of(OvernightIndices.USD_FED_FUND)).build();

		return SwapTrade.builder().product(Swap.of(payLeg, receiveLeg)).info(TradeInfo.builder().id(StandardId.of("example", "11")).addAttribute(AttributeType.DESCRIPTION, "Compounding fixed vs fed funds").counterparty(StandardId.of("example", "A")).settlementDate(LocalDate.of(2014, 2, 5)).build()).build();
	  }

	  // Create a compounding fed funds vs libor 3m swap
	  private static Trade createCompoundingFedFundsVsLibor3mSwap()
	  {
		NotionalSchedule notional = NotionalSchedule.of(Currency.USD, 100_000_000);

		SwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.PAY).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2020, 9, 12)).frequency(Frequency.P3M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(IborRateCalculation.of(IborIndices.USD_LIBOR_3M)).build();

		SwapLeg receiveLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2020, 9, 12)).frequency(Frequency.P3M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(OvernightRateCalculation.builder().index(OvernightIndices.USD_FED_FUND).accrualMethod(OvernightAccrualMethod.AVERAGED).build()).build();

		return SwapTrade.builder().product(Swap.of(payLeg, receiveLeg)).info(TradeInfo.builder().id(StandardId.of("example", "12")).addAttribute(AttributeType.DESCRIPTION, "Compounding fed funds vs libor 3m").counterparty(StandardId.of("example", "A")).settlementDate(LocalDate.of(2014, 9, 12)).build()).build();
	  }

	  // Create a compounding libor 6m vs libor 3m swap
	  private static Trade createCompoundingLibor6mVsLibor3mSwap()
	  {
		NotionalSchedule notional = NotionalSchedule.of(Currency.USD, 100_000_000);

		SwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.PAY).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 8, 27)).endDate(LocalDate.of(2024, 8, 27)).frequency(Frequency.P6M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(IborRateCalculation.of(IborIndices.USD_LIBOR_6M)).build();

		SwapLeg receiveLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 8, 27)).endDate(LocalDate.of(2024, 8, 27)).frequency(Frequency.P3M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.NONE).compoundingMethod(CompoundingMethod.STRAIGHT).build()).notionalSchedule(notional).calculation(IborRateCalculation.of(IborIndices.USD_LIBOR_3M)).build();

		return SwapTrade.builder().product(Swap.of(payLeg, receiveLeg)).info(TradeInfo.builder().id(StandardId.of("example", "13")).addAttribute(AttributeType.DESCRIPTION, "Compounding libor 6m vs libor 3m").counterparty(StandardId.of("example", "A")).settlementDate(LocalDate.of(2014, 8, 27)).build()).build();
	  }

	  // create a cross-currency GBP libor 3m vs USD libor 3m swap with spread
	  private static Trade createXCcyGbpLibor3mVsUsdLibor3mSwap()
	  {
		SwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.PAY).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 1, 24)).endDate(LocalDate.of(2021, 1, 24)).frequency(Frequency.P3M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.GBLO)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.of(Currency.GBP, 61_600_000)).calculation(IborRateCalculation.of(IborIndices.GBP_LIBOR_3M)).build();

		SwapLeg receiveLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 1, 24)).endDate(LocalDate.of(2021, 1, 24)).frequency(Frequency.P3M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.of(Currency.USD, 100_000_000)).calculation(IborRateCalculation.builder().index(IborIndices.USD_LIBOR_3M).spread(ValueSchedule.of(0.0091)).build()).build();

		return SwapTrade.builder().product(Swap.of(receiveLeg, payLeg)).info(TradeInfo.builder().id(StandardId.of("example", "14")).addAttribute(AttributeType.DESCRIPTION, "GBP Libor 3m vs USD Libor 3m").counterparty(StandardId.of("example", "A")).settlementDate(LocalDate.of(2014, 1, 24)).build()).build();
	  }

	  // create a cross-currency USD fixed vs GBP libor 3m swap
	  private static SwapTrade createXCcyUsdFixedVsGbpLibor3mSwap()
	  {
		SwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.PAY).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 1, 24)).endDate(LocalDate.of(2021, 1, 24)).frequency(Frequency.P6M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.GBLO)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.of(Currency.USD, 100_000_000)).calculation(FixedRateCalculation.of(0.03, DayCounts.THIRTY_U_360)).build();

		SwapLeg receiveLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 1, 24)).endDate(LocalDate.of(2021, 1, 24)).frequency(Frequency.P3M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.GBLO)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.of(Currency.GBP, 61_600_000)).calculation(IborRateCalculation.of(IborIndices.GBP_LIBOR_3M)).build();

		return SwapTrade.builder().product(Swap.of(payLeg, receiveLeg)).info(TradeInfo.builder().id(StandardId.of("example", "15")).addAttribute(AttributeType.DESCRIPTION, "USD fixed vs GBP Libor 3m").counterparty(StandardId.of("example", "A")).settlementDate(LocalDate.of(2014, 1, 24)).build()).build();
	  }

	  // create a cross-currency USD fixed vs GBP libor 3m swap with initial and final notional exchange
	  private static SwapTrade createNotionalExchangeSwap()
	  {
		SwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.PAY).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 1, 24)).endDate(LocalDate.of(2021, 1, 24)).frequency(Frequency.P6M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.GBLO)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.builder().currency(Currency.USD).amount(ValueSchedule.of(100_000_000)).initialExchange(true).finalExchange(true).build()).calculation(FixedRateCalculation.of(0.03, DayCounts.THIRTY_U_360)).build();

		SwapLeg receiveLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 1, 24)).endDate(LocalDate.of(2021, 1, 24)).frequency(Frequency.P3M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.GBLO)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.builder().currency(Currency.GBP).amount(ValueSchedule.of(61_600_000)).initialExchange(true).finalExchange(true).build()).calculation(IborRateCalculation.of(IborIndices.GBP_LIBOR_3M)).build();

		return SwapTrade.builder().product(Swap.of(payLeg, receiveLeg)).info(TradeInfo.builder().id(StandardId.of("example", "16")).addAttribute(AttributeType.DESCRIPTION, "USD fixed vs GBP Libor 3m (notional exchange)").counterparty(StandardId.of("example", "A")).settlementDate(LocalDate.of(2014, 1, 24)).build()).build();
	  }

	}

}