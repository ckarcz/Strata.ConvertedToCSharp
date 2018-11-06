/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fra.type
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
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
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
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.BUY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.fra.FraDiscountingMethod.AFMA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.fra.FraDiscountingMethod.ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using TenorAdjustment = com.opengamma.strata.basics.date.TenorAdjustment;
	using ImmutableIborIndex = com.opengamma.strata.basics.index.ImmutableIborIndex;

	/// <summary>
	/// Test <seealso cref="FraConvention"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FraConventionTest
	public class FraConventionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double NOTIONAL_2M = 2_000_000d;
	  private static readonly BusinessDayAdjustment BDA_FOLLOW = BusinessDayAdjustment.of(FOLLOWING, GBLO);
	  private static readonly BusinessDayAdjustment BDA_MOD_FOLLOW = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO);
	  private static readonly DaysAdjustment NEXT_SAME_BUS_DAY = DaysAdjustment.ofCalendarDays(0, BDA_FOLLOW);
	  private static readonly DaysAdjustment PLUS_ONE_DAY = DaysAdjustment.ofBusinessDays(1, GBLO);
	  private static readonly DaysAdjustment PLUS_TWO_DAYS = DaysAdjustment.ofBusinessDays(2, GBLO);
	  private static readonly DaysAdjustment MINUS_TWO_DAYS = DaysAdjustment.ofBusinessDays(-2, GBLO);
	  private static readonly DaysAdjustment MINUS_FIVE_DAYS = DaysAdjustment.ofBusinessDays(-5, GBLO);
	  private static readonly ImmutableIborIndex AUD_INDEX = ImmutableIborIndex.builder().name("AUD-INDEX-3M").currency(AUD).dayCount(ACT_360).fixingDateOffset(MINUS_TWO_DAYS).effectiveDateOffset(PLUS_TWO_DAYS).maturityDateOffset(TenorAdjustment.ofLastDay(TENOR_3M, BDA_MOD_FOLLOW)).fixingCalendar(SAT_SUN).fixingTime(LocalTime.NOON).fixingZone(ZoneId.of("Australia/Sydney")).build();
	  private static readonly ImmutableIborIndex NZD_INDEX = ImmutableIborIndex.builder().name("NZD-INDEX-3M").currency(NZD).dayCount(ACT_360).fixingDateOffset(MINUS_TWO_DAYS).effectiveDateOffset(PLUS_TWO_DAYS).maturityDateOffset(TenorAdjustment.ofLastDay(TENOR_3M, BDA_MOD_FOLLOW)).fixingCalendar(SAT_SUN).fixingTime(LocalTime.NOON).fixingZone(ZoneId.of("NZ")).build();

	  //-------------------------------------------------------------------------
	  public virtual void test_of_index()
	  {
		ImmutableFraConvention test = ImmutableFraConvention.of(GBP_LIBOR_3M);
		assertEquals(test.Index, GBP_LIBOR_3M);
		assertEquals(test.Name, GBP_LIBOR_3M.Name);
		assertEquals(test.Currency, GBP);
		assertEquals(test.SpotDateOffset, GBP_LIBOR_3M.EffectiveDateOffset);
		assertEquals(test.BusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.PaymentDateOffset, DaysAdjustment.NONE);
		assertEquals(test.FixingDateOffset, GBP_LIBOR_3M.FixingDateOffset);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.Discounting, ISDA);
		// ensure other factories match
		assertEquals(FraConvention.of(GBP_LIBOR_3M), test);
		assertEquals(FraConventions.of(GBP_LIBOR_3M), test);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_noIndex()
	  {
		assertThrowsIllegalArg(() => ImmutableFraConvention.builder().spotDateOffset(NEXT_SAME_BUS_DAY).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_minSpecified()
	  {
		ImmutableFraConvention test = ImmutableFraConvention.builder().index(GBP_LIBOR_3M).build();
		assertEquals(test.Name, GBP_LIBOR_3M.Name);
		assertEquals(test.Index, GBP_LIBOR_3M);
		assertEquals(test.Currency, GBP);
		assertEquals(test.SpotDateOffset, GBP_LIBOR_3M.EffectiveDateOffset);
		assertEquals(test.BusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.PaymentDateOffset, DaysAdjustment.NONE);
		assertEquals(test.FixingDateOffset, GBP_LIBOR_3M.FixingDateOffset);
		assertEquals(test.DayCount, GBP_LIBOR_3M.DayCount);
		assertEquals(test.Discounting, ISDA);
	  }

	  public virtual void test_builder_allSpecified()
	  {
		ImmutableFraConvention test = ImmutableFraConvention.builder().name(GBP_LIBOR_3M.Name).index(GBP_LIBOR_3M).currency(GBP).spotDateOffset(PLUS_ONE_DAY).businessDayAdjustment(BDA_FOLLOW).paymentDateOffset(PLUS_TWO_DAYS).fixingDateOffset(MINUS_FIVE_DAYS).dayCount(ACT_360).discounting(FraDiscountingMethod.NONE).build();
		assertEquals(test.Name, GBP_LIBOR_3M.Name);
		assertEquals(test.Index, GBP_LIBOR_3M);
		assertEquals(test.Currency, GBP);
		assertEquals(test.SpotDateOffset, PLUS_ONE_DAY);
		assertEquals(test.BusinessDayAdjustment, BDA_FOLLOW);
		assertEquals(test.PaymentDateOffset, PLUS_TWO_DAYS);
		assertEquals(test.FixingDateOffset, MINUS_FIVE_DAYS);
		assertEquals(test.DayCount, ACT_360);
		assertEquals(test.Discounting, FraDiscountingMethod.NONE);
	  }

	  public virtual void test_builder_AUD()
	  {
		ImmutableFraConvention test = ImmutableFraConvention.of(AUD_INDEX);
		assertEquals(test.Index, AUD_INDEX);
		assertEquals(test.Discounting, AFMA);
	  }

	  public virtual void test_builder_NZD()
	  {
		ImmutableFraConvention test = ImmutableFraConvention.of(NZD_INDEX);
		assertEquals(test.Index, NZD_INDEX);
		assertEquals(test.Discounting, AFMA);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createTrade_period()
	  {
		FraConvention @base = ImmutableFraConvention.builder().index(GBP_LIBOR_3M).spotDateOffset(NEXT_SAME_BUS_DAY).build();
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		FraTrade test = @base.createTrade(tradeDate, Period.ofMonths(3), BUY, NOTIONAL_2M, 0.25d, REF_DATA);
		Fra expected = Fra.builder().buySell(BUY).notional(NOTIONAL_2M).startDate(date(2015, 8, 5)).endDate(date(2015, 11, 5)).fixedRate(0.25d).index(GBP_LIBOR_3M).build();
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createTrade_periods()
	  {
		FraConvention @base = ImmutableFraConvention.builder().index(GBP_LIBOR_3M).spotDateOffset(NEXT_SAME_BUS_DAY).build();
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		FraTrade test = @base.createTrade(tradeDate, Period.ofMonths(3), Period.ofMonths(6), BUY, NOTIONAL_2M, 0.25d, REF_DATA);
		Fra expected = Fra.builder().buySell(BUY).notional(NOTIONAL_2M).startDate(date(2015, 8, 5)).endDate(date(2015, 11, 5)).fixedRate(0.25d).index(GBP_LIBOR_3M).build();
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  public virtual void test_createTrade_periods_adjust()
	  {
		FraConvention @base = ImmutableFraConvention.builder().index(GBP_LIBOR_3M).spotDateOffset(NEXT_SAME_BUS_DAY).paymentDateOffset(DaysAdjustment.ofCalendarDays(0, BDA_FOLLOW)).build();
		LocalDate tradeDate = LocalDate.of(2016, 8, 11);
		FraTrade test = @base.createTrade(tradeDate, Period.ofMonths(1), Period.ofMonths(4), BUY, NOTIONAL_2M, 0.25d, REF_DATA);
		Fra expected = Fra.builder().buySell(BUY).notional(NOTIONAL_2M).startDate(date(2016, 9, 12)).endDate(date(2016, 12, 12)).paymentDate(AdjustableDate.of(date(2016, 9, 12), BDA_FOLLOW)).fixedRate(0.25d).index(GBP_LIBOR_3M).build();
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  public virtual void test_createTrade_periods_adjust_payOffset()
	  {
		FraConvention @base = ImmutableFraConvention.builder().index(GBP_LIBOR_3M).spotDateOffset(NEXT_SAME_BUS_DAY).paymentDateOffset(PLUS_TWO_DAYS).build();
		LocalDate tradeDate = LocalDate.of(2016, 8, 11);
		FraTrade test = @base.createTrade(tradeDate, Period.ofMonths(1), Period.ofMonths(4), BUY, NOTIONAL_2M, 0.25d, REF_DATA);
		Fra expected = Fra.builder().buySell(BUY).notional(NOTIONAL_2M).startDate(date(2016, 9, 12)).endDate(date(2016, 12, 12)).paymentDate(AdjustableDate.of(date(2016, 9, 14), PLUS_TWO_DAYS.Adjustment)).fixedRate(0.25d).index(GBP_LIBOR_3M).build();
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toTrade_dates()
	  {
		FraConvention @base = ImmutableFraConvention.builder().index(GBP_LIBOR_3M).spotDateOffset(NEXT_SAME_BUS_DAY).build();
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate = date(2015, 8, 5);
		LocalDate endDate = date(2015, 11, 5);
		LocalDate paymentDate = startDate;
		FraTrade test = @base.toTrade(tradeDate, startDate, endDate, startDate, BUY, NOTIONAL_2M, 0.25d);
		Fra expected = Fra.builder().buySell(BUY).notional(NOTIONAL_2M).startDate(startDate).endDate(endDate).paymentDate(AdjustableDate.of(paymentDate)).fixedRate(0.25d).index(GBP_LIBOR_3M).build();
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  public virtual void test_toTrade_dates_paymentOffset()
	  {
		FraConvention @base = ImmutableFraConvention.builder().index(GBP_LIBOR_3M).spotDateOffset(NEXT_SAME_BUS_DAY).paymentDateOffset(PLUS_TWO_DAYS).build();
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate = date(2015, 8, 5);
		LocalDate endDate = date(2015, 11, 5);
		LocalDate paymentDate = date(2015, 8, 7);
		FraTrade test = @base.toTrade(tradeDate, startDate, endDate, paymentDate, BUY, NOTIONAL_2M, 0.25d);
		Fra expected = Fra.builder().buySell(BUY).notional(NOTIONAL_2M).startDate(date(2015, 8, 5)).endDate(date(2015, 11, 5)).paymentDate(AdjustableDate.of(paymentDate, PLUS_TWO_DAYS.Adjustment)).fixedRate(0.25d).index(GBP_LIBOR_3M).build();
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  public virtual void test_unknownIndex()
	  {
		assertThrowsIllegalArg(() => FraConvention.of("Rubbish"));
	  }

	  public virtual void test_toTemplate_badDateOrder()
	  {
		FraConvention @base = FraConvention.of(GBP_LIBOR_3M);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate = date(2015, 4, 5);
		LocalDate endDate = date(2015, 7, 5);
		LocalDate paymentDate = date(2015, 8, 7);
		assertThrowsIllegalArg(() => @base.toTrade(tradeDate, startDate, endDate, paymentDate, BUY, NOTIONAL_2M, 0.25d));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {ImmutableFraConvention.of(GBP_LIBOR_3M), "GBP-LIBOR-3M"},
			new object[] {ImmutableFraConvention.of(USD_LIBOR_3M), "USD-LIBOR-3M"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_name(FraConvention convention, String name)
	  public virtual void test_name(FraConvention convention, string name)
	  {
		assertEquals(convention.Name, name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(FraConvention convention, String name)
	  public virtual void test_toString(FraConvention convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(FraConvention convention, String name)
	  public virtual void test_of_lookup(FraConvention convention, string name)
	  {
		assertEquals(FraConvention.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_extendedEnum(FraConvention convention, String name)
	  public virtual void test_extendedEnum(FraConvention convention, string name)
	  {
		FraConvention.of(name); // ensures map is populated
		ImmutableMap<string, FraConvention> map = FraConvention.extendedEnum().lookupAll();
		assertEquals(map.get(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => FraConvention.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => FraConvention.of((string) null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ImmutableFraConvention test = ImmutableFraConvention.builder().index(GBP_LIBOR_3M).build();
		coverImmutableBean(test);
		ImmutableFraConvention test2 = ImmutableFraConvention.builder().index(GBP_LIBOR_3M).name("Test").currency(USD).spotDateOffset(PLUS_ONE_DAY).businessDayAdjustment(BDA_FOLLOW).paymentDateOffset(PLUS_TWO_DAYS).fixingDateOffset(MINUS_FIVE_DAYS).dayCount(ACT_360).discounting(FraDiscountingMethod.NONE).build();
		coverBeanEquals(test, test2);

		coverPrivateConstructor(typeof(FraConventions));
		coverPrivateConstructor(typeof(FraConventionLookup));
	  }

	  public virtual void test_serialization()
	  {
		ImmutableFraConvention test = ImmutableFraConvention.builder().index(GBP_LIBOR_3M).build();
		assertSerialization(test);
	  }

	}

}