/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fra.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
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
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.BUY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;

	/// <summary>
	/// Test <seealso cref="FraTemplate"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FraTemplateTest
	public class FraTemplateTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly FraConvention FRA_GBP_LIBOR_3M = FraConvention.of(GBP_LIBOR_3M);
	  private const double NOTIONAL_2M = 2_000_000d;
	  private static readonly DaysAdjustment PLUS_TWO_DAYS = DaysAdjustment.ofBusinessDays(2, GBLO);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_PeriodIndex()
	  {
		FraTemplate test = FraTemplate.of(Period.ofMonths(2), GBP_LIBOR_3M);
		assertEquals(test.PeriodToStart, Period.ofMonths(2));
		assertEquals(test.PeriodToEnd, Period.ofMonths(5)); // defaulted
		assertEquals(test.Convention, FRA_GBP_LIBOR_3M);
	  }

	  public virtual void test_of_PeriodPeriodConvention()
	  {
		FraTemplate test = FraTemplate.of(Period.ofMonths(2), Period.ofMonths(4), FRA_GBP_LIBOR_3M);
		assertEquals(test.PeriodToStart, Period.ofMonths(2));
		assertEquals(test.PeriodToEnd, Period.ofMonths(4));
		assertEquals(test.Convention, FRA_GBP_LIBOR_3M);
	  }

	  public virtual void test_builder_defaults()
	  {
		FraTemplate test = FraTemplate.builder().periodToStart(Period.ofMonths(2)).convention(FRA_GBP_LIBOR_3M).build();
		assertEquals(test.PeriodToStart, Period.ofMonths(2));
		assertEquals(test.PeriodToEnd, Period.ofMonths(5)); // defaulted
		assertEquals(test.Convention, FRA_GBP_LIBOR_3M);
	  }

	  public virtual void test_builder_insufficientInfo()
	  {
		assertThrowsIllegalArg(() => FraTemplate.builder().convention(FRA_GBP_LIBOR_3M).build());
		assertThrowsIllegalArg(() => FraTemplate.builder().periodToStart(Period.ofMonths(2)).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createTrade()
	  {
		FraTemplate @base = FraTemplate.of(Period.ofMonths(3), Period.ofMonths(6), FRA_GBP_LIBOR_3M);
		LocalDate tradeDate = LocalDate.of(2015, 5, 4); // trade date is a holiday!
		FraTrade test = @base.createTrade(tradeDate, BUY, NOTIONAL_2M, 0.25d, REF_DATA);
		Fra expected = Fra.builder().buySell(BUY).notional(NOTIONAL_2M).startDate(date(2015, 8, 5)).endDate(date(2015, 11, 5)).fixedRate(0.25d).index(GBP_LIBOR_3M).build();
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  public virtual void test_createTrade_paymentOffset()
	  {
		FraConvention convention = ((ImmutableFraConvention) FRA_GBP_LIBOR_3M).toBuilder().paymentDateOffset(PLUS_TWO_DAYS).build();
		FraTemplate @base = FraTemplate.of(Period.ofMonths(3), Period.ofMonths(6), convention);
		LocalDate tradeDate = LocalDate.of(2015, 5, 4); // trade date is a holiday!
		FraTrade test = @base.createTrade(tradeDate, BUY, NOTIONAL_2M, 0.25d, REF_DATA);
		Fra expected = Fra.builder().buySell(BUY).notional(NOTIONAL_2M).startDate(date(2015, 8, 5)).endDate(date(2015, 11, 5)).paymentDate(AdjustableDate.of(date(2015, 8, 7), PLUS_TWO_DAYS.Adjustment)).fixedRate(0.25d).index(GBP_LIBOR_3M).build();
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FraTemplate test = FraTemplate.of(Period.ofMonths(2), GBP_LIBOR_3M);
		coverImmutableBean(test);
		FraTemplate test2 = FraTemplate.of(Period.ofMonths(3), Period.ofMonths(6), FraConvention.of(USD_LIBOR_3M));
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		FraTemplate test = FraTemplate.of(Period.ofMonths(2), GBP_LIBOR_3M);
		assertSerialization(test);
	  }

	}

}