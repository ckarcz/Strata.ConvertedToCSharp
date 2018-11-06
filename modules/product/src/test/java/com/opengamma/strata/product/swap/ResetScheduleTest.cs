/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.DAY_5;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.IborRateResetMethod.UNWEIGHTED;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.IborRateResetMethod.WEIGHTED;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using Schedule = com.opengamma.strata.basics.schedule.Schedule;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResetScheduleTest
	public class ResetScheduleTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate DATE_01_05 = date(2014, 1, 5);
	  private static readonly LocalDate DATE_01_06 = date(2014, 1, 6);
	  private static readonly LocalDate DATE_02_05 = date(2014, 2, 5);
	  private static readonly LocalDate DATE_03_05 = date(2014, 3, 5);
	  private static readonly LocalDate DATE_04_05 = date(2014, 4, 5);
	  private static readonly LocalDate DATE_04_07 = date(2014, 4, 7);

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_ensureDefaults()
	  {
		ResetSchedule test = ResetSchedule.builder().resetFrequency(P1M).businessDayAdjustment(BusinessDayAdjustment.of(FOLLOWING, GBLO)).build();
		assertEquals(test.ResetFrequency, P1M);
		assertEquals(test.BusinessDayAdjustment, BusinessDayAdjustment.of(FOLLOWING, GBLO));
		assertEquals(test.ResetMethod, UNWEIGHTED);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		ResetSchedule test = ResetSchedule.builder().resetFrequency(P1M).businessDayAdjustment(BusinessDayAdjustment.of(FOLLOWING, GBLO)).build();
		SchedulePeriod accrualPeriod = SchedulePeriod.of(DATE_01_06, DATE_04_07, DATE_01_05, DATE_04_05);
		Schedule schedule = test.createSchedule(DAY_5, REF_DATA).apply(accrualPeriod);
		Schedule expected = Schedule.builder().periods(SchedulePeriod.of(DATE_01_06, DATE_02_05, DATE_01_05, DATE_02_05), SchedulePeriod.of(DATE_02_05, DATE_03_05, DATE_02_05, DATE_03_05), SchedulePeriod.of(DATE_03_05, DATE_04_07, DATE_03_05, DATE_04_05)).frequency(P1M).rollConvention(DAY_5).build();
		assertEquals(schedule, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ResetSchedule test = ResetSchedule.builder().resetFrequency(P1M).businessDayAdjustment(BusinessDayAdjustment.of(FOLLOWING, GBLO)).build();
		coverImmutableBean(test);
		ResetSchedule test2 = ResetSchedule.builder().resetFrequency(P3M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO)).resetMethod(WEIGHTED).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		ResetSchedule test = ResetSchedule.builder().resetFrequency(P1M).businessDayAdjustment(BusinessDayAdjustment.of(FOLLOWING, GBLO)).build();
		assertSerialization(test);
	  }

	}

}