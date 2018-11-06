/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.dsf
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
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

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using TestHelper = com.opengamma.strata.collect.TestHelper;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using FixedRateCalculation = com.opengamma.strata.product.swap.FixedRateCalculation;
	using IborRateCalculation = com.opengamma.strata.product.swap.IborRateCalculation;
	using KnownAmountSwapLeg = com.opengamma.strata.product.swap.KnownAmountSwapLeg;
	using NotionalSchedule = com.opengamma.strata.product.swap.NotionalSchedule;
	using PaymentSchedule = com.opengamma.strata.product.swap.PaymentSchedule;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;
	using FixedIborSwapConventions = com.opengamma.strata.product.swap.type.FixedIborSwapConventions;

	/// <summary>
	/// Test <seealso cref="DsfSecurity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DsfSecurityTest
	public class DsfSecurityTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly Dsf PRODUCT = DsfTest.sut();
	  private static readonly Dsf PRODUCT2 = DsfTest.sut2();
	  private static readonly SecurityPriceInfo PRICE_INFO = SecurityPriceInfo.of(0.1, CurrencyAmount.of(GBP, 25));
	  private static readonly SecurityInfo INFO = SecurityInfo.of(PRODUCT.SecurityId, PRICE_INFO);
	  private static readonly SecurityInfo INFO2 = SecurityInfo.of(PRODUCT2.SecurityId, PRICE_INFO);
	  private static readonly IborIndex INDEX = IborIndices.USD_LIBOR_3M;
	  private static readonly BusinessDayAdjustment BDA_MF = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, SAT_SUN);
	  private static readonly BusinessDayAdjustment BDA_P = BusinessDayAdjustment.of(PRECEDING, SAT_SUN);
	  private static readonly LocalDate LAST_TRADE_DATE = LocalDate.of(2014, 9, 5);
	  private static readonly Swap SWAP = FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M.createTrade(LAST_TRADE_DATE, Tenor.TENOR_10Y, BuySell.SELL, 1d, 0.015, REF_DATA).Product;
	  private const double NOTIONAL = 100000;

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		DsfSecurity test = sut();
		assertEquals(test.Info, INFO);
		assertEquals(test.SecurityId, PRODUCT.SecurityId);
		assertEquals(test.Currency, PRODUCT.Currency);
		assertEquals(test.UnderlyingIds, ImmutableSet.of());
	  }

	  public virtual void test_builder_notUnitNotional()
	  {
		SwapLeg fixedLeg10 = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2016, 9, 12)).frequency(P6M).businessDayAdjustment(BDA_MF).stubConvention(StubConvention.SHORT_INITIAL).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(P6M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.of(USD, 10d)).calculation(FixedRateCalculation.builder().dayCount(THIRTY_U_360).rate(ValueSchedule.of(0.015)).build()).build();
		SwapLeg knownAmountLeg = KnownAmountSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2016, 9, 12)).frequency(P6M).businessDayAdjustment(BDA_MF).stubConvention(StubConvention.SHORT_INITIAL).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(P6M).paymentDateOffset(DaysAdjustment.NONE).build()).amount(ValueSchedule.of(0.015)).currency(USD).build();
		SwapLeg iborLeg500 = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 9, 12)).endDate(LocalDate.of(2016, 9, 12)).frequency(P1M).businessDayAdjustment(BDA_MF).stubConvention(StubConvention.SHORT_INITIAL).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.builder().currency(USD).amount(ValueSchedule.of(500d)).finalExchange(true).initialExchange(true).build()).calculation(IborRateCalculation.builder().index(INDEX).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, SAT_SUN, BDA_P)).build()).build();
		Swap swap1 = Swap.of(fixedLeg10, SWAP.getLeg(PAY).get());
		Swap swap2 = Swap.of(SWAP.getLeg(RECEIVE).get(), iborLeg500);
		Swap swap3 = Swap.of(knownAmountLeg, SWAP.getLeg(PAY).get());
		assertThrowsIllegalArg(() => DsfSecurity.builder().info(INFO).notional(NOTIONAL).lastTradeDate(LAST_TRADE_DATE).underlyingSwap(swap1).build());
		assertThrowsIllegalArg(() => DsfSecurity.builder().info(INFO).notional(NOTIONAL).lastTradeDate(LAST_TRADE_DATE).underlyingSwap(swap2).build());
		// should succeed normally (no notional to validate on known amount leg)
		DsfSecurity.builder().info(INFO).notional(NOTIONAL).lastTradeDate(LAST_TRADE_DATE).underlyingSwap(swap3).build();
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createProduct()
	  {
		DsfSecurity test = sut();
		assertEquals(test.createProduct(ReferenceData.empty()), PRODUCT);
		TradeInfo tradeInfo = TradeInfo.of(PRODUCT.LastTradeDate.minusDays(1));
		DsfTrade expectedTrade = DsfTrade.builder().info(tradeInfo).product(PRODUCT).quantity(100).price(123.50).build();
		assertEquals(test.createTrade(tradeInfo, 100, 123.50, ReferenceData.empty()), expectedTrade);

		PositionInfo positionInfo = PositionInfo.empty();
		DsfPosition expectedPosition1 = DsfPosition.builder().info(positionInfo).product(PRODUCT).longQuantity(100).build();
		TestHelper.assertEqualsBean(test.createPosition(positionInfo, 100, ReferenceData.empty()), expectedPosition1);
		DsfPosition expectedPosition2 = DsfPosition.builder().info(positionInfo).product(PRODUCT).longQuantity(100).shortQuantity(50).build();
		assertEquals(test.createPosition(positionInfo, 100, 50, ReferenceData.empty()), expectedPosition2);
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
	  internal static DsfSecurity sut()
	  {
		return DsfSecurity.builder().info(INFO).notional(PRODUCT.Notional).lastTradeDate(PRODUCT.LastTradeDate).underlyingSwap(PRODUCT.UnderlyingSwap).build();
	  }

	  internal static DsfSecurity sut2()
	  {
		return DsfSecurity.builder().info(INFO2).notional(PRODUCT2.Notional).lastTradeDate(PRODUCT2.LastTradeDate).underlyingSwap(PRODUCT2.UnderlyingSwap).build();
	  }

	}

}