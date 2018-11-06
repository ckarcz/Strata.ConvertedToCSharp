using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.THIRTY_U_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.CollectProjectAssertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.offset;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using MoreExecutors = com.google.common.util.concurrent.MoreExecutors;
	using ImmutableReferenceData = com.opengamma.strata.basics.ImmutableReferenceData;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using ImmutableIborIndex = com.opengamma.strata.basics.index.ImmutableIborIndex;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using CalculationRunner = com.opengamma.strata.calc.CalculationRunner;
	using Column = com.opengamma.strata.calc.Column;
	using Results = com.opengamma.strata.calc.Results;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using Result = com.opengamma.strata.collect.result.Result;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using MarketData = com.opengamma.strata.data.MarketData;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using StandardDataSets = com.opengamma.strata.pricer.datasets.StandardDataSets;
	using CalendarUSD = com.opengamma.strata.pricer.swap.e2e.CalendarUSD;
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

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SwapPricingTest
	public class SwapPricingTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard().combinedWith(ImmutableReferenceData.of(CalendarUSD.NYC, CalendarUSD.NYC_CALENDAR));
	  private static readonly IborIndex USD_LIBOR_1M = lockIndexCalendar(IborIndices.USD_LIBOR_1M);
	  private static readonly IborIndex USD_LIBOR_3M = lockIndexCalendar(IborIndices.USD_LIBOR_3M);
	  private static readonly IborIndex USD_LIBOR_6M = lockIndexCalendar(IborIndices.USD_LIBOR_6M);
	  private static readonly NotionalSchedule NOTIONAL = NotionalSchedule.of(USD, 100_000_000);
	  private static readonly BusinessDayAdjustment BDA_MF = BusinessDayAdjustment.of(BusinessDayConventions.MODIFIED_FOLLOWING, CalendarUSD.NYC);
	  private static readonly BusinessDayAdjustment BDA_P = BusinessDayAdjustment.of(BusinessDayConventions.PRECEDING, CalendarUSD.NYC);

	  private static readonly LocalDate VAL_DATE = StandardDataSets.VAL_DATE_2014_01_22;

	  // tolerance
	  private const double TOLERANCE_PV = 1.0E-4;

	  //-------------------------------------------------------------------------
	  public virtual void presentValueVanillaFixedVsLibor1mSwap()
	  {
		SwapLeg payLeg = fixedLeg(LocalDate.of(2014, 9, 12), LocalDate.of(2016, 9, 12), Frequency.P6M, PayReceive.PAY, NOTIONAL, 0.0125, null);

		SwapLeg receiveLeg = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2016, 9, 12)).frequency(Frequency.P1M).businessDayAdjustment(BDA_MF).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P1M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NOTIONAL).calculation(IborRateCalculation.builder().index(USD_LIBOR_1M).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, CalendarUSD.NYC, BDA_P)).build()).build();

		SwapTrade trade = SwapTrade.builder().info(TradeInfo.builder().tradeDate(LocalDate.of(2014, 9, 10)).build()).product(Swap.of(payLeg, receiveLeg)).build();

		CurveGroupName groupName = CurveGroupName.of("Test");
		CurveId idUsdDsc = CurveId.of(groupName, StandardDataSets.GROUP1_USD_DSC.Name);
		CurveId idUsdOn = CurveId.of(groupName, StandardDataSets.GROUP1_USD_ON.Name);
		CurveId idUsdL1M = CurveId.of(groupName, StandardDataSets.GROUP1_USD_L1M.Name);
		CurveId idUsdL3M = CurveId.of(groupName, StandardDataSets.GROUP1_USD_L3M.Name);
		CurveId idUsdL6M = CurveId.of(groupName, StandardDataSets.GROUP1_USD_L6M.Name);
		MarketData suppliedData = ImmutableMarketData.builder(VAL_DATE).addValue(idUsdDsc, StandardDataSets.GROUP1_USD_DSC).addValue(idUsdOn, StandardDataSets.GROUP1_USD_ON).addValue(idUsdL1M, StandardDataSets.GROUP1_USD_L1M).addValue(idUsdL3M, StandardDataSets.GROUP1_USD_L3M).addValue(idUsdL6M, StandardDataSets.GROUP1_USD_L6M).build();

		CalculationFunctions functions = StandardComponents.calculationFunctions();

		RatesMarketDataLookup ratesLookup = RatesMarketDataLookup.of(ImmutableMap.of(USD, idUsdDsc), ImmutableMap.of(USD_FED_FUND, idUsdOn, USD_LIBOR_1M, idUsdL1M, USD_LIBOR_3M, idUsdL3M, USD_LIBOR_6M, idUsdL6M));

		// create the calculation runner
		IList<SwapTrade> trades = ImmutableList.of(trade);
		IList<Column> columns = ImmutableList.of(Column.of(Measures.PRESENT_VALUE));
		CalculationRules rules = CalculationRules.of(functions, USD, ratesLookup);

		// calculate results using the runner
		// using the direct executor means there is no need to close/shutdown the runner
		CalculationRunner runner = CalculationRunner.of(MoreExecutors.newDirectExecutorService());
		Results results = runner.calculate(rules, trades, columns, suppliedData, REF_DATA);

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = results.get(0, 0);
		Result<object> result = results.get(0, 0);
		assertThat(result).Success;

		CurrencyAmount pv = (CurrencyAmount) result.Value;
		assertThat(pv.Amount).isCloseTo(-1003684.8402, offset(TOLERANCE_PV));
	  }

	  private static SwapLeg fixedLeg(LocalDate start, LocalDate end, Frequency frequency, PayReceive payReceive, NotionalSchedule notional, double fixedRate, StubConvention stubConvention)
	  {

		return RateCalculationSwapLeg.builder().payReceive(payReceive).accrualSchedule(PeriodicSchedule.builder().startDate(start).endDate(end).frequency(frequency).businessDayAdjustment(BDA_MF).stubConvention(stubConvention).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(frequency).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(FixedRateCalculation.of(fixedRate, THIRTY_U_360)).build();
	  }

	  //-------------------------------------------------------------------------
	  // use a fixed known set of holiday dates to ensure tests produce same numbers
	  private static IborIndex lockIndexCalendar(IborIndex index)
	  {
		return ((ImmutableIborIndex) index).toBuilder().fixingCalendar(CalendarUSD.NYC).effectiveDateOffset(index.EffectiveDateOffset.toBuilder().calendar(CalendarUSD.NYC).adjustment(index.EffectiveDateOffset.Adjustment.toBuilder().calendar(CalendarUSD.NYC).build()).build()).maturityDateOffset(index.MaturityDateOffset.toBuilder().adjustment(index.MaturityDateOffset.Adjustment.toBuilder().calendar(CalendarUSD.NYC).build()).build()).build();
	  }
	}

}