/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.SAT_SUN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P2M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.TERM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.DAY_5;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.CompoundingMethod.NONE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.CompoundingMethod.STRAIGHT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.PaymentRelativeTo.PERIOD_END;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.PaymentRelativeTo.PERIOD_START;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using Schedule = com.opengamma.strata.basics.schedule.Schedule;
	using ScheduleException = com.opengamma.strata.basics.schedule.ScheduleException;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PaymentScheduleTest
	public class PaymentScheduleTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate DATE_01_05 = date(2014, 1, 5);
	  private static readonly LocalDate DATE_01_06 = date(2014, 1, 6);
	  private static readonly LocalDate DATE_01_08 = date(2014, 1, 8);
	  private static readonly LocalDate DATE_02_05 = date(2014, 2, 5);
	  private static readonly LocalDate DATE_03_05 = date(2014, 3, 5);
	  private static readonly LocalDate DATE_04_04 = date(2014, 4, 4);
	  private static readonly LocalDate DATE_04_05 = date(2014, 4, 5);
	  private static readonly LocalDate DATE_04_07 = date(2014, 4, 7);
	  private static readonly LocalDate DATE_04_30 = date(2014, 4, 30);
	  private static readonly LocalDate DATE_05_05 = date(2014, 5, 5);
	  private static readonly LocalDate DATE_05_06 = date(2014, 5, 6);
	  private static readonly BusinessDayAdjustment BDA = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, SAT_SUN);

	  private static readonly SchedulePeriod ACCRUAL1STUB = SchedulePeriod.of(DATE_01_08, DATE_02_05, DATE_01_08, DATE_02_05);
	  private static readonly SchedulePeriod ACCRUAL1 = SchedulePeriod.of(DATE_01_06, DATE_02_05, DATE_01_05, DATE_02_05);
	  private static readonly SchedulePeriod ACCRUAL2 = SchedulePeriod.of(DATE_02_05, DATE_03_05, DATE_02_05, DATE_03_05);
	  private static readonly SchedulePeriod ACCRUAL3 = SchedulePeriod.of(DATE_03_05, DATE_04_07, DATE_03_05, DATE_04_05);
	  private static readonly SchedulePeriod ACCRUAL4 = SchedulePeriod.of(DATE_04_07, DATE_05_06, DATE_04_05, DATE_05_05);
	  private static readonly SchedulePeriod ACCRUAL3STUB = SchedulePeriod.of(DATE_03_05, DATE_04_04, DATE_03_05, DATE_04_04);
	  private static readonly SchedulePeriod ACCRUAL4STUB = SchedulePeriod.of(DATE_04_07, DATE_04_30, DATE_04_05, DATE_04_30);
	  private static readonly Schedule ACCRUAL_SCHEDULE_SINGLE = Schedule.builder().periods(ACCRUAL1).frequency(P1M).rollConvention(RollConventions.DAY_5).build();
	  private static readonly Schedule ACCRUAL_SCHEDULE_TERM = Schedule.builder().periods(SchedulePeriod.of(DATE_01_06, DATE_04_07, DATE_01_05, DATE_04_05)).frequency(TERM).rollConvention(RollConventions.NONE).build();
	  private static readonly Schedule ACCRUAL_SCHEDULE = Schedule.builder().periods(ACCRUAL1, ACCRUAL2, ACCRUAL3).frequency(P1M).rollConvention(DAY_5).build();
	  private static readonly Schedule ACCRUAL_SCHEDULE_STUBS = Schedule.builder().periods(ACCRUAL1STUB, ACCRUAL2, ACCRUAL3STUB).frequency(P1M).rollConvention(DAY_5).build();
	  private static readonly Schedule ACCRUAL_SCHEDULE_INITIAL_STUB = Schedule.builder().periods(ACCRUAL1STUB, ACCRUAL2, ACCRUAL3, ACCRUAL4).frequency(P1M).rollConvention(DAY_5).build();
	  private static readonly Schedule ACCRUAL_SCHEDULE_FINAL_STUB = Schedule.builder().periods(ACCRUAL1, ACCRUAL2, ACCRUAL3STUB).frequency(P1M).rollConvention(DAY_5).build();
	  private static readonly Schedule ACCRUAL_SCHEDULE_FINAL_STUB_4PERIODS = Schedule.builder().periods(ACCRUAL1, ACCRUAL2, ACCRUAL3, ACCRUAL4STUB).frequency(P1M).rollConvention(DAY_5).build();

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_ensureDefaults()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(P1M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).build();
		assertEquals(test.PaymentFrequency, P1M);
		assertEquals(test.BusinessDayAdjustment, null);
		assertEquals(test.PaymentDateOffset, DaysAdjustment.ofBusinessDays(2, GBLO));
		assertEquals(test.PaymentRelativeTo, PERIOD_END);
		assertEquals(test.CompoundingMethod, NONE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createSchedule_sameFrequency()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(P1M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).build();
		Schedule schedule = test.createSchedule(ACCRUAL_SCHEDULE, REF_DATA);
		assertEquals(schedule, ACCRUAL_SCHEDULE);
	  }

	  public virtual void test_createSchedule_singleAccrualPeriod()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(P1M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).build();
		Schedule schedule = test.createSchedule(ACCRUAL_SCHEDULE_SINGLE, REF_DATA);
		assertEquals(schedule, ACCRUAL_SCHEDULE_SINGLE);
	  }

	  public virtual void test_createSchedule_term()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(TERM).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).build();
		Schedule schedule = test.createSchedule(ACCRUAL_SCHEDULE, REF_DATA);
		assertEquals(schedule, ACCRUAL_SCHEDULE_TERM);
	  }

	  public virtual void test_createSchedule_term_badFirstRegular()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(TERM).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).firstRegularStartDate(DATE_05_05).build();
		assertThrowsIllegalArg(() => test.createSchedule(ACCRUAL_SCHEDULE, REF_DATA));
	  }

	  public virtual void test_createSchedule_term_badLastRegular()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(TERM).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).lastRegularEndDate(DATE_05_05).build();
		assertThrowsIllegalArg(() => test.createSchedule(ACCRUAL_SCHEDULE, REF_DATA));
	  }

	  public virtual void test_createSchedule_fullMerge()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(P3M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).build();
		Schedule schedule = test.createSchedule(ACCRUAL_SCHEDULE, REF_DATA);
		Schedule expected = Schedule.builder().periods(SchedulePeriod.of(DATE_01_06, DATE_04_07, DATE_01_05, DATE_04_05)).frequency(P3M).rollConvention(DAY_5).build();
		assertEquals(schedule, expected);
	  }

	  public virtual void test_createSchedule_partMergeForwards()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(P2M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).build();
		Schedule schedule = test.createSchedule(ACCRUAL_SCHEDULE, REF_DATA);
		Schedule expected = Schedule.builder().periods(SchedulePeriod.of(DATE_01_06, DATE_03_05, DATE_01_05, DATE_03_05), SchedulePeriod.of(DATE_03_05, DATE_04_07, DATE_03_05, DATE_04_05)).frequency(P2M).rollConvention(DAY_5).build();
		assertEquals(schedule, expected);
	  }

	  public virtual void test_createSchedule_initialStubPartMergeBackwards()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(P2M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).build();
		Schedule schedule = test.createSchedule(ACCRUAL_SCHEDULE_INITIAL_STUB, REF_DATA);
		Schedule expected = Schedule.builder().periods(ACCRUAL1STUB, SchedulePeriod.of(DATE_02_05, DATE_03_05, DATE_02_05, DATE_03_05), SchedulePeriod.of(DATE_03_05, DATE_05_06, DATE_03_05, DATE_05_05)).frequency(P2M).rollConvention(DAY_5).build();
		assertEquals(schedule, expected);
	  }

	  public virtual void test_createSchedule_finalStubFullMerge()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(P2M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).build();
		Schedule schedule = test.createSchedule(ACCRUAL_SCHEDULE_FINAL_STUB, REF_DATA);
		Schedule expected = Schedule.builder().periods(SchedulePeriod.of(DATE_01_06, DATE_03_05, DATE_01_05, DATE_03_05), ACCRUAL3STUB).frequency(P2M).rollConvention(DAY_5).build();
		assertEquals(schedule, expected);
	  }

	  public virtual void test_createSchedule_dualStub()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(P2M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).build();
		Schedule schedule = test.createSchedule(ACCRUAL_SCHEDULE_STUBS, REF_DATA);
		assertEquals(schedule, ACCRUAL_SCHEDULE_STUBS.toBuilder().frequency(P2M).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createSchedule_firstAndLastDate()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(P3M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).firstRegularStartDate(DATE_01_06).lastRegularEndDate(DATE_04_07).build();
		Schedule schedule = test.createSchedule(ACCRUAL_SCHEDULE, REF_DATA);
		Schedule expected = Schedule.builder().periods(SchedulePeriod.of(DATE_01_06, DATE_04_07, DATE_01_05, DATE_04_05)).frequency(P3M).rollConvention(DAY_5).build();
		assertEquals(schedule, expected);
	  }

	  public virtual void test_createSchedule_firstAndLastDate_validInitialStub()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(P2M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).firstRegularStartDate(DATE_02_05).lastRegularEndDate(DATE_04_07).build();
		Schedule schedule = test.createSchedule(ACCRUAL_SCHEDULE, REF_DATA);
		Schedule expected = Schedule.builder().periods(SchedulePeriod.of(DATE_01_06, DATE_02_05, DATE_01_05, DATE_02_05), SchedulePeriod.of(DATE_02_05, DATE_04_07, DATE_02_05, DATE_04_05)).frequency(P2M).rollConvention(DAY_5).build();
		assertEquals(schedule, expected);
	  }

	  public virtual void test_createSchedule_firstAndLastDate_invalidInitialStub()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(P2M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).firstRegularStartDate(DATE_01_06).lastRegularEndDate(DATE_04_07).build();
		assertThrows(() => test.createSchedule(ACCRUAL_SCHEDULE, REF_DATA), typeof(ScheduleException));
	  }

	  public virtual void test_createSchedule_firstAndLastDate_initialAccrualStub()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(P2M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).firstRegularStartDate(DATE_03_05).lastRegularEndDate(DATE_05_05).build();
		Schedule schedule = test.createSchedule(ACCRUAL_SCHEDULE_INITIAL_STUB, REF_DATA);
		Schedule expected = Schedule.builder().periods(SchedulePeriod.of(DATE_01_08, DATE_03_05, DATE_01_08, DATE_03_05), SchedulePeriod.of(DATE_03_05, DATE_05_06, DATE_03_05, DATE_05_05)).frequency(P2M).rollConvention(DAY_5).build();
		assertEquals(schedule, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createSchedule_firstDate()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(P3M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).firstRegularStartDate(DATE_01_06).build();
		Schedule schedule = test.createSchedule(ACCRUAL_SCHEDULE, REF_DATA);
		Schedule expected = Schedule.builder().periods(SchedulePeriod.of(DATE_01_06, DATE_04_07, DATE_01_05, DATE_04_05)).frequency(P3M).rollConvention(DAY_5).build();
		assertEquals(schedule, expected);
	  }

	  public virtual void test_createSchedule_firstDate_validInitialStub()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(P2M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).firstRegularStartDate(DATE_02_05).build();
		Schedule schedule = test.createSchedule(ACCRUAL_SCHEDULE, REF_DATA);
		Schedule expected = Schedule.builder().periods(SchedulePeriod.of(DATE_01_06, DATE_02_05, DATE_01_05, DATE_02_05), SchedulePeriod.of(DATE_02_05, DATE_04_07, DATE_02_05, DATE_04_05)).frequency(P2M).rollConvention(DAY_5).build();
		assertEquals(schedule, expected);
	  }

	  public virtual void test_createSchedule_firstDate_invalidInitialStub()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(P2M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).firstRegularStartDate(DATE_01_06).build();
		assertThrows(() => test.createSchedule(ACCRUAL_SCHEDULE, REF_DATA), typeof(ScheduleException));
	  }

	  public virtual void test_createSchedule_firstDate_initialAccrualStub()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(P2M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).firstRegularStartDate(DATE_03_05).build();
		Schedule schedule = test.createSchedule(ACCRUAL_SCHEDULE_INITIAL_STUB, REF_DATA);
		Schedule expected = Schedule.builder().periods(SchedulePeriod.of(DATE_01_08, DATE_03_05, DATE_01_08, DATE_03_05), SchedulePeriod.of(DATE_03_05, DATE_05_06, DATE_03_05, DATE_05_05)).frequency(P2M).rollConvention(DAY_5).build();
		assertEquals(schedule, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createSchedule_lastDate()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(P3M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).lastRegularEndDate(DATE_04_05).build();
		Schedule schedule = test.createSchedule(ACCRUAL_SCHEDULE, REF_DATA);
		Schedule expected = Schedule.builder().periods(SchedulePeriod.of(DATE_01_06, DATE_04_07, DATE_01_05, DATE_04_05)).frequency(P3M).rollConvention(DAY_5).build();
		assertEquals(schedule, expected);
	  }

	  public virtual void test_createSchedule_lastDate_validFinalStub()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(P2M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).lastRegularEndDate(DATE_03_05).build();
		Schedule schedule = test.createSchedule(ACCRUAL_SCHEDULE, REF_DATA);
		Schedule expected = Schedule.builder().periods(SchedulePeriod.of(DATE_01_06, DATE_03_05, DATE_01_05, DATE_03_05), SchedulePeriod.of(DATE_03_05, DATE_04_07, DATE_03_05, DATE_04_05)).frequency(P2M).rollConvention(DAY_5).build();
		assertEquals(schedule, expected);
	  }

	  public virtual void test_createSchedule_lastDate_invalidFinalStub()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(P2M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).lastRegularEndDate(DATE_04_05).build();
		assertThrows(() => test.createSchedule(ACCRUAL_SCHEDULE, REF_DATA), typeof(ScheduleException));
	  }

	  public virtual void test_createSchedule_lastDate_finalAccrualStub()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(P2M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).lastRegularEndDate(DATE_03_05).build();
		Schedule schedule = test.createSchedule(ACCRUAL_SCHEDULE_FINAL_STUB_4PERIODS, REF_DATA);
		Schedule expected = Schedule.builder().periods(SchedulePeriod.of(DATE_01_06, DATE_03_05, DATE_01_05, DATE_03_05), SchedulePeriod.of(DATE_03_05, DATE_04_30, DATE_03_05, DATE_04_30)).frequency(P2M).rollConvention(DAY_5).build();
		assertEquals(schedule, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(P1M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).build();
		coverImmutableBean(test);
		PaymentSchedule test2 = PaymentSchedule.builder().paymentFrequency(P3M).businessDayAdjustment(BDA).paymentDateOffset(DaysAdjustment.ofBusinessDays(3, GBLO)).paymentRelativeTo(PERIOD_START).compoundingMethod(STRAIGHT).firstRegularStartDate(DATE_01_06).lastRegularEndDate(DATE_02_05).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		PaymentSchedule test = PaymentSchedule.builder().paymentFrequency(P3M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).build();
		assertSerialization(test);
	  }

	}

}