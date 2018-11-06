using System;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.finance
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;

	using JodaBeanSer = org.joda.beans.ser.JodaBeanSer;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueAdjustment = com.opengamma.strata.basics.value.ValueAdjustment;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using ValueStep = com.opengamma.strata.basics.value.ValueStep;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using CompoundingMethod = com.opengamma.strata.product.swap.CompoundingMethod;
	using FixedRateCalculation = com.opengamma.strata.product.swap.FixedRateCalculation;
	using FixingRelativeTo = com.opengamma.strata.product.swap.FixingRelativeTo;
	using IborRateCalculation = com.opengamma.strata.product.swap.IborRateCalculation;
	using NotionalSchedule = com.opengamma.strata.product.swap.NotionalSchedule;
	using PaymentRelativeTo = com.opengamma.strata.product.swap.PaymentRelativeTo;
	using PaymentSchedule = com.opengamma.strata.product.swap.PaymentSchedule;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;

	/// <summary>
	/// Demonstrate use of the API for interest rate swaps.
	/// <para>
	/// This class exists for demonstration purposes to aid with understanding interest rate swaps.
	/// It is not intended to be used in a production environment.
	/// </para>
	/// </summary>
	public class SwapTradeExample
	{

	  /// <summary>
	  /// Launch demo, no arguments needed.
	  /// </summary>
	  /// <param name="args">  no arguments needed </param>
	  public static void Main(string[] args)
	  {
		SwapTradeExample demo = new SwapTradeExample();
		demo.fixedSwapLeg();
		demo.floatingSwapLeg();
		demo.vanillaFixedVsLibor3mSwap();
	  }

	  //-----------------------------------------------------------------------
	  public virtual void fixedSwapLeg()
	  {
		// a PeriodicSchedule generates a schedule of accrual periods
		// - interest is accrued every 3 months from 2014-02-12 to 2014-07-31
		// - accrual period dates are adjusted "modified following" using the "GBLO" holiday calendar
		// - there will be a long initial stub
		// - the regular accrual period dates will be at the end-of-month
		PeriodicSchedule accrualSchedule = PeriodicSchedule.builder().startDate(LocalDate.of(2014, 2, 12)).endDate(LocalDate.of(2016, 7, 31)).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.GBLO)).frequency(Frequency.P3M).stubConvention(StubConvention.LONG_INITIAL).rollConvention(RollConventions.EOM).build();
		// a PaymentSchedule generates a schedule of payment periods, based on the accrual schedule
		// - payments are every 6 months
		// - payments are 2 business days after the end of the period
		// - straight compounding is used (the payments are less frequent than the accrual, so compounding occurs)
		PaymentSchedule paymentSchedule = PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentRelativeTo(PaymentRelativeTo.PERIOD_END).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, HolidayCalendarIds.GBLO)).compoundingMethod(CompoundingMethod.STRAIGHT).build();
		// a NotionalSchedule generates a schedule of notional amounts, based on the payment schedule
		// - in this simple case the notional is 1 million GBP and does not change
		NotionalSchedule notionalSchedule = NotionalSchedule.of(Currency.GBP, 1_000_000);
		// a RateCalculationSwapLeg can represent a fixed or floating swap leg
		// - a FixedRateCalculation is used to represent a fixed rate
		// - the "Act/Act ISDA" day count is used
		// - the rate starts at 0.8% and reduces to 0.7%
		RateCalculationSwapLeg swapLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.PAY).accrualSchedule(accrualSchedule).paymentSchedule(paymentSchedule).notionalSchedule(notionalSchedule).calculation(FixedRateCalculation.builder().dayCount(DayCounts.ACT_ACT_ISDA).rate(ValueSchedule.of(0.008, ValueStep.of(LocalDate.of(2015, 1, 31), ValueAdjustment.ofReplace(0.007)))).build()).build();
		// a ResolvedSwapLeg has all the dates of the cash flows
		// it remains valid so long as the holiday calendar does not change 
		ResolvedSwapLeg resolvedLeg = swapLeg.resolve(ReferenceData.standard());

		Console.WriteLine("===== Fixed =====");
		Console.WriteLine(JodaBeanSer.PRETTY.xmlWriter().write(swapLeg));
		Console.WriteLine();
		Console.WriteLine("===== Fixed resolved =====");
		Console.WriteLine(JodaBeanSer.PRETTY.xmlWriter().write(resolvedLeg));
		Console.WriteLine();
	  }

	  //-----------------------------------------------------------------------
	  public virtual void floatingSwapLeg()
	  {
		// a PeriodicSchedule generates a schedule of accrual periods
		// - interest is accrued every 6 months from 2014-02-12 to 2014-07-31
		// - accrual period dates are adjusted "modified following" using the "GBLO" holiday calendar
		// - there will be a long initial stub
		// - the regular accrual period dates will be at the end-of-month
		PeriodicSchedule accrualSchedule = PeriodicSchedule.builder().startDate(LocalDate.of(2014, 2, 12)).endDate(LocalDate.of(2016, 7, 31)).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.GBLO)).frequency(Frequency.P6M).stubConvention(StubConvention.LONG_INITIAL).rollConvention(RollConventions.EOM).build();
		// a PaymentSchedule generates a schedule of payment periods, based on the accrual schedule
		// - payments are every 6 months
		// - payments are 2 business days after the end of the period
		// - no compounding is needed as the payment schedule matches the accrual schedule
		PaymentSchedule paymentSchedule = PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentRelativeTo(PaymentRelativeTo.PERIOD_END).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, HolidayCalendarIds.GBLO)).build();
		// a NotionalSchedule generates a schedule of notional amounts, based on the payment schedule
		// - in this simple case the notional is 1 million GBP and does not change
		NotionalSchedule notionalSchedule = NotionalSchedule.of(Currency.GBP, 1_000_000);
		// a RateCalculationSwapLeg can represent a fixed or floating swap leg
		// - an IborRateCalculation is used to represent a floating Ibor rate
		// - the "Act/Act ISDA" day count is used
		// - the index is GBP LIBOR 6M
		// - fixing is 2 days before the start of the period using the "GBLO" holiday calendar
		RateCalculationSwapLeg swapLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.RECEIVE).accrualSchedule(accrualSchedule).paymentSchedule(paymentSchedule).notionalSchedule(notionalSchedule).calculation(IborRateCalculation.builder().dayCount(DayCounts.ACT_ACT_ISDA).index(IborIndices.GBP_LIBOR_6M).fixingRelativeTo(FixingRelativeTo.PERIOD_START).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, HolidayCalendarIds.GBLO)).build()).build();
		// a ResolvedSwapLeg has all the dates of the cash flows
		// it remains valid so long as the holiday calendar does not change 
		ResolvedSwapLeg resolvedLeg = swapLeg.resolve(ReferenceData.standard());

		Console.WriteLine("===== Floating =====");
		Console.WriteLine(JodaBeanSer.PRETTY.xmlWriter().write(swapLeg));
		Console.WriteLine();
		Console.WriteLine("===== Floating resolved =====");
		Console.WriteLine(JodaBeanSer.PRETTY.xmlWriter().write(resolvedLeg));
		Console.WriteLine();
	  }

	  //-----------------------------------------------------------------------
	  public virtual void vanillaFixedVsLibor3mSwap()
	  {
		// we are paying a fixed rate every 3 months at 1.5% with a 100 million notional
		RateCalculationSwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.PAY).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2021, 9, 12)).frequency(Frequency.P3M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.builder().currency(Currency.USD).amount(ValueSchedule.of(100_000_000)).build()).calculation(FixedRateCalculation.of(0.015, DayCounts.THIRTY_U_360)).build();
		// we are receiving USD LIBOR 3M every 3 months with a 100 million notional
		RateCalculationSwapLeg receiveLeg = RateCalculationSwapLeg.builder().payReceive(PayReceive.RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2021, 9, 12)).frequency(Frequency.P3M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HolidayCalendarIds.USNY)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.builder().currency(Currency.USD).amount(ValueSchedule.of(100_000_000)).build()).calculation(IborRateCalculation.of(IborIndices.USD_LIBOR_3M)).build();
		// a SwapTrade combines the two legs
		SwapTrade trade = SwapTrade.builder().info(TradeInfo.builder().id(StandardId.of("OG-Trade", "1")).tradeDate(LocalDate.of(2014, 9, 10)).build()).product(Swap.of(payLeg, receiveLeg)).build();

		Console.WriteLine("===== Vanilla fixed vs Libor3m =====");
		Console.WriteLine(JodaBeanSer.PRETTY.xmlWriter().write(trade));
		Console.WriteLine();
		Console.WriteLine("===== Vanilla fixed vs Libor3m pay leg =====");
		Console.WriteLine(JodaBeanSer.PRETTY.xmlWriter().write(payLeg.resolve(ReferenceData.standard())));
		Console.WriteLine();
		Console.WriteLine("===== Vanilla fixed vs Libor3m receive leg =====");
		Console.WriteLine(JodaBeanSer.PRETTY.xmlWriter().write(receiveLeg.resolve(ReferenceData.standard())));
		Console.WriteLine();
	  }

	}

}