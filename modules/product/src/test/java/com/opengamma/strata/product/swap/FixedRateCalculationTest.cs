/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Index = com.opengamma.strata.basics.index.Index;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using Schedule = com.opengamma.strata.basics.schedule.Schedule;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;
	using ValueAdjustment = com.opengamma.strata.basics.value.ValueAdjustment;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using ValueStep = com.opengamma.strata.basics.value.ValueStep;
	using FixedRateComputation = com.opengamma.strata.product.rate.FixedRateComputation;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FixedRateCalculationTest
	public class FixedRateCalculationTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  public virtual void test_of()
	  {
		FixedRateCalculation test = FixedRateCalculation.of(0.025d, ACT_365F);
		assertEquals(test.Type, SwapLegType.FIXED);
		assertEquals(test.Rate, ValueSchedule.of(0.025d));
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.InitialStub, null);
		assertEquals(test.FinalStub, null);
	  }

	  public virtual void test_builder()
	  {
		FixedRateCalculation test = FixedRateCalculation.builder().dayCount(ACT_365F).rate(ValueSchedule.of(0.025d)).initialStub(FixedRateStubCalculation.ofFixedRate(0.1d)).finalStub(FixedRateStubCalculation.ofFixedRate(0.2d)).build();
		assertEquals(test.Rate, ValueSchedule.of(0.025d));
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.InitialStub, FixedRateStubCalculation.ofFixedRate(0.1d));
		assertEquals(test.FinalStub, FixedRateStubCalculation.ofFixedRate(0.2d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_collectIndices()
	  {
		FixedRateCalculation test = FixedRateCalculation.builder().dayCount(ACT_365F).rate(ValueSchedule.of(0.025d)).build();
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_expand_oneValue()
	  {
		FixedRateCalculation test = FixedRateCalculation.builder().dayCount(ACT_365F).rate(ValueSchedule.of(0.025d)).build();
		SchedulePeriod period1 = SchedulePeriod.of(date(2014, 1, 6), date(2014, 2, 5), date(2014, 1, 5), date(2014, 2, 5));
		SchedulePeriod period2 = SchedulePeriod.of(date(2014, 1, 5), date(2014, 2, 5), date(2014, 2, 5), date(2014, 3, 5));
		SchedulePeriod period3 = SchedulePeriod.of(date(2014, 3, 5), date(2014, 4, 7), date(2014, 3, 5), date(2014, 4, 5));
		Schedule schedule = Schedule.builder().periods(period1, period2, period3).frequency(Frequency.P1M).rollConvention(RollConventions.DAY_5).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(period1).yearFraction(period1.yearFraction(ACT_365F, schedule)).rateComputation(FixedRateComputation.of(0.025d)).build();
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(period2).yearFraction(period2.yearFraction(ACT_365F, schedule)).rateComputation(FixedRateComputation.of(0.025d)).build();
		RateAccrualPeriod rap3 = RateAccrualPeriod.builder(period3).yearFraction(period3.yearFraction(ACT_365F, schedule)).rateComputation(FixedRateComputation.of(0.025d)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(schedule, schedule, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2, rap3));
	  }

	  public virtual void test_expand_distinctValues()
	  {
		FixedRateCalculation test = FixedRateCalculation.builder().dayCount(ACT_365F).rate(ValueSchedule.of(0.025d, ValueStep.of(1, ValueAdjustment.ofReplace(0.020d)), ValueStep.of(2, ValueAdjustment.ofReplace(0.015d)))).build();
		SchedulePeriod period1 = SchedulePeriod.of(date(2014, 1, 6), date(2014, 2, 5), date(2014, 1, 5), date(2014, 2, 5));
		SchedulePeriod period2 = SchedulePeriod.of(date(2014, 1, 5), date(2014, 2, 5), date(2014, 2, 5), date(2014, 3, 5));
		SchedulePeriod period3 = SchedulePeriod.of(date(2014, 3, 5), date(2014, 4, 7), date(2014, 3, 5), date(2014, 4, 5));
		Schedule schedule = Schedule.builder().periods(period1, period2, period3).frequency(Frequency.P1M).rollConvention(RollConventions.DAY_5).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(period1).yearFraction(period1.yearFraction(ACT_365F, schedule)).rateComputation(FixedRateComputation.of(0.025d)).build();
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(period2).yearFraction(period2.yearFraction(ACT_365F, schedule)).rateComputation(FixedRateComputation.of(0.020d)).build();
		RateAccrualPeriod rap3 = RateAccrualPeriod.builder(period3).yearFraction(period3.yearFraction(ACT_365F, schedule)).rateComputation(FixedRateComputation.of(0.015d)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(schedule, schedule, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2, rap3));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FixedRateCalculation test = FixedRateCalculation.builder().dayCount(ACT_365F).rate(ValueSchedule.of(0.025d)).build();
		coverImmutableBean(test);
		FixedRateCalculation test2 = FixedRateCalculation.builder().dayCount(ACT_360).rate(ValueSchedule.of(0.030d)).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		FixedRateCalculation test = FixedRateCalculation.builder().dayCount(ACT_365F).rate(ValueSchedule.of(0.025d)).build();
		assertSerialization(test);
	  }

	}

}