/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.dsf
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.PRECEDING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.THIRTY_U_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.SAT_SUN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using FixedRateCalculation = com.opengamma.strata.product.swap.FixedRateCalculation;
	using IborRateCalculation = com.opengamma.strata.product.swap.IborRateCalculation;
	using NotionalSchedule = com.opengamma.strata.product.swap.NotionalSchedule;
	using PaymentSchedule = com.opengamma.strata.product.swap.PaymentSchedule;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;
	using FixedIborSwapConventions = com.opengamma.strata.product.swap.type.FixedIborSwapConventions;

	/// <summary>
	/// Test <seealso cref="ResolvedDsf"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedDsfTest
	public class ResolvedDsfTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly Dsf PRODUCT = DsfTest.sut();
	  private static readonly IborIndex INDEX = IborIndices.USD_LIBOR_3M;
	  private static readonly BusinessDayAdjustment BDA_MF = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, SAT_SUN);
	  private static readonly BusinessDayAdjustment BDA_P = BusinessDayAdjustment.of(PRECEDING, SAT_SUN);
	  private static readonly LocalDate START_DATE = LocalDate.of(2014, 9, 12);
	  private static readonly Swap SWAP = FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M.createTrade(START_DATE, Tenor.TENOR_10Y, BuySell.SELL, 1d, 0.015, REF_DATA).Product;
	  private static readonly ResolvedSwap RSWAP = PRODUCT.UnderlyingSwap.resolve(REF_DATA);
	  private static readonly LocalDate LAST_TRADE_DATE = PRODUCT.LastTradeDate;
	  private static readonly LocalDate DELIVERY_DATE = PRODUCT.DeliveryDate;
	  private static readonly double NOTIONAL = PRODUCT.Notional;

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		ResolvedDsf test = sut();
		assertEquals(test.DeliveryDate, DELIVERY_DATE);
		assertEquals(test.LastTradeDate, LAST_TRADE_DATE);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.Currency, USD);
		assertEquals(test.UnderlyingSwap, RSWAP);
	  }

	  public virtual void test_builder_deliveryAfterStart()
	  {
		assertThrowsIllegalArg(() => ResolvedDsf.builder().notional(NOTIONAL).deliveryDate(LocalDate.of(2014, 9, 19)).lastTradeDate(LAST_TRADE_DATE).underlyingSwap(RSWAP).build());
	  }

	  public virtual void test_builder_tradeAfterdelivery()
	  {
		assertThrowsIllegalArg(() => ResolvedDsf.builder().notional(NOTIONAL).deliveryDate(DELIVERY_DATE).lastTradeDate(LocalDate.of(2014, 9, 11)).underlyingSwap(RSWAP).build());
	  }

	  public virtual void test_builder_notUnitNotional()
	  {
		SwapLeg fixedLeg10 = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2016, 9, 12)).frequency(P6M).businessDayAdjustment(BDA_MF).stubConvention(StubConvention.SHORT_INITIAL).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(P6M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.of(USD, 10d)).calculation(FixedRateCalculation.builder().dayCount(THIRTY_U_360).rate(ValueSchedule.of(0.015)).build()).build();
		SwapLeg iborLeg500 = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2016, 9, 12)).frequency(P1M).businessDayAdjustment(BDA_MF).stubConvention(StubConvention.SHORT_INITIAL).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.builder().currency(USD).amount(ValueSchedule.of(500d)).finalExchange(true).initialExchange(true).build()).calculation(IborRateCalculation.builder().index(INDEX).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, SAT_SUN, BDA_P)).build()).build();
		Swap swap1 = Swap.of(fixedLeg10, SWAP.getLeg(PAY).get());
		Swap swap2 = Swap.of(SWAP.getLeg(RECEIVE).get(), iborLeg500);
		assertThrowsIllegalArg(() => ResolvedDsf.builder().securityId(PRODUCT.SecurityId).notional(NOTIONAL).deliveryDate(DELIVERY_DATE).lastTradeDate(LAST_TRADE_DATE).underlyingSwap(swap1.resolve(REF_DATA)).build());
		assertThrowsIllegalArg(() => ResolvedDsf.builder().securityId(PRODUCT.SecurityId).notional(NOTIONAL).deliveryDate(DELIVERY_DATE).lastTradeDate(LAST_TRADE_DATE).underlyingSwap(swap2.resolve(REF_DATA)).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(sut());
		coverBeanEquals(sut(), sut2());
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(sut());
	  }

	  //-------------------------------------------------------------------------
	  internal static ResolvedDsf sut()
	  {
		return PRODUCT.resolve(REF_DATA);
	  }

	  internal static ResolvedDsf sut2()
	  {
		return DsfTest.sut2().resolve(REF_DATA);
	  }

	}

}