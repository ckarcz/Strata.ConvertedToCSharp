/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_10Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_2Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_6M;
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
	/// Test <seealso cref="ThreeLegBasisSwapTemplate"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ThreeLegBasisSwapTemplateTest
	public class ThreeLegBasisSwapTemplateTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double NOTIONAL_2M = 2_000_000d;
	  private static readonly BusinessDayAdjustment BDA_FOLLOW = BusinessDayAdjustment.of(FOLLOWING, GBLO);
	  private static readonly FixedRateSwapLegConvention FIXED = FixedRateSwapLegConvention.of(USD, ACT_360, P6M, BDA_FOLLOW);
	  private static readonly IborRateSwapLegConvention IBOR1M = IborRateSwapLegConvention.of(USD_LIBOR_1M);
	  private static readonly IborRateSwapLegConvention IBOR3M = IborRateSwapLegConvention.of(USD_LIBOR_3M);
	  private static readonly IborRateSwapLegConvention IBOR6M = IborRateSwapLegConvention.of(USD_LIBOR_6M);
	  private static readonly ThreeLegBasisSwapConvention CONV = ImmutableThreeLegBasisSwapConvention.of("USD-Swap", FIXED, IBOR3M, IBOR6M);
	  private static readonly ThreeLegBasisSwapConvention CONV2 = ImmutableThreeLegBasisSwapConvention.of("USD-Swap2", FIXED, IBOR1M, IBOR3M);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_spot()
	  {
		ThreeLegBasisSwapTemplate test = ThreeLegBasisSwapTemplate.of(TENOR_10Y, CONV);
		assertEquals(test.PeriodToStart, Period.ZERO);
		assertEquals(test.Tenor, TENOR_10Y);
		assertEquals(test.Convention, CONV);
	  }

	  public virtual void test_of()
	  {
		ThreeLegBasisSwapTemplate test = ThreeLegBasisSwapTemplate.of(Period.ofMonths(3), TENOR_10Y, CONV);
		assertEquals(test.PeriodToStart, Period.ofMonths(3));
		assertEquals(test.Tenor, TENOR_10Y);
		assertEquals(test.Convention, CONV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_notEnoughData()
	  {
		assertThrowsIllegalArg(() => ThreeLegBasisSwapTemplate.builder().tenor(TENOR_2Y).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createTrade()
	  {
		ThreeLegBasisSwapTemplate @base = ThreeLegBasisSwapTemplate.of(Period.ofMonths(3), TENOR_10Y, CONV);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate = date(2015, 8, 7);
		LocalDate endDate = date(2025, 8, 7);
		SwapTrade test = @base.createTrade(tradeDate, BUY, NOTIONAL_2M, 0.25d, REF_DATA);
		Swap expected = Swap.of(FIXED.toLeg(startDate, endDate, PAY, NOTIONAL_2M, 0.25d), IBOR3M.toLeg(startDate, endDate, PAY, NOTIONAL_2M), IBOR6M.toLeg(startDate, endDate, RECEIVE, NOTIONAL_2M));
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ThreeLegBasisSwapTemplate test = ThreeLegBasisSwapTemplate.of(Period.ofMonths(3), TENOR_10Y, CONV);
		coverImmutableBean(test);
		ThreeLegBasisSwapTemplate test2 = ThreeLegBasisSwapTemplate.of(Period.ofMonths(2), TENOR_2Y, CONV2);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		ThreeLegBasisSwapTemplate test = ThreeLegBasisSwapTemplate.of(Period.ofMonths(3), TENOR_10Y, CONV);
		assertSerialization(test);
	  }

	}

}