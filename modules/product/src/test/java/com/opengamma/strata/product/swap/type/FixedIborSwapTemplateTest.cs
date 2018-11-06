/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
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
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_10Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_2Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
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
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.BUY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;

	/// <summary>
	/// Test <seealso cref="FixedIborSwapTemplate"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FixedIborSwapTemplateTest
	public class FixedIborSwapTemplateTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double NOTIONAL_2M = 2_000_000d;
	  private static readonly BusinessDayAdjustment BDA_FOLLOW = BusinessDayAdjustment.of(FOLLOWING, GBLO);
	  private static readonly BusinessDayAdjustment BDA_MOD_FOLLOW = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO);

	  private static readonly FixedRateSwapLegConvention FIXED = FixedRateSwapLegConvention.of(USD, ACT_360, P6M, BDA_FOLLOW);
	  private static readonly FixedRateSwapLegConvention FIXED2 = FixedRateSwapLegConvention.of(GBP, ACT_365F, P3M, BDA_MOD_FOLLOW);
	  private static readonly IborRateSwapLegConvention IBOR = IborRateSwapLegConvention.of(USD_LIBOR_3M);
	  private static readonly IborRateSwapLegConvention IBOR2 = IborRateSwapLegConvention.of(GBP_LIBOR_3M);
	  private static readonly FixedIborSwapConvention CONV = ImmutableFixedIborSwapConvention.of("USD-Swap", FIXED, IBOR);
	  private static readonly FixedIborSwapConvention CONV2 = ImmutableFixedIborSwapConvention.of("GBP-Swap", FIXED2, IBOR2);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		FixedIborSwapTemplate test = FixedIborSwapTemplate.of(TENOR_10Y, CONV);
		assertEquals(test.PeriodToStart, Period.ZERO);
		assertEquals(test.Tenor, TENOR_10Y);
		assertEquals(test.Convention, CONV);
	  }

	  public virtual void test_of_period()
	  {
		FixedIborSwapTemplate test = FixedIborSwapTemplate.of(Period.ofMonths(3), TENOR_10Y, CONV);
		assertEquals(test.PeriodToStart, Period.ofMonths(3));
		assertEquals(test.Tenor, TENOR_10Y);
		assertEquals(test.Convention, CONV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_notEnoughData()
	  {
		assertThrowsIllegalArg(() => FixedIborSwapTemplate.builder().tenor(TENOR_2Y).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createTrade()
	  {
		FixedIborSwapTemplate @base = FixedIborSwapTemplate.of(Period.ofMonths(3), TENOR_10Y, CONV);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate = date(2015, 8, 7);
		LocalDate endDate = date(2025, 8, 7);
		SwapTrade test = @base.createTrade(tradeDate, BUY, NOTIONAL_2M, 0.25d, REF_DATA);
		Swap expected = Swap.of(FIXED.toLeg(startDate, endDate, PAY, NOTIONAL_2M, 0.25d), IBOR.toLeg(startDate, endDate, RECEIVE, NOTIONAL_2M));
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FixedIborSwapTemplate test = FixedIborSwapTemplate.of(Period.ofMonths(3), TENOR_10Y, CONV);
		coverImmutableBean(test);
		FixedIborSwapTemplate test2 = FixedIborSwapTemplate.of(Period.ofMonths(2), TENOR_2Y, CONV2);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		FixedIborSwapTemplate test = FixedIborSwapTemplate.of(Period.ofMonths(3), TENOR_10Y, CONV);
		assertSerialization(test);
	  }

	}

}