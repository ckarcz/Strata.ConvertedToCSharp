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
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;

	/// <summary>
	/// Test <seealso cref="FixedRateSwapLegConvention"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FixedRateSwapLegConventionTest
	public class FixedRateSwapLegConventionTest
	{

	  private const double NOTIONAL_2M = 2_000_000d;
	  private static readonly BusinessDayAdjustment BDA_FOLLOW = BusinessDayAdjustment.of(FOLLOWING, GBLO);
	  private static readonly BusinessDayAdjustment BDA_MOD_FOLLOW = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO);
	  private static readonly DaysAdjustment PLUS_TWO_DAYS = DaysAdjustment.ofBusinessDays(2, GBLO);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		FixedRateSwapLegConvention test = FixedRateSwapLegConvention.of(GBP, ACT_365F, P3M, BDA_MOD_FOLLOW);
		assertEquals(test.Currency, GBP);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.AccrualFrequency, P3M);
		assertEquals(test.AccrualBusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.StartDateBusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.EndDateBusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.StubConvention, StubConvention.SMART_INITIAL);
		assertEquals(test.RollConvention, RollConventions.EOM);
		assertEquals(test.PaymentFrequency, P3M);
		assertEquals(test.PaymentDateOffset, DaysAdjustment.NONE);
		assertEquals(test.CompoundingMethod, CompoundingMethod.NONE);
	  }

	  public virtual void test_builder()
	  {
		FixedRateSwapLegConvention test = FixedRateSwapLegConvention.builder().currency(GBP).dayCount(ACT_365F).accrualFrequency(P3M).accrualBusinessDayAdjustment(BDA_MOD_FOLLOW).build();
		assertEquals(test.Currency, GBP);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.AccrualFrequency, P3M);
		assertEquals(test.AccrualBusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.StartDateBusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.EndDateBusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.StubConvention, StubConvention.SMART_INITIAL);
		assertEquals(test.RollConvention, RollConventions.EOM);
		assertEquals(test.PaymentFrequency, P3M);
		assertEquals(test.PaymentDateOffset, DaysAdjustment.NONE);
		assertEquals(test.CompoundingMethod, CompoundingMethod.NONE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_notEnoughData()
	  {
		assertThrowsIllegalArg(() => FixedRateSwapLegConvention.builder().build());
	  }

	  public virtual void test_builderAllSpecified()
	  {
		FixedRateSwapLegConvention test = FixedRateSwapLegConvention.builder().currency(USD).dayCount(ACT_360).accrualFrequency(P6M).accrualBusinessDayAdjustment(BDA_FOLLOW).startDateBusinessDayAdjustment(BDA_FOLLOW).endDateBusinessDayAdjustment(BDA_FOLLOW).stubConvention(LONG_INITIAL).rollConvention(RollConventions.DAY_1).paymentFrequency(P6M).paymentDateOffset(PLUS_TWO_DAYS).compoundingMethod(CompoundingMethod.FLAT).build();
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
		FixedRateSwapLegConvention @base = FixedRateSwapLegConvention.of(GBP, ACT_365F, P3M, BDA_MOD_FOLLOW);
		LocalDate startDate = LocalDate.of(2015, 5, 5);
		LocalDate endDate = LocalDate.of(2020, 5, 5);
		RateCalculationSwapLeg test = @base.toLeg(startDate, endDate, PAY, NOTIONAL_2M, 0.25d);
		RateCalculationSwapLeg expected = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().frequency(P3M).startDate(startDate).endDate(endDate).businessDayAdjustment(BDA_MOD_FOLLOW).stubConvention(StubConvention.SMART_INITIAL).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.of(GBP, NOTIONAL_2M)).calculation(FixedRateCalculation.of(0.25d, ACT_365F)).build();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FixedRateSwapLegConvention test = FixedRateSwapLegConvention.of(GBP, ACT_365F, P3M, BDA_MOD_FOLLOW);
		coverImmutableBean(test);
		FixedRateSwapLegConvention test2 = FixedRateSwapLegConvention.builder().currency(USD).dayCount(ACT_360).accrualFrequency(P6M).accrualBusinessDayAdjustment(BDA_FOLLOW).startDateBusinessDayAdjustment(BDA_FOLLOW).endDateBusinessDayAdjustment(BDA_FOLLOW).stubConvention(LONG_INITIAL).rollConvention(RollConventions.EOM).paymentFrequency(P6M).paymentDateOffset(PLUS_TWO_DAYS).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		FixedRateSwapLegConvention test = FixedRateSwapLegConvention.of(GBP, ACT_365F, P3M, BDA_MOD_FOLLOW);
		assertSerialization(test);
	  }

	}

}