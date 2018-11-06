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
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.StubConvention.LONG_INITIAL;
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
//	import static com.opengamma.strata.product.swap.FixingRelativeTo.PERIOD_END;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.FixingRelativeTo.PERIOD_START;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;

	/// <summary>
	/// Test <seealso cref="IborRateSwapLegConvention"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborRateSwapLegConventionTest
	public class IborRateSwapLegConventionTest
	{

	  private const double NOTIONAL_2M = 2_000_000d;
	  private static readonly BusinessDayAdjustment BDA_FOLLOW = BusinessDayAdjustment.of(FOLLOWING, GBLO);
	  private static readonly BusinessDayAdjustment BDA_MOD_FOLLOW = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO);
	  private static readonly DaysAdjustment PLUS_TWO_DAYS = DaysAdjustment.ofBusinessDays(2, GBLO);
	  private static readonly DaysAdjustment MINUS_FIVE_DAYS = DaysAdjustment.ofBusinessDays(-5, GBLO);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		IborRateSwapLegConvention test = IborRateSwapLegConvention.of(GBP_LIBOR_3M);
		assertEquals(test.Index, GBP_LIBOR_3M);
		assertEquals(test.Currency, GBP);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.AccrualFrequency, P3M);
		assertEquals(test.AccrualBusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.StartDateBusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.EndDateBusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.StubConvention, StubConvention.SMART_INITIAL);
		assertEquals(test.RollConvention, RollConventions.EOM);
		assertEquals(test.FixingRelativeTo, PERIOD_START);
		assertEquals(test.FixingDateOffset, GBP_LIBOR_3M.FixingDateOffset);
		assertEquals(test.PaymentFrequency, P3M);
		assertEquals(test.PaymentDateOffset, DaysAdjustment.NONE);
		assertEquals(test.CompoundingMethod, CompoundingMethod.NONE);
		assertEquals(test.NotionalExchange, false);
	  }

	  public virtual void test_builder()
	  {
		IborRateSwapLegConvention test = IborRateSwapLegConvention.builder().index(GBP_LIBOR_3M).build();
		assertEquals(test.Index, GBP_LIBOR_3M);
		assertEquals(test.Currency, GBP);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.AccrualFrequency, P3M);
		assertEquals(test.AccrualBusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.StartDateBusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.EndDateBusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.StubConvention, StubConvention.SMART_INITIAL);
		assertEquals(test.RollConvention, RollConventions.EOM);
		assertEquals(test.FixingRelativeTo, PERIOD_START);
		assertEquals(test.FixingDateOffset, GBP_LIBOR_3M.FixingDateOffset);
		assertEquals(test.PaymentFrequency, P3M);
		assertEquals(test.PaymentDateOffset, DaysAdjustment.NONE);
		assertEquals(test.CompoundingMethod, CompoundingMethod.NONE);
		assertEquals(test.NotionalExchange, false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_notEnoughData()
	  {
		assertThrowsIllegalArg(() => IborRateSwapLegConvention.builder().build());
	  }

	  public virtual void test_builderAllSpecified()
	  {
		IborRateSwapLegConvention test = IborRateSwapLegConvention.builder().index(GBP_LIBOR_3M).currency(USD).dayCount(ACT_360).accrualFrequency(P6M).accrualBusinessDayAdjustment(BDA_FOLLOW).startDateBusinessDayAdjustment(BDA_FOLLOW).endDateBusinessDayAdjustment(BDA_FOLLOW).stubConvention(LONG_INITIAL).rollConvention(RollConventions.DAY_1).fixingRelativeTo(PERIOD_END).fixingDateOffset(MINUS_FIVE_DAYS).paymentFrequency(P6M).paymentDateOffset(PLUS_TWO_DAYS).compoundingMethod(CompoundingMethod.FLAT).notionalExchange(true).build();
		assertEquals(test.Index, GBP_LIBOR_3M);
		assertEquals(test.Currency, USD);
		assertEquals(test.DayCount, ACT_360);
		assertEquals(test.AccrualFrequency, P6M);
		assertEquals(test.AccrualBusinessDayAdjustment, BDA_FOLLOW);
		assertEquals(test.StartDateBusinessDayAdjustment, BDA_FOLLOW);
		assertEquals(test.EndDateBusinessDayAdjustment, BDA_FOLLOW);
		assertEquals(test.StubConvention, StubConvention.LONG_INITIAL);
		assertEquals(test.RollConvention, RollConventions.DAY_1);
		assertEquals(test.FixingRelativeTo, PERIOD_END);
		assertEquals(test.FixingDateOffset, MINUS_FIVE_DAYS);
		assertEquals(test.PaymentFrequency, P6M);
		assertEquals(test.PaymentDateOffset, PLUS_TWO_DAYS);
		assertEquals(test.CompoundingMethod, CompoundingMethod.FLAT);
		assertEquals(test.NotionalExchange, true);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toLeg()
	  {
		IborRateSwapLegConvention @base = IborRateSwapLegConvention.builder().index(GBP_LIBOR_3M).build();
		LocalDate startDate = LocalDate.of(2015, 5, 5);
		LocalDate endDate = LocalDate.of(2020, 5, 5);
		RateCalculationSwapLeg test = @base.toLeg(startDate, endDate, PAY, NOTIONAL_2M);
		RateCalculationSwapLeg expected = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().frequency(P3M).startDate(startDate).endDate(endDate).businessDayAdjustment(BDA_MOD_FOLLOW).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.of(GBP, NOTIONAL_2M)).calculation(IborRateCalculation.of(GBP_LIBOR_3M)).build();
		assertEquals(test, expected);
	  }

	  public virtual void test_toLeg_withSpread()
	  {
		IborRateSwapLegConvention @base = IborRateSwapLegConvention.builder().index(GBP_LIBOR_3M).build();
		LocalDate startDate = LocalDate.of(2015, 5, 5);
		LocalDate endDate = LocalDate.of(2020, 5, 5);
		RateCalculationSwapLeg test = @base.toLeg(startDate, endDate, PAY, NOTIONAL_2M, 0.25d);
		RateCalculationSwapLeg expected = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().frequency(P3M).startDate(startDate).endDate(endDate).businessDayAdjustment(BDA_MOD_FOLLOW).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.of(GBP, NOTIONAL_2M)).calculation(IborRateCalculation.builder().index(GBP_LIBOR_3M).spread(ValueSchedule.of(0.25d)).build()).build();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		IborRateSwapLegConvention test = IborRateSwapLegConvention.builder().index(GBP_LIBOR_3M).build();
		coverImmutableBean(test);
		IborRateSwapLegConvention test2 = IborRateSwapLegConvention.builder().index(GBP_LIBOR_3M).currency(USD).dayCount(ACT_360).accrualFrequency(P6M).accrualBusinessDayAdjustment(BDA_FOLLOW).startDateBusinessDayAdjustment(BDA_FOLLOW).endDateBusinessDayAdjustment(BDA_FOLLOW).stubConvention(LONG_INITIAL).rollConvention(RollConventions.EOM).fixingRelativeTo(PERIOD_END).fixingDateOffset(MINUS_FIVE_DAYS).paymentFrequency(P6M).paymentDateOffset(PLUS_TWO_DAYS).notionalExchange(true).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		IborRateSwapLegConvention test = IborRateSwapLegConvention.builder().index(GBP_LIBOR_3M).build();
		assertSerialization(test);
	  }

	}

}