/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_10Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_2Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.GBP_SONIA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
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
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;

	/// <summary>
	/// Test <seealso cref="OvernightIborSwapTemplate"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class OvernightIborSwapTemplateTest
	public class OvernightIborSwapTemplateTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double NOTIONAL_2M = 2_000_000d;

	  private static readonly OvernightRateSwapLegConvention ON_LEG = OvernightRateSwapLegConvention.of(USD_FED_FUND, P6M, 2);
	  private static readonly OvernightRateSwapLegConvention ON_LEG_2 = OvernightRateSwapLegConvention.of(GBP_SONIA, P3M, 0);
	  private static readonly IborRateSwapLegConvention IBOR = IborRateSwapLegConvention.of(USD_LIBOR_3M);
	  private static readonly IborRateSwapLegConvention IBOR2 = IborRateSwapLegConvention.of(GBP_LIBOR_3M);

	  private static readonly DaysAdjustment SPOT_DATE_ADJUSTMENT_2 = DaysAdjustment.ofBusinessDays(2, USNY);
	  private static readonly DaysAdjustment SPOT_DATE_ADJUSTMENT_0 = DaysAdjustment.ofBusinessDays(0, GBLO);

	  private static readonly OvernightIborSwapConvention CONV = ImmutableOvernightIborSwapConvention.of("USD-Swap", ON_LEG, IBOR, SPOT_DATE_ADJUSTMENT_2);
	  private static readonly OvernightIborSwapConvention CONV2 = ImmutableOvernightIborSwapConvention.of("GBP-Swap", ON_LEG_2, IBOR2, SPOT_DATE_ADJUSTMENT_0);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		OvernightIborSwapTemplate test = OvernightIborSwapTemplate.of(TENOR_10Y, CONV);
		assertEquals(test.PeriodToStart, Period.ZERO);
		assertEquals(test.Tenor, TENOR_10Y);
		assertEquals(test.Convention, CONV);
	  }

	  public virtual void test_of_period()
	  {
		OvernightIborSwapTemplate test = OvernightIborSwapTemplate.of(Period.ofMonths(3), TENOR_10Y, CONV);
		assertEquals(test.PeriodToStart, Period.ofMonths(3));
		assertEquals(test.Tenor, TENOR_10Y);
		assertEquals(test.Convention, CONV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_notEnoughData()
	  {
		assertThrowsIllegalArg(() => OvernightIborSwapTemplate.builder().tenor(TENOR_2Y).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createTrade()
	  {
		OvernightIborSwapTemplate @base = OvernightIborSwapTemplate.of(Period.ofMonths(3), TENOR_10Y, CONV);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate = date(2015, 8, 7);
		LocalDate endDate = date(2025, 8, 7);
		SwapTrade test = @base.createTrade(tradeDate, BUY, NOTIONAL_2M, 0.25d, REF_DATA);
		Swap expected = Swap.of(ON_LEG.toLeg(startDate, endDate, PAY, NOTIONAL_2M, 0.25d), IBOR.toLeg(startDate, endDate, RECEIVE, NOTIONAL_2M));
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		OvernightIborSwapTemplate test = OvernightIborSwapTemplate.of(Period.ofMonths(3), TENOR_10Y, CONV);
		coverImmutableBean(test);
		OvernightIborSwapTemplate test2 = OvernightIborSwapTemplate.of(Period.ofMonths(2), TENOR_2Y, CONV2);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		OvernightIborSwapTemplate test = OvernightIborSwapTemplate.of(Period.ofMonths(3), TENOR_10Y, CONV);
		assertSerialization(test);
	  }

	}

}