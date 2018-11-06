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
//	import static com.opengamma.strata.basics.index.OvernightIndices.GBP_SONIA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P12M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.TERM;
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
//	import static com.opengamma.strata.product.swap.OvernightAccrualMethod.AVERAGED;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.OvernightAccrualMethod.COMPOUNDED;
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
	/// Test <seealso cref="OvernightRateSwapLegConvention"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class OvernightRateSwapLegConventionTest
	public class OvernightRateSwapLegConventionTest
	{

	  private const double NOTIONAL_2M = 2_000_000d;
	  private static readonly BusinessDayAdjustment BDA_FOLLOW = BusinessDayAdjustment.of(FOLLOWING, GBLO);
	  private static readonly BusinessDayAdjustment BDA_MOD_FOLLOW = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO);
	  private static readonly DaysAdjustment PLUS_TWO_DAYS = DaysAdjustment.ofBusinessDays(2, GBLO);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		OvernightRateSwapLegConvention test = OvernightRateSwapLegConvention.of(GBP_SONIA, P12M, 2);
		assertEquals(test.Index, GBP_SONIA);
		assertEquals(test.AccrualMethod, COMPOUNDED);
		assertEquals(test.RateCutOffDays, 0);
		assertEquals(test.Currency, GBP);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.AccrualFrequency, P12M);
		assertEquals(test.AccrualBusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.StartDateBusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.EndDateBusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.StubConvention, StubConvention.SMART_INITIAL);
		assertEquals(test.RollConvention, RollConventions.EOM);
		assertEquals(test.PaymentFrequency, P12M);
		assertEquals(test.PaymentDateOffset, DaysAdjustment.ofBusinessDays(2, GBP_SONIA.FixingCalendar));
		assertEquals(test.CompoundingMethod, CompoundingMethod.NONE);
	  }

	  public virtual void test_of_method()
	  {
		OvernightRateSwapLegConvention test = OvernightRateSwapLegConvention.of(GBP_SONIA, P12M, 2, AVERAGED);
		assertEquals(test.Index, GBP_SONIA);
		assertEquals(test.AccrualMethod, AVERAGED);
		assertEquals(test.RateCutOffDays, 0);
		assertEquals(test.Currency, GBP);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.AccrualFrequency, P12M);
		assertEquals(test.AccrualBusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.StartDateBusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.EndDateBusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.StubConvention, StubConvention.SMART_INITIAL);
		assertEquals(test.RollConvention, RollConventions.EOM);
		assertEquals(test.PaymentFrequency, P12M);
		assertEquals(test.PaymentDateOffset, DaysAdjustment.ofBusinessDays(2, GBP_SONIA.FixingCalendar));
		assertEquals(test.CompoundingMethod, CompoundingMethod.NONE);
	  }

	  public virtual void test_builder()
	  {
		OvernightRateSwapLegConvention test = OvernightRateSwapLegConvention.builder().index(GBP_SONIA).build();
		assertEquals(test.Index, GBP_SONIA);
		assertEquals(test.AccrualMethod, COMPOUNDED);
		assertEquals(test.RateCutOffDays, 0);
		assertEquals(test.Currency, GBP);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.AccrualFrequency, TERM);
		assertEquals(test.AccrualBusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.StartDateBusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.EndDateBusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.StubConvention, StubConvention.SMART_INITIAL);
		assertEquals(test.RollConvention, RollConventions.EOM);
		assertEquals(test.PaymentFrequency, TERM);
		assertEquals(test.PaymentDateOffset, DaysAdjustment.NONE);
		assertEquals(test.CompoundingMethod, CompoundingMethod.NONE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_notEnoughData()
	  {
		assertThrowsIllegalArg(() => OvernightRateSwapLegConvention.builder().build());
	  }

	  public virtual void test_builderAllSpecified()
	  {
		OvernightRateSwapLegConvention test = OvernightRateSwapLegConvention.builder().index(GBP_SONIA).accrualMethod(COMPOUNDED).rateCutOffDays(2).currency(USD).dayCount(ACT_360).accrualFrequency(P6M).accrualBusinessDayAdjustment(BDA_FOLLOW).startDateBusinessDayAdjustment(BDA_FOLLOW).endDateBusinessDayAdjustment(BDA_FOLLOW).stubConvention(LONG_INITIAL).rollConvention(RollConventions.DAY_1).paymentFrequency(P6M).paymentDateOffset(PLUS_TWO_DAYS).compoundingMethod(CompoundingMethod.FLAT).build();
		assertEquals(test.Index, GBP_SONIA);
		assertEquals(test.AccrualMethod, COMPOUNDED);
		assertEquals(test.RateCutOffDays, 2);
		assertEquals(test.Currency, USD);
		assertEquals(test.DayCount, ACT_360);
		assertEquals(test.AccrualFrequency, P6M);
		assertEquals(test.AccrualBusinessDayAdjustment, BDA_FOLLOW);
		assertEquals(test.StartDateBusinessDayAdjustment, BDA_FOLLOW);
		assertEquals(test.EndDateBusinessDayAdjustment, BDA_FOLLOW);
		assertEquals(test.StubConvention, StubConvention.LONG_INITIAL);
		assertEquals(test.RollConvention, RollConventions.DAY_1);
		assertEquals(test.PaymentFrequency, P6M);
		assertEquals(test.PaymentDateOffset, PLUS_TWO_DAYS);
		assertEquals(test.CompoundingMethod, CompoundingMethod.FLAT);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toLeg()
	  {
		OvernightRateSwapLegConvention @base = OvernightRateSwapLegConvention.of(GBP_SONIA, TERM, 2);
		LocalDate startDate = LocalDate.of(2015, 5, 5);
		LocalDate endDate = LocalDate.of(2020, 5, 5);
		RateCalculationSwapLeg test = @base.toLeg(startDate, endDate, PAY, NOTIONAL_2M);
		RateCalculationSwapLeg expected = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().frequency(TERM).startDate(startDate).endDate(endDate).businessDayAdjustment(BDA_MOD_FOLLOW).stubConvention(StubConvention.SMART_INITIAL).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(TERM).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBP_SONIA.FixingCalendar)).build()).notionalSchedule(NotionalSchedule.of(GBP, NOTIONAL_2M)).calculation(OvernightRateCalculation.of(GBP_SONIA)).build();
		assertEquals(test, expected);
	  }

	  public virtual void test_toLeg_withSpread()
	  {
		OvernightRateSwapLegConvention @base = OvernightRateSwapLegConvention.builder().index(GBP_SONIA).accrualMethod(AVERAGED).build();
		LocalDate startDate = LocalDate.of(2015, 5, 5);
		LocalDate endDate = LocalDate.of(2020, 5, 5);
		RateCalculationSwapLeg test = @base.toLeg(startDate, endDate, PAY, NOTIONAL_2M, 0.25d);
		RateCalculationSwapLeg expected = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().frequency(TERM).startDate(startDate).endDate(endDate).businessDayAdjustment(BDA_MOD_FOLLOW).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(TERM).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.of(GBP, NOTIONAL_2M)).calculation(OvernightRateCalculation.builder().index(GBP_SONIA).accrualMethod(AVERAGED).spread(ValueSchedule.of(0.25d)).build()).build();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		OvernightRateSwapLegConvention test = OvernightRateSwapLegConvention.builder().index(GBP_SONIA).accrualMethod(COMPOUNDED).build();
		coverImmutableBean(test);
		OvernightRateSwapLegConvention test2 = OvernightRateSwapLegConvention.builder().index(USD_FED_FUND).accrualMethod(AVERAGED).rateCutOffDays(2).currency(USD).dayCount(ACT_360).accrualFrequency(P6M).accrualBusinessDayAdjustment(BDA_FOLLOW).startDateBusinessDayAdjustment(BDA_FOLLOW).endDateBusinessDayAdjustment(BDA_FOLLOW).stubConvention(LONG_INITIAL).rollConvention(RollConventions.EOM).paymentFrequency(P6M).paymentDateOffset(PLUS_TWO_DAYS).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		OvernightRateSwapLegConvention test = OvernightRateSwapLegConvention.of(GBP_SONIA, P12M, 2);
		assertSerialization(test);
	  }

	}

}