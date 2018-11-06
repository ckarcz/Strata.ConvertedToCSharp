/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fra
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.AUD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.NZD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.SAT_SUN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_2M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.BUY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.SELL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.fra.FraDiscountingMethod.AFMA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.fra.FraDiscountingMethod.ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using TenorAdjustment = com.opengamma.strata.basics.date.TenorAdjustment;
	using ImmutableIborIndex = com.opengamma.strata.basics.index.ImmutableIborIndex;
	using IborInterpolatedRateComputation = com.opengamma.strata.product.rate.IborInterpolatedRateComputation;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FraTest
	public class FraTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double NOTIONAL_1M = 1_000_000d;
	  private const double NOTIONAL_2M = 2_000_000d;
	  private const double FIXED_RATE = 0.025d;
	  private static readonly BusinessDayAdjustment BDA_MOD_FOLLOW = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO);
	  private static readonly DaysAdjustment PLUS_TWO_DAYS = DaysAdjustment.ofBusinessDays(2, GBLO);
	  private static readonly DaysAdjustment MINUS_TWO_DAYS = DaysAdjustment.ofBusinessDays(-2, GBLO);
	  private static readonly DaysAdjustment MINUS_FIVE_DAYS = DaysAdjustment.ofBusinessDays(-5, GBLO);

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		Fra test = sut();
		assertEquals(test.BuySell, BUY);
		assertEquals(test.Currency, GBP); // defaulted
		assertEquals(test.Notional, NOTIONAL_1M, 0d);
		assertEquals(test.StartDate, date(2015, 6, 15));
		assertEquals(test.EndDate, date(2015, 9, 15));
		assertEquals(test.BusinessDayAdjustment, null);
		assertEquals(test.PaymentDate, AdjustableDate.of(date(2015, 6, 15)));
		assertEquals(test.FixedRate, FIXED_RATE, 0d);
		assertEquals(test.Index, GBP_LIBOR_3M);
		assertEquals(test.IndexInterpolated, null);
		assertEquals(test.FixingDateOffset, GBP_LIBOR_3M.FixingDateOffset); // defaulted
		assertEquals(test.DayCount, ACT_365F); // defaulted
		assertEquals(test.Discounting, ISDA); // defaulted
		assertEquals(test.CrossCurrency, false);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(GBP));
		assertEquals(test.allCurrencies(), ImmutableSet.of(GBP));
	  }

	  public virtual void test_builder_AUD()
	  {
		ImmutableIborIndex dummyIndex = ImmutableIborIndex.builder().name("AUD-INDEX-3M").currency(AUD).dayCount(ACT_360).fixingDateOffset(MINUS_TWO_DAYS).effectiveDateOffset(PLUS_TWO_DAYS).maturityDateOffset(TenorAdjustment.ofLastDay(TENOR_3M, BDA_MOD_FOLLOW)).fixingCalendar(SAT_SUN).fixingTime(LocalTime.NOON).fixingZone(ZoneId.of("Australia/Sydney")).build();
		Fra test = Fra.builder().buySell(BUY).notional(NOTIONAL_1M).startDate(date(2015, 6, 15)).endDate(date(2015, 9, 15)).paymentDate(AdjustableDate.of(date(2015, 6, 16))).fixedRate(FIXED_RATE).index(dummyIndex).fixingDateOffset(MINUS_TWO_DAYS).build();
		assertEquals(test.BuySell, BUY);
		assertEquals(test.Currency, AUD); // defaulted
		assertEquals(test.Notional, NOTIONAL_1M, 0d);
		assertEquals(test.StartDate, date(2015, 6, 15));
		assertEquals(test.EndDate, date(2015, 9, 15));
		assertEquals(test.BusinessDayAdjustment, null);
		assertEquals(test.PaymentDate, AdjustableDate.of(date(2015, 6, 16)));
		assertEquals(test.FixedRate, FIXED_RATE, 0d);
		assertEquals(test.Index, dummyIndex);
		assertEquals(test.IndexInterpolated, null);
		assertEquals(test.FixingDateOffset, MINUS_TWO_DAYS);
		assertEquals(test.DayCount, ACT_360); // defaulted
		assertEquals(test.Discounting, AFMA); // defaulted
	  }

	  public virtual void test_builder_NZD()
	  {
		ImmutableIborIndex dummyIndex = ImmutableIborIndex.builder().name("NZD-INDEX-3M").currency(NZD).dayCount(ACT_360).fixingDateOffset(MINUS_TWO_DAYS).effectiveDateOffset(PLUS_TWO_DAYS).maturityDateOffset(TenorAdjustment.ofLastDay(TENOR_3M, BDA_MOD_FOLLOW)).fixingCalendar(SAT_SUN).fixingTime(LocalTime.NOON).fixingZone(ZoneId.of("NZ")).build();
		Fra test = Fra.builder().buySell(BUY).notional(NOTIONAL_1M).startDate(date(2015, 6, 15)).endDate(date(2015, 9, 15)).paymentDate(AdjustableDate.of(date(2015, 6, 16))).fixedRate(FIXED_RATE).index(dummyIndex).fixingDateOffset(MINUS_TWO_DAYS).build();
		assertEquals(test.BuySell, BUY);
		assertEquals(test.Currency, NZD); // defaulted
		assertEquals(test.Notional, NOTIONAL_1M, 0d);
		assertEquals(test.StartDate, date(2015, 6, 15));
		assertEquals(test.EndDate, date(2015, 9, 15));
		assertEquals(test.BusinessDayAdjustment, null);
		assertEquals(test.PaymentDate, AdjustableDate.of(date(2015, 6, 16)));
		assertEquals(test.FixedRate, FIXED_RATE, 0d);
		assertEquals(test.Index, dummyIndex);
		assertEquals(test.IndexInterpolated, null);
		assertEquals(test.FixingDateOffset, MINUS_TWO_DAYS);
		assertEquals(test.DayCount, ACT_360); // defaulted
		assertEquals(test.Discounting, AFMA); // defaulted
	  }

	  public virtual void test_builder_datesInOrder()
	  {
		assertThrowsIllegalArg(() => Fra.builder().buySell(BUY).notional(NOTIONAL_1M).startDate(date(2015, 6, 15)).endDate(date(2015, 6, 14)).fixedRate(FIXED_RATE).index(GBP_LIBOR_3M).build());
	  }

	  public virtual void test_builder_noIndex()
	  {
		assertThrowsIllegalArg(() => Fra.builder().buySell(BUY).notional(NOTIONAL_1M).startDate(date(2015, 6, 15)).endDate(date(2015, 9, 15)).fixedRate(FIXED_RATE).build());
	  }

	  public virtual void test_builder_noDates()
	  {
		assertThrowsIllegalArg(() => Fra.builder().buySell(BUY).notional(NOTIONAL_1M).endDate(date(2015, 9, 15)).fixedRate(FIXED_RATE).index(GBP_LIBOR_3M).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve_Ibor()
	  {
		Fra fra = Fra.builder().buySell(BUY).notional(NOTIONAL_1M).startDate(date(2015, 6, 15)).endDate(date(2015, 9, 15)).paymentDate(AdjustableDate.of(date(2015, 6, 20), BDA_MOD_FOLLOW)).fixedRate(FIXED_RATE).index(GBP_LIBOR_3M).fixingDateOffset(MINUS_TWO_DAYS).build();
		ResolvedFra test = fra.resolve(REF_DATA);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Notional, NOTIONAL_1M, 0d);
		assertEquals(test.StartDate, date(2015, 6, 15));
		assertEquals(test.EndDate, date(2015, 9, 15));
		assertEquals(test.PaymentDate, date(2015, 6, 22));
		assertEquals(test.FixedRate, FIXED_RATE, 0d);
		assertEquals(test.FloatingRate, IborRateComputation.of(GBP_LIBOR_3M, date(2015, 6, 11), REF_DATA));
		assertEquals(test.YearFraction, ACT_365F.yearFraction(date(2015, 6, 15), date(2015, 9, 15)), 0d);
		assertEquals(test.Discounting, ISDA);
	  }

	  public virtual void test_resolve_IborInterpolated()
	  {
		Fra fra = Fra.builder().buySell(SELL).notional(NOTIONAL_1M).startDate(date(2015, 6, 12)).endDate(date(2015, 9, 5)).businessDayAdjustment(BDA_MOD_FOLLOW).fixedRate(FIXED_RATE).index(GBP_LIBOR_3M).indexInterpolated(GBP_LIBOR_2M).fixingDateOffset(MINUS_TWO_DAYS).build();
		ResolvedFra test = fra.resolve(REF_DATA);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Notional, -NOTIONAL_1M, 0d); // sell
		assertEquals(test.StartDate, date(2015, 6, 12));
		assertEquals(test.EndDate, date(2015, 9, 7));
		assertEquals(test.PaymentDate, date(2015, 6, 12));
		assertEquals(test.FixedRate, FIXED_RATE, 0d);
		assertEquals(test.FloatingRate, IborInterpolatedRateComputation.of(GBP_LIBOR_2M, GBP_LIBOR_3M, date(2015, 6, 10), REF_DATA));
		assertEquals(test.YearFraction, ACT_365F.yearFraction(date(2015, 6, 12), date(2015, 9, 7)), 0d);
		assertEquals(test.Discounting, ISDA);
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
	  internal static Fra sut()
	  {
		return Fra.builder().buySell(BUY).notional(NOTIONAL_1M).startDate(date(2015, 6, 15)).endDate(date(2015, 9, 15)).fixedRate(FIXED_RATE).index(GBP_LIBOR_3M).build();
	  }

	  internal static Fra sut2()
	  {
		return Fra.builder().buySell(SELL).currency(USD).notional(NOTIONAL_2M).startDate(date(2015, 6, 16)).endDate(date(2015, 8, 17)).businessDayAdjustment(BDA_MOD_FOLLOW).paymentDate(AdjustableDate.of(date(2015, 6, 17))).dayCount(ACT_360).fixedRate(0.30d).index(GBP_LIBOR_2M).indexInterpolated(GBP_LIBOR_3M).fixingDateOffset(MINUS_FIVE_DAYS).discounting(FraDiscountingMethod.NONE).build();
	  }

	}

}