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
//	import static com.opengamma.strata.basics.index.OvernightIndices.CHF_TOIS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.GBP_SONIA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
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
//	import static com.opengamma.strata.product.swap.NegativeRateMethod.ALLOW_NEGATIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.NegativeRateMethod.NOT_NEGATIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.OvernightAccrualMethod.AVERAGED;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.OvernightAccrualMethod.COMPOUNDED;
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
	using OvernightAveragedRateComputation = com.opengamma.strata.product.rate.OvernightAveragedRateComputation;
	using OvernightCompoundedRateComputation = com.opengamma.strata.product.rate.OvernightCompoundedRateComputation;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class OvernightRateCalculationTest
	public class OvernightRateCalculationTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate DATE_01_05 = date(2014, 1, 5);
	  private static readonly LocalDate DATE_01_06 = date(2014, 1, 6);
	  private static readonly LocalDate DATE_02_05 = date(2014, 2, 5);
	  private static readonly LocalDate DATE_03_05 = date(2014, 3, 5);
	  private static readonly LocalDate DATE_04_05 = date(2014, 4, 5);
	  private static readonly LocalDate DATE_04_07 = date(2014, 4, 7);

	  private static readonly SchedulePeriod ACCRUAL1 = SchedulePeriod.of(DATE_01_06, DATE_02_05, DATE_01_05, DATE_02_05);
	  private static readonly SchedulePeriod ACCRUAL2 = SchedulePeriod.of(DATE_02_05, DATE_03_05, DATE_02_05, DATE_03_05);
	  private static readonly SchedulePeriod ACCRUAL3 = SchedulePeriod.of(DATE_03_05, DATE_04_07, DATE_03_05, DATE_04_05);
	  private static readonly Schedule ACCRUAL_SCHEDULE = Schedule.builder().periods(ACCRUAL1, ACCRUAL2, ACCRUAL3).frequency(Frequency.P1M).rollConvention(RollConventions.DAY_5).build();
	  private static readonly Schedule PAYMENT_SCHEDULE = Schedule.builder().periods(SchedulePeriod.of(DATE_01_06, DATE_04_07, DATE_01_05, DATE_04_05)).frequency(Frequency.P3M).rollConvention(RollConventions.DAY_5).build();

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		OvernightRateCalculation test = OvernightRateCalculation.of(GBP_SONIA);
		assertEquals(test.Type, SwapLegType.OVERNIGHT);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.Index, GBP_SONIA);
		assertEquals(test.AccrualMethod, COMPOUNDED);
		assertEquals(test.NegativeRateMethod, ALLOW_NEGATIVE);
		assertEquals(test.RateCutOffDays, 0);
		assertEquals(test.Gearing, null);
		assertEquals(test.Spread, null);
	  }

	  public virtual void test_builder_ensureDefaults()
	  {
		OvernightRateCalculation test = OvernightRateCalculation.builder().index(GBP_SONIA).build();
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.Index, GBP_SONIA);
		assertEquals(test.AccrualMethod, COMPOUNDED);
		assertEquals(test.NegativeRateMethod, ALLOW_NEGATIVE);
		assertEquals(test.RateCutOffDays, 0);
		assertEquals(test.Gearing, null);
		assertEquals(test.Spread, null);
	  }

	  public virtual void test_builder_noIndex()
	  {
		assertThrowsIllegalArg(() => OvernightRateCalculation.builder().build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_collectIndices()
	  {
		OvernightRateCalculation test = OvernightRateCalculation.builder().dayCount(ACT_365F).index(GBP_SONIA).build();
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of(GBP_SONIA));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_expand_simple()
	  {
		OvernightRateCalculation test = OvernightRateCalculation.builder().dayCount(ACT_365F).index(GBP_SONIA).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(ACCRUAL1).yearFraction(ACCRUAL1.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(OvernightCompoundedRateComputation.of(GBP_SONIA, DATE_01_06, DATE_02_05, 0, REF_DATA)).build();
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(ACCRUAL2).yearFraction(ACCRUAL2.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(OvernightCompoundedRateComputation.of(GBP_SONIA, DATE_02_05, DATE_03_05, 0, REF_DATA)).build();
		RateAccrualPeriod rap3 = RateAccrualPeriod.builder(ACCRUAL3).yearFraction(ACCRUAL3.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(OvernightCompoundedRateComputation.of(GBP_SONIA, DATE_03_05, DATE_04_07, 0, REF_DATA)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(ACCRUAL_SCHEDULE, ACCRUAL_SCHEDULE, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2, rap3));
	  }

	  public virtual void test_expand_tomNext()
	  {
		OvernightRateCalculation test = OvernightRateCalculation.builder().dayCount(ACT_360).index(CHF_TOIS).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(ACCRUAL1).yearFraction(ACCRUAL1.yearFraction(ACT_360, ACCRUAL_SCHEDULE)).rateComputation(OvernightCompoundedRateComputation.of(CHF_TOIS, DATE_01_06, DATE_02_05, 0, REF_DATA)).build();
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(ACCRUAL2).yearFraction(ACCRUAL2.yearFraction(ACT_360, ACCRUAL_SCHEDULE)).rateComputation(OvernightCompoundedRateComputation.of(CHF_TOIS, DATE_02_05, DATE_03_05, 0, REF_DATA)).build();
		RateAccrualPeriod rap3 = RateAccrualPeriod.builder(ACCRUAL3).yearFraction(ACCRUAL3.yearFraction(ACT_360, ACCRUAL_SCHEDULE)).rateComputation(OvernightCompoundedRateComputation.of(CHF_TOIS, DATE_03_05, DATE_04_07, 0, REF_DATA)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(ACCRUAL_SCHEDULE, ACCRUAL_SCHEDULE, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2, rap3));
	  }

	  public virtual void test_expand_rateCutOffDays_accrualIsPaymentPeriod()
	  {
		OvernightRateCalculation test = OvernightRateCalculation.builder().dayCount(ACT_365F).index(GBP_SONIA).rateCutOffDays(2).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(ACCRUAL1).yearFraction(ACCRUAL1.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(OvernightCompoundedRateComputation.of(GBP_SONIA, DATE_01_06, DATE_02_05, 2, REF_DATA)).build();
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(ACCRUAL2).yearFraction(ACCRUAL2.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(OvernightCompoundedRateComputation.of(GBP_SONIA, DATE_02_05, DATE_03_05, 2, REF_DATA)).build();
		RateAccrualPeriod rap3 = RateAccrualPeriod.builder(ACCRUAL3).yearFraction(ACCRUAL3.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(OvernightCompoundedRateComputation.of(GBP_SONIA, DATE_03_05, DATE_04_07, 2, REF_DATA)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(ACCRUAL_SCHEDULE, ACCRUAL_SCHEDULE, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2, rap3));
	  }

	  public virtual void test_expand_rateCutOffDays_threeAccrualsInPaymentPeriod()
	  {
		OvernightRateCalculation test = OvernightRateCalculation.builder().dayCount(ACT_365F).index(GBP_SONIA).rateCutOffDays(2).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(ACCRUAL1).yearFraction(ACCRUAL1.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(OvernightCompoundedRateComputation.of(GBP_SONIA, DATE_01_06, DATE_02_05, 0, REF_DATA)).build();
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(ACCRUAL2).yearFraction(ACCRUAL2.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(OvernightCompoundedRateComputation.of(GBP_SONIA, DATE_02_05, DATE_03_05, 0, REF_DATA)).build();
		RateAccrualPeriod rap3 = RateAccrualPeriod.builder(ACCRUAL3).yearFraction(ACCRUAL3.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(OvernightCompoundedRateComputation.of(GBP_SONIA, DATE_03_05, DATE_04_07, 2, REF_DATA)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(ACCRUAL_SCHEDULE, PAYMENT_SCHEDULE, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2, rap3));
	  }

	  public virtual void test_expand_gearingSpreadEverythingElse()
	  {
		OvernightRateCalculation test = OvernightRateCalculation.builder().dayCount(ACT_365F).index(GBP_SONIA).accrualMethod(AVERAGED).negativeRateMethod(NOT_NEGATIVE).rateCutOffDays(2).gearing(ValueSchedule.of(1d, ValueStep.of(2, ValueAdjustment.ofReplace(2d)))).spread(ValueSchedule.of(0d, ValueStep.of(1, ValueAdjustment.ofReplace(-0.025d)))).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(ACCRUAL1).yearFraction(ACCRUAL1.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(OvernightAveragedRateComputation.of(GBP_SONIA, DATE_01_06, DATE_02_05, 0, REF_DATA)).negativeRateMethod(NOT_NEGATIVE).build();
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(ACCRUAL2).yearFraction(ACCRUAL2.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(OvernightAveragedRateComputation.of(GBP_SONIA, DATE_02_05, DATE_03_05, 0, REF_DATA)).negativeRateMethod(NOT_NEGATIVE).spread(-0.025d).build();
		RateAccrualPeriod rap3 = RateAccrualPeriod.builder(ACCRUAL3).yearFraction(ACCRUAL3.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(OvernightAveragedRateComputation.of(GBP_SONIA, DATE_03_05, DATE_04_07, 2, REF_DATA)).negativeRateMethod(NOT_NEGATIVE).gearing(2d).spread(-0.025d).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(ACCRUAL_SCHEDULE, PAYMENT_SCHEDULE, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2, rap3));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		OvernightRateCalculation test = OvernightRateCalculation.builder().dayCount(ACT_365F).index(GBP_SONIA).build();
		coverImmutableBean(test);
		OvernightRateCalculation test2 = OvernightRateCalculation.builder().dayCount(ACT_360).index(USD_FED_FUND).accrualMethod(AVERAGED).negativeRateMethod(NOT_NEGATIVE).rateCutOffDays(2).gearing(ValueSchedule.of(2d)).spread(ValueSchedule.of(-0.025d)).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		OvernightRateCalculation test = OvernightRateCalculation.builder().dayCount(ACT_365F).index(GBP_SONIA).build();
		assertSerialization(test);
	  }

	}

}