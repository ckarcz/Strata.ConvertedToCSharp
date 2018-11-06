using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swap.e2e
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.THIRTY_U_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.e2e.SwapEnd2EndTest.BDA_MF;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.e2e.SwapEnd2EndTest.BDA_P;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.e2e.SwapEnd2EndTest.NOTIONAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.e2e.SwapEnd2EndTest.USD_LIBOR_1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.e2e.SwapEnd2EndTest.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.e2e.SwapEnd2EndTest.USD_LIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.e2e.SwapEnd2EndTest.swapPricer;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;

	using ImmutableReferenceData = com.opengamma.strata.basics.ImmutableReferenceData;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using CompoundingMethod = com.opengamma.strata.product.swap.CompoundingMethod;
	using FixedRateCalculation = com.opengamma.strata.product.swap.FixedRateCalculation;
	using IborRateCalculation = com.opengamma.strata.product.swap.IborRateCalculation;
	using NotionalSchedule = com.opengamma.strata.product.swap.NotionalSchedule;
	using PaymentSchedule = com.opengamma.strata.product.swap.PaymentSchedule;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;

	/// <summary>
	/// Vague performance test.
	/// </summary>
	public class SwapPricePerformance
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard().combinedWith(ImmutableReferenceData.of(CalendarUSD.NYC, CalendarUSD.NYC_CALENDAR));

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws Exception
	  public static void Main(string[] args)
	  {
		Console.WriteLine("Go");
		for (int i = 0; i < 12; i++)
		{
		  if (process() > 0)
		  {
			Console.WriteLine(i);
		  }
		}
	  }

	  private static double process()
	  {
		SwapPricePerformance test = new SwapPricePerformance();
		long start = System.nanoTime();
		double total = 0d;
		for (int i = 0; i < 10_000; i++)
		{
		  total += test.test_VanillaFixedVsLibor1mSwap();
		  total += test.test_VanillaFixedVsLibor3mSwap();
		  total += test.test_VanillaFixedVsLibor3mSwapWithFixing();
		  total += test.test_BasisLibor3mVsLibor6mSwapWithSpread();
		  total += test.test_BasisCompoundedLibor1mVsLibor3mSwap();
		}
		Console.WriteLine("Total: " + total);
		long end = System.nanoTime();
		Console.WriteLine((end - start) / 1_000_000_000d + " s");
		return total;
	  }

	  //-----------------------------------------------------------------------
	  private static readonly SwapLeg PAY1 = fixedLeg(LocalDate.of(2014, 9, 12), LocalDate.of(2016, 9, 12), P6M, PAY, NOTIONAL, 0.0125, null);

	  private static readonly SwapLeg RECEIVE1 = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2016, 9, 12)).frequency(P1M).businessDayAdjustment(BDA_MF).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(P1M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NOTIONAL).calculation(IborRateCalculation.builder().index(USD_LIBOR_1M).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, CalendarUSD.NYC, BDA_P)).build()).build();

	  private static readonly SwapTrade TRADE1 = SwapTrade.builder().info(TradeInfo.builder().tradeDate(LocalDate.of(2014, 9, 10)).build()).product(Swap.of(PAY1, RECEIVE1)).build();
	  private static readonly RatesProvider PROVIDER = SwapEnd2EndTest.provider();

	  public virtual double test_VanillaFixedVsLibor1mSwap()
	  {
		DiscountingSwapTradePricer pricer = swapPricer();
		ResolvedSwapTrade resolved = TRADE1.resolve(REF_DATA);
		CurrencyAmount pv = pricer.presentValue(resolved, USD, PROVIDER);
		return pv.Amount;
	  }

	  //-----------------------------------------------------------------------
	  private static readonly SwapLeg PAY2 = fixedLeg(LocalDate.of(2014, 9, 12), LocalDate.of(2021, 9, 12), P6M, PAY, NOTIONAL, 0.015, null);

	  private static readonly SwapLeg RECEIVE2 = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2021, 9, 12)).frequency(P3M).businessDayAdjustment(BDA_MF).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NOTIONAL).calculation(IborRateCalculation.builder().index(USD_LIBOR_3M).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, CalendarUSD.NYC, BDA_P)).build()).build();

	  private static readonly SwapTrade TRADE2 = SwapTrade.builder().info(TradeInfo.builder().tradeDate(LocalDate.of(2014, 9, 10)).build()).product(Swap.of(PAY2, RECEIVE2)).build();

	  public virtual double test_VanillaFixedVsLibor3mSwap()
	  {
		DiscountingSwapTradePricer pricer = swapPricer();
		ResolvedSwapTrade resolved = TRADE2.resolve(REF_DATA);
		CurrencyAmount pv = pricer.presentValue(resolved, USD, PROVIDER);
		return pv.Amount;
	  }

	  //-------------------------------------------------------------------------
	  private static readonly SwapLeg PAY3 = fixedLeg(LocalDate.of(2013, 9, 12), LocalDate.of(2020, 9, 12), P6M, PAY, NOTIONAL, 0.015, null);

	  private static readonly SwapLeg RECEIVE3 = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2013, 9, 12)).endDate(LocalDate.of(2020, 9, 12)).frequency(P3M).businessDayAdjustment(BDA_MF).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NOTIONAL).calculation(IborRateCalculation.builder().index(USD_LIBOR_3M).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, CalendarUSD.NYC, BDA_P)).build()).build();

	  private static readonly SwapTrade TRADE3 = SwapTrade.builder().info(TradeInfo.builder().tradeDate(LocalDate.of(2013, 9, 10)).build()).product(Swap.of(PAY3, RECEIVE3)).build();

	  public virtual double test_VanillaFixedVsLibor3mSwapWithFixing()
	  {
		DiscountingSwapTradePricer pricer = swapPricer();
		ResolvedSwapTrade resolved = TRADE3.resolve(REF_DATA);
		CurrencyAmount pv = pricer.presentValue(resolved, USD, PROVIDER);
		return pv.Amount;
	  }

	  //-------------------------------------------------------------------------
	  private static readonly SwapLeg PAY4 = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 8, 29)).endDate(LocalDate.of(2024, 8, 29)).frequency(P6M).businessDayAdjustment(BDA_MF).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P6M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NOTIONAL).calculation(IborRateCalculation.builder().index(USD_LIBOR_6M).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, CalendarUSD.NYC, BDA_P)).build()).build();

	  private static readonly SwapLeg RECEIVE4 = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 8, 29)).endDate(LocalDate.of(2024, 8, 29)).frequency(P3M).businessDayAdjustment(BDA_MF).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NOTIONAL).calculation(IborRateCalculation.builder().index(USD_LIBOR_3M).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, CalendarUSD.NYC, BDA_P)).spread(ValueSchedule.of(0.0010)).build()).build();

	  private static readonly SwapTrade TRADE4 = SwapTrade.builder().info(TradeInfo.builder().tradeDate(LocalDate.of(2014, 8, 27)).build()).product(Swap.of(PAY4, RECEIVE4)).build();

	  public virtual double test_BasisLibor3mVsLibor6mSwapWithSpread()
	  {
		DiscountingSwapTradePricer pricer = swapPricer();
		ResolvedSwapTrade resolved = TRADE4.resolve(REF_DATA);
		CurrencyAmount pv = pricer.presentValue(resolved, USD, PROVIDER);
		return pv.Amount;
	  }

	  //-------------------------------------------------------------------------
	  private static readonly SwapLeg RECEIVE5 = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 8, 29)).endDate(LocalDate.of(2019, 8, 29)).frequency(P1M).businessDayAdjustment(BDA_MF).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P3M).paymentDateOffset(DaysAdjustment.NONE).compoundingMethod(CompoundingMethod.FLAT).build()).notionalSchedule(NOTIONAL).calculation(IborRateCalculation.builder().index(USD_LIBOR_1M).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, CalendarUSD.NYC, BDA_P)).build()).build();

	  private static readonly SwapLeg PAY5 = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 8, 29)).endDate(LocalDate.of(2019, 8, 29)).frequency(P3M).businessDayAdjustment(BDA_MF).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NOTIONAL).calculation(IborRateCalculation.builder().index(USD_LIBOR_3M).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, CalendarUSD.NYC, BDA_P)).build()).build();

	  private static readonly SwapTrade TRADE5 = SwapTrade.builder().info(TradeInfo.builder().tradeDate(LocalDate.of(2014, 8, 27)).build()).product(Swap.of(RECEIVE5, PAY5)).build();

	  public virtual double test_BasisCompoundedLibor1mVsLibor3mSwap()
	  {
		DiscountingSwapTradePricer pricer = swapPricer();
		ResolvedSwapTrade resolved = TRADE5.resolve(REF_DATA);
		CurrencyAmount pv = pricer.presentValue(resolved, USD, PROVIDER);
		return pv.Amount;
	  }

	  //-------------------------------------------------------------------------
	  // fixed rate leg
	  private static SwapLeg fixedLeg(LocalDate start, LocalDate end, Frequency frequency, PayReceive payReceive, NotionalSchedule notional, double fixedRate, StubConvention stubConvention)
	  {

		return RateCalculationSwapLeg.builder().payReceive(payReceive).accrualSchedule(PeriodicSchedule.builder().startDate(start).endDate(end).frequency(frequency).businessDayAdjustment(BDA_MF).stubConvention(stubConvention).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(frequency).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(FixedRateCalculation.builder().dayCount(THIRTY_U_360).rate(ValueSchedule.of(fixedRate)).build()).build();
	  }

	}

}